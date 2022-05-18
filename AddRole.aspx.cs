

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class Role
    /// </summary>
    public partial class Role : System.Web.UI.Page
    {
        /// <summary>
        /// Data Set Object 
        /// </summary>
        private DataSet griddata = new DataSet();

        #region Public Methods

        /// <summary>        
        /// To replace special character
        /// </summary>
        /// <param name="str">receiving string</param>
        /// <returns>returning string</returns>
        public static string RegExValidate(string str)
        {
            string pattern = @"[;:>,</\\*]";

            return Regex.Replace(str, pattern, string.Empty);
        }

        /// <summary>
        /// Method to populate all roles
        /// </summary>
        public void InitRoles()
        {
            try
            {
                VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                DataTable userInfo = userDetailsBL.GetRolesBL();
                this.ddlUserRole.DataSource = userInfo;
                this.ddlUserRole.DataTextField = "Description";
                this.ddlUserRole.DataValueField = "RoleID";
                this.ddlUserRole.DataBind();
                this.ddlUserRole.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to get Initial Cities
        /// </summary>
        /// <param name="countryId">Country Id</param>
        public void InitCities(string countryId)
        {
            try
            {
                this.ddlCity.Items.Clear();
                this.ddlCity.Enabled = true;
                VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable cities = locationDetails.GetActiveCities(countryId);
                this.ddlCity.DataTextField = "LocationCity";
                this.ddlCity.DataValueField = "LocationCity";
                this.ddlCity.DataSource = cities;
                this.ddlCity.DataBind();
                this.ddlCity.Items.Insert(0, new ListItem("Select", "-1"));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to populate all Facilities within a City
        /// </summary>
        public void InitFacilities()
        {
            try
            {
                this.ddlFacility.Items.Clear();
                VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
                DataTable facilities = locationDetails.GetActiveFacilities(this.ddlCity.SelectedItem.Text);
                this.ddlFacility.DataTextField = "LocationName";
                this.ddlFacility.DataValueField = "LocationId";
                this.ddlFacility.DataSource = facilities;
                this.ddlFacility.DataBind();
                this.ddlFacility.Items.Insert(0, new ListItem("Select", "-1"));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to get ID card initial location
        /// </summary>
        public void InitIDCardLocations()
        {
            try
            {
                VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
                List<string> facilities = new List<string>();
                facilities = locationDetails.GetIDCardLocations();
                this.ddlIDCardLocation.DataSource = facilities;
                this.ddlIDCardLocation.DataBind();
                this.ddlIDCardLocation.Items.Insert(0, new ListItem("Select", string.Empty));
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to check Country
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="args">Server Validate Event Args</param>
        public void CheckCountry(object source, ServerValidateEventArgs args)
        {
            if (this.drpCountry.SelectedItem.Value == "-1")
            {
                args.IsValid = false;
            }
        }

        /// <summary>
        /// Method to check City
        /// </summary>
        /// <param name="source">source object</param>
        /// <param name="args">Server Validate Event Args</param>
        public void CheckCity(object source, ServerValidateEventArgs args)
        {
            if (this.ddlCity.SelectedItem.Value == "-1")
            {
                args.IsValid = false;
            }
        }

        /// <summary>
        /// Method to check facility
        /// </summary>
        /// <param name="source">source object</param>
        /// <param name="args">Server Validate Event Args</param>
        public void CheckFacility(object source, ServerValidateEventArgs args)
        {
            if (this.ddlFacility.SelectedItem.Value == "-1")
            {
                args.IsValid = false;
            }
        }

        /// <summary>
        /// Method to Verify Rendering In Server Form
        /// </summary>
        /// <param name="control">control object</param>
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Page Load function
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.InitCountry();
                    this.InitCities(this.drpCountry.SelectedItem.Value);
                    this.InitFacilities();
                    this.InitRoles();
                    this.pnlError.Visible = false;
                    this.ddlIDCardLocation.Visible = false;
                    this.lblIDCardLocation.Visible = false;
                    this.ddlCity.Enabled = true;
                    this.ddlFacility.Enabled = true;
                    this.drpCountry.Enabled = true;
                    if (this.ddlCity.SelectedItem.Text == "Select")
                    {
                    }

                    this.RequiredFieldValidatorIDCardLocation.Enabled = false;
                    this.imbExcel.Visible = false;
                    this.btnSubmit.Enabled = true;
                    this.btnViewAllocation.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Event for submitting role details
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Args</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlUserRole.SelectedItem.ToString().ToUpper().Equals("IDCARDADMIN"))
                {
                    this.CustomValidator1.Enabled = false;
                    this.CustomValidator2.Enabled = false;

                    if (this.ddlCity.SelectedItem.Text == "Select")
                    {
                    }

                    VMSBusinessLayer.UserDetailsBL userDetails = new VMSBusinessLayer.UserDetailsBL();
                    VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();

                    string associateFirstName = string.Empty;
                    string setUpdatedBy = Session["LoginID"].ToString();
                    DateTime today = DateTime.Now;
                    string setCity = this.ddlCity.SelectedValue.ToString();
                    int facilityId = Convert.ToInt32(this.ddlFacility.SelectedItem.Value);
                    string setRoleID = XSS.HtmlEncode(this.ddlUserRole.SelectedValue.ToString());
                    int countryId = 0;
                    countryId = int.Parse(this.drpCountry.SelectedItem.Value);
                    string setUserID = XSS.HtmlEncode(this.txtAssociateID.Text);
                    string strRoleName = this.ddlUserRole.SelectedItem.Text;
                    DataRow asscoaite = this.ValidateAssociate(setUserID);
                    string associateName = XSS.HtmlEncode(Convert.ToString(asscoaite["AssociateName"]));
                    string emailId = Convert.ToString(asscoaite["EmailID"]).Trim();
                    if (!string.IsNullOrEmpty(associateName))
                    {
                        if (associateName.Split(',').Count() > 1)
                        {
                            associateFirstName = associateName.Split(',')[1].ToString();
                        }

                        int rows = userDetails.SubmitRoleDetailsBL(setUserID, facilityId, setRoleID, setUpdatedBy, countryId);

                        ////added by krishna(449138) for temp access card
                        int rows2;
                        switch (this.ddlUserRole.SelectedItem.ToString().ToUpper())
                        {
                            case "IDCARDADMIN":
                                setCity = XSS.HtmlEncode(this.ddlIDCardLocation.SelectedItem.Value);
                                rows2 = userDetails.AddIDCardAdminRole(setUserID, setCity, setRoleID, setUpdatedBy);
                                break;
                            case "SUPERADMIN":
                                setCity = XSS.HtmlEncode(this.ddlCity.SelectedItem.Value);
                                rows2 = userDetails.AddIDCardAdminRole(setUserID, setCity, setRoleID, setUpdatedBy);
                                break;
                            case "ACCESSCARDCUSTODIAN":
                                break;
                            default:
                                userDetails.RemoveIDCardAdminRole(setUserID);
                                break;
                        }

                        this.txtAssociateName.Text = associateName;
                        if (rows != 0)
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "SubmitStatus", "<script language='javascript'>alert('Submitted Successfully.');</script>", false);
                            if (ConfigurationManager.AppSettings["MailSendRole_enable"].ToString() == "true")
                            {
                                if (!string.IsNullOrEmpty(emailId))
                                {
                                    requestDetails.SendMailToUser(emailId, setUserID, setCity, this.ddlFacility.SelectedItem.Text, strRoleName, associateFirstName);
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(typeof(Page), "SendMailDisabled", "<script language='javascript'>alert('Mailing Functionality has been disabled.');</script>", false);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "SubmitStatus", "<script language='javascript'>alert('Submit Unsuccessful');</script>", false);
                        }
                    }
                    else
                    {
                        this.btnSubmit.Enabled = false;
                        this.btnViewAllocation.Enabled = false;
                        this.lblError.Text = VMSConstants.VMSConstants.USERNOTFOUND;
                        this.pnlError.Visible = true;
                        this.txtAssociateID.Focus();
                    }
                }
                else if (this.IsValid)
                {
                    if (this.ddlCity.SelectedItem.Text == "Select")
                    {
                    }

                    VMSBusinessLayer.UserDetailsBL userDetails = new VMSBusinessLayer.UserDetailsBL();
                    VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
                    string associateFirstName = string.Empty;
                    string setUpdatedBy = Session["LoginID"].ToString();
                    DateTime today = DateTime.Now;
                    string setCity = this.ddlCity.SelectedValue.ToString();
                    int facilityId = Convert.ToInt32(this.ddlFacility.SelectedItem.Value);
                    string setRoleID = XSS.HtmlEncode(this.ddlUserRole.SelectedValue.ToString());
                    int countryId = 0;
                    countryId = int.Parse(this.drpCountry.SelectedItem.Value);
                    string setUserID = XSS.HtmlEncode(this.txtAssociateID.Text);
                    string strRoleName = this.ddlUserRole.SelectedItem.Text;
                    DataRow dtasscoaite = this.ValidateAssociate(setUserID);
                    string associateName = XSS.HtmlEncode(Convert.ToString(dtasscoaite["AssociateName"]));
                    string emailId = Convert.ToString(dtasscoaite["EmailID"]).Trim();
                    if (!string.IsNullOrEmpty(associateName))
                    {
                        if (associateName.Split(',').Count() > 1)
                        {
                            associateFirstName = associateName.Split(',')[1].ToString();
                        }

                        int rows = userDetails.SubmitRoleDetailsBL(setUserID, facilityId, setRoleID, setUpdatedBy, countryId);
                        ////added by ram(445894) for temp access card
                        int rows2;
                        switch (this.ddlUserRole.SelectedItem.ToString().ToUpper())
                        {
                            case "IDCARDADMIN":
                                setCity = XSS.HtmlEncode(this.ddlIDCardLocation.SelectedItem.Value);
                                rows2 = userDetails.AddIDCardAdminRole(setUserID, setCity, setRoleID, setUpdatedBy);
                                break;
                            case "SUPERADMIN":
                                setCity = XSS.HtmlEncode(this.ddlCity.SelectedItem.Value);
                                rows2 = userDetails.AddIDCardAdminRole(setUserID, setCity, setRoleID, setUpdatedBy);
                                break;
                            case "ACCESSCARDCUSTODIAN":
                                break;
                            default:
                                userDetails.RemoveIDCardAdminRole(setUserID);
                                break;
                        }

                        this.txtAssociateName.Text = associateName;
                        if (rows != 0)
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "SubmitStatus", "<script language='javascript'>alert('Submitted Successfully.');</script>", false);
                            if (ConfigurationManager.AppSettings["MailSendRole_enable"].ToString() == "true")
                            {
                                if (!string.IsNullOrEmpty(emailId))
                                {
                                    requestDetails.SendMailToUser(emailId, setUserID, setCity, this.ddlFacility.SelectedItem.Text, strRoleName, associateFirstName);
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(typeof(Page), "SendMailDisabled", "<script language='javascript'>alert('Mailing Functionality has been disabled.');</script>", false);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "SubmitStatus", "<script language='javascript'>alert('Submit Unsuccessful');</script>", false);
                        }
                    }
                    else
                    {
                        this.btnSubmit.Enabled = false;
                        this.btnViewAllocation.Enabled = false;
                        this.lblError.Text = VMSConstants.VMSConstants.USERNOTFOUND;
                        this.pnlError.Visible = true;
                        this.txtAssociateID.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Country drop down change method
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        protected void DrpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.InitCities(this.drpCountry.SelectedItem.Value);
                this.InitFacilities();
                if (this.ddlCity.SelectedItem.Text == "Select")
                {
                    ////RequiredFieldValidatorCity.Enabled = true;
                }
                else
                {
                    ////RequiredFieldValidatorCity.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Event for populating Facilities as per the city selected
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        protected void DdlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.InitFacilities();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// event for Resetting all values 
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.txtAssociateID.Text = string.Empty;
            this.txtAssociateName.Text = string.Empty;
            this.drpCountry.SelectedIndex = 0;
            this.ddlCity.Items.Clear();
            this.ddlCity.Items.Insert(0, new ListItem("Select", string.Empty));
            this.ddlFacility.Items.Clear();
            this.ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
            this.ddlFacility.SelectedIndex = 0;
            this.ddlUserRole.SelectedIndex = 0;
            this.gvAllocations.Visible = false;
            this.ddlIDCardLocation.Visible = false;
            this.lblIDCardLocation.Visible = false;
            this.ddlCity.Enabled = true;
            this.ddlFacility.Enabled = true;
            this.drpCountry.Enabled = true;
            ////bincey
            if (this.ddlCity.SelectedItem.Text == "Select")
            {
                ////RequiredFieldValidatorCity.Enabled = true;
            }

            this.RequiredFieldValidatorIDCardLocation.Enabled = false;
            this.imbExcel.Visible = false;
            this.pnlError.Visible = false;
            this.btnSubmit.Enabled = true;
            this.btnViewAllocation.Enabled = true;
            this.txtAssociateID.Focus();
        }

        /// <summary>
        /// Event for generating associate name on providing associate id
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Image Click Event Args</param>
        protected void ImbFindAssociate_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.pnlError.Visible = false;
                this.btnSubmit.Enabled = true;
                this.btnViewAllocation.Enabled = true;
                this.ddlIDCardLocation.Visible = false;
                this.lblIDCardLocation.Visible = false;
                this.ddlCity.Enabled = true;
                this.ddlFacility.Enabled = true;
                this.RequiredFieldValidatorIDCardLocation.Enabled = false;
                this.ddlUserRole.SelectedIndex = 0;
                this.gvAllocations.Visible = false;
                if (this.txtAssociateID.Text != string.Empty)
                {
                    DataRow dtassociateName = this.ValidateAssociate(this.txtAssociateID.Text.Trim());
                    if (dtassociateName == null)
                    {
                        this.txtAssociateName.Text = string.Empty;
                        this.drpCountry.ClearSelection();
                        this.ddlCity.ClearSelection();
                        this.ddlFacility.ClearSelection();
                        this.btnSubmit.Enabled = false;
                        this.btnViewAllocation.Enabled = false;
                        this.lblError.Text = VMSConstants.VMSConstants.USERNOTFOUND;
                        this.pnlError.Visible = true;
                        this.txtAssociateID.Focus();
                    }
                    else
                    {
                        string associateName = XSS.HtmlEncode(Convert.ToString(dtassociateName["AssociateName"]).Trim());
                        if (!string.IsNullOrEmpty(associateName))
                        {
                            this.txtAssociateName.Text = XSS.HtmlEncode(associateName);
                            string role = string.Empty;
                            VMSBusinessLayer.UserDetailsBL objUserRoleLoc = new VMSBusinessLayer.UserDetailsBL();
                            DataTable drinfo1 = objUserRoleLoc.GetLocationForUserRole_IDCardAdmin(this.txtAssociateID.Text.Trim());
                            if (drinfo1.Rows.Count > 0)
                            {
                                role = Convert.ToString(drinfo1.Rows[0]["RoleID"]);
                            }

                            DataTable drinfo = objUserRoleLoc.GetUserRoleLocationBL(this.txtAssociateID.Text.Trim());
                            if (drinfo1.Rows.Count > 0)
                            {
                                if (this.ddlUserRole.SelectedItem.ToString().ToUpper().Contains("IDCARDADMIN"))
                                {
                                    this.InitIDCardLocations();
                                    this.ddlIDCardLocation.Visible = true;
                                    this.lblIDCardLocation.Visible = true;
                                    this.ddlCity.Enabled = false;
                                    this.ddlFacility.Enabled = false;
                                    this.InitCountry();
                                    this.drpCountry.Items.FindByText(VMSConstants.VMSConstants.DEFAULTCOUNTRY).Selected = true;
                                    this.drpCountry.Enabled = false;
                                    this.CustomValidator1.Enabled = false;
                                    this.CustomValidator2.Enabled = false;
                                    this.RequiredFieldValidatorIDCardLocation.Enabled = true;
                                    string location = objUserRoleLoc.GetIDCardLocationForUser(this.txtAssociateID.Text.Trim()).Rows[0]["Location"].ToString();
                                    this.ddlIDCardLocation.Items.FindByText("Select").Selected = true;
                                    this.ddlCity.SelectedIndex = 0;
                                    this.ddlFacility.SelectedIndex = 0;
                                }
                                else
                                {
                                    this.InitCountry();
                                    this.drpCountry.Enabled = true;
                                    if (drinfo.Rows.Count > 0)
                                    {
                                        if (drinfo.Rows[0]["CountryId"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drinfo.Rows[0]["CountryId"].ToString()))
                                            {
                                                this.drpCountry.ClearSelection();
                                            }
                                        }

                                        this.InitCities(drinfo.Rows[0]["CountryId"].ToString());
                                        if (drinfo.Rows[0]["LocationCity"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drinfo.Rows[0]["LocationCity"].ToString()))
                                            {
                                                this.ddlCity.ClearSelection();
                                                ////Commented by Krishna (449138)
                                                ////ddlCity.Items.FindByText(Convert.ToString(drInfo.Rows[0]["LocationCity"]).ToString().Trim()).Selected = true;
                                            }
                                        }

                                        this.InitFacilities();
                                        if (drinfo.Rows[0]["LocationId"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drinfo.Rows[0]["LocationId"].ToString()))
                                            {
                                                this.ddlFacility.ClearSelection();
                                                ////Commented by Krishna (449138)
                                                ////ddlFacility.Items.FindByValue(drInfo.Rows[0]["LocationId"].ToString()).Selected = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (drinfo.Rows.Count > 0)
                            {
                                ////Commented by Krishna (449138)
                                ////ddlUserRole.SelectedValue = Convert.ToString(drInfo.Rows[0]["RoleID"]);
                                this.InitCountry();
                                this.drpCountry.Enabled = true;
                                if (drinfo.Rows[0]["CountryId"] != null)
                                {
                                    if (!string.IsNullOrEmpty(drinfo.Rows[0]["CountryId"].ToString()))
                                    {
                                        this.drpCountry.ClearSelection();
                                        ////Commented by Krishna (449138)
                                        ////drpCountry.Items.FindByValue(Convert.ToString(drInfo.Rows[0]["CountryId"])).Selected = true;
                                    }
                                }

                                this.InitCities(drinfo.Rows[0]["CountryId"].ToString());
                                if (drinfo.Rows[0]["LocationCity"] != null)
                                {
                                    if (!string.IsNullOrEmpty(drinfo.Rows[0]["LocationCity"].ToString()))
                                    {
                                        this.ddlCity.ClearSelection();
                                        ////Commented by Krishna (449138)
                                        ////ddlCity.Items.FindByText(Convert.ToString(drInfo.Rows[0]["LocationCity"]).ToString().Trim()).Selected = true;
                                    }
                                }

                                this.InitFacilities();
                                if (drinfo.Rows[0]["LocationId"] != null)
                                {
                                    if (!string.IsNullOrEmpty(drinfo.Rows[0]["LocationId"].ToString()))
                                    {
                                        this.ddlFacility.ClearSelection();
                                        ////Commented by Krishna (449138)
                                        ////ddlFacility.Items.FindByValue(drInfo.Rows[0]["LocationId"].ToString()).Selected = true;
                                    }
                                }
                            }
                            else
                            {
                                this.drpCountry.Enabled = true;
                                this.InitCountry();
                                this.InitCities(this.drpCountry.SelectedItem.Value);
                                this.InitFacilities();
                            }
                        }
                        else
                        {
                            this.btnSubmit.Enabled = false;
                            this.btnViewAllocation.Enabled = false;
                            this.lblError.Text = VMSConstants.VMSConstants.USERNOTFOUND;
                            this.pnlError.Visible = true;
                            this.txtAssociateID.Focus();
                        }
                    }
                }
                else
                {
                    ////lblMessageValidAssoc.Visible = true;
                    ////lblMessageValidAssoc.Text = VMSConstants.VMSConstants.ENTERASSOCIATEID;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to handle button click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        protected void BtnViewAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                this.SearchRoleAllocation();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to handle Excel button click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Image Click Event Args</param>
        protected void ImbExcel_Click(object sender, ImageClickEventArgs e)
        {
            string setAssociateID = this.txtAssociateID.Text;
            string setCity = this.ddlCity.SelectedValue.ToString();
            string setFacility = this.ddlFacility.SelectedValue.ToString();
            string setRoleID = this.ddlUserRole.SelectedValue.ToString();
            string setLocation = this.ddlIDCardLocation.SelectedValue.ToString();
            string countryId = Convert.ToString(this.drpCountry.SelectedItem.Value);
            try
            {
                VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                if (this.ddlUserRole.SelectedItem.ToString().ToUpper().Equals("IDCARDADMIN"))
                {
                    this.griddata = userDetailsBL.SearchIDCardAdminAllocationDetails(setAssociateID, setLocation);
                }
                else
                {
                    this.griddata = userDetailsBL.SearchRoleAllocationBL(setCity, setFacility, setRoleID, setAssociateID, countryId);
                }

                if (this.griddata.Tables[0].Rows.Count > 0)
                {
                    this.hdnGridView.DataSource = this.griddata;
                    this.hdnGridView.DataBind();

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.Cache.SetCacheability(HttpCacheability.Private);
                    Response.AddHeader("content-disposition", "attachment;filename=ViewRoleDelegation.xls");
                    Response.Charset = string.Empty;

                    Response.ContentType = "application/vnd.xls";
                    System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

                    this.hdnGridView.RenderControl(htmlWrite);
                    Response.Write(stringWrite.ToString());
                    htmlWrite.Close();
                    stringWrite.Close();
                    Response.End();
                }
                else
                {
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to get allocation Page Index Changing
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Grid View Page Event Args</param>
        protected void GvAllocations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SearchRoleAllocation();
            this.gvAllocations.PageIndex = e.NewPageIndex;
            this.gvAllocations.DataBind();
        }

        /// <summary>
        /// Method to handle User Role on Selected Index Changed
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        protected void DdlUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlUserRole.SelectedItem.ToString().ToUpper().Contains("IDCARDADMIN"))
                {
                    this.InitIDCardLocations();
                    this.ddlIDCardLocation.Visible = true;
                    this.lblIDCardLocation.Visible = true;
                    this.ddlCity.Enabled = false;
                    this.ddlFacility.Enabled = false;
                    ////drpCountry.Enabled = false;
                    this.ddlCity.SelectedIndex = 0;
                    ////drpCountry.SelectedIndex = 0;
                    this.InitCountry();
                    this.drpCountry.Items.FindByText(VMSConstants.VMSConstants.DEFAULTCOUNTRY).Selected = true;
                    ////drpCountry.SelectedValue = "101";
                    this.drpCountry.Enabled = false;
                    this.ddlFacility.SelectedIndex = 0;
                    ////RequiredFieldValidatorCity.Enabled = false;
                    ////RequiredFieldValidatorFacility.Enabled = false;
                    this.RequiredFieldValidatorIDCardLocation.Enabled = true;
                    ////RequiredFieldValidatorCountry.Enabled = false;
                    this.ddlIDCardLocation.Focus();
                }
                else
                {
                    this.ddlIDCardLocation.Visible = false;
                    this.lblIDCardLocation.Visible = false;
                    this.ddlCity.Enabled = true;
                    this.ddlFacility.Enabled = true;
                    this.drpCountry.Enabled = true;
                    ////bincey
                    if (this.ddlCity.SelectedItem.Text == "Select")
                    {
                        ////RequiredFieldValidatorCity.Enabled = true;
                    }
                    ////test bincey -- reverted

                    ////RequiredFieldValidatorFacility.Enabled = true;

                    this.RequiredFieldValidatorIDCardLocation.Enabled = false;
                    ////RequiredFieldValidatorCountry.Enabled = true;
                    this.ddlCity.Focus();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to handle Revoke AccessButton Click
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        protected void BtnRevokeAccess_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtAssociateID.Text.Trim() != Session["LoginID"].ToString())
                {
                    string countryId = string.Empty;
                    string setAssociateID = this.txtAssociateID.Text;
                    string setCity = this.ddlCity.SelectedValue;
                    string setFacility = this.ddlFacility.SelectedValue;
                    string setRoleID = this.ddlUserRole.SelectedValue;
                    if (!(this.drpCountry.SelectedItem.Text == "Select"))
                    {
                        countryId = Convert.ToString(this.drpCountry.SelectedItem.Value);
                    }
                    else
                    {
                        countryId = "-1";
                    }

                    VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                    this.griddata = userDetailsBL.SearchRoleAllocationBL(setCity, setFacility, setRoleID, setAssociateID, countryId);
                    int roleCount = this.griddata.Tables[0].Rows.Count;
                    if (setRoleID == "10") 
                    {
                        ////if id card admin
                        if (userDetailsBL.RemoveIDCardAdminRole(this.txtAssociateID.Text.Trim()))
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "RevokeStatus", "<script language='javascript'>alert('Role has been revoked successfully');</script>", false);
                            this.BtnClear_Click(null, null);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "RevokeErrorStatus", "<script language='javascript'>alert('The associate ID(" + XSS.HtmlEncode(this.txtAssociateID.Text.Trim()) + ") does not have any role in the Physical Security portal');</script>", false);
                        }
                    }
                    else if (roleCount != 0)
                    {
                        string role = this.griddata.Tables[0].Rows[0][1].ToString();
                        string facility = this.griddata.Tables[0].Rows[0][4].ToString();
                        if (userDetailsBL.RevokeAccess(XSS.HtmlEncode(RegExValidate(this.txtAssociateID.Text.Trim())), XSS.HtmlEncode(RegExValidate(this.txtAssociateName.Text.Trim())), Session["LoginID"].ToString(), role, facility))
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "RevokeStatus", "<script language='javascript'>alert('Role has been revoked successfully');</script>", false);
                            this.BtnClear_Click(null, null);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(typeof(Page), "RevokeErrorStatus", "<script language='javascript'>alert('The associate ID(" + XSS.HtmlEncode(this.txtAssociateID.Text.Trim()) + ") does not have any role in the Physical Security portal');</script>", false);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), "RevokeErrorStatus", "<script language='javascript'>alert('The associate ID(" + XSS.HtmlEncode(this.txtAssociateID.Text.Trim()) + ") does not have any role in the Physical Security portal');</script>", false);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "Revoke", "<script language='javascript'>alert('You cannot revoke your own role');</script>", false);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method for getting Country details
        /// </summary>
        private void InitCountry()
        {
            VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.LocationDetailsBL();
            DataTable dtcoubtryList = locationDetails.GetActiveCountry();
            this.drpCountry.DataTextField = "Country";
            this.drpCountry.DataValueField = "CountryId";
            this.drpCountry.DataSource = dtcoubtryList;
            this.drpCountry.DataBind();
            this.drpCountry.Items.Insert(0, new ListItem("Select", "-1"));
        }
        
        /// <summary>
        /// method to generate grid view report based on specified search criteria
        /// </summary>
        private void SearchRoleAllocation()
        {
            string countryId = string.Empty;
            string setAssociateID = this.txtAssociateID.Text;
            string setCity = this.ddlCity.SelectedValue;
            string setFacility = this.ddlFacility.SelectedValue;
            string setRoleID = this.ddlUserRole.SelectedValue;
            if (!(this.drpCountry.SelectedItem.Text == "Select"))
            {
                countryId = Convert.ToString(this.drpCountry.SelectedItem.Value);
            }
            else
            {
                countryId = "-1";
            }

            string setLocation = this.ddlIDCardLocation.SelectedValue.ToString();
            try
            {
                VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.UserDetailsBL();
                if (this.ddlUserRole.SelectedItem.ToString().ToUpper().Equals("IDCARDADMIN"))
                {
                    this.griddata = userDetailsBL.SearchIDCardAdminAllocationDetails(setAssociateID, setLocation);
                }
                else
                {
                    this.griddata = userDetailsBL.SearchRoleAllocationBL(setCity, setFacility, setRoleID, setAssociateID, countryId);
                }

                if (this.griddata.Tables[0].Rows.Count > 0)
                {
                    this.imbExcel.Visible = true;
                    this.gvAllocations.Visible = true;
                    this.gvAllocations.DataSource = this.griddata;
                    this.gvAllocations.DataBind();
                }
                else
                {
                    this.imbExcel.Visible = false;
                    this.gvAllocations.Visible = true;
                    this.gvAllocations.DataSource = this.griddata;
                    this.gvAllocations.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to validate Associate Id
        /// </summary>
        /// <param name="associateID">Associate ID</param>
        /// <returns>Associate rows</returns>
        private DataRow ValidateAssociate(string associateID)
        {
            EmployeeBL objEmployeeDetails = new EmployeeBL();
            DataRow associateRow = objEmployeeDetails.GetEmployeeDetails_Associate(associateID);
            return associateRow;
        }

        #endregion       
    }
}
