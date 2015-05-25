﻿/*******************************************************************
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
    using System.Drawing;

    /// <summary>
    /// This effect flashes a color as the background of an element. It is mostly used to draw attention to a part of the page that has been updated via JavaScript or AJAX, when the update would not otherwise be obvious.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectHighlight to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectHighlight\Overview\Default.aspx.cs" />
    /// </code> 
    /// </example>
    public class EffectHighlight : JQueryUIEffectBase
    {
        private Color _startColor = Color.LightYellow;
        private Color _endColor = Color.White;
        private Color _restoreColor = Color.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHighlight"/> class.
        /// </summary>
        public EffectHighlight() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHighlight"/> class.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        public EffectHighlight(Color startColor) : this(startColor, Color.White, Color.Empty, false, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHighlight"/> class.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="duration">The duration.</param>
        public EffectHighlight(Color startColor, decimal duration) : this(startColor, Color.White, Color.Empty, false, duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHighlight"/> class.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        [Obsolete("EndColor is no longer supported. Only use StartColor.")]
        public EffectHighlight(Color startColor, Color endColor) : this(startColor, endColor, Color.Empty, false, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHighlight"/> class.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        /// <param name="restoreColor">Color of the restore.</param>
        [Obsolete("EndColor and RestoreColor are no longer supported. Only use StartColor.")]
        public EffectHighlight(Color startColor, Color endColor, Color restoreColor) : this(startColor, endColor, restoreColor, false, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHighlight"/> class.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        /// <param name="restoreColor">Color of the restore.</param>
        /// <param name="keepBackgroundImage">if set to <c>true</c> [keep background image].</param>
        [Obsolete("EndColor, RestoreColor and KeepBackgroundImage are no longer supported. Only use StartColor.")]
        public EffectHighlight(Color startColor, Color endColor, Color restoreColor, bool keepBackgroundImage) : this(startColor, endColor, restoreColor, keepBackgroundImage, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHighlight"/> class.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        /// <param name="restoreColor">Color of the restore.</param>
        /// <param name="keepBackgroundImage">if set to <c>true</c> [keep background image].</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        [Obsolete("EndColor, RestoreColor and KeepBackgroundImage are no longer supported. Only use StartColor.")]
        public EffectHighlight(Color startColor, Color endColor, Color restoreColor, bool keepBackgroundImage, decimal duration, decimal delay)
        {
            Delay = delay;
            Duration = duration;
            StartColor = startColor;
            EndColor = endColor;
            RestoreColor = restoreColor;
            KeepBackgroundImage = keepBackgroundImage;
        }

        /// <summary>
        /// Sets the color of first frame of the highlight. Defaults to ”#ffff99” (a light yellow).
        /// </summary>
        public Color StartColor
        {
            get { return _startColor; }
            set { _startColor = value; }
        }

        /// <summary>
        /// Sets the color of the last frame of the highlight. This is best set to the background color of the highlighted element. Defaults to ”#ffffff” (white).
        /// </summary>
        [Obsolete("This property is no longer supported. Only use StartColor.")]
        public Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; }
        }

        /// <summary>
        /// Sets the background-color of the element after the highlight has finished. Defaults to the current background-color of the highlighted element (see Note).
        /// </summary>
        [Obsolete("This property is no longer supported. Only use StartColor.")]
        public Color RestoreColor
        {
            get { return _restoreColor; }
            set { _restoreColor = value; }
        }

        /// <summary>
        /// Unless this is set to true, any background image on the element will not be preserved. 
        /// </summary>
        [Obsolete("This property is no longer supported. Only use StartColor.")]
        public bool KeepBackgroundImage { get; set; }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect.AddPropertyIfTrue(StartColor != Color.LightYellow, "color", ColorToHexString(StartColor)));

        }

        private static readonly char[] HexDigits = {
                                                        '0', '1', '2', '3', '4', '5', '6', '7',
                                                        '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
                                                    };

        private static string ColorToHexString(Color color)
        {
            var bytes = new[] { color.R, color.G, color.B };
            var chars = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; ++i)
            {
                int b = bytes[i];
                chars[i * 2] = HexDigits[b >> 4];
                chars[i * 2 + 1] = HexDigits[b & 0xF];
            }
            return "#" + new string(chars);
        }

        protected override string EffectType
        {
            get { return "highlight"; }
        }
    }
}