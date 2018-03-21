#include "UpdateTableDetails.hpp"

#include "DebugManager/DebugManager.hpp"

#include <boost/foreach.hpp>

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	UpdateTableDetails::UpdateTableDetails() :
		RpcCommandBase(eRPC_COMMANDTYPE_UPDATETABLEDETAILS)
	{
	}

	/////////////////////////////////////////////////////////////////////
	UpdateTableDetails::UpdateTableDetails(std::string aTableNameIn, std::string aTableNameOut,
                                           bool aIsLocal, std::vector<LuaVariable> aSubValues) :
		RpcCommandBase(eRPC_COMMANDTYPE_UPDATETABLEDETAILS),
		mTableNameIn(aTableNameIn),
        mTableNameOut(aTableNameOut),
		mIsLocal(aIsLocal),
		mSubValues(aSubValues.begin(), aSubValues.end())
	{
	}

	/////////////////////////////////////////////////////////////////////
	UpdateTableDetails::~UpdateTableDetails()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int UpdateTableDetails::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetStringDataSize(mTableNameIn);
        totalSize += GetStringDataSize(mTableNameOut);
		totalSize += GetBooleanDataSize();

		//
		// Sub-Values
		//
		totalSize += GetIntegerDataSize();

		BOOST_FOREACH(LuaVariable luaVar, mSubValues)
		{
			totalSize += GetComplexDataSize(&luaVar);
		}

		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void UpdateTableDetails::DeserializeData(const char* data, int &offset)
	{
		DeserializeString(data, offset, mTableNameIn);
        DeserializeString(data, offset, mTableNameOut);
		DeserializeBoolean(data, offset, mIsLocal);

		//
		// Sub-Values
		//
		int subValArrLength = 0;
		DeserializeInteger(data, offset, subValArrLength);

		for (int x = 0; x < subValArrLength; ++x)
		{
			LuaVariable subVal;
			DeserializeComplexData(data, offset, &subVal);
			mSubValues.push_back(subVal);
		}
	}

    /////////////////////////////////////////////////////////////////////
    void UpdateTableDetails::SerializeData(char* data, int &offset) const
    {
	    SerializeString(data, offset, mTableNameIn);
        SerializeString(data, offset, mTableNameOut);
	    SerializeBoolean(data, offset, mIsLocal);

	    //
	    // Sub-Values
	    //
        SerializeInteger(data, offset, mSubValues.size());

	    BOOST_FOREACH(LuaVariable luaVar, mSubValues)
	    {
		    SerializeComplexData(data, offset, &luaVar);
	    }
    }

	/////////////////////////////////////////////////////////////////////
	void UpdateTableDetails::DoCommand()
	{
        static char TableSepBegin = '[';
        static char TableSepEnd = ']';

		std::string tableName = GetTableNameIn();
		std::vector<LuaVariable> tableDetails;
		lua_State* L = DebugManager::GetInstancePtr()->GetLuaState();

		// Ge the first level of table
        if (IsLocal())
        {
            int rootEndIndex = tableName.find_first_of('[');
            int rootLocalIndex = -1;

            if (rootEndIndex != std::string::npos)
            {
                rootLocalIndex = atoi(tableName.substr(0, rootEndIndex).c_str());
            }
            else
            {
                rootLocalIndex = atoi(tableName.c_str());
            }

            DebugManager::GetInstancePtr()->GetLocal(rootLocalIndex);
        }
        else
        {
            DebugManager::GetInstancePtr()->GetGlobal(tableName);
        }

		if (lua_type(L, -1) == LUA_TTABLE)
		{
			size_t sepIndex = tableName.find_first_of(TableSepBegin);
            size_t sepEndIndex = tableName.find_first_of(TableSepEnd);

            while (sepIndex != std::string::npos && sepEndIndex != std::string::npos)
            {
	            std::string currentTableName = tableName.substr(sepIndex + 1, sepEndIndex - sepIndex - 1);

                if (currentTableName.size() > 0 && (currentTableName[0] == '"' || currentTableName[0] == '\''))
                {
                    lua_pushstring(L, currentTableName.substr(1, currentTableName.length() - 2).c_str());
                }
                else
                {
                    lua_pushinteger(L, atoi(currentTableName.c_str()));
                }

	            lua_gettable(L, -2);

                if (lua_type(L, -1) != LUA_TTABLE)
                {
                    break;
                }

                // Get next table level
	            sepIndex = tableName.find_first_of(TableSepBegin, sepEndIndex);
                sepEndIndex = tableName.find_first_of(TableSepEnd, sepIndex);
            }

			if (lua_type(L, -1) == LUA_TTABLE)
			{
                int tableIndex = lua_gettop(L);
                lua_pushnil(L);  // First key
                while (lua_next(L, tableIndex) != 0)
                {
                    std::string varName = "";
                    std::string fullVarNameIn = mTableNameIn;
                    std::string fullVarNameOut = mTableNameOut;

                    if (lua_type(L, -2) == LUA_TSTRING)
                    {
                        varName += "[\"";
                        varName += lua_tostring(L, -2);
                        varName += "\"]";
                    }
                    else if (lua_type(L, -2) == LUA_TNUMBER)
                    {
                        lua_pushvalue(L, -2);
                        varName += "[";
                        varName += lua_tostring(L, -1);
                        varName += "]";
                        lua_pop(L, 1);
                    }

                    fullVarNameIn += varName;
                    fullVarNameOut += varName;

                    // Key is at index -2 and value is at index -1
					tableDetails.push_back(LuaVariable(fullVarNameIn, fullVarNameOut,
                                           DebugManager::GetInstancePtr()->LuaStackValueToString(L, -2),
                                           DebugManager::GetInstancePtr()->LuaStackValueToString(L, -1),
                                           lua_type(L, -1)));
                    // Removes value; keeps key for next iteration
                    lua_pop(L, 1);
                }

                UpdateTableDetails updateTableDetailsCmd(mTableNameIn, mTableNameOut, IsLocal(), tableDetails);
                DebugManager::GetInstancePtr()->GetRpcServer()->SendMessage(&updateTableDetailsCmd);
			}
		}
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase* UpdateTableDetails::MakeNew()
	{
		return new UpdateTableDetails();
	}
}