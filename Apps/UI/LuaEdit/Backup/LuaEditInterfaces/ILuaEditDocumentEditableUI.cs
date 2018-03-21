using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Fireball.Syntax;
using Fireball.Windows.Forms;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocumentEditableUI : ILuaEditDocumentUI
    {
        #region Members

        event EventHandler CaretChanged;
        event EventHandler SelectionChanged;
        event EventHandler RunToRequested;

        #endregion

        #region Properties

        bool IsOverwrite
        {
            get;
        }

        int CurrentLine
        {
            get;
            set;
        }

        int CurrentColumn
        {
            get;
        }

        int CurrentCharacter
        {
            get;
        }

        Row CurrentRow
        {
            get;
        }

        bool IsOutlining
        {
            get;
            set;
        }

        bool IsAllBookmarksEnabled
        {
            get;
        }

        bool IsAllSegmentsExpanded
        {
            get;
        }

        List<int> MarkedLines
        {
            get;
        }

        CodeEditorControl Editor
        {
            get;
        }

        #endregion

        #region Methods

        void Cut();
        void Copy();
        void Paste();
        void SelectAll();
        void ToggleBreakpoint(Row row);
        void ToggleBookmark(Row row);
        void EnableDisableBookmark(IBookmark bm);
        void EnableDisableAllBookmarks();
        void ClearBookmarks();
        void GotoNextBookmark();
        void GotoPreviousBookmark();
        void ToggleOutliningExpansion();
        void ToggleAllOutlining();
        void HighlightLine(int line, Color color);
        void MarkLine(int line, bool marked);
        void Goto(int line);
        void ShowGoto();

        #endregion
    }
}
