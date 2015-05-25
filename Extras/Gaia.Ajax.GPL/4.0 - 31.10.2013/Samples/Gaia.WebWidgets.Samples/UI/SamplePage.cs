namespace Gaia.WebWidgets.Samples.UI
{
    using System;
    using System.Web.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public class SamplePage : Page
    {
        //public override String StyleSheetTheme
        //{
        //    get { return WebUtility.StyleSheetTheme; }
        //    set { WebUtility.StyleSheetTheme = value; }
        //}

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack) CalendarController.Reset();

            base.OnInit(e);
        }
    }

    /// <summary>
    /// Base class used by effect samples to preload Effect library
    /// </summary>
    public class FxSamplePage : SamplePage
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            
            // If the SamplePage inherit from FxSamplePage, we automatically
            // preload the effects.js file for convenience
            ((IEffect)(new WebWidgets.Effects.EffectAppear())).IncludeScriptFiles();
        }
       
    }
}
