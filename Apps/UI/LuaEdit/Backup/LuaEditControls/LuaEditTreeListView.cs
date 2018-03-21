using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DotNetLib.Controls;
using LuaEdit.Win32;

namespace LuaEdit.Controls
{
    public class LuaEditTreeListView : TreeListView
    {
        #region Members

        //private Color _borderColor = Color.Black;

        #endregion

        #region Constructors

        public LuaEditTreeListView()
        {
        }

        #endregion

        #region Properties

        /*[Category("Appearance")]
        [Description("The color used for the border (used with BorderStyle.FixedSingle)")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }*/

        #endregion

        #region Methods

        protected override void WndProc(ref Message msg)
        {
            IntPtr hdc = User32.GetDCEx(msg.HWnd, (IntPtr)1, 1 | 0x0020);

            if (hdc != IntPtr.Zero)
            {
                Graphics g = Graphics.FromHdc(hdc);

                switch (msg.Msg)
                {
                    case (int)WindowMessages.WM_ERASEBKGND:
                    case (int)WindowMessages.WM_NCPAINT:
                    case (int)WindowMessages.WM_PAINT:
                        {
                            Rectangle borderRect = new Rectangle(0, 0, this.Width, this.Height);
                            PaintBorder(g, borderRect);
                            break;
                        }
                }

                User32.ReleaseDC(msg.HWnd, hdc);
            }
                
            base.WndProc(ref msg);
        }

        public void PaintBorder(Graphics g, Rectangle borderRect)
        {
            ControlPaint.DrawBorder(g, borderRect, this.BorderColor, ButtonBorderStyle.Solid);
        }

        #endregion
    }
}
