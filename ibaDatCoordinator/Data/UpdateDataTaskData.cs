using System;
using System.Collections.Generic;
using System.Text;
using iba.Utility;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class UpdateDataTaskData : TaskDataUNC
    {
        public UpdateDataTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.updateDataTaskTitle;
            m_subfolderChoice = SubfolderChoice.NONE;
            m_dbName = "ibaDE";
            m_dbTblName = "dataTable";
            m_dbProvider = DbProviderEnum.MsSql;
            m_dbAuthenticateNT = true;
            m_dbNamedServer = false;
        }

        public UpdateDataTaskData()
            : this(null)
        {
        }

        public override TaskData CloneInternal()
        {
            UpdateDataTaskData ud = new UpdateDataTaskData(null);
            CopyUNCData(ud);
            ud.m_dbProvider = m_dbProvider;
            ud.m_dbAuthenticateNT = m_dbAuthenticateNT;
            ud.m_dbName = m_dbName;
            ud.m_dbNamedServer = m_dbNamedServer;
            ud.m_dbServer = m_dbServer;
            ud.m_dbUserName = m_dbUserName;
            ud.m_dbPass = m_dbPass;
            ud.m_dbTblName = m_dbTblName;
            return ud;
        }

        public enum DbProviderEnum {MsSql,Odbc,Oracle,Db2};
        private DbProviderEnum m_dbProvider;
        public DbProviderEnum DbProvider
        {
            get { return m_dbProvider; }
            set { m_dbProvider = value; }
        }

        private bool m_dbAuthenticateNT;
        public bool DbAuthenticateNT
        {
            get { return m_dbAuthenticateNT; }
            set { m_dbAuthenticateNT = value; }
        }

        private string m_dbName;
        public string DbName
        {
            get { return m_dbName; }
            set { m_dbName = value; }
        }

        private bool m_dbNamedServer;
        public bool DbNamedServer
        {
            get { return m_dbNamedServer; }
            set { m_dbNamedServer = value; }
        }

        private string m_dbServer;
        public string DbServer
        {
            get { return m_dbServer; }
            set { m_dbServer = value; }
        }

        private string m_dbUserName;
        public string DbUserName
        {
            get { return m_dbUserName; }
            set { m_dbUserName = value; }
        }

        private string m_dbPass;
        [XmlIgnore]
        public string DbPassword
        {
            get { return m_dbPass; }
            set { m_dbPass = value; }
        }
        public string DbPasswordCrypted
        {
            get { return Crypt.Encrypt(m_dbPass); }
            set { m_dbPass = Crypt.Decrypt(value); }
        }

        private string m_dbTblName;
        public string DbTblName
        {
            get { return m_dbTblName; }
            set { m_dbTblName = value; }
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            UpdateDataTaskData other = taskData as UpdateDataTaskData;
            if (other == null) return false;
            if (other == this) return true;
            if (!UNCDataIsSame(other)) return false;
            return
            other.m_dbProvider == m_dbProvider &&
            other.m_dbAuthenticateNT == m_dbAuthenticateNT &&
            other.m_dbName == m_dbName &&
            other.m_dbNamedServer == m_dbNamedServer &&
            other.m_dbServer == m_dbServer &&
            other.m_dbUserName == m_dbUserName &&
            other.m_dbPass == m_dbPass &&
            other.m_dbTblName == m_dbTblName;
        }

        public override int RequiredLicense => Licensing.LicenseId.UpdateData;
    }
}
