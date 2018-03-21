using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace LuaEdit.Rpc
{
    static public class SocketUtils
    {
        #region Methods

        /// <summary>
        /// Determine whether or not the specified socket is connected to its host
        /// </summary>
        /// <param name="socket">The socket to test</param>
        /// <returns>True if the socket is connected. Otherwise false.</returns>
        static public bool IsSocketConnectedToHost(Socket socket)
        {
            bool pollingResult = !(socket.Poll(20, SelectMode.SelectRead) && socket.Available == 0);
            bool ackResult = true;

            try
            {
                socket.Send(new byte[0], 0, SocketFlags.None);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    ackResult = false;
                }
            }

            return ackResult && pollingResult;
        }

        #endregion
    }
}
