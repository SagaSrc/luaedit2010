using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LuaEdit.Rpc
{
    public class RpcServer
    {
        #region Members

        private const string ServerAppName = "RpcServer.exe";
        private const int DefaultServerPort = 2222;

        private Process _serverProcess = null;

        #endregion

        #region Constructors

        public RpcServer()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Stop the server
        /// </summary>
        public void StopServer()
        {
            if (_serverProcess != null)
            {
                _serverProcess.Kill();
                _serverProcess = null;
            }
        }

        /// <summary>
        /// Start the server using the default settings. The default settings are: any client IP
        /// addresses through port number 2222.
        /// </summary>
        public void StartServer()
        {
            _serverProcess = Process.Start(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), ServerAppName));
        }

        #endregion
    }
}
