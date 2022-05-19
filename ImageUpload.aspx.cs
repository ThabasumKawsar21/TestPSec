
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Partial class Image upload
    /// </summary>
    public partial class ImageUpload : System.Web.UI.Page
    {
        #region Page Load
        
        /// <summary>
        /// The Image field
        /// </summary>        
        private string strImage = string.Empty;

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // btnSave.Attributes.Add("onclick", "javascript:CloseWindow();");

                ////strImage = Request.QueryString["strImage"];
                if (this.strImage != "Proof")
                {
                    this.strImage = "Photo";
                }

                if (!Page.IsPostBack)
                {
                    this.errortbl.Visible = false;
#pragma warning disable CS0618 // 'ConfigurationSettings.AppSettings' is obsolete: 'This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.AppSettings'
                    string strFileTypes = System.Configuration.ConfigurationSettings.AppSettings["SupportedFileTypes"].Trim().ToString();
#pragma warning restore CS0618 // 'ConfigurationSettings.AppSettings' is obsolete: 'This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.AppSettings'
                    string strSupportedtypes = strFileTypes.Replace(',', '/');
                    //// lblImage.Text = string.Concat(ALIVEConstants.PLEASESELECT, strSupportedtypes, " ", ALIVEConstants.FILETYPES);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        #endregion

        /// <summary>
        /// The Image Cancel Upload_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgCancelUpload_Click(object sender, ImageClickEventArgs e)
        {
        }

        #region Control Methods

        /// <summary>
        /// Method to Save Image file into database  
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string strFileName = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(this.ImgUpload.FileName))
                {
                    HttpPostedFile filetoUpload = this.ImgUpload.PostedFile;
                    strFileName = this.UploadFiletoServer();

                    bool imagetype = this.CheckFileExtension(strFileName);

                    if (imagetype == false)
                    {
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = "Please Select valid image file.";
                        return;
                    }
                    else
                    {
                        if (filetoUpload.ContentLength > 200000)
                        {
                            this.errortbl.Visible = true;
                            this.lblMessage.Text = "Image size cannot be greater than 200 KB";
                            return;
                        }
                        else
                        {
                            this.errortbl.Visible = false;
                        }
                    }
                }
                else
                {
                    this.errortbl.Visible = false;
                }

                if (string.IsNullOrEmpty(this.ImgUpload.FileName))
                {
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = "Please select an image file to upload.";
                    return;
                }

                byte[] strImgContent = this.ReadFile(strFileName);
                if (this.strImage == "Photo")
                {
                    this.Session["VisitorImgByte"] = strImgContent;
                    this.Session["Webcamimage"] = null;
                }
                else if (this.strImage == "Proof")
                {
                    this.Session["ProofImgByte"] = strImgContent;
                }

                this.DeleteFile(strFileName);

                ClientScript.RegisterClientScriptBlock(
                    this.GetType(),
                  "script",
                  "<script language='javascript'>window.parent.document.forms[0].submit();window.returnValue = true;window.close()</script>");
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to clear values while clicking on clear button        
        /// </summary>
        #endregion
        #region Private Methods

        /// <summary>
        /// Method to Copy File to server
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
                    this.errortbl.Visible = true;
                }
                else
                {
                    string serverPath = Server.MapPath(@"AssociateImage/");
                    destinationPath = string.Concat(
                        serverPath, 
                        "Associate", 
                        "-", 
                        DateTime.Now.ToString("dd-MM-yy"), 
                        " ", 
                        DateTime.Now.ToString("hh-mm-ss-fff"), 
                        fileExtension);
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
        /// Method to validate the file extension
        /// input string file path
        /// </summary>
        /// <param name="strfilePath">The file path parameter</param>
        /// <returns>The string type object</returns>     
        private bool CheckFileExtension(string strfilePath)
        {
            bool imageStatus = false;
#pragma warning disable CS0618 // 'ConfigurationSettings.AppSettings' is obsolete: 'This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.AppSettings'
            string strypes = System.Configuration.ConfigurationSettings.AppSettings["SupportedFileTypes"].Trim().ToString();
#pragma warning restore CS0618 // 'ConfigurationSettings.AppSettings' is obsolete: 'This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.AppSettings'
            string[] splittedFile = { "," };
            string[] strSplittedSupportedTypes = strypes.Split(splittedFile, StringSplitOptions.None);
            string[] splittedFilePath = { "." };
            string[] strSplittedTypes = strfilePath.Split(splittedFilePath, StringSplitOptions.None);
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

        /// <summary>
        /// The Read File method
        /// </summary>
        /// <param name="strImagePath">The Image Path parameter</param>
        /// <returns>The byte[] type object</returns>        
        private byte[] ReadFile(string strImagePath)
        {
            try
            {
                GenericFileUpload fu = new GenericFileUpload();
                FileInfo fileinfo = new FileInfo(strImagePath);
                string strBinaryData = string.Empty;
                byte[] dbytes = null;
                long numBytes = fileinfo.Length;
                FileStream fstream = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fstream);
                dbytes = br.ReadBytes((int)numBytes);
                dbytes = fu.SaveOptimizedImage(dbytes); // Optimize image
                this.Session["UploadImage"] = dbytes;
                fstream.Close();
                fstream.Dispose();
                br.Close();
                return dbytes;
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
////Ended by Sangeetha
