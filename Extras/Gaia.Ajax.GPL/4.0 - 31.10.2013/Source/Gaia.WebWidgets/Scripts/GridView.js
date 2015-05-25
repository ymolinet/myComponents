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
    Gaia.GridView = Class(Gaia.ContainerWebControl, function() {

        var getCaption = function() {
            return this.element.children("caption") || $('<caption/>').prependTo(this.element);
        };

        var setAttribute = function(element, name, value, values) {
            if (value) {
                element.attr(name, values[value - 1]);
            } else {
                element.removeAttr(name);
            }
        };

        return {
            constructor: function(element, options) {
                Gaia.GridView.$super.call(this, element, options);
            },

            setBackImgUrl: function(value) {
                return this.setStyle({ backgroundImage: 'url(' + value + ')' });
            },

            setContent: function(value) {
                this.element.html(value);
                return this;
            },

            setCaption: function(value) {
                getCaption().call(this).html(value);
                return this;
            },

            setCaptionAlign: function(value) {
                setAttribute(getCaption.call(this), 'align', value, Gaia.GridView.CaptionAligns);
                return this;
            },

            setPadding: function(value) {
                return this.setAttribute('cellpadding', value === -1 ? undefined : value);
            },

            setSpacing: function(value) {
                if (value > -1) {
                    this.setStyle({ borderCollapse: value === 0 ? 'collapse' : '' });
                }

                return this.setAttribute('cellspacing', value === -1 ? undefined : value);
            },

            setRules: function(value) {
                setAttribute(this.element, 'rules', value, Gaia.GridView.Rules);
                return this;
            },

            setHorAlign: function(value) {
                setAttribute(this.element, 'align', value, Gaia.WebControl.HorAligns);
                return this;
            },

            $statics: {
                Rules: ['rows', 'cols', 'all'],
                CaptionAligns: ['top', 'bottom', 'left', 'right']
            }
        };
    });

    Gaia.GridViewRow = Class(Gaia.ContainerWebControl, {
        constructor: function(element, options) {
            Gaia.GridViewRow.$super.call(this, element, options);
        }
    });
})(window, jQuery, jsface.Class);