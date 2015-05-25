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
using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Base class for hybrid controls, ie. composite container controls.
    /// </summary>
    /// <remarks>
    /// Base class for controls which are <see cref="IAjaxContainerControl"/> and also support composition pattern.
    /// </remarks>
    /// <example>
    /// A typical example is the ExtendedPanel, which is a container like a <see cref="Panel"/>, but has additional
    /// child controls, such as minimize and restore buttons, which are added using composition pattern.
    /// </example>
    public abstract class HybridPanelBase : Panel, IAjaxContainerControl
    {
        #region [ -- Private members -- ]

        private string _bodyid;
        private bool _enforceComposition;
        private bool _compositionControlsCreated;
        private HybridControlCollection<Control> _childControls;
        private readonly List<Control> _compositionControls = new List<Control>();

        #endregion

        /// <summary>
        /// Gets a <see cref="T:System.Web.UI.ControlCollection"/> object that represents the child controls for a specified server control in the UI hierarchy.
        /// </summary>
        /// <returns>
        /// The collection of child controls for the specified server control.
        /// </returns>
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        /// <summary>
        /// Returns true if the hybrid control's composition controls have been created. Otherwise, false.
        /// </summary>
        internal protected bool CompositionControlsCreated
        {
            get { return _compositionControlsCreated; }
            set
            {
                if (_compositionControlsCreated && !value)
                {
                    _compositionControls.ForEach(base.Controls.Remove);
                    _compositionControls.Clear();
                }

                _compositionControlsCreated = value;
            }
        }

        /// <summary>
        /// Gets the ID of the DOM element into which <see cref="ChildControls"/> are rendered.
        /// </summary>
        protected string BodyID
        {
            get { return _bodyid ?? (_bodyid = string.Concat(ClientID, "_content")); }
        }

        /// <summary>
        /// Returns the <see cref="ICollection{Control}"/> into which composition controls are added.
        /// </summary>
        protected internal ICollection<Control> CompositionControls
        {
            get { return _compositionControls; }
        }

        /// <summary>
        /// Returns <see cref="ControlCollection"/> for contained child controls, except composition controls.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public virtual HybridControlCollection<Control> ChildControls
        {
            get
            {
                EnsureChildControls();
                return _childControls ?? (_childControls = new HybridControlCollection<Control>(this, CompositionControls));
            }
        }

        /// <summary>
        /// Called by the framework to notify server controls that use composition-based implementation 
        /// to create any child composition controls they contain in preparation for posting back or rendering.
        /// </summary>
        /// <remarks>
        /// Override in derived classes to add composition controls to the <see cref="CompositionControls"/> collection.
        /// </remarks>
        protected virtual void CreateCompositionControls() { }

        /// <summary>
        /// Used by inheritors to ensure that composite controls are ready for usage
        /// </summary>
        internal protected void EnsureCompositionControls()
        {
            // skip composition controls addition in DesignMode until explicitly enforced
            if (CompositionControlsCreated || (DesignMode && !_enforceComposition)) return;
            CompositionControlsCreated = true;

            CreateCompositionControls();

            var controls = base.Controls;
            for(var index = _compositionControls.Count - 1; index >= 0; --index)
                controls.AddAt(0, _compositionControls[index]);
        }

        /// <summary>
        /// Clears composition controls collection
        /// and removes composition controls from the controls collection.
        /// To be used by inheritors in pair with EnsureCompositionControl()
        /// </summary>
        [Obsolete("ClearCompositionControls() method is deprecated. Use CompositionControlsCreated property instead.", true)]
        protected void ClearCompositionControls()
        {
            CompositionControlsCreated = false;
        }

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Determines whether the server control contains child controls. If it does not, it creates child controls.
        /// </summary>
        protected override void EnsureChildControls()
        {
            EnsureCompositionControls();
            base.EnsureChildControls();
        }

        /// <summary>
        /// Raises the <see cref="Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            EnsureChildControls();
            base.OnLoad(e);
        }

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls.
        /// </summary>
        public override void DataBind()
        {
            OnDataBinding(EventArgs.Empty);
            EnsureChildControls();
            DataBindChildren();
        }

        /// <summary>
        /// See <see cref="AjaxControl.SaveViewState" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <returns>the saved ViewState</returns>
        protected override object SaveViewState()
        {
            EnsureChildControls();
            return base.SaveViewState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            // enforce composition in DesignMode
            _enforceComposition = DesignMode;

            EnsureChildControls();
            base.RenderControl(writer);
            
            _enforceComposition = false;
        }

        /// <summary>
        /// Outputs the content of a server control's children to a provided <paramref name="writer"/> object, 
        /// which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="HtmlTextWriter"/> object which receives the rendered content.</param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            foreach (var control in ChildControls)
                control.RenderControl(writer);
        }

        #endregion

        #region [ -- IAjaxContainerControl implementation -- ]

        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return BodyID;
        }

        #endregion
    }
}

