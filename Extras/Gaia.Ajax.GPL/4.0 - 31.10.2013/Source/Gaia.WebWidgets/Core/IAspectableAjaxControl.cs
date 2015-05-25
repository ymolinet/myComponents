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
    /// Implemented by Controls to which Aspects can be attached
    /// </summary>
    public interface IAspectableAjaxControl : IAjaxControl
    {
        
        /// <summary>
        /// The Aspect collection contains all the Aspects for this control. Aspects
        /// are functional units that extend the behavior of this control. It allows
        /// you to add new dynamic functionality to almost all Gaia controls. Aspects
        /// can also be created as separate extensions. There exists many core Aspects
        /// built-in like <see cref="AspectClickable"/> and <see cref="AspectDraggable"/>
        /// just to name a few. You can also make any control Modal, by adding 
        /// <see cref="AspectModal" />to the Aspects collection of that control. 
        /// </summary>
        /// <remarks>
        /// Not all aspects can be added to all controls. There's simply too many possible
        /// combinations to test everything. 
        /// </remarks>
        /// <example>
        /// <code title="Adding AspectClickable programmatically" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectClickable\Overview\Default.aspx.cs" region="Code" />
        /// </code>
        /// <code title="Adding AspectModal in CodeBehind" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectModal\Overview\Default.aspx.cs" />
        /// </code>
        /// </example>
        AspectCollection Aspects { get; }

        /// <summary>
        /// Retrieves the AspectableAjaxControl object associated with the Control
        /// </summary>
        AspectableAjaxControl AspectableAjaxControl { get; }
    }
}
