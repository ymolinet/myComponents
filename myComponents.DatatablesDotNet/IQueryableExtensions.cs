using NinjaNye.SearchExtensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace myComponents.DatatablesDotNet
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Compute<T>(this IQueryable<T> data, DTDNRequest request, out int filteredDataCount)
        {
            filteredDataCount = 0;
            if (!data.Any() || request == null)
                return data;

            IQueryable<T> filteredData = data;
            string[] filteredColumn = request.Columns.Where(c => c.Searchable == true).Select(c => c.Name).ToArray();
            filteredData = data.SearchIn(filteredColumn, request.Search.Value);

            //if (!string.IsNullOrEmpty(request.Search.Value))
            //{
            //    var filteredColumn = request.Columns.Where(c => c.Searchable == true);
            //    filteredData = filteredData.SearchIn(filteredColumn, request.Search.Value);
            //}
            filteredDataCount = filteredData.Count();
            var dataPage = filteredData.Skip(request.Start);
            if (request.Length > -1) dataPage = dataPage.Take(request.Length);

            return dataPage;
        }

        public static IQueryable<T> SearchIn<T>(this IQueryable<T> data, string[] columns, string value)
        {
            Type type = typeof(T);
            Expression<Func<T, string>> lambda = null;
            foreach (string aColumn in columns)
            {
                Expression<Func<T, string>> expression = BuildStringPropertyExpression<T>(aColumn);
                if (lambda == null) lambda = expression;
                else lambda = Expression.AndAlso(lambda, expression);
            }
            return data.Search(global_lambda).Containing(value);

        }

        public static Expression<Func<T, string>> BuildStringPropertyExpression<T>(string propertyName)
        {
            var props = typeof(T).GetProperties();
            var propertyInfo = props.FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
            if (propertyInfo == null || propertyInfo.PropertyType != typeof(string)) throw new Exception("invalid property");

            var param = Expression.Parameter(typeof(T), string.Empty);
            var property = Expression.Property(param, propertyInfo);
            return Expression.Lambda<Func<T, string>>(property, param);
        }
    }
}
