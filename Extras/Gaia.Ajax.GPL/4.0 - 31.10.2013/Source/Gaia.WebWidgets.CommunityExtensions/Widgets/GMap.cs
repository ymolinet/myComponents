/*******************************************************************
 * Google Maps for ASP.NET  
 * Copyright (C) 2009 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under GPL version 3 
 * as published by the Free Software Foundation
 ******************************************************************/

using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web.UI;
using Gaia.WebWidgets.HtmlFormatting;

[assembly: WebResource("Gaia.WebWidgets.CommunityExtensions.Scripts.GMap.js", "text/javascript")]
namespace Gaia.WebWidgets.CommunityExtensions
{
    /// <summary>
    /// GMap is an example on how you can extend Gaia Ajax by creating your own custom extension. 
    /// </summary>
    [ToolboxData("<{0}:GMap runat=\"server\" />")]
    [ToolboxBitmap(typeof(GMap), "Resources.Gaia.WebWidgets.CommunityExtensions.GMap.bmp")]
    public class GMap : GaiaWebControl, IAjaxControl, IExtraPropertyCallbackRenderer
    {
        //TODO
        //fix event in property MapType properly, put it in OnLoad and check for changed value
        //implement flags
        //follow gwt api, with methods, no properties? how is this different in C# and Java?
        //infowindows options
        //inspiration http://gwt.google.com/samples/HelloMaps-1.0.4/HelloMaps.html
        //use this.map.setUIToDefault() in JS to use the defaults, 
        //but make sure to grab the right control to remove it, lays double today
        //too many events fired when e.g. adding InfoWindow
        //set standard size for control

        #region [ -- Contstructors -- ]
        public GMap()
        {
      
        }

        public GMap(LatLng latLng, int zoomLevel)
        {
            CenterLatLng = latLng;
            ZoomLevel = zoomLevel;
        }

        #endregion

        #region [ -- Private Members -- ]

        private string _infoWndHtml;
        private LatLng _infoWndGLatLng;
        private bool _infoWndClose;
        private LatLng _setCenterLatLng;
        private int? _setZoomLevel;
        private LatLng _panCenterLatLng;

        #endregion

        #region [ -- Public Properties -- ]

        /// <summary>
        /// True if it should be possible to drag the map
        /// </summary>
        [DefaultValue(true)]
        [AjaxSerializable("isDraggable")]
        public bool IsDraggable
        {
            get
            {
                if (ViewState["IsDraggable"] == null)
                    return true;
                return (bool)ViewState["IsDraggable"];
            }
            set { ViewState["IsDraggable"] = value; }
        }

        /// <summary>
        /// True if you want to display the zoom pan control
        /// </summary>
        [DefaultValue(true)]
        [AjaxSerializable("setShowZoomPanControls")]
        public bool ShowZoomPanControls
        {
            get
            {
                if (ViewState["ShowZoomPanControls"] == null)
                    return true;
                return (bool)ViewState["ShowZoomPanControls"];
            }
            set { ViewState["ShowZoomPanControls"] = value; }
        }

        /// <summary>
        /// True if you want to display the standard map type control
        /// </summary>
        [DefaultValue(true)]
        [AjaxSerializable("setShowMapTypeControls")]
        public bool ShowMapTypeControls
        {
            get
            {
                if (ViewState["ShowMapTypeControls"] == null)
                    return true;
                return (bool)ViewState["ShowMapTypeControls"];
            }
            set { ViewState["ShowMapTypeControls"] = value; }
        }

        /// <summary>
        /// Set the map type
        /// </summary>
        [DefaultValue(MapTypeEnum.Normal)]
        [AjaxSerializable("setMapType")]
        public MapTypeEnum MapType
        {
            get
            {
                if (ViewState["MapType"] == null)
                    ViewState["MapType"] = MapTypeEnum.Normal;
                return (MapTypeEnum)Enum.Parse(typeof(MapTypeEnum), ViewState["MapType"].ToString());
            }
            set
            {
                MapTypeEnum initialValue = MapType;
                ViewState["MapType"] = value;
                if (MapTypeChanged != null && initialValue != value)
                    MapTypeChanged(this, new MapTypeChangedEventArgs(MapType));
            }
        }

        /// <summary>
        /// The Google Maps API key assigned from Google. Must be configured in web.config
        /// of your web application.
        /// If you want to deploy this application outside of localhost, you must obtain a 
        /// Google Maps API key at: http://www.google.com/apis/maps/signup.html 
        /// </summary>
        public string GoogleMapsApiKey
        {
            get { return ConfigurationManager.AppSettings["GoogleMapsApiKey"]; }
        }

        #endregion

