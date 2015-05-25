namespace Gaia.WebWidgets.Samples.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;

    public class SampleConfiguration
    {
        private const int ConstMenuMaxIndex = 1000;
        private const string ConstSampleMenuItemsName = "SampleMenuItems";
        private const string SampleFilesIncludeSearchPattern = "*.ascx,*.cs,*.vb,*.css,*.js,*.xml";
        private const string SampleFilesExcludeSearchPattern = "designer.cs";

        internal SampleConfiguration(string absolutePath)
        {
            //set absolutepath
            AbsolutePath = absolutePath;

            //split to see folders and filename
            _folderArray = AbsolutePath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            //validate valid sample path
            if (_folderArray.Length != 5)
                return;

            _isValidSample = true;

            //get index of the sample in the menu, get from xml
            var subSectionXmlDoc = GetXmlDocForPath(AbsolutePath);

            var currentSampleNode =
                subSectionXmlDoc.SelectSingleNode(string.Format("//Directories/Directory[@Name='{0}']", Name));

            //determine which title to grab
            if (currentSampleNode != null)
            {
                var overridenPageTitleAttr = currentSampleNode.Attributes["Title"];
                if (overridenPageTitleAttr != null)
                    _titleOverridden = overridenPageTitleAttr.Value;
            }

            ParentMenuItem = GetSampleMenuItem(SubSectionAbsolutePath, subSectionXmlDoc, false, true);

            ParentMenuItem.Parent.Parent = GetSampleMenuItem(SectionAbsolutePath, GetXmlDocForPath(SectionAbsolutePath), false, true);

            //add related files for sample
            SampleFiles = GetSampleFiles(AbsoluteDirectory).ToList();

            //do sorting
            SampleFiles.Sort();
        }

        private static SampleMenuItemBase GetSampleMenuItem(
            string relativeDirectory,
            XmlNode xmlNode,
            bool skipElementsNotPresentInXml,
            bool includeParent)
        {
            var directories = relativeDirectory.Substring(1).Split(new[] {'/'},
                                                                        StringSplitOptions.RemoveEmptyEntries);

            //get level for menu
            var sampleMenuLevel = GetSampleMenuLevel(directories.Length - 1);

            var lastDirectory = directories.Length == 0 ? "" : directories[directories.Length - 1];

            //construct xpath
            var xPath = string.Format("//Directories/Directory[@Name='{0}']", lastDirectory);

            var currentXmlNode = xmlNode.SelectSingleNode(xPath);

            //find current path
            int indexForPath = XmlUtility.GetIndexOfXmlNode(lastDirectory, currentXmlNode);

            //if we have negative index, it is not present in xml
            if (indexForPath == -1 && skipElementsNotPresentInXml)
                return null;

            //put in bottom, if not exist
            if (indexForPath == -1)
                indexForPath = ConstMenuMaxIndex;

            //set what to return
            string returnPath = relativeDirectory;

            //if we are at last level e.g. /Core/Button/Overview/ we want to include the default aspx file we expect to find:
            // /Core/Button/Overview/Overview.aspx
            if (sampleMenuLevel == SampleTypeEnum.Sample)
            {
                string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(relativeDirectory), "*.aspx");

                //if we found something else than one aspx, we return null to not include it
                if (files.Length != 1)
                    return null;

                //if a default sample called /overview/ and have not been defined (indexForPath == ConstMenuMaxIndex)  for an order in a xml file,
                //override it to be first
                if (lastDirectory.ToLower().Contains("overview") && indexForPath == ConstMenuMaxIndex)
                    indexForPath = -1;

                //construct directory as path for sample
                returnPath = relativeDirectory + "/";
            }

            //check if item has an attribute with a overriding url
            string overrideUrl = XmlUtility.GetAttributeValue(currentXmlNode, "Url");

            if (!string.IsNullOrEmpty(overrideUrl))
                returnPath = overrideUrl;

            //default set to lastDirectory
            string title = lastDirectory;

            //does it have a title we should override Name?
            string overrideTitle = XmlUtility.GetAttributeValue(currentXmlNode, "Title");

            if (!string.IsNullOrEmpty(overrideTitle))
                title = overrideTitle;

            List<SampleMenuItemBase> children = null;

            if (sampleMenuLevel == SampleTypeEnum.SubSection || sampleMenuLevel == SampleTypeEnum.Section)
                children = GetCachedChildrenMenuItems(relativeDirectory, false);

            SampleMenuItemBase parentMenuItem = null;
            if (includeParent && sampleMenuLevel != SampleTypeEnum.Section)
            {
                var parentPath = GetParentPath(relativeDirectory);
                parentMenuItem = GetSampleMenuItem(parentPath, GetXmlDocForPath(parentPath), false, false);
            }

            relativeDirectory += "/";

            return new SampleMenuItemBase(indexForPath, title, returnPath, relativeDirectory, children, parentMenuItem, sampleMenuLevel);
        }

        private static SampleTypeEnum GetSampleMenuLevel(int levelIndex)
        {
            switch (levelIndex)
            {
                case 0:
                    return SampleTypeEnum.Section;
                case 1:
                    return SampleTypeEnum.SubSection;
                case 2:
                    return SampleTypeEnum.Sample;
            }

            return SampleTypeEnum.Undefined;
        }

        public static List<SampleMenuCacheHelper> CurrentSampleMenuItemPaths
        {
            get
            {
                if (HttpContext.Current.Application[ConstSampleMenuItemsName] == null)
                    HttpContext.Current.Application[ConstSampleMenuItemsName] = new List<SampleMenuCacheHelper>();

                return (List<SampleMenuCacheHelper>)HttpContext.Current.Application[ConstSampleMenuItemsName];
            }
        }

        public static List<SampleMenuItemBase> GetCachedChildrenMenuItems(string absolutePath, bool skipElementsNotPresentInXml)
        {
            var cachedResult = GetSingleMenutItemCachedResult(absolutePath, skipElementsNotPresentInXml);

            if (cachedResult != null) return cachedResult.MenuItems;

            var childrenMenuItems = GetSortedChildrenMenuItems(absolutePath, skipElementsNotPresentInXml);

            if (childrenMenuItems != null)
                CurrentSampleMenuItemPaths.Add(new SampleMenuCacheHelper(absolutePath, skipElementsNotPresentInXml, childrenMenuItems));

            return childrenMenuItems;
        }

        private static SampleMenuCacheHelper GetSingleMenutItemCachedResult(string absolutePath, bool skipElementsNotPresentInXml)
        {
            return CurrentSampleMenuItemPaths.Where(
                m => m.AbsolutePath == absolutePath && m.SkipElementsNotPresentInXml == skipElementsNotPresentInXml).SingleOrDefault();
        }

        private static List<SampleMenuItemBase> GetSortedChildrenMenuItems(string absolutePath, bool skipElementsNotPresentInXml)
        {
            var xmlDocForPath = GetXmlDocForPath(absolutePath);

            var retVal = new List<SampleMenuItemBase>();

            foreach (var subFolder in WebUtility.GetSubFolders(absolutePath))
            {
                var sampleMenuItemBase = GetSampleMenuItem(subFolder, xmlDocForPath, skipElementsNotPresentInXml, false);

                if (sampleMenuItemBase == null) continue;

                retVal.Add(sampleMenuItemBase);
            }

            //sort with the lowest index first
            retVal.Sort();

            return retVal;
        }

        private static XmlDocument GetXmlDocForPath(string absolutePath)
        {
            //if the paths contain .aspx, then we are at sample level, and need to go parents parent to find it
            bool isSample = absolutePath.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase);
            if (isSample)
                absolutePath = GetParentPath(GetParentPath(absolutePath));

            if (!absolutePath.StartsWith("~/"))
                throw  new Exception("Absolute path doesn't start with ~");

            //first make sure we remove ~ when we will count dirs
            string[] folderArray = absolutePath.Substring(1).Split(new char[] {'/'},
                                                                   StringSplitOptions.RemoveEmptyEntries);

            //check if we'll use standard xml configuration or get the one for the root, with the section menus
            string xmlFileName = folderArray.Length > 0
                                     ? folderArray[folderArray.Length - 1]
                                     : "App_Data/SamplesConfiguration";

            string configFilePath =
                HttpContext.Current.Server.MapPath(string.Format("{0}/{1}.xml", absolutePath, xmlFileName));

            //check if exist
            bool fileExist = File.Exists(configFilePath);

            //get index of the sample in the menu, get from xml
            var subSectionXmlDoc = new XmlDocument();

            if (fileExist)
                subSectionXmlDoc.LoadXml(File.ReadAllText(configFilePath));

            return subSectionXmlDoc;
        }

        private static IEnumerable<SampleFile> GetSampleFiles(string virtualPath)
        {
            var sampleFiles = GetFilesByVirtualPath(virtualPath, SampleFilesIncludeSearchPattern, SampleFilesExcludeSearchPattern);

            foreach (var file in sampleFiles)
            {
                var fileTypeEnum = (SampleFile.FileTypeEnum) Enum.Parse(typeof (SampleFile.FileTypeEnum), file.Extension.Substring(1), true);

                string fileContent = File.ReadAllText(file.FullName);

                yield return new SampleFile(file.Name, fileTypeEnum, fileContent);
            }
        }

        private static IEnumerable<FileInfo> GetFilesByVirtualPath(string virtualPath, string includeSearchPattern, string excludeSearchPattern)
        {
            string path = HttpContext.Current.Server.MapPath(virtualPath);
            return FileUtility.GetFiles(new DirectoryInfo(path), includeSearchPattern, excludeSearchPattern);
        }

        private readonly string[] _folderArray;

        private readonly bool _isValidSample;
        public bool IsValidSample
        {
            get { return _isValidSample; }
        }

        private readonly string _titleOverridden;

        public string TitleOverridden
        {
            get { return _titleOverridden; }
        }

        public string AbsoluteDirectory
        {
            get { return AbsolutePath.Substring(0, AbsolutePath.LastIndexOf("/")); }
        }

        public string FileName
        {
            get { return _folderArray.Count() > 2 ?  _folderArray[3] : null; }
        }

        public string SectionAbsolutePath
        {
            get
            {
                string[] strings = AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return string.Format("~/{0}/", strings[1]);
            }
        }

        public SampleMenuItemBase ParentMenuItem { get; private set; }

        public string AbsolutePath { get; private set; }

        public string SubSectionAbsolutePath
        {
            get { return GetParentPath(AbsolutePath); }
        }

        public string Name
        {
            get { return _folderArray[2]; }
        }

        private static string GetParentPath(string pathToConvert)
        {
            return pathToConvert.Contains("/") ? pathToConvert.Substring(0, pathToConvert.LastIndexOf("/")) : "/";
        }

        public List<SampleFile> SampleFiles { get; private set; }

        public class SampleFile: IComparable<SampleFile>
        {
            public SampleFile(string name, FileTypeEnum fileType, string fileContent)
            {
                Name = name;
                FileType = fileType;
                FileContent = fileContent;
            }

            public enum FileTypeEnum
            {
                Cs,
                Vb,
                Js,
                Css,
                Ascx,
                Xml
            }

            public string Name { get; private set; }

            public string FileContent { get; set; }

            public FileTypeEnum FileType { get; private set; }

            public int CompareTo(SampleFile other)
            {
                return Name.CompareTo(other.Name);
            }
        }

        public enum SampleTypeEnum
        {
            Section = 0,
            SubSection = 1,
            Sample = 2,
            Undefined
        }

        public class SampleMenuCacheHelper
        {
            public SampleMenuCacheHelper(string absolutePath, bool skipElementsNotPresentInXml, List<SampleMenuItemBase> menuItems)
            {
                AbsolutePath = absolutePath;
                SkipElementsNotPresentInXml = skipElementsNotPresentInXml;
                MenuItems = menuItems;
            }

            public string AbsolutePath { get; set; }
            public bool SkipElementsNotPresentInXml { get; set; }
            public List<SampleMenuItemBase> MenuItems { get; set; }
        }

        public class SampleMenuItemBase: IComparable<SampleMenuItemBase>
        {
            public SampleMenuItemBase(int index, string title, string absolutePath, string originalAbsolutePath, List<SampleMenuItemBase> children, SampleMenuItemBase parent, SampleTypeEnum sampleTypeEnum)
            {
                Title = title;
                Children = children;
                AbsolutePath = absolutePath;
                OriginalAbsolutePath = originalAbsolutePath;
                Index = index;
                Parent = parent;
                SampleType = sampleTypeEnum;
            }

            public string Title { get; private set; }

            public int Index { get; private set; }

            public string AbsolutePath { get; private set; }

            public string OriginalAbsolutePath { get; private set; }

            public List<SampleMenuItemBase> Children { get; private set; }

            public SampleMenuItemBase Parent { get; set; }

            public SampleTypeEnum SampleType { get; set; }

            const string OverView = "Overview";
            public int CompareTo(SampleMenuItemBase other)
            {
                // Here we make sure that Overview get's put ontop 
                if (Title == OverView && other.Title == OverView)
                    return 0;
                if (Title == OverView)
                    return -1;
                if (other.Title == OverView)
                    return 1;

                //if index is 1000, "put on bottom", but still sort alphabetic
                if (Index == ConstMenuMaxIndex && other.Index == ConstMenuMaxIndex)
                    return Title.CompareTo(other.Title);

                if (Index == ConstMenuMaxIndex && other.Index != ConstMenuMaxIndex)
                    return 1;

                if (Index != ConstMenuMaxIndex && other.Index == ConstMenuMaxIndex)
                    return -1;

                return Index.CompareTo(other.Index);

            }
        }
    }

    public static class SampleMenuItemExtension
    {
        public static bool IsCurrentSection(this SampleConfiguration.SampleMenuItemBase menuItem, SampleConfiguration sampleConfiguration)
        {
            return menuItem.SampleType == SampleConfiguration.SampleTypeEnum.Section
                   && menuItem.Children != null
                   && sampleConfiguration.ParentMenuItem != null
                   && menuItem.OriginalAbsolutePath == sampleConfiguration.ParentMenuItem.Parent.Parent.AbsolutePath;
        }
    }
}