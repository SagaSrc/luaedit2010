using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.Controls
{
    public partial class LuaEditHeaderSeparator : UserControl
    {
        #region Members

        private Color _separatorColor = Color.Black;
        private const int SEPARATOR_LEFT_SPACING = 6;
        private const int SEPARATOR_RIGHT_SPACING = 1;

        #endregion

        #region Constructors

        public LuaEditHeaderSeparator()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.ContainerControl |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.UserPaint, true);
        }

        #endregion

        #region Properties

        [Category("Appearance"), Description("The separator's color.")]
        public Color SeparatorColor
        {
            get { return _separatorColor; }
            set
            {
                _separatorColor = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("The separator's text to display.")]
        public string Title
        {
            get { return lblTitle.Text; }
            set
            {
                lblTitle.Text = value;
                this.Invalidate();
            }
        }

        #endregion

        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            int sepXPos = lblTitle.Left + lblTitle.Width + SEPARATOR_LEFT_SPACING;
            int sepYPos = (this.Height / 2) + 1;
            e.Graphics.DrawLine(new Pen(_separatorColor), sepXPos, sepYPos, this.Width - SEPARATOR_RIGHT_SPACING, sepYPos);
        }

        #endregion
    }
}
