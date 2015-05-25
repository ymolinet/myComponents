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
    Gaia.AspectClickable = Class(Gaia.AspectWithEvents, {
        constructor: function(parentId, options) {
            Gaia.AspectClickable.$super.call(this, parentId, $.extend({
                enableBubbling: false,
                evaluate: function() { return true; }
            }, options));
        },

        // overridden to initialize the events specified for AspectClickable
        initEvents: function () {
            var me = this;
            
            // Called when wrapped widget is clicked or double clicked
            // Note that this one might be very useful since it also passes the x and y coordinates back to the server.
            var dispatchClickEvent = function(evtName, event) {
                if (!!this.options.evaluate.call(this, event)) {
                    var data = this.getMouseEventData(event);
                    Gaia.Control.callAspectMethod.call(this.getWrappedControl(),
                        'ClickMethod', [evtName, data.x, data.y, data.offsetLeft, data.offsetTop, data.controlKeys]);
                }

                if (!this.options.enableBubbling) {
                    event.stopPropagation();
                }
            };

            $.each(this.options.evts, function(idx, evtName) {
                me.addEvent(evtName, $.proxy(dispatchClickEvent, me, evtName));
            });
        }
    });
})(window, jQuery, jsface.Class);