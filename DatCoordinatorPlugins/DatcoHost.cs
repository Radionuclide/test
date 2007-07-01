using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Plugins
{
    /// <summary>
    /// Interface to usefull DatCoordinator routines, mostly regarding network shares
    /// </summary>
    public interface IDatCoHost
    {
        /// <summary>
        /// If an IPluginTaskData needs, due to network problems, have the datcoordinator reconnect to a shared network path 
        /// (which it has registered first with it's UNCPaths method) it can use this method;
        /// </summary>
        /// <param name="path">path on the share that needs reconnecting</param>
        /// <param name="username">user name to access the share</param>
        /// <param name="pass">password to access the share</param>
        /// <returns></returns>
        bool TryReconnect(string path, string username, string pass);
        /// <summary>
        /// Transform a path to an UNC path
        /// </summary>
        /// <param name="path">Path to transform</param>
        /// <param name="convertLocalPath">Indicates whether a local path needs to be transformed as well, or left intact</param>
        /// <returns>The UNC path</returns>
        string PathToUnc(string path, bool convertLocalPath);
        /// <summary>
        /// Ask the datcoordinator to check if the path is a valid UNC path given login information
        /// </summary>
        /// <param name="username">user name to access the share</param>
        /// <param name="path">path to check</param>
        /// <param name="pass">password to access the share</param>
        /// <param name="errormessage">if there was an error, the errormessage is returned here, otherwise null is returned</param>
        /// <param name="createAllowed">if the directory does not yet exist, is the datcoordinator allowed to create it or not</param>
        /// <returns>True if the path is valid, false otherwise</returns>
        bool TestPath(string path, string username, string pass, out string errormessage, bool createAllowed);
    
        /// <summary>
        /// Enables file autocompletion for a textbox
        /// </summary>
        /// <param name="handle">handle of the textbox to autocomplete</param>
        /// <param name="directory">set to true if autocompletion for a directory, false for a file</param>
        void EnableAutoComplete(IntPtr handle, bool directory);

        /// <summary>
        /// This method checks if a file already exists and if so suggests a suitable name for the new file
        /// to write so the new file does not replace an existing file. It does so by appending underscore and an index
        /// to the filename.
        /// </summary>
        /// <param name="filename">The name of the file a suitable replacement is to be found if the file allready exists</param>
        /// <returns>The same filename if the file didn't exist yet, else a modified filename of a not yet existing file</returns>
        string FindSuitableFileName(string filename);


        PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex);
    }
}
