#include "UpdateBreakpoints.hpp"

#include "DebugManager/DebugManager.hpp"

#include <iostream>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	UpdateBreakpoints::UpdateBreakpoints() :
		RpcCommandBase(eRPC_COMMANDTYPE_UPDATEBREAKPOINTS)
	{
	}

	/////////////////////////////////////////////////////////////////////
	UpdateBreakpoints::~UpdateBreakpoints()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int UpdateBreakpoints::GetSerializedDataSize() const
	{
		//
		// Breakpoints
		//
		int totalSize = GetIntegerDataSize();

		BOOST_FOREACH(Breakpoint bp, mBreakpoints)
		{
			totalSize += GetComplexDataSize(&bp);
		}

		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void UpdateBreakpoints::DeserializeData(const char* data, int &offset)
	{
		//
		// Breakpoints
		//
		int bpArrayLength = 0;
		DeserializeInteger(data, offset, bpArrayLength);

		for (int x = 0; x < bpArrayLength; ++x)
		{
			Breakpoint bp;
			DeserializeComplexData(data, offset, &bp);
			mBreakpoints.push_back(bp);
		}
	}

	/////////////////////////////////////////////////////////////////////
	void UpdateBreakpoints::SerializeData(char* data, int &offset) const
	{
		//
		// Breakpoints
		//
		SerializeInteger(data, offset, mBreakpoints.size());

		BOOST_FOREACH(Breakpoint bp, mBreakpoints)
		{
			SerializeComplexData(data, offset, &bp);
		}
	}

	/////////////////////////////////////////////////////////////////////
	void UpdateBreakpoints::DoCommand()
	{
        // Update breakpoint hit counts with current values
        BOOST_FOREACH(Breakpoint& bp, mBreakpoints)
		{
            Breakpoint bpToCopy = DebugManager::GetInstancePtr()->GetBreakpointAtLine(bp.GetFileName(), bp.GetLine());
            bp.SetCurrentHitCount(bpToCopy.GetCurrentHitCount());
        }

        // Clear breakpoint list and rebuild it from scratch
		DebugManager::GetInstancePtr()->ClearBreakpoints();

		BreakpointVec::iterator itStart = mBreakpoints.begin();
		BreakpointVec::iterator itEnd = mBreakpoints.end();

		BOOST_FOREACH(Breakpoint bp, mBreakpoints)
		{
			DebugManager::GetInstancePtr()->AddBreakpoint(bp);
		}
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase* UpdateBreakpoints::MakeNew()
	{
		return new UpdateBreakpoints();
	}
}