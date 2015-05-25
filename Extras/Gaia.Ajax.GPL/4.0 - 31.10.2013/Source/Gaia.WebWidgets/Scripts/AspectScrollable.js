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
   AspectScrollable, adds up server-side Scroll Event support for Controls
   --------------------------------------------------------------------------- */
(function(window, $, Class, undefined) {
    Gaia.AspectScrollable = Class(Gaia.Aspect, function () {
        var eventNamespace = ".gaia-ajax-aspectscrollable-events";
        
        // imitate enum
        var scrollModes = {
            none: 0,
            vertical: 2,
            horizontal: 4,
            all: 6 /* bitwise or of above values */
        };

        var checkScrollMode = function(scrollMode, checkMode) {
            return ((scrollMode & checkMode) == checkMode);
        };
        
        return {
            
            constructor: function (parentId, options) {

                Gaia.AspectScrollable.$super.call(this, parentId, $.extend({
                    scrollMode: 6, /* all */
                    onlyRaiseAtEdges: false
                }, options));

                var element = $("#" + parentId);

                if (checkScrollMode(options.scrollMode, scrollModes.vertical)) {
                    element.css("overflowY", "auto");
                }

                if (checkScrollMode(options.scrollMode, scrollModes.horizontal)) {
                    element.css("overflowX", "auto");
                }

                var me = this;
                var tolerance = 1;
                var prevPos = { top: 0, left: 0 };
                
                element.on("scroll" + eventNamespace, function(evt) {
                    var target = evt.target;

                    var size = {
                        width: target.clientWidth,
                        height: target.clientHeight
                    };

                    var scroll = {
                        top: $(target).scrollTop(),
                        left: $(target).scrollLeft(),
                        width: target.scrollWidth,
                        height: target.scrollHeight
                    };

                    var pos = {
                        top: scroll.top + size.height,
                        left: scroll.left + size.width,
                    };

                    var bars = {
                        vertical: prevPos.top !== pos.top,
                        horizontal: prevPos.left !== pos.left
                    };

                    prevPos = pos;

                    var atRightEdge = (pos.left + tolerance >= scroll.width);
                    var atBottomEdge = (pos.top + tolerance >= scroll.height);

                    var shouldCall = (!me.options.onlyRaiseAtEdges) || atRightEdge || atBottomEdge;

                    if (me.options.onlyRaiseAtEdges && atBottomEdge && !atRightEdge && bars["horizontal"]) {
                        shouldCall = false;
                    }

                    if (me.options.onlyRaiseAtEdges && atRightEdge && !atBottomEdge && bars["vertical"]) {
                        shouldCall = false;
                    }

                    if (shouldCall) {
                        Gaia.Control.callAspectMethod.call(me.getWrappedControl(),
                            'ScrollMethod',
                            [parseInt(scroll.left, 10) || 0,
                                parseInt(scroll.top, 10) || 0,
                                parseInt(scroll.width, 10) || 0,
                                parseInt(scroll.height, 10) || 0]);
                    }
                });
            },

            destroy: function() {
                ("#" + this.parentId).off(eventNamespace);
                Gaia.AspectScrollable.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
