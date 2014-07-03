using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using XmlExtract;

namespace iba.TKS_XML_Plugin
{
    [Serializable]
    public class PluginXMLTask : IPluginTaskDataUNC, IPluginTaskDataIsSame, IExtractorData
    {
        public PluginXMLTask()
        {
            InitData(null, null, null);
        }

        public PluginXMLTask(string name, IDatCoHost host, IJobData parentJob)
        {
            InitData(name, host, parentJob);
        }

        private void InitData(string name, IDatCoHost host, IJobData parentJob)
        {
            m_parentJob = parentJob;
            m_datcoHost = host;
            m_nameInfo = name;
        }

        private IDatCoHost m_datcoHost;

        #region IPluginTaskDataUNC Members

        public string Extension
        {
            get { return ".xml"; }
        }

        #endregion

        #region IPluginTaskData Members

        [NonSerialized]
        private PluginXMLControl m_control;
        public IPluginControl GetControl()
        {
            if (m_control == null) m_control = new PluginXMLControl();
            return m_control;
        }

        [NonSerialized]
        private PluginXMLWorker m_worker;
        public IPluginTaskWorker GetWorker()
        {
            if (m_worker == null) m_worker = new PluginXMLWorker(this);
            return m_worker;
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
            m_worker = worker as PluginXMLWorker;
        }



        private string m_nameInfo;
        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
        }

        public int DongleBitPos
        {
            get
            {
                return 4;
            } 
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
            PluginXMLTask xt = new PluginXMLTask(m_nameInfo, m_datcoHost, null);
            xt.StandOrt = StandOrt;
            xt.IdField = IdField;
            return xt;
        }

        #endregion

        // IExtractorData Members
        private XmlExtract.StandortType _standOrt = XmlExtract.StandortType.DU;
        public XmlExtract.StandortType StandOrt
        {
            get { return _standOrt; }
            set { _standOrt = value; }
        }


        private IdFieldLocation _idField = IdFieldLocation.Name;
        public IdFieldLocation IdField
        {
            get { return _idField; }
            set { _idField = value; }
        }

        private string _andererStandort = String.Empty;
        public string AndererStandort
        {
            get { return _andererStandort; }
            set { _andererStandort = value; }
        }

        private string _xsiSchemaLocation = "http://www-eai/schema/qbic/Messung/REL-2_5/Messreihe.xsd";
        public string XmlSchemaLocation
        {
            get { return _xsiSchemaLocation; }
            set { _xsiSchemaLocation = value; }
        }

        private string _xsdLocation = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                             @"iba\ibaDatCoordinator\TKS-XmlExport\Messreihe.xsd");
        public string XsdLocation
        {
            get { return _xsdLocation; }
            set { _xsdLocation = value; }
        }


        #region IPluginTaskDataIsSame Members

        public bool IsSame(IPluginTaskDataIsSame data)
        {
            var other = data as PluginXMLTask;
            if (other == null) return false;

            return (other.StandOrt == StandOrt
                && other.IdField == IdField
                && other.AndererStandort == AndererStandort
                && other.XmlSchemaLocation == XmlSchemaLocation);
        }
        #endregion


    }
}
