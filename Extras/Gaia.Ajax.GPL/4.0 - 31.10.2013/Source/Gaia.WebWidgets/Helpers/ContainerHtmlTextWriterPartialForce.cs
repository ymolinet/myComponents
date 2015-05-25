/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.IO;
using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    internal class ContainerHtmlTextWriterPartialForce : ContainerHtmlTextWriterBase
    {
        private readonly Stack<bool> _stack = new Stack<bool>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerHtmlTextWriterPartialForce"/> class that uses a default tab string.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.IO.TextWriter"/> instance that renders the markup content. 
        /// </param>
        public ContainerHtmlTextWriterPartialForce(TextWriter writer) : base(writer) { }

        public bool Peek()
        {
            return _stack.Peek();
        }

        public void Push()
        {
            _stack.Push(true);
        }

        public void Pop()
        {
            _stack.Pop();
        }
    }
}
