#ifndef DEBUGGING_LUATHREAD_HPP
#define DEBUGGING_LUATHREAD_HPP

#include <string>

#include <lua.hpp>

#include "../RpcCommand.hpp"
#include "CallStack.hpp"

namespace debugging
{
	class LuaThread : public IRPCSerializableData
	{
	public:
		LuaThread();
		LuaThread(const LuaThread &aCopy);
		LuaThread(lua_State* aState, int aThreadID, bool aIsMain);

        LuaThread& operator=(const LuaThread &aSource);

		inline lua_State* GetLuaState() const { return mLuaState; }
		inline int GetThreadID() const { return mThreadID; }
		inline bool IsMain() const { return mIsMain; }
		inline CallStack GetCallStack() const { return mCallStack; }
        inline CallStack& GetCallStack() { return mCallStack; }
		
		virtual inline IRPCSerializableData* GetData() { return this; }

		virtual int GetSerializedDataSize() const;
		virtual void DeserializeData(const char* data, int &offset);
		virtual void SerializeData(char* data, int &offset) const;

	private:
		lua_State* mLuaState;
		int mThreadID;
		bool mIsMain;
		CallStack mCallStack;
	};

	typedef std::map<int, LuaThread> LuaThreadsMap;
}

#endif //DEBUGGING_LUATHREAD_HPP