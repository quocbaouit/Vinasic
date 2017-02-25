using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace Dynamic.Framework.Infrastructure.Data
{
    public abstract class RepositoryBase<TDb, T> where TDb : DbContext where T : class
    {
        private readonly TDb _dataContext;
        private readonly IDbSet<T> _dbset;

        protected RepositoryBase(TDb dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException("dataContext");
            _dataContext = dataContext;
            _dbset = _dataContext.Set<T>();
        }
        public virtual IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate, string propertyName,
            bool isAscending, string includeProperties = "")
        {
            var query = _dbset.Where(predicate);
            if (!string.IsNullOrEmpty(propertyName))
            {
                var typeOfT = typeof(T);
                var parameter = Expression.Parameter(typeOfT, "parameter");
                var propertyType = typeOfT.GetProperty(propertyName).PropertyType;
                var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
                var orderExpression = Expression.Lambda(propertyAccess, parameter);
                if (isAscending)
                {
                    var expression = Expression.Call(typeof(Queryable), "OrderBy", new[] { typeOfT, propertyType }, query.AsQueryable().Expression, Expression.Quote(orderExpression));
                    return query.Provider.CreateQuery<T>(expression);
                }
                else
                {
                    var expression = Expression.Call(typeof(Queryable), "OrderByDescending", new[] { typeOfT, propertyType }, query.AsQueryable().Expression, Expression.Quote(orderExpression));
                    return query.Provider.CreateQuery<T>(expression);
                }
            }
            return includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.AsExpandable().Include(includeProperty));
        }
        public IEnumerable<T> CallStoreProcedure(string query, params object[] parameters)
        {
            var result = _dataContext.Database.SqlQuery<T>(query, parameters).AsQueryable().AsNoTracking();
            return result;
        }
        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            foreach (T entity in _dbset.Where(@where).AsEnumerable())
                _dbset.Remove(entity);
        }

        public T GetById(long Id)
        {
            T entity = _dbset.Find((object)Id);
            _dataContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public T GetById(string Id)
        {
            T entity = _dbset.Find((object)Id);
            _dataContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public T GetById(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(@where).FirstOrDefault();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(@where).FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return _dbset;
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(@where);
        }

        public ObjectResult<TSource> ExecuteFunction<TSource>(string functionName, params ObjectParameter[] parameters)
        {
            try
            {
                return null;
                //ObjectContext objectContext = this._dataContext.ObjectContext;
                //return objectContext.ExecuteFunction<TSource>(objectContext.DefaultContainerName + "." + functionName, parameters);
            }
            catch
            {
                return null;
            }
        }

        public bool ExecuteFunctionNonReturn(string functionName, params ObjectParameter[] parameters)
        {
            try
            {
                //ObjectContext objectContext = this._dataContext.ObjectContext;
                //objectContext.ExecuteFunction(objectContext.DefaultContainerName + "." + functionName, parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
