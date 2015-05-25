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
    Gaia.ImageMap = Class(Gaia.Image, function () {

        var or = function (other) {
            return this.length ? this : ($.isFunction(other) ? other() : other);
        };

        var imageMapEventNamespace = ".gaia_ajax_imagemap";

        var registerInitialAreaHandlers = function () {
            
            if (!this.options.spots) {
                return;
            }

            var spots = $.map((this.options.spots).split(','), function (idx, val) {
                return parseInt(val, 10);
            });

            var areas = getAreas.call(this)
                .filter(function(idx) {
                    return $.inArray(idx, spots) > -1;
                });

            addHandler.call(this, areas);
        };

        var removeHandler = function(elements) {
            elements.off('click' + imageMapEventNamespace);
        };

        var addHandler = function(elements) {
            elements.on('click' + imageMapEventNamespace, $.proxy(this._onEvent, this));
        };

        var applyHotSpot = function(hotSpot, area) {

            switch (hotSpot.md) {
                case '1':
                    area.attr('nohref', 'nohref');
                    break;
                case '2':
                    area.attr('href', 'javascript:void(0)');
                    addHandler.call(this, area);
                    break;
                default:
                    area.attr('href', hotSpot.nu || "");
                    break;
            }

            area.attr('shape', hotSpot.mn || 'default');
            checkSet.call(this, hotSpot, 'ak', area, 'accessKey');
            checkSet.call(this, hotSpot, 'at', area, 'alt');
            checkSet.call(this, hotSpot, 'at', area, 'title');
            checkSet.call(this, hotSpot, 'ti', area, 'tabIndex');
            checkSet.call(this, hotSpot, 'tg', area, 'target');
            checkSet.call(this, hotSpot, 'cd', area, 'coords');
        };

        var checkSet = function(spot, spotAttributeName, area, areaAttributeName) {
            var value = spot[spotAttributeName];
            if (value) {
                area.attr(areaAttributeName, value);
            }
        };

        var getMap = function() {
            return this.element.next("map");
        };

        var getAreas = function() {
            return getMap.call(this).children();
        };

        var createMap = function() {
            var id = 'ImageMap' + this.element.attr('id');

            var map = $('<map />').attr({
                id: id,
                name: id
            });

            this.element
                    .attr('usemap', '#' + id)
                    .after(map);

            return map;
        };

        return {

            constructor: function (element, options) {
                Gaia.ImageMap.$super.call(this, element, options);
                registerInitialAreaHandlers.call(this);
            },
            
            clear: function () {
                removeHandler(getAreas.call(this));
                getMap.call(this).remove();
                this.element.removeAttr('useMap');
                return this;
            },

            add: function (hotSpot) {
                var area = $('<area />');
                applyHotSpot.call(this, hotSpot, area);

                or.call(getMap.call(this),
                    $.proxy(createMap, this))
                    .append(area);

                return this;
            },

            remove: function (index) {
                var area = getAreas.call(this).eq(index);
                removeHandler(area);
                area.remove();
                return this;
            },

            change: function (index, hotSpot) {
                var area = getAreas.call(this)
                    .eq(index)
                    .removeAttr('href')
                    .removeAttr('nohref');

                removeHandler(area);
                applyHotSpot.call(this, hotSpot, area);

                return this;
            },
          
            _getElementPostValueEvent: function (evt) {
                return '&__EVENTTARGET=' + this.element.attr('id').replace(/_/g, '$') + '&__EVENTARGUMENT=' + getAreas.call(this).index($(evt.target));
            },

            destroy: function () {
                removeHandler(getAreas.call(this));
                Gaia.ImageMap.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);