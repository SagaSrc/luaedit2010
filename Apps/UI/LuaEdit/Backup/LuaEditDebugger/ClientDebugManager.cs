using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

using Fireball.Syntax;

using LuaEdit.Documents;
using LuaEdit.Documents.DocumentUtils;
using LuaEdit.Interfaces;
using LuaEdit.LuaEditDebugger;
using LuaEdit.LuaEditDebugger.DebugCommands;
using LuaEdit.Managers;
using LuaEdit.Rpc;
using LuaEdit.Utils;

namespace LuaEdit.LuaEditDebugger
{
    public enum DebugAction : int
    {
        None,
        Run,
        StepInto,
        StepOver,
        StepOut,
        Break,
        Stop,
        Continue
    }

    public enum OutputType : int
    {
        Lua = 0,
        LuaEdit = 1
    }

    public class DebugInfo : IRPCSerializableData
    {
        #region Members

        private int _processID = 0;
        private IPAddress _serverIP = IPAddress.Parse(RpcClient.DefaultServerIP);
        private int _serverPort = RpcClient.DefaultServerPort;
        private string _remotePath = string.Empty;
        private List<ILuaEditDocument> _scripts;
        private ILuaEditDocument _startupDoc = null;
        private bool _isLocal = true;
        private DebugStartAction _startAction;

        #endregion

        #region Constructors

        public DebugInfo(IPAddress serverIP, int serverPort, DebugStartAction startAction)
        {
            _serverIP = serverIP;
            _serverPort = serverPort;
            _startAction = startAction;
            _isLocal = serverIP.ToString() == "127.0.0.1";
        }

        public DebugInfo(IPAddress serverIP, int serverPort, string remotePath, List<ILuaEditDocument> scripts,
                         ILuaEditDocument startupDoc, DebugStartAction startAction) :
            this(serverIP, serverPort, startAction)
        {
            _remotePath = remotePath;
            _scripts = scripts;
            _startupDoc = startupDoc;
        }

