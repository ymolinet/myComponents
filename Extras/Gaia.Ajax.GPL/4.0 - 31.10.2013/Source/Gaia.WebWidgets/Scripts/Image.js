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
    Gaia.Image = Class(Gaia.WebControl, {

        constructor: function (element, options) {
            Gaia.Image.$super.call(this, element, options);
        },

        setImageUrl: function (value, index) {
            var me = this;
            this.element.attr('src', function(idx, src) {
                return me.decodeArgument(src, value, index);
            });
            return this;
        },

        setLongDesc: function (value) {
            this.element.attr('longDesc', value);
            return this;
        },

        setImgAlign: function (value) {
            this.element.attr('align', value.toLowerCase());
            return this;
        },

        setAlternateText: function (value) {
            this.element.attr('alt', value);
            return this;
        }
    });

})(window, jQuery, jsface.Class);