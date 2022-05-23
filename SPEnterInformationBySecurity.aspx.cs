

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using VMSBusinessEntity;
    using VMSBusinessLayer;
    using VMSConstants;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// partial class to enter information by security
    /// </summary>
    public partial class SPEnterInformationBySecurity : System.Web.UI.Page
    {
        #region Variables

        /// <summary>
        /// visitor Proof object
        /// </summary>
        private VMSBusinessEntity.VisitorProof visitorProofObj = new VMSBusinessEntity.VisitorProof();

        /// <summary>
        /// visitor master object
        /// </summary>
        private VMSBusinessEntity.VisitorMaster visitorMasterObj = new VMSBusinessEntity.VisitorMaster();

        /// <summary>
        /// visitor local object
        /// </summary>
        private VMSBusinessEntity.VisitorRequest visitorLocObj = new VMSBusinessEntity.VisitorRequest();

        /// <summary>
        /// array of visit details
        /// </summary>
        private VMSBusinessEntity.VisitDetail[] arrayofVisitDetails = null;

        /// <summary>
        /// visitor equipment object
        /// </summary>
        private VMSBusinessEntity.VisitorEquipment[] visitorEquipmentObj = new VMSBusinessEntity.VisitorEquipment[5];

        /// <summary>
        /// visitor emergency contact object
        /// </summary>
        private VMSBusinessEntity.VisitorEmergencyContact visitorEmergencyContactObj = new VMSBusinessEntity.VisitorEmergencyContact();

        /// <summary>
        /// request details
        /// </summary>
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// validation object
        /// </summary>
        private VMSBusinessLayer.Validations validations = new VMSBusinessLayer.Validations();

        /// <summary>
        /// properties DC
        /// </summary>
        private VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();

        /// <summary>
        /// generic time zone
        /// </summary>
        private GenericTimeZone genTimeZone = new GenericTimeZone();

        /// <summary>
        /// success submission
        /// </summary>
        private int? successSubmission = 0;

#pragma warning disable CS0414 // The field 'SPEnterInformationBySecurity.batchNo1' is assigned but its value is never used
        /// <summary>
        /// batch No 1
        /// </summary>
        private int batchNo1 = 0;
#pragma warning restore CS0414 // The field 'SPEnterInformationBySecurity.batchNo1' is assigned but its value is never used

        /// <summary>
        /// visit status
        /// </summary>
        private string visitstatus;

        /// <summary>
        /// save click
        /// </summary>
        private bool saveClick = false;

        /// <summary>
        /// submit click
        /// </summary>
        private bool submitClick = false;

        /// <summary>
        /// reset click
        /// </summary>
        private bool resetclick = false;

#pragma warning disable CS0414 // The field 'SPEnterInformationBySecurity.badgegenerate' is assigned but its value is never used
        /// <summary>
        /// badge generate
        /// </summary>
        private bool badgegenerate = false;
#pragma warning restore CS0414 // The field 'SPEnterInformationBySecurity.badgegenerate' is assigned but its value is never used

        /// <summary>
        /// generate badge click
        /// </summary>
        private bool generatebadgeclick = false;

#pragma warning disable CS0414 // The field 'SPEnterInformationBySecurity.count' is assigned but its value is never used
        /// <summary>
        /// count value
        /// </summary>
        private int count = 0;
#pragma warning restore CS0414 // The field 'SPEnterInformationBySecurity.count' is assigned but its value is never used
        ////string strHostId;
        #endregion

        /// <summary>
        /// Assign time zone offset
        /// </summary>
        /// <param name="strTimezoneoffset">time zone offset value</param>
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
        /// Assign Current date time
        /// </summary>
        /// <param name="currentDate">current date value</param>
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
        /// Submit data
        /// </summary>
        public void SubmitData()
        {
            string strErrorMessage = string.Empty;
            this.Session["SaveFlag"] = true;
            try
            {
                IdentityDetails identityDetails = this.GetIdentityDetailsForArgentina();

                string dtfromdate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromDate")).Text).ToString("MM/dd/yyyy");
                string dttoDate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToDate")).Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromdate.Split('/');
                string fromTime = ((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromTime")).Text;
                string totime = ((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToTime")).Text;
                DateTime fromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                DateTime todate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + totime), Convert.ToString(Session["TimezoneOffset"]));
                DateTime today = this.genTimeZone.GetCurrentDate();
                TimeSpan todateSpan = todate - today;
                TimeSpan fromDateSpan = fromDate - today;
                if (Convert.ToInt32(((HiddenField)this.VisitorLocationInformationControlSP.FindControl("AdvanceAllowabledays")).Value) > todateSpan.Days + 1 && Convert.ToInt32(((HiddenField)this.VisitorLocationInformationControlSP.FindControl("AdvanceAllowabledays")).Value) > fromDateSpan.Days + 1)
                {
                    if (this.Submit.Text == Resources.LocalizedText.Submit)
                    {
                        this.lblError.Visible = false;
                        this.lblError1.Visible = false;
                        this.lblError2.Visible = false;
                        this.lblError3.Visible = false;
                        this.lblError4.Visible = false;
                        this.lblSubmitSuccess.Visible = false;
                        this.lblSubmitSuccess.Text = string.Empty;

                        if (!Session["SaveFlag"].Equals(true))
                        {
                            this.visitorProofObj = this.VisitorGeneralInformationControlSP.InsertPhoto();
                            this.visitorMasterObj = this.VisitorGeneralInformationControlSP.InsertGeneralInformation();
                            this.visitorLocObj = this.VisitorLocationInformationControlSP.InsertLocationInformation();
                            this.arrayofVisitDetails = this.VisitorLocationInformationControlSP.GetVisitDetails();

                            if (this.generatebadgeclick == true)
                            {
                                if (this.visitorLocObj.FromTime > this.visitorLocObj.ToTime)
                                {
                                    this.visitorLocObj.ToDate = this.visitorLocObj.ToDate.Value.AddDays(-1);

                                    TextBox txtToDate = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToDate");

                                    txtToDate.Text = this.visitorLocObj.ToDate.Value.ToString("dd/MM/yyyy");
                                }
                            }

                            if (this.visitorLocObj.Facility.Split('|')[0].ToString().Equals("Select"))
                            {
                                this.visitorLocObj.Facility = this.visitorLocObj.Facility + "|" + "Security";
                            }

                            ////end
                            this.visitorLocObj.VisitorID = this.visitorMasterObj.VisitorID;
                            this.visitorLocObj.Status = "Submitted";
                            this.visitorEquipmentObj = this.EquipmentPermittedControlSP.InsertEquipmentInformation(Convert.ToInt32(Request.QueryString["RequestID"]));
                            this.visitorEmergencyContactObj = this.EmergencyContactInformationControlSP.InsertEmergencyContactInformation();
                            this.visitorEmergencyContactObj.RequestID = this.visitorLocObj.RequestID;
                            this.successSubmission = this.requestDetails.SubmitVisitorInformation(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, identityDetails);
                            this.Session["RequestID"] = this.visitorLocObj.RequestID;
                        }
                        else
                        {
                            ////12/9/09
                            if (this.Session["Status"] != null)
                            {
                                if (Session["Status"].ToString().Equals("Saved"))
                                {
                                    this.SaveVisitorInformation("Submitted");
                                }
                                ////14/09/09
                                if (((Session["Status"].ToString().Equals("Submitted") && (this.submitClick == true)) && (this.Submit.Text == Resources.LocalizedText.Submit)) && ((bool)Session["SaveClick"] == true))
                                {
                                    this.Session["Status"] = null;
                                    this.SaveVisitorInformation("Submitted");
                                }
                            }

                            if ((this.Session["Status"] == null) && (this.submitClick == true))
                            {
                                this.SaveVisitorInformation("Submitted");
                            }

                            ////end
                            this.visitorMasterObj = (VMSBusinessEntity.VisitorMaster)HttpContext.Current.Session["VisitorMasterObj"];
                            this.visitorProofObj = (VMSBusinessEntity.VisitorProof)HttpContext.Current.Session["VisitorProofObj"];
                            this.visitorLocObj = (VMSBusinessEntity.VisitorRequest)HttpContext.Current.Session["VisitorLocObj"];
                            this.visitorEmergencyContactObj = (VMSBusinessEntity.VisitorEmergencyContact)HttpContext.Current.Session["VisitorEmergencyContactObj"];
                            this.visitorEquipmentObj = (VMSBusinessEntity.VisitorEquipment[])HttpContext.Current.Session["VisitorEquipmentObj"];
                            ////Changes done for VMS CR VMS06072010CR09 by Priti
                            strErrorMessage = this.RequestsPageValidations(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.visitorEmergencyContactObj, this.visitorEquipmentObj);
                            this.Session["SaveFlag"] = false;

                            string[] strLogicErr = strErrorMessage.Split('%');
                            ////Changes done for VMS CR VMS06072010CR09 by Priti
                            if (!string.IsNullOrEmpty(strLogicErr[0]) || !string.IsNullOrEmpty(strLogicErr[1]) || !string.IsNullOrEmpty(strLogicErr[2]) || !string.IsNullOrEmpty(strLogicErr[3]))
                            {
                                throw new VMSBL.CustomException(strErrorMessage);
                            }
                            else
                            {
                                ////12/9/09
                                if (this.Session["Status"] != null)
                                {
                                    if (this.Session["Status"].ToString().Equals("Saved") && (this.submitClick == true))
                                    {
                                        this.UpdateVisitorInformation();
                                    }
                                    else if (((this.Session["Status"].ToString().Equals("Submitted") && (this.submitClick == true)) && (this.Submit.Text == Resources.LocalizedText.Submit)) && ((bool)Session["SaveClick"] == true))
                                    {
                                        this.UpdateVisitorInformation();
                                    }
                                }
                                else if ((this.Session["Status"] == null) && (this.submitClick == true) && ((bool)this.Session["SaveClick"] == false))
                                {
                                    this.successSubmission = this.requestDetails.SubmitVisitorInformation(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, identityDetails);
                                    if (this.successSubmission == 0)
                                    {
                                        this.Session["RequestID"] = this.visitorLocObj.RequestID;
                                        this.Session["VisitorID"] = this.visitorMasterObj.VisitorID;
                                        VMSBusinessEntity.VisitDetail visitDetail1 = this.arrayofVisitDetails[0];
                                        this.hdnVisitDetailsID.Value = visitDetail1.VisitDetailsID.ToString();
                                    }
                                }
                            }
                        }

                        if (this.successSubmission.Equals(1))
                        {
                            string strMessage = Resources.LocalizedText.DuplicateVisitor;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strMessage + "'); </script>");
                            this.lblSubmitSuccess.Text = strMessage;
                            TextBox txtFromTime = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromTime");
                            DateTime fromtime = Convert.ToDateTime(txtFromTime.Text.ToString());
                            if ((DateTime.Compare(this.visitorLocObj.FromDate.Value, DateTime.Today) <= 0) &&
                                (DateTime.Compare(this.visitorLocObj.ToDate.Value, DateTime.Today) >= 0) &&
                                this.visitstatus != "IN" && this.visitstatus != "CANCELLED" && (this.visitstatus != "YET TO ARRIVE"
                                || this.visitstatus != VMSConstants.REPEATVISITOR) &&
                                Convert.ToDateTime(System.DateTime.Now.ToString("H:mm")) <= fromtime.AddMinutes(10))
                            {
                                this.GenerateBadge.Enabled = true;
                            }
                            else
                            {
                                this.GenerateBadge.Enabled = false;
                            }
                        }
                        else
                        {
                            string strMessage = Resources.LocalizedText.SubmittedSuccessfully;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strMessage + "'); </script>");

                            ////  Page.RegisterClientScriptBlock("script", "<script language='javascript'> alert(\"Submitted Successfully\"); </script>");
                            this.lblSubmitSuccess.Text = strMessage;
                            ////this.lblSubmitSuccess.Visible = true;
                            this.GenerateBadge.Enabled = true;
                            TextBox txtFromTime = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromTime");
                            DateTime fromtime = Convert.ToDateTime(txtFromTime.Text.ToString());
                            var dtcurrentDate = this.genTimeZone.GetCurrentDate().ToShortDateString();
                            DateTime dttoday = DateTime.Parse(dtcurrentDate);
                            if ((DateTime.Compare(this.visitorLocObj.FromDate.Value, dttoday) <= 0) &
                                (DateTime.Compare(this.visitorLocObj.ToDate.Value, dttoday) >= 0) &&
                                this.visitstatus != "IN" && this.visitstatus != "CANCELLED" && this.visitstatus != "OUT"
                                && (this.visitstatus != "YET TO ARRIVE" ||
                                this.visitstatus != VMSConstants.REPEATVISITOR) &&
                            Convert.ToDateTime(dttoday.ToString("H:mm")) <= fromtime.AddMinutes(10))
                            {
                                this.GenerateBadge.Enabled = true;
                            }
                            else
                            {
                                this.GenerateBadge.Enabled = false;
                            }

                            this.Save.Enabled = false;
                            this.Submit.Enabled = false;
                            if (!Request.QueryString.ToString().Contains("RequestID="))
                            {
                                //// for submit action  Isreset parameter is false updated for CR IRVMS22062010CR07
                                if (((RadioButtonList)this.VisitorGeneralInformationControlSP.FindControl("multiplereqRadio")).SelectedValue == "1")
                                {
                                    this.ResetVisitorInformation(false);
                                    this.GenerateBadge.Enabled = false;
                                    this.Session["RequestID"] = null;
                                }
                            }
                        }
                    }
                    else if (this.Submit.Text == Resources.LocalizedText.Update)
                    {
                        if (Request.QueryString["details"] != null)
                        {
                            if (Request.QueryString.ToString().Contains("details="))
                            {
                                string detailsId = Request.QueryString["details"];
                                DataSet dt = this.requestDetails.Badgereturnvalues(detailsId);
                                string reqId = dt.Tables[0].Rows[0]["RequestId"].ToString().ToUpper();
                                string status = dt.Tables[0].Rows[0]["RequestStatus"].ToString().ToUpper();
                                if (status == "IN")
                                {
                                    HiddenField txtFromTime = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnFromTime");
                                    TextBox txtToTime = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToTime");
                                    HiddenField txtToDate = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnToDate");
                                    HiddenField txtFromDate = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnFromDate");

                                    string[] todates = txtToDate.Value.Split('/');
                                    string intime = dt.Tables[0].Rows[0]["inTime"].ToString();
                                    string[] startFromDate = txtFromDate.Value.Split('/');
                                    ////dt.Tables[0].Rows[0]["FromDate"].ToString().Split('/');
                                    DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(startFromDate[0] + "/" + startFromDate[1] + "/" + startFromDate[2] + " " + txtFromTime.Value), Convert.ToString(Session["TimezoneOffset"]));
                                    ////DateTime StartDate = Convert.ToDateTime(StartFromDate[1] + "/" + StartFromDate[0] + "/" + StartFromDate[2] + " " + txtFromTime.Text);
                                    ////DateTime EndDate = Convert.ToDateTime(Todate[1] + "/" + Todate[0] + "/" + Todate[2] + " " + txtToTime.Text);
                                    DateTime endDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todates[0] + "/" + todates[1] + "/" + todates[2] + " " + txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                                    string timeStart = startDate.ToString("HH:mm");
                                    TimeSpan fromTimespan = TimeSpan.Parse(timeStart);
                                    string timeEnd = endDate.ToString("HH:mm");
                                    TimeSpan totimespan = TimeSpan.Parse(timeEnd);
                                    string strtotime = totime.ToString();
                                    bool notvalid = this.validations.CheckTime(startDate, endDate);
                                    ////test by bincey
                                    if (notvalid == false)
                                    {
                                        this.lblError.Visible = true;
                                        ////  custError.Text = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.TIMEERROR].ToString();
                                        this.lblError.Text = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.TIMEERROR].ToString();
                                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetTimeAfterValidations", "GetTimeAfterValidations();", true);
                                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetEmergencyTimeAfterValidations", "GetEmergencyTimeAfterValidations();", true);
                                    }
                                    else
                                    {
                                        this.lblError.Visible = false;
                                        this.lblError1.Visible = false;
                                        this.lblError2.Visible = false;
                                        this.lblError3.Visible = false;
                                        this.lblError4.Visible = false;
                                        ////  Extendthetime(reqId, strtotime);
                                        //// Reset_Click(null, null);
                                        this.lblSubmitSuccess.Visible = false;
                                        this.lblSubmitSuccess.Text = VMSConstants.TIMEEXTENSIONSUCCESS.ToString();
                                        var dateTime = new DateTime(totimespan.Ticks); // Date part is 01-01-0001
                                        var formattedTime = dateTime.ToString("h:mm", CultureInfo.InvariantCulture);

                                        if (totimespan > TimeSpan.Parse(dt.Tables[0].Rows[0].ItemArray[9].ToString()))
                                        {
                                            this.Extendthetime(reqId, strtotime);
                                            string strMessage = Resources.LocalizedText.TimeExtend;
                                            Page.ClientScript.RegisterClientScriptBlock(Type.GetType("System.String"), "script", "<script language='javascript'> alert('" + strMessage + "'); </script>");
                                            this.Session["MailExtendTrigger"] = "true";
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetVisitTimeExtend", "GetExtendedTime();", true);
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "TimeExtendMailtrigger", "TimeExtendMailtrigger();", true);
                                            //// RequestDetails.Timeextendedmailtohost(dt, strtotime);
                                            this.Submit.Enabled = false;
                                        }
                                        else
                                        {
                                            this.Extendthetime(reqId, strtotime);
                                            string strMessage = "Time updated successfully.";
                                            Page.ClientScript.RegisterClientScriptBlock(Type.GetType("System.String"), "script", "<script language='javascript'> alert('" + strMessage + "'); </script>");
                                            this.Session["MailExtendTrigger"] = "true";
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetVisitTimeExtend", "GetExtendedTime();", true);
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "TimeExtendMailtrigger", "TimeExtendMailtrigger();", true);
                                            //// RequestDetails.Timeextendedmailtohost(dt, strtotime);
                                            this.Submit.Enabled = false;
                                        }
                                    }
                                }
                                else
                                {
                                    this.UpdateVisitorInformation();
                                }
                            }
                        }
                        else
                        {
                            this.UpdateVisitorInformation();
                        }
                    }
                }
                else
                {
                    this.lblError2.Visible = true;
                    this.lblError2.Text = Resources.LocalizedText.RequestallowedCheck + ((HiddenField)this.VisitorLocationInformationControlSP.FindControl("AdvanceAllowabledays")).Value + Resources.LocalizedText.DayInAdvance;
                }
            }
            catch (VMSBL.CustomException ex)
            {
                this.DisplayError(ex.ErrorMessage, true);
                return;
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// Request page validation
        /// </summary>
        /// <param name="objvisitorProof">visitor proof object</param>
        /// <param name="visitorMaster">visitor master</param>
        /// <param name="visitorRequest">visitor request</param>
        /// <param name="visitorEmergencyContact">visitor emergency contact</param>
        /// <param name="visitorEquipment">visitor equipment</param>
        /// <returns>string value</returns>
        public string RequestsPageValidations(VisitorProof objvisitorProof, VisitorMaster visitorMaster, VisitorRequest visitorRequest, VisitorEmergencyContact visitorEmergencyContact, VisitorEquipment[] visitorEquipment)
        {
            StringBuilder strError = new StringBuilder();
            StringBuilder strGeneralError = new StringBuilder();
            StringBuilder strReqError = new StringBuilder();
            StringBuilder strContactinfoError = new StringBuilder();
            StringBuilder strEquipmentError = new StringBuilder();
            try
            {
                //// string title = VisitorMaster.Title;
                string firstName = visitorMaster.FirstName;
                string lastName = visitorMaster.LastName;
                string company = visitorMaster.Company;
                string emailID = visitorMaster.EmailID;
                string nativeCountry = visitorMaster.Country;
                string mobileNumber = visitorMaster.MobileNo;

                string purpose = visitorRequest.Purpose;
                DateTime fromDate = visitorRequest.FromDate.Value;
                DateTime todate = visitorRequest.ToDate.Value;
                TimeSpan fromTime = visitorRequest.FromTime.Value;
                TimeSpan totime = visitorRequest.ToTime.Value;

                DateTime visitingFromDate = (DateTime)visitorEmergencyContact.FromDate;
                DateTime visitingToDate = (DateTime)visitorEmergencyContact.ToDate;

                if (this.CheckEquipment(visitorEquipment))
                {
                    strEquipmentError.Append(VMSConstants.BRMESSAGE);
                    strEquipmentError.AppendLine(VMSConstants.EQUIPMENTDUPLICATIONERROR.ToString());
                }

                strError = strGeneralError.Append("%").Append(strReqError).Append("%").Append(strContactinfoError).Append("%").Append(strEquipmentError);
                return strError.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// check drop down value
        /// </summary>
        /// <param name="ddlValue">DDL value</param>
        /// <returns>string value</returns>
        public bool CheckDropDownValue(string ddlValue)
        {
            bool hasNotSelected = false;

            ////Modified the if statement on Aug 18 2009 - start
            ////if (ddlValue.Equals("Select") || ddlValue.Equals("Select Purpose") || ddlValue.Equals("4"))
            if (ddlValue.Equals("Select") || ddlValue.Equals("Select Purpose") || ddlValue.Equals("9") || string.IsNullOrEmpty(ddlValue))
            {
                ////end                
                hasNotSelected = true;
            }
            else
            {
                hasNotSelected = false;
            }

            return hasNotSelected;
        }

        /// <summary>
        /// Check equipment
        /// </summary>
        /// <param name="visitorEquipment">visitor equipment</param>
        /// <returns>string value</returns>
        public bool CheckEquipment(VisitorEquipment[] visitorEquipment)
        {
            DataSet ds = new DataSet();
            bool isDuplicateEquipment = false;
            for (int rowCount = 0; rowCount < visitorEquipment.Count(); rowCount++)
            {
                int rowCountForCheck = visitorEquipment.Count() - 1;
                while (rowCountForCheck > rowCount)
                {
                    if (visitorEquipment[rowCount].MasterDataID == visitorEquipment[rowCountForCheck].MasterDataID)
                    {
                        if (visitorEquipment[rowCount].Make.Equals(visitorEquipment[rowCountForCheck].Make) && visitorEquipment[rowCount].SerialNo.Equals(visitorEquipment[rowCountForCheck].SerialNo))
                        {
                            isDuplicateEquipment = true;
                        }
                    }

                    rowCountForCheck--;
                }
            }

            return isDuplicateEquipment;
        }

        /// <summary>
        /// Check time with current time
        /// </summary>
        /// <param name="fromTime">from time value</param>
        /// <param name="totime">to time value</param>
        /// <param name="fromDate">from date</param>
        /// <param name="todate">to date</param>
        /// <returns>string value</returns>
        public bool CheckTimewithCurrentTime(TimeSpan fromTime, TimeSpan totime, DateTime fromDate, DateTime todate)
        {
            bool isnotMatch = false;
            GenericTimeZone genrTimeZone = new GenericTimeZone();
            ////24/09/2009
            DateTime currentdate = genrTimeZone.GetCurrentDate();
            DateTime dt = currentdate.AddMinutes(-10);
            ////end
            DateTime todaysDate = dt.Date;
            TimeSpan todaysTime = dt.TimeOfDay;
            ////    TimeSpan tsFrom = TimeSpan.Parse(VMSUtility.VMSUtility.GetTimeToISTZone(fromTime.ToString()));
            if (fromDate == todaysDate)
            {
                if (fromTime >= todaysTime && fromDate == todaysDate)
                {
                    isnotMatch = false;
                }
                else
                {
                    isnotMatch = true;
                }
            }

            return isnotMatch;
        }

        /// <summary>
        /// Check time with to time and from time
        /// </summary>
        /// <param name="fromTime">from time</param>
        /// <param name="totime">to time</param>
        /// <param name="fromDate">from date</param>
        /// <param name="todate">to date</param>
        /// <returns>boolean value</returns>
        public bool CheckTimewithToTimeAndFromTime(TimeSpan fromTime, TimeSpan totime, DateTime fromDate, DateTime todate)
        {
            bool isnotMatch = false;
            GenericTimeZone genrTimeZone = new GenericTimeZone();

            if (fromDate == todate)
            {
                if (fromTime <= totime && fromDate == todate)
                {
                    isnotMatch = false;
                }
                else
                {
                    isnotMatch = true;
                }
            }

            return isnotMatch;
        }

        /// <summary>
        /// extend the time
        /// </summary>
        /// <param name="reqid">request Id</param>
        /// <param name="totime">to time</param>
        public void Extendthetime(string reqid, string totime)
        {
            this.requestDetails.Extendthetime(reqid, totime);
        }

        /// <summary>
        /// reset data
        /// </summary>
        public void ResetData()
        {
            this.resetclick = true;
            this.Session["VisitorImgByte"] = null;
            this.Session["Webcamimage"] = null;
            if (Request.QueryString.ToString().Contains("RequestID="))
            {
                this.GetVisitorInformationDetailsbyRequestID();
            }
            else
            {
                this.Session["Status"] = null;
                this.Session["SaveFlag"] = true;
                this.Session["RequestID"] = null;
                this.ResetVisitorInformation(true);
            }

            if (this.Session["Status"] != null)
            {
                if (this.Session["Status"].ToString().Equals("Submitted") || this.Session["Status"].ToString().Equals("Updated"))
                {
                    this.Save.Enabled = false;
                }
                else
                {
                    this.Save.Enabled = true;
                }
            }
            else
            {
                this.Save.Enabled = true;
            }
        }

        /// <summary>
        /// show equipment control
        /// </summary>
        public void ShowEquipmentControl()
        {
            try
            {
                string hostid = ((HtmlInputText)this.VisitorLocationInformationControlSP.FindControl("txtHost")).Value;
                if (!string.IsNullOrEmpty(hostid))
                {
                    int startIndex = hostid.IndexOf("(") + 1;
                    string visitorType = ((DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlPurpose")).SelectedItem.Value;
                    bool permitEquipments = ((CheckBox)this.VisitorLocationInformationControlSP.FindControl("hdnPermitEquipments")).Checked;
                    if (permitEquipments && (visitorType.ToUpper().Equals(VMSConstants.VISITORTYPECLIENTS) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEVENDOR) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEVIP) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEGUESTS) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEAUDITORS)))
                    {
                        this.hrEquipmentControl.Visible = true;
                        this.EquipmentPermittedControlSP.Visible = true;
                    }
                    else
                    {
                        this.hrEquipmentControl.Visible = false;
                        this.EquipmentPermittedControlSP.Visible = false;
                        this.EquipmentPermittedControlSP.ResetEquipmentInformation(false);
                    }
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        #region show Equipment user controls
        /// <summary>
        /// show equipment permitted control
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void ShowEquipmentPermittedControl(object sender, EventArgs e)
        {
            this.ShowEquipmentControl();
        }

        #endregion
        /// <summary>
        /// page load function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int visitorID = Convert.ToInt32(Request.QueryString["VisitorID"]);
                if (this.lblSubmitSuccess.Text.Length > 0)
                {
                    this.lblSubmitSuccess.Text = string.Empty;
                    this.lblSubmitSuccess.Visible = false;
                }

                Ajax.Utility.RegisterTypeForAjax(typeof(VMSEnterInformationBySecurity));
                //// Ajax.Utility.RegisterTypeForAjax(typeof(VMSEnterInformationBySecurity));
                if (!Page.IsPostBack)
                {
                    if (this.Session["LoginID"] == null)
                    {
                        return;
                    }

                    if (this.Session["TimezoneOffset"] == null)
                    {
                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffsetTime", "GetOffsetTime();", true);
                    }

                    this.Session["SaveFlag"] = false;
                    this.Session["SaveClick"] = false;
                    this.Session["Webcamimage"] = null;
                    this.Session["VisitorImgByte"] = null;
                    Image imgPhoto = (Image)this.VisitorGeneralInformationControlSP.FindControl("imgphoto");
                    imgPhoto.Visible = true;
                    Label lblRequiredFacility = (Label)this.VisitorLocationInformationControlSP.FindControl("lblRequiredFacility");
                    lblRequiredFacility.Visible = true;
                    this.Session["SaveFlag"] = false;
                    this.lblError.Visible = false;
                    this.lblError1.Visible = false;
                    this.lblError2.Visible = false;
                    this.lblError3.Visible = false;
                    this.lblError4.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// Page load complete value
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                if (this.Session["Generated"] != null)
                {
                    if (this.Session["Generated"].ToString().Equals("True"))
                    {
                        this.ResetVisitorInformation(false);
                    }
                }

                if (!Page.IsPostBack)
                {
                    ////12/9/09
                    this.Session["Status"] = null;
                    this.Session["RequestID"] = null;
                    this.Session["Requeststatus"] = null;
                    ////end
                    this.Session["SaveClick"] = false;
                    this.Session["RefID"] = null;
                    if (this.Session["LoginID"] == null)
                    {
                        return;
                    }

                    DropDownList ddlFacility = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlFacility");
                    this.DisableButtons(string.Empty);
                    if (Request.QueryString.ToString().Contains("details="))
                    {
                        this.Save.Enabled = false;
                        ////24/09/2009
                        this.Save.Visible = false;
                        ////Search.Enabled = false;
                        this.btnIdProof.Enabled = true;
                        this.btnUpload.Enabled = true;
                        this.Submit.Enabled = true;
                        this.GetVisitorInformationDetailsbyRequestID();
                        DropDownList ddlOtherPurpose = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlPurpose");
                        TextBox txtOthers = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtPurpose");
                        if (txtOthers.Visible == true)
                        {
                            ddlOtherPurpose.SelectedItem.Value = "Others";
                        }

                        if (this.Session["Status"] != null)
                        {
                            ////12/9/09
                            if (Session["Status"].ToString().Equals("Saved"))
                            {
                                this.Submit.Text = Resources.LocalizedText.Submit;
                                this.Cancel.Enabled = false;
                                this.Save.Enabled = true;
                            }
                            else if (Session["Status"].ToString().Equals("Submitted") || Session["Status"].ToString().Equals("Updated"))
                            {
                                this.Submit.Text = Resources.LocalizedText.Update;
                                this.Cancel.Enabled = true;
                                ////test by bincey
                                this.Submit.Enabled = true;
                            }

                            if (Session["Status"].ToString().Equals("Cancelled"))
                            {
                                this.Submit.Enabled = false;
                                this.Save.Enabled = false;
                                this.Cancel.Enabled = false;
                            }
                        }

                        this.Session["SaveFlag"] = true;
                        HiddenField hdnDate = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnDate");
                        string[] todate1 = ((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToDate")).Text.Split('/');
                        //// DateTime toDate = Convert.ToDateTime(toDate1[2] + "-" + toDate1[1] + "-" + toDate1[0]);
                        string[] fromDate1 = ((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromDate")).Text.Split('/');
                        if (!string.IsNullOrEmpty(fromDate1[0])
                            || !string.IsNullOrEmpty(todate1[0]))
                        {
                            string fromTime = ((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromTime")).Text;
                            string totime = ((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToTime")).Text;
                            ////    DateTime FromDate = Convert.ToDateTime(FromDate1[2] + "-" + FromDate1[1] + "-" + FromDate1[0]);
                            DateTime fromDate = Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + fromTime);
                            DateTime todate = Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + totime);
                            DateTime today = this.genTimeZone.GetCurrentDate();
                            DateTime dt = Convert.ToDateTime(fromDate.ToShortDateString());

                            if ((DateTime.Compare(
                            Convert.ToDateTime(fromDate.ToShortDateString()),
                                Convert.ToDateTime(today.ToShortDateString())) <= 0) &
                                (DateTime.Compare(
                                Convert.ToDateTime(todate.ToShortDateString()),
                               Convert.ToDateTime(today.ToShortDateString())) >= 0) &&
                               this.visitstatus != "IN" && this.visitstatus != "CANCELLED" &&
                               this.visitstatus != "OUT" && (this.visitstatus == "YET TO ARRIVE" ||
                               this.visitstatus == VMSConstants.REPEATVISITOR) &&
                            Convert.ToDateTime(today.ToString("H:mm")) <= fromDate.AddMinutes(10))
                            {
                                this.GenerateBadge.Enabled = true;
                            }
                            else
                            {
                                this.GenerateBadge.Enabled = false;
                            }
                        }

                        HiddenField hdnHiddenField = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnBadgeStatus");
                        if (hdnHiddenField != null)
                        {
                            if ((hdnHiddenField.Value == "Returned") || (hdnHiddenField.Value == "Lost"))
                            {
                                this.GenerateBadge.Enabled = false;
                                this.btnIdProof.Enabled = false;
                                ////test by bincey
                                this.btnUpload.Enabled = false;
                                this.Save.Enabled = false;
                                this.Cancel.Enabled = false;
                                this.Reset.Enabled = false;
                                this.Submit.Enabled = false;
                            }
                            else if (hdnHiddenField.Value.ToUpper() == "ISSUED")
                            {
                                this.GenerateBadge.Enabled = false;
                                this.btnIdProof.Enabled = false;
                                ////test by bincey
                                this.btnUpload.Enabled = false;
                                this.Save.Enabled = false;
                                this.Cancel.Enabled = false;
                                this.Reset.Enabled = false;
                                this.Submit.Enabled = true;
                            }
                            else if (string.IsNullOrEmpty(hdnHiddenField.Value))
                            {
                                this.GenerateBadge.Enabled = true;
                                this.btnIdProof.Enabled = false;
                                this.btnUpload.Enabled = true;
                                this.Save.Enabled = true;
                                this.Cancel.Enabled = false;
                                this.Reset.Enabled = false;
                                TextBox txtFromDate = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromDate");
                                TextBox txtToDate = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToDate");
                                ImageButton imgBtnHost = (ImageButton)this.VisitorLocationInformationControlSP.FindControl("imgbutHost");
                                DropDownList ddlPurpose = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlPurpose");
                                TextBox txtVisitingFromDate = (TextBox)this.EmergencyContactInformationControlSP.FindControl("txtVisitingFromDate");
                                TextBox txtVisitingToDate = (TextBox)this.EmergencyContactInformationControlSP.FindControl("txtVisitingToDate");
                                TextBox txtAddress = (TextBox)this.EmergencyContactInformationControlSP.FindControl("txtAddress");
                                TextBox txtFirstName = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtFirstName");
                                TextBox txtLastName = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtLastName");
                                DropDownList ddlGender = (DropDownList)this.VisitorGeneralInformationControlSP.FindControl("ddlGender");
                                TextBox txtCompany = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtCompany");
                                TextBox txtDesignation = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtDesignation");
                                TextBox txtEmail = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtEmail");
                                DropDownList ddlNativeCountry = (DropDownList)this.VisitorGeneralInformationControlSP.FindControl("ddlNativeCountry");
                                TextBox txtMobileNo = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtMobileNo");
                                TextBox txtCountryCode = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtCountryCode");
                                CheckBox chkIsCofidential = (CheckBox)this.VisitorGeneralInformationControlSP.FindControl("chkIsCofidential");
                                CheckBox chkMultipleEntry = (CheckBox)this.VisitorGeneralInformationControlSP.FindControl("chkMultipleEntry");
                                ImageButton imgFromDate = (ImageButton)this.VisitorLocationInformationControlSP.FindControl("imgFromDate");
                                ImageButton imgToDate = (ImageButton)this.VisitorLocationInformationControlSP.FindControl("imgToDate");
                                ImageButton imgfromdate = (ImageButton)this.EmergencyContactInformationControlSP.FindControl("imgFromDate");
                                ImageButton imgtodate = (ImageButton)this.EmergencyContactInformationControlSP.FindControl("imgToDate");
                                HtmlControl txtHost = (HtmlControl)this.VisitorLocationInformationControlSP.FindControl("txtHost");
                                TextBox txtHostContactNo = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtHostContactNo");
                                txtFromDate.Enabled = false;
                                txtToDate.Enabled = false;
                                imgBtnHost.Enabled = false;
                                ddlPurpose.Enabled = false;
                                txtVisitingFromDate.Enabled = false;
                                txtVisitingToDate.Enabled = false;
                                txtAddress.Enabled = false;
                                txtFirstName.Enabled = false;
                                txtLastName.Enabled = false;
                                ddlGender.SelectedItem.Text = "Select";
                                ddlGender.Enabled = false;
                                txtCompany.Enabled = false;
                                txtDesignation.Enabled = false;
                                txtEmail.Enabled = false;
                                ddlNativeCountry.Enabled = false;
                                txtMobileNo.Enabled = false;
                                txtCountryCode.Enabled = false;
                                chkIsCofidential.Enabled = false;
                                chkMultipleEntry.Enabled = false;
                                imgFromDate.Enabled = false;
                                imgToDate.Enabled = false;
                                imgfromdate.Enabled = false;
                                imgtodate.Enabled = false;
                                txtHost.Disabled = true;
                                txtHostContactNo.Enabled = false;

                                ////string FromTime = (((TextBox)VisitorLocationInformationControl.FindControl("txtFromTime")).Text);
                            }
                        }

                        this.ShowEquipmentControl();
                    }
                    else
                    {
                        ////Search.Enabled = true;
                        this.btnIdProof.Enabled = true;
                        this.btnUpload.Enabled = true;
                        this.Submit.Enabled = false;
                        this.Save.Enabled = true;
                        this.Submit.Text = Resources.LocalizedText.Submit;
                        ////12/9/09
                        this.Cancel.Enabled = false;
                        this.Submit.Enabled = true;
                        this.Session["SaveFlag"] = true;
                        ////end
                        this.Session["RequestStatus"] = null;
                        this.Session["Status"] = null;
                    }

                    if (Request.QueryString.ToString().Contains("VisitorID="))
                    {
                        this.GetVisitorInformationDetailsbyVisitorID();
                    }

                    DropDownList ddlCountry = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlCountry");
                    ddlCountry.Enabled = false;
                    DropDownList ddlCity = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlCity");
                    ddlCity.Enabled = false;
                    ddlFacility.Enabled = false;
                    DropDownList drpGender = (DropDownList)this.VisitorGeneralInformationControlSP.FindControl("ddlGender");
                    drpGender.SelectedItem.Text = "Select";
                    drpGender.Enabled = false;
                    TextBox mobileNo = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtMobileNo");
                    TextBox countryCode = (TextBox)this.VisitorGeneralInformationControlSP.FindControl("txtCountryCode");
                    mobileNo.Enabled = false;
                    countryCode.Enabled = false;
                    this.ShowEquipmentControl();
                }

                if (this.Session["HostChanged"] != null)
                {
                    if (Session["HostChanged"].ToString().ToUpper().Equals("TRUE"))
                    {
                        this.ShowEquipmentControl();
                        this.Session["HostChanged"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// get security city
        /// </summary>
        /// <returns>returns security Id</returns>
        protected DataSet GetSecurityCity()
        {
            ////string city;
            string securityID = Session["LoginID"].ToString();

            return this.requestDetails.GetSecurityCity(securityID);
            ////return city;
        }

        /// <summary>
        /// Submitting the Enter Information Page Details.
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Submit_Click(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                this.submitClick = true;
                this.SubmitData();
                this.btnUpload.Enabled = false;
            }
        }

        /// <summary>
        /// Send Cancel Request Mail
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnCancelRequest_Click(object sender, EventArgs e)
        {
            HiddenField txtFromTime = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnFromTime");
            HiddenField txtToTime = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnToTime");
            HiddenField txtToDate = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnToDate");
            HiddenField txtFromDate = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnFromDate");
            string[] startFromDate = txtFromDate.Value.Split('/');
            DateTime startDate = Convert.ToDateTime(startFromDate[0] + "/" + startFromDate[1] + "/" + startFromDate[2] + " " + txtFromTime.Value);
            string[] endToDate = txtToDate.Value.Split('/');
            DateTime endDate = Convert.ToDateTime(endToDate[0] + "/" + endToDate[1] + "/" + endToDate[2] + " " + txtToTime.Value);
            if (this.requestDetails != null)
            {
                int visitDetailsID = Convert.ToInt32(this.hdnVisitDetailsID.Value);
                DataSet dt = this.requestDetails.Badgereturnvalues(Convert.ToString(visitDetailsID));
                string hostmailID = this.requestDetails.GetHostmailID(dt.Tables[0].Rows[0].ItemArray[0].ToString());

                VMSDataLayer.VMSDataLayer.PropertiesDC objpropertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                objpropertiesDC = this.requestDetails.DisplayInfo(visitDetailsID);
                DataTable dtlocationDetails = this.requestDetails.GetLocationDetailsById(objpropertiesDC.VisitorRequestProperty.RequestID);
                this.requestDetails.SendCancelMailtoHost(visitDetailsID, hostmailID, objpropertiesDC.VisitorMasterProperty, objpropertiesDC.VisitorRequestProperty, Session["LoginID"].ToString(), startDate, endDate, dtlocationDetails);
                this.DisableButtons("CANCELLED");
                ////ClientScript.RegisterStartupScript(typeof(Page), "CancelSuccess", "<script language='javascript'>alert('Request is cancelled');window.location.href = 'ViewLogbySecurity.aspx';</script>", false);
                try
                {
                    Response.Redirect("SafetyPermit.aspx", true);
                     
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }

        /// <summary>
        /// time extend hidden button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnTimeExtendHidden_Click(object sender, EventArgs e)
        {
            if (this.Session["MailExtendTrigger"] != null)
            {
                if (this.Session["MailExtendTrigger"].ToString() == "true")
                {
                    if (Request.QueryString.ToString().Contains("details="))
                    {
                        string detailsId = Request.QueryString["details"];
                        DataSet dt = this.requestDetails.Badgereturnvalues(detailsId);

                        string dtfromDate = DateTime.Parse(((HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnFromDate")).Value).ToString("MM/dd/yyyy");
                        string dttoDate = DateTime.Parse(((HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnToDate")).Value).ToString("MM/dd/yyyy");

                        HiddenField txtFromTime = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnFromTime");
                        HiddenField txtToTime = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnToTime");
                        HiddenField hdnTimeExtendHidden = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnToTimeBeforeExtended");
                        string[] startFromDate = dtfromDate.Split('/');
                        DateTime startDate = Convert.ToDateTime(startFromDate[0] + "/" + startFromDate[1] + "/" + startFromDate[2] + " " + txtFromTime.Value);
                        string[] extendedEndToDate = dttoDate.Split('/');
                        DateTime dtextendedEndToDateEndDate = Convert.ToDateTime(extendedEndToDate[0] + "/" + extendedEndToDate[1] + "/" + extendedEndToDate[2] + " " + txtToTime.Value);
                        ////string timeStart = StartDate.ToString("HH:mm t");

                        DateTime endDateBeforeTimeExtend = Convert.ToDateTime(extendedEndToDate[0] + "/" + extendedEndToDate[1] + "/" + extendedEndToDate[2] + " " + hdnTimeExtendHidden.Value);
                        ////string timeEnd = EndDate.ToString("HH:mm t");                                
                        this.requestDetails.TimeextendedmailtohostfromSP(dt, startDate, dtextendedEndToDateEndDate, endDateBeforeTimeExtend, Session["HostId"].ToString());
                        this.Session["MailExtendTrigger"] = "false";
                        ////Response.Redirect("ViewLogbySecurity.aspx", false);
                        ////Response.Redirect("SafetyPermit.aspx", false);
                    }
                }
            }
        }

        /// <summary>
        /// Submitting the Enter Information Page Details.
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.Session["SaveClick"] = true;
                this.saveClick = true;
                this.lblSubmitSuccess.Visible = false;

                if ((this.Session["Status"] == null) && (this.Session["RequestID"] == null))
                {
                    this.SaveVisitorInformation("Saved");
                }
                else if (this.Session["Status"] != null)
                {
                    if (this.Session["Status"].ToString().Equals("Saved"))
                    {
                        this.SaveVisitorInformation("Submitted");
                    }
                }
                else if ((this.Session["RequestID"] != null) && (this.Session["Status"] == null))
                {
                    this.SaveVisitorInformation("Submitted");
                }
            }
            catch (Exception ex)
            {
                // Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// display the error message
        /// </summary>
        /// <param name="strError">string error message</param>
        /// <param name="bldisplay">display value</param>
        protected void DisplayError(string strError, bool bldisplay)
        {
            string[] strGE = strError.Split('%');
            if (bldisplay == true)
            {
                if (string.IsNullOrEmpty(strGE[0].ToString()) && string.IsNullOrEmpty(strGE[1].ToString()) && string.IsNullOrEmpty(strGE[2].ToString()) && string.IsNullOrEmpty(strGE[3].ToString()))
                {
                    this.lblError1.Visible = false;
                    this.lblError1.Text = string.Empty;
                    this.lblError2.Visible = false;
                    this.lblError2.Text = string.Empty;
                    this.lblError3.Visible = false;
                    this.lblError3.Text = string.Empty;
                    this.lblError4.Visible = false;
                    this.lblError4.Text = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(strGE[0].ToString()))
                    {
                        this.lblError1.Visible = true;
                        this.lblError1.Text = strGE[0].ToString();
                    }

                    if (string.IsNullOrEmpty(strGE[0].ToString()))
                    {
                        this.lblError1.Visible = false;
                    }

                    if (!string.IsNullOrEmpty(strGE[1].ToString()))
                    {
                        this.lblError2.Visible = true;
                        this.lblError2.Text = strGE[1].ToString();
                    }

                    if (string.IsNullOrEmpty(strGE[1].ToString()))
                    {
                        this.lblError2.Visible = false;
                    }

                    if (!string.IsNullOrEmpty(strGE[2].ToString()))
                    {
                        this.lblError3.Visible = true;
                        this.lblError3.Text = strGE[2].ToString();
                    }

                    if (string.IsNullOrEmpty(strGE[2].ToString()))
                    {
                        this.lblError3.Visible = false;
                    }

                    if (!string.IsNullOrEmpty(strGE[3].ToString()))
                    {
                        this.lblError4.Visible = true;
                        this.lblError4.Text = strGE[3].ToString();
                    }

                    if (string.IsNullOrEmpty(strGE[0].ToString()))
                    {
                        this.lblError4.Visible = false;
                    }
                }
            }
            else
            {
                this.lblError1.Visible = false;
                this.lblError1.Text = string.Empty;
                this.lblError2.Visible = false;
                this.lblError2.Text = string.Empty;
                this.lblError3.Visible = false;
                this.lblError3.Text = string.Empty;
                this.lblError4.Visible = false;
                this.lblError4.Text = string.Empty;
            }
        }

        /// <summary>
        /// Used to save the visitor information into an object.
        /// </summary>
        /// <param name="status">status value</param>
        protected void SaveVisitorInformation(string status)
        {
            try
            {
                IdentityDetails identityDetails = this.GetIdentityDetailsForArgentina();

                if (status.Equals("Submitted"))
                {
                    this.lblError.Visible = false;
                }

                this.visitorMasterObj = this.VisitorGeneralInformationControlSP.InsertGeneralInformation();
                this.visitorProofObj = this.VisitorGeneralInformationControlSP.InsertPhoto();
                this.visitorLocObj = this.VisitorLocationInformationControlSP.InsertLocationInformation();
                this.arrayofVisitDetails = this.VisitorLocationInformationControlSP.GetVisitDetails();

                if (this.visitorLocObj.Purpose.Equals("Family Visit") ||
                this.visitorLocObj.Purpose.Equals("Interview Candidate") ||
                this.visitorLocObj.Purpose.Equals("Former Employee"))
                {
                    this.visitorMasterObj.Company = "NA";
                }

                if (this.generatebadgeclick == true)
                {
                    if (this.visitorLocObj.FromTime > this.visitorLocObj.ToTime)
                    {
                        this.visitorLocObj.ToDate = this.visitorLocObj.ToDate.Value.AddDays(-1);
                        TextBox txtToDate = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToDate");
                        txtToDate.Text = this.visitorLocObj.ToDate.Value.ToString("dd/MM/yyyy");
                    }
                }

                this.visitorLocObj.Facility = this.visitorLocObj.Facility + "|" + "Security";

                if (status.Equals("Saved"))
                {
                    this.visitorLocObj.Status = "Saved";
                }
                else if (status.Equals("Submitted"))
                {
                    if (this.Session["Status"] != null)
                    {
                        if (this.Session["Status"].ToString().Equals("Saved") && this.saveClick == true)
                        {
                            this.visitorLocObj.Status = "Saved";
                            status = "Saved";
                        }
                    }
                    else
                    {
                        if (this.Session["Status"] == null && this.submitClick == true && (bool)this.Session["SaveClick"] == true)
                        {
                            this.visitorLocObj.Status = "Submitted";
                            this.Session["Status"] = "Submitted";
                        }
                        else if (((this.Session["Status"] == null) && (this.submitClick == true)) && ((bool)this.Session["SaveClick"] == false))
                        {
                            this.visitorLocObj.Status = "Submitted";
                        }
                        else if (this.Session["Status"] != null)
                        {
                            if (this.Session["Status"].ToString().Equals("Saved") && this.submitClick == true)
                            {
                                this.visitorLocObj.Status = "Submitted";
                            }
                        }
                        else if (((this.saveClick == true) && this.Session["Status"] == null) && (this.Session["RequestID"] != null))
                        {
                            this.visitorLocObj.Status = "Saved";
                            status = "Saved";
                        }
                    }
                }

                this.visitorLocObj.VisitorID = this.visitorMasterObj.VisitorID;
                this.visitorEmergencyContactObj = this.EmergencyContactInformationControlSP.InsertEmergencyContactInformation();
                this.visitorEmergencyContactObj.RequestID = this.visitorLocObj.RequestID;
                this.visitorEquipmentObj = this.EquipmentPermittedControlSP.InsertEquipmentInformation(Convert.ToInt32(Request.QueryString["RequestID"]));

                HttpContext.Current.Session["VisitorMasterObj"] = this.visitorMasterObj;
                HttpContext.Current.Session["VisitorProofObj"] = this.visitorProofObj;
                HttpContext.Current.Session["VisitorLocObj"] = this.visitorLocObj;
                HttpContext.Current.Session["VisitorEmergencyContactObj"] = this.visitorEmergencyContactObj;
                HttpContext.Current.Session["VisitorEquipmentObj"] = this.visitorEquipmentObj;
                HttpContext.Current.Session["SaveFlag"] = true;

                if (status.Equals("Saved"))
                {
                    if (this.Session["Status"] != null)
                    {
                        if ((this.saveClick == true) && this.Session["Status"].ToString().Equals("Saved"))
                        {
                            this.UpdateVisitorInformation();
                        }
                    }
                    else if (this.Session["Status"] == null)
                    {
                        if (((this.saveClick == true) && this.Session["Status"] == null) && (this.Session["RequestID"] == null))
                        {
                            this.successSubmission = this.requestDetails.SubmitVisitorInformation(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, identityDetails);

                            if (this.successSubmission == 0)
                            {
                                this.Session["RequestID"] = this.visitorLocObj.RequestID;
                                this.Session["VisitorID"] = this.visitorMasterObj.VisitorID;
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.SavedForFutureSubmission + "'); </script>");
                            }

                            if (this.successSubmission.Equals(1))
                            {
                                ////this.lblSubmitSuccess.Visible = true;
                                this.lblSubmitSuccess.Text = Resources.LocalizedText.SavedForFutureSubmission;
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.SavedForFutureSubmission + "'); </script>");
                            }
                        }
                        else if (((this.saveClick == true) && this.Session["Status"] == null) && (this.Session["RequestID"] != null))
                        {
                            this.Session["Status"] = "Saved";
                            this.UpdateVisitorInformation();
                        }
                    }

                    this.lblSubmitSuccess.Text = Resources.LocalizedText.SavedForFutureSubmission;
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.SavedForFutureSubmission + "'); </script>");
                }
                else if (status.Equals("Submitted"))
                {
                    this.Save.Enabled = false;
                }
            }
            catch (VMSBL.CustomException ex)
            {
                this.DisplayError(ex.ErrorMessage, true);
                return;
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// reset click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Reset_Click(object sender, EventArgs e)
        {
            this.ResetData();
            this.GenerateBadge.Enabled = false;
            this.Cancel.Enabled = false;
        }

        /// <summary>
        /// Search Click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Search_Click(object sender, EventArgs e)
        {
            try
            {
                this.GetVisitorInformationDetailsbyVisitorID();
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// cancel click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Session["RequestID"] != null)
                {
                    int reqID = Convert.ToInt32(Session["RequestID"].ToString());
                    this.successSubmission = this.requestDetails.CancelRequest(reqID);
                    if (this.successSubmission.Equals(1))
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), "script", "<script language='javascript'>alert('" + Resources.LocalizedText.Requestnotcancelled + "');</script>", false);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "CancelRequestMail", "CancelRequestMail();", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "script", "<script language='javascript'>alert('" + Resources.LocalizedText.Requestnotcancelled + "');</script>", false);
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// hidden button click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Request.QueryString.ToString().Contains("RequestID="))
                {
                    if (((CheckBox)this.VisitorGeneralInformationControlSP.FindControl("chkMultipleEntry")).Checked == true)
                    {
                        this.ResetVisitorInformation(false);
                        this.btnUpload.Enabled = true;
                        this.btnIdProof.Enabled = true;
                        this.Session["RequestID"] = null;
                    }
                    else
                    {
                        this.ResetVisitorInformation(true);
                        this.VisitorGeneralInformationControlSP.ChkMultipleEntryVisibility();
                    }
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// hidden button 1 click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnHidden1_Click(object sender, EventArgs e)
        {
            CheckBox chkMultipleEntries = (CheckBox)this.VisitorGeneralInformationControlSP.FindControl("chkMultipleEntry");
            if (chkMultipleEntries.Checked)
            {
                this.ResetVisitorInformation(false);
                this.DisableButtons(string.Empty);
            }
            else
            {
                ////Response.Redirect("ViewLogbySecurity.aspx");
                try
                {
                    Response.Redirect("SafetyPermit.aspx", true);
                     
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }

        /// <summary>
        /// button generate badge click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnGenerateBadge_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Visible = false;
                if (!string.IsNullOrEmpty(this.hdnVisitDetailsID.Value))
                {
                    DataSet dt = this.requestDetails.Badgereturnvalues(this.hdnVisitDetailsID.Value);
                    string hostmailID = this.requestDetails.GetHostmailID(dt.Tables[0].Rows[0].ItemArray[0].ToString());
                    this.BatchNo.Value = this.requestDetails.GenerateBadge(Convert.ToInt32(this.hdnVisitDetailsID.Value), hostmailID, this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.visitorEquipmentObj, this.visitorEmergencyContactObj, Session["LoginID"].ToString());
                    string strScript = "<script language='javascript'>window.open('GenerateBadge.aspx?key=" + XSS.HtmlEncode(this.hdnVisitDetailsID.Value) + "', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript); ////Added for CR 17 Fix,12 May 2011 Tincy
                    this.GenerateBadge.Enabled = false;
                    this.btnIdProof.Enabled = false;
                    this.btnUpload.Enabled = false;
                    this.badgegenerate = true;
                }
                else
                {
                    this.lblError.Text = Resources.LocalizedText.SubmitMandatory;
                    this.lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// check in button click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnCheckIn_Click(object sender, EventArgs e)
        {
            try
            {
                string securityID = Session["LoginID"].ToString();
                ////string detailsID = Session["DetailsID"].ToString();
                string strdetailsID = Session["strdetailsIDsession"].ToString();
                this.lblError.Visible = false;
                if (!string.IsNullOrEmpty(this.hdnVisitDetailsID.Value))
                {
                    this.DisableButtons("IN");
                    if (this.requestDetails.VisitorCheckIn(Convert.ToInt32(this.hdnVisitDetailsID.Value.ToString().Trim()), securityID))
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert(\"Visitor's Badge is successfully generated.\"); </script>");
                        string strScript = "<script language='javascript'>(window.open('SPBadge.aspx?key=" + strdetailsID + "', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ')).focus();</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript);
                        return;
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.Visitorcheckinsuccessful + "'); </script>");
                    }
                }
                else
                {
                    this.lblError.Text = Resources.LocalizedText.SubmitMandatory;
                    this.lblError.Visible = true;
                }

                return;
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// back click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Back_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SafetyPermit.aspx", true);
                
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
        }

        /// <summary>
        /// disable buttons
        /// </summary>
        /// <param name="status">status value</param>
        protected void DisableButtons(string status)
        {
            try
            {
                this.visitstatus = status;
                switch (status.ToUpper().Trim())
                {
                    case "YET TO ARRIVE":
                        {
                            this.btnWebcam.Disabled = false;
                            this.GenerateBadge.Enabled = true;
                            break;
                        }

                    case "IN":
                        {
                            this.btnWebcam.Disabled = true;
                            this.btnUpload.Enabled = false;
                            this.btnIdProof.Enabled = false;
                            this.GenerateBadge.Enabled = false;
                            break;
                        }

                    case "CANCELLED":
                        {
                            this.btnWebcam.Disabled = true;
                            this.btnIdProof.Enabled = false;
                            this.btnUpload.Enabled = false;
                            ////this.Search.Enabled = false;
                            this.Save.Enabled = false;
                            this.Reset.Enabled = false;
                            this.Cancel.Enabled = false;
                            this.GenerateBadge.Enabled = false;
                            this.Submit.Enabled = false;
                            break;
                        }

                    case "OUT":
                        {
                            this.btnWebcam.Disabled = true;
                            this.btnIdProof.Enabled = false;
                            this.btnUpload.Enabled = false;
                            ////this.Search.Enabled = false;
                            this.Save.Enabled = false;
                            this.Reset.Enabled = false;
                            this.Cancel.Enabled = false;
                            this.GenerateBadge.Enabled = false;
                            this.Submit.Enabled = false;
                            break;
                        }

                    default:
                        {
                            this.btnWebcam.Disabled = false;
                            this.btnIdProof.Enabled = false;
                            this.btnUpload.Enabled = true;
                            ////this.Search.Enabled = true;
                            this.Save.Enabled = false;
                            this.Reset.Enabled = true;
                            this.Cancel.Enabled = false;
                            this.GenerateBadge.Enabled = false;
                            this.Submit.Enabled = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// reset visitor information
        /// </summary>
        /// <param name="isreset">is reset value</param>
        private void ResetVisitorInformation(bool isreset)
        {
            DropDownList ddlCity = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlCity");
            DropDownList ddlFacility = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlFacility");
            ddlFacility.Enabled = true;
            if (isreset == false & (((CheckBox)this.VisitorGeneralInformationControlSP.FindControl("chkMultipleEntry")).Checked == true))
            {
                this.VisitorGeneralInformationControlSP.ResetGeneralInformation(true);
                this.VisitorLocationInformationControlSP.ResetLocationInformation(true);
                this.EmergencyContactInformationControlSP.ResetEmergencyContactInformation();
                this.EquipmentPermittedControlSP.ResetEquipmentInformation(true);
            }
            else
            {
                this.VisitorGeneralInformationControlSP.ResetGeneralInformation(false);
                this.VisitorLocationInformationControlSP.ResetLocationInformation(false);
                this.EmergencyContactInformationControlSP.ResetEmergencyContactInformation();
                this.EquipmentPermittedControlSP.ResetEquipmentInformation(false);
                this.VisitorGeneralInformationControlSP.ChkMultipleEntryVisibility();
            }

            DataSet securitycity = this.GetSecurityCity();
            string city = securitycity.Tables[0].Rows[0]["City"].ToString();
            ddlCity.Items.Clear();
            ddlCity.Items.Add(city);
            ddlCity.Items.FindByText(city).Selected = true;
            ////ddlCity.Items.FindByText(City).Selected = true;
            ddlCity.Enabled = false;
            if (ddlFacility.Enabled == true)
            {
                ddlFacility.Items.Clear();
                ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
                if (securitycity.Tables[0].Rows.Count == 1)
                {
                    ddlFacility.Items.Add(securitycity.Tables[0].Rows[0]["Facility"].ToString());
                    ddlFacility.Items.FindByText(securitycity.Tables[0].Rows[0]["Facility"].ToString()).Selected = true;
                    ddlFacility.Enabled = false;
                }
                else
                {
                    for (int rowCount = 0; rowCount < securitycity.Tables[0].Rows.Count; rowCount++)
                    {
                        string facility = securitycity.Tables[0].Rows[rowCount]["Facility"].ToString();
                        ddlFacility.Enabled = true;
                        ddlFacility.Items.Add(facility);
                    }
                }
            }

            this.ShowEquipmentControl();

            this.lblError.Visible = false;
            this.lblError.Text = string.Empty;

            this.lblError1.Visible = false;
            this.lblError1.Text = string.Empty;

            this.lblError2.Visible = false;
            this.lblError2.Text = string.Empty;

            this.lblError3.Visible = false;
            this.lblError3.Text = string.Empty;

            this.lblError4.Visible = false;
            this.lblError4.Text = string.Empty;

            if (this.resetclick == true)
            {
                this.lblSubmitSuccess.Visible = false;
                this.lblSubmitSuccess.Text = string.Empty;
            }

            this.Reset.Enabled = true;
            this.Session["Status"] = null;
            this.Submit.Enabled = true;
            this.Save.Enabled = true;
            this.btnUpload.Enabled = true;
        }

        /// <summary>
        /// Get Visitor Information Details by Visitor ID
        /// </summary>
        private void GetVisitorInformationDetailsbyVisitorID()
        {
            int visitorID;
            VMSDataLayer.VMSDataLayer.PropertiesDC objpropertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
            try
            {
                visitorID = Convert.ToInt32(Request.QueryString["VisitorID"]);
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                if (this.Session["SearchVisitorID"] != null || visitorID != null)
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                {
                    if (visitorID == 0)
                    {
                        visitorID = int.Parse(Session["SearchVisitorID"].ToString());
                    }

                    this.Session["SearchVisitorID"] = null;
                    if (this.requestDetails != null)
                    {
                        objpropertiesDC = this.requestDetails.GetSearchDetails(visitorID);
                        if (objpropertiesDC != null)
                        {
                            if (objpropertiesDC.VisitorMasterProperty != null)
                            {
                                this.VisitorGeneralInformationControlSP.ShowGeneralInformationByRequestID(objpropertiesDC);
                                this.Session["VisitorID"] = objpropertiesDC.VisitorMasterProperty.VisitorID;
                            }

                            if (objpropertiesDC.VisitorProofProperty != null)
                            {
                                this.VisitorGeneralInformationControlSP.ShowGeneralInformationByPhoto(objpropertiesDC);
                                this.Session["VisitorID"] = objpropertiesDC.VisitorProofProperty.VisitorID;
                            }
                            else
                            {
                                this.Session["VisitorImgByte"] = null;
                                this.Session["ProofImgByte"] = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// Get Visitor Information Details by Request ID
        /// </summary>
        private void GetVisitorInformationDetailsbyRequestID()
        {
            int visitDetailsID;
            VMSDataLayer.VMSDataLayer.PropertiesDC objpropertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
            try
            {
                visitDetailsID = Convert.ToInt32(Request.QueryString["details"]);
                if (this.requestDetails != null)
                {
                    objpropertiesDC = this.requestDetails.DisplayInfo(visitDetailsID);
                    ////Session["StrHostId"] = propertiesDC.VisitorRequestProperty.Createdby;
                    if (objpropertiesDC.VisitorMasterProperty != null)
                    {
                        this.VisitorGeneralInformationControlSP.ShowGeneralInformationByRequestID(objpropertiesDC);
                        this.Session["VisitorID"] = objpropertiesDC.VisitorMasterProperty.VisitorID;
                    }

                    if (objpropertiesDC.VisitorRequestProperty != null)
                    {
                        this.VisitorLocationInformationControlSP.ShowLocationInformationByRequestID(objpropertiesDC);
                        this.Session["RequestID"] = objpropertiesDC.VisitorRequestProperty.RequestID;
                        this.Session["Status"] = objpropertiesDC.VisitorRequestProperty.Status;
                    }

                    if (objpropertiesDC.VisitorProofProperty != null)
                    {
                        this.VisitorGeneralInformationControlSP.ShowGeneralInformationByPhoto(objpropertiesDC);
                    }
                    else
                    {
                        this.Session["VisitorImgByte"] = null;
                        Image imgPhoto = (Image)this.VisitorGeneralInformationControlSP.FindControl("imgPhoto");
                        imgPhoto.ImageUrl = "~/Images/DummyPhoto.png";
                        this.Session["ProofImgByte"] = null;
                    }

                    if (objpropertiesDC.VisitorEquipmentProperty != null)
                    {
                        this.EquipmentPermittedControlSP.ShowEquipmentInformationByRequestID(objpropertiesDC);
                    }

                    if (objpropertiesDC.VisitorEmergencyContactProperty != null)
                    {
                        this.EmergencyContactInformationControlSP.ShowEmergencyContactInformationByRequestID(objpropertiesDC);
                    }

                    string badgeStatus = string.Empty;
                    if (!string.IsNullOrEmpty(objpropertiesDC.VisitDetailProperty.BadgeStatus))
                    {
                        badgeStatus = objpropertiesDC.VisitDetailProperty.BadgeStatus.ToUpper();
                    }

                    this.VisitorGeneralInformationControlSP.DisableVisitGeneralInformationControls(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper());
                    this.VisitorLocationInformationControlSP.DisableVisitLocationInformationControls(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper(), badgeStatus);
                    this.EquipmentPermittedControlSP.DisableEquipmentControl(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper(), badgeStatus);
                    this.EmergencyContactInformationControlSP.DisableEmergencyContactInformationControl(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper(), badgeStatus);
                    this.hdnVisitDetailsID.Value = visitDetailsID.ToString();
                    this.DisableButtons(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper());
                }
            }
            catch (Exception ex)
            {
                // Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// Update Visitor Information
        /// </summary>
        private void UpdateVisitorInformation()
        {
            try
            {
                IdentityDetails identityDetails = this.GetIdentityDetailsForArgentina();
                int equipcustody = 0;
                string strSP = "SafetyPermit";
                this.visitorMasterObj = this.VisitorGeneralInformationControlSP.InsertGeneralInformation();
                this.visitorProofObj = this.VisitorGeneralInformationControlSP.InsertPhoto();
                this.visitorMasterObj.VisitorID = Convert.ToInt32(this.Session["VisitorID"]);
                this.visitorLocObj = this.VisitorLocationInformationControlSP.InsertLocationInformation1(strSP);
                //// ArrayofVisitDetails = VisitorLocationInformationControl.GetVisitDetails();
                this.arrayofVisitDetails = this.VisitorLocationInformationControlSP.GetVisitDetailsByRequestID(Session["RequestID"].ToString());
                HiddenField hdnDate = (HiddenField)this.VisitorLocationInformationControlSP.FindControl("hdnDate");
                ////VisitorLocObj.HostID = VisitorLocObj.Createdby;
                ////VisitorLocObj.HostID = Session["strHostId"].ToString();
                this.visitorLocObj.HostID = Session["StrUserIDForSP"].ToString();
                ////TextBox txtHost = (TextBox)VisitorLocationInformationControlSP.FindControl("txtHost");
                ////txtHost.Text = Session["strHostId"].ToString();
                if (this.visitorLocObj.Purpose.Equals("Family Visit") ||
                this.visitorLocObj.Purpose.Equals("Interview Candidate") ||
                this.visitorLocObj.Purpose.Equals("Former Employee"))
                {
                    this.visitorMasterObj.Company = "NA";
                }

                if (this.generatebadgeclick == true)
                {
                    if (this.visitorLocObj.FromTime > this.visitorLocObj.ToTime)
                    {
                        this.visitorLocObj.ToDate = this.visitorLocObj.ToDate.Value.AddDays(-1);
                        TextBox txtToDate = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtToDate");
                        txtToDate.Text = this.visitorLocObj.ToDate.Value.ToString("dd/MM/yyyy");
                    }
                }

                this.visitorLocObj.VisitorID = Convert.ToInt32(this.Session["VisitorID"]);
                //// VisitorLocObj.Facility = VisitorLocObj.Facility + "|" + "Security";

                if (this.Session["Status"] != null)
                {
                    this.visitorLocObj.Status = Session["Status"].ToString();
                }

                if (this.visitorLocObj.Status.Equals("Saved") && this.saveClick == true)
                {
                    this.visitorLocObj.Status = "Saved";
                }
                else if (this.visitorLocObj.Status.Equals("Saved") && this.saveClick == false)
                {
                    this.visitorLocObj.Status = "Submitted";
                }
                else if (this.visitorLocObj.Status.Equals("Submitted"))
                {
                    if (this.Submit.Text.Equals(Resources.LocalizedText.Submit))
                    {
                        this.visitorLocObj.Status = "Submitted";
                    }
                    else if (this.Submit.Text.Equals(Resources.LocalizedText.Update))
                    {
                        this.visitorLocObj.Status = "Updated";
                    }
                }
                else if (this.visitorLocObj.Status.Equals("Updated"))
                {
                    this.visitorLocObj.Status = "Updated";
                }

                this.visitorProofObj = this.VisitorGeneralInformationControlSP.InsertPhoto();
                this.visitorProofObj.VisitorID = Convert.ToInt32(this.Session["VisitorID"]);
                this.visitorEquipmentObj = this.EquipmentPermittedControlSP.InsertEquipmentInformation(Convert.ToInt32(this.Session["RequestID"]));
                this.visitorEmergencyContactObj = this.EmergencyContactInformationControlSP.InsertEmergencyContactInformation();
                ////VisitorEmergencyContactObj.RequestID = Convert.ToInt32(Session["RequestID"]);
                //// Added by priti on 3rd June for VMS CR VMS31052010CR6
                if (this.visitorLocObj.RequestStatus.ToUpper().Equals(VMSConstants.REPEATVISITOR))
                {
                    this.successSubmission = this.requestDetails.EditVisitorInformationForRepeatVisitor(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, equipcustody);
                }
                else
                {
                    this.successSubmission = this.requestDetails.EditVisitorInformation(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, equipcustody, identityDetails);
                }

                if (this.visitorLocObj.RequestStatus.ToUpper().Equals("IN"))
                {
                    this.DisableButtons("IN");
                }

                if (this.successSubmission.Equals(1))
                {
                    ////this.lblSubmitSuccess.Visible = true;
                    this.lblSubmitSuccess.Text = Resources.LocalizedText.UpdationFailed;
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.UpdationFailed + "'); </script>");
                }
                else
                {
                    this.lblSubmitSuccess.Visible = false;
                    this.lblError.Visible = false;
                    this.lblError1.Visible = false;
                    this.lblError2.Visible = false;
                    this.lblError3.Visible = false;
                    this.lblError4.Visible = false;
                    if (!this.visitorLocObj.Status.Equals("Saved"))
                    {
                        if (this.visitorLocObj.Status.Equals("Submitted"))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.SubmittedSuccessfully + "'); </script>");
                            this.lblSubmitSuccess.Text = Resources.LocalizedText.SubmittedSuccessfully;
                        }
                        else if (this.visitorLocObj.Status.Equals("Updated"))
                        {
                            if (this.generatebadgeclick != true)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.UpdatedSuccessfully + "'); </script>");
                            }
                        }

                        this.lblSubmitSuccess.Text = Resources.LocalizedText.UpdatedSuccessfully;
                        this.lblSubmitSuccess.Visible = false;
                        this.Submit.Enabled = false;
                        this.Save.Enabled = false;
                    }
                    else if (this.visitorLocObj.Status.Equals("Saved"))
                    {
                        ////this.lblSubmitSuccess.Visible = true;
                        this.Submit.Enabled = true;
                        this.Save.Enabled = true;
                    }

                    TextBox txtFromTime = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromTime");
                    DateTime fromtime = Convert.ToDateTime(txtFromTime.Text.ToString());

                    string dtfromDate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControlSP.FindControl("txtFromDate")).Text).ToString("MM/dd/yyyy");
                    string[] fromdate = dtfromDate.Split('/');
                    DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate[0] + "/" + fromdate[1] + "/" + fromdate[2] + " " + txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                    DateTime dtcurrentTime = this.genTimeZone.GetCurrentDate();
                    if (
                       (DateTime.Compare(this.visitorLocObj.FromDate.Value, dtcurrentTime.Date) <= 0)
                         && (DateTime.Compare(DateTime.ParseExact(hdnDate.Value, "dd/MM/yyyy", null).Date, dtcurrentTime.Date) == 0)
                       && (DateTime.Compare(this.visitorLocObj.FromDate.Value, dtcurrentTime.Date) >= 0)
                       && this.visitstatus != "IN"
                       && this.visitstatus != "CANCELLED"
                       && this.visitstatus != "OUT"
                       && this.visitstatus != "YET TO ARRIVE"
                       && Convert.ToDateTime(dtcurrentTime.ToString("H:mm")) <= startDate.AddMinutes(10))
                    {
                        this.GenerateBadge.Enabled = true;
                        ////commented by bincey
                        ////VMSBusinessEntity.VisitDetail VisitDetail1 = ArrayofVisitDetails[0];
                        ////hdnVisitDetailsID.Value = VisitDetail1.VisitDetailsID.ToString();
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.UpdatedSuccessfully + "'); </script>");
                        this.lblSubmitSuccess.Text = Resources.LocalizedText.UpdatedSuccessfully;
                        this.lblSubmitSuccess.Visible = false;
                        ////commented by bincey
                        ////VMSBusinessEntity.VisitDetail VisitDetail1 = ArrayofVisitDetails[0];
                        ////hdnVisitDetailsID.Value = VisitDetail1.VisitDetailsID.ToString();
                    }
                }
            }
            catch (VMSBL.CustomException ex)
            {
                this.DisplayError(ex.ErrorMessage, true);
                return;
            }
            catch (Exception ex)
            {
                ////Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        //// addition for CR IRVMS22062010CR07  starts here done by Priti

        /// <summary>
        /// get identity details for Argentina
        /// </summary>
        /// <returns>object value</returns>
        private IdentityDetails GetIdentityDetailsForArgentina()
        {
            IdentityDetails identityDetails = null;

            DropDownList ddlCountry = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlCountry");
            DropDownList ddlIdentityType = (DropDownList)this.VisitorLocationInformationControlSP.FindControl("ddlIdentityType");
            TextBox txtIdentityNo = (TextBox)this.VisitorLocationInformationControlSP.FindControl("txtIdentityNo");
            if (ddlCountry.SelectedItem.Text == "Argentina")
            {
                identityDetails = new IdentityDetails();
                if (ddlIdentityType.SelectedItem != null)
                {
                    if (ddlIdentityType.SelectedItem.Text != "Select")
                    {
                        identityDetails.IdentityType = ddlIdentityType.SelectedItem.Text.Trim();
                        identityDetails.IdentityNo = txtIdentityNo.Text.Trim();
                    }
                }
            }

            return identityDetails;
        }
    }
}
