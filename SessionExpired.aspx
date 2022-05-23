<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionExpired.aspx.cs"
    Inherits="VMSDev.SessionExpired" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="App_Themes/StandardUI.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/UIStyle.css" rel="stylesheet" type="text/css" />
    <title>PhysicalSecurity</title>
    <%-- <script language="javascript" type="text/javascript">
 window.onload = disablebackbutton();
  function disablebackbutton() {
 window.history.clear();

                }

         window.history.clear();
         </script>--%>
</head>
<body>
    <form id="frmsessionexpired" method="post" runat="server">
    <center>
        <table height="30%" align="center">
        </table>
        <table width="75%" align="center">
            <tr align="center">
                <td valign="middle">
                    <table width="75%" class="innerTable" cellspacing="0" cellpadding="0" style="vertical-align: middle;
                        height: 100%;">
                        <tr>
                            <th class="innerTableHead" height="22" align="left">
                                <asp:Label ID="lblApplicationName" runat="server" CssClass="Table_header" Text="PhysicalSecurity"></asp:Label>
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
                                            <span id="spnInfoMessage" class="messageLabel">Session Expired, please <a id="Loginurl"
                                                runat="server" href="">try </a>again</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
