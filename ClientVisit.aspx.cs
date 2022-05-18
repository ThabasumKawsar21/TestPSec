
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
#pragma warning disable CS0105 // The using directive for 'System.Threading' appeared previously in this namespace
    using System.Threading;
#pragma warning restore CS0105 // The using directive for 'System.Threading' appeared previously in this namespace
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// partial class view log by security
    /// </summary>
    public partial class ClientVisit : System.Web.UI.Page
    {
        /// <summary>
        /// The Assign Time Zone Offset method
        /// </summary>
        /// <param name="strTimezoneoffset">The string Time zone offset parameter</param>        
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

        /// <summary>
        /// The button Check Out Request Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        public void BtnCheckOutRequest_Click(object sender, EventArgs e)
        {
            this.ViewLogbySecurity1.BtnCheckOutRequest_Click(sender, e);
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["TypeOfVisit"] = "Client";
                Ajax.Utility.RegisterTypeForAjax(typeof(ClientVisit));
                if (!Page.IsPostBack)
                {
                    if (this.IsHost())
                    {
                        
                            Response.Redirect("HostWP.aspx", true);
                            
                      
                       
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
        /// The button Hidden Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
            this.ViewLogbySecurity1.InitDates();
            this.ViewLogbySecurity1.BindData();
        }

        /// <summary>
        /// The InitializeCulture method
        /// </summary>        
        protected override void InitializeCulture()
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

        /// <summary>
        /// The IsHost method
        /// </summary>
        /// <returns>The boolean type object</returns>        
        protected bool IsHost()
        {
            bool ishost = false;
            try
            {
                if (this.Session["RoleID"] != null)
                {
                    string strRole = Session["RoleID"].ToString();
                    if (strRole.ToUpper().Equals("SECURITY") || strRole.ToUpper().Equals("SUPERADMIN") || strRole.ToUpper().Equals("VISITOR DESK"))
                    {
                        ishost = false;
                    }
                    else
                    {
                        ishost = true;
                    }
                }
                else
                {
                    string strUserID = string.Empty;
                    if (this.Session["LoginID"] != null)
                    {
                        strUserID = Session["LoginID"].ToString();
                    }
                    else
                    {
                        // Not modify this - required for SSO 
                        strUserID = VMSUtility.VMSUtility.GetUserId();
                        //// string[] strUserId = HttpContext.Current.Request.LogonUserIdentity.Name.ToString().Split('\\');
                        ////checking
                    }

                    List<string> roles = new List<string>();
                    string strRole = string.Empty;
                    roles = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(strUserID);
                    if (roles != null)
                    {
                        string[] userRoles = new string[roles.Count];
                        for (int i = 0; i < userRoles.Length; i++)
                        {
                            userRoles[i] = roles[i];
                        }

                        if (userRoles.Length > 0)
                        {
                            string roleID = userRoles[0].ToString();

                            this.Session["RoleID"] = roleID;
                            if (roleID.Equals("Security"))
                            {
                                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(strUserID), new string[] { "Security" });
                            }
                            else if (roleID.Equals("Admin"))
                            {
                                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(strUserID), new string[] { "Admin" });
                            }
                            else if (roleID.Equals("SuperAdmin"))
                            {
                                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(strUserID), new string[] { "SuperAdmin" });
                            }
                            else if (roleID.Equals("ReceptionAdmin"))
                            {
                                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(strUserID), new string[] { "ReceptionAdmin" });
                            }
                            else if (roleID.Equals("IDCardAdmin"))
                            {
                                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(strUserID), new string[] { "IDCardAdmin" });
                            }
                            else if (roleID.Equals("Visitor Desk"))
                            {
                                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(strUserID), new string[] { "Visitor Desk" });
                            }
                        }
                    }
                    else
                    {
                        strRole = "Host";
                    }

                    if (strRole.ToUpper().Equals("HOST"))
                    {
                        ishost = true;
                    }
                    else
                    {
                        ishost = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return ishost;
        }
    }
}
