using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foobricator.Tools;

namespace Foobricator.DataSets
{
    public class CrossJoin : IList<object>
    {
        private List<object> _list;

        public CrossJoin(DataReference refA, DataReference refB )
        {
            var a = refA.Dereference() as IList<object>;
            var b = refB.Dereference() as IList<object>;

            _list = a.Join(b, p => 1, p => 1, (l, r) => new List<object> {l, r}).Cast<object>().ToList();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(object item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(object item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public int IndexOf(object item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public object this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
    }
}
