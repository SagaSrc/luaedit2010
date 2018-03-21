#ifndef DEBUGGING_UPDATEBREAKPOINTS_HPP
#define DEBUGGING_UPDATEBREAKPOINTS_HPP

#include "RpcCommand.hpp"
#include "DebugManager/Breakpoint.hpp"

#include <vector>
#include <map>

namespace debugging
{
	class UpdateBreakpoints : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		UpdateBreakpoints();
		virtual ~UpdateBreakpoints();

		virtual inline IRPCSerializableData* GetData() { return this; }

		virtual int GetSerializedDataSize() const;
		virtual void DeserializeData(const char* data, int &offset);
		virtual void SerializeData(char* data, int &offset) const;

		virtual void DoCommand();
		virtual RpcCommandBase* MakeNew();

	private:
		typedef std::vector<Breakpoint> BreakpointVec;
		BreakpointVec mBreakpoints;
	};
}

#endif //DEBUGGING_UPDATEBREAKPOINTS_HPP