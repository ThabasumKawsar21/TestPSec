<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitorLocationInformationSP.ascx.cs"
    Inherits="VMSDev.SafetyPermitUserControls.VisitorLocationInformationSP" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc5" %>

<script src="../Scripts/jquery-3.4.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
    function pageload() {
        GetOffsetTimeLocation();
    }
    function SetDefaultTime() {
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
        $('#<%=txtFromTime.ClientID %>').val(fromtime);
        $('#<%=txtToTime.ClientID %>').val(totime);
        var now = new Date();
        var formatFromDate = fromNow.format("dd/MMM/yyyy");
        $('#<%=txtFromDate.ClientID %>').val(formatFromDate);
        var formatToDate = toNow.format("dd/MMM/yyyy");
        $('#<%=txtToDate.ClientID %>').val(formatToDate);

        $('#<%=hdnFromDate.ClientID %>').val(fromNow.format("MM/dd/yyyy"))
        $('#<%=hdnToDate.ClientID %>').val(toNow.format("MM/dd/yyyy"))
        $('#<%=hdnFromTime.ClientID %>').val(fromtime)
        $('#<%=hdnToTime.ClientID %>').val(totime)

    }
    function CurrentDateShowing(e) {
        if (!e.get_selectedDate() || !e.get_element().value)
            e._selectedDate = (new Date()).getDateOnly();
    }
    function GetOffsetTimeLocation() {
        var CurrentDate = new Date();
        var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
        VisitorLocationInformationSP.AssignTimeZoneOffsetLocation(CurrentDate.getTimezoneOffset());
        VisitorLocationInformationSP.AssignCurrentDateTimeLocation(today);
    }
    function GetExtendedTime() {

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
        var Enddate = $('#<%=hdnToDate.ClientID %>').val()
        var EndTime = $('#<%=txtToTime.ClientID %>').val()
        var newEndTime = (new Date(Enddate.split('-') + ' ' + EndTime)).toString().split(' ')
        $('#<%=hdnToTime.ClientID %>').val(newEndTime[3])
    }

    function GetVisitTime() {
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
        var Startdate = $('#<%=txtFromDate.ClientID %>').val();
        var StartTime = $('#<%=txtFromTime.ClientID %>').val();
        var Enddate = $('#<%=txtToDate.ClientID %>').val()
        var EndTime = $('#<%=txtToTime.ClientID %>').val()
        var meetingstarttime = (new Date(Startdate.split('-') + ' ' +
         StartTime.split(' ')[0]).toString().split(' '))
        var meetingendtime = (new Date(Enddate.split('-') + ' ' +
         EndTime.split(' ')[0]).toString().split(' '))
        var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
        meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
        var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
        meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
        $('#<%=txtFromDate.ClientID %>').val(new Date(meetingstarttime).format("dd/MMM/yyyy"));
        $('#<%=txtToDate.ClientID %>').val(new Date(meetingendtime).format("dd/MMM/yyyy"));
        var newStartTime = (new Date(Startdate.split('-') + ' ' + meetingstarttime[3])).toString().split(' ')
        var newEndTime = (new Date(Enddate.split('-') + ' ' + meetingendtime[3])).toString().split(' ')
        $('#<%=txtFromTime.ClientID %>').val(meetingstarttime[3])
        $('#<%=txtToTime.ClientID %>').val(meetingendtime[3])
        $('#<%=hdnFromDate.ClientID %>').val(new Date(meetingstarttime).format("MM/dd/yyyy"))
        $('#<%=hdnToDate.ClientID %>').val(new Date(meetingendtime).format("MM/dd/yyyy"))
        $('#<%=hdnFromTime.ClientID %>').val(newStartTime[3])
        $('#<%=hdnToTime.ClientID %>').val(newEndTime[3])
        $('#<%=hdnToTimeBeforeExtended.ClientID %>').val(newEndTime[3])

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

    function GetTimeAfterValidations() {
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
        var Startdate = $('#<%=hdnFromDate.ClientID %>').val();
        var StartTime = $('#<%=hdnFromTime.ClientID %>').val();
        var Enddate = $('#<%=hdnToDate.ClientID %>').val()
        var EndTime = $('#<%=hdnToTime.ClientID %>').val()
        var meetingstarttime = (new Date(Startdate.split('-') + ' ' +
       StartTime.split(' ')[0]).toString().split(' '))
        var meetingendtime = (new Date(Enddate.split('-') + ' ' +
       EndTime.split(' ')[0]).toString().split(' '))
        var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
        meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
        var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
        meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
        $('#<%=txtFromDate.ClientID %>').val(new Date(meetingstarttime).format("dd/MMM/yyyy"));
        $('#<%=txtToDate.ClientID %>').val(new Date(meetingendtime).format("dd/MMM/yyyy"));
        var newStartTime = (new Date(Startdate.split('-') + ' ' + StartTime)).toString().split(' ')
        $('#<%=txtFromTime.ClientID %>').val(newStartTime[3])
    }


</script>
<script language="javascript" type="text/javascript">

    function allowNo(ie, moz) {

        if (moz != null) {
            //alert(moz);
            if ((moz >= 48) && (moz < 58) || moz == 8 || moz == 13) {
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

    function PopUp() {

        var Result = window.showModalDialog('SearchUserDetails.aspx?escort', 'SearchDetails', 'status:no;dialogWidth:425px;dialogHeight:425px;dialogHide:true;help:no;');


        if (Result != "undefined") {
            var txtEscort = document.getElementById('ctl00_VMSContentPlaceHolder_VisitorLocationInformationControl_txtEscort');
            res = Result.split("%$%");
            var res1 = new Array();
            var res2 = new Array();
            if (res[0] != "") {
                res1 = res[0].split("(");
                res2 = res1[0].split(",");
                txtEscort.value = res2[1] + "," + " " + res2[0] + "(" + res1[1];;

            }
            else {

                txtEscort.value = "";
            }



            if (res[1] != "") {

                var myHiddenVar = document.getElementById('ctl00_VMSContentPlaceHolder_VisitorLocationInformationControl_myHiddenVar');
                myHiddenVar.value = "";
                myHiddenVar.value = res[1];
                // txtHostContactNo.disabled = true;
            }
        }
    }
    function PopUpHost() {
        var Result = window.showModalDialog('SearchUserDetails.aspx', 'SearchHost', 'status:no;dialogWidth:425px;dialogHeight:425px;dialogHide:true;help:no;');
        //for testing defects VMS_CR06_08 of VMS06072010CR09
        //         var ddlPurpose= document.getElementById('ctl00_VMSContentPlaceHolder_VisitorLocationInformationControl_ddlPurpose');
        //         ddlPurpose.

        if (Result != "undefined") {
            var res = new Array();
            var res1 = new Array();
            var res2 = new Array();
            res = Result.split("%$%");


            var txtHost = document.getElementById('ctl00_VMSContentPlaceHolder_VisitorLocationInformationControl_txtHost');
            var txtHostContactNo = document.getElementById('ctl00_VMSContentPlaceHolder_VisitorLocationInformationControl_txtHostContactNo');
            res1 = res[0].split("(");
            res2 = res1[0].split(",");



            txtHost.value = res2[1] + "," + " " + res2[0] + "(" + res1[1];
            if (res[1] != "") {
                txtHostContactNo.value = "";
                txtHostContactNo.value = res[1];
                // txtHostContactNo.disabled = true;
            }
            else
                txtHostContactNo.disabled = false;

        }

    }

    function ChkLength() {
        var s = document.getElementById("txtEscort").text;
        if (s.length < 6) {
            alert("Used Id should be 6 digits");
            return false;
        }
        else {
            return true;
        }
    }






    function ViewOtherTxtBoxControl(obj) {
        var txtOtherControlID = obj.id.replace('ddlPurpose', 'txtPurpose');
        var txtOtherControl = document.getElementById(txtOtherControlID)
        if (obj.innerText = "Others")
            txtOtherControl.style.visibility = "visible";
        else
            txtOtherControl.style.visibility = "hidden";
    }
</script>
<style type="text/css">
    .style19
    {
        text-align: left;
     
    }
    .style3
    {
        width: 162px;
    }
    .style4
    {
        width: 95px;
    }
    .style5
    {
        width: 189px;
    }
    .style6
    {
        width: 200px;
    }
    .style7
    {
        width: 168px;
        white-space: nowrap;
    }
</style>
<div class="style19">
    <asp:updatepanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <table class="tblHeadStyle" width="100%" style="white-space: nowrap">
                <tr>
                    <td colspan="7" align="left">
                        <asp:Label ID="lblVisitorLocation" runat="server" CssClass="lblHeada" Text="<%$ Resources:LocalizedText, VisitLocationInformation %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="5" style="width: 20px;">
                    </td>
                    <td align="right" class="style3">
                        <asp:Label ID="lbCountry" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, Country %>"></asp:Label>
                        &nbsp;<asp:Label ID="lblRequiredCountry" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                    </td>
                    <td align="left" class="style7">
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlCountry_SelectedIndexChanged"
                            CssClass="ddlField" Width="120px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="Requiredcountry1" runat="server" ControlToValidate="ddlCountry"
                            InitialValue="0" ErrorMessage="<%$ Resources:LocalizedText, SelectCountry %>" Display="None"></asp:RequiredFieldValidator>
                        <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="Requiredcountry1">
                        </cc5:ValidatorCalloutExtender>
                    </td>
                    <td align="right" class="tdBold">
                        <asp:Label ID="lblCity" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, City %>"></asp:Label>
                        &nbsp;<asp:Label ID="lblRequiredCity" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="True" CssClass="ddlField"
                            OnSelectedIndexChanged="DdlCity_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredCity" runat="server" ControlToValidate="ddlCity"
                            InitialValue="0" ErrorMessage="<%$ Resources:LocalizedText, SelectCity %>" Display="None"></asp:RequiredFieldValidator>
                        <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredCity">
                        </cc5:ValidatorCalloutExtender>
                    </td>
                    <td align="right" class="style5">
                        <asp:Label ID="lblFacility" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, Facility %>"></asp:Label>
                        &nbsp;<asp:Label ID="lblRequiredFacility" runat="server" CssClass="lblRequired" Text="*"
                            Visible="false"></asp:Label>&nbsp;
                    </td>
                    <td align="left" class="style6">
                        <asp:DropDownList ID="ddlFacility" runat="server" CssClass="ddlField">
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="tdBold">
                        <%--Changes done for VMS CR VMS06072010CR09 by Priti--%>
                        <asp:Label ID="lblPurpose" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, VisitorType %>"></asp:Label>
                        <%-- end Changes done for VMS CR VMS06072010CR09 by Priti    --%>
                        <asp:Label ID="lblRequiredPurpose" runat="server" CssClass="lblRequired" Text="*"></asp:Label>
                        <asp:Label ID="lblOtherPurpose" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, Others %>"></asp:Label>
                        &nbsp;
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlPurpose" CssClass="ddlField" runat="server" OnSelectedIndexChanged="DdlPurpose_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:XmlDataSource ID="XmlPurpose" runat="server"></asp:XmlDataSource>
                        <%--for testing defects VMS_CR02_04 of VMS06072010CR09    --%>
                        <asp:RequiredFieldValidator ID="Requiredpurpose" runat="server" ControlToValidate="ddlPurpose"
                            InitialValue="Select" ErrorMessage="<%$ Resources:LocalizedText, SelectVisitorType %>" Display="None"></asp:RequiredFieldValidator>
                        <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" TargetControlID="Requiredpurpose">
                        </cc5:ValidatorCalloutExtender>
                        <asp:TextBox ID="txtPurpose" runat="server" CssClass="txtField" Visible="false" MaxLength="100"></asp:TextBox>
                    </td>
                    <tr>
                        <td align="right" class="style3">
                            <asp:Label ID="lblFromDate" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, FromDate %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblFromDateRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left" class="style7">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtField" Width="75px"
                                ></asp:TextBox>
                            <asp:ImageButton ID="imgFromDate" ImageUrl="~/Images/calender-icon.png" runat="server"
                                Width="15px" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" ImageAlign="Middle" CausesValidation="false" />
                            <cc5:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                                PopupButtonID="imgFromDate" Format="dd/MMM/yyyy"  PopupPosition="BottomRight" EnableViewState="false">
                            </cc5:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFD" runat="server" ClearMaskOnLostFocus="false"
                                ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:LocalizedText, RequiredFromDate %>" Display="None"></asp:RequiredFieldValidator>
                            <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFD">
                            </cc5:ValidatorCalloutExtender>
                          
                        </td>
                        <td align="right" class="tdBold">
                            <asp:Label ID="lblToDate" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, ToDate %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblToDateRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="txtField" Width="75px"></asp:TextBox>
                            <asp:ImageButton ID="imgToDate" ImageUrl="~/Images/calender-icon.png" runat="server"
                                Width="15px" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" ImageAlign="Middle" CausesValidation="false" />
                            <cc5:CalendarExtender ID="ToDateCalendar" runat="server" TargetControlID="txtToDate"
                                PopupButtonID="imgToDate" Format="dd/MMM/yyyy"  PopupPosition="BottomRight" EnableViewState="false">
                            </cc5:CalendarExtender>
                         <%--   <cc5:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtToDate"
                                ClearMaskOnLostFocus="false" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Date" InputDirection="RightToLeft"
                                ErrorTooltipEnabled="True" CultureName="en-GB" />--%>
                            <%-- <cc5:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtToDate" ControlExtender="MaskedEditExtender4" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid date!" EmptyValueMessage="The date is not entered"  />--%>
                            <asp:RequiredFieldValidator ID="RequiredTD" runat="server" ClearMaskOnLostFocus="false"
                                ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:LocalizedText, RequiredToDate %>" Display="None"></asp:RequiredFieldValidator>
                            <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredTD">
                            </cc5:ValidatorCalloutExtender>
                        </td>
                        <td align="right" class="style5">
                            <asp:Label ID="lblFromTime" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, FromTime %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblFromTimeRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left" class="style6">
                            <asp:TextBox ID="txtFromTime" runat="server" CssClass="txtField" Width="70px">
                            </asp:TextBox>
                            <cc5:MaskedEditExtender ID="MaskedEditFromTime" runat="server" TargetControlID="txtFromTime" UserTimeFormat="TwentyFourHour"
                                Mask="99:99" ClearMaskOnLostFocus="false" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft"
                                ErrorTooltipEnabled="True" AutoCompleteValue="00:00" />                        
                        </td>
                        <td align="right" class="tdBold">
                            <asp:Label ID="lblToTime" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, ToTime %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblToTimeRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtToTime" runat="server" CssClass="txtField" Width="70px">
                            </asp:TextBox>
                            <cc5:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtToTime" UserTimeFormat="TwentyFourHour"
                                Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" ClearMaskOnLostFocus="false"
                                OnInvalidCssClass="MaskedEditError" MaskType="Time" ErrorTooltipEnabled="True"
                                AutoCompleteValue="00:00" />
                            <cc5:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlToValidate="txtToTime"
                                ControlExtender="MaskedEditExtender3" Display="Static" IsValidEmpty="false"
                                InvalidValueMessage="<%$ Resources:LocalizedText, InvalidTime %>" EmptyValueMessage="<%$ Resources:LocalizedText, TimeEmpty %>" />
                                                 </td>
                    </tr>
                    <tr>
                        <%-- <td align="right" class="tdBold">
                            <asp:Label ID="lblHost" runat="server" CssClass="lblField" Text="Host"></asp:Label> &nbsp;<asp:Label
                                ID="Label1" runat="server" CssClass="lblRequired" Text="*"></asp:Label> 
                        </td>--%>
                        <td align="right" class="style3">
                            <asp:Label ID="lblHost" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, Host %>"></asp:Label>
                            &nbsp;<asp:Label ID="Label2" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left" class="style7">
                            <input id="txtHost" type="text" runat="server" style="width:110px" readonly="readonly" class="txtField" />
                            <asp:ImageButton runat="server" ID="imgbutHost" ImageUrl="~/Images/search.gif" Width="20px"
                                Visible="true" OnClientClick="PopUpHost()" ToolTip="<%$ Resources:LocalizedText, SearchHostName %>"
                                OnClick="ImgbutHost_Click" />
                            <%-- Security not host start--%>
                            <asp:RequiredFieldValidator ID="RequiredFN" runat="server" ControlToValidate="txtHost"
                                ErrorMessage="<%$ Resources:LocalizedText, SelectHost %>" Display="None"></asp:RequiredFieldValidator>
                            <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender_ndew" runat="server" TargetControlID="RequiredFN">
                            </cc5:ValidatorCalloutExtender>
                            <%-- end--%>
                        </td>
                        <td align="right" class="tdBold">
                            <asp:Label ID="lblHostContactNo" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, HostContactNumber %>"></asp:Label>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtHostContactNo" runat="server" CssClass="txtField" MaxLength="10"
                                onKeyPress="return allowNo(event.keyCode, event.which);"></asp:TextBox>
                        </td>
                        <td align="right" class="style5">
                            <%--<asp:Label ID="lblEscort" runat="server" CssClass="lblField" Text="Escort"></asp:Label>--%>
                            &nbsp;
                        </td>
                        <td align="left" class="style6">                                          
                            <asp:HiddenField ID="myHiddenVar" runat="server" />
                        </td>
                        <td align="right" class="tdBold">
                            <%--<asp:Label ID="lblVehicleNo" runat="server" CssClass="lblField" Text="Vehicle No"></asp:Label>--%>
                            &nbsp;
                            <asp:HiddenField ID="hdnBadgeStatus" runat="server" />
                            <asp:HiddenField ID="hidHostDeptDesc" runat="server" />
                            <asp:CheckBox ID="hdnPermitEquipments" runat="server" Visible="false" />
                        </td>
                        <td align="left">
                            <%--    <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="txtField" MaxLength="20"></asp:TextBox>--%>
                            <%-- <input type="hidden" id="hidHostDeptDesc" runat="server" />    --%>
</div>
<%--<asp:LinqDataSource ID="LinqVMSDB" runat="server" 
    ContextTypeName="VMSDL.VMSDBDataContext" TableName="MasterDatas">
    </asp:LinqDataSource>--%>
<%-- Added by priti on 3rd June for VMS CR VMS31052010CR6--%>

</td> </tr> 
  <tr>
                        <td align="right" class="style3">
                           <asp:hiddenfield id="AdvanceAllowabledays" runat="server" value="0" />
<%--end--%>
<asp:hiddenfield id="hdnRecurrencePattern" runat="server" />
<asp:hiddenfield id="hdnOccurence" runat="server" />
<asp:hiddenfield id="hdnDate" runat="server" />
<asp:hiddenfield id="hdnFromDate" runat="server" />
<asp:hiddenfield id="hdnToDate" runat="server" />
<asp:hiddenfield id="hdnFromTime" runat="server" />
<asp:hiddenfield id="hdnToTime" runat="server" />
<asp:hiddenfield id="hdnToTimeBeforeExtended" runat="server" /></td>
                        <td align="left" valign="top" colspan="6" style="width:600px" >
                        <asp:ValidationSummary ID="vdnSummary" runat="server" 
                       ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" /> 

                              <asp:CustomValidator ID="custDateCheck" runat="server" 
                            ErrorMessage="<%$ Resources:LocalizedText, FromDateValidation %>"                                                     
                            OnServerValidate="CustDateCheck_Validate" Display="None"  >
                            </asp:CustomValidator>
                        
                             <asp:CustomValidator ID="custFromDateCurrentDateCheck" runat="server" 
                            ErrorMessage="<%$ Resources:LocalizedText, VisitFromTimeMandatory %>"                                                     
                            OnServerValidate="CheckFromDateWithCurrentDate_Validate" Display="None" >
                            </asp:CustomValidator>
                            
                            <asp:CustomValidator ID="custToDateCurrentDateCheck" runat="server" 
                            ErrorMessage="<%$ Resources:LocalizedText, VisitToTimeMandatory %>"                                                     
                            OnServerValidate="CheckToDateWithCurrentDate_Validate" Display="None" >
                            </asp:CustomValidator>
                            
                             <asp:CustomValidator ID="custCheckDateDuration" runat="server" 
                            ErrorMessage="<%$ Resources:LocalizedText, VisitDurationError %>"                                                     
                            OnServerValidate="CheckDateDuration" Display="None" >
                            </asp:CustomValidator>
                             <asp:CustomValidator ID="custCheckDateTimewithCurrentTime" runat="server" 
                            ErrorMessage="<%$ Resources:LocalizedText, CurrentTimeError %>"                                                     
                            OnServerValidate="CheckTimewithCurrentTime" Display="None" >
                            </asp:CustomValidator>
                             <asp:CustomValidator ID="custCheckTimewithToTimeAndFromTime" runat="server" 
                            ErrorMessage="<%$ Resources:LocalizedText, CurrentTimeFromTimeToTime %>"                                                     
                            OnServerValidate="CheckTimewithToTimeAndFromTime" Display="None" >
                            </asp:CustomValidator>

                            
                            </td>                    
                    </tr>                  
            </table> </ContentTemplate> </asp:UpdatePanel> </div> 
