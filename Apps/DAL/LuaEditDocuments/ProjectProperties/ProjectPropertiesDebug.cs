using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Utils;
using LuaEdit.Win32;

namespace LuaEdit.Documents.ProjectProperties
{
    public partial class ProjectPropertiesDebug : UserControl, ILuaEditPageControlPageContent
    {
        #region Members

        private bool _isUpdatingUIFromData = false;
        private HistoryStack _historyStack = null;
        private Decimal _portOldVal = 0;
        private string _remoteMachineOldVal = string.Empty;
        private string _startupFileOldVal = string.Empty;
        private string _externalProgramOldVal = string.Empty;
        private string _workingDirOldVal = string.Empty;
        private string _cmdLineArgsOldVal = string.Empty;

        private ILuaEditDocumentProject _prjDoc;

        #endregion

        #region Constructors

        public ProjectPropertiesDebug(HistoryStack historyStack, ILuaEditDocumentProject prjDoc)
        {
            _prjDoc = prjDoc;
            _historyStack = historyStack;
            InitializeComponent();
            cboStartupFile.DisplayMember = "NodeText";
        }

        #endregion

        #region Events

        private void radStartProject_CheckedChanged(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(radStartProject, radStartProject.GetType(), "Checked", radStartProject.Checked, !radStartProject.Checked);
            }

            SetupStartOption();
        }

        private void radStartExternalProgram_CheckedChanged(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(radStartExternalProgram, radStartExternalProgram.GetType(), "Checked", radStartExternalProgram.Checked, !radStartExternalProgram.Checked);
            }

