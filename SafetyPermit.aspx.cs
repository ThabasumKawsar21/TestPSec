
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBL;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// The SafetyPermit partial class
    /// </summary>  
    public partial class SafetyPermit : System.Web.UI.Page
    {
        #region Variables

        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();

        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// The security city field
        /// </summary>        
        private DataSet securitycity;

        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();
        #endregion

        /// <summary>
        /// The AssignTimeZoneOffset method
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

        ////added by bincey(testing)

        ////Function to Highlight the searched text

        /// <summary>
        /// The Highlight method
        /// </summary>
        /// <param name="inputTxt">The InputTxt parameter</param>
        /// <returns>The string type object</returns>        
        public string Highlight(string inputTxt)
        {
            try
            {
                string strSearch = this.txtSearch.Text.ToString();
                string[] arrSearch = strSearch.Split(' ');
                foreach (string str in arrSearch)
                {
                    if (inputTxt.ToUpper().Contains(str.ToUpper()) && string.IsNullOrEmpty(str))
                    {
                        int len = str.Length;
                        int i = 0;
                        while ((i = inputTxt.ToUpper().IndexOf(str.ToUpper(), i)) != -1)
                        {
                            inputTxt = inputTxt.Insert(i, "$");
                            inputTxt = inputTxt.Insert(i + len + 1, "^");
                            i = inputTxt.LastIndexOf('^');
                        }

                        inputTxt = inputTxt.Replace("$", "<span style=\"background-color: #FFFF00; font-weight:bolder;\">");
                        inputTxt = inputTxt.Replace("^", "</span>");
                    }

                    break;
                }
            }
            catch
            {
            }

            return inputTxt;
        }

        /// <summary>
        /// The Initial Dates method
        /// </summary>        
        public void InitDates()
        {
            try
            {
                ////Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "DefaultDate", "setDefaultDate();", true);

                string format = "dd/MM/yyyy HH:mm:ss";
                System.Globalization.CultureInfo provider =
                    System.Globalization.CultureInfo.InvariantCulture;
                if (this.Session["currentDateTime"] != null)
                {
                    string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                    var today = DateTime.ParseExact(currenttime, format, provider);
                    this.txtFromDate.Text = today.ToShortDateString();
                    this.txtToDate.Text = today.ToShortDateString();
                }
                else
                {
                    var today = this.genTimeZone.GetCurrentDate().ToShortDateString();
                    this.txtFromDate.Text = today;
                    this.txtToDate.Text = today;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The BindData method
        /// </summary>        
        public void BindData()
        {
            try
            {
                this.grdResult.DataSource = null;
                this.grdResult.DataBind();
                if (this.Session["LoginID"] != null)
                {
                    if (this.ValidateSearch())
                    {
                        this.securitycity = this.GetSecurityCity();
                        string reqStatus = Convert.ToString(this.ddlReqStatus.SelectedValue);
                        string securityID = Session["LoginID"].ToString();
                        string[] fromdateArray = this.txtFromDate.Text.Split('/');
                        //// string fromdateActual = fromdateArray[2] + '-' + fromdateArray[1] + '-' + fromdateArray[0];
                        string[] todateArray = this.txtToDate.Text.Split('/');
                        ////  string todateActual = todateArray[2] + '-' + todateArray[1] + '-' + todateArray[0];
                        string fromTime = DateTime.Now.ToShortTimeString();
                        string format = "dd/MM/yyyy HH:mm:ss";
                        System.Globalization.CultureInfo provider =
                            System.Globalization.CultureInfo.InvariantCulture;
                        if (this.Session["currentDateTime"] != null)
                        {
                            string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                            var today = DateTime.ParseExact(currenttime, format, provider);
                            fromTime = today.ToShortTimeString();
                        }

                        GenericTimeZone genrTimeZone = new GenericTimeZone();
                        DateTime fromDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdateArray[0] + "/" + fromdateArray[1] + "/" + fromdateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                        DateTime todate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todateArray[0] + "/" + todateArray[1] + "/" + todateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                        string fromdateActual = fromDate.ToShortDateString();
                        string todateActual = todate.ToShortDateString();
                        string strSP = "SafetyPermit";
                        this.griddata = this.requestDetails.ViewLogBySecurityforSP(this.txtSearch.Text, securityID, fromdateActual, todateActual, reqStatus, strSP);
                        for (int rowCount = 0; rowCount < this.griddata.Tables[0].Rows.Count; rowCount++)
                        {
                            bool isotherfacility = true;
                            if (this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("RETURNED") || this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("LOST"))
                            {
                                DateTime todates = Convert.ToDateTime(Convert.ToDateTime(this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString()).ToShortDateString());
                                DateTime fromdate = Convert.ToDateTime(Convert.ToDateTime(fromdateActual).ToShortDateString());
                                TimeSpan ts = todates - fromdate;
                                if ((ts.Days < 0) && (todates != fromdate))
                                {
                                    this.griddata.Tables[0].Rows[rowCount].Delete();
                                }
                            }
                            else
                            {
                                // added for CR IRVMS22062010CR07  starts here done by Priti
                                for (int rowCountFac = 0; rowCountFac < this.securitycity.Tables[0].Rows.Count; rowCountFac++)
                                {
                                    if (this.griddata.Tables[0].Rows[rowCount]["Facility"].ToString().Equals(this.securitycity.Tables[0].Rows[rowCountFac]["Facility"].ToString()))
                                    {
                                        isotherfacility = false;
                                    }
                                }

                                if (isotherfacility)
                                {
                                    this.griddata.Tables[0].Rows[rowCount].Delete();
                                }
                                ////end addition for CR IRVMS22062010CR07  starts here done by Priti
                            }
                        }

                        if (this.griddata.Tables[0].Rows.Count > 0)
                        {
                            this.lblResult.Visible = false;
                            this.lblStatusResult.Visible = false;
                            this.pnlRequests.Visible = true;
                            this.grdResult.DataSource = this.griddata;
                            this.grdResult.DataBind();
                            if (VMSUtility.VMSUtility.CountryTimeZoneCheck() == true)
                            {
                                this.btnExport.Visible = true;
                            }
                            else
                            {
                                this.btnExport.Visible = false;
                            }

                            this.pnlImage.Visible = true;
                            ////  UpnlVisitor.Visible = false;
                            this.hdnRecordFound.Value = "1";
                        }
                        else
                        {
                            this.lblStatusResult.Visible = false;
                            this.errortbl.Visible = true; ////*********ADDED  BY UMA-18 jULY 09************
                            this.lblResult.Visible = true;
                            this.pnlRequests.Visible = false;

                            this.lblResult.Text = Resources.LocalizedText.NoRecordFound;

                            this.btnExport.Visible = false;
                            this.hdnRecordFound.Value = "0";
                            if (this.hfSearch.Checked == false)
                            {
                                this.pnlGotoRequests.Visible = true;
                                this.pnlGotoVisitors.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The AssignCurrentDateTime method
        /// </summary>
        /// <param name="currentDate">The currentDate parameter</param>        
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void AssignCurrentDateTime(string currentDate)
        {
            if (string.IsNullOrEmpty(currentDate))
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
            ////todo
            ////ViewLogbySecurity1.btnCheckOutRequest_Click(sender, e);
            ////todo
            ////ViewLogbySecuritySP1.btnCheckOutRequest_Click(sender, e);
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
                Ajax.Utility.RegisterTypeForAjax(typeof(SafetyPermit));
                if (!Page.IsPostBack)
                {
                    Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetDefaultDate", "GetDefaultDate();", true);
                    this.lblResult.Visible = false;
                    this.lblStatusResult.Visible = false;
                    this.txtExpress.Focus();
                    this.txtExpress.Enabled = true;
                    this.TextBoxWatermarkExtender1.Enabled = true;
                    Page.Form.DefaultButton = this.btnSearch.UniqueID;
                    this.btnExport.Visible = false;
                    this.ddlReqStatus.Items.FindByValue("Yet to arrive").Selected = true;
                    ////ddlReqStatus.Items.Remove("Select Status");
                    this.hfSearch.Checked = true;
                    this.EnableOrDiableControls();
                    if (this.Session["currentDateTime"] != null)
                    {
                        this.InitDates();
                        this.BindData();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                ////  throw ex;
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Result_RowDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Grdresult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblVisitorID = (Label)e.Row.FindControl("lblVisitorId");
                    ////Image imgVisitorImage = (Image)e.Row.FindControl("imgVisitorImage");
                    ////Image imgVisitorImageBig = (Image)e.Row.FindControl("imgVisitorImageBig");
                    string visitorId = lblVisitorID.Text.Trim();
                    string encryptedVisitorID = VMSBusinessLayer.Encrypt(visitorId);
                    ////imgVisitorImage.ImageUrl = string.Concat("..\\EmployeeImage.aspx?key=", encryptedVisitorID);
                    ////imgVisitorImageBig.ImageUrl = string.Concat("..\\EmployeeImage.aspx?key=", encryptedVisitorID);
                    DateTime dtcurrentTime = this.genTimeZone.GetCurrentDate();
                    LinkButton btnCheckIn = (LinkButton)e.Row.FindControl("btnCheckIn");
                    LinkButton btnCheckOut = (LinkButton)e.Row.FindControl("btnCheckOut");
                    LinkButton btnLost = (LinkButton)e.Row.FindControl("btnLost");
                    LinkButton lbtnPrint = (LinkButton)e.Row.FindControl("btnPrint");
                    LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");
                    string currentTime;
                    bool vodaysRequest = Convert.ToDateTime(drv["Date"].ToString()).ToString("dd/MM/yyyy").Equals(dtcurrentTime.ToString("dd/MM/yyyy"));
                    if (string.IsNullOrEmpty(Convert.ToString(drv["intime"])))
                    {
                        currentTime = dtcurrentTime.ToString("H:mm");
                    }
                    else
                    {
                        currentTime = drv["intime"].ToString();
                    }

                    DateTime dtinTime = this.ConcateDateTime(drv["Date"].ToString(), currentTime);
                    bool currentRequest = false;
                    if (dtinTime <= dtcurrentTime && dtcurrentTime < dtinTime.AddDays(1))
                    {
                        currentRequest = true;
                    }

                    DateTime dtexpectedOutTime;
                    string expectedouttime;

                    if (string.IsNullOrEmpty(Convert.ToString(drv["ExpectedOutTime"])))
                    {
                        expectedouttime = dtcurrentTime.ToString("H:mm");
                    }
                    else
                    {
                        expectedouttime = drv["ExpectedOutTime"].ToString();
                    }

                    if (Convert.ToDateTime(currentTime) < Convert.ToDateTime(expectedouttime))
                    {
                        dtexpectedOutTime = this.ConcateDateTime(drv["Date"].ToString(), expectedouttime);
                    }
                    else
                    {
                        dtexpectedOutTime = this.ConcateDateTime(drv["Date"].ToString(), expectedouttime).AddDays(1);
                    }

                    Image imgStatus = (Image)e.Row.FindControl("image");
                    string strBadgeStatus = drv["BadgeStatus"].ToString();
                    imgStatus.ImageUrl = this.SetStatusImage(strBadgeStatus, dtinTime, dtexpectedOutTime);
                    if (vodaysRequest || currentRequest)
                    {
                        if (string.IsNullOrEmpty(strBadgeStatus))
                        {
                            btnCheckIn.Enabled = true;
                            btnCheckOut.Enabled = false;
                            btnLost.Enabled = false;
                            lbtnPrint.Enabled = false;
                            btnView.Enabled = true;
                        }
                        else
                        {
                            switch (drv["BadgeStatus"].ToString().ToUpper().Trim())
                            {
                                case "ISSUED":
                                    {
                                        btnCheckIn.Enabled = false;
                                        btnCheckOut.Enabled = true;
                                        btnLost.Enabled = true;
                                        lbtnPrint.Enabled = true;
                                        btnView.Enabled = true;
                                        break;
                                    }

                                case "RETURNED":
                                    {
                                        btnCheckIn.Enabled = false;
                                        btnCheckOut.Enabled = false;
                                        btnLost.Enabled = false;
                                        lbtnPrint.Enabled = false;
                                        btnView.Enabled = true;
                                        break;
                                    }

                                default:
                                    {
                                        btnCheckIn.Enabled = false;
                                        btnCheckOut.Enabled = false;
                                        btnLost.Enabled = false;
                                        lbtnPrint.Enabled = false;
                                        btnView.Enabled = true;
                                        break;
                                    }
                            }
                        }
                    }
                    else
                    {
                        btnCheckIn.Enabled = false;
                        btnCheckOut.Enabled = false;
                        btnLost.Enabled = false;
                        lbtnPrint.Enabled = false;
                        btnView.Enabled = true;
                    }

                    Label lblBadgeStatus = (Label)e.Row.FindControl("lblBadgeStatus");
                    if (string.Compare(lblBadgeStatus.Text.ToUpper(), "RETURNED") == 0)
                    {
                        lblBadgeStatus.Text = Resources.LocalizedText.Returned;
                    }
                    else if (string.Compare(lblBadgeStatus.Text.ToUpper(), "ISSUED") == 0)
                    {
                        lblBadgeStatus.Text = Resources.LocalizedText.Issued;
                    }
                    else if (string.Compare(lblBadgeStatus.Text.ToUpper(), "LOST") == 0)
                    {
                        lblBadgeStatus.Text = Resources.LocalizedText.Lost;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The button Print Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ButnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string securityID = Session["LoginID"].ToString();
                string detailsID = XSS.HtmlEncode(ViewState["DetailsID"].ToString());

                string status = this.ddlReason.SelectedItem.Value == "1" ? VMSConstants.VMSConstants.LOST :
                  VMSConstants.VMSConstants.PRINTERJAMMED;

                this.requestDetails.RePrintBadge(Convert.ToInt32(detailsID), securityID, status);

                string strMessgae = Resources.LocalizedText.Reprintsuccessful;
                string strPopScript = string.Concat("<script>alert('", strMessgae, "'); </script>");
                Page.ClientScript.RegisterStartupScript(typeof(Page), "LoadViewLogBySecurity", strPopScript);

                this.ViewState["DetailsID"] = string.Empty;
                ////txtReason.Text = string.Empty;
                string strScript = "<script language='javascript'>window.open('SPBadge.aspx?key=" + detailsID + "', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Hidden Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnHidden1_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// The Visitor Selected method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdVisitor_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            DataPager pager = (DataPager)this.UpdatePanel1.FindControl("pager");
            if (e.AffectedRows <= pager.PageSize)
            {
                pager.Visible = false;
            }
            else
            {
                pager.Visible = true;
            }

            if (e.AffectedRows > 0)
            {
                this.errortbl.Visible = false;
                this.lblResult.Visible = false;
            }
            else
            {
                this.errortbl.Visible = true;
                this.lblResult.Text = Resources.LocalizedText.NoRecordFound;
                this.lblResult.Visible = true;
            }
        }

        /// <summary>
        /// The Yes Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnYes_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The RefreshGrid_Tick method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void RefreshGrid_Tick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The Reset Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            this.txtExpress.Text = string.Empty;
            this.txtSearch.Text = string.Empty;
            this.txtFromDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            ////ddlReqStatus.Items.FindByValue("Select Status").Selected = true;
            this.ddlReqStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// The Export Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            this.ExportGridView();
        }

        /// <summary>
        /// The Hidden Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// The Initialize Culture method
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
        /// The Is Host method
        /// </summary>
        /// <returns>The boolean type object</returns>        
        protected bool IsHost()
        {
            bool isHost = false;
            try
            {
                if (this.Session["RoleID"] != null)
                {
                    string strRole = Session["RoleID"].ToString();
                    if (strRole.ToUpper().Equals("SECURITY") || strRole.ToUpper().Equals("SUPERADMIN"))
                    {
                        isHost = false;
                    }
                    else
                    {
                        isHost = true;
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
                        ////string[] strUserId = HttpContext.Current.Request.LogonUserIdentity.Name.ToString().Split('\\');
                        // Don't change - required for CAMS integration.
                        string strUserId = VMSUtility.VMSUtility.GetUserId();
                        if (string.IsNullOrEmpty(strUserId))
                        {
                             
                                this.Response.Redirect("AccessDenied.aspx", true);
                                 
                           
                           
                        }
                        else
                        {
                            strUserID = strUserId.ToString();
                        }
                    }

                    List<string> roles = new List<string>();
                    string strRole = string.Empty;
                    roles = new VMSBusinessLayer.UserDetailsBL().GetUserRole(strUserID);
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
                        }
                    }
                    else
                    {
                        strRole = "Host";
                    }

                    if (strRole.ToUpper().Equals("HOST"))
                    {
                        isHost = true;
                    }
                    else
                    {
                        isHost = false;
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

            return isHost;
        }

        /// <summary>
        /// The Result Row Command method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
                LinkButton btnCheckOut = (LinkButton)row.FindControl("btnCheckOut");
                LinkButton lbtnPrint = (LinkButton)row.FindControl("btnPrint");
                LinkButton btnCheckIn = (LinkButton)row.FindControl("btnCheckIn");
                LinkButton btnView = (LinkButton)row.FindControl("btnView");
                string securityID = Session["LoginID"].ToString();
                if (e.CommandName.ToString() == "CheckIn")
                {
                    VMSBusinessEntity.VisitorRequest visitorLocObj = new VMSBusinessEntity.VisitorRequest();
                    string detailsID = e.CommandArgument.ToString();
                    this.Session["strdetailsIDsession"] = detailsID;
                    if (this.requestDetails.VisitorCheckIn(Convert.ToInt32(detailsID), securityID))
                    {
                        string visitDetailsIDchk;
                        visitDetailsIDchk = detailsID;
                        string loginID = string.Empty;
                        VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                        VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                        MailNotification objMailNotofication = new MailNotification();
                        MailSMSNotification objMailSMSNotification = new MailSMSNotification();
                        if (!string.IsNullOrEmpty(visitDetailsIDchk))
                        {
                            if (this.requestDetails != null)
                            {
                                propertiesDC = this.requestDetails.DisplayInfo(Convert.ToInt32(visitDetailsIDchk));
                                if (propertiesDC != null)
                                {
                                    this.Session["RequestID"] = propertiesDC.VisitorRequestProperty.RequestID.ToString();
                                    string smsnot = Convert.ToString(propertiesDC.VisitorRequestProperty.ISSMSEnabled);
                                    if (smsnot == "True")
                                    {
                                        ////Visitor Master Details
                                        ////if receive notification chkd
                                        string strVisitorName = string.Concat(propertiesDC.VisitorMasterProperty.FirstName.ToString(), " ", propertiesDC.VisitorMasterProperty.LastName.ToString()).ToUpper();
                                        string strOrganization = Convert.ToString(propertiesDC.VisitorMasterProperty.Company);
                                        string strRequestId = Convert.ToString(propertiesDC.VisitorRequestProperty.RequestID);
                                        ////string strHostName = propertiesDC.VisitorRequestProperty.HostID.ToString();
                                        string strHostName = propertiesDC.VisitorRequestProperty.HostID.ToString().Split(',')[1].Split('(')[0].ToString();
                                        string strFacility = Convert.ToString(propertiesDC.VisitorRequestProperty.Facility);
                                        string strHostID = propertiesDC.VisitorRequestProperty.HostID.ToString().Split('(')[1].Split(')')[0].ToString();
                                        DataTable dt = userDetailsBL.GetCRSAssociateDetails(strHostID);
                                        string strHostEmailId = string.Empty;
                                        if (dt != null)
                                        {
                                            strHostEmailId = Convert.ToString(dt.Rows[0]["EmailID"]).Trim();
                                        }

                                        DateTime today = this.genTimeZone.GetLocalCurrentDateInFormat();
                                        string dttoday = today.ToString("dd/MMM/yyyy");
                                        string time = today.ToShortTimeString(); ////.ToLocalShortTime();                                       
                                        objMailSMSNotification.SendMail(strHostName, strVisitorName, strFacility, dttoday, time, strRequestId, strHostID);
                                    }
                                }
                            }

                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"Visitor's Badge is successfully generated.\"); </script>");
                            string strScript = "<script language='javascript'>(window.open('SPBadge.aspx?key=" + detailsID + "', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ')).focus();</script>";
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript);
                            ////return;
                        }
                    }
                    else
                    {
                        this.BindData();
                        string strMessgae = Resources.LocalizedText.Visitorcheckinsuccessful;
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strMessgae + "'); </script>");

                        return;
                    }
                }

                if (e.CommandName.ToString() == "CheckOut")
                {
                    string str = e.CommandArgument.ToString();
                    string visitDetailsID = e.CommandArgument.ToString();
                    this.Session["hdnVisitDetailId"] = visitDetailsID;
                    string bdgestatus = "Returned";
                    DateTime currentSystemTime = this.genTimeZone.GetCurrentDate();
                    string actualouttime = currentSystemTime.ToLongTimeString();
                    string reqstatus = "Out";
                    string comments = string.Empty;
                    string strCheckOut = string.Empty;
                    string strBadgeReturned = Resources.LocalizedText.BadgeReturned;
                    strCheckOut = "alert('" + strBadgeReturned + "')";

                    string strCheckOutNoRecord = "alert('" + strBadgeReturned + "');window.location.href ='ViewLogbySecurity.aspx'";
                    DataSet dtrequest = this.requestDetails.Updatebdgestatus(visitDetailsID, bdgestatus, actualouttime, reqstatus, comments, Session["LoginID"].ToString());
                    ////     BindData();

                    string[] fromDateArray = Convert.ToString(dtrequest.Tables[0].Rows[0]["FromDate"]).Split('/');
                    string fromDateActual = fromDateArray[2] + '-' + fromDateArray[1] + '-' + fromDateArray[0];

                    string[] todateArray = Convert.ToString(dtrequest.Tables[0].Rows[0]["ToDate"]).Split('/');
                    string todateActual = todateArray[2] + '-' + todateArray[1] + '-' + todateArray[0];

                    this.hdnCheckOutFromDate.Value = Convert.ToDateTime(fromDateActual.ToString()).ToString("MM/dd/yyyy");
                    this.hdnCheckOutToDate.Value = Convert.ToDateTime(todateActual.ToString()).ToString("MM/dd/yyyy");

                    this.hdnCheckOutFromTime.Value = Convert.ToString(dtrequest.Tables[0].Rows[0]["inTime"]);
                    this.hdnCheckOutToTime.Value = Convert.ToString(dtrequest.Tables[0].Rows[0]["ToTime"]);
                    this.hdnCheckOutActualTimeout.Value = Convert.ToString(dtrequest.Tables[0].Rows[0]["ActualOutTime"]);

                    if (this.hdnRecordFound.Value == "1")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "strCheckOut", strCheckOut, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "strCheckOutNoRecord", strCheckOutNoRecord, true);
                    }

                    if (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailtohostBadgereturn_enable"]) == "true")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "GetCheckOutTime", "GetCheckOutTime();", true);
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "LostRequestMail", "LostRequestMail();", true);
                        ////Sendbadgereturnmail(dt);
                    }
                    else
                    {
                        this.BindData();
                    }

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strBadgeReturned + "'); </script>");
                    return;
                }

                if (e.CommandName.ToString() == "Lost")
                {
                    string str = e.CommandArgument.ToString();
                    string visitDetailsID = e.CommandArgument.ToString();
                    string bdgestatus = "Lost";
                    DateTime currentSystemTime = this.genTimeZone.GetCurrentDate();
                    string actualouttime = currentSystemTime.ToLongTimeString();
                    this.Session["hdnVisitDetailId"] = visitDetailsID;
                    ////hdnVisitDetailId.Value = VisitDetailsID;
                    string reqstatus = "Out";
                    string comments = string.Empty;
                    string strbdgeLost;
                    strbdgeLost = Resources.LocalizedText.BadgeLost;
                    ////  strbdgeLost = "alert('Badge Lost. Check Out Successful');";

                    strbdgeLost = "alert('" + strbdgeLost + "');";
                    string strbdgeLostNoRecord = "alert('" + strbdgeLost + "');window.location.href ='ViewLogbySecurity.aspx'";
                    DataSet dtrequest = this.requestDetails.Updatebdgestatus(visitDetailsID, bdgestatus, actualouttime, reqstatus, comments, Session["LoginID"].ToString());
                    ////ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "strbadgeLost", "<script language='javascript'>alert('Badge Lost. Check Out Successful');window.location.href ='ViewLogbySecurity.aspx';</script>", false);
                    string[] fromDateArray = Convert.ToString(dtrequest.Tables[0].Rows[0]["FromDate"]).Split('/');
                    string fromDateActual = fromDateArray[2] + '-' + fromDateArray[1] + '-' + fromDateArray[0];

                    string[] todateArray = Convert.ToString(dtrequest.Tables[0].Rows[0]["ToDate"]).Split('/');
                    string todateActual = todateArray[2] + '-' + todateArray[1] + '-' + todateArray[0];

                    this.hdnCheckOutFromDate.Value = Convert.ToDateTime(fromDateActual.ToString()).ToString("MM/dd/yyyy");
                    this.hdnCheckOutToDate.Value = Convert.ToDateTime(todateActual.ToString()).ToString("MM/dd/yyyy");

                    this.hdnCheckOutFromTime.Value = Convert.ToString(dtrequest.Tables[0].Rows[0]["inTime"]);
                    this.hdnCheckOutToTime.Value = Convert.ToString(dtrequest.Tables[0].Rows[0]["ToTime"]);
                    this.hdnCheckOutActualTimeout.Value = Convert.ToString(dtrequest.Tables[0].Rows[0]["ActualOutTime"]);
                    ////BindData();
                    this.UpdatePanel1.Update();
                    if (this.hdnRecordFound.Value == "1")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "strbdgeLost", strbdgeLost, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "strbdgeLostNoRecord", strbdgeLostNoRecord, true);
                    }

                    if (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailtohostBadgereturn_enable"]) == "true")
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "GetCheckOutTime", "GetCheckOutTime();", true);
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "LostRequestMail", "LostRequestMail();", true);
                        ////Sendbadgereturnmail(dt);
                    }
                    else
                    {
                        this.BindData();
                    }

                    ////Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"Badge Lost. Check Out Successfull\"); </script>");
                    return;
                }

                if (e.CommandName.ToString() == "RePrint")
                {
                    this.ViewState["DetailsID"] = e.CommandArgument.ToString();
                    this.modalReprintComments.Show();
                    return;
                }

                if (e.CommandName.ToString() == "ViewDetailsLink")
                {
                    string str = e.CommandArgument.ToString();
                    string detailsID = e.CommandArgument.ToString();
                    this.Session["strdetailsIDsession"] = detailsID;
                     
                        this.Response.Redirect("SPEnterInformationBySecurity.aspx?details=" + str, true);
                         
                     
                    
                    return;
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
        /// The Search Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            this.lblError.Visible = false;
            try
            {
                if (this.hfSearch.Checked == true)
                {
                    if (string.IsNullOrEmpty(this.txtExpress.Text))
                    {
                        ////sendSearchParams();                 
                        if (this.ValidateSearch())
                        {
                            this.BindData();
                            ////if (grdResult.Rows.Count == 0)
                            ////{
                            //    modalVisit.Show();
                            ////}
                        }
                    }
                    else
                    {
                        try
                        {
                            this.securitycity = this.GetSecurityCity();
                            string reqStatus = Convert.ToString(this.ddlReqStatus.SelectedValue);
                            ////string securityID = Session["LoginID"].ToString();
                            string[] fromdateArray = this.txtFromDate.Text.Split('/');
                            //// string fromdateActual = fromdateArray[2] + '-' + fromdateArray[1] + '-' + fromdateArray[0];
                            string[] todateArray = this.txtToDate.Text.Split('/');
                            ////  string todateActual = todateArray[2] + '-' + todateArray[1] + '-' + todateArray[0];
                            string fromTime = DateTime.Now.ToShortTimeString();
                            string format = "dd/MM/yyyy HH:mm:ss";
                            System.Globalization.CultureInfo provider =
                                System.Globalization.CultureInfo.InvariantCulture;
                            if (this.Session["currentDateTime"] != null)
                            {
                                string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                                var today = DateTime.ParseExact(currenttime, format, provider);
                                fromTime = today.ToShortTimeString();
                            }

                            GenericTimeZone genrTimeZone = new GenericTimeZone();
                            DateTime fromDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdateArray[0] + "/" + fromdateArray[1] + "/" + fromdateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                            DateTime todate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todateArray[0] + "/" + todateArray[1] + "/" + todateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                            DateTime fromdateActual = fromDate.Date; ////FromDate.ToShortDateString();
                            DateTime todateActual = todate.Date; ////toDate.ToShortDateString();
                            string strSP = "SafetyPermit";
                            DataSet dtvisitDetailsID = new DataSet();
                            string securityID = string.Empty;
                            securityID = Session["LoginID"].ToString();
                            string requestID = this.txtExpress.Text.Trim();
                            dtvisitDetailsID = (DataSet)this.requestDetails.VerifySafetyPermitId(Convert.ToInt32(requestID), securityID, fromDate.Date, todate.Date, reqStatus, strSP);
                            ////Griddata = RequestDetails.ViewLogBySecurityforSP(txtExpress.Text, txtSearch.Text, securityID, fromdateActual, todateActual, reqStatus, strSP);
                            ////if (dtVisitDetailsID.Tables[0].Rows.Count == 0)                              
                            ////{
                            //    lblError.Visible = true;
                            //    lblError.Text = Resources.LocalizedText.NoOpenRequest;
                            ////}
                            ////else
                            ////{

                            ////    //int VisitDetailsID = int.Parse(dtVisitDetailsID.Tables[0].Rows[0]["VisitDetailsID"].ToString());
                            //    //Response.Redirect("SPEnterInformationBySecurity.aspx?details=" + VisitDetailsID.ToString(), false);
                            ////}

                            if (dtvisitDetailsID.Tables[0].Rows.Count > 0)
                            {
                                this.lblResult.Visible = false;
                                this.lblStatusResult.Visible = false;
                                this.pnlRequests.Visible = true;
                                this.grdResult.DataSource = dtvisitDetailsID;
                                this.grdResult.DataBind();
                                if (VMSUtility.VMSUtility.CountryTimeZoneCheck() == true)
                                {
                                    this.btnExport.Visible = true;
                                }
                                else
                                {
                                    this.btnExport.Visible = false;
                                }

                                this.pnlImage.Visible = true;
                                //// UpnlVisitor.Visible = false;
                                this.hdnRecordFound.Value = "1";
                            }
                            else
                            {
                                this.lblStatusResult.Visible = false;
                                this.errortbl.Visible = true;
                                this.lblResult.Visible = true;
                                this.pnlRequests.Visible = false;

                                this.lblResult.Text = Resources.LocalizedText.NoRecordFound;

                                this.btnExport.Visible = false;
                                this.hdnRecordFound.Value = "0";
                                if (this.hfSearch.Checked == false)
                                {
                                    this.pnlGotoRequests.Visible = true;
                                    this.pnlGotoVisitors.Visible = false;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            this.lblError.Visible = true;
                            this.lblError.Text = Resources.LocalizedText.lblInvalidCheckinCode;
                        }
                    }
                }
                else
                {
                    this.SearchVisitors();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Search Visitors method
        /// </summary>        
        private void SearchVisitors()
        {
            ////   UpnlVisitor.Visible = true;
            this.pnlRequests.Visible = false;
            this.lblResult.Visible = false;
            ////  grdVisitor.DataBind();
            this.hfSearch.Checked = false;
            this.EnableOrDiableControls();
        }

        /// <summary>
        /// The Enable Or Disable Controls method
        /// </summary>        
        private void EnableOrDiableControls()
        {
            try
            {
                if (this.hfSearch.Checked == true)
                {
                    this.pnlGotoVisitors.Visible = true;
                    this.pnlGotoRequests.Visible = false;
                    this.grdResult.Visible = true;
                    this.txtExpress.Enabled = true;
                    this.txtFromDate.Enabled = true;
                    this.txtToDate.Enabled = true;
                    this.FromDateCalendar.Enabled = true;
                    this.imgFromDate.Enabled = true;
                    this.imgFromDate.ImageUrl = "~/Images/calender-icon.png";
                    this.imbRefreshStartdate.Enabled = true;
                    this.imbRefreshStartdate.ImageUrl = "~/Images/Refresh_image.gif";
                    this.ToDateCalendar.Enabled = true;
                    this.imgToDate.Enabled = true;
                    this.imgToDate.ImageUrl = "~/Images/calender-icon.png";
                    this.imbRefreshEnddate.Enabled = true;
                    this.imbRefreshEnddate.ImageUrl = "~/Images/Refresh_image.gif";
                    this.ddlReqStatus.Enabled = true;
                    this.btnExport.Visible = true;
                }
                else
                {
                    this.pnlGotoVisitors.Visible = false;
                    this.pnlGotoRequests.Visible = true;
                    this.grdResult.Visible = false;
                    this.txtExpress.Enabled = false;
                    this.txtFromDate.Enabled = false;
                    this.txtToDate.Enabled = false;
                    this.FromDateCalendar.Enabled = false;
                    this.ToDateCalendar.Enabled = false;
                    this.imgFromDate.Enabled = false;
                    this.imgFromDate.ImageUrl = "~/Images/calender-icon_disabled.gif";
                    this.imbRefreshStartdate.Enabled = false;
                    this.imbRefreshStartdate.ImageUrl = "~/Images/Refresh_image_disabled.gif";
                    this.imgToDate.Enabled = false;
                    this.imgToDate.ImageUrl = "~/Images/calender-icon_disabled.gif";
                    this.imbRefreshEnddate.Enabled = false;
                    this.imbRefreshEnddate.ImageUrl = "~/Images/Refresh_image_disabled.gif";
                    this.ddlReqStatus.Enabled = false;
                    this.btnExport.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Export Grid View method
        /// </summary>        
        private void ExportGridView()
        {
            try
            {
                DataTable dt = this.ExcelData();
                string attachment = "attachment; filename=Visitor List.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = string.Empty;
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }

                Response.Write("\n");

                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = string.Empty;
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }

                    Response.Write("\n");
                }

                Response.End();

                ////HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Excel Data method
        /// </summary>
        /// <returns>The System.Data.DataTable type object</returns>        
        private DataTable ExcelData()
        {
            DataTable returnTable = new DataTable();
            try
            {
                if (this.Session["LoginID"] != null)
                {
                    this.securitycity = this.GetSecurityCity();
                    string reqStatus = Convert.ToString(this.ddlReqStatus.SelectedValue);
                    string securityID = Session["LoginID"].ToString();
                    string[] fromdateArray = this.txtFromDate.Text.Split('/');
                    string[] todateArray = this.txtToDate.Text.Split('/');
                    string fromTime = DateTime.Now.ToShortTimeString();
                    string format = "dd/MM/yyyy HH:mm:ss";
                    System.Globalization.CultureInfo provider =
                        System.Globalization.CultureInfo.InvariantCulture;
                    if (this.Session["currentDateTime"] != null)
                    {
                        string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                        var today = DateTime.ParseExact(currenttime, format, provider);
                        fromTime = today.ToShortTimeString();
                    }

                    GenericTimeZone genrTimeZone = new GenericTimeZone();
                    DateTime fromDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdateArray[0] + "/" + fromdateArray[1] + "/" + fromdateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                    DateTime todate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todateArray[0] + "/" + todateArray[1] + "/" + todateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                    string fromdateActual = fromDate.ToShortDateString();
                    string todateActual = todate.ToShortDateString();
                    string strSP = "SafetyPermit";
                    this.griddata = this.requestDetails.ViewLogBySecurityforSP(this.txtSearch.Text, securityID, fromdateActual, todateActual, reqStatus, strSP);
                    for (int rowCount = 0; rowCount < this.griddata.Tables[0].Rows.Count; rowCount++)
                    {
                        bool isotherfacility = true;
                        if (this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("RETURNED") || this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("LOST"))
                        {
                            DateTime todates = Convert.ToDateTime(Convert.ToDateTime(this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString()).ToShortDateString());
                            DateTime fromdate = Convert.ToDateTime(Convert.ToDateTime(fromdateActual).ToShortDateString());
                            TimeSpan ts = todates - fromdate;
                            if ((ts.Days < 0) && (todates != fromdate))
                            {
                                this.griddata.Tables[0].Rows[rowCount].Delete();
                            }
                        }
                        else
                        {
                            // added for CR IRVMS22062010CR07  starts here done by Priti
                            for (int rowCountFac = 0; rowCountFac < this.securitycity.Tables[0].Rows.Count; rowCountFac++)
                            {
                                if (this.griddata.Tables[0].Rows[rowCount]["Facility"].ToString().Equals(this.securitycity.Tables[0].Rows[rowCountFac]["Facility"].ToString()))
                                {
                                    isotherfacility = false;
                                }
                            }

                            if (isotherfacility)
                            {
                                this.griddata.Tables[0].Rows[rowCount].Delete();
                            }
                            ////end addition for CR IRVMS22062010CR07  starts here done by Priti
                        }
                    }

                    if (this.griddata.Tables[0].Rows.Count > 0)
                    {
                        this.lblResult.Visible = false;
                        this.lblStatusResult.Visible = false;
                        this.grdResult.Visible = true;
                        string status = this.ddlReqStatus.SelectedItem.Text;

                        returnTable = this.griddata.Tables[0].Copy();
                        //// DetailsID    Cnt    VisitorID    RequestID    date    VisitDetailsID    RequestStatus

                        returnTable.Columns.Remove("DetailsID");
                        returnTable.Columns.Remove("Cnt");
                        returnTable.Columns.Remove("VisitorID");
                        returnTable.Columns.Remove("RequestID");
                        returnTable.Columns.Remove("VisitDetailsID");
                        returnTable.Columns.Remove("NativeCountry");
                        returnTable.Columns.Remove("EmailID");
                        returnTable.Columns.Remove("VisitorReferenceNo");
                        returnTable.Columns.Remove("Comments");
                        returnTable.Columns.Remove("Designation");
                        returnTable.Columns.Remove("MobileNo");
                        returnTable.Columns.Remove("Purpose");
                        returnTable.Columns.Remove("VisitingCity");
                        returnTable.Columns.Remove("Facility");
                        returnTable.Columns.Remove("HostDepartment");
                        returnTable.Columns.Remove("BuddyId");
                        returnTable.Columns.Remove("FromDate");
                        returnTable.Columns.Remove("ToDate");
                        returnTable.Columns.Remove("SafetyPermitType");

                        DataColumn sno = new DataColumn("S.No", System.Type.GetType("System.Int32"));
                        sno.AutoIncrement = true;
                        sno.AutoIncrementSeed = 1000;
                        sno.AutoIncrementStep = 1;

                        // Add the column to a new DataTable. 
                        returnTable.Columns.Add(sno);

                        returnTable.Columns["RequestStatus"].ColumnName = "Status";
                        for (int i = 0; i < returnTable.Rows.Count; i++)
                        {
                            returnTable.Rows[i]["S.No"] = i + 1;
                            if (!(status == "Select Status"))
                            {
                                returnTable.Rows[i]["Status"] = status;
                            }
                        }

                        returnTable.Columns["date"].ColumnName = "Date";
                        returnTable.Columns["inTime"].ColumnName = "InTime";
                        ////returnTable.Columns["VisitingCity"].ColumnName = "City";
                        ////returnTable.Columns["BuddyId"].ColumnName = "Buddy";
                        returnTable.Columns["ExternalRequestId"].ColumnName = "Safety Permit ID";
                        returnTable.Columns["PermitId"].ColumnName = "Request ID";
                        returnTable.Columns["S.No"].SetOrdinal(0);
                        returnTable.Columns["Safety Permit ID"].SetOrdinal(1);
                        returnTable.Columns["Request ID"].SetOrdinal(2);
                        returnTable.Columns["Status"].SetOrdinal(3);
                        returnTable.Columns["Name"].SetOrdinal(4);
                        returnTable.Columns["Company"].SetOrdinal(5);

                        returnTable.Columns["Host"].SetOrdinal(6);
                        returnTable.Columns["Date"].SetOrdinal(7);
                        returnTable.Columns["InTime"].SetOrdinal(8);
                        returnTable.Columns["ExpectedOutTime"].SetOrdinal(9);
                        returnTable.Columns["ActualOutTime"].SetOrdinal(10);
                        returnTable.Columns["BadgeNo"].SetOrdinal(11);
                        returnTable.Columns["BadgeStatus"].SetOrdinal(12);

                        ////returnTable.Columns["Designation"].SetOrdinal(3);
                        ////returnTable.Columns["MobileNo"].SetOrdinal(4);
                        ////returnTable.Columns["Purpose"].SetOrdinal(6);
                        ////returnTable.Columns["City"].SetOrdinal(7);
                        ////returnTable.Columns["Facility"].SetOrdinal(8);                        
                        ////returnTable.Columns["FromDate"].SetOrdinal(10);
                        ////returnTable.Columns["ToDate"].SetOrdinal(11);                   
                        ////returnTable.Columns["HostDepartment"].SetOrdinal(18);
                        ////returnTable.Columns["Buddy"].SetOrdinal(19);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return returnTable;
        }

        /// <summary>
        /// The Date Time method
        /// </summary>
        /// <param name="date">The Date parameter</param>
        /// <param name="time">The time parameter</param>
        /// <returns>The System.DateTime type object</returns>        
        private DateTime ConcateDateTime(string date, string time)
        {
            DateTime retDate = Convert.ToDateTime(date).AddHours(double.Parse(time.Split(':')[0])).AddMinutes(double.Parse(time.Split(':')[1]));
            return retDate;
        }

        /// <summary>
        /// The Set Status Image method
        /// </summary>
        /// <param name="strStatus">The Status parameter</param>
        /// <param name="intime">The InTime parameter</param>
        /// <param name="outTime">The OutTime parameter</param>
        /// <returns>The string type object</returns>        
        private string SetStatusImage(string strStatus, DateTime intime, DateTime outTime)
        {
            string strImageUrl = string.Empty;
            try
            {
                // DateTime dtNow = DateTime.Now;
                DateTime dtnow = this.genTimeZone.GetCurrentDate();
                switch (strStatus.ToUpper().Trim())
                {
                    case "ISSUED":
                        {
                            if (dtnow > outTime)
                            {
                                strImageUrl = "~\\images\\Reddishblink.gif";
                            }
                            else
                            {
                                strImageUrl = "~\\images\\Amber1.jpg";
                            }

                            break;
                        }

                    case "RETURNED":
                        {
                            strImageUrl = "~\\images\\Green.jpg";
                            break;
                        }

                    case "LOST":
                        {
                            strImageUrl = "~\\images\\Green.jpg";
                            break;
                        }

                    default:
                        {
                            strImageUrl = "~/Images/Black.jpg";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return strImageUrl;
        }

        /// <summary>
        /// The GetSecurityCity method
        /// </summary>
        /// <returns>The System.Data.DataSet type object</returns>        
        private DataSet GetSecurityCity()
        {
            string securityID = Session["LoginID"].ToString();
            return this.requestDetails.GetSecurityCity(securityID);
        }

        /// <summary>
        /// The ValidateSearch method
        /// </summary>
        /// <returns>The boolean type object</returns>        
        private bool ValidateSearch()
        {
            bool retValue = false;

            try
            {
                if (this.Session["LoginID"] != null)
                {
                    string reqStatus = Convert.ToString(this.ddlReqStatus.SelectedValue);
                    string securityID = Session["LoginID"].ToString();
                    if (string.IsNullOrEmpty(this.txtSearch.Text) &&
                       !string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text)
                       && string.IsNullOrEmpty(reqStatus))
                    {
                        this.lblStatusResult.Visible = false;
                        this.grdResult.Visible = false;
                        this.errortbl.Visible = true; ////commented by 173710
                        this.lblResult.Visible = true;
                        this.grdResult.Visible = false;
                        this.btnExport.Visible = false;
                        this.pnlImage.Visible = false;
                        ////  lblResult.Text = "Select atleast one search option";
                        this.lblResult.Text = Resources.LocalizedText.Selectsearch;
                    }
                    else if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
                    {
                        string[] str1 = this.txtFromDate.Text.Split('/');
                        string[] str2 = this.txtToDate.Text.Split('/');

                        ////DateTime dt1 = Convert.ToDateTime(str1[0] + "/" + str1[1] + "/" + str1[2]);
                        ////DateTime dt2 = Convert.ToDateTime(str2[0] + "/" + str2[1] + "/" + str2[2]);
                        DateTime today = this.genTimeZone.GetLocalCurrentDate();
                        string time = today.ToShortTimeString();
                        DateTime fromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(str1[0] + "/" + str1[1] + "/" + str1[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));
                        DateTime todate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(str2[0] + "/" + str2[1] + "/" + str2[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));

                        if (fromDate > todate)
                        {
                            this.errortbl.Visible = true;
                            this.lblResult.Visible = true;
                            this.grdResult.Visible = false;
                            this.btnExport.Visible = false;
                            this.pnlImage.Visible = false;
                            ////   lblResult.Text = "To date should be greater than From date";
                            this.lblResult.Text = Resources.LocalizedText.DateValid;
                            retValue = false;
                        }
                        else
                        {
                            retValue = true;
                        }
                    }
                    else
                    {
                        retValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return retValue;
        }
    }
}
