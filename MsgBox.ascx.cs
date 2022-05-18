//---------------------------------------------------------------------------------------
// <copyright file="MsgBox.ascx.cs" company="Cognizant Technology Solution">
//     Copyright(c) . All rights reserved. 
// </copyright>
//---------------------------------------------------------------------------------------
namespace VMSDev.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The Box Type 
    /// </summary> 
    public partial class MsgBox : System.Web.UI.UserControl
    {
        /// <summary>
        /// The Box Type enumeration
        /// </summary>        
        public enum MsgBoxType
        {
            /// <summary>
            /// The Information enumeration
            /// </summary> 
            Information,

            /// <summary>
            /// The Error enumeration
            /// </summary> 
            Error,

            /// <summary>
            /// The Warning enumeration
            /// </summary> 
            Warning
        }

        ////public enum ButtonType
        ////{ 
        //    OK,
        //    OKCancel,
        //    YesNo,
        //    Custom,
        //    None
        ////}

        /// <summary>
        /// The Show method
        /// </summary>
        /// <param name="strMessage">The Message parameter</param>        
        public void Show(string strMessage)
        {
            this.MsgBody.Text = strMessage;
            this.mpeConfirm.Show();
        }

        /// <summary>
        /// The Show method
        /// </summary>
        /// <param name="alertType">The Alert Type parameter</param>
        /// <param name="strMessage">The Message parameter</param>        
        public void Show(MsgBoxType alertType, string strMessage)
        {
            // SetButtons("OK");
            this.SetAlertType(alertType);
            this.MsgBody.Text = strMessage;
            this.mpeConfirm.Show();
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        ////public void Show(MsgBoxType AlertType, string strMessage, string Button1)
        ////{
        //    if (string.IsNullOrEmpty(Button1))
        //    {
        //        SetButtons("OK");
        //    }
        //    else
        //    {
        //        SetButtons(Button1, Button2);
        //    }

        ////    SetAlertType(AlertType);
        //    MsgBody.Text = strMessage;
        //    mpeConfirm.Show();
        ////}

        /// <summary>
        /// The SetAlertType method
        /// </summary>
        /// <param name="alertType">The AlertType parameter</param>        
        private void SetAlertType(MsgBoxType alertType)
        {
            switch (alertType)
            {
                case MsgBoxType.Information:
                    {
                        this.imgMsgIcon.ImageUrl = "~/Images/info.png";
                        break;
                    }

                case MsgBoxType.Error:
                    {
                        this.imgMsgIcon.ImageUrl = "~/Images/error.png";
                        break;
                    }

                case MsgBoxType.Warning:
                    {
                        this.imgMsgIcon.ImageUrl = "~/Images/warning.png";
                        break;
                    }

                default:
                    {
                        this.imgMsgIcon.ImageUrl = "~/Images/info.png";
                        break;
                    }
            }
        }

        /// <summary>
        /// The SetButtons method
        /// </summary>
        /// <param name="btn">The button parameter</param>        
        private void SetButtons(string btn)
        {
            this.btnConfirm.Text = btn;
            this.btnConfirm.Visible = true;
            ////btnCancel.Visible = false;
        }

        ////private void SetButtons(string btn1, string btn2)
        ////{
        //    if (!string.IsNullOrEmpty(btn1))
        //    {
        //        btnConfirm.Text = btn1;
        //        //btnConfirm.Visible = true;
        //    }
        //    else
        //    {
        //        btnConfirm.Text = "OK";
        //    }
        //    if (!string.IsNullOrEmpty(btn2))
        //    {
        //        btnCancel.Text = btn2;
        //        //btnConfirm.Visible = true;
        //    }
        //    else
        //    {
        //        btnCancel.Text = "Cancel";
        //        //btnConfirm.Visible = false;
        //    }
        ////}
    }    
}
