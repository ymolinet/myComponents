namespace Gaia.WebWidgets.Samples
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Collections.Generic;
    using ASP = System.Web.UI.WebControls;

    using Utilities;
    using WebWidgets.Effects;
    using WebWidgets.Extensions;
    using Manoli.Utils.CSharpFormat;

    public partial class GaiaCoreMaster : MasterPage
    {
        private List<string> _additionalScriptFilesForInclusion;
        private static readonly string[] IgnoreFiles = new[] {"default.aspx.cs", "default.aspx.designer.cs"};
        
        /// <summary>
        /// Constructor. We set the id of the master to m to keep ids shorter. 
        /// </summary>
        public GaiaCoreMaster() { base.ID = "m"; }

        protected override void OnInit(EventArgs e)
        {
            // Load the xml documentation files for Gaia.WebWidgets.dll + Gaia.WebWidgets.Extensions.dll
            XmlCodeDocumentation.EnsureDocumentLoaded(typeof(Manager).Assembly);
            XmlCodeDocumentation.EnsureDocumentLoaded(typeof(Window).Assembly);
            XmlCodeDocumentation.EnsureDocumentLoaded(typeof(EffectAppear).Assembly);

            // initialize the sample here, get from cached static SampleConfigurationPerPage if present
            var sampleConfiguration = GetSampleConfigurationForPage();

            var sections = SampleConfiguration.GetCachedChildrenMenuItems("~/", true);
            InsertSectionAndChildLinks(sections, sampleConfiguration);

            if (sampleConfiguration.IsValidSample)
            {
                //render additional tabs for sample files
                AddMoreTabViewsForSampleFiles(sampleConfiguration.SampleFiles);

                // the following stuff is not required in ajax callbacks, only full rendering. 
                if (!Manager.Instance.IsAjaxCallback)
                {
                    // Add styles.css if the styles.css file exists in the sample
                    var sampleCssFile = sampleConfiguration.AbsoluteDirectory + "/styles.css";
                    if (File.Exists(MapPath(sampleCssFile)))
                        Manager.Instance.AddInclusionOfStyleSheet(sampleCssFile);

                    // Include custom scripts located in the scripts folder beneath the current sample
                    string scriptsFolder = MapPath(sampleConfiguration.AbsoluteDirectory + "/scripts/");
                    if (Directory.Exists(scriptsFolder))
                    {
                        string[] jsFiles = Directory.GetFiles(scriptsFolder, "*.js");

                        if (jsFiles.Length > 0)
                        {
                            if (_additionalScriptFilesForInclusion == null)
                                _additionalScriptFilesForInclusion = new List<string>();

                            foreach (string file in jsFiles)
                                _additionalScriptFilesForInclusion.Add(new FileInfo(file).Name);
                        }
                    }
                }
            }

            //display first code tab
            if (!IsPostBack)
            {
                zVersion.Text = GetGaiaAjaxVersion();

                tabCntrlCode.Visible = sampleConfiguration.IsValidSample;

                if (sampleConfiguration.IsValidSample)
                    ShowCode(tabCntrlCode.ActiveTabViewIndex);

                const string pageTitleStart = "Gaia Ajax 4.0 Samples for ASP.NET";

                if (sampleConfiguration.IsValidSample)
                {
                    var sampleTitle = sampleConfiguration.FileName.ToLower() == "overview"
                                           ? sampleConfiguration.ParentMenuItem.Parent.Title
                                           : sampleConfiguration.ParentMenuItem.Title;

                    Page.Title = string.Format("{0} - {1}", sampleTitle, pageTitleStart);
                }
                else
                {
                    Page.Title = pageTitleStart;
                }
            }

            base.OnInit(e);
        }

        private static string GetGaiaAjaxVersion()
        {
            return System.Reflection.Assembly.GetAssembly(typeof (Button)).GetName().Version.ToString();
        }

        protected override void OnPreRender(EventArgs e)
        {
            //if we don't have any content or controls within the sampleAspx section, we skip this + tabcontrol
            if (WebUtility.IsControlEmpty(p))
                zCodeContainer.Visible = tabCntrlCode.Visible = false;

            base.OnPreRender(e); 

            if (Manager.Instance.IsAjaxCallback)
                return;

            // make sure .js scripts are rendered at the end ...
            Page.PreRenderComplete += delegate { IncludeCustomJavaScriptFiles(); };

        }

        /// <summary>
        /// If custom JavaScript files are located in folder named scripts beneath the current sample, they
        /// are automatically included at the very end of the page. This is a useful feature in debugging as
        /// we can override the core script files in Gaia Ajax by just placing a copy of them here ... 
        /// Happy debugging :-) 
        /// </summary>
        private void IncludeCustomJavaScriptFiles()
        {
            if (_additionalScriptFilesForInclusion == null || _additionalScriptFilesForInclusion.Count == 0)
                return;

            foreach (string scriptFile in _additionalScriptFilesForInclusion)
                Page.ClientScript.RegisterClientScriptInclude(scriptFile, "scripts/" + scriptFile);

        }

        private static SampleConfiguration GetSampleConfigurationForPage()
        {
            return
                SampleConfigurationController.GetSampleConfigurationForPath(
                    WebUtility.AppRelativeCurrentExecutionFilePath);
        }

        private void AddMoreTabViewsForSampleFiles(IEnumerable<SampleConfiguration.SampleFile> sampleFiles)
        {
            foreach (var file in sampleFiles)
            {
                // We skip files that are part of our IgnoreFilter 
                var fileA = file.Name.ToLower();
                var shouldIgnore = Array.Exists(IgnoreFiles, fileB => fileA == fileB);

                if (shouldIgnore) continue;

                var tabView = new TabView
                                  {
                                      Caption = file.Name,
                                      CaptionImageCssClass = GetCssClassForSampleFileType(file)
                                  };
                tabCntrlCode.TabViews.Add(tabView);
            }
        }

        private static string GetCssClassForSampleFileType(SampleConfiguration.SampleFile sampleFile)
        {
            switch (sampleFile.FileType)
            {
                case SampleConfiguration.SampleFile.FileTypeEnum.Cs:
                    return "csharpTabControlIcon";
                case SampleConfiguration.SampleFile.FileTypeEnum.Vb:
                    return "vbTabControlIcon";
                case SampleConfiguration.SampleFile.FileTypeEnum.Ascx:
                    return "ascxTabControlIcon";
                case SampleConfiguration.SampleFile.FileTypeEnum.Css:
                    return "cssTabControlIcon";
                case SampleConfiguration.SampleFile.FileTypeEnum.Xml:
                    return "xmlTabControlIcon";
                case SampleConfiguration.SampleFile.FileTypeEnum.Js:
                    return "jsTabControlIcon";
            }

            return null;
        }

        private void InsertSectionAndChildLinks(IEnumerable<SampleConfiguration.SampleMenuItemBase> sampleMenuItems, SampleConfiguration sampleConfiguration)
        {
            foreach (var menuItem in sampleMenuItems)
            {
                var hyperLink = new ASP.HyperLink
                                    {
                                        Text = menuItem.Title,
                                        NavigateUrl = ResolveUrl(menuItem.AbsolutePath),
                                        CssClass = "section-link",
                                        EnableViewState = false
                                    };

                pS.Controls.Add(hyperLink);

                var isCurrentSection = menuItem.IsCurrentSection(sampleConfiguration);

                if (isCurrentSection && sampleConfiguration.IsValidSample)
                    pS.Controls.Add(GetCurrentSectionContainerWithChildren(sampleConfiguration));
            }

        }

        private static ASP.Panel GetCurrentSectionContainerWithChildren(SampleConfiguration sampleConfiguration)
        {
            var t = new ASP.Panel {CssClass = "section-container"};

            foreach (var subSectionMenuItem in sampleConfiguration.ParentMenuItem.Parent.Parent.Children)
            {
                var child = new ASP.Panel {CssClass = "section-container-child"};
                
                //skip empty menus
                if (subSectionMenuItem.Children == null || subSectionMenuItem.Children.Count == 0) continue;

                //mark as where we are and make current open
                var isCurrent = sampleConfiguration.ParentMenuItem.Parent.AbsolutePath ==
                                subSectionMenuItem.AbsolutePath;

                //determine if we one or many samples, if one show link directly, if more than one, show folder
                var isSingleChildSample = subSectionMenuItem.Children.Count == 1;

                if (isSingleChildSample || !isCurrent)
                {
                    var singleOrFirstLink = GetSubSamplesLinks(subSectionMenuItem, true, sampleConfiguration).First();
                    child.Controls.Add(singleOrFirstLink);
                }
                else
                {
                    child.CssClass += " has-children";

                    var lblHeader = new ASP.Label {Text = subSectionMenuItem.Title};
                    child.Controls.Add(lblHeader);

                    //render submenu
                    foreach (var item in GetSubSamplesLinks(subSectionMenuItem, false, sampleConfiguration))
                        child.Controls.Add(item);
                }

                t.Controls.Add(child);
            }

            return t;
        }

        private static IEnumerable<ASP.HyperLink> GetSubSamplesLinks(SampleConfiguration.SampleMenuItemBase menuItem, bool useParentName, SampleConfiguration currentSample)
        {
            foreach (var sampleMenuItemBase in menuItem.Children)
            {
                var link = new ASP.HyperLink {EnableViewState = false};
                if (menuItem.AbsolutePath == sampleMenuItemBase.AbsolutePath)
                    link.Text = @"-> ";
                link.Text += useParentName ? menuItem.Title : sampleMenuItemBase.Title;

                link.NavigateUrl = sampleMenuItemBase.AbsolutePath;

                //mark current sample
                if (currentSample.AbsolutePath.Contains(sampleMenuItemBase.AbsolutePath))
                {
                    link.CssClass = "sample-current";
                }

                yield return link;
            }
        }

        protected void ActiveCodeViewChanged(object sender, EventArgs e)
        {
            ShowCode(tabCntrlCode.ActiveTabViewIndex);
        }

        //TODO refactor this to check for aspx,cs, css, automatically, not hardcoded
        private void ShowCode(int view)
        {
            switch (view)
            {
                case 0:
                    var aspxUrl = WebUtility.AppRelativeCurrentExecutionFilePath;
                    var aspxLiteral = SampleConfigurationController.SourceFiles.ContainsKey(aspxUrl)
                                              ? SampleConfigurationController.SourceFiles[aspxUrl]
                                              : LoadAspx(aspxUrl);

                    aspxcodebehind.Controls.Clear();
                    aspxcodebehind.Controls.Add(aspxLiteral);
                    aspxcodebehind.ForceAnUpdate();
                    break;
                case 1:
                    var csUrl = WebUtility.AppRelativeCurrentExecutionFilePath + ".cs";
                    var csLiteral = SampleConfigurationController.SourceFiles.ContainsKey(csUrl)
                                            ? SampleConfigurationController.SourceFiles[csUrl]
                                            : LoadCSharpCodebehind(csUrl);

                    cscodebehind.Controls.Clear();
                    cscodebehind.Controls.Add(csLiteral);
                    cscodebehind.ForceAnUpdate();
                    break;
                case 2:
                    var vbUrl = WebUtility.AppRelativeCurrentExecutionFilePath + ".cs.vb";
                    var vbLiteral = SampleConfigurationController.SourceFiles.ContainsKey(vbUrl)
                                            ? SampleConfigurationController.SourceFiles[vbUrl]
                                            : LoadVbCodebehind(WebUtility.AppRelativeCurrentExecutionFilePath + ".cs");
                    vbcodebehind.Controls.Clear();
                    vbcodebehind.Controls.Add(vbLiteral);
                    vbcodebehind.ForceAnUpdate();
                    break;
                default: 
                    //url should be present in Caption
                    var currentTabView = tabCntrlCode.TabViews[view];
                    var fileUrl = currentTabView.Caption;
                    ASP.Literal literal = null;

                    if (fileUrl.EndsWith(".css"))
                        literal = SampleConfigurationController.SourceFiles.ContainsKey(fileUrl)
                                      ? SampleConfigurationController.SourceFiles[fileUrl]
                                      : LoadCss(fileUrl);
                    else if (fileUrl.EndsWith(".js"))
                        literal = SampleConfigurationController.SourceFiles.ContainsKey(fileUrl)
                                      ? SampleConfigurationController.SourceFiles[fileUrl]
                                      : LoadJavaScript(fileUrl);
                    else if (fileUrl.EndsWith(".cs"))
                        literal = SampleConfigurationController.SourceFiles.ContainsKey(fileUrl)
                                      ? SampleConfigurationController.SourceFiles[fileUrl]
                                      : LoadCSharpCodebehind(fileUrl);
                    else if (fileUrl.EndsWith(".ascx"))
                        literal = SampleConfigurationController.SourceFiles.ContainsKey(fileUrl)
                                      ? SampleConfigurationController.SourceFiles[fileUrl]
                                      : LoadAscx(fileUrl);

                    if (literal != null)
                    {
                        currentTabView.Controls.Clear();
                        var wrapper = new Panel {CssClass = "codePanel"};
                        wrapper.Controls.Add(literal);
                        
                        currentTabView.Controls.Add(wrapper);
                        currentTabView.ForceAnUpdate();
                    }
                    break;
            }
        }

        static ASP.Literal BuildLiteral(string url, string content)
        {
            var lit = new ASP.Literal {Text = content, EnableViewState = false};

            //note: consider wheter it makes any sense to cache these items statically
            if (!SampleConfigurationController.SourceFiles.ContainsKey(url))
                SampleConfigurationController.SourceFiles.Add(url, lit);

            return lit;
        }

        private ASP.Literal LoadAspx(string url)
        {            
            string aspx = CodeVisualizationUtils.Colorize(new HtmlFormat(), ParseAspxContentPlaceHolder(GetFileContent(url)));
            return BuildLiteral(url, string.Format("<div class=\"xml\">{0}</div>", aspx));
        }

        private ASP.Literal LoadAscx(string url)
        {
            string ascx = CodeVisualizationUtils.Colorize(new HtmlFormat(), GetFileContent(url));
            return BuildLiteral(url, string.Format("</h3><div class=\"xml\">{0}</div>", ascx));
        }

        private ASP.Literal LoadCSharpCodebehind(string url)
        {
            string codeBehind = CodeVisualizationUtils.Colorize(new CSharpFormat(), CodeVisualizationUtils.ParseCodeRegionFromCodebehind(GetFileContent(url), "Code"));
            return BuildLiteral(url,
                                string.Format("</h3><div class=\"c#\">{0}</div><br />",
                                              codeBehind));
        }

        private ASP.Literal LoadVbCodebehind(string url)
        {
            string vbCode = CodeVisualizationUtils.Colorize(new VisualBasicFormat(), CodeVisualizationUtils.ConvertCSharpToVisualBasic(GetFileContent(url)));
            return BuildLiteral(url + ".vb",
                                string.Format("<div class=\"c#\">{0}</div><br />",
                                              vbCode));
        }

        private ASP.Literal LoadCss(string url)
        {
            string css = CodeVisualizationUtils.Colorize(new HtmlFormat(), GetFileContent(url));
            return BuildLiteral(url,  string.Format("<div class=\"css\">{0}</div><br />", css));
        }

        private ASP.Literal LoadJavaScript(string url)
        {
            string js = CodeVisualizationUtils.Colorize(new JavaScriptFormat(), GetFileContent(url));

            // Adding code...
            var lit = new ASP.Literal
                          {
                              Text = string.Format("<div class=\"js\">{0}</div><br />", js)
                          };

            if (!SampleConfigurationController.SourceFiles.ContainsKey(url))
                SampleConfigurationController.SourceFiles.Add(url, lit);

            return lit;
        }

        private static string ParseAspxContentPlaceHolder(string aspx)
        {
            //only load content from placeholder with the markup code
            const string placeHolderStartString =
                "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"p\" runat=\"server\">";

            var index = aspx.IndexOf(placeHolderStartString);
            return index == -1 ? aspx : aspx.Substring(index).Substring(placeHolderStartString.Length).Replace("</asp:Content>", "");
        }

        private string GetFileContent(string url)
        {
            return File.ReadAllText(Server.MapPath(url));
        }
    }
}