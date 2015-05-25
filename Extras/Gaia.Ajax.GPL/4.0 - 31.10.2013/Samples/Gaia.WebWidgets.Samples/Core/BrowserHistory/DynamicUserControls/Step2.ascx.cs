using System;

namespace Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls
{
    public partial class Step2 : DynamicUserControlEntryBase
    {
        protected void zGoBack_Click(object sender, EventArgs e)
        {
            NavigateTo<Step1>();
        }

        protected void zNextStep_Click(object sender, EventArgs e)
        {
            NavigateTo<Step3>();
        }
    }
}