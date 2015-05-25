namespace Gaia.WebWidgets.Samples.Core.Manager.PageMethods
{
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        [Method]
        public string PageAjaxMethod(string value1, string value2, bool value3)
        {
            /* Notice in this function that we are BOTH returning a value to 
             * the client and setting the Text property of the label. */

            zLabel.Text = 
                "It's quite easy to utilize the Gaia Ajax " + 
                "framework to access the page yourself";
            
            return "Hello " + value1 + 
                   " World " + value2 + 
                   " !! " + value3 + " :)";
        }
    }
}
