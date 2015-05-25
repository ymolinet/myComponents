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
    Gaia.TableCell = Class(Gaia.ContainerWebControl, function () {

        var setAttribute = function(name, value, values) {
            if (value && values) {
                return this.setAttribute(name, values[value - 1]);
            } else {
                return this.setAttribute(name, value);
            }
        };

        return {
            constructor: function(element, options) {
                Gaia.TableCell.$super.call(this, element, options);
            },

            setText: function (value, index) {
                var me = this;
                this.element.html(function(idx, html) {
                    return me.decodeArgument(html, value, index);
                });
                return this;
            },

            setColSpan: function(value) {
                return setAttribute.call(this, 'colspan', value);
            },

            setRowSpan: function(value) {
                return setAttribute.call(this, 'rowspan', value);
            },

            setHeaders: function(value) {
                if ($.isArray(value)) {
                    this.element.attr('headers', value.join(' '));
                } else {
                    this.element.removeAttr('headers');
                }
                
                return this;
            },

            setHorAlign: function(value) {
                return setAttribute.call(this, 'align', value, Gaia.WebControl.HorAligns);
            },

            setVerAlign: function(value) {
                return setAttribute.call(this, 'valign', value, Gaia.WebControl.VerAligns);
            },

            setWrap: function (value) {
                return this.setStyle({ whiteSpace: value ? 'normal' : 'nowrap' });
            },

            setAbbr: function (value) {
                var me = this;
                this.element.attr('abbr', function(idx, text) {
                    return me.decodeAttribute(text, value, index);
                });
                return this;
            },

            setScope: function(value) {
                return setAttribute.call(this, 'scope', value, Gaia.TableCell.Scopes);
            },

            $statics: {
                Scopes: ['row', 'col']
            }
        };
    });
})(window, jQuery, jsface.Class);