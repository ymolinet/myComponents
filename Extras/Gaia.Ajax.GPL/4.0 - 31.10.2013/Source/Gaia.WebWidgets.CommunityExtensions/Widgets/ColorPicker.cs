using System;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing;
using System.Globalization;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;
using Gaia.WebWidgets.Extensions;
using Gaia.WebWidgets.HtmlFormatting;

[assembly: WebResource("Gaia.WebWidgets.CommunityExtensions.Resources.ColorPicker.Style.styles.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Gaia.WebWidgets.CommunityExtensions.Resources.ColorPicker.Style.color_bg.png", "image/png")]
[assembly: WebResource("Gaia.WebWidgets.CommunityExtensions.Resources.ColorPicker.Style.picker_mask.png", "image/png")]
[assembly: WebResource("Gaia.WebWidgets.CommunityExtensions.Resources.ColorPicker.Style.picker_handle.png", "image/png")]

namespace Gaia.WebWidgets.CommunityExtensions
{
    public class ColorPicker : GaiaControl, INamingContainer, IAjaxContainerControl
    {
        #region [ -- Private Members -- ]

        private const int Size = 150;
        private const int HandleSize = 10;
        private const int ContainerAdditionalSize = 82;
        private const int ForeColorChangePoint = 110;

        private Slider _slider;
        private Panel _colorPicker;
        private Panel _resultViewer;
        private Button _toggleButton;
        private Panel _colorPickerHandle;
        private TextBox _mainTextBox;
        private TextBox _htmlColorTextBox;
        private TextBox _hueTextBox;
        private TextBox _valueTextBox;
        private TextBox _saturationTextBox;
        private TextBox _redTextBox;
        private TextBox _blueTextBox;
        private TextBox _greenTextBox;

        private const string SliderId = "s";
        private const string PickerId = "p";

        private const string DoubleRepresentationFormat = "0.##";

        private ASP.Unit _width = ASP.Unit.Empty;
        private ASP.Unit _height = ASP.Unit.Empty;

        private AjaxContainerControl _instance;
        private AjaxContainerControl _ajaxContainerControl;

        #endregion

        #region [ -- Events -- ]

        public event EventHandler ColorChanged;

        #endregion

        #region [ -- Properies -- ]

        [DefaultValue(typeof(ASP.Unit), "")]
        [Category("Layout")]
        public ASP.Unit Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [DefaultValue(typeof(ASP.Unit), "")]
        [Category("Layout")]
        public ASP.Unit Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public string SelectedColor
        {
            get { return StateUtil.Get(ViewState, "SelectedColor", "#FFFFFF"); }
            set
            {
                SetStateValue("SelectedColor", value, "#FFFFFF");
                IsColorSelected = true;
            }
        }

        private bool IsColorSelected
        {
            get { return StateUtil.Get(ViewState, "IsColorSelected", false); }
            set { StateUtil.Set(ViewState, "IsColorSelected", value, false); }
        }

        private bool ColorPickerVisible
        {
            get { return StateUtil.Get(ViewState, "CalendarVisible", false); }
            set { SetStateValue("CalendarVisible", value, false); }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterCssFile();
        }

        protected override void CreateChildControls()
        {
            BuildColorPicker();
            base.CreateChildControls();
        }

        private void RegisterCssFile()
        {
            var css = "<link href=\"" + Page.ClientScript.GetWebResourceUrl(GetType(), "Gaia.WebWidgets.CommunityExtensions.Resources.ColorPicker.Style.styles.css") + "\" type=\"text/css\" rel=\"stylesheet\" />";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "cssFile", css, false);
        }

        internal void SetStateValue<T>(string key, T value, T defaultValue)
        {
            var initial = StateUtil.Get(ViewState, key, defaultValue);

            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(initial, value)) return;

