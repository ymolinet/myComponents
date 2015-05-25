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
    Gaia.CheckBoxList = Class([Gaia.WebControl, Gaia.ListControl, Gaia.TextAlignMixin], function () {

        var checkboxEventNamespace = ".gaia_ajax_checkboxlist";

        var setCheckBoxEnabled = function(checkbox, value) {
            checkbox.prop('disabled', !value);
        };

        var getInput = function (position) {
            return this.getInputs().eq(position);
        };

        return {

            constructor: function (element, options) {
                Gaia.CheckBoxList.$super.call(this, element, $.extend({ textalign: 'Right' }, options));
                this.initializeDefaultValidation();
            },
            
            setElementFocus: function () {
                getInput.call(this, 0).focus();
                return this;
            },

            change: function (position, item) {
                var input = getInput.call(this, position);
                
                if (item.text != null) {                    
                    this.setLabelText(input, item.text);
                }

                if (item.selected != null) {
                    input.prop('checked', !!item.selected);
                }

                if (item.enabled != null) {
                    setCheckBoxEnabled.call(this, input, item.enabled);
                }
                
                if (item.value != null) {
                    input.val(item.value);
                }

                return this;
            },

            setTextAlign: function (value) {
                return this._setTextAlign(this.getInputs(), value);
            },

            getInputs: function () {
                return this.element.find(':checkbox');
            },

            setEnabled: function (value) {
                var me = this;
                me.getInputs().each(function(idx, val) {
                    setCheckBoxEnabled.call(me, $(val), value);
                });                
                return me;
            },

            storeEnabled: function () {
                this.$$enabledstate$$ = this.getInputs().map(function (idx, val) {
                    var checkbox = $(val);
                    return {
                        checkbox: checkbox,
                        enabled: !checkbox.prop("disabled")
                    };
                });
            },

            restoreEnabled: function () {
                var me = this;
                if (me.$$enabledstate$$) {
                    me.$$enabledstate$$.each(function (idx, state) {
                        setCheckBoxEnabled.call(me, state.checkbox, state.enabled);
                    });
                }
                me.$$enabledstate$$ = undefined;
            },
            
            setAutoPostBack: function (value) {
                return this._setAutoPostBack(this.getInputs(), 'click', checkboxEventNamespace, value);
            },

            _getElementPostValue: function () {
                var me = this,
                    checkedInputs = this.element.find(':checked'),
                    retVal = '',
                    checkbox;

                checkedInputs.each(function (idx, val) {
                    checkbox = $(val);
                    retVal += me._toPostPair(checkbox.attr('name'), checkbox.val());
                });

                return retVal;
            },

            _getElementPostValueEvent: function (evt) {
                return '&__EVENTARGUMENT=&__EVENTTARGET=' + this._getEventTarget($(evt.target)) + this._getElementPostValue();
            },

            _getEventTarget: function (input) {
                return input.attr('name');
            },
            
            destroy: function () {
                this.setAutoPostBack(false);
                Gaia.WebControl.$superp.destroy.call(this);
            },

            add: function (position, item) {
                $.error("StateManagement failed");
            },

            remove: function (position) {
                $.error("StateManagement failed");
            }
                    
        };
    });
})(window, jQuery, jsface.Class);