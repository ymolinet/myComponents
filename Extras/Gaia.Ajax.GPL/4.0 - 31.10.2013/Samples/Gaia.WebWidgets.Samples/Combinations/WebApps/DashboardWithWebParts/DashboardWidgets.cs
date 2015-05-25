namespace Gaia.WebWidgets.Samples.Combinations.WebApps.DashboardWithWebParts
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using Gaia.WebWidgets;
    using Gaia.WebWidgets.Effects;
    using Gaia.WebWidgets.Extensions;
    using Gaia.WebWidgets.Samples.Utilities;

    public enum WidgetContainerEnum
    {
        Left,
        Center,
        Right
    }

    public class Widget : Control
    {
        private bool _initialized;
        private readonly string _caption;
        private readonly WidgetContainerEnum _container;

        public WidgetContainerEnum DefaultContainer
        {
            get { return _container; }
        }

        public string Caption
        {
            get { return _caption; }
        }

        public Widget() { }
        public Widget(string id, string caption, WidgetContainerEnum container)
        {
            base.ID = id;
            _caption = caption;
            _container = container;
        }

        protected override void OnInit(EventArgs e)
        {
            if (_initialized) return;
            _initialized = true;
            InitializeComponent();
            base.OnInit(e);
        }

        protected virtual void InitializeComponent() { }
    }

    public class ChromedWidget : Widget, INamingContainer
    {
        readonly ExtendedPanel _chrome = new ExtendedPanel();

        public ChromedWidget() { }
        public ChromedWidget(string id, string caption, WidgetContainerEnum container) : base(id, caption, container) { }

        protected override void OnLoad(EventArgs e)
        {
            _chrome.Effects.Add(ExtendedPanel.Minimize, ExtendedPanel.ApplyToContent(new EffectBlindUp(0.5M)));
            _chrome.Effects.Add(ExtendedPanel.Restore, ExtendedPanel.ApplyToContent(new EffectBlindDown(0.5M)));
            
            EnsureChildControls();
            base.OnLoad(e);
        }

        public CssStyleCollection Style
        {
            get { return _chrome.Style; }
        }
        
        protected override void CreateChildControls()
        {
            CreateChromeControls();
            Controls.Add(_chrome);
            base.CreateChildControls();
        }

        protected ControlCollection ChromeControls
        {
            get { return _chrome.Controls; }
        }

        protected virtual void CreateChromeControls() { }

        /// <summary>
        /// Helper function to create a Chrome with default values
        /// </summary>
        protected override void InitializeComponent()
        {
            _chrome.ID = "crm";
            _chrome.Caption = Caption;
            _chrome.CssClass = WebUtility.StyleSheetTheme;
            _chrome.AnimationDuration = 300;
            _chrome.Style["cursor"] = "move";
            _chrome.ScrollBars = System.Web.UI.WebControls.ScrollBars.None;
            const bool revert = true;
            var aspectDraggable = new AspectDraggable(null, Rectangle.Empty, revert, false, 0.5M, null, ID);
            aspectDraggable.Revert = true;
            aspectDraggable.UseDocumentBody = true;
            aspectDraggable.DragCssClass = "dragging";
            aspectDraggable.DeepCopy = true;
            aspectDraggable.MakeGhost = true;

            _chrome.Aspects.Add(aspectDraggable);

            base.InitializeComponent();
        }
    }

    /// <summary>
    /// ScratchPad is used in the IGoogle Sample
    /// </summary>
    public class ScratchPadWidget : ChromedWidget
    {
        private readonly Panel _panel = new Panel();
        private readonly InPlaceEdit _edit = new InPlaceEdit();
       
        public ScratchPadWidget() { }
        public ScratchPadWidget(string id, string caption, WidgetContainerEnum container) : base(id, caption, container) { }

        protected override void CreateChromeControls()
        {
            _panel.Controls.Add(_edit);
            ChromeControls.Add(_panel);

            base.CreateChromeControls();
        }

        protected override void InitializeComponent()
        {
            _panel.ID = "pnlInPlEdit";
            _panel.Style["padding"] = "15px";

            _edit.ID = "inPlEdit";
            _edit.SingleLine = false;
            _edit.Text = "Here's a little scratchpad application for you";

            base.InitializeComponent();
        }
    }

    public class SearchWidget : ChromedWidget
    {
        private readonly AutoCompleter _auto = new AutoCompleter();
        private readonly System.Web.UI.WebControls.Panel _panel = new System.Web.UI.WebControls.Panel();

        public SearchWidget() { }
        public SearchWidget(string id, string caption, WidgetContainerEnum container) : base(id, caption, container) { }

        protected override void CreateChromeControls()
        {
            _panel.Controls.Add(_auto);
            ChromeControls.Add(_panel);

            base.CreateChromeControls();
        }

        protected override void InitializeComponent()
        {
            _panel.ID = "auto_pnl";
            _panel.Style["padding"] = "5px";
            _panel.Style["height"] = "150px";

            _auto.ID = "auto";
            _auto.CssClass = WebUtility.StyleSheetTheme;
            _auto.Width = new System.Web.UI.WebControls.Unit("160px");
            _auto.Height = new System.Web.UI.WebControls.Unit("120px");
            _auto.GetAutoCompleterItems += auto_GetAutoCompleterItems;
            _auto.SelectionChanged += auto_SelectionChanged;

            base.InitializeComponent();
        }

        void auto_SelectionChanged(object sender, AutoCompleter.AutoCompleterSelectionChangedEventArgs e)
        {
            int id = int.Parse((e.SelectedValue as LiteralControl).ID);
            var item = AutoCompleterItem.Items.Find(idx => idx.Id == id);
            _auto.Text = item.Header;
        }

        void auto_GetAutoCompleterItems(object sender, EventArgs e)
        {
            if (_auto.Text.Length == 0) return;

            foreach (var idx in AutoCompleterItem.Items.FindAll(
                idx2 => idx2.Content.ToLower().IndexOf(_auto.Text.ToLower()) != -1
                        || idx2.Header.ToLower().IndexOf(_auto.Text.ToLower()) != -1
                        || idx2.ImageUrl.ToLower().IndexOf(_auto.Text.ToLower()) != -1))
            {
                _auto.AutoCompleterItems.Add(CreateAutoCompleterItem(idx));
            }
        }

        private Control CreateAutoCompleterItem(AutoCompleterItem item)
        {
            var ctrl = new LiteralControl {ID = item.Id.ToString()};
            var content = ComposeXhtml.ToString(
                delegate(HtmlFormatting.XhtmlTagFactory create)
                    {
                        using (create.Div())
                        {
                            using (create.Img(null, null, ResolveUrl("~/" + item.ImageUrl), "")
                                .SetStyle("float:left;margin-right:5px;width:25px;height:25px;")) { }
                            using (create.Em().SetStyle("font-size:9px;").WriteContent(item.Header)) { }
                        }
                        using (create.Br()) { }

                    });
            ctrl.Text = content;
            ctrl.ID = item.Id.ToString();
            return ctrl;
        }
    }

    public class NewsWidget : ChromedWidget
    {
        private readonly TabControl _tab = new TabControl();
        private readonly TabView _view1 = new TabView();
        private readonly TabView _view2 = new TabView();
        private readonly System.Web.UI.WebControls.Literal _lit1 = new System.Web.UI.WebControls.Literal();
        private readonly System.Web.UI.WebControls.Panel _wrapper = new System.Web.UI.WebControls.Panel();
        private readonly System.Web.UI.WebControls.Literal _lit2 = new System.Web.UI.WebControls.Literal();

        public NewsWidget() { }
        public NewsWidget(string id, string caption, WidgetContainerEnum container) : base(id, caption, container) { }

        protected override void CreateChromeControls()
        {
            _view1.Controls.Add(_lit1);
            _view2.Controls.Add(_lit2);
            _tab.Controls.Add(_view1);
            _tab.Controls.Add(_view2);
            _wrapper.Controls.Add(_tab);
            ChromeControls.Add(_wrapper);

            base.CreateChromeControls();
        }

        protected override void InitializeComponent()
        {
            _wrapper.ID = "tabwrapper";
            _wrapper.Style["padding"] = "10px";

            _tab.CssClass = WebUtility.StyleSheetTheme;
            _tab.ID = "tab";

            _view1.Caption = "Ajax";
            _view1.ID = "view1";

            _lit1.ID = "textOneForTab";
            _lit1.Text = ComposeXhtml.ToString(
                delegate(HtmlFormatting.XhtmlTagFactory create)
                    {
                        using (var div = create.Div().SetStyle("padding:5px;"))
                        {
                            using (create.Img(null, null, ResolveUrl("Images/ajax-wash-small.png"), "").SetStyle("float:left;padding-right:5px;padding-bottom:15px;")) { }
                            div.WriteContent("Complexity inside complexity with no complexity");
                        }
                    });

            _view2.Caption = "The Gaia way";
            _view2.ID = "view2";

            _lit2.ID = "textTwoForTab";
            _lit2.Text = ComposeXhtml.ToString(
                delegate(HtmlFormatting.XhtmlTagFactory create)
                    {
                        using (var div = create.Div().SetStyle("padding:5px;"))
                        {
                            using (create.Img(null, null, ResolveUrl("Images/clock-small.png"), "").SetStyle("float:left;padding-right:5px;")) { }
                            div.WriteContent("Not to mention Time2Market");
                        }
                    });

            base.InitializeComponent();
        }
    }

    public class AdWidget : ChromedWidget
    {
        private readonly System.Web.UI.WebControls.Panel _panel = new System.Web.UI.WebControls.Panel();
        private readonly System.Web.UI.WebControls.Label _lit = new System.Web.UI.WebControls.Label();
        private readonly System.Web.UI.WebControls.Image _img = new System.Web.UI.WebControls.Image();

        public AdWidget() { }
        public AdWidget(string id, string caption, WidgetContainerEnum container) : base(id, caption, container) { }

        protected override void CreateChromeControls()
        {
            _panel.Controls.Add(_img);
            _panel.Controls.Add(_lit);
            ChromeControls.Add(_panel);

            base.CreateChromeControls();
        }

        protected override void InitializeComponent()
        {
            _panel.Style["padding"] = "5px";
            _panel.Style["padding-bottom"] = "10px";
            _panel.ID = "pnlImage";

            _img.ImageUrl = "Images/no_js-small.png";
            _img.AlternateText = " ";
            _img.ID = "imageForBlues";
            _img.Style["padding"] = "10px";
            _img.Style["padding-top"] = "0px";
            _img.Style["float"] = "left";

            _lit.Text = "No JavaScript know how needed. Purely C# or VB.NET";
            _lit.ID = "NOJsLit";

            base.InitializeComponent();
        }
    }

    public class OpenWindowWidget : ChromedWidget
    {
        private readonly string _windowPlaceHolderID;
        private readonly Window _window = new Window();
        private readonly ExtendedButton _btn = new ExtendedButton();
        private readonly System.Web.UI.WebControls.Panel _wrapper = new System.Web.UI.WebControls.Panel();

        public OpenWindowWidget() { }
        public OpenWindowWidget(string id, string caption, WidgetContainerEnum container, string windowPlaceHolderID)
            : base(id, caption, container)
        {
            _windowPlaceHolderID = windowPlaceHolderID;
        }

        protected override void InitializeComponent()
        {
            _wrapper.ID = "buttonwrapper";
            _wrapper.Style["padding"] = "10px";

            _btn.CssClass = WebUtility.StyleSheetTheme;
            _btn.Width = System.Web.UI.WebControls.Unit.Percentage(90);
            _btn.ID = "opensWindow";
            _btn.Text = "Open Modal Window";
            _btn.Click += btn_Click;

            _window.ID = "win";
            _window.Caption = "Modal Popup Window";
            _window.Resizable = true;
            _window.Closable = true;
            _window.Draggable = true;
            _window.Maximizable = true;
            _window.CssClass = WebUtility.StyleSheetTheme;
            _window.OpacityWhenMoved = 1;
            _window.Visible = false;
            _window.Height = System.Web.UI.WebControls.Unit.Pixel(350);
            _window.Width = System.Web.UI.WebControls.Unit.Pixel(550);
            _window.Aspects.Add(new AspectModal());

            base.InitializeComponent();
        }

        protected override void CreateChromeControls()
        {
            _wrapper.Controls.Add(_btn);
            ChromeControls.Add(_wrapper);
            _window.Controls.Add(new LiteralControl(CreateContentForWindow()));
            GetWindowPlaceholder().Controls.Add(_window);

            base.CreateChromeControls();
        }

        void btn_Click(object sender, EventArgs e)
        {
            _window.Visible = true;
        }

        private Control GetWindowPlaceholder()
        {
            var selectors = _windowPlaceHolderID.Split(new char[] { '.' });
            var placeholder = Page.Form.FindControl(selectors[0]);
            for (int index = 1; index < selectors.Length; ++index)
                placeholder = placeholder.FindControl(selectors[index]);
            return placeholder;
        }

        private string CreateContentForWindow()
        {
            return ComposeXhtml.ToString(
                delegate(HtmlFormatting.XhtmlTagFactory create)
                    {
                        using (create.Div().SetStyle("padding:15px; font-size: 11px;"))
                        {
                            using (create.Img(null, null, ResolveUrl("Images/ajax-wash.png"), "Ajax").SetStyle("float:left;padding-right:10px;")) { }
                            using (var p = create.P())
                            {
                                p.WriteContent(@"Here we are opening a Window from an Event Handler in C# for a dynamically created control. Gaia's main 
advantage is that you can create really complex Ajax functionality with zero knowledge of JavaScript.
This has several advantages.");
                            }

                            using (create.Ul().SetStyle("list-style-type:disc;list-style-position:inside;"))
                            {
                                using (create.Li().WriteContent("No need to debug JavaScript")) { }
                                using (create.Li().WriteContent("More secure since no Business Logic leaks to the client")) { }
                                using (create.Li().WriteContent("Less Time2Market")) { }
                                using (create.Li().WriteContent("Less code still more functionality")) { }
                                using (create.Li().WriteContent("Less learning")) { }
                                using (create.Li().WriteContent("Less Risk")) { }
                            }

                            using (var p = create.P())
                            {
                                p.WriteContent(@"In fact a lot of the users of Gaia have said that Gaia feels like developing for the desktop. And this might
to some extent be true since it abstracts away all the difficulties from the HTTP and web like state, Ajax, DHTML 
skinning to make things beautiful and so on. Gaia builds completely on top of .Net but thanks to the really great 
Open Source .Net Linux port; Mono - most Gaia applications will also run on Linux");
                            }
                        }
                    });
        }
    }

    public class ImageGalleryWidget : ChromedWidget
    {
        private readonly Label _lit = new Label();
        private readonly Panel _panel = new Panel();
        private readonly ImageButton _imgGallery = new ImageButton();

        public ImageGalleryWidget() { }
        public ImageGalleryWidget(string id, string caption, WidgetContainerEnum container) : base(id, caption, container) { }

        protected override void CreateChromeControls()
        {
            _panel.Controls.Add(_imgGallery);
            _panel.Controls.Add(_lit);
            ChromeControls.Add(_panel);

            base.CreateChromeControls();
        }

        private const string ImageLocation = "~/media/collections/images/jazz_albums";

        static ImageGalleryWidget()
        {
            string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(ImageLocation));
            MusicImages = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
                MusicImages[i] = new FileInfo(files[i]).Name;
        }

        protected override void InitializeComponent()
        {
            _panel.ID = "imgGallery";
            _panel.Style["padding"] = "10px";
            _panel.Style["padding-bottom"] = "25px";

            _imgGallery.ID = "imgWidg";
            _imgGallery.Click += imgGallery_Click;
            _imgGallery.Style["float"] = "left";
            _imgGallery.Style["padding-right"] = "10px";
            _imgGallery.Style["margin-bottom"] = "15px";
            _imgGallery.ImageUrl = CurrentImageUrl;

            _lit.Text = "Click image to browse Image Gallery";
            _lit.ID = "clickText";

            base.InitializeComponent();
        }

        string CurrentImageUrl
        {
            get { return ResolveUrl(string.Format("{0}/{1}", ImageLocation, MusicImages[ImgNo])); }
        }

        private static int ImgNo
        {
            get { return (int)(HttpContext.Current.Session["JazzImgNo"] ?? 0); }
            set { HttpContext.Current.Session["JazzImgNo"] = value; }
        }

        void imgGallery_Click(object sender, ImageClickEventArgs e)
        {
            var img = sender as ImageButton;
            ImgNo = (++ImgNo) % MusicImages.Length;
            if (img != null) img.ImageUrl = CurrentImageUrl;
        }

        private static readonly string[] MusicImages; 
    }

    public class AutoCompleterItem
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Header { get; set; }

        public string Content { get; set; }

        public AutoCompleterItem(string header, string content, string image)
        {
            Content = content;
            Header = header;
            ImageUrl = image;
            Id = _items.Count;
        }

        private static readonly List<AutoCompleterItem> _items = new List<AutoCompleterItem>();
        public static List<AutoCompleterItem> Items
        {
            get
            {
                if (_items.Count == 0)
                {
                    _items.Add(new AutoCompleterItem("Kind of blue",
                                                     "John Coltrane and Miles Davis",
                                                     "media/collections/images/jazz_albums/kind_of_blue.jpg"));
                    _items.Add(new AutoCompleterItem("Live at the 1963 Monterey Jazz Festival",
                                                     "Miles Davis",
                                                     "media/collections/images/jazz_albums/liveat1963.jpg"));
                    _items.Add(new AutoCompleterItem("Miles Davis - Cool Jazz Sound",
                                                     "Miles Davis",
                                                     "media/collections/images/jazz_albums/cool_jazz.jpg"));
                    _items.Add(new AutoCompleterItem("Miles Davis & the Modern Jazz Giants",
                                                     "Miles Davis",
                                                     "media/collections/images/jazz_albums/giants.jpg"));
                    _items.Add(new AutoCompleterItem("Billy Remembers Billie",
                                                     "Billie Holiday",
                                                     "media/collections/images/jazz_albums/remember.jpg"));
                    _items.Add(new AutoCompleterItem("Troubled Souls",
                                                     "Billy Holiday",
                                                     "media/collections/images/jazz_albums/troubled.jpg"));
                    _items.Add(new AutoCompleterItem("A Love Supreme",
                                                     "John Coltrane",
                                                     "media/collections/images/jazz_albums/supreme.jpg"));
                    _items.Add(new AutoCompleterItem("My Favorite Things",
                                                     "John Coltrane",
                                                     "media/collections/images/jazz_albums/favorite.jpg"));
                    _items.Add(new AutoCompleterItem("Transformer",
                                                     "Lou Reed",
                                                     "media/collections/images/jazz_albums/transformer.jpg"));
                    _items.Add(new AutoCompleterItem("La Cucaracha",
                                                     "Ween",
                                                     "media/collections/images/jazz_albums/cucaracha.jpg"));
                    _items.Add(new AutoCompleterItem("12 Golden Country Greats",
                                                     "Ween",
                                                     "media/collections/images/jazz_albums/12.jpg"));
                    _items.Add(new AutoCompleterItem("Electriclarryland",
                                                     "Buthole Surfers",
                                                     "media/collections/images/jazz_albums/larryland.jpg"));
                    _items.Add(new AutoCompleterItem("Independent Worm Saloon",
                                                     "Buthole Surfers",
                                                     "media/collections/images/jazz_albums/worm.jpg"));
                    _items.Add(new AutoCompleterItem("Weird Revolution",
                                                     "Buthole Surfers",
                                                     "media/collections/images/jazz_albums/weird.jpg"));
                }
                return _items;
            }
        }
    }
}