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
    /// QueueDetails 
    /// </summary>
    public class ScriptaculousQueueDetails
    {
        /// <summary>
        /// Positioning in the EffectQueue
        /// </summary>
        public enum PositionEnum
        {
            /// <summary>
            /// Add the Effect in the front
            /// </summary>
            Front,

            /// <summary>
            /// Add the Effect in the end
            /// </summary>
            End
        }

        private string _scope;
        private PositionEnum _position = PositionEnum.End;
       
        /// <summary>
        /// A scope is grouping a set of Effects together in their own Queue
        /// </summary>
        public string Scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        /// <summary>
        /// Where to put the Effect in the queue
        /// </summary>
        public PositionEnum Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Creates the script that represents the options in the Queue
        /// </summary>
        /// <returns>The script</returns>
        public string GetScript()
        {
            return string.Format("{{ position: '{0}', scope: '{1}' }}", Position.ToString().ToLower(), Scope);
        }
    }
}