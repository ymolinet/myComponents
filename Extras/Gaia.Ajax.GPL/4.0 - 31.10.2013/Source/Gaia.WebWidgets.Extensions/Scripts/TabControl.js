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
 
(function (window, $, Class, undefined) {
    Gaia.Extensions = Gaia.Extensions || {};

    Gaia.Extensions.TabView = Class(Gaia.Panel, {
        constructor: function(element, options) {
            Gaia.Extensions.TabView.$super.call(this, element, $.extend({ active: false }, options));
            this.setActive(this.options.active);
        },

        setActive: function(value) {
            return this.setStyle({ 'display': value ? 'block' : 'none' });
        }
    });

    Gaia.Extensions.TabControl = Class(Gaia.Panel, function () {
        var headerClickEventName = "click.tab_control_header_events";
        
        return {
            constructor: function(element, options) {
                Gaia.Extensions.TabControl.$super.call(this, element, $.extend({ inactive: [] }, options));

                var me = this;
                $.each(this.options.inactive, function(idx, tabView) { me.setActive(tabView, false); });

                this.getHeaderContainer().on(headerClickEventName, $.proxy(this.onClick, this));
            },

            onClick: function (evt) {
                var tabViewHeader = $(evt.target).closest('li', this.getHeaderContainer().get(0));
                if (tabViewHeader.length > 0 && !tabViewHeader.data('tabview_not_clickable')) {
                    Gaia.Control.callControlMethod.call(this, 'TabViewHeaderClicked', [tabViewHeader.index()], null, this.element.attr('id'));
                }
            },

            clear: function(start, count) {
                if (!isNaN(start)) {
                    // remove headers from start index to count
                    var end = isNaN(count) ? start + 1 : count;
                    for (var index = start; index < end; ++index) {
                        this.getTabViewHeader(index).remove();
                    }
                } else {
                    // clear all tab view headers
                    this.getHeaderContainer().html("");
                } 

                return this;
            },

            add: function (value, index) {
                this.getTabViewHeader(index).before(value);
                return this;
            },

            update: function(index, options) {
                if (options) {
                    if (options.active !== undefined)
                        this.setActive(index, options.active);

                    if (options.classHeader !== undefined)
                        this.getTabViewHeader(index).attr('class', options.classHeader);

                    if (options.caption !== undefined)
                        this.getTabViewCaption(index).html(options.caption);

                    if (options.classContent !== undefined)
                        this.getTabViewCaption(index).attr('class', options.classContent);
                }

                return this;
            },

            setActive: function(index, value) {
                this.getTabViewHeader(index).data('tabview_not_clickable', !value);
            },

            setContentHeight: function(value) {
                this.getViewContainer().css({ 'height': value });
                return this;
            },

            setContentWidth: function(value) {
                this.getViewContainer().css({ 'width': value });
                return this;
            },

            getViewContainer: function() {
                return $("#" + this.element.attr('id') + "_content");
            },

            getHeaderContainer: function () {
                return this.element.find('div > div > ul');
            },
            
            getTabViewHeader: function(index) {
                return $(this.getHeaderContainer().children('li').get(index));
            },

            getTabViewCaption: function(index) {
                return $("#" + this.element.attr('id') + '_content_' + index);
            },

            _getElementPostValueEvent: function() {
                return '&__EVENTARGUMENT=&__EVENTTARGET=' + this.element.attr('id');
            },

            destroy: function() {
                this.getHeaderContainer().off(headerClickEventName);
                Gaia.Extensions.TabControl.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);