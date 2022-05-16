//-----------------------------------------------------------------------
// <copyright file="AccesscardReport.aspx.cs" company="MyCompanyName">
//     Copyright (c) MyCompanyName. All rights reserved.
// </copyright>
// <summary>
// This file contains AccesscardReport class.
// </summary>
//-----------------------------------------------------------------------
namespace VMSDev.Report_OneDayAccessCard
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.Reporting.Common;
    using Microsoft.Reporting.WebForms;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Access card report
    /// </summary>
    public partial class AccesscardReport : System.Web.UI.Page
    {              
       /// <summary>
        /// The ExportToExcel_Click method
        /// </summary>
        /// <param name="startDate">The StartDate parameter</param>
        /// <param name="endDate">The EndDate parameter</param>
        /// <returns>The System.Data.DataTable type object</returns>     
        public DataTable ExportToExcel_Click(DateTime startDate, DateTime endDate)
        {
            VMSDataLayer.VMSDataLayer obj = new VMSDataLayer.VMSDataLayer();
            DataTable reportdt = new DataTable();
            reportdt = obj.AccesscardReport(startDate, endDate, this.ddlCity.SelectedItem.Value, this.ddlFacility.SelectedItem.Value);

            if (this.ddlCity.SelectedItem.Value == "-1" && this.ddlFacility.SelectedItem.Value == "-1")
            {
                string panINDIA = string.Concat("The report will be generated for PAN INDIA.");
                string strScript = string.Concat("<script>alert('", panINDIA, "'); </script>");
                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
            }
            else if (this.ddlCity.SelectedItem.Value != "-1" && this.ddlFacility.SelectedItem.Value == "-1")
            {
                string cityReport = string.Concat("The report will be generated for " + XSS.HtmlEncode(this.ddlCity.SelectedItem.Text) + " city.");
                string strScript = string.Concat("<script>alert('", cityReport, "'); </script>");
                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
            }
            else
            {
                string facilityreport = string.Concat("The report will be generated for " + XSS.HtmlEncode(this.ddlCity.SelectedItem.Text) + " " + XSS.HtmlEncode(this.ddlFacility.SelectedItem.Text) + " facility.");
                string strScript = string.Concat("<script>alert('", facilityreport, "'); </script>");
                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
            }

           string cardtype = reportdt.Columns["CardType"].ToString();
           DataTable newreport = new DataTable();
           newreport.Columns.Add("AssociateID", typeof(int));
           newreport.Columns.Add("CardType", typeof(string));
           newreport.Columns.Add("IDCardIssuedDateTime", typeof(string));
           newreport.Columns.Add("AccessCardIssuedDateTime", typeof(string));
           newreport.Columns.Add("IDCardReturnedDateTime", typeof(string));
           newreport.Columns.Add("AccessCardReturnedDateTime", typeof(string));
           newreport.Columns.Add("AccessCardNumber", typeof(string));
           newreport.Columns.Add("PassIssuedCity", typeof(string));
           newreport.Columns.Add("IssuedFacility", typeof(string));
           newreport.Columns.Add("BadgeStatus_Description", typeof(string));
           newreport.Columns.Add("Badgestatus", typeof(string));
            ////added by ram(445894) for temp access card 
           newreport.Columns.Add("Reason", typeof(string));
           newreport.Columns.Add("ToDate", typeof(string));
           newreport.Columns.Add("PassReturnedCity", typeof(string));
           newreport.Columns.Add("ReturnedFacility", typeof(string));

           string idcardissued = string.Empty;
           string accesscardissued = string.Empty;
           string idcardreturn = string.Empty;
           string accesscardreturn = string.Empty;
           string issuereason = string.Empty;
           string card = string.Empty;

            if (reportdt.Rows.Count > 0)
            {
            foreach (DataRow dr in reportdt.Rows)
            {
                ////newreport.Columns["AssociateID"] = dr["AssociateID"];1 Day ID Card

                string[] badgestatussplit = dr["BadgeStatus_Description"].ToString().Split(' ');
                if (dr["CardType"].ToString() == "1 Day ID Card")
                {
                    if (dr["BadgeStatus_Description"].ToString() == "Issued" || dr["BadgeStatus_Description"].ToString() == "Returned")
                    {
                        idcardissued = dr["PassIssuedDate"].ToString();
                        accesscardissued = null;
                        idcardreturn = dr["PassReturneddate"].ToString();
                        accesscardreturn = null;
                    }
                    else if (dr["BadgeStatus_Description"].ToString() != "Issued" && dr["BadgeStatus_Description"].ToString() != "Returned")
                    {
                        idcardissued = dr["PassIssuedDate"].ToString();
                        accesscardissued = null;
                        idcardreturn = null;
                        accesscardreturn = null;
                    }

                    card = "Temporary ID Card";
                }  
                else if (dr["CardType"].ToString() == "1 Day Access Card")  
                {
                    if (dr["BadgeStatus_Description"].ToString() == "Issued" || dr["BadgeStatus_Description"].ToString() == "Returned")
                    {
                        idcardissued = null;
                        accesscardissued = dr["PassIssuedDate"].ToString();
                        idcardreturn = null;
                        accesscardreturn = dr["PassReturneddate"].ToString();
                    }
                    else if (dr["BadgeStatus_Description"].ToString() != "Issued" && dr["BadgeStatus_Description"].ToString() != "Returned")
                    {
                        idcardissued = null;
                        accesscardissued = dr["PassIssuedDate"].ToString();
                        idcardreturn = null;
                        accesscardreturn = null;
                    }

                    card = "Temporary Access Card";
                }
                else if (dr["CardType"].ToString() == "1 day ID Card and Access Card")
                {
                    if (dr["BadgeStatus_Description"].ToString() == "Issued" || dr["BadgeStatus_Description"].ToString() == "Returned")
                    {
                        idcardissued = dr["PassIssuedDate"].ToString();
                        accesscardissued = dr["PassIssuedDate"].ToString();
                        idcardreturn = dr["PassReturneddate"].ToString();
                        accesscardreturn = dr["PassReturneddate"].ToString();
                    }
                    else if (badgestatussplit[0] == "LostID") 
                    {
                        idcardissued = dr["PassIssuedDate"].ToString();
                        accesscardissued = dr["PassIssuedDate"].ToString();
                        idcardreturn = null;
                        accesscardreturn = dr["PassReturneddate"].ToString();
                    }
                    else if (badgestatussplit[0] == "LostAccCard") 
                    {
                        idcardissued = dr["PassIssuedDate"].ToString();
                        accesscardissued = dr["PassIssuedDate"].ToString();
                        idcardreturn = dr["PassReturneddate"].ToString();
                        accesscardreturn = null;
                    }
                    else if (badgestatussplit[0] == "LostBoth") 
                    {
                        idcardissued = dr["PassIssuedDate"].ToString();
                        accesscardissued = dr["PassIssuedDate"].ToString();
                        idcardreturn = null;
                        accesscardreturn = null;
                    }

                    card = "Temporary ID and Access Card";
                }
               ////added by ram(445894) for temp access card
                if (dr["Reason"].ToString() == "20")
                {
                    issuereason = "Forgot";
                }
                else if (dr["Reason"].ToString() == "21")
                {
                    issuereason = "OnSite Return";
                }
                else if (dr["Reason"].ToString() == "22")
                {
                    issuereason = "Lost";
                }

                newreport.Rows.Add(dr["AssociateID"], card, idcardissued, accesscardissued, idcardreturn, accesscardreturn, dr["AccessCardNumber"], dr["PassIssuedCity"], dr["IssuedFacility"], dr["BadgeStatus_Description"], dr["Badgestatus"], issuereason, dr["ToDate"], dr["PassReturnedCity"], dr["ReturnedFacility"]);
            }
            }

            this.ExportDataSetToExcel(newreport, "NewReportDataSet");
            return reportdt;
        }

        /// <summary>
        /// The Export Data Set To Excel method
        /// </summary>
        /// <param name="newreport">The new report parameter</param>
        /// <param name="reportName">The report Name parameter</param>        
        public void ExportDataSetToExcel(DataTable newreport, string reportName)
        {
            try
            {
                this.ReportViewer.Reset();
                if (newreport != null && newreport.Rows.Count > 0)
                {
                    string reportPath = Server.MapPath(string.Empty) + "\\\\" + "NewReportAccessCard.rdlc"; 
                    this.ReportViewer.ProcessingMode = ProcessingMode.Local;
                    this.ReportViewer.LocalReport.ReportPath = reportPath;
                    ReportDataSource datasource;
                    datasource = new ReportDataSource("NewReportDataSet", newreport);
                    this.ReportViewer.LocalReport.DataSources.Clear();
                    this.tblReports.Width = "500%";
                    this.tblReports.Height = "500%";
                    this.tblReports.Align = "Center";      
                    this.ReportViewer.LocalReport.DataSources.Add(datasource);
                }

                this.ReportViewer.LocalReport.Refresh();
                this.ReportViewer.Visible = true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
            }
        }
        
        /// <summary>
        /// The Cities method
        /// </summary>        
        public void InitCities()
        {
            try
            {
                this.ddlCity.Items.Clear();
                this.ddlCity.Enabled = true;
                VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL();
                DataTable cities = locationDetails.GetActiveCities("101");
                this.ddlCity.DataTextField = "LocationCity";
                this.ddlCity.DataValueField = "LocationCity";
                this.ddlCity.DataSource = cities;
                this.ddlCity.DataBind();
                this.ddlCity.Items.Insert(0, new ListItem("Select", "-1"));
                //// InitFacilities();
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
                this.ddlFacility.Items.Clear();
                VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL();
                DataTable facilities = locationDetails.GetActiveFacilities(this.ddlCity.SelectedItem.Text);
                this.ddlFacility.DataTextField = "LocationName";
                this.ddlFacility.DataValueField = "LocationId";
                this.ddlFacility.DataSource = facilities;
                this.ddlFacility.DataBind();
                this.ddlFacility.Items.Insert(0, new ListItem("Select", "-1"));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        
        /// <summary>
        /// The CheckCity method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        public void CheckCity(object source, ServerValidateEventArgs args)
        {
            if (this.ddlCity.SelectedItem.Value == "-1")
            {
                args.IsValid = false;
            }
        }

        /// <summary>
        /// The CheckFacility method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        public void CheckFacility(object source, ServerValidateEventArgs args)
        {
            if (this.ddlFacility.SelectedItem.Value == "-1")
            {
                args.IsValid = false;
            }
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
                    this.InitCities();
                    this.InitFacilities();
            }
        }

        /// <summary>
        /// The Report Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Report_Btn_Click(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                DateTime stdt = new DateTime();
                stdt = Convert.ToDateTime(this.FromDate.Text);
                DateTime etdt = new DateTime();
                etdt = Convert.ToDateTime(this.ToDate.Text);
                this.ExportToExcel_Click(stdt, etdt);
            }
        }

        /// <summary>
        /// The Reset Click method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void LbtnReset_Click(object source, EventArgs e)
        {
            this.FromDate.Text = string.Empty;
            this.ToDate.Text = string.Empty;
            this.ddlCity.SelectedIndex = -1;
            this.ddlFacility.SelectedIndex = -1;
            this.ReportViewer.Visible = false;
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
                this.InitFacilities();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
