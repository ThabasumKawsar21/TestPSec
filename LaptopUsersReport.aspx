<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LaptopUsersReport.aspx.cs" Inherits="VMSDev.LaptopUsersReport" MasterPageFile="~/VMSMain.Master"%>
<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">

    <script language="javascript" type="text/javascript" >
  function fnStartDateClear()
  {
        document.getElementById('VMSContentPlaceHolder_txtFromDate').value='';   
  }
  function fnEndDateClear()
  {
        document.getElementById('VMSContentPlaceHolder_txtToDate').value='';
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
  
  </script> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">              
            <tr>
             <td colspan="4" align="center" valign="baseline" style="height: 98%">
             <table width="100%" class="border" style="height: 98%"  >                     
             <tr>
                 <td align="left" class="table_header_bg" colspan="4">
                     &nbsp;&nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header" Text="Laptop Users Report"></asp:Label></td>
             </tr>
                 <tr>
                     <td align="center"  colspan="4">
                         <asp:Panel ID="panelSearch" runat="server" Width="100%" CssClass="border">
                         <table id ="Searchtbl" cellpadding="0" cellspacing="0" width="100%">                           
                             <tr>
                                 <td style="width: 7%" align="right">
                                     <asp:Label ID="lblEmployeeID" runat="server" CssClass="field_txt" Text="Associate ID:"></asp:Label></td>
                                 <td align="left" style="width: 8%">
                                     <asp:TextBox ID="txtEmpID" runat="server" Width="80%" MaxLength="6" CssClass="field_txt"></asp:TextBox></td>
                                 <td style="width: 6%" align="right">
                                     <asp:Label ID="lblLocation" runat="server" CssClass="field_txt" Text="Facility:"></asp:Label></td>    
                                 <td style="width: 15%" align="left">
                             <asp:DropDownList ID="ddlLocation" runat="server" Width="235px"  DataTextField="LocationName" DataValueField="LocationID" AppendDataBoundItems="True" CssClass="field_txt" AutoPostBack="True" OnSelectedIndexChanged="DdlLocation_SelectedIndexChanged">
                                 <asp:ListItem Value="-123">All</asp:ListItem>
                             
                             </asp:DropDownList></td>
                                 <td align="right" style="width: 8%">
                             <asp:Label ID="lblFromDate" runat="server" CssClass="field_txt" Text="From Date:"></asp:Label></td>
                                 <td style="width: 14%" align="left"><input name="ImgStartDate" style="width:70px" class="innerTableResultLabelCell" id="txtFromDate" type="text" size="10"  readonly="readonly" runat="server" />
                         <img alt="" src="~/Images/calender-icon.png" id="ImgStartDate" runat="server" style="cursor:pointer" />
                             <img id="imgStartDateClear" src="Images/Clear.jpg"  onclick="javascript:fnStartDateClear();" style="cursor: hand" alt="Clear Date"/></td>
                                 <td align="right" style="width: 5%">
                             <asp:Label ID="lblToDate" runat="server" CssClass="field_txt" Text="To Date:"></asp:Label></td>
                                 <td align="left" style="width: 15%">
                             
                             <input name="ImgEndDate" style="width:70px" class="innerTableResultLabelCell" id="txtToDate" type="text" size="10"  readonly="readonly" runat="server"/>&nbsp;
                             <img alt="" src="~/Images/calender-icon.png" id="ImgEndDate" runat="server"  style="cursor:pointer"/>
                            <img id="img1" src="Images/Clear.jpg"  onclick="javascript:fnEndDateClear();" style="cursor: hand" alt="Clear Date"/></td>
                                 <td align="left" colspan="3">
                                     <asp:Button ID="btnSearch" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                         ForeColor="White" Height="21px" Width="71px"  OnClick="BtnSearch_Click" Text="Search" /><asp:Button ID="btnClear" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                         ForeColor="White" Height="21px" Width="71px" Text="Clear" OnClick="BtnClear_Click" UseSubmitBehavior="False" /><asp:ImageButton ID="btnExcel" runat="server" Height="20px" ImageUrl="~/Images/Excel.JPG"
                                         OnClick="BtnExcel_Click" /></td>
                             </tr>
                         </table>
                 <asp:ObjectDataSource ID="odsLocations" runat="server" SelectMethod="GetFacilities"
                     TypeName="VMSBusinessLayer.LocationBL" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>                            
                             </asp:Panel>
                         
                         </td>
                 </tr>                
             </table>
                 </td>
         </tr>
          <tr>
                     <td align="left" colspan="4">
                         &nbsp;<asp:Label ID="lblEmployeeHeader" runat="server" CssClass="Table_header" Text="Employee Details" Font-Size="11px"></asp:Label></td>
                 </tr>
           <tr>    
           <td colspan="4" align="center" valign="top">            
           <table id ="gridtbl" runat = "server" cellpadding="0" cellspacing="0" style="height: 98%;width:100%">
           <tr align="center">
           <td align="center">
              <asp:GridView ID="grdEmployee"   runat="server" CssClass ="GridText" AutoGenerateColumns="False" Width="100%" PageSize="20" EmptyDataText="No Records Found" AllowPaging="True" OnPageIndexChanging="GrdEmployee_PageIndexChanging">
                             <RowStyle CssClass="field_txt" />
                             <HeaderStyle CssClass="field_txt" BackColor="#C6D4BB" />                             
                             <Columns>
                                 <asp:BoundField HeaderText="AssociateID" DataField ="AssociateID">
                                     <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                     <HeaderStyle HorizontalAlign="Center" />
                                 </asp:BoundField>
                                 <asp:BoundField HeaderText="Associate Name" DataField="AssociateName" >                                     
                                     <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                     <HeaderStyle HorizontalAlign="Left" />
                                 </asp:BoundField>
                                  <asp:BoundField HeaderText="Associate Location" DataField ="EmpLocation" >
                                      <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                      <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                   <asp:BoundField HeaderText="Serial Number" DataField ="SerialNumber" >
                                      <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                      <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  <asp:BoundField HeaderText="Asset Number" DataField ="AssetNo" >
                                      <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                      <HeaderStyle HorizontalAlign="Left" />
                                  </asp:BoundField>
                                  
                                  <asp:BoundField HeaderText="Check-in Date" DataField ="IssuedDate" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="False"   >
                                      <ItemStyle Wrap="True" />
                                      </asp:BoundField> 
                                  <asp:BoundField HeaderText="Check-in Time" DataField ="CheckInTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss}" HtmlEncode="False"   >
                                      <ItemStyle Wrap="True" />
                                      </asp:BoundField> 
                                      <asp:BoundField HeaderText="Check-Out Date" DataField ="ReturnedDate" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="False"   >
                                      <ItemStyle Wrap="True" />
                                  </asp:BoundField> 
                                  <asp:BoundField HeaderText="Check-out Time" DataField ="CheckOutTime" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss}"  HtmlEncode="False"  >
                                      <ItemStyle Wrap="True" />
                                  </asp:BoundField>                                                                                                                        
                                 <asp:BoundField HeaderText="Facility" DataField="VerifiedLocation">                                     
                                     <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                     <HeaderStyle HorizontalAlign="Left" />
                                 </asp:BoundField>                               
                                  <asp:BoundField HeaderText="Manager Name" DataField="ManagerDetails">                                     
                                     <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                     <HeaderStyle HorizontalAlign="Left" />
                                 </asp:BoundField>
                             </Columns>
                              <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Prev" />
                             <AlternatingRowStyle CssClass="field_txt" BackColor="#F4FCED" />
                         </asp:GridView>
               </td>
           </tr>        
             </table>
               </td>                         
         </tr>
          <tr>
             <td colspan="4" align="center" valign="middle">
             <table border="0" id="errortbl" cellpadding="0" cellspacing ="0" runat="server" 
             style="border-right: black 1pt solid; border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid; width: 60%;" >
             <tr>             
             <td colspan="6"  align="left" style="background-color: #edf6fd; width: 60%">
             &nbsp;
                           <asp:Label ID="lblMessage" runat="server" CssClass="field_txt" ForeColor="Red"></asp:Label>                               
                           
             </td>
             </tr>             
           </table>
             </td>
         </tr>
  
        </table> 
</asp:Content>
