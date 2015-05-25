namespace Gaia.WebWidgets.Samples.BasicControls.DynamicImage.Captcha
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Captcha : System.Web.UI.UserControl
    {
        // For generating random numbers
        private readonly Random _random = new Random();

        private const int NoiseSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CaptchaText = WebUtility.GeneratePasswordInLowercase(CaptchaLength);
        }

        [ToolboxItem(false)]
        public bool IsValid
        {
            get { return Text == CaptchaText; }
        }

        public string Text
        {
            get { return zText.Text; }
        }

        /// <summary>
        /// Store CaptchaText in Session because of DynamicImage implementation
        /// </summary>
        private string CaptchaText
        {
            get
            {
                if (HttpContext.Current.Session["CaptchaText"] == null)
                    CaptchaText = WebUtility.GeneratePasswordInLowercase(CaptchaLength);

                return HttpContext.Current.Session["CaptchaText"].ToString();
            }
            set
            {
                HttpContext.Current.Session["CaptchaText"] = value;
             
                //to make DynamicImage redraw, we need to set a new ID
                zDynImg.ImageId = Guid.NewGuid().ToString().Replace("-", "");
            }
        }

        public void SetCaptchaText(string captchaText)
        {
            CaptchaText = captchaText;
        }

        private int _captchaLength;
        [DefaultValue(5)]
        public int CaptchaLength
        {
            get { return _captchaLength == 0 ? 5 : _captchaLength; }
            set { _captchaLength = value; }
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get
            {
                if (_backgroundColor == Color.Empty)
                    _backgroundColor = Color.White;
                return _backgroundColor;
            }
            set { _backgroundColor = value; }
        }

        private Color _foreColor;
        public Color ForeColor
        {
            get
            {
                if (_foreColor == Color.Empty)
                    _foreColor = Color.Black;
                return _foreColor;
            }
            set { _foreColor = value; }
        }

        private Color _noiseColor;
        public Color NoiseColor
        {
            get
            {
                if (_noiseColor == Color.Empty)
                    _noiseColor = Color.LightGray;
                return _noiseColor;
            }
            set { _noiseColor = value; }
        }

        private int _captchaHeight;
        [DefaultValue(25)]
        public int CaptchaHeight
        {
            get
            {
                if (_captchaHeight == 0)
                    _captchaHeight = 25;
                return _captchaHeight;
            }
            set { _captchaHeight = value; }
        }

        private int _captchaWidth;
        [DefaultValue(70)]
        public int CaptchaWidth
        {
            get
            {
                if (_captchaWidth == 0)
                    _captchaWidth = 70;
                return _captchaWidth;
            }
            set { _captchaWidth = value; }
        }

        protected void zDynImg_RetrieveImage(object sender, Gaia.WebWidgets.DynamicImage.RetrieveImageEventArgs e)
        {
            e.Image = new Bitmap(CaptchaWidth, CaptchaHeight);
            var g = Graphics.FromImage(e.Image);

            var rectangle = new Rectangle(0, 0, e.Image.Width, e.Image.Height);

            //add background color
            using (var backgroundBrush = new SolidBrush(BackgroundColor))
            {
                g.FillRectangle(backgroundBrush, rectangle);
            }

            //add background noise
            AddBackgroundNoise(g, rectangle, NoiseSize);

            // Text
            var font = new Font(FontFamily.GenericSansSerif, 12F, FontStyle.Bold);
            g.DrawString(CaptchaText, font, new SolidBrush(ForeColor), 2, 2);
        }

        private void AddBackgroundNoise(Graphics g, Rectangle rectangle, int noiseSize)
        {
            var max = Convert.ToInt32(Math.Max(rectangle.Width, rectangle.Height) / noiseSize);
            using (var noiseBrush = new SolidBrush(NoiseColor))
            {
                for (var i = 0; i <= Convert.ToInt32(rectangle.Width*rectangle.Height/noiseSize); i++)
                    g.FillEllipse(noiseBrush, _random.Next(rectangle.Width), _random.Next(rectangle.Height),
                                  _random.Next(max),
                                  _random.Next(max));
            }
        }
    }
}