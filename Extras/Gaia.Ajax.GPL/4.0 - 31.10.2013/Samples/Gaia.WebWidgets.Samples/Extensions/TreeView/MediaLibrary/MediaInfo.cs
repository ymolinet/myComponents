namespace Gaia.WebWidgets.Samples.Extensions.TreeView.MediaLibrary
{
    /// <summary>
    /// MediaInfo Entity
    /// </summary>
    public class MediaInfo
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }

        public MediaInfo(string id, string text, string category)
        {
            Id = id;
            Text = text;
            Category = category;
        }
    }
}