using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DotNetLib.Controls;
using LuaEdit.Documents.DocumentUtils;
using LuaEdit.Interfaces;
using LuaEdit.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Documents
{
    public class DocumentBase : ILuaEditDocument
    {
        #region Members

        protected bool _enableFileWatcher = true;
        protected ReferenceCounter _refCounter = null;
        protected Guid _id = Guid.NewGuid();
        protected ILuaEditDocumentGroup _parentDoc = null;
        protected IDocumentRef _docRef;
        protected string _fileName = string.Empty;
        protected bool _isModified = false;
        protected bool _opening = false;
        protected bool _terminating = false;
        protected bool _terminated = false;

        protected FileSystemWatcher _fileWatcher = null;
        protected bool _lastKnownROAttr = false;

        public event EventHandler FileNameChanged;
        public event EventHandler DocumentTerminated;
        public event EventHandler ReadOnlyChanged;
        public event EventHandler ModifiedChanged;
        public event EventHandler ContentChanged;
        public event EventHandler DiskFileChanged;

        #endregion

        #region Constructors

        public DocumentBase(bool enableFileWatcher)
        {
            _enableFileWatcher = enableFileWatcher;

            _refCounter = new ReferenceCounter();
            _refCounter.ReferenceCountChanged += OnReferenceCountChanged;

            // Create file watchers
            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.IncludeSubdirectories = false;
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes;
            _fileWatcher.Changed += OnDiskFileChanged;
        }

        public DocumentBase() : 
            this(true)
        {
        }

        public DocumentBase(IDocumentRef docRef) :
            this()
        {
            _docRef = docRef;
            this.FileName = docRef.FileName;
            this.ParentDocument = docRef.ParentDocument;
        }

        #endregion

        #region Event Handlers

        private void OnReferenceCountChanged(object sender, EventArgs e)
        {
            if (_refCounter == 0)
            {
                TerminateDocument();
            }
        }

        /// <summary>
        /// Occurs when the file has changed on the disk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDiskFileChanged(object sender, FileSystemEventArgs e)
        {
            if (_enableFileWatcher)
            {
                bool currentROAttr = this.ReadOnly;

                if (_lastKnownROAttr != currentROAttr)
                {
                    _lastKnownROAttr = currentROAttr;
                    RaiseReadOnlyChangedEvent();
                }
                else
                {
                    DocumentsManager.Instance.NotifyDocumentHasChangedOnDisk(_fileName, this);
                    RaiseDiskFileChangedEvent();
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether this document was terminated. A terminated
        /// document should not be edited anymore. Only clean-up
        /// operations should be done.
        /// </summary>
        public bool IsTerminated
        {
            get { return _terminated; }
        }

        /// <summary>
        /// Gets or sets the number of document which have a reference
        /// to this document
        /// </summary>
        public ReferenceCounter ReferenceCount
        {
            get { return _refCounter; }
            set { _refCounter = value; }
        }

        /// <summary>
        /// The document reference to this document
        /// </summary>
        public IDocumentRef DocumentRef
        {
            get { return _docRef; }
        }

        /// <summary>
        /// Get/Set the script's filename
        /// </summary>
        public virtual string FileName
        {
            get { return _fileName; }
            set
            {
                if (value != _fileName)
                {
                    _fileName = value;

                    RenewFileWatcher();

                    _lastKnownROAttr = this.ReadOnly;
                    RaiseFileNameChangedEvent();
                }
            }
        }

        /// <summary>
        /// Get/Set the script's modification status
        /// </summary>
        public virtual bool IsModified
        {
            get { return _isModified; }
            set
            {
                if (!_opening && !_terminating && value != _isModified)
                {
                    _isModified = value;
                    RaiseModifiedChangedEvent();
                }
            }
        }

        /// <summary>
        /// Get/Set this document's read-only status
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                if (Path.IsPathRooted(_fileName) && File.Exists(_fileName) || Directory.Exists(_fileName))
                {
                    _fileWatcher.EnableRaisingEvents = false;
                    FileAttributes fa = File.GetAttributes(_fileName);
                    _fileWatcher.EnableRaisingEvents = true;
                    return (FileAttributes.ReadOnly & fa) == FileAttributes.ReadOnly;
                }

                return false;
            }
            set
            {
                if (Path.IsPathRooted(_fileName) && File.Exists(_fileName) || Directory.Exists(_fileName))
                {
                    _fileWatcher.EnableRaisingEvents = false;
                    FileInfo fi = new FileInfo(_fileName);
                    fi.IsReadOnly = value;
                    _fileWatcher.EnableRaisingEvents = true;
                    _lastKnownROAttr = value;
                }
            }
        }

        /// <summary>
        /// Get the associated DockContent instance
        /// </summary>
        public virtual DockContent DockContent
        {
            get { return null; }
        }

        /// <summary>
        /// Get the type of document
        /// </summary>
        public virtual Type DocumentType
        {
            get { return typeof(DocumentBase); }
        }

        /// <summary>
        /// Get/Set the parent document (EG: Solution, Project...)
        /// </summary>
        public virtual ILuaEditDocumentGroup ParentDocument
        {
            get { return _parentDoc; }
            set { _parentDoc = value; }
        }

        /// <summary>
        /// Get the text associated to this document's node
        /// </summary>
        public string NodeText
        {
            get { return GetNodeText(); }
        }

        #endregion

        #region Methods

        public virtual void Dispose()
        {
        }

        public override string ToString()
        {
            return Path.GetFileName(_fileName);
        }

        internal void RenewFileWatcher()
        {
            if (Path.IsPathRooted(_fileName) && File.Exists(_fileName) || Directory.Exists(_fileName))
            {
                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher.Path = Path.GetDirectoryName(_fileName);
                _fileWatcher.Filter = Path.GetFileName(_fileName);
                _fileWatcher.EnableRaisingEvents = true;
            }
        }

        internal void RaiseFileNameChangedEvent()
        {
            if (FileNameChanged != null)
            {
                FileNameChanged(this, EventArgs.Empty);
            }
        }

        internal void RaiseDiskFileChangedEvent()
        {
            if (DiskFileChanged != null)
            {
                DiskFileChanged(this, EventArgs.Empty);
            }
        }

        internal void RaiseModifiedChangedEvent()
        {
            if (ModifiedChanged != null)
            {
                ModifiedChanged(this, EventArgs.Empty);
            }
        }

        internal void RaiseContentChangedEvent()
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, EventArgs.Empty);
            }
        }

        internal void RaiseReadOnlyChangedEvent()
        {
            if (ReadOnlyChanged != null)
            {
                ReadOnlyChanged(this, EventArgs.Empty);
            }
        }

        internal void RaiseDocumentTerminatedEvent()
        {
            if (DocumentTerminated != null)
            {
                DocumentTerminated(this, EventArgs.Empty);
            }
        }

        public virtual ContextMenuStrip GetContextMenu()
        {
            return null;
        }

        public virtual void ToTreeListViewItem(TreeListViewItem parentItem)
        {
            int imageIndex = DocumentFactory.Instance.GetDocumentSmallImageIndex(this.DocumentType);
            TreeListViewItem node = new TreeListViewItem(this, imageIndex);
            node.ToolTip = _fileName;
            node.Tag = this;
            parentItem.Items.Add(node);
        }

        public virtual string GetNodeText()
        {
            return ToString();
        }

        public virtual void SaveDocument(string fileName)
        {
            this.FileName = fileName;
            this.IsModified = false;
        }

        public virtual void LoadDocument(string fileName)
        {
            this.IsModified = false;
            this.FileName = fileName;
        }

        protected virtual bool TerminateDocument()
        {
            try
            {
                ContentChanged = null;
                DiskFileChanged = null;
                ModifiedChanged = null;
                ReadOnlyChanged = null;
                _fileWatcher.Dispose();

                if (this.DockContent != null)
                {
                    this.DockContent.Close();
                }

                _terminated = true;
                RaiseDocumentTerminatedEvent();
                DocumentTerminated = null;
            }
            catch (Exception e)
            {
                string msg = string.Format("Error while closing document '{0}': {1}", this.FileName, e.Message);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #endregion
    }
}
