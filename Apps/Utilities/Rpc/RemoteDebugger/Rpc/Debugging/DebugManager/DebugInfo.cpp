#include "DebugInfo.hpp"

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
    DebugInfo::DebugInfo() :
        mStartAction((int)DebugStartAction::StartAttach),
        mRemotePath("")
	{
	}

    /////////////////////////////////////////////////////////////////////
    DebugInfo::DebugInfo(DebugInfo &aCopy) :
        mStartAction(aCopy.mStartAction),
        mRemotePath(aCopy.mRemotePath)
    {
    }

	/////////////////////////////////////////////////////////////////////
	int DebugInfo::GetSerializedDataSize() const
	{
		int totalSize = 0;
        totalSize += RpcCommandBase::GetIntegerDataSize();
        totalSize += RpcCommandBase::GetStringDataSize(mRemotePath);
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void DebugInfo::DeserializeData(const char* data, int &offset)
	{
        RpcCommandBase::DeserializeInteger(data, offset, mStartAction);
        RpcCommandBase::DeserializeString(data, offset, mRemotePath);
	}

	/////////////////////////////////////////////////////////////////////
	void DebugInfo::SerializeData(char* data, int &offset) const
	{
        RpcCommandBase::SerializeInteger(data, offset, mStartAction);
        RpcCommandBase::SerializeString(data, offset, mRemotePath);
	}

    /////////////////////////////////////////////////////////////////////
	DebugInfo& DebugInfo::operator=(const DebugInfo &aSource)
	{
		if (this != &aSource)
		{
			mStartAction = aSource.mStartAction;
		    mRemotePath = aSource.mRemotePath;
		}

		return *this;
	}
}