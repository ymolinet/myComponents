namespace Gaia.WebWidgets.Samples.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    public static class MediaUtility
    {
        private const string ImageSearchPattern = "*.jpg,*.png";
        private const string ImageAbsolutePath = "~/media/collections/images/";

        public static IEnumerable<MediaImage> GetImageFiles(string folder)
        {
            return GetImageFiles(folder, -1);
        }

        public static IEnumerable<MediaImage> GetImageFiles(string folder, int maxResults)
        {
            string urlRoot = ImageAbsolutePath + folder + "/";

            //check if thumbs folder exists
            bool thumbsExist = Directory.Exists(HttpContext.Current.Server.MapPath(urlRoot + "thumbs/"));

            int results = 0;
            foreach (var file in FileUtility.GetFiles(new DirectoryInfo(HttpContext.Current.Server.MapPath(urlRoot)), ImageSearchPattern))
            {
                results++;
                if (maxResults != -1 && results > maxResults) break;

                yield return
                    new MediaImage(
                        file.Name.ToLower(), GetNiceName(file.Name), urlRoot + file.Name,
                        thumbsExist ? urlRoot + "thumbs/" + file.Name : "");
            }
        }

        //split by underscore and make each first letter upper case
        private static string GetNiceName(string fileName)
        {
            string[] splitted = fileName.Split(new[] { '_' });

            var retVal = "";

            //Make each first letter Upper case
            foreach (var t in splitted)
            {
                if (!string.IsNullOrEmpty(retVal)) retVal += " ";
                retVal += t.Substring(0, 1).ToUpper() + t.Substring(1);
            }

            //remove extension
            retVal = retVal.Substring(0, retVal.LastIndexOf("."));

            return retVal;
        }
    }
}
