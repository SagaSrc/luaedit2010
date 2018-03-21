#ifndef DEBUGGING_UPDATEDEBUGINFO_HPP
#define DEBUGGING_UPDATEDEBUGINFO_HPP

#include "DebugManager/DebugInfo.hpp"
#include "RpcCommand.hpp"

namespace debugging
{
	class UpdateDebugInfo : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		UpdateDebugInfo();
		virtual ~UpdateDebugInfo();

		virtual inline IRPCSerializableData* GetData() { return this; }
		virtual void DoCommand();
		virtual RpcCommandBase* MakeNew();

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

	private:
		DebugInfo mDI;
	};
}

#endif //DEBUGGING_UPDATEDEBUGINFO_HPP