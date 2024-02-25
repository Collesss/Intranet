using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Intranet.Api.Extensions
{
    public static class LinqOrderByExetensions
    {
        private static MethodInfo LambdaGenericMethod { get; }
        private static MethodInfo OrderByGenericMethod { get; }
        private static MethodInfo OrderByDescendingGenericMethod { get; }


        static LinqOrderByExetensions()
        {
            LambdaGenericMethod = typeof(Expression).GetMethods().Single(methInf =>
                methInf.Name == "Lambda" && methInf.ContainsGenericParameters && methInf.GetParameters().Length == 2 &&
                methInf.GetParameters()[0].ParameterType.Name == "Expression" && methInf.GetParameters()[1].ParameterType.Name == "ParameterExpression[]");

            OrderByGenericMethod = typeof(Queryable).GetMethods().Single(methInf =>
                methInf.Name == "OrderBy" && methInf.GetParameters().Length == 2 &&
                methInf.GetParameters()[0].ParameterType.Name == "IQueryable`1" && methInf.GetParameters()[1].ParameterType.Name == "Expression`1");

            OrderByDescendingGenericMethod = typeof(Queryable).GetMethods().Single(methInf =>
                methInf.Name == "OrderByDescending" && methInf.GetParameters().Length == 2 &&
                methInf.GetParameters()[0].ParameterType.Name == "IQueryable`1" && methInf.GetParameters()[1].ParameterType.Name == "Expression`1");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type element collection.</typeparam>
        /// <param name="collection">Collection.</param>
        /// <param name="property">Sort property.</param>
        /// <param name="asc">If true sort ascending.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> collection, string property, bool asc = true)
        {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            ArgumentException.ThrowIfNullOrEmpty(property, nameof(property));

            Type typeEntity = typeof(T);

            if (!GetFields<T>().Contains(property))
                throw new ArgumentException($"Property with name: {property}, not exit in type: {typeEntity.Name}", nameof(property));

            ParameterExpression parameter = Expression.Parameter(typeEntity);
            Expression memberExpression = GetExpressionMember(parameter, new Queue<string>(property.Split('.')), out Type propertyInfoEntity);

            Type typeFuncWithGenericParam = typeof(Func<,>).MakeGenericType(typeEntity, propertyInfoEntity);

            MethodInfo methodInfoLambda = LambdaGenericMethod.MakeGenericMethod(typeFuncWithGenericParam);

            object expressionSelectProperty = methodInfoLambda.Invoke(null, new object[] { memberExpression, new ParameterExpression[] { parameter } });

            MethodInfo methodInfoOrderBy = (asc ? OrderByGenericMethod : OrderByDescendingGenericMethod).MakeGenericMethod(typeEntity, propertyInfoEntity);

            return (IQueryable<T>)methodInfoOrderBy.Invoke(null, new object[] { collection, expressionSelectProperty });
        }


        public static IQueryable<T> OrderBy<T>(this IQueryable<T> collection, IEnumerable<(string property, bool asc)> sorts)
        {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            ArgumentNullException.ThrowIfNull(sorts, nameof(sorts));

            return sorts.Aggregate(collection, (coll, sort) => coll.OrderBy(sort.property, sort.asc));
        }

        private static Expression GetExpressionMember(Expression parameter, Queue<string> fields, out Type typeLastProperty)
        {
            if (fields.TryDequeue(out string field))
            {
                MemberExpression memberExpression = Expression.Property(parameter, field);

                return GetExpressionMember(memberExpression, fields, out typeLastProperty);
            }

            typeLastProperty = parameter.Type;

            return parameter;
        }

        private static Expression GetExpressionMember(Expression parameter, Queue<string> fields) =>
            fields.TryDequeue(out string field) ? GetExpressionMember(Expression.Property(parameter, field), fields) : parameter;

        private static IEnumerable<string> GetFields<T>()
        {
            string[] defaultTypes = new string[] { "Enum", "Char", "String", "Boolean", "Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64", "Single", "Double", "Decimal" };

            IEnumerable<string> GetFields(PropertyInfo property, string prefix = "") =>
                defaultTypes.Contains(property.PropertyType.Name)
                ? new string[] { $"{prefix}{property.Name}" }
                : property.PropertyType.GetProperties().SelectMany(prop => GetFields(prop, $"{prefix}{property.Name}."));

            return typeof(T).GetProperties().SelectMany(prop => GetFields(prop)).ToList();
        }
    }
}
