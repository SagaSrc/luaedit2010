#ifndef DEBUGGING_DEBUGACTION_HPP
#define DEBUGGING_DEBUGACTION_HPP

#include "RpcCommand.hpp"

#include <boost/serialization/shared_ptr.hpp>

namespace debugging
{
	class DebugActionCmd : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		DebugActionCmd();
		virtual ~DebugActionCmd();

		virtual inline IRPCSerializableData* GetData() { return this; }

		virtual int GetSerializedDataSize() const;
		virtual void DeserializeData(const char* data, int &offset);
		virtual void SerializeData(char* data, int &offset) const;

		virtual void DoCommand();
		virtual RpcCommandBase* MakeNew();

		inline int GetDebugAction() const { return mDebugAction; }

	private:
		int mDebugAction;
	};
}

#endif //DEBUGGING_DEBUGACTION_HPP