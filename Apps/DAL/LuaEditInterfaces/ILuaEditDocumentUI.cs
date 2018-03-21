using System;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentUI : ILuaEditDocumentUIUndoRedo
    {
        #region Members

        event EventHandler ContentChanged;

        #endregion

        #region Properties

        ILuaEditDocument ParentDocument
        {
            get;
        }

        #endregion

        #region Methods

        bool TerminateUI();

        #endregion
    }
}
