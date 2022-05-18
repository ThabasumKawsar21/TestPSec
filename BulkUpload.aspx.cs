

namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using OfficeOpenXml; 

    /// <summary>
    /// partial class bulk upload
    /// </summary>
    public partial class BulkUpload : System.Web.UI.Page
    {
        /// <summary>
        ///   message array
        /// </summary>
        private ArrayList armessages2 = new ArrayList();        

        /// <summary>
        /// Page load function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lbtnUpload.Enabled = false;
            this.Error.Visible = false;
            this.lbtnPreviewEdit.Enabled = false;
            this.Close.Visible = true;
            ////to hide the cancel image if try to view the entries
            if (Convert.ToString(this.Session["ViewCancelImage"]) == "1")
            {
                this.ImgCancelUpload.Visible = false;
                ////Session["ViewCancelImage"] = null;     
            }

            if (!Page.IsPostBack)
            {
                this.lbtnValidate1.Visible = false;
                this.lbtnBack.Visible = false;
                this.lbtnUpload1.Visible = false;
                this.lbtnUpload1.Visible = false;
                this.lbtnBack1.Visible = false;
                this.lbtnBack1.Visible = false;
                this.Session["checked"] = "0";

                if (this.Session["ViewCandidates"] != null)
                {
                    this.EditGrid.Visible = true;
                    this.divUpload.Visible = false;
                    this.lbtnBack.Visible = false;
                    this.lbtnUpload1.Visible = true;
                    this.lbtnBack1.Visible = true;
                    this.lbtnValidate1.Visible = true;
                    this.lbtnValidate1.Enabled = false;
                    this.Session["checked"] = "1";
                    this.ViewDetails();
                    this.Session["ViewDelete"] = null;
                }

                if (this.Session["RowNumber"] != null)
                {
                    this.Display();
                }
            }
        }

        /// <summary>
        /// To validate excel file
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnValidate_Click(object sender, EventArgs e)
        {
            this.Session["DataSource1"] = null;
            try
            {
                HttpPostedFile filetoUpload = this.btnUpload.PostedFile;
                string fileExtension = Path.GetExtension(filetoUpload.FileName);
                this.Clear.Visible = true;
                ////checking if file is selected
                if (string.IsNullOrEmpty(filetoUpload.FileName))
                {
                    this.Clear.Visible = true;
                    this.lblmessage.Visible = true;
                    this.lblmessage.Text = "Please select a file to validate.";
                }
                else
                {
                    this.lblmessage.Visible = false;
                    this.BulkUploadAssociate();
                    if (this.Session["DataSource1"] == null)
                    {
                        this.lblmessage.Visible = true;
                    }
                    else
                    {
                        this.lblmessage.Visible = false;
                        this.lbtnValidate1.Visible = true;
                        this.lbtnPreviewEdit.Enabled = true;
                        this.Validate.Enabled = false;
                        this.Validates();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        #region testedbybincey
        ////added by bincey -- for validation

        ////private void Validates()
        ////{

        ////    DataTable dt = new DataTable();
        ////    DataTable dt1 = new DataTable();
        ////    DataTable dtss = new DataTable();
        ////    dt1.Columns.Add("ContractorId", typeof(string));
        ////    dt1.Columns.Add("ContractorName", typeof(string));
        ////    dt1.Columns.Add("VendorName", typeof(string));
        ////    dt1.Columns.Add("SuperVisiorPhone", typeof(string));
        ////    dt1.Columns.Add("VendorPhoneNumber", typeof(string));
        ////    dt1.Columns.Add("DOCStatus", typeof(string));
        ////    dt1.Columns.Add("Status", typeof(string));
        ////    divUpload.Visible = false;
        ////    dgAssociate.Visible = true;
        ////    bool check = false;
        ////    bool MobileCheck = false;
        ////    bool MailSuperVisiorPhoneCheck = false;
        ////    bool c = false;
        ////    bool NullFirstNameCheck = false;
        ////    bool NullContractorId = false;
        ////    bool NullVendorName = false;
        ////    bool NullSuperVisiorPhone = false;
        ////    bool NullVendorPhoneNumber = false;
        ////    bool ContractorIdCheck = false;

        ////    lblmessage2.Visible = true;
        ////    ArrayList arMessages = new ArrayList();
        ////    ArrayList arControls = new ArrayList();
        ////    try
        ////    {

        ////        dt = (DataTable)Session["DataSource1"];
        ////        DataTable dttt = dt.Copy();
        ////        dt1 = null;
        ////        dt1 = dt.Copy();
        ////        for (int i = Convert.ToInt32(dt.Rows.Count - 1); i >= 0; i--)
        ////        {
        ////            if (!string.IsNullOrEmpty(dt.Rows[i]["ContractorId"].ToString()))
        ////            {
        ////                if (((dt.Rows[i]["ContractorId"].ToString().Trim().Length == 6)))
        ////                { }
        ////                else
        ////                {
        ////                    ContractorIdCheck = true;
        ////                }

        ////            }

        ////            if (!string.IsNullOrEmpty(dt.Rows[i]["SuperVisiorPhone"].ToString()))
        ////            {
        ////                if ((dt.Rows[i]["SuperVisiorPhone"].ToString().Contains(('.'))))
        ////                {
        ////                    MailSuperVisiorPhoneCheck = true;
        ////                }
        ////                if ((((dt.Rows[i]["SuperVisiorPhone"].ToString().Trim().Length >= 10)))
        ////                    && ((dt.Rows[i]["SuperVisiorPhone"].ToString().Trim().Length < 12)))
        ////                {
        ////                    MobileCheck = true;
        ////                }
        ////                else
        ////                {
        ////                    MailSuperVisiorPhoneCheck = true;
        ////                }
        ////            }
        ////            if ((dt.Rows[i]["ContractorName"].ToString().Trim() == ""))
        ////            {
        ////                NullFirstNameCheck = true;

        ////            }
        ////            if ((dt.Rows[i]["VendorName"].ToString().Trim() == ""))
        ////            {
        ////                NullVendorName = true;
        ////            }

        ////            if ((dt.Rows[i]["SuperVisiorPhone"].ToString().Trim() == ""))
        ////            {
        ////                NullSuperVisiorPhone = true;
        ////            }

        ////            if ((dt.Rows[i]["VendorPhoneNumber"].ToString().Trim() == ""))
        ////            {
        ////                NullVendorPhoneNumber = true;
        ////            }
        ////            if ((dt.Rows[i]["FirstName"].ToString() == "") || (dt.Rows[i]["LastName"].ToString() == "") || (dt.Rows[i]["Gender"].ToString() == ""))
        ////            {
        ////                NullCheck = true;

        ////            }

        ////            if ((dt.Rows[i]["ContractorName"].ToString().Trim() == "") ||
        ////                (dt.Rows[i]["VendorName"].ToString().Trim() == "") ||
        ////                (dt.Rows[i]["SuperVisiorPhone"].ToString().Trim() == "")
        ////                || dt.Rows[i]["VendorPhoneNumber"].ToString().Trim() == "")
        ////            {
        ////                todo
        ////                MobileCheck = false;
        ////                MailCheck = false;
        ////                check = false;
        ////            }
        ////            else
        ////            {
        ////                DataTable dt4 = new DataTable();
        ////                DataRow delRow;
        ////                dt4 = dt.Clone();
        ////                delRow = (DataRow)dt.Rows[i];
        ////                dt4.ImportRow(delRow);
        ////                if (Session["validatedfinal"] != null)
        ////                {
        ////                    dtss = (DataTable)Session["validatedfinal"];
        ////                }

        ////                dtss.Merge(dt4);
        ////                Session["validatedfinal"] = dtss.Copy();
        ////                dt.Rows.RemoveAt(i);

        ////            }
        ////        }
        ////        if (ContractorIdCheck == true)
        ////        {
        ////            arMessages.Add("Contractor Id Number is not valid.");
        ////        }
        ////        if (MailSuperVisiorPhoneCheck == true)
        ////        {
        ////            arMessages.Add("SuperVisior Phone Number is not valid.");
        ////        }
        ////        if (MobileCheck == true)
        ////        {
        ////            arMessages.Add("MobileNumber  is not valid.");
        ////        }
        ////        if (NullFirstNameCheck == true)
        ////        {
        ////            arMessages.Add("Supervisor Name should not be blank.");
        ////        }
        ////        if (NullVendorName == true)
        ////        {
        ////            arMessages.Add("Vendor Name should not be blank.");
        ////        }
        ////        if (NullSuperVisiorPhone == true)
        ////        {
        ////            arMessages.Add("SuperVisior Phone Field should not be blank.");
        ////        }
        ////        if (NullVendorPhoneNumber == true)
        ////        {
        ////            arMessages.Add("VendorPhone Number Field should not be blank.");
        ////        }

        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((MailSuperVisiorPhoneCheck == true)))
        ////        {
        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dttt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dttt;

        ////        }
        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((MobileCheck == true)))
        ////        {

        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dttt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dttt;

        ////        }

        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((NullFirstNameCheck == true)))
        ////        {
        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dttt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dttt;
        ////        }
        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((NullVendorName == true)))
        ////        {

        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dttt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dttt;
        ////        }
        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((NullSuperVisiorPhone == true)))
        ////        {
        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dttt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dttt;
        ////        }
        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((NullVendorPhoneNumber == true)))
        ////        {

        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dttt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dttt;

        ////        }
        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((check == true)))
        ////        {

        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dttt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dttt;

        ////        }

        ////        if ((Convert.ToInt32(dt.Rows.Count) != 0) && (check == false) && (MailSuperVisiorPhoneCheck == false) && (MobileCheck == false) &&
        ////           (NullFirstNameCheck == false) && (NullVendorName == false) && (NullSuperVisiorPhone == false) && (NullVendorPhoneNumber == false))
        ////        {

        ////            dgAssociate.Visible = true;
        ////            dgAssociate.DataSource = dt;
        ////            dgAssociate.DataBind();
        ////            Session["DataSource1"] = dt;

        ////        }
        ////        else if (Convert.ToInt32(dt.Rows.Count) == 0)
        ////        {

        ////            DataTable dt4 = new DataTable();
        ////            dt4 = dt.Clone();
        ////            DataTable dt5 = new DataTable();
        ////            if (Session["validatedinitial"] != null)
        ////            {
        ////                dt4 = (DataTable)Session["validatedinitial"];
        ////            }
        ////            if (Convert.ToString(Session["Delete"]) == "1")//if entry is deleted
        ////            {
        ////                if (Convert.ToString(Session["flagcheck"]) == "1")//if validated succesfully on 2nd validation
        ////                {
        ////                    dt5 = dttt.Copy();
        ////                    dt4 = dttt.Copy();
        ////                }
        ////                else if (Convert.ToString(Session["checked"]) == "1")//if validated succesfully on first validation
        ////                {
        ////                    dt5 = dttt.Copy();
        ////                    dt4 = (DataTable)Session["validatedinitial"];
        ////                }
        ////                else if (Convert.ToString(Session["notchecked"]) == "1")//if not validated succesfully on first validation
        ////                {
        ////                    dt5 = dttt.Copy();
        ////                    dt4 = (DataTable)Session["validatedinitial"];
        ////                }
        ////                else
        ////                {
        ////                    dt5 = dttt.Copy();
        ////                    dt4 = dttt.Copy();
        ////                }
        ////                Session["Delete"] = "0";
        ////            }
        ////            else
        ////            {

        ////                if ((Convert.ToInt32(dt.Rows.Count) == 0) && (Convert.ToString(Session["checked"]) == "1"))//if validated succesfully on first validation
        ////                {
        ////                    if (Convert.ToString(Session["flagcheck"]) != "1")//Not Fully validated for 2nd validation
        ////                    {
        ////                        dt5 = (DataTable)Session["validatedfinal"];
        ////                        dt4 = (DataTable)Session["validatedfinal"];
        ////                    }
        ////                    else if ((Convert.ToString(Session["flagcheck"]) == "1") && (Convert.ToString(Session["checked"]) == "1"))
        ////                    {
        ////                        dt5 = dttt.Copy();
        ////                        dt4 = dttt.Copy();
        ////                    }
        ////                    else
        ////                    {
        ////                        dt5 = dttt.Copy();
        ////                        dt4 = dttt.Copy();
        ////                    }
        ////                }

        ////                else
        ////                {
        ////                    if ((Convert.ToString(Session["UpdatedChecked"]) == "1") && (Convert.ToString(Session["notchecked"]) == null))//updated and not validated fully on first validation
        ////                    {
        ////                        dt5 = dttt.Copy();
        ////                        dt4 = dttt.Copy();
        ////                    }
        ////                    else if (Convert.ToString(Session["notchecked"]) == "1")//not validated fully on first validation
        ////                    {
        ////                        dt5 = dttt.Copy();
        ////                        dt4 = (DataTable)Session["validatedinitial"];
        ////                    }
        ////                    else if ((Convert.ToString(Session["flagcheck"]) == "1") && (Convert.ToString(Session["UpdatedChecked"]) == "1"))
        ////                    {

        ////                        dt5 = dttt.Copy();
        ////                        dt4 = dttt.Copy();
        ////                    }
        ////                    else if (Convert.ToString(Session["Duplicates"]) == "1")
        ////                    {
        ////                        dt5 = dttt.Copy();
        ////                        dt4 = dttt.Copy();
        ////                    }
        ////                    else
        ////                    {
        ////                        dt5 = (DataTable)Session["validatedfinal"];
        ////                    }
        ////                }
        ////            }
        ////            Session["notchecked"] = null;
        ////            if (dt5 != null && dt4 != null)
        ////            {
        ////                dt4.Merge(dt5);

        ////            }
        ////            else
        ////            {
        ////                dt4 = dt5.Copy();

        ////            }
        ////            DataTable distinctTables = dt4.DefaultView.ToTable( /*distinct*/ true);
        ////            dgAssociate.DataSource = distinctTables;
        ////            dgAssociate.DataBind();

        ////            Session["DataSourceFinal"] = distinctTables.Copy();
        ////            Session["DataSourceFinalCount"] = distinctTables.Rows.Count;
        ////            lbtnValidate1.Enabled = false;
        ////            int count = distinctTables.Rows.Count;
        ////            Session["DataSource1"] = Session["DataSourceFinal"];
        ////            Session["ViewCandidates"] = distinctTables.Copy();
        ////            string message = string.Empty;
        ////            if (Convert.ToString(Session["Duplicates"]) == "1")
        ////            {
        ////                message = "Removed Duplicate Entries";
        ////                Session["Duplicates"] = null;
        ////            }
        ////            lblmessage2.Text = message + "<br/>" + count + " entries successfully validated. Go Back to upload the details";
        ////            lblmessage2.Text = count + " entries successfully validated. Upload the details.";
        ////            lbtnUpload1.Visible = true;
        ////            lbtnUpload1.Enabled = true;
        ////            lbtnBack1.Visible = true;
        ////            lbtnBack1.Enabled = false;
        ////            Session["flagcheck"] = "1";
        ////            lbtnBack.Enabled = true;
        ////            lbtnBack.Visible = false;
        ////            Session["UpdatedChecked"] = null;
        ////        }

        ////        if (arMessages.Count > 0)
        ////        {
        ////            lblmessage2.Visible = false;
        ////            BulletedList BlMessages = new BulletedList();
        ////            BlMessages.DataSource = arMessages;
        ////            BlMessages.DataBind();
        ////            BlMessages.DataSource = arMessages;
        ////            pnlError.Controls.Add(BlMessages);
        ////            pnlError.Visible = true;
        ////            lbtnBack.Enabled = false;
        ////            lbtnUpload1.Enabled = false;
        ////            lbtnBack1.Enabled = false;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}

        ////ends -- bincey
        #endregion
       
        /// <summary>
        /// upload click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                string securityID = string.Empty;
                string strLocationId = string.Empty;
                bool recInserted = false;
                this.dgAssociate.DataSource = this.Session["DataSourceFinal"];
                this.dgAssociate.DataBind();
                DataTable dt = new DataTable();
                dt = (DataTable)Session["DataSourceFinal"];
                securityID = Session["LoginID"].ToString();                
                DataSet dslocation = requestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
                DataTable dtlocation = dslocation.Tables[0];
                if (dtlocation.Rows.Count > 0)
                {
                    strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);                    
                }

                DateTime currDate = DateTime.Now;
                foreach (DataRow dr in dt.Rows)
                {
                    VMSBusinessLayer.VMSBusinessLayer.MasterDataBL objMasterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
                    recInserted = objMasterDataBL.InsertContractorIDDetails(dr["ContractorName"].ToString(), dr["ContractorId"].ToString(), dr["VendorName"].ToString(), dr["Status"].ToString(), dr["SuperVisiorPhone"].ToString(), dr["VendorPhoneNumber"].ToString(), dr["DOCStatus"].ToString(), securityID, currDate, strLocationId);
                    if (recInserted != true)
                    {
                        break;
                    }
                 }

                if (recInserted == true)
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "SubmitSuccess", "<script language='javascript'>alert('Template Uploaded Successfully.');</script>", false);
                    this.Session["DataSourceFinal"] = null;
                    DataTable dttest = new DataTable();
                    dttest = (DataTable)Session["DataSourceFinal"];
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "SubmitSuccess", "<script language='javascript'>alert('Template not uploaded.Please try again.');</script>", false);
                }

                ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Preview edit click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnPreviewEdit_Click(object sender, EventArgs e)
        {
            this.divUpload.Visible = false;
            ////EditGrid.Visible = true;
            DataTable dt = new DataTable();
            ////lbtnBack.Visible = true;
            this.lbtnBack.Visible = false;
            this.lbtnUpload1.Visible = true;
            this.lbtnBack1.Visible = true;
            this.lbtnBack1.Enabled = false;
            this.lbtnValidate1.Visible = true;
            if (Convert.ToString(this.Session["InitialValiationCheck"]) == "1")
            {
                this.pnlError.Visible = true;
                BulletedList blmessages = new BulletedList();
                blmessages.DataSource = this.Session["InitialValiateContent"];
                blmessages.DataBind();
                this.pnlError.Controls.Add(blmessages);
                this.Session["InitialValiationCheck"] = null;
                this.Session["InitialValiateContent"] = null;
            }

            if (this.Session["DataSourceFinalCount"] != null)
            {
                dt = (DataTable)this.Session["DataSourceFinal"];
                if (Convert.ToString(this.Session["Duplicates"]) == "1")
                {
                    this.lblmessage2.Visible = true;
                    ////lbtnBack.Visible = true;
                    this.lbtnBack.Visible = false;
                    this.lbtnUpload1.Visible = true;
                    this.lbtnBack1.Visible = true;
                    this.lbtnValidate1.Visible = true;
                    this.lbtnValidate1.Enabled = false;
                    ////lbtnBack.Enabled = false;
                    this.lbtnBack.Enabled = true;
                    this.lbtnUpload1.Enabled = true;
                    this.lbtnBack1.Enabled = false;
                    this.lblmessage2.Text = "Removed Duplicate Entries";
                    this.Session["DataSource1"] = this.Session["DataSourceFinal"];
                    this.lbtnUpload1.Enabled = false;
                        this.lbtnValidate1.Visible = true;
                        this.lbtnValidate1.Enabled = true;
                }
                else
                {
                    ////lbtnBack.Visible = true;
                    this.lbtnBack.Visible = false;
                    this.lbtnUpload1.Visible = true;
                    this.lbtnUpload1.Enabled = true;
                    this.lbtnBack1.Visible = true;
                    this.lbtnBack1.Enabled = false;
                    this.lbtnValidate1.Visible = true;
                    this.lbtnValidate1.Enabled = false;
                    this.lbtnBack.Enabled = true;
                }
            }
            else
            {
                dt = (DataTable)this.Session["DataForPreviewLoad"];
                this.lbtnBack.Enabled = false;
                this.lbtnUpload1.Enabled = false;
                this.lbtnBack1.Enabled = false;
                this.lbtnValidate1.Visible = true;
            }

            this.dgAssociate.Visible = true;
            this.dgAssociate.DataSource = dt;
            this.dgAssociate.DataBind();
        }

        /// <summary>
        /// Associate row data bound
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Dgassociate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ArrayList armessages1 = new ArrayList();
#pragma warning disable CS0219 // The variable 'superVisiorPhoneCheck' is assigned but its value is never used
                bool superVisiorPhoneCheck = false;
