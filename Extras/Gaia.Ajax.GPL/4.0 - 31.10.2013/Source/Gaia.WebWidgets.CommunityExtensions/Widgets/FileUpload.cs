/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

/*
 * **************************************************************
 * Author : Pavol Rusanov, Czech 
 * **************************************************************
*/

using System;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.CommunityExtensions.Resources.blank.htm", "text/html")]
[assembly: WebResource("Gaia.WebWidgets.CommunityExtensions.Scripts.FileUpload.js", "text/javascript")]

namespace Gaia.WebWidgets.CommunityExtensions
{
    using HtmlFormatting;

    /// <summary>
    /// Ajax FileUpload control that allows you to upload many files with ajax technology. The Control
    /// was created by community member Pavol. 
    /// </summary>
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ToolboxData("<{0}:FileUpload runat=\"server\" />")]
    [ToolboxBitmap(typeof(FileUpload), "Resources.Gaia.WebWidgets.CommunityExtensions.FileUpload.bmp")]
    public class FileUpload : Panel, IAjaxControl, IExtraPropertyCallbackRenderer
    {
        #region [ -- Private Members -- ]
        private bool _enabledChanged;
        private string _imgLoadingSrc = string.Empty;
        private bool _makeCallback;
        private int _maxFiles = 1;
        private string _textBtnUpload = "Save";
        private string _textDelete = "Delete";
        private string _textError = "Upload Failed";
        private string _textHeader = "Files to Upload";
        private string _textSizeError = "File Size Exceeded";
        private string _textUploaded = "Saved";
        private string _textUploading = "Uploading ...";
        private bool _uploadOnce;
        private readonly List<ASP.FileUpload> _listOfFileUploadControls = new List<ASP.FileUpload>();
        private string _blankPath = string.Empty;
        private readonly Button _callbackButton = new Button();
        private readonly HtmlGenericControl _uploadButton = new HtmlGenericControl("input");
        private readonly HiddenField _hiddenField = new HiddenField();
        private readonly HtmlGenericControl _iFrame = new HtmlGenericControl("iframe"); 
        #endregion

        #region [ -- Public Properties -- ]

        /// <summary>
        /// Display text while uploading files
        /// </summary>
        [Browsable(true)]
        [DefaultValue("Uploading ...")]
        public string TextUploading
        {
            get { return _textUploading; }
            set { _textUploading = value; }
        }

        /// <summary>
        /// Display text when file has been successfully uploaded
        /// </summary>
        [Browsable(true)]
        [DefaultValue("Saved")]
        public string TextUploaded
        {
            get { return _textUploaded; }
            set { _textUploaded = value; }
        }

        /// <summary>
        /// Display text when some error on the server occured.
        /// </summary>
        [Browsable(true)]
        [DefaultValue("Upload Failed")]
        public string TextError
        {
            get { return _textError; }
            set { _textError = value; }
        }

        /// <summary>
        /// Display text when file size has exceeded
        /// </summary>
        [Browsable(true)]
        [DefaultValue("File Size Exceeded")]
        public string TextSizeError
        {
            get { return _textSizeError; }
            set { _textSizeError = value; }
        }

        /// <summary>
        /// Display text for header
        /// </summary>
        [Browsable(true)]
        [DefaultValue("Files to Upload")]
        public string TextHeader
        {
            get { return _textHeader; }
            set { _textHeader = value; }
        }

        /// <summary>
        /// Save Button Text
        /// </summary>
        [Browsable(true)]
        [DefaultValue("Save")]
        public string TextBtnUpload
        {
            get { return _textBtnUpload; }
            set { _textBtnUpload = value; }
        }

        /// <summary>
        /// Delete Button Text
        /// </summary>
        [Browsable(true)]
        [DefaultValue("Delete")]
        public string TextDelete
        {
            get { return _textDelete; }
            set { _textDelete = value; }
        }

        /// <summary>
        /// How many files possible to upload at one moment
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1)]
        public int MaxFiles
        {
            get { return _maxFiles; }
            set { _maxFiles = value; }
        }

        /// <summary>
        /// Set to true if you want the files only once and then the control becomes disabled
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool UploadOnce
        {
            get { return _uploadOnce; }
            set { _uploadOnce = value; }
        }

        /// <summary>
        /// Set to true if you want a callback after all files has been uploaded to the server.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool MakeCallback
        {
            get { return _makeCallback; }
            set { _makeCallback = value; }
        }

        /// <summary>
        /// Source of the image loading.
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        public string ImgLoadingSrc
        {
            get { return _imgLoadingSrc; }
            set { _imgLoadingSrc = value; }
        }

        /// <summary>
        /// Enabled overridden property
        /// </summary>
        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if (base.Enabled != value)
                    _enabledChanged = true;
                base.Enabled = value;
            }
        }

        #endregion

        #region [ -- FileUploadEventArgs -- ]

        /// <summary>
        /// EventArgs used for the FileUploaded Event. 
        /// </summary>
        public class FileUploadEventArgs : EventArgs
        {
            public FileUploadEventArgs(HttpPostedFile postedFile, string text)
            {
                PostedFile = postedFile;
                InfoText = text;
            }

            /// <summary>
            /// The Posted File
            /// </summary>
            public HttpPostedFile PostedFile { get; set; }

            /// <summary>
            /// Info Text
            /// </summary>
            public string InfoText { get; set; }
        }

        #endregion

        #region [ -- Event Handlers -- ]

        /// <summary>
        /// This Event is fired when the file file has been uploaded
        /// </summary>
        public event EventHandler<FileUploadEventArgs> FileUploaded;

        /// <summary>
        /// This Event is fired when all files are completely uploaded. You need to set the
        /// MakeCallback property to true for this event to fire. 
        /// </summary>
        public event EventHandler UploadFinished;

        #endregion

        #region [ -- Overriden base class methods --]

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            EnsureChildControls();
        }

        private void InitializeComponent()
        {
            _iFrame.ID = "upFrame";
            Controls.Add(_iFrame);

            _iFrame.Attributes.Add("name", _iFrame.ClientID);
            _iFrame.Attributes.Add("frameborder", "0");
            _iFrame.Attributes.Add("scrolling", "no");
            _iFrame.Style["display"] = "none";

            for (var i = 0; i < _maxFiles; i++)
            {
                var fu = new ASP.FileUpload {ID = "f" + i};
                fu.Style["display"] = "none";
                _listOfFileUploadControls.Add(fu);
            }

            _uploadButton.ID = "btnUP";
            _uploadButton.Attributes.Add("type", "submit");
            _uploadButton.Attributes.Add("value", _textBtnUpload);

            _callbackButton.ID = "btnC";
            _callbackButton.Style["display"] = "none";
            _callbackButton.Click += BtnCallbackClick;

            _hiddenField.ID = "hf";
            _hiddenField.Value = "0";
        }

        protected override void CreateChildControls()
        {
            _iFrame.Controls.Add(new LiteralControl("&nbsp;"));

            foreach (ASP.FileUpload fileUpload in _listOfFileUploadControls)
                Controls.Add(fileUpload);

            Controls.Add(_uploadButton);
            Controls.Add(_callbackButton);
            Controls.Add(_hiddenField);

            base.CreateChildControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // very important attribute. The postback is targeted into blank.html window, which is embedded resource.
            _blankPath = Page.ClientScript.GetWebResourceUrl(typeof(FileUpload), "Gaia.WebWidgets.CommunityExtensions.Resources.blank.htm");
            _iFrame.Attributes.Add("src", _blankPath);

            CheckIfFileUploaded();
        }

        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.prototype.js", typeof (FileUpload), "");
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.FileUpload.js", typeof(FileUpload), "Gaia.Extensions.FileUpload.browserFinishedLoading");
        }

        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (Tag div = create.Div(ClientID, CssClass))
            {
                Css.SerializeAttributesAndStyles(this, div);
                _iFrame.RenderControl(create.GetHtmlTextWriter());

                for (int i = 0; i < _listOfFileUploadControls.Count; i++)
                    _listOfFileUploadControls[i].RenderControl(create.GetHtmlTextWriter());

                _uploadButton.RenderControl(create.GetHtmlTextWriter());
                _callbackButton.RenderControl(create.GetHtmlTextWriter());
                _hiddenField.RenderControl(create.GetHtmlTextWriter());
                using (create.Div(ClientID + "inner", CombineCssClass("fileupload-inner")))
                {
                    using (Tag span = create.Span(ClientID + "inner-header", CombineCssClass("fileupload-inner-header")))
                    {
                        span.WriteContent(_textHeader);
                    }
                }
            }
        }

        #endregion

        #region [ -- Helper Functions -- ]

        private void CheckIfFileUploaded()
        {
            string hfValue = Page.Request.Form[_hiddenField.UniqueID];
            if (string.IsNullOrEmpty(hfValue) || hfValue != "1") return;
            string text = _textError;
            for (int i = 0; i < Page.Request.Files.Count; i++)
            {
                if (string.IsNullOrEmpty(Page.Request.Files[i].FileName) || Page.Request.Files[i].ContentLength <= 0)
                    continue;

                var args = new FileUploadEventArgs(Page.Request.Files[i], null);
                if (FileUploaded != null)
                    FileUploaded(this, args);

                text = string.IsNullOrEmpty(args.InfoText) ? _textUploaded : args.InfoText;
                break;
            }

            Page.Title = "upload|" + text;
            Page.Form.Visible = false;
        }

        /// <summary>
        /// After all files has been uploaded you can catch UploadFinished event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCallbackClick(object sender, EventArgs e)
        {
            if (UploadFinished != null)
                UploadFinished(this, EventArgs.Empty);
        }

        private string CombineCssClass(params string[] cssclass)
        {
            return Css.Combine(CssClass, cssclass);
        }

        #endregion

        #region [ -- Overridden IAjaxControl Methods -- ]

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerWebControl(this, ClientID, this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Extensions.FileUpload", ClientID)
                .AddProperty("enabled", Enabled)
                .AddProperty("frameID", _iFrame.ClientID)
                .AddProperty("btnID", _uploadButton.ClientID)
                .AddProperty("hfID", _hiddenField.ClientID)
                .AddProperty("tUploading", _textUploading)
                .AddProperty("tSizeError", _textSizeError)
                .AddProperty("tDelete", _textDelete)
                .AddProperty("maxFiles", _maxFiles)
                .AddProperty("uploadOnce", _uploadOnce)
                .AddProperty("divFilesID", ClientID + "inner")
                .AddProperty("imgSrc", _imgLoadingSrc)
                .AddProperty("btnCallbackID", _callbackButton.ClientID)
                .AddProperty("makeCallback", _makeCallback)
                .AddPropertyIfTrue(!string.IsNullOrEmpty(_blankPath), "blankPath", _blankPath)
                .ToString();
        }

        #endregion

        #region IExtraPropertyCallbackRenderer Members

        public void InjectPropertyChangesToCallbackResponse(StringBuilder code)
        {
            if (_enabledChanged)
            {
                code.Append(".onEnabledChanged(" + Enabled.ToString().ToLower() + ")");
            }
        }

        #endregion

    }
}