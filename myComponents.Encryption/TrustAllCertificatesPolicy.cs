using System;
using System.Collections.Generic;
using System.Text;

namespace myComponents.Crypto
{
    public class TrustAllCertificatesPolicy : System.Net.ICertificatePolicy
    {
        public bool CheckValidationResult(System.Net.ServicePoint sp,
            System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Net.WebRequest req, int problem)
        {
            return true;
        }
    }
}
