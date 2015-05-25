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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Defines methods for managing <see cref="System.Web.UI.ClientScriptManager"/> registrations during Ajax callbacks.
    /// </summary>
    public sealed class ClientScriptManager
    {
        private readonly Manager _manager;

        private const string EndTrackingClientScriptManagerRegistrationMarker =
            "EndTrackingClientScriptManagerRegistrations";

        private const string BeginTrackingClientScriptManagerRegistrationsMarker =
            "BeginTrackingClientScriptManagerRegistrations";

        private List<string> _arrayNames;
        private List<string> _expandoControls;
        private List<string> _scriptBlockKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientScriptManager"/> class.
        /// </summary>
        internal ClientScriptManager(Manager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            _manager = manager;
        }

        /// <summary>
        /// Updates declaration of an array having <paramref name="arrayName"/>,
        /// </summary>
        /// <remarks>
        /// The array should have been declared using <see cref="System.Web.UI.ClientScriptManager.RegisterArrayDeclaration(string,string)"/> method.
        /// </remarks>
        /// <param name="arrayName">Name of the array which was previously declared and needs update.</param>
        public void UpdateArrayDeclaration(string arrayName)
        {
            AddToList(ref _arrayNames, arrayName);
        }

        /// <summary>
        /// Updates registration of expando attributes for a control having specified <paramref name="controlid"/>.
        /// </summary>
        /// <remarks>
        /// The array should have been registered using <see cref="System.Web.UI.ClientScriptManager.RegisterExpandoAttribute(string,string,string)"/> method.
        /// </remarks>
        /// <param name="controlid">Id of the control which had expando attributes which need to be updated.</param>
        public void UpdateExpandoAttributes(string controlid)
        {
            AddToList(ref _expandoControls, controlid);
        }

        /// <summary>
        /// Updates registrations of client script and startup blocks.
        /// </summary>
        /// <remarks>
        /// The array should have been tracked using <see cref="TrackScriptRegistrations"/> method.
        /// </remarks>
        public void UpdateScriptRegistrations(Type type, string key)
        {
            AddToList(ref _scriptBlockKeys, CreateScriptBlockKey(type, key));
        }

        /// <summary>
        /// Tracks registrations of client script and startup blocks.
        /// </summary>
        /// <remarks>
        /// Client script blocks should be registered using <see cref="System.Web.UI.ClientScriptManager.RegisterClientScriptBlock(System.Type,string,string,bool)"/> and
        /// <see cref="System.Web.UI.ClientScriptManager.RegisterStartupScript(System.Type,string,string,bool)"/> methods.
        /// Only client script blocks which are wrapped inside script tags are supported.
        /// </remarks>
        /// <example>
        /// using( Manager.Instance.TrackScriptRegistrations(typeof(MyControl), "wrapperKey") )
        /// {
        ///   Page.ClientScript.RegisterClientScriptBlock(typeof(MyControl), "scriptKey", "alert('hello world');", true);
        /// }
        /// </example>
        /// <returns>An <see cref="IDisposable"/> object which should be used with using statement.</returns>
        public IDisposable TrackScriptRegistrations(Type type, string key)
        {
            var clientScript = _manager.Page.ClientScript;
            return new AtomicInvoker(
                () => RegisterClientScriptBlockMarker(clientScript, type, key, true),
                () => RegisterClientScriptBlockMarker(clientScript, type, key, false));
        }

        /// <summary>
        /// Marks the script sections which are used for rendering array registrations and expando attributes.
        /// </summary>
        internal void TrackArrayAndExpandoRegistrations()
        {
            var clientScript = _manager.Page.ClientScript;
            clientScript.RegisterArrayDeclaration(Manager.ArrayRegistrationSectionMarker, "0");
            clientScript.RegisterExpandoAttribute(Manager.ExpandoAttributeSectionMarker, "begin", string.Empty);
        }

        /// <summary>
        /// Return true if the array registrations with the specified <paramref name="arrayName"/> need to be updated.
        /// </summary>
        internal bool IsUpdatableArrayName(string arrayName)
        {
            return _arrayNames != null && _arrayNames.Contains(arrayName);
        }

        /// <summary>
        /// Return true if the expando attribute registrations for the <paramref name="controlid"/> need to be updated.
        /// </summary>
        internal bool IsUpdatableExpandoControlId(string controlid)
        {
            return _expandoControls != null && _expandoControls.Contains(controlid);
        }

        /// <summary>
        /// Returns true if the specified <paramref name="scriptCapture"/> is inside a script block,
        /// which was marked for update using <see cref="UpdateScriptRegistrations"/>.
        /// </summary>
        internal bool IsInsideUpdatableScriptBlock(string pageContent, Capture scriptCapture)
        {
            if (_scriptBlockKeys != null)
            {
                var preface = pageContent.Substring(0, scriptCapture.Index);

                foreach (var key in _scriptBlockKeys)
                {
                    foreach(var isStartupScript in new [] { true, false })
                    {
                        var beginMarker = GetMarkerText(BeginTrackingClientScriptManagerRegistrationsMarker, key, isStartupScript);
                        if (!preface.Contains(beginMarker)) continue;

                        var endMarker = GetMarkerText(EndTrackingClientScriptManagerRegistrationMarker, key, isStartupScript);
                        if (preface.Contains(endMarker)) continue;

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the specified <paramref name="value"/> to the specified <paramref name="list"/>.
        /// Makes sure the <paramref name="list"/> is initialized and the <paramref name="value"/> is unique.
        /// </summary>
        private static void AddToList(ref List<string> list, string value)
        {
            if (list == null)
                list = new List<string>();

            if (list.Contains(value)) return;
            list.Add(value);
        }

        /// <summary>
        /// Registers begin or end marker with the specified <paramref name="clientScript"/>.
        /// </summary>
        private void RegisterClientScriptBlockMarker(System.Web.UI.ClientScriptManager clientScript, Type type, string key, bool beginMarker)
        {
            if (!_manager.IsAjaxCallback)
                return;

            var marker = beginMarker
                             ? BeginTrackingClientScriptManagerRegistrationsMarker
                             : EndTrackingClientScriptManagerRegistrationMarker;

            var registrationKey = CreateScriptBlockKey(type, key);
            clientScript.RegisterStartupScript(typeof (ClientScriptManager), registrationKey + "startup" + beginMarker,
                                               GetMarkerText(marker, registrationKey, true), false);

            clientScript.RegisterClientScriptBlock(typeof (ClientScriptManager), registrationKey + beginMarker,
                                                   GetMarkerText(marker, registrationKey, false), false);
        }

        /// <summary>
        /// Returns text for marker registrations
        /// </summary>
        private static string GetMarkerText(string marker, string key, bool trackStartupScripts)
        {
            var blockKey = marker + key;
            if (trackStartupScripts)
                blockKey += "startup";

            return "<!-- " + blockKey + " -->";
        }

        /// <summary>
        /// Creates a string which acts as a key for a script block.
        /// </summary>
        private static string CreateScriptBlockKey(Type type, string key)
        {
            return type.FullName + key;
        }
    }
}
