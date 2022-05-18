
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using VMSDev.DocUploadServiceRef;

    /// <summary>
    /// get image approved partial class
    /// </summary>
    public partial class ApproveImage : System.Web.UI.Page
    {
#pragma warning disable CS0414 // The field 'ApproveImage.value' is assigned but its value is never used
        /// <summary>
        /// The value field
        /// </summary>        
        private int value = 0;
#pragma warning restore CS0414 // The field 'ApproveImage.value' is assigned but its value is never used

        /// <summary>
        /// The File upload Response field
        /// </summary>        
        private MFileuploadResponse objMFileuploadResponse
            = new MFileuploadResponse();

        /// <summary>
        /// The File Upload Details Request field
        /// </summary>        
        private FileUploadDetailsRequest objFileUploadDetailsRequest
            = new FileUploadDetailsRequest();

        /// <summary>
        /// The Decrypt Binary Data method
        /// </summary>
        /// <param name="strEncrpytedDataImg">The Encrypted Data Image parameter</param>
        /// <returns>The string type object</returns>        
        public string DecryptBinaryData(string strEncrpytedDataImg)
        {
            return new EncryptDecrypt().Decrypt(strEncrpytedDataImg, "CTS", true);
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EmployeeBL objempbl = new EmployeeBL();
                this.lblUploadFailure.Text = string.Empty;

                if (!Page.IsPostBack)
                {

                    this.ViewState["SortDirection"] = "ASC";
                    this.ViewState["SortDirectionExp"] = SortDirection.Ascending;
                    this.ViewState["SortExpression"] = "UploadedOn";
                    this.grdImageChangeRequests.DataSource = objempbl.GetAllImageChangeRequests(); ////this.SQLDataSource;
                    this.grdImageChangeRequests.DataBind();
                    this.lblUploadFailure.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Row Data Bound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                foreach (GridViewRow grdRow in this.grdImageChangeRequests.Rows)
                {
                    Label lblRequestID = (Label)grdRow.FindControl("lblReqID");
                    Label lblAssociateID = (Label)grdRow.FindControl("lblAssociateID");

                    System.Web.UI.WebControls.Image imgOldSmall
                        = (System.Web.UI.WebControls.Image)grdRow.FindControl("imgOldSmall");
                    System.Web.UI.WebControls.Image imgOldBig
                        = (System.Web.UI.WebControls.Image)grdRow.FindControl("imgOldBig");
                    System.Web.UI.WebControls.Image imgNewSmall
                        = (System.Web.UI.WebControls.Image)grdRow.FindControl("imgNewSmall");
                    System.Web.UI.WebControls.Image imgNewBig
                        = (System.Web.UI.WebControls.Image)grdRow.FindControl("imgNewBig");

                    string requestID = lblRequestID.Text.Trim();
                    string associateID = lblAssociateID.Text.Trim();

                    string encryptedRequestID = VMSBusinessLayer.Encrypt(requestID);
                    string encryptedAssociateID = VMSBusinessLayer.Encrypt(associateID);

                    imgOldSmall.ImageUrl = string.Concat("AssociateImage.aspx?ID=", encryptedAssociateID);
                    ////imgOldBig.ImageUrl 
                    ////= string.Concat("AssociateImage.aspx?AssociateID=", encryptedAssociateID); 
                    imgOldBig.ImageUrl = string.Concat("AssociateImage.aspx?ID=", encryptedAssociateID);

                    imgNewSmall.ImageUrl = string.Concat("AssociateImage.aspx?Key=", encryptedRequestID);
                    imgNewBig.ImageUrl = string.Concat("AssociateImage.aspx?Key=", encryptedRequestID);

                    DropDownList ddlReason = (DropDownList)grdRow.FindControl("ddlComments");
                    this.BindDropDown(ddlReason);

                    ImageButton imgApproval = (ImageButton)grdRow.FindControl("btnApprove");
                    ImageButton imgReject = (ImageButton)grdRow.FindControl("btnReject");

                    imgApproval.CommandArgument = string.Concat(
                        requestID,
                        "|",
                        ddlReason.SelectedItem.Text,
                        "|",
                        associateID);
                    imgReject.CommandArgument = string.Concat(
                        requestID,
                        "|",
                        ddlReason.SelectedItem.Text,
                        "|",
                        associateID);

                    Label lblUploadedDate = (Label)grdRow.FindControl("lblUploadedOn");
                    DateTime dtdateTime = Convert.ToDateTime(lblUploadedDate.Text);
                    lblUploadedDate.Text = dtdateTime.ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Row Command method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper().Equals("APPROVE") || (e.CommandName.ToUpper() == "REJECT"))
                {
                    bool isapprove = false;
                    EmployeeBL objBusinessLayer = new EmployeeBL();
                    string[] commandArgsAccept = e.CommandArgument.ToString().Split(new char[] { '|' });
                    int intRequestID = Convert.ToInt32(commandArgsAccept[0].ToString());
                    int associateId = Convert.ToInt32(commandArgsAccept[2].ToString());
                    string loginID = Session["LoginID"].ToString().Trim();
                    string strApproverComments = commandArgsAccept[1].ToString();
                    string returnId = string.Empty;

                    if (strApproverComments.ToUpper().Equals("--SELECT A REASON--"))
                    {
                        string str0
                            = "<script language='javascript'> alert('Please Select a Comment');</script>";
                        ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", str0);

                        return;
                    }
                    else
                    {
                        if (e.CommandName.ToUpper().Equals("APPROVE"))
                        {
                            if (strApproverComments.ToUpper().Equals("APPROVED"))
                            {
                                isapprove = true;
                                this.value = 1;
                            }
                            else
                            {
                                string str0
                                    = "<script language='javascript'> alert('Invalid Comment Selected');</script>";
                                ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", str0);
                                return;
                            }
                        }
                        else if (e.CommandName.ToUpper().Equals("REJECT"))
                        {
                            if (strApproverComments.ToUpper().Equals("APPROVED"))
                            {
                                string str0
                                    = "<script language='javascript'> alert('Invalid Comment Selected');</script>";
                                ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", str0);
                                return;
                            }
                            else
                            {
                                isapprove = false;
                            }
                        }

                        EmployeeBL objEmployeeBL = new EmployeeBL();
                        var result = objEmployeeBL.GetUploadedImageDetails(intRequestID.ToString()).Split('|');
                        string strImage = result[0];
                        string strfileContentId = result[1];
                        bool isFilecontentId = Convert.ToBoolean(result[2]);
                        byte[] data = null;
                        if (!string.IsNullOrEmpty(strImage) && isFilecontentId == false)
                        {
                            string strDecryptedBinaryData = this.DecryptBinaryData(strImage);
                            data = Encoding.Default.GetBytes(strDecryptedBinaryData);
                        }

                        if (isFilecontentId && !string.IsNullOrEmpty(strfileContentId))
                        {
                            data = this.GetImageFromSAN(strfileContentId);
                        }

                        if (isapprove == true)
                        {
                            string filename = string.Concat(associateId.ToString(), ".jpeg");
                            if (data != null)
                            {
                                returnId = this.SaveToSANStorage(data, filename, associateId, loginID);
                            }

                            if (!string.IsNullOrEmpty(returnId))
                            {
                                VMSBusinessLayer.RequestDetailsBL objStoreFileContentID
                                    = new VMSBusinessLayer.RequestDetailsBL();
                                bool storeResult
                                    = objStoreFileContentID.StoreFileContentID(
                                    associateId.ToString(), returnId, System.Configuration.ConfigurationManager.AppSettings["AppKey"].ToString());
                                if (storeResult)
                                {
                                    objBusinessLayer.ApproveRejectImageChangeRequest(
                                        true,
                                        intRequestID,
                                        loginID,
                                        strApproverComments);
                                    ClientScript.RegisterStartupScript(
                                        typeof(Page),
                                        "SubmitStatus",
                                        "<script language='javascript'>alert('Image Approved Successfully.');</script>",
                                        false);
                                    DataSet dsrequestDetails = objBusinessLayer.ImgChangeRequestDetailswithReqID(intRequestID);
                                    ////if (string.Equals(
                                    ////    ConfigurationManager.AppSettings["SendMail"].ToString(), 
                                    ////    "Y"))
                                    if (ConfigurationManager.AppSettings["MailSendRole_enable"].ToString() == "true")
                                    {
                                        this.SendMail(
                                            dsrequestDetails.Tables[0].Rows[0]["AssociateID"].ToString(),
                                            dsrequestDetails.Tables[0].Rows[0]["AssociateName"].ToString(),
                                            Convert.ToDateTime(dsrequestDetails.Tables[0].Rows[0]["UploadedOn"].ToString()),
                                            isapprove,
                                            strApproverComments);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(typeof(Page), "SendMailDisabled", "<script language='javascript'>alert('Mailing Functionality has been disabled.');</script>", false);
                                    }
                                }
                            }
                            else if (string.IsNullOrEmpty(returnId))
                            {
                                this.lblUploadFailure.Text = ConfigurationManager.AppSettings["SANStorageFailure"].ToString();
                            }
                        }
                        else
                        {
                            string strstatus = objEmployeeBL.GetAssociateImageApproveStatus(intRequestID.ToString());
                            if (!string.IsNullOrEmpty(strstatus))
                            {
                                if (strstatus != "Rejected")
                                {
                                    objBusinessLayer.ApproveRejectImageChangeRequest(
                                        false,
                                        intRequestID,
                                        loginID,
                                        strApproverComments);

                                    ////Remove image from outlook
                                    objBusinessLayer.ManageImageOutlookIntegration(associateId.ToString(), false);

                                    ClientScript.RegisterStartupScript(
                                        typeof(Page),
                                        "SubmitStatus",
                                        "<script language='javascript'>alert('Image Rejected Successfully.');</script>",
                                        false);
                                    DataSet dsrequestDetails = objBusinessLayer.ImgChangeRequestDetailswithReqID(intRequestID);
                                    if (string.Equals(ConfigurationManager.AppSettings["SendMail"].ToString(), "Y"))
                                    {
                                        this.SendMail(
                                            dsrequestDetails.Tables[0].Rows[0]["AssociateID"].ToString(),
                                            dsrequestDetails.Tables[0].Rows[0]["AssociateName"].ToString(),
                                            Convert.ToDateTime(dsrequestDetails.Tables[0].Rows[0]["UploadedOn"].ToString()),
                                            isapprove,
                                            strApproverComments);
                                    }
                                }
                            }
                        }

                        this.BindGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Comments Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlCommentsSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlCurrentDropDownList = (DropDownList)sender;
                GridViewRow grdRow = (GridViewRow)ddlCurrentDropDownList.Parent.Parent;

                Label lblRequestID = (Label)grdRow.FindControl("lblReqID");
                ImageButton imgApproved = (ImageButton)grdRow.FindControl("btnApprove");
                ImageButton imgReject = (ImageButton)grdRow.FindControl("btnReject");
                DropDownList ddlReason = (DropDownList)grdRow.FindControl("ddlComments");

                string requestID = lblRequestID.Text.Trim();
                Label lblAssociateID = (Label)grdRow.FindControl("lblAssociateID");
                string associateID = lblAssociateID.Text.Trim();

                imgApproved.CommandArgument = string.Concat(
                    requestID,
                    "|",
                    ddlReason.SelectedItem.Text,
                    "|",
                    associateID);
                imgReject.CommandArgument = string.Concat(
                    requestID,
                    "|",
                    ddlReason.SelectedItem.Text,
                    "|",
                    associateID);
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
            EmployeeBL objempbl = new EmployeeBL();
            try
            {
                DataSet sortdata = objempbl.GetAllImageChangeRequests();
                DataView view = sortdata.Tables[0].DefaultView;  
                view.Sort = e.SortExpression + " " + this.GetSortDirection(e.SortExpression);
                this.grdImageChangeRequests.DataSource = view;
                this.grdImageChangeRequests.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The SaveToSANStorage method
        /// </summary>
        /// <param name="bytes">The bytes parameter</param>
        /// <param name="fileName">The fileName parameter</param>
        /// <param name="associateId">The AssociateId parameter</param>
        /// <param name="createdBy">The CreatedBy parameter</param>
        /// <returns>The string type object</returns>        
        protected string SaveToSANStorage(byte[] bytes, string fileName, int associateId, string createdBy)
        {
            try
            {
                string returnId = string.Empty;
                bytes = this.SaveOptimizedImage(bytes, associateId);
                using (DocumentUploadServiceClient objDocumentUploadServiceClient
                    = new DocumentUploadServiceClient())
                {
                    this.objFileUploadDetailsRequest.AppId
                        = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AppGlobalId"].ToString());
                    this.objFileUploadDetailsRequest.AppTemplateId
                        = System.Configuration.ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                    this.objFileUploadDetailsRequest.FileName = fileName;
                    this.objFileUploadDetailsRequest.IncomingFile = bytes;
                    this.objFileUploadDetailsRequest.CreatedBy = createdBy;
                    this.objFileUploadDetailsRequest.CreatedDate = DateTime.Now;
                    this.objFileUploadDetailsRequest.AssociateId = associateId;
                    this.objMFileuploadResponse
                        = objDocumentUploadServiceClient.UploadFile_WithResponse(
                        this.objFileUploadDetailsRequest);
                    if (string.Equals(this.objMFileuploadResponse.Filestatus, "Failed"))
                    {
                        string strException = this.objMFileuploadResponse.FileException;
                        VMSUtility.VMSUtility.WriteLog(
                            strException,
                            VMSUtility.VMSUtility.LogLevel.Normal);
                    }
                    else
                    {
                        returnId = Convert.ToString(this.objMFileuploadResponse.FileContentId);
                    }

                    return returnId;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// The Image Change Requests Page Index Changing method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdImageChangeRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.BindGrid();
            this.grdImageChangeRequests.PageIndex = e.NewPageIndex;
            this.grdImageChangeRequests.DataBind();
        }

        /// <summary>
        /// Get Associate Image
        /// </summary>
        /// <param name="fileContentID">file Content ID</param>
        /// <returns>byte array</returns>
        private byte[] GetImageFromSAN(string fileContentID)
        {
            try
            {
                byte[] associateImage = null;
                string strMessage = string.Empty;
                FileUploadDC objFileUploadDC = new FileUploadDC();
                MFileuploadResponse objMFileuploadResponses
                    = new MFileuploadResponse();
                FileUploadDetailsRequest objFileUploadDetailsRequests
                    = new FileUploadDetailsRequest();
                using (DocumentUploadServiceClient objDocumentUploadServiceClient
                    = new DocumentUploadServiceClient())
                {
                    // objFileUploadDC.FileUploadId = fileuploadId;
                    objFileUploadDC.FileContentId = new Guid(fileContentID);
                    objFileUploadDC.AppTemplateId
                        = ConfigurationManager.AppSettings["IDcardAppTemplateId"];
                    objMFileuploadResponses
                        = objDocumentUploadServiceClient.DownloadFile(objFileUploadDC);
                    associateImage = objMFileuploadResponses.OutgoingFile;

                    if (associateImage != null)
                    {
                        return associateImage;
                    }
                    else
                    {
                        string strException
                            = "Associate Image return as Null for FileContentId" + fileContentID;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        /// <summary>
        /// The SaveOptimizedImage method
        /// </summary>
        /// <param name="dbyte">The dByte parameter</param>
        /// <param name="associateID">The associateID parameter</param>
        /// <returns>The byte[] type object</returns>        
        private byte[] SaveOptimizedImage(byte[] dbyte, int associateID)
        {
            try
            {
                string filePath = string.Empty;
                string fileName = string.Empty;
                byte[] buffer = dbyte;

                using (MemoryStream ms = new MemoryStream(dbyte, 0, dbyte.Length))
                {
                    Bitmap imgIn = (Bitmap)System.Drawing.Image.FromStream(ms);
                    if (imgIn.Height > 480 || imgIn.Width > 640)
                    {
                        double y = imgIn.Height;
                        double x = imgIn.Width;
                        double factor = 1;
                        int width = 640, height = 480;

                        if (width * height != 0)
                        {
                            if (x > y)
                            {
                                factor = width / x;
                            }
                            else
                            {
                                factor = height / y;
                            }
                        }

                        Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
                        imgOut.SetResolution(120, 120);

                        using (Graphics gr = Graphics.FromImage(imgOut))
                        {
                            gr.Clear(Color.Transparent);
                            ////This is said to give best quality when resizing images   
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.SmoothingMode = SmoothingMode.AntiAlias;
                            gr.CompositingQuality = CompositingQuality.HighQuality;
                            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            gr.DrawImage(
                                imgIn,
                                new Rectangle(
                                    0,
                                    0,
                                    (int)(factor * x),
                                    (int)(factor * y)),
                                    new Rectangle(
                                        0,
                                        0,
                                        (int)x,
                                        (int)y),
                                        GraphicsUnit.Pixel);
                        }

                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        imgOut.Save(outStream, ImageFormat.Jpeg);
                        buffer = outStream.ToArray();
                        fileName = filePath + associateID + ".jpeg";
                    }

                    return buffer;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The BindGrid method
        /// </summary>        
        private void BindGrid()
        {
            EmployeeBL objempbl = new EmployeeBL();
            this.grdImageChangeRequests.DataSource = objempbl.GetAllImageChangeRequests(); ////this.SQLDataSource;
            this.grdImageChangeRequests.DataBind();
        }

        /// <summary>
        /// The BindDropDown method
        /// </summary>
        /// <param name="ddlReason">The Reason parameter</param>        
        private void BindDropDown(DropDownList ddlReason)
        {
            EmployeeBL objBusinessLayer = new EmployeeBL();
            ddlReason.DataSource = objBusinessLayer.GetApproverComments(); ////this.SqlReasonsData;
            ddlReason.DataTextField = "LookUpValue";
            ddlReason.DataValueField = "LookUpID";
            ddlReason.DataBind();
            ddlReason.Items.Insert(0, "--Select a Reason--");
        }

        /// <summary>
        /// The Send Mail method
        /// </summary>
        /// <param name="strUserID">The User ID parameter</param>
        /// <param name="strUserName">The Use rName parameter</param>
        /// <param name="dtsubmittedOn">The Submitted On parameter</param>
        /// <param name="approvalStatus">The Approval Status parameter</param>
        /// <param name="strComments">The Comments parameter</param>        
        private void SendMail(
            string strUserID,
            string strUserName,
            DateTime dtsubmittedOn,
            bool approvalStatus,
            string strComments)
        {
            try
            {
                VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

                string strAssociateEmailID = requestDetails.GetHostmailID(strUserID);
                string strEmployeeName = strUserName;
                string strApprovalStatus = string.Empty;
                if (approvalStatus)
                {
                    strApprovalStatus = "approved";
                }
                else
                {
                    strApprovalStatus = "rejected";
                }

                requestDetails.IVSImageApprovalMail(
                    strEmployeeName,
                    dtsubmittedOn,
                    strApprovalStatus,
                    strAssociateEmailID,
                    strComments);
            }
            catch (System.Net.Mail.SmtpException)
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
