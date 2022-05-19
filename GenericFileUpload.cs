

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Web;
    using VMSBusinessLayer;
    using VMSDev.DocUploadServiceRef; 
    using SD = System.Drawing;
    
    /// <summary>
    /// generic file upload
    /// </summary>
    public class GenericFileUpload
    {
        /// <summary>
        /// file upload response
        /// </summary>
        private MFileuploadResponse objMFileuploadResponse = new MFileuploadResponse();

        /// <summary>
        /// file upload details request
        /// </summary>
        private FileUploadDetailsRequest objFileUploadDetailsRequest = new FileUploadDetailsRequest();

        /// <summary>
        /// file upload collection
        /// </summary>
        private FileUploadCollection objFileUploadCollection = new FileUploadCollection();

        /// <summary>
        /// file upload DC
        /// </summary>
        private FileUploadDC objFileUploadDC = new FileUploadDC();

        /// <summary>
        /// upload file function
        /// </summary>
        /// <param name="bytes">byte value</param>
        /// <param name="fileName">file name</param>
        /// <param name="associateID">associate Id value</param>
        /// <param name="loginID">login Id value</param>
        /// <returns>object value</returns>
        public string UploadFile(byte[] bytes, string fileName, string associateID, string loginID)
        {
            string returnId = string.Empty;
            try
            {
                ////string fileName = Path.GetFileName(filePath);
                ////string contenttype = GetContentType(Path.GetExtension(fileName).ToLower());
                bytes = this.SaveOptimizedImage(bytes, associateID);
                using (DocumentUploadServiceClient objDocumentUploadServiceClient 
                    = new DocumentUploadServiceClient())
                {
                    this.objFileUploadDetailsRequest.AppId 
                        = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AppGlobalId"].ToString());
                    this.objFileUploadDetailsRequest.AppTemplateId 
                        = System.Configuration.ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                    this.objFileUploadDetailsRequest.FileName = fileName;
                    this.objFileUploadDetailsRequest.IncomingFile = bytes;
                    this.objFileUploadDetailsRequest.CreatedBy = loginID;
                    this.objFileUploadDetailsRequest.CreatedDate = DateTime.Now;
                    this.objFileUploadDetailsRequest.AssociateId 
                        = Convert.ToInt32(associateID);
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
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return returnId;
        }
        
        /// <summary>
        /// delete function
        /// </summary>
        /// <param name="fileContentID">file content Id</param>
        /// <param name="loginID">login Id</param>
        public void DeleteFile(string fileContentID, string loginID)
        {
            try
            {
                using (DocumentUploadServiceClient objDocumentUploadServiceClient 
                    = new DocumentUploadServiceClient())
                {
                    this.objFileUploadDetailsRequest.FileContentId = new Guid(fileContentID);
                    this.objFileUploadDetailsRequest.CreatedBy = loginID.Trim();
                    this.objFileUploadDetailsRequest.AppTemplateId 
                        = System.Configuration.ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                    this.objMFileuploadResponse 
                        = objDocumentUploadServiceClient.DeleteFileUploadDetails(
                        this.objFileUploadDetailsRequest);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// get associate image
        /// </summary>
        /// <param name="fileContentID">file content Id</param>
        /// <returns>object value</returns>
        public byte[] GetAssociateImage(string fileContentID)
        {
            try
            {
                byte[] associateImage = null;
                string strMessage = string.Empty;

                // string strMessage = "SAN Storage Service is Invoked for File Upload Id: " + fileuploadId;
                //  VMSUtility.VMSUtility.WriteSANLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);
                using (DocumentUploadServiceClient objDocumentUploadServiceClient 
                    = new DocumentUploadServiceClient())
                {
                    ////objFileUploadDC.FileUploadId = fileuploadId;
                    this.objFileUploadDC.FileContentId = new Guid(fileContentID);
                    this.objFileUploadDC.AppTemplateId 
                        = ConfigurationManager.AppSettings["AppTemplateId"];
                    this.objMFileuploadResponse 
                        = objDocumentUploadServiceClient.DownloadFile(this.objFileUploadDC);

                    if (string.Equals(this.objMFileuploadResponse.Filestatus, "Failed"))
                    {
                        string strException = this.objMFileuploadResponse.FileException;
                        VMSUtility.VMSUtility.WriteLog(strException, VMSUtility.VMSUtility.LogLevel.Normal);
                    }

                    associateImage = this.objMFileuploadResponse.OutgoingFile;

                    if (associateImage == null)
                    {
                        ////download image from onboarding
                        this.objFileUploadDC.FileContentId = new Guid(fileContentID);
                        this.objFileUploadDC.AppTemplateId 
                            = ConfigurationManager.AppSettings["OnBoardingAppTemplateId"];
                        this.objMFileuploadResponse 
                            = objDocumentUploadServiceClient.DownloadFile(this.objFileUploadDC);
                        associateImage = this.objMFileuploadResponse.OutgoingFile;
                    }

                    if (associateImage != null)
                    {
                        return associateImage;
                    }
                    else
                    {
                        string strException 
                            = "Associate Image return as Null for FileContentId" + fileContentID;
                        VMSUtility.VMSUtility.WriteSANLog(
                            strException, 
                            VMSUtility.VMSUtility.LogLevel.Normal);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// bulk upload function
        /// </summary>
        /// <param name="bytes">byte value</param>
        /// <param name="fileName">file name</param>
        /// <param name="loginID">login Id</param>
        /// <returns>string value</returns>
        public string BulkUpload(byte[] bytes, string fileName, string loginID)
        {
            try
            {
                bool isvalid = false;
                int intMessageID = 0;
                int intBulkUploadID = 0;
                string associateID = fileName.Split('.')[0].ToString();
                ////   string LoginID = HttpContext.Current.Session["LoginID"].ToString().Trim();

                EmployeeBL objEmployeeDetails = new EmployeeBL();
                if (this.CheckFileExtension(fileName))
                {
                    switch (objEmployeeDetails.ValidateAssociateImageDetails(associateID))
                    {
                        case 1:
                            {
                                isvalid = true;
                                intMessageID = Convert.ToInt16(
                                    ConfigurationManager.AppSettings["SuccessMsgID"].ToString());
                                break;
                            }

                        case 2:
                            {
                                isvalid = false;
                                intMessageID = Convert.ToInt16(
                                    ConfigurationManager.AppSettings["AlreadyExistsMsgID"].ToString());
                                break;
                            }

                        default:
                            {
                                isvalid = false;
                                intMessageID = Convert.ToInt16(
                                    ConfigurationManager.AppSettings["InvalidAssociateMsgID"].ToString());
                                break;
                            }
                    }
                }
                else
                {
                    isvalid = false;
                    intMessageID = Convert.ToInt16(
                        ConfigurationManager.AppSettings["InvalidFormatMsgID"].ToString());
                }

                if (string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["BulkUploadID"])))
                {
                    HttpContext.Current.Session["BulkUploadID"]
                        = objEmployeeDetails.GetBulkUploadId(loginID);
                }

                intBulkUploadID = Convert.ToInt32(
                    HttpContext.Current.Session["BulkUploadID"].ToString().Trim());

                if (isvalid)
                {
                    bytes = this.SaveOptimizedImage(bytes, associateID);
                    using (DocumentUploadServiceClient objDocumentUploadServiceClient
                        = new DocumentUploadServiceClient())
                    {
                        this.objFileUploadDetailsRequest.AssociateId = Convert.ToInt32(associateID);
                        this.objFileUploadDetailsRequest.AppId
                            = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AppGlobalId"].ToString());
                        this.objFileUploadDetailsRequest.AppTemplateId
                            = ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                        this.objFileUploadDetailsRequest.FileName = fileName;
                        this.objFileUploadDetailsRequest.IncomingFile = bytes;
                        this.objFileUploadDetailsRequest.CreatedBy = loginID;
                        this.objFileUploadDetailsRequest.CreatedDate = DateTime.Now;
                        this.objMFileuploadResponse
                            = objDocumentUploadServiceClient.UploadFile_WithResponse(
                            this.objFileUploadDetailsRequest);
                        if (string.Equals(this.objMFileuploadResponse.Filestatus, "Failed"))
                        {
                            string strException = this.objMFileuploadResponse.FileException;
                            VMSUtility.VMSUtility.WriteLog(strException, VMSUtility.VMSUtility.LogLevel.Normal);
                        }
                        else
                        {
                            VMSBusinessLayer.RequestDetailsBL objStoreFileContentID
                                = new VMSBusinessLayer.RequestDetailsBL();
                            bool result = objStoreFileContentID.StoreFileContentID(
                                associateID,
                                Convert.ToString(this.objMFileuploadResponse.FileContentId),
                                System.Configuration.ConfigurationManager.AppSettings["AppKey"].ToString());
                        }
                    }
                }

                objEmployeeDetails.InsertImageuploadDetails(
                    intBulkUploadID,
                    associateID,
                    isvalid,
                    intMessageID);

                return Convert.ToString(HttpContext.Current.Session["BulkUploadID"]);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// save optimized image
        /// </summary>
        /// <param name="bytes">byte value</param>
        /// <returns>object value</returns>
        public byte[] SaveOptimizedImage(byte[] bytes)
        {
            try
            {
                byte[] dbyte = bytes;
                string filePath = string.Empty;
                string fileName = string.Empty;
#pragma warning disable CS0168 // The variable 'strBinaryData' is declared but never used
                string strBinaryData;
#pragma warning restore CS0168 // The variable 'strBinaryData' is declared but never used

                int maxLength = Convert.ToInt32(ConfigurationManager.AppSettings["MaxVMSImageSize"]);

                if (dbyte.Length > maxLength)
                {
                    using (MemoryStream ms = new MemoryStream(dbyte, 0, dbyte.Length))
                    {
                        Bitmap imgIn = (Bitmap)System.Drawing.Image.FromStream(ms);
                        double y = imgIn.Height;
                        double x = imgIn.Width;
                        double factor = 1;

                        int width = Convert.ToInt32(ConfigurationManager.AppSettings["ImageResizeWidth"]);
                        int height = Convert.ToInt32(ConfigurationManager.AppSettings["ImageResizeHeight"]);

                        ////width = 640, height = 480;

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
                        ////    imgOut.SetResolution(120, 120);
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
                                new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), 
                                new Rectangle(0, 0, (int)x, (int)y), 
                                GraphicsUnit.Pixel);
                        }

                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        imgOut.Save(outStream, ImageFormat.Jpeg);
                        ////  strBinaryData = Encoding.Default.GetString(outStream.ToArray());
                        return outStream.ToArray();
                    }
                }
                else
                {
                    // strBinaryData = Encoding.Default.GetString(bytes);
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get content type
        /// </summary>
        /// <param name="contentType">content type</param>
        /// <returns>object value</returns>
        private string GetContentType(string contentType)
        {
            string contenttype = string.Empty;

            switch (contentType)
            {
                case ".jpg":
                    contenttype = "image/jpg";
                    break;

                case ".jpeg":
                    contenttype = "image/jpeg";
                    break;

                case ".png":
                    contenttype = "image/png";
                    break;

                case ".gif":
                    contenttype = "image/gif";
                    break;

                case ".bmp":
                    contenttype = "image/bmp";
                    break;
                default:
                    break;
            }

            return contenttype;
        }

        /// <summary>
        /// save optimized image
        /// </summary>
        /// <param name="bytes">bytes value</param>
        /// <param name="associateID">associate Id</param>
        /// <returns>object value</returns>
        private byte[] SaveOptimizedImage(byte[] bytes, string associateID)
        {
            byte[] dbyte = bytes;
            try
            {
#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
                bool result = false;
#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used
                string filePath = string.Empty;
                string fileName = string.Empty;

                if (dbyte.Length > 204800)
                {
                    using (MemoryStream ms = new MemoryStream(dbyte, 0, dbyte.Length))
                    {
                        Bitmap imgIn = (Bitmap)System.Drawing.Image.FromStream(ms);
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
                                new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), 
                                new Rectangle(0, 0, (int)x, (int)y), 
                                GraphicsUnit.Pixel);
                        }

                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        imgOut.Save(outStream, ImageFormat.Jpeg);
                        dbyte = outStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return dbyte;
        }

        // Method to validate the file extension

        /// <summary>
        /// check file extension
        /// </summary>
        /// <param name="strfilePath">file path</param>
        /// <returns>object value</returns>
        private bool CheckFileExtension(string strfilePath)
        {
            bool imageStatus = false;
            string strypes = ConfigurationManager.AppSettings["SupportedFileTypes"].ToString().Trim();
            string[] splittedFile = { "," };
            string[] strSplittedSupportedTypes 
                = strypes.Split(splittedFile, StringSplitOptions.None);
            string[] splittedFilePath = { "." };
            string[] strSplittedTypes 
                = strfilePath.Split(splittedFilePath, StringSplitOptions.None);
            int arraylen = strSplittedTypes.Length - 1;
            string strExtension = strSplittedTypes[arraylen].ToString();

            for (int j = 0; j < strSplittedSupportedTypes.Length; j++)
            {
                if (strSplittedSupportedTypes[j].ToUpper().ToString() 
                    == strExtension.ToUpper().ToString())
                {
                    imageStatus = true;
                }
            }

            return imageStatus;
        }
    }
}
