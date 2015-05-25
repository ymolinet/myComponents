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
    Gaia.DynamicImage = Class(Gaia.WebControl, function () {

        return {
            constructor: function (element, options) {
                Gaia.DynamicImage.$super.call(this, element, options);
            },

            setImageId: function (value) {
                value = Gaia.Control._defaultUrl + (Gaia.Control._defaultUrl.indexOf('?') !== -1 ? '&' : '?') + 'Gaia.WebWidgets.DynamicImage.GetImage=' + this.element.attr('id') + '&ImageId=' + value;
                this.element.attr("src", value);
                return this;
            },

            setAltText: function (value) {
                this.element.attr("alt", value);
                return this;
            }
        };
    });
})(window, jQuery, jsface.Class);