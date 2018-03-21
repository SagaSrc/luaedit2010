#ifndef DEBUGGING_LUAVARIABLE_HPP
#define DEBUGGING_LUAVARIABLE_HPP

#include <string>

#include "../RpcCommand.hpp"

#include <lua.hpp>

namespace debugging
{
	class LuaVariable : public IRPCSerializableData
	{
	public:
		LuaVariable();
		LuaVariable(const LuaVariable &aCopy);
        LuaVariable(std::string aFullNameIn, std::string aFullNameOut, std::string aName,
                    std::string aValue, int aType, int aIndex = -1);

		virtual inline IRPCSerializableData* GetData() { return this; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

		static const char* GetStringType(int aType);

        inline std::string GetFullNameIn() const { return mFullNameIn; }
        inline std::string GetFullNameOut() const { return mFullNameOut; }
		inline std::string GetName() const { return mName; }
		inline std::string GetValue() const { return mValue; }
		inline int GetType() const { return mType; }
        inline int GetIndex() const { return mIndex; }

	private:
        std::string mFullNameIn;
        std::string mFullNameOut;
		std::string mName;
		std::string mValue;
		int mType;
        int mIndex;
	};

	typedef std::vector<LuaVariable> LuaVariableList;
}

#endif //DEBUGGING_LUAVARIABLE_HPP