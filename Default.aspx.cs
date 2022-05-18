
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using VMSUtility;

    /// <summary>
    /// The Assign Time Zone Off set method
    /// </summary>
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// The Assign Time Zone Off set method
        /// </summary>
        /// <param name="strTimezoneoffset">The Time zone offset parameter</param>        
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void AssignTimeZoneOffset(string strTimezoneoffset)
        {
            if (!string.IsNullOrEmpty(strTimezoneoffset))
            {
                this.Session["TimezoneOffset"] = strTimezoneoffset;
            }
            else
            {
                this.Session["TimezoneOffset"] = "0";
            }
        }
        
        /// <summary>
        /// The AssignCurrentDateTime method
        /// </summary>
        /// <param name="currentDate">The currentDate parameter</param>        
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void AssignCurrentDateTime(string currentDate)
        {
            if (!string.IsNullOrEmpty(currentDate))
            {
                this.Session["currentDateTime"] = currentDate;
            }
            else
            {
                this.Session["currentDateTime"] = "0";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["proxyuser"]))
                {
                    string receivedProxyId = Request.QueryString["proxyuser"].ToString();
                    this.Session["LoginID"] = receivedProxyId;

                    List<string> roles = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(receivedProxyId);
                    string[] userRoles = new string[roles.Count];

                    for (int i = 0; i < userRoles.Length; i++)
                    {
                        userRoles[i] = roles[i];
                    }

                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new GenericIdentity(receivedProxyId), userRoles);
                }
                else
                {
                    string CurrentUserId = string.Empty;

                    if (Session["LoginID"] != null)
                    {
                        CurrentUserId = Session["LoginID"].ToString();
                    }
                    else
                    {                        
                        CurrentUserId = VMSUtility.GetUserId();
                        this.Session["LoginID"] = CurrentUserId;
                    }
                    
                    List<string> roles = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(CurrentUserId);

                    string[] userRoles = new string[roles.Count];

                    for (int i = 0; i < userRoles.Length; i++)
                    {
                        userRoles[i] = roles[i];
                    }

                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new GenericIdentity(CurrentUserId), userRoles);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        /// <summary>
        /// The Page_LoadComplete method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                
                if (!this.IsPostBack)
                {
                   
                        Ajax.Utility.RegisterTypeForAjax(typeof(Default));
                    
                    if (this.Session["TimezoneOffset"] == null)
                    {
                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
                    }
                }

                if (this.Session["RoleID"] != null && this.Session["LoginID"] != null)
                {
                    string roleID = Session["RoleID"].ToString();

                    if (roleID.ToUpper().Equals("SECURITY"))
                    {
                        
                            HttpContext.Current.Response.Redirect("ViewLogbySecurity.aspx", true);
                          
                       
                       
                    }
                    if (roleID.ToUpper().Equals("Visitor Desk"))
                    {

                        HttpContext.Current.Response.Redirect("ViewLogbySecurity.aspx", true);



                    }


                    else if (roleID.ToUpper().Equals("HOST"))
                    {
                        
                            Response.Redirect("AccessDenied.aspx", true);                                              
                    }
                }
}
                    catch (System.Threading.ThreadAbortException ex)
                        {

                        }

            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The InitializeCulture method
        /// </summary>        
        protected override void InitializeCulture()
        {
            try
            {
                //string selectedLanguage = Thread.CurrentThread.CurrentCulture.Name;
                //this.UICulture = selectedLanguage;
                //this.Culture = selectedLanguage;
                //Thread.CurrentThread.CurrentCulture =
                //    CultureInfo.CreateSpecificCulture(selectedLanguage);
                //Thread.CurrentThread.CurrentUICulture = new
                //    CultureInfo(selectedLanguage);
                base.InitializeCulture();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
