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
    Gaia.ListBox = Class([Gaia.WebControl, Gaia.ListControl], function () {
        
        return {
            constructor: function (element, options) {
                Gaia.ListBox.$super.call(this, element, options);
                this.initializeDefaultValidation();
            },

            setMode: function (value) {
                if (value === 'Single') {
                    this.element.removeAttr('multiple');
                } else {
                    this.element.attr('multiple', 'multiple');
                }
                
                return this;
            },

            setRows: function (value) {
                this.element.attr('size', value);
                return this;
            },

            _getElementPostValue: function () {
                var me = this;
                var returnValue = '';
                var selectedValues = this.element.val();

                if (!selectedValues) {
                    return returnValue;
                }
                
                if ($.isArray(selectedValues)) {
                    $.each(selectedValues, function (idx, val) {
                        returnValue += me._toPostPair(me.getCallbackName(), val);
                    });
                    return returnValue;
                }

                return this._toPostPair(this.getCallbackName(), selectedValues);
            },
            
            _getElementPostValueEvent: function () {
                var postValue = this._getElementPostValue();
                return (postValue ? postValue : '&' + this.getCallbackName() + '=') + '&__EVENTTARGET=' + this.getCallbackName();
            }
           
        };
    });
})(window, jQuery, jsface.Class);