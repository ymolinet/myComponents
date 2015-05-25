namespace Gaia.WebWidgets.Samples.Aspects.AspectDraggable.Snapping
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupFirstImageForSnapping();
            SetupSecondImageForSnapping();
            SetupThirdImageForSnapping();
        }

        #region Code
        private void SetupFirstImageForSnapping()
        {
            zSnapImage1.ImageUrl = Images[0].ThumbUrl;
            zSnapImage1.AlternateText = Images[0].Text;
            var aspectDraggable = new Gaia.WebWidgets.AspectDraggable();
            aspectDraggable.Revert = true;
            aspectDraggable.Snap.Delta = new Point(40, 40);
            zSnapImage1.Aspects.Add(aspectDraggable);
        }

        private void SetupSecondImageForSnapping()
        {
            zSnapImage2.ImageUrl = Images[0].ThumbUrl;
            zSnapImage2.AlternateText = Images[0].Text;
            var aspectDraggable = new Gaia.WebWidgets.AspectDraggable();
            aspectDraggable.Revert = true;
            aspectDraggable.Snap.ClientFunctionName = "snapper";
            zSnapImage2.Aspects.Add(aspectDraggable);
        }

        private void SetupThirdImageForSnapping()
        {
            zSnapImage3.ImageUrl = Images[0].ThumbUrl;
            zSnapImage3.AlternateText = Images[0].Text;
            var aspectDraggable = new Gaia.WebWidgets.AspectDraggable();
            aspectDraggable.Snap.Delta = new Point(10, 10);
            aspectDraggable.Rectangle = new Rectangle(0, 0, 50, 50);
            zSnapImage3.Aspects.Add(aspectDraggable);
        } 
        #endregion

        private static List<MediaImage> Images
        {
            get { return new List<MediaImage>(MediaUtility.GetImageFiles("scenery")); }
        }
    }
}
