
namespace VMSBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// User details class
    /// </summary>
    public class UserDetailsBL
    {
        #region GetUserType
        /// <summary>
        /// Method to get RoleId of a User Id
        /// </summary>
        /// <param name="strUserId">Login User Id</param>
        /// <returns>Returns RoleId</returns>
        public int GetUserType(string strUserId)
        {
            int iroleId = 0;
            try
            {
                VMSDataLayer.UserDetailsDL objUserDetailsDL = new VMSDataLayer.UserDetailsDL();
                iroleId = objUserDetailsDL.GetUserType(strUserId);
                return iroleId;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            catch (System.NullReferenceException ex)
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
}
