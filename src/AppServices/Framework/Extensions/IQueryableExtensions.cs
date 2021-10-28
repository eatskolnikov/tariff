using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Framework.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string orderByExpression)
        {
            if (string.IsNullOrEmpty(orderByExpression))
                return query;

            string propertyName, orderByMethod;
            string[] strs = orderByExpression.Split(' ');
            propertyName = strs[0];

            if (strs.Length == 1)
                orderByMethod = "OrderBy";
            else
                orderByMethod = strs[1].Equals("DESC", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

            ParameterExpression pe = Expression.Parameter(query.ElementType);
            MemberExpression me = Expression.Property(pe, propertyName);

            MethodCallExpression orderByCall = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { query.ElementType, me.Type }, query.Expression
                , Expression.Quote(Expression.Lambda(me, pe)));

            return query.Provider.CreateQuery(orderByCall) as IQueryable<T>;
        }
    }
}
