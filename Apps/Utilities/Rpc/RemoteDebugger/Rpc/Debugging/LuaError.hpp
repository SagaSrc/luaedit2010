#ifndef DEBUGGING_LUAERROR_HPP
#define DEBUGGING_LUAERROR_HPP

#include "RpcCommand.hpp"
#include "DebugManager/Breakpoint.hpp"

#include <vector>

namespace debugging
{
	enum ErrorType
	{
		LUAERROR_SYNTAX,
		LUAERROR_RUNTIME,
		LUAERROR_MEMORY,
        LUAERROR_UNHANDLED
	};

	class LuaError : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		LuaError();
		LuaError(std::string aFullMsg, ErrorType aErrType);
		LuaError(std::string aMsg, std::string aFileName, int aLine, ErrorType aErrType);
		virtual ~LuaError();

		virtual inline IRPCSerializableData* GetData() { return this; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

	private:
		std::string mErrMsg;
		std::string mFileName;
		int mLine;
		int mErrorType;

		std::string GetFileNameFromErrorMsg(std::string aFullMsg);
		int GetLineFromErrorMsg(std::string aFullMsg);
		std::string GetMsgFromErrorMsg(std::string aFullMsg);
	};
}

#endif //DEBUGGING_LUAERROR_HPP