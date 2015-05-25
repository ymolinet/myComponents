namespace Gaia.WebWidgets.Samples.Core.BrowserHistory.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    
    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BookMarkState();
        }

        void BookMarkState()
        {
            zHistory.AddHistory(zComedian.SelectedItem.Value);
        }

        protected void zComedian_SelectedIndexChanged(object sender, EventArgs e)
        {
            BookMarkState();
            zInfo.Visible = true;
        }

        protected void zHistory_Navigated(object sender,
            Gaia.WebWidgets.BrowserHistory.BrowserHistoryEventArgs e)
        {
            zComedian.SelectedValue = e.Token;
        } 
        #endregion
    }
}
