
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
    /// A page generating safety permit badge
    /// </summary>
    public partial class SPBadge : System.Web.UI.Page
    {
        /// <summary>
        /// The MasterDataBL field
        /// </summary>        
        private VMSBusinessLayer.VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL userDetails = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
            VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();

            try
            {
                if (!Page.IsPostBack)
                {
                    string strRequestId = Convert.ToString(Session["RequestID"]);   ////Session["RequestID"].ToString();;
                    this.imgVisitor.Visible = true;
                    string key = string.Empty;
                    string visitDetailsID = string.Empty;
                    try
                    {
                        key = Request.QueryString["key"].ToString();
                    }
                    catch (NullReferenceException)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseWindow", "window.opener.document.getElementById('ctl00_VMSContentPlaceHolder_ViewLogbySecurity1_btnHidden').click();window.close();", true);
                        return;
                    }

                    DataTable dtvisitDetailsID = new DataTable();
                    dtvisitDetailsID = userDetails.GetVisitDetailsID(strRequestId);
                    if (dtvisitDetailsID.Rows.Count > 0)
                    {
                        visitDetailsID = dtvisitDetailsID.Rows[0]["VisitDetailsID"].ToString();
                    }
                    else
                    {
                        visitDetailsID = key; 
                    }                      
                        
                    string loginID = string.Empty;

                    ////Get Request objects
                    VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    if (!string.IsNullOrEmpty(visitDetailsID))
                    {
                        if (requestDetails != null)
                        {
                            propertiesDC = requestDetails.DisplayInfo(Convert.ToInt32(visitDetailsID));
                            if (propertiesDC != null)
                            {
                                ////Visitor Master Details
                                DataTable dtgetUserDetailsByIdName = new DataTable();
                                string hostid = string.Empty;
                                dtgetUserDetailsByIdName = userDetails.GetUserDetailsByIdName(propertiesDC.VisitorRequestProperty.HostID.ToString(), hostid);
                                if (dtgetUserDetailsByIdName.Rows.Count > 0)
                                {
                                    hostid = dtgetUserDetailsByIdName.Rows[0]["AssociateIdName"].ToString();
                                }

                                this.lblName.Text = string.Concat(propertiesDC.VisitorMasterProperty.FirstName.ToString(), ", ", Convert.ToString(propertiesDC.VisitorMasterProperty.LastName)).ToUpper();
                                this.lblOrganisation.Text = propertiesDC.VisitorMasterProperty.Company.ToString();

                                ////Visitor Request Details
                                this.lblPurpose.Text = propertiesDC.VisitorRequestProperty.Purpose.ToString();
                                this.lblHostName.Text = XSS.HtmlEncode(hostid.ToString()); // propertiesDC.VisitorRequestProperty.HostID.ToString();
                                this.lblDateTimeIngen.InnerText = ((DateTime)propertiesDC.VisitorRequestProperty.FromDate).ToString("dd-MMM-yyyy", provider);
                                DateTime fromTime = DateTime.Parse(propertiesDC.VisitorRequestProperty.FromTime.Value.ToString(), provider);
                                this.lblTimIngen.InnerText = string.Concat(fromTime.ToString("HH:mm"), " Hrs");
                                this.lblToDateTimeIngen.InnerText = ((DateTime)propertiesDC.VisitorRequestProperty.ToDate).ToString("dd-MMM-yyyy", provider);
                                DateTime totime = DateTime.Parse(propertiesDC.VisitorRequestProperty.ToTime.Value.ToString(), provider);
                                this.lblTimOutIngen.InnerText = string.Concat(totime.ToString("HH:mm"), " Hrs");
                                this.lblBuddyIdValue.InnerText = Convert.ToString(propertiesDC.VisitorRequestProperty.BuddyId);
                                this.lblPermitTypeValue.InnerText = Convert.ToString(propertiesDC.VisitorRequestProperty.SafetyPermitType);
                                this.lblRequestIdValue.Text = Convert.ToString(propertiesDC.VisitorRequestProperty.PermitId);
                                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestInfo =
                               new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();

                                DataTable dtlocationDetails = requestInfo.GetLocationDetailsById(
                                                    propertiesDC.VisitorRequestProperty.RequestID);
                                if (dtlocationDetails.Rows.Count > 0)
                                {
                                    this.lblCity.Text = Convert.ToString(dtlocationDetails.Rows[0]["City"]);
                                    this.lblFacility.Text = Convert.ToString(dtlocationDetails.Rows[0]["Facility"]);
                                    string vnet = string.Empty;
                                    vnet = requestDetails.GetFacilityVnet(Convert.ToInt32(dtlocationDetails.Rows[0]["LocationId"]));
                                    this.lblsecurityno.Text = XSS.HtmlEncode(vnet);
                                }

                                DataTable dt = new DataTable();
                                VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
                                string strHostID = propertiesDC.VisitorRequestProperty.HostID.ToString();
                                if (strHostID.Contains("("))
                                {
                                    int startIndex = strHostID.IndexOf("(") + 1;
                                    string userID = strHostID.Substring(startIndex, strHostID.Length - (startIndex + 1)).ToString();                                    
                                    dt = userDetailsBL.GetCRSAssociateDetails(userID);
                                }
                                else
                                {
                                    dt = userDetailsBL.GetCRSAssociateDetails(strHostID);
                                }

                                string strHostEmailId = string.Empty;
                                if (dt != null)
                                {
                                    strHostEmailId = dt.Rows[0]["EmailID"].ToString().Trim();
                                }

                                string badgeNo = string.Empty;
                                if (string.IsNullOrEmpty(propertiesDC.VisitDetailProperty.BadgeNo))
                                {
                                    ////BadgeNo = RequestDetails.GenerateBadge(Convert.ToInt32(VisitDetailsID), strHostEmailId, propertiesDC.VisitorProofProperty, propertiesDC.VisitorMasterProperty, propertiesDC.VisitorRequestProperty, null, propertiesDC.VisitorEmergencyContactProperty, Session["LoginID"].ToString());
                                    badgeNo = Convert.ToString(propertiesDC.VisitorRequestProperty.ExternalRequestId);
                                }
                                else
                                {
                                    ////BadgeNo = propertiesDC.VisitDetailProperty.BadgeNo;
                                    badgeNo = Convert.ToString(propertiesDC.VisitorRequestProperty.ExternalRequestId);
                                }

                                this.lblbadge.Text = badgeNo;
                                this.lblbadge2.Text = badgeNo;

                                ////if (propertiesDC.VisitorProofProperty != null)
                                ////{
                                //    string encryptedVisitorID = VMSBusinessLayer.VMSBusinessLayer.Encrypt(propertiesDC.VisitorMasterProperty.VisitorID.ToString().Trim());
                                //    imgVisitor.ImageUrl = string.Concat("EmployeeImage.aspx?key=", encryptedVisitorID);
                                ////}
                                if (propertiesDC.VisitorProofProperty != null)
                                {
                                    string encryptedVisitorID = VMSBusinessLayer.VMSBusinessLayer.Encrypt(propertiesDC.VisitorMasterProperty.VisitorID.ToString().Trim());
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

                                    this.GridView1.DataSource = dttable;
                                    this.GridView1.DataBind();
                                    this.lblNoEquipment.Visible = false;
                                }
                                else
                                {
                                    this.lblNoEquipment.Visible = true;
                                }

                                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "TimeConversion", "TimeConversion();", true);
                            }
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
