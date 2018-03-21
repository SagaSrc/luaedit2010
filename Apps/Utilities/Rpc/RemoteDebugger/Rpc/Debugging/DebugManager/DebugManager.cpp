#include "DebugManager.hpp"

#include "../BreakAtLine.hpp"

#include <fstream>
#include <iostream>

namespace debugging
{
	DebugManager DebugManager::mInstance;

    /////////////////////////////////////////////////////////////////////
    std::string wstrtostr(const std::wstring &wstr)
    {
        // Convert a Unicode string to an ASCII string
        std::string strTo;
        char *szTo = new char[wstr.length() + 1];
        szTo[wstr.size()] = '\0';
        WideCharToMultiByte(CP_ACP, 0, wstr.c_str(), -1, szTo, (int)wstr.length(), NULL, NULL);
        strTo = szTo;
        delete[] szTo;
        return strTo;
    }
    
    /////////////////////////////////////////////////////////////////////
    std::wstring strtowstr(const std::string &str)
    {
        // Convert an ASCII string to a Unicode String
        std::wstring wstrTo;
        wchar_t *wszTo = new wchar_t[str.length() + 1];
        wszTo[str.size()] = L'\0';
        MultiByteToWideChar(CP_ACP, 0, str.c_str(), -1, wszTo, (int)str.length());
        wstrTo = wszTo;
        delete[] wszTo;
        return wstrTo;
    }

	/////////////////////////////////////////////////////////////////////
	void LuaDebugHookCallback(lua_State* L, lua_Debug* ar)
	{
		DebugManager::GetInstancePtr()->LuaDebugHookCallback(L, ar);
	}

	/////////////////////////////////////////////////////////////////////
	int LuaDebugErrorHandler(lua_State* L)
	{
		return DebugManager::GetInstancePtr()->LuaDebugErrorHandler(L);
	}

    /////////////////////////////////////////////////////////////////////
	int LuaDebugPanicHandler(lua_State* L)
	{
		return DebugManager::GetInstancePtr()->LuaDebugPanicHandler(L);
	}

	/////////////////////////////////////////////////////////////////////
	int LuaPrintHandler(lua_State* L)
	{
		return DebugManager::GetInstancePtr()->LuaPrintHandler(L);
	}

	/////////////////////////////////////////////////////////////////////
	int LuaCreateCoroutineHandler(lua_State* L)
	{
		return DebugManager::GetInstancePtr()->LuaCreateCoroutineHandler(L);
	}

	/////////////////////////////////////////////////////////////////////
	DebugManager::DebugManager() :
		mIsInitialized(false),
		mpLuaState(NULL),
        mpCurrentContext(NULL),
		mNextDebugAction(eDEBUG_ACTION_NONE),
		mDebuggerThreadID(0),
		mDebuggerThread(NULL),
        mLocalsDirty(true),
        mLastLineCall(-1),
        mIsOwnMainState(true),
        mCurrentState(eDEBUGMANAGER_RUNNING)
	{
	}

	/////////////////////////////////////////////////////////////////////
	DebugManager::~DebugManager()
	{
	}

    /////////////////////////////////////////////////////////////////////
    bool DebugManager::CheckLuaSyntax(const char* aScript, const char* aScriptName, char* aErrBuf, int aErrBufLen)
    {
        bool err = false;

        if (aScript)
        {
            lua_State* L = luaL_newstate();

            if (luaL_loadbuffer(L, aScript, strlen(aScript), aScriptName) == LUA_ERRSYNTAX)
		    {
                if (lua_type(L, -1) == LUA_TSTRING)
                {
                    err = true;
                    strncpy(aErrBuf, lua_tostring(L, -1), (size_t)aErrBufLen);
                }
            }

            lua_close(L);
        }

        return err;
    }

