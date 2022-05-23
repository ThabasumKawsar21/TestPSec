
namespace VMSDev
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// search partial class
    /// </summary>
    public partial class SearchPage : System.Web.UI.Page
    {
        /// <summary>
        /// The Grid data field
        /// </summary>        
        private DataSet griddata = new DataSet();
        
        /// <summary>
        /// The RequestDetails field
        /// </summary>        
        private VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.VMSBusinessLayer.RequestDetailsBL();
        
        /// <summary>
        /// The MobNo field
        /// </summary>        
        private string firstName, LastName, Company, MobNo;
        ////End Changes for VMS CR17 07Mar2011 Vimal

        // Begin changes made for search
        
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.firstName = XSS.HtmlEncode(Request.QueryString["FirstName"]);
                this.LastName = XSS.HtmlEncode(Request.QueryString["LastName"]);
                this.Company = XSS.HtmlEncode(Request.QueryString["Company"]);
                this.MobNo = XSS.HtmlEncode(Request.QueryString["MobNo"]);
                string srch = string.Empty;
                if (string.IsNullOrEmpty(srch))
                {
                    this.txtSearch.Text += (this.firstName + " " + this.LastName + " " + this.Company + " " + this.MobNo).Trim();
                }
               //// SearchMasterData(srch, FirstName, LastName, Company, MobNo);
this.SearchVisitor();

this.errortbl.Visible = false;              
            }
        }
        
        //// End changes made for search

        /// <summary>
        /// The grid Search Result Row Command method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void GrdSearchResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string str = e.CommandArgument.ToString();
                string str2 = "VMSEnterInformationBySecurity.aspx?VisitorID=" + str;
                ////string strScript = "<script language='javascript'> window.opener.document.location.href='VMSEnterInformationBySecurity.aspx?VisitorID=" + str + "';window.close();</script>";
                ////string strScript = "<script language='javascript'>window.returnvalue='"+str+"' ; window.close();</script>";
                string strScript = "<script language='javascript'>window.close();</script>";
                this.Session["SearchVisitorID"] = str.ToString();
                ////                string strScript = "<script language='javascript'> window.returnValue='" + str + "';window.close();</script>";
#pragma warning disable CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
                Page.RegisterClientScriptBlock("LoadSearchPage", strScript);
#pragma warning restore CS0618 // 'Page.RegisterClientScriptBlock(string, string)' is obsolete: 'The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202'
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
      
        /// <summary>
        /// The button Search Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.SearchVisitor();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The SearchMasterData method
        /// </summary>
        /// <param name="srch">The search parameter</param>
        /// <param name="strfirstName">The FirstName parameter</param>
        /// <param name="lastName">The LastName parameter</param>
        /// <param name="company">The Company parameter</param>
        /// <param name="mobNo">The MobNo parameter</param>        
        private void SearchMasterData(string srch, string strfirstName, string lastName, string company, string mobNo)
        {
            try
            {
                string searchText = this.txtSearch.Text;
                this.griddata = this.requestDetails.SearchVisitorMasterDetails(searchText, strfirstName, this.LastName, this.MobNo, this.Company);

                if (this.griddata.Tables[0].Rows.Count > 0)
                {
                    this.errortbl.Visible = false;
                    this.grdSearchResult.Visible = true;
                    this.grdSearchResult.DataSource = this.griddata;
                    this.grdSearchResult.DataBind();
                }
                else
                {
                    this.errortbl.Visible = true;
                    this.lblMessage.Text = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The SearchVisitor method
        /// </summary>        
        private void SearchVisitor()
        {
            this.hdnHostID.Value = null;
            this.hdnSearchText.Value = XSS.HtmlEncode(this.txtSearch.Text.Replace("'", "''"));
            this.grdSearchResult.DataBind();
        }
    }
}
