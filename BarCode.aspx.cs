
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Linq;
    using System.Web;
    using System.Web.SessionState;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The BarCode partial class
    /// </summary>
    public partial class BarCode : System.Web.UI.Page
    {
        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Get the Requested code to be created.
                string code = Request.QueryString["code"].ToString();

                // Multiply the lenght of the code by 40 (just to have enough width)
                int w = code.Length * 80;

                // Create a bitmap object of the width that we calculated and height of 100
                Bitmap obitmap = new Bitmap(w, 80);

                // then create a Graphic object for the bitmap we just created.
                Graphics ographics = Graphics.FromImage(obitmap);

                // Now create a Font object for the Barcode Font
                // (in this case the IDAutomationHC39M) of 18 point size

                ////Font oFont = new Font("3 of 9 Barcode", 100);
                string typeFaceName = System.Configuration.ConfigurationManager.
                AppSettings["TypeFaceName"].ToString();
                PrivateFontCollection fnts = new PrivateFontCollection();
                fnts.AddFontFile(Server.MapPath(System.Configuration.ConfigurationManager.
                AppSettings["Font"].ToString()));
                FontFamily fntfam = new FontFamily(typeFaceName, fnts);
                Font ofont = new Font(fntfam, 100);

                // Let's create the Point and Brushes for the barcode
                PointF opoint = new PointF(2f, 2f);
                SolidBrush obrushWrite = new SolidBrush(Color.Black);
                SolidBrush obrush = new SolidBrush(Color.White);

                // Now lets create the actual barcode image
                // with a rectangle filled with white color
                ographics.FillRectangle(obrush, 0, 0, w, 80);

                // We have to put prefix and sufix of an asterisk (*),
                // in order to be a valid barcode
                ographics.DrawString(code, ofont, obrushWrite, opoint);

                // Then we send the Graphics with the actual barcode
                Response.ContentType = "image/jpeg";                
                obitmap.Save(Response.OutputStream, ImageFormat.Jpeg);
            }
            catch
            {
                try
                {
                    string TargetPage = "Images/Blank.png";
                    Response.Redirect(TargetPage, true);
                    
                }
                catch (System.Threading.ThreadAbortException ex)
                {

                }
            }
        }
    }
}
