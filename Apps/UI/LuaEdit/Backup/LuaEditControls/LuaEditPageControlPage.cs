using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.Controls
{
    public class LuaEditPageControlPage
    {
        #region Members

        private int _index = -1;
        private ILuaEditPageControlPageContent _pageContent;
        private LuaEditPageControl _parent = null;

        public event LuaEditPageIndexChangedEventHandler PageIndexChanged = null;

        #endregion

        #region Constructors

        public LuaEditPageControlPage()
        {
        }

        public LuaEditPageControlPage(ILuaEditPageControlPageContent pageContent)
            : this()
        {
            _pageContent = pageContent;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The page's index in the parent page control
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (_parent != null && _index >= 0)
                {
                    _parent.Pages.RemoveAt(_index);
                    _parent.Pages.Insert(value, this);

                    int oldIndex = _index;
                    _index = value;

                    if (PageIndexChanged != null)
                        PageIndexChanged(this, new LuaEditPageIndexChangedEventArgs(this, oldIndex, value));
                }
                else
                    _index = -1;
            }
        }

        /// <summary>
        /// The parent page control, owner of this page
        /// </summary>
        public LuaEditPageControl Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;

                if (value == null)
                    _index = -1;
                else
                    _index = _parent.Pages.IndexOf(this);
            }
        }

        /// <summary>
        /// Get the page's content
        /// </summary>
        public ILuaEditPageControlPageContent PageContent
        {
            get { return _pageContent; }
        }

        /// <summary>
        /// The page's title to display
        /// </summary>
        public string Title
        {
            get { return _pageContent.Title; }
        }

        #endregion
    }

    public interface ILuaEditPageControlPageContent
    {
        #region Properties

        Control Content
        {
            get;
        }

        string Title
        {
            get;
        }

        #endregion

        #region Methods

        void SetUIFromData(object source);
        void SetDataFromUI(object destination);

        #endregion
    }
}
