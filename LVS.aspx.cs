
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Security.Principal;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using VMSBusinessLayer;
    using VMSConstants;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class LVS
    /// </summary>
    public partial class LVS : System.Web.UI.Page
    {
        ////CR.No. -  PHYSC17052010CR07 start
        
        #region variables
        /// <summary>
        /// VMS BusinessLayer
        /// </summary> 
      private VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();       

       /// <summary>
       /// Get City Name
       /// </summary>
       private string city = string.Empty;

       /// <summary>
       /// Get Facility Name
       /// </summary>
       private string facility = string.Empty;

       /// <summary>
       /// Get Employee ID
       /// </summary>
       private string strEmployeeID = string.Empty;

       /// <summary>
       /// Get search click
       /// </summary>
        private int searchclick = 0;

        /// <summary>
        /// Method to set controls on Page Load
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Page.Form.DefaultButton = this.btnSearch.UniqueID;

                this.errortbl.Visible = false;
                this.SetVisibilityLevel(0);
                ////   FillControlValues();
                this.txtAssetNumber.Attributes.Add("onkeypress", "javascript:return NumberCharacterValidation(this);");
                this.txtAssociateID.Attributes.Add("onkeypress", "javascript:return SpecialCharacterValidation(this);");
                this.ImgMember.Visible = false;
                this.tdmember.Visible = false;

                this.searchclick = 0;
            }
        }
        #endregion

        /// <summary>
        /// Method to check if changed
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void RdobottomIN_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdobottomIN.Checked == true)
                {
                    this.rdobottomIN.Checked = true;
                    this.rdobottomOUT.Checked = false;
                    this.lblSuccessMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to check if changed
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void RdobottomOUT_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdobottomOUT.Checked == true)
                {
                    this.rdobottomOUT.Checked = true;
                    this.rdobottomIN.Checked = false;
                    this.lblSuccessMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        #region Private Methods

        /// <summary>
        /// Method to get security city
        /// </summary>
        /// <returns>Data set</returns>
        protected DataSet GetSecurityCity()
        {
            ////string city;
            string securityID = Session["LoginID"].ToString();

            return this.requestDetails.GetSecurityCity(securityID);
            ////return city;
        }
        
        /// <summary>
        /// Method to fetch the Facility once the Location is changed
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void DdlLocation_selectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet securitydetails = new DataSet();
                securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                this.city = securitydetails.Tables[0].Rows[0]["City"].ToString();

                if (this.tdmember.Visible == false)
                {
                    this.lblSuccessMessage.Visible = false;
                    this.errortbl.Visible = false;
                    DataTable datatableFacility = new LocationBL().GetFacilityDetails(this.city);
                    this.ddlFacility.DataSource = datatableFacility;
                    this.ddlFacility.DataBind();
                    this.ddlFacility.Items.Insert(0, new ListItem(VMSConstants.SELECT, "-1"));
                    //// CR.No. -  PHYSC17052010CR07 start

                    securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    this.facility = securitydetails.Tables[0].Rows[0]["Facility"].ToString();
                    this.ddlFacility.ClearSelection();
                    this.ddlFacility.Items.FindByText(this.facility).Selected = true;
                    this.ddlFacility.Enabled = false;
                    this.rdoCheckIn.Checked = true;
                    this.DdlFacility_selectedIndexChanged(null, null);
                    //// CR.No. -  PHYSC17052010CR07 end
                }
                else if (this.tdmember.Visible == true)
                {
                    this.lblSuccessMessage.Visible = false;
                    this.errortbl.Visible = false;
                    DataTable datatableFacility = new LocationBL().GetFacilityDetails(this.city);
                    this.ddlFacilitybottom.DataSource = datatableFacility;
                    this.ddlFacilitybottom.DataBind();
                    this.ddlFacilitybottom.Items.Insert(0, new ListItem(VMSConstants.SELECT, "-1"));

                    //// CR.No. -  PHYSC17052010CR07 start
                    securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    this.facility = securitydetails.Tables[0].Rows[0]["Facility"].ToString();
                    this.ddlFacilitybottom.ClearSelection();
                    this.ddlFacilitybottom.Items.FindByText(this.facility).Selected = true;
                    this.ddlFacilitybottom.Enabled = false;
                    this.rdobottomIN.Checked = true;
                    this.DdlFacility_selectedIndexChanged(null, null);
                    //// CR.No. -  PHYSC17052010CR07 end
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to set the values once the facility value is changed
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void DdlFacility_selectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string laptopPassIssuedDate = string.Empty;
                string laptopPassReturnedDate = string.Empty;
                int checkFlag = 0;

                if (this.tdmember.Visible == false)
                {
                    if (this.rdoCheckIn.Checked == true)
                    {
                        laptopPassIssuedDate = DateTime.Now.ToString();
                    }

                    if (this.rdoCheckOut.Checked == true)
                    {
                        laptopPassReturnedDate = DateTime.Now.ToString();
                    }
                }

                if (this.tdmember.Visible == true)
                {
                    if (this.rdobottomIN.Checked == true)
                    {
                        laptopPassIssuedDate = DateTime.Now.ToString();
                    }

                    if (this.rdobottomOUT.Checked == true)
                    {
                        laptopPassReturnedDate = DateTime.Now.ToString();
                    }
                }

                if (this.tdmember.Visible == true)
                {
                    checkFlag = this.Valiadte(this.txtMemeberID.Text.Trim(), laptopPassIssuedDate, laptopPassReturnedDate, this.city, this.ddlFacilitybottom.SelectedValue);
                }
                else if (this.tdmember.Visible == false)
                {
                    checkFlag = this.Valiadte(this.strEmployeeID, laptopPassIssuedDate, laptopPassReturnedDate, this.city, this.ddlFacility.SelectedValue);
                }

                if (checkFlag == 1 || checkFlag == 5)
                {
                    this.lblSuccessMessage.Visible = false;
                    this.rdoCheckIn.Checked = false;
                    this.rdoCheckIn.Enabled = true;
                    this.rdoCheckOut.Enabled = false;
                    this.rdobottomIN.Checked = false;
                    this.rdobottomIN.Enabled = true;
                    this.rdobottomOUT.Enabled = false;
                    this.lblMessage.Text = VMSConstants.CHECKINLAPTOP;
                    this.btnIdentityCard.Enabled = true;
                    this.btnVerify.Enabled = true;
                }

                this.lblSuccessMessage.Visible = false;
                this.errortbl.Visible = true;
                if (this.ViewState["dtIDCardIssuedLocation"] != null)
                {
                    DataTable datatableIDCardIssuedLocation = (DataTable)ViewState["dtIDCardIssuedLocation"];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Control Methods
        
        /// <summary>
        /// Method to search laptop details on the basis of Serial number/AssociateID
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string yesorno = string.Empty;
                string strSerialNo = string.Empty;
                string strAssetNumber = string.Empty;
                string strAssociateID = string.Empty;
                string strErrorMsg = string.Empty;
                string strSelectFlag = string.Empty;
                string strParameter = string.Empty;
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                if (!string.IsNullOrEmpty(this.txtAssociateID.Text.Trim().ToString()))
                {
                    strSelectFlag = VMSConstants.ASSOCIATEID;
                    strParameter = this.txtAssociateID.Text.Trim().ToString();
                }

                if (!string.IsNullOrEmpty(this.txtSerialNo.Text.Trim().ToString()))
                {
                    strSelectFlag = VMSConstants.SERIALNUMBER;
                    strParameter = this.txtSerialNo.Text.Trim().ToString();
                }

                if (!string.IsNullOrEmpty(this.txtAssetNumber.Text.Trim().ToString()))
                {
                    strSelectFlag = VMSConstants.ASSETNUMBER;
                    strParameter = this.txtAssetNumber.Text.Trim().ToString();
                }

                strErrorMsg = objEmployeeDetails.ValidateLaptopDetails(strParameter, strSelectFlag);

                if (!string.IsNullOrEmpty(this.txtAssociateID.Text.Trim().ToString()))
                {
                    strAssociateID = this.txtAssociateID.Text.Trim().ToString();
                    this.strEmployeeID = strAssociateID;
                }

                if (!string.IsNullOrEmpty(this.txtSerialNo.Text.Trim().ToString()))
                {
                    strSerialNo = objEmployeeDetails.GetAssociateID(this.txtSerialNo.Text.Trim().ToString());
                    this.strEmployeeID = strSerialNo;
                }

                if (!string.IsNullOrEmpty(this.txtAssetNumber.Text.Trim().ToString()))
                {
                    strAssetNumber = objEmployeeDetails.GetAssetNumber(this.txtAssetNumber.Text.Trim().ToString());
                    this.strEmployeeID = strAssetNumber;
                }

                this.FillControlValues();
                this.errortbl.Visible = false;

                this.ImgMember.Visible = false;
                this.txtMembername.Visible = false;
                this.txtMemeberID.Visible = false;
                this.tdmember.Visible = false;
                this.tdTop.Visible = true;
                ////lblEmpID.Text = string.Empty;
                if (string.IsNullOrEmpty(this.txtAssociateID.Text.Trim().ToString()) && (string.IsNullOrEmpty(this.txtSerialNo.Text.Trim().ToString())) && (string.IsNullOrEmpty(this.txtAssetNumber.Text.Trim().ToString())))
                {
                    this.tdTop.Visible = false;
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = VMSConstants.SELECTONECRITERIA;
                    this.lblSuccessMessage.Visible = false;
                    this.panelEmp.Visible = false;
                    return;
                }

                if (!string.IsNullOrEmpty(this.txtAssociateID.Text.Trim().ToString()))
                {
                    if (this.txtAssociateID.Text.Trim().ToString().Length < 6)
                    {
                        this.tdTop.Visible = false;
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = "Enter the Valid Associate ID";
                        this.lblSuccessMessage.Visible = false;
                        this.panelEmp.Visible = false;
                        return;
                    }
                }

                ////if ((strAssociateID != strSerialNo) && (strSerialNo != string.Empty) && (strAssociateID != string.Empty)
                ////   || ((strAssociateID != strAssetNumber) && (strAssetNumber != string.Empty) && (strAssociateID != string.Empty))
                ////     || ((strAssetNumber != strSerialNo) && (strAssetNumber != string.Empty) && (strSerialNo != string.Empty)))
                ////{
                ////    errortbl.Visible = true;
                ////    lblMessage.Text = VMSConstants.VMSConstants.NORECORDFOUND;
                ////    lblSuccessMessage.Visible = false;
                ////    panelEmp.Visible = false;
                ////    return;
                ////}
                DataRow datareaderEmployeeInfo = objEmployeeDetails.GetLVSEmployeeDetails(this.strEmployeeID);

                if (datareaderEmployeeInfo == null)
                {
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = "Associate is  not active/Laptop details Not found";
                }

                if (datareaderEmployeeInfo != null && strErrorMsg == string.Empty)
                {
                    this.errortbl.Visible = false;
                    this.SetVisibilityLevel(1);
                    ////ddlLocation.SelectedIndex = 0;
                    ////ddlFacility.SelectedIndex = 0;
                    System.Text.RegularExpressions.Regex nonNumericCharacters = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                    this.lblEmpID.Text = datareaderEmployeeInfo["Associate_id"].ToString();
                    this.ViewState["AssociateID"] = this.lblEmpID.Text.ToString();
                    this.lblEmpName.Text = datareaderEmployeeInfo["AssociateName"].ToString();
                    this.lblEmpEmailID.Text = datareaderEmployeeInfo["EmailID"].ToString();
                    this.lblEmpLocation.Text = datareaderEmployeeInfo["location"].ToString();
                    this.lblEmpCountry.Text = datareaderEmployeeInfo["Country"].ToString();
                    this.lblEmpDepartment.Text = datareaderEmployeeInfo["department"].ToString();
                    this.lblEmployeeMobile.Text = nonNumericCharacters.Replace(datareaderEmployeeInfo["Mobile"].ToString(), string.Empty);
                    if (datareaderEmployeeInfo["AssociateImage"].ToString() == VMSConstants.IMAGE)
                    {
                        this.ImgAssociate.Visible = true;
                        this.ImgAssociate.ImageUrl = "AssociateImage.aspx?AssociateID=" + this.strEmployeeID;
                    }
                    else
                    {
                        this.ImgAssociate.ImageUrl = VMSConstants.IMAGEPATH;
                    }

                    strAssetNumber = this.txtAssetNumber.Text.Trim().ToString();

                    DataRow datareaderLaptopInfo = objEmployeeDetails.GetLaptopDetails(this.strEmployeeID, strAssetNumber);

                    this.lblMake.Text = datareaderLaptopInfo["Laptop Make"].ToString();
                    this.lblModel.Text = datareaderLaptopInfo["Laptop Model"].ToString();
                    this.lblSerial.Text = datareaderLaptopInfo["Serial Number"].ToString();
                    this.lblAssetNo.Text = datareaderLaptopInfo["Asset Number"].ToString();
                    this.lblDateIssued.Text = datareaderLaptopInfo["Date Issued"].ToString();
                    this.lblSuccessMessage.Visible = false;
                    if (this.txtAssociateID.Text.Trim() == this.lblEmpID.Text.Trim())
                    {
                        this.ImgMember.Visible = false;
                        this.txtMemeberID.Visible = false;
                        this.txtMembername.Visible = false;
                        this.tdmember.Visible = false;
                    }
                }

                if (this.txtAssociateID.Text.Trim().ToString() != string.Empty)
                {
                    string strAID = this.txtAssociateID.Text.Trim().ToString();
                    if (this.txtSerialNo.Text.Trim().ToString() != string.Empty)
                    {
                        string strSNO = this.txtSerialNo.Text.Trim().ToString();

                        yesorno = objEmployeeDetails.Checkmembership(strAID, strSNO, "Serialno");
                    }
                    else if (this.txtAssetNumber.Text.Trim().ToString() != string.Empty)
                    {
                        string strAssetNO = this.txtAssetNumber.Text.Trim().ToString();
                        yesorno = objEmployeeDetails.Checkmembership(strAID, strAssetNO, "Assetno");
                    }
                    else
                    {
                        if (this.txtAssociateID.Text.Trim() != this.lblEmpID.Text.Trim())
                        {
                            this.SetVisibilityLevel(0);
                        }

                        this.ImgMember.Visible = false;
                        this.txtMemeberID.Visible = false;
                        this.txtMembername.Visible = false;
                        this.lblMessage.Visible = true;

                        if (strErrorMsg != string.Empty)
                        {
                            this.errortbl.Visible = true;
                            this.lblMessage.Text = VMSConstants.LTSERIALASSETNO;
                        }

                        this.tdmember.Visible = false;
                    }

                    if (yesorno == "Mbryes")
                    {
                        this.errortbl.Visible = false;

                        this.Topdetails(this.strEmployeeID, ref strAssetNumber, objEmployeeDetails, ref datareaderEmployeeInfo);
                        datareaderEmployeeInfo = objEmployeeDetails.GetLVSEmployeeDetails(this.txtAssociateID.Text);

                        if (datareaderEmployeeInfo != null)
                        {
                            if (this.txtAssociateID.Text != this.lblEmpID.Text)
                            {
                                this.tdmember.Visible = true;
                                this.FillControlValues();
                                this.errortbl.Visible = false;
                                this.tdTop.Visible = false;
                                this.txtMembername.Visible = true;
                                this.txtMemeberID.Visible = true;
                                this.txtMemeberID.Text = datareaderEmployeeInfo["Associate_id"].ToString();

                                this.txtMembername.Text = datareaderEmployeeInfo["AssociateName"].ToString();
                                if (datareaderEmployeeInfo["AssociateImage"].ToString() == VMSConstants.IMAGE)
                                {
                                    this.ImgMember.Visible = true;
                                    this.ImgMember.ImageUrl = "AssociateImage.aspx?AssociateID=" + this.txtMemeberID.Text;
                                }
                                else
                                {
                                    this.ImgMember.Visible = true;
                                    this.ImgMember.ImageUrl = VMSConstants.IMAGEPATH;
                                }

                                this.FillControlValues();
                            }
                        }
                    }
                    ////if (yesorno == "NF")
                    ////{
                    ////    if (strErrorMsg != string.Empty)
                    ////    {
                    ////        SetVisibilityLevel(0);
                    ////        errortbl.Visible = true;
                    ////        lblMessage.Text = VMSConstants.VMSConstants.LTNOTFOUND;
                    ////    }
                    ////    else
                    ////    {
                    ////        errortbl.Visible = true;
                    ////        lblMessage.Text = txtAssociateID.Text + VMSConstants.VMSConstants.LTNOTPERMIT;
                    ////        tdTop.Visible = false;
                    ////    }
                    ////    Topdetails(strEmployeeID, ref strAssetNumber, ObjEmployeeDetails, ref drEmployeeInfo);
                    ////    //SetVisibilityLevel(0);
                    ////    ImgMember.Visible = false;
                    ////    txtMembername.Visible = false;
                    ////    txtMemeberID.Visible = false;
                    ////    tdmember.Visible = false;
                    ////}

                    if (yesorno == "NF")
                    {
                        ////Topdetails(strEmployeeID, ref strAssetNumber, ObjEmployeeDetails, ref drEmployeeInfo);
                        if (strErrorMsg != string.Empty)
                        {
                            this.errortbl.Visible = true;
                            this.SetVisibilityLevel(0);
                            this.lblMessage.Text = VMSConstants.LTNOTFOUND;
                        }
                        else
                        {
                            this.errortbl.Visible = true;
                            this.lblMessage.Text = this.txtAssociateID.Text + VMSConstants.LTNOTPERMIT;
                        }

                        this.ImgMember.Visible = false;
                        this.txtMembername.Visible = false;
                        this.txtMemeberID.Visible = false;
                        this.tdmember.Visible = false;
                        this.tdTop.Visible = false;
                    }
                    ////SetVisibilityLevel(0);
                    ////errortbl.Visible = true;
                    ////lblMessage.Text = strErrorMsg;
                    ////txtSerialNo.Visible = true;
                    ////}
                }
                else
                {
                    if (strErrorMsg != string.Empty)
                    {
                        this.SetVisibilityLevel(0);
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = "Laptop Details not found";
                        this.ImgMember.Visible = false;
                        this.txtMembername.Visible = false;
                        this.txtMemeberID.Visible = false;
                        this.tdmember.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to Verify the Laptop details of user
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                int saveFlag = 0;
                int checkFlag = 0;
                string laptopPassIssuedDate = string.Empty;
                string laptopPassReturnedDate = string.Empty;
                string strAssociateID;
                this.searchclick = 1;
                string strCheckStatus = string.Empty;
                EmployeeBL objEmployeeBL = new EmployeeBL();
                ////string strUserId = VMSUtility.VMSUtility.GetUserId();

                string strUserId = Session["LoginID"].ToString();
                if (this.ImgMember.Visible == false)
                {
                    if (this.ddlLocation.SelectedIndex == 0)
                    {
                        this.errortbl.Visible = true;
                        this.lblSuccessMessage.Visible = false;
                        this.lblMessage.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.SELECTLOCATION;
                        return;
                    }

                    if (this.ddlFacility.SelectedIndex == 0)
                    {
                        this.errortbl.Visible = true;
                        this.lblSuccessMessage.Visible = false;
                        this.lblMessage.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.SELECTFACILITY;
                        return;
                    }

                    if (this.rdoCheckIn.Checked == false && (this.rdoCheckOut.Checked == false))
                    {
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = VMSConstants.SELECTCHECKINORCHECKOUT;
                        return;
                    }

                    strAssociateID = this.txtAssociateID.Text.Trim().ToString();
                    if (!string.IsNullOrEmpty(strAssociateID))
                    {
                        if (objEmployeeBL.ValidateAssociateDetails(strAssociateID))
                        {
                            DataRow datareaderEmployeeInfo = objEmployeeBL.GetEmployeeDetails(strAssociateID);
                            if (datareaderEmployeeInfo != null)
                            {
                                if (ViewState["AssociateID"].ToString().Trim() != strAssociateID)
                                {
                                    this.SetVisibilityLevel(0);
                                    this.errortbl.Visible = true;
                                    this.lblMessage.Text = VMSConstants.ASSOCIATEIDMATCH;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            this.SetVisibilityLevel(0);
                            this.errortbl.Visible = true;
                            this.lblMessage.Text = VMSConstants.VALIDASSOCIATEID;
                            return;
                        }
                    }

                    if (this.rdoCheckIn.Checked == true)
                    {
                        strCheckStatus = VMSConstants.CHECKIN;
                        laptopPassIssuedDate = DateTime.Now.ToString();
                    }

                    if (this.rdoCheckOut.Checked == true)
                    {
                        strCheckStatus = VMSConstants.CHECKOUT;
                        laptopPassReturnedDate = DateTime.Now.ToString();
                    }

                    checkFlag = this.Valiadte(this.lblEmpID.Text.ToString(), laptopPassIssuedDate, laptopPassReturnedDate, this.ddlLocation.SelectedValue, this.ddlFacility.SelectedValue);

                    if (checkFlag == 1 || checkFlag == 5)
                    {
                        this.errortbl.Visible = false;
                        //// SaveFlag = objEmployeeBL.SaveLaptopInformation(lblEmpID.Text.ToString(), strCheckStatus, Session["UserID"].ToString(), ddlFacility.SelectedValue);
                        saveFlag = objEmployeeBL.SaveLaptopInformation(this.lblEmpID.Text.ToString(), strCheckStatus, strUserId, this.ddlFacility.SelectedValue, this.lblAssetNo.Text.ToString(), this.lblSerial.Text.ToString());

                        if (saveFlag == 1)
                        {
                            this.rdoCheckOut.Enabled = true;

                            this.rdoCheckIn.Enabled = false;
                        }

                        if (saveFlag == 2)
                        {
                            this.rdoCheckOut.Enabled = false;

                            this.rdoCheckIn.Enabled = true;
                        }

                        if ((saveFlag == 1) || (saveFlag == 2))
                        {
                            this.lblSuccessMessage.Visible = true;
                            this.lblSuccessMessage.Text = string.Concat(VMSConstants.VERIFYLOCATION, this.lblEmpID.Text.ToString());
                        }
                    }
                }
                else if (this.ImgMember.Visible == true)
                {
                    if (this.ddlLocationbottom.SelectedIndex == 0)
                    {
                        this.errortbl.Visible = true;
                        this.lblSuccessMessage.Visible = false;
                        this.lblMessage.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.SELECTLOCATION;
                        return;
                    }

                    if (this.ddlFacilitybottom.SelectedIndex == 0)
                    {
                        this.errortbl.Visible = true;
                        this.lblSuccessMessage.Visible = false;
                        this.lblMessage.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.SELECTFACILITY;
                        return;
                    }

                    if (this.rdobottomIN.Checked == false && (this.rdobottomOUT.Checked == false))
                    {
                        this.errortbl.Visible = true;
                        this.lblMessage.Text = VMSConstants.SELECTCHECKINORCHECKOUT;
                        return;
                    }

                    strAssociateID = this.txtMemeberID.Text.Trim().ToString();
                    ////if (strAssociateID != string.Empty)
                    ////{

                    ////    if (objEmployeeBL.ValidateAssociateDetails(strAssociateID))
                    ////    {
                    ////        DataRow drEmployeeInfo = objEmployeeBL.GetEmployeeDetails(strAssociateID);
                    ////        if (drEmployeeInfo != null)
                    ////        {
                    ////            if (ViewState["AssociateID"].ToString().Trim() != strAssociateID)
                    ////            {
                    ////                SetVisibilityLevel(0);
                    ////                errortbl.Visible = true;
                    ////                lblMessage.Text = VMSConstants.VMSConstants.ASSOCIATEIDMATCH;
                    ////                return;
                    ////            }

                    ////        }

                    ////    }
                    ////    else
                    ////    {
                    ////        SetVisibilityLevel(0);
                    ////        errortbl.Visible = true;
                    ////        lblMessage.Text = VMSConstants.VMSConstants.VALIDASSOCIATEID;
                    ////        return;
                    ////    }
                    ////}
                    if (this.rdobottomIN.Checked == true)
                    {
                        strCheckStatus = VMSConstants.CHECKIN;
                        laptopPassIssuedDate = DateTime.Now.ToString();
                    }

                    if (this.rdobottomOUT.Checked == true)
                    {
                        strCheckStatus = VMSConstants.CHECKOUT;
                        laptopPassReturnedDate = DateTime.Now.ToString();
                    }

                    checkFlag = this.Valiadte(this.txtMemeberID.Text.Trim(), laptopPassIssuedDate, laptopPassReturnedDate, this.ddlLocationbottom.SelectedValue, this.ddlFacilitybottom.SelectedValue);

                    if (checkFlag == 1 || checkFlag == 5)
                    {
                        this.errortbl.Visible = false;
                        //// SaveFlag = objEmployeeBL.SaveLaptopInformation(lblEmpID.Text.ToString(), strCheckStatus, Session["UserID"].ToString(), ddlFacility.SelectedValue);
                        saveFlag = objEmployeeBL.SaveLaptopInformation(this.txtMemeberID.Text.Trim().ToString(), strCheckStatus, strUserId, this.ddlFacilitybottom.SelectedValue, this.lblAssetNo.Text.ToString(), this.lblSerial.Text.ToString());

                        if (saveFlag == 1)
                        {
                            this.rdobottomIN.Enabled = false;
                            this.rdobottomOUT.Enabled = true;
                        }

                        if (saveFlag == 2)
                        {
                            this.rdobottomOUT.Enabled = false;
                            this.rdobottomIN.Enabled = true;
                        }

                        if ((saveFlag == 1) || (saveFlag == 2))
                        {
                            this.lblSuccessMessage.Visible = true;
                            this.lblSuccessMessage.Text = string.Concat(VMSConstants.VERIFYLOCATION, this.txtMemeberID.Text.ToString());
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
        /// Method to clear values of controls
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetVisibilityLevel(0);
                this.txtAssociateID.Text = string.Empty;
                this.txtSerialNo.Text = string.Empty;
                this.txtAssetNumber.Text = string.Empty;
                this.errortbl.Visible = false;
                this.ImgMember.Visible = false;
                this.txtMemeberID.Visible = false;
                this.txtMembername.Visible = false;
                this.tdmember.Visible = false;
                this.lblSuccessMessage.Visible = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to set visibility level of controls on the basis selecting check in option of radio button        
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void RdoCheckIn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdoCheckIn.Checked == true)
                {
                    this.rdoCheckIn.Checked = true;
                    this.rdoCheckOut.Checked = false;
                    this.lblSuccessMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to set visibility level of controls on the basis selecting checkout option of radio button        
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argument</param>
        protected void RdoCheckOut_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdoCheckOut.Checked == true)
                {
                    this.rdoCheckOut.Checked = true;
                    this.rdoCheckIn.Checked = false;
                    this.lblSuccessMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Top details method
        /// </summary>
        /// <param name="strgEmployeeID">The Employee ID parameter</param>
        /// <param name="strAssetNumber">The Asset Number parameter</param>
        /// <param name="objEmployeeDetails">The Employee Details parameter</param>
        /// <param name="datareaderEmployeeInfo">The data reade rEmployee Info parameter</param>        
        private void Topdetails(string strgEmployeeID, ref string strAssetNumber, EmployeeBL objEmployeeDetails, ref DataRow datareaderEmployeeInfo)
        {
            datareaderEmployeeInfo = objEmployeeDetails.GetLVSEmployeeDetails(strgEmployeeID);
            datareaderEmployeeInfo["Associate_id"] = strgEmployeeID;
            if (datareaderEmployeeInfo != null)
            {
                this.errortbl.Visible = false;
                this.SetVisibilityLevel(1);
                //// CR.No. -  PHYSC17052010CR07 start commented
                // //ddlLocation.SelectedIndex = 0;
                // //ddlFacility.SelectedIndex = 0;
                //// CR.No. -  PHYSC17052010CR07 end

                System.Text.RegularExpressions.Regex nonNumericCharacters = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                this.lblEmpID.Text = XSS.HtmlEncode(datareaderEmployeeInfo["Associate_id"].ToString());
                this.ViewState["AssociateID"] = this.lblEmpID.Text.ToString();
                this.lblEmpName.Text = XSS.HtmlEncode(datareaderEmployeeInfo["AssociateName"].ToString());
                this.lblEmpEmailID.Text = XSS.HtmlEncode(datareaderEmployeeInfo["EmailID"].ToString());
                this.lblEmpLocation.Text = XSS.HtmlEncode(datareaderEmployeeInfo["location"].ToString());
                this.lblEmpCountry.Text = XSS.HtmlEncode(datareaderEmployeeInfo["Country"].ToString());
                this.lblEmpDepartment.Text = XSS.HtmlEncode(datareaderEmployeeInfo["department"].ToString());
                this.lblEmployeeMobile.Text = XSS.HtmlEncode(nonNumericCharacters.Replace(datareaderEmployeeInfo["Mobile"].ToString(), string.Empty));
                if (datareaderEmployeeInfo["AssociateImage"].ToString() == VMSConstants.IMAGE)
                {
                    this.ImgAssociate.Visible = true;
                    this.ImgAssociate.ImageUrl = "AssociateImage.aspx?AssociateID=" + strgEmployeeID;
                }
                else
                {
                    this.ImgAssociate.ImageUrl = VMSConstants.IMAGEPATH;
                }

                strAssetNumber = XSS.HtmlEncode(this.txtAssetNumber.Text.Trim().ToString());
                DataRow datareaderLaptopInfo = objEmployeeDetails.GetLaptopDetails(strgEmployeeID, strAssetNumber);

                this.lblMake.Text = XSS.HtmlEncode(datareaderLaptopInfo["Laptop Make"].ToString());
                this.lblModel.Text = XSS.HtmlEncode(datareaderLaptopInfo["Laptop Model"].ToString());
                this.lblSerial.Text = XSS.HtmlEncode(datareaderLaptopInfo["Serial Number"].ToString());
                this.lblAssetNo.Text = XSS.HtmlEncode(datareaderLaptopInfo["Asset Number"].ToString());
                this.lblDateIssued.Text = XSS.HtmlEncode(datareaderLaptopInfo["Date Issued"].ToString());
                this.lblSuccessMessage.Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// Method to validate
        /// </summary>
        /// <param name="associateID">Associate ID</param>
        /// <param name="passIssuedDate">Pass Issued Date</param>
        /// <param name="passReturnedDate">Pass Returned Date</param>
        /// <param name="locationCity">Location City</param>
        /// <param name="strfacility">Facility Name</param>
        /// <returns>Check Flag</returns>
        private int Valiadte(string associateID, string passIssuedDate, string passReturnedDate, string locationCity, string strfacility)
        {
            int checkFlag = 0;
            try
            {
                EmployeeBL objEmployeeDetails = new EmployeeBL();
                checkFlag = objEmployeeDetails.ValidateLaptopUserDetails(associateID, passIssuedDate, passReturnedDate, locationCity, strfacility);

                if (this.ImgMember.Visible == false)
                {
                    ////if (ddlLocation.SelectedIndex == 0)
                    ////{
                    ////    errortbl.Visible = true;
                    ////    lblSuccessMessage.Visible = false;
                    ////    lblMessage.Text = string.Empty;
                    ////    lblMessage.Text = VMSConstants.VMSConstants.SELECTLOCATION;
                    ////    return 0;
                    ////}
                }

                if (this.ImgMember.Visible == true)
                {
                    if (this.ddlLocationbottom.SelectedIndex == 0)
                    {
                        ////errortbl.Visible = true;
                        ////lblSuccessMessage.Visible = false;
                        ////lblMessage.Text = string.Empty;
                        ////lblMessage.Text = VMSConstants.VMSConstants.SELECTLOCATION;
                        ////return 0;
                    }
                }

                if (checkFlag == 4)
                {
                    this.lblSuccessMessage.Visible = false;
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = VMSConstants.ISSUECARDFOROTHERLOCATION;
                    this.rdoCheckOut.Enabled = false;

                    this.rdoCheckIn.Enabled = false;
                    this.rdobottomIN.Enabled = false;
                    this.rdobottomOUT.Enabled = false;
                    this.btnIdentityCard.Enabled = false;
                    this.btnVerify.Enabled = false;
                    return 0;
                }

                if (this.ImgMember.Visible == false)
                {
                    if (this.ddlFacility.SelectedIndex == 0)
                    {
                        this.errortbl.Visible = true;
                        this.lblSuccessMessage.Visible = false;
                        this.lblMessage.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.SELECTFACILITY;
                        return 0;
                    }
                }

                if (this.ImgMember.Visible == true)
                {
                    if (this.ddlFacilitybottom.SelectedIndex == 0)
                    {
                        this.errortbl.Visible = true;
                        this.lblSuccessMessage.Visible = false;
                        this.lblMessage.Text = string.Empty;
                        this.lblMessage.Text = VMSConstants.SELECTFACILITY;
                        return 0;
                    }
                }
                ////if (CheckFlag == 5)
                ////{

                ////    lblSuccessMessage.Visible = false;
                ////    errortbl.Visible = true;
                ////    lblMessage.Text = VMSConstants.VMSConstants.CARDISSUEDTODAY;
                ////    rdoCheckOut.Enabled = false;
                ////    rdoCheckIn.Enabled = false;
                ////    rdobottomIN.Enabled = false;
                ////    rdobottomOUT.Enabled = false;
                ////    btnIdentityCard.Enabled = false;
                ////    btnVerify.Enabled = false;
                ////    return 0;
                ////}
                if (checkFlag == 2)
                {
                    this.lblSuccessMessage.Visible = false;
                    this.errortbl.Visible = true;
                    this.rdoCheckIn.Enabled = false;
                    this.rdoCheckOut.Enabled = true;
                    this.rdobottomIN.Enabled = false;
                    this.rdobottomOUT.Enabled = true;
                    this.btnIdentityCard.Enabled = true;
                    this.btnVerify.Enabled = true;
                    this.lblMessage.Text = VMSConstants.CHECKOUTLAPTOP;
                    return 0;
                }

                if (checkFlag == 3)
                {
                    this.lblSuccessMessage.Visible = false;
                    this.errortbl.Visible = true;
                    this.rdoCheckOut.Enabled = false;
                    this.rdoCheckIn.Enabled = true;
                    this.rdobottomIN.Enabled = true;
                    this.rdobottomOUT.Enabled = false;
                    this.btnIdentityCard.Enabled = true;
                    this.btnVerify.Enabled = true;
                    this.lblMessage.Text = VMSConstants.CHECKINLAPTOP;
                    return 0;
                }

                ////incomplete verification process LVS17052010CR07
                if (checkFlag == 7)
                {
                    if (this.tdmember.Visible != true)
                    {
                        this.rdoCheckIn.Checked = true;
                        this.rdoCheckIn.Enabled = true;
                        this.rdoCheckOut.Enabled = false;
                        this.rdoCheckOut.Checked = false;
                    }
                    else
                    {
                        this.rdobottomIN.Checked = true;
                        this.rdobottomIN.Enabled = true;
                        this.rdobottomOUT.Enabled = false;
                        this.rdobottomOUT.Checked = false;
                    }

                    if (this.searchclick == 1)
                    {
                        ////string strScript1 = "<script language='javascript'>  window.showModalDialog(\"incompleteverification.aspx?associateid=" + AssociateID + "&userid=" + Session["LoginID"].ToString() + "&Locationid=" + ddlFacility.SelectedValue.ToString() + "\", \"dialogHeight:20px;dialogWidth:425px;center:yes;status=0\"); </script>";
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script type='text/javascript' language='javascript'>");

                        sb.Append(" window.name=\"MyWindow\"; window.showModalDialog('incompleteverification.aspx?associateid=" + associateID + "&userid=" + Session["LoginID"].ToString() + "&Locationid=" + this.ddlFacility.SelectedValue.ToString() + "','Incomplete verification - confirmation','dialogHeight:140px;dialogWidth:800px;status:no');");
                        sb.Append("</script>");

#pragma warning disable CS0618 // 'Page.RegisterStartupScript(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterStartupScript(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
                        Page.RegisterStartupScript("OpenModalDialog", sb.ToString());
#pragma warning restore CS0618 // 'Page.RegisterStartupScript(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterStartupScript(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
                        ////Page.RegisterClientScriptBlock("LoadSearchPage", strScript1);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return checkFlag;
        }

        /// <summary>
        /// Method to set the visibility levels of controls
        /// </summary>
        /// <param name="flag">set flag </param>
        private void SetVisibilityLevel(int flag)
        {
            try
            {
                if (flag == 1)
                {
                    this.panelEmp.Visible = true;
                    this.lblEmployeeHeader.Visible = true;
                    this.lblCardIssuedLocation.Visible = true;
                    this.lblCardIssuedFacility.Visible = true;
                    this.ddlLocation.Visible = true;
                    this.ddlFacility.Visible = true;
                    this.rdoCheckIn.Visible = true;
                    this.rdoCheckOut.Visible = true;

                    //// CR.No. -  PHYSC17052010CR07 start commented
                    //// rdoCheckIn.Enabled = false;
                    ////rdoCheckOut.Enabled = false;
                    ////rdobottomIN.Enabled = false;
                    ////rdobottomOUT.Enabled = false;
                    //// CR.No. -  PHYSC17052010CR07 end

                    this.lblLaptopHeader.Visible = true;
                    this.btnClear.Visible = true;
                    this.btnIdentityCard.Visible = true;
                    this.btnVerify.Visible = true;
                    ////rdoCheckIn.Checked = false;
                    ////rdoCheckOut.Checked = false;
                    this.rdoCheckIn.Checked = true;
                    this.rdoCheckOut.Checked = false;

                    this.rdobottomIN.Checked = true;
                    this.rdobottomOUT.Checked = false;
                }
                else if (flag == 0)
                {
                    this.ddlLocation.Visible = false;
                    this.ddlFacility.Visible = false;
                    this.panelEmp.Visible = false;
                    this.btnClear.Visible = false;
                    this.btnIdentityCard.Visible = false;
                    this.btnVerify.Visible = false;
                    this.rdoCheckIn.Visible = false;
                    this.rdoCheckOut.Visible = false;
                    this.rdobottomIN.Enabled = false;
                    this.rdobottomOUT.Enabled = false;
                    this.lblEmployeeHeader.Visible = false;
                    this.lblLaptopHeader.Visible = false;
                    this.lblCardIssuedLocation.Visible = false;
                    this.lblCardIssuedFacility.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to populate city Details        
        /// </summary>
        private void FillControlValues()
        {
            try
            {
                DataTable datatableLocation = new LocationBL().GetCityDetails();
                DataSet securitydetails = new DataSet();
                if (this.tdmember.Visible == false)
                {
                    this.ddlLocation.DataSource = datatableLocation;
                    this.ddlLocation.DataBind();
                    this.ddlLocation.Items.Insert(0, new ListItem(VMSConstants.SELECT, "-1"));
                    this.DdlLocation_selectedIndexChanged(null, null);

                    //// CR.No. -  PHYSC17052010CR07 start
                    securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    this.city = securitydetails.Tables[0].Rows[0]["City"].ToString();
                    this.ddlLocation.ClearSelection();
                    this.ddlLocation.Items.FindByText(this.city).Selected = true;
                    this.ddlLocation.Enabled = false;
                    //// CR.No. -  PHYSC17052010CR07 end
                }
                else
                {
                    this.ddlLocationbottom.DataSource = datatableLocation;
                    this.ddlLocationbottom.DataBind();
                    this.ddlLocationbottom.Items.Insert(0, new ListItem(VMSConstants.SELECT, "-1"));
                    this.DdlLocation_selectedIndexChanged(null, null);

                    //// CR.No. -  PHYSC17052010CR07 start
                    securitydetails = this.requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    this.city = securitydetails.Tables[0].Rows[0]["City"].ToString();
                    this.ddlLocationbottom.ClearSelection();
                    this.ddlLocationbottom.Items.FindByText(this.city).Selected = true;
                    this.ddlLocationbottom.Enabled = false;
                    //// CR.No. -  PHYSC17052010CR07 end
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
