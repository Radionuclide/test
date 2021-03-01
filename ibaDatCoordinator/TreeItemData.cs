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

        public virtual object DataSource { get { return null; } set { } }
	}
	#endregion

    #region NewConfigurationTreeItemDataBase
    public class NewConfigurationTreeItemDataBase : TreeItemData
    {
         public NewConfigurationTreeItemDataBase(IPropertyPaneManager propManager) : base(propManager) {}
         public override string What
         {
             get { return "NewConfigurationTreeItemDataBase"; }
         }
    }
    #endregion

    #region NewConfigurationTreeItemData
    public class NewConfigurationTreeItemData : NewConfigurationTreeItemDataBase
	{
        public NewConfigurationTreeItemData(IPropertyPaneManager propManager) : base(propManager) {}
        public override string What
        {
            get { return "NewConfigurationTreeItemData"; }
        }
    }
	#endregion

    #region NewOneTimeConfigurationTreeItemData
    public class NewOneTimeConfigurationTreeItemData : NewConfigurationTreeItemDataBase
    {
        public NewOneTimeConfigurationTreeItemData(IPropertyPaneManager propManager) : base(propManager) { }
        public override string What
        {
            get { return "NewOneTimeConfigurationTreeItemData"; }
        }
    }
    #endregion

    #region NewScheduledConfigurationTreeItemData
    public class NewScheduledConfigurationTreeItemData : NewConfigurationTreeItemDataBase
    {
        public NewScheduledConfigurationTreeItemData(IPropertyPaneManager propManager) : base(propManager) { }
        public override string What
        {
            get { return "NewScheduledConfigurationTreeItemData"; }
        }
    }
    #endregion

    #region NewEventConfigurationTreeItemData
    public class NewEventConfigurationTreeItemData : NewConfigurationTreeItemDataBase
    {
        public NewEventConfigurationTreeItemData(IPropertyPaneManager propManager) : base(propManager) { }
        public override string What
        {
            get { return "NewEventConfigurationTreeItemData"; }
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

        public override object DataSource
        {
            get
            {
                return m_conf;
            }
            set
            {
                m_conf = value as ConfigurationData;
            }
        }

        public ConfigurationData ConfigurationData
        {
            get
            {
                return m_conf;
            }
            set
            {
                m_conf = value;
            }
        }

		public override Control CreateControl()
		{
            string what = null;
            switch(m_conf.JobType)
            {
                case Data.ConfigurationData.JobTypeEnum.DatTriggered:
                    what = "ConfigurationControl"; break;
                case Data.ConfigurationData.JobTypeEnum.OneTime:
                    what = "OneTimeConfigurationControl"; break;
                case Data.ConfigurationData.JobTypeEnum.Scheduled:
                    what = "ScheduledConfigurationControl"; break;
                case Data.ConfigurationData.JobTypeEnum.Event:
                    what = "EventConfigurationControl"; break;
            }
            Control ctrl = manager.PropertyPanes[what] as Control;
            if (ctrl == null)
            {
                ctrl = new ConfigurationControl(m_conf.JobType);
                manager.PropertyPanes[what] = ctrl;
            }
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

        public override object DataSource
        {
            get
            {
                return m_rep;
            }
            set
            {
                m_rep = value as ReportData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["ReportControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new ReportControl());
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

        public override object DataSource
        {
            get
            {
                return m_cop;
            }
            set
            {
                m_cop = value as CopyMoveTaskData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["CopyTaskControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new CopyControl());
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

        public override object DataSource
        {
            get
            {
                return m_ext;
            }
            set
            {
                m_ext = value as ExtractData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["ExtractControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new ExtractControl());
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

        public override object DataSource
        {
            get
            {
                return m_bat;
            }
            set
            {
                m_bat = value as BatchFileData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["BatchFileControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new BatchFileControl());
                manager.PropertyPanes["BatchFileControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region IfTaskTreeItemData
    public class IfTaskTreeItemData : TreeItemData
    {
        public IfTaskTreeItemData(IPropertyPaneManager propManager, IfTaskData ift)
            : base(propManager)
        {
            m_ift = ift;
        }

        public override string What
        {
            get { return "IfTask"; }
        }

        protected IfTaskData m_ift;

        public override object DataSource
        {
            get
            {
                return m_ift;
            }
            set
            {
                m_ift = value as IfTaskData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["IfTaskControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new IfTaskControl());
                manager.PropertyPanes["IfTaskControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region UpdateDataTaskTreeItemData
    public class UpdateDataTaskTreeItemData : TreeItemData
    {
        public UpdateDataTaskTreeItemData(IPropertyPaneManager propManager, UpdateDataTaskData udt)
            : base(propManager)
        {
            m_udt = udt;
        }

        public override string What
        {
            get { return "UpdateDataTask"; }
        }

        protected UpdateDataTaskData m_udt;

        public override object DataSource
        {
            get
            {
                return m_udt;
            }
            set
            {
                m_udt = value as UpdateDataTaskData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["UpdateDataTaskControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new UpdateDataTaskControl());
                manager.PropertyPanes["UpdateDataTaskControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region PauseTaskTreeItemData
    public class PauseTaskTreeItemData : TreeItemData
    {
        public PauseTaskTreeItemData(IPropertyPaneManager propManager, PauseTaskData pt)
            : base(propManager)
        {
            m_pt = pt;
        }

        public override string What
        {
            get { return "PauseTask"; }
        }

        protected PauseTaskData m_pt;

        public override object DataSource
        {
            get
            {
                return m_pt;
            }
            set
            {
                m_pt = value as PauseTaskData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["PauseTaskControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new PauseTaskControl());
                manager.PropertyPanes["PauseTaskControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region SplitterTaskTreeItemData
    public class SplitterTaskTreeItemData : TreeItemData
    {
        public SplitterTaskTreeItemData(IPropertyPaneManager propManager, SplitterTaskData pt)
            : base(propManager)
        {
            m_st = pt;
        }

        public override string What
        {
            get { return "SplitterTask"; }
        }

        protected SplitterTaskData m_st;

        public override object DataSource
        {
            get
            {
                return m_st;
            }
            set
            {
                m_st = value as SplitterTaskData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["SplitterTaskControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new SplitterTaskControl());
                manager.PropertyPanes["SplitterTaskControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region CleanupTaskTreeItemData
    public class CleanupTaskTreeItemData : TreeItemData
    {
        public CleanupTaskTreeItemData(IPropertyPaneManager propManager, TaskWithTargetDirData tdc)
            : base(propManager)
        {
            m_tdc = tdc;
        }

        public override string What
        {
            get { return "CleanupTask"; }
        }

        protected TaskWithTargetDirData m_tdc;

        public override object DataSource
        {
            get
            {
                return m_tdc;
            }
            set
            {
                m_tdc = value as TaskWithTargetDirData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["CleanupTaskControl"] as Control;
            if(ctrl == null)
            {
                ctrl = new CommonTaskControl(new CleanupTaskControl());
                manager.PropertyPanes["CleanupTaskControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region CustomTaskTreeItemData
    public class CustomTaskTreeItemData : TreeItemData
    {
        public CustomTaskTreeItemData(IPropertyPaneManager propManager, ICustomTaskData cust)
            : base(propManager)
        {
            m_cust = cust;
        }

        public override string What
        {
            get { return (m_cust is CustomTaskDataUNC)?"CustomTaskUNC": "CustomTask"; }
        }

        protected ICustomTaskData m_cust;

        public override object DataSource
        {
            get
            {
                return m_cust;
            }
            set
            {
                m_cust = value as ICustomTaskData;
            }
        }

        public override Control CreateControl()
        {
            string id = m_cust.Plugin.NameInfo + "Control";
            Control ctrl = manager.PropertyPanes[id] as Control;
            if (ctrl == null)
            {
                if (m_cust.Plugin is ErrorPluginTaskData)
                    ctrl = m_cust.Plugin.GetControl() as Control;
                else
                {
                    var plugin = m_cust.Plugin;
                    if (plugin is Plugins.IGridAnalyzer gridPlugin)
                    {
                        var analyzer = new AnalyzerManager();
                        gridPlugin.SetGridAnalyzer(new RepositoryItemChannelTreeEdit(analyzer, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Infofields), analyzer);
                    }

                    if (m_cust is CustomTaskDataUNC)
                        ctrl = new CommonTaskControl(new CustomUNCTaskControl(m_cust.Plugin.GetControl() as iba.Plugins.IPluginControlUNC));
                    else
                        ctrl = new CommonTaskControl(plugin.GetControl() as Control);
                }

                manager.PropertyPanes[id] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion

    #region HDCreateEventTaskTreeItemData
    public class HDCreateEventTaskTreeItemData : TreeItemData
    {
        public HDCreateEventTaskTreeItemData(IPropertyPaneManager propManager, HDCreateEventTaskData cet)
            : base(propManager)
        {
            m_cet = cet;
        }

        public override string What
        {
            get { return "HDCreateEventTask"; }
        }

        protected HDCreateEventTaskData m_cet;

        public override object DataSource
        {
            get
            {
                return m_cet;
            }
            set
            {
                m_cet = value as HDCreateEventTaskData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["HDEventCreationTaskControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new CommonTaskControl(new HDEventCreationTaskControl());
                manager.PropertyPanes["HDEventCreationTaskControl"] = ctrl;
            }
            return ctrl;
        }
    }
	#endregion


	#region OPCUAWriterTaskTreeItemData
	public class OPCUAWriterTaskTreeItemData : TreeItemData
	{
		public OPCUAWriterTaskTreeItemData(IPropertyPaneManager propManager, OPCUAWriterTaskData tdc)
			: base(propManager)
		{
			m_tdc = tdc;
		}

		public override string What
		{
			get { return "OPCUAWriterTask"; }
		}

		protected OPCUAWriterTaskData m_tdc;

		public override object DataSource
		{
			get
			{
				return m_tdc;
			}
			set
			{
				m_tdc = value as OPCUAWriterTaskData;
			}
		}

		public override Control CreateControl()
		{
			Control ctrl = manager.PropertyPanes["OPCUAWriterTaskControl"] as Control;
			if (ctrl == null)
			{
				ctrl = new CommonTaskControl(new OPCUAWriterTaskControl());
				manager.PropertyPanes["OPCUAWriterTaskControl"] = ctrl;
			}
			return ctrl;
		}
	}
	#endregion

	#region StatusTreeItemData
	public class StatusTreeItemData : TreeItemData
    {
        public StatusTreeItemData(IPropertyPaneManager propManager, ConfigurationData dat)
            : base(propManager)
        {
            m_conf = dat;
        }

        public override string What
        {
            get { return "Status"; }
        }

        protected ConfigurationData m_conf;

        public ConfigurationData CorrConfigurationData
        {
            get { return m_conf; }
        }

        public override object DataSource
        {
            get
            {
                return m_conf;
            }
            set
            {
                m_conf = value as ConfigurationData;
            }
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

    #region StatusPermanentlyErrorFilesTreeItemData
    public class StatusPermanentlyErrorFilesTreeItemData : TreeItemData
    {
        public StatusPermanentlyErrorFilesTreeItemData(IPropertyPaneManager propManager, ConfigurationData dat)
            : base(propManager)
        {
            m_conf = dat;
        }

        public override string What
        {
            get { return "PermanentlyErrorFiles"; }
        }

        protected ConfigurationData m_conf;

        public ConfigurationData CorrConfigurationData
        {
            get { return m_conf; }
        }

        public override object DataSource
        {
            get
            {
                return m_conf;
            }
            set
            {
                m_conf = value as ConfigurationData;
            }
        }

        public override Control CreateControl()
        {
            Control ctrl = manager.PropertyPanes["StatusPermanentlyErrorFilesControl"] as Control;
            if (ctrl == null)
            {
                ctrl = new PermanentFileErrorsControl();
                manager.PropertyPanes["StatusPermanentlyErrorFilesControl"] = ctrl;
            }
            return ctrl;
        }
    }
    #endregion
}
