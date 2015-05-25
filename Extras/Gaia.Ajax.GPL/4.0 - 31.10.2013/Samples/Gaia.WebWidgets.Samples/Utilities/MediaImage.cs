namespace Gaia.WebWidgets.Samples.Utilities
{
    /// <summary>
    /// MediaImage Entity
    /// </summary>
    public class MediaImage : MediaBase
    {
        public MediaImage(string id, string text, string imageUrl, string thumbUrl)
            : base(id, text)
        {
            ImageUrl = imageUrl;
            ThumbUrl = thumbUrl;
        }

        public string ImageUrl { get; set; }

        public string ThumbUrl { get; set; }

        public bool HasThumb { get { return !string.IsNullOrEmpty(ThumbUrl); } }
    }
}
