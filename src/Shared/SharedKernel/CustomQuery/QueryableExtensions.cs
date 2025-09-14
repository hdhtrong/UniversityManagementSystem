using Shared.SharedKernel.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace Shared.SharedKernel.CustomQuery
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilterPaging<T>(this IQueryable<T> query,
            FilterRequest request, out int total, HashSet<string> allowedFields)
        {
            // 1. Filtering
            if (request.Filter != null)
            {
                var predicate = BuildExpression<T>(request.Filter, allowedFields);
                query = query.Where(predicate);
            }

            // 2. Total count
            total = query.Count();

            // 3. Sorting
            if (request.Sort != null && request.Sort.Any())
            {
                query = ApplySorting(query, request.Sort, allowedFields);
            }

            // 4. Paging
            query = query.Skip(request.PageIndex * request.PageSize)
                         .Take(request.PageSize);

            return query;
        }

        private static Expression<Func<T, bool>> BuildExpression<T>(Filter filter, HashSet<string> allowedFields)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var body = BuildFilterExpression<T>(filter, parameter, allowedFields);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression BuildFilterExpression<T>(
    Filter filter, ParameterExpression parameter, HashSet<string> allowedFields)
        {
            // Nếu filter chỉ là group (không có field/value) thì chỉ xử lý children
            if (string.IsNullOrWhiteSpace(filter.Field))
            {
                if (filter.Filters == null || !filter.Filters.Any())
                    throw new ArgumentException("Invalid filter group: no field and no children");

                var childExprs = filter.Filters
                    .Select(f => BuildFilterExpression<T>(f, parameter, allowedFields));

                return filter.Logic?.ToLower() == "or"
                    ? childExprs.Aggregate(Expression.OrElse)
                    : childExprs.Aggregate(Expression.AndAlso);
            }

            if (!allowedFields.Contains(filter.Field))
                throw new ArgumentException($"Field {filter.Field} is not allowed.");

            var property = Expression.Property(parameter, filter.Field);

            // ép kiểu value
            var targetType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
            var convertedValue = ConvertValue(filter.Value, targetType);
            var constant = Expression.Constant(convertedValue, property.Type);

            Expression comparison = filter.Operator switch
            {
                FilterExpressions.EQUAL => Expression.Equal(property, constant),
                FilterExpressions.NOT_EQUAL => Expression.NotEqual(property, constant),
                FilterExpressions.GREATER_THAN => Expression.GreaterThan(property, constant),
                FilterExpressions.GREATER_THAN_OR_EQUAL => Expression.GreaterThanOrEqual(property, constant),
                FilterExpressions.LESS_THAN => Expression.LessThan(property, constant),
                FilterExpressions.LESS_THAN_OR_EQUAL => Expression.LessThanOrEqual(property, constant),

                // chỉ string mới cho Contains/StartsWith/EndsWith
                FilterExpressions.CONTAINS when property.Type == typeof(string)
                    => CallMethod(property, "Contains", constant),
                FilterExpressions.NOT_CONTAIN when property.Type == typeof(string)
                    => Expression.Not(CallMethod(property, "Contains", constant)),
                FilterExpressions.STARTS_WITH when property.Type == typeof(string)
                    => CallMethod(property, "StartsWith", constant),
                FilterExpressions.ENDS_WITH when property.Type == typeof(string)
                    => CallMethod(property, "EndsWith", constant),

                _ => throw new NotSupportedException($"Operator {filter.Operator} is not supported on {property.Type.Name}")
            };

            // combine với children nếu có
            if (filter.Filters != null && filter.Filters.Any())
            {
                var childExprs = filter.Filters
                    .Select(f => BuildFilterExpression<T>(f, parameter, allowedFields));

                comparison = filter.Logic?.ToLower() == "or"
                    ? childExprs.Aggregate(comparison, Expression.OrElse)
                    : childExprs.Aggregate(comparison, Expression.AndAlso);
            }

            return comparison;
        }

        private static Expression CallMethod(Expression property, string method, Expression constant)
        {
            return Expression.Call(property, typeof(string).GetMethod(method, new[] { typeof(string) })!, constant);
        }

        private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, IEnumerable<Sort> sorts, HashSet<string> allowedFields)
        {
            bool first = true;
            foreach (var sort in sorts)
            {
                if (!allowedFields.Contains(sort.Field))
                    throw new ArgumentException($"Field {sort.Field} is not allowed for sorting.");

                query = ApplyOrder(query, sort.Field, sort.Dir, first);
                first = false;
            }
            return query;
        }

        private static IQueryable<T> ApplyOrder<T>(IQueryable<T> query, string field, string dir, bool first)
        {
            string method = first
                ? (dir.ToLower() == "desc" ? "OrderByDescending" : "OrderBy")
                : (dir.ToLower() == "desc" ? "ThenByDescending" : "ThenBy");

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);
            var lambda = Expression.Lambda(property, parameter);

            var result = typeof(Queryable).GetMethods()
                .First(m => m.Name == method && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { query, lambda });

            return (IQueryable<T>)result!;
        }

        private static object? ConvertValue(object value, Type targetType)
        {
            if (value == null) return null;

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (value is JsonElement jsonElement)
            {
                // Nếu cần parse string
                if (underlyingType == typeof(string))
                    return jsonElement.GetString();

                if (underlyingType == typeof(int))
                    return jsonElement.GetInt32();

                if (underlyingType == typeof(long))
                    return jsonElement.GetInt64();

                if (underlyingType == typeof(decimal))
                    return jsonElement.GetDecimal();

                if (underlyingType == typeof(double))
                    return jsonElement.GetDouble();

                if (underlyingType == typeof(bool))
                    return jsonElement.GetBoolean();

                if (underlyingType == typeof(DateTime))
                    return jsonElement.GetDateTime();

                // fallback: dùng string parse
                return Convert.ChangeType(jsonElement.ToString(), underlyingType);
            }

            return Convert.ChangeType(value, underlyingType);
        }

    }
}
