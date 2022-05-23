<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="VMSDev.Success" MasterPageFile="~/VMSMain.Master" %>

<asp:Content ID="VMSEnterInformationContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <div style="overflow: auto; height: 100%; width: 100%; vertical-align: top;">
                            
    <table width="60%" height="60%" align="center">
        <tr valign="middle" align="center">
            <td>
                <table width="100%" class="innerTable" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table cellpadding="10" cellspacing="1">
                                <tr>
                                    <td>
                                      <span id="ctl00_ContentPlaceHolder1_labelMessage" class="messageLabel">
                                       <asp:Label ID="lblMessage" runat="server"></asp:Label></span>
                                    </td>
                                </tr>                          
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div> 
</asp:Content>
            
