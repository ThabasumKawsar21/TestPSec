
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using VMSBusinessEntity;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial class generate badge
    /// </summary>
    public partial class GenerateBadge : System.Web.UI.Page
    {
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.ToString().Contains("key="))
            {
                VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                VMSBusinessLayer.VMSBusinessLayer.UserDetails<string, string, string, string> userDetails = new VMSBusinessLayer.VMSBusinessLayer.UserDetails<string, string, string, string>();
                VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();

                int viewDetailsID = Convert.ToInt32(Request.QueryString["key"].ToString().Trim());
                propertiesDC = requestDetails.DisplayInfo(viewDetailsID);
                this.lblHostName.Text = propertiesDC.VisitorRequestProperty.HostID.ToString();

                if (propertiesDC.VisitorProofProperty != null)
                {
                    string encryptedVisitorID = VMSBusinessLayer.VMSBusinessLayer.Encrypt(propertiesDC.VisitorMasterProperty.VisitorID.ToString().Trim());
                    this.Image2.ImageUrl = string.Concat("EmployeeImage.aspx?key=", encryptedVisitorID);
                }
                else
                {
                    this.Image2.ImageUrl = "~\\Images\\DummyPhoto.png";
                }

                int gridRowCount = 0;
                List<VisitorEquipment> visitorEquipmentList = propertiesDC.VisitorEquipmentProperty;
                ////if (VisitorEquipmentList != null && VisitorEquipmentList.Count > 0)
                if (visitorEquipmentList != null)
                {
                    DataTable dt = this.CreateDataTable();
                    DataRow dr;
                    VisitorEquipment objVisitorEquip;
                    ////for (int iVisitor = VisitorEquipmentList.Count - 1; iVisitor >= 0; iVisitor--)
                    for (int ivisitor = 0; ivisitor <= visitorEquipmentList.Count - 1; ivisitor++)
                    {
                        string equipment = string.Empty;
                        objVisitorEquip = visitorEquipmentList[ivisitor];
                        dr = dt.NewRow();
                        //////dr["RowNumber"] = iVisitor;

                        if (objVisitorEquip.MasterDataID == 12)
                        {
                            equipment = "Laptop";
                        }

                        if (objVisitorEquip.MasterDataID == 13)
                        {
                            equipment = "Data Storage Device";
                        }

                        if (objVisitorEquip.MasterDataID == 14)
                        {
                            equipment = "USB Hard Disk";
                        }

                        if (objVisitorEquip.MasterDataID == 29)
                        {
                            equipment = "Camera";
                        }

                        if (objVisitorEquip.MasterDataID == 30)
                        {
                            equipment = "I Pod";
                        }

                        if (objVisitorEquip.MasterDataID == 22)
                        {
                            equipment = "Select";
                        }
                        else if (!string.IsNullOrEmpty(objVisitorEquip.Others.ToString()))
                        {
                            equipment = objVisitorEquip.Others.ToString();
                        }

                        dr["EquipmentType"] = equipment;
                        dr["Make"] = objVisitorEquip.Make;
                        dr["Model"] = objVisitorEquip.Model;
                        dr["SerialNo"] = objVisitorEquip.SerialNo;
                        ////dr["Others"] = objVisitorEquip.Others;
                        dt.Rows.Add(dr);
                    }

                    this.ViewState["CurrentTable"] = dt;
                    this.GridView1.DataSource = dt;
                    gridRowCount = dt.Rows.Count;
                    this.GridView1.DataBind();
                    this.GridView1.RowStyle.BorderColor = System.Drawing.Color.White;
                    this.GridView1.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;

                    VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestInfo =
                           new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();

                    DataTable dtlocationDetails = requestInfo.GetLocationDetailsById(
                        propertiesDC.VisitorRequestProperty.RequestID);
                    if (dtlocationDetails.Rows.Count > 0)
                    {
                        string vnet = string.Empty;
                        vnet = requestDetails.GetFacilityVnet(Convert.ToInt32(dtlocationDetails.Rows[0]["LocationId"]));
                        this.lblsecurityno.Text = XSS.HtmlEncode(vnet);
                    }
                }
            }
        }

        /// <summary>
        /// The CreateDataTable method
        /// </summary>
        /// <returns>The System.Data.DataTable type object</returns>        
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            ////dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("EquipmentType", typeof(string)));
            dt.Columns.Add(new DataColumn("Make", typeof(string)));
            dt.Columns.Add(new DataColumn("Model", typeof(string)));
            dt.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            ////dt.Columns.Add(new DataColumn("Others", typeof(string)));
            return dt;
        }

        /// <summary>
        /// The UserDetails class
        /// </summary>   
        /// <typeparam name="T">T parameter</typeparam>
        /// <typeparam name="U">U parameter</typeparam>
        /// <typeparam name="V">V parameter</typeparam>
        /// <typeparam name="S">S parameter</typeparam>
        public class UserDetails<T, U, V, S>
        {
            /// <summary>
            /// Gets or sets AssociateName field
            /// </summary>            
            public T AssociateName
            {
                get;
                set;
            }
            
            /// <summary>
            /// Gets or sets V net field
            /// </summary>            
            public U Vnet
            {
                get;
                set;
            }
            
            /// <summary>
            /// Gets or sets Department Description field
            /// </summary>            
            public V DeptDesc
            {
                get;
                set;
            }
            
            /// <summary>
            /// Gets or sets MailID field
            /// </summary>            
            public S MailID
            {
                get;
                set;
            }
        }        
    }
}
