using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Controls;
using LuaEdit.Interfaces;

using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    public partial class FindAndReplaceDlg : DockContent
    {
        #region Members

        private static readonly int InnerFindAndReplaceCtrlLeftValue = 0;
        private static readonly int InnerFindAndReplaceCtrlTopValue = 25;
        private static readonly int InnerFindAndReplaceCtrlWidthSpacing = 8;
        private static readonly int InnerFindAndReplaceCtrlHeightSpacing = 49;

        private ILuaEditDocument _currentDoc = null;
        private FindAndReplaceType _findAndReplaceType = FindAndReplaceType.None;
        private IFindAndReplaceControl _findAndReplaceControl = null;
        private QuickFindControl _quickFindCtrl = null;
        private FindInFilesControl _findInFilesCtrl = null;

        #endregion

        #region Constructors

        public FindAndReplaceDlg()
        {
            InitializeComponent();

            // Create members
            _quickFindCtrl = new QuickFindControl();
            _quickFindCtrl.Visible = false;
            _findInFilesCtrl = new FindInFilesControl();
            _findInFilesCtrl.Visible = false;

            // Add find and replace controls
            this.Controls.AddRange(new Control[] {
                _quickFindCtrl,
                _findInFilesCtrl
                });

            // Setup toolstrip renderer
            findAndReplaceToolStrip.Renderer = new ToolStripFlatBorderRenderer();
        }

        #endregion

        #region Event Handlers

        private void FindAndReplaceDlg_Shown(object sender, EventArgs e)
        {
            this.FloatPane.FloatWindow.Activated += FindAndReplaceDlg_Activated;
        }

        private void FindAndReplaceDlg_Activated(object sender, EventArgs e)
        {
            InitializeSetup();
        }

        private void FindAndReplaceDlg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void OnFindAndReplaceInnerCtrlLayoutChanged(object sender, int width, int height)
        {
            this.FloatPane.FloatWindow.Height = height + InnerFindAndReplaceCtrlHeightSpacing;
        }

        private void quickFindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FindAndReplaceType = FindAndReplaceType.QuickFind;
        }

        private void findInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FindAndReplaceType = FindAndReplaceType.FindInFiles;
        }

        private void quickReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FindAndReplaceType = FindAndReplaceType.QuickReplace;
        }

        private void replaceInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FindAndReplaceType = FindAndReplaceType.ReplaceInFiles;
        }

        #endregion

        #region Properties

        public ILuaEditDocument CurrentDocument
        {
            get { return _currentDoc; }
            set
            {
                if (_currentDoc != value)
                {
                    _currentDoc = value;

                    if (this.Visible)
                    {
                        InitializeSetup();
                    }
                }
            }
        }

        public FindAndReplaceType FindAndReplaceType
        {
            get { return _findAndReplaceType; }
            set
            {
                if (_findAndReplaceType != value)
                {
                    _findAndReplaceType = value;
                    InitializeSetup();
                }
            }
        }

        #endregion

        #region Methods

        private void InitializeSetup()
        {
            if (_findAndReplaceControl != null)
            {
                if (_findAndReplaceType != _findAndReplaceControl.FindAndReplaceType)
                {
                    _findAndReplaceControl.ControlVisible = false;
                }
                else
                {
                    _findAndReplaceControl.Initialize(string.Empty, _currentDoc);
                    return;
                }
            }

            switch (_findAndReplaceType)
            {
                case FindAndReplaceType.ReplaceInFiles:
                    //_findAndReplaceControl = _quickFindCtrl;
                    break;
                case FindAndReplaceType.FindInFiles:
                    findToolStripDropDownButton.Text = "Find in Files";
                    findToolStripDropDownButton.Image = findInFilesToolStripMenuItem.Image;
                    _findAndReplaceControl = _findInFilesCtrl;
                    break;
                case FindAndReplaceType.QuickReplace:
                    //_findAndReplaceControl = _quickFindCtrl;
                    break;
                case FindAndReplaceType.QuickFind:
                default:
                    findToolStripDropDownButton.Text = "Quick Find";
                    findToolStripDropDownButton.Image = quickFindToolStripMenuItem.Image;
                    _findAndReplaceControl = _quickFindCtrl;
                    break;
            }

            this.FindAndReplaceType = _findAndReplaceControl.FindAndReplaceType;
            this.AcceptButton = _findAndReplaceControl.DefaultButton;
            this.FloatPane.FloatWindow.Width = _findAndReplaceControl.ControlStartWidth + InnerFindAndReplaceCtrlWidthSpacing;
            this.FloatPane.FloatWindow.Height = _findAndReplaceControl.ControlStartHeight + InnerFindAndReplaceCtrlHeightSpacing;
            _findAndReplaceControl.ControlVisible = true;
            _findAndReplaceControl.Initialize(string.Empty, _currentDoc);
            _findAndReplaceControl.ControlAnchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            _findAndReplaceControl.ControlLeft = InnerFindAndReplaceCtrlLeftValue;
            _findAndReplaceControl.ControlTop = InnerFindAndReplaceCtrlTopValue;
            _findAndReplaceControl.LayoutChanged += OnFindAndReplaceInnerCtrlLayoutChanged;
        }

        public void Show(DockPanel dockPanel, FindAndReplaceType findAndReplaceType)
        {
            base.Show(dockPanel);
            this.FindAndReplaceType = findAndReplaceType;
        }

        #endregion
    }

    public delegate void LayoutChangedEventHandler(object sender, int width, int height);
    public interface IFindAndReplaceControl
    {
        #region Events

        event LayoutChangedEventHandler LayoutChanged;

        #endregion

        #region Properties

        FindAndReplaceType FindAndReplaceType
        {
            get;
        }

        Button DefaultButton
        {
            get;
        }

        int ControlStartWidth
        {
            get;
        }

        int ControlStartHeight
        {
            get;
        }

        bool ControlVisible
        {
            get;
            set;
        }

        int ControlLeft
        {
            get;
            set;
        }

        int ControlTop
        {
            get;
            set;
        }

        int ControlHeight
        {
            get;
        }

        int ControlWidth
        {
            get;
        }

        AnchorStyles ControlAnchor
        {
            get;
            set;
        }

        #endregion

        #region Methods

        void Initialize(string initialText, ILuaEditDocument currentDoc);
        void Search();
        void StopSearch();

        #endregion
    }

    public enum FindAndReplaceType
    {
        None,

        QuickFind,
        QuickReplace,
        FindInFiles,
        ReplaceInFiles
    }
}