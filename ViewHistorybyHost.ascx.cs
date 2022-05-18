
namespace VMSDev.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Linq;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessEntity;

    /// <summary>
    /// Partial class View History by Host
    /// </summary>   
    public partial class ViewHistorybyHost : System.Web.UI.UserControl
    {
        /// <summary>
        /// The Countries field
        /// </summary>        
        private List<string> vountries = new List<string>();
        
        /// <summary>
        /// The Cities field
        /// </summary>        
        private List<string> cities = new List<string>();
        
        /// <summary>
        /// The Facilities field
        /// </summary>        
        private List<string> facilities = new List<string>();
        
        /// <summary>
        /// The Search field
        /// </summary>        
        private object searchParamObj = new object();
        
        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();

        /// <summary>
        /// The LocationDetails field
        /// </summary>        
        private VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL();
        
        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
        
        ////Sending the search parameters to the SP
        
        /// <summary>
        /// The send Search method
        /// </summary>        
        public void SendSearchParams()
        {
            try
            {
                if (this.txtOthers.Visible == true)
                {
                    if (string.IsNullOrEmpty(this.txtFirstName.Text) && string.IsNullOrEmpty(this.txtLastName.Text) && string.IsNullOrEmpty(this.txtCompany.Text) && string.IsNullOrEmpty(this.txtDesignation.Text) && this.ddlNativeCountry.SelectedIndex == 0 && this.txtOthers.Text == string.Empty && this.ddlVisitingCity.SelectedIndex == 0 && this.ddlFacility.SelectedIndex == 0)
                    {
                        this.grdResult.Visible = false;
                        this.lblResult.Visible = true;
                        this.lblResult.Text = "Select atleast one search option";
                    }
                    else
                    {
                        string hostID = Session["LoginID"].ToString();
                        ////Begin Changes for VMS CR17 07Mar2011 Vimal
                        ////Griddata = RequestDetails.SearchVisitorInfo(txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtCompany.Text.Trim(), txtDesignation.Text.Trim(), ddlNativeCountry.SelectedItem.Text.Trim(), txtOthers.Text.Trim(), ddlVisitingCity.SelectedItem.Text, ddlFacility.SelectedItem.Text, null, null, HostID.Trim(), null, null, null, null, null, null,null);
                        this.griddata = this.requestDetails.SearchVisitorInfo(this.txtFirstName.Text.Trim(), this.txtLastName.Text.Trim(), this.txtCompany.Text.Trim(), this.txtDesignation.Text.Trim(), this.ddlNativeCountry.SelectedItem.Text.Trim(), this.txtOthers.Text.Trim(), this.ddlVisitingCity.SelectedItem.Text, this.ddlFacility.SelectedItem.Text, null, null, hostID.Trim(), null, null, null, null, null, null, null, null);
                        ////End Changes for VMS CR17 07Mar2011 Vimal
                        if (this.griddata.Tables[0].Rows.Count > 0)
                        {
                            this.grdResult.Visible = true;
                            this.lblResult.Visible = false;
                            this.grdResult.DataSource = this.griddata;
                            this.grdResult.DataBind();
                        }
                        else
                        {
                            this.lblResult.Visible = true;
                            this.grdResult.Visible = false;
                            this.lblResult.Text = "No Record Found";
                        }
                    }
                }
                else if (this.txtOthers.Visible == false)
                {
                    if (this.txtFirstName.Text.Trim().Equals(string.Empty) && this.txtLastName.Text.Trim().Equals(string.Empty) && this.txtCompany.Text.Trim().Equals(string.Empty) && this.txtDesignation.Text.Trim().Equals(string.Empty) && this.ddlNativeCountry.SelectedIndex == 0 && this.ddlPurpose.SelectedIndex == 0 && this.ddlVisitingCity.SelectedIndex == 0 && this.ddlFacility.SelectedIndex == 0)
                    {
                        this.grdResult.Visible = false;

                        this.lblResult.Visible = true;
                        this.lblResult.Text = "Select atleast one search option";
                    }
                    else
                    {
                        ////Begin Changes for VMS CR17 07Mar2011 Vimal
                        ////string HostID = Session["LoginID"].ToString();
                        ////Griddata = RequestDetails.SearchVisitorInfo(txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtCompany.Text.Trim(), txtDesignation.Text.Trim(), ddlNativeCountry.SelectedItem.Text, ddlPurpose.SelectedItem.Text, ddlVisitingCity.SelectedItem.Text, ddlFacility.SelectedItem.Text, null, null,HostID.Trim(), null, null, null, null, null, null,null);
                        string purpose = string.Empty;
                        if (this.ddlPurpose.SelectedIndex == 0)
                        {
                            purpose = null;
                        }
                        else
                        {
                            purpose = this.ddlPurpose.SelectedItem.Text;
                        }

                        ////End Changes for VMS CR17 07Mar2011 Vimal
                        string hostID = Session["LoginID"].ToString();
                        ////Begin Changes for VMS CR17 07Mar2011 Vimal
                        this.griddata = this.requestDetails.SearchVisitorInfo(this.txtFirstName.Text.Trim(), this.txtLastName.Text.Trim(), this.txtCompany.Text.Trim(), this.txtDesignation.Text.Trim(), this.ddlNativeCountry.SelectedItem.Text, purpose, this.ddlVisitingCity.SelectedItem.Text, this.ddlFacility.SelectedItem.Text, null, null, hostID.Trim(), null, null, null, null, null, null, null, null);
                        ////End Changes for VMS CR17 07Mar2011 Vimal

                        if (this.griddata.Tables[0].Rows.Count > 0)
                        {
                            this.grdResult.Visible = true;
                            this.lblResult.Visible = false;
                            this.lblResult.Visible = false;
                            this.grdResult.DataSource = this.griddata;
                            this.grdResult.DataBind();
                        }
                        else
                        {
                            this.lblResult.Visible = true;
                            this.grdResult.Visible = false;

                            this.lblResult.Text = "No Record Found";
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
        /// Description: Method to Initialize Country, Cities and 
        /// </summary>
        public void InitCountries()
        {
            try
            {
                this.vountries = this.locationDetails.GetCountries();
                this.ddlNativeCountry.DataSource = this.vountries;
                this.ddlNativeCountry.DataBind();
                if ((this.ddlNativeCountry.Items.Count == 0) || (this.ddlNativeCountry.Items.Count > 0))
                {
                    this.ddlNativeCountry.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Cities method
        /// </summary>        
        public void InitCities()
        {
            try
            {
                this.cities = this.locationDetails.GetCountries();
                this.ddlVisitingCity.DataSource = this.cities;
                this.ddlVisitingCity.DataBind();

                if ((this.ddlVisitingCity.Items.Count == 0) || (this.ddlVisitingCity.Items.Count > 0))
                {
                    this.ddlVisitingCity.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Facilities method
        /// </summary>        
        public void InitFacilities()
        {
            try
            {
                DataTable dtfacilities;
                if (!this.ddlVisitingCity.SelectedItem.Text.Equals("Select"))
                {
                    dtfacilities = this.locationDetails.GetActiveFacilities(this.ddlVisitingCity.SelectedItem.Text);
                    this.ddlFacility.DataSource = dtfacilities;
                    this.ddlFacility.DataTextField = "LocationName";
                    this.ddlFacility.DataValueField = "LocationId";
                    this.ddlFacility.DataBind();
                }

                if ((this.ddlFacility.Items.Count == 0) || (this.ddlFacility.Items.Count > 0))
                {
                    this.ddlFacility.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The PopulatePurpose method
        /// </summary>        
        public void PopulatePurpose()
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
                List<string> purpose = new List<string>();
                List<string> purposeDataText = new List<string>();
                string[] purposeListArray;
                purpose = masterDataBL.GetMasterData("Purpose");
                foreach (string purposeData in purpose)
                {
                    purposeListArray = purposeData.ToString().Split('|');
                    purposeDataText.Add(purposeListArray[0]);
                }

                this.ddlPurpose.DataSource = purposeDataText;
                this.ddlPurpose.DataBind();
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
                this.grdResult.Visible = false;
                this.lblResult.Visible = false;
                this.txtFirstName.Focus();
                Page.Form.DefaultButton = this.btnSearch.UniqueID;
                
                if (!Page.IsPostBack)
                {
                    this.txtOthers.Visible = false;

                    this.ddlFacility.SelectedItem.Text = " ";
                    this.InitCountries();
                    this.InitCities();
                    this.PopulatePurpose();
                    this.ddlFacility.Items.Clear();
                    this.ddlFacility.Items.Insert(0, new ListItem("Select", string.Empty));
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Result Row Command method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToString() == "ViewDetailsLink")
                {
                    string str = e.CommandArgument.ToString();
                    
                        Response.Redirect("VMSEnterInformation.aspx?RequestID=" + str, true);
                        
                    
                    
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Visiting City Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlVisitingCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlVisitingCity.SelectedIndex == 0)
                {
                    this.ddlFacility.Items.Clear();
                    this.ddlFacility.Items.Insert(0, "Select");
                }

                if (this.ddlVisitingCity.SelectedIndex != 0)
                {
                    this.InitFacilities();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Search_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.SendSearchParams();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Reset_Click1 method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset_Click1(object sender, EventArgs e)
        {
            ////Response.Redirect("ViewHistorybyHost.aspx");
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtOthers.Visible = false;
            this.txtOthers.Text = string.Empty;
            this.txtCompany.Text = string.Empty;
            this.txtDesignation.Text = string.Empty;
            this.ddlNativeCountry.ClearSelection();
            this.ddlPurpose.ClearSelection();
            this.ddlFacility.ClearSelection();
            this.ddlVisitingCity.ClearSelection();
        }

        /// <summary>
        /// The Purpose Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtOthers.Visible = false;
                if (this.ddlPurpose.SelectedItem.Text == "Others")
                {
                    this.txtOthers.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Result page Index Changing method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdResult.PageIndex = e.NewPageIndex;
                this.SendSearchParams();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
