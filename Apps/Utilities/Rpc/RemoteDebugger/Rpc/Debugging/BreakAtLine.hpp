#ifndef DEBUGGING_BREAKATLINE_HPP
#define DEBUGGING_BREAKATLINE_HPP

#include <string>
#include <sstream>
#include <vector>
#include <map>

#include <boost/shared_ptr.hpp>

#include "RpcCommand.hpp"
#include "DebugManager/Breakpoint.hpp"
#include "DebugManager/LuaThread.hpp"
#include "DebugManager/LuaVariable.hpp"

namespace debugging
{
	class BreakAtLine : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		BreakAtLine();
		BreakAtLine(std::string aFileName, int aLine, LuaVariableList aLocals, LuaThreadsMap aLuaThreads, int aLuaThreadID, CallStack aCurrentThreadCallStack, bool aIsInError);
		virtual ~BreakAtLine();

		virtual inline IRPCSerializableData* GetData() { return this; }

		virtual int GetSerializedDataSize() const;
		virtual void DeserializeData(const char* data, int &offset);
		virtual void SerializeData(char* data, int &offset) const;

	private:
		std::string mFileName;
        int mLine;
		int mThreadID;
        bool mIsInError;
		LuaVariableList mLocals;
		LuaThreadsMap mLuaThreads;
        CallStack mCurrentThreadCallStack;
	};
}

#endif //DEBUGGING_BREAKATLINE_HPP