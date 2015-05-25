namespace Gaia.WebWidgets.Samples.Aspects.AspectMouseMove.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default :SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            InitMouseMoveSimple();
        }

        private void InitMouseMoveSimple()
        {
            Gaia.WebWidgets.AspectMouseMove aspectMouseMove =
                new Gaia.WebWidgets.AspectMouseMove();

            aspectMouseMove.MillisecondsInterval = GetPollingTime();
            aspectMouseMove.MouseMove += aspectMouseMove_MouseMove;

            zPanel.Aspects.Add(aspectMouseMove);
        }

        void aspectMouseMove_MouseMove(object sender,
            Gaia.WebWidgets.AspectMouseMove.MouseMoveEventArgs e)
        {
            zMessage.Text = string.Format("Mouse Moved to {0}:{1}px",
                e.Left, e.Top);
        }

        private static int GetPollingTime()
        {
            return WebUtility.IsLocalhost ? 100 : 750;
        }

        #endregion
    }
}