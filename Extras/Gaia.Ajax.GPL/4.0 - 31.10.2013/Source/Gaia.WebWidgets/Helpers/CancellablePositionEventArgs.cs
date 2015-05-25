using System.Drawing;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Base class for event argument classes, which denote position and can be cancelled.
    /// </summary>
    public abstract class CancellablePositionEventArgs : CancellableEventArgs
    {
        private readonly Point _position;

        /// <summary>
        /// Position
        /// </summary>
        public Point Position
        {
            get { return _position; }
        }

        /// <summary>
        /// Constructs object using specified position.
        /// </summary>
        /// <param name="position">Position for the event argument.</param>
        protected CancellablePositionEventArgs(Point position)
        {
            _position = position;
        }
    }
}
