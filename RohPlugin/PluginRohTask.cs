using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Alunorf_roh_plugin
{
    [Serializable]
    class PluginRohTask : IPluginTaskData
    {
        #region IPluginTaskData Members

        private iba.RohWriterInput m_rohInput;
        public iba.RohWriterInput RohInput
        {
            get { return m_rohInput; }
            set { m_rohInput = value; }
        }

        private string m_ftpUser;
        public string FtpUser
        {
            get { return m_ftpUser; }
            set { m_ftpUser = value; }
        }
        private string m_ftpPass;
        [XmlIgnore]
        public string FtpPassword
        {
            get { return m_ftpPass; }
            set { m_ftpPass = value; }
        }

        public string FtpPasswordCrypted
        {
            get { return Crypt.Encrypt(m_ftpPass); }
            set { m_ftpPass = Crypt.Decrypt(value); }
        }

        private string m_ftpHost;
        public string FtpHost
        {
            get { return m_ftpHost; }
            set { m_ftpHost = value; }
        }

        int m_ftpPort;
        public int FtpPort
        {
            get { return m_ftpPort; }
            set { m_ftpPort = value; }
        }
        

        private string m_ftpDirectory;
        public string FtpDirectory
        {
            get { return m_ftpDirectory; }
            set { m_ftpDirectory = value; }
        }

        [NonSerialized]
        private PluginRohControl m_control;
        public IPluginControl GetControl()
        {
            if (m_control == null) m_control = new PluginRohControl(m_datcoHost);
            return m_control;
        }

        [NonSerialized]
        private PluginRohWorker m_worker;
        public IPluginTaskWorker GetWorker()
        {
            if (m_worker == null) m_worker = new PluginRohWorker(this);
            return m_worker;
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
            m_worker = worker as PluginRohWorker;
        }

        public int DongleBitPos
        {
            get
            {
                return 3;
            } 
        }

        private IDatCoHost m_datcoHost;

        private string m_nameInfo;

        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
        }

        public void Reset(IDatCoHost host)
        {
            m_datcoHost = host;
        }

        private IJobData m_parentJob;
        public void SetParentJob(IJobData data)
        {
            m_parentJob = data;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            PluginRohTask rt = new PluginRohTask(m_nameInfo, m_datcoHost, null);
            rt.FtpDirectory = FtpDirectory;
            rt.FtpHost = FtpHost;
            rt.FtpUser = FtpUser;
            rt.FtpPassword = FtpPassword;
            rt.FtpPort = FtpPort;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, RohInput);
            ms.Flush();
            ms.Position = 0;
            rt.RohInput = bf.Deserialize(ms) as iba.RohWriterInput;
            return rt;
        }

        #endregion

         public PluginRohTask()
        {
            InitData(null, null, null);
        }

        public PluginRohTask(string name, IDatCoHost host, IJobData parentJob)
        {
            InitData(name, host, parentJob);
        }

        private void InitData(string name, IDatCoHost host, IJobData parentJob)
        {
            m_parentJob = parentJob;
            m_datcoHost = host;
            m_nameInfo = name;
        }
    }

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

    }
}
