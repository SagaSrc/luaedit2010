#include "LuaOutput.hpp"

#include "DebugManager/DebugManager.hpp"

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	LuaOutput::LuaOutput() :
		RpcCommandBase(eRPC_COMMANDTYPE_LUAOUTPUT)
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaOutput::LuaOutput(const char* aOutput, OutputType aOutputType) :
		RpcCommandBase(eRPC_COMMANDTYPE_LUAOUTPUT),
		mOutputType((int)aOutputType),
		mOutput(aOutput)
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaOutput::~LuaOutput()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int LuaOutput::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetIntegerDataSize();
		totalSize += GetStringDataSize(mOutput);
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void LuaOutput::DeserializeData(const char* data, int &offset)
	{
		DeserializeInteger(data, offset, mOutputType);
		DeserializeString(data, offset, mOutput);
	}

	/////////////////////////////////////////////////////////////////////
	void LuaOutput::SerializeData(char* data, int &offset) const
	{
		SerializeInteger(data, offset, mOutputType);
		SerializeString(data, offset, mOutput);
	}
}