	/////////////////////////////////////////////////////////////////////
	void DebugManager::Initialize(rpc::RpcServer* apServer, lua_State* apInitialLuaState /* = NULL*/)
	{
		if (!mIsInitialized)
		{
			mIsInitialized = true;
			mpServer = apServer;
			mpLuaState = apInitialLuaState ? apInitialLuaState : luaL_newstate();
            mIsOwnMainState = apInitialLuaState == NULL;

			// Open lua libs
			luaL_openlibs(mpLuaState);

			// Init the main thread
			InitLuaThread(mpLuaState);

            // Panic function override
            mOriginalLuaPanicFct = lua_atpanic(mpLuaState, &debugging::LuaDebugPanicHandler);

			// Print function override
			lua_pushstring(mpLuaState, "print");
			lua_rawget(mpLuaState, LUA_GLOBALSINDEX);
			mOriginalLuaPrintFct = lua_tocfunction(mpLuaState, -1);
			lua_pop(mpLuaState, 1);
			lua_register(mpLuaState, "print", &debugging::LuaPrintHandler);

			// Coroutine.create function override
			lua_pushstring(mpLuaState, "coroutine");
			lua_rawget(mpLuaState, LUA_GLOBALSINDEX);
			lua_pushstring(mpLuaState, "create");
			lua_pushcfunction(mpLuaState, &debugging::LuaCreateCoroutineHandler);
			lua_rawset(mpLuaState, -3);
			lua_pop(mpLuaState, 1);
		}
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::Uninitialize()
	{
		if (mIsInitialized)
		{
			// Clear DebugManager's internal data
			mIsInitialized = false;
			mpServer = NULL;

			if (mDebuggerThread != NULL)
			{
				mNextDebugAction = eDEBUG_ACTION_STOP;
				WaitForSingleObject(mDebuggerThread, INFINITE);
				mDebuggerThreadID = 0;
				CloseHandle(mDebuggerThread);
				mDebuggerThread = NULL;
			}

			// Clear debug data
            UninitializeDebugData();
            ClearLuaThreads();
            mNextDebugAction = eDEBUG_ACTION_NONE;

			// Close lua state if we own it
			if (mIsOwnMainState && mpLuaState)
			{
				lua_close(mpLuaState);
				mpLuaState = NULL;
			}
		}
	}

    /////////////////////////////////////////////////////////////////////
    void DebugManager::UninitializeDebugData()
    {
        lua_atpanic(mpLuaState, mOriginalLuaPanicFct);
        ClearBreakpoints();
    }

    /////////////////////////////////////////////////////////////////////
    void DebugManager::SetNextDebugAction(int aDebugAction)
    {
        mNextDebugAction = (DebugAction)aDebugAction;
    }

	/////////////////////////////////////////////////////////////////////
	void DebugManager::AddBreakpoint(const Breakpoint &aBreakpoint)
	{
		std::string fileName = aBreakpoint.GetFileName();
        std::transform(fileName.begin(), fileName.end(), fileName.begin(), tolower);

		if (mBreakpoints.find(fileName) == mBreakpoints.end())
		{
			BreakpointList bpl;
			mBreakpoints[fileName] = bpl;
		}

		mBreakpoints[fileName].GetBreakpointList()[aBreakpoint.GetLine()] = aBreakpoint;
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::AddBreakpoint(std::string aFileName, int aLine, std::string aCondition /*= ""*/, 
						             HitCountConditions hitCountCondition /*= BreakAlways*/, bool aIsEnabled /*= true*/)
	{
		if (mBreakpoints.find(aFileName) == mBreakpoints.end())
		{
			BreakpointList bpl;
			mBreakpoints[aFileName] = bpl;
		}

		Breakpoint bp(aFileName, aLine, aCondition, hitCountCondition, aIsEnabled);
		mBreakpoints[aFileName].GetBreakpointList()[aLine] = bp;
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::ClearBreakpoints()
	{
		mBreakpoints.clear();
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::AddLuaThread(const LuaThread &aLuaThread)
	{
		mLuaThreads[aLuaThread.GetThreadID()] = aLuaThread;
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::AddLuaThread(lua_State* L)
	{
		LuaThread luaThread(L, GetLuaThreadID(L), L == mpLuaState);
		mLuaThreads[luaThread.GetThreadID()] = luaThread;
	}

    /////////////////////////////////////////////////////////////////////
    void DebugManager::CleanUpThreads(lua_State* L)
    {
        if (mLuaThreads.begin() != mLuaThreads.end())
        {
            for (LuaThreadsMap::iterator it = mLuaThreads.begin(); it != mLuaThreads.end();)
            {
                lua_State* co = it->second.GetLuaState();

                if (co != NULL && !it->second.IsMain() && co != L)
                {
                    switch (lua_status(co))
                    {
                    case LUA_YIELD:
                        {
                            ++it;
                            break;
                        }
                    case 0:
                        {
                            lua_Debug ar;

                            if (lua_getstack(co, 0, &ar) <= 0 && lua_gettop(co) == 0)
                            {
                                // The thread is dead, let's remove it
                                mLuaThreads.erase(it++);
                            }
                            else
                            {
                                ++it;
                            }

                            break;
                        }
                    default:
                        {
                            // The thread is dead, let's remove it
                            mLuaThreads.erase(it++);
                            break;
                        }
                    }
                }
                else
                {
                    ++it;
                }
            }
        }
    }

	/////////////////////////////////////////////////////////////////////
	void DebugManager::ClearLuaThreads()
	{
		mLuaThreads.clear();
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::LoadScriptInVM(std::string aScriptFileName, bool aMultiThreaded)
	{
        int errorHandlerIndex = PushErrorHandler(mpLuaState);

		if (luaL_loadfile(mpLuaState, aScriptFileName.c_str()) == 0)
		{
            mRunDebuggerStartParams.mErrorHandlerIndex = errorHandlerIndex;
            mRunDebuggerStartParams.mParamCount = 0;
            mRunDebuggerStartParams.mRetCount = -1;

            if (aMultiThreaded)
            {
			    mDebuggerThread = CreateThread(NULL, 0, RunDebugger, (LPVOID)&mRunDebuggerStartParams, 0, &mDebuggerThreadID);
            }
            else
            {
                RunDebugger((LPVOID)&mRunDebuggerStartParams);
            }
		}
		else if (lua_type(mpLuaState, -1) == LUA_TSTRING)
		{
			const char* pErrMsg = lua_tostring(mpLuaState, -1);
			LuaError luaErrCmd(pErrMsg, LUAERROR_SYNTAX);
			mpServer->SendMessage(&luaErrCmd);
		}
	}

    /////////////////////////////////////////////////////////////////////
    void DebugManager::LoadCFunctionInVM(lua_CFunction aFunction, void* aUD, bool aMultiThreaded)
    {
        int errorHandlerIndex = PushErrorHandler(mpLuaState);
        lua_pushcfunction(mpLuaState, aFunction);
        lua_pushlightuserdata(mpLuaState, aUD);

        mRunDebuggerStartParams.mErrorHandlerIndex = errorHandlerIndex;
        mRunDebuggerStartParams.mParamCount = 1;
        mRunDebuggerStartParams.mRetCount = -1;

        if (aMultiThreaded)
        {
            mDebuggerThread = CreateThread(NULL, 0, RunDebugger, (LPVOID)&mRunDebuggerStartParams, 0, &mDebuggerThreadID);
        }
        else
        {
            RunDebugger((LPVOID)&mRunDebuggerStartParams);
        }
    }

    /////////////////////////////////////////////////////////////////////
    void DebugManager::CallFunctionOnStack(lua_State* L, int aParamCount, int aRetCount)
    {
        PushErrorHandler(L);
        int errorHandlerIndex = lua_gettop(L) - aParamCount - 2;
        lua_insert(L, errorHandlerIndex);
        mRunDebuggerStartParams.mErrorHandlerIndex = errorHandlerIndex;
        mRunDebuggerStartParams.mParamCount = aParamCount;
        mRunDebuggerStartParams.mRetCount = aRetCount;
        RunDebugger((LPVOID)&mRunDebuggerStartParams);
    }

	/////////////////////////////////////////////////////////////////////
	void DebugManager::GetGlobal(std::string aGlobalName)
	{
		lua_getglobal(mpLuaState, aGlobalName.c_str());
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::GetLocal(std::string aLocalName)
	{
		int index = 1;
		lua_Debug ar;
		lua_getstack(mpCurrentContext, mCurrentCallStackLevel, &ar);
		lua_getinfo(mpCurrentContext, "Snl", &ar);
		const char* name = lua_getlocal(mpCurrentContext, &ar, index);

        while (name && strlen(name) != 0)
        {
			if (aLocalName == name)
			{
				return;
			}
			else
			{
				lua_pop(mpCurrentContext, 1);
				++index;
				name = lua_getlocal(mpCurrentContext, &ar, index);
			}
		}
	}

    /////////////////////////////////////////////////////////////////////
    void DebugManager::GetLocal(int aLocalIndex)
    {
        lua_Debug ar;
		lua_getstack(mpCurrentContext, mCurrentCallStackLevel, &ar);
		lua_getinfo(mpCurrentContext, "Snl", &ar);
        lua_getlocal(mpCurrentContext, &ar, aLocalIndex);
    }

    /////////////////////////////////////////////////////////////////////
    void DebugManager::SetLocal(int aLocalIndex, LuaVariable aLuaVar)
    {
        lua_Debug ar;
		lua_getstack(mpCurrentContext, mCurrentCallStackLevel, &ar);
        PushLuaVariable(mpCurrentContext, aLuaVar);
        lua_setlocal(mpCurrentContext, &ar, aLocalIndex);
    }

    /////////////////////////////////////////////////////////////////////
    int DebugManager::PushErrorHandler(lua_State* L)
    {
        lua_pushcfunction(L, &debugging::LuaDebugErrorHandler);
        return lua_gettop(L);
    }

    /////////////////////////////////////////////////////////////////////
    void DebugManager::PushLuaVariable(lua_State* L, LuaVariable aLuaVar)
    {
        switch (aLuaVar.GetType())
		{
        case LUA_TSTRING:
            {
                lua_pushstring(L, aLuaVar.GetValue().c_str());
                break;
            }
        case LUA_TNUMBER:
            {
                lua_pushnumber(L, atof(aLuaVar.GetValue().c_str()));
                break;
            }
        case LUA_TNIL:
            {
                lua_pushnil(L);
                break;
            }
        }
    }

	/////////////////////////////////////////////////////////////////////
	std::string DebugManager::LuaStackValueToString(lua_State* L, int aValIndex)
	{
		switch (lua_type(L, aValIndex))
		{
		case LUA_TSTRING:
			{
				return lua_tostring(L, aValIndex);
			}
		case LUA_TNUMBER:
			{
				char buff[16];
				sprintf_s(buff, sizeof(buff), "%d", lua_tonumber(L, aValIndex));
				return buff;
			}
		case LUA_TTABLE:
		case LUA_TFUNCTION:
		case LUA_TLIGHTUSERDATA:
		case LUA_TUSERDATA:
		case LUA_TTHREAD:
			{
				char buff[16];
				sprintf_s(buff, sizeof(buff), "0x%X", lua_topointer(L, aValIndex));
				return buff;
			}
		case LUA_TNIL:
			{
				return "nil";
			}
		case LUA_TNONE:
		default:
			{
				return "none";
			}
		}
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::GetLocals(lua_State* L, lua_Debug* ar, bool aIsInError)
	{
        if (mLocalsDirty)
        {
		    CallStack& threadCallStack = mLuaThreads[GetLuaThreadID(L)].GetCallStack();
		    int stackDepth = aIsInError ? threadCallStack.GetCurrentStackLevel() + 1 : threadCallStack.GetCurrentStackLevel();
            mLocals.clear();

		    if (lua_getstack(L, stackDepth, ar) == 0)
		    {
			    return;
		    }

		    int index = 1;
		    const char* fullNameOut = lua_getlocal(L, ar, index);

            while (fullNameOut && strlen(fullNameOut) != 0)
            {
                char fullNameIn[128];
			    int type = lua_type(L, -1);
			    std::string localVal = LuaStackValueToString(L, -1);
                itoa(index, fullNameIn, 10);
                mLocals.push_back(LuaVariable(fullNameIn, fullNameOut, fullNameOut, localVal.empty() ? "" : localVal, lua_type(L, -1), index));
                lua_pop(L, 1);
                ++index;
                fullNameOut = lua_getlocal(L, ar, index);
            }

            mLocalsDirty = false;
        }
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::InitLuaThread(lua_State* L)
	{
		// Initialize debug hooks for this thread
		lua_sethook(L, &debugging::LuaDebugHookCallback, LUA_MASKLINE | LUA_MASKCALL | LUA_MASKRET, 0);
		
		// Add thread to thread map
		AddLuaThread(L);
	}

	/////////////////////////////////////////////////////////////////////
	int DebugManager::GetLuaThreadID(lua_State* L)
	{
		return (int)L;
	}

	/////////////////////////////////////////////////////////////////////
	Breakpoint& DebugManager::GetBreakpointAtLine(std::string aFileName, int aLine)
	{
        std::transform(aFileName.begin(), aFileName.end(), aFileName.begin(), tolower);

		if (mBreakpoints.find(aFileName) == mBreakpoints.end())
		{
			BreakpointList bpl;
			mBreakpoints[aFileName] = bpl;
		}

		return mBreakpoints[aFileName].GetBreakpointList()[aLine];
	}

	/////////////////////////////////////////////////////////////////////
	bool DebugManager::ShouldBreak(lua_State* L, Breakpoint &aBreakpoint, std::string aFileName, int line, int aStackTop)
	{
        // Always break when the next action is step into
		if (mNextDebugAction == eDEBUG_ACTION_STEP_INTO)
		{
			return true;
		}

        // Run to line condition is true
        if (aFileName == mRunToLine.mFileName && line == mRunToLine.mLine)
        {
            return true;
        }

		// Step over/out breaking if we're located at the same call
        // stack level as the initial step over was called
		if ((mNextDebugAction == eDEBUG_ACTION_STEP_OVER && aStackTop <= mLastCallStackLevel) ||
            (mNextDebugAction == eDEBUG_ACTION_STEP_OUT && aStackTop < mLastCallStackLevel))
		{
			return true;
		}

		if (aBreakpoint.IsEnabled())
		{
		    if (!aBreakpoint.GetCondition().empty())
		    {
                // Evaluates the data condition
                std::string cond = aBreakpoint.GetCondition();

                // Append return keyword since luaL_loadstring
                // will create a function from this string. The
                // function's result is to be evaluated below.
                cond = "return " + cond;

                // Loads the string
                if (luaL_loadstring(L, cond.c_str()) == 0)
                {
                    // Call the function knowing it should return a bool
                    if (lua_pcall(L, 0, 1, 0) == 0)
                    {
                        // Evaluate the bool result
                        if (lua_toboolean(L, -1) == 0)
                        {
                            // Increment current hit count for this breakpoint
                            aBreakpoint.Hit();

                            // Data condition has failed
                            return false;
                        }
                    }
                }

                // Increment current hit count for this breakpoint
                aBreakpoint.Hit();

                return false;
		    }

            if (aBreakpoint.GetHitCountCondition() != BreakAlways)
            {
                bool hasHitCountMatch = false;

                switch (aBreakpoint.GetHitCountCondition())
                {
                case BreakEqualTo:
                    {
                        hasHitCountMatch = aBreakpoint.GetCurrentHitCount() == aBreakpoint.GetHitCount();
                        break;
                    }
                case BreakMultipleOf:
                    {
                        hasHitCountMatch = aBreakpoint.GetCurrentHitCount() % aBreakpoint.GetHitCount() == 0;
                        break;
                    }
                case BreakGreaterOrEqualTo:
                    {
                        hasHitCountMatch = aBreakpoint.GetCurrentHitCount() >= aBreakpoint.GetHitCount();
                        break;
                    }
                }

                if (!hasHitCountMatch)
                {
                    // Increment current hit count for this breakpoint
                    aBreakpoint.Hit();

                    // Hit count condition has failed
                    return false;
                }
            }

            // Increment current hit count for this breakpoint
            aBreakpoint.Hit();
			
            // None of the previous condition failed so
            // we should break at this breakpoint
            return true;
		}

		return false;
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::WaitForNextDebugAction()
	{
		// Reset next debugging action to none
		mNextDebugAction = eDEBUG_ACTION_NONE;
        mCurrentState = eDEBUGMANAGER_WAITINGDEBUGACTION;

		while (mNextDebugAction == eDEBUG_ACTION_NONE)
        {
            Sleep(20);
        }

        mCurrentState = eDEBUGMANAGER_RUNNING;
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::DoLine(lua_State *L, lua_Debug *ar, bool aIsInError)
	{
        // Get current thread ID
        int luaThreadID = GetLuaThreadID(L);
		CallStack& threadCallStack = mLuaThreads[luaThreadID].GetCallStack();
		std::string src(ar->source);
		std::string fileName = src[0] == '@' ? src.substr(1, src.length()) : src;

#ifdef WIN32
        if (fileName.find_first_of(':') == std::string::npos)
        {
            std::wstring wsRelativePath = strtowstr(fileName);
            DWORD pathSize = GetFullPathName(wsRelativePath.c_str(), 0, NULL, NULL);
            boost::scoped_ptr<TCHAR> absolutePath(new TCHAR[pathSize]);
            GetFullPathName(wsRelativePath.c_str(), pathSize, absolutePath.get(), NULL);
            fileName = wstrtostr(absolutePath.get());
        }
#endif //WIN32

        std::transform(fileName.begin(), fileName.end(), fileName.begin(), tolower);
		Breakpoint& breakpoint = GetBreakpointAtLine(fileName, ar->currentline);

		if (ShouldBreak(L, breakpoint, fileName, ar->currentline, threadCallStack.GetStackTop()) ||
            (mNextDebugAction == eDEBUG_ACTION_BREAK && (mpCurrentContext == L || mpCurrentContext == NULL)))
		{
            // Set the new current debugging context call stack level
            mCurrentCallStackLevel = 0;
            mpCurrentContext = L;

            // Clean-up the thread list (some threads might have died since)
            CleanUpThreads(L);

			// Push line call callstack item
			threadCallStack.PushCallStackItem(ar->currentline, fileName.c_str(),
                                              ar->name == NULL ? "" : ar->name, "Lua", "", mLastLineCall, true);

			// Send break at line message
            GetLocals(L, ar, aIsInError);
			BreakAtLine breakAtLnCmd(fileName.c_str(), ar->currentline, mLocals,
									 mLuaThreads, luaThreadID, threadCallStack, aIsInError);
			mpServer->SendMessage(&breakAtLnCmd);

			// Wait for incoming next debugging action from client
			WaitForNextDebugAction();

			// Raise error with specific message to stop Lua's execution (if requested so)
			if (mNextDebugAction == eDEBUG_ACTION_STOP)
			{
                if (mDebugInfo.GetStartAction() != StartAttach)
                {
                    lua_pushstring(mpLuaState, "@@STOP@@");
				    lua_error(mpLuaState);
                    return;
                }
			}

			// Remove line call callstack item
			threadCallStack.PopCallStackItem();

			// Send unbreak from line message
			UnBreakAtLine unbreakAtLnCmd(fileName, ar->currentline);
			mpServer->SendMessage(&unbreakAtLnCmd);
            mLastCallStackLevel = threadCallStack.GetStackTop();
		}
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::DoCall(lua_State *L, lua_Debug *ar)
	{
		CallStack& threadCallStack = mLuaThreads[GetLuaThreadID(L)].GetCallStack();
		int luaStackTop = lua_gettop(L);
        std::string src(ar->source);
		std::string fileName = src[0] == '@' ? src.substr(1, src.length()) : src;
		std::string params = "";

		if (strcmp(ar->what, "main") == 0)
		{
			threadCallStack.PushCallStackItem(0, fileName, "[EntryPoint]", "C", "", mLastLineCall, false);
		}
		else
		{
			if (luaStackTop > 0)
			{
				for (int x = 1; x < luaStackTop; ++x)
				{
					if (lua_type(L, x) == LUA_TSTRING)
					{
						params += "'";
						params += LuaStackValueToString(L, x);
						params += "'";
					}
					else
					{
						params += LuaStackValueToString(L, x);
					}

					if (x < luaStackTop - 1)
					{
						params += ", ";
					}
				}
			}

			threadCallStack.PushCallStackItem(ar->currentline, fileName,
                                              ar->name == NULL ? "" : ar->name,
                                              ar->what, params, mLastLineCall, false);
		}
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::DoReturn(lua_State *L, lua_Debug *ar)
	{
		CallStack& threadCallStack = mLuaThreads[GetLuaThreadID(L)].GetCallStack();
		threadCallStack.PopCallStackItem();
	}

	/////////////////////////////////////////////////////////////////////
	void DebugManager::LuaDebugHookCallback(lua_State *L, lua_Debug *ar)
	{
        // Saturday night live! :P
        // Seriously this is to get information about
        // the callback's reason (EG: the current line)
		lua_getinfo(L, "Snl", ar);
		
		switch (ar->event)
		{
		case LUA_HOOKLINE:
			{
                mLastLineCall = ar->currentline;

				if (strlen(ar->source) > 0 && ar->source[0] == '@')
				{
                    mLocalsDirty = true;
					DoLine(L, ar, false);
				}

				break;
			}
		case LUA_HOOKCALL:
			{
				DoCall(L, ar);
				break;
			}
		case LUA_HOOKRET:
			{
				DoReturn(L, ar);
				break;
			}
		}
	}

    /////////////////////////////////////////////////////////////////////
	int DebugManager::LuaDebugPanicHandler(lua_State* L)
	{
        if (lua_gettop(L) > 0 && lua_type(L, -1) == LUA_TSTRING)
		{
			const char* pErrMsg = lua_tostring(L, -1);

			if (strcmp(pErrMsg, "@@STOP@@") != 0)
			{
                // Send runtime error to client
				LuaError luaErrCmd(pErrMsg, LUAERROR_UNHANDLED);
				mpServer->SendMessage(&luaErrCmd);

                // Wait for incoming next debugging action from client
				WaitForNextDebugAction();
            }
        }
        
        return 0;
    }

	/////////////////////////////////////////////////////////////////////
	int DebugManager::LuaDebugErrorHandler(lua_State* L)
	{
		if (lua_gettop(L) > 0 && lua_type(L, -1) == LUA_TSTRING)
		{
			const char* pErrMsg = lua_tostring(L, -1);

			if (strcmp(pErrMsg, "@@STOP@@") != 0)
			{
				// Send runtime error to client
				LuaError luaErrCmd(pErrMsg, LUAERROR_RUNTIME);
				mpServer->SendMessage(&luaErrCmd);

				// Wait for incoming next debugging action from client
				WaitForNextDebugAction();

				// todo: take actions regarding the response...
				if (mNextDebugAction == eDEBUG_ACTION_BREAK)
				{
					lua_Debug ar;

					// Call getstack at level 1 since level 0 (current level) is this error handler
					if (lua_getstack(L, 1, &ar) != 0)
					{
						lua_getinfo(L, "Snl", &ar);
						DoLine(L, &ar, true);
					}
				}
				else if (mNextDebugAction == eDEBUG_ACTION_STOP)
				{
                    UninitializeDebugData();

                    if (mDebugInfo.GetStartAction() == StartAttach)
                    {
                        return 0;
                    }
                    else
                    {
					    lua_pushstring(L, "@@STOP@@");
					    return 1;
                    }
				}
			}
		}

		return 0;
	}

	/////////////////////////////////////////////////////////////////////
	int DebugManager::LuaPrintHandler(lua_State* L)
	{
		int nargs = lua_gettop(L);
		std::string outVal;

		for (int i = 1; i <= nargs; ++i)
		{
			if (i != 1)
				outVal += '\t';

			lua_pushvalue(L, i);

			switch (lua_type(L, -1))
			{
			case LUA_TSTRING:
			case LUA_TNUMBER:
				{
					outVal += lua_tostring(L, -1);
					break;
				}
			case LUA_TNIL:
				{
					outVal += "nil";
					break;
				}
			case LUA_TTABLE:
			case LUA_TFUNCTION:
			case LUA_TLIGHTUSERDATA:
			case LUA_TUSERDATA:
			case LUA_TTHREAD:
				{
					char buffer[32];
					sprintf_s(buffer, sizeof(buffer), "%s: 0x%p", lua_typename(L, lua_type(L, -1)), lua_topointer(L, -1));
					outVal += buffer;
					break;
				}
			}

			lua_pop(L, 1);
		}

		outVal += '\n';

		// Send output command to debugger
		LuaOutput luaOutputCmd(outVal.c_str(), OUTPUTTYPE_LUA);
		DebugManager::GetInstancePtr()->GetRpcServer()->SendMessage(&luaOutputCmd);

		// Call the original print function
		if (mOriginalLuaPrintFct)
			mOriginalLuaPrintFct(L);

		lua_settop(L, 0);
		return 0;
	}

	/////////////////////////////////////////////////////////////////////
	int DebugManager::LuaCreateCoroutineHandler(lua_State* L)
	{
		lua_State *co = lua_newthread(L);
		luaL_argcheck(L, lua_isfunction(L, 1) && !lua_iscfunction(L, 1), 1, "Lua function expected");
		InitLuaThread(co);
		lua_pushvalue(L, 1);
		lua_xmove(L, co, 1);
		return 1;
	}

	/////////////////////////////////////////////////////////////////////
	DWORD WINAPI RunDebugger(LPVOID lpParam)
	{
        RunDebuggerStartParams startParams = *(RunDebuggerStartParams*)lpParam;
		lua_State* L = DebugManager::GetInstancePtr()->GetLuaState();

        if (lua_pcall(L, startParams.mParamCount,
                      startParams.mRetCount < 0 ? LUA_MULTRET : startParams.mRetCount,
                      startParams.mErrorHandlerIndex) != 0)
		{
			if (lua_gettop(L) > 0 && lua_type(L, -1) == LUA_TSTRING)
			{
				const char* pErrMsg = lua_tostring(L, -1);

				if (strcmp(pErrMsg, "@@STOP@@") == 0)
				{
					DebugManager::GetInstancePtr()->GetRpcServer()->Disconnect();
				}
				else
				{
					// Send memory error to client (can only be memory error here since
					// the error handler will handle all runtime errors)
					LuaError luaErrCmd(pErrMsg, LUAERROR_MEMORY);
					DebugManager::GetInstancePtr()->GetRpcServer()->SendMessage(&luaErrCmd);
				}
			}
		}

		// Removes error handler
        if (startParams.mErrorHandlerIndex > 0)
        {
            lua_remove(L, startParams.mErrorHandlerIndex);
        }

		return 0;
	}
}