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
            get{return _dataContext;}
        }
        public UnitOfWork(T dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException("dataContext");
            _dataContext = dataContext;
        }
        public UnitOfWork(string connectionString)
        {
            Type type = typeof(T);
            _dataContext = type.InvokeMember(type.Name, BindingFlags.CreateInstance, null, null, new object[1]{connectionString}) as T;
        }
        public void Commit()
        {
            _dataContext.SaveChanges();
        }
        public void Dispose()
        {
            _dataContext.SaveChanges();
            _dataContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
