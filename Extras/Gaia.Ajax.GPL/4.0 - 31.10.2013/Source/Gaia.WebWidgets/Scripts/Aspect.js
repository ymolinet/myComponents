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
   Base class for all Aspects
   All aspects should inherit from this class.
   --------------------------------------------------------------------------- */
(function(window, $, Class, undefined) {
    Gaia.Aspect = Class({
        constructor: function(parentId, options) {
            this.parentId = parentId;
            this.options = options || {};
        },

        setWrappedControlID: function(value) {
            this._parent = null;
            this.parentId = value;
        },

        // Returns the control the aspect is attached to
        getWrappedControl: function() {
            return this._parent || (this._parent = Gaia.Control.get(this.parentId));
        },

        // Called when forceAnUpdate is called on the owning widget
        forceAnUpdate: $.noop,

        // Called when widget is "re-initialized" after a forceAnUpdate
        // Pairs together with the Aspect.forceAnUpdate method
        reInit: $.noop,

        // Override in derived aspects (must override)
        destroy: $.noop
    });

    /* ---------------------------------------------------------------------------
       AspectWithEvents, contains event lists with automatic initialization 
       --------------------------------------------------------------------------- */
    Gaia.AspectWithEvents = Class(Gaia.Aspect, function () {
        var eventNamespace = ".aspect_events";
        
        return {
            constructor: function(parentId, options) {
                Gaia.AspectWithEvents.$super.call(this, parentId, options);
                this.initEvents();
            },

            // adding the event with handler
            addEvent: function(evtName, evtHandler) {
                var el = $("#" + this.parentId);
                var evtFullName = evtName + eventNamespace;
                el.off(evtFullName).on(evtFullName, evtHandler);
            },

            // cleaning up event handlers
            clearEvents: function() {
                var el = $("#" + this.parentId);
                el.off(eventNamespace);
            },

            // retrieve mouse event data from event. 
            getMouseEventData: function(event) {
                var offset = this.getWrappedControl().element.offset();
                var keys = (event.shiftKey ? 1 : 0) | (event.ctrlKey ? 2 : 0) | (event.altKey ? 4 : 0);

                return {
                    x: event.pageX,
                    y: event.pageY,
                    controlKeys: keys,
                    offsetTop: Math.ceil(offset.top),
                    offsetLeft: Math.ceil(offset.left)
                };
            },


            // override this function to initialize your own events
            initEvents: $.noop,

            forceAnUpdate: function() {
                this.clearEvents();
            },

            reInit: function() {
                this.initEvents();
            },

            destroy: function() {
                this.clearEvents();
            }
        };
    });
})(window, jQuery, jsface.Class);
