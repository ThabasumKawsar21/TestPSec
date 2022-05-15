
namespace VMSBusinessEntity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To hold Contract details
    /// </summary>
    public class ContractDetails
    {
        /// <summary>
        /// Gets or sets the ContractorId property
        /// </summary>        
        public string ContractorId { get; set; }

        /// <summary>
        /// Gets or sets the ContractorName property
        /// </summary>        
        public string ContractorName { get; set; }

        /// <summary>
        /// Gets or sets the VendorName property
        /// </summary>        
        public string VendorName { get; set; }

        /// <summary>
        /// Gets or sets the Supervisor Mobile property
        /// </summary>        
        public string SupervisiorMobile { get; set; }

        /// <summary>
        /// Gets or sets the VendorPhoneNumber property
        /// </summary>        
        public string VendorPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the FacilityAddress property
        /// </summary>        
        public string FacilityAddress { get; set; }

        /// <summary>
        /// Gets or sets the Card Validity property
        /// </summary>        
        public string CardValidlity { get; set; }

        /// <summary>
        /// Gets or sets the CardValid property
        /// </summary>        
        public string CardValid { get; set; }

        /// <summary>
        /// Gets or sets the ContractorNumber property
        /// </summary>        
        public string ContractorNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contractorIdentityNo property
        /// </summary>        
        public string ContractorIdentityNo { get; set; }
    }
}
