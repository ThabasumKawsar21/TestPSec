<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    CodeBehind="ContractorSearch.aspx.cs" Inherits="VMSDev.ContractorSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>



<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">


<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
</head>
<style>
  div.image  
  {
      position:relative;
       width: 100%;
        /* for IE 6 */

border: 2px solid white;


        background: url(Images/CWR360.PNG);
        
}

</style>

<script language="javascript" type="text/javascript">
    function Opencwr360() {
        window.open("https://onecsit.cognizant.com/?globalappid=2365");
    }
    </script>
<body>

  <table align="center" border="0" cellpadding="0" cellspacing="0" width="924" height="200">
  <br />  <br />  <br />  <br />  <br />  <br />  <br />  <br />
    <tr>
      <td id="mainbg" valign="top">
    
<DIV class="image">

<%--<img src="Images/Capture.PNG" title="ss"/>--%>
<h4 style="color:#29D0F5"><center><asp:LinkButton ID="cwr360" OnClientClick="return Opencwr360()" 
                            CssClass="cont-btn" runat="server">
                            <span style="color:white"><u>Explore the Contractor DB in new Avatar as CWR 360 App</u></span>
                        </asp:LinkButton></center></h4>
                       
                        </DIV>

       </td>


    </tr>
</table>
</body>
</html>
</asp:Content>
