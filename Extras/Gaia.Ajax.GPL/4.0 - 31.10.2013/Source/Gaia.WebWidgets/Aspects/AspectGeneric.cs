/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Web.UI;
using System.Collections.Generic;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectGeneric.js", "text/javascript")]
namespace Gaia.WebWidgets
{
    /// <summary>
    /// Generic Aspect with Generic Events. This aspect is used to handle some scenarios
    /// the other aspects where not designed to handle but which we didn't feel justified creating
    /// explicit aspect for. E.g. "blur" and "focus" on widgets can be handled with this Aspect.
    /// </summary>
    public class AspectGeneric : Aspect<AspectGeneric>,  IAspect
    {
        #region [ -- Effect Events -- ]
        /// <summary>
        /// Add this EffectEvent to a control to add an effect to the blur event. 
        /// note: This EffectEvent doesn't require that you add the AspectGeneric to your control.
        /// </summary>
        public static AjaxEffectEvent EffectEventBlur { get { return AjaxEffectEventFactory.Create("blur"); } }

        /// <summary>
        /// Add this EffectEvent to a control to add an effect to the focus event. 
        /// note: This EffectEvent doesn't require that you add the AspectGeneric to your control.
        /// </summary>
        public static AjaxEffectEvent EffectEventFocus { get { return AjaxEffectEventFactory.Create("focus"); } }

        /// <summary>
        /// Add this EffectEvent to a control to add an effect to the select event. 
        /// note: This EffectEvent doesn't require that you add the AspectGeneric to your control.
        /// </summary>
        public static AjaxEffectEvent EffectEventSelect { get { return AjaxEffectEventFactory.Create("select"); } }

        #endregion

        #region Private Members

        #endregion

        #region Enums

        /// <summary>
        /// GenericEvent Enum, defines what type of event on the client-side you are interested in
        /// trapping.
        /// </summary>
        public enum GenericEvent
        {
            /// <summary>
            /// Triggered when: Image loading is interrupted
            /// Supported by: IMG
            /// </summary>
            Abort,

            /// <summary>
            /// Triggered when: Element looses input focus
            /// Supported by: BUTTON, INPUT, LABEL, SELECT, TEXTAREA
            /// </summary>
            Blur,

            /// <summary>
            /// Triggered when: Image loading fails
            /// Supported by: IMG
            /// </summary>
            Error,

            /// <summary>
            /// Triggered when: Element gains input focus
            /// Supported by: BUTTON, INPUT, LABEL, SELECT, TEXTAREA
            /// </summary>
            Focus,

            /// <summary>
            /// Triggered when: Text is selected
            /// Supported by: INPUT, TEXTAREA
            /// </summary>
            Select, 


            /// <summary>
            /// Set this to None to construct AspectGeneric so that it listens to the other events you specify directly instead
            /// </summary>
            None
        }

        #endregion

        /// <summary>
        /// EventRaised is called when the specified event is fired from the Client. 
        /// </summary>
        [Obsolete("Using the specific EventHandlers (Focus, Blur ...) is recommended instead")]
        public event EventHandler EventRaised;


        /// <summary>
        /// Event Raised when Focus occurs
        /// </summary>
        public event EventHandler Focus;

        /// <summary>
        /// Event Raised when Blur occurs
        /// </summary>
        public event EventHandler Blur;

        /// <summary>
        /// Event Raised when Select occurs
        /// </summary>
        public event EventHandler Select;

    
        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public AspectGeneric()
        {
            Event = GenericEvent.None;
        }

        /// <summary>
        /// Constructor. Specify what kind of event to listen for and the eventHandler for the Event.
        /// </summary>
        /// <param name="eventHandler">delegate to be called when event occurs</param>
        /// <param name="whatEvent">Which event to listen to on the client</param>
        [Obsolete("The EventRaised event is no longer recommended. Use the default/empty constructor instead")]
        public AspectGeneric(EventHandler eventHandler, GenericEvent whatEvent)
            :this(eventHandler, whatEvent, 0)
        { }


        /// <summary>
        /// Constructor. Specify what kind of event to listen for and the eventHandler for the Event.
        /// Also specify a delay interval before the event is actually forwarded to the server ...
        /// </summary>
        /// <param name="eventHandler">delegate to be called when event occurs</param>
        /// <param name="whatEvent">Which event to listen to on the client</param>
        /// <param name="interval">Interval before raising the event back to the server</param>
        [Obsolete("The EventRaised event is no longer recommended. Use the default/empty constructor instead")]
        public AspectGeneric(EventHandler eventHandler, 
            GenericEvent whatEvent, 
            int interval)
        {
            EventRaised += eventHandler;
            Event = whatEvent;
            Interval = interval;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Specify what kind of event to listen for. 
        /// </summary>
        [Obsolete("The EventRaised event is no longer recommended and the Event property is only used in conjunction with that Event. Please use the other events instead")]
        public GenericEvent Event { get; set; }

        /// <summary>
        /// Specify a delay before the event is fired.
        /// </summary>
        [Obsolete("The EventRaised event is no longer recommended and Interval is only used in conjunction with that Event. Please use the other events instead")]
        public int Interval { get; set; }

        [Method]
        internal void EventRaisedMethod()
        {
            if (EventRaised == null) return;
            EventRaised(GetSender(), EventArgs.Empty);
        }

        [Method]
        internal void EventFiredMethod(string evt)
        {
            // fire the event that was subscribed to
            if (evt == "focus" && Focus != null)
                Focus(GetSender(), EventArgs.Empty);
            else if (evt == "blur" && Blur != null)
                Blur(GetSender(), EventArgs.Empty);
            else if (evt == "select" && Select != null)
                Select(GetSender(), EventArgs.Empty);
          
        }

        #endregion

        #region [ -- IAspect Implementation -- ]

        string IAspect.GetScript()
        {
            return new RegisterAspect("Gaia.AspectGeneric", ParentControl.Control.ClientID)
                .AddProperty("enableBubbling", false)
                .AddProperty("raiseEvent", EventRaised != null && Event != GenericEvent.None)
                .AddProperty("eventName", GetValidEventName())
                .AddProperty("interval", Interval)
                .AddProperty("evts", GetJsEventArray(), false)
                .ToString();
        }

        private string GetJsEventArray()
        {
            var events = new List<string>(3);
            if (Focus != null) events.Add("'focus'");
            if (Blur != null) events.Add("'blur'");
            if (Select != null) events.Add("'select'");
           
            return string.Format("[{0}]", string.Join(",", events.ToArray()));

        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectGeneric.js", typeof(Manager), "Gaia.AspectGeneric.browserFinishedLoading", true);   
        }

        #endregion

        #region Helpers

        private string GetValidEventName()
        {
            switch (Event)
            {
                case GenericEvent.Abort:
                    return "abort";
                case GenericEvent.Blur:
                    return "blur";
                case GenericEvent.Error:
                    return "error";
                case GenericEvent.Focus:
                    return "focus";
                case GenericEvent.Select:
                    return "select";
                case GenericEvent.None:
                    return "";
                default:
                    throw new Exception("Unsupported EventName in AspectGeneric");
            }
        } 
        #endregion


    }
}
