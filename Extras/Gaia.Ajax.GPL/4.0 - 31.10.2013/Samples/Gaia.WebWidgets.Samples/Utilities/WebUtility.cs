namespace Gaia.WebWidgets.Samples.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;

    public static class WebUtility
    {
        public static Control FindControl(Control control, string id)
        {
            var foundControl = control.FindControl(id);

            if (foundControl != null)
                return foundControl;

            foreach (Control child in control.Controls)
            {
                foundControl = FindControl(child, id);

                if (foundControl != null)
                    return foundControl;
            }

            return null;
        }

        public static T First<T>(ControlCollection controls) where T: class
        {
            return controls.OfType<T>().Select(control => control).FirstOrDefault();
        }

        public static T FindFirstParent<T>(Control startingControl) where T : Control
        {
            // traverse the control ladder until we find an instance of the type
            for (var control = startingControl; control != null; control = control.Parent)
                if (control is T)
                    return control as T;

            return null;

        }

        public static IEnumerable<Control> All(ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                foreach (Control grandchild in All(control.Controls))
                    yield return grandchild;

                yield return control;
            }
        }

        public static string AppRelativeCurrentExecutionFilePath
        {
            get { return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath; }
        }

        public static IEnumerable<string> GetSubFolders(string absolutePath)
        {
            foreach (var directory in Directory.GetDirectories(HttpContext.Current.Server.MapPath(absolutePath)))
            {
                // avoid subversion directories
                if (directory.Contains("\\.")) continue;

                string resultPath = directory;

                //make sure always relative to web root
                resultPath = resultPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length - 1);

                //make sure only from root folder and up are returned
                resultPath = resultPath.Replace("\\", "/");

                yield return "~" + resultPath;
            }
        }

        public static bool IsControlEmpty(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            var controls = control.Controls;

            // control has no children
            if (controls.Count == 0)
                return true;

            if (controls.Count == 1)
            {
                var staticContent = controls[0] as LiteralControl;

                // control only has a literal child which is either empty,
                // or contains nothing but whitespace
                if (staticContent != null)
                {
                    if (String.IsNullOrEmpty(staticContent.Text))
                        return true;

                    if (IsWhiteSpace(staticContent.Text))
                        return true;
                }
            }

            return false;
        }

        public static bool IsWhiteSpace(string s)
        {
            return s.All(Char.IsWhiteSpace);
        }

        public static bool IsLocalhost
        {
            get { return HttpContext.Current.Request.Url.DnsSafeHost.ToLower() == "localhost"; }
        }

        public static string[] GetThemes()
        {
            //if already cached
            if (HttpContext.Current.Cache["SiteThemes"] != null)
                return (string[])HttpContext.Current.Cache["SiteThemes"];

            var themesDirPath = HttpContext.Current.Server.MapPath("~/App_Themes");
            var themeDirs = Directory.GetDirectories(themesDirPath);
            var themeNames = new List<string>();

            for (var i = 0; i <= themeDirs.Length - 1; i++)
            {
                var themeFileName = Path.GetFileName(themeDirs[i].ToLower());
                if (themeFileName != null && !themeFileName.StartsWith("."))
                    themeNames.Add(themeFileName);
            }

            // cache the array
            var dep = new CacheDependency(themesDirPath);
            HttpContext.Current.Cache.Insert("SiteThemes", themeNames.ToArray(), dep);

            return themeNames.ToArray();
        }

        public static String StyleSheetTheme
        {
            get
            {
                //set default theme
                if (HttpContext.Current.Session["CurrentTheme"] == null)
                    StyleSheetTheme = "gaiax";

                return HttpContext.Current.Session["CurrentTheme"].ToString();

            }
            set { HttpContext.Current.Session["CurrentTheme"] = value; }
        }

        public static Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromArgb(
                random.Next(0, 255), 
                random.Next(0, 255), 
                random.Next(0, 255));
        }

        /// <summary>
        /// Makes lowercase password
        /// </summary>
        /// <param name="length">How many Characters?</param>
        /// <returns>The generated password</returns>
        public static string GeneratePasswordInLowercase(int length)
        {
            var itsRandomGenerator = new Random(DateTime.Now.DayOfYear * DateTime.Now.Millisecond);
            string[] itsCharacters = { "A","B","C","D","E","F","G","H","J","K","L","M","N","P","Q","R","S","T","U","V","W","X","Y","Z",
												"2","3","4","5","6","7","8","9",
												"a","b","c","d","e","f","g","h","k","m","n","p","q","r","s","t","u","v","w","x","y","z"};

            var strPass = new StringBuilder();
            for (var x = 0; x <= length - 1; x++)
            {
                var p = itsRandomGenerator.Next(0, 35);
                strPass.Append(itsCharacters[p]);
            }
            return strPass.ToString().ToLower(CultureInfo.InvariantCulture);
        }

        public static IEnumerable<System.Web.UI.WebControls.ListItem> GetUserControlViews<TView>(Page page, string folder)
        {
            foreach (string userControl in Directory.GetFiles(page.Server.MapPath(folder)))
            {
                var file = new FileInfo(userControl);
                if (!file.Extension.ToLowerInvariant().EndsWith("ascx")) continue;
                var control = page.LoadControl(Path.Combine(folder, file.Name));

                if (control is TView)
                    yield return new System.Web.UI.WebControls.ListItem(file.Name.Replace(".ascx", String.Empty));
            }
        }
    }
}