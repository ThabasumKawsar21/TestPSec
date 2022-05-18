
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
    using VMSBusinessEntity;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// data by department partial class
    /// </summary>
    public partial class DatabyDeptMultiVisit : System.Web.UI.Page
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
        /// The VisitorID field
        /// </summary>        
        private List<int> visitorID = new List<int>();
        
        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();

        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = 
            new VMSBusinessLayer.RequestDetailsBL();
        
        /// <summary>
        /// The LocationDetails field
        /// </summary>        
        private VMSBusinessLayer.LocationDetailsBL locationDetails = 
            new VMSBusinessLayer.LocationDetailsBL();
        
        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();
        
        /// <summary>
        /// The MergeRows method
        /// </summary>
        /// <param name="gridView">The gridView parameter</param>        
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];
                Image image = row.FindControl("Image123") as Image;
                Image image1 = previousRow.FindControl("Image123") as Image;
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (image1.ImageUrl.ToString().Contains("Brown") && image.ImageUrl.ToString().Contains("Brown"))
                    {
                        row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                                               previousRow.Cells[10].RowSpan + 1;
                        previousRow.Cells[10].Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// The AssignTimeZoneOffset method
        /// </summary>
        /// <param name="strTimezoneoffset">The string Time zone off set parameter</param>        
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
        /// The Initial Dates method
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
                    
                        Response.Redirect("~/SessionExpired.aspx",true);
                     
                    
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
        /// The GetFacilities method
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
                    this.ddlFacility.DataTextField = "LocationName";
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
        /// The Initial Cities method
        /// </summary>
        /// <param name="countryId">The countryId parameter</param>        
        public void InitCities(string countryId)
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL strlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable cities = strlocationDetails.GetActiveCities(countryId);
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
        /// The Initial Facilities method
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
        /// The MultipleVisitors method
        /// </summary>
        /// <param name="type">The type parameter</param>
        /// <returns>The System.Collections.Generic.List</returns>       
        public List<int> MultipleVisitors(string type)
        {
            try
            {
                this.visitorID = this.requestDetails.MultiCityFacility(type);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return this.visitorID;
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(DatabyDeptMultiVisit));         

            if (!Page.IsPostBack)
            {
                this.Form.Target = "_blank";
                try
                {
                    Response.Redirect("1CAAPReportsPage.aspx",true);
                   
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }

        /// <summary>
        /// The DDLCity_SelectedIndexChanged method
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
        /// The Button Data by Department Multi Visit Hidden_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnDatabyDeptMultiVisitHidden_Click(object sender, EventArgs e)
        {
            this.InitDates();
        }

        /// <summary>
        /// The ButtonSearch_Click method
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
        /// The OnRowCreated method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[10].CssClass = "hiddencol";
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[10].CssClass = "hiddencol";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[11].CssClass = "hiddencol";
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[11].CssClass = "hiddencol";
            }
        }

        /// <summary>
        /// The Clear on click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Clear_onclick(object sender, ImageClickEventArgs e)
        {
            this.txtFromDate.Text = string.Empty;
        }

        /// <summary>
        /// The Clear on click 1 method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Clear_onclick1(object sender, ImageClickEventArgs e)
        {
            this.txtToDate.Text = string.Empty;
        }

        /// <summary>
        /// The Clear on click 2 method
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
        /// The Hide_time method
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
        /// The ButtonExport_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            this.PrepareGridViewForExport(this.grdDataByDeptResult);
            this.grdDataByDeptResult.Columns[10].Visible = false;
            this.grdDataByDeptResult.Columns[11].Visible = false;

            HtmlForm form = new HtmlForm();

            string attachment = "attachment; filename= Data_by_Department.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/ms-excel";

            StringWriter stw = new StringWriter();

            HtmlTextWriter htextw = new HtmlTextWriter(stw);

            form.Controls.Add(this.grdDataByDeptResult);

            this.Controls.Add(form);

            form.RenderControl(htextw);

            Response.Write(stw.ToString());
            stw.Close();
            htextw.Close();
            Response.End();
        }

        /// <summary>
        /// The Grid Data By Department Result_PageIndexChanging method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdDataByDeptResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdDataByDeptResult.PageIndex = e.NewPageIndex;
                this.BindingGrid();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The ButtonReset0_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset0_Click(object sender, EventArgs e)
        {
            this.InitDates();
            this.lblFromTime.Visible = false;
            this.lblToTime.Visible = false;
            this.txtFromTime.Visible = false;
            this.txtToTime.Visible = false;
            this.btnhidetime.Visible = false;
            this.btnShowtime.Visible = true;
            this.lblResult.Visible = false;
            this.MV1.Visible = false;
            this.MV2.Visible = false;
            this.btnExport.Visible = false;
            this.grdDataByDeptResult.Visible = false;
        }

        /// <summary>
        /// The GridView_PreRender method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GridView_PreRender(object sender, EventArgs e)
        {
            DatabyDeptMultiVisit.MergeRows(this.grdDataByDeptResult);
        }

        /// <summary>
        /// The DDLCountry_SelectedIndexChanged method
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
        /// The Initial Departments method
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
        /// The Initial Country method
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

        /// <summary>
        /// The BindingGrid method
        /// </summary>        
        private void BindingGrid()
        {
            string[] fromdate = this.txtFromDate.Text.Split('/');
            string[] todate = this.txtToDate.Text.Split('/');
            ////added by bincey for Location Name Change CR
            int countryId = 0;
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
                string fromTime1 = "23:59:00";
                string totime1 = "23:59:00";
                /* to convert different time zone to Indian time zone format */
                DateTime dtfromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2] + " " + this.txtFromTime.Text.ToString()), Convert.ToString(Session["TimezoneOffset"]));
                DateTime dttoDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2] + " " + this.txtToTime.Text.ToString()), Convert.ToString(Session["TimezoneOffset"]));
                string fromDate = dtfromDate.ToString("dd/MM/yyyy");
                string todate2 = dttoDate.ToString("dd/MM/yyyy");
                string fromTime = dtfromDate.ToLongTimeString();
                string totime = dttoDate.ToLongTimeString();
                ////Griddata = RequestDetails.ReportInfo(ddlCountry.SelectedItem.Text.Trim(),ddlCity.SelectedItem.Text.Trim(), ddlFacility.SelectedItem.Text.Trim(), txtFromDate.Text.ToString().Trim(), txtToDate.Text.ToString().Trim(), txtFromTime.Text.ToString(), txtToTime.Text.ToString(), ddlDepartment.SelectedItem.Text.ToString().Trim(), "Multivisit", null);
                this.griddata = this.requestDetails.ReportInfo(countryId, this.ddlCity.SelectedItem.Text.Trim(), this.ddlFacility.SelectedItem.Value, fromDate, todate2, fromTime1, totime1, this.ddlDepartment.SelectedItem.Text.ToString().Trim(), "Multivisit", null);
            }
            else
            {
                string fromTime1 = "23:59:00";
                string totime1 = "23:59:00";
                /* to convert different time zone to Indian time zone format */
                DateTime dtfromDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2] + " " + fromTime1), Convert.ToString(Session["TimezoneOffset"]));
                DateTime dttoDate = this.genTimeZone.GetdatetimedetailsinIST(Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2] + " " + totime1), Convert.ToString(Session["TimezoneOffset"]));
                string fromDate = dtfromDate.ToString("dd/MM/yyyy");
                string todate1 = dttoDate.ToString("dd/MM/yyyy");
                string fromTime = dtfromDate.ToLongTimeString();
                string totime = dttoDate.ToLongTimeString();
                ////Griddata = RequestDetails.ReportInfo(ddlCountry.SelectedItem.Text.Trim(), ddlCity.SelectedItem.Text.Trim(), ddlFacility.SelectedItem.Text.Trim(), txtFromDate.Text.ToString().Trim(), txtToDate.Text.ToString().Trim(), null, null, ddlDepartment.SelectedItem.Text.ToString().Trim(), "Multivisit", null);
                this.griddata = this.requestDetails.ReportInfo(countryId, this.ddlCity.SelectedItem.Text.Trim(), this.ddlFacility.SelectedItem.Value, fromDate, todate1, null, null, this.ddlDepartment.SelectedItem.Text.ToString().Trim(), "Multivisit", null);
            }

            if (this.griddata.Tables[0].Rows.Count > 0)
            {
                this.lblResult.Visible = false;
                this.lblStatusResult.Visible = false;
                this.grdDataByDeptResult.Visible = true;

                this.grdDataByDeptResult.DataSource = this.griddata;
                this.grdDataByDeptResult.DataBind();
                    if (VMSUtility.VMSUtility.CountryTimeZoneCheck() == true)
                {
                    this.btnExport.Visible = true;
                }
                else
                {
                    this.btnExport.Visible = false;
                }              
              ////  ImgColorIndicate.Visible = true;
            }
            else
            {
                this.lblStatusResult.Visible = false;
                this.errortbl.Visible = true; ////*********ADDED  BY UMA-18 jULY 09************
                this.lblResult.Visible = true;
                this.grdDataByDeptResult.Visible = false;
                this.lblResult.Text = "No Record Found";
                this.btnExport.Visible = false;
               //// ImgColorIndicate.Visible = false;
            }
        }

        /// <summary>
        /// The Validations method
        /// </summary>
        /// <returns>The initial type object</returns>        
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
                    this.grdDataByDeptResult.Visible = false;
                   //// ImgColorIndicate.Visible = false;
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
                    this.grdDataByDeptResult.Visible = false;
                    ////ImgColorIndicate.Visible = false;
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
        /// The PrepareGridViewForExport method
        /// </summary>
        /// <param name="gv">The GV parameter</param>        
        private void PrepareGridViewForExport(Control gv)
        {
            LinkButton lb = new LinkButton();

            Literal l = new Literal();

            string name = string.Empty;

            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    l.Text = XSS.HtmlEncode((gv.Controls[i] as DropDownList).SelectedItem.Text);

                    gv.Controls.Remove(gv.Controls[i]);

                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(TextBox))
                {
                    l.Text = (gv.Controls[i] as TextBox).Text;

                    gv.Controls.Remove(gv.Controls[i]);

                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(Button))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(Image))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";

                    gv.Controls.Remove(gv.Controls[i]);

                    gv.Controls.AddAt(i, l);
                }

                if (gv.Controls[i].HasControls())
                {
                    this.PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }
    }
}
