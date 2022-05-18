<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmergencyContactInformation.ascx.cs" Inherits="VMSDev.UserControls.EmergencyContactInformation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<script language="javascript" type="text/javascript">
    function SetEmergencyDefaultTime() {
        var fromNow = new Date();
        var minutes = 5;
        var hour = 3
        fromNow.setMinutes(fromNow.getMinutes() + minutes);
        var currenthour = fromNow.getHours();
        var currentminute = fromNow.getMinutes();
        var fromtime = currenthour + ":" + currentminute;
        var toNow = new Date();
        toNow.setHours(toNow.getHours() + hour);
        toNow.setMinutes(toNow.getMinutes() + minutes);
        var tohour = toNow.getHours();
        var tominute = toNow.getMinutes();
        var totime = tohour + ":" + tominute;
        var now = new Date();
        var emergencyhour = now.getHours();
        var emergencytminute = now.getMinutes();
        var emergencyTime = emergencyhour + ":" + emergencytminute;
        $('#<%=hdnFromTime.ClientID %>').val(emergencyTime);
        var formatFromDate = fromNow.format("dd/MMM/yyyy");
        $('#<%=txtVisitingFromDate.ClientID %>').val(formatFromDate);
        var formatToDate = toNow.format("dd/MMM/yyyy");
        $('#<%=txtVisitingToDate.ClientID %>').val(formatToDate);

    }

    function GetEmergencyTimeAfterValidations() {

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
        var Startdate = $('#<%=hdnFromEmergency.ClientID %>').val();
        var Enddate = $('#<%=hdnToEmergency.ClientID %>').val()

        var StartTime = $('#<%=hdnFromDateTime.ClientID %>').val();
        var EndTime = $('#<%=hdnToDateTime.ClientID %>').val();


        $('#<%=txtVisitingFromDate.ClientID %>').val(moment(Startdate).format("dd/MMM/yyyy"));
        $('#<%=txtVisitingToDate.ClientID %>').val(moment(Startdate).format("dd/MMM/yyyy"));

    }

    function CurrentEmergencyShowing(e) {
        if (!e.get_selectedDate() || !e.get_element().value)
            e._selectedDate = (new Date()).getDateOnly();
    }

    function SetEmergencyTimeByRequestId() {

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

        var Startdate = $('#<%=txtVisitingFromDate.ClientID %>').val();
        var Enddate = $('#<%=txtVisitingToDate.ClientID %>').val();

        var StartTime = $('#<%=hdnFromDateTime.ClientID %>').val();
        var EndTime = $('#<%=hdnToDateTime.ClientID %>').val();

        var meetingstarttime = (new Date(Startdate.split('-') + ' ' +
            StartTime.split(' ')[0]).toString().split(' '))
        var meetingendtime = (new Date(Enddate.split('-') + ' ' +
            EndTime.split(' ')[0]).toString().split(' '))
        var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
        meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
        var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
        meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
        <%--$('#<%=txtVisitingFromDate.ClientID %>').val(new Date(meetingstarttime).format("dd/MMM/yyyy"));
        $('#<%=txtVisitingToDate.ClientID %>').val(new Date(meetingendtime).format("dd/MMM/yyyy"));
        $('#<%=hdnFromEmergency.ClientID %>').val(new Date(meetingstarttime).format("dd/MM/yyyy"));--%>
        $('#<%=hdnToEmergency.ClientID %>').val(new Date(meetingendtime).format("dd/MM/yyyy"));
    }

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
        <td colspan="7" align="left">
            <asp:Label ID="lblEmergencyContact" runat="server" CssClass="lblHeada" Text="<%$ Resources:LocalizedText, lblEmergencyContact %>"></asp:Label>
        </td>
    </tr>
    <tr>

        <td style="width: 15px" valign="left"></td>
        <td style="width: 135px" align="right" class="tdBold">
            <asp:Label ID="lblContactAddress" runat="server" Text="<%$ Resources:LocalizedText, ContactAddress %>" CssClass="lblField"></asp:Label>&nbsp;                               
        </td>
        <td align="left" style="width: 70px">
            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox>
        </td>
        <td align="right" class="tdBold" valign="middle">
            <asp:Label ID="lblVisitingFromDate" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, FromDate %>"></asp:Label>
        </td>

        <td align="left" valign="middle">
            <asp:TextBox ID="txtVisitingFromDate" runat="server" CssClass="txtField"
                OnPaste="false" Width="70px"></asp:TextBox>&nbsp;
                <asp:ImageButton ID="imgFromDate" ImageUrl="~/Images/calender-icon.png" runat="server" Width="15px" ImageAlign="Middle" CausesValidation="false" ToolTip="To View the calender control" />
            <cc2:CalendarExtender ID="FromDateCalendar" OnClientShowing="CurrentEmergencyShowing"
                runat="server" TargetControlID="txtVisitingFromDate" Format="dd/MMM/yyyy" PopupButtonID="imgFromDate" PopupPosition="BottomLeft" EnableViewState="false">
            </cc2:CalendarExtender>

        </td>

        <td align="right" class="tdBold">

            <asp:Label ID="lblToDate" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, ToDate %>"></asp:Label>
        </td>
        <td align="left">
            <asp:TextBox ID="txtVisitingToDate" runat="server" OnPaste="false" CssClass="txtField" Width="70px"></asp:TextBox>&nbsp;
            <asp:ImageButton ID="imgToDate" ImageUrl="~/Images/calender-icon.png" runat="server" Width="15px" ImageAlign="Middle" CausesValidation="false" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" />
            <cc2:CalendarExtender ID="ToDateCalendar" runat="server"
                TargetControlID="txtVisitingToDate" OnClientShowing="CurrentEmergencyShowing" Format="dd/MMM/yyyy" PopupButtonID="imgToDate"
                PopupPosition="BottomLeft" EnableViewState="false">
            </cc2:CalendarExtender>
        </td>


    </tr>
</table>
<asp:HiddenField ID="hdnFromTime" runat="server" />
<asp:HiddenField ID="hdnToDateTime" runat="server" />
<asp:HiddenField ID="hdnFromDateTime" runat="server" />
<asp:HiddenField ID="hdnFromEmergency" runat="server" />
<asp:HiddenField ID="hdnToEmergency" runat="server" />
