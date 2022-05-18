
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Ionic.Zip;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Associate id card list
    /// </summary>
    public partial class AssociateIDCardList : System.Web.UI.Page
    { 
        /// <summary>
        /// Gets or sets the property
        /// </summary>        
        public SortDirection Dir
        {
            get
            {
                if (this.ViewState["dirState"] == null)
                {
                    this.ViewState["dirState"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["dirState"];
            }

            set
            {
                this.ViewState["dirState"] = value;
            }
        }
        
        /// <summary>        
        /// To replace special character
        /// </summary>
        /// <param name="str">receiving string</param>
        /// <returns>returning string</returns>
        public static string RegExValidate(string str)
        {
            string pattern = @"[;:>,<*]";

            return Regex.Replace(str, pattern, string.Empty);
        }

        /// <summary>
        /// The IDCardLocations method
        /// </summary>        
        public void InitIDCardLocations()
        {
            try
            {
                VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL locationDetails = new VMSBusinessLayer.VMSBusinessLayer.LocationDetailsBL();
                DataTable dtfacilities = new DataTable();
                dtfacilities = locationDetails.GetIDCardLocationDetails();
                this.drpCity.DataSource = dtfacilities;
                this.drpCity.DataBind();
                this.drpCity.Items.Insert(0, new ListItem("Select", string.Empty));
                if (this.Session["UserRole"] != null)
                {
                    List<string> roles = (List<string>)Session["UserRole"];
                    if (roles[0] == "IDCardAdmin")
                    {
                        if (this.Session["LoginID"] != null)
                        {
                            string userId = Convert.ToString(Session["LoginID"]).Trim();
                            VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL objUserRoleLoc = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
                            string city = objUserRoleLoc.GetIDCardLocationForUser(userId).Rows[0]["City"].ToString();
                            if (!string.IsNullOrEmpty(city))
                            {
                                this.drpCity.Items.FindByText(city.ToString().Trim()).Selected = true;
                                this.drpCity.Enabled = false;
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
        /// The VerifyRenderingInServerForm method
        /// </summary>
        /// <param name="control">The control parameter</param>        
        public override void VerifyRenderingInServerForm(Control control)
        {
            //// Confirms that an HtmlForm control is rendered for the specified ASP.NET      server control at run time. 
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
                if (!this.IsPostBack)
                {
                    this.InitIDCardLocations();
                    this.txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtFromDate.Attributes.Add("readonly", "readonly");
                    this.txtToDate.Attributes.Add("readonly", "readonly");
                    this.grdIDCardList.DataSource = this.BindGrid(this.drpStatus.SelectedItem.Value);
                    this.grdIDCardList.DataBind();
                    this.grdIDCardList.PagerSettings.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Reset_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset_Click(object sender, GridViewRowEventArgs e)
        {
        }

        /// <summary>
        /// The IDCardList_RowDataBound method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdIDCardList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                foreach (GridViewRow grdRow in this.grdIDCardList.Rows)
                {
                    Label lblAssociateID = (Label)grdRow.FindControl("lblAssociateID");
                    HiddenField hdnTemplateId = (HiddenField)grdRow.FindControl("hdnFileUploadID");
                    Label lblAssociateBloodGroup = (Label)grdRow.FindControl("lblAssociateBloodGroup");
                    Label lblEmergenyContact = (Label)grdRow.FindControl("lblAssociateEmergencyContact");

                    Label lblPrintStatus = (Label)grdRow.FindControl("lblPrintStatus");
                    if ((string.Compare(
                        Convert.ToString(lblAssociateBloodGroup.Text.ToString().Trim().ToUpper()),
                               VMSConstants.VMSConstants.UNKNOWNBLOODLONG) == 0) ||
                                 (string.Compare(
                                 Convert.ToString(lblAssociateBloodGroup.Text.ToString().Trim().ToUpper()),
                               VMSConstants.VMSConstants.UNKNOWNBLOODSHORT) == 0))
                    {
                        lblAssociateBloodGroup.Text = string.Empty;
                    }

                    ////if (lblEmergenyContact.Text == null)
                    ////{
                    //    lblEmergenyContact.Text = VMSConstants.VMSConstants.NotAvaliable;
                    ////}

                    if (lblEmergenyContact.Text != null)
                    {
                        if (string.IsNullOrEmpty(lblEmergenyContact.Text.Trim()))
                        {
                            lblEmergenyContact.Text = VMSConstants.VMSConstants.NOTAVALIABLE;
                        }
                    }

                    ////if (lblAssociateBloodGroup.Text == null)
                    ////{
                    //    lblAssociateBloodGroup.Text = VMSConstants.VMSConstants.NotAvaliable;
                    ////}

                    if (lblAssociateBloodGroup.Text != null)
                    {
                        if (string.IsNullOrEmpty(lblAssociateBloodGroup.Text.Trim()))
                        {
                            lblAssociateBloodGroup.Text = VMSConstants.VMSConstants.NOTAVALIABLE;
                        }
                    }

                    System.Web.UI.WebControls.Image imgAssociatePhoto = (System.Web.UI.WebControls.Image)grdRow.FindControl("imgAssociatePhoto");
                    ////Image imgNewBig = (Image)grdRow.FindControl("imgNewBig");
                    string fileContentId = XSS.HtmlEncode(RegExValidate(hdnTemplateId.Value));
                    string associateID = lblAssociateID.Text.Trim();
                    if (this.drpStatus.SelectedItem.Value == "2")
                    {
                        LinkButton lnkPrint = (LinkButton)grdRow.FindControl("btnPrint");
                        lnkPrint.CssClass = "GridLinkButtonDisabled";
                        lnkPrint.Enabled = false;
                        LinkButton lnkbtnStatusUpdate = (LinkButton)grdRow.FindControl("btnStatusUpdate");
                        lnkbtnStatusUpdate.CssClass = "GridLinkButtonDisabled";
                        lnkbtnStatusUpdate.Enabled = false;
                    }

                    if (!string.IsNullOrEmpty(lblPrintStatus.Text))
                    {
                        if (string.Compare(Convert.ToString(lblPrintStatus.Text).Trim(), VMSConstants.VMSConstants.PRINTSTATUS) == 0)
                        {
                            LinkButton lnkDownLoad = (LinkButton)grdRow.FindControl("btnStatusUpdate");
                            lnkDownLoad.CssClass = "GridLinkButtonDisabled";
                            lnkDownLoad.Enabled = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(fileContentId))
                    {
                        GenericFileUpload gnfileUpload = new GenericFileUpload();
                        byte[] data = gnfileUpload.GetAssociateImage(fileContentId);
                        if (data != null)
                        {
                            int width = 0;
                            int height = 0;
                            System.Drawing.Image image = System.Drawing.Image.FromStream(
                                new System.IO.MemoryStream(data));
                            width = image.Width;
                            height = image.Height;
                            //// testImage.Src = string.Concat("GetImage.aspx?IDCARD=", ID, "&w=", width, "&h=", height, "&TempId=", hdnFileUploadId.Value);
                            imgAssociatePhoto.ImageUrl = string.Concat("GetImage1.ashx?IDCARD=", associateID, "&w=", width, "&h=", height, "&TempId=", fileContentId);
                        }
                        else
                        {
                            LinkButton lnkDownLoad = (LinkButton)grdRow.FindControl("btnDownLoad");
                            lnkDownLoad.CssClass = "GridLinkButtonDisabled";
                            lnkDownLoad.Enabled = false;
                            imgAssociatePhoto.ImageUrl = VMSConstants.VMSConstants.IMAGEPATH;
                        }
                    }
                    else
                    {
                        LinkButton lnkDownLoad = (LinkButton)grdRow.FindControl("btnDownLoad");
                        lnkDownLoad.CssClass = "GridLinkButtonDisabled";
                        lnkDownLoad.Enabled = false;
                        imgAssociatePhoto.ImageUrl = VMSConstants.VMSConstants.IMAGEPATH;
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Grid download and print operations
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">view e</param>
        protected void GrdRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Download"))
                {
                    Control ctrl = e.CommandSource as Control;
                    if (ctrl != null)
                    {
                        GridViewRow currenrtrow = ctrl.Parent.NamingContainer as GridViewRow;
                        ////Now you can find control on the row where event is raised by using FindControl method.
                        Label lblAssociateId = (Label)currenrtrow.FindControl("lblAssociateID");
                        string associateId = lblAssociateId.Text.Trim();
                        HiddenField hdnTemplateId = (HiddenField)currenrtrow.FindControl("hdnFileUploadID");
                        string fileContentId = RegExValidate(hdnTemplateId.Value);
                        GenericFileUpload gnfileUpload = new GenericFileUpload();
                        byte[] data = gnfileUpload.GetAssociateImage(fileContentId);
                        int width = 0;
                        int height = 0;
                        System.Drawing.Image image = System.Drawing.Image.FromStream(
                            new System.IO.MemoryStream(data));
                        width = image.Width;
                        height = image.Height;
                        this.DownloadSingleImage(data, Convert.ToString(associateId));
                        ////Save Log information
                        if (this.Session["LoginId"] != null)
                        {
                            VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                            objRequestDetailsBL.SaveLogDownLoadPhoto(associateId, Convert.ToString(this.Session["LoginId"]));
                        }
                    }
                }

                if (e.CommandName.Equals("Print"))
                {
                    Control ctrl = e.CommandSource as Control;
                    if (ctrl != null)
                    {
                        GridViewRow currenrtrow = ctrl.Parent.NamingContainer as GridViewRow;
                        ////Now you can find control on the row where event is raised by using FindControl method.
                        Label lblAssociateId = (Label)currenrtrow.FindControl("lblAssociateID");
                        string associateId = lblAssociateId.Text.Trim();
                        HiddenField hdnTemplateId = (HiddenField)currenrtrow.FindControl("hdnFileUploadID");
                        string templateId = XSS.HtmlEncode(Convert.ToString(hdnTemplateId.Value).Trim());
                        string strScript = string.Empty;
                        strScript = "<script language='javascript'>window.open('IDCard.aspx?key=";
                        string encryptedData = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(associateId.Trim())));
                        strScript = string.Concat(strScript, encryptedData);
                        string encryptTempId = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(templateId)));
                        strScript = string.Concat(strScript, " & Id=");
                        strScript = string.Concat(strScript, encryptTempId);
                        strScript = string.Concat(strScript, "', 'List', 'toolbar=no,menubar=no,scrollbars=no,resizable=no,width=207,height=800, location=center');</script>");
                        ClientScript.RegisterStartupScript(typeof(Page), "LoadGenerateIDCard", strScript);
                    }
                }

                if (e.CommandName.Equals("UpdateStatus"))
                {
                    Control ctrl = e.CommandSource as Control;
                    if (ctrl != null)
                    {
                        if (this.drpCity.SelectedIndex == 0)
                        {
                            this.lblError.Text = "Please select City";
                        }
                        else
                        {
                            string printStatus = VMSConstants.VMSConstants.PRINTSTATUS;
                            int location = Convert.ToInt32(this.drpCity.SelectedItem.Value);
                            GridViewRow currenrtrow = ctrl.Parent.NamingContainer as GridViewRow;
                            ////Now you can find control on the row where event is raised by using FindControl method.
                            Label lblAssociateId = (Label)currenrtrow.FindControl("lblAssociateID");
                            Label lblPrintStatus = (Label)currenrtrow.FindControl("lblPrintStatus");
                            LinkButton btnStatusUpdate = (LinkButton)currenrtrow.FindControl("btnStatusUpdate");
                            string associateId = Convert.ToString(lblAssociateId.Text.Trim());
                            if (this.Session["LoginId"] != null)
                            {
                                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                                objRequestDetailsBL.UpdatePrintStatus(associateId, Convert.ToString(this.Session["LoginId"]), printStatus, location);
                                ////  btnStatusUpdate.Text = VMSConstants.VMSConstants.PrintStatus;
                                this.grdIDCardList.DataSource = this.BindGrid(this.drpStatus.SelectedItem.Value);
                                this.grdIDCardList.DataBind();
                            }
                            else
                            {
                                
                                    Response.Redirect("~/SessionExpired.aspx", true);
                                                                                                  
                            }
                        }
                    }
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
        /// ID card list page index changed
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">view e</param>
        protected void GrdIDCardList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.grdIDCardList.PageIndex = e.NewPageIndex;
                this.grdIDCardList.SelectedIndex = -1;

                this.grdIDCardList.DataSource = this.BindGrid(this.drpStatus.SelectedItem.Value);
                this.grdIDCardList.DataBind();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Download associate photo
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event e</param>
        protected void ImgDownload_Click(object sender, EventArgs e)
        {
            string fileContentId = string.Empty;
            string associateId = string.Empty;
            string fileName = string.Empty;
            bool download = false;
            this.lblError.Text = string.Empty;
            //// string filePath = Server.MapPath("~/FilesServer/AssociatePhoto/");
            string filePath = string.Concat(Server.MapPath("FilesServer\\AssociatePhoto\\").ToString());
            string downloadFileName = "AssociateImages.zip";           

                using (ZipFile zip = new ZipFile())
                {
                    foreach (GridViewRow row in this.grdIDCardList.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("chkIDCard");
                        if (cb != null && cb.Checked)
                        {
                            fileContentId = (row.FindControl("hdnFileUploadID") as HiddenField).Value;
                            if (!string.IsNullOrEmpty(fileContentId))
                            {
                                associateId = (row.FindControl("lblAssociateID") as Label).Text;
                                fileName = this.DownLoadImage(fileContentId, associateId);
                                fileName = string.Concat(filePath, fileName);
                                ////AddFileToZip(downloadFileName, (Path.Combine(filePath, fileName)));                               
                                zip.AddFile(Path.Combine(filePath, fileName), string.Empty);
                                download = true;
                                if (this.Session["LoginId"] != null)
                                {
                                    VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                                    objRequestDetailsBL.SaveLogDownLoadPhoto(associateId, Convert.ToString(this.Session["LoginId"]));
                                }
                            }
                            //// RemoveFile(fileName);
                        }
                    }

                    if (download == false)
                    {
                        this.lblError.Text = "No Files downloaded !";
                    }
                    else
                    {
                        this.lblError.Text = string.Empty;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Expires = 1;
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "filename=" + downloadFileName);
                        zip.ParallelDeflateThreshold = -1;
                        zip.Save(Response.OutputStream);
                    }
                }

                foreach (GridViewRow row in this.grdIDCardList.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("chkIDCard");
                    if (cb != null && cb.Checked)
                    {
                        fileContentId = (row.FindControl("hdnFileUploadID") as HiddenField).Value;
                        if (!string.IsNullOrEmpty(fileContentId))
                        {
                            associateId = (row.FindControl("lblAssociateID") as Label).Text;
                            fileName = string.Concat(associateId.Trim(), ".jpeg");
                            fileName = string.Concat(filePath, fileName);

                            RemoveFile(fileName);
                        }
                    }
                }

                if (download == true)
                {
                    Response.End();
                }
        }

        /// <summary>
        /// The DownLoadImage method
        /// </summary>
        /// <param name="fileContentId">The fileContentId parameter</param>
        /// <param name="associateId">The associateId parameter</param>
        /// <returns>The string type object</returns>        
        protected string DownLoadImage(string fileContentId, string associateId)
        {
            try
            {
                GenericFileUpload gnfileUpload = new GenericFileUpload();
                byte[] data = gnfileUpload.GetAssociateImage(fileContentId);
                int width = 0;
                int height = 0;
                System.Drawing.Image image = System.Drawing.Image.FromStream(
                    new System.IO.MemoryStream(data));
                width = image.Width;
                height = image.Height;
                return this.DownloadImage(data, width, height, associateId);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// Sort DataGrid
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">view e</param>
        protected void GrdIDCardList_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (this.Dir == SortDirection.Ascending)
            {
                this.Dir = SortDirection.Descending;
                sortingDirection = "Desc";
            }
            else
            {
                this.Dir = SortDirection.Ascending;
                sortingDirection = "Asc";
            }

            DataView sortedView = new DataView(this.BindGrid(this.drpStatus.SelectedItem.Value));
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            this.grdIDCardList.DataSource = sortedView;
            this.grdIDCardList.DataBind();
        }

        /// <summary>
        /// To Search records
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string[] fromdateArray = this.txtFromDate.Text.Split('/');
                string fromDate = fromdateArray[2] + '-' + fromdateArray[1] + '-' + fromdateArray[0];
                string[] todateArray = this.txtToDate.Text.Split('/');
                string todate = todateArray[2] + '-' + todateArray[1] + '-' + todateArray[0];

                DateTime startDate = Convert.ToDateTime(fromDate);
                DateTime endDate = Convert.ToDateTime(todate);
                if (string.IsNullOrEmpty(this.drpCity.SelectedItem.Value))
                {
                    this.lblError.Text = "Please select City";
                    this.lblError.ForeColor = Color.Red;
                }          
                else if (DateTime.Compare(startDate, endDate) > 0)
                {
                    this.lblError.Text = "Please select valid Date";
                    this.lblError.ForeColor = Color.Red;
                }
                else
                {
                    this.lblError.Text = string.Empty;
                    this.grdIDCardList.PageIndex = 0;
                    this.grdIDCardList.DataSource = this.BindGrid(this.drpStatus.SelectedItem.Value);
                    this.grdIDCardList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

          /// <summary>
        /// To Export selected details to the excel
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void BtnExport_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.ExportExcel();
                //// Response.End();
            }
            catch
            {
            }
        }

        /// <summary>
        /// To enable printing
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event e</param>
        protected void ImgPrint_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder associatesId = new StringBuilder();
            foreach (GridViewRow row in this.grdIDCardList.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkIDCard");
                if (chk.Checked == true)
                {
                    int i = row.RowIndex;
                    Label lblAssociateId = (Label)this.grdIDCardList.Rows[i].FindControl("lblAssociateID");
                    associatesId.Append(string.Concat(lblAssociateId.Text.Trim(), ","));
                }
            }

            string strScript = string.Empty;
            ////    string templateId = "25678";
            strScript = "<script language='javascript'>window.open('IDCardBulkPrint.aspx?key=";
            strScript = string.Concat(strScript, associatesId);
            strScript = string.Concat(strScript, "', 'List', 'toolbar=no,menubar=no,scrollbars=no,resizable=no,width=207,height=800, location=center');</script>");
            ClientScript.RegisterStartupScript(typeof(Page), "LoadBulkIDCard", strScript);
        }

        /// <summary>
        /// The Reset_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnReset_Click(object sender, EventArgs e)
        {
            this.InitIDCardLocations();
            this.txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.txtFromDate.Attributes.Add("readonly", "readonly");
            this.txtToDate.Attributes.Add("readonly", "readonly");
            this.drpStatus.SelectedIndex = 0;
            this.lblError.Text = string.Empty;
            this.grdIDCardList.DataSource = this.BindGrid(this.drpStatus.SelectedItem.Value);
            this.grdIDCardList.DataBind(); 
        }

        /// <summary>
        /// Check whether file is exist or not
        /// </summary>
        /// <param name="path">string path</param>
        /// <returns>file exits</returns>
        private static bool CheckFileExist(string path)
        {
            try
            {
                FileInfo imageFile = new FileInfo(path);
                bool fileExists = imageFile.Exists;
                return fileExists;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return false;
            }
        }

        /// <summary>
        /// To remove file from server path.
        /// </summary>
        /// <param name="path">string path</param>
        private static void RemoveFile(string path)
        {
            FileStream fs = null;
            try
            {
                if (CheckFileExist(path))
                {
                    fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Delete);
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// The ResetButton method
        /// </summary>        
        private void ResetButton()
        {
            this.grdIDCardList.PagerSettings.Visible = false;
            this.grdIDCardList.DataSource = null;
            this.grdIDCardList.DataBind();
        }

        /// <summary>
        /// The BindGrid method
        /// </summary>
        /// <param name="status">The status parameter</param>
        /// <returns>The System.Data.DataTable type object</returns>        
        private DataTable BindGrid(string status)
        {
            try
            {
                string selectedCity = Convert.ToString(this.drpCity.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(this.drpCity.SelectedItem.Value))
                {
                    VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL objRequestDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                    string[] fromdateArray = this.txtFromDate.Text.Split('/');
                    string fromDate = fromdateArray[2] + '-' + fromdateArray[1] + '-' + fromdateArray[0];
                    string[] todateArray = this.txtToDate.Text.Split('/');
                    string todate = todateArray[2] + '-' + todateArray[1] + '-' + todateArray[0];
                    DataTable dtdisplay = objRequestDetailsBL.GetIDCardAssociateDetails(Convert.ToDateTime(fromDate), Convert.ToDateTime(todate), selectedCity, status);
                    if (dtdisplay.Rows.Count > 0)
                    {
                        this.imgDownload.Visible = true;
                        this.imgExport.Visible = true;
                        if (status == "2")
                        {
                            this.imgPrint.Visible = false;
                        }
                        else
                        {
                            this.imgPrint.Visible = true;
                        }

                        this.lblError.Text = string.Empty;
                    }
                    else
                    {
                        this.imgDownload.Visible = false;
                        this.imgExport.Visible = false;
                        this.imgPrint.Visible = false;
                        this.lblError.Text = "No Records Found !";
                    }

                    return dtdisplay;
                }
                else
                {
                    this.imgDownload.Visible = false;
                    this.imgExport.Visible = false;
                    this.imgPrint.Visible = false;

                    return null;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// Download associate image and save to server path for zip operation
        /// </summary>
        /// <param name="data">data byte</param>
        /// <param name="width">width length</param>
        /// <param name="height">height length</param>
        /// <param name="associateId">associate id</param>
        /// <returns>file name</returns>
        private string DownloadImage(byte[] data, int width, int height, string associateId)
        {
            try
            {
                if (data != null)
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        System.Drawing.Image newImage;
                        ms.Write(data, 0, data.Length);
                        newImage = System.Drawing.Image.FromStream(ms, true);
                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        newImage.Save(outStream, ImageFormat.Jpeg);
                        byte[] buffer = outStream.ToArray();
                        string strFileName = string.Concat(associateId.Trim(), ".jpeg");
                        string strFile = string.Concat(Server.MapPath("FilesServer\\AssociatePhoto\\").ToString(), strFileName);
                        File.WriteAllBytes(strFile, buffer);
                        return strFileName;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                return null;
            }
        }

        /// <summary>
        /// The downloadSingleImage method
        /// </summary>
        /// <param name="data">The data parameter</param>
        /// <param name="associateId">The associateId parameter</param>        
        private void DownloadSingleImage(byte[] data, string associateId)
        {
            try
            {
                if (data != null)
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        string filename = string.Empty;
                        if (!string.IsNullOrEmpty(associateId))
                        {
                            filename = associateId + ".jpeg";
                        }
                        else
                        {
                            filename = "download.jpeg";
                        }

                        System.Drawing.Image newImage;
                        ms.Write(data, 0, data.Length);
                        newImage = System.Drawing.Image.FromStream(ms, true);
                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        newImage.Save(outStream, ImageFormat.Jpeg);
                        byte[] buffer = outStream.ToArray();
                        Response.ContentType = "image/jpeg";
                        Response.BinaryWrite((byte[])buffer);
                        Response.AppendHeader("Content-Disposition:", "attachment; filename=" + filename + string.Empty);
                        Response.ContentType = "application/download";
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The FindCheckedRows method
        /// </summary>        
        private void FindCheckedRows()
        {
            ArrayList checkedRowsList;
            if (this.ViewState["checkedRowsList"] != null)
            {
                checkedRowsList = (ArrayList)ViewState["checkedRowsList"];
            }
            else
            {
                checkedRowsList = new ArrayList();
            }

            foreach (GridViewRow gvrow in this.grdIDCardList.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    string rowIndex = Convert.ToString(this.grdIDCardList.DataKeys[gvrow.RowIndex]["AssociateId"].ToString().Trim());
                    ////int rowIndex = Convert.ToInt32(gvRow.RowIndex) +  Convert.ToInt32(grdIDCardList.PageIndex);
                    CheckBox chkSelect = (CheckBox)gvrow.FindControl("chkIDCard");
                    if (chkSelect.Checked && !checkedRowsList.Contains(rowIndex))
                    {
                        checkedRowsList.Add(rowIndex);
                    }
                    else if (!chkSelect.Checked && checkedRowsList.Contains(rowIndex))
                    {
                        checkedRowsList.Remove(rowIndex);
                    }
                }
            }

            this.ViewState["checkedRowsList"] = checkedRowsList;
        }

              /// <summary>
        /// Export to excel
        /// </summary>
        private void ExportExcel()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("AssociateId");
            dt.Columns.Add("EmergencyContact");
            dt.Columns.Add("BloodGroup");
            foreach (GridViewRow row in this.grdIDCardList.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkIDCard");
                if (chk.Checked == true)
                {
                    int i = row.RowIndex;
                    Label lblAssociateId = (Label)this.grdIDCardList.Rows[i].FindControl("lblAssociateID");
                    Label lblAssociateDisplayName = (Label)this.grdIDCardList.Rows[i].FindControl("lblAssociateDisplayName");
                    Label lblAssociateBloodGroup = (Label)this.grdIDCardList.Rows[i].FindControl("lblAssociateBloodGroup");
                    Label lblAssociateEmergencyContact = (Label)this.grdIDCardList.Rows[i].FindControl("lblAssociateEmergencyContact");
                    DataRow dr = dt.NewRow();
                    dr["Name"] = Convert.ToString(lblAssociateDisplayName.Text);
                    dr["AssociateId"] = Convert.ToString(lblAssociateId.Text);
                    dr["BloodGroup"] = Convert.ToString(lblAssociateBloodGroup.Text);
                    dr["EmergencyContact"] = Convert.ToString(lblAssociateEmergencyContact.Text);
                    dt.Rows.Add(dr);
                }
            }

            GridView gridView1 = new GridView();          
            gridView1.DataSource = dt;
            gridView1.ShowHeader = false;
            gridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.xls", "AssociateList"));
            Response.Charset = string.Empty;
            System.IO.StringWriter stringwriter = new StringWriter();
            HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
            gridView1.RenderControl(htmlwriter);
            Response.Write(stringwriter.ToString());
            stringwriter.Close();
            htmlwriter.Close();
            Response.End();
        }
    }
}
