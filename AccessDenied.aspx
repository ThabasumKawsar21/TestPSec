<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="VMSDev.AccessDenied" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="App_Themes/StandardUI.css" rel="stylesheet" type="text/css" />
    <title>Physical Security</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="60%" align="center">
            <tr valign="middle" align="center">
                <td>
                    <table width="100%" class="innerTable" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="height:30px;"> 
                                <span id="spnInfoMessage" class="messageLabel">You don't have access to the Page. Please
                                    contact Physical Security Admin.</span>
                                 <%--   <span id="Span1" class="messageLabel">You don't have access to the Page. Please <a style="text-decoration: none" href="">
                                                Click Here</a>
                                    to raise the GSD to get the Access.</span>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
