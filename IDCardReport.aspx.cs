
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
    using VMSBusinessLayer;
    using VMSConstants;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// partial class for report
    /// </summary>    
    public partial class IDCardReport : System.Web.UI.Page
    {
#pragma warning disable CS0169 // The field 'IDCardReport.count' is never used
        /// <summary>
        /// The count field
        /// </summary>        
        private static int count;
#pragma warning restore CS0169 // The field 'IDCardReport.count' is never used
        
        /// <summary>
        /// The applicant details field
        /// </summary>        
        private static DataTable dtapplicantdetails = new DataTable();

        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();

        /// <summary>
        /// The VerifyRenderingInServerForm method
        /// </summary>
        /// <param name="control">The control parameter</param>        
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.txtFromDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                this.txtToDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            }

            this.lblSuccessMessage.Visible = false;
        }

        /// <summary>
        /// The Search Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Btnsearch_Click(object sender, EventArgs e)
        {
            this.lblSuccessMessage.Visible = false;
            try
            {
                EmployeeBL objEmployeeBL = new EmployeeBL();

                string fromdate;
                string todate;
                DataTable dtidcarddistinct = new DataTable();
                fromdate = this.txtFromDate.Text.ToString();
                string[] fromdte = fromdate.Split('/');
                fromdate = fromdte[2] + "-" + fromdte[1] + "-" + fromdte[0];
                todate = this.txtToDate.Text.ToString();
                string[] todte = todate.Split('/');
                todate = todte[2] + "-" + todte[1] + "-" + todte[0];
                dtapplicantdetails = objEmployeeBL.GetApplicantNoImageReport(Session["LoginID"].ToString().Trim(), fromdate, todate);
                if (dtapplicantdetails != null)
                {
                    if (dtapplicantdetails.Rows.Count > 0)
                    {
                        this.imbExcel.Visible = true;
                        this.lblSuccessMessage.Visible = true;
                        this.lblSuccessMessage.Text = dtapplicantdetails.Rows.Count.ToString() + ": records found";
                        this.grdOuter.DataSource = dtapplicantdetails;
                        this.grdOuter.DataBind();
                    }
                    else
                    {
                        this.imbExcel.Visible = false;
                        this.lblSuccessMessage.Visible = true;
                        this.lblSuccessMessage.Text = "No records found";
                    }
                }
                else
                {
                    this.lblSuccessMessage.Text = "No records found";
                    this.lblSuccessMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Excel_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Imbexcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.PrepareGridViewForExport(this.grdOuter);
                this.ExportGridView();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        
        /// <summary>
        /// The Clear_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            dtapplicantdetails = null;
            try
            {
                Response.Redirect("IDCardReport.aspx", true);
                 
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
        }
        
        /// <summary>
        /// The Outer_RowDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdOuter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            EmployeeBL objEmployeeBL = new EmployeeBL();
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
                {
                    string applicantid = Convert.ToString(this.grdOuter.DataKeys[e.Row.RowIndex].Values[0].ToString());                 
                    string fromdate;
                    string todate;
                    fromdate = this.txtFromDate.Text.ToString();
                    string[] fromdte = fromdate.Split('/');
                    fromdate = fromdte[2] + "-" + fromdte[1] + "-" + fromdte[0];
                    todate = this.txtToDate.Text.ToString();
                    string[] todte = todate.Split('/');
                    todate = todte[2] + "-" + todte[1] + "-" + todte[0];                  
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        
        /// <summary>
        /// The Outer_RowCommand method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdOuter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "ShowHistory")
            {
                this.hdnApplicantId.Value = XSS.HtmlEncode(e.CommandArgument.ToString());
                this.PopulateDetailsGrid();
                this.modalGeneratedDates.Show();
            }
        }

        /// <summary>
        /// The PopulateDetailsGrid method
        /// </summary>        
        protected void PopulateDetailsGrid()
        {
            try
            {
                EmployeeBL objEmployeeBL = new EmployeeBL();
                DataTable dtapplicantdetails = new DataTable();
                string fromdate;
                string todate;
                fromdate = this.txtFromDate.Text.ToString();
                string[] fromdte = fromdate.Split('/');
                fromdate = fromdte[2] + "-" + fromdte[1] + "-" + fromdte[0];
                todate = this.txtToDate.Text.ToString();
                string[] todte = todate.Split('/');
                todate = todte[2] + "-" + todte[1] + "-" + todte[0];

                dtapplicantdetails = objEmployeeBL.GetApplicantNoImage(this.hdnApplicantId.Value.ToString());
                if (dtapplicantdetails.Rows.Count > 0)
                {
                    this.grdNoImageReport.DataSource = dtapplicantdetails;
                    this.grdNoImageReport.DataBind();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// The No Image Report_PageIndexChanging method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdNoImageReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.grdNoImageReport.PageIndex = e.NewPageIndex;
            this.PopulateDetailsGrid();
        }

        /// <summary>
        /// The PrepareGridViewForExport method
        /// </summary>
        /// <param name="gv">The parameter</param>        
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
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
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
        /// The ShowPopUpMessage method
        /// </summary>
        /// <param name="strNoRecordsMessage">The NoRecordsMessage parameter</param>        
        private void ShowPopUpMessage(string strNoRecordsMessage)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "NoRecordsFound", "alert('" + strNoRecordsMessage + "');", true);
        }

        /// <summary>
        /// The ExportGridView method
        /// </summary>        
        private void ExportGridView()
        {
            try
            {
                if (this.grdOuter.Rows.Count != 0)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.Cache.SetCacheability(HttpCacheability.Private);
                    Response.AddHeader("content-disposition", "attachment;filename=TrackDetails.xls");
                    Response.Charset = string.Empty;
                    Response.ContentType = "application/vnd.xls";
                    StringBuilder sb = new StringBuilder();
                    System.IO.StringWriter stringWrite = new System.IO.StringWriter(sb);
                    System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                    this.grdOuter.RenderControl(htmlWrite);
                    Response.Write(stringWrite.ToString());
                    stringWrite.Close();
                    htmlWrite.Close();
                    Response.End();
                }
                else
                {
                    string strNoRecordsMessage = "Zero records matched the selected criteria to generate the selected report.";
                    this.ShowPopUpMessage(strNoRecordsMessage);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The BindingGrid method
        /// </summary>        
        private void BindingGrid()
        {
            try
            {
                string purpose = string.Empty;
                this.grdOuter.DataSource = this.griddata;
                this.grdOuter.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The GridViewState method
        /// </summary>
        /// <param name="panel">The panel parameter</param>        
        private void GridViewState(Panel panel)
        {
            string[] statusArray = this.hfGvStatus.Value.Split('|');
            string[] panelArray = this.hfGvPanel.Value.Split('|');
            for (int i = 0; i < statusArray.Length; i++)
            {
                if (panel.ClientID == panelArray[i])
                {
                    if (statusArray[i] == "E")
                    {
                        panel.Attributes.Add("style", "display:");
                    }
                    else
                    {
                        panel.Attributes.Add("style", "display:none");
                    }
                }
            }
        }
    }
}
