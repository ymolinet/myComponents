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
   AspectModal, adds up Modality support for Controls
   --------------------------------------------------------------------------- */
(function (window, $, Class, undefined) {

    var support = {};
    (function () {
        var body = $(document.body);

        var height = body.height();
        var scrollTop = body.scrollTop();

        var subject = $("<div/>").css({ position: "fixed", top: "10px" }).appendTo(body);

        body.height(3000).scrollTop(scrollTop + 10);
        support.positionFixed = subject.offset().top === 10;
        body.height(height).scrollTop(scrollTop);
    })();
    
    Gaia.AspectModal = Class(Gaia.Aspect, function () {

        var modalWidgetClassName = "gaia-modal-widget";
        var modalBackgroundClassName = "gaia-modal-back";
        var modalContainerClassName = "gaia-modal-container";

        var backEventNamespace = ".gaia-ajax-modality-back-events";
        var windowEventNamespace = ".gaia-ajax-modality-window-events";
        var originalStates = {}; // keeps original states to restore during destruction

        var bubbleStopHandler = function (evt) {
            evt.stopImmediatePropagation();
            evt.stopPropagation();
            evt.preventDefault();
        };

        var timer;
        var onWindowResize = function(callback) {
            clearTimeout(timer);
            timer = setTimeout(callback, 100);
        };
        $(window).on("resize" + windowEventNamespace, $.proxy(onWindowResize, window, function() {
            $("iframe." + modalBackgroundClassName)
                .width($(document).width())
                .height($(document).height());
        }));

        var createBackground = function (options) {
            var background;

            if (support.positionFixed) {
                background = $("<div/>")
                    .addClass(modalBackgroundClassName)
                    .css({
                        position: "fixed",
                        top: "0px",
                        left: "0px",
                        width: "100%",
                        height: "100%",
                        borderStyle: "none",
                        zIndex: options.zIndex,
                        opacity: options.opacity,
                        backgroundColor: options.color
                    });
            } else {
                background = $("<iframe/>")
                    .attr({
                        src: "about:blank",
                        frameborder: 0,
                        scrolling: "no",
                        "class": modalBackgroundClassName
                    })
                    .css({
                        borderStyle: "none",
                        position: "absolute",
                        zIndex: options.zIndex,
                        opacity: options.opacity
                    })
                    .one("load", function(evt) {
                        $(evt.target)
                            .offset({ left: 0, top: 0 })
                            .width($(document).width())
                            .height($(document).height());
                        evt.target.contentWindow.document.body.style.backgroundColor = options.color;
                    });
            }

            return background
                .on("click" + backEventNamespace, bubbleStopHandler)
                .on("mouseUp" + backEventNamespace, bubbleStopHandler)
                .on("mouseDown" + backEventNamespace, bubbleStopHandler);
        };

        return {
            constructor: function (parentId, options) {
                
                Gaia.AspectModal.$super.call(this, parentId, $.extend({
                    color: '#aaf',
                    opacity: 0.5
                }, options));

                var me = this;
                var state = {};
                var element = $("#" + this.parentId);

                element
                    .addClass(modalWidgetClassName)
                    .css("position", function(idx, value) {
                        state.position = value;
                        return (!value || value === "static") ? "relative" : value;
                    });
                
                Gaia.WebControl.bringElementToFront(element);
                
                var backgroundZ;
                element.css("zIndex", function(idx, value) {
                    state.zIndex = value;
                    backgroundZ = parseInt(value, 10) || 1;
                    return backgroundZ + 1;
                });

                originalStates[parentId] = state;

                createBackground.call(this, $.extend({}, this.options, { zIndex: backgroundZ }))
                    .appendTo(element.parent());

                element
                    .parents()
                    .not("." + modalContainerClassName)
                    .filter(function() {
                        return $(this).css("overflow") === "hidden";
                    })
                    .each(function() {
                        $(this)
                            .uniqueId()
                            .addClass(modalContainerClassName)
                            .css("zIndex", function(idx, value) {
                                originalStates[this.id] = { zIndex: value };
                                backgroundZ = parseInt(value, 10) || 1;
                                return backgroundZ + 1;
                            })
                            .after(createBackground.call(me, $.extend({}, me.options, { zIndex: backgroundZ })));
                    });
            },

            destroy: function() {
                var element = $("#" + this.parentId);
                
                element
                    .removeClass(modalWidgetClassName)
                    .css(originalStates[this.parentId])
                    .nextAll("." + modalBackgroundClassName)
                    .off(backEventNamespace)
                    .remove();

                element
                    .parents("." + modalContainerClassName)
                    .each(function () {
                        
                        var parent = $(this);
                        if (parent.find("." + modalWidgetClassName).length > 0) {
                            return false;
                        }

                        parent.removeClass(modalContainerClassName)
                            .css(originalStates[this.id])
                            .removeUniqueId()
                            .nextAll("." + modalBackgroundClassName)
                            .off(backEventNamespace)
                            .remove();
                    });
                
                Gaia.AspectModal.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);
