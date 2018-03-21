using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Fireball.Syntax;

namespace LuaEdit.Documents.DocumentUtils
{
    public class Bookmark : IBookmark
    {
        #region Members

        private int _line = 0;
        private bool _enabled = true;
        private Bitmap _bookmarkEnabledImage = null;
        private Bitmap _bookmarkDisabledImage = null;

        public event EventHandler BookmarkChanged = null;

        #endregion

        #region Constructors

        public Bookmark(int line, Bitmap bookmarkEnabledImage, Bitmap bookmarkDisabledImage)
        {
            _line = line;
            _bookmarkEnabledImage = bookmarkEnabledImage;
            _bookmarkEnabledImage.MakeTransparent(_bookmarkEnabledImage.GetPixel(0, 0));
            _bookmarkDisabledImage = bookmarkDisabledImage;
            _bookmarkDisabledImage.MakeTransparent(_bookmarkDisabledImage.GetPixel(0, 0));
        }

        #endregion

        #region Properties

        public int Line
        {
            get { return _line; }
            set
            {
                _line = value;
                if (BookmarkChanged != null)
                    BookmarkChanged(this, new EventArgs());
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (BookmarkChanged != null)
                    BookmarkChanged(this, new EventArgs());
            }
        }

        public Bitmap BookmarkEnabledImage
        {
            get { return _bookmarkEnabledImage; }
            set
            {
                _bookmarkEnabledImage = value;
                _bookmarkEnabledImage.MakeTransparent(value.GetPixel(0, 0));

                if (BookmarkChanged != null)
                    BookmarkChanged(this, new EventArgs());
            }
        }

        public Bitmap BookmarkDisabledImage
        {
            get { return _bookmarkDisabledImage; }
            set
            {
                _bookmarkDisabledImage = value;
                _bookmarkDisabledImage.MakeTransparent(value.GetPixel(0, 0));

                if (BookmarkChanged != null)
                    BookmarkChanged(this, new EventArgs());
            }
        }

        #endregion
    }
}
