namespace myComponents.DatatablesDotNet
{
    public class DTDNColumn
    {
        public int id { get; set; }
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DTDNOrder Order { get; set; }
        public DTDNSearch Search { get; set; }
        public DTDNColumn()
        {
            Search = new DTDNSearch();
        }
    }

    public class DTDNParameter
    {
        public string key { get; set; }
        public string value { get; set; }

        public DTDNParameter(string _key, string _value)
        {
            key = _key;
            value = _value;
        }
    }

    public class DTDNSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class DTDNOrder
    {
        public int id { get; set; }
        public int Column { get; set; }
        public DTDNOrderDir Dir { get; set; }
    }

    public enum DTDNOrderDir
    {
        Ascending,
        Descending
    }
}