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

/*
* ListControl Mixin
*/
(function (window, $, Class, undefined) {
    
    var support = {};
    (function () {
        var select = $("<select name='s' size='3'><option selected='selected' /></select>");

        try {
            var option = select.children('option');
            option.prop("selected", false);
            option.text("t1");
            option.val("v1");
            support.optionModifications = !option.prop("selected") && option.text() === "t1" && option.val() == "v1";
            option.remove();
        } catch (e) {
            support.optionModifications = false;
        } finally {
            select = null;
        }
    })();

    Gaia.ListControl = Class(function () {

        var listControlEventNamespace = ".gaia_ajax_listcontrol";

        var doAdd = function(position, item) {
            var options = this.element.children();
            var option = new Option(item.text, item.value, false, item.selected);

            if (position < options.length) {
                this.element.children().eq(position).before(option);
            } else {
                this.element.append(option);
            }

            return this;
        };

        var doChange = function(position, item) {
            var target = this.element.children().eq(position);
            var option = support.optionModifications ? target : target.clone(true, true);

            if (item.text != null) {
                option.text(item.text);
            }
            if (item.value != null) {
                target.val(item.value);
            }
            if (item.selected != null) {
                target.prop('selected', !!item.selected);
            }
        
            if (!support.optionModifications) {
                target.replaceWith(option);
            }

            return this;
        };

        return {
            
            clear: function () {
                this.element.empty();
                return this;
            },

            add: function (position, item) {
                return doAdd.call(this, position, item);
            },

            remove: function (position) {
                this.element.children().eq(position).remove();
                return this;
            },

            change: function (position, item) {
                return doChange.call(this, position, item);
            },

            setAutoPostBack: function (value) {
                return this._setAutoPostBack(this.element, 'change', listControlEventNamespace, value);
            }
        };
    });
})(window, jQuery, jsface.Class);
