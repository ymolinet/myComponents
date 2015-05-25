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
    Gaia.Extensions = Gaia.Extensions || {};

    var timer;
    var onWindowResize = function(callback) {
        clearTimeout(timer);
        timer = setTimeout(callback, 100);
    };

    var topMostWindows = [];
    $(window).on("resize.gaia-ajax-extwindow-window-events", $.proxy(onWindowResize, window, function () {
        $.each(topMostWindows, function(idx, id) {
            var wnd = Gaia.Control.get(id);
            if (wnd && wnd.options.maximized) {
                wnd.setMaximized(true);
            }
        });
    }));

    var topMostWindow = null;
    
    Gaia.Extensions.Window = Class(Gaia.Panel, function () {

        var windowStates = {};
        var windowEventNamespace = ".gaia_extensions_window_events";

        var getMiddle = function() {
            return $("#" + this.element.attr('id') + '_middle');
        };

        var syncContentDimensions = function() {
            this.setWidth(this.element.width());
            this.setHeight(this.element.height());
        };

        var storeState = function() {
            var contentState = this.getContent().css(['width', 'height']);
            var state = this.element.css(['top', 'left', 'width', 'height']);

            windowStates[this.element.attr("id")] = {
                top: state.top,
                left: state.left,
                width: state.width,
                height: state.height,
                widthInner: contentState.width,
                heightInner: contentState.height
            };
        };

        var getOuterContainer = function () {
            return Gaia.Control.get(this.options.outerContainerId);
        };

        var updateChildWindows = function() {
            var maximizable = [];
            
            this.eachGaiaChild(function (control) {
                if (control.options.maximized && control.setMaximized) {
                    maximizable.push(control);
                }
            });

            var comparer = function(x, y) { return x.element.attr('id').length - y.element.attr('id').length; };
            $.each(maximizable.sort(comparer), function (idx, wnd) { wnd.setMaximized(1, true); });
        };
        
        return {
            constructor: function(element, options) {

                this.initializeWindowEffects(options);
                Gaia.Extensions.Window.$super.call(this, element, options);

                this.initializeWindowDimensions();

                var isWindowNested = !isNaN(options.nested);
                if (isWindowNested) {
                    this.options.outerContainerId = element.substr(0, options.nested);
                } else {
                    topMostWindows.push(element);
                }

                var properties = options.p;
                if (properties !== undefined && !isNaN(properties)) {
                    if ((properties & 1) === 1) {
                        this.setMinimized(1);
                    }   

                    if ((properties & 2) === 2) {
                        this.options.maximized = true;
                    }

                    if ((properties & 4) === 4) {
                        this.bringWindowToFront();
                    }
                }

                var me = this;
                this.setStyle({ visibility: "visible", display: "none" });

                this.element
                    .on('click' + windowEventNamespace, function (evt) {
                        
                        if (evt.target.id !== element) {
                            var hasNestedWindow = false;
                            $(evt.target).parentsUntil(me.element).each(function() {
                                var control = Gaia.Control.get(this.id);
                                if (control && control.initializeWindowDimensions) {
                                    hasNestedWindow = true;
                                    return false;
                                }
                            });
                            
                            if (hasNestedWindow) {
                                return;
                            }
                        }

                        me.bringWindowToFront();
                    })
                    .trigger("gaia:appearing");
            },

            initializeWindowEffects: function(options) {
                // Add base effects to the Window
                options.effects = $.extend({
                    // Default minimize implementation
                    gaiaminimizing: function () {
                        var defaultMinimizeWidth = "300px";
                        
                        var content = this.getContent();
                        if (content.is(":visible")) {
                            content.toggle();
                            this.setStyle({ width: defaultMinimizeWidth, height: "auto" });
                        }
                    },

                    // Default restoring implementation after maximize
                    gaiarestoring: function(evt) {
                        this.defaultRestore(evt);
                    },

                    // Default restoring implementation after minimize
                    gaiarestoreafterminimize: function(evt) {
                        this.defaultRestore(evt);
                    },

                    gaiaclosing: function(evt) {
                        evt.memo.afterFinish.call(this);
                    },

                    gaiaappearing: function (e) {
                        var duration = parseInt(this.options.animateAppearance, 10) || undefined;
                        $(e.target).show(duration);
                    }
                }, options.effects);
            },

            initializeWindowDimensions: function () {
                var id = this.element.attr('id');

                // Getting width and height of borders (needed to resize the widget)
                this.options.widthOfBorders = this.element.width() - this.getContent().width();
                this.options.heightOfBorders = $("#" + id + '_header').height() + $("#" + id + '_bottom').height();

                var inlineHeight = parseInt(this.element.get(0).style.height, 10);
                if (!isNaN(inlineHeight)) {
                    this.setHeight(inlineHeight);
                }
            },

            setBars: function(value) {
                // also update the overflow value on the content wrapper
                var overflowValue = value === 'None' ? 'visible' : 'hidden';
                getMiddle.call(this).parent().css({ 'overflow': overflowValue });

                return this.setScrollBars(value, this.getContent());
            },

            setCCH: function (value) {
                $("#" + this.element.attr("id") + "_h").attr('class', value);
                return this;
            },

            setSkinCssClass: function(current, value) {
                this.options.className = value;
                return Gaia.Extensions.Window.$superp.setSkinCssClass.call(this, current, value);
            },

            setMinimized: function(value) {
                this.options.minimized = !!value;

                if (this.options.minimized) {
                    if (this.options.maximized) {
                        this.setMaximized(0);
                    }

                    storeState.call(this);
                    this.element.trigger("gaia:minimizing");
                } else {
                    this.restore("gaia:restoreafterminimize");
                }

                return this;
            },

            setMaximized: function (value, skipChildren) {
                this.options.maximized = !!value;
                var maximizeCssClass = this.options.className + "-window-maximized";
                
                if (this.options.maximized) {
                    if (this.options.minimized) {
                        this.setMinimized(0);
                    }
                    
                    // Storing "old" position and size for restoring later
                    storeState.call(this);
                    
                    // Adding up the "maximized" class name
                    this.element.addClass(maximizeCssClass);

                    // Sizing according to the "viewport" size      
                    var container = getOuterContainer.call(this);
                    if (!container) {
                        this.element.offset({ left: $(document).scrollLeft(), top: $(document).scrollTop() });
                        
                        this.setWidth($(window).width());
                        this.setHeight($(window).height());
                    } else {
                        container = container.getBody();
                        this.element.offset(container.offset());
                        
                        this.setWidth(container.width());
                        this.setHeight(container.height());
                    }
                } else {
                    this.element.removeClass(maximizeCssClass);
                    this.restore('gaia:restoring');
                }

                if (!skipChildren) {
                    updateChildWindows.call(this);
                }

                return this;
            },

            restore: function (evtName) {
                var id = this.element.attr("id");
                var windowState = windowStates[id];
                windowState[id] = undefined;
                delete windowStates[id];
                
                this.element.trigger({
                    type: evtName,
                    memo: {
                        w: windowState.width,
                        h: windowState.height,
                        t: windowState.top,
                        l: windowState.left,
                        widthInner: windowState.widthInner,
                        heightInner: windowState.heightInner,
                        afterFinish: $.proxy(syncContentDimensions, this)
                    }
                });
            },

            defaultRestore: function (evt) {
                var data = evt.memo;
                this.element.css({
                    top: data.t,
                    left: data.l,
                    width: data.w,
                    height: data.h
                });

                this.getContent().show().css({
                    width: data.widthInner,
                    height: data.heightInner
                });
            },

            setHeight: function (value) {
                this.element.height(value);

                // set content height to the expected value
                var content = this.getContent();
                var expectedHeight = this.element.height() - this.options.heightOfBorders;
                content.height(expectedHeight);

                // adjust content height to exact value
                var difference = getMiddle.call(this).height() - expectedHeight;
                if (difference > 0) {
                    content.height(expectedHeight - difference);
                }

                return this;
            },

            setWidth: function(value) {
                this.element.width(value);

                // set content width to the expected value
                var content = this.getContent();
                var expectedWidth = this.element.width() - this.options.widthOfBorders;
                content.width(expectedWidth);

                // adjust content width to exact value
                var contentWidth = content.width();
                var difference = contentWidth - expectedWidth;
                if (difference > 0) {
                    content.width(value - this.options.widthOfBorders - difference);
                }

                return this;
            },

            reInit: function() {
                Gaia.Extensions.Window.$superp.reInit.call(this);
                this.initializeWindowDimensions();
            },

            // SetVisible is overriden here to inject a custom Closing effect if it has been defined on the Window
            setVisible: function (value) {
                if (!value) {
                    this.element.trigger({
                        type: "gaia:closing",
                        memo: {
                            afterFinish: $.proxy(Gaia.Extensions.Window.$superp.setVisible, this, value)
                        }
                    });
                } else {
                    Gaia.Extensions.Window.$superp.setVisible.call(this, value);
                }
            },

            getContent: function() {
                return $("#" + this.element.attr('id') + '_content');
            },

            center: function (skipChildren) {
                var viewportSize = {
                    width: $(window).width(),
                    height: $(window).height()
                };

                var windowSize = {
                    width: this.element.width(),
                    height: this.element.height()
                };

                var left = Math.round(Math.max((viewportSize.width / 2) - (windowSize.width / 2), 0));
                var top = Math.round(Math.max((viewportSize.height / 2) - (windowSize.height / 2), 0));

                // try assumed values
                this.element.offset({
                    top: top + $(document).scrollTop(),
                    left: left + $(document).scrollLeft()
                });
            },

            bringWindowToFront: function() {
                var id = this.element.attr('id');
                
                if (topMostWindow !== id) {
                    topMostWindow = id;
                    this.bringToFront();
                }
            },

            destroy: function () {
                this.element.off(windowEventNamespace);

                var id = this.element.attr("id");
                topMostWindows = $.grep(topMostWindows, function (wnd) {
                    return wnd !== id;
                });

                if (topMostWindow === id) {
                    topMostWindow = undefined;
                }

                Gaia.Extensions.Window.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
