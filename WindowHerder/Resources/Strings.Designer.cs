﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowHerder.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WindowHerder.Resources.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to WindowHerder.
        /// </summary>
        internal static string ApplicationTitle {
            get {
                return ResourceManager.GetString("ApplicationTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The latest window snapshot has been restored.
        /// </summary>
        internal static string SnapshotRestored_SuccessBalloonMessage {
            get {
                return ResourceManager.GetString("SnapshotRestored_SuccessBalloonMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error taking a new window snapshot, please try again.
        /// </summary>
        internal static string SnapshotTaken_ErrorBalloonMessage {
            get {
                return ResourceManager.GetString("SnapshotTaken_ErrorBalloonMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A new window snapshot has been taken.
        /// </summary>
        internal static string SnapshotTaken_SuccessBalloonMessage {
            get {
                return ResourceManager.GetString("SnapshotTaken_SuccessBalloonMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WindowHerder is now running.
        /// </summary>
        internal static string StartupBalloonMessage {
            get {
                return ResourceManager.GetString("StartupBalloonMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exit.
        /// </summary>
        internal static string SystemTrayMenu_Exit {
            get {
                return ResourceManager.GetString("SystemTrayMenu_Exit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Restore the latest snapshot.
        /// </summary>
        internal static string SystemTrayMenu_RestoreSnapshot {
            get {
                return ResourceManager.GetString("SystemTrayMenu_RestoreSnapshot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Take a new snapshot.
        /// </summary>
        internal static string SystemTrayMenu_TakeSnapshot {
            get {
                return ResourceManager.GetString("SystemTrayMenu_TakeSnapshot", resourceCulture);
            }
        }
    }
}
