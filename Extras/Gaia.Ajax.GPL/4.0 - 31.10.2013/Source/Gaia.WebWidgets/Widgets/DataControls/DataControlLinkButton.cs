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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Widgets.DataControls
{
    [System.ComponentModel.ToolboxItem(false)]
    class DataControlLinkButton : LinkButton, IAjaxControl
    {
        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="DataControlLinkButton"/>.
        /// </summary>
        public class PropertyStateManagerDataControlLinkButton : PropertyStateManagerButtonControl<LinkButton>
        {
            private readonly DataControlLinkButton _linkButton;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerButtonControl{T}"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerDataControlLinkButton(DataControlLinkButton control) : this(control, control.ClientID, null) { }

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerButtonControl{T}"/> for specified <paramref name="control"/>
            /// using specified <paramref name="clientId"/> for reference and specified <see cref="IExtraPropertyCallbackRenderer"/>
            /// for additional state change serialization.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <param name="clientId">The client-side ID of the <paramref name="control"/> to use.</param>
            /// <param name="extra">Provides additional state change rendering during callbacks.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerDataControlLinkButton(DataControlLinkButton control, string clientId, IExtraPropertyCallbackRenderer extra)
                : base(control, clientId, extra)
            {
                _linkButton = control;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                base.TakeSnapshot();
                OverrideForeColor();
            }

            private void OverrideForeColor()
            {
                if (_linkButton.ForeColorSet) return;
                
                Control parent = _linkButton;
                var depth = _linkButton.Depth + 1;
                for (var i = 0; i < depth; ++i)
                {
                    parent = parent.Parent;
                    var parentAjaxControl = parent as IAjaxControl;
                    if (parentAjaxControl == null) continue;

                    var stateManagerWebControl = parentAjaxControl.StateManager as PropertyStateManagerWebControl;
                    if (stateManagerWebControl == null) continue;

                    var foreColor = stateManagerWebControl.ForeColor;
                    if (foreColor == Color.Empty) continue;

                    ForeColor = foreColor;
                    return;
                }
            }
        }

        #endregion

        private readonly ASP.IPostBackContainer _container;

        /// <summary>
        /// Initializes new instance of <see cref="DataControlLinkButton"/>.
        /// </summary>
        /// <param name="container">Button container.</param>
        public DataControlLinkButton(ASP.IPostBackContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Returns true if the <see cref="ForeColor"/> property was set.
        /// </summary>
        internal bool ForeColorSet { get; private set; }

        /// <summary>
        /// Returns depth of the <see cref="DataControlLinkButton"/> in the container.
        /// </summary>
        protected internal virtual int Depth
        {
            get { return 3; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the <see cref="T:System.Web.UI.WebControls.LinkButton"/> control is clicked.
        /// </summary>
        /// <returns>
        /// True if validation is performed when the <see cref="T:System.Web.UI.WebControls.LinkButton"/> control is clicked; otherwise, false. The default value is true.
        /// </returns>
        public override bool CausesValidation
        {
            get { return _container == null && base.CausesValidation; }
            set
            {
                ThrowIfNotSupported();
                base.CausesValidation = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color (typically the color of the text) of the Web server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Drawing.Color"/> that represents the foreground color of the control. The default is <see cref="F:System.Drawing.Color.Empty"/>.
        /// </returns>
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                ForeColorSet = true;
                base.ForeColor = value;
            }
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.UI.PostBackOptions"/> object that represents the <see cref="T:System.Web.UI.WebControls.LinkButton"/> control's postback behavior.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.PostBackOptions"/> that represents the <see cref="T:System.Web.UI.WebControls.LinkButton"/> control's postback behavior.
        /// </returns>
        protected override PostBackOptions GetPostBackOptions()
        {
            return _container != null ? _container.GetPostBackOptions(this) : base.GetPostBackOptions();
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and 
        /// stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            OverrideForeColor();
            base.RenderControl(writer);
        }

        private void OverrideForeColor()
        {
            if (ForeColorSet) return;

            Control parent = this;
            for (var i = 0; i < Depth; ++i)
            {
                parent = parent.Parent;
                
                var foreColor = ((ASP.WebControl)parent).ForeColor;
                if (foreColor == Color.Empty) continue;
                
                ForeColor = foreColor;
                return;
            }
        }

        private void ThrowIfNotSupported()
        {
            if (_container != null) throw new NotSupportedException();
        }

        #region [ -- IAjaxControl Implementation -- ]

        /// <summary>
        /// Called by the Gaia framework when the StateManager of the Control needs to be initialized
        /// </summary>
        /// <returns>StateManager for the Control</returns>
        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerDataControlLinkButton(this);
        }

        #endregion
    }
}
