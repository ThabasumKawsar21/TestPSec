

namespace BusinessManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To define Transaction Parameters
    /// </summary>
    public class TransactionParameters
    {
        /// <summary>
        /// Gets or sets  RequestId
        /// </summary>
        public string RequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets  CCList
        /// </summary>
        public string CcList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets  RequestorId
        /// </summary>
        public string RequestorId
        {
            get;
            set;
        }
    }
}
