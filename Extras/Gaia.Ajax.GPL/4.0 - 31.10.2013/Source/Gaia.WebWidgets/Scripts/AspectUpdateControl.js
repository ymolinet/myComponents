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
    Gaia.AspectUpdateControl = Class(Gaia.Aspect, {
        constructor: function(parentId, options) {
            Gaia.AspectUpdateControl.$super.call(this, parentId, options);
            $("#" + this.options.updateControl).hide();
        },

        startAjaxRequest: function() {
            $("#" + this.options.updateControl).show();
            return true;
        },

        endAjaxRequest: function() {
            $("#" + this.options.updateControl).hide();
            return true;
        }
    });
})(window, jQuery, jsface.Class);
