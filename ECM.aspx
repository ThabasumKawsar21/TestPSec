<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ECM.aspx.cs" Inherits="VMSDev.ECM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <title></title>
</head>
<body>
    <script type="text/javascript">
        $(function () {
            
            TriggerHiddenButtonClick();
           
        });

        function TriggerHiddenButtonClick() {
            console.log("Inside click");
            var r = document.getElementById('<%=Button1.ClientID %>');
            r.click();
        }
       
    </script>
    <form id="ECMUploadForm" runat="server">
        <%--  <asp:Panel ID="Panel1" runat="server" >--%>
        <%--<div style="height:14px;border-left:3px solid red;margin-bottom:7px"><b><span style="font-size:13px;margin-left:3px">Upload Proof of Attachment</span></b></div>--%>
        <asp:Button ID="Button1" runat="server" Text="Click Here to Upload" style="display:none"
             />
        <asp:Label ID="testData" runat="server"></asp:Label>
        <asp:Label ID="contentId" runat="server"></asp:Label>
        <asp:HiddenField ID="_ecmMClientHdnField" runat="server" />
        <asp:HiddenField ID="FileName" runat="server" />
        <asp:HiddenField ID="ecmMHiddenField" runat="server" />
        <asp:HiddenField ID="_ecmContentIdList" runat="server" />
        <asp:HiddenField ID="originalFileName" runat="server" />
        <asp:HiddenField ID="FileContentId" runat="server" />
        <asp:HiddenField ID="FileAppDocId" runat="server" />
        <asp:HiddenField ID="EcmDid" runat="server" />
        <asp:HiddenField ID="UploadedImage" runat="server" />

        <%--    </asp:Panel>   --%>
    </form>



</body>
</html>
