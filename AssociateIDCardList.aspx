<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssociateIDCardList.aspx.cs"
    Inherits="VMSDev.AssociateIDCardList" MasterPageFile="~/VMSMain.Master" EnableEventValidation="false" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

         <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript" ></script>
    <script type="text/javascript">
        function SelectAllCheckboxes(chk) {
            $('#<%=grdIDCardList.ClientID %>').find("input:checkbox").each(function () {
                if (this != chk) {
                    this.checked = chk.checked;
                }
            });
        }
        $(document).ready(function () {
            $('#<%=imgDownload.ClientID %>').click(function () {
                var chkboxrowcount = $("#<%=grdIDCardList.ClientID%> input[id*='chkIDCard']:checkbox:checked").size();
                if (chkboxrowcount == 0) {
                    alert("Please select at least one record");
                    return false;
                }
                return true;
            });
            $('#<%=imgExport.ClientID %>').click(function () {
                var chkboxrowcount = $("#<%=grdIDCardList.ClientID%> input[id*='chkIDCard']:checkbox:checked").size();
                if (chkboxrowcount == 0) {
                    alert("Please select at least one record");
                    return false;
                }
                return true;
            });
            $('#<%=imgPrint.ClientID %>').click(function () {
                var chkboxrowcount = $("#<%=grdIDCardList.ClientID%> input[id*='chkIDCard']:checkbox:checked").size();
                if (chkboxrowcount == 0) {
                    alert("Please select at least one record");
                    return false;
                }
                return true;
            });
        });

    </script>
     <style type="text/css">
         .style2
         {
             width: 103px;
         }
         .style3
         {
             width: 43px;
         }
         .style4
         {
             width: 100px;
         }
     </style>
