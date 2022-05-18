

namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// card report partial class
    /// </summary>
    public partial class CardReport : System.Web.UI.Page
    {
        /// <summary>
        /// Gets or sets request details object
        /// </summary>
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// Gets or sets location details object
        /// </summary>
        private VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();

        /// <summary>        
        /// To replace special character
        /// </summary>
        /// <param name="str">receiving string</param>
        /// <returns>returning string</returns>
        public static string RegExValidate(string str)
        {
            string pattern = @"[;:>,</\\*]";

            return Regex.Replace(str, pattern, string.Empty);
        }

        /// <summary>
        /// Initializes cities function
        /// </summary>
        /// <param name="countryId">country Id value</param>
        public void InitCities(string countryId)
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable cities = this.locationDetails.GetActiveCitiesIVS(countryId);
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
        /// Initializes facilities function
        /// </summary>
        public void InitFacilities()
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL objlocationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable facilities = this.locationDetails.GetActiveFacilitiesIVS(this.ddlCity.SelectedItem.Text);
                this.ddlFacility.DataTextField = "LocationName";
                this.ddlFacility.DataValueField = "LocationName";
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
        /// Initializes dates function
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
                    this.txtStartDate.Text = today.ToString("MM/dd/yyyy");
                    this.txtEndDate.Text = today.ToString("MM/dd/yyyy");
                }
                else
                {
                     
                        string Targetpage = "~/SessionExpired.aspx";
                        Response.Redirect(Targetpage, true);
                         
                    
                   
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
        /// Confirms that an HtmlForm control is rendered for the  specified ASP.NET server control at run time.
        /// </summary>
        /// <param name="control">control parameter</param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            ////Confirms that an HtmlForm control is rendered for the  specified ASP.NET server control at run time.
        }

        #region PageLoad

        /// <summary>
        /// page load function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.InitCountry();

                Page.Form.DefaultButton = this.btnSearch.UniqueID;
                this.errortbl.Visible = false;
                this.lblEmployeeHeader.Visible = false;
                ////txtStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ////txtEndDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                this.ddlCity.Items.Insert(0, new ListItem("Select", "0"));
                this.ddlFacility.Items.Insert(0, new ListItem("Select", "0"));

                ////this.ImgStartDate.Attributes.Add("onClick", "javascript:calendarPicker('document.aspnetForm." + this.txtFromDate.ClientID.ToString() + "');");
                ////this.ImgEndDate.Attributes.Add("onClick", "javascript:calendarPicker('document.aspnetForm." + this.txtToDate.ClientID.ToString() + "');");
                this.txtEmpID.Attributes.Add("onkeypress", "javascript:return SpecialCharacterValidation(this);");
                //// ddlLocation.Items.Insert(0, new ListItem(VMSConstants.VMSConstants.LOCATIONSELECT, "0"));

                if (this.Session["currentDateTime"] != null)
                {
                    this.InitDates();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "GetOffset", "GetOffset();", true);
                }
               
               ////Commented Redirection 
               ////this.Form.Target = "_blank";
               ////Response.Redirect("1CAAPReportsPage.aspx");
            }
        }
        #endregion

        #region Control Methods
        
        /// <summary>
        /// Method to search card report on the basis of different search criteria    
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
             //// if ((ddlLocation.SelectedIndex != 0) || (txtStartDate.Text.Length != 0) || (txtEndDate.Text.Length != 0) || (txtEmpID.Text.Length != 0))
                if ((this.ddlCity.SelectedIndex != 0) || (this.ddlFacility.SelectedIndex != 0) || (this.txtStartDate.Text.Length != 0) || (this.txtEndDate.Text.Length != 0) || (this.txtEmpID.Text.Length != 0))
                {
                    if (this.ddlCountry.SelectedIndex == 0)
                    {
                        this.errortbl.Visible = true;
                        this.lblEmployeeHeader.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.VMSConstants.COUNTRYSELECTCRITERIA;
                        this.grdEmployee.Visible = false;
                        return;
                    }

                    DateTime dtstartDate = new DateTime();
                    DateTime dtendDate = new DateTime();
                    DateTime currentDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy"));
                    if (this.txtStartDate.Text.Length > 0)
                    {
                        dtstartDate = DateTime.Parse(this.txtStartDate.Text);
                    }

                    if (this.txtEndDate.Text.Length > 0)
                    {
                        dtendDate = DateTime.Parse(this.txtEndDate.Text);
                    }

                    if ((this.txtStartDate.Text.Length > 0) && (this.txtEndDate.Text.Length > 0))
                    {
                        if ((dtstartDate > dtendDate) || (dtstartDate > currentDate))
                        {
                            this.errortbl.Visible = true;
                            this.lblEmployeeHeader.Text = string.Empty;
                            this.lblMessage.Text = VMSConstants.VMSConstants.STARTDATECRITERIA;
                            this.grdEmployee.DataSourceID = string.Empty;
                            this.grdEmployee.EmptyDataText = string.Empty;
                            this.grdEmployee.DataBind();
                            return;
                        }
                    }

                    if (this.txtEndDate.Text.Length > 0 && (this.txtStartDate.Text.Length == 0))
                    {
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = VMSConstants.VMSConstants.SELECTSTARTDATE;
                        this.grdEmployee.DataSourceID = string.Empty;
                        this.grdEmployee.EmptyDataText = string.Empty;
                        return;
                    }

                    this.errortbl.Visible = false;
                    this.BindEmployeeDetails();
                }
                else
                {
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = VMSConstants.VMSConstants.SEARCHCRITERIA;
                    return;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to clear the values of controls once clear button is clicked 
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.InitCountry();
               ////ddlLocation.SelectedIndex = 0;

                this.txtEmpID.Text = string.Empty;
                this.txtStartDate.Text = string.Empty;
                this.txtEndDate.Text = string.Empty;
                this.gridtbl.Visible = false;
                this.lblEmployeeHeader.Visible = false;
                this.btnExcel.Enabled = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to rebind the rebind the grid view once the user select paging option
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void GrdEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdEmployee.PageIndex = e.NewPageIndex;
                this.grdEmployee.DataSource = this.BindEmployeeDetails();
                this.grdEmployee.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to clear the values in the grid view control once Location changes
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void DdlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.gridtbl.Visible = false;
                this.lblEmployeeHeader.Visible = false;
                this.btnExcel.Enabled = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to download the report in excel format 
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void BtnExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (this.grdEmployee.Rows.Count > 0)
            {
                var dataTable = (DataTable)this.grdEmployee.DataSource; 

                Response.Clear();
                Response.ClearHeaders();
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.AddHeader("content-disposition", "attachment;filename=PassIssuedDetailsReport.xls");
                Response.Charset = string.Empty;
                Response.ContentType = "application/vnd.xls";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                this.grdEmployee.AllowPaging = false;
                this.grdEmployee.DataSource = this.BindEmployeeDetails();
                this.grdEmployee.DataBind();
                this.grdEmployee.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());
                stringWrite.Close();
                htmlWrite.Close();
                Response.End();
                this.grdEmployee.AllowPaging = true;
            }
        }

        #endregion

        /// <summary>
        /// country selected index changed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
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
        /// city selected index changed
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
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

        #region Private Methods

        /// <summary>
        ///  Method to bind the grid view on the basis of search criteria   
        /// </summary>
        /// <returns>returns employee details</returns>
        private DataView BindEmployeeDetails()
        {
            // DataView dv = new DataView();
            DataView dvsearchResult = new DataView();

            try
            {
                this.grdEmployee.Visible = true;
                DataTable dtemployee = new DataTable();
                EmployeeBL objEmployeeList = new EmployeeBL();
                dtemployee = objEmployeeList.GetEmployeeList(this.txtEmpID.Text.ToString(), this.ddlCity.SelectedValue.ToString(), this.txtStartDate.Text.ToString(), this.txtEndDate.Text.ToString(), this.ddlCountry.SelectedValue.ToString(), this.ddlFacility.SelectedValue.ToString());
                ////dv = new DataView(dtEmployee);
                if (dtemployee != null)
                {
                    dvsearchResult = new DataView(dtemployee);
                    string searchExpression = string.Empty;

                    if (this.ddlFacility.SelectedIndex != 0)
                    {
                        searchExpression = "PassIssuedLocation like '%" + RegExValidate(this.ddlFacility.SelectedItem.ToString()) + "%'";
                        dvsearchResult.RowFilter = searchExpression;
                    }

                    if (this.ddlCountry.SelectedIndex != 0)
                    {
                        searchExpression = "PassIssuedcountry = " + RegExValidate(this.ddlCountry.SelectedValue.ToString().Trim()) + string.Empty;
                        dvsearchResult.RowFilter = searchExpression;
                    }

                    this.grdEmployee.DataSource = dvsearchResult;
                    if (dtemployee.Rows.Count > 0)
                    {
                        if (VMSUtility.VMSUtility.CountryTimeZoneCheck() == true)
                        {
                            this.btnExcel.Visible = true;
                        }
                        else
                        {
                            this.btnExcel.Visible = false;
                        }
                        ////btnExcel.Enabled = true;
                        this.grdEmployee.Width = Unit.Percentage(100);
                        this.grdEmployee.Height = Unit.Percentage(98);
                        ////gridtbl.Width = Unit.Percentage(100).ToString();
                        ////gridtbl.Height = Unit.Percentage(98).ToString();
                        this.lblEmployeeHeader.Visible = true;
                        this.gridtbl.Visible = true;
                    }
                    else
                    {
                        this.btnExcel.Enabled = false;
                        this.lblEmployeeHeader.Visible = false;
                        this.grdEmployee.Width = Unit.Percentage(60);
                        this.grdEmployee.Height = Unit.Percentage(.5);
                        this.gridtbl.Visible = true;
                        ////gridtbl.Width = Unit.Percentage(60).ToString();
                        ////gridtbl.Height = Unit.Percentage(.5).ToString();
                    }

                    this.grdEmployee.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
            ////return dv;
            return dvsearchResult;
        }

        #endregion

        /// <summary>
        /// Initializes country function
        /// </summary>
        private void InitCountry()
        {
            DataTable dtcoubtryList = this.locationDetails.GetActiveCountryIVS();
            this.ddlCountry.DataTextField = "Country";
            this.ddlCountry.DataValueField = "CountryId";
            this.ddlCountry.DataSource = dtcoubtryList;
            this.ddlCountry.DataBind();
            this.ddlCountry.Items.Insert(0, new ListItem("Select", string.Empty));
        }
    }
}
