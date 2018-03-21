using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using DotNetLib.Controls;

namespace LuaEdit.Interfaces
{
    public interface IDocumentNode
    {
        #region Properties

        string NodeText
        {
            get;
        }

        #endregion

        #region Methods

        string GetNodeText();
        void ToTreeListViewItem(TreeListViewItem parentItem);

        #endregion
    }
}
