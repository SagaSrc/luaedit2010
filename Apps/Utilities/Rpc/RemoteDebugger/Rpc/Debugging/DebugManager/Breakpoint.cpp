#include "Breakpoint.hpp"

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	Breakpoint::Breakpoint() :
		mFileName(""),
		mLine(-1),
		mCondition(""),
		mIsEnabled(false),
		mHitCount(0),
        mCurrentHitCount(0)
	{
	}

	/////////////////////////////////////////////////////////////////////
	Breakpoint::Breakpoint(const Breakpoint &aCopy) :
		mFileName(aCopy.mFileName),
		mLine(aCopy.mLine),
		mCondition(aCopy.mCondition),
		mIsEnabled(aCopy.mIsEnabled),
		mHitCount(aCopy.mHitCount),
        mHitCountCondition(aCopy.mHitCountCondition),
        mCurrentHitCount(aCopy.mCurrentHitCount)
	{
	}

	/////////////////////////////////////////////////////////////////////
	Breakpoint::Breakpoint(std::string aFileName, int aLine, std::string aCondition /*= ""*/,
                           HitCountConditions hitCountCondition /*= BreakAlways*/, bool aIsEnabled /*= true*/) :
		mFileName(aFileName),
		mLine(aLine),
		mCondition(aCondition),
		mIsEnabled(aIsEnabled),
		mHitCount(0),
        mHitCountCondition(hitCountCondition),
        mCurrentHitCount(0)
	{
	}

    /////////////////////////////////////////////////////////////////////
    void Breakpoint::SetCurrentHitCount(int aHitCount)
    {
        mCurrentHitCount = aHitCount;
    }

	/////////////////////////////////////////////////////////////////////
	int Breakpoint::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += RpcCommandBase::GetStringDataSize(mFileName);
		totalSize += RpcCommandBase::GetIntegerDataSize();
		totalSize += RpcCommandBase::GetStringDataSize(mCondition);
		totalSize += RpcCommandBase::GetIntegerDataSize();
        totalSize += RpcCommandBase::GetIntegerDataSize();
		totalSize += RpcCommandBase::GetBooleanDataSize();
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void Breakpoint::DeserializeData(const char* data, int &offset)
	{
		RpcCommandBase::DeserializeString(data, offset, mFileName);
		RpcCommandBase::DeserializeInteger(data, offset, mLine);
		RpcCommandBase::DeserializeString(data, offset, mCondition);
		RpcCommandBase::DeserializeInteger(data, offset, mHitCount);
        RpcCommandBase::DeserializeInteger(data, offset, (int&)mHitCountCondition);
		RpcCommandBase::DeserializeBoolean(data, offset, mIsEnabled);
	}

	/////////////////////////////////////////////////////////////////////
	void Breakpoint::SerializeData(char* data, int &offset) const
	{
		RpcCommandBase::SerializeString(data, offset, mFileName);
		RpcCommandBase::SerializeInteger(data, offset, mLine);
		RpcCommandBase::SerializeString(data, offset, mCondition);
		RpcCommandBase::SerializeInteger(data, offset, mHitCount);
        RpcCommandBase::SerializeInteger(data, offset, mHitCountCondition);
		RpcCommandBase::SerializeBoolean(data, offset, mIsEnabled);
	}

	/////////////////////////////////////////////////////////////////////
	Breakpoint& Breakpoint::operator=(const Breakpoint &aSource)
	{
		if (this != &aSource)
		{
			mFileName = aSource.mFileName;
			mLine = aSource.mLine;
			mCondition = aSource.mCondition;
			mIsEnabled = aSource.mIsEnabled;
			mHitCount = aSource.mHitCount;
            mHitCountCondition = aSource.mHitCountCondition;
            mCurrentHitCount = aSource.mCurrentHitCount;
		}

		return *this;
	}
}