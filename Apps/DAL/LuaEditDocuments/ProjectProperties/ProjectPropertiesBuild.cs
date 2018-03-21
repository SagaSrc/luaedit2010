using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Utils;

namespace LuaEdit.Documents.ProjectProperties
{
    public partial class ProjectPropertiesBuild : UserControl, ILuaEditPageControlPageContent
    {
        #region Members

        private bool _isUpdatingUIFromData = false;
        private HistoryStack _historyStack = null;
        private string _outputDirOldVal = string.Empty;

        #endregion

        #region Constructors

        public ProjectPropertiesBuild(HistoryStack historyStack)
        {
            _historyStack = historyStack;
            InitializeComponent();
        }

        #endregion

        #region Properties

        public Control Content
        {
            get { return this; }
        }

        public string Title
        {
            get { return "Build"; }
        }

        #endregion

        #region Event Handlers

        private void btnBrowseOutputDir_Click(object sender, EventArgs e)
        {
            folderBrowserOutputDir.SelectedPath = txtOutputDir.Text;

            if (folderBrowserOutputDir.ShowDialog() == DialogResult.OK)
            {
                txtOutputDir.Text = folderBrowserOutputDir.SelectedPath;
                _outputDirOldVal = txtOutputDir.Text;
                txtOutputDir.Focus();
            }
        }

        private void txtOutputDir_Enter(object sender, EventArgs e)
        {
            _outputDirOldVal = txtOutputDir.Text;
        }

        private void txtOutputDir_Leave(object sender, EventArgs e)
        {
            if (!_historyStack.IsUndoingOrRedoing && !_isUpdatingUIFromData)
            {
                PushUndoRedoCmd(txtOutputDir, txtOutputDir.GetType(), "Text", txtOutputDir.Text, _outputDirOldVal);
            }
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
            ILuaEditDocumentProject luaPrj = source as ILuaEditDocumentProject;

            if (luaPrj != null)
            {
                try
                {
                    _isUpdatingUIFromData = true;
                    ILuaEditProjectProperties luaPrjProps = luaPrj.ProjectProperties;

                    if (luaPrjProps != null)
                    {
                        txtOutputDir.Text = luaPrjProps.BuildOutputDirectory;
                    }
                }
                finally
                {
                    _isUpdatingUIFromData = false;
                }
            }
        }

        public void SetDataFromUI(object destination)
        {
            ILuaEditDocumentProject luaPrj = destination as ILuaEditDocumentProject;

            if (luaPrj != null)
            {
                try
                {
                    _isUpdatingUIFromData = true;
                    ILuaEditProjectProperties luaPrjProps = luaPrj.ProjectProperties;

                    if (luaPrjProps != null)
                    {
                        luaPrjProps.BuildOutputDirectory = txtOutputDir.Text;
                    }
                }
                finally
                {
                    _isUpdatingUIFromData = false;
                }
            }
        }

        #endregion
    }
}