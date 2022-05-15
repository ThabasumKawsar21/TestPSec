
namespace VMSDataLayer
{
    using AzureSQLHelper;
    using EnterpriseLibConn;
    using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.Linq;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Transactions;
    using VMSBusinessEntity;
    using System.Web;
    
    
    /// <summary>
    /// Data layer
    /// </summary>
    public class VMSDataLayer
    {
        /// <summary>
        /// string connection
        /// </summary>
        /// <param name="vmsConn">object list</param>
        /// <returns>string value</returns>
        private static string vmsConn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
        /// <summary>
        /// Function to log exception to Database
        /// </summary>
        /// <param name="message">messsage</param>
        /// <param name="method">method name</param>
        /// <param name="stackTrace">Stack trace</param>
        public void LogException(string message, string method, string stackTrace)
        {
            string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
            SqlConnection con = new SqlConnection(conn);

            try
            {
                using (con)
                {
                    con.OpenWithMSI(); //// con.Open();  
                    SqlCommand cmd = new SqlCommand("[FAM_LogError]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.Parameters.AddWithValue("@Method", method);
                    cmd.Parameters.AddWithValue("@StackTrace", stackTrace);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con?.Close();
            }
        }
        /// <summary>
        /// Safety Permit Security Searches
        /// </summary>
        /// <param name="strAccessCardNo">Access Card number</param>   
        /// <returns>data set</returns> 
        public bool AccesscardNumberCheck(string strAccessCardNo)
        {
            // SqlConnection sqlConn = null;
            SqlDatabase sqlConn;
            DbCommand dbcmd;
            bool status = false;
            try
            {
                sqlConn = new SqlAzureDatabase(ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                dbcmd = sqlConn.GetStoredProcCommand("IVS_AccesscardumberCheck");
                dbcmd.CommandType = CommandType.StoredProcedure;

                // string strConnectionString = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                // sqlConn = new SqlConnection(strConnectionString);
                // sqlConn.OpenWithMSI(); //// sqlConn.Open();
                // string strRequestInsertSP = "IVS_AccesscardumberCheck";
                // SqlCommand sqlComm = new SqlCommand(strRequestInsertSP, sqlConn);
                // sqlComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbcmd, "@Accesscardnumber", SqlDbType.VarChar, strAccessCardNo);

                // sqlConn.Parameters.Add("@Accesscardnumber", DbType.String).Value = strAccessCardNo;
                // sqlConn.AddInParameter(sqlComm, "Accesscardnumber", SqlDbType.VarChar, Convert.ToInt32(strAccessCardNo));
                // int count = (int)sqlComm.ExecuteNonQuery();
                // return count;
                using (IDataReader dbreader = sqlConn.ExecuteReader(dbcmd))
                {
                    while (dbreader.Read())
                    {
                        if (Convert.ToString(dbreader[0]) == "true")
                        {
                            status = true;
                        }
                    }
                }
                dbcmd.Connection?.Close();
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// access card report
        /// </summary>
        /// <param name="startDate">start Date</param>  
        /// <param name="endDate">end Date</param>  
        /// <param name="city">city value</param>  
        /// <param name="facility">facility value</param>  
        /// <returns>data table</returns> 
        public DataTable AccesscardReport(DateTime startDate, DateTime endDate, string city, string facility)
        {
            DataSet dsreturnSet = new DataSet();
            try
            {
                string sdate = startDate.ToShortDateString();
                string stime = "00:00:00.000";
                string sdt = sdate + " " + stime;
                string edate = endDate.ToShortDateString();
                string etime = "23:59:59.999";
                string edt = edate + " " + etime;
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand("GetOneDayCardDetails");
                                 
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "startdate", SqlDbType.DateTime, sdt);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "enddate", SqlDbType.DateTime, edt);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "LocationName", SqlDbType.VarChar, city);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "FacilityId", SqlDbType.VarChar, facility);
                        dbemployeeInfoComm.CommandTimeout = 1000;
                using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                {
                    dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    
                }
                dbemployeeInfoComm.Connection?.Close();
                return dsreturnSet.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// CheckCardStatus
        /// </summary>
        /// <param name="vCard">Vcard number</param>          
        /// <returns>data table</returns>
        public string CheckCardStatus(string vCard)
        {
            // changed by Priti 22 aug.
            string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
            SqlConnection con = new SqlConnection(conn);
            // SqlConnection con = new SqlConnection("Data Source=CTSINTCOVSEZ;Initial Catalog=AdminRepository;User ID=sezpmo;Password=Sezpm0");
            string result = string.Empty;
            SqlCommand cmd;
            try
            {              
                con.OpenWithMSI(); 
                cmd = new SqlCommand("CheckVCardStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(vCard))
                {

                    cmd.Parameters.Add("@VcardNo", SqlDbType.VarChar, 50).Value = vCard;
                    SqlParameter status = cmd.Parameters.Add("@status", SqlDbType.VarChar, 100);
                    status.Direction = ParameterDirection.Output;
                }


                cmd.ExecuteNonQuery();

                result = (string)cmd.Parameters[1].Value;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con?.Close();
                cmd = null;
            }
        }

        /// <summary>
        /// Get time Zone Info
        /// </summary>
        /// <param name="visitDetailsId">Country value</param>  
        /// <returns>data table</returns> 
        public DataTable GetTimeZoneInfo(string Country)
        {
            DataTable dataTable = new DataTable();
            try
            {

                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                SqlCommand cmd;
                cmd = new SqlCommand("usp_GetTimezonebyfacility_PS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@countryName", SqlDbType.VarChar, 100).Value = Country;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                con?.Close();
                da.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Visitor Card Info
        /// </summary>
        /// <param name="visitDetailsId">facility value</param>  
        /// <returns>data table</returns> 
        public DataTable GetVisitorCardInfo(string visitDetailsId)
        {
            DataTable dataTable = new DataTable();
            try
            {

                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                SqlCommand cmd;
                cmd = new SqlCommand("GetVisitorCardInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@visitdetailsID", SqlDbType.VarChar, 50).Value = visitDetailsId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                con?.Close();
                da.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         /// <summary>
        /// Get Visitor Card Info
        /// </summary>
        /// <param name="visitDetailsId">facility value</param>  
        /// <returns>data table</returns> 
        public DataTable GetVisitorDetailsforMailProcess(int visitDetailsId,string action)
        {
            DataTable dataTable = new DataTable();
            try
            {

                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                SqlCommand cmd;
                if (action == "checkin")
                {
                    cmd = new SqlCommand("GetVisitorCardInfoForCheck_inMailProcess", con);
                }
                else if (action == "checkout")
                {
                    cmd = new SqlCommand("GetVisitorCardInfoForCheckoutMailProcess", con);
                }
                else if (action == "reissue")
                {
                    cmd = new SqlCommand("GetVisitorCardInfoForReissueMailProcess", con);
                }
                else {
                    cmd = new SqlCommand("GetVisitorCardInfoForSurrenderMailProcess", con);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@visitdetailsID", SqlDbType.VarChar, 50).Value = visitDetailsId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                con?.Close();
                da.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ////597397
        /// <summary>
        /// Get Visitor Card Info
        /// </summary>
        /// <param name="visitDetailsId">facility value</param>  
        /// <returns>data table</returns> 
        public DataTable GetClientDetailsforInfoCard(int RequestId, int VisitorId, int ParentId)
        {
            DataTable dataTable = new DataTable();
            try
            {

                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                SqlCommand cmd;
 
               
                    cmd = new SqlCommand("GetClientCardInfoForReissueCard", con);
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestID", SqlDbType.VarChar, 50).Value = RequestId;
                cmd.Parameters.Add("@VisitorID", SqlDbType.VarChar, 50).Value = VisitorId;
                cmd.Parameters.Add("@ParentID", SqlDbType.VarChar, 50).Value = ParentId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                con?.Close();
                da.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ////597397
        /// <summary>
        /// Function to get cardLog details
        /// </summary>
        /// <param name="visitDetailsId"></param>
        /// <returns>cardlog details</returns>
        public DataTable GetCardLog(string visitDetailsId)
        {
            DataTable dataTable = new DataTable();
            try
            {

                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                SqlCommand cmd;
                cmd = new SqlCommand("GetCardLog", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@visitdetailsID", SqlDbType.VarChar, 50).Value = visitDetailsId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                con?.Close();
                da.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Function to get Visitor Schedules
        /// </summary>
        /// <param name="visitDetailsId"></param>
        /// <returns>datatable</returns>
        public DataTable GetVisitorSchedule(string visitDetailsId)
        {
            DataTable dataTable = new DataTable();
            try
            {

                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                SqlCommand cmd;
                cmd = new SqlCommand("GetVisitorSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@visitdetailsID", SqlDbType.VarChar, 50).Value = visitDetailsId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                con?.Close();
                da.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// class for properties
        /// </summary>
        public class PropertiesDC
        {
            /// <summary>
            /// string connection
            /// </summary>
            private VisitorMaster visitorMaster;

            /// <summary>
            /// string connection
            /// </summary>
            private VisitorRequest visitorRequest;

            /// <summary>
            /// string connection
            /// </summary>          
            private List<VisitorEquipment> visitorEquipment;

            /// <summary>
            /// string connection
            /// </summary>
            private VisitorEmergencyContact visitorEmergencyContact;

            /// <summary>
            /// string connection
            /// </summary>
            private VisitorProof visitorProof;

            /// <summary>
            /// string connection
            /// </summary>
            private VisitDetail vistDetail;

            /// <summary>
            /// string connection
            /// </summary>
            private List<tblEquipmentsInCustody> equipmentsInCustody;
            #region properties

            /// <summary>
            /// Gets or sets property Identity detail.
            /// </summary> 
            public IdentityDetails IndentityDetailsProperty { get; set; }

            /// <summary>
            /// Gets or sets property Visitor Master Property
            /// </summary> 
            public VisitorMaster VisitorMasterProperty
            {
                get
                {
                    return this.visitorMaster;
                }

                set
                {
                    this.visitorMaster = value;
                }
            }

            /// <summary>
            /// Gets or sets   property Visitor Request Property
            /// </summary> 
            public VisitorRequest VisitorRequestProperty
            {
                get
                {
                    return this.visitorRequest;
                }

                set
                {
                    this.visitorRequest = value;
                }
            }

            /// <summary>
            /// Gets or sets property Visitor Proof Property
            /// </summary> 
            public VisitorProof VisitorProofProperty
            {
                get
                {
                    return this.visitorProof;
                }

                set
                {
                    this.visitorProof = value;
                }
            }

            /// <summary>
            /// Gets or sets property Visitor Equipment Property
            /// </summary> 
            public List<VisitorEquipment> VisitorEquipmentProperty
            {
                get
                {
                    return this.visitorEquipment;
                }

                set
                {
                    this.visitorEquipment = value;
                }
            }

            /// <summary>
            /// Gets or sets property Visitor Emergency Property
            /// </summary> 
            public VisitorEmergencyContact VisitorEmergencyContactProperty
            {
                get
                {
                    return this.visitorEmergencyContact;
                }

                set
                {
                    this.visitorEmergencyContact = value;
                }
            }

            /// <summary>
            /// Gets or sets property Visitor details Property
            /// </summary> 
            public VisitDetail VisitDetailProperty
            {
                get
                {
                    return this.vistDetail;
                }

                set
                {
                    this.vistDetail = value;
                }
            }

            /// <summary>
            /// Gets or sets property Equipment Custody
            /// </summary> 
            public List<tblEquipmentsInCustody> EquipmentCustodyProperty
            {
                get
                {
                    return this.equipmentsInCustody;
                }

                set
                {
                    this.equipmentsInCustody = value;
                }
            }
            #endregion
        }

        /// <summary>
        /// status detail
        /// </summary>
        public class UserDetailsDL
        {
            /// <summary>
            /// Method print status detail
            /// </summary>
            /// <param name="contractorId">object list</param>
            /// <returns>data table</returns>
            public PrintStatusDetails GetLastPrintStatus(int contractorId)
            {
                SqlAzureDatabase sqlConn;
                DbCommand dbcmd;
                PrintStatusDetails printDetails = null;

                try
                {
                    printDetails = new PrintStatusDetails();
                    sqlConn = new SqlAzureDatabase(ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    dbcmd = sqlConn.GetStoredProcCommand("IVS_GetLastPrintStatus");
                    dbcmd.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dbcmd, "@ContractorId", SqlDbType.Int, contractorId);
                    using (IDataReader dbreader = sqlConn.ExecuteReader(dbcmd))
                    {
                        while (dbreader.Read())
                        {
                            printDetails.ContractorId = Convert.ToString(dbreader["ContractorId"]);

                            if (dbreader.IsDBNull(1))
                            {
                                printDetails.ContractorId = string.Empty;
                            }
                            else
                            {
                                printDetails.ContractorId = Convert.ToString(dbreader["ContractorId"]);
                            }

                            if (dbreader.IsDBNull(2))
                            {
                                printDetails.PrintedDate = string.Empty;
                            }
                            else
                            {
                                printDetails.PrintedDate = Convert.ToDateTime(dbreader["PrintedDate"]).ToShortDateString();
                            }

                            if (dbreader.IsDBNull(3))
                            {
                                printDetails.UpdatedBy = string.Empty;
                            }
                            else
                            {
                                printDetails.UpdatedBy = Convert.ToString(dbreader["UpdatedBy"]);
                            }

                            if (dbreader.IsDBNull(4))
                            {
                                printDetails.PrintStatus = string.Empty;
                            }
                            else
                            {
                                printDetails.PrintStatus = Convert.ToString(dbreader["PrintStatus"]);
                            }

                            if (dbreader.IsDBNull(5))
                            {
                                printDetails.LocationName = string.Empty;
                            }
                            else
                            {
                                printDetails.LocationName = Convert.ToString(dbreader["LocationName"]);
                            }

                            if (dbreader.IsDBNull(6))
                            {
                                printDetails.IDGeneratedDate = string.Empty;
                            }
                            else
                            {
                                printDetails.IDGeneratedDate = Convert.ToDateTime(dbreader["BadgeGenerated"]).ToShortDateString();
                            }
                        }
                    }
                    dbcmd.Connection?.Close();
                    return printDetails;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

#pragma warning disable CS0162 // Unreachable code detected
                return printDetails;
#pragma warning restore CS0162 // Unreachable code detected
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="strAssociateID">variable Associate ID</param>
            /// <param name="strInDate">variable name</param>
            /// <param name="strSecurityID">variable Security ID</param>
            public void HostNotificationDetails(string strAssociateID, string strInDate, string strSecurityID)
            {
                string strSql = "HostNotificationDetails";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                    
                       
                            dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                            sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                            sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "InDate", DbType.DateTime, DateTime.Now);
                            sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "MailSentBy", DbType.String, strSecurityID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeDetailsSaveCmd))
                    {
                        sqlConn.ExecuteDataSet(dbemployeeDetailsSaveCmd);
                        
                    }
                    dbemployeeDetailsSaveCmd.Connection.Close();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="requestId">variable name</param>
            /// <returns>data table</returns>
            public DataTable GetVisitDetailsID(string requestId)
            {
                string sqlGetRequestInfoproc = "GetVisitDetailsId";

                try
                {
                    DataTable dtuserrolelocation = new DataTable();
                    SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    ////sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    sqlConn.OpenWithMSI();
                    SqlCommand cmd = new SqlCommand(sqlGetRequestInfoproc, sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@RequestId", SqlDbType.VarChar).Value = requestId;
                    adp.Fill(dtuserrolelocation);
                    sqlConn?.Close();
                    return dtuserrolelocation;

                    // drRoles = cmd.ExecuteReader();                    
                    // return drRoles;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                // finally
                // {
                // }
            }

            ///// <summary>
            ///// Get User role
            ///// </summary>
            ///// <param name="userID">user id</param>
            ///// <returns>List string</returns>
            //public List<string> GetUserRole(string userID)
            //{
            //    List<string> userRole = new List<string>();
            //    try
            //    {

            //        using (SqlConnection connection = new SqlConnection(vmsConn))
            //        {
            //            connection.SetAccessToken();
            //            VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection);
            //            var role = vmsdb.GetUserRole(userID);
            //            foreach (var data in role)
            //            {
            //                userRole.Add(data.UserRole);
            //            }
            //        }

            //        ////SqlConnection connection = new SqlConnection(vmsConn);
            //        ////connection.SetAccessToken();
            //        ////using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb =
            //        ////    new VMSDataObjectsDataContext(connection))
            //        ////{
            //        ////    var role = vmsdb.GetUserRole(userID);
            //        ////    foreach (var data in role)
            //        ////    {
            //        ////        userRole.Add(data.UserRole);
            //        ////    }
            //        ////}
            //    }
            //    catch (System.Data.SqlClient.SqlException ex)
            //    {
            //        throw ex;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }

            //    return userRole;
            //}

            /// <summary>
            /// Get User role
            /// </summary>
            /// <param name="userID">user id</param>
            /// <returns>List string</returns>
            public List<string> GetUserRole(string userID)
            {
                List<string> userRole = new List<string>();
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlConnection sqlConn = new SqlConnection(conn);
                    sqlConn.OpenWithMSI();
                    SqlCommand cmd = new SqlCommand("GetUserRole", sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (sqlConn)
                    {                        
                        IDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            userRole.Add(Convert.ToString(reader["UserRole"]));
                        }

                        reader.Close();
                    }

                    sqlConn?.Close();

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return userRole;
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <returns>data table</returns>
            public List<string> GetStatusDL()
            {
                SqlConnection con;

                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                try
                {
                    List<string> status = new List<string>();
                    SqlDataReader sdr;
                    SqlCommand cmd;
                    ////con.OpenWithMSI(); //// con.Open();  
                    con.OpenWithMSI();
                    cmd = new SqlCommand("GetRequestStatus", con);
                    sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        status.Add(sdr.GetString(0));
                    }

                    return status;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="userID">variable name</param>
            /// <returns>data table</returns>
            public DataTable GetUserRoleLocationDL(string userID)
            {
                string sqlGetRequestInfoproc = "GetLocationForUserRole";
                try
                {
                    DataTable dtuserrolelocation = new DataTable();
                    SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    ////sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    sqlConn.OpenWithMSI();
                    SqlCommand cmd = new SqlCommand(sqlGetRequestInfoproc, sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userID;
                    adp.Fill(dtuserrolelocation);
                    sqlConn?.Close();
                    return dtuserrolelocation;

                    // drRoles = cmd.ExecuteReader();                    
                    // return drRoles;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="userID">variable name</param>
            /// <returns>data table</returns>
            public DataTable GetLocationForUserRole_IDCardAdmin(string userID)
            {
                string sqlGetRequestInfoproc = "GetLocationForUserRole_IDCardAdmin";

                try
                {
                    DataTable dtuserrolelocation = new DataTable();
                    SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    /////sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    sqlConn.OpenWithMSI();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(sqlGetRequestInfoproc, sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userID;
                    adp.Fill(dtuserrolelocation);
                    sqlConn?.Close();
                    return dtuserrolelocation;

                    // drRoles = cmd.ExecuteReader();                    
                    // return drRoles;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="setUserID">variable name</param>
            /// <returns>data table</returns>
            public DataTable GetIDCardLocationForUser(string setUserID)
            {
                string sqlGetRequestInfoproc = "IVS_GetIDCardLocationForUser";

                try
                {
                    DataTable dtuserrolelocation = new DataTable();
                    SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    ////sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    sqlConn.OpenWithMSI();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand(sqlGetRequestInfoproc, sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = setUserID;
                    adp.Fill(dtuserrolelocation);

                    // if (dtUserRoleLocation.Rows.Count>0)
                    // {
                    // Location = dtUserRoleLocation.Rows[0]["Location"].ToString().Replace("NoResult",string.Empty);
                    // }
                    sqlConn?.Close();
                    return dtuserrolelocation;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <returns>data table</returns>
            public DataTable GetRolesDL()
            {
                string sqlGetRequestInfoproc = "PopulateRoles";
                try
                {
                    DataTable dtrole = new DataTable();
                    SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    ////sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    sqlConn.OpenWithMSI();
                    SqlCommand cmd = new SqlCommand(sqlGetRequestInfoproc, sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtrole);
                    sqlConn?.Close();
                    return dtrole;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="setAssociateID">variable name</param>
            /// <param name="setLocation">variable set Location</param>
            /// <returns>data table</returns>
            public DataSet SearchIDCardAdminAllocationDetails(string setAssociateID, string setLocation)
            {
                DataSet set = new DataSet();
                try
                {
                    // SqlParameter paramsetAssociateID;
                    // SqlParameter paramsetLocation;
                    string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    SqlConnection con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    SqlCommand cmd = new SqlCommand("IVS_GetIDCardAdminMappingDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdminID", setAssociateID);
                    cmd.Parameters.AddWithValue("@Location", setLocation);

                    // if (string.IsNullOrEmpty(setAssociateID))
                    // {
                    // paramsetAssociateID = new SqlParameter("@AdminID", SqlDbType.VarChar, 50);
                    // paramsetAssociateID.Value = null;
                    // cmd.Parameters.Add(paramsetAssociateID);
                    // }
                    // else
                    // {
                    // paramsetAssociateID = new SqlParameter("@AdminID", SqlDbType.VarChar, 50);
                    // paramsetAssociateID.Value = setAssociateID;
                    // cmd.Parameters.Add(paramsetAssociateID);
                    // }
                    // if (string.IsNullOrEmpty(setLocation))
                    // {
                    // paramsetLocation = new SqlParameter("@Location", SqlDbType.VarChar, 50);
                    // paramsetLocation.Value = null;
                    // cmd.Parameters.Add(paramsetLocation);
                    // }
                    // else
                    // {
                    // paramsetLocation = new SqlParameter("@Location", SqlDbType.VarChar, 50);
                    // paramsetLocation.Value = setLocation;
                    // cmd.Parameters.Add(paramsetLocation);
                    // }
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                    return set;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="setCity">variable set City</param>
            /// <param name="setFacility">variable set Facility</param>
            /// <param name="setRoleID">variable set Role ID</param>
            /// <param name="setAssociateID">variable set Associate ID</param>
            /// <param name="countryId">variable country Id</param>
            /// <returns>data table</returns>
            public DataSet SearchRoleAllocationDL(string setCity, string setFacility, string setRoleID, string setAssociateID, string countryId)
            {
                DataSet set = new DataSet();
                try
                {
                    if (string.IsNullOrEmpty(setCity) || (setCity == "-1"))
                    {
                        setCity = null;
                    }

                    if (string.IsNullOrEmpty(setFacility))
                    {
                        // setFacility = null;
                        setFacility = "-1";
                    }

                    if (string.IsNullOrEmpty(setAssociateID))
                    {
                        setAssociateID = null;
                    }

                    if (string.IsNullOrEmpty(setRoleID))
                    {
                        setRoleID = null;
                    }

                    if (string.IsNullOrEmpty(countryId))
                    {
                        countryId = null;
                    }

                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlConnection con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    SqlCommand cmd = new SqlCommand("View_UserDetails_LocationId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = setCity;
                    cmd.Parameters.Add("@LocationId", SqlDbType.VarChar, 50).Value = setFacility;
                    cmd.Parameters.Add("@RoleID", SqlDbType.VarChar, 50).Value = setRoleID;
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 50).Value = setAssociateID;
                    cmd.Parameters.Add("@CountryId", SqlDbType.VarChar, 50).Value = countryId;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                    return set;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="setUserID">variable set City</param>
            /// <param name="setFacility">variable set Facility</param>
            /// <param name="setRoleID">variable set Role ID</param>
            /// <param name="setUpdatedBy">variable set Associate ID</param>
            /// <param name="countryId">variable country Id</param>
            /// <returns>data table</returns>
            public int SubmitRoleDetailsDL(string setUserID, int setFacility, string setRoleID, string setUpdatedBy, int countryId)
            {
                try
                {
                    int rows = 0;
                    SqlConnection con;
                    SqlCommand cmd;
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    //// con.OpenWithMSI(); //// con.Open();   
                    con.OpenWithMSI();
                    cmd = new SqlCommand("InsertUserRole_LocationId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userID", setUserID);

                    // cmd.Parameters.AddWithValue("@city", setCity);
                    cmd.Parameters.AddWithValue("@LocationId", setFacility);
                    cmd.Parameters.AddWithValue("@roleID", setRoleID);
                    cmd.Parameters.AddWithValue("@updatedBy", setUpdatedBy);
                    cmd.Parameters.AddWithValue("@countryId", countryId);
                    rows = cmd.ExecuteNonQuery();
                    con?.Close();
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Revoke user access
            /// </summary>
            /// <param name="assoiateID">associate id</param>
            /// <param name="updatedBy">updated by</param>
            /// <param name="role">role value</param>
            /// <returns>boolean value</returns>
            public bool RevokeAccess(string assoiateID, string updatedBy, string role)
            {
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;

                    if (role != "10")
                    {
                        string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();

                        con = new SqlConnection(conn);
                        //// con.OpenWithMSI(); //// con.Open();   
                        con.OpenWithMSI();

                        cmd = new SqlCommand("RevokeUserRole", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userID", assoiateID);
                        cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                        var res = cmd.ExecuteNonQuery();
                        con?.Close();
                        return res == 1 ? true : false;
                    }
                    else
                    {
                        string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();

                        con = new SqlConnection(conn);
                        //// //// con.Open(); con.OpenWithMSI(); 
                        con.OpenWithMSI();

                        cmd = new SqlCommand("RevokeIDCardAdminUserRole", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userID", assoiateID);
                        cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                        var res = cmd.ExecuteNonQuery();
                        con?.Close();
                        return res == 1 ? true : false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Delete contractor
            /// </summary>
            /// <param name="contractorID">contractor id</param>
            /// <param name="updatedBy">updated by</param>
            /// <returns>boolean value</returns>
            public bool DeleteContractor(int contractorID, string updatedBy)
            {
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;
                    string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  

                    cmd = new SqlCommand("DisableContractor", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContractorId", contractorID);
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                    var res = cmd.ExecuteNonQuery();
                    con?.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="setUserID">variable set City</param>
            /// <param name="location">variable set Facility</param>
            /// <param name="setRoleID">variable set Role ID</param>
            /// <param name="setUpdatedBy">variable set Associate ID</param>    
            /// <returns>data table</returns>
            public int AddIDCardAdminRole(string setUserID, string location, string setRoleID, string setUpdatedBy)
            {
                try
                {
                    int rows = 0;
                    SqlConnection con;
                    SqlCommand cmd;
                    string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand("IVS_AddIDCardAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("userID", setUserID);
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@roleID", setRoleID);
                    cmd.Parameters.AddWithValue("@updatedBy", setUpdatedBy);
                    rows = cmd.ExecuteNonQuery();
                    con?.Close();
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method or function
            /// </summary>
            /// <param name="contractorId">variable contractor Id</param>
            /// <param name="contractorName">variable name</param>
            /// <param name="vendorName">variable vendor name</param>
            /// <param name="superVisiorPhone">variable super Visitor Phone</param>
            /// <param name="vendorPhoneNumber">variable vendor Phone Number</param>
            /// <param name="docStatus">variable a Status</param>
            /// <param name="status">variable status</param>
            /// <param name="strContractorId">variable Contractor Id</param>
            /// <param name="userId">variable user Id</param>
            /// <returns>data table</returns>
            public bool UpdateContractorDetails(string contractorId, string contractorName, string vendorName, string superVisiorPhone, string vendorPhoneNumber, string docStatus, string status, int strContractorId, string userId)
            {
                int bitStatus = 0;
                if (status == "Active")
                {
                    bitStatus = 1;
                }
                else
                {
                    bitStatus = 0;
                }

                try
                {
                    string strConnectionString = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    SqlConnection sqlConn = new SqlConnection(strConnectionString);
                    sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    string strRequestInsertSP = "IVS_UpdateContractorInformation";
                    SqlCommand sqlComm = new SqlCommand(strRequestInsertSP, sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.Add("@ContractorNumber", SqlDbType.VarChar, 50).Value = contractorId;
                    sqlComm.Parameters.Add("@ContractorName", SqlDbType.VarChar, 50).Value = contractorName;
                    sqlComm.Parameters.Add("@VendorName", SqlDbType.VarChar, 50).Value = vendorName;
                    sqlComm.Parameters.Add("@Status", SqlDbType.Bit, 50).Value = bitStatus;
                    sqlComm.Parameters.Add("@SuperVisiorPhone", SqlDbType.VarChar, 50).Value = superVisiorPhone;
                    sqlComm.Parameters.Add("@VendorPhoneNumber", SqlDbType.VarChar, 50).Value = vendorPhoneNumber;
                    sqlComm.Parameters.Add("@DOCStatus", SqlDbType.VarChar, 50).Value = docStatus;
                    sqlComm.Parameters.Add("@ContractorId", SqlDbType.Int).Value = Convert.ToInt32(strContractorId);
                    sqlComm.Parameters.Add("@UserId", SqlDbType.VarChar, 11).Value = userId;
                    sqlComm.ExecuteNonQuery();
                    sqlConn?.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Remove ID Card Admin Role
            /// </summary>
            /// <param name="setUserID">variable user Id</param>
            /// <returns>data table</returns>
            public bool RemoveIDCardAdminRole(string setUserID)
            {
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    SqlConnection con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    SqlCommand cmd = new SqlCommand("IVS_RemoveIDCardAdmin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userID", setUserID);
                    var result = cmd.ExecuteNonQuery();
                    con?.Close();
                    return result == -1 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method print status detail
            /// </summary>
            /// <param name="userId">object list</param>
            /// <param name="userName">object user Name</param>
            /// <returns>data table</returns>
            public DataTable GetUserDetailsByIdName(string userId, string userName)
            {
                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                // SqlConnection con = new SqlConnection("Data Source=CTSINTCOVSEZ;Initial Catalog=AdminRepository;User ID=sezpmo;Password=Sezpm0");
                DataTable dt = new DataTable("AssociateInfo");
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("GetUserDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        cmd.Parameters.Add("@AssociateId", SqlDbType.VarChar).Value = userId;
                    }

                    if (!string.IsNullOrEmpty(userName))
                    {
                        cmd.Parameters.Add("@AssociateName", SqlDbType.VarChar).Value = userName;
                    }

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// Method print status detail
            /// </summary>
            /// <param name="userId">object list</param>
            /// <param name="userName">object user Name</param>
            /// <returns>data table</returns>
            public DataTable GetUserDetailsByIdNameclients(string userId, string userName)
            {
                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI();
                // SqlConnection con = new SqlConnection("Data Source=CTSINTCOVSEZ;Initial Catalog=AdminRepository;User ID=sezpmo;Password=Sezpm0");
                DataTable dt = new DataTable("AssociateInfo");
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("GetUserdetailsforhostClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (userId != string.Empty)
                    {
                        cmd.Parameters.Add("@AssociateId", SqlDbType.VarChar).Value = userId;
                    }

                    if (userName != string.Empty)
                    {
                        cmd.Parameters.Add("@AssociateName", SqlDbType.VarChar).Value = userName;
                    }

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// is security
            /// </summary>
            /// <param name="userId">object list</param>          
            /// <returns>data table</returns>
            public string IsSecurity(string userId)
            {
                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  

                // SqlConnection con = new SqlConnection("Data Source=CTSINTCOVSEZ;Initial Catalog=AdminRepository;User ID=sezpmo;Password=Sezpm0");
                string result = string.Empty;
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("ISSecurity", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userId;
                    }

                    result = Convert.ToString(cmd.ExecuteScalar());
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// Get Associate Details
            /// </summary>
            /// <param name="associateID">associate ID</param>            
            /// <returns>data table</returns>
            public DataTable GetCRSAssociateDetails(string associateID)
            {
                DataSet dsreturnSet = new DataSet();
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand("GetAssociateDetails");
                   
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "AssociateId", SqlDbType.VarChar, associateID);
                 
                 using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                {
                    dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                    return dsreturnSet.Tables[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Associate Details
            /// </summary>
            /// <param name="associateID">associate ID</param>            
            /// <returns>data table</returns>
            public IList<string> GetAssciateDetails(string associateID, string visitortype)
            {
                DataSet dsreturnSet = new DataSet();
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlConnection sqlConn = new SqlConnection(conn);
                    List<string> lstAssociates = new List<string>();
                    SqlCommand cmd = new SqlCommand("GetHostNamePS", sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SerachText", associateID);
                    cmd.Parameters.AddWithValue("@visitortype", visitortype);
                    using (sqlConn)
                    {
                        sqlConn.OpenWithMSI();
                        IDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lstAssociates.Add(Convert.ToString(reader["AssociateName"]));
                        }

                        reader.Close();
                    }
                    sqlConn?.Close();
                    return lstAssociates;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method print status detail
            /// </summary>
            /// <param name="visitDetailsID">object list</param>
            ///  /// <param name="locationID">object user Name</param>
            /// <returns>integer value</returns>
            public int GetToken(int visitDetailsID, string locationID)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  

                // SqlConnection con = new SqlConnection("Data Source=CTSINTCOVSEZ;Initial Catalog=AdminRepository;User ID=sezpmo;Password=Sezpm0");
                int result = 0;
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("GetToken", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(locationID))
                    {
                        cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = locationID;
                        cmd.Parameters.Add("@VisitDetailsID", SqlDbType.VarChar).Value = visitDetailsID;
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// Method Submit Equipment Custody
            /// </summary>
            /// <param name="visitDetailsID">object list</param>
            /// <param name="tokenNumber">object token Number</param>
            /// <param name="equipmentType">equipment Type</param>
            /// <param name="description">object description</param>
            /// <param name="status">object status</param>
            /// <param name="facilityId">object facility Id</param>
            /// <param name="createdDate">created Date</param>
            /// <param name="equipmentID">object equipment ID</param>
            public void SubmitEquipmentCustody(int visitDetailsID, int tokenNumber, string equipmentType, string description, string status, string facilityId, DateTime createdDate, int equipmentID)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("SubmitEquipmentsInCustodyDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VisitDetailsID", SqlDbType.Int).Value = visitDetailsID;
                    cmd.Parameters.Add("@TokenNumber", SqlDbType.Int).Value = tokenNumber;
                    cmd.Parameters.Add("@EquipmentType", SqlDbType.VarChar).Value = equipmentType;
                    cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                    cmd.Parameters.Add("@FacilityId", SqlDbType.VarChar).Value = facilityId;
                    cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = createdDate;
                    cmd.Parameters.Add("@EquipmentID", SqlDbType.Int).Value = equipmentID;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// Get Equipment In Custody
            /// </summary>
            /// <param name="visitdetaisID">object list</param>
            /// <returns>Data Table</returns>
            public DataTable GetEquipmentInCustody(int visitdetaisID)
            {
                DataSet dsequip = null;
                DataTable dtequip = null;
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand("GetEquipmentInCustody");
                   
                        dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbapplicantInfoComm, "VisitDetailsID", SqlDbType.VarChar, visitdetaisID);
                        
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsequip = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                        dtequip = dsequip.Tables[0];
                    }
                    sqlConn = null;
                    dbapplicantInfoComm.Connection?.Close();
                    return dtequip;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method Get Equipment In Custody
            /// </summary>
            /// <param name="visitdetaisID">object list</param>
            /// <returns>Data Table</returns>
            public DataTable GetTokenDetails(int visitdetaisID)
            {
                DataSet dstoken = null;
                DataTable dttoken = null;
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand("GetTokenDetails");
                    
                        dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbapplicantInfoComm, "VisitDetailsID", SqlDbType.VarChar, visitdetaisID);
                        
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dstoken = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                        dttoken = dstoken.Tables[0];
                    }
                    sqlConn = null;
                    dbapplicantInfoComm.Connection?.Close();
                    return dttoken;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method Get Equipment In Custody
            /// </summary>
            /// <param name="detailsId">object list</param>
            public void ClearEquipmentData(string detailsId)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("ClearVisitorEquipmentCustody", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VisitDetailId", SqlDbType.VarChar).Value = detailsId;

                    // DbCommand dbApplicantInfoComm = sqlConn.GetStoredProcCommand("ClearVisitorEquipmentCustody");
                    // dbApplicantInfoComm.CommandType = CommandType.StoredProcedure;
                    // sqlConn.AddInParameter(dbApplicantInfoComm, "VisitDetailsID", SqlDbType.VarChar, detailsId);
                    // dbApplicantInfoComm.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }
        }

        /// <summary>
        /// Master Data
        /// </summary>
        public class MasterDataDL
        {
            #region "variables"

            #endregion

            /// <summary>
            /// Method Get Equipment In Custody
            /// </summary>
            /// <param name="associateID">object list</param>
            /// <param name="passNumber">object pass Number</param>
            /// <param name="strAccessCardNo">object AccessCard No</param>
            /// <param name="facilityId">object facility Id</param>
            /// <returns>Data Table</returns>
            public bool StoreTempAccessCardDetails(string associateID, string passNumber, string strAccessCardNo, string facilityId)
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("IVS_StoreOneDayAccessCard", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AssociateID", SqlDbType.VarChar).Value = associateID;
                    cmd.Parameters.Add("@PassNumber", SqlDbType.VarChar).Value = passNumber;
                    cmd.Parameters.Add("@AccessCardNumber", SqlDbType.VarChar).Value = strAccessCardNo;
                    cmd.Parameters.Add("@FacilityID", SqlDbType.VarChar).Value = facilityId;
                    cmd.CommandTimeout = 1000;
                    var result = cmd.ExecuteNonQuery();
                    return result == -1 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// get Facility ID For AccessCard
            /// </summary>
            /// <param name="facilityID">object facility ID</param>
            /// <returns>Data Table</returns>
            public string GetFacilityIDForAccessCard(string facilityID)
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("[IVS_GetLocationIDForOneDayAccessCard]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(facilityID))
                    {
                        cmd.Parameters.Add("@FACILITY_ID", SqlDbType.VarChar).Value = facilityID;
                    }

                    string result = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// Save the id to DB
            /// </summary>
            /// <param name="associateID">associate id</param>
            /// <param name="fileContentID">file content id</param>
            /// <param name="appTemplateID">application template id</param>
            /// <returns>boolean value</returns>
            public bool StoreFileContentID(string associateID, string fileContentID, string appTemplateID)
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("IVS_StoreFileContentID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AssociateID", SqlDbType.VarChar).Value = associateID;
                    cmd.Parameters.Add("@FileContentID", SqlDbType.VarChar).Value = fileContentID;
                    cmd.Parameters.Add("@AppTemplateId", SqlDbType.VarChar).Value = appTemplateID;
                    var result = cmd.ExecuteNonQuery();
                    return result == -1 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// Delete the Chosen id from DB
            /// </summary>
            /// <param name="associateID">associate id</param>
            /// <returns>boolean value</returns>
            public bool DeleteFileContentID(string associateID)
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("IVS_DeleteFileContentID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AssociateID", SqlDbType.VarChar).Value = associateID;
                    var result = cmd.ExecuteNonQuery();
                    return result == -1 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// GetAssociateDes_Mailer from DB for IVS
            /// </summary>
            /// <param name="associateID">associate id</param>
            /// <returns>integer value</returns>
            public int GetAssociateDes_Mailer(string associateID)
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    int result = 0;
                    cmd = new SqlCommand("IVS_GetAssociateDes_Mailer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AssociateID", SqlDbType.VarChar).Value = associateID;
                    SqlParameter output = new SqlParameter("@Result", SqlDbType.Int);
                    output.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(output);
                    cmd.ExecuteNonQuery();
                    result = Convert.ToInt32(output.Value);
                    con?.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// Save the Chosen name to DB
            /// </summary>
            /// <param name="associateID">associate Id</param>
            /// <param name="nameChosen">name chosen</param>
            /// <returns>boolean value</returns>
            public bool StoreChosenName(string associateID, string nameChosen)
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("IVS_StoreNameChosen", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AssociateID", SqlDbType.VarChar).Value = associateID;
                    cmd.Parameters.Add("@AssociateName", SqlDbType.VarChar).Value = nameChosen;
                    var result = cmd.ExecuteNonQuery();
                    con?.Close();
                    return result == -1 ? true : false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// check if the chosen name already exists in DB
            /// </summary>
            /// <param name="associateID">associate ID</param>
            /// <returns>string value</returns>
            public string CheckNameAlreadyExists(string associateID)
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                SqlCommand cmd;
                try
                {
                    cmd = new SqlCommand("IVS_CheckNameChosen", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AssociateID", SqlDbType.VarChar).Value = associateID;
                    object result = cmd.ExecuteScalar();
                    string nameChosen = result == null ? string.Empty : Convert.ToString(result);
                    return nameChosen;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                    cmd = null;
                }
            }

            /// <summary>
            /// for last print details
            /// </summary>
            /// <param name="requestId">request id</param>
            /// <returns>data table</returns>
            public PrintStatusDetails GetVisitStatusByRequestId(int requestId)
            {
                SqlAzureDatabase sqlConn;
                DbCommand dbcmd;
                PrintStatusDetails visitDetails = null;

                try
                {
                    sqlConn = new SqlAzureDatabase(ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    dbcmd = sqlConn.GetStoredProcCommand("GetVisitStatusByRequestId");
                    dbcmd.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dbcmd, "@VisitDetailsId", SqlDbType.VarChar, requestId);
                    visitDetails = new PrintStatusDetails();
                    using (IDataReader dbreader = sqlConn.ExecuteReader(dbcmd))
                    {
                        while (dbreader.Read())
                        {
                            // #region chk
                            // visitDetails.RequestId = Convert.ToInt32(dbReader["RequestID"]);
                            // if (dbReader.IsDBNull(1))
                            // visitDetails.Date = string.Empty;
                            // else
                            // visitDetails.Date = DateTime.Parse(dbReader["Date"].ToString()).ToShortDateString();
                            // if (dbReader.IsDBNull(2))
                            // visitDetails.ActualOutTime = string.Empty;
                            // else
                            // visitDetails.ActualOutTime = DateTime.Parse(dbReader["ActualOutTime"].ToString()).ToShortTimeString();
                            // if (dbReader.IsDBNull(3))
                            // visitDetails.BadgeNo = string.Empty;
                            // else
                            // visitDetails.BadgeNo = Convert.ToString(dbReader["BadgeNo"]);
                            // if (dbReader.IsDBNull(4))
                            // visitDetails.ActualInTime = string.Empty;
                            // else
                            // visitDetails.ActualInTime = DateTime.Parse(dbReader["ActualCheckin"].ToString()).ToShortTimeString();
                            // visitDetails.BadgeStatus = Convert.ToString(dbReader["BadgeStatus"]);
                            // if (dbReader.IsDBNull(6))
                            // visitDetails.ExpectedInTime = string.Empty;
                            // else
                            // visitDetails.ExpectedInTime = DateTime.Parse(dbReader["ExpectedCheckin"].ToString()).ToShortTimeString();
                            // if (dbReader.IsDBNull(7))
                            // visitDetails.ExpectedOutTime = string.Empty;
                            // else
                            // visitDetails.ExpectedOutTime = DateTime.Parse(dbReader["ExpectedCheckOut"].ToString()).ToShortTimeString();
                            // #endregion
                        }
                    }
                 
                    return visitDetails;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get Facility Id
            /// </summary>
            /// <param name="facilityName">object list</param>    
            /// <returns>Data Table</returns>
            public DataTable GetFacilityId(string facilityName)
            {
                DataSet dsfacilityId = null;
                DataTable dtfacilityId = null;
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand("GetFacilityId");
                    
                        dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbapplicantInfoComm, "FacilityName", SqlDbType.VarChar, facilityName);
                        
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsfacilityId = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                        dtfacilityId = dsfacilityId.Tables[0];
                    }
                    sqlConn = null;
                    dbapplicantInfoComm.Connection?.Close();
                    return dtfacilityId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get Master Data
            /// </summary>
            /// <param name="type">object list</param>    
            /// <returns>string list</returns>
            public List<string> GetMasterData(string type)
            {
                List<string> masterData = new List<string>();

                SqlConnection connection = new SqlConnection(vmsConn);
                connection.SetAccessToken();

                using (VMSBusinessEntity.VMSDataObjectsDataContext vms_db = new VMSDataObjectsDataContext(connection))
                {
                    var purpose = vms_db.GetMasterData(type);
                    foreach (var data in purpose)
                    {
                        masterData.Add(data.MasterDataDescription + '|' + data.MasterDataID);
                    }
                }
                connection?.Close();
                return masterData;
            }

            /// <summary>
            /// Chart VMS
            /// </summary>
            /// <param name="city">object city</param>  
            /// <param name="facility">object facility</param>  
            /// <param name="fromdate">object from date</param>  
            /// <param name="todate">object to date</param>  
            /// <param name="department">object department</param>  
            /// <param name="country">object country</param>  
            /// <returns>Data set</returns>
            public DataSet Chart_VMS(string city, string facility, string fromdate, string todate, string department, string country)
            {
                DataSet set = new DataSet();
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;
                    SqlParameter paramvisitingcountry;
                    SqlParameter paramvisitingcity;
                    SqlParameter paramvisitingfacility;
                    SqlParameter paramdepartment;
                    SqlParameter paramfromdate;
                    SqlParameter paramtodate;
                    SqlDataAdapter adp;

                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    cmd = new SqlCommand("Chart_VMS_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (country == "0" || country == null)
                    {
                        paramvisitingcountry = new SqlParameter("@Country", SqlDbType.VarChar, 50);
                        paramvisitingcountry.Value = null;
                        cmd.Parameters.Add(paramvisitingcountry);
                    }
                    else
                    {
                        paramvisitingcountry = new SqlParameter("@Country", SqlDbType.VarChar, 50);
                        paramvisitingcountry.Value = country;
                        cmd.Parameters.Add(paramvisitingcountry);
                    }

                    // added by uma on 22/aug/09
                    if (city == "Select" || city == null)
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = null;
                        cmd.Parameters.Add(paramvisitingcity);
                    }
                    else
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = city;
                        cmd.Parameters.Add(paramvisitingcity);
                    }

                    if (facility == "Select" || string.IsNullOrEmpty(facility))
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = null;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }
                    else
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = facility;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }

                    if (department == "Select" || string.IsNullOrEmpty(department))
                    {
                        paramdepartment = new SqlParameter("@Department", SqlDbType.VarChar, 50);
                        paramdepartment.Value = null;
                        cmd.Parameters.Add(paramdepartment);
                    }
                    else
                    {
                        paramdepartment = new SqlParameter("@Department", SqlDbType.VarChar, 50);
                        paramdepartment.Value = department;
                        cmd.Parameters.Add(paramdepartment);
                    }

                    if (fromdate == null || string.IsNullOrEmpty(fromdate.Trim()) || fromdate.Trim() == "__/__/____")
                    {
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = null;
                        cmd.Parameters.Add(paramfromdate);
                    }
                    else
                    {
                        string[] fromDate = fromdate.Split('/');
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = fromDate[2] + "-" + fromDate[1] + "-" + fromDate[0];
                        cmd.Parameters.Add(paramfromdate);
                    }

                    if (string.IsNullOrEmpty(todate) || todate.Trim() == "__/__/____")
                    {
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = null;
                        cmd.Parameters.Add(paramtodate);
                    }
                    else
                    {
                        string[] todates = todate.Split('/');
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = todates[2] + "-" + todates[1] + "-" + todates[0];
                        cmd.Parameters.Add(paramtodate);
                    }

                    // added by uma on 22/aug/09----------End.
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                    return set;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Usage Report
            /// </summary>
            /// <param name="city">object city</param>  
            /// <param name="facility">object facility</param>  
            /// <param name="fromdate">object from date</param>  
            /// <param name="todate">object to date</param>  
            /// <param name="department">object department</param>  
            /// <param name="country">object country</param>  
            /// <returns>Data set</returns>
            public DataSet UsageReport(string city, string facility, string fromdate, string todate, string department, string country)
            {
                DataSet set = new DataSet();
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;
                    SqlParameter paramvisitingcountry;
                    SqlParameter paramvisitingcity;
                    SqlParameter paramvisitingfacility;
                    SqlParameter paramdepartment;
                    SqlParameter paramfromdate;
                    SqlParameter paramtodate;
                    SqlDataAdapter adp;

                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    cmd = new SqlCommand("VMS_Usage_Report_LocationId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (country == "Select" || country == null)
                    {
                        paramvisitingcountry = new SqlParameter("@Country", SqlDbType.VarChar, 50);
                        paramvisitingcountry.Value = null;
                        cmd.Parameters.Add(paramvisitingcountry);
                    }
                    else
                    {
                        paramvisitingcountry = new SqlParameter("@Country", SqlDbType.VarChar, 50);
                        paramvisitingcountry.Value = country;
                        cmd.Parameters.Add(paramvisitingcountry);
                    }

                    // added by uma on 22/aug/09
                    if (city == "Select" || city == null)
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = null;
                        cmd.Parameters.Add(paramvisitingcity);
                    }
                    else
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = city;
                        cmd.Parameters.Add(paramvisitingcity);
                    }

                    if (facility == "Select" || string.IsNullOrEmpty(facility))
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = null;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }
                    else
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = facility;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }

                    if (department == "Select" || string.IsNullOrEmpty(department))
                    {
                        paramdepartment = new SqlParameter("@Department", SqlDbType.VarChar, 50);
                        paramdepartment.Value = null;
                        cmd.Parameters.Add(paramdepartment);
                    }
                    else
                    {
                        paramdepartment = new SqlParameter("@Department", SqlDbType.VarChar, 50);
                        paramdepartment.Value = department;
                        cmd.Parameters.Add(paramdepartment);
                    }

                    if (fromdate == null || string.IsNullOrEmpty(fromdate.Trim()) || fromdate.Trim() == "__/__/____")
                    {
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = null;
                        cmd.Parameters.Add(paramfromdate);
                    }
                    else
                    {
                        string[] fromDate = fromdate.Split('/');
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = fromDate[2] + "-" + fromDate[1] + "-" + fromDate[0];
                        cmd.Parameters.Add(paramfromdate);
                    }

                    if (string.IsNullOrEmpty(todate) || todate.Trim() == "__/__/____")
                    {
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = null;
                        cmd.Parameters.Add(paramtodate);
                    }
                    else
                    {
                        string[] todates = todate.Split('/');
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = todates[2] + "-" + todates[1] + "-" + todates[0];
                        cmd.Parameters.Add(paramtodate);
                    }

                    // added by uma on 22/aug/09----------End.
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                    return set;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Usage Report
            /// </summary>
            /// <param name="city">object city</param>  
            /// <param name="facility">object facility</param>  
            /// <param name="fromdate">object from date</param>  
            /// <param name="todate">object to date</param>  
            /// <param name="department">object department</param>  
            /// <param name="country">object country</param>  
            /// <returns>Data set</returns>
            public DataSet PieGraph(string city, string facility, string fromdate, string todate, string department, string country)
            {
                DataSet set = new DataSet();
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;
                    SqlParameter paramvisitingcountry;
                    SqlParameter paramvisitingcity;
                    SqlParameter paramvisitingfacility;
                    SqlParameter paramdepartment;
                    SqlParameter paramfromdate;
                    SqlParameter paramtodate;
                    SqlDataAdapter adp;

                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    cmd = new SqlCommand("VMS_PIE_GRAPH_LocationId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (country == "Select" || country == null)
                    {
                        paramvisitingcountry = new SqlParameter("@Country", SqlDbType.VarChar, 50);
                        paramvisitingcountry.Value = null;
                        cmd.Parameters.Add(paramvisitingcountry);
                    }
                    else
                    {
                        paramvisitingcountry = new SqlParameter("@Country", SqlDbType.VarChar, 50);
                        paramvisitingcountry.Value = country;
                        cmd.Parameters.Add(paramvisitingcountry);
                    }

                    // added by uma on 22/aug/09
                    if (city == "Select" || city == null)
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = null;
                        cmd.Parameters.Add(paramvisitingcity);
                    }
                    else
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = city;
                        cmd.Parameters.Add(paramvisitingcity);
                    }

                    if (facility == "Select" || string.IsNullOrEmpty(facility))
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = null;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }
                    else
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = facility;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }

                    if (department == "Select" || string.IsNullOrEmpty(department))
                    {
                        paramdepartment = new SqlParameter("@Department", SqlDbType.VarChar, 50);
                        paramdepartment.Value = null;
                        cmd.Parameters.Add(paramdepartment);
                    }
                    else
                    {
                        paramdepartment = new SqlParameter("@Department", SqlDbType.VarChar, 50);
                        paramdepartment.Value = department;
                        cmd.Parameters.Add(paramdepartment);
                    }

                    if (fromdate == null || string.IsNullOrEmpty(fromdate.Trim()) || fromdate.Trim() == "__/__/____")
                    {
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = null;
                        cmd.Parameters.Add(paramfromdate);
                    }
                    else
                    {
                        string[] fromDate = fromdate.Split('/');
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = fromDate[2] + "-" + fromDate[1] + "-" + fromDate[0];
                        cmd.Parameters.Add(paramfromdate);
                    }

                    if (todate == null || string.IsNullOrEmpty(todate) || todate.Trim() == "__/__/____")
                    {
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = null;
                        cmd.Parameters.Add(paramtodate);
                    }
                    else
                    {
                        string[] todates = todate.Split('/');
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = todates[2] + "-" + todates[1] + "-" + todates[0];
                        cmd.Parameters.Add(paramtodate);
                    }

                    // added by uma on 22/aug/09----------End.
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                    return set;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            ///  End VMS CR 16 Changes.
            /// </summary>
            /// <param name="visitorProof">visitor Proof</param>  
            /// <param name="visitorMaster">visitor Master</param>  
            /// <param name="visitorRequest">visitor Request</param>  
            /// <param name="visitDetail">visit Detail</param>  
            /// <param name="visitorEquipmentObj">visitor Equipment Object</param>  
            /// <param name="visitorEmergencyContact">visitor Emergency Contact</param>  
            /// <param name="visitorID">visitor ID</param>  
            /// <param name="identityDetails">identity Details</param>  
            /// <returns>Data set</returns>
            public int SubmitVisitorInformation(VMSBusinessEntity.VisitorProof visitorProof, VMSBusinessEntity.VisitorMaster visitorMaster, VMSBusinessEntity.VisitorRequest visitorRequest, VMSBusinessEntity.VisitDetail[] visitDetail, VisitorEquipment[] visitorEquipmentObj, VisitorEmergencyContact visitorEmergencyContact, int? visitorID, IdentityDetails identityDetails)
            {
                int? insRequestID = 0;
#pragma warning disable CS0219 // The variable 'insertFlag' is assigned but its value is never used
                bool insertFlag = false;
#pragma warning restore CS0219 // The variable 'insertFlag' is assigned but its value is never used
                int tempVisitorID = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {

                        SqlConnection connection = new SqlConnection(vmsConn);
                        connection.SetAccessToken();

                        using (VMSBusinessEntity.VMSDataObjectsDataContext vms_DB = new VMSDataObjectsDataContext(connection))
                        {
                            vms_DB.Connection.Open();

                            if (visitorID.Equals(0))
                            {
                                vms_DB.VisitorMasters.InsertOnSubmit(visitorMaster);
                                vms_DB.SubmitChanges();
                                visitorRequest.VisitorID = visitorMaster.VisitorID;
                                tempVisitorID = visitorMaster.VisitorID;
                                vms_DB.VisitorRequests.InsertOnSubmit(visitorRequest);
                                vms_DB.SubmitChanges();

                                // if ((VisitorProof.Photo != null || VisitorProof.IDProofImage != null))
                                if (visitorProof.Photo != null)
                                {
                                    visitorProof.VisitorID = visitorMaster.VisitorID;
                                    vms_DB.VisitorProofs.InsertOnSubmit(visitorProof);
                                    vms_DB.SubmitChanges();
                                }

                                visitorEmergencyContact.RequestID = visitorRequest.RequestID;
                                insRequestID = visitorRequest.RequestID;
                                vms_DB.VisitorEmergencyContacts.InsertOnSubmit(visitorEmergencyContact);

                                if (visitDetail != null)
                                {
                                    foreach (VMSBusinessEntity.VisitDetail visitDetailObj in visitDetail)
                                    {
                                        visitDetailObj.RequestID = (int)insRequestID;
                                        vms_DB.VisitDetails.InsertOnSubmit(visitDetailObj);
                                        vms_DB.SubmitChanges();
                                    }
                                }

                                insertFlag = true;
                                vms_DB.SubmitChanges();
                            }
                            else
                            {
                                visitorRequest.VisitorID = (int)visitorID;

                                //// VMSDb.UpdateVisitorDetails(VisitorMaster.Title.ToString(), VisitorMaster.FirstName.ToString(), VisitorMaster.LastName.ToString(), VisitorMaster.Company.ToString(), VisitorMaster.Designation.ToString(), VisitorMaster.Country.ToString(), VisitorMaster.MobileNo.ToString(), VisitorMaster.EmailID.ToString(), visitorID.ToString());
                                vms_DB.UpdateVisitorDetails(visitorMaster.FirstName.ToString(), visitorMaster.LastName.ToString(), visitorMaster.Company.ToString(), visitorMaster.Designation.ToString(), visitorMaster.Country.ToString(), visitorMaster.MobileNo.ToString(), visitorMaster.EmailID.ToString(), visitorID.ToString(), visitorMaster.Gender.ToString(), visitorMaster.IsConfidential);
                                vms_DB.VisitorRequests.InsertOnSubmit(visitorRequest);

                                vms_DB.SubmitChanges();

                                visitorEmergencyContact.RequestID = visitorRequest.RequestID;
                                //// 25/8/09
                                insRequestID = visitorRequest.RequestID;

                                // end
                                vms_DB.VisitorEmergencyContacts.InsertOnSubmit(visitorEmergencyContact);
                                vms_DB.SubmitChanges();
                                if (visitDetail != null)
                                {
                                    foreach (VMSBusinessEntity.VisitDetail visitDetailObj in visitDetail)
                                    {
                                        visitDetailObj.RequestID = (int)insRequestID;
                                        vms_DB.VisitDetails.InsertOnSubmit(visitDetailObj);
                                        vms_DB.SubmitChanges();
                                    }
                                }

                                vms_DB.SubmitChanges();
                            }

                            scope.Complete();
                        }
                        connection?.Close();
                        return tempVisitorID;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            /// <summary>
            /// Insert Identity Details
            /// </summary>
            /// <param name="identityDetails">identity Details</param>  
            public void InsertIdentityDetails(IdentityDetails identityDetails)
            {
                string sqlInsertIdentityDetails = "InsertIdentityDetails";
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection sqlConn = new SqlConnection(conn);
                try
                {
                   
                        sqlConn.OpenWithMSI(); //// sqlConn.Open();
                        SqlCommand dbidentity = new SqlCommand(sqlInsertIdentityDetails, sqlConn);
                        dbidentity.CommandType = CommandType.StoredProcedure;
                        dbidentity.Parameters.AddWithValue("VisitorID", identityDetails.VisitorID);
                        dbidentity.Parameters.AddWithValue("IdentityType", identityDetails.IdentityType.Trim());
                        dbidentity.Parameters.AddWithValue("IdentityNo", identityDetails.IdentityNo.Trim());
                    using (sqlConn)
                    {
                        dbidentity.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            /// <summary>
            /// Insert Identity Details
            /// </summary>
            /// <param name="requestId">Request Id</param>  
            public void UpdateParentReferenceId(int? requestId)
            {
                string sqlInsertIdentityDetails = "UpdateParentReferenceId";
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection sqlConn = new SqlConnection(conn);
                try
                {
                    
                        sqlConn.OpenWithMSI(); //// sqlConn.Open();
                        SqlCommand dbidentity = new SqlCommand(sqlInsertIdentityDetails, sqlConn);
                        dbidentity.CommandType = CommandType.StoredProcedure;
                        dbidentity.Parameters.AddWithValue("RequestId", requestId.ToString());
                    using (sqlConn)
                    {
                        dbidentity.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConn?.Close();
                }
            }

            /// <summary>
            /// Insert Identity Details
            /// </summary>
            /// <param name="visitDetail">visit Details</param>  
            /// <param name="visitorEquip">visit Equipment</param>  
            public void InsertEquipment(VisitDetail[] visitDetail, VisitorEquipment[] visitorEquip)
            {
                try
                {
                    VisitorEquipment[] visitorEquipMentCollection = null;
                    ArrayList arrayList1 = new ArrayList();
                    if (visitorEquip != null)
                    {
                        foreach (VMSBusinessEntity.VisitDetail visitDetailObj in visitDetail)
                        {
                            foreach (VMSBusinessEntity.VisitorEquipment vistorEquipmentObj in visitorEquip)
                            {
                                vistorEquipmentObj.VisitDetailsID = visitDetailObj.VisitDetailsID;
                                VMSBusinessEntity.VisitorEquipment equipNew = new VisitorEquipment();
                                equipNew.VisitDetailsID = vistorEquipmentObj.VisitDetailsID;
                                equipNew.MasterDataID = vistorEquipmentObj.MasterDataID;
                                equipNew.Make = vistorEquipmentObj.Make;
                                equipNew.Model = vistorEquipmentObj.Model;
                                equipNew.SerialNo = vistorEquipmentObj.SerialNo;
                                equipNew.Others = vistorEquipmentObj.Others;
                                arrayList1.Add(equipNew);
                            }
                        }

                        visitorEquipMentCollection = new VMSBusinessEntity.VisitorEquipment[arrayList1.Count];
                        arrayList1.CopyTo(0, visitorEquipMentCollection, 0, arrayList1.Count);
                        using (TransactionScope scope = new TransactionScope())
                        {

                            SqlConnection connection = new SqlConnection(vmsConn);
                            connection.SetAccessToken();

                            using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                            {
                                vmsdb.Connection.Open();

                                foreach (VMSBusinessEntity.VisitorEquipment vistorEquipmentNew in visitorEquipMentCollection)
                                {
                                    vmsdb.VisitorEquipments.InsertOnSubmit(vistorEquipmentNew);
                                    vmsdb.SubmitChanges();
                                }

                                vmsdb.SubmitChanges();
                                vmsdb.Connection?.Close();
                            }

                            scope.Complete();
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Begin Changes URL Restriction 
            /// </summary>
            /// <param name="refID">reference ID Details</param>  
            /// <param name="emailID">email ID</param>  
            /// <param name="country">country ID</param>  
            /// <param name="city">city ID</param>  
            /// <param name="facility">facility ID</param>  
            /// <param name="purpose">purpose ID</param>  
            /// <param name="hostID">host ID</param>  
            /// <param name="hostPhoneNo">host Phone</param>  
            /// <param name="fromDate">from Date</param>  
            /// <param name="todate">to Date</param>  
            /// <param name="fromTime">from Time</param>  
            /// <param name="totime">to Time</param>  
            /// <param name="createdBy">created By</param>  
            /// <returns>Data set</returns>
            public DataSet SendLinkToVisitor(string refID, string emailID, string country, string city, string facility, string purpose, string hostID, string hostPhoneNo, string fromDate, string todate, string fromTime, string totime, string createdBy)
            {
                DataSet dsreturnSet = new DataSet();
                dsreturnSet = null;
                string sqlGetEmployeeInfoproc = "SubmitRequestByHost";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                    
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "RefID", SqlDbType.VarChar, refID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "EmailID", SqlDbType.VarChar, emailID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "Country", SqlDbType.VarChar, country);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "City", SqlDbType.VarChar, city);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "Facility", SqlDbType.VarChar, facility);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "Purpose", SqlDbType.VarChar, purpose);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "HostID", SqlDbType.VarChar, hostID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "HostPhoneNumber", SqlDbType.VarChar, hostPhoneNo);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "CreatedBy", SqlDbType.VarChar, createdBy);

                        if (!(string.IsNullOrEmpty(fromDate) || fromDate.Trim().Equals("__/__/____")))
                        {
                            string[] fromdate = fromDate.Split('/');
                            sqlConn.AddInParameter(dbemployeeInfoComm, "FromDate", SqlDbType.Date, fromdate[2] + "-" + fromdate[1] + "-" + fromdate[0]);
                        }

                        if (!(string.IsNullOrEmpty(todate) || todate.Trim().Equals("__/__/____")))
                        {
                            string[] todates = todate.Split('/');
                            sqlConn.AddInParameter(dbemployeeInfoComm, "ToDate", SqlDbType.Date, todates[2] + "-" + todates[1] + "-" + todates[0]);
                        }

                        if (!(string.IsNullOrEmpty(fromTime) || fromTime.Trim().Equals("__/__/____")))
                        {
                            sqlConn.AddInParameter(dbemployeeInfoComm, "FromTime", SqlDbType.Time, fromTime);
                        }

                        if (!(string.IsNullOrEmpty(totime) || totime.Trim().Equals("__/__/____")))
                        {
                            string[] todates = todate.Split('/');
                            sqlConn.AddInParameter(dbemployeeInfoComm, "ToTime", SqlDbType.Time, totime);
                        }
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                    return dsreturnSet;
                    
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dsreturnSet = null;
                }
            }

            /// <summary>
            /// Get Link Archive
            /// </summary>
            /// <param name="hostID">host ID</param>  
            /// <returns>Data set</returns>
            public DataSet GetLinkArchieve(string hostID)
            {
                DataSet dsreturnSet = new DataSet();
                dsreturnSet = null;
                string sqlGetEmployeeInfoproc = "GetLinksArchieve";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                    
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "HostID", SqlDbType.VarChar, hostID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                    return dsreturnSet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dsreturnSet = null;
                }
            }

            /// <summary>
            /// Resend Visitor Link
            /// </summary>
            /// <param name="hostID">host ID</param>  
            /// <param name="requestID">request ID</param>  
            /// <param name="emailID">email ID</param>  
            /// <returns>Data set</returns>
            public DataSet ResendVisitorLink(string hostID, int requestID, string emailID)
            {
                DataSet dsreturnSet = new DataSet();
                dsreturnSet = null;
                string sqlGetEmployeeInfoproc = "InsertLinkRequest";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                 
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                   
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "HostID", SqlDbType.VarChar, hostID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "RequestID", SqlDbType.Int, requestID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "EmailID", SqlDbType.VarChar, emailID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                    return dsreturnSet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dsreturnSet = null;
                }
            }

            /// <summary>
            /// DeActivate Link
            /// </summary>
            /// <param name="linkID">link ID</param>                
            public void DeActivateLink(int linkID)
            {
                string sqlGetEmployeeInfoproc = "DeActivateLink";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                   
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "LinkID", SqlDbType.BigInt, linkID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Check Visitor Details Exists
            /// </summary>
            /// <param name="visitorMaster">visitor Master</param>  
            /// <param name="visitorRequest">visitor Request</param>  
            /// <param name="visitorID">visitor ID</param>  
            /// <returns>integer value</returns> 
            public int? CheckVisitorDetailsExists(VMSBusinessEntity.VisitorMaster visitorMaster, VMSBusinessEntity.VisitorRequest visitorRequest, ref int? visitorID)
            {
                try
                {

                    SqlConnection connection = new SqlConnection(vmsConn);
                    connection.SetAccessToken();

                    using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                    {
                        List<int?> visitorExistsDetails = new List<int?>();
                        vmsdb.Connection.Open();
                        int? success = 0;

                        // int? visitorID = 0;
                        int? visitorRequestExists = 0;

                        // VMSDb.GetVisitorDetails(VisitorMaster.Title, VisitorMaster.FirstName, VisitorMaster.LastName, VisitorMaster.Company, VisitorMaster.Designation, VisitorMaster.Country, VisitorMaster.MobileNo, VisitorMaster.EmailID, VisitorRequest.Country, VisitorRequest.City, VisitorRequest.Facility, VisitorRequest.Purpose, VisitorRequest.HostID, VisitorRequest.HostContactNo, VisitorRequest.FromDate, VisitorRequest.ToDate, VisitorRequest.FromTime, VisitorRequest.ToTime, VisitorRequest.Escort, VisitorRequest.VehicleNo, ref success, ref visitorID, ref visitorRequestExists);
                        // commented by bincey
                        vmsdb.GetVisitorDetails(visitorMaster.FirstName, visitorMaster.LastName, visitorMaster.Company, visitorMaster.Designation, Convert.ToInt32(visitorMaster.Country), visitorMaster.MobileNo, visitorMaster.EmailID, Convert.ToInt32(visitorRequest.LocationId), visitorRequest.Purpose, visitorRequest.HostID, visitorRequest.HostContactNo, visitorRequest.FromDate, visitorRequest.ToDate, visitorRequest.FromTime, visitorRequest.ToTime, visitorRequest.RecurrencePattern, visitorRequest.Occurence, ref success, ref visitorID, ref visitorRequestExists);
                        vmsdb.Connection?.Close();
                        return success;
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Verify Express Check in Code
            /// </summary>
            /// <param name="requestID">request ID</param>  
            /// <param name="securityID">security ID</param>              
            /// <returns>long value</returns> 
            public long? VerifyExpressCheckinCode(int requestID, string securityID)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(vmsConn);
                    connection.SetAccessToken();

                    using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                    {
                        List<int?> visitorExistsDetails = new List<int?>();
                        vmsdb.Connection.Open();
                        long? success = 0;
                        vmsdb.VerifyExpressCheckIn(requestID, securityID, ref success);
                        connection?.Close();
                        return success;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Verify Safety PermitId
            /// </summary>
            /// <param name="requestID">request ID</param>  
            /// <param name="securityID">security ID</param>  
            /// <param name="fromdateActual">from date Actual</param>  
            /// <param name="todateActual">to date Actual</param>  
            /// <param name="reqStatus">request status</param>  
            /// <param name="strSP">stored procedure string</param>  
            /// <returns>integer value</returns> 
            public DataSet VerifySafetyPermitId(int requestID, string securityID, DateTime fromdateActual, DateTime todateActual, string reqStatus, string strSP)
            {
                DataSet dsreturnSet = new DataSet();
                string sqlGetEmployeeInfoproc = "VerifySafetyPermitId_LocationId";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                   
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "RequestID", SqlDbType.Int, requestID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "SecurityID", SqlDbType.VarChar, securityID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "ReqStatus", SqlDbType.VarChar, reqStatus);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "FromDate", SqlDbType.DateTime, fromdateActual);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "ToDate", SqlDbType.DateTime, todateActual);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "IsSP", SqlDbType.VarChar, strSP);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        // sqlConn.AddInParameter(dbEmployeeInfoComm, "VisitDetailsID", SqlDbType.Int, VisitDetailsId);
                        dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                    return dsreturnSet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="securityID">security ID</param>
            /// <param name="reqStatus">request Status</param>     
            /// <param name="requestID">request ID</param>  
            /// <returns>data set</returns> 
            public DataSet SafetyPermitSecuritySearch(string securityID, string reqStatus, string requestID)
            {
                DataSet dsreturnSet = new DataSet();
                string sqlGetEmployeeInfoproc = "SafetyPermitSecuritySearch";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                   
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                   
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "SearchValue", SqlDbType.VarChar, requestID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "AssociateID", SqlDbType.VarChar, securityID);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "ReqStatus", SqlDbType.VarChar, reqStatus);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        // sqlConn.AddInParameter(dbEmployeeInfoComm, "VisitDetailsID", SqlDbType.Int, VisitDetailsId);
                        dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                    return dsreturnSet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="strpermitId">permit id</param>  
            /// <param name="reqstatus">request Status</param>  
            /// <param name="currentSystemTime">current System Time</param>              
            public void PermitCheckOut(string strpermitId, string reqstatus, DateTime currentSystemTime)
            {
                string sqlGetEmployeeInfoproc = "SubmitWorkPermitCheckOut";
                try
                {
                    int permitId = Convert.ToInt32(strpermitId);
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                   
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "PermitId", SqlDbType.Int, permitId);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "Date", SqlDbType.DateTime, currentSystemTime);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "Status", SqlDbType.VarChar, reqstatus);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="strpermitId">permit id</param>  
            /// <param name="reqstatus">request Status</param>             
            /// <param name="currentSystemTime">current System Time</param>  
            public void PermitCheckIn(string strpermitId, string reqstatus, DateTime currentSystemTime)
            {
                string sqlGetEmployeeInfoproc = "SubmitWorkPermitCheckin";
                try
                {
                    int permitId = Convert.ToInt32(strpermitId);
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                   
                        dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbemployeeInfoComm, "PermitId", SqlDbType.Int, permitId);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "Date", SqlDbType.DateTime, currentSystemTime);
                        sqlConn.AddInParameter(dbemployeeInfoComm, "Status", SqlDbType.VarChar, reqstatus);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbemployeeInfoComm))
                    {
                        sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                    }
                    dbemployeeInfoComm.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// cancel request
            /// </summary>
            /// <param name="requestID">request id</param>  
            /// <returns>integer value</returns> 
            public int? CancelRequest(int requestID)
            {
                int? success = 1;
                try
                {
                    SqlConnection connection = new SqlConnection(vmsConn);
                    connection.SetAccessToken();
                    using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                    {
                        vmsdb.UpdateStatus(requestID, ref success);
                    }
                    connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return success;
            }

            /// <summary>
            /// View Log By Security
            /// </summary>
            /// <param name="strSearchString">string Search String</param>  
            /// <param name="strUserID">user id</param>  
            /// <param name="fromdate">from date</param>  
            ///  <param name="todate">to date</param>  
            ///   <param name="reqStatus">request Status</param>  
            /// <returns>data set</returns> 
            public DataSet ViewLogBySecurity(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus,string Expresscheckin,string VCardNumber)
            {
                DataSet dsresult = new DataSet();
                string searchDetails = "ViewLogBySecurityBasedLocationId";
                try
                {
                    if (string.IsNullOrEmpty(reqStatus))
                    {
                        reqStatus = null;
                    }

                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbsearchRequest = sqlConn.GetStoredProcCommand(searchDetails);
                    dbsearchRequest.CommandType = CommandType.StoredProcedure;
                    
                        sqlConn.AddInParameter(dbsearchRequest, "AssociateID", SqlDbType.VarChar, strUserID);
                        sqlConn.AddInParameter(dbsearchRequest, "SearchValue", SqlDbType.VarChar, strSearchString);
                        sqlConn.AddInParameter(dbsearchRequest, "ReqStatus", SqlDbType.VarChar, reqStatus);
                        sqlConn.AddInParameter(dbsearchRequest, "FromDate", SqlDbType.DateTime, fromdate);
                        sqlConn.AddInParameter(dbsearchRequest, "ToDate", SqlDbType.DateTime, todate);
                        sqlConn.AddInParameter(dbsearchRequest, "ExpressCheckin", SqlDbType.VarChar, Expresscheckin);
                        sqlConn.AddInParameter(dbsearchRequest, "VCardNumber", SqlDbType.VarChar, VCardNumber);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbsearchRequest))
                    {
                        dsresult = sqlConn.ExecuteDataSet(dbsearchRequest);
                    }
                    dbsearchRequest.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dsresult;
            }

            /// <summary>
            /// View Card Log By Security
            /// </summary>
            /// <param name="strSearchString">string Search String</param>  
            /// <param name="strUserID">user id</param>  
            /// <param name="fromdate">from date</param>  
            ///  <param name="todate">to date</param>  
            ///   <param name="reqStatus">request Status</param>  
            /// <returns>data set</returns> 
            public DataSet ViewCardLogBySecurity(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus)
            {
                DataSet dsresult = new DataSet();
                string searchDetails = "ViewCardLogBySecurity";
                try
                {
                    if (string.IsNullOrEmpty(reqStatus))
                    {
                        reqStatus = null;
                    }

                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbsearchRequest = sqlConn.GetStoredProcCommand(searchDetails);
                    
                        dbsearchRequest.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbsearchRequest, "AssociateID", SqlDbType.VarChar, strUserID);
                        sqlConn.AddInParameter(dbsearchRequest, "SearchValue", SqlDbType.VarChar, strSearchString);
                        sqlConn.AddInParameter(dbsearchRequest, "ReqStatus", SqlDbType.VarChar, reqStatus);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbsearchRequest))
                    {
                        dsresult = sqlConn.ExecuteDataSet(dbsearchRequest);
                    }
                    dbsearchRequest.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dsresult;
            }



            /// <summary>
            /// View Log By Security
            /// </summary>
            /// <param name="strSearchString">string Search String</param>  
            /// <param name="strUserID">user id</param>  
            /// <param name="fromdate">from date</param>  
            ///  <param name="todate">to date</param>  
            ///   <param name="reqStatus">request Status</param>  
            /// <returns>data set</returns> 
            public DataSet ViewLogBySecurityClients(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus)
            {
                DataSet dsresult = new DataSet();
                string searchDetails = "ViewLogBySecurityBasedLocationId_Clients";
                try
                {
                    if (string.IsNullOrEmpty(reqStatus))
                    {
                        reqStatus = null;
                    }

                    if (reqStatus == "Dispatched")
                    {
                        reqStatus = "IN";
                    }

                    if (reqStatus == "To Be Processed")
                    {
                        reqStatus = "Yet to arrive";
                    }

                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbsearchRequest = sqlConn.GetStoredProcCommand(searchDetails);
                    dbsearchRequest.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dbsearchRequest, "AssociateID", SqlDbType.VarChar, strUserID);
                    sqlConn.AddInParameter(dbsearchRequest, "SearchValue", SqlDbType.VarChar, strSearchString);
                    sqlConn.AddInParameter(dbsearchRequest, "ReqStatus", SqlDbType.VarChar, reqStatus);
                    sqlConn.AddInParameter(dbsearchRequest, "FromDate", SqlDbType.DateTime, fromdate);
                    sqlConn.AddInParameter(dbsearchRequest, "ToDate", SqlDbType.DateTime, todate);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbsearchRequest))
                    {
                        dsresult = sqlConn.ExecuteDataSet(dbsearchRequest);
                    }
                    dbsearchRequest.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
                return dsresult;
            }

            /// <summary>
            /// View Log By Security
            /// </summary>
            ///   <param name="parentID">parent ID</param>  
            /// <returns>data set</returns> 
            public DataSet GetFirstClientVisitFacility(int parentID)
            {
                DataSet dsresult = new DataSet();
                string searchDetails = "GetFirstClientVisitFacility";
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbsearchRequest = sqlConn.GetStoredProcCommand(searchDetails);
                    
                        dbsearchRequest.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbsearchRequest, "parentreferenceid", SqlDbType.Int, parentID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbsearchRequest))
                    {
                        dsresult = sqlConn.ExecuteDataSet(dbsearchRequest);
                    }
                    dbsearchRequest.Connection?.Close();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
                return dsresult;
            }

            /// <summary>
            /// View Log By Security
            /// </summary>
            ///   <param name="parentID">parent ID</param>  
            /// <returns>data set</returns> 
            public DataSet GetAllVisitorList(int parentID)
            {
                DataSet dsresult = new DataSet();
                string searchDetails = "GetAllVisitorlist";
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbsearchRequest = sqlConn.GetStoredProcCommand(searchDetails);
                    
                        dbsearchRequest.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbsearchRequest, "parentID", SqlDbType.Int, parentID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbsearchRequest))
                    {
                        dsresult = sqlConn.ExecuteDataSet(dbsearchRequest);
                    }
                    dbsearchRequest.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
                return dsresult;
            }

            /// <summary>
            /// View Log By Security for SP
            /// </summary>
            /// <param name="strSearchString">string Search String</param>  
            /// <param name="strUserID">user id</param>  
            /// <param name="fromdate">from date</param>  
            /// <param name="todate">to date</param>  
            /// <param name="reqStatus">request Status</param> 
            /// <param name="strSP">stored procedure string</param> 
            /// <returns>data set</returns> 
            public DataSet ViewLogBySecurityforSP(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus, string strSP)
            {
                DataSet dsresult = new DataSet();
                string searchDetails = "ViewLogBySecurityforSP";
                try
                {
                    if (string.IsNullOrEmpty(reqStatus))
                    {
                        reqStatus = null;
                    }

                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbsearchRequest = sqlConn.GetStoredProcCommand(searchDetails);
                   
                        dbsearchRequest.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbsearchRequest, "AssociateID", SqlDbType.VarChar, strUserID);
                        sqlConn.AddInParameter(dbsearchRequest, "SearchValue", SqlDbType.VarChar, strSearchString);
                        sqlConn.AddInParameter(dbsearchRequest, "ReqStatus", SqlDbType.VarChar, reqStatus);
                        sqlConn.AddInParameter(dbsearchRequest, "FromDate", SqlDbType.DateTime, fromdate);
                        sqlConn.AddInParameter(dbsearchRequest, "ToDate", SqlDbType.DateTime, todate);
                        sqlConn.AddInParameter(dbsearchRequest, "IsSP", SqlDbType.VarChar, strSP);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbsearchRequest))
                    {
                        dsresult = sqlConn.ExecuteDataSet(dbsearchRequest);
                    }
                    dbsearchRequest.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dsresult;
            }



            /// <summary>
            /// View Log By Security for SP Work Details
            /// </summary>
            /// <param name="strUserID">User ID</param>  
            /// <param name="reqStatus">request status</param>  
            /// <returns>data set</returns> 
            public DataSet ViewLogBySecurityforSPWorkDetails(string strUserID, string reqStatus)
            {
                DataSet dsresult = new DataSet();
                string searchDetails = "ViewLogBySecurityforSPWorkDetails";
                try
                {
                    if (string.IsNullOrEmpty(reqStatus))
                    {
                        reqStatus = null;
                    }

                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbsearchRequest = sqlConn.GetStoredProcCommand(searchDetails);
                    
                        dbsearchRequest.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbsearchRequest, "AssociateID", SqlDbType.VarChar, strUserID);

                        // sqlConn.AddInParameter(dbSearchRequest, "SearchValue", SqlDbType.VarChar, strSearchString);
                        sqlConn.AddInParameter(dbsearchRequest, "ReqStatus", SqlDbType.VarChar, reqStatus);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbsearchRequest))
                    {
                        dsresult = sqlConn.ExecuteDataSet(dbsearchRequest);
                    }
                    dbsearchRequest.Connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return dsresult;
            }

            /// <summary>
            /// Get Visitor offset
            /// </summary>
            /// <param name="requestId">request id</param>
            /// <returns>string value</returns>
            public string GetVisitorOffset(string requestId)
            {
                SqlConnection con = new SqlConnection(DBConnection.Connectionstring());
                try
                {
                    // GetVisitorOffset                    
                    SqlCommand cmd = new SqlCommand("GetVisitorOffset", con);
                    string offset = string.Empty;
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        cmd.CommandType = CommandType.StoredProcedure;
#pragma warning disable CS0618 // 'SqlParameterCollection.Add(string, object)' is obsolete: 'Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202'
                        cmd.Parameters.Add("@RequestID", requestId);
#pragma warning restore CS0618 // 'SqlParameterCollection.Add(string, object)' is obsolete: 'Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202'
                        IDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            offset = Convert.ToString(reader["Offset"]);
                        }
                    }

                    return offset;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="firstname">first name</param>  
            /// <param name="lastname">last name</param>  
            /// <param name="company">value for </param>  
            /// <param name="designation">value for designation</param>  
            /// <param name="nativecountry">value for native country</param>  
            /// <param name="purpose">value for purpose</param>  
            /// <param name="city">value for city</param>  
            /// <param name="facility">value for facility</param>  
            /// <param name="badgeno">value for badge number</param>  
            /// <param name="status">value for status</param>  
            /// <param name="host">value for host</param>  
            /// <param name="fromdate">value for from date</param>  
            /// <param name="todate">value for to date</param>  
            /// <param name="mobileNo">value for mobile</param>  
            /// <param name="date">value for date</param>  
            /// <param name="fromtime">value for from time</param>  
            /// <param name="totime">value for to time</param>  
            /// <param name="dept">value for department</param>  
            /// <param name="statusflag">value for status flag</param>  
            /// <returns>data set</returns> 
            [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmantainableCode", Justification = "Reviewed")]
            public DataSet SearchVisitorInfo(string firstname, string lastname, string company, string designation, string nativecountry, string purpose, string city, string facility, string badgeno, string status, string host, string fromdate, string todate, string mobileNo, string date, string fromtime, string totime, string dept, string statusflag)
            {
                DataSet set = new DataSet();
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;
                    SqlParameter paramfirstname;
                    SqlParameter paramlastname;
                    SqlParameter paramcompany;
                    SqlParameter paramdesignation;
                    SqlParameter paramnativecountry;
                    SqlParameter parampurpose;
                    SqlParameter paramvisitingcity;
                    SqlParameter paramvisitingfacility;
                    SqlParameter parambadgeno;
                    SqlParameter paramhostname;
                    SqlParameter parambadgestatus;
                    SqlParameter paramfromdate;
                    SqlParameter paramtodate;
                    SqlParameter paramdate;
                    SqlParameter paramfromtime;
                    SqlParameter paramtotime;
                    SqlParameter paramdept;
                    SqlParameter paramstatusflag;
                    SqlParameter paramMobileNo;
                    SqlDataAdapter adp;

                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    cmd = new SqlCommand("ViewDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // added by uma on 22/aug/09
                    if (string.IsNullOrEmpty(firstname))
                    {
                        paramfirstname = new SqlParameter("@FirstName", SqlDbType.VarChar, 50);
                        paramfirstname.Value = null;
                        cmd.Parameters.Add(paramfirstname);
                    }
                    else
                    {
                        paramfirstname = new SqlParameter("@FirstName", SqlDbType.VarChar, 50);
                        paramfirstname.Value = firstname;
                        cmd.Parameters.Add(paramfirstname);
                    }

                    if (string.IsNullOrEmpty(lastname))
                    {
                        paramlastname = new SqlParameter("@LastName", SqlDbType.VarChar, 50);
                        paramlastname.Value = null;
                        cmd.Parameters.Add(paramlastname);
                    }
                    else
                    {
                        paramlastname = new SqlParameter("@LastName", SqlDbType.VarChar, 50);
                        paramlastname.Value = lastname;
                        cmd.Parameters.Add(paramlastname);
                    }

                    if (string.IsNullOrEmpty(company))
                    {
                        paramcompany = new SqlParameter("@Company", SqlDbType.VarChar, 50);
                        paramcompany.Value = null;
                        cmd.Parameters.Add(paramcompany);
                    }
                    else
                    {
                        paramcompany = new SqlParameter("@Company", SqlDbType.VarChar, 50);
                        paramcompany.Value = company;
                        cmd.Parameters.Add(paramcompany);
                    }

                    if (string.IsNullOrEmpty(designation))
                    {
                        paramdesignation = new SqlParameter("@Designation", SqlDbType.VarChar, 50);
                        paramdesignation.Value = null;
                        cmd.Parameters.Add(paramdesignation);
                    }
                    else
                    {
                        paramdesignation = new SqlParameter("@Designation", SqlDbType.VarChar, 50);
                        paramdesignation.Value = designation;
                        cmd.Parameters.Add(paramdesignation);
                    }

                    if (nativecountry == "Select" || string.IsNullOrEmpty(nativecountry))
                    {
                        paramnativecountry = new SqlParameter("@NativeCountry", SqlDbType.VarChar, 50);
                        paramnativecountry.Value = null;
                        cmd.Parameters.Add(paramnativecountry);
                    }
                    else
                    {
                        paramnativecountry = new SqlParameter("@NativeCountry", SqlDbType.VarChar, 50);
                        paramnativecountry.Value = nativecountry;
                        cmd.Parameters.Add(paramnativecountry);
                    }

                    if (purpose == "Select Purpose" || string.IsNullOrEmpty(purpose))
                    {
                        parampurpose = new SqlParameter("@Purpose", SqlDbType.VarChar, 50);
                        parampurpose.Value = null;
                        cmd.Parameters.Add(parampurpose);
                    }
                    else
                    {
                        parampurpose = new SqlParameter("@Purpose", SqlDbType.VarChar, 50);
                        parampurpose.Value = purpose;
                        cmd.Parameters.Add(parampurpose);
                    }

                    if (city == "Select" || city == null)
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = null;
                        cmd.Parameters.Add(paramvisitingcity);
                    }
                    else
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = city;
                        cmd.Parameters.Add(paramvisitingcity);
                    }

                    if (facility == "Select" || string.IsNullOrEmpty(facility))
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = null;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }
                    else
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = facility;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }

                    if (string.IsNullOrEmpty(badgeno))
                    {
                        parambadgeno = new SqlParameter("@BadgeNo", SqlDbType.VarChar, 50);
                        parambadgeno.Value = null;
                        cmd.Parameters.Add(parambadgeno);
                    }
                    else
                    {
                        parambadgeno = new SqlParameter("@BadgeNo", SqlDbType.VarChar, 50);
                        parambadgeno.Value = badgeno;
                        cmd.Parameters.Add(parambadgeno);
                    }

                    if (string.IsNullOrEmpty(host))
                    {
                        paramhostname = new SqlParameter("@HostName", SqlDbType.VarChar, 50);
                        paramhostname.Value = null;
                        cmd.Parameters.Add(paramhostname);
                    }
                    else
                    {
                        paramhostname = new SqlParameter("@HostName", SqlDbType.VarChar, 50);
                        paramhostname.Value = host;
                        cmd.Parameters.Add(paramhostname);
                    }

                    if (string.IsNullOrEmpty(status) || status.Equals("Select"))
                    {
                        parambadgestatus = new SqlParameter("@Status", SqlDbType.VarChar, 50);
                        parambadgestatus.Value = null;
                        cmd.Parameters.Add(parambadgestatus);
                    }
                    else
                    {
                        parambadgestatus = new SqlParameter("@Status", SqlDbType.VarChar, 50);
                        parambadgestatus.Value = status;
                        cmd.Parameters.Add(parambadgestatus);
                    }

                    if (fromdate == null || string.IsNullOrEmpty(fromdate.Trim()) || fromdate.Trim() == "__/__/____")
                    {
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = null;
                        cmd.Parameters.Add(paramfromdate);
                    }
                    else
                    {
                        string[] fromDate = fromdate.Split('/');
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = fromDate[2] + "-" + fromDate[1] + "-" + fromDate[0];
                        cmd.Parameters.Add(paramfromdate);
                    }

                    if (string.IsNullOrEmpty(todate) || todate.Trim() == "__/__/____")
                    {
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = null;
                        cmd.Parameters.Add(paramtodate);
                    }
                    else
                    {
                        string[] todates = todate.Split('/');
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = todates[2] + "-" + todates[1] + "-" + todates[0];
                        cmd.Parameters.Add(paramtodate);
                    }

                    if (string.IsNullOrEmpty(date))
                    {
                        paramdate = new SqlParameter("@Date", SqlDbType.VarChar, 50);
                        paramdate.Value = null;
                        cmd.Parameters.Add(paramdate);
                    }
                    else
                    {
                        string[] dates = date.Split('/');
                        paramdate = new SqlParameter("@Date", SqlDbType.VarChar, 50);
                        paramdate.Value = dates[2] + "-" + dates[1] + "-" + dates[0];
                        cmd.Parameters.Add(paramdate);
                    }

                    if (string.IsNullOrEmpty(fromtime))
                    {
                        paramfromtime = new SqlParameter("@FromTime", SqlDbType.VarChar, 50);
                        paramfromtime.Value = null;
                        cmd.Parameters.Add(paramfromtime);
                    }
                    else
                    {
                        paramfromtime = new SqlParameter("@FromTime", SqlDbType.VarChar, 50);
                        paramfromtime.Value = fromtime;
                        cmd.Parameters.Add(paramfromtime);
                    }

                    if (string.IsNullOrEmpty(totime))
                    {
                        paramtotime = new SqlParameter("@ToTime", SqlDbType.VarChar, 50);
                        paramtotime.Value = null;
                        cmd.Parameters.Add(paramtotime);
                    }
                    else
                    {
                        paramtotime = new SqlParameter("@ToTime", SqlDbType.VarChar, 50);
                        paramtotime.Value = totime;
                        cmd.Parameters.Add(paramtotime);
                    }

                    if (dept == null || dept == "Select")
                    {
                        paramdept = new SqlParameter("@HostDept", SqlDbType.VarChar, 50);
                        paramdept.Value = null;
                        cmd.Parameters.Add(paramdept);
                    }
                    else
                    {
                        paramdept = new SqlParameter("@HostDept", SqlDbType.VarChar, 50);
                        paramdept.Value = dept;
                        cmd.Parameters.Add(paramdept);
                    }

                    if (statusflag == null || statusflag == "Select")
                    {
                        paramstatusflag = new SqlParameter("@Statusflag", SqlDbType.VarChar, 50);
                        paramstatusflag.Value = null;
                        cmd.Parameters.Add(paramstatusflag);
                    }
                    else
                    {
                        paramstatusflag = new SqlParameter("@Statusflag", SqlDbType.VarChar, 50);
                        paramstatusflag.Value = statusflag;
                        cmd.Parameters.Add(paramstatusflag);
                    }

                    // added by Vimal on 03/Mar/11----------Begin.
                    if (string.IsNullOrEmpty(mobileNo))
                    {
                        paramMobileNo = new SqlParameter("@MobileNo", SqlDbType.VarChar, 20);
                        paramMobileNo.Value = null;
                        cmd.Parameters.Add(paramMobileNo);
                    }
                    else
                    {
                        paramMobileNo = new SqlParameter("@MobileNo", SqlDbType.VarChar, 20);
                        paramMobileNo.Value = mobileNo;
                        cmd.Parameters.Add(paramMobileNo);
                    }

                    // added by Vimal on 03/Mar/11----------End.
                    // added by uma on 22/aug/09----------End.
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                    return set;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Applicant Details
            /// </summary>
            /// <param name="applicantID">applicant ID</param>  
            /// <returns>data set</returns> 
            #region ApplicantDetails
            public DataSet GetApplicantDetails(string applicantID)
            {
                DataSet dsapplicantDetails = new DataSet();
                string sqlGetApplicantInfoproc = "IVS_GetApplicantDetails";
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                    dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                   
                        sqlConn.AddInParameter(dbapplicantInfoComm, "Applicant_ID", SqlDbType.VarChar, applicantID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsapplicantDetails = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                        sqlConn = null;
                    }
                    dbapplicantInfoComm.Connection.Close();
                    return dsapplicantDetails;
                }
                catch
                {
                    throw;
                }
            }
            #endregion

            #region GetIDCardDetails
            /// <summary>
            /// Get ID Card Details
            /// </summary>
            /// <param name="associateID">associate ID</param>  
            /// <param name="location">location detail</param>   
            /// <returns>data set</returns> 
            public DataSet GetIDCardDetails(string associateID, string location)
            {
                DataSet dsidCardDetails = new DataSet();

                string sqlGetApplicantInfoproc = "IVS_GetIDCardDetails";
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                   
                        dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbapplicantInfoComm, "ASSOCIATEID", SqlDbType.VarChar, associateID);
                        sqlConn.AddInParameter(dbapplicantInfoComm, "LOCATION", SqlDbType.VarChar, location);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsidCardDetails = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                         
                    }
                    dbapplicantInfoComm.Connection.Close();
                    return dsidCardDetails;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dsidCardDetails = null;
                }
            }
            #endregion

            #region GetIDCardLocation
            /// <summary>
            /// Get ID Card Details
            /// </summary>
            /// <param name="adminID">admin ID</param>  
            /// <returns>data set</returns> 
            public DataSet GetIDCardLocation(string adminID)
            {
                DataSet dsidCardLocation = new DataSet();
                string location = string.Empty;
                string locaionID = string.Empty;
                string sqlGetApplicantInfoproc = "IVS_GetIDCardAdminLocation";
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                    
                        
                            dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                            sqlConn.AddInParameter(dbapplicantInfoComm, "AdminID", SqlDbType.VarChar, adminID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsidCardLocation = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                            
                            location = dsidCardLocation.Tables[1].Rows[0]["LocationID"].ToString().Trim();
                         
                    }
                    sqlConn = null;
                    dbapplicantInfoComm.Connection.Close();
                    return dsidCardLocation;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dsidCardLocation = null;
                    location = null;
                }
            }

            #endregion

            #region IDCardDetails
            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="adminID">admin ID</param>  
            /// <param name="associateID">associate ID</param>  
            /// <param name="city">city detail</param>  
            /// <returns>data set</returns> 
            public DataTable InsertIDCardDetails(string adminID, string associateID, int city)
            {
                DataSet dsinsert = null;
                DataTable dtinsert = null;
                try
                {
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand("IVS_InsertIDCardDetails");
                   
                        
                            dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;

                            sqlConn.AddInParameter(dbapplicantInfoComm, "AdminID", SqlDbType.VarChar, adminID);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "Associate_ID", SqlDbType.VarChar, associateID);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "IssuedLocation", SqlDbType.Int, city);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsinsert = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                            dtinsert = dsinsert.Tables[0];
                        
                    }
                    dbapplicantInfoComm.Connection?.Close();
                    sqlConn = null;
                    
                    return dtinsert;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get contractor details
            /// </summary>
            /// <param name="contractorId">contractor Id</param>
            /// <returns>Data table</returns>
            public DataTable PreviewContractorDetails(string contractorId)
            {
                DataTable dtdisplay = new DataTable();
                try
                {
                    string sqlGetApplicantInfoproc = "IVS_GetIDCardContractorDetails";
                    try
                    {
                        SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                        DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                       
                            
                                dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                                sqlConn.AddInParameter(dbapplicantInfoComm, "ContractorId", SqlDbType.VarChar, contractorId);
                        using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                        {
                            DataSet dsdisplay = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                                dtdisplay = dsdisplay.Tables[0];
                            
                            
                        }
                        dbapplicantInfoComm.Connection?.Close();
                        return dtdisplay;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        dtdisplay = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get Associate preview details
            /// </summary>
            /// <param name="associateID">associate id</param>
            /// <returns>data table</returns>
            public DataTable PreviewDetails(string associateID)
            {
                DataTable dtdisplay;
                DataSet dsdisplay;
                int checkFlag = 1;
                try
                {
                    string sqlGetApplicantInfoproc = "IVS_GetIDCardDetailsForSAN";

                    try
                    {
                        SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                        string appTemplateID = System.Configuration.ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                        DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                        
                            dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                            sqlConn.AddInParameter(dbapplicantInfoComm, "AssociateID", SqlDbType.VarChar, associateID);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "AppTemplateID", SqlDbType.VarChar, appTemplateID);
                            sqlConn.AddOutParameter(dbapplicantInfoComm, "CheckFlag", SqlDbType.Int, 500);
                        using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                        {
                            dsdisplay = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                            checkFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbapplicantInfoComm, "CheckFlag"));

                            if (checkFlag == 1)
                            {
                                dtdisplay = dsdisplay.Tables[0];
                                dbapplicantInfoComm.Connection?.Close();
                                return dtdisplay;
                            }
                            else
                            {
                                dbapplicantInfoComm.Connection?.Close();
                                return null;
                            }
                           
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        
                        dtdisplay = null;
                        
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Contractor details ready for ID printing
            /// </summary>
            /// <param name="searchKey">search Key</param>
            /// <param name="locationId">location Id</param>
            /// <returns>data table</returns>
            public DataTable GetIDCardContractorDetails(string searchKey, string locationId)
            {
                DataTable dtdisplay;
                DataSet dsdisplay;
                try
                {
                    string sqlGetApplicantInfoproc = "SearchContractor";
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                  
                        dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbapplicantInfoComm, "SearchValue", SqlDbType.VarChar, searchKey);
                        sqlConn.AddInParameter(dbapplicantInfoComm, "LocationId", SqlDbType.VarChar, locationId);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsdisplay = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                        if (dsdisplay.Tables.Count > 0)
                        {
                            dtdisplay = dsdisplay.Tables[0];
                        }
                        else
                        {
                            dtdisplay = null;
                        }

                        
                    }
                    dbapplicantInfoComm.Connection?.Close();
                    return dtdisplay;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get ID Card Contractor Details By Id
            /// </summary>
            /// <param name="contractorId">contractor Id</param>
            /// <returns>Data Table</returns>
            public DataTable GetIDCardContractorDetailsById(int contractorId)
            {
                DataTable dtdisplay;
                DataSet dsdisplay;
                try
                {
                    string sqlGetApplicantInfoproc = "SearchContractorById";
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                  
                        dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbapplicantInfoComm, "Id", SqlDbType.Int, contractorId);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsdisplay = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                        if (dsdisplay.Tables.Count > 0)
                        {
                            dtdisplay = dsdisplay.Tables[0];
                        }
                        else
                        {
                            dtdisplay = null;
                        }

                        
                    }
                    dbapplicantInfoComm.Connection?.Close();
                    return dtdisplay;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Vendor Name
            /// </summary>
            /// <param name="sname">request name</param>  
            /// <returns>data set</returns> 
            public List<string> GetVendorName(string sname)
            {
                SqlAzureDatabase sqlConn;
                DbCommand dbcmd;
                List<string> customers = null;

                try
                {
                    string sqlGetApplicantInfoproc = "GetVendorNamebyName";
                    sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    dbcmd = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                    
                    dbcmd.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dbcmd, "@Searchkey", SqlDbType.VarChar, sname);
                    customers = new List<string>();
                    using (IDataReader dbreader = sqlConn.ExecuteReader(dbcmd))
                    {
                        while (dbreader.Read())
                        {
                            customers.Add(dbreader.GetString(0));
                        }
                    }
                    dbcmd.Connection?.Close();
                    return customers;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                 
                    sqlConn = null;
                    dbcmd = null;
                }
            }

            /// <summary>
            /// To get location details
            /// </summary>
            /// <param name="hostId">host Id</param>
            /// <returns>data table</returns>
            public DataTable GetIDCardLocationDetails(string hostId)
            {
                DataSet dsdisplay;
                try
                {
                    string sqlGetApplicantInfoproc = "GetUserLocationDetails";
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                    
                        dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbapplicantInfoComm, "UserID", SqlDbType.VarChar, hostId);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                    {
                        dsdisplay = sqlConn.ExecuteDataSet(dbapplicantInfoComm);

                        
                    }
                    dbapplicantInfoComm.Connection?.Close();
                    return dsdisplay.Tables[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Associates details ready for ID printing
            /// </summary>
            /// <param name="fromDate">from Date</param>
            /// <param name="todate">to Date</param>
            /// <param name="selectedCity">selected City</param>
            /// <param name="status">value status</param>
            /// <returns>Data table</returns>
            public DataTable GetIDCardAssociateDetails(DateTime fromDate, DateTime todate, string selectedCity, string status)
            {
                DataTable dtdisplay;
                DataSet dsdisplay;
                try
                {
                    if (Convert.ToInt32(status) == 1)
                    {
                        string sqlGetApplicantInfoproc = "IVS_GetIDCardAssociateDetails";
                        string appTemplateID = ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                        SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                        DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                      
                            dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                            dbapplicantInfoComm.CommandTimeout = 1000;
                            sqlConn.AddInParameter(dbapplicantInfoComm, "FromDate", SqlDbType.Date, fromDate);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "ToDate", SqlDbType.Date, todate);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "City", SqlDbType.VarChar, selectedCity);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "AppTemplateID", SqlDbType.VarChar, appTemplateID);
                        using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                        {
                            dsdisplay = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                            if (dsdisplay.Tables.Count > 0)
                            {
                                dtdisplay = dsdisplay.Tables[0];
                            }
                            else
                            {
                                dtdisplay = null;
                            }

                           
                        }
                        dbapplicantInfoComm.Connection?.Close();
                        return dtdisplay;
                    }
                    else
                    {
                        string sqlGetApplicantInfoproc = "IVS_GetIDCardMissingInformation";
                        SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                        DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                       
                            dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                            dbapplicantInfoComm.CommandTimeout = 1000;
                            sqlConn.AddInParameter(dbapplicantInfoComm, "FromDate", SqlDbType.Date, fromDate);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "ToDate", SqlDbType.Date, todate);
                            sqlConn.AddInParameter(dbapplicantInfoComm, "City", SqlDbType.VarChar, selectedCity);
                        using (DataSet dbreader = sqlConn.ExecuteDataSet(dbapplicantInfoComm))
                        {
                            dsdisplay = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                            if (dsdisplay.Tables.Count > 0)
                            {
                                dtdisplay = dsdisplay.Tables[0];
                            }
                            else
                            {
                                dtdisplay = null;
                            }

                            
                        }
                        dbapplicantInfoComm.Connection?.Close();
                        return dtdisplay;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            #endregion

            #region ValidateCandidateId
            /// <summary>
            /// Validate Candidate ID
            /// </summary>
            /// <param name="applicantID">applicant ID</param>  
            /// <returns>data set</returns> 
            public bool ValidateCandidatID(string applicantID)
            {
                bool result = false;
                SqlDataReader dr = null;
                SqlCommand cmd;
                SqlConnection con = null;
                SqlParameter paramRequestId;

                try
                {
                    string conn = System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    cmd = new SqlCommand("ValidateCandidateID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    paramRequestId = new SqlParameter("@ApplicantID", SqlDbType.VarChar, 50);
                    paramRequestId.Value = applicantID;
                    cmd.Parameters.Add(paramRequestId);
                    con.OpenWithMSI(); //// con.Open();  
                    dr = (SqlDataReader)cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        object[] values = new object[dr.FieldCount];

                        // Get the column values into the array
                        dr.GetValues(values);
                        result = Convert.ToBoolean(values[0].ToString());
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Dispose();
                    }

                    con.Close();
                    con.Dispose();
                }
            }
            #endregion

            #region CheckApplicantDetails
            /// <summary>
            /// Check Applicant Details
            /// </summary>
            /// <param name="applicantID">applicant ID</param>  
            /// <returns>data set</returns> 
            public DataSet CheckApplicantDetails(string applicantID)
            {
                DataSet ds = new DataSet();
                try
                {
                    string sqlUpdateVisitorImg = string.Empty;
                    sqlUpdateVisitorImg = "IVS_CheckCandidateDetails";
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbloginUserNamecmd = sqlConn.GetStoredProcCommand(sqlUpdateVisitorImg);
                    
                        sqlConn.AddInParameter(dbloginUserNamecmd, "CandidateID", SqlDbType.VarChar, applicantID);
                        dbloginUserNamecmd.CommandType = CommandType.StoredProcedure;
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbloginUserNamecmd))
                    {
                        ds = sqlConn.ExecuteDataSet(dbloginUserNamecmd);                      
                    }
                    dbloginUserNamecmd.Connection.Close();
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            #endregion

            #region ApplicantImageUpload

            /// <summary>
            /// Insert Applicant image
            /// </summary>
            /// <param name="applicantID">applicant ID</param>  
            /// <param name="strEncryptedBinaryData">Encrypted Binary Data</param>  
            /// <param name="strbloodGroup">string blood Group</param> 
            /// <param name="emergencyContact">emergency Contact</param>  
            /// <param name="location">location value</param>  
            /// <param name="adminID">admin ID</param>  
            /// <returns>integer value</returns> 
            public int InsertApplicantImgInDB(string applicantID, string strEncryptedBinaryData, string strbloodGroup, string emergencyContact, string location, string adminID)
            {
                DataSet ds = new DataSet();
                int n = 0;
                try
                {
                    string sqlUpdateVisitorImg = string.Empty;
                    sqlUpdateVisitorImg = "InsertApplicantImgInDB";
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    DbCommand dbloginUserNamecmd = sqlConn.GetStoredProcCommand(sqlUpdateVisitorImg);
                   
                        sqlConn.AddInParameter(dbloginUserNamecmd, "ApplicantID", SqlDbType.VarChar, applicantID);
                        sqlConn.AddInParameter(dbloginUserNamecmd, "Photo", SqlDbType.VarChar, strEncryptedBinaryData);
                        sqlConn.AddInParameter(dbloginUserNamecmd, "BloodGroup", SqlDbType.VarChar, strbloodGroup);
                        sqlConn.AddInParameter(dbloginUserNamecmd, "EmergencyContact", SqlDbType.VarChar, emergencyContact);
                        sqlConn.AddInParameter(dbloginUserNamecmd, "Location", SqlDbType.VarChar, location);
                        sqlConn.AddInParameter(dbloginUserNamecmd, "adminID", SqlDbType.VarChar, adminID);
                        dbloginUserNamecmd.CommandType = CommandType.StoredProcedure;
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbloginUserNamecmd))
                    {
                        ds = sqlConn.ExecuteDataSet(dbloginUserNamecmd);
                        if (ds.Tables.Count != 0)
                        {
                            n = Convert.ToInt32(ds.Tables[0].Rows[0]["Result"].ToString().Trim());
                        }
                    }
                    dbloginUserNamecmd.Connection.Close();
                    return n;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (NullReferenceException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            #endregion
            /// <summary>
            /// Description: method return configurable day field for city to raise request in advance
            /// </summary>
            /// Added by priti on 3rd June for VMS CR VMS31052010CR6
            /// <param name="purpose">purpose name</param>
            /// <returns>Number of days allowable to raise request in advance </returns>
            public int GetAdvanceAllowabledays(string purpose)
            {
                SqlCommand cmd;
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                int advanceAllowabledays;
                try
                {
                    cmd = new SqlCommand("AdvanceAllowabledays", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@visitortype", SqlDbType.VarChar, 50).Value = purpose;
                    conn.OpenWithMSI();
                    return advanceAllowabledays = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    conn?.Close();
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <returns>data list</returns> 
            public List<int> MultipleVisitors()
            {
                SqlConnection con;
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                try
                {
                    List<int> visitorID = new List<int>();
                    SqlDataReader sdr;
                    SqlCommand cmd;

                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    // changed by Priti 22 aug.                
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand("Select VisitorID from VisitorRequest where Status!='Saved' group by VisitorID having count(VisitorID)>3 ", con);
                    sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        visitorID.Add(sdr.GetInt32(0));
                    }

                    return visitorID;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="type">type value</param>  
            /// <returns>list value</returns> 
            public List<int> MultiCityFacility(string type)
            {
                List<int> visitorID = new List<int>();
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter paramRequestId;

                SqlDataAdapter adp;
                DataTable set = new DataTable();

                // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");              
                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                cmd = new SqlCommand("multicityfacilitysameday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    paramRequestId = new SqlParameter("@type", SqlDbType.VarChar, 50);
                    paramRequestId.Value = type;
                    cmd.Parameters.Add(paramRequestId);
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    foreach (DataRow dr in set.Rows)
                    {
                        visitorID.Add((int)dr[0]);
                    }

                    return visitorID;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// for generating Badge number
            /// </summary>
            /// <param name="requestID">request id</param> 
            /// <param name="modifiedBy">Modified associate detail</param> 
            /// <returns>data table</returns>
            public string GenerateBadge(int requestID, string modifiedBy)
            {
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter paramRequestId;
                DataTable dt = new DataTable();
                SqlDataAdapter adp;
                DataTable set = new DataTable();

                // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");              
                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI();
                cmd = new SqlCommand("BatchGenerationBySecurity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    paramRequestId = new SqlParameter("@VisitDetails", SqlDbType.Int);
                    paramRequestId.Value = requestID;
                    cmd.Parameters.Add(paramRequestId);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 50).Value = modifiedBy;
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    string batchNo = set.Rows[0]["BadgeNo"].ToString();
                    return batchNo;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="requestID">request ID</param>  
            /// <param name="modifiedBy">modified By</param>  
            /// <param name="comments">comments field</param>  
            public void RePrintBadge(int requestID, string modifiedBy, string comments)
            {
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter paramRequestId;
                DataTable dt = new DataTable();
                DataTable set = new DataTable();

                // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");              
                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI();
                cmd = new SqlCommand("ReprintBadge", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                try
                {
                    paramRequestId = new SqlParameter("@VisitDetails", SqlDbType.Int);
                    paramRequestId.Value = requestID;
                    cmd.Parameters.Add(paramRequestId);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 50).Value = modifiedBy;
                    cmd.Parameters.Add("@ReprintReason", SqlDbType.VarChar, 50).Value = comments;
                    cmd.ExecuteNonQuery();

                    // adp = new SqlDataAdapter(cmd);
                    // adp.Fill(set);
                    // string BatchNo = set.Rows[0]["BadgeNo"].ToString();
                    // return BatchNo;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="requestID">request Status</param>  
            /// <param name="modifiedBy">modified By</param>  
            /// <returns>data set</returns> 
            public bool VisitorCheckIn(int requestID, string modifiedBy)
            {

                bool isFirst = false;
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter paramRequestId;
                DataTable dt = new DataTable();
                SqlDataAdapter adp;
                DataTable set = new DataTable();
                
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);                
                con.OpenWithMSI();
               
                cmd = new SqlCommand("CheckInVisitor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //GenericTimeZone gt = new GenericTimeZone();
                string format;
                format = "dd/MM/yyyy HH:mm:ss";
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                string dat = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                var da = DateTime.ParseExact(dat, format, provider);
                //DateTime da = gt.GetLocalCurrentDate();
                string time = da.ToString("yyyy-MM-dd HH:mm:ss");
                
                try
                {
                    paramRequestId = new SqlParameter("@VisitDetails", SqlDbType.Int);
                    paramRequestId.Value = requestID;
                    cmd.Parameters.Add(paramRequestId);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 50).Value = modifiedBy;
                    cmd.Parameters.Add("@BadgeIssueddate", SqlDbType.VarChar, 50).Value = time;
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    if (set.Rows[0]["IsFirst"] != null)
                    {
                        if (set.Rows[0]["IsFirst"].ToString().Equals("1"))
                        {
                            isFirst = true;
                        }
                        else
                        {
                            isFirst = false;
                        }
                    }

                    return isFirst;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }
            /// <summary>
            ///  Safety Permit Security Search
            /// </summary>
            /// <param name="requestID"></param>
            /// <param name="modifiedBy"></param>
            /// <param name="vcardNo"></param>
            /// <returns>dataset</returns>
            public bool CheckinVisitor(int requestID, string modifiedBy, string vcardNo, string accessCardNo)
            {
                bool isFirst = false;
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter paramRequestId;
                DataTable dt = new DataTable();
                SqlDataAdapter adp;
                DataTable set = new DataTable();
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI();
                cmd = new SqlCommand("VisitorCheckinWithVCard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    int accessCard = 0;
                    accessCard = accessCardNo == "NA" ? 0 : Convert.ToInt32(accessCardNo);
                    paramRequestId = new SqlParameter("@VisitDetails", SqlDbType.Int);
                    paramRequestId.Value = requestID;
                    cmd.Parameters.Add(paramRequestId);
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 50).Value = modifiedBy;
                    cmd.Parameters.Add("@VcardNo", SqlDbType.VarChar, 50).Value = vcardNo;
                    cmd.Parameters.Add("@AccessCardNo", SqlDbType.Int).Value = accessCard;
                   // cmd.Parameters.Add("@BadgeIssueddate", SqlDbType.VarChar, 50).Value = currenttime;
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    if (set.Rows[0]["IsFirst"] != null)
                    {
                        if (set.Rows[0]["IsFirst"].ToString().Equals("1"))
                        {
                            isFirst = true;
                        }
                        else
                        {
                            isFirst = false;
                        }
                    }

                    return isFirst;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }
            /// <summary>
            /// Function to surrenderLost and found card
            /// </summary>
            /// <param name="checkoutDetails"></param>
            /// <param name="visitDetailsID"></param>
            /// <returns>true/false</returns>
            public bool SurrenderLostCard(string checkoutDetails, string visitDetailsID)
            {
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    cmd = new SqlCommand("SurrenderLostCard", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@checkoutDetails", SqlDbType.VarChar, 500).Value = checkoutDetails;
                    cmd.Parameters.Add("@visitDetailId", SqlDbType.Int).Value = Convert.ToInt32(visitDetailsID);
                    cmd.ExecuteNonQuery();
               
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return true;
            }
            /// <summary>
            /// Function to update checkout status
            /// </summary>
            /// <param name="visitDetailsID"></param>
            /// <param name="bdgestatus"></param>
            /// <param name="reqstatus"></param>
            /// <param name="modifiedBy"></param>
            public bool UpdateCheckoutStatus(string checkoutDetails, string visitDetailsID, string reqstatus, string modifiedBy)
            {
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    cmd = new SqlCommand("UpdateCheckoutStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@checkoutDetails", SqlDbType.VarChar,500).Value = checkoutDetails;
                    cmd.Parameters.Add("reqStatus", SqlDbType.VarChar, 50).Value = reqstatus;
                    cmd.Parameters.Add("@visitDetailId", SqlDbType.Int).Value = Convert.ToInt32(visitDetailsID);
                    cmd.Parameters.Add("@modifiedBy", SqlDbType.VarChar, 50).Value = modifiedBy;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }

                return true;
            }
            /// <summary>
            /// Function to ReissueCard
            /// </summary>
            /// <param name="currentCard"></param>
            /// <param name="newCard"></param>
            /// <param name="visitDetailsId"></param>
            /// <param name="reason"></param>
            public bool ReIssueLostCard(string currentCard, string newCard, string visitDetailsId,string reason)
            {
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);

                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    con.OpenWithMSI(); //// con.Open();  

                    // Added by priti on 3rd June for VMS CR VMS31052010CR6
                    cmd = new SqlCommand("ReissueVcard", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@currentcard", SqlDbType.VarChar, 100).Value = currentCard;
                    cmd.Parameters.Add("@newcard", SqlDbType.VarChar, 100).Value = newCard;
                    cmd.Parameters.Add("@visitdetailsid", SqlDbType.VarChar, 100).Value = visitDetailsId;
                    cmd.Parameters.Add("@reason", SqlDbType.VarChar, 100).Value = reason;


                    // commnetd by priti on 3rd June for VMS CR VMS31052010CR6
                    // cmd = new SqlCommand("Update VisitorRequest set BadgeStatus='" + bdgestatus + "',ActualOutTime='" + ActualOutTime + "',RequestStatus='" + reqstatus + "', Comments='" + comments + "' where RequestID='" + ReqID + "'", con);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            public DataSet SameDayMultipleVisits(string date, string city)
            {
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter paramvisitdate;
                SqlParameter paramvisitcity;
                SqlDataAdapter adp;
                DataSet set = new DataSet();

                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI();
                // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                cmd = new SqlCommand("SamedayVisits", con);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    if (string.IsNullOrEmpty(date))
                    {
                        paramvisitdate = new SqlParameter("@Date", SqlDbType.VarChar, 50);
                        paramvisitdate.Value = null;
                        cmd.Parameters.Add(paramvisitdate);
                    }
                    else
                    {
                        string[] visiteddate = date.Split('/');
                        paramvisitdate = new SqlParameter("@Date", SqlDbType.VarChar, 50);
                        paramvisitdate.Value = visiteddate[2] + "-" + visiteddate[1] + "-" + visiteddate[0];
                        cmd.Parameters.Add(paramvisitdate);
                    }

                    if (string.IsNullOrEmpty(city))
                    {
                        paramvisitcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitcity.Value = null;
                        cmd.Parameters.Add(paramvisitcity);
                    }
                    else
                    {
                        paramvisitcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitcity.Value = city;
                        cmd.Parameters.Add(paramvisitcity);
                    }

                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }

                return set;
            }

            /// <summary>
            /// Get Departments
            /// </summary>
            /// <returns>list value</returns> 
            public List<string> GetDepartments()
            {
                SqlConnection con;
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                try
                {
                    List<string> department = new List<string>();
                    SqlDataReader sdr;
                    SqlCommand cmd;
                    cmd = new SqlCommand("GetAllDepartments", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.OpenWithMSI(); //// con.Open();  
                    sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        department.Add(sdr.GetString(0));
                    }

                    return department;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update Badge Status
            /// </summary>
            /// <param name="visitDetailsID">visit Details ID</param>  
            /// <param name="bdgestatus">badge status</param>  
            /// <param name="actualOutTime">actual Out Time</param> 
            /// <param name="reqstatus">request status</param> 
            /// <param name="comments">comments value</param> 
            /// <param name="modifiedBy">modified By</param> 
            public void UpdateBadgeStatus(string visitDetailsID, string bdgestatus, string actualOutTime, string reqstatus, string comments, string modifiedBy)
            {
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);

                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    con.OpenWithMSI(); //// con.Open();  

                    // Added by priti on 3rd June for VMS CR VMS31052010CR6
                    cmd = new SqlCommand("UpdateBadgeStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@bdgestatus", SqlDbType.VarChar, 100).Value = bdgestatus;
                    cmd.Parameters.Add("@ActualOutTime", SqlDbType.DateTime).Value = actualOutTime;
                    cmd.Parameters.Add("@reqstatus", SqlDbType.VarChar, 50).Value = reqstatus;
                    cmd.Parameters.Add("@comments", SqlDbType.VarChar, 500).Value = comments;
                    cmd.Parameters.Add("@VisDetailsID", SqlDbType.Int).Value = visitDetailsID;
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 50).Value = modifiedBy;

                    // commnetd by priti on 3rd June for VMS CR VMS31052010CR6
                    // cmd = new SqlCommand("Update VisitorRequest set BadgeStatus='" + bdgestatus + "',ActualOutTime='" + ActualOutTime + "',RequestStatus='" + reqstatus + "', Comments='" + comments + "' where RequestID='" + ReqID + "'", con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update Badge Status
            /// </summary>
            /// <param name="visitDetailsID">visit Details ID</param>  
            /// <param name="bdgestatus">badge status</param>  
            /// <param name="visitorid">visitor id</param> 
            /// <param name="reqstatus">request status</param> 
            /// <param name="comments">comments value</param> 
            /// <param name="modifiedBy">modified By</param> 
            /// <param name="selectedfacility">selected facility</param>
            /// <param name="reportedby">reported by</param>
            /// <param name="reportedon">reported on</param>
            public void UpdateBadgeStatusClients(string visitDetailsID, string bdgestatus, string visitorid, string reqstatus, string comments, string modifiedBy, string selectedfacility, string reportedby, string reportedon)
            {
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);

                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    con.OpenWithMSI(); //// con.Open();  

                    // Added by priti on 3rd June for VMS CR VMS31052010CR6
                    cmd = new SqlCommand("UpdateBadgeStatus_clients", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@bdgestatus", SqlDbType.VarChar, 100).Value = bdgestatus;
                    cmd.Parameters.Add("@visitorid", SqlDbType.Int).Value = visitorid;
                    cmd.Parameters.Add("@reqstatus", SqlDbType.VarChar, 50).Value = reqstatus;
                    cmd.Parameters.Add("@comments", SqlDbType.VarChar, 500).Value = comments;
                    cmd.Parameters.Add("@VisDetailsID", SqlDbType.Int).Value = visitDetailsID;
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 50).Value = modifiedBy;
                    cmd.Parameters.Add("@reportedfacility", SqlDbType.VarChar, 50).Value = selectedfacility;
                    cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = reportedby;

                    if (reportedon == "null")
                    {
                        cmd.Parameters.Add("@reportedon", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@reportedon", SqlDbType.DateTime).Value = Convert.ToDateTime(reportedon);
                    }
                    //// commnetd by priti on 3rd June for VMS CR VMS31052010CR6
                    //// cmd = new SqlCommand("Update VisitorRequest set BadgeStatus='" + bdgestatus + "',ActualOutTime='" + ActualOutTime + "',RequestStatus='" + reqstatus + "', Comments='" + comments + "' where RequestID='" + ReqID + "'", con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// badge return values
            /// </summary>
            /// <param name="reqID">request id</param>  
            /// <returns>data set</returns> 
            public DataSet Badgereturnvalues(string reqID)
            {
                SqlParameter paramreqID;
                SqlDataAdapter adp;
                DataSet set = new DataSet();

                // changed by Priti 22 Aug
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con1;
                con1 = new SqlConnection(conn);
                con1.OpenWithMSI();
                try
                {
                    SqlCommand cmd1 = new SqlCommand("GetBadgeReturnedDetails", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    paramreqID = new SqlParameter("@VisitDetailsID", SqlDbType.VarChar, 50);
                    paramreqID.Value = reqID;
                    cmd1.Parameters.Add(paramreqID);
                    adp = new SqlDataAdapter(cmd1);
                    adp.Fill(set);
                    return set;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con1?.Close();
                }
            }

            /// <summary>
            /// badge return values
            /// </summary>
            /// <param name="reqID">request id</param>  
            /// <returns>data set</returns> 
            public DataSet Getgrdvisitorsearchresult(string searchtext)
            {
                SqlParameter paramreqID;
                SqlDataAdapter adp;
                DataSet set = new DataSet();

                // changed by Priti 22 Aug
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con1;
                con1 = new SqlConnection(conn);
                con1.OpenWithMSI();
                try
                {
                    SqlCommand cmd1 = new SqlCommand("SearchMasterDetails", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    paramreqID = new SqlParameter("@Search", SqlDbType.VarChar, 50);
                    paramreqID.Value = searchtext;
                    cmd1.Parameters.Add(paramreqID);
                    adp = new SqlDataAdapter(cmd1);
                    adp.Fill(set);
                    return set;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con1?.Close();
                }
            }

            /// <summary>
            /// badge return values
            /// </summary>
            /// <param name="reqID">request id</param>  
            /// <returns>data set</returns> 
            public DataSet BadgereturnvaluesClients(string reqID)
            {
                SqlParameter paramreqID;
                SqlDataAdapter adp;
                DataSet set = new DataSet();

                // changed by Priti 22 Aug
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con1;
                con1 = new SqlConnection(conn);
                con1.OpenWithMSI();
                try
                {
                    SqlCommand cmd1 = new SqlCommand("GetBadgeReturnedDetails_Clients", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    paramreqID = new SqlParameter("@VisitDetailsID", SqlDbType.VarChar, 50);
                    paramreqID.Value = reqID;
                    cmd1.Parameters.Add(paramreqID);
                    adp = new SqlDataAdapter(cmd1);
                    adp.Fill(set);
                    return set;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con1?.Close();
                }
            }

            /// <summary>
            /// badge return values
            /// </summary>
            /// <param name="parentID">parent ID</param>  
            /// <returns>data set</returns> 
            public DataSet GetBadgeStatusClients(int parentID)
            {
                SqlParameter paramreqID;
                SqlDataAdapter adp;
                DataSet set = new DataSet();

                // changed by Priti 22 Aug
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con1;
                con1 = new SqlConnection(conn);
                con1.OpenWithMSI();
                try
                {
                    SqlCommand cmd1 = new SqlCommand("GetBadgeStatusClients", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    paramreqID = new SqlParameter("@parentID", SqlDbType.Int);
                    paramreqID.Value = parentID;
                    cmd1.Parameters.Add(paramreqID);
                    adp = new SqlDataAdapter(cmd1);
                    adp.Fill(set);
                    return set;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con1?.Close();
                }
            }

            /// <summary>
            /// Get Visits to Department
            /// </summary>
            /// <param name="associate">associate data</param>  
            /// <returns>data set</returns> 
            public DataSet GetVisitstoDept(List<string> associate)
            {
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter paramassociatelist;
                SqlDataAdapter adp;
                DataSet set = new DataSet();

                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI();
                // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                cmd = new SqlCommand("VisitstoDept", con);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    if (associate.Count == 0)
                    {
                        paramassociatelist = new SqlParameter("@HostList", SqlDbType.VarChar, 2500);
                        paramassociatelist.Value = null;
                        cmd.Parameters.Add(paramassociatelist);
                    }
                    else
                    {
                        paramassociatelist = new SqlParameter("@HostList", SqlDbType.VarChar, 2500);
                        paramassociatelist.Value = associate;
                        cmd.Parameters.Add(associate);
                    }

                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    return set;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="reqid">request id</param>  
            /// <param name="totime">to time</param>              
            public void Extendthetime(string reqid, string totime)
            {
                SqlConnection con;
                SqlCommand cmd;
                SqlParameter sqltotime;
                SqlParameter sqlreqid;
                SqlDataAdapter adp;
                DataSet set = new DataSet();

                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);

                // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                cmd = new SqlCommand("Extendthetime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.OpenWithMSI(); //// con.Open();  
                try
                {
                    if (reqid == null)
                    {
                        sqlreqid = new SqlParameter("@ReqID", SqlDbType.VarChar, 2500);
                        sqlreqid.Value = null;
                        cmd.Parameters.Add(sqlreqid);
                    }
                    else
                    {
                        sqlreqid = new SqlParameter("@ReqID", SqlDbType.VarChar, 2500);
                        sqlreqid.Value = reqid;
                        cmd.Parameters.Add(sqlreqid);
                    }

                    if (totime == null)
                    {
                        sqltotime = new SqlParameter("@totime", SqlDbType.VarChar, 2500);
                        sqltotime.Value = null;
                        cmd.Parameters.Add(null);
                    }
                    else
                    {
                        sqltotime = new SqlParameter("@totime", SqlDbType.VarChar, 2500);
                        sqltotime.Value = totime;
                        cmd.Parameters.Add(sqltotime);
                    }

                    adp = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Search Visitor Master Details
            /// </summary>
            /// <param name="searchstr">search string</param>
            /// <param name="strfn">search value</param>
            /// <param name="strln">search line</param>
            /// <param name="strmn">search data</param>
            /// <param name="strcomp">data search</param>
            /// <returns>Data set</returns>
            public DataSet SearchVisitorMasterDetails(string searchstr, string strfn, string strln, string strmn, string strcomp)
            {
                DataSet set = new DataSet();
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;
                    SqlParameter paramsearchText;
                    SqlDataAdapter adp;
                    SqlParameter paramfirstname;
                    SqlParameter paramlastname;
                    SqlParameter paramcompany;
                    SqlParameter parammobno;

                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    cmd = new SqlCommand("SearchVisitorsSecurity", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (string.IsNullOrEmpty(searchstr))
                    {
                        paramsearchText = new SqlParameter("@SearchValue", SqlDbType.VarChar, 50);
                        paramsearchText.Value = string.Empty;
                        cmd.Parameters.Add(paramsearchText);
                    }
                    else
                    {
                        paramsearchText = new SqlParameter("@SearchValue", SqlDbType.VarChar, 50);
                        paramsearchText.Value = searchstr;
                        cmd.Parameters.Add(paramsearchText);
                    }

                    if (string.IsNullOrEmpty(strfn))
                    {
                        paramfirstname = new SqlParameter("@FirstName", SqlDbType.VarChar, 50);
                        paramfirstname.Value = null;
                        cmd.Parameters.Add(paramfirstname);
                    }
                    else
                    {
                        paramfirstname = new SqlParameter("@FirstName", SqlDbType.VarChar, 50);
                        paramfirstname.Value = strfn;
                        cmd.Parameters.Add(paramfirstname);
                    }

                    if (string.IsNullOrEmpty(strln))
                    {
                        paramlastname = new SqlParameter("@LastName", SqlDbType.VarChar, 50);
                        paramlastname.Value = null;
                        cmd.Parameters.Add(paramlastname);
                    }
                    else
                    {
                        paramlastname = new SqlParameter("@LastName", SqlDbType.VarChar, 50);
                        paramlastname.Value = strln;
                        cmd.Parameters.Add(paramlastname);
                    }

                    if (string.IsNullOrEmpty(strcomp))
                    {
                        paramcompany = new SqlParameter("@Company", SqlDbType.VarChar, 50);
                        paramcompany.Value = null;
                        cmd.Parameters.Add(paramcompany);
                    }
                    else
                    {
                        paramcompany = new SqlParameter("@Company", SqlDbType.VarChar, 50);
                        paramcompany.Value = strcomp;
                        cmd.Parameters.Add(paramcompany);
                    }

                    if (string.IsNullOrEmpty(strmn))
                    {
                        parammobno = new SqlParameter("@Mobile", SqlDbType.VarChar, 50);
                        parammobno.Value = null;
                        cmd.Parameters.Add(parammobno);
                    }
                    else
                    {
                        parammobno = new SqlParameter("@Mobile", SqlDbType.VarChar, 50);
                        parammobno.Value = strmn;
                        cmd.Parameters.Add(parammobno);
                    }

                    // End Changes for VMS CR17 07Mar2011 Vimal
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
             
                return set;
            }

            /// <summary>
            /// Display details
            /// </summary>
            /// <param name="visitDetailsID">visit Details ID</param>  
            /// <returns>data set</returns> 
            public PropertiesDC Displaydetails(int visitDetailsID)
            {
                PropertiesDC propertiesDc = new PropertiesDC();
                VMSBusinessEntity.VisitorRequest visitorRequest = new VMSBusinessEntity.VisitorRequest();
                try
                {

                    SqlConnection connection = new SqlConnection(vmsConn);
                    connection.SetAccessToken();

                    using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                    {
                        VMSBusinessEntity.VisitDetail visitdetails = (from visitdetail in vmsdb.VisitDetails
                                                                      where visitdetail.VisitDetailsID == visitDetailsID
                                                                      select visitdetail).FirstOrDefault<VisitDetail>();

                        VMSBusinessEntity.VisitorRequest visitorrequest = (from visitrequest in vmsdb.VisitorRequests
                                                                           where visitrequest.RequestID == visitdetails.RequestID
                                                                           select visitrequest).FirstOrDefault<VisitorRequest>();

                        VMSBusinessEntity.VisitorEmergencyContact visitoremergencycontactdetails = (from visitoremergencycontact in vmsdb.VisitorEmergencyContacts
                                                                                                    where visitoremergencycontact.RequestID == visitorrequest.RequestID
                                                                                                    select visitoremergencycontact).FirstOrDefault<VisitorEmergencyContact>();

                        List<VisitorEquipment> cC = (from visitorequipment in vmsdb.VisitorEquipments
                                                     where visitorequipment.VisitDetailsID == visitDetailsID
                                                     select visitorequipment).ToList<VisitorEquipment>();
                        List<tblEquipmentsInCustody> eC = (from equipmentcustody in vmsdb.tblEquipmentsInCustodies
                                                           where equipmentcustody.VisitDetailsID == visitDetailsID
                                                           select equipmentcustody).ToList<tblEquipmentsInCustody>();
                        VMSBusinessEntity.VisitorMaster visitormasterdetails = (from visitormaster in vmsdb.VisitorMasters
                                                                                where visitormaster.VisitorID == visitorrequest.VisitorID
                                                                                select visitormaster).FirstOrDefault<VisitorMaster>();

                        VMSBusinessEntity.VisitorProof visitorproofdetails = (from visitorproof in vmsdb.VisitorProofs
                                                                              where visitorproof.VisitorID == visitormasterdetails.VisitorID
                                                                              select visitorproof).FirstOrDefault<VisitorProof>();

                        propertiesDc.VisitorMasterProperty = visitormasterdetails;
                        propertiesDc.VisitorProofProperty = visitorproofdetails;
                        propertiesDc.VisitorRequestProperty = visitorrequest;
                        propertiesDc.VisitDetailProperty = visitdetails;
                        propertiesDc.VisitorEquipmentProperty = cC;
                        propertiesDc.EquipmentCustodyProperty = eC;
                        propertiesDc.VisitorEmergencyContactProperty = visitoremergencycontactdetails;
                        propertiesDc = this.GetIdentityDetails(propertiesDc);
                        connection?.Close();
                        return propertiesDc;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Identity Details
            /// </summary>
            /// <param name="propertiesDc">properties Detail</param>              
            /// <returns>data set</returns> 
            public PropertiesDC GetIdentityDetails(PropertiesDC propertiesDc)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                try
                {
                    IdentityDetails identity = new IdentityDetails();
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("GetIdentityDetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
#pragma warning disable CS0618 // 'SqlParameterCollection.Add(string, object)' is obsolete: 'Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202'
                        cmd.Parameters.Add("@VisitorID", propertiesDc.VisitorMasterProperty.VisitorID);
#pragma warning restore CS0618 // 'SqlParameterCollection.Add(string, object)' is obsolete: 'Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202'
                        IDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            identity.IdentityType = Convert.ToString(reader["IdentityType"]);
                            identity.IdentityNo = Convert.ToString(reader["Identityno"]);
                        }

                        reader.Close();
                        propertiesDc.IndentityDetailsProperty = identity;
                    }

                    return propertiesDc;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Get Search Details
            /// </summary>
            /// <param name="visitorID">visitor ID</param>  
            /// <returns>data set</returns> 
            public PropertiesDC GetSearchDetails(int visitorID)
            {
                PropertiesDC propertiesDc = new PropertiesDC();
                VMSBusinessEntity.VisitorRequest visitorRequest = new VMSBusinessEntity.VisitorRequest();
                try
                {

                    SqlConnection connection = new SqlConnection(vmsConn);
                    connection.SetAccessToken();

                    using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                    {
                        VMSBusinessEntity.VisitorMaster visitormasterdetails = (from visitormaster in vmsdb.VisitorMasters
                                                                                where visitormaster.VisitorID == visitorID
                                                                                select visitormaster).FirstOrDefault<VisitorMaster>();
                        VMSBusinessEntity.VisitorProof visitorproofdetails = (from visitorproof in vmsdb.VisitorProofs
                                                                              where visitorproof.VisitorID == visitormasterdetails.VisitorID
                                                                              select visitorproof).FirstOrDefault<VisitorProof>();
                        VMSBusinessEntity.VisitorRequest visitorrequest = (from visitrequest in vmsdb.VisitorRequests
                                                                           where visitrequest.VisitorID == visitorID
                                                                           select visitrequest).FirstOrDefault<VisitorRequest>();
                        propertiesDc.VisitorMasterProperty = visitormasterdetails;
                        propertiesDc.VisitorProofProperty = visitorproofdetails;
                        propertiesDc.VisitorRequestProperty = visitorrequest;
                    }
                    connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
                return propertiesDc;
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="requestID">request ID</param>  
            /// <returns>data set</returns> 
            public List<VisitDetail> GetVisitDetailsByRequestID(int requestID)
            {
                List<VisitDetail> visitDetailObj = null;
                try
                {

                    SqlConnection connection = new SqlConnection(vmsConn);
                    connection.SetAccessToken();

                    using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                    {
                        List<VisitDetail> cc = (from visitdetail in vmsdb.VisitDetails
                                                where visitdetail.RequestID == requestID
                                                select visitdetail).ToList<VisitDetail>();
                        visitDetailObj = cc;
                    }
                    connection?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return visitDetailObj;
            }

            /// <summary>
            /// get Country Id
            /// </summary>
            /// <param name="countryName">country Name</param>  
            /// <returns>data table</returns> 
            public DataTable GetCountryId(string countryName)
            {
                string strSql = "GetCountryIdFromCountryName";
                DataSet dscountry = new DataSet();
                DataTable dtcountrId = new DataTable();
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand(strSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CountryName", SqlDbType.VarChar, 50).Value = countryName;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dscountry);
                    dtcountrId = dscountry.Tables[0];
                    return dtcountrId;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update identity details
            /// </summary>
            /// <param name="parentid">parent id</param>
            /// <param name="collectedby">collected by</param>
            /// <param name="dispatchedBy">dispatched By</param>
            public void Updatedispatchdetails(int parentid, int collectedby, int dispatchedBy)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("updatedispatchstatus", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ParentRefernceID", parentid);
                        cmd.Parameters.AddWithValue("@collectedby", collectedby);
                        cmd.Parameters.AddWithValue("@Dispatchedby", dispatchedBy);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update identity details
            /// </summary>
            /// <param name="parentid">parent id</param>
            /// <param name="notifiedBy">notified By</param>
            public void UpdateNotifcationtoHost(int parentid, int notifiedBy)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("updateNotificationtoHost", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ParentRefernceID", parentid);
                        cmd.Parameters.AddWithValue("@NotifiedBy", notifiedBy);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// get Country Id
            /// </summary>
            /// <param name="parentReferenceId">parent Reference Id</param>  
            /// <returns>data table</returns> 
            public DataTable GetClientRequestwithParentrefernceID(int parentReferenceId)
            {
                string strSql = "GetClientDetailswithParentID";

                DataTable clientdetails = new DataTable();
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand(strSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ParentReferenceId", SqlDbType.Int).Value = parentReferenceId;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(clientdetails);
                    return clientdetails;
                }
                catch (SqlException exception)
                {
                    throw exception;
                }
                finally
                {
                    con.Close();
                }
            }

            /// <summary>
            /// Get Dummy Equipment Details
            /// </summary>
            /// <param name="visitorID">visitor ID</param>
            /// <returns>Data Table</returns>
            public DataTable GetEquipmentDetails(int visitorID)
            {
                DataTable getVisitorID = new DataTable();
                string strSql = "usp_GetEquipmentDetails";
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand(strSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VisitorID", SqlDbType.Int).Value = visitorID;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(getVisitorID);
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    con?.Close();
                }

                return getVisitorID;
            }

            /// <summary>
            /// Get Dummy Equipment Details
            /// </summary>
            /// <param name="parentID">parent ID</param>
            /// <param name="visitorID">visitor ID</param>
            /// <returns>Data Table</returns>
            public DataTable GetVisitDetails(int parentID, int visitorID)
            {
                DataTable visitdetails = new DataTable();
                string strSql = "getvisitdetails_Clients";
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand(strSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@parentid", SqlDbType.Int).Value = parentID;
                    cmd.Parameters.Add("@visitorid", SqlDbType.Int).Value = visitorID;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(visitdetails);
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }

                return visitdetails;
            }

            /// <summary>
            /// Get Dummy Equipment Details
            /// </summary>
            /// <param name="parentID">parent ID</param>
            /// <param name="visitorID">visitor ID</param>
            /// <returns>Data Table</returns>
            public DataTable GetLogDetails(int parentID, int visitorID)
            {
                DataTable logdetails = new DataTable();
                string strSql = "getlogdetails";
                SqlCommand cmd;
                SqlConnection con = null;
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand(strSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@parentid", SqlDbType.Int).Value = parentID;
                    cmd.Parameters.Add("@visitorid", SqlDbType.Int).Value = visitorID;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(logdetails);
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    con?.Close();
                }

                return logdetails;
            }

            /// <summary>
            /// Search Visitor Master Details
            /// </summary>
            /// <param name="country">country value</param>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="fromdate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <param name="fromtime">from time value</param>
            /// <param name="totime">to time</param>
            /// <param name="dept">department value</param>
            /// <param name="rpttype">type value</param>
            /// <param name="purpose">purpose value</param>
            /// <returns>Data set</returns>
            public DataSet ReportInfo(int country, string city, string facility, string fromdate, string todate, string fromtime, string totime, string dept, string rpttype, string purpose)
            {
                DataSet set = new DataSet();
                string procetype = string.Empty;
                try
                {
                    SqlConnection con;
                    SqlCommand cmd;
                    ////SqlParameter paramvisitingcountry;
                    SqlParameter paramvisitingcity;
                    SqlParameter paramvisitingfacility;
                    SqlParameter paramfromdate;
                    SqlParameter paramtodate;
                    SqlParameter paramfromtime;
                    SqlParameter paramtotime;
                    SqlParameter paramdept;

                    // Begin VMS CR 16 Changes Uma
                    SqlParameter parampurpose;

                    // End VMS CR 16 Changes Uma
                    SqlDataAdapter adp;

                    // changed by Priti 22 aug.
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    con = new SqlConnection(conn);
                    con.OpenWithMSI();
                    // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                    if (rpttype == "Headcount")
                    {
                        procetype = "Headcount_Report";
                    }
                    else if (rpttype == "Multivisit")
                    {
                        procetype = "Multivisit_Report";
                    }

                    // added by uma on 22/aug/09
                    cmd = new SqlCommand(procetype, con);
                    cmd.CommandType = CommandType.StoredProcedure;
#pragma warning disable CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
                    if (country == 0 || country == null)
#pragma warning restore CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
                    {
                        // con.AddInParameter(procetype, "Country", SqlDbType.Int, country);
                        cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.Int)).Value = null;

                        // paramvisitingcountry = new SqlParameter("@Country", SqlDbType.int);
                        // paramvisitingcountry.Value = null;
                        // cmd.Parameters.Add(paramvisitingcountry);
                    }
                    else
                    {
                        // paramvisitingcountry = new SqlParameter("@Country", SqlDbType.VarChar, 50);
                        // paramvisitingcountry.Value = country;
                        // cmd.Parameters.Add(paramvisitingcountry);
                        cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.Int)).Value = country;
                    }

                    if (city == "Select" || city == null)
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = null;
                        cmd.Parameters.Add(paramvisitingcity);
                    }
                    else
                    {
                        paramvisitingcity = new SqlParameter("@City", SqlDbType.VarChar, 50);
                        paramvisitingcity.Value = city;
                        cmd.Parameters.Add(paramvisitingcity);
                    }

                    if (facility == "Select" || string.IsNullOrEmpty(facility) || facility == "0")
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = null;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }
                    else
                    {
                        paramvisitingfacility = new SqlParameter("@Facility", SqlDbType.VarChar, 50);
                        paramvisitingfacility.Value = facility;
                        cmd.Parameters.Add(paramvisitingfacility);
                    }

                    if (fromdate == null || string.IsNullOrEmpty(fromdate.Trim()) || fromdate.Trim() == "__/__/____")
                    {
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = null;
                        cmd.Parameters.Add(paramfromdate);
                    }
                    else
                    {
                        string[] fromDate = fromdate.Split('/');
                        paramfromdate = new SqlParameter("@FromDate", SqlDbType.VarChar, 50);
                        paramfromdate.Value = fromDate[2] + "-" + fromDate[1] + "-" + fromDate[0];
                        cmd.Parameters.Add(paramfromdate);
                    }

                    if (string.IsNullOrEmpty(todate) || todate.Trim() == "__/__/____")
                    {
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = null;
                        cmd.Parameters.Add(paramtodate);
                    }
                    else
                    {
                        string[] todates = todate.Split('/');
                        paramtodate = new SqlParameter("@ToDate", SqlDbType.VarChar, 50);
                        paramtodate.Value = todates[2] + "-" + todates[1] + "-" + todates[0];
                        cmd.Parameters.Add(paramtodate);
                    }

                    if (string.IsNullOrEmpty(fromtime))
                    {
                        paramfromtime = new SqlParameter("@FromTime", SqlDbType.VarChar, 50);
                        paramfromtime.Value = null;
                        cmd.Parameters.Add(paramfromtime);
                    }
                    else
                    {
                        paramfromtime = new SqlParameter("@FromTime", SqlDbType.VarChar, 50);
                        paramfromtime.Value = fromtime;
                        cmd.Parameters.Add(paramfromtime);
                    }

                    if (string.IsNullOrEmpty(totime))
                    {
                        paramtotime = new SqlParameter("@ToTime", SqlDbType.VarChar, 50);
                        paramtotime.Value = null;
                        cmd.Parameters.Add(paramtotime);
                    }
                    else
                    {
                        paramtotime = new SqlParameter("@ToTime", SqlDbType.VarChar, 50);
                        paramtotime.Value = totime;
                        cmd.Parameters.Add(paramtotime);
                    }

                    if (dept == null || dept == "Select")
                    {
                        paramdept = new SqlParameter("@HostDept", SqlDbType.VarChar, 50);
                        paramdept.Value = null;
                        cmd.Parameters.Add(paramdept);
                    }
                    else
                    {
                        paramdept = new SqlParameter("@HostDept", SqlDbType.VarChar, 50);
                        paramdept.Value = dept;
                        cmd.Parameters.Add(paramdept);
                    }

                    // Begin VMS CR 16 Changes Uma
                    if (rpttype == "Headcount")
                    {
                        if (purpose == null)
                        {
                            parampurpose = new SqlParameter("@Purpose", SqlDbType.VarChar, 50);
                            parampurpose.Value = null;
                            cmd.Parameters.Add(parampurpose);
                        }
                        else
                        {
                            parampurpose = new SqlParameter("@Purpose", SqlDbType.VarChar, 50);
                            parampurpose.Value = purpose;
                            cmd.Parameters.Add(parampurpose);
                        }
                    }

                    // End VMS CR 16 Changes Uma
                    // added by uma on 22/aug/09----------End.
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(set);
                    con?.Close();
                    return set;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Edit Visitor Information
            /// </summary>
            /// <param name="visitorProof">visitor Proof</param>  
            /// <param name="visitorMaster">visitor Master</param> 
            /// <param name="visitorRequest">visitor request</param> 
            /// <param name="visitDetailObj">visit Detail</param>  
            /// <param name="visitorEquipmentsObj">visitor equipment</param>  
            /// <param name="visitorEmergencyContactObj">visitor Emergency Contact</param>  
            /// <param name="equipcustody">equipment custody</param>  
            /// <returns>integer value</returns> 
            public int EditVisitorInformation(VMSBusinessEntity.VisitorProof visitorProof, VMSBusinessEntity.VisitorMaster visitorMaster, VMSBusinessEntity.VisitorRequest visitorRequest, VMSBusinessEntity.VisitDetail[] visitDetailObj, VisitorEquipment[] visitorEquipmentsObj, VisitorEmergencyContact visitorEmergencyContactObj, int equipcustody)
            {
                int success = 1;
                if (this.UpdateRequestDetails(visitorRequest, equipcustody))
                {
                    ////if (VisitorProof.Photo != null)
                    ////    {
                    ////    UpdateUserImgInDB(VisitorProof);
                    ////    }
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            SqlConnection connection = new SqlConnection(vmsConn);
                            connection.SetAccessToken();
                            using (VMSBusinessEntity.VMSDataObjectsDataContext vmsdb = new VMSDataObjectsDataContext(connection))
                            {
                                vmsdb.Connection.Open();
                                if (visitorProof.Photo != null)
                                {
                                    visitorProof.VisitorID = visitorMaster.VisitorID;
                                    vmsdb.UpdateUserImgInDB(visitorMaster.VisitorID, visitorProof.Photo, visitorProof.FileContentId);
                                    vmsdb.SubmitChanges();
                                }

                                vmsdb.ClearVisitDetails(visitorRequest.RequestID, Convert.ToString(visitDetailObj[0].VisitDetailsID));
                                vmsdb.SubmitChanges();

                                if (visitDetailObj != null)
                                {
                                    foreach (VMSBusinessEntity.VisitDetail visitDetail in visitDetailObj)
                                    {
                                        visitDetail.RequestID = (int)visitorRequest.RequestID;
                                        vmsdb.VisitDetails.InsertOnSubmit(visitDetail);
                                        vmsdb.SubmitChanges();
                                    }
                                }

                                vmsdb.SubmitChanges();
                                visitorEmergencyContactObj.RequestID = visitorRequest.RequestID;
                                vmsdb.VisitorEmergencyContacts.Attach(visitorEmergencyContactObj);
                                foreach (ObjectChangeConflict conflict in vmsdb.ChangeConflicts)
                                {
                                    foreach (MemberChangeConflict memberConflict in conflict.MemberConflicts)
                                    {
                                        memberConflict.Resolve(RefreshMode.KeepCurrentValues);
                                    }
                                }
                                ////vmsdb.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, visitorEmergencyContactObj);
                                vmsdb.VisitorMasters.Attach(visitorMaster);
                                vmsdb.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, visitorMaster);
                                vmsdb.SubmitChanges();
                                success = 0;
                                scope.Complete();
                            }
                            connection?.Close();
                            return success;
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    return success;
                }
            }

            /// <summary>
            /// Update identity details
            /// </summary>
            /// <param name="requestid">request id</param>
            /// <param name="visitorid">visitor id</param>
            /// <param name="visitorname">visitor name</param>
            /// <param name="accesscardnumber">access card number</param>
            public void UpdateCardDetails(int requestid, int visitorid, string visitorname, int accesscardnumber, string vcardnumber)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("InsertAccessCardNumber", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@RequestID", requestid);
                        cmd.Parameters.AddWithValue("@VisitorID", visitorid);
                        cmd.Parameters.AddWithValue("@VisitorName", visitorname);
                        if (accesscardnumber == 0)
                        {
                            cmd.Parameters.AddWithValue("@AccessCardNumber", DBNull.Value);
                            cmd.Parameters.AddWithValue("@VCardNumber", DBNull.Value);
                            cmd.Parameters.AddWithValue("@badgestatus", "VCard Partially Updated");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@AccessCardNumber", accesscardnumber);
                            cmd.Parameters.AddWithValue("@VCardNumber", vcardnumber);
                            cmd.Parameters.AddWithValue("@badgestatus", "Updated  VCard");
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update identity details
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
            public void Reissuecards(string visitorname, string accessnumber, string vcardnumber, string requestid, string visitorid, string parentid, string reissuedby, string recollectedby, string reissuereason, string selectedfacility)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("Reissuecards", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RequestID", Convert.ToInt32(requestid));
                        cmd.Parameters.AddWithValue("@VisitorID", Convert.ToInt32(visitorid));
                        cmd.Parameters.AddWithValue("@ParentID", Convert.ToInt32(parentid));
                        cmd.Parameters.AddWithValue("@reissueddispatchby", Convert.ToInt32(reissuedby));
                        cmd.Parameters.AddWithValue("@reissuecollectedby", Convert.ToInt32(recollectedby));
                        cmd.Parameters.AddWithValue("@VisitorName", visitorname);
                        cmd.Parameters.AddWithValue("@VcardNumber", vcardnumber);
                        cmd.Parameters.AddWithValue("@AccessCardNumber", Convert.ToInt32(accessnumber));
                        cmd.Parameters.AddWithValue("@reissuereason", reissuereason);
                        cmd.Parameters.AddWithValue("@reportedfacility", selectedfacility);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update identity details
            /// </summary>
            /// <param name="securityid">security id</param>
            /// <returns>access card usage</returns>
            public DataTable Loadfacilitylist(int securityid)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                DataTable facilitylistload = new DataTable();
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("Loadfacilitylist", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@secuirtyid", securityid);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(facilitylistload);
                        return facilitylistload;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update identity details
            /// </summary>
            /// <returns>reissue reason list</returns>
            public DataTable LoadReissuelistDL()
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                DataTable reissuelistload = new DataTable();
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("GetReissuelist", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(reissuelistload);
                        return reissuelistload;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update identity details
            /// </summary>
            /// <param name="accesscardnumber">access card number</param>
            /// <returns>access card usage</returns>
            public DataTable Checkaccesscardusage(int accesscardnumber)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                DataTable accesscardusage = new DataTable();
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("CheckAccesscardusage", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AccessCardNumber", accesscardnumber);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(accesscardusage);
                        return accesscardusage;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update identity details
            /// </summary>
            /// <param name="identityDetails">identity Details</param>
            /// <param name="visitorid">visitor Details</param>
            public void UpdateIdentityDetails(IdentityDetails identityDetails, int visitorid)
            {
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection con = new SqlConnection(conn);
                try
                {
                    using (con)
                    {
                        con.OpenWithMSI(); //// con.Open();  
                        SqlCommand cmd = new SqlCommand("UpdateIdentityDetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VisitorID", visitorid);
                        cmd.Parameters.AddWithValue("@IdentityType", identityDetails == null ? string.Empty : identityDetails.IdentityType);
                        cmd.Parameters.AddWithValue("@IdentityNo", identityDetails == null ? string.Empty : identityDetails.IdentityNo);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Update User Image In Database
            /// </summary>
            /// <param name="objVisitorProof">object Visitor Proof</param>  
            /// <returns>boolean value</returns> 
            public bool UpdateUserImgInDB(VMSBusinessEntity.VisitorProof objVisitorProof)
            {
                try
                {
                    string sqlUpdateVisitorImg = string.Empty;
                    sqlUpdateVisitorImg = "UpdateUserImgInDB";
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase(DBConnection.Connectionstring());
                    DbCommand dbloginUserNamecmd = sqlConn.GetStoredProcCommand(sqlUpdateVisitorImg);
                   
                        sqlConn.AddInParameter(dbloginUserNamecmd, "VisitorID", SqlDbType.Int, Convert.ToInt32(objVisitorProof.VisitorID));
                        sqlConn.AddInParameter(dbloginUserNamecmd, "Photo", SqlDbType.Text, Convert.ToString(objVisitorProof.Photo));
                        sqlConn.AddInParameter(dbloginUserNamecmd, "FileContentId", SqlDbType.VarChar, Convert.ToString(objVisitorProof.FileContentId));

                        // sqlConn.AddInParameter(dbLoginUserNamecmd, "ProofID", SqlDbType.Int, Convert.ToInt32(objVisitorProof.ProofID));
                        // sqlConn.AddInParameter(dbLoginUserNamecmd, "IDProofImage", SqlDbType.Text, objVisitorProof.IDProofImage)
                        dbloginUserNamecmd.CommandType = CommandType.StoredProcedure;
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbloginUserNamecmd))
                    {
                        sqlConn.ExecuteNonQuery(dbloginUserNamecmd);
                    }
                    dbloginUserNamecmd.Connection.Close();
                    return true;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (NullReferenceException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            #region UpdateUserImgInDB
            /// <summary>
            /// Method to Update Images of Associate
            /// </summary>
            /// <param name="requestID">Request id</param>
            /// <param name="strEncryptedBinaryData">Encrypted binary data</param> 
            /// <returns>boolean value</returns>
            public bool UpdateUserImgInDB(int requestID, string strEncryptedBinaryData)
            {
                try
                {
                    string sqlUpdateVisitorImg = string.Empty;
                    sqlUpdateVisitorImg = "UpdatePhoto";
                    SqlAzureDatabase sqlConn = new SqlAzureDatabase("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l; Column Encryption Setting=enabled; Encrypt=true; TrustServerCertificate=true");
                    DbCommand dbloginUserNamecmd = sqlConn.GetStoredProcCommand(sqlUpdateVisitorImg);
                    
                        sqlConn.AddInParameter(dbloginUserNamecmd, "RequestID", SqlDbType.VarChar, requestID);
                        sqlConn.AddInParameter(dbloginUserNamecmd, "BinaryImage", SqlDbType.VarChar, strEncryptedBinaryData);

                        dbloginUserNamecmd.CommandType = CommandType.StoredProcedure;
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbloginUserNamecmd))
                    {

                        sqlConn.ExecuteNonQuery(dbloginUserNamecmd);
                    }
                    dbloginUserNamecmd.Connection.Close();
                    return true;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (NullReferenceException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            #endregion

            /// <summary>
            /// Get Country Code
            /// </summary>
            /// <param name="country">country value</param>  
            /// <returns>string value</returns> 
            public string GetCountryCode(string country)
            {
                string countrycode;
                SqlCommand cmd;
                SqlConnection con;

                // con = new SqlConnection("Data Source=ctsintcosead\\sql2008;Initial Catalog=VMS;User ID=PhysicalSecurity;Password=phy5ic@l");
                // changed by Priti 22 aug.
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                cmd = new SqlCommand("Select Code from CountryCode where Country='" + country + "'", con);

                try
                {
                    countrycode = cmd.ExecuteScalar().ToString();
                    return countrycode;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Get Native Country
            /// </summary>
            /// <param name="country">country name</param>   
            /// <returns>data table value</returns> 
            public DataTable GetNativeCountry(string country)
            {
                DataSet dsreturn = new DataSet();
                string countrycode;
                SqlCommand cmd;
                SqlConnection con;
                string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                con = new SqlConnection(conn);
                con.OpenWithMSI(); //// con.Open();  
                if (string.IsNullOrEmpty(country))
                {
                    cmd = new SqlCommand("Select Country, Code as Code,CountryId from CountryCode order by Country", con);

                    // cmd = new SqlCommand("Select Country, CASE(LEN(Code)) WHEN 1 Then '000'+Code WHEN 2 THEN '00'+Code WHEN 3 THEN '0'+Code ELSE Code End as Code from CountryCode order by Country", con);
                }
                else
                {
                    cmd = new SqlCommand("Select Country, Code as Code,CountryId from CountryCode WHERE Country='" + country.Trim() + "'", con);

                    // cmd = new SqlCommand("Select Country, CASE(LEN(Code)) WHEN 1 Then '000'+Code WHEN 2 THEN '00'+Code WHEN 3 THEN '0'+Code ELSE Code End as Code from CountryCode WHERE Country='" + country.Trim() + "'", con);
                }

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                try
                {
                    adp.Fill(dsreturn);
                    countrycode = cmd.ExecuteScalar().ToString();
                    return dsreturn.Tables[0];
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Get Location Details By Id
            /// </summary>
            /// <param name="requestId">request id</param>  
            /// <returns>data table</returns> 
            public DataTable GetLocationDetailsById(int requestId)
            {
                SqlCommand dbcommand;
                SqlConnection dbconnection = null;
                try
                {
                    DataSet dslocation = new DataSet();
                    string configConnection = Convert.ToString(ConfigurationManager.ConnectionStrings["VMSConnectionString"]);
                    dbconnection = new SqlConnection(configConnection);
                    ////dbconnection.Open();
                    dbconnection.OpenWithMSI();
                    dbcommand = new SqlCommand("GetLocationDetailsById", dbconnection);
                    dbcommand.CommandType = CommandType.StoredProcedure;
                    dbcommand.Parameters.Add("@RequestId", SqlDbType.Int).Value = requestId;
                    SqlDataAdapter dbapatper = new SqlDataAdapter(dbcommand);
                    dbapatper.Fill(dslocation);
                    return dslocation.Tables[0];
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    if (dbconnection.State == ConnectionState.Open)
                    {
                        dbconnection?.Close();
                    }
                }
            }

            /// <summary>
            /// Get Security facility information
            /// </summary>
            /// <param name="securityID">security ID</param>
            /// <returns>data set</returns>
            public DataSet GetSecurityCity(string securityID)
            {
                DataSet dscity = new DataSet();
                SqlAzureDatabase sqlConn;
                DbCommand dbcmd;

                try
                {                                      
                    sqlConn = new SqlAzureDatabase(ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                    dbcmd = sqlConn.GetStoredProcCommand("GetSecurityFacilityByLocationId");
                   
                        dbcmd.CommandType = CommandType.StoredProcedure;
                        sqlConn.AddInParameter(dbcmd, "@securityID", SqlDbType.VarChar, securityID);
                    using (DataSet dbreader = sqlConn.ExecuteDataSet(dbcmd))
                    {
                        dscity = sqlConn.ExecuteDataSet(dbcmd);
                    }
                    dbcmd.Connection?.Close();
                    return dscity;


                    ////SqlDataAdapter dbapatper = new SqlDataAdapter(dbcmd);
                    ////DataSet dscity = new DataSet();
                    ////string configConnection = Convert.ToString(ConfigurationManager.ConnectionStrings["VMSConnectionString"]);
                    ////dbconnection = new SqlConnection(configConnection);
                    ////////dbconnection.Open();
                    ////dbconnection.OpenWithMSI();
                    ////dbcommand = new SqlCommand("GetSecurityFacilityByLocationId", dbconnection);
                    ////dbcommand.CommandType = CommandType.StoredProcedure;
                    ////dbcommand.Parameters.Add("@securityID", SqlDbType.VarChar, 10).Value = securityID;
                    ////SqlDataAdapter dbapatper = new SqlDataAdapter(dbcommand);
                    ////dbapatper.Fill(dscity);
                    ////return dscity;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                ////finally
                ////{
                ////    if (dbconnection.State == ConnectionState.Open)
                ////    {
                ////        dbconnection.Close();
                ////    }
                ////}
            }

            /// <summary>
            /// Get Facility V net
            /// </summary>
            /// <param name="locationId">location Id</param>   
            /// <returns>string value</returns> 
            public string GetFacilityVnet(int locationId)
            {
                SqlConnection con = null;
                try
                {
                    DataSet dscity = new DataSet();
                    SqlCommand cmd;
                    string conn = Convert.ToString(ConfigurationManager.ConnectionStrings["VMSConnectionString"]);
                    con = new SqlConnection(conn);
                    con.OpenWithMSI(); //// con.Open();  
                    cmd = new SqlCommand("GetFacilityVnet", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LocationId", SqlDbType.Int, 100).Value = locationId;
                    string vnet = Convert.ToString(cmd.ExecuteScalar());
                    return vnet;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    con?.Close();
                }
            }

            /// <summary>
            /// Check Contractor Number Exist
            /// </summary>
            /// <param name="contractorNumber">contractor Number</param>  
            /// <param name="locationId">location id</param>  
            /// <returns>boolean value</returns> 
            public bool CheckContratorNumberExist(string contractorNumber, string locationId)
            {
                SqlAzureDatabase sqlConn;
                DbCommand dbcmd;
                PrintStatusDetails printDetails = null;
                bool status = false;
                try
                {
                    printDetails = new PrintStatusDetails();
                    sqlConn = new SqlAzureDatabase(ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    dbcmd = sqlConn.GetStoredProcCommand("CheckExistContractorNumber");
                    dbcmd.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dbcmd, "@contractorNumber", SqlDbType.Int, contractorNumber);
                    sqlConn.AddInParameter(dbcmd, "@LocationId", SqlDbType.Int, locationId);
                    using (IDataReader dbreader = sqlConn.ExecuteReader(dbcmd))
                    {
                        while (dbreader.Read())
                        {
                            if (Convert.ToString(dbreader[0]) == "true")
                            {
                                status = true;
                            }
                        }
                    }
                 
                    return status;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Check Contractor Number Exist For Edit
            /// </summary>
            /// <param name="strContractorId">Contractor id</param>  
            /// <param name="contractorNumber">contractor Number</param>  
            /// <param name="locationId">location Id</param>  
            /// <returns>boolean value</returns> 
            public bool CheckContratorNumberExistForEdit(string strContractorId, string contractorNumber, string locationId)
            {
                SqlAzureDatabase sqlConn;
                DbCommand dbcmd;
                PrintStatusDetails printDetails = null;
                bool status = false;
                try
                {
                    printDetails = new PrintStatusDetails();
                    sqlConn = new SqlAzureDatabase(ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString());
                    dbcmd = sqlConn.GetStoredProcCommand("CheckExistContractorNumberForEdit");
                    dbcmd.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dbcmd, "@contractorID", SqlDbType.VarChar, strContractorId);
                    if (string.IsNullOrEmpty(contractorNumber))
                    {
                        sqlConn.AddInParameter(dbcmd, "@contractorNumber", SqlDbType.Int, DBNull.Value);
                    }
                    else
                    {
                        sqlConn.AddInParameter(dbcmd, "@contractorNumber", SqlDbType.Int, Convert.ToInt32(contractorNumber));
                    }

                    sqlConn.AddInParameter(dbcmd, "@LocationId", SqlDbType.Int, Convert.ToInt32(locationId));
                    using (IDataReader dbreader = sqlConn.ExecuteReader(dbcmd))
                    {
                        while (dbreader.Read())
                        {
                            if (Convert.ToString(dbreader[0]) == "true")
                            {
                                status = true;
                            }
                        }
                    }
                    dbcmd.Connection?.Close();
                    return status;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="contractorId">contractor Id</param>  
            /// <param name="hostId">host id</param>  
            /// <param name="locationId">location Id</param>  
            /// <returns>data set</returns> 
            public int SaveContractorPrintDetails(int contractorId, string hostId, string locationId)
            {
                SqlConnection sqlConn = null;
                try
                {
                    string strConnectionString = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    sqlConn = new SqlConnection(strConnectionString);
                    sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    string strRequestInsertSP = "IVS_SaveContractorPrintDetails";
                    SqlCommand sqlComm = new SqlCommand(strRequestInsertSP, sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.Add("@ContractorId", SqlDbType.Int, 50).Value = contractorId;
                    sqlComm.Parameters.Add("@HostId", SqlDbType.VarChar, 50).Value = hostId;
                    sqlComm.Parameters.Add("@UpdatedOn", SqlDbType.DateTime).Value = DateTime.Now;
                    sqlComm.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;

                    int count = (int)sqlComm.ExecuteNonQuery();
                    return count;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        sqlConn?.Close();
                    }
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="requestid">request id</param>  
            /// <returns>data set</returns> 
            public int GetAccessCardnumber(int requestid)
            {
                SqlConnection sqlConn = null;
                try
                {
                    string strConnectionString = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    sqlConn = new SqlConnection(strConnectionString);
                    sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    string strgetaccesscardnoSP = "GetAccessCardnumber";
                    SqlCommand sqlComm = new SqlCommand(strgetaccesscardnoSP, sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.Add("@RequestID", SqlDbType.Int, 50).Value = requestid;
                    int accesscardnumber = (int)sqlComm.ExecuteScalar();
                    return accesscardnumber;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        sqlConn?.Close();
                    }
                }
            }

            /// <summary>
            /// Save Contractor ID Details
            /// </summary>
            /// <param name="contractorName">contractor Name</param>  
            /// <param name="contractorNumber">contractor Number</param>  
            /// <param name="vendorName">vendor Name</param>  
            /// <param name="status">status value</param>  
            /// <param name="superVisiorPhone">super Visitor Phone</param>  
            /// <param name="vendorPhoneNumber">vendor Phone Number</param>  
            /// <param name="docStatus">document Status</param>
            /// <param name="securityID">security ID</param>  
            /// <param name="currDate">current date</param>  
            /// <param name="locationId">location Id</param>  
            /// <returns>integer value</returns> 
            public int SaveContractorIDDetails(string contractorName, string contractorNumber, string vendorName, string status, string superVisiorPhone, string vendorPhoneNumber, string docStatus, string securityID, DateTime currDate, string locationId)
            {
                SqlConnection sqlConn = null;
                int id;
                int bitStatus = 0;
                if (status == "Active")
                {
                    bitStatus = 1;
                }
                else
                {
                    bitStatus = 0;
                }

                try
                {
                    string strConnectionString = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    sqlConn = new SqlConnection(strConnectionString);
                    sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    string strRequestInsertSP = "IVS_SaveContractor";
                    SqlCommand sqlComm = new SqlCommand(strRequestInsertSP, sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.Add("@ContractorNumber", SqlDbType.VarChar, 50).Value = contractorNumber;
                    sqlComm.Parameters.Add("@ContractorName", SqlDbType.VarChar, 50).Value = contractorName;
                    sqlComm.Parameters.Add("@VendorName", SqlDbType.VarChar, 50).Value = vendorName;
                    sqlComm.Parameters.Add("@Status", SqlDbType.Bit, 50).Value = bitStatus;
                    sqlComm.Parameters.Add("@SuperVisiorPhone", SqlDbType.VarChar, 50).Value = superVisiorPhone;
                    sqlComm.Parameters.Add("@VendorPhoneNumber", SqlDbType.VarChar, 50).Value = vendorPhoneNumber;
                    sqlComm.Parameters.Add("@DOCStatus", SqlDbType.VarChar, 50).Value = docStatus;
                    sqlComm.Parameters.Add("@SecurityID", SqlDbType.VarChar, 10).Value = securityID;
                    sqlComm.Parameters.Add("@LocationId", SqlDbType.VarChar, 10).Value = locationId;
                    SqlParameter output = new SqlParameter("@ContractorId", SqlDbType.Int);
                    output.Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add(output);
                    sqlComm.ExecuteNonQuery();
                    id = Convert.ToInt32(output.Value);
                    return id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConn?.Close();
                }
            }

            /// <summary>
            /// Safety Permit Security Search
            /// </summary>
            /// <param name="contractorName">contractor Name</param>  
            /// <param name="contractorNumber">contractor Number</param>  
            /// <param name="vendorName">vendor Name</param>  
            /// <param name="status">status value</param> 
            /// <param name="superVisiorPhone">supervisor Phone</param> 
            /// <param name="vendorPhoneNumber">vendor Phone Number</param> 
            /// <param name="docStatus">document Status</param> 
            /// <param name="securityID">security ID</param> 
            /// <param name="currDate">current date</param> 
            /// <param name="strLocationId">Location Id</param> 
            /// <returns>data set</returns> 
            public bool InsertContractorIDDetails(string contractorName, string contractorNumber, string vendorName, string status, string superVisiorPhone, string vendorPhoneNumber, string docStatus, string securityID, DateTime currDate, string strLocationId)
            {
                int bitStatus = 0;
                if (status == "Active")
                {
                    bitStatus = 1;
                }
                else
                {
                    bitStatus = 0;
                }

                try
                {
                    string strConnectionString = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                    SqlConnection sqlConn = new SqlConnection(strConnectionString);
                    sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    string strRequestInsertSP = "IVS_SaveContractorInformation";
                    SqlCommand sqlComm = new SqlCommand(strRequestInsertSP, sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.Add("@ContractorNumber", SqlDbType.VarChar, 50).Value = contractorNumber;
                    sqlComm.Parameters.Add("@ContractorName", SqlDbType.VarChar, 50).Value = contractorName;
                    sqlComm.Parameters.Add("@VendorName", SqlDbType.VarChar, 50).Value = vendorName;
                    sqlComm.Parameters.Add("@Status", SqlDbType.Bit, 50).Value = bitStatus;
                    sqlComm.Parameters.Add("@SuperVisiorPhone", SqlDbType.VarChar, 50).Value = superVisiorPhone;
                    sqlComm.Parameters.Add("@VendorPhoneNumber", SqlDbType.VarChar, 50).Value = vendorPhoneNumber;
                    sqlComm.Parameters.Add("@DOCStatus", SqlDbType.VarChar, 50).Value = docStatus;
                    sqlComm.Parameters.Add("@SecurityID", SqlDbType.VarChar, 10).Value = securityID;
                    sqlComm.Parameters.Add("@LocationID", SqlDbType.VarChar, 10).Value = strLocationId;

                    // sqlComm.Parameters.Add("@Date", SqlDbType.DateTime, 50).Value = currDate;
                    // sqlComm.Parameters.AddWithValue("@Date", SqlDbType.DateTime);
                    int count = Convert.ToInt32(sqlComm.ExecuteScalar());
                    sqlConn.Close();
                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    return false;
                }
            }

            /// <summary>
            /// Update Proof Details
            /// </summary>
            /// <param name="objVisitorProof">object Visitor Proof</param>  
            /// <returns>boolean value</returns> 
            private bool UpdateProofDetails(VMSBusinessEntity.VisitorProof objVisitorProof)
            {
                string strConnectionString = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                SqlConnection sqlConn = new SqlConnection(strConnectionString);
                try
                {
                    sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    string strRequestUpdateQuery = "UPDATE VisitorProof Set VisitorID = '" + Convert.ToInt32(objVisitorProof.VisitorID) + "',";
                    strRequestUpdateQuery += "Photo = '" + Convert.ToString(objVisitorProof.Photo) + "',";

                    // strRequestUpdateQuery += "ProofID = '" + Convert.ToInt32(objVisitorProof.ProofID) + "',";
                    // strRequestUpdateQuery += "IDProofImage = '" + Convert.ToString(objVisitorProof.IDProofImage) + "' ";
                    strRequestUpdateQuery += " WHERE VisitorID = " + objVisitorProof.VisitorID;
                    SqlCommand sqlComm = new SqlCommand(strRequestUpdateQuery, sqlConn);
                    int recAffected = sqlComm.ExecuteNonQuery();
                    sqlConn.Close();
                    if (recAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConn?.Close();
                }
            }

            /// <summary>
            /// Update Request Details
            /// </summary>
            /// <param name="objVisitorRequest">object Visitor Request</param>  
            /// <param name="equipcustody">equipment custody</param>  
            /// <returns>boolean value</returns> 
            private bool UpdateRequestDetails(VMSBusinessEntity.VisitorRequest objVisitorRequest, int equipcustody)
            {
                int recAffected = 0;
                try
                {
                    string strConnectionString = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlConnection sqlConn = new SqlConnection(strConnectionString);
                    sqlConn.OpenWithMSI(); //// sqlConn.Open();
                    string strRequestUpdateQuery = "UPDATE VisitorRequest Set RequestedDate = '" + Convert.ToString(objVisitorRequest.RequestedDate) + "',";
                    strRequestUpdateQuery += "LocationId = '" + Convert.ToString(objVisitorRequest.LocationId) + "',";
                    strRequestUpdateQuery += "Purpose = '" + Convert.ToString(objVisitorRequest.Purpose) + "',";
                    strRequestUpdateQuery += "HostID = '" + Convert.ToString(objVisitorRequest.HostID) + "',";
                    strRequestUpdateQuery += "HostContactNo = '" + Convert.ToString(objVisitorRequest.HostContactNo) + "',";
                    strRequestUpdateQuery += "FromDate = '" + Convert.ToString(objVisitorRequest.FromDate) + "',";
                    strRequestUpdateQuery += "ToDate = '" + Convert.ToString(objVisitorRequest.ToDate) + "',";
                    strRequestUpdateQuery += "FromTime = '" + Convert.ToString(objVisitorRequest.FromTime) + "',";
                    strRequestUpdateQuery += "ToTime = '" + Convert.ToString(objVisitorRequest.ToTime) + "',";

                    // commented by uma --start
                    // strRequestUpdateQuery += "ActualOutTime = '" + Convert.ToString(objVisitorRequest.ActualOutTime) + "',";
                    // end
                    // strRequestUpdateQuery += "Escort = '" + Convert.ToString(objVisitorRequest.Escort) + "',";
                    // strRequestUpdateQuery += "VehicleNo = '" + Convert.ToString(objVisitorRequest.VehicleNo) + "',";
                    strRequestUpdateQuery += "RequestStatus = '" + Convert.ToString(objVisitorRequest.RequestStatus) + "',";
                    if (objVisitorRequest.Comments != null)
                    {
                        strRequestUpdateQuery += "Comments = '" + Convert.ToString(objVisitorRequest.Comments).Replace("'", "''") + "',";
                    }
                    else
                    {
                        strRequestUpdateQuery += "Comments = null,";
                    }

                    strRequestUpdateQuery += "LastUpdatedby = '" + Convert.ToString(objVisitorRequest.LastUpdatedby) + "',";
                    strRequestUpdateQuery += "LastUpdatedDate = getdate(),";
                    strRequestUpdateQuery += "Status = '" + objVisitorRequest.Status + "',";
                    strRequestUpdateQuery += "Offset= '" + objVisitorRequest.Offset + "',";
                    strRequestUpdateQuery += "HostDepartment = '" + Convert.ToString(objVisitorRequest.HostDepartment).Replace("'", "''") + "'";
                    strRequestUpdateQuery += " WHERE RequestID = " + objVisitorRequest.RequestID;
                    SqlCommand sqlComm = new SqlCommand(strRequestUpdateQuery, sqlConn);
                    recAffected = sqlComm.ExecuteNonQuery();
                    sqlConn?.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (recAffected > 0 || (equipcustody == 1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// database connection
            /// </summary>
            public class DBConnection
            {
                /// <summary>
                /// Purpose : Method to retrieve connection string from config file and send to calling place.        
                /// </summary>
                /// <returns>string value</returns> 
                public static string Connectionstring()
                {
                    string conn = string.Empty;
                    try
                    {
                        conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                        return conn;
                    }
                    catch (ConfigurationException ex)
                    {
                        throw ex;
                    }
                }
            }


        }

      
    }
}
