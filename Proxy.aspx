<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Proxy.aspx.cs" Inherits="Proxy.Proxy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="0" cellspacing="5" cellpadding="5">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Proxy Login ID"></asp:Label> 
                    </td>
                    <td>
                        <asp:TextBox ID="txtProxyID" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table> 
        </div>
    </form>
</body>
</html>
