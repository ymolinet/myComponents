using IMA.Portail.Custom.DatatablesDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IMA.Portail.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Compute<T>(this IEnumerable<T> data, DTDNRequest request, out int filteredDataCount)
        {
            filteredDataCount = 0;
            if (!data.Any() || request == null)
                return data;

            // Global filtering.
            // Filter is being manually applied due to in-memmory (IEnumerable) data.
            // If you want something rather easier, check IEnumerableExtensions Sample.
            // var filteredData = data.Where(_item => _item.Hostname.Contains(request.Search.Value));
            IEnumerable<T> filteredData = Enumerable.Empty<T>();

            // Inutile de faire une recherche s'il n'y a rien à chercher.
            if (!String.IsNullOrEmpty(request.Search.Value))
            {
                var filteredColumn = request.Columns.Where(c => c.Searchable == true);
                foreach (DTDNColumn sColumn in filteredColumn)
                {
                    IEnumerable<T> columnResult = data.SearchIn(sColumn.Name, request.Search.Value);
                    filteredData = filteredData.Concat(columnResult);
                }
                // Pour éviter les doublons
                filteredData = filteredData.Distinct();
            }
            else filteredData = data;

            // Ordering filtred data
            var orderedColumn = request.Columns.Where(c => c.Orderable == true && c.Order != null);
            foreach (DTDNColumn sColumn in orderedColumn)
            {
                filteredData = filteredData.OrderBy(sColumn);
            }

            // Paging filtered data.
            // Paging is rather manual due to in-memmory (IEnumerable) data.
            // var dataPage = filteredData.OrderBy(d => d.ID).Skip(request.Start);
            var dataPage = filteredData.Skip(request.Start);
            if (request.Length > -1) dataPage = dataPage.Take(request.Length);

            filteredDataCount = filteredData.Count();
            return dataPage;
        }

        // https://github.com/ALMMa/datatables.aspnet/issues/58
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, DTDNColumn column)
        {
            if (!entities.Any() || column == null)
                return entities;

            var propertyInfo = entities.First().GetType().GetProperty(column.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (column.Order.Dir == DTDNOrderDir.Ascending)
            {
                return entities.OrderBy(e => propertyInfo.GetValue(e, null));
            }
            return entities.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }

        // Inspire : https://stackoverflow.com/questions/22104050/linq-to-entities-does-not-recognize-the-method-system-object-getvalue
        // and : https://stackoverflow.com/questions/4553836/how-to-create-an-expression-tree-to-do-the-same-as-startswith
        public static IEnumerable<T> PropertyContains<T>(this IEnumerable<T> data, PropertyInfo propertyInfo, string value, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            //var propertyInfo = data.First().GetType().GetProperty(sColumn.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            //// IEnumerable<T> columnResult = data.Where(d => d.GetType().GetProperty(sColumn.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(d, null).ToString().Contains(request.Search.Value));

            //// On cast en minuscule pour que la recherche ne soit pas sensible à la case
            //ParameterExpression param = Expression.Parameter(typeof(T));
            //MemberExpression m = Expression.MakeMemberAccess(param, propertyInfo);

            //ConstantExpression v = Expression.Constant(value, typeof(string));
            //ConstantExpression c = Expression.Constant(comparison);

            //MethodInfo mi_tostring = typeof(object).GetMethod("ToString");
            //var obj_to_string = Expression.Call(m, mi_tostring);

            //var indexOf = Expression.Call(obj_to_string, "IndexOf", null,
            //    Expression.Constant(value, typeof(string)),
            //    Expression.Constant(comparison));

            //var like = Expression.GreaterThanOrEqual(indexOf, Expression.Constant(0));
            //Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(like, param);

            //return data.AsQueryable().Where(lambda);

            ParameterExpression param = Expression.Parameter(typeof(T));
            MemberExpression m = Expression.MakeMemberAccess(param, propertyInfo);
            ConstantExpression c = Expression.Constant(value, typeof(string));
            MethodInfo mi_contains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            MethodInfo mi_tostring = typeof(object).GetMethod("ToString");
            Expression call = Expression.Call(Expression.Call(m, mi_tostring), mi_contains, c);

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(call, param);

            return data.AsQueryable().Where(lambda);
        }

        // https://stackoverflow.com/questions/32176293/how-to-get-value-from-ienumerable-collection-using-its-key/32176481
        public static IEnumerable<T> SearchIn<T>(this IEnumerable<T> data, string propertyName, string value, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            Type type = typeof(T);
            var propertyInfo = data.First().GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            ParameterExpression param = Expression.Parameter(type, "param");
            MemberExpression m = Expression.MakeMemberAccess(param, propertyInfo);
            // Expression m = Expression.PropertyOrField(param, param);

            ConstantExpression v = Expression.Constant(value, typeof(string));
            ConstantExpression c = Expression.Constant(comparison);

            MethodInfo mi_tostring = typeof(object).GetMethod("ToString");
            var obj_to_string = Expression.Call(m, mi_tostring);

            var indexOf = Expression.Call(obj_to_string, "IndexOf", null,
                Expression.Constant(value, typeof(string)),
                Expression.Constant(comparison));

            var like = Expression.GreaterThanOrEqual(indexOf, Expression.Constant(0));
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(like, param);

            return data.AsQueryable().Where(lambda);

        }
    }
}