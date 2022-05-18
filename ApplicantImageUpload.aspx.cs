

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.Sql;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// The Applicant image upload
    /// </summary>
    public partial class ApplicantImageUpload : System.Web.UI.Page
    {
        #region Event Methods
        
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblApplicantIDError.Visible = false;
            this.lblMessage.Visible = false;
            ////lblEmergencyContactError.Visible = false;
            ////lblBloodGroupError.Visible = false;
         
             string loginID = string.Empty;
             string strImage = string.Empty;
             try
             {
                 try
                 {
                     loginID = Session["LoginID"].ToString().Trim();
                 }
                 catch (NullReferenceException)
                 {
                    try
                    {
                        Response.Redirect("~/Login.aspx", true);
                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {

                    }
                }

                 if (!this.IsPostBack)
                 {
                     ////ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('" + Session["LoginID"].ToString() + "');</script>");
                     VMSBusinessLayer.UserDetailsBL location = new VMSBusinessLayer.UserDetailsBL();
                     DataSet dslocation = location.GetIDCardLocation(loginID);
                     string s = dslocation.Tables[0].Rows[0]["Location"].ToString();
                     EmployeeBL objEmployeeBL = new EmployeeBL();
                     this.ddlLocation.DataSource = objEmployeeBL.GetIDCardLocations();
                     this.ddlLocation.DataTextField = "Location";
                     this.ddlLocation.DataBind();
                     this.ddlLocation.Items.Insert(0, "--- Select a Location ---");
                     this.ddlLocation.Items.FindByText(s).Selected = true;
                     this.btnSave.Enabled = false;

                     ////UnComment the next line

                     this.Session["Webcamimage"] = null;

                     ////Comment the following before moving to prod

                     ////Byte[] data = objEmployeeBL.GetEmployeeImageDetails("221107");
                     ////Session["Webcamimage"] = data;
                 }
                 else
                 {
                     if (this.Session["Webcamimage"] != null)
                     {
                         if (!string.IsNullOrEmpty(Session["Webcamimage"].ToString()))
                         {
                             this.UImage.ImageUrl = "../EmployeeImage.aspx?strImage=Photo";
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
        /// The Save Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string strEncryptedBinaryData = string.Empty;
            this.lblApplicantIDError.Visible = false;
            ////lblEmergencyContactError.Visible = false;
            ////lblBloodGroupError.Visible = false;
            EmployeeBL objEmployeeBL = new EmployeeBL();
            int blnSuccess = 4;
            try
            {
                if (this.ValidateEntries())
                {
                    if (!string.IsNullOrEmpty(Session["Webcamimage"].ToString()))
                    {
                        string strBinaryData = Encoding.Default.GetString((byte[])Session["Webcamimage"]);
                        blnSuccess = objEmployeeBL.InsertApplicantImgInDB(this.txtApplicantIDCapture.Text.Trim().ToString(), strBinaryData, null, null, this.ddlLocation.SelectedItem.Text.Trim().ToString(), Session["LoginID"].ToString().Trim());
                        this.Session["Webcamimage"] = null;
                    }
                    else if (this.ImgUpload.PostedFile != null)
                    {
                        string strFileName = string.Empty;

                        if (!string.IsNullOrEmpty(this.ImgUpload.FileName))
                        {
                            HttpPostedFile filetoUpload = this.ImgUpload.PostedFile;
                            strFileName = this.UploadFiletoServer();
                            bool imagetype = this.CheckFileExtension(strFileName);
                            if (imagetype == false)
                            {
                                ////errortbl.Visible = true;
                                this.lblMessage.Visible = true;
                                this.lblMessage.Text = "Please Select valid image file";
                            }
                            else
                            {
                                string strBinaryData = this.ReadFile(strFileName);
                                blnSuccess = objEmployeeBL.InsertApplicantImgInDB(this.txtApplicantIDCapture.Text.Trim().ToString(), strBinaryData, null, null, this.ddlLocation.SelectedItem.Text.Trim().ToString(), Session["LoginID"].ToString().Trim());
                                this.DeleteFile(strFileName);
                            }
                        }
                        else if (string.IsNullOrEmpty(this.ImgUpload.FileName))
                        {
                            ////errortbl.Visible = true;
                            this.lblMessage.Visible = true;
                            this.lblMessage.Text = "Please Select Image";
                        }
                    }
                    else
                    {
                        blnSuccess = 0;
                    }

                    switch (blnSuccess)
                    {
                        case 0:
                            ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('Data Not Inserted. Applicant ID maybe already existing in a different location.');</script>");
                            this.Session["Webcamimage"] = null;
                            this.txtApplicantIDCapture.Text = string.Empty;
                            this.UImage.ImageUrl = "../images/DummyPhoto.png";
                            break;
                        case 1:
                            this.lblApplicantIDError.Visible = false;
                            this.btnSave.Enabled = false;
                            string encryptedData = VMSBusinessLayer.Encrypt(XSS.HtmlEncode(this.txtApplicantIDCapture.Text.Trim()));
                            this.UImage.ImageUrl = "AssociateImage.aspx?ApplicantID=" + encryptedData;
                            ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('Data Inserted Successfully');</script>");
                            break;
                        case 2:
                            this.lblApplicantIDError.Visible = false;
                            this.btnSave.Enabled = false;
                            string encryptedData2 = VMSBusinessLayer.Encrypt(XSS.HtmlEncode(this.txtApplicantIDCapture.Text.Trim()));
                            this.UImage.ImageUrl = "AssociateImage.aspx?ApplicantID=" + encryptedData2;
                            ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('Data Updated Successfully');</script>");
                            break;
                        default:
                            break;
                    }
                    ////txtEmergencyContactNo.Text = string.Empty;
                    ////ddlBloodGroup.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);                
            }
        }

        /// <summary>
        /// The Clear Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ApplicantImageUpload.aspx", true);
                 
            }
            catch(System.Threading.ThreadAbortException ex)
            {

            }
        }

        /// <summary>
        /// The ImageButton1_Click1 method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImageButton1_Click1(object sender, ImageClickEventArgs e)
        {
            EmployeeBL objEmployeeBL = new EmployeeBL();
            this.Session["Webcamimage"] = string.Empty;
            if (string.IsNullOrEmpty(this.txtApplicantIDCapture.Text))
            {
                this.lblApplicantIDError.Text = System.Configuration.ConfigurationManager.AppSettings["ApplicantIDError"].ToString();
                this.lblApplicantIDError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (objEmployeeBL.ValidateCandidateID(this.txtApplicantIDCapture.Text.Trim()))
                {
                    this.lblApplicantIDError.Text = System.Configuration.ConfigurationManager.AppSettings["ApplicantIDValid"].ToString();
                    this.lblApplicantIDError.ForeColor = System.Drawing.Color.YellowGreen;
                    this.btnSave.Enabled = true;
                    string encryptedData = VMSBusinessLayer.Encrypt(XSS.HtmlEncode(this.txtApplicantIDCapture.Text.Trim()));
                    this.UImage.ImageUrl = "AssociateImage.aspx?ApplicantID=" + encryptedData;
                }
                else
                {
                    this.lblApplicantIDError.Text = System.Configuration.ConfigurationManager.AppSettings["ApplicantIDInValid"].ToString();
                    this.lblApplicantIDError.ForeColor = System.Drawing.Color.Red;
                    this.UImage.ImageUrl = "../images/DummyPhoto.png";
                }
            }

            this.lblApplicantIDError.Visible = true;
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// The Validate Entries method
        /// </summary>
        /// <returns>The type object</returns>        
        private bool ValidateEntries()
        {
            this.lblApplicantIDError.Visible = false;
            bool isValid = true;
            if (this.Session["Webcamimage"] == null && this.ImgUpload.PostedFile == null)
            {
                ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('Image not Available');</script>");
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// The Upload Server method
        /// </summary>
        /// <returns>The string type object</returns>        
        private string UploadFiletoServer()
        {
            string destinationPath = string.Empty;
            try
            {
                HttpPostedFile filetoUpload = this.ImgUpload.PostedFile;
                string fileExtension = Path.GetExtension(filetoUpload.FileName.Replace("\\", string.Empty).Replace("..", string.Empty));
                if (string.IsNullOrEmpty(filetoUpload.FileName))
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('Image File is not Available');</script>");
                    ////errortbl.Visible = true;
                }
                else
                {
                    string serverPath = Server.MapPath(@"AssociateImage/");
                    destinationPath = string.Concat(serverPath, "Associate", "-", DateTime.Now.ToString("dd-MM-yy"), " ", DateTime.Now.ToString("hh-mm-ss-fff"), fileExtension);
                    filetoUpload.SaveAs(destinationPath);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);                
            }

            return destinationPath;
        }

        /// <summary>
        /// The CheckFileExtension method
        /// </summary>
        /// <param name="strfilePath">The string file Path parameter</param>
        /// <returns>The type object</returns>        
        private bool CheckFileExtension(string strfilePath)
        {
            bool imageStatus = false;
            string strypes = ConfigurationManager.AppSettings["SupportedFileTypes"].Trim().ToString();
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

        /// <summary>
        /// The ReadFile method
        /// </summary>
        /// <param name="strImagePath">The string Image Path parameter</param>
        /// <returns>The string type object</returns>        
        private string ReadFile(string strImagePath)
        {
            try
            {
                FileInfo fileinfo = new FileInfo(strImagePath);
                string strBinaryData = string.Empty;
                byte[] dbytes = null;
                long numBytes = fileinfo.Length;
                FileStream fstream = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fstream);
                dbytes = br.ReadBytes((int)numBytes);
                strBinaryData = Encoding.Default.GetString(dbytes);
                fstream.Close();
                fstream.Dispose();
                br.Close();
                return strBinaryData;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
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
            catch (Exception)
            {            
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
