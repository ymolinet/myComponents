namespace Gaia.WebWidgets.Samples.Aspects.AspectUpdateControl.Multiple
{
    using System;
    using System.Web.UI;
    using Gaia.WebWidgets;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            const string quote = "The quick brown fox jumps over the lazy dog";
            string[] words = quote.Split(' ');

            foreach (string word in words)
            {
                // Label for the Word with Click handler
                var wordLabel = new Label();
                wordLabel.Style["cursor"] = "pointer";
                wordLabel.Text = word;
                wordLabel.Click += wordLabel_Click;

                // Another label that will act as the "update-icon"
                var updateControl = new Label();
                updateControl.Style["display"] = "none";
                updateControl.CssClass = "ajax-loader";
                updateControl.Text = "&nbsp;";

                // Add AspectUpdateControl with a reference to the updateLabel
                wordLabel.Aspects.Add(new AspectUpdateControl(updateControl));

                // Add the Controls to the container
                zPanel.Controls.Add(wordLabel);
                zPanel.Controls.Add(updateControl);
                zPanel.Controls.Add(new LiteralControl("&nbsp;"));
            }
        }

        void wordLabel_Click(object sender, AspectClickable.ClickEventArgs e)
        {
            System.Threading.Thread.Sleep(1000); /* Sleep for a second */
            (sender as Label).ForeColor = WebUtility.GetRandomColor();
        } 
        #endregion
    }
}