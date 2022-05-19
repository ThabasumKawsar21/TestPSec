
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
    /// partial class incomplete verification
    /// </summary>
    public partial class Incompleteverification : System.Web.UI.Page
    {
        /// <summary>
        /// The facility field
        /// </summary>        
        private string associateid, userid, facility;
        
        /// <summary>
        /// The Object Employee Details field
        /// </summary>        
        private EmployeeBL objEmployeeDetails1 = new EmployeeBL();

        /// <summary>
        /// The object send mail field
        /// </summary>        
        private VMSBusinessLayer.RequestDetailsBL objsendmail 
            = new VMSBusinessLayer.RequestDetailsBL();
        
        /// <summary>
        /// The mail details field
        /// </summary>        
        private DataSet maildetails = new DataSet();
        
        /// <summary>
        /// The popup details field
        /// </summary>        
        private DataSet popupdetails = new DataSet();

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.associateid = Request.QueryString["associateid"];
            this.userid = Request.QueryString["userid"];
            this.facility = Request.QueryString["Locationid"];
            this.popupdetails = this.objEmployeeDetails1.Incompleteverificationupdation(
                this.associateid, 
                this.userid, 
                this.facility, 
                "yes");
            this.lblwarningmessage.Text 
                = "Your Laptop verification is incomplete. Kindly ensure your Check Out from " 
                + this.popupdetails.Tables[0].Rows[0]["VerifiedLocation"].ToString() 
                + ". (Check In Date: " 
                + this.popupdetails.Tables[0].Rows[0]["CheckInTime"].ToString() 
                + " Location: " 
                + this.popupdetails.Tables[0].Rows[0]["VerifiedLocation"].ToString() 
                + " )";
        }

        /// <summary>
        /// The button Save Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            this.maildetails 
                = this.objEmployeeDetails1.Incompleteverificationupdation(
                this.associateid, 
                this.userid, 
                this.facility, 
                "No");
            this.objsendmail.LVSincomplete_verificationmail(this.maildetails);

                        string strScript1 = "<script language='javascript'>  window.close(); </script>";
#pragma warning disable CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
                        Page.RegisterClientScriptBlock(
                            "LoadSearchPage", 
                            strScript1);
#pragma warning restore CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
        }

        /// <summary>
        /// The button Clear Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.Session["Continue"] = "No";
            string strScript1 = "<script language='javascript'>  window.close(); </script>";
#pragma warning disable CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
            Page.RegisterClientScriptBlock("LoadSearchPage", strScript1);
#pragma warning restore CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
        }
    }
}
