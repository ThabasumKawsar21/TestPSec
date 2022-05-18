
namespace VMSDev.UserControls
{
    using System;
    using System.Collections;
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
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using VMSBL;
    using VMSBusinessEntity;
    using VMSBusinessLayer;
    using VMSDev.DocUploadServiceRef;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class Send Message To The Page Handler
    /// </summary>   
    /// <param name="messageToThePage">message To The Page</param>
    public delegate void SendMessageToThePageHandler(string messageToThePage);

    /// <summary>
    /// Partial class View History by Host
    /// </summary>   
    public partial class VisitorGeneralInformation : System.Web.UI.UserControl
    {
        #region private variables
        
        /// <summary>
        /// The Purpose Data field
        /// </summary>        
        private Hashtable htpurposeData = new Hashtable();

        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
        
        /// <summary>
        /// The Countries field
        /// </summary>        
        private List<string> countries = new List<string>();
        
        /// <summary>
        /// The LocationDetails field
        /// </summary>        
        private VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
        
        /// <summary>
        /// The genTimeZone field
        /// </summary>        
        private GenericTimeZone genTimeZone = new GenericTimeZone();
        
        /// <summary>
        /// The File upload Response field
        /// </summary>        
        private MFileuploadResponse objMFileuploadResponse = new MFileuploadResponse();
        
        /// <summary>
        /// The File Upload Details Request field
        /// </summary>        
        private FileUploadDetailsRequest objFileUploadDetailsRequest = new FileUploadDetailsRequest();
        
        /// <summary>
        /// The sendMessageToThePage event
        /// </summary>        
        public event SendMessageToThePageHandler SendMessageToThePage;
        #endregion
      
        /// <summary>
        /// The GetUserDetailsByUserID method
        /// </summary>
        /// <param name="userID">The UserID parameter</param>
        /// <returns>The string type object</returns>        
        public string GetUserDetailsByUserID(string userID)
        {
            VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
            VMSBusinessLayer.UserDetails<string, string, string, string> userDetails = new VMSBusinessLayer.UserDetails<string, string, string, string>();
            userDetails = userDetailsBL.GetUserDetails(userID);
            return userDetails.AssociateName + "(" + userID + ")";
        }

