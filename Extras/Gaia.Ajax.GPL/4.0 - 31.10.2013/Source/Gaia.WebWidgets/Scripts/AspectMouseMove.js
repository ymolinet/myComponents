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
   AspectMouseMove, adds up server-side Mouse Move event support for Controls
   --------------------------------------------------------------------------- */
(function(window, $, Class, undefined) {
    Gaia.AspectMouseMove = Class(Gaia.AspectWithEvents, function () {
        var timer;
        
        return {
            constructor: function (parentId, options) {

                Gaia.AspectMouseMove.$super.call(this, parentId, $.extend({
                    enabled: true,
                    interval: 100,
                    hasEvent: false
                }, options));
            },

            // Creating the move event handlers
            // Note that this works so that it checks for movements of the mouse and if movements has occured
            // it will create a timer that when executed will call the server raising the OnMouseMove event.
            // Then when the server is called it will start listening for mouse move again.
            // This is done to conserve bandwidth since a server call on EVERY mouse moves can be extremely costly.
            initEvents: function () {
                var timerTick = function(data) {
                    if (timer) {
                        timer = undefined;

                        this.getWrappedControl().element.trigger('gaia:moving');

                        if (this.options.hasEvent) {
                            Gaia.Control.callAspectMethod.call(this.getWrappedControl(), 'MouseMoveMethod',
                                [data.x, data.y, data.offsetLeft, data.offsetTop, data.controlKeys]);
                        }
                    }
                };

                var me = this;
                this.addEvent('mousemove', function(evt) {
                    timer = timer || setTimeout($.proxy(timerTick, me, me.getMouseEventData(evt)), me.options.interval);
                });
            },

            destroy: function() {
                if (timer) {
                    clearTimeout(timer);
                    timer = undefined;
                }

                Gaia.AspectMouseMove.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
