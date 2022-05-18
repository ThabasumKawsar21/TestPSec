
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessEntity;

    /// <summary>
    /// Contractor ID Card partial class
    /// </summary>
    public partial class ContractorIdCard : System.Web.UI.Page
    {
        #region Variables       
        
        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
        
        /// <summary>
        /// The  field
        /// </summary>        
        #endregion

        /// <summary>
        /// Grid download and print operations
        /// </summary>
        /// <param name="sname">name parameter</param>
        /// <returns>list of string</returns>        
        [WebMethod]
        public static List<string> GetVendorName(string sname)
        {
            List<string> strNamesData = new List<string>();
           
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL =
                    new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();

                strNamesData = objRequestDetailsBL.GetVendorName(sname);

            ////var namelist = strNamesData.Where(n => n.ToLower().StartsWith(sname.ToLower()));
            return strNamesData.ToList();
        }

        /// <summary>
        /// Save Contractor details
        /// </summary>
        /// <param name="contractorId">Contractor ID</param>
        /// <param name="contractorName">Contractor Name</param>
        /// <param name="vendorName">Vendor Name</param>
        /// <param name="docStatus">Document Status</param>
        /// <param name="status">Status parameter</param>
        /// <param name="supervisiorPhone">Supervisor Phone</param>
        /// <param name="vendorPhone">Vendor Phone</param>
        /// <returns>integer value</returns>
        [WebMethod]
        public static int SaveRequest(
        string contractorId,
        string contractorName,
            string vendorName,
            string docStatus,
            string status,
            string supervisiorPhone,
            string vendorPhone)
        {
            string strLocationId = string.Empty;
            VMSDataLayer.VMSDataLayer.MasterDataDL objMasterDataDL =
                new VMSDataLayer.VMSDataLayer.MasterDataDL();
            VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails =
               new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
            DataSet dslocation = requestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
            DataTable dtlocation = dslocation.Tables[0];
            if (dtlocation.Rows.Count > 0)
            {
                strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);
                HttpContext.Current.Session["LocationId"] = strLocationId;
            }

            if (!string.IsNullOrEmpty(contractorId))
            {
                if (!requestDetails.CheckContratorNumberExist(contractorId, strLocationId))
                {
                    return objMasterDataDL.SaveContractorIDDetails(
                    contractorName,
                    contractorId,
                    vendorName,
                        status,
                        supervisiorPhone,
                        vendorPhone,
                        docStatus,
                        Convert.ToString(HttpContext.Current.Session["LoginId"]),
                        DateTime.Now,
                        strLocationId);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return objMasterDataDL.SaveContractorIDDetails(
                contractorName,
                contractorId,
                vendorName,
                       status,
                       supervisiorPhone,
                       vendorPhone,
                       docStatus,
                       Convert.ToString(HttpContext.Current.Session["LoginId"]),
                       DateTime.Now,
                       strLocationId);
            }
        }

        /// <summary>
        /// Save Last Print Date when print Badge
        /// </summary>
        /// <param name="id">Contractor ID</param>
        /// <returns>integer value</returns>
        [WebMethod]
        public static int SavePrintDeatils(int id)
        {
            VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails =
                new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
            string hostId = Convert.ToString(HttpContext.Current.Session["LoginID"]);
            DataSet locdetails = requestDetails.GetSecurityCity(hostId);
            string locationId = string.Empty;
            if (locdetails != null)
            {
                if (locdetails.Tables[0].Rows.Count > 0)
                {
                    locationId = locdetails.Tables[0].Rows[0]["LocationId"].ToString();
                }
            }

            return requestDetails.SaveContractorPrintDetails(id, hostId, locationId);
        }

        /// <summary>
        /// Get Contractor details.
        /// </summary>
        /// <param name="id">Contractor Id</param>
        /// <returns>list of contractor details</returns>
        [WebMethod]
        public static IList<ContractDetails> GetContractorDetails(string id)
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL objUserDetailsBL;
                string strLoginID = Convert.ToString(HttpContext.Current.Session["LoginId"]);
                string strDisplayName = string.Empty;
                IList<ContractDetails> lstContractInfo = null;
                string facilityCode = string.Empty;
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL contractDetails =
                    new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                objUserDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();

                DataTable dtlocationDetails = contractDetails.GetIDCardLocationDetails(strLoginID);

                DataTable dtcontractorDetail = contractDetails.PreviewContractorDetails(id);

                if (dtcontractorDetail.Rows.Count > 0)
                {
                    lstContractInfo = new List<ContractDetails>();

                    foreach (DataRow dr in dtcontractorDetail.Rows)
                    {
                        ContractDetails contractdetail = new ContractDetails();

                        contractdetail.ContractorId = Convert.ToString(dr["ContractorId"]);

                        contractdetail.ContractorName = Convert.ToString(dr["Name"]).ToUpper();
                        contractdetail.VendorName = Convert.ToString(dr["VendorName"]).ToUpper();
                        contractdetail.SupervisiorMobile = Convert.ToString(dr["SuperVisiorPhone"]);
                        contractdetail.VendorPhoneNumber = Convert.ToString(dr["VendorPhoneNumber"]);
                        contractdetail.ContractorNumber = Convert.ToString(dr["ContractorNumber"]);
                        if (dtlocationDetails.Rows.Count > 0)
                        {
                            contractdetail.FacilityAddress = Convert.ToString(dtlocationDetails.Rows[0]["LocationAddress"]);
                            facilityCode = Convert.ToString(dtlocationDetails.Rows[0]["FacilityCode"]);
                        }

                        contractdetail.CardValidlity = DateTime.Now.AddMonths(6).ToShortDateString();
                        contractdetail.CardValid = DateTime.Now.AddMonths(6).ToString("MM/yy");

                        var contrID = string.Empty;
                        if (contractdetail.ContractorId.ToString().Length <= 4)
                        {
                            contrID = contractdetail.ContractorId.ToString().PadLeft(4, '0');
                        }
                        else
                        {
                            contrID = contractdetail.ContractorId.ToString().Substring(0, 4).PadLeft(4, '0');
                        }

                        string removedSpecialCharsCWRname = Regex.Replace(contractdetail.ContractorName, "[^a-zA-Z]+", string.Empty).Replace(".", string.Empty);
                        string removedSpecialCharVendorName = Regex.Replace(contractdetail.VendorName, "[^a-zA-Z]+", string.Empty, RegexOptions.Compiled).Replace(".", string.Empty);
                        facilityCode = string.IsNullOrEmpty(facilityCode) ? string.Empty : facilityCode.Substring(4, 3).ToUpper();
                        contractdetail.ContractorIdentityNo = facilityCode + removedSpecialCharsCWRname.Substring(0, 2).ToUpper() + removedSpecialCharVendorName.Substring(0, 2).ToUpper() + contrID;

                        lstContractInfo.Add(contractdetail);
                    }
                }

                return lstContractInfo;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// To Get last print Badge information
        /// </summary>
        /// <param name="id">Contractor Id</param>
        /// <returns>print status details</returns>
        [WebMethod]
        public static PrintStatusDetails GetLastPrintStatus(int id)
        {
            PrintStatusDetails printstatus = new PrintStatusDetails();
            VMSBusinessLayer.VMSBusinessLayer.MasterDataBL objRequestDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
            printstatus = objRequestDetailsBL.GetLastPrintStatus(id);
            return printstatus;
        }

        /// <summary>
        /// to print status details
        /// </summary>
        /// <param name="requestId">request id</param>
        /// <returns>print status details</returns>
        public static PrintStatusDetails GetVisitStatusByRequestId(string requestId)
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.MasterDataBL objMasterBL =
                    new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
                ////return 
                return objMasterBL.GetVisitStatusByRequestId(Convert.ToInt32(requestId));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// The test method
        /// </summary>        
        [WebMethod]
        [ScriptMethod]
        public static void Test()
        {
#pragma warning disable CS0168 // The variable 'test' is declared but never used
            string test;
#pragma warning restore CS0168 // The variable 'test' is declared but never used
        }

        /// <summary>
        /// To enable paging for badge preview.
        /// </summary>
        public void BuildPagers()
        {
            if (Convert.ToInt32(this.TotalSize.Value) >= Convert.ToInt32(this.PageSize.Value))
            {
                ////Check if its possible to have the previous page
                if ((int.Parse(this.CurrentPage.Value) - 1) <= 0)
                {
                    this.imgPrev.Visible = false;
                    ////   lbtnPrev.Visible = false;
                }
                else
                {
                    ////  lbtnPrev.Visible = true;
                    this.imgPrev.Visible = true;
                }

                if ((int.Parse(this.CurrentPage.Value) * int.Parse(this.PageSize.Value)) >= int.Parse(this.TotalSize.Value))
                {
                    this.imgNext.Visible = false;
                    ////   lbtnNext.Visible = false;
                }
                else
                {
                    ////    lbtnNext.Visible = true;
                    this.imgNext.Visible = true;
                }
            }
            else
            {
                this.imgPrev.Visible = false;
                this.imgNext.Visible = false;
            }
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "HideReset", "HideReset();", true);

                this.DisableSession();
                this.SetPermission();
                this.lbtnDelete.Visible = false;
            }
            else
            {
                this.DisableSession();
            }
        }

        /// <summary>
        /// The RowCommand method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdRowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        /// <summary>
        /// Method to rebind the rebind the grid view once the user select paging option
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdIDCardList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdIDCardList.PageIndex = e.NewPageIndex;
                this.BtnSearch_Click(sender, e);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Bulk upload model pop up will be displayed.
        /// </summary>
        /// <param name="source">source parameter</param>
        /// <param name="e">event e</param>
        protected void LbtnBulkUpload_Click(object source, EventArgs e)
        {
            this.ModalPopupExtender1.Show();
        }

        /// <summary>
        /// Search by Contractor ID 
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event e</param>
        protected void BtnSearachbyId_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(this.hdnContractorSearchId.Value);
            DataTable dttable = this.BindGridById(id);
            dttable = dttable.Select("Status='Active'").Count() > 0 ? dttable.Select("Status='Active'").CopyToDataTable() : new DataTable();
            if (dttable.Rows.Count > 0)
            {
                this.lblDetails.Text = "Contractor Details";
                this.lblDetails.Visible = true;
                this.grdIDCardList.DataSource = dttable;
                this.Session["DataForEdit"] = dttable;
                this.grdIDCardList.DataBind();
                this.grdIDCardList.Visible = true;
                this.lblError.Visible = false;
                this.grdIDCardList.PagerSettings.Visible = true;
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetReset", "SetReset();", true);
            }
            else
            {
                this.lblError.Text = "No Records Found!";
                this.lblError.Visible = true;
                this.grdIDCardList.Visible = false;
                this.lblDetails.Visible = false;
                //// lbtnReset.Attributes.Add("style", "display:none");
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "HideReset", "HideReset();", true);                     
            }
        }

        /// <summary>
        /// To search contractor details
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event e</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            this.Session["DataForEdit"] = null;
            this.grdIDCardList.EditIndex = -1;
            string strLocationId = string.Empty;
            this.SetPermission();

            DataSet dslocation = this.requestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
            DataTable dtlocation = dslocation.Tables[0];
            if (dtlocation.Rows.Count > 0)
            {
                strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);
            }

            if (this.txtSearch.Text.Trim().Length == 0)
            {
                this.grdIDCardList.Visible = this.lbtnDelete.Visible = false;
                this.lblError.Text = "Please enter search criteria";
                this.lblError.Visible = true;
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "HideReset", "HideReset();", true);
            }
            else
            {
                if (!string.IsNullOrEmpty(this.txtSearch.Text))
                {
                    DataTable dttable = this.BindGrid(this.txtSearch.Text, strLocationId);
                    dttable = dttable.Select("Status='Active'").Count() > 0 ? dttable.Select("Status='Active'").CopyToDataTable() : new DataTable();
                    if (dttable.Rows.Count > 0)
                    {
                        this.Session["DataForEdit"] = dttable;
                        this.lblDetails.Text = "Contractor Details";
                        this.lblDetails.Visible = true;
                        this.grdIDCardList.DataSource = dttable;
                        this.grdIDCardList.DataBind();
                        this.grdIDCardList.Visible = true;
                        this.lblError.Visible = false;

                        this.grdIDCardList.PagerSettings.Visible = true;
                        this.SetPermission();
                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetReset", "SetReset();", true);
                    }
                    else
                    {
                        this.lblError.Text = "No Records Found!";
                        this.lblError.Visible = true;
                        this.grdIDCardList.Visible = false;
                        this.lblDetails.Visible = false;
                        this.lbtnDelete.Visible = false;
                        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "HideReset", "HideReset();", true);
                    }
                }
                else
                {
                    this.grdIDCardList.Visible = false;
                    this.lblError.Text = "Please enter search criteria";
                    this.lblError.Visible = true;
                    Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "HideReset", "HideReset();", true);
                }
            }
        }

        /// <summary>
        /// Reset existing details
        /// </summary>
        /// <param name="source">source parameter</param>
        /// <param name="e">event e</param>
        protected void LbtnReset_Click(object source, EventArgs e)
        {
            this.grdIDCardList.DataSource = null;
            this.grdIDCardList.DataBind();
            this.grdIDCardList.Visible = false;
            this.lblDetails.Visible = false;
            this.lbtnDelete.Visible = false;
            this.lblError.Visible = false;
            this.txtSearch.Text = string.Empty;
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "HideReset", "HideReset();", true);
        }

        /// <summary>
        /// Contractor Grid check whether badge is already printed. if yes,image will be displayed
        /// in grid to get details of last print details.
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="args">event args</param>
        protected void GrdIDCardList_RowDataBound(object sender, GridViewRowEventArgs args)
        {
            foreach (GridViewRow grdRow in this.grdIDCardList.Rows)
            {
                Label lblLastPrintDate = (Label)grdRow.FindControl("lblLastPrintDate");
                Label lablStatus = (Label)grdRow.FindControl("lblStatus");
                ImageButton imgGetStatus = (ImageButton)grdRow.FindControl("ImgGetLastPrintedDate");          

                if (!string.IsNullOrEmpty(lblLastPrintDate.Text))
                {
                    imgGetStatus.Visible = true;
                }
                else
                {
                    imgGetStatus.Visible = false;
                }
            }
        }

        /// <summary>
        /// Next button event for Badge preview
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event e</param>
        protected void ImgNext_Click(object sender, ImageClickEventArgs e)
        {
            ////if (Session["BulkContractorId"] != null)
            ////{

            ////    //Check if we can display the next page.
            //    if ((int.Parse(CurrentPage.Value) * int.Parse(PageSize.Value)) < int.Parse(TotalSize.Value))
            //    {
            //        //Increment the CurrentPage value
            //        CurrentPage.Value = (int.Parse(CurrentPage.Value) + 1).ToString();
            //    }
            //    BuildPagers();

            ////    List<string> ContractorList = (List<string>)Session["BulkContractorId"];

            ////    //objBulkId = (AssociateBulkIDList)Session["BulkAssociateList"];
            //    ////string[] arrAssociate = hdnAssociateId.Value.Split(',');
            //    PrintIDCardInfo(ContractorList[Convert.ToInt32(CurrentPage.Value) - 1]);
            // }
        }

        /// <summary>
        /// The image Preview_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgPrev_Click(object sender, ImageClickEventArgs e)
        {
            ////Check if we are on any page greater than 0 
            ////if ((int.Parse(CurrentPage.Value) - 1) >= 0)
            ////{
            //    //Decrease the CurrentPage Value
            //    CurrentPage.Value = (int.Parse(CurrentPage.Value) - 1).ToString();
            ////}
            ////BuildPagers();

            ////List<string> ContractorList = (List<string>)Session["BulkContractorId"];
            ////PrintIDCardInfo(ContractorList[Convert.ToInt32(CurrentPage.Value) - 1]);
        }

        /// <summary>
        /// Export to Excel contractor details
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event e</param>
        protected void ExportLink_Click(object sender, EventArgs e)
        {
            this.ExportGridView();
        }

        /// <summary>
        /// The IDCardList_SelectedIndexChanged method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdIDCardList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Edit Contractor details
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event e</param>
        protected void GrdIDCardList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                this.grdIDCardList.EditIndex = e.NewEditIndex;
                this.grdIDCardList.DataSource = this.Session["DataForEdit"];
                this.grdIDCardList.DataBind();
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetResetEdit", "SetResetEdit();", true);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Cancel selected contractor details if edit mode is on
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event e</param>
        protected void GrdIDCardList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.grdIDCardList.EditIndex = -1;
            this.grdIDCardList.DataSource = this.Session["DataForEdit"];
            this.grdIDCardList.DataBind();
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetsetEdit", "SetsetEdit();", true);
        }

        /// <summary>
        /// Update contractor details.
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event e</param>
        protected void GrdIDCardList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string strLocationId = string.Empty;
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objrequestDetails =
                new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                DataSet dslocation = objrequestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
                DataTable dtlocation = dslocation.Tables[0];
                if (dtlocation.Rows.Count > 0)
                {
                    strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);
                    HttpContext.Current.Session["LocationId"] = strLocationId;
                }

                bool recrdUpdated = false;
                VMSBusinessLayer.VMSBusinessLayer.MasterDataBL objMasterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
                int strContractorId = Convert.ToInt32(this.grdIDCardList.DataKeys[e.RowIndex].Value);
                TextBox txtbxContractorId = (TextBox)this.grdIDCardList.Rows[e.RowIndex].FindControl("txtContractorId");
                TextBox txtbxContractorName = (TextBox)this.grdIDCardList.Rows[e.RowIndex].FindControl("txtContractorName");
                TextBox txtbxVendorName = (TextBox)this.grdIDCardList.Rows[e.RowIndex].FindControl("txtVendorName");
                TextBox txtSuperVisiorPhone = (TextBox)this.grdIDCardList.Rows[e.RowIndex].FindControl("txtSuperVisiorPhone");
                TextBox txtVendorPhoneNumber = (TextBox)this.grdIDCardList.Rows[e.RowIndex].FindControl("txtVendorPhoneNumber");
                DropDownList drplDocStatus = (DropDownList)this.grdIDCardList.Rows[e.RowIndex].FindControl("drpDocStatus");
                DropDownList drpStatus = (DropDownList)this.grdIDCardList.Rows[e.RowIndex].FindControl("drpStatus");
                if (!objrequestDetails.CheckContratorNumberExistForEdit(Convert.ToString(strContractorId), Convert.ToString(txtbxContractorId.Text), strLocationId))
                {
                    recrdUpdated = objMasterDataBL.UpdateContractorDetails(txtbxContractorId.Text, txtbxContractorName.Text, txtbxVendorName.Text, txtSuperVisiorPhone.Text, txtVendorPhoneNumber.Text, drplDocStatus.Text, drpStatus.Text, strContractorId, Convert.ToString(HttpContext.Current.Session["LoginId"]));
                    this.grdIDCardList.EditIndex = -1;
                    if (!string.IsNullOrEmpty(this.txtSearch.Text))
                    {
                        this.BtnSearch_Click(sender, e);
                        if (this.Session["DataForEdit"] == null)
                        {
                            string strSearch = string.Empty;
                            ////Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('Updated Successfully.'); </script>");                                                        
                            this.txtSearch.Text = strSearch.ToString();
                            this.lblError.Text = string.Empty;
                            ////this.Page_Load(null, null);
                            ////Response.Redirect(Request.Url.AbsoluteUri);
                            ClientScript.RegisterStartupScript(typeof(Page), "SubmitSuccess", "<script language='javascript'>alert('Updated Successfully.');window.location.href = 'ContractorIdCard.aspx';</script>", false);
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('Updated Successfully.'); </script>"); 
                        }
                    }
                    else
                    {
                        this.ShowGrid(Convert.ToInt32(strContractorId));
                    }
                }
                else
                {
                    this.grdIDCardList.EditIndex = -1;
                    if (!string.IsNullOrEmpty(this.txtSearch.Text))
                    {
                        this.BtnSearch_Click(sender, e);
                    }
                    else
                    {
                        this.ShowGrid(Convert.ToInt32(strContractorId));
                    }

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('Contractor ID already exists.'); </script>");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Delete_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void LbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool deleteFlag = false;
                VMSBusinessLayer.VMSBusinessLayer.MasterDataBL objMasterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
                foreach (GridViewRow row in this.grdIDCardList.Rows)
                {
                    if (((System.Web.UI.HtmlControls.HtmlInputCheckBox)row.FindControl("chkIDCard")).Checked)
                    {
                        int contractorID = Convert.ToInt32(this.grdIDCardList.DataKeys[row.RowIndex]["ContractorId"]);
                        objMasterDataBL.DeleteContractor(contractorID, Session["LoginID"].ToString());
                        deleteFlag = true;
                    }
                }

                if (deleteFlag)
                {
                    this.BtnSearch_Click(sender, e);
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('Contractor entry(s) deleted successfully'); </script>");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The SetPermission method
        /// </summary>        
        private void SetPermission()
        {
            if (this.Session["RoleID"] != null)
            {
                string roleId = Convert.ToString(Session["RoleID"]);
                if (roleId.Equals("Security"))
                {
                    this.btnBulkUpload.Visible = false;
                    this.ExportLink.Visible = false;
                    this.lbtnAddNew.Visible = false;
                    this.lbtnSave.Visible = false;
                    this.imgBulkUpload.Visible = false;
                    this.lbtnDelete.Visible = false;
                }
                else
                {
                    this.btnBulkUpload.Visible = true;
                    this.ExportLink.Visible = true;
                    this.lbtnAddNew.Visible = true;
                    this.lbtnSave.Visible = true;
                    this.imgBulkUpload.Visible = true;
                    this.lbtnDelete.Visible = true;
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// The DisableSession method
        /// </summary>        
        private void DisableSession()
        {
            this.Session["DataSourceFinal"] = null;
            this.Session["ViewCandidates"] = null;
            this.Session["DataSource1"] = null;
            this.Session["DataSourceFinal"] = null;
            this.Session["RowNumber"] = null;
            this.Session["validatedfinal"] = null;
            this.Session["DataSourceFinalCount"] = null;
            this.Session["validatedinitial"] = null;
            this.Session["Visitor"] = null;
            this.Session["ViewModify"] = null;
            this.Session["flagcheck"] = null;
            this.Session["Delete"] = null;
            this.Session["checked"] = null;
            this.Session["notchecked"] = null;
            this.Session["UpdatedChecked"] = null;
            this.Session["Duplicates"] = null;
            this.Session["ViewCandidatesEdit"] = null;
            this.Session["Cancelled"] = null;
            this.Session["ViewCancelImage"] = null;
        }

        /// <summary>
        /// To Bind Contractor details
        /// </summary>
        /// <param name="searckKey">Based on search key , Grid will bind</param>
        /// <param name="locationId">Location id parameter</param>
        /// <returns>DataTable contains contractor details</returns>
        private DataTable BindGrid(string searckKey, string locationId)
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL =
                    new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                DataTable dtdisplay =
                    objRequestDetailsBL.GetIDCardContractorDetails(searckKey, locationId);
                return dtdisplay;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// To Bind Contractor details by contractor ID.
        /// </summary>
        /// <param name="id">id parameter</param>
        /// <returns>data table</returns>
        private DataTable BindGridById(int id)
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL =
                    new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                DataTable dtdisplay =
                    objRequestDetailsBL.GetIDCardContractorDetailsById(id);
                return dtdisplay;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// Export to Excel contractor details
        /// </summary>
        private void ExportGridView()
        {
            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                try
                {
                    DataTable dt = this.ExcelData();
                    if (dt.Rows.Count > 0)
                    {
                        string attachment = "attachment; filename=Contractors List.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", attachment);
                        Response.ContentType = "application/vnd.ms-excel";
                        string tab = string.Empty;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            Response.Write(tab + dc.ColumnName);
                            tab = "\t";
                        }

                        Response.Write("\n");

                        int i;
                        foreach (DataRow dr in dt.Rows)
                        {
                            tab = string.Empty;
                            for (i = 0; i < dt.Columns.Count; i++)
                            {
                                Response.Write(tab + dr[i].ToString());
                                tab = "\t";
                            }

                            Response.Write("\n");
                        }

                        Response.End();
                    }
                    else
                    {
                        this.lblError.Text = "No Records Found!";
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                }
            }
            else
            {
                this.lblError.Text = "Please enter search criteria";
            }
        }

        /// <summary>
        /// The ExcelData method
        /// </summary>
        /// <returns>The System.Data.DataTable type object</returns>        
        private DataTable ExcelData()
        {
            DataTable returnTable = new DataTable();
            DataTable dttable = new DataTable();
            string strLocationId = string.Empty;
            DataSet dslocation = this.requestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
            DataTable dtlocation = dslocation.Tables[0];
            if (dtlocation.Rows.Count > 0)
            {
                strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);
            }

            if (!string.IsNullOrEmpty(this.txtSearch.Text))
            {
                dttable = this.BindGrid(this.txtSearch.Text, strLocationId);
                dttable = dttable.Select("Status='Active'").Count() > 0 ? dttable.Select("Status='Active'").CopyToDataTable() : new DataTable();
                if (dttable.Rows.Count > 0)
                {
                    this.lblDetails.Text = "Contractor Details";
                    this.grdIDCardList.DataSource = dttable;
                    this.grdIDCardList.DataBind();
                    this.grdIDCardList.PagerSettings.Visible = true;

                    returnTable = dttable.Copy();

                    returnTable.Columns.Remove("ContractorId");
                    returnTable.Columns.Remove("Rank");
                    returnTable.Columns.Remove("SearchPrId");
                    ////returnTable.Columns.Remove("HighPriority");
                    returnTable.Columns.Remove("TotalCount");

                    try
                    {
                        ////DataColumn sno = new DataColumn("S.No", System.Type.GetType("System.Int32"));
                        ////sno.AutoIncrement = true;
                        ////sno.AutoIncrementSeed = 1000;
                        ////sno.AutoIncrementStep = 1;

                        //// Add the column to a new DataTable. 
                        ////returnTable.Columns.Add(sno);

                        ////returnTable.Columns["RequestStatus"].ColumnName = "Status";
                        ////for (int i = 0; i < returnTable.Rows.Count; i++)
                        ////{
                        //    returnTable.Rows[i]["S.No"] = i + 1;
                        //    if (!(status == "Select Status"))
                        //        returnTable.Rows[i]["Status"] = status;
                        ////}

                        ////returnTable.Columns["VisitingCity"].ColumnName = "City";

                        returnTable.Columns["ContractorNumber"].ColumnName = "Contractor Id";

                        returnTable.Columns["Contractor Id"].SetOrdinal(0);
                        returnTable.Columns["Name"].SetOrdinal(1);
                        returnTable.Columns["VendorName"].SetOrdinal(2);
                        returnTable.Columns["DOCStatus"].SetOrdinal(3);
                        returnTable.Columns["Status"].SetOrdinal(4);
                        returnTable.Columns["SupervisiorPhone"].SetOrdinal(5);
                        returnTable.Columns["VendorPhoneNumber"].SetOrdinal(6);
                    }
                    catch (Exception ex)
                    {
                        Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                    }
                }
                else
                {
                    this.lblError.Text = "No Records Found!";
                }
            }

            return returnTable;
        }

        /// <summary>
        /// Get location details of user who logged in to app.
        /// </summary>
        /// <returns>data set</returns>
        private DataSet GetSecurityCity()
        {
            string securityID = Session["LoginID"].ToString();
            return this.requestDetails.GetSecurityCity(securityID);
        }

        /// <summary>
        /// display contractor details for Edit operation
        /// </summary>
        /// <param name="id">id parameter</param>
        private void ShowGrid(int id)
        {
            DataTable dttable = this.BindGridById(id);
            dttable = dttable.Select("Status='Active'").Count() > 0 ? dttable.Select("Status='Active'").CopyToDataTable() : new DataTable();
            if (dttable.Rows.Count > 0)
            {
                this.lblDetails.Text = "Contractor Details";
                this.lblDetails.Visible = true;
                this.grdIDCardList.DataSource = dttable;
                this.grdIDCardList.DataBind();
                this.grdIDCardList.Visible = true;
                this.lblError.Visible = false;
                this.grdIDCardList.PagerSettings.Visible = true;
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "SetReset", "SetReset();", true);
            }
        }
    }
}
