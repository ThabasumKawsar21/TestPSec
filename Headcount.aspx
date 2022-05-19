<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Headcount.aspx.cs" Inherits="VMSDev.Headcount"
    MasterPageFile="~/VMSMain.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="VMSVolumeofVisitorsContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript" ></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript" ></script>

    <script language="javascript" type="text/javascript">

        function pageLoad() {

            GetOffsetTime();
        }
        function GetOffsetTime() {

            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            Headcount.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            Headcount.AssignCurrentDateTime(today);
        }

        function GetOffset() {

         
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            Headcount.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            Headcount.AssignCurrentDateTime(today);

            var r = document.getElementById('<%=btnHeadcountHidden.ClientID %>');
            r.click();

        }


            </script>
         <script language="javascript" type="text/javascript">           
        //************************To allow only Alpha characters in text box fields******************************
        function allowAlpha(ie, moz) {

            if (moz != null) {
                //alert(moz);
                if ((moz == 32) || (moz >= 65) && (moz < 91) || moz == 8 || moz == 13 || (moz >= 97) && (moz < 123)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                if ((ie == 32) || (ie >= 65) && (ie < 91) || (ie >= 97) && (ie < 123)) {
                    return true;
                }
                else {
                    return false;
                }
            }

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

       
    </script>
    <table class="tblHeadStyle" width="100%">
        <tr>
            <td colspan="5" align="left">
                <asp:Label ID="lblVisitorInfo" runat="server" CssClass="lblHeada" Text="Headcount Report"></asp:Label>
            </td>
            <td class="style9">
            </td>
            <td align="right">
                <asp:Label ID="lblRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                <asp:Label ID="lblIndication" runat="server" CssClass="lblIndication" Text="Indicates Mandatory Field"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" class="style11">
                <asp:Label ID="lblFromDate" runat="server" CssClass="lblField" Text="From Date"></asp:Label>
                <asp:Label ID="lblRequiredCity" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
            </td>
            <td align="Left" valign="top" class="style8">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtField" Width="75px"></asp:TextBox>
                <asp:ImageButton ID="imgFromDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                    runat="server" Width="15px" ImageAlign="Middle" ToolTip="From Date" />
                <cc1:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                    PopupButtonID="imgFromDate" Format="dd/MM/yyyy" PopupPosition="BottomRight">
                </cc1:CalendarExtender>
                <%-- <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtFromDate" ControlExtender="MaskedEditExtenderFromDate" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid date!" EmptyValueMessage="The date is not entered">

 

        </cc1:MaskedEditValidator>   --%>
            </td>
            <td align="right" width="9%" valign="top">
                <asp:Label ID="lblToDate" runat="server" CssClass="lblField" Text="To Date"></asp:Label>
                <asp:Label ID="lblRequiredCity0" runat="server" CssClass="lblRequired" Text="*"></asp:Label>
            </td>
            <td align="Left" valign="top" class="style5">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="txtField" Width="75px"></asp:TextBox>
                <asp:ImageButton ID="imgToDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                    runat="server" Width="15px" ImageAlign="Middle" ToolTip="To Date" />
                <cc1:CalendarExtender ID="ToDateCalendar" runat="server" TargetControlID="txtToDate"
                    PopupButtonID="imgToDate" Format="dd/MM/yyyy" PopupPosition="BottomRight">
                </cc1:CalendarExtender>
            </td>
            <td align="left" valign="top" class="style12">
                <asp:Label ID="lblFacility1" runat="server" CssClass="lblField" Text="Department"></asp:Label></td>
                &nbsp;<td align="left" class="style9">
                <asp:DropDownList ID="ddlDepartment" runat="server" Width="235px">
                    <asp:ListItem>Select</asp:ListItem>
                    <asp:ListItem>In</asp:ListItem>
                    <asp:ListItem>Out</asp:ListItem>
                    <asp:ListItem>Yet to arrive</asp:ListItem>
                    <asp:ListItem>Exceeded ETD</asp:ListItem>
                    <%--Added by priti on 3rd June for VMS CR VMS31052010CR6--%>
                    <asp:ListItem>Repeat Visitor</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="left" valign="top" class="style4">    
            
            <table cellpadding="0" cellspacing="0" border="0">
            <tr>
            <td style="white-space:nowrap"> <asp:Label ID="lblPurpose" runat="server" CssClass="lblField" Text="Visitor Type"></asp:Label></td>
            <td>   <asp:DropDownList ID="DdlPurpose" Width="190px" runat="server" OnSelectedIndexChanged="DdlPurpose_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList></td>
                <td><asp:TextBox ID="txtPurpose" runat="server" CssClass="txtField" Visible="false" MaxLength="100"></asp:TextBox></td>
            </tr>            
            </table>                   
                </td>
            <td align="left">
                <asp:Label ID="lblOtherPurpose" runat="server" Visible="false" Font-Size="X-Small"
                    ForeColor="Red" Font-Bold="true" CssClass="lblField" Text="<br>If Others Specify"
                    valign="top"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" width="9%" valign="top" style="padding-left:33px">
                <asp:Label ID="lblCountry" runat="server" CssClass="lblField" Text="Country"></asp:Label>
            </td>
            <td align="left" class="style3" valign="top">
                <asp:DropDownList ID="ddlCountry" runat="server" Width="100px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td align="left" valign="top" style="padding-left:53px">
                <asp:Label ID="lblFacility0" runat="server" CssClass="lblField" Text="City"></asp:Label>
            </td>
            <td align="left" class="style3" valign="top">
                <asp:DropDownList ID="ddlCity" runat="server" Width="100px" OnSelectedIndexChanged="DdlCity_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td align="left" class="style13" valign="top">

                <asp:Label ID="lblFacility2" runat="server" CssClass="lblField" Text="Facility"></asp:Label></td>
            
                 <td align="left" class="style3" width="9%" valign="top"  ><asp:DropDownList ID="ddlFacility"
                    runat="server" Width="235px"  >
                </asp:DropDownList>
            </td>
            <td align="left" class="style5">
                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnShowtime" Style="cursor: hand" ImageUrl="~/Images/Clk1.jpg"
                                runat="server" Width="19px" ImageAlign="Middle" ToolTip="Show Time" OnClick="Show_Time"
                                Height="18px" Visible="false" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lblFromTime" runat="server" Text="From Time" CssClass="lblField"></asp:Label>
                        </td>
                        <td align="left" class="tdBold">
                            <asp:TextBox ID="txtFromTime" runat="server" CssClass="txtField" Width="30px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditFromTime" runat="server" TargetControlID="txtFromTime"
                                Mask="99:99" ClearMaskOnLostFocus="false" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft"
                                ErrorTooltipEnabled="True" />
                            <cc1:MaskedEditValidator ID="MV1" runat="server" ControlToValidate="txtFromTime"
                                ControlExtender="MaskedEditFromTime" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid Time!"
                                EmptyValueMessage="Time is not entered" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lblToTime" runat="server" Text="To Time" CssClass="lblField"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:TextBox ID="txtToTime" runat="server" CssClass="txtField" Width="30px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MaskedEditExtenderToTime" runat="server" TargetControlID="txtToTime"
                                Mask="99:99" ClearMaskOnLostFocus="false" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft"
                                ErrorTooltipEnabled="True" />
                        </td>
                        <td align="left">
                            <cc1:MaskedEditValidator ID="MV2" runat="server" ControlToValidate="txtToTime" ControlExtender="MaskedEditExtenderToTime"
                                Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid Time!" EmptyValueMessage="Time is not entered" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnhidetime" Style="cursor: hand" ImageUrl="~/Images/cancelClk1.jpg"
                                runat="server" Width="18px" ImageAlign="Middle" ToolTip="Hide Time" OnClick="Hide_time"
                                Height="17px" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" valign="top" class="style4">
            </td>
            <td align="left" valign="top" class="style9">
                &nbsp;
            </td>
            <td valign="top">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" valign="top" class="style11">
                &nbsp;
            </td>
            <td align="left" class="style3" valign="top">
                &nbsp;
            </td>
            <td align="right" width="9%" valign="top">
                &nbsp;
            </td>
            <td align="left" class="style5">
                &nbsp;
            </td>
            <td align="left" valign="top" class="style12">
                &nbsp;
            </td>
            <td align="left" valign="top" class="style9">
                &nbsp;
            </td>
            <td align="left" valign="top" class="style6">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="cssButton" OnClick="BtnSearch_Click" />
                <asp:Button ID="btnReset0" runat="server" Text="Reset" CausesValidation="False" CssClass="cssButton"
                    OnClick="BtnReset0_Click" Width="52px" />
                <asp:ImageButton ImageUrl="~/Images/excel_icon.GIF" ID="btnExport" runat="server"
                    Text="Export" Width="16px" OnClick="BtnExport_Click" Style="height: 16px" />
            </td>
        </tr>
    </table>
    <hr width="100%" height="1" />
    <table border="0" id="errortbl" cellpadding="0" cellspacing="0" width="100%" runat="server"
        visible="false">
        <tr>
            <td>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Label ID="lblResult" runat="server" CssClass="cssDisplay" Font-Bold="true" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblStatusResult" runat="server" CssClass="cssDisplay" Font-Bold="true"
                    ForeColor="Green"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:Panel ID="PanelEqipment" runat="server" ScrollBars="Auto" margin="0px" Height="300px">
        <asp:GridView ID="grdResult" runat="server" CellPadding="4" ForeColor="#333333" Font-Names="Arial"
            Font-Size="Small" HeaderStyle-Wrap="True" CssClass="GridText" HeaderStyle-CssClass="GridText"
            GridLines="Vertical" AutoGenerateColumns="False">
           <%-- <RowStyle  BackColor="#EFF3FB" CssClass="field_txt" />--%>
           <RowStyle  BackColor="#EFF3FB" CssClass="even_row" />
            <Columns>
                <asp:TemplateField HeaderText="Sl.no">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="From Date" SortExpression="Date" 
                     ItemStyle-Width="85px" >
                    <ItemTemplate  >
                        <label id="lblgrdFromDate">
                            <%#Eval("FromDate", "{0:dd-MMM-yyyy}")%></label>                           
                    </ItemTemplate>
                       
                </asp:TemplateField>
          <%--      <asp:TemplateField Visible="true">
                 <ItemTemplate>
                        <label id="lblgrdFromTime">
                            <%#Eval("FromTime")%></label>                           
                    </ItemTemplate>
                    </asp:TemplateField>
              --%>
                <asp:TemplateField HeaderText="To Date" SortExpression="Date"  
                     ItemStyle-Width="85px" >
                    <ItemTemplate  >
                        <label id="lblgrdToDate">
                            <%#Eval("ToDate","{0:dd-MMM-yyyy}")%> </label>
                    </ItemTemplate> 
                                     
                </asp:TemplateField>
            <%--      <asp:TemplateField Visible="true">
                 <ItemTemplate>
                        <label id="lblgrdToTime">
                            <%#Eval("ToTime")%></label>                           
                    </ItemTemplate>
                    </asp:TemplateField>--%>
                
      <%--          <asp:BoundField HeaderText="From Date" DataField="FromDate" DataFormatString="{0:dd-MMM-yyyy}"
                    ItemStyle-Width="85px" />--%>
               <%-- <asp:BoundField HeaderText="To Date" DataField="ToDate" DataFormatString="{0:dd-MMM-yyyy}"
                    ItemStyle-Width="85px" />--%>
                <asp:BoundField HeaderText="City" DataField="LocationCity" />
                <asp:BoundField HeaderText="Facility" DataField="Facility" />
                <asp:BoundField HeaderText="Department" DataField="HostDepartment" />
                <asp:BoundField HeaderText="Headcount" DataField="HeadCount" />              
                                       
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle CssClass="odd_row" BackColor="White" />
        </asp:GridView>
    </asp:Panel>
          <asp:Button ID="btnHeadcountHidden" runat="server" Style="display: none" Text="Button"
        OnClick="BtnHeadcountHidden_Click" />
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style3
        {
            height: 10px;
            width: 112;
        }
        .style4
        {
            width: 26%;
        }
        .style5
        {
            width: 14%;
        }
        .style6
        {
            width: 158px;
        }
        .style8
        {
            width: 112px;
        }
        .style9
        {
            width: 94px;
        }
        .style11
        {
            width: 9%;
        }
        .style12
        {
            width: 70px;
            padding-top:10px;
        }
        .style13
        {
            height: 10px;
            width: 70px;
        }
    </style>
  
</asp:Content>
