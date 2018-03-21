#include "RpcCommand.hpp"

#include <boost/foreach.hpp>
#include <iostream>

namespace debugging
{
	RpcCommandManager* RpcCommandManager::mpInstance;

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase::RpcCommandBase(const RpcCommandBase &aCopy) :
		muiType(aCopy.muiType),
		mDataSize(aCopy.mDataSize)
	{
		mData.reset(new char[aCopy.mDataSize]);
		memcpy(mData.get(), aCopy.mData.get(), aCopy.mDataSize);
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase::RpcCommandBase(RpcCommandType auiType) :
		muiType(auiType),
		mDataSize(0)
	{
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase::~RpcCommandBase()
	{
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::DeserializeCommand(const char* aData)
	{
		int offset = 0;
		GetData()->DeserializeData(aData, offset);
		mDataSize = offset;
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::SerializeCommand()
	{
		int offset = 0;
		mDataSize = GetData()->GetSerializedDataSize();
		mData.reset(new char[mDataSize]);
		GetData()->SerializeData(mData.get(), offset);
	}

	/////////////////////////////////////////////////////////////////////
	int RpcCommandBase::GetIntegerDataSize()
	{
		return sizeof(int);
	}

	/////////////////////////////////////////////////////////////////////
	int RpcCommandBase::GetBooleanDataSize()
	{
		return sizeof(bool);
	}

	/////////////////////////////////////////////////////////////////////
	int RpcCommandBase::GetStringDataSize(std::string str)
	{
		return GetIntegerDataSize() + str.length();
	}

	/////////////////////////////////////////////////////////////////////
	int RpcCommandBase::GetComplexDataSize(const IRPCSerializableData* complexData)
	{
		return complexData->GetSerializedDataSize();
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::DeserializeInteger(const char* data, int &offset, int &integer)
	{
		memcpy(&integer, &data[offset], GetIntegerDataSize());
		offset += GetIntegerDataSize();
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::DeserializeBoolean(const char* data, int &offset, bool &boolean)
	{
		memcpy(&boolean, &data[offset], GetBooleanDataSize());
		offset += GetBooleanDataSize();
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::DeserializeString(const char* data, int &offset, std::string &str)
	{
		int strLen = 0;
		DeserializeInteger(data, offset, strLen);
		boost::scoped_ptr<char> buffer(new char[strLen + 1]);
		memcpy(buffer.get(), &data[offset], strLen);
		offset += strLen;
		buffer.get()[strLen] = '\0';
		str = buffer.get();
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::DeserializeComplexData(const char* data, int &offset, IRPCSerializableData* complexData)
	{
		complexData->DeserializeData(data, offset);
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::SerializeInteger(char* data, int &offset, int integer)
	{
		memcpy(&data[offset], &integer, GetIntegerDataSize());
		offset += GetIntegerDataSize();
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::SerializeBoolean(char* data, int &offset, bool boolean)
	{
		memcpy(&data[offset], &boolean, GetBooleanDataSize());
		offset += GetBooleanDataSize();
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::SerializeString(char* data, int &offset, std::string str)
	{
		int strLen = str.length();
		SerializeInteger(data, offset, strLen);
		memcpy(&data[offset], str.c_str(), strLen);
		offset += strLen;
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandBase::SerializeComplexData(char* data, int &offset, const IRPCSerializableData* complexData)
	{
		complexData->SerializeData(data, offset);
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandManager::RpcCommandManager() :
		mIsInitialized(true)
	{
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandManager::~RpcCommandManager()
	{
		mIsInitialized = false;
	}

    /////////////////////////////////////////////////////////////////////
    RpcCommandManager* RpcCommandManager::GetInstancePtr()
    {
        if (!mpInstance)
        {
            mpInstance = new RpcCommandManager();
            ScheduleForDestruction(RpcCommandManager::Destroy);
        }

        return mpInstance;
    }

    void RpcCommandManager::ScheduleForDestruction(void (*pFun)())
    {
        std::atexit(pFun);
    }

    /////////////////////////////////////////////////////////////////////
    void RpcCommandManager::Destroy()
    {
        if (mpInstance)
        {
            delete mpInstance;
            mpInstance = NULL;
        }
    }

	/////////////////////////////////////////////////////////////////////
	void RpcCommandManager::UnRegisterCommandByType(RpcCommandType auiType)
	{
		mCommandsByType.erase(mCommandsByType.find(auiType));
	}

	/////////////////////////////////////////////////////////////////////
	void RpcCommandManager::RegisterCommandByType(RpcCommandType auiType, RpcCommandBase* aCmd)
	{
        mCommandsByType[auiType] = aCmd; //.insert(std::make_pair(auiType, apCmd));
	}

	/////////////////////////////////////////////////////////////////////
	RpcCommandBase* RpcCommandManager::CreateCommandByType(RpcCommandType auiType)
	{
		CommandsByTypeMap::iterator it = mCommandsByType.find(auiType);
		return it->second->MakeNew();
	}
}