using System;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace myComponents.Utilities
{
    public static class myConvert
    {
        public static SecureString ToSecureString(String str)
        {
            SecureString sstr = new SecureString();
            foreach (char x in str)
                sstr.AppendChar(x);
            return sstr;
        }
    }
}
