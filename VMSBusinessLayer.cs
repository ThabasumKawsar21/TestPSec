

namespace VMSBusinessLayer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Xml;
    using BusinessManager;
    ////using VMSBL.localhost;
    using VMSBL;
    using VMSBL.Adminwebservices;
    ////using VMSBL.WebReference;
    using VMSBusinessEntity;
    ////using VMSConstants.VMSConstants;
    ////using VMSDataLayer.VMSDataLayer;
    using VMSUtility;

    /// <summary>
    /// VMS business layer class
    /// </summary>
    public class VMSBusinessLayer
    {
        /// <summary>
        /// Encryption/Decryption for retrieving images
        /// </summary>
        #region EncryptDecrypt

        ////Begin BugFix for Vulnerability Issue 15Apr2011 Vimal

        /// <summary>
        /// Function to encrypt data
        /// </summary>
        /// <param name="tobeEncrypted">to be encrypted value</param>
        /// <returns>encrypted data</returns>
        public static string Encrypt(string tobeEncrypted)
        {
            string ivsValue = ConfigurationManager.AppSettings["IVSValue"].ToString();
            string codeKey = Convert.ToString(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(ivsValue)));
            string textToBeEncrypted = string.Concat(codeKey, "*", tobeEncrypted, "*");
            byte[] encData_byte = new byte[textToBeEncrypted.Length];
            encData_byte = System.Text.Encoding.UTF32.GetBytes(textToBeEncrypted);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }

        /// <summary>
        /// Function to decrypt
        /// </summary>
        /// <param name="textToBeDecrypted">text to be decrypted</param>
        /// <returns>decrypted value</returns>
        public static string Decrypt(string textToBeDecrypted)
        {
            string ivsValue = ConfigurationManager.AppSettings["IVSValue"].ToString();
            System.Text.UTF32Encoding encoder = new System.Text.UTF32Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(textToBeDecrypted);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new string(decoded_char);
            string[] a = result.Split('*');
            string codeKey = System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(a[0].ToString()));
            if (codeKey.Equals(ivsValue))
            {
                return a[1];
            }
            else
            {
                return string.Empty;
            }
        }
        ////End BugFix for Vulnerability Issue 15Apr2011 Vimal
        #endregion

        /// <summary>
        /// ID Details class
        /// </summary>
        /// <typeparam name="T">T value</typeparam>
        /// <typeparam name="U">U value</typeparam>
        /// <typeparam name="V">V value</typeparam>
        public class IDDetails<T, U, V>
        {
            /// <summary>
            /// Gets or sets associate name
            /// </summary>
            public T AssociateName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets blood group
            /// </summary>
            public U Bloodgroup
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets emergency contact value
            /// </summary>
            public V Emergencycontact
            {
                get;
                set;
            }
        }
        #region "variables"

        #endregion

        #region LocationDetailsBL
        /// <summary>
        /// Location Details Business Layer class
        /// </summary>
        public class LocationDetailsBL
        {
            #region Variables

            /// <summary>
            /// Gets or sets output XML value
            /// </summary>
            public string OutputXML
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets Node name value
            /// </summary>
            public string NodeName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets list of location value
            /// </summary>
            public List<string> Locations
            {
                get;
                set;
            }

            ////AdminWebServices AdminWebServiceInfo = new AdminWebServices();
            #endregion

            /// <summary>
            /// Gets the country
            /// </summary>
            /// <returns>location value</returns>
            public List<string> GetCountries()
            {
                try
                {
                    VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                    DataSet dsresult = locationDL.GetIDCardLocations();
                    return this.Locations;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To Get Active Cities in a selected country
            /// </summary>
            /// <param name="countryId">country value</param>
            /// <returns>city values</returns>
            public DataTable GetActiveCities(string countryId)
            {
                try
                {
                    VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                    DataTable dtcity = locationDL.GetActiveCity(countryId);
                    return dtcity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To Get Active Cities in a selected country
            /// </summary>
            /// <param name="countryId">country value</param>
            /// <returns>city values</returns>
            public DataTable GetActiveCitiesIVS(string countryId)
            {
                try
                {
                    VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                    DataTable dtcity = locationDL.GetActiveCityIVS(countryId);
                    return dtcity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get Active Facilities
            /// </summary>
            /// <param name="city">country value</param>
            /// <returns>facility values</returns>
            public DataTable GetActiveFacilities(string city)
            {
                try
                {
                    VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                    DataTable dtfacility = locationDL.GetActiveFacility(city);
                    return dtfacility;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get security city function
            /// </summary>
            /// <param name="city">city value</param>
            /// <returns>security city value</returns>
            public DataTable GetSecurityCity(string city)
            {
                try
                {
                    VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                    DataTable dtfacility = locationDL.GetActiveFacility(city);
                    return dtfacility;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get Active Facilities
            /// </summary>
            /// <param name="city">city value</param>
            /// <returns>active facilities values</returns>
            public DataTable GetActiveFacilitiesIVS(string city)
            {
                try
                {
                    VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                    DataTable dtfacility = locationDL.GetActiveFacilityIVS(city);
                    return dtfacility;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Description: Method to get all locations based on country, cities and facilities
            /// </summary>
            /// <param name="Country"></param>
            /// <param name="City"></param>
            /// <returns>List of locations</returns>
            ////public List<string> GetCountries(string Country, string City)
            ////{

            ////    try
            ////    {

            ////        outputXML = AdminWebServiceInfo.AdminService_GetLocation(Country, City);

            ////        if (outputXML != null && outputXML.Trim() != string.Empty)
            ////        {
            ////            XmlDocument doc = new XmlDocument();
            ////            doc.LoadXml(outputXML);

            ////            if (Country.Equals(string.Empty))
            ////                NodeName = "admin:Countries";
            ////            else if ((Country.Length > 0) && (City.Equals(string.Empty)))
            ////                NodeName = "admin:Cities";
            ////            else if ((Country.Length > 0) && (City.Length > 0))
            ////                NodeName = "admin:Facilities";

            ////            XmlNodeList Nodes = doc.GetElementsByTagName(NodeName);
            ////            Locations.Clear();

            ////            if (Nodes != null)
            ////            {
            ////                foreach (XmlNode node in Nodes)
            ////                {
            ////                    Locations.Add(node.InnerText);

            ////                }
            ////            }
            ////        }

            ////        return Locations;
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        throw ex;
            ////    }

            ////}

            /// <summary>
            /// Description: method return configurable day field for city to raise request in advance
            /// </summary>
            /// Added by priti on 3rd June for VMS CR VMS31052010CR6
            /// <param name="purpose">Purpose value</param>
            /// <returns>Number of days allowable to raise request in advance</returns>
            public int GetAdvanceAllowabledays(string purpose)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL vmsMasterDataDL = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return vmsMasterDataDL.GetAdvanceAllowabledays(purpose);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Description: Method to get all locations based on country, cities and facilities
            /// </summary>
            /// <param name="Country"></param>
            /// <param name="City"></param>
            /// <returns>List of locations</returns>
            ////public List<string> GetFacilities(string City)
            ////{

            ////    try
            ////    {

            ////        outputXML = AdminWebServiceInfo.AdminService_GetFacilites(City);

            ////        if (outputXML != null && outputXML.Trim() != string.Empty)
            ////        {
            ////            XmlDocument doc = new XmlDocument();
            ////            doc.LoadXml(outputXML);
            ////            if (City.Length > 0)
            ////                NodeName = "Facility";

            ////            XmlNodeList Nodes = doc.GetElementsByTagName(NodeName);
            ////            Locations.Clear();

            ////            if (Nodes != null)
            ////            {
            ////                foreach (XmlNode node in Nodes)
            ////                {
            ////                    Locations.Add(node.InnerText.Trim());

            ////                }
            ////            }
            ////        }

            ////        return Locations;
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        throw ex;
            ////    }
            ////}

            /// <summary>
            /// Gets ID Card Location details
            /// </summary>
            /// <returns>location values as table</returns>
            public DataTable GetIDCardLocationDetails()
            {
                VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                DataSet dsresult = locationDL.GetIDCardLocations();
                return dsresult.Tables[0];
            }

            /// <summary>
            /// Get ID Card details for location
            /// </summary>
            /// <returns>ID Card locations</returns>
            public List<string> GetIDCardLocations()
            {
                List<string> idcardLocations = new List<string>();
                try
                {
                    VMSDataLayer.LocationDL locationDL = new VMSDataLayer.LocationDL();
                    DataSet dsresult = locationDL.GetIDCardLocations();

                    if (dsresult != null)
                    {
                        if (dsresult.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsresult.Tables[0].Rows.Count; i++)
                            {
                                idcardLocations.Add(dsresult.Tables[0].Rows[i]["Location"].ToString().Trim());
                            }
                        }
                    }

                    return idcardLocations;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get Active Country List
            /// </summary>
            /// <returns>country value</returns>
            public System.Data.DataTable GetActiveCountry()
            {
                try
                {
                    VMSDataLayer.LocationDL objLocationDL = new VMSDataLayer.LocationDL();
                    DataTable dtcountry = objLocationDL.GetActiveCountry();
                    return dtcountry;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get Active Country List From IVS
            /// </summary>
            /// <returns>country value</returns>
            public System.Data.DataTable GetActiveCountryIVS()
            {
                try
                {
                    VMSDataLayer.LocationDL objLocationDL = new VMSDataLayer.LocationDL();
                    DataTable dtcountry = objLocationDL.GetActiveCountryIVS();
                    return dtcountry;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        /// <summary>
        /// User details business layer
        /// </summary>
        public class UserDetailsBL
        {
            #region Variables

            /// <summary>
            /// Gets or sets Associate name
            /// </summary>
            public string AssociateName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets VNET value
            /// </summary>
            public string VNET
            {
                get;
                set;
            }

            ////List<string, string> UserDetails = new List<string, string>();

            /// <summary>
            /// Gets or sets output XML value
            /// </summary>
            public string OutputXML
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets Node name value
            /// </summary>
            public string NodeName
            {
                get;
                set;
            }

            #endregion

            ////Added for AddRole CR, generate report based on city/facility/roleID/associateID 

            /// <summary>
            /// Search role allocation Business layer
            /// </summary>
            /// <param name="setCity">Selected city value</param>
            /// <param name="setFacility">Selected facility</param>
            /// <param name="setRoleID">selected role Id</param>
            /// <param name="setAssociateID">selected associate Id</param>
            /// <param name="countryId">country Id value</param>
            /// <returns>roll allocation value</returns>
            public DataSet SearchRoleAllocationBL(string setCity, string setFacility, string setRoleID, string setAssociateID, string countryId)
            {
                try
                {
                    DataSet griddata = new DataSet();
                    griddata = new VMSDataLayer.VMSDataLayer.UserDetailsDL().SearchRoleAllocationDL(setCity, setFacility, setRoleID, setAssociateID, countryId);
                    return griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Search ID card admin allocation details
            /// </summary>
            /// <param name="setAssociateID">associate Id value</param>
            /// <param name="setLocation">location value</param>
            /// <returns>grid data</returns>
            public DataSet SearchIDCardAdminAllocationDetails(string setAssociateID, string setLocation)
            {
                try
                {
                    DataSet griddata = new VMSDataLayer.VMSDataLayer.UserDetailsDL().SearchIDCardAdminAllocationDetails(setAssociateID, setLocation);
                    return griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            ////End AddRole CR            

            /// <summary>
            /// Gets status
            /// </summary>
            /// <returns>user information</returns>
            public List<string> GetStatus()
            {
                try
                {
                    List<string> druserInfo = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetStatusDL();
                    return druserInfo;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////Added for AddRole CR, Get all the Roles from the Database

            /// <summary>
            /// Gets roles Business layer
            /// </summary>
            /// <returns>user information</returns>
            public DataTable GetRolesBL()
            {
                try
                {
                    DataTable druserInfo = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetRolesDL();
                    return druserInfo;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //// End AddRole CR 

            ////Added for AddRole CR, retrieve user role and location from User ID 

            /// <summary>
            /// Get user role location Business layer
            /// </summary>
            /// <param name="strUserId">user Id value</param>
            /// <returns>user information</returns>
            public DataTable GetUserRoleLocationBL(string strUserId)
            {
                try
                {
                    DataTable dtuserInfo = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetUserRoleLocationDL(strUserId);
                    return dtuserInfo;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (System.NullReferenceException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Gets location for user role ID card admin
            /// </summary>
            /// <param name="strUserId">user Id string value</param>
            /// <returns>user information</returns>
            public DataTable GetLocationForUserRole_IDCardAdmin(string strUserId)
            {
                try
                {
                    DataTable dtuserInfo = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetLocationForUserRole_IDCardAdmin(strUserId);
                    return dtuserInfo;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                catch (System.NullReferenceException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //// End AddRole CR 

            //// Added for AddRole CR, Insert/Update user's role and location in database
            ////changes made by bincey for LocationNameChange -- setUserID, FacilityId, setRoleID, setUpdatedBy

            /// <summary>
            /// Submit role details Business layer
            /// </summary>
            /// <param name="setUserID">user Id value</param>
            /// <param name="setFacility">facility value</param>
            /// <param name="setRoleID">role Id</param>
            /// <param name="setUpdatedBy">updated value</param>
            /// <param name="countryId">country Id</param>
            /// <returns>role details row</returns>
            public int SubmitRoleDetailsBL(string setUserID, int setFacility, string setRoleID, string setUpdatedBy, int countryId)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL submit = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    int rows = submit.SubmitRoleDetailsDL(setUserID, setFacility, setRoleID, setUpdatedBy, countryId);
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////added by bincey -- eqpmnt in custody

            /// <summary>
            /// Submit equipment custody
            /// </summary>
            /// <param name="visitDetailsID">visit details Id</param>
            /// <param name="tokenNumber">token number value</param>
            /// <param name="equipmentType">equipment type</param>
            /// <param name="description">description value</param>
            /// <param name="status">status value</param>
            /// <param name="facilityId">facility Id value</param>
            /// <param name="createdDate">created date</param>
            /// <param name="equipmentID">equipment Id value</param>
            public void SubmitEquipmentCustody(int visitDetailsID, int tokenNumber, string equipmentType, string description, string status, string facilityId, DateTime createdDate, int equipmentID)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL userDetailsDLObj = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    userDetailsDLObj.SubmitEquipmentCustody(visitDetailsID, tokenNumber, equipmentType, description, status, facilityId, createdDate, equipmentID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get token
            /// </summary>
            /// <param name="visitDetailsID">visitor details Id value</param>
            /// <param name="locationID">location Id value</param>
            /// <returns>token value</returns>
            public int GetToken(int visitDetailsID, string locationID)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL userDetailsDLObj = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    int tokenNumber = userDetailsDLObj.GetToken(visitDetailsID, locationID);
                    return tokenNumber;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////ends --

            /// <summary>
            /// adds Id card admin role
            /// </summary>
            /// <param name="setUserID">User Id value</param>
            /// <param name="location">location value</param>
            /// <param name="setRoleID">role Id</param>
            /// <param name="setUpdatedBy">updated by</param>
            /// <returns>Id Card admin role rows</returns>
            public int AddIDCardAdminRole(string setUserID, string location, string setRoleID, string setUpdatedBy)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL submit = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    int rows = submit.AddIDCardAdminRole(setUserID, location, setRoleID, setUpdatedBy);
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// removes Id card admin role
            /// </summary>
            /// <param name="setUserID">user Id value</param>
            /// <returns>submits the value</returns>
            public bool RemoveIDCardAdminRole(string setUserID)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL submit = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    return submit.RemoveIDCardAdminRole(setUserID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Gets ID card location for user
            /// </summary>
            /// <param name="setUserID">user Id value</param>
            /// <returns>user details</returns>
            public DataTable GetIDCardLocationForUser(string setUserID)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL userDetails = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    return userDetails.GetIDCardLocationForUser(setUserID);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            //// End AddRole CR 

            /// <summary>
            /// Get the User Roles
            /// </summary>
            /// <param name="userID">user Id value</param>
            /// <returns>user role value</returns>
            public List<string> GetUserRole(string userID)
            {
                List<string> userRole = new List<string>();

                try
                {
                    userRole = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetUserRole(userID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return userRole;
            }

            /// <summary>
            /// Getting CR 
            /// </summary>
            /// <param name="userID">user Id value</param>
            /// <returns>user details</returns>
            public AssociateGrade<string> GetGradeCode(string userID)
            {
                AssociateGrade<string> userDetails = new AssociateGrade<string>();
                try
                {
                    DataTable dt = new DataTable();
                    dt = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetCRSAssociateDetails(userID);

                    if (dt != null)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0]["Grade"].ToString()))
                        {
                            userDetails.GradeCode = dt.Rows[0]["Grade"].ToString().Trim();
                        }
                    }
                    else
                    {
                        userDetails.GradeCode = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return userDetails;
            }

            /// <summary>
            /// Get blood group
            /// </summary>
            /// <param name="userID">user Id value</param>
            /// <returns>user details value</returns>
            public BloodGroup<string> GetBloodGroup(string userID)
            {
                BloodGroup<string> userDetails = new BloodGroup<string>();
                try
                {
                    AdminWebServices adminWebServiceInfo = new AdminWebServices();
                    this.OutputXML = adminWebServiceInfo.AdminService_GetAssociate(userID);

                    if (!string.IsNullOrEmpty(this.OutputXML))
                    {
                        if (this.OutputXML.Trim() != string.Empty)
                        {
                            this.OutputXML = "<Root>" + this.OutputXML + "</Root>";
                        }

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(this.OutputXML);
                        XmlNodeList nodeGradeCode = doc.GetElementsByTagName("admin:BloodGroup");
                        if (nodeGradeCode != null)
                        {
                            string bloodGroup = nodeGradeCode[0].InnerText;
                            userDetails.AssociateBloodGroup = bloodGroup;
                        }
                        ////END 186631
                    }
                    else
                    {
                        this.OutputXML = "<Root></Root>";
                        ////Begin BugFix for NullReferenceException 15Apr2011 Vimal
                        userDetails.AssociateBloodGroup = "-";
                        ////End BugFix for NullReferenceException 15Apr2011 Vimal
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return userDetails;
            }

            /// <summary>
            /// Get associate details
            /// </summary>
            /// <param name="userId">user Id value</param>
            /// <returns>data table value</returns>
            public DataTable GetAssociateDetails(string userId)
            {
                DataTable dt = new DataTable();
                dt = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetCRSAssociateDetails(userId);
                return dt;
            }

            /// <summary>
            /// Get associate details
            /// </summary>
            /// <param name="userId">user Id value</param>
            /// <param name="visitortype">visitor type</param>
            /// <returns>data table value</returns>
            public IList<string> GetAssciateDetails(string userId, string visitortype)
            {
                var lstAssociate = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetAssciateDetails(userId, visitortype);
                return lstAssociate;
            }

            /// <summary>
            /// User details class
            /// </summary>
            /// <param name="userID">user Id value</param>
            /// <returns>data table value</returns>
            public UserDetails<string, string, string, string> GetUserDetails(string userID)
            {
                UserDetails<string, string, string, string> userDetails = new UserDetails<string, string, string, string>();
                try
                {
                    DataTable dt = new DataTable();
                    dt = new VMSDataLayer.VMSDataLayer.UserDetailsDL().GetCRSAssociateDetails(userID);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[0]["FirstName"].ToString()))
                            {
                                string firstName = dt.Rows[0]["FirstName"].ToString().Trim();
                                userDetails.AssociateName = firstName;
                                if (!string.IsNullOrEmpty(dt.Rows[0]["LastName"].ToString()))
                                {
                                    userDetails.AssociateName = dt.Rows[0]["LastName"].ToString().Trim() + "," + userDetails.AssociateName;
                                }
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[0]["EmailID"].ToString()))
                            {
                                userDetails.MailID = dt.Rows[0]["EmailID"].ToString().Trim();
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[0]["MobileNo"].ToString()))
                            {
                                userDetails.Vnet = dt.Rows[0]["MobileNo"].ToString().Trim();
                            }

                            if (!string.IsNullOrEmpty(dt.Rows[0]["Department"].ToString()))
                            {
                                userDetails.DeptDesc = dt.Rows[0]["Department"].ToString().Trim();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return userDetails;
            }

            ////public UserDetails<string, string, string, string> GetUserDetails(string UserID)
            ////{
            ////    UserDetails<string, string, string, string> UserDetails = new UserDetails<string, string, string, string>();
            ////    try
            ////    {
            ////        outputXML = AdminWebServiceInfo.AdminService_GetAssociate(UserID);

            ////        if (outputXML != string.Empty)
            ////        {
            ////            if (outputXML.Trim() != string.Empty)
            ////                outputXML = "<Root>" + outputXML + "</Root>";

            ////            XmlDocument doc = new XmlDocument();
            ////            doc.LoadXml(outputXML);
            ////            XmlNodeList NodeName = doc.GetElementsByTagName("admin:AssociateDisplayName");
            ////            XmlNodeList NodeVnet = doc.GetElementsByTagName("admin:PhoneHome1");
            ////            XmlNodeList NodeDeptDesc = doc.GetElementsByTagName("admin:DepartmentDescription");
            ////            XmlNodeList NodeEMailID = doc.GetElementsByTagName("admin:EMailID");

            ////            if (NodeName != null)
            ////            {
            ////                string name = NodeName[0].InnerText;
            ////                UserDetails.AssociateName = name;

            ////            }
            ////            if (NodeVnet != null)
            ////            {
            ////                string Vnet = NodeVnet[0].InnerText;
            ////                UserDetails.Vnet = Vnet;
            ////            }
            ////            if (NodeDeptDesc != null)
            ////            {
            ////                string DeptDesc = NodeDeptDesc[0].InnerText;
            ////                UserDetails.DeptDesc = DeptDesc;
            ////            }
            ////            //186631
            ////            if (NodeEMailID != null)
            ////            {
            ////                string EmailID = NodeEMailID[0].InnerText;
            ////                UserDetails.MailID = EmailID;

            ////            }

            ////            //END 186631
            ////        }
            ////        else
            ////        {
            ////            outputXML = "<Root></Root>";
            ////        }
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        throw ex;
            ////    }
            ////    return UserDetails;
            ////}

            /// <summary>
            /// Commented by 173710
            /// </summary>
            /// <param name="userId">user Id value</param>
            /// <param name="userName">user name value</param>
            /// <returns>user detail value</returns>
            public DataTable GetUserDetailsByIdName(string userId, string userName)
            {
                VMSDataLayer.VMSDataLayer.UserDetailsDL vmsUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                return vmsUserDetailsDL.GetUserDetailsByIdName(userId, userName);
            }

            /// <summary>
            /// host M+
            /// </summary>
            /// <param name="userId">user Id value</param>
            /// <param name="userName">user name value</param>
            /// <returns>user detail value</returns>
            public DataTable GetUserDetailsByIdNameClient(string userId, string userName)
            {
                VMSDataLayer.VMSDataLayer.UserDetailsDL vmsUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                return vmsUserDetailsDL.GetUserDetailsByIdNameclients(userId, userName);
            }
            ////bincey

            /// <summary>
            /// Get visit details Id
            /// </summary>
            /// <param name="requestId">request Id value</param>
            /// <returns>visit details</returns>
            public DataTable GetVisitDetailsID(string requestId)
            {
                VMSDataLayer.VMSDataLayer.UserDetailsDL vmsUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                return vmsUserDetailsDL.GetVisitDetailsID(requestId);
            }

            /// <summary>
            /// Is security class
            /// </summary>
            /// <param name="userId">user Id value</param>
            /// <returns>is security value</returns>
            public string IsSecurity(string userId)
            {
                VMSDataLayer.VMSDataLayer.UserDetailsDL vmsUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                return vmsUserDetailsDL.IsSecurity(userId);
            }

            /// <summary>
            /// Get CRS Associate Details
            /// </summary>
            /// <param name="associateID">associate Id</param>
            /// <returns>CRS details</returns>
            public DataTable GetCRSAssociateDetails(string associateID)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL userDetails = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    return userDetails.GetCRSAssociateDetails(associateID);
                }
                catch
                {
                    throw;
                }
            }

            /// <summary>
            /// Check application details
            /// </summary>
            /// <param name="applicantID">application Id</param>
            /// <returns>row value</returns>
            public DataTable CheckApplicantDetails(string applicantID)
            {
                DataTable returnTable = new DataTable();
                DataColumn dc = new DataColumn("Code");
                dc.DataType = System.Type.GetType("System.Int16");
                returnTable.Columns.Add(dc);
                DataColumn dc1 = new DataColumn("Reason");
                dc1.DataType = System.Type.GetType("System.String");
                returnTable.Columns.Add(dc1);
                VMSDataLayer.VMSDataLayer.MasterDataDL objIDCardDetails = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                string returnCode = string.Empty;
                try
                {
                    DataSet ds = new DataSet();
                    ds = objIDCardDetails.CheckApplicantDetails(applicantID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["AssociateId"].ToString()))
                        {
                            DataRow dr = returnTable.NewRow();
                            dr[0] = 0;
                            dr[1] = VMSConstants.VMSConstants.NOASSOCIATEID;
                            returnTable.Rows.Add(dr);
                        }

                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["BloodGroup"].ToString()))
                        {
                            DataRow dr = returnTable.NewRow();
                            dr[0] = 0;
                            dr[1] = VMSConstants.VMSConstants.NOBLOODGROUP;
                            returnTable.Rows.Add(dr);
                        }

                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["EmergencyContact"].ToString()))
                        {
                            DataRow dr = returnTable.NewRow();
                            dr[0] = 0;
                            dr[1] = VMSConstants.VMSConstants.NOEMERGENCYCONTACT;
                            returnTable.Rows.Add(dr);
                        }

                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FirstName"].ToString()))
                        {
                            DataRow dr = returnTable.NewRow();
                            dr[0] = 0;
                            dr[1] = VMSConstants.VMSConstants.NOFIRSTNAME;
                            returnTable.Rows.Add(dr);
                        }

                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LastName"].ToString()))
                        {
                            DataRow dr = returnTable.NewRow();
                            dr[0] = 0;
                            dr[1] = VMSConstants.VMSConstants.NOLASTNAME;
                            returnTable.Rows.Add(dr);
                        }

                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["AssociateType"].ToString()))
                        {
                            DataRow dr = returnTable.NewRow();
                            dr[0] = 0;
                            dr[1] = VMSConstants.VMSConstants.NOASSOCIATETYPE;
                            returnTable.Rows.Add(dr);
                        }

                        ////if (string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Image"].ToString()))
                        ////{
                        ////    DataRow dr = returnTable.NewRow();
                        ////    dr[0] = 0;
                        ////    dr[1] = VMSConstants.VMSConstants.NoImage;
                        ////    returnTable.Rows.Add(dr);
                        ////}
                        ////if (ds.Tables[1].Rows[0]["CardIssued"].ToString().Equals("1"))
                        ////{
                        ////    DataRow dr = returnTable.NewRow();
                        ////    dr[0] = 0;
                        ////    dr[1] = VMSConstants.VMSConstants.CardIssued;
                        ////    returnTable.Rows.Add(dr);
                        ////}
                        if (returnTable.Rows.Count == 0)
                        {
                            DataRow dr = returnTable.NewRow();
                            dr[0] = 1;
                            dr[1] = VMSConstants.VMSConstants.ALLDETAILSAVAILABLE;
                            returnTable.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = returnTable.NewRow();
                        dr[0] = 0;
                        dr[1] = VMSConstants.VMSConstants.NORECORD;
                        returnTable.Rows.Add(dr);
                    }

                    return returnTable;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (returnTable != null)
                    {
                        returnTable.Dispose();
                    }
                }
            }

            /// <summary>
            /// Get Id Card details
            /// </summary>
            /// <param name="associateID">associate Id</param>
            /// <param name="adminID">admin Id value</param>
            /// <returns>Id card details</returns>
            public DataSet GetIDCardDetails(string associateID, string adminID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    DataSet dslocation = getIDCard.GetIDCardLocation(adminID);
                    return getIDCard.GetIDCardDetails(associateID, dslocation.Tables[0].Rows[0]["Location"].ToString().Trim());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Insert Id card details
            /// </summary>
            /// <param name="adminID">admin Id value</param>
            /// <param name="associateID">associate Id</param>
            /// <returns>data table value</returns>
            public DataTable InsertIDCardDetails(string adminID, string associateID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    DataTable dtinsert = null;
                    DataSet dsidCardLocation = this.GetIDCardLocation(adminID);
                    int strlocation = Convert.ToInt32(dsidCardLocation.Tables[1].Rows[0]["LocationID"]);

                    dtinsert = getIDCard.InsertIDCardDetails(adminID, associateID, strlocation);
                    return dtinsert;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Get ID card location details
            /// </summary>
            /// <param name="adminId">admin Id value</param>
            /// <returns>Id card location details</returns>
            public DataSet GetIDCardLocation(string adminId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCardLocation = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return getIDCardLocation.GetIDCardLocation(adminId);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Get equipment in custody
            /// </summary>
            /// <param name="visitdetaisID">visit details</param>
            /// <returns>equipment details</returns>
            public DataTable GetEquipmentInCustody(int visitdetaisID)
            {
                DataTable dtequip = new DataTable();
                VMSDataLayer.VMSDataLayer.UserDetailsDL userDetailsDLObj = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                dtequip = userDetailsDLObj.GetEquipmentInCustody(visitdetaisID);
                return dtequip;
            }

            /// <summary>
            /// get token details
            /// </summary>
            /// <param name="visitdetaisID">visit details Id</param>
            /// <returns>token details</returns>
            public DataTable GetTokenDetails(int visitdetaisID)
            {
                DataTable dttokenDetaisl = new DataTable();
                VMSDataLayer.VMSDataLayer.UserDetailsDL userDetailsDLObj = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                dttokenDetaisl = userDetailsDLObj.GetTokenDetails(visitdetaisID);
                return dttokenDetaisl;
            }

            /// <summary>
            /// clear equipment data
            /// </summary>
            /// <param name="detailsId">details Id value</param>
            public void ClearEquipmentData(string detailsId)
            {
                VMSDataLayer.VMSDataLayer.UserDetailsDL userDetailsDLObj = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                userDetailsDLObj.ClearEquipmentData(detailsId);
            }

            /// <summary>
            /// Revoke access
            /// </summary>
            /// <param name="assoiateID">associate ID value</param>
            /// <param name="associateName">associate name value</param>
            /// <param name="updatedBy">updated by</param>
            /// <param name="role">role Id value</param>
            /// <param name="facility">facility value</param>
            /// <returns>result value</returns>
            public bool RevokeAccess(string assoiateID, string associateName, string updatedBy, string role, string facility)
            {
                VMSDataLayer.VMSDataLayer.UserDetailsDL userdetails = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                try
                {
                    var result = userdetails.RevokeAccess(assoiateID, updatedBy, role);
                    ////Send notification mail
                    if (result == true)
                    {
                        MailNotification mailNotification = new MailNotification();
                        mailNotification.SendRevokeNotification(assoiateID, associateName, updatedBy, role, facility);
                    }

                    return result;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        ////Begin Changes IVS CR006 Vimal

        /// <summary>
        /// Blood group class
        /// </summary>
        /// <typeparam name="B">associate blood group value</typeparam>
        public class BloodGroup<B>
        {
            /// <summary>
            /// Gets or sets B value
            /// </summary>
            public B AssociateBloodGroup
            {
                get;
                set;
            }
        }

        ////End Changes IVS CR006 Vimal

        ////Changes done for VMS CR VMS06072010CR09 by Priti

        /// <summary>
        /// Associate grade class
        /// </summary>
        /// <typeparam name="G">G value</typeparam>
        public class AssociateGrade<G>
        {
            /// <summary>
            /// Gets or sets G value
            /// </summary>
            public G GradeCode
            {
                get;
                set;
            }
        }
        ////end Changes done for VMS CR VMS06072010CR09 by Priti

        /// <summary>
        /// user details class
        /// </summary>
        /// <typeparam name="T">T value</typeparam>
        /// <typeparam name="U">U value</typeparam>
        /// <typeparam name="V">V value</typeparam>
        /// <typeparam name="S">S value</typeparam>
        public class UserDetails<T, U, V, S>
        {
            /// <summary>
            /// Gets or sets Associate value
            /// </summary>
            public T AssociateName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets VNET value
            /// </summary>
            public U Vnet
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets V department description
            /// </summary>
            public V DeptDesc
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets S Mail Id value
            /// </summary>
            public S MailID
            {
                get;
                set;
            }
        }

        #region MasterDataBL
        /// <summary>
        /// Location Details BL
        /// </summary>
        public class MasterDataBL
        {
            #region variables

            /// <summary>
            /// Gets or sets VMS Data layer
            /// </summary>
            private VMSDataLayer.VMSDataLayer.MasterDataDL vmsMasterDataDL = new VMSDataLayer.VMSDataLayer.MasterDataDL();

            #endregion

            ////bincey -- for last print details

            ////for last print details

            /// <summary>
            /// Update contractor details
            /// </summary>
            /// <param name="contractorId">contractor Id value</param>
            /// <param name="contractorName">contractor name value</param>
            /// <param name="vendorName">vendor name</param>
            /// <param name="superVisiorPhone">supervisor phone number</param>
            /// <param name="vendorPhoneNumber">vendor phone number</param>
            /// <param name="docStatus">documentation status value</param>
            /// <param name="status">status value</param>
            /// <param name="strContractorId">contractor value</param>
            /// <param name="userId">user Id</param>
            /// <returns>updated contractor value</returns>
            public bool UpdateContractorDetails(string contractorId, string contractorName, string vendorName, string superVisiorPhone, string vendorPhoneNumber, string docStatus, string status, int strContractorId, string userId)
            {
                bool updatestatus = false;
                VMSDataLayer.VMSDataLayer.UserDetailsDL objUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                updatestatus = objUserDetailsDL.UpdateContractorDetails(contractorId, contractorName, vendorName, superVisiorPhone, vendorPhoneNumber, docStatus, status, strContractorId, userId);
                return updatestatus;
            }

            /// <summary>
            /// print status details
            /// </summary>
            /// <param name="id">Id value</param>
            /// <returns>print status</returns>
            public PrintStatusDetails GetLastPrintStatus(int id)
            {
                PrintStatusDetails printstatus = new PrintStatusDetails();
                VMSDataLayer.VMSDataLayer.UserDetailsDL objUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                printstatus = objUserDetailsDL.GetLastPrintStatus(id);
                return printstatus;
            }

            /// <summary>
            /// Get Visit Status By request Id
            /// </summary>
            /// <param name="requestId">request id value</param>
            /// <returns>visit status</returns>
            public PrintStatusDetails GetVisitStatusByRequestId(int requestId)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.MasterDataDL objVisitorDetailsDL =
                        new VMSDataLayer.VMSDataLayer.MasterDataDL();
                    return objVisitorDetailsDL.GetVisitStatusByRequestId(requestId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Master data
            /// </summary>
            /// <param name="type">type value</param>
            /// <returns>purpose value</returns>
            public List<string> GetMasterData(string type)
            {
                List<string> purpose = new List<string>();
                try
                {
                    purpose = this.vmsMasterDataDL.GetMasterData(type);
                    return purpose;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get visitor image
            /// </summary>
            /// <param name="visitorID">visitor Id value</param>
            /// <returns>visitor image</returns>
            [SuppressMessage("Microsoft.Maintainability", "CA1500:UsePropertiesWhereAppropriate", Justification = "Reviewed")]
            public byte[] GetVisitorImage(string visitorID)
            {
                try
                {
                    byte[] dbyte;
                    VMSDataLayer.VMSDataLayer.MasterDataDL vmsMasterDataDL = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                    VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                    propertiesDC = vmsMasterDataDL.GetSearchDetails(Convert.ToInt32(visitorID));

                    if (!string.IsNullOrEmpty(propertiesDC.VisitorProofProperty.Photo.ToString()))
                    {
                        EmployeeBL ebl = new EmployeeBL();
                        string strDecryptedBinaryData = ebl.DecryptBinaryData(propertiesDC.VisitorProofProperty.Photo.ToString());
                        dbyte = Encoding.Default.GetBytes(strDecryptedBinaryData);
                        return dbyte;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Insert contractor details
            /// </summary>
            /// <param name="contractorName">Contractor name value</param>
            /// <param name="contractorNumber">contractor number value</param>
            /// <param name="vendorName">vendor name</param>
            /// <param name="status">status value</param>
            /// <param name="superVisiorPhone">supervisor phone number</param>
            /// <param name="vendorPhoneNumber">vendor phone number</param>
            /// <param name="docStatus">DOC status value</param>
            /// <param name="securityID">security Id value</param>
            /// <param name="currDate">current date</param>
            /// <param name="strLocationId">location Id value</param>
            /// <returns>contract details</returns>
            public bool InsertContractorIDDetails(string contractorName, string contractorNumber, string vendorName, string status, string superVisiorPhone, string vendorPhoneNumber, string docStatus, string securityID, DateTime currDate, string strLocationId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL objMasterDataDL = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                return objMasterDataDL.InsertContractorIDDetails(contractorName, contractorNumber, vendorName, status, superVisiorPhone, vendorPhoneNumber, docStatus, securityID, currDate, strLocationId);
            }

            /// <summary>
            /// Disable contractor
            /// </summary>
            /// <param name="contractorID">contractor Id value</param>
            /// <param name="updatedBy">updated by value</param>
            /// <returns>boolean value</returns>
            public bool DeleteContractor(int contractorID, string updatedBy)
            {
                VMSDataLayer.VMSDataLayer.UserDetailsDL objUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                return objUserDetailsDL.DeleteContractor(contractorID, updatedBy);
            }
        }
        #endregion

        /// <summary>
        /// request details BL
        /// </summary>
        public class RequestDetailsBL
        {
            #region variables

            /// <summary>
            /// Gets or sets VMS data layer
            /// </summary>
            private VMSDataLayer.VMSDataLayer.MasterDataDL vmsMasterDataDL = new VMSDataLayer.VMSDataLayer.MasterDataDL();

            /// <summary>
            /// Gets or sets object validation
            /// </summary>
            private Validations objValidation = new Validations();

            /// <summary>
            /// Gets or sets error message value
            /// </summary>
            public string StrErrorMessage
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets Grid data object
            /// </summary>
            public DataSet Griddata
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets department list value
            /// </summary>
            public List<string> Departments
            {
                get;
                set;
            }

            /// <summary>
            ///  Gets or sets associate list value
            /// </summary>
            public List<string> Associate
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets batch number value
            /// </summary>
            public string BatchNo
            {
                get;
                set;
            }

            #endregion

            /// <summary>
            /// Store Temporary AccessCard Details
            /// </summary>
            /// <param name="associateID">Associate ID</param>
            /// <param name="passNumber">Pass Number</param>
            /// <param name="strAccessCardNo">Access Card</param>
            /// <param name="facilityId">Facility Id value</param>
            /// <returns>success value</returns>
            public bool StoreTempAccessCardDetails(string associateID, string passNumber, string strAccessCardNo, string facilityId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL storeAccessCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return storeAccessCard.StoreTempAccessCardDetails(associateID, passNumber, strAccessCardNo, facilityId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Returns the Facility value
            /// </summary>
            /// <param name="facilityID">Facility ID value</param>
            /// <returns>returns facility value</returns>
            public string GetFacilityIDForAccessCard(string facilityID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getID = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return getID.GetFacilityIDForAccessCard(facilityID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// store the FileContentId
            /// </summary>
            /// <param name="associateID">Associate ID value</param>
            /// <param name="fileContentID">File Content ID value</param>
            /// <param name="appTemplateID">Application template value</param>
            /// <returns>returns store content value</returns>
            public bool StoreFileContentID(string associateID, string fileContentID, string appTemplateID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL storeID = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return storeID.StoreFileContentID(associateID, fileContentID, appTemplateID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Delete the FileContentId
            /// </summary>
            /// <param name="associateID">associate ID value</param>
            /// <returns>returns status of deleted file</returns>
            public bool DeleteFileContentID(string associateID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL storeID = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return storeID.DeleteFileContentID(associateID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// GetAssociateDes_Mailer from DB for IVS
            /// </summary>
            /// <param name="associateID">Associate ID value</param>
            /// <returns>returns associate details</returns>
            public int GetAssociateDes_Mailer(string associateID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL ivs_AssociateMailer = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return ivs_AssociateMailer.GetAssociateDes_Mailer(associateID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// store the chosen name if name>20 characters
            /// </summary>
            /// <param name="associateID">associate ID value</param>
            /// <param name="nameChosen">name chosen value</param>
            /// <returns>returns chosen name</returns>
            public bool StoreChosenName(string associateID, string nameChosen)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL storeName = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return storeName.StoreChosenName(associateID, nameChosen);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// check if the chosen name already exists in DB
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <returns>returns value if associate exists or not</returns>
            public string CheckNameAlreadyExists(string associateID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL checkName = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return checkName.CheckNameAlreadyExists(associateID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////added by ram(445894) for temp access card

            /// <summary>
            /// to enable access card
            /// </summary>
            /// <param name="passdetailsid">pass card Id value</param>
            public void EnableaccesscardBL(int passdetailsid)
            {
                VMSDataLayer.EmployeeDL objenablecard = new VMSDataLayer.EmployeeDL();
                objenablecard.Enableaccesscard(passdetailsid);
            }

            /// <summary>
            /// Get facility Id
            /// </summary>
            /// <param name="facilityName">Facility name</param>
            /// <returns>returns facility Id</returns>
            public DataTable GetFacilityId(string facilityName)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL objMasterDataDL = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                return objMasterDataDL.GetFacilityId(facilityName);
            }

            /// <summary>
            /// preview details
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <returns>gets ID card details</returns>
            public DataTable PreviewDetails(string associateID)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    ////string Location = GetIDCard.GetIDCardLocation(AdminID);
                    return getIDCard.PreviewDetails(associateID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Preview contractor details
            /// </summary>
            /// <param name="contractorId">contractor Id details</param>
            /// <returns>returns Id card details</returns>
            public DataTable PreviewContractorDetails(string contractorId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return getIDCard.PreviewContractorDetails(contractorId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Associate Details ready for ID Card Print
            /// </summary>
            /// <param name="fromDate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <param name="selectedCity">selected city value</param>
            /// <param name="status">status value</param>
            /// <returns>associate details</returns>
            public DataTable GetIDCardAssociateDetails(DateTime fromDate, DateTime todate, string selectedCity, string status)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    ////string Location = GetIDCard.GetIDCardLocation(AdminID);
                    return getIDCard.GetIDCardAssociateDetails(fromDate, todate, selectedCity, status);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get access card details
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <param name="strLocation">location value</param>
            /// <param name="passDetailsID">Pass details Id value</param>
            /// <returns>returns access card details</returns>
            public DataSet GetAccessCardDetails(string associateID, string strLocation, string passDetailsID)
            {
                string accesscardno = string.Empty;
                VMSDataLayer.EmployeeDL objaccess = new VMSDataLayer.EmployeeDL();
                try
                {
                    return objaccess.GetAccessCardNo(associateID.Trim(), strLocation, passDetailsID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get access card number
            /// </summary>
            /// <param name="strAccessCardNo">Access card number value</param>
            /// <returns>returns status value</returns>
            public bool Get_Accesscardnumber(string strAccessCardNo)
            {
                VMSDataLayer.VMSDataLayer objaccess = new VMSDataLayer.VMSDataLayer();
                bool status;

                try
                {
                    status = objaccess.AccesscardNumberCheck(strAccessCardNo);
                    return status;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get select card
            /// </summary>
            /// <returns>card details</returns>
            public DataSet GetSelect_CARD()
            {
                VMSDataLayer.EmployeeDL objaccess = new VMSDataLayer.EmployeeDL();
                try
                {
                    return objaccess.Select_Card_DDL();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            ////added by ram(445894) by temp access card

            /// <summary>
            /// To check access card history
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <returns>returns access card details</returns>
            public DataSet CheckaccesscardhistoryBL(string associateID)
            {
                VMSDataLayer.EmployeeDL objaccess = new VMSDataLayer.EmployeeDL();
                try
                {
                    return objaccess.CheckaccesscardhistoryDL(associateID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////added by ram(445894) for temp access card

            /// <summary>
            /// Access card service status
            /// </summary>
            /// <param name="associateid">associate Id value</param>
            /// <param name="passdetailid">pass detail Id value</param>
            /// <param name="activated_status">activated status value</param>
            /// <param name="deactivated_status">deactivated status value</param>
            public void Accesscardservicestatus(string associateid, string passdetailid, bool? activated_status, bool? deactivated_status)
            {
                VMSDataLayer.EmployeeDL objaccess = new VMSDataLayer.EmployeeDL();
                try
                {
                    objaccess.Accesscardservicestatus(associateid, passdetailid, activated_status, deactivated_status);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////private DataTable GetAccessCardNo(string access)
            ////{
            ////    throw new NotImplementedException();
            ////}

            /// <summary>
            /// Get Contractor ID card Details.
            /// </summary>
            /// <param name="searchKey">Search key value</param>
            /// <param name="locationId">Location Id value</param>
            /// <returns>Get Id card details</returns>
            public DataTable GetIDCardContractorDetails(string searchKey, string locationId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard =
                    new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    ////string Location = GetIDCard.GetIDCardLocation(AdminID);
                    return getIDCard.GetIDCardContractorDetails(searchKey, locationId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get Contractor ID card Details by ID
            /// </summary>
            /// <param name="contractorId">contractor Id value</param>
            /// <returns>Id card details</returns>
            public DataTable GetIDCardContractorDetailsById(int contractorId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return getIDCard.GetIDCardContractorDetailsById(contractorId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get vendor name
            /// </summary>
            /// <param name="sname">name value</param>
            /// <returns>returns vendor name</returns>
            public List<string> GetVendorName(string sname)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL objDataAccess =
                new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return objDataAccess.GetVendorName(sname);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Saves contractor print details
            /// </summary>
            /// <param name="contractorId">contractor Id value</param>
            /// <param name="hostId">host Id value</param>
            /// <param name="locationId">location value</param>
            /// <returns>status of saved values</returns>
            public int SaveContractorPrintDetails(int contractorId, string hostId, string locationId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL savePrintDetails =
                   new VMSDataLayer.VMSDataLayer.MasterDataDL();

                return savePrintDetails.SaveContractorPrintDetails(contractorId, hostId, locationId);
            }

            /// <summary>
            /// Get Location Details who logged into the system
            /// </summary>
            /// <param name="hostId">host Id value</param>
            /// <returns>location details</returns>
            public DataTable GetIDCardLocationDetails(string hostId)
            {
                VMSDataLayer.VMSDataLayer.MasterDataDL getIDCard = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                try
                {
                    return getIDCard.GetIDCardLocationDetails(hostId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Submit visitor information
            /// </summary>
            /// <param name="visitorProofObj">visitor project object</param>
            /// <param name="visitorMasterObj">visitor Master Object</param>
            /// <param name="visitorRequestObj">visitor request object</param>
            /// <param name="visitDetailObj">visit detail object</param>
            /// <param name="visitorEquipmentObj">visitor equipment object</param>
            /// <param name="visitorEmergencyContactObj">visitor emergency contact object</param>
            /// <param name="identityDetails">identity details</param>
            /// <returns>submit visitor information</returns>
            public int? SubmitVisitorInformation(
                VisitorProof visitorProofObj,
                VisitorMaster visitorMasterObj,
                VisitorRequest visitorRequestObj,
                VisitDetail[] visitDetailObj,
                VisitorEquipment[] visitorEquipmentObj,
                VisitorEmergencyContact visitorEmergencyContactObj,
                IdentityDetails identityDetails)
            {
                ////int? InsRequestID = 0;
                string returnsuccess = string.Empty;
                try
                {
                    this.objValidation = new Validations();
                    if (Convert.ToString(this.StrErrorMessage) != null)
                    {
                        throw new VMSBL.CustomException(this.StrErrorMessage);
                    }

                    int? success;
                    int? visitorID = 0;

                    success = this.vmsMasterDataDL.CheckVisitorDetailsExists(visitorMasterObj, visitorRequestObj, ref visitorID);

                    if (success.Equals(0))
                    {
                        var visitID = this.vmsMasterDataDL.SubmitVisitorInformation(visitorProofObj, visitorMasterObj, visitorRequestObj, visitDetailObj, visitorEquipmentObj, visitorEmergencyContactObj, visitorID, identityDetails);
                        if (visitDetailObj != null)
                        {
                            this.vmsMasterDataDL.InsertEquipment(visitDetailObj, visitorEquipmentObj);
                        }

                        if (visitID != 0 && identityDetails != null)
                        {
                            identityDetails.VisitorID = visitID;
                            this.vmsMasterDataDL.InsertIdentityDetails(identityDetails);
                        }
                    }

                    return success;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get native country
            /// </summary>
            /// <param name="country">country value</param>
            /// <returns>returns native country</returns>
            public DataTable GetNativeCountry(string country)
            {
                try
                {
                    return this.vmsMasterDataDL.GetNativeCountry(country);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get native country
            /// </summary>
            /// <param name="requestid">request id</param>
            /// <returns>returns native country</returns>
            public int GetAccessCardnumber(int requestid)
            {
                try
                {
                    return this.vmsMasterDataDL.GetAccessCardnumber(requestid);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////Begin Changes URL Restriction Vimal 14Jun2011

            /// <summary>
            /// submit request and send link
            /// </summary>
            /// <param name="refID">reference Id value</param>
            /// <param name="emailID">email Id value</param>
            /// <param name="country">country value</param>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="purpose">purpose value</param>
            /// <param name="hostID">host Id value</param>
            /// <param name="hostPhoneNo">host phone number</param>
            /// <param name="fromDate">from date</param>
            /// <param name="todate">to date</param>
            /// <param name="fromTime">from time</param>
            /// <param name="totime">to time</param>
            /// <param name="createdBy">created by</param>
            /// <returns>returns link</returns>
            public DataSet SubmitRequestAndSendLink(string refID, string emailID, string country, string city, string facility, string purpose, string hostID, string hostPhoneNo, string fromDate, string todate, string fromTime, string totime, string createdBy)
            {
                return this.vmsMasterDataDL.SendLinkToVisitor(refID, emailID, country, city, facility, purpose, hostID, hostPhoneNo, fromDate, todate, fromTime, totime, createdBy);
            }

            /// <summary>
            /// get link archive
            /// </summary>
            /// <param name="hostID">host Id value</param>
            /// <returns>archived link value</returns>
            public DataSet GetLinkArchieve(string hostID)
            {
                return this.vmsMasterDataDL.GetLinkArchieve(hostID);
            }

            /// <summary>
            /// resend visitor link
            /// </summary>
            /// <param name="hostID">host Id value</param>
            /// <param name="requestID">request Id value</param>
            /// <param name="emailID">email Id value</param>
            /// <returns>returns the visitor link</returns>
            public int ResendVisitorLink(string hostID, int requestID, string emailID)
            {
                DataSet ds = this.vmsMasterDataDL.ResendVisitorLink(hostID, requestID, emailID);
                return Convert.ToInt32(ds.Tables[0].Rows[0]["LinkID"].ToString());
            }

            /// <summary>
            /// Deactivate link
            /// </summary>
            /// <param name="linkID">link Id value</param>
            public void DeActivateLink(int linkID)
            {
                this.vmsMasterDataDL.DeActivateLink(linkID);
            }

            ////End Changes URL Restriction Vimal 14Jun2011

            ////changed by priti on 8th June for VMS CR VMS31052010CR6

            /// <summary>
            /// Badge Number value
            /// </summary>
            /// <param name="requestID">request Id value</param>
            /// <param name="hostMailID">host mail id</param>
            /// <param name="visitorProofObj">visitor proof object</param>
            /// <param name="visitorMasterObj">visitor master object</param>
            /// <param name="visitorRequestObj">visitor request object</param>
            /// <param name="visitorEquipmentObj">visitor equipment object</param>
            /// <param name="visitorEmergencyContactObj">visitor emergency contact object</param>
            /// <param name="modifiedBy">modified by</param>
            /// <returns>returns badge number</returns>
            public string GenerateBadge(int requestID, string hostMailID, VisitorProof visitorProofObj, VisitorMaster visitorMasterObj, VisitorRequest visitorRequestObj, VisitorEquipment[] visitorEquipmentObj, VisitorEmergencyContact visitorEmergencyContactObj, string modifiedBy)
            {
                try
                {
                    this.BatchNo = this.vmsMasterDataDL.GenerateBadge(requestID, modifiedBy);

                    if (ConfigurationManager.AppSettings["Mailtohostaftersubmission_enable"].ToString() == "true")
                    {
                        this.SendMailToHost(requestID, hostMailID, visitorProofObj, visitorMasterObj, visitorRequestObj, visitorEquipmentObj, visitorEmergencyContactObj);
                    }

                    return this.BatchNo;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Reprint Badge
            /// </summary>
            /// <param name="visitDetailsID">visitor details Id</param>
            /// <param name="modifiedBy">modified by value</param>
            /// <param name="comments">comments value</param>
            public void RePrintBadge(int visitDetailsID, string modifiedBy, string comments)
            {
                try
                {
                    this.vmsMasterDataDL.RePrintBadge(visitDetailsID, modifiedBy, comments);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Method to check the status of the client vcard in VMS DB.
            /// </summary>
            /// <param name="accessCard"></param>
            /// <returns></returns>
            public string CheckClientCardStatus(int accessCard)
            {
                RequestDetailsBL clientbusinessObj = new RequestDetailsBL();

                //method that gets the card usage detail
                DataTable cardDetails = clientbusinessObj.Checkaccesscardusage(accessCard);
                string clientCardStatus = "Available";
                foreach (DataRow dr in cardDetails.Rows)
                {
                    //the card is in use
                    if ((dr["badgestatus"].ToString().Contains("Host Notified") ||
                        dr["badgestatus"].ToString().Contains("Updated  VCard") ||
                        dr["badgestatus"].ToString().Contains("issued")))
                    {
                        clientCardStatus = "This Card is not available";
                    }
                }

                return clientCardStatus;
            }


            /// <summary>
            /// visitor check In
            /// </summary>
            /// <param name="visitDetailsID">visit details Id</param>
            /// <param name="modifiedBy">modified by value</param>
            /// <param name="vcardNo">visitor card</param>
            /// <returns>returns status of visitor checked in</returns>
            public bool VisitorCheckIn(int visitDetailsID, string modifiedBy,  string vcardNo = "NA", string accesscardNo = "NA")
            {
                string currenttime1 = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);

                try
                {
                    if (vcardNo == "NA")
                    {
                        
                        return this.vmsMasterDataDL.VisitorCheckIn(visitDetailsID, modifiedBy);
                    }
                    else
                    {
                        VMSDataLayer.VMSDataLayer objdetails = new VMSDataLayer.VMSDataLayer();
                        var cardStatus = accesscardNo == "NA" ? objdetails.CheckCardStatus(vcardNo.ToUpper()) :
                          CheckClientCardStatus(Convert.ToInt32(accesscardNo));

                        if (cardStatus == "Available")
                            return this.vmsMasterDataDL.CheckinVisitor(visitDetailsID, modifiedBy, vcardNo, accesscardNo);
                        else
                            return false;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// visitor check In from grid pop up
            /// </summary>
            /// <param name="visitDetailsID">visit details Id</param>
            /// <param name="modifiedBy">modified by value</param>
            /// <param name="vcardNo">visitor card</param>
            /// <returns>returns status of visitor checked in</returns>
            public bool VisitorCheckInFromPopup(int visitDetailsID, string modifiedBy, string vcardNo)
            {
                string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                try
                {
                    VMSDataLayer.VMSDataLayer objdetails = new VMSDataLayer.VMSDataLayer();
                    return this.vmsMasterDataDL.CheckinVisitor(visitDetailsID, modifiedBy, vcardNo, "NA");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Funxtion to surrender VCards
            /// </summary>
            /// <param name="checkoutDetails"></param>
            /// <param name="visitDetailsID"></param>
            /// <returns>true or false</returns>
            public bool SurrenderLostcard(string checkoutDetails, string visitDetailsID)
            {
                try
                {

                    return this.vmsMasterDataDL.SurrenderLostCard(checkoutDetails, visitDetailsID);



                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// extend the time
            /// </summary>
            /// <param name="requestID">request Id value</param>
            /// <param name="totime">to time value</param>
            public void Extendthetime(string requestID, string totime)
            {
                try
                {
                    this.vmsMasterDataDL.Extendthetime(requestID, totime);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// verify express check in code
            /// </summary>
            /// <param name="requestID">request Id value</param>
            /// <param name="securityID">security Id value</param>
            /// <returns>returns verify express check in code</returns>
            public long? VerifyExpressCheckinCode(int requestID, string securityID)
            {
                try
                {
                    return this.vmsMasterDataDL.VerifyExpressCheckinCode(requestID, securityID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// verify safety permit Id
            /// </summary>
            /// <param name="requestID">request Id value</param>
            /// <param name="securityID">security Id value</param>
            /// <param name="fromdateActual">from date actual</param>
            /// <param name="todateActual">actual to date</param>
            /// <param name="reqStatus">request status</param>
            /// <param name="strSP">SP value</param>
            /// <returns>returns verify safety permit id</returns>
            public DataSet VerifySafetyPermitId(int requestID, string securityID, DateTime fromdateActual, DateTime todateActual, string reqStatus, string strSP)
            {
                try
                {
                    return this.vmsMasterDataDL.VerifySafetyPermitId(requestID, securityID, fromdateActual, todateActual, reqStatus, strSP);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// safety permit security search
            /// </summary>
            /// <param name="securityID">security Id value</param>
            /// <param name="reqStatus">request status</param>
            /// <param name="requestID">request Id value</param>
            /// <returns>returns safety permit security search</returns>
            public DataSet SafetyPermitSecuritySearch(string securityID, string reqStatus, string requestID)
            {
                try
                {
                    return this.vmsMasterDataDL.SafetyPermitSecuritySearch(securityID, reqStatus, requestID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// permit check out
            /// </summary>
            /// <param name="permitId">permit Id value</param>
            /// <param name="reqstatus">request status</param>
            /// <param name="currentSystemTime">current system time</param>
            public void PermitCheckOut(string permitId, string reqstatus, DateTime currentSystemTime)
            {
                try
                {
                    this.vmsMasterDataDL.PermitCheckOut(permitId, reqstatus, currentSystemTime);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// permit check In
            /// </summary>
            /// <param name="permitId">permit Id</param>
            /// <param name="reqstatus">request status</param>
            /// <param name="currentSystemTime">current system time</param>
            public void PermitCheckIn(string permitId, string reqstatus, DateTime currentSystemTime)
            {
                try
                {
                    this.vmsMasterDataDL.PermitCheckIn(permitId, reqstatus, currentSystemTime);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Send mail to host
            /// </summary>
            /// <param name="requestID">request Id value</param>
            /// <param name="hostEmailID">host email Id</param>
            /// <param name="visitorProofObj">visitor proof object</param>
            /// <param name="visitorMasterObj">visitor master object</param>
            /// <param name="visitorLocObj">visitor location object</param>
            /// <param name="visitorEquipmentObj">visitor equipment object</param>
            /// <param name="visitorEmergencyContactObj">visitor emergency contact object</param>
            public void SendMailToHost(int requestID, string hostEmailID, VisitorProof visitorProofObj, VisitorMaster visitorMasterObj, VisitorRequest visitorLocObj, VisitorEquipment[] visitorEquipmentObj, VisitorEmergencyContact visitorEmergencyContactObj)
            {
                string strEmailMessage = null;

                try
                {
                    ////if (VisitorLocObj.BadgeStatus==VMSConstants.VMSConstants.BadgeIssued && VisitorLocObj.RequestID>0)
                    {
                        ////added for mail additions
                        TemplateParser htmlFile = new TemplateParser();
                        string strHost = string.Empty;
                        string strVisitor = string.Empty;
                        string strCompany = string.Empty;
                        string strRequestID = string.Empty;
                        string strInDate = string.Empty;
                        string strInTime = string.Empty;
                        string strBadgeNo = string.Empty;
                        string strOutDate = string.Empty;
                        string strOutTime = string.Empty;

                        ////ADD-9-feb2010
                        string strcity = string.Empty;
                        string strfacility = string.Empty;

                        string strSubjectLine = string.Empty;
                        string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                        string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                        int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                        string tokens = VMSConstants.VMSConstants.TOKENFORMAILTEMPLATE;
                        string values = string.Empty;

                        ////tknName|tknVisitor|tknCompany|tknReqID|tknInDate|tknInTime|tknBadgeNo|tknOutDate|tknOutTime

                        if (visitorLocObj.HostID != string.Empty && visitorLocObj.HostID != null)
                        {
                            strHost = visitorLocObj.HostID;
                        }

                        if (visitorMasterObj.FirstName != string.Empty && visitorMasterObj.FirstName != null)
                        {
                            strVisitor = visitorMasterObj.FirstName + ", " + visitorMasterObj.LastName;
                        }

                        if (visitorMasterObj.Company != string.Empty && visitorMasterObj.Company != null)
                        {
                            strCompany = visitorMasterObj.Company;
                        }

#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                        if (visitorLocObj.RequestID.ToString() != string.Empty && visitorLocObj.RequestID != null)
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                        {
                            strRequestID = visitorLocObj.RequestID.ToString();
                        }

                        if (visitorLocObj.City.ToString() != string.Empty && visitorLocObj.City != null)
                        {
                            strcity = visitorLocObj.City.ToString();
                        }

                        if (visitorLocObj.Facility.ToString() != string.Empty && visitorLocObj.Facility != null)
                        {
                            strfacility = visitorLocObj.Facility.ToString();
                        }

                        if (visitorLocObj.FromDate.ToString() != string.Empty && visitorLocObj.FromDate != null)
                        {
                            strInDate = visitorLocObj.FromDate.Value.ToString("dd/MM/yyyy");
                        }

                        if (visitorLocObj.FromTime.ToString() != string.Empty && visitorLocObj.FromTime != null)
                        {
                            strInTime = visitorLocObj.FromTime.ToString();
                        }

                        //// if (VisitorLocObj.BadgeNo.ToString() != string.Empty && VisitorLocObj.BadgeNo != null)
                        ////strBadgeNo = VisitorLocObj.BadgeNo.ToString();
                        strBadgeNo = this.BatchNo;

                        if (visitorLocObj.ToDate.ToString() != string.Empty && visitorLocObj.ToDate != null)
                        {
                            strOutDate = visitorLocObj.ToDate.Value.ToString("dd/MM/yyyy");
                        }

                        if (visitorLocObj.ToTime.ToString() != string.Empty && visitorLocObj.ToTime != null)
                        {
                            strOutTime = visitorLocObj.ToTime.ToString();
                        }

                        values = strHost + "|" + strVisitor + "|" + strCompany + "|" + strRequestID + "|" + strInDate + "|" + strInTime + "|" + strBadgeNo + "|" + strOutDate + "|" + strOutTime + "|" + strcity + "|" + strfacility;

                        ////if (VisitorLocObj.BadgeStatus.Equals(VMSConstants.VMSConstants.BadgeIssued))
                        {
                            string templateFileName = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.BADGEISSUEDTEMPLATE].ToString();
                            string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                            strSubjectLine = VMSConstants.VMSConstants.BADGEISSUEDSUBJECTLINE;
                            //// strEmailMessage.Append(Body);
                            strEmailMessage = strEmailMessage + body;
                            ////Send mail 
                            VMSUtility.SendEmail(hostEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            /// <summary>
            /// Method to send mail notification to the user who's role was added/edited
            /// </summary>
            /// <param name="strToAddress">to address value</param>
            /// <param name="userID">user Id value</param>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="role">role value</param>
            /// <param name="userName">user name</param>
            public void SendMailToUser(string strToAddress, string userID, string city, string facility, string role, string userName)
            {
                string strEmailMessage = null;
                try
                {
                    TemplateParser htmlFile = new TemplateParser();
                    string strSubjectLine = VMSConstants.VMSConstants.ROLEDELEGATIONSUBJECTLINE;
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();

                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string tokens = VMSConstants.VMSConstants.TOKENFORMAILTOUSER;
                    string values = string.Empty;

                    if (!string.IsNullOrEmpty(facility))
                    {
                        facility = string.Concat(facility, ",");
                    }

                    if (facility == "Select,")
                    {
                        facility = string.Empty;
                    }

                    values = userName + "|" + role + "|" + facility + "|" + city;
                    {
                        string templateFileName = System.Configuration.ConfigurationManager.AppSettings["RoleDelegationTemplate"].ToString();
                        ////string templateFileName = HttpContext.Current.Server.MapPath("~/MailTemplates/RoleDelegation.htm");
                        string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                        strSubjectLine = VMSConstants.VMSConstants.ROLEDELEGATIONSUBJECTLINE;
                        strEmailMessage = strEmailMessage + body;
                        ////added by ram(445894) forcheckin
                        VMSUtility.SendEmail(strToAddress, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            ////OCM start

            ////public void IVSmailtoHCM(string AssociateID, string strEmpemailID, string strManagerName, string strEmployeeName, string strManagerEmailId, string Facility, string strSubjectLine, string City,string country)

            /// <summary>
            /// IVS mail to HCM
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <param name="strEmpemailID">EMP email Id value</param>
            /// <param name="strManagerName">manager name</param>
            /// <param name="strEmployeeName">employee name</param>
            /// <param name="strManagerEmailId">manager email Id</param>
            /// <param name="facility">facility value</param>
            /// <param name="strSubjectLine">subject line</param>
            /// <param name="city">city value</param>
            /// <param name="strFromDate">from date</param>
            /// <param name="strToDate">to date</param>
            /// <param name="country">country value</param>
            public void IVSmailtoHCM(string associateID, string strEmpemailID, string strManagerName, string strEmployeeName, string strManagerEmailId, string facility, string strSubjectLine, string city, string strFromDate, string strToDate, string country)
            {
                string strEmailMessage = null;
                try
                {
                    ////added for mail additions
                    TemplateParser htmlFile = new TemplateParser();
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string tokens = VMSConstants.VMSConstants.TOKENFORIVSMAILTEMPLATE;

                    string values = string.Empty;
                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    ////tknmanager|tknemployee |tknfacility|tkndate
                    string fname = string.Empty;
                    string[] managerName = strManagerName.Split(',');
                    if (string.IsNullOrEmpty(managerName[1].ToString()))
                    {
                        fname = strManagerName;
                    }
                    else
                    {
                        fname = managerName[1];
                    }

                    ////Values = fName + "|" + strEmployeeName + "(" + AssociateID + ")" + "|" + Facility + "|" + DateTime.Today.DayOfWeek + " " + DateTime.Today.ToString("MMMM dd yyyy") + "|" + City +"|" + country;
                    values = fname + "|" + strEmployeeName + "(" + associateID + ")" + "|" + facility + "|" + city + "|" + strFromDate + "|" + strToDate;

                    string templateFileName = System.Configuration.ConfigurationManager.AppSettings["IVSmailTemplate"].ToString();
                    string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();

                    strEmailMessage = strEmailMessage + body;
                    ////Send mail 
                    VMSUtility.SendEmail(strManagerEmailId, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), strEmpemailID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            ////end

            /// <summary>
            /// send link to visitor
            /// </summary>
            /// <param name="strEmailMessage">email message value</param>
            /// <param name="visitorEmailID">visitor email Id</param>
            public void SendLinkToVisitor(string strEmailMessage, string visitorEmailID)
            {
                try
                {
                    ////added for mail additions
                    string strSubjectLine = string.Empty;
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    strSubjectLine = VMSConstants.VMSConstants.SENDLINKBYMAILSUBJECTLINE;
                    ////Send mail 
                    VMSUtility.SendEmail(visitorEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage, string.Empty);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            ////public void SendLinkToVisitor(string Link, string VisitorEmailID)
            ////{

            ////    string strEmailMessage = null;

            ////    try
            ////    {

            ////        //added for mail additions
            ////        TemplateParser HtmlFile = new TemplateParser();

            ////        string strLink = Link;
            ////        string strVisitorEmail = VisitorEmailID;

            ////        string strSubjectLine = string.Empty;
            ////        string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FromAddress].ToString();
            ////        string strSMTPVisitorAddress = strVisitorEmail;
            ////        int iSMTPPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.Port]);
            ////        string Tokens = VMSConstants.VMSConstants.TOKENFORMAILTOVISITORTEMPLATE;
            ////        string Values = string.Empty;
            ////        string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HostAdd].ToString();
            ////        //tknName|tknVisitor|tknCompany|tknReqID|tknInDate|tknInTime|tknBadgeNo|tknOutDate|tknOutTime

            ////        Values = strLink
            ////        string TemplateFileName = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.SendLinkTemplate].ToString();
            ////        string Body = HtmlFile.GetParsedHtmlFile(TemplateFileName, Tokens, Values).ToString();
            ////        strSubjectLine = VMSConstants.VMSConstants.SendLinkbyMailSubjectLine;
            ////        strEmailMessage = strEmailMessage + Body;
            ////        //Send mail 
            ////        VMSUtility.VMSUtility.SendEmail(strSMTPVisitorAddress, strFromAddress, strSMTPHostAddress, iSMTPPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);

            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        throw ex;
            ////    }

            ////    finally
            ////    {
            ////        strEmailMessage = null;
            ////    }
            ////}

            ////Begin Changes IVS CR 009 Vimal 30May2011

            /// <summary>
            /// Send acknowledgement to host
            /// </summary>
            /// <param name="strEmailID">email Id value</param>
            /// <param name="strHostName">host name</param>
            /// <param name="visitorEmailID">visitor email Id</param>
            public void SendAcknowledgementToHost(string strEmailID, string strHostName, string visitorEmailID)
            {
                string strEmailMessage = null;
                try
                {
                    TemplateParser htmlFile = new TemplateParser();
                    string tokens = VMSConstants.VMSConstants.TOKENFORMAILTOHOST;
                    string values = string.Empty;
                    values = string.Concat(strHostName, "|", visitorEmailID);
                    string templateFileName = System.Configuration.ConfigurationManager.AppSettings["VMSSendMailToHost"].ToString();
                    string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                    strEmailMessage = strEmailMessage + body;
                    string strVisitorEmail = visitorEmailID;
                    string strSubjectLine = string.Empty;
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    strSubjectLine = VMSConstants.VMSConstants.SENDLINKBYMAILSUBJECTLINE;
                    VMSUtility.SendEmail(strEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage, string.Empty);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// IVS image approval mail
            /// </summary>
            /// <param name="strEmployeeName">employee name</param>
            /// <param name="dtsubmittedOn">submitted on value</param>
            /// <param name="strStatus">status value</param>
            /// <param name="strEmployeeEmailID">employee email Id</param>
            /// <param name="strComments">comments value</param>
            public void IVSImageApprovalMail(string strEmployeeName, DateTime dtsubmittedOn, string strStatus, string strEmployeeEmailID, string strComments)
            {
                string strEmailMessage = null;
                try
                {
                    TemplateParser htmlFile = new TemplateParser();
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    string tokens = string.Empty;
                    string values = string.Empty;
                    string strSubjectLine = string.Empty;

                    if (strStatus.ToUpper().Equals("APPROVED"))
                    {
                        tokens = VMSConstants.VMSConstants.TOKENFORIVSIMAGEAPPROVEDMAIL;
                        values = strEmployeeName.Split(',')[1].Trim() + "|" + DateTime.Now.ToString("dd-MMM-yyyy");

                        string templateFileName = System.Configuration.ConfigurationManager.AppSettings["IVSImageApprovedMailTemplate"].ToString();
                        string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                        strSubjectLine = System.Configuration.ConfigurationManager.AppSettings["IVSImageApprovedSubjectLine"].ToString();
                        strEmailMessage = strEmailMessage + body;
                    }
                    else if (strStatus.ToUpper().Equals("REJECTED"))
                    {
                        tokens = VMSConstants.VMSConstants.TOKENFORIVSIMAGEREJECTEDEDMAIL;
                        values = strEmployeeName.Split(',')[1].Trim() + "|" + DateTime.Now.ToString("dd-MMM-yyyy") + "|" + strComments;
                        string templateFileName = System.Configuration.ConfigurationManager.AppSettings["IVSImageRejectedMailTemplate"].ToString();
                        string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                        strSubjectLine = System.Configuration.ConfigurationManager.AppSettings["IVSImageRejectedSubjectLine"].ToString();
                        strEmailMessage = strEmailMessage + body;
                    }

                    VMSUtility.SendEmail(strEmployeeEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            /// <summary>
            /// IVS SAN storage exception
            /// </summary>
            /// <param name="strException">exception value</param>
            public void IVSSANStorageException(string strException)
            {
                string strEmailMessage = null;
                try
                {
                    TemplateParser htmlFile = new TemplateParser();
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                    string strToAddress = System.Configuration.ConfigurationManager.AppSettings["ToAddress"].ToString();
                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    string tokens = string.Empty;
                    string values = string.Empty;
                    string strSubjectLine = string.Empty;

                    tokens = VMSConstants.VMSConstants.TOKENFORIVSSANSTORAGE;

                    values = strException + "|" + DateTime.Now.ToString("dd-MMM-yyyy");

                    string templateFileName = System.Configuration.ConfigurationManager.AppSettings["IVSSANException"].ToString();
                    string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                    strSubjectLine = System.Configuration.ConfigurationManager.AppSettings["IVSSANExceptionSubjectLine"].ToString();
                    strEmailMessage = strEmailMessage + body;
                    VMSUtility.SendEmail(strToAddress, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            /// <summary>
            /// IVS Image submitted mail
            /// </summary>
            /// <param name="strEmployeeName">employee name</param>
            /// <param name="dtsubmittedOn">submitted on value</param>
            /// <param name="strEmployeeEmailID">employee email Id value</param>
            public void IVSImageSubmittedMail(string strEmployeeName, DateTime dtsubmittedOn, string strEmployeeEmailID)
            {
                string strEmailMessage = null;
                try
                {
                    TemplateParser htmlFile = new TemplateParser();
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    string tokens = string.Empty;
                    string values = string.Empty;
                    string strSubjectLine = string.Empty;
                    tokens = VMSConstants.VMSConstants.TOKENFORIVSIMAGESUBMITTEDMAIL;
                    values = strEmployeeName.Split(',')[1].Trim() + "|" + DateTime.Now.ToString("dd-MMM-yyyy");
                    string templateFileName = System.Configuration.ConfigurationManager.AppSettings["IVSImageSubmittedMailTemplate"].ToString();
                    string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                    strSubjectLine = System.Configuration.ConfigurationManager.AppSettings["IVSImageSubmittedSubjectLine"].ToString();
                    strEmailMessage = strEmailMessage + body;
                    VMSUtility.SendEmail(strEmployeeEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }
            ////End Changes IVS CR 009 Vimal 30May2011

            ////LVS17052010CR07 start

            /// <summary>
            /// LVS incomplete
            /// </summary>
            /// <param name="mailDetails">mail details</param>
            public void LVSincomplete_verificationmail(DataSet mailDetails)
            {
                string strEmailMessage = null;
                try
                {
                    ////added for mail additions
                    TemplateParser htmlFile = new TemplateParser();
                    string strcheckintime = mailDetails.Tables[0].Rows[0]["CheckInTime"].ToString();
                    string strcheckouttime = mailDetails.Tables[0].Rows[0]["CheckOutTime"].ToString();
                    string strcheckincity = mailDetails.Tables[0].Rows[0]["LaptopPassIssuedCity"].ToString();
                    string strcheckinfacility = mailDetails.Tables[0].Rows[0]["VerifiedLocation"].ToString();
                    string strcheckoutcity = mailDetails.Tables[0].Rows[0]["LaptopPassReturnedCity"].ToString();
                    string strcheckoutfacility = mailDetails.Tables[0].Rows[0]["LaptopPassReturnLocation"].ToString();
                    string associate = mailDetails.Tables[0].Rows[0]["AssociateID"].ToString();
                    string securitymail = mailDetails.Tables[0].Rows[0]["Securitymail"].ToString();
                    string associatemail = mailDetails.Tables[0].Rows[0]["Associatemail"].ToString();

                    string strSubjectLine = string.Empty;
                    string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();

                    int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                    string tokens = VMSConstants.VMSConstants.TOKENFORMAILTOASSOCIATELVS;
                    string values = string.Empty;
                    string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                    ////tknName|tknVisitor|tknCompany|tknReqID|tknInDate|tknInTime|tknBadgeNo|tknOutDate|tknOutTime

                    ////tkncheckindate|tkncheckoutdate|tkncheckincity|tkncheckinfacility|tkncheckoutcity|tkncheckoutfacility|tknassociate
                    values = strcheckintime + "|" + strcheckouttime + "|" + strcheckincity + "|" + strcheckinfacility + "|" + strcheckoutcity + "|" + strcheckoutfacility + "|" + this.Associate;

                    string templateFileName = System.Configuration.ConfigurationManager.AppSettings["LVSmailTemplate"].ToString();
                    string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                    strSubjectLine = System.Configuration.ConfigurationManager.AppSettings["LVSmailTemplateSubject"].ToString();
                    strEmailMessage = strEmailMessage + body;
                    ////Send mail 
                    VMSUtility.SendEmail(associatemail, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), securitymail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            ////LVS17052010CR07 end

            /// <summary>
            /// ////////////
            /// 
            /// </summary>
            /// <param name="firstname"></param>
            /// <param name="lastname"></param>
            /// <param name="company"></param>
            /// <param name="HostID"></param>
            /// <returns></returns>     
            ////Begin VMS CR 16 Changes Uma            
            ////changes made bu bincey for Location Name Change CR

            /// <summary>
            /// get country Id
            /// </summary>
            /// <param name="countryName">country name value</param>
            /// <returns>country Id value</returns>
            public DataTable GetCountryId(string countryName)
            {
                try
                {
                    DataTable countryId = this.vmsMasterDataDL.GetCountryId(countryName);
                    return countryId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get country Id
            /// </summary>
            /// <param name="parentReferenceId">parent Reference Id</param>
            /// <param name="collectedby">collected by</param>
            /// <param name="dispatchedBy">dispatched By</param>
            public void Updatedispatchdetails(int parentReferenceId, int collectedby, int dispatchedBy)
            {
                try
                {
                    this.vmsMasterDataDL.Updatedispatchdetails(parentReferenceId, collectedby, dispatchedBy);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get country Id
            /// </summary>
            /// <param name="parentReferenceId">parent Reference Id</param>
            /// <param name="notifiedBy">notified By</param>
            public void UpdateNotifcationtoHost(int parentReferenceId, int notifiedBy)
            {
                try
                {
                    this.vmsMasterDataDL.UpdateNotifcationtoHost(parentReferenceId, notifiedBy);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get country Id
            /// </summary>
            /// <param name="parentReferenceId">parent Reference Id</param>
            /// <returns>country Id value</returns>
            public DataTable GetclientdetailswithParentrefernceID(int parentReferenceId)
            {
                try
                {
                    DataTable countryId = this.vmsMasterDataDL.GetClientRequestwithParentrefernceID(parentReferenceId);
                    return countryId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get equipment
            /// </summary>
            /// <param name="visitorID">visitor ID</param>
            /// <returns>equipment value</returns>
            public DataTable GetEquipmentDetails(int visitorID)
            {
                try
                {
                    DataTable equipment = this.vmsMasterDataDL.GetEquipmentDetails(visitorID);
                    return equipment;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get equipment
            /// </summary>
            /// <param name="parentID">parent ID</param>
            /// <param name="visitorID">visitor ID</param>
            /// <returns>Data Table</returns>
            public DataTable GetVisitDetails(int parentID, int visitorID)
            {
                try
                {
                    DataTable dtvisitdetails = this.vmsMasterDataDL.GetVisitDetails(parentID, visitorID);
                    return dtvisitdetails;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get equipment
            /// </summary>
            /// <param name="parentID">parent ID</param>
            /// <param name="visitorID">visitor ID</param>
            /// <returns>equipment value</returns>
            public DataTable GetLogDetails(int parentID, int visitorID)
            {
                try
                {
                    DataTable dtlogdetails = this.vmsMasterDataDL.GetLogDetails(parentID, visitorID);
                    return dtlogdetails;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// report information
            /// </summary>
            /// <param name="country">country value</param>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="fromdate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <param name="fromtime">from time value</param>
            /// <param name="totime">to time value</param>
            /// <param name="dept">department value</param>
            /// <param name="rpttype">RPT type</param>
            /// <param name="purpose">purpose value</param>
            /// <returns>returns report information</returns>
            public DataSet ReportInfo(int country, string city, string facility, string fromdate, string todate, string fromtime, string totime, string dept, string rpttype, string purpose)
            {
                ////public DataSet ReportInfo(string facility, string fromdate, string todate, string fromtime, string totime, string dept, string Rpttype, string purpose)
                try
                {
                    this.Griddata = this.vmsMasterDataDL.ReportInfo(country, city, facility, fromdate, todate, fromtime, totime, dept, rpttype, purpose);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////public DataSet ReportInfo(string facility, string fromdate, string todate, string fromtime, string totime, string dept, string Rpttype, string purpose)
            ////{
            ////    try
            ////    {
            ////        //Griddata = VMSMasterDataDL.ReportInfo(facility, fromdate, todate, fromtime, totime, dept, Rpttype, purpose);
            ////        //return Griddata;
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        throw ex;
            ////    }
            ////}
            ////End VMS CR 16 Changes Uma

            /// <summary>
            /// Chart VMS
            /// </summary>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="fromdate">from date</param>
            /// <param name="todate">to date</param>
            /// <param name="department">department value</param>
            /// <param name="country">country value</param>
            /// <returns>returns VMS value</returns>
            public DataSet Chart_VMS(string city, string facility, string fromdate, string todate, string department, string country)
            {
                try
                {
                    ////Begin VMS CR 16 Changes Uma
                    VMSDataLayer.VMSDataLayer.MasterDataDL dl = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                    this.Griddata = dl.Chart_VMS(city, facility, fromdate, todate, department, country);
                    ////End VMS CR 16 Changes Uma
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////Begin VMS CR 16 Changes Uma

            /// <summary>
            /// usage report search
            /// </summary>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="fromdate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <param name="department">department value</param>
            /// <param name="country">country value</param>
            /// <returns>returns usage report</returns>
            public DataSet Usagereportesearch(string city, string facility, string fromdate, string todate, string department, string country)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.MasterDataDL dl = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                    this.Griddata = dl.UsageReport(city, facility, fromdate, todate, department, country);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Pie graph
            /// </summary>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="fromdate">from date</param>
            /// <param name="todate">to date</param>
            /// <param name="department">department value</param>
            /// <param name="country">country value</param>
            /// <returns>returns PIE graph</returns>
            public DataSet PieGraph(string city, string facility, string fromdate, string todate, string department, string country)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.MasterDataDL dl = new VMSDataLayer.VMSDataLayer.MasterDataDL();
                    this.Griddata = dl.PieGraph(city, facility, fromdate, todate, department, country);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            ////End VMS CR 16 Changes Uma

            ////Begin Changes for VMS CR17 07Mar2011 Vimal

            /// <summary>
            /// Search Visitor master details
            /// </summary>
            /// <param name="searchstr">search value</param>
            /// <param name="strfn"> function value</param>
            /// <param name="strln">length value</param>
            /// <param name="strmn">MN value</param>
            /// <param name="strcomp">COMP value</param>
            /// <returns>returns visitor master details</returns>
            public DataSet SearchVisitorMasterDetails(string searchstr, string strfn, string strln, string strmn, string strcomp)
            {
                try
                {
                    ////Modified by lakshmi on aug 19 2009 - start
                    ////Griddata = VMSMasterDataDL.SearchVisitorMasterDetails(firstname, lastname, company, HostID);
                    ////end
                    this.Griddata = this.vmsMasterDataDL.SearchVisitorMasterDetails(searchstr, strfn, strln, strmn, strcomp);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            ////End Changes for VMS CR17 07Mar2011 Vimal
            ////added code uma
            ////Begin Changes for VMS CR17 07Mar2011 Vimal

            /// <summary>
            /// search visitor information
            /// </summary>
            /// <param name="firstname">first name value</param>
            /// <param name="lastname">last name value</param>
            /// <param name="company">company value</param>
            /// <param name="designation">designation value</param>
            /// <param name="nativecountry">native country</param>
            /// <param name="purpose">purpose value</param>
            /// <param name="city">city value</param>
            /// <param name="facility">facility value</param>
            /// <param name="badgeno">badge number</param>
            /// <param name="status">status value</param>
            /// <param name="host">host value</param>
            /// <param name="fromdate">from date</param>
            /// <param name="todate">to date</param>
            /// <param name="mobileNo">mobile number</param>
            /// <param name="date">date value</param>
            /// <param name="fromtime">from time</param>
            /// <param name="totime">to time</param>
            /// <param name="dept">department value</param>
            /// <param name="statusflag">status flag value</param>
            /// <returns>visitor information</returns>
            public DataSet SearchVisitorInfo(string firstname, string lastname, string company, string designation, string nativecountry, string purpose, string city, string facility, string badgeno, string status, string host, string fromdate, string todate, string mobileNo, string date, string fromtime, string totime, string dept, string statusflag)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.SearchVisitorInfo(firstname, lastname, company, designation, nativecountry, purpose, city, facility, badgeno, status, host, fromdate, todate, mobileNo, date, fromtime, totime, dept, statusflag);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// View log by security
            /// </summary>
            /// <param name="strSearchString">search string value</param>
            /// <param name="strUserID">user Id value</param>
            /// <param name="fromdate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <param name="reqStatus">request status</param>
            /// <returns>returns view log by security</returns>
            public DataSet ViewLogBySecurity(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus, string Expresscheckin, string Vcardnumber)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.ViewLogBySecurity(strSearchString, strUserID, fromdate, todate, reqStatus, Expresscheckin, Vcardnumber);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /// <summary>
            /// View Card log by security
            /// </summary>
            /// <param name="strSearchString">search string value</param>
            /// <param name="strUserID">user Id value</param>
            /// <param name="fromdate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <param name="reqStatus">request status</param>
            /// <returns>returns view log by security</returns>
            public DataSet ViewCardLogBySecurity(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.ViewCardLogBySecurity(strSearchString, strUserID, fromdate, todate, reqStatus);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



            /// <summary>
            /// View log by security
            /// </summary>
            /// <param name="strSearchString">search string value</param>
            /// <param name="strUserID">user Id value</param>
            /// <param name="fromdate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <param name="reqStatus">request status</param>
            /// <returns>returns view log by security</returns>
            public DataSet ViewLogBySecurityClients(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.ViewLogBySecurityClients(strSearchString, strUserID, fromdate, todate, reqStatus);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// View log by security
            /// </summary>
            /// <param name="parentID">parent ID</param>
            /// <returns>returns view log by security</returns>
            public DataSet GetAllVisitorList(int parentID)
            {
                try
                {
                    DataSet visitorlist = new DataSet();
                    visitorlist = this.vmsMasterDataDL.GetAllVisitorList(parentID);
                    return visitorlist;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// View log by security
            /// </summary>
            /// <param name="parentID">parent ID</param>
            /// <returns>returns view log by security</returns>
            public DataSet GetFirstClientVisitFacility(int parentID)
            {
                try
                {
                    DataSet clientfirstfacility = new DataSet();
                    clientfirstfacility = this.vmsMasterDataDL.GetFirstClientVisitFacility(parentID);
                    return clientfirstfacility;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            ////End Changes for VMS CR17 07Mar2011 Vimal
            ////added code uma

            /// <summary>
            /// view log by security for SP
            /// </summary>
            /// <param name="strSearchString">search string value</param>
            /// <param name="strUserID">user Id value</param>
            /// <param name="fromdate">from date</param>
            /// <param name="todate">to date value</param>
            /// <param name="reqStatus">request status</param>
            /// <param name="strSP">SP value</param>
            /// <returns>returns Log by security</returns>
            public DataSet ViewLogBySecurityforSP(string strSearchString, string strUserID, string fromdate, string todate, string reqStatus, string strSP)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.ViewLogBySecurityforSP(strSearchString, strUserID, fromdate, todate, reqStatus, strSP);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// View log by security for SP work details
            /// </summary>
            /// <param name="strUserID">user Id value</param>
            /// <param name="reqStatus">request status</param>
            /// <returns>returns view by security for SP work details</returns>
            public DataSet ViewLogBySecurityforSPWorkDetails(string strUserID, string reqStatus)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.ViewLogBySecurityforSPWorkDetails(strUserID, reqStatus);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// get visitor off set
            /// </summary>
            /// <param name="requestId">request Id value</param>
            /// <returns>returns visitor off set</returns>
            public string GetVisitorOffset(string requestId)
            {
                return this.vmsMasterDataDL.GetVisitorOffset(requestId);
            }

            /// <summary>
            /// Multiple visitors
            /// </summary>
            /// <returns>returns visitor Id</returns>
            public List<int> MultipleVisitors()
            {
                List<int> visitorID = new List<int>();

                try
                {
                    visitorID = this.vmsMasterDataDL.MultipleVisitors();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return visitorID;
            }

            ////code added by 190075

            /// <summary>
            /// Multi city facility
            /// </summary>
            /// <param name="type">type value</param>
            /// <returns>returns visitor Id</returns>
            public List<int> MultiCityFacility(string type)
            {
                List<int> visitorID = new List<int>();

                try
                {
                    visitorID = this.vmsMasterDataDL.MultiCityFacility(type);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return visitorID;
            }

            ////added code uma

            /// <summary>
            /// Same day multiple visits
            /// </summary>
            /// <param name="date">date value</param>
            /// <param name="city">city value</param>
            /// <returns>returns grid data</returns>
            public DataSet SameDayMultipleVisits(string date, string city)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.SameDayMultipleVisits(date, city);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get country code
            /// </summary>
            /// <param name="country">country value</param>
            /// <returns>returns country code</returns>
            public string GetCountryCode(string country)
            {
                string countrycode;
                try
                {
                    countrycode = this.vmsMasterDataDL.GetCountryCode(country);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return countrycode;
            }

            /// <summary>
            /// Get Security Facility Details
            /// </summary>
            /// <param name="securityID">security value</param>
            /// <returns>security city value</returns>
            public DataSet GetSecurityCity(string securityID)
            {
                return this.vmsMasterDataDL.GetSecurityCity(securityID);
            }

            /// <summary>
            /// Check Contractor Number already available
            /// </summary>
            /// <param name="contractorNumber">contractor number value</param>
            /// <param name="locationId">location id value</param>
            /// <returns>check contractor number exist</returns>
            public bool CheckContratorNumberExist(string contractorNumber, string locationId)
            {
                return this.vmsMasterDataDL.CheckContratorNumberExist(contractorNumber, locationId);
            }

            //// bincey -- for Contr ID edit

            /// <summary>
            /// check contractor number exists for edit
            /// </summary>
            /// <param name="strContractorId">contractor Id value</param>
            /// <param name="contractorNumber">contractor number value</param>
            /// <param name="locationId">location Id value</param>
            /// <returns>returns status if contractor number exists</returns>
            public bool CheckContratorNumberExistForEdit(string strContractorId, string contractorNumber, string locationId)
            {
                return this.vmsMasterDataDL.CheckContratorNumberExistForEdit(strContractorId, contractorNumber, locationId);
            }

            //// ends

            /// <summary>
            /// Get location details by Id
            /// </summary>
            /// <param name="requestId">request Id</param>
            /// <returns>returns location details by Id</returns>
            public DataTable GetLocationDetailsById(int requestId)
            {
                return this.vmsMasterDataDL.GetLocationDetailsById(requestId);
            }

            /// <summary>
            /// Get facility VNET
            /// </summary>
            /// <param name="locationId">location Id</param>
            /// <returns>returns VNET value</returns>
            public string GetFacilityVnet(int locationId)
            {
                ////string city;
                try
                {
                    return this.vmsMasterDataDL.GetFacilityVnet(locationId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ////return city;
            }
            ////end

            #region Comment for CR IRVMS22062010CR07
            //// Commented for CR IRVMS22062010CR07  starts here done by Priti
            ////public string GetSecurityCity(string securityID)
            ////{
            ////    string city;

            ////    try
            ////    {

            ////        city = VMSMasterDataDL.GetSecurityCity(securityID);
            ////    }

            ////    catch (Exception ex)
            ////    {
            ////        throw ex;
            ////    }

            ////    return city;
            ////}
            ////end comment for CR IRVMS22062010CR07  starts here done by Priti
            #endregion

            /// <summary>
            /// Get visits to department
            /// </summary>
            /// <param name="associate">associate name</param>
            /// <returns>returns grid data</returns>
            public DataSet GetVisitstoDept(List<string> associate)
            {
                try
                {
                    this.Griddata = this.vmsMasterDataDL.GetVisitstoDept(associate);
                    return this.Griddata;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get departments
            /// </summary>
            /// <returns>department value</returns>
            public List<string> GetDepartments()
            {
                try
                {
                    this.Departments = this.vmsMasterDataDL.GetDepartments();
                    return this.Departments;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Badge return values
            /// </summary>
            /// <param name="reqid">request Id</param>
            /// <returns>returns dataset</returns>
            public DataSet Badgereturnvalues(string reqid)
            {
                DataSet dt = new DataSet();
                dt = this.vmsMasterDataDL.Badgereturnvalues(reqid);
                return dt;
            }

            /// <summary>
            /// Badge return values
            /// </summary>
            /// <param name="reqid">request Id</param>
            /// <returns>returns dataset</returns>
            public DataSet BadgereturnvaluesClients(string reqid)
            {
                DataSet dt = new DataSet();
                dt = this.vmsMasterDataDL.BadgereturnvaluesClients(reqid);
                return dt;
            }

            /// <summary>
            /// Badge return values
            /// </summary>
            /// <returns>returns dataset</returns>
            public DataSet Getgrdvisitorsearchresult(string searchtext)
            {
                DataSet dt = new DataSet();
                dt = this.vmsMasterDataDL.Getgrdvisitorsearchresult(searchtext);
                return dt;
            }

            /// <summary>
            /// Badge return values
            /// </summary>
            /// <param name="parentID">parent ID</param>
            /// <returns>returns dataset</returns>
            public DataSet GetBadgeStatusClients(int parentID)
            {
                DataSet dt = new DataSet();
                dt = this.vmsMasterDataDL.GetBadgeStatusClients(parentID);
                return dt;
            }

            ////changed by priti on 8th June for VMS CR VMS31052010CR6

            /// <summary>
            /// update badge status
            /// </summary>
            /// <param name="visitDetailsID">visit details Id</param>
            /// <param name="bdgestatus">badge status</param>
            /// <param name="actualOutTime">actual out time</param>
            /// <param name="reqstatus">request status</param>
            /// <param name="comments">comments value</param>
            /// <param name="modifiedBy">modified by value</param>
            /// <returns>returns data table value</returns>
            public DataSet Updatebdgestatus(string visitDetailsID, string bdgestatus, string actualOutTime, string reqstatus, string comments, string modifiedBy)
            {
                try
                {
                    RequestDetailsBL obj = new RequestDetailsBL();
                    ////added for mail additions
                    DataSet dt = new DataSet();
                    string currenttime = Convert.ToString(HttpContext.Current.Session["currentDateTime"]);
                    this.vmsMasterDataDL.UpdateBadgeStatus(visitDetailsID, bdgestatus, actualOutTime, reqstatus, comments, modifiedBy);
                    dt = obj.Badgereturnvalues(visitDetailsID);
                    return dt;

                    ////VMSMasterDataDL.UpdateBadgeStatus(ReqID, bdgestatus, ActualOutTime, reqstatus, comments);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Function to update checkout details
            /// </summary>
            /// <param name="visitDetailsID"></param>
            /// <param name="bdgestatus"></param>
            /// <param name="actualOutTime"></param>
            /// <param name="reqstatus"></param>
            /// <param name="comments"></param>
            /// <param name="modifiedBy"></param>
            /// <returns></returns>
            public bool UpdateCheckoutStatus(string checkoutDetails, string visitDetailsID, string reqstatus, string modifiedBy)
            {
                try
                {
                    RequestDetailsBL obj = new RequestDetailsBL();
                    return this.vmsMasterDataDL.UpdateCheckoutStatus(checkoutDetails, visitDetailsID, reqstatus, modifiedBy);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// Function to update reissued card status
            /// </summary>
            /// <param name="currentCard">current card</param>
            /// <param name="newCard">new card</param>
            /// <param name="visitDetailsId">visit details id</param>
            /// <param name="reason">card reason</param>
            /// <returns> returns boolean</returns>
            public bool ReIssueLostCard(string currentCard, string newCard, string visitDetailsId, string reason)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer objdetails = new VMSDataLayer.VMSDataLayer();
                    var cardStatus = objdetails.CheckCardStatus(newCard.ToUpper());
                    if (cardStatus == "Available")
                    {
                        return this.vmsMasterDataDL.ReIssueLostCard(currentCard, newCard, visitDetailsId, reason);
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
            }

            /// <summary>
            /// update badge status
            /// </summary>
            /// <param name="visitDetailsID">visit details Id</param>
            /// <param name="bdgestatus">badge status</param>
            /// <param name="visitorid">visitor id</param>
            /// <param name="reqstatus">request status</param>
            /// <param name="comments">comments value</param>
            /// <param name="modifiedBy">modified by value</param>
            /// <param name="selectedfacility">selected facility</param>
            /// <param name="reportedby">reported by</param>
            /// <param name="reportedon">reported on</param>
            /// <returns>returns data table value</returns>
            public DataSet UpdatebdgestatusClients(string visitDetailsID, string bdgestatus, string visitorid, string reqstatus, string comments, string modifiedBy, string selectedfacility, string reportedby, string reportedon)
            {
                try
                {
                    RequestDetailsBL obj = new RequestDetailsBL();
                    ////added for mail additions
                    DataSet dt = new DataSet();

                    this.vmsMasterDataDL.UpdateBadgeStatusClients(visitDetailsID, bdgestatus, visitorid, reqstatus, comments, modifiedBy, selectedfacility, reportedby, reportedon);
                    dt = obj.BadgereturnvaluesClients(visitDetailsID);
                    return dt;

                    ////VMSMasterDataDL.UpdateBadgeStatus(ReqID, bdgestatus, ActualOutTime, reqstatus, comments);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Send badge return mail
            /// </summary>
            /// <param name="dt">data set value</param>
            /// <param name="startDate">start date</param>
            /// <param name="endDate">end date</param>
            /// <param name="actualOutDate">actual out date</param>
            public void Sendbadgereturnmail(DataSet dt, DateTime startDate, DateTime endDate, DateTime actualOutDate)
            {
                TemplateParser htmlFile = new TemplateParser();

                string strEmailMessage = string.Empty;
                string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                string tokens = "tknName|tknCompany|tknFromdate|tknTodate|tknFromtime|tknTotime|tknOutTime|tknBadgeno|tknCity|tknFacility";
                string values = string.Empty;
                string hostEmailID = this.GetHostmailID(dt.Tables[0].Rows[0].ItemArray[0].ToString());
                string name = dt.Tables[0].Rows[0].ItemArray[4].ToString();
                string company = dt.Tables[0].Rows[0].ItemArray[5].ToString();

                string fromdate = startDate.ToString("dd/MM/yyyy");
                string todate = endDate.ToString("dd/MM/yyyy");
                string intime = startDate.ToShortTimeString();
                string totime = endDate.ToShortTimeString();
                string outTime = actualOutDate.ToShortTimeString();
                string badgeno = dt.Tables[0].Rows[0].ItemArray[11].ToString();
                string city = dt.Tables[0].Rows[0].ItemArray[12].ToString();
                string facility = dt.Tables[0].Rows[0].ItemArray[13].ToString();
                values = name + "|" + company + "|" + fromdate + "|" + todate + "|" + intime + "|" + totime + "|" + outTime + "|" + badgeno + "|" + city + "|" + facility;
                string templateFileName = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.BADGERETURNEDTEMPLATE].ToString();
                string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                string strSubjectLine = VMSConstants.VMSConstants.SUBJECTLINERETURNED;
                //// strEmailMessage.Append(Body);
                strEmailMessage = strEmailMessage + body;
                ////Send mail 
                VMSUtility.SendEmail(hostEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
            }

            /// <summary>
            /// To Log download photo information
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <param name="adminId">admin Id value</param>
            public void SaveLogDownLoadPhoto(string associateID, string adminId)
            {
                try
                {
                    VMSDataLayer.EmployeeDL objEmp = new VMSDataLayer.EmployeeDL();
                    objEmp.SaveLogDownLoadPhoto(associateID, adminId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To update print status
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <param name="adminId">admin Id value</param>
            /// <param name="printstatus">print status</param>
            /// <param name="location">location value</param>
            public void UpdatePrintStatus(string associateID, string adminId, string printstatus, int location)
            {
                try
                {
                    VMSDataLayer.EmployeeDL objEmp = new VMSDataLayer.EmployeeDL();
                    objEmp.UpdatePrintStatus(associateID, adminId, printstatus, location);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get host mail Id
            /// </summary>
            /// <param name="hostid">host id value</param>
            /// <returns>host email Id</returns>
            public string GetHostmailID(string hostid)
            {
                UserDetailsBL bl = new UserDetailsBL();
                UserDetails<string, string, string, string> userDetails1 = new UserDetails<string, string, string, string>();
                userDetails1 = bl.GetUserDetails(hostid);
                string hostEmailID = Convert.ToString(userDetails1.MailID);
                return hostEmailID;
            }

            ////added for done for VMS CR VMS06072010CR09 by Priti

            /// <summary>
            /// Gives grade code
            /// </summary>
            /// <param name="associateId">associate id value</param>
            /// <returns>Grade code of Associate Id</returns>
            public string GetAssociateGrade(string associateId)
            {
                UserDetailsBL bl = new UserDetailsBL();
                AssociateGrade<string> grade = new AssociateGrade<string>();
                grade = bl.GetGradeCode(associateId);
                string gradeCode = grade.GradeCode.ToString();
                return gradeCode;
            }

            /// <summary>
            /// time extended mail to host
            /// </summary>
            /// <param name="dt">data set value</param>
            /// <param name="startDate">start date value</param>
            /// <param name="endDate">end date value</param>
            /// <param name="beforeExtend">before extend value</param>
            public void Timeextendedmailtohost(DataSet dt, DateTime startDate, DateTime endDate, DateTime beforeExtend)
            {
                TemplateParser htmlFile = new TemplateParser();
                GenericTimeZone genTimeZone = new GenericTimeZone();
                string strEmailMessage = string.Empty;
                string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                string tokens = "tknName|tknCompany|tknFromdate|tknTodate|tknFromtime|tknTotime|tknOutTime|tknBadgeno|tknCity|tknFacility|tknExtendedTime";
                string values = string.Empty;
                UserDetailsBL bl = new UserDetailsBL();
                UserDetails<string, string, string, string> userDetails1 = new UserDetails<string, string, string, string>();
                userDetails1 = bl.GetUserDetails(dt.Tables[0].Rows[0].ItemArray[0].ToString());
                string hostEmailID = userDetails1.MailID.ToString();
                string name = dt.Tables[0].Rows[0].ItemArray[4].ToString();
                string company = dt.Tables[0].Rows[0].ItemArray[5].ToString();
                string fromdate = startDate.ToString("dd/MM/yyyy");
                string todate = endDate.ToString("dd/MM/yyyy");
                string timeStart = startDate.ToShortTimeString();
                string timeEnd = beforeExtend.ToShortTimeString();
                string extendTime = endDate.ToShortTimeString();
                string intime = timeStart;
                string totime = timeEnd;
                string outTime = timeEnd;
                string badgeno = dt.Tables[0].Rows[0].ItemArray[11].ToString();
                string city = dt.Tables[0].Rows[0].ItemArray[12].ToString();
                string facility = dt.Tables[0].Rows[0].ItemArray[13].ToString();
                values = name + "|" + company + "|" + fromdate + "|" + todate + "|" + intime + "|" + totime + "|" + outTime + "|" + badgeno + "|" + city + "|" + facility + "|" + extendTime;
                string strPath = HttpContext.Current.Server.MapPath(string.Empty);
                string templateFileName = string.Concat(strPath, System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.TIMEEXTENDEDTEMPLATE].ToString());
                string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                string strSubjectLine = VMSConstants.VMSConstants.ALERTNOTIFICATION;
                //// strEmailMessage.Append(Body);
                strEmailMessage = strEmailMessage + body;
                ////Send mail 
                VMSUtility.SendEmail(hostEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
            }

            /// <summary>
            /// time extended mail to host from SP
            /// </summary>
            /// <param name="dt">data set value</param>
            /// <param name="startDate">start date value</param>
            /// <param name="endDate">end date value</param>
            /// <param name="beforeExtend">before extend value</param>
            /// <param name="strHostId">host Id value</param>
            public void TimeextendedmailtohostfromSP(DataSet dt, DateTime startDate, DateTime endDate, DateTime beforeExtend, string strHostId)
            {
                string userID = string.Empty;
                if (strHostId.Contains("("))
                {
                    int startIndex = strHostId.IndexOf("(") + 1;
                    userID = strHostId.Substring(startIndex, strHostId.Length - (startIndex + 1)).ToString();
                }
                else
                {
                    userID = strHostId;
                }

                TemplateParser htmlFile = new TemplateParser();
                GenericTimeZone genTimeZone = new GenericTimeZone();
                string strEmailMessage = string.Empty;
                string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                string tokens = "tknName|tknCompany|tknFromdate|tknTodate|tknFromtime|tknTotime|tknOutTime|tknBadgeno|tknCity|tknFacility|tknExtendedTime";
                string values = string.Empty;
                UserDetailsBL bl = new UserDetailsBL();
                UserDetails<string, string, string, string> userDetails1 = new UserDetails<string, string, string, string>();
                ////UserDetails1 = BL.GetUserDetails(dt.Tables[0].Rows[0].ItemArray[0].ToString());
                userDetails1 = bl.GetUserDetails(userID);
                string hostEmailID = userDetails1.MailID.ToString();
                string name = dt.Tables[0].Rows[0].ItemArray[4].ToString();
                string company = dt.Tables[0].Rows[0].ItemArray[5].ToString();
                string fromdate = startDate.ToString("dd/MM/yyyy");
                string todate = endDate.ToString("dd/MM/yyyy");
                string timeStart = startDate.ToShortTimeString();
                string timeEnd = beforeExtend.ToShortTimeString();
                string extendTime = endDate.ToShortTimeString();
                string intime = timeStart;
                string totime = timeEnd;
                string outTime = timeEnd;
                string badgeno = dt.Tables[0].Rows[0].ItemArray[11].ToString();
                string city = dt.Tables[0].Rows[0].ItemArray[12].ToString();
                string facility = dt.Tables[0].Rows[0].ItemArray[13].ToString();
                values = name + "|" + company + "|" + fromdate + "|" + todate + "|" + intime + "|" + totime + "|" + outTime + "|" + badgeno + "|" + city + "|" + facility + "|" + extendTime;
                string strPath = HttpContext.Current.Server.MapPath(string.Empty);
                string templateFileName = string.Concat(strPath, System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.TIMEEXTENDEDTEMPLATE].ToString());
                string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                string strSubjectLine = VMSConstants.VMSConstants.ALERTNOTIFICATION;
                //// strEmailMessage.Append(Body);
                strEmailMessage = strEmailMessage + body;
                //// Send mail 
                VMSUtility.SendEmail(hostEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
            }

            /// <summary>
            /// send cancel mail to host
            /// </summary>
            /// <param name="requestID">request Id value</param>
            /// <param name="hostEmailID">host email Id</param>
            /// <param name="visitorMasterObj">visitor master object</param>
            /// <param name="visitorLocObj">visitor location object</param>
            /// <param name="loginID">login Id</param>
            /// <param name="startDate">start date</param>
            /// <param name="endDate">end date</param>
            /// <param name="dtlocationDetails">location details</param>
            public void SendCancelMailtoHost(
                int requestID,
                string hostEmailID,
                VisitorMaster visitorMasterObj,
                VisitorRequest visitorLocObj,
                string loginID,
                DateTime startDate,
                DateTime endDate,
                DataTable dtlocationDetails)
            {
                string strEmailMessage = null;
                try
                {
                    if (ConfigurationManager.AppSettings["Mailtohostaftercancel_enable"].ToString() == "true")
                    {
                        ////if (VisitorLocObj.BadgeStatus==VMSConstants.VMSConstants.BadgeIssued && VisitorLocObj.RequestID>0)
                        {
                            ////added for mail additions
                            TemplateParser htmlFile = new TemplateParser();
                            string strHost = string.Empty;
                            string strVisitor = string.Empty;
                            string strCompany = string.Empty;
                            string strRequestID = string.Empty;
                            string strInDate = string.Empty;
                            string strInTime = string.Empty;
                            string strBadgeNo = string.Empty;
                            string strOutDate = string.Empty;
                            string strOutTime = string.Empty;

                            ////ADD-9-feb2010
                            string strcity = string.Empty;
                            string strfacility = string.Empty;

                            string strSubjectLine = string.Empty;
                            string strFromAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.FROMADDRESS].ToString();
                            string strSMTPHostAddress = System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.HOSTADD].ToString();
                            int ismtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PORT]);
                            string tokens = VMSConstants.VMSConstants.TOKENFORCANCELREQUESTTOHOSTMAILTEMPLATE;
                            string values = string.Empty;

                            ////tknName|tknVisitor|tknCompany|tknReqID|tknInDate|tknInTime|tknBadgeNo|tknOutDate|tknOutTime

                            if (visitorLocObj.HostID != string.Empty && visitorLocObj.HostID != null)
                            {
                                strHost = visitorLocObj.HostID;
                            }

                            if (visitorMasterObj.FirstName != string.Empty && visitorMasterObj.FirstName != null)
                            {
                                strVisitor = visitorMasterObj.FirstName;
                            }

                            if (visitorMasterObj.LastName != string.Empty && visitorMasterObj.LastName != null)
                            {
                                strVisitor = strVisitor + "  " + visitorMasterObj.LastName;
                            }

                            if (visitorMasterObj.Company != string.Empty && visitorMasterObj.Company != null)
                            {
                                strCompany = visitorMasterObj.Company;
                            }

#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                            if (visitorLocObj.RequestID.ToString() != string.Empty && visitorLocObj.RequestID != null)
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
                            {
                                strRequestID = visitorLocObj.RequestID.ToString();
                            }

                            if (dtlocationDetails.Rows.Count > 0)
                            {
                                strcity = Convert.ToString(dtlocationDetails.Rows[0]["City"]);
                                strfacility = Convert.ToString(dtlocationDetails.Rows[0]["Facility"]);
                            }

                            if (startDate.ToString() != string.Empty)
                            {
                                strInDate = startDate.ToString("dd/MM/yyyy");
                            }

                            // strInDate = VisitorLocObj.FromDate.Value.ToString("dd/MM/yyyy");
                            if (startDate.ToString() != string.Empty)
                            {
                                strInTime = startDate.ToShortTimeString();
                            }

                            if (endDate.ToString() != string.Empty)
                            {
                                strOutDate = endDate.ToString("dd/MM/yyyy");
                            }

                            if (endDate.ToString() != string.Empty)
                            {
                                strOutTime = endDate.ToShortTimeString();
                            }

                            values = strHost + "|" + strVisitor + "|" + strCompany + "|" + strInDate + "|" + strInTime + "|" + strOutDate + "|" + strOutTime + "|" + strcity + "|" + strfacility;

                            try
                            {
                                string templateFileName = HttpContext.Current.Server.MapPath("~/MailTemplates/CancelRequest.htm");
                                string body = htmlFile.GetParsedHtmlFile(templateFileName, tokens, values).ToString();
                                strSubjectLine = VMSConstants.VMSConstants.SUBJECTLINECANCELLED;
                                //// strEmailMessage.Append(Body);
                                strEmailMessage = strEmailMessage + body;
                                ////Send mail 
                                VMSUtility.SendEmail(hostEmailID, strFromAddress, strSMTPHostAddress, ismtpPort, strSubjectLine, strEmailMessage.ToString(), string.Empty);
                            }
                            catch (Exception ex)
                            {
                                strEmailMessage = ex.Message;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    strEmailMessage = null;
                }
            }

            /// <summary>
            /// Host notification details
            /// </summary>
            /// <param name="strAssociateID">associate Id value</param>
            /// <param name="strInDate">In date value</param>
            /// <param name="strSecurityID">security Id value</param>
            public void HostNotificationDetails(string strAssociateID, string strInDate, string strSecurityID)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer.UserDetailsDL objUserDetailsDL = new VMSDataLayer.VMSDataLayer.UserDetailsDL();
                    objUserDetailsDL.HostNotificationDetails(strAssociateID, strInDate, strSecurityID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ////public void SendReminderMail(string strFacility, string strCity, string strAssoName, string strInDate, string strInTime, string strAssociateMailID, string strAssociateID, string strSecurityID,string country)

            /// <summary>
            /// Send remainder log
            /// </summary>
            /// <param name="strInDate">In date value</param>
            /// <param name="strInTime">In time value</param>
            /// <param name="strAssociateID">associate Id value</param>
            /// <param name="strSecurityID">security Id value</param>
            /// <param name="passDetailId">pass detail Id</param>
            public void SendReminderLog(string strInDate, string strInTime, string strAssociateID, string strSecurityID, string passDetailId)
            {
                try
                {
                    VMSDataLayer.EmployeeDL objEmployeeDL = new VMSDataLayer.EmployeeDL();
                    objEmployeeDL.ReminderDetails(strAssociateID, strInDate, strInTime, strSecurityID, passDetailId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Display information
            /// </summary>
            /// <param name="requestID">request id value</param>
            /// <returns>properties value</returns>
            public VMSDataLayer.VMSDataLayer.PropertiesDC DisplayInfo(int requestID)
            {
                VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                try
                {
                    propertiesDC = this.vmsMasterDataDL.Displaydetails(requestID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return propertiesDC;
            }

            /// <summary>
            /// Get visit details by request Id
            /// </summary>
            /// <param name="requestID">request Id value</param>
            /// <returns>visit details array</returns>
            public List<VisitDetail> GetVisitDetailsByRequestID(string requestID)
            {
                List<VisitDetail> visitDetailArray = new List<VisitDetail>();
                try
                {
                    visitDetailArray = this.vmsMasterDataDL.GetVisitDetailsByRequestID(Convert.ToInt32(requestID));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return visitDetailArray;
            }

            /// <summary>
            /// Get search details
            /// </summary>
            /// <param name="visitorID">visitor Id value</param>
            /// <returns>properties DC value</returns>
            public VMSDataLayer.VMSDataLayer.PropertiesDC GetSearchDetails(int visitorID)
            {
                VMSDataLayer.VMSDataLayer.PropertiesDC propertiesDC = new VMSDataLayer.VMSDataLayer.PropertiesDC();
                try
                {
                    propertiesDC = this.vmsMasterDataDL.GetSearchDetails(visitorID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return propertiesDC;
            }

            ////added by laksmi on aug 19 2009 9'0 clock

            /// <summary>
            /// Cancel request
            /// </summary>
            /// <param name="requestID">request Id</param>
            /// <returns>returns success value</returns>
            public int? CancelRequest(int requestID)
            {
                int? success = 1;
                try
                {
                    ////objValidation = new Validations();
                    ////strErrorMessage = objValidation.RequestsPageValidations(VisitorProof, VisitorMasterObj, VisitorRequestObj, VisitorEmergencyContactObj);
                    ////if (strErrorMessage != string.Empty)
                    ////  throw new VMSBL.CustomException(strErrorMessage);
                    success = this.vmsMasterDataDL.CancelRequest(requestID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return success;
            }
            ////end

            /// <summary>
            /// Cancel request
            /// </summary>
            /// <param name="requestid">request Id</param>
            /// <param name="visitorid">visitor id</param>
            /// <param name="visitorname">visitor name</param>
            /// <param name="accesscardnumber">access card number</param>
            /// <param name="vcardnumber">visitor card number</param>
            public void UpdateCardDetails(int requestid, int visitorid, string visitorname, int accesscardnumber, string vcardnumber)
            {
                try
                {
                    this.vmsMasterDataDL.UpdateCardDetails(requestid, visitorid, visitorname, accesscardnumber, vcardnumber);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Cancel request
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
            public void ReissuecardsBL(string visitorname, string accessnumber, string vcardnumber,
                string requestid, string visitorid, string parentid, string reissuedby, string recollectedby,
                string reissuereason, string selectedfacility)
            {
                try
                {
                    this.vmsMasterDataDL.Reissuecards(visitorname, accessnumber, vcardnumber, requestid, visitorid, parentid, reissuedby, recollectedby, reissuereason, selectedfacility);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Cancel request
            /// </summary>
            /// <param name="accesscardnumber">access card number</param>
            /// <returns>access card usage</returns>
            public DataTable Checkaccesscardusage(int accesscardnumber)
            {
                DataTable accessscardusage = new DataTable();
                try
                {
                    accessscardusage = this.vmsMasterDataDL.Checkaccesscardusage(accesscardnumber);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return accessscardusage;
            }

            /// <summary>
            /// Cancel request
            /// </summary>
            /// <param name="securityid">security id</param>
            /// <returns>access card usage</returns>
            public DataTable LoadfacilitylistBL(int securityid)
            {
                DataTable facilitylistload = new DataTable();
                try
                {
                    facilitylistload = this.vmsMasterDataDL.Loadfacilitylist(securityid);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return facilitylistload;
            }

            /// <summary>
            /// Cancel request
            /// </summary>
            /// <returns>reissue reason list</returns>
            public DataTable LoadReissuelistBL()
            {
                DataTable reissuelistload = new DataTable();
                try
                {
                    reissuelistload = this.vmsMasterDataDL.LoadReissuelistDL();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return reissuelistload;
            }

            /// <summary>
            /// Edit visitor information
            /// </summary>
            /// <param name="visitorProof">visitor proof</param>
            /// <param name="visitorMasterObj">visitor master object</param>
            /// <param name="visitorRequestObj">visitor request object</param>
            /// <param name="visitDetailObj">visit detail object</param>
            /// <param name="visitorEquipmentObj">visitor equipment object</param>
            /// <param name="visitorEmergencyContactObj">visitor emergency contact object</param>
            /// <param name="equipcustody">equipment custody</param>
            /// <param name="identityDetails">identity details</param>
            /// <returns>return success value</returns>
            public int? EditVisitorInformation(
                VisitorProof visitorProof,
                VisitorMaster visitorMasterObj,
                VisitorRequest visitorRequestObj,
                VisitDetail[] visitDetailObj,
                VisitorEquipment[] visitorEquipmentObj,
                VisitorEmergencyContact visitorEmergencyContactObj,
                int equipcustody,
                IdentityDetails identityDetails)
            {
                int success = 0;

                try
                {
                    ////12/9/09
                    if (!visitorRequestObj.Status.Equals("Saved"))
                    {
                        ////END
                        this.objValidation = new Validations();
                        ////Changes done for VMS CR VMS06072010CR09 by Priti
                        this.StrErrorMessage = this.objValidation.RequestsPageValidations(visitorProof, visitorMasterObj, visitorRequestObj, visitorEmergencyContactObj, visitorEquipmentObj);
                        string[] strError = this.StrErrorMessage.Split('%');
                        if (!string.IsNullOrEmpty(strError[0]) || !string.IsNullOrEmpty(strError[1]) || !string.IsNullOrEmpty(strError[2]) || !string.IsNullOrEmpty(strError[3]))
                        {
                            throw new VMSBL.CustomException(this.StrErrorMessage);
                        }
                    }

                    int visitorID = visitorMasterObj.VisitorID;
                    success = this.vmsMasterDataDL.EditVisitorInformation(visitorProof, visitorMasterObj, visitorRequestObj, visitDetailObj, visitorEquipmentObj, visitorEmergencyContactObj, equipcustody);
                    if (visitDetailObj != null && visitorRequestObj.PermitITEquipments == true)
                    {
                        this.vmsMasterDataDL.InsertEquipment(visitDetailObj, visitorEquipmentObj);
                    }
                    ////Update Identity details
                    this.vmsMasterDataDL.UpdateIdentityDetails(identityDetails, visitorID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return success;
            }

            /// <summary>
            /// Added by PRITI on 3rd June for VMS CR VMS31052010CR6
            /// used for changing status of request for repeat visitor.
            /// </summary>
            /// <param name="visitorProof">visitor proof value</param>
            /// <param name="visitorMasterObj">visitor master object</param>
            /// <param name="visitorRequestObj">visitor request object</param>
            /// <param name="visitDetail">visit detail</param>
            /// <param name="visitorEquipmentObj">visitor equipment object</param>
            /// <param name="visitorEmergencyContactObj">visitor emergency contact object</param>
            /// <param name="equipcustody">equipment custody</param>
            /// <returns>return success value</returns>
            public int? EditVisitorInformationForRepeatVisitor(
                VisitorProof visitorProof,
                VisitorMaster visitorMasterObj,
                VisitorRequest visitorRequestObj,
                VisitDetail[] visitDetail,
                VisitorEquipment[] visitorEquipmentObj,
                VisitorEmergencyContact visitorEmergencyContactObj,
                int equipcustody)
            {
                int success = 0;

                try
                {
                    ////12/9/09
                    if (!visitorRequestObj.Status.Equals("Saved"))
                    {
                        ////END
                        this.objValidation = new Validations();
                        ////Changes done for VMS CR VMS06072010CR09 by Priti
                        this.StrErrorMessage = this.objValidation.RequestsPageValidations(visitorProof, visitorMasterObj, visitorRequestObj, visitorEmergencyContactObj, visitorEquipmentObj);
                        string[] strError = this.StrErrorMessage.Split('%');
                        if (!string.IsNullOrEmpty(strError[0]))
                        {
                            throw new VMSBL.CustomException(this.StrErrorMessage);
                        }
                    }

                    success = this.vmsMasterDataDL.EditVisitorInformation(visitorProof, visitorMasterObj, visitorRequestObj, visitDetail, visitorEquipmentObj, visitorEmergencyContactObj, equipcustody);
                    if (visitDetail != null && visitorRequestObj.PermitITEquipments == true)
                    {
                        this.vmsMasterDataDL.InsertEquipment(visitDetail, visitorEquipmentObj);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return success;
            }

            #region GetImageFilesAndStoreInDB

            /// <summary>
            /// Method to store the photo image in Database
            /// </summary>
            /// <param name="strImgPath">Image path value</param>
            /// <param name="requestID">Request Id value</param>
            /// <returns>get image files and store in DB</returns>
            public bool GetImageFilesAndStoreInDB(string strImgPath, int requestID)
            {
                try
                {
                    string strBinaryDataImg;
                    string strEncryptedBinaryData;
                    bool blnSuccess = false;

                    if (!strImgPath.ToUpper().Contains("ASSOCIATEIMAGE"))
                    {
                        strBinaryDataImg = this.ReadFile(strImgPath);
                        strEncryptedBinaryData = this.EncrpytBinaryData(strBinaryDataImg);
                    }
                    else
                    {
                        strEncryptedBinaryData = this.EncrpytBinaryData(strImgPath);
                    }

                    if (strEncryptedBinaryData != null && strEncryptedBinaryData != string.Empty)
                    {
                        blnSuccess = this.vmsMasterDataDL.UpdateUserImgInDB(requestID, strEncryptedBinaryData);
                    }

                    return blnSuccess;
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

            ////Delete/viewphoto

            /// <summary>
            /// Delete photo
            /// </summary>
            /// <param name="associateID">associate Id value</param>
            /// <returns>returns success value</returns>
            public bool DeletePhoto(string associateID)
            {
                try
                {
                    VMSDataLayer.EmployeeDL objEmployeeDL = new VMSDataLayer.EmployeeDL();
                    bool blnSuccess;
                    blnSuccess = objEmployeeDL.DeletePhoto(associateID);
                    return blnSuccess;
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

            /// <summary>
            /// Check Visitor Card Info
            /// </summary>
            /// <param name="visitDetailsId"></param>
            /// <returns>DataTable</returns>
            public DataTable GetVisitorDetailsforMailProcess(int visitDetailsId, string action)

            {
                try
                {
                    VMSDataLayer.VMSDataLayer objEmployeeDL = new VMSDataLayer.VMSDataLayer();
                    return objEmployeeDL.GetVisitorDetailsforMailProcess(visitDetailsId, action);
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


            ////597397
            /// <summary>
            /// 
            /// </summary>
            /// <param name="visitDetailsId"></param>
            /// 
            /// <returns></returns>
            public DataTable GetClientDetailsforInfoCard(int RequestId, int VisitorId, int ParentId)
            {
                try
                {
                    VMSDataLayer.VMSDataLayer objEmployeeDL = new VMSDataLayer.VMSDataLayer();
                    return objEmployeeDL.GetClientDetailsforInfoCard(RequestId, VisitorId, ParentId);
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

            ////597397

            #region ReadFile

            /// <summary>
            /// Method to read the image file
            /// </summary>
            /// <param name="strImagePath">image path string</param>
            /// <returns>string Binary Data</returns>
            private string ReadFile(string strImagePath)
            {
                try
                {
                    FileInfo fileinfo = new FileInfo(strImagePath);
                    string strBinaryData = string.Empty;
                    byte[] dbytes = null;
                    long numBytes = fileinfo.Length;
                    FileStream fstream = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fstream);
                    dbytes = br.ReadBytes((int)numBytes);
                    strBinaryData = Encoding.Default.GetString(dbytes);
                    br.Close();
                    fstream.Close();
                    return strBinaryData;
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

            #region EncryptData
            /// <summary>
            /// Method to EncryptData
            /// Input string Binary Data Image
            /// </summary>
            /// <param name="strBinaryDataImg">binary data image value</param>
            /// <returns>Binary Data string</returns>
            private string EncrpytBinaryData(string strBinaryDataImg)
            {
                return new EncryptDecrypt().Encrypt(strBinaryDataImg, "CTS", true);
            }
            #endregion
        }

        /// <summary>
        /// Visit details class
        /// </summary>
        public class VisitDetails
        {
            /// <summary>
            /// Gets or sets Visitor name
            /// </summary>
            public string VisitorName { get; set; }

            /// <summary>
            /// Gets or sets Organization
            /// </summary>
            public string Organization { get; set; }

            /// <summary>
            /// Gets or sets Visit date
            /// </summary>
            public string VisitDate { get; set; }

            /// <summary>
            /// Gets or sets From time
            /// </summary>
            public string FromTime { get; set; }

            /// <summary>
            /// Gets or sets To Time
            /// </summary>
            public string ToTime { get; set; }
        }

        #region Validations
        ////Changes done for VMS CR VMS06072010CR09 by PRITI

        /// <summary>
        /// Requests page validation
        /// </summary>
        public class Validations
        {
            /// <summary>
            /// Requests page validation
            /// </summary>
            /// <param name="visitorProofObj">visitor proof object</param>
            /// <param name="visitorMaster">visitor master</param>
            /// <param name="visitorRequest">visitor request</param>
            /// <param name="visitorEmergencyContact">visitor emergency contact</param>
            /// <param name="visitorEquipment">visitor equipment</param>
            /// <returns>validation data</returns>
            public string RequestsPageValidations(VisitorProof visitorProofObj, VisitorMaster visitorMaster, VisitorRequest visitorRequest, VisitorEmergencyContact visitorEmergencyContact, VisitorEquipment[] visitorEquipment)
            {
                StringBuilder strError = new StringBuilder();
                StringBuilder strGeneralError = new StringBuilder();
                StringBuilder strReqError = new StringBuilder();
                StringBuilder strContactinfoError = new StringBuilder();
                StringBuilder strEquipmentError = new StringBuilder();
                try
                {
                    // string title = VisitorMaster.Title;
                    string firstName = visitorMaster.FirstName;
                    string lastName = visitorMaster.LastName;
                    string company = visitorMaster.Company;
                    string emailID = visitorMaster.EmailID;
                    string nativeCountry = visitorMaster.Country;
                    string mobileNumber = visitorMaster.MobileNo;

                    string purpose = visitorRequest.Purpose;
                    DateTime fromDate = visitorRequest.FromDate.Value;
                    DateTime todate = visitorRequest.ToDate.Value;
                    TimeSpan fromTime = visitorRequest.FromTime.Value;
                    TimeSpan totime = visitorRequest.ToTime.Value;

                    if (this.CheckDropDownValue(purpose))
                    {
                        strReqError.Append(VMSConstants.VMSConstants.BRMESSAGE);
                        strReqError.AppendLine(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.PURPOSEEMPTYERROR].ToString());
                    }

                    if ((string.Equals(visitorRequest.Status.ToUpper().Trim(), "SUBMITTED")) || (string.Equals(visitorRequest.Status.ToUpper().Trim(), "UPDATED")))
                    {
                        if (this.CheckDate(fromDate, todate))
                        {
                            strReqError.Append(VMSConstants.VMSConstants.BRMESSAGE);
                            strReqError.AppendLine(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.DATEERROR].ToString());
                        }

                        if (this.CheckDateDuration(fromDate, todate))
                        {
                            strReqError.Append(VMSConstants.VMSConstants.BRMESSAGE);
                            strReqError.AppendLine(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.DATEDURATIONERROR].ToString());
                        }

                        if (this.CheckTimewithCurrentTime(fromTime, totime, fromDate, todate))
                        {
                            strReqError.Append(VMSConstants.VMSConstants.BRMESSAGE);
                            strReqError.AppendLine(System.Configuration.ConfigurationManager.AppSettings[VMSConstants.VMSConstants.CURRENTTIMEERROR].ToString());
                        }
                    }

                    strError = strGeneralError.Append("%").Append(strReqError).Append("%").Append(strContactinfoError).Append("%").Append(strEquipmentError);
                    return strError.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// used to check if a value is entered.
            /// </summary>
            /// <param name="strValue">value string value</param>
            /// <returns>returns if empty or not</returns>
            public bool CheckEmptyValue(string strValue)
            {
                bool isEmpty = false;
                if (string.IsNullOrEmpty(strValue))
                {
                    isEmpty = true;
                }

                return isEmpty;
            }

            /// <summary>
            /// used to check non numeric for names
            /// </summary>
            /// <param name="strValue">string value</param>
            /// <returns>returns if match or not</returns>
            public bool CheckNumericAndSpecialCharacters(string strValue)
            {
                bool isnotMatch = false;

                string strRegex = "^[a-zA-Z]+((\\ [a-zA-Z])?[a-zA-Z]*)*$";

                Regex re = new Regex(strRegex);
                if (re.IsMatch(strValue))
                {
                    isnotMatch = false;
                }
                else
                {
                    isnotMatch = true;
                }

                return isnotMatch;
            }

            /// <summary>
            /// used to check Email Format
            /// </summary>
            /// <param name="strValue">string value</param>
            /// <returns>check if match or not</returns>
            public bool CheckEmailFormat(string strValue)
            {
                bool isnotMatch = false;
                if (string.IsNullOrEmpty(strValue))
                {
                    return isnotMatch;
                }
                else
                {
                    string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                          @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    string atsign = strValue.Substring(0, strValue.LastIndexOf("@"));
                    string domain = strValue.Substring(atsign.Length, strValue.Length - atsign.Length);
                    Regex re = new Regex(strRegex);
                    if (domain.ToUpper().ToString() == "@COGNIZANT.COM")
                    {
                        isnotMatch = true;
                    }
                    else
                    {
                        isnotMatch = false;
                    }

                    return isnotMatch;
                }
            }

            // commented by lakshmi on Aug 14 2009

            ///// <summary>
            ///// used to check Mobile Number has preceeded with country code
            ///// </summary>
            ///// <param name="strValue"></param>
            ///// <returns></returns>
            ////public bool CheckMobileNumberHasCountryCode(string strValue)
            ////{

            ////    bool isnotMatch = false;

            ////    string strRegex = "^\\d{10}";

            ////    if (strValue != string.Empty)
            ////    {
            ////        Regex re = new Regex(strRegex);
            ////        if (re.IsMatch(strValue))
            ////            isnotMatch = false;
            ////        else
            ////            isnotMatch = true;
            ////    }

            ////    return isnotMatch;

            ////}

            ////End

            //// Added by lakshmi on Aug 14 2009 - start

            /// <summary>
            /// used to check Mobile Number has preceded with country code
            /// </summary>
            /// <param name="strValue">string value</param>
            /// <returns>to check mobile number format</returns>
            public bool CheckMobileNumberFormat(string strValue)
            {
                bool isnotMatch = false;
                ////string strRegex = "^\\d{4}d{10}";

                ////30/8/09
                ////string strRegex = "^\\d{4}-\\d{10}$";
                ////end
                string strRegex = "^\\d{10}$";

                if (!string.IsNullOrEmpty(strValue))
                {
                    Regex re = new Regex(strRegex);
                    if (re.IsMatch(strValue))
                    {
                        isnotMatch = false;
                    }
                    else
                    {
                        isnotMatch = true;
                    }
                }

                return isnotMatch;
            }

            ////End

            /// <summary>
            /// used to Validate From Date and To Date
            /// </summary>
            /// <param name="fromDate">from date</param>
            /// <param name="todate">to date</param>
            /// <returns>to check date</returns>
            public bool CheckDate(DateTime fromDate, DateTime todate)
            {
                bool isnotMatch = false;

                GenericTimeZone genTimeZone = new GenericTimeZone();
                DateTime dt = genTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;

                if ((fromDate <= todate) && (fromDate >= todaysDate) && (todate >= todaysDate))
                {
                    isnotMatch = false;
                }
                else
                {
                    isnotMatch = true;
                }

                return isnotMatch;
            }

            /// <summary>
            /// added for VMS CR VMS06072010CR09 by PRITI
            /// added to check duplication for equipment.
            /// </summary>
            /// <param name="visitorEquipment">visitor equipment value</param>
            /// <returns>to check duplication value</returns>
            public bool CheckEquipment(VisitorEquipment[] visitorEquipment)
            {
                DataSet ds = new DataSet();
                bool isDuplicateEquipment = false;
                for (int rowCount = 0; rowCount < visitorEquipment.Count(); rowCount++)
                {
                    int rowCountForCheck = visitorEquipment.Count() - 1;
                    while (rowCountForCheck > rowCount)
                    {
                        if (visitorEquipment[rowCount].MasterDataID == visitorEquipment[rowCountForCheck].MasterDataID)
                        {
                            if (visitorEquipment[rowCount].Make.Equals(visitorEquipment[rowCountForCheck].Make) && visitorEquipment[rowCount].SerialNo.Equals(visitorEquipment[rowCountForCheck].SerialNo))
                            {
                                isDuplicateEquipment = true;
                            }
                        }

                        rowCountForCheck--;
                    }
                }

                return isDuplicateEquipment;
            }

            /// <summary>
            /// check date duration
            /// </summary>
            /// <param name="fromDate">from date value</param>
            /// <param name="todate">to date value</param>
            /// <returns>check if match or not</returns>
            public bool CheckDateDuration(DateTime fromDate, DateTime todate)
            {
                bool isnotMatch = false;

                GenericTimeZone genTimeZone = new GenericTimeZone();
                DateTime dt = genTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;

                if ((todate - fromDate).TotalDays + 1 <= 32)
                {
                    isnotMatch = false;
                }
                else
                {
                    isnotMatch = true;
                }

                return isnotMatch;
            }

            /// <summary>
            /// used to Validate From Time and To ToTime
            /// </summary>
            /// <param name="fromTime">from time</param>
            /// <param name="totime">to time</param>
            /// <param name="fromDate">from date</param>
            /// <param name="todate">to date</param>
            /// <returns>if match or not value</returns>
            public bool CheckTime(TimeSpan fromTime, TimeSpan totime, DateTime fromDate, DateTime todate)
            {
                bool isnotMatch = false;
                GenericTimeZone genTimeZone = new GenericTimeZone();
                DateTime dt = genTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;
                TimeSpan todaysTime = dt.TimeOfDay;

                if (fromTime <= totime)
                {
                    isnotMatch = false;
                }
                else
                {
                    isnotMatch = true;
                }

                return isnotMatch;
            }

            /// <summary>
            /// check time
            /// </summary>
            /// <param name="fromTime">from time</param>
            /// <param name="totime">to time</param>
            /// <returns>check if match or not</returns>
            public bool CheckTime(DateTime fromTime, DateTime totime)
            {
                bool isnotMatch = false;
                GenericTimeZone genTimeZone = new GenericTimeZone();
                DateTime dt = genTimeZone.GetCurrentDate();
                DateTime todaysDate = dt.Date;
                TimeSpan todaysTime = dt.TimeOfDay;

                if (DateTime.Compare(totime, fromTime) < 0)
                {
                    if (TimeSpan.Compare(fromTime.TimeOfDay, totime.TimeOfDay) <= 0)
                    {
                        isnotMatch = true;
                    }
                    else
                    {
                        isnotMatch = false;
                    }
                }
                else
                {
                    isnotMatch = true;
                }

                return isnotMatch;
            }

            /// <summary>
            /// check time with current time
            /// </summary>
            /// <param name="fromTime">from time</param>
            /// <param name="totime">to time</param>
            /// <param name="fromDate">from date</param>
            /// <param name="todate">to date</param>
            /// <returns>check if match or not</returns>
            public bool CheckTimewithCurrentTime(TimeSpan fromTime, TimeSpan totime, DateTime fromDate, DateTime todate)
            {
                bool isnotMatch = false;
                GenericTimeZone genTimeZone = new GenericTimeZone();

                ////24/09/2009
                DateTime currentdate = genTimeZone.GetCurrentDate();
                DateTime dt = currentdate.AddMinutes(-10);

                ////end
                DateTime todaysDate = dt.Date;
                TimeSpan todaysTime = dt.TimeOfDay;
                ////    TimeSpan tsFrom = TimeSpan.Parse(VMSUtility.VMSUtility.GetTimeToISTZone(fromTime.ToString()));
                if (fromDate == todaysDate)
                {
                    if (fromTime >= todaysTime && fromDate == todaysDate)
                    {
                        isnotMatch = false;
                    }
                    else
                    {
                        isnotMatch = true;
                    }
                }

                return isnotMatch;
            }

            /// <summary>
            /// used to Validate dropdown
            /// </summary>
            /// <param name="ddlValue">DDL value</param>
            /// <returns>to check drop down value</returns>
            public bool CheckDropDownValue(string ddlValue)
            {
                bool hasNotSelected = false;

                ////Modified the if statement on Aug 18 2009 - start
                ////if (ddlValue.Equals("Select") || ddlValue.Equals("Select Purpose") || ddlValue.Equals("4"))
                if (ddlValue.Equals("Select") || ddlValue.Equals("Select Purpose") || ddlValue.Equals("9") || string.IsNullOrEmpty(ddlValue))
                {
                    hasNotSelected = true;
                }
                else
                {
                    hasNotSelected = false;
                }

                return hasNotSelected;
            }

            ////Vms_Host_upload PHYS19042010CR02

            /// <summary>
            /// check Id proof value
            /// </summary>
            /// <param name="ddlValue">DDL value</param>
            /// <param name="proofimage">proof image  value</param>
            /// <returns>check if selected or not</returns>
            public bool CheckIDproofvalue(string ddlValue, string proofimage)
            {
                bool hasNotSelected = false;
                if (Convert.ToInt32(ddlValue) == 4)
                {
                    if (proofimage != null)
                    {
                        hasNotSelected = true;
                    }
                }
                else if (Convert.ToInt32(ddlValue) != 4)
                {
                    if (proofimage == null)
                    {
                        hasNotSelected = true;
                    }
                }
                else
                {
                    hasNotSelected = false;
                }

                return hasNotSelected;
            }

            /// <summary>
            /// check Id proof value validation
            /// </summary>
            /// <param name="ddlValue">DDL value</param>
            /// <param name="proofimage">proof image</param>
            /// <returns>returns if selected or not</returns>
            public bool CheckIDproofvalueValidation1(string ddlValue, string proofimage)
            {
                bool hasNotSelected = false;
                if (Convert.ToInt32(ddlValue) == 4)
                {
                    if (proofimage != null)
                    {
                        hasNotSelected = true;
                    }
                }
                else
                {
                    hasNotSelected = false;
                }

                return hasNotSelected;
            }

            /// <summary>
            /// check Id proof value validation
            /// </summary>
            /// <param name="ddlValue">DDL value</param>
            /// <param name="proofimage">proof image</param>
            /// <returns>check if selected or not</returns>
            public bool CheckIDproofvalueValidation2(string ddlValue, string proofimage)
            {
                bool hasNotSelected = false;

                if (Convert.ToInt32(ddlValue) != 4)
                {
                    if (proofimage == null)
                    {
                        hasNotSelected = true;
                    }
                }
                else
                {
                    hasNotSelected = false;
                }

                return hasNotSelected;
            }
            ////end PHYS19042010CR02
        }

        #endregion

        #region EncryptDecrypt

        /// <summary>
        /// Encrypt Decrypt class
        /// </summary>
        public class EncryptDecrypt
        {
            /// <summary>
            /// To encrypt data
            /// </summary>
            /// <param name="toencrypt">to be encrypted value</param>
            /// <param name="key">key value</param>
            /// <param name="useHashing">hash value parameter</param>
            /// <returns>encrypted value</returns>
            public string Encrypt(string toencrypt, string key, bool useHashing)
            {
                try
                {
                    byte[] keyArray = null;
                    byte[] toencryptArray = UTF8Encoding.UTF8.GetBytes(toencrypt);

                    if (useHashing)
                    {
                        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    }
                    else
                    {
                        keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    }

                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform ctransform = tdes.CreateEncryptor();
                    byte[] resultArray = ctransform.TransformFinalBlock(toencryptArray, 0, toencryptArray.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                {
                }

                return null;
            }

            /// <summary>
            /// To decrypt value
            /// </summary>
            /// <param name="todecrypt">to be decrypted value</param>
            /// <param name="key">key value</param>
            /// <param name="useHashing">hash value parameter</param>
            /// <returns>decrypted value</returns>
            public string Decrypt(string todecrypt, string key, bool useHashing)
            {
                try
                {
                    byte[] keyArray = null;
                    byte[] toencryptArray = Convert.FromBase64String(todecrypt);

                    if (useHashing)
                    {
                        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    }
                    else
                    {
                        keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    }

                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform ctransform = tdes.CreateDecryptor();
                    byte[] resultArray = ctransform.TransformFinalBlock(toencryptArray, 0, toencryptArray.Length);

                    return UTF8Encoding.UTF8.GetString(resultArray);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ////return null;
            }
        }

        #endregion
        /// <summary>
        /// Check Vcard status
        /// </summary>
        /// <param name="vCard"></param>
        /// <returns>status</returns>
        public string CheckCardStatus(string vCard)
        {
            try
            {
                VMSDataLayer.VMSDataLayer objEmployeeDL = new VMSDataLayer.VMSDataLayer();
                return objEmployeeDL.CheckCardStatus(vCard);
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

        /// <summary>
        /// Check Visitor Card Info
        /// </summary>
        /// <param name="visitDetailsId"></param>
        /// <returns>DataTable</returns>
        public DataTable GetVisitorCardInfo(string visitDetailsId)
        {
            try
            {
                VMSDataLayer.VMSDataLayer objEmployeeDL = new VMSDataLayer.VMSDataLayer();
                return objEmployeeDL.GetVisitorCardInfo(visitDetailsId);
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
        /// <summary>
        /// Get CardLog Details
        /// </summary>
        /// <param name="visitDetailsId"></param>
        /// <returns>cardlog Details</returns>
        public DataTable GetCardLog(string visitDetailsId)
        {
            try
            {
                VMSDataLayer.VMSDataLayer objEmployeeDL = new VMSDataLayer.VMSDataLayer();
                return objEmployeeDL.GetCardLog(visitDetailsId);
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
        /// <summary>
        /// Get Visitor Schedule
        /// </summary>
        /// <param name="visitDetailsId"></param>
        /// <returns></returns>
        public DataTable GetVisitorSchedule(string visitDetailsId)
        {
            try
            {
                VMSDataLayer.VMSDataLayer objEmployeeDL = new VMSDataLayer.VMSDataLayer();
                return objEmployeeDL.GetVisitorSchedule(visitDetailsId);
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
        /// <summary>
        /// Function to Log Exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="method">method</param>
        /// <param name="stackTrace">stack trace</param>
        public void LogException(string message, string method, string stackTrace)
        {
            try
            {
                VMSDataLayer.VMSDataLayer objExceptionLogger = new VMSDataLayer.VMSDataLayer();
                objExceptionLogger.LogException(message, method, stackTrace);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
        }


    }
}
