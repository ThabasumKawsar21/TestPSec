using VMSDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proxy
{
    public partial class Proxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                txtProxyID.Text = string.Empty;
            }
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string TargetPage = "Default.aspx?proxyuser=" + txtProxyID.Text.Trim();
                // Response.Redirect(TargetPage, true);
                Response.Redirect(TargetPage, false);
                Context.ApplicationInstance.CompleteRequest();

            }
            //catch (System.Threading.ThreadAbortException)
            //{

            //}
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
