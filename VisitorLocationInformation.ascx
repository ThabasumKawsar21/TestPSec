<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitorLocationInformation.ascx.cs"
    Inherits="VMSDev.UserControls.VisitorLocationInformation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc5" %>
<script type="text/javascript">
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

        $('#<%=hdnFromDate.ClientID %>').val(fromNow.format("dd/MM/yyyy"))
        $('#<%=hdnToDate.ClientID %>').val(toNow.format("dd/MM/yyyy"))
        $('#<%=hdnFromTime.ClientID %>').val(fromtime)
        $('#<%=hdnToTime.ClientID %>').val(totime)

    }
    function CurrentDateShowing(e) {
        if (!e.get_selectedDate() || !e.get_element().value)
            e._selectedDate = (new Date()).getDateOnly();
    }
    function GetOffsetTimeLocation() {

        var rightNow = new Date();
        $('#<%=hdnClientOffset.ClientID %>').val(rightNow.getTimezoneOffset());

        var CurrentDate = new Date();
        var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
        VisitorLocationInformation.AssignTimeZoneOffsetLocation(CurrentDate.getTimezoneOffset());
        VisitorLocationInformation.AssignCurrentDateTimeLocation(today);
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

        var visitorOffset = $.trim($('#<%=hdnVisitorOffset.ClientID %>').val());
        var rightNow = new Date();

        if (visitorOffset == "")
            visitorOffset = rightNow.getTimezoneOffset();

        var offset = (visitorOffset) / 60;
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

        $('#<%=hdnFromDate.ClientID %>').val(moment(Startdate).format('DD/MMM/YYYY'))
        $('#<%=hdnToDate.ClientID %>').val(moment(Enddate).format('DD/MMM/YYYY'))
        $('#<%=hdnFromTime.ClientID %>').val(moment(StartTime, 'HH:mm').format('HH:mm'))
        $('#<%=hdnToTime.ClientID %>').val(moment(EndTime, 'HH:mm').format('HH:mm'))
        //$('#<%=hdnToTimeBeforeExtended.ClientID %>').val(newEndTime[3])


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
      
    }

