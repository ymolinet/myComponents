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

    var visibleItemClassName = "gaia-ti-active";

    $(document).on("click", function(evt) {
        if (evt.data && evt.data.gaiaToolbarClicked) return;

        $("." + visibleItemClassName).each(function () {
            Gaia.Control.get(this.id).hide(true);
        });
    });

    Gaia.Extensions.ToolbarItem = Class(Gaia.Panel, function () {

        var eventNamespace = ".gaia-ajax-toolbar-item-events";

        var getDropper = function() {
            var dropper = getDropperAlways.call(this);
            return dropper.is(":empty") ? undefined : dropper;
        };

        var getDropperAlways = function() {
            return $("#" + this.element.attr("id") + '_children');
        };

        var showDropDown = function(dropper) {
            // hide other visible toolbaritems
            var visibleItemsSelector = "." + visibleItemClassName;
            $(visibleItemsSelector)
                .not(this.element.parents(visibleItemsSelector))
                .each(function() {
                    Gaia.Control.get(this.id).hide(true);
                });

            // clone position
            var offset = this.element.offset();
            var dropperOffset;
            if (this.options.isTop) {
                dropperOffset = {
                    left: offset.left,
                    top: offset.top + this.element.height()
                };
            } else {
                dropperOffset = {
                    left: offset.left + this.element.width(),
                    top: offset.top
                };
            }

            dropper.show().offset(dropperOffset).hide();

            this.element
                .addClass(visibleItemClassName)
                .trigger({
                    type: 'gaia:showchildren',
                    memo: { afterFinish: function() { dropper.css("height", "auto"); } }
                });
        };
            
        var hideDropDown = function (dropper) {
            this.element
                .removeClass(visibleItemClassName)
                .trigger({
                    type: 'gaia:hidechildren',
                    memo: { afterFinish: function() { dropper.css("height", "auto"); } }
                });
        };

        var removeHandlers = function() {
            this.element.off(eventNamespace);
            getDropperAlways.call(this).off(eventNamespace);
        };

        return {
            constructor: function(element, options) {

                options.effects = $.extend({
                    // Default hide implementation
                    gaiahidechildren: function (evt) {
                        getDropper.call(this).hide();
                        evt.memo.afterFinish.call(this);
                    },

                    // Default show implementation
                    gaiashowchildren: function (evt) {
                        getDropper.call(this).show();
                        evt.memo.afterFinish.call(this);
                    }
                }, options.effects);

                Gaia.Extensions.ToolbarItem.$super.call(this, element, $.extend({
                    isTop: false,
                    closeOnClick: true
                }, options));

                // create empty placeholder for DRIMR if required
                if (getDropper.call(this) === undefined) {
                    $("<ul/>").attr("id", this.element.attr("id") + "_children").empty().hide().appendTo(this.element);
                }
                
                this.setDropDownMethod(this.options.dropMethod);
            },

            setIconCssClass: function (value) {
                var icon = this.element.find(".span-for-image");
                if (value) {
                    icon.attr("class", value);
                } else {
                    icon.removeAttr("class");
                }

                return this;
            },

            setNested: function(value) {
                this.options.isTop = !!value;
                return this;
            },

            setCssClassRoot: function (value) {
                return this.setAttribute("class", value);
            },
            
            setCloseOnClick: function (value) {
                this.options.closeOnClick = !!value;
                return this;
            },

            setClose: function () {
                this.hide();
                return this;
            },
            
            setDropDownMethod: function (value) {
                var me = this;
                removeHandlers.call(this);
                
                switch (value) {
                    case 'Hover':
                        this.element
                            .on("mouseenter" + eventNamespace, function() {
                                var dropper = getDropper.call(me);
                                if (dropper && dropper.is(":hidden")) {
                                    showDropDown.call(me, dropper);
                                }
                            })
                            .on("mouseleave" + eventNamespace, $.proxy(this.hide, this));

                        getDropperAlways.call(this).on("mouseleave" + eventNamespace, $.proxy(this.hide, this));
                        break;
                    case 'Click':
                        this.element
                            .on("click" + eventNamespace, { gaiaToolbarClicked: true }, function(evt) {
                                var dropper = getDropper.call(me);
                                if (dropper) {
                                    if (dropper.is(":hidden")) {
                                        showDropDown.call(me, dropper);
                                        evt.preventDefault();
                                        evt.stopPropagation();
                                    } else if (me.options.closeOnClick) {
                                        hideDropDown.call(me, dropper);
                                    }
                                } else if (!me.options.closeOnClick) {
                                    evt.stopPropagation();
                                }
                            });
                    break;
                }

                return this;
            },

            hide: function (noVisibilityCheck) {
                var dropper = getDropper.call(this);
                if (noVisibilityCheck || (dropper && dropper.is(":visible"))) {
                    hideDropDown.call(this, dropper);
                }
            },

            destroy: function() {
                removeHandlers.call(this);
                Gaia.Extensions.ToolbarItem.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
