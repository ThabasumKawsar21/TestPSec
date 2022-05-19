

namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using VMSBusinessLayer;
    using VMSConstants;
    using XSS = CAS.Security.Application.EncodeHelper;
    ////CR---IVS01062010CR09- newly added page

    /// <summary>
    /// Class IVS No image Report
    /// </summary>
    public partial class IVSNoimageReport : System.Web.UI.Page
    {
        /// <summary>
        /// Data for associate details field
        /// </summary>        
        private static DataTable dtassociatedetails = new DataTable();

        /// <summary>
        /// Object for Location Details field
        /// </summary>        
        private LocationBL objLocationDetails = new LocationBL();

        /// <summary>
        /// Method to populate all Cities
        /// </summary>
        /// <param name="countryId">Country Id</param>    
        public void InitCities(string countryId)
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable cities = locationDetails.GetActiveCities(countryId);
                this.ddlLocation.DataTextField = "LocationCity";
                this.ddlLocation.DataValueField = "LocationCity";
                this.ddlLocation.DataSource = cities;
                this.ddlLocation.DataBind();
                this.ddlLocation.Items.Insert(0, new ListItem("Select", string.Empty));
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
                VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable facilities = locationDetails.GetActiveFacilities(this.ddlLocation.SelectedItem.Text);
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
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = this.btnSearch.UniqueID;

            if (!Page.IsPostBack)
            {
                ////#region commenting
                ////////FillControlValues();
                //////InitCountry();
                //////InitCities(drpCountry.SelectedItem.Value);
                //////InitFacilities();
                //////lblSuccessMessage.Visible = false;
                //////grdEmployee.AllowPaging = true;
                ////#endregion
                ////// redirecting to 1CAAP
                this.Form.Target = "_blank";
                try
                {
                    Response.Redirect("1CAAPReportsPage.aspx", true);
                    
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }

            ////if (Request.Form["__EVENTARGUMENT"] == "ResetPrint")
            ////{
            ////    grdEmployee.AllowPaging = true;
            ////}
        }

        /// <summary>
        /// The Country field Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DrpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.InitCities(this.drpCountry.SelectedItem.Value);
            this.InitFacilities();
        }

        /// <summary>
        /// The Location Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.InitFacilities();
        }

        /// <summary>
        /// The Search Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The events parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            if (this.drpCountry.SelectedIndex == 0 && string.IsNullOrEmpty(this.txtEmpID.Text))
            {
                this.errortbl.Visible = true;
                this.lblMessage.Text = VMSConstants.COUNTRYSELECTCRITERIA;
                this.btnExcel.Enabled = false;
                this.lblSuccessMessage.Visible = false;
                this.grdEmployee.Visible = false;
                this.lblSuccessMessage.Visible = false;
            }
            else
            {
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                dtassociatedetails = this.objLocationDetails.Assoiciateswithoutphoto(XSS.HtmlEncode(this.txtEmpID.Text.Trim()), XSS.HtmlEncode(this.ddlLocation.SelectedItem.Text), XSS.HtmlEncode(this.ddlFacility.SelectedItem.Text), XSS.HtmlEncode(this.drpCountry.SelectedItem.Value));
                if (!string.IsNullOrEmpty(this.txtEmpID.Text))
                {
                    if (objEmployeeDetails.ValidateAssociateDetails(this.txtEmpID.Text))
                    {
                        if (dtassociatedetails.Rows.Count == 0)
                        {
                            this.errortbl.Visible = true;
                            this.grdEmployee.Visible = false;
                            this.lblMessage.Text = VMSConstants.NORECORDFOUND;
                            this.btnExcel.Enabled = false;
                            this.lblSuccessMessage.Visible = false;
                        }
                        else
                        {
                            this.grdEmployee.Visible = true;
                            this.errortbl.Visible = false;
                            this.grdEmployee.DataSource = dtassociatedetails;
                            this.grdEmployee.DataBind();
                            this.btnExcel.Enabled = true;
                            this.lblSuccessMessage.Visible = true;
                            this.lblSuccessMessage.Text = VMSConstants.TOTAL + dtassociatedetails.Rows.Count.ToString();
                            ////grdEmployee1.DataSource = dtassociatedetails;
                            ////grdEmployee1.DataBind();
                            ////grdEmployee1.Visible = true;
                        }
                    }
                    else
                    {
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = VMSConstants.VALIDASSOCIATEID;
                        this.grdEmployee.Visible = false;
                        this.btnExcel.Enabled = false;
                        this.lblSuccessMessage.Visible = false;
                    }
                }
                else
                {
                    if (dtassociatedetails.Rows.Count == 0)
                    {
                        this.errortbl.Visible = true;
                        this.grdEmployee.Visible = false;
                        this.lblMessage.Text = VMSConstants.NORECORDFOUND;
                        this.btnExcel.Enabled = false;
                        this.lblSuccessMessage.Visible = false;
                    }
                    else
                    {
                        this.grdEmployee.Visible = true;
                        this.errortbl.Visible = false;
                        this.grdEmployee.DataSource = dtassociatedetails;
                        this.grdEmployee.DataBind();
                        this.btnExcel.Enabled = true;
                        this.lblSuccessMessage.Visible = true;
                        this.lblSuccessMessage.Text = VMSConstants.TOTAL + dtassociatedetails.Rows.Count.ToString();
                        ////grdEmployee1.DataSource = dtassociatedetails;
                        ////grdEmployee1.DataBind();
                        ////grdEmployee1.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// The Clear button Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.FillControlValues();
            this.txtEmpID.Text = string.Empty;
            this.errortbl.Visible = false;
            this.grdEmployee.Visible = false;
            this.btnExcel.Enabled = false;
            this.lblSuccessMessage.Visible = false;
        }

        /// <summary>
        /// The Employee grid Page Index Changing method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The events parameter</param>        
        protected void GrdEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdEmployee.PageIndex = e.NewPageIndex;
                this.grdEmployee.DataSource = dtassociatedetails;
                this.grdEmployee.DataBind();
                ////grdEmployee1.DataSource = dtassociatedetails;
                ////grdEmployee1.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Excel button Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.PrepareGridViewForExport(this.grdEmployee);
            this.ExportGridView();
        }

        /// <summary>
        /// Load Country method
        /// </summary>        
        private void InitCountry()
        {
            VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
            DataTable dtcountrylist = locationDetails.GetActiveCountry();
            this.drpCountry.DataTextField = "Country";
            this.drpCountry.DataValueField = "CountryId";
            this.drpCountry.DataSource = dtcountrylist;
            this.drpCountry.DataBind();
            this.drpCountry.Items.Insert(0, new ListItem("Select", string.Empty));
        }

        /// <summary>
        /// The FillControlValues method
        /// </summary>        
        private void FillControlValues()
        {
            try
            {
                ////grdEmployee1.Visible = false;
                DataTable dtlocation = new LocationBL().GetCityDetails();
                this.ddlLocation.DataSource = dtlocation;
                this.ddlLocation.DataBind();
                this.ddlLocation.Items.Insert(0, new ListItem(VMSConstants.SELECT, "-1"));
                this.ddlLocation.Items.Insert(1, new ListItem("All", "-2"));
                this.ddlFacility.Items.Clear();
                this.ddlFacility.Items.Insert(0, new ListItem(VMSConstants.SELECT, "-1"));
                this.lblSuccessMessage.Visible = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Prepare Grid View For Export method
        /// </summary>
        /// <param name="gv">The grid view parameter</param>        
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

        /// <summary>
        /// The ExportGridView method
        /// </summary>        
        private void ExportGridView()
        {
            HtmlForm form = new HtmlForm();
            string attachment = "attachment; filename=" + DateTime.Now + "_IVS_Report.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/ms-excel";
            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            DataGrid dg = new DataGrid();
            dg.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            dg.HeaderStyle.BackColor = System.Drawing.Color.SkyBlue;
            dg.HeaderStyle.Font.Bold = true;
            dg.ItemStyle.ForeColor = System.Drawing.Color.Black;
            dg.ItemStyle.BackColor = System.Drawing.Color.White;
            dg.AlternatingItemStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dg.DataSource = dtassociatedetails;
            dg.DataBind();
            form.Controls.Add(dg);
            this.Controls.Add(form);
            form.RenderControl(htextw);
            Response.Write(stw.ToString());
            stw.Close();
            htextw.Close();
            Response.End();
        }
    }
    ////CR---IVS01062010CR09--end
}