</script>
<script type="text/javascript">

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
            var txtEscort = document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_txtEscort');
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

                var myHiddenVar = document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_myHiddenVar');
                myHiddenVar.value = "";
                myHiddenVar.value = res[1];
                // txtHostContactNo.disabled = true;
            }
        }
    }
   

    function GetAssociatedetail(ID) {
        var ddlPurpose = document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_ddlPurpose');

        var type = ddlPurpose.options[ddlPurpose.selectedIndex].value;
        if (type == "" || type == "Select Purpose" || type == "Select") {
            alert("Please select a visitor Type.");
            $("#VMSContentPlaceHolder_VisitorLocationInformationControl_txtHost").val("");
            $("#VMSContentPlaceHolder_VisitorLocationInformationControl_hdnSelectedHost").val('');
        }

        else {
            var text = document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_txtHost');
            var hiddenHost = $("#VMSContentPlaceHolder_VisitorLocationInformationControl_hdnSelectedHost");
            if (ID == 0) {
               
                if (text.value.length >= 4) {
                   $("#VMSContentPlaceHolder_VisitorLocationInformationControl_txtHost").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: 'VMSEnterInformationBySecurity.aspx/GetAssociateDetails',
                                data: '{"text":"' + text.value + '","type":"' + type + '"}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    hiddenHost.val('');
                                    var serachData = JSON.parse(data.d)
                                    if (serachData.length > 0) {
                                        response(serachData);
                                    }
                                    else {
                                        if (type == "Client") {
                                            var result = [
                                                {
                                                    label: 'Error: Entered Asssociate doesn’t belong to M+ level',
                                                    value: response.term
                                                }
                                            ];
                                            response(result);
                                            hiddenHost.val('');
                                        }
                                        else {
                                            var result = [
                                                {
                                                    label: 'Error: Entered Asssociate doesn’t exist.',
                                                    value: response.term
                                                }
                                            ];
                                            response(result);
                                            hiddenHost.val('');
                                        }
                                    }
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    hiddenHost.val('');
                                }
                            });
                       },
                        select: function (event, ui) {
                            if (ui.item.value.indexOf('Error') != -1) {
                                hiddenHost.val('');
                                text.innerText = '';
                            }
                            else {
                                hiddenHost.val(ui.item.value);
                            }
                        },
                        focus: function (event, ui) {
                            event.preventDefault();
                            jQuery(this).val(ui.item.suggestion);
                        },
                        minLength: 3
                    });
                }
            }
            else {
    
                if (text.value.length >= 4) {
                    document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_txtHost' + ID + '').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: 'VMSEnterInformationBySecurity.aspx/GetAssociateDetails',
                                data: '{"text":"' + text.value + '","type":"' + type + '"}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    hiddenHost.val('');
                                    var serachData = JSON.parse(data.d)
                                    if (serachData.length > 0) {
                                        response(serachData);
                                    }
                                    else {
                                        if (type == "Client") {
                                            var result = [
                                                {
                                                    label: 'Error: Entered Asssociate doesn’t belong to M+ level',
                                                    value: response.term
                                                }
                                            ];
                                            response(result);
                                            hiddenHost.val('');
                                        }
                                        else {
                                            var result = [
                                                {
                                                    label: 'Error: Entered Asssociate doesn’t exist.',
                                                    value: response.term
                                                }
                                            ];
                                            response(result);
                                            hiddenHost.val('');
                                        }
                                    }
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    if (ui.item.value.indexOf('Error') != -1) {
                                        hiddenHost.val('');
                                        text.innerText = '';
                                    }
                                    else {
                                        hiddenHost.val(ui.item.value);
                                    }
                                }
                            });
                        },
                        select: function (event, ui) {
                            hiddenHost.val(ui.item.value);
                        },
                        focus: function (event, ui) {
                            event.preventDefault();
                            jQuery(this).val(ui.item.suggestion);
                        },
                        minLength: 3
                    });
                }

            }
            
        }
    }
   
    function PopUpHost() {

        var ddlPurpose = document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_ddlPurpose');

        var type = ddlPurpose.options[ddlPurpose.selectedIndex].value;
        if (type == "" || type == "Select Purpose" || type == "Select") {
            alert("Please select a visitor Type.");
        }
        else if (type == "Client") {
            var Result = window.showModalDialog('SearchUserDetails.aspx?type=Client', 'SearchHost', 'status:no;dialogWidth:425px;dialogHeight:425px;dialogHide:true;help:no;');
        }
        else {
            var Result = window.showModalDialog('SearchUserDetails.aspx?type=other', 'SearchHost', 'status:no;dialogWidth:425px;dialogHeight:425px;dialogHide:true;help:no;');
        }

        if (Result != "undefined") {
            var res = new Array();
            var res1 = new Array();
            var res2 = new Array();
            res = Result.split("%$%");


            //var txtHost = document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_txtHost');            
            //var txtHostContactNo = document.getElementById('VMSContentPlaceHolder_VisitorLocationInformationControl_txtHostContactNo');
            res1 = res[0].split("(");
            res2 = res1[0].split(",");

            $('#<%=txtHost.ClientID %>').val(res2[1] + "," + " " + res2[0] + "(" + res1[1]);
            $('#<%=txtHost.ClientID %>').val(res2[1] + "," + " " + res2[0] + "(" + res1[1]);
            //txtHost.value = res2[1] + "," + " " + res2[0] + "(" + res1[1];
            if (res[1] != "") {
                //txtHostContactNo.value = "";
                //txtHostContactNo.value = res[1];
                // txtHostContactNo.disabled = true;
                $('#<%=txtHostContactNo.ClientID %>').val("");
                $('#<%=txtHostContactNo.ClientID %>').val(res[1]);
            }
            else
                //txtHostContactNo.disabled = false;
                $('#<%=txtHostContactNo.ClientID %>').disabled = false;

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
    .style1 {
        text-align: left;
    }

    .style3 {
        width: 162px;
    }

    .style4 {
        width: 95px;
    }

    .style5 {
    }

    .style6 {
        width: 200px;
    }

    .style7 {
        width: 168px;
        white-space: nowrap;
    }

    .ui-widget {
        font-size: 11px;
    }
</style>
<div>
     <input type="hidden" id="hdnSelectedHost" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <table class="tblHeadStyle" width="100%" style="white-space: nowrap">
                <tr>
                    <td colspan="7" align="left">
                        <asp:Label ID="lblVisitorLocation" runat="server" CssClass="lblHeada" Text="<%$ Resources:LocalizedText, VisitLocationInformation %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="5" style="width: 20px;"></td>
                    <td align="right" class="style3">
                        <asp:Label ID="lbCountry" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, Country %>"></asp:Label>
                        &nbsp;<asp:Label ID="lblRequiredCountry" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                    </td>
                    <td align="left" class="style7">
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlCountry_SelectedIndexChanged"
                            CssClass="ddlField" Width="120px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="Requiredcountry1" runat="server" ControlToValidate="ddlCountry"
                            InitialValue="0" ErrorMessage="<%$ Resources:LocalizedText, SelectCountry %>"
                            Display="None"></asp:RequiredFieldValidator>
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
                            InitialValue="Select" ErrorMessage="<%$ Resources:LocalizedText, SelectVisitorType %>"
                            Display="None"></asp:RequiredFieldValidator>
                        <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" TargetControlID="Requiredpurpose">
                        </cc5:ValidatorCalloutExtender>
                        <asp:TextBox ID="txtPurpose" runat="server" CssClass="txtField" Visible="false" MaxLength="100" Width="80"></asp:TextBox>
                    </td>
                    <tr>
                        <td align="right" class="style3">
                            <asp:Label ID="lblFromDate" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, FromDate %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblFromDateRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left" class="style7">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtField" Width="75px"></asp:TextBox>
                            <asp:ImageButton ID="imgFromDate" ImageUrl="~/Images/calender-icon.png" runat="server"
                                Width="15px" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" ImageAlign="Middle"
                                CausesValidation="false" />
                            <cc5:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                                PopupButtonID="imgFromDate" Format="dd/MMM/yyyy" PopupPosition="BottomRight"
                                EnableViewState="false">
                            </cc5:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFD" runat="server" ClearMaskOnLostFocus="false"
                                ControlToValidate="txtFromDate" ErrorMessage="<%$ Resources:LocalizedText, RequiredFromDate %>"
                                Display="None"></asp:RequiredFieldValidator>
                            <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFD">
                            </cc5:ValidatorCalloutExtender>
                        </td>
                        <td align="right" class="tdBold">
                            <asp:Label ID="lblToDate" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, ToDate %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblToDateRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="txtField" Width="75px" align="left"></asp:TextBox>
                            <asp:ImageButton ID="imgToDate" ImageUrl="~/Images/calender-icon.png" runat="server"
                                Width="15px" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" ImageAlign="Middle"
                                CausesValidation="false" />
                            <cc5:CalendarExtender ID="ToDateCalendar" runat="server" TargetControlID="txtToDate"
                                PopupButtonID="imgToDate" Format="dd/MMM/yyyy" PopupPosition="BottomRight" EnableViewState="false">
                            </cc5:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredTD" runat="server" ClearMaskOnLostFocus="false"
                                ControlToValidate="txtToDate" ErrorMessage="<%$ Resources:LocalizedText, RequiredToDate %>"
                                Display="None"></asp:RequiredFieldValidator>
                            <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredTD">
                            </cc5:ValidatorCalloutExtender>
                        </td>
                        <td align="right" class="style5">
                            <asp:Label ID="lblFromTime" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, FromTime %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblFromTimeRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left" class="style6">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromTime" runat="server" CssClass="txtField" Width="70px" Enabled="true">
                                        </asp:TextBox>
                                    </td>
                                    <td style="font-size: 12px;">Time zone
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTimeZone1" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            Font-Bold="True" Font-Size="11pt" Style="padding-left: 3px; padding-right: 3px;"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <cc5:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtFromTime" UserTimeFormat="TwentyFourHour"
                                Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" ClearMaskOnLostFocus="false"
                                OnInvalidCssClass="MaskedEditError" MaskType="Time" ErrorTooltipEnabled="True"
                                AutoCompleteValue="00:00" />
                            <cc5:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtFromTime"
                                ControlExtender="MaskedEditExtender3" Display="Static" IsValidEmpty="false" InvalidValueMessage="<%$ Resources:LocalizedText, InvalidTime %>"
                                EmptyValueMessage="<%$ Resources:LocalizedText, TimeEmpty %>" />
                        </td>
                        <td align="right" class="tdBold">
                            <asp:Label ID="lblToTime" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, ToTime %>"></asp:Label>
                            &nbsp;<asp:Label ID="lblToTimeRequired" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                        </td>
                        <td align="left">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtToTime" runat="server" CssClass="txtField" Width="70px" Enabled="true"> </asp:TextBox>
                                    </td>
                                    <td style="font-size: 12px;">Time zone
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTimeZone2" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            Font-Bold="True" Font-Size="11pt" Style="padding-left: 3px; padding-right: 3px;"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <cc5:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtToTime" UserTimeFormat="TwentyFourHour"
                                Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" ClearMaskOnLostFocus="false"
                                OnInvalidCssClass="MaskedEditError" MaskType="Time" ErrorTooltipEnabled="True"
                                AutoCompleteValue="00:00" />
                            <cc5:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlToValidate="txtToTime"
                                ControlExtender="MaskedEditExtender3" Display="Static" IsValidEmpty="false" InvalidValueMessage="<%$ Resources:LocalizedText, InvalidTime %>"
                                EmptyValueMessage="<%$ Resources:LocalizedText, TimeEmpty %>" />
                        </td>
                    </tr>
                <tr>

                    <td align="right" class="style3">
                        <asp:Label ID="lblHost" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, Host %>"></asp:Label>
                        &nbsp;<asp:Label ID="Label2" runat="server" CssClass="lblRequired" Text="*"></asp:Label>&nbsp;
                    </td>
                    <td align="left" class="style7">
                        <input id="txtHost" type="text" runat="server" placeholder="Enter Employee ID"  autocomplete="off" onkeyup="GetAssociatedetail(0)"  style="width: 190px"
                            class="txtField" />

                        <asp:RequiredFieldValidator ID="RequiredFN" runat="server" ControlToValidate="txtHost"
                            ErrorMessage="<%$ Resources:LocalizedText, SelectHost %>" Display="None"></asp:RequiredFieldValidator>
                        <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtender_ndew" runat="server" TargetControlID="RequiredFN">
                        </cc5:ValidatorCalloutExtender>

                    </td>
                    <td align="right" class="tdBold">
                        <asp:Label ID="lblHostContactNo" runat="server" CssClass="lblField" Text="<%$ Resources:LocalizedText, HostContactNumber %>"></asp:Label>
                        &nbsp;
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtHostContactNo" runat="server" CssClass="txtField" MaxLength="10"
                            onKeyPress="return allowNo(event.keyCode, event.which);"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <table id="tblIdentity" runat="server">
                            <tr>
                                <td style="width: 108px;">
                                    <asp:Label ID="lblIdentityType" runat="server" CssClass="lblField" Text="Identity Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlIdentityType" runat="server" CssClass="ddlField" AutoPostBack="True"
                                        OnSelectedIndexChanged="DdlIdentityType_SelectedIndexChanged">
                                        <asp:ListItem>Select</asp:ListItem>
                                        <asp:ListItem>Driver´s license</asp:ListItem>
                                        <asp:ListItem>Passport #</asp:ListItem>
                                        <asp:ListItem>legal ID</asp:ListItem>
                                        <asp:ListItem>Argentinean ID</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIdentityNo" runat="server" CssClass="lblField" Text="Identity No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIdentityNo" runat="server" CssClass="txtField" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" class="tdBold">
                        <%--<asp:Label ID="lblVehicleNo" runat="server" CssClass="lblField" Text="Vehicle No"></asp:Label>--%>
                            &nbsp;
                            <asp:HiddenField ID="hdnBadgeStatus" runat="server" />
                        <asp:HiddenField ID="hidHostDeptDesc" runat="server" />
                        <asp:CheckBox ID="hdnPermitEquipments" runat="server" Visible="false" Checked="true" />
                    </td>
                    <td align="left">
                        <%--    <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="txtField" MaxLength="20"></asp:TextBox>--%>
                        <%-- <input type="hidden" id="hidHostDeptDesc" runat="server" />    --%>
                            </div>
                            <%--<asp:LinqDataSource ID="LinqVMSDB" runat="server" 
    ContextTypeName="VMSDL.VMSDBDataContext" TableName="MasterDatas">
    </asp:LinqDataSource>--%>
                        <%-- Added by priti on 3rd June for VMS CR VMS31052010CR6--%>
                        <asp:HiddenField ID="myHiddenVar" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="style3">
                        <asp:HiddenField ID="AdvanceAllowabledays" runat="server" Value="0" />
                        <%--end--%>
                        <asp:HiddenField ID="hdnRecurrencePattern" runat="server" />
                        <asp:HiddenField ID="hdnOccurence" runat="server" />
                        <asp:HiddenField ID="hdnDate" runat="server" />
                        <asp:HiddenField ID="hdnFromDate" runat="server" />
                        <asp:HiddenField ID="hdnToDate" runat="server" />
                        <asp:HiddenField ID="hdnFromTime" runat="server" />
                        <asp:HiddenField ID="hdnToTime" runat="server" />
                        <asp:HiddenField ID="hdnToTimeBeforeExtended" runat="server" />
                        <asp:HiddenField ID="hdnVisitorOffset" runat="server" />
                        <asp:HiddenField ID="hdnClientOffset" runat="server" />
                    </td>
                    <td align="left" valign="top" colspan="6" style="width: 600px">
                        <asp:ValidationSummary ID="vdnSummary" runat="server" ShowMessageBox="false" DisplayMode="BulletList"
                            ShowSummary="true" />
                        <asp:CustomValidator ID="custDateCheck" runat="server" ErrorMessage="<%$ Resources:LocalizedText, FromDateValidation %>"
                            OnServerValidate="CustDateCheck_Validate" Display="None">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="custFromDateCurrentDateCheck" runat="server" ErrorMessage="<%$ Resources:LocalizedText, VisitFromTimeMandatory %>"
                            OnServerValidate="CheckFromDateWithCurrentDate_Validate" Display="None">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="custToDateCurrentDateCheck" runat="server" ErrorMessage="<%$ Resources:LocalizedText, VisitToTimeMandatory %>"
                            OnServerValidate="CheckToDateWithCurrentDate_Validate" Display="None">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="custCheckDateDuration" runat="server" ErrorMessage="<%$ Resources:LocalizedText, VisitDurationError %>"
                            OnServerValidate="CheckDateDuration" Display="None">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="custCheckDateTimewithCurrentTime" runat="server" ErrorMessage="<%$ Resources:LocalizedText, CurrentTimeError %>"
                            OnServerValidate="CheckTimewithCurrentTime" Display="None">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="custCheckTimewithToTimeAndFromTime" runat="server" ErrorMessage="<%$ Resources:LocalizedText, CurrentTimeFromTimeToTime %>"
                            OnServerValidate="CheckTimewithToTimeAndFromTime" Display="None">
                        </asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
