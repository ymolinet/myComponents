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
using System.Drawing;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;
using Gaia.WebWidgets.HtmlFormatting;

[assembly: WebResource("Gaia.WebWidgets.Scripts.Timer.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// This is an <em>Ajax Timer Control</em>. The Gaia Ajax Timer will poll the server raising an event handler
    /// every n'th second according to which interval you have configured it to do so. It can be disabled and enabled
    /// from the server and from the Event Handler you can do whatever you wish with other controls on the page. Including
    /// creating new controls and update existing ones.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for Timer" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\Timer\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for Timer" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\Timer\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    [DefaultEvent("Tick")]
    [ToolboxData("<{0}:Timer runat=\"server\" />")]
    [ToolboxBitmap(typeof(Timer), "Resources.Gaia.WebWidgets.Timer.bmp")]
    [Designer("Gaia.WebWidgets.Design.TimerDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaWebControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class Timer : GaiaControl, IAjaxControl, IPostBackEventHandler
    {
        #region [ -- Properties and Events -- ]
        
        /// <summary>
        /// Raised when the specified amount of time is ellapsed.
        /// </summary>
        [Category("Action")]
        [Description("Event handler raised when the Milliseconds is elapsed.")]
        public event EventHandler Tick;

        /// <summary>
        /// Gets or sets if the <see cref="Timer"/> should tick and raise events.
        /// </summary>
        [DefaultValue(true)]
        [AjaxSerializable("setEnabled")]
        [Category("Behavior")]
        [Description("Specifies if the Timer should tick and raise events.")]
        public bool Enabled
        {
            get { return StateUtil.Get(ViewState, "Enabled", true); }
            set { StateUtil.Set(ViewState, "Enabled", value, true); }
        }

        /// <summary>
        /// Gets or sets the time frame after which <see cref="Tick"/> event is raised.
        /// </summary>
        [DefaultValue(1000)]
        [Category("Behavior")]
        [AjaxSerializable("setMilliseconds")]
        [Description("Milliseconds between every execution of the timer.")]
        public int Milliseconds
        {
            get { return StateUtil.Get(ViewState, "Milliseconds", 1000); }
            set { StateUtil.Set(ViewState, "Milliseconds", value, 1000); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
        /// </summary>
        /// <returns>
        /// true if the control is visible on the page; otherwise false.
        /// </returns>
        public override bool Visible
        {
            get { return base.Visible; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets if the timer should be reset.
        /// </summary>
        [AjaxSerializable("setReset")]
        internal bool ResetInternal { get; set; }

        /// <summary>
        /// Will reset an active timer. If the timer is Enabled it will continue to Tick, but it will start from the beginning again.
        /// This will be useful if you rely on a regular callback, but if a callback happens due to "other" events it makes no sense
        /// to have the extra roundtrip to the server
        /// </summary>
        public void Reset()
        {
            ResetInternal = true;   
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Include Timer Javascript stuff
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Timer.js", typeof(Manager), "Gaia.Timer.loaded", true);
        }

        /// <summary>
        /// Overridden to provide design time support for timer
        /// </summary>
        /// <param name="create">XHtmlTagFactory for markup rendering</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (create.Span(ClientID).SetStyle("display:none;visibility:hidden")) { }
        }

        #endregion

        #region [ -- IPostBackEventHandler implementation -- ]

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">
        /// A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler.
        /// </param>
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            if (Tick == null) return;
            Tick(this, EventArgs.Empty);
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        string IAjaxControl.GetScript()
        {
            var callbackName = Utilities.GetCallbackName(this);

            return new RegisterControl("Gaia.Timer", ClientID)
                .AddProperty("milliseconds", Milliseconds)
                .AddPropertyIfTrue(callbackName != null, "callbackName", callbackName)
                .InvokeIf(Tick != null && Enabled, "setEnabled", false, 1)
                .ToString();
        }

        #endregion
    }
}