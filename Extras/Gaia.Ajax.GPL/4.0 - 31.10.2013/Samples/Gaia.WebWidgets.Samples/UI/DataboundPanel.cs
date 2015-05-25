namespace Gaia.WebWidgets.Samples.UI
{
    using System;
    using System.Web.UI;
    using Gaia.WebWidgets;

    public class DataItemContainerPanel : Panel, IDataItemContainer
    {
        #region [-- IDataItemContainer implementation -- ]

        public object DataItem
        {
            get { return GetParentContainer().DataItem; }
        }

        public int DataItemIndex
        {
            get { return GetParentContainer().DataItemIndex; }
        }
        public int DisplayIndex
        {
            get { return GetParentContainer().DisplayIndex; }
        }
        
        #endregion

        IDataItemContainer GetParentContainer()
        {
            var parent = Parent;
            do
            {
                var container = parent as IDataItemContainer;
                if (container != null) return container;

                parent = parent.Parent;

            } while (parent != null);

            throw new InvalidOperationException("Unable to find a parent control that implements IDataItemContainer interface");
        }
    }
}
