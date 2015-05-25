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
using System.Drawing;
using System.Web.UI;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectModal.js", "text/javascript")]
namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect for making Widgets Modal by obscuring the entire surface on the screen. If you attach this to a widget and
    /// you make the widget visible during a callback then only that widget (and widgets with higher z-index)
    /// will be "accessible" to be clicked by the end user since all other widgets will be "obscured" by a DIV 
    /// which will fill up the entire viewport of the browser.
    /// <p>Modality is something that previously only was available when developing Windows Forms applications, but now with Gaia Ajax 
    /// it's made available as an Aspect which you can attach to almost all Gaia Controls via the Aspects collection </p>
    /// <br /> <p>It can be customized with a Color and Opacity by using an overloaded ctor </p>
    /// <p>The Window has the Modal property which is automatically adding the Aspect for you if set to true. </p>
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectModal" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectModal\Overview\Default.aspx"  />
    /// </code> 
    /// <code title="Adding AspectModal in CodeBehind" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectModal\Overview\Default.aspx.cs" />
    /// </code>
    /// </example>
    public class AspectModal : Aspect<AspectModal>, IAspect
    {
        #region [ -- Private members --]

        private Color _color;
        private readonly double _opacity;
     
        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor
        /// </summary>
        public AspectModal()
            : this(Color.FromArgb(200, 200, 255))
        { }

        /// <summary>
        /// CTOR taking Color argument for the obscurer DIV
        /// </summary>
        /// <param name="color">Background Color of the obscurer DIV</param>
        public AspectModal(Color color)
            : this(color, 0.5)
        { }

        /// <summary>
        /// CTOR taking Color argument for the obscurer DIV in addition to opacity
        /// </summary>
        /// <param name="color">Background Color of the obscurer DIV</param>
        /// <param name="opacity">Opacity of obscurer DIV 0 == invisible, 1 == fully visible</param>
        public AspectModal(Color color, double opacity)
        {
            if (opacity < 0D || opacity > 1D)
                throw new ArgumentException("Cannot have opacity being less than 0 or more than 1");
            _color = color;
            _opacity = opacity;
        }

        #endregion

        #region [ -- IAspect Implmenetation -- ]

        string IAspect.GetScript()
        {
            string color = _color.IsNamedColor ? _color.Name : "#" + _color.Name.Substring(2);

            return new RegisterAspect("Gaia.AspectModal", ParentControl.Control.ClientID)
                .AddProperty("color", color)
                .AddProperty("opacity", _opacity.ToString(System.Globalization.NumberFormatInfo.InvariantInfo))
                .ToString();
        }

       

        #endregion


        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            
            // for BringElementToFront logic
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.WebControl.js", typeof(Manager), "Gaia_WebControl_browserFinishedLoading", true);

            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectModal.js", typeof(Manager), "Gaia.AspectModal.browserFinishedLoading", true);
        }


    }
}
