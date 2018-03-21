using System;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentGroup : ILuaEditDocument
    {
        #region Members

        event DocGroupDocActionEventHandler DocumentAdded;
        event DocGroupDocActionEventHandler DocumentRemoved;
        event DocumentFolderActionEventHandler FolderAdded;
        event DocumentFolderActionEventHandler FolderRemoved;

        #endregion

        #region Properties

        List<ILuaEditDocument> Documents
        {
            get;
        }

        List<ILuaEditDocumentGroup> DocumentGroups
        {
            get;
        }

        List<ILuaEditDocumentFolder> DocumentFolders
        {
            get;
        }

        #endregion

        #region Methods

        bool CanAdd();
        void AddDocument(ILuaEditDocument doc);
        void AddDocument(ILuaEditDocumentGroup docGrp);
        void AddDocument(ILuaEditDocumentFolder folderDoc);
        void RemoveDocument(ILuaEditDocument doc);
        void RemoveDocument(ILuaEditDocumentGroup docGrp);
        void RemoveDocument(ILuaEditDocumentFolder docFolder);
        void GetAllDocuments(List<ILuaEditDocument> docs);
        void GetAllDocumentsOfType<T>(List<T> docs) where T:ILuaEditDocument;
        ILuaEditDocument FindDocument(string fileName);
        ILuaEditDocumentGroup FindDocumentGroup(string fileName);
        ILuaEditDocumentFolder FindFolder(string fileName);
        ILuaEditDocumentFolder CreateNewFolder();

        #endregion
    }

    public delegate void DocGroupDocActionEventHandler(object sender, DocGroupDocActionEventArgs e);
    public class DocGroupDocActionEventArgs
    {
        #region Members

        private ILuaEditDocument _doc = null;

        #endregion

        #region Constructors

        public DocGroupDocActionEventArgs(ILuaEditDocument doc)
        {
            _doc = doc;
        }

        #endregion

        #region Properties

        public ILuaEditDocument Document
        {
            get { return _doc; }
        }

        #endregion
    }

    public delegate void DocumentFolderActionEventHandler(object sender, DocumentFolderActionEventArgs e);
    public class DocumentFolderActionEventArgs
    {
        #region Members

        private ILuaEditDocumentFolder _folderDoc = null;

        #endregion

        #region Constructors

        public DocumentFolderActionEventArgs(ILuaEditDocumentFolder folderDoc)
        {
            _folderDoc = folderDoc;
        }

        #endregion

        #region Properties

        public ILuaEditDocumentFolder ProjectFolder
        {
            get { return _folderDoc; }
        }

        #endregion
    }
}