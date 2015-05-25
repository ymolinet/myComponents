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
    Gaia.RadioButtonList = Class(Gaia.CheckBoxList, function() {

        return {

            constructor: function (element, options) {
                Gaia.RadioButtonList.$super.call(this, element, options);
                
                if (this.options.fixDis) {
                    this.element.removeAttr("disabled");
                    this.element.find("span[disabled]").removeAttr("disabled");
                }
            },
            
            getInputs: function () {
                return this.element.find(':radio');
            },

            _getElementPostValue: function () {
                var me = this;
                var returnValue = '';
                    
                me.getInputs().each(function (idx, val) {
                    var radio = $(val);
                    if (radio.prop('checked')) {
                        returnValue = me._toPostPair(radio.attr('name'), radio.val());
                        return false;
                    }
                });

                return returnValue;
            },

            _getEventTarget: function (input) {
                return input.attr('id').replace(/_/g, '$');
            }
        };

    });
})(window, jQuery, jsface.Class);