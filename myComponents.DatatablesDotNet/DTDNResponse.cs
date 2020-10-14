using myComponents.myExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace myComponents.DatatablesDotNet
{
    public class DTDNResponse
    {
        private int _draw { get; set; }
        private int _recordsTotal { get; set; }
        private int _recordsFiltered { get; set; }
        private object _data { get; set; }

        public DTDNResponse(Int32 Draw, Int32 totalRecords, Int32 filtredRecords, object data)
        {
            _draw = Draw;
            _recordsTotal = totalRecords;
            _recordsFiltered = filtredRecords;
            _data = data;
        }

        //public static DTDNResponse Create<T>(IQueryable<T> data, DTDNRequest request)
        //{
        //    int filteredResultsCount = 0;
        //    var dataPage = data.Compute(request, out filteredResultsCount);
        //    return new DTDNResponse(request.Draw, data.Count(), filteredResultsCount, dataPage);

        //}

        public static DTDNResponse Create<T>(IEnumerable<T> data, DTDNRequest request)
        {
            int filteredResultsCount = 0;
            var dataPage = data.Compute(request, out filteredResultsCount);
            return new DTDNResponse(request.Draw, data.Count(), filteredResultsCount, dataPage);
        }

        public override string ToString()
        {
            var jsondata = new
            {
                draw = this._draw,
                recordsTotal = this._recordsTotal,
                recordsFiltered = this._recordsFiltered,
                data = this._data,
            };
            return JsonConvert.SerializeObject(jsondata);
        }
    }

    public class DTDNResult : JsonResult
    {
        public DTDNResult(DTDNResponse response)
        {
            Common();
            Data = response;
        }

        public static DTDNResult Create<T>(IEnumerable<T> data, DTDNRequest request)
        {
            DTDNResponse response = DTDNResponse.Create(data, request);
            return new DTDNResult(response);
        }

        private void Common()
        {
            ContentEncoding = System.Text.Encoding.UTF8;
            ContentType = String.Format("application/json; charset={0}", ContentEncoding.WebName);
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            // base.ExecuteResult(context);
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var content = Data.ToString();
                response.Write(content);
            }
        }
    }
}