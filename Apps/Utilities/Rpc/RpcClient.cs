using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LuaEdit.Rpc
{
    public delegate void DisconnectedEventHandler(object sender, EventArgs e);

    public class RpcClient
    {
        #region Members

        private Thread _clientThread;
        private Socket _clientSocket = null;
        private List<RPCCommand> _commandsToSend;
        private Dictionary<Guid, CommandReplyEventHandler> _eventsByCmdId;

        public event DisconnectedEventHandler Disconnected;

        public const int AnswerTimeoutSeconds = 30;
        public const string DefaultServerIP = "127.0.0.1";
        public const int DefaultServerPort = 2222;

        #endregion

        #region Constructors

        public RpcClient()
        {
            _commandsToSend = new List<RPCCommand>();
            _eventsByCmdId = new Dictionary<Guid, CommandReplyEventHandler>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether or not the client is currently running
        /// </summary>
        public bool IsRuning
        {
            get { return _clientThread != null && _clientThread.IsAlive; }
        }

        #endregion

        #region Methods

        public void StopClient()
        {
            if (this.IsRuning && _clientSocket != null && _clientSocket.Connected)
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close(1);
                _clientThread.Abort();
            }
        }

        /// <summary>
        /// Start and connect the client to the specified server
        /// </summary>
        public bool StartClient(IPAddress serverIP, int serverPort)
        {
            try
            {
                if (_clientThread == null || !_clientThread.IsAlive)
                {
                    IPEndPoint ipep = new IPEndPoint(serverIP, serverPort);
                    _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _clientSocket.Connect(ipep);
                    _clientSocket.Blocking = false;
                    _clientThread = new Thread(RunClient);
                    _clientThread.Name = "Client Thread";
                    _clientThread.Start(this);

                    return _clientSocket.Connected;
                }
            }
            catch (Exception error)
            {
                string msg = string.Format("Could not connect to debug server: {0}", error.Message);
                throw new Exception(msg);
            }

            return false;
        }

        public void SendCommand(RPCCommand cmd)
        {
            SendCommandWithAnswer(cmd, null);
        }

        public void SendCommandWithAnswer(RPCCommand cmd, CommandReplyEventHandler OnCommandReply)
        {
            lock (_commandsToSend)
            {
                if (OnCommandReply != null)
                {
                    //_eventsByCmdId.Add(cmd.Uid, OnCommandReply);
                }
                
                _commandsToSend.Add(cmd);
            }
        }

        public void RunClient(object sender)
        {
            bool shouldRun = true;
            List<RPCCommand> cmds = new List<RPCCommand>();

            while (shouldRun && _clientSocket.Connected && SocketUtils.IsSocketConnectedToHost(_clientSocket))
            {
                const int MaxPerFrame = 16;
                int cmdsReceived = 0;
                bool hasNoMoreData = false;

                // Get any incoming command if any
                while (shouldRun && !hasNoMoreData && cmdsReceived < MaxPerFrame)
                {
                    try
                    {
                        CommandHeader cmdHeader = new CommandHeader(_clientSocket);
                        RPCCommand cmd = CommandManager.Instance.CreateCommandFromCommandHeader(cmdHeader);
                        cmds.Add(cmd);
                        cmd.DeserializeCommand();
                        RPCCommand cmdReply = cmd.DoCommand();

                        if (cmdReply != null)
                        {
                            lock (_commandsToSend)
                            {
                                _commandsToSend.Add(cmdReply);
                            }
                        }

                        ++cmdsReceived;
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode == SocketError.WouldBlock)
                        {
                            hasNoMoreData = true;
                        }
                        else
                        {
                            shouldRun = false;
                        }
                    }
                }

                // Send all buffered commands if any
                if (_commandsToSend.Count > 0)
                {
                    lock (_commandsToSend)
                    {
                        foreach (RPCCommand cmd in _commandsToSend)
                        {
                            cmd.SerializeCommand();
                            cmd.CommandHeader.WriteCommand(_clientSocket);
                        }
                    
                        _commandsToSend.Clear();
                    }
                }

                Thread.Sleep(20);
            }

            _clientSocket.Close(1);

            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }

        #endregion
    };
}
