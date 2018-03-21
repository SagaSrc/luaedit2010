using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Fireball.Syntax;
using Fireball.Windows.Forms;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentEditable : ILuaEditDocument
    {
        #region Properties

        ILuaEditDocumentEditableUI DocumentUI
        {
            get;
        }

        string SyntaxFileName
        {
            get;
        }

        Icon DocumentIcon
        {
            get;
        }

        SyntaxDocument Document
        {
            get;
        }

        #endregion
    }
}
