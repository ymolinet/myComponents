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
    class DataControlImageButton : ImageButton
    {
        private readonly ASP.IPostBackContainer _container;

        public DataControlImageButton(ASP.IPostBackContainer container)
        {
            _container = container;
        }

        public override bool CausesValidation
        {
            get { return false; }
            set { throw new NotSupportedException(); }
        }

        protected override PostBackOptions GetPostBackOptions()
        {
            return _container != null ? _container.GetPostBackOptions(this) : base.GetPostBackOptions();
        }
    }
}
