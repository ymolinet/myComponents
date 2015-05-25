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
    Gaia.AspectHoverable = Class(Gaia.AspectWithEvents, function() {
        return {
            constructor: function(parentId, options) {
                Gaia.AspectHoverable.$super.call(this, parentId, options);
            },

            initEvents: function () {
                var me = this;
                
                var mouseEnter = function (evt) {
                    var control = me.getWrappedControl();
                    control.element.trigger("gaia:mouseover");
                    
                    if (me.options.mouseOver) {
                        var data = me.getMouseEventData(evt);
                        Gaia.Control.callAspectMethod.call(control, 'MouseOverMethod',
                            [data.x, data.y, data.offsetLeft, data.offsetTop, data.controlKeys]);
                    }
                };

                var mouseLeave = function () {
                    var control = me.getWrappedControl();
                    control.element.trigger("gaia:mouseout");
                    
                    if (me.options.mouseOut) {
                        Gaia.Control.callAspectMethod.call(control, 'MouseOutMethod');
                    }
                };
                
                this.addEvent('mouseenter', mouseEnter);
                this.addEvent('mouseleave', mouseLeave);
            }
        };
    });
})(window, jQuery, jsface.Class);
