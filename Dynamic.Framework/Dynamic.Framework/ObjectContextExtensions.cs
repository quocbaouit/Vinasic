using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dynamic.Framework
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ObjectContextExtensions
    {
        private static readonly IDictionary<Type, EntitySetBase> TypeMapper = (IDictionary<Type, EntitySetBase>)new Dictionary<Type, EntitySetBase>();

        public static IQueryable<T> CreateQuery<T>(this ObjectContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            string name = ObjectContextExtensions.GetEntitySet(context, typeof(T)).Name;
            return (IQueryable<T>)context.CreateQuery<T>(string.Format("[{0}]", (object)name));
        }

        private static EntitySetBase GetEntitySet(ObjectContext context, Type type)
        {
            EntitySetBase entitySetBase;
            if (!ObjectContextExtensions.TypeMapper.ContainsKey(type))
            {
                EntityContainer entityContainer = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
                context.MetadataWorkspace.LoadFromAssembly(type.Assembly);
                EdmType edmType = context.MetadataWorkspace.GetType(type.Name, type.Namespace, DataSpace.OSpace);
                while (edmType.BaseType != null)
                    edmType = edmType.BaseType;
                entitySetBase = Enumerable.First<EntitySetBase>((IEnumerable<EntitySetBase>)entityContainer.BaseEntitySets, (Func<EntitySetBase, bool>)(es => es.ElementType.Name == edmType.Name));
                ObjectContextExtensions.TypeMapper.Add(type, entitySetBase);
            }
            else
                entitySetBase = ObjectContextExtensions.TypeMapper[type];
            return entitySetBase;
        }

        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> queryable, string searchKey)
        {
            return ObjectContextExtensions.FullTextSearch<T>(queryable, searchKey, false);
        }

        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> queryable, string searchKey, bool exactMatch)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "c");
            MethodInfo method = typeof(string).GetMethod("Contains", new Type[1]
      {
        typeof (string)
      });
            typeof(object).GetMethod("ToString", new Type[0]);
            IEnumerable<PropertyInfo> enumerable = Enumerable.Where<PropertyInfo>((IEnumerable<PropertyInfo>)typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public), (Func<PropertyInfo, bool>)(p => p.PropertyType == typeof(string)));
            Expression expression1 = (Expression)null;
            string[] strArray;
            if (exactMatch)
                strArray = new string[1]
        {
          searchKey
        };
            else
                strArray = searchKey.Split(' ');
            foreach (PropertyInfo property in enumerable)
            {
                Expression instance = (Expression)Expression.Property((Expression)parameterExpression, property);
                foreach (object obj in strArray)
                {
                    Expression expression2 = (Expression)Expression.Constant(obj);
                    Expression right = (Expression)Expression.Call(instance, method, new Expression[1]
          {
            expression2
          });
                    expression1 = expression1 != null ? (Expression)Expression.Or(expression1, right) : right;
                }
            }
            MethodCallExpression methodCallExpression = Expression.Call(typeof(Queryable), "Where", new Type[1]
      {
        queryable.ElementType
      }, new Expression[2]
      {
        queryable.Expression,
        (Expression) Expression.Lambda<Func<T, bool>>(expression1, new ParameterExpression[1]
        {
          parameterExpression
        })
      });
            return queryable.Provider.CreateQuery<T>((Expression)methodCallExpression);
        }
    }
}