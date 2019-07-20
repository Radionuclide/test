using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opc.Ua;

namespace iba.ibaOPCServer
{
    public struct IbaOpcUaUserAccount
    {
        public UserTokenType TokenType;

        public string UserName; // applicable only for UserTokenType.UserName
        public string Password; // applicable only for UserTokenType.UserName

        public bool CanRead; // in mask = 0x1
        public bool CanWrite; // in mask = 0x2
        public bool CanBrowse; // in mask = 0x4
        public bool CanMonitor; // in mask = 0x8

        public bool CheckIntegrity()
        {
            if (TokenType == UserTokenType.Anonymous) return true; // anonymous can miss user name and psw 
            if (TokenType == UserTokenType.Certificate) return false; // this type is not supported
            if (TokenType == UserTokenType.IssuedToken) return false; // this type is not supported

            // TokenType == UserTokenType.UserName
            if (string.IsNullOrWhiteSpace(UserName)) return false;
            if (string.IsNullOrWhiteSpace(Password)) return false;
            // named user cannot have name "Anonymous"
            if (string.Compare(UserName, "Anonymous", StringComparison.OrdinalIgnoreCase) == 0) return false;
            return true;
        }

        /// <summary>
        /// Accepts a string that can be one of the follofing:
        /// "Full", "Default", "None" or a string containing an uint value representing a permissions mask, for example "7".
        /// Char Case is ignored.
        /// Returns true if string was successfully recognized, of false otherwise
        /// </summary>
        public bool PermissionsSetFromString(string permissionsString)
        {
            switch (permissionsString.ToUpper())
            {
                case "FULL":
                    PermissionsEnableAll();
                    return true;
                case "DEFAULT":
                    PermissionsMakeDefault();
                    return true;
                case "NONE":
                    PermissionsDisableAll();
                    return true;
                default:
                    uint permissionsMask;
                    if (! uint.TryParse(permissionsString, out permissionsMask)) return false;
                    PermissionsMask = permissionsMask;
                    return true;
            }
        }

        /// <summary>
        /// Gets or sets all access rights (CanXxxx) at once according to their binary value.
        /// see comment at Canxxx variables
        /// </summary>
        public uint PermissionsMask
        {
            get
            {
                uint mask = 0;
                if (CanRead) mask += 0x1;
                if (CanWrite) mask += 0x2;
                if (CanBrowse) mask += 0x4;
                if (CanMonitor) mask += 0x8;
                return mask;
            }
            set
            {
                CanRead = ((value & 0x1) != 0);
                CanWrite = ((value & 0x2) != 0);
                CanBrowse = ((value & 0x4) != 0);
                CanMonitor = ((value & 0x8) != 0);                
            }
        }
        public void PermissionsDisableAll()
        {
            CanBrowse = false;
            CanRead = false;
            CanWrite = false;
            CanMonitor = false;
        }
        public void PermissionsEnableAll()
        {
            CanBrowse = true;
            CanRead = true;
            CanWrite = true;
            CanMonitor = true;
        }

        public void PermissionsMakeDefault()
        {
            PermissionsDisableAll();
            CanBrowse = true;
            CanRead = true;
        }

        /// <summary>
        /// Returns 'Anonymous' for Anonymous token type,
        /// 'Invalid' for unsupporded token types or with bad integrity,
        /// and UserName for UserName token type
        /// </summary>
        /// <returns></returns>
        public string GetShortStringDescription()
        {
            if (!CheckIntegrity()) return "<Invalid>";
            if (TokenType == UserTokenType.Anonymous) return "<Anonymous>"; // anonymous can miss user name and psw 
            return UserName;
        }
        public override string ToString()
        {
            return $"{GetShortStringDescription()} r={CanRead} w={CanWrite} b={CanBrowse} m={CanMonitor}";
        }
    }
}
