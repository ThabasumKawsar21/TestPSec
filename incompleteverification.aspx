<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="incompleteverification.aspx.cs" Inherits="VMSDev.Incompleteverification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">


<base target="_self" /> 
    <title>Incomplete verification - confirmation</title>
    <style type="text/css">




.field_txt {
	font-family: Arial;
	font-size: 11px;
	font-style: normal;
	font-weight: normal;
	font-variant: normal;
	color: #00000;
	text-decoration: none;
}

    </style>
    
    
</head>
<body bgcolor="#c5c5c5" >
    <form id="form1" runat="server"   >
    <div>
    
    </div>
    <p> 
                                     <asp:Label ID="lblwarningmessage" runat="server" 
            CssClass="field_txt" Font-Bold="True" Font-Size="Small"
                                         ForeColor="Black" Text= "Laptop verification process is incomplete in another facility. Click continue to check out"></asp:Label>
                                    
    </p>
    <p>
                     <table width=100%>
                     <tr>
                     <td align=center>
                     
                     <asp:Button ID="btnSave" runat="server" BackColor="#767561" Font-Bold="False"
                             ForeColor="White"  OnClick="BtnSave_Click" 
            Text="Continue" Font-Size="10px" 
                                         Height="21px" Width="71px"  />
                         &nbsp;<asp:Button ID="btnClear" runat="server" BackColor="#767561" 
            Font-Bold="False" Font-Size="10px"
                             ForeColor="White"  OnClick="BtnClear_Click" Text="Cancel" 
            Height="21px" Width="71px"/>
            </td>
            </tr>
            </table>
    </p>
    </form>
</body>
</html>
