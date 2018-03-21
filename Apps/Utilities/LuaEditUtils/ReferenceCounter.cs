using System;
using System.Collections.Generic;
using System.Text;

namespace LuaEdit.Utils
{
    public class ReferenceCounter
    {
        #region Members

        private List<object> _references = new List<object>();
        public event EventHandler ReferenceCountChanged = null;

        #endregion

        #region Constructors

        public ReferenceCounter()
        {
        }

        #endregion

        #region Properties

        internal int Count
        {
            get { return _references.Count; }
        }

        #endregion

        #region Methods

        internal void AddReference(object obj)
        {
            int index = _references.IndexOf(obj);
            if (index < 0)
            {
                _references.Add(obj);

                if (ReferenceCountChanged != null)
                {
                    ReferenceCountChanged(this, EventArgs.Empty);
                }
            }
        }

        internal void RemoveReference(object obj)
        {
            int index = _references.IndexOf(obj);
            if (index >= 0)
            {
                _references.RemoveAt(index);

                if (ReferenceCountChanged != null)
                {
                    ReferenceCountChanged(this, EventArgs.Empty);
                }
            }
        }

        public override int GetHashCode()
        {
 	         return this.Count.GetHashCode();
        }

        public static bool operator ==(ReferenceCounter obj1, int obj2)
        {
            return obj1.Count == obj2;
        }

        public static bool operator !=(ReferenceCounter obj1, int obj2)
        {
            return obj1.Count != obj2;
        }

        public static bool operator >=(ReferenceCounter obj1, int obj2)
        {
            return obj1.Count >= obj2;
        }

        public static bool operator <=(ReferenceCounter obj1, int obj2)
        {
            return obj1.Count <= obj2;
        }

        public static bool operator >(ReferenceCounter obj1, int obj2)
        {
            return obj1.Count > obj2;
        }

        public static bool operator <(ReferenceCounter obj1, int obj2)
        {
            return obj1.Count < obj2;
        }

        public static ReferenceCounter operator +(ReferenceCounter obj1, object obj2)
        {
            obj1.AddReference(obj2);
            return obj1;
        }

        public static ReferenceCounter operator -(ReferenceCounter obj1, object obj2)
        {
            obj1.RemoveReference(obj2);
            return obj1;
        }

        #endregion
    }
}
