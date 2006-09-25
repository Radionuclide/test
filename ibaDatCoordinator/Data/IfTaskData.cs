using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Data
{
    [Serializable]
    public class IfTaskData : TaskData
    {
        private string m_expression;
        public string Expression
        {
            get { return m_expression; }
            set { m_expression = value; }
        }

        private string m_testDatFile;
        public string TestDatFile
        {
            get { return m_testDatFile; }
            set { m_testDatFile = value; }
        }

        public enum XTypeEnum 
        {
            XTimeBased=0,
            XLengthBased=1,
            XFrequencyBased=2,
            XInvLengthBased=3
        };

        private XTypeEnum m_xtype;
        public XTypeEnum XType
        {
            get { return m_xtype; }
            set { m_xtype = value; }
        }



        public IfTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.iftaskTitle;
            m_testDatFile = String.Empty;
            m_expression = String.Empty;
            m_xtype = 0;
        }

        public IfTaskData()
            : this(null)
        {

        }

        public override object Clone()
        {
            IfTaskData ifd = new IfTaskData(null);
            ifd.m_name = m_name.Clone() as string;
            ifd.m_xtype = m_xtype;
            ifd.m_wtodo = m_wtodo;
            ifd.m_pdoFile = m_pdoFile.Clone() as string;
            ifd.m_testDatFile = m_testDatFile.Clone() as string;
            ifd.m_notify = m_notify;
            ifd.m_expression = m_expression;
            return ifd;
        }
    }
}
