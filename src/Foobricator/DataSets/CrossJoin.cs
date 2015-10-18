using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.DataSets
{
    /// <summary>
    /// Cross join two lists
    /// </summary>
    public class CrossJoin : IList<object>
    {
        private List<object> _list;

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="refA"></param>
        /// <param name="refB"></param>
        public CrossJoin(DataReference refA, DataReference refB )
        {
            var a = refA.Dereference() as IList<object>;
            var b = refB.Dereference() as IList<object>;

            _list = a.Join(b, p => 1, p => 1, (l, r) => new List<object> {l, r}).Cast<object>().ToList();
        }

        /// <summary>
        /// An enumerator for the joined list
        /// </summary>        
        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// An enumerator for the joined list
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// See List
        /// </summary>
        public void Add(object item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// See List
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// See List
        /// </summary>
        public bool Contains(object item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// See List
        /// </summary>
        public void CopyTo(object[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// See List
        /// </summary>
        public bool Remove(object item)
        {
            return _list.Remove(item);
        }

        /// <summary>
        /// See List
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Always true
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// See List
        /// </summary>
        public int IndexOf(object item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// See List
        /// </summary>
        public void Insert(int index, object item)
        {
            _list.Insert(index, item);
        }

        /// <summary>
        /// See List
        /// </summary>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Get item at
        /// </summary>
        public object this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
    }
}
