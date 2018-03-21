using System;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentProject : ILuaEditDocumentGroup, ILuaEditDocumentUIUndoRedo
    {
        #region Members

        event DocumentFolderActionEventHandler FolderAdded;

        #endregion

        #region Properties

        ILuaEditProjectProperties ProjectProperties
        {
            get;
        }

        #endregion

        #region Methods

        ILuaEditDocumentFolder FindFolder(string fileName);
        void AddDocument(ILuaEditDocumentFolder folderDoc);

        #endregion
    }
}
