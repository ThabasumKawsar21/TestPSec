

namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;   
    using System.Data;    
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;   
    using VMSBusinessLayer;
    using VMSDev.DocUploadServiceRef;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class AssociateImage
    /// </summary>
    public partial class AssociateImage : System.Web.UI.Page
    {
        /// <summary>
        /// ImageToByte Method
        /// </summary>
        /// <param name="img">image object</param>
        /// <returns>converted image</returns>
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        /// Method to decrypt Binary Data
        /// </summary>
        /// <param name="strEncrpytedDataImg">Encrypted Data Image object</param>
        /// <returns>returns decrypted data</returns>
        public string DecryptBinaryData(string strEncrpytedDataImg)
        {
            return new EncryptDecrypt().Decrypt(strEncrpytedDataImg, "CTS", true);
        }

        /// <summary>
        /// Initial page load method
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            EmployeeBL objEmployeeBL = new EmployeeBL();
            try
            {
                //// Load NameValueCollection object.
                NameValueCollection coll = Request.QueryString;
                //// Get names of all keys into a string array.
                string[] arr1 = coll.AllKeys;

                int width = 0;
                int height = 0;
                string fileContentId = string.Empty;

                if (arr1.Length == 3)
                {
                    width = Convert.ToInt16(coll.GetValues(1)[0].ToString());
                    height = Convert.ToInt16(coll.GetValues(2)[0].ToString());
                }
                else if (arr1.Length == 4)
                {
                    width = Convert.ToInt16(coll.GetValues(1)[0].ToString());
                    height = Convert.ToInt16(coll.GetValues(2)[0].ToString());
                    fileContentId = coll.GetValues(3)[0].ToString();
                }

                switch (arr1[0].ToString())
                {
                    case "ID":
                        {
                            string strAssociateID = VMSBusinessLayer.Decrypt(coll.GetValues(0)[0].ToString());
                            string fileContentID = objEmployeeBL.GetEmployeeImageDetails(strAssociateID);
                            if (!string.IsNullOrEmpty(fileContentID))
                            {
                                GenericFileUpload genericFileUpload = new GenericFileUpload();
                                byte[] data = genericFileUpload.GetAssociateImage(fileContentID);
                                this.Drawimage(data, width, height);
                            }
                        }

                        break;

                    case "Key":
                        {
                            string strApplicantID = VMSBusinessLayer.Decrypt(coll.GetValues(0)[0].ToString());

                            var result = objEmployeeBL.GetUploadedImageDetails(strApplicantID).Split('|');

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

                            this.Drawimage(data, width, height);
                        }

                        break;

                    case "IDCard":
                        {
                            if (!string.IsNullOrEmpty(fileContentId))
                            {
                                GenericFileUpload genericFileUpload = new GenericFileUpload();
                                byte[] data = genericFileUpload.GetAssociateImage(fileContentId);
                                this.Drawimage(data, width, height);
                            }
                        }

                        break;

                    case "Path":
                        {
                            System.Drawing.Image orgImage = System.Drawing.Image.FromFile(coll.GetValues(0)[0].ToString());
                            byte[] convertedImage = ImageToByte(orgImage);
                            this.Drawimage(convertedImage, width, height);
                            orgImage.Dispose();
                        }

                        break;
                    default:
                         
                            string Targetpage = "~\\Images\\DummyPhoto.png";
                            Response.Redirect(Targetpage, true);                                                                           
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
            catch
            {
                try
                {
                    string targetpage = "~\\Images\\DummyPhoto.png";
                    Response.Redirect(targetpage, true);
                   
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }

        /// <summary>
        /// drawImage method
        /// </summary>
        /// <param name="data">data object</param>
        /// <param name="width">width object</param>
        /// <param name="height">height object</param>
        private void Drawimage(byte[] data, int width, int height)
        {
            try
            {
                if (data != null)
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        Bitmap imgIn = (Bitmap)System.Drawing.Image.FromStream(ms);
                        double y = imgIn.Height;
                        double x = imgIn.Width;
                        double factor = 1;

                        if (width * height != 0)
                        {
                            if (width > 0)
                            {
                                factor = width / x;
                            }
                            else if (height > 0)
                            {
                                factor = height / y;
                            }
                        }

                        Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
                        Graphics g = Graphics.FromImage(imgOut);
                        g.Clear(Color.White);
                        g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);
                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        imgOut.Save(outStream, ImageFormat.Jpeg);
                        byte[] buffer = outStream.ToArray();
                        Response.ContentType = "image/jpeg";
                        Response.BinaryWrite((byte[])buffer);
                    }
                }
                else
                {
                    
                        string targetpage = "~\\Images\\DummyPhoto.png";
                        Response.Redirect(targetpage, true);                                                              
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
            catch
            {
                try
                {
                    string targetPage = "~\\Images\\DummyPhoto.png";
                    Response.Redirect(targetPage, true);
                     
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
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
                MFileuploadResponse objMFileuploadResponse = new MFileuploadResponse();
                FileUploadDetailsRequest objFileUploadDetailsRequests = new FileUploadDetailsRequest();
                using (DocumentUploadServiceClient objDocumentUploadServiceClient = new DocumentUploadServiceClient())
                {
                    // objFileUploadDC.FileUploadId = fileuploadId;
                    objFileUploadDC.FileContentId = new Guid(fileContentID);
                    objFileUploadDC.AppTemplateId = ConfigurationManager.AppSettings["IDcardAppTemplateId"];
                    objMFileuploadResponse = objDocumentUploadServiceClient.DownloadFile(objFileUploadDC);
                    associateImage = objMFileuploadResponse.OutgoingFile;

                    if (associateImage != null)
                    {
                        return associateImage;
                    }
                    else
                    {
                        string strException = "Associate Image return as Null for FileContentId" + fileContentID;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
