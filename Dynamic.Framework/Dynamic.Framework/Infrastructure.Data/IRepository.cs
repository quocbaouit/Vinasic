using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Dynamic.Framework.Infrastructure.Data
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        T GetById(long Id);

        T GetById(string Id);

        T GetById(Expression<Func<T, bool>> where);

        T Get(Expression<Func<T, bool>> where);

        IQueryable<T> GetAll();

        IQueryable<T> GetMany(Expression<Func<T, bool>> where);

        ObjectResult<TSource> ExecuteFunction<TSource>(string functionName, params ObjectParameter[] parameters);

        bool ExecuteFunctionNonReturn(string functionName, params ObjectParameter[] parameters);

        IEnumerable<T> CallStoreProcedure(string query, params object[] parameters);

        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate, string propertyName,
            bool isAscending, string includeProperties = "");
    }
}
