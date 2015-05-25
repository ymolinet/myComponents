namespace Gaia.WebWidgets.Samples.Extensions.Scheduler.MonthView
{
    using System;
    using System.Web.UI;
    using ASP = System.Web.UI.WebControls;

    class ResourceControl : ASP.CompositeControl
    {
        private const string NonBreakingSpace = "&nbsp;";
        private const string ViewStateIndexKey = "index";

        public Resource Resource { get; set; }

        public event EventHandler SelectionChanged;

        void RaiseSelectionChanged()
        {
            var handler = SelectionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public bool Selected
        {
            get { return (bool) (ViewState["selected"] ?? false); }
            set { ViewState["selected"] = value; }
        }

        public int Index
        {
            get { return (int)(ViewState[ViewStateIndexKey] ?? 0); }
            set { ViewState[ViewStateIndexKey] = value; }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            // no root element, just render the children
            RenderContents(writer);
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            if (Resource == null)
                throw new ArgumentNullException("You must set the appropriate Resource for this Control");

            var wrap = new Panel();
            wrap.ID = "w";
            wrap.CssClass = "resource";

            var icon = new Label
                           {
                               ID = "icon",
                               CssClass = "ico " + Resource.CssClass,
                               Text = NonBreakingSpace
                           };

            wrap.Controls.Add(icon);

            var chk = new CheckBox();
            chk.AutoPostBack = false;
            chk.ID = "chk";
            chk.Text = NonBreakingSpace;
            chk.Checked = Selected;
            chk.CheckedChanged += delegate
                                      {
                                          Selected = chk.Checked;
                                          RaiseSelectionChanged();
                                          ChildControlsCreated = false;
                                      };
            chk.Text = Resource.Name;
            wrap.Controls.Add(chk);

            Controls.Add(wrap);
        }
    }
}