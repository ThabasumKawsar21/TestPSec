

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessEntity;
    using VMSBusinessLayer;

    /// <summary>
    /// VMS badge partial class
    /// </summary>
    public partial class VMSBadge : System.Web.UI.Page
    {
        /// <summary>
        /// The MasterDataBL field
        /// </summary>        
        private VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.MasterDataBL();

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>          
        protected void Page_Load(object sender, EventArgs e)
        {
            int intTokenNumber = Convert.ToInt32(ConfigurationManager.AppSettings["TokenNumber"].ToString());

            VMSBusinessLayer.UserDetailsBL userDetails =
                new VMSBusinessLayer.UserDetailsBL();
            VMSBusinessLayer.RequestDetailsBL requestDetails =
                new VMSBusinessLayer.RequestDetailsBL();
            try
            {
                GenericTimeZone genTimeZone = new GenericTimeZone();
                DateTime today = genTimeZone.GetCurrentDate();
                DateTime dttoday = Convert.ToDateTime(today.ToShortDateString());
                if (!Page.IsPostBack)
                {
                    this.imgVisitor.Visible = true;
                    string key = string.Empty;
                    string visitDetailsID;
                    try
                    {
                        key = Request.QueryString["key"].ToString();
                    }
                    catch (NullReferenceException)
                    {
                        ////Response.Redirect("ErrorPage.aspx");                        
                        ////Parent.Page.Response.Redirect("ErrorPage.aspx");
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseWindow", "window.opener.document.getElementById('ctl00_VMSContentPlaceHolder_ViewLogbySecurity1_btnHidden1').click();window.close();", true);
                        return;
                    }
                    ////  VisitDetailsID = System.Text.Encoding.Unicode.GetString((Convert.FromBase64String(key)));
                    visitDetailsID = key;
                    string loginID = string.Empty;

                    ////Get Request objects
                    VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC =
                        new VMSDataLayer.VMSDataLayer.PropertiesDC();
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    if (!string.IsNullOrEmpty(visitDetailsID))
                    {
                        if (requestDetails != null)
                        {
                            propertiesDC = requestDetails.DisplayInfo(Convert.ToInt32(visitDetailsID));
                            if (propertiesDC != null)
                            {
                                string offset = propertiesDC.VisitorRequestProperty.Offset;
                                ////Visitor Master Details
                                this.lblName.Text = string.Concat(
                                Convert.ToString(propertiesDC.VisitorMasterProperty.FirstName),
                                " ",
                                    Convert.ToString(propertiesDC.VisitorMasterProperty.LastName)).ToUpper();
                                this.lblOrganisation.Text = Convert.ToString(propertiesDC.VisitorMasterProperty.Company);

                                ////Visitor Request Details
                                this.lblPurpose.Text = Convert.ToString(propertiesDC.VisitorRequestProperty.Purpose);
                                this.lblHostName.Text = Convert.ToString(propertiesDC.VisitorRequestProperty.HostID);
                                if (propertiesDC.EquipmentCustodyProperty.Count > 0)
                                {
                                    string vmsindate = genTimeZone.ToLocalTimeZone(Convert.ToDateTime(dttoday), offset);
                                    this.lblDateTimeIngen.InnerText = Convert.ToDateTime(vmsindate).ToString("dd-MMM-yyyy");
                                    this.lblToDateTimeIngen.InnerText = Convert.ToDateTime(vmsindate).ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    //string vmsfromdate = genTimeZone.ToLocalTimeZone(Convert.ToDateTime(propertiesDC.VisitorRequestProperty.FromDate + propertiesDC.VisitorRequestProperty.FromTime), offset);
                                    //string vmstodate = genTimeZone.ToLocalTimeZone(Convert.ToDateTime(propertiesDC.VisitorRequestProperty.ToDate + propertiesDC.VisitorRequestProperty.ToTime), offset);
                                    //597397 Badge generation bug fix
                                    string vmsfromdate = Convert.ToDateTime(propertiesDC.VisitorRequestProperty.FromDate + propertiesDC.VisitorRequestProperty.FromTime).ToString();
                                    string vmstodate = Convert.ToDateTime(propertiesDC.VisitorRequestProperty.ToDate + propertiesDC.VisitorRequestProperty.ToTime).ToString();
                                    this.lblDateTimeIngen.InnerText = Convert.ToDateTime(vmsfromdate).ToString("dd-MMM-yyyy", provider);
                                    this.lblToDateTimeIngen.InnerText = Convert.ToDateTime(vmstodate).ToString("dd-MMM-yyyy", provider);
                                }

                                //string vmsfromtime = genTimeZone.ToLocalTimeZone(Convert.ToDateTime(propertiesDC.VisitorRequestProperty.FromDate + propertiesDC.VisitorRequestProperty.FromTime), offset);
                                //597397 Badge generation bug fix
                                string vmsfromtime = Convert.ToDateTime(propertiesDC.VisitorRequestProperty.FromDate + propertiesDC.VisitorRequestProperty.FromTime).ToString();
                                DateTime fromTime = DateTime.Parse(vmsfromtime, provider);
                                this.lblTimIngen.InnerText = string.Concat(fromTime.ToString("HH:mm"), " Hrs");
                                ////string.Concat(FromTime.ToString("HH:mm"), " Hrs");

                               // string vmstotime = genTimeZone.ToLocalTimeZone(Convert.ToDateTime(propertiesDC.VisitorRequestProperty.ToDate + propertiesDC.VisitorRequestProperty.ToTime), offset);
                                //597397 Badge generation bug fix
                                string vmstotime = Convert.ToDateTime(propertiesDC.VisitorRequestProperty.ToDate + propertiesDC.VisitorRequestProperty.ToTime).ToString();
                                DateTime totime = DateTime.Parse(vmstotime.ToString(), provider);
                                this.lblTimOutIngen.InnerText = string.Concat(totime.ToString("HH:mm"), " Hrs");

                                VMSBusinessLayer.RequestDetailsBL requestInfo =
                                new VMSBusinessLayer.RequestDetailsBL();

                                DataTable dtlocationDetails = requestInfo.GetLocationDetailsById(propertiesDC.VisitorRequestProperty.RequestID);
                                if (dtlocationDetails.Rows.Count > 0)
                                {
                                    ////lblCity.Text = Convert.ToString(dtLocationDetails.Rows[0]["City"]);
                                    this.lblFacility.Text = Convert.ToString(dtlocationDetails.Rows[0]["Facility"]);
                                    string vnet = string.Empty;
                                    vnet = requestDetails.GetFacilityVnet(Convert.ToInt32(dtlocationDetails.Rows[0]["LocationId"]));
                                    this.lblsecurityno.Text = vnet;
                                }

                                VMSBusinessLayer.UserDetailsBL userDetailsBL =
                                    new VMSBusinessLayer.UserDetailsBL();

                                string strHostID = propertiesDC.VisitorRequestProperty.HostID.ToString().Split('(')[1].Split(')')[0].ToString();
                                DataTable dt = userDetailsBL.GetCRSAssociateDetails(strHostID);
                                string strHostEmailId = string.Empty;
                                if (dt != null)
                                {
                                    strHostEmailId = Convert.ToString(dt.Rows[0]["EmailID"]);
                                }

                                string badgeNo = string.Empty;
                                if (string.IsNullOrEmpty(propertiesDC.VisitDetailProperty.BadgeNo))
                                {
                                    badgeNo = requestDetails.GenerateBadge(Convert.ToInt32(visitDetailsID), strHostEmailId, propertiesDC.VisitorProofProperty, propertiesDC.VisitorMasterProperty, propertiesDC.VisitorRequestProperty, null, propertiesDC.VisitorEmergencyContactProperty, Session["LoginID"].ToString());
                                }
                                else
                                {
                                    badgeNo = propertiesDC.VisitDetailProperty.BadgeNo;
                                }

                                this.lblbadge.Text = badgeNo;
                                this.lblbadge2.Text = badgeNo;

                                if (propertiesDC.VisitorProofProperty != null)
                                {
                                    string encryptedVisitorID = VMSBusinessLayer.Encrypt(propertiesDC.VisitorMasterProperty.VisitorID.ToString().Trim());
                                    this.imgVisitor.ImageUrl = string.Concat("EmployeeImage.aspx?key=", encryptedVisitorID);
                                }
                                else
                                {
                                    this.imgVisitor.ImageUrl = "~\\Images\\DummyPhoto.png";
                                }

                                List<VisitorEquipment> visitorEquipmentList = propertiesDC.VisitorEquipmentProperty;
                                if (visitorEquipmentList != null && visitorEquipmentList.Count > 0)
                                {
                                    DataTable dttable = this.CreateDataTable();
                                    DataRow dr;
                                    VisitorEquipment objVisitorEquip;
                                    for (int ivisitor = 0; ivisitor <= visitorEquipmentList.Count - 1; ivisitor++)
                                    {
                                        objVisitorEquip = visitorEquipmentList[ivisitor];
                                        dr = dttable.NewRow();
                                        dr["RowNumber"] = ivisitor;
                                        dr["EquipmentType"] = this.GetEquipment(Convert.ToInt32(objVisitorEquip.MasterDataID));
                                        dr["Make"] = objVisitorEquip.Make;
                                        dr["Model"] = objVisitorEquip.Model;
                                        dr["SerialNo"] = objVisitorEquip.SerialNo;
                                        dr["Others"] = objVisitorEquip.Others;
                                        dttable.Rows.Add(dr);
                                    }
                                    //// ViewState["CurrentTable"] = dtTable;
                                    this.GridView1.DataSource = dttable;
                                    this.GridView1.DataBind();
                                    this.lblNoEquipment.Visible = false;
                                    this.lblEquipmentCust.Visible = false;
                                    this.DisclaimerFirst.Visible = false;
                                    this.DisclaimerNext.Visible = false;
                                    this.imgToken.Visible = false;
                                }
                                else
                                {
                                    int tokenNumber = 0;
                                    DataTable dtequip = new DataTable();
                                    VMSBusinessLayer.UserDetailsBL userDetailsBLObj = new VMSBusinessLayer.UserDetailsBL();
                                    dtequip = userDetailsBLObj.GetEquipmentInCustody(Convert.ToInt32(visitDetailsID));

                                    if (dtequip != null && dtequip.Rows.Count != 0)
                                    {
                                        tokenNumber = Convert.ToInt32(dtequip.Rows[0]["TokenNumber"]);
                                        if (tokenNumber > intTokenNumber)
                                        {
                                            tokenNumber = tokenNumber - intTokenNumber;
                                        }

                                        this.lblToken.Text = tokenNumber.ToString();
                                        this.Disclaimer.Attributes.Add("display", "block");
                                        List<tblEquipmentsInCustody> visitorEquipmentCustodyList = propertiesDC.EquipmentCustodyProperty;
                                        if (visitorEquipmentCustodyList != null && visitorEquipmentCustodyList.Count > 0)
                                        {
                                            DataTable dttable = this.CreateDataTableForEquipmentCustody();
                                            DataRow dr;
                                            tblEquipmentsInCustody objVisitorEquip;
                                            for (int ivisitor = 0; ivisitor <= visitorEquipmentCustodyList.Count - 1; ivisitor++)
                                            {
                                                objVisitorEquip = visitorEquipmentCustodyList[ivisitor];
                                                dr = dttable.NewRow();
                                                dr["RowNumber"] = ivisitor;
                                                dr["EquipmentType"] = objVisitorEquip.EquipmentType;
                                                dr["Description"] = objVisitorEquip.Description;
                                                dr["EquipmentTypeId"] = objVisitorEquip.EquipmentID;
                                                dttable.Rows.Add(dr);
                                            }

                                            this.grdEquipments.DataSource = dttable;
                                            this.grdEquipments.DataBind();
                                            this.grdEquipments.Visible = true;
                                            this.lblEquipmentCust.Visible = true;
                                            this.lblNoEquipment.Visible = false;
                                            this.Label17.Visible = false;
                                            this.DisclaimerFirst.Visible = true;
                                            this.DisclaimerNext.Visible = true;
                                            this.imgToken.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        this.lblEquipmentCust.Visible = false;
                                        this.lblToken.Text = "--";
                                        this.lblToken.Visible = false;
                                        this.lblNoEquipment.Visible = true;
                                        this.Label17.Visible = true;
                                    }
                                }

                            }
                        }
                    }

                    this.Focus();
                    this.SetFocus(this.imgVisitor);
                    this.SetFocus(this.lblFacility);
                    this.SetFocus(this.lblbadge);
                    this.SetFocus(this.lblHostName);
                    this.SetFocus(this.Label2);
                    this.SetFocus(this.Label23);
                }

                this.Focus();
                this.SetFocus(this.imgVisitor);
                this.SetFocus(this.lblFacility);
                ////this.SetFocus(lblCity);
                this.SetFocus(this.lblbadge);
                this.SetFocus(this.lblHostName);
                this.SetFocus(this.Label2);
                this.SetFocus(this.Label23);
            }
            catch (System.IndexOutOfRangeException)
            {
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            this.Focus();
            this.SetFocus(this.imgVisitor);
            this.SetFocus(this.lblFacility);
            ////this.SetFocus(lblCity);
            this.SetFocus(this.lblbadge);
            this.SetFocus(this.lblHostName);
            this.SetFocus(this.Label2);
            this.SetFocus(this.Label23);
        }

        /// <summary>
        /// The CreateDataTable method
        /// </summary>
        /// <returns>The System.Data.DataTable type object</returns>        
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("EquipmentType", typeof(string)));
            dt.Columns.Add(new DataColumn("Make", typeof(string)));
            dt.Columns.Add(new DataColumn("Model", typeof(string)));
            dt.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Others", typeof(string)));
            return dt;
        }

        /// <summary>
        /// The CreateDataTableForEquipmentCustody method
        /// </summary>
        /// <returns>The System.Data.DataTable type object</returns>        
        private DataTable CreateDataTableForEquipmentCustody()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("EquipmentType", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("EquipmentTypeId", typeof(string)));
            return dt;
        }

        /// <summary>
        /// The GetEquipment method
        /// </summary>
        /// <param name="value">The value parameter</param>
        /// <returns>The string type object</returns>        
        private string GetEquipment(int value)
        {
            string strEquipment = string.Empty;
            List<string> equipmentType = new List<string>();
            try
            {
                if (this.masterDataBL != null)
                {
                    equipmentType = this.masterDataBL.GetMasterData("Equipment");
                    foreach (string li in equipmentType)
                    {
                        if (li.Contains(value.ToString()))
                        {
                            strEquipment = li.Split('|')[0];
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return strEquipment;
        }
    }
}
