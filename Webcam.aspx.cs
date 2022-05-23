
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Partial class webcam
    /// </summary>
    public partial class Webcam : System.Web.UI.Page
    {
        /// <summary>
        /// The Get Webcam image method
        /// </summary>    
        public void GetWebCamImage()
        {
            byte[] imgvalue = System.Convert.FromBase64String(this.hdnfldImage.Value);
            this.Session["Webcamimage"] = imgvalue;
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {            
        }
        
        /// <summary>
        /// The Upload_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Upload_Click(object sender, EventArgs e)
        {
            this.GetWebCamImage();
            ClientScript.RegisterClientScriptBlock(
                    this.GetType(),
                  "script",
                  "<script language='javascript'>window.opener.document.forms[0].submit();window.returnValue = true;window.close()</script>");
        }
    }
}
