using CAS.Security.Application;
using ECMCommon;
using ECMSharedServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using VMSUtility;

namespace VMSDev
{
    public partial class ECM : System.Web.UI.Page
    {
        private WrapperCheckIn objCheckInServices;

        /// <summary>
        /// Used to call Search service Library methods
        /// </summary>
        ////private WrapperSearch objSearchServices;

        /// <summary>
        /// Used to call Additional service Library methods
        /// </summary>
        ////private WrapperAdditional objCheckInAdditionalServices;

        /// <summary>
        /// Used to assign result from Wrapper
        /// </summary>
        private XDocument ecmResult;

        /// <summary>
        /// Used to assign application id
        /// </summary>
        private int appId = Convert.ToInt32(ConfigurationManager.AppSettings["ECMAPPID"]);

        /// <summary>
        /// Used to assign file upload page
        /// </summary>
        private string fileUploadUI = Convert.ToString(ConfigurationManager.AppSettings["UploadUI"]);

        /// <summary>
        /// Used to final query string
        /// </summary>
        private string finalQueryString = string.Empty;

        /// <summary>
        /// Used to assign client URL
        /// </summary>
        private string clientURL = Convert.ToString(ConfigurationManager.AppSettings["ECMOneCQueryUrl"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ClientScript.RegisterClientScriptBlock(
                //   this.GetType(),
                // "script",
                // "<script language='javascript'>window.parent.window.alert('Page Load!');</script>");

                if (!this.IsPostBack)
                {

                    ///ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertPostback", "alert('If not post back BLOCK');", true);
                    this.objCheckInServices = new WrapperCheckIn(this.appId);

                    if (Request.Form["_ecmMReturnHdnField"] != null)
                    {
                        this.ecmMHiddenField.Value = Request.Form["_ecmMReturnHdnField"];
                        this.ecmResult = XDocument.Parse(this.objCheckInServices.GetMetaResultValue(
                                                this.appId,
                                                this.ecmMHiddenField.Value));
                        XElement returnElement = this.ecmResult != null ? this.ecmResult.Root.Element("ECMStatus")
                                                                : null;
                        if (returnElement != null)
                        {
                            var responseData = from datas in this.ecmResult.Descendants("ECMStatus")
                                               select new
                                               {
                                                   dContentId = datas.Element("ECMContentId").Value,
                                                   dAppDocId = datas.Element("AppDocId").Value,
                                                   ////dID = datas.Element("ECMdID").Value,
                                                   fileName = datas.Element("OriginalFileName").Value
                                               };
                            /*this.Session["ECMFileContentId"] = Convert.ToString(responseData.FirstOrDefault().dContentId)*/;//added for ecm implentation
                            WrapperUICheckIn objECMCheckInScrapSaleEmail = new WrapperUICheckIn(appId);
                            byte[] fileContent = null;
                            ECMCommon.IdcFile fileVal = new ECMCommon.IdcFile();
                            fileVal = objECMCheckInScrapSaleEmail.DownloadFileContent(Convert.ToString(responseData.FirstOrDefault().dContentId), appId);
                            fileContent = fileVal.Filecontent;
                            this.Session["VisitorImgByte"] = fileContent;
                            this.Session["Webcamimage"] = null;
                            this.Session["UploadImage"] = fileContent;
                            this.FileContentId.Value = Convert.ToString(responseData.FirstOrDefault().dContentId);
                            this.FileAppDocId.Value = Convert.ToString(responseData.FirstOrDefault().dAppDocId);
                            this.originalFileName.Value = Convert.ToString(responseData.FirstOrDefault().fileName);

                        }

                        ClientScript.RegisterClientScriptBlock(
                         this.GetType(),
                       "scriptWindowBlock",
                       "<script language='javascript'>window.parent.document.forms[0].submit();window.returnValue = true;window.close();</script>");
                    }
                    else if (Request.Form["ctl00$MainContent$_ecmMReturnHdnField"] != null)
                    {
                        this.ecmMHiddenField.Value = Request.Form["ctl00$MainContent$_ecmMReturnHdnField"];
                        this.ecmResult = XDocument.Parse(this.objCheckInServices.GetMetaResultValue(
                                this.appId,
                                this.ecmMHiddenField.Value));

                    }

                }


                

                XDocument metadata = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/ECMMetaData/ECMMetaData.xml"));
                ////XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/ECMMetaData/ECMMetaData.xml"));
                this._ecmMClientHdnField.Value = this.objCheckInServices.PrepareEncryptionKey(
                                    this.appId,
                                    Convert.ToString(metadata));

                this.GetQueryStringValues();
                string URL = string.Format(this.fileUploadUI + this.finalQueryString);

                ////ECMUploadForm.Action = URL;
                this.Button1.PostBackUrl = URL;


            }

            catch (Exception ex)
            {
                throw ex;
            }          

        }

        /// <summary>
        /// method to get the query string
        /// </summary>
        /// <returns>the query string url</returns>
        private string GetQueryStringValues()
        {
            string logonUser = HttpContext.Current.User.Identity.Name;
            if (!logonUser.Contains("CTS"))
            {
                logonUser = "CTS\\" + HttpContext.Current.User.Identity.Name;
            }

            string encryptedKey = string.Empty;
            try
            {
                this.objCheckInServices = new WrapperCheckIn(this.appId);
                encryptedKey = this.objCheckInServices.GetAuthrTokenKey(this.appId, logonUser);
                this.finalQueryString = string.Format(
                    "?ECMAppId={0}&ECMOneCQueryUrl={1}&ECMAuthrTokenKey={2}",
                        this.appId,
                        this.clientURL,
                        encryptedKey);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
            //catch (FormatException ex)
            //{
            //    var frame = new System.Diagnostics.StackFrame(0);
            //    Exceptions.ExceptionserviceCall(
            //        ex, frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);
            //    throw;
            //}
            //catch (InvalidCastException ex)
            //{
            //    var frame = new System.Diagnostics.StackFrame(0);
            //    Exceptions.ExceptionserviceCall(
            //        ex, frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);
            //    throw;
            //}
            //catch (NotSupportedException ex)
            //{
            //    var frame = new System.Diagnostics.StackFrame(0);
            //    Exceptions.ExceptionserviceCall(
            //        ex, frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);
            //    throw;
            //}
            //catch (OverflowException ex)
            //{
            //    var frame = new System.Diagnostics.StackFrame(0);
            //    Exceptions.ExceptionserviceCall(
            //        ex, frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);
            //    throw;
            //}
            //catch (NullReferenceException ex)
            //{
            //    var frame = new System.Diagnostics.StackFrame(0);
            //    Exceptions.ExceptionserviceCall(
            //        ex, frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);
            //    throw;
            //}
            //catch (AccessViolationException ex)
            //{
            //    var frame = new System.Diagnostics.StackFrame(0);
            //    Exceptions.ExceptionserviceCall(
            //        ex, frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);
            //    throw;
            //}
            //catch (ArgumentNullException ex)
            //{
            //    var frame = new System.Diagnostics.StackFrame(0);
            //    Exceptions.ExceptionserviceCall(
            //        ex, frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);
            //    throw;
            //}

            return this.finalQueryString;
        }    
      
    }
}
