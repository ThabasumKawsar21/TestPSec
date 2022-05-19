
namespace VMSDev
{
    using CAS.Security.Application;
    using ECMSharedServices;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Data.Sql;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// id card Service class
    /// </summary>       
    public partial class IDCard : System.Web.UI.Page
    {
       /// <summary>
       /// The id card Service field
       /// </summary>       
       private IDCardService.IDcardAppClient idcardService = new IDCardService.IDcardAppClient();
        
        /// <summary>
        /// To remove session when user close browse button
        /// </summary>
        [System.Web.Services.WebMethod]
        public static void RemoveSessions()
        {
            ////HttpContext.Current.Session.Abandon();

            string strFileName = string.Empty;
            string strFullName = string.Empty;
            //// Delete image if it is avaliable in ReSize Folder
            if (HttpContext.Current.Session["hdnAssociateId"] != null)
            {
                strFileName = string.Concat("Photo_", HttpContext.Current.Session["hdnAssociateId"].ToString(), ".jpeg");
                strFullName = HttpContext.Current.Server.MapPath(@"FilesServer/Cropped/" + strFileName);
                HttpContext.Current.Session["AssociateId"] = null;
                if (CheckFileExist(strFullName.Replace("\\", "/")))
                {
                    RemoveFile(strFullName.Replace("\\", "/"));
                }
            }
        }

        /// <summary>
        /// Method to call ECM Download Service for getting Chire  image
        /// </summary>
        /// <param name="associateId">associate id</param>
        /// <returns>the image of the associate</returns>
        [WebMethod]
        public static string GetChireImageFromECM(string contentId)
        {
            int appId = Convert.ToInt32(ConfigurationManager.AppSettings["ECMAPPID"]);
            byte[] fileContent = null;
            string imageUrl = "";
            //string contentId = hdnFileUploadID.Value;
            string ecmContentId = EncodeHelper.HtmlEncode(contentId);


            if (!string.IsNullOrEmpty(ecmContentId))
            {
                WrapperUICheckIn checkInObj = new WrapperUICheckIn(appId);
                ECMCommon.IdcFile fileVal = new ECMCommon.IdcFile();
                fileVal = checkInObj.DownloadFileContent(ecmContentId, appId);
                fileContent = fileVal.Filecontent;

                imageUrl = JsonConvert.SerializeObject("data:image/png;base64," + Convert.ToBase64String(fileContent));

            }

            return imageUrl;
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection coll = Request.QueryString;
       
            // Get names of all keys into a string array.
            string[] arr1 = coll.AllKeys;
            string key = string.Empty;
            string tempId = string.Empty;
            string nameChosen = string.Empty;
            string filecontentId = string.Empty;
            if (arr1.Length == 3)
            {
                key = Convert.ToString(coll.GetValues(0)[0]);
                tempId = Convert.ToString(coll.GetValues(1)[0]);
                nameChosen = Convert.ToString(coll.GetValues(2)[0]);
                filecontentId = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(tempId));
                this.hdnFileUploadID.Value = filecontentId;
                nameChosen = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(nameChosen));
            }
            else
            {
                key = Convert.ToString(coll.GetValues(0)[0]);
            }

