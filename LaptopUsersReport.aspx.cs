
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using VMSBusinessLayer;

    /// <summary>
    /// laptop user report
    /// </summary>
    public partial class LaptopUsersReport : System.Web.UI.Page
    {
        /// <summary>
        /// Method to Confirms that an HtmlForm control is rendered for the  specified ASP.NET server control at run time   
        /// </summary>
        /// <param name="control">The control parameter</param>      
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the  specified ASP.NET server control at run time.
        }
        #region PageLoad
        
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.btnSearch.Focus();
                Page.Form.DefaultButton = this.btnSearch.UniqueID;
                this.errortbl.Visible = false;
                this.lblEmployeeHeader.Visible = false;
                this.txtFromDate.Value = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
                this.ImgStartDate.Attributes.Add("onClick", "javascript:calendarPicker('document.aspnetForm." + this.txtFromDate.ClientID.ToString() + "');");
                this.ImgEndDate.Attributes.Add("onClick", "javascript:calendarPicker('document.aspnetForm." + this.txtToDate.ClientID.ToString() + "');");
                this.txtEmpID.Attributes.Add("onkeypress", "javascript:return SpecialCharacterValidation(this);");
                this.ddlLocation.Items.Insert(0, new ListItem(VMSConstants.VMSConstants.FACILITYSELECT, "0"));
            }
        }
        #endregion

        #region Control Methods
        /// <summary>
        /// Method to search Laptop users report on the basis of different search criteria        
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.ddlLocation.SelectedIndex != 0) || (this.txtFromDate.Value.Length != 0) || (this.txtToDate.Value.Length != 0) || (this.txtEmpID.Text.Length != 0))
                {
                    DateTime dtstartDate = new DateTime();
                    DateTime dtendDate = new DateTime();
                    DateTime currentDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy"));

                    if (this.txtFromDate.Value.Length > 0)
                    {
                        dtstartDate = DateTime.Parse(this.txtFromDate.Value);
                    }

                    if (this.txtToDate.Value.Length > 0)
                    {
                        dtendDate = DateTime.Parse(this.txtToDate.Value);
                    }

                    if ((this.txtFromDate.Value.Length > 0) && (this.txtToDate.Value.Length > 0))
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

                    if (this.txtToDate.Value.Length > 0 && (this.txtFromDate.Value.Length == 0))
                    {
                        this.errortbl.Visible = true;
                        this.lblEmployeeHeader.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.VMSConstants.SELECTSTARTDATE;
                        this.grdEmployee.DataSourceID = string.Empty;
                        this.grdEmployee.EmptyDataText = string.Empty;
                        this.grdEmployee.DataBind();
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
        /// Method to download the Laptop users  report in excel format
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>
        protected void BtnExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (this.grdEmployee.Rows.Count > 0)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.AddHeader("content-disposition", "attachment;filename=LaptopUsersReport.xls");
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

        /// <summary>
        /// Method to clear values     
        /// </summary>
        /// /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.ddlLocation.SelectedIndex = 0;
                this.txtEmpID.Text = string.Empty;
                this.txtFromDate.Value = string.Empty;
                this.txtToDate.Value = string.Empty;
                this.gridtbl.Visible = false;
                this.lblEmployeeHeader.Visible = false;
                this.btnExcel.Enabled = false;
                this.errortbl.Visible = false;
                this.lblMessage.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to rebind the grid view on the basis of paging of grid view      
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
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
        /// Method to clear the grid view values once the location changes  
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
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
        #endregion

        #region Private Methods
        
        /// <summary>
        /// Method to bind the grid view on the basis of search criteria 
        /// </summary>
        /// <returns>The System.Data.DataView type object</returns>        
        private DataView BindEmployeeDetails()
        {
            DataView dv = new DataView();
            try
            {
                this.grdEmployee.Visible = true;
                DataTable dtemployee = new DataTable();
                EmployeeBL objEmployeeList = new EmployeeBL();
                dtemployee = objEmployeeList.GetLaptopUsersList(this.txtEmpID.Text.ToString(), this.ddlLocation.SelectedValue.ToString(), this.txtFromDate.Value.ToString(), this.txtToDate.Value.ToString());
                dv = new DataView(dtemployee);
                this.grdEmployee.DataSource = dtemployee;
                if (dtemployee.Rows.Count > 0)
                {
                    this.btnExcel.Enabled = true;
                    this.grdEmployee.Width = Unit.Percentage(100);
                    this.grdEmployee.Height = Unit.Percentage(98);
                    this.gridtbl.Width = Unit.Percentage(100).ToString();
                    this.gridtbl.Height = Unit.Percentage(98).ToString();
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
                    this.gridtbl.Width = Unit.Percentage(60).ToString();
                    this.gridtbl.Height = Unit.Percentage(.5).ToString();
                }

                this.grdEmployee.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return dv;
        }
        #endregion
    }
}
