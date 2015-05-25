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
   Class wrapping a FishEyeMenu that fakes the Mac OS X taskbar
   --------------------------------------------------------------------------- */

if( !Gaia.Extensions )
  Gaia.Extensions = Class.create();
 
Gaia.Extensions.FishEyeMenu = jsface.Class(Gaia.Panel, {

  constructor: function (element, options) {
    Gaia.Extensions.FishEyeMenu.$super.call(this, element, options);
    this.initializeFishEyeMenu(element, options);
  },

  initializeFishEyeMenu: function(element, options) {

    // set properties 
    this.options.startSize = this.options.startSize || 55;
    this.options.endSize = this.options.endSize || 88;
    this.options.threshold = this.options.threshold || 300;
    this._currentlyEnabled = this.options.enabled || true;


    // all image elements within this element.
    this.animateElements = $$('#' + this.element[0].id + ' input[type="image"]');
    var tmpArray = $$('#' + this.element[0].id + ' img')
    tmpArray.each(function(idx){
      this.animateElements.push(idx);
    }.bind(this));
    Element.observe(document, 'mousemove', this.onBodyMove.bindAsEventListener(this));
  },
  
  setEnabled: function(value) {
    this._currentlyEnabled = value;
    return this;
  },

  isEnabled: function() {
    return this._currentlyEnabled;
  },

  onBodyMove: function(event) {
    if (!this._currentlyEnabled) {
      return;
    }
    
    for (var y = 0; y < this.animateElements.length; y ++) {
      var s = this.animateElements[y];
      var position = Position.cumulativeOffset(s);
      var xOff = Math.abs(Event.pointerX(event) - (position[0] + (s.offsetWidth / 2)));
      var yOff = Math.abs(Event.pointerY(event) - (position[1] + (s.offsetHeight / 2)));
      var offset = Math.sqrt((xOff*xOff)+(yOff*yOff));
      if( offset < this.options.threshold ){
        offset = offset || 1; // Prevent divide by zero bugs
        var delta = this.options.endSize - this.options.startSize;
        var factorIncrease = 1 - (offset / this.options.threshold);
        s.style.width = (parseInt(this.options.startSize + (delta * factorIncrease), 10)) + 'px';
        s.style.height = (parseInt(this.options.startSize + (delta * factorIncrease), 10)) + 'px';
      } else {
        if( parseInt(s.style.width, 10) != this.options.startSize ){
          s.style.width = this.options.startSize+'px';
          s.style.height = this.options.startSize+'px';
        }
      }
    }
  }
});
