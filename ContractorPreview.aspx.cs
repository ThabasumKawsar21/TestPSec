

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessEntity;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Contractor preview
    /// </summary>
    public partial class ContractorPreview : System.Web.UI.Page
    {
        /// <summary>
        /// To display contractor badge details for print
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string id = XSS.HtmlEncode(Request.QueryString["key"].ToString());
                string facilityCode = string.Empty;
                VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL objUserDetailsBL;
                string strLoginID = Convert.ToString(HttpContext.Current.Session["LoginId"]);
                string strDisplayName = string.Empty;
                string contractorID = string.Empty;
#pragma warning disable CS0219 // The variable 'lstContractInfo' is assigned but its value is never used
                IList<ContractDetails> lstContractInfo = null;
#pragma warning restore CS0219 // The variable 'lstContractInfo' is assigned but its value is never used

                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL contractDetails =
                    new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                objUserDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
                DataTable dtlocationDetails = contractDetails.GetIDCardLocationDetails(strLoginID);

                DataTable dtcontractorDetail = contractDetails.PreviewContractorDetails(id);

                if (dtcontractorDetail.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtcontractorDetail.Rows)
                    {
                        ContractDetails contractdetail = new ContractDetails();

                        contractdetail.ContractorId = Convert.ToString(dr["ContractorId"]);
                        this.lblContratorId.Text = XSS.HtmlEncode(Convert.ToString(dr["ContractorNumber"]));

                        contractdetail.ContractorName = Convert.ToString(dr["Name"]);
                        this.lblContratcorName.Text = XSS.HtmlEncode(Convert.ToString(dr["Name"]).ToUpper());
                        contractdetail.VendorName = Convert.ToString(dr["VendorName"]);
                        this.lblVendorName.Text = XSS.HtmlEncode(Convert.ToString(dr["VendorName"]).ToUpper());
                        this.lblVendorName1.Text = XSS.HtmlEncode(Convert.ToString(dr["VendorName"]).ToUpper());
                        contractdetail.SupervisiorMobile = Convert.ToString(dr["SuperVisiorPhone"]);
                        this.lblSupervisiorCell.Text = XSS.HtmlEncode(Convert.ToString(dr["SuperVisiorPhone"]));
                        contractdetail.VendorPhoneNumber = Convert.ToString(dr["VendorPhoneNumber"]);
                        this.lblServiceProviderCell.Text = XSS.HtmlEncode(Convert.ToString(dr["VendorPhoneNumber"]));
                        contractdetail.ContractorNumber = Convert.ToString(dr["ContractorNumber"]);

                        if (dtlocationDetails.Rows.Count > 0)
                        {
                            contractdetail.FacilityAddress = Convert.ToString(dtlocationDetails.Rows[0]["LocationAddress"]);
                            this.lblFacilityAddress.Text = XSS.HtmlEncode(Convert.ToString(dtlocationDetails.Rows[0]["LocationAddress"]));
                            facilityCode = Convert.ToString(dtlocationDetails.Rows[0]["FacilityCode"]);
                        }

                        contractdetail.CardValidlity = DateTime.Now.AddMonths(6).ToShortDateString();
                        contractdetail.CardValid = DateTime.Now.AddMonths(6).ToString("MMyy");
                        this.lblValidUpTo.Text = DateTime.Now.AddMonths(6).ToShortDateString();
                        this.lblValid.Text = DateTime.Now.AddMonths(6).ToString("MM/yy");

                        string removedSpecialCharsCWRname = Regex.Replace(contractdetail.ContractorName, "[^a-zA-Z]+", string.Empty).Replace(".", string.Empty);
                        string removedSpecialCharVendorName = Regex.Replace(contractdetail.VendorName, "[^a-zA-Z]+", string.Empty, RegexOptions.Compiled).Replace(".", string.Empty);

                        var contrID = string.Empty;
                        if (contractdetail.ContractorId.ToString().Length <= 4)
                        {
                            contrID = contractdetail.ContractorId.ToString().PadLeft(4, '0');
                        }
                        else
                        {
                            contrID = contractdetail.ContractorId.ToString().Substring(0, 4).PadLeft(4, '0');
                        }

                        facilityCode = string.IsNullOrEmpty(facilityCode) ? string.Empty : facilityCode.Substring(4, 3).ToUpper();
                        contractorID = facilityCode + removedSpecialCharsCWRname.Substring(0, 2).ToUpper() + removedSpecialCharVendorName.Substring(0, 2).ToUpper() + contrID;
                        this.lblCWRID.Text = XSS.HtmlEncode(contractorID);
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
