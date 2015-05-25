/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets
{
    /// <summary>
    /// This class implements IAspect and can be used as the base class for creating your own Aspects. If you want to be able to 
    /// bind your aspects to members (ie. properties, events) you need to inherit from this class so that the sender argument get's
    /// correct. 
    /// </summary>
    public abstract class Aspect: IAspect 
    {
        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected virtual void IncludeScriptFiles()
        {
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Aspect.js", typeof(Manager), "Gaia.Aspect.browserFinishedLoading", true);
        }

        #region [ -- IAspect Implementation -- ]


        string IAspect.GetScript()
        {
            return string.Empty;
        }

        /// <summary>
        /// See <see cref="IAspect.ParentControl" /> for documentation
        /// </summary>
        public IAspectableAjaxControl ParentControl { get; set; }

        /// <summary>
        /// Marked as internal as it is only used to detect wheter the aspect instance is memberbound or not. Is only using within GetSender()
        /// </summary>
        internal AspectCollection Owner { get; set; }

        void IAspect.IncludeScriptFiles()
        {
            IncludeScriptFiles();

        }

        /// <summary>
        /// Returns the actual sender of the event raised by the aspect.
        /// Used by derived classes to provide the correct sender in aspect binding scenarios.
        /// </summary>
        /// <returns>Actual sender of the event(s) raised by the aspect</returns>
        protected object GetSender()
        {
            return Owner != null && Owner.IsBound(this) ? (object) ParentControl : this;
        }

        #endregion
    }

    /// <summary>
    /// Contains equality operators to avoid duplicate aspects. Inherit from this class if you don't plan to add your
    /// own equality checks. 
    /// </summary>
    /// <typeparam name="T">Aspect type.</typeparam>
    public abstract class Aspect<T> : Aspect where T : class
    {
        /// <summary>
        /// Determines whether the specified <see cref="Aspect"/> is equal to the current.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="Aspect"/> is equal to the current object; otherwise, false.
        /// </returns>
        public bool Equals(Aspect other)
        {
            return other != null && other is T;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return ReferenceEquals(this, obj) || Equals(obj as Aspect);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }

}