            string associateID = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(key));
            string loginid = string.Empty;
            try
            {
                if (!Page.IsPostBack)
                {
                    if (this.Session["LoginId"] != null)
                    {
                        string strLoginID = Convert.ToString(Session["LoginId"]);
                        ////#DPNAME
                        string strDisplayName = string.Empty;

                        VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL associatedetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                        VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL objUserDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
                        DataTable dtdisplay = objUserDetailsBL.InsertIDCardDetails(strLoginID, associateID);
                        DataTable dt = associatedetails.PreviewDetails(associateID);
                        if (!string.IsNullOrEmpty(associateID))
                        {
                            ////this.imgPrint.Disabled = false;
                            ////this.print.Enabled = true;
                            ////this.prints.Disabled = false;
                            if (dtdisplay.Rows.Count > 0)
                            {
                               // string pattern = "[^0-9]";
                                filecontentId = dt.Rows[0]["FileUploadID"].ToString().Trim();
                                //if (this.Session["ImageURL"] != null)
                                //{
                                //    this.ImgAssociate.ImageUrl = Session["ImageURL"].ToString();
                                //}
                                //else
                                //{
                                    //GenericFileUpload gnfileupload = new GenericFileUpload();
                                    //byte[] data = gnfileupload.GetAssociateImage(filecontentId);
                                    //int width = 0;
                                    //int height = 0;
                                    //System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(data));
                                    //width = image.Width;
                                    //height = image.Height;
                                    //string encryptedData = VMSBusinessLayer.VMSBusinessLayer.Encrypt(XSS.HtmlEncode(associateID));
                                    //this.ImgAssociate.ImageUrl = XSS.HtmlEncode(string.Concat("GetImage1.ashx?IDCARD=", encryptedData, "&w=", width, "&h=", height, "&FileId=", filecontentId));
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "GetImageFromIDCard", "GetImageFromIDCard();", true);
                                //}

                                strDisplayName = dt.Rows[0]["DisplayName"].ToString().ToUpper();

                                this.lblFrontID.InnerHtml = XSS.HtmlEncode(dt.Rows[0]["AssociateID"].ToString().Trim());
                                this.lblRearID.InnerHtml = XSS.HtmlEncode(dt.Rows[0]["AssociateID"].ToString().Trim());
                                this.hdnAssociateId.Value = XSS.HtmlEncode(dt.Rows[0]["AssociateID"].ToString().Trim());
                                this.Session["hdnAssociateId"] = this.hdnAssociateId.Value;
                                ////this.lblyear.InnerHtml = strYear.Substring(2, 2);
                                this.imgBarCode.ImageUrl = string.Concat("~/Barcode.aspx?Code=*", XSS.HtmlEncode(associateID.Trim()), "*");
                                ////this.lblBarcodeAssociateId.InnerHtml = XSS.HtmlEncode(dt.Rows[0]["AssociateID"].ToString().Trim());
                                //// commented existing and added new logic by Krishna(449138)
                                ////Added by Abi 311857 for philippines alone to display sticker 23/09/2015
                                ////if (country == "Philippines")
                                ////{
                                ////    this.img_phil.Visible = true;
                                ////}
                                ////////Added and commented for ID card bottom sticker for each reagional Abi 311857
                                ////if (city == "Chennai")
                                ////{
                                ////    this.img_CHN.Visible = true;
                                ////}

                                this.lblArea.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["Area"].ToString().Trim());
                                this.lblPin.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["PinCode"].ToString().Trim());
                                this.lblCountry.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["Country"].ToString().Trim());
                                this.lblOfficeAddress.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["OfficeAddress"].ToString().Trim());
                                this.lblCompanyName.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["CompanyName"].ToString().Trim());
                                this.lblTele.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["TelephoneNo"].ToString().Trim());
                                this.lblFax.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["FaxNo"].ToString().Trim());
                                this.lblCity.InnerHtml = XSS.HtmlEncode(dtdisplay.Rows[0]["City"].ToString().Trim());

                                if (!string.IsNullOrEmpty(strDisplayName))
                                {
                                    ////if (strDisplayName.Length > 18) 
                                    ////{
                                    ////    this.lblName.Attributes.Add("style", "font-size:13pt");
                                    ////}

                                    this.lblName.InnerHtml = XSS.HtmlEncode(strDisplayName.ToUpper());
                                }
                                else if (!string.IsNullOrEmpty(nameChosen)) 
                                {
                                    ////if (nameChosen.Length > 18)
                                    ////{
                                    ////    this.lblName.Attributes.Add("style", "font-size:13pt");
                                    ////}

                                    this.lblName.InnerHtml = XSS.HtmlEncode(nameChosen.ToUpper());
                                 }
                               
                                ////Making card common as same as Chennai location
                                ////this.imgIDCardTop.Src = "~/Images/" + city + "/CardTop.jpg";
                                ////this.imgIdCardBottom.Src = "~/Images/" + city + "/IsAssociate.jpg";
                                ////this.imgFrontLine.Src = "~/Images/" + city + "/Line.jpg";

                                this.Session["ImageURL"] = null;
                                this.Session["DisplayName"] = null;

                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'> alert('Data Not Found');</script>");
                            }
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
        /// Redirect to parent page.
        /// </summary>
        /// <param name="sender">parameter sender</param>
        /// <param name="e">event e</param>
        protected void Btnhidden_Click(object sender, EventArgs e)
        {
            this.RemoveResizeImage();
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "Reload", "ReloadParent()", true);
        }

        /// <summary>
        /// The CheckFileExist method
        /// </summary>
        /// <param name="path">The path parameter</param>
        /// <returns>The boolean type object</returns>        
        private static bool CheckFileExist(string path)
        {
            FileInfo imageFile = new FileInfo(path);
            bool fileExists = imageFile.Exists;
            return fileExists;
        }

        /// <summary>
        /// The RemoveFile method
        /// </summary>
        /// <param name="path">The path parameter</param>        
        private static void RemoveFile(string path)
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
        /// Check whether File is already exist
        /// </summary>
        /// <param name="path">path of file</param>
        /// <returns>file Exists</returns>
        private bool FileExist(string path)
        {
            try
            {
                FileInfo imageFile = new FileInfo(path);
                bool fileExists = imageFile.Exists;
                return fileExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Delete file
        /// </summary>
        /// <param name="path">file path</param>
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
        /// To Remove Cropped image
        /// </summary>
        private void RemoveResizeImage()
        {
            string strFullName = string.Empty;
            this.Session["hdnAssociateId"] = null;

            // Delete image if it is avaliable in ReSize Folder
            string strFileName = string.Concat("Photo_", this.hdnAssociateId.Value, ".jpeg");
            if (this.Session["FullPath"] != null)
            {
                strFullName = Session["FullPath"].ToString();
                if (this.FileExist(strFullName))
                {
                    this.DeleteFile(strFullName);
                }
            }
        }
    }
}
