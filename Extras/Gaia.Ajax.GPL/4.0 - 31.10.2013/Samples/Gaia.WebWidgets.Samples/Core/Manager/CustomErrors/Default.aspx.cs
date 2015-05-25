namespace Gaia.WebWidgets.Samples.Core.Manager.CustomErrors
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Gaia.WebWidgets.Manager.Instance.ErrorHandler = "errorHandler";
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            const string msg = "This exception will be handled on the client";
            throw new Exception(msg);
        } 
        #endregion
    }
}
