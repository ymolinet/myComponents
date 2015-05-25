/*******************************************************************
* Gaia Ajax - Ajax Control Library for ASP.NET  
* Copyright (C) 2008 - 2011 Gaiaware AS
* All rights reserved. 
* This program is distributed under either GPL version 3 
* as published by the Free Software Foundation or the
* Gaia Commercial License version 1 as published by Gaiaware AS
* read the details at http://gaiaware.net/product/dual-licensing 
******************************************************************/

using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    using System.ComponentModel;
    using System.Reflection;
    using System;
    using System.Web.UI;

    [ToolboxItem(false)]
    public partial class Effect
    {
        /// <summary>
        /// The type of effect to execute
        /// </summary>
        [Obsolete]
        public enum TypeOfEffect
        {
            /// <summary>
            /// Highlights or "flash" the control
            /// </summary>
            Highlight,
            /// <summary>
            /// Makes the control "fade into" visibility, control should be INVISIBLE first
            /// </summary>
            Appear,
            /// <summary>
            /// Fades control "out of visibility", contro should be visible
            /// </summary>
            Fade,
            /// <summary>
            /// Makes the control "explode and fade" out of visibility
            /// </summary>
            Puff,
            /// <summary>
            /// Makes the control "roll up" into invisibility
            /// </summary>
            BlindUp,
            /// <summary>
            /// Makes the control "roll down" into VISIbility
            /// </summary>
            BlindDown,
            /// <summary>
            /// Makes the control "collapse" into invisibility
            /// </summary>
            SwitchOff,
            /// <summary>
            /// Makes the control "fall down" on the page at the same time it's being faded
            /// </summary>
            DropOut,
            /// <summary>
            /// "Shakes" the element to the right and left a bit, doesn't modify visibility
            /// </summary>
            Shake,
            /// <summary>
            /// Makes the element shrink into a corner before disappearing
            /// </summary>
            Squish,
            /// <summary>
            /// Makes control grow from a point and into visibility, control should be INvisile first
            /// </summary>
            Grow,
            /// <summary>
            /// Makes control vanish into a point, opposite of Grow
            /// </summary>
            Shrink,
            /// <summary>
            /// Makes control pulse from and to visibility and vice versa, control should be visible first, control becomes INvisible
            /// </summary>
            Pulsate,
            /// <summary>
            /// Makes control first roll up and then fold to the right, makes control IN visible, control should be visible first
            /// </summary>
            Fold,

            /// <summary>
            /// This effect simulates a window blind, where the contents of the affected elements scroll up.
            /// </summary>
            SlideUp,

            /// <summary>
            /// This effect simulates a window blind, where the contents of the affected elements scroll down.
            /// </summary>
            SlideDown
        };
       

        #region [ -- Private Members -- ]

        private static Assembly _assembly;
        private readonly int _duration = 1000;
        private readonly string _htmlElementToUpdate;
        private readonly Control _controlToUpdate;
        private readonly TypeOfEffect _effectType = TypeOfEffect.Highlight;
        private static Dictionary<string, KeyValuePair<ConstructorInfo, MethodInfo>> _effectCache;

        #endregion

        #region [ -- Properties -- ]

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Constructor
        /// </summary>
        protected Effect() { }

        /// <summary>
        /// Constructor
        /// </summary>
        [Obsolete(ObsoleteText)]
        public Effect(Control controlToUpdate, TypeOfEffect effectType) : this(controlToUpdate, effectType, 1000) { }

        /// <summary>
        /// Constructor
        /// </summary>
        [Obsolete(ObsoleteText)]
        public Effect(string htmlElementToUpdate, TypeOfEffect effectType) : this(htmlElementToUpdate, effectType, 1000) { }

        /// <summary>
        /// Constructor
        /// </summary>
        [Obsolete(ObsoleteText)]
        public Effect(string htmlElementToUpdate, TypeOfEffect effectType, int duration)
        {
            _htmlElementToUpdate = htmlElementToUpdate;
            _effectType = effectType;
            _duration = duration;
            ShowEffect();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        [Obsolete(ObsoleteText)]
        public Effect(Control controlToUpdate, TypeOfEffect effectType, int duration)
        {
            _controlToUpdate = controlToUpdate;
            _effectType = effectType;
            _duration = duration;
            ShowEffect();
        }

        #endregion

        private void ShowEffect()
        {
            var hasTarget = (_controlToUpdate != null || _htmlElementToUpdate != null);
            if (!hasTarget) return;

            const string effectAssemblyName = "Gaia.WebWidgets.Effects";
            const string effectPrefix = effectAssemblyName + ".Effect";
            var effectType = effectPrefix + _effectType;

            MethodInfo setterMethod;
            ConstructorInfo constructor;
            KeyValuePair<ConstructorInfo, MethodInfo> pair;

            if (_effectCache == null)
                _effectCache = new Dictionary<string, KeyValuePair<ConstructorInfo, MethodInfo>>();

            if (!_effectCache.TryGetValue(effectType, out pair))
            {
                if (_assembly == null)
                    _assembly = Assembly.Load(effectAssemblyName);

                var type = _assembly.GetType(effectType);
                if (type == null) return;
                constructor = type.GetConstructor(Type.EmptyTypes);
                setterMethod = type.GetProperty("Duration").GetSetMethod();
                pair = new KeyValuePair<ConstructorInfo, MethodInfo>(constructor, setterMethod);
                _effectCache[effectType] = pair;
            }
            else
            {
                constructor = pair.Key;
                setterMethod = pair.Value;
            }

            var effect = (Effect)constructor.Invoke(null);

            // here we set the duration of the effect
            var duration = _duration / 1000M;
            setterMethod.Invoke(effect, new object[] { duration });
            string targetid = _htmlElementToUpdate!= null ? _htmlElementToUpdate: _controlToUpdate.ClientID;
            Show(targetid, effect);
        }
    }
}
