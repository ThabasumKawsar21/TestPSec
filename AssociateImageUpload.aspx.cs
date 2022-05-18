
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// image upload partial class
    /// </summary>
    public partial class AssociateImageUpload : System.Web.UI.Page
    {
        #region Public Methods
        
        /// <summary>
        /// The roles field
        /// </summary>        
        private List<string> roles = new List<string>();

        // Method to handle the operations On Page Load
        
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.roles = (List<string>)Session["UserRole"];
            Page.Form.DefaultButton = this.btnView.UniqueID;

            if (this.Session["Webcamimage"] != null)
            {
                ////this.UImage.ImageUrl = "../EmployeeImage.aspx?strImage=Photo";
                this.ImgUpload.Enabled = false;
                this.btnSave.Enabled = true;
                byte[] visitorImage = (byte[])Session["Webcamimage"];
                this.UImage.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(visitorImage);
            }
            else
            {
                this.UImage.ImageUrl = "./Images/DummyPhoto.png";
                this.ImgUpload.Enabled = true;
            }
          
            if (!Page.IsPostBack)
            {
                this.ResetControls();
                ////txtAssociateID.Attributes.Add("onkeypress", "javascript:return SpecialCharacterValidation(this);");
                string strFileTypes = ConfigurationManager.AppSettings["SupportedFileTypes"].Trim().ToString();
                string strSupportedtypes = strFileTypes.Replace(',', '/');
                if (this.Session["Webcamimage"] == null)
                {
                    this.lblImage.Text = string.Concat(VMSConstants.VMSConstants.PLEASESELECT, strSupportedtypes, 
                    " ", VMSConstants.VMSConstants.FILETYPES);
                }
            }
        }

        // Method to Save the Image of the Associate
        
        /// <summary>
        /// The Save Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string returnId = string.Empty;
            bool blnSize = false;
            string strFileName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(this.ImgUpload.FileName))
                    {
                        HttpPostedFile filetoUpload = this.ImgUpload.PostedFile;
                        int maxSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxImageSize"].Trim().ToString());

                        if (filetoUpload.ContentLength < maxSize)
                        {
                            bool imagetype = this.CheckFileExtension(filetoUpload.FileName.ToString());
                            if (imagetype == false)
                            {
                                this.ResetControls();
                                this.errortbl.Visible = true;
                                this.lblMessage.Text = VMSConstants.VMSConstants.SELECTVALIDIMAGEFILE;
                                return;
                            }

                            blnSize = true;
                            returnId = this.UploadFiletoServer();
                        }
                        else
                        {
                            this.errortbl.Visible = true;
                            this.lblMessage.Text = VMSConstants.VMSConstants.IMAGEUPLOADSIZE;
                        }
                    }

                if (this.Session["Webcamimage"] != null)
                    {
                        returnId = this.UploadCamImagetoServer();
                    }

                this.lblMessage.Visible = true;
                    if (!string.IsNullOrEmpty(returnId))
                    {
                        ////code to store in tbl_FileContentID
                        VMSBusinessLayer.RequestDetailsBL objStoreFileContentID = new VMSBusinessLayer.RequestDetailsBL();
                        bool result = objStoreFileContentID.StoreFileContentID(this.txtAssociateID.Text.Trim(), returnId, System.Configuration.ConfigurationManager.AppSettings["AppKey"].ToString());
                        ////Delete/viewphoto
                        ////txtAssociateID.Text = string.Empty;
                        if (result)
                        {
                            this.BtnView_Click(null, null);
                            this.errortbl.Visible = true;
                            this.lblMessage.Text = string.Concat(VMSConstants.VMSConstants.IMAGEUPLOADEDSUCESSFULLY, XSS.HtmlEncode(this.txtAssociateID.Text.ToString()));
                            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                    else if (blnSize)
                    {
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = VMSConstants.VMSConstants.IMAGEUPLOADFAILED;
                    }
                }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
 
        // Method to clear values while clicking on clear button
        
        /// <summary>
        /// The Clear Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.ResetControls();
        }

        // Method to View the Associates Image
        
        /// <summary>
        /// The View Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnView_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL = new VMSBusinessLayer.RequestDetailsBL();
            if (objEmployeeDetails.ValidateAssociateDetails(this.txtAssociateID.Text.Trim().ToString()))
            {
                DataRow dremployeeInfo = objEmployeeDetails.GetEmployeeDetails(XSS.HtmlEncode(this.txtAssociateID.Text.Trim().ToString()));
                if (dremployeeInfo != null)
                {
                    this.ImgUpload.Enabled = true;
                    this.btnCam.Disabled = false;
                    this.hdnFileID.Value = XSS.HtmlEncode(dremployeeInfo["FileUploadID"].ToString());
                    ////if (string.IsNullOrEmpty(drEmployeeInfo["AssociateImage"].ToString()))
                    ////{
                    //    UImage.ImageUrl = "~\\Images\\DummyPhoto.png";
                    //    errortbl.Visible = true;
                    //    lblMessage.Visible = true;
                    //    lblMessage.Text = "Image not available";
                    //    btnDelete.Visible = false;
                    //    hdnFileID.Value = string.Empty;
                    ////}
                    ////else
                    ////{

                    if (!string.IsNullOrEmpty(this.hdnFileID.Value))
                    {
                        GenericFileUpload gnfileUpload = new GenericFileUpload();
                        byte[] data = gnfileUpload.GetAssociateImage(this.hdnFileID.Value);
                        int width = 0;
                        int height = 0;
                        this.UImage.Visible = true;
                        if (data != null)
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(
                                new System.IO.MemoryStream(data));
                            width = image.Width;
                            height = image.Height;
                            this.UImage.ImageUrl = string.Concat("GetImage1.ashx?IDCARD=", this.ID, "&w=", width, "&h=", height, "&TempId=", XSS.HtmlEncode(this.hdnFileID.Value));
                            this.errortbl.Visible = false;
                            this.btnDelete.Visible = true;
                        }
                        else
                        {
                            this.UImage.ImageUrl = "~\\Images\\DummyPhoto.png";
                            this.errortbl.Visible = true;
                            this.lblMessage.Visible = true;
                            this.lblMessage.Text = "Image not available";
                            this.btnDelete.Visible = false;
                            this.hdnFileID.Value = string.Empty;
                        }

                        if (this.roles[0] == "IDCardAdmin")
                        {
                            this.btnDelete.Visible = false;
                        }

                        this.hdnFileID.Value = XSS.HtmlEncode(dremployeeInfo["FileUploadID"].ToString());
                    }
                    else
                    {
                        this.UImage.ImageUrl = "~\\Images\\DummyPhoto.png";
                        this.errortbl.Visible = true;
                        this.lblMessage.Visible = true;
                        this.lblMessage.Text = "Image not available";
                        this.btnDelete.Visible = false;
                        this.hdnFileID.Value = string.Empty;
                    }
                }
            }
            else
            {
                this.errortbl.Visible = true;
                this.lblMessage.Visible = true;
                this.lblMessage.Text = "Enter Valid AssociateID";
                this.UImage.ImageUrl = "~\\Images\\DummyPhoto.png";
                this.btnDelete.Visible = false;
                this.btnSave.Enabled = false;
                this.ImgUpload.Enabled = false;
                return;
            }
        }

        // Method to Delete the Image of an Associate
        
        /// <summary>
        /// The Delete Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                VMSBusinessLayer.RequestDetailsBL objDeleteID = new VMSBusinessLayer.RequestDetailsBL();
                GenericFileUpload flobj = new GenericFileUpload();
                flobj.DeleteFile(this.hdnFileID.Value.ToString().Trim(), Session["LoginID"].ToString());
                bool result = objDeleteID.DeleteFileContentID(this.txtAssociateID.Text.Trim());
                if (result)
                {
                    this.btnDelete.Visible = false;
                    this.errortbl.Visible = true;
                    this.lblMessage.Visible = true;
                    this.lblMessage.Text = "Image deleted successfully.";
                    this.UImage.ImageUrl = "~\\Images\\DummyPhoto.png";
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        #endregion

        #region Private Methods

        // Method to ResetControls' Visiblity and Enabled Properties
        
        /// <summary>
        /// The ResetControls method
        /// </summary>        
        private void ResetControls()
        {
            this.txtAssociateID.Text = string.Empty;
            this.Session["Webcamimage"] = null;
            this.UImage.ImageUrl = "./Images/DummyPhoto.png";
            this.errortbl.Visible = false;
            this.btnDelete.Visible = false;
            this.panelEmp.Visible = true;
            this.ImgUpload.Enabled = false;
            this.btnCam.Disabled = true;
        }
        
        /// <summary>
        /// The Upload File to Server method
        /// </summary>
        /// <returns>The string type object</returns>        
        private string UploadFiletoServer()
        {
            string returnId = string.Empty;
            Stream fs = this.ImgUpload.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            try
            {
                HttpPostedFile filetoUpload = this.ImgUpload.PostedFile;
                string fileExtension = Path.GetExtension(filetoUpload.FileName);
                if (string.IsNullOrEmpty(filetoUpload.FileName))
                {
                    this.errortbl.Visible = true;
                }
                else
                {
                    GenericFileUpload fileUpload = new GenericFileUpload();
                    string strUserID = Session["LoginID"].ToString();
                    byte[] bytes = br.ReadBytes((int)fs.Length);
                    returnId = fileUpload.UploadFile(bytes, Path.GetFileName(this.ImgUpload.PostedFile.FileName).ToString(), this.txtAssociateID.Text.Trim(), strUserID);
                }               
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
            finally
            {
                br.Close();
                fs.Close();
                fs.Dispose();
            }

            return returnId;
        }

        // Method to Insert the Web Cam Image to server
        
        /// <summary>
        /// The Upload Camera Image to Server method
        /// </summary>
        /// <returns>The string type object</returns>        
        private string UploadCamImagetoServer()
        {
            string returnId = string.Empty;
            try
            {
                GenericFileUpload fileUpload = new GenericFileUpload();
                string strUserID = Session["LoginID"].ToString();
                byte[] bytes = (byte[])Session["Webcamimage"];
                string strFileName = string.Concat("Associate", "-", DateTime.Now.ToString("dd-MM-yy"), " ", DateTime.Now.ToString("hh-mm-ss-fff"), ".jpeg");
                returnId = fileUpload.UploadFile(bytes, strFileName, this.txtAssociateID.Text.Trim(), strUserID);
             }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return returnId;
        }

        // Method to validate the file extension
        
        /// <summary>
        /// The CheckFileExtension method
        /// </summary>
        /// <param name="strfilePath">The file Path parameter</param>
        /// <returns>The boolean type object</returns>        
        private bool CheckFileExtension(string strfilePath)
        {
            bool imageStatus = false;
            string strypes = ConfigurationManager.AppSettings["SupportedFileTypes"].ToString().Trim();
            string[] splittedFile = { "," };
            string[] strSplittedSupportedTypes = strypes.Split(splittedFile, StringSplitOptions.None);
            string[] splittedFilePath = { "." };
            string[] strSplittedTypes = strfilePath.Split(splittedFilePath, StringSplitOptions.None);
            int arraylen = strSplittedTypes.Length - 1;
            string strExtension = strSplittedTypes[arraylen].ToString();

            for (int j = 0; j < strSplittedSupportedTypes.Length; j++)
            {
                if (strSplittedSupportedTypes[j].ToUpper().ToString() == strExtension.ToUpper().ToString())
                {
                    imageStatus = true;
                }
            }

            return imageStatus;
        }

        // Method to delete the file in the Server
        
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

        #endregion
    }
}
