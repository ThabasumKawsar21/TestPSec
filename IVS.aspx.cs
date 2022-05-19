
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.Reporting.WebForms;
    using VMSBL;
    using VMSBusinessLayer;
    using VMSConstants;
    using VMSDev.OneCommunicatorService;
    using VMSDev.OneDayAccessCardService;
    using XSS = CAS.Security.Application.EncodeHelper;
    using System.Web.Services;
    using ECMCommon;
    using CAS.Security.Application;
    using ECMSharedServices;
    using Newtonsoft.Json;

    /// <summary>
    /// Partial class IVS
    /// </summary>    
    public partial class IVS : System.Web.UI.Page
    {
        /// <summary>
        /// Dataset for select card DDL
        /// </summary>
        private DataSet dtselectCardddl = new DataSet();
        #region Variables
        /// <summary>
        /// VMS BusinessLayer
        /// </summary>
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// Dataset for security details
        /// </summary>
        private DataSet securitydetails = new DataSet();

        /// <summary>
        /// Get City Name
        /// </summary>
        private string cityName = string.Empty;

        /// <summary>
        /// Get Facility Name
        /// </summary>
        private string facility = string.Empty;

        /// <summary>
        /// Get Location
        /// </summary>
        private string strLocation = string.Empty;

        /// <summary>
        /// start Added for 1 day access card PAN IND RollOut
        /// </summary>
        private string panIND = ConfigurationManager.AppSettings["OnedayAccessCard_PANIND"].ToString();

        /// <summary>
        /// Get Country
        /// </summary>
        private string strCountry = string.Empty;

        /// <summary>
        /// End Added for 1 day access card PAN IND RollOut
        /// </summary>
        private string locationId = string.Empty;

        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();

        /// <summary>
        /// declaring the ECM service object.
        /// </summary>
        private static WrapperCheckIn objCheckInServices;

        /// <summary>
        /// Used to assign application ID
        /// </summary>
        private static int appId = Convert.ToInt32(ConfigurationManager.AppSettings["ECMAPPID"]);
        #endregion

        #region Control Events

        /// <summary>
        /// Method to Assign Time Zone Offset
        /// </summary>
        /// <param name="strTimezoneoffset">Time zone offset</param>
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
        /// Method to Assign Current Date Time
        /// </summary>
        /// <param name="currentDate">current Date</param>
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
        /// Method to get dates
        /// </summary>
        public void InitDates()
        {
            try
            {
                string format = "dd/MM/yyyy HH:mm:ss";
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                if (this.Session["currentDateTime"] != null)
                {
                    string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                    var today = DateTime.ParseExact(currenttime, format, provider);
                    this.txtFromDate.Text = today.ToString("dd/MMM/yyyy", provider);
                    this.txtToDate.Text = today.ToString("dd/MMM/yyyy", provider);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to check access
        /// </summary>
        /// <param name="count">count of check</param>
        public void Access_Check(string count)
        {
            string accessCardCheck = string.Concat(Resources.LocalizedText.AccessCardCheck);
            string strScript = string.Concat("<script>alert('", accessCardCheck, "'); </script>");
            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);

            this.txtAccess.Text = string.Empty;
            count = string.Empty;
        }

        /// <summary>
        /// Method to call ECM Download Service for getting Chire image
        /// </summary>
        /// <param name="associateId">associate id</param>
        /// <returns>the image of the associate</returns>
        [WebMethod]
        public static string GetChireImageFromECM(string contentId)
        {
            byte[] fileContent = null;
            string imageUrl = VMSConstants.IMAGEPATH;
            //string contentId = hdnFileUploadID.Value;
            string ecmContentId = EncodeHelper.HtmlEncode(contentId);
            //int appId = 1132;
            if (!string.IsNullOrEmpty(ecmContentId))
            {
                WrapperUICheckIn checkInObj = new WrapperUICheckIn(appId);
                //ECMFileURLResult url = new ECMFileURLResult();       
                ECMCommon.IdcFile fileVal = new ECMCommon.IdcFile();
                fileVal = checkInObj.DownloadFileContent(ecmContentId, appId);

                 fileContent = fileVal.Filecontent;
                // url = objCheckInServices.GetNativeFileURL(ecmContentId, 0, appId, true, false, );

                imageUrl = JsonConvert.SerializeObject("data:image/png;base64," + Convert.ToBase64String(fileContent));
            }

            return imageUrl;
        }

        /// <summary>
        /// Method to set controls on Page Load
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Ajax.Utility.RegisterTypeForAjax(typeof(IVS));
                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                ////Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetDefaultTime", "SetDefaultTime();", true);   
                if (!Page.IsPostBack)
                {
                    ////  Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetDefaultTime", "SetDefaultTime();", true);        
                    this.FillControlValues();
                    this.btnSearch.Focus();
                    Page.Form.DefaultButton = this.btnSearch.UniqueID;
                    this.errortbl.Visible = false;
                    this.SetVisibilityLevel("PAGELOAD");
                    this.InitDates();
                    if (this.Session["currentDateTime"] != null)
                    {
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
                Console.Write(ex);
                ////Utility.VMSUtility.logExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method on click of button
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnIVSHidden_Click(object sender, EventArgs e)
        {
            // InitDates();
            this.BindData();
        }

        /// <summary>
        /// Method to handle Search Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Session["LoginID"] != null)
                {
                    this.FillControlValues();
                    ////InitDates();
                    this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                    string strErrorMsg = string.Empty;
                    this.lblMessage.Text = string.Empty;
                    string strAssociateID;
                    strAssociateID = XSS.HtmlEncode(Convert.ToString(this.txtEmpID.Text).Trim());
                    string strgLocation = string.Empty;
                    string strgCountry = string.Empty; ////Added for 1 day access card PAN IND RollOut                   
                    ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetOffset", "GetOffset();", true);
                    EmployeeBL objEmployeDetails = new EmployeeBL();
                    this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    string accessCardNo = string.Empty;
                    string currentdatefetch = string.Empty;
                    string[] onedayAccesscard = this.panIND.Split(',');
                    string format;
                    format = "dd/MM/yyyy HH:mm:ss";
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    string Curdate = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                    var today1 = DateTime.ParseExact(Curdate, format, provider);
                    //  DateTime CurDate = Convert.ToDateTime(HttpContext.Current.Session["currentDateTime"]);
                    if (this.securitydetails != null)
                    {
                        if (this.securitydetails.Tables[0].Rows.Count > 0)
                        {
                            strgLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                            strgCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                        }

                        DataSet griddata = objEmployeDetails.GetODICardsIssued(strgLocation, strAssociateID, today1);

                        int rowCount = griddata.Tables[0].Rows.Count;
                        string strgDetailId = string.Empty;
                        int flag = 0;
                        for (int i = 0; i < rowCount; i++)
                        {
                            if ((griddata.Tables[0].Rows[i]["BadgeStatus"].ToString() == "Issued") &&
                                (griddata.Tables[0].Rows[i]["AssociateID"].ToString() == strAssociateID))
                            {
                                accessCardNo = griddata.Tables[0].Rows[i]["AccessCardNo"].ToString();
                                currentdatefetch = griddata.Tables[0].Rows[i]["IssuedDate"].ToString();
                                strgDetailId = griddata.Tables[0].Rows[i]["PassDetailID"].ToString();
                                flag = 1;
                                break;
                            }
                            else
                            {
                                flag = 0;
                            }
                        }

                        if (flag == 1)
                        {
                            string strMessage = string.Concat(Resources.LocalizedText.Notification);
                            this.lblTempIdIssued.Text = strMessage;
                            this.lblTempIdIssued.Visible = true;
                            this.RdlReason.Enabled = false;
                            VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                            DataSet dtset = new DataSet();
                            dtset = objaccess.GetAccessCardDetails(strAssociateID, this.hdnFacility.Value.ToString(), strgDetailId);

                            this.accesstextid.Visible = false;
                            this.accesstextid.Enabled = false;
                            this.ddAccessdetail.Visible = true;
                            this.DDAccessLabel.Enabled = true;

                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (!string.IsNullOrEmpty(strCountrychk))
                                {
                                    this.InitDates();
                                }
                                else
                                {
                                    //System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                                    // string format = "dd/MM/yyyy HH:mm:ss";
                                    var fromdatefetch = DateTime.Parse(currentdatefetch); ////DateTime.ParseExact(thisdate, format, provider);
                                    this.txtFromDate.Text = fromdatefetch.ToString("dd/MMM/yyyy", provider);
                                    if (this.Session["currentDateTime"] != null)
                                    {
                                        string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                                        var today = DateTime.ParseExact(currenttime, format, provider);
                                        this.txtFromDate.Text = today.ToString("dd/MMM/yyyy", provider);
                                        this.txtToDate.Text = today.ToString("dd/MMM/yyyy", provider);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (!string.IsNullOrEmpty(strCountrychk))
                                {
                                    this.InitDates();
                                }
                                else
                                {
                                    //  System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                                    //  string format = "dd/MM/yyyy HH:mm:ss";
                                    if (this.Session["currentDateTime"] != null)
                                    {
                                        string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                                        var today = DateTime.ParseExact(currenttime, format, provider);
                                        this.txtFromDate.Text = today.ToString("dd/MMM/yyyy", provider);
                                        this.txtToDate.Text = today.ToString("dd/MMM/yyyy", provider);
                                    }
                                }
                            }

                            this.lblTempIdIssued.Text = string.Empty;
                            this.lblTempIdIssued.Visible = false;
                            this.RdlReason.Enabled = true;
                            VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                            this.dtselectCardddl = objaccess.GetSelect_CARD();
                            this.ddAccessdetail.DataSource = this.dtselectCardddl;
                            this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Temporary_Cards"].ToString();
                            this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                            this.ddAccessdetail.DataBind();
                            this.txtAccess.Visible = false;
                            this.LblAccess.Visible = false;
                        }
                    }

                    foreach (string strCountrychk in onedayAccesscard)
                    {
                        if (strCountrychk != strgCountry)
                        {
                            this.ddAccessdetail.Enabled = false;
                            this.imgToDate.Visible = true;
                        }
                        else
                        {
                            this.ddAccessdetail.Enabled = true;
                            this.imgToDate.Visible = true;
                        }
                    }

                    if (strAssociateID != string.Empty)
                    {
                        //// EmployeeBL objEmployeeDetails = new EmployeeBL();
                        this.lblTerminated.Visible = false;

                        if (objEmployeDetails.ValidateAssociateDetails(strAssociateID))
                        {
                            this.PopulateAssociateData(strAssociateID, objEmployeDetails);
                        }
                        else
                        {
                            this.SetVisibilityLevel("PAGELOAD");
                            this.errortbl.Visible = true;
                            this.lblMessage.Text = Resources.LocalizedText.InValidAssociateId;
                            ////Commented for CR 37
                            DataRow dr = this.PopulateTerminatedAssociateData(strAssociateID, objEmployeDetails);
                            if (dr == null)
                            {
                                this.lblMessage.Visible = true;
                                this.lblMessage.Text = Resources.LocalizedText.InValidAssociateId;
                            }
                            else if (Convert.ToString(dr["HR_Status"]) == "I")
                            {
                                this.btnCheckIn.Enabled = false;
                                this.btnCheckOut.Enabled = false;
                                this.btnReprint.Enabled = false;
                                this.btnPrint.Enabled = false;
                                this.RdlReason.Enabled = false;
                                this.lblTerminated.Visible = true;
                                this.lblTerminated.Text = Resources.LocalizedText.TerminatedMessage;
                                string strScript = string.Concat("<script>alert('", Resources.LocalizedText.TerminatedMessage, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                this.lblMessage.Visible = false;
                                ////lblMessage.Visible = true;
                                this.lblIDCardStatus.Visible = false;

                            }
                        }
                    }
                    else
                    {
                        this.errortbl.Visible = true;
                        this.lblIDCardStatus.Visible = false;
                        this.SetVisibilityLevel("PAGELOAD");
                        this.lblMessage.Text = Resources.LocalizedText.EnterAssociateId;
                        return;
                    }

                    foreach (string strCountrychk in onedayAccesscard)
                    {
                        if (strCountrychk == strgCountry)
                        {
                            DataSet checkaccesscardhistory = new DataSet();
                            VMSBusinessLayer.RequestDetailsBL objcheckhistory = new VMSBusinessLayer.RequestDetailsBL();
                            checkaccesscardhistory = objcheckhistory.CheckaccesscardhistoryBL(this.txtEmpID.Text);
                            DataRow dr = this.PopulateTerminatedAssociateData(strAssociateID, objEmployeDetails);
                            if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                            {
                                if (Convert.ToString(dr["HR_Status"]) == "I")
                                {

                                    this.btnCheckIn.Enabled = false;
                                    this.btnCheckOut.Enabled = false;
                                    this.btnReprint.Enabled = false;

                                }
                                else
                                {

                                    if (checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                    {
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                    }
                                    else if (checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                    {
                                        this.btnCheckIn.Enabled = false;
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                    }
                                }
                            }

                            else if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                            {
                                if (Convert.ToString(dr["HR_Status"]) == "I")
                                {

                                    this.btnCheckIn.Enabled = false;
                                    this.btnCheckOut.Enabled = false;
                                    this.btnReprint.Enabled = false;

                                }
                                else
                                {

                                    if (checkaccesscardhistory.Tables[0].Rows.Count == 1)
                                    {
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                    }
                                    else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2)
                                    {
                                        this.btnCheckIn.Enabled = false;
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                    }
                                }
                            }

                            else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                            {
                                if (Convert.ToString(dr["HR_Status"]) == "I")
                                {

                                    this.btnCheckIn.Enabled = false;
                                    this.btnCheckOut.Enabled = false;
                                    this.btnReprint.Enabled = false;

                                }
                                else
                                {

                                    if (checkaccesscardhistory.Tables[0].Rows.Count == 1 || checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                    {
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                    }
                                    else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                    {
                                        this.btnCheckIn.Enabled = false;
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to clear values once the clear button is clicked      
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.InitDates();
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
            ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
            try
            {
                this.SetVisibilityLevel("PAGELOAD");
                this.txtEmpID.Text = string.Empty;
                this.accesstextid.Text = string.Empty;
                this.txtAccess.Text = string.Empty;
                this.errortbl.Visible = false;
                this.BindData();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to handle Check In Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
#pragma warning disable CS0219 // The variable 'strCheckStatus' is assigned but its value is never used
                string strCheckStatus = VMSConstants.CHECKIN;
#pragma warning restore CS0219 // The variable 'strCheckStatus' is assigned but its value is never used
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                VMSBusinessLayer.RequestDetailsBL objFacilityID = new VMSBusinessLayer.RequestDetailsBL();
                AccessCard_EApprovalClient client = new AccessCard_EApprovalClient();
                string strNumber = string.Empty;
                string strUserId = string.Empty;
                string strAssociateID = string.Empty;
                /////added by Krishna(449138) for temp accesscard
                string facilityName = string.Empty;
                string cityName1 = string.Empty;
                string countryName = string.Empty;

                string strAccessCardNo = this.txtAccess.Text.Trim();
                strAssociateID = Convert.ToString(this.txtEmpID.Text).Trim();
                string[] arrDtFromDate = this.txtFromDate.Text.Split('/');
                DateTime today = this.genTimeZone.GetLocalCurrentDateInFormat();


                string time = today.ToString("HH:mm");  // Today.ToShortTimeString();                                                
                                                        //  DateTime dateFromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(arrDtFromDate[0] + "/" + arrDtFromDate[1] + "/" + arrDtFromDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));

                //string dateFromDate_str = this.genTimeZone.ToLocalTimeZone(Convert.ToDateTime(arrDtFromDate[0] + "/" + arrDtFromDate[1] + "/" + arrDtFromDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));
                //DateTime dateFromDate = Convert.ToDateTime(dateFromDate_str);

                DateTime dateFromDate = Convert.ToDateTime(arrDtFromDate[0] + "/" + arrDtFromDate[1] + "/" + arrDtFromDate[2] + " " + time);

                // DateTime datetFromDate1 = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(arrDtFromDate[0] + "/" + arrDtFromDate[1] + "/" + arrDtFromDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));

                //string datetFromDate1_str = this.genTimeZone.ToLocalTimeZone(Convert.ToDateTime(arrDtFromDate[0] + "/" + arrDtFromDate[1] + "/" + arrDtFromDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));
                //DateTime datetFromDate1 = Convert.ToDateTime(datetFromDate1_str);

                DateTime datetFromDate1 = Convert.ToDateTime(arrDtFromDate[0] + "/" + arrDtFromDate[1] + "/" + arrDtFromDate[2] + " " + time);
                string[] arrDtToDate = this.txtToDate.Text.Split('/');
                // DateTime dateToDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(arrDtToDate[0] + "/" + arrDtToDate[1] + "/" + arrDtToDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));
                //string dateToDate_str = this.genTimeZone.ToLocalTimeZone(Convert.ToDateTime(arrDtToDate[0] + "/" + arrDtToDate[1] + "/" + arrDtToDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));

                //DateTime dateToDate = Convert.ToDateTime(dateToDate_str);
                DateTime dateToDate = Convert.ToDateTime(arrDtToDate[0] + "/" + arrDtToDate[1] + "/" + arrDtToDate[2] + " " + time);
                int dayTo = dateToDate.Day;
                int dayFrom = dateFromDate.Day;
                TimeSpan ts = dateToDate - dateFromDate;
                int dateDiff = Convert.ToInt32(ts.TotalDays);
                try
                {
                    strUserId = Session["LoginID"].ToString();
                }
                catch (System.NullReferenceException)
                {
                    try
                    {
                        Response.Redirect("~/SessionExpired.aspx", true);

                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {

                    }
                }
                ////check access cardnumber 
                VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();

                ////Check duplicate entry               
                string facilityId = objFacilityID.GetFacilityIDForAccessCard(this.hdnFacility.Value);

                /* access card no duplicate check as per new change--done by ram*/
                string[] onedayAccesscard = this.panIND.Split(',');
                this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                if (this.securitydetails.Tables[0].Rows.Count > 0)
                {
                    this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                    this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                    ////added by Krishna(449138) for temp access card
                    facilityName = this.securitydetails.Tables[0].Rows[0]["Facility"].ToString();
                    cityName1 = this.securitydetails.Tables[0].Rows[0]["City"].ToString();
                    countryName = this.securitydetails.Tables[0].Rows[0]["CountryName"].ToString();
                }

                foreach (string strCountrychk in onedayAccesscard)
                {
                    if (strCountrychk == this.strCountry)
                    {
                        for (int difference = 0; difference <= dateDiff; difference++)
                        {
                            strNumber = objEmployeeDetails.CheckInAssociate(this.txtEmpID.Text.ToString(), strUserId, this.hdnFacility.Value, this.RdlReason.SelectedValue, datetFromDate1, dateFromDate, dateToDate, this.ddAccessdetail.SelectedValue);
                            this.errortbl.Visible = false;
                            this.SetVisibilityLevel("CHECKEDIN");
                            this.hdnPassDetailsID.Value = strNumber;
                            dateFromDate = dateFromDate.AddDays(1);
                        }

                        int externalRequestID = Convert.ToInt32(Convert.ToInt32(strNumber) - dateDiff);
                        objFacilityID.StoreTempAccessCardDetails(this.txtEmpID.Text.ToString(), externalRequestID.ToString(), strAccessCardNo, this.strLocation);
                        this.GenerateIDCard(externalRequestID.ToString());
                        if (this.ddAccessdetail.SelectedValue == "1 Day Access Card" || this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                        {
                            int accessCardNumber = Convert.ToInt32(strAccessCardNo);
                            string reason = this.RdlReason.SelectedValue;
                            string reasonDesc = string.Empty;
                            if (reason == "20")
                            {
                                reasonDesc = "Forgot ID";
                            }
                            else if (reason == "21")
                            {
                                reasonDesc = "Onsite return";
                            }
                            else if (reason == "22")
                            {
                                reasonDesc = "ID Lost";
                            }

                            DataSet gsmslocation = new DataSet();
                            gsmslocation = objEmployeeDetails.GetlocationfromGSMS(this.strLocation);
                            string locationgsms = gsmslocation.Tables[0].Rows[0]["facilityid"].ToString();
                            bool result = client.GenerateOneDayAccessRequest(strAssociateID, externalRequestID, accessCardNumber, Convert.ToInt32(locationgsms), reasonDesc, dateToDate);
                            objaccess.Accesscardservicestatus(strAssociateID.ToString(), externalRequestID.ToString(), result, null);
                        }

                        DataSet dtset = new DataSet();
                        dtset = objaccess.GetAccessCardDetails(strAssociateID, this.hdnFacility.Value.ToString(), externalRequestID.ToString());
                        this.accesstextid.Text = dtset.Tables[1].Rows[0]["CardType"].ToString();

                        if (ConfigurationManager.AppSettings["IVSmailer_enable"].ToString() == "true")
                        {
                            this.SendMail(strNumber, this.hdnManagerEmailID.Value, this.hdnManagerName.Value, this.hdnEmployeeName.Value, this.hdnAssociateID.Value, this.accesstextid.Text, strAccessCardNo);
                        }

                        this.dtselectCardddl = objaccess.GetSelect_CARD();
                        this.ddAccessdetail.DataSource = this.dtselectCardddl;
                        this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Temporary_Cards"].ToString();
                        this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                        this.ddAccessdetail.DataBind();
                    }
                    else
                    {
                        if ((this.RdlReason.SelectedValue == VMSConstants.ONSITERETURN) || (this.RdlReason.SelectedValue == VMSConstants.IDLOST))
                        {
                            for (int diff = 0; diff <= dateDiff; diff++)
                            {
                                if (objEmployeeDetails.DuplicateIVSEntry(this.txtEmpID.Text.ToString(), strUserId, this.hdnFacility.Value, this.RdlReason.SelectedValue, dateFromDate, dateToDate) != 1)
                                {
                                    ////int CardType = ddAccessdetail.SelectedValue;
                                    strNumber = objEmployeeDetails.CheckInAssociate(this.txtEmpID.Text.ToString(), strUserId, this.hdnFacility.Value, this.RdlReason.SelectedValue, datetFromDate1, dateFromDate, dateToDate, this.ddAccessdetail.SelectedValue);
                                    bool success = objFacilityID.StoreTempAccessCardDetails(this.txtEmpID.Text.ToString(), strNumber, strAccessCardNo, this.strLocation);
                                    dateFromDate = dateFromDate.AddDays(1);
                                    this.errortbl.Visible = false;
                                    this.SetVisibilityLevel("CHECKEDIN");
                                    this.hdnPassDetailsID.Value = strNumber;
                                }
                            }

                            if (objEmployeeDetails.DuplicateIVSEntry(this.txtEmpID.Text.ToString(), strUserId, this.hdnFacility.Value, this.RdlReason.SelectedValue, dateFromDate, dateToDate) != 1)
                            {
                                // GenerateIDCard(PassNumber);
                                if (this.ddAccessdetail.SelectedValue == "1 Day ID Card" || this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    this.GenerateIDCard(strNumber);
                                }

                                int externalRequestID = Convert.ToInt32(Convert.ToInt32(strNumber) - dateDiff);
                                if (this.ddAccessdetail.SelectedValue == "1 Day Access Card" || this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    int accessCardNumber = Convert.ToInt32(strAccessCardNo);
                                    string reason = this.RdlReason.SelectedValue;
                                    string reasonDesc = string.Empty;
                                    if (reason == "20")
                                    {
                                        reasonDesc = "Forgot ID";
                                    }
                                    else if (reason == "21")
                                    {
                                        reasonDesc = "Onsite return";
                                    }
                                    else if (reason == "22")
                                    {
                                        reasonDesc = "ID Lost";
                                    }

                                    DataSet gsmslocation = new DataSet();
                                    gsmslocation = objEmployeeDetails.GetlocationfromGSMS(this.strLocation);
                                    string locationgsms = gsmslocation.Tables[0].Rows[0]["facilityid"].ToString();
                                    bool result = client.GenerateOneDayAccessRequest(strAssociateID, externalRequestID, accessCardNumber, Convert.ToInt32(locationgsms), reasonDesc, dateToDate);
                                    objaccess.Accesscardservicestatus(strAssociateID.ToString(), externalRequestID.ToString(), result, null);
                                    if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                    {
                                        this.GenerateIDCard(strNumber);
                                    }
                                }

                                DataSet dtset = new DataSet();
                                dtset = objaccess.GetAccessCardDetails(strAssociateID, this.hdnFacility.Value.ToString(), externalRequestID.ToString());
                                this.accesstextid.Text = dtset.Tables[1].Rows[0]["CardType"].ToString();
                                if (ConfigurationManager.AppSettings["IVSmailer_enable"].ToString() == "true")
                                {
                                    this.SendMail(strNumber, this.hdnManagerEmailID.Value, this.hdnManagerName.Value, this.hdnEmployeeName.Value, this.hdnAssociateID.Value, this.accesstextid.Text, strAccessCardNo);
                                }
                            }
                        }
                        else
                        {
                            if (objEmployeeDetails.DuplicateIVSEntry(this.txtEmpID.Text.ToString(), strUserId, this.hdnFacility.Value, this.RdlReason.SelectedValue, dateFromDate, dateToDate) != 1)
                            {
                                strNumber = objEmployeeDetails.CheckInAssociate(this.txtEmpID.Text.ToString(), strUserId, this.hdnFacility.Value, this.RdlReason.SelectedValue, dateFromDate, dateFromDate, dateToDate, this.ddAccessdetail.SelectedValue);
                                this.errortbl.Visible = false;
                                this.SetVisibilityLevel("CHECKEDIN");
                                this.hdnPassDetailsID.Value = strNumber;
                                if (this.ddAccessdetail.SelectedValue == "1 Day ID Card" || this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                                    {
                                        objFacilityID.StoreTempAccessCardDetails(this.txtEmpID.Text.ToString(), strNumber, strAccessCardNo, this.strLocation);
                                    }

                                    this.GenerateIDCard(strNumber);
                                }

                                int externalRequestID = Convert.ToInt32(Convert.ToInt32(strNumber) - dateDiff);
                                if (this.ddAccessdetail.SelectedValue == "1 Day Access Card" || this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    int accessCardNumber = Convert.ToInt32(strAccessCardNo);
                                    string reason = this.RdlReason.SelectedValue;
                                    string reasonDesc = string.Empty;
                                    if (reason == "20")
                                    {
                                        reasonDesc = "Forgot ID";
                                    }
                                    else if (reason == "21")
                                    {
                                        reasonDesc = "Onsite return";
                                    }
                                    else if (reason == "22")
                                    {
                                        reasonDesc = "ID Lost";
                                    }

                                    DataSet gsmslocation = new DataSet();
                                    gsmslocation = objEmployeeDetails.GetlocationfromGSMS(this.strLocation);
                                    string locationgsms = gsmslocation.Tables[0].Rows[0]["facilityid"].ToString();
                                    bool result = client.GenerateOneDayAccessRequest(strAssociateID, externalRequestID, accessCardNumber, Convert.ToInt32(locationgsms), reasonDesc, dateToDate);
                                    if (result)
                                    {
                                        bool success = objFacilityID.StoreTempAccessCardDetails(this.txtEmpID.Text.ToString(), strNumber, strAccessCardNo, this.strLocation);
                                        objaccess.Accesscardservicestatus(strAssociateID.ToString(), externalRequestID.ToString(), result, null);
                                        if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                        {
                                            this.GenerateIDCard(strNumber);
                                        }
                                    }
                                }

                                DataSet dtset = new DataSet();
                                dtset = objaccess.GetAccessCardDetails(strAssociateID, this.hdnFacility.Value.ToString(), externalRequestID.ToString());
                                this.accesstextid.Text = dtset.Tables[1].Rows[0]["CardType"].ToString();
                                if (ConfigurationManager.AppSettings["IVSmailer_enable"].ToString() == "true")
                                {
                                    this.SendMail(strNumber, this.hdnManagerEmailID.Value, this.hdnManagerName.Value, this.hdnEmployeeName.Value, this.hdnAssociateID.Value, this.accesstextid.Text, strAccessCardNo);
                                }
                            }
                        }
                    }
                }

                this.SetVisibilityLevel("CHECKEDIN");
                this.BindData();
                string strMessage = string.Concat(Resources.LocalizedText.Notification);
                this.lblTempIdIssued.Text = strMessage;
                this.imgToDate.Enabled = false;
                this.txtToDate.Enabled = false;
                this.lblTempIdIssued.Visible = true;
                this.txtAccess.Text = string.Empty;

                client.Close();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to handle Check In Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnCheckIn_Click(object sender, EventArgs e)
        {
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;  //added for Image source integration
            this.lblTempIdIssued.Visible = false;
            string strAssociateID = this.txtEmpID.Text;
            ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetOffset", "GetOffset();", true);
            ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
            //  DateTime today = this.genTimeZone.GetCurrentDate();
            DateTime today = this.genTimeZone.GetLocalCurrentDateInFormat();


            string time = today.ToString("HH:mm");
            DateTime endDate;
            /*change by ram*/
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            string[] onedayAccesscard = this.panIND.Split(',');
            this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
            if (this.securitydetails != null)
            {
                if (this.securitydetails.Tables[0].Rows.Count > 0)
                {
                    this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                    this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                }
            }

            foreach (string strCountrychk in onedayAccesscard)
            {
                if (strCountrychk == this.strCountry)
                {
                    this.txtFromDate.Text = DateTime.Now.ToShortDateString();
                    string todate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                    string[] arrtoDate = todate.Split('/');
                    DateTime todate1 = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(arrtoDate[0] + "/" + arrtoDate[1] + "/" + arrtoDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));
                    if (todate1 > this.genTimeZone.GetCurrentDate().AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["Issuancetodate"])))
                    {
                        string strMessage = "Please select a date within " + ConfigurationManager.AppSettings["Issuancetodate"] + " day(s) from today";

                        this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, strMessage);
                        return;
                    }
                }
            }

            string dateFromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
            string[] fromDate1 = dateFromDate.Split('/');
            this.txtFromDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
            AccessCard_EApprovalClient client = new AccessCard_EApprovalClient();
            string count = objaccess.Get_Accesscardnumber(this.txtAccess.Text).ToString();
            string oneday = client.CheckOneDayAccessCard(this.txtAccess.Text).ToString();

            if (count == "True")
            {
                this.Access_Check(count);
                count = string.Empty;
            }
            else if (oneday == "1")
            {
                string access = "True";
                this.Access_Check(access);
                access = string.Empty;
            }
            else
            {
                /* to convert different time zone to Indian time zone format */
                //  DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));

                DateTime startDate = Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + time);
                if (string.IsNullOrEmpty(this.txtToDate.Text))
                {
                    string strMessage = string.Concat(Resources.LocalizedText.lblTodateMandatory);
                    this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Warning, strMessage);
                    return;
                }
                else
                {
                    string todate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                    string[] arrtoDate = todate.Split('/');
                    //  endDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(arrtoDate[0] + "/" + arrtoDate[1] + "/" + arrtoDate[2] + " " + time), Convert.ToString(this.Session["TimezoneOffset"]));

                    endDate = Convert.ToDateTime(arrtoDate[0] + "/" + arrtoDate[1] + "/" + arrtoDate[2] + " " + time);
                }

                int dayTo = endDate.Day;
                int dayFrom = startDate.Day;
                TimeSpan ts = endDate - startDate;
                int dateDiff = Convert.ToInt32(ts.TotalDays);
                if (this.RdlReason.SelectedValue == VMSConstants.ONSITERETURN || this.RdlReason.SelectedValue == VMSConstants.IDLOST)
                {
                    string todate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                    string[] arrtoDate = todate.Split('/');
                    DateTime todate1 = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(arrtoDate[0] + "/" + arrtoDate[1] + "/" + arrtoDate[2] + " " + time), Convert.ToString(Session["TimezoneOffset"]));

                    if (todate1 > this.genTimeZone.GetCurrentDate().AddDays(7))
                    {
                        ////added by ram (445894) for temp access card 
                        foreach (string strCountrychk in onedayAccesscard)
                        {
                            if (strCountrychk != this.strCountry)
                            {
                                string strMessage = string.Concat(Resources.LocalizedText.DateRange);
                                this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, strMessage);
                            }
                            else if (ts.Days == 0)
                            {
                                if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                                {
                                    this.RequiredFieldValidator1.Text = string.Empty;
                                    this.mpeConfirm.Show();
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                {
                                    this.CompareValidator2.ErrorMessage = string.Empty;
                                    this.mpeConfirm1.Show();
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    this.CompareValidator2.ErrorMessage = string.Empty;
                                    this.mpeConfirm2.Show();
                                }
                            }
                            else if (ts.Days == 0)
                            {
                                if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                                {
                                    this.RequiredFieldValidator1.Text = string.Empty;
                                    this.mpeConfirm.Show();
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                {
                                    this.CompareValidator2.ErrorMessage = string.Empty;
                                    this.mpeConfirm1.Show();
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    this.CompareValidator2.ErrorMessage = string.Empty;
                                    this.mpeConfirm2.Show();
                                }
                            }
                            else if (todate1 < this.genTimeZone.GetCurrentDate())
                            {
                                string strMessage = string.Concat(Resources.LocalizedText.DateValid);
                                this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, strMessage);
                            }
                            else
                            {
                                if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                                {
                                    this.RequiredFieldValidator1.Text = string.Empty;
                                    this.mpeConfirm.Show();
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                {
                                    this.CompareValidator2.ErrorMessage = string.Empty;
                                    this.mpeConfirm1.Show();
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    this.CompareValidator2.ErrorMessage = string.Empty;
                                    this.mpeConfirm2.Show();
                                }
                            }
                        }
                    }
                    else if (ts.Days == 0)
                    {
                        if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                        {
                            this.RequiredFieldValidator1.Text = string.Empty;
                            this.mpeConfirm.Show();
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                        {
                            this.CompareValidator2.ErrorMessage = string.Empty;
                            this.mpeConfirm1.Show();
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                        {
                            this.CompareValidator2.ErrorMessage = string.Empty;
                            this.mpeConfirm2.Show();
                        }
                    }
                    else if (ts.Days == 0)
                    {
                        if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                        {
                            this.RequiredFieldValidator1.Text = string.Empty;
                            this.mpeConfirm.Show();
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                        {
                            this.CompareValidator2.ErrorMessage = string.Empty;
                            this.mpeConfirm1.Show();
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                        {
                            this.CompareValidator2.ErrorMessage = string.Empty;
                            this.mpeConfirm2.Show();
                        }
                    }
                    else if (todate1 < this.genTimeZone.GetCurrentDate())
                    {
                        string strMessage = string.Concat(Resources.LocalizedText.DateValid);
                        this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, strMessage);
                    }
                    else
                    {
                        if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                        {
                            this.RequiredFieldValidator1.Text = string.Empty;
                            this.mpeConfirm.Show();
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                        {
                            this.CompareValidator2.ErrorMessage = string.Empty;
                            this.mpeConfirm1.Show();
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                        {
                            this.CompareValidator2.ErrorMessage = string.Empty;
                            this.mpeConfirm2.Show();
                        }
                    }
                }
                else if (this.RdlReason.SelectedValue == VMSConstants.IDFORGOT)
                {
                    if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                    {
                        this.RequiredFieldValidator1.Text = string.Empty;
                        this.mpeConfirm.Show();
                    }
                    else if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                    {
                        this.CompareValidator2.ErrorMessage = string.Empty;
                        this.mpeConfirm1.Show();
                    }
                    else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                    {
                        this.CompareValidator2.ErrorMessage = string.Empty;
                        this.mpeConfirm2.Show();
                    }
                }

                return;
            }
        }

        /// <summary>
        /// Method to handle Check Out Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;  //added for Image source integration

                ////check out button functionality change as per sandeep requirment
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                string[] onedayAccesscard = this.panIND.Split(',');
                this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                if (this.securitydetails != null)
                {
                    if (this.securitydetails.Tables[0].Rows.Count > 0)
                    {
                        this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                        this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                    }
                }

                foreach (string strCountrychk in onedayAccesscard)
                {
                    if (strCountrychk == this.strCountry)
                    {
                        string strAssociateID = this.txtEmpID.Text;
                        string strCardIssued = objEmployeeDetails.IsCardIssuedLocation(strAssociateID, Convert.ToInt16(this.hdnFacility.Value.ToString()));
                        if (!string.IsNullOrEmpty(strCardIssued))
                        {
                            if (Convert.ToInt32(strCardIssued.Split('|')[1]) == 1)
                            {
                                string checkoutbuttonmsg = string.Concat(Resources.LocalizedText.Checkoutbuttonmsg);
                                string strScript = string.Concat("<script>alert('", checkoutbuttonmsg, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                            }
                        }
                    }
                    else
                    {
                        AccessCard_EApprovalClient obj = new AccessCard_EApprovalClient();
                        string securityID = string.Empty;
                        if (this.Session["LoginID"] != null)
                        {
                            securityID = Session["LoginID"].ToString();
                            this.securitydetails = this.requestDetails.GetSecurityCity(securityID);
                            this.facility = this.securitydetails.Tables[0].Rows[0]["Facility"].ToString();
                            this.cityName = this.securitydetails.Tables[0].Rows[0]["City"].ToString();

                            if (!string.IsNullOrEmpty(this.hdnPassDetailsID.Value))
                            {
                                string bdgestatus = string.Empty;
                                EmployeeBL objlostcard = new EmployeeBL();
                                DataSet lostcarddetail = new DataSet();
                                lostcarddetail = objlostcard.GetlostStatus(Convert.ToInt32(this.hdnPassDetailsID.Value));
                                string lostdetail = lostcarddetail.Tables[0].Rows[0]["BadgeStatus_Description"].ToString();
                                string[] lostdetailarray = lostdetail.Split(' ');
                                var bdgestatusDesr = string.Empty;
                                ////var bdgestatus = "";
                                if (lostdetailarray[0] == "Issued")
                                {
                                    bdgestatusDesr = "Returned";
                                }
                                else if (lostdetailarray[0] == "LostID" || lostdetailarray[0] == "LostAccCard")
                                {
                                    bdgestatusDesr = lostdetailarray[0];
                                }

                                bdgestatus = "Returned";
                                //// var bdgestatusDesr = "Returned";
                                //  DateTime currenttime = Convert.ToDateTime(HttpContext.Current.Session["currentDateTime"]);
                                DateTime currenttime = this.genTimeZone.GetCurrentDate();
                                string curtime = this.genTimeZone.ToLocalTimeZone(currenttime, Convert.ToString(Session["TimezoneOffset"]));
                                DateTime curdatetime = Convert.ToDateTime(curtime);
                                // DateTime currenttime = System.DateTime.Now;
                                string strDetailParentID = objEmployeeDetails.CheckOutAssociate(this.hdnPassDetailsID.Value.ToString().Trim(), securityID, bdgestatus, bdgestatusDesr, this.strLocation, curdatetime);
                                if (!string.IsNullOrEmpty(strDetailParentID))
                                {
                                    DateTime dt = DateTime.Now; // ddlFacility.SelectedItem.Text 
                                    if (this.accesstextid.Text == "1 Day ID Card")
                                    {
                                        string idcardRet = string.Concat(Resources.LocalizedText.TempCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                        string strScript = string.Concat("<script>alert('", idcardRet, "'); </script>");
                                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                    }
                                    else if (this.accesstextid.Text == "1 Day Access Card")
                                    {
                                        string accessCardret = string.Concat(Resources.LocalizedText.AccessCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                        string strScript = string.Concat("<script>alert('", accessCardret, "'); </script>");
                                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                    }
                                    else if (this.accesstextid.Text == "1 day ID Card and Access Card")
                                    {
                                        if (lostdetailarray[0] == "Issued")
                                        {
                                            string bothret = string.Concat(Resources.LocalizedText.IDCardandAccessCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                            string strScript = string.Concat("<script>alert('", bothret, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                        else if (lostdetailarray[0] == "LostID")
                                        {
                                            string accessCardret = string.Concat(Resources.LocalizedText.AccessCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                            string strScript = string.Concat("<script>alert('", accessCardret, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                        else if (lostdetailarray[0] == "LostAccCard")
                                        {
                                            string idcardRet = string.Concat(Resources.LocalizedText.TempCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                            string strScript = string.Concat("<script>alert('", idcardRet, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }

                                    this.BindData();
                                    this.InitDates();
                                    this.txtToDate.Enabled = false;
                                    this.imgToDate.Enabled = false;
                                    this.SetVisibilityLevel("SEARCH");
                                    this.lblTempIdIssued.Visible = false;
                                    this.RdlReason.Enabled = false;
                                    ////changed for access card 06-02-2015
                                    this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                                    bool success = obj.DeactivateOneDayAccessCard(Convert.ToInt32(this.hdnPassDetailsID.Value));
                                    /////objaccess1.Accesscardservicestatus(strAssociateID.ToString(), PassDetailsID.ToString(), null, success);
                                    VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                                    //// DataSet select_card_ddl = new DataSet();
                                    this.dtselectCardddl = objaccess.GetSelect_CARD();
                                    this.ddAccessdetail.DataSource = this.dtselectCardddl;
                                    this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                    this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                    this.ddAccessdetail.DataBind();
                                    this.txtAccess.Visible = false;
                                    this.LblAccess.Visible = false;
                                    this.ddAccessdetail.Enabled = false;
                                }
                                else
                                {
                                    string errorMessage = Resources.LocalizedText.ErrorMessageCheckOut;
                                    string strScript = string.Concat("<script>alert('", errorMessage, "'); </script>");
                                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                }
                            }
                            else
                            {
                                string errorMessage = Resources.LocalizedText.ErrorMessageCheckOut;
                                string strScript = string.Concat("<script>alert('", errorMessage, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
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
        /// Method to handle RePrint Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnReprint_Click(object sender, EventArgs e)
        {
            try
            {
                this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;  //added for Image source integration

                /*reprint button function change by sandeep*/
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                string[] onedayAccesscard = this.panIND.Split(',');
                this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                if (this.securitydetails != null)
                {
                    if (this.securitydetails.Tables[0].Rows.Count > 0)
                    {
                        this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                        this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                    }
                }

                foreach (string strCountrychk in onedayAccesscard)
                {
                    if (strCountrychk == this.strCountry)
                    {
                        ////VMSBusinessLayer.EmployeeBL objEmployeeDetails = new VMSBusinessLayer.EmployeeBL();
                        string strAssociateID = this.txtEmpID.Text;
                        string strCardIssued = objEmployeeDetails.IsCardIssuedLocation(strAssociateID, Convert.ToInt16(this.hdnFacility.Value.ToString()));
                        if (!string.IsNullOrEmpty(strCardIssued))
                        {
                            if (Convert.ToInt32(strCardIssued.Split('|')[1]) == 1)
                            {
                                string reprintbuttonmsg = string.Concat(Resources.LocalizedText.Reprintbuttonmsg);
                                string strScript = string.Concat("<script>alert('", reprintbuttonmsg, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                            }
                        }
                    }
                    else
                    {
                        this.modalReprintComments.Show();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to handle Print Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;  //added for Image source integration

                EmployeeBL objEmployeeDetails = new EmployeeBL();
                int passDetailsID = 0;
                string securityID = string.Empty;
                if (this.Session["LoginID"] != null)
                {
                    securityID = Session["LoginID"].ToString();

                    if (!string.IsNullOrEmpty(this.hdnPassDetailsID.Value))
                    {
                        passDetailsID = Convert.ToInt32(this.hdnPassDetailsID.Value.ToString().Trim());
                    }

                    string status = this.ddlReason.SelectedItem.Value == "1" ? VMSConstants.LOST :
                        VMSConstants.PRINTERJAMMED;

                    objEmployeeDetails.SaveReprintDetails(passDetailsID, securityID, status);

                    this.GenerateIDCard(XSS.HtmlEncode(this.hdnPassDetailsID.Value.ToString().Trim()));
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

        /// <summary>
        /// Method to handle Reminder Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnConfirmReminder_Click(object sender, EventArgs e)
        {
            try
            {
                this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                MailNotification objMailNotification = new MailNotification();
                MailSMSNotification objMailSMSNotification = new MailSMSNotification();
                string strFromDate = this.txtFromDate.Text.ToString();
                ////string strToDate = txtToDate.Text.ToString();
                //// string strToDate = DateTime.Now.ToString("dd-MMM-yyyy");
                string strAssociateID = Convert.ToString(Session["strAssociateID"]);
                string strAssociateName = Convert.ToString(Session["strAssociateName"]);
                DateTime dateStartDate = Convert.ToDateTime(Session["strCardGivenDate"], provider);
                string strIssuedDate = dateStartDate.ToString("dd/MMM/yyyy", provider);

                DateTime dateToDate = Convert.ToDateTime(Session["strIssuedDate"], provider);

                string strToDate = dateToDate.ToString("dd/MMM/yyyy", provider);

                string strIssuedTime = Convert.ToString(Session["strIssuedTime"]);
                string strAssociateMailID = Convert.ToString(Session["strAssociateMailID"]);
                string strSecurityID = Convert.ToString(Session["LoginID"]);
                ////this.cityName = Convert.ToString(this.Session["City"]);
                ////this.facility = Convert.ToString(this.Session["Facility"]);
                this.cityName = this.hdnIssuedCity.Value;
                this.facility = this.hdnIssuedLocation.Value;
                string strDetailId = this.hdnPassDetailsID.Value;
                this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                if (this.securitydetails != null)
                {
                    if (this.securitydetails.Tables[0].Rows.Count > 0)
                    {
                        this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                        this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                    }
                }

                string remindermail2 = Convert.ToString(Session["lbtnSendReminder"]);
                if (strAssociateMailID != string.Empty && strAssociateMailID != null)
                {
                    objMailSMSNotification.SendReminderMail(this.facility, this.cityName, strAssociateName, strIssuedDate, strIssuedTime, strAssociateMailID, strAssociateID, strSecurityID, strFromDate, strToDate, Session["SecurityCountry"].ToString().Trim(), strDetailId, remindermail2, this.strLocation);
                }

                this.requestDetails.SendReminderLog(strIssuedDate, strIssuedTime, strAssociateID, strSecurityID, strDetailId);
                string strMessage = string.Concat(Resources.LocalizedText.ReminderNotification, " ", strAssociateID, " ", Resources.LocalizedText.Under, " ", this.facility, " ", Resources.LocalizedText.Location);
                this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, strMessage);
                this.BindData();
                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to view item data bound results 
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void LstvwResults_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

            DateTime currentTime = this.genTimeZone.GetCurrentDate();
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;
            string time = currentTime.ToString("HH:mm");
            int extendedDuration = Convert.ToInt32(ConfigurationManager.AppSettings["ReminderMail_enableTimePeriod"]);
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HiddenField hdntoDate = (HiddenField)e.Item.FindControl("hdnGridToDate");
                Label hdnReminderCount = (Label)e.Item.FindControl("hdnReminderCount");

                //Label returnd = (Label)e.Item.FindControl("ReturnedDate");
                //if (!string.IsNullOrEmpty(returnd.Text))
                //{
                //    DateTime rdate = Convert.ToDateTime(returnd.Text);
                //    string localdate = this.genTimeZone.ToLocalTimeZone(rdate, Convert.ToString(Session["TimezoneOffset"]));

                //    returnd.Text = localdate;
                //}
                LinkButton btnSendReminder = (LinkButton)e.Item.FindControl("btnSendReminder");
                LinkButton btnenablecard = (LinkButton)e.Item.FindControl("btnenablecard");

                if (hdnReminderCount != null)
                {
                    if (!string.IsNullOrEmpty(hdnReminderCount.Text))
                    {
                        if (Convert.ToInt32(hdnReminderCount.Text) == 1)
                        {
                            btnSendReminder.Text = "Reminder Mail 2";
                        }
                        else if (Convert.ToInt32(hdnReminderCount.Text) > 1)
                        {
                            btnSendReminder.Text = "Reminder Mail 2";
                            btnSendReminder.Enabled = false;
                        }
                    }
                }

                DateTime issuedTime = Convert.ToDateTime(hdntoDate.Value);
                DateTime enabledTime = issuedTime.AddHours(extendedDuration);
                Label lblBadgeStatus = (Label)e.Item.FindControl("lblBadgeStatus");
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

                if (DateTime.Compare(currentTime, enabledTime) <= 0)
                {
                    btnSendReminder.CssClass = "GridLinkButtonDisabled";
                    btnSendReminder.Enabled = false;
                }

                this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                if (this.securitydetails != null)
                {
                    if (this.securitydetails.Tables[0].Rows.Count > 0)
                    {
                        this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                        this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Method to handle id card lost confirm Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnidcardLostConfirm_Click(object sender, EventArgs e)
        {
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            VMSBusinessLayer.RequestDetailsBL objrequestDetails = new VMSBusinessLayer.RequestDetailsBL();
            AccessCard_EApprovalClient obj = new AccessCard_EApprovalClient();
            string securityID = string.Empty;
            securityID = Session["LoginID"].ToString();
            this.securitydetails = objrequestDetails.GetSecurityCity(securityID);
            this.facility = this.securitydetails.Tables[0].Rows[0]["Facility"].ToString();
            this.cityName = this.securitydetails.Tables[0].Rows[0]["City"].ToString();
            string location = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
            var country = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["CountryName"]);
            string associateName = Session["associateNae"].ToString();
            string associateID = Session["strAssociateID1"].ToString();
            this.Session["City"] = this.cityName;
            this.Session["Facility"] = this.facility;

            VMSBusinessLayer.RequestDetailsBL objaccess1 = new VMSBusinessLayer.RequestDetailsBL();
            DataSet dtset = new DataSet();
            string bdgestatus = "Lost";
            string comments = string.Empty;
            string strScript = string.Empty;
            this.locationId = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["LocationId"]);
            string strDetailId = Session["PassDetailIDSession"].ToString();
            string bdgestatusDesr = string.Empty;
            if (this.accesstextid.Text == "1 Day ID Card")
            {
                bdgestatusDesr = "LOSTID";
            }
            else if (this.accesstextid.Text == "1 Day Access Card")
            {
                bdgestatusDesr = "LostAccCard";
            }
            // DateTime currentdate = Convert.ToDateTime(HttpContext.Current.Session["currentDateTime"]);
            DateTime currentdate = this.genTimeZone.GetCurrentDate();
            string strDetailParentID = objEmployeeDetails.CheckOutAssociate(strDetailId, securityID, bdgestatus, bdgestatusDesr, this.locationId, currentdate);
            if (!string.IsNullOrEmpty(strDetailParentID))
            {
                this.btnCheckOut.Enabled = false;
                this.btnCheckIn.Enabled = true;
                this.btnReprint.Enabled = false;
                this.BindData();
                if (this.accesstextid.Text == "1 Day Access Card")
                {
                    bool success = obj.DeactivateOneDayAccessCard(Convert.ToInt32(strDetailId));
                    objaccess1.Accesscardservicestatus(associateID.ToString(), strDetailId.ToString(), null, success);
                }

                this.PopulateAssociateData(associateID, objEmployeeDetails);
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                string format = "dd/MM/yyyy HH:mm:ss";
                if (this.Session["currentDateTime"] != null)
                {
                    string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                    var today = DateTime.ParseExact(currenttime, format, provider);
                    this.txtFromDate.Text = today.ToString("dd/MMM/yyyy", provider);
                    this.txtToDate.Text = today.ToString("dd/MMM/yyyy", provider);
                }

                this.lblTempIdIssued.Visible = false;
                this.RdlReason.Enabled = true;
                this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetOffset", "GetOffset();", true);
                VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                this.dtselectCardddl = objaccess.GetSelect_CARD();
                this.ddAccessdetail.DataSource = this.dtselectCardddl;
                string[] onedayAccesscard = this.panIND.Split(',');
                foreach (string strCountrychk in onedayAccesscard)
                {
                    if (strCountrychk == this.strCountry)
                    {
                        this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Temporary_Cards"].ToString();
                        this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                        this.ddAccessdetail.DataBind();
                    }
                    else
                    {
                        this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                        this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                        this.ddAccessdetail.DataBind();
                    }
                }

                this.txtAccess.Visible = false;
                this.LblAccess.Visible = false;
                this.txtAccess.Text = string.Empty;
                this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                if (this.accesstextid.Text == "1 Day ID Card")
                {
                    string idcardRet = string.Concat("Associate (" + XSS.HtmlEncode(this.txtEmpID.Text) + ") has lost the Temporary ID card.Check out successful");
                    strScript = string.Concat("<script>alert('", idcardRet, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (this.accesstextid.Text == "1 Day Access Card")
                {
                    string accessCardret = string.Concat("Associate (" + XSS.HtmlEncode(this.txtEmpID.Text) + ") has lost the Temporary Access card.Check out successful");
                    strScript = string.Concat("<script>alert('", accessCardret, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    this.LostMailReportToPOC(associateID, associateName, this.facility, this.cityName, country, this.locationId, Session["accessCardNmbr"].ToString(), strDetailId);
                }

                return;
            }
        }

        /// <summary>
        /// Method to handle enable card popup Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void Btnenablecardpopup(object sender, EventArgs e)
        {
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;
            int enablecardID;
            enablecardID = Convert.ToInt32(this.Session["enablecardpassdetailID"]);
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            VMSBusinessLayer.RequestDetailsBL objrequestDetails = new VMSBusinessLayer.RequestDetailsBL();
            AccessCard_EApprovalClient obj = new AccessCard_EApprovalClient();

            bool returncheck = obj.EnablePermanentCard(enablecardID);

            if (returncheck == true)
            {
                objrequestDetails.EnableaccesscardBL(enablecardID);
                BusinessManager.MailNotification mailNotification = new BusinessManager.MailNotification();
                mailNotification.EnableCardNotification(Session["strAssociateID"].ToString(), Session["AccessCardNo"].ToString(), Session["associateNae"].ToString(), Session["enablecardpassdetailID"].ToString());
                string enablecardsuccess = "The temporary card will be deacticvated and the permanent card will be activated.";
                string strScriptenable = string.Concat("<script>alert('", enablecardsuccess, "'); </script>");
                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScriptenable);
            }
            else
            {
                string enablecarderror = "The process did not success. Kinldy redo";
                string strScriptenable = string.Concat("<script>alert('", enablecarderror, "'); </script>");
                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScriptenable);
            }
        }

        /// <summary>
        /// Method to handle lost confirm Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnLostConfirm_Click(object sender, EventArgs e)
        {
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;
            {
                string bdgestatusDesr = string.Empty;
                var lost_Details = string.Empty;
                if (this.rbtLstRating.SelectedItem != null)
                {
                    lost_Details = this.rbtLstRating.SelectedItem.Text;
                }
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                VMSBusinessLayer.RequestDetailsBL objrequestDetails = new VMSBusinessLayer.RequestDetailsBL();
                AccessCard_EApprovalClient obj = new AccessCard_EApprovalClient();
                string securityID = string.Empty;
                securityID = Session["LoginID"].ToString();
                this.securitydetails = objrequestDetails.GetSecurityCity(securityID);
                this.facility = this.securitydetails.Tables[0].Rows[0]["Facility"].ToString();
                this.cityName = this.securitydetails.Tables[0].Rows[0]["City"].ToString();
                string location = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                var country = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["CountryName"]);
                string associateName = Session["associateNae"].ToString();
                string associateID = Session["strAssociateID1"].ToString();
                this.Session["City"] = this.cityName;
                this.Session["Facility"] = this.facility;
                string info1 = Session["str_lost"].ToString();
                string[] arg1 = new string[2];
                char[] splitter1 = { ';' };
                arg1 = info1.Split(splitter1);

                VMSBusinessLayer.RequestDetailsBL objaccess1 = new VMSBusinessLayer.RequestDetailsBL();
                DataSet dtset = new DataSet();
                var flag = 0;
                string strDetailsID1 = Session["PassDetailIDSession"].ToString();
                string bdgestatus = string.Empty;
                EmployeeBL objlostcard = new EmployeeBL();
                DataSet lostcarddetail = new DataSet();
                lostcarddetail = objlostcard.GetlostStatus(Convert.ToInt32(strDetailsID1));
                string lostdetail = lostcarddetail.Tables[0].Rows[0]["BadgeStatus_Description"].ToString();
                string[] lostdetailarray = lostdetail.Split(' ');
                if (lost_Details == "OnlyIDCard")
                {
                    flag = 1;
                    if (lostdetailarray[0] == "Issued")
                    {
                        bdgestatusDesr = "LostID";
                        bdgestatus = "Issued";
                    }
                    else if (lostdetailarray[0] == "LostAccCard")
                    {
                        bdgestatusDesr = "LostBoth";
                        bdgestatus = "Lost";
                    }
                }
                else if (lost_Details == "OnlyAccessCard")
                {
                    flag = 2;
                    if (lostdetailarray[0] == "Issued")
                    {
                        bdgestatusDesr = "LostAccCard";
                        bdgestatus = "Issued";
                    }
                    else if (lostdetailarray[0] == "LostID")
                    {
                        bdgestatusDesr = "LostBoth";
                        bdgestatus = "Lost";
                    }

                    this.LostMailReportToPOC(associateID, associateName, this.facility, this.cityName, country, location, Session["accessCardNmbr"].ToString(), strDetailsID1);
                }
                else if (lost_Details == "BothID & AccessCard")
                {
                    flag = 2;
                    bdgestatusDesr = "LostBoth";
                    bdgestatus = "Lost";
                    this.LostMailReportToPOC(associateID, associateName, this.facility, this.cityName, country, location, Session["accessCardNmbr"].ToString(), strDetailsID1);
                }

                if ((flag == 1) || (flag == 2))
                {
                    string str = Session["str_lost"].ToString();
                    string comments = string.Empty;
                    string strScript = string.Empty;
                    string info = Session["info_lost"].ToString();
                    string[] arg = new string[2];
                    char[] splitter = { ';' };
                    arg = info.Split(splitter);
                    string strDetailId = arg[0];
                    string strAssociateID = arg[1];
                    //DateTime currentdate = Convert.ToDateTime(HttpContext.Current.Session["currentDateTime"]);
                    DateTime currentdate = this.genTimeZone.GetCurrentDate();
                    string strDetailParentID = objEmployeeDetails.CheckOutAssociate(strDetailId, securityID, bdgestatus, bdgestatusDesr, location, currentdate);

                    /*code added by ram*/

                    this.securitydetails = objrequestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    if (this.securitydetails != null)
                    {
                        if (this.securitydetails.Tables[0].Rows.Count > 0)
                        {
                            this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                            this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                        }
                    }

                    string[] onedayAccesscard = this.panIND.Split(',');
                    foreach (string strCountrychk in onedayAccesscard)
                    {
                        if (strCountrychk == this.strCountry)
                        {
                            if (!string.IsNullOrEmpty(strDetailParentID))
                            {
                                this.btnCheckOut.Enabled = true;
                                this.btnCheckIn.Enabled = true;
                                this.btnReprint.Enabled = true;
                                this.BindData();
                                if ((lost_Details == "BothID & AccessCard") || (lost_Details == "OnlyAccessCard"))
                                {
                                    bool success = obj.DeactivateOneDayAccessCard(Convert.ToInt32(strDetailId));
                                    objaccess1.Accesscardservicestatus(strAssociateID.ToString(), strDetailId.ToString(), null, success);
                                }

                                this.PopulateAssociateData(strAssociateID, objEmployeeDetails);
                                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                                string format = "dd/MM/yyyy HH:mm:ss";
                                if (this.Session["currentDateTime"] != null)
                                {
                                    string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                                    var today = DateTime.ParseExact(currenttime, format, provider);
                                    this.txtFromDate.Text = today.ToString("dd/MMM/yyyy", provider);
                                    this.txtToDate.Text = today.ToString("dd/MMM/yyyy", provider);
                                }

                                this.lblTempIdIssued.Visible = false;
                                this.RdlReason.Enabled = true;
                                this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                                if (lost_Details == "OnlyIDCard")
                                {
                                    this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, "One day ID card lost for associate " + strAssociateID + ". Check Out Successful");
                                }
                                else if (lost_Details == "OnlyAccessCard")
                                {
                                    this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, "Access Card lost for associate " + strAssociateID + ". Check Out Successful");
                                }
                                else if (lost_Details == "BothID & AccessCard")
                                {
                                    this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, "Both ID & Access Card lost for associate " + strAssociateID + ". Check Out Successful");
                                }

                                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetOffset", "GetOffset();", true);
                                VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                                this.dtselectCardddl = objaccess.GetSelect_CARD();
                                this.ddAccessdetail.DataSource = this.dtselectCardddl;
                                this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Temporary_Cards"].ToString();
                                this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                this.ddAccessdetail.DataBind();
                                this.txtAccess.Visible = false;
                                this.LblAccess.Visible = false;
                                this.ddAccessdetail.Enabled = true;
                                this.RdlReason.Enabled = true;
                                this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                                return;
                            }
                            else
                            {
                                string errorMessage = Resources.LocalizedText.ErrorMessageCheckOut;
                                this.MsgBox.Show(UserControls.MsgBox.MsgBoxType.Error, errorMessage);
                                return;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(strDetailParentID))
                            {
                                this.btnCheckOut.Enabled = false;
                                this.btnCheckIn.Enabled = true;
                                this.btnReprint.Enabled = false;
                                this.BindData();
                                if ((lost_Details == "BothID & AccessCard") || (lost_Details == "OnlyAccessCard"))
                                {
                                    bool success = obj.DeactivateOneDayAccessCard(Convert.ToInt32(strDetailId));
                                    objaccess1.Accesscardservicestatus(strAssociateID.ToString(), strDetailId.ToString(), null, success);
                                }

                                this.PopulateAssociateData(strAssociateID, objEmployeeDetails);
                                this.InitDates();
                                this.lblTempIdIssued.Visible = false;
                                this.RdlReason.Enabled = true;
                                this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                                if (lost_Details == "OnlyIDCard")
                                {
                                    this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, "One day ID card lost for associate " + strAssociateID + ". Check Out Successful");
                                }
                                else if (lost_Details == "OnlyAccessCard")
                                {
                                    this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, "Access Card lost for associate " + strAssociateID + ". Check Out Successful");
                                }
                                else if (lost_Details == "BothID & AccessCard")
                                {
                                    this.MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, "Both ID & Access Card lost for associate " + strAssociateID + ". Check Out Successful");
                                }

                                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetOffset", "GetOffset();", true);
                                VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                                this.dtselectCardddl = objaccess.GetSelect_CARD();
                                this.ddAccessdetail.DataSource = this.dtselectCardddl;
                                this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                this.ddAccessdetail.DataBind();
                                this.txtAccess.Visible = false;
                                this.LblAccess.Visible = false;
                                this.ddAccessdetail.Enabled = false;
                                this.RdlReason.Enabled = false;
                                this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                                return;
                            }
                            else
                            {
                                string errorMessage = Resources.LocalizedText.ErrorMessageCheckOut;
                                this.MsgBox.Show(UserControls.MsgBox.MsgBoxType.Error, errorMessage);
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to list item command results
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void LstvwResults_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            VMSBusinessLayer.RequestDetailsBL objrequestDetails = new VMSBusinessLayer.RequestDetailsBL();
            AccessCard_EApprovalClient obj = new AccessCard_EApprovalClient();
            string securityID = string.Empty;
            securityID = Session["LoginID"].ToString();
            this.securitydetails = this.requestDetails.GetSecurityCity(securityID);
            this.facility = this.securitydetails.Tables[0].Rows[0]["Facility"].ToString();
            this.cityName = this.securitydetails.Tables[0].Rows[0]["City"].ToString();
            this.locationId = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
            string associateName = ((Label)e.Item.FindControl("lblAssociateName")).Text;
            this.Session["associateNae"] = associateName;
            this.Session["City"] = this.cityName;
            this.Session["Facility"] = this.facility;
            LinkButton lbtnCheckOut = (LinkButton)e.Item.FindControl("btnlstCheckOut");
            LinkButton lbtnLost = (LinkButton)e.Item.FindControl("btnLost");
            LinkButton lbtnPrint = (LinkButton)e.Item.FindControl("btnPrint");
            LinkButton lbtnSendReminder = (LinkButton)e.Item.FindControl("btnSendReminder");
            string rmndrmailtxt = lbtnSendReminder.Text;
            this.Session["lbtnSendReminder"] = rmndrmailtxt;
            VMSDev.UserControls.MsgBox messageBox1 = (VMSDev.UserControls.MsgBox)e.Item.FindControl("MsgBox1");
            string[] onedayAccesscard = this.panIND.Split(',');

            if (e.CommandName == "CheckOut")
            {
                string str = e.CommandArgument.ToString();
                string bdgestatus = "Returned";
                string bdgestatusDesr = string.Empty;
                string comments = string.Empty;
                string info = e.CommandArgument.ToString();
                string[] arg = new string[5];
                char[] splitter = { ';' };
                arg = info.Split(splitter);
                string strDetailId = arg[0];
                string strAssociateID = arg[1];
                string issuedLocationID = arg[2];
                string issuedFacility = arg[3];
                string issuedCity = arg[4];
                EmployeeBL objlostcard = new EmployeeBL();
                DataSet lostcarddetail = new DataSet();
                if (objEmployeeDetails != null)
                {
                    DataRow associateRow = objEmployeeDetails.GetEmployeeDetails(strAssociateID);
                    if (associateRow != null)
                    {
                        ////hdnFileUploadID.Value = Convert.ToString(AssociateRow["FileUploadID"]);
                        lostcarddetail = objlostcard.GetlostStatus(Convert.ToInt32(strDetailId));
                        string lostdetail = lostcarddetail.Tables[0].Rows[0]["BadgeStatus_Description"].ToString();
                        string[] lostdetailarray = lostdetail.Split(' ');
                        if (lostdetailarray[0] == "Issued")
                        {
                            bdgestatusDesr = "Returned";
                        }
                        else if (lostdetailarray[0] == "LostID" || lostdetailarray[0] == "LostAccCard")
                        {
                            bdgestatusDesr = lostdetailarray[0];
                        }
                        //DateTime currentdate = Convert.ToDateTime(HttpContext.Current.Session["currentDateTime"]);
                        string format;
                        format = "dd/MM/yyyy HH:mm:ss";
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                        string Curdate = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                        var today = DateTime.ParseExact(Curdate, format, provider);

                        DateTime currenttime = this.genTimeZone.GetCurrentDate();
                        string curtime = this.genTimeZone.ToLocalTimeZone(currenttime, Convert.ToString(Session["TimezoneOffset"]));
                        DateTime curdatetime = Convert.ToDateTime(curtime);




                        string strDetailParentID = objEmployeeDetails.CheckOutAssociate(strDetailId, securityID, bdgestatus, bdgestatusDesr, this.locationId, curdatetime);
                        if (!string.IsNullOrEmpty(strDetailParentID))
                        {
                            lbtnCheckOut.Enabled = false;
                            lbtnLost.Enabled = false;
                            lbtnPrint.Enabled = false;
                            lbtnSendReminder.Enabled = false;
                            this.btnCheckOut.Enabled = false;
                            this.btnCheckIn.Enabled = true;
                            this.btnReprint.Enabled = false;
                            lbtnCheckOut.CssClass = "GridLinkButtonDisabled";
                            lbtnLost.CssClass = "GridLinkButtonDisabled";
                            lbtnPrint.CssClass = "GridLinkButtonDisabled";
                            lbtnSendReminder.CssClass = "GridLinkButtonDisabled";
                            VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                            DataSet dtset = new DataSet();
                            dtset = objaccess.GetAccessCardDetails(strAssociateID, issuedLocationID, strDetailId);
                            this.accesstextid.Text = dtset.Tables[1].Rows[0]["CardType"].ToString();
                            if (this.accesstextid.Text == "1 Day ID Card")
                            {
                                string idcardRet = string.Concat(Resources.LocalizedText.TempCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                string strScript = string.Concat("<script>alert('", idcardRet, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                            }
                            else if (this.accesstextid.Text == "1 Day Access Card")
                            {
                                string accessCardret = string.Concat(Resources.LocalizedText.AccessCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                string strScript = string.Concat("<script>alert('", accessCardret, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                            }
                            else if (this.accesstextid.Text == "1 day ID Card and Access Card")
                            {
                                if (lostdetailarray[0] == "Issued")
                                {
                                    string bothret = string.Concat(Resources.LocalizedText.IDCardandAccessCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                    string strScript = string.Concat("<script>alert('", bothret, "'); </script>");
                                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                }
                                else if (lostdetailarray[0] == "LostID")
                                {
                                    string accessCardret = string.Concat(Resources.LocalizedText.AccessCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                    string strScript = string.Concat("<script>alert('", accessCardret, "'); </script>");
                                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                }
                                else if (lostdetailarray[0] == "LostAccCard")
                                {
                                    string idcardRet = string.Concat(Resources.LocalizedText.TempCardReturned, " ", this.txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                                    string strScript = string.Concat("<script>alert('", idcardRet, "'); </script>");
                                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                }
                            }

                            bool success = obj.DeactivateOneDayAccessCard(Convert.ToInt32(strDetailId));
                            objaccess.Accesscardservicestatus(strAssociateID.ToString(), strDetailId.ToString(), null, success);
                            this.SendCheckoutNotificationMail(strAssociateID, associateName, issuedFacility.Trim() + ", " + issuedCity.Trim(), this.facility.Trim() + ", " + this.cityName.Trim(), this.accesstextid.Text);

                            this.BindData();
                            this.PopulateAssociateData(strAssociateID, objEmployeeDetails);
                            this.InitDates();
                            this.lblTempIdIssued.Visible = false;
                            this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                            this.txtToDate.Enabled = false;
                            this.dtselectCardddl = objaccess.GetSelect_CARD();
                            this.ddAccessdetail.DataSource = this.dtselectCardddl;
                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (strCountrychk == this.strCountry)
                                {
                                    this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Temporary_Cards"].ToString();
                                    this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                                    this.ddAccessdetail.DataBind();
                                    this.btnCheckIn.Enabled = false;
                                }
                                else
                                {
                                    this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                                    this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                                    this.ddAccessdetail.DataBind();
                                }
                            }

                            this.txtAccess.Visible = false;
                            this.LblAccess.Visible = false;
                            this.txtAccess.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetOffset", "GetOffset();", true);
                            return;
                        }
                        else
                        {
                            string errorMessage = Resources.LocalizedText.ErrorMessageCheckOut;
                            this.MsgBox.Show(UserControls.MsgBox.MsgBoxType.Error, errorMessage);
                            return;
                        }
                    }
                    else
                    {
                        string inactiveMsg = string.Concat(Resources.LocalizedText.InactiveMsg);
                        string strScript = string.Concat("<script>alert('", inactiveMsg, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    }
                }
            }
            else if (e.CommandName == "Lost")
            {
                this.rbtLstRating.ClearSelection();
                string info1 = e.CommandArgument.ToString();
                string[] arg1 = new string[3];
                char[] splitter1 = { ';' };
                arg1 = info1.Split(splitter1);
                string strAssociateID1 = arg1[1];
                string strDetailId = arg1[0];
                string issuedLocationID = arg1[3];
                this.Session["PassDetailIDSession"] = strDetailId;
                this.Session["strAssociateID1"] = strAssociateID1;
                this.txtEmpID.Text = strAssociateID1;

                ////By Krishna (449138)
                string straccessCardNmbr = arg1[2];
                this.Session["accessCardNmbr"] = straccessCardNmbr;

                VMSBusinessLayer.RequestDetailsBL objaccess1 = new VMSBusinessLayer.RequestDetailsBL();
                EmployeeBL objpocid = new EmployeeBL();
                DataSet dtset = new DataSet();
                DataSet pocList = new DataSet();
                dtset = objaccess1.GetAccessCardDetails(strAssociateID1, issuedLocationID, strDetailId);
                this.accesstextid.Text = dtset.Tables[1].Rows[0]["CardType"].ToString();
                string bdgestatusDesr = string.Empty;
#pragma warning disable CS0219 // The variable 'flag' is assigned but its value is never used
                var flag = 0;
#pragma warning restore CS0219 // The variable 'flag' is assigned but its value is never used
                pocList = objpocid.GetLostMailerACCPOCID(this.locationId);

                if (pocList.Tables[0].Rows.Count > 0)
                {
                    if (this.accesstextid.Text == "1 Day ID Card")
                    {
                        flag = 1;
                        bdgestatusDesr = "LostID";
                        this.mpeidcard.Show();
                    }
                    else if (this.accesstextid.Text == "1 Day Access Card")
                    {
                        flag = 2;
                        bdgestatusDesr = "LostAccCard";
                        this.securitydetails = this.requestDetails.GetSecurityCity(Convert.ToString(this.Session["LoginID"]));
                        this.cityName = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["City"]);
                        this.facility = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["Facility"]);
                        var country = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["CountryName"]);
                        string associateName1 = Convert.ToString(Session["associateNae"]);
                        string associateID = strAssociateID1;
                        this.mpeidcard.Show();
                        //// this.LostMailReportToPOC(AssociateID, AssociateName, Facility, City, Country);
                        ////strAssociateID1
                    }
                    else if (this.accesstextid.Text == "1 day ID Card and Access Card")
                    {
                        EmployeeBL objlostcard = new EmployeeBL();
                        DataSet lostcarddetail = new DataSet();
                        lostcarddetail = objlostcard.GetlostStatus(Convert.ToInt32(strDetailId));
                        string lostdetail = lostcarddetail.Tables[0].Rows[0]["BadgeStatus_Description"].ToString();
                        string[] lostdetailarray = lostdetail.Split(' ');
                        if (lostdetailarray[0] == "LostID")
                        {
                            this.rbtLstRating.Items[0].Enabled = false;
                            this.rbtLstRating.Items[2].Enabled = false;
                        }
                        else if (lostdetailarray[0] == "LostAccCard")
                        {
                            this.rbtLstRating.Items[1].Enabled = false;
                            this.rbtLstRating.Items[2].Enabled = false;
                        }
                        else if (lostdetailarray[0] == "Issued")
                        {
                            this.rbtLstRating.Items[0].Enabled = true;
                            this.rbtLstRating.Items[1].Enabled = true;
                            this.rbtLstRating.Items[2].Enabled = true;
                        }

                        this.mpelostcheck.Show();
                    }

                    string info_lost = e.CommandArgument.ToString();
                    this.Session["info_lost"] = info_lost;
                    string str_lost = e.CommandArgument.ToString();
                    this.Session["str_lost"] = str_lost;
                }
                else
                {
                    string accesscardcustodianmsg = Resources.LocalizedText.accesscardcustodianmsg;
                    string strScript = string.Concat("<script>alert('", accesscardcustodianmsg, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
            }
            else if (e.CommandName == "ViewDetailsLink")
            {
                string info = e.CommandArgument.ToString();
                string[] arg = new string[3];
                char[] splitter = { ';' };
                arg = info.Split(splitter);
                string strAssociateID = arg[0];
                string strBadgeStatus = arg[1];
                string issuedLocationID = arg[4];
                string strDetailId = string.Empty;
                EmployeeBL objEmployeDetails = new EmployeeBL();
                string currentdatefetch = string.Empty;
                this.txtToDate.Enabled = true;
                this.txtEmpID.Text = strAssociateID;
                this.RdlReason.SelectedValue = VMSConstants.IDFORGOT;
                this.PopulateAssociateData(strAssociateID, objEmployeeDetails);

                this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                if (this.securitydetails != null)
                {
                    if (this.securitydetails.Tables[0].Rows.Count > 0)
                    {
                        this.strLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                        this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                    }
                }
                //   DateTime Curdate = Convert.ToDateTime(HttpContext.Current.Session["currentDateTime"]);
                string format;
                format = "dd/MM/yyyy HH:mm:ss";
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                string Curdate = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                var today1 = DateTime.ParseExact(Curdate, format, provider);
                DataSet griddata = objEmployeDetails.GetODICardsIssued(this.strLocation, strAssociateID, today1);

                int rowCount = griddata.Tables[0].Rows.Count;
                int flag = 0;
                for (int i = 0; i < rowCount; i++)
                {
                    if ((griddata.Tables[0].Rows[i]["BadgeStatus"].ToString() == "Issued") &&
                        (griddata.Tables[0].Rows[i]["AssociateID"].ToString() == strAssociateID))
                    {
                        strDetailId = griddata.Tables[0].Rows[i]["PassDetailID"].ToString();
                        currentdatefetch = griddata.Tables[0].Rows[i]["IssuedDate"].ToString();
                        flag = 1;
                        break;
                    }
                    else
                    {
                        flag = 0;
                    }
                }

                if (griddata.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                }

                if (flag == 1)
                {
                    string strMessage = string.Concat(Resources.LocalizedText.Notification);
                    this.lblTempIdIssued.Text = strMessage;
                    this.lblTempIdIssued.Visible = true;
                    this.RdlReason.Enabled = true;
                    ////addded by ram(445894) for temp access card 
                    this.txtToDate.Enabled = true;
                    /////txtToDate.Enabled = false;
                    VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                    DataSet dtset = new DataSet();
                    dtset = objaccess.GetAccessCardDetails(strAssociateID, issuedLocationID, strDetailId);
                    this.accesstextid.Text = dtset.Tables[1].Rows[0]["CardType"].ToString();
                    this.accesstextid.Visible = false;
                    this.accesstextid.Enabled = false;
                    this.dtselectCardddl = objaccess.GetSelect_CARD();
                    this.ddAccessdetail.DataSource = this.dtselectCardddl;
                    this.ddAccessdetail.Visible = true;
                    this.ddAccessdetail.Enabled = true;
                    this.DDAccessLabel.Enabled = true;
                    VMSBusinessLayer.RequestDetailsBL objaccesschkout = new VMSBusinessLayer.RequestDetailsBL();
                    if (this.accesstextid.Text == "1 Day ID Card")
                    {
                        this.LblAccess.Visible = false;
                        this.txtAccess.Visible = false;
                    }

                    foreach (string strCountrychk in onedayAccesscard)
                    {
                        if (strCountrychk != this.strCountry)
                        {
                            this.InitDates();
                            this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                            this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                            this.ddAccessdetail.DataBind();
                        }
                        else
                        {
                            /////added by ram(445894) for temp access card
                            // System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                            // string format = "dd/MM/yyyy HH:mm:ss";
                            var fromdatefetch = DateTime.Parse(currentdatefetch); ////DateTime.ParseExact(thisdate, format, provider);
                            this.txtFromDate.Text = fromdatefetch.ToString("dd/MMM/yyyy", provider);
                            if (this.Session["currentDateTime"] != null)
                            {
                                string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                                var today = DateTime.ParseExact(currenttime, format, provider);
                                this.txtFromDate.Text = today.ToString("dd/MMM/yyyy", provider);
                                this.txtToDate.Text = today.ToString("dd/MMM/yyyy", provider);
                            }

                            this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Temporary_Cards"].ToString();
                            this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_CARD"].ToString();
                            this.ddAccessdetail.DataBind();
                        }
                    }
                }
                else
                {
                    this.lblTempIdIssued.Text = string.Empty;
                    this.lblTempIdIssued.Visible = false;
                    this.RdlReason.Enabled = true;
                    VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                    DataSet select_card_ddl = new DataSet();
                    select_card_ddl = objaccess.GetSelect_CARD();
                    this.ddAccessdetail.DataSource = select_card_ddl;
                    foreach (string strCountrychk in onedayAccesscard)
                    {
                        if (strCountrychk == this.strCountry)
                        {
                            this.ddAccessdetail.DataTextField = select_card_ddl.Tables[0].Columns["Temporary_Cards"].ToString();
                            this.ddAccessdetail.DataValueField = select_card_ddl.Tables[0].Columns["Select_CARD"].ToString();
                            this.ddAccessdetail.DataBind();
                        }
                        else
                        {
                            this.ddAccessdetail.DataTextField = select_card_ddl.Tables[0].Columns["Select_CARD"].ToString();
                            this.ddAccessdetail.DataValueField = select_card_ddl.Tables[0].Columns["Select_CARD"].ToString();
                            this.ddAccessdetail.DataBind();
                        }
                    }

                    this.accesstextid.Visible = false;
                    if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                    {
                        this.LblAccess.Visible = false;
                        this.txtAccess.Visible = false;
                    }
                }
                ////string[] OnedayAccesscard = PANIND.Split(',');
                foreach (string strCountrychk in onedayAccesscard)
                {
                    if (strCountrychk != this.strCountry)
                    {
                        this.ddAccessdetail.Enabled = false;
                    }
                    else
                    {
                        if (objEmployeeDetails != null)
                        {
                            DataRow associateRow = objEmployeeDetails.GetEmployeeDetails(strAssociateID);
                            if (associateRow != null)
                            {
                                this.ddAccessdetail.Enabled = true;
                            }
                            else
                            {
                                this.ddAccessdetail.Visible = false;
                                this.lblTempIdIssued.Visible = false;
                            }
                        }
                    }
                }

                foreach (string strCountrychk in onedayAccesscard)
                {
                    if (strCountrychk == this.strCountry)
                    {
                        DataSet checkaccesscardhistory = new DataSet();
                        VMSBusinessLayer.RequestDetailsBL objcheckhistory = new VMSBusinessLayer.RequestDetailsBL();
                        checkaccesscardhistory = objcheckhistory.CheckaccesscardhistoryBL(this.txtEmpID.Text);

                        if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                        {
                            this.LblAccess.Visible = true;
                            this.txtAccess.Visible = true;
                            this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                            if (checkaccesscardhistory.Tables[1].Rows.Count == 1)
                            {
                                this.txtToDate.Enabled = false;
                                this.imgToDate.Enabled = false;

                                string accessCardhistorycheck1 = "Associate has already got a temporary Access card, Hence this card will be issued as one day access card.";
                                string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                            }
                            else if (checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                            {
                                this.btnCheckIn.Enabled = false;
                                this.txtToDate.Enabled = false;
                                this.imgToDate.Enabled = false;
                                string accessCardhistorycheck2 = "Associate holds 2 Temporay access cards already,hence new access cards cannot be issued until any one of the past access card(s) is returned.";
                                string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                            }
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                        {
                            this.LblAccess.Visible = false;
                            this.txtAccess.Visible = false;
                            this.txtAccess.Text = string.Empty;
                            this.RequiredFieldValidator1.Text = string.Empty;

                            if (checkaccesscardhistory.Tables[0].Rows.Count == 1)
                            {
                                this.txtToDate.Enabled = false;
                                this.imgToDate.Enabled = false;
                            }
                            else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2)
                            {
                                this.btnCheckIn.Enabled = false;
                                this.txtToDate.Enabled = false;
                                this.imgToDate.Enabled = false;
                            }
                        }
                        else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                        {
                            this.LblAccess.Visible = true;
                            this.txtAccess.Visible = true;
                            this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                            if (checkaccesscardhistory.Tables[0].Rows.Count == 1 || checkaccesscardhistory.Tables[1].Rows.Count == 1)
                            {
                                if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                {
                                    this.btnCheckIn.Enabled = false;
                                    this.txtToDate.Enabled = false;
                                    this.imgToDate.Enabled = false;
                                }
                                else
                                {
                                    this.txtToDate.Enabled = false;
                                    this.imgToDate.Enabled = false;
                                }
                            }
                            else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                            {
                                this.btnCheckIn.Enabled = false;
                                this.txtToDate.Enabled = false;
                                this.imgToDate.Enabled = false;
                            }
                        }
                    }
                }
            }
            else if (e.CommandName == "RePrint")
            {
                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                string info = e.CommandArgument.ToString();
                string[] arg = new string[2];
                char[] splitter = { ';' };
                arg = info.Split(splitter);
                string strDetailId = arg[0];
                string strAssociateID = arg[1];
                this.hdnPassDetailsID.Value = strDetailId.ToString();

                if (objEmployeeDetails != null)
                {
                    DataRow associateRow = objEmployeeDetails.GetEmployeeDetails(strAssociateID);
                    if (associateRow != null)
                    {
                        this.hdnAssociateID.Value = strAssociateID;
                        this.hdnFileUploadID.Value = Convert.ToString(associateRow["FileUploadID"]);
                        this.Session["CHireFileContentID"] = hdnFileUploadID.Value;
                        this.modalReprintComments.Show();
                        ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetOffset", "GetOffset();", true);
                    }
                    else
                    {
                        string inactiveMsg = string.Concat(Resources.LocalizedText.InactiveMsg);
                        string strScript = string.Concat("<script>alert('", inactiveMsg, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    }
                }

                return;
            }
            else if (e.CommandName == "SendReminder")
            {
                // mpeReminderConfirm1.Show();
                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                lbtnSendReminder = (LinkButton)e.Item.FindControl("btnSendReminder");
                if (lbtnSendReminder.Text == "Reminder Mail 2")
                {
                    this.lblReminderCommitting.Text = Resources.LocalizedText.lblReminderCommitting2;
                }
                else
                {
                    this.lblReminderCommitting.Text = Resources.LocalizedText.lblReminderCommitting;
                }

                string[] arg = new string[4];
                string info = Convert.ToString(e.CommandArgument);
                char[] splitter = { ';' };
                arg = info.Split(splitter);
                string strDetailId = arg[5];
                string issuedLocation = arg[6];
                string issuedCity = arg[7];
                this.hdnPassDetailsID.Value = strDetailId.ToString();
                this.hdnIssuedLocation.Value = issuedLocation.Trim();
                this.hdnIssuedCity.Value = issuedCity.Trim();
                if (objEmployeeDetails != null)
                {
                    DataRow associateRow = objEmployeeDetails.GetEmployeeDetails(arg[0]);
                    if (associateRow != null)
                    {
                        this.mpeConfirmRemider.Show();
                        HiddenField hdnfToDate = (HiddenField)e.Item.FindControl("hdnGridToDate");
                        HiddenField hdnStartDate = (HiddenField)e.Item.FindControl("hdnIssuedDate");
                        this.Session["strAssociateID"] = arg[0];
                        this.Session["strAssociateName"] = arg[1];
                        this.Session["strIssuedDate"] = hdnfToDate.Value;
                        this.Session["strIssuedTime"] = arg[3];
                        //// Session["strCardGivenDate"] = arg[4];
                        this.Session["strCardGivenDate"] = hdnStartDate.Value;
                        this.Session["strAssociateMailID"] = this.requestDetails.GetHostmailID(arg[0]);
                    }
                    else
                    {
                        string inactiveMsg = string.Concat(Resources.LocalizedText.InactiveMsg);
                        string strScript = string.Concat("<script>alert('", inactiveMsg, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    }
                }
            }
            else if (e.CommandName == "enablecard")
            {
                string info = e.CommandArgument.ToString();
                string[] arg = new string[2];
                char[] splitter = { ';' };
                arg = info.Split(splitter);
                string strDetailId = arg[0];
                string strAssociateID = arg[1];
                this.Session["enablecardpassdetailID"] = strDetailId;
                this.Session["strAssociateID"] = strAssociateID;
                this.Session["AccessCardNo"] = arg[2];
                this.mpeenablecardpopup.Show();
            }

            return;
        }

        /// <summary>
        /// Method to pre render pager 
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void Pager_PreRender(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Method to handle hidden Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
            this.BindData();
        }

        /// <summary>
        /// Method to handle hidden Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
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
        /// Method to handle hidden Button Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnHiddenReminder_Click(object sender, EventArgs e)
        {
        }

        #endregion

        /// <summary>
        /// Initialize culture
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
        /// Method to handle Selected Index Changed
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void RdlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;

            ////Added for 1 day access card PAN IND RollOut  
            this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
            if (this.securitydetails.Tables[0].Rows.Count > 0)
            {
                this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
            }
            ////Added for 1 day access card PAN IND RollOut  
            string[] onedayAccesscard = this.panIND.Split(',');
            foreach (string strCountrychk in onedayAccesscard)
            {
                if (strCountrychk == this.strCountry)
                {
                    if (this.RdlReason.SelectedValue == VMSConstants.IDFORGOT || this.RdlReason.SelectedValue == VMSConstants.ONSITERETURN ||
                    this.RdlReason.SelectedValue == VMSConstants.IDLOST)
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                        string format = "dd/MM/yyyy HH:mm:ss";
                        if (this.Session["currentDateTime"] != null)
                        {
                            string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                            var today = DateTime.ParseExact(currenttime, format, provider);
                            this.txtFromDate.Text = today.ToString("dd/MMM/yyyy", provider);
                            this.txtToDate.Text = today.ToString("dd/MMM/yyyy", provider);
                        }

                        this.txtToDate.Enabled = true;
                        this.imgToDate.Enabled = true;
                        ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                    }
                }
                else
                {
                    if (this.RdlReason.SelectedValue == VMSConstants.ONSITERETURN ||
                        this.RdlReason.SelectedValue == VMSConstants.IDLOST)
                    {
                        this.txtToDate.Enabled = true;
                        this.imgToDate.Enabled = true;
                        this.txtToDate.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                    }
                    else
                    {
                        ////Added for 1 day access card PAN IND RollOut 
                        this.InitDates();
                        this.txtToDate.Enabled = false;
                        this.imgToDate.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                    }
                }
            }
        }

        /// <summary>
        /// Method for access card visibility
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void DDAccessdetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ImgAssociate.ImageUrl = this.hdnImageURL.Value;

            this.lblTempIdIssued.Visible = false;
            this.btnCheckIn.Enabled = true;
            this.txtToDate.Enabled = true;
            this.imgToDate.Enabled = true;
            ////added by ram (445894) for temp access card 
            this.txtToDate.Text = XSS.HtmlEncode(this.txtFromDate.Text);
            DataSet checkaccesscardhistory = new DataSet();
            VMSBusinessLayer.RequestDetailsBL objcheckhistory = new VMSBusinessLayer.RequestDetailsBL();
            checkaccesscardhistory = objcheckhistory.CheckaccesscardhistoryBL(this.txtEmpID.Text);
            EmployeeBL objEmployeDetails = new EmployeeBL();
            DataRow associateRow = objEmployeDetails.GetTerminatedEmployeeDetails(this.txtEmpID.Text);
            if (Convert.ToString(associateRow["HR_Status"]) == "I")

            {
                this.lblTempIdIssued.Visible = false;
                this.btnCheckIn.Enabled = false;
                this.txtToDate.Enabled = false;
                this.imgToDate.Enabled = false;
            }
            else
            {
                this.lblTempIdIssued.Visible = false;
                this.btnCheckIn.Enabled = true;
                this.txtToDate.Enabled = true;
                this.imgToDate.Enabled = true;
            }
            if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
            {
                this.LblAccess.Visible = true;
                this.txtAccess.Visible = true;
                this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                if (checkaccesscardhistory.Tables[1].Rows.Count == 1)
                {
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;

                    string accessCardhistorycheck1 = "Associate has already got a temporary Access card, Hence this card will be issued as one day access card.";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                {
                    this.btnCheckIn.Enabled = false;
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;
                    string accessCardhistorycheck2 = "Associate holds 2 Temporay access cards already,hence new access cards cannot be issued until any one of the past access card(s) is returned.";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
            }
            else if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
            {
                this.LblAccess.Visible = false;
                this.txtAccess.Visible = false;
                this.txtAccess.Text = string.Empty;
                this.RequiredFieldValidator1.Text = string.Empty;

                if (checkaccesscardhistory.Tables[0].Rows.Count == 1)
                {
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;

                    string accessCardhistorycheck1 = "Associate has already got a temporary ID card, Hence this card will be issued as one day ID card.";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2)
                {
                    this.btnCheckIn.Enabled = false;
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;
                    string accessCardhistorycheck2 = "Associate holds 2 Temporay ID cards already,hence new ID cards cannot be issued until any one of the past ID card(s) is returned.";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
            }
            else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
            {
                this.LblAccess.Visible = true;
                this.txtAccess.Visible = true;
                this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 && checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                {
                    this.btnCheckIn.Enabled = false;
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;
                    string accessCardhistorycheck2 = "Associate holds 2 Temporay ID and Access cards already,hence new cards cannot be issued until any one of the past card(s) is returned.";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 && !(checkaccesscardhistory.Tables[1].Rows.Count >= 2))
                {
                    this.btnCheckIn.Enabled = false;
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;
                    string accessCardhistorycheck2 = "Associate holds 2 Temporay ID cards already,hence new ID cards cannot be issued until any one of the past ID card(s) is returned.";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (checkaccesscardhistory.Tables[1].Rows.Count >= 2 && !(checkaccesscardhistory.Tables[0].Rows.Count >= 2))
                {
                    this.btnCheckIn.Enabled = false;
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;
                    string accessCardhistorycheck2 = "Associate holds 2 Temporay access cards already,hence new access cards cannot be issued until any one of the past access card(s) is returned.";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (checkaccesscardhistory.Tables[0].Rows.Count == 1 && checkaccesscardhistory.Tables[1].Rows.Count == 1)
                {
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;

                    string accessCardhistorycheck1 = "Associate has already got a temporary ID and Access card, Hence this card will be issued as one day card(s).";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (checkaccesscardhistory.Tables[0].Rows.Count == 1 && checkaccesscardhistory.Tables[1].Rows.Count != 1)
                {
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;

                    string accessCardhistorycheck1 = "Associate has already got a temporary ID card, Hence this card will be issued as one day card(s).";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
                else if (checkaccesscardhistory.Tables[1].Rows.Count == 1 && checkaccesscardhistory.Tables[0].Rows.Count != 1)
                {
                    this.txtToDate.Enabled = false;
                    this.imgToDate.Enabled = false;

                    string accessCardhistorycheck1 = "Associate has already got a temporary Access card, Hence this card will be issued as one day card(s).";
                    string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                }
            }
        }

        /// <summary>
        /// Method to handle Refresh One day Card tick
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void RefreshOneDayCard_Tick(object sender, EventArgs e)
        {
            this.BindData();
        }

        #region Private Methods

        /// <summary>
        /// Method to search for user details and Populating them in the page.
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>
        /// <param name="objEmployeeDetails">Employee Details object</param>
        private void PopulateAssociateData(string strAssociateID, EmployeeBL objEmployeeDetails)
        {
            try
            {
                string templateId = string.Empty;
                ////   EmployeeBL objEmployeeDetails = new EmployeeBL();
                DataRow associateRow = objEmployeeDetails.GetEmployeeDetails(strAssociateID);
                this.txtEmpID.Text = strAssociateID;
                
                string strMessage = string.Concat(Resources.LocalizedText.Notification);
                if (associateRow != null)
                {
                    this.hdnFileUploadID.Value = Convert.ToString(associateRow["FileUploadID"]);
                    this.Session["CHireFileContentID"] = hdnFileUploadID.Value;
                    this.SetVisibilityLevel("SEARCH");  // Convert.ToInt16(hdnFacility.Value.ToString()) lblCardIssuedFacilityName.Text.ToString().Trim()
                    ////hdnFacility.Value = LocationId.ToString();
                    string strCardIssued = objEmployeeDetails.IsCardIssuedLocation(strAssociateID, Convert.ToInt16(this.hdnFacility.Value.ToString()));
                    /*grid popualte for assocaite*/
                    this.BindData();
                    if (!string.IsNullOrEmpty(strCardIssued))
                    {
                        if (Convert.ToInt32(strCardIssued.Split('|')[1]) == 1)
                        {
                            string[] onedayAccesscard = this.panIND.Split(',');
                            this.hdnPassDetailsID.Value = strCardIssued.Split('|')[0].ToString();
                            ////ADDED FOR ACCESS CARD
                            VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                            ////txtAccess.Text = objaccess.GetAccessCardDetails(strAssociateID).ToString();
                            DataSet dtset = new DataSet();
                            dtset = objaccess.GetAccessCardDetails(strAssociateID, this.hdnFacility.Value.ToString(), this.hdnPassDetailsID.Value.ToString());

                            /*change by ram*/
                            //// txtAccess.Text = dtset.Tables[0].Rows[0]["AccessCardNumber"].ToString();

                            ////ddAccessdetail.SelectedValue = dtset.Tables[1].Rows[0]["CardType"].ToString();

                            this.SetVisibilityLevel("CHECKEDIN");

                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (strCountrychk == this.strCountry)
                                {
                                    DataSet checkaccesscardhistory = new DataSet();
                                    VMSBusinessLayer.RequestDetailsBL objcheckhistory = new VMSBusinessLayer.RequestDetailsBL();
                                    checkaccesscardhistory = objcheckhistory.CheckaccesscardhistoryBL(this.txtEmpID.Text);

                                    if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                    {
                                        this.LblAccess.Visible = true;
                                        this.txtAccess.Visible = true;
                                        this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                                        if (checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                        {
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;

                                            string pendingrequest = string.Concat(Resources.LocalizedText.Pendingrequest);
                                            string accessCardhistorycheck1 = ".       Associate has already got a temporary Access card, Hence this card will be issued as one day access card.";
                                            string strScript = string.Concat("<script>alert('", pendingrequest, "'); </script>") + accessCardhistorycheck1;
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                        else if (checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                        {
                                            this.btnCheckIn.Enabled = false;
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;

                                            string pendingrequest = string.Concat(Resources.LocalizedText.Pendingrequest);
                                            string accessCardhistorycheck2 = ".       Associate holds 2 Temporay access cards already,hence new access cards cannot be issued until any one of the past access card(s) is returned.";
                                            string strScript = string.Concat("<script>alert('", pendingrequest, "'); </script>") + accessCardhistorycheck2;
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }
                                    else if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                                    {
                                        this.LblAccess.Visible = false;
                                        this.txtAccess.Visible = false;
                                        this.txtAccess.Text = string.Empty;
                                        this.RequiredFieldValidator1.Text = string.Empty;
                                        if (checkaccesscardhistory.Tables[0].Rows.Count == 1)
                                        {
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;
                                            this.btnCheckIn.Enabled = true;
                                            string accessCardhistorycheck1 = ".        Associate has already got a temporary ID card, Hence this card will be issued as one day ID card.";
                                            string pendingrequest = string.Concat(Resources.LocalizedText.Pendingrequest) + accessCardhistorycheck1;

                                            string strScript = string.Concat("<script>alert('", pendingrequest, "'); </script>"); //// +AccessCardhistorycheck1;
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                        else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2)
                                        {
                                            this.btnCheckIn.Enabled = false;
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;

                                            string accessCardhistorycheck2 = ".       Associate holds 2 Temporay ID cards already,hence new ID cards cannot be issued until any one of the past ID card(s) is returned.";
                                            string pendingrequest = string.Concat(Resources.LocalizedText.Pendingrequest) + accessCardhistorycheck2;

                                            string strScript = string.Concat("<script>alert('", pendingrequest, "'); </script>"); //// +AccessCardhistorycheck2;
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }
                                    else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                    {
                                        this.LblAccess.Visible = true;
                                        this.txtAccess.Visible = true;
                                        this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                                        if (checkaccesscardhistory.Tables[0].Rows.Count == 1 || checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                        {
                                            if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                            {
                                                this.btnCheckIn.Enabled = false;
                                                this.txtToDate.Enabled = false;
                                                this.imgToDate.Enabled = false;

                                                string accessCardhistorycheck2 = ".         Associate holds 2 Temporay ID and Access cards already,hence new cards cannot be issued until any one of the past card(s) is returned.";
                                                string pendingrequest = string.Concat(Resources.LocalizedText.Pendingrequest) + accessCardhistorycheck2;

                                                string strScript = string.Concat("<script>alert('", pendingrequest, "'); </script>");
                                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                            }
                                            else
                                            {
                                                this.txtToDate.Enabled = false;
                                                this.imgToDate.Enabled = false;

                                                string accessCardhistorycheck1 = ".       Associate has already got a temporary ID or Access card, Hence this card will be issued as one day card.";
                                                string pendingrequest = string.Concat(Resources.LocalizedText.Pendingrequest) + accessCardhistorycheck1;

                                                string strScript = string.Concat("<script>alert('", pendingrequest, "'); </script>"); //// +AccessCardhistorycheck1;
                                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                            }
                                        }
                                        else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                        {
                                            this.btnCheckIn.Enabled = false;
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;
                                            string accessCardhistorycheck2 = ".       Associate holds 2 Temporay Id and access cards already,hence new cards cannot be issued until any one of the past cards is returned.";
                                            string pendingrequest = string.Concat(Resources.LocalizedText.Pendingrequest) + accessCardhistorycheck2;

                                            string strScript = string.Concat("<script>alert('", pendingrequest, "'); </script>"); //// +AccessCardhistorycheck2;
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.SetVisibilityLevel("SEARCH");
                            string[] onedayAccesscard = this.panIND.Split(',');
                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (strCountrychk == this.strCountry)
                                {
                                    DataSet checkaccesscardhistory = new DataSet();
                                    VMSBusinessLayer.RequestDetailsBL objcheckhistory = new VMSBusinessLayer.RequestDetailsBL();
                                    checkaccesscardhistory = objcheckhistory.CheckaccesscardhistoryBL(this.txtEmpID.Text);

                                    if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                    {
                                        this.LblAccess.Visible = true;
                                        this.txtAccess.Visible = true;
                                        this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                                        if (checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                        {
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;

                                            string accessCardhistorycheck1 = "Associate has already got a temporary Access card, Hence this card will be issued as one day access card.";
                                            string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                        else if (checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                        {
                                            this.btnCheckIn.Enabled = false;
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;
                                            string accessCardhistorycheck2 = "Associate holds 2 Temporay access cards already,hence new access cards cannot be issued until any one of the past access card(s) is returned.";
                                            string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }
                                    else if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                                    {
                                        this.LblAccess.Visible = false;
                                        this.txtAccess.Visible = false;
                                        this.txtAccess.Text = string.Empty;
                                        this.RequiredFieldValidator1.Text = string.Empty;
                                        if (checkaccesscardhistory.Tables[0].Rows.Count == 1)
                                        {
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;
                                            this.btnCheckIn.Enabled = true;
                                            string accessCardhistorycheck1 = "Associate has already got a temporary ID card, Hence this card will be issued as one day ID card.";
                                            string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                        else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2)
                                        {
                                            this.btnCheckIn.Enabled = false;
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;
                                            string accessCardhistorycheck2 = "Associate holds 2 Temporay ID cards already,hence new ID cards cannot be issued until any one of the past ID card(s) is returned.";
                                            string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }
                                    else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                    {
                                        this.LblAccess.Visible = true;
                                        this.txtAccess.Visible = true;
                                        this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                                        if (checkaccesscardhistory.Tables[0].Rows.Count == 1 || checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                        {
                                            if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                            {
                                                this.btnCheckIn.Enabled = false;
                                                this.txtToDate.Enabled = false;
                                                this.imgToDate.Enabled = false;
                                                string accessCardhistorycheck2 = "Associate holds 2 Temporay ID and Access cards already,hence new cards cannot be issued until any one of the past card(s) is returned.";
                                                string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                            }
                                            else
                                            {
                                                this.txtToDate.Enabled = false;
                                                this.imgToDate.Enabled = false;

                                                string accessCardhistorycheck1 = "Associate has already got a temporary ID and Access card, Hence this card will be issued as one day card.";
                                                string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                            }
                                        }
                                        else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                        {
                                            this.btnCheckIn.Enabled = false;
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;
                                            string accessCardhistorycheck2 = "Associate holds 2 Temporay Id and access cards already,hence new cards cannot be issued until any one of the past cards is returned.";
                                            string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string[] onedayAccesscard = this.panIND.Split(',');
                        foreach (string strCountrychk in onedayAccesscard)
                        {
                            if (strCountrychk == this.strCountry)
                            {
                                DataSet checkaccesscardhistory = new DataSet();
                                VMSBusinessLayer.RequestDetailsBL objcheckhistory = new VMSBusinessLayer.RequestDetailsBL();
                                checkaccesscardhistory = objcheckhistory.CheckaccesscardhistoryBL(this.txtEmpID.Text);

                                if (this.ddAccessdetail.SelectedValue == "1 Day Access Card")
                                {
                                    this.LblAccess.Visible = true;
                                    this.txtAccess.Visible = true;
                                    this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                                    if (checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                    {
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;

                                        string accessCardhistorycheck1 = "Associate has already got a temporary Access card, Hence this card will be issued as one day access card.";
                                        string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                    }
                                    else if (checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                    {
                                        this.btnCheckIn.Enabled = false;
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                        string accessCardhistorycheck2 = "Associate holds 2 Temporay access cards already,hence new access cards cannot be issued until any one of the past access card(s) is returned.";
                                        string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                    }
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 Day ID Card")
                                {
                                    this.LblAccess.Visible = false;
                                    this.txtAccess.Visible = false;
                                    this.txtAccess.Text = string.Empty;
                                    this.RequiredFieldValidator1.Text = string.Empty;
                                    if (checkaccesscardhistory.Tables[0].Rows.Count == 1)
                                    {
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                        this.btnCheckIn.Enabled = true;
                                        string accessCardhistorycheck1 = "Associate has already got a temporary ID card, Hence this card will be issued as one day ID card.";
                                        string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                    }
                                    else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2)
                                    {
                                        this.btnCheckIn.Enabled = false;
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                        string accessCardhistorycheck2 = "Associate holds 2 Temporay ID cards already,hence new ID cards cannot be issued until any one of the past ID card(s) is returned.";
                                        string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                    }
                                }
                                else if (this.ddAccessdetail.SelectedValue == "1 day ID Card and Access Card")
                                {
                                    this.LblAccess.Visible = true;
                                    this.txtAccess.Visible = true;
                                    this.RequiredFieldValidator1.Text = "Please enter the Access Card ID!";

                                    if (checkaccesscardhistory.Tables[0].Rows.Count == 1 || checkaccesscardhistory.Tables[1].Rows.Count == 1)
                                    {
                                        if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                        {
                                            this.btnCheckIn.Enabled = false;
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;
                                            string accessCardhistorycheck2 = "Associate holds 2 Temporay ID and Access cards already,hence new cards cannot be issued until any one of the past card(s) is returned.";
                                            string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                        else
                                        {
                                            this.txtToDate.Enabled = false;
                                            this.imgToDate.Enabled = false;

                                            string accessCardhistorycheck1 = "Associate has already got a temporary ID and Access card, Hence this card will be issued as one day card.";
                                            string strScript = string.Concat("<script>alert('", accessCardhistorycheck1, "'); </script>");
                                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                        }
                                    }
                                    else if (checkaccesscardhistory.Tables[0].Rows.Count >= 2 || checkaccesscardhistory.Tables[1].Rows.Count >= 2)
                                    {
                                        this.btnCheckIn.Enabled = false;
                                        this.txtToDate.Enabled = false;
                                        this.imgToDate.Enabled = false;
                                        string accessCardhistorycheck2 = "Associate holds 2 Temporay Id and access cards already,hence new cards cannot be issued until any one of the past cards is returned.";
                                        string strScript = string.Concat("<script>alert('", accessCardhistorycheck2, "'); </script>");
                                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                                    }
                                }
                            }
                        }
                    }

                    foreach (ListViewItem items in this.lstvwResults.Items)
                    {
                        LinkButton print = (LinkButton)items.FindControl("btnPrint");
                        print.Enabled = false;
                        print.CssClass = "GridLinkButtonDisabled";
                    }

                    this.BindData();
                    this.errortbl.Visible = false;
                    System.Text.RegularExpressions.Regex nonNumericCharacters = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                    this.lblEmpID.Text = Convert.ToString(associateRow["Associate_id"]);
                    this.ViewState["AssociateID"] = this.lblEmpID.Text.ToString();
                    this.lblEmpName.Text = Convert.ToString(associateRow["AssociateName"]);
                    this.lblEmpEmailID.Text = Convert.ToString(associateRow["EmailID"]);
                    this.lblEmpLocation.Text = Convert.ToString(associateRow["Location"]);
                    this.lblEmpCity.Text = Convert.ToString(associateRow["City"]);
                    this.lblEmployeeMobile.Text = nonNumericCharacters.Replace(Convert.ToString(associateRow["Mobile"]), string.Empty);
                    this.lblEmployeeExtension.Text = Convert.ToString(associateRow["Vnet"]);
                    this.lblMgrID.Text = Convert.ToString(associateRow["ManagerID"]);
                    this.lblMgrName.Text = Convert.ToString(associateRow["ManagerName"]);
                    this.lblMgrEmailID.Text = Convert.ToString(associateRow["ManagerEmailID"]);
                    this.lblManagerMobileNo.Text = nonNumericCharacters.Replace(Convert.ToString(associateRow["ManagerMobile"]), string.Empty);
                    this.lblManagerExtension.Text = Convert.ToString(associateRow["ManagerVnet"]);
                    ////Session["ManagerEmailID"] = Convert.ToString(AssociateRow["ManagerEmailID"]);
                    this.hdnManagerEmailID.Value = Convert.ToString(associateRow["ManagerEmailID"]);
                    this.hdnManagerName.Value = Convert.ToString(associateRow["ManagerName"]);
                    this.hdnEmployeeName.Value = Convert.ToString(associateRow["AssociateName"]);
                    this.hdnAssociateID.Value = Convert.ToString(associateRow["Associate_id"]);

                    this.ImgAssociate.ImageUrl = this.hdnImageURL.Value = VMSConstants.IMAGEPATH;
                    this.hdnFileUploadID.Value = null;
                    this.hdnFileUploadID.Value = Convert.ToString(associateRow["FileUploadID"]);
                    this.Session["CHireFileContentID"] = hdnFileUploadID.Value;
                    templateId = this.hdnFileUploadID.Value;
                    ////if (!string.IsNullOrEmpty(templateId))
                    ////{
                        this.ImgAssociate.Visible = true;
                        string strWidth = Convert.ToString(ConfigurationManager.AppSettings["ThumbnailW"]);
                        string strHeight = Convert.ToString(ConfigurationManager.AppSettings["ThumbnailH"]);
                        ////this.ImgAssociate.ImageUrl = string.Concat("GetImage1.ashx?IDCARD=", Convert.ToString(associateRow["Associate_id"]), "&w=", strWidth, "&h=", strHeight, "&TempId=", templateId);
                        ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetImageFromIDCard", "GetImageFromIDCard();", true);
                   //// }
                    ////else
                    ////{
                    ////    this.ImgAssociate.ImageUrl = this.hdnImageURL.Value = VMSConstants.IMAGEPATH;
                    ////}
                }
                else
                {
                    this.SetVisibilityLevel("PAGELOAD");
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = Resources.LocalizedText.InValidAssociateId;
                    this.lblIDCardStatus.Visible = false;
                }

                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to search for Terminated Associate Details Commented for CR37
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>
        /// <param name="objEmployeeDetails">Employee Details object</param>
        /// <returns>Row of data</returns>
        private DataRow PopulateTerminatedAssociateData(string strAssociateID, EmployeeBL objEmployeeDetails)
        {
            try
            {
                string templateId = string.Empty;
                ////   EmployeeBL objEmployeeDetails = new EmployeeBL();
                DataRow associateRow = objEmployeeDetails.GetTerminatedEmployeeDetails(strAssociateID);
                this.txtEmpID.Text = strAssociateID;
                string strMessage = string.Concat(Resources.LocalizedText.Notification);
                if (associateRow != null)
                {
                    this.SetVisibilityLevel("SEARCH");  // Convert.ToInt16(hdnFacility.Value.ToString()) lblCardIssuedFacilityName.Text.ToString().Trim()
                    ////hdnFacility.Value = LocationId.ToString();
                    string strCardIssued = objEmployeeDetails.IsCardIssuedLocation(strAssociateID, Convert.ToInt16(this.hdnFacility.Value.ToString()));
                    if (!string.IsNullOrEmpty(strCardIssued))
                    {
                        if (Convert.ToInt32(strCardIssued.Split('|')[1]) == 1)
                        {
                            this.hdnPassDetailsID.Value = strCardIssued.Split('|')[0].ToString();
                            this.SetVisibilityLevel("CHECKEDIN");
                            ////MsgBox.Show(VMSDev.UserControls.MsgBox.MsgBoxType.Information, strMessage);
                        }
                        else
                        {
                            this.SetVisibilityLevel("SEARCH");
                        }
                    }

                    this.errortbl.Visible = false;
                    System.Text.RegularExpressions.Regex nonNumericCharacters = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                    this.lblEmpID.Text = XSS.HtmlEncode(Convert.ToString(associateRow["Associate_id"]));
                    this.ViewState["AssociateID"] = this.lblEmpID.Text.ToString();
                    this.lblEmpName.Text = XSS.HtmlEncode(Convert.ToString(associateRow["AssociateName"]));
                    this.lblEmpEmailID.Text = XSS.HtmlEncode(Convert.ToString(associateRow["EmailID"]));
                    this.lblEmpLocation.Text = XSS.HtmlEncode(Convert.ToString(associateRow["Location"]));
                    this.lblEmpCity.Text = XSS.HtmlEncode(Convert.ToString(associateRow["City"]));
                    this.lblEmployeeMobile.Text = XSS.HtmlEncode(nonNumericCharacters.Replace(Convert.ToString(associateRow["Mobile"]), string.Empty));
                    this.lblEmployeeExtension.Text = XSS.HtmlEncode(Convert.ToString(associateRow["Vnet"]));
                    this.lblMgrID.Text = XSS.HtmlEncode(Convert.ToString(associateRow["ManagerID"]));
                    this.lblMgrName.Text = XSS.HtmlEncode(Convert.ToString(associateRow["ManagerName"]));
                    this.lblMgrEmailID.Text = XSS.HtmlEncode(Convert.ToString(associateRow["ManagerEmailID"]));
                    this.lblManagerMobileNo.Text = XSS.HtmlEncode(nonNumericCharacters.Replace(Convert.ToString(associateRow["ManagerMobile"]), string.Empty));
                    this.lblManagerExtension.Text = XSS.HtmlEncode(Convert.ToString(associateRow["ManagerVnet"]));
                    //// Session["ManagerEmailID"] = Convert.ToString(AssociateRow["ManagerEmailID"]);
                    this.hdnManagerEmailID.Value = XSS.HtmlEncode(Convert.ToString(associateRow["ManagerEmailID"]));

                    ////Session["ManagerName"] = Convert.ToString(AssociateRow["ManagerName"]);
                    this.hdnManagerName.Value = XSS.HtmlEncode(Convert.ToString(associateRow["ManagerName"]));

                    ////Session["EmployeeName"] = Convert.ToString(AssociateRow["AssociateName"]);
                    this.hdnEmployeeName.Value = XSS.HtmlEncode(Convert.ToString(associateRow["AssociateName"]));
                    ////Session["AssociateID"] = null;

                    ////Session["AssociateID"] = Convert.ToString(AssociateRow["Associate_id"]);
                    this.hdnAssociateID.Value = XSS.HtmlEncode(Convert.ToString(associateRow["Associate_id"]));

                    ////need touncomment.
                   // this.hdnFileUploadID.Value = null;
                   // this.hdnFileUploadID.Value = XSS.HtmlEncode(Convert.ToString(associateRow["FileUploadID"]));
                    templateId = XSS.HtmlEncode(this.hdnFileUploadID.Value);
                }
                else
                {
                    this.SetVisibilityLevel("PAGELOAD");
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = Resources.LocalizedText.NoRecordFound;
                    this.lblIDCardStatus.Visible = false;
                }

                if (!string.IsNullOrEmpty(templateId))
                {
                    this.ImgAssociate.Visible = true;
                    string strWidth = Convert.ToString(ConfigurationManager.AppSettings["ThumbnailW"]);
                    string strHeight = Convert.ToString(ConfigurationManager.AppSettings["ThumbnailH"]);
                    ////this.ImgAssociate.ImageUrl = string.Concat("GetImage1.ashx?IDCARD=", XSS.HtmlEncode(Convert.ToString(associateRow["Associate_id"])), "&w=", strWidth, "&h=", strHeight, "&TempId=", templateId);
                    ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "GetImageFromIDCard", "GetImageFromIDCard();", true);
                }
                else
                {
                    this.ImgAssociate.ImageUrl = this.hdnImageURL.Value = VMSConstants.IMAGEPATH;
                }

                ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                return associateRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to send mail to the manager  after issuing temporary card        
        /// </summary>
        /// <param name="passNumber">Pass Number</param>
        /// <param name="managerEmailID">Manager Email ID</param>
        /// <param name="managerName">Manager Name</param>
        /// <param name="employeeName">Employee Name</param>
        /// <param name="associateID">Associate ID</param>
        /// <param name="accesstype">Access type</param>
        /// <param name="accessCardNo">Access Card No</param>
        private void SendMail(string passNumber, string managerEmailID, string managerName, string employeeName, string associateID, string accesstype, string accessCardNo)
        {
            try
            {
                MailNotification objMailNotofication = new MailNotification();
                SMSNotification objSMSNotification = new SMSNotification();
                ////added by bincey for OneCommunicator mail get intime
                DateTime today = this.genTimeZone.GetLocalCurrentDateInFormat();
                string dateToday = today.ToString("dd/MMM/yyyy");
                string time = today.ToShortTimeString();
                string strFromDate = this.txtFromDate.Text.ToString();
                string strToDate = this.txtToDate.Text.ToString();
                ////by Krishna (449138)
                DateTime todate = Convert.ToDateTime(strToDate);
                string strToTime = DateTime.Now.ToString("HH:mm:ss tt");

                DateTime todateTime = Convert.ToDateTime(strToDate).Date.Add(Convert.ToDateTime(strToTime).TimeOfDay);
                string returnDateTime = todateTime.AddHours(18).ToString();
                string strEmpemailID = this.lblEmpEmailID.Text.ToString();
                string strManagerEmailId = managerEmailID; ////Convert.ToString(Session["ManagerEmailID"]);
                string strManagerName = managerName; //// Convert.ToString(Session["ManagerName"]);
                ////added by bincey on 02 Nov
                string strManagerID = this.lblMgrID.Text;
                string strEmployeeName = employeeName; //// Convert.ToString(Session["EmployeeName"]);
                string strFromAddress = Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]);
                string strSMTPHostAddress = Convert.ToString(ConfigurationManager.AppSettings["SMTPHostAddress"]);
                int ismtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                string strSubjectLine = string.Concat(ConfigurationManager.AppSettings["IVSMailSubjectLine"].ToString(), associateID); ////lblCardIssuedCityName  ddlFacility.SelectedItem.Text.ToString().Trim()  ddlLocation.SelectedItem.ToString().Trim()
                                                                                                                                       ////string AssociateID = associateID.Trim();
                VMSBusinessLayer.RequestDetailsBL objrequestDetails = new VMSBusinessLayer.RequestDetailsBL();
                int designationstatus = objrequestDetails.GetAssociateDes_Mailer(associateID.Trim()); /*CR-50 Designationstatus--1(SD and above)*/

                ////added by ram(445894) for temp access card
                string reasonforissue = this.RdlReason.SelectedValue;
                if (strManagerEmailId != string.Empty && strManagerEmailId != null)
                {
                    objMailNotofication.IVSmailtoHCM(associateID.ToString().Trim(), strEmpemailID.Trim(), strManagerName.Trim(), strEmployeeName.Trim(), strManagerEmailId.Trim(), this.lblCardIssuedFacilityName.Text.ToString().Trim(), strSubjectLine.Trim(), this.lblCardIssuedCityName.Text.ToString().Trim(), Session["SecurityCountry"].ToString().Trim(), strManagerID, time, passNumber, accesstype, designationstatus, reasonforissue, accessCardNo, returnDateTime);
                }

                objSMSNotification.SendIVSSmsToHCM(associateID.ToString().Trim(), strEmployeeName.Trim(), this.lblCardIssuedFacilityName.Text.ToString().Trim(), this.lblCardIssuedCityName.Text.ToString().Trim(), time, passNumber, strManagerID, accesstype, Session["SecurityCountry"].ToString().Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to send lost mail report to POC      
        /// </summary>
        /// <param name="associateID">Associate ID</param>
        /// <param name="associateName">Associate Name</param>
        /// <param name="strfacility">Facility Name</param>
        /// <param name="city">City Name</param>
        /// <param name="country">Country Name</param>
        /// <param name="facilityid">facility Id</param>
        /// <param name="accessCardNo">Access Card Number</param>
        /// <param name="requestid">Request Id</param>
        private void LostMailReportToPOC(string associateID, string associateName, string strfacility, string city, string country, string facilityid, string accessCardNo, string requestid)
        {
            EmployeeBL objpocid = new EmployeeBL();
            DataSet pocList = new DataSet();

            ////added by Krishna(449138) fro temp access card 
            pocList = objpocid.GetLostMailerACCPOCID(facilityid);
            string pocId = pocList.Tables[0].Rows[0]["UserID"].ToString();
            string getdate = System.DateTime.Today.ToShortDateString();
            ////string strToAddress = System.Configuration.ConfigurationManager.AppSettings["ToAccessAdmin"].ToString();
            string strToAddress = pocId;
            ////string adminList = ConfigurationManager.AppSettings["CCList"];
            string requestId = requestid;
            var requestXML = new StringBuilder();
            requestXML.Append("<OneCommunicator version=\"1\">");
            requestXML.Append("<TransactionParameters>");
            requestXML.Append("<GlobalAppId>415</GlobalAppId>");
            requestXML.Append("<Process>IVS_LostMailProcess</Process>");
            requestXML.Append("<RequestId>" + requestId + "</RequestId>");
            requestXML.Append("<Recipients>" + strToAddress + "</Recipients>");
            requestXML.Append("</TransactionParameters>");
            requestXML.Append("<ChannelParameters>");
            requestXML.Append("<OCS ></OCS>");
            requestXML.Append("<Email>");
            requestXML.Append("<CC>" + null + "</CC>");
            requestXML.Append("<BCC/>");
            requestXML.Append("<TemplateParameters>");
            requestXML.Append("<AssociateID> " + associateID + "</AssociateID>");
            requestXML.Append("<AssociateName> " + associateName + "</AssociateName>");
            requestXML.Append("<getdate> " + getdate + "</getdate>");
            requestXML.Append("<Facility> " + strfacility + "</Facility>");
            requestXML.Append("<City> " + this.cityName + "</City>");
            requestXML.Append("<Country> " + country + "</Country>");
            requestXML.Append("<AccessCardNo> " + accessCardNo + "</AccessCardNo>");
            requestXML.Append("</TemplateParameters>");
            requestXML.Append("</Email>");
            requestXML.Append("<SMS></SMS>");
            requestXML.Append("</ChannelParameters>");
            requestXML.Append("</OneCommunicator>");
            RequestUnifiedVASContractClient objOneCommEmailClient = new RequestUnifiedVASContractClient();
            objOneCommEmailClient.Notify(requestXML.ToString(), null);
        }

        /// <summary>
        /// Method to set the visibility level 
        /// Input flag
        /// </summary>
        /// <param name="strCase">Case of string</param>
        private void SetVisibilityLevel(string strCase)
        {
            try
            {
                this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                this.strCountry = this.securitydetails.Tables[0].Rows[0]["CountryId"].ToString();
                switch (strCase.ToUpper().Trim())
                {
                    case "CHECKEDIN":
                        {
                            this.btnSearch.Enabled = false;

                            this.btnClear.Visible = true;
                            this.btnClear.Enabled = true;

                            this.lblCardIssuedCity.Visible = true;
                            this.lblCardIssuedFacility.Visible = true;

                            this.lblCardIssuedCityName.Visible = true;
                            this.lblCardIssuedFacilityName.Visible = true;
                            this.panelEmp.Visible = true;

                            this.lblEmployeeHeader.Visible = true;
                            this.lblManagerHeader.Visible = true;

                            this.btnCheckIn.Visible = true;
                            this.btnCheckOut.Visible = true;
                            this.btnReprint.Visible = true;
                            ////RdlReason.Enabled = false; // added for access card
                            this.btnCheckIn.Enabled = false;
                            ////btnCheckOut.Enabled = true;
                            this.btnReprint.Enabled = true;
                            string[] onedayAccesscard = this.panIND.Split(',');
                            ////Added for 1 day access card PAN IND RollOut  
                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (strCountrychk == this.strCountry)
                                {
                                    this.txtToDate.Enabled = true;
                                    this.RdlReason.Enabled = true;
                                    this.ddAccessdetail.Visible = true;
                                    this.ddAccessdetail.Enabled = true;
                                    this.btnCheckOut.Enabled = true;
                                    this.btnCheckIn.Enabled = false;
                                    this.accesstextid.Text = string.Empty;
                                    this.lblTempIdIssued.Visible = false;
                                    this.CompareValidator2.ErrorMessage = "Invalid Access Card ID!";
                                }
                                else
                                {
                                    if (this.RdlReason.SelectedValue == VMSConstants.IDFORGOT)
                                    {
                                        VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                                        this.dtselectCardddl = objaccess.GetSelect_CARD();
                                        this.ddAccessdetail.DataSource = this.dtselectCardddl;
                                        this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                        this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                        this.ddAccessdetail.DataBind();
                                        this.txtToDate.Enabled = false;
                                        this.RdlReason.Enabled = false;
                                        this.btnCheckOut.Enabled = true;
                                        this.ddAccessdetail.Visible = true;
                                        this.ddAccessdetail.Enabled = false;
                                        this.btnCheckIn.Enabled = false;
                                        this.txtAccess.Visible = false;
                                    }
                                    else
                                    {
                                        VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                                        this.dtselectCardddl = objaccess.GetSelect_CARD();
                                        this.ddAccessdetail.DataSource = this.dtselectCardddl;
                                        this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                        this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                        this.ddAccessdetail.DataBind();
                                        this.txtToDate.Enabled = true;
                                        this.RdlReason.Enabled = false;
                                        this.btnCheckOut.Enabled = true;
                                        this.ddAccessdetail.Visible = true;
                                        this.ddAccessdetail.Enabled = false;
                                        this.btnCheckIn.Enabled = false;
                                        this.txtAccess.Visible = false;
                                        ////accesstextid.Visible = true;
                                    }
                                }
                            }

                            this.txtEmpID.ReadOnly = true;
                            this.LblAccess.Visible = false;
                            this.txtAccess.ReadOnly = false;
                            this.txtAccess.Visible = false;
                            this.DDAccessLabel.Enabled = true;

                            break;
                        }

                    case "SEARCH":
                        {
                            this.btnSearch.Enabled = false;
                            this.btnClear.Visible = true;
                            this.btnClear.Enabled = true;
                            this.lblCardIssuedCity.Visible = true;
                            this.lblCardIssuedFacility.Visible = true;
                            this.lblCardIssuedCityName.Visible = true;
                            this.lblCardIssuedFacilityName.Visible = true;
                            this.panelEmp.Visible = true;
                            this.lblEmployeeHeader.Visible = true;
                            this.lblManagerHeader.Visible = true;
                            this.btnCheckIn.Visible = true;
                            this.btnCheckOut.Visible = true;
                            this.btnReprint.Visible = true;
                            this.btnCheckIn.Enabled = true;
                            this.txtEmpID.ReadOnly = true;
                            this.LblReason.Visible = true;
                            this.RdlReason.Visible = true;
                            this.LblFromDate.Visible = true;
                            this.LblToDate.Visible = true;
                            this.txtToDate.Visible = true;
                            ////txtToDate.Enabled = false;
                            this.txtFromDate.Visible = true;
                            this.imgToDate.Visible = true;
                            ////imgToDate.Enabled = false;
                            this.txtFromDate.Enabled = false;
                            ////Added for 1 day access card PAN IND RollOut  
                            string[] onedayAccesscard = this.panIND.Split(',');
                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (strCountrychk == this.strCountry)
                                {
                                    VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                                    this.dtselectCardddl = objaccess.GetSelect_CARD();
                                    this.ddAccessdetail.DataSource = this.dtselectCardddl;
                                    this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Temporary_Cards"].ToString();
                                    this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                    this.ddAccessdetail.DataBind();
                                    ////imgToDate.Visible = false;

                                    //////////added by ram(445894) for temp access card 
                                    //////imgToDate.Visible = true;
                                    ////added by ram(445894) for temp access card 
                                    this.txtToDate.Enabled = true;
                                    this.imgToDate.Enabled = true;
                                    this.btnCheckOut.Enabled = true;
                                    this.btnReprint.Enabled = true;
                                    this.btnCheckIn.Enabled = true;
                                    this.ddAccessdetail.Visible = true;
                                    this.ddAccessdetail.Enabled = true;
                                    ////ddAccessdetail.Enabled = false;
                                    this.RdlReason.Enabled = true;
                                    this.lblTempIdIssued.Visible = false;
                                }
                                else
                                {
                                    VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                                    this.dtselectCardddl = objaccess.GetSelect_CARD();
                                    this.ddAccessdetail.DataSource = this.dtselectCardddl;
                                    this.ddAccessdetail.DataTextField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                    this.ddAccessdetail.DataValueField = this.dtselectCardddl.Tables[0].Columns["Select_Card"].ToString();
                                    this.ddAccessdetail.DataBind();
                                    this.imgToDate.Visible = true;
                                    this.btnCheckOut.Enabled = false;
                                    this.btnReprint.Enabled = false;
                                    this.btnCheckIn.Enabled = true;
                                    this.ddAccessdetail.Visible = true;
                                    this.ddAccessdetail.Enabled = false;
                                    this.txtAccess.Visible = false;
                                    this.RdlReason.Enabled = true;
                                    this.lblTempIdIssued.Visible = true;
                                    ////added by ram(445894) for temp access card 
                                    this.txtToDate.Enabled = false;
                                }
                            }
                            this.LblAccess.Visible = false;
                            this.txtAccess.ReadOnly = false;
                            this.txtAccess.Visible = false;
                            this.DDAccessLabel.Visible = true;
                            ////accesstextid.Visible = false;
                            break;
                        }

                    case "PAGELOAD":
                        {
                            this.btnSearch.Enabled = true;
                            this.btnClear.Visible = false;
                            this.btnClear.Enabled = true;
                            this.lblCardIssuedCity.Visible = false;
                            this.lblCardIssuedFacility.Visible = false;
                            this.lblCardIssuedCityName.Visible = false;
                            this.lblCardIssuedFacilityName.Visible = false;
                            this.lblTempIdIssued.Visible = false;
                            this.panelEmp.Visible = false;
                            ////lblSuccessMessage.Visible = false;
                            this.lblEmployeeHeader.Visible = false;
                            this.lblManagerHeader.Visible = false;
                            this.btnCheckIn.Visible = false;
                            this.btnCheckOut.Visible = false;
                            this.btnReprint.Visible = false;
                            this.txtEmpID.ReadOnly = false;
                            this.LblReason.Visible = false;
                            this.RdlReason.Visible = false;
                            this.LblFromDate.Visible = false;
                            this.LblToDate.Visible = false;
                            this.txtToDate.Visible = false;
                            this.txtFromDate.Visible = false;
                            this.imgToDate.Visible = false;
                            this.LblAccess.Visible = false;
                            this.txtAccess.Visible = false;
                            this.ddAccessdetail.Visible = false;
                            this.DDAccessLabel.Visible = false;
                            this.accesstextid.Visible = false;
                            ////Added for 1 day access card PAN IND RollOut  
                            string[] onedayAccesscard = this.panIND.Split(',');
                            foreach (string strCountrychk in onedayAccesscard)
                            {
                                if (strCountrychk == this.strCountry)
                                {
                                    this.lbtnAccesscardreport.Visible = true;
                                }
                                else
                                {
                                    this.lbtnAccesscardreport.Visible = false;
                                }
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to populate city Details        
        /// </summary>
        private void FillControlValues()
        {
            try
            {
                DataTable dataLocation = new LocationBL().GetCityDetails();
                if (this.Session["LoginID"] != null)
                {
                    if (this.requestDetails != null)
                    {
                        this.securitydetails = this.requestDetails.GetSecurityCity(Convert.ToString(this.Session["LoginID"]));
                        this.cityName = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["City"]);
                        this.Session["SecurityCountry"] = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["CountryName"]);

                        this.facility = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["Facility"]);
                        this.locationId = Convert.ToString(this.securitydetails.Tables[0].Rows[0]["LocationId"]);
                        if (!string.IsNullOrEmpty(this.cityName))
                        {
                            ////ddlLocation.Items.FindByText(City).Selected = true;
                            ////ddlLocation.Enabled = false;
                            this.lblCardIssuedCityName.Text = this.cityName;
                            this.lblCardIssuedFacilityName.Text = this.facility;
                            this.hdnFacility.Value = this.locationId;
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
        /// Method to Show the Temporary ID Card        
        /// </summary>
        /// <param name="passNumber">Pass Number</param>
        private void GenerateIDCard(string passNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(passNumber))
                {
                    string strScript = string.Empty;
                    string templateId = XSS.HtmlEncode(this.hdnFileUploadID.Value);
                    this.securitydetails = this.requestDetails.GetSecurityCity(Convert.ToString(this.Session["LoginID"]));
                    //// string strAssociateID= Convert.ToString(securitydetails.Tables[0].Rows[0]["AssociateID"]);

                    string strAssociateID = string.IsNullOrEmpty(this.txtEmpID.Text) ? this.hdnAssociateID.Value : this.txtEmpID.Text.Trim();
                    DataSet dtset = new DataSet();
                    VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.RequestDetailsBL();
                    dtset = objaccess.GetAccessCardDetails(strAssociateID, this.hdnFacility.Value.ToString(), passNumber);
                    string msgboxdetails = dtset.Tables[1].Rows[0]["CardType"].ToString();
                    if (msgboxdetails == "1 Day ID Card" || msgboxdetails == "1 day ID Card and Access Card")
                    {
                        strScript = "<script language='javascript'>window.open('TemporaryIDCard.aspx?key=";
                    }
                    else if (msgboxdetails == "1 Day Access Card")
                    {
                        ////string AccessCardret = string.Concat(Resources.LocalizedText.AccessCardReturned, " ", txtEmpID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                        string accessCardret = string.Concat(Resources.LocalizedText.AccessCardGenerated, " ", XSS.HtmlEncode(this.txtEmpID.Text.ToString()), " ", Resources.LocalizedText.Under, " ", this.lblCardIssuedFacilityName.Text, " ", Resources.LocalizedText.Location);
                        string accessscript = string.Concat("<script>alert('", accessCardret, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", accessscript);
                    }

                    this.accesstextid.Text = string.Empty;
                    //// strScript = "<script language='javascript'>window.open('TemporaryIDCard.aspx?key=";
                    string encryptedData = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(passNumber)));
                    strScript = string.Concat(strScript, encryptedData);
                    string encryptTempId = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(templateId)));
                    strScript = string.Concat(strScript, " & Id=");
                    strScript = string.Concat(strScript, encryptTempId);
                    strScript = string.Concat(strScript, "', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ');</script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    return;
                }
                else
                {
                    try
                    {
                        Response.Redirect("ErrorPage.aspx", true);

                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {

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
        private DataSet MapTimeZoneToDataSet(DataSet griddata, int colNum)
        {
            if (griddata.Tables[0].Rows.Count > 0)
            {
                var count = 0;
                foreach (var x in griddata.Tables[0].Rows)
                {
                    if (griddata.Tables[0].Rows[count].ItemArray[colNum] != null)
                    {
                        var actualDate = griddata.Tables[0].Rows[count].ItemArray[colNum].ToString();
                        var offsetTZ = Session["TimezoneOffset"];
                        if (actualDate != null && actualDate != "" && actualDate != "{}")
                        {
                            var convertedDate = this.genTimeZone.ToLocalTimeZone(Convert.ToDateTime(actualDate), Convert.ToString(offsetTZ));

                            griddata.Tables[0].Rows[count][colNum] = convertedDate;
                        }
                    }
                    count++;
                }
            }
            return griddata;
        }

        /// <summary>
        /// Method to Bind data to Grid        
        /// </summary>
        private void BindData()
        {
            try
            {
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                string strgLocation = string.Empty;
                // DateTime Curdate = Convert.ToDateTime(HttpContext.Current.Session["currentDateTime"]);
                // string Cdate = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                //var Curdate = DateTime.ParseExact(Cdate, "MM/dd/yyyy HH:mm", null);
                string format;
                format = "dd/MM/yyyy HH:mm:ss";
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                string Curdate = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                var today = DateTime.ParseExact(Curdate, format, provider);
                Curdate = this.genTimeZone.GetCurrentDate().ToString();
                if (this.Session["LoginID"] != null)
                {
                    this.securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    if (this.securitydetails != null)
                    {
                        if (this.securitydetails.Tables[0].Rows.Count > 0)
                        {
                            strgLocation = this.securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                        }
                        else
                        {
                            string strScript = string.Concat("<script>alert('", Resources.LocalizedText.lblSecurityInValid, "'); </script>");
                            ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                        }
                    }
                    else
                    {
                        string strScript = string.Concat("<script>alert('", Resources.LocalizedText.lblSecurityInValid, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    }

                    DataSet griddata = objEmployeeDetails.GetODICardsIssued(strgLocation, this.txtEmpID.Text, today);
                    if (griddata.Tables[0].Rows.Count > 0)
                    {
                        //griddata = MapTimeZoneToDataSet(griddata, 5);
                        //griddata = MapTimeZoneToDataSet(griddata, 6);
                        griddata = MapTimeZoneToDataSet(griddata, 7);
                        this.pager.Visible = true;
                        this.lstvwResults.DataSource = griddata;
                        this.lstvwResults.DataBind();
                        ScriptManager.RegisterStartupScript(this.Upnl, this.Upnl.GetType(), "SetIVSLocalTime", "SetIVSLocalTime();", true);
                        this.pnlGrid.Visible = true;
                    }
                    else
                    {
                        this.lblMessage.Visible = true;
                        this.lblMessage.Text = Resources.LocalizedText.InValidAssociateId;
                        this.pnlGrid.Visible = false;
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
        /// Send Check out Notification mail
        /// </summary>
        /// <param name="associateID">Associate ID</param>
        /// <param name="associateName">Associate Name</param>
        /// <param name="issuedlocation">Issued Location area</param>
        /// <param name="location">Location area</param>
        /// <param name="accesstype">Access type</param>
        private void SendCheckoutNotificationMail(string associateID, string associateName, string issuedlocation, string location, string accesstype)
        {
            BusinessManager.MailNotification mailNotification = new BusinessManager.MailNotification();
            mailNotification.SendCheckoutNotification(associateID, associateName, issuedlocation, location, accesstype, Session["SecurityCountry"].ToString().Trim());
        }

        #endregion
    }
}
