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
    var timer;
    var lastServerCallValue;
    var eventNamespace = '.gaia_ajax_textbox';
    
    Gaia.TextBox = Class(Gaia.WebControl, {
        constructor: function(element, options) {
            options = $.extend({
                keyChangeEvents: false,
                keyChangeEventsInterval: 500
            }, options);

            Gaia.TextBox.$super.call(this, element, options);
            this.initializeDefaultValidation();
            this.setKeyChangeEvents(this.options.keyChangeEvents);
        },

        setROnly: function (value) {
            this.element.prop('readonly', !!value);
            return this;
        },

        setKeyChangeEvents: function (value) {
            var me = this;
            this.options.keyChangeEvents = value;

            this.element.off('keyup' + eventNamespace);
            
            if (value) {
                var timerTick = function(evt) {
                    var elValue = me.element.val();
                    if (lastServerCallValue !== elValue) {
                        me.lastServerCall = elValue;
                        me._onEventImpl(evt, 'keyup', true);
                    }
                };
                
                this.element.on('keyup' + eventNamespace, function() {
                    if (timer) {
                        clearTimeout(timer);
                    }
                    
                    timer = setTimeout(timerTick, me.options.keyChangeEventsInterval);
                });
            }
            
            return this;
        },

        setKeyChangeEventsInterval: function(value) {
            this.options.keyChangeEventsInterval = value;
            return this;
        },

        setText: function(value) {
            this.element.val(value);
            return this;
        },

        setSelectAll: function() {
            this.element[0].select();
            return this;
        },

        setAutoPostBack: function (value) {
            return this._setAutoPostBack(this.element, 'change', eventNamespace, value);
        },
        
        destroy: function () {
            this.element.off(eventNamespace);
            Gaia.TextBox.$superp.destroy.call(this);
        },

        _getElementPostValue: function() {
            return this._toPostPair(this.getCallbackName(), this.element.val());
        },

        _getElementPostValueEvent: function() {
            return '&__EVENTTARGET=' + this.getCallbackName() + this._getElementPostValue();
        }
    });
})(window, jQuery, jsface.Class);
