using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DotNetLib.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Win32;

namespace LuaEdit.Documents
{
    public class DocumentGroupBase : DocumentBase, ILuaEditDocumentGroup
    {
        #region Members

        protected List<ILuaEditDocument> _documents;
        protected List<ILuaEditDocumentGroup> _documentGroups;
        protected List<ILuaEditDocumentFolder> _documentFolders;

        public event DocGroupDocActionEventHandler DocumentAdded;
        public event DocGroupDocActionEventHandler DocumentRemoved;
        public event DocumentFolderActionEventHandler FolderAdded;
        public event DocumentFolderActionEventHandler FolderRemoved;

        #endregion

        #region Constructors

        public DocumentGroupBase(bool enableFileWatcher) :
            base(enableFileWatcher)
        {
            Initialize();
        }

        public DocumentGroupBase() :
            this(true)
        {
        }

        public DocumentGroupBase(IDocumentRef docRef) :
            base(docRef)
        {
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of documents child of this document
        /// </summary>
        public List<ILuaEditDocument> Documents
        {
            get { return _documents; }
        }

        /// <summary>
        /// Gets the list of document groups child of this document
        /// </summary>
        public List<ILuaEditDocumentGroup> DocumentGroups
        {
            get { return _documentGroups; }
        }

        /// <summary>
        /// Gets the list of document folders child of this document
        /// </summary>
        public List<ILuaEditDocumentFolder> DocumentFolders
        {
            get { return _documentFolders; }
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            // Create member lists
            _documents = new List<ILuaEditDocument>();
            _documentGroups = new List<ILuaEditDocumentGroup>();
            _documentFolders = new List<ILuaEditDocumentFolder>();
        }

        /// <summary>
        /// Determine whether child documents can be added to this document group
        /// </summary>
        public virtual bool CanAdd()
        {
            return true;
        }

        public override void ToTreeListViewItem(TreeListViewItem parentItem)
        {
            foreach (ILuaEditDocumentGroup docGrp in _documentGroups)
            {
                docGrp.ToTreeListViewItem(parentItem);
            }

            foreach (ILuaEditDocument doc in _documents)
            {
                doc.ToTreeListViewItem(parentItem);
            }

            foreach (ILuaEditDocumentFolder docFolder in _documentFolders)
            {
                docFolder.ToTreeListViewItem(parentItem);
            }
        }

        public virtual void AddDocument(ILuaEditDocument doc)
        {
            if (_opening || FindDocument(doc.FileName) == null)
            {
                doc.ParentDocument = this;
                doc.ReferenceCount += this;
                _documents.Add(doc);

                if (!_opening)
                {
                    IsModified = true;

                    if (DocumentAdded != null)
                    {
                        DocumentAdded(this, new DocGroupDocActionEventArgs(doc));
                    }
                }
            }
        }

        public virtual void AddDocument(ILuaEditDocumentGroup docGrp)
        {
            if (_opening || FindDocumentGroup(docGrp.FileName) == null)
            {
                docGrp.ParentDocument = this;
                docGrp.ReferenceCount += this;
                _documentGroups.Add(docGrp);

                if (!_opening)
                {
                    IsModified = true;

                    if (DocumentAdded != null)
                    {
                        DocumentAdded(this, new DocGroupDocActionEventArgs(docGrp));
                    }
                }
            }
        }

        public virtual void AddDocument(ILuaEditDocumentFolder docFolder)
        {
            if (_opening || FindFolder(docFolder.FileName) == null)
            {
                docFolder.ParentDocument = this;
                _documentFolders.Add(docFolder);

                if (!_opening)
                {
                    IsModified = true;

                    if (DocumentAdded != null)
                    {
                        DocumentAdded(this, new DocGroupDocActionEventArgs(docFolder));
                    }
                }
            }
        }

        public virtual void RemoveDocument(ILuaEditDocument doc)
        {
            doc.ParentDocument = null;
            doc.ReferenceCount -= this;
            _documents.Remove(doc);

            if (!_terminating)
            {
                IsModified = true;

                if (DocumentRemoved != null)
                {
                    DocumentRemoved(this, new DocGroupDocActionEventArgs(doc));
                }
            }
        }

        public virtual void RemoveDocument(ILuaEditDocumentGroup docGrp)
        {
            docGrp.ParentDocument = null;
            docGrp.ReferenceCount -= this;
            _documentGroups.Remove(docGrp);

            if (!_terminating)
            {
                IsModified = true;

                if (DocumentRemoved != null)
                {
                    DocumentRemoved(this, new DocGroupDocActionEventArgs(docGrp));
                }
            }
        }

        public virtual void RemoveDocument(ILuaEditDocumentFolder docFolder)
        {
            docFolder.ParentDocument = null;
            docFolder.ReferenceCount -= this;
            _documentFolders.Remove(docFolder);

            if (!_terminating)
            {
                IsModified = true;

                if (DocumentRemoved != null)
                {
                    DocumentRemoved(this, new DocGroupDocActionEventArgs(docFolder));
                }
            }
        }

        public void GetAllDocuments(List<ILuaEditDocument> docs)
        {
            docs.Add(this);
            docs.AddRange(_documents);

            foreach (ILuaEditDocumentGroup docGrp in _documentGroups)
            {
                docGrp.GetAllDocuments(docs);
            }

            foreach (ILuaEditDocumentFolder docFolder in _documentFolders)
            {
                docFolder.GetAllDocuments(docs);
            }
        }

        public void GetAllDocumentsOfType<T>(List<T> docs) where T : ILuaEditDocument
        {
            if (typeof(T) is ILuaEditDocumentFolder)
            {
                foreach (ILuaEditDocumentFolder docFolder in _documentFolders)
                {
                    if (docFolder is T)
                    {
                        docs.Add((T)docFolder);
                    }
                }
            }
            else if (typeof(T) is ILuaEditDocumentGroup)
            {
                foreach (ILuaEditDocument doc in _documentGroups)
                {
                    if (doc is T)
                    {
                        docs.Add((T)doc);
                    }
                }

                foreach (ILuaEditDocumentFolder docFolder in _documentFolders)
                {
                    if (docFolder is T)
                    {
                        docs.Add((T)docFolder);
                    }
                }
            }
            else
            {
                foreach (ILuaEditDocument doc in _documents)
                {
                    if (doc is T)
                    {
                        docs.Add((T)doc);
                    }
                }
            }
        }

        public ILuaEditDocument FindDocument(string fileName)
        {
            fileName = fileName.ToLower();

            foreach (ILuaEditDocument childDoc in _documents)
            {
                if (childDoc.FileName.ToLower() == fileName)
                {
                    return childDoc;
                }
            }

            return null;
        }

        public ILuaEditDocumentGroup FindDocumentGroup(string fileName)
        {
            foreach (ILuaEditDocumentGroup childDocGrp in _documentGroups)
            {
                if (childDocGrp.FileName == fileName)
                {
                    return childDocGrp;
                }
            }

            return null;
        }

        public ILuaEditDocumentFolder FindFolder(string fileName)
        {
            foreach (ILuaEditDocumentFolder folderDoc in _documentFolders)
            {
                if (folderDoc.FileName == fileName)
                {
                    return folderDoc;
                }
            }

            return null;
        }

        public ILuaEditDocumentFolder CreateNewFolder()
        {
            ILuaEditDocumentFolder newFolder = null;
            DirectoryInfo dirInfo = DocumentsManager.Instance.CreateDirectory(Path.GetDirectoryName(this.FileName));

            if (dirInfo != null)
            {
                newFolder = new DocumentFolder(dirInfo);
                AddDocument(newFolder);
            }

            return newFolder;
        }

        protected override bool TerminateDocument()
        {
            try
            {
                _terminating = true;

                while (_documentGroups.Count > 0)
                {
                    ILuaEditDocumentGroup childDocGrp = _documentGroups[_documentGroups.Count - 1];
                    RemoveDocument(childDocGrp);
                }

                while (_documents.Count > 0)
                {
                    ILuaEditDocument childDoc = _documents[_documents.Count - 1];
                    RemoveDocument(childDoc);
                }

                return base.TerminateDocument();
            }
            finally
            {
                _terminating = false;
            }
        }

        internal XmlLuaGroupDocumentBase ToXml(XmlLuaGroupDocumentBase objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                objectToSerialize = new XmlLuaGroupDocumentBase();
            }

            // Serialize sub-documents
            int fileCount = 0;
            Array.Resize<LuaEdit.Documents.DocumentRef>(ref objectToSerialize.Documents, _documents.Count);
            foreach (ILuaEditDocument doc in _documents)
            {
                objectToSerialize.Documents[fileCount] = new DocumentRef(Win32Utils.GetRelativePath(doc.FileName, _fileName));
                ++fileCount;
            }

            // Serialize sub-folders
            int folderCount = 0;
            Array.Resize<XmlDocumentFolder>(ref objectToSerialize.DocumentFolders, _documentFolders.Count);
            foreach (ILuaEditDocumentFolder docFolder in _documentFolders)
            {
                DocumentFolder folder = docFolder as DocumentFolder;
                if (folder != null)
                {
                    objectToSerialize.DocumentFolders[folderCount] = folder.ToXml(null);
                    ++folderCount;
                }
            }

            // Serialize sub-group documents
            int grpCount = 0;
            Array.Resize<LuaEdit.Documents.DocumentRef>(ref objectToSerialize.DocumentGroups, _documentGroups.Count);
            foreach (ILuaEditDocumentGroup docGrp in _documentGroups)
            {
                objectToSerialize.DocumentGroups[grpCount] = new DocumentRef(Win32Utils.GetRelativePath(docGrp.FileName, _fileName));
                ++grpCount;
            }

            return objectToSerialize;
        }

