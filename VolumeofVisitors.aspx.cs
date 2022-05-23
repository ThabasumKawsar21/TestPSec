

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Linq;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using VMSBusinessEntity;

    /// <summary>
    /// Volume of visitors
    /// </summary>
    public partial class VolumeofVisitors : System.Web.UI.Page
    {
        /// <summary>
        /// The Search field
        /// </summary>        
        private object searchParamObj = new object();
        
        /// <summary>
        /// The Grid data field
        /// </summary>        
       private DataSet griddata = new DataSet();
        
        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.txtDate.Attributes.Add("readonly", "readonly");
                this.txtdayFromDate.Attributes.Add("readonly", "readonly");
                this.txtdayTodate.Attributes.Add("readonly", "readonly");
                this.txtMonthFromdate.Attributes.Add("readonly", "readonly");
                this.txtMonthTodate.Attributes.Add("readonly", "readonly");
                this.txtWeekFromDate.Attributes.Add("readonly", "readonly");
                this.txtWeekToDate.Attributes.Add("readonly", "readonly");           
                if (!Page.IsPostBack)
                {
                    this.lblTitle.Visible = false;
                    this.lblResult.Visible = false;
                    this.tblByHour.Visible = false;
                    this.tblByDay.Visible = false;
                    this.tblByMonth.Visible = false;
                    this.tblByWeek.Visible = false;
                    this.btnGenerateReport.Visible = false;
                    this.grdResult.Visible = false;
                    this.btnExport.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The RadioButtonList1_SelectedIndexChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.RadioButtonList1.SelectedIndex == 0)
                {
                    this.lblTitle.Visible = true;
                    this.lblTitle.Text = "Reports By Hour";
                    this.grdResult.Visible = false;
                    this.tblByHour.Visible = true;
                    this.txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    ////txtFromTime.Text = System.DateTime.Now.ToString("hh:mm:ss");
                    ////txtToTime.Text = System.DateTime.Now.ToString("hh:mm:ss");
                    this.txtFromTime.Text = VMSConstants.VMSConstants.FROMTIME;
                    this.txtToTime.Text = VMSConstants.VMSConstants.TOTIME;
                    this.tblByDay.Visible = false;
                    this.tblByMonth.Visible = false;
                    this.tblByWeek.Visible = false;
                    this.btnGenerateReport.Visible = true;
                    this.lblResult.Visible = false;
                    this.btnExport.Visible = false;
                }

                if (this.RadioButtonList1.SelectedIndex == 1)
                {
                    this.lblTitle.Visible = true;
                    this.lblTitle.Text = "Reports By Day";
                    this.grdResult.Visible = false;
                    this.tblByDay.Visible = true;
                    this.tblByMonth.Visible = false;
                    this.tblByWeek.Visible = false;
                    this.tblByHour.Visible = false;
                    this.btnGenerateReport.Visible = true;
                    this.txtdayFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtdayTodate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    this.lblResult.Visible = false;
                    this.btnExport.Visible = false;
                }

                if (this.RadioButtonList1.SelectedIndex == 2)
                {
                    this.lblTitle.Visible = true;
                    this.lblTitle.Text = "Reports By Week";
                    this.grdResult.Visible = false;
                    this.tblByWeek.Visible = true;
                    this.tblByDay.Visible = false;
                    this.tblByMonth.Visible = false;
                    this.tblByHour.Visible = false;
                    this.btnGenerateReport.Visible = true;
                    this.txtWeekFromDate.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                    this.txtWeekToDate.Text = Convert.ToDateTime(this.txtWeekFromDate.Text).AddDays(7).ToString("yyyy/MM/dd");
                    this.txtWeekFromDate.Text = Convert.ToDateTime(this.txtWeekFromDate.Text).ToString("dd/MM/yyyy");
                    this.txtWeekToDate.Text = Convert.ToDateTime(this.txtWeekToDate.Text).ToString("dd/MM/yyyy");
                    this.lblResult.Visible = false;
                    this.btnExport.Visible = false;
                }

                if (this.RadioButtonList1.SelectedIndex == 3)
                {
                    this.lblTitle.Visible = true;
                    this.lblTitle.Text = "Reports By Month";
                    this.grdResult.Visible = false;
                    this.tblByMonth.Visible = true;
                    this.tblByWeek.Visible = false;
                    this.tblByDay.Visible = false;
                    this.tblByHour.Visible = false;
                    this.btnGenerateReport.Visible = true;
                    this.txtMonthFromdate.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                    this.txtMonthTodate.Text = Convert.ToDateTime(this.txtMonthFromdate.Text).AddMonths(1).ToString("yyyy/MM/dd");
                    this.txtMonthFromdate.Text = Convert.ToDateTime(this.txtMonthFromdate.Text).ToString("dd/MM/yyyy");
                    this.txtMonthTodate.Text = Convert.ToDateTime(this.txtMonthTodate.Text).ToString("dd/MM/yyyy");
                    this.lblResult.Visible = false;
                    this.btnExport.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GenerateReport_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.GenerateReport();
                ////btnExport.Visible =true;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }  

        /// <summary>
        /// The txtWeekFromDate_TextChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void TxtWeekFromDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtWeekFromDate.Text) || this.txtWeekFromDate.Text == "__/__/____")
                {
                    this.lblResult.Visible = true;
                    this.grdResult.Visible = false;
                    this.btnExport.Visible = false;
                    this.lblResult.Text = "Select the From date";
                }
                else if (string.IsNullOrEmpty(this.txtWeekFromDate.Text) || this.txtWeekFromDate.Text != "__/__/____")
                {
                    string str;
                    str = this.txtWeekFromDate.Text;
                    DateTime week = new DateTime();

                    week = Convert.ToDateTime(str).AddDays(07);
                    this.txtWeekFromDate.Text = Convert.ToDateTime(this.txtWeekFromDate.Text).ToString("dd/MM/yyyy");

                    this.txtWeekToDate.Text = Convert.ToDateTime(week).ToString("dd/MM/yyyy");
                    return;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Month From date_TextChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void TxtMonthFromdate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtMonthFromdate.Text) || this.txtMonthFromdate.Text == "__/__/____")
                {
                    this.lblResult.Visible = true;
                    this.grdResult.Visible = false;
                    this.btnExport.Visible = false;
                    this.lblResult.Text = "Select the From date";
                }
                else if (!string.IsNullOrEmpty(this.txtMonthFromdate.Text) || this.txtMonthFromdate.Text != "__/__/____")
                {
                    string str;
                    str = this.txtMonthFromdate.Text;
                    DateTime month = new DateTime();

                    month = Convert.ToDateTime(str).AddMonths(1);
                    this.txtMonthFromdate.Text = Convert.ToDateTime(this.txtMonthFromdate.Text).ToString("dd/MM/yyyy");

                    this.txtMonthTodate.Text = Convert.ToDateTime(month).ToString("dd/MM/yyyy");
                    return;
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
        /// The txtToTime_TextChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void TxtToTime_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The GenerateReport method
        /// </summary>        
        private void GenerateReport()
        {
            try
            {
                if (this.tblByHour.Visible == true)
                {
                    if (string.IsNullOrEmpty(this.txtDate.Text) || string.IsNullOrEmpty(txtFromTime.Text) || string.IsNullOrEmpty(txtToTime.Text))
                    {
                        this.lblResult.Text = "Select";
                        this.btnExport.Visible = false;
                        this.grdResult.Visible = false;

                        if (string.IsNullOrEmpty(this.txtDate.Text) || this.txtDate.Text == "__/__/____")
                        {
                            this.lblResult.Text = " Date";
                        }

                        if (string.IsNullOrEmpty(txtFromTime.Text) || this.txtFromTime.Text == "__:__")
                        {
                            if (this.lblResult.Text == "Select")
                            {
                                this.lblResult.Text += " From Time ";
                            }
                            else
                            {
                                this.lblResult.Text += " ,From Time ";
                            }
                        }

                        if (string.IsNullOrEmpty(txtToTime.Text) || this.txtToTime.Text == "__:__")
                        {
                            if (this.lblResult.Text == "Select")
                            {
                                this.lblResult.Text += " To Time ";
                            }
                            else
                            {
                                this.lblResult.Text += " ,To Time ";
                            }
                        }

                        this.lblResult.Visible = true;
                    }
                    else
                    {
                        ////Griddata = RequestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, null, null, txtDate.Text, txtFromTime.Text, txtToTime.Text, null, "Submitted");
                        ////Begin Changes for VMS CR17 07Mar2011 Vimal
                        this.griddata = this.requestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, null, null, null, this.txtDate.Text, this.txtFromTime.Text, this.txtToTime.Text, null, "Submitted");
                        ////End Changes for VMS CR17 07Mar2011 Vimal
                        this.BindwithGrid();
                    }
                }

                if (this.tblByDay.Visible == true)
                {
                    if (string.IsNullOrEmpty(this.txtdayFromDate.Text) || string.IsNullOrEmpty(txtdayTodate.Text))
                    {
                        this.lblResult.Visible = true;
                        this.btnExport.Visible = false;
                        this.grdResult.Visible = false;
                        if (string.IsNullOrEmpty(this.txtdayFromDate.Text))
                        {
                            this.lblResult.Text = "Select the FromDate ";
                        }

                        if (string.IsNullOrEmpty(txtdayTodate.Text))
                        {
                            this.lblResult.Text = "Select the ToDate ";
                        }

                        if (string.IsNullOrEmpty(this.txtdayFromDate.Text) && string.IsNullOrEmpty(txtdayTodate.Text))
                        {
                            this.lblResult.Text = "Select the FromDate and ToDate ";
                        }
                    }
                    else
                    {
                        string[] str1 = this.txtdayFromDate.Text.Split('/');
                        string[] str2 = this.txtdayTodate.Text.Split('/');

                        DateTime dt1 = Convert.ToDateTime(str1[1] + "/" + str1[0] + "/" + str1[2]);
                        DateTime dt2 = Convert.ToDateTime(str2[1] + "/" + str2[0] + "/" + str2[2]);

                        if (dt1 > dt2)
                        {
                            this.lblResult.Visible = true;
                            this.btnExport.Visible = false;
                            this.grdResult.Visible = false;
                            this.lblResult.Text = "To date should be greater than fromdate";
                        }
                        else
                        {
                           ////Griddata = RequestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, txtdayFromDate.Text, txtdayTodate.Text, null, null, null, null, "Submitted");
                            ////Begin Changes for VMS CR17 07Mar2011 Vimal
                            this.griddata = this.requestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, this.txtdayFromDate.Text, this.txtdayTodate.Text, null, null, null, null, null, "Submitted");
                            ////End Changes for VMS CR17 07Mar2011 Vimal
                            this.BindwithGrid();
                        }
                    }
                }

                if (this.tblByWeek.Visible == true)
                {
                    if (this.txtWeekFromDate.Text == "__/__/____" && this.txtWeekToDate.Text == "__/__/____")
                    {
                        this.lblResult.Visible = true;
                        this.btnExport.Visible = false;
                        this.grdResult.Visible = false;
                        this.lblResult.Text = "Select the FromDate and ToDate";
                    }
                    else if (this.txtWeekFromDate.Text == "__/__/____")
                    {
                        this.lblResult.Visible = true;
                        this.btnExport.Visible = false;
                        this.grdResult.Visible = false;
                        this.lblResult.Text = "Select the FromDate";
                    }
                    else if (this.txtWeekToDate.Text == "__/__/____")
                    {
                        this.lblResult.Visible = true;
                        this.btnExport.Visible = false;
                        this.grdResult.Visible = false;
                        this.lblResult.Text = "Select the Todate";
                    }
                    else
                    {
                        string[] str1 = this.txtWeekFromDate.Text.Split('/');
                        string[] str2 = this.txtWeekToDate.Text.Split('/');

                        DateTime dt1 = Convert.ToDateTime(str1[1] + "/" + str1[0] + "/" + str1[2]);
                        DateTime dt2 = Convert.ToDateTime(str2[1] + "/" + str2[0] + "/" + str2[2]);

                        if (dt1 > dt2)
                        {
                            this.lblResult.Visible = true;
                            this.btnExport.Visible = false;
                            this.grdResult.Visible = false;
                            this.lblResult.Text = "To date should be greater than fromdate";
                        }
                        else
                        {
                            ////Griddata = RequestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, txtWeekFromDate.Text, txtWeekToDate.Text, null, null, null, null, "Submitted");
                            ////Begin Changes for VMS CR17 07Mar2011 Vimal
                            this.griddata = this.requestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, this.txtWeekFromDate.Text, this.txtWeekToDate.Text, null, null, null, null, null, "Submitted");
                            ////End Changes for VMS CR17 07Mar2011 Vimal
                            this.BindwithGrid();
                        }
                    }
                }

                if (this.tblByMonth.Visible == true)
                {
                    if (string.IsNullOrEmpty(txtMonthFromdate.Text) || string.IsNullOrEmpty(this.txtMonthTodate.Text))
                    {
                        this.lblResult.Visible = true;
                        this.btnExport.Visible = false;
                        this.grdResult.Visible = false;
                        this.lblResult.Text = "Select the FromDate and ToDate";
                    }
                   else
                    {
                        string[] str1 = this.txtMonthFromdate.Text.Split('/');
                        string[] str2 = this.txtMonthTodate.Text.Split('/');

                        DateTime dt1 = Convert.ToDateTime(str1[1] + "/" + str1[0] + "/" + str1[2]);
                        DateTime dt2 = Convert.ToDateTime(str2[1] + "/" + str2[0] + "/" + str2[2]);

                        if (dt1 > dt2)
                        {
                            this.lblResult.Visible = true;
                            this.btnExport.Visible = false;
                            this.grdResult.Visible = false;
                            this.lblResult.Text = "To date should be greater than fromdate";
                        }
                        else
                        {
                            ////Griddata = RequestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, txtMonthFromdate.Text, txtMonthTodate.Text, null, null, null, null, "Submitted");
                            ////Begin Changes for VMS CR17 07Mar2011 Vimal
                            this.griddata = this.requestDetails.SearchVisitorInfo(null, null, null, null, null, null, null, null, null, null, null, this.txtMonthFromdate.Text, this.txtMonthTodate.Text, null, null, null, null, null, "Submitted");
                            ////End Changes for VMS CR17 07Mar2011 Vimal
                            this.BindwithGrid();
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
        /// The Bind with Grid method
        /// </summary>        
        private void BindwithGrid()
        {
            try
            {
                if (this.griddata.Tables[0].Rows.Count > 0)
                {
                    this.grdResult.Visible = true;
                    this.lblResult.Visible = false;
                    this.grdResult.DataSource = this.griddata;
                    this.btnExport.Visible = true;
                    this.grdResult.DataBind();
                }
                else
                {
                    this.lblResult.Visible = true;
                    this.grdResult.Visible = false;
                    this.btnExport.Visible = false;
                    this.lblResult.Text = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
