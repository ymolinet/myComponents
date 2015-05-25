/*******************************************************************
* Gaia Ajax - Ajax Control Library for ASP.NET  
* Copyright (C) 2008 - 2013 Gaiaware AS
* All rights reserved. 
* This program is distributed under either GPL version 3 
* as published by the Free Software Foundation or the
* Gaia Commercial License version 1 as published by Gaiaware AS
* read the details at http://gaiaware.net/product/dual-licensing 
******************************************************************/

(function(window, $, Class, undefined) {
    Gaia.ButtonControl = Class(Gaia.WebControl, {
        constructor: function(element, options) {
            Gaia.ButtonControl.$super.call(this, element, options);
        },

        setText: function(value, index) {
            this.element.val(this.decodeArgument(this.element.val(), value, index));
            return this;
        },

        // Sets event bubbling
        setBubble: function(value) {
            this.stopObserve('click');
            this.observe('click', value);
            return this;
        },

        // Sets text of button
        setPostUrl: function(value, index) {
            this.options.url = this.decodeArgument(this.options.url, value, index);
            return this;
        },

        setTgt: function(value, index) {
            this.options.callbackName = this.decodeArgument(this.getCallbackName(), value, index);
            return this;
        },

        setArg: function(value) {
            this.options.arg = value;
            return this;
        },

        // Sets a function to call when button is clicked
        setOnClick: function(value) {
            if (value) {
                this.element.onclick = $.proxy(new Function(value), this.element);
            } else {
                this.element.onclick = null;
                this.element.removeAttribute('onclick');
            }
            return this;
        }
    });
})(window, jQuery, jsface.Class);
