#ifndef DEBUGGING_DEBUGMANAGER_HPP
#define DEBUGGING_DEBUGMANAGER_HPP

#include <limits>
#include <map>
#include <string>
#include <vector>

#include <boost/noncopyable.hpp>
#include <boost/shared_ptr.hpp>
#include <lua.hpp>

#include "Breakpoint.hpp"
#include "CallStack.hpp"
#include "DebugInfo.hpp"
#include "LuaVariable.hpp"
#include "LuaThread.hpp"
#include "../../RpcServer.hpp"

namespace debugging
{
	void LuaDebugHookCallback(lua_State *L, lua_Debug *ar);
	int LuaDebugErrorHandler(lua_State* L);
	int LuaPrintHandler(lua_State* L);
	int LuaCreateCoroutineHandler(lua_State* L);
	DWORD WINAPI RunDebugger(LPVOID lpParam);

	enum DebugAction
	{
		eDEBUG_ACTION_NONE = 0,
		eDEBUG_ACTION_RUN,
		eDEBUG_ACTION_STEP_INTO,
		eDEBUG_ACTION_STEP_OVER,
        eDEBUG_ACTION_STEP_OUT,
		eDEBUG_ACTION_BREAK,
		eDEBUG_ACTION_STOP,
		eDEBUG_ACTION_CONTINUE
	};

	typedef public std::map<int, Breakpoint> BreakpointListMap;

	class BreakpointList
	{
	public:
		inline BreakpointList() {}
		inline BreakpointList(const BreakpointList &aCopy) :
			mBreakpointList(aCopy.mBreakpointList.begin(), aCopy.mBreakpointList.end()) {}

		inline BreakpointListMap& GetBreakpointList() { return mBreakpointList; }

	private:
		BreakpointListMap mBreakpointList;
	};

	typedef std::map<std::string, BreakpointList> BreakpointsMapType;

    struct RunDebuggerStartParams
    {
        int mErrorHandlerIndex;
        int mParamCount;
        int mRetCount;
    };

    struct RunToLine
    {
        RunToLine() :
            mFileName(""),
            mLine(std::numeric_limits<int>::max())
        {
        }

        RunToLine(RunToLine &aCopy) :
            mFileName(aCopy.mFileName),
            mLine(aCopy.mLine)
        {
        }

        std::string mFileName;
        int mLine;
    };

    enum DebugManagerStates
    {
        eDEBUGMANAGER_WAITINGDEBUGACTION,
        eDEBUGMANAGER_RUNNING
    };

	class DebugManager : private boost::noncopyable
	{
		friend void LuaDebugHookCallback(lua_State *L, lua_Debug *ar);
        friend int LuaDebugPanicHandler(lua_State* L);
		friend int LuaDebugErrorHandler(lua_State* L);
		friend int LuaPrintHandler(lua_State* L);
		friend int LuaCreateCoroutineHandler(lua_State* L);

	public:
		inline static DebugManager* GetInstancePtr() { return &mInstance; }
        inline static const char* GetLuaVersion() { return LUA_RELEASE; }
        static bool CheckLuaSyntax(const char* aScript, const char* aScriptName, char* aErrBuf, int aErrBufLen);

		void Initialize(rpc::RpcServer* apServer, lua_State* apInitialLuaState = NULL);
		void Uninitialize();
        void UninitializeDebugData();

        void SetNextDebugAction(int aDebugAction);

		void AddBreakpoint(const Breakpoint &aBreakpoint);
		void AddBreakpoint(std::string aFileName, int aLine, std::string aCondition = "",
                           HitCountConditions hitCountCondition = BreakAlways, bool aIsEnabled = true);
        Breakpoint& GetBreakpointAtLine(std::string aFileName, int aLine);
		void ClearBreakpoints();

		void AddLuaThread(const LuaThread &aLuaThread);
		void AddLuaThread(lua_State* L);
        void CleanUpThreads(lua_State* L);
		void ClearLuaThreads();

		void LoadScriptInVM(std::string aScriptFileName, bool aMultiThreaded);
        void LoadCFunctionInVM(lua_CFunction aFunction, void* aUD, bool aMultiThreaded);
        void CallFunctionOnStack(lua_State* L, int aParamCount, int aRetCount);

		void GetGlobal(std::string aGlobalName);
		void GetLocal(std::string aLocalName);
        void GetLocal(int aLocalIndex);
        void SetLocal(int aLocalIndex, LuaVariable aLuaVar);
		void GetLocals(lua_State* L, lua_Debug* ar, bool aIsInError);

        int PushErrorHandler(lua_State* L);
        void PushLuaVariable(lua_State* L, LuaVariable aLuaVar);
        std::string LuaStackValueToString(lua_State* L, int aValIndex);

        inline lua_State* GetLuaState() { return mpLuaState; }
		inline rpc::RpcServer* GetRpcServer() { return mpServer; }
        inline DebugManagerStates GetCurrentState() const { return mCurrentState; }
        inline void SetDebugInfo(DebugInfo aDI) { mDebugInfo = aDI; }
        inline void SetRunToLine(RunToLine aRunToLine) { mRunToLine = aRunToLine; }
        inline lua_State* GetCurrentContext() const { return mpCurrentContext; }
        inline int GetCurrentCallStackLevel() const { return mCurrentCallStackLevel; }

	private:
		DebugManager();
		~DebugManager();

		void LuaDebugHookCallback(lua_State* L, lua_Debug* ar);
		int LuaDebugErrorHandler(lua_State* L);
        int LuaDebugPanicHandler(lua_State* L);
		int LuaPrintHandler(lua_State* L);
		int LuaCreateCoroutineHandler(lua_State* L);

		void InitLuaThread(lua_State* L);
		int GetLuaThreadID(lua_State* L);
        bool ShouldBreak(lua_State* L, Breakpoint &aBreakpoint, std::string aFileName, int line, int aStackTop);

		void WaitForNextDebugAction();

		void DoLine(lua_State *L, lua_Debug *ar, bool aIsInError);
		void DoCall(lua_State *L, lua_Debug *ar);
		void DoReturn(lua_State *L, lua_Debug *ar);

		static DebugManager mInstance;
        DebugManagerStates mCurrentState;

		lua_State* mpLuaState;
        lua_State* mpCurrentContext;
        DebugInfo mDebugInfo;
		
        LuaVariableList mLocals;
		BreakpointsMapType mBreakpoints;
		LuaThreadsMap mLuaThreads;

        int mCurrentCallStackLevel;
        int mLastCallStackLevel;
        int mLastLineCall;
		volatile DebugAction mNextDebugAction;

        bool mIsOwnMainState;
        bool mLocalsDirty;
		bool mIsInitialized;
		lua_CFunction mOriginalLuaPrintFct;
        lua_CFunction mOriginalLuaPanicFct;

        RunToLine mRunToLine;
        RunDebuggerStartParams mRunDebuggerStartParams;
		HANDLE mDebuggerThread;
		DWORD mDebuggerThreadID;
		rpc::RpcServer* mpServer;
	};
}

#endif //DEBUGGING_DEBUGMANAGER_HPP