
namespace BusinessManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// To define Email
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Gets or sets CC
        /// </summary>
        public string CC
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets  BCC
        /// </summary>
        public string BCC
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets  BCC
        /// </summary>
        public TemplateParameters TemplateParameters
        {
            get;
            set;
        }
    }
}
