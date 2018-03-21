using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using LuaEdit.Win32;
using LuaEdit.Documents;
using LuaEdit.Documents.DocumentUtils;
using LuaEdit.Managers;
using LuaEdit.Rpc;
using LuaEdit.Utils;
using LuaInterface;

namespace LuaEdit.LuaEditDebugger.DebugCommands
{
    public enum RpcCommandType : int
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
    }

    public class RunToCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public string FileName;
        public int Line;

        #endregion

        #region Constructors

        public RunToCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        public RunToCommand(string fileName, int line)
            : base((int)RpcCommandType.eRPC_COMMANDTYPE_RUNTO)
        {
            this.FileName = fileName;
            this.Line = line;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetStringDataSize(FileName);
            totalSize += GetIntegerDataSize();
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeString(data, ref offset, ref FileName);
            DeserializeInteger(data, ref offset, ref Line);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeString(ref data, ref offset, this.FileName);
            SerializeInteger(ref data, ref offset, this.Line);
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_RUNTO,
                                                        typeof(RunToCommand));
        }

        #endregion
    };

    public class UpdateDebugInfoCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public DebugInfo DebugInfo;

        #endregion

        #region Constructors

        public UpdateDebugInfoCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        public UpdateDebugInfoCommand(DebugInfo di)
            : base((int)RpcCommandType.eRPC_COMMANDTYPE_UPDATEDEBUGINFO)
        {
            this.DebugInfo = di;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetComplexDataSize(this.DebugInfo);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            IRPCSerializableData temp = null;
            DeserializeComplexData(data, ref offset, ref temp, typeof(DebugInfo));
            this.DebugInfo = temp as DebugInfo;
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeComplexData(ref data, ref offset, this.DebugInfo);
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_UPDATEDEBUGINFO,
                                                        typeof(UpdateDebugInfoCommand));
        }

        #endregion
    };

    public class RunScriptInVMCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public string ScriptFileName;
        public int RunToLine;

        #endregion

        #region Constructors

        public RunScriptInVMCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        public RunScriptInVMCommand(string scriptFileName, int runToLine)
            : base((int)RpcCommandType.eRPC_COMMANDTYPE_RUNSCRIPT)
        {
            this.ScriptFileName = scriptFileName;
            this.RunToLine = runToLine;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetStringDataSize(ScriptFileName);
            totalSize += GetIntegerDataSize();
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeString(data, ref offset, ref ScriptFileName);
            DeserializeInteger(data, ref offset, ref RunToLine);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeString(ref data, ref offset, ScriptFileName);
            SerializeInteger(ref data, ref offset, RunToLine);
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_RUNSCRIPT,
                                                        typeof(RunScriptInVMCommand));
        }

        #endregion
    };

    public class UpdateBreakpointsCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public SimpleBreakpoint[] Breakpoints;

        #endregion

        #region Constructors

        public UpdateBreakpointsCommand(CommandHeader cmdHeader) :
            base(cmdHeader)
        {
        }

        public UpdateBreakpointsCommand(SimpleBreakpoint[] simpleBreakpoints) :
            base((int)RpcCommandType.eRPC_COMMANDTYPE_UPDATEBREAKPOINTS)
        {
            this.Breakpoints = simpleBreakpoints;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            return GetComplexDataArraySize(Breakpoints);
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            IRPCSerializableData[] arrBreakpoints = new IRPCSerializableData[0];
            DeserializeComplexDataArray(data, ref offset, ref arrBreakpoints, typeof(SimpleBreakpoint));
            Breakpoints = (SimpleBreakpoint[])arrBreakpoints;
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeComplexDataArray(ref data, ref offset, Breakpoints);
        }

        public override RPCCommand DoCommand()
        {
            BreakpointManager.Instance.Breakpoints.Clear();

            foreach (SimpleBreakpoint bp in this.Breakpoints)
                BreakpointManager.Instance.AddBreakpoint(bp.FileName, new Breakpoint(bp));

            return null;
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_UPDATEBREAKPOINTS,
                                                        typeof(UpdateBreakpointsCommand));
        }

        #endregion
    };

    public class UpdateLocalsCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public int StackLevel;
        public LuaVariable[] Locals;

        #endregion

        #region Constructors

        public UpdateLocalsCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        public UpdateLocalsCommand(int stackLevel, LuaVariable[] locals)
            : base((int)RpcCommandType.eRPC_COMMANDTYPE_INVALID)
        {
            this.StackLevel = stackLevel;
            this.Locals = locals;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetIntegerDataSize();
            totalSize += GetComplexDataArraySize(Locals);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeInteger(data, ref offset, ref StackLevel);
            IRPCSerializableData[] arrLocals = new IRPCSerializableData[0];
            DeserializeComplexDataArray(data, ref offset, ref arrLocals, typeof(LuaVariable));
            Locals = (LuaVariable[])arrLocals;
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeInteger(ref data, ref offset, StackLevel);
            SerializeComplexDataArray(ref data, ref offset, Locals);
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_INVALID,
                                                        typeof(UpdateLocalsCommand));
        }

        #endregion
    };

    public class UpdateTableDetailsCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public string TableNameIn;
        public string TableNameOut;
        public bool IsLocal;
        public LuaVariable[] SubValues;

        #endregion

        #region Constructors

        public UpdateTableDetailsCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        public UpdateTableDetailsCommand(string tableNameIn, string tableNameOut, bool isLocal, LuaVariable[] subValues)
            : base((int)RpcCommandType.eRPC_COMMANDTYPE_UPDATETABLEDETAILS)
        {
            this.TableNameIn = tableNameIn;
            this.TableNameOut = tableNameOut;
            this.IsLocal = isLocal;
            this.SubValues = subValues;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetStringDataSize(TableNameIn);
            totalSize += GetStringDataSize(TableNameOut);
            totalSize += GetBooleanDataSize();
            totalSize += GetComplexDataArraySize(SubValues);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeString(data, ref offset, ref TableNameIn);
            DeserializeString(data, ref offset, ref TableNameOut);
            DeserializeBoolean(data, ref offset, ref IsLocal);
            IRPCSerializableData[] arrSubValues = new IRPCSerializableData[0];
            DeserializeComplexDataArray(data, ref offset, ref arrSubValues, typeof(LuaVariable));
            SubValues = new LuaVariable[arrSubValues.Length];
            Array.Copy(arrSubValues, SubValues, arrSubValues.Length);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeString(ref data, ref offset, TableNameIn);
            SerializeString(ref data, ref offset, TableNameOut);
            SerializeBoolean(ref data, ref offset, IsLocal);
            SerializeComplexDataArray(ref data, ref offset, SubValues);
        }

        public override RPCCommand DoCommand()
        {
            foreach (LuaVariable var in SubValues)
            {
                var.IsDirty = true;
            }

            ClientDebugManager.Instance.RaiseTableDetailsUpdatedEvent(this.TableNameIn, this.TableNameOut,
                                                                      this.IsLocal, new List<LuaVariable>(SubValues));
            return null;
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_UPDATETABLEDETAILS,
                                                        typeof(UpdateTableDetailsCommand));
        }

        #endregion
    };

    public class LuaErrorCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public enum ErrorTypes : int
        {
            SyntaxError = 0,
            RuntimeError,
            MemoryError,
            Unhandled
        }

        public string Message;
        public string FileName;
        public int Line;
        public int ErrorType;

        #endregion

        #region Constructors

        public LuaErrorCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetStringDataSize(Message);
            totalSize += GetStringDataSize(FileName);
            totalSize += GetIntegerDataSize() * 2; // Line + ErrorType
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeString(data, ref offset, ref Message);
            DeserializeString(data, ref offset, ref FileName);
            DeserializeInteger(data, ref offset, ref Line);
            DeserializeInteger(data, ref offset, ref ErrorType);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeString(ref data, ref offset, Message);
            SerializeString(ref data, ref offset, FileName);
            SerializeInteger(ref data, ref offset, Line);
            SerializeInteger(ref data, ref offset, ErrorType);
        }

        public override RPCCommand DoCommand()
        {
            // Open the document and break at the current line
            if (File.Exists(this.FileName))
            {
                DocumentsManager.Instance.OpenDocument(this.FileName, false);
            }

            string errFormatStr = string.Empty;
            string errMsg = string.Empty;

            switch ((ErrorTypes)Enum.ToObject(typeof(ErrorTypes), this.ErrorType))
            {
                case ErrorTypes.SyntaxError: errFormatStr = "Syntax Error at line {0}: {1}"; break;
                case ErrorTypes.RuntimeError: errFormatStr = "Runtime Error at line {0}: {1}"; break;
                case ErrorTypes.MemoryError: errFormatStr = "Memory Error: {0}"; break;
                case ErrorTypes.Unhandled: errFormatStr = "Unhandled Error at line {0}: {1}"; break;
            }

            FrameworkManager.Instance.FlashMainDialog();

            if ((ErrorTypes)this.ErrorType == ErrorTypes.MemoryError)
            {
                errMsg = string.Format(errFormatStr, this.Message);
                FrameworkManager.ShowMessageBox(errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((ErrorTypes)this.ErrorType == ErrorTypes.Unhandled)
            {
                errMsg = string.Format(errFormatStr, this.Line, this.Message);
                FrameworkManager.ShowMessageBox(errMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                errMsg = string.Format(errFormatStr, this.Line, this.Message);
                DialogResult dr = FrameworkManager.ShowMessageBox(errMsg, MessageBoxButtons.AbortRetryIgnore,
                                                                  MessageBoxIcon.Error);
                DebugAction debugAction = DebugAction.None;

                switch (dr)
                {
                    case DialogResult.Retry: debugAction = DebugAction.Break; break;
                    case DialogResult.Abort: debugAction = DebugAction.Stop; break;
                    case DialogResult.Ignore: debugAction = DebugAction.Continue; break;
                }

                return new DebugActionCommand(debugAction);
            }

            return null;
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_RUNTIMEERROR,
                                                        typeof(LuaErrorCommand));
        }

        #endregion
    };

    public class BreakAtLineCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public string FileName;
        public int Line;
        public int ThreadID;
        public bool IsInError;
        public LuaVariable[] Locals;
        public LuaThread[] LuaThreads;
        public LuaCall[] CallStack;

        #endregion

        #region Constructors

        public BreakAtLineCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetStringDataSize(FileName);
            totalSize += GetIntegerDataSize();
            totalSize += GetIntegerDataSize();
            totalSize += GetBooleanDataSize();
            totalSize += GetComplexDataArraySize(Locals);
            totalSize += GetComplexDataArraySize(LuaThreads);
            totalSize += GetComplexDataArraySize(CallStack);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeString(data, ref offset, ref FileName);
            DeserializeInteger(data, ref offset, ref Line);
            DeserializeInteger(data, ref offset, ref ThreadID);
            DeserializeBoolean(data, ref offset, ref IsInError);
            IRPCSerializableData[] arrLocals = new IRPCSerializableData[0];
            IRPCSerializableData[] arrLuaThreads = new IRPCSerializableData[0];
            IRPCSerializableData[] arrCallStack = new IRPCSerializableData[0];
            DeserializeComplexDataArray(data, ref offset, ref arrLocals, typeof(LuaVariable));
            DeserializeComplexDataArray(data, ref offset, ref arrLuaThreads, typeof(LuaThread));
            DeserializeComplexDataArray(data, ref offset, ref arrCallStack, typeof(LuaCall));
            Locals = new LuaVariable[arrLocals.Length];
            LuaThreads = new LuaThread[arrLuaThreads.Length];
            CallStack = new LuaCall[arrCallStack.Length];
            Array.Copy(arrLocals, Locals, arrLocals.Length);
            Array.Copy(arrLuaThreads, LuaThreads, arrLuaThreads.Length);
            Array.Copy(arrCallStack, CallStack, arrCallStack.Length);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeString(ref data, ref offset, FileName);
            SerializeInteger(ref data, ref offset, Line);
            SerializeInteger(ref data, ref offset, ThreadID);
            SerializeBoolean(ref data, ref offset, IsInError);
            SerializeComplexDataArray(ref data, ref offset, Locals);
            SerializeComplexDataArray(ref data, ref offset, LuaThreads);
            SerializeComplexDataArray(ref data, ref offset, CallStack);
        }

        public override RPCCommand DoCommand()
        {
            // Open the document and break at the current line
            DocumentsManager.Instance.OpenDocument(this.FileName, false,
                new PostOpenDocumentOptions(this.Line, Color.FromArgb(255, 238, 98), true));

            ClientDebugManager.Instance.IsBreaked = true;
            ClientDebugManager.Instance.CurrentThreadID = this.ThreadID;
            ClientDebugManager.Instance.IsInError = this.IsInError;
            ClientDebugManager.Instance.CallStack = this.CallStack;
            ClientDebugManager.Instance.LuaThreads = this.LuaThreads;

            foreach (LuaVariable var in Locals)
            {
                var.IsDirty = true;
            }

            ClientDebugManager.Instance.Locals = new List<LuaVariable>(Locals);
            FrameworkManager.Instance.FlashMainDialog();

            return null;
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_BREAKATLINE,
                                                        typeof(BreakAtLineCommand));
        }

        #endregion
    };

    public class UnBreakAtLineCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public string FileName;
        public int Line;

        #endregion

        #region Constructors

        public UnBreakAtLineCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetStringDataSize(FileName);
            totalSize += GetIntegerDataSize();
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeString(data, ref offset, ref FileName);
            DeserializeInteger(data, ref offset, ref Line);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeString(ref data, ref offset, FileName);
            SerializeInteger(ref data, ref offset, Line);
        }

        public override RPCCommand DoCommand()
        {
            DocumentsManager.Instance.OpenDocument(this.FileName, false, new PostOpenDocumentOptions(this.Line, Color.White));
            ClientDebugManager.Instance.IsBreaked = false;

            return null;
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_UNBREAKATLINE,
                                                        typeof(UnBreakAtLineCommand));
        }

        #endregion
    };

    public class DebugActionCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public int DebugAction;

        #endregion

        #region Constructors

        public DebugActionCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        public DebugActionCommand(DebugAction debugAction) :
            base((int)RpcCommandType.eRPC_COMMANDTYPE_DEBUGACTION)
        {
            this.DebugAction = (int)debugAction;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            return GetIntegerDataSize();
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeInteger(data, ref offset, ref DebugAction);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeInteger(ref data, ref offset, DebugAction);
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_DEBUGACTION,
                                                        typeof(DebugActionCommand));
        }

        #endregion
    };

    public class LuaOutputCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public int OutputType;
        public string Output;

        #endregion

        #region Constructors

        public LuaOutputCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetIntegerDataSize();
            totalSize += GetStringDataSize(Output);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeInteger(data, ref offset, ref OutputType);
            DeserializeString(data, ref offset, ref Output);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeInteger(ref data, ref offset, OutputType);
            SerializeString(ref data, ref offset, Output);
        }

        public override RPCCommand DoCommand()
        {
            ClientDebugManager.Instance.RaiseOutputOccuredEvent(this.Output, (OutputType)this.OutputType);

            return null;
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_LUAOUTPUT,
                                                        typeof(LuaOutputCommand));
        }

        #endregion
    };

    public class CopyScriptCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public string FileName;
        public string ScriptContent;

        #endregion

        #region Constructors

        public CopyScriptCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetStringDataSize(FileName);
            totalSize += GetStringDataSize(ScriptContent);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            DeserializeString(data, ref offset, ref FileName);
            DeserializeString(data, ref offset, ref ScriptContent);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeString(ref data, ref offset, FileName);
            SerializeString(ref data, ref offset, ScriptContent);
        }

        public override RPCCommand DoCommand()
        {
            using (FileStream fs = new FileStream(this.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                fs.Write(Encoding.GetBytes(this.ScriptContent), 0, this.ScriptContent.Length);

            return null;
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_INVALID,
                                                        typeof(CopyScriptCommand));
        }

        #endregion
    };

    public class SetLuaVariableCommand : RPCCommand, IRPCSerializableData
    {
        #region Members

        public LuaVariable LuaVar;

        #endregion

        #region Constructors

        public SetLuaVariableCommand(CommandHeader cmdHeader)
            : base(cmdHeader)
        {
        }

        public SetLuaVariableCommand(LuaVariable luaVar)
            : base((int)RpcCommandType.eRPC_COMMANDTYPE_SETLUAVARIABLE)
        {
            LuaVar = luaVar;
        }

        #endregion

        #region Properties

        public override IRPCSerializableData Data
        {
            get { return this; }
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += GetComplexDataSize(LuaVar);
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            IRPCSerializableData tempLuaVar = null;
            DeserializeComplexData(data, ref offset, ref tempLuaVar, typeof(LuaVariable));
            LuaVar = tempLuaVar as LuaVariable;
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            SerializeComplexData(ref data, ref offset, LuaVar);
        }

        public static void Register()
        {
            CommandManager.Instance.RegisterCommandType((int)RpcCommandType.eRPC_COMMANDTYPE_SETLUAVARIABLE,
                                                        typeof(SetLuaVariableCommand));
        }

        #endregion
    };

    public static class DebugCommandUtils
    {
        #region Methods

        /// <summary>
        /// Registers all available commands to the packet manager
        /// </summary>
        public static void RegisterAllCommands()
        {
            UpdateDebugInfoCommand.Register();
            BreakAtLineCommand.Register();
            UnBreakAtLineCommand.Register();
            RunScriptInVMCommand.Register();
            UpdateBreakpointsCommand.Register();
            DebugActionCommand.Register();
            LuaErrorCommand.Register();
            UpdateTableDetailsCommand.Register();
            LuaOutputCommand.Register();
        }

        #endregion
    }
}
