<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/VMSMain.Master" CodeBehind="ErrorPage.aspx.cs" Inherits="VMSDev.ErrorPage" %>

 

<asp:Content ID="Content1" ContentPlaceHolderID="VMSContentPlaceHolder" Runat="Server">
    <div>
<table width="60%" height="60%" align="center">
        <tr valign="middle" align="center">
            <td>
                <table width="100%" class="innerTable" cellspacing="0" cellpadding="0">
                    <tr>
                        <th class="innerTableHead" height="22" align="left">
                            Error
                        </th>
                    </tr>
                    <tr>
                        <td style="height: 70px">
                            <table cellpadding="10" cellspacing="1">
                                <tr>
                                <td>
                                    <img src="Images/error_icon.jpg" height="30" />
                                </td>
                                    <td>
                                        <span id="spnInfoMessage" class="messageLabel"><asp:Label Id="lblError" CssClass="messageLabel" Text="<%$ Resources:LocalizedText, ErrorMessage %>" runat="server"></asp:Label>
                                       </span>
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
 
