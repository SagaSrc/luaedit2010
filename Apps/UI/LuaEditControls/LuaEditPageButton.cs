using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.Controls
{
    public partial class LuaEditPageButton : UserControl
    {
        #region Members

        private bool _checked = false;
        private Bitmap _buttonImage = null;
        private LuaEditPageControlPage _page = null;

        new public event EventHandler Click = null;

        #endregion

        #region Constructors

        public LuaEditPageButton(LuaEditPageControlPage page)
        {
            InitializeComponent();

            (pictureBox1.Image as Bitmap).MakeTransparent(Color.Fuchsia);
            _buttonImage = pictureBox1.Image as Bitmap;
            pictureBox1.Image = null;
            _page = page;

            base.Click += OnControlClicked;
            pictureBox1.Click += OnControlClicked;
            lblText.Click += OnControlClicked;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Determine whether or not the button is checked
        /// </summary>
        [Browsable(false)]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                if (_checked)
                    pictureBox1.Image = _buttonImage;
                else
                    pictureBox1.Image = null;
            }
        }

        [Category("Appearance"), Description("The control's text to display.")]
        new public string Text
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        /// <summary>
        /// The page associated to this button
        /// </summary>
        [Browsable(false)]
        public LuaEditPageControlPage Page
        {
            get { return _page; }
        }

        #endregion

        #region Events

        private void LuaEditPageButton_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = _buttonImage;
        }

        private void LuaEditPageButton_MouseLeave(object sender, EventArgs e)
        {
            if (!_checked)
                pictureBox1.Image = null;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = _buttonImage;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (!_checked)
                pictureBox1.Image = null;
        }

        private void lblCaption_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = _buttonImage;
        }

        private void OnControlClicked(object sender, EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        #endregion
    }
}
