
// <summary>
// This file contains LocationDL class.
// </summary>
//-----------------------------------------------------------------------
namespace VMSDataLayer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
    using VMSConstants;
    using AzureSQLHelper;
    using EnterpriseLibConn;

    /// <summary>
    /// Location Data Layer
    /// </summary>
    #region LocationDL
    public class LocationDL
    {
        /// <summary>
        /// associates with out photo
        /// </summary>
        /// <param name="strEmployeeID">Employee ID</param>  
        /// <param name="strCity">City name</param>  
        /// <param name="strFacility">Facility name</param>  
        /// <param name="strCountry">Country name</param>  
        /// <returns>data set</returns> 
        public System.Data.DataTable Assoiciateswithoutphoto(
            string strEmployeeID, 
            string strCity, 
            string strFacility, 
            string strCountry)
        {
            string noImagereportsp = "IVS_NoImageReport";
            DataSet dsassociateswithoutphoto = new DataSet();
            DataTable dtassociateswithoutphoto = new DataTable();

            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(noImagereportsp);
                dbfactilityDetailsCmd.CommandTimeout = 30000;
                dbfactilityDetailsCmd.CommandType = CommandType.StoredProcedure;
                if (string.IsNullOrEmpty(strEmployeeID))
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "Associateid", SqlDbType.VarChar, null);
                }
                else
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "Associateid", SqlDbType.VarChar, strEmployeeID);
                }

                if (strCity == VMSConstants.SELECTNoImageReport)
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "City", SqlDbType.VarChar, null);
                }
                else
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "City", SqlDbType.VarChar, strCity);
                }

                if (strFacility == VMSConstants.SELECTNoImageReport)
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "Facility", SqlDbType.VarChar, null);
                }
                else
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "Facility", SqlDbType.VarChar, strFacility);
                }

                if (string.IsNullOrEmpty(strCountry))
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "CountryId", SqlDbType.VarChar, null);
                }
                else
                {
                    sqlConn.AddInParameter(dbfactilityDetailsCmd, "CountryId", SqlDbType.VarChar, strCountry);
                }

                dsassociateswithoutphoto = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);
                if (dsassociateswithoutphoto.Tables.Count > 0)
                {
                    dtassociateswithoutphoto = dsassociateswithoutphoto.Tables[0];
                }

                sqlConn = null;
                return dtassociateswithoutphoto;
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
            finally
            {
                dsassociateswithoutphoto.Dispose();
            }
        }

        // CR---IVS01062010CR09--end
        #region GetCityDetails
        /// <summary>
        /// Method to retrieve city details
        /// </summary>
        /// <returns>City Details Data Table</returns>
        public System.Data.DataTable GetCityDetails()
        {
            string sqlGetCityDetails = "IVS_GetCities";
            DataSet dscityDetails = new DataSet();
            DataTable dtcityDetails = new DataTable();

            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbcityDetailsCmd = sqlConn.GetStoredProcCommand(sqlGetCityDetails);
                dbcityDetailsCmd.CommandType = CommandType.StoredProcedure;
                dscityDetails = sqlConn.ExecuteDataSet(dbcityDetailsCmd);
                if (dscityDetails.Tables.Count > 0)
                {
                    dtcityDetails = dscityDetails.Tables[0];
                }

                sqlConn = null;
                return dtcityDetails;
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
            finally
            {
                dscityDetails.Dispose();
            }
        }
        #endregion

        #region GetFacilityDetails
        /// <summary>
        /// Method to retrieve facility details
        /// </summary>
        /// <param name="cityName">city name</param>  
        /// <returns>Facility Details Data Table</returns>
        public System.Data.DataTable GetFacilityDetails(string cityName)
        {
            string sqlGetFactilityDetails = "IVS_GetFacilities";
            DataSet dsfactilityDetails = new DataSet();
            DataTable dtfactilityDetails = new DataTable();

            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(sqlGetFactilityDetails);
                dbfactilityDetailsCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbfactilityDetailsCmd, "CityName", SqlDbType.VarChar, cityName);
                dsfactilityDetails = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);
                if (dsfactilityDetails.Tables.Count > 0)
                {
                    dtfactilityDetails = dsfactilityDetails.Tables[0];
                }

                sqlConn = null;
                return dtfactilityDetails;
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
            finally
            {
                dsfactilityDetails.Dispose();
            }
        }
        #endregion

        #region GetFacilities
        /// <summary>
        /// Method to retrieve city details
        /// </summary>
        /// <returns>Facility Details Data Table</returns>
        public System.Data.DataTable GetFacilities()
        {
            string sqlGetFactilityDetails = "LVS_GetFacilities";
            DataSet dsfactilityDetails = new DataSet();
            DataTable dtfactilityDetails = new DataTable();

            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(sqlGetFactilityDetails);
                dbfactilityDetailsCmd.CommandType = CommandType.StoredProcedure;
                dsfactilityDetails = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);
                if (dsfactilityDetails.Tables.Count > 0)
                {
                    dtfactilityDetails = dsfactilityDetails.Tables[0];
                }

                sqlConn = null;
                return dtfactilityDetails;
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
            finally
            {
                dsfactilityDetails.Dispose();
            }
        }
        #endregion

        #region GetLocationsForIDCardIssued
        /// <summary>
        /// Method to retrieve Location details where card is issued
        /// </summary>
        /// <param name="strEmployeeID">employee id</param>   
        /// <param name="strIssuedDate">issued date</param>  
        /// <returns>Facility Details Data Table</returns>
        public System.Data.DataTable GetLocationsForIDCardIssued(string strEmployeeID, string strIssuedDate)
        {
            string sqlGetFactilityDetails = "IVS_GetEmployeeLocation";
            DataSet dsfactilityDetails = new DataSet();
            DataTable dtfactilityDetails = new DataTable();

            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(sqlGetFactilityDetails);
                dbfactilityDetailsCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbfactilityDetailsCmd, "EmployeeId", SqlDbType.VarChar, strEmployeeID);
                sqlConn.AddInParameter(
                    dbfactilityDetailsCmd, 
                    "CardIssuedDate", 
                    DbType.DateTime, 
                    Convert.ToDateTime(strIssuedDate));
                dsfactilityDetails = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);
                if (dsfactilityDetails.Tables.Count > 0)
                {
                    dtfactilityDetails = dsfactilityDetails.Tables[0];
                }

                sqlConn = null;
                return dtfactilityDetails;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsfactilityDetails.Dispose();
            }
        }

        #endregion

        #region GetIDCardLocations
        /// <summary>
        /// Method to retrieve Location details where card is issued
        /// </summary>
        /// <returns>Facility Details Data Table</returns>
        public DataSet GetIDCardLocations()
        {
            string sqlGetIDCardLocationDetails = "IVS_GETIDCardLocationDetails";
            DataSet dsfactilityDetails = new DataSet();
            try
            {
                SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
                DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(sqlGetIDCardLocationDetails);
                dbfactilityDetailsCmd.CommandType = CommandType.StoredProcedure;
                dsfactilityDetails = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);
                sqlConn = null;
                return dsfactilityDetails;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsfactilityDetails.Dispose();
            }
        }

        /// <summary>
        /// Get Active Country
        /// </summary>
        /// <returns>data table</returns> 
        public DataTable GetActiveCountryIVS()
        {
            string sqlGetCountry = "IVS_GetCountry";
            DataTable dtcountry = new DataTable();
            SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
            DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(sqlGetCountry);
            try
            {
                DataSet dscountryList = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);

                DataTable dtcountryList = dscountryList.Tables[0];
                return dtcountryList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn = null;
            }
        }

        /// <summary>
        /// To Get Cities
        /// </summary>
        /// <param name="countryId">is integrated</param>
        /// <returns>data table</returns>
        public DataTable GetActiveCityIVS(string countryId)
        {
            string sqlGetCountry = "IVS_GetCity";
            DataTable dtcity = new DataTable();
            SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
            try
            {
                DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(sqlGetCountry);
                dbfactilityDetailsCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbfactilityDetailsCmd, "countryId", SqlDbType.VarChar, countryId);
                DataSet dscityDetails = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);
                if (dscityDetails.Tables.Count > 0)
                {
                    dtcity = dscityDetails.Tables[0];
                }

                return dtcity;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn = null;
            }
        }

        /// <summary>
        /// To Get Facilities
        /// </summary>
        /// <param name="city">is integrated</param>
        /// <returns>data table</returns>
        public DataTable GetActiveFacilityIVS(string city)
        {
            string sqlgetCountry = "IVS_GetActiveFacility";
            DataTable dtfacility = new DataTable();
            SqlDatabase sqlConn = new SqlAzureDatabase(DBConnection.IVSConnectionstring()); ////SqlDatabase sqlConn = new SqlDatabase(DBConnection.IVSConnectionstring());string conn = DBConnection.IVSConnectionstring();SqlDatabase sqlConn = new SqlAzureDatabase(conn);
            try
            {
                DbCommand dbfactilityDetailsCmd = sqlConn.GetStoredProcCommand(sqlgetCountry);
                dbfactilityDetailsCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.AddInParameter(dbfactilityDetailsCmd, "City", SqlDbType.VarChar, city);
                DataSet dscityDetails = sqlConn.ExecuteDataSet(dbfactilityDetailsCmd);
                if (dscityDetails.Tables.Count > 0)
                {
                    dtfacility = dscityDetails.Tables[0];
                }

                return dtfacility;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn = null;
            }
        }

        /// <summary>
        /// To Get Active Country List
        /// </summary>
        /// <returns>data table</returns>
        public DataTable GetActiveCountry()
        {
            string sqlGetCountry = "GetCountry";
            DataTable dtcountry = new DataTable();
            SqlConnection sqlConn = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
            ////sqlConn.Open();
            sqlConn.OpenWithMSI();
            try
            {
                SqlCommand cmdCommand = new SqlCommand(sqlGetCountry, sqlConn);
                cmdCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmdCommand);
                adp.Fill(dtcountry);
                return dtcountry;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
        }

        /// <summary>
        /// To Get Cities
        /// </summary>
        /// <param name="countryId">is integrated</param>
        /// <returns>data table</returns>
        public DataTable GetActiveCity(string countryId)
        {
            string sqlGetCountry = "GetCity";
            DataTable dtcity = new DataTable();
            SqlConnection sqlConn = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
            ////sqlConn.Open();
            sqlConn.OpenWithMSI();
            try
            {
                SqlCommand cmdCity = new SqlCommand(sqlGetCountry, sqlConn);
                cmdCity.CommandType = CommandType.StoredProcedure;
                cmdCity.Parameters.Add("@countryId", SqlDbType.VarChar, 100).Value = countryId;
                SqlDataAdapter adp = new SqlDataAdapter(cmdCity);
                adp.Fill(dtcity);
                return dtcity;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
        }

        /// <summary>
        /// To Get Facilities
        /// </summary>
        /// <param name="city">is integrated</param>
        /// <returns>data table</returns>
        public DataTable GetActiveFacility(string city)
        {
            string sqlGetCountry = "GetActiveFacility";
            DataTable dtfacility = new DataTable();
            SqlConnection sqlConn = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["VMSConnectionString"].ToString());
            ////sqlConn.Open();
            sqlConn.OpenWithMSI();
            try
            {
                SqlCommand cmdCity = new SqlCommand(sqlGetCountry, sqlConn);
                cmdCity.CommandType = CommandType.StoredProcedure;
                cmdCity.Parameters.Add("@City", SqlDbType.VarChar, 100).Value = city.Trim();
                SqlDataAdapter adp = new SqlDataAdapter(cmdCity);
                adp.Fill(dtfacility);
                return dtfacility;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }
        }

        #endregion
    }
    #endregion
}
