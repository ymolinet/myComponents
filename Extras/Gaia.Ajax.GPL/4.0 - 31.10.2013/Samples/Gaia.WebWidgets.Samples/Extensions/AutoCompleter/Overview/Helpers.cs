namespace Gaia.WebWidgets.Samples.Extensions.AutoCompleter.Overview
{
    using System.Collections.Generic;
    using System.Globalization;
    using Gaia.WebWidgets.Samples.Utilities;

    public class CountryInfo
    {
        public CountryInfo(string code, string name, MediaImage mediaInfo)
        {
            Code = code;
            Name = name;
            MediaInfo = mediaInfo;
        }

        public string Code { get; set; }

        public string Name { get; set; }

        public MediaImage MediaInfo { get; set; }
    }

    public class SearchResult
    {
        private readonly List<CountryInfo> _countries;
        private readonly string _query;

        internal SearchResult(List<CountryInfo> countries, string query)
        {
            _countries = countries;
            _query = query;
        }

        public bool IsEmpty
        {
            get { return _countries.Count == 0; }
        }

        public List<CountryInfo> Countries
        {
            get { return _countries; }
        }

        public IEnumerable<CountryControl> BuildControlsFromResult()
        {
            return BuildControlsFromResult(10);
        }

        public IEnumerable<CountryControl> BuildControlsFromResult(int maxCount)
        {
            for (int i = 0; i < _countries.Count && i < maxCount; i++)
                yield return new CountryControl(_countries[i], _query);
        }
    }

    public class CountrySearch
    {
        private static List<MediaImage> _flags;
        private static List<MediaImage> Flags
        {
            get { return _flags ?? (_flags = new List<MediaImage>(MediaUtility.GetImageFiles("flags"))); }
        }

        public SearchResult Search(string query)
        {
            return new SearchResult(new List<CountryInfo>(SearchForCountry(query)), query);
        }

        private static IEnumerable<CountryInfo> SearchForCountry(string query, bool ignoreCase = true)
        {
            for (var i = 0; i < CountryNames.Length; i++)
                if (CountryNames[i].StartsWith(query, ignoreCase, CultureInfo.InvariantCulture))
                {
                    var idx = i;
                    var media = Flags.Find(mediaInfo => CountryCodes[idx].ToLowerInvariant() == mediaInfo.Text.ToLowerInvariant());

                    if (media != null)
                        yield return new CountryInfo(CountryCodes[i], CountryNames[i], media);
                }
        }

        static readonly string[] CountryCodes = { 
			"--","AP","EU","AD","AE","AF","AG","AI","AL","AM","AN","AO","AQ","AR","AS",
			"AT","AU","AW","AZ","BA","BB","BD","BE","BF","BG","BH","BI","BJ","BM","BN",
			"BO","BR","BS","BT","BV","BW","BY","BZ","CA","CC","CD","CF","CG","CH","CI",
			"CK","CL","CM","CN","CO","CR","CU","CV","CX","CY","CZ","DE","DJ","DK","DM",
			"DO","DZ","EC","EE","EG","EH","ER","ES","ET","FI","FJ","FK","FM","FO","FR",
			"FX","GA","GB","GD","GE","GF","GH","GI","GL","GM","GN","GP","GQ","GR","GS",
			"GT","GU","GW","GY","HK","HM","HN","HR","HT","HU","ID","IE","IL","IN","IO",
			"IQ","IR","IS","IT","JM","JO","JP","KE","KG","KH","KI","KM","KN","KP","KR",
			"KW","KY","KZ","LA","LB","LC","LI","LK","LR","LS","LT","LU","LV","LY","MA",
			"MC","MD","MG","MH","MK","ML","MM","MN","MO","MP","MQ","MR","MS","MT","MU",
			"MV","MW","MX","MY","MZ","NA","NC","NE","NF","NG","NI","NL","NO","NP","NR",
			"NU","NZ","OM","PA","PE","PF","PG","PH","PK","PL","PM","PN","PR","PS","PT",
			"PW","PY","QA","RE","RO","RU","RW","SA","SB","SC","SD","SE","SG","SH","SI",
			"SJ","SK","SL","SM","SN","SO","SR","ST","SV","SY","SZ","TC","TD","TF","TG",
			"TH","TJ","TK","TM","TN","TO","TL","TR","TT","TV","TW","TZ","UA","UG","UM",
			"US","UY","UZ","VA","VC","VE","VG","VI","VN","VU","WF","WS","YE","YT","RS",
			"ZA","ZM","ME","ZW","A1","A2","O1","AX","GG","IM","JE","BL","MF"
		};

        public static readonly string[] CountryNames = {
			"N/A","Asia/Pacific Region","Europe","Andorra","United Arab Emirates","Afghanistan",
			"Antigua and Barbuda","Anguilla","Albania","Armenia","Netherlands Antilles","Angola",
			"Antarctica","Argentina","American Samoa","Austria","Australia","Aruba","Azerbaijan",
			"Bosnia and Herzegovina","Barbados","Bangladesh","Belgium","Burkina Faso","Bulgaria",
			"Bahrain","Burundi","Benin","Bermuda","Brunei Darussalam","Bolivia","Brazil","Bahamas",
			"Bhutan","Bouvet Island","Botswana","Belarus","Belize","Canada","Cocos (Keeling) Islands",
			"Congo, The Democratic Republic of the","Central African Republic","Congo","Switzerland",
			"Cote D'Ivoire","Cook Islands","Chile","Cameroon","China","Colombia","Costa Rica","Cuba",
			"Cape Verde","Christmas Island","Cyprus","Czech Republic","Germany","Djibouti","Denmark",
			"Dominica","Dominican Republic","Algeria","Ecuador","Estonia","Egypt","Western Sahara",
			"Eritrea","Spain","Ethiopia","Finland","Fiji","Falkland Islands (Malvinas)",
			"Micronesia, Federated States of","Faroe Islands","France","France, Metropolitan","Gabon",
			"United Kingdom","Grenada","Georgia","French Guiana","Ghana","Gibraltar","Greenland",
			"Gambia","Guinea","Guadeloupe","Equatorial Guinea","Greece",
			"South Georgia and the South Sandwich Islands","Guatemala","Guam","Guinea-Bissau","Guyana",
			"Hong Kong","Heard Island and McDonald Islands","Honduras","Croatia","Haiti","Hungary",
			"Indonesia","Ireland","Israel","India","British Indian Ocean Territory","Iraq",
			"Iran, Islamic Republic of","Iceland","Italy","Jamaica","Jordan","Japan","Kenya",
			"Kyrgyzstan","Cambodia","Kiribati","Comoros","Saint Kitts and Nevis",
			"Korea, Democratic People's Republic of","Korea, Republic of","Kuwait","Cayman Islands",
			"Kazakstan","Lao People's Democratic Republic","Lebanon","Saint Lucia","Liechtenstein",
			"Sri Lanka","Liberia","Lesotho","Lithuania","Luxembourg","Latvia","Libyan Arab Jamahiriya",
			"Morocco","Monaco","Moldova, Republic of","Madagascar","Marshall Islands","Macedonia",
			"Mali","Myanmar","Mongolia","Macau","Northern Mariana Islands","Martinique","Mauritania",
			"Montserrat","Malta","Mauritius","Maldives","Malawi","Mexico","Malaysia","Mozambique",
			"Namibia","New Caledonia","Niger","Norfolk Island","Nigeria","Nicaragua","Netherlands",
			"Norway","Nepal","Nauru","Niue","New Zealand","Oman","Panama","Peru","French Polynesia",
			"Papua New Guinea","Philippines","Pakistan","Poland","Saint Pierre and Miquelon",
			"Pitcairn Islands","Puerto Rico","Palestinian Territory","Portugal","Palau","Paraguay",
			"Qatar","Reunion","Romania","Russian Federation","Rwanda","Saudi Arabia",
			"Solomon Islands","Seychelles","Sudan","Sweden","Singapore","Saint Helena","Slovenia",
			"Svalbard and Jan Mayen","Slovakia","Sierra Leone","San Marino","Senegal","Somalia",
			"Suriname","Sao Tome and Principe","El Salvador","Syrian Arab Republic","Swaziland",
			"Turks and Caicos Islands","Chad","French Southern Territories","Togo","Thailand",
			"Tajikistan","Tokelau","Turkmenistan","Tunisia","Tonga","Timor-Leste","Turkey",
			"Trinidad and Tobago","Tuvalu","Taiwan","Tanzania, United Republic of","Ukraine","Uganda",
			"United States Minor Outlying Islands","United States","Uruguay","Uzbekistan",
			"Holy See (Vatican City State)","Saint Vincent and the Grenadines","Venezuela",
			"Virgin Islands, British","Virgin Islands, U.S.","Vietnam","Vanuatu","Wallis and Futuna",
			"Samoa","Yemen","Mayotte","Serbia","South Africa","Zambia","Montenegro","Zimbabwe",
			"Anonymous Proxy","Satellite Provider","Other","Aland Islands","Guernsey","Isle of Man",
			"Jersey","Saint Barthelemy","Saint Martin"
		};
    }

    public class CountryControl : System.Web.UI.WebControls.CompositeControl
    {
        private readonly CountryInfo _country;
        private readonly string _query;

        public bool HighlightQuery { get; set; }

        public CountryInfo Country
        {
            get { return _country; }
        }

        public CountryControl(CountryInfo country, string query)
        {
            _country = country;
            _query = query;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation
        /// to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            var image = new Image { ID = "i", ImageUrl = _country.MediaInfo.ImageUrl };

            var panel = new System.Web.UI.WebControls.Panel {CssClass = "auto-item"};
            panel.Controls.Add(image);
            Controls.Add(panel);

            var length = _query.Length;
            var highlight = _country.Name.Substring(0, length);
            var theRest = _country.Name.Substring(length, _country.Name.Length - length);

            var label = new Label { ID = "hl", CssClass = "highlight", Text = highlight, Visible = HighlightQuery };
            Controls.Add(label);

            label = new Label {ID = "l", Text = HighlightQuery ? theRest : _country.Name, Visible = HighlightQuery};
            Controls.Add(label);
            
            base.CreateChildControls();
        }
    }
}
