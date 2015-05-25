namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// Explodes or implodes the element into/from many pieces. 
    /// </summary>
    public class EffectExplode : JQueryUIEffectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EffectExplode() : this(DefaultDuration) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public EffectExplode(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public EffectExplode(decimal duration, decimal delay)
        {
            Duration = duration;
            Delay = delay;
        }

        /// <summary>
        /// The name of the effect implemented in jQuery
        /// </summary>
        protected override string EffectType
        {
            get { return "explode"; }
        }
    }
}