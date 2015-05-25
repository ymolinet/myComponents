namespace Gaia.WebWidgets.Samples.Aspects.AspectHoverable.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            InitAspectHoverable();
        }

        private void InitAspectHoverable()
        {
            Gaia.WebWidgets.AspectHoverable aspectHoverable =
                new Gaia.WebWidgets.AspectHoverable(
                                    aspectHoverable_MouseOver,
                                    aspectHoverable_MouseOut);

            zPanel.Aspects.Add(aspectHoverable);
        }

        void aspectHoverable_MouseOut(object sender, EventArgs e)
        {
            zPanel.CssClass = "panel";
            zMessage.Text = string.Empty;
        }

        void aspectHoverable_MouseOver(object sender,
            Gaia.WebWidgets.AspectHoverable.HoverEventArgs e)
        {
            zPanel.CssClass = "panel-hovered";
            zMessage.Text = string.Format("Hovered at {0}:{1}px", e.Left, e.Top);
        }  
        #endregion
    }
}
