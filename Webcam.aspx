<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/Webcam.aspx.cs" Inherits="VMSDev.Webcam" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="App_Themes/UIStyle.css" rel="stylesheet" type="text/css" />
<link href="App_Themes/StandardUI.css" rel="stylesheet" type="text/css" />
<script runat="server">
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <base target="_self" />
    <title>Capture Image</title>

    <style type="text/css">
        html, body
        {
            height: 100%;
            overflow: auto;
        }

        body
        {
            padding: 0;
            margin: 0;
        }

        #silverlightControlHost
        {
            height: 100%;
            text-align: center;
        }
    </style>
</head>
<body style="background-color: Black">
    <form id="form1" runat="server">        
        <asp:HiddenField ID="hdnfldImage" runat="server" />
        <div id="silverlightControlHost" style="position: absolute; top: 0px; left: 0px; height: 350px; width: 800px; z-index: 2">

           <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="800px" height="350px" id="SlvObj">
                <param name="source" id="source" value="ClientBin/SilverlightApplication1.xap" />
                <param name="onError" value="onSilverlightError" />
                <param name="background" value="white" />
                <param name="minRuntimeVersion" value="4.0.50401.0" />
                <param name="autoUpgrade" value="true" />                
            </object>
            <%--<iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px; border: 0px" runat="server" src=""></iframe>--%>


            <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
         <asp:Button runat="server" ID="Upload" Text="<%$ Resources:LocalizedText, UploadAndClose %>"
             OnClick="Upload_Click" class="cssButton" Style="border-color: White" />
        </div>

    </form>
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript" ></script>
        <script type="text/javascript" src="Silverlight.js"></script>
   
        <script type="text/javascript" >

        
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>

    <script type="text/javascript">

        function WebCamImageFetching(imgdata) {
            debugger;
           
            $('#<%=hdnfldImage.ClientID %>').val(imgdata);

        }


</script>
    <%--<script type="text/javascript">

        window.onunload = refreshParent;
        function refreshParent() {
            window.opener.document.forms(0).submit();            
        }
    </script>--%>
</body>
</html>
