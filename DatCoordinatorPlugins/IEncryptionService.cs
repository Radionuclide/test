using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iba.Plugins
{
    public interface IEncryptionService
    {
        string Encrypt(string msg);
        string Decrypt(string msg);
    }
}
