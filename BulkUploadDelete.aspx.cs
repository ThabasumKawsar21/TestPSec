

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// bulk upload delete partial class
    /// </summary>
    public partial class BulkUploadDelete : System.Web.UI.Page
    {
        /// <summary>
        /// page upload function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// continue click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnContinue_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            try
            {
                if (this.Session["DataForPreviewLoad"] != null)
                {
                    dt = (DataTable)Session["DataForPreviewLoad"];
                    dt1 = dt.Copy();
                    dt1.Clear();
                    if (Convert.ToInt32(dt.Rows.Count) > 0)
                    {
                        dt = (DataTable)Session["DataForPreviewLoad"];        
                    }
                    else
                    {
                        if (this.Session["DataSourceFinal"] != null)
                        {
                            dt = (DataTable)Session["DataSourceFinal"];
                        }
                    }
                }

                int row = Convert.ToInt32(Session["RowNumber"]);
                ////dt1= dt.Rows[row].Table;   

                DataTable dt4 = new DataTable();
                DataRow delRow;
                dt4 = dt.Clone();
                delRow = (DataRow)dt.Rows[row];
                dt4.ImportRow(delRow);
                if (this.Session["View"] != null)
                {
                    if (Convert.ToString(this.Session["View"]) == "1")
                    {
                        this.Session["ViewCandidatesEdit"] = dt4.Copy();
                        this.Session["View"] = null;
                    }
                }

                dt.Rows.RemoveAt(row);
                this.Session["DataSource1"] = dt;
                this.Session["validatedinitial"] = dt;
                this.Session["Delete"] = "1";
                this.Session["ViewCandidates"] = null;

                ScriptManager.RegisterStartupScript(this, typeof(string), "CancelUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
            }
            catch (Exception ex)
            {
                ////ExceptionLogger.OneC_ExceptionLogger(ex, this.Page);
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Cancel click button
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void LbtnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "CloseUpload", "<script type='text/javascript' language='javascript'>parent.location.href = parent.location.href;</script>", false);
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
    }
}
