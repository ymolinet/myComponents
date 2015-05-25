namespace Gaia.WebWidgets.Samples.UI
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using Gaia.WebWidgets.Extensions;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class SampleSearch : UserControl
    {
        private const int MaxResultsCount = 10;
        private const string DefaultSampleTitleEndsWith = " > Overview";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) ac.KeyChangeEventsInterval = WebUtility.IsLocalhost ? 25 : 250;
        }

        protected void AcGetAutoCompleterItems(object sender, EventArgs e)
        {
            string query = Query;

            // If the query is empty, exit
            if (string.IsNullOrEmpty(query))
            {
                SetCountLabel(null);
                return;
            }

            var results = new SampleSearchController().Find(query).ToList();

            SetCountLabel(results.Count());

            var idx = 0;
            foreach (var sample in results.Take(MaxResultsCount))
            {
                var lit = new Label { ID = idx.ToString(), CssClass = sample.Key, Text = ConcatenateTitle(sample.Value) };
                ac.AutoCompleterItems.Add(lit);
                ++idx;
            }  
        }

        private string Query
        {
            get { return ac.Text.Trim(); }
        }

        private static string ConcatenateTitle(string title)
        {
            return title.EndsWith(DefaultSampleTitleEndsWith) ? title.Substring(0, title.Length - DefaultSampleTitleEndsWith.Length) : title;
        }

        private void SetCountLabel(int? count)
        {
            l.Visible = true;
            l.Text = count.HasValue ? count.ToString() : "&nbsp;";
        }

        protected void AcSelectionChanged(object sender, AutoCompleter.AutoCompleterSelectionChangedEventArgs e)
        {
            var result = e.SelectedValue as Label;
            if (result != null) Response.Redirect(result.CssClass);
        }

        protected void AcClosing(object sender, AutoCompleter.AutoCompleterClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(Query))
                SetCountLabel(null);
        }
    }
}