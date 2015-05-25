namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// “Shrinks” an element into a specific direction (see demo for better understanding), hides it when the effect is complete.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectShrink to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectShrink\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectShrink : EffectScale
    {
        public EffectShrink() : base(0)
        {
            Method = EffectMethod.Hide;
            ScaleFromCenter = true;
        }
    }
}