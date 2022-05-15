
// This file contains EmployeeDL class.
// </summary>
//-----------------------------------------------------------------------
namespace VMSDataLayer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
    using AzureSQLHelper;
    using EnterpriseLibConn;
    using System.Globalization;

    /// <summary>
    /// Employee Data layer
    /// </summary>
    public class EmployeeDL
    {
        #region UpdateUserImgInDB
        /// <summary>
        /// Method to Update Images of Associate
        /// </summary>
        /// <param name="strAssociateID">Associate Id</param>
        /// <param name="strEncryptedBinaryData">Encrypted binary data</param>  
        /// <returns>boolean value</returns>
        public bool UpdateUserImgInDB(string strAssociateID, string strEncryptedBinaryData)
        {
            try
            {
                string sqlUpdateAssociateImg = string.Empty;
                sqlUpdateAssociateImg = "IVS_UpdateUserImage_App";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                
                DbCommand dbloginUserNamecmd = sqlConn.GetStoredProcCommand(sqlUpdateAssociateImg);

                sqlConn.AddInParameter(dbloginUserNamecmd, "AssociateID", SqlDbType.VarChar, strAssociateID);
                sqlConn.AddInParameter(dbloginUserNamecmd, "BinaryImage", SqlDbType.VarChar, strEncryptedBinaryData);

                dbloginUserNamecmd.CommandType = CommandType.StoredProcedure;
                sqlConn.ExecuteNonQuery(dbloginUserNamecmd);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SubmitImageChangeRequest

        /// <summary>
        /// Submit Image Change Request
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>  
        /// <param name="strEncryptedBinaryData">Encrypted Binary Data</param>  
        /// <param name="strUserId">User Id</param>  
        /// <param name="strComments">Comments Status</param>  
        /// <returns>data set</returns> 
        public bool SubmitImageChangeRequest(string strAssociateID, string strEncryptedBinaryData, string strUserId, string strComments)
        {
            DataSet dsresult = new DataSet();

            string sqlSubmitImageRequest = "IVS_SubmitImageRequest";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbsubmitRequest = sqlConn.GetStoredProcCommand(sqlSubmitImageRequest);
                dbsubmitRequest.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbsubmitRequest, "AssociateID", SqlDbType.VarChar, strAssociateID);
                sqlConn.AddInParameter(dbsubmitRequest, "BinaryImage", SqlDbType.Text, strEncryptedBinaryData);
                sqlConn.AddInParameter(dbsubmitRequest, "UserId", SqlDbType.VarChar, strUserId);
                sqlConn.AddInParameter(dbsubmitRequest, "AssociateComments", SqlDbType.VarChar, strComments);
                dsresult = sqlConn.ExecuteDataSet(dbsubmitRequest);
                if (dsresult.Tables.Count > 0)
                {
                    if (dsresult.Tables[0].Rows[0]["Result"].ToString().Equals("1"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult.Dispose();
            }
        }
        #endregion

        #region ImgChangeRequestDetails

        /// <summary>
        /// Image change request status
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>  
        /// <returns>data set</returns> 
        public DataSet ImgChangeRequestDetails(string strAssociateID)
        {
            DataSet dsresult = new DataSet();
            string sqlSubmitImageRequest = "IVS_GetImageChangeRequestDetails";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbsubmitRequest = sqlConn.GetStoredProcCommand(sqlSubmitImageRequest);
                dbsubmitRequest.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbsubmitRequest, "AssociateID", SqlDbType.VarChar, strAssociateID);
                dsresult = sqlConn.ExecuteDataSet(dbsubmitRequest);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Image change request status
        /// </summary>
        /// <returns>data set</returns> 
        public DataSet GetApproverComments()
        {
            DataSet dsresult = new DataSet();
            string sqlSubmitImageRequest = "IVS_GetApproverReason";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbsubmitRequest = sqlConn.GetStoredProcCommand(sqlSubmitImageRequest);
                dbsubmitRequest.CommandType = CommandType.StoredProcedure;
                dsresult = sqlConn.ExecuteDataSet(dbsubmitRequest);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult.Dispose();
            }
        }

        #region ImgChangeRequestDetailswithRequestID

        /// <summary>
        /// image change request details with request id
        /// </summary>
        /// <param name="requestID">request id</param>  
        /// <returns>data set</returns> 
        public DataSet ImgChangeRequestDetailswithReqID(int requestID)
        {
            DataSet dsresult = new DataSet();
            string sqlSubmitImageRequest = "IVS_GetImageChangeRequestDetailsWithRequestID";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbsubmitRequest = sqlConn.GetStoredProcCommand(sqlSubmitImageRequest);
                dbsubmitRequest.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbsubmitRequest, "RequestID", SqlDbType.BigInt, requestID);
                dsresult = sqlConn.ExecuteDataSet(dbsubmitRequest);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult.Dispose();
            }
        }

        /// <summary>
        /// Manage Image Outlook Integration
        /// </summary>
        /// <param name="associateId">associate id</param>
        /// <param name="isIntegrated">is integrated</param>
        /// <returns>boolean value</returns>
        public bool ManageImageOutlookIntegration(string associateId, bool isIntegrated)
        {
            string sqlManageoutlookIntegrationproc = "IVS_ManagePhotoIntegrationWithOutlook";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);

                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlManageoutlookIntegrationproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "AssociateID", SqlDbType.VarChar, associateId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "Outlookflag", SqlDbType.Bit, isIntegrated == true ? 1 : 0);
                sqlConn.ExecuteNonQuery(dbemployeeInfoComm);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetUploadedImageDetails

        /// <summary>
        /// Method to retrieve Upload Employee Image details        
        /// </summary>       
        /// <param name="requestID">request id</param> 
        /// <returns>data table</returns>
        public string GetUploadedImageDetails(string requestID)
        {
            string sqlGetEmployeeInfoproc = "IVS_GetUploadedImageWithReqID";
            DataSet dsuploadedImage = new DataSet();
            string imageData = "NoImageAvailable";
            string fileContentId = string.Empty;
            bool isFileContentId = true;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "Request_ID", SqlDbType.VarChar, requestID);
                dsuploadedImage = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                if (dsuploadedImage.Tables.Count > 0)
                {
                    if (dsuploadedImage.Tables[0].Rows.Count > 0)
                    {
                        fileContentId = dsuploadedImage.Tables[0].Rows[0]["FileContentId"].ToString();
                        imageData = dsuploadedImage.Tables[0].Rows[0]["UploadedImage"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(fileContentId))
                {
                    isFileContentId = true;
                }
                else
                {
                    isFileContentId = false;
                }

                sqlConn = null;
                return imageData + "|" + fileContentId + "|" + isFileContentId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsuploadedImage = null;
            }
        }
        #endregion

        #region GetAllImageChangeRequests

        /// <summary>
        /// Get all image change request.
        /// </summary>
        /// <param name="startIndex">start Index</param>  
        /// <param name="pageSize">page Size</param>  
        /// <param name="sortBy">sort By</param>  
        /// <returns>Image Requests</returns> 
        public DataSet GetAllImageChangeRequests()
        {
            string sqlGetEmployeeInfoproc = "IVS_GetImageChangeRequests";
            DataSet dsimageRequests = new DataSet();
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                ////sqlConn.AddInParameter(dbemployeeInfoComm, "startIndex", SqlDbType.Int, startIndex);
                ////sqlConn.AddInParameter(dbemployeeInfoComm, "pageSize", SqlDbType.Int, pageSize);
                ////sqlConn.AddInParameter(dbemployeeInfoComm, "sortBy", SqlDbType.NVarChar, sortBy);
                dsimageRequests = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                return dsimageRequests;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsimageRequests = null;
            }
        }
        #endregion

        #region ApproveImageUploadRequest

        /// <summary>
        /// Safety Permit Security Search
        /// </summary>
        /// <param name="intRequestID">Request ID</param>  
        /// <param name="strAdminId">Admin Id</param>  
        /// <param name="strApproverComments">Approver Comments</param>  
        /// <returns>boolean value</returns> 
        public bool ApproveImageUploadRequest(int intRequestID, string strAdminId, string strApproverComments)
        {
            string sqlGetRequestInfoproc = "IVS_ApproveImageRequest";
            DataSet dsresult = new DataSet();
            bool result = false;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbrequestInfoComm = sqlConn.GetStoredProcCommand(sqlGetRequestInfoproc);
                dbrequestInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbrequestInfoComm, "RequestID", SqlDbType.BigInt, intRequestID);
                sqlConn.AddInParameter(dbrequestInfoComm, "UserID", SqlDbType.VarChar, strAdminId);
                sqlConn.AddInParameter(dbrequestInfoComm, "ApproverComments", SqlDbType.VarChar, strApproverComments);
                dsresult = sqlConn.ExecuteDataSet(dbrequestInfoComm);

                if (dsresult.Tables.Count > 0)
                {
                    if (dsresult.Tables[0].Rows.Count > 0)
                    {
                        result = Convert.ToBoolean(dsresult.Tables[0].Rows[0]["Result"].ToString());
                    }
                }

                sqlConn = null;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult = null;
            }
        }
        #endregion

        #region RejectImageUploadRequest

        /// <summary>
        /// Reject Image Upload Request
        /// </summary>
        /// <param name="intRequestID">request id</param>  
        /// <param name="strAdminId">admin id</param>  
        /// <param name="strApproverComments">Approver Comments</param>  
        /// <returns>data set result</returns> 
        public bool RejectImageUploadRequest(int intRequestID, string strAdminId, string strApproverComments)
        {
            string sqlGetRequestInfoproc = "IVS_RejectImageRequest";
            DataSet dsresult = new DataSet();
            bool result = false;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbrequestInfoComm = sqlConn.GetStoredProcCommand(sqlGetRequestInfoproc);
                dbrequestInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbrequestInfoComm, "RequestID", SqlDbType.BigInt, intRequestID);
                sqlConn.AddInParameter(dbrequestInfoComm, "UserID", SqlDbType.VarChar, strAdminId);
                sqlConn.AddInParameter(dbrequestInfoComm, "ApproverComments", SqlDbType.VarChar, strApproverComments);
                dsresult = sqlConn.ExecuteDataSet(dbrequestInfoComm);
                if (dsresult.Tables.Count > 0)
                {
                    if (dsresult.Tables[0].Rows.Count > 0)
                    {
                        result = Convert.ToBoolean(dsresult.Tables[0].Rows[0]["Result"].ToString());
                    }
                }

                sqlConn = null;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult = null;
            }
        }
        #endregion

        #region BulkUploadAssocaiteImages

        /// <summary>
        /// Get Bulk Upload Id
        /// </summary>
        /// <param name="strAdminID">Admin ID</param>  
        /// <returns>integer value</returns> 
        public int GetBulkUploadId(string strAdminID)
        {
            int intBulkUploadID = 0;
            try
            {
                string sqlUpdateAssociateImg = string.Empty;
                sqlUpdateAssociateImg = "IVS_GetBulkUploadID";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand objDBCommand = sqlConn.GetStoredProcCommand(sqlUpdateAssociateImg);
                sqlConn.AddInParameter(objDBCommand, "AdminID", SqlDbType.VarChar, strAdminID);
                sqlConn.AddOutParameter(objDBCommand, "BulkUploadID", SqlDbType.BigInt, intBulkUploadID);
                objDBCommand.CommandType = CommandType.StoredProcedure;
                sqlConn.ExecuteNonQuery(objDBCommand);
                intBulkUploadID = Convert.ToInt32(sqlConn.GetParameterValue(objDBCommand, "BulkUploadID").ToString());
                return intBulkUploadID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert Image upload Details
        /// </summary>
        /// <param name="intBulkUploadID">Bulk Upload ID</param>  
        /// <param name="strFileName">File Name</param>  
        /// <param name="isSuccess">is Success</param> 
        /// <param name="intErrorMessage">Error Message</param>  
        public void InsertImageuploadDetails(
            int intBulkUploadID, 
            string strFileName, 
            bool isSuccess, 
            int intErrorMessage)
        {
            try
            {
                string sqlUpdateAssociateImg = string.Empty;
                sqlUpdateAssociateImg = "IVS_InsertImageUploadDetails";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand objDBCommand = sqlConn.GetStoredProcCommand(sqlUpdateAssociateImg);

                sqlConn.AddInParameter(objDBCommand, "BulkUploadID", SqlDbType.BigInt, intBulkUploadID);
                sqlConn.AddInParameter(objDBCommand, "FileName", SqlDbType.VarChar, strFileName);
                sqlConn.AddInParameter(objDBCommand, "IsSuccess", SqlDbType.VarChar, isSuccess);
                sqlConn.AddInParameter(objDBCommand, "MessageID", SqlDbType.Int, intErrorMessage);

                objDBCommand.CommandType = CommandType.StoredProcedure;
                sqlConn.ExecuteNonQuery(objDBCommand);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Bulk Upload Details
        /// </summary>
        /// <param name="intBulkUploadID">Bulk Upload ID</param>  
        /// <returns>data set</returns> 
        public DataSet GetBulkUploadDetails(int intBulkUploadID)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                string sqlUpdateAssociateImg = string.Empty;
                sqlUpdateAssociateImg = "IVS_GetBulkUploadDetails";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand objDBCommand = sqlConn.GetStoredProcCommand(sqlUpdateAssociateImg);
                sqlConn.AddInParameter(objDBCommand, "BulkUploadID", SqlDbType.BigInt, intBulkUploadID);
                objDBCommand.CommandType = CommandType.StoredProcedure;
                dsreturn = sqlConn.ExecuteDataSet(objDBCommand);
                return dsreturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// next bulk upload details
        /// </summary>
        /// <param name="intBulkUploadID">Bulk Upload ID</param>  
        /// <param name="intLastID">Last id</param>  
        /// <param name="intCount">count value</param>  
        /// <returns>data set</returns> 
        public DataSet GetNextBulkUploadDetails(int intBulkUploadID, int intLastID, int intCount)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                string sqlUpdateAssociateImg = string.Empty;
                sqlUpdateAssociateImg = "IVS_GetNextBulkUploadDetails";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand objDBCommand = sqlConn.GetStoredProcCommand(sqlUpdateAssociateImg);
                sqlConn.AddInParameter(objDBCommand, "BulkUploadID", SqlDbType.BigInt, intBulkUploadID);
                sqlConn.AddInParameter(objDBCommand, "LastID", SqlDbType.BigInt, intLastID);
                sqlConn.AddInParameter(objDBCommand, "Count", SqlDbType.Int, intCount);
                objDBCommand.CommandType = CommandType.StoredProcedure;
                dsreturn = sqlConn.ExecuteDataSet(objDBCommand);
                return dsreturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Delete or view photo
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>    
        /// <returns>boolean value</returns> 
        public bool DeletePhoto(string strAssociateID)
        {
            try
            {
                string sqlUpdateAssociateImg = string.Empty;
                sqlUpdateAssociateImg = "IVS_DeletePhoto_App";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbloginUserNamecmd = sqlConn.GetStoredProcCommand(sqlUpdateAssociateImg);
                sqlConn.AddInParameter(dbloginUserNamecmd, "AssociateID", SqlDbType.VarChar, strAssociateID);
                dbloginUserNamecmd.CommandType = CommandType.StoredProcedure;
                sqlConn.ExecuteNonQuery(dbloginUserNamecmd);
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

        #region SaveAssociateInformation
        /// <summary>
        /// Method to Save Associate Information for IVS
        /// </summary>
        /// <param name="strAssociateID">associate id</param>
        /// <param name="strCheckStatus">Check Status</param> 
        /// <param name="intCardcount">card count</param>
        /// <param name="strAdminID">admin id</param> 
        /// <param name="strLocationID">location id</param>        
        /// <returns>SaveFlag string</returns>
        public ArrayList SaveAssociateInformation(string strAssociateID, string strCheckStatus, int intCardcount, string strAdminID, string strLocationID)
        {
            string strSql = "IVS_SaveAssociateDetails";
            int saveFlag = 0;
            string strDetailId = string.Empty;
            ArrayList list = new ArrayList();

            // DataSet dsReturnValues;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);

                // 181795 code starts here
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "CheckStatus", DbType.String, strCheckStatus);

                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "PassReturnedDate", DbType.String, PassReturnedDate);
                // 181795 code ends here
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "CardNo", DbType.Int16, intCardcount);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassIssuedBy", DbType.String, strAdminID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationID", DbType.String, strLocationID);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "SaveFlag", SqlDbType.Int, 4);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "RequestID", SqlDbType.VarChar, 7);
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);
                saveFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "SaveFlag").ToString());
                strDetailId = sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "RequestID").ToString();
                list.Add(saveFlag.ToString());
                list.Add(strDetailId);
                return list;
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

        // End Changes IVS CR006 Vimal
        #endregion

        #region IsCardIssuedLocation

        /// <summary>
        /// Is Card Issued Location
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>  
        /// <param name="intLocationID">Location ID</param>  
        /// <returns>string value</returns> 
        public string IsCardIssuedLocation(string strAssociateID, int intLocationID)
        {
            string strSql = "IVS_IsCardIssuedLocation";
            DataSet dsreturnValues;
            string strRetValue = string.Empty;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "Location", DbType.Int16, intLocationID);
                dsreturnValues = sqlConn.ExecuteDataSet(dbemployeeDetailsSaveCmd);
                if (dsreturnValues != null)
                {
                    if (dsreturnValues.Tables[0].Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dsreturnValues.Tables[0].Rows[0]["PassReturnedBy"].ToString()))
                        {
                            strRetValue = string.Concat(dsreturnValues.Tables[0].Rows[0]["PassDetailID"].ToString(), "|1");
                        }
                        else
                        {
                            strRetValue = string.Concat(dsreturnValues.Tables[0].Rows[0]["PassDetailID"].ToString(), "|0");
                        }
                    }
                }

                return strRetValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CheckInAssociate

        /// <summary>
        /// Safety Permit Security Search
        /// </summary>
        /// <param name="associateID">associate ID</param>  
        /// <param name="stradminID">admin id</param>  
        /// <param name="strlocationID">location ID</param>  
        /// <param name="strReason">reason Status</param>  
        /// <param name="dtfromDate1">From Date</param>  
        /// <param name="dtfromDate">From Date value</param>  
        /// <param name="dttoDate">to date</param>  
        /// <param name="cardType">card Type</param>  
        /// <returns>data set</returns> 
        public string CheckInAssociate(string associateID, string stradminID, string strlocationID, string strReason, DateTime dtfromDate1, DateTime dtfromDate, DateTime dttoDate, string cardType)
        {
            string strSql = "IVS_CheckInAssociate";

            // int SaveFlag = 0;
            string strDetailId = string.Empty;

            // ArrayList list = new ArrayList();
            // DataSet dsReturnValues;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;

                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, associateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassIssuedBy", DbType.String, stradminID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationID", DbType.String, strlocationID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "Reason", DbType.Int16, strReason);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "FromDate1", DbType.DateTime, dtfromDate1);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "FromDate", DbType.DateTime, dtfromDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "ToDate", DbType.DateTime, dttoDate);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "RequestID", SqlDbType.VarChar, 7);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "Cardtype", DbType.String, cardType);
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);

                // SaveFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbEmployeeDetailsSaveCmd, "SaveFlag").ToString());
                strDetailId = sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "RequestID").ToString();

                // list.Add(SaveFlag.ToString());
                // list.Add(PassDetailsID);
                return strDetailId;
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
        /// Duplicate Entry.
        /// </summary>
        /// <param name="associateID">associate id</param>
        /// <param name="strAdminID">admin id</param>
        /// <param name="strLocationID">location id</param>
        /// <param name="strReason">string reason</param>
        /// <param name="dtfromDate">from date</param>
        /// <param name="dttoDate">to date</param>
        /// <returns>data table</returns>
        public int DuplicateIVSEntry(string associateID, string strAdminID, string strLocationID, string strReason, DateTime dtfromDate, DateTime dttoDate)
        {
            string strSql = "IVS_DuplicateIVSEntry";

            // int SaveFlag = 0;
            int passDetailsID = 0;

            // ArrayList list = new ArrayList();
            // DataSet dsReturnValues;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;

                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, associateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassIssuedBy", DbType.String, strAdminID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationID", DbType.String, strLocationID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "Reason", DbType.Int16, strReason);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "FromDate", DbType.DateTime, dtfromDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "ToDate", DbType.DateTime, dttoDate);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "DuplicateId", SqlDbType.Bit, 1);
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);

                // SaveFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbEmployeeDetailsSaveCmd, "SaveFlag").ToString());
                passDetailsID = Convert.ToInt32(sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "DuplicateId"));

                // list.Add(SaveFlag.ToString());
                // list.Add(PassDetailsID);
                return passDetailsID;
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
        /// Save Log Down Load Photo
        /// </summary>
        /// <param name="associateID">associate ID</param>  
        /// <param name="adminID">admin id</param>  
        public void SaveLogDownLoadPhoto(string associateID, string adminID)
        {
            string strSql = "IVS_LogDownLoadPhoto";

            // int SaveFlag = 0;
            string strDetailId = string.Empty;

            // ArrayList list = new ArrayList();
            // DataSet dsReturnValues;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dblogDownLoadPhoto = sqlConn.GetStoredProcCommand(strSql);
                dblogDownLoadPhoto.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dblogDownLoadPhoto, "AssociateID", DbType.String, associateID);
                sqlConn.AddInParameter(dblogDownLoadPhoto, "DownloadedBy", DbType.String, adminID);
                sqlConn.ExecuteNonQuery(dblogDownLoadPhoto);
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
        /// To update print status
        /// </summary>
        /// <param name="associateID">associate id</param>
        /// <param name="adminID">admin id</param>
        /// <param name="printStatus">print status</param>
        /// <param name="location">location name</param>
        public void UpdatePrintStatus(string associateID, string adminID, string printStatus, int location)
        {
            string strSql = "IVS_LogInsertIDCardDetails";

            // int SaveFlag = 0;
            string strDetailId = string.Empty;

            // ArrayList list = new ArrayList();
            // DataSet dsReturnValues;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dblogDownLoadPhoto = sqlConn.GetStoredProcCommand(strSql);
                dblogDownLoadPhoto.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dblogDownLoadPhoto, "ADMINID", DbType.String, adminID);
                sqlConn.AddInParameter(dblogDownLoadPhoto, "ASSOCIATE_ID", DbType.String, associateID);
                sqlConn.AddInParameter(dblogDownLoadPhoto, "IssuedLocation", DbType.Int32, location);
                sqlConn.AddInParameter(dblogDownLoadPhoto, "PrintStatus", DbType.String, printStatus);
                sqlConn.ExecuteNonQuery(dblogDownLoadPhoto);
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
        #endregion
        #region CheckInAssociate

        /// <summary>
        /// Reminder Details
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>  
        /// <param name="strInDate">in date</param>  
        /// <param name="strInTime">In Time</param>  
        /// <param name="strSecurityID">security id</param>  
        /// <param name="passDetailId">pass detail id</param>  
        public void ReminderDetails(string strAssociateID, string strInDate, string strInTime, string strSecurityID, string passDetailId)
        {
            string strSql = "IVS_ReminderDetails";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "InDate", DbType.DateTime, DateTime.Now);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "ReminderTime", DbType.DateTime, DateTime.Now);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "MailSentBy", DbType.String, strSecurityID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassDetailID", DbType.Int32, Convert.ToInt32(passDetailId));
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);
                return;
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

        #endregion

        #region CheckOutAssociate

        /// <summary>
        /// Check Out Associate
        /// </summary>
        /// <param name="passNumber">pass number</param>  
        /// <param name="strAdminID">admin id</param>  
        /// <param name="badgeStatus">badge status</param>  
        /// <param name="bdgestatusDesr">badge status description</param> 
        /// <param name="strLocation">string location</param> 
        /// <returns>data set</returns> 
        public string CheckOutAssociate(string passNumber, string strAdminID, string badgeStatus, string bdgestatusDesr, string strLocation,DateTime currenttime)
        {
            string CurrentDate = currenttime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string strSql = "IVS_CheckOutAssociate";
            int passDetailParentID = 0;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassID", SqlDbType.BigInt, passNumber);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassIssuedBy", SqlDbType.VarChar, strAdminID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "BadgeStatus", SqlDbType.VarChar, badgeStatus);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "BadgeStatus_desc", SqlDbType.VarChar, bdgestatusDesr);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "ReturnPassParent", SqlDbType.BigInt, passDetailParentID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "Location", SqlDbType.Int, Convert.ToInt32(strLocation));
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "CurDate", SqlDbType.VarChar, CurrentDate);
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);
                passDetailParentID = Convert.ToInt32(sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "ReturnPassParent"));
                return Convert.ToString(passDetailParentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Get lost Status
        /// </summary>
        /// <param name="passdetailid">pass detail id</param>  
        /// <returns>data set</returns> 
        #region IVS_GetlostStatus
        public DataSet GetlostStatus(int passdetailid)
        {
            string strSql = "IVS_GetlostStatus";
            try
            {
                DataSet lostcarddetail = new DataSet();
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dblostcarddetail = sqlConn.GetStoredProcCommand(strSql);
                dblostcarddetail.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dblostcarddetail, "@PassDetailID", SqlDbType.BigInt, passdetailid);
                lostcarddetail = sqlConn.ExecuteDataSet(dblostcarddetail);
                return lostcarddetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Get ODI Card Issued
        /// </summary>
        /// <param name="strLocationID">Location ID</param>  
        /// <param name="strAssociateID">Associate id</param>  
        /// <returns>data set</returns> 
        #region GetODICardsIssued
        public DataSet GetODICardsIssued(string strLocationID, string strAssociateID,DateTime curdate)
        {
            
            var CurrentDate = curdate.ToString("yyyy-MM-dd HH:mm:ss.fff");

           
            DataSet dsresult = new DataSet();
            string sqlSubmitImageRequest = "IVS_GetODICardsIssued";
            try
            {
                if (string.IsNullOrEmpty(strAssociateID))
                {
                    strAssociateID = "NULL";
                }

                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbsubmitRequest = sqlConn.GetStoredProcCommand(sqlSubmitImageRequest);
                dbsubmitRequest.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbsubmitRequest, "Location", SqlDbType.Int, Convert.ToInt32(strLocationID));
                sqlConn.AddInParameter(dbsubmitRequest, "Assosciateid", SqlDbType.VarChar, strAssociateID);
                sqlConn.AddInParameter(dbsubmitRequest, "Curdate", SqlDbType.VarChar, CurrentDate);
                dsresult = sqlConn.ExecuteDataSet(dbsubmitRequest);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Save Reprint Details
        /// </summary>
        /// <param name="passDetailsID">pass Details ID</param>  
        /// <param name="securityID">security id</param>  
        /// <param name="strComments">comments value</param>  
        /// <returns>data set</returns> 
        #region SaveReprintDetails
        public bool SaveReprintDetails(int passDetailsID, string securityID, string strComments)
        {
            bool result = false;
            SqlParameter paramRequestId;
            string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand("SaveReprintDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            ////con.Open();
            con.OpenWithMSI();
            try
            {
                paramRequestId = new SqlParameter("@PassDetailsID", SqlDbType.Int);
                paramRequestId.Value = passDetailsID;
                cmd.Parameters.Add(paramRequestId);
                cmd.Parameters.Add("@SecurityID", SqlDbType.VarChar, 50).Value = securityID;
                cmd.Parameters.Add("@ReprintReason", SqlDbType.VarChar, 50).Value = strComments;
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                con.Close();
            }

            return result;
        }
        #endregion

        #region SaveLaptopInformation
        /// <summary>
        /// Method to Save Laptop Information
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>
        /// <param name="strCheckStatus">Check Status</param>
        /// <param name="strUserID">User ID</param>
        /// <param name="strLocationID">Location ID</param>  
        /// <param name="strAssetNumber">Asset Number</param>
        /// <param name="strSerialNumber">Serial Number</param>
        /// <returns>Save Flag integer </returns>
        public int SaveLaptopInformation(string strAssociateID, string strCheckStatus, string strUserID, string strLocationID, string strAssetNumber, string strSerialNumber)
        {
            string strSql = "IVS_SaveLaptopDetails";
            int saveFlag;
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "CheckStatus", DbType.String, strCheckStatus);

                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "LaptopPassReturnedDate", DbType.String, LaptopPassReturnedDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "VerifiedUser", DbType.String, strUserID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationID", DbType.String, strLocationID);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "SaveFlag", SqlDbType.Int, 4);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssetNumber", DbType.String, strAssetNumber);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "SerialNumber", DbType.String, strSerialNumber);
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);
                saveFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "SaveFlag").ToString());
                return saveFlag;
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
        #endregion

        #region SaveLaptopInformation
        /// <summary>
        /// Method to Save Laptop Information
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>
        /// <param name="strUserID">user ID</param>
        /// <param name="strLocationID">Location ID</param>
        /// <param name="popupdetails">pop up details</param>
        /// <returns>integer value string</returns>
        public DataSet Incompleteverificationupdation(string strAssociateID, string strUserID, string strLocationID, string popupdetails)
        {
            string strSql = "LVS_Incompleteverificationupdation";
            DataSet dscardDetails = new DataSet();
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);

                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "CheckStatus", DbType.String, strCheckStatus);
                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "LaptopPassReturnedDate", DbType.String, LaptopPassReturnedDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "VerifiedUser", DbType.String, strUserID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationID", DbType.String, strLocationID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "popupdetails", DbType.String, popupdetails);

                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "AssetNumber", DbType.String, strAssetNumber);
                // sqlConn.AddInParameter(dbEmployeeDetailsSaveCmd, "SerialNumber", DbType.String, strSerialNumber);
                // sqlConn.ExecuteNonQuery(dbEmployeeDetailsSaveCmd);
                dscardDetails = sqlConn.ExecuteDataSet(dbemployeeDetailsSaveCmd);

                return dscardDetails;
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
        #endregion

        /// <summary>
        /// Validate Login User
        /// </summary>
        /// <param name="strAssociateId">Associate Id</param>  
        /// <param name="strPassword">Password value</param>   
        /// <returns>string value</returns> 
        #region ValidateLoginUser
        public string ValidateLoginUser(string strAssociateId, string strPassword)
        {
            string strMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "Alive_CheckLoginUser";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);

                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "AssociateID", SqlDbType.VarChar, strAssociateId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "Password", SqlDbType.VarChar, strPassword);
                sqlConn.AddOutParameter(dbemployeeInfoComm, "StatusFlag", SqlDbType.VarChar, 500);
                sqlConn.ExecuteNonQuery(dbemployeeInfoComm);
                strMessage = sqlConn.GetParameterValue(dbemployeeInfoComm, "StatusFlag").ToString();
                return strMessage;
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
        #endregion

        #region ValidatePassDetails
        /// <summary>
        /// Method to Validate Pass Details
        /// </summary>
        /// <param name="strAssociateID">Associate ID</param>
        /// <param name="passIssuedDate">pass Issued Date</param>
        /// <param name="passReturnedDate">pass Returned Date</param>   
        /// <param name="locationCity">location City</param>   
        /// <param name="strLocationID">Location ID</param>   
        /// <returns>integer value string</returns>
        public int ValidatePassDetails(string strAssociateID, string passIssuedDate, string passReturnedDate, string locationCity, string strLocationID)
        {
            string strSql = "IVS_ValidatePassDetails";
            int checkFlag;

            // string strErrorMessage;
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassIssuedDate", DbType.String, passIssuedDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "PassReturnedDate", DbType.String, passReturnedDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationCity", DbType.String, locationCity);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationID", DbType.String, strLocationID);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "CheckFlag", DbType.String, 500);
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);
                checkFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "CheckFlag").ToString());
                return checkFlag;

                // strErrorMessage = Convert.ToString(sqlConn.GetParameterValue(dbEmployeeDetailsSaveCmd, "ErrorMessage").ToString());
                // return strErrorMessage;                    
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
        #endregion

        #region ValidateLaptopUserDetails
        /// <summary>
        /// Method to validate Laptop users details.
        /// </summary>
        /// <param name="strAssociateID">associate id</param> 
        /// <param name="laptopPassIssuedDate">laptop Pass Issued Date</param>       .
        /// <param name="laptopPassReturnedDate">laptop Pass Returned Date</param> 
        /// <param name="locationCity">location City</param> 
        /// <param name="strLocationID">Location ID</param> 
        /// <returns>integer value string</returns>
        public int ValidateLaptopUserDetails(string strAssociateID, string laptopPassIssuedDate, string laptopPassReturnedDate, string locationCity, string strLocationID)
        {
            string strSql = "IVS_ValidateLaptopUserDetails";
            int checkFlag;
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());
                DbCommand dbemployeeDetailsSaveCmd = sqlConn.GetStoredProcCommand(strSql);
                dbemployeeDetailsSaveCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "AssociateID", DbType.String, strAssociateID);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LaptopPassIssuedDate", DbType.String, laptopPassIssuedDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LaptopPassReturnedDate", DbType.String, laptopPassReturnedDate);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationCity", DbType.String, locationCity);
                sqlConn.AddInParameter(dbemployeeDetailsSaveCmd, "LocationID", DbType.String, strLocationID);
                sqlConn.AddOutParameter(dbemployeeDetailsSaveCmd, "CheckFlag", DbType.String, 500);
                sqlConn.ExecuteNonQuery(dbemployeeDetailsSaveCmd);
                checkFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbemployeeDetailsSaveCmd, "CheckFlag").ToString());
                return checkFlag;
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
        #endregion

        #region ValidateLaptopDetails
        /// <summary>
        /// Method to Validate Laptop details
        /// </summary>
        /// <param name="strParameter">string parameter</param>  
        /// <param name="strSelectFlag">select parameter</param>                   
        /// <returns>integer value string</returns>
        public string ValidateLaptopDetails(string strParameter, string strSelectFlag)
        {
            DataSet dsemployeeInfo = new DataSet();
            string errorMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_ValidateLaptopDetails";
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());

                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "Parameter", SqlDbType.VarChar, strParameter);
                sqlConn.AddInParameter(dbemployeeInfoComm, "Parmatertype", SqlDbType.VarChar, strSelectFlag);
                sqlConn.AddOutParameter(dbemployeeInfoComm, "StatusFlag", SqlDbType.VarChar, 500);
                sqlConn.ExecuteNonQuery(dbemployeeInfoComm);
                string strErrorMessage = sqlConn.GetParameterValue(dbemployeeInfoComm, "StatusFlag").ToString();
                return strErrorMessage;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dsemployeeInfo = null;
            }
        }
        #endregion

        #region ValidateAssociateDetails
        /// <summary>
        /// Method to Validate Associate Details
        /// </summary>
        /// <param name="associateId">Associate id</param>  
        /// <returns>integer value string</returns>
        public int ValidateAssociateDetails(string associateId)
        {
            DataSet dsemployeeInfo = new DataSet();
            int intFlag = 0;
            string sqlGetEmployeeInfoproc = "IVS_ValidateAssociateDetails";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);

                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "Associate_ID", SqlDbType.VarChar, associateId);
                sqlConn.AddOutParameter(dbemployeeInfoComm, "ReturnFlag", SqlDbType.Int, intFlag);
                sqlConn.ExecuteNonQuery(dbemployeeInfoComm);
                intFlag = Convert.ToInt32(sqlConn.GetParameterValue(dbemployeeInfoComm, "ReturnFlag").ToString());
                return intFlag;
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
                dsemployeeInfo = null;
            }
        }
        #endregion

        #region TemporaryCardReport
        /// <summary>
        /// Method to Validate Associate Details
        /// </summary>
        /// <param name="associateId">associate id</param>  
        /// <param name="strCity">string City</param>  
        /// <param name="strStartDate">string Start Date</param>  
        /// <param name="strEndDate">string End Date</param>  
        /// <param name="countryId">country Id</param>  
        /// <param name="strFacility">string Facility</param>  
        /// <returns>integer value string</returns>
        public DataTable IDCardIssuedReport(string associateId, string strCity, string strStartDate, string strEndDate, string countryId, string strFacility)
        {
            DataSet dsemployeeInfo = new DataSet();
            string sqlGetEmployeeInfoproc = string.Empty;
            try
            {
                sqlGetEmployeeInfoproc = "IVS_GetTemporaryCardReport";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "AssociateID", SqlDbType.VarChar, associateId);
                if (string.IsNullOrEmpty(strCity))
                {
                    strCity = null;
                }

                if (string.IsNullOrEmpty(countryId))
                {
                    countryId = null;
                }

                sqlConn.AddInParameter(dbemployeeInfoComm, "City", SqlDbType.VarChar, strCity);
                sqlConn.AddInParameter(dbemployeeInfoComm, "CountryId", SqlDbType.VarChar, countryId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "StartDate", SqlDbType.VarChar, strStartDate);
                sqlConn.AddInParameter(dbemployeeInfoComm, "EndDate", SqlDbType.VarChar, strEndDate);
                if (string.IsNullOrEmpty(strFacility))
                {
                    strFacility = null;
                }

                sqlConn.AddInParameter(dbemployeeInfoComm, "Facility", SqlDbType.VarChar, strFacility);
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                if (dsemployeeInfo != null)
                {
                    return dsemployeeInfo.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsemployeeInfo = null;
            }
        }
        #endregion

        #region GetEmployeeList
        /// <summary>
        /// Modified by 173710
        /// Method to retrieve Employee list on the basis of search criteria
        /// </summary>
        /// <param name="strEmployeeID">employee id</param>  
        /// <param name="strCity">city name</param>  
        /// <param name="strStartDate">start date</param>  
        /// <param name="strEndDate">end date</param>            
        /// <returns>integer value string</returns>
        public System.Data.DataTable GetEmployeeList(string strEmployeeID, string strCity, string strStartDate, string strEndDate)
        {
            DataTable dtcardDetails = new DataTable();
            DataSet dscardDetails = new DataSet();
            System.Text.StringBuilder strCriteriaBuilder = new System.Text.StringBuilder();
            string sqlStr = "IVS_GetEmployeeList";
            strCriteriaBuilder.Append(string.Empty);
            try
            {
                if (strEmployeeID.Length > 0)
                {
                    strCriteriaBuilder.Append(" and tblPassDetails.EmployeeID='" + strEmployeeID + "'");
                }

                if (strCity != "0")
                {
                    if (strCity != "-123")
                    {
                        // IVS17082010AUTOCLOSE start added "or" condition
                        strCriteriaBuilder.Append(" and (( tblPassDetails.PassIssuedCity= '" + strCity + "' ) or (tblPassDetails.PassReturnedCity= '" + strCity + "' )) ");

                        // IVS17082010AUTOCLOSE end
                    }
                }

                if (strStartDate.Length > 0)
                {
                    if (strEndDate.Trim().Length == 0)
                    {
                        strEndDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }

                    // IVS17082010AUTOCLOSE start added "or" condition
                    strCriteriaBuilder.Append(" and((convert(varchar(12),tblPassDetails.PassIssuedDate,101) >=" +
                  "'" + strStartDate + "'" + " and convert(varchar(12),tblPassDetails.PassIssuedDate,101) <=" + "'" +
                  strEndDate + "') or (convert(varchar(12),tblPassDetails.PassReturnedDate,101) >=" +
                  "'" + strStartDate + "'" + " and convert(varchar(12),tblPassDetails.PassReturnedDate,101) <=" + "'" +
                  strEndDate + "'))");

                    // IVS17082010AUTOCLOSE end
                }

                if (strStartDate.Trim().Length == 0 && strEndDate.Trim().Length == 0)
                {
                    strCriteriaBuilder.Append(" order by tblPassDetails.PassDetailID DESC");
                }
                else
                {
                    strCriteriaBuilder.Append(" order by tblPassDetails.PassDetailID Desc");
                }

                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbcardDetailsComm = sqlConn.GetStoredProcCommand(sqlStr);
                dbcardDetailsComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbcardDetailsComm, "Criteria", DbType.String, strCriteriaBuilder.ToString());
                dscardDetails = sqlConn.ExecuteDataSet(dbcardDetailsComm);
                if (dscardDetails.Tables.Count > 0)
                {
                    dtcardDetails = dscardDetails.Tables[0];
                }

                return dtcardDetails;
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
                dscardDetails = null;
                dtcardDetails = null;
                strCriteriaBuilder = null;
            }
        }
        #endregion

        #region GetLaptopUsersList

        /// <summary>
        /// Modified by 173710
        /// Method to retrieve Laptop list on the basis of search criteria
        /// </summary>
        /// <param name="strEmployeeID">Employee ID</param>  
        /// <param name="strCity">city name</param>  
        /// <param name="strStartDate">start date</param>  
        /// <param name="strEndDate">end date</param>  
        /// <returns>Laptop Details Data table</returns>
        public System.Data.DataTable GetLaptopUsersList(string strEmployeeID, string strCity, string strStartDate, string strEndDate)
        {
            DataTable dtlaptopDetails = new DataTable();
            DataSet dslaptopDetails = new DataSet();
            System.Text.StringBuilder strCriteriaBuilder = new System.Text.StringBuilder();
            string sqlStr = "LaptopReport";
            strCriteriaBuilder.Append(string.Empty);
            try
            {
                if (strEmployeeID.Length > 0)
                {
                    strCriteriaBuilder.Append(" and tblLaptopHistory.AssociateID='" + strEmployeeID + "'");
                }

                if (strCity != "0")
                {
                    if (strCity != "-123")
                    {
                        strCriteriaBuilder.Append(" and tblLaptopHistory.VerifiedLocation='" + strCity + "'");
                    }
                }

                if (strStartDate.Length > 0)
                {
                    if (strEndDate.Trim().Length == 0)
                    {
                        strEndDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }

                    strCriteriaBuilder.Append(" and (convert(varchar(12),tblLaptopHistory.VerifiedDate,101) >=" +
                  "'" + strStartDate + "'" + " and convert(varchar(12),tblLaptopHistory.VerifiedDate,101) <=" + "'" +
                  strEndDate + "')");
                }

                if (strStartDate.Trim().Length == 0 && strEndDate.Trim().Length == 0)
                {
                    strCriteriaBuilder.Append(" order by tblLaptopHistory.VerifiedDate DESC");
                }
                else
                {
                    strCriteriaBuilder.Append(" order by tblLaptopHistory.VerifiedDate ASC");
                }

                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());
                DbCommand dblaptopDetailsComm = sqlConn.GetStoredProcCommand(sqlStr);
                dblaptopDetailsComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dblaptopDetailsComm, "Criteria", DbType.String, strCriteriaBuilder.ToString());
                dslaptopDetails = sqlConn.ExecuteDataSet(dblaptopDetailsComm);

                if (dslaptopDetails.Tables.Count > 0)
                {
                    dtlaptopDetails = dslaptopDetails.Tables[0];
                }

                return dtlaptopDetails;
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
                dslaptopDetails = null;
                dtlaptopDetails = null;
                strCriteriaBuilder = null;
            }
        }
        #endregion

        #region GetLVSEmployeeDetails
        /// <summary>
        /// Method to retrieve Employee Details of LVS
        /// </summary>
        /// <param name="associateId">string associate id</param>
        /// <returns>Employee Data row</returns>
        public System.Data.DataRow GetLVSEmployeeDetails(string associateId)
        {
            DataSet dsemployeeInfo = new DataSet();
            DataRow dremployeeInfo = null;
            string errorMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_GetEmployeeDetails";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, associateId);
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);

                if (dsemployeeInfo.Tables.Count > 0)
                {
                    if (dsemployeeInfo.Tables[0].Rows.Count > 0)
                    {
                        dremployeeInfo = dsemployeeInfo.Tables[0].Rows[0];
                    }
                }

                sqlConn = null;
                return dremployeeInfo;
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
                dsemployeeInfo = null;
            }
        }
        #endregion

        #region GetUserDetails
        /// <summary>
        /// Method to get User Name of a User Id
        /// </summary>
        /// <param name="strUserId">Login User Id</param>
        /// <returns>Returns User Name as string</returns>
        public Hashtable GetUserDetails(string strUserId)
        {
            string sqlGetLoginUserName = string.Empty;
            string strUserName = string.Empty;
            string strRoleID = string.Empty;
            Hashtable htrecord = new Hashtable();
            try
            {
                sqlGetLoginUserName = "ALIVE_GetLoginUserName";
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbloginUserNamecmd = sqlConn.GetStoredProcCommand(sqlGetLoginUserName);
                sqlConn.AddInParameter(dbloginUserNamecmd, "UserId", DbType.String, strUserId);
                sqlConn.AddOutParameter(dbloginUserNamecmd, "UserName", DbType.String, 150);
                sqlConn.AddOutParameter(dbloginUserNamecmd, "RoleID", DbType.Int16, 4);
                dbloginUserNamecmd.CommandType = CommandType.StoredProcedure;
                sqlConn.ExecuteNonQuery(dbloginUserNamecmd);
                htrecord.Add("UserName", sqlConn.GetParameterValue(dbloginUserNamecmd, "UserName").ToString());
                htrecord.Add("RoleID", sqlConn.GetParameterValue(dbloginUserNamecmd, "RoleID").ToString());
                return htrecord;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            catch (System.Configuration.ConfigurationException ex)
            {
                throw ex;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  GetAssociateID

        /// <summary>
        /// Method to retrieve AssociateID on the basis of Laptop serial number
        /// </summary>
        /// <param name="strSerialNo">serial number</param>     
        /// <returns>integer value string</returns>
        public string GetAssociateID(string strSerialNo)
        {
            string sqlGetEmployeeInfoproc = "IVS_GetAssociateIDBySerialNo";
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());

                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "SerialNo", SqlDbType.VarChar, strSerialNo);
                sqlConn.AddOutParameter(dbemployeeInfoComm, "AssociateID", SqlDbType.VarChar, 500);
                sqlConn.ExecuteNonQuery(dbemployeeInfoComm);
                string strAssociateID = sqlConn.GetParameterValue(dbemployeeInfoComm, "AssociateID").ToString();
                return strAssociateID;
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
        /// Get Access Card number
        /// </summary>
        /// <param name="associateID">associate ID</param>  
        /// <param name="strLocation">location value</param>  
        /// <param name="passDetailsID">pass Details ID</param>  
        /// <returns>data set</returns> 
        public DataSet GetAccessCardNo(string associateID, string strLocation, string passDetailsID)
        {
            {
                DataSet dsresult = new DataSet();
                DataTable dtaccess = new DataTable();
                string sqlAccessCard = "IVS_Get_Checkedout_AccessCardNo";
                try
                {
                    SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dbaccessCmd = sqlConn.GetStoredProcCommand(sqlAccessCard);
                    dbaccessCmd.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dbaccessCmd, "AssociateID", SqlDbType.VarChar, Convert.ToInt32(associateID));
                    sqlConn.AddInParameter(dbaccessCmd, "Location", SqlDbType.Int, Convert.ToInt32(strLocation));
                    sqlConn.AddInParameter(dbaccessCmd, "PassID", SqlDbType.Int, Convert.ToInt32(passDetailsID));
                    dsresult = sqlConn.ExecuteDataSet(dbaccessCmd);

                    // if (dsResult.Tables.Count > 0)
                    // {

                    //// dtAccess = dsResult.Tables[0];
                    // dsResult=
                    // }
                    return dsresult;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (System.Configuration.ConfigurationException ex)
                {
                    throw ex;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get location from GSMS
        /// </summary>
        /// <param name="locationid">location id</param>  
        /// <returns>data set</returns> 
        public DataSet GetlocationfromGSMS(string locationid)
        {
            {
                DataSet dsresult = new DataSet();
                DataTable dtaccess = new DataTable();
                string sqlgetlocationgsms = "GetLocationIDfromGSMS";
                try
                {
                    string conn = ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString();
                    SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                    DbCommand dblocationgsms = sqlConn.GetStoredProcCommand(sqlgetlocationgsms);
                    dblocationgsms.CommandType = CommandType.StoredProcedure;
                    sqlConn.AddInParameter(dblocationgsms, "LocationID", SqlDbType.Int, Convert.ToInt32(locationid));
                    dsresult = sqlConn.ExecuteDataSet(dblocationgsms);
                    return dsresult;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (System.Configuration.ConfigurationException ex)
                {
                    throw ex;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Select card
        /// </summary>
        /// <returns>data set</returns> 
        public DataSet Select_Card_DDL()
        {
            DataTable dtselect_card = new DataTable();
            DataSet dsselect_card = new DataSet();
            string sqlAccessCard = "Select_CARD_DDL_SP";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbselect_card = sqlConn.GetStoredProcCommand(sqlAccessCard);
                dbselect_card.CommandType = CommandType.StoredProcedure;
                dsselect_card = sqlConn.ExecuteDataSet(dbselect_card);

                // if (dsselect_card.Tables.Count > 0)
                // {

                // dtselect_card = dsselect_card.Tables[0];
                return dsselect_card;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            catch (System.Configuration.ConfigurationException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check access card history Data Layer
        /// </summary>
        /// <param name="associateid">associate id</param>  
        /// <returns>data set</returns> 
        public DataSet CheckaccesscardhistoryDL(string associateid)
        {
            //// string sqlaccesshistoryproc = "IVS_GetAccessCardHistoryCount";
            //// DataTable checkaccesshistory = new DataTable();
            //// try
            //// {
            ////    string Conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
            ////    SqlConnection sqlConn = new SqlConnection(Conn);
            ////    SqlCommand accesshistoryinfo = new SqlCommand(sqlaccesshistoryproc, sqlConn);
            ////    accesshistoryinfo.CommandType = CommandType.StoredProcedure;
            ////    accesshistoryinfo.Parameters.Add("@AssociateID", SqlDbType.VarChar, 15).Value = associateid;
            ////    SqlDataAdapter accesshistory = new SqlDataAdapter(accesshistoryinfo);
            ////    accesshistory.Fill(checkaccesshistory);
            ////    return checkaccesshistory;
            //// }
            DataSet dsaccesscardhistory = new DataSet();
            string errorMessage = string.Empty;
            string accesscardhistorysp = "IVS_GetAccessCardHistoryCount";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand cmdaccesscardhistory = sqlConn.GetStoredProcCommand(accesscardhistorysp);
                cmdaccesscardhistory.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(cmdaccesscardhistory, "AssociateID", SqlDbType.VarChar, associateid);
                dsaccesscardhistory = sqlConn.ExecuteDataSet(cmdaccesscardhistory);
                sqlConn = null;
                return dsaccesscardhistory;
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
        /// Safety Permit Security Search
        /// </summary>
        /// <param name="associateid">associate id</param>  
        /// <param name="passdetailid">pass detail id</param>  
        /// <param name="activated_status">activated status</param>  
        /// <param name="deactivated_status">deactivated status</param> 
        public void Accesscardservicestatus(string associateid, string passdetailid, bool? activated_status, bool? deactivated_status)
        {
            string sqlaccessservicestatusproc = "IVS_AccessCardServiceStatus";
            int accessstatus;
            if (activated_status == true || deactivated_status == true)
            {
                accessstatus = 1;
            }
            else
            {
                accessstatus = 0;
            }

            try
            {
                string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
                SqlConnection sqlConn = new SqlConnection(conn);
                sqlConn.OpenWithMSI();
                SqlCommand accesscardservicestatus = new SqlCommand(sqlaccessservicestatusproc, sqlConn);
                accesscardservicestatus.CommandType = CommandType.StoredProcedure;
                accesscardservicestatus.Parameters.Add("@AssociateID", SqlDbType.VarChar, 11).Value = associateid;
                accesscardservicestatus.Parameters.Add("@PassNumber", SqlDbType.VarChar, 50).Value = passdetailid;
                if (activated_status == null)
                {
                    accesscardservicestatus.Parameters.Add("@Activated_Status", SqlDbType.Bit).Value = DBNull.Value;
                }
                else
                {
                    accesscardservicestatus.Parameters.Add("@Activated_Status", SqlDbType.Bit).Value = accessstatus;
                }

                if (deactivated_status == null)
                {
                    accesscardservicestatus.Parameters.Add("@Deactivated_Status", SqlDbType.Bit).Value = DBNull.Value;
                }
                else
                {
                    accesscardservicestatus.Parameters.Add("@Deactivated_Status", SqlDbType.Bit).Value = accessstatus;
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
        /// Get Asset Number
        /// </summary>
        /// <param name="assetNumber">asset number</param>    
        /// <returns>string value</returns> 
        public string GetAssetNumber(string assetNumber)
        {
            string sqlAssetNumberInfoproc = "IVS_GetAssociateIDAssetNumber";
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlAssetNumberInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "AssetNumber", SqlDbType.VarChar, assetNumber);
                sqlConn.AddOutParameter(dbemployeeInfoComm, "AssociateID", SqlDbType.VarChar, 500);
                sqlConn.ExecuteNonQuery(dbemployeeInfoComm);
                string strAssociateID = sqlConn.GetParameterValue(dbemployeeInfoComm, "AssociateID").ToString();
                return strAssociateID;
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
        #endregion

        /// <summary>
        /// Get ID Card Locations
        /// </summary> 
        /// <returns>data set</returns> 
        #region GetIDCardLocations
        public DataSet GetIDCardLocations()
        {
            DataSet dslocations = new DataSet();
            string errorMessage = string.Empty;
            string sqlGetApplicantInfoproc = "IVS_GetIDCardLocations";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;

                // sqlConn.AddInParameter(dbApplicantInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, AssociateID);
                dslocations = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                sqlConn = null;
                return dslocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dslocations = null;
            }
        }
        #endregion

        #region MoveImagesToFolder

        /// <summary>
        /// Move Images To Folder
        /// </summary>
        /// <param name="location">location data</param>  
        /// <param name="dt">date time</param>  
        /// <returns>data table</returns> 
        public DataTable MoveImagesToFolder(string location, DateTime dt)
        {
            DataSet dsapplicantImages = new DataSet();
            string sqlGetApplicantInfoproc = "MoveImagesToFolder";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbapplicantInfoComm, "Location", SqlDbType.VarChar, location);
                sqlConn.AddInParameter(dbapplicantInfoComm, "DateCreated", SqlDbType.DateTime, dt);
                dsapplicantImages = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                sqlConn = null;
                return dsapplicantImages.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsapplicantImages = null;
            }
        }

        #endregion

        /// <summary>
        /// Get Applicant no Image Report
        /// </summary>
        /// <param name="location">location name</param>  
        /// <param name="fromDate">from Date</param>  
        /// <param name="todate">to Date</param>  
        /// <returns>data table</returns> 
        #region GetApplicantNoImageReport
        public DataTable GetApplicantNoImageReport(string location, string fromDate, string todate)
        {
            DataSet dsapplicantNoImage = new DataSet();
            string sqlGetApplicantInfoproc = "IVS_IDCardReport";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbapplicantInfoComm, "Location", SqlDbType.VarChar, location);
                sqlConn.AddInParameter(dbapplicantInfoComm, "FromDate", SqlDbType.VarChar, fromDate);
                sqlConn.AddInParameter(dbapplicantInfoComm, "ToDate", SqlDbType.VarChar, todate);
                dsapplicantNoImage = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                sqlConn = null;
                return dsapplicantNoImage.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsapplicantNoImage = null;
            }
        }

        /// <summary>
        /// Get Applicant No Image
        /// </summary>
        /// <param name="applicantID">applicant ID</param>   
        /// <returns>data table</returns> 
        public DataTable GetApplicantNoImage(string applicantID)
        {
            DataSet dsapplicantNoImage = new DataSet();
            string sqlGetApplicantInfoproc = "IVS_IDCardHistory";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbapplicantInfoComm, "AssociateID", SqlDbType.VarChar, applicantID);
                dsapplicantNoImage = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                sqlConn = null;
                return dsapplicantNoImage.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsapplicantNoImage = null;
            }
        }
        #endregion

        /// <summary>
        /// Applicant ID By Associate ID
        /// </summary>
        /// <param name="associateID">associate ID</param>  
        /// <returns>string value</returns> 
        #region ApplicantIDByAssociateID
        public string GetApplicantIDByAssociateID(string associateID)
        {
            DataSet dsapplicant = new DataSet();
            string applicantID = null;
            string errorMessage = string.Empty;
            string sqlGetApplicantInfoproc = "IVS_GetCandidateIdByAssociateID";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbapplicantInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, associateID);
                dsapplicant = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                if (dsapplicant.Tables.Count > 0)
                {
                    if (dsapplicant.Tables[0].Rows.Count > 0)
                    {
                        applicantID = dsapplicant.Tables[0].Rows[0]["CandidateID"].ToString().Trim();
                    }
                }

                sqlConn = null;
                return applicantID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsapplicant = null;
                applicantID = null;
            }
        }
        #endregion

        #region UpdateAssociateID

        /// <summary>
        /// Update Associate ID
        /// </summary>
        /// <param name="applicantID">applicant ID</param>  
        /// <param name="associateID">associate ID</param>  
        /// <param name="associateName">associate Name</param>
        /// <param name="bloodGroup">blood Group</param>  
        /// <param name="emergencyContact">emergency Contact</param>  
        /// <returns>data set</returns> 
        public bool UpdateAssociateID(string applicantID, string associateID, string associateName, string bloodGroup, string emergencyContact)
        {
            DataSet dsapplicantInfo = new DataSet();
            bool state = false;

            // DataRow drApplicantInfo = null;
            string errorMessage = string.Empty;
            string sqlGetApplicantInfoproc = "UpdateAssociateID";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbapplicantInfoComm = sqlConn.GetStoredProcCommand(sqlGetApplicantInfoproc);
                dbapplicantInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbapplicantInfoComm, "ApplicantID", SqlDbType.VarChar, applicantID);
                sqlConn.AddInParameter(dbapplicantInfoComm, "AssociateID", SqlDbType.VarChar, associateID);
                sqlConn.AddInParameter(dbapplicantInfoComm, "AssociateName", SqlDbType.VarChar, associateName);
                sqlConn.AddInParameter(dbapplicantInfoComm, "BloodGroup", SqlDbType.VarChar, bloodGroup);
                sqlConn.AddInParameter(dbapplicantInfoComm, "EmergencyContact", SqlDbType.VarChar, emergencyContact);
                dsapplicantInfo = sqlConn.ExecuteDataSet(dbapplicantInfoComm);
                if (dsapplicantInfo.Tables.Count > 0)
                {
                    if (dsapplicantInfo.Tables[0].Rows.Count > 0)
                    {
                        state = Convert.ToBoolean(dsapplicantInfo.Tables[0].Rows[0][0].ToString());
                    }
                }

                sqlConn = null;
                return state;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsapplicantInfo = null;
            }
        }

        #endregion

        /// <summary>
        /// Get Applicant Image Details
        /// </summary>
        /// <param name="applicantID">applicant ID</param>  
        /// <returns>data set</returns> 
        #region GetApplicantImageDetails
        public string GetApplicantImageDetails(string applicantID)
        {
            DataSet dsemployeeInfo = new DataSet();
            DataRow dremployeeInfo = null;

            // object obj = null;
            string errorMessage = string.Empty;
            string image = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_GetApplicantImageDetails";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "APPLICANT_ID", SqlDbType.VarChar, applicantID);

                // obj = sqlConn.ExecuteScalar(dbEmployeeInfoComm).ToString();
                // if (obj != null)
                //    image = obj.ToString();
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                if (dsemployeeInfo.Tables.Count > 0)
                {
                    if (dsemployeeInfo.Tables[0].Rows.Count > 0)
                    {
                        dremployeeInfo = dsemployeeInfo.Tables[0].Rows[0];
                        image = dremployeeInfo[0].ToString();
                    }
                }

                sqlConn = null;
                return image;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetEmployeeImageDetails

        /// <summary>
        /// Method to retrieve Employee Image details 
        /// </summary>
        /// <param name="associateId">associate id</param>  
        /// <returns>EmployeeInfo Data Row</returns>
        public System.Data.DataRow GetEmployeeImageDetails(string associateId)
        {
            DataSet dsemployeeInfo = new DataSet();
            DataRow dremployeeInfo = null;
            string errorMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_GetAssociateImageDetails";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                string appTemplateID = System.Configuration.ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, associateId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "AppTemplateID", SqlDbType.VarChar, appTemplateID);
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                if (dsemployeeInfo.Tables.Count > 0)
                {
                    if (dsemployeeInfo.Tables[0].Rows.Count > 0)
                    {
                        dremployeeInfo = dsemployeeInfo.Tables[0].Rows[0];
                    }
                }

                sqlConn = null;
                return dremployeeInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsemployeeInfo = null;
            }
        }
        #endregion

        #region GetEmployeeDetails
        /// <summary>
        /// Method to retrieve Employee details 
        /// </summary>
        /// <param name="associateId">associate id</param>  
        /// <returns>Employee Information Data Row</returns>
        public System.Data.DataRow GetEmployeeDetails(string associateId)
        {
            DataSet dsemployeeInfo = new DataSet();
            DataRow dremployeeInfo = null;
            string errorMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_GetAssociateDetailsForSAN"; // "IVS_GetAssociateDetails" 
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                string appTemplateID = System.Configuration.ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, associateId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "AppTemplateID", SqlDbType.VarChar, appTemplateID);
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);

                if (dsemployeeInfo.Tables.Count > 0)
                {
                    if (dsemployeeInfo.Tables[0].Rows.Count > 0)
                    {
                        dremployeeInfo = dsemployeeInfo.Tables[0].Rows[0];
                    }
                }

                sqlConn = null;
                return dremployeeInfo;
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
                dsemployeeInfo = null;
            }
        }

        /// <summary>
        /// Method to retrieve Terminated Employee details 
        /// </summary>
        /// <param name="associateId">associate Id</param>  
        /// <returns>EmployeeInfo Data Row</returns>
        public System.Data.DataRow GetTerminatedEmployeeDetails(string associateId)
        {
            DataSet dsemployeeInfo = new DataSet();
            DataRow dremployeeInfo = null;
            string errorMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_GetTerminatedAssociateDetails";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, associateId);
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);

                if (dsemployeeInfo.Tables.Count > 0)
                {
                    if (dsemployeeInfo.Tables[0].Rows.Count > 0)
                    {
                        dremployeeInfo = dsemployeeInfo.Tables[0].Rows[0];
                    }
                }

                sqlConn = null;
                return dremployeeInfo;
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
                dsemployeeInfo = null;
            }
        }

        /// <summary>
        /// Get Employee Details Associate
        /// </summary>
        /// <param name="associateId">associate Id</param>  
        /// <returns>data set</returns> 
        public System.Data.DataRow GetEmployeeDetails_Associate(string associateId)
        {
            DataSet dsemployeeInfo = new DataSet();
            DataRow dremployeeInfo = null;
            string errorMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_GetAssociateDetails_Associate";
            try
            {

                string conn = DBConnection.IVSConnectionstring();
                SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                string appTemplateID = System.Configuration.ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                string appTemplateId = ConfigurationManager.AppSettings["AppTemplateId"].ToString();
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, associateId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "AppTemplateId", SqlDbType.VarChar, appTemplateId);
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);

                if (dsemployeeInfo.Tables.Count > 0)
                {
                    if (dsemployeeInfo.Tables[0].Rows.Count > 0)
                    {
                        dremployeeInfo = dsemployeeInfo.Tables[0].Rows[0];
                    }
                }

                sqlConn = null;
                return dremployeeInfo;
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
                dsemployeeInfo = null;
            }
        }
        #endregion

        /// <summary>
        /// Get Temporary ID Card Details
        /// </summary>
        /// <param name="passDetailsID">pass Details ID</param>  
        /// <returns>data set</returns> 
        #region GetTempIDCardDetails
        public DataSet GetTempIDCardDetails(string passDetailsID)
        {
            DataSet dsreturnSet = new DataSet();
            string sqlGetEmployeeInfoproc = "IVS_GenerateTempID";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "PassDetailsID", SqlDbType.VarChar, passDetailsID);
                dsreturnSet = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
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
        #endregion

        #region GetLaptopDetails

        /// <summary>
        /// Method to retrieve Laptop details 
        /// </summary>
        /// <param name="associateId">associate Id</param>  
        /// <param name="strassetno">Variable value</param>  
        /// <param name="type">Variable type</param>  
        /// <returns>Laptop Details Data Row</returns>
        public string Checkmembership(string associateId, string strassetno, string type)
        {
            string sqlAssetNumberInfoproc = "CheckMembership";
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());
                DataSet dslaptopInfo = new DataSet();
                DataRow drlaptopInfo = null;
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlAssetNumberInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "@Associate_ID", SqlDbType.VarChar, associateId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "@Lapparam", SqlDbType.VarChar, strassetno);
                sqlConn.AddInParameter(dbemployeeInfoComm, "@Lapparamtype", SqlDbType.VarChar, type);

                // sqlConn.AddInParameter(dbEmployeeInfoComm, "@Lapparamtype", SqlDbType.VarChar, type);
                sqlConn.ExecuteNonQuery(dbemployeeInfoComm);
                string member = string.Empty;
                dslaptopInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                drlaptopInfo = dslaptopInfo.Tables[0].Rows[0];
                member = drlaptopInfo.ItemArray[0].ToString();
                return member;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Laptop Details
        /// </summary>
        /// <param name="associateId">associate Id</param>  
        /// <param name="strassetno">asset no</param>   
        /// <returns>data row</returns> 
        public System.Data.DataRow GetLaptopDetails(string associateId, string strassetno)
        {
            DataSet dslaptopInfo = new DataSet();
            DataRow drlaptopInfo = null;
            string errorMessage = string.Empty;
            string sqlGetEmployeeInfoproc = "IVS_GetLaptopDetails";
            try
            {
                SqlDatabase sqlConn = new SqlDatabase(DBConnection.LVSConnectionstring());
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "ASSOCIATE_ID", SqlDbType.VarChar, associateId);
                sqlConn.AddInParameter(dbemployeeInfoComm, "ASSETNO", SqlDbType.VarChar, strassetno);
                dslaptopInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                if (dslaptopInfo.Tables.Count > 0)
                {
                    if (dslaptopInfo.Tables[0].Rows.Count > 0)
                    {
                        drlaptopInfo = dslaptopInfo.Tables[0].Rows[0];
                    }
                }

                sqlConn = null;
                return drlaptopInfo;
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
                dslaptopInfo = null;
            }
        }
        #endregion

        /// <summary>
        /// for temp access card
        /// </summary>
        /// <param name="strLocationID">Location Id</param>
        /// <returns>Data Set</returns>
        #region GetLostMailerPOCID
        public DataSet GetLostMailerPOCID(string strLocationID)
        {
            DataSet dsresult = new DataSet();
            string sqlSubmitImageRequest = "IVS_Lost_ACMailer";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbsubmitRequest = sqlConn.GetStoredProcCommand(sqlSubmitImageRequest);
                dbsubmitRequest.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbsubmitRequest, "FacilityID", SqlDbType.Int, strLocationID);
                dsresult = sqlConn.ExecuteDataSet(dbsubmitRequest);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Get Lost Mailer point of contact id
        /// </summary>
        /// <param name="strLocationID">Location ID</param>  
        /// <returns>data set</returns> 
        #region GetLostMailerACCPOCID
        public DataSet GetLostMailerACCPOCID(string strLocationID)
        {
            DataSet dsresult = new DataSet();
            string sqlSubmitImageRequest = "IVS_Lost_ACMailerPOC";
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
                DbCommand dbsubmitRequest = sqlConn.GetStoredProcCommand(sqlSubmitImageRequest);
                dbsubmitRequest.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbsubmitRequest, "LocationID", SqlDbType.Int, strLocationID);
                dsresult = sqlConn.ExecuteDataSet(dbsubmitRequest);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsresult.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Enable access card
        /// </summary> 
        /// <param name="passdetailsid">pass details id</param>  
        public void Enableaccesscard(int passdetailsid)
        {
            string conn = ConfigurationManager.ConnectionStrings["IVSConnectionString"].ToString();
            SqlConnection con = new SqlConnection(conn);
            ////con.Open();
            con.OpenWithMSI();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("IVS_Enablecarddatetime", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@passdetailparentid", SqlDbType.Int).Value = passdetailsid;
                cmd.ExecuteNonQuery();
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
        /// Get Associate Image Approve Status
        /// </summary>
        /// <param name="requestID">request ID</param>  
        /// <returns>data set</returns> 
        public string GetAssociateImageApproveStatus(string requestID)
        {
            string sqlGetEmployeeInfoproc = "IVS_GetAssociateImageApproveStatus";
            string strStatus = null;
            DataSet dsemployeeInfo = new DataSet();
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbemployeeInfoComm = sqlConn.GetStoredProcCommand(sqlGetEmployeeInfoproc);
                dbemployeeInfoComm.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbemployeeInfoComm, "RequestId", SqlDbType.VarChar, requestID);
                dsemployeeInfo = sqlConn.ExecuteDataSet(dbemployeeInfoComm);
                if (dsemployeeInfo.Tables.Count > 0)
                {
                    if (dsemployeeInfo.Tables[0].Rows.Count > 0)
                    {
                        strStatus = dsemployeeInfo.Tables[0].Rows[0]["Status"].ToString();
                    }
                }

                sqlConn = null;
                return strStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsemployeeInfo = null;
            }
        }

        
    }
}
