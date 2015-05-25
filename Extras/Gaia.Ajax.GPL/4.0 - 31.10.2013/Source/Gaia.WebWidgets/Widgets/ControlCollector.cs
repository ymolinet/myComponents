/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2009 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

[assembly: System.Web.UI.WebResource("Gaia.WebWidgets.Scripts.ControlCollector.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using System.Web.UI;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This control can be used to select several controls on the client.
    /// The list of controls to enumerate can be filtered using corresponding property.
    /// </summary>
    public class ControlCollector : GaiaControl, IAjaxControl
    {
        /// <summary>
        /// Event argument for the Collected event.
        /// </summary>
        public class CollectedEventArgs : EventArgs
        {
            /// <summary>
            /// Set of the collected controls
            /// </summary>
            public IEnumerable<Control> Controls { get; private set; }

            /// <summary>
            /// Constructor taking the set of the collected controls.
            /// </summary>
            /// <param name="controls">Collected controls enumeration</param>
            public CollectedEventArgs(IEnumerable<Control> controls)
            {
                Controls = controls;
            }
        }

        /// <summary>
        /// The event is raised when at least one control is collected.
        /// </summary>
        public event EventHandler<CollectedEventArgs> Collected;

        /// <summary>
        /// Specifies CSS Selector, which will be used to filter the elements, which can be the source of the bubbling event.
        /// </summary>
        [AjaxSerializable("setESFilter")]
        public string FilterEventSource
        {
            get { return StateUtil.Get(ViewState, "es", string.Empty); }
            set { StateUtil.Set(ViewState, "es", value); }
        }

        /// <summary>
        /// Specifies CSS Selector, which will be used to filter the elements to be collected.
        /// </summary>
        [AjaxSerializable("setFilter")]
        public string Filter
        {
            get { return StateUtil.Get(ViewState, "ts", string.Empty); }
            set { StateUtil.Set(ViewState, "ts", value); }
        }

        /// <summary>
        /// CssClass for the collected controls. This will be applied to each collected control.
        /// </summary>
        [AjaxSerializable("setTCss")]
        public string CssClassCollected
        {
            get { return StateUtil.Get(ViewState, "cs", string.Empty); }
            set { StateUtil.Set(ViewState, "cs", value); }
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup.</param>
        protected override void RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
        {
            using(create.Span(ClientID).SetStyle("display:none")) { }
        }

        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.ControlCollector.js", typeof(ControlCollector), "Gaia.Collector.prototype.destroy");
        }

        string IAjaxControl.GetScript()
        {
            if (Collected == null) return string.Empty;

            return new RegisterControl("Gaia.Collector", ClientID).
                AddPropertyIfTrue(!string.IsNullOrEmpty(Filter), "ts", Filter).
                AddPropertyIfTrue(!string.IsNullOrEmpty(CssClassCollected), "cs", CssClassCollected).
                AddPropertyIfTrue(!string.IsNullOrEmpty(FilterEventSource), "es", FilterEventSource).
                ToString();
        }

        [Method]
        internal void Selected(string selection)
        {
            var list = new List<Control>();
            var keys = selection.Split(new[] {';'});
            Array.ForEach(keys, key =>
                                    {
                                        var uniqueKey = key.Replace("_", "$");
                                        var control = Page.FindControl(uniqueKey);
                                        if (control == null) return;
                                        list.Add(control);
                                    });
            
            if (list.Count == 0) return;
            Collected(this, new CollectedEventArgs(list));
        }
    }
}
