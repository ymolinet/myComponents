using System.Drawing;

namespace Gaia.WebWidgets.Samples.Aspects.AspectResizable.MaxHeightWidth
{
    using Gaia.WebWidgets.Samples.UI;
    using System;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            InitAspectResizableWithMaxHeightWidth();
            InitAspectResizableWithMaxHeightWidthAndSnap();
            InitAspectResizableWithMaxHeightWidthAndBounds();
        }

        private void InitAspectResizableWithMaxHeightWidthAndBounds()
        {
            Gaia.WebWidgets.AspectResizable aspectResizable
                = new Gaia.WebWidgets.AspectResizable();

            aspectResizable.MinWidth = 50;
            aspectResizable.MinHeight = 50;
            aspectResizable.BoundingRectangle = new Rectangle(-50, -50, 200, 200);

            zPanelBound.Aspects.Add(aspectResizable);
        }

        private void InitAspectResizableWithMaxHeightWidth()
        {
            Gaia.WebWidgets.AspectResizable aspectResizable
                = new Gaia.WebWidgets.AspectResizable();

            aspectResizable.MaxWidth = 250;
            aspectResizable.MaxHeight = 250;

            zPanel.Aspects.Add(aspectResizable);
        }

        private void InitAspectResizableWithMaxHeightWidthAndSnap()
        {
            Gaia.WebWidgets.AspectResizable aspectResizable
                = new Gaia.WebWidgets.AspectResizable();

            aspectResizable.Snap.Delta = new Point(10, 10);
            aspectResizable.MaxWidth = 250;
            aspectResizable.MaxHeight = 250;

            zPanelSnap.Aspects.Add(aspectResizable);
        }  
        #endregion
    }
}
