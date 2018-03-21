#include "BreakAtLine.hpp"

#include "DebugManager/DebugManager.hpp"

#include <boost/foreach.hpp>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	BreakAtLine::BreakAtLine() :
		RpcCommandBase(eRPC_COMMANDTYPE_BREAKATLINE)
	{
	}

	/////////////////////////////////////////////////////////////////////
		BreakAtLine::BreakAtLine(std::string aFileName, int aLine, LuaVariableList aLocals, LuaThreadsMap aLuaThreads, int aLuaThreadID, CallStack aCurrentThreadCallStack, bool aIsInError) :
		RpcCommandBase(eRPC_COMMANDTYPE_BREAKATLINE),
		mFileName(aFileName),
		mLine(aLine),
		mThreadID(aLuaThreadID),
		mIsInError(aIsInError),
		mLuaThreads(aLuaThreads.begin(), aLuaThreads.end()),
		mLocals(aLocals.begin(), aLocals.end()),
        mCurrentThreadCallStack(aCurrentThreadCallStack)
	{
	}

	/////////////////////////////////////////////////////////////////////
	BreakAtLine::~BreakAtLine()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int BreakAtLine::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetStringDataSize(mFileName);
		totalSize += GetIntegerDataSize();
		totalSize += GetIntegerDataSize();
		totalSize += GetBooleanDataSize();

		//
		// Lua Variables
		//
		totalSize += GetIntegerDataSize();

		BOOST_FOREACH(LuaVariable luaVar, mLocals)
		{
			totalSize += GetComplexDataSize(&luaVar);
		}

		//
		// Lua threads
		//
		totalSize += GetIntegerDataSize();

		BOOST_FOREACH(const LuaThreadsMap::value_type &luaThreadPair, mLuaThreads)
		{
			totalSize += GetComplexDataSize(&luaThreadPair.second);
		}

        //
        // Call stack of current thread
        //
        totalSize += RpcCommandBase::GetIntegerDataSize();

        BOOST_FOREACH(const CallStackItem csi, mCurrentThreadCallStack.GetCallStack())
		{
			totalSize += RpcCommandBase::GetComplexDataSize(&csi);
		}

		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void BreakAtLine::DeserializeData(const char* data, int &offset)
	{
		/* There is no need for this command to be deserialized since
		   it should never be sent from client. Only the server should
		   send this type of command. */
	}

	/////////////////////////////////////////////////////////////////////
	void BreakAtLine::SerializeData(char* data, int &offset) const
	{
		SerializeString(data, offset, mFileName);
		SerializeInteger(data, offset, mLine);
		SerializeInteger(data, offset, mThreadID);
		SerializeBoolean(data, offset, mIsInError);

		//
		// Lua Variables
		//
		SerializeInteger(data, offset, mLocals.size());

		BOOST_FOREACH(LuaVariable luaVar, mLocals)
		{
			SerializeComplexData(data, offset, &luaVar);
		}

		//
		// Lua threads
		//
		SerializeInteger(data, offset, mLuaThreads.size());

		BOOST_FOREACH(const LuaThreadsMap::value_type &luaThreadPair, mLuaThreads)
		{
			SerializeComplexData(data, offset, &luaThreadPair.second);
		}

        //
		// Call stack
		//
		RpcCommandBase::SerializeInteger(data, offset, mCurrentThreadCallStack.GetCallStack().size());

		BOOST_FOREACH(const CallStackItem csi, mCurrentThreadCallStack.GetCallStack())
		{
			RpcCommandBase::SerializeComplexData(data, offset, &csi);
		}
	}
}