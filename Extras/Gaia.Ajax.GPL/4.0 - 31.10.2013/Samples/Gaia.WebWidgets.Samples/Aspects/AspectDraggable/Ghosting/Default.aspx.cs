namespace Gaia.WebWidgets.Samples.Aspects.AspectDraggable.Ghosting
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
            SetupFirstImageForGhosting();
            SetupSecondImageForGhosting();
            SetupThirdImageForGhosting();
        }

        #region Code
        private void SetupThirdImageForGhosting()
        {
            zGhostImage3.ImageUrl = Images[2].ThumbUrl;
            zGhostImage3.AlternateText = Images[2].Text;
            var aspectDraggable = new Gaia.WebWidgets.AspectDraggable();
            aspectDraggable.Dropped +=
                delegate
                {
                    zGhostLabel3.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight(Color.Black));
                };
            aspectDraggable.MakeGhost = true;
            aspectDraggable.UseDocumentBody = true;
            aspectDraggable.Revert = false;
            aspectDraggable.DeepCopy = true;

            zGhostImage3.Aspects.Add(aspectDraggable);
        }

        private void SetupFirstImageForGhosting()
        {
            zGhostImage1.ImageUrl = Images[0].ThumbUrl;
            zGhostImage1.AlternateText = Images[0].Text;
            var aspectDraggable = new Gaia.WebWidgets.AspectDraggable();
            aspectDraggable.Dropped +=
                delegate
                {
                    zGhostLabel1.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight(Color.Black));
                };

            aspectDraggable.MakeGhost = true;
            aspectDraggable.DeepCopy = true;
            aspectDraggable.UseDocumentBody = true;
            aspectDraggable.Revert = true;
            aspectDraggable.DragCssClass = "img-drag";
            zGhostImage1.Aspects.Add(aspectDraggable);
        }

        private void SetupSecondImageForGhosting()
        {
            zGhostImage2.ImageUrl = Images[1].ThumbUrl;
            zGhostImage2.AlternateText = Images[1].Text;

            var aspectDraggable = new Gaia.WebWidgets.AspectDraggable();
            aspectDraggable.Dropped +=
                delegate
                {
                    zGhostLabel2.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight(Color.Black));
                };
            aspectDraggable.MakeGhost = true;
            aspectDraggable.DeepCopy = false;
            aspectDraggable.UseDocumentBody = true;
            aspectDraggable.Revert = true;
            aspectDraggable.DragCssClass = "img-drag";

            zGhostPanel.Aspects.Add(aspectDraggable);
        } 
        #endregion

        private static List<MediaImage> Images
        {
            get { return new List<MediaImage>(MediaUtility.GetImageFiles("scenery")); }
        }
    }
}
