
// <summary>
// This file contains UserDetailsDL class.
// </summary>
//-----------------------------------------------------------------------
namespace VMSDataLayer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
    using EnterpriseLibConn;

    /// <summary>
    /// User details data layer
    /// </summary>
    #region UserDetailsDL
    public class UserDetailsDL
    {
        #region GetUserType
        /// <summary>
        /// Method to get User Type of a User Id
        /// </summary>
        /// <param name="strUserId">Login User Id</param>
        /// <returns>Return User Role id </returns>
        public int GetUserType(string strUserId)
        {
            string sqlGetUserType = "IVS_GetUserType";
            int iroleId = 0;

            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbuserTypecmd = sqlConn.GetStoredProcCommand(sqlGetUserType);
                dbuserTypecmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbuserTypecmd, "UserID", DbType.String, strUserId);
                sqlConn.AddOutParameter(dbuserTypecmd, "RoleId", SqlDbType.Int, 4);
                sqlConn.ExecuteNonQuery(dbuserTypecmd);
                iroleId = Convert.ToInt32(sqlConn.GetParameterValue(dbuserTypecmd, "RoleId").ToString());
                return iroleId;
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
    }

    #endregion
}
