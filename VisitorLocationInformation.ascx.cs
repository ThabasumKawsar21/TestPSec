
namespace VMSDev.UserControls
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Security;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml;
    using System.Xml.Linq;
    using VMSBL;
    using VMSBusinessEntity;
    using VMSBusinessLayer;
    using VMSConstants;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class Visitor Location Information
    /// </summary>   
    public partial class VisitorLocationInformation : System.Web.UI.UserControl
    {
        #region "Variables"

        /// <summary>
        /// The AssociateDetails field
        /// </summary>        
        private List<string> associateDetails = new List<string>();

        /// <summary>
        /// The User ID field
        /// </summary>        
        private string userID;

        /// <summary>
        /// The CountryID field
        /// </summary>        
        private string strCountryID;

        /// <summary>
        /// The Location Details field
        /// </summary>        
        private VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();

        /// <summary>
        /// The VisitorRequest field
        /// </summary>        
        private VMSBusinessEntity.VisitorRequest visitorRequest = new VMSBusinessEntity.VisitorRequest();

        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// The UserDetails field
        /// </summary>        
        private VMSBusinessLayer.UserDetailsBL userDetails = new VMSBusinessLayer.UserDetailsBL();

        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();
        #endregion

        ////added for VMS CR VMS06072010CR09 by Priti
        #region "Equipemnt visibility Fill Event handling"

        /// <summary>
        /// The BubbleIndexChanged event
        /// </summary>        
        public event EventHandler BubbleIndexChanged;

        /// <summary>
        /// The Assign Time Zone Off set Location method
        /// </summary>
        /// <param name="strTimezoneoffset">The Time zone off set parameter</param>        
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void AssignTimeZoneOffsetLocation(string strTimezoneoffset)
        {
            if (!string.IsNullOrEmpty(strTimezoneoffset))
            {
                HttpContext.Current.Session["TimezoneOffset"] = strTimezoneoffset;
            }
            else
            {
                HttpContext.Current.Session["TimezoneOffset"] = "0";
            }
        }

        /// <summary>
        /// The AssignCurrentDateTimeLocation method
        /// </summary>
        /// <param name="currentDate">The currentDate parameter</param>        
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void AssignCurrentDateTimeLocation(string currentDate)
        {
            if (!string.IsNullOrEmpty(currentDate))
            {
                HttpContext.Current.Session["currentDateTime"] = currentDate;
            }
            else
            {
                HttpContext.Current.Session["currentDateTime"] = "0";
            }
        }

        /// <summary>
        /// Description: Method to Initialize Default current calendar date and Time
        /// </summary>
        public void InitDates()
        {
            try
            {
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetDefaultTime", "SetDefaultTime();", true);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Description: Method to Initialize Country, Cities and 
        /// </summary>
        public void InitCountries()
        {
            try
            {
                VMSBusinessLayer.MasterDataBL masterData = new VMSBusinessLayer.MasterDataBL();
                VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable dtsecurityCity = new DataTable();
                if (this.Session["LoginID"] != null)
                {
                    this.userID = this.Session["LoginID"].ToString();
                    DataTable dtcountries = new DataTable();
                    dtcountries = objlocationDetails.GetActiveCountry();
                    dtsecurityCity = this.requestDetails.GetSecurityCity(this.userID).Tables[0];
                    this.strCountryID = Convert.ToString(dtsecurityCity.Rows[0]["CountryId"]);
                    string locationId = Convert.ToString(dtsecurityCity.Rows[0]["LocationId"]);
                    this.ddlCountry.DataSource = dtcountries;
                    this.ddlCountry.DataTextField = "Country";
                    this.ddlCountry.DataValueField = "CountryId";
                    this.ddlCountry.DataBind();
                    if ((this.ddlCountry.Items.Count == 0) || (this.ddlCountry.Items.Count > 0))
                    {
                        this.ddlCountry.Items.Insert(0, new ListItem("Select", "0"));
                    }

                    this.ddlCountry.ClearSelection();
                    this.ddlCountry.SelectedIndex = this.ddlCountry.Items.IndexOf(this.ddlCountry.Items.FindByValue(this.strCountryID.ToString()));

                    this.LoadTimeZone(this.ddlCountry.SelectedItem.Text.Trim());

                    this.ddlCountry.Enabled = false;
                    if (this.ddlCountry.SelectedItem.Text == "Argentina")
                    {
                        this.tblIdentity.Visible = true;
                    }
                    else
                    {
                        this.tblIdentity.Visible = false;
                    }

                    this.GetCities(dtsecurityCity);
                }

                // ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The get Advance method
        /// </summary>
        /// <returns>The string type object</returns>        
        public string GetAdvanceAllowabledays()
        {
            try
            {
                string date = string.Empty;
                if (!this.ddlPurpose.SelectedItem.Value.ToUpper().Contains("SELECT"))
                {
                    ////AdvanceAllowabledays.Value = LocationDetails.getAdvanceAllowabledays(ddlCity.SelectedValue.ToString()).ToString();
                    this.AdvanceAllowabledays.Value = this.locationDetails.GetAdvanceAllowabledays(XSS.HtmlEncode(this.ddlPurpose.SelectedItem.Value.ToString())).ToString();
                    date = this.AdvanceAllowabledays.Value;
                }

                return date;

                ////else
                ////{

                ////   // AdvanceAllowabledays.Value = "0";
                ////}
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// The OnBubbleSelectedIndexChange method
        /// </summary>
        /// <param name="sender">The Sender parameter</param>
        /// <param name="e">The e parameter</param>        
        public void OnBubbleSelectedIndexChange(object sender, EventArgs e)
        {
            if (this.BubbleIndexChanged != null)
            {
                this.BubbleIndexChanged(sender, e);
            }
        }

        #endregion

        /// <summary>
        /// The GetUserDetails method
        /// </summary>
        /// <param name="struserID">The UserID parameter</param>        
        public void GetUserDetails(string struserID)
        {
            try
            {
                VMSBusinessLayer.UserDetails<string, string, string, string> objuserDetails = new VMSBusinessLayer.UserDetails<string, string, string, string>();
                VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                objuserDetails = userDetailsBL.GetUserDetails(struserID);
                //this.txtHost.Value = objuserDetails.AssociateName;
                //this.txtHost.Value += " (" + struserID + ")";
                //this.hdnSelectedHost.Value = this.txtHost.Value;
                //this.txtHostContactNo.Text = objuserDetails.Vnet;

                if (this.txtHostContactNo.Text.Trim().Length > 0)
                {
                    this.txtHostContactNo.Visible = true;
                    Session.Add("HostNo", objuserDetails.Vnet);
                }

                this.hidHostDeptDesc.Value = objuserDetails.DeptDesc;
                Session.Add("HostMailID", objuserDetails.MailID);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Populate purpose data
        /// </summary>
        public void PopulatePurpose()
        {
            try
            {
                string visitorType = Resources.LocalizedText.SelectVisitorType;
                this.XmlPurpose.Data = Resources.LocalizedText.PurposeList;
                this.XmlPurpose.EnableCaching = false;
                this.XmlPurpose.DataBind();
                this.ddlPurpose.DataSourceID = "XmlPurpose";
                this.ddlPurpose.DataTextField = "text";
                this.ddlPurpose.DataValueField = "value";
                this.ddlPurpose.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GetCities method
        /// </summary>
        /// <param name="strgCountryID">The CountryID parameter</param>        
        public void GetCities(string strgCountryID)
        {
            try
            {
                DataTable dtcities = new DataTable();
                dtcities = this.locationDetails.GetActiveCities(strgCountryID);
                this.ddlCity.DataSource = dtcities;
                this.ddlCity.DataValueField = "LocationCity";
                this.ddlCity.DataTextField = "LocationId";
                this.ddlCity.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Get Cities method
        /// </summary>
        /// <param name="dtsecurityCity">The date Security City parameter</param>        
        public void GetCities(DataTable dtsecurityCity)
        {
            try
            {
                DataTable dtcities = new DataTable();
                dtcities = dtsecurityCity;
                this.ddlCity.DataSource = dtcities;
                this.ddlCity.DataTextField = "City";
                this.ddlCity.DataValueField = "LocationId";
                this.ddlCity.DataBind();
                this.ddlCity.SelectedIndex = this.ddlCity.Items.IndexOf(this.ddlCity.Items.FindByValue(Convert.ToString(dtsecurityCity.Rows[0]["City"])));
                this.Getfacilities(dtsecurityCity);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Get facilities for the selected cities
        /// </summary>
        public void GetFacilities()
        {
            try
            {
                DataTable dtfacilities;
                if (!this.ddlCity.SelectedItem.Text.Equals("Select"))
                {
                    dtfacilities = this.locationDetails.GetActiveFacilities(this.ddlCity.SelectedItem.Text);
                    this.ddlFacility.DataSource = dtfacilities;
                    this.ddlFacility.DataTextField = "CountryCityFacility";
                    this.ddlFacility.DataValueField = "LocationId";
                    this.ddlFacility.DataBind();
                }

                if ((this.ddlFacility.Items.Count == 0) || (this.ddlFacility.Items.Count > 0))
                {
                    this.ddlFacility.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GetFacilities method
        /// </summary>
        /// <param name="dtsecurityCity">The date SecurityCity parameter</param>        
        public void Getfacilities(DataTable dtsecurityCity)
        {
            try
            {
                DataTable dtfacilities;
                dtfacilities = dtsecurityCity;
                this.ddlFacility.DataSource = dtfacilities;
                this.ddlFacility.DataTextField = "CountryCityFacility";
                this.ddlFacility.DataValueField = "LocationId";
                this.ddlFacility.DataBind();
                this.ddlFacility.SelectedIndex = this.ddlFacility.Items.IndexOf(this.ddlFacility.Items.FindByValue(Convert.ToString(dtsecurityCity.Rows[0]["LocationId"])));
                ((HiddenField)this.Parent.FindControl("hdnSecurityFacility")).Value = dtsecurityCity.Rows[0]["Facility"].ToString();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The InsertLocationInformation1 method
        /// </summary>
        /// <param name="strSP">The SP parameter</param>
        /// <returns>The VMSBusinessEntity.VisitorRequest type object</returns>        
        public VisitorRequest InsertLocationInformation1(string strSP)
        {
            string ipaddress;

            ////if (Session["RequestID"] != null)
            ////{
            ////}
            ////VisitorRequest.Country = ddlCountry.SelectedItem.Text.Trim();
            ////VisitorRequest.City = ddlCity.SelectedItem.Text.Trim();
            this.visitorRequest.LocationId = Convert.ToInt32(this.ddlFacility.SelectedItem.Value);
            ////VisitorRequest.Facility = ddlFacility.SelectedItem.Text.Trim();
            this.visitorRequest.Purpose = this.ddlPurpose.SelectedItem.Value.Trim();
            this.visitorRequest.HostDepartment = this.hidHostDeptDesc.Value;

            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipaddress))
            {
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            this.visitorRequest.IPAddress = ipaddress;

            if (this.ddlPurpose.SelectedItem.Value.Equals("Others"))
            {
                this.txtPurpose.Visible = true;
                this.visitorRequest.Purpose = this.txtPurpose.Text;
            }
            else
            {
                this.txtPurpose.Visible = false;
                this.visitorRequest.Purpose = this.ddlPurpose.SelectedItem.Value.Trim();
            }

            string hostAssociateID = string.Empty;

            if (this.txtHost.Value.Length > 0)
            {
                int startIndex = this.txtHost.Value.IndexOf("(") + 1;
                hostAssociateID = this.txtHost.Value.Substring(startIndex, this.txtHost.Value.Length - (startIndex + 1));
            }

            this.visitorRequest.HostID = this.txtHost.Value;
            this.visitorRequest.HostContactNo = this.txtHostContactNo.Text;
            this.visitorRequest.PermitITEquipments = this.hdnPermitEquipments.Checked;
            ////test by bincey
            this.txtHost.Value = this.hdnSelectedHost.Value = Session["strHostId"].ToString();
            string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
            string dttoDate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
            string[] todate1 = dttoDate.Split('/');
            string[] fromDate1 = dtfromDate.Split('/');

            /* to convert different time zone to Indian time zone format */
            DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
            string timestart = startDate.ToString("HH:mm");
            this.visitorRequest.FromTime = TimeSpan.Parse(timestart);
            DateTime endDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));
            string timeend = endDate.ToString("HH:mm");
            this.visitorRequest.ToTime = TimeSpan.Parse(timeend);
            this.visitorRequest.FromDate = Convert.ToDateTime(startDate.ToShortDateString());
            this.visitorRequest.ToDate = Convert.ToDateTime(endDate.ToShortDateString());
            if (Request.QueryString.ToString().Contains("details="))
            {
                if (this.Session["FromDate"] != null && this.Session["ToDate"] != null && this.Session["FromTime"] != null && this.Session["ToTime"] != null)
                {
                    if (this.Session["FromDate"].ToString() != this.visitorRequest.FromDate.Value.ToString("dd/MM/yyyy") || this.Session["ToDate"].ToString() != this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy")
                          || this.Session["FromTime"].ToString() != this.visitorRequest.FromTime.ToString() || this.Session["ToTime"].ToString() != this.visitorRequest.ToTime.ToString())
                    {
                        if ((this.Session["FromDate"].ToString() == this.visitorRequest.FromDate.Value.ToString("dd/MM/yyyy") &&
                             this.Session["ToDate"].ToString() == this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy") &&
                         this.Session["FromTime"].ToString() == this.visitorRequest.FromTime.ToString() &&
                         this.Session["ToTime"].ToString() != this.visitorRequest.ToTime.ToString())
                             || (this.Session["ToDate"].ToString() != this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy") &&
                             this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy") != this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy")))
                        {
                            ////no increment\
                            if (this.visitorRequest.FromDate == this.visitorRequest.ToDate)
                            {
                                if (this.visitorRequest.FromTime > this.visitorRequest.ToTime)
                                {
                                    this.visitorRequest.ToDate = this.visitorRequest.ToDate.Value.AddDays(1);

                                    this.txtToDate.Text = this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy");
                                }
                            }
                        }
                        else if (this.visitorRequest.FromTime > this.visitorRequest.ToTime)
                        {
                            this.visitorRequest.ToDate = this.visitorRequest.ToDate.Value.AddDays(1);

                            this.txtToDate.Text = this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy");
                        }
                    }
                }
            }

            DateTime dtfromDateTime = (DateTime)this.visitorRequest.FromDate;
            DateTime dttoDateTime = (DateTime)this.visitorRequest.ToDate;
            TimeSpan span = dttoDateTime.Subtract(dtfromDateTime);
            if (string.IsNullOrEmpty(this.hdnRecurrencePattern.Value))
            {
                if (span.Days > 1)
                {
                    this.visitorRequest.RecurrencePattern = "DAILY";
                }
                else
                {
                    this.visitorRequest.RecurrencePattern = "ONCE";
                }
            }
            else
            {
                this.visitorRequest.RecurrencePattern = this.hdnRecurrencePattern.Value.ToUpper();
            }

            if (string.IsNullOrEmpty(this.hdnOccurence.Value))
            {
                this.visitorRequest.Occurence = string.Empty;
            }
            else
            {
                this.visitorRequest.Occurence = this.hdnOccurence.Value;
            }

            this.visitorRequest.Comments = string.Empty;
            DateTime createdDate = this.genTimeZone.GetCurrentDate();
            this.visitorRequest.CreatedDate = this.genTimeZone.GetCurrentDate();
            this.visitorRequest.Createdby = Session["LoginID"].ToString();
            ////VisitorRequest.BadgeIssuedDate = null;
            this.visitorRequest.RequestedDate = this.genTimeZone.GetCurrentDate();
            if (this.Session["RequestStatus"] != null)
            {
                if (Session["RequestStatus"].ToString().ToUpper().Equals("IN"))
                {
                    this.visitorRequest.RequestStatus = "IN";
                }
                else if (Session["RequestStatus"].ToString().ToUpper().Equals("OUT"))
                {
                    this.visitorRequest.RequestStatus = "OUT";
                }
                else if (Session["RequestStatus"].ToString().ToUpper().Equals("YET TO ARRIVE"))
                {
                    this.visitorRequest.RequestStatus = "yet to arrive";
                }
                else if (Session["RequestStatus"].ToString().ToUpper().Equals(VMSConstants.REPEATVISITOR))
                {
                    this.visitorRequest.RequestStatus = VMSConstants.REPEATVISITOR;
                }
            }
            else
            {
                this.visitorRequest.RequestStatus = "yet to arrive";
            }

            this.OnBubbleSelectedIndexChange(null, null);
            return this.visitorRequest;
        }

        /// <summary>
        /// The InsertLocationInformation method
        /// </summary>
        /// <returns>The VMSBusinessEntity.VisitorRequest type object</returns>
        public VisitorRequest InsertLocationInformation()
        {
            string ipaddress;
            if (Request.QueryString["RequestID"] != null)
            {
                this.visitorRequest.RequestID = Convert.ToInt32(Request.QueryString["RequestID"]);
            }
            else
            {
                if (this.Session["RequestID"] != null)
                {
                    this.visitorRequest.RequestID = Convert.ToInt32(this.Session["RequestID"]);
                }
            }
            ////  VisitorRequest.Country = ddlCountry.SelectedItem.Text.Trim();
            //// VisitorRequest.City = ddlCity.SelectedItem.Text.Trim();

            this.visitorRequest.LocationId = Convert.ToInt32(this.ddlFacility.SelectedItem.Value);
            this.visitorRequest.Facility = this.ddlFacility.SelectedItem.Text.Trim();
            //// VisitorRequest.Facility = ddlFacility.SelectedItem.Text.Trim();
            this.visitorRequest.Purpose = this.ddlPurpose.SelectedItem.Value.Trim();
            this.visitorRequest.HostDepartment = this.hidHostDeptDesc.Value;
            this.visitorRequest.ParentReferenceId = this.visitorRequest.RequestID;

            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipaddress))
            {
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            this.visitorRequest.IPAddress = ipaddress;

            if (this.ddlPurpose.SelectedItem.Value.Equals("Others"))
            {
                this.txtPurpose.Visible = true;
                this.visitorRequest.Purpose = this.txtPurpose.Text;
            }
            else
            {
                this.txtPurpose.Visible = false;
                this.visitorRequest.Purpose = this.ddlPurpose.SelectedItem.Value.Trim();
            }

            string hostAssociateID = string.Empty;
            if (this.txtHost.Value.Length > 0)
            {
                int startIndex = this.txtHost.Value.IndexOf("(") + 1;
                hostAssociateID = this.txtHost.Value.Substring(startIndex, this.txtHost.Value.Length - (startIndex + 1));
            }

            this.visitorRequest.HostID = this.txtHost.Value;
            this.visitorRequest.HostContactNo = this.txtHostContactNo.Text;
            this.visitorRequest.PermitITEquipments = true; ////this.hdnPermitEquipments.Checked;
            ////if (Session["EquipmentCustody"].Equals(true))
            ////{
            //    VisitorRequest.IsEquipmentInCustody = true;
            ////}
            ////else
            ////{
            //    VisitorRequest.IsEquipmentInCustody = false;
            ////}

            string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");

            string dttoDate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
            string[] todate1 = dttoDate.Split('/');
            string[] fromDate1 = dtfromDate.Split('/');

            /* to convert different time zone to Indian time zone format */
            DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
            string timestart = startDate.ToString("HH:mm");
            this.visitorRequest.FromTime = TimeSpan.Parse(timestart);
            DateTime endDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));
            string timeend = endDate.ToString("HH:mm");
            this.visitorRequest.ToTime = TimeSpan.Parse(timeend);
            this.visitorRequest.FromDate = Convert.ToDateTime(startDate.ToShortDateString());
            this.visitorRequest.ToDate = Convert.ToDateTime(endDate.ToShortDateString());
            if (Request.QueryString.ToString().Contains("details="))
            {
                if (this.Session["FromDate"] != null && this.Session["ToDate"] != null && this.Session["FromTime"] != null && this.Session["ToTime"] != null)
                {
                    if (this.Session["FromDate"].ToString() != this.visitorRequest.FromDate.Value.ToString("dd/MM/yyyy") || this.Session["ToDate"].ToString() != this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy")
                          || this.Session["FromTime"].ToString() != this.visitorRequest.FromTime.ToString() || this.Session["ToTime"].ToString() != this.visitorRequest.ToTime.ToString())
                    {
                        if ((this.Session["FromDate"].ToString() == this.visitorRequest.FromDate.Value.ToString("dd/MM/yyyy") &&
                             this.Session["ToDate"].ToString() == this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy") &&
                         this.Session["FromTime"].ToString() == this.visitorRequest.FromTime.ToString() &&
                         Session["ToTime"].ToString() != this.visitorRequest.ToTime.ToString())
                             || (Session["ToDate"].ToString() != this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy") &&
                             this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy") != this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy")))
                        {
                            ////no increment\
                            if (this.visitorRequest.FromDate == this.visitorRequest.ToDate)
                            {
                                if (this.visitorRequest.FromTime > this.visitorRequest.ToTime)
                                {
                                    this.visitorRequest.ToDate = this.visitorRequest.ToDate.Value.AddDays(1);

                                    this.txtToDate.Text = this.visitorRequest.ToDate.Value.ToString("dd/MM/yyyy");
                                }
                            }
                        }
                    }
                }
            }

            DateTime dtfromDateTime = (DateTime)this.visitorRequest.FromDate;
            DateTime dttoDateTime = (DateTime)this.visitorRequest.ToDate;
            TimeSpan span = dttoDateTime.Subtract(dtfromDateTime);
            if (string.IsNullOrEmpty(this.hdnRecurrencePattern.Value))
            {
                if (span.Days > 1)
                {
                    this.visitorRequest.RecurrencePattern = "DAILY";
                }
                else
                {
                    this.visitorRequest.RecurrencePattern = "ONCE";
                }
            }
            else
            {
                this.visitorRequest.RecurrencePattern = this.hdnRecurrencePattern.Value.ToUpper();
            }

            if (string.IsNullOrEmpty(this.hdnOccurence.Value))
            {
                this.visitorRequest.Occurence = string.Empty;
            }
            else
            {
                this.visitorRequest.Occurence = this.hdnOccurence.Value;
            }

            this.visitorRequest.BulkUpload = false;
            this.visitorRequest.Comments = string.Empty;
            DateTime createdDate = this.genTimeZone.GetCurrentDate();
            this.visitorRequest.CreatedDate = this.genTimeZone.GetCurrentDate();
            this.visitorRequest.Createdby = Session["LoginID"].ToString();
            ////VisitorRequest.BadgeIssuedDate = null;
            this.visitorRequest.RequestedDate = this.genTimeZone.GetCurrentDate();
            if (this.Session["RequestStatus"] != null)
            {
                if (this.Session["RequestStatus"].ToString().ToUpper().Equals("IN"))
                {
                    this.visitorRequest.RequestStatus = "IN";
                }
                else if (this.Session["RequestStatus"].ToString().ToUpper().Equals("OUT"))
                {
                    this.visitorRequest.RequestStatus = "OUT";
                }
                else if (this.Session["RequestStatus"].ToString().ToUpper().Equals("YET TO ARRIVE"))
                {
                    this.visitorRequest.RequestStatus = "yet to arrive";
                }
                else if (this.Session["RequestStatus"].ToString().ToUpper().Equals(VMSConstants.REPEATVISITOR))
                {
                    this.visitorRequest.RequestStatus = VMSConstants.REPEATVISITOR;
                }
                else if (this.Session["RequestStatus"].ToString().ToUpper().Equals("SUBMITTED"))
                {
                    this.visitorRequest.RequestStatus = "yet to arrive";
                }
            }
            else
            {
                this.visitorRequest.RequestStatus = "yet to arrive";
            }

            this.OnBubbleSelectedIndexChange(null, null);
            return this.visitorRequest;
        }

        /// <summary>
        /// The ResetLocationInformation method
        /// </summary>
        /// <param name="multipleEntry">The IsMultipleEntry parameter</param>        
        public void ResetLocationInformation(bool multipleEntry)
        {
            // for single entry updated for CR IRVMS22062010CR07
            if (multipleEntry == false)
            {
                this.ddlPurpose.SelectedIndex = 0;
                this.GetUserDetails(Session["LoginID"].ToString());
                //if (this.txtHostContactNo.Text = true)
                {
                    this.txtHostContactNo.Text = string.Empty;
                }

                this.txtPurpose.Text = string.Empty;
                this.txtPurpose.Visible = false;
                this.InitDates();
            }

            this.HideControlsBasedonRoles();
        }

        /// <summary>
        /// The ShowLocationInformationByRequestID method
        /// </summary>
        /// <param name="propertiesDC">The propertiesDC parameter</param>        
        public void ShowLocationInformationByRequestID(VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC)
        {
            try
            {
                VMSBusinessLayer.RequestDetailsBL requestInfo =
                    new VMSBusinessLayer.RequestDetailsBL();
                DataTable dtlocationDetails = requestInfo.GetLocationDetailsById(propertiesDC.VisitorRequestProperty.RequestID);
                if (dtlocationDetails.Rows[0]["CountryId"] != null)
                {
                    this.ddlCountry.Items.FindByValue(dtlocationDetails.Rows[0]["CountryId"].ToString()).Selected = true;
                    if (this.ddlCountry.SelectedItem.Text == "Argentina")
                    {
                        this.tblIdentity.Visible = true;
                        this.ddlIdentityType.SelectedValue = propertiesDC.IndentityDetailsProperty.IdentityType;
                        this.txtIdentityNo.Text = propertiesDC.IndentityDetailsProperty.IdentityNo;
                    }
                    else
                    {
                        this.tblIdentity.Visible = false;
                    }
                }

                if (dtlocationDetails.Rows[0]["LocationId"] != null)
                {
                    this.ddlCity.Items.FindByValue(dtlocationDetails.Rows[0]["LocationId"].ToString()).Selected = true;
                    this.ddlFacility.Items.FindByValue(dtlocationDetails.Rows[0]["LocationId"].ToString()).Selected = true;
                }

                string purpose = propertiesDC.VisitorRequestProperty.Purpose;
                this.PopulatePurpose();
                ////12/9/09
                if (purpose.Equals("Clients") || purpose.Equals("New Joinee") ||
                    purpose.Equals("Vendor") || purpose.Equals("Guests") ||
                    purpose.Equals("Imp/VIP Visitor") || purpose.Equals("Auditors") ||
                    purpose.Equals("Interview Candidate") || purpose.Equals("Former Employee") ||
                  //  purpose.Equals("Family Member- Visa processing") || purpose.Equals("Associate Driver")  
                    purpose.Equals("Family Members") || purpose.Equals("Associate Driver") ||
                    purpose.Equals("Business Partner") ||
                    purpose.Equals("Select visitor type"))
                {
                    this.ddlPurpose.ClearSelection();
                    purpose = purpose == "Clients" ? "Client" : purpose;
                    this.ddlPurpose.Items.FindByValue(purpose.ToString()).Selected = true;
                    this.txtPurpose.Visible = false;
                }
                else
                {
                    this.ddlPurpose.Items.FindByValue("Others").Selected = true;
                    this.txtPurpose.Visible = true;
                    this.txtPurpose.Text = purpose;
                }
                ////Begin Changes By Vimal 08Nov2011
                this.GetAdvanceAllowabledays();
                ////END Changes By Vimal 08Nov2011
                this.txtHost.Value = this.hdnSelectedHost.Value = propertiesDC.VisitorRequestProperty.HostID;
                this.txtHostContactNo.Text = propertiesDC.VisitorRequestProperty.HostContactNo;
                this.hidHostDeptDesc.Value = propertiesDC.VisitorRequestProperty.HostDepartment;
                if (propertiesDC.VisitorRequestProperty.PermitITEquipments != null)
                {
                    this.hdnPermitEquipments.Checked = (bool)propertiesDC.VisitorRequestProperty.PermitITEquipments;
                }
                else
                {
                    this.hdnPermitEquipments.Checked = false;
                }

                string[] fromdate = propertiesDC.VisitorRequestProperty.FromDate.Value.ToShortDateString().Split('/');

                string fromTime = Convert.ToString(propertiesDC.VisitorRequestProperty.FromTime);
                DateTime startDate = Convert.ToDateTime(fromdate[0] + "/" + fromdate[1] + "/" + fromdate[2] + " " + fromTime);

                VMSBusinessLayer.RequestDetailsBL req = new VMSBusinessLayer.RequestDetailsBL();
                this.hdnVisitorOffset.Value = req.GetVisitorOffset(propertiesDC.VisitorRequestProperty.RequestID.ToString());

                string[] todate = propertiesDC.VisitorRequestProperty.ToDate.Value.ToShortDateString().Split('/');
                string time = Convert.ToString(propertiesDC.VisitorRequestProperty.ToTime);
                DateTime endDate = Convert.ToDateTime(todate[0] + "/" + todate[1] + "/" + todate[2] + " " + time);

                ////timezone conversion to local
                string strstartDate = this.genTimeZone.ToLocalTimeZone(startDate, propertiesDC.VisitorRequestProperty.Offset);
                string strendDate = this.genTimeZone.ToLocalTimeZone(endDate, propertiesDC.VisitorRequestProperty.Offset);

                this.txtFromDate.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(strstartDate));
                //// txtFromDate.Text = propertiesDC.VisitorRequestProperty.FromDate.Value.ToString("dd/MM/yyyy");
                // txtToDate.Text = propertiesDC.VisitorRequestProperty.ToDate.Value.ToString("dd/MM/yyyy");
                this.txtToDate.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(strendDate));
                ////txtEscort.Value = propertiesDC.VisitorRequestProperty.Escort;

                ////To display time in HH:mm format                
                this.txtFromTime.Text = DateTime.Parse(strstartDate).ToString("HH:mm");
                this.txtToTime.Text = DateTime.Parse(strendDate).ToString("HH:mm");
                ////timezone conversion to local

                ////txtVehicleNo.Text = Convert.ToString(propertiesDC.VisitorRequestProperty.VehicleNo);
                this.hdnBadgeStatus.Value = Convert.ToString(propertiesDC.VisitDetailProperty.BadgeStatus);
                if (string.IsNullOrEmpty(propertiesDC.VisitorRequestProperty.RecurrencePattern))
                {
                    this.hdnRecurrencePattern.Value = string.Empty;
                }
                else
                {
                    this.hdnRecurrencePattern.Value = propertiesDC.VisitorRequestProperty.RecurrencePattern;
                }

                if (string.IsNullOrEmpty(propertiesDC.VisitorRequestProperty.Occurence))
                {
                    this.hdnOccurence.Value = string.Empty;
                }
                else
                {
                    this.hdnOccurence.Value = propertiesDC.VisitorRequestProperty.Occurence;
                }

                this.hdnDate.Value = propertiesDC.VisitDetailProperty.Date.Value.ToString("dd/MM/yyyy");
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetVisitTime", "GetVisitTime();", true);

                this.Session["RequestStatus"] = Convert.ToString(propertiesDC.VisitorRequestProperty.RequestStatus);
                this.Session["FromDate"] = propertiesDC.VisitorRequestProperty.FromDate.Value.ToString("dd/MM/yyyy");
                this.Session["ToDate"] = propertiesDC.VisitorRequestProperty.ToDate.Value.ToString("dd/MM/yyyy");
                this.Session["FromTime"] = Convert.ToString(propertiesDC.VisitorRequestProperty.FromTime);
                this.Session["ToTime"] = Convert.ToString(propertiesDC.VisitorRequestProperty.ToTime);

                if (this.userDetails.IsSecurity(propertiesDC.VisitorRequestProperty.Createdby.ToString().Trim()).ToUpper() == "NO")
                {
                    this.ddlCountry.Enabled = false;
                    this.ddlCity.Enabled = false;
                    this.ddlFacility.Enabled = false;
                    this.ddlPurpose.Enabled = false;
                    this.txtPurpose.Enabled = false;
                    this.txtHostContactNo.Enabled = false;
                }
                else
                {
                    this.EnableVisitLocationInformationControls();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The DisableVisitLocationInformationControls method
        /// </summary>
        /// <param name="requestStatus">The RequestStatus parameter</param>
        /// <param name="badgeStatus">The BadgeStatus parameter</param>        
        public void DisableVisitLocationInformationControls(string requestStatus, string badgeStatus)
        {
            try
            {
                if (badgeStatus.ToUpper().Equals("RETURNED") || requestStatus.ToUpper().Equals("CANCELLED")
                    || badgeStatus.ToUpper().Equals("LOST"))
                {
                    this.ddlCountry.Enabled = false;
                    this.ddlCity.Enabled = false;
                    this.ddlFacility.Enabled = false;
                    this.ddlPurpose.Enabled = false;
                    this.txtPurpose.Enabled = false;
                    this.txtHostContactNo.Enabled = false;
                    this.txtFromDate.ReadOnly = true;
                    this.txtToDate.ReadOnly = true;
                    this.txtFromTime.ReadOnly = true;
                    this.txtToTime.ReadOnly = true;
                    this.txtFromDate.CssClass = "DisabledText";
                    this.txtToDate.CssClass = "DisabledText";
                    this.txtFromTime.CssClass = "DisabledText";
                    this.txtToTime.CssClass = "DisabledText";
                    ////txtVehicleNo.Enabled = false;
                    ////this.imgbutHost.Enabled = false;
                    ////imgbutEscort.Enabled = false;
                    ////Refresh.Enabled = false;
                    this.imgFromDate.Enabled = false;
                    this.imgToDate.Enabled = false;
                }
                else if (string.IsNullOrEmpty(badgeStatus))
                {
                    this.EnableVisitLocationInformationControls();
                }
                else if (badgeStatus.ToUpper().Equals("ISSUED"))
                {
                    this.ddlCountry.Enabled = false;
                    this.ddlCity.Enabled = false;
                    this.ddlFacility.Enabled = false;
                    this.ddlPurpose.Enabled = false;
                    this.txtPurpose.Enabled = false;
                    this.txtHostContactNo.Enabled = false;
                    this.txtFromDate.ReadOnly = true;
                    this.txtToDate.ReadOnly = true;
                    this.txtFromDate.CssClass = "DisabledText";
                    this.txtToDate.CssClass = "DisabledText";
                    this.txtToDate.Attributes.Add("CssStyle", "DisabledText");
                    this.txtFromTime.Attributes.Add("ReadOnly", "ReadOnly");
                    this.txtFromTime.CssClass = "DisabledText";
                    this.txtToTime.Enabled = true;               
                    this.imgFromDate.Enabled = false;
                    this.imgToDate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The CheckFromDateWithCurrentDate_Validate method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        public void CheckFromDateWithCurrentDate_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
            {
                GenericTimeZone genrTimeZone = new GenericTimeZone();
                string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
                string dttoDate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');

                /* to convert different time zone to Indian time zone format */
                DateTime startDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                DateTime endDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                DateTime dt = genrTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;
                Button submit = (Button)this.Parent.FindControl("Submit");
                //if (submit.Text == Resources.LocalizedText.Submit)
                //{
                    if (startDate.Date < todaysDate)
                    {
                        args.IsValid = false;
                    }
                //}
            }
        }

        /// <summary>
        /// The CheckToDateWithCurrentDate_Validate method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        public void CheckToDateWithCurrentDate_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
            {
                GenericTimeZone genrTimeZone = new GenericTimeZone();
                string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
                string dttodate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttodate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');

                DateTime endDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                DateTime dt = genrTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;
                if (endDate.Date < todaysDate)
                {
                    args.IsValid = false;
                }
            }
        }

        /// <summary>
        /// The CheckDateDuration method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        public void CheckDateDuration(object source, ServerValidateEventArgs args)
        {
            this.custCheckDateDuration.ErrorMessage = string.Empty;
            if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
            {
                GenericTimeZone genrTimeZone = new GenericTimeZone();
                DateTime dt = genrTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;

                string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
                string dttoDate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');
                /* to convert different time zone to Indian time zone format */
                DateTime startDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                DateTime endDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                string date = this.GetAdvanceAllowabledays();
                if (!string.IsNullOrEmpty(date))
                {
                    ////if ((endDate.Date - startDate.Date).TotalDays + 1 >= 32)
                    //    args.IsValid = false;
                    if ((endDate.Date - startDate.Date).TotalDays + 1 > Convert.ToInt32(date))
                    {
                        this.custCheckDateDuration.ErrorMessage = Resources.LocalizedText.VisitDurationError + date + ' ' + Resources.LocalizedText.VisitDurationErrorday;
                        args.IsValid = false;
                    }
                }
            }
        }

        /// <summary>
        /// The Check Time with Current Time method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The parameter</param>        
        public void CheckTimewithCurrentTime(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
            {
                GenericTimeZone genrTimeZone = new GenericTimeZone();
                string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
                string dttoDate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');
                /* to convert different time zone to Indian time zone format */
                DateTime startDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                DateTime endDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                ////24/09/2009
                DateTime currentdate = genrTimeZone.GetCurrentDate();
                DateTime dt = currentdate.AddMinutes(-10);
                ////end
                DateTime todaysDate = dt.Date;
                TimeSpan todaysTime = dt.TimeOfDay;
                ////    TimeSpan tsFrom = TimeSpan.Parse(VMSUtility.VMSUtility.GetTimeToISTZone(fromTime.ToString()));
                Button submit = (Button)this.Parent.FindControl("Submit");
                //if (startDate.Date == todaysDate &&
                //        submit.Text != Resources.LocalizedText.Update)
                if(startDate.Date == todaysDate)
                {
                    if (startDate.TimeOfDay <= todaysTime)
                    {
                        args.IsValid = false;
                    }
                }
            }
        }

        /// <summary>
        /// The Check Time with To Time And From Time method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        public void CheckTimewithToTimeAndFromTime(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
            {
                ////TimeSpan fromTime, TimeSpan toTime, DateTime fromDate, DateTime toDate
                GenericTimeZone genrTimeZone = new GenericTimeZone();
                string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
                string dttoDate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');
                /* to convert different time zone to Indian time zone format */
                DateTime startDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                DateTime endDate = genrTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                if (startDate.Date == endDate.Date)
                {
                    if (startDate.TimeOfDay >= endDate.TimeOfDay)
                    {
                        args.IsValid = false;
                    }
                }
            }
        }

        /// <summary>
        /// The EnableVisitLocationInformationControls method
        /// </summary>        
        public void EnableVisitLocationInformationControls()
        {
            this.ddlCity.Enabled = true;
            this.ddlFacility.Enabled = true;
            this.ddlPurpose.Enabled = true;
            this.txtPurpose.Enabled = true;
            this.txtHostContactNo.Enabled = true;
            this.txtFromDate.Enabled = true;
            this.txtToDate.Enabled = true;
            this.txtFromTime.Enabled = true;
            this.txtToTime.Enabled = true;
            ////txtVehicleNo.Enabled = true;
        }

        /// <summary>
        /// Get Visit Details
        /// </summary>
        /// <returns>The Business Entity Visit Detail type object</returns>        
        public VMSBusinessEntity.VisitDetail[] GetVisitDetails()
        {
            VMSBusinessEntity.VisitDetail[] returnObj = null;
            try
            {
                ArrayList datesList = this.GetDates();
                ArrayList arrayList1 = new ArrayList();
                foreach (string li in datesList)
                {
                    VMSBusinessEntity.VisitDetail visitDetailObj = new VMSBusinessEntity.VisitDetail();
                    visitDetailObj.Date = Convert.ToDateTime(li);
                    arrayList1.Add(visitDetailObj);
                    returnObj = new VMSBusinessEntity.VisitDetail[arrayList1.Count];
                    arrayList1.CopyTo(0, returnObj, 0, arrayList1.Count);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return returnObj;
        }

        /// <summary>
        /// The GetVisitDetailsByRequestID method
        /// </summary>
        /// <param name="requestID">The RequestID parameter</param>
        /// <returns>The VMSBusinessEntity.VisitDetail[] type object</returns>        
        public VMSBusinessEntity.VisitDetail[] GetVisitDetailsByRequestID(string requestID)
        {
            VMSBusinessEntity.VisitDetail[] returnObj = null;
            try
            {
                VMSBusinessLayer.RequestDetailsBL req = new VMSBusinessLayer.RequestDetailsBL();
                List<VisitDetail> visitDetailArray = req.GetVisitDetailsByRequestID(requestID);
                bool addToday = false;
                DateTime dt = this.genTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;
                foreach (VMSBusinessEntity.VisitDetail li in visitDetailArray)
                {
                    if ((li.Date == todaysDate) && (li.BadgeStatus != null))
                    {
                        addToday = false;
                        break;
                    }
                    else
                    {
                        addToday = true;
                    }
                }

                ArrayList datesList = this.GetDates();
                ArrayList arrayList1 = new ArrayList();
                foreach (string li in datesList)
                {
                    VMSBusinessEntity.VisitDetail visitDetailObj = new VMSBusinessEntity.VisitDetail();
                    if (addToday)
                    {
                        if (Convert.ToDateTime(li) >= todaysDate)
                        {
                            visitDetailObj.Date = Convert.ToDateTime(li);
                            arrayList1.Add(visitDetailObj);
                            returnObj = new VMSBusinessEntity.VisitDetail[arrayList1.Count];
                            arrayList1.CopyTo(0, returnObj, 0, arrayList1.Count);
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(li).Date > todaysDate)
                        {
                            visitDetailObj.Date = Convert.ToDateTime(li);
                            arrayList1.Add(visitDetailObj);
                            returnObj = new VMSBusinessEntity.VisitDetail[arrayList1.Count];
                            arrayList1.CopyTo(0, returnObj, 0, arrayList1.Count);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return returnObj;
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
                this.txtFromDate.Attributes.Add("readonly", "readonly");
                this.txtToDate.Attributes.Add("readonly", "readonly");
                if (this.txtHost.Value != string.Empty)
                {
                    string hostid = XSS.HtmlEncode(this.txtHost.Value);
                    int startIndex = hostid.IndexOf("(") + 1;
                    this.userID = hostid.Substring(startIndex, hostid.Length - (startIndex + 1)).ToString();
                    this.GetUserDetails(this.userID);
                }

                Ajax.Utility.RegisterTypeForAjax(typeof(VisitorLocationInformation));
                if (!Page.IsPostBack)
                {
                    Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffsetTimeLocation", "GetOffsetTimeLocation();", true);
                    if (this.Session["LoginID"] == null)
                    {
                        return;
                    }

                    if (Request.QueryString.ToString().Contains("details="))
                    {
                    }
                    else
                    {
                        this.InitDates();
                    }

                    this.InitCountries();
                    this.userID = Session["LoginID"].ToString();
                    this.GetUserDetails(this.userID);
                    this.PopulatePurpose();
                    this.lblOtherPurpose.Visible = false;
                    this.txtPurpose.Visible = false;
                    this.HideControlsBasedonRoles();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Country_SelectedIndexChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.ddlCountry.SelectedItem.Text.Equals("Select"))
                {
                    this.GetCities(this.strCountryID);
                }
                else
                {
                    this.ddlCity.Items.Clear();
                    this.ddlCity.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The City_SelectedIndexChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.ddlCity.SelectedItem.Text.Equals("Select"))
                {
                    this.GetFacilities();
                    //// Added by priti on 3rd June for VMS CR VMS31052010CR6
                    ////   getAdvanceAllowabledays();
                }
                else
                {
                    this.ddlFacility.Items.Clear();
                    this.ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Purpose Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Session["purposetype"] = this.ddlPurpose.SelectedItem.Value;
                if (this.ddlPurpose.SelectedItem.Value == "Others")
                {
                    this.lblOtherPurpose.Visible = true;
                    this.txtPurpose.Visible = true;
                }
                else
                {
                    this.lblOtherPurpose.Visible = false;
                    this.txtPurpose.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The DateCheck_Validate method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        protected void CustDateCheck_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
            {
                ////GenericTimeZone genrTimeZone = new GenericTimeZone();            
                string dtfromDate = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(this.txtFromDate.Text));
                string dttoDate = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(this.txtToDate.Text));
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');

                DateTime startDate = Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2]);
                DateTime endDate = Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2]);

                if (startDate > endDate)
                {
                    args.IsValid = false;
                }
            }
        }

        /// <summary>
        /// The Purpose Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DlPurpose_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlPurpose.SelectedItem.Value == VMSConstants.OTHERS)
                {
                    this.txtPurpose.Visible = true;
                }
                else
                {
                    this.txtPurpose.Visible = false;
                }

                ////Begin Changes By Vimal 08Nov2011
                this.GetAdvanceAllowabledays();
                ////END Changes By Vimal 08Nov2011 
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The txtFromDate_TextChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void TxtFromDate_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgbutHost_Click(object sender, ImageClickEventArgs e)
        {
        }

        /// <summary>
        /// The Identity Type Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlIdentityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtIdentityNo.Text = string.Empty;
            this.txtIdentityNo.Focus();
        }

        /// <summary>
        /// Description:Hide controls based on roles
        /// </summary>
        private void HideControlsBasedonRoles()
        {
            try
            {
                List<string> roles = (List<string>)Session["UserRole"];
                if (roles.Contains("Security") || roles.Contains("SuperAdmin"))
                {
                    this.txtHost.Value = this.hdnSelectedHost.Value = string.Empty;
                    this.txtHostContactNo.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GetDates method
        /// </summary>
        /// <returns>The System.Collections.ArrayList type object</returns>        
        private ArrayList GetDates()
        {
            ArrayList dt = new ArrayList();
            try
            {
                string dtfromDate = DateTime.Parse(this.txtFromDate.Text).ToString("MM/dd/yyyy");
                string dttoDate = DateTime.Parse(this.txtToDate.Text).ToString("MM/dd/yyyy");
                string[] todate1 = dttoDate.Split('/');
                string[] fromDate1 = dtfromDate.Split('/');

                ////VisitorRequest.Escort = txtEscort.Value;

                /* to convert different time zone to Indian time zone format */
                DateTime startDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                switch (this.hdnRecurrencePattern.Value.ToUpper())
                {
                    case "ONCE":
                        {
                            DateTime dtfromDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                            dt.Add(dtfromDateTime.ToString());
                            break;
                        }

                    case "DAILY":
                        {
                            DateTime dtfromDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                            DateTime dttoDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                            TimeSpan ts = dttoDateTime.Subtract(dtfromDateTime);
                            for (int i = 0; i <= ts.Days; i++)
                            {
                                dt.Add(dtfromDateTime.AddDays(Convert.ToDouble(i)).ToString());
                            }

                            break;
                        }

                    case "WEEKLY":
                        {
                            string weekdays = this.hdnOccurence.Value.ToUpper();
                            DateTime dtfromDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                            DateTime dttoDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                            for (DateTime k = dtfromDateTime; k <= dttoDateTime; k = k.AddDays(1))
                            {
                                if (weekdays.Contains(k.DayOfWeek.ToString().Substring(0, 3).ToUpper()))
                                {
                                    dt.Add(k.Date.ToString());
                                }
                            }

                            break;
                        }

                    case "MONTHLY":
                        {
                            char[] delimiters = new char[] { '\r', '\n', ',' };
                            string[] strings = this.hdnOccurence.Value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            int[] intDates = strings.Select(x => int.Parse(x)).ToArray();
                            Array.Sort(intDates);
                            string[] stringArranged = intDates.Select(y => y.ToString()).ToArray();
                            DateTime dtfromDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                            DateTime dttoDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                            for (DateTime k = dtfromDateTime; k <= dttoDateTime; k = k.AddDays(1))
                            {
                                if (stringArranged.Contains(k.Day.ToString().Trim()))
                                {
                                    dt.Add(k.Date.ToString());
                                }
                            }

                            break;
                        }

                    default:
                        {
                            DateTime dtfromDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromDate1[0] + "/" + fromDate1[1] + "/" + fromDate1[2] + " " + this.txtFromTime.Text), Convert.ToString(Session["TimezoneOffset"]));
                            DateTime dttoDateTime = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[0] + "/" + todate1[1] + "/" + todate1[2] + " " + this.txtToTime.Text), Convert.ToString(Session["TimezoneOffset"]));

                            if (dtfromDateTime.ToString("dd/MM/yyyy") == dttoDateTime.ToString("dd/MM/yyyy"))
                            {
                                dt.Add(dtfromDateTime.ToString());
                            }
                            else
                            {
                                TimeSpan ts = dttoDateTime.Subtract(dtfromDateTime);
                                for (int i = 0; i <= ts.Days; i++)
                                {
                                    dt.Add(dtfromDateTime.AddDays(Convert.ToDouble(i)).ToString());
                                }
                            }

                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(e, HttpContext.Current);
            }

            return dt;
        }

        /// <summary>
        /// The LoadTimeZone method
        /// </summary>
        /// <param name="countryName">The countryName parameter</param>        
        private void LoadTimeZone(string countryName)
        {

            VMSDataLayer.VMSDataLayer dal = new VMSDataLayer.VMSDataLayer();
            DataTable dttimezone = new DataTable();
            dttimezone = dal.GetTimeZoneInfo(countryName);

            if (dttimezone.Rows.Count > 0)
            {
                this.lblTimeZone1.Text = this.lblTimeZone2.Text = dttimezone.Rows[0]["timezone"].ToString();
            }
            else
            {
                this.lblTimeZone1.Text = this.lblTimeZone2.Text = string.Empty;
            }
            //switch (countryName)
            //{
            //    case "India": this.lblTimeZone1.Text = this.lblTimeZone2.Text = "IST";
            //        break;
            //    case "China": this.lblTimeZone1.Text = this.lblTimeZone2.Text = "CST";
            //        break;
            //    case "Argentina": this.lblTimeZone1.Text = this.lblTimeZone2.Text = "ART";
            //        break;
            //    case "Hungary": this.lblTimeZone1.Text = this.lblTimeZone2.Text = "CET";
            //        break;
            //    case "Philippines": this.lblTimeZone1.Text = this.lblTimeZone2.Text = "PST";
            //        break;
            //    default: this.lblTimeZone1.Text = this.lblTimeZone2.Text = string.Empty;
            //        break;
            //}
        }
    }
}