#pragma warning restore CS0219 // The variable 'superVisiorPhoneCheck' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'nullFirstNameCheck' is assigned but its value is never used
                bool nullFirstNameCheck = false;
#pragma warning restore CS0219 // The variable 'nullFirstNameCheck' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'vendorNameCheck' is assigned but its value is never used
                bool vendorNameCheck = false;
#pragma warning restore CS0219 // The variable 'vendorNameCheck' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'vendorPhoneNumberCheck' is assigned but its value is never used
                bool vendorPhoneNumberCheck = false;
#pragma warning restore CS0219 // The variable 'vendorPhoneNumberCheck' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'contractorIdCheck' is assigned but its value is never used
                bool contractorIdCheck = false;
#pragma warning restore CS0219 // The variable 'contractorIdCheck' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'existingContractorId' is assigned but its value is never used
                bool existingContractorId = false;
#pragma warning restore CS0219 // The variable 'existingContractorId' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'dupContractorId' is assigned but its value is never used
                bool dupContractorId = false;
#pragma warning restore CS0219 // The variable 'dupContractorId' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'alpha' is assigned but its value is never used
                string alpha = "^[a-zA-Z]'?([a-zA-Z]|\\.| |-)+$";
#pragma warning restore CS0219 // The variable 'alpha' is assigned but its value is never used
                string alphaNumSpecial = "^[A-Za-z0-9\\s!@#$%^&*()_+=-`~\\][{}|';:/.,?><]*$";
                string patternForMobile = "^([0-9]{10})$";
                string patternForContractorId = "^([0-9]{6,15})$";
                int i = 0;

                foreach (Control lblEmpty in e.Row.Controls)
                {
                    if (lblEmpty.HasControls())
                    {
                        if (i <= 4)
                        {
                            string lbname = "Label" + (i + 1);

                            if (lbname == "Label1")
                            {
                                Label lbl = (Label)lblEmpty.FindControl("lblContratorId");

                                if (lbl == null)
                                {
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(lbl.Text.Trim()))
                                {
                                    if (Regex.IsMatch(lbl.Text.Trim(), patternForContractorId))
                                    {
                                    }
                                    else 
                                    { 
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                        contractorIdCheck = true;
                                    }

                                    if (lbl.Text.Trim().Length == 6)
                                    {
                                    }
                                    else
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                        contractorIdCheck = true;
                                    }
                              }
                            }
                            }

                            if (lbname == "Label2")
                            {
                                Label lbl = (Label)lblEmpty.FindControl("lblContractorName");
                                if (lbl == null)
                                { 
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(lbl.Text.ToString().Trim()))
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                        nullFirstNameCheck = true;
                                    }
                                    else
                                    {
                                        if (Regex.IsMatch(lbl.Text.ToString().Trim(), alphaNumSpecial))
                                        {
                                        }
                                        else 
                                        { 
                                            e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                            nullFirstNameCheck = true;
                                        }
                                    }
                                }
                            }

                            if (lbname == "Label3")
                            {
                                Label lbl = (Label)lblEmpty.FindControl("lblVendorName");
                                if (lbl == null)
                                {
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(lbl.Text.Trim()))
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                        vendorNameCheck = true;
                                    }
                                    else
                                    {
                                        if (Regex.IsMatch(lbl.Text.Trim(), alphaNumSpecial))
                                        { 
                                        }
                                        else 
                                        { 
                                            e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                            vendorNameCheck = true;
                                        }
                                    }
                                }
                            }

                            if (lbname == "Label4")
                            {
                                Label lbl = (Label)lblEmpty.FindControl("lblSuperVisiorPhone");
                                if (lbl == null)
                                { 
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(lbl.Text))
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                        superVisiorPhoneCheck = true;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(lbl.Text.Trim()))
                                        {
                                            if (Regex.IsMatch(lbl.Text.Trim(), patternForMobile))
                                            { 
                                            }
                                            else 
                                            { 
                                                e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                                superVisiorPhoneCheck = true;
                                            }
                                        }
                                    }
                                }
                            }

                            if (lbname == "Label5")
                            {
                                Label lbl = (Label)lblEmpty.FindControl("lblVendorPhoneNumber");
                                if (lbl == null)
                                { 
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(lbl.Text))
                                    {
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                        vendorPhoneNumberCheck = true;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(lbl.Text.Trim()))
                                        {
                                            if (Regex.IsMatch(lbl.Text.Trim(), patternForMobile))
                                            {
                                            }
                                            else 
                                            { 
                                                e.Row.Cells[i].BackColor = System.Drawing.Color.Pink;
                                                vendorPhoneNumberCheck = true;
                                            }
                                        }
                                    }
                                }
                            }

                            i++;
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
        /// Associate row editing function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Dgassociate_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                this.dgAssociate.EditIndex = e.NewEditIndex;

                ////chk - bincey
                this.dgAssociate.DataSource = this.Session["DataForPreviewLoad"];
                this.dgAssociate.DataBind();
                this.lbtnValidate1.Enabled = false;
                this.lbtnUpload1.Enabled = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// associate row cancel button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Dgassociate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                this.dgAssociate.EditIndex = -1;
                this.dgAssociate.DataSource = this.Session["DataForPreviewLoad"];
                this.dgAssociate.DataBind();
                this.lbtnValidate1.Enabled = true;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// associate row updating function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Dgassociate_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt1 = new DataTable();
            try
            {
                string contractorID = string.Empty;
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

                dt1 = (DataTable)Session["DataForPreviewLoad"];
                //// int i =  e.RowIndex;
                int i = e.RowIndex + (10 * this.dgAssociate.PageIndex);
                ////bincey
                ////Int32 strContractorId = Convert.ToInt32(dgAssociate.DataKeys[e.RowIndex].Value);
                ////end -- bincey
                TextBox tx1 = (TextBox)this.dgAssociate.Rows[e.RowIndex].FindControl("txtContractorId");
                TextBox tx2 = (TextBox)this.dgAssociate.Rows[e.RowIndex].FindControl("txtContractorName");
                TextBox tx3 = (TextBox)this.dgAssociate.Rows[e.RowIndex].FindControl("txtVendorName");
                TextBox tx4 = (TextBox)this.dgAssociate.Rows[e.RowIndex].FindControl("txtSuperVisiorPhone");
                TextBox tx5 = (TextBox)this.dgAssociate.Rows[e.RowIndex].FindControl("txtVendorPhoneNumber");
                DropDownList tx6 = (DropDownList)this.dgAssociate.Rows[e.RowIndex].FindControl("drpDocStatus");
                DropDownList tx7 = (DropDownList)this.dgAssociate.Rows[e.RowIndex].FindControl("drpStatus");

                if (!string.IsNullOrEmpty(tx1.Text))
                {
                    contractorID = Convert.ToString(tx1.Text);
                    
                    if (!requestDetails.CheckContratorNumberExist(Convert.ToString(tx1.Text), strLocationId))
                    {
                        dt1.Rows[i]["ContractorId"] = tx1.Text.ToString();
                        dt1.Rows[i]["ContractorName"] = tx2.Text.ToString();
                        dt1.Rows[i]["VendorName"] = tx3.Text.ToString();
                        dt1.Rows[i]["SupervisiorPhone"] = tx4.Text.ToString();
                        dt1.Rows[i]["VendorPhoneNumber"] = tx5.Text.ToString();
                        dt1.Rows[i]["DOCStatus"] = tx6.Text.ToString();
                        dt1.Rows[i]["Status"] = tx7.Text.ToString();
                        this.dgAssociate.EditIndex = -1;
                        this.Session["DataSource1"] = dt1;
                        ////added by bincey
                        this.Session["DataForPreviewLoad"] = null;
                        this.Session["DataForPreviewLoad"] = dt1;
                        this.Session["validatedinitial"] = dt1;
                        //// ends  - bincey
                        this.dgAssociate.DataSource = this.Session["DataSource1"];
                        this.dgAssociate.DataBind();
                        this.lbtnValidate1.Enabled = true;
                        ////lbtnBack.Visible = true;
                        this.lbtnBack.Visible = false;
                        this.lbtnUpload1.Visible = true;
                        this.lbtnUpload1.Enabled = false;
                        this.lbtnBack1.Visible = true;
                        this.lbtnBack1.Enabled = false;
                        this.lbtnBack.Enabled = false;
                        this.lblmessage2.Visible = false;
                        this.Session["UpdatedChecked"] = "1";
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "<script language='javascript'> alert('Duplicate Contractor ID .'); </script>");
                    }                  
                }
                else 
                {                   
                        dt1.Rows[i]["ContractorId"] = tx1.Text.ToString();
                        dt1.Rows[i]["ContractorName"] = tx2.Text.ToString();
                        dt1.Rows[i]["VendorName"] = tx3.Text.ToString();
                        dt1.Rows[i]["SupervisiorPhone"] = tx4.Text.ToString();
                        dt1.Rows[i]["VendorPhoneNumber"] = tx5.Text.ToString();
                        dt1.Rows[i]["DOCStatus"] = tx6.Text.ToString();
                        dt1.Rows[i]["Status"] = tx7.Text.ToString();
                        this.dgAssociate.EditIndex = -1;
                        this.Session["DataSource1"] = dt1;
                        ////added by bincey
                        this.Session["DataForPreviewLoad"] = null;
                        this.Session["DataForPreviewLoad"] = dt1;
                        this.Session["validatedinitial"] = dt1;
                        //// ends  - bincey
                        this.dgAssociate.DataSource = this.Session["DataSource1"];
                        this.dgAssociate.DataBind();
                        this.lbtnValidate1.Enabled = true;
                        ////lbtnBack.Visible = true;
                        this.lbtnBack.Visible = false;
                        this.lbtnUpload1.Visible = true;
                        this.lbtnUpload1.Enabled = false;
                        this.lbtnBack1.Visible = true;
                        this.lbtnBack1.Enabled = false;
                        this.lbtnBack.Enabled = false;
                        this.lblmessage2.Visible = false;
                        this.Session["UpdatedChecked"] = "1";               
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// associate row deleting
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Dgassociate_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = new DataTable();
            this.lbtnBack1.Enabled = false;
            try
            {
                if (this.Session["DataSource1"] != null)
                {
                    dt = (DataTable)this.Session["DataSource1"];
                    if (Convert.ToInt32(dt.Rows.Count) > 0)
                    {
                        dt = (DataTable)this.Session["DataSource1"];
                    }
                    else
                    {
                        if (this.Session["DataSourceFinal"] != null)
                        {
                            dt = (DataTable)this.Session["DataSourceFinal"];
                        }
                    }
                }

                this.Session["RowNumber"] = e.RowIndex + (10 * this.dgAssociate.PageIndex);
                ////changed from datasource1 -- bincey -- to DataForPreviewLoad -- then to validatedfinal
                dt = (DataTable)this.Session["DataForPreviewLoad"];
                this.dgAssociate.DataSource = dt;
                this.dgAssociate.DataBind();
                if (this.Session["ViewCandidates"] != null)
                {
                    this.Session["ViewCandidates"] = this.Session["DataSource1"];
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// associate page index changing
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Dgassociate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.dgAssociate.PageIndex = e.NewPageIndex;
                ////cnaged by bincey from DataSource1 to DataForPreviewLoad
                this.dgAssociate.DataSource = this.Session["DataForPreviewLoad"];
                this.dgAssociate.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Display the details of the Uploaded Excel
        /// </summary>
        protected void Display()
        {
            try
            {
                DataTable dt = (DataTable)this.Session["DataForPreviewLoad"];
                ////for (int i = Convert.ToInt32(dt.Rows.Count - 1); i >= 0; i--)
                ////{

                ////    if (!string.IsNullOrEmpty(dt.Rows[i]["VendorPhoneNumber"].ToString()))
                ////    {
                ////        if (((dt.Rows[i]["VendorPhoneNumber"].ToString().Contains(('+')))) || ((dt.Rows[i]["VendorPhoneNumber"].ToString().Contains(('-')))))
                ////        {
                ////            string[] values = dt.Rows[i]["VendorPhoneNumber"].ToString().Split('-');
                ////            dt.Rows[i]["VendorPhoneNumber"] = values[1];
                ////        }

                ////    }

                ////}
                this.dgAssociate.DataSource = dt;
                this.dgAssociate.DataBind();
                this.divUpload.Visible = false;
                this.lbtnValidate1.Visible = true;
                if (Convert.ToString(this.Session["Duplicates"]) == "1")
                {
                    this.lbtnBack.Visible = false;
                    this.lbtnBack.Enabled = false;
                    this.lbtnUpload1.Visible = true;
                    this.lbtnUpload1.Enabled = false;
                    this.lbtnBack1.Visible = true;
                    this.lbtnBack1.Enabled = false;
                }
                else
                {
                    this.lbtnBack.Visible = false;
                    this.lbtnUpload1.Visible = true;
                    this.lbtnUpload1.Enabled = false;
                    this.lbtnBack1.Visible = true;
                    this.lbtnValidate1.Enabled = true;
                    if (Convert.ToString(this.Session["Delete"]) == "1")
                    {
                        this.lbtnBack1.Visible = true;
                        this.lbtnBack1.Enabled = false;
                        this.lbtnUpload1.Enabled = false;
                        this.lbtnValidate1.Enabled = true;
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    this.lbtnValidate1.Enabled = false;
                    this.lbtnBack1.Enabled = true;
                    this.Validate.Enabled = true;
                    this.lbtnUpload.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// delete click button
        /// </summary>
        /// <param name="source">source parameter</param>
        /// <param name="e">event parameter</param>
        protected void LnkDelete_Click(object source, EventArgs e)
        {
            this.ModelPopupBulkUploadDelete.Show();
        }

        /// <summary>
        /// validate date click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnValidate1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtss = new DataTable();            
            dt1.Columns.Add("ContractorId", typeof(string));
            dt1.Columns.Add("ContractorName", typeof(string));
            dt1.Columns.Add("VendorName", typeof(string));
            dt1.Columns.Add("SuperVisiorPhone", typeof(string));
            dt1.Columns.Add("VendorPhoneNumber", typeof(string));
            dt1.Columns.Add("DOCStatus", typeof(string));
            dt1.Columns.Add("Status", typeof(string));
            this.divUpload.Visible = false;
            this.dgAssociate.Visible = true;
            bool mobileCheck = false;
            bool superVisiorPhoneCheck = false;
            ////bool c = false;
            bool nullFirstNameCheck = false;
            bool vendorNameCheck = false;
            bool vendorPhoneNumberCheck = false;
            bool contractorIdCheck = false;
            bool existingContractorId = false;
            bool dupContractorId = false;
            ArrayList armessages1 = new ArrayList();
            ArrayList arcontrols1 = new ArrayList();
            string alpha = "^[a-zA-Z]'?([a-zA-Z]|\\.| |-)+$";
            string alphaNumSpecial = "^[A-Za-z0-9\\s!@#$%^&*()_+=-`~\\][{}|';:/.,?><]*$";
            string patternForMobile = "^([0-9]{10})$";          ////"^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"; // "^[0-9]";
            string patternForContractorId = "^([0-9]{6,})$";    ////"^\\(?([0-9]{6})\\)$";
            string dupId = string.Empty;
            this.lblmessage2.Visible = true;
            ArrayList armessages = new ArrayList();
            ArrayList arcontrols = new ArrayList();
            int nullContId = 0;

            try
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL objMasterDataDL =
                    new VMSDataLayer.VMSDataLayer.MasterDataDL();
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails =
                   new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();

                ////changed by bincey frm validatedinitial to DataForPreviewLoad
                dt = (DataTable)Session["DataForPreviewLoad"];
                DataTable dttt = dt.Copy();
                dt1 = null;
                dt1 = dt.Copy();

                string strLocationId = string.Empty;
                DataSet dslocation = requestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
                DataTable dtlocation = dslocation.Tables[0];
                if (dtlocation.Rows.Count > 0)
                {
                    strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);
                }

                for (int i = Convert.ToInt32(dt.Rows.Count - 1); i >= 0; i--)
                {                    
                    Regex rgx = new Regex(patternForMobile, RegexOptions.IgnoreCase);
                    Regex regex = new Regex(alpha, RegexOptions.IgnoreCase);
                    if (!string.IsNullOrEmpty(dt.Rows[i]["ContractorId"].ToString()))
                    {
                        if (Regex.IsMatch(dt.Rows[i]["ContractorId"].ToString().Trim(), patternForContractorId))
                        {
                            dupContractorId = requestDetails.CheckContratorNumberExist(dt.Rows[i]["ContractorId"].ToString(), strLocationId);
                            {
                                if (dupContractorId == true)
                                {
                                    existingContractorId = true;
                                    dupId = dupId + dt.Rows[i]["ContractorId"].ToString() + ";";
                                }
                            }
                        }
                        else 
                        { 
                            contractorIdCheck = true;
                        }  
                      
                        if (dt.Rows[i]["ContractorId"].ToString().Trim().Length == 6)
                        { 
                        }
                        else
                        {
                            contractorIdCheck = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[i]["SuperVisiorPhone"].ToString()))
                    {
                        if (dt.Rows[i]["SuperVisiorPhone"].ToString().Contains('.'))
                        {
                            superVisiorPhoneCheck = true;
                        }

                        if (dt.Rows[i]["SuperVisiorPhone"].ToString().Trim().Length >= 10
                            && dt.Rows[i]["SuperVisiorPhone"].ToString().Trim().Length < 12)
                        {
                            ////MobileCheck = true;
                        }
                        else
                        {
                            superVisiorPhoneCheck = true;
                        }

                        if (Regex.IsMatch(dt.Rows[i]["SuperVisiorPhone"].ToString().Trim(), patternForMobile))
                        {
                        }
                        else 
                        { 
                            superVisiorPhoneCheck = true;
                        }
                        ////MatchCollection matches = rgx.Matches((dt.Rows[i]["SuperVisiorPhone"].ToString().Trim()));
                        ////if (matches.Count > 0)
                        ////{

                        ////}
                        ////else
                        ////{
                        ////    SuperVisiorPhoneCheck = true;
                        ////}
                    }

                    if (string.IsNullOrEmpty(dt.Rows[i]["SuperVisiorPhone"].ToString().Trim()))
                    {
                        superVisiorPhoneCheck = true;
                    }

                    if (string.IsNullOrEmpty(dt.Rows[i]["ContractorName"].ToString().Trim()))
                    {
                        nullFirstNameCheck = true;
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[i]["ContractorName"].ToString().Trim()))
                    {
                        if (Regex.IsMatch(dt.Rows[i]["ContractorName"].ToString().Trim(), alphaNumSpecial))
                        {
                            var ctrName = Regex.Replace(dt.Rows[i]["ContractorName"].ToString().Trim(), "[^a-zA-Z]+", string.Empty).Replace(".", string.Empty);
                            if (ctrName.Length < 2)
                            {
                                nullFirstNameCheck = true;
                            }
                        }
                        else 
                        { 
                            nullFirstNameCheck = true; 
                        }
                        ////MatchCollection matches3 = regex.Matches((dt.Rows[i]["ContractorName"].ToString().Trim()));
                        ////if (matches3.Count > 0)
                        ////{

                        ////}
                        ////else
                        ////{
                        ////    NullFirstNameCheck = true;
                        ////}
                    }

                    if (string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString().Trim()))
                    {
                        vendorNameCheck = true;
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString().Trim()))
                    {
                        if (Regex.IsMatch(dt.Rows[i]["VendorName"].ToString().Trim(), alphaNumSpecial))
                        {
                         var vendorName = Regex.Replace(dt.Rows[i]["VendorName"].ToString().Trim(), "[^a-zA-Z]+", string.Empty).Replace(".", string.Empty);
                         if (vendorName.Length < 2)
                         {
                             vendorNameCheck = true;
                         }
                        }
                        else 
                        { 
                            vendorNameCheck = true;
                        }
                        ////MatchCollection matches4 = regex.Matches((dt.Rows[i]["VendorName"].ToString().Trim()));
                        ////if (matches4.Count > 0)
                        ////{

                        ////}
                        ////else
                        ////{
                        ////    VendorNameCheck = true;
                        ////}
                    }

                    if (string.IsNullOrEmpty(dt.Rows[i]["VendorPhoneNumber"].ToString().Trim()))
                    {
                        vendorPhoneNumberCheck = true;
                    }

                    if (Regex.IsMatch(dt.Rows[i]["VendorPhoneNumber"].ToString().Trim(), patternForMobile))
                    { 
                    }
                    else 
                    { 
                        vendorPhoneNumberCheck = true; 
                    }
                    ////MatchCollection matches1 = rgx.Matches((dt.Rows[i]["VendorPhoneNumber"].ToString().Trim()));
                    ////if (matches1.Count > 0)
                    ////{

                    ////}
                    ////else
                    ////{
                    ////    VendorPhoneNumberCheck = true;
                    ////}
                    ////if ((dt.Rows[i]["FirstName"].ToString() == "") || (dt.Rows[i]["LastName"].ToString() == "") || (dt.Rows[i]["Gender"].ToString() == "") )
                    ////{
                    ////    NullCheck = true;

                    ////}

                    if ((string.IsNullOrEmpty(dt.Rows[i]["ContractorName"].ToString().Trim())) ||
                        (string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString().Trim())) ||
                        (string.IsNullOrEmpty(dt.Rows[i]["SuperVisiorPhone"].ToString().Trim()))
                        || string.IsNullOrEmpty(dt.Rows[i]["VendorPhoneNumber"].ToString().Trim()))
                    {
                        ////todo
                        ////MobileCheck = false;
                        ////MailCheck = false;
                        ////check = false;
                    }
                    else
                    {
                        DataTable dt4 = new DataTable();
                        DataRow delRow;
                        dt4 = dt.Clone();
                        delRow = (DataRow)dt.Rows[i];
                        dt4.ImportRow(delRow);
                        if (this.Session["validatedfinal"] != null)
                        {
                            dtss = (DataTable)Session["validatedfinal"];
                        }

                        dtss.Merge(dt4);
                        this.Session["validatedfinal"] = dtss.Copy();
                        dt.Rows.RemoveAt(i);
                    }
                }

                ////added by bincey

                if (existingContractorId == true)
                {
                    armessages1.Add("Plaese remove the duplicate entry for Contractor Id: " + dupId);
                }

                if (contractorIdCheck == true)
                {
                    armessages1.Add("Please enter valid 6 digit Contractor Id.");
                }

                if (superVisiorPhoneCheck == true)
                {
                    armessages1.Add("Supervisor Phone Number is not valid.");
                }

                if (nullFirstNameCheck == true)
                {
                    armessages1.Add("Contractor Name is not valid.");
                }

                if (vendorNameCheck == true)
                {
                    armessages1.Add("Vendor Name  is not valid.");
                }

                if (vendorPhoneNumberCheck == true)
                {
                    armessages1.Add("VendorPhone Number is not valid.");
                }

                ////ends here -- bincey

                //// bincey--dupid

                DataTable distinctIdTables = new DataTable();
                ////Columns["ContractorId"].Table;
                distinctIdTables.Columns.Add("ContractorId", typeof(string));   
                foreach (DataRow drcopy in dt1.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(drcopy["ContractorId"])))
                    {
                        nullContId++;
                        continue;
                    }
                    else
                    {
                        distinctIdTables.Rows.Add(drcopy["ContractorId"]);
                    }
                }

                DataTable distinctIdTablesUnique = distinctIdTables.DefaultView.ToTable(/*distinct*/ true);

                int count_Id = distinctIdTablesUnique.Rows.Count + nullContId;
                if (Convert.ToInt32(count_Id) != Convert.ToInt32(dt1.Rows.Count) && (distinctIdTablesUnique.Rows.Count != 0))
                {
                    armessages1.Add("Duplicate Contractor IDs exist.");
                    this.Validate.Enabled = true;         
                }

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (existingContractorId == true))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dttt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dttt;
                    this.Session["DataForPreviewLoad"] = dttt;
                }

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (superVisiorPhoneCheck == true))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dttt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dttt;
                    this.Session["DataForPreviewLoad"] = dttt;
                }

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (mobileCheck == true))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dttt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dttt;
                    this.Session["DataForPreviewLoad"] = dttt;
                }

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (nullFirstNameCheck == true))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dttt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dttt;
                    this.Session["DataForPreviewLoad"] = dttt;
                }

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (vendorNameCheck == true))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dttt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dttt;
                    this.Session["DataForPreviewLoad"] = dttt;
                }

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (superVisiorPhoneCheck == true))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dttt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dttt;
                    this.Session["DataForPreviewLoad"] = dttt;
                }

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (vendorPhoneNumberCheck == true))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dttt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dttt;
                    this.Session["DataForPreviewLoad"] = dttt;
                }
                ////if ((Convert.ToInt32(dt.Rows.Count) != 0) && ((check == true)))
                ////{
                ////    dgAssociate.Visible = true;
                ////    dgAssociate.DataSource = dttt;
                ////    dgAssociate.DataBind();
                ////    Session["DataSource1"] = dttt;
                ////}

                if ((Convert.ToInt32(dt.Rows.Count) != 0) && (existingContractorId == false) && (superVisiorPhoneCheck == false) && (contractorIdCheck == false) && (nullFirstNameCheck == false) && (vendorNameCheck == false) && (vendorPhoneNumberCheck == false))
                {
                    this.dgAssociate.Visible = true;
                    this.dgAssociate.DataSource = dt;
                    this.dgAssociate.DataBind();
                    this.Session["DataSource1"] = dt;
                }
                else if (Convert.ToInt32(dt.Rows.Count) == 0)
                {
                    DataTable dt4 = new DataTable();
                    dt4 = dt.Clone();
                    DataTable dt5 = new DataTable();
                    if (this.Session["validatedinitial"] != null)
                    {
                        dt4 = (DataTable)this.Session["validatedinitial"];
                    }
                    ////if entry is deleted
                    if (Convert.ToString(this.Session["Delete"]) == "1") 
                    {
                        if (Convert.ToString(this.Session["flagcheck"]) == "1") 
                        {
                            dt5 = dttt.Copy();
                            dt4 = dttt.Copy();
                        }
                        else if (Convert.ToString(this.Session["checked"]) == "1") 
                        {
                            dt5 = dttt.Copy();
                            dt4 = (DataTable)Session["validatedinitial"];
                        } 
                        else if (Convert.ToString(this.Session["notchecked"]) == "1") 
                        {
                            dt5 = dttt.Copy();
                            dt4 = (DataTable)this.Session["validatedinitial"];
                        }
                        else
                        {
                            dt5 = dttt.Copy();
                            dt4 = dttt.Copy();
                        }

                        this.Session["Delete"] = "0";
                    }
                    else
                    {
                        ////if validated succesfully on first validation
                        if ((Convert.ToInt32(dt.Rows.Count) == 0) && (Convert.ToString(this.Session["checked"]) == "1")) 
                        {
                            ////Not Fully validated for 2nd validation
                            if (Convert.ToString(this.Session["flagcheck"]) != "1") 
                            {
                                dt5 = (DataTable)this.Session["validatedfinal"];
                                dt4 = (DataTable)this.Session["validatedfinal"];
                            }
                            else if ((Convert.ToString(this.Session["flagcheck"]) == "1") && (Convert.ToString(this.Session["checked"]) == "1"))
                            {
                                dt5 = dttt.Copy();
                                dt4 = dttt.Copy();
                            }
                            else
                            {
                                dt5 = dttt.Copy();
                                dt4 = dttt.Copy();
                            }
                        }
                        else
                        {
                            if ((Convert.ToString(this.Session["UpdatedChecked"]) == "1") && (Convert.ToString(this.Session["notchecked"]) == null)) 
                            {
                                dt5 = dttt.Copy();
                                dt4 = dttt.Copy();
                            }
                            else if (Convert.ToString(this.Session["notchecked"]) == "1") 
                            {
                                dt5 = dttt.Copy();
                                dt4 = (DataTable)this.Session["validatedinitial"];
                            }
                            else if ((Convert.ToString(this.Session["flagcheck"]) == "1") && (Convert.ToString(this.Session["UpdatedChecked"]) == "1"))
                            {
                                dt5 = dttt.Copy();
                                dt4 = dttt.Copy();
                            }
                            else if (Convert.ToString(this.Session["Duplicates"]) == "1")
                            {
                                dt5 = dttt.Copy();
                                dt4 = dttt.Copy();
                            }
                            else
                            {
                                dt5 = (DataTable)this.Session["validatedfinal"];
                            }
                        }
                    }

                    this.Session["notchecked"] = null;
                    if (dt5 != null && dt4 != null)
                    {
                        dt4.Merge(dt5);
                    }
                    else
                    {
                        dt4 = dt5.Copy();
                    }

                    DataTable distinctTables = dt4.DefaultView.ToTable(/*distinct*/ true);

                    this.dgAssociate.DataSource = distinctTables;
                    this.dgAssociate.DataBind();

                    this.Session["DataSourceFinal"] = distinctTables.Copy();
                    this.Session["DataSourceFinalCount"] = distinctTables.Rows.Count;
                    this.lbtnValidate1.Enabled = false;
                    int count = distinctTables.Rows.Count;
                    this.Session["DataSource1"] = this.Session["DataSourceFinal"];
                    this.Session["DataForPreviewLoad"] = this.Session["DataSourceFinal"];
                    this.Session["ViewCandidates"] = distinctTables.Copy();
                    string message = string.Empty;
                    if (Convert.ToString(this.Session["Duplicates"]) == "1")
                    {
                        ////message = "Removed Duplicate Entries";
                        this.Session["Duplicates"] = null;
                    }
                    ////lblmessage2.Text = message+ "<br/>"+ count + " entries successfully validated. Go Back to upload the details";
                    this.lblmessage2.Text = count + " entries successfully validated. Upload the details.";
                    this.lbtnUpload1.Visible = true;
                    this.lbtnUpload1.Enabled = true;
                    this.lbtnBack1.Visible = true;
                    this.lbtnBack1.Enabled = false;
                    this.Session["flagcheck"] = "1";
                    ////lbtnBack.Enabled = true;
                    this.lbtnBack.Visible = false;
                    this.Session["UpdatedChecked"] = null;
                }

                if (armessages1.Count > 0)
                {
                    this.lblmessage2.Visible = false;
                    BulletedList blmessages = new BulletedList();
                    blmessages.DataSource = armessages1;
                    blmessages.DataBind();
                    blmessages.DataSource = armessages1;
                    this.pnlError.Controls.Add(blmessages);
                    this.pnlError.Visible = true;
                    this.lbtnBack.Enabled = false;
                    this.lbtnUpload1.Enabled = false;
                    this.lbtnBack1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// button upload click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnUpload1_Click(object sender, EventArgs e)
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails =
                new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                string securityID = string.Empty;
                DataTable dt = new DataTable();
                bool recInserted = false;
                this.dgAssociate.Visible = false;
                this.divUpload.Visible = true;
                this.lbtnValidate1.Visible = false;
                this.lbtnBack.Visible = false;
                this.lbtnUpload1.Visible = false;
                this.Validate.Enabled = false;
                this.lbtnUpload.Enabled = true;
                this.lblmessage.Visible = false;
                this.lblmessage2.Visible = false;
                this.lblSuccess.Visible = false;
                ////added by bincey for data insert

                dt = (DataTable)Session["DataSourceFinal"];
                securityID = Session["LoginID"].ToString();
                string strLocationId = string.Empty;
                DataSet dslocation = requestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
                DataTable dtlocation = dslocation.Tables[0];
                if (dtlocation.Rows.Count > 0)
                {
                    strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);
                }

                DateTime currDate = DateTime.Now;
                foreach (DataRow dr in dt.Rows)
                {
                    VMSBusinessLayer.VMSBusinessLayer.MasterDataBL objMasterDataBL = new VMSBusinessLayer.VMSBusinessLayer.MasterDataBL();
                    recInserted = objMasterDataBL.InsertContractorIDDetails(dr["ContractorName"].ToString(), dr["ContractorId"].ToString(), dr["VendorName"].ToString(), dr["Status"].ToString(), dr["SuperVisiorPhone"].ToString(), dr["VendorPhoneNumber"].ToString(), dr["DOCStatus"].ToString(), securityID, currDate, strLocationId);
                    if (recInserted != true)
                    {
                        break;
                    }
                }

                ////ends here

                if (this.Session["ViewCandidates"] != null)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)Session["ViewCandidates"];
                    int viewCount = dt1.Rows.Count;
                    this.Session["DataSourceFinal"] = this.Session["ViewCandidates"];
                    this.Session["DataSourceFinalCount"] = dt1.Rows.Count;
                    if (this.Session["Visitor"] == null)
                    {
                        this.Session["DataSourceFinalCount"] = viewCount;
                        this.lbtnUpload.Visible = false;
                        this.divUpload.Visible = false;
                        ////ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
                    }
                }

                if (Convert.ToString(this.Session["ViewCancelImage"]) == "1") 
                {
                    this.ImgCancelUpload.Visible = false;
                    ////Session["ViewCancelImage"] = null;
                    this.divUpload.Visible = false;
                    this.Bulk.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
                    this.Session["ViewCancelImage"] = null;
                }
                else
                {
                    if (recInserted == true)
                    {
                        this.divUpload.Visible = false;
                        this.Bulk.Visible = false;
                        ClientScript.RegisterStartupScript(typeof(Page), "SubmitSuccess", "<script language='javascript'>alert('Template Uploaded Successfully.');</script>", false);
                        ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), "SubmitSuccess", "<script language='javascript'>alert('Template not uploaded.Please try again.');</script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// back click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                this.dgAssociate.Visible = false;
                this.divUpload.Visible = true;
                this.lbtnValidate1.Visible = false;
                this.lbtnBack.Visible = false;
                this.Validate.Enabled = false;
                this.lbtnUpload.Enabled = true;
                this.lblmessage.Visible = false;
                this.lblmessage2.Visible = false;
                this.lblSuccess.Visible = false;
                if (this.Session["ViewCandidates"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["ViewCandidates"];
                    int viewCount = dt.Rows.Count;
                    this.Session["DataSourceFinal"] = this.Session["ViewCandidates"];
                    this.Session["DataSourceFinalCount"] = dt.Rows.Count;
                    if (this.Session["Visitor"] == null)
                    {
                        this.Session["DataSourceFinalCount"] = viewCount;
                        this.lbtnUpload.Visible = false;
                        this.divUpload.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// back click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnBack1_Click(object sender, EventArgs e)
        {
            try
            {
                this.dgAssociate.Visible = false;
                this.divUpload.Visible = true;
                this.lbtnValidate1.Visible = false;
                this.lbtnBack.Visible = false;
                this.lbtnUpload1.Visible = false;
                this.Validate.Enabled = true;
                this.lbtnUpload.Enabled = false;
                this.lblmessage.Visible = false;
                this.lblmessage2.Visible = false;
                this.lblSuccess.Visible = false;
                if (this.Session["ViewCandidates"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)this.Session["ViewCandidates"];
                    int viewCount = dt.Rows.Count;
                    this.Session["DataSourceFinal"] = this.Session["ViewCandidates"];
                    this.Session["DataSourceFinalCount"] = dt.Rows.Count;
                    if (this.Session["Visitor"] == null)
                    {
                        this.Session["DataSourceFinalCount"] = viewCount;
                        this.lbtnUpload.Visible = false;
                        this.divUpload.Visible = false;
                        ////ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
                    }
                }

                if (Convert.ToString(this.Session["ViewCancelImage"]) == "1") 
                {
                    this.ImgCancelUpload.Visible = false;
                    ////Session["ViewCancelImage"] = null;
                    this.divUpload.Visible = false;
                    this.Bulk.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
                    this.Session["ViewCancelImage"] = null;
                }
                else
                {
                    ////changed by bincey                   

                    this.divUpload.Visible = true;
                    this.Bulk.Visible = true;
                    this.lbtnBack1.Visible = false;
                    ////Session["DataForPreviewLoad"] = null;
                    ////ClientScript.RegisterStartupScript(typeof(Page), "SubmitSuccess", "<script language='javascript'>alert('Template Uploaded Successfully.');</script>", false);
                    ////ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// cancel upload click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void ImgCancelUpload_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "CancelUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
        }

        /// <summary>
        /// Validate the Excel On Initial Validate Click
        /// </summary>
        #region validation for metrics
        private void Validates()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtss = new DataTable();
            dt1.Columns.Add("ContractorId", typeof(string));
            dt1.Columns.Add("ContractorName", typeof(string));
            dt1.Columns.Add("VendorName", typeof(string));
            dt1.Columns.Add("SuperVisiorPhone", typeof(string));
            dt1.Columns.Add("VendorPhoneNumber", typeof(string));
            dt1.Columns.Add("DOCStatus", typeof(string));
            dt1.Columns.Add("Status", typeof(string));

            this.divUpload.Visible = true;

            ////bool MobileCheck = false;
            bool superVisiorPhoneCheck = false;
            ////bool c = false;
            bool nullFirstNameCheck = false;
            bool vendorNameCheck = false;
            bool vendorPhoneNumberCheck = false;
            bool contractorIdCheck = false;
            bool existingContractorId = false;
            bool dupContractorId = false;
            ////ArrayList arMessages1 = new ArrayList();
            ArrayList arcontrols1 = new ArrayList();
            string alpha = "^[a-zA-Z]'?([a-zA-Z]|\\.| |-)+$";   ////"^[A-Z]";
            string alphaNumSpecial = "^[A-Za-z0-9\\s!@#$%^&*()_+=-`~\\][{}|';:/.,?><]*$";   ////@"[a-zA-Z0-9@#$%&*+\-_(),+':;?.,![]\s\\/]+$";
            string patternForMobile = "^([0-9]{1,11})$";          ////"^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"; // "^[0-9]";
            string patternForContractorId = "^([0-9]{6,})$";    ////"^\\(?([0-9]{6})\\)$";
            string dupId = string.Empty;
            int nullContId = 0;
            try
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL objMasterDataDL =
                new VMSDataLayer.VMSDataLayer.MasterDataDL();
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails =
                   new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();

                dt = (DataTable)Session["DataSource1"];
                this.Session["DataForPreviewLoad"] = null;
                this.Session["DataForPreviewLoad"] = this.Session["DataSource1"];

                dt1 = dt.Copy();
                this.Session["DataForPreviewLoad"] = null;
                this.Session["DataForPreviewLoad"] = dt1;
                string strLocationId = string.Empty;
                DataSet dslocation = requestDetails.GetSecurityCity(Convert.ToString(HttpContext.Current.Session["LoginId"]));
                DataTable dtlocation = dslocation.Tables[0];
                if (dtlocation.Rows.Count > 0)
                {
                    strLocationId = Convert.ToString(dtlocation.Rows[0]["LocationId"]);
                }

                for (int i = Convert.ToInt32(dt.Rows.Count - 1); i >= 0; i--)
                {
                    Regex rgx = new Regex(patternForMobile, RegexOptions.IgnoreCase);
                    Regex regex = new Regex(alpha, RegexOptions.IgnoreCase);
                    if (!string.IsNullOrEmpty(dt.Rows[i]["ContractorId"].ToString().Trim()))
                    {
                        if (Regex.IsMatch(dt.Rows[i]["ContractorId"].ToString().Trim(), patternForContractorId))
                        {
                            dupContractorId = requestDetails.CheckContratorNumberExist(dt.Rows[i]["ContractorId"].ToString().Trim(), strLocationId);
                            {
                                if (dupContractorId == true)
                                {
                                    existingContractorId = true;
                                    dupId = dupId + dt.Rows[i]["ContractorId"].ToString().Trim() + ";";
                                }
                            }
                        }
                        else
                        {
                            contractorIdCheck = true;
                        }

                        //if (dt.Rows[i]["ContractorId"].ToString().Trim().Length == 6)
                        if (dt.Rows[i]["ContractorId"].ToString().Trim().Length >= 6)
                        {
                            ////ContractorIdCheck = false;
                        }
                        else
                        {
                            contractorIdCheck = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[i]["SuperVisiorPhone"].ToString()))
                    {
                        if (dt.Rows[i]["SuperVisiorPhone"].ToString().Contains('.'))
                        {
                            superVisiorPhoneCheck = true;
                        }

                        if (dt.Rows[i]["SuperVisiorPhone"].ToString().Trim().Length >= 1 && dt.Rows[i]["SuperVisiorPhone"].ToString().Trim().Length < 12)
                        {
                        }
                        else
                        {
                            superVisiorPhoneCheck = true;
                        }

                        ////if( Convert.ToBoolean(Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "ShowITLink", "onlyNumbers()", true)))

                        if (Regex.IsMatch(dt.Rows[i]["SuperVisiorPhone"].ToString().Trim(), patternForMobile))
                        {
                        }
                        else
                        {
                            superVisiorPhoneCheck = true;
                        }
                    }

                    if (string.IsNullOrEmpty(dt.Rows[i]["SuperVisiorPhone"].ToString().Trim()))
                    {
                        superVisiorPhoneCheck = true;
                    }

                    if (string.IsNullOrEmpty(dt.Rows[i]["ContractorName"].ToString().Trim()))
                    {
                        nullFirstNameCheck = true;
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[i]["ContractorName"].ToString().Trim()))
                    {
                        if (Regex.IsMatch(dt.Rows[i]["ContractorName"].ToString().Trim(), alpha))
                        {
                            string removedSpecialCharCWRname = Regex.Replace(dt.Rows[i]["ContractorName"].ToString().Trim(), "[^a-zA-Z0-9]+", string.Empty, RegexOptions.Compiled).Replace(".", string.Empty);
                            if (removedSpecialCharCWRname.Length < 2)
                            {
                                nullFirstNameCheck = true;
                            }
                        }
                        else
                        {
                            nullFirstNameCheck = true;
                        }
                    }

                    if (string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString().Trim()))
                    {
                        vendorNameCheck = true;
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString().Trim()))
                    {
                        if (Regex.IsMatch(dt.Rows[i]["VendorName"].ToString().Trim(), alphaNumSpecial))
                        {
                            string removedSpecialCharVendorName = Regex.Replace(dt.Rows[i]["VendorName"].ToString().Trim(), "[^a-zA-Z0-9]+", string.Empty, RegexOptions.Compiled).Replace(".", string.Empty);
                            if (removedSpecialCharVendorName.Length < 2)
                            {
                                vendorNameCheck = true;
                            }
                        }
                        else
                        {
                            vendorNameCheck = true;
                        }
                    }

                    if (dt.Rows[i]["VendorPhoneNumber"].ToString().Trim() == string.Empty)
                    {
                        vendorPhoneNumberCheck = true;
                    }

                    if (Regex.IsMatch(dt.Rows[i]["VendorPhoneNumber"].ToString().Trim(), patternForMobile))
                    {
                    }
                    else
                    {
                        vendorPhoneNumberCheck = true;
                    }

                    if ((dt.Rows[i]["ContractorName"].ToString().Trim() == string.Empty) ||
                        (dt.Rows[i]["VendorName"].ToString().Trim() == string.Empty) ||
                        (dt.Rows[i]["SuperVisiorPhone"].ToString().Trim() == string.Empty)
                        || dt.Rows[i]["VendorPhoneNumber"].ToString().Trim() == string.Empty)
                    {
                        ////todo
                        ////MobileCheck = false;
                        ////MailCheck = false;
                        ////check = false;
                    }
                    else
                    {
                        DataTable dts = new DataTable();
                        DataRow delRow;
                        dts = dt.Clone();
                        delRow = (DataRow)dt.Rows[i];
                        dts.ImportRow(delRow);
                        if (this.Session["validatedinitial"] != null)
                        {
                            dtss = (DataTable)Session["validatedinitial"];
                        }

                        dtss.Merge(dts);
                        this.Session["validatedinitial"] = dtss.Copy();
                        dt.Rows[i].Delete();
                    }
                }

                ////added by Bincey//
                if (existingContractorId == true)
                {
                    this.armessages2.Add("Contractor ID already exisits for: " + dupId);
                }

                if (contractorIdCheck == true)
                {
                    this.armessages2.Add("Please enter valid 6 digit Contractor Id.");
                }

                if (superVisiorPhoneCheck == true)
                {
                    this.armessages2.Add("Supervisor Phone Number is not valid.");
                }

                if (nullFirstNameCheck == true)
                {
                    this.armessages2.Add("Contractor Name is not valid.");
                }

                if (vendorNameCheck == true)
                {
                    this.armessages2.Add("Vendor Name  is not valid.");
                }

                if (vendorPhoneNumberCheck == true)
                {
                    this.armessages2.Add("VendorPhone Number is not valid.");
                }
                ////ends

                this.dgAssociate.Visible = false;
                this.dgAssociate.DataSource = dt;
                this.dgAssociate.DataBind();
                ////commented by bincey for the grid loading bug fix in preview n edit button
                ////Session["DataSource1"] = dt;
                DataTable distinctTable = dt1.DefaultView.ToTable(/*distinct*/ true);
                DataTable distinctIdTables = new DataTable();
                distinctIdTables.Columns.Add("ContractorId", typeof(string));   ////Columns["ContractorId"].Table;
                foreach (DataRow drcopy in dt1.Rows)
                {
                    if (Convert.ToString(drcopy["ContractorId"]) == string.Empty)
                    {
                        nullContId++;
                        continue;
                    }
                    else
                    {
                        distinctIdTables.Rows.Add(drcopy["ContractorId"]);
                    }
                }

                DataTable distinctIdTablesUnique = distinctIdTables.DefaultView.ToTable(/*distinct*/ true);
                int count_Id = distinctIdTablesUnique.Rows.Count + nullContId;
                if (Convert.ToInt32(count_Id) != Convert.ToInt32(dt1.Rows.Count) && (distinctIdTablesUnique.Rows.Count != 0))
                {
                    this.lblSuccess.Visible = false;
                    this.lblmessage.Visible = true;
                    this.lblmessage.Text = "Duplicate Contractor Ids in Excel";
                    this.Validate.Enabled = true;
                    this.lbtnPreviewEdit.Enabled = false;
                }
                else
                {
                    if (Convert.ToInt32(distinctTable.Rows.Count) != Convert.ToInt32(dt1.Rows.Count))
                    {
                        this.lblSuccess.Visible = false;
                        this.lblmessage.Visible = true;
                        this.lblmessage.Text = "Please Remove Duplicate Entries from Excel";
                        this.lbtnValidate1.Visible = false;
                        this.Session["DataSourceFinal"] = distinctTable;
                        int count = distinctTable.Rows.Count;

                        this.Session["DataSourceFinalCount"] = count;
                        this.Session["Duplicates"] = "1";
                        this.Session["DataForPreviewLoad"] = distinctTable;

                        if (this.armessages2.Count > 0)
                        {
                            this.Session["InitialValiationCheck"] = "1";
                            BulletedList blmessages = new BulletedList();
                            blmessages.DataSource = this.armessages2;
                            blmessages.DataBind();
                            this.pnlError.Controls.Add(blmessages);
                            this.Session["InitialValiateContent"] = this.armessages2;
                            this.pnlError.Visible = false;
                            ////lbtnBack.Enabled = false;
                        }
                    }
                    else
                    {
                        if ((Convert.ToInt32(dt.Rows.Count) == 0) && (this.armessages2.Count == 0))
                        {
                            this.lblSuccess.Visible = true;
                            int count = dt1.Rows.Count;
                            this.lblSuccess.Text = " * " + count + " entries successfully validated. " + "<br/>" + "Upload the entries.";

                            dt = (DataTable)Session["validatedinitial"];
                            dt1.Clear();
                            dt1.Merge(dt); ////added

                            this.Session["DataSourceFinal"] = dt1;
                            this.Session["DataSourceFinalCount"] = count;
                            ////commented by bicney
                            ////Session["DataSource1"] = dt1;
                            this.lbtnValidate1.Visible = false;
                            this.dgAssociate.Visible = false;
                            this.dgAssociate.DataSource = dt1;
                            this.dgAssociate.DataBind();
                            this.lbtnValidate1.Enabled = false;
                            this.Session["checked"] = "1";
                            this.lbtnUpload.Enabled = true;
                            this.lbtnPreviewEdit.Enabled = true;
                        }
                        else
                        {
                            this.Session["notchecked"] = "1";
                            this.Error.Visible = true;
                            this.lbtnValidate1.Visible = false;
                            this.lbtnBack.Visible = false;
                            this.lbtnUpload.Enabled = false;

                            if (this.armessages2.Count > 0)
                            {
                                this.Session["InitialValiationCheck"] = "1";
                                BulletedList blmessages = new BulletedList();
                                blmessages.DataSource = this.armessages2;
                                blmessages.DataBind();
                                this.pnlError.Controls.Add(blmessages);
                                this.Session["InitialValiateContent"] = this.armessages2;
                                this.pnlError.Visible = false;
                                ////lbtnBack.Enabled = false;
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
        #endregion

        /// <summary>
        /// View details
        /// </summary>
        private void ViewDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)this.Session["ViewCandidates"];
                this.dgAssociate.Visible = true;
                this.dgAssociate.DataSource = dt;
                this.dgAssociate.DataBind();
                this.Session["DataSource1"] = dt.Copy();
                this.lbtnBack.Visible = false;
                this.lbtnUpload1.Visible = true;
                this.lbtnBack1.Visible = true;
                this.lbtnBack1.Enabled = true;
                this.lbtnValidate1.Visible = true;
                this.Session["View"] = "1";
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);                
            }
        }
        
        /// <summary>
        /// Upload the Excel 
        /// </summary>
        private void BulkUploadAssociate()
        {
            try
            {
                this.Session["DataSource"] = this.ValidateUploadFile();
                this.dgAssociate.DataSource = this.Session["DataSource"];
                this.dgAssociate.DataBind();
                this.dgAssociate.Visible = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);                
            }
        }

        /// <summary>
        /// To validate uploaded file
        /// </summary>
        /// <returns>record values</returns>
        private DataTable ValidateUploadFile()
        {
            try
            {
                string destinationPath = string.Empty;
                HttpPostedFile filetoUpload = this.btnUpload.PostedFile;
                string fileExtension = Path.GetExtension(filetoUpload.FileName.Replace("\\", string.Empty).Replace("..", string.Empty));
                if (string.IsNullOrEmpty(filetoUpload.FileName))
                {
                    this.lblmessage.Text = VMSConstants.VMSConstants.INVALIDFILEFORMAT;
                }
                else
                {
                    string serverPath = HttpContext.Current.Server.MapPath(@"UploadTemplate/");
                    destinationPath = string.Concat(serverPath, "Associate", "-", DateTime.Now.ToString("dd-MM-yy"), " ", DateTime.Now.ToString("hh-mm-ss-fff"), fileExtension);
                    filetoUpload.SaveAs(destinationPath);
                }

                FileInfo fileinfo = new FileInfo(destinationPath);
                if (fileinfo.Length <= 0)
                {
                    this.lblmessage.Text = VMSConstants.VMSConstants.UPLOADINVALIDFILE;
                    File.Delete(destinationPath);
                    return null;
                }

                if ((fileinfo.Extension.ToLower() != VMSConstants.VMSConstants.ISXLSXFILE) && (fileinfo.Extension.ToLower() != VMSConstants.VMSConstants.ISXLSFILE))
                {
                    this.lblmessage.Text = VMSConstants.VMSConstants.UPLOADFILEERROR;
                    File.Delete(destinationPath);
                    return null;
                }

                DataTable dtcell = this.CheckRecords(destinationPath);
                return dtcell;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                throw ex;
            }
        }

        /// <summary>
        /// Check Records
        /// </summary>
        /// <param name="strCurrentExcelPath">current excel path</param>
        /// <returns>record values</returns>
        private DataTable CheckRecords(string strCurrentExcelPath)
        {
            FileInfo fs = new FileInfo(strCurrentExcelPath);
            string strConnection = string.Empty;
            DataTable dtcell = new DataTable();
            dtcell.Columns.Add("ContractorId", typeof(string));
            dtcell.Columns.Add("ContractorName", typeof(string));
            dtcell.Columns.Add("VendorName", typeof(string));
            dtcell.Columns.Add("SuperVisiorPhone", typeof(string));
            dtcell.Columns.Add("VendorPhoneNumber", typeof(string));
            dtcell.Columns.Add("DOCStatus", typeof(string));
            dtcell.Columns.Add("Status", typeof(string));
            DataRow row1;
            bool worksheetstatus = true;
            try
            {
                using (ExcelPackage xlpackage = new ExcelPackage(fs))
                {
                    //// get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlpackage.Workbook.Worksheets["Sheet1"];
                    int icol = 2;  //// the column to read
                    int irow = 2;
                    if ((worksheet.Cell(1, 1).Value.ToUpper() != "CONTRACTORID")
                        || (worksheet.Cell(1, 2).Value.ToUpper() != "CONTRACTORNAME")
                        || (worksheet.Cell(1, 3).Value.ToUpper() != "VENDORNAME")
                          || (worksheet.Cell(1, 4).Value.ToUpper() != "SUPERVISIORPHONE")
                          || (worksheet.Cell(1, 5).Value.ToUpper() != "VENDORPHONENUMBER")
                          || (worksheet.Cell(1, 6).Value.ToUpper() != "DOCSTATUS")
                          || (worksheet.Cell(1, 7).Value.ToUpper() != "STATUS"))
                    {
                        worksheetstatus = false;
                    }

                    if (worksheetstatus == true)
                    {
                        while ((!string.IsNullOrEmpty(worksheet.Cell(irow, icol).Value)) || (!string.IsNullOrEmpty(worksheet.Cell(irow, icol + 1).Value)) || (!string.IsNullOrEmpty(worksheet.Cell(irow, icol + 2).Value)) || (!string.IsNullOrEmpty(worksheet.Cell(irow, icol + 3).Value)) || (!string.IsNullOrEmpty(worksheet.Cell(irow, icol + 4).Value)) || (!string.IsNullOrEmpty(worksheet.Cell(irow, icol + 5).Value)))
                        {
                            string ab = worksheet.Cell(irow, icol - 1).Value;
                            row1 = dtcell.NewRow();
                            row1["ContractorId"] = ab;
                            row1["ContractorName"] = worksheet.Cell(irow, icol).Value;
                            row1["VendorName"] = worksheet.Cell(irow, icol + 1).Value;
                            row1["SuperVisiorPhone"] = worksheet.Cell(irow, icol + 2).Value;
                            row1["VendorPhoneNumber"] = worksheet.Cell(irow, icol + 3).Value;
                            if (string.IsNullOrEmpty(worksheet.Cell(irow, icol + 4).Value))
                            {
                                row1["DOCStatus"] = "Completed";
                            }
                            else
                            {
                                row1["DOCStatus"] = worksheet.Cell(irow, icol + 4).Value;
                            }

                            if (string.IsNullOrEmpty(worksheet.Cell(irow, icol + 5).Value))
                            {
                                row1["Status"] = "Active";
                            }
                            else
                            {
                                row1["Status"] = worksheet.Cell(irow, icol + 5).Value;
                            }

                            dtcell.Rows.Add(row1);
                            row1 = null;
                            irow++;
                        }
                    }
                }

                if (worksheetstatus == false)
                {
                    File.Delete(strCurrentExcelPath);
                    this.Session["DataSource1"] = null;
                    this.lblmessage.Text = "Incorrect Excel file.Please download pre defined template and then upload.";
                    return null;
                }

                if (Convert.ToInt16(dtcell.Rows.Count) == 0)
                {
                    this.lblmessage.Text = VMSConstants.VMSConstants.UPLOADEMPTYRECORDS;
                    File.Delete(strCurrentExcelPath);
                    this.Session["DataSource1"] = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("File contains corrupted data"))
                {
                    this.lblmessage.Text = "Incorrect Excel file";
                    return null;
                }
                else
                {
                    Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                    return null;
                }
            }

            File.Delete(strCurrentExcelPath);
            this.Session["DataSource1"] = dtcell;
            DataTable dtvisitor = dtcell.Copy();
            this.Session["Visitor"] = dtvisitor;
            return dtcell;
        }

        /// <summary>
        /// special character validation function
        /// </summary>
        /// <returns>validated data</returns>
        private bool SpecialCharacterValidation()
        {
            try
            {
                DataTable validateAssociatetbl = new DataTable();
                validateAssociatetbl = (DataTable)Session["BulkUploadDataSource"];
                bool returnVal;
                for (int count = 0; count < validateAssociatetbl.Rows.Count; count++)
                {
                    Regex objNotNumberPattern = new Regex("[^0-9.-]");
                    Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
                    Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
                    string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
                    string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
                    Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
                    returnVal = !objNotNumberPattern.IsMatch(validateAssociatetbl.Rows[count][0].ToString()) &&
                    !objTwoDotPattern.IsMatch(validateAssociatetbl.Rows[count][0].ToString()) &&
                    !objTwoMinusPattern.IsMatch(validateAssociatetbl.Rows[count][0].ToString()) &&
                    objNumberPattern.IsMatch(validateAssociatetbl.Rows[count][0].ToString());
                    if (returnVal == false)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }

            return true;
        }
    }
}
