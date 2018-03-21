using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Fireball.Syntax;
using LuaEdit.Documents;
using LuaEdit.Documents.DocumentUtils;
using LuaEdit.Interfaces;

namespace LuaEdit.Managers
{
    public delegate void BreakpointAddedEventHandler(object sender, Breakpoint bp);
    public delegate void BreakpointRemovedEventHandler(object sender, Breakpoint bp);
    public delegate void BreakpointChangedEventHandler(object sender, Breakpoint bp);
    public delegate void BreakpointsDirtyEventHandler(object sender, string fileName);

    public class BreakpointManager
    {
        #region Members

        public event BreakpointAddedEventHandler BreakpointAdded = null;
        public event BreakpointRemovedEventHandler BreakpointRemoved = null;
        public event BreakpointChangedEventHandler BreakpointChanged = null;
        public event BreakpointsDirtyEventHandler BreakpointsDirty = null;

        /// <summary>
        /// The only instance of BreakpointManager
        /// </summary>
        private static readonly BreakpointManager _breakpointManager;

        private Dictionary<string, Dictionary<Row, Breakpoint>> _breakpoints;

        #endregion

        #region Constructors

        static BreakpointManager()
        {
            _breakpointManager = new BreakpointManager();
        }

        private BreakpointManager()
        {
            _breakpoints = new Dictionary<string, Dictionary<Row, Breakpoint>>();
        }

        #endregion

        #region Events

        private void OnBreakpointChanged(object sender, EventArgs e)
        {
            if (BreakpointChanged != null && sender is Breakpoint)
                BreakpointChanged(this, sender as Breakpoint);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the only instance of BreakpointManager
        /// </summary>
        public static BreakpointManager Instance
        {
            get { return _breakpointManager; }
        }

        /// <summary>
        /// Get the list of breakpoints per line, per file
        /// </summary>
        public Dictionary<string, Dictionary<Row, Breakpoint>> Breakpoints
        {
            get { return _breakpoints; }
        }

        #endregion

        #region Methods

        public void DirtyBreakpoints(string fileName)
        {
            if (BreakpointsDirty != null)
            {
                BreakpointsDirty(this, fileName);
            }
        }

        public void ClearBreakpointsForFileName(string fileName)
        {
            if (_breakpoints.ContainsKey(fileName))
            {
                foreach (Breakpoint bp in _breakpoints[fileName].Values)
                {
                    bp.BreakpointChanged -= OnBreakpointChanged;

                    if (BreakpointRemoved != null)
                        BreakpointRemoved(this, bp);
                }

                _breakpoints[fileName].Clear();
                _breakpoints.Remove(fileName);
            }
        }

        public void AddBreakpoint(string fileName, Breakpoint bp)
        {
            if (!_breakpoints.ContainsKey(fileName))
                _breakpoints.Add(fileName, new Dictionary<Row, Breakpoint>());

            if (!_breakpoints[fileName].ContainsKey(bp.Row))
                _breakpoints[fileName].Add(bp.Row, bp);
            else
                _breakpoints[fileName][bp.Row] = bp;

            bp.BreakpointChanged += OnBreakpointChanged;

            if (BreakpointAdded != null)
                BreakpointAdded(this, bp);
        }

        public void RemoveBreakpoint(string fileName, Row row)
        {
            if (_breakpoints.ContainsKey(fileName))
            {
                if (_breakpoints[fileName].ContainsKey(row))
                {
                    Breakpoint bp = _breakpoints[fileName][row];
                    bp.BreakpointChanged -= OnBreakpointChanged;
                    _breakpoints[fileName].Remove(row);

                    if (BreakpointRemoved != null)
                        BreakpointRemoved(this, bp);
                }
            }
        }

        public void DeleteAllBreakpoints()
        {
            if (FrameworkManager.ShowMessageBox("Do you want to delete all breakpoints?",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (string fileName in _breakpoints.Keys)
                {
                    foreach (Breakpoint bp in _breakpoints[fileName].Values)
                    {
                        bp.BreakpointChanged -= OnBreakpointChanged;

                        if (BreakpointRemoved != null)
                            BreakpointRemoved(this, bp);
                    }

                    _breakpoints[fileName].Clear();
                }

                _breakpoints.Clear();
            }
        }

        public void DisableAllBreakpoints()
        {
            foreach (string fileName in _breakpoints.Keys)
            {
                foreach (Breakpoint bp in _breakpoints[fileName].Values)
                {
                    bp.Enabled = false;
                }
            }
        }

        public Breakpoint GetBreakpointAtLine(string fileName, int line)
        {
            if (_breakpoints.ContainsKey(fileName))
            {
                ILuaEditDocument doc = DocumentsManager.Instance.OpenDocument(fileName, false);
                ILuaEditDocumentEditable editableDoc = doc as ILuaEditDocumentEditable;
                Row row = editableDoc.Document.LineToRow(line);

                if (editableDoc != null && row != null && _breakpoints[fileName].ContainsKey(row))
                {
                    return _breakpoints[fileName][row];
                }
            }

            return null;
        }

        #endregion
    }
}
