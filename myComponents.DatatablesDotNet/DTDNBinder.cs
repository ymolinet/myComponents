using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace myComponents.DatatablesDotNet
{
    public class DTDNBinder : IModelBinder
    {
        private List<string> _additionnalparameters;

        public DTDNBinder() : base()
        {
            this._additionnalparameters = new List<string>();
        }

        public DTDNBinder(string[] additionnalParameters):base()
        {
            this._additionnalparameters = new List<string>(additionnalParameters);
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            DTDNAttributes attributes = new DTDNAttributes(controllerContext.RequestContext.HttpContext.Request.HttpMethod);

            IValueProvider values = bindingContext.ValueProvider;
            DTDNRequest request = new DTDNRequest();

            var _draw = values.GetValue(attributes.Draw);
            if (_draw == null)
                throw new Exception("La requête reçue n'est pas conforme à une requête provenant de Datatables.net");
            request.Draw = Convert.ToInt32(_draw.AttemptedValue);

            var _start = values.GetValue(attributes.Start);
            request.Start = Convert.ToInt32(_start.AttemptedValue);

            var _length = values.GetValue(attributes.Length);
            request.Length = Convert.ToInt32(_length.AttemptedValue);

            var _searchvalue = values.GetValue(attributes.SearchValue);
            if (_searchvalue != null)
                request.Search.Value = Convert.ToString(_searchvalue.AttemptedValue);
            else request.Search.Value = string.Empty;

            var _searchregex = values.GetValue(attributes.SearchRegex);
            if (_searchregex != null)
                request.Search.Regex = Convert.ToBoolean(_searchregex.AttemptedValue);
            else request.Search.Regex = false;

            request.Orders.AddRange(ParseOrders(values, attributes));
            request.Columns.AddRange(ParseColumns(values, request.Orders, attributes));
            request.AdditionalParameters.AddRange(ParseAdditionnalParameters(values, _additionnalparameters));
            return request;
        }

        public List<string> AdditionnalParameters
        {
            get { return this._additionnalparameters; }
            private set { }
        }

        private static IEnumerable<DTDNParameter> ParseAdditionnalParameters(IValueProvider values, List<string> additionnalparameters)
        {
            var parameters = new List<DTDNParameter>();
            foreach (string parameter in additionnalparameters)
            {
                var value = values.GetValue(parameter);
                string paramvalue = string.Empty;
                if (value != null)
                    paramvalue = value.AttemptedValue;
                parameters.Add(new DTDNParameter(parameter, paramvalue));
            }
            return parameters;
        }

        private static IEnumerable<DTDNOrder> ParseOrders(IValueProvider values, DTDNAttributes attributes)
        {
            var orders = new List<DTDNOrder>();
            int counter = 0;
            while (true)
            {
                DTDNOrder anOrder = ParseOrder(values, attributes, counter);
                if (anOrder == null) break;
                orders.Add(anOrder);
                counter++;
            }
            return orders;
        }

        private static DTDNOrder ParseOrder(IValueProvider values, DTDNAttributes attributes, int counter)
        {
            string prefixOrder = string.Format("order[{0}]", counter);
            if (values.ContainsPrefix(prefixOrder))
            {
                DTDNOrder anOrder = new DTDNOrder();
                anOrder.id = counter;

                var colum = values.GetValue(prefixOrder + attributes.Column);
                anOrder.Column = Convert.ToInt32(colum.AttemptedValue);

                var dir = values.GetValue(prefixOrder + attributes.Dir);
                if (dir.AttemptedValue.ToLower() == "asc") anOrder.Dir = DTDNOrderDir.Ascending;
                else anOrder.Dir = DTDNOrderDir.Descending;
                return anOrder;
            }
            else return null;
        }

        private static IEnumerable<DTDNColumn> ParseColumns(IValueProvider values, List<DTDNOrder> orders, DTDNAttributes attributes)
        {
            var columns = new List<DTDNColumn>();

            int counter = 0;
            while (true)
            {
                string prefixColumn = string.Format("columns[{0}]", counter);
                if (values.ContainsPrefix(prefixColumn))
                {
                    DTDNColumn aColumn = new DTDNColumn();
                    aColumn.id = counter;

                    var name = values.GetValue(prefixColumn + attributes.Name);
                    aColumn.Name = name.AttemptedValue;

                    var data = values.GetValue(prefixColumn + attributes.Data);
                    aColumn.Data = name.AttemptedValue;

                    var orderable = values.GetValue(prefixColumn + attributes.Orderable);
                    aColumn.Orderable = Convert.ToBoolean(orderable.AttemptedValue);

                    var searchable = values.GetValue(prefixColumn + attributes.Searchable);
                    aColumn.Searchable = Convert.ToBoolean(searchable.AttemptedValue);

                    var search_value = values.GetValue(prefixColumn + attributes.SearchValue);
                    if (search_value != null)
                        aColumn.Search.Value = search_value.AttemptedValue;
                    else aColumn.Search.Value = string.Empty;

                    var search_regex = values.GetValue(prefixColumn + attributes.SearchRegex);
                    if (search_regex != null)
                        aColumn.Search.Regex = Convert.ToBoolean(search_regex.AttemptedValue);
                    else aColumn.Search.Regex = false;

                    aColumn.Order = orders.Where(o => o.Column == aColumn.id).FirstOrDefault();

                    columns.Add(aColumn);
                    counter++;
                }
                else break;
            }
            return columns;
        }
    }
}