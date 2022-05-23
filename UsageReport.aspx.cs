
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
    using System.Web.UI.DataVisualization.Charting;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using VMSBL;
    using VMSBusinessLayer;

    /// <summary>
    ///   to get usage report
    /// </summary>
    public partial class UsageReport : System.Web.UI.Page
    {
        /// <summary>
        /// The Departments field
        /// </summary>        
        private List<string> departments = new List<string>();
        
        /// <summary>
        /// The Cities field
        /// </summary>        
        private List<string> cities = new List<string>();
        
        /// <summary>
        /// The Facilities field
        /// </summary>        
        private List<string> facilities = new List<string>();
        
        /// <summary>
        /// The LocationDetails field
        /// </summary>        
        private VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
        
        /// <summary>
        /// The Request details field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestdetails = new VMSBusinessLayer.RequestDetailsBL();
        
        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();

        /// <summary>
        /// The AssignTimeZoneOffset method
        /// </summary>
        /// <param name="strTimezoneoffset">The time zone parameter</param>        
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
        /// The method to populate all cities
        /// </summary>
        /// <param name="countryId">The country parameter</param>        
        public void InitCities(string countryId)
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable dtcities = objlocationDetails.GetActiveCities(countryId);
                this.ddlCity.DataTextField = "LocationCity";
                this.ddlCity.DataValueField = "LocationCity";
                this.ddlCity.DataSource = dtcities;
                this.ddlCity.DataBind();
                this.ddlCity.Items.Insert(0, new ListItem("Select", string.Empty));
                this.ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
                //// InitFacilities();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to populate all Facilities within a City
        /// </summary>
        public void InitFacilities()
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable dtfacilities = objlocationDetails.GetActiveFacilities(this.ddlCity.SelectedItem.Text);
                this.ddlFacility.DataTextField = "LocationName";
                this.ddlFacility.DataValueField = "LocationName";
                this.ddlFacility.DataSource = dtfacilities;
                this.ddlFacility.DataBind();
                this.ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
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
                ////txtFromDate.Text = VMSConstants.VMSConstants.currentDate.ToString("dd/MM/yyyy");
                ////txtToDate.Text = VMSConstants.VMSConstants.currentDate.ToString("dd/MM/yyyy");
                string format = "dd/MM/yyyy HH:mm:ss";
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                if (this.Session["currentDateTime"] != null)
                {
                    string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                    var today = DateTime.ParseExact(currenttime, format, provider);
                    this.txtFromDate.Text = today.ToString("dd/MM/yyyy");
                    this.txtToDate.Text = today.ToString("dd/MM/yyyy");
                }
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
            Ajax.Utility.RegisterTypeForAjax(typeof(UsageReport));
            ////Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
            
            if (!Page.IsPostBack)
            {
              ////  #region commenting
              //  Chart1.Enabled = false;
              //  InitCountry();               
              //  //InitDates();
              //  InitDepartments();
              //  VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL objUserRoleLoc =
              //  new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
              //  DataTable drInfo = objUserRoleLoc.GetUserRoleLocationBL(Convert.ToString(Session["LoginID"]));
              //  if (drInfo.Rows.Count > 0)
              //  {
              //      drpCountry.SelectedValue=Convert.ToString(drInfo.Rows[0]["CountryId"]);
              //  }
              //  InitCities(drpCountry.SelectedItem.Value);
        
              ////  //btnExport.Enabled = false;
              //  string city = string.Empty;
              //  string Facility = string.Empty;
              //  string Fromdate = string.Empty;
              //  string Todate = string.Empty;
               
              ////  ddlCity.Enabled = true;
              //  drpCountry.Enabled = true;
              //  ddlFacility.Enabled = true;
              //  List<string> Roles = ((List<string>)Session["UserRole"]);

              ////  DataSet securitycity = GetSecurityCity();
              //  drpCountry.SelectedValue = Convert.ToString(securitycity.Tables[0].Rows[0]["CountryId"]);
              //  ddlCity.SelectedValue = Convert.ToString(securitycity.Tables[0].Rows[0]["City"]);
              //  InitFacilities();
              //  ddlFacility.SelectedValue = Convert.ToString(securitycity.Tables[0].Rows[0]["Facility"]);
           
              ////  if (!Roles.Contains("SuperAdmin"))
              //  {
              //      ddlCity.Enabled = false;
              //      drpCountry.Enabled = false;
              //      ddlFacility.Enabled = false;
              //  }
              //  if (Session["currentDateTime"] != null)
              //  {
              //      InitDates();
              //  }
              //  else
              //  {
              //      Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
              //  }

              ////  string[] str1 = txtFromDate.Text.Split('/');
              //  string[] str2 = txtToDate.Text.Split('/');
              //  DateTime dt1 = Convert.ToDateTime(str1[1] + "/" + str1[0] + "/" + str1[2]);
              //  DateTime dt2 = Convert.ToDateTime(str2[1] + "/" + str2[0] + "/" + str2[2]);
              //  if (ddlCity.SelectedIndex == 0)
              //      city = null;
              //  else
              //      city = ddlCity.SelectedItem.Text;

              ////  if (ddlFacility.SelectedIndex == 0 ||
              //      ddlFacility.SelectedIndex==-1)
              //      Facility = null;
              //  else
              //      Facility = ddlFacility.SelectedItem.Text;

              ////  if (txtFromDate.Text == string.Empty)
              //      Fromdate = null;

              ////  else
              //      Fromdate = txtFromDate.Text;

              ////  if (txtToDate.Text == string.Empty)
              //      Todate = null;

              ////  else
              //      Todate = txtToDate.Text;
              ////btnSearch_Click(null, null);
              //  #endregion
              this.Form.Target = "_blank";
                try
                {
                    Response.Redirect("1CAAPReportsPage.aspx", true);
                     
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }

        /// <summary>
        /// The GetSecurityCity method
        /// </summary>
        /// <returns>The System.Data.DataSet type object</returns>        
        protected DataSet GetSecurityCity()
        {
            ////string city;
            string securityID = Session["LoginID"].ToString();
            return this.requestdetails.GetSecurityCity(securityID);
            ////return city;
        }
        
        /// <summary>
        /// The report method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnUsageReportHidden_Click(object sender, EventArgs e)
        {
            this.InitDates();
        }

        /// <summary>
        /// The search method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            ////VMSBusinessLayer.VMSBusinessLayer.MasterDataBL obj
            string[] fromdate1 = this.txtFromDate.Text.Split('/');
            string[] todate1 = this.txtToDate.Text.Split('/');
            string fromTime1 = "23:59:00";
            string totime1 = "23:59:00";
            DateTime dtfromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate1[1] + "/" + fromdate1[0] + "/" + fromdate1[2] + " " + fromTime1), Convert.ToString(Session["TimezoneOffset"]));
            DateTime dttoDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate1[1] + "/" + todate1[0] + "/" + todate1[2] + " " + totime1), Convert.ToString(Session["TimezoneOffset"]));
            string fromDate = dtfromDate.ToString("dd/MM/yyyy");
            string todate = dttoDate.ToString("dd/MM/yyyy");
            string fromTime = dtfromDate.ToLongTimeString();
            string totime = dttoDate.ToLongTimeString();

            ////Added on 18May2011 for Placing Charts within folder --Vimal
            string chartPath = System.Configuration.ConfigurationManager.AppSettings["ChartsPath"].ToString();
            this.Chart1.ImageStorageMode = ImageStorageMode.UseImageLocation;
            this.Chart1.ImageLocation = Request.ApplicationPath + chartPath;
            this.Chart2.ImageStorageMode = ImageStorageMode.UseImageLocation;
            this.Chart2.ImageLocation = Request.ApplicationPath + chartPath;
            ////End Changes

            string city = string.Empty;
            string facility = string.Empty;
            string fromdate = string.Empty;
            string todates = string.Empty;
            string department = string.Empty;
            string country = string.Empty;

            string[] str1 = this.txtFromDate.Text.Split('/');
            string[] str2 = this.txtToDate.Text.Split('/');

            DateTime dt1 = Convert.ToDateTime(str1[1] + "/" + str1[0] + "/" + str1[2]);
            DateTime dt2 = Convert.ToDateTime(str2[1] + "/" + str2[0] + "/" + str2[2]);

            DataTable dtcounrtyId = new DataTable();

            if (this.drpCountry.SelectedItem.Text.ToString() == "Select")
            {
                country = null;
            }
            else
            {
                country = this.drpCountry.SelectedItem.Value;
            }

            ////dtCounrtyId = 

            if (this.ddlCity.SelectedIndex == 0 && this.ddlCity.SelectedItem.Text.ToString() == "Select")
            {
                city = null;
            }
            else
            {
                city = this.ddlCity.SelectedItem.Text;
            }

            if (this.ddlFacility.SelectedIndex == 0 || this.ddlFacility.SelectedIndex == -1)
            {
                facility = null;
            }
            else
            {
                facility = this.ddlFacility.SelectedItem.Text;
            }

            if (this.ddlDepartment.SelectedIndex == 0 && this.ddlDepartment.SelectedItem.Text.ToString() == "Select")
            {
                department = null;
            }
            else
            {
                department = this.ddlDepartment.SelectedItem.Text;
            }

            if (string.IsNullOrEmpty(this.txtFromDate.Text))
            {
                fromdate = null;
            }
            else
            {
                fromdate = this.txtFromDate.Text;
            }

            if (string.IsNullOrEmpty(this.txtToDate.Text))
            {
                todates = null;
            }
            else
            {
                todates = this.txtToDate.Text;
            }

            if (dt1 <= dt2)
            {
                DataSet griddata = new DataSet();
                griddata = this.requestdetails.Usagereportesearch(city, facility, fromdate, todates, department, country);
                this.grdResult.Visible = true;
                this.grdResult.DataSource = griddata;
                this.grdResult.DataBind();

                List<string> roles = (List<string>)Session["UserRole"];

                if (roles.Contains("SuperAdmin"))
                {
                    if (this.drpCountry.SelectedItem.Text.ToString() != "Select")
                    {
                        griddata.Tables.Clear();
                        country = this.drpCountry.SelectedItem.Value;
                        griddata = this.requestdetails.Chart_VMS(this.ddlCity.SelectedItem.Text, facility, fromdate, todates, department, country);

                        if (griddata.Tables[0].Rows.Count > 0)
                        {
                            this.Chart1.Series["Host"].XValueMember = "Facility";
                        }

                        this.Chart1.Series["Host"].YValueMembers = "Host";
                        this.Chart1.Series["Security"].YValueMembers = "Security";
                        this.Chart1.DataSource = griddata.Tables[0];
                        this.Chart1.DataBind();

                        DataSet griddata2 = new DataSet();
                        ////country = Convert.ToString(drpCountry.SelectedItem.Text);
                        country = this.drpCountry.SelectedItem.Value;
                        griddata2 = this.requestdetails.PieGraph(this.ddlCity.SelectedItem.Text, facility, fromdate, todates, department, country);
                        if (griddata2.Tables[0].Rows.Count > 0)
                        {
                            this.Chart2.DataSource = griddata2.Tables[0];
                        }

                        this.Chart2.Series["statuscount"].YValueMembers = "statuscount";
                        this.Chart2.Series["statuscount"].XValueMember = "Visitstatus";
                        this.Chart2.DataBind();
                    }

                    if (this.ddlCity.SelectedIndex != 0 && this.ddlCity.SelectedItem.Text.ToString() != "Select")
                    {
                        griddata.Tables.Clear();
                        country = this.drpCountry.SelectedItem.Value;
                        griddata = this.requestdetails.Chart_VMS(this.ddlCity.SelectedItem.Text, facility, fromdate, todates, department, country);

                        if (griddata.Tables[0].Rows.Count > 0)
                        {
                            this.Chart1.Series["Host"].XValueMember = "Facility";
                        }

                        this.Chart1.Series["Host"].YValueMembers = "Host";
                        this.Chart1.Series["Security"].YValueMembers = "Security";
                        this.Chart1.DataSource = griddata.Tables[0];
                        this.Chart1.DataBind();

                        DataSet griddata2 = new DataSet();
                        ////country = Convert.ToString(drpCountry.SelectedItem.Text);
                        country = this.drpCountry.SelectedItem.Value;
                        griddata2 = this.requestdetails.PieGraph(this.ddlCity.SelectedItem.Text, facility, fromdate, todates, department, country);
                        if (griddata2.Tables[0].Rows.Count > 0)
                        {
                            this.Chart2.DataSource = griddata2.Tables[0];
                        }

                        this.Chart2.Series["statuscount"].YValueMembers = "statuscount";
                        this.Chart2.Series["statuscount"].XValueMember = "Visitstatus";
                        this.Chart2.DataBind();
                    }
                    else
                    {
                        string dept = this.ddlDepartment.SelectedItem.Text;
                        griddata.Tables.Clear();
                        country = this.drpCountry.SelectedItem.Value;
                        griddata = this.requestdetails.Chart_VMS(null, facility, fromdate, todates, dept, country);

                        if (griddata.Tables[0].Rows.Count > 0)
                        {
                            this.Chart1.Series["Host"].XValueMember = "Facility";
                        }

                        this.Chart1.Series["Host"].YValueMembers = "Host";
                        this.Chart1.Series["Security"].YValueMembers = "Security";
                        this.Chart1.DataSource = griddata.Tables[0];
                        this.Chart1.DataBind();

                        DataSet griddata2 = new DataSet();
                        ////country = Convert.ToString(drpCountry.SelectedItem.Text);
                        ////changed by bincey
                        country = this.drpCountry.SelectedItem.Value;
                        griddata2 = this.requestdetails.PieGraph(null, facility, fromdate, todates, department, country);
                        if (griddata2.Tables[0].Rows.Count > 0)
                        {
                            this.Chart2.DataSource = griddata2.Tables[0];
                        }

                        this.Chart2.Series["statuscount"].YValueMembers = "statuscount";
                        this.Chart2.Series["statuscount"].XValueMember = "Visitstatus";
                        this.Chart2.DataBind();
                    }
                }
                else
                {
                    string dept = this.ddlDepartment.SelectedItem.Text;
                    griddata = this.requestdetails.Chart_VMS(this.ddlCity.SelectedItem.Text, facility, fromdate, todates, dept, country);
                    griddata.Tables[0].Columns.RemoveAt(0);

                    if (griddata.Tables[0].Rows.Count > 0)
                    {
                        ////Chart1.Series["Host"].XValueMember = "Facility";
                    }

                    // Chart1.Series["Facility"].XValueMember = "Facility";
                    this.Chart1.Series["Host"].YValueMembers = "Host";
                    this.Chart1.Series["Security"].YValueMembers = "Security";
                    this.Chart1.DataSource = griddata.Tables[0];
                    this.Chart1.DataBind();

                    DataSet griddata2 = new DataSet();
                    ////country = Convert.ToString(drpCountry.SelectedItem.Text);
                    ////changed by bincey
                    country = this.drpCountry.SelectedItem.Value;
                    griddata2 = this.requestdetails.PieGraph(this.ddlCity.SelectedItem.Text, facility, fromdate, todates, department, country);
                    if (griddata2.Tables[0].Rows.Count > 0)
                    {
                        this.Chart2.DataSource = griddata2.Tables[0];
                    }

                    this.Chart2.Series["statuscount"].YValueMembers = "statuscount";
                    this.Chart2.Series["statuscount"].XValueMember = "Visitstatus";
                    this.Chart2.DataBind();
                }
            }
            else
            {
#pragma warning disable CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
                Page.RegisterClientScriptBlock("script", "<script language='javascript'> alert(\"To Date should be greater than From Date\"); </script>");
#pragma warning restore CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
            }
        }
  
        /// <summary>
        /// The export method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            HtmlForm form = new HtmlForm();

            string attachment = "attachment; filename=Visitors.xls";

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
        /// The select country method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DrpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.InitCities(this.drpCountry.SelectedItem.Value);
                this.InitFacilities();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Event for populating Facilities as per the city selected
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        protected void DdlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.InitFacilities();
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
                this.departments = this.requestdetails.GetDepartments();
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
        /// The Country method
        /// </summary>        
        private void InitCountry()
        {
            VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
            DataTable dtcoubtryList = objlocationDetails.GetActiveCountry();
            this.drpCountry.DataTextField = "Country";
            this.drpCountry.DataValueField = "CountryId";
            this.drpCountry.DataSource = dtcoubtryList;
            this.drpCountry.DataBind();
            this.drpCountry.Items.Insert(0, new ListItem("Select", string.Empty));
        }
    }
}
