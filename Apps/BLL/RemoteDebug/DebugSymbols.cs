using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using LuaEdit.Rpc;
using LuaInterface;

namespace LuaEdit.LuaEditDebugger
{
    public class LuaThread : IRPCSerializableData
    {
        #region Members

        public int ThreadID;
        public LuaCall CallStackTopCall;

        #endregion

        #region Constructors

        public LuaThread()
        {
        }

        public LuaThread(int threadID, LuaCall callStackTopCall)
        {
            ThreadID = threadID;
            CallStackTopCall = callStackTopCall;
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetComplexDataSize(CallStackTopCall);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            RPCCommand.DeserializeInteger(data, ref offset, ref ThreadID);
            IRPCSerializableData callStackTopCall = null;
            RPCCommand.DeserializeComplexData(data, ref offset, ref callStackTopCall, typeof(LuaCall));
            CallStackTopCall = callStackTopCall as LuaCall;
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            RPCCommand.SerializeInteger(ref data, ref offset, ThreadID);
            RPCCommand.SerializeComplexData(ref data, ref offset, CallStackTopCall);
        }

        #endregion
    }

    public class LuaVariable : IRPCSerializableData
    {
        #region Members

        public const string TemporaryName = "(*temporary)";
        public const string TableBegin = "{";
        public const string TableEnd = "}";
        public const string UndifinedTable = "{...}";
        public const string EmptyTable = "{}";

        public string FullNameIn;
        public string FullNameOut;
        public string Name;
        public string Value;
        public LuaTypes Type;
        public int Index;
        public bool IsDirty;
        public LuaVariable[] SubValues;

        #endregion

        #region Constructors

        public LuaVariable()
        {
            IsDirty = false;
        }

        public LuaVariable(string fullNameIn, string fullNameOut, string name, string value, string type, LuaVariable[] subValues)
            : this()
        {
            FullNameOut = fullNameOut;
            FullNameIn = fullNameIn;
            Name = name;
            Value = value;
            Type = (LuaTypes)Enum.Parse(typeof(LuaTypes), type, true);
            SubValues = subValues;
        }

        public LuaVariable(string fullNameIn, string fullNameOut, string name, string value, string type)
            : this(fullNameIn, fullNameOut, name, value, type, type == LuaTypes.LUA_TTABLE.ToString() ? new LuaVariable[] { } : null)
        {
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += RPCCommand.GetStringDataSize(FullNameIn);
            totalSize += RPCCommand.GetStringDataSize(FullNameOut);
            totalSize += RPCCommand.GetStringDataSize(Name);
            totalSize += RPCCommand.GetStringDataSize(Value);
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetIntegerDataSize();
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            RPCCommand.DeserializeString(data, ref offset, ref FullNameIn);
            RPCCommand.DeserializeString(data, ref offset, ref FullNameOut);
            RPCCommand.DeserializeString(data, ref offset, ref Name);
            RPCCommand.DeserializeString(data, ref offset, ref Value);
            int typeVal = 0;
            RPCCommand.DeserializeInteger(data, ref offset, ref typeVal);
            Type = (LuaTypes)typeVal;
            RPCCommand.DeserializeInteger(data, ref offset, ref Index);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            RPCCommand.SerializeString(ref data, ref offset, FullNameIn);
            RPCCommand.SerializeString(ref data, ref offset, FullNameOut);
            RPCCommand.SerializeString(ref data, ref offset, Name);
            RPCCommand.SerializeString(ref data, ref offset, Value);
            RPCCommand.SerializeInteger(ref data, ref offset, (int)Type);
            RPCCommand.SerializeInteger(ref data, ref offset, Index);
        }

        public string GetPrettyPrintValue()
        {
            return ValueToString(this);
        }

        public static string ValueToString(LuaVariable luaVar)
        {
            string result = luaVar.Value.ToString();

            // Special Handling for tables and strings
            switch (luaVar.Type)
            {
                case LuaTypes.LUA_TTABLE:
                    {
                        if (luaVar.Value == string.Empty || (luaVar.Value != string.Empty && luaVar.Value[0] != '{'))
                        {
                            result = UndifinedTable;
                        }
                        else
                        {
                            result = TableBegin;

                            foreach (LuaVariable luaVarSubVal in luaVar.SubValues)
                            {
                                if (result != TableBegin)
                                {
                                    result += ", ";
                                }

                                result += string.Format("{0}={1}", luaVarSubVal.Name, luaVarSubVal.Value);
                            }
                        }

                        break;
                    }
                case LuaTypes.LUA_TSTRING:
                    {
                        result = string.Format("\"{0}\"", luaVar.Value);
                        break;
                    }
            }

            return result;
        }

        #endregion
    }

    public class LuaCall : IRPCSerializableData
    {
        #region Members

        public string FileName;
        public string FunctionName;
        public int FunctionLineCall;
        public string FunctionSource;
        public string ParamString;
        public int LineCalled;
        public bool IsLineCall;

        #endregion

        #region Constructors

        public LuaCall()
        {
        }

        public LuaCall(string fileName, string functionName, int functionLineCall, string functionSource,
                       string paramString, int lineCalled, bool isLineCall) :
            this()
        {
            FileName = fileName;
            FunctionName = functionName;
            FunctionLineCall = functionLineCall;
            FunctionSource = functionSource;
            ParamString = paramString;
            LineCalled = lineCalled;
            IsLineCall = isLineCall;
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += RPCCommand.GetStringDataSize(FileName);
            totalSize += RPCCommand.GetStringDataSize(FunctionName);
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetStringDataSize(FunctionSource);
            totalSize += RPCCommand.GetStringDataSize(ParamString);
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetBooleanDataSize();
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            RPCCommand.DeserializeString(data, ref offset, ref FileName);
            RPCCommand.DeserializeString(data, ref offset, ref FunctionName);
            RPCCommand.DeserializeInteger(data, ref offset, ref FunctionLineCall);
            RPCCommand.DeserializeString(data, ref offset, ref FunctionSource);
            RPCCommand.DeserializeString(data, ref offset, ref ParamString);
            RPCCommand.DeserializeInteger(data, ref offset, ref LineCalled);
            RPCCommand.DeserializeBoolean(data, ref offset, ref IsLineCall);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            RPCCommand.SerializeString(ref data, ref offset, FileName);
            RPCCommand.SerializeString(ref data, ref offset, FunctionName);
            RPCCommand.SerializeInteger(ref data, ref offset, FunctionLineCall);
            RPCCommand.SerializeString(ref data, ref offset, FunctionSource);
            RPCCommand.SerializeString(ref data, ref offset, ParamString);
            RPCCommand.SerializeInteger(ref data, ref offset, LineCalled);
            RPCCommand.SerializeBoolean(ref data, ref offset, IsLineCall);
        }

        #endregion
    };
}