        public DebugInfo(int processID, IPAddress serverIP, int serverPort, string remotePath,
                         List<ILuaEditDocument> scripts, ILuaEditDocument startupDoc, DebugStartAction startAction)
            : this(serverIP, serverPort, remotePath, scripts, startupDoc, startAction)
        {
            _processID = processID;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The process' ID on which to attach
        /// </summary>
        public int ProcessID
        {
            get { return _processID; }
            set { _processID = value; }
        }

        /// <summary>
        /// The server's IP address
        /// </summary>
        public IPAddress ServerIP
        {
            get { return _serverIP; }
            set { _serverIP = value; }
        }

        /// <summary>
        /// The server's port number
        /// </summary>
        public int ServerPort
        {
            get { return _serverPort; }
            set { _serverPort = value; }
        }

        /// <summary>
        /// The path from which to perform the remote debugging
        /// </summary>
        /// <remarks>The scripts will be copied in this location</remarks>
        public string RemotePath
        {
            get { return _remotePath; }
            set { _remotePath = value; }
        }

        /// <summary>
        /// The list of documents that will be used to copy to the server
        /// </summary>
        public List<ILuaEditDocument> Scripts
        {
            get { return _scripts; }
        }

        /// <summary>
        /// The main script to first run
        /// </summary>
        public ILuaEditDocument StartupDocument
        {
            get { return _startupDoc; }
            set { _startupDoc = value; }
        }

        /// <summary>
        /// The starting action when starting debugging
        /// </summary>
        public DebugStartAction StartAction
        {
            get { return _startAction; }
            set { _startAction = value; }
        }

        /// <summary>
        /// Determine whether the debugging is performed locally or remotely
        /// </summary>
        public bool IsLocal
        {
            get { return _isLocal; }
            set { _isLocal = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the serialized data size
        /// </summary>
        /// <returns>The serialized data size</returns>
        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetStringDataSize(_remotePath);
            return totalSize;
        }

        /// <summary>
        /// Deserializes the serialized data
        /// </summary>
        public void DeserializeData(byte[] data, ref int offset)
        {
            int startActionTemp = 0;
            RPCCommand.DeserializeInteger(data, ref offset, ref startActionTemp);
            _startAction = (DebugStartAction)startActionTemp;
            RPCCommand.DeserializeString(data, ref offset, ref _remotePath);
        }

        /// <summary>
        /// Serializes the the deserialized data
        /// </summary>
        public void SerializeData(ref byte[] data, ref int offset)
        {
            RPCCommand.SerializeInteger(ref data, ref offset, (int)_startAction);
            RPCCommand.SerializeString(ref data, ref offset, _remotePath);
        }

        #endregion
    }

    public delegate void DebuggingStartedEventHandler(object sender, EventArgs e);
    public delegate void DebuggingStoppedEventHandler(object sender, EventArgs e);

    public class ClientDebugManager
    {
        #region Members

        public event EventHandler LocalsChanged = null;
        public event EventHandler ThreadsChanged = null;
        public event EventHandler CallStackChanged = null;
        public event EventHandler BreakChanged = null;
        public event DebuggingStartedEventHandler DebuggingStarted = null;
        public event DebuggingStoppedEventHandler DebuggingStopped = null;
        public event TableDetailsUpdatedEventHandler TableDetailsUpdated = null;
        public event OutputOccuredEventHandler OutputOccured = null;

        public const int DefaultDebugPort = 32201;

        /// <summary>
        /// The only instance of ClientDebugManager
        /// </summary>
        private static readonly ClientDebugManager _clientDebugManager;

        private DebugInfo _debugInfo;
        private RpcClient _clientDebugger;
        private List<LuaVariable> _locals;
        private LuaCall[] _callStack;
        private LuaThread[] _threads;
        private int _currentThreadID = 0;
        private bool _isDebugging = false;
        private bool _isBreaked = false;
        private bool _isInError = false;

        #endregion

        #region Constructors

        static ClientDebugManager()
        {
            _clientDebugManager = new ClientDebugManager();
            DebugCommandUtils.RegisterAllCommands();
        }

        private ClientDebugManager()
        {
            DebugCommandUtils.RegisterAllCommands();
            BreakpointManager.Instance.BreakpointChanged += OnBreakpointChangedAddedRemoved;
            BreakpointManager.Instance.BreakpointAdded += OnBreakpointChangedAddedRemoved;
            BreakpointManager.Instance.BreakpointRemoved += OnBreakpointChangedAddedRemoved;
            _clientDebugger = new RpcClient();
            _clientDebugger.Disconnected += OnClientDisconnected;
            _locals = new List<LuaVariable>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the only instance of ClientDebugManager
        /// </summary>
        public static ClientDebugManager Instance
        {
            get { return _clientDebugManager; }
        }

        /// <summary>
        /// Gets the current debug info data for the current debug session
        /// </summary>
        public DebugInfo DebugInfo
        {
            get { return _debugInfo; }
        }

        /// <summary>
        /// Get whether or not the debugging session is currently running
        /// </summary>
        public bool IsDebugging
        {
            get { return _isDebugging; }
        }

        /// <summary>
        /// Gets or sets the currently debugging thread ID
        /// </summary>
        public int CurrentThreadID
        {
            get { return _currentThreadID; }
            set
            {
                if (value != _currentThreadID)
                {
                    _currentThreadID = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not the debugging session is currently breaked on a line
        /// </summary>
        public bool IsBreaked
        {
            get { return _isBreaked; }
            set
            {
                if (_isBreaked != value)
                {
                    _isBreaked = value;

                    if (BreakChanged != null)
                    {
                        BreakChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not the debugging session is currently breaked due to an error
        /// </summary>
        public bool IsInError
        {
            get { return _isInError; }
            set { _isInError = value; }
        }

        /// <summary>
        /// Contains the current callstack
        /// </summary>
        public LuaCall[] CallStack
        {
            get { return _callStack; }
            set
            {
                if (_callStack != value)
                {
                    _callStack = value;

                    if (CallStackChanged != null)
                    {
                        CallStackChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Contains the list of locals
        /// </summary>
        public List<LuaVariable> Locals
        {
            get { return _locals; }
            set
            {
                if (value != _locals)
                {
                    _locals = value;

                    if (LocalsChanged != null)
                    {
                        LocalsChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Contains the list of threads
        /// </summary>
        public LuaThread[] LuaThreads
        {
            get { return _threads; }
            set
            {
                if (_threads != value)
                {
                    _threads = value;

                    if (ThreadsChanged != null)
                    {
                        ThreadsChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        private void OnClientDisconnected(object sender, EventArgs e)
        {
            StopDebugging(false);
        }

        private void OnBreakpointChangedAddedRemoved(object sender, Breakpoint bp)
        {
            SendBreakpoints();
        }

        #endregion

        #region Methods

        public void StartDebugging(DebugInfo di, int runToLine)
        {
            _debugInfo = di;

            // Starts a
            if (di.IsLocal && di.StartAction == DebugStartAction.StartProject)
            {
                ServerDebugManager.Instance.StartServer(DefaultDebugPort);
            }
            else if (di.IsLocal && di.StartAction == DebugStartAction.StartExternalProgram)
            {
                // todo: start the external program
            }

            if (_clientDebugger.StartClient(di.ServerIP, di.ServerPort))
            {
                _isDebugging = true;
            }
            else
            {
                throw new Exception("There was a problem starting the debugging client!");
            }

            if (DebuggingStarted != null)
            {
                DebuggingStarted(this, EventArgs.Empty);
            }

            UpdateDebugInfoCommand updateDICmd = new UpdateDebugInfoCommand(di);
            _clientDebugger.SendCommand(updateDICmd);

            /*if (!di.IsLocal)
            {
                foreach (ILuaEditDocument doc in di.Scripts)
                {
                    using (StreamReader streamReader = new StreamReader(doc.FileName))
                    {
                        string fileName = Path.Combine(di.RemotePath, doc.FileName);
                        CopyScriptCommand copyScriptCmd = new CopyScriptCommand(fileName, streamReader.ReadToEnd());
                        _clientDebugger.SendCommand(copyScriptCmd);
                    }
                }
            }*/

            // Update breakpoints
            SendBreakpoints();

            // Run startup script in VM
            if (di.StartupDocument != null)
            {
                RunScriptInVMCommand runScriptInVMCmd =
                    new RunScriptInVMCommand(di.StartupDocument.FileName, runToLine);

                _clientDebugger.SendCommand(runScriptInVMCmd);
            }
        }

        public void StopDebugging()
        {
            StopDebugging(true);
        }

        public void StopDebugging(bool stopClient)
        {
            if (stopClient)
            {
                _clientDebugger.StopClient();
            }

            if (_debugInfo.StartAction == DebugStartAction.StartExternalProgram && _debugInfo.ProcessID > 0)
            {
                try
                {
                    Process p = Process.GetProcessById(_debugInfo.ProcessID);
                    if (p != null)
                    {
                        p.Kill();
                    }
                }
                catch (Exception)
                {
                    // .Net will throw an exception if process is not running
                    // so let's simply trap it and ignore it
                }
            }

            _isDebugging = false;
            _isBreaked = false;
            _debugInfo = null;

            if (DebuggingStopped != null)
            {
                DebuggingStopped(this, EventArgs.Empty);
            }
        }

        public void AddCommand(RPCCommand cmd)
        {
            if (_clientDebugger.IsRuning)
            {
                _clientDebugger.SendCommand(cmd);
            }
        }

        public void RaiseTableDetailsUpdatedEvent(string tableNameIn, string tableNameOut, bool isLocal, List<LuaVariable> subValues)
        {
            if (TableDetailsUpdated != null)
            {
                TableDetailsUpdated(this, new TableDetailsUpdatedEventArgs(tableNameIn, tableNameOut, isLocal, subValues));
            }
        }

        public void RaiseOutputOccuredEvent(string output, OutputType outputType)
        {
            if (OutputOccured != null)
            {
                OutputOccured(this, new OutputOccuredEventArgs(output, outputType));
            }
        }

        private void SendBreakpoints()
        {
            if (_isDebugging)
            {
                List<SimpleBreakpoint> simpleBreakpoints = new List<SimpleBreakpoint>();
                foreach (string fileName in BreakpointManager.Instance.Breakpoints.Keys)
                {
                    Dictionary<Row, Breakpoint> bps = BreakpointManager.Instance.Breakpoints[fileName];

                    foreach (Breakpoint bp in bps.Values)
                    {
                        simpleBreakpoints.Add(bp.ToSimpleBreakpoint());
                    }
                }

                if (simpleBreakpoints.Count > 0)
                {
                    UpdateBreakpointsCommand updateBPCmd = new UpdateBreakpointsCommand(simpleBreakpoints.ToArray());
                    _clientDebugger.SendCommand(updateBPCmd);
                }
            }
        }

        #endregion
    }

    #region Delegates and EventArgs

    public delegate void TableDetailsUpdatedEventHandler(object sender, TableDetailsUpdatedEventArgs e);
    public class TableDetailsUpdatedEventArgs
    {
        #region Members

        private string _tableNameIn = string.Empty;
        private string _tableNameOut = string.Empty;
        private bool _isLocal = false;
        private List<LuaVariable> _subValues = null;

        #endregion

        #region Constructors

        public TableDetailsUpdatedEventArgs(string tableNameIn, string tableNameOut,
                                            bool isLocal, List<LuaVariable> subValues)
        {
            _tableNameIn = tableNameIn;
            _tableNameOut = tableNameOut;
            _subValues = subValues;
            _isLocal = isLocal;
        }

        #endregion

        #region Properties

        public string TableNameIn
        {
            get { return _tableNameIn; }
        }

        public string TableNameOut
        {
            get { return _tableNameOut; }
        }

        public bool IsLocal
        {
            get { return _isLocal; }
        }

        public List<LuaVariable> SubValues
        {
            get { return _subValues; }
        }

        #endregion
    }

    public delegate void OutputOccuredEventHandler(object sender, OutputOccuredEventArgs e);
    public class OutputOccuredEventArgs
    {
        #region Members

        private string _output = string.Empty;
        private OutputType _outputType;

        #endregion

        #region Constructors

        public OutputOccuredEventArgs(string output, OutputType outputType)
        {
            _output = output;
            _outputType = outputType;
        }

        #endregion

        #region Properties

        public string Output
        {
            get { return _output; }
        }

        public OutputType OutputType
        {
            get { return _outputType; }
        }

        #endregion
    }

    #endregion
}
