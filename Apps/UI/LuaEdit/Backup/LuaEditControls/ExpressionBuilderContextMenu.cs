using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.Controls
{
    public partial class ExpressionBuilderContextMenu : ContextMenuStrip
    {
        #region Members

        private ComboBox _comboBox = null;
        private int _lastComboBoxSelStart = 0;
        private int _lastComboBoxSelLength = 0;

        private ToolStripMenuItem _anySingleCharactersToolStripMenuItem;
        private ToolStripMenuItem _zeroOrMoreToolStripMenuItem;
        private ToolStripMenuItem _oneOrMoreToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator1;
        private ToolStripMenuItem _beginningOfLineToolStripMenuItem;
        private ToolStripMenuItem _endOfLineToolStripMenuItem;
        private ToolStripMenuItem _beginningOfWordToolStripMenuItem;
        private ToolStripMenuItem _endOfWordToolStripMenuItem;
        private ToolStripMenuItem _nLineBreakToolStripMenuItem;
        private ToolStripSeparator _toolStripSeparator2;
        private ToolStripMenuItem _anyOneCharacterInTheSetToolStripMenuItem;
        private ToolStripMenuItem _anyOneCharacterNotInTheSetToolStripMenuItem;
        private ToolStripMenuItem _orToolStripMenuItem;
        private ToolStripMenuItem _escapeSpecialCharacterToolStripMenuItem;
        private ToolStripMenuItem _tagExpressionToolStripMenuItem;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of ExpressionBuilderContextMenu
        /// </summary>
        /// <param name="comboBox">The combo-box control to which to context menu strip is associated to</param>
        public ExpressionBuilderContextMenu(ComboBox comboBox)
        {
            InitializeComponent();
            InitializeContextMenu();
            _comboBox = comboBox;
            _comboBox.MouseUp += _comboBox_OnMouseUp;
            _comboBox.KeyUp += _comboBox_OnKeyUp;
        }

        #endregion

        #region Event Handlers

        private void _comboBox_OnMouseUp(object sender, MouseEventArgs e)
        {
            _lastComboBoxSelStart = _comboBox.SelectionStart;
            _lastComboBoxSelLength = _comboBox.SelectionLength;
        }

        private void _comboBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            _lastComboBoxSelStart = _comboBox.SelectionStart;
            _lastComboBoxSelLength = _comboBox.SelectionLength;
        }

        private void _anySingleCharactersToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox(".");
        }

        private void _zeroOrMoreToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("*");
        }

        private void _oneOrMoreToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("+");
        }

        private void _beginningOfLineToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("^");
        }

        private void _endOfLineToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("$");
        }

        private void _beginningOfWordToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("<");
        }

        private void _endOfWordToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox(">");
        }

        private void _nLineBreakToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("\\n");
        }

        private void _anyOneCharacterInTheSetToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("[]", -1);
        }

        private void _anyOneCharacterNotInTheSetToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("[^]", -1);
        }

        private void _orToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("|");
        }
        private void _escapeSpecialCharacterToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("\\");
        }

        private void _tagExpressionToolStripMenuItem_OnClick(object sender, EventArgs e)
        {
            InsertTextInComboBox("{}", -1);
        }

        #endregion

        #region Methods

        private void InitializeContextMenu()
        {
            _anySingleCharactersToolStripMenuItem = new ToolStripMenuItem();
            _zeroOrMoreToolStripMenuItem = new ToolStripMenuItem();
            _oneOrMoreToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator1 = new ToolStripSeparator();
            _beginningOfLineToolStripMenuItem = new ToolStripMenuItem();
            _endOfLineToolStripMenuItem = new ToolStripMenuItem();
            _beginningOfWordToolStripMenuItem = new ToolStripMenuItem();
            _endOfWordToolStripMenuItem = new ToolStripMenuItem();
            _nLineBreakToolStripMenuItem = new ToolStripMenuItem();
            _toolStripSeparator2 = new ToolStripSeparator();
            _anyOneCharacterInTheSetToolStripMenuItem = new ToolStripMenuItem();
            _anyOneCharacterNotInTheSetToolStripMenuItem = new ToolStripMenuItem();
            _orToolStripMenuItem = new ToolStripMenuItem();
            _escapeSpecialCharacterToolStripMenuItem = new ToolStripMenuItem();
            _tagExpressionToolStripMenuItem = new ToolStripMenuItem();

            // 
            // _anySingleCharactersToolStripMenuItem
            // 
            _anySingleCharactersToolStripMenuItem.Name = "anySingleCharactersToolStripMenuItem";
            _anySingleCharactersToolStripMenuItem.Text = ".  Any single characters";
            _anySingleCharactersToolStripMenuItem.Click += _anySingleCharactersToolStripMenuItem_OnClick;
            // 
            // _zeroOrMoreToolStripMenuItem
            // 
            _zeroOrMoreToolStripMenuItem.Name = "zeroOrMoreToolStripMenuItem";
            _zeroOrMoreToolStripMenuItem.Text = "*  Zero or more";
            _zeroOrMoreToolStripMenuItem.Click += _zeroOrMoreToolStripMenuItem_OnClick;
            // 
            // _oneOrMoreToolStripMenuItem
            // 
            _oneOrMoreToolStripMenuItem.Name = "oneOrMoreToolStripMenuItem";
            _oneOrMoreToolStripMenuItem.Text = "+  One or more";
            _oneOrMoreToolStripMenuItem.Click += _oneOrMoreToolStripMenuItem_OnClick;
            // 
            // _toolStripSeparator1
            // 
            _toolStripSeparator1.Name = "toolStripMenuItem1";
            // 
            // _beginningOfLineToolStripMenuItem
            // 
            _beginningOfLineToolStripMenuItem.Name = "beginningOfLineToolStripMenuItem";
            _beginningOfLineToolStripMenuItem.Text = "^  Beginning of line";
            _beginningOfLineToolStripMenuItem.Click += _beginningOfLineToolStripMenuItem_OnClick;
            // 
            // _endOfLineToolStripMenuItem
            // 
            _endOfLineToolStripMenuItem.Name = "endOfLineToolStripMenuItem";
            _endOfLineToolStripMenuItem.Text = "$  End of line";
            _endOfLineToolStripMenuItem.Click += _endOfLineToolStripMenuItem_OnClick;
            // 
            // _beginningOfWordToolStripMenuItem
            // 
            _beginningOfWordToolStripMenuItem.Name = "beginningOfWordToolStripMenuItem";
            _beginningOfWordToolStripMenuItem.Text = "<  Beginning of word";
            _beginningOfWordToolStripMenuItem.Click += _beginningOfWordToolStripMenuItem_OnClick;
            // 
            // _endOfWordToolStripMenuItem
            // 
            _endOfWordToolStripMenuItem.Name = "endOfWordToolStripMenuItem";
            _endOfWordToolStripMenuItem.Text = "> End of word";
            _endOfWordToolStripMenuItem.Click += _endOfWordToolStripMenuItem_OnClick;
            // 
            // _nLineBreakToolStripMenuItem
            // 
            _nLineBreakToolStripMenuItem.Name = "nLineBreakToolStripMenuItem";
            _nLineBreakToolStripMenuItem.Text = "\\n  Line break";
            _nLineBreakToolStripMenuItem.Click += _nLineBreakToolStripMenuItem_OnClick;
            // 
            // _toolStripSeparator2
            // 
            _toolStripSeparator2.Name = "toolStripMenuItem2";
            // 
            // _anyOneCharacterInTheSetToolStripMenuItem
            // 
            _anyOneCharacterInTheSetToolStripMenuItem.Name = "anyOneCharacterInTheSetToolStripMenuItem";
            _anyOneCharacterInTheSetToolStripMenuItem.Text = "[ ]  Any one character in the set";
            _anyOneCharacterInTheSetToolStripMenuItem.Click += _anyOneCharacterInTheSetToolStripMenuItem_OnClick;
            // 
            // _anyOneCharacterNotInTheSetToolStripMenuItem
            // 
            _anyOneCharacterNotInTheSetToolStripMenuItem.Name = "anyOneCharacterNotInTheSetToolStripMenuItem";
            _anyOneCharacterNotInTheSetToolStripMenuItem.Text = "[^ ]  Any one character not in the set";
            _anyOneCharacterNotInTheSetToolStripMenuItem.Click += _anyOneCharacterNotInTheSetToolStripMenuItem_OnClick;
            // 
            // _orToolStripMenuItem
            // 
            _orToolStripMenuItem.Name = "orToolStripMenuItem";
            _orToolStripMenuItem.Text = " |  Or";
            _orToolStripMenuItem.Click += _orToolStripMenuItem_OnClick;
            // 
            // _escapeSpecialCharacterToolStripMenuItem
            // 
            _escapeSpecialCharacterToolStripMenuItem.Name = "escapeSpecialCharacterToolStripMenuItem";
            _escapeSpecialCharacterToolStripMenuItem.Text = "\\  Escape Special Character";
            _escapeSpecialCharacterToolStripMenuItem.Click += _escapeSpecialCharacterToolStripMenuItem_OnClick;
            // 
            // _tagExpressionToolStripMenuItem
            // 
            _tagExpressionToolStripMenuItem.Name = "tagExpressionToolStripMenuItem";
            _tagExpressionToolStripMenuItem.Text = "{ }  Tag expression";
            _tagExpressionToolStripMenuItem.Click += _tagExpressionToolStripMenuItem_OnClick;

            // Add menu items to list of items for this context menu
            this.Items.AddRange(new ToolStripItem[] {
                    _anySingleCharactersToolStripMenuItem,
                    _zeroOrMoreToolStripMenuItem,
                    _oneOrMoreToolStripMenuItem,
                    _toolStripSeparator1,
                    _beginningOfLineToolStripMenuItem,
                    _endOfLineToolStripMenuItem,
                    _beginningOfWordToolStripMenuItem,
                    _endOfWordToolStripMenuItem,
                    _nLineBreakToolStripMenuItem,
                    _toolStripSeparator2,
                    _anyOneCharacterInTheSetToolStripMenuItem,
                    _anyOneCharacterNotInTheSetToolStripMenuItem,
                    _orToolStripMenuItem,
                    _escapeSpecialCharacterToolStripMenuItem,
                    _tagExpressionToolStripMenuItem});
        }

        private void InsertTextInComboBox(string text)
        {
            if (text != string.Empty)
            {
                InsertTextInComboBox(text, 0);
            }
        }

        private void InsertTextInComboBox(string text, int cursorOffset)
        {
            if (_comboBox != null)
            {
                // Restore combo box last selection
                _comboBox.Focus();
                _comboBox.SelectionStart = _lastComboBoxSelStart;
                _comboBox.SelectionLength = _lastComboBoxSelLength;

                // Replace selection by text to insert
                _comboBox.SelectedText = text;

                // Reposition caret
                _comboBox.SelectionStart = _comboBox.SelectionStart + cursorOffset;

                // Set last selection to new selection
                _lastComboBoxSelStart = _comboBox.SelectionStart;
                _lastComboBoxSelLength = _comboBox.SelectionLength;
            }
        }

        #endregion
    }
}
