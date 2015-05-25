namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// Denotes jQuery effect.
    /// </summary>
    public interface IJQueryEffect : IEffect
    {
        /// <summary>
        /// Gets or sets queuing options.
        /// </summary>
        JQueryQueueDetails Queue { get; set; }

        /// <summary>
        /// A client-side function to be called for each animated property of each animated element.
        /// This function provides an opportunity to modify the Tween object to change the value of the property before it is set.
        /// </summary>
        string AfterUpdate { get; set; }

        /// <summary>
        /// A client-side function to call once the animation is complete.
        /// Quite useful for restoring and keeping in sync the program.
        /// </summary>
        string AfterFinish { get; set; }
    }
}