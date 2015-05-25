namespace Gaia.WebWidgets.Samples.BasicControls.DynamicImage.Overview
{
    using UI;

    using System;
    using System.IO;
    using System.Web.UI;
    using System.Drawing;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    
    public partial class Default : SamplePage
    {
        private readonly DynamicImageController _controller = new DynamicImageController();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only enable file uploading for localhost. 
            uploadImageButton.Visible = _controller.IsLocalhost;

            if (!IsPostBack)
            {
                PopulateImagesList();

                slider.Value = 68.0D;

                // Initialize image controller
                _controller.Left = 0;
                _controller.Top = 0;
                _controller.Index = 0;
                _controller.Zoom = 2D;
                _controller.Width = 200;
                _controller.Height = 200;
                _controller.SelectedItem = _controller.GetByFileName(ddl.SelectedValue);
                
                // Making map re-render
                SetNewImageId();
            }

            // Adding aspects
            map.Aspects.Add(new AspectDraggable(MapDragged));
            winFileUpload.Aspects.Add(new AspectModal());
            mapWrapper.Aspects.Add(new AspectResizable(
                                       MapResized,  AspectResizable.ResizeModes.RightBorder | AspectResizable.ResizeModes.BottomBorder,
                                       150, 150, 400, 400));
        }

        private static ListItem CreateListItem(KeyValuePair<FileInfo, Bitmap> kv)
        {
            return new ListItem
                       {
                           Value = kv.Key.Name,
                           Text = string.Format("{0} ({1} by {2} pixels)", kv.Key.Name,
                                                kv.Value.Width, kv.Value.Height)
                       };
        }

        protected void MapResized(object sender, EventArgs e)
        {
            _controller.Width = Convert.ToInt32(mapWrapper.Width.Value);
            _controller.Height = Convert.ToInt32(mapWrapper.Height.Value);

            // Making map re-render
            SetNewImageId();
        }

        private void SetNewImageId()
        {
            map.ImageId = Guid.NewGuid().ToString().Replace("-", "_");
        }

        protected void MapDragged(object sender, EventArgs e)
        {
            var dragger = (AspectDraggable) sender;
            var img = (WebWidgets.DynamicImage) dragger.ParentControl;

            _controller.Index += 1;

            // Getting delta
            string leftCoords = img.Style[HtmlTextWriterStyle.Left].Replace("px", "");
            string topCoords = img.Style[HtmlTextWriterStyle.Top].Replace("px", "");
            int leftInt = -Convert.ToInt32(leftCoords);
            int topInt = -Convert.ToInt32(topCoords);
            leftInt = (int)(leftInt * _controller.Zoom);
            topInt = (int)(topInt * _controller.Zoom);

            _controller.Left += leftInt;
            _controller.Top += topInt;

            // Validating inside of map
            var sourceWidth = (int)(_controller.Width * _controller.Zoom);
            var sourceHeight = (int)(_controller.Height * _controller.Zoom);

            var maxWidth = _controller.SelectedItem.Value.Width;
            var maxHeight = _controller.SelectedItem.Value.Height;
            _controller.Left = Math.Min(Math.Max(0, _controller.Left), maxWidth - sourceWidth);
            _controller.Top = Math.Min(Math.Max(0, _controller.Top), maxHeight - sourceHeight);

            // Setting label
            SetCoordinateLabel();

            // Setting map placement (IMG element) back to "zenith"
            img.Style[HtmlTextWriterStyle.Left] = "0px";
            img.Style[HtmlTextWriterStyle.Top] = "0px";

            // Making map re-render
            SetNewImageId();
        }

        private void SetCoordinateLabel()
        {
            mapCoords.Text = string.Format("Left: {0}px, Top: {1}px", _controller.Left, _controller.Top);
        }

        protected void DdlSelectedIndexChanged(object sender, EventArgs e)
        {
            //set top and left to make sure to display small pictures so they don't disappear outside
            _controller.Top = 0;
            _controller.Left = 0;
            _controller.SelectedItem = _controller.GetByFileName(ddl.SelectedValue);
            
            // Making map re-render
            SetNewImageId();

            // Setting label
            SetCoordinateLabel();
        }

        protected void MapRetrieveImage(object sender, WebWidgets.DynamicImage.RetrieveImageEventArgs e)
        {
            DrawMap(e);
        }

        #region DynamicImageRetrieveEvent
        private void DrawMap(WebWidgets.DynamicImage.RetrieveImageEventArgs e)
        {
            DrawActualImage(e);
        }

        private void DrawActualImage(WebWidgets.DynamicImage.RetrieveImageEventArgs e)
        {
            e.Image = new Bitmap(_controller.Width, _controller.Height);
            var g = Graphics.FromImage(e.Image);
            var img = _controller.SelectedItem.Value;
            var sourceWidth = (int)(_controller.Width * _controller.Zoom);
            var sourceHeight = (int)(_controller.Height * _controller.Zoom);
            var rect = new Rectangle(_controller.Left, _controller.Top, sourceWidth, sourceHeight);
            g.DrawImage(img, new Rectangle(0, 0, _controller.Width, _controller.Height), rect, GraphicsUnit.Pixel);
            g.FillRectangle(Brushes.Beige, new Rectangle(0, _controller.Height - 35, 120, 35));
            g.DrawString("Generated on the fly from the server", new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold), Brushes.Black,
                         new RectangleF(5, _controller.Height - 30, 120, 30));
        } 
        #endregion

        protected void SliderValueChanged(object sender, EventArgs e)
        {
            lblResult.Text = String.Format("{0:0.0} %", slider.Value);
            lblResult.Effects.Add(new WebWidgets.Effects.EffectHighlight());
            _controller.Zoom = (Math.Max(100D - slider.Value, 1.0D) * 0.1D) + 0.5D;

            // Making map re-render
            SetNewImageId();
        }

        protected void UploadImageButtonClick(object sender, EventArgs e)
        {
            // Open the window with the Ajax File Uploader
            winFileUpload.Visible = true;
        }

        protected void fileUpload_OnFileUploaded(object sender, WebWidgets.CommunityExtensions.FileUpload.FileUploadEventArgs e)
        {
            var targetFileName = _controller.SourceFolder + "\\" + e.PostedFile.FileName; 
            e.PostedFile.SaveAs(targetFileName);

            var fileInfo = new FileInfo(targetFileName);

            _controller.ImagesList.Add(fileInfo, new Bitmap(fileInfo.FullName));
        }

        protected void fileUpload_OnUploadFinished(object sender, EventArgs e)
        {
            ddl.Items.Clear();

            PopulateImagesList();

            winFileUpload.Visible = false;
        }

        private void PopulateImagesList()
        {
            foreach (var image in _controller.ImagesList)
                ddl.Items.Add(CreateListItem(image));
        }
    }
}