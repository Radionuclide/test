﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Alunorf_roh_plugin.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Alunorf_roh_plugin.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to The .dat file {0} could not be read.
        /// </summary>
        internal static string DatFileCouldNotBeOpened {
            get {
                return ResourceManager.GetString("DatFileCouldNotBeOpened", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected error while reading .dat file: {0}.
        /// </summary>
        internal static string ErrorDatUnexpected {
            get {
                return ResourceManager.GetString("ErrorDatUnexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: could not create ibaFiles instance: {0}.
        /// </summary>
        internal static string ErrorIbaFiles {
            get {
                return ResourceManager.GetString("ErrorIbaFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: could not open .dat file: {0}.
        /// </summary>
        internal static string ErrorIbaFilesOpen {
            get {
                return ResourceManager.GetString("ErrorIbaFilesOpen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected error while writing .roh file: {0}.
        /// </summary>
        internal static string ErrorRohUnexpected {
            get {
                return ResourceManager.GetString("ErrorRohUnexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: could not write .roh file: {0}.
        /// </summary>
        internal static string ErrorRohWrite {
            get {
                return ResourceManager.GetString("ErrorRohWrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected error: {0}.
        /// </summary>
        internal static string ErrorUnexpected {
            get {
                return ResourceManager.GetString("ErrorUnexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error:  Kanalbeschreibung table line {1}: channel &apos;{0}&apos; not found in the .dat file.
        /// </summary>
        internal static string KanalDataNotFound {
            get {
                return ResourceManager.GetString("KanalDataNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Kopfdaten table line {1}: infofield &apos;{0}&apos; not found in the .dat file.
        /// </summary>
        internal static string KopfDataNotFound {
            get {
                return ResourceManager.GetString("KopfDataNotFound", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap left {
            get {
                object obj = ResourceManager.GetObject("left", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap open {
            get {
                object obj = ResourceManager.GetObject("open", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap right {
            get {
                object obj = ResourceManager.GetObject("right", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Icon RohTask {
            get {
                object obj = ResourceManager.GetObject("RohTask", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Schlussdaten table line {1}: infofield &apos;{0}&apos; not found in the .dat file.
        /// </summary>
        internal static string SchlussDataNotFound {
            get {
                return ResourceManager.GetString("SchlussDataNotFound", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap select {
            get {
                object obj = ResourceManager.GetObject("select", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select channels for additional Kanalbeschreibung table entries.
        /// </summary>
        internal static string SelectKanal {
            get {
                return ResourceManager.GetString("SelectKanal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select infofields for additional Kopfdaten table entries.
        /// </summary>
        internal static string SelectKopf {
            get {
                return ResourceManager.GetString("SelectKopf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select infofields for additional Schlussdaten table entries.
        /// </summary>
        internal static string SelectSchluss {
            get {
                return ResourceManager.GetString("SelectSchluss", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select infofields for additional Stichdaten table entries.
        /// </summary>
        internal static string SelectStich {
            get {
                return ResourceManager.GetString("SelectStich", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Stichdaten table line {1}: infofield &apos;{0}&apos; not found in the .dat file.
        /// </summary>
        internal static string StichDataNotFound {
            get {
                return ResourceManager.GetString("StichDataNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .roh file succesfully written.
        /// </summary>
        internal static string TestSuccess {
            get {
                return ResourceManager.GetString("TestSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Browse for template .dat file.
        /// </summary>
        internal static string tooltipBrowse {
            get {
                return ResourceManager.GetString("tooltipBrowse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select infofields and channels from .dat file.
        /// </summary>
        internal static string tooltipSelect {
            get {
                return ResourceManager.GetString("tooltipSelect", resourceCulture);
            }
        }
    }
}
