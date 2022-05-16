//-----------------------------------------------------------------------
// <copyright file="Calender.aspx.cs" company="MyCompanyName">
//     Copyright (c) MyCompanyName. All rights reserved.
// </copyright>
// <summary>
// This file contains Calender class.
// </summary>
//-----------------------------------------------------------------------
namespace VMSDev.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using XSS = CAS.Security.Application.EncodeHelper;

    /// <summary>
    /// Details Page
    /// </summary>
    public partial class Calender : System.Web.UI.Page
    {
        #region Page Load
        
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 12; i++)
            {
                this.CboMonth.Items[i].Text = new DateTime(1, i + 1, 1).ToString("MMMM"); 
                ////MMMM Month Format
            }

            if (!Page.IsPostBack)
            {
                ////Adding the value to Combobox. [1900- thisYear]
                for (int iyear = 2009; iyear <= DateTime.Now.Year; iyear++)
                {
                    this.CboYear.Items.Add(new ListItem(iyear.ToString())); 

                    ////System.Math.Min(System.Threading.Interlocked.Increment(ref iYear), iYear - 1);
                } 

                this.RefreshControls();
                ////Array.Copy(arr, arr.GetLength(1)*4, arrB, 0, arr.GetLength(1));
            }
        }
        #endregion

        #region Private Methods
       
        #endregion
        #region Page Events

        /// <summary>
        /// The Select Date Click method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void HlSelectDate_Click(object sender, EventArgs e)
        {
            try
            {
                string returnDate = this.CdrDatePicker.SelectedDate.ToString("MM/dd/yyyy");
                this.ReturnToPage(returnDate);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        
        /// <summary>
        /// The Date Picker Selection Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void CdrDatePicker_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string retDate = this.CdrDatePicker.SelectedDate.ToString("MM/dd/yyyy");
                this.ReturnToPage(retDate);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        
        /// <summary>
        /// The Month Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void CboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.SetCalendarDate();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        
        /// <summary>
        /// The Year Selected Index Changed method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void CboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.SetCalendarDate();
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The returnToPage method
        /// </summary>
        /// <param name="retDate">The retDate parameter</param>        
        private void ReturnToPage(string retDate)
        {
            ////"<script>window.opener." + Request.QueryString("field") 
            ////+ ".value ='" + ReturnDate + "';" + " " + "window.close();</script>")
            Response.Write(
                "<script>window.opener." 
                + XSS.HtmlEncode(Request.QueryString["field"].ToString()) 
                + ".value='" + retDate + "';window.close();</script>");
            ////Response.Write("<script>alert(window.opener." + Request.QueryString["field"].ToString() + ".value);</script>");
            ////Console.WriteLine(Request.QueryString["field"].ToString());
            ////Response.Write("<script>alert('1dd');</script>");
        }

        /// <summary>
        /// Method to set Date Calendar
        /// </summary>
        private void SetCalendarDate()
        {
            try
            {
                int day;
                int year = int.Parse(this.CboYear.SelectedValue.ToString());
                int month = int.Parse(this.CboMonth.SelectedValue.ToString());
                day = this.CdrDatePicker.SelectedDate.Day;
                if (day > DateTime.DaysInMonth(year, month))
                {
                    day = DateTime.DaysInMonth(year, month);
                }

                this.CdrDatePicker.VisibleDate = new DateTime(year, month, day);
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// Method to refresh controls
        /// </summary>
        private void RefreshControls()
        {
            try
            {
                DateTime selectedDate;
                if (!(Request.QueryString["ShowDate"] == null))
                {
                    selectedDate = System.Convert.ToDateTime(Request.QueryString["ShowDate"]);
                    this.CboYear.SelectedIndex = selectedDate.Year - 1900;
                    this.CboMonth.SelectedIndex = selectedDate.Month - 1;
                    this.CdrDatePicker.VisibleDate = selectedDate.Date;
                }
                else
                {
                    {
                        ////CboYear.SelectedIndex = DateTime.Now.Year - 1900;
                    }

                    this.CboYear.SelectedIndex = this.CboYear.Items.IndexOf(
                        this.CboYear.Items.FindByText(DateTime.Now.Year.ToString()));
                    this.CboMonth.SelectedIndex = DateTime.Now.Month - 1;
                    this.CdrDatePicker.VisibleDate = DateTime.Now.Date;
                }
            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }
        #endregion
    }
}
