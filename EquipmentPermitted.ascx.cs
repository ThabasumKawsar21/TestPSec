
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
    using System.Xml.Linq;
    using VMSBusinessEntity;

    /// <summary>
    /// Partial class Equipment Permitted
    /// </summary>   
    public partial class EquipmentPermitted : System.Web.UI.UserControl
    {
        /// <summary>
        /// The GridRowCount field
        /// </summary>        
        private int gridRowCount = 0;
        
        /// <summary>
        /// The ResetEquipmentInformation method
        /// </summary>
        /// <param name="multipleEntry">The IsMultipleEntry parameter</param>        
        public void ResetEquipmentInformation(bool multipleEntry)
        {
            // for single entry updated for CR IRVMS22062010CR07 
            ////if (IsMultipleEntry==false)
            ////{
            this.SetInitialRow();
            ////}
            this.lblmessage.Visible = false;
        }
        ////end updated for CR IRVMS22062010CR07  starts here done by Priti

        /// <summary>
        /// The PopulateEquipmentType method
        /// </summary>
        /// <param name="ddlEquipmentType">The EquipmentType parameter</param>        
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

            ////ddlEquipmentType.Items.RemoveAt(0);
            ////if ((ddlEquipmentType.Items.Count == 0) || (ddlEquipmentType.Items.Count > 0))
            //    ddlEquipmentType.Items.Insert(0, new ListItem("Select", "0"));
        }

        /// <summary>
        /// The InsertEquipmentInformation method
        /// </summary>
        /// <param name="visitDetailsID">The VisitDetailsID parameter</param>
        /// <returns>The VMSBusinessEntity.VisitorEquipment[] type object</returns>        
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
                    ////if (Convert.ToString(dt.Rows[i]["RowNumber"]) != String.Empty)
                    ////{
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

                    ////}
                    ////else
                    //    dt.Rows.RemoveAt(i);
                }

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["EquipmentType"])) && Convert.ToString(dt.Rows[i]["EquipmentType"]) != "22")
                    {
                        visitorEquipmentObj = new VMSBusinessEntity.VisitorEquipment();
                        masterDataID = 0;
                        visitorEquipmentObj.VisitDetailsID = visitDetailsID;
                        if (Convert.ToString(dt.Rows[i]["EquipmentType"]) != string.Empty)
                        {
                            masterDataID = Convert.ToInt32(dt.Rows[i]["EquipmentType"]);
                            visitorEquipmentObj.MasterDataID = masterDataID;
                        }

                        if (masterDataID == 31)
                        {
                            visitorEquipmentObj.Others = Convert.ToString(dt.Rows[i]["Others"]);
                            visitorEquipmentObj.Make = Convert.ToString(dt.Rows[i]["Make"]);
                            visitorEquipmentObj.SerialNo = Convert.ToString(dt.Rows[i]["SerialNo"]);
                            visitorEquipmentObj.Model = dt.Rows[i]["Model"].ToString();
                            ret.Add(visitorEquipmentObj);
                        }
                        else
                        {
                            visitorEquipmentObj.Others = string.Empty;
                            visitorEquipmentObj.Make = Convert.ToString(dt.Rows[i]["Make"]);
                            visitorEquipmentObj.SerialNo = Convert.ToString(dt.Rows[i]["SerialNo"]);
                            visitorEquipmentObj.Model = dt.Rows[i]["Model"].ToString();
                            ret.Add(visitorEquipmentObj);
                        }
                    }
                }

                arrayOfVisitorEquipmentObj = new VMSBusinessEntity.VisitorEquipment[ret.Count];
                ret.CopyTo(0, arrayOfVisitorEquipmentObj, 0, ret.Count);
            }

            return arrayOfVisitorEquipmentObj;
        }
        
        /// <summary>
        /// The ShowEquipmentInformationByRequestID method
        /// </summary>
        /// <param name="propertiesDc">The propertiesDc parameter</param>        
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

        ////Added by lakshmi - START
        
        /// <summary>
        /// The DisableEquipmentControl method
        /// </summary>
        /// <param name="requestStatus">The RequestStatus parameter</param>
        /// <param name="badgeStatus">The BadgeStatus parameter</param>        
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
        /// The EnableEquipmentControl method
        /// </summary>        
        public void EnableEquipmentControl()
        {
            for (int i = 0; i <= this.GridView1.Rows.Count - 1; i++)
            {
                ((DropDownList)this.GridView1.Rows[i].FindControl("ddlEquipmentType")).Enabled = true;
                ((HtmlInputText)this.GridView1.Rows[i].FindControl("txtOthers")).Disabled = false;
                ((TextBox)this.GridView1.Rows[i].FindControl("txtMake")).Enabled = true;
                ((TextBox)this.GridView1.Rows[i].FindControl("txtSerialNo")).Enabled = true;
                ////((Button)GridView1.Rows[i].FindControl("ButtonAdd")).Enabled = true;
                ////((Button)GridView1.Rows[i].FindControl("ButtonDelete")).Enabled = true;
            }
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
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
        /// The ButtonAdd_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
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
        /// The GridView1_RowDeleting method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
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
        /// The GridView1_RowDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
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
        ////END

        /// <summary>
        /// The Equipment Type Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
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
        /// The SetInitialRow method
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
        /// The RefreshTable method
        /// </summary>
        /// <returns>The System.Data.DataTable type object</returns>        
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
        /// The AddNewRowToTable method
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

        ////protected void ddlEquipmentType_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        //    HtmlSelect ddlEquipmentType = ((HtmlSelect)sender);

        ////    string txtOthersId =  ddlEquipmentType.Name.Replace("ddlEquipmentType", "txtOthers").Replace("$","_");
        //    HtmlInputText txtOthers = ((HtmlInputText)GridView1.FindControl(txtOthersId));
        //    if (ddlEquipmentType.Items[ddlEquipmentType.SelectedIndex].Text == VMSConstants.VMSConstants.Others)
        //        //txtOthers.Style.Add("visibility", "visible");
        //        txtOthers.Visible = true;
        //    else
        //        //txtOthers.Style.Add("visibility", "hidden");
        //        txtOthers.Visible = false;
        ////}
    }
}
