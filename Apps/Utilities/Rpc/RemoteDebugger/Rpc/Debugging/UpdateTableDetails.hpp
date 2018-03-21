#ifndef DEBUGGING_UPDATETABLEDETAILS_HPP
#define DEBUGGING_UPDATETABLEDETAILS_HPP

#include <vector>

#include <boost/shared_ptr.hpp>

#include "RpcCommand.hpp"
#include "DebugManager/Breakpoint.hpp"
#include "DebugManager/LuaVariable.hpp"

namespace debugging
{
	class UpdateTableDetails : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		UpdateTableDetails();
		UpdateTableDetails(std::string aTableNameIn, std::string aTableNameOut,
                           bool aIsLocal, std::vector<LuaVariable> aSubValues);
		virtual ~UpdateTableDetails();

		virtual inline IRPCSerializableData* GetData() { return this; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

		virtual void DoCommand();
		virtual RpcCommandBase* MakeNew();

	private:
		std::string mTableNameIn;
        std::string mTableNameOut;
		bool mIsLocal;
		std::vector<LuaVariable> mSubValues;

		inline std::string GetTableNameIn() { return mTableNameIn; }
        inline std::string GetTableNameOut() { return mTableNameOut; }
		inline bool IsLocal() { return mIsLocal; }
		inline std::vector<LuaVariable> GetSubValues() { return mSubValues; }
	};
}

#endif //DEBUGGING_UPDATETABLEDETAILS_HPP