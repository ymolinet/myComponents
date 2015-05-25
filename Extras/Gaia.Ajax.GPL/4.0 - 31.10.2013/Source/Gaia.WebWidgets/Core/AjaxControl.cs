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
using System.IO;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gaia.WebWidgets
{
    using Helpers;
    using HtmlFormatting;

    /// <summary>
    /// This is the worker class used by Gaia Ajax Controls to initialize the ajax machinery.
    /// It handles state, rendering, callbacks, general inclusion of resources and more. 
    /// <para>Usage is to implement IAjaxControl and write forward calls in that interface to this class.</para>
    /// </summary>   
    /// <seealso cref="AspectableAjaxControl"/>
    /// <seealso cref="AjaxContainerControl"/>
    /// <seealso cref="IAjaxControl"/>
    /// <seealso cref="IAjaxContainerControl"/>
    public class AjaxControl
    {
        #region [ -- Private Members -- ]

        private Page _page;
        private bool _moved;
        private bool _removed;
        private bool? _resolved;
        private bool _inCallback;
        private bool _parentMoved;
        private bool _hasReplacement;
        private object _extractedState;
        private List<int> _coordinates;
        private bool? _viewstateEnabled;
        private readonly Control _control;
        private List<int> _virtualCoordinates;
        private string _replacementClientReference;
        private readonly IAjaxControl _ajaxControl;
        private bool _clientReferenceConflictsResolved;
        private List<KeyValuePair<Type, string>> _requiredStyleSheets;
        
        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// See <see cref="IAjaxControl.StateManager" /> for documentation on this method
        /// </summary>
        public PropertyStateManagerControl StateManager { get; private set; }

        /// <summary>
        /// Shortend way to access Manager.Instance.
        /// Used by derived classes for cached access to Manager.Instance.
        /// </summary>
        protected Manager Manager { get; private set; }

        /// <summary>
        /// Returns the client reference which the control has been rendered with.
        /// </summary>
        protected internal string ClientReference { get; private set; }

        /// <summary>
        /// Returns true if the ClientID of the associated <see cref="Control"/> was modified.
        /// </summary>
        internal bool Renamed
        {
            get { return ClientReference != _control.ClientID; }
        }

        /// <summary>
        /// Returns true if the control should be completely re-rendered. Gaia tries to do "partial rendering" as
        /// seldom as possible, however there are a few scenarios where partial rendering is "inevitable".
        /// One of those are if the control has not previously been rendered due to either being "invisible" or
        /// some ancestor container widget was invisible. Another reason might be because the control needs a full
        /// re-rendering like for instance ForceAnUpdate on container widgets. If this property is true then
        /// a full re-rendering will be initiated.
        /// </summary>
        public bool ShouldRender { get; set; }

        /// <summary>
        /// Returns true if the associated <see cref="Control"/> needs to be rendered.
        /// </summary>
        /// <remarks>
        /// Used by derived classes to know if the control should be rendered or not
        /// based on the facts if the <see cref="ShouldRender"/> was set or <see cref="Control"/> was previously rendered
        /// </remarks>
        internal protected virtual bool ShouldRenderAjaxControl
        {
            get
            {
                if (ShouldRender || WasMadeVisible)
                    return true;

                var stateManager = StateManager as IMayRequireRerendering;
                return stateManager != null && stateManager.RequiresRerendering;
            }
        }

        /// <summary>
        /// True if the visibility of control was changed from false to true.
        /// Otherwise, false.
        /// </summary>
        private bool WasMadeVisible
        {
            get { return _control.Visible && !StateManager.WasVisible; }
        }

        /// <summary>
        /// True if the visibility of the control was changed from true to false.
        /// Otherwise, false.
        /// </summary>
        private bool WasMadeHidden
        {
            get { return !_control.Visible && StateManager.WasVisible; }
        }

        /// <summary>
        /// Gets value indicating if this ajax control was previously rendered or not.
        /// Used by derived classes.
        /// </summary>
        protected bool Rendered { get; private set; }

        /// <summary>
        /// Gets value indicating if the ajax control is rerendered.
        /// Used by derived classes.
        /// </summary>
        protected bool Rerendered { get; set; }

        /// <summary>
        /// True if the control was removed and readded to a position
        /// which is different from the initial one.
        /// </summary>
        private bool Moved
        {
            get
            {
                if (!_removed) return false;
                var coordinates = GetCoordinates(true);
                return !coordinates.Contains(-1) && !DenoteSamePosition(coordinates, _virtualCoordinates);
            }
        }

        /// <summary>
        /// True if the control was removed and it is not part of the control tree.
        /// </summary>
        private bool Removed
        {
            get { return _removed && !PartOfControlTree; }
        }

        /// <summary>
        /// True if the control is part of the control tree,
        /// meaning it is possible to climb up to the page using Parent property.
        /// </summary>
        internal bool PartOfControlTree
        {
            get
            {
                var control = _control;

                // check if has page
                var page = control.Page;
                if (page == null) return false;

                // check if can get to the page
                for (var parent = control.Parent; parent != null; parent = parent.Parent)
                    control = parent;

                return control == page;
            }
        }

        /// <summary>
        /// Returns true if after all controls are rendered it does not act as a placeholder
        /// for any DRIMR operation and it is not part of any rerendering (which would have resulted in correct client state), and:
        /// 1. the control was removed and not readded to the control tree
        /// - or -
        /// 2. it was rendered on the client, but wasn't or couldn't be rendered during this request
        /// - or -
        /// 3. control was moved and the initial state was not used for that (ie. moved and rerendered)
        /// </summary>
        internal bool ShouldRemove
        {
            get { return !_hasReplacement && (Removed || (Rendered && (!_resolved.HasValue || (_moved && Rerendered)))) && !IsPartOfRerendering(true); }
        }

        /// <summary>
        /// Returns true if State information can be persisted in the ViewState of the control.
        /// Otherwise, false.
        /// </summary>
        private bool ViewStateEnabled
        {
            get
            {
                if (!_viewstateEnabled.HasValue)
                    _viewstateEnabled = Utilities.IsViewStateEnabled(_control);
                
                return _viewstateEnabled.Value;
            }
        }

        /// <summary>
        /// Gets or sets if the input focus is set to this control.
        /// </summary>
        public bool Focused { get; private set; }

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// The constructor takes a control that implements the IAjaxControl interface. 
        /// </summary>
        /// <param name="ajaxControl">Control which implements the IAjaxControl interface</param>
        public AjaxControl(IAjaxControl ajaxControl)
        {
            // get a reference back to the main control
            _ajaxControl = ajaxControl;
            _control = _ajaxControl.Control;
        }

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Sets input focus to this control.
        /// </summary>
        public void Focus()
        {
            Focused = true;
        }

        /// <summary>
        /// Forward calls of <see cref="Control.OnInit"/> to this method.
        /// Prepares ajax functionality of the control.        
        /// </summary>
        public void OnInit()
        {
            if (_ajaxControl.InDesigner) return;

            // initialize occasionally used properties
            _page = _control.Page;
            Manager = Manager.Instance;
            _inCallback = Manager.IsAjaxCallback;

            // register ajax control on the manager
            Manager.RegisterAjaxControl(_page, _control);

            // apply default skin on controls implementing ISkinControl
            ApplyDefaultSkin();
        }

        /// <summary>
        /// Forward calls of <see cref="Control.TrackViewState"/> to this method.
        /// </summary>
        public void TrackViewState()
        {
            // RegisterRequiresControlState may call LoadControlState
            // if the control is added dynamically.
            // That's why it's important that this step is delayed as much as possible
            // and is done in TrackViewState instead of OnInit.
            if (_page != null && !ViewStateEnabled)
                _page.RegisterRequiresControlState(_control);
        }

        /// <summary>
        /// Called to load control state
        /// </summary>
        /// <param name="savedState">Saved control state</param>
        /// <returns>Saved state to pass to the base LoadControlState method</returns>
        [Obsolete("LoadControlState() is deprecated. Use BeginLoadControlState() and EndLoadControlState() instead.", true)]
        public object LoadControlState(object savedState)
        {
            throw new NotSupportedException("LoadControlState() is deprecated. Use BeginLoadControlState() and EndLoadControlState() instead.");
        }

        /// <summary>
        /// Called at the beginning of the <see cref="Control.LoadControlState"/> method 
        /// to extract saved state for the chained <see cref="Control.LoadControlState"/> method.
        /// Should be coupled with <see cref="EndLoadControlState"/> method after the control state is loaded.
        /// </summary>
        /// <param name="savedState">Saved control state</param>
        /// <returns>Saved state to pass to the chained (base) <see cref="Control.LoadControlState"/> method</returns>
        public object BeginLoadControlState(object savedState)
        {
            return ViewStateEnabled ? savedState : LoadState(savedState);
        }

        /// <summary>
        /// Called at the end of the <see cref="Control.LoadControlState"/> method
        /// after the state loading is fully complete.
        /// Should be coupled with the <see cref="BeginLoadControlState"/> method.
        /// </summary>
        public void EndLoadControlState()
        {
            if (!_inCallback || ViewStateEnabled) return;
            InitializeAjaxControl();
        }

        /// <summary>
        /// Called at the beginning of the <see cref="Control.LoadViewState"/> method 
        /// to extract saved state for the chained <see cref="Control.LoadViewState"/> method.
        /// Should be coupled with <see cref="EndLoadViewState"/> method after the viewstate is loaded.
        /// </summary>
        /// <param name="savedState">Saved view state</param>
        /// <returns>Saved state to pass to the chained (base) <see cref="Control.LoadViewState"/> method</returns>
        public object BeginLoadViewState(object savedState)
        {
            return LoadState(savedState);
        }

        /// <summary>
        /// Called at the end of the <see cref="Control.LoadViewState"/> method
        /// after the state loading is fully complete.
        /// Should be coupled with the <see cref="BeginLoadViewState"/> method.
        /// </summary>
        public void EndLoadViewState()
        {
            if (!_inCallback) return;
            InitializeAjaxControl();
        }

        /// <summary>
        /// Callback delegate for the <see cref="IPostBackDataHandler.LoadPostData"/> method.
        /// Used in combination with <see cref="AjaxControl.ExecuteLoadPostData"/> method.
        /// </summary>
        /// <param name="postDataKey">Post Data Key</param>
        /// <param name="postCollection">Name Value Collection</param>
        /// <returns>True if State Changed</returns>
        public delegate bool LoadPostDataDelegate(string postDataKey, NameValueCollection postCollection);

        /// <summary>
        /// Delegate specifying a delegate which will be called
        /// when clearing of the dirtiness of some properties
        /// during LoadPostData() may be necessary
        /// </summary>
        /// <param name="stateMananger">StateManager instance to use for clearing property state</param>
        public delegate void ClearPropertyStateDelegate(PropertyStateManagerControl stateMananger);

        /// <summary>
        /// This function is used to make sure we detect changes and merge them when LoadPostData Second Try fires after you have modified
        /// the state of your control. 
        /// </summary>
        /// <param name="loadPostDataDelegate">Pass in the LoadPostDataDelegate</param>
        /// <param name="postDataKey">Post Data Key</param>
        /// <param name="postCollection">Name Value Collection </param>
        /// <param name="clearPropertyStateDelegate">
        /// Delegate which will be called when clearing of dirtiness
        /// may be necessary for some properties
        /// </param>
        /// <returns>True if State Changed</returns>
        public bool ExecuteLoadPostData(LoadPostDataDelegate loadPostDataDelegate, string postDataKey, NameValueCollection postCollection, ClearPropertyStateDelegate clearPropertyStateDelegate)
        {
            var stateChanged = loadPostDataDelegate(postDataKey, postCollection);

            if (_inCallback && stateChanged && clearPropertyStateDelegate != null)
            {
                // LoadPostData may be called after OnLoad phase (LoadPostData second try).
                // In this case, it may be that the dynamic control is recreated after LoadViewState is called,
                // in which case it won't have StateManager until a replacement is found during rendering.
                PropertyStateManagerControl replacementStateManager = null;

                if (StateManager == null)
                {
                    var replacement = GetReplacement();
                    if (replacement == null)
                        throw new InvalidOperationException("LoadPostBack() is called for a dynamic control, which was not initialized.");

                    replacementStateManager = replacement.StateManager;
                    
                    // assign replacement PSM to this PSM
                    StateManager = _ajaxControl.CreateControlStateManager();
                    StateManager.AssignState(replacementStateManager, false);
                }
                
                clearPropertyStateDelegate(StateManager);

                if (replacementStateManager != null)
                {
                    // assign this PSM to replacement PSM
                    replacementStateManager.AssignState(StateManager, false);
                    StateManager = null;
                }
            }

            return stateChanged;
        }

        /// <summary>
        /// Called just before the rendering of the Control is being initiated.
        /// We use this override to add inclusion of the JavaScript files for the <see cref="Control"/> and signalize that we have
        /// a Gaia Control visible on the <see cref="Page" />
        /// </summary>
        public virtual void OnPreRender()
        {
            Manager.HasVisibleGaiaControl = true;
            Manager.AddInclusionOfCommonFiles();            
            
            // include default stylesheets, if they were previously required by the control implementing ISkinControl interface
            // and at this point in the control lifecycle the default skin is still enabled.
            if (_requiredStyleSheets == null) return;
            
            var skinControl = (ISkinControl) _control;
            if (!skinControl.Enabled) return;

            _requiredStyleSheets.ForEach(
                styleSheet =>
                Manager.AddInclusionOfStyleSheetFromResource(styleSheet.Key, styleSheet.Value));
        }

        /// <summary>
        /// Forward calls of <see cref="Control.SaveViewState"/> to this method. 
        /// Stores required state information.
        /// </summary>
        public object SaveViewState(object savedState)
        {
            return ViewStateEnabled ? SaveState(savedState) : savedState;
        }

        /// <summary>
        /// Saves state information into control state if <see cref="Control.EnableViewState"/> is false.
        /// </summary>
        /// <param name="savedState">Base saved state</param>
        /// <returns>New state to save into control state</returns>
        public object SaveControlState(object savedState)
        {
            return ViewStateEnabled ? savedState : SaveState(savedState);
        }

        /// <summary>
        /// Forward calls of <see cref="Control.RenderControl(HtmlTextWriter)"/> to this method.
        /// Renders the control. Handles various scenarios that controls can be rendered in. 
        /// </summary>        
        /// <param name="writer">Specifies where the Control will be rendered</param>
        public void RenderControl(HtmlTextWriter writer)
        {
            // InDesigner check is not enough, because for GridView child controls DesignMode will be false
            if (_ajaxControl.InDesigner || HttpContext.Current == null)
            {
                RenderDesignTime(GetDesignTimeWriter(writer));
                return;
            }

            _resolved = true;
            _parentMoved = writer is MovedContainerHtmlTextWriter;
            _moved = !_parentMoved && Moved;

            var create = new XhtmlTagFactory(writer);
            if (_inCallback && !(writer is ContainerHtmlTextWriter))
            {
                var additionsWriter = writer as ContainerHtmlTextWriterPartialForce;
                if (additionsWriter != null && (additionsWriter.Peek() || ShouldRender))
                {
                    additionsWriter.Push();
                    RenderNonCallback(create);
                    additionsWriter.Pop();
                }
                else
                    RenderCallback(writer);
            }
            else
                RenderNonCallback(create);
        }

        /// <summary>
        /// Forward calls of <see cref="Control.OnLoad"/> to this method.
        /// </summary>
        public void OnUnload()
        {
            if (!_inCallback || !Rendered) return;

            // check to see if OnUnload is called during lifecycle's unload phase
            // or the control is being removed from the page, in which case
            // either it should not have a parent or it should not be part of parent control.
            var control = _control;
            for (var parent = control.Parent; parent != null && !_removed; parent = parent.Parent)
                _removed = parent.Controls.IndexOf(control) == -1;
        }

        /// <summary>
        /// Registers a stylesheet from the resource using the specified <paramref name="type"/> and <paramref name="resourceName"/> 
        /// for the associated <see cref="ISkinControl"/>.
        /// </summary>
        /// <param name="type">Type which acts as a key for the embedded resource to include.</param>
        /// <param name="resourceName">The embedded resource name to include.</param>
        public void RegisterDefaultSkinStyleSheetFromResource(Type type, string resourceName)
        {
            var skinControl = _control as ISkinControl;
            if (skinControl == null)
                throw new InvalidOperationException("RegisterDefaultSkinStyleSheetFromResource method should be called only on controls implementing ISkinControl interface.");

            if (_requiredStyleSheets == null)
                _requiredStyleSheets = new List<KeyValuePair<Type, string>>();

            if (_requiredStyleSheets.Exists(pair => pair.Key == type && pair.Value == resourceName)) return;
            _requiredStyleSheets.Add(new KeyValuePair<Type, string>(type, resourceName));
        }

        #endregion

        /// <summary>
        /// This method will run when either this is an initial hit, a conventional postback or
        /// for some other reasons (e.g. ForceAnUpdate) the control needs to render HTML instead
        /// of just changing the property values.
        /// </summary>
        /// <param name="create">XhtmlTagFactory passed to use as basis for creating markup</param>
        protected void RenderNonCallback(XhtmlTagFactory create)
        {
            Rerendered = true;

            if (Manager.IsAjaxCallback)
                ResolveClientReferenceConflicts(create.GetHtmlTextWriter());

            if (_control.Visible)
            {
                // Since there's a issue in ASP.NET GridView which makes no controls render ID attributes
                // if the ClientID property is NOT accessed before we render the actual control
                // and this also have HUGE implications for Gaia we MUST call the GetScript
                // method BEFORE we render the markup HTML since this will trigger a call
                // to ClientID property which will make sure the ID attribute is rendered on the
                // root element.
                var objectInitializationScript = _ajaxControl.GetScript();

                // Ensuring we render the default url for our controls
                Manager.RenderGlobalSettings();

                // Actual control, the base class handles the Visible stuff itself...!!
                _ajaxControl.RenderControlHtml(create);

                // Object JavaScript code
                // We always write ALL script into the Managers Writer
                Manager.Writer.Write(objectInitializationScript);
            }
            else
            {
                // create placeholder
                using(var textWriter = create.GetHtmlTextWriter())
                {
                    textWriter.AddAttribute(HtmlTextWriterAttribute.Id, _control.ClientID);
                    textWriter.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    textWriter.AddAttribute(HtmlTextWriterAttribute.Class, GaiaAjaxConfiguration.Instance.CssClassHiddenControl);
                    textWriter.RenderBeginTag(_ajaxControl.TagName);
                    textWriter.RenderEndTag();
                }
            }
        }

        /// <summary>
        /// Resolves client reference and client id conflicts.
        /// </summary>
        private void ResolveClientReferenceConflicts(TextWriter writer = null)
        {
            if (_clientReferenceConflictsResolved || (ClientReference != null && !Renamed)) return;
            _clientReferenceConflictsResolved = true;

            var clientid = _control.ClientID;
            AjaxControl conflictingControl = null;
            foreach (var ajaxControl in Manager.RegisteredAjaxControls)
            {
                var impl = ajaxControl.AjaxControl;
                
                if (!ReferenceEquals(impl, this) && impl.ClientReference == clientid && 
                    !impl._resolved.HasValue && !impl._hasReplacement && !impl.IsPartOfRerendering(false))
                {
                    conflictingControl = impl;
                    break;
                }
            }

            if (conflictingControl == null) return;
            ResolveClientReferenceConflict(conflictingControl, writer);
        }

        /// <summary>
        /// Resolve <see cref="ClientReference"/> conflict when itself.
        /// </summary>
        /// <remarks>
        /// Such conflicts occur for example when the control is moved and rerendered,
        /// in which case the old reference needs to cleaned up.
        /// </remarks>
        private void ResolveClientReferenceSelfConflict()
        {
            if (!_moved || ClientReference != _control.ClientID) return;
            ResolveClientReferenceConflict(this, null);
        }

        /// <summary>
        /// Returns true if the <see cref="ClientReference"/> is part of rerendering.
        /// </summary>
        /// <param name="includeSelf">True if this <see cref="AjaxControl"/> should be analyzed as well.</param>
        /// <remarks>
        /// Being part of rerendering means that <see cref="ClientReference"/> cannot be used as client reference.
        /// </remarks>
        private bool IsPartOfRerendering(bool includeSelf)
        {
            Control control = _page;
            var end = includeSelf ? 0 : 1;
            for (var index = _coordinates.Count - 1; index >= end; --index)
            {
                var position = _coordinates[index];
                var controls = control.Controls;

                // if the position is out of range of possible positions,
                // we can't continue
                if (position >= controls.Count) break;

                // check to see if the control at the position is an IAjaxControl
                // and skip the ones which are not
                control = controls[position];
                var ajaxControl = control as IAjaxControl;
                if (ajaxControl == null) continue;

                // check the implementation of the ajax control to find if it is rerendering.
                // for the control to be rerendered, it should meet following requirements:
                // 1. it was rendered previously 
                // - or -
                // 2. rerendered during this request using DRIMR, which includes the cases, when:
                // 2a. the control was initially invisible and now it is visible, thus it will be fully rerendered
                // 2b. the control was initially visible and now it's being made invisible (thus being destroyed)
                var impl = ajaxControl.AjaxControl;
                if (!impl._resolved.HasValue)
                    continue;
                
                // if control was not rendered, it's children cannot be rendered as well.
                if (!impl.Rendered)
                    break;

                if (impl.Rerendered || impl.WasMadeHidden)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Resolve <see cref="ClientReference"/> conflict with the specified <paramref name="conflictingControl"/>.
        /// </summary>
        private void ResolveClientReferenceConflict(AjaxControl conflictingControl, TextWriter writer)
        {
            const string renameSuffix = "---";
            var reference = conflictingControl.ClientReference;
            conflictingControl.ClientReference += renameSuffix;

            var action = string.Concat(Constants.GaiaClientModule, ".TR('", reference, "');");

            var forceAnUpdateWriter = writer as ContainerHtmlTextWriterBase;
            if (forceAnUpdateWriter == null)
                Manager.Writer.Write(action);
            else
                forceAnUpdateWriter.RenderForceAnUpdateRequirement(action);
        }

        /// <summary>
        /// Returns containing <see cref="ControlCollection"/> for the specified control.
        /// </summary>
        /// <seealso cref="GetVirtualContainer(System.Web.UI.Control)"/>
        private static ControlCollection GetContainer(Control control, bool useVirtualContainers = true)
        {
            var parentControl = control.Parent;
            var parentAjaxControl = parentControl as IAjaxControl;

            return parentAjaxControl == null || !useVirtualContainers
                       ? parentControl.Controls
                       : parentAjaxControl.AjaxControl.GetVirtualContainer(control);
        }

        /// <summary>
        /// Returns virtual <see cref="ControlCollection"/> for the specified child <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Child control to get container for.</param>
        /// <returns>Virtual container for the specified control.</returns>
        /// <remarks>
        /// Container <see cref="ControlCollection"/> can be partitioned into different sets or
        /// combined into different groups which are rendered and handled separately.
        /// Such an example is child ToolbarItems and ChildControls of a ToolbarItem.
        /// This method should return virtual partition of the container to use for DRIMR operations.
        /// </remarks>
        protected virtual ControlCollection GetVirtualContainer(Control control)
        {
            // by default all child controls should be part of our control collection.
            return _control.Controls;
        }

        /// <summary>
        /// Defines how the replacement was handled.
        /// </summary>
        /// <seealso cref="AjaxControl.HandleReplacement"/>.
        private enum ReplacementMode
        {
            /// <summary>
            /// No replacement was found or was not able to replace.
            /// </summary>
            NotReplaced,

            /// <summary>
            /// Replacement was found with required markup rendering.
            /// </summary>
            ReplacedWithRendering,
            
            /// <summary>
            /// Replacement was found which did not require markup rendering.
            /// </summary>
            ReplacedWithoutRendering
        } ;

        /// <summary>
        /// Tries to find a replacement and render changes instead of it.
        /// </summary>
        /// <remarks>
        /// Replacement is an <see cref="IAjaxControl"/>, which was removed
        /// from the same position as the control being rendered.
        /// </remarks>
        /// <param name="rendered">True if the control was rendered.</param>
        /// <returns>
        /// True if replacement of the same type was found and replacement does not require markup rendering.
        /// </returns>
        private ReplacementMode HandleReplacement(bool rendered)
        {
            var replacement = GetReplacement();
            if (replacement == null)
                return ReplacementMode.NotReplaced;

            // check if the replacement is the same type and in case of markup rendering
            // the destination should have a rendered root element.
            var replacementControl = replacement.Control;
            var requiresRendering = !Type.GetTypeHandle(replacementControl).Equals(Type.GetTypeHandle(_control));
            if (requiresRendering && (replacementControl is IAjaxControlNoRoot))
                return ReplacementMode.NotReplaced;

            var replacementAjaxControl = replacement.AjaxControl;
            replacementAjaxControl._hasReplacement = true;

            if (!requiresRendering)
            {
                if (!rendered)
                {
                    Rendered = true;

                    _coordinates = replacementAjaxControl._coordinates;
                    _virtualCoordinates = replacementAjaxControl._virtualCoordinates;

                    ClientReference = replacementAjaxControl.ClientReference;
                    ResolveClientReferenceConflicts();

                    StateManager = _ajaxControl.CreateControlStateManager();
                }

                StateManager.AssignState(replacement.StateManager, _moved);
                requiresRendering = rendered && DetermineRequiresMoveSerialization();
            }
            else if (rendered)
                requiresRendering = !ShouldRenderAjaxControl;

            if (requiresRendering)
                RenderReplaced(replacementAjaxControl.ClientReference, rendered);
            else
                _replacementClientReference = replacementAjaxControl.ClientReference;

            return requiresRendering ? ReplacementMode.ReplacedWithRendering : ReplacementMode.ReplacedWithoutRendering;
        }

        /// <summary>
        /// Tries to render the control into the top or bottom parts of its parent container.
        /// </summary>
        /// <returns>True if placement succeeded. Otherwise, false.</returns>
        private bool HandleContainerPlacement(CallbackWriter callbackWriter, bool isFirst, bool rendered)
        {
            var parent = _control.Parent;
            var ajaxContainerControl = parent as IAjaxContainerControl;

            var page = _page ?? _control.Page;
            var isPage = parent == page;
            var isForm = parent == page.Form;

            if (ajaxContainerControl == null && !isForm && !isPage && callbackWriter == null)
                return false;

            _resolved = true;

            // determine reference id
            var referenceid = string.Empty;
            if (!isPage)
            {
                if (!isForm)
                {
                    if (ajaxContainerControl == null)
                    {
                        var upwardAjaxControl = callbackWriter.AjaxControl;
                        var upwardAjaxContainerControl = callbackWriter.AjaxControl as IAjaxContainerControl;
                        referenceid = upwardAjaxContainerControl != null
                                          ? upwardAjaxContainerControl.GetDOMContainer(_ajaxControl)
                                          : upwardAjaxControl.Control.ClientID;
                    }
                    else
                        referenceid = ajaxContainerControl.GetDOMContainer(_ajaxControl);
                }
                else
                    referenceid = parent.ClientID;
            }

            // render
            if (isFirst)
                RenderTop(referenceid, rendered);
            else
                RenderBottom(referenceid, rendered);

            return true;
        }

        /// <summary>
        /// Try to find a place after which to put this control.
        /// </summary>
        private void HandleAfterPlacement(ControlCollection activeContainer, int position, bool rendered)
        {
            int index;
            IAjaxControl referenceAjaxControl = null;
            for (index = position - 1; index >= 0; --index)
            {
                referenceAjaxControl = activeContainer[index] as IAjaxControl;
                if (referenceAjaxControl != null) break;
            }

            if (referenceAjaxControl == null || !referenceAjaxControl.AjaxControl._resolved.HasValue ||
                !referenceAjaxControl.AjaxControl._resolved.Value ||
                (index != position - 1 && !AreAllEmptyLiteralControls(activeContainer, index + 1, position)))
                return;

            _resolved = true;
            RenderAfter(referenceAjaxControl.Control.ClientID, rendered);
        }

        /// <summary>
        /// Tries to handle the case when the any placement did not succeed.
        /// </summary>
        private void HandlePlacementFailure(CallbackWriter callbackWriter, ControlCollection container, int position, bool isLast)
        {
            if (!_resolved.HasValue || _resolved.Value) return;

            if (callbackWriter == null)
            {
                var laterPlacementPossible = false;

                if (!isLast)
                {
                    var count = container.Count;
                    laterPlacementPossible = true;

                    for (var index = position + 1; index < count && laterPlacementPossible; ++index)
                    {
                        var nextControl = container[index];
                        laterPlacementPossible = IsEmptyLiteralControl(nextControl) || nextControl is IAjaxControl;
                    }
                }

                if (!laterPlacementPossible)
                    RequireForceAnUpdate(_control.Parent);
            }
            else
                callbackWriter.RegisterUnresolved(_ajaxControl);
        }

        /// <summary>
        /// Renders the Control during Ajax Callback.
        /// </summary>
        protected void RenderCallback(HtmlTextWriter writer)
        {
            var parent = _control.Parent;
            var callbackWriter = writer as CallbackWriter;
            var ajaxContainerControl = parent as IAjaxContainerControl;
            var upwardRendering = ajaxContainerControl == null && callbackWriter != null && !callbackWriter.HasContent();

            var container = GetContainer(_control);
            var siblings = upwardRendering ? callbackWriter.Siblings : null;
            var position = upwardRendering ? callbackWriter.Add(_ajaxControl) : container.IndexOf(_control);
            var activeContainer = siblings ?? container;

            var rendered = Rendered;
            var inplaceRendering = rendered && !_moved;

            if (!inplaceRendering)
            {
                if (rendered && IsPartOfRerendering(false))
                {
                    _moved = false;
                    rendered = false;
                }

                // try replacement first.
                var replacementMode = HandleReplacement(rendered);
                ResolveClientReferenceConflicts();

                if (replacementMode != ReplacementMode.NotReplaced)
                {
                    if (replacementMode == ReplacementMode.ReplacedWithoutRendering)
                    {
                        rendered = !ShouldRenderAjaxControl;
                        inplaceRendering = true;
                    }
                }
                else
                {
                    _resolved = false;
                    rendered = rendered && !ShouldRenderAjaxControl;
                    var activeCallbackWriter = upwardRendering ? callbackWriter : null;

                    // determine if is first or last control in the container.
                    var isLast = false;
                    var isFirst = position == 0 || AreAllEmptyLiteralControls(activeContainer, 0, position);

                    if (!upwardRendering)
                    {
                        var count = container.Count;
                        isLast = position == count - 1 || AreAllEmptyLiteralControls(container, position + 1, count);
                    }

                    var containerPlacementSucceeded = (isFirst || isLast) &&
                                                      HandleContainerPlacement(activeCallbackWriter, isFirst, rendered);

                    if (!containerPlacementSucceeded && !isFirst)
                        HandleAfterPlacement(activeContainer, position, rendered);

                    HandlePlacementFailure(activeCallbackWriter, container, position, isLast);
                }
            }
            else
                ResolveClientReferenceConflicts();

            if (inplaceRendering && ShouldRenderAjaxControl)
                RenderControlFirstTimeOnAjaxCallback();
            else if (rendered)
                RenderControlChangesOnAjaxCallback();

            if (_resolved.HasValue && _resolved.Value)
                ResolveAjaxControls(parent, activeContainer, position);
        }

        /// <summary>
        /// When the ajax control is replaced, this method is called to check
        /// if the move script needs to serialized, or if it is handled by the replacement script
        /// </summary>
        /// <returns>True if during ajax control replacement, move script needs to be serialized</returns>
        protected virtual bool DetermineRequiresMoveSerialization()
        {
            return !ShouldRenderAjaxControl;
        }

        /// <summary>
        /// Called when the control is being rerendered during ajax callback.
        /// </summary>
        protected virtual void RenderControlFirstTimeOnAjaxCallback()
        {
            RenderReplaced(_replacementClientReference ?? ClientReference, _moved && !ShouldRenderAjaxControl);
        }

        /// <summary>
        /// Called during ajax callback to render control state changes.
        /// </summary>
        protected virtual void RenderControlChangesOnAjaxCallback()
        {
            if (!_ajaxControl.StateManager.ShouldRender) return;

            _ajaxControl.StateManager.RenderChanges(Manager.Writer);
            if (!_control.Visible) return;
            
            // We still need to make sure the childcontrols are being rendered since they might have changes
            // Since all Gaia controls are rendering into the Manager Stream object we can just throw away
            // the stream output and the rendering operations of the children are being rendered into.
            // here we just throw away the stream.
            ComposeXhtml.Write(delegate(Stream stream)
                                   {
                                       using (TextWriter textWriter = new StreamWriter(stream))
                                       {
                                           RenderControlChangesOnAjaxCallback(stream, textWriter);
                                       }
                                   });
        }

        /// <summary>
        /// When the control has not yet been rendered to the page, this function is used to extract the control 
        /// content and inject it into the page.
        /// </summary>
        /// <returns>Widget HTML initialization sent back to the client</returns>
        protected virtual string GetMarkup()
        {
            return ComposeXhtml.ToString(create => _ajaxControl.RenderControlHtml(create));
        }

        private void ResolveAjaxControls(Control parent, ControlCollection activeContainer, int position)
        {
            int index;
            var forceUpdate = false;
            var unresolvedControls = new List<AjaxControl>();
            
            for (index = position - 1; index >= 0; --index)
            {
                var control = activeContainer[index];
                if (IsEmptyLiteralControl(control)) continue;

                var ajaxControl = control as IAjaxControl;
                if (ajaxControl == null) break;
                
                var impl = ajaxControl.AjaxControl;
                if (!impl._resolved.HasValue || impl._resolved.Value) continue;
                unresolvedControls.Add(impl);
            }

            // todo: this can be chained to minimize response
            var renderid = _control.ClientID;
            for(var idx = 0; idx < unresolvedControls.Count; ++idx)
            {
                var impl = unresolvedControls[idx];
                impl.RenderBefore(renderid, impl._moved);
                impl._resolved = true;
            }

            for (--index; index >= 0 && !forceUpdate; --index)
            {
                var ajaxControl = activeContainer[index] as IAjaxControl;
                if (ajaxControl == null) continue;

                var resolved = ajaxControl.AjaxControl._resolved;
                if (!resolved.HasValue || resolved.Value) continue;
                
                forceUpdate = true;
            }

            if (!forceUpdate) return;
            RequireForceAnUpdate(parent);
        }

        private static void RequireForceAnUpdate(Control parent)
        {
            var parentContainer = parent as IAjaxContainerControl;

            for (var parentControl = parent.Parent; parentContainer == null && parentControl != null; parentControl = parentControl.Parent)
                parentContainer = parentControl as IAjaxContainerControl;

            if (parentContainer != null)
                parentContainer.ForceAnUpdate();
        }

        #region [ -- Design-Time rendering related methods -- ]

        /// <summary>
        /// Returns provided <paramref name="writer"/> as is, if it's already a <see cref="DesignTimeHtmlTextWriter"/>,
        /// or creates and returns new <see cref="DesignTimeHtmlTextWriter"/>.
        /// </summary>
        private static DesignTimeHtmlTextWriter GetDesignTimeWriter(TextWriter writer)
        {
            return writer as DesignTimeHtmlTextWriter ?? new DesignTimeHtmlTextWriter(writer);
        }

        /// <summary>
        /// Renders specified <paramref name="styleSheet"/> into the provided <paramref name="writer"/>.
        /// </summary>
        private void RenderStyleSheetsDesignTime(DesignTimeHtmlTextWriter writer, KeyValuePair<Type, string> styleSheet)
        {
            var type = styleSheet.Key;
            var resourceName = styleSheet.Value;

            // we want to make sure that we don't include same stylesheet twice.
            if (writer.IsStyleSheetRendered(type, resourceName))
                return;

            writer.RenderStyleSheetResource(_page.ClientScript, type, resourceName);
        }

        /// <summary>
        /// Renders the associated control into the specified <paramref name="writer"/> during design-time.
        /// </summary>
        private void RenderDesignTime(DesignTimeHtmlTextWriter writer)
        {
            // we should apply this during rendering, because we don't want
            // the default skin values to appear on the properties grid as modified values
            _page = _control.Page;
            ApplyDefaultSkin();

            // Default skin may apply properties, which require recomposition.
            // We need to ensure composition controls before actual rendering.
            var hybridControl = _control as HybridPanelBase;
            if (hybridControl != null)
                hybridControl.EnsureCompositionControls();

            // required style sheet should not be null only if the default skin
            // was somehow applied, thus we don't need to check for ISkinControl 
            // and similar things again
            if (_requiredStyleSheets != null)
                _requiredStyleSheets.ForEach(styleSheet => RenderStyleSheetsDesignTime(writer, styleSheet));

            if (_control.Visible)
                _ajaxControl.RenderControlHtml(new XhtmlTagFactory(writer, true));
        }

        #endregion

        #region [ -- Rendering related methods -- ]

        private void Render(string action, string referenceNode, bool shouldMove, bool useCurrentClientReference = false)
        {
            if (!shouldMove)
            {
                Rerendered = true;
                ResolveClientReferenceSelfConflict();

                if (_control.Visible)
                {
                    // Since there's a issue in ASP.NET GridView which makes no controls render ID attributes
                    // if the ClientID property is NOT accessed before we render the actual control
                    // and this also have HUGE implications for Gaia we MUST call the GetScript
                    // method BEFORE we render the markup HTML since this will trigger a call
                    // to ClientID property which will make sure the ID attribute is rendered on the
                    // root element.
                    var objectInitializationScript = _ajaxControl.GetScript();

                    // This control has NEVER been rendered...!!
                    // Therefore we must render the complete contents of the control and also add up the object script for it...
                    // First we render the control into a memory stream
                    using (new AtomicInvoker(delegate
                                                 {
                                                     var writer = Manager.PushWriter();
                                                     var markup = HtmlFormatter.FormatHtmlForInnerHTML(GetMarkup());
                                                     RenderBasicAction(action, false, referenceNode, markup, writer);
                                                 }, () => Manager.PopWriter()))
                    {
                    }

                    // We always write ALL script into the Managers Writer
                    Manager.Writer.Write(objectInitializationScript);
                }
                else
                {
                    var markup = string.Format(CultureInfo.InvariantCulture,
                                               @"<{0} id=""{1}"" style=""display:none"" class=""{2}""/>",
                                               _ajaxControl.TagName, _control.ClientID, GaiaAjaxConfiguration.Instance.CssClassHiddenControl);
                    
                    RenderBasicAction(action, false, referenceNode, markup);
                }
            }
            else
                RenderBasicAction(action, true, referenceNode, useCurrentClientReference ? _control.ClientID : ClientReference);
        }

        /// <summary>
        /// Renders basic DRIMR action to the current writer.
        /// </summary>
        /// <param name="action">DRIMR action to perform.</param>
        /// <param name="move">
        /// True if sourceContent is the id of the node which needs to be moved to referenceNode; otherwise: false.
        /// </param>
        /// <param name="referenceNode">Reference node for the action.</param>
        /// <param name="sourceContent">Markup for the action if not moved, otherwise: id of the node to move.</param>
        /// <param name="writer">Writer for rendering.</param>
        private void RenderBasicAction(string action, bool move, string referenceNode, string sourceContent, TextWriter writer = null)
        {
            var format = Constants.GaiaClientModule + (move ? ".{0}('{1}','{2}',1);" : ".{0}('{1}','{2}');");
            (writer ?? Manager.Writer).Write(string.Format(CultureInfo.InvariantCulture, format,
                                                            action, referenceNode, sourceContent));
        }

        private void RenderTop(string id, bool moved)
        {
            Render("IT", id, moved);
        }

        private void RenderBottom(string id, bool moved)
        {
            Render("IE", id, moved);
        }

        private void RenderAfter(string id, bool moved)
        {
            Render("IA", id, moved);
        }

        private void RenderBefore(string id, bool moved)
        {
            Render("IB", id, moved, true);
        }

        private void RenderReplaced(string id, bool moved)
        {
            Render("CRP", id, moved);
        }

        private void RenderControlChangesOnAjaxCallback(Stream stream, TextWriter textWriter)
        {
            HtmlTextWriter htmlTextWriter;
            CallbackWriter callbackWriter = null;

            if (_parentMoved || _moved)
            {
                callbackWriter = new MovedContainerHtmlTextWriter(textWriter, stream, _ajaxControl);
                htmlTextWriter = callbackWriter;
            }
            else if (!(_control is IAjaxControlNoRoot))
            {
                callbackWriter = new CallbackWriter(textWriter, stream, _ajaxControl);
                htmlTextWriter = callbackWriter;
            }
            else
                htmlTextWriter = new HtmlTextWriter(textWriter);


            using (htmlTextWriter)
            {
                foreach (Control child in _control.Controls)
                {
                    child.RenderControl(htmlTextWriter);
                    if (callbackWriter != null)
                        callbackWriter.Reset();
                }
            }

            if (callbackWriter == null) return;
            
            var unresolvedAjaxControls = callbackWriter.UnresolvedAjaxControls;
            if (unresolvedAjaxControls == null) return;
            
            foreach (var ajaxControl in unresolvedAjaxControls)
            {
                var resolved = ajaxControl.AjaxControl._resolved;
                if (!resolved.HasValue || resolved.Value) continue;
                RequireForceAnUpdate(_control);
                break;
            }
        }

        #endregion

        #region [ -- DRIMR helper methods -- ]

        /// <summary>
        /// Returns coordinates of the control on the page.
        /// </summary>
        /// <param name="useVirtualContainers">
        /// When true uses virtual containers to get virtual coordinates.
        /// </param>
        private List<int> GetCoordinates(bool useVirtualContainers = false)
        {
            var control = _control;
            var page = control.Page;
            var hasPage = page != null;

            var coordinates = new List<int>();
            if (hasPage)
            {
                for (var parent = control.Parent; parent != null; parent = parent.Parent)
                {
                    var index = GetContainer(control, useVirtualContainers).IndexOf(control);
                    if (index == -1) break;

                    coordinates.Add(index);
                    control = parent;
                }
            }

            if (!hasPage || control != page)
                coordinates.Add(-1);

            return coordinates;
        }

        /// <summary>
        /// Tries to find replacement for the owner control.
        /// </summary>
        private IAjaxControl GetReplacement()
        {
            var coordinates = GetCoordinates(true);

            foreach (var ajaxControl in Manager.RegisteredAjaxControls)
            {
                var impl = ajaxControl.AjaxControl;
                if (!impl.Removed || !DenoteSamePosition(impl._virtualCoordinates, coordinates)) continue;
                return ajaxControl;
            }

            return null;
        }

        /// <summary>
        /// Returns true if <paramref name="first"/> and <paramref name="second"/> have the same size
        /// and same elements. Otherwise return false.
        /// </summary>
        private static bool DenoteSamePosition(IList<int> first, IList<int> second)
        {
            // sizes should be same
            var count = first.Count;
            if (count != second.Count) return false;
            
            // elements should be same
            int index;
            for (index = 0; index < count && first[index] == second[index]; ++index) { }
            return index == count;
        }

        /// <summary>
        /// Returns true if all controls in the specified <paramref name="container"/>
        /// from <paramref name="start"/> to <paramref name="end"/> satisfy <see cref="IsEmptyLiteralControl"/>.
        /// </summary>
        private static bool AreAllEmptyLiteralControls(ControlCollection container, int start, int end)
        {
            int index;
            for (index = start; index < end && IsEmptyLiteralControl(container[index]); ++index) { }
            return index == end;
        }

        /// <summary>
        /// Returns true if specified <paramref name="control"/> is a <see cref="LiteralControl"/>,
        /// which does not have text or text is only whitespace which can be ignored.
        /// </summary>
        private static bool IsEmptyLiteralControl(Control control)
        {
            var literalControl = control as LiteralControl;
            return literalControl != null && literalControl.Text != null && string.IsNullOrEmpty(literalControl.Text.Trim());
        }

        #endregion

        private void InitializeAjaxControl()
        {
            _coordinates = GetCoordinates();
            _virtualCoordinates = GetCoordinates(true);

            ClientReference = _control.ClientID;
            StateManager = _ajaxControl.CreateControlStateManager();
            StateManager.TrackChanges();
            
            if (!TryLoadAjaxControlState(_extractedState, false))
            {
                throw new InvalidCastException(string.Empty,
                                               new HttpException("Ajax control state was not loaded successfully."));
            }

            // for hybrid controls we need to make sure that as soon as
            // parent control is initialized, composition controls are recreated
            // with latest possible state.
            var hybridControl = _control as HybridPanelBase;
            if (hybridControl != null)
            {
                hybridControl.CompositionControlsCreated = false;
                hybridControl.EnsureCompositionControls();
            }
        }

        /// <summary>
        /// Applies default skin to the controls implementing ISkinControl interface
        /// </summary>
        private void ApplyDefaultSkin()
        {
            if (!GaiaAjaxConfiguration.Instance.EnableDefaultTheme) return;
            var skinControl = _control as ISkinControl;
            if (skinControl == null || !skinControl.Enabled) return;

            var webControl = _control as System.Web.UI.WebControls.WebControl;
            if (webControl != null && !string.IsNullOrEmpty(webControl.CssClass)) return;

            skinControl.ApplySkin();
        }

        #region [ -- State related methods -- ]

        /// <summary>
        /// Saves ajax control state.        
        /// </summary>
        /// <remarks>
        /// Ajax control state is encoded based on following assumptions:
        /// 1. If the control is visible, then it rendered
        /// 2. If the control is invisible, but its parent is visible, then control is rendered invisible
        /// 3. If the control is invisible, but its parent is invisible, then control is not rendered
        /// 
        /// Resulting state is prepared so that it uses least possible bytes when encoded.
        /// </remarks>
        /// <param name="savedState">Saved state</param>
        /// <returns>Saved state for ajax control</returns>
        private object SaveState(object savedState)
        {
            object ajaxControlState;

            // we use three values here, which will be encoded 
            // into 1 byte by ObjectStateFormatter class.
            if (_control.Visible)
                ajaxControlState = 0;
            else if (_control.Parent.Visible)
                ajaxControlState = false;
            else
                ajaxControlState = string.Empty;

            // we need to save at least something, so that LoadViewState/LoadControlState is always triggered
            if (savedState == null)
                return ajaxControlState;

            // we always save the control state paired with the saved state,
            // except the default case, when the control is visible and the saved state is not ambiguous
            // meaning all the state information can be deduced during TryLoadState
            if (!_control.Visible || TryLoadAjaxControlState(savedState, true))
                return new Pair(savedState, ajaxControlState);

            return savedState;
        }

        /// <summary>
        /// Extracts ajax control state from specified saved state
        /// and returns saved state to pass to base state loading methods.
        /// </summary>
        /// <seealso cref="SaveState"/>
        /// <seealso cref="TryLoadAjaxControlState"/>
        /// <param name="savedState">Save state to load from.</param>
        /// <returns>Saved state to pass to base state loading methods.</returns>
        private object LoadState(object savedState)
        {
            _extractedState = null;
            var pair = savedState as Pair;
            if (pair == null)
            {
                if (TryLoadAjaxControlState(savedState, true))
                {
                    _extractedState = savedState;
                    return null;
                }
            }

            // check for the default case, when the control was visible
            // and we didn't encode any state information to save space
            if (pair == null || !TryLoadAjaxControlState(pair.Second, true))
            {
                _extractedState = 0;
                return savedState;
            }

            _extractedState = pair.Second;
            return pair.First;
        }

        /// <summary>
        /// Tries to load ajax control state from the specified saved state.
        /// </summary>
        /// <see cref="SaveState"/> for more info.
        /// <param name="savedState">Saved state to load from.</param>
        /// <param name="checkOnly">If true, only saved state is checked, otherwise the state is actually loaded.</param>
        /// <returns>True if the it was possible to load state from the specified saved state. Otherwise, false.</returns>
        private bool TryLoadAjaxControlState(object savedState, bool checkOnly)
        {
            if ((savedState is int) && ((int)savedState) == 0)
            {
                if (!checkOnly)
                {
                    Rendered = true;
                    StateManager.WasVisible = true;
                }

                return true;
            }

            if ((savedState is bool) && ((bool)savedState) == false)
            {
                if (!checkOnly)
                {
                    Rendered = true;
                    StateManager.WasVisible = false;
                }

                return true;
            }

            var str = savedState as string;
            if (str != null && str.Length == 0)
            {
                if (!checkOnly)
                {
                    Rendered = false;
                    StateManager.WasVisible = false;
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
