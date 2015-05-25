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
   Class basically wrapping the common functions of Container Widgets 
   (Window, Panel, MultiView, ViewportView etc)
   --------------------------------------------------------------------------- */

(function (window, $, Class, undefined) {

    Gaia.Container = Class(Gaia.Control, function () {
        var skipChildrenDataName = "gaia_ajax_container_skip_children";

        var destroyChildren = function() {
            var controls = [];

            this.eachGaiaChild(function (control, id) {
                controls.push({ control: control, id: id });
            });

            controls.sort(function(ctrlA, ctrlB) { return ctrlB.id.length - ctrlA.id.length; });

            $.each(controls, function(idx, ctrl) {
                ctrl.control.element.data(skipChildrenDataName, true);
                ctrl.control.destroy();
            });
        };
        
        return {
            constructor: function(element, options) {
                Gaia.Container.$super.call(this, element, options);
            },

            isAjaxContainer: function() {
                return true;
            },

            setEnabled: function(value, skipChildren) {
                this.setSelfEnabled(value);

                if (!skipChildren) {
                    this.eachGaiaChild(function(child) {
                        child.setEnabled(value, true);
                    });
                }

                return this;
            },

            storeEnabled: function(value, skipChildren) {
                this.storeSelfEnabled();

                if (!skipChildren) {
                    this.eachGaiaChild(function(child) {
                        child.storeEnabled(value, true);
                    });
                }
            },

            restoreEnabled: function(value, skipChildren) {
                this.restoreSelfEnabled();

                if (!skipChildren) {
                    this.eachGaiaChild(function(child) {
                        child.restoreEnabled(value, true);
                    });
                }
            },

            setSelfEnabled: $.noop,
            storeSelfEnabled: $.noop,
            restoreSelfEnabled: $.noop,

            forceAnUpdate: function() {
                destroyChildren.call(this);

                if (this.options.aspects) {
                    $.each(this.options.aspects, function(idx, aspect) {
                        aspect.forceAnUpdate();
                    });
                }
            },

            appendHtml: function(html) {
                this.getBody().append(html);
                return this;
            },

            getBody: function() {
                var ctrl = $('#' + this.element.attr("id") + "_content");
                return ctrl.length > 0 ? ctrl : this.element;
            },

            destroy: function() {
                if (!this.element.data(skipChildrenDataName)) {
                    destroyChildren.call(this);
                }

                Gaia.Container.$superp.destroy.call(this);
            },

            // Called after the forceAnUpdate and the "new innerHTML" has been set.
            reInit: function() {
                // Since DOM element physically has been removed and then added again we run the initialization again...
                // Note that the Control reference doesn't need to be "re-initialized" since it contains a reference to
                // the Gaia object on the "element.id" hash value...
                // Only the "this.element" needs to be re-initialized
                this.element = $('#' + this.element.attr('id'));
                if (this.options.aspects) {
                    $.each(this.options.aspects, function (idx, aspect) { aspect.reInit(); });
                }
            }
        };
    });

    Gaia.ContainerWebControl = Class([Gaia.WebControl, Gaia.Container], {
        constructor: function(element, options) {
            Gaia.ContainerWebControl.$super.call(this, element, options);
        },

        setSelfEnabled: function (value) {
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

        storeSelfEnabled: function() {
            this.enabledState = this.isEnabled();
        },

        restoreSelfEnabled: function() {
            if (this.enabledState !== undefined) {
                this.setSelfEnabled(this.enabledState);
            }

            this.enabledState = undefined;
        }
    });
})(window, jQuery, jsface.Class);
