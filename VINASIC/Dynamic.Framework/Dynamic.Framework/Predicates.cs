using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamic.Framework.Generic;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using ExpressionVisitor = System.Linq.Expressions.ExpressionVisitor;

namespace Dynamic.Framework
{
    public class Predicates
    {
        private static readonly MethodInfo StringContainsMethod = typeof(string).GetMethod(@"Contains", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null);
        private static readonly MethodInfo BooleanEqualsMethod = typeof(bool).GetMethod(@"Equals", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(bool) }, null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDbType"></typeparam>
        /// <typeparam name="TSearchCriteria"></typeparam>
        /// <param name="searchCriteria"></param>
        /// <param name="logical"> </param>
        /// <returns></returns>
        public static Expression<Func<TDbType, bool>> BuildPredicate<TDbType, TSearchCriteria>(TSearchCriteria searchCriteria, EnumLogicals logical)
        {
            var predicate = PredicateBuilder.True<TDbType>();
            var searchCriteriaPropertyInfos = searchCriteria.GetType().GetProperties();
            foreach (var searchCriteriaPropertyInfo in searchCriteriaPropertyInfos)
            {
                EnumLogicals logicalTemp;
                if (searchCriteriaPropertyInfo.Equals(searchCriteriaPropertyInfos.First()))
                {
                    logicalTemp = EnumLogicals.And;
                }
                else
                {
                    logicalTemp = logical;
                }

                var dbFieldName = searchCriteriaPropertyInfo.Name;
                // Get the target DB type (table)
                var dbType = typeof(TDbType);
                var dbFieldMemberInfo = dbType.GetMember(dbFieldName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).Single();
                if (searchCriteriaPropertyInfo.PropertyType == typeof(string))
                {
                    predicate = ApplyStringCriterion(searchCriteria,
                      searchCriteriaPropertyInfo, dbType, dbFieldMemberInfo, predicate, logicalTemp);
                }
                // BOOLEANS
                else if (searchCriteriaPropertyInfo.PropertyType == typeof(bool?))
                {
                    predicate = ApplyBoolCriterion(searchCriteria,
                      searchCriteriaPropertyInfo, dbType, dbFieldMemberInfo, predicate, logicalTemp);
                }
                // LIST DATE
                else if (searchCriteriaPropertyInfo.PropertyType == typeof(List<DateTime>))
                {

                    predicate = ApplyDateTime(searchCriteria,
                     searchCriteriaPropertyInfo, dbType, dbFieldMemberInfo, predicate, logicalTemp);

                }
                else if (searchCriteriaPropertyInfo.PropertyType == typeof(List<string>))
                {
                    predicate = ApplyListString(searchCriteria,
                    searchCriteriaPropertyInfo, dbType, dbFieldMemberInfo, predicate, logicalTemp);
                }
            }
            return predicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDbType"></typeparam>
        /// <typeparam name="TSearchCriteria"></typeparam>
        /// <param name="searchCriteria"></param>
        /// <param name="searchCriterionPropertyInfo"></param>
        /// <param name="dbType"></param>
        /// <param name="dbFieldMemberInfo"></param>
        /// <param name="predicate"></param>
        /// <param name="logical"> </param>
        /// <returns></returns>
        private static Expression<Func<TDbType, bool>> ApplyStringCriterion<TDbType,
            TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo,
            Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate, EnumLogicals logical)
        {
            var searchString = searchCriterionPropertyInfo.GetValue(searchCriteria) as string;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return predicate;
            }
            var dbTypeParameter = Expression.Parameter(dbType, @"x");
            var dbFieldMember = Expression.MakeMemberAccess(dbTypeParameter, dbFieldMemberInfo);
            var criterionConstant = new Expression[] { Expression.Constant(searchString) };
            var containsCall = Expression.Call(dbFieldMember, StringContainsMethod, criterionConstant);
            var lambda = Expression.Lambda(containsCall, dbTypeParameter) as Expression<Func<TDbType, bool>>;
            if (logical.Equals(EnumLogicals.And))
            {
                return predicate.And(lambda);
            }
            else if (logical.Equals(EnumLogicals.Or))
            {
                return predicate.Or(lambda);
            }
            return predicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDbType"></typeparam>
        /// <typeparam name="TSearchCriteria"></typeparam>
        /// <param name="searchCriteria"></param>
        /// <param name="searchCriterionPropertyInfo"></param>
        /// <param name="dbType"></param>
        /// <param name="dbFieldMemberInfo"></param>
        /// <param name="predicate"></param>
        /// <param name="logical"> </param>
        /// <returns></returns>
        private static Expression<Func<TDbType, bool>> ApplyBoolCriterion<TDbType,
          TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo,
          Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate, EnumLogicals logical)
        {
            var searchBool = searchCriterionPropertyInfo.GetValue(searchCriteria) as bool?;
            if (searchBool == null)
            {
                return predicate;
            }
            var dbTypeParameter = Expression.Parameter(dbType, @"x");
            var dbFieldMember = Expression.MakeMemberAccess(dbTypeParameter, dbFieldMemberInfo);
            var criterionConstant = new Expression[] { Expression.Constant(searchBool) };
            var equalsCall = Expression.Call(dbFieldMember, BooleanEqualsMethod, criterionConstant);
            var lambda = Expression.Lambda(equalsCall, dbTypeParameter) as Expression<Func<TDbType, bool>>;
            if (logical.Equals(EnumLogicals.And))
            {
                return predicate.And(lambda);
            }
            else if (logical.Equals(EnumLogicals.Or))
            {
                return predicate.Or(lambda);
            }
            return predicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDbType"></typeparam>
        /// <typeparam name="TSearchCriteria"></typeparam>
        /// <param name="searchCriteria"></param>
        /// <param name="searchCriterionPropertyInfo"></param>
        /// <param name="dbType"></param>
        /// <param name="dbFieldMemberInfo"></param>
        /// <param name="predicate"></param>
        /// <param name="logical"> </param>
        /// <returns></returns>
        private static Expression<Func<TDbType, bool>> ApplyDateTime<TDbType,
          TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo,
          Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate, EnumLogicals logical)
        {
            var searchDateTime = (List<DateTime>)searchCriterionPropertyInfo.GetValue(searchCriteria);
            if (searchDateTime == null)
            {
                return predicate;
            }
            var dateTimeFrom = searchDateTime.First().AddDays(-1);
            var dateTimeTo = searchDateTime.Last().AddDays(1);
            var dbTypeParameter = Expression.Parameter(dbType, @"x");
            var dbFieldMember = Expression.MakeMemberAccess(dbTypeParameter, dbFieldMemberInfo);

            // check greater than
            var criterionConstant = Expression.Constant(dateTimeFrom);
            var greater = Expression.GreaterThanOrEqual(dbFieldMember, criterionConstant);
            var lambdaGreater = Expression.Lambda(greater, dbTypeParameter) as Expression<Func<TDbType, bool>>;
            if (logical.Equals(EnumLogicals.And))
            {
                predicate = predicate.And(lambdaGreater);
            }
            else if (logical.Equals(EnumLogicals.Or))
            {
                predicate = predicate.Or(lambdaGreater);
            }

            //check less than
            criterionConstant = Expression.Constant(dateTimeTo);
            var less = Expression.LessThanOrEqual(dbFieldMember, criterionConstant);
            var lambdaLess = Expression.Lambda(less, dbTypeParameter) as Expression<Func<TDbType, bool>>;
            if (logical.Equals(EnumLogicals.And))
            {
                predicate = predicate.And(lambdaLess);
            }
            else if (logical.Equals(EnumLogicals.Or))
            {
                predicate = predicate.Or(lambdaLess);
            }
            return predicate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDbType"></typeparam>
        /// <typeparam name="TSearchCriteria"></typeparam>
        /// <param name="searchCriteria"></param>
        /// <param name="searchCriterionPropertyInfo"></param>
        /// <param name="dbType"></param>
        /// <param name="dbFieldMemberInfo"></param>
        /// <param name="predicate"></param>
        /// <param name="logical"> </param>
        /// <returns></returns>
        private static Expression<Func<TDbType, bool>> ApplyListString<TDbType,
          TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo,
          Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate, EnumLogicals logical)
        {
            var searchStrings = (List<string>)searchCriterionPropertyInfo.GetValue(searchCriteria);
            if (searchStrings == null || searchStrings.Count < 1)
            {
                return predicate;
            }
            var dbTypeParameter = Expression.Parameter(dbType, @"x");
            var dbFieldMember = Expression.MakeMemberAccess(dbTypeParameter, dbFieldMemberInfo);

            var criterionFirstConstant = new Expression[] { Expression.Constant(searchStrings[0]) };
            var containsFirstCall = Expression.Call(dbFieldMember, StringContainsMethod, criterionFirstConstant);
            var lambda = Expression.Lambda(containsFirstCall, dbTypeParameter) as Expression<Func<TDbType, bool>>;
            if (searchStrings.Count > 1)
            {
                for (var i = 1; i < searchStrings.Count; i++)
                {
                    var criterionConstant = new Expression[] { Expression.Constant(searchStrings[i]) };
                    var containsCall = Expression.Call(dbFieldMember, StringContainsMethod, criterionConstant);
                    var lambdaSub = Expression.Lambda(containsCall, dbTypeParameter) as Expression<Func<TDbType, bool>>;
                    lambda = lambda.Or(lambdaSub);
                }
            }
            lambda.Reduce();
            if (logical.Equals(EnumLogicals.And))
            {
                predicate = predicate.And(lambda);
            }
            else if (logical.Equals(EnumLogicals.Or))
            {
                predicate = predicate.Or(lambda);
            }
            return predicate;
        }
    }
}
