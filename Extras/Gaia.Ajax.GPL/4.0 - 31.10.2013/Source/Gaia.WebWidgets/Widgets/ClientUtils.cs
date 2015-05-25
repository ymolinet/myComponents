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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

[assembly: WebResource("Gaia.WebWidgets.Scripts.ClientUtils.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Various ClientSide utilities to modify DOM from the server.
    /// </summary>
    [ToolboxItem(false)]
    public class ClientUtils : GaiaControl, IAjaxControlNoRoot, IExtraPropertyCallbackRenderer
    {
        #region [ -- Private Members -- ]
        private readonly RegisterControl _registerControl = new RegisterControl();
        private List<string> _invoke; 
        #endregion

        #region [ -- Public Functions -- ]

        /// <summary>
        /// This function will take two controls and position the target directly beneath the
        /// source control. It doesn't copy width or height properties ...
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void ClonePosition(Control source, Control target)
        {
            Invoke("setClonePosition", true, source.ClientID, target.ClientID);
        }

        #endregion

        #region [ -- Overriden base class methods -- ]

        /// <summary>
        /// Include javascript files.
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.ClientUtils.js", typeof(ClientUtils), "Gaia.ClientUtils.browserFinishedLoading", true);
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        /// <summary>
        /// Called by the Gaia framework when the StateManager of the Control needs to be initialized
        /// </summary>
        /// <returns>StateManager for the Control</returns>
        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerControl(this, ClientID, this);
        }

        /// <summary>
        /// Returns the Javascript used to register the control on the client. 
        /// </summary>
        /// <returns></returns>
        string IAjaxControl.GetScript()
        {
            // we do late-initialization of the clientId and ControlType to ensure 
            // that RegisterControl's Invoke function can be used 
            
            _registerControl.ClientId = ClientID;
            _registerControl.ControlType = "Gaia.ClientUtils";
            return _registerControl.ToString();
        }


        #endregion

        #region [ -- ExtraPropertyCallbackRenderer Members -- ]

        /// <summary>
        /// Implementation of your custom rendering. This expects to be in the form of; 
        /// ".foo(x)" where the foo method on the client MUST return the this argument.
        /// If this method returns nothing, nothing will be rendered unless other things 
        /// are having rendering output.
        /// </summary>
        /// <param name="code">An already built StringBuilder which you're supposed to render your data into</param>
        public void InjectPropertyChangesToCallbackResponse(StringBuilder code)
        {
            // serialize client-side invocations
            if (_invoke == null) return;
            foreach (string observe in _invoke)
            {
                code.Append(observe);
            }
        }

        #endregion

        #region [ -- Helper Functions -- ]
        private void Invoke(string function, bool addQuotes, params object[] args)
        {
            if (_invoke == null)
                _invoke = new List<string>();

            var invoke = new StringBuilder();
            invoke.Append(".").Append(function).Append("(");

            if (args != null)
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    if (args[i] is Array) // arrays are rendered as System.Object[] so we omit those
                        continue;

                    if (i != 0) invoke.Append(", ");

                    if (addQuotes)
                        invoke.Append("'");

                    invoke.Append(args[i].ToString());

                    if (addQuotes)
                        invoke.Append("'");
                }
            }

            invoke.Append(")");

            // add the invocation to the list of invokes
            _invoke.Add(invoke.ToString());

            // forward the invoke to the RegisterControl variable
            _registerControl.Invoke(function, addQuotes, args);

        } 
        #endregion

    }
}
