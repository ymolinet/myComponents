/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Web.UI;
using System.Reflection;
using MethodCache = Gaia.WebWidgets.Reflection.Cache<Gaia.WebWidgets.MethodAttribute>;

namespace Gaia.WebWidgets
{
    internal static class MethodDispatcher
    {
        private static string _returnValue;

        public class DispatchContext
        {
            private readonly Control _control;
            private readonly string[] _parameters;

            public string[] Parameters
            {
                get { return _parameters; }
            }

            public Control Control
            {
                get { return _control; }
            }

            public DispatchContext(Control control, string[] parameters)
            {
                _parameters = parameters;
                _control = control;
            }
        }

        public static string ReturnValue
        {
            get { return _returnValue; }
        }

        public static void DispatchMethodCall(DispatchContext context)
        {
            _returnValue = null;
            string typeOfMethod = context.Parameters[1];

            if (typeOfMethod.Equals("AspectMethod"))
            {
                // Setting response to NON-JAVASCRIPT to avoid execution within prototype.js/Ajax.Request
                Manager.Instance.Page.Response.ContentType = "text/gaiaScript";

                DispatchAspectMethodCall(context);
            }
            else if (typeOfMethod.Equals("ControlMethod"))
            {
                // Setting response to NON-JAVASCRIPT to avoid execution within prototype.js/Ajax.Request
                Manager.Instance.Page.Response.ContentType = "text/gaiaScript";

                DispatchControlMethodCall(context);
            }
            else if (typeOfMethod.Equals("PageMethod"))
            {
                // Setting response to NON-JAVASCRIPT to avoid execution within prototype.js/Ajax.Request
                Manager.Instance.Page.Response.ContentType = "text/gaiaScript";

                DispatchPageMethodCall(context);
            }
        }

        private static void DispatchAspectMethodCall(DispatchContext context)
        {
            // prepare parameters
            var parameters = new string[context.Parameters.Length - 3];
            Array.Copy(context.Parameters, 3, parameters, 0, parameters.Length);

            string methodName = context.Parameters[0];

            bool foundAspectMethod = false;
            var ctrl = (IAspectableAjaxControl)context.Control;
            foreach (IAspect idx in ctrl.Aspects)
            {
                MethodInfo methodInfo = MethodCache.GetMethod(idx, methodName, false);
                if (methodInfo != null)
                {
                    foundAspectMethod = true;
                    ExecuteMethodCall(idx, methodInfo, parameters);
                }
            }

            if (!foundAspectMethod)
                throw new ApplicationException("No aspect contains method " + methodName);
        }

        private static void DispatchControlMethodCall(DispatchContext context)
        {
            string methodName = context.Parameters[0];
            MethodInfo methodInfo = MethodCache.GetMethod(context.Control, methodName, true);

            // prepare parameters
            var parameters = new string[context.Parameters.Length - 3];
            Array.Copy(context.Parameters, 3, parameters, 0, parameters.Length);
            
            ExecuteMethodCall(context.Control, methodInfo, parameters);
        }

        private static void DispatchPageMethodCall(DispatchContext context)
        {
            string[] parameters = context.Parameters;

            // Now the first entry in the array contains the name of the method and the second contains the TYPE of method
            string methodName = parameters[0];
            string[] splits = methodName.Split('.');

            if (splits.Length == 1)
                ExecutePageMethodCall(context, methodName);
            else
                ExecuteUserControlCall(context, splits);
        }

        private static void ExecuteUserControlCall(DispatchContext context, string[] components)
        {
            string name = components[0];
            var page = (Page)context.Control;
            string methodName = components[components.Length - 1];

            // try to find the first UserControl in this Page or its possible Nested Masters
            Control control = page.FindControl(name);
            if (control == null)
            {
                MasterPage master = page.Master;
                while (master != null)
                {
                    control = master.FindControl(name);

                    if (control != null)
                        break;

                    master = master.Master;
                }
            }

            if (control == null)
                throw new ApplicationException("Could not find UserControl " + name + " on the WebPage or on its Nested MasterPages");

            // find nested user controls
            for (int index = 1; index < components.Length - 1; ++index)
            {
                control = control.FindControl(components[index]);

                if (control == null)
                    throw new ApplicationException("Could not find UserControl " + string.Join(".", components, 0, index + 1));
            }

            MethodInfo method = MethodCache.GetMethod(control, methodName, true);

            // prepare parameters
            var parameters = new string[context.Parameters.Length - 2];
            Array.Copy(context.Parameters, 2, parameters, 0, parameters.Length);

            ExecuteMethodCall(control, method, parameters);
        }

        private static void ExecutePageMethodCall(DispatchContext context, string methodName)
        {
            var page = (Page)context.Control;
            object sender = page;
            MethodInfo method = MethodCache.GetMethod(page, methodName, false);
            
            if (method == null)
            {
                // Searching in MasterPages...!
                for(MasterPage master = page.Master; master != null; master = master.Master)
                {
                    sender = master;
                    method = MethodCache.GetMethod(master, methodName, false);
                    if (method != null) break;
                }

                if (method == null)
                    throw new ApplicationException("Couldn't find the method named: " + methodName + " on neither WebPage nor Nested MasterPages. Are you sure you tagged the method with the 'Method' attribute?");
            }

            // prepare parameters
            var parameters = new string[context.Parameters.Length - 2];
            Array.Copy(context.Parameters, 2, parameters, 0, parameters.Length);

            ExecuteMethodCall(sender, method, parameters);
        }

        private static void ExecuteMethodCall(object sender, MethodInfo method, string[] parameters)
        {
            var paramsToMethod = new object[parameters.Length];
            var prs = method.GetParameters();

            for (var idx = 0; idx < prs.Length; ++idx)
            {
                paramsToMethod[idx] = Convert.ChangeType(parameters[idx], prs[idx].ParameterType, System.Globalization.CultureInfo.InvariantCulture);
                var value = paramsToMethod[idx] as string;
                if (value != null)
                    paramsToMethod[idx] = value.Replace("|$|", ",");
            }

            object retVal = method.Invoke(sender, paramsToMethod);

            if (retVal != null)
                _returnValue = retVal.ToString();
        }
    }
}
