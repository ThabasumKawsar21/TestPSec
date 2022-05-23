//-----------------------------------------------------------------------
// <copyright file="MyDetails.aspx.cs" company="CTS">
//     Copyright (c) MyCompanyName. All rights reserved. 
// </copyright>
// <summary>
// This file contains MyDetails class.
// </summary>
//-----------------------------------------------------------------------
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;

    /// <summary>
    /// partial class My details
    /// </summary>
    public partial class MyDetails : System.Web.UI.Page
    {
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strUserID = Session["LoginID"].ToString();
                this.ucEmpInfo.UserID = strUserID;
                this.ShowPanels(strUserID);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Upload method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            string strFileName = string.Empty;
            try
            {
                // if (UploadImage.FileName != string.Empty)
                string strPostedFileName = this.UploadImage.PostedFile.FileName.ToString();
                if (!string.IsNullOrEmpty(strPostedFileName))
                {
                    string strFileExtension = System.IO.Path.GetExtension(strPostedFileName).Replace('.', ' ').Trim().ToLower();
                    string strypes = System.Configuration.ConfigurationManager.AppSettings["SupportedFileTypes"].Trim().ToString();
                    List<string> stringtoList = new List<string>();
                    stringtoList.AddRange(strypes.Split(','));
                    if (stringtoList.Contains(strFileExtension))
                    {
                        HttpPostedFile filetoUpload = this.UploadImage.PostedFile;
                        string[] arrFileName = Path.GetFileName(filetoUpload.FileName).Split('.');
                        string imageName = arrFileName[0].ToString();
                        string loginID = Session["LoginID"].ToString().Trim();
                        if (imageName.Equals(loginID))
                        {
                            string serverPath = Server.MapPath(@"AssociateImage/");
                            strFileName = string.Concat(serverPath, "Associate", "-", DateTime.Now.ToString("dd-MM-yy"), " ", DateTime.Now.ToString("hh-mm-ss-fff"), ".", strFileExtension);
                            filetoUpload.SaveAs(strFileName);
                            EmployeeBL objEmployeeDetails = new EmployeeBL();
                            if (objEmployeeDetails.SubmitImageChangeRequest(imageName, strFileName, loginID, string.Empty))
                            {
                                string str0 = "<script language='javascript'> alert('Image Submitted Successfully');</script>";
                                ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", str0);
                                this.Page_Load(sender, e);
                                this.DeleteFile(strFileName);
                                VMSBusinessLayer.RequestDetailsBL requestdetails = new VMSBusinessLayer.RequestDetailsBL();
                                this.SendEmail(loginID);
                            }
                            else
                            {
                                this.lblMessage1.InnerText = VMSConstants.VMSConstants.IMAGEUPLOADFAILED;
                                this.lblMessage1.Visible = true;
                                this.ModalPopupExtender1.Show();
                            }
                        }
                        else
                        {
                            this.lblMessage1.InnerText = VMSConstants.VMSConstants.IMAGENAME;
                            this.lblMessage1.Visible = true;
                            this.ModalPopupExtender1.Show();
                        }
                    }
                    else
                    {
                        this.lblMessage1.InnerText = VMSConstants.VMSConstants.INVALIDFILEFORMAT;
                        this.lblMessage1.Visible = true;
                        this.ModalPopupExtender1.Show();
                    }
                }
                else
                {
                    this.lblMessage1.InnerText = VMSConstants.VMSConstants.SELECTFILE;
                    this.lblMessage1.Visible = true;
                    this.lblMessage1.Style.Add("color", "Red");
                    this.ModalPopupExtender1.Show();
                }

                this.chkDisclaimer.Checked = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Cancel method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnCancel2_Click(object sender, ImageClickEventArgs e)
        {
            this.ModalPopupExtender1.Hide();
        }

        /// <summary>
        /// The work flow grid method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdWorkflow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblAction = (Label)e.Row.FindControl("lblAction");
            if (lblAction != null)
            {
                if (lblAction.Text.ToUpper().Contains("REJECTED"))
                {
                    lblAction.ForeColor = System.Drawing.Color.Maroon;
                }
            }
        }

        /// <summary>
        /// Method to Show Image Upload/WorkFlow Panels
        /// </summary>
        /// <param name="strLoginId">The login id parameter</param>
        private void ShowPanels(string strLoginId)
        {
            try
            {
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                DataSet dsrequestDetails = objEmployeeDetails.ImgChangeRequestDetails(strLoginId);

                if (dsrequestDetails.Tables[0].Rows.Count > 0)
                {
                    string status = dsrequestDetails.Tables[0].Rows[0]["Status"].ToString().Trim();
                    DataTable dt = new DataTable();
                    DataColumn dc;
                    dc = new DataColumn("Associate");
                    dt.Columns.Add(dc);
                    dc = new DataColumn("Action");
                    dt.Columns.Add(dc);
                    dc = new DataColumn("Status");
                    dt.Columns.Add(dc);
                    dc = new DataColumn("DateofRequest");
                    dt.Columns.Add(dc);
                    DateTime dtdateTime = Convert.ToDateTime(dsrequestDetails.Tables[0].Rows[0]["UploadedOn"].ToString().Trim());
                    string strDateTime = dtdateTime.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    dt.Rows.Add(string.Concat(dsrequestDetails.Tables[0].Rows[0]["AssociateName"].ToString(), " (", dsrequestDetails.Tables[0].Rows[0]["AssociateID"].ToString(), ")"), VMSConstants.VMSConstants.SUBMITTED, "Submitted", strDateTime);
                    string encryptedData = VMSBusinessLayer.Encrypt(dsrequestDetails.Tables[0].Rows[0]["RequestID"].ToString().Trim());
                    switch (status.ToUpper())
                    {
                        case "SUBMITTED":
                            {
                                this.pnlWorkFlow.Visible = true;
                                this.pnlImageUpload.Visible = false;
                                this.grdWorkflow.DataSource = dt;
                                this.grdWorkflow.DataBind();
                                this.imgUploadImage.ImageUrl = string.Concat("AssociateImage.aspx?Key=", encryptedData);
                                this.pnlImage.Visible = true;
                                this.imgUploadImage.Visible = true;
                                break;
                            }

                        case "APPROVED":
                            {
                                DateTime dtdateTime2 = Convert.ToDateTime(dsrequestDetails.Tables[0].Rows[0]["ApprovedOn"].ToString().Trim());
                                string strDateTime2 = dtdateTime2.ToString("dd-MMM-yyyy hh:mm:ss tt");
                                dt.Rows.Add(string.Concat(dsrequestDetails.Tables[0].Rows[0]["ApproverName"].ToString(), " (", dsrequestDetails.Tables[0].Rows[0]["ApproverID"].ToString(), ")"), VMSConstants.VMSConstants.APPROVED, "Approved", strDateTime2);
                                this.grdWorkflow.DataSource = dt;
                                this.grdWorkflow.DataBind();
                                this.pnlWorkFlow.Visible = true;
                                this.pnlImageUpload.Visible = false;
                                this.imgUploadImage.Visible = false;
                                this.pnlImage.Visible = false;
                                break;
                            }

                        case "REJECTED":
                            {
                                DateTime dtdateTime2 = Convert.ToDateTime(dsrequestDetails.Tables[0].Rows[0]["ApprovedOn"].ToString().Trim());
                                string strDateTime2 = dtdateTime2.ToString("dd-MMM-yyyy hh:mm:ss tt");
                                dt.Rows.Add(string.Concat(dsrequestDetails.Tables[0].Rows[0]["ApproverName"].ToString(), " (", dsrequestDetails.Tables[0].Rows[0]["ApproverID"].ToString(), ")"), string.Concat(VMSConstants.VMSConstants.REJECTED, " ", dsrequestDetails.Tables[0].Rows[0]["ApproverComment"].ToString()), "Rejected", strDateTime2);
                                this.grdWorkflow.DataSource = dt;
                                this.grdWorkflow.DataBind();

                                this.pnlWorkFlow.Visible = true;
                                this.pnlImageUpload.Visible = true;
                                this.imgUploadImage.Visible = false;
                                this.pnlImage.Visible = false;
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    this.pnlWorkFlow.Visible = false;
                    this.pnlImageUpload.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The DeleteFile method
        /// </summary>
        /// <param name="path">The path parameter</param>        
        private void DeleteFile(string path)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Delete);
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// The SendEmail method
        /// </summary>
        /// <param name="loginID">The loginID parameter</param>        
        private void SendEmail(string loginID)
        {
            try
            {
                VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
                VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                VMSBusinessLayer.UserDetails<string, string, string, string> userDetails = new VMSBusinessLayer.UserDetails<string, string, string, string>();
                userDetails = userDetailsBL.GetUserDetails(loginID);
                string strAssociateName = userDetails.AssociateName;
                string strAssociateEmailID = requestDetails.GetHostmailID(loginID);
                requestDetails.IVSImageSubmittedMail(strAssociateName, DateTime.Now, strAssociateEmailID);
            }
            catch
            {
                throw;
            }
        }
    }
}
