#include "LuaVariable.hpp"

#include <sstream>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	LuaVariable::LuaVariable()
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaVariable::LuaVariable(const LuaVariable &aCopy) :
        mFullNameIn(aCopy.mFullNameIn),
        mFullNameOut(aCopy.mFullNameOut),
		mName(aCopy.mName),
		mValue(aCopy.mValue),
		mType(aCopy.mType),
        mIndex(aCopy.mIndex)
	{
	}

	/////////////////////////////////////////////////////////////////////
    LuaVariable::LuaVariable(std::string aFullNameIn, std::string aFullNameOut, std::string aName, std::string aValue, int aType, int aIndex /*= -1*/) :
        mFullNameIn(aFullNameIn),
        mFullNameOut(aFullNameOut),
		mName(aName),
		mValue(aValue),
		mType(aType),
        mIndex(aIndex)
	{
	}

	/////////////////////////////////////////////////////////////////////
	int LuaVariable::GetSerializedDataSize() const
	{
		int totalSize = 0;
        totalSize += RpcCommandBase::GetStringDataSize(mFullNameIn);
        totalSize += RpcCommandBase::GetStringDataSize(mFullNameOut);
		totalSize += RpcCommandBase::GetStringDataSize(mName);
		totalSize += RpcCommandBase::GetStringDataSize(mValue);
		totalSize += RpcCommandBase::GetIntegerDataSize();
        totalSize += RpcCommandBase::GetIntegerDataSize();
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void LuaVariable::DeserializeData(const char* data, int &offset)
	{
        RpcCommandBase::DeserializeString(data, offset, mFullNameIn);
        RpcCommandBase::DeserializeString(data, offset, mFullNameOut);
		RpcCommandBase::DeserializeString(data, offset, mName);
		RpcCommandBase::DeserializeString(data, offset, mValue);
		RpcCommandBase::DeserializeInteger(data, offset, mType);
        RpcCommandBase::DeserializeInteger(data, offset, mIndex);
	}

	/////////////////////////////////////////////////////////////////////
	void LuaVariable::SerializeData(char* data, int &offset) const
	{
        RpcCommandBase::SerializeString(data, offset, mFullNameIn);
        RpcCommandBase::SerializeString(data, offset, mFullNameOut);
		RpcCommandBase::SerializeString(data, offset, mName);
		RpcCommandBase::SerializeString(data, offset, mValue);
		RpcCommandBase::SerializeInteger(data, offset, mType);
        RpcCommandBase::SerializeInteger(data, offset, mIndex);
	}

	/////////////////////////////////////////////////////////////////////
	const char* LuaVariable::GetStringType(int aType)
	{
		switch (aType)
		{
		case LUA_TNIL:
			{
				return "LUA_TNIL";
				break;
			}
		case LUA_TBOOLEAN:
			{
				return "LUA_TBOOLEAN";
				break;
			}
		case LUA_TLIGHTUSERDATA:
			{
				return "LUA_TLIGHTUSERDATA";
				break;
			}
		case LUA_TNUMBER:
			{
				return "LUA_TNUMBER";
				break;
			}
		case LUA_TSTRING:
			{
				return "LUA_TSTRING";
				break;
			}
		case LUA_TTABLE:
			{
				return "LUA_TTABLE";
				break;
			}
		case LUA_TFUNCTION:
			{
				return "LUA_TFUNCTION";
				break;
			}
		case LUA_TUSERDATA:
			{
				return "LUA_TUSERDATA";
				break;
			}
		case LUA_TTHREAD:
			{
				return "LUA_TTHREAD";
				break;
			}
		default:
			{
				return "LUA_TNONE";
				break;
			}
		}
	}
}