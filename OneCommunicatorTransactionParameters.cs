

namespace BusinessManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To define OneCommunicatorTransactionParameters
    /// </summary>
    [Serializable]
    public class OneCommunicatorTransactionParameters
    {
        /// <summary>
        /// Gets or sets Recipients
        /// </summary>
        public string Recipients
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets RequestId
        /// </summary>
        public string RequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Process
        /// </summary>
        public string Process
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets GlobalAppId
        /// </summary>
        public string GlobalAppId
        {
            get;
            set;
        }
    }
}
