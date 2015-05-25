using System;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Base class for event argument classes, which can be cancelled.
    /// </summary>
    public abstract class CancellableEventArgs : EventArgs
    {
        /// <summary>
        /// True if the event should be cancelled. Otherwise, false.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
