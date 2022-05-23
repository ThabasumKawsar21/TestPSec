
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
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// temporary Id card partial class
    /// </summary>
    public partial class TemporaryIDCard : System.Web.UI.Page
    {
        /// <summary>
        /// The AssociateDetails field
        /// </summary>        
        private VMSBusinessLayer.EmployeeBL associateDetails = new VMSBusinessLayer.EmployeeBL();

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
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "TimeConversion", "TimeConversion();", true);
            VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL userDetails = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
            try
            {
                this.imgAssociate.Visible = true;
                string key = string.Empty;
                string tempId = string.Empty;
                string strDetailId;
                string templateId = string.Empty;
                NameValueCollection coll = Request.QueryString;
                //// Get names of all keys into a string array.
                string[] arr1 = coll.AllKeys;

                try
                {
                    key = Convert.ToString(coll.GetValues(0)[0]);
                    tempId = Convert.ToString(coll.GetValues(1)[0]);
                }
                catch (NullReferenceException)
                {
                }

                strDetailId = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(key));
                templateId = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(tempId));
                ////if (Session["CHireFileContentID"].ToString() != null)
                ////{
                ////    templateId = Session["CHireFileContentID"].ToString();
                ////}
                string loginID = string.Empty;

                DataSet dstempCardInfo = this.associateDetails.GetTempIDCardDetails(strDetailId);

                if (dstempCardInfo.Tables.Count >= 1)
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    this.lblName.Text = dstempCardInfo.Tables[0].Rows[0]["AssociateName"].ToString();
                    this.lblAssociateID.Text = dstempCardInfo.Tables[0].Rows[0]["AssociateID"].ToString();
                    ////lblFacilityName.Text = dsTempCardInfo.Tables[0].Rows[0]["City"].ToString() + "-" + dsTempCardInfo.Tables[0].Rows[0]["Facility"].ToString();
                    this.lblFacilityName.Text = dstempCardInfo.Tables[0].Rows[0]["Facility"].ToString();
                    //// lblID_barcode.Text = dsTempCardInfo.Tables[0].Rows[0]["AssociateID"].ToString();
                    this.imgBarCode.ImageUrl = string.Concat("~/Barcode.aspx?Code=*", dstempCardInfo.Tables[0].Rows[0]["AssociateID"].ToString().Trim(), "*");
                    this.lblPassNumber.Text = dstempCardInfo.Tables[0].Rows[0]["PassNumber"].ToString();
                    this.lblEmercengyContactNumber.Text = dstempCardInfo.Tables[0].Rows[0]["EmergencyContact"].ToString();
                    ////lblBloodGroup1.Text = UserDetails.GetBloodGroup(lblAssociateID.Text.Trim()).AssociateBloodGroup.ToString();
                    ////lblBloodGroup1.Text = Convert.ToString(dsTempCardInfo.Tables[0].Rows[0]["BloodGroup"]);    
                    ////dsTempCardInfo.Tables[0].Rows[0]["BloodGroup"].ToString();

                    this.lblBloodGroup1.Text = Convert.ToString(dstempCardInfo.Tables[0].Rows[0]["BloodGroup"]);
                    this.lblFacilityAddress.Text = dstempCardInfo.Tables[0].Rows[0]["OfficeAddress"].ToString();
                    this.lblArea.Text = dstempCardInfo.Tables[0].Rows[0]["City"].ToString() + " - " + dstempCardInfo.Tables[0].Rows[0]["PinCode"].ToString() + ", " + dstempCardInfo.Tables[0].Rows[0]["State"].ToString() + ", " + dstempCardInfo.Tables[0].Rows[0]["Country"].ToString();

                    ////lblValidfor.Text = string.Concat("VALID FOR ", dsTempCardInfo.Tables[0].Rows[0]["ValidTill"].ToString());                    
                    ////DateTime dtToDate =  DateTime.ParseExact(dsTempCardInfo.Tables[0].Rows[0]["ToDate"],"MM/dd/yyyy", CultureInfo.InvariantCulture);

                    DateTime dttoDate = DateTime.Parse(dstempCardInfo.Tables[0].Rows[0]["ToDate"].ToString(), provider);
                    DateTime dtfromDate = DateTime.Parse(dstempCardInfo.Tables[0].Rows[0]["FromDate"].ToString(), provider);
                    string strDateTime = dttoDate.ToString("MM/dd/yyyy");
                    string strFromDate = dtfromDate.ToString("MM/dd/yyyy");
                    string strFromTime = dtfromDate.ToString("HH:mm");
                    string strToTime = dttoDate.ToString("HH:mm");
                    this.hdnFromTime.Value = strFromTime;
                    this.hdnToTime.Value = strToTime;

                    ////modified by bincey om Oct 4 2012  strFromDate.Substring(0, 10); strDateTime.Substring(0, 10);

                    this.lblFrom.InnerText = dtfromDate.ToShortDateString();
                    this.lblTo.InnerText = dttoDate.ToShortDateString();
                    string strAssociateID = this.lblAssociateID.Text;
                    VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                    DataSet securitydetails = new DataSet();
                    string strLocation = string.Empty;
                    securitydetails = requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    if (securitydetails != null)
                    {
                        if (securitydetails.Tables[0].Rows.Count > 0)
                        {
                            strLocation = securitydetails.Tables[0].Rows[0]["LocationId"].ToString();
                        }
                    }

                    DataSet dtset = new DataSet();
                    VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objaccess = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                    dtset = objaccess.GetAccessCardDetails(strAssociateID, strLocation, strDetailId);
                    string msgboxdetails = dtset.Tables[1].Rows[0]["CardType"].ToString();

                    if (msgboxdetails == "1 Day Access Card")
                    {
                        string accessCardret = string.Concat(Resources.LocalizedText.AccessCardGenerated, " ", this.lblAssociateID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblFacilityName.Text, " ", Resources.LocalizedText.Location);
                        string strScript = string.Concat("<script>alert('", accessCardret, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    }

                   
                        string encryptedData = VMSBusinessLayer.VMSBusinessLayer.Encrypt(this.lblAssociateID.Text.Trim());
                        string strWidth = ConfigurationManager.AppSettings["ThumbnailW"].ToString();
                        string strHeight = ConfigurationManager.AppSettings["ThumbnailH"].ToString();
                    ////this.imgAssociate.ImageUrl = XSS.HtmlEncode(string.Concat("GetImage1.ashx?IDCARD=", XSS.HtmlEncode(this.lblAssociateID.Text), "&w=", strWidth, "&h=", strHeight, "&TempId=", templateId));
                    this.hdnFileContent.Value = templateId;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "GetImageFromIDCard", "GetImageFromIDCard();", true);

                    if (Convert.ToInt16(dstempCardInfo.Tables[0].Rows[0]["IsContractor"].ToString()) == 0)
                    {
                        this.imgAssociateFlag.Src = "~/Images/IsAssociate.png";
                        this.imgAssociateFlag2.Src = "~/Images/IsAssociate.png";
                    }
                    else if (Convert.ToInt16(dstempCardInfo.Tables[0].Rows[0]["IsContractor"].ToString()) == 1)
                    {
                        this.imgAssociateFlag.Src = "~/Images/IsContractor.png";
                        this.imgAssociateFlag2.Src = "~/Images/IsContractor.png";
                    }

                    if (msgboxdetails == "1 Day ID Card")
                    {
                        string idcardret = string.Concat(Resources.LocalizedText.TempCardGenerated, " ", this.lblAssociateID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblFacilityName.Text, " ", Resources.LocalizedText.Location);
                        string strScript = string.Concat("<script>alert('", idcardret, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    }
                    else if (msgboxdetails == "1 day ID Card and Access Card")
                    {
                        string bothret = string.Concat(Resources.LocalizedText.IDCardandAccessCardGenerated, " ", this.lblAssociateID.Text.ToString(), " ", Resources.LocalizedText.Under, " ", this.lblFacilityName.Text, " ", Resources.LocalizedText.Location);
                        string strScript = string.Concat("<script>alert('", bothret, "'); </script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", strScript);
                    }
                    //// ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", "<script>alert('"+Resources.LocalizedText.TempCardGenerated+"'); </script>");
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "LoadTempIDCardpage", "<script>alert('" + Resources.LocalizedText.ErrorMessage + "'); window.close();</script>");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);                
            }
        }
    }
}
