/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2012 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets
{
    using System;
    using System.Web;

    /// <summary>
    /// An <see cref="IHttpModule"/> handling Gaia Ajax requests.
    /// </summary>
    public sealed class AjaxModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application 
        /// </param>
        void IHttpModule.Init(HttpApplication context)
        {
            context.ReleaseRequestState += ProcessResponse;
            context.PreSendRequestHeaders += ProcessResponse;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        void IHttpModule.Dispose() { }

        /// <summary>
        /// Process <see cref="HttpContext.Response"/> before sending it to the client.
        /// </summary>
        private static void ProcessResponse(object sender, EventArgs e)
        {
            if (!Manager.Instance.ShouldApplyResponseFilter) return;

            var application = (HttpApplication) sender;
            var contextItems = application.Context.Items;
            
            const string moduleContext = "Gaia_WebWidgets_AjaxModule";
            
            if (contextItems.Contains(moduleContext)) return;
            contextItems.Add(moduleContext, moduleContext);

            var response = application.Response;
            response.Filter = new HttpResponseFilter(response.Filter);
        }
    }
}