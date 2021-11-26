using iba.CertificateStore;
using iba.CertificateStore.Forms;
using iba.Utility;
using System;
using System.Collections.Generic;

namespace iba.Controls
{
    public class AppCertificatesControl : CertificatesControl
    {
        static AppCertificatesControl()
        {
            ApplicationName = "ibaDatCoordinator";
        }

        public AppCertificatesControl()
        {

        }
    }

    class AppCryptographer : ICertificateStoreCryptographer
    {
        public string Encrypt(string plain) => Crypt.Encrypt(plain);
        public string Decrypt(string cypher) => Crypt.Decrypt(cypher);
        public string EncryptWithTime(string plain) => Crypt.Encrypt(plain);
        public string DecryptWithTime(string cypher, TimeSpan timeSpan) => Crypt.Decrypt(cypher);
    }
}
