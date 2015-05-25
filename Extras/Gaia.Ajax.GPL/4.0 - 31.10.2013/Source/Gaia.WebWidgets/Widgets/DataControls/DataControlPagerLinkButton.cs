using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Widgets.DataControls
{
    [System.ComponentModel.ToolboxItem(false)]
    class DataControlPagerLinkButton : DataControlLinkButton
    {
        public DataControlPagerLinkButton(ASP.IPostBackContainer container) : base(container) { }
        
        protected internal override int Depth
        {
            get { return 6; }
        }
    }
}
