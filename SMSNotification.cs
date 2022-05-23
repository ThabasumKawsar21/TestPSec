
namespace VMSDev
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
    using VMSBL.SMS;
    using VMSDev.OneCommunicatorService;

    /// <summary>
    /// The SMSNotification class
    /// </summary>
    public class SMSNotification
    {
        ////CommunicatorSMSBLL blResource

        /// <summary>
        /// Send message To HCM method
        /// </summary>
        /// <param name="associateID">The Associate ID parameter</param>
        /// <param name="hostName">The Host Name parameter</param>
        /// <param name="facility">The Facility parameter</param>
        /// <param name="city">The City parameter</param>
        /// <param name="intime">The In Time parameter</param>
        /// <param name="passNumber">The Pass Number parameter</param>
        /// <param name="strManagerID">The Manager ID parameter</param>
        /// <param name="accesstype">The Access type parameter</param>
        /// <param name="country">The Country parameter</param>        
        public void SendIVSSmsToHCM(string associateID, string hostName, string facility, string city, string intime, string passNumber, string strManagerID, string accesstype, string country)
        {
            CommunicatorSMSBLL.TemplateParameters templateParameters = new CommunicatorSMSBLL.TemplateParameters();
            string associateFirstName = hostName.Split(',')[1].ToString();
            string associateLastName = hostName.Split(',')[0].ToString();
            hostName = associateFirstName + ' ' + associateLastName;
            templateParameters.HostName = hostName.Trim();
            templateParameters.AssociateID = associateID.Trim();
            templateParameters.City = city.Trim();
            templateParameters.Facility = facility.Trim();
            templateParameters.InTime = intime.Trim();
            templateParameters.Accesstype = accesstype.Trim();
            CommunicatorSMSBLL.OneCommunicatorTransactionParameters oneCommunicatorTransactionParameters = new CommunicatorSMSBLL.OneCommunicatorTransactionParameters();
            oneCommunicatorTransactionParameters.GlobalAppId = System.Configuration.ConfigurationManager.AppSettings["appId"]; ////"116";
            oneCommunicatorTransactionParameters.Process = VMSConstants.VMSConstants.IVSCHECKINSMS; ////"IVSCheckInSMSProcess";
            oneCommunicatorTransactionParameters.Recipients = strManagerID + ";" + associateID;
            oneCommunicatorTransactionParameters.RequestId = passNumber.Trim();
            CommunicatorSMSBLL.SMS sms = new CommunicatorSMSBLL.SMS();
            sms.ShortMessage = hostName + "(" + associateID + ")" + " has been issued with " + accesstype + " at " + facility + " at " + intime + ". This is for your information. Regards, Corporate Security Team.";
            CommunicatorSMSBLL.ChannelParameters channelParameters = new CommunicatorSMSBLL.ChannelParameters();
            channelParameters.SMS = sms;
            CommunicatorSMSBLL.OneCommunicator oneCommunicator = new CommunicatorSMSBLL.OneCommunicator();
            oneCommunicator.TransactionParameters = oneCommunicatorTransactionParameters;
            oneCommunicator.ChannelParameters = channelParameters;
            XDocument xdocument = null;
            RequestUnifiedVASContractClient requestUnifiedVASContractClient = null;
            string returnVal = string.Empty;
            try
            {
                XmlSerializer xmlSerializerEApproval = new XmlSerializer(oneCommunicator.GetType());
                StringBuilder sbeApproval = new StringBuilder();
                StringWriter sweApproval = new StringWriter(sbeApproval);
                xmlSerializerEApproval.Serialize(sweApproval, oneCommunicator);
                xdocument = XDocument.Load(new StringReader(sbeApproval.ToString()));
                sweApproval.Close();
                requestUnifiedVASContractClient = new RequestUnifiedVASContractClient();
                returnVal = requestUnifiedVASContractClient.Notify(xdocument.ToString(), null);
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
    }
}