        internal void FromXml(XmlLuaGroupDocumentBase objectToDeserialize)
        {
            // Deserialize sub-documents
            if (objectToDeserialize.Documents != null)
            {
                foreach (DocumentRef refDoc in objectToDeserialize.Documents)
                {
                    refDoc.FileName = Win32Utils.GetAbsolutePath(refDoc.FileName, _fileName);
                    ILuaEditDocument doc = DocumentsManager.Instance.BaseOpenDocument(refDoc);

                    if (doc != null)
                    {
                        this.AddDocument(doc);
                    }
                }
            }

            // Deserialize sub-folders
            if (objectToDeserialize.DocumentFolders != null)
            {
                foreach (XmlDocumentFolder xmlFolder in objectToDeserialize.DocumentFolders)
                {
                    DocumentFolder folderDoc = new DocumentFolder();
                    folderDoc.ParentDocument = this;
                    folderDoc.FromXml(xmlFolder);

                    if (folderDoc != null)
                    {
                        this.AddDocument(folderDoc as ILuaEditDocumentFolder);
                    }
                }
            }

            // Deserialize sub-group documents
            if (objectToDeserialize.DocumentGroups != null)
            {
                foreach (DocumentRef refDoc in objectToDeserialize.DocumentGroups)
                {
                    refDoc.FileName = Win32Utils.GetAbsolutePath(refDoc.FileName, _fileName);
                    ILuaEditDocumentGroup docGrp = DocumentsManager.Instance.BaseOpenDocument(refDoc) as ILuaEditDocumentGroup;

                    if (docGrp != null)
                    {
                        this.AddDocument(docGrp);
                    }
                }
            }
        }

        #endregion
    }

    public class XmlLuaGroupDocumentBase
    {
        #region Members

        public DocumentRef[] Documents;
        public DocumentRef[] DocumentGroups;
        public XmlDocumentFolder[] DocumentFolders;

        #endregion

        #region Constructors

        public XmlLuaGroupDocumentBase()
        {
        }

        #endregion
    }
}
