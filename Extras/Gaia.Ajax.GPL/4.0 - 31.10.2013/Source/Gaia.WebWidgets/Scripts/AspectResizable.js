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

    /* ---------------------------------------------------------------------------
       AspectResizable, adds up server-side Resized event support for Controls
       --------------------------------------------------------------------------- */
    Gaia.AspectResizable = Class(Gaia.Aspect, function () {

        var resizeModes = Class({
            $statics: {
                None: 0,
                Left: 2,
                Right: 4,
                Top: 8,
                Bottom: 16,
                All: 30 /* bitwise or of above values */,

                isSet: function (resizeMode, flag) {
                    return ((resizeMode & flag) === flag);
                }
            }
        });

        var destroyResizable = function () {
            $("#" + this.parentId).resizable("destroy");
        };

        var onSnapResize = function(callback, callbackOptions) {
            var a = this.axis;
            var cs = this.size;
            var cp = this.position;
            var os = this.originalSize;
            var op = this.originalPosition;

            var diff = {
                x: (cp.left - op.left) || (cs.width - os.width),
                y: (cp.top - op.top) || (cs.height - os.height)
            };

            var result = callback.call(window, diff.x, diff.y, callbackOptions);

            var dx = result[0];
            var dy = result[1];

            var resizeMode = callbackOptions.resizeMode;
            if (resizeMode.top) {
                this.size.height = os.height - dy;
                this.position.top = op.top + dy;
            }
            
            if (resizeMode.left) {
                this.size.width = os.width - dx;
                this.position.left = op.left + dx;
            }
            
            if (resizeMode.right) {
                this.size.width = os.width + dx;
            }
            
            if (resizeMode.bottom) {
                this.size.height = os.height + dy;
            }
        };

        var generateHandles = function(handle, mode) {
            if (!handle) {
                var handles = [];

                if (mode.left) {
                    handles.push("w");

                    if (mode.top) {
                        handles.push("nw");
                    }

                    if (mode.bottom) {
                        handles.push("sw");
                    }
                }

                if (mode.right) {
                    handles.push("e");

                    if (mode.top) {
                        handles.push("ne");
                    }

                    if (mode.bottom) {
                        handles.push("se");
                    }
                }

                if (mode.top) {
                    handles.push("n");
                }

                if (mode.bottom) {
                    handles.push("s");
                }

                return handles.join(",");
            } else {
                return { se: $("#" + handle).addClass("ui-resizable-handle ui-resizable-se") };
            }
        };

        var createResizable = function() {
            var me = this;
            var element = $("#" + this.parentId);
            var aspectOptions = this.options;

            var sibling = aspectOptions.sibling;
            sibling = sibling ? "#" + sibling : false;

            var resizeMode = aspectOptions.resizeMode;
            var mode = {
                left: resizeModes.isSet(resizeMode, resizeModes.Left),
                right: resizeModes.isSet(resizeMode, resizeModes.Right),
                top: resizeModes.isSet(resizeMode, resizeModes.Top),
                bottom: resizeModes.isSet(resizeMode, resizeModes.Bottom)
            };

            element.resizable({
                containment: false,
                alsoResize: sibling,
                handles: generateHandles.call(this, aspectOptions.handle, mode),

                minWidth: aspectOptions.minWidth,
                maxWidth: aspectOptions.maxWidth,
                minHeight: aspectOptions.minHeight,
                maxHeight: aspectOptions.maxHeight,

                start: function(evt, ui) {
                    me.getWrappedControl().element.trigger({
                        type: "gaia:resizeStart",
                        memo: ui
                    });
                },

                stop: function(evt, ui) {
                    var control = me.getWrappedControl();
                    control.element.trigger({
                        type: "gaia:resized",
                        memo: ui
                    });

                    me.onResized(evt, ui);
                }
            });

            var resizable = element.data("ui-resizable");
            var containment = this.options.rect;
            if ($.isArray(containment) && containment.length === 4) {
                var styles = element.css(["left", "top"]);
                var top = (parseInt(styles.top, 10) || 0);
                var left = (parseInt(styles.left, 10) || 0);
                var width = element.width();

                if (containment[0] === 0 && containment[2] === 0) {
                    containment[2] = width;
                }
                
                if (containment[1] === 0 && containment[3] === 0) {
                    containment[3] = height;
                }
                
                var bounds = aspectOptions.bounds = {
                    left: left + containment[0],
                    top: top + containment[1],
                    right: left + containment[2] - (element.outerWidth() - width),
                    bottom: top + containment[3] - (element.outerHeight() - element.height())
                };
                
                var containmentWrapped = resizable._propagate;
                resizable._propagate = function(n, event) {
                    containmentWrapped.call(resizable, n, event);

                    if (n === "resize") {
                        if (resizable.position.left < bounds.left) {
                            resizable.size.width -= bounds.left - resizable.position.left;
                            resizable.position.left = bounds.left;
                        }

                        if (resizable.position.top < bounds.top) {
                            resizable.size.height -= bounds.top - resizable.position.top;
                            resizable.position.top = bounds.top;
                        }

                        if (resizable.position.left + resizable.size.width > bounds.right) {
                            resizable.size.width = bounds.right - resizable.position.left;
                        }

                        if (resizable.position.top + resizable.size.height > bounds.bottom) {
                            resizable.size.height = bounds.bottom - resizable.position.top;
                        }
                    }
                };
            }

            if ($.isFunction(aspectOptions.sp)) {
                var snapWrapped = resizable._propagate;
                resizable._propagate = function(n, event) {
                    if (n === "resize") {
                        var axis = resizable.axis;
                        onSnapResize.call(resizable, aspectOptions.sp, {
                            element: element,
                            resizeMode: {
                                top: /^n/.test(axis),
                                left: /w$/.test(axis),
                                right: /e$/.test(axis),
                                bottom: /^s/.test(axis)
                            }
                        });
                    }

                    snapWrapped.call(resizable, n, event);
                };
            } else if ($.isArray(aspectOptions.sp)) {
                element.resizable("option", "grid", aspectOptions.sp);
            }
        };
        
        return {
            constructor: function(parentId, options) {

                Gaia.AspectResizable.$super.call(this, parentId, $.extend({
                    hasEvent: true,
                    minWidth: 0,
                    minHeight: 0,
                    maxWidth: null,
                    maxHeight: null,
                    resizeMode: resizeModes.All,
                    sibling: null,
                    handle: false,
                    rect: false,
                    sp: false
                }, options));

                createResizable.call(this);
            },
            
            onResized: function(evt, ui) {
                if (this.options.hasEvent) {
                    var control = this.getWrappedControl();
                    var element = control.element;
                    Gaia.Control.callAspectMethod.call(control, 'ResizedMethod',
                        [
                            ui.position.left,
                            ui.position.top,
                            element.outerWidth(),
                            element.outerHeight()
                        ],
                        null, element.attr("id")
                    );
                }
            },
            
            forceAnUpdate: function () {
                destroyResizable.call(this);
            },

            reInit: function () {
                createResizable.call(this);
            },

            onWrapperControlRenaming: function() {
                this.destroy();
            },

            setWrappedControlID: function(value) {
                this.constructor.call(this, value, this.options);
            },

            destroy: function() {
                destroyResizable.call(this);
                Gaia.AspectResizable.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
