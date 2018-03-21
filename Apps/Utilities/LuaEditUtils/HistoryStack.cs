using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LuaEdit.Utils
{
    /// <summary>
    /// This class manages an undo/redo stack for a class instance
    /// </summary>
    public class HistoryStack
    {
        #region Members

        private List<UndoRedoCommand> _historyStack;
        private int _currentCmdIndex = -1;
        private bool _isUndoingOrRedoing = false;

        public event EventHandler ItemPushed = null;
        public event EventHandler CurrentIndexChanged = null;

        #endregion

        #region Constructors

        public HistoryStack()
        {
            _historyStack = new List<UndoRedoCommand>();
            _currentCmdIndex = -1;
            _isUndoingOrRedoing = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get whether or not an undo action could be performed
        /// </summary>
        public bool CanUndo
        {
            get { return CurrentIndex >= 0; }
        }

        /// <summary>
        /// Get whether or not a redo action could be performed
        /// </summary>
        public bool CanRedo
        {
            get { return CurrentIndex < this.HistoryStackTop; }
        }

        /// <summary>
        /// Gets the history stack's current index
        /// </summary>
        public int CurrentIndex
        {
            get { return _currentCmdIndex; }
            set
            {
                if (value != _currentCmdIndex)
                {
                    _currentCmdIndex = value;

                    if (CurrentIndexChanged != null)
                    {
                        CurrentIndexChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Get the top of the history stack
        /// </summary>
        public int HistoryStackTop
        {
            get { return _historyStack.Count - 1; }
        }

        /// <summary>
        /// Gets whether or not the history stack is currently performing an
        /// Undo or a Redo action
        /// </summary>
        public bool IsUndoingOrRedoing
        {
            get { return _isUndoingOrRedoing; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove a undo/redo command from the stack
        /// </summary>
        private void PopHistoryItem()
        {
            if (this.CanUndo)
            {
                _historyStack.RemoveAt(this.HistoryStackTop);
            }
        }

        /// <summary>
        /// Add a undo/redo command to the stack and pop all trailing commands after the current command
        /// </summary>
        /// <param name="item"></param>
        public void PushHistoryItem(UndoRedoCommand item)
        {
            while (this.HistoryStackTop > CurrentIndex)
            {
                PopHistoryItem();
            }

            _historyStack.Add(item);
            CurrentIndex = this.HistoryStackTop;

            if (ItemPushed != null)
            {
                ItemPushed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Perform undo on current command
        /// </summary>
        public void Undo()
        {
            if (this.CanUndo)
            {
                try
                {
                    _isUndoingOrRedoing = true;
                    UndoRedoCommand currentItem = _historyStack[CurrentIndex];
                    currentItem.ObjectType.GetProperty(currentItem.AffectedPropertyName).SetValue(currentItem.Object, currentItem.OldValue, null);
                    --CurrentIndex;
                }
                finally
                {
                    _isUndoingOrRedoing = false;
                }
            }
        }

        /// <summary>
        /// Perform redo on next command
        /// </summary>
        public void Redo()
        {
            if (this.CanRedo)
            {
                try
                {
                    _isUndoingOrRedoing = true;
                    ++CurrentIndex;
                    UndoRedoCommand currentItem = _historyStack[CurrentIndex];
                    currentItem.ObjectType.GetProperty(currentItem.AffectedPropertyName).SetValue(currentItem.Object, currentItem.NewValue, null);
                }
                finally
                {
                    _isUndoingOrRedoing = false;
                }
            }
        }

        #endregion
    }

    public class UndoRedoCommand
    {
        #region Members

        private object _object;
        private Type _objectType;
        private string _affectedPropertyName;
        private object _oldValue;
        private object _newValue;

        #endregion

        #region Constructors

        public UndoRedoCommand(object obj, Type objectType, string affectedPropertyName,
                               object newValue, object oldValue)
        {
            _object = obj;
            _objectType = objectType;
            _affectedPropertyName = affectedPropertyName;
            _newValue = newValue;
            _oldValue = oldValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The object on which the undo/redo command applies to
        /// </summary>
        public object Object
        {
            get { return _object; }
        }

        /// <summary>
        /// The type of object on which the undo/redo command applies to
        /// </summary>
        public Type ObjectType
        {
            get { return _objectType; }
        }

        /// <summary>
        /// The property name on which the undo/redo command applies to
        /// </summary>
        public string AffectedPropertyName
        {
            get { return _affectedPropertyName; }
        }

        /// <summary>
        /// The object undo/redo command's old value (before changes occured)
        /// </summary>
        public object OldValue
        {
            get { return _oldValue; }
        }

        /// <summary>
        /// The object undo/redo command's modified value
        /// </summary>
        public object NewValue
        {
            get { return _newValue; }
        }

        #endregion
    }
}
