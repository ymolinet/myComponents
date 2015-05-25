namespace Gaia.WebWidgets.Samples.Aspects.AspectDraggable.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            zDraggable.Aspects.Add(new Gaia.WebWidgets.AspectDraggable());
        } 
        #endregion
    }
}
