﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AM_OSPC_plugin.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AM_OSPC_plugin.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Analysis file not found: .
        /// </summary>
        internal static string AnalysisFileNotFound {
            get {
                return ResourceManager.GetString("AnalysisFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not evaluate expression for variable {0}.
        /// </summary>
        internal static string BadEvaluate {
            get {
                return ResourceManager.GetString("BadEvaluate", resourceCulture);
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
        ///   Looks up a localized string similar to You have version {0} of ibaAnalyzer installed. The minimum required version is 6.5.0, 6.7.0 or later is recommended..
        /// </summary>
        internal static string ibaAnalyzerVersionError {
            get {
                return ResourceManager.GetString("ibaAnalyzerVersionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to no ibaAnalyzer registered.
        /// </summary>
        internal static string noIbaAnalyser {
            get {
                return ResourceManager.GetString("noIbaAnalyser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No valid fields are specified for the OSPC message.
        /// </summary>
        internal static string NoValidEntriesSpecified {
            get {
                return ResourceManager.GetString("NoValidEntriesSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ibaAnalyzer version could not be determined.
        /// </summary>
        internal static string NoVersion {
            get {
                return ResourceManager.GetString("NoVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap open {
            get {
                object obj = ResourceManager.GetObject("open", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon OSPC {
            get {
                object obj = ResourceManager.GetObject("OSPC", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ibaAnalyzer PDO files (*.pdo)|*.pdo.
        /// </summary>
        internal static string PdoFileFilter {
            get {
                return ResourceManager.GetString("PdoFileFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap select {
            get {
                object obj = ResourceManager.GetObject("select", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
