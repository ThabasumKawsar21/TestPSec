
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using System.Web.SessionState;
    using System.Windows.Forms;
    using System.Xml.Linq;
    using VMSUtility;

    /// <summary>
    /// The Global class
    /// </summary>    
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// The Application_Start method 
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Application_Start(object sender, EventArgs e)
        {

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
          

        }

        /// <summary>
        /// The Session_Start method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Session_Start(object sender, EventArgs e)
        {
        }
        
        /// <summary>
        /// The Application_BeginRequest method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //try
            //{
            //    //HttpCookie cookie = Request.Cookies["CultureInfo"];
            //    //if (cookie != null && cookie.Value != null)
            //    //{
            //    //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
            //    //    Thread.CurrentThread.CurrentCulture = new CultureInfo(cookie.Value);
            //    //}
            //    //else
            //    //{
            //    //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            //    //    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            //}
        }
        
        /// <summary>
        /// The Application_AuthenticateRequest method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }
        
        /// <summary>
        /// The Application_Error method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Application_Error(object sender, EventArgs e)
        {
            System.Diagnostics.EventLog.WriteEntry("Application", String.Format("Global Exception: {0}", e.ToString()));
        }

        /// <summary>
        /// The Session_End method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The Application_End method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Application_End(object sender, EventArgs e)
        {
        }

        private void Application_PostAuthenticateRequest()
        {
            try
            {
                string CurrentUserId = VMSUtility.GetUserId();
                List<string> roles = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(CurrentUserId);

                string[] userRoles = new string[roles.Count];

                for (int i = 0; i < userRoles.Length; i++)
                {
                    userRoles[i] = roles[i];
                }

                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new GenericIdentity(CurrentUserId), userRoles);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.EventLog.WriteEntry("Application", String.Format("Global Exception: {0}", e.ExceptionObject));
        }
    }
}
