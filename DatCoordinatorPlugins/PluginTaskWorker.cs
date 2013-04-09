using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Plugins
{
    /// <summary>
    /// give an instance of a class derived from this class to return worker status
    /// </summary>
    [Serializable]
    public class PluginTaskWorkerStatus
    {
        public bool started;
        public object extraData;
    }

    /// <summary>
    /// Interface implemented by a plugin task, worker part
    /// </summary>
    public interface IPluginTaskWorker
    {
        /// <summary>
        /// Function to be called when parent job is started
        /// </summary>
        /// <returns>true if succesfull, false otherwise</returns>
        bool OnStart();

        /// <summary>
        /// Function to be called when parent job is started
        /// </summary>
        /// <returns>true if succesfull, false otherwise</returns>
        bool OnStop();

        /// <summary>
        /// Function to be called when a change in jobconfiguration is applied to the running parent job
        /// Always copy parent jobdata and new taskdata in this method
        /// </summary>
        /// <returns>true if succesfull, false otherwise</returns>
        bool OnApply(IPluginTaskData newtask, IJobData newParentJob);

        /// <summary>
        /// Function to be called when the task should be executed
        /// </summary>
        /// <param name="datFile">datfile on which the task should be performed</param>
        /// <returns>true if successfull, false otherwise</returns>
        bool ExecuteTask(string datFile);

        /// <summary>
        /// Function to get a description of the error if either OnStart, OnStop or ExecuteTask failed
        /// </summary>
        /// <returns>null if no error occured, description of the error otherwise</returns>
        string GetLastError();


        PluginTaskWorkerStatus GetWorkerStatus();
            
    }

    public interface IPluginTaskWorkerUNC : IPluginTaskWorker
    {
        /// <summary>
        /// Function to be called when the task should be executed
        /// </summary>
        /// <param name="datFile">datfile on which the task should be performed</param>
        /// <param name="output">filename of the resulting file </param>
        /// <returns>true if successfull, false otherwise</returns>
        bool ExecuteTask(string datFile, string output);
    }
}
