using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Utils;

namespace LuaEdit.Documents.ProjectProperties
{
    public partial class ProjectPropertiesBuildEvents : UserControl, ILuaEditPageControlPageContent
    {
        #region Members

        private bool _isUpdatingUIFromData = false;
        private HistoryStack _historyStack = null;
        private string _preBuildEventCmdLineOldVal = string.Empty;
        private string _postBuildEventCmdLineOldVal = string.Empty;
        private string _runPostBuildEventOldVal = string.Empty;

        #endregion

        #region Constructors

        public ProjectPropertiesBuildEvents(HistoryStack historyStack)
        {
            _historyStack = historyStack;
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void txtPreBuildEventCmdLine_Enter(object sender, EventArgs e)
        {
            _preBuildEventCmdLineOldVal = txtPreBuildEventCmdLine.Text;
        }

        private void txtPreBuildEventCmdLine_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(txtPreBuildEventCmdLine, txtPreBuildEventCmdLine.GetType(), "Text", txtPreBuildEventCmdLine.Text, _preBuildEventCmdLineOldVal);
            }
        }

        private void txtPostBuildEventCmdLine_Enter(object sender, EventArgs e)
        {
            _postBuildEventCmdLineOldVal = txtPostBuildEventCmdLine.Text;
        }

        private void txtPostBuildEventCmdLine_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(txtPostBuildEventCmdLine, txtPostBuildEventCmdLine.GetType(), "Text", txtPostBuildEventCmdLine.Text, _postBuildEventCmdLineOldVal);
            }
        }

        private void cboRunPostBuildEvent_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(cboRunPostBuildEvent, cboRunPostBuildEvent.GetType(), "Text", cboRunPostBuildEvent.Text, _runPostBuildEventOldVal);
            }

            _runPostBuildEventOldVal = cboRunPostBuildEvent.Text;
        }

        #endregion

        #region Properties

        public Control Content
        {
            get { return this; }
        }

        public string Title
        {
            get { return "Build Events"; }
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

        public void SetUIFromData(object source)
        {
            try
            {
                _isUpdatingUIFromData = true;
                ILuaEditDocumentProject luaPrj = source as ILuaEditDocumentProject;

                if (luaPrj != null)
                {
                    ILuaEditProjectProperties luaPrjProps = luaPrj.ProjectProperties;

                    if (luaPrjProps != null)
                    {
                        txtPreBuildEventCmdLine.Text = luaPrjProps.PreBuildEventCmdLine;
                        txtPostBuildEventCmdLine.Text = luaPrjProps.PostBuildEventCmdLine;
                        cboRunPostBuildEvent.SelectedIndex = (int)luaPrjProps.RunPostBuildEvent;
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
                    luaPrjProps.PreBuildEventCmdLine = txtPreBuildEventCmdLine.Text;
                    luaPrjProps.PostBuildEventCmdLine = txtPostBuildEventCmdLine.Text;
                    luaPrjProps.RunPostBuildEvent = (PostBuildRunType)cboRunPostBuildEvent.SelectedIndex;
                }
            }
        }

        #endregion
    }
}
