﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iba.DatCoordinator.Status.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("iba.DatCoordinator.Status.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Force stop.
        /// </summary>
        internal static string ForceStopCaption {
            get {
                return ResourceManager.GetString("ForceStopCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This action will forcefully stop the ibaDatCoordinator service and any associated ibaAnalyzer processes. This can result in unsaved work, or an inconsistent state of the output of the various tasks..
        /// </summary>
        internal static string ForceStopWarning {
            get {
                return ResourceManager.GetString("ForceStopWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ibaAnalyzer {
            get {
                object obj = ResourceManager.GetObject("ibaAnalyzer", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ibaDatCoordinator {
            get {
                object obj = ResourceManager.GetObject("ibaDatCoordinator", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exit.
        /// </summary>
        internal static string notifyIconMenuItemExit {
            get {
                return ResourceManager.GetString("notifyIconMenuItemExit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Restore.
        /// </summary>
        internal static string notifyIconMenuItemRestore {
            get {
                return ResourceManager.GetString("notifyIconMenuItemRestore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Service.
        /// </summary>
        internal static string notifyIconMenuItemStartService {
            get {
                return ResourceManager.GetString("notifyIconMenuItemStartService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stop Service.
        /// </summary>
        internal static string notifyIconMenuItemStopService {
            get {
                return ResourceManager.GetString("notifyIconMenuItemStopService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The new port number will not take effect until the server is restarted. Do you want to restart the server now?.
        /// </summary>
        internal static string RestartServerQuestion {
            get {
                return ResourceManager.GetString("RestartServerQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was a problem connecting to the ibaDatCoordinator service : &apos;&apos;{0}&quot;{1}ibaDatCoordinator will continue in disconnected mode..
        /// </summary>
        internal static string ServiceConnectProblem {
            get {
                return ResourceManager.GetString("ServiceConnectProblem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The service could not be started.
        /// </summary>
        internal static string ServiceConnectProblem2 {
            get {
                return ResourceManager.GetString("ServiceConnectProblem2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was a problem connecting to the ibaDatCoordinator service :&quot;{0}&quot;{1}Please stop the service manually.
        /// </summary>
        internal static string ServiceConnectProblem3 {
            get {
                return ResourceManager.GetString("ServiceConnectProblem3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The service could not be stopped.
        /// </summary>
        internal static string ServiceConnectProblem4 {
            get {
                return ResourceManager.GetString("ServiceConnectProblem4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not connect to the ibaDatCoordinator Service.
        /// </summary>
        internal static string ServiceConnectProblemCaption {
            get {
                return ResourceManager.GetString("ServiceConnectProblemCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Running.
        /// </summary>
        internal static string serviceStatRunning {
            get {
                return ResourceManager.GetString("serviceStatRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stopped.
        /// </summary>
        internal static string serviceStatStopped {
            get {
                return ResourceManager.GetString("serviceStatStopped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ibaDatCoordinator Service is not available.
        /// </summary>
        internal static string ServiceStatusTooltipError {
            get {
                return ResourceManager.GetString("ServiceStatusTooltipError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ibaDatCoordinator Service is running.
        /// </summary>
        internal static string ServiceStatusTooltipRunning {
            get {
                return ResourceManager.GetString("ServiceStatusTooltipRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ibaDatCoordinator Service is stopped.
        /// </summary>
        internal static string ServiceStatusTooltipStopped {
            get {
                return ResourceManager.GetString("ServiceStatusTooltipStopped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap shield {
            get {
                object obj = ResourceManager.GetObject("shield", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ibaAnalyzer settings could not be transferred from local account to service account.
        /// </summary>
        internal static string TransferIbaAnalyzerSettingsFailed {
            get {
                return ResourceManager.GetString("TransferIbaAnalyzerSettingsFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ibaAnalyzer settings successfully transferred from local account to service account.
        /// </summary>
        internal static string TransferIbaAnalyzerSettingsSuccess {
            get {
                return ResourceManager.GetString("TransferIbaAnalyzerSettingsSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User Account Control.
        /// </summary>
        internal static string UACCaption {
            get {
                return ResourceManager.GetString("UACCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevated UAC privileges are required to alter the ibaDatCoordinator service status.
        /// </summary>
        internal static string UACText {
            get {
                return ResourceManager.GetString("UACText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elevated UAC privileges are required to alter the windows registry settings.
        /// </summary>
        internal static string UACTextRegistrySettings {
            get {
                return ResourceManager.GetString("UACTextRegistrySettings", resourceCulture);
            }
        }
    }
}
