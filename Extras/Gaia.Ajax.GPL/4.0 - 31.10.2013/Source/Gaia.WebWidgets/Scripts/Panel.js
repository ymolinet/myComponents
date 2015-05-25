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

/* ---------------------------------------------------------------------------
   Class basically wrapping the ASP.Panel WebControl class
   --------------------------------------------------------------------------- */
(function (window, $, Class, undefined) {
    Gaia.Panel = Class(Gaia.ContainerWebControl, {
        constructor: function (element, options) {
            Gaia.Panel.$super.call(this, element, options);
            
            if (options && options.db) {
                this.setDefaultButton(options.db);
            }
        },

        setDirection: function (value) {
            if (value === 'LeftToRight') {
                this.setAttribute('dir', 'ltr');
            } else if (value === 'RightToLeft') {
                this.setAttribute('dir', 'rtl');
            } else {
                this.setAttribute('dir');
            }

            return this;
        },

        setBars: function (value) {
            return this.setScrollBars(value, this.element);
        },
        
        setScrollBars: function(value, element) {
            var defaultOverflowValue = 'visible';
            var overflowValue = defaultOverflowValue;
            var overflowXValue = defaultOverflowValue;
            var overflowYValue = defaultOverflowValue;

            switch (value) {
                case 'Auto':
                    overflowValue = 'auto';
                    break;
                case 'None':
                    overflowValue = 'visible';
                    break;
                case 'Both':
                    overflowValue = 'scroll';
                    break;
                case 'Vertical':
                    overflowXValue = 'scroll';
                    break;
                case 'Horizontal':
                    overflowYValue = 'scroll';
                    break;
            }

            element.css({
                'overflow': overflowValue,
                'overflow-x': overflowXValue,
                'overflow-y': overflowYValue
            });

            return this;
        },

        setGroupingText: function (value) {
            // if new group should be set
            var fieldset = this.element.children('fieldset');
            if (value && value.length > 0) {
                // if previous group exists, then just rename it, otherwise create.
                if (fieldset.length === 0) {
                    fieldset = $('<fieldset>').append($('<legend>').html(value));
                    this.element.children().appendTo(fieldset);
                    this.element.html(fieldset);
                } else {
                    fieldset.children('legend').html(value);
                }
            } else if (fieldset.length > 0) {
                fieldset.children().not('legend').appendTo(this.element);
                fieldset.remove();
            }

            return this;
        },

        setFocus: function () {
            var input = this.element.find(':input').first();
            if (input.length > 0) {
                input[0].focus();
            } else {
                this.element.focus();
            }
            
            return this;
        },

        setDefaultButton: function (value) {
            var defaultButtonId = value;
            var onKeyPress = function(event) {
                var element = $(event.target);
                var keyCode = event.which || event.keyCode;

                if (keyCode != 13 || element.is('textarea')) return;

                // try as Gaia control first
                var defaultButton;
                var control = Gaia.Control.get(defaultButtonId);
                if (control !== null) {
                    defaultButton = control.element;
                } else {
                    defaultButton = $("#" + defaultButtonId);
                }

                defaultButton.trigger('click');
                event.preventDefault();
                event.stopPropagation();
            };

            this.element.off('keypress.panel_default_button');
            if (value) {
                this.element.on('keypress.panel_default_button', onKeyPress);
            }
        },

        // destructor. override this one. 
        destroy: function () {
            this.setDefaultButton(undefined);
            Gaia.Panel.$superp.destroy.call(this);
        }
    });
})(window, jQuery, jsface.Class);
