#ifndef DEBUGGING_LUAOUTPUT_HPP
#define DEBUGGING_LUAOUTPUT_HPP

#include "RpcCommand.hpp"

namespace debugging
{
	enum OutputType
	{
		OUTPUTTYPE_LUA = 0,
		OUTPUTTYPE_LUAEDIT = 1
	};

	class LuaOutput : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		LuaOutput();
		LuaOutput(const char* aOutput, OutputType aOutputType);
		virtual ~LuaOutput();

		virtual inline IRPCSerializableData* GetData() { return this; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

	private:
		int mOutputType;
		std::string mOutput;
	};
}

#endif //DEBUGGING_LUAOUTPUT_HPP