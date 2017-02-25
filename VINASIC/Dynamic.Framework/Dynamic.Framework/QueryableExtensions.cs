using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Dynamic.Framework
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int size) where TSource : class
        {
            QueryableExtensions.CheckSource((object)source);
            if (page < 0)
                throw new ArgumentOutOfRangeException("page");
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size");
            int index = QueryableExtensions.GetIndex(page, size);
            return Queryable.Take<TSource>(Queryable.Skip<TSource>(source, index), size);
        }

        public static TSource SelectScalar<TSource>(this IQueryable<TSource> source, int value, string propertyName = "id") where TSource : class
        {
            QueryableExtensions.CheckSource((object)source);
            if (value < 0)
                throw new ArgumentOutOfRangeException("value");
            QueryableExtensions.CheckNullOrEmpty(propertyName);
            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType);
            Expression<Func<TSource, bool>> predicate = Expression.Lambda<Func<TSource, bool>>((Expression)Expression.Equal((Expression)Expression.Property((Expression)parameterExpression, propertyName), (Expression)Expression.Constant((object)value)), new ParameterExpression[1]
      {
        parameterExpression
      });
            return Queryable.FirstOrDefault<TSource>(source, predicate);
        }

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName, SortDirection direction = SortDirection.Ascending)
        {
            QueryableExtensions.CheckSource((object)source);
            QueryableExtensions.CheckNullOrEmpty(propertyName);
            string methodName = "OrderBy";
            if (direction == SortDirection.Descending)
                methodName = "OrderByDescending";
            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType);
            MemberExpression memberExpression = Expression.PropertyOrField((Expression)parameterExpression, propertyName);
            LambdaExpression lambdaExpression = Expression.Lambda((Expression)memberExpression, new ParameterExpression[1]
      {
        parameterExpression
      });
            MethodCallExpression methodCallExpression = Expression.Call(typeof(Queryable), methodName, new Type[2]
      {
        source.ElementType,
        memberExpression.Type
      }, new Expression[2]
      {
        source.Expression,
        (Expression) lambdaExpression
      });
            return source.Provider.CreateQuery<TSource>((Expression)methodCallExpression);
        }

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string strSort)
        {
            int length = strSort.LastIndexOf(" ");
            string propertyOrFieldName = strSort.Substring(0, length);
            string str = strSort.Substring(length + 1, strSort.Length - length - 1);
            QueryableExtensions.CheckSource((object)source);
            QueryableExtensions.CheckNullOrEmpty(propertyOrFieldName);
            string methodName = "OrderBy";
            if (SortDirection.Descending.ToString().ToUpper().Contains(str.ToUpper()))
                methodName = "OrderByDescending";
            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType);
            MemberExpression memberExpression = Expression.PropertyOrField((Expression)parameterExpression, propertyOrFieldName);
            LambdaExpression lambdaExpression = Expression.Lambda((Expression)memberExpression, new ParameterExpression[1]
      {
        parameterExpression
      });
            MethodCallExpression methodCallExpression = Expression.Call(typeof(Queryable), methodName, new Type[2]
      {
        source.ElementType,
        memberExpression.Type
      }, new Expression[2]
      {
        source.Expression,
        (Expression) lambdaExpression
      });
            return source.Provider.CreateQuery<TSource>((Expression)methodCallExpression);
        }

        private static void CheckNullOrEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("propertyName");
        }

        private static void CheckSource(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        }

        private static int GetIndex(int page, int size)
        {
            return page * size;
        }
    }
}
