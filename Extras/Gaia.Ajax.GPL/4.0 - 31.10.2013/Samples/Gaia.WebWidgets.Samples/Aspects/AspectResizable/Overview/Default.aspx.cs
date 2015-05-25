
namespace Gaia.WebWidgets.Samples.Aspects.AspectResizable.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlResizable.Aspects.Add(new Gaia.WebWidgets.AspectResizable(pnlResizable_Resized));
        }

        void pnlResizable_Resized(object sender, EventArgs e)
        {
            lblMsg.Text = string.Format("Panel was resized. Width: {0} Height: {1}", pnlResizable.Width,
                                        pnlResizable.Height);
        } 
        #endregion

       
    }
}
