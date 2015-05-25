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

    var taskClassName = "gaia-re-task";
    var taskDataKey = "gaia-resizable-task";

    $(document).on("gaia:resizeStart", function(evt) {
        var element = $(evt.target);
        if (element.is("." + taskClassName)) {
            element.data(taskDataKey, {
                top: evt.memo.position.top,
                bottom: evt.memo.position.top + element.height()
            });
        }
    });

    Gaia.ART = Gaia.AspectResizableTask = Class(Gaia.AspectResizable, {
        constructor: function (parentId, options) {
            Gaia.AspectResizableTask.$super.call(this, parentId, $.extend(options, {
                sp: $.proxy(this.resizableSnap, this)
            }));
            
            $("#" + parentId).addClass(taskClassName);
        },
        
        onResized: function(evt, ui) {
            var control = this.getWrappedControl();
            var element = control.element;
            
            var position = ui.position;
            var data = element.data(taskDataKey);
                    
            var topSnap = data.topSnap || 0;
            var bottomSnap = data.bottomSnap || 0;
            
            Gaia.Control.callAspectMethod.call(control, 'ResizedMethod',
                [position.left, position.top - topSnap, element.outerWidth(), element.outerHeight() + topSnap - bottomSnap],
                null, element.attr("id"));
        },

        resizableSnap: function (x, y, options) {
            var bounds = this.options.bounds;
            var cellHeight = this.options.clh;
            var snapSize = this.options.ssz;
            var borderSize = this.options.bsz;
            var data = options.element.data(taskDataKey);
            var resizable = options.element.data("ui-resizable");

            var nextY;
            var snapY = Math.round(y / snapSize) * snapSize;
            if (options.resizeMode.bottom) {
                data.topSnap = 0;
                if (resizable.position.top + resizable.size.height <= bounds.bottom) {
                    nextY = data.bottom + snapY;
                    data.bottomSnap = (Math.floor(nextY / cellHeight) - Math.floor(data.bottom / cellHeight)) * borderSize;
                }
            } else {
                data.bottomSnap = 0;
                if (resizable.position.top >= bounds.top) {
                    nextY = data.top + snapY;
                    data.topSnap = Math.floor(nextY / cellHeight) * borderSize;
                }
            }

            return [0, snapY + data.bottomSnap + data.topSnap];
        }
    });
})(window, jQuery, jsface.Class);