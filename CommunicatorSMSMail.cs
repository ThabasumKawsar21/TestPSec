// -----------------------------------------------------------------------
// <copyright file="CommunicatorSMSMail.cs" company="CTS">
// Copyright (c) . All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VMSBL.SMSMail
{
    using BusinessManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// communicator class for SMS mail sending
    /// </summary>
    public class CommunicatorSMSMail
    {
        /// <summary>
        /// communicator class for SMS mail sending
        /// </summary>
        [Serializable]
        public class ComSMSMail
        {
            /// <summary>
            /// transaction parameter value
            /// </summary>
            private TransactionParameters transactionParameters = null;

            /// <summary>
            /// template parameter value
            /// </summary>
            private TemplateParameters templateParameters = null;

            /// <summary>
            /// Gets or sets Transaction parameter value
            /// </summary>
            public TransactionParameters TransactionParameters
            {
                get
                {
                    return this.transactionParameters;
                }

                set
                {
                    this.transactionParameters = value;
                }
            }

            /// <summary>
            /// Gets or sets template parameters
            /// </summary>
            public TemplateParameters TemplateParameters
            {
                get
                {
                    return this.templateParameters;
                }

                set
                {
                    this.templateParameters = value;
                }
            }
        }

        /// <summary>
        /// one communicator class
        /// </summary>
        [Serializable]
        public class OneCommunicator
        {
            /// <summary>
            /// transaction parameter value
            /// </summary>
            private OneCommunicatorTransactionParameters transactionParameters = null;

            /// <summary>
            /// channel parameter value
            /// </summary>
            private ChannelParameters channelParameters = null;

            /// <summary>
            /// Gets or sets transaction parameter value
            /// </summary>
            public OneCommunicatorTransactionParameters TransactionParameters
            {
                get
                {
                    return this.transactionParameters;
                }

                set
                {
                    this.transactionParameters = value;
                }
            }

            /// <summary>
            /// Gets or sets channel parameters
            /// </summary>
            public ChannelParameters ChannelParameters
            {
                get
                {
                    return this.channelParameters;
                }

                set
                {
                    this.channelParameters = value;
                }
            }
        }

        /// <summary>
        /// channel parameter class
        /// </summary>
        [Serializable]
        public class ChannelParameters
        {
            /// <summary>
            /// email value
            /// </summary>
            private Email email = null;

            /// <summary>
            /// mobile value
            /// </summary>
            private Mobile mobile = null;


            /// <summary>
            /// SMS value
            /// </summary>
            private SMS sms = null;

            /// <summary>
            /// OCS value
            /// </summary>
            private string ocs = string.Empty;

            /// <summary>
            /// oneClick value
            /// </summary>
            private string oneClick = string.Empty;

            /// <summary>
            /// Gets or sets email value
            /// </summary>
            public Email Email
            {
                get
                {
                    return this.email;
                }

                set
                {
                    this.email = value;
                }
            }

            public Mobile Mobile
            {
                get
                {
                    return this.mobile;
                }

                set
                {
                    this.mobile = value;
                }
            }
            /// <summary>
            /// Gets or sets SMS value
            /// </summary>
            public SMS SMS
            {
                get
                {
                    return this.sms;
                }

                set
                {
                    this.sms = value;
                }
            }

            /// <summary>
            /// Gets or sets OCS value
            /// </summary>
            public string OCS
            {
                get
                {
                    return this.ocs;
                }

                set
                {
                    this.ocs = value;
                }
            }

            /// <summary>
            /// Gets or sets one click value
            /// </summary>
            public string OneClick
            {
                get
                {
                    return this.oneClick;
                }

                set
                {
                    this.oneClick = value;
                }
            }

        }




        /// <summary>
        /// SMS class
        /// </summary>
        [Serializable]
        public class SMS
        {
            /// <summary>
            /// short message value
            /// </summary>
            private string shortmeassage = string.Empty;

            /// <summary>
            /// is Required value
            /// </summary>
            private string isRequired = string.Empty;

            /// <summary>
            /// transaction parameters value
            /// </summary>
            private TransactionParameters transactionparameters = null;

            /// <summary>
            /// Gets or sets short message value
            /// </summary>
            public string ShortMessage
            {
                get
                {
                    return this.shortmeassage;
                }

                set
                {
                    this.shortmeassage = value;
                }
            }
            ////public bool  IsRequired 
            ////{
            ////    get
            ////    {
            ////        return _isRequired;
            ////    }
            ////    set
            ////    {
            ////        _isRequired = value;
            ////    }

            ////}

            /// <summary>
            /// Gets or sets transaction parameter
            /// </summary>
            public TransactionParameters TransactionParameters
            {
                get
                {
                    return this.transactionparameters;
                }

                set
                {
                    this.transactionparameters = value;
                }
            }
        }

        ////bincey
        /// <summary>
        /// email class
        /// </summary>
        public class Email
        {
            /// <summary>
            /// CC value
            /// </summary>
            private string cc = string.Empty;

            /// <summary>
            /// BCC value
            /// </summary>
            private string bcc = string.Empty;
            ////private string _RequestorId = string.Empty;

            /// <summary>
            /// template parameter value
            /// </summary>
            private TemplateParameters templateParameters = null;

            /// <summary>
            /// Gets or sets CC value
            /// </summary>
            public string CC
            {
                get
                {
                    return this.cc;
                }

                set
                {
                    this.cc = value;
                }
            }

            /// <summary>
            /// Gets or sets BCC value
            /// </summary>
            public string BCC
            {
                get
                {
                    return this.bcc;
                }

                set
                {
                    this.bcc = value;
                }
            }

            /// <summary>
            /// Gets or sets template parameter value
            /// </summary>
            public TemplateParameters TemplateParameters
            {
                get
                {
                    return this.templateParameters;
                }

                set
                {
                    this.templateParameters = value;
                }
            }
        }

        public class Mobile
        {
            /// <summary>
            /// CC value
            /// </summary>
            private string summary = string.Empty;


            private string summaryJSON = string.Empty;

            private string contentJSON = string.Empty;

            private string templateID = string.Empty;
            private string content = string.Empty;
            private string title = string.Empty;
            private string dueDate = string.Empty;


            public string Summary
            {
                get
                {
                    return this.summary;
                }

                set
                {
                    this.summary = value;
                }
            }

            /// <summary>
            /// Gets or sets BCC value
            /// </summary>
            public string SummaryJSON
            {
                get
                {
                    return this.summaryJSON;
                }

                set
                {
                    this.summaryJSON = value;
                }
            }

            /// <summary>
            /// Gets or sets BCC value
            /// </summary>
            public string ContentJSON
            {
                get
                {
                    return this.contentJSON;
                }

                set
                {
                    this.contentJSON = value;
                }
            }



            /// <summary>
            /// Gets or sets template parameter value
            /// </summary>
            public string TemplateID
            {
                get
                {
                    return this.templateID;
                }

                set
                {
                    this.templateID = value;
                }
            }
            public string Content
            {
                get
                {
                    return this.content;
                }

                set
                {
                    this.content = value;
                }
            }
            public string Title
            {
                get
                {
                    return this.title;
                }

                set
                {
                    this.title = value;
                }
            }

            public string DueDate
            {

                get
                {
                    return this.dueDate;
                }

                set
                {
                    this.dueDate = value;
                }
            }

        }





        /// <summary>
        /// one communicator transaction parameter class
        /// </summary>
        /// 
        [Serializable]
        public class OneCommunicatorTransactionParameters
        {
            /// <summary>
            /// recipient value
            /// </summary>
            private string recipients = string.Empty;

            /// <summary>
            /// request Id value
            /// </summary>
            private string requestId = string.Empty;

            /// <summary>
            /// process value
            /// </summary>
            private string process = string.Empty;

            /// <summary>
            /// global App Id value
            /// </summary>
            private string globalAppId = string.Empty;

            /// <summary>
            /// Gets or sets recipient value
            /// </summary>
            public string Recipients
            {
                get
                {
                    return this.recipients;
                }

                set
                {
                    this.recipients = value;
                }
            }

            /// <summary>
            /// Gets or sets Request Id value
            /// </summary>
            public string RequestId
            {
                get
                {
                    return this.requestId;
                }

                set
                {
                    this.requestId = value;
                }
            }

            /// <summary>
            /// Gets or sets process value
            /// </summary>
            public string Process
            {
                get
                {
                    return this.process;
                }

                set
                {
                    this.process = value;
                }
            }

            /// <summary>
            /// Gets or sets global App Id value
            /// </summary>
            public string GlobalAppId
            {
                get
                {
                    return this.globalAppId;
                }

                set
                {
                    this.globalAppId = value;
                }
            }
        }

        /// <summary>
        /// transaction parameter class
        /// </summary>
        public class TransactionParameters
        {
            /// <summary>
            /// request Id value
            /// </summary>
            private string requestID = string.Empty;

            /// <summary>
            /// CC list value
            /// </summary>
            private string cclist = string.Empty;

            /// <summary>
            /// requestor Id value
            /// </summary>
            private string requestorId = string.Empty;

            /// <summary>
            /// Gets or sets request Id
            /// </summary>
            public string RequestID
            {
                get
                {
                    return this.requestID;
                }

                set
                {
                    this.requestID = value;
                }
            }

            /// <summary>
            /// Gets or sets CC value
            /// </summary>
            public string CCList
            {
                get
                {
                    return this.cclist;
                }

                set
                {
                    this.cclist = value;
                }
            }

            /// <summary>
            /// Gets or sets requestor Id
            /// </summary>
            public string RequestorId
            {
                get
                {
                    return this.requestorId;
                }

                set
                {
                    this.requestorId = value;
                }
            }
        }

        /// <summary>
        /// template parameters class
        /// </summary>
        [Serializable]
        public class TemplateParameters
        {
            /// <summary>
            /// host name value
            /// </summary>
            private string hostName = string.Empty;

            /// <summary>
            /// visitor name value
            /// </summary>
            private string visitorName = string.Empty;

            /// <summary>
            /// facility value
            /// </summary>
            private string facility = string.Empty;

            /// <summary>
            /// in time value
            /// </summary>
            private string intime = string.Empty;

            /// <summary>
            /// in date value
            /// </summary>
            private string indate = string.Empty;

            /// <summary>
            /// manager value
            /// </summary>
            private string manager = string.Empty;

            /// <summary>
            /// city value
            /// </summary>
            private string city = string.Empty;

            /// <summary>
            /// from date value
            /// </summary>
            private string fromDate = string.Empty;

            /// <summary>
            /// to Date value
            /// </summary>
            private string todate = string.Empty;

            /// <summary>
            /// country value
            /// </summary>
            private string country = string.Empty;

            /// <summary>
            /// subject value
            /// </summary>
            private string subject = string.Empty;

            /// <summary>
            /// Gets or sets host name value
            /// </summary>
            public string HostName
            {
                get
                {
                    return this.hostName;
                }

                set
                {
                    this.hostName = value;
                }
            }

            /// <summary>
            /// Gets or sets visitor name
            /// </summary>
            public string VisitorName
            {
                get
                {
                    return this.visitorName;
                }

                set
                {
                    this.visitorName = value;
                }
            }

            /// <summary>
            /// Gets or sets facility value
            /// </summary>
            public string Facility
            {
                get
                {
                    return this.facility;
                }

                set
                {
                    this.facility = value;
                }
            }

            /// <summary>
            /// Gets or sets in time value
            /// </summary>
            public string InTime
            {
                get
                {
                    return this.intime;
                }

                set
                {
                    this.intime = value;
                }
            }

            /// <summary>
            /// Gets or sets in date value
            /// </summary>
            public string InDate
            {
                get
                {
                    return this.indate;
                }

                set
                {
                    this.indate = value;
                }
            }

            /// <summary>
            /// Gets or sets manager value
            /// </summary>
            public string Manager
            {
                get
                {
                    return this.manager;
                }

                set
                {
                    this.manager = value;
                }
            }

            /// <summary>
            /// Gets or sets city value
            /// </summary>
            public string City
            {
                get
                {
                    return this.city;
                }

                set
                {
                    this.city = value;
                }
            }

            /// <summary>
            /// Gets or sets from date value
            /// </summary>
            public string FromDate
            {
                get
                {
                    return this.fromDate;
                }

                set
                {
                    this.fromDate = value;
                }
            }

            /// <summary>
            /// Gets or sets to date value
            /// </summary>
            public string ToDate
            {
                get
                {
                    return this.todate;
                }

                set
                {
                    this.todate = value;
                }
            }

            /// <summary>
            /// Gets or sets country value
            /// </summary>
            public string Country
            {
                get
                {
                    return this.country;
                }

                set
                {
                    this.country = value;
                }
            }

            /// <summary>
            /// Gets or sets subject value
            /// </summary>
            public string Subject
            {
                get
                {
                    return this.subject;
                }

                set
                {
                    this.subject = value;
                }
            }
        }

        /// <summary>
        /// template parameter reminder part class
        /// </summary>
        [Serializable]
        public class TemplateParametersReminderpart
        {
            /// <summary>
            /// host name value
            /// </summary>
            private string hostName = string.Empty;

            /// <summary>
            /// visitor name value
            /// </summary>
            private string visitorName = string.Empty;

            /// <summary>
            /// facility value
            /// </summary>
            private string facility = string.Empty;

            /// <summary>
            /// in time value
            /// </summary>
            private string intime = string.Empty;

            /// <summary>
            /// in date value
            /// </summary>
            private string indate = string.Empty;

            /// <summary>
            /// manager value
            /// </summary>
            private string manager = string.Empty;

            /// <summary>
            /// city value
            /// </summary>
            private string city = string.Empty;

            /// <summary>
            /// from date value
            /// </summary>
            private string fromDate = string.Empty;

            /// <summary>
            /// to date value
            /// </summary>
            private string todate = string.Empty;

            /// <summary>
            /// country value
            /// </summary>
            private string country = string.Empty;

            /// <summary>
            /// subject value
            /// </summary>
            private string subject = string.Empty;

            /// <summary>
            /// admin POC
            /// </summary>
            private string adminPOC = string.Empty;

            /// <summary>
            /// Gets or sets Admin POC
            /// </summary>
            public string Adminpoc
            {
                get
                {
                    return this.adminPOC;
                }

                set
                {
                    this.adminPOC = value;
                }
            }

            /// <summary>
            /// Gets or sets host name
            /// </summary>
            public string HostName
            {
                get
                {
                    return this.hostName;
                }

                set
                {
                    this.hostName = value;
                }
            }

            /// <summary>
            /// Gets or sets visitor name
            /// </summary>
            public string VisitorName
            {
                get
                {
                    return this.visitorName;
                }

                set
                {
                    this.visitorName = value;
                }
            }

            /// <summary>
            /// Gets or sets facility value
            /// </summary>
            public string Facility
            {
                get
                {
                    return this.facility;
                }

                set
                {
                    this.facility = value;
                }
            }

            /// <summary>
            /// Gets or sets in time value
            /// </summary>
            public string InTime
            {
                get
                {
                    return this.intime;
                }

                set
                {
                    this.intime = value;
                }
            }

            /// <summary>
            /// Gets or sets in date 
            /// </summary>
            public string InDate
            {
                get
                {
                    return this.indate;
                }

                set
                {
                    this.indate = value;
                }
            }

            /// <summary>
            /// Gets or sets manager value
            /// </summary>
            public string Manager
            {
                get
                {
                    return this.manager;
                }

                set
                {
                    this.manager = value;
                }
            }

            /// <summary>
            /// Gets or sets city value
            /// </summary>
            public string City
            {
                get
                {
                    return this.city;
                }

                set
                {
                    this.city = value;
                }
            }

            /// <summary>
            /// Gets or sets from date value
            /// </summary>
            public string FromDate
            {
                get
                {
                    return this.fromDate;
                }

                set
                {
                    this.fromDate = value;
                }
            }

            /// <summary>
            /// Gets or sets to date value
            /// </summary>
            public string ToDate
            {
                get
                {
                    return this.todate;
                }

                set
                {
                    this.todate = value;
                }
            }

            /// <summary>
            /// Gets or sets country value
            /// </summary>
            public string Country
            {
                get
                {
                    return this.country;
                }

                set
                {
                    this.country = value;
                }
            }

            /// <summary>
            /// Gets or sets subject value
            /// </summary>
            public string Subject
            {
                get
                {
                    return this.subject;
                }

                set
                {
                    this.subject = value;
                }
            }
        }

        /// <summary>
        /// technical detail class
        /// </summary>
        [Serializable]
        public class TechnicalDetail
        {
            /// <summary>
            /// associate Id value
            /// </summary>
            private string associateID = string.Empty;

            /// <summary>
            /// associate name value
            /// </summary>
            private string associateName = string.Empty;

            /// <summary>
            /// SEZ facility value
            /// </summary>
            private string sezFacility = string.Empty;

            /// <summary>
            /// date of joining value
            /// </summary>
            private string dateofJoining = string.Empty;

            /// <summary>
            /// designation value
            /// </summary>
            private string designation = string.Empty;

            /// <summary>
            /// practice value
            /// </summary>
            private string practice = string.Empty;

            /// <summary>
            /// account value
            /// </summary>
            private string account = string.Empty;

            /// <summary>
            /// billability value
            /// </summary>
            private string billability = string.Empty;

            /// <summary>
            /// category value
            /// </summary>
            private string category = string.Empty;

            /// <summary>
            /// request id value
            /// </summary>
            private string requestid = string.Empty;

            /// <summary>
            /// SEZ status value
            /// </summary>
            private string sezStatus = string.Empty;

            /// <summary>
            /// vertical Id value
            /// </summary>
            private string verticalId = string.Empty;

            /// <summary>
            /// location value
            /// </summary>
            private string location = string.Empty;

            /// <summary>
            /// release date value
            /// </summary>
            private string releaseDate = string.Empty;

            /// <summary>
            /// release status value
            /// </summary>
            private string releaseStatus = string.Empty;

            /// <summary>
            /// Gets or sets associate Id value
            /// </summary>
            public string AssociateID
            {
                get
                {
                    return this.associateID;
                }

                set
                {
                    this.associateID = value;
                }
            }

            /// <summary>
            /// Gets or sets associate name value
            /// </summary>
            public string AssociateName
            {
                get
                {
                    return this.associateName;
                }

                set
                {
                    this.associateName = value;
                }
            }

            /// <summary>
            /// Gets or sets SEZ facility
            /// </summary>
            public string SEZFacility
            {
                get
                {
                    return this.sezFacility;
                }

                set
                {
                    this.sezFacility = value;
                }
            }

            /// <summary>
            /// Gets or sets date of joining value
            /// </summary>
            public string DateofJoining
            {
                get
                {
                    return this.dateofJoining;
                }

                set
                {
                    this.dateofJoining = value;
                }
            }

            /// <summary>
            /// Gets or sets designation value
            /// </summary>
            public string Designation
            {
                get
                {
                    return this.designation;
                }

                set
                {
                    this.designation = value;
                }
            }

            /// <summary>
            /// Gets or sets practice value
            /// </summary>
            public string Practice
            {
                get
                {
                    return this.practice;
                }

                set
                {
                    this.practice = value;
                }
            }

            /// <summary>
            /// Gets or sets account value
            /// </summary>
            public string Account
            {
                get
                {
                    return this.account;
                }

                set
                {
                    this.account = value;
                }
            }

            /// <summary>
            /// Gets or sets billability value
            /// </summary>
            public string Billability
            {
                get
                {
                    return this.billability;
                }

                set
                {
                    this.billability = value;
                }
            }

            /// <summary>
            /// Gets or sets category value
            /// </summary>
            public string Category
            {
                get
                {
                    return this.category;
                }

                set
                {
                    this.category = value;
                }
            }

            /// <summary>
            /// Gets or sets request id
            /// </summary>
            public string Requestid
            {
                get
                {
                    return this.requestid;
                }

                set
                {
                    this.requestid = value;
                }
            }

            /// <summary>
            /// Gets or sets SEZ status
            /// </summary>
            public string SEZStatus
            {
                get
                {
                    return this.sezStatus;
                }

                set
                {
                    this.sezStatus = value;
                }
            }

            /// <summary>
            /// Gets or sets vertical id
            /// </summary>
            public string VerticalId
            {
                get
                {
                    return this.verticalId;
                }

                set
                {
                    this.verticalId = value;
                }
            }

            /// <summary>
            /// Gets or sets Location value
            /// </summary>
            public string Location
            {
                get
                {
                    return this.location;
                }

                set
                {
                    this.location = value;
                }
            }

            /// <summary>
            /// Gets or sets release date
            /// </summary>
            public string ReleaseDate
            {
                get
                {
                    return this.releaseDate;
                }

                set
                {
                    this.releaseDate = value;
                }
            }

            /// <summary>
            /// Gets or sets release status
            /// </summary>
            public string ReleaseStatus
            {
                get
                {
                    return this.releaseStatus;
                }

                set
                {
                    this.releaseStatus = value;
                }
            }
        }
    }
    }

