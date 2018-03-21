#include "LuaError.hpp"

#include "DebugManager/DebugManager.hpp"

namespace debugging
{
	/////////////////////////////////////////////////////////////////////
	LuaError::LuaError() :
		RpcCommandBase(eRPC_COMMANDTYPE_RUNTIMEERROR)
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaError::LuaError(std::string aFullMsg, ErrorType aErrType) :
		RpcCommandBase(eRPC_COMMANDTYPE_RUNTIMEERROR),
		mErrMsg(aFullMsg),
		mFileName(""),
		mErrorType((int)aErrType),
		mLine(-1)
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaError::LuaError(std::string aMsg, std::string aFileName, int aLine, ErrorType aErrType) :
		RpcCommandBase(eRPC_COMMANDTYPE_RUNTIMEERROR),
		mErrMsg(aMsg),
		mFileName(aFileName),
		mErrorType((int)aErrType),
		mLine(aLine)
	{
	}

	/////////////////////////////////////////////////////////////////////
	LuaError::~LuaError()
	{
	}

	/////////////////////////////////////////////////////////////////////
	int LuaError::GetSerializedDataSize() const
	{
		int totalSize = 0;
		totalSize += GetStringDataSize(mErrMsg);
		totalSize += GetStringDataSize(mFileName);
		totalSize += GetIntegerDataSize();
		totalSize += GetIntegerDataSize();
		return totalSize;
	}

	/////////////////////////////////////////////////////////////////////
	void LuaError::DeserializeData(const char* data, int &offset)
	{
		DeserializeString(data, offset, mErrMsg);
		DeserializeString(data, offset, mFileName);
		DeserializeInteger(data, offset, mLine);
		DeserializeInteger(data, offset, mErrorType);
	}

	/////////////////////////////////////////////////////////////////////
	void LuaError::SerializeData(char* data, int &offset) const
	{
		SerializeString(data, offset, mErrMsg);
		SerializeString(data, offset, mFileName);
		SerializeInteger(data, offset, mLine);
		SerializeInteger(data, offset, mErrorType);
	}

	/////////////////////////////////////////////////////////////////////
	std::string LuaError::GetFileNameFromErrorMsg(std::string aFullMsg)
	{
		// Expected error message layout to be one of these cases:
		//
		//   -  [string <StringName>]:<LineNumber>:<ErrorMsg>
		//   -  <FileName>:<LineNumber>:<ErrorMsg>
		//
		if (aFullMsg.length() > 2)
		{
			if (aFullMsg[1] == ':')
			{
				// Found drive indicator, we'll start searching for ':' passed that
				int driveSepIndex = 2;
				int endIndex = aFullMsg.find(':', driveSepIndex);

				if (endIndex != std::string::npos)
				{
					return aFullMsg.substr(0, endIndex);
				}
			}
			else
			{
				// Search for [string ...] part of the error message
				int endIndex = 0;
				int stringMarker = aFullMsg.find("[string");

				if (stringMarker != std::string::npos)
				{
					endIndex = aFullMsg.find(']', stringMarker);

					if (endIndex != std::string::npos)
					{
						return aFullMsg.substr(stringMarker, endIndex);
					}
				}
			}
		}

		return "";
	}

	/////////////////////////////////////////////////////////////////////
	int LuaError::GetLineFromErrorMsg(std::string aFullMsg)
	{
		// Expected error message layout to be one of these cases:
		//
		//   -  [string <StringName>]:<LineNumber>:<ErrorMsg>
		//   -  <FileName>:<LineNumber>:<ErrorMsg>
		//
		if (aFullMsg.length() > 2)
		{
			int startIndex = 0;

			if (aFullMsg[1] == ':')
			{
				// Found drive indicator, we'll start searching for ':' passed that
				startIndex = 2;
			}
			else
			{
				// Search for [string ...] part of the error message
				int stringMarker = aFullMsg.find("[string");
				if (stringMarker != std::string::npos)
				{
					startIndex = aFullMsg.find(']', stringMarker);
				}
			}

			// Search for ':' marker for line number
			int sepIndex = (int)aFullMsg.find(':', startIndex);
			
			if (sepIndex != std::string::npos)
			{
				int endIndex = (int)aFullMsg.find(':', sepIndex + 1);

				if (endIndex != std::string::npos)
				{
					return atoi(aFullMsg.substr(sepIndex + 1, endIndex - sepIndex).c_str());
				}
			}
		}

		return 0;
	}

	/////////////////////////////////////////////////////////////////////
	std::string LuaError::GetMsgFromErrorMsg(std::string aFullMsg)
	{
		// Expected error message layout to be one of these cases:
		//
		//   -  [string <StringName>]:<LineNumber>:<ErrorMsg>
		//   -  <FileName>:<LineNumber>:<ErrorMsg>
		//
		if (aFullMsg.length() > 2)
		{
			int startIndex = 0;

			if (aFullMsg[1] == ':')
			{
				// Found drive indicator, we'll start searching for ':' passed that
				startIndex = 2;
			}
			else
			{
				// Search for [string ...] part of the error message
				int stringMarker = aFullMsg.find("[string");
				if (stringMarker != std::string::npos)
				{
					startIndex = aFullMsg.find(']', stringMarker + 7); // 7 == strlen("[string")
				}
			}

			int sepIndex = (int)aFullMsg.find(':', startIndex);
			
			if (sepIndex != std::string::npos)
			{
				sepIndex = (int)aFullMsg.find(':', sepIndex + 1);

				if (sepIndex != std::string::npos)
				{
					return aFullMsg.substr(sepIndex + 2);
				}
			}
		}

		return "";
	}
}