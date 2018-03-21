#ifndef RPC_RPCSERVER_HPP
#define RPC_RPCSERVER_HPP

#include <boost/scoped_ptr.hpp>

#include <Rpc/RemoteDebugger/Events/Events.hpp>
#include <Rpc/RemoteDebugger/Network/TcpSocket.hpp>

#include <Rpc/RemoteDebugger/Rpc/Debugging/RpcCommand.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/RunScriptInVM.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/UpdateBreakpoints.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/BreakAtLine.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/UnBreakAtLine.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/DebugActionCmd.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/LuaError.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/UpdateTableDetails.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/LuaOutput.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/RunToLineCmd.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/SetLuaVariable.hpp>
#include <Rpc/RemoteDebugger/Rpc/Debugging/UpdateDebugInfo.hpp>

#include <deque>

namespace rpc
{
	class RpcServer
	{
	public:
		RpcServer();
		~RpcServer();

		void Run(int aPort);
		void Stop();
		void Disconnect();

		void SendMessage(debugging::RpcCommandBase* cmd);

		events::Event<int, const char*> mOnMessageReceived;

	private:
		void ReceiveMessageInternal();
		void SendMessageInternal();
		void TryConnectClient();

		boost::scoped_ptr<network::TcpSocket> mpSocket;
		volatile bool mbConnected;
		volatile bool mbShouldRun;
		int miPort;
		HANDLE mhMutex;
		std::deque<debugging::RpcCommandBase> mMsgsBuffer;

		debugging::RunScriptInVM mRunScriptInVMCmd;
		debugging::UpdateBreakpoints mUpdateBreakpointsCmd;
		debugging::BreakAtLine mBreakAtLineCmd;
		debugging::UnBreakAtLine mUnBreakAtLineCmd;
		debugging::DebugActionCmd mDebugActionCmd;
		debugging::LuaError mLuaErrorCmd;
		debugging::UpdateTableDetails mUpdateTableDetailsCmd;
		debugging::LuaOutput mLuaOutputCmd;
        debugging::SetLuaVariable mSetLuaVariable;
        debugging::UpdateDebugInfo mUpdateDebugInfo;
        debugging::RunToLineCmd mRunToLineCmd;
	};
}

#endif //RPC_RPCSERVER_HPP