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
    Gaia.Extensions = Gaia.Extensions || {};

    Gaia.Extensions.ControlPicker = Class(Gaia.Panel, function() {

        var pickerEventNamespace = ".gaia_extensions_controlpicker";
        var inputEventNamespace = ".gaia_extensions_controlpicker_input_events";

        var indexMark;
        var isMouseOverItem;
        var getItemSelector = function() {
            return '.' + this.options.className + '-controlpicker-item';
        };

        return {
            constructor: function(element, options) {

                Gaia.Extensions.ControlPicker.$super.call(this, element, options);

                // initialize events and state for the control picker
                this.initEventsAndState();

                var getContainingItem = function(evt) {
                    return $(evt.target).closest(getItemSelector.call(this), this.element.get(0));
                };

                var onClick = function (evt) {
                    var item = getContainingItem.call(this, evt);
                    if (item.length > 0) {
                        var index = this.getItems().index(item);
                        this.selectItem(evt, index);
                    }
                };

                var onMouseOver = function(evt) {
                    var item = getContainingItem.call(this, evt);
                    if (item.length > 0) {
                        var index = this.getItems().index(item);
                        this.markItem(index);
                        isMouseOverItem = true;
                    }
                };

                var onMouseOut = function(evt) {
                    var item = getContainingItem.call(this, evt);
                    if (item.length > 0) {
                        this.markItem(-1);
                        isMouseOverItem = false;
                    }
                };
                
                this.element
                    .on('click' + pickerEventNamespace, $.proxy(onClick, this))
                    .on('mouseout' + pickerEventNamespace, $.proxy(onMouseOut, this))
                    .on('mouseover' + pickerEventNamespace, $.proxy(onMouseOver, this));
            },

            reInit: function() {
                this.initEventsAndState();
                Gaia.Extensions.ControlPicker.$superp.reInit.call(this);
            },

            // initializes events and state for the control picker
            initEventsAndState: function() {
                indexMark = -1;
                this.setHoverSelection(this.options.hoverSelection); // activate item hovering.
            },

            setSkinCssClass: function(current, value) {
                this.options.className = value;
                return Gaia.Extensions.ControlPicker.$superp.setSkinCssClass.call(this, current, value);
            },

            setSel: function(values, cssClass) {
                var items = this.getItems();
                $.each(values, function(idx, value) {
                    items[value].attr('class', cssClass);
                });

                return this;
            },

            // Set item as selected and update the server
            selectItem: function(evt, index) {
                var items = this.getItems();
                if (index >= 0 && index < items.length) {
                    evt.memo = $.extend(evt.memo, { selectedIndex: index });
                    this._onEvent(evt, evt.type);
                }
            },

            setHoverSelection: function(value) {
                this.options.hoverSelection = value;

                var Keys = {
                    KEY_RETURN: 13,
                    KEY_ESC: 27,
                    KEY_LEFT: 37,
                    KEY_UP: 38,
                    KEY_RIGHT: 39,
                    KEY_DOWN: 40,
                    KEY_HOME: 36,
                    KEY_END: 35,
                    KEY_PAGEUP: 33,
                    KEY_PAGEDOWN: 34,
                };

                var onKeyDown = function (event) {
                    var index;
                    var itemCount;
                    var pageSize = 5;

                    switch (event.keyCode) {
                    case Keys.KEY_RETURN:
                        this.selectItem(event, indexMark);
                        break;
                    case Keys.KEY_ESC:
                        event.target.trigger('blur');
                        break;
                    case Keys.KEY_DOWN:
                        itemCount = this.getItems().length;
                        index = Math.min(indexMark + 1, itemCount - 1);
                        break;
                        case Keys.KEY_UP:
                        index = Math.max(0, indexMark - 1);
                        break;
                    case Keys.KEY_PAGEDOWN:
                        itemCount = this.getItems().length;
                        index = Math.min(indexMark + pageSize, itemCount - 1);
                        break;
                    case Keys.KEY_PAGEUP:
                        index = Math.max(0, indexMark - pageSize);
                        break;
                    case Keys.KEY_END:
                        index = this.getItems().length - 1;
                        break;
                    case Keys.KEY_HOME:
                        index = 0;
                        break;
                    default:
                        return;
                    }

                    if (index !== undefined) {
                        this.markItem(index, true);
                    }

                    event.preventDefault();
                    event.stopPropagation();
                };

                if (this.options.input) {
                    var input = $("#" + this.options.input);

                    input.off('keydown' + inputEventNamespace);

                    if (value) {
                        input
                            .attr('autocomplete', 'off')
                            .on('keydown' + inputEventNamespace, $.proxy(onKeyDown, this));
                    } else {
                        this.markItem(-1);
                    }
                }

                return this;
            },

            getItems: function() {
                var selector = getItemSelector.call(this).replace(/(\s+)/g, "$1.");
                return this.element.find(selector);
            },

            checkMark: function(itemCount) {
                if (indexMark < -1 || indexMark >= itemCount)
                    this.markItem(0, true);

                return this;
            },

            // mark entry. not the same as selected ...
            markItem: function(index, scrollIntoView) {
                var items = this.getItems();
                var itemHoverCssClass = this.options.className + '-controlpicker-hover';

                // remove marking from previously marked element
                if (indexMark > -1) {
                    items.eq(indexMark).removeClass(itemHoverCssClass);
                    indexMark = -1;
                }

                if (index >= 0) {
                    // mark new element
                    var element = items.eq(index).addClass(itemHoverCssClass);
                    if (element.length > 0) {
                        var domEl = element.get(0);
                        if (scrollIntoView && domEl.scrollIntoView) {
                            domEl.scrollIntoView(false);
                        }

                        indexMark = index;
                    }
                }

                return this;
            },

            destroy: function() {
                this.setHoverSelection(false);
                this.element.off(pickerEventNamespace);
                Gaia.Extensions.ControlPicker.$superp.destroy.call(this);
            },

            _getElementPostValue: function() {
                if (isMouseOverItem) {
                    return '&' + this.getCallbackName() + "=M";
                }

                return '';
            },

            _getElementPostValueEvent: function (evt) {
                if (evt.memo && (evt.memo.selectedIndex !== undefined)) {
                    return '&' + this.getCallbackName() + "=S" + evt.memo.selectedIndex;
                }

                return '&__EVENTARGUMENT=&__EVENTTARGET=' + this.element.attr('id');
            }
        };
    });
})(window, jQuery, jsface.Class);