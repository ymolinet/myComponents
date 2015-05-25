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

(function(window, $, Class, History, undefined) {
    Gaia.BrowserHistory = Class(Gaia.Control, function () {

        var rToken = /&token=[^&]*/i;
        var rUrl = /^([^\?]+)\?*([^\?]*)$/;

        var generateUrl = function (url, token) {
            return url.replace(rToken, "").replace(rUrl, "$1?$2&token=" + token);
        };

        var lastAddedToken;

        return {
            constructor: function(element, options) {
                Gaia.BrowserHistory.$super.call(this, element, options);

                var me = this;
                History.Adapter.bind(window, "statechange", function () {
                    me.onNavigated();
                });

                if (this.options.inittoken) {
                    this.addToken(this.options.inittoken);
                } else {
                    this.onNavigated();
                }
            },

            onNavigated: function () {
                var token = History.getState().data.token;
                if (token && lastAddedToken !== token) {
                    lastAddedToken = undefined;
                    Gaia.Control.callControlMethod.call(this, 'OnNavigatedMethod', [token], null, null, /* skipDisable */ true);
                }
            },

            go: function(value) {
                History.go(value);
                return this;
            },             

            addToken: function(token) {
                lastAddedToken = token;
                var url = History.getState().url;
                History.pushState({ token: token }, null, generateUrl(url, token));
                return this;
            }
        };
    });
})(window, jQuery, jsface.Class, History);