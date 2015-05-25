namespace Gaia.WebWidgets.Samples.Aspects.AspectResizable.SpecificBorders
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            InitAspectResizableWithSpecificBorders();
        }

        private void InitAspectResizableWithSpecificBorders()
        {
            var aspectResizable =
                new Gaia.WebWidgets.AspectResizable();

            aspectResizable.Mode =
                Gaia.WebWidgets.AspectResizable.ResizeModes.BottomBorder |
                Gaia.WebWidgets.AspectResizable.ResizeModes.RightBorder;

            zPanel.Aspects.Add(aspectResizable);
        } 
        #endregion

    }
}
