#include "RunToLineCmd.hpp"

#include "DebugManager/DebugManager.hpp"

#include <lua.hpp>

#include <iostream>
#include <limits>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	RunToLineCmd::RunToLineCmd() :
		RpcCommandBase(eRPC_COMMANDTYPE_RUNTO),
        mFileName(""),
		mRunToLine(std::numeric_limits<int>::max())
	{
	}

	/////////////////////////////////////////////////////////////////////
	RunToLineCmd::~RunToLineCmd()
	{
	}

	/////////////////////////////////////////////////////////////////////
	void RunToLineCmd::DoCommand()
	{
		RunToLine rtl;
        std::transform(mFileName.begin(), mFileName.end(), mFileName.begin(), tolower);
        rtl.mFileName = mFileName;
        rtl.mLine = mRunToLine;
        DebugManager::GetInstancePtr()->SetRunToLine(rtl);
        DebugManager::GetInstancePtr()->SetNextDebugAction(DebugAction::eDEBUG_ACTION_RUN);
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase* RunToLineCmd::MakeNew()
	{
		return new RunToLineCmd();
	}

	/////////////////////////////////////////////////////////////////////
	int RunToLineCmd::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetStringDataSize(mFileName);
		totalSize += GetIntegerDataSize();
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void RunToLineCmd::DeserializeData(const char* data, int &offset)
	{
		DeserializeString(data, offset, mFileName);
		DeserializeInteger(data, offset, mRunToLine);
	}

	/////////////////////////////////////////////////////////////////////
	void RunToLineCmd::SerializeData(char* data, int &offset) const
	{
		SerializeString(data, offset, mFileName);
		SerializeInteger(data, offset, mRunToLine);
	}
}