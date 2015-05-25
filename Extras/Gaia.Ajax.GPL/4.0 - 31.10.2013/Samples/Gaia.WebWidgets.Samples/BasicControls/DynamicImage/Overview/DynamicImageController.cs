using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;

namespace Gaia.WebWidgets.Samples.BasicControls.DynamicImage.Overview
{
    public class DynamicImageController
    {

        public string SourceFolder
        {
            get { return HttpContext.Current.Server.MapPath("~/BasicControls/DynamicImage/Overview/dynamicImages"); }
        }

        public Dictionary<FileInfo, Bitmap> ImagesList
        {
            get
            {
                if (HttpContext.Current.Application["DynamicImageBitmaps"] == null)
                {
                    Dictionary<FileInfo, Bitmap> imagesList = new Dictionary<FileInfo, Bitmap>();

                    foreach (FileInfo fileInfo in ImageFiles)
                        imagesList.Add(fileInfo, new Bitmap(fileInfo.FullName));

                    HttpContext.Current.Application["DynamicImageBitmaps"] = imagesList;
                }

                return HttpContext.Current.Application["DynamicImageBitmaps"] as Dictionary<FileInfo, Bitmap>;
            }
        }


        public KeyValuePair<FileInfo, Bitmap> SelectedItem
        {
            get
            {
                if (HttpContext.Current.Session["SelectedItem"] == null)
                    return new KeyValuePair<FileInfo, Bitmap>();

                return (KeyValuePair<FileInfo, Bitmap>)HttpContext.Current.Session["SelectedItem"];

            }
            set
            {
                HttpContext.Current.Session["SelectedItem"] = value;
            }
        }

        public KeyValuePair<FileInfo, Bitmap> GetByFileName(string fileName)
        {
            foreach (KeyValuePair<FileInfo, Bitmap> kv in ImagesList)
                if (kv.Key.Name.ToLower() == fileName.ToLower())
                    return kv;

            return new KeyValuePair<FileInfo, Bitmap>();
        }

        public IEnumerable<FileInfo> ImageFiles
        {
            get
            {
                foreach (string imageFile in Directory.GetFiles(SourceFolder))
                    if (IsImageFile(imageFile)) yield return new FileInfo(imageFile);
            }
        }

        private static bool IsImageFile(string fileName)
        {
            FileInfo file = new FileInfo(fileName);

            switch (file.Extension.ToUpper())
            {
                case ".JPG":
                case ".JPEG":
                case ".GIF":
                case ".PNG":
                    return true;
                default:
                    return false;
            }
        }


        public int Index
        {
            get
            {
                if (HttpContext.Current.Session["MapIndex"] == null)
                    return 0;
                return (int)HttpContext.Current.Session["MapIndex"];
            }
            set
            {
                HttpContext.Current.Session["MapIndex"] = value;
            }
        }

        public int Left
        {
            get
            {
                if (HttpContext.Current.Session["LeftImage"] == null)
                    return 0;
                return (int)HttpContext.Current.Session["LeftImage"];
            }
            set
            {
                HttpContext.Current.Session["LeftImage"] = value;
            }
        }

        public int Top
        {
            get
            {
                if (HttpContext.Current.Session["TopImage"] == null)
                    return 0;
                return (int)HttpContext.Current.Session["TopImage"];
            }
            set
            {
                HttpContext.Current.Session["TopImage"] = value;
            }
        }

        public double Zoom
        {
            get
            {
                if (HttpContext.Current.Session["Zoom"] == null)
                    return 0D;
                return (double)HttpContext.Current.Session["Zoom"];
            }
            set
            {
                HttpContext.Current.Session["Zoom"] = value;
            }
        }


        public int Width
        {
            get
            {
                if (HttpContext.Current.Session["MapWidth"] == null)
                    return 200;
                return (int)HttpContext.Current.Session["MapWidth"];
            }
            set
            {
                HttpContext.Current.Session["MapWidth"] = value;
            }
        }

        public int Height
        {
            get
            {
                if (HttpContext.Current.Session["MapHeight"] == null)
                    return 200;
                return (int)HttpContext.Current.Session["MapHeight"];
            }
            set
            {
                HttpContext.Current.Session["MapHeight"] = value;
            }
        }

        public bool IsLocalhost
        {
            get
            {
                return HttpContext.Current.Request.Url.DnsSafeHost.ToLower() == "localhost";
            }
        }

    }
}