        #region [ -- Private Properties -- ]
        private int ZoomLevel
        {
            get
            {
                //set initial ZoomLevel
                if (ViewState["ZoomLevel"] == null)
                    ViewState["ZoomLevel"] = 5;
                return (int) ViewState["ZoomLevel"];
            }
            set { ViewState["ZoomLevel"] = value; }
        }

        private LatLng CenterLatLng
        {
            get
            {
                //set initial LatLng
                if (ViewState["CenterLatLng"] == null)
                    ViewState["CenterLatLng"] = new LatLng(37.4419f, -122.1419f);

                return (LatLng) ViewState["CenterLatLng"];
            }
            set { ViewState["CenterLatLng"] = value; }
        }
        #endregion

        #region [ -- Gaia Internal Helper Methods -- ]

        [Method]
        internal string MapTypeChangedMethod(string maptype)
        {
            //set the property which will fire the event
            MapType = (MapTypeEnum) Enum.Parse(typeof (MapTypeEnum), maptype);

            return string.Empty;
        }

        [Method]
        internal string ClickMethod(string latitude, string longitude)
        {
            if (Click != null)
                Click(this,
                      new LatLngEventArgs(new LatLng(float.Parse(latitude, CultureInfo.InvariantCulture),
                                                     float.Parse(longitude, CultureInfo.InvariantCulture))));

            return string.Empty;
        }

        [Method]
        internal string MoveStartMethod()
        {
            if (MoveStart != null)
                MoveStart(this, EventArgs.Empty);

            return string.Empty;
        }

        [Method]
        internal string MoveEndMethod(string latitude, string longitude)
        {
            if (MoveEnd != null)
            {
                //update value
                CenterLatLng = new LatLng(float.Parse(latitude, CultureInfo.InvariantCulture),
                                          float.Parse(longitude, CultureInfo.InvariantCulture));

                //fire event
                MoveEnd(this, EventArgs.Empty);
            }
            return string.Empty;
        }

        [Method]
        internal string ZoomMethod(string oldZoomLevel, string newZoomLevel)
        {
            if (Zoom != null)
                Zoom(this, new ZoomEventArgs(int.Parse(oldZoomLevel), int.Parse(newZoomLevel)));

            return string.Empty;
        }

        #endregion

        #region [ -- Public Methods -- ]
        /// <summary>
        /// Returns the geographical coordinates of the center point of the map view
        /// </summary>
        /// <returns>Return the center of the map view</returns>
        public LatLng GetCenter()
        {
            return CenterLatLng;
        }

        /// <summary>
        /// Return the zoom level of the map view
        /// </summary>
        /// <returns>The zoom level</returns>
        public int GetZoomLevel()
        {
            return ZoomLevel;
        }

        /// <summary>
        /// Sets a new zoomlevel
        /// </summary>
        /// <param name="zoomLevel">New zoomlevel</param>
        public void SetZoomLevel(int zoomLevel)
        {
            ZoomLevel = zoomLevel;
            _setZoomLevel = zoomLevel;
        }

        /// <summary>
        /// Sets a new center of the map
        /// </summary>
        /// <param name="latLng">The coordinates for the center</param>
        public void SetCenter(LatLng latLng)
        {
            CenterLatLng = latLng;
            _setCenterLatLng = latLng;
        }

        /// <summary>
        /// Sets a new center of the map
        /// </summary>
        /// <param name="latLng">The coordinates to move to</param>
        /// <param name="zoomLevel">The zoom level to set</param>
        public void SetCenter(LatLng latLng, int zoomLevel)
        {
            CenterLatLng = latLng;
            _setCenterLatLng = latLng;
            ZoomLevel = zoomLevel;
            _setZoomLevel = zoomLevel;
        }

        /// <summary>
        /// Pan to a new position on the map
        /// </summary>
        /// <param name="gLatLng">The coordinate to pan to</param>
        public void PanTo(LatLng gLatLng)
        {
            CenterLatLng = gLatLng;
            _panCenterLatLng = gLatLng;
        }

        /// <summary>
        /// Open an infowindow with Html on the current position
        /// </summary>
        /// <param name="html">Html to display in the infowindow</param>
        public void OpenInfoWindow(string html)
        {
            OpenInfoWindow(CenterLatLng, html);
        }

        /// <summary>
        /// Open an infowindow with Html on specified position
        /// </summary>
        /// <param name="gLatLng">Coordinate to show the infowindow</param>
        /// <param name="html">Html to display in the infowindow</param>
        public void OpenInfoWindow(LatLng gLatLng, string html)
        {
            if (_infoWndGLatLng == null)
                _infoWndGLatLng = new LatLng();

            _infoWndHtml = html;
            _infoWndGLatLng.Longitude = gLatLng.Longitude;
            _infoWndGLatLng.Latitude = gLatLng.Latitude;
        }
        
