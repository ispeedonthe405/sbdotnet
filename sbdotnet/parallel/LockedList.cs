namespace sbdotnet.parallel
{
    public class LockedList<T> : List<T>, ILockedContainer
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private readonly Lock _Locker = new();

        #endregion Fields
        /////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////
        #region ILockedContainer

        void ILockedContainer.Lock()
        {
            _Locker.Enter();
        }

        void ILockedContainer.Unlock()
        {
            _Locker.Exit();
        }

        #endregion ILockedContainer
        /////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////
        #region Interface

        public LockedList() : base()
        {
        }

        public new void Add(T t)
        {
            lock (_Locker)
            {
                base.Add(t);
            }
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            lock (_Locker)
            {
                base.AddRange(collection);
            }
        }

        public bool TryAdd(T t)
        {
            bool result = false;
            lock (_Locker)
            {
                if (!Contains(t))
                {
                    Add(t);
                    result = true;
                }
            }
            return result;
        }

        public new void Remove(T t)
        {
            lock (_Locker)
            {
                base.Remove(t);
            }
        }

        public new void Clear()
        {
            lock (_Locker)
            {
                base.Clear();
            }
        }

        public new bool Contains(T t)
        {
            lock (_Locker)
            {
                return base.Contains(t);
            }
        }

        public new bool Exists(Predicate<T> match)
        {
            lock (_Locker)
            {
                return base.Exists(match);
            }
        }
    }

    #endregion Interface
    /////////////////////////////////////////////////////////
}
