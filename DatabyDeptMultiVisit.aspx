<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatabyDeptMultiVisit.aspx.cs"
    Inherits="VMSDev.DatabyDeptMultiVisit" MasterPageFile="~/VMSMain.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content id="VMSVolumeofVisitorsContent" contentplaceholderid="VMSContentPlaceHolder"
    runat="server">

    
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>


    <script language="javascript" type="text/javascript">

        function pageLoad() {
          //  GetOffsetTime();
        }
        function GetOffsetTime() {            
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            DatabyDeptMultiVisit.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            DatabyDeptMultiVisit.AssignCurrentDateTime(today);
        }
        function GetOffset() {
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            DatabyDeptMultiVisit.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            DatabyDeptMultiVisit.AssignCurrentDateTime(today);
            var r = document.getElementById('<%=btnDatabyDeptMultiVisitHidden.ClientID %>');
            r.click();
        }
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var rightNow = new Date();
            var offset = (rightNow.getTimezoneOffset()) / 60;
            std_time_offset = (offset * -1) - 5.50;
            var hours = parseInt(std_time_offset);
            std_time_offset -= parseInt(std_time_offset);
            std_time_offset *= 60;
            var mins = parseInt(std_time_offset);
            std_time_offset -= parseInt(std_time_offset);
            std_time_offset *= 60;
            var secs = parseInt(std_time_offset);
            var display_hours = hours;
            $('#<%=grdDataByDeptResult.ClientID%>').children().find("tr[class='odd_row'],tr[class='even_row']").each(function () {
              
                var meetingstarttime = (new Date($(this).children().find("label[id='lblgrdFromDate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='lblgrdFromTime']").html())).toString().split(' ')
                var meetingendtime = (new Date($(this).children().find("label[id='lblgrdToDate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='lblgrdToTime']").html())).toString().split(' ')
                if (meetingstarttime.length > 1) {
                    var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
                    meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                    $(this).children().find("label[id='lblgrdFromDate']").html((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);
                    $(this).children().find("label[id='lblgrdFromTime']").html(meetingstarttime[3]);
                }
                if (meetingendtime.length > 1) {
                    var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
                    meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                    $(this).children().find("label[id='lblgrdToDate']").html((meetingendtime[2].length < 2 ? '0' + meetingendtime[2] : meetingendtime[2]) + ' ' + meetingendtime[1] + ' ' + meetingendtime[5]);
                    $(this).children().find("label[id='lblgrdToTime']").html(meetingendtime[3]);
                }
            });
        });
    
         
        function gettimedtls(arg1, arg2, arg3, arg4) {
            switch (arg1) {
                case 'Jan':
                    x = '0|31';
                    break;
                case 'Feb':
                    x = '1|28:29';
                    break;
                case 'Mar':
                    x = '2|31';
                    break;
                case 'Apr':
                    x = '3|30';
                    break;
                case 'May':
                    x = '4|31';
                    break;
                case 'Jun':
                    x = '5|30';
                    break;
                case 'Jul':
                    x = '6|31';
                    break;
                case 'Aug':
                    x = '7|31';
                    break;
                case 'Sep':
                    x = '8|30';
                    break;
                case 'Oct':
                    x = '9|31';
                    break;
                case 'Nov':
                    x = '10|30';
                    break;
                case 'Dec':
                    x = '11|31';
                    break;
            }
            var tmpdtl = arg2.split(':');
            var tmpmin = parseInt(tmpdtl[1]);
            var tmphour = parseFloat(tmpdtl[0]);
            var remminhour = parseInt((tmpmin + arg4) / 60);
            tmpmin = (((tmpmin + arg4) / 60) - remminhour) * 60;
            tmphour = tmphour + parseInt(arg3) + remminhour;
            x = x + '|' + tmphour + '|' + tmpmin;
            return x;
        } 
    </script>
    <table class="tblHeadStyle" width="100%">
        <tr>
            <td colspan="5" align="left">
                <asp:label id="lblVisitorInfo" runat="server" cssclass="lblHeada" text="DataByDepartment Report">
                </asp:label>
            </td>
            <td align="right">
                <asp:label id="lblRequired" runat="server" cssclass="lblRequired" text="*"></asp:label>&nbsp;
                <asp:label id="lblIndication" runat="server" cssclass="lblIndication" text="Indicates Mandatory Field">
                </asp:label>
            </td>
        </tr>
        <tr>
            <td align="right" width="9%" valign="top">
                <asp:label id="lblFromDate" runat="server" cssclass="lblField" text="From Date"></asp:label>
                <asp:label id="lblRequiredCity" runat="server" cssclass="lblRequired" text="*"></asp:label>&nbsp;
            </td>
            <td align="Left" width="14%" valign="top">
                <asp:textbox id="txtFromDate" runat="server" cssclass="txtField" width="75px">
                </asp:textbox>
                &nbsp;<asp:imagebutton id="imgFromDate" style="cursor: hand" imageurl="~/Images/calender-icon.png"
                    runat="server" width="15px" imagealign="Middle" tooltip="From Date" />
                <cc1:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                    PopupButtonID="imgFromDate" Format="dd/MM/yyyy" PopupPosition="BottomRight">
                </cc1:CalendarExtender>
                <%-- <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtFromDate" ControlExtender="MaskedEditExtenderFromDate" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid date!" EmptyValueMessage="The date is not entered">

 

        </cc1:MaskedEditValidator>   --%>
            </td>
            <td align="left" width="6%" valign="top">
                <asp:label id="lblToDate" runat="server" cssclass="lblField" text="To Date"></asp:label>
                <asp:label id="lblRequiredCity0" runat="server" cssclass="lblRequired" text="*"></asp:label>
            </td>
            <td align="Left" width="9%" valign="top">
                <asp:textbox id="txtToDate" runat="server" cssclass="txtField" width="75px">
                </asp:textbox>
                <asp:imagebutton id="imgToDate" style="cursor: hand" imageurl="~/Images/calender-icon.png"
                    runat="server" width="15px" imagealign="Middle" tooltip="To Date" />
                <cc1:CalendarExtender ID="ToDateCalendar" runat="server" TargetControlID="txtToDate"
                    PopupButtonID="imgToDate" Format="dd/MM/yyyy" PopupPosition="BottomRight">
                </cc1:CalendarExtender>
            </td>
            <td align="left" width="32%" valign="top">
                <asp:label id="lblFacility1" runat="server" cssclass="lblField" text="Department"></asp:label>
                &nbsp;
                <asp:dropdownlist id="ddlDepartment" runat="server" width="235px">
                    <asp:listitem>Select</asp:listitem>
                    <asp:listitem>In</asp:listitem>
                    <asp:listitem>Out</asp:listitem>
                    <asp:listitem>Yet to arrive</asp:listitem>
                    <asp:listitem>Exceeded ETD</asp:listitem>
                    <%--Added by priti on 3rd June for VMS CR VMS31052010CR6--%>
                    <asp:listitem>Repeat Visitor</asp:listitem>
                </asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td align="center" style="padding-left: 12px" width="9%">
                <asp:label id="lblCountry" runat="server" cssclass="lblField" text="Country"></asp:label>
            </td>
            <td align="left" width="9%" class="style3">
                <asp:dropdownlist id="ddlCountry" runat="server" width="100px" autopostback="true"
                    onselectedindexchanged="DdlCountry_SelectedIndexChanged">
                </asp:dropdownlist>
            </td>
            <td align="left"  width="6%">
                <asp:label id="lblFacility0" runat="server" cssclass="lblField" text="City"></asp:label>
            </td>
            <td align="left" width="9%">
                <asp:dropdownlist id="ddlCity" runat="server" width="100px" onselectedindexchanged="DdlCity_SelectedIndexChanged"
                    autopostback="true">
                </asp:dropdownlist>
            </td>
            <td align="left" width="36%" valign="top">
                <%--<table>
           <tr>
           <td align="left" >
           <asp:Label ID="lblFromTime" runat="server" Text="From Time"  CssClass="lblField"></asp:Label>
                                </td> 
                                
                                 <td align="left" class="tdBold">
                                
                                <asp:TextBox ID="txtFromTime" runat="server" CssClass="txtField" width="30px" ></asp:TextBox>
                                   <cc1:MaskedEditExtender ID="MaskedEditFromTime" runat="server" TargetControlID="txtFromTime" Mask="99:99" ClearMaskOnLostFocus="false"  MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft" ErrorTooltipEnabled="True"/>
    
    

    
    
    <cc1:MaskedEditValidator ID="MV1" runat="server" ControlToValidate="txtFromTime" ControlExtender="MaskedEditFromTime" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid Time!" EmptyValueMessage="Time is not entered" />
        
    </td>
    
    <td align="left" >
                                <asp:Label ID="lblToTime" runat="server" Text="To Time" CssClass="lblField"></asp:Label>
                              
                              </td>
                              
                              <td align="right" >
                              
                                <asp:TextBox ID="txtToTime" runat="server" CssClass="txtField" 
                                         width="30px"></asp:TextBox>
                                     <cc1:MaskedEditExtender ID="MaskedEditExtenderToTime" runat="server" TargetControlID="txtToTime" Mask="99:99" ClearMaskOnLostFocus="false"  MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft" ErrorTooltipEnabled="True"/>
                                     
                                     </td>
                                     <td align=left>
  <cc1:MaskedEditValidator ID="MV2" runat="server" ControlToValidate="txtToTime" ControlExtender="MaskedEditExtenderToTime" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid Time!"  EmptyValueMessage="Time is not entered" />
                                 </td>
                                 <td>  
            <asp:ImageButton ID="btnhidetime" style="cursor:hand" 
                ImageUrl="~/Images/cancelClk1.jpg" runat="server" Width="18px" 
                ImageAlign="Middle" ToolTip="Hide Time"  OnClick="hide_time" 
                Height="17px" />
                         </td> 
                          </tr>
                          </table>--%>
                <asp:label id="lblFacility2" runat="server" cssclass="lblField" text="Facility"></asp:label>
                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                <asp:dropdownlist id="ddlFacility" runat="server" width="235px">
                </asp:dropdownlist>
            </td>
            <td align="left" valign="top">
                <asp:button id="btnSearch" runat="server" text="Search" cssclass="cssButton" onclick="BtnSearch_Click" />
                <asp:button id="btnReset0" runat="server" text="Reset" causesvalidation="False" cssclass="cssButton"
                    onclick="BtnReset0_Click" />
                <asp:imagebutton imageurl="~/Images/excel_icon.GIF" id="btnExport" runat="server"
                    text="Export" width="16px" onclick="BtnExport_Click" style="height: 16px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:imagebutton id="btnShowtime" style="cursor: hand" imageurl="~/Images/Clk1.jpg"
                    runat="server" width="19px" imagealign="Middle" tooltip="Show Time" onclick="Show_Time"
                    height="18px" />
            </td>
            <td>
                <table>
                    <tr>
                        <td align="left">
                            <asp:label id="lblFromTime" runat="server" text="From Time" cssclass="lblField"></asp:label>
                        </td>
                        <td align="left" class="tdBold">
                            <asp:textbox id="txtFromTime" runat="server" cssclass="txtField" width="30px">
                            </asp:textbox>
                            <cc1:MaskedEditExtender ID="MaskedEditFromTime" runat="server" TargetControlID="txtFromTime"
                                Mask="99:99" ClearMaskOnLostFocus="false" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft"
                                ErrorTooltipEnabled="True" />
                            <cc1:MaskedEditValidator ID="MV1" runat="server" ControlToValidate="txtFromTime"
                                ControlExtender="MaskedEditFromTime" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid Time!"
                                EmptyValueMessage="Time is not entered" />
                        </td>
                        <td align="left">
                            <asp:label id="lblToTime" runat="server" text="To Time" cssclass="lblField"></asp:label>
                        </td>
                        <td align="right">
                            <asp:textbox id="txtToTime" runat="server" cssclass="txtField" width="30px">
                            </asp:textbox>
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
                            <asp:imagebutton id="btnhidetime" style="cursor: hand" imageurl="~/Images/cancelClk1.jpg"
                                runat="server" width="18px" imagealign="Middle" tooltip="Hide Time" onclick="Hide_time"
                                height="17px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <hr width="100%" height="1" />
    <table border="0" id="errortbl" cellpadding="0" cellspacing="0" width="100%" runat="server"
        visible="false">
        <tr>
            <td>
                <asp:scriptmanager id="ScriptManager1" runat="server">
                </asp:scriptmanager>
                <asp:label id="lblResult" runat="server" cssclass="cssDisplay" font-bold="true" forecolor="Red">
                </asp:label>
                <asp:label id="lblStatusResult" runat="server" cssclass="cssDisplay" font-bold="true"
                    forecolor="Green"></asp:label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:panel id="PanelEqipment" runat="server" scrollbars="Auto" margin="0px" height="300px">
        <asp:gridview id="grdDataByDeptResult" runat="server" cellpadding="4" forecolor="#333333" autogeneratecolumns="False"
            datakeynames="VisitorID" headerstyle-wrap="True" pagesize="100" allowpaging="True" OnRowCreated="OnRowCreated" OnPageIndexChanging="GrdDataByDeptResult_PageIndexChanging"
            cssclass="GridText"  gridlines="Vertical" headerstyle-cssclass="GridText">
            <%--  <RowStyle BackColor="#EFF3FB" CssClass="field_txt"  />--%>          
            <columns>
                           <asp:TemplateField HeaderText="Sl.no">
                                <ItemTemplate> 
                                    <%# Container.DataItemIndex + 1 %> 
                                </ItemTemplate> 
                            <ControlStyle  Height="10px" Width="10px" />
                            <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>                            
                            <asp:BoundField HeaderText = "Name"  ItemStyle-Width="120px" DataField="Name" />
                            <asp:BoundField HeaderText = "Company" DataField="Company" ItemStyle-Width="75px"  />                                               
                           <asp:TemplateField HeaderText="From Date" SortExpression="Date" 
                     ItemStyle-Width="85px" >
                    <ItemTemplate>
                        <label id="lblgrdFromDate" >
                         <%#Eval("FromDate")%></label>                                                  
                    </ItemTemplate>                       
                </asp:TemplateField>
                      
                <asp:TemplateField HeaderText="To Date" SortExpression="Date"  
                     ItemStyle-Width="85px"  >
                    <ItemTemplate  >
                        <label id="lblgrdToDate">
                           <%#Eval("ToDate")%></label>  
                    </ItemTemplate> 
                                     
                </asp:TemplateField>
                            
                              <asp:BoundField HeaderText="Host Name" DataField="Host" />
                              <asp:BoundField HeaderText="Department" DataField="Department" />
                              <asp:BoundField HeaderText="City" DataField="VisitingCity" />
                              <asp:BoundField HeaderText="Facility" DataField="Facility" />
                              <asp:BoundField HeaderText="Purpose" DataField="Purpose" />                                       
                 <asp:TemplateField >
                 <ItemTemplate >
                 <ItemStyle HorizontalAlign="Right" />

                        <label id="lblgrdFromTime">
                            <%#Eval("inTime")%></label>           
                                            
                    </ItemTemplate>
                    </asp:TemplateField>     
                      <asp:TemplateField >
                 <ItemTemplate >
                        <label id="lblgrdToTime" >
                            <%#Eval("ActualOutTime")%></label>                           
                    </ItemTemplate>
                    </asp:TemplateField> 
                              </columns>
            <footerstyle backcolor="#507CD1" font-bold="True" forecolor="White" />
            <pagerstyle backcolor="#2461BF" forecolor="White" horizontalalign="Center" />
            <selectedrowstyle backcolor="#99CCFF" font-bold="True" forecolor="#333333" />
            <headerstyle backcolor="#507CD1" font-bold="True" forecolor="White" />
            <editrowstyle backcolor="#2461BF" />
            <RowStyle CssClass="even_row" />
            <AlternatingRowStyle CssClass="odd_row" BackColor="#F4FCED" />
        </asp:gridview>        
    </asp:panel>      
    <asp:button id="btnDatabyDeptMultiVisitHidden" runat="server" style="display: none"
        text="Button" onclick="BtnDatabyDeptMultiVisitHidden_Click" />
</asp:content>
<asp:content id="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style2
        {
            width: 127px;
        }
        .style3
        {
            width: 171px;
            height: 28px;
        }
        .style4
        {
            width: 240px;
        }
        .style6
        {
            height: 28px;
        }
        .style7
        {
            width: 176px;
            height: 28px;
        }
        .style8
        {
            height: 28px;
            width: 92px;
        }
        .style9
        {
            width: 399px;
        }
        .hiddencol { display: none; }
    </style>
</asp:content>
