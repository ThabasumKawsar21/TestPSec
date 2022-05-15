
namespace VMSDataContract
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using VMSBusinessEntity;

    /// <summary>
    /// The DataContract class
    /// </summary>    
[SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "Reviewed")]
    public class DataContract
    {
        /// <summary>
        /// The GeneralInformationDataContract class
        /// </summary>        
[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Reviewed")]
        public class GeneralInformationDataContract
        {
        ////#region private variables
        //    string _firstName;
        //    string _lastName;
        //    string _title;

        ////#endregion
           
           /// <summary>
           /// The VisitorMasterData field
           /// </summary>           
           private VisitorMaster visitorMasterData;

        #region public properties

            /// <summary>
            /// Gets or sets the VisitorMaster property
            /// </summary>            
            public VMSBusinessEntity.VisitorMaster VisitorMaster
            {
                get
                {
                    return this.visitorMasterData;
                }

                set
                {
                     this.visitorMasterData  = value;
                }
            }

            ////public string LastName
            ////{
            //    get
            //    {
            //        return _lastName;
            //    }
            //    set
            //    {
            //        _lastName = value;
            //    }

            ////}

            ////public string Title
            ////{
            //    get
            //    {
            //        return _title;
            //    }
            //    set
            //    {
            //        _title = value;
            //    }

            ////}
           
#endregion
        }
    }
}
