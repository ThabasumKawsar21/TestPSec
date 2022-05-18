
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    
    /// <summary>
    /// partial class contractor search
    /// </summary>
    public partial class ContractorSearch : System.Web.UI.Page
    {
        /// <summary>
        /// Gets token Id value
        /// </summary>
        protected string TokenIDVal
        {
            get
            {
                return Convert.ToString(System.Web.HttpContext.Current.Session["TokenID"]);
            }
        }

        /// <summary>
        /// test method
        /// </summary>
        [WebMethod]
        [ScriptMethod]
        public static void Test()
        {
            ////string test;
        }

        /// <summary>
        /// page load function
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(this.Session["TokenID"])))
                {
                    this.Session["TokenID"] = Guid.NewGuid().ToString();
                }
            }
        }
    }
}
