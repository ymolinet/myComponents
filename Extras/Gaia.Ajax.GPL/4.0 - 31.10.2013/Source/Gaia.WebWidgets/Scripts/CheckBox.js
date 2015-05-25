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
    Gaia.CheckBox = Class([Gaia.WebControl, Gaia.TextAlignMixin], function () {

        var checkboxEventNamespace = '.gaia_ajax_checkbox';

        return {
            constructor: function(element, options) {
                Gaia.CheckBox.$super.call(this, element, $.extend({ 'textalign': 'Right' }, options));
                this.initializeDefaultValidation();
            },

            setTabIndex: function(value) {
                var el = this._getCheckBox();
                if (value) {
                    el.attr('tabIndex', value);
                } else {
                    el.removeAttr('tabIndex');
                }

                return this;
            },

            setEnabled: function(value) {
                this._getCheckBox().prop('disabled', !value);
                return this;
            },

            isEnabled: function() {
                return !this._getCheckBox().prop('disabled');
            },

            setText : function(value) {
                this.setLabelText(this._getCheckBox(), value);
                return this;
            },
            
            setElementFocus: function() {
                this._getCheckBox().focus();
            },

            setAccessKey: function(value) {
                this._getCheckBox().attr('accessKey', value);
                return this;
            },

            setAutoPostBack: function(value) {
                return this._setAutoPostBack(this._getCheckBox(), 'click', checkboxEventNamespace, value);
            },

            setChecked: function(value) {
                this._getCheckBox().prop('checked', !!value);
                return this;
            },

            setTextAlign: function(value) {
                return this._setTextAlign(this._getCheckBox(), value);
            },

            _getElementPostValue: function() {
                var value = this._getCheckBox().prop('checked');
                return value ? this._toPostPair(this.getCallbackName(), this._getCheckBox().val()) : "";
            },

            _getElementPostValueEvent: function() {
                return '&__EVENTARGUMENT=&__EVENTTARGET=' + this._getEventTarget() + this._getElementPostValue();
            },

            _getEventTarget: function() {
                return this.getCallbackName();
            },

            _getCheckBox: function() {
                return this.element.children('input[type=checkbox]');
            },

            getCallbackName: function() {
                return this._getCheckBox().attr('name');
            },
            
            destroy: function () {
                this.setAutoPostBack(false);
                Gaia.CheckBox.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);