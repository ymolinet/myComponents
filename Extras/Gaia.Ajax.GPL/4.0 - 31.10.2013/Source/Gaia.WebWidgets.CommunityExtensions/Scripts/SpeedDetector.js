// Gaia Ajax Copyright (C) 2008 - 2013 Gaiaware AS. details at http://gaiaware.net/

/* 
 * Gaia Ajax - Ajax Control Library for ASP.NET
 * Copyright (C) 2008 - 2013 Gaiaware AS
 * All rights reserved.
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by
 * Gaiaware AS
 * read the details at http://gaiaware.net
 */
 
/* ---------------------------------------------------------------------------
   Class wrapping a SpeedDetector that will discover the client's bandwith
   --------------------------------------------------------------------------- */

if( !Gaia.Extensions )
  Gaia.Extensions = Class.create();
 
Gaia.Extensions.SpeedDetector = jsface.Class(Gaia.Control, {

  constructor: function(element, options) {
      Gaia.Extensions.SpeedDetector.$super.call(this, element, options);
      this.initializeSpeedDetector(element, options);
  },

  initializeSpeedDetector: function(element, options) {

    if (this.options.startDetection)
      setTimeout(this.startDetection.bind(this), 500);
  },
  
  startDetection: function(value){

    // find the target for the ajax request. this is either the downloadFile passed as an option or it's the control which can generate lorem ipsum content
    var target = this.options.downloadFile || Gaia.Control._defaultUrl + (Gaia.Control._defaultUrl.indexOf('?') != -1 ? '&' : '?') + 'Gaia.WebWidgets.CommunityExtensions.SpeedDetector.GetLoremIpsum=' + this.element[0].id + '&DownloadSize=' + this.options.downloadSize;
    var ajaxMethod = (this.options.downloadFile) ? 'get' : 'post';
    timeBefore = new Date().getTime();
    
    new Ajax.Request(target, {
      method: ajaxMethod,
      onSuccess: function(transport) {
        
        timeAfter = new Date().getTime();
        timeSpent = timeAfter - timeBefore;
        
        Gaia.Control.callControlMethod.bind(this)('SpeedDetectionCompleteMethod', [timeSpent], function(){}, this.element[0].id);

      }.bind(this)
    });
        
    return this;
  },
  
  _getElementPostValue: function(){
    return '';
  }

});
