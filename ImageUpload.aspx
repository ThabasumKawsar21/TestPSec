<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageUpload.aspx.cs" Inherits="VMSDev.ImageUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<base target="_self" />  
<%--<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />--%> 
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" /> 
<title>Photo/Proof Image upload</title>
<link href="App_Themes/UIStyle.css" rel="stylesheet" type="text/css" />  
 <script language="javascript" type="text/javascript">
//     function refreshParent() {
//                    
//           window.parent.document.forms[0].submit();      
//           window.returnValue = true;
     //       }
//     function CloseParentWindow() {
//         window.parent.location.href = window.parent.location.href;
//         // return false;
//     }
     </script>
  </head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">              
            <tr>
             <td colspan="4" align="center" valign="middle" style="height: 100%">
             <table width="100%" class="border" style="height: 98%"  >                     
                 <tr>
              
                     <td align="left" class="table_header_bg" style="height: 19px">
                     <%--<asp:imagebutton id="ImgCancelUpload" 
                            alternatetext="Close" runat="server" imagealign="Right" imageurl="~/Images/Close.png"
                            oncontextmenu="return false" style="position: relative; height:14px; width:14px;" />--%>
                    </td>
                 </tr>
                 <tr>
                     <td align="center"  colspan="2">
                         <asp:Panel ID="panelEmp" runat="server" Width="100%" CssClass="border"  >
                         <table id ="tblEmp" cellpadding="0" cellspacing="0" width ="100%">
                             <tr>
                                 <td align="left" colspan="2" class="field_column_bg">
                                     <asp:Label ID="lblModuleValue0" runat="server" CssClass="lblHeada" 
                                        Text="<%$ Resources:LocalizedText, UploadPhoto %>"  nowrap=nowrap></asp:Label></td>
                                
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg">
                                     <asp:Label ID="lblEmployeeImage" runat="server" CssClass="lblField" 
                                         Text="<%$ Resources:LocalizedText, VisitorImage %>"></asp:Label>
                                     &nbsp
                                     :</td>                                    
                                 <td align="left" class="field_column_bg">
                                 &nbsp
                                     <asp:FileUpload ID="ImgUpload" runat="server" />
                                 </td>
                             </tr>                       
                             <tr>
                                 <td  class="field_column_bg">
                                 </td>
                                 <td align="left" class="field_column_bg">
                                 &nbsp
                                     <asp:Button ID="btnSave" runat="server" CssClass="cssButton" OnClick="BtnSave_Click" 
                                         Text="<%$ Resources:LocalizedText, Upload %>" Width="71px"  />
                                 </td>
                             </tr>
                         
             </table>            
           </asp:Panel>
           </td>
                 </tr>
            </table>
   
 <table border="0" cellpadding="0" cellspacing="0" width="98%">       
     <tr>
           <td colspan="6" align="center" valign="middle">              
           <br />
             <table border="0" id="errortbl" cellpadding="0" cellspacing ="0" runat="server" 
             style="border-right: black 1pt solid; border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid; width: 60%;" >
             <tr>             
             <td colspan="6"  align="left" style="background-color: #edf6fd; width: 60%">
            
                           <asp:Label ID="lblMessage" runat="server" CssClass="lblField" ForeColor="Red"></asp:Label>                               
                           
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
    <%--<asp:Image ID="UImage" runat="server" Height="140px" 
        ImageUrl="~/Images/char.jpeg" oncontextmenu="return false" Visible="true" 
        Width="120px" />--%>
    </form>
</body>
</html>
