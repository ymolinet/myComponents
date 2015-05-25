using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Widgets.DataControls
{
    class DataControlFactory
    {
        public static Button Button(string text, string commandName, string commandArgument, bool causeValidation, ASP.IPostBackContainer container)
        {
            bool inContainer = container != null && !causeValidation;
            Button button = inContainer ? new DataControlButton(container) : new Button();
            Setup(button, text, commandName, commandArgument, causeValidation, inContainer);
            return button;
        }

        public static LinkButton LinkButton(string text, string commandName, string commandArgument, bool causeValidation, ASP.IPostBackContainer container)
        {
            bool inContainer = container != null && !causeValidation;
            LinkButton button = inContainer ? new DataControlLinkButton(container) : new LinkButton();
            Setup(button, text, commandName, commandArgument, causeValidation, inContainer);
            return button;
        }

        public static ImageButton ImageButton(string text, string imageUrl, string commandName, string commandArgument, bool causeValidation, ASP.IPostBackContainer container)
        {
            bool inContainer = container != null && !causeValidation;
            ImageButton button = inContainer ? new DataControlImageButton(container) : new ImageButton();
            button.ImageUrl = imageUrl;
            button.AlternateText = text;
            Setup(button, text, commandName, commandArgument, causeValidation, inContainer);
            return button;
        }

        public static LinkButton PagerLinkButton(string text, string commandName, string commandArgument, bool causeValidation, ASP.IPostBackContainer container)
        {
            bool inContainer = container != null && !causeValidation;
            LinkButton button = inContainer ? new DataControlPagerLinkButton(container) : new LinkButton();
            Setup(button, text, commandName, commandArgument, causeValidation, inContainer);
            return button;
        }

        private static void Setup(ASP.IButtonControl buttonControl, string text, string commandName, string commandArgument, bool causeValidation, bool inContainer)
        {
            buttonControl.Text = text;
            buttonControl.CommandName = commandName;
            buttonControl.CommandArgument = commandArgument;
            if (!inContainer) buttonControl.CausesValidation = causeValidation;
        }
    }
}
