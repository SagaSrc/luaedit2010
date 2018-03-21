using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LuaEdit.Interfaces
{
    public enum PostBuildRunType
    {
        Always = 0,
        OnSuccessful
    }

    public enum DebugStartAction
    {
        StartProject = 0,
        StartExternalProgram,
        StartAttach
    }

    public interface ILuaEditProjectProperties : ILuaEditDocumentUI
    {
        #region Properties

        #region Debug Properties

        DebugStartAction StartAction
        {
            get;
            set;
        }

        string ExternalProgram
        {
            get;
            set;
        }

        string CommandLineArguments
        {
            get;
            set;
        }

        IPAddress RemoteMachineIP
        {
            get;
        }

        int RemotePort
        {
            get;
            set;
        }

        string StartupFileName
        {
            get;
            set;
        }

        string WorkingDirectory
        {
            get;
            set;
        }

        bool UseRemoteMachine
        {
            get;
            set;
        }

        string RemoteMachineName
        {
            get;
            set;
        }

        #endregion

        #region Build Properties

        string BuildOutputDirectory
        {
            get;
            set;
        }

        #endregion

        #region Build Events Properties

        string PreBuildEventCmdLine
        {
            get;
            set;
        }

        string PostBuildEventCmdLine
        {
            get;
            set;
        }

        PostBuildRunType RunPostBuildEvent
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Methods

        void SetUI(ILuaEditDocumentProject prjDoc);
        void GetUI(ILuaEditDocumentProject prjDoc);

        #endregion
    }
}
