
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessEntity;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// The contractor preview landscape partial class
    /// </summary>
    public partial class ContractorProviewLandscape : System.Web.UI.Page
    {
        /// <summary>
        /// The Page Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string id = XSS.HtmlEncode(Request.QueryString["key"].ToString());

                //// string Id = "1136"; 

                VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL objUserDetailsBL;
                string strLoginID = Convert.ToString(HttpContext.Current.Session["LoginId"]);
                string strDisplayName = string.Empty;
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
                        this.lblContratcorName.Text = XSS.HtmlEncode(Convert.ToString(dr["Name"]));
                        contractdetail.VendorName = Convert.ToString(dr["VendorName"]);
                        this.lblVendorName.Text = XSS.HtmlEncode(Convert.ToString(dr["VendorName"]));
                        this.lblVendorName1.Text = XSS.HtmlEncode(Convert.ToString(dr["VendorName"]));
                        contractdetail.SupervisiorMobile = Convert.ToString(dr["SuperVisiorPhone"]);
                        this.lblSupervisiorCell.Text = XSS.HtmlEncode(Convert.ToString(dr["SuperVisiorPhone"]));
                        contractdetail.VendorPhoneNumber = Convert.ToString(dr["VendorPhoneNumber"]);
                        this.lblServiceProviderCell.Text = XSS.HtmlEncode(Convert.ToString(dr["VendorPhoneNumber"]));
                        contractdetail.ContractorNumber = Convert.ToString(dr["ContractorNumber"]);

                        if (dtlocationDetails.Rows.Count > 0)
                        {
                            contractdetail.FacilityAddress = Convert.ToString(dtlocationDetails.Rows[0]["LocationAddress"]);
                            this.lblFacilityAddress.Text = XSS.HtmlEncode(Convert.ToString(dtlocationDetails.Rows[0]["LocationAddress"]));
                        }

                        contractdetail.CardValidlity = DateTime.Now.AddMonths(6).ToShortDateString();
                        contractdetail.CardValid = DateTime.Now.AddMonths(6).ToString("MMyy");
                        this.lblValidUpTo.Text = DateTime.Now.AddMonths(6).ToShortDateString();
                        this.lblValid.Text = DateTime.Now.AddMonths(6).ToString("MM/yy");
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
