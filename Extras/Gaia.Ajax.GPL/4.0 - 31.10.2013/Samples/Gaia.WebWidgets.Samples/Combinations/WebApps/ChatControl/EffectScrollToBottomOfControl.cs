namespace Gaia.WebWidgets.Samples.Combinations.WebApps.ChatControl
{
    /// <summary>
    /// This is a custom effect which scrolls to bottom of an element, like a Gaia Panel
    /// </summary>
    public class EffectScrollToBottomOfControl : Effect, IEffect
    {
        public string GetScript()
        {
            return string.Format("var x = jQuery('#'+{0});x.scrollTop(x[0].scrollHeight);", GetElementReference());
        }
    }
}
