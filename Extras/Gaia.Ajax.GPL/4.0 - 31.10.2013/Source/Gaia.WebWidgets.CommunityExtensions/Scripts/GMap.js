/*
* Google Maps for ASP.NET  
* Copyright (C) 2009-2013 Gaiaware AS
* All rights reserved. 
* This program is distributed under GPL version 3 
* as published by the Free Software Foundation
*/

if( !Gaia.Extensions )
  Gaia.Extensions = Class.create();

Gaia.Extensions.GMap = jsface.Class(Gaia.WebControl, {
  constructor: function(element, options){
    Gaia.Extensions.GMap.$super.call(this, element, options);
    this.initializeMap();
  },
  
  initializeMap: function(){
     if ( !GBrowserIsCompatible() )
      return this;

    this.map = new GMap2(this.element[0]);
    this.map.setCenter(new GLatLng(this.options.initLat,this.options.initLong),this.options.initZoomLevel);
    
    this.options.gmaptypecontrol = new GMapTypeControl();
    
    this.map.addControl(this.options.gmaptypecontrol);
    
    this.options.glargemapcontrol3d = new GLargeMapControl3D();
    
    if (this.options.setShowZoomPanControls) {
        this.map.addControl(this.options.glargemapcontrol3d);
      }
        
    if (this.options.isDraggable === null || this.options.isDraggable === false) {
        this.map.disableDragging();
     }
     
    this.setMapType(this.options.mapType);
    
    this.setZoomLevel(this.options.zoomLevel);
    
     //listen to maptypechanged
    this.maptypechangedbinding = GEvent.bind(this.map, 'maptypechanged', this, this._onmaptypechanged);
    
    //listen to click event
    this.mapclickbinding = GEvent.bind(this.map, 'click', this, this._onclick);
    
    //listen to movesteart event
    this.mapmovestartbinding = GEvent.bind(this.map, 'movestart', this, this._movestart);
    
     //listen to moveend event
    this.mapmoveendbinding = GEvent.bind(this.map, 'moveend', this, this._moveend);
    
    //listen to zoom event
    this.mapzoombinding = GEvent.bind(this.map, 'zoomend', this, this._zoom);
    
    return this;
  },
  
  _onmaptypechanged: function(){
    if (this.options.maptypechanged) {
    var map_type; 
    if (this.map.getCurrentMapType() == G_NORMAL_MAP) 
        map_type = "Normal"; 
    if (this.map.getCurrentMapType() == G_SATELLITE_MAP) 
        map_type = "Satellite"; 
    if (this.map.getCurrentMapType() == G_HYBRID_MAP) 
        map_type = "Hybrid";
    if (this.map.getCurrentMapType() == G_PHYSICAL_MAP)
        map_type = "Physical";
    
    Gaia.Control.callControlMethod.bind(this)('MapTypeChangedMethod', [map_type], null, this.map.id);
    }
    return this;
  },
  
  _onclick: function(overlay, point) {
    if (this.options.click && point != null) {
     Gaia.Control.callControlMethod.bind(this)('ClickMethod', [point.x, point.y], null, this.map.id);
    }
    return this;
  },
  
  _movestart: function() {
    if (this.options.moveStart) {
        Gaia.Control.callControlMethod.bind(this)('MoveStartMethod', [], null, this.map.id);
    }
    return this;
  },
  
  _moveend: function() {
    // we fire this each time, so we can update new center location
    //    if (this.options.moveEnd) 
    var getcenter = this.map.getCenter();
    Gaia.Control.callControlMethod.bind(this)('MoveEndMethod', [getcenter.lat(), getcenter.lng()], null, this.map.id);

    return this;
  },
  
  _zoom: function(oldLevel, newLevel) {
    if (this.options.zoomEvent){
        Gaia.Control.callControlMethod.bind(this)('ZoomMethod', [oldLevel, newLevel], null, this.map.id);
    }
    return this;
  },
  
  setMapType: function(value){
      var map_type;
      if (value == 'Normal')
        map_type = G_NORMAL_MAP;
      if (value == 'Satellite')
        map_type = G_SATELLITE_MAP;
      if (value == 'Hybrid')
        map_type = G_HYBRID_MAP;
      if (value ==  'Physical')
        map_type = G_PHYSICAL_MAP;
        
      this.map.setMapType(map_type);
      
      return this;
  },

  setZoomLevel: function(value){
    this.map.setZoom(value);
    return this;
  },
  
  setShowZoomPanControls: function(value){  
    if (value)
        this.map.addControl(this.options.glargemapcontrol3d);
    else
        this.map.removeControl(this.options.glargemapcontrol3d);
        
    return this;
  },
  
  setShowMapTypeControls: function(value){
    if (value)
        this.map.addControl(this.options.gmaptypecontrol);
    else
        this.map.removeControl(this.options.gmaptypecontrol);
        
    return this;
  },
  
  isDraggable: function(value){  
    value ? this.map.enableDragging() : this.map.disableDragging();
        
    return this;
  },
  
  _getElementPostValue: function(){
    return '';
  },
  
  _getElementPostValueEvent: function() {
    return '&__EVENTARGUMENT=&__EVENTTARGET=' + this.map.id;
  },
  
  destroy: function() {
  GEvent.removeListener(this.maptypechangedbinding);
  GEvent.removeListener(this.mapclickbinding);
  GEvent.removeListener(this.mapmovestartbinding);
  GEvent.removeListener(this.mapmoveendbinding);
  GEvent.removeListener(this.mapzoombinding);
  this._destroyImpl();
  }
  
});

Gaia.Extensions.GMap.browserFinishedLoading = true;
