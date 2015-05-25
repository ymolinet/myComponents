/* 
 * Gaia Ajax Widgets, an Ajax Widget Library for ASP.NET 2.0
 * Copyright (C) 2007 - 2010 Gaiaware AS
 * All rights reserved.
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License version 3 as 
 * published by the Free Software Foundation.
 * 
 * Or if you have purchased the "Professional Edition" from Gaiaware
 * it is distributed to you in person under the Gaia Commercial License
 * which makes it possible for you to create closed source applications
 * and still be able to distribute those applications without creating 
 * restrictions for your source code.
 * 
 * Unless you're developing GPL software you should and probably legally
 * have to purchase a license of the Gaia Commercial License since otherwise
 * you might be forced to GPL license your derived works.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should find a copy of both the GPL version 3 and the 
 * Gaia Commercial License on disc where you have extracted these files.
 * If not visit http://www.gnu.org for the GPL version or 
 * http://ajaxwidgets.com for the Gaia Commercial License.
 * 
 * Breaking the terms of whatever license you're entitled to use will cause
 * prosecution by Gaiaware AS. If possible we will demand to settle any 
 * law suits at a court in Norway.
 * 
 */

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Concrete implementation of GPL License Validation
    /// </summary>
    internal class GaiaLicenseValidator : GaiaLicenseProvider
    {
        public const string GPL_COMMENT = @"



                          <!-- *************************************************************************************
                               This software is GPL Software. Since it is considered a distribution merely
                               since you are using it and it also is executing code linked against in the 
                               application itself on your system and therefore is a derived product you may 
                               ask the author for the source code for this application or product. If your 
                               request is not met, please send an email to support@gaiaware.net and if you 
                               like CC abuse@fsf.org ...
                          ****************************************************************************************** -->
            


            ";

        public override void Validate()
        {
            var page = System.Web.HttpContext.Current.CurrentHandler as System.Web.UI.Page;
            if (page == null) return;
            page.ClientScript.RegisterClientScriptBlock(typeof(GaiaLicenseValidator), "GPL_COMMENT", GPL_COMMENT, false);
        }
    }
}