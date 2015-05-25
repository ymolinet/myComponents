namespace Gaia.WebWidgets.Samples.BasicControls.DropDownList.CascadingDropDown
{
    using System;
    using System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateGamesGenres();
        }

        protected void ddlGamesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGames(ddlGamesGenres.SelectedValue);
            ddlGames.Enabled = ddlGames.Items.Count > 1;
            lblSelectedGame.Text = string.Empty;
        }

        protected void ddlGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelectedGame.Text = String.Format("You selected: {0}", ddlGames.SelectedItem.Text);
        }

        private void PopulateGamesGenres()
        {
            ddlGamesGenres.Items.Add(new ListItem("Please Select...", "Select"));
            ddlGamesGenres.Items.Add(new ListItem("1st Person Shooter", "FPS"));
            ddlGamesGenres.Items.Add(new ListItem("Car games", "Cars"));

            PopulateGames(string.Empty);
        }

        private void PopulateGames(string gameGenre)
        {
            ddlGames.Items.Clear();

            switch (gameGenre)
            {
                case "FPS":
                    ddlGames.Items.Add(new ListItem("Doom 1", "Doom1"));
                    ddlGames.Items.Add(new ListItem("Doom 2", "Doom2"));
                    ddlGames.Items.Add(new ListItem("Doom 3", "Doom3"));
                    ddlGames.Items.Add(new ListItem("Wolfenstein Castle", "WolfensteinCastle"));
                    ddlGames.Items.Add(new ListItem("Counterstrike", "Counterstrike"));
                    break;
                case "Cars":
                    ddlGames.Items.Add(new ListItem("Need For Speed 1", "NFS1"));
                    ddlGames.Items.Add(new ListItem("Need For Speed 2", "NFS2"));
                    break;
                default:
                    ddlGames.Items.Add(new ListItem("None..."));
                    break;
            }
        }
    }
}