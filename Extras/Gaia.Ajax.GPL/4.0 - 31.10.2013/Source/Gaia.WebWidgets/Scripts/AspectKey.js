
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
    Gaia.AspectKey = Class(Gaia.AspectWithEvents,  function () {
        return {
            constructor: function(parentId, options) {
                Gaia.AspectKey.$super.call(this, parentId, options);
            },

            initEvents: function () {
                var me = this;
                var options = me.options;

                var onKeyPressed = function(event) {
                    var keyCode = event.which || event.keyCode;
                    var modifiers = (event.shiftKey ? 0x1 : 0) | (event.ctrlKey ? 0x10 : 0) | (event.altKey ? 0x100 : 0);
                    var type = event.type === 'keypress' ? 1 : 0;
                    var key = type + '#' + keyCode + '#' + modifiers;
                    var suppress = 0;
                    var found = false;
                    if (options.filters) {
                        $.each(options.filters, function (idx, item) {
                            var matches = $.isArray(item) && item[0] === key;
                            if (matches) {
                                suppress = item[1];
                            }
                            found = matches || item === key;
                            return !found;
                        });
                    }

                    if (options.filters && !found) return;
                    
                    suppress = suppress || options.suppress;
                    var preventDefault = (suppress & 0x1) === 0x1;
                    var stopPropagation = (suppress & 0x10) === 0x10;
                    if (preventDefault) event.preventDefault();
                    if (stopPropagation) event.stopPropagation();
                    
                    Gaia.Control.callAspectMethod.call(me.getWrappedControl(), 'KeyMethod', [event.type, keyCode, modifiers]);
                };

                $.each(this.options.evts, function(idx, evt) {
                    me.addEvent(evt, onKeyPressed);
                });
            }
        };
    });
})(window, jQuery, jsface.Class);
