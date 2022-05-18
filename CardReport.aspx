<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardReport.aspx.cs" Inherits="VMSDev.CardReport"
    MasterPageFile="~/VMSMain.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder"    runat="server">

    <script src="Scripts/jquery-3.4.1.js" type="text/javascript" ></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript" ></script>

    <script language="javascript" type="text/javascript" >
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
            $('#<%=grdEmployee.ClientID%>').children().find("tr[class='odd_row'],tr[class='even_row']").each(function () {

                var meetingstarttime = (new Date($(this).children().find("label[id='startdate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='starttime']").html())).toString().split(' ');
                var meetingendtime = (new Date($(this).children().find("label[id='enddate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='endtime']").html())).toString().split(' ');
                var meetingtodate = (new Date($(this).children().find("label[id='todate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='starttime']").html())).toString().split(' ');


                if (rightNow.getTimezoneOffset() != -330) {
                    //                if (meetingendtime[3] == "23:59:00")
                    //                    meetingendtime[3] = "00:00:00";
                }
                if (meetingstarttime.length > 1) {
                    var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
                    meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                    $(this).children().find("label[id='startdate']").html((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);
                    $(this).children().find("label[id='starttime']").html(meetingstarttime[3]);
                }

                if (meetingendtime.length > 1) {
                    var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
                    meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                    $(this).children().find("label[id='enddate']").html((meetingendtime[2].length < 2 ? '0' + meetingendtime[2] : meetingendtime[2]) + ' ' + meetingendtime[1] + ' ' + meetingendtime[5]);
                    $(this).children().find("label[id='endtime']").html(meetingendtime[3]);
                }

                if (meetingtodate.length > 1) {
                    var temptotime = gettimedtls(meetingtodate[1], meetingtodate[3], hours, mins).split('|');
                    meetingtodate = (new Date(meetingtodate[5], temptotime[0], meetingtodate[2], temptotime[2], temptotime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                    $(this).children().find("label[id='todate']").html((meetingtodate[2].length < 2 ? '0' + meetingtodate[2] : meetingtodate[2]) + ' ' + meetingtodate[1] + ' ' + meetingtodate[5]);

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



    <script language="javascript" type="text/javascript">
        function fnStartDateClear() {
            document.getElementById('VMSContentPlaceHolder_txtFromDate').value = '';
        }
        function fnEndDateClear() {
            document.getElementById('VMSContentPlaceHolder_txtToDate').value = '';
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
    </script>

    <asp:ScriptManager ID="CardReportScriptManager" runat="server">
    </asp:ScriptManager>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="4" align="center" valign="baseline" style="height: 98%">
                <table width="100%" class="border" style="height: 98%">
                    <tr>
                        <td align="left" class="table_header_bg" colspan="4">
                            &nbsp;&nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header"
                                Text="Card Issued Report"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Panel ID="panelSearch" runat="server" Width="100%" CssClass="border">
                                <table id="Searchtbl" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 7%" align="right">
                                            <asp:Label ID="lblEmployeeID" runat="server" CssClass="field_txt" Text="Associate ID:"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 7%">
                                            <asp:TextBox ID="txtEmpID" runat="server" Width="80%" MaxLength="6" CssClass="field_txt"></asp:TextBox>
                                        </td>
                                          <td style="width: 7%" align="right">
                                            <asp:Label ID="lblCountry" runat="server" CssClass="field_txt" Text="Country:"></asp:Label>
                                        </td>
                                        <td style="width: 7%">
                                            <%--   <asp:DropDownList ID="ddlLocation" runat="server" Width="135px" DataSourceID="odsLocations" DataTextField="LocationCity" DataValueField="LocationCity" AppendDataBoundItems="True" CssClass="field_txt" AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                 <asp:ListItem Value="-123">All</asp:ListItem>
                             
                             </asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlCountry" runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="DdlCountry_SelectedIndexChanged">     </asp:DropDownList>
                                                </td>



                                        <td align="left">
                                            <asp:Label ID="lblCity" runat="server" CssClass="field_txt" Text="City:"></asp:Label>
                                        </td>
                                        <td align="left" >
                                            <%--   <asp:DropDownList ID="ddlLocation" runat="server" Width="135px" DataSourceID="odsLocations" DataTextField="LocationCity" DataValueField="LocationCity" AppendDataBoundItems="True" CssClass="field_txt" AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                 <asp:ListItem Value="-123">All</asp:ListItem>
                             
                             </asp:DropDownList>--%>
                                            <asp:DropDownList ID="ddlCity" CssClass="field_txt" runat="server" Width="100px"
                                                OnSelectedIndexChanged="DdlCity_SelectedIndexChanged" AutoPostBack="true"> </asp:DropDownList>
                                                </td>
                                                      <td align="left" >
                                                    <asp:Label ID="lblFacility" runat="server" CssClass="field_txt" 
                                                        Text="Facility:"></asp:Label>
                                                </td>
                                                <td align="left">
                                           
                                            <asp:DropDownList ID="ddlFacility" CssClass="field_txt" runat="server" Width="215px">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" style="white-space:nowrap">
                                            <asp:Label ID="lblFromDate" runat="server" CssClass="field_txt" Text="From Date:"></asp:Label>
                                        </td>
                                        <td  align="left">
                                            <%--<input name="ImgStartDate" style="width: 70px" class="innerTableResultLabelCell"
                                                id="txtFromDate" type="text" size="10" readonly="readonly" runat="server" />--%>
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="txtField" style="width: 70px" class="innerTableResultLabelCell"></asp:TextBox>                                                
                                                </td> <td style="width: 7%" align="left">
                                           <%-- <img alt="" src="~/Images/calender-icon.png" id="ImgStartDate" runat="server" style="cursor: pointer" />--%>
                                     <asp:ImageButton ID="imgFromDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                runat="server" Width="15px" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" />
            <cc1:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtStartDate"
                PopupButtonID="imgFromDate" Format="MM/dd/yyyy" PopupPosition="BottomRight">
            </cc1:CalendarExtender>

                                            <img id="imgStartDateClear" src="Images/Clear.jpg" onclick="javascript:fnStartDateClear();"
                                                style="cursor: hand" alt="Clear Date" />
                                        </td>
                                        <td align="left" style="white-space:nowrap" >
                                            <asp:Label ID="lblToDate" runat="server" CssClass="field_txt" Text="To Date:"></asp:Label>
                                        </td>
                                        <td align="left" >
                                            <%--<input name="ImgEndDate" style="width: 70px" class="innerTableResultLabelCell" id="txtToDate"
                                                type="text" size="10" readonly="readonly" runat="server" />--%>  
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="txtField" style="width: 70px" class="innerTableResultLabelCell"></asp:TextBox>
                                        </td> <td style="width: 7%" align="left">
                                            
                                            <%--<img alt="" src="~/Images/calender-icon.png" id="ImgEndDate" runat="server" style="cursor: pointer" />--%>
                                                     <asp:ImageButton ID="imgToDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                runat="server" Width="15px" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" />
            <cc1:CalendarExtender ID="ToDateCalendar" runat="server" TargetControlID="txtEndDate"
                PopupButtonID="imgToDate" Format="MM/dd/yyyy" PopupPosition="BottomRight">
            </cc1:CalendarExtender>
                                            <img id="img1" src="Images/Clear.jpg" onclick="javascript:fnEndDateClear();" style="cursor: hand"
                                                alt="Clear Date" />
                                        </td>
                                        <td align="right" valign="middle" style="width: 15%; vertical-align:middle">
                                            <asp:Button ID="btnSearch" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                                ForeColor="White" Height="21px" Width="71px" OnClick="BtnSearch_Click" Text="Search" />
                                               <asp:Button ID="btnClear" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                                    ForeColor="White" Height="21px" Width="71px" Text="Clear" OnClick="BtnClear_Click"
                                                    UseSubmitBehavior="False" /></td> <td align="left" style="width: 5%; vertical-align:middle">
                                                    
                                                    <asp:ImageButton ID="btnExcel" runat="server" Height="20px"
                                                        ImageUrl="~/Images/Excel.JPG"  OnClick="BtnExcel_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:ObjectDataSource ID="odsLocations" runat="server" SelectMethod="GetCityDetails"
                                    TypeName="VMSBusinessLayer.LocationBL"></asp:ObjectDataSource>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4">
                &nbsp;<asp:Label ID="lblEmployeeHeader" runat="server" CssClass="Table_header" Text="Employee Details"
                    Font-Size="11px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center" valign="top">
                <table id="gridtbl" runat="server" cellpadding="0" cellspacing="0" width="100%">
                    <tr align="center">
                        <td align="center">
                            <asp:GridView ID="grdEmployee" runat="server" CssClass="GridText" AutoGenerateColumns="False"
                                Width="100%" PageSize="100" EmptyDataText="No Records Found" AllowPaging="True"
                                OnPageIndexChanging="GrdEmployee_PageIndexChanging" PagerStyle-HorizontalAlign="Center">
                                <RowStyle CssClass="field_txt" />
                                <HeaderStyle CssClass="field_txt" BackColor="#C6D4BB" />
                                <Columns>
                                    <asp:BoundField HeaderText="AssociateID" DataField="AssociateID">
                                        <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Associate Name" DataField="AssociateName">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Associate Location" DataField="EmpLocation">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                     <asp:TemplateField HeaderText="From Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <label id="startdate">
                                                    <%#Eval("FromDate")%></label>                                            
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                     <asp:TemplateField HeaderText="To Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <label id="todate">
                                                    <%#Eval("ToDate")%></label>                                              
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Check-in Time" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <label id="starttime">
                                                    <%#Eval("CheckinTime")%></label>
                                                <%--    <asp:Label CssClass="cssDate" ID="lblDate" runat="server"  Text='<%# Bind("Date", "{0:dd/MM/yyyy }") %>' />
                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Pass Returned Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <label id="enddate">
                                                    <%#Eval("PassReturnedDate")%></label>
                                                <%--    <asp:Label CssClass="cssDate" ID="lblDate" runat="server"  Text='<%# Bind("Date", "{0:dd/MM/yyyy }") %>' />
                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                 
                                       <asp:TemplateField HeaderText="Check-out Time" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <label id="endtime">
                                                    <%#Eval("CheckoutTime")%></label>
                                                <%--    <asp:Label CssClass="cssDate" ID="lblDate" runat="server"  Text='<%# Bind("Date", "{0:dd/MM/yyyy }") %>' />
                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                    <%-- // IVS17082010AUTOCLOSE end--%>
                                   
                                    <asp:BoundField HeaderText="Pass Issued/Returned Location" DataField="PassIssuedLocation">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Pass Issued By" DataField="PassIssued">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Pass Returned Location" DataField="PassReturnedLocation">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerSettings  Mode="NextPrevious" NextPageText="Next" PreviousPageText="Prev" />                              
                                   <RowStyle CssClass="even_row" />
                                    <AlternatingRowStyle CssClass="odd_row" BackColor="#F4FCED" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center" valign="middle">
                <table border="0" id="errortbl" cellpadding="0" cellspacing="0" runat="server" style="border-right: black 1pt solid;
                    border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid;
                    width: 100%;">
                    <tr>
                        <td colspan="6" align="center" style="background-color: #edf6fd; width: 100%">
                            &nbsp;
                            <asp:Label ID="lblMessage" runat="server" CssClass="field_txt" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
