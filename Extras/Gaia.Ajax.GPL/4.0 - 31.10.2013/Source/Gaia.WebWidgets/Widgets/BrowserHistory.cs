/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

[assembly: System.Web.UI.WebResource(Gaia.WebWidgets.BrowserHistory.ScriptFile, "text/javascript")]
[assembly: System.Web.UI.WebResource("Gaia.WebWidgets.LibraryScripts.jquery.history.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using HtmlFormatting;

    /// <summary>
    /// The BrowserHistory Control allows you to add states to your application by using the
    /// AddHistory function. When the user clicks back in the browser, the state info will be passed
    /// to the server in the Navigated event. This is an Ajax request where you can restore the
    /// state of your application based on this information. 
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for BrowserHistory" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Core\BrowserHistory\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for BrowserHistory" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Core\BrowserHistory\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    [NonVisualControl]
    [DefaultEvent("Navigated")]
    [ToolboxItem(false)]
    public class BrowserHistory : GaiaControl, IAjaxControl
    {
        #region [ -- Private Members and Consts -- ]

        internal const string ScriptFile = "Gaia.WebWidgets.Scripts.BrowserHistory.js";
        internal const string EmptyPage = "Gaia.WebWidgets.Resources.blank.html";

        #endregion

        #region [ -- EventArgs -- ]

        /// <summary>
        /// EventArgs used when the Navigated event fires. Here you will get access to the
        /// Token that was used as a bookmark in the browser history. From that you can 
        /// restore state. 
        /// </summary>
        public sealed class BrowserHistoryEventArgs : EventArgs
        {
            private readonly string _token;

            internal BrowserHistoryEventArgs(string token)
            {
                _token = token;
            }

            /// <summary>
            /// The Token is the bookmark/hash (#) value in the browser location field. 
            /// Use this token to restore state in your application. 
            /// </summary>
            public string Token
            {
                get { return _token; }
            }
        }

        #endregion

        #region [ -- Events -- ]

        /// <summary>
        /// The Navigated Event is fired when the user clicks the back/forward button
        /// in the browser and ends up with a bookmarked page. A bookmarked page uses the
        /// # (hash) symbol + an identifier that will be passed into the EventArgs. 
        /// You can reset the state of the application based on this identifier. 
        /// </summary>
        public event EventHandler<BrowserHistoryEventArgs> Navigated; 

        #endregion

        /// <summary>
        /// Default constructor for <see cref="BrowserHistory"/> instances.
        /// </summary>
        public BrowserHistory()
        {
            TokenProxy = string.Empty;
        }

        #region [ -- Add entry to the history code -- ]

        [AjaxSerializable("addToken")]
        internal string TokenProxy { get; private set; }

        [AjaxSerializable("go")]
        internal int GoProxy { get; private set; }

        /// <summary>
        /// Adds an entry to the history of the browser allowing the user to navigate back/forward
        /// between different *cue* points. The string token will be passed back in the Navigated
        /// event when the user performs custom navigation. 
        /// </summary>
        /// <param name="token">The identifier that will be set as a bookmark</param>
        public void AddHistory(string token) { TokenProxy = token; }
        
        /// <summary>
        /// Go back in the browser history one step;
        /// </summary>
        public void GoBack() { --GoProxy; }

        /// <summary>
        /// Go one step forward in the browser history
        /// </summary>
        public void GoForward() { ++GoProxy; }

        /// <summary>
        /// Go back/forward in the browser history based on an integer value. +1 signalizes
        /// one step forward, whereas -2 signalizes two steps backwards in the browser history 
        /// </summary>
        /// <param name="value"></param>
        public void Go(int value) { GoProxy = value;}

        #endregion

        #region [ -- Helper Methods -- ]

        [Method]
        internal void OnNavigatedMethod(string token)
        {
            if (Navigated != null)
                Navigated(this, new BrowserHistoryEventArgs(token));
        } 

        #endregion

        #region [ -- Overriden base class methods -- ]

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.jquery.history.js", typeof(BrowserHistory), "", /* part of core js package */ true);
            Manager.Instance.AddInclusionOfFileFromResource(ScriptFile, typeof(BrowserHistory), "Gaia.BrowserHistory.browserFinishedLoading", /* part of core js package */ true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            // In DesignMode we render the ClientID so that when pushed the default event
            // is wired up in the codebehind file. 
            if (DesignMode)
            {
                create.GetHtmlTextWriter().Write("<div>[{0}]</div>", ClientID);
            }
            else
            {
                using (create.Div(ClientID).SetStyle("display: none;").WriteContent(""))
                    RenderChildren(create.GetHtmlTextWriter());
            }
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.BrowserHistory", ClientID)
                .AddPropertyIfTrue(!String.IsNullOrEmpty(TokenProxy), "inittoken", TokenProxy)
                .ToString();
        }

        #endregion

    }
}
