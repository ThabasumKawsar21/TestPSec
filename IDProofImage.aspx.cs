

namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
 
    /// <summary>
    /// partial class IDProofImage
    /// </summary>
    public partial class IDProofImage : System.Web.UI.Page
    {
        /// <summary>
        /// Page load function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////Vms_Host_upload PHYS19042010CR02

                Response.Expires = 0;
                Response.Cache.SetNoStore();
                Response.AppendHeader("Pragma", "no-cache");

                ////end PHYS19042010CR02
                if (this.Session["ProofImgByte"] == null)
                {
                    this.imgIDProof.ImageUrl = "~/Images/DummyPhoto.png";
                }
                else
                {
                    this.imgIDProof.ImageUrl = "EmployeeImage.aspx?strImage=Proof";
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
