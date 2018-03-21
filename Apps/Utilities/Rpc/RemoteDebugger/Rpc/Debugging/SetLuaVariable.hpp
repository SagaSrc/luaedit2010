#ifndef DEBUGGING_SETLUAVARIABLE_HPP
#define DEBUGGING_SETLUAVARIABLE_HPP

#include "RpcCommand.hpp"
#include "DebugManager/LuaVariable.hpp"

namespace debugging
{
	class SetLuaVariable : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		SetLuaVariable();
		virtual ~SetLuaVariable();

		virtual inline IRPCSerializableData* GetData() { return this; }

		virtual int GetSerializedDataSize() const;
		virtual void DeserializeData(const char* data, int &offset);
		virtual void SerializeData(char* data, int &offset) const;

        virtual void DoCommand();
		virtual RpcCommandBase* MakeNew();

	private:
		LuaVariable mLuaVar;
	};
}

#endif //DEBUGGING_SETLUAVARIABLE_HPP