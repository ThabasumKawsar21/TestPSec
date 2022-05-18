
namespace VMSDev.UserControls
{
    using Newtonsoft.Json;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using VMSDev.OneDayAccessCardService;
    using static VMSBusinessLayer.VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;
    using Telecom.BLL;


    /// <summary>
    /// Partial class View Log by security
    /// </summary>   
    public partial class ViewLogbySecurity : System.Web.UI.UserControl
    {
        #region Variables

        /// <summary>
        /// The Facilities field
        /// </summary>        
        private List<string> facilities = new List<string>();

        /// <summary>
        /// The Search field
        /// </summary>        
        private object searchParamObj = new object();

        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();

#pragma warning disable CS0169 // The field 'ViewLogbySecurity.city' is never used
        /// <summary>
        /// The city field
        /// </summary>        
        private string city;
#pragma warning restore CS0169 // The field 'ViewLogbySecurity.city' is never used

        /// <summary>
        /// The security city field
        /// </summary>        
        private DataSet securitycity;

        /// <summary>
        /// The LocationDetails field
        /// </summary>        
        private VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();

        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();

        #endregion

        #region Events Methods

        /// <summary>
        /// The CheckOutRequest_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        public void BtnCheckOutRequest_Click(object sender, EventArgs e)
        {
            string requestId = Convert.ToString(Session["hdnVisitDetailId"]);
            if (!string.IsNullOrEmpty(requestId))
            {
                if (this.requestDetails != null)
                {
                    DataSet dtbadgeStatus = this.requestDetails.Badgereturnvalues(requestId);
                    string[] startFromDate = this.hdnCheckOutFromDate.Value.Split('/');
                    DateTime startDate = Convert.ToDateTime(startFromDate[1] + "/" + startFromDate[0] + "/" + startFromDate[2] + " " + this.hdnCheckOutFromTime.Value);
                    string[] endToDate = this.hdnCheckOutToDate.Value.Split('/');
                    DateTime endDate = Convert.ToDateTime(endToDate[1] + "/" + endToDate[0] + "/" + endToDate[2] + " " + this.hdnCheckOutToTime.Value);
                    DateTime actulOutDate = Convert.ToDateTime(endToDate[1] + "/" + endToDate[0] + "/" + endToDate[2] + " " + this.hdnCheckOutActualTimeout.Value);
                    Task.Run(() =>
                    {
                        this.requestDetails.Sendbadgereturnmail(dtbadgeStatus, startDate, endDate, actulOutDate);
                    });
                }
            }

            this.BindData();
        }

        /// <summary>
        /// Checkout button click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        public void btn_Checkout_Click(object sender, EventArgs e)
        {
            string strCheckoutDetails = this.hdnCheckoutDetails.Value;
            string visitDetailsID = this.hdnVisitDetailsId.Value;
            DateTime currentSystemTime = this.genTimeZone.GetCurrentDate();
            string reqstatus = "Out";
            string strBadgeReturned = Resources.LocalizedText.VCardReturned;
            string strCheckOut = "alert('" + strBadgeReturned + "');";
            string strCheckOutNoRecord = "alert('" + strBadgeReturned + "');window.location.href ='ViewLogbySecurity.aspx';";
            VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
            VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
            propertiesDC = this.requestDetails.DisplayInfo(Convert.ToInt32(visitDetailsID));
            if (this.requestDetails.UpdateCheckoutStatus(strCheckoutDetails, visitDetailsID, reqstatus, Session["LoginID"].ToString()))
            {
                // Updating card status using access card service if it is Lost
                IList<VisitorCard> vcardslist = new List<VisitorCard>();
                AccessCard_EApprovalClient client = new AccessCard_EApprovalClient();
                // string action = "checkout";
                var cardsList = strCheckoutDetails.Split('|');
                foreach (var item in cardsList)
                {
                    var cardNo = item.Split('-')[0];
                    var reason = item.Split('-')[1].ToLower();
                    if (reason == "lost")
                    {
                        string reasonId = "3";
                        vcardslist.Add(new VisitorCard { CardSlNumber = cardNo, CardType = "General Visitor", ReasonCode = reasonId });
                    }
                }

                //invoking service call
                client.UpdateReasonsForVisitorCards(vcardslist.ToArray());
                this.BindData();
            }

            if (this.hdnRecordFound.Value == "1")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "strCheckOut", strCheckOut, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "strCheckOutNoRecord", strCheckOutNoRecord, true);
            }

