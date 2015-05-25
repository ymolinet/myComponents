namespace Gaia.WebWidgets.Samples.Extensions.Window.OnClosing
{
    using System;
    using System.Linq;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void zWindow_OnClosing(object sender, Gaia.WebWidgets.Extensions.Window.WindowClosingEventArgs e)
        {
            // In the closing event we can override if the Window should be closed or not
            e.ShouldClose = AllDefined(zl1.CssClass, zl2.CssClass, zl3.CssClass);

            if (!e.ShouldClose)
                zWindow.Caption = "All squares were not clicked";
        }

        protected void Label_Clicked(object sender, EventArgs e)
        {
            var label = (Label)sender;
            
            //toggle CssClass to decide whether clicked
            label.CssClass = string.IsNullOrEmpty(label.CssClass) ? "clicked" : "";
        }

        protected void zReOpenWindow_OnClick(object sender, EventArgs e)
        {
            ResetWindow();
            zWindow.Visible = true;
        }

        private void ResetWindow()
        {
            zWindow.Caption = "Force user action";
            zl1.CssClass = zl2.CssClass = zl3.CssClass = string.Empty;
        }

        static bool AllDefined(params string[] input)
        {
            return input.All(s => !string.IsNullOrEmpty(s));
        }
    }
}
