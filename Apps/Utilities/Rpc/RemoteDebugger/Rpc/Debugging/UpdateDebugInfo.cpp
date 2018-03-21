#include "UpdateDebugInfo.hpp"

#include "DebugManager/DebugManager.hpp"

#include <lua.hpp>

#include <iostream>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	UpdateDebugInfo::UpdateDebugInfo() :
		RpcCommandBase(eRPC_COMMANDTYPE_UPDATEDEBUGINFO)
	{
	}

	/////////////////////////////////////////////////////////////////////
	UpdateDebugInfo::~UpdateDebugInfo()
	{
	}

	/////////////////////////////////////////////////////////////////////
	void UpdateDebugInfo::DoCommand()
	{
		DebugManager::GetInstancePtr()->SetDebugInfo(mDI);
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase* UpdateDebugInfo::MakeNew()
	{
		return new UpdateDebugInfo();
	}

	/////////////////////////////////////////////////////////////////////
	int UpdateDebugInfo::GetSerializedDataSize() const
	{
		int totalSize = 0;
        totalSize += GetComplexDataSize(&mDI);
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void UpdateDebugInfo::DeserializeData(const char* data, int &offset)
	{
		DeserializeComplexData(data, offset, &mDI);
	}

	/////////////////////////////////////////////////////////////////////
	void UpdateDebugInfo::SerializeData(char* data, int &offset) const
	{
		SerializeComplexData(data, offset, &mDI);
	}
}