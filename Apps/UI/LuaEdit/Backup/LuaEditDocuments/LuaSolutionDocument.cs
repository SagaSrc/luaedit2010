using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using DotNetLib.Controls;
using LuaEdit.Win32;
using LuaEdit.Interfaces;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Documents
{
    public class LuaSolutionDocument : DocumentGroupBase, ILuaEditDocumentSolution
    {
        #region Members

        private ContextMenuStrip _contextMenu;
        private ILuaEditDocumentProject _activeProject;

        public const string Extension = ".lsln";
        public const string DescriptiveName = "Lua Solution";

        public event ActiveProjectEventHandler ActiveProjectChanged;

        #endregion

        #region Constructors

        public LuaSolutionDocument() :
            base()
        {
        }

        public LuaSolutionDocument(IDocumentRef docRef)
            : base(docRef)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get/set the currently active project for that solution
        /// </summary>
        public ILuaEditDocumentProject ActiveProject
        {
            get { return _activeProject; }
            set
            {
                if (value != _activeProject)
                {
                    if (!_opening)
                    {
                        this.IsModified = true;
                    }

                    ILuaEditDocumentProject previousActivePrj = _activeProject;
                    _activeProject = value;

                    if (ActiveProjectChanged != null)
                    {
                        ActiveProjectChanged(this, new ActiveProjectEventArgs(_activeProject, previousActivePrj));
                    }
                }
            }
        }

        

        /// <summary>
        /// Get the associated DockContent instance
        /// </summary>
        public override DockContent DockContent
        {
            get { return null; }
        }

        /// <summary>
        /// Get the type of document
        /// </summary>
        public override Type DocumentType
        {
            get { return typeof(LuaSolutionDocument); }
        }

        #endregion

        #region Events

        #region Context Menu Events

        /// <summary>
        /// Occurs when the add new project menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddNewProjectMenuClicked(object sender, EventArgs e)
        {
            if (this.CanAdd())
            {
                string title = string.Format("Add New Project - {0}", this.ToString());
                DocumentsManager.Instance.NewDocument(title, this, LuaEdit.HelperDialogs.NewItemTypes.ItemGroup);
            }
        }

        /// <summary>
        /// Occurs when the add existing project menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddExistingProjectMenuClicked(object sender, EventArgs e)
        {
            if (this.CanAdd())
            {
                string title = string.Format("Add Existing Project - {0}", this.ToString());
                DocumentsManager.Instance.OpenProjectDocument("All Project Files", this, title, Path.GetDirectoryName(this.FileName));
            }
        }

        /// <summary>
        /// Occurs when the add new item menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddNewItemMenuClicked(object sender, EventArgs e)
        {
            if (this.CanAdd())
            {
                string title = string.Format("Add New Item - {0}", this.ToString());
                DocumentsManager.Instance.NewDocument(title, this, LuaEdit.HelperDialogs.NewItemTypes.Item);
            }
        }

        /// <summary>
        /// Occurs when the add existing item menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddExistingItemMenuClicked(object sender, EventArgs e)
        {
            if (this.CanAdd())
            {
                string title = string.Format("Add Existing Item - {0}", this.ToString());
                DocumentsManager.Instance.OpenDocuments(typeof(ILuaEditDocumentEditable), this, title, Path.GetDirectoryName(this.FileName));
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Validate whether or not documents can be added to this solution document
        /// </summary>
        /// <returns>True if you can. Otherwise false.</returns>
        /// <remarks>You can't add documents to a solution if it's never been saved on the disk yet</remarks>
        public override bool CanAdd()
        {
            if (!File.Exists(_fileName) || !Path.IsPathRooted(_fileName))
                return DocumentsManager.Instance.SaveDocument(typeof(LuaSolutionDocument), this, true);
            else
                return true;
        }

        private XmlLuaSolutionDocument ToXml(XmlLuaSolutionDocument objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                objectToSerialize = new XmlLuaSolutionDocument();
            }

            base.ToXml(objectToSerialize);

            objectToSerialize.ActiveProject = _activeProject == null ? null : new DocumentRef(_activeProject.FileName);

            return objectToSerialize;
        }

        private void FromXml(XmlLuaSolutionDocument objectToDeserialize)
        {
            base.FromXml(objectToDeserialize);

            if (objectToDeserialize.ActiveProject != null)
            {
                _activeProject = FindDocumentGroup(objectToDeserialize.ActiveProject.FileName) as ILuaEditDocumentProject;
            }
        }

        public override string ToString()
        {
            int projectCount = 0;

            foreach (ILuaEditDocumentGroup docGrp in _documentGroups)
            {
                if (docGrp is LuaProjectDocument)
                    ++projectCount;
            }

            return string.Format("Solution '{0}' ({1} projects)", Path.GetFileNameWithoutExtension(_fileName), projectCount);
        }

        public override void ToTreeListViewItem(TreeListViewItem parentItem)
        {
            int imageIndex = DocumentFactory.Instance.GetDocumentSmallImageIndex(typeof(LuaSolutionDocument));
            TreeListViewItem node = new TreeListViewItem(this, imageIndex);
            node.ToolTip = _fileName;
            node.Tag = this;

            parentItem.Items.Add(node);
            base.ToTreeListViewItem(node);
        }

        public override void SaveDocument(string fileName)
        {
            base.SaveDocument(fileName);
            _fileWatcher.EnableRaisingEvents = false;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlLuaSolutionDocument));

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(_fileName, Encoding.UTF8))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(xmlTextWriter, ToXml(null));
                xmlTextWriter.Flush();
            }

            RenewFileWatcher();
            _fileWatcher.EnableRaisingEvents = true;
        }

        public override void LoadDocument(string fileName)
        {
            try
            {
                _opening = true;
                base.LoadDocument(fileName);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlLuaSolutionDocument));

                using (XmlTextReader xmlTextReader = new XmlTextReader(_fileName))
                {
                    FromXml(xmlSerializer.Deserialize(xmlTextReader) as XmlLuaSolutionDocument);
                }
            }
            finally
            {
                _opening = false;
            }
        }

        public override void AddDocument(ILuaEditDocumentGroup docGrp)
        {
            base.AddDocument(docGrp);

            if (!_opening && DocumentGroups.Count == 1 && DocumentGroups[0] is ILuaEditDocumentProject)
            {
                this.ActiveProject = DocumentGroups[0] as ILuaEditDocumentProject;
            }
        }

        public override void RemoveDocument(ILuaEditDocumentGroup docGrp)
        {
            if (docGrp == this.ActiveProject)
            {
                this.ActiveProject = null;
            }

            base.RemoveDocument(docGrp);
        }

        #endregion
    }

    public class XmlLuaSolutionDocument : XmlLuaGroupDocumentBase
    {
        #region Members

        public DocumentRef ActiveProject;

        #endregion

        #region Constructors

        public XmlLuaSolutionDocument() :
            base()
        {
        }

        #endregion
    }

    public delegate void ActiveProjectEventHandler(object sender, ActiveProjectEventArgs e);
    public class ActiveProjectEventArgs
    {
        #region Members

        private ILuaEditDocumentProject _currentlyActivePrj;
        private ILuaEditDocumentProject _previouslyActivePrj;

        #endregion

        #region Constructors

        public ActiveProjectEventArgs(ILuaEditDocumentProject currentlyActivePrj,
                                      ILuaEditDocumentProject previouslyActivePrj)
        {
            _currentlyActivePrj = currentlyActivePrj;
            _previouslyActivePrj = previouslyActivePrj;
        }

        #endregion

        #region Properties

        public ILuaEditDocumentProject CurrentlyActiveProject
        {
            get { return _currentlyActivePrj; }
        }

        public ILuaEditDocumentProject PreviouslyActiveProject
        {
            get { return _previouslyActivePrj; }
        }

        #endregion
    }
}
