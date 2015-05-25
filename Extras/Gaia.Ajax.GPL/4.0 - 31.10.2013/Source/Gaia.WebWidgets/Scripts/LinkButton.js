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
    Gaia.LinkButton = Class(Gaia.ButtonControl, {

        constructor: function (element, options) {
            Gaia.LinkButton.$super.call(this, element, options);
            this.initializeDefaultValidation();
            this.href = this.element.attr("href") || "javascript:void(0)";
        },

        setEnabled: function (value) {
            Gaia.LinkButton.$superp.setEnabled.call(this, value);
            if (value) {
                this.element.attr("href", this.href);
            } else {
                this.element.removeAttr("href");
            }
            return this;
        },

        setText: function (value, index) {
            var me = this;
            me.element.html(function(idx, html) {
                return me.decodeArgument(html, value, index);
            });
            return me;
        },

        _getElementPostValueEvent: function () {
            return '&__EVENTTARGET=' + this.getCallbackName() + '&__EVENTARGUMENT=' + this.options.arg;
        } 
    });

})(window, jQuery, jsface.Class);
