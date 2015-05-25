/*******************************************************************
* Gaia Ajax - Ajax Control Library for ASP.NET  
* Copyright (C) 2008 - 2013 Gaiaware AS
* All rights reserved. 
* This program is distributed under either GPL version 3 
* as published by the Free Software Foundation or the
* Gaia Commercial License version 1 as published by Gaiaware AS
* read the details at http://gaiaware.net/product/dual-licensing 
******************************************************************/

(function (window, $, Class, undefined) {

    var support = {
        selectstart: "onselectstart" in document.createElement("div")
    };

    var uuid = 0;

    Gaia.Collector = Class(Gaia.Control, function () {

        var eventNamespace = ".gaia-collector";

        var onMouseDown = function(state, evt) {
            //if (Gaia.ResizeManager && Gaia.ResizeManager._activeElementObject) return;
            var eventSourceSelector = this.options.es;
            if (eventSourceSelector && !$(evt.target).is(eventSourceSelector)) return;

            if (!support.selectstart) {
                evt.preventDefault();
            }

            // check if mouse up was not called, such as happened outside of the window
            if (state.moved) {
                onMouseUp.call(this, evt);
            }

            var widgets = $($.map(Gaia.Control.getAll(), function(control) { return control.element[0]; }));
            if (this.options.ts) {
                widgets = widgets.filter(this.options.ts);
            }

            var observables = widgets.map(function() {
                var element = $(this);

                var offset = element.offset();
                var width = element.width();
                var height = element.height();

                return {
                    'id': this.id,
                    'bounds': {
                        top: offset.top,
                        left: offset.left,
                        right: offset.left + width,
                        bottom: offset.top + height
                    }
                };
            });

            state.isTracking = true;
            state.startX = evt.pageX;
            state.startY = evt.pageY;
            state.observables = observables;
        };

        var onMouseMove = function (state, evt) {
            if (state.isTracking) {
                state.moved = true;
                state.selection = getSelection.call(this, state, evt.pageX, evt.pageY);
            }
        };

        var onMouseUp = function (state, evt) {
            var selectedClass = this.options.cs;
            var eventSourceSelector = this.options.es;
            
            if (state.isTracking) {
                var shouldCheck = !state.moved;

                state.moved = false;
                state.isTracking = false;
                
                if (!eventSourceSelector || $(evt.target).is(eventSourceSelector)) {
                    if (shouldCheck) {
                        state.selection = getSelection.call(this, state, evt.pageX, evt.pageY);
                    }

                    if (state.selection && state.selection.length) {
                        if (selectedClass) {
                            $.each(state.selection, function(idx, id) {
                                $("#" + id).removeClass(selectedClass);
                            });
                        }

                        Gaia.Control.callControlMethod.call(this, 'Selected', [state.selection.join(";")], null);
                    }
                }
            }
        };
        
        var getSelection = function (state, endX, endY) {
            var top = Math.min(state.startY, endY);
            var left = Math.min(state.startX, endX);
            var right = Math.max(state.startX, endX);
            var bottom = Math.max(state.startY, endY);

            var selectedClass = this.options.cs;

            return $.map(state.observables, function (observable) {
                var bounds = observable.bounds;

                var intersectionTop = Math.max(top, bounds.top);
                var intersectionLeft = Math.max(left, bounds.left);
                var intersectionRight = Math.min(right, bounds.right);
                var intersectionBottom = Math.min(bottom, bounds.bottom);

                if (intersectionTop <= intersectionBottom && intersectionLeft <= intersectionRight) {
                    if (selectedClass) {
                        $("#" + observable.id).addClass(selectedClass);
                    }

                    return observable.id;
                } else if (selectedClass) {
                    $("#" + observable.id).removeClass(selectedClass);
                }

                return null;
            });
        };

        return {
            constructor: function(element, options) {

                Gaia.Collector.$super.call(this, element, options);

                var state = {};
                this.instanceNumber = ++uuid;

                $(document)
                    .on("mouseup" + eventNamespace + this.instanceNumber, $.proxy(onMouseUp, this, state))
                    .on("mousedown" + eventNamespace + this.instanceNumber, $.proxy(onMouseDown, this, state))
                    .on("mousemove" + eventNamespace + this.instanceNumber, $.proxy(onMouseMove, this, state));

                if (support.selectstart) {
                    $(document).on("selectstart" + eventNamespace + this.instanceNumber, function (evt) {
                        if (state.isTracking) {
                            evt.preventDefault();
                        }
                    });
                }
            },

            setFilter: function(value) {
                this.options.ts = value;
                return this;
            },

            setTCss: function(value) {
                this.options.cs = value;
                return this;
            },

            setESFilter: function(value) {
                this.options.es = value;
                return this;
            },

            destroy: function() {
                $(document).off(eventNamespace + this.instanceNumber);
                Gaia.ControlCollector.$superp.destroy.call(this);
            }
        };
    });
})(window, jQuery, jsface.Class);