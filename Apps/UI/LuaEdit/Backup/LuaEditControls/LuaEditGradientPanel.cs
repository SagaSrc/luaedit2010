using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.Controls
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))] 
    public partial class LuaEditGradientPanel : UserControl
    {
        #region Members

        private Color _bgStartColor = Color.White;
        private Color _bgEndColor = SystemColors.Control;

        #endregion

        #region Constructors

        public LuaEditGradientPanel()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.ContainerControl |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.UserPaint, true);
        }

        #endregion

        #region Properties

        [Category("Appearance"), Description("The control's background start color.")]
        public Color BackGroundStartColor
        {
            get { return _bgStartColor; }
            set
            {
                _bgStartColor = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("The control's background end color.")]
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

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush b = new LinearGradientBrush(this.ClientRectangle, _bgStartColor, _bgEndColor, 90.0f))
                e.Graphics.FillRectangle(b, this.ClientRectangle);
        }

        #endregion
    }
}
