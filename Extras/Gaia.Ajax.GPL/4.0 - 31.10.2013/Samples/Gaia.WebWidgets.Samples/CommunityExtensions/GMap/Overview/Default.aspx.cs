namespace Gaia.WebWidgets.Samples.CommunityExtensions.GMap.Overview
{
    using System;
    using System.Drawing;
    using System.Globalization;

    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //initialize
                GMap1.SetCenter(new Gaia.WebWidgets.CommunityExtensions.GMap.LatLng(Convert.ToSingle(txtLatitude.Text, CultureInfo.InvariantCulture),
                                                Convert.ToSingle(txtLongitude.Text, CultureInfo.InvariantCulture)),
                                int.Parse(ddlZoom.SelectedValue));
            }
        }

        protected void btnGoTo_Click(object sender, EventArgs e)
        {
            try
            {
                GMap1.SetCenter(new Gaia.WebWidgets.CommunityExtensions.GMap.LatLng(Single.Parse(txtLatitude.Text, CultureInfo.InvariantCulture),
                                                Single.Parse(txtLongitude.Text, CultureInfo.InvariantCulture)));

                ShowMessage(string.Format("Moved to Lat: {0} Long: {1}",
                                          GMap1.GetCenter().Latitude,
                                          GMap1.GetCenter().Longitude));
            }
            catch
            {
                err.Text = "You must write the right types of values into the text boxes";
            }
        }

        protected void btnPanTo_Click(object sender, EventArgs e)
        {
            try
            {
                GMap1.PanTo(new Gaia.WebWidgets.CommunityExtensions.GMap.LatLng(Convert.ToSingle(txtLatitude.Text, CultureInfo.InvariantCulture),
                                            Convert.ToSingle(txtLongitude.Text, CultureInfo.InvariantCulture)));
                ShowMessage(string.Format("Paned to Lat: {0} Long: {1}", txtLatitude.Text, txtLongitude.Text));
            }
            catch
            {
                err.Text = "You must write the right types of values into the text boxes";
            }
        }

        protected void ddlZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            GMap1.SetZoomLevel(int.Parse(ddlZoom.SelectedValue));
        }

        protected void btnAddInfoWindow_Click(object sender, EventArgs e)
        {
            GMap1.OpenInfoWindow(new Gaia.WebWidgets.CommunityExtensions.GMap.LatLng(62.50826f, 25.98926f), "Hello Finland.");
            ShowMessage("InfoWindow added in Finland");
        }

        protected void btnCloseInfoWindow_Click(object sender, EventArgs e)
        {
            GMap1.CloseInfoWindow();
            ShowMessage("InfoWindow closed");
        }

        protected void btnToggleZoomControls_Click(object sender, EventArgs e)
        {
            GMap1.ShowZoomPanControls = !GMap1.ShowZoomPanControls;
            ShowMessage("Zoom control is now: " + (GMap1.ShowZoomPanControls ? "Visible" : "Invisible"));
        }

        protected void btnToggleDragging_Click(object sender, EventArgs e)
        {
            GMap1.IsDraggable = !GMap1.IsDraggable;
            ShowMessage("Map is now: " + (GMap1.IsDraggable ? "Draggable" : "Not Draggable"));
        }

        protected void btnGetCenterLatLng_Click(object sender, EventArgs e)
        {
            ShowMessage(string.Format("Get Coordinates: Latitude: {0} Longitude: {1}",
                                      GMap1.GetCenter().Latitude,
                                      GMap1.GetCenter().Longitude));
        }

        protected void GMap1_Click(object sender, Gaia.WebWidgets.CommunityExtensions.GMap.LatLngEventArgs e)
        {
            ShowMessage(string.Format("Click event. Clicked Lat: {0} Long: {1}", e.LatLng.Latitude, e.LatLng.Longitude));
        }

        protected void GMap1_MapTypeChanged(object sender, Gaia.WebWidgets.CommunityExtensions.GMap.MapTypeChangedEventArgs e)
        {
            ddlMapType.Items.FindByValue(e.MapType.ToString()).Selected = true;
            ShowMessage("MapTypeChanged event. Changed to: " + e.MapType);
        }

        protected void GMap1_MoveStart(object sender, EventArgs e)
        {
            ShowMessage("MoveStart event");
        }

        protected void GMap1_MoveEnd(object sender, EventArgs e)
        {
            txtLatitude.Text = GMap1.GetCenter().Latitude.ToString(CultureInfo.InvariantCulture);
            txtLongitude.Text = GMap1.GetCenter().Longitude.ToString(CultureInfo.InvariantCulture);

            ShowMessage(string.Format("MoveEnd. New coordinates - Lat: {0} Long: {1}",
                                      GMap1.GetCenter().Latitude,
                                      GMap1.GetCenter().Longitude));
        }

        protected void GMap1_Zoom(object sender, Gaia.WebWidgets.CommunityExtensions.GMap.ZoomEventArgs e)
        {
            ShowMessage(string.Format("Zoom event fired. Old level: {0} New level: {1} ", e.OldZoomLevel, e.NewZoomLevel));
            ddlZoom.Items.FindByValue(e.NewZoomLevel.ToString()).Selected = true;
        }

        protected void ddlMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GMap1.MapType = (WebWidgets.CommunityExtensions.GMap.MapTypeEnum)Enum.Parse(typeof(WebWidgets.CommunityExtensions.GMap.MapTypeEnum), ddlMapType.SelectedValue);
        }

        private void ShowMessage(string message)
        {
            msg.Text = message;
            msg.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight(Color.DarkOrange, Color.Orange, Color.White));
        }
    }
}