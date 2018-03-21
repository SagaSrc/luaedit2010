using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.LuaEditDebugger
{
    class ServerDebugManager
    {
        #region Members

        /// <summary>
        /// The only instance of ServerDebugManager
        /// </summary>
        private static readonly ServerDebugManager _serverDebugManager;

        [DllImport("rdbg.dll", SetLastError = true)]
        public static extern void StartLuaEditRemoteDebugger(int port, IntPtr L);
        [DllImport("rdbg.dll", SetLastError = true)]
        public static extern void StopLuaEditRemoteDebugger();

        #endregion

        #region Constructors

        static ServerDebugManager()
        {
            _serverDebugManager = new ServerDebugManager();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the only instance of ServerDebugManager
        /// </summary>
        public static ServerDebugManager Instance
        {
            get { return _serverDebugManager; }
        }

        #endregion

        #region Methods

        public void StartServer(int port)
        {
            StartLuaEditRemoteDebugger(port, IntPtr.Zero);
        }

        public void StopServer()
        {
            StopLuaEditRemoteDebugger();
        }

        #endregion
    }
}
