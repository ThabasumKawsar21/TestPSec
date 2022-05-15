// -----------------------------------------------------------------------
// <copyright file="TemplateParser.cs" company="CTS">
// Copyright (c) . All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VMSBusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Template parser class
    /// </summary>
    public class TemplateParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateParser" /> class.
        /// </summary>
        public TemplateParser()
        {
            // TODO: Add constructor logic here
        }

        /// <summary>
        /// get the email message from the given template
        /// </summary>
        /// <param name="strTemplateFileName">template file name</param>
        /// <param name="strTokens">token value</param>
        /// <param name="strValues">string values</param>
        /// <returns>gets parsed HTML file</returns>
        public string GetParsedHtmlFile(string strTemplateFileName, string strTokens, string strValues)
        {
            try
            {
                StringBuilder fileContents = new StringBuilder();

                ////Retrieve content of the Template HTML file
                fileContents = fileContents.Append(this.GetContentOfTemplateFile(strTemplateFileName));
                string[] tokens = strTokens.Split(Convert.ToChar("|"));
                string[] values = strValues.Split(Convert.ToChar("|"));
                ////First retrieve the file, then replace the Tokens by their respective Values
                ////Iterate over the Token array retrieving the array boundaries with "GetLowerBound" and "GetUpperBound"
                ////Then replace each token with its respective value for each Token element in the array

                for (int tokenCounter = tokens.GetLowerBound(0); tokenCounter <= tokens.GetUpperBound(0); tokenCounter++)
                {
                    fileContents = fileContents.Replace(tokens[tokenCounter], values[tokenCounter]);
                }

                ////cast the contents of the StringBuilder class to a String
                return fileContents.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get the content of the given template
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>gets content of template file</returns>
        private string GetContentOfTemplateFile(string fileName)
        {
            try
            {
                ////Retrieve the content of the HTML file using a StreamReader class
                StreamReader streamReaderHtmlFile;
                string contentOfFile;
                streamReaderHtmlFile = File.OpenText(HttpContext.Current.Server.MapPath(fileName));
                contentOfFile = streamReaderHtmlFile.ReadToEnd();
                streamReaderHtmlFile.Close();
                return contentOfFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
