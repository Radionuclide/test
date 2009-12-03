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
    public class PluginRohTask : IPluginTaskData
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

        private string m_ftpDirectory;
        public string FtpDirectory
        {
            get { return m_ftpDirectory; }
            set { m_ftpDirectory = value; }
        }

        private string m_filePrefix;
        public string FilePrefix
        {
            get { return m_filePrefix; }
            set { m_filePrefix = value; }
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

        private string m_templateDatFile;
        public string TemplateDatFile
        {
            get { return m_templateDatFile; }
            set { m_templateDatFile = value; }
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
            rt.FilePrefix = FilePrefix;
            rt.SelectedTab = SelectedTab;
            rt.TemplateDatFile = TemplateDatFile;
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
            m_selectedTab = 0;
            m_rohInput = new iba.RohWriterInput();
            m_rohInput.Kurzbezeichner =
              "FA_NR                            AuftV___                          \r\n" +
              "STUECK_NR                        STnrV___                          \r\n" +
              "TEILBAND_NR                      TBnrV___                          \r\n" +
              "BANDBREITE                       breiVbd_                          \r\n" +
              "DICKE_SOLL                       Dik_SAL_                          \r\n" +
              "DICKE_EINLAUF                    Dik_SEL_                          \r\n" +
              "LEGIERUNG                        Leg_V___                          \r\n" +
              "DATUM                            DmUtIbS_                          \r\n" +
              "BUNDMASSE                        m___Vcl_                          \r\n" +
              "ANLAGE                           anlage                            \r\n" +
              "SYSTEM                           system                            \r\n" +
              "DATENART                         datenart                          \r\n" +
              "SPEICHER                         Permanen                          \r\n" +
              "QUALITAET                        QZPLMG2_                          \r\n";
            m_ftpDirectory = "";
            m_ftpHost = "";
            m_ftpUser = "";
            m_ftpPass = "";
            m_templateDatFile = "";
            m_filePrefix = "";
        }

        public string[] RohInputKommentareMultiLine
        {
            get 
            {
                if (m_rohInput == null || m_rohInput.Kommentare == null) return null;
                string[] splitted = m_rohInput.Kommentare.Replace("\r\n","\n").Split('\n');
                //count empty strings at the end
                int count = 0;
                for (int j = splitted.Length - 1; j >= 0; j--)
                {
                    if (String.IsNullOrEmpty(splitted[j])) count++;
                    else break;
                }
                if (count > 0) Array.Resize(ref splitted, splitted.Length - count);
                return splitted;
            }
            set
            {
                if (m_rohInput != null && m_rohInput.Kommentare != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in value)
                    {
                        sb.AppendLine(s);
                    }
                    m_rohInput.Kommentare = sb.ToString();
                }
            }
        }

        public string[] RohInputParameterMultiLine
        {
            get
            {
                if (m_rohInput == null || m_rohInput.Parameter == null) return null;
                string[] splitted = m_rohInput.Parameter.Replace("\r\n", "\n").Split('\n');
                //count empty strings at the end
                int count = 0;
                for (int j = splitted.Length - 1; j >= 0; j--)
                {
                    if (String.IsNullOrEmpty(splitted[j])) count++;
                    else break;
                }
                if (count > 0) Array.Resize(ref splitted, splitted.Length - count);
                return splitted;
            }
            set
            {
                if (m_rohInput != null && m_rohInput.Parameter != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in value)
                    {
                        sb.AppendLine(s);
                    }
                    m_rohInput.Parameter = sb.ToString();
                }
            }
        }

        public string[] RohInputKurzbezeichnerMultiLine
        {
            get
            {
                if (m_rohInput == null || m_rohInput.Kurzbezeichner == null) return null;
                string[] splitted = m_rohInput.Kurzbezeichner.Replace("\r\n", "\n").Split('\n');
                //count empty strings at the end
                int count = 0;
                for (int j = splitted.Length - 1; j >= 0; j--)
                {
                    if (String.IsNullOrEmpty(splitted[j])) count++;
                    else break;
                }
                if (count > 0) Array.Resize(ref splitted, splitted.Length - count);
                return splitted;
            }
            set
            {
                if (m_rohInput != null && m_rohInput.Kurzbezeichner != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in value)
                    {
                        sb.AppendLine(s);
                    }
                    m_rohInput.Kurzbezeichner = sb.ToString();
                }
            }
        }

        private int m_selectedTab;
        public int SelectedTab
        {
            get { return m_selectedTab; }
            set { m_selectedTab = value; }
        }

        static public int FindDataLine(iba.RohWriterDataLineInput[] rohWriterDataLineInputArray, iba.RohWriterDataLineInput rohWriterDataLineInput)
        {
            for (int i = 0; i < rohWriterDataLineInputArray.Length; i++)
                if (rohWriterDataLineInput == rohWriterDataLineInputArray[i])
                    return i + 1;
            return -1;
        }

        static public int FindChannelLine(iba.RohWriterChannelLineInput[] rohWriterChannelLineInputArray, iba.RohWriterChannelLineInput rohWriterChannelLineInput)
        {
            for (int i = 0; i < rohWriterChannelLineInputArray.Length; i++)
                if (rohWriterChannelLineInput == rohWriterChannelLineInputArray[i])
                    return i + 1;
            return -1;
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
