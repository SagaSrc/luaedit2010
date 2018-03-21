using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using DotNetLib.Controls;
using LuaEdit.Win32;
using LuaEdit.Documents.ProjectProperties;
using LuaEdit.Interfaces;
using LuaEdit.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Documents
{
    public class LuaProjectDocument : DocumentGroupBase, ILuaEditDocumentProject
    {
        #region Members

        private HistoryStack _historyStack;
        private ContextMenuStrip _contextMenu;
        private ILuaEditProjectProperties _projectProperties;
        private int _lastHistoryStackIndexSinceSave = -1;

        public const string Extension = ".luaproj";
        public const string DescriptiveName = "Lua Project";

        #endregion

        #region Constructors

        public LuaProjectDocument() :
            base()
        {
            Initialize();
        }

        public LuaProjectDocument(IDocumentRef docRef) :
            base(docRef)
        {
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether there is a command to undo or not
        /// </summary>
        public bool CanUndo
        {
            get { return _historyStack.CanUndo; }
        }

        /// <summary>
        /// Gets whether there is a command to redo or not
        /// </summary>
        public bool CanRedo
        {
            get { return _historyStack.CanRedo; }
        }

        /// <summary>
        /// Get the associated DockContent instance
        /// </summary>
        public override DockContent DockContent
        {
            get
            {
                ProjectPropertiesMain _projectPropertiesUI = _projectProperties as ProjectPropertiesMain;
                if (_projectPropertiesUI == null || _projectPropertiesUI.IsDisposed)
                {
                    _projectProperties = new ProjectPropertiesMain(this, _historyStack, _projectProperties);
                    _projectProperties.SetUI(this);
                }

                return _projectProperties as DockContent;
            }
        }

        /// <summary>
        /// Get the type of document
        /// </summary>
        public override Type DocumentType
        {
            get { return typeof(LuaProjectDocument); }
        }

        /// <summary>
        /// Get the project's properties
        /// </summary>
        public ILuaEditProjectProperties ProjectProperties
        {
            get { return _projectProperties; }
        }

        #endregion

        #region Events

        private void OnHistoryStackCurrentIndexChanged(object sender, EventArgs e)
        {
            this.IsModified = _lastHistoryStackIndexSinceSave != _historyStack.CurrentIndex;
        }

        #region Context Menu Events

        /// <summary>
        /// Occurs when the add new item menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddNewItemMenuClicked(object sender, EventArgs e)
        {
            string title = string.Format("Add New Item - {0}", this.ToString());
            DocumentsManager.Instance.NewDocument(title, this, LuaEdit.HelperDialogs.NewItemTypes.Item);
        }

        /// <summary>
        /// Occurs when the add existing item menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddExistingItemMenuClicked(object sender, EventArgs e)
        {
            string title = string.Format("Add Existing Item - {0}", this.ToString());
            DocumentsManager.Instance.OpenDocuments(typeof(ILuaEditDocumentEditable), this, title, Path.GetDirectoryName(this.FileName));
        }

        /// <summary>
        /// Occurs when the set as active project item menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSetAsActiveProjectItemMenuClicked(object sender, EventArgs e)
        {
            DocumentsManager.Instance.CurrentSolution.ActiveProject = this;
        }

        /// <summary>
        /// Occurs when the properties item menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertiesMenuClicked(object sender, EventArgs e)
        {
            DocumentsManager.Instance.OpenDocument(this);
        }

        #endregion

        #endregion

        #region Methods

        private void Initialize()
        {
            _historyStack = new HistoryStack();
            _historyStack.CurrentIndexChanged += OnHistoryStackCurrentIndexChanged;
            _projectProperties = new ProjectPropertiesMain(this, _historyStack);
            _projectProperties.SetUI(this);
        }

        internal XmlLuaProjectDocument ToXml(XmlLuaProjectDocument objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                objectToSerialize = new XmlLuaProjectDocument();
            }

            // Serialize project's properties
            _projectProperties.GetUI(this);
            objectToSerialize.ProjectProperties = new XmlLuaProjectProperties(_projectProperties.StartAction,
                                                                              _projectProperties.ExternalProgram,
                                                                              _projectProperties.CommandLineArguments,
                                                                              _projectProperties.RemotePort,
                                                                              _projectProperties.StartupFileName,
                                                                              _projectProperties.WorkingDirectory,
                                                                              _projectProperties.UseRemoteMachine,
                                                                              _projectProperties.RemoteMachineName,
                                                                              _projectProperties.BuildOutputDirectory,
                                                                              _projectProperties.PreBuildEventCmdLine,
                                                                              _projectProperties.PostBuildEventCmdLine,
                                                                              _projectProperties.RunPostBuildEvent);

            base.ToXml(objectToSerialize);

            return objectToSerialize;
        }

        internal void FromXml(XmlLuaProjectDocument objectToDeserialize)
        {
            base.FromXml(objectToDeserialize);

            // Deserialize project's properties
            if (objectToDeserialize.ProjectProperties != null)
            {
                _historyStack = new HistoryStack();
                _historyStack.CurrentIndexChanged += OnHistoryStackCurrentIndexChanged;
                _projectProperties = new ProjectPropertiesMain(this, _historyStack, objectToDeserialize.ProjectProperties);
            }

            _projectProperties.SetUI(this);
        }

        public override string ToString()
        {
 	         return Path.GetFileNameWithoutExtension(_fileName);
        }

        public override void ToTreeListViewItem(TreeListViewItem parentItem)
        {
            int imageIndex = DocumentFactory.Instance.GetDocumentSmallImageIndex(typeof(LuaProjectDocument));
            TreeListViewItem node = new TreeListViewItem(this, imageIndex);
            node.ToolTip = _fileName;
            node.Tag = this;

            if (this == DocumentsManager.Instance.CurrentSolution.ActiveProject)
            {
                node.Font = new Font(node.Font, FontStyle.Bold);
            }

            parentItem.Items.Add(node);
            base.ToTreeListViewItem(node);
        }

        public override void SaveDocument(string fileName)
        {
            base.SaveDocument(fileName);
            _lastHistoryStackIndexSinceSave = _historyStack.CurrentIndex;
            _fileWatcher.EnableRaisingEvents = false;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlLuaProjectDocument));

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(_fileName, Encoding.UTF8))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(xmlTextWriter, ToXml(null));
                xmlTextWriter.Flush();
            }

            _fileWatcher.EnableRaisingEvents = true;
        }

        public override void LoadDocument(string fileName)
        {
            try
            {
                _opening = true;
                base.LoadDocument(fileName);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlLuaProjectDocument));

                using (XmlTextReader xmlTextReader = new XmlTextReader(_fileName))
                {
                    FromXml(xmlSerializer.Deserialize(xmlTextReader) as XmlLuaProjectDocument);
                }
            }
            finally
            {
                _opening = false;
            }
        }

        public void Undo()
        {
            if (_historyStack.CanUndo)
            {
                _historyStack.Undo();
            }
        }

        public void Redo()
        {
            if (_historyStack.CanRedo)
            {
                _historyStack.Redo();
            }
        }

        #endregion
    }

    public class XmlLuaProjectDocument : XmlLuaGroupDocumentBase
    {
        #region Members

        public XmlLuaProjectProperties ProjectProperties;

        #endregion

        #region Constructors

        public XmlLuaProjectDocument() :
            base()
        {
        }

        #endregion
    }
}
