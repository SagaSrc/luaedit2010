#ifndef DEBUGGING_RPCCOMMAND_HPP
#define DEBUGGING_RPCCOMMAND_HPP

#include "../../Network/TcpSocket.hpp"

#include <boost/scoped_ptr.hpp>
#include <boost/noncopyable.hpp>
#include <map>
#include <vector>

namespace debugging
{
	enum RpcCommandType
	{
		eRPC_COMMANDTYPE_INVALID = 0,
		eRPC_COMMANDTYPE_START = 1000,
		eRPC_COMMANDTYPE_RUNSCRIPT,
		eRPC_COMMANDTYPE_UPDATEBREAKPOINTS,
		eRPC_COMMANDTYPE_BREAKATLINE,
		eRPC_COMMANDTYPE_UNBREAKATLINE,
		eRPC_COMMANDTYPE_DEBUGACTION,
		eRPC_COMMANDTYPE_RUNTIMEERROR,
		eRPC_COMMANDTYPE_UPDATETABLEDETAILS,
		eRPC_COMMANDTYPE_LUAOUTPUT,
        eRPC_COMMANDTYPE_SETLUAVARIABLE,
        eRPC_COMMANDTYPE_UPDATEDEBUGINFO,
        eRPC_COMMANDTYPE_RUNTO
	};

	class IRPCSerializableData
	{
	public:
		virtual int GetSerializedDataSize() const = 0;
		virtual void DeserializeData(const char* data, int &offset) = 0;
		virtual void SerializeData(char* data, int &offset) const = 0;
	};

	class RpcCommandBase
	{
	public:
		RpcCommandBase(const RpcCommandBase &aCopy);
		RpcCommandBase(RpcCommandType auiType);
		virtual ~RpcCommandBase();

		void DeserializeCommand(const char* aData);
		void SerializeCommand();

		inline RpcCommandType GetRpcCommandType() const { return muiType; }

		virtual inline void DoCommand() {}
		virtual inline IRPCSerializableData* GetData() { return NULL; }
		virtual inline const char* GetRawData() { return mData.get(); }
		virtual inline int GetRawDataSize() { return mDataSize; }
		virtual inline RpcCommandBase* MakeNew() { return NULL; }

		//
		// Size helpers
		//
		static int GetIntegerDataSize();
		static int GetBooleanDataSize();
		static int GetStringDataSize(std::string str);
		static int GetComplexDataSize(const IRPCSerializableData* complexData);

		//
		// Deserialization helpers
		//
		static void DeserializeInteger(const char* data, int &offset, int &integer);
		static void DeserializeBoolean(const char* data, int &offset, bool &boolean);
		static void DeserializeString(const char* data, int &offset, std::string &str);
		static void DeserializeComplexData(const char* data, int &offset, IRPCSerializableData* complexData);

		//
		// Serialization helpers
		//
		static void SerializeInteger(char* data, int &offset, const int integer);
		static void SerializeBoolean(char* data, int &offset, const bool boolean);
		static void SerializeString(char* data, int &offset, const std::string str);
		static void SerializeComplexData(char* data, int &offset, const IRPCSerializableData* complexData);

	private:
		RpcCommandType muiType;
		boost::scoped_ptr<char> mData;
		int mDataSize;
	};

	class RpcCommandManager : private boost::noncopyable
	{
	public:
		static RpcCommandManager* GetInstancePtr();
        
		void RegisterCommandByType(RpcCommandType auiType, RpcCommandBase* aCmd);
		void UnRegisterCommandByType(RpcCommandType auiType);
		RpcCommandBase* CreateCommandByType(RpcCommandType auiType);

	private:
		RpcCommandManager();
		~RpcCommandManager();

		static RpcCommandManager* mpInstance;

        static void ScheduleForDestruction(void (*pFun)());
        static void Destroy();

		typedef std::map<RpcCommandType, RpcCommandBase*> CommandsByTypeMap;
		bool mIsInitialized;
		CommandsByTypeMap mCommandsByType;
	};
}

#endif //DEBUGGING_RPCCOMMAND_HPP