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

(function(window, $, Class, undefined) {
    Gaia.Button = Class(Gaia.ButtonControl, {
        constructor: function (element, options) {
            Gaia.Button.$super.call(this, element, options);
            this.initializeDefaultValidation();
        },

        setSubmit: function (value) {
            this.element.attr('type', !!value ? 'submit' : 'button');
            return this;
        },

        _getElementPostValueEvent: function() {
            var callbackName = this.getCallbackName();
            if (callbackName !== this.element.attr('name'))
                return '&__EVENTTARGET=' + callbackName + '&__EVENTARGUMENT=' + this.options.arg;
            else
                return '&' + callbackName + '=' + $(this.element).val();
        }
    });
})(window, jQuery, jsface.Class);
