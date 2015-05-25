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
   AspectDroppable, adds up Drag'n'Drop for any control you choose
   --------------------------------------------------------------------------- */
(function(window, $, Class, undefined) {
    Gaia.AspectDroppable = Class(Gaia.Aspect, function () {

        var destroyDroppable = function() {
            $("#" + this.parentId).droppable("destroy");
        };

        var createDropper = function () {
            var me = this;

            $("#" + this.parentId).droppable({
                tolerance: "pointer",
                hoverClass: this.options.hoverclass,
                greedy: !!this.options.greedy,

                drop: function (evt, ui) {
                    var position = ui.position;
                    var draggable = ui.draggable;

                    if (draggable.draggable("option").hitEffect) {
                        draggable.trigger("gaia:dropped");
                    }
                    
                    Gaia.Control.callAspectMethod.call(me.getWrappedControl(), 'DraggableDropped',
                        [draggable.attr("id"), Math.ceil(position.left), Math.ceil(position.top)], null);
                }
            });
        };

        return {
            constructor: function(parentId, options) {
                Gaia.AspectDroppable.$super.call(this, parentId, options);
                createDropper.call(this);
            },

            forceAnUpdate: function () {
                destroyDroppable.call(this);
            },

            reInit: function () {
                createDropper.call(this);
            },

            onWrapperControlRenaming: function () {
                this.destroy();
            },

            setWrappedControlID: function (value) {
                this.constructor.call(this, value, this.options);
            },

            destroy: function () {
                destroyDroppable.call(this);
                Gaia.AspectDroppable.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
