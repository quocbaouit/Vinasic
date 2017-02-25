using System;
using System.Data.Entity;
using System.Reflection;

namespace Dynamic.Framework.Infrastructure.Data
{
    public class UnitOfWork<T> : IDisposable, IUnitOfWork<T> where T : DbContext
    {
        private T _dataContext;

        public T DataContext
        {
            get
            {
                return this._dataContext;
            }
        }

        public UnitOfWork(T dataContext)
        {
            if ((object)dataContext == null)
                throw new ArgumentNullException("dataContext");
            this._dataContext = dataContext;
        }

        public UnitOfWork(string connectionString)
        {
            Type type = typeof(T);
            this._dataContext = type.InvokeMember(type.Name, BindingFlags.CreateInstance, (Binder)null, (object)null, new object[1]
      {
        (object) connectionString
      }) as T;
        }

        public void Commit()
        {
            this._dataContext.SaveChanges();
        }

        public void Dispose()
        {
            this._dataContext.SaveChanges();
            this._dataContext.Dispose();
            GC.SuppressFinalize((object)this);
        }
    }
}
