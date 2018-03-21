#ifndef DEBUGGING_RUNSCRIPTINVM_HPP
#define DEBUGGING_RUNSCRIPTINVM_HPP

#include "RpcCommand.hpp"

#include <boost/serialization/shared_ptr.hpp>

namespace debugging
{
	class RunScriptInVM : public RpcCommandBase, public IRPCSerializableData
	{
	public:
		RunScriptInVM();
		virtual ~RunScriptInVM();

		virtual inline IRPCSerializableData* GetData() { return this; }
		virtual void DoCommand();
		virtual RpcCommandBase* MakeNew();

		inline std::string GetScriptFileName() const { return mScriptFileName; }
		inline int GetRunToLine() const { return mRunToLine; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

	private:
		std::string mScriptFileName;
		int mRunToLine;
	};
}

#endif //DEBUGGING_RUNSCRIPTINVM_HPP