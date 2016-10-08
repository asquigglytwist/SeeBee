using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace SeeBee.FxUtils.AuthentiCode
{
    public static class SignerInfo
    {
        // [BIB]:  http://stackoverflow.com/questions/28556981/c-sharp-how-to-get-dll-or-exe-files-digital-signer-certificate-info-even-the-c
        public static X509Certificate GetSignerInfo(string inputBinary)
        {
            return X509Certificate.CreateFromSignedFile(inputBinary);
        }

        public static bool IsSignedBy(string inputBinary, string expectedSignerName, bool exactMatch = false)
        {
            var cert = GetSignerInfo(inputBinary);
            bool meetsSigningRequirements = (exactMatch ? cert.Issuer.Equals(expectedSignerName) : cert.Issuer.Contains(expectedSignerName));
            return meetsSigningRequirements;
        }
    }
}
