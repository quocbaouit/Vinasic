using System;

namespace Dynamic.Framework.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool isDisposed;

        ~Disposable()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
                this.DisposeCore();
            this.isDisposed = true;
        }

        protected virtual void DisposeCore()
        {
        }
    }
}
