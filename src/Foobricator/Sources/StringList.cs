using System;
using System.Collections;
using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    public class StringList: ISource, IDebugInfoProvider, IList<object>
    {
        public readonly IList<object> Items;

        public StringList(IList<object> items)
        {
            Items = items;
        }

        public object GetItem()
        {
            return Items;
        }

        public DebugInfo DebugInfo { get; set; }

        #region IList Memebers

        public IEnumerator<object> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(object item)
        {
            throw new NotImplementedException();
        }

        public int Count { get { return Items.Count; } }
        public bool IsReadOnly { get { return true; } }
        public int IndexOf(object item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get { return Items[index]; }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