        /// <summary>
        /// Close current open infowindow
        /// </summary>
        public void CloseInfoWindow()
        {
            _infoWndClose = true;
        }

        #endregion

        #region [ -- Events -- ]
        /// <summary>
        /// This Event is fired when the Map Type has been changed
        /// </summary>
        public event EventHandler<MapTypeChangedEventArgs> MapTypeChanged;

        /// <summary>
        /// This event is fired when the user clicks on the map with the mouse. 
        /// A click event passes different arguments based on the context of the click, 
        /// and whether or not the click occured on a clickable overlay. If the click 
        /// does not occur on a clickable overlay, the overlay argument is null and the 
        /// latlng argument contains the geographical coordinates of the point that was 
        /// clicked. If the user clicks on an overlay that is clickable (such as a GMarker, 
        /// GPolygon, GPolyline, or GInfoWindow), the overlay argument contains the overlay 
        /// object, while the overlaylatlng argument contains the coordinates of the clicked 
        /// overlay. In addition, a click event is then also fired on the overlay itself.
        /// </summary>
        public event EventHandler<LatLngEventArgs> Click;

        /// <summary>
        /// This event is fired when the map view starts changing. 
        /// </summary>
        public event EventHandler MoveStart;

        /// <summary>
        /// This event is fired when the change of the map view ends.
        /// </summary>
        public event EventHandler MoveEnd;

        /// <summary>
        /// This event is fired when the map reaches a new zoom level. 
        /// The event handler receives the previous and the new zoom level as arguments.
        /// </summary>
        public event EventHandler<ZoomEventArgs> Zoom;

        public class LatLngEventArgs : EventArgs
        {
            public LatLngEventArgs(LatLng latLng)
            {
                _latLng = latLng;
            }

            private LatLng _latLng;
            public LatLng LatLng
            {
                get { return _latLng; }
                set { _latLng = value; }
            }
        }

        /// <summary>
        /// EventArgs for the MapTypeChanged event. 
        /// </summary>
        public class MapTypeChangedEventArgs : EventArgs
        {
            private readonly MapTypeEnum _mapType;
            internal MapTypeChangedEventArgs(MapTypeEnum mapType)
            {
                _mapType = mapType;
            }

            public MapTypeEnum MapType
            {
                get { return _mapType; }
            }
        }

        /// <summary>
        /// Event args for the MoveEnd event.
        /// </summary>
        public class MovedEventArgs : EventArgs
        {
            public MovedEventArgs(LatLng latLng)
            {
                _latLng = latLng;
            }

            private LatLng _latLng;
            public LatLng LatLng
            {
                get { return _latLng; }
                set { _latLng = value; }
            }
        }

        /// <summary>
        /// Event args for Zoom event.
        /// </summary>
        public class ZoomEventArgs : EventArgs
        {
            public ZoomEventArgs(int oldZoomLevel, int newZoomLevel)
            {
                _oldZoomLevel = oldZoomLevel;
                _newZoomLevel = newZoomLevel;
            }

            private int _oldZoomLevel;
            public int OldZoomLevel
            {
                get { return _oldZoomLevel; }
                set { _oldZoomLevel = value; }
            }

            private int _newZoomLevel;
            public int NewZoomLevel
            {
                get { return _newZoomLevel; }
                set { _newZoomLevel = value; }
            }
        }

        #endregion

        #region [ -- Protected Methods for Inheritance -- ]
        /// <summary>
        /// Render GMap control
        /// </summary>
        /// <param name="create"></param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (Tag div = create.Div(ClientID, CssClass).SetStyle("width:" + Width.Value + "px;height:" + Height.Value + "px"))
            {
                Css.SerializeAttributesAndStyles(this, div);
            }
        }

        /// <summary>
        /// Include required javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // Include GMap Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.prototype.js", typeof(GMap), "");
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.CommunityExtensions.Scripts.GMap.js", typeof(GMap), "Gaia.Extensions.GMap.browserFinishedLoading");
            
