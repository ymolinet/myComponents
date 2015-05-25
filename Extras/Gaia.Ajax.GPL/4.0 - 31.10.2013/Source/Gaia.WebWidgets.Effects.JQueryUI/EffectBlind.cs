namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// Common ancestor for <see cref="EffectBlindUp"/> and <see cref="EffectBlindDown"/>
    /// </summary>
    public class EffectBlind : JQueryUIEffectBase
    {
        private BlindDirection _direction = BlindDirection.Vertical;

        protected override string EffectType
        {
            get { return "blind"; }
        }

        public enum BlindDirection
        {
            Horizontal, 

            Vertical
        }

        /// <summary>
        /// Which direction the Blind should go
        /// </summary>
        public BlindDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddPropertyIfTrue(Direction != BlindDirection.Vertical, "direction", "horizontal");
        }
    }
}