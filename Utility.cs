
 namespace VMSDev
{
    using ExceptionService;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Mail;
    using System.Reflection;
    using System.Security;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml.Serialization;
    using VMSConstants;

    /// <summary>
    /// The Utility class
    /// </summary>    
    public class Utility
    {
        /// <summary>
        /// The ListenerType enumeration
        /// </summary>        
        public enum ListenerType
        {
            /// <summary>
            /// Enum  File
            /// </summary>
            File,

            /// <summary>
            /// Enum  Email
            /// </summary>
            Email,

            /// <summary>
            /// Enum  event viewer
            /// </summary>
            EventViewer,

            /// <summary>
            /// Enum  all
            /// </summary>
            All
        }
        
        /// <summary>
        /// The VMSUtility class
        /// </summary>        
        public class VMSUtility
        {
            /// <summary>
            /// To log the exceptions and show the error page
            /// </summary>
            /// <param name="ex">exception message</param>
            /// <param name="cont">page context</param>
            public static void LogExceptionAndShowErrorPage(Exception ex, HttpContext cont)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, cont);     
            }
        }
    }
}x