        /// <summary>
        /// Populate  ID Proof
        /// </summary>
        public void PopulateIDProofs()
        {
            DataTable dtpurpose = new DataTable();
            List<string> purposeDataType = new List<string>();
            string[] purposeListArray;
            try
            {
                VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.MasterDataBL();
                purposeDataType.Clear();
                dtpurpose.Columns.Add("Id");
                dtpurpose.Columns.Add("Name");

                ////Get Master Data to Populate IDProof 
                purposeDataType = masterDataBL.GetMasterData("IDProof");
                for (int i = 0; i <= purposeDataType.Count - 1; i++)
                {
                    purposeListArray = purposeDataType[i].ToString().Split('|');
                    DataRow drpurpose = dtpurpose.NewRow();
                    drpurpose["Id"] = purposeListArray[1].ToString();
                    drpurpose["Name"] = purposeListArray[0].ToString();
                    dtpurpose.Rows.Add(drpurpose);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The InsertPhoto method
        /// </summary>
        /// <returns>The VMSBusinessEntity.VisitorProof type object</returns>        
        public VisitorProof InsertPhoto()
        {
            GenericFileUpload fu = new GenericFileUpload();
            VMSBusinessEntity.VisitorProof visitorProof = new VisitorProof();
            if (this.Session["VisitorImgByte"] != null || this.Session["Webcamimage"] != null)
            {
                if (this.Session["Webcamimage"] != null)
                {
                    byte[] img = (byte[])Session["Webcamimage"];
                    string fileContentID = this.SaveImageIntoSAN(this.txtFirstName.Text.Trim() + "_" + DateTime.Now.Ticks, img, Session["LoginID"].ToString());
                    visitorProof.Photo = string.Empty;
                    visitorProof.FileContentId = fileContentID;
                }
                else if (this.Session["VisitorImgByte"] != null && this.Session["UploadImage"] != null)
                {
                    byte[] img = (byte[])Session["UploadImage"];

                    ////Save image into SAN
                    string fileContentID = this.SaveImageIntoSAN(this.txtFirstName.Text.Trim() + "_" + DateTime.Now.Ticks, img, Session["LoginID"].ToString());

                    visitorProof.Photo = string.Empty;
                    visitorProof.FileContentId = fileContentID;
                }
            }

            return visitorProof;
        }

        /// <summary>
        /// The InsertGeneralInformation method
        /// </summary>
        /// <returns>The VMSBusinessEntity.VisitorMaster type object</returns>        
        public VisitorMaster InsertGeneralInformation()
        {
            VMSBusinessEntity.VisitorMaster visitorMaster = new VisitorMaster();

            visitorMaster.Gender = this.ddlGender.SelectedValue.Trim();
            visitorMaster.FirstName = this.txtFirstName.Text.Trim();
            visitorMaster.LastName = this.txtLastName.Text.Trim();
            visitorMaster.Company = this.txtCompany.Text.Trim();
            visitorMaster.Designation = this.txtDesignation.Text.Trim();
            visitorMaster.MobileNo = this.txtCountryCode.Text.Trim() + "-" + this.txtMobileNo.Text.Trim();
            visitorMaster.EmailID = this.txtEmail.Text.Trim();
            visitorMaster.Country = this.ddlNativeCountry.SelectedItem.Value;
            visitorMaster.CreatedBy = Session["LoginID"].ToString();
            visitorMaster.CreatedDate = this.genTimeZone.GetCurrentDate();
            ////  VMSUtility.VMSUtility.WriteLog("Created Date" + VisitorMaster.CreatedDate, VMSUtility.VMSUtility.LogLevel.Normal);
            visitorMaster.IsConfidential = this.chkIsCofidential.Checked;
            if (this.Session["RefID"] != null)
            {
                visitorMaster.VisitorReferenceNo = this.Session["RefID"].ToString();
            }

            return visitorMaster;
        }

        /// <summary>
        /// Get Parameters 
        /// </summary>
        /// <returns>Visitor Master</returns>
        public VisitorMaster GetParameters()
        {
            VMSBusinessEntity.VisitorMaster visitorMaster = new VisitorMaster();
            visitorMaster.Gender = this.ddlGender.SelectedValue.Trim();
            visitorMaster.FirstName = this.txtFirstName.Text.Trim();
            visitorMaster.LastName = this.txtLastName.Text.Trim();
            visitorMaster.Company = this.txtCompany.Text.Trim();
            visitorMaster.Designation = this.txtDesignation.Text.Trim();
            visitorMaster.MobileNo = this.txtMobileNo.Text.Trim();
            visitorMaster.EmailID = this.txtEmail.Text.Trim();
            return visitorMaster;
        }

        /// <summary>
        /// The ResetGeneralInformation method
        /// </summary>
        /// <param name="multipleEntry">The IsMultipleEntry parameter</param>        
        public void ResetGeneralInformation(bool multipleEntry)
        {
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtMobileNo.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtDesignation.Text = string.Empty;
            //// for single entry updated for CR IRVMS22062010CR07
            if (multipleEntry == false)
            {
                this.txtCompany.Text = string.Empty;
                this.ddlNativeCountry.SelectedIndex = this.ddlNativeCountry.Items.IndexOf(this.ddlNativeCountry.Items.FindByText("India"));
                ////ddlTitle.SelectedIndex = ddlTitle.Items.IndexOf(ddlTitle.Items.FindByValue("Mr"));
                this.ddlGender.ClearSelection();
                this.ddlGender.Items.FindByValue("0").Selected = true;
            }
        }

        /// <summary>
        /// The ShowGeneralInformationByRequestID method
        /// </summary>
        /// <param name="propertiesDC">The propertiesDC parameter</param>        
        public void ShowGeneralInformationByRequestID(VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC)
        {
            try
            {
                if (!string.IsNullOrEmpty(propertiesDC.VisitorMasterProperty.Gender.Trim()))
                {
                    this.ddlGender.Items.FindByValue(propertiesDC.VisitorMasterProperty.Gender.Trim()).Selected = true;
                }

                this.txtFirstName.Text = propertiesDC.VisitorMasterProperty.FirstName;
                this.txtLastName.Text = propertiesDC.VisitorMasterProperty.LastName;
                if (propertiesDC.VisitorMasterProperty.Company == "NA")
                {
                    this.txtCompany.Text = string.Empty;
                }
                else
                {
                    this.txtCompany.Text = propertiesDC.VisitorMasterProperty.Company;
                }

                this.txtDesignation.Text = propertiesDC.VisitorMasterProperty.Designation;
                string strMobileNo = propertiesDC.VisitorMasterProperty.MobileNo;
                if (!string.IsNullOrEmpty(strMobileNo))
                {
                    string[] values = strMobileNo.Split('-');
                    this.txtCountryCode.Text = values[0];
                    this.txtMobileNo.Text = values[1];
                }

                this.txtEmail.Text = propertiesDC.VisitorMasterProperty.EmailID;

                if (!string.IsNullOrEmpty(propertiesDC.VisitorMasterProperty.Gender))
                {
                    if (propertiesDC.VisitorMasterProperty.Gender.Trim() == "0")
                    {
                        this.ddlGender.SelectedIndex = 0;
                    }
                    else if (propertiesDC.VisitorMasterProperty.Gender.Trim() == "M")
                    {
                        this.ddlGender.SelectedIndex = 1;
                    }
                    else
                    {
                        this.ddlGender.SelectedIndex = 2;
                    }
                }

                if (!string.IsNullOrEmpty(this.ddlNativeCountry.SelectedValue))
                {
                    if (propertiesDC.VisitorRequestProperty != null)
                    {
                        if (propertiesDC.VisitorRequestProperty.ExternalRequestCameFrom == "ClientVisit" && propertiesDC.VisitorMasterProperty.Country == "0")
                        {
                            this.ddlNativeCountry.SelectedIndex = 0;
                        }
                        else
                        {
                            this.ddlNativeCountry.SelectedIndex = this.ddlNativeCountry.Items.IndexOf(this.ddlNativeCountry.Items.FindByValue(propertiesDC.VisitorMasterProperty.Country));
                        }
                    }
                    else
                    {
                        this.ddlNativeCountry.SelectedIndex = this.ddlNativeCountry.Items.IndexOf(this.ddlNativeCountry.Items.FindByValue(propertiesDC.VisitorMasterProperty.Country));
                    }
                }

                if (propertiesDC.VisitorMasterProperty.VisitorReferenceNo != null)
                {
                    this.Session["RefID"] = propertiesDC.VisitorMasterProperty.VisitorReferenceNo.ToString();
                }

                if (!this.chkIsCofidential.Text.Equals(string.Empty))
                {
                    this.chkIsCofidential.Checked = (bool)propertiesDC.VisitorMasterProperty.IsConfidential;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The DisableVisitGeneralInformationControls method
        /// </summary>
        /// <param name="requestStatus">The RequestStatus parameter</param>        
        public void DisableVisitGeneralInformationControls(string requestStatus)
        {
            try
            {
                ////changed by priti on 3rd June for VMS CR VMS31052010CR6
                if ((requestStatus == "OUT") || (requestStatus == "IN") || (requestStatus == "CANCELLED") || (requestStatus == VMSConstants.VMSConstants.REPEATVISITOR))
                {
                    ////ddlTitle.Enabled = false;
                    this.ddlGender.Enabled = false;
                    this.txtFirstName.Enabled = false;
                    this.txtLastName.Enabled = false;
                    this.txtCompany.Enabled = false;
                    this.txtDesignation.Enabled = false;
                    this.txtMobileNo.Enabled = false;
                    this.txtEmail.Enabled = false;
                    this.ddlNativeCountry.Enabled = false;
                }
                else if (requestStatus == "YET TO ARRIVE")
                {
                    this.EnableVisitGeneralInformationControls();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Enable Visitor General Information Controls
        /// </summary>
        public void EnableVisitGeneralInformationControls()
        {
            // ddlTitle.Enabled = true;
            this.ddlGender.Enabled = true;
            this.txtFirstName.Enabled = true;
            this.txtLastName.Enabled = true;
            this.txtCompany.Enabled = true;
            this.txtDesignation.Enabled = true;
            this.txtMobileNo.Enabled = true;
            this.txtEmail.Enabled = true;
            this.ddlNativeCountry.Enabled = true;
        }

        /// <summary>
        /// Description: Method to Initialize Country, Cities
        /// </summary>
        public void InitCountries()
        {
            try
            {
                VMSBusinessLayer.RequestDetailsBL objrequestDetails = new VMSBusinessLayer.RequestDetailsBL();
                int countryId = 0;
                DataTable dtsecurityCity = new DataTable();
                if (this.Session["LoginID"] != null)
                {
                    string userID = Session["LoginID"].ToString();
                    dtsecurityCity = objrequestDetails.GetSecurityCity(userID).Tables[0];
                    countryId = Convert.ToInt32(dtsecurityCity.Rows[0]["CountryId"]);
                }

                DataTable dtcountry = objrequestDetails.GetNativeCountry(string.Empty);
                this.ddlNativeCountry.DataSource = dtcountry;
                this.ddlNativeCountry.DataTextField = "Country";
                this.ddlNativeCountry.DataValueField = "CountryId";
                this.ddlNativeCountry.DataBind();

                this.ddlNativeCountry.Items.Insert(0, new ListItem { Text = "Select", Value = "0" });
                if ((this.ddlNativeCountry.Items.Count == 0) || (this.ddlNativeCountry.Items.Count > 0))
                {
                    this.ddlNativeCountry.SelectedValue = Convert.ToString(countryId);
                }
                else
                {
                    this.ddlNativeCountry.SelectedIndex = 0;
                }

                var query = (from p in dtcountry.AsEnumerable()
                             where p.Field<int>("CountryId") == countryId
                             select p).FirstOrDefault();

                this.txtCountryCode.Text = string.Concat("+", Convert.ToString(query["Code"]));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
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
                if (this.Session["LoginID"] == null)
                {
                    return;
                }

                this.txtFirstName.Focus();

                if (!Page.IsPostBack)
                {
                    if (!Request.QueryString.ToString().Contains("RequestID="))
                    {                       
                        List<string> roles = (List<string>)Session["UserRole"];
                    }

                    this.HideControlsBasedOnRoles();
                    this.InitCountries();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The txtFirstName_LostFocus method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void TxtFirstName_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (this.SendMessageToThePage != null)
                {
                    this.SendMessageToThePage(this.txtFirstName.Text);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The NativeCountry_SelectedIndexChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlNativeCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            VMSBusinessLayer.RequestDetailsBL objrequestDetails = new VMSBusinessLayer.RequestDetailsBL();
            string countrycode;
            if (this.ddlNativeCountry.SelectedItem.Text == "Select")
            {
                countrycode = "91";
            }
            else
            {
                countrycode = objrequestDetails.GetCountryCode(XSS.HtmlEncode(this.ddlNativeCountry.SelectedItem.Text));
            }

            this.txtCountryCode.Text = string.Concat("+", countrycode.Trim());
        }

        /// <summary>
        /// The HideControlsBasedOnRoles method
        /// </summary>        
        private void HideControlsBasedOnRoles()
        {
            try
            {
                List<string> roles = (List<string>)Session["UserRole"];
                if (roles.Contains("Security") || roles.Contains("SuperAdmin"))
                {
                    this.divIDProof.Visible = true;
                }
                else
                {
                    this.divIDProof.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Decrypt Binary Data method
        /// </summary>
        /// <param name="strEncrpytedDataImg">The Data parameter</param>
        /// <returns>The string type object</returns>        
        private string DecryptBinaryData(string strEncrpytedDataImg)
        {
            return new EncryptDecrypt().Decrypt(strEncrpytedDataImg, "CTS", true);
        }

        /// <summary>
        /// The BinaryData method
        /// </summary>
        /// <param name="strBinaryDataImg">The BinaryData parameter</param>
        /// <returns>The string type object</returns>        
        private string EncrpytBinaryData(string strBinaryDataImg)
        {
            return new EncryptDecrypt().Encrypt(strBinaryDataImg, "CTS", true);
        }

        /// <summary>
        /// The SaveImageIntoSAN method
        /// </summary>
        /// <param name="fileName">The fileName parameter</param>
        /// <param name="imageBytes">The imageBytes parameter</param>
        /// <param name="createdBy">The createdBy parameter</param>
        /// <returns>The string type object</returns>        
        private string SaveImageIntoSAN(string fileName, byte[] imageBytes, string createdBy)
        {
            using (DocumentUploadServiceClient objDocumentUploadServiceClient = new DocumentUploadServiceClient())
            {
                this.objFileUploadDetailsRequest.AppId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["VMSappId"].ToString());
                this.objFileUploadDetailsRequest.AppTemplateId = System.Configuration.ConfigurationManager.AppSettings["VMSappTemplateId"].ToString();
                this.objFileUploadDetailsRequest.FileName = fileName + ".jpg";
                this.objFileUploadDetailsRequest.IncomingFile = imageBytes;
                this.objFileUploadDetailsRequest.CreatedBy = createdBy;
                this.objFileUploadDetailsRequest.CreatedDate = DateTime.Now;
                this.objFileUploadDetailsRequest.AssociateId = Convert.ToInt32(createdBy);
                this.objMFileuploadResponse = objDocumentUploadServiceClient.UploadFile_WithResponse(this.objFileUploadDetailsRequest);
                return this.objMFileuploadResponse.FileContentId.ToString();
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
                MFileuploadResponse objMFileuploadResponses = new MFileuploadResponse();
                FileUploadDetailsRequest objFileUploadDetailsRequests = new FileUploadDetailsRequest();
                using (DocumentUploadServiceClient objDocumentUploadServiceClient = new DocumentUploadServiceClient())
                {
                    // objFileUploadDC.FileUploadId = fileuploadId;
                    objFileUploadDC.FileContentId = new Guid(fileContentID);
                    objFileUploadDC.AppTemplateId = ConfigurationManager.AppSettings["VMSappTemplateId"];
                    objMFileuploadResponses = objDocumentUploadServiceClient.DownloadFile(objFileUploadDC);
                    associateImage = objMFileuploadResponses.OutgoingFile;

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
