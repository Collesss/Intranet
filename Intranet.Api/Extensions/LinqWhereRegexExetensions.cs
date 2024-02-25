using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Intranet.Api.Extensions
{
    public static class LinqWhereRegexExetensions
    {
        /*
        private static MethodInfo LambdaGenericMethod { get; }

        private static MethodInfo WhereGenericMethod { get; }
        */

        private static MethodInfo ToStringMethod { get; }

        private static MethodInfo RegexIsMatchMethod { get; }


        static LinqWhereRegexExetensions()
        {
            /*
            LambdaGenericMethod = typeof(Expression).GetMethods().Single(methInf =>
                methInf.Name == "Lambda" && methInf.ContainsGenericParameters && methInf.GetParameters().Length == 2 &&
                methInf.GetParameters()[0].ParameterType.Name == "Expression" && methInf.GetParameters()[1].ParameterType.Name == "ParameterExpression[]");
            */

            RegexIsMatchMethod = typeof(Regex).GetMethods().Single(methInf =>
                methInf.Name == "IsMatch" && methInf.GetParameters().Length == 2 && methInf.GetParameters()[0].ParameterType.Name == "String" &&
                methInf.GetParameters()[1].ParameterType.Name == "String");

            ToStringMethod = typeof(object).GetMethods().Single(methInf =>
                methInf.Name == "ToString");

            /*
            WhereGenericMethod = typeof(Queryable).GetMethods().Single(methInf =>
                methInf.Name == "Where" && methInf.ContainsGenericParameters && methInf.GetParameters().Length == 2 &&
                methInf.GetParameters()[0].ParameterType.Name == "IQueryable`1" && methInf.GetParameters()[1].ParameterType.Name == "Expression`1" &&
                methInf.GetParameters()[1].ParameterType.GenericTypeArguments.Length == 1 &&
                methInf.GetParameters()[1].ParameterType.GenericTypeArguments[0].Name == "Func`2");
            */
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

        /*
        private static MethodInfo GetOrderByOrOrderByDescendingGenericMethod(bool asc) =>
            typeof(Queryable).GetMethods().Single(methInf =>
                methInf.Name == (asc ? "OrderBy" : "OrderByDescending") && methInf.GetParameters().Length == 2 && 
                methInf.GetParameters()[0].ParameterType.Name == "IQueryable`1" && methInf.GetParameters()[1].ParameterType.Name == "Expression`1");

        private static MethodInfo GetLambdaGenericMethod() =>
            typeof(Expression).GetMethods().Single(methInf =>
                methInf.Name == "Lambda" && methInf.ContainsGenericParameters && methInf.GetParameters().Length == 2 && 
                methInf.GetParameters()[0].ParameterType.Name == "Expression" && methInf.GetParameters()[1].ParameterType.Name == "ParameterExpression[]");

        private static MethodInfo GetWhereGenericMethod() =>
            typeof(Queryable).GetMethods().Single(methInf => 
                methInf.Name == "Where" && methInf.ContainsGenericParameters && methInf.GetParameters().Length == 2 &&
                methInf.GetParameters()[0].ParameterType.Name == "IQueryable`1" && methInf.GetParameters()[1].ParameterType.Name == "Expression`1" &&
                methInf.GetParameters()[1].ParameterType.GenericTypeArguments.Length == 1 && 
                methInf.GetParameters()[1].ParameterType.GenericTypeArguments[0].Name == "Func`2");
        

        private static MethodInfo GetIsMatchMethod() =>
            typeof(Regex).GetMethods().Single(methInf => 
                methInf.Name == "IsMatch" && methInf.GetParameters().Length == 2 && methInf.GetParameters()[0].ParameterType.Name == "String" && 
                methInf.GetParameters()[1].ParameterType.Name == "String");
        */

        private static IEnumerable<string> GetFields<T>()
        {
            string[] defaultTypes = new string[] { "Enum", "Char", "String", "Boolean", "Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64", "Single", "Double", "Decimal" };

            IEnumerable<string> GetFields(PropertyInfo property, string prefix = "") =>
                defaultTypes.Contains(property.PropertyType.Name) ? new string[] { $"{prefix}{property.Name}" }
                : property.PropertyType.GetProperties().SelectMany(prop => GetFields(prop, $"{prefix}{property.Name}."));

            return typeof(T).GetProperties().SelectMany(prop => GetFields(prop)).ToList();
        }

        public static IQueryable<T> WhereRegex<T>(this IQueryable<T> collection, string property, string regExExpression)
        {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            ArgumentException.ThrowIfNullOrEmpty(property, nameof(property));
            ArgumentException.ThrowIfNullOrEmpty(regExExpression, nameof(regExExpression));

            Type typeEntity = typeof(T);

            if (!GetFields<T>().Contains(property))
                throw new ArgumentException($"Property with name: {property}, not exit in type: {typeEntity.Name}", nameof(property));

            ParameterExpression parameter = Expression.Parameter(typeEntity);

            Expression memberExpression = GetExpressionMember(parameter, new Queue<string>(property.Split('.')));

            Expression constRegex = Expression.Constant(regExExpression);

            Expression expressionCallIsMatch = Expression.Call(RegexIsMatchMethod, memberExpression, constRegex);

            Expression<Func<T, bool>> expressionPredicate = Expression.Lambda<Func<T, bool>>(expressionCallIsMatch, parameter);

            return collection.Where(expressionPredicate);
        }

        public static IQueryable<T> WhereRegexAnd<T>(this IQueryable<T> collection, IEnumerable<(string property, string regExExpression)> regexPredicates)
        {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            ArgumentNullException.ThrowIfNull(regexPredicates, nameof(regexPredicates));

            if (!regexPredicates.Any())
                return collection;

            Type typeEntity = typeof(T);

            foreach (var regexPredicate in regexPredicates)
            {
                if (!GetFields<T>().Contains(regexPredicate.property))
                    throw new ArgumentException($"Property with name: {regexPredicate}, not exit in type: {typeEntity.Name}", nameof(regexPredicate));

                ArgumentException.ThrowIfNullOrEmpty("regExExpression can't be null or empty.", nameof(regexPredicates));
            }

            ParameterExpression parameter = Expression.Parameter(typeEntity);


            Expression expressionsAnd = regexPredicates.Select(regexPredicate =>
            {
                Expression memberExpression = GetExpressionMember(parameter, new Queue<string>(regexPredicate.property.Split('.')));

                Expression constRegex = Expression.Constant(regexPredicate.regExExpression);

                Expression expressionCallToString = Expression.Call(memberExpression, ToStringMethod);

                Expression expressionCallIsMatch = Expression.Call(RegexIsMatchMethod, expressionCallToString, constRegex);

                return expressionCallIsMatch;

            }).Aggregate(Expression.AndAlso);


            Expression<Func<T, bool>> expressionPredicate = Expression.Lambda<Func<T, bool>>(expressionsAnd, parameter);


            return collection.Where(expressionPredicate);
        }


        /*
        public static IQueryable<T> Where<T>(this IQueryable<T> collection, string predicate)
        {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));
            ArgumentException.ThrowIfNullOrEmpty(predicate, nameof(predicate));

            Expression<Func<T, bool>> expressionPredicate = GetPredicate<T>(predicate);

            return collection.Where(expressionPredicate);
        }


        private static Expression<Func<T, bool>> GetPredicate<T>(string predicate)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));



            return null;
        }
        */
    }
}
