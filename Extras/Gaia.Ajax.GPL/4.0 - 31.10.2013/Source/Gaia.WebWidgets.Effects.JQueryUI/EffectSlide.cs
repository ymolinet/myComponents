namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// Common ancestor for <see cref="EffectSlideDown"/> and <see cref="EffectSlideUp"/>
    /// </summary>
    public class EffectSlide : JQueryUIEffectBase
    {
        private SlideDirection _direction = SlideDirection.Left;

        public enum SlideDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        public SlideDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect.AddPropertyIfTrue(Direction != SlideDirection.Left, "direction", Direction.ToString().ToLowerInvariant()));
        }

        protected override string EffectType
        {
            get { return "slide"; }
        }
    }
}