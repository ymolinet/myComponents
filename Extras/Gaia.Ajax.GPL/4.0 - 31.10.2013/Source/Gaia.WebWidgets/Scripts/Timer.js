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
    Gaia.Timer = Class(Gaia.Control, function () {
        var timer;

        var timerLoop = function () {
            var me = this;
            timer = setTimeout(function() {
                me._onEvent($.Event("gaia:timerTick"), undefined, undefined);
                timerLoop.call(me);
            }, me.options.milliseconds);
        };
        
        return {
            constructor: function(element, options) {
                Gaia.Timer.$super.call(this, element, options);
            },

            setEnabled: function(value) {
                this.options.enabled = !!value;

                if (timer) {
                    clearTimeout(timer);
                    timer = undefined;
                }

                if (value) {
                    timerLoop.call(this);
                }

                return this;
            },

            isEnabled: function() {
                return this.options.enabled;
            },

            storeEnabled: function() {
                this.enabledState = this.options.enabled;
            },

            restoreEnabled: function () {
                if (this.enabledState !== undefined) {
                    this.setEnabled(this.enabledState);
                }
                this.enabledState = undefined;
            },
            
            setMilliseconds: function(value) {
                var wasEnabled = this.options.enabled;

                this.setEnabled(false);
                this.options.milliseconds = value;
                this.setEnabled(wasEnabled);

                return this;
            },

            setReset: function(value) {
                if (value) {
                    var wasEnabled = this.options.enabled;
                    this.setEnabled(false);
                    this.setEnabled(wasEnabled);
                }

                return this;
            },

            destroy: function() {
                this.setEnabled(false);
                Gaia.Timer.$superp.destroy.call(this);
            },

            _getElementPostValueEvent: function() {
                return '&__EVENTARGUMENT=&__EVENTTARGET=' + this.getCallbackName();
            }
        };
    });
})(window, jQuery, jsface.Class);