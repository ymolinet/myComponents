namespace Gaia.WebWidgets.Samples.BasicControls.Validators.CustomValidator
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zSubmit_Click(object sender, EventArgs e)
        {
            // did it pass the custom validation
            zResult.Text = Page.IsValid ? "Page is valid." : "Page is not valid!";
        }

        protected void zCustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            try
            {
                // is the value in text box even?
                int i = int.Parse(args.Value);
                args.IsValid = ((i%2) == 0);
            }

            catch
            {
                args.IsValid = false;
            }
        }
    }
}
