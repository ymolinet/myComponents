namespace Gaia.WebWidgets.Samples.Combinations.WebUtilities.Cropper
{
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;
    using System;

    public partial class Default : SamplePage
    {
        // todo: add MaxWidth and MaxHeight for resizable as well.
        private const int PanelCropBorderWidth = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentRectangle = new Rectangle(0, 0, 200, 200);
            }

            var boundingRect = new Rectangle(0, 0,
                                                   (int) (zImage.Width.Value - zPanelCrop.Width.Value),
                                                   (int) (zImage.Height.Value - zPanelCrop.Height.Value));

            var draggable = new AspectDraggable(UpdateCoordinates, boundingRect);
            draggable.HandleControl = zPanelCropHandle;
            zPanelCrop.Aspects.Add(draggable);

            var resizable = new AspectResizable();
            resizable.BoundingRectangle = new Rectangle(0, 0, (int) zImage.Width.Value, (int) zImage.Height.Value);
            resizable.MinHeight = 100;
            resizable.MinWidth = 100;
            resizable.Resized += OnCropperResized;

            zPanelCrop.Aspects.Add(resizable);
        }

        void OnCropperResized(object sender, EventArgs e)
        {
            UpdateCoordinates(sender, e);

            zPanelCrop.Width = (int)zPanelCrop.Width.Value - PanelCropBorderWidth * 2;
            zPanelCrop.Height = (int)zPanelCrop.Height.Value - PanelCropBorderWidth * 2;

            // recompute bounding rectangle for dragging and force re-rendering to get new aspect state
            var left = int.Parse(zPanelCrop.Style["left"].Replace("px", ""));
            var top = int.Parse(zPanelCrop.Style["top"].Replace("px", ""));
            
            zPanelCrop.Aspects.Find<AspectDraggable>().Rectangle = new Rectangle(-left, -top,
                                                   (int)(zImage.Width.Value - zPanelCrop.Width.Value),
                                                   (int)(zImage.Height.Value - zPanelCrop.Height.Value));
            
            zPanelCrop.Aspects.Find<AspectResizable>().BoundingRectangle = new Rectangle(-left, -top,
                                                                                         (int) zImage.Width.Value,
                                                                                         (int) zImage.Height.Value);

            ((IAjaxControl) zPanelCrop).AjaxControl.ShouldRender = true;
        }

        void UpdateCoordinates(object sender, EventArgs e)
        {
            const int totalBorderWidth = PanelCropBorderWidth*2;
            int w = (int)zPanelCrop.Width.Value - totalBorderWidth;
            int h = (int)zPanelCrop.Height.Value - totalBorderWidth;
            int x = int.Parse(zPanelCrop.Style["left"].Replace("px", ""));
            int y = int.Parse(zPanelCrop.Style["top"].Replace("px", ""));

            CurrentRectangle = new Rectangle(x, y, w, h);
        }

        protected void zButtonCrop_Click(object sender, EventArgs e)
        {
            zCroppedImage.ImageId = Guid.NewGuid().ToString().Replace("-", "");
            EnsureWindowMatchCroppedImage();
            zWindowResult.Visible = true;
        }

        private void EnsureWindowMatchCroppedImage()
        {
            zWindowResult.Height = CurrentRectangle.Height + 36;
            zWindowResult.Width = CurrentRectangle.Width + 20;
        }

        private static Image CropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        protected void zCroppedImage_RetrieveImage(object sender, WebWidgets.DynamicImage.RetrieveImageEventArgs e)
        {
            var input = Image.FromFile(MapPath("~/Combinations/WebUtilities/Cropper/crop.jpg"));
            var output = CropImage(input, CurrentRectangle);
            e.Image = output;
        }

        private Rectangle CurrentRectangle
        {
            get { return (Rectangle) (Session["rect"]); }
            set { Session["rect"] = value; }
        }
    }
}
