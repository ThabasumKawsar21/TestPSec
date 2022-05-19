//-----------------------------------------------------------------------
// <copyright file="ImageBulkUpload.aspx.cs" company="CTS">
//     Copyright (c) MyCompanyName. All rights reserved. 
// </copyright>
// <summary>
// This file contains ImageBulkUpload interface.
// </summary>
//-----------------------------------------------------------------------
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Image bulk upload
    /// </summary>
    public partial class ImageBulkUpload : System.Web.UI.Page
    {
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
            if (!Page.IsPostBack)
            {
                this.pnlViewBtns.Visible = false;
                this.pnlListView.Visible = false;
                this.pnlGridView.Visible = false;
            }
        }

        /// <summary>
        /// The ExcelIcon_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgExcelIcon_Click(object sender, EventArgs e)
        {
            this.ExportGridView();
        }

        /// <summary>
        /// The GetImages_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnGetImages_Click(object sender, EventArgs e)
        {
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            try
            {
                ////hdnBulkUploadID.Value = "10"; Session["BulkUploadID"]
                if (this.hdnBulkUploadID.Value != null)
                {
                    ////this.hdnBulkUploadID.Value = Session["BulkUploadID"].ToString();
                    DataTable dt = objEmployeeDetails.GetBulkUploadDetails(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim())).Tables[1];
                    this.hdnTotalValue.Value = dt.Rows[0]["TotalCount"].ToString().Trim();

                    string strSuccessCount = string.Empty;
                    strSuccessCount = dt.Rows[0]["SuccessCount"].ToString().Trim();

                    int total, success;

                    total = Convert.ToInt32(this.hdnTotalValue.Value);
                    success = Convert.ToInt32(strSuccessCount);

                    if (total * success == 0)
                    {
                        this.lblMessage.Text = "No files were uploaded";
                        this.lblMessage.ForeColor = System.Drawing.Color.Red;
                        this.lblMessage.Attributes.Add("style", "text-decoration:blink;");
                    }
                    else
                    {
                        this.lblMessage.Text = string.Concat(strSuccessCount + " out of " + XSS.HtmlEncode(this.hdnTotalValue.Value) + " files uploaded successfully. It may take 30 minutes to reflect the image.");
                        this.lblMessage.ForeColor = System.Drawing.Color.Green;
                        this.lblMessage.Attributes.Add("style", "text-decoration:none;");
                        this.BindDataList(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim()), 0, 21);
                        this.Session["BulkUploadID"] = null;
                    }
                }
                else
                {
                    this.lblMessage.Text = "Bulk upload of images unsuccessful";
                    this.lblMessage.ForeColor = System.Drawing.Color.Red;
                    this.lblMessage.Attributes.Add("style", "text-decoration:blink;");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Images_ItemDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void LstImages_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                Image imgFile = (Image)e.Item.FindControl("imgFile");
                Label lblFileName = (Label)e.Item.FindControl("lblFileName");
                Label lblMessageID = (Label)e.Item.FindControl("lblMessageID");
                Image imgStatus = (Image)e.Item.FindControl("imgStatus");

                if (string.Equals(lblMessageID.Text.Trim(), ConfigurationManager.AppSettings["SuccessMsgID"].ToString().Trim()))
                {
                    string encryptedData = VMSBusinessLayer.Encrypt(lblFileName.Text.Trim());
                    string strWidth = "80";
                    string strHeight = "80";
                    imgFile.ImageUrl = string.Concat("AssociateImage.aspx?ID=", encryptedData, "&w=", strWidth, "&h=", strHeight);
                    imgStatus.ImageUrl = "~/Images/approve.png";
                }
                else if (string.Equals(lblMessageID.Text.Trim(), ConfigurationManager.AppSettings["InvalidFormatMsgID"].ToString().Trim()))
                {
                    imgFile.Height = 80;
                    imgFile.ImageUrl = "~/Images/InvalidFile.png";
                    imgStatus.ImageUrl = "~/Images/rejected.png";
                }
                else if (string.Equals(lblMessageID.Text.Trim(), ConfigurationManager.AppSettings["InvalidAssociateMsgID"].ToString().Trim()))
                {
                    imgFile.Height = 80;
                    imgFile.ImageUrl = "~/Images/InvalidID.png";
                    imgStatus.ImageUrl = "~/Images/rejected.png";
                }
                else if (string.Equals(lblMessageID.Text.Trim(), ConfigurationManager.AppSettings["AlreadyExistsMsgID"].ToString().Trim()))
                {
                    string encryptedData = VMSBusinessLayer.Encrypt(lblFileName.Text.Trim());
                    string strWidth = "80";
                    string strHeight = "80";
                    imgFile.ImageUrl = string.Concat("AssociateImage.aspx?ID=", encryptedData, "&w=", strWidth, "&h=", strHeight);
                    imgStatus.ImageUrl = "~/Images/warning.png";
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GridView_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgGridView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                ////DataSet dsImages = objEmployeeDetails.GetBulkUploadDetails(9);
                ////grdUploadDetails.DataSource = dsImages.Tables[0];
                this.grdUploadDetails.DataSource = objEmployeeDetails.GetBulkUploadDetails(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim())).Tables[0]; ;//// this.sdsUploadDetails;
                this.grdUploadDetails.DataBind();
                this.ToggleView("GRID");
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Previous_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void LnkPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                int totalCount = Convert.ToInt32(this.hdnTotalValue.Value);
                int prev = Convert.ToInt32(this.hdnStartValue.Value.ToString().Trim());
                int next = Convert.ToInt32(this.hdnLastValue.Value.ToString().Trim());

                this.hdnLastValue.Value = XSS.HtmlEncode(this.hdnStartValue.Value);
                next = prev;
                prev -= 21;

                if (prev <= 0)
                {
                    this.lnkPrevious.Enabled = false;
                    this.lnkNext.Enabled = true;
                    this.EnablePrevNext("FIRST");
                }
                else
                {
                    this.lnkPrevious.Enabled = true;
                    this.EnablePrevNext("0");
                }

                this.BindDataList(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim()), prev, 21);
                this.hdnStartValue.Value = prev.ToString();
                this.hdnLastValue.Value = next.ToString();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The IconView_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgIconView_Click(object sender, ImageClickEventArgs e)
        {
            this.BindDataList(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim()), 0, 21);
        }

        /// <summary>
        /// The UploadDetails_PageIndexChanging method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdUploadDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdUploadDetails.DataSource = this.sdsUploadDetails;
                this.grdUploadDetails.PageIndex = e.NewPageIndex;
                this.grdUploadDetails.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Sorting method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdSorting(object sender, GridViewSortEventArgs e)
        {
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            try
            {
                DataSourceSelectArguments args = new DataSourceSelectArguments();
                DataView view = objEmployeeDetails.GetBulkUploadDetails(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim())).Tables[0].DefaultView;
                view.Sort = e.SortExpression + " " + this.GetSortDirection(e.SortExpression);
                this.grdUploadDetails.DataSource = view;
                this.grdUploadDetails.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The UploadDetails_RowDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdUploadDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Next_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void LnkNext_Click(object sender, EventArgs e)
        {
            try
            {
                int totalCount = Convert.ToInt32(this.hdnTotalValue.Value);
                int prev = Convert.ToInt32(this.hdnStartValue.Value.ToString().Trim());
                int next = Convert.ToInt32(this.hdnLastValue.Value.ToString().Trim());

                prev = next;
                next += 21;

                if (next >= totalCount)
                {
                    this.lnkNext.Enabled = false;
                    this.lnkPrevious.Enabled = true;
                    next = totalCount;
                    this.EnablePrevNext("LAST");
                }
                else
                {
                    this.lnkNext.Enabled = true;
                    this.EnablePrevNext("0");
                }

                this.BindDataList(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim()), prev, 21);
                this.hdnStartValue.Value = prev.ToString();
                this.hdnLastValue.Value = next.ToString();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The BindDataList method
        /// </summary>
        /// <param name="bulkUploadID">The BulkUploadID parameter</param>
        /// <param name="startValue">The startValue parameter</param>
        /// <param name="intCount">The Count parameter</param>        
        private void BindDataList(int bulkUploadID, int startValue, int intCount)
        {
            try
            {
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                DataSet dsimages = objEmployeeDetails.GetNextBulkUploadDetails(bulkUploadID, startValue, intCount);
                this.lstImages.DataSource = dsimages.Tables[0];
                this.lstImages.DataBind();

                this.hdnStartValue.Value = "0";

                if (Convert.ToInt32(this.hdnTotalValue.Value.ToString().Trim()) < 21)
                {
                    this.lnkNext.Visible = false;
                    this.lnkPrevious.Visible = false;
                    this.hdnLastValue.Value = XSS.HtmlEncode(this.hdnTotalValue.Value);
                }
                else
                {
                    this.lnkNext.Visible = true;
                    this.lnkPrevious.Visible = true;
                    this.hdnLastValue.Value = "21";
                    this.EnablePrevNext("FIRST");
                }

                this.ToggleView("ICON");
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The ExportGridView method
        /// </summary>        
        private void ExportGridView()
        {
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            try
            {
                DataTable dt = objEmployeeDetails.GetBulkUploadDetails(Convert.ToInt32(this.hdnBulkUploadID.Value.ToString().Trim())).Tables[0];

                dt.Columns.Remove("BulkUploadID");
                dt.Columns.Remove("DetailsID");
                dt.Columns.Remove("MessageID");
                dt.Columns["IsSuccess"].ColumnName = "Upload Successful";
                string attachment = "attachment; filename=BulkUploadDetails.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = string.Empty;
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }

                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = string.Empty;
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }

                    Response.Write("\n");
                }

                Response.End();
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
        /// The Enable Previous Next method
        /// </summary>
        /// <param name="strPage">The Page parameter</param>        
        private void EnablePrevNext(string strPage)
        {
            try
            {
                switch (strPage.ToUpper())
                {
                    case "FIRST":
                        {
                            this.lnkPrevious.ImageUrl = "Images\\prevDataList_disabled.png";
                            this.lnkNext.ImageUrl = "Images\\nextDataList.png";
                            break;
                        }

                    case "LAST":
                        {
                            this.lnkPrevious.ImageUrl = "Images\\prevDataList.png";
                            this.lnkNext.ImageUrl = "Images\\nextDataList_disabled.png";
                            break;
                        }

                    default:
                        {
                            this.lnkNext.ImageUrl = "Images\\nextDataList.png";
                            this.lnkPrevious.ImageUrl = "Images\\prevDataList.png";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The ToggleView method
        /// </summary>
        /// <param name="strView">The View parameter</param>        
        private void ToggleView(string strView)
        {
            try
            {
                this.pnlViewBtns.Visible = true;
                switch (strView.ToUpper())
                {
                    case "ICON":
                        {
                            this.imgGridView.ImageUrl = "Images\\GridView_disabled.png";
                            this.imgIconView.ImageUrl = "Images\\IconView.png";
                            this.pnlGridView.Visible = false;
                            this.pnlListView.Visible = true;
                            break;
                        }

                    case "GRID":
                        {
                            this.imgGridView.ImageUrl = "Images\\GridView.png";
                            this.imgIconView.ImageUrl = "Images\\IconView_disabled.png";
                            this.pnlGridView.Visible = true;
                            this.pnlListView.Visible = false;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The GetSortDirection method
        /// </summary>
        /// <param name="column">The column parameter</param>
        /// <returns>The string type object</returns>        
        private string GetSortDirection(string column)
        {
            try
            {
                SortDirection sd = SortDirection.Ascending;
                string sortDirection = "ASC";
                string sortExpression = ViewState["SortExpression"] as string;
                if (sortExpression != null)
                {
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["SortDirection"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                            sd = SortDirection.Descending;
                        }
                    }
                }

                this.ViewState["SortDirection"] = sortDirection;
                this.ViewState["SortDirectionExp"] = sd;
                this.ViewState["SortExpression"] = column;
                return sortDirection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
