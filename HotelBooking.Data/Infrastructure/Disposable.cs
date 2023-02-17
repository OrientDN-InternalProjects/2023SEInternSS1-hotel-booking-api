using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Data.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DisposeCore();
                }
            }
            this.disposed = true;
        }
        ~Disposable()
        {
            Dispose(false);
        }
        // Ovveride this to dispose custom objects
        protected virtual void DisposeCore()
        {
        }
    }
}
