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
   AspectDraggable, adds up Drag'n'Drop for any control you choose
   --------------------------------------------------------------------------- */
(function (window, $, Class, undefined) {
    Gaia.AspectDraggable = Class(Gaia.Aspect, function () {

        var draggableEventNamespace = ".gaia_ajax_aspectdraggable";

        var destroyDraggable = function () {
            $("#" + this.parentId).off(draggableEventNamespace).draggable("destroy");
        };

        var onDragStart = function (evt, ui) {
            var control = this.getWrappedControl();

            if (this.options.makeGhost) {
                ui.helper.offset(control.element.offset());
            }
            
            ui.helper.addClass(this.options.dragClass);
            control.element.trigger("gaia:startdrag");

            if (this.options.hasBeginDragEvent) {
                Gaia.Control.callAspectMethod.call(control, "BeginDragMethod");
            }
        };

        var onDragStop = function (evt, ui) {
            var control = this.getWrappedControl();
            var element = control.element;
            
            if (this.options.makeGhost && !this.options.revert) {
                element.offset(ui.offset);
            }

            element.trigger("gaia:enddrag").removeClass(this.options.dragClass);

            if (this.options.hasDropEvent) {
                var position;
                var methodName;

                if (this.options.rel) {
                    var offsetParent = element.offsetParent();
                    var dimensions = {
                        width: offsetParent.width() - element.width(),
                        height: offsetParent.height() - element.height()
                    };

                    methodName = 'DroppedMethodRel';
                    position = {
                        top: dimensions.height === 0 ? 0 : (ui.position.top / dimensions.height * 100),
                        left: dimensions.width === 0 ? 0 : (ui.position.left / dimensions.width * 100)
                    };
                } else {
                    methodName = 'DroppedMethod';
                    position = ui.position;
                }

                Gaia.Control.callAspectMethod.call(control, methodName, [Math.ceil(position.left), Math.ceil(position.top)]);
            }
        };

        var onRevert = function (draggable) {
            var options = this.options;
            var control = this.getWrappedControl();
            if (!control.options.effects || !control.options.effects.gaiadragreverting) {
                control.options.effects = $.extend({
                    gaiadragreverting: function (evt) {
                        if (options.makeGhost) return;

                        this.element.css({
                            left: function(index, value) {
                                return parseFloat(value || 0) + evt.memo.x;
                            },

                            top: function(index, value) {
                                return parseFloat(value || 0) + evt.memo.y;
                            }
                        });
                    }
                }, control.options.effects);

                control._addEffectEvent("gaiadragreverting", control.options.effects.gaiadragreverting);
            }

            var currentPosition = draggable.position;
            var originalPosition = draggable.originalPosition;
            
            control.element.trigger(({
                type: "gaia:dragreverting",
                memo: {
                    x: originalPosition.left - currentPosition.left,
                    y: originalPosition.top - currentPosition.top,
                    duration: parseInt(draggable.options.revertDuration, 10) || 500
                }
            }));
        };
        
        return {            
            
            constructor: function (parentId, options) {
                var p = options.p;
                Gaia.AspectDraggable.$super.call(this, parentId, $.extend({
                    hasDropEvent: (p & 1) === 1,
                    revert: (p & 2) === 2,
                    zIndex: 5000,
                    rect: false,
                    silent: (p & 64) === 64,
                    opacity: undefined,
                    hitEffect: (p & 32) === 32,
                    handle: false,
                    dragClass: null,
                    useDocumentBody: (p & 16) === 16,
                    makeGhost: (p & 8) === 8,
                    deepCopy: (p & 4) === 4,
                    sp: false,
                    rel: false,
                    rectType: false,
                    hasBeginDragEvent: (p & 128) === 128
                }, options));

                this.createDragger();
            },

            createDragger: function () {
                var helper;
                var dragNode = $("#" + this.parentId);
                
                if (this.options.makeGhost) {
                    if (!this.options.deepCopy) {
                        helper = function () {
                            return dragNode.clone().removeAttr("id").empty();
                        };
                    } else {
                        helper = "clone";
                    }
                } else {
                    helper = "original";
                }
                
                var containment = false;
                var rect = this.options.rect;
                if (rect === "parent") {
                    containment = "parent";
                } else if (rect) {
                    var offset = dragNode.offset();
                    containment = [offset.left + rect.left, offset.top + rect.top,
                        offset.left + rect.right, offset.top + rect.bottom];
                }

                var handle = this.options.handle && $("#" + this.options.handle);

                dragNode.draggable({
                    helper: helper,
                    handle: handle,
                    containment: containment,
                    zIndex: this.options.zIndex,
                    opacity: this.options.opacity,
                    hitEffect: !!this.options.hitEffect,
                    addClasses: !!this.options.dragClass,
                    appendTo: this.options.useDocumentBody ? "body" : "parent",

                    start: $.proxy(onDragStart, this),
                    stop: $.proxy(onDragStop, this)
                });
                
                var snap = this.options.sp;
                if ($.isFunction(snap)) {
                    dragNode.on("drag" + draggableEventNamespace, function (event, ui) {
                        var result = snap(ui.position.left, ui.position.top, dragNode);
                        ui.position = { left: result[0], top: result[1] };
                    });
                } else if ($.isArray(snap)) {
                    dragNode.draggable("option", "grid", snap);
                } else if (snap) {
                    dragNode.draggable("option", "snap", snap);
                }
                
                if (!this.options.hitEffect && !!this.options.revert) {
                    dragNode.draggable("option", "revert", $.proxy(onRevert, this, dragNode.data("ui-draggable")));
                }
            },

            forceAnUpdate: function() {
                destroyDraggable.call(this);
            },

            reInit: function() {
                this.createDragger();
            },

            onWrapperControlRenaming: function() {
                this.destroy();
            },

            setWrappedControlID: function(value) {
                this.constructor.call(this, value, this.options);
            },

            destroy: function() {
                destroyDraggable.call(this);
                Gaia.AspectDraggable.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
