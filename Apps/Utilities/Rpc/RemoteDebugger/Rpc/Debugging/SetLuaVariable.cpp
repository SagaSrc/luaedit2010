#include "SetLuaVariable.hpp"

#include "DebugManager/DebugManager.hpp"

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	SetLuaVariable::SetLuaVariable() :
		RpcCommandBase(eRPC_COMMANDTYPE_SETLUAVARIABLE)
	{
	}

	/////////////////////////////////////////////////////////////////////
	SetLuaVariable::~SetLuaVariable()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int SetLuaVariable::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetComplexDataSize(&mLuaVar);
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void SetLuaVariable::DeserializeData(const char* data, int &offset)
	{
        DeserializeComplexData(data, offset, &mLuaVar);
	}

	/////////////////////////////////////////////////////////////////////
	void SetLuaVariable::SerializeData(char* data, int &offset) const
	{
		SerializeComplexData(data, offset, &mLuaVar);
	}

    /////////////////////////////////////////////////////////////////////
	void SetLuaVariable::DoCommand()
	{
		//DebugManager::GetInstancePtr()->SetNextDebugAction(GetDebugAction());

        // Determine whether the variable to set is a local or a global
        if (mLuaVar.GetIndex() >= 0)
        {
            DebugManager::GetInstancePtr()->SetLocal(mLuaVar.GetIndex(), mLuaVar);
        }
        else
        {
            // global
        }
	}

    /////////////////////////////////////////////////////////////////////
	RpcCommandBase* SetLuaVariable::MakeNew()
	{
		return new SetLuaVariable();
	}
}