
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
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Commented by 173710
    /// A popup page to get the user id information 
    /// </summary>
    public partial class SearchUserDetails : System.Web.UI.Page
    {
        /// <summary>
        /// The name field
        /// </summary>        
        private static string name = string.Empty;

        /// <summary>
        /// Commented by 173710
        /// </summary>
        /// <param name="userId"> user  id</param>
        /// <param name="userName">user name</param>
        /// <returns>data table</returns>
        public DataTable GetUserDetailsByIdName(string userId, string userName)
        {
            VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
            return userDetailsBL.GetUserDetailsByIdName(userId, userName);
        }

        /// <summary>
        /// Commented by 173710
        /// </summary>
        /// <param name="userId"> user  id</param>
        /// <param name="userName">user name</param>
        /// <returns>data table</returns>
        public DataTable GetUserDetailsByIdNameClients(string userId, string userName)
            {
            VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
            return userDetailsBL.GetUserDetailsByIdNameClient(userId, userName);
            }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Title = Resources.LocalizedText.SearchUserDetails;
                this.txtUserId.Attributes.Add("onkeypress", "javascript:return SpecialCharacterValidation(this);");
                if (!Page.IsPostBack)
                {
                    this.hdnvisitortype.Value =XSS.HtmlEncode(Request.QueryString["type"]);
                    if (this.hdnvisitortype.Value == "Client")
                        {
                             this.trError.Visible = true;
                        this.lblclienthost.Visible = true;
                        this.lblclienthost.Text = "Host should be M+ level associate for client visitor.";
                        }

                    Page.Form.DefaultButton = this.butSearch.UniqueID;
                    this.txtUserId.Focus();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The butSearch_Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void ButSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (this.hdnvisitortype.Value == "Client")
                    {
                    dt = this.GetUserDetailsByIdNameClients(this.txtUserId.Text, this.txtUserName.Text);
                    if (dt.Rows.Count == 0)
                        {
                        this.grdSearchUserDetails.DataBind();
                        this.trError.Visible = true;
                        this.lblErrorMsg.Visible = true;
                        this.lblErrorMsg.Text = "Entered associate doesn't belong to M+ level.";
                        }
                    else
                        {
                        this.pnlSearch.Visible = true;
                        this.trError.Visible = false;
                        this.lblErrorMsg.Visible = false;
                        this.grdSearchUserDetails.DataSource = dt;
                        this.grdSearchUserDetails.DataBind();
                        }
                    }
                else
                    {
                    dt = this.GetUserDetailsByIdName(this.txtUserId.Text, this.txtUserName.Text);
                    if (dt.Rows.Count == 0)
                        {
                        this.grdSearchUserDetails.DataBind();
                        this.trError.Visible = true;
                        this.lblErrorMsg.Visible = true;
                        this.lblErrorMsg.Text = Resources.LocalizedText.AssociateDetailsPopUpCaption;
                        }
                    else
                        {
                        this.pnlSearch.Visible = true;
                        this.trError.Visible = false;
                        this.lblErrorMsg.Visible = false;
                        this.grdSearchUserDetails.DataSource = dt;
                        this.grdSearchUserDetails.DataBind();
                        }
                    }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Commented by 173710
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">grid view object</param>
        protected void GrdSearchUserDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AssociateId")
                {
                    this.txtUserId.Text = XSS.HtmlEncode(name + "(" + e.CommandArgument.ToString() + ")");
                }

                if (e.CommandName == "AssociateName")
                {
                    string name = e.CommandArgument.ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Commented by 173710
        /// </summary>
        /// <param name="sender"> object sender</param>
        /// <param name="e">grid view object</param>
        protected void GrdSearchUserDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lb = (LinkButton)e.Row.FindControl("lblAssociateId");
                    string fromHost;

                    ////string HostPhoneNo = Convert.ToString(e.Row.Cells[1].Text).Replace("&nbsp;", "");
                    if (Request.QueryString["FromHost"] != null && Request.QueryString["FromHost"] == "Y")
                    {
                        fromHost = Request.QueryString["FromHost"];
                        string strPhoneNo = e.Row.Cells[1].Text;
                        string hostdetails = fromHost;
                        ////+"oo" + strPhoneNo;
                        lb.Attributes.Add("onclick", "javascript:GetAssociateIdNameList(this,'" + hostdetails + "');");
                    }
                    else
                    {
                        fromHost = "N";
                        ////added for VMS CR VMS06072010CR09 by Priti
                        ////for testing defects VMS_CR02_11 VMS_CR06_08 of VMS06072010CR09
                        this.Session["HostChanged"] = true;
                        string strPhoneNo = e.Row.Cells[1].Text.ToString();
                        string hostdetails = fromHost;
                        HiddenField hidden = (HiddenField)e.Row.FindControl("HiddenField1");
                        string strhidden = hidden.Value;
                        ////+"oo" + strPhoneNo;
                        ////lb.Attributes.Add("onclick", "javascript:GetAssociateIdNameList(this,'" + Hostdetails + "');");
                        lb.Attributes.Add("onclick", "javascript:GetAssociateIdNameList(this,'" + fromHost + "','" + strhidden + "');");
                        //// lb.Attributes.Add("onclick", "javascript:GetAssociateIdNameList(this,'" + FromHost + "');");
                    }
                    ////Security not host start

                    string[] str1 = lb.Text.Split('(');

                    string[] str2 = str1[1].Split(')');

                    string id = str2[0].ToString();

                    VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL userDetailsBL = new VMSBusinessLayer.VMSBusinessLayer.UserDetailsBL();
                    string issecurity = userDetailsBL.IsSecurity(id);

                    ////if (Request.QueryString.ToString().ToUpper().Contains("ESCORT"))
                    ////{

                    ////}
                    ////else
                    ////{

                    if (issecurity.ToString() == "yes")
                    {
                        e.Row.Visible = false;
                    }

                    ////}//end
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
    }
}
