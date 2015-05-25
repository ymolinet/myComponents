using System;

namespace Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls
{
    public partial class Step3 : DynamicUserControlEntryBase
    {
        protected void zNextStep_Click(object sender, EventArgs e)
        {
            NavigateTo<Step4>();
        }
    }
}