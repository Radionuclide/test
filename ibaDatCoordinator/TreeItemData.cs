using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

using iba.Controls;
using iba.Data;
using iba.Utility;

namespace iba
{

	#region TreeItemData
	abstract public class TreeItemData
	{
		public TreeItemData(IPropertyPaneManager propManager)
		{
            manager = propManager;
		}

        protected IPropertyPaneManager manager;
        public virtual Control CreateControl() {return null;}
        public abstract string What { get; }
        public virtual object GetDataSource()
        {
            return null;
        }

        //public virtual void SetupPopupMenu(ArrayList items)
        //{
        //}
	}
	#endregion

	#region NewConfigurationTreeItemData
    public class NewConfigurationTreeItemData : TreeItemData
	{
        public NewConfigurationTreeItemData(IPropertyPaneManager propManager) : base(propManager) {}
        public override string What
        {
            get { return "NewConfigurationTreeItemData"; }
        }
    }
	#endregion

	#region ConfigurationTreeItemData
    public class ConfigurationTreeItemData : TreeItemData
	{
		public ConfigurationTreeItemData(IPropertyPaneManager propManager, ConfigurationData conf) 
            : base(propManager)
        {
            m_conf = conf;
		}

        public override string What
        {
            get { return "Configuration"; }
        }

        protected ConfigurationData m_conf;

        public ConfigurationData ConfigurationData
		{
			get {return m_conf;}
            set { m_conf = value; }
		}

		public override Control CreateControl()
		{
			Control ctrl = manager.PropertyPanes["ConfigurationControl"] as Control;
            //this code
            //if(ctrl == null)
            //{
            //    ctrl = new ConfigurationControl();
            //    manager.PropertyPanes["ConfigurationControl"] = ctrl;
            //}
			return ctrl;
		}
	}
	#endregion

    #region ReportTreeItemData
    public class ReportTreeItemData : TreeItemData
    {
        public ReportTreeItemData(IPropertyPaneManager propManager, ReportData rep)
            : base(propManager)
        {
            m_rep = rep;
        }

        public override string What
        {
            get { return "Report"; }
        }

        protected ReportData m_rep;

        public ReportData ReportData
        {
            get { return m_rep; }
            set { m_rep = value; }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["ReportControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new ReportControl();
                manager.PropertyPanes["ReportControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region CopyTaskTreeItemData
    public class CopyTaskTreeItemData : TreeItemData
    {
        public CopyTaskTreeItemData(IPropertyPaneManager propManager, CopyMoveTaskData cop)
            : base(propManager)
        {
            m_cop = cop;
        }

        public override string What
        {
            get { return "CopyTask"; }
        }

        protected CopyMoveTaskData m_cop;

        public CopyMoveTaskData CopyTaskData
        {
            get { return m_cop; }
            set { m_cop = value; }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["CopyTaskControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CopyControl();
                manager.PropertyPanes["CopyTaskControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region ExtractTreeItemData
    public class ExtractTreeItemData : TreeItemData
    {
        public ExtractTreeItemData(IPropertyPaneManager propManager, ExtractData ext)
            : base(propManager)
        {
            m_ext = ext;
        }

        public override string What
        {
            get { return "Extract"; }
        }

        protected ExtractData m_ext;

        public ExtractData ExtractData
        {
            get { return m_ext; }
            set { m_ext = value; }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["ExtractControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new ExtractControl();
                manager.PropertyPanes["ExtractControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region BatchFileTreeItemData
    public class BatchFileTreeItemData : TreeItemData
    {
        public BatchFileTreeItemData(IPropertyPaneManager propManager, BatchFileData bat)
            : base(propManager)
        {
            m_bat = bat;
        }

        public override string What
        {
            get { return "BatchFile"; }
        }

        protected BatchFileData m_bat;

        public BatchFileData BatchFileData
        {
            get { return m_bat; }
            set { m_bat=value; }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["BatchFileControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new BatchFileControl();
                manager.PropertyPanes["BatchFileControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion


    #region StatusTreeItemData
    public class StatusTreeItemData : TreeItemData
    {
        public StatusTreeItemData(IPropertyPaneManager propManager, StatusData stat)
            : base(propManager)
        {
            m_stat = stat;
        }

        public override string What
        {
            get { return "Status"; }
        }

        protected StatusData m_stat;

        public StatusData StatusData
        {
            get { return m_stat; }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["StatusControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new StatusControl();
                manager.PropertyPanes["StatusControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

}
