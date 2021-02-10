using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Plugins
{

    /// <summary>
    /// Interface implemented by a plugin task, data part
    /// Implementation must have the Serializable attribute
    /// When saving or loading a datcoordinator configuration, the datcoordinator
    /// will try to serialise the plugin with an XMLSerializer. If this in problematic,
    /// derive your plugin also from IXmlSerializable to write your own XMLSerialization
    /// </summary>
    public interface IPluginTaskData : ICloneable
    {
        /// <summary>
        /// Get GUI Control 
        /// If the resulting class is not a Control, 
        /// this will result in error
        /// </summary>
        IPluginControl GetControl();

        /// <summary>
        /// Function to get the worker part of the plugin
        /// </summary>
        IPluginTaskWorker GetWorker();
        void SetWorker(IPluginTaskWorker worker);

        /// <summary>
        /// Index to retrieve info from pluginmanager after serialisation
        /// </summary>
        string NameInfo
        {
            get;
        }

        /// <summary>
        /// What dongle bit needs to be checked
        /// </summary>
        int DongleBitPos
        {
            get;
        }

        /// <summary>
        /// Function to reset the PluginTask after serialisation
        /// </summary>
        /// <param name="info">information to set</param>
        void Reset(IDatCoHost host);
        
        /// <summary>
        /// Function to set the parent job data
        /// </summary>
        /// <param name="data">the parent jobconfiguration to set</param>
        void SetParentJob (IJobData data);
    }

	/// <summary>
	/// Interface for plugins which uses analyzer tree to select signals
	/// We use SetGridAnalyzer method to avoid direct reference to ibaDatCoordinator
	/// </summary>
	public interface IGridAnalyzer
	{
		/// <summary>
		/// Method should be called before CreateTask call
		/// </summary>
		/// <param name="editor">DevExpress tree editor - RepositoryItemChannelTreeEdit </param>
		/// <param name="analyzer">AnalyzerManager class </param>
		void SetGridAnalyzer(DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit editor, IAnalyzerManagerUpdateSource analyzer);
	}

	/// <summary>
	/// Interface to use AnalyzerManager without reference to ibaDatCoordinator
	/// </summary>
	public interface IAnalyzerManagerUpdateSource
	{       
		/// <summary>
		/// Use this method to set .pdo and .dat files to Analyzer tree
		/// </summary>
		void UpdateSource(string pdoFile, string datFile, string datFilePassword);

		void UnLoadAnalysis();

		void ReopenAnalysis();
	}


	/// <summary>
	/// Interface implemented by a plugin task, data part in the case that the plugin needs
	/// to use the datcoordinator network shares handling functionallity
	/// Implementation must have the Serializable attribute
	/// </summary>
	public interface IPluginTaskDataUNC : IPluginTaskData
    {
        /// <summary>
        /// Extention of the generated output file, e.g.: ".xml"
        /// </summary>
        string Extension
        {
            get;
        }
    }

    /// <summary>
    /// Implement this interface to provide a way to compare besides binary compare
    /// </summary>
    public interface IPluginTaskDataIsSame
    {
        /// <summary>
        /// Test contents of two instances for equality
        /// <param name="data">other instance to compare with</param>
        /// <returns>true if same</returns>
        bool IsSame(IPluginTaskDataIsSame data);
    }

    /// <summary>
    /// Implement this interface to indicate you require ibaAnalyzer
    /// </summary>
    public interface IPluginTaskDataIbaAnalyzer
    {
        /// <summary>
        /// True if task uses an analysis (that needs to be closed on error)
        /// </summary>
        bool UsesAnalysis
        {
            get;
        }
        void SetIbaAnalyzer(IbaAnalyzer.IbaAnalyzer Analyzer, iba.Processing.IIbaAnalyzerMonitor Monitor);

        /// <summary>
        /// Get the parameters specified for the ibaAnalyzer monitor (memory limit, time limit)
        /// </summary>
        Data.MonitorData MonitorData { get;  }
    }
}
