using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentFolder : ILuaEditDocumentGroup
    {
        #region Properties

        DirectoryInfo DirectoryInfo
        {
            get;
        }

        #endregion
    }
}
