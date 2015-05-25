namespace Gaia.WebWidgets.Samples.Extensions.Window.Nested
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            int windowCount = int.Parse(zNumberOfWindows.SelectedItem.Text);
            var windows = new Gaia.WebWidgets.Extensions.Window[windowCount];

            for (int i = 0; i < windowCount; i++)
            {
                int index = i;
                bool isLastWindow = index == windowCount - 1;

                var window = windows[i] = CreateWindowWithButton(
                                                 (i + 1),
                                                 delegate { windows[index + 1].Visible = true; },
                                                 !isLastWindow);

                bool isFirstWindow = i == 0;
                if (isFirstWindow)
                {
                    // The first Window will be opened by the static button
                    Form.Controls.Add(window);
                    zOpenFirstWindow.Click += delegate { window.Visible = true; };
                }
                else
                    windows[i - 1].Controls.Add(window);
            }

        } 
        #endregion

        private Gaia.WebWidgets.Extensions.Window CreateWindowWithButton(int number, EventHandler handler, bool buttonVisible)
        {
            var window = new Gaia.WebWidgets.Extensions.Window();
            window.ID = "win";
            window.CssClass = StyleSheetTheme;
            window.CenterInForm = true;
            window.Draggable = number == 1;
            window.Maximized = false;
            window.Resizable = true;
            window.Minimizable = true;
            window.OpacityWhenMoved = 1;
            window.Caption = "Window " + number;
            window.Visible = false;
            window.Modal = zModal.Checked;
            window.Width = 900 - (number*50);
            window.Height = 750 - (number*50);

            var button = new Gaia.WebWidgets.Button();
            button.ID = "open";
            button.Text = "Open Window # " + (number + 1);
            button.Click += handler;
            button.Visible = buttonVisible;
            button.Style["display"] = "block";

            window.Controls.Add(button);

            return window;
        }
    }
}