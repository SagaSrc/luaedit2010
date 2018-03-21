#ifndef DEBUGGING_DEBUGINFO_HPP
#define DEBUGGING_DEBUGINFO_HPP

#include <string>

#include "../RpcCommand.hpp"

namespace debugging
{
    enum DebugStartAction
    {
        StartProject = 0,
        StartExternalProgram,
        StartAttach
    };

	class DebugInfo : public IRPCSerializableData
	{
	public:
		DebugInfo();
        DebugInfo(DebugInfo &aCopy);

		virtual inline IRPCSerializableData* GetData() { return this; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

        DebugInfo& operator=(const DebugInfo &aSource);

        inline DebugStartAction GetStartAction() const { return (DebugStartAction)mStartAction; }
        inline std::string GetRemotePath() const { return mRemotePath; }

	private:
        int mStartAction;
		std::string mRemotePath;
	};
}

#endif //DEBUGGING_DEBUGINFO_HPP