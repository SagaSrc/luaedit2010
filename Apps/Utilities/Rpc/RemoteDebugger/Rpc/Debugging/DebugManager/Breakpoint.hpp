#ifndef DEBUGGING_BREAKPOINT_HPP
#define DEBUGGING_BREAKPOINT_HPP

#include <string>

#include "../RpcCommand.hpp"

namespace debugging
{
    enum HitCountConditions
    {
        BreakAlways = 0,
        BreakEqualTo,
        BreakMultipleOf,
        BreakGreaterOrEqualTo
    };

	class Breakpoint : public IRPCSerializableData
	{
	public:
		Breakpoint();
		Breakpoint(const Breakpoint &aCopy);
		Breakpoint(std::string aFileName, int aLine, std::string aCondition = "",
                   HitCountConditions hitCountCondition = BreakAlways, bool aIsEnabled = true);

		virtual inline IRPCSerializableData* GetData() { return this; }

		int GetSerializedDataSize() const;
		void DeserializeData(const char* data, int &offset);
		void SerializeData(char* data, int &offset) const;

		Breakpoint& operator=(const Breakpoint &aSource);

		inline std::string GetFileName() const { return mFileName; }
		inline std::string GetCondition() const { return mCondition; }
		inline int GetLine() const { return mLine; }
		inline int GetHitCount() const { return mHitCount; }
        inline HitCountConditions GetHitCountCondition() const { return mHitCountCondition; }
		inline bool IsEnabled() const { return mIsEnabled; }
        inline int GetCurrentHitCount() const { return mCurrentHitCount; }
        inline void Hit() { ++mCurrentHitCount; }
        void SetCurrentHitCount(int aHitCount);

	private:
		std::string mFileName;
		std::string mCondition;
		int mLine;
        int mCurrentHitCount;
		int mHitCount;
        HitCountConditions mHitCountCondition;
		bool mIsEnabled;
	};
}

#endif //DEBUGGING_BREAKPOINT_HPP