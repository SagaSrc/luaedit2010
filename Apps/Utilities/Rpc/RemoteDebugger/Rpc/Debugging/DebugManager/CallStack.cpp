#include "CallStack.hpp"

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
    CallStackItem::CallStackItem() :
        mLineCall(-1),
        mFileName(""),
		mFuncName(""),
		mParams(""),
		mSource(""),
        mLineCalled(-1),
		mIsLineCall(false)
	{
	}

	/////////////////////////////////////////////////////////////////////
	CallStackItem::CallStackItem(const CallStackItem &aCopy) :
		mLineCall(aCopy.mLineCall),
		mFileName(aCopy.mFileName),
		mFuncName(aCopy.mFuncName),
		mParams(aCopy.mParams),
		mSource(aCopy.mSource),
        mLineCalled(aCopy.mLineCalled),
		mIsLineCall(aCopy.mIsLineCall)
	{
	}

	/////////////////////////////////////////////////////////////////////
	CallStackItem::CallStackItem(int aLineCall, std::string aFileName, std::string aFuncName, std::string aSource,
                                 std::string aParams, int aLineCalled, bool aIsLineCall) :
		mLineCall(aLineCall),
		mFileName(aFileName),
		mFuncName(aFuncName),
		mParams(aParams),
		mSource(aSource),
        mLineCalled(aLineCalled),
		mIsLineCall(aIsLineCall)
	{
	}

	/////////////////////////////////////////////////////////////////////
	int CallStackItem::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += RpcCommandBase::GetStringDataSize(mFileName);
		totalSize += RpcCommandBase::GetStringDataSize(mFuncName);
		totalSize += RpcCommandBase::GetIntegerDataSize();
		totalSize += RpcCommandBase::GetStringDataSize(mSource);
		totalSize += RpcCommandBase::GetStringDataSize(mParams);
        totalSize += RpcCommandBase::GetIntegerDataSize();
		totalSize += RpcCommandBase::GetBooleanDataSize();
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void CallStackItem::DeserializeData(const char* data, int &offset)
	{
		RpcCommandBase::DeserializeString(data, offset, mFileName);
		RpcCommandBase::DeserializeString(data, offset, mFuncName);
		RpcCommandBase::DeserializeInteger(data, offset, mLineCall);
		RpcCommandBase::DeserializeString(data, offset, mSource);
		RpcCommandBase::DeserializeString(data, offset, mParams);
        RpcCommandBase::DeserializeInteger(data, offset, mLineCalled);
		RpcCommandBase::DeserializeBoolean(data, offset, mIsLineCall);
	}

	/////////////////////////////////////////////////////////////////////
	void CallStackItem::SerializeData(char* data, int &offset) const
	{
		RpcCommandBase::SerializeString(data, offset, mFileName);
		RpcCommandBase::SerializeString(data, offset, mFuncName);
		RpcCommandBase::SerializeInteger(data, offset, mLineCall);
		RpcCommandBase::SerializeString(data, offset, mSource);
		RpcCommandBase::SerializeString(data, offset, mParams);
        RpcCommandBase::SerializeInteger(data, offset, mLineCalled);
		RpcCommandBase::SerializeBoolean(data, offset, mIsLineCall);
	}

	/////////////////////////////////////////////////////////////////////
	CallStack::CallStack(const CallStack &aCopy) :
		mCurrentStackLevel(aCopy.mCurrentStackLevel),
		mCallStack(aCopy.mCallStack)
	{
	}

	/////////////////////////////////////////////////////////////////////
	CallStack::CallStack() :
		mCurrentStackLevel(0)
	{
	}

	/////////////////////////////////////////////////////////////////////
	void CallStack::PushCallStackItem(int aLineCall, std::string aFileName, std::string aFuncName, std::string aSource,
                                      std::string aParams, int aLineCalled, bool aIsLineCall)
	{
		mCallStack.push_back(CallStackItem(aLineCall, aFileName, aFuncName, aSource, aParams, aLineCalled, aIsLineCall));
	}

	/////////////////////////////////////////////////////////////////////
	void CallStack::PopCallStackItem()
	{
		if (GetStackTop() > 0)
		{
			mCallStack.pop_back();
		}
	}

	/////////////////////////////////////////////////////////////////////
	void CallStack::ClearCallStack()
	{
		while (mCallStack.size() > 0)
		{
			PopCallStackItem();
		}
	}
}