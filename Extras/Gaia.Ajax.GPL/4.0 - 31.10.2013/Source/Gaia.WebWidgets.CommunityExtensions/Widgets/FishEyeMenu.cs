/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.ComponentModel;
using System.Web.UI;
using System.Drawing;

namespace Gaia.WebWidgets.CommunityExtensions
{
    /// <summary>
    /// The Gaia Ajax FishEyeMenu is like the dock menu of Mac OS X.
    /// The Gaia Ajax FishEyeMenu is just a normal panel that will find all image elements within it during initialization 
    /// and add up logic to zoom into them as you hover over them. To use it just add up any widget (or HTML elements) that 
    /// renders "img" elements and set your properties. You can easily combine it together with a Repeater, add up 
    /// ImageButtons (if you want to trap clicks of elements in it). You can create Ajax FishEye Menus containing anything you want.
    /// </summary>
    [DefaultProperty("CssClass")]
    [ToolboxData("<{0}:FishEyeMenu runat=\"server\"></{0}:FishEyeMenu>")]
    [ToolboxBitmap(typeof(FishEyeMenu), "Resources.Gaia.WebWidgets.CommunityExtensions.FishEye.bmp")]
    public class FishEyeMenu : Panel, IAjaxControl
    {
        #region [ -- Properties -- ]
        /// <summary>
        /// What size the images are at default when not hovered over
        /// </summary>
        [DefaultValue(55)]
        public int StartSize
        {
            get
            {
                if (ViewState["StartSize"] == null)
                    return 55;
                return (int)ViewState["StartSize"];   
            }
            set
            {
                ViewState["StartSize"] = value;
            }
        }

        /// <summary>
        /// Maximum size the images will resize to
        /// </summary>
        [DefaultValue(88)]
        public int EndSize
        {
            get
            {
                if (ViewState["EndSize"] == null)
                    return 88;
                return (int)ViewState["EndSize"];
            }
            set
            {
                ViewState["EndSize"] = value;
            }
        }

        /// <summary>
        /// At what distance the images will start blowing up in size
        /// </summary>
        [DefaultValue(300)]
        public int Threshold
        {
            get
            {
                if (ViewState["Threshold"] == null)
                    return 300;
                return (int)ViewState["Threshold"];
            }
            set
            {
                ViewState["Threshold"] = value;
            }
        }
        #endregion

        #region [ -- Overridden Base class methods -- ]

        /// <summary>
        /// Include FishEyeMenu Javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // Include FishEyeMenu Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.prototype.js", typeof(FishEyeMenu), "");
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.FishEyeMenu.js", typeof(FishEyeMenu), "Gaia.Extensions.FishEyeMenu.browserFinishedLoading");
        }

        #endregion

        #region [ -- Overridden IAjaxControl methods -- ]

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Extensions.FishEyeMenu", ClientID)
                .AddProperty("startSize", StartSize, false)
                .AddProperty("endSize", EndSize, false)
                .AddProperty("threshold", Threshold, false)
                .AddProperty("enabled", Enabled)
                .AddAspects(Aspects).ToString();
        }

        #endregion
    }
}
