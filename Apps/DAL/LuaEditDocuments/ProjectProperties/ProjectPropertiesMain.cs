using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Documents.ProjectProperties
{
    public partial class ProjectPropertiesMain : DockContent, ILuaEditProjectProperties
    {
        #region Members

        private ILuaEditDocumentProject _parentDoc = null;
        private HistoryStack _prjHistoryStack = null;
        private DebugStartAction _startAction = DebugStartAction.StartProject;
        private string _externalProgram = string.Empty;
        private string _cmdLineArgs = string.Empty;
        private int _remotePort = 0;
        private string _startupFileName = string.Empty;
        private string _workingDir = string.Empty;
        private bool _useRemoteMachine = false;
        private string _remoteMachineName = string.Empty;
        private string _buildOutputDir = string.Empty;
        private string _preBuildEventCmdLine = string.Empty;
        private string _postBuildEventCmdLine = string.Empty;
        private PostBuildRunType _runPostBuildEvent = PostBuildRunType.OnSuccessful;

        public event EventHandler ContentChanged;

        #endregion

        #region Constructors

        public ProjectPropertiesMain(ILuaEditDocumentProject parentDoc)
        {
            _parentDoc = parentDoc;
            InitializeComponent();
        }

        public ProjectPropertiesMain(ILuaEditDocumentProject parentDoc, HistoryStack prjHistoryStack, string title)
            : this(parentDoc)
        {
            this.Text = title;
            _prjHistoryStack = prjHistoryStack;
            _prjHistoryStack.CurrentIndexChanged += OnHistoryStackIndexChanged;
            ConstructPageControl();
        }

        public ProjectPropertiesMain(ILuaEditDocumentProject parentDoc, HistoryStack prjHistoryStack)
            : this(parentDoc, prjHistoryStack, string.Empty)
        {
        }

        public ProjectPropertiesMain(ILuaEditDocumentProject parentDoc, HistoryStack prjHistoryStack, ILuaEditProjectProperties copy)
            : this(parentDoc, prjHistoryStack)
        {
            if (copy != null)
            {
                this.BuildOutputDirectory = copy.BuildOutputDirectory;
                this.CommandLineArguments = copy.CommandLineArguments;
                this.ExternalProgram = copy.ExternalProgram;
                this.PostBuildEventCmdLine = copy.PostBuildEventCmdLine;
                this.PreBuildEventCmdLine = copy.PreBuildEventCmdLine;
                this.RemoteMachineName = copy.RemoteMachineName;
                this.RemotePort = copy.RemotePort;
                this.RunPostBuildEvent = copy.RunPostBuildEvent;
                this.StartAction = copy.StartAction;
                this.StartupFileName = copy.StartupFileName;
                this.UseRemoteMachine = copy.UseRemoteMachine;
                this.WorkingDirectory = copy.WorkingDirectory;
            }
        }

        public ProjectPropertiesMain(ILuaEditDocumentProject parentDoc, HistoryStack prjHistoryStack, XmlLuaProjectProperties xmlData)
            : this(parentDoc, prjHistoryStack)
        {
            this.BuildOutputDirectory = xmlData.BuildOutputDir;
            this.CommandLineArguments = xmlData.CmdLineArgs;
            this.ExternalProgram = xmlData.ExternalProgram;
            this.PostBuildEventCmdLine = xmlData.PostBuildEventCmdLine;
            this.PreBuildEventCmdLine = xmlData.PreBuildEventCmdLine;
            this.RemoteMachineName = xmlData.RemoteMachineName;
            this.RemotePort = xmlData.RemotePort;
            this.RunPostBuildEvent = xmlData.RunPostBuildEvent;
            this.StartAction = xmlData.StartAction;
            this.StartupFileName = xmlData.StartupFileName;
            this.UseRemoteMachine = xmlData.UseRemoteMachine;
            this.WorkingDirectory = xmlData.WorkingDir;
        }

        #endregion

        #region Event Handlers

        private void OnHistoryStackIndexChanged(object sender, EventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Properties

        public bool CanUndo
        {
            get { return _parentDoc.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _parentDoc.CanRedo; }
        }

        public ILuaEditDocument ParentDocument
        {
            get { return _parentDoc; }
        }

        #region Debug Properties

        public DebugStartAction StartAction
        {
            get { return _startAction; }
            set { _startAction = value; }
        }

        public string ExternalProgram
        {
            get { return _externalProgram; }
            set { _externalProgram = value; }
        }

        public string CommandLineArguments
        {
            get { return _cmdLineArgs; }
            set { _cmdLineArgs = value; }
        }

        public IPAddress RemoteMachineIP
        {
            get
            {
                if (_startAction == DebugStartAction.StartExternalProgram ||
                    string.IsNullOrEmpty(this.RemoteMachineName))
                {
                    // Return the local host IP
                    return IPAddress.Parse("127.0.0.1");
                }
                else
                {
                    IPHostEntry iphe = Dns.GetHostEntry(this.RemoteMachineName);
                    if (iphe != null && iphe.AddressList.Length > 0)
                    {
                        return iphe.AddressList[0];
                    }
                    else
                    {
                        return IPAddress.None;
                    }
                }
            }
        }

        public int RemotePort
        {
            get { return _remotePort; }
            set { _remotePort = value; }
        }

        public string StartupFileName
        {
            get { return _startupFileName; }
            set { _startupFileName = value; }
        }

        public string WorkingDirectory
        {
            get { return _workingDir; }
            set { _workingDir = value; }
        }

        public bool UseRemoteMachine
        {
            get { return _useRemoteMachine; }
            set { _useRemoteMachine = value; }
        }

        public string RemoteMachineName
        {
            get { return _remoteMachineName; }
            set { _remoteMachineName = value; }
        }

        #endregion

        #region Build Properties

        public string BuildOutputDirectory
        {
            get { return _buildOutputDir; }
            set { _buildOutputDir = value; }
        }

        #endregion

        #region Build Events Properties

        public string PreBuildEventCmdLine
        {
            get { return _preBuildEventCmdLine; }
            set { _preBuildEventCmdLine = value; }
        }

        public string PostBuildEventCmdLine
        {
            get { return _postBuildEventCmdLine; }
            set { _postBuildEventCmdLine = value; }
        }

        public PostBuildRunType RunPostBuildEvent
        {
            get { return _runPostBuildEvent; }
            set { _runPostBuildEvent = value; }
        }

        #endregion

        #endregion

        #region Methods

        private void ConstructPageControl()
        {
            ProjectPropertiesDebug projectPropertiesDebug = new ProjectPropertiesDebug(_prjHistoryStack, _parentDoc);
            projectPropertiesPageControl.AddPage(projectPropertiesDebug);

            ProjectPropertiesBuild projectPropertiesBuild = new ProjectPropertiesBuild(_prjHistoryStack);
            projectPropertiesPageControl.AddPage(projectPropertiesBuild);

            ProjectPropertiesBuildEvents projectPropertiesBuildEvents = new ProjectPropertiesBuildEvents(_prjHistoryStack);
            projectPropertiesPageControl.AddPage(projectPropertiesBuildEvents);
        }

        public void Undo()
        {
            _parentDoc.Undo();
        }

        public void Redo()
        {
            _parentDoc.Redo();
        }

        public void SetUI(ILuaEditDocumentProject prjDoc)
        {
            foreach (LuaEditPageControlPage page in projectPropertiesPageControl.Pages)
            {
                page.PageContent.SetUIFromData(prjDoc);
            }
        }

        public void GetUI(ILuaEditDocumentProject prjDoc)
        {
            foreach (LuaEditPageControlPage page in projectPropertiesPageControl.Pages)
            {
                page.PageContent.SetDataFromUI(prjDoc);
            }
        }

        public bool TerminateUI()
        {
            return true;
        }

        #endregion
    }

    public class XmlLuaProjectProperties
    {
        #region Members

        [DefaultValue(DebugStartAction.StartProject)]
        public DebugStartAction StartAction;
        public string ExternalProgram;
        public string CmdLineArgs;
        [DefaultValue(0)]
        public int RemotePort;
        public string StartupFileName;
        public string WorkingDir;
        public bool UseRemoteMachine;
        public string RemoteMachineName;
        public string BuildOutputDir;
        public string PreBuildEventCmdLine;
        public string PostBuildEventCmdLine;
        [DefaultValue(PostBuildRunType.OnSuccessful)]
        public PostBuildRunType RunPostBuildEvent;

        #endregion

        #region Constructors

        public XmlLuaProjectProperties()
        {
        }

        public XmlLuaProjectProperties(DebugStartAction startAction,
                                       string externalProgram,
                                       string cmdLineArgs,
                                       int remotePort,
                                       string startupFileName,
                                       string workingDir,
                                       bool useRemoteMachine,
                                       string remoteMachineName,
                                       string buildOutputDir,
                                       string preBuildEventCmdLine,
                                       string postBuildEventCmdLine,
                                       PostBuildRunType runPostBuildEvent)
        {
            StartAction = startAction;
            ExternalProgram = externalProgram;
            CmdLineArgs = cmdLineArgs;
            RemotePort = remotePort;
            StartupFileName = startupFileName;
            WorkingDir = workingDir;
            UseRemoteMachine = useRemoteMachine;
            RemoteMachineName = remoteMachineName;
            BuildOutputDir = buildOutputDir;
            PreBuildEventCmdLine = preBuildEventCmdLine;
            PostBuildEventCmdLine = postBuildEventCmdLine;
            RunPostBuildEvent = runPostBuildEvent;
        }

        #endregion
    }
}