</asp:Content>
<asp:Content id="Content2" contentplaceholderid="VMSContentPlaceHolder"    runat="server">   
    <br />
    <asp:scriptmanager runat="server" id="scriptIDCardList">
    </asp:scriptmanager>
   
    <table cellpadding="0" cellspacing="0" border="0" width="100%" >
        <tr>
            <td align="left" style="padding-top: 5px">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 950px">
                    <tr>
                        <td width="65px" style="white-space: nowrap; vertical-align:middle ">
                            <asp:label id="lblFromDate" cssclass="lblField" width="60px" text="From Date" 
                                runat="server"></asp:label>
                        </td>
                        <td align="left" style="white-space: nowrap ;vertical-align:middle"
                            class="style4" >
                            <asp:textbox id="txtFromDate" runat="server" cssclass="txtField" width="75px">
                            </asp:textbox>
                            <asp:imagebutton id="imgFromDate" style="cursor: hand" imageurl="~/Images/calender-icon.png"
                                runat="server" width="15px" imagealign="Middle" tooltip="From Date" />
                            <cc1:calendarextender id="FromDateCalendar" runat="server" targetcontrolid="txtFromDate"
                                popupbuttonid="imgFromDate" format="dd/MM/yyyy" popupposition="BottomRight"></cc1:calendarextender>
                        </td>
                        <td align="left" width="50px" style="white-space: nowrap ;vertical-align:middle">
                            <asp:label id="lblToDate" cssclass="lblField" width="46px" text="To Date" 
                                runat="server"></asp:label>
                        </td>
                        <td valign="middle" style="white-space: nowrap ;vertical-align: middle" 
                            class="style2">
                            <asp:textbox id="txtToDate" runat="server" cssclass="txtField" width="75px">
                            </asp:textbox>
                            <asp:imagebutton id="imgToDate" style="cursor: hand" imageurl="~/Images/calender-icon.png"
                                runat="server" width="15px" imagealign="Middle" tooltip="To Date" />
                            <cc1:calendarextender id="CalendarExtender1" runat="server" targetcontrolid="txtToDate"
                                popupbuttonid="imgToDate" format="dd/MM/yyyy" popupposition="BottomRight"></cc1:calendarextender>
                        </td>
                        <td width="40px"> 
                            <asp:label id="lblCity" cssclass="lblField" width="24px" text="City" 
                                runat="server"></asp:label></td>
                        <td width="70px" style="padding-left:2px"> <asp:DropDownList ID="drpCity" runat="server" DataTextField="Location" 
                             DataValueField="LocationId" CssClass="field_text" TabIndex="5"
                        Visible="True" Width="100px">
                    </asp:DropDownList></td>
                      <td style="padding-left:2px" class="style3"> 
                          <asp:label id="Label1" cssclass="lblField" width="42px" text="Status" 
                                runat="server"></asp:label></td>
                        <td width="120px" style="padding-left:2px"> <asp:DropDownList ID="drpStatus" runat="server" CssClass="field_text" TabIndex="6"
                        Visible="True" Width="120px">
                        <asp:ListItem Selected="True" Text="Ready to Print" Value="1"></asp:ListItem> 
                        <asp:ListItem Text="Missing Information" Value="2"></asp:ListItem>
                    </asp:DropDownList></td>
                        <td align="left" width="40px" style="white-space: nowrap" ;vertical-align: top">
                            <asp:button id="btnSearch" runat="server" backcolor="#767561" font-bold="False" font-size="10px"
                                forecolor="White" height="21px" width="71px" onclick="BtnSearch_Click" text="Search" />
                        </td>
                         <td align="left" width="40px" style="white-space: nowrap" ;vertical-align: top">
                            <asp:button id="btnReset" runat="server" backcolor="#767561" font-bold="False" font-size="10px"
                                forecolor="White" height="21px" width="71px" onclick="BtnReset_Click" text="Reset" />
                        </td>
                        <td valign="top" style="white-space: nowrap" ;vertical-align: top" width="35px">
                            <asp:imagebutton id="imgDownload" imageurl="~/Images/download image.bmp" width="32px"
                                height="30px" runat="server" onclick="ImgDownload_Click" tooltip="Download Selected Associate Images" />
                        </td>
                        <td valign="top" style="white-space: nowrap" ;vertical-align: top" width="35px">
                            <asp:imagebutton id="imgExport" imageurl="~/Images/Excel.JPG" width="32px" height="30px"
                                runat="server" onclick="BtnExport_Click" tooltip="Export Selected Associate Details" />
                        </td>
                        <td valign="top" style="white-space: nowrap" ;vertical-align: top" width="35px">
                            <asp:imagebutton id="imgPrint" imageurl="~/Images/Printer-icon.png" width="32px"
                                height="30px" runat="server" onclick="ImgPrint_Click" tooltip="Print Selected Associate Details" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
        <td colspan="13" align="left" style="padding-left:75px;padding-top:10px"><asp:label id="lblError" runat="server"  CssClass="errorLabel"></asp:label></td>
        </tr>
        <tr>
            <td colspan="10" align="left">
                <asp:GridView id="grdIDCardList" runat="server" cellpadding="4" forecolor="#333333"
                    autogeneratecolumns="False" font-names="Verdana" font-size="X-Small" datakeynames="AssociateId"
                    cssclass="gridStyle" headerstyle-wrap="True" pagesize="10" allowpaging="True"
                    onpageindexchanging="GrdIDCardList_PageIndexChanging" gridlines="Vertical" onrowdatabound="GrdIDCardList_RowDataBound"
                    onrowcommand="GrdRowCommand" allowsorting="true"
                    onsorting="GrdIDCardList_Sorting">
                    <emptydatarowstyle cssclass="EmptyRecord" />
                    <columns>                                     
                    <asp:TemplateField  ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15px">
                    <HeaderTemplate>           
                                <asp:CheckBox  ID="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);"></asp:CheckBox>
                                                 </HeaderTemplate> 
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIDCard" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>                                               
                                        <asp:TemplateField HeaderText="Photo" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                                            <ItemTemplate>
                                             <asp:Image ID="imgAssociatePhoto" runat="server" ImageUrl='' Height="75px"  Width="75px" oncontextmenu="return false" />
                                         </ItemTemplate>
                                        </asp:TemplateField> 
                                             <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssociateDisplayName" runat="server" Text='<%# Eval("DisplayName").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                 
                                             <asp:TemplateField  HeaderText="Associate ID" SortExpression="Associateid" HeaderStyle-Width="100px"   ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px">
                                            <ItemTemplate>                                            
                                                <asp:Label  ID="lblAssociateID" runat="server" Text='<%# Eval("Associateid").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                       
                                       <asp:TemplateField HeaderText="Emergency Contact No" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssociateEmergencyContact" runat="server" Text='<%# Eval("EmergencyContact").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>   
                                        <asp:TemplateField HeaderText="Blood Group" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssociateBloodGroup" runat="server" Text='<%# Eval("BloodGroup").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>       
                                           <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrintStatus" runat="server" Text='<%# Eval("PrintStatus").ToString() %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                                    
                                       
                                        <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="280px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnPrint" runat="server" Text="Print ID Card" CommandArgument='<%#Eval("AssociateId")%>'
                                                    CommandName="Print" CssClass="GridLinkButton" />
                                                <asp:LinkButton ID="btnDownLoad" runat="server" Text="Download Photo" CommandArgument='<%#Eval("AssociateId")%>'
                                                    CommandName="Download" CssClass="GridLinkButton" />   
                                                <asp:LinkButton ID="btnStatusUpdate" runat="server" Text="Update Print Status" CommandArgument='<%#Eval("AssociateId")%>'
                                                    CommandName="UpdateStatus" CssClass="GridLinkButton" />                                         
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                                                          
                                            <asp:TemplateField ItemStyle-Width="0px" HeaderStyle-CssClass="HideCol"  ItemStyle-CssClass="HideCol" >
                                         <ItemTemplate >          
                                         <asp:HiddenField ID="hdnFileUploadID" runat="server" Value='<%# Eval("FileUploadID")%>' /> 
                                         </ItemTemplate>
                                         </asp:TemplateField>
                                    
                                    </columns>
                    <footerstyle backcolor="#507CD1" font-bold="True" forecolor="White" />
                    <rowstyle backcolor="#EFF3FB" bordercolor="Gray" />
                    <alternatingrowstyle backcolor="#CDE3F1" forecolor="black" bordercolor="Gray" />
                    <headerstyle wrap="False" backcolor="#507CD1" font-bold="True" forecolor="White" />
                       <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Prev" />
                </asp:gridview>
            </td>
        </tr>
    </table>
</asp:content>


