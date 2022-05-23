<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SafetyPermit.aspx.cs" Inherits="VMSDev.SafetyPermit" 
MasterPageFile="~/VMSMain.Master" %>
<%@ Register src="~/SafetyPermitUserControls/ViewLogbySecuritySP.ascx" tagname="ViewLogbySecuritySP" tagprefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="VMSFramework" Namespace="VMSFramework" TagPrefix="vf" %>
<%--<link href="../App_Themes/StandardUI.css" rel="stylesheet" type="text/css" />--%>
<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server"> 
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript"src="Scripts/common.js"></script>
    <script type="text/javascript"src="Scripts/SafetyPermit.js"></script>
<script language="javascript" type="text/javascript">
    //************************To allow only Alpha characters in text box fields******************************
    function onlyNumbers(evt) {
        var e = event || evt; // for trans-browser compatibility
        var charCode = e.which || e.keyCode;

        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    };
    

   
    function avoidSpecialCharacters(obj) {
        //  alert("id of control is:" + obj.id);
        var ctrl = document.getElementById(obj.id);
        var temp = ctrl.value;
        temp = temp.replace(/[^a-zA-Z 0-9]+/g, '');
        ctrl.value = temp;
        //alert("id of control is:" + temp);
    };
    function allowAlpha(ie, moz) {

        if (moz != null) {
            //alert(moz);
            if (((moz >= 17) && (moz < 106)) || moz == 8 || moz == 9 || moz == 190) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            if (((ie >= 17) && (ie < 106)) || (ie == 8) || (ie == 9) || (ie == 190)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    function fnStartDateRefresh() {
         
        var today = (new Date()).format('MM/dd/yyyy HH:mm:ss');
        var calendarBehavior = $find("<%=FromDateCalendar.ClientID %>");
        calendarBehavior.set_selectedDate(today);

    }
    function fnEndDateRefresh() {
         
        var today = (new Date()).format('MM/dd/yyyy HH:mm:ss');
        var calendarBehavior = $find("<%=ToDateCalendar.ClientID %>");
        calendarBehavior.set_selectedDate(today);
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
        $('#<%=grdResult.ClientID%>').children().find("tr[class='odd_row'],tr[class='even_row']").each(function () {
           
            //var meetingstarttime = (new Date($(this).children().find("label[id='startdate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='starttime']").html())).toString().split(' ');
            //var meetingendtime = (new Date($(this).children().find("label[id='startdate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='expectedouttime']").html())).toString().split(' ');
            //var actualouttime = (new Date($(this).children().find("label[id='startdate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='actualouttime']").html())).toString().split(' ');
            if (rightNow.getTimezoneOffset() != -330) {
                //                if (meetingendtime[3] == "23:59:00")
                //                    meetingendtime[3] = "00:00:00";
            }
            var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
            var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
            if (actualouttime[3] == "00:00:00")
                var tempactualouttime = "";
            else
                var tempactualouttime = gettimedtls(actualouttime[1], actualouttime[3], hours, mins).split('|');
            meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
            meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
            actualouttime = (new Date(actualouttime[5], tempactualouttime[0], actualouttime[2], tempactualouttime[2], tempactualouttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');

            //$(this).children().find("label[id='startdate']").html((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);
            //$(this).children().find("label[id='starttime']").html(meetingstarttime[3]);
            //$(this).children().find("label[id='expectedouttime']").html(meetingendtime[3]);
            //$(this).children().find("label[id='actualouttime']").html(actualouttime[3]);
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
    function GetOffset() {
        setDefaultDate
        var CurrentDate = new Date();
        var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
        SafetyPermit.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
        SafetyPermit.AssignCurrentDateTime(today);
        var r = document.getElementById('<%=btnHidden.ClientID %>');
        r.click();
    }
    function setDefaultDate() {
        var now = new Date();
        var formatDate = now.format("dd/MM/yyyy");
        $('#<%=hdnCurrentDate.ClientID %>').val(formatDate);
        var currenthour = now.getHours();
        var currentminute = now.getMinutes();
        var fromtime = "00" + ":" + "01";
        $('#<%=hdnCurrentTime.ClientID %>').val(fromtime);
    }
    function GetDefaultDate() {
        var rightNow = new Date();
       
        var formatFromDate = rightNow.format("MM/dd/yyyy");
        $('#<%=txtFromDate.ClientID %>').val(formatFromDate);
        $('#<%=txtToDate.ClientID %>').val(formatFromDate);
    }

    function GetCheckOutTime() {
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
         
        var Startdate = $('#<%=hdnCheckOutFromDate.ClientID %>').val();
        var StartTime = $('#<%=hdnCheckOutFromTime.ClientID %>').val();
        var Enddate = $('#<%=hdnCheckOutToDate.ClientID %>').val();
        var EndTime = $('#<%=hdnCheckOutToTime.ClientID %>').val();
        var ActualTimeOut = $('#<%=hdnCheckOutActualTimeout.ClientID %>').val();
        var meetingstarttime = (new Date(Startdate.split('-') + ' ' +
       StartTime.split(' ')[0]).toString().split(' '))
        var meetingendtime = (new Date(Enddate.split('-') + ' ' +
       EndTime.split(' ')[0]).toString().split(' '))
        var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
        meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
        var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
        meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');

        var actualoutTime = (new Date(Enddate.split('-') + ' ' + ActualTimeOut.split(' ')[0]).toString().split(' '))
        var tempactualoutTime = gettimedtls(actualoutTime[1], actualoutTime[3], hours, mins).split('|');
        actualoutTime == (new Date(actualoutTime[5], tempactualoutTime[0], actualoutTime[2], tempactualoutTime[2], tempactualoutTime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');

        $('#<%=hdnCheckOutFromDate.ClientID %>').val(new Date(meetingstarttime).format("dd/MM/yyyy"));
        $('#<%=hdnCheckOutToDate.ClientID %>').val(new Date(meetingendtime).format("dd/MM/yyyy"));
        var newStartTime = (new Date(Startdate.split('-') + ' ' + StartTime)).toString().split(' ')
        $('#<%=hdnCheckOutFromTime.ClientID %>').val(newStartTime[3])
        var newEndTime = (new Date(Enddate.split('-') + ' ' + meetingendtime[3])).toString().split(' ')
        $('#<%=hdnCheckOutToTime.ClientID %>').val(newEndTime[3])

        var newOutTime = (new Date(Enddate.split('-') + ' ' + actualoutTime[3])).toString().split(' ')
        $('#<%=hdnCheckOutActualTimeout.ClientID %>').val(newOutTime[3])

    }

</script>
<style type="text/css">
    .ModalWindow
    {
        border: 1px solid #c0c0c0;
        background: #EEEEEE;
        padding: 0px 0px 0px 0px;
        width: 500px;
        height: auto;
    }
    .modalBackground
    {
        background-color: #878776;
        filter: alpha(opacity=40);
        opacity: 0.5;
    }
    p.MsoNormal
    {
        margin-top: 1.3pt;
        margin-right: 5.75pt;
        margin-bottom: 12.0pt;
        margin-left: 0in;
        line-height: 12.0pt;
        font-size: 10.0pt;
        font-family: "Arial" , "sans-serif";
    }
    
    .gridStyle
    {
    }
    .gridStyle th
    {
        background-color: #3188B5;
        font-family: Verdana;
        border-color: Black;
        color: White;
    }
    .style1
    {
        width: 19px;
    }
</style>
<form>
<table class="tblHeadStyle" width="100%">
    <tr>
        <td colspan="7">
            <div style="height: 10px">
            </div>
        </td>
    </tr>
    <tr>
        <td align="right" class="tdBold" width="15%" style="white-space: nowrap; padding-left: 20px">
            <asp:TextBox ID="txtExpress" runat="server" CssClass="txtField" 
                onkeypress="return onlyNumbers()" Width="150px" MaxLength="20"></asp:TextBox>
            <cc1:textboxwatermarkextender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtExpress"
                WatermarkText="<%$ Resources:LocalizedText, lblSafetyPermitId %>" >
            </cc1:textboxwatermarkextender>
            <span class="field_txt" style="white-space: nowrap; padding-left: 20px">
            <asp:label Id="lblSearchOR"   Text="<%$ Resources:LocalizedText, lblSearchOR %>" runat="server" CssClass="field_txt"></asp:label>
            </span>
        </td>
        <td width="10%" style="white-space: nowrap; padding-left: 20px">
            <asp:TextBox ID="txtSearch" runat="server" onkeyup="avoidSpecialCharacters(this)"
                CssClass="txtField" Width="350px" MaxLength="30"></asp:TextBox>
            <cc1:textboxwatermarkextender ID="TBWE2" runat="server" TargetControlID="txtSearch"
             WatermarkText="<%$ Resources:LocalizedText, lblSearchSafetyPermit %>">
            </cc1:textboxwatermarkextender>
        </td>
        <td align="right" width="5%" style="white-space: nowrap">
            <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:LocalizedText, FromDate %>" CssClass="field_txt" 
                Height="16px" Width="50px"></asp:Label>
        </td>
        <td width="15%" style="white-space: nowrap">
            <asp:TextBox ID="txtFromDate" runat="server" 
                 CssClass="txtField" onkeydown="return false;"></asp:TextBox>
            <asp:ImageButton ID="imgFromDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                runat="server" Width="15px" ImageAlign="Middle" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" />
            <cc1:calendarextender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                PopupButtonID="imgFromDate" Format="MM/dd/yyyy" 
                PopupPosition="BottomRight">
            </cc1:calendarextender>
            <%--<cc1:MaskedEditExtender ID="MaskedEditExtenderFromDate" ClearMaskOnLostFocus="false"
                InputDirection="RightToLeft" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999"
                MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                MaskType="Date" ErrorTooltipEnabled="True" UserDateFormat="None" UserTimeFormat="None" />--%>
            <asp:ImageButton ID="imbRefreshStartdate" runat="server" ImageUrl="~/Images/Refresh_image.gif"
                OnClientClick="javascript:fnStartDateRefresh();" Style="cursor: hand" 
              alt="<%$ Resources:LocalizedText, RefreshDate %>" />
        </td>
        <td align="right" width="5%" style="white-space: nowrap">
            <asp:Label ID="lblToDate" runat="server" CssClass="field_txt" Text="<%$ Resources:LocalizedText, ToDate %>"  Height="16px"
                Width="50px"></asp:Label>
        </td>
        <td width="15%" style="white-space: nowrap">
            <asp:TextBox ID="txtToDate" runat="server" CssClass="txtField" onkeydown="return false;"></asp:TextBox>
            <asp:ImageButton ID="imgToDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                runat="server" Width="15px" ImageAlign="Middle"  ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>"/>
            <cc1:calendarextender ID="ToDateCalendar" runat="server" TargetControlID="txtToDate"
                PopupButtonID="imgToDate" Format="MM/dd/yyyy" PopupPosition="BottomRight">
            </cc1:calendarextender>
            <asp:ImageButton ID="imbRefreshEnddate" runat="server" ImageUrl="~/Images/Refresh_image.gif"
                OnClientClick="javascript:fnEndDateRefresh();" Style="cursor: hand" alt="<%$ Resources:LocalizedText, RefreshDate %>"  />
         <%--   <cc1:MaskedEditExtender ID="MaskedEditExtenderToDate" InputDirection="RightToLeft"
                ClearMaskOnLostFocus="false" runat="server" TargetControlID="txtToDate" Mask="99/99/9999"
                MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                MaskType="Date" ErrorTooltipEnabled="True" />--%>
        </td>
        <td width="10%">
            <asp:DropDownList ID="ddlReqStatus" runat="server" Style="width: auto; min-width: 100px;height: 20px">
              <asp:ListItem Text="<%$ Resources:LocalizedText, SelectStatus %>" Value="Select Status"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:LocalizedText, Yettoarrive %>" Value="Yet to arrive"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:LocalizedText, In %>" Value="In"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:LocalizedText, Out %>" Value="Out"></asp:ListItem>
                                             
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
    <td colspan="7">
        &nbsp;
    </td></tr>
    <tr>
        <td colspan="7" align="center">
            <table style="width: 586px">
                <tr align="center">
                    <td width="100px">
                        <asp:Button ID="btnSearch" runat="server" OnClick="BtnSearch_Click" Text="<%$ Resources:LocalizedText, Search %>"
                            Width="95px" CssClass="cssButton" />
                    </td>
                    <td width="100px">
                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:LocalizedText, Clear %>" OnClick="BtnReset_Click" CausesValidation="False"
                            Width="95px" CssClass="cssButton" />
                    </td>
                    <td colspan="2" width="350px" align="center">
                    <%--<asp:ImageButton ImageUrl="~/Images/excel_icon.gif" ID="btnExport" runat="server"
                        Text="<%$ Resources:LocalizedText, btnExport %>" Tooltip = "Export to Excel" OnClick="btnExport_Click" Width="40px" />--%>
                        <asp:Panel ID="pnlGotoVisitors" runat="server">
                           <%-- <asp:Label runat="server" Id="lblNoRelevantRequest"  Text="<%$ Resources:LocalizedText, lblNoRelevantRequest %>"  CssClass="field_txt"></asp:Label>--%>
                              <%--  <asp:LinkButton ID="btnAddNew" runat="server"  Text="<%$ Resources:LocalizedText, lblClickHere %>" CssClass="LinkButton"
                                    Style="margin-left: 10px" Visible="True" OnClick="btnAddNew_Click"></asp:LinkButton>--%>
                        </asp:Panel>
                        <asp:Panel ID="pnlGotoRequests" runat="server">
                           <%-- <asp:LinkButton ID="btnBackToRequests" 
                                Text="<%$ Resources:LocalizedText, btnBackToRequests %>"  runat="server" CssClass="LinkButton"
                                Width="330px" Visible="True" OnClick="btnBackToRequests_Click"></asp:LinkButton>--%>
                        </asp:Panel>
                    </td>
                    <td width="20px">
                        <asp:ImageButton ImageUrl="~/Images/Excel.jpg" ID="btnExport" runat="server"
                        Text="<%$ Resources:LocalizedText, btnExport %>"  Tooltip = "Export to Excel" OnClick="BtnExport_Click" Width="25px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>   
    <tr>
        <td align="right" colspan="7">
            <div style="height: 10px">
            <%--<tr>
            <td colspan="7" align="left">
            <asp:Label ID="lblSP" runat="server" CssClass="errorLabel" Text = "Safety Permit"></asp:Label>
        </td>
        </tr>--%>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="7" align="center">
            <asp:Label ID="lblError" runat="server" CssClass="errorLabel" Visible="false"></asp:Label>
        </td>
    </tr>
</table>
<div style="height: 7px">
</div>
<hr width="100%" />
<div style="height: 7px">
</div>
<table border="0" id="errortbl" cellpadding="0" cellspacing="0" width="100%" runat="server"
    visible="false">
    <tr>
        <td align="center" class="style6" bgcolor="White">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Label ID="lblResult" runat="server" ForeColor="Red" Font-Bold="True" CssClass="cssDisplay"
                Visible="False"></asp:Label>
            <asp:Label ID="lblStatusResult" runat="server" ForeColor="Green" Font-Bold="true"
                CssClass="cssDisplay"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:HiddenField ID="hdnCurrentTime" runat="server" />
        </td>
    </tr>
</table>
<center>
    <asp:Panel ID="pnlRequests" runat="server">
        <asp:Panel ID="pnlImage" runat="server" HorizontalAlign="Left">
        <table>
                <tr>
                    <td align="left" class="field_txt" style="font-weight: bold"> &nbsp;
                    <asp:label Id="lblLegend" runat="server" Text="<%$ Resources:LocalizedText, lblLegend %>"  CssClass="field_txt"></asp:label>

                    </td>
                    <td align="left">
                        <asp:Image ID="image" runat="server" ImageUrl="~/Images/Black.jpg" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblBlack" runat="server" Text="<%$ Resources:LocalizedText, Yettoarrive %>" CssClass="field_txt"></asp:Label>
                    </td>
                    <td>
                        <asp:Image ID="imgAmber" runat="server" ImageUrl="~/images/Amber1.jpg" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblAmber" Text="<%$ Resources:LocalizedText, lblAmber %>"  runat="server" CssClass="field_txt"></asp:Label>
                    </td>
                    <td>
                        <asp:Image ID="imgGreen" runat="server" ImageUrl="~/images/Green.jpg" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblGreen" runat="server" Text="<%$ Resources:LocalizedText, lblGreen %>" CssClass="field_txt"></asp:Label>
                    </td>
                    <td>
                        <asp:Image ID="imgRed" runat="server" ImageUrl="~/images/Reddishblink.gif" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblRed" runat="server"  Text="<%$ Resources:LocalizedText, lblExceedOutTime %>"  CssClass="field_txt"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table width="98%">
            <tr>
                <td>
                    <div style="height: 100%; width: 100%;">
                        <asp:Timer ID="TMRefresh" runat="server" Interval="300000" OnTick="RefreshGrid_Tick">
                        </asp:Timer>
                    </div>
                     <asp:Panel ID="pnlResult" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TMRefresh" EventName="Tick" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:GridView ID="grdResult" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="False"
                                    Font-Names="Verdana" Font-Size="X-Small" DataKeyNames="RequestID" OnRowCommand="GrdResult_RowCommand"
                                    CssClass="gridStyle" HeaderStyle-Wrap="True" PageSize="25" AllowPaging="False" width="100%"
                                    OnRowDataBound="Grdresult_RowDataBound" GridLines="Vertical">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, SlNo %>"  ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ControlStyle Height="10px" Width="10px" />
                                            <ControlStyle Height="10px" Width="10px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Safety Permit ID:"  DataField="ExternalRequestId" ItemStyle-Width="30px" />
                                        <asp:BoundField HeaderText="Request ID:"   DataField="PermitId" ItemStyle-Width="30px"/>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Status %>"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <%-- Bugfix - Nightshift visitor start--%>
                                                <asp:Image ID="image" runat="server" ImageUrl="~/Images/Black.jpg" Width="10" />
                                                <%--'<%# GetImageUrl(DataBinder.Eval(Container.DataItem, "intime", "{0:d}") as string, DataBinder.Eval(Container.DataItem, "ExpectedOutTime", "{0:d}") as string,Eval("RequestStatus")as string) %>'--%>
                                                <%-- Bugfix - Nightshift visitor end--%>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>                                    
                                        <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, Name %>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVName" runat="server" Text='<%# Highlight(Eval("Name").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, Company %>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVCompany" runat="server" Text='<%# Highlight(Eval("Company").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, Mobile %>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVMobile" runat="server" Text='<%# Highlight(Eval("MobileNo").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:BoundField  HeaderText="<%$ Resources:LocalizedText, Host %>" DataField="Host" ItemStyle-Width="150px" />
                                        
                                        <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, Date %>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <label id="startdate">
                                                    <%# Eval("Date", "{0:dd/MMM/yyyy}") %></label>
                                                <%--    <asp:Label CssClass="cssDate" ID="lblDate" runat="server"  Text='<%# Bind("Date", "{0:dd/MM/yyyy }") %>' />
                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, InTime %>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <label id="starttime">
                                                    <%#Eval("intime")%></label>
                                                <%--    <asp:Label ID="lblinTime" CssClass="cssTime" runat="server"  Text='<%# Bind("intime", "{0:HH:mm:ss }") %>' />
                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--  <asp:BoundField HeaderText="Date" DataField="Date" DataFormatString="{0:dd-MMM-yyyy}"
                                            ItemStyle-Width="75px" />--%>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, lblExpectedOutTime %>"  ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <label id="expectedouttime">
                                                    <%#Eval("Expectedouttime")%></label>
                                                <%--    <asp:Label ID="lblinTime" CssClass="cssTime" runat="server"  Text='<%# Bind("intime", "{0:HH:mm:ss }") %>' />
                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, ActualOutTime %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <label id="actualouttime">
                                                    <%#Eval("ActualOutTime")%></label>
                                                <%--    <asp:Label ID="lblinTime" CssClass="cssTime" runat="server"  Text='<%# Bind("intime", "{0:HH:mm:ss }") %>' />
                                                --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField HeaderText="Actual Out Time" DataField="ActualOutTime" DataFormatString="{0:d-MMM-yyyy HH:mm}"
                                            HeaderStyle-Wrap="true" ItemStyle-Width="50px" />--%>
                                        <asp:BoundField  HeaderText="<%$ Resources:LocalizedText, BadgeNo %>" DataField="BadgeNo" />
                                        
                                          <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, BadgeStatus %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="75px">
                                            <ItemTemplate>
                                            <asp:Label ID="lblBadgeStatus" runat="server" Text='<%# Highlight(Eval("BadgeStatus").ToString()) %>' />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, BadgeStatusUpdate %>" ItemStyle-HorizontalAlign="Center"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Button ID="btnBgStatus" runat="server" Text="<%$ Resources:LocalizedText, Update %>" CommandName="FindValue"
                                                    CommandArgument='<%#Eval("VisitDetailsID") %>' CssClass="cssButton" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, Actions %>" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="220px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnCheckIn" runat="server" Text="<%$ Resources:LocalizedText, CheckIn %>"  CommandArgument='<%#Eval("VisitDetailsID")%>'
                                                    CommandName="CheckIn" CssClass="GridLinkButton" />
                                                <asp:LinkButton ID="btnCheckOut" runat="server" Text="<%$ Resources:LocalizedText, CheckOut %>" CommandArgument='<%#Eval("VisitDetailsID")%>'
                                                    CommandName="CheckOut" CssClass="GridLinkButton" />
                                                <asp:LinkButton ID="btnLost" runat="server" Text="<%$ Resources:LocalizedText, Lost %>"  CommandArgument='<%#Eval("VisitDetailsID")%>'
                                                    CommandName="Lost" CssClass="GridLinkButton" />
                                                <asp:LinkButton ID="btnPrint" runat="server" Text="<%$ Resources:LocalizedText, Reprint %>" CommandArgument='<%#Eval("VisitDetailsID")%>'
                                                    CommandName="RePrint" CssClass="GridLinkButton" />
                                                <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:LocalizedText, btnView %>"  CommandArgument='<%#Eval("VisitDetailsID")%>'
                                                    CommandName="ViewDetailsLink" CssClass="GridLinkButton" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, ViewDetails %>" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnView" ImageUrl="~/Images/Search_1.png" Height="20px" runat="server"
                                                    CommandArgument='<%#Eval("VisitDetailsID")%>' CommandName="ViewDetailsLink" />
                                                <asp:HiddenField ID="hdnViewDetailsID" runat="server" />
                                                <asp:HiddenField ID="hdnBadgeStatus" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Comments %>" ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVisitorId" Text='<%#Eval("VisitorID") %>' runat="server"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtComments" Width="85px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle CssClass="even_row" />
                                    <AlternatingRowStyle CssClass="odd_row" />
                                </asp:GridView>
                                 
                                     <asp:HiddenField ID="hdnCheckOutFromDate" runat="server" />
                    <asp:HiddenField ID="hdnCheckOutToDate" runat="server" />
                    <asp:HiddenField ID="hdnCheckOutFromTime" runat="server" />
                    <asp:HiddenField ID="hdnCheckOutToTime" runat="server" />
                    <asp:HiddenField ID="hdnCheckOutActualTimeout" runat="server" />
                    </ContentTemplate></asp:UpdatePanel>                 
                    </asp:Panel>                   
                    <asp:HiddenField ID="hdnDetailsID" runat="server" />
                    <cc1:ModalPopupExtender ID="modalReprintComments" BackgroundCssClass="modalBackground"
                        TargetControlID="hdnDetailsID" PopupControlID="pnlReprintComments" CancelControlID="imgClose"
                        runat="server" X="-1" Y="-1" DropShadow="true">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="pnlReprintComments" runat="server" Width="300px" Height="125px" Style="display: none"
                        CssClass="ModalWindow">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr style="background: #EDF6E3; height: 25px">
                                <td class="header_txt" valign="top" align="left">
                                <br />
                                &nbsp;&nbsp;
                                <asp:Label Id="lblReprintComment" Text="<%$ Resources:LocalizedText, SelectReason %>"  runat="server"></asp:Label>                                   
                                </td>
                                <td width="16px" valign="top">
                                    <div style="margin-top: 2px; float: right; margin-right: 2px;">
                                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/Close.png" Height="14px" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    &nbsp;
                                    <asp:DropDownList ID="ddlReason" runat="server" Width="200px">
                                     <asp:ListItem Text="<%$ Resources:LocalizedText, Lost %>" selected="true" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:LocalizedText, PrinterJammed %>" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;<asp:Button ID="btnPrint" CssClass="cssButton" runat="server" Text="<%$ Resources:LocalizedText, Print %>"
                                        OnClick="ButnPrint_Click" ValidationGroup="validReason" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    &nbsp;
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="<%$ Resources:LocalizedText, SelectReason %>"
                                        CssClass="errorLabel" ControlToValidate="ddlReason" MinimumValue="1" MaximumValue="5"
                                        Type="Integer" ValidationGroup="validReason"></asp:RangeValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Button ID="btnHidden" runat="server" Text="Button" Style="display: none" OnClick="BtnHidden_Click" />   
                    <asp:Button ID="btnHidden1" runat="server" Text="Button" Style="display: none" OnClick="BtnHidden1_Click" />                          
                </td>
            </tr>
        </table>
    </asp:Panel>
</center>
<table width="98%" style="margin-left: 50px">
    <tr>
        <td>
            <div style="height: 100%; width: 100%;">
                <asp:SqlDataSource ID="dsSearchResult" runat="server" ConnectionString="<%$ ConnectionStrings:VMSConnectionString %>"
                    SelectCommand="SearchMasterDetails" SelectCommandType="StoredProcedure" OnSelected="GrdVisitor_Selected">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearch" DefaultValue=" " Name="Search" PropertyName="Text"
                            Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>        
            </td></tr>
</table>
<asp:HiddenField ID="hfGvPanel" runat="server" />
<asp:HiddenField ID="hdnRecordFound" runat="server" />
<asp:HiddenField ID="hdnVisitDetailId" runat="server" />
<asp:HiddenField ID="hfGvStatus" runat="server" />
<asp:HiddenField ID="hdnCurrentDate" runat="server" />
                                 
<div style="visibility: hidden">
    <asp:CheckBox ID="hfSearch" runat="server" Visible="true" /></div>
<%--<asp:HiddenField ID="hdnSearchText" runat="server" />--%>
</form>
</asp:Content>
