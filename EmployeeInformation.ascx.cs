
namespace VMSDev.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// The Employee Information partial class
    /// </summary>
    public partial class EmployeeInformation : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets UserID field
        /// </summary>       
 [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Justification = "Reviewed")]
        public string UserID
        {
            get
            {
                return this.UserID;
            }

            set
            {
                this.UserID = string.Empty;
            }
        }
        
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            if (objEmployeeDetails.ValidateAssociateDetails(this.UserID))
            {
                DataRow dremployeeInfo = objEmployeeDetails.GetEmployeeDetails(this.UserID);
                if (dremployeeInfo != null)
                {
                    this.PopulateAssociateData(dremployeeInfo);
                }
            }
        }
        
        /// <summary>
        /// The PopulateAssociateData method
        /// </summary>
        /// <param name="associateRow">The AssociateRow parameter</param>        
        private void PopulateAssociateData(DataRow associateRow)
        {
            try
            {
                System.Text.RegularExpressions.Regex nonNumericCharacters = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                this.lblEmpID.Text = XSS.HtmlEncode(associateRow["Associate_id"].ToString());
                this.ViewState["AssociateID"] = this.lblEmpID.Text.ToString();
                this.lblEmpName.Text = XSS.HtmlEncode(associateRow["AssociateName"].ToString());
                this.lblEmpEmailID.Text = XSS.HtmlEncode(associateRow["EmailID"].ToString());
                this.lblEmpLocation.Text = XSS.HtmlEncode(associateRow["Location"].ToString());
                this.lblEmpCity.Text = XSS.HtmlEncode(associateRow["City"].ToString());
                this.lblEmployeeMobile.Text = XSS.HtmlEncode(nonNumericCharacters.Replace(associateRow["Mobile"].ToString(), string.Empty));
                this.lblEmployeeExtension.Text = XSS.HtmlEncode(associateRow["Vnet"].ToString());
                this.lblMgrID.Text = XSS.HtmlEncode(associateRow["ManagerID"].ToString());
                this.lblMgrName.Text = XSS.HtmlEncode(associateRow["ManagerName"].ToString());
                this.lblMgrEmailID.Text = XSS.HtmlEncode(associateRow["ManagerEmailID"].ToString());
                this.lblManagerMobileNo.Text = XSS.HtmlEncode(nonNumericCharacters.Replace(associateRow["ManagerMobile"].ToString(), string.Empty));
                this.lblManagerExtension.Text = XSS.HtmlEncode(associateRow["ManagerVnet"].ToString());

                if (string.IsNullOrEmpty(associateRow["AssociateImage"].ToString()))
                {
                    this.ImgAssociate.ImageUrl = VMSConstants.VMSConstants.IMAGEPATH;
                }
                else
                {
                    this.ImgAssociate.Visible = true;
                    string encryptedData = VMSBusinessLayer.Encrypt(Session["Loginid"].ToString());
                    string strWidth = ConfigurationManager.AppSettings["ThumbnailW"].ToString();
                    string strHeight = ConfigurationManager.AppSettings["ThumbnailH"].ToString();
                    this.ImgAssociate.ImageUrl = string.Concat("~/AssociateImage.aspx?ID=", encryptedData, "&w=", strWidth, "&h=", strHeight);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
