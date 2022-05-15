//-----------------------------------------------------------------------
// <copyright file="VMSUtility.cs" company="Cognizant Technology Solution">
// Copyright(c). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace VMSUtility
{
    using CTS.OneCognizant.Platform.CoreServices;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Security;
    using System.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.Util;

    //using CTS.OneCognizant.Platform.CoreServices;


    /// <summary>
    /// VMS Utility Class
    /// </summary>
    public static class VMSUtility
    {
        #region Enumerator

        /// <summary>
        /// Log level value
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// normal value
            /// </summary>
            Normal = 0,

            /// <summary>
            /// Debug value
            /// </summary>
            Debug = 1
        }
        #endregion        

        /// <summary>
        /// Get User ID
        /// </summary>
        /// <returns>user Id value</returns>
        public static string GetUserId()
        {

            string strUserId = string.Empty;
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings.GetValues("PROXYUSER_MODE")[0].Equals("On")
                    && !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["proxyuser"]))
                {
                    strUserId = HttpContext.Current.Request.QueryString["proxyuser"];
                }
                else
                {
                    //UserContext usr = UserContext.GetUserContext();
                    //strUserId = usr.CurrentUser.UserId;
                    strUserId = "597397";
                }
               

            }
            catch (SystemException ex)
            {
               
                //Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
                 throw ex;
            }

            return strUserId;
          }

        /// <summary>
        /// To check Country Time zone value
        /// </summary>
        /// <returns>country time zone value</returns>
        public static bool CountryTimeZoneCheck()
        {
            try
            {
               string str = (string)HttpContext.Current.Session["TimezoneOffset"];

              if (string.Compare(str, "-330") == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                }
            catch (Exception ex)
            {
                throw ex;
                ////Utility.VMSUtility.logExceptionAndShowErrorPage(ex, HttpContext.Current);               
            }
        }

        /// <summary>
        /// Implements mailing functionality to send mail to outside Cognizant domain.
        /// </summary>
        /// <summary>
        /// Send mail to outside Cognizant domain.
        /// </summary>
        /// <param name="mailSubject">mail subject</param>
        /// <param name="mailBody">Mail Body</param>
        /// <param name="mailTo">Mail To</param>
        /// <returns>has send mail</returns>
        //public static bool SendEmailnew(
        //    string mailSubject, string mailBody, string mailTo)
        //{
        //    bool isSend = false;
        //    string[] multipleEmailAddress = null;
        //    ExchangeService service = new ExchangeService();
        //    ////            service.AutodiscoverUrl(ConfigurationManager.AppSettings

        //    ////["ExternalEmailBoxAddress"].ToString());
        //    service.AutodiscoverUrl("VisitorManagement@cognizant.com");
        //    string userName = ConfigurationManager.AppSettings["MailBoxUserID"].ToString();
        //    string codeword = ConfigurationManager.AppSettings["MailBoxUserValue"].ToString();
        //    string domain = ConfigurationManager.AppSettings["DomainName"].ToString();
        //    var securePassword = new SecureString();
        //    foreach (var c in codeword)
        //    {
        //        securePassword.AppendChar(c);
        //    }

        //    EmailMessage emailMessage = new EmailMessage(service);
        //    emailMessage.Subject = mailSubject;
        //    string body = mailBody;
        //    emailMessage.Body = body;
        //    if (mailTo != null)
        //    {
        //        multipleEmailAddress = mailTo.Split(';');
        //    }

        //    foreach (string email in multipleEmailAddress)
        //    {
        //        if (!string.IsNullOrEmpty(email))
        //        {
        //            emailMessage.ToRecipients.Add(email);
        //        }
        //    }

        //    //////For CC
        //    ////if (!string.IsNullOrEmpty(mailCC))
        //    ////{
        //    ////    string[] multipleccEmailAddress = mailCC.Split(';');
        //    ////    foreach (string ccemail in multipleccEmailAddress)
        //    ////    {
        //    ////        if (!string.IsNullOrEmpty(ccemail))
        //    ////        {
        //    ////            emailMessage.CcRecipients.Add(ccemail);
        //    ////        }
        //    ////    }
        //    ////}

        //    service.Credentials = new System.Net.NetworkCredential(userName, securePassword, domain);
        //    ////if (!string.IsNullOrEmpty(filePath))
        //    ////{
        //    ////    string file = System.IO.Path.GetFileName(filePath);
        //    ////    emailMessage.Attachments.AddFileAttachment(file, filePath);
        //    ////    emailMessage.Attachments[0].IsInline = true;
        //    ////    emailMessage.Attachments[0].ContentId = file;
        //    ////}

        //    //// emailMessage.SendAndSaveCopy(WellKnownFolderName.SentItems);
        //    emailMessage.Send();
        //    isSend = true;
        //    return isSend;
        //}

        #region Public Methods
        /// <summary>
        /// Send mail to the requestor
        /// </summary>
        /// <param name="toaddress">To Address</param>
        /// <param name="fromAddress">From Address</param>
        /// <param name="smtpHostAddress">SMTP Host address</param>
        /// <param name="ismtpPort">SMTP port value</param>
        /// <param name="subjectLine">Subject line value</param>
        /// <param name="messageType">message type</param>
        /// <param name="managerAddress">manager address</param>
        /// <returns>mail to be sent</returns>
        public static bool SendEmail(
            string toaddress, 
            string fromAddress, 
            string smtpHostAddress,
            int ismtpPort, 
            string subjectLine, 
            string messageType, 
            string managerAddress)
        {
            bool blnReturn = false;
            try
            {
                MailMessage mailMessage = new MailMessage(
                    new MailAddress(
                        fromAddress, 
                        fromAddress),
                    new MailAddress(toaddress, toaddress));
                if (!string.IsNullOrEmpty(managerAddress))
                {
                    mailMessage.CC.Add(managerAddress);
                }

                mailMessage.Subject = subjectLine;
                mailMessage.Body = messageType;
                ////mailMessage.IsBodyHtml = true;
                ////SmtpClient smtpClient = new SmtpClient(strSMTPHostAddress, iSMTPPort);
                ////smtpClient.Send(mailMessage);

                ////Begin Fix for Image Issue in Mails
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(messageType, null, "text/html");

                ////create the LinkedResource (embedded image)
                ////LinkedResource logo = new LinkedResource(System.Configuration.ConfigurationManager.AppSettings["LogoImage"].ToString());
                LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Images/Logo_cognizant.png"));
                logo.ContentId = "companylogo";
                ////add the LinkedResource to the appropriate view
                htmlView.LinkedResources.Add(logo);
                mailMessage.AlternateViews.Add(htmlView);
                //// mailMessage.IsBodyHtml = true;

                //// Commenting as part of 1C move
                ////SmtpClient smtpClient = new SmtpClient(smtpHostAddress, ismtpPort);
                ////smtpClient.Send(mailMessage);

                ////End Fix for Image Issue in Mails                

                //SendEmailnew(subjectLine, messageType, toaddress);

                blnReturn = true;
                return blnReturn;
            }
            catch (SmtpException)
            {
                string htmlFiles = System.Configuration.ConfigurationManager.AppSettings["HTMLFiles"].ToString();

                System.IO.FileInfo notifierCheck = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(htmlFiles).ToString() + "TestMail_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".html");

                if (notifierCheck.Exists)
                {
                    // System.IO.StreamWriter s = notifierCheck.CreateText();
                    System.IO.StreamWriter s = notifierCheck.AppendText();

                    s.WriteLine(" ");
                    s.WriteLine(@"</br>----------------------------------------------------------------------------------</br>");
                    s.WriteLine(@"<b>Subject:</b>" + subjectLine);
                    s.WriteLine(@"</br><b>To:</b>" + toaddress);
                    s.WriteLine(@"</br><b>CC:</b>" + managerAddress + "</br></br>");
                    s.WriteLine(messageType);
                    s.Close();
                }
                else
                {
                    System.IO.StreamWriter s = notifierCheck.AppendText();

                    s.WriteLine(" ");
                    s.WriteLine(@"<b>Subject:</b>" + subjectLine);
                    s.WriteLine(@"</br><b>To:</b>" + toaddress);
                    s.WriteLine(@"</br><b>CC:</b>" + managerAddress + "</br></br>");
                    s.WriteLine(messageType);
                    s.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;

                ////Utility.VMSUtility.logExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Gets the content of the mail.
        /// </summary>
        /// <param name="managerName">Manager name</param>
        /// <param name="employeeName">Employee name</param>
        /// <param name="employeeId">Employee Id</param>
        /// <param name="issuedFacility">issued facility value</param>
        /// <returns>mail content as a string</returns>
        public static string GetMailFormat(
            string managerName, 
            string employeeName, 
            string employeeId, 
            string issuedFacility)
        {
            StringBuilder strBodyText = new StringBuilder();
            strBodyText.Append(string.Empty);

            try
            {
                strBodyText.Append("<html><title></title><body>");
                strBodyText.Append("Hi " + managerName.Trim() + "," + "<br/><br/>This is for your kind attention that " + employeeName.Replace(",", " "));
                strBodyText.Append("(" + employeeId + ")" + " has not declared Cognizant ID card while entering the Cognizant ( " + issuedFacility + " ) ");
                strBodyText.Append(" premises and has been issued a One Day ID Card for " + DateTime.Today.DayOfWeek + " " + DateTime.Today.ToString("MMMM dd yyyy") + " ");
                strBodyText.Append("to enter the facility." + "<br/><br/>The ID card is valid only for today and ");
                strBodyText.Append("needs to be returned duly signed by the supervisor while leaving the office premises.");
                ////begin LVS19032010CR01
                strBodyText.Append("<br/><br/><br/>Thanks,<br/>Physical Security Team. <br/><br/>");
                ////end
                strBodyText.Append("Note : The mail is auto generated mail, please do not reply to this mail.");
                strBodyText.Append("Kindly get back to Physical Security team (physicalsecuritychn@cognizant.com) ");
                strBodyText.Append("for any queries.</body></html>");
                return strBodyText.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The Send Email To Manager method
        /// </summary>
        /// <param name="employeeId">The employee Id parameter</param>
        /// <param name="empemailId">The employee email Id parameter</param>
        /// <param name="managerName">The manager Name parameter</param>
        /// <param name="employeeName">The employee Name parameter</param>
        /// <param name="toaddress">The to Address parameter</param>
        /// <param name="fromAddress">The from Address parameter</param>
        /// <param name="smtpHostAddress">The mail Host Address parameter</param>
        /// <param name="ismtpPort">The mail Port parameter</param>
        /// <param name="subjectLine">The subject Line parameter</param>
        /// <param name="issuedFacility">The issued Facility parameter</param>
        /// <returns>The boolean type object</returns>        
        public static bool SendEmailToManager(
            string employeeId, 
            string empemailId, 
            string managerName, 
            string employeeName, 
            string toaddress, 
            string fromAddress, 
            string smtpHostAddress, 
            int ismtpPort, 
            string subjectLine, 
            string issuedFacility)
        {
            bool blnReturn = false;

            try
            {
                ////strToAddress = strToAddress;
                string strBodyText = GetMailFormat(managerName, employeeName, employeeId, issuedFacility);
                ////string strAdminEmailId = ConfigurationManager.AppSettings["FromAddress"].ToString();
                MailMessage mailMessage = new MailMessage(
                    new MailAddress(
                        fromAddress, 
                        fromAddress),
                new MailAddress(toaddress, toaddress));
                mailMessage.CC.Add(empemailId.ToString());
                mailMessage.Body = strBodyText;
                mailMessage.Subject = subjectLine;
                mailMessage.IsBodyHtml = true;

                ////Commented for 1C move
                ////SmtpClient smtpClient = new SmtpClient(smtpHostAddress, ismtpPort);
                ////smtpClient.Send(mailMessage);
                //SendEmailnew(subjectLine, strBodyText, toaddress);

                blnReturn = true;
                return blnReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Function to write log
        /// </summary>
        /// <param name="logText">log text value</param>
        /// <param name="loggingLevel">logging level value</param>
        public static void WriteLog(string logText, LogLevel loggingLevel)
        {
            //try
            //{
                //string logEnabled = ConfigurationManager.AppSettings["GenLogEnabled"].ToString();

                //string logFile = ConfigurationManager.AppSettings["LogFileName"].ToString();

                //logFile = logFile + "Log_GeneralMessage_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".txt";

                //if (string.Compare(logEnabled, "true") == 0)
                //{
                //    using (StreamWriter log = new StreamWriter(logFile, true))
                //    {
                //        logText = logText + " General Log On " + string.Format("{0:MM/dd/yy}", DateTime.Now)
                //        + " " + DateTime.Now.ToLongTimeString().ToString();
                //        log.WriteLine("*********************************************************************");
                //        log.WriteLine(logText);
                //    }
                //}
            //}
            //catch (Exception ex)
            //{
                
            //}
        }

        /// <summary>
        /// function to write SAN Log
        /// </summary>
        /// <param name="logText">log text value</param>
        /// <param name="loggingLevel">logging level value</param>
        public static void WriteSANLog(string logText, LogLevel loggingLevel)
        {
            //try
            //{
            //    string logEnabled = ConfigurationManager.AppSettings["SANLogEnabled"].ToString();

            //    string logFile = ConfigurationManager.AppSettings["LogFileName"].ToString();

            //    logFile = logFile + "Log_SANStorage_Message" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".txt";

            //    if (string.Compare(logEnabled, "true") == 0)
            //    {
            //        using (StreamWriter log = new StreamWriter(logFile, true))
            //        {
            //            logText = logText + " SAN Storage Log On " + string.Format("{0:MM/dd/yy}", DateTime.Now)
            //            + " " + DateTime.Now.ToLongTimeString().ToString();
            //            log.WriteLine("*********************************************************************");
            //            log.WriteLine(logText);
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    // By pass Exception
            //}
        }

        ///// <summary>
        ///// To Get IST Date Time
        ///// </summary>
        ///// <param name="dtDate"></param>
        ///// <returns></returns>
        ////public static DateTime GetISTTimeZone(DateTime dtDate)
        ////{

        // string timeZoneFormat = Convert.ToString(ConfigurationManager.AppSettings["TimeZone"]);
        // DateTimeOffset dtISTTime = TimeZoneInfo.ConvertTime(dtDate,
        // TimeZoneInfo.FindSystemTimeZoneById(timeZoneFormat));
        // string dtFromTime = dtISTTime.ToString("MM/dd/yyyy HH:mm:ss");
        // return Convert.ToDateTime(dtFromTime);
        ////}

        /// <summary>
        /// To Get IST Date Time
        /// </summary>
        /// <param name="dtDate"></param>
        ///// <returns></returns>
        ////public static DateTime GetISTDate(DateTime dtDate)
        ////{

        // TimeZoneInfo timeZoneInfo;
        // DateTime dateTime;
        // string timeZoneFormat = Convert.ToString(ConfigurationManager.AppSettings["TimeZone"]);
        ////Set the time zone information to US Mountain Standard Time 
        // timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneFormat);
        ////Get date and time in US Mountain Standard Time 
        // dateTime = TimeZoneInfo.ConvertTime(dtDate, timeZoneInfo);
        // return dateTime;
        ////}

        /// <summary>
        /// To Get IST Time Zone from different time zone 
        /// </summary>
        /// <param name="timeValue"></param>
        /// <returns></returns>
        ////public static string GetTimeToISTZone(string timeValue)
        ////{
        //    try
        //    {
        //        string timeZoneFormat = Convert.ToString(ConfigurationManager.AppSettings["TimeZone"]);
        //        string dtFromTime = DateTime.Parse(timeValue).ToString("MM/dd/yyyy HH:mm:ss");
        //        DateTimeOffset FromDateTime = TimeZoneInfo.ConvertTime(Convert.ToDateTime(dtFromTime),
        //        TimeZoneInfo.FindSystemTimeZoneById(timeZoneFormat));
        //        string fromTime = FromDateTime.ToString("HH:mm");
        //        return fromTime;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        ////}

        #endregion
    }
}