            StateUtil.Set(ViewState, key, value, defaultValue);
            RequiresRecomposition();
        }

        private void RequiresRecomposition()
        {
            if (!ChildControlsCreated) return;
            ChildControlsCreated = false;
        }

        #region [ -- Nested Classes -- ]

        private sealed class ColorPickerTable : ASP.Table
        {
            private readonly ColorPicker _owner;

            public ColorPickerTable(ColorPicker owner)
            {
                _owner = owner;
            }

            protected override void AddAttributesToRender(HtmlTextWriter writer)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID);
                base.AddAttributesToRender(writer);
            }
        }

        private sealed class Translator
        {
            public Translator(double left, double top, double value)
            {
                Hue = 360 - (value * 3.6);
                Saturation = left / (Size - HandleSize);
                Value = 1 - (top / (Size - HandleSize));
            }

            public Translator(string htmlColor)
            {
                var color = ColorTranslator.FromHtml(htmlColor);
                Initialize(color);
            }

            public Translator(int r, int g, int b)
            {
                var color = Color.FromArgb(255, r, g, b);
                Initialize(color);
            }

            public Translator(string hue, string saturation, string value)
            {
                Hue = double.Parse(hue);
                Saturation = double.Parse(saturation);
                Value = double.Parse(value);
            }

            private void Initialize(Color color)
            {
                int max = Math.Max(color.R, Math.Max(color.G, color.B));
                int min = Math.Min(color.R, Math.Min(color.G, color.B));

                Hue = color.GetHue();
                Saturation = (max == 0) ? 0 : 1d - (1d * min / max);
                Value = max / 255d;
            }

            public double Hue { get; private set; }
            public double Saturation { get; private set; }
            public double Value { get; private set; }

            public string HtmlColor
            {
                get { return ColorTranslator.ToHtml(GetColor()); }
            }

            public Color GetColor()
            {
                var hueI = Convert.ToInt32(Math.Floor(Hue / 60)) % 6;
                var remainder = Hue / 60 - Math.Floor(Hue / 60);

                var value = Value * 255;
                var v = Convert.ToInt32(value);
                var p = Convert.ToInt32(value * (1 - Saturation));
                var q = Convert.ToInt32(value * (1 - remainder * Saturation));
                var t = Convert.ToInt32(value * (1 - (1 - remainder) * Saturation));

                if (hueI == 0)
                    return Color.FromArgb(255, v, t, p);
                if (hueI == 1)
                    return Color.FromArgb(255, q, v, p);
                if (hueI == 2)
                    return Color.FromArgb(255, p, v, t);
                if (hueI == 3)
                    return Color.FromArgb(255, p, q, v);
                if (hueI == 4)
                    return Color.FromArgb(255, t, p, v);

                return Color.FromArgb(255, v, p, q);
            }
        }

        #endregion

        #region [ -- Build Color Picker -- ]

        private void BuildColorPicker()
        {
            var headerRow = new ASP.TableRow();
            headerRow.Cells.Add(CreateTextBoxCell());
            headerRow.Cells.Add(CreateButtonCell());

            var colorPickerRow = new ASP.TableRow();
            colorPickerRow.Cells.Add(CreateColorPickerCell());

            var table = new ColorPickerTable(this) { CellPadding = 0, CellSpacing = 0, BorderWidth = 0 };
            table.Rows.Add(headerRow);
            table.Rows.Add(colorPickerRow);
            Controls.Add(table);
        }

        private ASP.TableCell CreateTextBoxCell()
        {
            var textBoxCell = new ASP.TableCell();
            _mainTextBox = CreateTextBox(IsColorSelected ? SelectedColor : string.Empty, HtmlValueChanged, string.Empty);
            _mainTextBox.Width = Width;
            _mainTextBox.Height = Height;
            _mainTextBox.Style[HtmlTextWriterStyle.BackgroundColor] = SelectedColor;
            _mainTextBox.Style[HtmlTextWriterStyle.Color] = GetNegativeColor();
            textBoxCell.Controls.Add(_mainTextBox);
            return textBoxCell;
        }

        private ASP.TableCell CreateButtonCell()
        {
            _toggleButton = new Button { Text = "..." };
            _toggleButton.Click += TogglePicker;

            var buttonCell = new ASP.TableCell();
            buttonCell.Controls.Add(_toggleButton);
            return buttonCell;
        }

        private ASP.TableCell CreateColorPickerCell()
        {
            var pickerCell = new ASP.TableCell { ColumnSpan = 2 };
            pickerCell.Style[HtmlTextWriterStyle.ZIndex] = "1000";
            pickerCell.Style[HtmlTextWriterStyle.Position] = "absolute";
            pickerCell.Controls.Add(CreateColorPicker());
            return pickerCell;
        }

        private Panel CreateColorPicker()
        {
            var translator = new Translator(SelectedColor);
            var colorPickerContainer = new Panel
                                        {
                                            Height = Size + 2,
                                            Width = Size + ContainerAdditionalSize,
                                            Visible = ColorPickerVisible,
                                            CssClass = "color-picker-container"
                                        };

            var closeButton = new LinkButton { Text = "x", CssClass = "color-picker-close" };
            closeButton.Click += TogglePicker;

            CreateSlider();
            CreatePicker();

            SetColorPickerPosition(translator);
            SetColorPickerBackgroundColor((360 - translator.Hue) / 3.6);

            CreateResultViewer(translator.GetColor());
            CreateHtmlColorTextBox(SelectedColor);

            _hueTextBox = CreateHsvTextBox(translator.Hue);
            _saturationTextBox = CreateHsvTextBox(translator.Saturation);
            _valueTextBox = CreateHsvTextBox(translator.Value);
            _redTextBox = CreateRgbTextBox(translator.GetColor().R);
            _greenTextBox = CreateRgbTextBox(translator.GetColor().G);
            _blueTextBox = CreateRgbTextBox(translator.GetColor().B);

            colorPickerContainer.Controls.Add(_slider);
            colorPickerContainer.Controls.Add(_colorPicker);
            colorPickerContainer.Controls.Add(closeButton);
            colorPickerContainer.Controls.Add(_resultViewer);
            colorPickerContainer.Controls.Add(_htmlColorTextBox);

            colorPickerContainer.Controls.Add(CreateLabel("HSV:"));
            colorPickerContainer.Controls.Add(CreateLabel("RGB:"));

            colorPickerContainer.Controls.Add(_hueTextBox);
            colorPickerContainer.Controls.Add(_redTextBox);
            colorPickerContainer.Controls.Add(_saturationTextBox);
            colorPickerContainer.Controls.Add(_greenTextBox);
            colorPickerContainer.Controls.Add(_valueTextBox);
            colorPickerContainer.Controls.Add(_blueTextBox);

            return colorPickerContainer;
        }

        private TextBox CreateRgbTextBox(int defaultValue)
        {
            return CreateTextBox(defaultValue.ToString(CultureInfo.InvariantCulture), RgbValueChanged, "output-textbox");
        }

        private TextBox CreateHsvTextBox(double defaultValue)
        {
            return CreateTextBox(defaultValue.ToString(DoubleRepresentationFormat), HsvValueChanged, "output-textbox");
        }

        private void CreateHtmlColorTextBox(string defaultValue)
        {
            _htmlColorTextBox = CreateTextBox(defaultValue, HtmlValueChanged, "html-value");
        }

        private void CreateResultViewer(Color color)
        {
            _resultViewer = new Panel { CssClass = "result-panel" };
            SetResultViewerColor(color);
        }

        private void CreateSlider()
        {
            _slider = new Slider { ID = SliderId, DisplayDirection = Slider.Direction.Vertical, CssClass = "color-picker", Height = Size };
            _slider.ValueChanged += SliderValueChanged;
        }

        private void CreatePicker()
        {
            _colorPicker = new Panel { ID = PickerId, CssClass = "color-picker", Height = Size, Width = Size };
            _colorPicker.Aspects.Add(new AspectClickable(ColorPickerClicked) { UseRelativeCoordinates = true });

            _colorPickerHandle = new Panel
            {
                CssClass = "color-picker-handle"
            };

            var aspectDraggable = new AspectDraggable(null, new Rectangle(0, 0, Size - HandleSize, Size - HandleSize));
            aspectDraggable.Dropping += ColorPickerHandleDropping;
            _colorPickerHandle.Aspects.Add(aspectDraggable);

            _colorPicker.Controls.Add(_colorPickerHandle);
        }

        private static Label CreateLabel(string text)
        {
            return new Label { Text = text, CssClass = "color-picker-label" };
        }

        private static TextBox CreateTextBox(string defaultValue, EventHandler handler, string cssClass)
        {
            var textBox = new TextBox { Text = defaultValue, CssClass = cssClass, AutoPostBack = true };
            textBox.TextChanged += handler;
            return textBox;
        }

        #endregion

        #region [ -- Event Handlers -- ]

        public void TogglePicker(object sender, EventArgs e)
        {
            ColorPicker picker = null;
            foreach (var control in GetAllControls(Page.Controls))
            {
                if (control is ColorPicker && ((ColorPicker)control).ColorPickerVisible)
                    picker = (ColorPicker)control;
            }

            if (picker != null && picker != this)
                picker.ColorPickerVisible = false;

            ColorPickerVisible = !ColorPickerVisible;
        }

        void ColorPickerHandleDropping(object sender, AspectDraggable.DroppingEventArgs e)
        {
            var translator = new Translator(e.Position.X, e.Position.Y, _slider.Value);
            ApplyChanges(translator);
        }

        void ColorPickerClicked(object sender, AspectClickable.ClickEventArgs e)
        {
            var leftMax = _colorPicker.Width.Value - HandleSize;
            var topMax = _colorPicker.Height.Value - HandleSize;
            const int halfHandleSize = HandleSize / 2;

            var left = e.Left - halfHandleSize;
            var top = e.Top - halfHandleSize;

            if (left > leftMax)
                left = (int)leftMax;

            if (top > topMax)
                top = (int)(topMax);

            if (e.Left - HandleSize < 0)
                left = 0;

            if (e.Top - HandleSize < 0)
                top = 0;

            _colorPickerHandle.Style[HtmlTextWriterStyle.Left] = ASP.Unit.Pixel(left).ToString();
            _colorPickerHandle.Style[HtmlTextWriterStyle.Top] = ASP.Unit.Pixel(top).ToString();

            var translator = new Translator(left, top, _slider.Value);
            ApplyChanges(translator);
        }

        void SliderValueChanged(object sender, EventArgs e)
        {
            SetColorPickerBackgroundColor(_slider.Value);
            var translator = new Translator(ASP.Unit.Parse(_colorPickerHandle.Style["left"]).Value, ASP.Unit.Parse(_colorPickerHandle.Style["top"]).Value, _slider.Value);
            ApplyChanges(translator);
        }

        void HtmlValueChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var translator = new Translator(textBox.Text);
            ApplyChanges(translator);
            SetColorPickerBackgroundColor((360 - translator.Hue) / 3.6);
            SetColorPickerPosition(translator);
        }

        void RgbValueChanged(object sender, EventArgs e)
        {
            var translator = new Translator(int.Parse(_redTextBox.Text), int.Parse(_greenTextBox.Text), int.Parse(_blueTextBox.Text));
            ApplyChanges(translator);
            SetColorPickerBackgroundColor((360 - translator.Hue) / 3.6);
            SetColorPickerPosition(translator);
        }

        void HsvValueChanged(object sender, EventArgs e)
        {
            var translator = new Translator(_hueTextBox.Text, _saturationTextBox.Text, _valueTextBox.Text);
            ApplyChanges(translator);
            SetColorPickerBackgroundColor((360 - translator.Hue) / 3.6);
            SetColorPickerPosition(translator);
        }

        #endregion

        #region [ -- Helpers -- ]

        private void ApplyChanges(Translator translator)
        {
            var color = translator.GetColor();
            SelectedColor = translator.HtmlColor;
            _mainTextBox.Text = translator.HtmlColor;
            _mainTextBox.Style[HtmlTextWriterStyle.BackgroundColor] = translator.HtmlColor;
            _mainTextBox.Style[HtmlTextWriterStyle.Color] = GetNegativeColor();
            _htmlColorTextBox.Text = translator.HtmlColor;

            _redTextBox.Text = color.R.ToString(CultureInfo.InvariantCulture);
            _greenTextBox.Text = color.G.ToString(CultureInfo.InvariantCulture);
            _blueTextBox.Text = color.B.ToString(CultureInfo.InvariantCulture);

            _hueTextBox.Text = translator.Hue.ToString(DoubleRepresentationFormat);
            _saturationTextBox.Text = translator.Saturation.ToString(DoubleRepresentationFormat);
            _valueTextBox.Text = translator.Value.ToString(DoubleRepresentationFormat);

            SetResultViewerColor(color);

            if (ColorChanged == null) return;
            ColorChanged(this, EventArgs.Empty);
        }

        private string GetNegativeColor()
        {
            var color = ColorTranslator.FromHtml(SelectedColor);
            var result = color.R < ForeColorChangePoint && color.G < ForeColorChangePoint && color.B < ForeColorChangePoint ? Color.White : Color.Black;
            return ColorTranslator.ToHtml(result);
        }

        private void SetResultViewerColor(Color color)
        {
            _resultViewer.Style[HtmlTextWriterStyle.BackgroundColor] = string.Format("rgb({0}, {1}, {2})", color.R, color.G, color.B);
        }

        private void SetColorPickerBackgroundColor(double hueValue)
        {
            _colorPicker.Style[HtmlTextWriterStyle.BackgroundColor] = GetColorPickerBackgroundColor(hueValue);
        }

        private void SetColorPickerPosition(Translator translator)
        {
            _slider.Value = (360 - translator.Hue) / 3.6;

            _colorPickerHandle.Style["top"] = ASP.Unit.Pixel(Convert.ToInt32((1 - translator.Value) * (Size - HandleSize))).ToString();
            _colorPickerHandle.Style["left"] = ASP.Unit.Pixel(Convert.ToInt32(translator.Saturation * (Size - HandleSize))).ToString();
        }

        private static string GetColorPickerBackgroundColor(double value)
        {
            var hue = value * 3.6;
            var hueI = Convert.ToInt32(Math.Floor(hue / 60)) % 6;

            var rgbColor = "rgb(255, {0}, 0)";

            if (Math.Abs(value - 100.0) < 0.000000000000000000000000009)
                hueI = 5;

            if (hueI == 0)
                rgbColor = "rgb(255, 0, {0})";
            if (hueI == 1)
                rgbColor = "rgb({0}, 0, 255)";
            if (hueI == 2)
                rgbColor = "rgb(0, {0}, 255)";
            if (hueI == 3)
                rgbColor = "rgb(0, 255, {0})";
            if (hueI == 4)
                rgbColor = "rgb({0}, 255, 0)";

            return string.Format(rgbColor, ComputeBackgroundColor(value, hueI));
        }

        private static double ComputeBackgroundColor(double value, int hueI)
        {
            value = Math.Round(255 * (value * 6 / 100 - hueI));
            return hueI % 2 == 0 ? value : (255 - value);
        }

        private static IEnumerable<Control> GetAllControls(ControlCollection controls)
        {
            foreach (Control proxy in controls)
            {
                foreach (var grandchild in GetAllControls(proxy.Controls))
                    yield return grandchild;

                yield return proxy;
            }
        }

        #endregion

        #region [ -- IAjaxControl and IAjaxContainerControl implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AjaxContainerControl(this)); }
        }

        AspectCollection IAspectableAjaxControl.Aspects
        {
            get { return AjaxContainerControl.Aspects; }
        }

        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return AjaxContainerControl; }
        }

        void IAjaxContainerControl.ForceAnUpdate()
        {
            AjaxContainerControl.ForceAnUpdate();
        }

        void IAjaxContainerControl.ForceAnUpdateWithAppending()
        {
            throw new NotSupportedException();
        }

        void IAjaxContainerControl.TrackControlAdditions()
        {
            throw new NotSupportedException();
        }

        void IAjaxContainerControl.RenderChildrenOnForceAnUpdate(XhtmlTagFactory create)
        {
            throw new NotSupportedException();
        }

        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return ClientID;
        }

        AjaxContainerControl IAjaxContainerControl.AjaxContainerControl
        {
            get { return (AjaxContainerControl)((IAjaxContainerControl)this).AjaxControl; }
        }

        private AjaxContainerControl AjaxContainerControl
        {
            get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl)this).AjaxContainerControl); }
        }

        #endregion
    }
}