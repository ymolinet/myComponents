/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// Denotes a ScriptaculousEffect
    /// </summary>
    public interface IScriptaculousEffect : IEffect
    {
        /// <summary>
        /// Sets whether the effect should render new frames automatically (which it does by default). 
        /// Set this to true in usage of EffectParallel or other scenarios where you want to control the 
        /// rendering of each frame yourself 
        /// </summary>
        bool Sync { get; set;}

        /// <summary>
        /// Sets queuing options. 
        /// </summary>
        ScriptaculousQueueDetails ScriptaculousQueue { get; set;}

        /// <summary>
        /// Client Function that will be invoked on each frame update of the Effect. 
        /// </summary>
        string AfterUpdate { get; set;}

        /// <summary>
        /// Client Function that will be invoked after the effect is complete. Quite useful for restoring and keeping in sync the program
        /// </summary>
        string AfterFinish { get; set; }
    }
}