
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Security;
    using System.Text;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Newtonsoft.Json;
    using VMSBusinessLayer;
    using VMSDev.OneDayAccessCardService;
    using XSS = CAS.Security.Application.EncodeHelper;
    using ExceptionService;

    /// <summary>
    /// Client Page
    /// </summary>
    public partial class ClientPage : System.Web.UI.Page
    {
        /// <summary>
        /// get visitor list
        /// </summary>
        /// <param name="parentID">parent ID</param>
        /// <returns>return list</returns>
        [WebMethod]
        public static string GetVisitorList(int parentID)
        {
            ////DALClass objDal = new DALClass();
            ////HomePage home = new HomePage();
            string visitorslist = string.Empty;
            ClientPage page = new ClientPage();
            DataSet visitorlist = new DataSet();
            DataTable uniquevisitorlist = new DataTable();
            DataTable collectedbydetails = new DataTable();
            VMSBusinessLayer.RequestDetailsBL objVMSBL = new VMSBusinessLayer.RequestDetailsBL();
            VMSBusinessLayer.UserDetailsBL objVMSUSBL = new VMSBusinessLayer.UserDetailsBL();
            try
            {
                visitorlist = objVMSBL.GetAllVisitorList(parentID);
                ArrayList uniqueRecords = new ArrayList();
                ArrayList duplicateRecords = new ArrayList();
                uniquevisitorlist = visitorlist.Tables[0];
                //// Check if records is already added to UniqueRecords otherwise,
                //// Add the records to DuplicateRecords
                foreach (DataRow drow in uniquevisitorlist.Rows)
                {
                    if (uniqueRecords.Contains(drow["VisitorID"]))
                    {
                        duplicateRecords.Add(drow);
                    }
                    else
                    {
                        uniqueRecords.Add(drow["VisitorID"]);
                    }
                }

                //// Remove duplicate rows from DataTable added to DuplicateRecords
                foreach (DataRow drow in duplicateRecords)
                {
                    uniquevisitorlist.Rows.Remove(drow);
                }

                if (!string.IsNullOrEmpty(uniquevisitorlist.Rows[0]["CollectedBy"].ToString()))
                {
                    collectedbydetails = objVMSUSBL.GetAssociateDetails(uniquevisitorlist.Rows[0]["CollectedBy"].ToString());
                    string collectedby = string.Concat(collectedbydetails.Rows[0]["LastName"], ",", collectedbydetails.Rows[0]["FirstName"], "(", uniquevisitorlist.Rows[0]["CollectedBy"], ")");
                    uniquevisitorlist.Columns.Add("CollectedByName");

                    foreach (DataRow dr in uniquevisitorlist.Rows)
                    {
                        dr["CollectedByName"] = collectedby;
                    }
                }

                visitorslist = JsonConvert.SerializeObject(uniquevisitorlist, Formatting.Indented);
                ////}
            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return visitorslist;
        }

        /// <summary>
        /// notify click
        /// </summary>
        /// <param name="parentid">parent id</param>
        /// <param name="securityid">security id</param>
        /// <returns>return success</returns>
        [WebMethod]
        public static string NotifyClick(string parentid, string securityid)
        {
            MailNotification objMailNotofication = new MailNotification();
            VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
            VMSBusinessLayer.UserDetailsBL associateDetails = new VMSBusinessLayer.UserDetailsBL();
            int str = Convert.ToInt32(parentid); ////parentreferenceID is passed here
            string visitorsname = string.Empty;
            DataTable clientrequestdetails = new DataTable();
            int securityID = Convert.ToInt32(securityid);
            clientrequestdetails = requestDetails.GetclientdetailswithParentrefernceID(str);
            DataSet securitycity = new DataSet();
            securitycity = requestDetails.GetSecurityCity(securityID.ToString());
            string loginfacility = securitycity.Tables[0].Rows[0]["CountryCityFacility"].ToString();

            ////StringWriter swriter = new StringWriter();
            ////HtmlTextWriter writer = new HtmlTextWriter(swriter);
            string visitors = string.Empty;
            string hostid = string.Empty;
            string hostname = string.Empty;
            DateTime fromdate;
            DateTime todate;
            string clientfromdate = string.Empty;
            string clienttodate = string.Empty;
            string summary = string.Empty;
            string summaryJSON = string.Empty;
            string contentJSON = string.Empty;
            string templateID = string.Empty;
            string content = string.Empty;
            string title = string.Empty;
            string summaryJSONreq = string.Empty;
            string contentJSONreq = string.Empty;
            string summaryJSONotp = string.Empty;
            string contentJSONotp = string.Empty;
            string visitplan = string.Empty;
            string sourcefacility = string.Empty;
            string parentHost = string.Empty;
            DataTable requestorcrsname = new DataTable();
            DataTable hostCRSname = new DataTable();
            string requestorname = string.Empty;
            DateTime fromdate1;
            DateTime todate1;
            ArrayList hostlist = new ArrayList();
            ArrayList namelist = new ArrayList();
            ArrayList visitlist = new ArrayList();

            string header = @"<tr><td width=" + "'10%'" + ">" + "<b>Name</b>" + "</td>" +
                            @"<td width=" + "'10%'" + ">" + "<b>Company</b>" + "</td>" +
                            @"<td width=" + "'10%'" + ">" + "<b>Mobile Number</b>" + "</td>" +
                            @"<td width=" + "'10%'" + ">" + "<b>Access Card Number</b>" + "</td></tr>";
            if (clientrequestdetails.Rows.Count > 0)
            {
                DataView view = new DataView(clientrequestdetails);
                DataTable distinctValues = view.ToTable(true, "FromDate", "ToDate", "FromTime", "ToTime", "Facility", "HostID");
                DataTable distinctvisitorname = view.ToTable(true, "VisitorID");

                for (var i = 0; i < clientrequestdetails.Rows.Count; i++)
                {
                    if (!namelist.Contains(clientrequestdetails.Rows[i]["VisitorID"].ToString()))
                    {
                        visitors = visitors + @"<tr><td width= " + "'10%'" + ">" + clientrequestdetails.Rows[i]["Name"].ToString() +
                               @"</td><td width = " + "'10%'" + ">" + clientrequestdetails.Rows[i]["Company"].ToString() +
                               @"</td><td width = " + "'10%'" + ">" + clientrequestdetails.Rows[i]["MobileNo"].ToString() +
                               @"</td><td width = " + "'10%'" + ">" + clientrequestdetails.Rows[i]["AccessCardNumber"].ToString() +
                               @"</td></tr>";
                    }

                    namelist.Add(clientrequestdetails.Rows[i]["VisitorID"].ToString());

                }

                for (var z = 0; z < distinctValues.Rows.Count; z++)
                {

                    clientfromdate = string.Concat(distinctValues.Rows[z]["FromDate"].ToString(), " ", distinctValues.Rows[z]["FromTime"].ToString());
                    clienttodate = string.Concat(distinctValues.Rows[z]["ToDate"].ToString(), " ", distinctValues.Rows[z]["ToTime"].ToString());

                }
                visitors = header + visitors;
                parentHost = clientrequestdetails.Rows[0]["HostID"].ToString();
                sourcefacility = clientrequestdetails.Rows[0]["Facility"].ToString();
                int visitorcount = distinctvisitorname.Rows.Count;
                int facilitycount = distinctValues.Rows.Count;

                for (var i = 0; i < clientrequestdetails.Rows.Count; i++)
                {
                    if (!hostlist.Contains(clientrequestdetails.Rows[i]["HostID"].ToString()))
                    {

                        if (clientrequestdetails.Rows[i]["HostID"].ToString().Contains('('))
                        {
                            string[] host = clientrequestdetails.Rows[i]["HostID"].ToString().Split('(');
                            hostid = host[1].Substring(0, host[1].Length - 1);
                            hostname = host[0];

                        }
                        else
                        {
                            hostid = clientrequestdetails.Rows[i]["HostID"].ToString();

                        }

                        string requestorid = clientrequestdetails.Rows[i]["HostAssociateID"].ToString();

                        if (hostid == clientrequestdetails.Rows[i]["HostAssociateID"].ToString())
                        {
                            if (visitorcount == 1 && facilitycount == 1)
                            {
                                summary = "Your client visitor request summary";

                                summaryJSONotp = "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[i]["Name"].ToString() + "\"}," +
                                                                              "\"Organization\":{\"value\":\"" + clientrequestdetails.Rows[i]["Company"].ToString() + "\"}," +

                                                                              "\"Express check-in code\":{\"value\":\"" + clientrequestdetails.Rows[i]["RequestID"].ToString() + "\"}," +
                                                                              "\"Access Card Number\":{\"value\":\"" + clientrequestdetails.Rows[i]["AccessCardNumber"].ToString() + "\"}," +

                                                                              "\"Card collection/visiting  facility\":{\"value\":\"" + clientrequestdetails.Rows[i]["Facility"].ToString() + "\"}," +
                                                                              "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}}";

                                templateID = "1";
                                content = string.Empty;
                                title = "Visitor Request";
                            }
                            else if (visitorcount > 1 && facilitycount == 1)
                            {
                                summary = "Your client visitor request summary.";


                                summaryJSONotp = "{\"Visit Location\":{\"value\":\"" + distinctValues.Rows[0]["Facility"].ToString() + "\"}," +
                                    "\"Express check-in code\":{\"value\":\"" + clientrequestdetails.Rows[0]["ParentReferenceId"].ToString() + "\"}," +
                                    "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}}";

                                templateID = "7";
                                content = "Details of your client visitors";
                                title = "Visitor Request";

                                namelist.Clear();

                                contentJSONotp = "[";
                                for (var j = 0; j < clientrequestdetails.Rows.Count; j++)
                                {
                                    if (!namelist.Contains(clientrequestdetails.Rows[j]["VisitorID"].ToString()))
                                    {

                                        contentJSONotp = contentJSONotp + "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[j]["Name"].ToString() + "\"}," +
                                            "\"Organization\":{\"value\":\"" + clientrequestdetails.Rows[j]["Company"].ToString() + "\"}," +

                                             "\"Visitor Card Number\":{\"value\":\"" + clientrequestdetails.Rows[0]["AccessCardNumber"].ToString() + "\"}}," +
                                              "\"Action-Call\":{\"value\":\"" + clientrequestdetails.Rows[0]["MobileNo"].ToString() + "\"}}";
                                    }

                                    namelist.Add(clientrequestdetails.Rows[j]["VisitorID"].ToString());
                                }

                                contentJSONotp = contentJSONotp.Remove(contentJSONotp.Length - 1) + "]";
                            }

                            else if (visitorcount == 1 && facilitycount > 1)
                            {
                                summary = "Your client visitor request summary.";
                                summaryJSONotp = "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[0]["Name"].ToString() + "\"}," +
                                    "\"Visitor Organization \":{\"value\":\"" + clientrequestdetails.Rows[0]["Company"].ToString() + "\"}," +

                                     "\"Visitor Card Number\":{\"value\":\"" + clientrequestdetails.Rows[0]["AccessCardNumber"].ToString() + "\"}," +
                                    "\"Card collection facility \":{\"value\":\"" + sourcefacility + "\"}," +
                                  "\"Express check-in code\":{\"value\":\"" + clientrequestdetails.Rows[0]["ParentReferenceId"].ToString() + "\"}}" +
                                   "\"Action-Call\":{\"value\":\"" + clientrequestdetails.Rows[0]["MobileNo"].ToString() + "\"}}";
                                templateID = "7";
                                content = "Visit Itinerary";
                                title = "Visitor Request";
                                contentJSONotp = "[";
                                for (var z = 0; z < distinctValues.Rows.Count; z++)
                                {

                                    clientfromdate = string.Concat(distinctValues.Rows[z]["FromDate"].ToString(), " ", distinctValues.Rows[z]["FromTime"].ToString());
                                    clienttodate = string.Concat(distinctValues.Rows[z]["ToDate"].ToString(), " ", distinctValues.Rows[z]["ToTime"].ToString());

                                    contentJSONotp = contentJSONotp + "{\"Visit Facility \":{\"value\":\"" + distinctValues.Rows[z]["Facility"].ToString() + "\"}," +
                                           "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}},";
                                }
                                contentJSONotp = contentJSONotp.Remove(contentJSONotp.Length - 1) + "]";
                            }

                            else if (visitorcount > 1 && facilitycount > 1)
                            {
                                summary = "Your client visitor request summary.";

                                summaryJSONotp = "{\"Express check-in code \":{\"value\":\"" + clientrequestdetails.Rows[0]["ParentReferenceId"].ToString() + "\"}," +
                                    "\"Badge collection facility \":{\"value\":\"" + sourcefacility + "\"}," +
                                    "\"Total Vcards to be collected \":{\"value\":\"" + visitorcount.ToString() + "\"}}";
                                templateID = "7";
                                content = "Visitor details";
                                title = "Visitor Request";

                                ////contentJSON = "[{\"Header\":{\"value\":\"" + "Visit itinerary " + "\",\"highlight\":\"1\"}},";

                                contentJSONotp = "[{\"Header\":{\"value\":\"" + "Visit itinerary" + "\"}},";
                                for (var z = 0; z < distinctValues.Rows.Count; z++)
                                {
                                    clientfromdate = string.Concat(distinctValues.Rows[z]["FromDate"].ToString(), " ", distinctValues.Rows[z]["FromTime"].ToString());
                                    clienttodate = string.Concat(distinctValues.Rows[z]["ToDate"].ToString(), " ", distinctValues.Rows[z]["ToTime"].ToString());

                                    contentJSONotp = contentJSONotp + "{\"Visit Facility \":{\"value\":\"" + distinctValues.Rows[z]["Facility"].ToString() + "\"}," +

                                          "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}},";

                                }

                                namelist.Clear();
                                ////contentJSON = contentJSON + "{\"Header\":{\"value\":\"" + "Visitor details" + "\",\"highlight\":\"1\"}},";

                                contentJSONotp = contentJSONotp + "{\"Header\":{\"value\":\"" + "Visitor details" + "\"}},";
                                for (var j = 0; j < clientrequestdetails.Rows.Count; j++)
                                {
                                    if (!namelist.Contains(clientrequestdetails.Rows[j]["VisitorID"].ToString()))
                                    {

                                        contentJSONotp = contentJSONotp + "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[j]["Name"].ToString() + "\"}," +
                                           "\"Organization\":{\"value\":\"" + clientrequestdetails.Rows[j]["Company"].ToString() + "\"}," +
                                           "\"Visitor Card Number\":{\"value\":\"" + clientrequestdetails.Rows[0]["AccessCardNumber"].ToString() + "\"}},";

                                    }

                                    namelist.Add(clientrequestdetails.Rows[j]["VisitorID"].ToString());
                                }

                                contentJSONotp = contentJSONotp.Remove(contentJSONotp.Length - 1) + "]";
                            }
                        }

                        else
                        {
                            requestorcrsname = associateDetails.GetAssociateDetails(requestorid);
                            requestorname = requestorcrsname.Rows[0]["FirstName"].ToString() + "(" + requestorid + ")";
                            if (visitorcount == 1 && facilitycount == 1)
                            {
                                summary = "Your client visitor request summary.";


                                summaryJSONotp = "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[0]["Name"].ToString() + "\"}," +
                                    "\"Organization\":{\"value\":\"" + clientrequestdetails.Rows[0]["Company"].ToString() + "\"}," +
                                    "\"Card collection/visiting  facility\":{\"value\":\"" + distinctValues.Rows[0]["Facility"].ToString() + "\"}," +
                                    "\"Express check-in code\":{\"value\":\"" + clientrequestdetails.Rows[0]["ParentReferenceId"].ToString() + "\"}," +
                                "\"Visitor Smart Card #\":{\"value\":\"" + clientrequestdetails.Rows[0]["AccessCardNumber"].ToString() + "\"}," +
                                "\"Request Raised by\":{\"value\":\"" + requestorname + "\"}," +
                                    "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}}";

                                templateID = "1";
                                content = string.Empty;
                                title = "Visitor Request";
                            }
                            else if (visitorcount > 1 && facilitycount == 1)
                            {
                                summary = "Your client visitor request summary.";

                                summaryJSONotp = "{\"Card collection/ visiting facility \":{\"value\":\"" + distinctValues.Rows[0]["Facility"].ToString() + "\"}," +
                                    "\"Express check-in code\":{\"value\":\"" + clientrequestdetails.Rows[0]["ParentReferenceId"].ToString() + "\"}," +
                                "\"Request Submitted By\":{\"value\":\"" + requestorname + "\"}," +
                                    "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}}";


                                templateID = "7";
                                content = "Details of your client visitors";
                                title = "Visitor Request";

                                namelist.Clear();

                                contentJSONotp = "[";
                                for (var j = 0; j < clientrequestdetails.Rows.Count; j++)
                                {
                                    if (!namelist.Contains(clientrequestdetails.Rows[j]["VisitorID"].ToString()))
                                    {


                                        contentJSONotp = contentJSONotp + "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[j]["Name"].ToString() + "\"}," +
                                            "\"Organization\":{\"value\":\"" + clientrequestdetails.Rows[j]["Company"].ToString() + "\"}," +
                                            "\"Card Details\":{\"value\":\"" + clientrequestdetails.Rows[j]["AccessCardNumber"].ToString() + "\"}},";
                                    }

                                    namelist.Add(clientrequestdetails.Rows[j]["VisitorID"].ToString());
                                }

                                contentJSONotp = contentJSONotp.Remove(contentJSONotp.Length - 1) + "]";
                            }
                            else if (visitorcount == 1 && facilitycount > 1)
                            {
                                summary = "Your client visitor request summary.";


                                summaryJSONotp = "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[0]["Name"].ToString() + "\"}," +
                                   "\"Visitor Organization \":{\"value\":\"" + clientrequestdetails.Rows[0]["Company"].ToString() + "\"}," +

                                   "\"Express check-in code\":{\"value\":\"" + clientrequestdetails.Rows[0]["ParentReferenceId"].ToString() + "\"}," +
                                "\"Visitor Card Number\":{\"value\":\"" + clientrequestdetails.Rows[0]["AccessCardNumber"].ToString() + "\"}," +
                                "\"Card collection facility\":{\"value\":\"" + distinctValues.Rows[0]["Facility"].ToString() + "\"}}" +
                                "\"Action-Call\":{\"value\":\"" + clientrequestdetails.Rows[0]["MobileNo"].ToString() + "\"}}";

                                templateID = "7";
                                content = "Visit Itinerary";
                                title = "Visitor Request";

                                contentJSONotp = "[";
                                for (var z = 0; z < distinctValues.Rows.Count; z++)
                                {
                                    clientfromdate = string.Concat(distinctValues.Rows[z]["FromDate"].ToString(), " ", distinctValues.Rows[z]["FromTime"].ToString());
                                    clienttodate = string.Concat(distinctValues.Rows[z]["ToDate"].ToString(), " ", distinctValues.Rows[z]["ToTime"].ToString());


                                    contentJSONotp = contentJSONotp + "{\"Visit Facility \":{\"value\":\"" + distinctValues.Rows[z]["Facility"].ToString() + "\"}," +
                                           "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}," +
                                           "\"Request Submitted by\":{\"value\":\"" + requestorname + "\"}},";
                                }

                                contentJSONotp = contentJSONotp.Remove(contentJSONotp.Length - 1) + "]";
                            }

                            else if (visitorcount > 1 && facilitycount > 1)
                            {
                                summary = "Your client visitor request summary.";


                                summaryJSONotp = "{\"Card collection facility \":{\"value\":\"" + sourcefacility + "\"}" +
                                    "\"Total V Cards \":{\"value\":\"" + visitorcount + "\"}" +
                                    "\"Request Submitted by \":{\"value\":\"" + requestorname + "\"}" +
                                    "\"Express check-in code\":{\"value\":\"" + clientrequestdetails.Rows[0]["ParentReferenceId"].ToString() + "\"}}";
                                templateID = "7";
                                content = "Visitor details";
                                title = "Visitor Request";

                                ////contentJSON = "[{\"Header\":{\"value\":\"" + "Visit itinerary " + "\",\"highlight\":\"1\"}},";

                                contentJSONotp = "[{\"Header\":{\"value\":\"" + "Visit itinerary" + "\"}},";
                                for (var z = 0; z < distinctValues.Rows.Count; z++)
                                {
                                    clientfromdate = string.Concat(distinctValues.Rows[z]["FromDate"].ToString(), " ", distinctValues.Rows[z]["FromTime"].ToString());
                                    clienttodate = string.Concat(distinctValues.Rows[z]["ToDate"].ToString(), " ", distinctValues.Rows[z]["ToTime"].ToString());


                                    contentJSONotp = contentJSONotp + "{\"Visit Facility \":{\"value\":\"" + distinctValues.Rows[z]["Facility"].ToString() + "\"}," +
                                          "\"Meeting Schedule\":{\"value\":\"" + clientfromdate + " to " + clienttodate + "\",\"icon\":\"\\uD83D\\uDCC5\"}},";
                                }

                                namelist.Clear();
                                ////contentJSON = contentJSON + "{\"Header\":{\"value\":\"" + "Visitor details" + "\",\"highlight\":\"1\"}},";

                                contentJSONotp = contentJSONotp + "{\"Header\":{\"value\":\"" + "Visitor details" + "\"}},";
                                for (var j = 0; j < clientrequestdetails.Rows.Count; j++)
                                {
                                    if (!namelist.Contains(clientrequestdetails.Rows[j]["VisitorID"].ToString()))
                                    {

                                        contentJSONotp = contentJSONotp + "{\"Visitor Name \":{\"value\":\"" + clientrequestdetails.Rows[j]["Name"].ToString() + "\"}," +
                                           "\"Organization\":{\"value\":\"" + clientrequestdetails.Rows[j]["Company"].ToString() + "\"}," +
                                           "\"Card Details\":{\"value\":\"" + clientrequestdetails.Rows[j]["AccessCardNumber"].ToString() + "\"}},";

                                    }

                                    namelist.Add(clientrequestdetails.Rows[j]["VisitorID"].ToString());
                                }

                                contentJSONotp = contentJSONotp.Remove(contentJSONotp.Length - 1) + "]";


                            }
                        }
                    }
                }
                ////string[] host = clientrequestdetails.Rows[0]["HostID"].ToString().Split('(');


                ////string hostid = host[1].Substring(0, host[1].Length - 1);
                ////string hostname = host[0];
                requestDetails.UpdateNotifcationtoHost(str, Convert.ToInt32(securityID));
                objMailNotofication.CollectNotificationToHost(
                    hostid,
                    hostname,
                    visitors,
                    loginfacility, ////clientrequestdetails.Rows[0]["Facility"].ToString(),
                    clientrequestdetails.Rows[0]["ParentReferenceId"].ToString(),
                    clientrequestdetails.Rows[0]["HostAssociateID"].ToString(),
                    summary, summaryJSONotp, contentJSONotp, templateID, content, title);
            }

            return "Clientvisit.aspx?requeststatus=To Be Processed";

        }


        /// <summary>
        /// Get Dummy Equipment Details
        /// </summary>
        /// <param name="visitorID">visitor ID</param>
        /// <returns>string value</returns>
        [WebMethod]
        public static string GetEquipmentDetails(string visitorID)
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
            string jsonString = string.Empty;
            try
            {
                DataTable dtdata = requestDetails.GetEquipmentDetails(Convert.ToInt32(visitorID));
                DataTable tableSearchDetails = new DataTable();
                DataRow dr;
                tableSearchDetails.Columns.Add("EquipmentType");
                tableSearchDetails.Columns.Add("EquipmentMake");
                tableSearchDetails.Columns.Add("EquipmentSerial");
                tableSearchDetails.Columns.Add("EquipmentModel");
                for (int i = 0; i < dtdata.Rows.Count; i++)
                {
                    dr = tableSearchDetails.NewRow();
                    dr["EquipmentType"] = dtdata.Rows[i].ItemArray[0];
                    dr["EquipmentMake"] = dtdata.Rows[i].ItemArray[1];
                    dr["EquipmentSerial"] = dtdata.Rows[i].ItemArray[3];
                    dr["EquipmentModel"] = dtdata.Rows[i].ItemArray[2];
                    tableSearchDetails.Rows.Add(dr);
                }

                jsonString = JsonConvert.SerializeObject(tableSearchDetails);
            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return jsonString;
        }

        /// <summary>
        /// Get Dummy Equipment Details
        /// </summary>
        /// <param name="parentID">parent ID</param>
        /// <param name="visitorID">visitor ID</param>
        /// <returns>string value</returns>
        [WebMethod]
        public static string GetVisitDetails(string parentID, string visitorID)
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
            string jsonString = string.Empty;
            DataTable uniquevisitlist = new DataTable();
            try
            {
                DataTable dtvisitdetails = requestDetails.GetVisitDetails(Convert.ToInt32(parentID), Convert.ToInt32(visitorID));

                ArrayList uniqueRecords = new ArrayList();
                ArrayList duplicateRecords = new ArrayList();
                uniquevisitlist = dtvisitdetails;
                //// Check if records is already added to UniqueRecords otherwise,
                //// Add the records to DuplicateRecords
                foreach (DataRow drow in uniquevisitlist.Rows)
                {
                    if (uniqueRecords.Contains(drow["requestid"]))
                    {
                        duplicateRecords.Add(drow);
                    }
                    else
                    {
                        uniqueRecords.Add(drow["requestid"]);
                    }
                }

                dtvisitdetails.Columns.Add("finalVisitDate");

                for (int i = 0; i < uniqueRecords.Count; i++)
                {
                    string visitdate = string.Empty;
                    for (int j = 0; j < dtvisitdetails.Rows.Count; j++)
                    {
                        if (uniqueRecords[i].ToString() == dtvisitdetails.Rows[j]["requestid"].ToString())
                        {
                            visitdate = visitdate + dtvisitdetails.Rows[j]["VisitDate"] + ',';
                        }
                    }

                    visitdate = visitdate.Substring(0, visitdate.Length - 1);
                    foreach (DataRow dr in dtvisitdetails.Rows)
                    {
                        if (dr["requestid"].ToString() == uniqueRecords[i].ToString())
                        {
                            dr["finalVisitDate"] = visitdate;
                        }
                    }
                }

                //// Remove duplicate rows from DataTable added to DuplicateRecords
                foreach (DataRow drow in duplicateRecords)
                {
                    uniquevisitlist.Rows.Remove(drow);
                }

                jsonString = JsonConvert.SerializeObject(uniquevisitlist);
            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return jsonString;
        }

        /// <summary>
        /// Get Dummy Equipment Details
        /// </summary>
        /// <param name="parentID">parent ID</param>
        /// <param name="visitorID">visitor ID</param>
        /// <returns>string value</returns>
        [WebMethod]
        public static string GetLogDetails(string parentID, string visitorID)
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
            string jsonString = string.Empty;
            try
            {
                DataTable dtlogdetails = requestDetails.GetLogDetails(Convert.ToInt32(parentID), Convert.ToInt32(visitorID));
                DataTable uniquevisitorlist = new DataTable();
                ArrayList uniqueRecords = new ArrayList();
                ArrayList duplicateRecords = new ArrayList();
                uniquevisitorlist = dtlogdetails;
                //// Check if records is already added to UniqueRecords otherwise,
                //// Add the records to DuplicateRecords
                foreach (DataRow drow in uniquevisitorlist.Rows)
                {
                    if (uniqueRecords.Contains(drow["AccessCardNumber"]))
                    {
                        duplicateRecords.Add(drow);
                    }
                    else
                    {
                        uniqueRecords.Add(drow["AccessCardNumber"]);
                    }
                }

                //// Remove duplicate rows from DataTable added to DuplicateRecords
                foreach (DataRow drow in duplicateRecords)
                {
                    uniquevisitorlist.Rows.Remove(drow);
                }

                jsonString = JsonConvert.SerializeObject(uniquevisitorlist);
            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return jsonString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportedby"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Checkvalidassociate(string reportedby)
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.UserDetailsBL requestDetails = new VMSBusinessLayer.UserDetailsBL();
            string jsonString = string.Empty;
            try
            {
                DataTable reportedbydetails = requestDetails.GetAssociateDetails(reportedby);
                jsonString = JsonConvert.SerializeObject(reportedbydetails);
            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return jsonString;
        }

        /// <summary>
        /// Get Dummy Equipment Details
        /// </summary>
        /// <param name="parentid">parent id</param>
        /// <param name="requestid">request id</param>
        /// <param name="securityid">security id</param>
        /// <param name="selectedvisitorid">selected visitor id</param>
        /// <param name="selectedfacility">selected facility</param>
        /// <param name="reportedby">reported by</param>
        /// <returns>string value</returns>
        [WebMethod]
        public static string ReturnandCheckout(string parentid, string requestid, string securityid, string selectedvisitorid, string selectedfacility, string reportedby)
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
            DataSet dtbadgestatus = new DataSet();
            string badgestatusclient = string.Empty;
            string reqstatus = string.Empty;
            string jsonString = string.Empty;
            try
            {
                string[] selectedvisitorids = selectedvisitorid.Split(',');
                ////string[] unselectedvisitorids = unselectedvisitorid.Split(',');
                foreach (var req in selectedvisitorids)
                {
                    if (!string.IsNullOrEmpty(req))
                    {
                        badgestatusclient = "Returned";
                        reqstatus = "Out";
                        dtbadgestatus = requestDetails.UpdatebdgestatusClients(parentid, badgestatusclient, req, reqstatus, string.Empty, securityid, selectedfacility, reportedby, "null");
                    }
                }

                jsonString = JsonConvert.SerializeObject(dtbadgestatus.Tables[0]);
                ////foreach (var req in unselectedvisitorids)
                ////    {
                ////    if (!string.IsNullOrEmpty(req))
                ////        {
                ////        badgestatusclient = "Lost";
                ////        reqstatus = "Out";
                ////        requestDetails.UpdatebdgestatusClients(parentid, badgestatusclient, req, reqstatus, string.Empty, securityid);
                ////        }
                ////    }
            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return jsonString;
        }

        /// <summary>
        /// Get Dummy Equipment Details
        /// </summary>
        /// <param name="parentid">parent id</param>
        /// <param name="requestid">request id</param>
        /// <param name="securityid">security id</param>
        /// <param name="selectedvisitorid">selected visitor id</param>
        /// <param name="selectedfacility">selected facility</param>
        /// <param name="reportedby">reported by</param>
        /// <param name="reportedon">reported on</param>
        /// <returns>string value</returns>
        [WebMethod]
        public static string LostandCheckout(string parentid, string requestid, string securityid, string selectedvisitorid, string lostvcards, string selectedfacility, string reportedby, string reportedon)
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
            DataSet dtbadgestatus = new DataSet();

            string badgestatusclient = string.Empty;
            string reqstatus = string.Empty;
            string jsonString = "Error Occured";
            //bool responseStatus = false;

            IList<VisitorCard> vcardslist = new List<VisitorCard>();
            AccessCard_EApprovalClient client = new AccessCard_EApprovalClient();
            string[] selectedVcards = lostvcards.Split(',');
            bool responseStatus = false;
            try
            {
                foreach (var card in selectedVcards)
                {
                    VisitorCard vcard = new VisitorCard();
                    vcard.CardSlNumber = card;
                    vcard.CardType = "Client Visitor";
                    vcard.ReasonCode = "3";
                    vcardslist.Add(vcard);
                }
                VisitorCard[] vcardsdetails = vcardslist.ToArray();
                string[] selectedvisitorids = selectedvisitorid.Split(',');

                //// service call to update the status to  access card repository.
                responseStatus = page.UpdateLostOrDamagedCardStatus(lostvcards, "3");

                if (responseStatus)
                {
                    foreach (var req in selectedvisitorids)
                    {
                        if (!string.IsNullOrEmpty(req))
                        {
                            badgestatusclient = "Lost";
                            reqstatus = "Out";

                            dtbadgestatus = requestDetails.UpdatebdgestatusClients(parentid, badgestatusclient, req, reqstatus, string.Empty, securityid, selectedfacility, reportedby, reportedon);
                        }


                    }
                    jsonString = JsonConvert.SerializeObject(dtbadgestatus.Tables[0]);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return jsonString;
        }

        /// <summary>
        /// methos to update the status of damaged or lost card in access card repository
        /// </summary>
        /// <param name="vcards">comma seperated vcard list</param>
        /// <param name="reasonCode">3 for lost card and 8 for damaged card</param>
        /// <returns></returns>
        [WebMethod]
        public bool UpdateLostOrDamagedCardStatus(string vcards, string reasonCode)
        {

            IList<VisitorCard> vcardslist = new List<VisitorCard>();
            AccessCard_EApprovalClient client = new AccessCard_EApprovalClient("BasicHttpBinding_IAccessCard_EApproval");
            string[] selectedVcards = vcards.Split(',');
            bool responseStatus = false;
            try
            {
                foreach (var card in selectedVcards)
                {
                    VisitorCard vcard = new VisitorCard();
                    vcard.CardSlNumber = card;
                    vcard.CardType = "Client Visitor";
                    vcard.ReasonCode = reasonCode;
                    vcardslist.Add(vcard);
                }
                VisitorCard[] vcardsdetails = vcardslist.ToArray();
                responseStatus = client.UpdateReasonsForVisitorCards(vcardsdetails);
                return responseStatus;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// update details
        /// </summary>
        /// <param name="securityid">security id</param>
        /// <returns>success return</returns>
        [WebMethod]
        public static string LoadFacilityList(string securityid)
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.RequestDetailsBL objVMBL = new VMSBusinessLayer.RequestDetailsBL();
            DataTable facilitylist = new DataTable();
            string jsonString = string.Empty;
            facilitylist = objVMBL.LoadfacilitylistBL(Convert.ToInt32(securityid));
            List<ListItem> faclitylist = new List<ListItem>();
            foreach (DataRow dr in facilitylist.Rows)
            {
                faclitylist.Add(new ListItem
                {
                    Value = dr["CountryCityFacility"].ToString(),
                    Text = dr["CountryCityFacility"].ToString()
                });
            }

            jsonString = JsonConvert.SerializeObject(faclitylist);
            return jsonString;
        }

        /// <summary>
        /// update details
        /// </summary>
        /// <returns>reissue reason list</returns>
        [WebMethod]
        public static string LoadReissuelist()
        {
            ClientPage page = new ClientPage();
            VMSBusinessLayer.RequestDetailsBL objVMBL = new VMSBusinessLayer.RequestDetailsBL();
            DataTable reissuelist = new DataTable();
            string jsonString = string.Empty;
            reissuelist = objVMBL.LoadReissuelistBL();
            List<ListItem> relist = new List<ListItem>();
            foreach (DataRow dr in reissuelist.Rows)
            {
                relist.Add(new ListItem
                {
                    Value = dr["ReasonDescription"].ToString(),
                    Text = dr["ReasonDescription"].ToString()
                });
            }

            jsonString = JsonConvert.SerializeObject(relist);
            return jsonString;
        }

        /// <summary>
        /// update details
        /// </summary>
        /// <param name="visitorname">visitor name</param>
        /// <param name="accessnumber">access number</param>
        /// <param name="requestid">request id</param>
        /// <param name="visitorid">visitor id</param>
        /// <returns>success return</returns>
        [WebMethod]
        public static string UpdateCardDetails(string visitorname, string accessnumber, string vcardnumber, string requestid, string visitorid)
        {
            VMSBusinessLayer.RequestDetailsBL objVMBL = new VMSBusinessLayer.RequestDetailsBL();

            if (accessnumber == null || accessnumber == string.Empty)
            {
                accessnumber = "0";
            }

            objVMBL.UpdateCardDetails(Convert.ToInt32(requestid), Convert.ToInt32(visitorid), visitorname, Convert.ToInt32(accessnumber), vcardnumber);
            return "success";

        }

        /// <summary>
        /// update details
        /// </summary>
        /// <param name="visitorname">visitor name</param>
        /// <param name="accessnumber">access card number</param>
        /// <param name="requestid">request id</param>
        /// <param name="visitorid">visitor id</param>
        /// <param name="parentid">parent id</param>
        /// <param name="reissuedby">issued by</param>
        /// <param name="recollectedby">collected by</param>
        /// <param name="reissuereason">issue reason</param>
        /// <param name="selectedfacility">selected facility</param>
        /// <returns>success return</returns>
        [WebMethod]
        public static string Reissuecards(string visitorname, string accessnumber, string vcardnumber, string Newvcardnumber, string requestid, string visitorid, string parentid, string reissuedby, string recollectedby, string reissuereason, string selectedfacility)
        {
            string summary = string.Empty;
            string summaryJSON = string.Empty;
            string contentJSON = string.Empty;
            string templateID = string.Empty;
            string content = string.Empty;
            string title = string.Empty;
            string hostid = string.Empty;
            string hostname = string.Empty;
            ClientPage page = new ClientPage();
            MailNotification objMailNotofication = new MailNotification();
            string reason = "Lost";
            int str = Convert.ToInt32(parentid);
            VMSBusinessLayer.RequestDetailsBL objVMBL = new VMSBusinessLayer.RequestDetailsBL();
            DataTable clientdtls = objVMBL.GetclientdetailswithParentrefernceID(str);
            if (clientdtls.Rows.Count > 0)
            {
                if (clientdtls.Rows[0]["HostID"].ToString().Contains('('))
                {
                    string[] host = clientdtls.Rows[0]["HostID"].ToString().Split('(');
                    hostid = host[1].Substring(0, host[1].Length - 1);
                    hostname = host[0];

                }
                else
                {
                    hostid = clientdtls.Rows[0]["HostID"].ToString();

                }
            }
            if (reissuereason.ToUpper() == "LOST AND REISSUED")
                reason = "3";
            else
                reason = "8";
            bool responseStatus = page.UpdateLostOrDamagedCardStatus(vcardnumber, reason);
            if (responseStatus)
            {
                objVMBL.ReissuecardsBL(visitorname, accessnumber, Newvcardnumber, requestid, visitorid, parentid, reissuedby, recollectedby, reissuereason, selectedfacility);
                DataTable clientrequestdetails = objVMBL.GetClientDetailsforInfoCard(Convert.ToInt32(requestid), Convert.ToInt32(visitorid), Convert.ToInt32(parentid));
                if (clientrequestdetails.Rows.Count > 0)
                {
                    summary = "Visitor badge has been reissued to your visitor";

                    summaryJSON = "{\"Visitor Name \":{\"value\":\"" + visitorname + "\"}," +
                    "\"Reason for re-issue\":{\"value\":\"" + reissuereason + "\"}," +
                     "\"Old Badge Number\":{\"value\":\"" + vcardnumber + "\"}," +
                      "\"New Badge Number\":{\"value\":\"" + Newvcardnumber + "\"}," +
                      "\"Re-issued On\":{\"value\":\"" + clientrequestdetails.Rows[0]["ReportedOn"].ToString() + "\"}," +
                      "\"Re-issued at\":{\"value\":\"" + selectedfacility + "\",\"icon\":\"\\uD83D\\uDCC5\"}}";
                    templateID = "1";
                    content = string.Empty;
                    title = "Visitor Request";
                }
                objMailNotofication.SendNotificationToHostICard(hostid, parentid, summary, summaryJSON, templateID, content, title);
                return "success";

            }
            else
                return "error";
        }

        /// <summary>
        /// update details
        /// </summary>
        /// <param name="accesscards">access cards</param>
        /// <param name="parentid">parent id</param>
        /// <returns>success return</returns>
        [WebMethod]
        public static string Checkaccesscard(string accesscards, string parentid, string isReissue = "0")
        {
            ClientPage page = new ClientPage();

            VMSBusinessLayer.RequestDetailsBL objVMSRD = new VMSBusinessLayer.RequestDetailsBL();
            DataTable accesscardusage = new DataTable();
            ArrayList accesscheck = new ArrayList();
            string jsonString = string.Empty;
            try
            {
                string[] accesscard = accesscards.Split(',');

                foreach (var card in accesscard)
                {
                    if (!string.IsNullOrEmpty(card))
                    {
                        string count = objVMSRD.Get_Accesscardnumber(card).ToString();
                        if (count == "True")
                        {
                            accesscheck.Add(card);
                        }
                    }
                }
                if (isReissue.Equals("1"))
                {
                    foreach (var card in accesscard)
                    {
                        if (!string.IsNullOrEmpty(card))
                        {
                            accesscardusage = objVMSRD.Checkaccesscardusage(Convert.ToInt32(card));
                            foreach (DataRow dr in accesscardusage.Rows)
                            {
                                if (dr["badgestatus"].ToString().Contains("Host Notified") || dr["badgestatus"].ToString().Contains("Updated  VCard") || dr["badgestatus"].ToString().Contains("issued"))
                                {
                                    accesscheck.Add(card);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var card in accesscard)
                    {
                        if (!string.IsNullOrEmpty(card))
                        {
                            accesscardusage = objVMSRD.Checkaccesscardusage(Convert.ToInt32(card));
                            foreach (DataRow dr in accesscardusage.Rows)
                            {
                                if ((parentid != dr["ParentRefernceID"].ToString()) && (dr["badgestatus"].ToString().Contains("Host Notified") || dr["badgestatus"].ToString().Contains("Updated  VCard") || dr["badgestatus"].ToString().Contains("issued")))
                                {
                                    accesscheck.Add(card);
                                }
                            }
                        }
                    }
                }

                jsonString = JsonConvert.SerializeObject(accesscheck);
            }
            catch (Exception ex)
            {
                ExceptionLogger.OneC_ExceptionLogger(ex, page.Page);
            }

            return jsonString;
        }

        /// <summary>
        /// method to check the validity of the Vcard and populate the linked access card number
        /// </summary>
        /// <param name="vcard">the visitor card number</param>
        /// <param name="location">the location of security</param>
        /// <returns></returns>
        [WebMethod]
        public static string GetAccessCardDetailsByVcard(string vcard, string location)
        {
            string cardType = "Client Visitor";
            string result = "Invalid Vcard";
            string jsonresult = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(vcard))
                {
                    AccessCard_EApprovalClient client = new AccessCard_EApprovalClient();
                    var response = client.GetVisitorCardDetails(vcard, cardType);

                    if (!string.IsNullOrEmpty(response.CardSlNumber))
                    {
                        if (response.CardFacility.Split('-')[0].Trim().ToUpper() == location.ToUpper())
                        {
                            if ((response.IsActive == true && string.IsNullOrEmpty(response.CardStatus)))
                            {
                                if (string.IsNullOrEmpty(response.AccessCardNumber))
                                    result = "No Access Card Linked";
                                else
                                {
                                    result = response.AccessCardNumber;

                                }
                            }
                        }
                        else
                        {
                            result = "Location mismatch";
                        }
                    }

                }

                jsonresult = JsonConvert.SerializeObject(result);
                return jsonresult;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Get access card no
        /// </summary>
        /// <param name="vcard">visitor card</param>
        /// <returns>access card no</returns>
        [WebMethod]
        public static string GetAccessCardNo(string vcard)
        {
            string cardType = "Client Visitor";
            string accessCard = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(vcard))
                {
                    AccessCard_EApprovalClient client = new AccessCard_EApprovalClient();
                    var response = client.GetVisitorCardDetails(vcard, cardType);
                    accessCard = response.AccessCardNumber;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return accessCard;
        }

        /// <summary>
        /// back click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        protected void Back_Click(object sender, EventArgs e)
        {
            if ((this.hdnbadgestatus.Value.ToUpper() == "UPDATED  VCARD") || (this.hdnbadgestatus.Value.ToUpper() == "VCARD PARTIALLY UPDATED") || (this.hdnbadgestatus.Value.ToUpper() == "HOST NOTIFIED"))
            {
                string TargetPage = "Clientvisit.aspx?requeststatus=To Be Processed";
                try
                {
                    Response.Redirect(TargetPage, true);
                 
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
            else if (this.hdnbadgestatus.Value.ToUpper() == "ISSUED" || this.hdnbadgestatus.Value.ToUpper() == "PARTIALLY CLOSED")
            {
                try
                {
                    Response.Redirect("Clientvisit.aspx?requeststatus=Dispatched", true);
                   
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
            else if (this.hdnbadgestatus.Value.ToUpper() == "RETURNED" || this.hdnbadgestatus.Value.ToUpper() == "LOST")
            {
                try
                {
                    Response.Redirect("Clientvisit.aspx?requeststatus=Out", true);
                   
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
            else
            {
                try
                {
                    Response.Redirect("Clientvisit.aspx?requeststatus=To Be Processed", true);
                    
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }

        /// <summary>
        /// notify click
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">e parameter</param>
        protected void BtnNotifyconfirm_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Clientvisit.aspx", true);
              
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
        }

        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">the e parameter</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString.ToString().Contains("details=") && Request.QueryString.ToString().Contains("status="))
                {
                    VMSBusinessLayer.RequestDetailsBL requestDetails = new VMSBusinessLayer.RequestDetailsBL();
                    DataSet clientrequestdetails = new DataSet();
                    DataTable accesscardlist = new DataTable();
                    int accesscount = 0;
                    string parentid = VMSBusinessLayer.Decrypt(Request.QueryString["details"]);
                    string badgestatus = Request.QueryString["status"].ToString();
                    this.hdnParentID.Value = XSS.HtmlEncode(parentid);
                    this.hdnSecurityID.Value = XSS.HtmlEncode(Session["LoginID"].ToString());
                    this.hdnbadgestatus.Value = badgestatus.ToString();
                    DataSet securitycity = new DataSet();
                    securitycity = requestDetails.GetSecurityCity(Session["LoginID"].ToString());
                    this.hdnloginfacility.Value = securitycity.Tables[0].Rows[0]["CountryCityFacility"].ToString();
                    if (badgestatus.ToUpper() == "UPDATED  VCARD")
                    {
                        clientrequestdetails = requestDetails.GetAllVisitorList(Convert.ToInt32(parentid));
                        accesscardlist = clientrequestdetails.Tables[0];
                        this.divnotes.Visible = true;
                        //// Check if records is already added to UniqueRecords otherwise,
                        //// Add the records to DuplicateRecords
                        foreach (DataRow drow in accesscardlist.Rows)
                        {
                            if (drow["AccessCardNumber"].ToString() == string.Empty || drow["AccessCardNumber"].ToString() == null)
                            {
                                accesscount = accesscount + 1;
                            }
                        }

                        if (accesscount > 0)
                        {
                            this.btnnotify.Disabled = true;
                            this.btnupdate.Disabled = false;
                            this.btnnotify.Style.Add("background-color", "grey");
                        }
                        else
                        {
                            this.btnnotify.Disabled = false;
                            ////this.btnupdate.Disabled = true;
                            ////this.btnupdate.Style.Add("background-color", "grey");
                            this.btnupdate.Disabled = false;
                            this.btnupdate.Style.Add("background-color", "#3188B5");
                        }
                    }
                    else if (badgestatus.ToUpper() == "ACCESS CARD PARTIALLY UPDATED")
                    {
                        this.divnotes.Visible = true;
                        this.btnnotify.Disabled = true;
                        this.btnupdate.Disabled = false;
                        this.btnnotify.Style.Add("background-color", "grey");
                    }
                    else if (badgestatus.ToUpper() == "HOST NOTIFIED")
                    {
                        this.divnotes.Visible = true;
                        this.btnnotify.Disabled = true;
                        this.btnupdate.Disabled = true;
                        this.btnupdate.Style.Add("background-color", "grey");
                        this.btnnotify.Style.Add("background-color", "grey");
                    }
                    else if (badgestatus.ToUpper() == "ISSUED" || badgestatus.ToUpper() == "PARTIALLY CLOSED")
                    {
                        this.divnotes.Visible = false;
                        this.btnnotify.Visible = false;
                        this.btnupdate.Visible = false;
                        ////this.btncheckout.Visible = true;
                        this.btnretcheckout.Visible = true;
                        this.btnlcheckout.Visible = true;
                        this.btnreissue.Visible = true;
                        this.btnreset.Visible = true;
                    }
                    else if (badgestatus.ToUpper() == "RETURNED" || badgestatus.ToUpper() == "LOST")
                    {
                        this.divnotes.Visible = false;
                        this.btnnotify.Visible = false;
                        this.btnupdate.Visible = false;
                        ////this.btncheckout.Visible = false;
                        this.btnretcheckout.Visible = false;
                        this.btnlcheckout.Visible = false;
                        this.btnreissue.Visible = false;
                        this.btnupdate.Style.Add("background-color", "grey");
                        this.btnnotify.Style.Add("background-color", "grey");
                    }
                    else
                    {
                        this.btnnotify.Disabled = true;
                        this.btnupdate.Disabled = false;
                        ////this.btnupdate.Style.Add("background-color", "grey");
                        this.btnnotify.Style.Add("background-color", "grey");
                    }
                }
            }
            else
            {
                string hdncount = this.hdnaccount.Value;
                string hdnnotifyclick = this.hdnnotifyclick.Value;
                ////this.btncheckout.Disabled = true;
                ////this.btncheckout.Style.Add("background-color", "grey");
                if (string.IsNullOrEmpty(hdncount) && string.IsNullOrEmpty(hdnnotifyclick))
                {
                    this.btnnotify.Disabled = true;
                    this.btnupdate.Disabled = false;
                    this.btnupdate.Style.Add("background-color", "#3188B5");
                    this.btnnotify.Style.Add("background-color", "grey");
                }
                else if (string.IsNullOrEmpty(hdncount))
                {
                    this.btnnotify.Disabled = true;
                    this.btnupdate.Disabled = false;
                    this.btnupdate.Style.Add("background-color", "#3188B5");
                    this.btnnotify.Style.Add("background-color", "grey");
                }
                else
                {
                    if (Convert.ToInt32(hdncount) == 0 && Convert.ToInt32(hdnnotifyclick) == 0)
                    {
                        this.btnnotify.Disabled = false;
                        this.btnupdate.Disabled = true;
                        this.btnupdate.Style.Add("background-color", "grey");
                        this.btnnotify.Style.Add("background-color", "#3188B5");
                    }
                    else if (Convert.ToInt32(hdncount) > 0)
                    {
                        this.btnnotify.Disabled = true;
                        this.btnupdate.Disabled = false;
                        this.btnupdate.Style.Add("background-color", "#3188B5");
                        this.btnnotify.Style.Add("background-color", "grey");
                    }
                    else if (Convert.ToInt32(hdnnotifyclick) > 0)
                    {
                        this.btnnotify.Disabled = true;
                        this.btnupdate.Disabled = true;
                        this.btnupdate.Style.Add("background-color", "grey");
                        this.btnnotify.Style.Add("background-color", "grey");
                    }
                }
            }
        }


    }
}
