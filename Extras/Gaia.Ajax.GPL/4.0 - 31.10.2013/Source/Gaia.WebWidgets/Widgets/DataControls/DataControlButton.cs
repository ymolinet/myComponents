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
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Widgets.DataControls
{
    [System.ComponentModel.ToolboxItem(false)]
    class DataControlButton : Button
    {
        private readonly ASP.IPostBackContainer _container;

        public DataControlButton(ASP.IPostBackContainer container)
        {
            _container = container;
        }

        public override bool CausesValidation
        {
            get { return false; }
            set { throw new NotSupportedException(); }
        }

        public override bool UseSubmitBehavior
        {
            get { return false; }
            set { throw new NotSupportedException(); }
        }

        protected override PostBackOptions GetPostBackOptions()
        {
            if (_container != null)
            {
                var postBackOptions = _container.GetPostBackOptions(this);
                if (Page != null) postBackOptions.ClientSubmit = true;
                return postBackOptions;
            }
            return base.GetPostBackOptions();
        }
    }
}
