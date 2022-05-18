
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
    using VMSBusinessEntity;
    using VMSBusinessLayer;
    using VMSConstants;

    /// <summary>
    /// The data by Department partial class
    /// </summary>     
    public partial class DatabyDepartmentHost : System.Web.UI.Page
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
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblResult.Visible = false;
                this.btnExport.Visible = false;
                if (!Page.IsPostBack)
                {
                    this.InitDepartments();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        ////protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        //    if (ddlDepartment.SelectedIndex != 0)
        //    {
        //        txtHost.Enabled = false;

        ////    }

        ////}

        /// <summary>
        /// The GenerateReport_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtHost.Text) && this.ddlDepartment.SelectedIndex == 0)
                {
                    this.grdResult.Visible = false;

                    this.lblResult.Visible = true;
                    this.lblResult.Text = "Select atleast one search option";
                }
                else
                {
                    ////Begin Changes for VMS CR17 07Mar2011 Vimal
                    ////Griddata = RequestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, txtHost.Text, null, null, null, null, null, ddlDepartment.SelectedItem.Text, "Submitted");
                    this.griddata = this.requestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, this.txtHost.Text, null, null, null, null, null, null, this.ddlDepartment.SelectedItem.Text, "Submitted");
                    ////End Changes for VMS CR17 07Mar2011 Vimal
                    if (this.griddata.Tables[0].Rows.Count > 0)
                    {
                        this.grdResult.Visible = true;
                        this.lblResult.Visible = false;
                        this.grdResult.DataSource = this.griddata;
                        this.grdResult.DataBind();
                        this.btnExport.Visible = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(this.txtHost.Text) && this.ddlDepartment.SelectedIndex != 0)
                        {
                            this.lblResult.Visible = true;
                            this.grdResult.Visible = false;

                            this.lblResult.Text = "No Record Found or Mismatch of Host and Department";
                        }
                        else
                        {
                            this.lblResult.Visible = true;
                            this.grdResult.Visible = false;

                            this.lblResult.Text = "No Record Found ";
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
        /// The Export_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            HtmlForm form = new HtmlForm();

            string attachment = "attachment; filename=Employee.xls";

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

        ////protected void txtHost_TextChanged(object sender, EventArgs e)
        ////{
        //    if (txtHost.Text != string.Empty)
        //    {
        //        ddlDepartment.Enabled = false;
        //    }

        ////}

        /// <summary>
        /// The Result_SelectedIndexChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdResult_SelectedIndexChanged(object sender, EventArgs e)
        {
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
    }
}
