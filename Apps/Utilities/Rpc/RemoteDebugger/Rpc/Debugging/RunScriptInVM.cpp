#include "RunScriptInVM.hpp"

#include "DebugManager/DebugManager.hpp"

#include <lua.hpp>

#include <iostream>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	RunScriptInVM::RunScriptInVM() :
		RpcCommandBase(eRPC_COMMANDTYPE_RUNSCRIPT),
        mScriptFileName(""),
		mRunToLine(std::numeric_limits<int>::max())
	{
	}

	/////////////////////////////////////////////////////////////////////
	RunScriptInVM::~RunScriptInVM()
	{
	}

	/////////////////////////////////////////////////////////////////////
	void RunScriptInVM::DoCommand()
	{
		RunToLine rtl;
        std::transform(mScriptFileName.begin(), mScriptFileName.end(), mScriptFileName.begin(), tolower);
        rtl.mFileName = mScriptFileName;
        rtl.mLine = mRunToLine;
        DebugManager::GetInstancePtr()->SetRunToLine(rtl);
		DebugManager::GetInstancePtr()->LoadScriptInVM(GetScriptFileName(), true);
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase* RunScriptInVM::MakeNew()
	{
		return new RunScriptInVM();
	}

	/////////////////////////////////////////////////////////////////////
	int RunScriptInVM::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetStringDataSize(mScriptFileName);
		totalSize += GetIntegerDataSize();
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void RunScriptInVM::DeserializeData(const char* data, int &offset)
	{
		DeserializeString(data, offset, mScriptFileName);
		DeserializeInteger(data, offset, mRunToLine);
	}

	/////////////////////////////////////////////////////////////////////
	void RunScriptInVM::SerializeData(char* data, int &offset) const
	{
		SerializeString(data, offset, mScriptFileName);
		SerializeInteger(data, offset, mRunToLine);
	}
}