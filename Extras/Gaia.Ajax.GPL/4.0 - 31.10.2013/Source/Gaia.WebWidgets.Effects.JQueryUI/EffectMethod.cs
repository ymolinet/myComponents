namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// JQuery effects can be fired using different methods. Their distinct usage is encapsulated in the various 
    /// effects, but can be overridden to create new variations beyond what is supported in the base construction. 
    /// </summary>
    public enum EffectMethod
    {
        Show,

        Hide,
        
        Effect,
        
        Toggle,

        Animate
    }
}