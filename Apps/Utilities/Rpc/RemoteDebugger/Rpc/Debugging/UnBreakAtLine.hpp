#ifndef DEBUGGING_UNBREAKATLINE_HPP
#define DEBUGGING_UNBREAKATLINE_HPP

#include "RpcCommand.hpp"
#include "DebugManager/Breakpoint.hpp"

#include <vector>

namespace debugging
{
	class UnBreakAtLine : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		UnBreakAtLine();
		UnBreakAtLine(std::string aFileName, int aLine);
		virtual ~UnBreakAtLine();

		virtual inline IRPCSerializableData* GetData() { return this; }

		virtual int GetSerializedDataSize() const;
		virtual void DeserializeData(const char* data, int &offset);
		virtual void SerializeData(char* data, int &offset) const;

	protected:
		std::string mFileName;
		int mLine;
	};
}

#endif //DEBUGGING_UNBREAKATLINE_HPP