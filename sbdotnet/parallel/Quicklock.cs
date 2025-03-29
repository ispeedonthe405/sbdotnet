using System.ComponentModel.DataAnnotations;

namespace sbdotnet.parallel
{
    public class Quicklock : IDisposable
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private ILockedContainer _container;

        #endregion Fields
        /////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////
        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _container.Unlock();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
        /////////////////////////////////////////////////////////


        /////////////////////////////////////////////////////////
        #region Interface

        public Quicklock(ILockedContainer container)
        {
            _container = container;
            _container!.Lock();
        }

        #endregion Interface
        /////////////////////////////////////////////////////////
    }
}
