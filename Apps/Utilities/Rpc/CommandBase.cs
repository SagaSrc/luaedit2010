using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace LuaEdit.Rpc
{
    public delegate void CommandReplyEventHandler(object sender, CommandReplyEventArgs e);
    public class CommandReplyEventArgs : EventArgs
    {
        #region Members

        private RPCCommand _cmd;

        #endregion

        #region Constructors

        public CommandReplyEventArgs(RPCCommand cmd)
            : base()
        {
            _cmd = cmd;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The command instance received as a reply
        /// </summary>
        public RPCCommand Command
        {
            get { return _cmd; }
        }

        #endregion
    };

    public class CommandHeader
    {
        #region Members

        private byte[] _rawData = null;
        private int _commandType = 0;

        #endregion

        #region Constructors

        public CommandHeader(int commandType)
        {
            _commandType = commandType;
            _rawData = new byte[0];
        }

        public CommandHeader(Socket socket)
        {
            ReadCommand(socket);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the underlying RPCCommand contained within RawData
        /// </summary>
        public int CommandType
        {
            get { return _commandType; }
        }

        /// <summary>
        /// The RPCCommand raw data (serialized)
        /// </summary>
        public byte[] RawData
        {
            get { return _rawData; }
            set { _rawData = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the raw data
        /// </summary>
        public void ClearRawData()
        {
            _rawData = new byte[0];
        }

        /// <summary>
        /// Write the specified command to the provided network stream
        /// </summary>
        /// <param name="socket">The socket on which to write the command</param>
        public void WriteCommand(Socket socket)
        {
            List<byte> buffer = new List<byte>();

            ////////////////////////////////////////////////////////////////////////////////////
            // Append the command type
            buffer.AddRange(BitConverter.GetBytes(this.CommandType));

            ////////////////////////////////////////////////////////////////////////////////////
            // Append the data length
            buffer.AddRange(BitConverter.GetBytes(_rawData.Length));

            ////////////////////////////////////////////////////////////////////////////////////
            // Append the data
            buffer.AddRange(_rawData);

            ////////////////////////////////////////////////////////////////////////////////////
            // Send the data
            socket.Send(buffer.ToArray(), buffer.Count, SocketFlags.None);
        }

        /// <summary>
        /// Read the general layout of command using a network stream
        /// </summary>
        /// <param name="socket">The socket from wich to read the command</param>
        public void ReadCommand(Socket socket)
        {
            byte[] buffer;

            ////////////////////////////////////////////////////////////////////////////////////
            // Receive the command type
            buffer = new byte[sizeof(int)];
            int readBytes = socket.Receive(buffer, buffer.Length, SocketFlags.None);
            if (readBytes == 0)
                throw new SocketException((int)SocketError.ConnectionAborted);

            int cmdType = BitConverter.ToInt32(buffer, 0);

            if (!CommandManager.Instance.CommandsByType.ContainsKey(cmdType))
            {
                buffer = new byte[cmdType];
                socket.Receive(buffer, buffer.Length, SocketFlags.None);
                string maybe = Encoding.ASCII.GetString(buffer);
                throw new Exception("OMG!!!! " + maybe);
            }

            ////////////////////////////////////////////////////////////////////////////////////
            // Receive the command data length
            buffer = new byte[sizeof(int)];
            readBytes = socket.Receive(buffer, buffer.Length, SocketFlags.None);
            if (readBytes == 0)
                throw new SocketException((int)SocketError.ConnectionAborted);

            int dataLength = BitConverter.ToInt32(buffer, 0);

            ////////////////////////////////////////////////////////////////////////////////////
            // Receive the command data
            if (dataLength > 0)
            {
                int dataLengthToRead = dataLength;
                int totalReadBytes = 0;
                readBytes = 0;
                List<byte> dataBuffer = new List<byte>();

                while (totalReadBytes < dataLength)
                {
                    buffer = new byte[dataLengthToRead];
                    readBytes = socket.Receive(buffer, dataLengthToRead, SocketFlags.None);
                    totalReadBytes += readBytes;
                    dataBuffer.AddRange(buffer);

                    if (readBytes > 0)
                    {
                        dataLengthToRead -= readBytes;
                    }
                }

                buffer = dataBuffer.ToArray();
                Array.Resize<byte>(ref buffer, dataLength);
            }

            _commandType = cmdType;
            _rawData = buffer;
        }

        #endregion
    }

    public interface IRPCSerializableData
    {
        #region Methods

        /// <summary>
        /// Gets the serialized data size
        /// </summary>
        /// <returns>The serialized data size</returns>
        int GetSerializedDataSize();

        /// <summary>
        /// Deserializes the serialized data
        /// </summary>
        void DeserializeData(byte[] data, ref int offset);

        /// <summary>
        /// Serializes the the deserialized data
        /// </summary>
        void SerializeData(ref byte[] data, ref int offset);

        #endregion
    }

    public class RPCCommand
    {
        #region Members

        protected CommandHeader _cmdHeader = null;
        protected static ASCIIEncoding Encoding = new ASCIIEncoding();

        #endregion

        #region Constructors

        public RPCCommand(CommandHeader cmdHeader)
        {
            _cmdHeader = cmdHeader;
        }

        public RPCCommand(int commandType)
        {
            _cmdHeader = new CommandHeader(commandType);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the CommandHeader for this RPCCommand
        /// </summary>
        public CommandHeader CommandHeader
        {
            get { return _cmdHeader; }
        }

        /// <summary>
        /// Gets the underlying data
        /// </summary>
        public virtual IRPCSerializableData Data
        {
            get { return null; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear the serialized data
        /// </summary>
        public void ClearSerializedData()
        {
            _cmdHeader.ClearRawData();
        }

        /// <summary>
        /// Executes the command and optionaly returns a reply command
        /// </summary>
        public virtual RPCCommand DoCommand()
        {
            return null;
        }

        /// <summary>
        /// Deserializes the serialized data
        /// </summary>
        public virtual void DeserializeCommand()
        {
            int offset = 0;
            this.Data.DeserializeData(this.CommandHeader.RawData, ref offset);
        }

        /// <summary>
        /// Serializes the the deserialized data
        /// </summary>
        public virtual void SerializeCommand()
        {
            int offset = 0;
            byte[] buffer = new byte[this.Data.GetSerializedDataSize()];
            this.Data.SerializeData(ref buffer, ref offset);
            _cmdHeader.RawData = buffer;
        }

        #region Size Helpers

        public static int GetStringDataSize(string str)
        {
            return GetIntegerDataSize() + str.Length;
        }

        public static int GetIntegerDataSize()
        {
            return sizeof(int);
        }

        public static int GetBooleanDataSize()
        {
            return sizeof(bool);
        }

        public static int GetComplexDataSize(IRPCSerializableData complexData)
        {
            return complexData.GetSerializedDataSize();
        }

        public static int GetComplexDataArraySize(IRPCSerializableData[] complexDataArray)
        {
            int totalSize = GetIntegerDataSize();

            if (complexDataArray != null)
            {
                foreach (IRPCSerializableData complexData in complexDataArray)
                {
                    totalSize += GetComplexDataSize(complexData);
                }
            }

            return totalSize;
        }

        #endregion

        #region Deserialization Helpers

        public static void DeserializeString(byte[] data, ref int offset, ref string str)
        {
            int strLen = 0;
            DeserializeInteger(data, ref offset, ref strLen);
            str = Encoding.GetString(data, offset, strLen);
            offset += strLen;
        }

        public static void DeserializeInteger(byte[] data, ref int offset, ref int integer)
        {
            integer = BitConverter.ToInt32(data, offset);
            offset += GetIntegerDataSize();
        }

        public static void DeserializeBoolean(byte[] data, ref int offset, ref bool boolean)
        {
            boolean = BitConverter.ToBoolean(data, offset);
            offset += GetBooleanDataSize();
        }

        public static void DeserializeComplexData(byte[] data, ref int offset, ref IRPCSerializableData complexData, Type complexDataType)
        {
            complexData = Activator.CreateInstance(complexDataType) as IRPCSerializableData;
            complexData.DeserializeData(data, ref offset);
        }

        public static void DeserializeComplexDataArray(byte[] data, ref int offset, ref IRPCSerializableData[] complexDataArray, Type complexDataType)
        {
            int complexDataArrayLength = 0;
            DeserializeInteger(data, ref offset, ref complexDataArrayLength);
            Array.Resize<IRPCSerializableData>(ref complexDataArray, complexDataArrayLength);

            for (int x = 0; x < complexDataArrayLength; ++x)
            {
                IRPCSerializableData complexData = Activator.CreateInstance(complexDataType) as IRPCSerializableData;
                DeserializeComplexData(data, ref offset, ref complexData, complexDataType);
                complexDataArray[x] = complexData;
            }
        }

        #endregion

        #region Serialization Helpers

        public static void SerializeString(ref byte[] data, ref int offset, string str)
        {
            SerializeInteger(ref data, ref offset, str.Length);
            byte[] buffer = Encoding.GetBytes(str);
            Array.Copy(buffer, 0, data, offset, buffer.Length);
            offset += str.Length;
        }

        public static void SerializeInteger(ref byte[] data, ref int offset, int integer)
        {
            byte[] buffer = BitConverter.GetBytes(integer);
            Array.Copy(buffer, 0, data, offset, buffer.Length);
            offset += GetIntegerDataSize();
        }

        public static void SerializeBoolean(ref byte[] data, ref int offset, bool boolean)
        {
            byte[] buffer = BitConverter.GetBytes(boolean);
            Array.Copy(buffer, 0, data, offset, buffer.Length);
            offset += GetBooleanDataSize();
        }

        public static void SerializeComplexData(ref byte[] data, ref int offset, IRPCSerializableData complexData)
        {
            complexData.SerializeData(ref data, ref offset);
        }

        public static void SerializeComplexDataArray(ref byte[] data, ref int offset, IRPCSerializableData[] complexDataArray)
        {
            SerializeInteger(ref data, ref offset, complexDataArray == null ? 0 : complexDataArray.Length);

            if (complexDataArray != null)
            {
                foreach (IRPCSerializableData complexData in complexDataArray)
                {
                    SerializeComplexData(ref data, ref offset, complexData);
                }
            }
        }

        #endregion

        #endregion
    }
}
