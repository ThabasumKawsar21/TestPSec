//-----------------------------------------------------------------------
// <copyright file="EmergencyContactInformationSP.ascx.cs" company="MyCompanyName">
//     Copyright (c) MyCompanyName. All rights reserved.
// </copyright>
// <summary>
// This file contains EmergencyContactInformationSP class.
// </summary>
//-----------------------------------------------------------------------

namespace VMSDev.SafetyPermitUserControls
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using VMSBusinessEntity;
    using VMSBusinessLayer;

    /// <summary>
    /// Emergency contact information
    /// </summary> 
    public partial class EmergencyContactInformationSP : System.Web.UI.UserControl
    {
        #region variables

        /// <summary>
        /// Request details
        /// </summary>   
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// Time zone
        /// </summary>   
        private GenericTimeZone genTimeZone = new GenericTimeZone();

        #endregion

        /// <summary>
        /// insert emergency contact information
        /// </summary>
        /// <returns>visitor contact</returns>
        public VisitorEmergencyContact InsertEmergencyContactInformation()
        {
            VMSBusinessEntity.VisitorEmergencyContact visitorEmergencyContact = new VisitorEmergencyContact();
            if (!this.txtVisitingFromDate.Text.Equals(string.Empty) || this.txtVisitingToDate.Text.Equals(string.Empty))
            {
                string[] fromdate = this.txtVisitingFromDate.Text.Split('/');
                string[] todate = this.txtVisitingToDate.Text.Split('/');
                string dtfromTime = this.hdnFromTime.Value;
                ////DateTime startDate = genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(Fromdate[1] + "/" + Fromdate[0] + "/" + Fromdate[2] + " " + dtFromTime), Convert.ToString(Session["TimezoneOffset"]));
                ////DateTime endDate = genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(Todate[1] + "/" + Todate[0] + "/" + Todate[2] + " " + dtFromTime), Convert.ToString(Session["TimezoneOffset"]));
                DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate[0] + "/" + fromdate[1] + "/" + fromdate[2] + " " + dtfromTime), Convert.ToString(Session["TimezoneOffset"]));
                DateTime endDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate[0] + "/" + todate[1] + "/" + todate[2] + " " + dtfromTime), Convert.ToString(Session["TimezoneOffset"]));
                visitorEmergencyContact.FromDate = startDate;
                visitorEmergencyContact.ToDate = endDate;
                visitorEmergencyContact.Address = this.txtAddress.Text.Trim();
            }
            else
            {
                ////VisitorEmergencyContact.FromDate = VMSConstants.VMSConstants.currentDate;
                ////VisitorEmergencyContact.ToDate = VMSConstants.VMSConstants.currentDate;
                visitorEmergencyContact.FromDate = this.genTimeZone.GetCurrentDate();
                visitorEmergencyContact.ToDate = this.genTimeZone.GetCurrentDate();
            }

            return visitorEmergencyContact;
        }

        /// <summary>
        /// Description: Method to Initialize Default current calendar date and Time
        /// </summary>
        public void InitDates()
        {
            ////FromDateCalendar.SelectedDate = VMSConstants.VMSConstants.currentDate;
            ////ToDateCalendar.SelectedDate = VMSConstants.VMSConstants.currentDate;
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetEmergencyDefaultTime", "SetEmergencyDefaultTime();", true);
        }

        /// <summary>
        /// Reset emergency contact information
        /// </summary>      
        public void ResetEmergencyContactInformation()
        {
            this.txtAddress.Text = string.Empty;
            if (Request.QueryString.ToString().Contains("RequestID="))
            {
            }
            else
            {
                this.FromDateCalendar.SelectedDate = DateTime.Now;
                this.ToDateCalendar.SelectedDate = DateTime.Now;
            }
        }

        /// <summary>
        /// The ShowEmergencyContactInformationByRequestID method
        /// </summary>
        /// <param name="propertiesDc">The propertiesDc parameter</param>        
        public void ShowEmergencyContactInformationByRequestID(VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDc)
        {
            try
            {
                this.txtAddress.Text = propertiesDc.VisitorEmergencyContactProperty.Address;
                this.txtVisitingFromDate.Text = Convert.ToDateTime(propertiesDc.VisitorEmergencyContactProperty.FromDate).ToShortDateString();
                this.txtVisitingToDate.Text = Convert.ToDateTime(propertiesDc.VisitorEmergencyContactProperty.ToDate).ToShortDateString();
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetEmergencyTimeByRequestId", "SetEmergencyTimeByRequestId();", true);
                this.hdnFromDateTime.Value = Convert.ToDateTime(propertiesDc.VisitorEmergencyContactProperty.FromDate).ToShortTimeString();
                this.hdnToDateTime.Value = Convert.ToDateTime(propertiesDc.VisitorEmergencyContactProperty.ToDate).ToShortTimeString();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The DisableEmergencyContactInformationControl method
        /// </summary>
        /// <param name="requestStatus">The RequestStatus parameter</param>
        /// <param name="badgeStatus">The BadgeStatus parameter</param>        
        public void DisableEmergencyContactInformationControl(string requestStatus, string badgeStatus)
        {
            ////changed by priti on 3rd June for VMS CR VMS31052010CR6
            ////changed by priti on 15th June for VMS CR VMS31052010CR6 code review defects
            if ((badgeStatus == "ISSUED") || (badgeStatus == "RETURNED") ||
                    (badgeStatus == "LOST") || (requestStatus == "CANCELLED"))
            {
                this.txtAddress.Enabled = false;
                this.txtVisitingFromDate.Enabled = false;
                this.txtVisitingToDate.Enabled = false;
                this.imgFromDate.Enabled = false;
                this.imgToDate.Enabled = false;
            }
            else if (string.IsNullOrEmpty(badgeStatus))
            {
                this.EnableEmergencyContactInformationControl();
            }
        }

        /// <summary>
        /// Enable emergency contact information
        /// </summary>
        public void EnableEmergencyContactInformationControl()
        {
            this.txtAddress.Enabled = true;
            this.txtVisitingFromDate.Enabled = true;
            this.txtVisitingToDate.Enabled = true;
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtVisitingFromDate.Attributes.Add("readonly", "readonly");
            this.txtVisitingToDate.Attributes.Add("readonly", "readonly");

            try
            {
                if (this.Session["LoginID"] == null)
                {
                    return;
                }

                if (!Page.IsPostBack)
                {
                    if (Request.QueryString.ToString().Contains("RequestID="))
                    {
                    }
                    else
                    {
                        this.InitDates();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
