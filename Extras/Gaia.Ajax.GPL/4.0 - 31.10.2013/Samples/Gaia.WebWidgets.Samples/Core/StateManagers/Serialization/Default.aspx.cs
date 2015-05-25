namespace Gaia.WebWidgets.Samples.Core.StateManagers.Serialization
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            const int count = 40;
            var labels = new Label[count];
            for (int i = 0; i < count; i++)
            {
                var label = labels[i] = new Label();
                label.Click += delegate
                                   {
                                       label.BackColor = WebUtility.GetRandomColor();
                                       var doPartialRendering = int.Parse(zRenderOption.SelectedValue) == 0;

                                       if (doPartialRendering)
                                           zRoot.ForceAnUpdate();
                                   };
                if (i > 0)
                    labels[i - 1].Controls.Add(label);
            }

            zRoot.Controls.Add(labels[0]);
        }
    }
}
