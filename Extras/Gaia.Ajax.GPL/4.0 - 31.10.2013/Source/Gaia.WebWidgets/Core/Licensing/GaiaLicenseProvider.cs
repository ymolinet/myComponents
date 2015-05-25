/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets
{
    /// <summary>
    /// The Gaia Licensing Provider validates if Gaia is running under a valid license. It follows a simple factory 
    /// pattern where we use different implementations based on file. Currently Gaia supports three modes. 
    /// 1) Commercial 
    /// 2) Trial
    /// 3) GPL
    /// </summary>
    internal abstract class GaiaLicenseProvider
    {        
        private static volatile GaiaLicenseProvider _instance;

        public static GaiaLicenseProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(GaiaLicenseProvider))
                    {
                        if (_instance == null)
                        {
                            _instance = new GaiaLicenseValidator();
                       }
                    }
                }

                return _instance;
            }
        }

        public abstract void Validate();
    }
}
