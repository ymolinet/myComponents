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
    /// <para>Class that encapsulates initialization and destruction logic as a unit
    /// and deterministically executes them by exploiting the "using" or "IDisposable" pattern.</para>
    /// <para>This class is very useful for having code executes by guarantee at some specific point in
    /// your sequence. If you have two or three operations which are completely dependant upon
    /// the order of execution in addition to that you also need guarantees that all three things
    /// should execute then this class is just what you need.</para>
    /// </summary>
    /// <example>
    /// <code>
    /// int first = 0, second = 0, result = 0;
    /// using (new Gaia.WebWidgets.AtomicInvoker(
    ///    delegate
    ///     {
    ///        // Run initialization logic here
    ///        first = 5;
    ///    },
    ///    delegate
    ///    {
    ///        // run destruction logic here
    ///        result = first + second;
    ///    }))
    /// {
    ///    // run body here...
    ///    second = 3;
    /// }
    /// </code>
    /// </example>
    public class AtomicInvoker : IDisposable
    {
        /// <summary>
        /// Callback method type
        /// </summary>
        public delegate void Action();

        private bool _disposed;

        /// <summary>
        /// Deinitializer can be set in the derived classes
        /// </summary>
        protected Action Destructor { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initializer">Initialization delegate</param>
        /// <param name="destructor">Destructor delegate</param>
        public AtomicInvoker(Action initializer, Action destructor)
        {
            if (initializer != null)
                initializer();

            Destructor = destructor;
        }

        /// <summary>
        /// Constructor used only for inheritance.
        /// You don't need to provide initializer in that case,
        /// because you can do initialization in the constructor.
        /// Inheritors should only set the Deinitializer property.
        /// </summary>
        protected AtomicInvoker()
        { }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Basic implementation of IDisposable pattern
        /// </summary>
        /// <param name="disposing">If true and not already disposed, disposing will be performed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Dispose unmanaged resources
            _disposed = true;

            if (!disposing) return;

            // Dispose managed resources
            if (Destructor != null)
                Destructor();
        }
    }
}
