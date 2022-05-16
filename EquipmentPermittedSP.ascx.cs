--

namespace VMSDev.SafetyPermitUserControls
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

    /// <summary>
    /// Equipment Permitted
    /// </summary> 
    public partial class EquipmentPermittedSP : System.Web.UI.UserControl
    {
        /// <summary>
        /// The GridRowCount field
        /// </summary>        
        private int gridRowCount = 0;

        /// <summary>
        /// Reset equipment information
        /// </summary>
        /// <param name="isMultipleEntry">Is multiple entry</param>
        public void ResetEquipmentInformation(bool isMultipleEntry)
        {
            this.SetInitialRow();
            this.lblmessage.Visible = false;
        }

        /// <summary>
        /// Populate Equipment type
        /// </summary>
        /// <param name="ddlEquipmentType">Equipment type</param>
        public void PopulateEquipmentType(DropDownList ddlEquipmentType)
        {
            VMSBusinessLayer.VMSBusinessLayer.MasterDataBL masterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
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

        #region "Variables"
        /// <summary>
        /// Insert equipment information
        /// </summary>
        /// <param name="visitDetailsID">visitor id</param>
        /// <returns>visitor object</returns>
        public VMSBusinessEntity.VisitorEquipment[] InsertEquipmentInformation(int visitDetailsID)
        {
            VMSBusinessEntity.VisitorEquipment[] arrayOfVisitorEquipmentObj = null;
            VMSBusinessEntity.VisitorEquipment visitorEquipmentObj;
            ArrayList ret = new ArrayList();
            int masterDataID;
            if (this.ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DropDownList ddlEquipmentType = (DropDownList)this.GridView1.Rows[i].FindControl("ddlEquipmentType");
                    TextBox txtMake1 = (TextBox)this.GridView1.Rows[i].FindControl("txtMake");
                    TextBox txtModel = (TextBox)this.GridView1.Rows[i].FindControl("txtModel");
                    TextBox txtSerialNo1 = (TextBox)this.GridView1.Rows[i].FindControl("txtSerialNo");
                    HtmlInputText txtOthers = (HtmlInputText)this.GridView1.Rows[i].FindControl("txtOthers");
                    dt.Rows[i]["RowNumber"] = i;
                    if (ddlEquipmentType.SelectedIndex > 0)
                    {
                        dt.Rows[i]["EquipmentType"] = ddlEquipmentType.SelectedValue;
                    }
                    else
                    {
                        dt.Rows[i]["EquipmentType"] = string.Empty;
                    }

                    dt.Rows[i]["Make"] = txtMake1.Text.Trim();
                    dt.Rows[i]["Model"] = txtModel.Text.Trim();
                        dt.Rows[i]["SerialNo"] = txtSerialNo1.Text.Trim();

                        if (ddlEquipmentType.Items[ddlEquipmentType.SelectedIndex].Text == VMSConstants.VMSConstants.OTHERS)
                        {
                            dt.Rows[i]["Others"] = txtOthers.Value.Trim();
                        }
                        else
                        {
                            dt.Rows[i]["Others"] = string.Empty;
                        }
                }
                #endregion

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EquipmentType"])) && Convert.ToString(dt.Rows[i]["EquipmentType"]) != "22")
                    {
                        visitorEquipmentObj = new VMSBusinessEntity.VisitorEquipment();
                        masterDataID = 0;
                        visitorEquipmentObj.VisitDetailsID = visitDetailsID;
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EquipmentType"])))
                        {
                            masterDataID = Convert.ToInt32(dt.Rows[i]["EquipmentType"]);
                        }

                        visitorEquipmentObj.MasterDataID = masterDataID;
                        if (masterDataID == 31)
                        {
                            visitorEquipmentObj.Others = Convert.ToString(dt.Rows[i]["Others"]);
                        }
                        else
                        {
                            visitorEquipmentObj.Others = string.Empty;
                        }

                        visitorEquipmentObj.Make = Convert.ToString(dt.Rows[i]["Make"]);
                        visitorEquipmentObj.SerialNo = Convert.ToString(dt.Rows[i]["SerialNo"]);
                        visitorEquipmentObj.Model = dt.Rows[i]["Model"].ToString();
                        ret.Add(visitorEquipmentObj);
                    }
                }

                arrayOfVisitorEquipmentObj = new VMSBusinessEntity.VisitorEquipment[ret.Count];
                ret.CopyTo(0,  arrayOfVisitorEquipmentObj, 0, ret.Count);
            }

            return arrayOfVisitorEquipmentObj;
        }

        /// <summary>
        /// Show equipment information request
        /// </summary>
        /// <param name="propertiesDc">Properties details</param>
        public void ShowEquipmentInformationByRequestID(VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDc)
        {
            List<VisitorEquipment> visitorEquipmentList = propertiesDc.VisitorEquipmentProperty;
            if (visitorEquipmentList != null && visitorEquipmentList.Count > 0)
            {
                DataTable dt = this.CreateDataTable();
                DataRow dr;
                VisitorEquipment objVisitorEquip;
                ////for (int iVisitor = VisitorEquipmentList.Count - 1; iVisitor >= 0; iVisitor--)
                for (int ivisitor = 0; ivisitor <= visitorEquipmentList.Count - 1; ivisitor++)
                {
                    objVisitorEquip = visitorEquipmentList[ivisitor];
                    dr = dt.NewRow();
                    dr["RowNumber"] = ivisitor;
                    dr["EquipmentType"] = objVisitorEquip.MasterDataID;
                    dr["Make"] = objVisitorEquip.Make;
                    dr["Model"] = objVisitorEquip.Model;
                    dr["SerialNo"] = objVisitorEquip.SerialNo;
                    dr["Others"] = objVisitorEquip.Others;
                    dt.Rows.Add(dr);
                }

                this.ViewState["CurrentTable"] = dt;
                this.GridView1.DataSource = dt;
                this.gridRowCount = dt.Rows.Count;
                this.GridView1.DataBind();
            }
            else
            {
                this.SetInitialRow();
            }

            this.lblmessage.Visible = false;
        }

        /// <summary>
        /// Disable equipment control
        /// </summary>
        /// <param name="requestStatus">Request status</param>
        /// <param name="badgeStatus">badge status</param>
        public void DisableEquipmentControl(string requestStatus, string badgeStatus)
        {
            ////changed by priti on 3rd June for VMS CR VMS31052010CR6
            if ((badgeStatus == "ISSUED") || (badgeStatus == "RETURNED") ||
                    (badgeStatus == "LOST") || (requestStatus == "CANCELLED"))
            {
                for (int i = 0; i <= this.GridView1.Rows.Count - 1; i++)
                {
                    ((DropDownList)this.GridView1.Rows[i].FindControl("ddlEquipmentType")).Enabled = false;
                    ((HtmlInputText)this.GridView1.Rows[i].FindControl("txtOthers")).Disabled = true;
                    ((TextBox)this.GridView1.Rows[i].FindControl("txtMake")).Enabled = false;
                    ((TextBox)this.GridView1.Rows[i].FindControl("txtModel")).Enabled = false;
                    ((TextBox)this.GridView1.Rows[i].FindControl("txtSerialNo")).Enabled = false;
                    ((Button)this.GridView1.Rows[i].FindControl("ButtonAdd")).Enabled = false;
                    ((Button)this.GridView1.Rows[i].FindControl("ButtonDelete")).Enabled = false;
                }
            }
            else if (string.IsNullOrEmpty(badgeStatus))
            {
                this.EnableEquipmentControl();
            }
        }

        /// <summary>
        /// Enable equipment control
        /// </summary>
        public void EnableEquipmentControl()
        {
            for (int i = 0; i <= this.GridView1.Rows.Count - 1; i++)
            {
                ((DropDownList)this.GridView1.Rows[i].FindControl("ddlEquipmentType")).Enabled = true;
                ((HtmlInputText)this.GridView1.Rows[i].FindControl("txtOthers")).Disabled = false;
                ((TextBox)this.GridView1.Rows[i].FindControl("txtMake")).Enabled = true;
                ((TextBox)this.GridView1.Rows[i].FindControl("txtSerialNo")).Enabled = true;
            }
        }

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
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
        /// Button add click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                DropDownList dd = (DropDownList)this.GridView1.Rows[i].FindControl("ddlEquipmentType");
                Button btnAdd = (Button)this.GridView1.Rows[i].FindControl("ButtonAdd");
                if (dd.SelectedItem.Text.ToString() == "Select")
                {
                    this.lblmessage.Text = Resources.LocalizedText.SelectEquipmentType;
                    return;
                }
                else
                {
                    this.lblmessage.Text = string.Empty;
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

            if (this.GridView1.Rows.Count == 5)
            {
                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {
                    ((Button)this.GridView1.Rows[i].FindControl("ButtonAdd")).Enabled = false;
                }
            }
        }

        /// <summary>
        /// Grid view row deleting
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt16(e.RowIndex);
            this.DeleteRowFromTable(index);

            if (this.GridView1.Rows.Count == 1)
            {
                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {
                    ((Button)this.GridView1.Rows[i].FindControl("ButtonDelete")).Enabled = false;
                }
            }
        }

        /// <summary>
        /// Grid view row data bound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlEquipmentType = (DropDownList)e.Row.FindControl("ddlEquipmentType");
                HtmlInputText txtOthers = (HtmlInputText)e.Row.FindControl("txtOthers");
                Label lblEquipmentType = (Label)e.Row.FindControl("lblEquipmentType");
                this.PopulateEquipmentType(ddlEquipmentType);          
                ////ddlEquipmentType.Attributes.Add("onchange", "javascript:ViewOtherTxtBoxControl(this,'" + VMSConstants.VMSConstants.Others + "');");
                if (!string.IsNullOrEmpty(lblEquipmentType.Text))
                {
                    ddlEquipmentType.SelectedValue = lblEquipmentType.Text;
                }

                if (ddlEquipmentType.Items[ddlEquipmentType.SelectedIndex].Text == VMSConstants.VMSConstants.OTHERS)
                {
                    txtOthers.Visible = true;
                }
                else
                {
                    txtOthers.Visible = false;
                }

                if (this.gridRowCount == 5)
                {
                    Button buttonAdd = (Button)e.Row.FindControl("ButtonAdd");
                    buttonAdd.Enabled = false;
                }

                if (this.gridRowCount == 1)
                {
                    Button buttonDelete = (Button)e.Row.FindControl("ButtonDelete");
                    buttonDelete.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Equipment type selected index changed
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event e</param>
        protected void DdlEquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblmessage.Text = string.Empty;

            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                DropDownList ddlEquipmentType = (DropDownList)this.GridView1.Rows[i].Cells[1].FindControl("ddlEquipmentType");
                HtmlInputText txtOthers = (HtmlInputText)this.GridView1.Rows[i].Cells[1].FindControl("txtOthers");
                if (ddlEquipmentType.Items[ddlEquipmentType.SelectedIndex].Text == VMSConstants.VMSConstants.OTHERS)
                {
                    txtOthers.Visible = true;
                }
                else
                {
                    txtOthers.Visible = false;
                }
            }
        }

        /// <summary>
        /// Delete row from table
        /// </summary>
        /// <param name="index">index length</param>
        private void DeleteRowFromTable(int index)
        {
            DataTable dtcurrentData = this.RefreshTable();
           if (dtcurrentData != null)
            {
                dtcurrentData.Rows.RemoveAt(index);
                dtcurrentData.AcceptChanges();
                this.GridView1.DataSource = dtcurrentData;
                this.GridView1.DataBind();
            }

           if (this.GridView1.Rows.Count == 1)
            {
                for (int i = 0; i < this.GridView1.Rows.Count - 1; i++)
                {
                    ((Button)this.GridView1.Rows[i].FindControl("ButtonDelete")).Enabled = false;
                }
            }
        }

        /// <summary>
        /// Create data table
        /// </summary>
        /// <returns>date time</returns>
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
        /// Set initial row
        /// </summary>
        private void SetInitialRow()
        {
            DataTable dt = this.CreateDataTable();
            DataRow dr = dt.NewRow();
            dr["RowNumber"] = string.Empty;
            dr["EquipmentType"] = string.Empty;
            dr["Make"] = string.Empty;
            dr["Model"] = string.Empty;
            dr["SerialNo"] = string.Empty;
            dr["Others"] = string.Empty;
            dt.Rows.Add(dr);
            this.ViewState["CurrentTable"] = dt;
            this.GridView1.DataSource = dt;
            this.GridView1.DataBind();
            if (this.GridView1.Rows.Count > 0)
            {
                Button deletebut = (Button)this.GridView1.Rows[0].FindControl("ButtonDelete");
                if (this.GridView1.Rows.Count > 1)
                {
                    deletebut.Enabled = true;
                }
                else
                {
                    deletebut.Enabled = false;
                }
            }     
        }

        /// <summary>
        /// Refresh table
        /// </summary>
        /// <returns>date time</returns>
        private DataTable RefreshTable()
        {
            DataTable dt = this.CreateDataTable();
            ////Updating all existing records in DataTable 
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                ////extract the TextBox values 
                DropDownList ddlEquipmentType = (DropDownList)this.GridView1.Rows[i].FindControl("ddlEquipmentType");
                TextBox txtMake1 = (TextBox)this.GridView1.Rows[i].FindControl("txtMake");
                TextBox txtModel = (TextBox)this.GridView1.Rows[i].FindControl("txtModel");
                TextBox txtSerialNo1 = (TextBox)this.GridView1.Rows[i].FindControl("txtSerialNo");
                HtmlInputText txtOthers = (HtmlInputText)this.GridView1.Rows[i].FindControl("txtOthers");
                DataRow dr = dt.NewRow();
                dr["RowNumber"] = i;
                if (ddlEquipmentType.SelectedIndex >= 0)
                {
                    dr["EquipmentType"] = ddlEquipmentType.SelectedValue;
                }
                else
                {
                    dr["EquipmentType"] = string.Empty;
                }

                dr["Make"] = txtMake1.Text.Trim();
                dr["Model"] = txtModel.Text.Trim();
                dr["SerialNo"] = txtSerialNo1.Text.Trim();
                if (ddlEquipmentType.Items[ddlEquipmentType.SelectedIndex].Text == VMSConstants.VMSConstants.OTHERS)
                {
                    dr["Others"] = txtOthers.Value.Trim();
                }
                else
                {
                    dr["Others"] = string.Empty;
                }

                dt.Rows.Add(dr);
            }

            this.ViewState["CurrentTable"] = dt;
            return dt;
        }

        /// <summary>
        /// Add new row to table
        /// </summary>
        /// <param name="dt">date time</param>
        private void AddNewRowToTable(DataTable dt)
        {
            int rowIndex = 0;
            DataRow drcurrentRow = null;

            if (dt.Rows.Count == 0)
            {
                ////add new row to DataTable
                drcurrentRow = dt.NewRow();
                drcurrentRow["RowNumber"] = rowIndex;
                drcurrentRow["EquipmentType"] = string.Empty;
                drcurrentRow["Make"] = string.Empty;
                drcurrentRow["SerialNo"] = string.Empty;
                drcurrentRow["Others"] = string.Empty;
                dt.Rows.Add(drcurrentRow);
            }

            ////Updating all existing records in DataTable 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ////extract the TextBox values 
                DropDownList ddlEquipmentType = (DropDownList)this.GridView1.Rows[rowIndex].FindControl("ddlEquipmentType");
                TextBox txtMake1 = (TextBox)this.GridView1.Rows[rowIndex].FindControl("txtMake");
                TextBox txtModel = (TextBox)this.GridView1.Rows[rowIndex].FindControl("txtModel");
                TextBox txtSerialNo1 = (TextBox)this.GridView1.Rows[rowIndex].FindControl("txtSerialNo");
                HtmlInputText txtOthers = (HtmlInputText)this.GridView1.Rows[rowIndex].FindControl("txtOthers");

                dt.Rows[rowIndex]["RowNumber"] = rowIndex;
                if (ddlEquipmentType.SelectedIndex >= 0)
                {
                    dt.Rows[rowIndex]["EquipmentType"] = ddlEquipmentType.SelectedValue;
                }
                else
                {
                    dt.Rows[rowIndex]["EquipmentType"] = string.Empty;
                }

                dt.Rows[rowIndex]["Make"] = txtMake1.Text.Trim();
                dt.Rows[rowIndex]["Model"] = txtModel.Text.Trim();
                dt.Rows[rowIndex]["SerialNo"] = txtSerialNo1.Text.Trim();

                if (ddlEquipmentType.Items[ddlEquipmentType.SelectedIndex].Text == VMSConstants.VMSConstants.OTHERS)
                {
                    dt.Rows[rowIndex]["Others"] = txtOthers.Value.Trim();
                }
                else
                {
                    dt.Rows[rowIndex]["Others"] = string.Empty;
                }

                rowIndex++;
            }

            ////add new row to DataTable
            drcurrentRow = dt.NewRow();
            drcurrentRow["RowNumber"] = rowIndex;
            drcurrentRow["EquipmentType"] = string.Empty;
            drcurrentRow["Make"] = string.Empty;
            drcurrentRow["Model"] = string.Empty;
            drcurrentRow["SerialNo"] = string.Empty;
            drcurrentRow["Others"] = string.Empty;
            dt.Rows.Add(drcurrentRow);

            ////Store the current data to ViewState 
            this.ViewState["CurrentTable"] = dt;

            ////Rebind the Grid with the current data 
            this.GridView1.DataSource = dt;
            this.GridView1.DataBind();
        }
    }
}
