#include "LuaThread.hpp"

#include <boost/foreach.hpp>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	LuaThread::LuaThread() :
		mLuaState(NULL),
		mThreadID(0),
		mIsMain(false)
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaThread::LuaThread(const LuaThread &aCopy) :
		mLuaState(aCopy.mLuaState),
		mThreadID(aCopy.mThreadID),
		mIsMain(aCopy.mIsMain),
		mCallStack(aCopy.mCallStack)
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaThread::LuaThread(lua_State* aState, int aThreadID, bool aIsMain) :
		mLuaState(aState),
		mThreadID(aThreadID),
		mIsMain(aIsMain)
	{
	}

    /////////////////////////////////////////////////////////////////////
	LuaThread& LuaThread::operator=(const LuaThread &aSource)
	{
		if (this != &aSource)
		{
			mLuaState = aSource.mLuaState;
		    mThreadID = aSource.mThreadID;
		    mIsMain = aSource.mIsMain;
		    mCallStack = aSource.mCallStack;
		}

		return *this;
	}

	/////////////////////////////////////////////////////////////////////
	int LuaThread::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += RpcCommandBase::GetIntegerDataSize();

        if (mCallStack.GetCallStack().size() > 0)
        {
            CallStackItem csiTop = mCallStack.GetCallStack().back();
            totalSize += RpcCommandBase::GetComplexDataSize(&csiTop);
        }
        else
        {
            CallStackItem csiEmpty;
            totalSize += RpcCommandBase::GetComplexDataSize(&csiEmpty);
        }

		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void LuaThread::DeserializeData(const char* data, int &offset)
	{
		/* LuaThread deserialization is not supported. The client should
           always receive this type of data and not sending it. */
	}

	/////////////////////////////////////////////////////////////////////
	void LuaThread::SerializeData(char* data, int &offset) const
	{
		RpcCommandBase::SerializeInteger(data, offset, mThreadID);
        if (mCallStack.GetCallStack().size() > 0)
        {
            const CallStackItem csiTop = mCallStack.GetCallStack().back();
            RpcCommandBase::SerializeComplexData(data, offset, &csiTop);
        }
        else
        {
            CallStackItem csiEmpty;
            RpcCommandBase::SerializeComplexData(data, offset, &csiEmpty);
        }
	}
}