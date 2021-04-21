using System;
using System.Diagnostics;

namespace iba.Utility
{
    class Crypt
    {
        static iba.Plugins.IEncryptionService encryptionService;

        public static void InitializeKeys(byte[] xorKey, byte[] aesKey)
        {
            encryptionService = new EncryptionService(xorKey, aesKey);
        }

        public static string Encrypt(string msg)
        {
            Debug.Assert(encryptionService != null);
            return encryptionService.Encrypt(msg);
        }

        public static string Decrypt(string msg)
        {
            Debug.Assert(encryptionService != null);
            return encryptionService.Decrypt(msg);
        }

        public static bool CheckPassword()
        {
            return CheckPassword(null);
        }


        private static string LastSpecifiedPass = null;
        private static DateTime LastTime = DateTime.MinValue;

        public static bool CheckPassword(System.Windows.Forms.Form parent)
        {
            string pass = null;
            TimeSpan rememberTime = TimeSpan.MinValue;
            bool doRemember = false;
            try 
            {
                pass = iba.Processing.TaskManager.Manager.Password;
                rememberTime = iba.Processing.TaskManager.Manager.RememberPassTime;
                doRemember = iba.Processing.TaskManager.Manager.RememberPassEnabled;
            }
            catch{}

            if (string.IsNullOrEmpty(pass) || 
                (LastSpecifiedPass != null && doRemember && pass == LastSpecifiedPass && Math.Abs((DateTime.UtcNow - LastTime).TotalSeconds) < rememberTime.TotalSeconds))
                return true;

            using (iba.Dialogs.PasswordConfirm dlg = new iba.Dialogs.PasswordConfirm(pass))
            {
                dlg.StartPosition = (parent != null) ? System.Windows.Forms.FormStartPosition.CenterParent : System.Windows.Forms.FormStartPosition.CenterScreen;
                if (parent != null)
                    dlg.ShowDialog(parent);
                else
                    dlg.ShowDialog();
                if (!dlg.Cancelled)
                {
                    if (doRemember)
                    {
                        LastSpecifiedPass = pass;
                        LastTime = DateTime.UtcNow;
                    }
                    return true;
                }
                return false;
            }
            
        }
    }
}
