using System;
using System.Collections.Generic;
using System.Text;
using iba.Utility;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public abstract class TaskDataUNC : TaskData
    {
        public TaskDataUNC(ConfigurationData parent) : base(parent) {
            m_pass = "";
            m_username = "";
            m_destinationMapUNC = "";
        }

        public TaskDataUNC() : this(null) { }

        protected string m_destinationMap;
        public string DestinationMap
        {
            get { return m_destinationMap; }
            set { m_destinationMap = value; }
        }

        protected string m_destinationMapUNC;
        public string DestinationMapUNC
        {
            get { return m_destinationMapUNC; }
            set { m_destinationMapUNC = value; }
        }

        public void UpdateUNC()
        {
            m_destinationMapUNC = Shares.PathToUnc(m_destinationMap, false);
        }

        protected string m_username;
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        protected string m_pass;

        [XmlIgnore]
        public string Password
        {
            get { return m_pass; }
            set { m_pass = value; }
        }

        public string PasswordCrypted
        {
            get { return Crypt.Encrypt(m_pass); }
            set { m_pass = Crypt.Decrypt(value); }
        }
    }
}
