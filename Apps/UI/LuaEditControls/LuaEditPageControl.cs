using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.Controls
{
    public partial class LuaEditPageControl : UserControl
    {
        #region Members

        private List<LuaEditPageControlPage> _pages;
        private LuaEditPageControlPage _currentPage = null;
        private Color _bgStartColor = Color.White;
        private Color _bgEndColor = SystemColors.Control;

        #endregion

        #region Constructors

        public LuaEditPageControl()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            InitializeComponent();

            _pages = new List<LuaEditPageControlPage>();

            (pictureBox4.Image as Bitmap).MakeTransparent(Color.Fuchsia);
            (pictureBox5.Image as Bitmap).MakeTransparent(Color.Fuchsia);
            (pictureBox6.Image as Bitmap).MakeTransparent(Color.Fuchsia);
            (picPageControlAngle.Image as Bitmap).MakeTransparent(Color.Fuchsia);
            (picPageControlLeftCorner.BackgroundImage as Bitmap).MakeTransparent(Color.Fuchsia);
            (pnlLeftSide.BackgroundImage as Bitmap).MakeTransparent(Color.Fuchsia);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The list of all pages in the control
        /// </summary>
        [Browsable(false)]
        public List<LuaEditPageControlPage> Pages
        {
            get { return _pages; }
        }

        /// <summary>
        /// The currently selected page
        /// </summary>
        public LuaEditPageControlPage CurrentPage
        {
            get { return _currentPage; }
        }

        [Category("Appearance"), Description("The page control's background start color.")]
        public Color BackGroundStartColor
        {
            get { return _bgStartColor; }
            set
            {
                _bgStartColor = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("The page control's background end color.")]
        public Color BackGroundEndColor
        {
            get { return _bgEndColor; }
            set
            {
                _bgEndColor = value;
                this.Invalidate();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when one of the page's button is being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPageButtonClicked(object sender, EventArgs e)
        {
            LuaEditPageButton button = sender as LuaEditPageButton;

            if (button != null && !button.Checked)
            {
                foreach (Control ctrl in pageButtonsPanel.Controls)
                {
                    LuaEditPageButton pageButton = ctrl as LuaEditPageButton;

                    if (pageButton != null && pageButton.Checked)
                    {
                        pageButton.Checked = false;
                        break;
                    }
                }

                pageContentPanel.Controls.Clear();
                pageContentPanel.Controls.Add(button.Page.PageContent.Content);
                pageContentPanel.Controls[0].Left = 1;
                pageContentPanel.Controls[0].Top = 1;
                pageContentPanel.InvalidationLines.Clear();
                pageContentPanel.AddInvalidationLine(button.PointToScreen(new Point(button.Right-2, 1)),
                                                     button.PointToScreen(new Point(button.Right-2, button.Height - 2)));
                _currentPage = button.Page;
                button.Checked = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compute the page's buttons panel
        /// </summary>
        private void ComputePageButtonsPanel()
        {
            int overallButtonHeight = 0;

            foreach (Control ctrl in pageButtonsPanel.Controls)
            {
                if (ctrl is LuaEditPageButton)
                {
                    ctrl.Top = (ctrl as LuaEditPageButton).Page.Index * ctrl.Height;
                    overallButtonHeight += ctrl.Height;
                }
            }

            pageButtonsPanel.Height = overallButtonHeight + pageButtonsPanel.Padding.Bottom;
        }

        /// <summary>
        /// Find a page using its title
        /// </summary>
        /// <param name="pageTitle">The page's title to search for</param>
        /// <returns>Return the page's instance if found. Otherwise return null.</returns>
        public LuaEditPageControlPage FindPageByTitle(string pageTitle)
        {
            foreach (LuaEditPageControlPage page in _pages)
            {
                if (page.Title == pageTitle)
                    return page;
            }

            return null;
        }

        /// <summary>
        /// Add a page to the page control
        /// </summary>
        /// <param name="pageContent">The page's content</param>
        /// <returns>The create LuaEditPage</returns>
        public LuaEditPageControlPage AddPage(ILuaEditPageControlPageContent pageContent)
        {
            LuaEditPageControlPage page = null;

            if (FindPageByTitle(pageContent.Title) == null)
            {
                page = new LuaEditPageControlPage(pageContent);
                _pages.Add(page);
                page.Parent = this;

                LuaEditPageButton button = new LuaEditPageButton(page);
                button.Text = pageContent.Title;
                button.Click += OnPageButtonClicked;
                pageButtonsPanel.Controls.Add(button);
                ComputePageButtonsPanel();

                if (_currentPage == null)
                    OnPageButtonClicked(button, new EventArgs());
            }

            return page;
        }

        #endregion
    }

    #region Delegates and EventArgs

    public delegate void LuaEditPageIndexChangedEventHandler(LuaEditPageControlPage sender, LuaEditPageIndexChangedEventArgs e);

    public class LuaEditPageIndexChangedEventArgs
    {
        #region Members

        private LuaEditPageControlPage _page = null;
        private int _oldIndex = -1;
        private int _newIndex = -1;

        #endregion

        #region Constructors

        public LuaEditPageIndexChangedEventArgs(LuaEditPageControlPage page, int oldIndex, int newIndex)
        {
            _page = page;
            _oldIndex = oldIndex;
            _newIndex = newIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The page for which the changes applies to
        /// </summary>
        public LuaEditPageControlPage Page
        {
            get { return _page; }
        }

        /// <summary>
        /// The page's old index
        /// </summary>
        public int OldIndex
        {
            get { return _oldIndex; }
        }

        /// <summary>
        /// The page's new index
        /// </summary>
        public int NewIndex
        {
            get { return _newIndex; }
        }

        #endregion
    }

    #endregion
}
