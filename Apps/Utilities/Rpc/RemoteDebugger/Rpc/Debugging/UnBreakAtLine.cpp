#include "UnBreakAtLine.hpp"

#include "DebugManager/DebugManager.hpp"

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	UnBreakAtLine::UnBreakAtLine() :
		RpcCommandBase(eRPC_COMMANDTYPE_UNBREAKATLINE)
	{
	}

	/////////////////////////////////////////////////////////////////////
	UnBreakAtLine::UnBreakAtLine(std::string aFileName, int aLine) :
		RpcCommandBase(eRPC_COMMANDTYPE_UNBREAKATLINE),
		mFileName(aFileName),
		mLine(aLine)
	{
	}

	/////////////////////////////////////////////////////////////////////
	UnBreakAtLine::~UnBreakAtLine()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int UnBreakAtLine::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetStringDataSize(mFileName);
		totalSize += GetIntegerDataSize();
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void UnBreakAtLine::DeserializeData(const char* data, int &offset)
	{
		DeserializeString(data, offset, mFileName);
		DeserializeInteger(data, offset, mLine);
	}

	/////////////////////////////////////////////////////////////////////
	void UnBreakAtLine::SerializeData(char* data, int &offset) const
	{
		SerializeString(data, offset, mFileName);
		SerializeInteger(data, offset, mLine);
	}
}