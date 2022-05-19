
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using VMSBL;
    using VMSBusinessLayer;
    using VMSUtility;

    /// <summary>
    /// The Head count partial class
    /// </summary>
    public partial class Headcount : System.Web.UI.Page
    {
        /// <summary>
        /// The Departments field
        /// </summary>        
        private List<string> departments = new List<string>();

        /// <summary>
        /// The Associate field
        /// </summary>        
        private List<string> associate = new List<string>();

        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();

        /// <summary>
        /// The Request Details field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// The LocationDetails field
        /// </summary>        
        private VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();

        /// <summary>
        /// The Time Zone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();

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

        ////Begin VMS CR 16 Changes Uma

        /// <summary>
        /// The PopulatePurpose method
        /// </summary>        
        public void PopulatePurpose()
        {
            try
            {
                VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.MasterDataBL();
                List<string> purpose = new List<string>();
                List<string> purposeDataText = new List<string>();
                string[] purposeListArray;
                purpose = masterDataBL.GetMasterData("Purpose");
                foreach (string purposeData in purpose)
                {
                    purposeListArray = purposeData.ToString().Split('|');
                    purposeDataText.Add(purposeListArray[0]);
                }

                this.DdlPurpose.DataSource = purposeDataText;
                this.DdlPurpose.DataBind();
                this.DdlPurpose.Items.RemoveAt(0);
                if ((this.DdlPurpose.Items.Count == 0) || (this.DdlPurpose.Items.Count > 0))
                {
                    this.DdlPurpose.Items.Insert(0, new ListItem("Select visitor type", "0"));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        ////End VMS CR 16 Changes Uma

        /// <summary>
        /// The GetCities method
        /// </summary>        
        public void GetCities()
        {
            try
            {
                List<string> cities = new List<string>();
                cities = this.locationDetails.GetCountries();
                this.ddlCity.DataSource = cities;
                this.ddlCity.DataBind();

                if ((this.ddlCity.Items.Count == 0) || (this.ddlCity.Items.Count > 0))
                {
                    this.ddlCity.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Dates method
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
                    this.txtFromDate.Text = today.ToString("dd/MM/yyyy");
                    this.txtToDate.Text = today.ToString("dd/MM/yyyy");
                }
                else
                {
                     
                        string Targetpage = "~/SessionExpired.aspx";
                        this.Response.Redirect(Targetpage, true);
                         
                    
                    
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
        /// The Cities method
        /// </summary>
        /// <param name="countryId">The countryId parameter</param>        
        public void InitCities(string countryId)
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable cities = objlocationDetails.GetActiveCities(countryId);
                this.ddlCity.DataTextField = "LocationCity";
                this.ddlCity.DataValueField = "LocationCity";
                this.ddlCity.DataSource = cities;
                this.ddlCity.DataBind();
                this.ddlCity.Items.Insert(0, new ListItem("Select", string.Empty));
                this.InitFacilities();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Facilities method
        /// </summary>        
        public void InitFacilities()
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable facilities = objlocationDetails.GetActiveFacilities(this.ddlCity.SelectedItem.Text);
                this.ddlFacility.DataTextField = "LocationName";
                this.ddlFacility.DataValueField = "LocationId";
                this.ddlFacility.DataSource = facilities;
                this.ddlFacility.DataBind();
                this.ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(Headcount));

            if (!Page.IsPostBack)
            {
                this.Form.Target = "_blank";
                try
                {
                    Response.Redirect("1CAAPReportsPage.aspx", true);
                     
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
                ////#region commenting
                ////Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
                ////Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffsetTime", "GetOffsetTime();", true);

                ////InitCountry();

                ////InitDepartments();

                ////Begin VMS CR 16 Changes Uma
                ////PopulatePurpose();
                ////End VMS CR 16 Changes Uma
                ////txtFromDate.Attributes.Add("readonly", "readonly");
                ////txtToDate.Attributes.Add("readonly", "readonly");

                ////txtFromTime.Text = VMSConstants.VMSConstants.fromTime;
                ////txtToTime.Text = VMSConstants.VMSConstants.toTime;

                ////txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                ////txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                ////ddlCity.Items.Insert(0, new ListItem("Select", "0"));
                ////ddlFacility.Items.Insert(0, new ListItem("Select", "0"));
                ////lblFromTime.Visible = false;
                ////lblToTime.Visible = false;
                ////txtFromTime.Visible = false;
                ////txtToTime.Visible = false;
                ////btnhidetime.Visible = false;
                ////btnShowtime.Visible = true;
                ////MV1.Visible = false;

                ////MV2.Visible = false;

                ////btnExport.Visible = false;
                ////lblResult.Visible = false;
                ////Page.Form.DefaultButton = btnSearch.UniqueID;
                ////if (Session["currentDateTime"] != null)
                ////{
                //    InitDates();
                ////}
                ////else
                ////{
                //    Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
                ////}
                ////#endregion
            }
        }

        /// <summary>
        /// The City Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ////changed by priti on 15th June for VMS CR VMS31052010CR6 code review defects
                if (!this.ddlCity.SelectedItem.Text.Trim().Equals("Select"))
                {
                    this.InitFacilities();
                }
                else
                {
                    this.ddlFacility.Items.Clear();
                    this.ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
                }
                //// end of changes by priti on 15th June for VMS CR VMS31052010CR6 code review defects
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Search_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            int err = 0;
            err = this.Validations();

            if (err == 0)
            {
                this.BindingGrid();
            }
        }

        /// <summary>
        /// The clear on click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Clear_onclick(object sender, ImageClickEventArgs e)
        {
            this.txtFromDate.Text = string.Empty;
        }

        /// <summary>
        /// The clear on click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Clear_onclick1(object sender, ImageClickEventArgs e)
        {
            this.txtToDate.Text = string.Empty;
        }

        /// <summary>
        /// The clear on click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Clear_onclick2(object sender, ImageClickEventArgs e)
        {
            this.txtFromTime.Text = string.Empty;
        }

        /// <summary>
        /// The Show_Time method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Show_Time(object sender, ImageClickEventArgs e)
        {
            this.lblFromTime.Visible = true;
            this.lblToTime.Visible = true;
            this.txtFromTime.Visible = true;
            this.txtToTime.Visible = true;
            this.btnhidetime.Visible = true;
            this.btnShowtime.Visible = false;
            this.MV1.Visible = true;
            this.MV2.Visible = true;
        }

        /// <summary>
        /// The hide_time method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Hide_time(object sender, ImageClickEventArgs e)
        {
            this.lblFromTime.Visible = false;
            this.lblToTime.Visible = false;
            this.txtFromTime.Visible = false;
            this.txtToTime.Visible = false;
            this.btnhidetime.Visible = false;
            this.btnShowtime.Visible = true;
        }

        /// <summary>
        /// The Export_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            HtmlForm form = new HtmlForm();
            string attachment = "attachment; filename=HeadcountReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            form.Controls.Add(this.grdResult);
            this.Controls.Add(form);
            form.RenderControl(htextw);
            Response.Write(stw.ToString());
            stw.Close();
            htextw.Close();
            Response.End();
        }

        /// <summary>
        /// The Reset Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset0_Click(object sender, EventArgs e)
        {
            this.ddlDepartment.Items.Clear();
            this.ddlCity.Items.Clear();
            this.ddlFacility.Items.Clear();
            this.InitDepartments();
            this.ddlCountry.Items.Clear();
            this.InitCountry();
            this.DdlPurpose.Items.Clear();
            this.ddlCity.Items.Insert(0, new ListItem("Select", "0"));

            this.txtFromDate.Attributes.Add("readonly", "readonly");
            this.txtToDate.Attributes.Add("readonly", "readonly");

            this.txtFromTime.Text = VMSConstants.VMSConstants.FROMTIME;
            this.txtToTime.Text = VMSConstants.VMSConstants.TOTIME;

            ////txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            ////txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            this.InitDates();
            this.ddlFacility.Items.Insert(0, new ListItem("Select", "0"));
            this.lblFromTime.Visible = false;
            this.lblToTime.Visible = false;
            this.txtFromTime.Visible = false;
            this.txtToTime.Visible = false;
            this.btnhidetime.Visible = false;
            this.btnShowtime.Visible = true;
            this.MV1.Visible = false;
            this.lblResult.Visible = false;
            this.MV2.Visible = false;
            this.btnExport.Visible = false;
            this.grdResult.Visible = false;
            ////Begin VMS CR 16 Changes Uma
            this.txtPurpose.Visible = false;
            this.PopulatePurpose();
            this.lblOtherPurpose.Visible = false;
            ////End VMS CR 16 Changes Uma
        }

        /// <summary>
        /// The Head count Hidden Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnHeadcountHidden_Click(object sender, EventArgs e)
        {
            this.InitDates();
        }

        /// <summary>
        /// The txtFromDate_TextChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void TxtFromDate_TextChanged(object sender, EventArgs e)
        {
        }

        ////Begin VMS CR 16 Changes Uma

        /// <summary>
        /// The Purpose Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.DdlPurpose.SelectedValue == "Others")
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
        ////End VMS CR 16 Changes Uma

        /// <summary>
        /// The Country Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.InitCities(this.ddlCountry.SelectedItem.Value);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Departments method
        /// </summary>        
        private void InitDepartments()
        {
            try
            {
                this.departments = this.requestDetails.GetDepartments();

                this.ddlDepartment.DataSource = this.departments;
                this.ddlDepartment.DataBind();
                if ((this.ddlDepartment.Items.Count == 0) || (this.ddlDepartment.Items.Count > 0))
                {
                    this.ddlDepartment.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The BindingGrid method
        /// </summary>        
        private void BindingGrid()
        {
            ////Begin VMS CR 16 Changes Uma
            string purpose = string.Empty;
            string[] fromdate = this.txtFromDate.Text.Split('/');
            string[] todate = this.txtToDate.Text.Split('/');
            int countryId = 0;
            ////DateTime StartDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            ////DateTime EndDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            if (this.DdlPurpose.SelectedIndex == 0)
            {
                purpose = null;
            }
            else
                if (this.DdlPurpose.SelectedItem.Text.ToUpper() == "OTHERS")
            {
                purpose = this.txtPurpose.Text;
            }
            else
            {
                purpose = this.DdlPurpose.SelectedItem.Text;
            }

            if (!(this.ddlCountry.SelectedItem.Text == "Select"))
            {
                DataTable dtcountryId = this.requestDetails.GetCountryId(this.ddlCountry.SelectedItem.Text.Trim());
                if (dtcountryId.Rows.Count > 0)
                {
                    countryId = int.Parse(dtcountryId.Rows[0]["CountryId"].ToString());
                }
            }

            if (this.btnhidetime.Visible == true)
            {
                string fromTime1 = "23:59:59";
                string totime1 = "23:59:59";
                /* to convert different time zone to Indian time zone format */
                DateTime dtfromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2] + " " + fromTime1), Convert.ToString(Session["TimezoneOffset"]));
                DateTime dttoDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2] + " " + totime1), Convert.ToString(Session["TimezoneOffset"]));
                string fromDate = dtfromDate.ToString("dd/MM/yyyy");
                string totime = dttoDate.ToString("dd/MM/yyyy");

                ////Begin VMS CR 16 Changes Uma
                ////Griddata = RequestDetails.ReportInfo(ddlCountry.SelectedItem.Text.Trim(), ddlCity.SelectedItem.Text.Trim(), ddlFacility.SelectedItem.Text.Trim(), txtFromDate.Text.ToString().Trim(), txtToDate.Text.ToString().Trim(), txtFromTime.Text.ToString(), txtToTime.Text.ToString(), ddlDepartment.SelectedItem.Text.ToString().Trim(), "Headcount", purpose);
                ////chenges made by bincey for Location Name Change CR
                ////Griddata = RequestDetails.ReportInfo(ddlCountry.SelectedItem.Text.Trim(), ddlCity.SelectedItem.Text.Trim(), ddlFacility.SelectedItem.Text.Trim(), FromDate, ToDate, txtFromTime.Text.ToString(), txtToTime.Text.ToString(), ddlDepartment.SelectedItem.Text.ToString().Trim(), "Headcount", purpose);
                this.griddata = this.requestDetails.ReportInfo(countryId, this.ddlCity.SelectedItem.Text.Trim(), this.ddlFacility.SelectedItem.Value, fromDate, totime, this.txtFromTime.Text.ToString(), this.txtToTime.Text.ToString(), this.ddlDepartment.SelectedItem.Text.ToString().Trim(), "Headcount", purpose);
                ////End VMS CR 16 Changes Uma
            }
            else
            {
                DateTime dt = this.genTimeZone.GetLocalCurrentDate();
                string fromTime = "23:59:59";
                string totime2 = "23:59:59";
                DateTime dtfromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2] + " " + fromTime), Convert.ToString(Session["TimezoneOffset"]));
                DateTime dttoDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2] + " " + totime2), Convert.ToString(Session["TimezoneOffset"]));
                string fromDate = dtfromDate.ToString("dd/MM/yyyy");
                string totime = dttoDate.ToString("dd/MM/yyyy");
                this.griddata = this.requestDetails.ReportInfo(countryId, this.ddlCity.SelectedItem.Text.Trim(), this.ddlFacility.SelectedItem.Value, fromDate, totime, this.txtFromTime.Text.ToString(), this.txtToTime.Text.ToString(), this.ddlDepartment.SelectedItem.Text.ToString().Trim(), "Headcount", purpose);
            }

            if (this.griddata.Tables[0].Rows.Count > 0)
            {
                this.lblResult.Visible = false;
                this.lblStatusResult.Visible = false;
                this.grdResult.Visible = true;
                this.grdResult.DataSource = this.griddata;
                this.grdResult.DataBind();
                if (VMSUtility.CountryTimeZoneCheck() == true)
                {
                    this.btnExport.Visible = true;
                }
                else
                {
                    this.btnExport.Visible = false;
                }
            }
            else
            {
                this.lblStatusResult.Visible = false;
                this.errortbl.Visible = true;
                this.lblResult.Visible = true;
                this.grdResult.Visible = false;
                this.lblResult.Text = "No Record Found";
                this.btnExport.Visible = false;
            }
        }

        /// <summary>
        /// The Validations method
        /// </summary>
        /// <returns>The integer type object</returns>        
        private int Validations()
        {
            StringBuilder errstring = new StringBuilder();
            errstring.AppendLine(string.Empty);
            string[] str1 = this.txtFromDate.Text.Split('/');
            string[] str2 = this.txtToDate.Text.Split('/');

            DateTime dt1 = Convert.ToDateTime(str1[1] + "/" + str1[0] + "/" + str1[2]);
            DateTime dt2 = Convert.ToDateTime(str2[1] + "/" + str2[0] + "/" + str2[2]);

            DateTime dt3 = Convert.ToDateTime(this.txtFromTime.Text);
            DateTime dt4 = Convert.ToDateTime(this.txtToTime.Text);
            int error = 0;

            if (!string.IsNullOrEmpty(this.txtFromDate.Text) && !string.IsNullOrEmpty(this.txtToDate.Text))
            {
                if (dt1 > dt2)
                {
                    this.errortbl.Visible = true;
                    this.lblResult.Visible = true;
                    this.grdResult.Visible = false;
                    this.btnExport.Visible = false;

                    errstring.AppendLine("To Date should be greater than From Date");

                    this.lblResult.Text = errstring.ToString();
                    error = error + 1;
                }
            }

            if (this.btnhidetime.Visible == true)
            {
                if (dt3 > dt4)
                {
                    this.errortbl.Visible = true;
                    this.lblResult.Visible = true;
                    this.grdResult.Visible = false;
                    this.btnExport.Visible = false;
                    errstring.Append("<br>");
                    errstring.AppendLine(" To Time should be greater than From Time ");

                    this.lblResult.Text = errstring.ToString();

                    error = error + 1;
                }
            }

            return error;
        }

        /// <summary>
        /// The Country method
        /// </summary>        
        private void InitCountry()
        {
            VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
            DataTable dtcoubtryList = objlocationDetails.GetActiveCountry();
            this.ddlCountry.DataTextField = "Country";
            this.ddlCountry.DataValueField = "CountryId";
            this.ddlCountry.DataSource = dtcoubtryList;
            this.ddlCountry.DataBind();
            this.ddlCountry.Items.Insert(0, new ListItem("Select", string.Empty));
        }
    }
}
