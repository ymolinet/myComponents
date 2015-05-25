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

    Gaia.Extensions.ExtendedButton = Class(Gaia.Button, function () {

        var getButton = function() {
            return $("#" + this.element.attr("id") + '_btn');
        };

        return {
            constructor: function(element, options) {
                Gaia.Extensions.ExtendedButton.$super.call(this, element, options);
            },

            setSkinCssClass: function(current, value) {
                this.options.className = current;
                Gaia.Extensions.ExtendedButton.$superp.setSkinCssClass.call(this, current, value);
            },

            // sets the button to either toggled or un-toggled. 
            setToggle: function(value) {
                this.element.toggleClass(this.options.className + '-button-checked', !!value);
                return this;
            },

            setText: function (value, index) {
                var me = this;
                $("#" + this.element.attr('id') + '_content').html(function(idx, html) {
                    return me.decodeArgument(html, value, index);
                });
                
                return this;
            },

            // Toggle enabled/disabled button
            setEnabled: function (value) {
                getButton.call(this).prop('disabled', !value);
                this.element.toggleClass(this.options.className + '-button-disabled', !value);
                
                return this;
            },

            isEnabled: function() {
                return !getButton.call(this).is(':disabled');
            },

            // Sets the focus to the button
            setFocus: function() {
                getButton.call(this).focus();
                this.element.toggleClass(this.options.className + '-button-focus', true);
                return this;
            }
        };
    });
})(window, jQuery, jsface.Class);
