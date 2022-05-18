

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
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
    /// VMS Enter information by security
    /// </summary>
    public partial class ClientEnterInformationBySecurity : System.Web.UI.Page
    {
        /// <summary>
        /// variables region
        /// </summary>
        #region Variables
        private VMSBusinessEntity.tblEquipmentsInCustody equipmentsInCustodyObj = new VMSBusinessEntity.tblEquipmentsInCustody();

        /// <summary>
        /// equipment in custody object list
        /// </summary>
        private VMSBusinessEntity.tblEquipmentsInCustody[] equipmentsInCustodyObjList = new VMSBusinessEntity.tblEquipmentsInCustody[5];

        /// <summary>
        /// visitor proof object
        /// </summary>
        private VMSBusinessEntity.VisitorProof visitorProofObj = new VMSBusinessEntity.VisitorProof();

        /// <summary>
        /// visitor master object
        /// </summary>
        private VMSBusinessEntity.VisitorMaster visitorMasterObj = new VMSBusinessEntity.VisitorMaster();

        /// <summary>
        /// visitor location object
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
        /// validations object
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

#pragma warning disable CS0414 // The field 'ClientEnterInformationBySecurity.batchNo1' is assigned but its value is never used
        /// <summary>
        /// batch No
        /// </summary>
        private int batchNo1 = 0;
#pragma warning restore CS0414 // The field 'ClientEnterInformationBySecurity.batchNo1' is assigned but its value is never used

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

#pragma warning disable CS0414 // The field 'ClientEnterInformationBySecurity.badgegenerate' is assigned but its value is never used
        /// <summary>
        /// badge generate
        /// </summary>
        private bool badgegenerate = false;
#pragma warning restore CS0414 // The field 'ClientEnterInformationBySecurity.badgegenerate' is assigned but its value is never used

        /// <summary>
        /// generate badge click
        /// </summary>
        private bool generatebadgeclick = false;

        /// <summary>
        /// count value
        /// </summary>
        private int count = 0;

        /// <summary>
        /// token number
        /// </summary>
        private int intTokenNumber = Convert.ToInt32(ConfigurationManager.AppSettings["TokenNumber"]);
        #endregion

        /// <summary>
        /// assign time zone offset
        /// </summary>
        /// <param name="strTimezoneoffset">string value</param>
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
        /// assign current date time
        /// </summary>
        /// <param name="currentDate">current date</param>
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
        /// submit data
        /// </summary>
        public void SubmitData()
        {
            ////TextBox txtDescription = ((TextBox)EquipmentCustodyControl.FindControl("txtDescription"));
            string strErrorMessage = string.Empty;
            this.Session["SaveFlag"] = true;
            try
            {
                IdentityDetails identityDetails = this.GetIdentityDetailsForArgentina();

                string dtfromDate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromDate")).Text).ToString("MM/dd/yyyy");
                string dttoDate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControl.FindControl("txtToDate")).Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');
                string fromTime = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromTime")).Text;
                string totime = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtToTime")).Text;
                DateTime fromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                DateTime todate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + totime), Convert.ToString(Session["TimezoneOffset"]));
                DateTime today = this.genTimeZone.GetCurrentDate();
                TimeSpan todateSpan = todate - today;
                TimeSpan fromDateSpan = fromDate - today;
                var clientOffset = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnClientOffset");
                if (Convert.ToInt32(((HiddenField)this.VisitorLocationInformationControl.FindControl("AdvanceAllowabledays")).Value) > todateSpan.Days + 1 && Convert.ToInt32(((HiddenField)this.VisitorLocationInformationControl.FindControl("AdvanceAllowabledays")).Value) > fromDateSpan.Days + 1)
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
                            this.visitorProofObj = this.VisitorGeneralInformationControl.InsertPhoto();
                            this.visitorMasterObj = this.VisitorGeneralInformationControl.InsertGeneralInformation();
                            this.visitorLocObj = this.VisitorLocationInformationControl.InsertLocationInformation();
                            this.arrayofVisitDetails = this.VisitorLocationInformationControl.GetVisitDetails();
                            this.visitorLocObj.Offset = clientOffset.Value;

                            if (this.generatebadgeclick == true)
                            {
                                if (this.visitorLocObj.FromTime > this.visitorLocObj.ToTime)
                                {
                                    this.visitorLocObj.ToDate = this.visitorLocObj.ToDate.Value.AddDays(-1);

                                    TextBox txtToDate = (TextBox)this.VisitorLocationInformationControl.FindControl("txtToDate");

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
                            this.visitorEquipmentObj = this.EquipmentPermittedControl.InsertEquipmentInformation(Convert.ToInt32(Request.QueryString["RequestID"]));
                            this.visitorEmergencyContactObj = this.EmergencyContactInformationControl.InsertEmergencyContactInformation();
                            this.visitorEmergencyContactObj.RequestID = this.visitorLocObj.RequestID;
                            this.successSubmission = this.requestDetails.SubmitVisitorInformation(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, identityDetails);
                            this.Session["RequestID"] = this.visitorLocObj.RequestID;
                        }
                        else
                        {
                            ////12/9/09
                            if (this.Session["Status"] != null)
                            {
                                if (this.Session["Status"].ToString().Equals("Saved"))
                                {
                                    this.SaveVisitorInformation("Submitted");
                                }
                                ////14/09/09
                                if (this.Session["Status"].ToString().Equals("Submitted") && (this.submitClick == true) && (this.Submit.Text == Resources.LocalizedText.Submit) && ((bool)Session["SaveClick"] == true))
                                {
                                    this.Session["Status"] = null;
                                    this.SaveVisitorInformation("Submitted");
                                }
                                ////end
                            }

                            if ((this.Session["Status"] == null) && (this.submitClick == true))
                            {
                                this.SaveVisitorInformation("Submitted");
                            }

                            ////end
                            ////Commented for 1C migration
                            ////this.visitorMasterObj = (VMSBusinessEntity.VisitorMaster)HttpContext.Current.Session["VisitorMasterObj"];
                            ////this.visitorProofObj = (VMSBusinessEntity.VisitorProof)HttpContext.Current.Session["VisitorProofObj"];
                            ////this.visitorLocObj = (VMSBusinessEntity.VisitorRequest)HttpContext.Current.Session["VisitorLocObj"];
                            ////this.visitorEmergencyContactObj = (VMSBusinessEntity.VisitorEmergencyContact)HttpContext.Current.Session["VisitorEmergencyContactObj"];
                            ////this.visitorEquipmentObj = (VMSBusinessEntity.VisitorEquipment[])HttpContext.Current.Session["VisitorEquipmentObj"];
                            this.visitorLocObj.Offset = clientOffset.Value;
                            ////Changes done for VMS CR VMS06072010CR09 by Priti
                            strErrorMessage = this.RequestsPageValidations(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.visitorEmergencyContactObj, this.visitorEquipmentObj);
                            this.Session["SaveFlag"] = false;
                            int countCust = 0;
                            string[] strLogicErr = strErrorMessage.Split('%');
                            ////Changes done for VMS CR VMS06072010CR09 by Priti
                            if (!string.IsNullOrEmpty(strLogicErr[0]) || !string.IsNullOrEmpty(strLogicErr[1]) || !string.IsNullOrEmpty(strLogicErr[2]) || !string.IsNullOrEmpty(strLogicErr[3]))
                            {
                                throw new VMSBL.CustomException(strErrorMessage);
                            }
                            else
                            {
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
                                else if ((this.Session["Status"] == null) && (this.submitClick == true) && ((bool)Session["SaveClick"] == false))
                                {
                                    this.successSubmission = this.requestDetails.SubmitVisitorInformation(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, identityDetails);
                                    ////test -- eqpmnt in custody
                                    ////int tokenNumer = 0;
                                    this.Session["RequestID"] = this.visitorLocObj.RequestID;
                                    string visitDetailsID = string.Empty;
                                    VMSBusinessLayer.UserDetailsBL userDetailsBLObj = new VMSBusinessLayer.UserDetailsBL();
                                    DataTable dtvisitDetailsId = new DataTable();
                                    dtvisitDetailsId = userDetailsBLObj.GetVisitDetailsID(Convert.ToString(this.Session["RequestID"]));
                                    if (dtvisitDetailsId.Rows.Count > 0)
                                    {
                                        visitDetailsID = dtvisitDetailsId.Rows[0]["VisitDetailsID"].ToString();
                                    }

                                    ////DropDownList ddlEqpmnt = (DropDownList)EquipmentCustodyControl.FindControl("ddlEquipmentType");
                                    ////if (!ddlEqpmnt.SelectedItem.Value.Equals("22"))
                                    int tokenNumber = 0;
                                    if (this.Session["EquipmentCustody"].Equals(true))
                                    {
                                        string locationID = ((DropDownList)this.VisitorLocationInformationControl.FindControl("ddlFacility")).SelectedValue;
                                        this.equipmentsInCustodyObjList = this.EquipmentCustodyControl.InsertEquipmentCustodyInformation(Convert.ToInt32(visitDetailsID), locationID, tokenNumber);
                                        countCust = this.equipmentsInCustodyObjList.Length;

                                        for (int i = 0; i < countCust; i++)
                                        {
                                            userDetailsBLObj.SubmitEquipmentCustody(
                                            Convert.ToInt32(this.equipmentsInCustodyObjList[i].VisitDetailsID),
                                            this.equipmentsInCustodyObjList[i].TokenNumber,
                                            this.equipmentsInCustodyObjList[i].EquipmentType,
                                            this.equipmentsInCustodyObjList[i].Description,
                                            this.equipmentsInCustodyObjList[i].Status,
                                            Convert.ToString(this.equipmentsInCustodyObjList[i].FacilityID),
                                            Convert.ToDateTime(this.visitorLocObj.CreatedDate),
                                            Convert.ToInt32(this.equipmentsInCustodyObjList[i].EquipmentID));
                                        }
                                    }

                                    if (this.successSubmission == 0)
                                    {
                                        ////Notification To Host On submit the request by Security
                                        string hostNotificationmailer = ConfigurationManager.AppSettings["HostNotificationmailer_enable"].ToString();
                                        if (string.Compare(hostNotificationmailer, "Y") == 0)
                                        {
                                            MailNotification objMailNotofication = new MailNotification();
                                            string visitorName = this.visitorMasterObj.FirstName.ToString() + ',' + this.visitorMasterObj.LastName.ToString();
                                            string strVisitorType = string.Empty;
                                            if (((DropDownList)this.VisitorLocationInformationControl.FindControl("ddlPurpose")).SelectedItem.Value.Equals("Others"))
                                            {
                                                strVisitorType = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtPurpose")).Text;
                                            }
                                            else
                                            {
                                                strVisitorType = ((DropDownList)this.VisitorLocationInformationControl.FindControl("ddlPurpose")).SelectedItem.Value;
                                            }

                                            string[] name = this.visitorLocObj.HostID.Split('(');
                                            string[] id = name[1].Split(')');
                                            DataTable dtlocationDetails = this.requestDetails.GetLocationDetailsById(this.visitorLocObj.RequestID);
                                            string strCity = Convert.ToString(dtlocationDetails.Rows[0]["City"]).Trim();
                                            string strFacility = Convert.ToString(dtlocationDetails.Rows[0]["Facility"]);
                                            HiddenField txtToDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToDate");
                                            HiddenField txtFromDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromDate");
                                            HiddenField txtToTime = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToTime");
                                            HiddenField txtFromTime = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromTime");
                                            TextBox txtbxFromtime = (TextBox)this.VisitorLocationInformationControl.FindControl("txtFromDate");
                                            TextBox txtbxTotime = (TextBox)this.VisitorLocationInformationControl.FindControl("txtToDate");
                                            string[] startFromDate = txtbxFromtime.Text.Split('/');
                                            ////DateTime StartDate = Convert.ToDateTime(StartFromDate[0] + "/" + StartFromDate[1] + "/" + StartFromDate[2] + " " + txtFromTime.Value);
                                            DateTime startDate = DateTime.Parse(startFromDate[0] + "/" + startFromDate[1] + "/" + startFromDate[2] + " " + txtFromTime.Value, new CultureInfo("en-CA"));
                                            string[] endToDate = txtbxTotime.Text.Split('/');
                                            ////DateTime EndDate = Convert.ToDateTime(EndToDate[0] + "/" + EndToDate[1] + "/" + EndToDate[2] + " " + txtToTime.Value);
                                            DateTime endDate = DateTime.Parse(endToDate[0] + "/" + endToDate[1] + "/" + endToDate[2] + " " + txtToTime.Value, new CultureInfo("en-CA"));

                                            objMailNotofication.SendNotificationToHost(id[0].ToString(), name[0].ToString(), visitorName.ToString(), this.visitorMasterObj.Company.ToString(), strFacility, strCity, strVisitorType, Convert.ToString(startDate), Convert.ToString(endDate), this.visitorLocObj.RequestID.ToString());
                                            string securityID = Session["LoginID"].ToString();

                                            this.requestDetails.HostNotificationDetails(this.visitorLocObj.HostID.ToString(), this.visitorMasterObj.CreatedDate.ToString(), securityID.ToString());
                                        }

                                        ////end Notification
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
                            TextBox txtFromTime = (TextBox)this.VisitorLocationInformationControl.FindControl("txtFromTime");
                            DateTime fromtime = Convert.ToDateTime(txtFromTime.Text.ToString());
                            if ((DateTime.Compare(this.visitorLocObj.FromDate.Value, DateTime.Today) <= 0) &&
                                (DateTime.Compare(this.visitorLocObj.ToDate.Value, DateTime.Today) >= 0) &&
                                this.visitstatus != "IN" && this.visitstatus != "CANCELLED" && (this.visitstatus != "YET TO ARRIVE"
                                || this.visitstatus != VMSConstants.REPEATVISITOR) &&
                                Convert.ToDateTime(System.DateTime.Now.ToString("H:mm")) <= fromtime.AddMinutes(10))
                            {
                                ////this.GenerateBadge.Enabled = false;
                            }
                            else
                            {
                                ////this.GenerateBadge.Enabled = false;
                            }
                        }
                        else
                        {
                            string strMessage = Resources.LocalizedText.SubmittedSuccessfully;
                            int token = 0;
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + strMessage + "'); </script>");
                            ////add here token display
                            Label txtToken = (Label)this.EquipmentCustodyControl.FindControl("txttoken");
                            Label lbltokenNumber = (Label)this.EquipmentCustodyControl.FindControl("lbltokenNumber");
                            if (Session["EquipmentCustody"].Equals(true))
                            {
                                if (this.equipmentsInCustodyObjList.Count() > 0)
                                {
                                    if (this.equipmentsInCustodyObjList[0].TokenNumber > this.intTokenNumber)
                                    {
                                        token = Convert.ToInt32(this.equipmentsInCustodyObjList[0].TokenNumber) - this.intTokenNumber;
                                        txtToken.Text = Convert.ToString(token);
                                        txtToken.Visible = true;
                                        lbltokenNumber.Visible = true;
                                    }
                                    else
                                    {
                                        txtToken.Text = Convert.ToString(this.equipmentsInCustodyObjList[0].TokenNumber);
                                        txtToken.Visible = true;
                                        lbltokenNumber.Visible = true;
                                    }
                                }
                                else
                                {
                                    txtToken.Text = string.Empty;
                                }
                            }

                            ////  Page.RegisterClientScriptBlock("script", "<script language='javascript'> alert(\"Submitted Successfully\"); </script>");
                            this.lblSubmitSuccess.Text = strMessage;
                            ////this.lblSubmitSuccess.Visible = true;
                            ////this.GenerateBadge.Enabled = true;
                            TextBox txtFromTime = (TextBox)this.VisitorLocationInformationControl.FindControl("txtFromTime");
                            DateTime fromtime = Convert.ToDateTime(txtFromTime.Text.ToString());
                            var dtcurrentDate = this.genTimeZone.GetCurrentDate().ToShortDateString();
                            DateTime dttoday = DateTime.Parse(dtcurrentDate);
                            if ((DateTime.Compare(
                                this.visitorLocObj.FromDate.Value, dttoday) <= 0) &
                                (DateTime.Compare(this.visitorLocObj.ToDate.Value, dttoday) >= 0) &&
                                this.visitstatus != "IN" && this.visitstatus != "CANCELLED" && this.visitstatus != "OUT"
                                && (this.visitstatus != "YET TO ARRIVE" ||
                                this.visitstatus != VMSConstants.REPEATVISITOR) &&
                                Convert.ToDateTime(dttoday.ToString("H:mm")) <= fromtime.AddMinutes(10))
                            {
                                ////this.GenerateBadge.Enabled = true;
                            }
                            else
                            {
                                ////this.GenerateBadge.Enabled = false;
                            }

                            this.Save.Enabled = false;
                            this.Submit.Enabled = false;
                            ////this.btnUpload.Enabled = false;
                            ////this.btnWebcam.Disabled = true;
                            ////   this.btnWebcam.Disabled = false;
                            if (!Request.QueryString.ToString().Contains("RequestID="))
                            {
                                //// for submit action  Isreset parameter is false updated for CR IRVMS22062010CR07
                                if (((RadioButtonList)this.VisitorGeneralInformationControl.FindControl("multiplereqRadio")).SelectedValue == "1")
                                {
                                    this.ResetVisitorInformation(false);
                                    ////this.GenerateBadge.Enabled = false;
                                    this.Session["RequestID"] = null;
                                }
                            }
                        }
                    }
                    else
                        if (this.Submit.Text == Resources.LocalizedText.Update)
                    {
                        this.Session["UpdateFlag"] = true;
                        DataTable dttokenDetails = new DataTable();
                        int tokenNumber = 0;
                        if (Request.QueryString["details"] != null)
                        {
                            if (Request.QueryString.ToString().Contains("details="))
                            {
                                ////DateTime Today = genTimeZone.GetCurrentDate();
                                VMSBusinessLayer.UserDetailsBL userDetailsBLObj = new VMSBusinessLayer.UserDetailsBL();
                                int countCust = 0;
                                string locationID = ((DropDownList)this.VisitorLocationInformationControl.FindControl("ddlFacility")).SelectedValue;
                                string detailsId = VMSBusinessLayer.Decrypt(Request.QueryString["details"]);
                                this.Session["VisitDetailID"] = Convert.ToString(detailsId);
                                DataSet dt = this.requestDetails.Badgereturnvalues(detailsId);
                                string reqId = dt.Tables[0].Rows[0]["RequestId"].ToString().ToUpper();
                                this.Session["RequestID"] = Convert.ToString(reqId);
                                string status = dt.Tables[0].Rows[0]["RequestStatus"].ToString().ToUpper();
                                if (this.Session["UpdateFlag"].Equals(true))
                                {
                                    if (this.Session["EquipmentCustody"].Equals(true))
                                    {
                                        dttokenDetails = userDetailsBLObj.GetTokenDetails(Convert.ToInt32(this.Session["VisitDetailID"]));
                                        if (dttokenDetails.Rows.Count != 0)
                                        {
                                            tokenNumber = Convert.ToInt32(dttokenDetails.Rows[0]["TokenNumber"]);
                                        }
                                        else
                                        {
                                            tokenNumber = userDetailsBLObj.GetToken(Convert.ToInt32(detailsId), locationID);
                                        }
                                    }
                                }

                                if (status == "IN")
                                {
                                    ////added by bincey for eqpmnt custody update for in status

                                    if (this.Session["EquipmentCustody"].Equals(true))
                                    {
                                        this.equipmentsInCustodyObjList = this.EquipmentCustodyControl.InsertEquipmentCustodyInformation(Convert.ToInt32(detailsId), locationID, tokenNumber);
                                        userDetailsBLObj.ClearEquipmentData(detailsId);
                                        countCust = this.equipmentsInCustodyObjList.Length;
                                        for (int i = 0; i < countCust; i++)
                                        {
                                            userDetailsBLObj.SubmitEquipmentCustody(
                                              Convert.ToInt32(this.equipmentsInCustodyObjList[i].VisitDetailsID),
                                              this.equipmentsInCustodyObjList[i].TokenNumber,
                                              this.equipmentsInCustodyObjList[i].EquipmentType,
                                              this.equipmentsInCustodyObjList[i].Description,
                                              this.equipmentsInCustodyObjList[i].Status,
                                              Convert.ToString(this.equipmentsInCustodyObjList[i].FacilityID),
                                              Convert.ToDateTime(today),
                                              Convert.ToInt32(this.equipmentsInCustodyObjList[i].EquipmentID));
                                        }
                                    }
                                    ////added by bincey
                                    ////EquipmentCustodyControl.ShowEquipmentInformationByVisitDetailsID(propertiesDC); 
                                    //// ends here

                                    HiddenField txtFromTime = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromTime");
                                    TextBox txtToTime = (TextBox)this.VisitorLocationInformationControl.FindControl("txtToTime");
                                    HiddenField txtToDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToDate");
                                    HiddenField txtFromDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromDate");
                                    string[] todated = txtToDate.Value.Split('/');
                                    string intime = dt.Tables[0].Rows[0]["inTime"].ToString();
                                    string[] startFromDate = txtFromDate.Value.Split('/');
                                    ////dt.Tables[0].Rows[0]["FromDate"].ToString().Split('/');
                                    DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(startFromDate[0] + "/" + startFromDate[1] + "/" + startFromDate[2] + " " + txtFromTime.Value), Convert.ToString(Session["TimezoneOffset"]));
                                    ////DateTime StartDate = Convert.ToDateTime(StartFromDate[1] + "/" + StartFromDate[0] + "/" + StartFromDate[2] + " " + txtFromTime.Text);
                                    ////DateTime EndDate = Convert.ToDateTime(Todate[1] + "/" + Todate[0] + "/" + Todate[2] + " " + txtToTime.Text);
                                    DateTime endDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todated[0] + "/" + todated[1] + "/" + todated[2] + " " + txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                                    string timeStart = startDate.ToString("HH:mm");
                                    TimeSpan fromTimespan = TimeSpan.Parse(timeStart);
                                    string timeEnd = endDate.ToString("HH:mm");
                                    TimeSpan totimespan = TimeSpan.Parse(timeEnd);
                                    string strtotime = totime.ToString();
                                    bool valid = this.validations.CheckTime(startDate, endDate);
                                    if (valid == false)
                                    {
                                        this.lblError.Visible = true;
                                        ////  custError.Text = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.TimeError].ToString();
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

                                        ////inserted by bincey at conflict
                                        if (totimespan > TimeSpan.Parse(dt.Tables[0].Rows[0].ItemArray[9].ToString())
                                            && (startDate.Date == endDate.Date))
                                        {
                                            this.Extendthetime(reqId, strtotime);
                                            string strMessage = Resources.LocalizedText.TimeExtend;
                                            Page.ClientScript.RegisterClientScriptBlock(Type.GetType("System.String"), "script", "<script language='javascript'> alert('" + strMessage + "'); </script>");
                                            this.Session["MailExtendTrigger"] = "true";
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetVisitTimeExtend", "GetExtendedTime();", true);
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "TimeExtendMailtrigger", "TimeExtendMailtrigger();", true);
                                            //// RequestDetails.Timeextendedmailtohost(dt, strtotime);
                                        }
                                        ////end insertion
                                        if (endDate.Date > startDate.Date)
                                        {
                                            this.Extendthetime(reqId, strtotime);
                                            ////string strMessage = Resources.LocalizedText.TimeExtend;
                                            string strMessage = Resources.LocalizedText.UpdatedSuccessfully;
                                            Page.ClientScript.RegisterClientScriptBlock(Type.GetType("System.String"), "script", "<script language='javascript'> alert('" + strMessage + "'); </script>");

                                            //// bincey -- test -- eqpmnt custody updated successfully -- token display
                                            if (Session["EquipmentCustody"].Equals(true))
                                            {
                                                Label txtToken = (Label)this.EquipmentCustodyControl.FindControl("txttoken");
                                                Label lbltokenNumber = (Label)this.EquipmentCustodyControl.FindControl("lbltokenNumber");
                                                int token = 0;
                                                if (this.equipmentsInCustodyObjList.Count() > 0)
                                                {
                                                    if (this.equipmentsInCustodyObjList[0].TokenNumber > this.intTokenNumber)
                                                    {
                                                        token = Convert.ToInt32(this.equipmentsInCustodyObjList[0].TokenNumber) - this.intTokenNumber;
                                                        txtToken.Text = Convert.ToString(token);
                                                        txtToken.Visible = true;
                                                        lbltokenNumber.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        txtToken.Text = Convert.ToString(this.equipmentsInCustodyObjList[0].TokenNumber);
                                                        txtToken.Visible = true;
                                                        lbltokenNumber.Visible = true;
                                                    }
                                                }
                                                else
                                                {
                                                    txtToken.Text = string.Empty;
                                                }
                                            }

                                            //// bincey -- test ends -- eqpmnt custody updated successfully -- token display

                                            this.Session["MailExtendTrigger"] = "true";
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetVisitTimeExtend", "GetExtendedTime();", true);
                                            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "TimeExtendMailtrigger", "TimeExtendMailtrigger();", true);
                                            //// RequestDetails.Timeextendedmailtohost(dt, strtotime);
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
                    this.lblError2.Text = Resources.LocalizedText.RequestallowedCheck + ((HiddenField)this.VisitorLocationInformationControl.FindControl("AdvanceAllowabledays")).Value + Resources.LocalizedText.DayInAdvance;
                }
            }
            catch (VMSBL.CustomException ex)
            {
                this.DisplayError(ex.ErrorMessage, true);
                return;
            }
            catch (System.IndexOutOfRangeException)
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// requests page validations
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
                string country = visitorRequest.Country;
                ////  string City = VisitorRequest.City;
                ////  string Facility = VisitorRequest.Facility;
                string purpose = visitorRequest.Purpose;
                DateTime fromDate = visitorRequest.FromDate.Value;
                DateTime todate = visitorRequest.ToDate.Value;
                TimeSpan fromTime = visitorRequest.FromTime.Value;
                TimeSpan totime = visitorRequest.ToTime.Value;

                DateTime visitingFromDate = (DateTime)visitorEmergencyContact.FromDate;
                DateTime visitingToDate = (DateTime)visitorEmergencyContact.ToDate;

                ////added for VMS CR VMS06072010CR09 by Priti
                ////added to check duplication for equipment.
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
        /// check equipment
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
        /// <param name="fromTime">from time</param>
        /// <param name="totime">to time</param>
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
        /// <returns>string value</returns>
        public bool CheckTimewithToTimeAndFromTime(TimeSpan fromTime, TimeSpan totime, DateTime fromDate, DateTime todate)
        {
            bool isnotMatch = false;
            GenericTimeZone genrTimeZone = new GenericTimeZone();

            ////    TimeSpan tsFrom = TimeSpan.Parse(VMSUtility.VMSUtility.GetTimeToISTZone(fromTime.ToString()));

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

        #region show Equipment user controls

        /// <summary>
        /// show equipment control
        /// </summary>
        public void ShowEquipmentControl()
        {
            try
            {
                bool permitEquipments = false;
                string hostid = ((HtmlInputText)this.VisitorLocationInformationControl.FindControl("txtHost")).Value;
                if (!string.IsNullOrEmpty(hostid))
                {
                    int startIndex = hostid.IndexOf("(") + 1;
                    string visitorType = ((DropDownList)this.VisitorLocationInformationControl.FindControl("ddlPurpose")).SelectedItem.Value;
                    permitEquipments = ((CheckBox)this.VisitorLocationInformationControl.FindControl("hdnPermitEquipments")).Checked;
                    if (permitEquipments && (visitorType.ToUpper().Equals(VMSConstants.VISITORTYPECLIENTS) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEVENDOR) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEVIP) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEGUESTS) ||
                          visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEBUSINESSPARTNER) ||
                        visitorType.ToUpper().Equals(VMSConstants.VISITORTYPEAUDITORS)))
                    {
                        this.hrEquipmentControl.Visible = true;
                        this.EquipmentPermittedControl.Visible = true;
                    }
                    else
                    {
                        this.hrEquipmentControl.Visible = false;
                        this.EquipmentPermittedControl.Visible = false;
                        this.EquipmentPermittedControl.ResetEquipmentInformation(false);
                    }
                }
                else
                {
                    this.hrEquipmentcustody.Visible = true;
                    this.EquipmentControl.Visible = true;
                    this.EquipmentCustodyControl.SetInitialRow();
                }

                if (permitEquipments == true || permitEquipments.Equals(true))
                {
                    this.EquipmentCustodyControl.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

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
                this.Session["UpdateFlag"] = false;
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
                    //// Image imgPhoto = (Image)this.VisitorGeneralInformationControl.FindControl("imgphoto");
                    //// imgPhoto.Visible = true;
                    Label lblRequiredFacility = (Label)this.VisitorLocationInformationControl.FindControl("lblRequiredFacility");
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// page load complete
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

                    DropDownList ddlFacility = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlFacility");
                    this.DisableButtons(string.Empty);
                    if (Request.QueryString.ToString().Contains("details="))
                    {
                        this.Save.Enabled = false;
                        ////24/09/2009
                        this.Save.Visible = false;
                        ////Search.Enabled = false;
                        this.btnIdProof.Enabled = true;
                        //// this.btnUpload.Enabled = true;
                        this.Submit.Enabled = true;
                        this.GetVisitorInformationDetailsbyRequestID();
                        DropDownList ddlOtherPurpose = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlPurpose");
                        TextBox txtOthers = (TextBox)this.VisitorLocationInformationControl.FindControl("txtPurpose");
                        if (txtOthers.Visible == true)
                        {
                            ddlOtherPurpose.SelectedItem.Value = "Others";
                        }

                        if (this.Session["Status"] != null)
                        {
                            ////12/9/09
                            if (this.Session["Status"].ToString().Equals("Saved"))
                            {
                                this.Submit.Text = Resources.LocalizedText.Submit;
                                this.Cancel.Enabled = false;
                                this.Save.Enabled = true;
                            }
                            else if (this.Session["Status"].ToString().Equals("Submitted") || this.Session["Status"].ToString().Equals("Updated"))
                            {
                                this.Submit.Text = Resources.LocalizedText.Update;
                                this.Cancel.Enabled = true;
                            }

                            if (this.Session["Status"].ToString().Equals("Cancelled"))
                            {
                                this.Submit.Enabled = false;
                                this.Save.Enabled = false;
                                this.Cancel.Enabled = false;
                            }
                        }

                        this.Session["SaveFlag"] = true;
                        HiddenField hdnDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnDate");
                        string[] todate1 = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtToDate")).Text.Split('/');
                        //// DateTime toDate = Convert.ToDateTime(toDate1[2] + "-" + toDate1[1] + "-" + toDate1[0]);
                        string[] fromDate1 = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromDate")).Text.Split('/');
                        if (!string.IsNullOrEmpty(fromDate1[0])
                            || !string.IsNullOrEmpty(todate1[0]))
                        {
                            string fromTime = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromTime")).Text;
                            string totime = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtToTime")).Text;
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
                                // this.GenerateBadge.Enabled = true;
                            }
                            else
                            {
                                // this.GenerateBadge.Enabled = false;
                            }
                        }

                        HiddenField hdnHiddenField = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnBadgeStatus");
                        if (hdnHiddenField != null)
                        {
                            if ((hdnHiddenField.Value == "Returned") || (hdnHiddenField.Value == "Lost") ||
                                (hdnHiddenField.Value.ToUpper() == "ISSUED"))
                            {
                                //// this.GenerateBadge.Enabled = false;
                                this.btnIdProof.Enabled = false;
                                ////this.btnUpload.Enabled = false;
                                this.Save.Enabled = false;
                                this.Cancel.Enabled = false;
                                this.Reset.Enabled = false;
                            }
                            else if (string.IsNullOrEmpty(hdnHiddenField.Value))
                            {
                                ////this.btnUpload.Enabled = true;
                                this.Reset.Enabled = true;
                                this.Cancel.Enabled = true;
                                this.Submit.Enabled = true;
                                string[] strdate = hdnDate.Value.Split('/');
                                string fromTime = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromTime")).Text;
                                DateTime today = this.genTimeZone.GetCurrentDate();
                                DateTime dtdate = Convert.ToDateTime(strdate[1] + "/" + strdate[0] + "/" +
                                    strdate[2] + " " + fromTime);
                                ////  Convert.ToDateTime(strdate[1] + "/" + strdate[0] + "/" + strdate[2]);
                                if (
                                   DateTime.Compare(
                                   Convert.ToDateTime(dtdate.ToShortDateString()),
                                   Convert.ToDateTime(today.ToShortDateString())) == 0)
                                {
                                    //// this.GenerateBadge.Enabled = true;
                                }
                                else if (DateTime.Compare(
                                    Convert.ToDateTime(dtdate.ToShortDateString()),
                                           Convert.ToDateTime(today.ToShortDateString())) < 0)
                                {
                                    //// this.GenerateBadge.Enabled = false;
                                    this.btnIdProof.Enabled = false;
                                    ////this.btnUpload.Enabled = false;
                                    this.Save.Enabled = false;
                                    this.Submit.Enabled = false;
                                    this.Cancel.Enabled = false;
                                    this.Reset.Enabled = false;
                                }
                                else
                                {
                                    // this.GenerateBadge.Enabled = false;
                                }
                            }
                        }

                        this.ShowEquipmentControl();
                    }
                    else
                    {
                        ////Search.Enabled = true;
                        this.btnIdProof.Enabled = true;
                        //// this.btnUpload.Enabled = true;
                        this.Submit.Enabled = false;
                        this.Save.Enabled = true;
                        this.Submit.Text = Resources.LocalizedText.Submit;
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

                    bool permitEquipments = false;
                    DropDownList ddlCountry = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlCountry");
                    ddlCountry.Enabled = false;
                    DropDownList ddlCity = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlCity");
                    ddlCity.Enabled = false;
                    ddlFacility.Enabled = false;
                    this.ShowEquipmentControl();
                    ////bincey -- for equipment in custody
                    permitEquipments = ((CheckBox)this.VisitorLocationInformationControl.FindControl("hdnPermitEquipments")).Checked;
                    if (permitEquipments.Equals(true))
                    {
                        this.hrEquipmentcustody.Visible = false;
                        this.EquipmentControl.Visible = true;
                        ////EquipmentCustodyControl.SetInitialRow();
                        this.EquipmentCustodyControl.Visible = false;
                    }
                    else
                    {
                        this.hrEquipmentcustody.Visible = true;
                        this.EquipmentCustodyControl.Visible = true;
                    }
                }

                if (this.Session["HostChanged"] != null)
                {
                    if (this.Session["HostChanged"].ToString().ToUpper().Equals("TRUE"))
                    {
                        this.ShowEquipmentControl();
                        ////bincey -- for equipment in custody
                        bool permitEquipments = false;
                        permitEquipments = ((CheckBox)this.VisitorLocationInformationControl.FindControl("hdnPermitEquipments")).Checked;
                        if (permitEquipments.Equals(true))
                        {
                            this.hrEquipmentcustody.Visible = false;
                            this.EquipmentControl.Visible = true;
                            ////EquipmentCustodyControl.SetInitialRow();
                            this.EquipmentCustodyControl.Visible = false;
                        }
                        else
                        {
                            this.hrEquipmentcustody.Visible = true;
                            this.EquipmentCustodyControl.Visible = true;
                        }

                        this.Session["HostChanged"] = false;
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// get security city
        /// </summary>
        /// <returns>security Id</returns>
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
                ////   btnUpload.Enabled = true;
            }
        }

        /// <summary>
        /// Send Cancel Request Mail
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnCancelRequest_Click(object sender, EventArgs e)
        {
            HiddenField txtFromTime = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromTime");
            HiddenField txtToTime = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToTime");
            HiddenField txtToDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToDate");
            HiddenField txtFromDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromDate");
            string dtfromDate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromDate")).Text).ToString("MM/dd/yyyy");
            string dttoDate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControl.FindControl("txtToDate")).Text).ToString("MM/dd/yyyy");

            string fromTime = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromTime")).Text;
            string totime = ((TextBox)this.VisitorLocationInformationControl.FindControl("txtToTime")).Text;

            string[] startFromDate = dtfromDate.Split('/');

            DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(startFromDate[0] + "/" + startFromDate[1] + "/" + startFromDate[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
            //// DateTime StartDate =Convert.ToDateTime(StartFromDate[0] + "/" + StartFromDate[1] + "/" + StartFromDate[2] + " " + txtFromTime.Value))
            string[] endToDate = dttoDate.Split('/');
            DateTime endDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(endToDate[0] + "/" + endToDate[1] + "/" + endToDate[2] + " " + totime), Convert.ToString(Session["TimezoneOffset"]));
            if (this.requestDetails != null)
            {
                int visitDetailsID = Convert.ToInt32(this.hdnVisitDetailsID.Value);
                DataSet dt = this.requestDetails.Badgereturnvalues(Convert.ToString(visitDetailsID));
                string hostmailID = this.requestDetails.GetHostmailID(dt.Tables[0].Rows[0].ItemArray[0].ToString());

                VMSDataLayer.VMSDataLayer.PropertiesDC objpropertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                objpropertiesDC = this.requestDetails.DisplayInfo(visitDetailsID);

                DataTable dtlocationDetails = this.requestDetails.GetLocationDetailsById(objpropertiesDC.VisitorRequestProperty.RequestID);
                this.requestDetails.SendCancelMailtoHost(visitDetailsID, hostmailID, objpropertiesDC.VisitorMasterProperty, objpropertiesDC.VisitorRequestProperty, Convert.ToString(this.Session["LoginID"]), startDate, endDate, dtlocationDetails);
                this.DisableButtons("CANCELLED");
                ////ClientScript.RegisterStartupScript(typeof(Page), "CancelSuccess", "<script language='javascript'>alert('Request is cancelled');window.location.href = 'ViewLogbySecurity.aspx';</script>", false);
                try
                {
                    Response.Redirect("ViewLogbySecurity.aspx", true);
        
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }

        /// <summary>
        /// button time extend hidden click 
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
                        string detailsId = VMSBusinessLayer.Decrypt(Request.QueryString["details"]);
                        DataSet dt = this.requestDetails.Badgereturnvalues(detailsId);

                        string dtfromDate = DateTime.Parse(((HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromDate")).Value).ToString("MM/dd/yyyy");
                        string dttoDate = DateTime.Parse(((HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToDate")).Value).ToString("MM/dd/yyyy");

                        HiddenField txtFromTime = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnFromTime");
                        HiddenField txtToTime = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToTime");
                        HiddenField hdnTimeExtendHidden = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnToTimeBeforeExtended");
                        string[] startFromDate = dtfromDate.Split('/');
                        DateTime startDate = Convert.ToDateTime(startFromDate[0] + "/" + startFromDate[1] + "/" + startFromDate[2] + " " + txtFromTime.Value);
                        string[] extendedEndToDate = dttoDate.Split('/');
                        DateTime dtextendedEndToDateEndDate = Convert.ToDateTime(extendedEndToDate[0] + "/" + extendedEndToDate[1] + "/" + extendedEndToDate[2] + " " + txtToTime.Value);
                        ////string timeStart = StartDate.ToString("HH:mm t");

                        DateTime endDateBeforeTimeExtend = Convert.ToDateTime(extendedEndToDate[0] + "/" + extendedEndToDate[1] + "/" + extendedEndToDate[2] + " " + hdnTimeExtendHidden.Value);
                        ////string timeEnd = EndDate.ToString("HH:mm t");                                
                        this.requestDetails.Timeextendedmailtohost(dt, startDate, dtextendedEndToDateEndDate, endDateBeforeTimeExtend);
                        this.Session["MailExtendTrigger"] = "false";
                        try
                        {

                            Response.Redirect("ViewLogbySecurity.aspx", true);
                            
                        }
                        catch (System.Threading.ThreadAbortException ex)
                        {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generating batch number.
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void GenerateBadge_Click(object sender, EventArgs e)
        {
            //// added by priti on 3rd June for VMS CR VMS31052010CR6
            VMSDataLayer.VMSDataLayer.PropertiesDC objpropertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
            this.generatebadgeclick = true;
            if (this.Submit.Text.Equals(Resources.LocalizedText.Update))
            {
                this.SubmitData();
            }

            int requestID = Convert.ToInt32(Session["RequestID"].ToString());
            objpropertiesDC = this.requestDetails.DisplayInfo(requestID);

            CheckBox chkMultipleEntry = (CheckBox)this.VisitorGeneralInformationControl.FindControl("chkMultipleEntry");

            try
            {
                if (this.Session["RequestID"] != null)
                {
                    string tomailAddress = this.Session["HostMailID"].ToString();
                    //// added by priti on 3rd June for VMS CR VMS31052010CR6
                    if (objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper() == VMSConstants.REPEATVISITOR)
                    {
                        if (this.lblError.Visible == true || this.lblError1.Visible == true)
                        {
                            ////this.GenerateBadge.Attributes.Remove("onclick");
                        }
                    }
                    else
                    {
                        if (this.lblError.Visible == true || this.lblError1.Visible == true || this.lblError2.Visible == true || this.lblError3.Visible == true || this.lblError4.Visible == true)
                        {
                            //// this.GenerateBadge.Attributes.Remove("onclick");
                        }
                    }
                    //// added by priti on 3rd June for VMS CR VMS31052010CR6
                    if (objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper() == VMSConstants.REPEATVISITOR)
                    {
                        this.lblError2.Visible = false;
                        this.lblError3.Visible = false;
                        this.lblError4.Visible = false;
                        if (this.lblError.Visible == false && this.lblError1.Visible == false)
                        {
                            this.GenerateBadgeId(requestID);
                        }
                    }
                    else
                    {
                        if (this.lblError.Visible == false && this.lblError1.Visible == false && this.lblError2.Visible == false && this.lblError3.Visible == false && this.lblError4.Visible == false)
                        {
                            this.GenerateBadgeId(requestID);
                        }
                    }
                }
                else
                {
                    this.lblError.Text = Resources.LocalizedText.SubmitMandatory;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Generate badge number and update status of request as "IN"
        /// </summary>
        /// <param name="requestID">request Id</param>
        protected void GenerateBadgeId(int requestID)
        {
            ////Commented for 1C migration
            ////if (Request.QueryString.ToString().Contains("RequestID="))
            ////{
            ////    if (this.submitClick == true)
            ////    {
            ////        ////this.visitorMasterObj = (VMSBusinessEntity.VisitorMaster)HttpContext.Current.Session["VisitorMasterObj"];
            ////        ////VisitorProofObj = (VMSBusinessEntity.VisitorProof)HttpContext.Current.Session["VisitorProofObj"];
            ////        this.visitorLocObj = (VMSBusinessEntity.VisitorRequest)HttpContext.Current.Session["VisitorLocObj"];
            ////    }
            ////}
            ////else
            ////{
            ////    ////this.visitorMasterObj = (VMSBusinessEntity.VisitorMaster)HttpContext.Current.Session["VisitorMasterObj"];
            ////    ////VisitorProofObj = (VMSBusinessEntity.VisitorProof)HttpContext.Current.Session["VisitorProofObj"];
            ////    this.visitorLocObj = (VMSBusinessEntity.VisitorRequest)HttpContext.Current.Session["VisitorLocObj"];
            ////    ////VisitorEmergencyContactObj = (VMSBusinessEntity.VisitorEmergencyContact)HttpContext.Current.Session["VisitorEmergencyContactObj"];
            ////}

            string reqid = requestID.ToString();
            DataSet dt = this.requestDetails.Badgereturnvalues(reqid);
            string hostmailID = this.requestDetails.GetHostmailID(dt.Tables[0].Rows[0].ItemArray[0].ToString());
            this.BatchNo.Value = this.requestDetails.GenerateBadge(requestID, hostmailID, this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.visitorEquipmentObj, this.visitorEmergencyContactObj, this.Session["LoginID"].ToString());
            //// this.GenerateBadge.Enabled = true;
            this.count = this.count + 1;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Concat(this.visitorMasterObj.VisitorID.ToString().Trim(), "|"));
            sb.AppendLine(string.Concat(this.visitorMasterObj.FirstName.ToString().Trim() + "," + this.visitorMasterObj.LastName.ToString().Trim(), "|"));
            sb.AppendLine(string.Concat(this.visitorLocObj.HostID.ToString().Trim(), "|"));
            sb.AppendLine(string.Concat(this.visitorLocObj.Facility.ToString().Trim(), "|"));
            sb.AppendLine(string.Concat(this.visitorLocObj.FromDate.ToString().Trim(), "|"));
            sb.AppendLine(string.Concat(this.visitorLocObj.FromTime.ToString().Trim(), "|"));
            this.Session["QRimage"] = sb.ToString();
            string str1Script = string.Empty;
            if (this.EquipmentPermittedControl.Visible == true)
            {
                str1Script = "<script language='javascript'>window.open('GenerateBadge.aspx', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ');</script>";
            }
            else
            {
                str1Script = "<script language='javascript'>window.open('GenerateBadge.aspx', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ');</script>";
            }

            ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", str1Script);
            //// this.GenerateBadge.Enabled = false;
            this.btnIdProof.Enabled = false;
            //// this.btnUpload.Enabled = false;
            this.badgegenerate = true;
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
                else if (this.Session["RequestID"] != null && this.Session["Status"] == null)
                {
                    this.SaveVisitorInformation("Submitted");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// display the error message
        /// </summary>
        /// <param name="strError">string value</param>
        /// <param name="bldisplay">boolean value</param>
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

                    if (string.IsNullOrEmpty(strGE[3].ToString()))
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

                ////int tokenNumber = 0;
                this.visitorMasterObj = this.VisitorGeneralInformationControl.InsertGeneralInformation();
                this.visitorProofObj = this.VisitorGeneralInformationControl.InsertPhoto();
                this.visitorLocObj = this.VisitorLocationInformationControl.InsertLocationInformation();

                var clientOffset = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnClientOffset");
                this.visitorLocObj.Offset = clientOffset.Value;

                this.arrayofVisitDetails = this.VisitorLocationInformationControl.GetVisitDetails();
                ////try here -- bincey -- for equipment in custody
                ////string locationID = ((DropDownList)VisitorLocationInformationControl.FindControl("ddlFacility")).SelectedValue;
                ////EquipmentsInCustodyObjList = EquipmentCustodyControl.InsertEquipmentCustodyInformation(Convert.ToInt32(Request.QueryString["RequestID"]),locationID,tokenNumber);

                if (this.visitorLocObj.Purpose.Equals("Family Member- Visa processing") ||
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
                        TextBox txtToDate = (TextBox)this.VisitorLocationInformationControl.FindControl("txtToDate");
                        txtToDate.Text = this.visitorLocObj.ToDate.Value.ToString("dd/MM/yyyy");
                    }
                }
                ////  VisitorLocObj.Facility = VisitorLocObj.Facility + "|" + "Security";

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
                        if ((this.Session["Status"] == null && this.submitClick == true) && (bool)this.Session["SaveClick"] == true)
                        {
                            this.visitorLocObj.Status = "Submitted";

                            this.Session["Status"] = "Submitted";
                        }
                        else if ((this.Session["Status"] == null && this.submitClick == true) && (bool)this.Session["SaveClick"] == false)
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
                        else if (this.saveClick == true && this.Session["Status"] == null && this.Session["RequestID"] != null)
                        {
                            this.visitorLocObj.Status = "Saved";
                            status = "Saved";
                        }
                    }
                }

                this.visitorLocObj.VisitorID = this.visitorMasterObj.VisitorID;
                this.visitorEmergencyContactObj = this.EmergencyContactInformationControl.InsertEmergencyContactInformation();
                this.visitorEmergencyContactObj.RequestID = this.visitorLocObj.RequestID;
                this.visitorEquipmentObj = this.EquipmentPermittedControl.InsertEquipmentInformation(Convert.ToInt32(Request.QueryString["RequestID"]));
                ////try here -- bincey -- for equipment in custody
                ////locationID = ((DropDownList)VisitorLocationInformationControl.FindControl("ddlFacility")).SelectedValue;
                ////EquipmentsInCustodyObjList = EquipmentCustodyControl.InsertEquipmentCustodyInformation(Convert.ToInt32(Request.QueryString["RequestID"]),locationID,tokenNumber);

                ////Commented for 1C migration
                ////HttpContext.Current.Session["VisitorMasterObj"] = this.visitorMasterObj;
                ////HttpContext.Current.Session["VisitorProofObj"] = this.visitorProofObj;
                ////HttpContext.Current.Session["VisitorLocObj"] = this.visitorLocObj;
                ////HttpContext.Current.Session["VisitorEmergencyContactObj"] = this.visitorEmergencyContactObj;
                ////HttpContext.Current.Session["VisitorEquipmentObj"] = this.visitorEquipmentObj;
                HttpContext.Current.Session["SaveFlag"] = true;
                ////HttpContext.Current.Session["VisitorEquipmentCustodyObj"] = this.equipmentsInCustodyObjList;

                if (status.Equals("Saved"))
                {
                    if (this.Session["Status"] != null)
                    {
                        if (this.saveClick == true && this.Session["Status"].ToString().Equals("Saved"))
                        {
                            this.UpdateVisitorInformation();
                        }
                    }
                    else if (this.Session["Status"] == null)
                    {
                        if (this.saveClick == true && this.Session["Status"] == null && this.Session["RequestID"] == null)
                        {
                            clientOffset = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnClientOffset");
                            this.visitorLocObj.Offset = clientOffset.Value;

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
                        else if (this.saveClick == true && this.Session["Status"] == null && this.Session["RequestID"] != null)
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Reset click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Reset_Click(object sender, EventArgs e)
        {
            this.ResetData();
            //// this.GenerateBadge.Enabled = false;
            this.Cancel.Enabled = false;
        }

        /// <summary>
        /// search click
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Cancel click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Session["RequestID"] != null)
                {
                    int reqID = Convert.ToInt32(this.Session["RequestID"].ToString());
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

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
        /// Disable buttons
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
                            // this.btnWebcam.Disabled = false;
                            // this.GenerateBadge.Enabled = true;                            
                            break;
                        }

                    case "IN":
                        {
                            ////  this.btnWebcam.Disabled = true;
                            ////  this.btnUpload.Enabled = false;
                            this.btnIdProof.Enabled = false;
                            //// this.GenerateBadge.Enabled = false; 
                            break;
                        }

                    case "CANCELLED":
                        {
                            //// this.btnWebcam.Disabled = true;
                            this.btnIdProof.Enabled = false;
                            //// this.btnUpload.Enabled = false;
                            ////this.Search.Enabled = false;
                            this.Save.Enabled = false;
                            this.Reset.Enabled = false;
                            this.Cancel.Enabled = false;
                            ////this.GenerateBadge.Enabled = false;
                            this.Submit.Enabled = false;
                            break;
                        }

                    case "OUT":
                        {
                            ////this.btnWebcam.Disabled = true;
                            this.btnIdProof.Enabled = false;
                            //// this.btnUpload.Enabled = false;
                            //////this.Search.Enabled = false;
                            this.Save.Enabled = false;
                            this.Reset.Enabled = false;
                            this.Cancel.Enabled = false;
                            //// this.GenerateBadge.Enabled = false;
                            this.Submit.Enabled = false;
                            break;
                        }

                    default:
                        {
                            //// this.btnWebcam.Disabled = false;
                            this.btnIdProof.Enabled = false;
                            ////this.btnUpload.Enabled = true;
                            ////this.Search.Enabled = true;
                            this.Save.Enabled = false;
                            this.Reset.Enabled = true;
                            this.Cancel.Enabled = false;
                            //// this.GenerateBadge.Enabled = false;
                            this.Submit.Enabled = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        // addition for CR IRVMS22062010CR07  starts here done by Priti

        /// <summary>
        /// button hidden click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Request.QueryString.ToString().Contains("RequestID="))
                {
                    if (((CheckBox)this.VisitorGeneralInformationControl.FindControl("chkMultipleEntry")).Checked == true)
                    {
                        this.ResetVisitorInformation(false);
                        //// this.btnUpload.Enabled = true;
                        this.btnIdProof.Enabled = true;
                        this.Session["RequestID"] = null;
                    }
                    else
                    {
                        this.ResetVisitorInformation(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// button hidden click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnHidden1_Click(object sender, EventArgs e)
        {
            CheckBox chkMultipleEntries = (CheckBox)this.VisitorGeneralInformationControl.FindControl("chkMultipleEntry");
            if (chkMultipleEntries.Checked)
            {
                this.ResetVisitorInformation(false);
                this.DisableButtons(string.Empty);
            }
            else
            {
                try
                {
                    Response.Redirect("ViewLogbySecurity.aspx",true);
                   
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
                    this.BatchNo.Value = this.requestDetails.GenerateBadge(Convert.ToInt32(this.hdnVisitDetailsID.Value), hostmailID, this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.visitorEquipmentObj, this.visitorEmergencyContactObj, this.Session["LoginID"].ToString());
                    string strScript = "<script language='javascript'>window.open('GenerateBadge.aspx?key=" + XSS.HtmlEncode(this.hdnVisitDetailsID.Value) + "', 'List', 'scrollbars=no,resizable=no,width=780,height=330, location=center ');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript); ////Added for CR 17 Fix,12 May 2011 Tincy
                                                                                                        ////  this.GenerateBadge.Enabled = false;
                    this.btnIdProof.Enabled = false;
                    ////this.btnUpload.Enabled = true;
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        ///// <summary>
        ///// button check in click
        ///// </summary>
        ///// <param name="sender">sender parameter</param>
        ///// <param name="e">event parameter</param>
        ////protected void BtnCheckIn_Click(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        string securityID = Session["LoginID"].ToString();
        ////        this.lblError.Visible = false;
        ////        if (!string.IsNullOrEmpty(this.hdnVisitDetailsID.Value))
        ////        {
        ////            this.DisableButtons("IN");
        ////            ////if (RequestDetails.VisitorCheckIn(Convert.ToInt32(hdnVisitDetailsID.Value.ToString().Trim()), securityID))
        ////            ////{
        ////                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.VisitorBadgeGenerated + "'); </script>");
        ////                string strScript = "<script language='javascript'>(window.open('VMSBadge.aspx?key=" + XSS.HtmlEncode(this.hdnVisitDetailsID.Value.ToString().Trim()) + "', 'List', 'scrollbars=no,resizable=no,width=780,height=370, location=center ')).focus();</script>";
        ////                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript);
        ////                return;
        ////        }
        ////        else
        ////        {
        ////            this.lblError.Text = Resources.LocalizedText.SubmitMandatory;
        ////            this.lblError.Visible = true;
        ////        }

        ////        return;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
        ////    }
        ////}

        /// <summary>
        /// button check in click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnNotify_Click(object sender, EventArgs e)
        {
            try
            {
                int visitDetailsID = Convert.ToInt32(VMSBusinessLayer.Decrypt(Request.QueryString["details"]));
                string securityID = Session["LoginID"].ToString();
                this.lblError.Visible = false;
                if (!string.IsNullOrEmpty(this.hdnVisitDetailsID.Value))
                {
                    this.DisableButtons("IN");
                    ////if (RequestDetails.VisitorCheckIn(Convert.ToInt32(hdnVisitDetailsID.Value.ToString().Trim()), securityID))
                    ////{
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.VisitorBadgeGenerated + "'); </script>");
                    string strScript = "<script language='javascript'>(window.open('VMSBadge.aspx?key=" + XSS.HtmlEncode(this.hdnVisitDetailsID.Value.ToString().Trim()) + "', 'List', 'scrollbars=no,resizable=no,width=780,height=370, location=center ')).focus();</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadBadgepage", strScript);
                    return;
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// back click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Back_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ClientVisit.aspx", true);
                 
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
        }

        /// <summary>
        /// Reset Visitor information
        /// </summary>
        /// <param name="isreset">Is reset value</param>
        private void ResetVisitorInformation(bool isreset)
        {
            DropDownList ddlCountry = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlCountry");
            DropDownList ddlCity = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlCity");
            DropDownList ddlFacility = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlFacility");

            ddlFacility.Enabled = true;
            if (isreset == false & (((CheckBox)this.VisitorGeneralInformationControl.FindControl("chkMultipleEntry")).Checked == true))
            {
                this.VisitorGeneralInformationControl.ResetGeneralInformation(true);
                this.VisitorLocationInformationControl.ResetLocationInformation(true);
                this.EmergencyContactInformationControl.ResetEmergencyContactInformation();
                this.EquipmentPermittedControl.ResetEquipmentInformation(true);
                this.EquipmentCustodyControl.ResetEquipmentCustodyInformation(true);
            }
            else
            {
                this.VisitorGeneralInformationControl.ResetGeneralInformation(false);
                this.VisitorLocationInformationControl.ResetLocationInformation(false);
                this.EmergencyContactInformationControl.ResetEmergencyContactInformation();
                this.EquipmentPermittedControl.ResetEquipmentInformation(false);
                this.EquipmentCustodyControl.ResetEquipmentCustodyInformation(false);
            }

            ddlCity.Enabled = false;
            ddlCountry.Enabled = false;
            ddlFacility.Enabled = false;
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
            ////this.btnUpload.Enabled = true;
        }

        /// <summary>
        /// Get visitor information details by visitor Id
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
                        visitorID = int.Parse(this.Session["SearchVisitorID"].ToString());
                    }

                    this.Session["SearchVisitorID"] = null;
                    if (this.requestDetails != null)
                    {
                        objpropertiesDC = this.requestDetails.GetSearchDetails(visitorID);
                        if (objpropertiesDC != null)
                        {
                            if (objpropertiesDC.VisitorMasterProperty != null)
                            {
                                this.VisitorGeneralInformationControl.ShowGeneralInformationByRequestID(objpropertiesDC);
                                this.Session["VisitorID"] = objpropertiesDC.VisitorMasterProperty.VisitorID;
                            }

                            if (objpropertiesDC.VisitorProofProperty != null)
                            {
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Get visitor information details by request Id
        /// </summary>
        private void GetVisitorInformationDetailsbyRequestID()
        {
            int visitDetailsID;
            VMSDataLayer.VMSDataLayer.PropertiesDC objpropertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
            try
            {
                visitDetailsID = Convert.ToInt32(VMSBusinessLayer.Decrypt(Request.QueryString["details"]));
                if (this.requestDetails != null)
                {
                    objpropertiesDC = this.requestDetails.DisplayInfo(visitDetailsID);
                    if (objpropertiesDC.VisitorMasterProperty != null)
                    {
                        this.VisitorGeneralInformationControl.ShowGeneralInformationByRequestID(objpropertiesDC);
                        this.Session["VisitorID"] = objpropertiesDC.VisitorMasterProperty.VisitorID;
                    }

                    if (objpropertiesDC.VisitorRequestProperty != null)
                    {
                        this.VisitorLocationInformationControl.ShowLocationInformationByRequestID(objpropertiesDC);
                        this.Session["RequestID"] = objpropertiesDC.VisitorRequestProperty.RequestID;
                        this.Session["Status"] = objpropertiesDC.VisitorRequestProperty.Status;
                    }

                    if (objpropertiesDC.VisitorEquipmentProperty != null)
                    {
                        this.EquipmentPermittedControl.ShowEquipmentInformationByRequestID(objpropertiesDC);
                    }

                    ////add code for eqpmnt in custody code here -- bincey
                    if (objpropertiesDC.VisitorEquipmentProperty.Count == 0)
                    {
                        ////add eqpmntcustody.
                        this.Session["showequipmentcustody"] = true;
                        this.EquipmentCustodyControl.ShowEquipmentInformationByVisitDetailsID(objpropertiesDC);
                    }

                    if (objpropertiesDC.VisitorEmergencyContactProperty != null)
                    {
                        this.EmergencyContactInformationControl.ShowEmergencyContactInformationByRequestID(objpropertiesDC);
                    }

                    string badgeStatus = string.Empty;
                    if (!string.IsNullOrEmpty(objpropertiesDC.VisitDetailProperty.BadgeStatus))
                    {
                        badgeStatus = objpropertiesDC.VisitDetailProperty.BadgeStatus.ToUpper();
                    }

                    this.VisitorGeneralInformationControl.DisableVisitGeneralInformationControls(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper());
                    this.VisitorLocationInformationControl.DisableVisitLocationInformationControls(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper(), badgeStatus);
                    this.EquipmentPermittedControl.DisableEquipmentControl(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper(), badgeStatus);
                    this.EmergencyContactInformationControl.DisableEmergencyContactInformationControl(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper(), badgeStatus);
                    this.EquipmentCustodyControl.DisableEquipmentCustody(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper(), badgeStatus);
                    this.hdnVisitDetailsID.Value = visitDetailsID.ToString();
                    this.DisableButtons(objpropertiesDC.VisitorRequestProperty.RequestStatus.ToUpper());
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
        /// Update visitor information
        /// </summary>
        private void UpdateVisitorInformation()
        {
            try
            {
                IdentityDetails identityDetails = this.GetIdentityDetailsForArgentina();
                int tokenNumber = 0;
                string locationID = ((DropDownList)this.VisitorLocationInformationControl.FindControl("ddlFacility")).SelectedValue;
                this.Session["UpdateFlag"] = true;
                int equipcustody = 0;
                int countCust = 0;
                DataTable dttokenDetails = new DataTable();
                ////Session["RequestID"] = VisitorLocObj.RequestID;
                string visitDetailsID = Convert.ToString(this.Session["VisitDetailID"]);
                VMSBusinessLayer.UserDetailsBL userDetailsBLObj = new VMSBusinessLayer.UserDetailsBL();
                DataTable dtvisitDetailsID = new DataTable();
                if (this.Session["UpdateFlag"].Equals(true))
                {
                    if (this.Session["EquipmentCustody"].Equals(true))
                    {
                        dttokenDetails = userDetailsBLObj.GetTokenDetails(Convert.ToInt32(this.Session["VisitDetailID"]));
                        if (dttokenDetails.Rows.Count != 0)
                        {
                            tokenNumber = Convert.ToInt32(dttokenDetails.Rows[0]["TokenNumber"]);
                        }
                        else
                        {
                            tokenNumber = userDetailsBLObj.GetToken(Convert.ToInt32(visitDetailsID), locationID);
                        }
                    }
                }

                this.visitorMasterObj = this.VisitorGeneralInformationControl.InsertGeneralInformation();
                //// int accesscardnumber = this.VisitorGeneralInformationControl.GetAccessCardDetails();
                this.visitorMasterObj.VisitorID = Convert.ToInt32(this.Session["VisitorID"]);
                this.visitorLocObj = this.VisitorLocationInformationControl.InsertLocationInformation();

                this.arrayofVisitDetails = this.VisitorLocationInformationControl.GetVisitDetailsByRequestID(this.Session["RequestID"].ToString());

                this.arrayofVisitDetails[0].VisitDetailsID = Convert.ToInt32(this.Session["VisitDetailID"]);
                HiddenField hdnDate = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnDate");

                if (this.visitorLocObj.Purpose.Equals("Family Member- Visa processing") ||
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
                        TextBox txtToDate = (TextBox)this.VisitorLocationInformationControl.FindControl("txtToDate");
                        txtToDate.Text = this.visitorLocObj.ToDate.Value.ToString("dd/MM/yyyy");
                    }
                }

                this.visitorLocObj.VisitorID = Convert.ToInt32(this.Session["VisitorID"]);
                this.visitorLocObj.Facility = this.visitorLocObj.Facility + "|" + "Security";

                if (this.Session["Status"] != null)
                {
                    this.visitorLocObj.Status = this.Session["Status"].ToString();
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

                ////insert new photo
                this.visitorProofObj = this.VisitorGeneralInformationControl.InsertPhoto();
                this.visitorProofObj.VisitorID = Convert.ToInt32(this.Session["VisitorID"]);
                this.visitorEquipmentObj = this.EquipmentPermittedControl.InsertEquipmentInformation(Convert.ToInt32(this.Session["RequestID"]));
                this.visitorEmergencyContactObj = this.EmergencyContactInformationControl.InsertEmergencyContactInformation();
                this.visitorEmergencyContactObj.RequestID = Convert.ToInt32(this.Session["RequestID"]);
                var clientOffset = (HiddenField)this.VisitorLocationInformationControl.FindControl("hdnClientOffset");
                this.visitorLocObj.Offset = clientOffset.Value;

                if (this.Session["EquipmentCustody"].Equals(true))
                {
                    equipcustody = 1;
                }

                if (this.visitorLocObj.RequestStatus.ToUpper().Equals(VMSConstants.REPEATVISITOR))
                {
                    this.successSubmission = this.requestDetails.EditVisitorInformationForRepeatVisitor(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, equipcustody);
                }
                else
                {
                    this.successSubmission = this.requestDetails.EditVisitorInformation(this.visitorProofObj, this.visitorMasterObj, this.visitorLocObj, this.arrayofVisitDetails, this.visitorEquipmentObj, this.visitorEmergencyContactObj, equipcustody, identityDetails);
                    ////this.requestDetails.UpdateAccessCardDetails(Convert.ToInt32(this.Session["RequestID"]), this.visitorMasterObj.VisitorID, this.visitorMasterObj.FirstName, accesscardnumber);
                    dtvisitDetailsID = userDetailsBLObj.GetVisitDetailsID(Convert.ToString(this.Session["RequestID"]));
                    if (dtvisitDetailsID.Rows.Count > 0)
                    {
                        visitDetailsID = dtvisitDetailsID.Rows[0]["VisitDetailsID"].ToString();
                    }
                    ////if (Session["EquipmentCustody"].Equals(true))
                    ////{

                    if (this.equipmentsInCustodyObjList.Count() > 0)
                    {
                        this.equipmentsInCustodyObjList = this.EquipmentCustodyControl.InsertEquipmentCustodyInformation(Convert.ToInt32(visitDetailsID), locationID, tokenNumber);
                        countCust = this.equipmentsInCustodyObjList.Length;
                        for (int i = 0; i < countCust; i++)
                        {
                            userDetailsBLObj.SubmitEquipmentCustody(
                              Convert.ToInt32(this.equipmentsInCustodyObjList[i].VisitDetailsID),
                              this.equipmentsInCustodyObjList[i].TokenNumber,
                              this.equipmentsInCustodyObjList[i].EquipmentType,
                              this.equipmentsInCustodyObjList[i].Description,
                              this.equipmentsInCustodyObjList[i].Status,
                              Convert.ToString(this.equipmentsInCustodyObjList[i].FacilityID),
                              Convert.ToDateTime(this.visitorLocObj.CreatedDate),
                              Convert.ToInt32(this.equipmentsInCustodyObjList[i].EquipmentID));
                            ////}
                        }
                    }
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
                    ////add here token display
                    ////((TextBox)VisitorLocationInformationControl.FindControl("txttoken")).Text = Convert.ToString(EquipmentsInCustodyObjList[0].TokenNumber);
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

                        Label txtToken = (Label)this.EquipmentCustodyControl.FindControl("txttoken");
                        Label lbltokenNumber = (Label)this.EquipmentCustodyControl.FindControl("lbltokenNumber");
                        int token = 0;
                        if (Session["EquipmentCustody"].Equals(true))
                        {
                            if (this.equipmentsInCustodyObjList.Count() > 0)
                            {
                                if (this.equipmentsInCustodyObjList[0].TokenNumber > this.intTokenNumber)
                                {
                                    token = Convert.ToInt32(this.equipmentsInCustodyObjList[0].TokenNumber) - this.intTokenNumber;
                                    txtToken.Text = Convert.ToString(token);
                                    txtToken.Visible = true;
                                    lbltokenNumber.Visible = true;
                                }
                                else
                                {
                                    txtToken.Text = Convert.ToString(this.equipmentsInCustodyObjList[0].TokenNumber);
                                    txtToken.Visible = true;
                                    lbltokenNumber.Visible = true;
                                }
                            }
                            else
                            {
                                txtToken.Text = string.Empty;
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

                    TextBox txtFromTime = (TextBox)this.VisitorLocationInformationControl.FindControl("txtFromTime");
                    DateTime fromtime = Convert.ToDateTime(txtFromTime.Text.ToString());

                    string dtfromDate = DateTime.Parse(((TextBox)this.VisitorLocationInformationControl.FindControl("txtFromDate")).Text).ToString("MM/dd/yyyy");
                    string[] fromdate = dtfromDate.Split('/');
                    DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate[0] + "/" + fromdate[1] + "/" + fromdate[2] + " " + txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                    DateTime dtcurrentTime = this.genTimeZone.GetCurrentDate();
                    if ((DateTime.Compare(
                        this.visitorLocObj.FromDate.Value, dtcurrentTime.Date) <= 0)
                         && (DateTime.Compare(DateTime.ParseExact(hdnDate.Value, "dd/MM/yyyy", null).Date, dtcurrentTime.Date) == 0)
                       && (DateTime.Compare(this.visitorLocObj.FromDate.Value, dtcurrentTime.Date) >= 0)
                       && this.visitstatus != "IN"
                       && this.visitstatus != "CANCELLED"
                       && this.visitstatus != "OUT"
                       && this.visitstatus != "YET TO ARRIVE"
                       && Convert.ToDateTime(dtcurrentTime.ToString("H:mm")) <= startDate.AddMinutes(10))
                    {
                        //// this.GenerateBadge.Enabled = true;
                        VMSBusinessEntity.VisitDetail visitDetail1 = this.arrayofVisitDetails[0];
                        this.hdnVisitDetailsID.Value = visitDetail1.VisitDetailsID.ToString();
                        //// this.btnWebcam.Disabled = true;
                        //// this.btnUpload.Enabled = false;
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('" + Resources.LocalizedText.UpdatedSuccessfully + "'); </script>");
                        this.lblSubmitSuccess.Text = Resources.LocalizedText.UpdatedSuccessfully;
                        this.lblSubmitSuccess.Visible = true;
                        VMSBusinessEntity.VisitDetail visitDetail1 = this.arrayofVisitDetails[0];
                        this.hdnVisitDetailsID.Value = visitDetail1.VisitDetailsID.ToString();
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
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Get Identity details
        /// </summary>
        /// <returns>string value</returns>
        private IdentityDetails GetIdentityDetailsForArgentina()
        {
            IdentityDetails identityDetails = null;
            DropDownList ddlCountry = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlCountry");
            DropDownList ddlIdentityType = (DropDownList)this.VisitorLocationInformationControl.FindControl("ddlIdentityType");
            TextBox txtIdentityNo = (TextBox)this.VisitorLocationInformationControl.FindControl("txtIdentityNo");
            if (ddlCountry.SelectedItem.Text == "Argentina")
            {
                if (ddlIdentityType.SelectedItem != null)
                {
                    if (ddlIdentityType.SelectedItem.Text != "Select")
                    {
                        identityDetails = new IdentityDetails();
                        identityDetails.IdentityType = ddlIdentityType.SelectedItem.Text.Trim();
                        identityDetails.IdentityNo = txtIdentityNo.Text.Trim();
                    }
                }
            }

            return identityDetails;
        }
    }
}
