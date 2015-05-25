namespace Gaia.WebWidgets.Samples.Combinations.WebApps.SimpleCalendarApplication
{
    using System;
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Instead of using the property Modal="true" on the window, we can customize the modality like this
            zWindow.Aspects.Add(new AspectModal(Color.Black, 0.7));
        }

        /// <summary>
        /// In this ClickHandler we open the Main Application (Window) itself. Also setting the button to toggled.
        /// </summary>
        protected void zBtnStartApplication_Click(object sender, EventArgs e)
        {
            zWindow.Visible = true;
            zBtnStartApplication.Toggled = true;
        }

        /// <summary>
        /// The Window_Closing event is fired when the user closes the window. You could set e.ShouldClose to false if you
        /// want to deny the user the ability to close the window. 
        /// </summary>
        protected void zWindow_Closing(object sender, Gaia.WebWidgets.Extensions.Window.WindowClosingEventArgs e)
        {
            zBtnStartApplication.Toggled = false;
        }
    }
}