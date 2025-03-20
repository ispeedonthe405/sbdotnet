namespace sbdotnet
{
    public class LockedList<T> : List<T>
    {
        private readonly Lock _collectionLocker = new();

        public LockedList() : base()
        {
        }

        public new void Add(T t)
        {
            lock (_collectionLocker)
            {
                base.Add(t);
            }
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            lock (_collectionLocker)
            {
                base.AddRange(collection);
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

        public new bool Exists(Predicate<T> match)
        {
            lock (_collectionLocker)
            {
                return base.Exists(match);
            }
        }
    }
}
