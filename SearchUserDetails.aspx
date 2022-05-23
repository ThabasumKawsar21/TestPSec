<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchUserDetails.aspx.cs" Inherits="VMSDev.SearchUserDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<base target="_self">
<head runat="server">
    <title></title>

     <link href="App_Themes/UIStyle.css" rel="stylesheet" type="text/css" />
     
    <script language="javascript" type="text/javascript">
     // 
        function GetAssociateIdNameList(lblAssociateId, FromHost, PhoneNo) {           
               window.returnValue = lblAssociateId.innerText;
               window.returnValue = lblAssociateId.innerText + '%$%' + PhoneNo;
            window.close();
    }   
    function SpecialCharacterValidation(e)
        {
            var keycode;     
            if (window.event)    
             keycode = window.event.keyCode;        
            else if (event) keycode = event.keyCode; 
            else if (e) keycode = e.which;
            else return true;
            if(keycode > 47 && keycode <= 57) 
            {
                return true; 
            }
            else
            {
            return false; 
            }           
        }
     function ChkValidation() {

         var validationAssociate = '<%=Resources.LocalizedText.AssociateDetailsPopUpCaption%>'; 
         if((document.getElementById("txtUserId").value=="") && (document.getElementById("txtUserName").value==""))
         {
             alert(validationAssociate);
             return false;
         }
         else
         {
             return true;
         }
     }
     
     function ValidUser() {
         var validationAssociate = '<%=Resources.LocalizedText.AssociateDetailsPopUpCaption%>';
         alert(validationAssociate);
             return false;
         }
     function changeScreenSize(w,h) 
         {      
          alert('resize');
          window.resizeTo( w,h );
          alert('size');         
         }
               
    </script>
</head>

<body>
    <form id="form1" runat="server">
    <div>
    <h2 align="center" class="lblHeada"><asp:label Id="lblCaption" runat="server" Text="<%$ Resources:LocalizedText, AssociateDetailsPopUpCaption %>" ></asp:label> </h2>
    <table class="border" cellpadding="2" cellspacing="2" id="errortbl" width="98%" runat="server" align="center">
       <tr align="left">
             <th class="field_column_bg">
                  <asp:Label ID="lblUserID" CssClass="lblField"  runat="server" Text="<%$ Resources:LocalizedText, AssociateId %>" ></asp:Label>
             </th>
             <th class="field_column_bg">
                 <asp:Label ID="lblUserName" CssClass="lblField" runat="server" Text="<%$ Resources:LocalizedText, AssociateName %>"></asp:Label>
             </th>
        </tr>
        <tr align="left">
            <td>
               <asp:TextBox ID="txtUserId" runat="server" CssClass="txtField" Width="120px" ></asp:TextBox>
            </td>
            <td>
               <asp:TextBox ID="txtUserName" runat="server" CssClass="txtField" Width="120px"></asp:TextBox>&nbsp;
               <asp:Button ID="butSearch" runat="server"  Text="<%$ Resources:LocalizedText, Search %>"
                        onclick="ButSearch_Click" OnClientClick="return ChkValidation()"  CssClass="cssButton"/>         
            </td>
        </tr>
        <tr id="trError" visible="false" runat="server">
            <td colspan="2" align="center" style="background-color: #edf6fd; height: 19px;">
               <asp:Label ID="lblErrorMsg" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label> 
                 <asp:Label ID="lblclienthost" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label> 
            </td> 
        </tr>
        </table>
        <table id="Table1" class="border" cellpadding="2" cellspacing="2" runat="server" align="center">
            <tr><td colspan="2"><br /></td></tr>
            <tr align="left">
                 <td colspan="2">
                 <asp:Panel ID="pnlSearch" runat="server" ScrollBars="Vertical" Height="250" Width="350px">
                       <%--//Security not host start--%>
                     <asp:GridView ID="grdSearchUserDetails" runat="server" 
                         AutoGenerateColumns="false" CssClass="txtField" DataKeyNames="AssociateId" 
                         Font-Names="Arial" ForeColor="#333333" GridLines="None" 
                         onrowdatabound="GrdSearchUserDetails_RowDataBound" Width="95%" EmptyDataText="<%$ Resources:LocalizedText, NoRecordFound %>">
                         <RowStyle BackColor="#EFF3FB" />
                         <Columns>                          
                             <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, SlNo %>">
                                 <HeaderTemplate>
                                     <table align="left">
                                         <tr>
                                             <th align="left" colspan="2">
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedText, AssociateList %>"></asp:Label>
                                             </th>
                                             <%--   <th>
                                        <asp:Label ID="Label3" runat="server" Text="User Name"></asp:Label></th>   --%>
                                         </tr>
                                     </table>
                                 </HeaderTemplate>
                                 <ItemTemplate>
                                     <tr>
                                         <td align="left" colspan="4">
                                             <asp:LinkButton ID="lblAssociateId" runat="server" 
                                                 CommandArgument='<%#Eval("AssociateIdName") %>' CommandName="AssociateId" 
                                                 Text='<%#Eval("AssociateIdName") %>'>LinkButton</asp:LinkButton>
                                         </td>
                                     </tr>
                                 </ItemTemplate>
                                 <FooterTemplate>
                                     </table>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             <%--  <asp:BoundField  DataField="Phone_Home1"  /> --%>
                             <asp:TemplateField Visible="false">
                                 <ItemTemplate>
                                 <asp:HiddenField ID="HiddenField1" runat="server"   Value='<%#Eval("Phone_Home1") %>' />
                                  <%--   <asp:Label ID="lblPhone" runat="server" 
                                         CommandArgument='<%#Eval("Phone_Home1") %>' Text='<%#Eval("Phone_Home1") %>'></asp:Label>--%>
                                 </ItemTemplate>
                             </asp:TemplateField>
                         </Columns>
                     </asp:GridView>
                  </asp:Panel>
               </td>
            </tr>
      </table>

     <asp:HiddenField  ID="hdnvisitortype" runat="server"/>
    </div>
   
    </form>
</body>
</html>
