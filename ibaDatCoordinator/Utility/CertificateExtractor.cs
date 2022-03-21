using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace iba.Utility
{
    internal class CertificateExtractor
    {
        public static string GetCertificate(X509Certificate2 cert)
        {
            var builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.RawData, Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        public static string GetPrivateKey(X509Certificate2 cert)
        {
            if (!cert.HasPrivateKey)
            {
                return String.Empty;
            }

            var rsa = cert.GetRSAPrivateKey();

            // Mark the private key as plaintext exportable, required for PEM format
            if (rsa is RSACng cng)
            {
                byte[] exportValue = { 0x02, 0x00, 0x00, 0x00 }; // 0x02 DWORD in little endian
                cng.Key.SetProperty(new CngProperty("Export Policy", exportValue, CngPropertyOptions.None));
            }

            var rsaKeyPair = DotNetUtilities.GetRsaKeyPair(rsa);
            using (var writer = new StringWriter())
            {
                var pemWriter = new PemWriter(writer);
                pemWriter.WriteObject(rsaKeyPair.Private);
                return writer.ToString();
            }
        }
    }
}
