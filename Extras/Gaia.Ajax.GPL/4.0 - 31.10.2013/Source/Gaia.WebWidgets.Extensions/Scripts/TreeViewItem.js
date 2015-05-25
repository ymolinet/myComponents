// Gaia Ajax Copyright (C) 2008 - 2013 Gaiaware AS. details at http://gaiaware.net/

/* 
 * Gaia Ajax - Ajax Control Library for ASP.NET
 * Copyright (C) 2008 - 2013 Gaiaware AS
 * All rights reserved.
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net
 */

/* ---------------------------------------------------------------------------
   Class wrapping a TreeViewItem.
   --------------------------------------------------------------------------- */
(function(window, $, Class, undefined) {
    Gaia.Extensions = Gaia.Extensions || {};

    Gaia.TreeItem = Gaia.Extensions.TreeViewItem = Class(Gaia.Container, function() {

        var collapseQueue = $({});
        var eventNamespace = ".extensions_treeviewitem";

        var onContainerClicked = function(evt) {
            var element = evt.target;

            if (!this.options.isLeaf) {
                var expander = this.getExpander().get(0);
                if (expander === element || $.contains(expander, element))
                    return;
            }
            
            this._onEvent(evt, 'click', true);
        };

        var addEventListeners = function() {
            if (!this.options.isLeaf) {
                var handler = function(evt) { this._onEvent(evt, 'click', true); };
                this.getExpander().on('click' + eventNamespace, $.proxy(handler, this));
            }

            if (this.options.allowSelections) {
                this.getContainer().on('click' + eventNamespace, { nodeSelectEvent: true }, $.proxy(onContainerClicked, this));
            }
        };

        var removeEventListeners = function() {
            this.getExpander().off('click' + eventNamespace);
            this.getContainer().off('click' + eventNamespace);
        };

        var resetEventListeners = function() {
            addEventListener.call(this);
            removeEventListeners.call(this);
        };

        return {
            constructor: function (element, options) {
                var p = options.p;
                options = $.extend({
                    isEnd: (p & 1) === 1,
                    isLeaf: (p & 2) === 2,
                    allowSelections: (p & 4) === 4,
                    fetchedChildren: (p & 8) === 8,
                    collapsed: (p & 16) === 16
                }, options);
                
                // Add base effects
                options.effects = $.extend({
                    // Default collapse implementation
                    gaiacollapsing: function(evt) {
                        this.getChildrenContainer().hide();
                        evt.memo.afterFinish.call(this);
                    },

                    // Default expand implementation
                    gaiaexpanding: function(evt) {
                        this.getChildrenContainer().show();
                        evt.memo.afterFinish.call(this);
                    }
                }, options.effects);

                Gaia.Extensions.TreeViewItem.$super.call(this, element, options);
                addEventListeners.call(this);
            },

            setSel: function(value) {
                this.options.allowSelections = value;
                resetEventListeners.call(this);
                return this;
            },

            setLeaf: function(value) {
                this.options.isLeaf = value;
                resetEventListeners.call(this);
                return this;
            },

            setCol: function(value) {
                var me = this;
                collapseQueue.queue(function(next) {
                    if (value && !me.options.collapsed) {
                        me.element.trigger({
                            type: "gaia:collapsing",
                            memo : {
                                afterFinish: $.proxy(next, me)
                            }
                        });
                    } else if (!value && me.options.collapsed) {
                        me.element.trigger({
                            type: "gaia:expanding",
                            memo : {
                                afterFinish: $.proxy(next, me)
                            }                            
                        });
                    }

                    me.options.collapsed = value;
                });

                return me;
            },

            setInd: function(value, index) {
                var me = this;
                var element = this.element.find('.span-for-indent');
                element.html(function(idx, html) {
                    return me.decodeArgument(html, value, index);
                });
                return me;
            },

            setCCnt: function(value, index) {
                var me = this;
                this.getContainer().attr('class', function(idx, cls) {
                    return me.decodeArgument(cls, value, index);
                });
                return me;
            },

            setCExp: function(value, index) {
                var me = this;
                this.getExpander().attr('class', function(idx, cls) {
                    return me.decodeArgument(cls, value, index);
                });
                return me;
            },

            reInit: function() {
                Gaia.Extensions.TreeViewItem.$superp.reInit.call(this);
                resetEventListeners.call(this);
            },

            appendHtml: function(value) {
                this.getChildrenContainer().append(value);
                return this;
            },

            getExpander: function() {
                return $("#" + this.element.attr('id') + '_expander');
            },

            getContainer: function() {
                return $("#" + this.element.attr('id') + '_container');
            },

            getChildrenContainer: function() {
                return $("#" + this.element.attr('id') + '_children');
            },

            destroy: function() {
                removeEventListeners.call(this);
                Gaia.Extensions.TreeViewItem.$superp.destroy.call(this);
            },

            _getElementPostValueEvent: function(evt) {
                var argument;
                var postValue = '&__EVENTTARGET=' + this.getCallbackName();

                if (evt.data && evt.data.nodeSelectEvent) {
                    argument = 'S';
                } else if (this.options.collapsed) {
                    argument = 'E';
                } else {
                    argument = 'C';
                }

                return postValue + '&__EVENTARGUMENT=' + argument;
            }
        };
    });
})(window, jQuery, jsface.Class);