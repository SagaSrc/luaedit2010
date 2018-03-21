using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace LuaEdit.Rpc
{
    public class CommandManager
    {
        #region Members

        /// <summary>
        /// The only instance of CommandManager
        /// </summary>
        private static readonly CommandManager _commandManager;

        /// <summary>
        /// The list of registered commands by their type
        /// </summary>
        private Dictionary<int, Type> _commandsByType;

        #endregion

        #region Constructors

        static CommandManager()
        {
            _commandManager = new CommandManager();
        }

        private CommandManager()
        {
            _commandsByType = new Dictionary<int, Type>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the only instance of CommandManager
        /// </summary>
        public static CommandManager Instance
        {
            get { return _commandManager; }
        }

        /// <summary>
        /// The registered commands ordered by type name
        /// </summary>
        public Dictionary<int, Type> CommandsByType
        {
            get { return _commandsByType; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers a command type to the command manager
        /// </summary>
        /// <param name="commandInstanceType">The instance type of the command to register</param>
        public void RegisterCommandType(int commandType, Type commandInstanceType)
        {
            if (!_commandsByType.ContainsKey(commandType))
            {
                _commandsByType.Add(commandType, commandInstanceType);
            }
        }

        public RPCCommand CreateCommandFromCommandHeader(CommandHeader cmdHeader)
        {
            if (_commandsByType.ContainsKey(cmdHeader.CommandType))
            {
                Type cmdInstanceType = _commandsByType[cmdHeader.CommandType];
                return Activator.CreateInstance(cmdInstanceType, new object[] { cmdHeader }) as RPCCommand;
            }

            return null;
        }

        #endregion
    }
}
