namespace Gaia.WebWidgets.Samples.Extensions.AutoCompleter.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void AutoCompleter1_GetAutoCompleteData(object sender, EventArgs e)
        {
            string query = AutoCompleter1.Text;

            // If the query is empty or identical to previously selected item, exit!
            if (query.Length == 0 || query == zCountryResult.Text)
                return;

            // Perform the search
            var result = new CountrySearch().Search(query);

            if (result.IsEmpty) /* no results found, exit! */
                return;

            // The Autocompleter likes to be fed controls
            // The Result class can build them for us!
            foreach(var control in result.BuildControlsFromResult())
            {
                control.HighlightQuery = true;
                AutoCompleter1.AutoCompleterItems.Add(control);
            }

            //update status label with friendly message
            Label1.Text = String.Format("{0} results for '{1}'",
                result.Countries.Count, 
                AutoCompleter1.Text);
            
            // Make the results panel invi(n)sible
            zPanelResult.Visible = false;

        }

        protected void AutoCompleter1_SelectionChanged
            (object sender, Gaia.WebWidgets.Extensions.AutoCompleter.AutoCompleterSelectionChangedEventArgs e)
        {
            // After a selection was made, we should be able to retrieve that control here
            var countryControl = e.SelectedValue as CountryControl;
            if (countryControl == null)
                return;

            // Populate some results 
            string text = countryControl.Country.Name;
            zCountryResult.Text = text;
            zImageResult.ImageUrl = countryControl.Country.MediaInfo.ImageUrl;
            zImageResult.AlternateText = countryControl.Country.MediaInfo.Text;
            AutoCompleter1.Text = text;
            Label1.Text = string.Empty;

            zPanelResult.Visible = true;

        }
    }
}