namespace Gaia.WebWidgets.Samples.Extensions.TreeView.MediaLibrary
{
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Summary description for MediaController
    /// </summary>
    public class MediaController
    {
        public List<MediaInfo> Media
        {
            get
            {
                if (HttpContext.Current.Session["MediaElements"] == null)
                    HttpContext.Current.Session["MediaElements"] = new List<MediaInfo>(LoadMedia());

                return HttpContext.Current.Session["MediaElements"] as List<MediaInfo>;
            }
        }

        public MediaInfo GetMediaById(string id)
        {
            return Media.Find(index => index.Id == id);
        }

        public List<MediaInfo> GetMediaByCategory(string category)
        {
            return Media.FindAll(index => index.Category == category);
        }

        private static IEnumerable<MediaInfo> LoadMedia()
        {
            return new[]
                       {
                           new MediaInfo("field", "Field", "beautiful"),
                           new MediaInfo("flower", "Flower", "beautiful"),
                           new MediaInfo("lake", "Lake", "beautiful"),
                           new MediaInfo("skyview", "Sky-Field", "beautiful"),

                           new MediaInfo("feet", "Feet", "people"),
                           new MediaInfo("green", "Green lady", "people"),
                           new MediaInfo("earring", "Ear ring", "people"),
                           new MediaInfo("indian", "Indian", "people"),

                           new MediaInfo("crisps", "Crisps", "food"),
                           new MediaInfo("chocolate", "Chocolate", "food"),
                           new MediaInfo("orange", "Orange", "food"),
                           new MediaInfo("blueberry", "Blueberry", "food"),

                           new MediaInfo("oQm_8vX3sYU", "Peanut", "jeff_dunham"),
                           new MediaInfo("5QFtEYMsnck", "Blooper", "jeff_dunham"),
                           new MediaInfo("VnXQfzX0ijI", "Walter", "jeff_dunham"),
                           new MediaInfo("1iMd8Pm", "Bubba J", "jeff_dunham"),

                           new MediaInfo("miles", "Miles Davis", "jazz_music"),
                           new MediaInfo("holiday", "Billy Holiday", "jazz_music"),
                           new MediaInfo("armstrong", "Louis Armstrong", "jazz_music"),

                           new MediaInfo("beethoven", "Beethoven", "classic"),
                           new MediaInfo("mozart", "Mozart", "classic"),
                           new MediaInfo("bach", "Bach", "classic"),
                       };
        }
    }
}