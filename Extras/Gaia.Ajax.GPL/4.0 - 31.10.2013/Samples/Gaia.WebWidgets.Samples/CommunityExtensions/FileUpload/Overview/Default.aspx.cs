namespace Gaia.WebWidgets.Samples.CommunityExtensions.FileUpload.Overview
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.CommunityExtensions;
    using Image = System.Drawing.Image;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string folder = Server.MapPath(AppRelativeTemplateSourceDirectory + "/Images/");
            foreach (string image in Directory.GetFiles(folder))
            {
                FileInfo file = new FileInfo(image);
                if (file.Extension.ToLower() != ".png")
                    continue;

                zResults.Controls.Add(CreateImage("Images/" + file.Name));
            }
        }

        protected void fileUpload_OnFileUploaded(object sender, FileUpload.FileUploadEventArgs e)
        {
            string contentType = e.PostedFile.ContentType.ToLower();
            
            if (!contentType.StartsWith("image"))
            {
                e.InfoText = "You are only allowed to upload images";
                return;
            }

            // note: try/catch this
            Image image = Image.FromStream(e.PostedFile.InputStream);
            if (image.Width < 100 || image.Height < 100)
            {
                e.InfoText = "Minimum 100*100 pixels images are allowed to be uploaded";
                return;
            }

            const decimal thumbnailMaxWidthHeight = 100M;
            decimal ratio = thumbnailMaxWidthHeight/Math.Max(image.Width, image.Height);

            Image thumbnail =  image.GetThumbnailImage(
                                        Convert.ToInt32(image.Width*ratio), 
                                        Convert.ToInt32(image.Height*ratio), 
                                        delegate {return false; }, 
                                        IntPtr.Zero);

            // write the image using a guid name instead to avoid conflicts ... 
            string uniqueName = Guid.NewGuid().ToString().Replace("-", "") + ".png";
            string destFolder = Server.MapPath(AppRelativeTemplateSourceDirectory + "/Images/");
            string filename = destFolder + uniqueName;  
            
            // all new thumbnails end up as png
            thumbnail.Save(filename, ImageFormat.Png);

            // We add the newly uploaded image to Session, because this FileUpload event
            // cannot serialize changes back to the page in this event. 
            NewlyUploadedImages.Add(uniqueName);

        }

        protected void fileUpload_OnUploadFinished(object sender, EventArgs e)
        {
            if (NewlyUploadedImages.Count == 0)
                return; // no files were uploaded so just exit

            foreach (string imgurl in NewlyUploadedImages)
                zResults.Controls.Add(CreateImage("Images/" + imgurl));

            NewlyUploadedImages.Clear();
        }

        private static Gaia.WebWidgets.Image CreateImage(string url)
        {
            Gaia.WebWidgets.Image newImageControl = new Gaia.WebWidgets.Image();
            newImageControl.ID = url.Replace("/", "").Replace("-", "").Replace(".", "");
            newImageControl.BorderWidth = 1;
            newImageControl.ImageUrl = url;
            return newImageControl;
        }

        //TODO fix so they are put in the same order in the temp session
         //   maybe remove id after that
        public List<string> NewlyUploadedImages
        {
            get
            {
                if (Session["newImages"] == null)
                    Session["newImages"] = new List<string>();

                return Session["newImages"] as List<string>;
            }
            set { Session["newImages"] = value; }
        }
    }
}
