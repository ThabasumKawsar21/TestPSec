
namespace VMSDev
{
    using System;
    using System.Collections;
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
    using VMSBusinessLayer;

    /// <summary>
    /// home partial class
    /// </summary>
    public partial class Home : System.Web.UI.Page
    {
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ////string strUserId = VMSUtility.VMSUtility.GetUserId();
                ////string Role 
                ////= new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL().GetUserRole(strUserId).ToString();
                ////if (Role == "Host")
                ////{
                //    Response.Redirect(VMSConstants.VMSConstants.HOSTREDIRECTIONPAGE);
                ////}
                ////else
                ////{
                //    Response.Redirect(VMSConstants.VMSConstants.HOMEPAGE);
                ////}
            }      
        }
        }
}
