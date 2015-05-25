/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Web.UI;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectUpdateControl.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect for specializing UpdateControls for specific widgets. If some widget needs special UpdateControl 
    /// capabilities then use this one. The existence of this aspect will override the 
    /// Manager.Instance.UpdateControl property but only for the specific widget owning this Aspect. 
    /// <br />
    /// This is a useful feature when you only have a few time consuming functions and/or you want to display a local update control 
    /// near that particular control.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectUpdateControl" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectUpdateControl\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for AspectUpdateControl" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectUpdateControl\Overview\Default.aspx.cs" region="Code" />
    /// </code>
    /// </example>
    public class AspectUpdateControl : Aspect<AspectUpdateControl>, IAspect
    {
       private string _updateControlId;
        private Control _updateControl;

        #region [ -- Properties -- ]

        /// <summary>
        /// ID of HTML DOM element to use as the UpdateControl
        /// </summary>
        public string UpdateControlId
        {
            get { return _updateControlId; }
            set { _updateControlId = value; }
        }

        /// <summary>
        /// Control to use as the UpdateControl
        /// </summary>
        public Control UpdateControl
        {
            get { return _updateControl; }
            set { _updateControl = value; }
        }

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor
        /// </summary>
        public AspectUpdateControl()
            : this((string)null)
        { }

        /// <summary>
        /// Constructor taking id of HTML element to use as UpdateControl
        /// </summary>
        /// <param name="updateControlId">Id of control to use as UpdateControl for widget</param>
        public AspectUpdateControl(string updateControlId)
        {
            _updateControlId = updateControlId;
        }

        /// <summary>
        /// Constructor taking Control to use as UpdateControl
        /// </summary>
        /// <param name="updateControl">UpdateControl for widget</param>
        public AspectUpdateControl(Control updateControl)
        {
            _updateControl = updateControl;
        }

        #endregion

        #region [ -- IAspect Implementation -- ]
        
        string IAspect.GetScript()
        {
            return new RegisterAspect("Gaia.AspectUpdateControl", ParentControl.Control.ClientID)
                .AddProperty("updateControl", 
                    _updateControl == null ? _updateControlId : _updateControl.ClientID)
                .ToString();
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectUpdateControl.js", typeof(Manager), "Gaia.AspectUpdateControl.browserFinishedLoading", true);
        }

        #endregion

    }
}
