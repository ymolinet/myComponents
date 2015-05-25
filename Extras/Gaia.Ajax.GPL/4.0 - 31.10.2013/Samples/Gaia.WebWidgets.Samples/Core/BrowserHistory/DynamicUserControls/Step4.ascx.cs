using System;

namespace Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls
{
    public partial class Step4 : DynamicUserControlEntryBase
    {
        protected void zStartOver_Click(object sender, EventArgs e)
        {
            NavigateTo<Step1>();
        }
    }
}