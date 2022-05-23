
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Partial class Bulk upload file
    /// </summary>
    public partial class UploadBulkFile : System.Web.UI.Page
    {
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                string data;
                string filename = Request.QueryString["InputFile"].ToString();
                GenericFileUpload fileUpload = new GenericFileUpload();
                using (StreamReader strm = new StreamReader(Request.InputStream))
                {
                    data = strm.ReadToEnd();
                }

                byte[] bytes = Convert.FromBase64String(data);
                string str = fileUpload.BulkUpload(bytes, filename, Session["LoginID"].ToString()); 
                this.Session["BulkUploadID"] = str;
                Response.Write(str);
            }
        }
    }
}
