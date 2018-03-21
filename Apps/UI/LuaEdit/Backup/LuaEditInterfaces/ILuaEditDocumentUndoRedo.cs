using System;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentUIUndoRedo
    {
        #region Properties

        bool CanUndo
        {
            get;
        }

        bool CanRedo
        {
            get;
        }

        #endregion

        #region Methods

        void Undo();
        void Redo();

        #endregion
    }
}
