﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Alunorf_sinec_h1_plugin.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Alunorf_sinec_h1_plugin.Properties.Resources", typeof(Resources).Assembly);
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
        
        internal static System.Drawing.Bitmap Aktualisieren {
            get {
                object obj = ResourceManager.GetObject("Aktualisieren", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to connected.
        /// </summary>
        internal static string connected {
            get {
                return ResourceManager.GetString("connected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to disconnected.
        /// </summary>
        internal static string disconnected {
            get {
                return ResourceManager.GetString("disconnected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not connect to one of the first NQS server: .
        /// </summary>
        internal static string err_connect1 {
            get {
                return ResourceManager.GetString("err_connect1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not connect to  the second NQS server: .
        /// </summary>
        internal static string err_connect2 {
            get {
                return ResourceManager.GetString("err_connect2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not connect to either of the NQS servers: .
        /// </summary>
        internal static string err_connect3 {
            get {
                return ResourceManager.GetString("err_connect3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not open .dat file for reading.
        /// </summary>
        internal static string err_no_open {
            get {
                return ResourceManager.GetString("err_no_open", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to QDT telegram not (yet) acknowledged.
        /// </summary>
        internal static string err_noAck {
            get {
                return ResourceManager.GetString("err_noAck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to None of the NQS servers is ready to process QDT telegrams.
        /// </summary>
        internal static string err_nogo {
            get {
                return ResourceManager.GetString("err_nogo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To many telegrams queued for sending.
        /// </summary>
        internal static string err_queue {
            get {
                return ResourceManager.GetString("err_queue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not send telegram: .
        /// </summary>
        internal static string err_send {
            get {
                return ResourceManager.GetString("err_send", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There were errors sending the telegrams:.
        /// </summary>
        internal static string err_tele {
            get {
                return ResourceManager.GetString("err_tele", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to First error at {0}th infofield .
        /// </summary>
        internal static string err_tele_info {
            get {
                return ResourceManager.GetString("err_tele_info", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following telegrams were malformed or not compatible with the .dat file:.
        /// </summary>
        internal static string err_tele_malformed {
            get {
                return ResourceManager.GetString("err_tele_malformed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following telegrams were negatively acknowledged (NAK):.
        /// </summary>
        internal static string err_tele_NAK {
            get {
                return ResourceManager.GetString("err_tele_NAK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following telegrams could not be sent:.
        /// </summary>
        internal static string err_tele_send {
            get {
                return ResourceManager.GetString("err_tele_send", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to First error at {0}th signalfield.
        /// </summary>
        internal static string err_tele_signal {
            get {
                return ResourceManager.GetString("err_tele_signal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following telegrams were not acknowledged in time:.
        /// </summary>
        internal static string err_tele_time {
            get {
                return ResourceManager.GetString("err_tele_time", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to load message configuration: .
        /// </summary>
        internal static string err_xml_load {
            get {
                return ResourceManager.GetString("err_xml_load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to save message configuration: .
        /// </summary>
        internal static string err_xml_save {
            get {
                return ResourceManager.GetString("err_xml_save", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to recieving.
        /// </summary>
        internal static string go {
            get {
                return ResourceManager.GetString("go", resourceCulture);
            }
        }
        
        internal static System.Drawing.Icon H1Task {
            get {
                object obj = ResourceManager.GetObject("H1Task", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        internal static System.Drawing.Icon info {
            get {
                object obj = ResourceManager.GetObject("info", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Info fields.
        /// </summary>
        internal static string infoNodeText {
            get {
                return ResourceManager.GetString("infoNodeText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to initialised.
        /// </summary>
        internal static string initialised {
            get {
                return ResourceManager.GetString("initialised", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add telegram.
        /// </summary>
        internal static string newTelegramNodeText {
            get {
                return ResourceManager.GetString("newTelegramNodeText", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap open {
            get {
                object obj = ResourceManager.GetObject("open", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Icon signal {
            get {
                object obj = ResourceManager.GetObject("signal", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Signal fields.
        /// </summary>
        internal static string signalNodeText {
            get {
                return ResourceManager.GetString("signalNodeText", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap Speichern {
            get {
                object obj = ResourceManager.GetObject("Speichern", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Icon telegram {
            get {
                object obj = ResourceManager.GetObject("telegram", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        internal static System.Drawing.Icon telegram_new {
            get {
                object obj = ResourceManager.GetObject("telegram_new", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Telegram .
        /// </summary>
        internal static string telegramNodeText {
            get {
                return ResourceManager.GetString("telegramNodeText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type here optionally a description of the telegram. This text is not part of the telegram to be sent to the NQS servers..
        /// </summary>
        internal static string tooltipDesc {
            get {
                return ResourceManager.GetString("tooltipDesc", resourceCulture);
            }
        }
    }
}
