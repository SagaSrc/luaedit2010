using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.HelperDialogs
{
    public partial class ItemAlreadyExistDialog : Form
    {
        #region Constructors

        public ItemAlreadyExistDialog()
        {
            InitializeComponent();

            Bitmap bmp = pictureBox1.Image as Bitmap;
            if (bmp != null)
            {
                bmp.MakeTransparent(bmp.GetPixel(0, 0));
                pictureBox1.Image = bmp;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The title to display for this dialog
        /// </summary>
        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }

        /// <summary>
        /// Gets whether the user wants his action to be applied to
        /// all following similar dialogs
        /// </summary>
        public bool ApplyAll
        {
            get { return chkApplyAll.Checked; }
        }

        #endregion
    }
}