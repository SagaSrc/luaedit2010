#ifndef DEBUGGING_RUNTOLINECMD_HPP
#define DEBUGGING_RUNTOLINECMD_HPP

#include "RpcCommand.hpp"

namespace debugging
{
	class RunToLineCmd : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		RunToLineCmd();
		virtual ~RunToLineCmd();

		virtual inline IRPCSerializableData* GetData() { return this; }
		virtual void DoCommand();
		virtual RpcCommandBase* MakeNew();

		inline std::string GetFileName() const { return mFileName; }
		inline int GetRunToLine() const { return mRunToLine; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

	private:
		std::string mFileName;
		int mRunToLine;
	};
}

#endif //DEBUGGING_RUNTOLINECMD_HPP