            this.UpdatePanel1.Update();

        }

        /// <summary>
        /// Checkin button click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        public void btn_Checkin_Click(object sender, EventArgs e)
        {
            string securityID = Session["LoginID"].ToString();
            string detailsID = this.hdnVisitDetailsId.Value;
            string vcardNo = this.txtVcardnumber.Text.Trim().ToUpper();
            string visitDetailsIDchk;
            visitDetailsIDchk = detailsID;
            VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
            VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
            propertiesDC = this.requestDetails.DisplayInfo(Convert.ToInt32(visitDetailsIDchk));
            VMSBusinessEntity.VisitorRequest visitorLocObj = new VMSBusinessEntity.VisitorRequest();

            if (this.requestDetails.VisitorCheckInFromPopup(Convert.ToInt32(detailsID), securityID, vcardNo) || (propertiesDC.VisitDetailProperty.BadgeStatus == "Returned"))
            {
                string strMessgae = Resources.LocalizedText.Visitorcheckinsuccessful;
                // string action = "checkin";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strMessgae + "'); </script>");
                this.BindData();
            }
            else
            {
                string strMessgae = "Entered Vcard is already in use or not active. Please assign some other card";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strMessgae + "'); </script>");
            }
        }
        /// <summary>
        /// ReIssue Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btn_ReIssue_Click(Object sender, EventArgs e)     
        {                  
            string securityID = Session["LoginID"].ToString();
            string detailsID = this.hdnVisitDetailsId.Value;
            string newVcardNo = this.txtVcard.Text.Trim().ToUpper();
            string currentCardNo = this.hdnCurrentHoldingCard.Value;
            string reason = "Lost";
            VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
            VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
            propertiesDC = this.requestDetails.DisplayInfo(Convert.ToInt32(detailsID));         
            string action = "reissue";
            if (this.requestDetails.ReIssueLostCard(currentCardNo, newVcardNo, detailsID, reason))
            {
                IList<VisitorCard> vcardslist = new List<VisitorCard>();
                AccessCard_EApprovalClient client = new AccessCard_EApprovalClient();
                if (reason.ToLower() == "lost")
                {
                    string reasonId = "3";
                    vcardslist.Add(new VisitorCard { CardSlNumber = currentCardNo, CardType = "General Visitor", ReasonCode = reasonId });
                    //invoking service call
                    client.UpdateReasonsForVisitorCards(vcardslist.ToArray());
                }                            
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('Card reissued successfully'); </script>");
                this.BindData();
                Task.Run(() =>
                {
                    this.SendCheckinNotificationToHost(Convert.ToInt32(detailsID), propertiesDC, action);
                });
            }
            else
            {
                string strMessgae = "Entered Vcard is already in use or not active. Please assign some other card";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strMessgae + "'); </script>");
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
                string strSearch = this.txtSearch.Text.ToString();
                string[] arrSearch = strSearch.Split(' ');
                foreach (string str in arrSearch)
                {
                    if (inputTxt.ToUpper().Contains(str.ToUpper()) && !string.IsNullOrEmpty(str))
                    {
                        int len = str.Length;
                        int i = 0;
                        while ((i = inputTxt.ToUpper().IndexOf(str.ToUpper(), i)) != -1)
                        {
                            inputTxt = inputTxt.Insert(i, "$");
                            inputTxt = inputTxt.Insert(i + len + 1, "^");
                            i = inputTxt.LastIndexOf('^');
                        }                        
                    }
                }
                inputTxt = inputTxt.Replace("$", "<span style=\"background-color: #FFFF00; font-weight:bolder;\">");
                inputTxt = inputTxt.Replace("^", "</span>");
            }
            catch
            {
            }

            return inputTxt;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Commented by 173710
        /// To set date fields with default values
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

        ////Sending search params to the SP

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
                        this.hdnSecurityLocation.Value = this.securitycity.Tables[0].Rows[0]["Facility"].ToString();
                        string reqStatus = Convert.ToString(this.ddlReqStatus.SelectedValue);
                        string securityID = Convert.ToString(Session["LoginID"]);
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

                        DateTime fromDate = genrTimeZone.GetdatetimedetailsinIST(
                            Convert.ToDateTime(fromdateArray[0] + "/" + fromdateArray[1] + "/" + fromdateArray[2] + " " + fromTime),
                            Convert.ToString(Session["TimezoneOffset"]));
                        fromTime = "4:00 PM";
                        DateTime todate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todateArray[0] + "/" + todateArray[1] + "/" + todateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));

                        string frmDt = fromDate.ToString("dd/MM/yyyy");
                        if (this.txtFromDate.Text != frmDt)
                        {
                            fromDate = Convert.ToDateTime(this.txtFromDate.Text);
                        }

                        string fromdateActual = fromDate.ToShortDateString();
                        string todateActual = todate.ToShortDateString();
                        
                        this.griddata = this.requestDetails.ViewLogBySecurity(this.txtSearch.Text, securityID, fromdateActual, todateActual, reqStatus,this.txtExpress.Text,this.txtVCNumber.Text);
                        for (int rowCount = 0; rowCount < this.griddata.Tables[0].Rows.Count; rowCount++)
                        {
                            bool otherfacility = true;
                            ////timezone conversion
                            string offset = this.griddata.Tables[0].Rows[rowCount]["Offset"].ToString();
                            DateTime vmsindate = Convert.ToDateTime(this.griddata.Tables[0].Rows[rowCount]["Date"]);
                            string vmsintime = vmsindate.ToString("MM/dd/yyyy ") + this.griddata.Tables[0].Rows[rowCount]["intime"].ToString();
                            string vmsouttime = vmsindate.ToString("MM/dd/yyyy ") + this.griddata.Tables[0].Rows[rowCount]["Expectedouttime"].ToString();
                            if (!string.IsNullOrEmpty(this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString()))
                            {
                                string vmsactualouttime = vmsindate.ToString("MM/dd/yyyy ") + this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString();
                                vmsactualouttime = this.genTimeZone.ToLocalTimeZone(Convert.ToDateTime(vmsactualouttime), offset);
                                this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"] = DateTime.Parse(vmsactualouttime).ToString("HH:mm");
                            }

                            vmsintime = this.genTimeZone.ToLocalTimeZone(Convert.ToDateTime(vmsintime), offset);
                            vmsouttime = this.genTimeZone.ToLocalTimeZone(Convert.ToDateTime(vmsouttime), offset);

                            this.griddata.Tables[0].Rows[rowCount]["Date"] = Convert.ToDateTime(vmsintime).ToString("MM/dd/yyyy");
                            this.griddata.Tables[0].Rows[rowCount]["intime"] = DateTime.Parse(vmsintime).ToString("HH:mm");
                            this.griddata.Tables[0].Rows[rowCount]["Expectedouttime"] = DateTime.Parse(vmsouttime).ToString("HH:mm");

                            if (this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("RETURNED") || this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("LOST"))
                            {
                                DateTime todates = DateTime.Now;
                                if (string.IsNullOrEmpty(this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString()))
                                {
                                    todates = Convert.ToDateTime(Convert.ToDateTime(this.griddata.Tables[0].Rows[rowCount]["Date"].ToString()).ToShortDateString());
                                }
                                else
                                {
                                    todates = Convert.ToDateTime(Convert.ToDateTime(this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString()).ToShortDateString());
                                }

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
                                        otherfacility = false;
                                    }
                                }

                                if (otherfacility)
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
                            this.UpnlVisitor.Visible = false;
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
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        ////Setting the Status-color based on the badgestatus
        ////Bugfix - Nightshift visitor 

        /// <summary>
        /// The Get date time details in method
        /// </summary>
        /// <param name="datetimeinfo">The date time parameter</param>
        /// <param name="timeoffsetvalue">The time off set value parameter</param>
        /// <returns>The System.DateTime type object</returns>        
        public DateTime GetdatetimedetailsinIST(DateTime datetimeinfo, string timeoffsetvalue)
        {
            datetimeinfo = Convert.ToDateTime(datetimeinfo);
            string offset;
            if (string.IsNullOrEmpty(timeoffsetvalue))
            {
                offset = "-330";
            }
            else
            {
                offset = timeoffsetvalue;
            }

            string timeZoneFormat = "Central Standard Time";
            string strIndianTimezone = timeZoneFormat;
            ////TimeZoneInfo tzinfoIndian = TimeZoneInfo.FindSystemTimeZoneById(strIndianTimezone);
            ////DateTime dtDateTime = datetimeinfo.AddMinutes(Convert.ToInt32(offset));//ToUniversalTime();
            ////dtDateTime = TimeZoneInfo.ConvertTimeFromUtc(dtDateTime, tzinfoIndian);

            TimeZoneInfo tzinfo = TimeZoneInfo.FindSystemTimeZoneById(strIndianTimezone);
            DateTime dtdateTime = datetimeinfo.AddMinutes(Convert.ToInt32("-" + offset));
            dtdateTime = TimeZoneInfo.ConvertTimeFromUtc(dtdateTime, tzinfo);

            return dtdateTime;
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
                this.Session["TypeOfVisit"] = "General";
                if (!Page.IsPostBack)
                {
                    this.txtFromDate.Attributes.Add("readonly", "readonly");
                    this.txtToDate.Attributes.Add("readonly", "readonly");
                    this.Session["EquipmentCustody"] = false;
                    Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetDefaultDate", "GetDefaultDate();", true);
                    this.lblResult.Visible = false;
                    this.lblStatusResult.Visible = false;
                    this.txtExpress.Focus();
                    Page.Form.DefaultButton = this.btnSearch.UniqueID;
                    this.btnExport.Visible = false;
                    this.ddlReqStatus.Items.FindByValue("Yet to arrive").Selected = true;
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
                else
                {
                    if (this.hdnRebind.Value == "In")
                    {
                        this.ddlReqStatus.Items.FindByValue("In").Selected = true;
                        this.BindData();
                    }
                    else if (this.hdnRebind.Value == "Out")
                    {
                        this.ddlReqStatus.Items.FindByValue("Out").Selected = true;
                        this.BindData();
                    }

                    this.txtFromDate.Attributes.Add("readonly", "readonly");
                    this.txtToDate.Attributes.Add("readonly", "readonly");
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
        /// The AddNew_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnAddNew_Click(object sender, EventArgs e)
        {
            this.Session["TypeOfVisit"] = "General";
            this.modalVisit.Show();
        }

        /// <summary>
        /// The BackToRequests_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnBackToRequests_Click(object sender, EventArgs e)
        {
            this.hfSearch.Checked = true;
            this.EnableOrDiableControls();
            this.BindData();
        }

        /// <summary>
        /// The Reset_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            this.hfSearch.Checked = true;
            if (this.hfSearch.Checked == true)
            {
                try
                {
                    Response.Redirect("ViewLogbySecurity.aspx", true);
                     
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
            else
            {
                this.txtSearch.Text = string.Empty;
                this.SearchVisitors();
            }
        }

        /// <summary>
        /// The Search_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            this.lblError.Visible = false;
            if (this.ddlReqStatus.SelectedValue == "In" || this.ddlReqStatus.SelectedValue == "Out")
            {
                this.lblSearchOR1.Visible = true;
                this.txtVCNumber.Visible = true;
              
            }
            else
            {
                this.lblSearchOR1.Visible = false;
                this.txtVCNumber.Visible = false;
            }
            try
            {
                if (this.hfSearch.Checked == true)
                {
                    //if (string.IsNullOrEmpty(this.txtExpress.Text))
                    //{
                        ////sendSearchParams();                 
                        if (this.ValidateSearch())
                        {
                            this.BindData();
                            if (this.grdResult.Rows.Count == 0)
                            {
                                if (this.ddlReqStatus.SelectedValue == "Yet to arrive")
                                {
                                    this.modalVisit.Show();
                                }
                            }
                        }
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        string securityID = string.Empty;
                    //        securityID = Session["LoginID"].ToString();
                    //        string requestID = this.txtExpress.Text.Trim();
                    //        long visitDetailsID = (long)this.requestDetails.VerifyExpressCheckinCode(Convert.ToInt32(requestID), securityID);
                    //        if (visitDetailsID == 0)
                    //        {
                    //            this.lblError.Visible = true;
                    //            this.lblError.Text = Resources.LocalizedText.NoOpenRequest;
                    //        }
                    //        else
                    //        {
                    //            Response.Redirect("VMSEnterInformationBySecurity.aspx?details=" + VMSBusinessLayer.Encrypt(visitDetailsID.ToString()), false);
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {
                    //        this.lblError.Visible = true;
                    //        this.lblError.Text = Resources.LocalizedText.lblInvalidCheckinCode;
                    //    }
                    //}
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
        /// The Export_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            this.ExportGridView();
        }

        /// <summary>
        /// The Hidden_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// The Hidden1_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnHidden1_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ErrorPage.aspx", true);
                 
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
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
                if (e.CommandName.ToString() == "ViewDetailsLink")
                {
                    string str = e.CommandArgument.ToString();

                    string Targetpage = "VMSEnterInformationBySecurity.aspx?details=" + VMSBusinessLayer.Encrypt(str);
                        Response.Redirect(Targetpage, true);
                        
                  
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
        /// The Result Row Data Bound method
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
                    Label lblVisitorID = (Label)e.Row.FindControl("lblVisitorId");
                    string visitorId = lblVisitorID.Text.Trim();
                    string encryptedVisitorID = VMSBusinessLayer.Encrypt(visitorId);
                    DateTime dtcurrentTime = this.genTimeZone.GetCurrentDate();
                    LinkButton btnCheckIn = (LinkButton)e.Row.FindControl("btnCheckIn");
                    LinkButton btnCheckOut = (LinkButton)e.Row.FindControl("btnCheckOut");
                    LinkButton lbtnPrint = (LinkButton)e.Row.FindControl("btnReIssue");
                    LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");
                    string currentTime;
                    bool todaysRequest = Convert.ToDateTime(drv["Date"].ToString()).ToString("dd/MM/yyyy").Equals(dtcurrentTime.ToString("dd/MM/yyyy"));
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
                    else if (dtcurrentTime < dtinTime.AddDays(1) && drv["Offset"].ToString() == "180")
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
                    ////Check out for Past dates 
                    DateTime PastdtforCheckout = dtcurrentTime.AddDays(-8);

                    DateTime dtCheckout = Convert.ToDateTime(drv["Date"].ToString());
                    //End Check out for Past dates 
                    if (todaysRequest || currentRequest)
                    {

                        if (string.IsNullOrEmpty(strBadgeStatus))
                        {
                            btnCheckIn.Enabled = true;
                            btnCheckOut.Enabled = false;
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
                                        lbtnPrint.Enabled = true;
                                        btnView.Enabled = true;
                                        break;
                                    }

                                case "RETURNED":
                                    {
                                        btnCheckIn.Enabled = false;
                                        btnCheckOut.Enabled = false;
                                        lbtnPrint.Enabled = false;
                                        btnView.Enabled = true;
                                        break;
                                    }

                                default:
                                    {
                                        btnCheckIn.Enabled = false;
                                        btnCheckOut.Enabled = false;
                                        lbtnPrint.Enabled = false;
                                        btnView.Enabled = true;
                                        break;
                                    }
                            }
                        }
                    }
                    //Check out for Past dates 
                    else if ((string.Compare(drv["BadgeStatus"].ToString().ToUpper().Trim(), "ISSUED") == 0) && (dtCheckout >= PastdtforCheckout))
                    {
                        btnCheckIn.Enabled = false;
                        btnCheckOut.Enabled = true;
                        lbtnPrint.Enabled = false;
                        btnView.Enabled = true;
                    }
                    //Check out for Past dates 
                    else
                    {
                        btnCheckIn.Enabled = false;
                        btnCheckOut.Enabled = false;
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
        /// The RefreshGrid_Tick method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void RefreshGrid_Tick(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// The Yes_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnYes_Click(object sender, EventArgs e)
        {
            this.modalVisit.Hide();
            this.SearchVisitors();
        }

        /// <summary>
        /// The Visitor Selected method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdVisitor_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            DataPager dapager = (DataPager)this.UpdatePanel1.FindControl("pager");
            if (e.AffectedRows <= dapager.PageSize)
            {
                dapager.Visible = false;
            }
            else
            {
                dapager.Visible = true;
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
        /// The GetSecurityCity method
        /// </summary>
        /// <returns>The System.Data.DataSet type object</returns>        
        private DataSet GetSecurityCity()
        {
            string securityID = Convert.ToString(Session["LoginID"]);
            return this.requestDetails.GetSecurityCity(securityID);
        }

        /// <summary>
        /// The ExportGridView method
        /// </summary>        
        private void ExportGridView()
        {
            try
            {
                DataTable dt = this.ExcelData();
                string attachment = "attachment; filename=Visitorlog.xls";
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
        /// The send Search method
        /// </summary>        
        private void SendSearchParams()
        {
            string reqStatus = Convert.ToString(this.ddlReqStatus.SelectedValue);
            string securityID = Session["LoginID"].ToString();
            try
            {
                if (string.IsNullOrEmpty(this.txtSearch.Text) && this.txtFromDate.Text.Trim().Equals("__/__/____") && this.txtToDate.Text.Trim().Equals("__/__/____") && string.IsNullOrEmpty(reqStatus))
                {
                    this.lblStatusResult.Visible = false;
                    this.grdResult.Visible = false;
                    this.errortbl.Visible = true;
                    this.lblResult.Visible = true;
                    this.grdResult.Visible = false;
                    this.btnExport.Visible = false;
                    this.pnlImage.Visible = false;
                    //// lblResult.Text = "Select atleast one search option";
                    this.lblResult.Text = Resources.LocalizedText.Selectsearch;
                }
                else if (this.txtFromDate.Text != "__/__/____" && this.txtToDate.Text != "__/__/____")
                {
                    string[] str1 = this.txtFromDate.Text.Split('/');
                    string[] str2 = this.txtToDate.Text.Split('/');

                    DateTime dt1 = Convert.ToDateTime(str1[1] + "/" + str1[0] + "/" + str1[2]);
                    DateTime dt2 = Convert.ToDateTime(str2[1] + "/" + str2[0] + "/" + str2[2]);

                    if (dt1 > dt2)
                    {
                        this.errortbl.Visible = true;
                        this.lblResult.Visible = true;
                        this.grdResult.Visible = false;
                        this.btnExport.Visible = false;
                        this.pnlImage.Visible = false;
                        ////  lblResult.Text = "To date should be greater than From date";
                        this.lblResult.Text = Resources.LocalizedText.DateValid;
                        this.modalVisit.Hide();
                    }
                    else
                    {
                        this.BindData();
                    }
                }
                else
                {
                    this.BindData();
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
                    string securityID = Convert.ToString(Session["LoginID"]);
                    if (string.IsNullOrEmpty(this.txtSearch.Text) &&
                       !string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text)
                       && string.IsNullOrEmpty(reqStatus))
                    {
                        this.lblStatusResult.Visible = false;
                        this.grdResult.Visible = false;
                        this.errortbl.Visible = true;
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
                        DateTime date = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(str2[0] + "/" + str2[1] + "/" + str2[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));

                        if (fromDate > date)
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
                    else if(!string.IsNullOrEmpty(this.txtVCNumber.Text))
                    {
                        if (reqStatus == "IN")
                        {
                            retValue = true;
                        }
                        else
                        {
                            this.lblResult.Text = "PLease check the cards in IN status";
                            retValue = false;
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

        /// <summary>
        /// The ExcelData method
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
                    DateTime todated = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todateArray[0] + "/" + todateArray[1] + "/" + todateArray[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                    string fromdateActual = fromDate.ToShortDateString();
                    string todateActual = todated.ToShortDateString();

                    this.griddata = this.requestDetails.ViewLogBySecurity(this.txtSearch.Text, securityID, fromdateActual, todateActual, reqStatus,this.txtExpress.Text,this.txtVCNumber.Text);
                    for (int rowCount = 0; rowCount < this.griddata.Tables[0].Rows.Count; rowCount++)
                    {
                        bool otherfacility = true;
                        if (this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("RETURNED") || this.griddata.Tables[0].Rows[rowCount]["BadgeStatus"].ToString().ToUpper().Equals("LOST"))
                        {
                            DateTime todate = Convert.ToDateTime(Convert.ToDateTime(this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString()).ToShortDateString());
                            DateTime fromdate = Convert.ToDateTime(Convert.ToDateTime(fromdateActual).ToShortDateString());
                            TimeSpan ts = todate - fromdate;
                            if ((ts.Days < 0) && (todate != fromdate))
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
                                    otherfacility = false;
                                }
                            }

                            if (otherfacility)
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
                        returnTable = this.griddata.Tables[0].Copy();
                        returnTable.Columns.Remove("DetailsID");
                        returnTable.Columns.Remove("Cnt");
                        returnTable.Columns.Remove("VisitorID");
                        returnTable.Columns.Remove("RequestID");
                        returnTable.Columns.Remove("VisitDetailsID");
                        returnTable.Columns.Remove("NativeCountry");
                        returnTable.Columns.Remove("EmailID");
                        returnTable.Columns.Remove("VisitorReferenceNo");
                        returnTable.Columns.Remove("Comments");
                        returnTable.Columns.Remove("Offset");
                        returnTable.Columns.Remove("Designation");
                        DataColumn sno = new DataColumn("S.No", System.Type.GetType("System.Int32"));
                        sno.AutoIncrement = true;
                        sno.AutoIncrementSeed = 1000;
                        sno.AutoIncrementStep = 1;
                        returnTable.Columns.Add(sno);
                        for (int i = 0; i < returnTable.Rows.Count; i++)
                        {
                            returnTable.Rows[i]["S.No"] = i + 1;
                        }

                        returnTable.Columns["date"].ColumnName = "Date";
                        returnTable.Columns["inTime"].ColumnName = "InTime";
                        returnTable.Columns["VisitingCity"].ColumnName = "City";

                        returnTable.Columns["S.No"].SetOrdinal(0);
                        returnTable.Columns["Name"].SetOrdinal(1);
                        returnTable.Columns["Company"].SetOrdinal(2);
                        returnTable.Columns["MobileNo"].SetOrdinal(3);
                        returnTable.Columns["Host"].SetOrdinal(4);
                        returnTable.Columns["Purpose"].SetOrdinal(5);
                        returnTable.Columns["City"].SetOrdinal(6);
                        returnTable.Columns["Facility"].SetOrdinal(7);
                        returnTable.Columns["Date"].SetOrdinal(8);
                        returnTable.Columns["FromDate"].SetOrdinal(9);
                        returnTable.Columns["ToDate"].SetOrdinal(10);
                        returnTable.Columns["InTime"].SetOrdinal(11);
                        returnTable.Columns["ExpectedOutTime"].SetOrdinal(12);
                        returnTable.Columns["ActualOutTime"].SetOrdinal(13);
                        returnTable.Columns["BadgeNo"].SetOrdinal(14);
                        returnTable.Columns["BadgeStatus"].SetOrdinal(15);
                        returnTable.Columns["RequestStatus"].SetOrdinal(16);
                        returnTable.Columns["HostDepartment"].SetOrdinal(17);
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
        /// The DateTime method
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
        /// The SetStatusImage method
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
        /// The Status method
        /// </summary>        
        private void InitStatus()
        {
            VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
            List<string> druserInfo = userDetailsBL.GetStatus();
            this.ddlReqStatus.DataSource = druserInfo;
            this.ddlReqStatus.DataBind();
            if ((this.ddlReqStatus.Items.Count == 0) || (this.ddlReqStatus.Items.Count > 0))
            {
                this.ddlReqStatus.Items.Insert(0, new ListItem("Select", string.Empty));
            }
        }

        /// <summary>
        /// The Reason method
        /// </summary>        
        private void InitReason()
        {
            string strType = Resources.LocalizedText.ReprintComments;
            List<string> reasonDataText = new List<string>();
            string[] reasonListArray;
            VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.MasterDataBL();
            List<string> drreason = masterDataBL.GetMasterData(strType);
            int intIterValue = 0;
            foreach (string reasonData in drreason)
            {
                reasonListArray = reasonData.ToString().Split('|');
                this.ddlReason.Items.Insert(intIterValue, new ListItem(reasonListArray[0].ToString(), intIterValue.ToString()));
                intIterValue++;
            }
        }

        /// <summary>
        /// The GetImageUrl method
        /// </summary>
        /// <param name="intime">The in time parameter</param>
        /// <param name="dt">The date parameter</param>
        /// <param name="status">The Status parameter</param>
        /// <returns>The string type object</returns>        
        private string GetImageUrl(string intime, string dt, string status)
        {
            DateTime dt1 = Convert.ToDateTime(dt);
            DateTime dt2 = Convert.ToDateTime(System.DateTime.Now.ToString("H:mm"));
            ////Bugfix - Nightshift visitor 
            DateTime intime1 = Convert.ToDateTime(intime);
            if (status.ToUpper().Equals("IN"))
            {
                ////Bugfix - Nightshift visitor 
                if (dt2 > dt1 && dt1 > intime1)
                {
                    return "~\\images\\Reddishblink.gif";
                }
                else
                {
                    return "~\\images\\Amber1.jpg";
                }
            }
            else if (status.ToUpper().Equals("OUT"))
            {
                return "~\\images\\Green.jpg";
            }
            else
            {
                return "~\\images\\Black.jpg";
            }
        }

        /// <summary>
        /// The EnableOrDisableControls method
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
                    this.grdVisitor.Visible = false;
                    ////this.pager.Visible = false;
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
                    this.grdVisitor.Visible = true;
                    ////this.pager.Visible = true;
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
        /// The SearchVisitors method
        /// </summary>        
        private void SearchVisitors()
        {
            VMSBusinessLayer.RequestDetailsBL objmasterbl = new VMSBusinessLayer.RequestDetailsBL();
            this.UpnlVisitor.Visible = true;
            this.pnlRequests.Visible = false;
            this.lblResult.Visible = false;
            this.grdVisitor.DataSource = objmasterbl.Getgrdvisitorsearchresult(this.txtSearch.Text);
            this.grdVisitor.DataBind();
            this.hfSearch.Checked = false;
            this.EnableOrDiableControls();
            if (this.grdVisitor.Rows.Count == 0)
            {
                ////this.pager.Visible = false;
            }
            else
            {
                ////this.pager.Visible = true;
            }
        }

        /// <summary>
        /// Gets local current date in format
        /// </summary>
        /// <returns>current date</returns>
        private DateTime GetLocalCurrentDateInFormat()
        {
            DateTime objDate = new DateTime();
            if (this.Session["currentDateTime"] != null)
            {
                string format;
                format = "dd/MM/yyyy HH:mm:ss";
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                string currenttime = Convert.ToString(this.Session["currentDateTime"]);
                var today = DateTime.ParseExact(currenttime, format, provider);
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "dd-MM-yyyy";
                dtfi.DateSeparator = "-";
                objDate = Convert.ToDateTime(currenttime, dtfi);
            }
            else
            {
                objDate = DateTime.Now;
            }

            return objDate;
        }

        /// <summary>
        /// Send check in mail notification to host
        /// </summary>
        /// <param name="visitDeatilID">visit detail id</param>
        /// <param name="propertiesDC">property object</param>
        private void SendCheckinNotificationToHost(int visitDeatilID, VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC, string action)
        {
            string summary = string.Empty;
            string summaryJSON = string.Empty;
            string contentJSON = string.Empty;
            string templateID = string.Empty;
            string content = string.Empty;
            string title = string.Empty;
            string hostid = string.Empty;
            string hostname = string.Empty;

            MailNotification objMailNotofication = new MailNotification();
            MailSMSNotification objMailSMSNotification = new MailSMSNotification();
            VMSBusinessLayer.RequestDetailsBL requestInfo = new VMSBusinessLayer.RequestDetailsBL();
            VMSBusinessLayer.UserDetailsBL userObj = new VMSBusinessLayer.UserDetailsBL();
            if (this.requestDetails != null)
            {
                if (propertiesDC != null)
                {
                    string strFacility = string.Empty;
                    DataTable dtlocationDetails = requestInfo.GetLocationDetailsById(propertiesDC.VisitorRequestProperty.RequestID);
                    if (dtlocationDetails.Rows.Count > 0)
                    {
                        strFacility = Convert.ToString(dtlocationDetails.Rows[0]["Facility"]);
                    }

                    string strVisitorName = string.Concat(
                    Convert.ToString(propertiesDC.VisitorMasterProperty.FirstName),
                    " ",
                    Convert.ToString(propertiesDC.VisitorMasterProperty.LastName)).ToUpper();
                    string strOrganization = Convert.ToString(propertiesDC.VisitorMasterProperty.Company);
                    string strRequestId = Convert.ToString(propertiesDC.VisitorRequestProperty.RequestID);
                    string strHostID = propertiesDC.VisitorRequestProperty.HostID.ToString().Split('(')[1].Split(')')[0].ToString();
                    var userDetails = userObj.GetAssociateDetails(strHostID);
                    string firstName = userDetails.Rows[0]["FirstName"].ToString().Trim();
                    if (!string.IsNullOrEmpty(userDetails.Rows[0]["LastName"].ToString()))
                    {
                        //removing extra comma in the bug
                       // firstName = userDetails.Rows[0]["LastName"].ToString().Trim() + "," + firstName;
                        firstName = userDetails.Rows[0]["LastName"].ToString().Trim() + " " + firstName;
                    }

                    string strHostName = firstName;
                    DataTable mailAttachment = new DataTable();
                    if (action == "checkin")
                    {
                        mailAttachment.Clear();
                        DataRow cardinfo = requestInfo.GetVisitorDetailsforMailProcess(visitDeatilID, action).Rows[0];
                        mailAttachment.Columns.Add("VisitorName", typeof(string));
                        mailAttachment.Columns.Add("VisitorType", typeof(string));
                        mailAttachment.Columns.Add("Check-in Date and Time", typeof(string));
                        mailAttachment.Columns.Add("Checked-in Location", typeof(string));
                        mailAttachment.Columns.Add("VcardNumber", typeof(string));
                        mailAttachment.Rows.Add(strVisitorName, propertiesDC.VisitorRequestProperty.Purpose,
                        cardinfo[1], strFacility, cardinfo[0]);
                        DateTime today = Convert.ToDateTime(cardinfo[1]);
                        string dttoday = today.ToString("dd/MMM/yyyy");
                        string time = today.ToShortTimeString();
                        objMailSMSNotification.SendMail(strHostName, strVisitorName, strFacility, dttoday, time, strRequestId, strHostID, mailAttachment);
                    }
                    else if (action == "checkout")
                    {
                        string expectedDate = (propertiesDC.VisitorRequestProperty.ToDate != null) ? ((DateTime)propertiesDC.VisitorRequestProperty.ToDate).ToString("dd-MM-yyy") : "";
                        string expectedTime = propertiesDC.VisitorRequestProperty.ToTime.ToString().Split(':')[0] + ":" + propertiesDC.VisitorRequestProperty.ToTime.ToString().Split(':')[1];
                        mailAttachment.Clear();
                        DataTable cardinfo = requestInfo.GetVisitorDetailsforMailProcess(visitDeatilID, action);
                        string actualTimeout = string.Empty;
                        mailAttachment.Columns.Add("Visitor Name", typeof(string));
                        mailAttachment.Columns.Add("Visitor Type", typeof(string));
                        mailAttachment.Columns.Add("Number of  visitor cards issued", typeof(string));
                        mailAttachment.Columns.Add("Card Number", typeof(string));
                        mailAttachment.Columns.Add("Checkout Location", typeof(string));
                        mailAttachment.Columns.Add("Expected Check-out time", typeof(string));
                        mailAttachment.Columns.Add("Actual Check-out time", typeof(string));
                        mailAttachment.Columns.Add("Visitor badge return status", typeof(string));
                        actualTimeout = Convert.ToDateTime(cardinfo.Rows[0]["ActualTimeOut"]).ToString("dd-MM-yyy HH:mm");
                        foreach (DataRow row in cardinfo.Rows)
                        {
                            // mailAttachment.Rows.Add(strVisitorName, propertiesDC.VisitorRequestProperty.Purpose, cardinfo.Rows.Count, row["BadgeNo"],
                            //strFacility, expectedDate + " " + expectedTime, actualTimeout, row["BadgeNo"]);
                            mailAttachment.Rows.Add(strVisitorName, propertiesDC.VisitorRequestProperty.Purpose, row["cardCount"], row["BadgeNo"],
                           strFacility, expectedDate + " " + expectedTime, actualTimeout, row["BadgeNo"]);
                        }

                        objMailSMSNotification.SendMail_Checkout(strHostName, strFacility, strRequestId, strHostID, mailAttachment);
                    }
                    else if (action == "reissue")
                    {
                        mailAttachment.Clear();
                        Mobile mobile = new Mobile();
                        DataRow cardinfo = requestInfo.GetVisitorDetailsforMailProcess(visitDeatilID, action).Rows[0];
                        mailAttachment.Columns.Add("Visitor Name", typeof(string));
                        mailAttachment.Columns.Add("Visitor Type", typeof(string));
                        mailAttachment.Columns.Add("New Card", typeof(string));
                        mailAttachment.Columns.Add("Old Card", typeof(string));
                        mailAttachment.Columns.Add("Reason for reissue", typeof(string));
                        mailAttachment.Columns.Add("Reissued on", typeof(string));
                        mailAttachment.Columns.Add("Reissued at", typeof(string));
                        mailAttachment.Rows.Add(strVisitorName, propertiesDC.VisitorRequestProperty.Purpose,
                        cardinfo[0], cardinfo[1], cardinfo[2], cardinfo[3], strFacility);
                        //"770505" FOR INFO CARDS
                        {
                            summary = "Visitor badge has been reissued to your visitor";

                            summaryJSON = "{\"Visitor Name \":{\"value\":\"" + strVisitorName + "\"}," +
                              "\"Visitor Type \":{\"value\":\"" + propertiesDC.VisitorRequestProperty.Purpose + "\"}," +                     
                             "\"Old Badge Number\":{\"value\":\"" + cardinfo[1] + "\"}," +
                              "\"New Badge Number\":{\"value\":\"" + cardinfo[0] + "\"}," +
                              "\"Re-issued On\":{\"value\":\"" + cardinfo[3] + "\"}," +
                              "\"Re-issued at\":{\"value\":\"" + strFacility + "\"},"+
                              "\"Reason for re-issue\":{\"value\":\"" + cardinfo[2] + "\",\"icon\":\"\\uD83D\\uDCC5\"}}";
                            templateID = "1";
                            content = string.Empty;
                            title = "Visitor Request";
                        }
                        //"770505" FOR INFO CARDS
                        objMailSMSNotification.SendMail_Reissue(strHostName, strFacility, strRequestId, strHostID, mailAttachment, summary, summaryJSON, templateID, content, title, contentJSON);

                    }
                }
            }
        }


        /// <summary>
        /// The Image Change Requests Page Index Changing method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void grdVisitor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VMSBusinessLayer.RequestDetailsBL objmasterbl = new VMSBusinessLayer.RequestDetailsBL();
            this.grdVisitor.DataSource = objmasterbl.Getgrdvisitorsearchresult(this.txtSearch.Text);
            this.grdVisitor.DataBind();
            this.grdVisitor.PageIndex = e.NewPageIndex;
            this.grdVisitor.DataBind();
        }
        #endregion
    }
}
