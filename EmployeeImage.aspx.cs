
namespace VMSDev
{
    using CAS.Security.Application;
    using ECMCommon;
    using ECMSharedServices;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSDev.DocUploadServiceRef;
    using System.Xml.Linq;



    /// <summary>
    /// Employee image
    /// </summary>
    public partial class EmployeeImage : System.Web.UI.Page
    {
        /// <summary>
        /// The Image field
        /// </summary>        
        private string strImage = string.Empty;
        private WrapperCheckIn objCheckInServices;

        /// <summary>
        /// Used to call Search service Library methods
        /// </summary>
        ////private WrapperSearch objSearchServices;

        /// <summary>
        /// Used to call Additional service Library methods
        /// </summary>
        ////private WrapperAdditional objCheckInAdditionalServices;
        
        /// <summary>
        /// Used to assign application id
        /// </summary>
        private int appId = Convert.ToInt32(ConfigurationManager.AppSettings["ECMAPPID"]);

        /// <summary>
        /// Used to assign file upload page
        /// </summary>
        private string fileUploadUI = Convert.ToString(ConfigurationManager.AppSettings["UploadUI"]);

        /// <summary>
        /// Used to final query string
        /// </summary>
        private string finalQueryString = string.Empty;

        /// <summary>
        /// Used to assign client URL
        /// </summary>
        private string clientURL = Convert.ToString(ConfigurationManager.AppSettings["ECMOneCQueryUrl"]);

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////Vms_Host_upload PHYS19042010CR02
                Response.Expires = 0;
                Response.Cache.SetNoStore();
                Response.AppendHeader("Pragma", "no-cache");
                ////end PHYS19042010CR02
                VMSBusinessLayer.VMSBusinessLayer.MasterDataBL objMasterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
                //// Load NameValueCollection object.
                NameValueCollection coll = Request.QueryString;
                //// Get names of all keys into a string array.
                string[] arr1 = coll.AllKeys;
                switch (arr1[0].ToString())
                {
                    case "strImage":
                        {
                            this.strImage = Request.QueryString["strImage"];
                            if (!Page.IsPostBack)
                            {
                                if (this.strImage.Equals("Photo"))
                                {
                                    if (this.Session["Webcamimage"] != null)
                                    {
                                        Response.ContentType = "image/jpeg";
                                        Response.BinaryWrite((byte[])Session["Webcamimage"]);
                                    }
                                    else if (this.Session["VisitorImgByte"] != null)
                                    {
                                        Response.ContentType = "image/jpeg";
                                        Response.BinaryWrite((byte[])Session["VisitorImgByte"]);
                                    }
                                }
                                else if (this.strImage.Equals("Proof"))
                                {
                                    if (this.Session["ProofImgByte"] != null)
                                    {
                                        Response.ContentType = "image/jpeg";
                                        Response.BinaryWrite(Encoding.Default.GetBytes((string)Session["ProofImgByte"]));
                                    }
                                }
                            }

                            break;
                        }

                    case "key":
                        {
                            string strVisitorID = VMSBusinessLayer.VMSBusinessLayer.Decrypt(coll.GetValues(0)[0].ToString());
                            byte[] data = objMasterDataBL.GetVisitorImage(strVisitorID); ////get image from DB                        
                            if (data == null) 
                            {
                                VMSDataLayer.VMSDataLayer.MasterDataDL vmsMasterDataDL = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                                VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                                propertiesDC = vmsMasterDataDL.GetSearchDetails(Convert.ToInt32(strVisitorID));
                                //commented for 
                                //data = this.GetImageFromSAN(propertiesDC.VisitorProofProperty.FileContentId);
                                data = this.DownloadECM(propertiesDC.VisitorProofProperty.FileContentId);
                                //data = DownloadECM(propertiesDC.VisitorProofProperty.FileContentId);
                            }

                            if (data != null)
                            {
                                using (MemoryStream ms = new MemoryStream(data))
                                {
                                    Bitmap imgIn = (Bitmap)System.Drawing.Image.FromStream(ms);
                                    double y = imgIn.Height;
                                    double x = imgIn.Width;
                                    double factor = 1;
                                    int width = 200, height = 150;
                                    if (width > 0)
                                    {
                                        factor = width / x;
                                    }
                                    else if (height > 0)
                                    {
                                        factor = height / y;
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
                                
                                    string Targetpage = "Images/DummyPhoto.png";
                                    Response.Redirect(Targetpage, true);                                                                                                 
                            }

                            break;
                        }
                    default:
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                try
                {
                    string TargetPage = "Images/DummyPhoto.png";
                    Response.Redirect(TargetPage, true);                   
                }
                catch (System.Threading.ThreadAbortException exp)
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
                FileUploadDetailsRequest objFileUploadDetailsRequest = new FileUploadDetailsRequest();
                using (DocumentUploadServiceClient objDocumentUploadServiceClient = new DocumentUploadServiceClient())
                {
                    // objFileUploadDC.FileUploadId = fileuploadId;
                    objFileUploadDC.FileContentId = new Guid(fileContentID);
                    objFileUploadDC.AppTemplateId = ConfigurationManager.AppSettings["VMSappTemplateId"];
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

        /// <summary>
        /// Function to download Image from ECM
        /// </summary>
        /// <param name="contentId">file content Id</param>
        private byte[]  DownloadECM(string contentId)
        {
            //documentId="1155369"
            //string ecmContentId = EncodeHelper.HtmlEncode("SIT_1153605");
            string ecmContentId = EncodeHelper.HtmlEncode(contentId);
            
            this.objCheckInServices = new WrapperCheckIn(this.appId);
            try
            {
                if (!string.IsNullOrEmpty(ecmContentId))
                {
                    WrapperUICheckIn objECMCheckInScrapSaleEmail = new WrapperUICheckIn(this.appId);
                    byte[] fileContent = null;
                    ECMCommon.IdcFile fileVal = new ECMCommon.IdcFile();
                    fileVal = objECMCheckInScrapSaleEmail.DownloadFileContent(ecmContentId,this.appId);
                    fileContent = fileVal.Filecontent;
                    return fileContent;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
