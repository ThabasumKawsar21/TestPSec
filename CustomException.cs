

namespace VMSBL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #region "Input Validation Exception"

    /// <summary>
    /// customer exception class
    /// </summary>
    public class CustomException : SystemException
    {
        /// <summary>
        /// error message string
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException" /> class.
        /// </summary>
        public CustomException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException" /> class.
        /// </summary>
        /// <param name="errorMessage">error message</param>
        public CustomException(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }        

        /// <summary>
        /// Gets or sets error message
        /// </summary>
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }        
    }
    #endregion
}
