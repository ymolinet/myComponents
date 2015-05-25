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
    // Main namespace for everything Gaia related.
    var Gaia = window.Gaia = window.Gaia || {
        load: function (registrationCallback) {
            var decl = registrationCallback.toString();
            var body = decl.substring(decl.indexOf('{') + 1, decl.lastIndexOf('}'));
            evalServerCallback($.trim(body));
        }
    };
    
    // Evaluate server response.
    // Expected format for the response is {js:js_file_array, css:css_file_array, rv:return_value};;;;scripts, 
    // where the part before ';;;;' delimiter is optional with optional components
    var evalServerCallback = function (script, callback) {
        var returnValue;

        var executionQueue = $({}); // we use to queue to make sure that script files are loaded before the scripts are evaluated.
        var parts = script.split(';;;;', 2);

        if (parts.length === 2) {
            var inclusions = $.parseJSON(parts.shift());

            // start with css, because we can't guarantee that stylesheets are loaded, so at least we can buy some time
            if (inclusions.css) {
                var supportsCreateStyleSheet = $.isFunction(document.createStyleSheet);
                $.each(inclusions.css, function (idx, css) {
                    if (supportsCreateStyleSheet) {
                        document.createStyleSheet(css);
                    } else {
                        $('head').append($("<link rel='stylesheet' type='text/css' href='" + css + "'/>"));
                    }
                });
            }

            if (inclusions.js) {
                $.each(inclusions.js, function (idx, js) {
                    executionQueue.queue(function (next) { $.getScript(js).then(next, next); });
                });
            }

            if (inclusions.rv) {
                executionQueue.queue(function (next) {
                    returnValue = inclusions.rv;
                    next();
                });
            }
        }

        var evalContextDeclPre = '(function($G, $FCg, $RC, $$) {';
        var evalContextDeclPost = '})(Gaia.Control.get, Gaia.Control.changeField, Gaia.Control.registerControl, Gaia.Control);';

        executionQueue.queue(function (next) {
            $.each(parts, function (idx, code) {
                $.globalEval(evalContextDeclPre + code + evalContextDeclPost);
            });
            next();
        });

        executionQueue.queue(function () {
            (callback || $.noop)(returnValue);
        });
    };

    Gaia.Control = Class(function() {

        var effectEventsNamespace = ".gaia_effects";
        var subscribedEventsNamespace = ".gaia_subscriptions";

        // This is a reference to the currently used UpdateControl!
        // Note that this only works for the _FIRST_ Ajax request meaning if you don't set 
        // it in e.g. the Page_Load method for _EVERY_ call it will only be visible once...
        var updateControl = null;

        // This is the list (hash) of controls registered on the page
        var registeredControls = {};

        // We need to use queue for ajax requests to make sure client and server states are synchronized
        var requestQueue = $({});
        var isRequestRunning = false; // Denotes whether there is a running request to the server

        var $el = function(id) {
            return $('#' + id);
        };

        var getContainer = function(id) {
            return id ? $el(id) : $(document.body);
        };

        var raiseEvent = function(element, evtName) {
            $el(element).trigger(evtName);
        };

        var handleMove = function (source, wasMoved, action) {
            var content = source;
            if (wasMoved) {
                content = $el(source);
                raiseEvent(source, "gaia:moving");
            }

            action(content);

            if (wasMoved) {
                raiseEvent(source, "gaia:moved");
            }
        };
        
        // Retrieves an input field, if it doesn't exists it creates a new hidden field and returns that one
        var ensureField = function(field) {
            var el = $el(field);
            if (el.length === 0) {
                el = $('<input type="hidden" />').attr({ id: field, name: field }).appendTo($('form'));
            }

            return el;
        };

        // Default error handler which may be overridden by the user
        var errorHandlerFrame;
        var errorHandler = function(status, statusText, responseText) {
            var message = 'An error occurred. The details are:\n Status: ' + status + ',\n Message: "' + statusText;
            if (responseText && responseText.length > 0) {
                if (!confirm(message + '"\n\nDo you wish to see additional details?')) {
                    return;
                }

                errorHandlerFrame = $('<iframe width="100%" height="100%" style="top:0;left:0;z-index:10000;position:absolute">').
                    appendTo($(document.body)).get(0);

                var contentDocument = errorHandlerFrame.contentWindow.document;
                contentDocument.open();
                contentDocument.write(responseText);
                contentDocument.write('<div onclick="window.parent.Gaia.Control._closeErrorHandler();"' +
                    'style="cursor:pointer;z-index:10001;right:5px;top:5px;width:100px;height:40px;border:Solid 3px Black;background-color:Red;position:absolute;padding-top:15px;text-align:center;font-size:18px;">' +
                    'Close</div>');
                contentDocument.close();
            } else {
                window.alert(message);
            }
        };

        // Serializes form's input controls taking into account gaia controls as well.
        var serializeForm = function (eventControl, evt) {
            // Get all input controls contained in gaia control (not container ones)
            var containedFormControls = $.map(registeredControls, function (control) {
                if (control.isAjaxContainer()) return null;

                var elem = control.element;
                var result = control.element.find(":input");
                if (elem.is(":input")) {
                    result = result.add(elem);
                }

                return result.get();
            });

            // __EVENTTARGET and __EVENTARGUMENT are serialized from the specific Gaia Widgets
            // and if not, it's another type of postback meaning we won't be in this method in the first place.
            // __LASTFOCUS makes only sense in a postback scenarios.
            var rAspNetFields = /^(?:__EVENTTARGET|__EVENTARGUMENT|__LASTFOCUS)/i;

            var theAspNetForm = document.theForm || $('form').get(0);
            var data = $(theAspNetForm).find(':input')
                .not(function () { return rAspNetFields.test(this.name); })
                .not(containedFormControls)
                .serialize();

            // Now process gaia controls
            $.each(registeredControls, function (id, control) {
                data += eventControl === control ?
                    control._getElementPostValueEvent(evt) :
                    control._getElementPostValue();
            });

            return data;
        };

        var disablers = {
            local: function() { // Disabler for local update controls
                var result;

                if (this.options.aspects) {
                    $.each(this.options.aspects, function(idx, aspect) {
                        if ($.isFunction(aspect.startAjaxRequest) && $.isFunction(aspect.endAjaxRequest) && aspect.startAjaxRequest()) {
                            result = function() { return aspect.endAjaxRequest(); };
                            return false;
                        }
                        return undefined;
                    });
                }

                return result;
            },

            global: function() { // Disabler for global update control
                var elem = $el(updateControl);
                if (elem.length > 0) {
                    elem.show();
                    return function() { elem.hide(); };
                }
                
                return undefined;
            },

            instance: function(skipDisable) { // Disabler for the control itself
                if (!skipDisable) {
                    this.storeEnabled();
                    this.setEnabled(false);

                    var self = this;
                    return function() { self.restoreEnabled(); };
                }
                
                return undefined;
            }
        };

        var onSuccess = function(callback, data) {
            if (!data) return;

            try {
                evalServerCallback(data, callback);
            } catch (e) {
                $.error('Server response evaluation failure:\n ' + e.message);
            }
        };

        var onError = function (jqXhr, textStatus) {
            errorHandler(
                jqXhr.status || -1,
                jqXhr.statusText || textStatus || 'CONNECTION_PROBLEM',
                jqXhr.responseText || 'Unknown failure');
        };

        // Context of this function should be control to call server for.
        var createRequestInstance = function(evt, skipDisable, next) {
            // short-circuit in cases where we have stacked requests where the previous one destroyed the widget or disabled it
            if (!this.element || !$.contains(document.documentElement, this.element[0]) || !this.isEnabled()) {
                next();
                return;
            }

            var params = serializeForm(this, evt);
            var disableHandlers = [disablers.local, disablers.global, disablers.instance];
            createRequest.call(this, this.options.url, undefined, params, skipDisable, disableHandlers, next);
        };
        
        // There won't be dependency on the context of this function when making the callback.
        var createRequestStatic = function (methodAfter, extraParams, skipDisable, next) {
            // Adding up the extra parameters like for instance parameters to Page Methods.
            var params = '&gaiaParams=' + encodeURIComponent(extraParams) + '&' + serializeForm(undefined, undefined);
            createRequest.call(window, Gaia.Control._defaultUrl, methodAfter, params, skipDisable, [disablers.global], next);
        };
        
        // Context of this function should be control to call server for.
        var createRequestInstanceNoEvent = function(methodAfter, extraParams, skipDisable, next) {
            // short-circuit in cases where we have stacked requests where the previous one destroyed the widget or disabled it
            if (!this.element || !$.contains(document.documentElement, this.element[0]) || !this.isEnabled()) {
                next();
                return;
            }
            
            var disableHandlers = [disablers.local, disablers.global, disablers.instance];
            
            // Adding up the extra parameters like for instance parameters to Control and Aspect Methods.
            var params = '&gaiaParams=' + encodeURIComponent(extraParams) + '&' + serializeForm(undefined, undefined);
            createRequest.call(this, this.options.url, methodAfter, params, skipDisable, disableHandlers, next);
        };

        // Creates the request and hits the server setting the Ajax engine into "busy" mode 
        // (meaning no other request can be created before the current request returns)
        var createRequest = function(
            url, /* Request url */
            methodAfter, /* Callback to execute afterwards */
            params, /* POST parameters to pass */
            skipDisable, /* True if we should skip disabling the event control */
            disableHandlers, /* Array of functions which disable event context and return corresponding enabler */
            next /* Next request handler in the request queue */) {

            var enabler;
            var self = this;
            $.each(disableHandlers, function(idx, disabler) {
                enabler = disabler.call(self, skipDisable);
                return !enabler; // continue while not disabled
            });

            var data = 'GaiaCallback=true&' + params;

            isRequestRunning = true;
            var stateToggler = function () { isRequestRunning = false; };

            var onSuccessComplete = function(returnValue) {
                (methodAfter || $.noop)(returnValue);
                stateToggler();
                next();
            };

            $.ajax({
                url: url,
                type: 'POST',
                data: data,
                dataType: 'text',
                context: this
            }).success(enabler).success($.proxy(onSuccess, this, onSuccessComplete))
                .fail(enabler).fail(onError).fail(stateToggler).fail(next);
        };
        
        // Calls server-side method, either a Page method or a control method
        var callServerSideMethod = function (requestFunction, method, params, onFinished, methodType, skipDisable) {
            var data = [method, methodType];
            if (params) {
                $.merge(data, $.map(params, function(param) {
                    return typeof param === 'string' ? param.replace(/,/g, '|$|') : param.toString();
                }));
            }
            
            // Creating an queuing actual ajax request
            requestQueue.queue($.proxy(requestFunction, this, onFinished, data.join(','), skipDisable));
        };
        
        return {
            constructor: function (element, options) {

                // Finding the element in the DOM and caching it to the this.element member
                // todo: here we need to do some kind of assertion that the root actually exists
                this.element = $el(element);

                // Setting the options
                this.options = $.extend({ url: Gaia.Control._defaultUrl }, options);

                // If control has set focus we make sure it really does...
                if (this.options.hasSetFocus) {
                    this.setFocus();
                }

                // If control has effects attached to it we wire them up to their events here ...
                if (this.options.effects) {
                    var handler = $.proxy(this._addEffectEvent, this);
                    $.each(this.options.effects, handler);
                }
            },
            
            isAjaxContainer: function () {
                return false;
            },
            
            setEnabled: $.noop,
            storeEnabled: $.noop,
            restoreEnabled: $.noop,
            isEnabled: function () { return true; },
            
            sepID: function (suffix, index) {
                return this.setID(this.element.attr('id').substring(0, index) + suffix);
            },
            
            // The setID function will loop through it's children, skipping Gaia elements and change the IDs based 
            // on their new designated id. 
            setID: function (id) {

                var me = this;
                var element = this.element;
                var currentId = element.attr("id");

                this.rename(currentId, id);

                this.eachNonGaiaChild(function (node) {
                    var nodeId = node.id;
                    if (nodeId && nodeId.indexOf(currentId) === 0) {
                        me.rename(nodeId, nodeId.replace(currentId, id), -1, true);
                    }
                });

                return this;
            },
            
            // Sets the focus to the control
            setFocus: function () {
                // This looks REALLY stupid but is needed for situations where widget
                // is inside of a Window since the window will be rendered with "display:none" 
                // by default which will prevent the focus() method from working...
                setTimeout($.proxy(this.setElementFocus, this), 500);
                return this;
            },

            setElementFocus: function () {
                this.element.focus();
            },

            setContent: function (value) {
                this.element.contents().remove().end().append(value);
                return this;
            },
            
            eachNonGaiaChild: function (action) {
                var children = [this.element];
                var getNonGaiaChildren = function (child) {
                    return $(child).children().not(function() {
                        return Gaia.Control.isGaiaControlRootNode(this.id);
                    }).get();
                };

                var onNonGaiaChild = function (idx, child) { action(child); };

                while (children.length > 0) {
                    children = $.map(children, getNonGaiaChildren);
                    $.each(children, onNonGaiaChild);
                }
            },
            
            // Returns the "callback name" for the widget used by ASP.NET to map the widget to server side postback control
            getCallbackName: function () {

                // Checking to see if we've got it in the cache first
                if (this.options.callbackName && this.options.callbackName.length > 0)
                    return this.options.callbackName;

                // Then checking against the "name" attribute in the ELEMENT of the widget
                var name = this.element.attr('name');
                if (name && name.length > 0) {
                    return this.options.callbackName = name;
                }

                // Then resorting to "last resort" by constructing the callbackName by 
                // taking the ID of the element and replacing every occurency of an underscore "_" with a "$" sign...
                // This method is REALLY not good and all widgets that doesn't have the name attribute should
                // override the object initialization script ("registerControl") with the Name attribute 
                // for the widget since in containers and similar having underscores as part
                // of their ID this logic will fail...!!!!!!
                return this.options.callbackName = this.element.attr('id').replace(/_/g, '$');
            },
            
            // Use to observe specific events and call server when they occur
            // If you observe an event through this method the server WILL be called when that event occurs
            // and if there's a match on the server for such an event that event will be called
            // Used to map the events inherited from ASP.NET WebControl controls like Button.Click event etc...
            // Use Gaia.Control.callControlMethod if you're implementing CUSTOM events and such...
            observe: function (evtName, bubbleUp) {
                this._observeImpl(evtName, !!bubbleUp);
                return this;
            },
            
            // Implementation of "observe"...!
            _observeImpl: function (evtName, bubbleUp) {
                var handler = function (evt) { return this._onEvent(evt, evtName, bubbleUp); };
                this.element.on(evtName + subscribedEventsNamespace, $.proxy(handler, this));
            },
            
            _onEvent: function (evt, evtName, bubbleUp) {
                // modified 04-09-2007 by Matthew M.
                // -- added updated alert message if this section errors out
                // suggestion - all events should have a 'fall-through' to normal web function (non-ajax) if 
                // this routine fails.

                // Raising event
                try {
                    this._onEventImpl(evt, evtName);
                } catch (err) {
                    // More informative error report
                    alert('Gaia Event Handling Error:\n\nError Message:\n\n' + err);
                }

                // Preventing event to bubble upwards!
                // But only if we SHOULD prevent it...
                if (!bubbleUp) {
                    evt.preventDefault();
                    evt.stopPropagation();
                }
            },
            
            // Private implementation of the observe event handler
            // This is the one doing the actually "ajax magic" and calls our server when
            // a WebControl "native" event occurs.
            // This is the one subscribing to whatever JavaScript DOM event that triggers the event
            // like for instance the JavaScript "click" event for the input type="button" event and calls the
            // server.
            _onEventImpl: function (evt, evtName, skipDisable) {

                // check if event for cancelled
                if (evt && (typeof evt.cancelAjaxRequest !== 'undefined') && evt.cancelAjaxRequest) return;

                // if the element is disabled, do not fire any event
                if (!this.isEnabled()) return;

                // Checking to see if the page "validates" using ASP.NET routines
                if (this.options && this.options.validate &&
                    (($.isFunction(window.Page_ClientValidate) && !window.Page_ClientValidate(this.options.validationGroup)) ||
                     ($.isFunction(window.WebForm_OnSubmit) && !window.WebForm_OnSubmit()))) return;

                // Code to circumvent flaw in Sharepoint 2007
                // This is a BUG in SharePoint, but what can we do.... ;)
                if (typeof window._spFormOnSubmitCalled !== "undefined") {
                    window._spFormOnSubmitCalled = false;
                }

                // Creating an queuing actual ajax request
                requestQueue.queue($.proxy(createRequestInstance, this, evt, skipDisable));
            },

            stopObserve: function (evtName) {
                this.element.off(evtName + subscribedEventsNamespace);
                return this;
            },

            // todo: this shouldn't be here, needs to be refactored
            _setAutoPostBack: function (elements, eventName, eventNameSpace, value) {
                var me = this;
                if (value != me.isAutoPostBack) {
                    me.isAutoPostBack = !!value;
                    if (me.isAutoPostBack) {
                        elements.on(eventName + eventNameSpace, function (evt) {
                            me._onEvent(evt, eventName, true /*bubble*/);
                        });
                    } else {
                        elements.off(eventName + eventNameSpace);
                    }
                }
                return me;
            },
            
            eachGaiaChild: function (action) {
                // first check to see if there are any DOM elements available
                if (!this.element.is(":empty")) {
                    var container = this.element.get(0);
                    $.each(registeredControls, function (id, control) {
                        if ($.contains(container, control.element.get(0))) {
                            action(control, id);
                        }
                    });
                }
            },
            
            // Shows or hides the wrapped element
            // Note that normal elements are just being set the visibility property for
            // while container widgets are physically being destroyed...
            // (Which is overridden in derived classes)
            setVisible: function (value) {
                if (!!value) {
                    this.element.show();
                } else {
                    this.element.hide();
                    // Making sure control and child controls is destroyed...
                    this.destroy();
                }

                return this;
            },
            
            // Function that allows you to add effect events to widgets. The key is the event signature without the : and if it's a gaia event
            // it will be prefixed with gaia. normal events follow the prototype naming convention. the next thing is the function that will be bound
            // to the control instance. 
            _addEffectEvent: function (key, fxFunc) {
                var evtName = key.indexOf("gaia") === 0 ? "gaia:" + key.substring(4) : key;

                var control = this;
                var func = function(e) {
                    // We don't support bubbling of effect events so here we check if this is the element from which the event originated
                    if (e.target.id !== control.element.attr('id'))
                        return;

                    fxFunc.call(control, e);
                    e.preventDefault();
                    e.stopPropagation();
                };
                
                this.element.on(evtName + effectEventsNamespace, func);
            },
            
            // MUST override function!
            // Called when _ANY_ control creates a request and wants to call back
            // to the server. This is the "value" property of the widget (if any)
            // Just return '' in your overridden function if there are no value for it!
            _getElementPostValue: function () {
                return '';
            },

            _toPostPair: function (key, value) {
                return '&' + key + '=' + encodeURIComponent(value);
            },

            // This is the method called when the widget is ITSELF raising an event which goes to the server
            // and THIS is the value of the Widget in ADDITION to potentially "meta data" to indicate that
            // THIS widget raised the event!
            _getElementPostValueEvent: function () {
                return '';
            },
            
            // Destroys the widget
            // Override and call "_destroyImp()" to make sure you run the Control.destroy implementation if
            // you override this method...
            destroy: function () {
                this._destroyImpl();
            },

            _destroyImpl: function () {
                // Stopping effect event observers  
                this.element.off(effectEventsNamespace);

                // Stopping event observers
                this.element.off(subscribedEventsNamespace);

                // Destroying all of its ASPECTS...!
                if (this.options.aspects) {
                    $.each(this.options.aspects, function(idx, aspect) {
                        aspect.destroy();
                    });
                }

                // Removing control from Control Collection....!
                delete registeredControls[this.element.attr('id')];

                // Create a placeholder and remove from DOM tree
                $(this.element).replaceWith($('<' + this.element.prop('tagName') + ' />',
                    { id: this.element.attr('id') }).attr("class", Gaia.Control.CssClassHiddenControl).hide());

                this.element = undefined;
            },
            
            $statics: {
                // The default URL to postback to when Ajax request initiated, note that this CAN be 
                // overridden in widget level...
                _defaultUrl: null,

                ZIndexThreshold: NaN,

                CssClassHiddenControl: 'plh__gaia',

                setUpdateControl: function(el) {
                    updateControl = el;
                    $el(el).hide();
                },
                
                get: function (id) {
                    /// <summary>Retrieves Gaia.Control by the specified <paramref name="id"/>.</summary>
                    /// <returns type="Gaia.Control" mayBeNull="true">Gaia Ajax Control or null if not available.</returns>
                    return registeredControls[id] || null;
                },

                replace: function(id, html, moved) {
                    var control = $G(id);
                    if (control !== null) {
                        control.destroy();
                    }
                    
                    handleMove(html, moved, function(content) {
                        $el(id).replaceWith(content);
                    });
                },

                insertEnd: function(id, html, moved) {
                    handleMove(html, moved, function (content) {
                        getContainer(id).append(content);
                    });
                },

                insertTop: function(id, html, moved) {
                    handleMove(html, moved, function (content) {
                        getContainer(id).prepend(content);
                    });
                },

                insertBefore: function(id, html, moved) {
                    handleMove(html, moved, function (content) {
                        $el(id).before(content);
                    });
                },

                insertAfter: function(id, html, moved) {
                    handleMove(html, moved, function (content) {
                        $el(id).after(content);
                    });
                },

                destroyRemove: function () {
                    $.each(arguments, function(idx, value) {
                        Gaia.Control.destroyRemoveControl(value);
                    });
                },

                destroyRemoveControl: function(id) {
                    var control = $G(id);

                    if (control) {
                        control.destroy();
                    }

                    $el(id).remove();
                },

                // Static method responsible for registering a newly created control
                registerControl: function (control) {
                    return registeredControls[control.element.attr('id')] = control;
                },

                isGaiaControlRootNode: function (id) {
                    return $G(id) !== null || $el(id).hasClass(Gaia.Control.CssClassHiddenControl);
                },

                decodeArgument: function(initial, current, index) {
                    if (!$.isNumeric(index) || index === -1)
                        return current;

                    return initial.substring(0, index) + current;
                },

                tryRename: function(id) {
                    var control = $G(id);
                    if (control !== null) {
                        control.sepID("---", id.length);
                    } else if (Gaia.Control.isGaiaControlRootNode(id)) {
                        this.rename(id, "---", id.length);
                    }
                },

                rename: function(initial, current, index, skipLookup) {
                    var control = null;
                    var element = null;
                    var options = null;
                    var aspects = null;

                    var id = this.decodeArgument(initial, current, index);

                    if (!skipLookup) {
                        control = $G(initial);
                        if (control !== null) {
                            aspects = [];
                            options = control.options;
                            element = control.element;
                            raiseEvent(initial, 'gaia:renaming');

                            if (options && $.isArray(options.aspects))
                                aspects = options.aspects;

                            $.each(aspects, function(idx, aspect) {
                                if (!!aspect.onWrapperControlRenaming)
                                    aspect.onWrapperControlRenaming();
                            });

                            delete registeredControls[initial];
                            registeredControls[id] = control;
                        }
                    }

                    if (!element)
                        element = $el(initial);

                    element.attr('id', id);

                    var initialName = initial.replace(/_/g, '$');
                    var currentName = id.replace(/_/g, '$');

                    if (control && options && options.callbackName && options.callbackName === initialName)
                        control.options.callbackName = currentName;

                    var name = element.attr('name');
                    if (name && name === initialName)
                        element.attr('name', currentName);

                    // update references to this element in the associated aspects
                    if (control === null) return;

                    $.each(aspects, function(idx, aspect) { aspect.setWrappedControlID(id); });
                    raiseEvent(id, 'gaia:renamed');
                },
                
                executeScript: function (code) {
                    $.globalEval(code);
                },

                registerArray: function(hashCode, arrayName) {
                    Gaia[hashCode] = arrayName;
                },

                destroyArray: function(hashCode) {
                    var arrayName = Gaia[hashCode];

                    Gaia[arrayName] = undefined;
                    window[arrayName] = undefined;
                },

                setErrorHandler: function(handler) {
                    errorHandler = handler;
                },

                // Called when the error handler IFrame should be closed
                _closeErrorHandler: function() {
                    $(errorHandlerFrame).remove();
                    errorHandlerFrame = undefined;
                },

                // Calls a method on the ASP.NET Page.
                // Takes the name of the method as a string, the params as an iterateable collection and the callback to
                // be called when the server returns.
                // The callback will be passed the return value from the server-side method.
                callPageMethod: function(method, params, onFinished) {
                    callServerSideMethod.call(window, createRequestStatic, method, params, onFinished, 'PageMethod');
                },

                // Call a method on a Control.
                callControlMethod: function(method, params, onFinished, passId, skipDisable) {
                    callServerSideMethod.call(this, createRequestInstanceNoEvent, method, params, onFinished,
                        'ControlMethod,' + (passId || this.element.attr('id')), skipDisable);
                },

                // Calls a method on an Aspect.
                callAspectMethod: function(method, params, onFinished, passId) {
                    callServerSideMethod.call(this, createRequestInstanceNoEvent, method, params, onFinished,
                        'AspectMethod,' + (passId || this.element.attr('id')), true);
                },
                
                // Used for incrementally updating hidden fields.
                // Takes the new value together with an offset of from where to start "replacing" the current value of the field.
                changeField: function(field, value, index) {
                    var element = ensureField(field);
                    var fieldValue = Gaia.Control.decodeArgument(element.val(), value, index);

                    if (fieldValue === undefined)
                        Element.remove(element);
                    else
                        element.val(fieldValue);
                },
                
                isEngineIdle: function() {
                    return requestQueue.queue().length === 0 && !isRequestRunning;
                },
                
                getAll: function() {
                    return $.map(registeredControls, function(control) { return control; });
                }
            }
        };
        

    });
    
    // Shorthand notations
    var $G = Gaia.Control.get;
    (function($$) {
        $$.SID = $$.rename;
        $$.CRP = $$.replace;
        $$.IT = $$.insertTop;
        $$.IE = $$.insertEnd;
        $$.IB = $$.insertBefore;
        $$.IA = $$.insertAfter;
        $$.DR = $$.destroyRemove;
        $$.TR = $$.tryRename;
        $$.ES = $$.executeScript;
        $$.RA = $$.registerArray;
        $$.DA = $$.destroyArray;
        $$.RC = $$.registerControl;
        $$.SU = $$.setUpdateControl;
        $$.SE = $$.setErrorHandler;
    })(Gaia.Control);
})(window, jQuery.noConflict(), jsface.Class);
