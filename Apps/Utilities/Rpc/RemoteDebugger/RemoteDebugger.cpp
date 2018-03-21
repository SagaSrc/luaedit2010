// RpcServer.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#endif

#include "include/RemoteDebugger.hpp"
#include "Rpc/RpcServer.hpp"
#include "Rpc/Debugging/DebugManager/DebugManager.hpp"

rpc::RpcServer gServer;
int gServerPort;
HANDLE gServerThreadHandle = NULL;

BOOL WINAPI DllMain(HINSTANCE hInstance,DWORD fwdReason, LPVOID lpvReserved)
{
	switch(fwdReason)
	{
		case DLL_PROCESS_ATTACH:
			break;
		case DLL_THREAD_ATTACH:
			break;
		case DLL_PROCESS_DETACH:
			break;
		case DLL_THREAD_DETACH:
			break;
	}
#ifdef _DEBUG
	_CrtSetReportMode( _CRT_ERROR, _CRTDBG_MODE_DEBUG );
	_CrtSetDbgFlag( _CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF );
#endif
	return(TRUE);				// The initialization was successful, a FALSE will abort
								// the DLL attach
}

DWORD WINAPI RunServer(LPVOID lpParam)
{
	gServer.Run(gServerPort);
	return 0;
}

void StartLuaEditRemoteDebugger(int aPort /*= 32201*/, lua_State* L /*= NULL*/)
{
	if (gServerThreadHandle == NULL)
	{
		DWORD threadID;
        gServerPort = aPort;
		gServerThreadHandle = CreateThread(NULL, 0, RunServer, L, 0, &threadID);
        debugging::DebugManager::GetInstancePtr()->Initialize(&gServer, L);
	}
}

void StopLuaEditRemoteDebugger()
{
	if (gServerThreadHandle != NULL)
	{
		gServer.Stop();
		WaitForSingleObject(gServerThreadHandle, INFINITE);
		CloseHandle(gServerThreadHandle);
		gServerThreadHandle = NULL;
        debugging::DebugManager::GetInstancePtr()->Uninitialize();
	}
}

const char* GetCurrentLuaVersion()
{
    return debugging::DebugManager::GetLuaVersion();
}

bool CheckLuaScriptSyntax(const char* aScript, const char* aScriptName, char* aErrBuf, int aErrBufLen)
{
    return debugging::DebugManager::CheckLuaSyntax(aScript, aScriptName, aErrBuf, aErrBufLen);
}
