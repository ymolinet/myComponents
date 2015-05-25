/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using ASP = System.Web.UI;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// This is the worker class used by Aspectable Gaia Controls. It's primary responsibility is to handle Aspects.
    /// Usage is to implement IAjaxControl and forward every method in that interface to this class.
    /// </summary>
    /// <seealso cref="IAjaxContainerControl"/>
    /// <seealso cref="AjaxContainerControl"/>
    /// <seealso cref="AjaxControl"/>
    /// <seealso cref="IAjaxControl"/>
    public class AspectableAjaxControl : AjaxControl
    {
        #region [ -- Private Members -- ]

        private AspectCollection _aspects;
        private readonly IAspectableAjaxControl _aspectableAjaxControl;

        #endregion

        #region [ -- Properties -- ]
        
        /// <summary>
        /// <see cref="IAspectableAjaxControl.Aspects"></see> for documentation for this method
        /// </summary>
        public AspectCollection Aspects
        {
            get { return _aspects ?? (_aspects = new AspectCollection(_aspectableAjaxControl)); }
        }
        
        #endregion

        #region [ -- Constructors -- ]
        
        /// <summary>
        /// Creates new instance of <see cref="AspectableAjaxControl"/> class.
        /// </summary>
        /// <param name="control">Control to associate with.</param>
        public AspectableAjaxControl(IAspectableAjaxControl control) : base(control)
        {
            _aspectableAjaxControl = control;
        }
        
        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// PreRender override to make sure we add up the JavaScript files for the aspects in the Aspects 
        /// list of the control.
        /// </summary>
        public override void OnPreRender()
        {
            base.OnPreRender();

            // Rendering JavaScript inclusion files for Aspects
            foreach (var aspect in Aspects)
                aspect.IncludeScriptFiles();
        }

        #endregion
    }
}
