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
   Common class for all WebControls, basically wraps the WebControl class
   --------------------------------------------------------------------------- */
(function (window, $, Class, undefined) {

    var support = {};
    (function () {
        var div = $("<div />");
        
        try {
            div.css({ color: "rgba(100, 100, 100, 0.5)" });
            support.rgba = !!div.css("color");
        } catch(e) {
            support.rgba = false;
        } finally {
            div = null;
        }
    })();

    Gaia.WebControl = Class(Gaia.Control, function() {

        // Expects a color value in RRGGBB[AA] format, and returns color and alpha parts for opacity.
        var parseColor = function(value) {
            return { color: value.substring(0, 6), alpha: (parseInt(value.substring(6), 16) / 255) };
        };

        var setColor = function(name, value, setOpacity) {
            var style = {};
            if (value) {
                var rgba = parseColor(value);
                if (support.rgba) {
                    var components = $.map(rgba.color.match(/.{2}/g),
                        function(color) { return parseInt(color, 16); });

                    if (!isNaN(rgba.alpha)) {
                        components.push(rgba.alpha);
                    }

                    style[name] = "rgba(" + components.join(",") + ")";
                } else {
                    if (setOpacity && $.support.opacity && !isNaN(rgba.alpha)) {
                        style.opacity = rgba.alpha;
                    }
                    
                    style[name] = "#" + rgba.color;
                }
            } else {
                style[name] = "";
            }

            return this.setStyle(style);
        };

        var getStackingContextZIndex = function(element) {
            // check if element creates stacking context, the required conditions are
            // see https://developer.mozilla.org/en-US/docs/CSS/Understanding_z-index/The_stacking_context
            var styles = element.css(['position', 'opacity', 'zIndex']);
            var zIndex = parseInt(styles.zIndex, 10); // may result in NaN

            if (styles.opacity < 1) {
                return zIndex || 0;
            }

            // we ignore case of explicit z-index:0 to differentiate between IE 'auto' and '0' values
            if (!isNaN(zIndex) && zIndex !== 0) {
                var position = styles.position;
                if (position === "absolute" || position === "relative" || position === "fixed") {
                    return zIndex;
                }
            }

            return NaN;
        };

        return {
            constructor: function(element, options) {
                Gaia.WebControl.$super.call(this, element, options);
            },

            // TODO: Check up how to get to work on FF and Opera etc (if possible)
            setAccessKey: function(value) {
                return this.setAttribute('accessKey', value);
            },

            setAttribute: function(name, value) {
                if (value) {
                    this.element.attr(name, value);
                } else {
                    this.element.removeAttr(name);
                }
                return this;
            },

            removeAttribute: function(name) {
                this.element.removeAttr(name);
                return this;
            },

            setStyle: function(styles) {
                this.element.css(styles);
                return this;
            },

            setBackColor: function(value) {
                return setColor.call(this, 'background-color', value, true);
            },

            setBorderColor: function(value) {
                return setColor.call(this, 'border-color', value, false);
            },

            setBorderStyle: function(value) {
                return this.setStyle({ 'border-style': value });
            },

            setBorderWidth: function(value) {
                return this.setStyle({ 'border-width': value });
            },

            // Will replace all current classes
            setCssClass: function(value) {
                return this.setAttribute('class', value);
            },

            setSkinCssClass: function(current, value) {
                var currentClassMatchRegex = new RegExp('^' + current);

                this.eachNonGaiaChild(function(node) {
                    node.attr('class', function(i, val) {
                        return $.map(val.split(/\s+/), function(className) {
                            return className.replace(currentClassMatchRegex, value);
                        }).join(' ');
                    });
                });

                return this;
            },

            // Enables or disables the element (true/false)
            setEnabled: function(value) {
                // we don't know if the element supports disabled property,
                // so we apply both .attr and .prop
                if (value) {
                    this.element.removeAttr("disabled");
                } else {
                    this.element.attr("disabled", "disabled");
                }
                this.element.prop('disabled', !value);
                return this;
            },

            isEnabled: function() {
                return !this.element.prop('disabled');
            },

            storeEnabled: function() {
                this.enabledState = this.isEnabled();
            },

            restoreEnabled: function () {
                if (this.enabledState !== undefined) {
                    this.setEnabled(this.enabledState);
                }

                this.enabledState = undefined;
            },

            // Sets the font of the element to either bold(true) or normal(false)
            setFontBold: function(value) {
                return this.setStyle({ 'font-weight': (value ? 'bold' : 'normal') });
            },

            // Sets the font of the element to either italic(true) or normal(false)
            setFontItalic: function(value) {
                return this.setStyle({ 'font-style': (value ? 'italic' : 'normal') });
            },

            setFontNames: function(value) {
                return this.setStyle({ 'font-family': value });
            },

            // Sets the font of the element to either overline style(true) or normal(false)
            setFontOverline: function(value) {
                return this.setStyle({ 'text-decoration': (value ? 'overline' : '') });
            },

            setFontSize: function(value) {
                return this.setStyle({ 'font-size': value });
            },

            // Sets the font of the element to either strikeout(true) or normal(false)
            setFontStrikeout: function(value) {
                return this.setStyle({ 'text-decoration': (value ? 'line-through' : '') });
            },

            // Sets the font of the element to either underline(true) or normal(false)
            setFontUnderline: function(value) {
                return this.setStyle({ 'text-decoration': (value ? 'underline' : '') });
            },

            setForeColor: function(value) {
                return setColor.call(this, 'color', value, false);
            },

            // Sets the height of the element
            setHeight: function(value) {
                return this.setStyle({ height: value });
            },

            // Sets the tooltip of the element (title element)
            setToolTip: function(value) {
                if (value) {
                    this.element.prop('title', value);
                } else {
                    this.element.removeProp('title');
                }

                return this;
            },

            // Sets the tabindex of the element
            setTabIndex: function(value) {
                if (value) {
                    this.element.prop('tabIndex', value);
                } else {
                    this.element.removeProp('tabIndex');
                }

                return this;
            },

            // Sets the width of the element
            setWidth: function(value) {
                return this.setStyle({ width: value });
            },

            initializeDefaultValidation: function() {
                this.options = $.extend({ validate: 1, validationGroup: '' }, this.options);
            },

            setVal: function(value) {
                this.options.validate = value;
                return this;
            },

            setValGrp: function(value) {
                this.options.validationGroup = value;
                return this;
            },

            bringToFront: function() {
                Gaia.WebControl.bringElementToFront(this.element);
                return this;
            },

            $statics: {
                VerAligns: ['top', 'middle', 'bottom'],
                HorAligns: ['left', 'center', 'right', 'justify'],

                bringElementToFront: function(element) {
                    var threshold;
                    var hint = Gaia.Control.ZIndexThreshold;
                    if (!isNaN(hint) && hint !== null) {
                        threshold = hint;
                    } else {
                        threshold = Number.MAX_VALUE;
                    }

                    var target = element;
                    var body = document.body;
                    element
                        .parentsUntil(body.parentNode)
                        .filter(function() {
                            return body === this || !isNaN(getStackingContextZIndex($(this)));
                        })
                        .each(function() {
                            var maxZIndex = 0;
                            var stackingContext = $(this);
                            var children = stackingContext.children();

                            var onEachChild = function() {
                                var child = $(this);
                                var zIndex = getStackingContextZIndex(child);

                                if (isNaN(zIndex)) {
                                    return child.children().get();
                                } else if (zIndex <= threshold) {
                                    maxZIndex = Math.max(maxZIndex, zIndex);
                                }

                                return null;
                            };

                            while (children.length > 0) {
                                children = children
                                    .not(target)
                                    .not('script')
                                    .map(onEachChild);
                            }

                            // bring target to front in its stacking context
                            var suggested = maxZIndex + 1;
                            var current = parseInt(target.css('z-index'), 10);
                            if (isNaN(current) || suggested > current) {
                                target.css('z-index', suggested);
                            }

                            target = stackingContext;
                        });
                }
            }
        };
    });

    /*
    * Mixin class that allows you to specify a Label for an Input control and then 
    * change it's alignment (Left || Right). 
    */
    Gaia.TextAlignMixin = Class(function() {

        var isLeftAlign = function() {
            return this.options.textalign === 'Left';
        };

        var createLabel = function(input, text) {
            var label = $('<label />').attr({ 'for': input.attr('id') }).html(text);

            if (isLeftAlign.call(this)) {
                input.before(label);
            } else {
                input.after(label);
            }
        };

        var getLabelForInput = function(input) {
            return isLeftAlign.call(this) ? input.prev() : input.next();
        };

        return {
            _setTextAlign: function(inputs, value) {
                var me = this;

                var collectedLabels = inputs.map(function(idx, val) {
                    return getLabelForInput.call(me, $(val)).text();
                });

                inputs.each(function(idx, val) {
                    getLabelForInput.call(me, $(val)).remove();
                });

                me.options.textalign = value;

                inputs.each(function(idx, input) {
                    me.setLabelText(input, collectedLabels[idx]);
                });

                return me;
            },

            setLabelText: function(input, value) {
                var label = getLabelForInput.call(this, input);
                var labelFound = label.length === 1;

                if (value) {
                    if (labelFound) {
                        label.html(value);
                    } else {
                        createLabel.call(this, input, value);
                    }
                } else if (labelFound) {
                    label.remove();
                }
            }
        };
    });
})(window, jQuery, jsface.Class);