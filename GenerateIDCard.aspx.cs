
namespace VMSDev
{
    using CAS.Security.Application;
    using ECMSharedServices;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Data.Sql;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class Generate Id Card
    /// </summary>
    public partial class GenerateIDCard : System.Web.UI.Page
    {
        /// <summary>
        /// Id card service.
        /// </summary>
        private IDCardService.IDcardAppClient idcardService = new IDCardService.IDcardAppClient();

        /// <summary>
        /// Request details
        /// </summary>
        private VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL = new VMSBusinessLayer.RequestDetailsBL();

        /// <summary>
        /// Location detail
        /// </summary>
        private VMSBusinessLayer.UserDetailsBL location = new VMSBusinessLayer.UserDetailsBL();

        /// <summary>
        /// Location detail
        /// </summary>
        private string flag = string.Empty;

        /// <summary>
        /// first count
        /// </summary>
        private int firstCount = 0;

        /// <summary>
        /// Middle count
        /// </summary>
        private int midCount = 0;

        /// <summary>
        /// Last count
        /// </summary>
        private int lastCount = 0;

        /// <summary>        
        /// To replace special character
        /// </summary>
        /// <param name="str">receiving string</param>
        /// <returns>returning string</returns>
        public static string RegExValidate(string str)
        {
            string pattern = @"[;:>,<*]";

            return Regex.Replace(str, pattern, string.Empty);
        }

        /// <summary>
        /// Get Associate Image
        /// </summary>
        /// <returns>Image value</returns>        
        public System.Drawing.Image GetAssociateImage()
        {
            GenericFileUpload gnfileUpload = new GenericFileUpload();
            byte[] data = gnfileUpload.GetAssociateImage(RegExValidate(this.hdnFileUploadId.Value));
            return this.Writeimage(data, 200, 150);
        }

        /// <summary>
        /// Image Print Status
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param>  
        public void ImgPrintStatus_Click(object sender, ImageClickEventArgs e)
        {
            if (this.Session["LoginId"] != null)
            {
                EmployeeBL objEmployeeBL = new EmployeeBL();
                string id = this.txtID.Text.Trim().ToString();
                DataTable dtdisplay = this.objRequestDetailsBL.PreviewDetails(id);
                if (dtdisplay == null)
                {
                    this.lblMessage.Visible = true;
                    this.lblMessage.Text = "Enter a Valid Associate ID";
                }
                else
                {
                    string printStatus = VMSConstants.VMSConstants.PRINTSTATUS;

                    // Int32 location = Convert.ToInt32(drpCity.SelectedItem.Value);
                    this.objRequestDetailsBL.UpdatePrintStatus(this.txtID.Text.Trim(), Convert.ToString(this.Session["LoginId"]), printStatus, Convert.ToInt32(this.hdnLocation.Value));
                }
            }
        }

