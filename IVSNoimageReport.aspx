<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IVSNoimageReport.aspx.cs" Inherits="VMSDev.IVSNoimageReport" MasterPageFile="~/VMSMain.Master"  %>

<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">

<script language="javascript" type="text/javascript" >
    window.onload = function () {

        var txtassociate = document.getElementById('VMSContentPlaceHolder_txtEmpID');
        txtassociate.focus();
    }
    function fnStartDateClear() {
        document.getElementById('ctl00$VMSContentPlaceHolder$txtFromDate').value = '';
    }
    function fnEndDateClear() {
        document.getElementById('ctl00$VMSContentPlaceHolder$txtToDate').value = '';
    }

    function SpecialCharacterValidation(e) {
        var keycode;
        if (window.event)
            keycode = window.event.keyCode;
        else if (event) keycode = event.keyCode;
        else if (e) keycode = e.which;
        else return true;
        if (keycode > 47 && keycode <= 57) {
            return true;
        }
        else {
            return false;
        }
    }
    function setClipBoardData() {
        setInterval("window.clipboardData.setData('text','')", 20);
    }
    function blockError() {
        window.location.reload(true);
        return true;
    }

    function allowNo(ie, moz) {

        if (moz != null) {
            //alert(moz);
            if ((moz >= 48) && (moz < 58) || moz == 8 || moz == 13 || moz == 45) {
                return true;
            }
            else {
                return false;
            }
        }
        else {

            if ((ie >= 48) && (ie < 58)) {
                return true;
            }
            else {
                return false;
            }
        }

    }

    function CallPrint() {

        //var grdContent = document.getElementById('VMSContentPlaceHolder_grdEmployee');

        //     

        //grdContent.setAttribute('AllowPaging', 'false');

        //    grdContent.visible=true;
        var prtContent = document.getElementById('VMSContentPlaceHolder_grdEmployee');

        //    grdContent.attributes.removeNamedItem('AllowPaging');
        var WinPrint = window.open('', '', 'letf=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
        WinPrint.setAttribute("rel", "noopener noreferrer")
        WinPrint.document.write(prtContent.innerHTML);
        WinPrint.document.close();
        WinPrint.focus();
        WinPrint.print();
        WinPrint.close();
        //    prtContent.innerHTML = strOldOne;

        __doPostBack('', 'ResetPrint');

    }

  
  </script> 
  
    <table border="0" cellpadding="0" cellspacing="0" width="100%">     
            <tr>
             <td colspan="4" align="center" valign="bottom">
             <table width="100%" class="border" valign="bottom">                     
            
                 <tr valign="bottom">
                     <td align="left" class="table_header_bg" colspan="9">
                         &nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header" Text="Associates without Photo"></asp:Label></td>
                 </tr>
                 <tr>                 
                     <td style="width: 8%" valign="bottom" align="right">                         
                         <asp:Label ID="Label6" runat="server" CssClass="field_text" Text="Associate ID :"></asp:Label></td>                         
                     <td align="left" colspan="8" valign="bottom">
                     
                         <asp:TextBox ID="txtEmpID" runat="server" MaxLength="6" Width="10%" CssClass="field_text" onpaste="return false"  onKeyPress="return allowNo(event.keyCode, event.which);" ></asp:TextBox>
                         &nbsp; &nbsp;

                           <asp:Label ID="lblCountry" runat="server" CssClass="field_text" 
                             Text="Country :"></asp:Label>&nbsp;<asp:DropDownList ID="drpCountry" 
                             runat="server" AutoPostBack="true"   CssClass="field_text" width="100" 
                             onselectedindexchanged="DrpCountry_SelectedIndexChanged"></asp:DropDownList>

                                 &nbsp; &nbsp;
                          <asp:Label ID="lblCardIssuedLocation" runat="server" CssClass="field_text" 
                             Text="City :"></asp:Label>&nbsp;<asp:DropDownList ID="ddlLocation" 
                             runat="server" AutoPostBack="true"  DataTextField="LocationCity" width="100"  
                             DataValueField="LocationCity" CssClass="field_text" 
                             onselectedindexchanged="DdlLocation_SelectedIndexChanged"></asp:DropDownList>&nbsp;
                         <asp:Label ID="lblCardIssuedFacility" runat="server" CssClass="field_text" 
                             Text="Facility:"></asp:Label>&nbsp;<asp:DropDownList ID="ddlFacility" width="215px" 
                             DataValueField="LocationId" DataTextField="LocationName" 
                             runat="server" 
                             CssClass="field_text" AutoPostBack="true"></asp:DropDownList>&nbsp;
                         &nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" BackColor="#767561" 
                             Font-Bold="False" Font-Size="10px"
                             ForeColor="White" Height="20px"  Text="Search" 
                             CausesValidation="False" onclick="BtnSearch_Click"  />
                         <asp:Button ID="btnClear" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                             ForeColor="White" Height="20px" OnClick="BtnClear_Click" Text="Clear" CausesValidation="False" UseSubmitBehavior="False" />
                                     
                               &nbsp;<asp:ImageButton ID="btnExcel" runat="server" Height="15px" ImageUrl="~/Images/excel_icon.GIF"
                                         OnClick="BtnExcel_Click" Enabled="false"  
                             ToolTip="Export to excel file" style="width: 15px" />
                                      
                                         <%--<asp:Button ID="btnPrint" runat="server" BackColor="#767561" 
                             Font-Bold="False" Font-Size="10px"
                             ForeColor="White" Height="21px" OnClientClick="CallPrint()" Text="Print" 
                             CausesValidation="False" UseSubmitBehavior="False" 
                             />--%>
                                     
                                        <%-- <asp:button ID="btnPrint" runat="server" BackColor="#767561"  Font-Bold="False" Font-Size="10px" ForeColor="White" Height="20px"
                             Text="Print" onClick="javascript:CallPrint('divPrint');"  /> --%>
                        </td>
                 </tr>
                 </table>
                 </td></tr>
                
                 <tr>
                     <td colspan="9" align="center">
                         <br />
                         <asp:Label ID="lblSuccessMessage" runat="server" CssClass="Table_header" Visible="false"></asp:Label>
                         <br />
                        <br />
                         <div ID="divPrint">
                             <asp:GridView ID="grdEmployee" runat="server" AllowPaging="True" 
                                 AutoGenerateColumns="False" CssClass="GridText" 
                                 EmptyDataText="No Records Found" HorizontalAlign="Center" 
                                 OnPageIndexChanging="GrdEmployee_PageIndexChanging" 
                                 PagerStyle-HorizontalAlign="Center" PageSize="20" Width="65%">
                                 <RowStyle CssClass="field_txt" HorizontalAlign="Center" />
                                 <HeaderStyle BackColor="#C6D4BB" CssClass="field_txt" 
                                     HorizontalAlign="Center" />
                                 <Columns>
                                     <asp:TemplateField HeaderText="Sl.no">
                                         <ItemTemplate>
                                             <%# Container.DataItemIndex + 1 %>
                                         </ItemTemplate>
                                         <ControlStyle Height="10px" Width="10px" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:TemplateField>
                                     <asp:BoundField DataField="AssociateID" HeaderStyle-HorizontalAlign="Center" 
                                         HeaderText="AssociateID">
                                         <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="AssociateName" HeaderStyle-HorizontalAlign="Center" 
                                         HeaderText="Associate Name">
                                         <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="LocationCity" HeaderStyle-HorizontalAlign="Center" 
                                         HeaderText="City">
                                         <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="LocationName" HeaderStyle-HorizontalAlign="Center" 
                                         HeaderText="Facility">
                                         <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="Manager" HeaderStyle-HorizontalAlign="Center" 
                                         HeaderText="Manager">
                                         <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="Project_ID" HeaderStyle-HorizontalAlign="Center" 
                                         HeaderText="Project ID">
                                         <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="DateofJoining" HeaderStyle-HorizontalAlign="Center" 
                                         HeaderText="Date of Joining">
                                         <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                         <HeaderStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                 </Columns>
                                 <PagerSettings Mode="NextPrevious" NextPageText="Next" 
                                     PreviousPageText="Prev" />
                                 <AlternatingRowStyle BackColor="#F4FCED" CssClass="field_txt" />
                             </asp:GridView>
                         </div>
                        </br>
                     </td>
                 </tr>
       </table>
                 &nbsp;
      

              <table border="0" id="Table1" cellpadding="0" cellspacing ="0" runat="server" width="58%" > 
              
                 <tr align="center">
             <td colspan="6"  align="center" style="border-right: black 1pt solid; border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid;background-color: #edf6fd; width: 60%; height: 19px;" id="errortbl" visible="false">
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <asp:Label ID="lblMessage" runat="server" CssClass="field_text" Font-Bold="true" ForeColor="Red"></asp:Label>                               
                         <asp:Label ID="lblIDCardStatus" runat="server" Visible="false" CssClass="field_text" Font-Bold="true" ForeColor="Red"></asp:Label>
                           
             </td>
                 </tr>
           </table>  
           
           
                        <%-- <div id="div1">
                         
                         <asp:GridView ID="grdEmployee1" runat="server" Visible=false >
                     </asp:GridView>
                   </div>--%>
               
</asp:Content>
