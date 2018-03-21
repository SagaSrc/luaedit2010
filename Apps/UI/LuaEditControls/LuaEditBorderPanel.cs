using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.Controls
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))] 
    public partial class LuaEditBorderPanel : UserControl
    {
        #region Members

        private Color _borderColor = Color.Black;
        private Border3DStyle _borderStyle = Border3DStyle.Flat;
        private bool _showLeftBorder = true;
        private bool _showRightBorder = true;
        private bool _showTopBorder = true;
        private bool _showBottomBorder = true;
        private List<InvalidationLine> _invalidationLines;

        #endregion

        #region Constructors

        public LuaEditBorderPanel()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.ContainerControl |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.UserPaint, true);

            _invalidationLines = new List<InvalidationLine>();
        }

        #endregion

        #region Properties

        [Category("Appearance"), Description("The control's border color.")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("The control's border style.")]
        public new Border3DStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("Determine whether or not to display the control's left border.")]
        public bool ShowLeftBorder
        {
            get { return _showLeftBorder; }
            set
            {
                _showLeftBorder = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("Determine whether or not to display the control's right border.")]
        public bool ShowRightBorder
        {
            get { return _showRightBorder; }
            set
            {
                _showRightBorder = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("Determine whether or not to display the control's top border.")]
        public bool ShowTopBorder
        {
            get { return _showTopBorder; }
            set
            {
                _showTopBorder = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"), Description("Determine whether or not to display the control's bottom border.")]
        public bool ShowBottomBorder
        {
            get { return _showBottomBorder; }
            set
            {
                _showBottomBorder = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Special invalidation lines
        /// </summary>
        /// <remarks>Used especially to invalidate parts of the border.</remarks>
        [Browsable(false)]
        public List<InvalidationLine> InvalidationLines
        {
            get { return _invalidationLines; }
        }

        #endregion

        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_borderStyle == Border3DStyle.Flat)
            {
                if (_showLeftBorder && _showRightBorder && _showTopBorder && _showBottomBorder)
                {
                    Rectangle borderRect = this.ClientRectangle;
                    borderRect.Width -= 1;
                    borderRect.Height -= 1;
                    e.Graphics.DrawRectangle(new Pen(_borderColor), borderRect);
                }
                else
                {
                    if (_showLeftBorder)
                        e.Graphics.DrawLine(new Pen(_borderColor), 0, 0, 0, this.Height - 1);
                    if (_showRightBorder)
                        e.Graphics.DrawLine(new Pen(_borderColor), this.Width - 1, 0, this.Width - 1, this.Height - 1);
                    if (_showTopBorder)
                        e.Graphics.DrawLine(new Pen(_borderColor), 0, 0, this.Width - 1, 0);
                    if (_showBottomBorder)
                        e.Graphics.DrawLine(new Pen(_borderColor), 0, this.Height - 1, this.Width - 1, this.Height - 1);
                }
            }
            else
            {
                ControlPaint.DrawBorder3D(e.Graphics, this.ClientRectangle, _borderStyle, Border3DSide.All);
            }

            Color invalidationLinesColor = this.BackColor;
            foreach (InvalidationLine il in _invalidationLines)
                e.Graphics.DrawLine(new Pen(invalidationLinesColor), this.PointToClient(il.StartPt), this.PointToClient(il.EndPt));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new invalidation line
        /// </summary>
        /// <param name="startPt">The line's start point</param>
        /// <param name="endPt">The line's end point</param>
        public void AddInvalidationLine(Point startPt, Point endPt)
        {
            _invalidationLines.Add(new InvalidationLine(startPt, endPt));
            this.Invalidate();
        }

        #endregion
    }

    public class InvalidationLine
    {
        #region Members

        private Point _pt1 = Point.Empty;
        private Point _pt2 = Point.Empty;

        #endregion

        #region Constructor

        public InvalidationLine(Point pt1, Point pt2)
        {
            _pt1 = pt1;
            _pt2 = pt2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Starting point of the line
        /// </summary>
        public Point StartPt
        {
            get { return _pt1; }
        }

        /// <summary>
        /// Ending point of the line
        /// </summary>
        public Point EndPt
        {
            get { return _pt2; }
        }

        #endregion
    }
}
