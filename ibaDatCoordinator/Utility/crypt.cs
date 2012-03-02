using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Utility
{
    public class Crypt
    {
        static byte[] key = new byte[] { 12, 34, 179, 69, 231, 92 };

        public static string Encrypt(string msg)
        {
            if (msg == "")
                return msg;

            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("0x");
                System.Text.Encoding enc = new System.Text.UTF8Encoding();
                byte[] b = enc.GetBytes(msg);
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = (byte)(b[i] ^ key[i % key.Length]);
                    sb.Append(b[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
            }
            return msg;
        }

        public static string Decrypt(string msg)
        {
            if (!msg.StartsWith("0x"))
                return msg;

            try
            {
                msg = msg.Substring(2);
                byte[] b = new byte[msg.Length / 2];
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = Byte.Parse(msg.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                    b[i] = (byte)(b[i] ^ key[i % key.Length]);
                }
                System.Text.Encoding enc = new System.Text.UTF8Encoding();
                return enc.GetString(b);
            }
            catch (Exception)
            {
            }
            return msg;
        }

        public static bool CheckPassword()
        {
            return CheckPassword(null);
        }

        public static bool CheckPassword(System.Windows.Forms.Form parent)
        {
            string pass = null;
            try 
            {
                pass = iba.Processing.TaskManager.Manager.Password;
            }
            catch{}
            if (string.IsNullOrEmpty(pass)) return true;
            iba.Dialogs.PasswordConfirm dlg = new iba.Dialogs.PasswordConfirm(pass);
            dlg.StartPosition = (parent != null) ? System.Windows.Forms.FormStartPosition.CenterParent : System.Windows.Forms.FormStartPosition.CenterScreen;
            if (parent != null)
                dlg.ShowDialog(parent);
            else
                dlg.ShowDialog();
            return !dlg.Cancelled;
        }
    }
}
