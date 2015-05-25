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
   AspectGeneric, adds up server-side Generic Event support for Controls
   --------------------------------------------------------------------------- */
(function(window, $, Class, undefined) {
    Gaia.AspectGeneric = Class(Gaia.AspectWithEvents, function () {
        var eventNamespace = '.aspect_generic';
        var timerId;

        return {
            constructor: function (parentId, options) {
                Gaia.AspectGeneric.$super.call(this, parentId, options);

                if (this.options.raiseEvent) {
                    this.onTimerTick = $.proxy(this.timerTick, this);
                    $("#" + parentId).on(this.options.eventName + eventNamespace, $.proxy(this.eventRaised, this));
                }
            },

            // overridden to initialize the events specified in AspectKey 
            initEvents: function () {
                var self = this;
                $.each(this.options.evts, function(idx, evt) {
                    self.addEvent(evt, $.proxy(self.dispatchGenericEvent, self, evt));
                });
            },

            // this function is used for the generic event handlers themselves. 
            dispatchGenericEvent: function (evtValue) {
                Gaia.Control.callAspectMethod.call(this.getWrappedControl(), 'EventFiredMethod', [evtValue]);
            },

            // we use a timer to delay the servercall for a specified interval.
            timerTick: function() {
                Gaia.Control.callAspectMethod.call(this.getWrappedControl(), 'EventRaisedMethod');
                timerId = undefined;
            },

            // Called when the attached event for the wrapper element is fired. No extra arguments
            // is passed to the server. If you need more specific handling use another aspect
            eventRaised: function () {
                if (timerId) return;
                timerId = setTimeout(this.onTimerTick, this.options.interval);
            },

            destroy: function() {
                if (this.options.raiseEvent) {
                    $("#" + this.parentId).off(eventNamespace);
                }

                if (timerId) {
                    clearTimeout(timerId);
                    timerId = undefined;
                }
                
                Gaia.AspectGeneric.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
