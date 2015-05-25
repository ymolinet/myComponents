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

/* ---------------------------------------------------------------------------------------------------------------
   A class to keep generic ClientSide utilities and DOM functions. 
   --------------------------------------------------------------------------------------------------------------- */
(function(window, $, Class, undefined) {
    Gaia.ClientUtils = Class(Gaia.Control, {
        constructor: function(element, options) {
            Gaia.ClientUtils.$super.call(this, element, options);

            // faking a full Gaia Ajax Control by using detached DOM node.
            this.element = $('<div />').attr('id', element).hide();
        },

        // Positions target element just below source element.
        setClonePosition: function(source, target) {
            var offset = $("#" + source).offset();
            var height = $("#" + source).outerHeight();

            $("#" + target)
                .css("position", "absolute")
                .offset({
                    left: offset.left,
                    top: offset.top + height
                });

            return this;
        }
    });
})(window, jQuery, jsface.Class);