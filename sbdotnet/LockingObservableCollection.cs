using System.Collections.ObjectModel;

namespace sbdotnet
{
    internal class LockingObservableCollection<T> : ObservableCollection<T>
    {
        private readonly Lock _collectionLocker = new();

        public LockingObservableCollection() : base()
        {

        }

        public new void Add(T t)
        {
            lock (_collectionLocker)
            {
                base.Add(t);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            lock (_collectionLocker)
            {
                foreach (T t in collection)
                {
                    base.Add(t);
                }
            }
        }

        public new void Remove(T t)
        {
            lock (_collectionLocker)
            {
                base.Remove(t);
            }
        }

        public new void Clear()
        {
            lock (_collectionLocker)
            {
                base.Clear();
            }
        }

        public new bool Contains(T t)
        {
            lock (_collectionLocker)
            {
                return base.Contains(t);
            }
        }        
    }
}
