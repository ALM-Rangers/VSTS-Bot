﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vsar.TSBot.Resources {
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
    internal class Labels {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Labels() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Vsar.TSBot.Resources.Labels", typeof(Labels).Assembly);
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
        ///   Looks up a localized string similar to Approvals.
        /// </summary>
        internal static string Approvals {
            get {
                return ResourceManager.GetString("Approvals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication is required..
        /// </summary>
        internal static string AuthenticationRequired {
            get {
                return ResourceManager.GetString("AuthenticationRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connected to {0}..
        /// </summary>
        internal static string ConnectedTo {
            get {
                return ResourceManager.GetString("ConnectedTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please login to Team Services..
        /// </summary>
        internal static string PleaseLogin {
            get {
                return ResourceManager.GetString("PleaseLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Queue Build.
        /// </summary>
        internal static string QueueBuild {
            get {
                return ResourceManager.GetString("QueueBuild", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to QueueRelease.
        /// </summary>
        internal static string QueueRelease {
            get {
                return ResourceManager.GetString("QueueRelease", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome back {0}. I have connected you to Account &apos;{1}&apos;, Team Project &apos;{2}&apos;..
        /// </summary>
        internal static string WelcomeExistingUser {
            get {
                return ResourceManager.GetString("WelcomeExistingUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome {0}. I see this is the first team we speak..
        /// </summary>
        internal static string WelcomeNewUser {
            get {
                return ResourceManager.GetString("WelcomeNewUser", resourceCulture);
            }
        }
    }
}
