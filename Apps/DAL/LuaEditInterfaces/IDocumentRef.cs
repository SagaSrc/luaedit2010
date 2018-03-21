using System;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.Interfaces
{
    public interface IDocumentRef
    {
        #region Properties

        string FileName
        {
            get;
            set;
        }

        ILuaEditDocumentGroup ParentDocument
        {
            get;
            set;
        }

        #endregion

        #region Methods

        string ToString();

        #endregion
    }
}
