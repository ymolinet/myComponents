/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using ASP = System.Web.UI;

[assembly: ASP.WebResource("Gaia.WebWidgets.CommunityExtensions.Scripts.SpeedDetector.js", "text/javascript")]

namespace Gaia.WebWidgets.CommunityExtensions
{
    /// <summary>
    /// SpeedDetector is an example on how you can extend Gaia Ajax with your own custom Extension. The SpeedDetector
    /// detects your network connection by downloading dummy data and by performing some simple calculation on the time
    /// spent we can try som approximate meassure of the bandwith. 
    /// </summary>
    [DefaultEvent("SpeedDetectionComplete")]
    [ASP.ToolboxData("<{0}:SpeedDetector runat=\"server\" />")]
    [ToolboxBitmap(typeof(SpeedDetector), "Resources.Gaia.WebWidgets.CommunityExtensions.SpeedDetector.bmp")]
    public class SpeedDetector : GaiaControl, IAjaxControl
    {
        #region [-- Private Members --]

        private const string Lorem = @"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. 
Quisque ac turpis. Nam scelerisque ante non ante. Sed pede. Sed lobortis. Vivamus fringilla. 
Morbi vehicula elit. Donec congue lectus et nunc. Mauris diam. Pellentesque habitant 
morbi tristique senectus et netus et malesuada fames ac turpis egestas. 
Nullam at nulla quis sem luctus tempus. Nam purus ligula, adipiscing eu, congue non, mattis at, dui. 
Curabitur viverra, leo sit amet rhoncus laoreet, eros metus tincidunt erat, eu lobortis nunc risus eu ipsum. 
Fusce vitae mauris. Donec tincidunt ullamcorper dolor.";

        #endregion

        #region [-- Properties --]

        /// <summary>
        /// Specify a url to a file that will be downloaded using HTTP GET. Specify this option if you have an existing 
        /// File that the client will download. If you set this property, you cannot set the DownloadSize property
        /// </summary>
        public string DownloadFile { get; set; }

        /// <summary>
        /// Specify a size (in bytes) of how much lorem ipsum text you want generated over the wire. The SpeedDetector control
        /// will render the content for you. 
        /// </summary>
        public int DownloadSize { get; set; }

        private bool IsLoremIpsumRequest
        {
            get
            {
                return (!DesignMode &&
                        Page.Request.Params["Gaia.WebWidgets.CommunityExtensions.SpeedDetector.GetLoremIpsum"] == ClientID);
            }
        } 
        #endregion

        #region [-- Events --]
        /// <summary>
        /// Custom Event class forwarded to the client once the SpeedDetector has downloaded the content. The time it took
        /// is specified in the TimeSpent property
        /// </summary>
        public class DetectionCompleteEventArgs : EventArgs
        {
            private readonly TimeSpan _timeSpent;

            /// <summary>
            /// Specifies the time it took to download the file as a timespan
            /// </summary>
            public TimeSpan TimeSpent
            {
                get { return _timeSpent; }
            }

            internal DetectionCompleteEventArgs(TimeSpan timeSpent)
            {
                _timeSpent = timeSpent;
            }
        }

        /// <summary>
        /// This event is fired when the SpeedDetector has downloaded the specified content
        /// </summary>
        public event EventHandler<DetectionCompleteEventArgs> SpeedDetectionComplete;

        #endregion

        #region [-- AjaxSerializable Methods --]

        public void Start()
        {
            StartDetection = true;
        }

        [AjaxSerializable("startDetection")]
        internal bool StartDetection { get; private set; }

        [Method]
        internal void SpeedDetectionCompleteMethod(string timeSpent)
        {
            if (SpeedDetectionComplete != null)
            {
                SpeedDetectionComplete(this, new DetectionCompleteEventArgs(new TimeSpan(0, 0, 0, 0, int.Parse(timeSpent))));

            }
        }

        #endregion

        #region [-- Helper Methods --]
        /// <summary>
        /// This function is used to generate lorem ipsum text that will be sent over the wire. 
        /// </summary>
        private void WriteLoremIpsum()
        {
            bool sizeSpecified = !String.IsNullOrEmpty(Page.Request.Params["DownloadSize"]);
            double amount = sizeSpecified ? double.Parse(Page.Request.Params["DownloadSize"]) : 1000000;

            double times = Math.Round(amount / Lorem.Length);

            try
            {
                Page.Response.Clear();
                Page.Response.ContentType = "text/plain";
                Page.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Page.Response.Buffer = false;


                using (var writer = new StreamWriter(Page.Response.OutputStream))
                {
                    for (int i = 0; i < times; i++)
                        writer.Write(Lorem);
                }

                Page.Response.End();
            }
            catch (System.Threading.ThreadAbortException) // do nothing
            {
            }
        }


        #endregion

        #region [ -- Overridden Base class methods -- ]

        /// <summary>
        /// Core Control Initialization
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            bool onlyOneAlternativeSpecified = !String.IsNullOrEmpty(DownloadFile) ^ DownloadSize > 0;

            if (!onlyOneAlternativeSpecified)
                throw new Exception("You can only specify either a file to download or a given size to download. Not both.");

            if (IsLoremIpsumRequest)
                WriteLoremIpsum();
            else
                base.OnInit(e);
        }

        /// <summary>
        /// Renders a simple tag that acts as a placeholder for the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory</param>
        protected override void RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
        {
            using (create.Div(ClientID)) { }
        }


        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // Include javascript
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.prototype.js", typeof(SpeedDetector), "");
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.SpeedDetector.js", 
                typeof(SpeedDetector), "Gaia.Extensions.SpeedDetector.browserFinishedLoading");

        }
        #endregion

        #region [ -- Overridden IAjaxControl methods -- ]

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Extensions.SpeedDetector", ClientID)
                .AddPropertyIfTrue(DownloadSize > 0, "downloadSize", DownloadSize)
                .AddPropertyIfTrue(StartDetection, "startDetection", StartDetection)
                .AddPropertyIfTrue(!String.IsNullOrEmpty(DownloadFile), "downloadFile", DownloadFile).ToString();
        }

        #endregion
    }
}
