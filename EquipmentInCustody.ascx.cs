
namespace VMSDev.UserControls
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
    using VMSBusinessEntity;
    using VMSBusinessLayer;

    /// <summary>
    /// To handle Equipment in Custody functionality
    /// </summary>
    public partial class EquipmentInCustody : System.Web.UI.UserControl
    {
        /// <summary>
        /// The GridRowCount field
        /// </summary>        
        private int gridRowCount = 1;
        
        /// <summary>
        /// The Token Number field
        /// </summary>        
        private int intTokenNumber = Convert.ToInt32(ConfigurationManager.AppSettings["TokenNumber"]);
        
        /// <summary>
        /// Set Initial Row for Equipment Type
        /// </summary>
        public void SetInitialRow()
        {
            DataTable dt = this.CreateDataTable();

            DataRow dr = dt.NewRow();
            dr["RowNumber"] = string.Empty;

            dr["EquipmentType"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["EquipmentTypeId"] = string.Empty;

            dt.Rows.Add(dr);
            this.ViewState["CurrentTable"] = dt;

            this.grdEquipments.DataSource = dt;
            this.grdEquipments.DataBind();

            if (this.grdEquipments.Rows.Count > 0)
            {
                ImageButton deletebut = (ImageButton)this.grdEquipments.Rows[0].FindControl("ButtonDelete");
                if (this.grdEquipments.Rows.Count > 1)
                {
                    deletebut.Enabled = true;
                }
                else
                {
                    deletebut.Enabled = false;
                }
            }

            this.grdEquipments.Visible = true;
            ////this.Visible = true;
        }
        
        /// <summary>
        /// The ShowEquipmentInformationByVisitDetailsID method
        /// </summary>
        /// <param name="propertiesDc">The propertiesDc parameter</param>        
        public void ShowEquipmentInformationByVisitDetailsID(VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDc)
        {
            int tokenNumber = 0;
            List<tblEquipmentsInCustody> visitorEquipmentCustodyList = propertiesDc.EquipmentCustodyProperty;
            if (visitorEquipmentCustodyList != null && visitorEquipmentCustodyList.Count > 0)
            {
                GenericTimeZone genTimeZone = new GenericTimeZone();
                DateTime dttoday = genTimeZone.GetCurrentDate();
                ////DateTime dtToday = DateTime.Today;
                DateTime dateOnly = dttoday.Date;
                DataTable dt = this.CreateDataTable();
                DataRow dr;
                tblEquipmentsInCustody objVisitorEquip;

                ////for (int iVisitor = VisitorEquipmentList.Count - 1; iVisitor >= 0; iVisitor--)

                for (int ivisitor = 0; ivisitor <= visitorEquipmentCustodyList.Count - 1; ivisitor++)
                {
                    if (dateOnly == Convert.ToDateTime(visitorEquipmentCustodyList[ivisitor].CreatedDate).Date)
                   {
                        objVisitorEquip = visitorEquipmentCustodyList[ivisitor];
                        dr = dt.NewRow();
                        dr["RowNumber"] = ivisitor;
                        dr["EquipmentType"] = objVisitorEquip.EquipmentType;
                        dr["Description"] = objVisitorEquip.Description;
                        dr["EquipmentTypeId"] = objVisitorEquip.EquipmentID;
                        tokenNumber = objVisitorEquip.TokenNumber;
                        ////dr["Model"] = objVisitorEquip.Model;
                        ////dr["SerialNo"] = objVisitorEquip.SerialNo;
                        ////dr["Others"] = objVisitorEquip.Others;

                        dt.Rows.Add(dr);
                    }

                    this.ViewState["CurrentTable"] = dt;
                    this.grdEquipments.DataSource = dt;
                    this.gridRowCount = dt.Rows.Count;
                    this.grdEquipments.DataBind();
                    if (Convert.ToInt32(tokenNumber) > this.intTokenNumber)
                    {
                        tokenNumber = tokenNumber - this.intTokenNumber;
                    }

                    this.txttoken.Text = Convert.ToString(tokenNumber);
                    this.txttoken.Visible = true;
                    this.lbltokenNumber.Visible = true;
                }
            }
            else
            {
                this.SetInitialRow();
            }
        }
        
        /// <summary>
        /// The InsertEquipmentCustodyInformation method
        /// </summary>
        /// <param name="visitDetailsID">The VisitDetailsID parameter</param>
        /// <param name="locationID">The locationID parameter</param>
        /// <param name="tokenNumber">The tokenNumber parameter</param>
        /// <returns>The Business Entity type object</returns>        
        public VMSBusinessEntity.tblEquipmentsInCustody[] InsertEquipmentCustodyInformation(int visitDetailsID, string locationID, int tokenNumber)
        {
            ////int tokenNumber = 0;
            DataTable dttokenDetails = new DataTable();
            VMSBusinessLayer.UserDetailsBL userDetailsBLObj = new VMSBusinessLayer.UserDetailsBL();
            VMSBusinessEntity.tblEquipmentsInCustody[] arrayOfVisitorEquipmentCustodyObj = null;
            VMSBusinessEntity.tblEquipmentsInCustody visitorEquipmentCustodyObj;
            ArrayList ret = new ArrayList();

            ////get token details
            if (!Session["UpdateFlag"].Equals(true))
            {
                DropDownList ddlEquipmentType = (DropDownList)this.grdEquipments.Rows[0].Cells[1].FindControl("ddlEquipmentType");
                if (ddlEquipmentType.SelectedItem.Text.ToUpper() != "SELECT")
                {
                    tokenNumber = userDetailsBLObj.GetToken(visitDetailsID, locationID);
                }
                ////dtTokenDetails = UserDetailsBLObj.getTokenDetails(Convert.ToInt32(Session["VisitDetailID"]));
                ////if (dtTokenDetails.Rows.Count != 0)
                ////{
                //    tokenNumber = Convert.ToInt32(dtTokenDetails.Rows[0]["TokenNumber"]);
                   
                ////}
                ////else
                ////{
                //    tokenNumber = UserDetailsBLObj.GetToken(VisitDetailsID, locationID);
                    
                ////}
            }
            ////else
            ////{
               
            ////}

            if (this.ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DropDownList ddlEquipmentType = (DropDownList)this.grdEquipments.Rows[i].FindControl("ddlEquipmentType");
                    TextBox txtDescription = (TextBox)this.grdEquipments.Rows[i].FindControl("txtDescription");

                    dt.Rows[i]["RowNumber"] = i;
                    if (ddlEquipmentType.SelectedIndex > 0)
                    {
                        dt.Rows[i]["EquipmentType"] = ddlEquipmentType.SelectedItem.Text;
                        dt.Rows[i]["EquipmentTypeId"] = ddlEquipmentType.SelectedValue;
                    }
                    else
                    {
                        dt.Rows[i]["EquipmentType"] = string.Empty;
                        dt.Rows[i]["EquipmentTypeId"] = 0;
                    }

                    dt.Rows[i]["Description"] = txtDescription.Text.Trim();
                }

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EquipmentType"])) && Convert.ToString(dt.Rows[i]["EquipmentType"]) != "22")
                    {
                        visitorEquipmentCustodyObj = new VMSBusinessEntity.tblEquipmentsInCustody();

                        visitorEquipmentCustodyObj.VisitDetailsID = visitDetailsID;
                        ////if (Convert.ToString(dt.Rows[i]["EquipmentType"]) != String.Empty)
                        //    MasterDataID = Convert.ToInt32(dt.Rows[i]["EquipmentTypeId"]);

                        ////VisitorEquipmentCustodyObj.MasterDataID = MasterDataID;

                        ////if (MasterDataID == 31) //Added for CR 17 Fix,12 May 2011,Others=31

                        visitorEquipmentCustodyObj.EquipmentType = Convert.ToString(dt.Rows[i]["EquipmentType"]);
                        visitorEquipmentCustodyObj.Description = Convert.ToString(dt.Rows[i]["Description"]);
                        visitorEquipmentCustodyObj.Status = "Issued";
                        visitorEquipmentCustodyObj.VisitDetailsID = visitDetailsID;
                        visitorEquipmentCustodyObj.FacilityID = locationID;
                        visitorEquipmentCustodyObj.TokenNumber = tokenNumber;
                        visitorEquipmentCustodyObj.EquipmentID = Convert.ToInt32(dt.Rows[i]["EquipmentTypeId"]);
                        this.Session["EquipmentCustody"] = true;

                        ret.Add(visitorEquipmentCustodyObj);
                    }
                }

                arrayOfVisitorEquipmentCustodyObj = new VMSBusinessEntity.tblEquipmentsInCustody[ret.Count];
                ret.CopyTo(0, arrayOfVisitorEquipmentCustodyObj, 0, ret.Count);
            }

            return arrayOfVisitorEquipmentCustodyObj;
        }
        
        /// <summary>
        /// The EnableEquipmentControl method
        /// </summary>        
        public void EnableEquipmentControl()
        {
            this.Visible = true;

            this.grdEquipments.Visible = true;

            for (int i = 0; i <= this.grdEquipments.Rows.Count - 1; i++)
            {
                DropDownList ddlEquipmentType = (DropDownList)this.grdEquipments.Rows[i].Cells[1].FindControl("ddlEquipmentType");
                TextBox txtDescription = (TextBox)this.grdEquipments.Rows[i].Cells[1].FindControl("txtDescription");

                ddlEquipmentType.Enabled = true;

                if (ddlEquipmentType.SelectedItem.Text.ToUpper() != "SELECT")
                {
                    txtDescription.Enabled = true;
                    txtDescription.Visible = true;
                }
                else
                {
                    txtDescription.Visible = true;
                }

                ((ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonAdd")).Enabled = true;
                if (this.grdEquipments.Rows.Count == 1)
                {
                    ((ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonDelete")).Enabled = false;
                }
                else
                {
                    ((ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonDelete")).Enabled = true;
                }
            }
        }

        /// <summary>
        /// The PopulateEquipmentType method
        /// </summary>
        /// <param name="ddlEquipmentType">The EquipmentType parameter</param>        
        public void PopulateEquipmentType(DropDownList ddlEquipmentType)
        {
            try
            {
                VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.MasterDataBL();
                DataTable dtequipments = new DataTable();
                List<string> equipmentType = new List<string>();
                string[] equipmentTypeListArray;
                equipmentType = masterDataBL.GetMasterData("Equipment");

                dtequipments.Columns.Add("EquipmentId");
                dtequipments.Columns.Add("EquipmentName");

                for (int i = 0; i <= equipmentType.Count - 1; i++)
                {
                    equipmentTypeListArray = equipmentType[i].ToString().Split('|');
                    DataRow drequipment = dtequipments.NewRow();
                    drequipment["EquipmentId"] = equipmentTypeListArray[1].ToString();
                    drequipment["EquipmentName"] = equipmentTypeListArray[0].ToString();
                    dtequipments.Rows.Add(drequipment);
                }

                ddlEquipmentType.DataSource = dtequipments;
                ddlEquipmentType.DataTextField = "EquipmentName";
                ddlEquipmentType.DataValueField = "EquipmentId";
                ddlEquipmentType.DataBind();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                ////Utility.VMSUtility.logExceptionAndShowErrorPage(ex, HttpContext.Current);
                ////ExceptionLogger.OneC_ExceptionLogger(ex, this.Page);
            }
        }

        ////protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        ////protected void ButtonAdd_Click(object sender, EventArgs e)
        ////{
        //    try
        //    {
        //        int index = Convert.ToInt16(e.RowIndex);
        //        DeleteRowFromTable(index);

        ////        if (grdEquipments.Rows.Count == 1)
        //        {
        //            for (int i = 0; i < grdEquipments.Rows.Count; i++)
        //            {
        //                ImageButton imgBtnDelete = (ImageButton)grdEquipments.Rows[i].FindControl("ButtonDelete");
        //                imgBtnDelete.Enabled = false;
        //                imgBtnDelete.ImageUrl = "images/IT_equip2_disabled.png";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Utility.VMSUtility.logExceptionAndShowErrorPage(ex, HttpContext.Current);
        //        //ExceptionLogger.OneC_ExceptionLogger(ex, this.Page);
        //    }
        ////}

        /// <summary>
        /// The DisableEquipmentCustody method
        /// </summary>
        /// <param name="requestStatus">The RequestStatus parameter</param>
        /// <param name="badgeStatus">The BadgeStatus parameter</param>        
        public void DisableEquipmentCustody(string requestStatus, string badgeStatus)
        {
            ////changed by priti on 3rd June for VMS CR VMS31052010CR6
            if ((badgeStatus == "ISSUED") || (badgeStatus == "RETURNED") ||
                    (badgeStatus == "LOST") || (requestStatus == "CANCELLED"))
            {
                for (int i = 0; i <= this.grdEquipments.Rows.Count - 1; i++)
                {
                    ((DropDownList)this.grdEquipments.Rows[i].FindControl("ddlEquipmentType")).Enabled = false;
                    ((TextBox)this.grdEquipments.Rows[i].FindControl("txtDescription")).Enabled = false;
                    ((ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonDelete")).Enabled = false;
                    ((ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonAdd")).Enabled = false;
                }
            }
            else if (string.IsNullOrEmpty(badgeStatus))
            {
                this.EnableEquipmentControl();
            }
        }

        /// <summary>
        /// The ResetEquipmentCustodyInformation method
        /// </summary>
        /// <param name="multipleEntry">The IsMultipleEntry parameter</param>        
        public void ResetEquipmentCustodyInformation(bool multipleEntry)
        {
            // for single entry updated for CR IRVMS22062010CR07
            ////if (IsMultipleEntry==false)
            ////{
            this.SetInitialRow();
            ////}
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Session["showequipmentcustody"] = false;
            if (this.Session["LoginID"] == null)
            {
                return;
            }

            if (!Page.IsPostBack)
            {
                this.SetInitialRow();
            }
        }

        /// <summary>
        /// The Check_Description method
        /// </summary>
        /// <param name="source">The source parameter</param>
        /// <param name="args">The args parameter</param>        
        protected void Check_Description(object source, ServerValidateEventArgs args)
        {
            for (int i = 0; i <= this.grdEquipments.Rows.Count - 1; i++)
            {
                DropDownList ddlEquipmentType = (DropDownList)this.grdEquipments.Rows[i].Cells[1].FindControl("ddlEquipmentType");
                TextBox txtDescription = (TextBox)this.grdEquipments.Rows[i].Cells[1].FindControl("txtDescription");

                ddlEquipmentType.Enabled = true;

                if (ddlEquipmentType.SelectedItem.Text.ToUpper() != "SELECT")
                {
                    txtDescription.Enabled = true;
                    txtDescription.Visible = true;
                    if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                    {
                        args.IsValid = false; 
                    }
                }
                else
                {
                    txtDescription.Visible = true;
                }
                ////((ImageButton)grdEquipments.Rows[i].FindControl("ButtonAdd")).Enabled = true;
                ////if (grdEquipments.Rows.Count == 1)
                ////{
                //    ((ImageButton)grdEquipments.Rows[i].FindControl("ButtonDelete")).Enabled = false;
                ////}
                ////else
                ////{
                //    ((ImageButton)grdEquipments.Rows[i].FindControl("ButtonDelete")).Enabled = true;
                ////}
            }   
        }

        /// <summary>
        /// The Equipment Type Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void DdlEquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.grdEquipments.Rows.Count; i++)
            {
                DropDownList ddlEquipmentType = (DropDownList)this.grdEquipments.Rows[i].Cells[1].FindControl("ddlEquipmentType");

                TextBox txtDescription = (TextBox)this.grdEquipments.Rows[i].FindControl("txtDescription");

                if (ddlEquipmentType.SelectedItem.Text.ToUpper() != "SELECT")
                {
                    txtDescription.Enabled = true;
                    this.Session["EquipmentCustody"] = true;
                }
                else
                {
                    txtDescription.Text = string.Empty;
                    ////Session["EquipmentCustody"] = false;
                }
            }
        }

        /// <summary>
        /// The ButtonAdd_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.grdEquipments.Rows.Count; i++)
            {
                DropDownList dd = (DropDownList)this.grdEquipments.Rows[i].FindControl("ddlEquipmentType");
                ImageButton btnAdd = (ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonAdd");
                if (dd.SelectedItem.Text.ToString() == "Select")
                {
                    ////((ImageButton)(grdEquipments.Rows[i].FindControl("ButtonAdd"))).Enabled = false;                   
                    ////lblmessage.Text = "Please Select any of the Equipment Type";
                    return;
                }
                else
                {
                    ////lblmessage.Text = string.Empty;
                }
            }

            if (this.ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count == 1 && string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["RowNumber"])))
                {
                    dt = this.CreateDataTable();
                }

                this.AddNewRowToTable(dt);
            }

            if (this.grdEquipments.Rows.Count == 5)
            {
                for (int i = 0; i < this.grdEquipments.Rows.Count; i++)
                {
                    ImageButton btnAdd = (ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonAdd");
                    btnAdd.Enabled = false;
                    btnAdd.ImageUrl = "~/Images/IT_equip_disabled.png";
                }
            }
        }
        
        /// <summary>
        /// The Row Deleting method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdEquipments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                this.Session["EquipmentCustody"] = true;
                int index = Convert.ToInt16(e.RowIndex);
                this.DeleteRowFromTable(index);

                if (this.grdEquipments.Rows.Count == 1)
                {
                    for (int i = 0; i < this.grdEquipments.Rows.Count; i++)
                    {
                        ImageButton imgBtnDelete = (ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonDelete");
                        imgBtnDelete.Enabled = false;
                        ////imgBtnDelete.ImageUrl = "images/IT_equip2_disabled.png";
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                ////Utility.VMSUtility.logExceptionAndShowErrorPage(ex, HttpContext.Current);
                ////ExceptionLogger.OneC_ExceptionLogger(ex, this.Page);
            }
        }

        /// <summary>
        /// The RowDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdEquipments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEquipmentType = (DropDownList)e.Row.FindControl("ddlEquipmentType");
                ////TextBox txtOthers = ((TextBox)e.Row.FindControl("txtOtherEquipment"));
                Label lblEquipmentType = (Label)e.Row.FindControl("lblEquipmentType");
                ////if (!Session["showequipmentcustody"].Equals(true))
                ////{
                this.PopulateEquipmentType(ddlEquipmentType);
                ////}

                if (!string.IsNullOrEmpty(lblEquipmentType.Text))
                {
                    //// ddlEquipmentType.SelectedIndex

                    ddlEquipmentType.SelectedValue = lblEquipmentType.Text;
                }

                ddlEquipmentType.SelectedValue.Equals(lblEquipmentType.Text); // = lblEquipmentType.Text;

                TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");

                ImageButton buttonAdd = (ImageButton)e.Row.FindControl("ButtonAdd");
                ImageButton buttonDelete = (ImageButton)e.Row.FindControl("ButtonDelete");
                if (ddlEquipmentType.SelectedItem.Text.ToUpper() != "SELECT")
                {
                    txtDescription.Enabled = true;
                }
                else
                {
                    txtDescription.Enabled = false;
                }

                if (this.gridRowCount == 5)
                {
                    ////ImageButton ButtonAdd = (ImageButton)e.Row.FindControl("ButtonAdd");
                    buttonAdd.Enabled = false;
                }

                if (this.gridRowCount == 1)
                {
                    ////ImageButton ButtonDelete = (ImageButton)e.Row.FindControl("ButtonDelete");
                    ////ButtonDelete.Enabled = false;
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
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("EquipmentType", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("EquipmentTypeId", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("VisitDetailsID", typeof(string)));

            return dt;
        }

        /// <summary>
        /// The Add New Row To Table method
        /// </summary>
        /// <param name="dt">The date parameter</param>        
        private void AddNewRowToTable(DataTable dt)
        {
            int rowIndex = 0;
            DataRow drcurrentRow = null;

            if (dt.Rows.Count == 0)
            {
                ////add new row to DataTable
                drcurrentRow = dt.NewRow();
                drcurrentRow["RowNumber"] = rowIndex;
                drcurrentRow["EquipmentTypeID"] = string.Empty;
                drcurrentRow["EquipmentType"] = string.Empty;
                drcurrentRow["Description"] = string.Empty;

                dt.Rows.Add(drcurrentRow);
            }

            ////Updating all existing records in DataTable 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ////extract the TextBox values 
                DropDownList ddlEquipmentType = (DropDownList)this.grdEquipments.Rows[rowIndex].FindControl("ddlEquipmentType");
                TextBox txtDescription = (TextBox)this.grdEquipments.Rows[rowIndex].FindControl("txtDescription");

                dt.Rows[rowIndex]["RowNumber"] = rowIndex;
                if (ddlEquipmentType.SelectedIndex >= 0)
                {
                    dt.Rows[rowIndex]["EquipmentTypeID"] = ddlEquipmentType.SelectedValue;
                    dt.Rows[rowIndex]["EquipmentType"] = ddlEquipmentType.SelectedItem.Text;
                }
                else
                {
                    dt.Rows[rowIndex]["EquipmentType"] = string.Empty;
                }

                dt.Rows[rowIndex]["Description"] = txtDescription.Text.Trim();

                rowIndex++;
            }

            ////add new row to DataTable
            drcurrentRow = dt.NewRow();
            drcurrentRow["RowNumber"] = rowIndex;
            drcurrentRow["EquipmentType"] = string.Empty;
            drcurrentRow["Description"] = string.Empty;

            dt.Rows.Add(drcurrentRow);

            ////Store the current data to ViewState 
            this.ViewState["CurrentTable"] = dt;

            ////Rebind the Grid with the current data 
            this.grdEquipments.DataSource = dt;
            this.grdEquipments.DataBind();
        }

        /// <summary>
        /// The DeleteRowFromTable method
        /// </summary>
        /// <param name="index">The index parameter</param>        
        private void DeleteRowFromTable(int index)
        {
            DataTable dtcurrentData = this.RefreshTable();
            if (dtcurrentData != null)
            {
                dtcurrentData.Rows.RemoveAt(index);
                dtcurrentData.AcceptChanges();
                this.grdEquipments.DataSource = dtcurrentData;
                this.grdEquipments.DataBind();
            }

            if (this.grdEquipments.Rows.Count == 1)
            {
                for (int i = 0; i < this.grdEquipments.Rows.Count - 1; i++)
                {
                    ((ImageButton)this.grdEquipments.Rows[i].FindControl("ButtonDelete")).Enabled = false;
                }
            }
        }

        /// <summary>
        /// The RefreshTable method
        /// </summary>
        /// <returns>The System.Data.DataTable type object</returns>        
        private DataTable RefreshTable()
        {
            DataTable dt = this.CreateDataTable();
            ////Updating all existing records in DataTable 
            for (int i = 0; i < this.grdEquipments.Rows.Count; i++)
            {
                ////extract the TextBox values 
                DropDownList ddlEquipmentType = (DropDownList)this.grdEquipments.Rows[i].FindControl("ddlEquipmentType");
                TextBox txtDescription = (TextBox)this.grdEquipments.Rows[i].FindControl("txtDescription");

                DataRow dr = dt.NewRow();
                dr["RowNumber"] = i;
                if (ddlEquipmentType.SelectedIndex >= 0)
                {
                    dr["EquipmentTypeId"] = ddlEquipmentType.SelectedValue;
                    dr["EquipmentType"] = ddlEquipmentType.SelectedItem.Text;
                }
                else
                {
                    dr["EquipmentType"] = string.Empty;
                }

                dr["Description"] = txtDescription.Text.Trim();

                dt.Rows.Add(dr);
            }

            this.ViewState["CurrentTable"] = dt;
            return dt;
        }
    }
}
