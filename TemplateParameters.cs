// -----------------------------------------------------------------------
// <copyright file="TemplateParameters.cs" company="CTS">
// Copyright (c) . All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace BusinessManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To define Template Parameters
    /// </summary>
    [Serializable]
    public class TemplateParameters
    {
        /// <summary>
        /// Gets or sets  Host Id
        /// </summary>
        public string EmailBodyText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets  Host Id
        /// </summary>
        public string AssociateId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Host First Name
        /// </summary>
        public string AssociateName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Facility Address
        /// </summary>
        public string FacilityAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Check out Time
        /// </summary>
        public string CheckoutTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Updated By
        /// </summary>
        public string UpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Role
        /// </summary>
        public string Role
        {
            get;
            set;
        }
    }
}
