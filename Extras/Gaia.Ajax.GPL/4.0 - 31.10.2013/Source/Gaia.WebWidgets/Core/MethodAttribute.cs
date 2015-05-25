/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Attribute to mark a Method as valid for an Ajax method invocation.
    /// Without this attribute no method can be called from JavaScript on the client side. Used
    /// by Control Developers to create extension controls with support from calling methods from the server
    /// which maps back to methods on the client side.
    /// </summary>
    /// <example>
    /// <code title="Using the METHOD attribute on the Page (ASPX)" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Core\Manager\PageMethods\Default.aspx" />
    /// </code> 
    /// <code title="Using the METHOD attribute on the Page (Codebehind)" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Core\Manager\PageMethods\Default.aspx.cs" />
    /// </code> 
    /// </example>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MethodAttribute : Attribute
    {
    }
}
