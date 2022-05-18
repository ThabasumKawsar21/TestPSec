

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.Sql;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using VMSBusinessLayer;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Partial classAssociateID Upload
    /// </summary>
    public partial class AssociateIDUpload : System.Web.UI.Page
    {
        /// <summary>
        /// Initial Page Load method
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args object</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;
            string loginID = string.Empty;
            try
            {
                try
                {
                    loginID = Session["LoginID"].ToString().Trim();
                }
                catch (NullReferenceException)
                {
                    ////Response.Redirect("~/Login.aspx"); 
                    try
                    {
                        Response.Redirect("~/SessionExpired.aspx", true);
                    }
                  catch (System.Threading.ThreadAbortException ex)
                    {

                    }

                }             
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        
        /// <summary>
        /// Clear Excel Button click method
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args object</param>
        protected void BtnClearExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string excelSharePath = System.Configuration.ConfigurationManager.AppSettings["ExcelFilesSharePath"].ToString();
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(excelSharePath));
                FileInfo[] fi = di.GetFiles();
                foreach (FileInfo fileTemp in fi)
                {
                    File.Delete(fileTemp.FullName);
                }

                this.DataGrid2.DataSource = null;
                this.DataGrid2.DataBind();
                this.DataGrid2.Visible = false;
            }
            catch (NullReferenceException)
            {
                this.DataGrid2.DataSource = null;
                this.DataGrid2.DataBind();
                this.DataGrid2.Visible = false;
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Upload Excel Button click method
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args object</param>
        protected void BtnUploadExcel_Click(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;
            try
            {
                EmployeeBL objEmployeeBL = new EmployeeBL();
                string strfilename = this.fupExcel.FileName;
                string strFileExtension = System.IO.Path.GetExtension(this.fupExcel.PostedFile.FileName);
                if (strFileExtension.Equals(VMSConstants.VMSConstants.EXCELFORMAT) || strFileExtension.Equals(VMSConstants.VMSConstants.EXCELFORMAT1) || strFileExtension.Equals(VMSConstants.VMSConstants.EXCELFORMAT2))
                {
                    string uploadedfile = Server.MapPath("/ExcelFiles/" + this.fupExcel.FileName).ToString();
                    this.fupExcel.PostedFile.SaveAs(uploadedfile);
                     ////ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('File upload success');</script>");
                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploadedfile + ";Extended Properties= Excel 12.0 Xml";
                    OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                    DataSet myDataSet = new DataSet();
                    DataSet resultSet = new DataSet();
                    myCommand.Fill(myDataSet, "ExcelInfo");
                    if (myDataSet.Tables["ExcelInfo"].Columns.Contains("ApplicantID") && myDataSet.Tables["ExcelInfo"].Columns.Contains("AssociateID") && myDataSet.Tables["ExcelInfo"].Columns.Contains("AssociateName") && myDataSet.Tables["ExcelInfo"].Columns.Contains("BloodGroup") && myDataSet.Tables["ExcelInfo"].Columns.Contains("EmergencyContact"))
                    {
                        resultSet = objEmployeeBL.UploadAssociateIDs(myDataSet.Tables["ExcelInfo"]);
                        string rowCount = resultSet.Tables[0].Rows[0]["Count"].ToString();
                        this.lblMessage.Text = rowCount + " : Records updated successfully";
                        this.lblMessage.Style.Add("color", "Green");
                        this.lblMessage.Visible = true;
                        //// ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('" + rowCount + " : rows updated successfully');</script>");
                        this.DataGrid2.Visible = true;
                        this.DataGrid2.DataSource = resultSet.Tables[1];
                        this.DataGrid2.DataBind();
                    }
                    else
                    {
                        this.lblMessage.Text = "File does not contain specific data. Kindly refer the template";
                        this.lblMessage.Style.Add("color", "Red");
                        this.lblMessage.Visible = true;
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "<script language='javascript'>alert('File format not supported. Kindly upload files in .xls, .xlsx and .xlsm formats');</script>");
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }    
    }
}
