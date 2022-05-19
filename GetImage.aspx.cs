
namespace VMSDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// get image partial class
    /// </summary>
    public partial class GetImage : System.Web.UI.Page
    {
        /// <summary>
        /// The Image To Byte method
        /// </summary>
        /// <param name="img">The image parameter</param>
        /// <returns>The byte[] type object</returns>        
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        /// The Page_Load method
        /// </summary>
        /// <param name="sender">The sender parameter</param>
        /// <param name="e">The e parameter</param>        
        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection coll = Request.QueryString;
             string strMessage;
            //// Get names of all keys into a string array.
            string[] arr1 = coll.AllKeys;

            strMessage = "Query String Length and content" + arr1.Length;
            VMSUtility.VMSUtility.WriteLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);
          
            int width = 0;
            int height = 0;
            string fileContentId = string.Empty;
           
            width = Convert.ToInt32(coll.GetValues(1)[0].ToString());
            height = Convert.ToInt32(coll.GetValues(2)[0].ToString());
            if (arr1.Length == 4)
            {
                fileContentId = Convert.ToString(coll.GetValues(3)[0]);
            }

            strMessage 
                = "Get Image page  is called inside before switch for File Content Id" 
                + fileContentId 
                + " and Query String Value" 
                + arr1[0].ToString();
            VMSUtility.VMSUtility.WriteLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);                    
                        
            switch (arr1[0].ToString())
            {
                case "IDCARD":
                    {
                        strMessage 
                            = "San Storage is invoked inside switch File Content Id" 
                            + fileContentId;
                        VMSUtility.VMSUtility.WriteLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);
                        GenericFileUpload gnfileUpload = new GenericFileUpload();
                        byte[] data = gnfileUpload.GetAssociateImage(fileContentId);
                       //// drawimage(data, width, height);
                         this.ThumbNail(data, width, height);
                         strMessage 
                             = "San Storage service is completed inside switch File Content Id" 
                             + fileContentId;
                        VMSUtility.VMSUtility.WriteLog(
                            strMessage, 
                            VMSUtility.VMSUtility.LogLevel.Normal);
                    }

                    break;
                case "PATH":
                    {
                        strMessage = "IDCard Path is invoked inside switch";
                        VMSUtility.VMSUtility.WriteLog(strMessage, VMSUtility.VMSUtility.LogLevel.Normal);
                        System.Drawing.Image orgImage 
                            = System.Drawing.Image.FromFile(coll.GetValues(0)[0].ToString());
                        byte[] convertedImage = ImageToByte(orgImage);

                        System.Drawing.Image image = System.Drawing.Image.FromStream(
                            new System.IO.MemoryStream(convertedImage));
                        width = image.Width;
                        height = image.Height;                  
                        this.Drawimage(convertedImage, width, height);
                        orgImage.Dispose();
                        strMessage = "IDCard Path operation completed inside switch";
                        VMSUtility.VMSUtility.WriteLog(
                            strMessage, 
                            VMSUtility.VMSUtility.LogLevel.Normal);
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The draw image method
        /// </summary>
        /// <param name="data">The data parameter</param>
        /// <param name="width">The width parameter</param>
        /// <param name="height">The height parameter</param>        
        private void Drawimage(byte[] data, int width, int height)
        {
            try
            {
                if (data != null)
                {
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        Bitmap imgIn = (Bitmap)System.Drawing.Image.FromStream(ms);
                        double y = imgIn.Height;
                        double x = imgIn.Width;
                        double factor = 1;

                        if (width * height != 0)
                        {
                            if (width > 0)
                            {
                                factor = width / x;
                            }
                            else if (height > 0)
                            {
                                factor = height / y;
                            }
                        }

                        Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
                        Graphics g = Graphics.FromImage(imgOut);
                        g.Clear(Color.White);
                        g.DrawImage(
                            imgIn, 
                            new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), 
                            new Rectangle(0, 0, (int)x, (int)y), 
                            GraphicsUnit.Pixel);
                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        imgOut.Save(outStream, ImageFormat.Jpeg);
                        byte[] buffer = outStream.ToArray();
                        Response.ContentType = "image/jpeg";
                        Response.BinaryWrite((byte[])buffer);
                    }
                }
                else
                {
                        string Targetpage = "~\\Images\\DummyPhoto.png";
                        Response.Redirect(Targetpage, true);
                         
                    
                    
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
            catch (Exception ex)
            {
                Utility.VMSUtility.LogExceptionAndShowErrorPage(ex, HttpContext.Current);
            }
        }

        /// <summary>
        /// The DownLoad method
        /// </summary>
        /// <param name="data">The data parameter</param>
        /// <param name="width">The width parameter</param>
        /// <param name="height">The height parameter</param>        
        private void DownLoad(byte[] data, int width, int height)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    System.Drawing.Image newImage;
                    ms.Write(data, 0, data.Length);
                    newImage = System.Drawing.Image.FromStream(ms, true);
                    System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                    newImage.Save(outStream, ImageFormat.Jpeg);
                    byte[] buffer = outStream.ToArray();
                    Response.ContentType = "image/jpeg";
                    Response.BinaryWrite((byte[])buffer);
                }
            }
            catch (SystemException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The ThumbNail method
        /// </summary>
        /// <param name="data">The data parameter</param>
        /// <param name="width">The width parameter</param>
        /// <param name="height">The height parameter</param>        
        private void ThumbNail(byte[] data, int width, int height)
        {
            int imageSize = 220;
            using (MemoryStream ms = new MemoryStream(data, 0, data.Length))
            {
                Bitmap imgIn = (Bitmap)System.Drawing.Image.FromStream(ms);
                double y = imgIn.Height;
                double x = imgIn.Width;
                /////////////
                int thumbnailSize = imageSize;
                int newWidth, newHeight;

                if (imgIn.Width > imgIn.Height)
                {
                    newWidth = thumbnailSize;
                    newHeight = imgIn.Height * thumbnailSize / imgIn.Width;
                }
                else
                {
                    newWidth = imgIn.Width * thumbnailSize / imgIn.Height;
                    newHeight = thumbnailSize;
                }

                var thumbnailBitmap = new Bitmap(newWidth, newHeight);
                var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbnailGraph.DrawImage(imgIn, imageRectangle);

                System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                thumbnailBitmap.Save(outStream, ImageFormat.Jpeg);
                byte[] buffer = outStream.ToArray();
                Response.ContentType = "image/jpeg";
                Response.BinaryWrite((byte[])buffer);
            }
        }
    }
}
