using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Win32;

namespace LuaEdit.Controls
{
    public partial class LuaEditListView : ListView
    {
        #region Members

        private const int NM_DBLCLK = (-3);

        private bool _toggleCheckOnDoubleClick = false;
        private bool _isDoubleClickCheckHack = false;
        private Color _borderColor = Color.Black;

        #endregion

        #region Constructors

        public LuaEditListView()
        {
        }

        #endregion

        #region Properties

        [Category("Appearance")]
        [Description("The color used for the border (used with BorderStyle.FixedSingle)")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [Description("When CheckBoxes is true, determine whether or not double clicking will toggle the check on items.")]
        [Category("Behaviors")]
        [DefaultValue(false)]
        public bool ToggleCheckOnDoubleClick
        {
            get { return _toggleCheckOnDoubleClick; }
            set { _toggleCheckOnDoubleClick = value; }
        }

        #endregion

        #region Methods

        //****************************************************************************************        
        // This function helps us overcome the problem with the managed listview wrapper wanting
        // to turn double-clicks on checklist items into checkbox clicks.  We count on the fact        
        // that the base handler for NM_DBLCLK will send a hit test request back at us right away.        
        // So we set a special flag to return a bogus hit test result in that case.        
        //****************************************************************************************        
        private void OnWmReflectNotify(ref Message msg)
        {
            if (!ToggleCheckOnDoubleClick && CheckBoxes)
            {
                User32.NMHDR nmhdr = (User32.NMHDR)Marshal.PtrToStructure(msg.LParam, typeof(User32.NMHDR));
                _isDoubleClickCheckHack = (nmhdr.code == NM_DBLCLK);
            }
        }

        protected override void WndProc(ref Message msg)
        {
            IntPtr hdc = User32.GetDCEx(msg.HWnd, (IntPtr)1, 1 | 0x0020);

            if (hdc != IntPtr.Zero)
            {
                switch (msg.Msg)
                {
                    case (int)WindowMessages.WM_ERASEBKGND:
                    case (int)WindowMessages.WM_NCPAINT:
                    case (int)WindowMessages.WM_PAINT:
                        {
                            Graphics g = Graphics.FromHdc(hdc);
                            Rectangle borderRect = new Rectangle(0, 0, this.Width, this.Height);
                            PaintBorder(g, borderRect);
                            User32.ReleaseDC(msg.HWnd, hdc);
                            break;
                        }
                    //  This code is to hack around the fact that the managed listview                
                    //  wrapper translates double clicks into checks without giving the                 
                    //  host to participate.                
                    //  See OnWmReflectNotify() for more details.                
                    case (int)WindowMessages.WM_REFLECT + (int)WindowMessages.WM_NOTIFY:
                        {
                            OnWmReflectNotify(ref msg);
                            break;
                        }
                    //  This code checks to see if we have entered our hack check for                
                    //  double clicking items in check lists.  During the NM_DBLCLK                
                    //  processing, the managed handler will send a hit test message                
                    //  to see which item to check.  Returning -1 will convince that                
                    //  code not to proceed.                
                    case (int)WindowMessages.LVM_HITTEST:
                        {
                            if (_isDoubleClickCheckHack)
                            {
                                _isDoubleClickCheckHack = false;
                                msg.Result = new IntPtr(-1);
                                return;
                            }

                            break;
                        }
                }
            }

            base.WndProc(ref msg);
        }

        private void PaintBorder(Graphics g, Rectangle borderRect)
        {
            ControlPaint.DrawBorder(g, borderRect, _borderColor, ButtonBorderStyle.Solid);
        }

        #endregion
    }
}
