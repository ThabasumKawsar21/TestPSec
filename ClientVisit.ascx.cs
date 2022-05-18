
namespace VMSDev.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class View Log by security
    /// </summary>   
    public partial class ClientVisit : System.Web.UI.UserControl
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

#pragma warning disable CS0169 // The field 'ClientVisit.city' is never used
        /// <summary>
        /// The city field
        /// </summary>        
        private string city;
#pragma warning restore CS0169 // The field 'ClientVisit.city' is never used

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
        /// The AddNew_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnAddNew_Click(object sender, EventArgs e)
        {
            this.modalVisit.Show();
        }

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
                    this.requestDetails.Sendbadgereturnmail(dtbadgeStatus, startDate, endDate, actulOutDate);
                }
            }

            this.BindData();
        }

        ////changed by priti on 8th June for VMS CR VMS31052010CR6

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
                if (!string.IsNullOrEmpty(this.txtSearch.Text.Trim()))
                {
                    string strSearch = this.txtSearch.Text.ToString();
                    string[] arrSearch = strSearch.Split(' ');
                    foreach (string str in arrSearch)
                    {
                        if (inputTxt.ToUpper().Contains(str.ToUpper()) && (!string.IsNullOrEmpty(str)))
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
                    }
                }
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
                if (this.Session["LoginID"] != null)
                {
                    if (this.ValidateSearch())
                    {
                        this.securitycity = this.GetSecurityCity();
                        string reqStatus = string.Empty;

                        reqStatus = Convert.ToString(this.ddlReqStatus.SelectedValue);

                        string securityID = Convert.ToString(Session["LoginID"]);
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


                        this.griddata = this.requestDetails.ViewLogBySecurityClients(this.txtSearch.Text, securityID, fromdateActual, todateActual, reqStatus);
                        for (int rowCount = 0; rowCount < this.griddata.Tables[0].Rows.Count; rowCount++)
                        {
                            ////timezone conversion
                            string offset = this.griddata.Tables[0].Rows[rowCount]["Offset"].ToString();
                            DateTime vmsindate = Convert.ToDateTime(this.griddata.Tables[0].Rows[rowCount]["Date"]);
                            string vmsintime = vmsindate.ToString("MM/dd/yyyy ") + this.griddata.Tables[0].Rows[rowCount]["intime"].ToString();
                            string vmsouttime = vmsindate.ToString("MM/dd/yyyy ") + this.griddata.Tables[0].Rows[rowCount]["Expectedouttime"].ToString();
                            if (this.griddata.Tables[0].Rows[rowCount]["ActualOutTime"].ToString() != string.Empty)
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
                        } //// End of Grid records loop

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
                        if (this.griddata.Tables[0].Rows.Count == 0)
                        {
                            this.HideControls();
                        }
                    }
                    else
                    {
                        this.HideControls();                      
                    }
                }
                else
                {
                    this.HideControls();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

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
                if (!Page.IsPostBack)
                {
                   //// Session["TypeOfVisit"] = "Client";
                    this.txtFromDate.Attributes.Add("readonly", "readonly");
                    this.txtToDate.Attributes.Add("readonly", "readonly");
                    this.securitycity = this.GetSecurityCity();
                    if (this.securitycity.Tables[0].Rows[0]["CountryName"].ToString() == "India")
                    {
                        this.Session["EquipmentCustody"] = false;
                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetDefaultDate", "GetDefaultDate();", true);
                        this.lblResult.Visible = false;
                        this.lblStatusResult.Visible = false;
                        this.txtExpress.Focus();
                        Page.Form.DefaultButton = this.btnSearch.UniqueID;
                        this.btnExport.Visible = false;
                        string ddlrequeststatus = Request.QueryString["requeststatus"].ToString();
                        if (string.IsNullOrEmpty(ddlrequeststatus))
                        {
                            this.ddlReqStatus.Items.FindByValue("To Be Processed").Selected = true;
                        }
                        else
                        {
                            this.ddlReqStatus.Items.FindByValue(ddlrequeststatus).Selected = true;
                        }

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
                         
                            Response.Redirect("ViewLogbySecurity.aspx", true);
                                                                             
                    }
                }
                else
                {
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

        ///// <summary>
        ///// The AddNew_Click method
        ///// </summary>
        ///// <param name="sender">The sender parameter</param>
        ///// <param name="e">The e parameter</param>        
        //protected void BtnAddNew_Click(object sender, EventArgs e)
        //{
        //    Session["TypeOfVisit"] = "Client";
        //    this.modalVisit.Show();
           
        //}

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
                    Response.Redirect("ClientVisit.aspx?requeststatus=", true);
                     
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
            try
            {
                DataSet badgestatusclients = new DataSet();
                if (this.hfSearch.Checked == true)
                {
                    if (string.IsNullOrEmpty(this.txtExpress.Text))
                    {
                        ////sendSearchParams();                 
                        if (this.ValidateSearch())
                        {
                            this.BindData();
                            if (this.grdResult.Rows.Count == 0)
                            {
                                ////this.modalVisit.Show();
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            string securityID = string.Empty;
                            securityID = Session["LoginID"].ToString();
                            string requestID = this.txtExpress.Text.Trim();
                            ////long visitDetailsID = (long)this.requestDetails.VerifyExpressCheckinCode(Convert.ToInt32(requestID), securityID);
                            if (string.IsNullOrEmpty(requestID))
                            {
                                this.lblError.Visible = true;
                                this.lblError.Text = Resources.LocalizedText.NoOpenRequest;
                            }
                            else
                            {
                                badgestatusclients = this.requestDetails.GetBadgeStatusClients(Convert.ToInt32(requestID));
                                
                                    string TargetPage = "Clientpage.aspx?details=" + VMSBusinessLayer.Encrypt(requestID.ToString()) + "&status=" + badgestatusclients.Tables[0].Rows[0]["badgestatus"];                                
                                    Response.Redirect(TargetPage, false);

                            }
                        }
                        catch (System.Threading.ThreadAbortException ex)
                        {

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
        /// The Export_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            this.ExportGridView();
        }

        /// <summary>
        /// The Print_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnPrint_Click(object sender, EventArgs e)
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
                string strScript = "<script language='javascript'>window.open('VMSBadge.aspx?key=" + detailsID + "', 'List', 'scrollbars=no,resizable=no,width=780,height=370, location=center ');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Print_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnCollectconfirm_Click(object sender, EventArgs e)
        {
            DataTable collectordetails = new DataTable();
            VMSBusinessLayer.UserDetailsBL objuser = new VMSBusinessLayer.UserDetailsBL();
            try
            {
                string securityID = Session["LoginID"].ToString();
                string parentID = XSS.HtmlEncode(ViewState["ParentRefernceID"].ToString());

                if (string.IsNullOrEmpty(this.textcollectedby.Value))
                {
                    this.SuccessModal.Style.Add("display", "block");
                    this.tbodycart1.InnerText = "Kindly enter a valid Associate ID.";
                    ////Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"Kindly enter a valid Associate ID.\"); </script>");
                }
                else
                {
                    int collectedby = Convert.ToInt32(this.textcollectedby.Value);
                    collectordetails = objuser.GetAssociateDetails(collectedby.ToString());
                    if (collectordetails.Rows.Count > 0)
                    {
                        string collecteduser = collectordetails.Rows[0]["LastName"].ToString() + "," + collectordetails.Rows[0]["FirstName"].ToString() + "(" + collectedby + ")";
                        this.hdncollecteduser.Value = collectedby.ToString();
                        this.confirmationmodal.Style.Add("display", "block");
                        this.tbody1.InnerText = "Are you sure the vcards are handed over to  " + collecteduser;
                    }
                    else
                    {
                        this.SuccessModal.Style.Add("display", "block");
                        this.tbodycart1.InnerText = "Kindly enter a valid Associate ID.";
                    }
                    ////    this.requestDetails.Updatedispatchdetails(Convert.ToInt32(parentID), collectedby, Convert.ToInt32(securityID));
                    ////this.BindData();
                    ////Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"The Passes are dispatched to " + collectedby + ".\"); </script>");
                    ////    this.CardModel.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Print_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Btnconfirmation_Click(object sender, EventArgs e)
        {
            string securityID = Session["LoginID"].ToString();
            string parentID = XSS.HtmlEncode(ViewState["ParentRefernceID"].ToString());
            this.requestDetails.Updatedispatchdetails(Convert.ToInt32(parentID), Convert.ToInt32(this.hdncollecteduser.Value), Convert.ToInt32(securityID));
            this.SuccessModal.Style.Add("display", "block");
            this.tbodycart1.InnerText = "The passes are collected by " + this.hdncollecteduser.Value;
            this.CardModel.Style.Add("display", "none");
            this.confirmationmodal.Style.Add("display", "none");
            this.BindData();
        }

        /// <summary>
        /// The Print_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnCollectok_Click(object sender, EventArgs e)
        {
            if (this.tbodycart1.InnerText.Contains("The passes are collected by") || this.tbodycart1.InnerText.Contains("Lost") || this.tbodycart1.InnerText.Contains("Returned"))
            {
                this.SuccessModal.Style.Add("display", "none");
                this.CardModel.Style.Add("display", "none");
            }
            else
            {
                this.SuccessModal.Style.Add("display", "none");
                this.CardModel.Style.Add("display", "block");
            }

            this.textcollectedby.Value = string.Empty;
        }

        /// <summary>
        /// The Print_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Cancel_Click(object sender, EventArgs e)
        {
            this.confirmationmodal.Style.Add("display", "none");
            this.SuccessModal.Style.Add("display", "none");
            this.CardModel.Style.Add("display", "block");
            this.textcollectedby.Value = string.Empty;
        }

        /// <summary>
        /// The Hidden_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Close_Click(object sender, EventArgs e)
        {
            this.CardModel.Style.Add("display", "none");
            this.confirmationmodal.Style.Add("display", "none");
            this.SuccessModal.Style.Add("display", "none");
            this.textcollectedby.Value = string.Empty;
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
        /// The Button3_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Button3_Click(object sender, EventArgs e)
        {
            string strScript = "<script language='javascript'>window.open('VMSBadge.aspx', 'List', 'scrollbars=no,resizable=no,width=780,height=370, location=center ');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript);
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
                string detailsID = e.CommandArgument.ToString();
                string visitDetailsIDchk;
                visitDetailsIDchk = detailsID;
                string title = string.Empty;
                string summaryJSONreq = string.Empty;
                string contentJSONreq = string.Empty;
                string summaryJSONotp = string.Empty;
                string contentJSONotp = string.Empty;
                string visitplan = string.Empty;
                string summary = string.Empty;
                string templateID = string.Empty;
                string content = string.Empty;
                string requestorname = string.Empty;
                string sourcefacility = string.Empty;
                VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
                LinkButton btnCheckOut = (LinkButton)row.FindControl("btnCheckOut");
                LinkButton lbtnPrint = (LinkButton)row.FindControl("btnPrint");
                LinkButton btnCheckIn = (LinkButton)row.FindControl("btnCheckIn");
                LinkButton btnView = (LinkButton)row.FindControl("btnView");
                string securityID = Session["LoginID"].ToString();
                if (e.CommandName.ToString() == "CheckOut")
                {
                    string str = e.CommandArgument.ToString();
                    string[] strarray = str.Split(',');
                     
                        string targetpage = "ClientPage.aspx?details=" + VMSBusinessLayer.Encrypt(strarray[0]) + "&status=" + strarray[1];
                        Response.Redirect(targetpage, false);
                                                            
                    return;
                }
                else if (e.CommandName.ToString() == "Lost")
                {
                    string str = e.CommandArgument.ToString();
                    string[] strarray = str.Split(',');
                     
                        string TargetPage = "ClientPage.aspx?details=" + VMSBusinessLayer.Encrypt(strarray[0]) + "&status=" + strarray[1];
                        Response.Redirect(TargetPage, false);
                       
                   
                     
                    return;
                }
                else if (e.CommandName.ToString() == "RePrint")
                {
                    this.ViewState["DetailsID"] = e.CommandArgument.ToString();
                    this.modalReprintComments.Show();
                    return;
                }
                else if (e.CommandName.ToString() == "ViewDetailsLink")
                {
                    string str = e.CommandArgument.ToString();
                    string[] strarray = str.Split(',');
                     
                        string TargetPage = "ClientPage.aspx?details=" + VMSBusinessLayer.Encrypt(strarray[0]) + "&status=" + strarray[1];
                        Response.Redirect(TargetPage, false);                                                              
                    return;
                }
                else if (e.CommandName.ToString() == "Notify")
                {
                    MailNotification objMailNotofication = new MailNotification();
                    int str = Convert.ToInt32(e.CommandArgument); ////parentreferenceID is passed here
                    string visitorsname = string.Empty;
                    DataTable clientrequestdetails = new DataTable();
                    clientrequestdetails = this.requestDetails.GetclientdetailswithParentrefernceID(str);
                    if (clientrequestdetails.Rows.Count > 0)
                    {
                        for (var i = 0; i < clientrequestdetails.Rows.Count; i++)
                        {
                            visitorsname = visitorsname + clientrequestdetails.Rows[i]["Name"].ToString() + ',';
                        }

                        visitorsname = visitorsname.Substring(0, visitorsname.Length - 1);
                    }

                    string[] host = clientrequestdetails.Rows[0]["HostID"].ToString().Split('(');
                    string hostid = host[1].Substring(0, host[1].Length - 1);
                    string hostname = host[0];
                    this.requestDetails.UpdateNotifcationtoHost(str, Convert.ToInt32(securityID));
                    objMailNotofication.CollectNotificationToHost(
                        hostid,
                        hostname,
                        visitorsname,
                        clientrequestdetails.Rows[0]["Facility"].ToString(),
                        clientrequestdetails.Rows[0]["ParentReferenceId"].ToString(),
                        clientrequestdetails.Rows[0]["HostAssociateID"].ToString(),
                         summary, summaryJSONotp, contentJSONotp, templateID, content, title);
                    this.UpdatePanel1.Update();
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"Notification mail has been sent to Host to collect the passes.\"); </script>");
                    return;
                }

                if (e.CommandName.ToString() == "Dispatch")
                {
                    MailNotification objMailNotofication = new MailNotification();
                    int str = Convert.ToInt32(e.CommandArgument); ////parentreferenceID is passed here
                    this.ViewState["ParentRefernceID"] = e.CommandArgument.ToString();
                    this.CardModel.Style.Add("display", "block");
                    this.UpdatePanel1.Update();
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
                    LinkButton btnCheckOut = (LinkButton)e.Row.FindControl("btnCheckOut");
                    LinkButton btnLost = (LinkButton)e.Row.FindControl("btnLost");
                    LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");
                    LinkButton btndispatch = (LinkButton)e.Row.FindControl("Dispatch");
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

                    if (string.IsNullOrEmpty(strBadgeStatus))
                    {
                        btnCheckOut.Enabled = false;
                        btnLost.Enabled = false;
                        btnView.Enabled = true;
                        btndispatch.Enabled = false;
                    }
                    else
                    {
                        switch (drv["BadgeStatus"].ToString().ToUpper().Trim())
                        {
                            case "ISSUED":
                                {
                                    btnCheckOut.Enabled = true;
                                    btnLost.Enabled = true;
                                    btnView.Enabled = true;
                                    btndispatch.Enabled = false;
                                    break;
                                }

                            case "RETURNED":
                                {
                                    btnCheckOut.Enabled = false;
                                    btnLost.Enabled = false;
                                    btnView.Enabled = true;
                                    btndispatch.Enabled = false;
                                    break;
                                }

                            case "UPDATED  VCARD":
                                {
                                    btnCheckOut.Enabled = false;
                                    btnLost.Enabled = false;
                                    btnView.Enabled = true;
                                    btndispatch.Enabled = false;
                                    break;
                                }
                            case "UPDATED  ACCESS CARD":
                                {
                                    btnCheckOut.Enabled = false;
                                    btnLost.Enabled = false;
                                    btnView.Enabled = true;
                                    btndispatch.Enabled = false;
                                    break;
                                }

                            case "HOST NOTIFIED":
                                {
                                    btnCheckOut.Enabled = false;
                                    btnLost.Enabled = false;
                                    btnView.Enabled = true;
                                    btndispatch.Enabled = true;
                                    break;
                                }

                            default:
                                {
                                    btnCheckOut.Enabled = false;
                                    btnLost.Enabled = false;
                                    btnView.Enabled = true;
                                    btndispatch.Enabled = false;
                                    break;
                                }
                        }
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

                    ////merge cells 
                    if (e.Row.RowIndex > 0)
                    {
                        GridViewRow previousRow = this.grdResult.Rows[e.Row.RowIndex - 1];
                        if (this.grdResult.DataKeys[e.Row.RowIndex].Values["ParentReferenceId"].ToString() ==
                             this.grdResult.DataKeys[previousRow.RowIndex].Values["ParentReferenceId"].ToString())
                        {
                            if (previousRow.Cells[0].RowSpan == 0)
                            {
                                e.Row.Visible = false;
                            }

                            if (this.grdResult.DataKeys[e.Row.RowIndex].Values["ParentReferenceId"].ToString() == "0")
                            {
                                e.Row.Visible = false;
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
                    //dt.Columns.Remove("FrstVisitingLoc");
                    dt.Columns.Remove("RecBadgeStatus");

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
                        ////this.modalVisit.Hide();
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

                    this.griddata = this.requestDetails.ViewLogBySecurityClients(this.txtSearch.Text, securityID, fromdateActual, todateActual, reqStatus);
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
                            // for excel defect fix by 545841
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
                        returnTable.Columns.Remove("ParentReferenceId");
                        returnTable.Columns.Remove("Designation");
                        returnTable.Columns["HostAssociateID"].ColumnName = "Requester ID";
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
                        returnTable.Columns["ActualInTime"].SetOrdinal(18);
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
                    this.grdResult.Visible = true;
                    this.grdVisitor.Visible = false;
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
                    this.grdResult.Visible = false;
                    this.grdVisitor.Visible = true;
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
        }

        /// <summary>
        /// Hide conrols if no records on grid
        /// </summary>
        private void HideControls()
        {
            this.lblStatusResult.Visible = false;
            this.errortbl.Visible = true;
            this.lblResult.Visible = true;
            this.pnlRequests.Visible = false;
            this.lblResult.Text = Resources.LocalizedText.NoRecordFound;
            this.btnExport.Visible = false;
            this.hdnRecordFound.Value = "0";
            this.grdResult.DataSource = null;
            this.grdResult.DataBind();
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
