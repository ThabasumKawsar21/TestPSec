
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Partial class Session Expired
    /// </summary>
    public partial class SessionExpired : System.Web.UI.Page
    {
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Loginurl.HRef = System.Configuration.ConfigurationManager.AppSettings["HomeURL"].ToString();
                Session.Abandon();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// The Page_PreRender method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_PreRender(object sender, EventArgs e)
        {
           // IHttpContext httpContext = this.httpContextLocatorService.GetCurrentContext();
        }
    }
}
