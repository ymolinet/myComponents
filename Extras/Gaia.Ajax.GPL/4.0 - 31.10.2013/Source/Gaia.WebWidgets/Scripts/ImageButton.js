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

(function (window, $, Class, undefined) {
    Gaia.ImageButton = Class(Gaia.ButtonControl, {
        constructor: function(element, options) {
            Gaia.ImageButton.$super.call(this, element, options);
            this.initializeDefaultValidation();
        },

        setImageUrl: function(value, index) {
            var me = this;
            this.element.attr('src', function(idx, src) {
                return me.decodeArgument(src, value, index);
            });
            return this;
        },

        setText: function(value, index) {
            var me = this;
            this.element.attr('alt', function(idx, src) {
                return me.decodeArgument(src, value, index);
            });
            return this;
        },

        _getElementPostValueEvent: function(evt) {
            var callbackName = this.getCallbackName();
            if (callbackName === this.element.attr('name')) {
                var offset = this.element.offset();
                var x = parseInt(offset.left, 10);
                var y = parseInt(offset.top, 10);
                return '&' + callbackName + '.x=' + x + '&' + callbackName + '.y=' + y;
            } else {
                return '&__EVENTTARGET=' + callbackName + '&__EVENTARGUMENT=' + this.options.arg;
            }
        }
    });
})(window, jQuery, jsface.Class);