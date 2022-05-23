
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using VMSUtility;

    /// <summary>
    /// safety work permit
    /// </summary>
    public partial class SafetyWorkPermit : System.Web.UI.Page
    {
        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
        
        /// <summary>
        /// The security city field
        /// </summary>        
        private DataSet securitycity;
        
        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();
        
        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();

        /// <summary>
        /// The Initialize Dates method
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
                    ////txtFromDate.Text = today.ToShortDateString();
                    ////txtToDate.Text = today.ToShortDateString();
                }
                else
                {
                    ////var today = genTimeZone.GetCurrentDate().ToShortDateString();
                    ////txtFromDate.Text = today;
                    ////txtToDate.Text = today;
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
                        if (string.IsNullOrEmpty(this.txtExpress.Text))
                        {
                            this.griddata = this.requestDetails.ViewLogBySecurityforSPWorkDetails(securityID, reqStatus);
                        }
                        else
                        {
                        }

                        for (int rowCount = 0; rowCount < this.griddata.Tables[0].Rows.Count; rowCount++)
                        {
#pragma warning disable CS0219 // The variable 'isotherfacility' is assigned but its value is never used
                            bool isotherfacility = true;
#pragma warning restore CS0219 // The variable 'isotherfacility' is assigned but its value is never used

                            ////if (Griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("RETURNED") || Griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("LOST"))
                            ////{
                            //    DateTime todate = Convert.ToDateTime(Convert.ToDateTime(Griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString()).ToShortDateString());
                            //    DateTime fromdate = Convert.ToDateTime(Convert.ToDateTime(Convert.ToString(DateTime.Now)).ToShortDateString());
                            //    TimeSpan ts = todate - fromdate;
                            //    if ((ts.Days < 0) && (todate != fromdate))
                            //    {
                            //        Griddata.Tables[0].Rows[rowCount].Delete();
                            //    }
                            ////}
                            ////else
                            ////{
                            //    // added for CR IRVMS22062010CR07  starts here done by Priti
                            //    for (int rowCountFac = 0; rowCountFac < securitycity.Tables[0].Rows.Count; rowCountFac++)
                            //    {
                            //        if (Griddata.Tables[0].Rows[rowCount]["Facility"].ToString().Equals(securitycity.Tables[0].Rows[rowCountFac]["Facility"].ToString()))
                            //        {
                            //            Isotherfacility = false;
                            //        }
                            //    }
                            //    if (Isotherfacility)
                            //    {
                            //        Griddata.Tables[0].Rows[rowCount].Delete();
                            //    }
                            //    //end addition for CR IRVMS22062010CR07  starts here done by Priti
                            ////}
                        }

                        if (this.griddata.Tables[0].Rows.Count > 0)
                        {
                            ////lblResult.Visible = false;
                            ////lblStatusResult.Visible = false;
                            ////pnlRequests.Visible = true;
                            this.grdResult.DataSource = this.griddata;
                            this.grdResult.DataBind();
                            this.grdResult.Visible = true;
                            this.pnlImage.Visible = true;
                            this.lblResult.Visible = false;
                            if (VMSUtility.CountryTimeZoneCheck() == true)
                            {
                                ////btnExport.Visible = true;
                            }
                            else
                            {
                                ////btnExport.Visible = false;
                            }

                            ////pnlImage.Visible = true;
                            ////  UpnlVisitor.Visible = false;
                            ////hdnRecordFound.Value = "1";
                        }
                        else
                        {
                            this.grdResult.Visible = false;
                            this.pnlImage.Visible = false;
                            this.lblResult.Text = Resources.LocalizedText.NoRecordFound;
                            this.lblResult.Visible = true;
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
        /// The Highlight method
        /// </summary>
        /// <param name="inputTxt">The InputTxt parameter</param>
        /// <returns>The string type object</returns>        
        public string Highlight(string inputTxt)
        {
            try
            {
                string strSearch = this.txtExpress.Text.ToString();
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
                    ////txtExpress.Focus();
                    this.txtExpress.Enabled = true;
                    this.TBWE2.Enabled = true;
                    this.ddlReqStatus.Items.FindByValue("Yet to arrive").Selected = true;
                    this.hfSearch.Checked = true;
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
            }
        }

        /// <summary>
        /// The button search_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Btnsearch_Click(object sender, ImageClickEventArgs e)
        {
            ////comm
           
            ////lblError.Visible = false;
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
                          
                            ////string[] fromdateArray = txtFromDate.Text.Split('/');
                           
                            ////string[] todateArray = txtToDate.Text.Split('/');
                          
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
                            DataSet dtvisitDetailsID = new DataSet();
                            string securityID = string.Empty;
                            securityID = Session["LoginID"].ToString();
                            string requestID = this.txtExpress.Text.Trim();
                            dtvisitDetailsID = (DataSet)this.requestDetails.SafetyPermitSecuritySearch(securityID, reqStatus, requestID);  

                            if (dtvisitDetailsID.Tables[0].Rows.Count > 0)
                            {
                                ////lblResult.Visible = false;
                                ////lblStatusResult.Visible = false;
                                ////pnlRequests.Visible = true;
                                this.grdResult.DataSource = dtvisitDetailsID;
                                this.grdResult.DataBind();
                                this.grdResult.Visible = true;
                                this.pnlImage.Visible = true;
                                this.lblResult.Visible = false;
                                if (VMSUtility.CountryTimeZoneCheck() == true)
                                {
                                    ////btnExport.Visible = true;
                                }
                                else
                                {
                                    ////btnExport.Visible = false;
                                }

                                ////pnlImage.Visible = true;
                                // UpnlVisitor.Visible = false;
                                this.hdnRecordFound.Value = "1";
                            }
                            else
                            {
                                this.grdResult.Visible = false;
                                this.pnlImage.Visible = false;
                                this.lblResult.Text = Resources.LocalizedText.NoRecordFound;
                                this.lblResult.Visible = true;
                                this.hdnRecordFound.Value = "0";
                                if (this.hfSearch.Checked == false)
                                {
                                    ////pnlGotoRequests.Visible = true;
                                    ////pnlGotoVisitors.Visible = false;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ////lblError.Visible = true;
                            ////lblError.Text = Resources.LocalizedText.lblInvalidCheckinCode;
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
        /// The grid Result_RowCommand method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;

                LinkButton btnCheckOut = (LinkButton)row.FindControl("btnCheckOut");
                LinkButton btnCheckIn = (LinkButton)row.FindControl("btnCheckIn");
                string securityID = Session["LoginID"].ToString();
                if (e.CommandName.ToString() == "CheckIn")
                {
                    DateTime currentSystemTime = this.genTimeZone.GetCurrentDate();
                    DateTime fromTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                    string actualouttime = currentSystemTime.ToLongTimeString();
                    string reqstatus = "In";
                    string comments = string.Empty;
                    string strCheckOut = string.Empty;
                    string permitId = e.CommandArgument.ToString();
                    strCheckOut = "alert(CheckIn Successful.)";
                    this.requestDetails.PermitCheckIn(permitId, reqstatus, fromTime);
                    this.BindData();
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"CheckIn Successful.\"); </script>");
                    return;
                }

                if (e.CommandName.ToString() == "CheckOut")
                {
                    DateTime currentSystemTime = this.genTimeZone.GetCurrentDate();
                    DateTime totime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                    string actualouttime = currentSystemTime.ToLongTimeString();
                    string reqstatus = "Out";
                    string comments = string.Empty;
                    string strCheckOut = string.Empty;
                    string permitId = e.CommandArgument.ToString();                   
                    strCheckOut = "alert(CheckOut Successful.)";
                    this.requestDetails.PermitCheckOut(permitId, reqstatus, totime);
                    this.BindData();
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"CheckOut Successful.\"); </script>");
                    return;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            { 
            }
        }

        /// <summary>
        /// The grid Result_RowDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DateTime dtcurrentTime = this.genTimeZone.GetCurrentDate();
                    LinkButton btnCheckIn = (LinkButton)e.Row.FindControl("btnCheckIn");
                    LinkButton btnCheckOut = (LinkButton)e.Row.FindControl("btnCheckOut");
                    Image imgStatus = (Image)e.Row.FindControl("image");
                    DateTime dtexpectedOutTime;
                    string expectedouttime;
                    string currentTime;
                    currentTime = dtcurrentTime.ToString("H:mm");
                    if (string.IsNullOrEmpty(Convert.ToString(drv["CheckoutTime"])))
                    {
                        expectedouttime = dtcurrentTime.ToString("H:mm");
                    }
                    else
                    {
                        expectedouttime = drv["CheckoutTime"].ToString();
                    }

                    dtexpectedOutTime = Convert.ToDateTime(expectedouttime);
                    string strStatus = drv["Status"].ToString();
                    imgStatus.ImageUrl = this.SetStatusImage(strStatus, dtexpectedOutTime);
                    switch (drv["Status"].ToString().ToUpper().Trim())
                    {
                        case "YET TO ARRIVE":
                            {
                                btnCheckIn.Enabled = true;
                                btnCheckOut.Enabled = false;
                                break;
                            }

                        case "IN":
                            {
                                btnCheckIn.Enabled = false;
                                btnCheckOut.Enabled = true;
                                break;
                            }

                        case "OUT":
                            {
                                btnCheckIn.Enabled = false;
                                btnCheckOut.Enabled = false;
                                break;
                            }

                        default:
                            {
                                btnCheckIn.Enabled = false;
                                btnCheckOut.Enabled = false;
                                break;
                            }
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
            }
        }

        /// <summary>
        /// The button Hidden_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// The button Reset_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            this.txtExpress.Text = string.Empty;
            this.ddlReqStatus.SelectedIndex = 1;
            this.BindData();
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
        /// The SearchVisitors method
        /// </summary>        
        private void SearchVisitors()
        {
            ////   UpnlVisitor.Visible = true;
            ////pnlRequests.Visible = false;
            ////lblResult.Visible = false;
            //  grdVisitor.DataBind();
            this.hfSearch.Checked = false;
            this.EnableOrDiableControls();
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
        /// The Enable Or Disable Controls method
        /// </summary>        
        private void EnableOrDiableControls()
        {
            try
            {
                if (this.hfSearch.Checked == true)
                {
                    this.grdResult.Visible = true;
                    this.pnlImage.Visible = true;
                    this.txtExpress.Enabled = true;
                    this.ddlReqStatus.Enabled = true;
                }
                else
                {
                    this.grdResult.Visible = false;
                    this.pnlImage.Visible = false;
                    this.txtExpress.Enabled = false;
                    this.ddlReqStatus.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
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
                    if (string.IsNullOrEmpty(this.txtExpress.Text) && reqStatus.Equals("Select Status"))
                    {
                        this.grdResult.Visible = false;
                        this.pnlImage.Visible = false;
                        this.lblResult.Visible = true;
                        this.lblResult.Text = "Select atleast one search option";
                        this.lblResult.Text = Resources.LocalizedText.Selectsearch;
                    }
                    else
                    {
                        retValue = true;
                    }
                    ////else if !string.IsNullOrEmpty txtFromDate.Text && !String.IsNullOrEmpty txtToDate.Text
                    ////{
                    ////   string[] str1 = txtFromDate.Text.Split('/');
                    ////    string[] str2 = txtToDate.Text.Split('/');
                    ////    DateTime dt1 = Convert.ToDateTime(str1[0] + "/" + str1[1] + "/" + str1[2]);
                    ////    DateTime dt2 = Convert.ToDateTime(str2[0] + "/" + str2[1] + "/" + str2[2]);
                    ////    DateTime Today = genTimeZone.GetLocalCurrentDate();
                    ////    string time = Today.ToShortTimeString();
                    ////    DateTime FromDate = genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(str1[0] + "/" + str1[1] + "/" + str1[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));
                    ////    DateTime ToDate = genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(str2[0] + "/" + str2[1] + "/" + str2[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));
                    ////    if (FromDate > ToDate)
                    ////    {
                    ////        errortbl.Visible = true;
                    ////        lblResult.Visible = true;
                    ////        grdResult.Visible = false;
                    ////        btnExport.Visible = false;
                    ////        pnlImage.Visible = false;
                    ////          lblResult.Text = "To date should be greater than From date";
                    ////        lblResult.Text = Resources.LocalizedText.DateValid;
                    ////        retValue = false;
                    ////    }
                    ////    else
                    ////    {
                    ////        retValue = true;
                    ////    }
                    ////}
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return retValue;
        }

        /// <summary>
        /// The SetStatusImage method
        /// </summary>
        /// <param name="strStatus">The Status parameter</param>
        /// <param name="outTime">The OutTime parameter</param>
        /// <returns>The string type object</returns>        
        private string SetStatusImage(string strStatus, DateTime outTime)
        {
            string strImageUrl = string.Empty;
            try
            {
                // DateTime dtNow = DateTime.Now;
                DateTime dtnow = this.genTimeZone.GetCurrentDate();
                switch (strStatus.ToUpper().Trim())
                {
                    case "IN":
                        {
                            if (dtnow > outTime)
                            {
                                strImageUrl = "~\\images\\redBall.png";
                            }
                            else
                            {
                                strImageUrl = "~\\images\\orangeBall.png";
                            }

                            break;
                        }

                    case "OUT":
                        {
                            strImageUrl = "~\\images\\greenBall.png";
                            break;
                        }

                    case "YET TO ARRIVE":
                        {
                            strImageUrl = "~/Images/blaclBall.png";
                            break;
                        }

                    default:
                        {
                            strImageUrl = "~/Images/blaclBall.png";
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
    }
}
