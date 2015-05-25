/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2013 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Effects
{
    using System;

    /// <summary>
    /// Queue details for JQuery effects.
    /// </summary>
    public sealed class JQueryQueueDetails : IEquatable<JQueryQueueDetails>
    {
        /// <summary>
        /// Default queue options.
        /// </summary>
        public static readonly JQueryQueueDetails Default = new JQueryQueueDetails(true);

        private readonly bool _useQueue;
        private readonly string _queueName;

        /// <summary>
        /// Initializes new instance of <see cref="JQueryQueueDetails"/> by specifying 
        /// if default queue should be used or not.
        /// </summary>
        /// <param name="useQueue">Denotes whether default queue should be used or not.</param>
        public JQueryQueueDetails(bool useQueue) : this(null, useQueue) { }

        /// <summary>
        /// Initializes new instance of <see cref="JQueryQueueDetails"/> by specifying 
        /// the name of the queue to use.
        /// </summary>
        /// <param name="queueName">Name of the queue to use.</param>
        public JQueryQueueDetails(string queueName) : this(queueName, true) { }

        private JQueryQueueDetails(string queueName, bool useQueue)
        {
            _useQueue = useQueue;
            _queueName = queueName;
        }

        /// <summary>
        /// Name of the queue to use.
        /// Can be null if default queue is used.
        /// </summary>
        public string QueueName
        {
            get { return _queueName; }
        }

        /// <summary>
        /// Returns whether queue should be used or not.
        /// </summary>
        public bool UseQueue
        {
            get { return _useQueue; }
        }

        public bool Equals(JQueryQueueDetails other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _useQueue.Equals(other._useQueue) && string.Equals(_queueName, other._queueName);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as JQueryQueueDetails);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_useQueue.GetHashCode()*397) ^ (_queueName != null ? _queueName.GetHashCode() : 0);
            }
        }

        public static bool operator ==(JQueryQueueDetails left, JQueryQueueDetails right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(JQueryQueueDetails left, JQueryQueueDetails right)
        {
            return !Equals(left, right);
        }
    }
}