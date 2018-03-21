#include "DebugActionCmd.hpp"

#include "DebugManager/DebugManager.hpp"

#include <iostream>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	DebugActionCmd::DebugActionCmd() :
		RpcCommandBase(eRPC_COMMANDTYPE_DEBUGACTION)
	{
	}

	/////////////////////////////////////////////////////////////////////
	DebugActionCmd::~DebugActionCmd()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int DebugActionCmd::GetSerializedDataSize() const
	{
		return GetIntegerDataSize();
	}

	/////////////////////////////////////////////////////////////////////
	void DebugActionCmd::DeserializeData(const char* data, int &offset)
	{
		DeserializeInteger(data, offset, mDebugAction);
	}

	/////////////////////////////////////////////////////////////////////
	void DebugActionCmd::SerializeData(char* data, int &offset) const
	{
		SerializeInteger(data, offset, mDebugAction);
	}

	/////////////////////////////////////////////////////////////////////
	void DebugActionCmd::DoCommand()
	{
        // Make sure the debug manager is expecting a debug action
        if (DebugManager::GetInstancePtr()->GetCurrentState() == eDEBUGMANAGER_WAITINGDEBUGACTION)
        {
		    DebugManager::GetInstancePtr()->SetNextDebugAction(GetDebugAction());
        }
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase* DebugActionCmd::MakeNew()
	{
		return new DebugActionCmd();
	}
}