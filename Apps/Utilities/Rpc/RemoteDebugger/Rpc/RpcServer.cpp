#include "RpcServer.hpp"

#include "Debugging/DebugManager/DebugManager.hpp"

#include <winsock2.h>

namespace rpc
{
	/////////////////////////////////////////////////////////////////////
	RpcServer::RpcServer() :
		mbConnected(false),
		mbShouldRun(false),
		mpSocket(NULL)
	{
		mhMutex = CreateMutex(NULL, FALSE, (LPCWSTR)"RpcServerMutex");
		WSAData wsaData;
		int nCode;

		if ((nCode = WSAStartup(MAKEWORD(1, 1), &wsaData)) != 0)
		{
			std::cerr << "WSAStartup() returned error code " << nCode << "!" << std::endl;
		}

		// Create socket for the first time
		mpSocket.reset(new network::TcpSocket);

		// Register debug commands to rpc command manager
		debugging::RpcCommandManager* rpcCmdMgr = debugging::RpcCommandManager::GetInstancePtr();
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_RUNSCRIPT, &mRunScriptInVMCmd);
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_UPDATEBREAKPOINTS, &mUpdateBreakpointsCmd);
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_BREAKATLINE, &mBreakAtLineCmd);
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_UNBREAKATLINE, &mUnBreakAtLineCmd);
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_DEBUGACTION, &mDebugActionCmd);
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_RUNTIMEERROR, &mLuaErrorCmd);
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_UPDATETABLEDETAILS, &mUpdateTableDetailsCmd);
		rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_LUAOUTPUT, &mLuaOutputCmd);
        rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_SETLUAVARIABLE, &mSetLuaVariable);
        rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_UPDATEDEBUGINFO, &mUpdateDebugInfo);
        rpcCmdMgr->RegisterCommandByType(debugging::eRPC_COMMANDTYPE_RUNTO, &mRunToLineCmd);
	}

	/////////////////////////////////////////////////////////////////////
	RpcServer::~RpcServer()
	{
		ReleaseMutex(mhMutex);
		CloseHandle(mhMutex);
		mhMutex = NULL;
		WSACleanup();

        // Unregister debug commands from rpc command manager
        debugging::RpcCommandManager* rpcCmdMgr = debugging::RpcCommandManager::GetInstancePtr();
		rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_RUNSCRIPT);
		rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_UPDATEBREAKPOINTS);
		rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_BREAKATLINE);
		rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_UNBREAKATLINE);
		rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_DEBUGACTION);
		rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_RUNTIMEERROR);
		rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_UPDATETABLEDETAILS);
        rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_LUAOUTPUT);
        rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_SETLUAVARIABLE);
        rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_UPDATEDEBUGINFO);
        rpcCmdMgr->UnRegisterCommandByType(debugging::eRPC_COMMANDTYPE_RUNTO);
	}

	/////////////////////////////////////////////////////////////////////
	void RpcServer::Run(int aPort)
	{
        miPort = aPort;
		mbShouldRun = true;
		mpSocket->Listen(miPort);

		while (mbShouldRun)
		{
			TryConnectClient();

			if (mbConnected && mbShouldRun)
			{
				while (mbConnected && mbShouldRun)
				{
					ReceiveMessageInternal();
					SendMessageInternal();
					Sleep(20);
				}

                debugging::DebugManager::GetInstancePtr()->SetNextDebugAction(debugging::DebugAction::eDEBUG_ACTION_STOP);
                debugging::DebugManager::GetInstancePtr()->UninitializeDebugData();
				mpSocket.reset(new network::TcpSocket);
				mpSocket->Listen(miPort);
			}

            Sleep(20);
		}
	}

	/////////////////////////////////////////////////////////////////////
	void RpcServer::Stop()
	{
		mbShouldRun = false;
	}

	/////////////////////////////////////////////////////////////////////
	void RpcServer::Disconnect()
	{
		mbConnected = false;
	}

	/////////////////////////////////////////////////////////////////////
	void RpcServer::ReceiveMessageInternal()
	{
		int type = mpSocket->ReceiveInt();
		int dataSize = mpSocket->ReceiveInt();
		boost::scoped_ptr<char> data(new char[dataSize]);
		mpSocket->ReceiveBuffer(data.get(), dataSize);

		if (!mpSocket->IsConnected())
		{
			mbConnected = false;
			return;
		}

		if (mOnMessageReceived.GetHandlersCount() > 0)
		{
			mOnMessageReceived(type, data.get());
		}

		if (type != debugging::eRPC_COMMANDTYPE_INVALID)
		{
			boost::scoped_ptr<debugging::RpcCommandBase> cmd(debugging::RpcCommandManager::GetInstancePtr()->CreateCommandByType(static_cast<debugging::RpcCommandType>(type)));
			cmd->DeserializeCommand(data.get());
			cmd->DoCommand();
		}
	}

	/////////////////////////////////////////////////////////////////////
	void RpcServer::SendMessageInternal()
	{
		WaitForSingleObject(mhMutex, INFINITE);
		static const int MaxPerFrame = 16;
		int sendCount = 0;

		while (mMsgsBuffer.begin() != mMsgsBuffer.end() && sendCount < MaxPerFrame)
		{
			debugging::RpcCommandBase cmd = *mMsgsBuffer.begin();

			int cmdHeaderSize = sizeof(int) * 2 + cmd.GetRawDataSize();
			boost::scoped_ptr<char> cmdHeaderData(new char[cmdHeaderSize]);
			int offset = 0;

			// Command type
			int cmdType = (int)cmd.GetRpcCommandType();
			memcpy(&cmdHeaderData.get()[offset], &cmdType, sizeof(int));
			offset += sizeof(int);

			// Raw data size
			int cmdRawDataSize = cmd.GetRawDataSize();
			memcpy(&cmdHeaderData.get()[offset], &cmdRawDataSize, sizeof(int));
			offset += sizeof(int);

			// Raw data
			memcpy(&cmdHeaderData.get()[offset], cmd.GetRawData(), cmdRawDataSize);
			offset += cmdRawDataSize;

			// Send the command header
			mpSocket.get()->SendBuffer(cmdHeaderData.get(), cmdHeaderSize);
			mMsgsBuffer.pop_front();
			++sendCount;
		}

		ReleaseMutex(mhMutex);
	}

	/////////////////////////////////////////////////////////////////////
	void RpcServer::SendMessage(debugging::RpcCommandBase* cmd)
	{
		WaitForSingleObject(mhMutex, INFINITE);
		cmd->SerializeCommand();
		mMsgsBuffer.push_back(*cmd);
		ReleaseMutex(mhMutex);
	}

	/////////////////////////////////////////////////////////////////////
	void RpcServer::TryConnectClient()
	{
		try
		{
			if (mpSocket->Accept() == 0)
			{
				mbConnected = true;
				//std::cout << "Client connected on port " << miPort << std::endl;
			}
		}
		catch (...)
		{
		}
	}
}