            // Google Maps API Script
            string googleMapsScriptPath = "http://maps.google.com/maps?file=api&v=2&key=" + GoogleMapsApiKey;
            Manager.Instance.AddInclusionOfFile(googleMapsScriptPath, typeof (GMap), googleMapsScriptPath,
                                                "GLoadMapsScript");
        }

        #endregion

        #region [ -- Overridden IAjaxControl methods -- ]

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Extensions.GMap", ClientID)
                .AddProperty("moveStart", MoveStart != null)
                //we always fire moveEnd so we can update new center coords
                //.AddProperty("moveEnd", MoveEnd != null)
                .AddProperty("click", Click != null)
                .AddProperty("zoomEvent", Zoom != null)
                .AddProperty("maptypechanged", MapTypeChanged != null)
                .AddProperty("initZoomLevel", ZoomLevel)
                .AddProperty("initLat", CenterLatLng.Latitude.ToString(CultureInfo.InvariantCulture))
                .AddProperty("initLong", CenterLatLng.Longitude.ToString(CultureInfo.InvariantCulture))
                .AddProperty("mapType", MapType.ToString())
                .AddProperty("setShowZoomPanControls", ShowZoomPanControls)
                .AddProperty("isDraggable", IsDraggable).ToString();
        }

        #endregion

        #region [ -- IExtraPropertyCallbackRenderer Implementation -- ]

        private void PrepareBuilder(StringBuilder builder, bool first)
        {
            if (first) return;
            builder.AppendFormat(";$G('{0}')", ClientID);
        }

        public void InjectPropertyChangesToCallbackResponse(StringBuilder code)
        {
            bool isFirstScript = true;
            if (!string.IsNullOrEmpty(_infoWndHtml))
            {
                //send directly to Google Map instance, without wrapper in GMap.js
                code.AppendFormat(".map.setCenter(new GLatLng({1}, {2}), $G('{0}').map.getZoom());" +
                                  "$G('{0}').map.openInfoWindow($G('{0}').map.getCenter(), document.createTextNode('{3}'))",
                                  ClientID,
                                  _infoWndGLatLng.Latitude.ToString(CultureInfo.InvariantCulture),
                                  _infoWndGLatLng.Longitude.ToString(CultureInfo.InvariantCulture),
                                  _infoWndHtml);
                isFirstScript = false;
            }

            if (_infoWndClose)
            {
                PrepareBuilder(code, isFirstScript);
                code.Append(".map.closeInfoWindow()");
                isFirstScript = false;
            }

            //do setCenter operations, with or without zoom
            if (_setCenterLatLng != null)
            {
                PrepareBuilder(code, isFirstScript);
                if (!_setZoomLevel.HasValue)
                {
                    code.AppendFormat(".map.setCenter(new GLatLng({0},{1}))",
                                      _setCenterLatLng.Latitude.ToString(CultureInfo.InvariantCulture),
                                      _setCenterLatLng.Longitude.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    code.AppendFormat(".map.setCenter(new GLatLng({0},{1}), {2})",
                                      _setCenterLatLng.Latitude.ToString(CultureInfo.InvariantCulture),
                                      _setCenterLatLng.Longitude.ToString(CultureInfo.InvariantCulture),
                                      _setZoomLevel.Value);
                }
                isFirstScript = false;
            }
            else if (_setZoomLevel.HasValue)
            {
                PrepareBuilder(code, isFirstScript);
                code.AppendFormat(".map.setZoom({0})", _setZoomLevel.Value);
                isFirstScript = false;
            }

            if (_panCenterLatLng != null)
            {
                PrepareBuilder(code, isFirstScript);
                code.AppendFormat(".map.panTo(new GLatLng({0},{1}))",
                                  _panCenterLatLng.Latitude.ToString(CultureInfo.InvariantCulture),
                                  _panCenterLatLng.Longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region [ --- IAjaxControl Members -- ]

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerWebControl(this, ClientID, this);
        }

        #endregion

        #region [ -- GMap Enums --]
        public enum MapTypeEnum
        {
            Normal,
            Satellite,
            Hybrid,
            Physical
        }

        #endregion

        #region [ -- GMap Helper Classes -- ]
        /// <summary>
        /// A Point represents a point on the map by its pixel coordinates
        /// </summary>
        public class Point
        {
            public Point(float x, float y)
            {
                _x = x;
                _y = y;
            }

            private float _x;
            public float X
            {
                get { return _x; }
                set { _x = value; }
            }

            private float _y;
            public float Y
            {
                get { return _y; }
                set { _y = value; }
            }
        }

        /// <summary>
        /// LatLng is a point in geographical coordinates longitude and latitude.
        /// </summary>
        [Serializable]
        public class LatLng
        {
            public LatLng() { }

            public LatLng(float latitude, float longitude)
            {
                _latitude = latitude;
                _longitude = longitude;
            }

            private float _latitude;
            private float _longitude;
            public float Latitude
            {
                get { return _latitude; }
                set { _latitude = value; }
            }

            public float Longitude
            {
                get { return _longitude; }
                set { _longitude = value; }
            }
        }
        #endregion
    }
}