using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Plugins
{
    /// <summary>
    /// Interface implemented by a plugin managing one or more custom tasks
    /// </summary>
    public interface IDatCoPlugin
    {
        /// <summary>
        /// Gets the info about the tasks that this plugin offers
        /// </summary>
        /// <returns>taskinfos</returns>
        PluginTaskInfo[] GetTasks();
        /// <summary>
        /// Property to get or set the callbackinterface to some usefull DatCoordinator routines
        /// </summary>
        IDatCoHost DatCoordinatorHost { get; set;}
        /// <summary>
        /// Create the plugin data object
        /// </summary>
        /// <param name="taskname">task identifier</param>
        /// <param name="parentjob">parent job of the custom task</param>
        /// <returns>The plugin data object</returns>
        IPluginTaskData CreateTask(string taskname, IJobData parentjob);
    }

	/// <summary>
	/// Interface used to indicate new plugin version
	/// </summary>
	public interface IDatCoPlugin2
	{
	}
}
