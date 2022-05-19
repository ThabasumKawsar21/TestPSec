
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Data.Sql;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// class for id card bulk print
    /// </summary> 
    public partial class IDCardBulkPrint : System.Web.UI.Page
    {
#pragma warning disable CS0414 // The field 'IDCardBulkPrint.incr' is assigned but its value is never used
        /// <summary>
        /// The  field
        /// </summary>        
        private static int incr = 0;
#pragma warning restore CS0414 // The field 'IDCardBulkPrint.incr' is assigned but its value is never used
        ////  List<String> AssociatesId = new List<String>(); 
        
        /// <summary>
        /// The BulkId field
        /// </summary>        
        private AssociateBulkIDList objBulkId = new AssociateBulkIDList();
        
        /// <summary>
        /// The BuildPagers method
        /// </summary>        
        public void BuildPagers()
        {
            if (Convert.ToInt32(this.TotalSize.Value) >= Convert.ToInt32(this.PageSize.Value))
            {
                ////Check if its possible to have the previous page
                if ((int.Parse(this.CurrentPage.Value) - 1) <= 0)
                {
                    this.imgPrev.Visible = false;
                    ////   lbtnPrev.Visible = false;
                }
                else
                {
                    ////  lbtnPrev.Visible = true;
                    this.imgPrev.Visible = true;
                }

                if ((int.Parse(this.CurrentPage.Value) * int.Parse(this.PageSize.Value)) >= int.Parse(this.TotalSize.Value))
                {
                    this.imgNext.Visible = false;
                    ////   lbtnNext.Visible = false;
                }
                else
                {
                    ////    lbtnNext.Visible = true;
                    this.imgNext.Visible = true;
                }
            }
            else
            {
                this.imgPrev.Visible = false;
                this.imgNext.Visible = false;
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
                if (!this.IsPostBack)
                {
                    NameValueCollection coll = Request.QueryString;
                    //// Get names of all keys into a string array.
                    string[] arr1 = coll.AllKeys;
                    string key = string.Empty;
                    string tempId = string.Empty;
                    string templateId = string.Empty;

                    if (arr1.Length == 1)
                    {
                        key = Convert.ToString(coll.GetValues(0)[0]);
                        string[] arrAssociate = key.Split(',');

                        this.objBulkId.RecordCount = arrAssociate.Length - 1;
                        this.TotalSize.Value = Convert.ToString(this.objBulkId.RecordCount);
                        for (int i = 0; i < arrAssociate.Length - 1; i++)
                        {
                            this.objBulkId.AssociatesId.Add(arrAssociate[i]);
                        }

                        this.Session["BulkAssociateList"] = this.objBulkId;
                        this.PrintIDCardInfo(this.objBulkId.AssociatesId[0]);
                    }
                    //// SetPaging();
                    this.BuildPagers();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The Print1_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgPrint1_Click(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument pt = new System.Drawing.Printing.PrintDocument();
        }

        /// <summary>
        /// Redirect to parent page.
        /// </summary>
        /// <param name="sender">parameter sender</param>
        /// <param name="e">event e</param>
        protected void BtnHidden_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The Next_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgNext_Click(object sender, ImageClickEventArgs e)
        {
            if (this.Session["BulkAssociateList"] != null)
            {
                ////Check if we can display the next page.
                if ((int.Parse(this.CurrentPage.Value) * int.Parse(this.PageSize.Value)) < int.Parse(this.TotalSize.Value))
                {
                    ////Increment the CurrentPage value
                    this.CurrentPage.Value = (int.Parse(this.CurrentPage.Value) + 1).ToString();
                }

                this.BuildPagers();

                this.objBulkId = (AssociateBulkIDList)Session["BulkAssociateList"];
                ////string[] arrAssociate = hdnAssociateId.Value.Split(',');
                this.PrintIDCardInfo(this.objBulkId.AssociatesId[Convert.ToInt32(this.CurrentPage.Value) - 1]);
            }
        }

        /// <summary>
        /// The Preview_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ImgPrev_Click(object sender, ImageClickEventArgs e)
        {
            ////Check if we are on any page greater than 0 
            if ((int.Parse(this.CurrentPage.Value) - 1) >= 0)
            {
                ////Decrease the CurrentPage Value
                this.CurrentPage.Value = (int.Parse(this.CurrentPage.Value) - 1).ToString();
            }

            this.BuildPagers();

            this.objBulkId = (AssociateBulkIDList)Session["BulkAssociateList"];
            ////string[] arrAssociate = hdnAssociateId.Value.Split(',');
            this.PrintIDCardInfo(this.objBulkId.AssociatesId[Convert.ToInt32(this.CurrentPage.Value) - 1]);
        }

        /// <summary>
        /// The PrintIDCardInfo method
        /// </summary>
        /// <param name="associateId">The associateId parameter</param>        
        private void PrintIDCardInfo(string associateId)
        {
            if (this.Session["LoginId"] != null)
            {
                VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL associateDetails;
                VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL objUserDetailsBL;
                string strLoginID = Convert.ToString(Session["LoginId"]);
                ////#DPNAME
                string strDisplayName = string.Empty;
                string fileContentId;
                associateDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
                objUserDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();

                DataTable dtdisplay = objUserDetailsBL.InsertIDCardDetails(strLoginID, associateId);
                DataTable dt = associateDetails.PreviewDetails(associateId);

                if (!string.IsNullOrEmpty(associateId))
                {
                    if (dtdisplay.Rows.Count > 0)
                    {
                        ////string pattern = "[^0-9]";
                        string strYear = DateTime.Now.Year.ToString();
                        string area = dtdisplay.Rows[0]["Area"].ToString().Trim();
                        string city = dtdisplay.Rows[0]["Location"].ToString().Trim();
                        string pincode = dtdisplay.Rows[0]["PinCode"].ToString().Trim();
                        string state = dtdisplay.Rows[0]["State"].ToString().Trim();
                        string country = dtdisplay.Rows[0]["Country"].ToString().Trim();
                        fileContentId = dt.Rows[0]["FileUploadID"].ToString();

                        GenericFileUpload gnfileupload = new GenericFileUpload();
                        byte[] data = gnfileupload.GetAssociateImage(fileContentId);
                        int width = 0;
                        int height = 0;
                        System.Drawing.Image image = System.Drawing.Image.FromStream(
                            new System.IO.MemoryStream(data));
                        width = image.Width;
                        height = image.Height;
                        this.ImgAssociate.ImageUrl = XSS.HtmlEncode(string.Concat("GetImage1.ashx?IDCARD=", associateId, "&w=", width, "&h=", height, "&FileId=", fileContentId));
                        strDisplayName = dt.Rows[0]["DisplayName"].ToString().ToUpper();

                        this.lblFrontID.InnerHtml = XSS.HtmlEncode(dt.Rows[0]["AssociateID"].ToString().Trim());
                        this.lblRearID.InnerHtml = XSS.HtmlEncode(dt.Rows[0]["AssociateID"].ToString().Trim());
                        this.hdnAssociateId.Value = dt.Rows[0]["AssociateID"].ToString().Trim();
                        this.Session["hdnAssociateId"] = this.hdnAssociateId.Value;
                        this.lblyear.InnerHtml = strYear.Substring(2, 2);
                        this.imgBarCode.ImageUrl = string.Concat("~/Barcode.aspx?Code=*", associateId.Trim(), "*");
                        this.lblBarcodeAssociateId.InnerHtml = XSS.HtmlEncode(dt.Rows[0]["AssociateID"].ToString().Trim());
                        if (!string.IsNullOrEmpty(strDisplayName))
                        {
                            if (strDisplayName.Length > 18)
                            {
                                this.lblName.Attributes.Add("style", "font-size:13pt");
                            }

                            this.lblName.InnerHtml = XSS.HtmlEncode(strDisplayName.ToUpper());
                        }
                        else
                        {
                            this.lblName.InnerHtml = XSS.HtmlEncode(dt.Rows[0]["FirstName"].ToString().Trim().ToUpper() + " " + dt.Rows[0]["LastName"].ToString().Trim().ToUpper());
                        }
 
                        city = "Chennai";
                        this.imgIDCardTop.Src = "~/Images/" + city + "/CardTop.jpg";
                        this.imgIdCardBottom.Src = "~/Images/" + city + "/IsAssociate.jpg";
                        this.imgFrontLine.Src = "~/Images/" + city + "/Line.jpg";
                        this.Session["ImageURL"] = null;
                        this.Session["DisplayName"] = null;
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'> alert('Data Not Found');</script>");
                }
            }
        }

        /// <summary>
        /// Check whether File is already exist
        /// </summary>
        /// <param name="path">parameter path</param>
        /// <returns>file exists</returns>
        private bool FileExist(string path)
        {
            try
            {
                FileInfo imageFile = new FileInfo(path);
                bool fileExists = imageFile.Exists;
                return fileExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Delete file
        /// </summary>
        /// <param name="path">parameter path</param>
        private void DeleteFile(string path)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Delete);
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// The AssociateBulkIDList class
        /// </summary>    
        public class AssociateBulkIDList
        {
            /// <summary>
            /// The count field
            /// </summary>        
            private int count;

            /// <summary>
            /// The associatesId field
            /// </summary>        
            private List<string> associatesId = new List<string>();

            /// <summary>
            /// Gets or sets the RecordCount property
            /// </summary>        
            public int RecordCount
            {
                get { return this.count; }
                set { this.count = value; }
            }

            /// <summary>
            /// Gets or sets the AssociatesId property
            /// </summary>        
            public List<string> AssociatesId
            {
                get
                {
                    return this.associatesId;
                }

                set
                {
                    this.associatesId = value;
                }
            }
        }
    }
}
