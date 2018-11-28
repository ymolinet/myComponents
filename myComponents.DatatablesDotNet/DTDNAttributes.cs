namespace myComponents.DatatablesDotNet
{
    public class DTDNAttributes
    {
        private bool isPost;

        public DTDNAttributes(string HttpMethod = "POST")
        {
            isPost = (HttpMethod == "POST");
        }

        public string Draw { get { return "draw"; } }
        public string Start { get { return "start"; } }
        public string Length { get { return "length"; } }
        public string SearchValue
        {
            get
            {
                if (isPost) return "search.value";
                else return "[search][value]";
            }
        }
        public string SearchRegex
        {
            get
            {
                if (isPost) return "search.regex";
                else return "[search][regex]";
            }
        }
        public string Name
        {
            get
            {
                if (isPost) return ".name";
                else return "[name]";
            }
        }
        public string Data
        {
            get
            {
                if (isPost) return ".data";
                else return "[data]";
            }
        }
        public string Orderable
        {
            get
            {
                if (isPost) return ".orderable";
                else return "[orderable]";
            }
        }
        public string Searchable
        {
            get
            {
                if (isPost) return ".searchable";
                else return "[searchable]";
            }
        }
        public string Column
        {
            get
            {
                if (isPost) return ".column";
                else return "[column]";
            }
        }
        public string Dir
        {
            get
            {
                if (isPost) return ".dir";
                else return "[dir]";
            }
        }
    }
}