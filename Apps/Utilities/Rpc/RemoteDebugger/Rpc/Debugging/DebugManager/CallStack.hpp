#ifndef DEBUGGING_CALLSTACK_HPP
#define DEBUGGING_CALLSTACK_HPP

#include <sstream>
#include <string>
#include <vector>

#include "../RpcCommand.hpp"

#include <boost/foreach.hpp>

namespace debugging
{
	class CallStackItem : public IRPCSerializableData
	{
	public:
		CallStackItem();
		CallStackItem(const CallStackItem &aCopy);
		CallStackItem(int aLineCall, std::string aFileName, std::string aFuncName, std::string aSource,
                      std::string aParams, int aLineCalled, bool aIsLineCall);

		virtual inline IRPCSerializableData* GetData() { return this; }

		virtual int GetSerializedDataSize() const;
		virtual void DeserializeData(const char* data, int &offset);
		virtual void SerializeData(char* data, int &offset) const;

		inline int GetLineCall() const { return mLineCall; }
		inline std::string GetFileName() const { return mFileName; }
		inline std::string GetFuncName() const { return mFuncName; }
		inline std::string GetSource() const { return mSource; }
		inline std::string GetParamStr() const { return mParams; }
		inline bool IsLineCall() const { return mIsLineCall; }

	private:
		std::string mFileName;
		std::string mFuncName;
		int mLineCall;
		std::string mSource;
		std::string mParams;
        int mLineCalled;
		bool mIsLineCall;
	};

	class CallStack
	{
	public:
		CallStack();
		CallStack(const CallStack &aCopy);

		inline int GetCurrentStackLevel() const { return mCurrentStackLevel; }
		inline int GetStackTop() const { return (int)mCallStack.size(); }
		inline std::vector<CallStackItem> GetCallStack() const { return mCallStack; }

		void PushCallStackItem(int aLineCall, std::string aFileName, std::string aFuncName, std::string aSource,
                               std::string aParams, int aLineCalled, bool aIsLineCall);
		void PopCallStackItem();
		void ClearCallStack();

	private:
		std::vector<CallStackItem> mCallStack;
		int mCurrentStackLevel;
	};
}

#endif //DEBUGGING_CALLSTACK_HPP