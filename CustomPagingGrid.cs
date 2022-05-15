
namespace VMSFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// DataPagerGridView is a custom control that implements Grid View.
    /// </summary>
    public class CustomPagingGrid : GridView, IPageableItemContainer
    {
        /// <summary>
        /// TotalRowCountAvailable event key
        /// </summary>
        private static readonly object EventTotalRowCountAvailable = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPagingGrid"/> class.
        /// </summary>
        public CustomPagingGrid()
            : base()
        {
            PagerSettings.Visible = false;
        }

        /// <summary>
        /// event for total row count available
        /// </summary>
[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Reviewed")]
        event EventHandler<PageEventArgs> IPageableItemContainer.TotalRowCountAvailable
        {
            add { Events.AddHandler(CustomPagingGrid.EventTotalRowCountAvailable, value); }
            remove { Events.RemoveHandler(CustomPagingGrid.EventTotalRowCountAvailable, value); }
        }

        /// <summary>
        /// Gets StartRowIndex = PageSize * PageIndex properties
        /// </summary>
[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Reviewed")]
        int IPageableItemContainer.StartRowIndex
        {
            get { return this.PageSize * this.PageIndex; }
        }

        /// <summary>
        /// Gets MaximumRows  = PageSize property
        /// </summary>
[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Reviewed")]
        int IPageableItemContainer.MaximumRows
        {
            get { return this.PageSize; }
        }

        /// <summary>
        /// Set the control with appropriate parameters and bind to right chunk of data.
        /// </summary>
        /// <param name="startRowIndex">Starting value of row index</param>
        /// <param name="maximumRows">Maximum rows allowed</param>
        /// <param name="databind">boolean value data bind</param>
[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Reviewed")]
        void IPageableItemContainer.SetPageProperties(int startRowIndex, int maximumRows, bool databind)
        {
            int newPageIndex = startRowIndex / maximumRows;
            this.PageSize = maximumRows;

            if (this.PageIndex != newPageIndex)
            {
                bool isCanceled = false;
                if (databind)
                {
                    // create the event arguments and raise the event
                    GridViewPageEventArgs args = new GridViewPageEventArgs(newPageIndex);
                    this.OnPageIndexChanging(args);

                    isCanceled = args.Cancel;
                    newPageIndex = args.NewPageIndex;
                }

                // if the event wasn't cancelled change the paging values
                if (!isCanceled)
                {
                    this.PageIndex = newPageIndex;

                    if (databind)
                    {
                        this.OnPageIndexChanged(EventArgs.Empty);
                    }
                }

                if (databind)
                {
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// Call base control's CreateChildControls method and determine the number of rows in the source 
        /// then fire off the event with the derived data and then we return the original result.
        /// </summary>
        /// <param name="dataSource">accepts data source</param>
        /// <param name="dataBinding">accepts data binding</param>
        /// <returns>returns rows</returns>
        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            int rows = base.CreateChildControls(dataSource, dataBinding);

            // if the paging feature is enabled, determine the total number of rows in the datasource
            if (this.AllowPaging)
            {
                // if we are databinding, use the number of rows that were created, otherwise cast the datasource to an Collection and use that as the count
                int totalRowCount = dataBinding ? rows : ((ICollection)dataSource).Count;

                // raise the row count available event
                IPageableItemContainer pageableItemContainer = this as IPageableItemContainer;
                this.OnTotalRowCountAvailable(new PageEventArgs(pageableItemContainer.StartRowIndex, pageableItemContainer.MaximumRows, totalRowCount));

                // make sure the top and bottom pager rows are not visible
                if (this.TopPagerRow != null)
                {
                    this.TopPagerRow.Visible = false;
                }

                if (this.BottomPagerRow != null)
                {
                    this.BottomPagerRow.Visible = false;
                }
            }

            return rows;
        }

        /// <summary>
        /// on total row count available
        /// </summary>
        /// <param name="e">total row count available</param>
        protected virtual void OnTotalRowCountAvailable(PageEventArgs e)
        {
            EventHandler<PageEventArgs> handler = (EventHandler<PageEventArgs>)Events[CustomPagingGrid.EventTotalRowCountAvailable];
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