        /// <summary>
        /// Image download
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param> 
        public void ImgDownload_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtID.Text))
            {
                VMSBusinessLayer.RequestDetailsBL objtRequestDetailsBL = new VMSBusinessLayer.RequestDetailsBL();

                DataTable dtdisplay = objtRequestDetailsBL.PreviewDetails(Convert.ToString(this.txtID.Text).Trim());
                if (dtdisplay == null)
                {
                    this.lblMessage.Visible = true;
                    this.lblMessage.Text = "Enter a Valid Associate ID";
                }
                else
                {
                    this.lblMessage.Text = string.Empty;
                    GenericFileUpload gnfileUpload = new GenericFileUpload();
                    byte[] data = gnfileUpload.GetAssociateImage(RegExValidate(this.hdnFileUploadId.Value));
                    this.DownloadImage(data, XSS.HtmlEncode(this.txtID.Text));

                    // Save Log information
                    if (this.Session["LoginId"] != null)
                    {
                        objtRequestDetailsBL.SaveLogDownLoadPhoto(this.txtID.Text, Convert.ToString(this.Session["LoginId"]));
                    }                    
                }
            }
        }

        /// <summary>
        /// to generate first name based list
        /// </summary>
        /// <param name="strfirstName">first name</param>
        /// <param name="middleName">middle name</param>
        /// <param name="strlastName">last name</param>
        /// <returns>list of name</returns>
        public IList<string> GenerateNameSuggestionBasedFirstName(string strfirstName, string middleName, string strlastName)
        {
            string currentName = string.Empty;
            string firstMiddle = string.Empty;
            string firstlast = string.Empty;
            IList<string> firstNameList = new List<string>();
            if (strfirstName.Length <= 20)
            {
                firstNameList.Add(strfirstName);
            }

            currentName = strfirstName + " " + strlastName;
            if (currentName.Length <= 20)
            {
                if (!firstNameList.Contains(currentName))
                {
                    firstNameList.Add(currentName);
                }
            }

            currentName = strfirstName;
            if (!string.IsNullOrEmpty(middleName))
            {
                firstMiddle = middleName.Substring(0, 1);
                currentName = currentName + " " + firstMiddle;
            }

            if (!string.IsNullOrEmpty(strlastName))
            {
                firstlast = strlastName.Substring(0, 1);
                currentName = currentName + " " + firstlast;
            }

            if (currentName.Length <= 20)
            {
                if (!firstNameList.Contains(currentName))
                {
                    firstNameList.Add(currentName);
                }
            }

            return firstNameList;
        }

        /// <summary>
        /// to generate middle name based list
        /// </summary>
        /// <param name="strfirstName">first name</param>
        /// <param name="middleName">middle name</param>
        /// <param name="strlastName">last name</param>
        /// <returns>list of name</returns>
        public IList<string> GenerateNameSuggestionBasedMiddleName(string strfirstName, string middleName, string strlastName)
        {
            string currentName = string.Empty;
            string firstMain = string.Empty;
            string firstlast = string.Empty;
            IList<string> middleNameList = new List<string>();
            if (!string.IsNullOrEmpty(strfirstName))
            {
                firstMain = strfirstName.Substring(0, 1);
            }

            if (!string.IsNullOrEmpty(strlastName))
            {
                firstlast = strlastName.Substring(0, 1);
            }

            string[] middleNameArray = middleName.Split(' ');

            currentName = firstMain + " " + middleNameArray[0] + " " + strlastName;
            currentName = currentName.Trim();
            if (!middleNameList.Contains(currentName))
            {
                if (currentName.Length <= 20)
                {
                    middleNameList.Add(currentName);
                }
            }

            currentName = firstMain + " " + middleName + " " + firstlast;
            currentName = currentName.Trim();
            if (!middleNameList.Contains(currentName))
            {
                if (currentName.Length <= 20)
                {
                    middleNameList.Add(currentName);
                }
            }

            currentName = strfirstName + " " + middleNameArray[0] + " " + firstlast;
            currentName = currentName.Trim();
            if (!middleNameList.Contains(currentName))
            {
                if (currentName.Length <= 20)
                {
                    middleNameList.Add(currentName);
                }
            }

            return middleNameList;
        }

        /// <summary>
        /// to generate last name based list
        /// </summary>
        /// <param name="strfirstName">first name</param>
        /// <param name="middleName">middle name</param>
        /// <param name="strlastName">last name</param>
        /// <returns>list of name</returns>
        public IList<string> GenerateNameSuggestionBasedLastName(string strfirstName, string middleName, string strlastName)
        {
            string currentName = string.Empty;
            string firstMain = string.Empty;
            string firstMiddle = string.Empty;
            IList<string> lastNameList = new List<string>();
            if (!string.IsNullOrEmpty(strfirstName))
            {
                firstMain = strfirstName.Substring(0, 1);
            }

            if (!string.IsNullOrEmpty(middleName))
            {
                firstMiddle = middleName.Substring(0, 1);
            }

            currentName = firstMain + " " + firstMiddle + " " + strlastName;
            currentName = currentName.Trim();
            if (!lastNameList.Contains(currentName))
            {
                if (currentName.Length <= 20)
                {
                    lastNameList.Add(currentName);
                }
            }

            if (!string.IsNullOrEmpty(firstMiddle))
            {
                currentName = strfirstName + " " + firstMiddle + " " + strlastName;
                currentName = currentName.Trim();
                if (!lastNameList.Contains(currentName))
                {
                    if (currentName.Length <= 20)
                    {
                        lastNameList.Add(currentName);
                    }
                }
            }

            currentName = strlastName;
            currentName = currentName.Trim();
            if (!lastNameList.Contains(currentName))
            {
                if (currentName.Length <= 20)
                {
                    lastNameList.Add(currentName);
                }
            }

            return lastNameList;
        }
        
        /// <summary>
        /// Image Edit
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param> 
        public void ImgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int strWidth = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailW"].ToString());
                int strHeight = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailH"].ToString());
                GenericFileUpload gnfileUpload = new GenericFileUpload();
                byte[] data = gnfileUpload.GetAssociateImage(RegExValidate(this.hdnFileUploadId.Value));
                int imgwidth = 0;
                int imgheight = 0;
                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(data));
                imgwidth = image.Width;
                imgheight = image.Height;
                if (imgwidth == 0)
                {
                    imgwidth = strWidth;
                }

                if (imgheight == 0)
                {
                    imgheight = strHeight;
                }

                this.testImage.Src = string.Concat("GetImage1.ashx?IDCARD=", this.ID, "&w=", imgwidth, "&h=", imgheight, "&TempId=", this.hdnFileUploadId.Value);
                this.testImage.Width = Convert.ToInt32(imgwidth);
                this.testImage.Height = Convert.ToInt32(imgheight);
                this.pnlCropImage.Attributes.Add("style", "display:block");
                this.mdlCropImage.Show();
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "ShowCrop", "PreviewImage();", true);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// call the ID card page to print card
        /// </summary>
        /// <param name="nameChosen">chosen name</param>
        /// <param name="strLocation">location code</param>
        public void CallIdCardPage(string nameChosen, string strLocation)
        {
            this.imgAssociateImage.ImageUrl = this.hdnAssociateImage.Value;

            if (this.Session["ImageURL"] != null)
            {
                ////string encryptedData = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(XSS.HtmlEncode(this.txtID.Text.Trim()))));
                ////string str1Script = "<script language='javascript'>window.open('IDCard.aspx?key=" + encryptedData + "', 'List', 'toolbar=no,menubar=no,scrollbars=no,resizable=no,width=207,height=700, location=center', false);</script>";
                ////ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", str1Script);
            }

            else
            {
                string strScript = string.Empty;
                string templateId = XSS.HtmlEncode(this.hdnFileUploadId.Value);
                strScript = "<script language='javascript'>window.open('IDCard.aspx?key=";
                string encryptedData = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(XSS.HtmlEncode(this.txtID.Text.Trim()))));
                strScript = string.Concat(strScript, encryptedData);
                string encryptTempId = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(templateId)));
                strScript = string.Concat(strScript, " & Id=");
                strScript = string.Concat(strScript, encryptTempId);
                string encryptedUserName = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(nameChosen)));
                strScript = string.Concat(strScript, " & Name=");
                strScript = string.Concat(strScript, encryptedUserName);
                strScript = string.Concat(strScript, "', 'List', 'toolbar=no,menubar=no,scrollbars=no,resizable=no,width=207,height=700, location=center');</script>");
                ClientScript.RegisterStartupScript(typeof(Page), "LoadGenerateIDCard", strScript);

                if (this.Session["LoginId"] != null)
                {
                    string printstatus = VMSConstants.VMSConstants.PRINTSTATUS;
                    this.objRequestDetailsBL.UpdatePrintStatus(this.txtID.Text.Trim(), Convert.ToString(this.Session["LoginId"]), printstatus, Convert.ToInt32(strLocation));
                }
            }
        }

        /// <summary>
        /// Method to call ECM Download Service for getting Chire image
        /// </summary>
        /// <param name="associateId">associate id</param>
        /// <returns>the image of the associate</returns>
        [WebMethod]
        public static string GetChireImageFromECMForIDCard(string contentId)
        {
            int appId = Convert.ToInt32(ConfigurationManager.AppSettings["ECMAPPID"]);
            byte[] fileContent = null;
            string imageUrl = "";
            //string contentId = hdnFileUploadID.Value;
            string ecmContentId = EncodeHelper.HtmlEncode(contentId);
            //int appId = 1132;
            if (!string.IsNullOrEmpty(ecmContentId))
            {
                WrapperUICheckIn checkInObj = new WrapperUICheckIn(appId);
                //ECMFileURLResult url = new ECMFileURLResult();       
                ECMCommon.IdcFile fileVal = new ECMCommon.IdcFile();
                fileVal = checkInObj.DownloadFileContent(ecmContentId, appId);

                fileContent = fileVal.Filecontent;
                // url = objCheckInServices.GetNativeFileURL(ecmContentId, 0, appId, true, false, );

                imageUrl = JsonConvert.SerializeObject("data:image/png;base64," + Convert.ToBase64String(fileContent));
            }

            return imageUrl;
        }

        /// <summary>
        /// button ok click to save the name chosen
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event e</param>
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.hdnNameSelect.Value))
                {
                    var nameSelect = XSS.HtmlEncode(this.hdnNameSelect.Value.Replace('-', ' '));
                    VMSBusinessLayer.RequestDetailsBL objStoreChosenName = new VMSBusinessLayer.RequestDetailsBL();
                    bool result = objStoreChosenName.StoreChosenName(this.hdnAssocId.Value, nameSelect);
                    if (result)
                    {
                        this.lblMessage.Visible = true;
                        this.lblMessage.Text = "Chosen Name Saved Successfully.";
                        this.CallIdCardPage(nameSelect, this.hdnLocation.Value);
                    }
                    else
                    {
                        this.lblMessage.Visible = true;
                        this.lblMessage.Text = "Chosen Name Not Saved.";
                    }
                }
                else
                {
                    Label lblErrorMsg = new Label();
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please choose a name.";
                    this.pnlNineNameSuggestion.Controls.Add(lblErrorMsg);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Image save Status
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param> 
        protected void ImgSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string loginID = Session["LoginID"].ToString().Trim();
                string id = XSS.HtmlEncode(this.txtID.Text.Replace(":", string.Empty).Trim());
                if (this.Session["FullPath"] != null)
                {
                    if (this.FileExist(Session["FullPath"].ToString()))
                    {
                        this.DeleteFile(Session["FullPath"].ToString());
                    }
                }

                this.Session["FileName"] = string.Concat(id, "_", DateTime.Now.ToString("ddMMyyyy_HH_mm_ss"), ".jpeg");
                string strFileName = Session["FileName"].ToString();
                string strFullName = Server.MapPath(@"FilesServer/" + strFileName);
                this.Session["FullPath"] = strFullName;

                if (!string.IsNullOrEmpty(this.hdnFileUploadId.Value))
                {
                    GenericFileUpload gnfileUpload = new GenericFileUpload();
                    byte[] data = gnfileUpload.GetAssociateImage(RegExValidate(this.hdnFileUploadId.Value));
                    List<string> stringtoList = new List<string>();
                    string strMessage = "Image Reading from SAN Storage:" + data;
                    VMSUtility.VMSUtility.WriteLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);
                    
                    // Start Point of Selected Portion
                    int ix1 = 0;
                    int iy1 = 0;
                    int w = 0;
                    int h = 0;
                    if (!string.IsNullOrEmpty(this.x1.Value))
                    {
                        // Start Point of Selected Portion
                        ix1 = Convert.ToInt32(this.x1.Value);
                    }

                    if (!string.IsNullOrEmpty(this.y1.Value))
                    {
                        iy1 = Convert.ToInt32(this.y1.Value);
                    }

                    if (string.IsNullOrEmpty(this.width.Value))
                    {
                        w = Convert.ToInt32(this.testImage.Width);
                    }
                    else
                    {
                        w = Convert.ToInt32(this.width.Value);
                    }

                    if (string.IsNullOrEmpty(this.height.Value))
                    {
                        h = Convert.ToInt32(this.testImage.Height);
                    }
                    else
                    {
                        h = Convert.ToInt32(this.height.Value);
                    }

                    // Dimension of Selected Portion
                    // Dimension of the Selected Image
                    int w2 = this.testImage.Width;
                    int h2 = this.testImage.Height;

                    // byte[] imageData = client.DownloadData(Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl(testImage.Src));
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        System.Drawing.Image imgOrig = new Bitmap(ms);
                        
                        // Dimension of the Original Image
                        int w1 = imgOrig.Width;
                        int h1 = imgOrig.Height;
                        int iX1 = ix1 * w1 / w2;
                        int iY1 = iy1 * h1 / h2;
                        int ww = w * w1 / w2;
                        int hh = h * h1 / h2;
                        if (ww <= 0)
                        {
                            ww = w1;
                        }

                        if (hh <= 0)
                        {
                            hh = h1;
                        }

                        Bitmap b = new Bitmap(ww, hh);
                        Rectangle sourceRect = new Rectangle(iX1, iY1, ww, hh);
                        System.Drawing.Image imgNew = CropImage(imgOrig, sourceRect);
                        if (this.Session["FileName"] != null)
                        {
                            strFileName = Session["FileName"].ToString();
                        }
                        else
                        {
                            strFileName = string.Concat(id, "_", DateTime.Now.ToString("ddMMyyyy_HH_mm_ss"), ".jpeg");
                        }

                        strFullName = Server.MapPath(@"FilesServer/" + strFileName);
                        this.Session["FullPath"] = strFullName;
                        strMessage = "Image Save File Path:" + strFullName;
                        VMSUtility.VMSUtility.WriteLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);
                        imgNew.Save(strFullName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imgNew.Dispose();
                        strMessage = "Image Saved to Server Path :" + strFullName;
                        VMSUtility.VMSUtility.WriteLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);
                        int strWidth;
                        int strHeight;
                        strWidth = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailW"].ToString());
                        strHeight = Convert.ToInt32(ConfigurationManager.AppSettings["ThumbnailH"].ToString());

                        // ssociateImage.Src = "FilesServer/Cropped/" + strFileName;
                        this.imgAssociateImage.ImageUrl = string.Concat("GetImage1.ashx?PATH=", strFullName, "&w=", strWidth, "&h=", strHeight);
                        this.Session["ImageURL"] = this.imgAssociateImage.ImageUrl;
                        ms.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Image page load Status
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param> 
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;

            // pnlNineNameSuggestion.Visible = false; 
            string loginID = string.Empty;
            try
            {
                if (!Page.IsPostBack)
                {
                    this.tblDetails.Visible = false;
                    DataSet dslocation = this.location.GetIDCardLocation(Session["LoginId"].ToString());
                    string strlocationID = dslocation.Tables[1].Rows[0]["LocationID"].ToString();
                    this.hdnLocation.Value = strlocationID;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Button Generate ID Card Click
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param> 
        protected void BtnGenerateIDCard_Click(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;

            // pnlNineNameSuggestion.Visible = false;
            string id = XSS.HtmlEncode(Convert.ToString(this.txtID.Text.Trim()));
            try
            {
                DataTable dtdisplay = this.objRequestDetailsBL.PreviewDetails(id);
                string strDisplayName = dtdisplay.Rows[0]["DisplayName"].ToString().ToUpper();
                string assocId = dtdisplay.Rows[0]["AssociateID"].ToString().Trim();
                this.hdnAssocId.Value = Convert.ToString(assocId);
                string strDesiredName = string.Empty;
                string strgfirstName = /*"SAI KRISHNA";//*/dtdisplay.Rows[0]["FirstName"].ToString().ToUpper().Trim();
                string middleInitial = /*"TEJA";//*/dtdisplay.Rows[0]["MiddleName"].ToString().ToUpper().Trim();
                string strglastName = /*"POTHAPRAGADA";//*/dtdisplay.Rows[0]["LastName"].ToString().ToUpper().Trim();
                var psassociateName = string.Empty;
                this.pnlError.Visible = false;
                if (dtdisplay == null)
                {
                    this.lblMessage.Visible = true;
                    this.lblMessage.Text = "Enter a Valid Associate ID";
                }
                else
                {
                    if (this.ValidToGenerate(dtdisplay))
                    {
                        if (!string.IsNullOrEmpty(strDisplayName))
                        {
                            // #DPNAME Session["DisplayName"] = lblDisplayNameText.Text.Replace(":", "").Trim();
                            this.CallIdCardPage(strDisplayName, this.hdnLocation.Value);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(middleInitial))
                            {
                                psassociateName = strgfirstName.ToUpper() + " " + middleInitial.ToUpper() + " " + strglastName.ToUpper();
                            }
                            else
                            {
                                psassociateName = strgfirstName.ToUpper() + " " + strglastName.ToUpper();
                            }

                            if (psassociateName.Length <= 20)
                            {
                                strDesiredName = psassociateName;

                                // #DPNAME Session["DisplayName"] = lblDisplayNameText.Text.Replace(":", "").Trim();
                                this.CallIdCardPage(strDesiredName, this.hdnLocation.Value);
                            }
                            else
                            {
                                var idcardAssName = this.idcardService.GetNameToBePrintedByAssociateId(assocId);
                                if (!string.IsNullOrEmpty(idcardAssName))
                                {
                                    strDesiredName = idcardAssName.ToUpper();
                                    this.CallIdCardPage(strDesiredName, this.hdnLocation.Value);

                                    // VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objStoreChosenName = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                                    // DataTable dt = objStoreChosenName.StoreChosenName(hdnAssocId.Value, hdnNameChosen.Value); no need to store name frm Id cars service to local DB
                                }
                                else
                                {
                                    VMSBusinessLayer.RequestDetailsBL objCheckChosenName = new VMSBusinessLayer.RequestDetailsBL();
                                    string result = objCheckChosenName.CheckNameAlreadyExists(XSS.HtmlEncode(this.hdnAssocId.Value));
                                    if (!string.IsNullOrEmpty(result))
                                    {
                                        strDesiredName = result.ToUpper();
                                        this.CallIdCardPage(strDesiredName, this.hdnLocation.Value);
                                    }
                                    else
                                    {
                                        this.mid.Visible = true;
                                        this.last.Visible = true;
                                        this.lblInformation.Text = "The name " + psassociateName + " exceeds 20 characters.";
                                        this.lblInformatn.Text = "Please select a name, to be printed in the ID card";
                                        IList<string> middleNameList = new List<string>();
                                        IList<string> lastNameList = new List<string>();
                                        IList<string> firstNameList = new List<string>();
                                        firstNameList = this.GenerateNameSuggestionBasedFirstName(strgfirstName, middleInitial, strglastName);
                                        this.firstCount = firstNameList.Count;
                                        if (!string.IsNullOrEmpty(middleInitial))
                                        {
                                            middleNameList = this.GenerateNameSuggestionBasedMiddleName(strgfirstName, middleInitial, strglastName);
                                            this.midCount = middleNameList.Count;
                                        }

                                        if (!string.IsNullOrEmpty(strglastName))
                                        {
                                            lastNameList = this.GenerateNameSuggestionBasedLastName(strgfirstName, middleInitial, strglastName);
                                            this.lastCount = lastNameList.Count;
                                        }

                                        string rbtnNames = string.Empty;
                                        string table = @"<table width=100%><tr><td width=210px Style=text-align:left>";
                                        for (int i = 0; i < this.firstCount; i++)
                                        {
                                            if (i == 0)
                                            {
                                                table += rbtnNames;
                                            }

                                            table += @"<input type=radio name=names id=fName" + i + " value=" + firstNameList[i].Replace(' ', '-') + " onchange=NameSelect(this) /><span id=span1 style=font-size:11px>" + firstNameList[i] + "</span> </br>";
                                        }

                                        if (this.midCount > 0)
                                        {
                                            table += @"</td><td width=230px Style=text-align:left>";
                                            for (int i = 0; i < this.midCount; i++)
                                            {
                                                table += @"<input type=radio name=names id=mName" + i + " value=" + middleNameList[i].Replace(' ', '-') + " onchange=NameSelect(this) /><span id=span2 style=font-size:11px text-align:center >" + middleNameList[i] + "</span> </br>";
                                            }
                                        }
                                        else
                                        {
                                            this.mid.Visible = false;

                                            // mm.Visible = false;
                                        }

                                        if (this.lastCount > 0)
                                        {
                                            table += @"</td><td width=210px Style=text-align:left>";
                                            for (int i = 0; i < this.lastCount; i++)
                                            {
                                                table += @"<input type=radio name=names id=lName" + i + " value=" + lastNameList[i].Replace(' ', '-') + " onchange=NameSelect(this) /><span id=span3 style=font-size:11px text-align:center>" + lastNameList[i] + "</span> </br>";
                                            }
                                        }
                                        else
                                        {
                                            this.last.Visible = false;

                                            // ll.Visible = false;
                                        }

                                        table += @"</td></tr></table>";
                                        this.container.InnerHtml = table;
                                        this.mdlNineNameSuggestions.Show();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this.pnlError.Visible = true;
                    }
                }
            }
            catch (FormatException)
            {
                this.lblMessage.Visible = true;
                this.lblMessage.Text = "Enter a Valid Associate ID";
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Button Generate ID Card Click
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param> 
        protected void ImgbtnClearID_Click(object sender, ImageClickEventArgs e)
        {
            this.txtID.Text = string.Empty;
            this.pnlDetails.Visible = false;
            
            // Response.Redirect("GenerateIDCard.aspx");
        }

        /// <summary>
        /// Button Generate ID Card Click
        /// </summary>
        /// <param name="applicantID">applicant ID</param>  
        /// <param name="associateID">associate ID</param> 
        protected void Generate_IDCard(string applicantID, string associateID)
        {
            string loginID = string.Empty;
            DataTable dtiddetails;
            VMSBusinessLayer.UserDetailsBL applicantDetails = new VMSBusinessLayer.UserDetailsBL();
            VMSBusinessLayer.RequestDetailsBL objtRequestDetailsBL = new VMSBusinessLayer.RequestDetailsBL();

            try
            {
                if (this.Session["LoginId"] != null)
                {
                    loginID = Session["LoginId"].ToString().Trim();
                    this.tblDetails.Visible = false;
                    dtiddetails = objtRequestDetailsBL.PreviewDetails(associateID);
                    string encryptedData = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(applicantID)));
                    string str1Script = "<script language='javascript'>window.open('IDCard.aspx?key=" + encryptedData + "', 'List', 'toolbar=no,menubar=no,scrollbars=no,resizable=no,width=207,height=700, location=center', false);</script>";
                    ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", str1Script);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Button search image
        /// </summary>
        /// <param name="sender">object sender</param>  
        /// <param name="e">image click event</param> 
        protected void ImgbtnSearchImage_Click(object sender, ImageClickEventArgs e)
        {
            this.imgAssociateImage.ImageUrl = hdnAssociateImage.Value = "Images/char.jpeg";
            VMSBusinessLayer.RequestDetailsBL objtRequestDetailsBL = new VMSBusinessLayer.RequestDetailsBL();
            VMSBusinessLayer.UserDetailsBL objlocation = new VMSBusinessLayer.UserDetailsBL();
            EmployeeBL objEmployeeBL = new EmployeeBL();
            string id = XSS.HtmlEncode(this.txtID.Text.Trim().ToString());
            string encryptedData;
            DataTable dtdisplay;
            this.hdnAssocId.Value = XSS.HtmlEncode(this.txtID.Text);
            this.tblDetails.Visible = false;
            try
            {
                if (this.Session["LoginId"] != null)
                {
                    this.pnlDetails.Visible = true;
                    string loginID = Session["LoginId"].ToString().Trim();
                    DataSet dslocation = objlocation.GetIDCardLocation(loginID);
                    string strLocation = dslocation.Tables[0].Rows[0]["Location"].ToString();
                    encryptedData = VMSBusinessLayer.Encrypt(id);
                    using (dtdisplay = objtRequestDetailsBL.PreviewDetails(id))
                    {
                        if (dtdisplay == null)
                        {
                            this.lblMessage.Visible = true;
                            this.lblMessage.Text = "Enter a Valid Associate ID";
                        }
                        else
                        {
                            this.Session["ImageURL"] = null;
                            this.Session["DisplayName"] = null;
                            string strURL = Convert.ToString(dtdisplay.Rows[0]["ImageURL"]);
#pragma warning disable CS0219 // The variable 'pattern' is assigned but its value is never used
                            string pattern = "[^0-9]";
#pragma warning restore CS0219 // The variable 'pattern' is assigned but its value is never used
                            this.tblDetails.Visible = true;
                            this.hdnFileUploadId.Value = Convert.ToString(dtdisplay.Rows[0]["FileUploadID"]);
                            this.lblCityText.Text = ": " + strLocation;
                            this.lblIDText.Text = ": " + Convert.ToString(dtdisplay.Rows[0]["AssociateID"]);
                            this.lblFirstNameText.Text = ": " + Convert.ToString(dtdisplay.Rows[0]["FirstName"]).ToUpper();
                            this.lblLastNameText.Text = ": " + Convert.ToString(dtdisplay.Rows[0]["LastName"]).ToUpper();

                            if (dtdisplay.Rows[0]["BloodGroup"] != null)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dtdisplay.Rows[0]["BloodGroup"]).Trim()))
                                {
                                    if ((string.Compare(Convert.ToString(dtdisplay.Rows[0]["BloodGroup"].ToString().Trim().ToUpper()), VMSConstants.VMSConstants.UNKNOWNBLOODLONG) == 0) || (string.Compare(Convert.ToString(dtdisplay.Rows[0]["BloodGroup"].ToString().Trim().ToUpper()), VMSConstants.VMSConstants.UNKNOWNBLOODSHORT) == 0))
                                    {
                                        // lblBloodGroupText.Text = ": " + string.Empty;
                                    }
                                    else
                                    {
                                        // lblBloodGroupText.Text = ": " + Convert.ToString(dtDisplay.Rows[0]["BloodGroup"]);
                                    }
                                }
                                else
                                {
                                    // lblBloodGroupText.Text = ": " + VMSConstants.VMSConstants.NotAvaliable;
                                }
                            }
                            else
                            {
                                // lblBloodGroupText.Text = ": " + VMSConstants.VMSConstants.NotAvaliable;
                            }

                            if (dtdisplay.Rows[0]["EmergencyContact"] != null)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dtdisplay.Rows[0]["EmergencyContact"])))
                                {
                                    // lblEmergencyContactText.Text = ": " + String.Concat("+91 ", System.Text.RegularExpressions.Regex.Replace(dtDisplay.Rows[0]["EmergencyContact"].ToString().Trim(), pattern, ""));
                                }
                                else
                                {
                                    // lblEmergencyContactText.Text = ": " + VMSConstants.VMSConstants.NotAvaliable;
                                }
                            }
                            else
                            {
                                // lblEmergencyContactText.Text = ": " + VMSConstants.VMSConstants.NotAvaliable;
                            }

                            //if (!string.IsNullOrEmpty(this.hdnFileUploadId.Value))
                            //{
                            //    ////GenericFileUpload gnfileUpload = new GenericFileUpload();
                                ////byte[] data = gnfileUpload.GetAssociateImage(RegExValidate(this.hdnFileUploadId.Value));
                                ////int imwidth = 0;
                                ////int imheight = 0;
                                ////if (data != null)
                                ////{
                                    
                                ////    System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(data));
                                ////    imwidth = image.Width;
                                ////    imheight = image.Height;
                                    ////this.imgAssociateImage.ImageUrl = string.Concat("GetImage1.ashx?IDCARD=", id, "&w=", imwidth, "&h=", imheight, "&TempId=", XSS.HtmlEncode(this.hdnFileUploadId.Value));
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "GetImageFromIDCard", "GetImageFromIDCard();", true);
                                   // this.Session["ImageURL"] = this.imgAssociateImage.ImageUrl;
                                ////}
                                ////else
                                ////{
                                ////    this.imgAssociateImage.ImageUrl = VMSConstants.VMSConstants.IMAGEPATH;
                                    
                                
                                ////}
                                ////this.hdnFileDwnLoad.Value = "Failed";
                            ////}
                            ////else
                            ////{
                            ////    this.imgAssociateImage.ImageUrl = VMSConstants.VMSConstants.IMAGEPATH;
                            ////}

                            if (string.IsNullOrEmpty(Convert.ToString(dtdisplay.Rows[0]["DisplayName"])))
                            {
                                VMSBusinessLayer.RequestDetailsBL objCheckChosenName = new VMSBusinessLayer.RequestDetailsBL();
                                string result = objCheckChosenName.CheckNameAlreadyExists(XSS.HtmlEncode(this.hdnAssocId.Value));
                                if (!result.Equals(string.Empty))
                                {
                                    this.lblDisplayNameText.Text = ": " + result.ToUpper();
                                }
                                else
                                {
                                    this.lblDisplayNameText.Text = ": " + string.Empty;
                                }
                            }
                            else
                            {
                                this.lblDisplayNameText.Text = ": " + Convert.ToString(dtdisplay.Rows[0]["DisplayName"]).ToUpper();
                                this.Session["DisplayName"] = Convert.ToString(dtdisplay.Rows[0]["DisplayName"]).ToUpper();
                            }

                            if (Convert.ToString(dtdisplay.Rows[0]["HR_Status"]) == "I")
                            {
                                this.lblMessage.Visible = true;
                                this.lblMessage.Text = Resources.LocalizedText.TerminatedMessage;
                                string strScript = string.Concat("<script>alert('", Resources.LocalizedText.TerminatedMessage, "'); </script>");
                                ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
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
        /// Button Generate ID Card Click
        /// </summary>
        /// <param name="img">object sender</param>  
        /// <param name="cropArea">crop Area</param> 
        /// <returns>Cropped image</returns>
        private static System.Drawing.Image CropImage(System.Drawing.Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (System.Drawing.Image)bmpCrop;
        }

        /// <summary>
        /// Button Generate ID Card Click
        /// </summary>
        /// <param name="data">object sender</param>  
        /// <param name="wiwidth">image width</param> 
        /// <param name="wiheight">image height</param> 
        /// <returns>Write image</returns>
        private System.Drawing.Image Writeimage(byte[] data, int wiwidth, int wiheight)
        {
            try
            {
                if (data != null)
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                        return returnImage;
                    }
                }
                else
                {
                    
                        string Targetpage = "~\\Images\\DummyPhoto.png";
                        Response.Redirect(Targetpage, true);
                                                           
                    return null;
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                return null;

            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// Button Generate ID Card Click
        /// </summary>
        /// <param name="data">object sender</param>  
        /// <param name="diwidth">image width</param> 
        /// <param name="diheight">image height</param> 
        /// <returns>Draw image</returns>
        private string Drawimage(byte[] data, int diwidth, int diheight)
        {
            string strFileName = string.Empty;
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

                        if (diwidth * diheight != 0)
                        {
                            if (diwidth > 0)
                            {
                                factor = diwidth / x;
                            }
                            else if (diheight > 0)
                            {
                                factor = diheight / y;
                            }
                        }

                        Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
                        Graphics g = Graphics.FromImage(imgOut);
                        g.Clear(Color.White);
                        g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);
                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        imgOut.Save(outStream, ImageFormat.Jpeg);
                        byte[] buffer = outStream.ToArray();
                        strFileName = string.Concat("FilesServer\\Associate", "-", DateTime.Now.ToString("dd-MM-yy"), " ", DateTime.Now.ToString("hh-mm-ss-fff"), ".jpeg");
                        string strFile = string.Concat(Server.MapPath("\\").ToString(), strFileName);
                        File.WriteAllBytes(strFile, buffer);
                    }
                }
                else
                {
                   
                        string Targetpage = "~\\Images\\DummyPhoto.png";
                        Response.Redirect(Targetpage, true);
                        
                  
                    
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

            return strFileName;
        }

        /// <summary>
        /// Get dimensions
        /// </summary>
        /// <param name="strFileName">file name</param>  
        /// <returns>integer dimensions</returns> 
        private int[] GetDimensions(string strFileName)
        {
            int[] dim = new int[2];
            try
            {
                System.Drawing.Image imgOrig = new Bitmap(strFileName);
                int idwidth = 0, idheight = 0;

                // Dimension of the Original Image
                int imgWidth = imgOrig.Width;
                int imgHeight = imgOrig.Height;
                if (imgWidth > imgHeight)
                {
                    idwidth = 400;
                    idheight = imgHeight * idwidth / imgWidth;
                }
                else
                {
                    idheight = 300;
                    idwidth = imgWidth * idheight / imgHeight;
                }

                dim[0] = idwidth;
                dim[1] = idheight;
                imgOrig.Dispose();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return dim;
        }

        /// <summary>
        /// checks file exists or not
        /// </summary>
        /// <param name="path">object sender</param>  
        /// <returns>Boolean value</returns>
        private bool FileExist(string path)
        {
            FileInfo imageFile = new FileInfo(path);
            bool fileExists = imageFile.Exists;
            return fileExists;
        }

        /// <summary>
        /// checks file exists or not
        /// </summary>
        /// <param name="path">object sender</param>  
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
        /// Valid to Generate
        /// </summary>
        /// <param name="dtgenerate">object generate</param>  
        /// <returns>Boolean value</returns>
        private bool ValidToGenerate(DataTable dtgenerate)
        {
            bool returnValue = true;
            ArrayList armessages = new ArrayList();
            ArrayList arcontrols = new ArrayList();
            try
            {
                if (string.IsNullOrEmpty(dtgenerate.Rows[0]["FirstName"].ToString()))
                {
                    arcontrols.Add("First Name");
                }

                if (string.IsNullOrEmpty(dtgenerate.Rows[0]["LastName"].ToString()))
                {
                    arcontrols.Add("Last Name");
                }

               
                    if (this.hdnAssociateImage.Value == "" || this.hdnAssociateImage.Value == "Images/char.jpeg")
                    {
                        arcontrols.Add("Associate Photo");
                    }
                
                if (arcontrols.Count > 0)
                {
                    armessages.Add("The following information is not available to print ID Card:");
                    foreach (string val in arcontrols)
                    {
                        armessages.Add("     - " + val + " \n");
                    }
                }

                if (armessages.Count > 0)
                {
                    BulletedList blmessages = new BulletedList();
                    returnValue = false;
                    blmessages.DataSource = armessages;
                    blmessages.DataBind();
                    this.pnlError.Controls.Add(blmessages);
                }
                else
                {
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return returnValue;
        }

        /// <summary>
        /// Valid to Generate
        /// </summary>
        /// <param name="dtgenerate">object generate</param>  
        /// <returns>Boolean value</returns>
        private bool ValidAssociateDetails(DataTable dtgenerate)
        {
            try
            {
                bool returnValue = true;

                if (string.IsNullOrEmpty(dtgenerate.Rows[0]["FirstName"].ToString()))
                {
                    returnValue = false;
                }

                if (string.IsNullOrEmpty(dtgenerate.Rows[0]["LastName"].ToString()))
                {
                    returnValue = false;
                }

                if (string.IsNullOrEmpty(dtgenerate.Rows[0]["BloodGroup"].ToString()))
                {
                    returnValue = false;
                }

                if (string.IsNullOrEmpty(dtgenerate.Rows[0]["EmergencyContact"].ToString()))
                {
                    returnValue = false;
                }
                ////if (string.IsNullOrEmpty(dtgenerate.Rows[0]["ImageURL"].ToString()))
                ////{
                ////    returnValue = false;
                ////}

                return returnValue;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return false;
            }
        }

        /// <summary>
        /// To download image
        /// </summary>
        /// <param name="data">byte data</param>
        /// <param name="associateId">associate id</param>
        private void DownloadImage(byte[] data, string associateId)
        {
            try
            {
                if (data != null)
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        string filename = string.Empty;
                        if (!string.IsNullOrEmpty(associateId))
                        {
                            filename = associateId + ".jpeg";
                        }
                        else
                        {
                            filename = "download.jpeg";
                        }

                        System.Drawing.Image newImage;
                        ms.Write(data, 0, data.Length);
                        newImage = System.Drawing.Image.FromStream(ms, true);
                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        newImage.Save(outStream, ImageFormat.Jpeg);
                        byte[] buffer = outStream.ToArray();
                        Response.ContentType = "image/jpeg";
                        Response.BinaryWrite((byte[])buffer);
                        Response.AppendHeader("Content-Disposition:", "attachment; filename=" + XSS.HtmlEncode(filename) + string.Empty);
                        Response.ContentType = "application/download";
                    }
                }
                else
                {
                     
                        string TargetPage = "~\\Images\\DummyPhoto.png";
                        Response.Redirect(TargetPage, true);
                         
                    
                    
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
            catch
            {
                try
                {
                    string Targetpage = "~\\Images\\DummyPhoto.png";
                    Response.Redirect(Targetpage, true);
                    
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }
    }
}
