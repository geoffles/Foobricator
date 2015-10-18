using System;
using System.Collections;
using System.Collections.Generic;
using Foobricator.Tools;

namespace Foobricator.Sources
{
    /// <summary>
    /// A list of numbers
    /// </summary>
    public class NumberList: ISource, IDebugInfoProvider, IList<object>
    {
        /// <summary>
        /// The numbers
        /// </summary>
        public readonly IList<object> Items;

        /// <summary>
        /// Initalise
        /// </summary>
        /// <param name="items"></param>
        public NumberList(IList<object> items)
        {
            Items = items;
        }

        /// <summary>
        /// Returns the list of numbers. From <see cref="Foobricator.Sources.ISource"/>
        /// </summary>
        public object GetItem()
        {
            return Items;
        }

        /// <summary>
        /// Debug information from parsing. From <see cref="Foobricator.Tools.IDebugInfoProvider"/>
        /// </summary>
        public DebugInfo DebugInfo { get; set; }

        #region IList Memebers

        /// <summary>
        /// See List
        /// </summary>
        public IEnumerator<object> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// See List
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public void Add(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// See List
        /// </summary>
        public bool Contains(object item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// See List
        /// </summary>
        public void CopyTo(object[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public bool Remove(object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// See List
        /// </summary>
        public int Count { get { return Items.Count; } }

        /// <summary>
        /// Always true
        /// </summary>
        public bool IsReadOnly { get { return true; } }

        /// <summary>
        /// See List
        /// </summary>
        public int IndexOf(object item)
        {
            return Items.IndexOf(item);
        }

        /// <summary>
        /// See List
        /// </summary>
        public void Insert(int index, object item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// See List
        /// </summary>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// See List
        /// </summary>
        public object this[int index]
        {
            get { return Items[index]; }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
