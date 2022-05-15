
namespace BusinessManager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Web;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using VMSBL.OneCommunicatorService;
    
    /// <summary>
    /// To Define Mail Notification
    /// </summary>
    public class MailNotification
    {
        /// <summary>
        /// function to send revoke notification
        /// </summary>
        /// <param name="associateId">associate Id value</param>
        /// <param name="associateName">associate name value</param>
        /// <param name="updatedBy">updated by value</param>
        /// <param name="role">role value</param>
        /// <param name="facility">facility value</param>
        public void SendRevokeNotification(
             string associateId, 
             string associateName, 
             string updatedBy, 
             string role, 
             string facility)
        {
            TemplateParameters templateParameters = new TemplateParameters();
            templateParameters.AssociateId = associateId.Trim();
            templateParameters.AssociateName = associateName.Trim();
            templateParameters.Role = role.Trim();
            templateParameters.FacilityAddress = facility.Trim();

            OneCommunicatorTransactionParameters oneCommunicatorTransactionParameters = new OneCommunicatorTransactionParameters();
            oneCommunicatorTransactionParameters.GlobalAppId = "116";
            oneCommunicatorTransactionParameters.Process = "PhysicalSecurity_IVSRevokeAccessNotification";
            oneCommunicatorTransactionParameters.Recipients = associateId;
            oneCommunicatorTransactionParameters.RequestId = Guid.NewGuid().ToString();
            Email email = new Email();
            email.CC = updatedBy;
            this.MailSettings(templateParameters, oneCommunicatorTransactionParameters, email);
        }

        /// <summary>
        /// Checkout mail notification
        /// </summary>
        /// <param name="associateId">associate Id value</param>
        /// <param name="associateName">associate name value</param>
        /// <param name="issuedlocation">issued location value</param>
        /// <param name="location">location value</param>
        /// <param name="accesstype">Access type value</param>
        /// <param name="country">country value</param>
        public void SendCheckoutNotification(
            string associateId, 
            string associateName, 
            string issuedlocation, 
            string location, 
            string accesstype, 
            string country)
        {
            string s = ConfigurationManager.AppSettings["OnedayAccessCard_PANIND_Mailer"].ToString();
            string[] onedayAccesscard = s.Split(',');
            foreach (string strCountrychk in onedayAccesscard)
            {
                if (strCountrychk == country)
                {
                    TemplateParameters templateParameters = new TemplateParameters();
                    templateParameters.AssociateId = associateId.Trim();
                    templateParameters.AssociateName = associateName;
                    templateParameters.FacilityAddress = location;
                    templateParameters.CheckoutTime = DateTime.Now.ToString("dd/MMM/yyyy");
                    if (accesstype == "1 Day ID Card")
                    {
                        templateParameters.EmailBodyText = @"Thank you for surrendering the Temporary Identity card in " + location + " on " + DateTime.Now.ToString("dd/MMM/yyyy") + ", which was issued from " + issuedlocation +
                        @".";
                    }
                    else if (accesstype == "1 Day Access Card")
                    {
                        templateParameters.EmailBodyText = @"Thank you for surrendering the Temporary Access Card in " + location + " on " + DateTime.Now.ToString("dd/MMM/yyyy") + ", which was issued from " + issuedlocation +
                        @".";
                    }
                    else if (accesstype == "1 day ID Card and Access Card")
                    {
                        templateParameters.EmailBodyText = @"Thank you for surrendering the Temporary Identity card and Access Card in " + location + " on " + DateTime.Now.ToString("dd/MMM/yyyy") + ", which was issued from " + issuedlocation +
                        @".";
                    }
                    else if (accesstype == " ") 
                    {
                        ////If the dropdown with no value
                        templateParameters.EmailBodyText = @"Thank you for surrendering the Card in " + location + " on " + DateTime.Now.ToString("dd/MMM/yyyy") + ", which was issued from " + issuedlocation +
                        @".";
                    }
                    //// templateParameters.emailBodyText = @"We would like to thank you for returning your ‘One Day’ Identify card and Access Card for " + location + " on " + DateTime.Now.ToString("dd/MMM/yyyy") +
                    //// @".";

                    OneCommunicatorTransactionParameters oneCommunicatorTransactionParameters = new OneCommunicatorTransactionParameters();
                    oneCommunicatorTransactionParameters.GlobalAppId = "116";
                    oneCommunicatorTransactionParameters.Process = "PhysicalSecurity_IVSCheckoutNotification";
                    oneCommunicatorTransactionParameters.Recipients = associateId;
                    oneCommunicatorTransactionParameters.RequestId = Guid.NewGuid().ToString();
                    Email email = new Email();
                    this.MailSettings(templateParameters, oneCommunicatorTransactionParameters, email);
                }
                else
                {
                    TemplateParameters templateParameters = new TemplateParameters();
                    templateParameters.AssociateId = associateId.Trim();
                    templateParameters.AssociateName = associateName;
                    templateParameters.FacilityAddress = location;
                    templateParameters.CheckoutTime = DateTime.Now.ToString("dd/MMM/yyyy");
                    templateParameters.EmailBodyText = @"We would like to thank you for returning your ‘One Day’ Identity card  for " + location + " on " + DateTime.Now.ToString("dd/MMM/yyyy") +
                       @".";

                    OneCommunicatorTransactionParameters oneCommunicatorTransactionParameters = new OneCommunicatorTransactionParameters();
                    oneCommunicatorTransactionParameters.GlobalAppId = "116";
                    oneCommunicatorTransactionParameters.Process = "PhysicalSecurity_IVSCheckoutNotification";
                    oneCommunicatorTransactionParameters.Recipients = associateId;
                    oneCommunicatorTransactionParameters.RequestId = Guid.NewGuid().ToString();
                    Email email = new Email();
                    this.MailSettings(templateParameters, oneCommunicatorTransactionParameters, email);
                }
            }
        }

        /// <summary>
        /// Enable card mail notification
        /// </summary>
        /// <param name="associateId">associate Id value</param>
        /// <param name="accessCardNo">access card Number value</param>
        /// /// <param name="associateName">associate name value</param>
        /// /// <param name="requestID">request Id value</param>
        public void EnableCardNotification(string associateId, string accessCardNo, string associateName, string requestID)
        {
            ////string requestId = requestid;
            string getdate = DateTime.Now.ToString();
            var requestXML = new StringBuilder();
            requestXML.Append("<OneCommunicator version=\"1\">");
            requestXML.Append("<TransactionParameters>");
            requestXML.Append("<GlobalAppId>116</GlobalAppId>");
            requestXML.Append("<Process>IVS_EnablePermanentAccessCard</Process>");
            requestXML.Append("<RequestId>" + requestID + "</RequestId>");
            requestXML.Append("<Recipients>" + associateId + "</Recipients>");
            requestXML.Append("</TransactionParameters>");
            requestXML.Append("<ChannelParameters>");
            requestXML.Append("<OCS ></OCS>");
            requestXML.Append("<Email>");
            requestXML.Append("<CC>" + null + "</CC>");
            requestXML.Append("<BCC/>");
            requestXML.Append("<TemplateParameters>");
            requestXML.Append("<AssociateID> " + associateId + "</AssociateID>");
            requestXML.Append("<AssociateName> " + associateName + "</AssociateName>");
            requestXML.Append("<getdate> " + getdate + "</getdate>");
            requestXML.Append("<AccessCardNo> " + accessCardNo + "</AccessCardNo>");
            requestXML.Append("</TemplateParameters>");
            requestXML.Append("</Email>");
            requestXML.Append("<SMS></SMS>");
            requestXML.Append("</ChannelParameters>");
            requestXML.Append("</OneCommunicator>");
            RequestUnifiedVASContractClient objOneCommEmailClient = new RequestUnifiedVASContractClient();
            objOneCommEmailClient.Notify(requestXML.ToString(), null);
        }

        #region Private Methods

        /// <summary>
        /// Mail settings
        /// </summary>
        /// <param name="templateParameters">template parameter value</param>
        /// <param name="oneCommunicatorTransactionParameters">one communicator transaction parameter value</param>
        /// <param name="email">email value</param>
        private void MailSettings(TemplateParameters templateParameters, OneCommunicatorTransactionParameters oneCommunicatorTransactionParameters, Email email)
        {
            email.TemplateParameters = templateParameters;
            ChannelParameters channelParameters = new ChannelParameters();
            channelParameters.Email = email;
            OneCommunicator oneCommunicator = new OneCommunicator();
            oneCommunicator.TransactionParameters = oneCommunicatorTransactionParameters;
            oneCommunicator.ChannelParameters = channelParameters;
            XDocument xmlDocument = null;
            RequestUnifiedVASContractClient requestUnifiedVASContractClient = null;
            try
            {
                XmlSerializer xmlSerializerEApproval = new XmlSerializer(oneCommunicator.GetType());
                StringBuilder approvalBuilder = new StringBuilder();
                StringWriter approvalWriter = new StringWriter(approvalBuilder);
                xmlSerializerEApproval.Serialize(approvalWriter, oneCommunicator);
                xmlDocument = XDocument.Load(new StringReader(approvalBuilder.ToString()));
                requestUnifiedVASContractClient = new RequestUnifiedVASContractClient();
                requestUnifiedVASContractClient.Notify(xmlDocument.ToString(), null);
                approvalWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (requestUnifiedVASContractClient != null && requestUnifiedVASContractClient.State != CommunicationState.Closed)
                {
                    requestUnifiedVASContractClient.Close();
                }
            }
        }
        #endregion
    }
}
