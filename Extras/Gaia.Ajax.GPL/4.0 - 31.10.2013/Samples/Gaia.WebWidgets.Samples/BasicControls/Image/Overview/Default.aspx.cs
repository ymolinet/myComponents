namespace Gaia.WebWidgets.Samples.BasicControls.Image.Overview
{
    using System;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            FillDropDownListImages();
            SetSelectedImage();
        }

        protected void zDropDownListAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            zImage1.ImageAlign = (ASP.ImageAlign)Enum.Parse(typeof(ASP.ImageAlign), zDropDownListAlign.SelectedValue);
        }

        protected void zDropDownListImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSelectedImage();
        }

        private void SetSelectedImage()
        {
            zImage1.ImageUrl = zDropDownListImage.SelectedValue;
        }

        private void FillDropDownListImages()
        {
            zDropDownListImage.DataSource = Utilities.MediaUtility.GetImageFiles("scenery");
            zDropDownListImage.DataBind();
        } 
        #endregion
    }
}
