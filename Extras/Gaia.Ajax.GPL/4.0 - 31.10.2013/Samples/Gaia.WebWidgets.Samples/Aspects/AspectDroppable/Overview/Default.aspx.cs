namespace Gaia.WebWidgets.Samples.Aspects.AspectDroppable.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            InitDragDropPassingCustomIdAndHoverClass();
        }

        void InitDragDropPassingCustomIdAndHoverClass()
        {
            var aspectDraggable = new Gaia.WebWidgets.AspectDraggable();

            aspectDraggable.Revert = true;
            aspectDraggable.IdToPass = "CustomID Here";

            zDraggable.Aspects.Add(aspectDraggable);

            var aspectDroppable = new Gaia.WebWidgets.AspectDroppable(AspectDroppableDropped);
            aspectDroppable.HoverClass = "panel-hover";

            zDroppable.Aspects.Add(aspectDroppable);

        }

        void AspectDroppableDropped(object sender, Gaia.WebWidgets.AspectDroppable.DroppedEventArgs e)
        {
            zLabel.Text = e.IdToPass + " Dropped at " + DateTime.Now;
        }

        #endregion
    }
}