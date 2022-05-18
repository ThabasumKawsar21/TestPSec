<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgBox.ascx.cs" Inherits="VMSDev.UserControls.MsgBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<style type="text/css">
    .mdlWindow
    {
        border: 1px solid #c0c0c0;
        background: #f0f0f0;
        padding: 0px 0px 0px 0px;
        width: 400px;
        height: auto;
        font-family: Arial;
        font-size: small;
    }
    .mdlWindow span
    {
        font-family: Arial;
        font-size: small;
    }
    .mdlBckgrnd
    {
        background-color: #878776;
        filter: alpha(opacity=40);
        opacity: 0.5;
    }
    p.MsoNormal
    {
        margin-top: 1.3pt;
        margin-right: 5.75pt;
        margin-bottom: 12.0pt;
        margin-left: 0in;
        line-height: 12.0pt;
        font-size: 10.0pt;
        font-family: "Arial" , "sans-serif";
    }
    .table_header_bg
    {
    }
</style>
<asp:Panel ID="panMessageBox" runat="server" CssClass="mdlWindow" Style="display: none">
    <table width="100%">
        <tr>
            <td colspan="2" align="left">
                <asp:Label ID="lblTitle" runat="server" Style="font-weight: bold" Text="<%$ Resources:LocalizedText, lblInformation %>" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="middle" height="60px" width="60px">
                <center>
                    <asp:Image ID="imgMsgIcon" Height="33px" Width="33px" Style="margin: 7px 7px 7px 7px"
                        oncontextmenu="return false" runat="server" ImageUrl="~/Images/info.png" />
                </center>
            </td>
            <td>
                <asp:Label ID="MsgBody" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnConfirm" runat="server" Text="<%$ Resources:LocalizedText, Ok %>"  BackColor="#767561" Font-Bold="False"
                    Font-Size="11px" ForeColor="White" Height="21px" Style="min-width: 50px; padding: 1px 10px 0px 0px;
                    text-align: center" />
                <%-- &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Go Back" BackColor="#767561"
                    Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" style="min-width:50px" />--%>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button ID="btnhidden" runat="server" Text="" Style="display: none" />
<cc1:ModalPopupExtender ID="mpeConfirm" runat="server" TargetControlID="btnhidden"
    DropShadow="true" X="-1" Y="-1" PopupControlID="panMessageBox" BackgroundCssClass="mdlBckgrnd"
    CancelControlID="btnCancel" PopupDragHandleControlID="panMessageBox">
</cc1:ModalPopupExtender>
