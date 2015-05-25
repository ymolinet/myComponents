using System;

namespace Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls
{
    public partial class Step1 : DynamicUserControlEntryBase
    {
        protected void zNextStep_Click(object sender, EventArgs e)
        {
            NavigateTo<Step2>();
        }
    }
}