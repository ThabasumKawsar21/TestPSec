<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientVisit.aspx.cs" Inherits="VMSDev.ClientVisit" MasterPageFile="~/VMSMain.Master" %>

<%@ Register Src="~/UserControls/ClientVisit.ascx" TagName="ClientVisit" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
   
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="Scripts/common.js"></script>
    <script language="javascript" type="text/javascript">

        function GetOffset() {
            setDefaultDate
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            ClientVisit.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            ClientVisit.AssignCurrentDateTime(today);
            var r = document.getElementById('<%=btnHidden.ClientID %>');
            r.click();
        }
        function LostRequestMail() {

            var r = $('#<%=btnCheckOutRequest.ClientID %>');
            r.click();
        }
        function setDefaultDate() {
            var now = new Date();
            var formatDate = now.format("dd/MM/yyyy");
            $('#<%=hdnCurrentDate.ClientID %>').val(formatDate);
            var currenthour = now.getHours();
            var currentminute = now.getMinutes();
            var fromtime = "00" + ":" + "01";
            $('#<%=hdnCurrentTime.ClientID %>').val(fromtime);
        }
    </script>

    <div>
        <table align="center" width="97%">
            <tr>
                <td>
                    <asp:Button ID="btnHidden" runat="server" Style="display: none" Text="Button" OnClick="BtnHidden_Click" />
                    <asp:Button ID="btnCheckOutRequest" runat="server" Text="Button" Style="display: none"
                        OnClick="BtnCheckOutRequest_Click" />
                    <asp:HiddenField ID="hdnCurrentDate" runat="server" />
                    <asp:HiddenField ID="hdnCurrentTime" runat="server" />
                    <uc3:ClientVisit ID="ViewLogbySecurity1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
