namespace Gaia.WebWidgets.Samples.Utilities
{
    public class MediaBase
    {
        public MediaBase(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id { get; set; }

        public string Text { get; set; }
    }
}
