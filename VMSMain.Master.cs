
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
    using VMSDev;
    using XSS = CAS.Security.Application.EncodeHelper;
    using VMSUtility;

    /// <summary>
    /// Partial class IVS
    /// </summary>
    public partial class VMSMain : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Gets or sets the Roles field
        /// </summary>        
        public List<string> Roles { get; set; }



        /// <summary>
        /// The IsInteger method
        /// </summary>
        /// <param name="theValue">The theValue parameter</param>
        /// <returns>The boolean type object</returns>        
        public static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// disables all menu controls in master page when error is occurred.
        /// </summary>
        public void DisableMenuControls()
        {
            try
            {
                ////To clear Menus
                if (this.tdmenu != null)
                {
                    this.tdmenu.Visible = false;
                    this.VMSMenu.DataSource = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GetUser method
        /// </summary>
        /// <returns>The string type object</returns>        
        public string GetUser()
        {
            try
            {
                // string[] strUserId = HttpContext.Current.Request.LogonUserIdentity.Name.ToString().Split('\\');
                // Required in PROD - please dont modify - required for CAMS integration
                string userID = string.Empty;
                if (this.Session["LoginID"] == null)
                {
                    userID = VMSUtility.GetUserId();
                    this.Session["LoginID"] = userID;
                }
                else
                {
                    userID = this.Session["LoginID"].ToString();
                }

                return userID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ////End Changes SSO integration

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserRole"] == null && this.Session["LoginID"] != null)
            {
                this.Session["UserRole"] = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(Session["LoginID"].ToString());
                this.Roles = (List<string>)Session["UserRole"];
            }
            try
            { 

                if (!this.IsPostBack)
                {
                    this.GetRole();
                    this.SetUserNameonLoad();
                    this.VMSSiteMapDataSource.ShowStartingNode = false;
                }
            }
            catch (ThreadAbortException)
            {
                try
                {
                    Response.Redirect("AccessDenied.aspx", true);
                     
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Page_PreRender method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AppendHeader("Refresh", Convert.ToString(HttpContext.Current.Session.Timeout * 60) + "; Url=" + Page.ResolveUrl("SessionExpired.aspx"));
        }

        /// <summary>
        /// The method to logout
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void LnkLogout_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The method for footer menu
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void MnuFooter_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objreq = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                DataSet securitycity = new DataSet();
                this.Session["UserRole"] = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(Session["LoginID"].ToString());
                this.Roles = (List<string>)Session["UserRole"];
                if (this.Roles.Contains("Security")||this.Roles.Contains("Visitor Desk"))
                {
                    if (e.Item.NavigateUrl.ToString().Contains("HostIVS.aspx"))
                    {
                        this.VMSMenu.Items.Remove(e.Item);
                    }
                }
               

                securitycity = objreq.GetSecurityCity(Session["LoginID"].ToString());
                if (securitycity.Tables[0].Rows.Count > 0)
                {
                    if (securitycity.Tables[0].Rows[0]["CountryName"].ToString() != "India")
                    {
                        if (e.Item.NavigateUrl.ToString().Contains("ClientVisit.aspx"))
                        {
                            e.Item.Enabled = false;
                            e.Item.Text = string.Empty;
                            e.Item.ToolTip = string.Empty;
                            e.Item.Value = string.Empty;
                            e.Item.NavigateUrl = string.Empty;
                        }
                        else if (e.Item.NavigateUrl.ToString().Contains("ViewLogbySecurity.aspx"))
                        {
                            e.Item.NavigateUrl = "OldViewLogbySecurity.aspx";
                        }
                    }
                    else
                    {
                        if (e.Item.Text.ToLower() == "visitors")
                        {
                            e.Item.Text = "General visitors";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        ////Begin Changes SSO integration        

        /// <summary>
        /// The method for preference language
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DrpPreferLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////Sets the cookie that is to be used by Global.asax
            this.SetUserCulture();

            Server.Transfer(Request.Path);
        }

        /// <summary>
        /// The SetUserCulture method
        /// </summary>        
        protected void SetUserCulture()
        {
            //try
            //{
            //    HttpCookie cookie = new HttpCookie("CultureInfo");
            //    cookie.Value = XSS.HtmlEncode(this.drpPreferLang.SelectedValue);
            //    Response.Cookies.Add(cookie);
            //    ////Set the culture and reload for immediate effect. 
            //    ////Future effects are handled by Global.asax
            //    Thread.CurrentThread.CurrentCulture = new CultureInfo(this.drpPreferLang.SelectedValue);
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.drpPreferLang.SelectedValue);
            //    //// Server.Transfer(Request.Path);
            //}
            //catch (Exception ex)
            //{
            //    Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            //}
        }

        /// <summary>
        /// sets the username on the top of the screen when page is loaded
        /// </summary>
        private void SetUserNameonLoad()
        {
            try
            {
                ////VMSSiteMapDataSource.ShowStartingNode = false;
                if (this.Session["LoginID"] != null)
                {
                    DataTable userDetails = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetAssociateDetails(
                                                    Session["LoginID"].ToString());
#pragma warning disable CS0219 // The variable 'userCulture' is assigned but its value is never used
                    string userCulture = VMSConstants.VMSConstants.DEFAULTCULTURE;
#pragma warning restore CS0219 // The variable 'userCulture' is assigned but its value is never used
                    if (userDetails.Rows.Count > 0)
                    {
                        string firstName = userDetails.Rows[0]["FirstName"].ToString().Trim();
                        if (!string.IsNullOrEmpty(userDetails.Rows[0]["LastName"].ToString()))
                        {
                            firstName = userDetails.Rows[0]["LastName"].ToString().Trim() + "," + firstName;
                        }

                        this.lblName.Text = XSS.HtmlEncode(string.Concat(firstName));
                        if (this.Roles != null)
                        {
                            if (this.Session["UserCulture"] == null)
                            {
                                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new
                                    VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                                DataSet securitydetails = requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                                

                                if (securitydetails != null)
                                {
                                    if (securitydetails.Tables[0].Rows.Count > 0)
                                    {
                                        this.Session["UserCulture"] = Convert.ToString(securitydetails.Tables[0].Rows[0]["CultureInfo"]);
                                        this.Session["Country"] = Convert.ToString(securitydetails.Tables[0].Rows[0]["CountryName"]);
                                        if (this.Session["UserCulture"] != null && string.Compare(this.Roles[0].ToString(), "Security") == 0)
                                        {
                                            this.drpPreferLang.Items.FindByValue(Convert.ToString(Session["UserCulture"])).Selected = true;
                                            this.SetUserCulture();
                                            this.Abouthome.Text = Resources.LocalizedText.Home;
                                            this.lnkLogout.Text = Resources.LocalizedText.Logout;
                                            this.lblwelcome.Text = Resources.LocalizedText.Welcome;
                                            this.lblLanguage.Text = Resources.LocalizedText.OfferedLanguage;
                                            ////lblHelp.Text = Resources.LocalizedText.Help;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                this.drpPreferLang.SelectedValue = Thread.CurrentThread.CurrentCulture.Name;
                            }
                        }
                    }
                }
            }
            catch (System.NullReferenceException)
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GetRole method
        /// </summary>        
        private void GetRole()
        {           
            try
            {
                string CurrentUserId = this.GetUser();
                this.Session["LoginID"] = CurrentUserId;
                this.Session.Add("RoleID", "Host");
                this.Roles = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(CurrentUserId);
                if (this.Roles != null)
                {
                    string[] userRoles = new string[this.Roles.Count];
                    for (int i = 0; i < userRoles.Length; i++)
                    {
                        userRoles[i] = this.Roles[i];
                    }
                    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(CurrentUserId), userRoles);
                    if (userRoles.Length > 0)
                    {
                        string roleID = userRoles[0].ToString();

                        this.Session["RoleID"] = roleID;
                        //if (roleID.Equals("Security"))
                        //{
                        //    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(CurrentUserId), new string[] { "Security" });
                        //}
                        //else if (roleID.Equals("Admin"))
                        //{
                        //    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(CurrentUserId), new string[] { "Admin" });
                        //}
                        //else if (roleID.Equals("SuperAdmin"))
                        //{
                        //    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(CurrentUserId), new string[] { "SuperAdmin" });
                        //}
                        //else if (roleID.Equals("ReceptionAdmin"))
                        //{
                        //    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(CurrentUserId), new string[] { "ReceptionAdmin" });
                        //}
                        //else if (roleID.Equals("IDCardAdmin"))
                        //{
                        //    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(CurrentUserId), new string[] { "IDCardAdmin" });
                        //}
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