            SetupStartOption();
        }

        private void chkRemoteMachine_CheckedChanged(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(chkRemoteMachine, chkRemoteMachine.GetType(), "Checked", chkRemoteMachine.Checked, !chkRemoteMachine.Checked);
            }

            txtRemoteMachine.Enabled = chkRemoteMachine.Checked;
        }

        private void nudPort_Enter(object sender, EventArgs e)
        {
            _portOldVal = nudPort.Value;
        }

        private void nudPort_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(nudPort, nudPort.GetType(), "Value", nudPort.Value, _portOldVal);
            }
        }

        private void txtRemoteMachine_Enter(object sender, EventArgs e)
        {
            _remoteMachineOldVal = txtRemoteMachine.Text;
        }

        private void txtRemoteMachine_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(txtRemoteMachine, txtRemoteMachine.GetType(), "Text", txtRemoteMachine.Text, _remoteMachineOldVal);
            }
        }

        private void cboStartupFile_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(cboStartupFile, cboStartupFile.GetType(), "Text", cboStartupFile.Text, _startupFileOldVal);
            }

            _startupFileOldVal = cboStartupFile.Text;
        }

        private void txtExternalProgram_Enter(object sender, EventArgs e)
        {
            _externalProgramOldVal = txtExternalProgram.Text;
        }

        private void txtExternalProgram_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(txtExternalProgram, txtExternalProgram.GetType(), "Text", txtExternalProgram.Text, _externalProgramOldVal);
            }
        }

        private void txtWorkingDir_Enter(object sender, EventArgs e)
        {
            _workingDirOldVal = txtWorkingDir.Text;
        }

        private void txtWorkingDir_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(txtWorkingDir, txtWorkingDir.GetType(), "Text", txtWorkingDir.Text, _workingDirOldVal);
            }
        }

        private void txtCmdLineArgs_Enter(object sender, EventArgs e)
        {
            _cmdLineArgsOldVal = txtCmdLineArgs.Text;
        }

        private void txtCmdLineArgs_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(txtCmdLineArgs, txtCmdLineArgs.GetType(), "Text", txtCmdLineArgs.Text, _cmdLineArgsOldVal);
            }
        }

        private void btnBrowseWorkingDir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWorkingDir.Text))
            {
                folderBrowserWorkingDir.SelectedPath = txtWorkingDir.Text;
            }

            if (folderBrowserWorkingDir.ShowDialog() == DialogResult.OK)
            {
                string oldVal = txtWorkingDir.Text;
                txtWorkingDir.Text = folderBrowserWorkingDir.SelectedPath;
                txtWorkingDir.Focus();
                _workingDirOldVal = oldVal;
            }
        }

        private void btnBrowseExternalProgram_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtExternalProgram.Text))
            {
                externalProgramBrowser.FileName = txtExternalProgram.Text;
            }

            if (externalProgramBrowser.ShowDialog() == DialogResult.OK)
            {
                string progFileName = externalProgramBrowser.FileName;

                if (File.Exists(_prjDoc.FileName))
                {
                    progFileName = Win32Utils.GetRelativePath(progFileName, Path.GetDirectoryName(_prjDoc.FileName));
                }

                string oldVal = txtExternalProgram.Text;
                txtExternalProgram.Text = progFileName;
                txtExternalProgram.Focus();
                _externalProgramOldVal = oldVal;
            }
        }

        #endregion

        #region Properties

        public Control Content
        {
            get { return this; }
        }

        public string Title
        {
            get { return "Debug"; }
        }

        #endregion

        #region Methods

        private void PushUndoRedoCmd(object obj, Type objType, string propName, object newVal, object oldVal)
        {
            if (!newVal.Equals(oldVal))
            {
                UndoRedoCommand cmd = new UndoRedoCommand(obj, objType, propName, newVal, oldVal);
                _historyStack.PushHistoryItem(cmd);
            }
        }

        private void GetEditableFilesFromPrjRecursive(ILuaEditDocumentGroup docGrp, List<ILuaEditDocumentEditable> scriptList)
        {
            foreach (ILuaEditDocumentEditable doc in docGrp.Documents)
            {
                scriptList.Add(doc as ILuaEditDocumentEditable);
            }

            foreach (ILuaEditDocumentGroup docGrpChild in docGrp.DocumentGroups)
            {
                GetEditableFilesFromPrjRecursive(docGrpChild, scriptList);
            }

            foreach (ILuaEditDocumentFolder docFolderChild in docGrp.DocumentFolders)
            {
                GetEditableFilesFromPrjRecursive(docFolderChild, scriptList);
            }
        }

        private void SetupStartOption()
        {
            cboStartupFile.Enabled = radStartProject.Checked;
            txtExternalProgram.Enabled = radStartExternalProgram.Checked;
            btnBrowseExternalProgram.Enabled = radStartExternalProgram.Checked;
        }

        public void SetUIFromData(object source)
        {
            try
            {
                _isUpdatingUIFromData = true;
                ILuaEditDocumentProject luaPrj = source as ILuaEditDocumentProject;

                if (luaPrj != null)
                {
                    ILuaEditProjectProperties luaPrjProps = luaPrj.ProjectProperties;
                    List<ILuaEditDocumentEditable> docList = new List<ILuaEditDocumentEditable>();
                    GetEditableFilesFromPrjRecursive(luaPrj, docList);

                    cboStartupFile.Items.Clear();
                    int startupFileSelIndex = cboStartupFile.Items.Add("Currently Selected File");

                    foreach (ILuaEditDocumentEditable doc in docList)
                    {
                        int index = cboStartupFile.Items.Add(doc);
                        string relativeDocPath = Win32Utils.GetRelativePath(doc.FileName, Path.GetDirectoryName(_prjDoc.FileName));

                        if (luaPrjProps != null && luaPrjProps.StartupFileName == relativeDocPath)
                        {
                            startupFileSelIndex = index;
                        }
                    }

                    cboStartupFile.SelectedIndex = startupFileSelIndex;

                    if (luaPrjProps != null)
                    {
                        radStartProject.Checked = luaPrjProps.StartAction == DebugStartAction.StartProject;
                        radStartExternalProgram.Checked = luaPrjProps.StartAction == DebugStartAction.StartExternalProgram;
                        txtExternalProgram.Text = luaPrjProps.ExternalProgram;
                        txtCmdLineArgs.Text = luaPrjProps.CommandLineArguments;
                        chkRemoteMachine.Checked = luaPrjProps.UseRemoteMachine;
                        txtRemoteMachine.Text = luaPrjProps.RemoteMachineName;
                        nudPort.Value = luaPrjProps.RemotePort;
                        txtWorkingDir.Text = luaPrjProps.WorkingDirectory;
                    }
                }
            }
            finally
            {
                _isUpdatingUIFromData = false;
            }
        }

        public void SetDataFromUI(object destination)
        {
            ILuaEditDocumentProject luaPrj = destination as ILuaEditDocumentProject;

            if (luaPrj != null)
            {
                ILuaEditProjectProperties luaPrjProps = luaPrj.ProjectProperties;

                if (luaPrjProps != null)
                {
                    if (cboStartupFile.SelectedIndex == 0)
                    {
                        luaPrjProps.StartupFileName = string.Empty;
                    }
                    else if (cboStartupFile.SelectedIndex > 0)
                    {
                        luaPrjProps.StartupFileName = Win32Utils.GetRelativePath((cboStartupFile.Items[cboStartupFile.SelectedIndex] as ILuaEditDocumentEditable).FileName, Path.GetDirectoryName(_prjDoc.FileName));
                    }

                    luaPrjProps.StartAction = radStartProject.Checked ? DebugStartAction.StartProject : DebugStartAction.StartExternalProgram;
                    luaPrjProps.UseRemoteMachine = chkRemoteMachine.Checked;
                    luaPrjProps.RemoteMachineName = txtRemoteMachine.Text;
                    luaPrjProps.RemotePort = Convert.ToInt32(nudPort.Value);
                    luaPrjProps.WorkingDirectory = txtWorkingDir.Text;
                    luaPrjProps.ExternalProgram = txtExternalProgram.Text;
                    luaPrjProps.CommandLineArguments = txtCmdLineArgs.Text;
                }
            }
        }

        #endregion
    }
}