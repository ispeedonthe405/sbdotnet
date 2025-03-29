using System.Collections.ObjectModel;

namespace sbdotnet.parallel
{
    public class LockedObservableCollection<T> : ObservableCollection<T>, ILockedContainer
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

        public LockedObservableCollection() : base()
        { }
        
        public new void Add(T t)
        {
            lock (_Locker)
            {
                base.Add(t);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            lock (_Locker)
            {
                AddRange(collection);
            }
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
    }

    #endregion Interface
    /////////////////////////////////////////////////////////
}
