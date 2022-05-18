<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewLogbySecurity.ascx.cs"
    Inherits="VMSDev.UserControls.ViewLogbySecurity" %>
<%@ Register Assembly="VMSFramework" Namespace="VMSFramework" TagPrefix="vf" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style type="text/css">
    .cellborder {
        border: 1px solid black;
        border-collapse: collapse;
    }

    .cell {
        border: 1px solid black;
    }
</style>

<script type="text/javascript">
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
    //function to checkin visitors

</script>
<script type="text/javascript">
    $(document).ready(function () {
        var hdnCurrentLocalDate = new Date();
        document.getElementById('<%=hdnCurrentLocalDate.ClientID %>').value = hdnCurrentLocalDate.format("MM/dd/yyyy hh:mm:ss");
        SetTimeFormat();
    });

    function SetTimeFormat() {

        $('#<%=grdResult.ClientID%>').children().find("tr[class='odd_row'],tr[class='even_row']").each(function () {

            var visitorOffset = $.trim($(this).children().find("label[id='hdnVisitorOffset']").html().toString());
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

            //var meetingstarttime = (new Date($.trim($(this).children().find("label[id='startdate']").html().toString()).split(' ')[0] + ' ' + $(this).children().find("label[id='starttime']").html())).toString().split(' ');
            //var meetingendtime = (new Date($.trim($(this).children().find("label[id='startdate']").html().toString()).split(' ')[0] + ' ' + $(this).children().find("label[id='expectedouttime']").html())).toString().split(' ');
            //var actualouttime = (new Date($.trim($(this).children().find("label[id='startdate']").html().toString()).split(' ')[0] + ' ' + $(this).children().find("label[id='actualouttime']").html())).toString().split(' ');
            //var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
            //var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');

            //alert(moment($(this).children().find("label[id='startdate']").html().toString()).format('DD MMM YYYY'));
            //          alert(moment($(this).children().find("label[id='expectedouttime']").html(), 'HH:mm').format('hh:mm a'));
            // alert(moment($(this).children().find("label[id='expectedouttime']").html().toString()).format('LT'));
            //if (actualouttime[3] == "00:00:00")
            //    var tempactualouttime = "";
            //else
            //    var tempactualouttime = gettimedtls(actualouttime[1], actualouttime[3], hours, mins).split('|');


            //meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
            //meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
            //actualouttime = (new Date(actualouttime[5], tempactualouttime[0], actualouttime[2], tempactualouttime[2], tempactualouttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
            ////$(this).children().find("label[id='startdate']").html(moment($(this).children().find("label[id='startdate']").html().toString()).format('DD/MMM/YYYY'));
            //$(this).children().find("label[id='starttime']").html(moment($(this).children().find("label[id='starttime']").html(), 'HH:mm').format('hh:mm a'));
            //$(this).children().find("label[id='expectedouttime']").html(moment($(this).children().find("label[id='expectedouttime']").html(), 'HH:mm').format('hh:mm a'));
            //$(this).children().find("label[id='actualouttime']").html(actualouttime[3]);

        });
    }

    function gettimedtls(arg1, arg2, arg3, arg4) {
        var x = "";
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
        var tmpmin = "00";
        var tmphour = "12";
        try {
            var tmpdtl = arg2.split(':');
            tmpmin = parseInt(tmpdtl[1]);
            tmphour = parseFloat(tmpdtl[0]);
            var remminhour = parseInt((tmpmin + arg4) / 60);
            tmpmin = (((tmpmin + arg4) / 60) - remminhour) * 60;
            tmphour = tmphour + parseInt(arg3) + remminhour;
        }
        catch (e) {

        }
        x = x + '|' + tmphour + '|' + tmpmin;
        return x;
    }
</script>
<script type="text/javascript">
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

    function ClosePopup(val) {
        if (val == "checkin")
            $find("popCheckin").hide();
        else if (val == "checkout")
            $find("popCheckout").hide();
        else if (val == "cardlog")
            $find("popCardlog").hide();
        else if (val == "reissue")
            $find("popReissue").hide();

        return false;
    }

    function Checkin(val, obj) {
        var result = null;
        var Visitorcardhistory = null;        
        Visitorcardhistory = getVCardHistory(val);
        if ($(obj).attr('disabled') != "disabled") {
            $('#hdnVisitDetailsId').val(val);
            $.ajax({
                type: "POST",
                url: "ViewLogbySecurity.aspx/CheckinPopupDisplay",
                contentType: "application/json;charset=utf-8",
                data: "{'visitDetailID':" + val + "}",
                dataType: "json",
                success: function (response) {
                    result = response;
                },
                complete: function () {
                    $('#VMSContentPlaceHolder_ViewLogbySecurity1_lblname').html(result.d.VisitorName);
                    $('#VMSContentPlaceHolder_ViewLogbySecurity1_lblCname').html(result.d.Organization);
                    $('#VMSContentPlaceHolder_ViewLogbySecurity1_lblPopVisitDate').html(result.d.VisitDate);
                    $('#VMSContentPlaceHolder_ViewLogbySecurity1_lblPopVisitFromTime').html(result.d.FromTime);
                    $('#VMSContentPlaceHolder_ViewLogbySecurity1_lblPopVisitEndTime').html(result.d.ToTime);
                    $("#txtVcardnumber").val('');
                    $("#spnProcessing").html('');
                    $("#lblValiadationText").html('');
                    $find("popCheckin").show();
                    document.getElementById('txtVcardnumber').focus();
                }

            });
        }

        return false;
    }
    //function to get the Cardloghistory
    function getVCardHistory(val) {
        debugger;
        var result = null;
        $('#hdnVisitDetailsId').val(val);
        $.ajax({
            type: "POST",
            url: "ViewLogbySecurity.aspx/GetVcardSummary",
            contentType: "application/json;charset=utf-8",
            data: "{'visitDetailID':" + val + "}",
            dataType: "json",
            success: function (response) {            
                result = response;                
            },
            error: function(data)
            {
                alert("error");
            },            
            complete: function () {
                debugger;
                var tableHeaderRowCount = 1;
                var table = document.getElementById('tblVCardSummary');
                var rowCount = table.rows.length;
                for (var i = tableHeaderRowCount; i < rowCount; i++) {
                    table.deleteRow(tableHeaderRowCount);
                }
                if (result.d[0].length != 0) {
                    $("#divVcardSummary").show();
                    for (i = 0; i < result.d[0].length; i++) {                      
                        $("#tblVCardSummary").append("<tr id='dynamicData' class='cellborder' style='text-align:center;'>" +
                       "<td class='cell'><Label ID='lblVcardno" + i + "' class='field_text'>" + result.d[0][i][0] + "</Label ></td >" +
                       "<td class='cell'><Label ID='lblIssuedon" + i + "' class='field_text'>" + result.d[0][i][2] + "</Label ></td ></tr >");                       
                    }
                }
                else
                {
                    $("#divVcardSummary").hide();
                    $("#tblVCardSummary").append("<tr id='dynamicData' class='cellborder' style='text-align:center;'><td  colspan=7 class='field_text'>No Records Found</td></tr>");
                }
                result.d.val(null);
                }
            
        });

    }


</script>
<style type="text/css">
    .ModalWindow {
        border: 1px solid #c0c0c0;
        background: #EEEEEE;
        padding: 0px 0px 0px 0px;
        width: 500px;
        height: auto;
    }

    .modalBackground {
        background-color: #878776;
        filter: alpha(opacity=40);
        opacity: 0.5;
    }

    p.MsoNormal {
        margin-top: 1.3pt;
        margin-right: 5.75pt;
        margin-bottom: 12.0pt;
        margin-left: 0in;
        line-height: 12.0pt;
        font-size: 10.0pt;
        font-family: "Arial", "sans-serif";
    }

    .gridStyle {
        width: 100%;
    }

        .gridStyle th {
            background-color: #3188B5;
            font-family: Verdana;
            border-color: Black;
            color: White;
        }

    .style1 {
        width: 19px;
    }

    .cssPager td {
        padding-left: 4px;
        padding-right: 4px;
        font-size: 15px;
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
                <asp:TextBox ID="txtExpress" runat="server" CssClass="txtField" onpaste="return false"
                    onkeypress="return onlyNumbers()" Width="100px" MaxLength="20"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtExpress"
                    WatermarkText="<%$ Resources:LocalizedText, lblExpressCheckin %>">
                </cc1:TextBoxWatermarkExtender>
                <span class="field_txt" style="white-space: nowrap; padding-left: 20px">
                    <asp:Label ID="lblSearchOR" Text="<%$ Resources:LocalizedText, lblSearchOR %>" runat="server"
                        CssClass="field_txt"></asp:Label>
                </span>
            </td>
            <td width="4%" style="white-space: nowrap; padding-left: 20px">
                <asp:TextBox ID="txtSearch" runat="server" onkeyup="avoidSpecialCharacters(this)"
                    CssClass="txtField" Width="250px" MaxLength="30"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtSearch"
                    WatermarkText="<%$ Resources:LocalizedText, searchByName %>">
                </cc1:TextBoxWatermarkExtender>
               <span id="VCardNoSpan" class="field_txt" style="white-space: nowrap; padding-left: 20px" align="left">               
              <asp:Label ID="lblSearchOR1" Text="<%$ Resources:LocalizedText, lblSearchOR %>" runat="server" 
              CssClass="field_txt" Visible="false" ></asp:Label>
                </span>
            </td>
             <td id="VCardNumberfield" align="left" class="tdBold" width="10%" style="white-space: nowrap; padding-left: 20px"
                 >
                <asp:TextBox ID="txtVCNumber" runat="server" CssClass="txtField" onpaste="return false"
                    onkeyup="avoidSpecialCharacters(this)" Width="100px" MaxLength="20" Visible="false"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtVCNumber"
                    WatermarkText="VCard Number">
                </cc1:TextBoxWatermarkExtender>
               
            </td>
            <td align="left" width="3%" style="white-space: nowrap">
                <asp:Label ID="lblFromDate" runat="server" Text="<%$ Resources:LocalizedText, FromDate %>"
                    CssClass="field_txt" Width="40px"></asp:Label>
            </td>
            <td width="10%" style="white-space: nowrap; padding-left: 5px;">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtField" Enabled="true" Style="width: 70px;"></asp:TextBox>
                <asp:ImageButton ID="imgFromDate" Style="cursor: pointer" ImageUrl="~/Images/calender-icon.png"
                    runat="server" Width="15px" ImageAlign="Middle" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" />
                <cc1:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                    PopupButtonID="imgFromDate" Format="MM/dd/yyyy" PopupPosition="BottomRight">
                </cc1:CalendarExtender>

                <asp:ImageButton ID="imbRefreshStartdate" runat="server" ImageUrl="~/Images/Refresh_image.gif"
                    OnClientClick="javascript:fnStartDateRefresh();" Style="cursor: pointer" alt="<%$ Resources:LocalizedText, RefreshDate %>" />
            </td>
            <td align="left" width="3%" style="white-space: nowrap">
                <asp:Label ID="lblToDate" runat="server" CssClass="field_txt" Text="<%$ Resources:LocalizedText, ToDate %>"
                    Width="40px"></asp:Label>
            </td>
            <td width="10%" style="white-space: nowrap; padding-left: 5px;">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="txtField" Enabled="true" Style="width: 70px;"></asp:TextBox>
                <asp:ImageButton ID="imgToDate" Style="cursor: pointer" ImageUrl="~/Images/calender-icon.png"
                    runat="server" Width="15px" ImageAlign="Middle" ToolTip="<%$ Resources:LocalizedText, imgcalenderControl %>" />
                <cc1:CalendarExtender ID="ToDateCalendar" runat="server" TargetControlID="txtToDate"
                    PopupButtonID="imgToDate" Format="MM/dd/yyyy" PopupPosition="BottomRight">
                </cc1:CalendarExtender>
                <asp:ImageButton ID="imbRefreshEnddate" runat="server" ImageUrl="~/Images/Refresh_image.gif"
                    OnClientClick="javascript:fnEndDateRefresh();" Style="cursor: pointer" alt="<%$ Resources:LocalizedText, RefreshDate %>" />

            </td>
            <td width="10%">
                <asp:Label ID="lblstatus" runat="server" Text="Status " Height="16px" Style="padding-bottom: 3px;"
                    CssClass="field_txt"></asp:Label>
                <asp:DropDownList ID="ddlReqStatus" runat="server" Style="width: auto; min-width: 100px; height: 20px; margin-left: 10px;">
                    <asp:ListItem Text="<%$ Resources:LocalizedText, SelectStatus %>" Value="Select Status"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:LocalizedText, Yettoarrive %>" Value="Yet to arrive"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:LocalizedText, In %>" Value="In"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:LocalizedText, Out %>" Value="Out"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="7">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="7" align="center">
                <table style="width: 586px">
                    <tr align="center">
                        <td width="100px">
                            <asp:Button ID="btnSearch" runat="server" OnClick="BtnSearch_Click" Text="<%$ Resources:LocalizedText, Search %>"
                                Width="95px" CssClass="cssButton" />
                        </td>
                        <td width="100px">
                            <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:LocalizedText, Clear %>"
                                OnClick="BtnReset_Click" CausesValidation="False" Width="95px" CssClass="cssButton" />
                        </td>
                        <td colspan="2" width="350px" align="center">
                            <asp:Panel ID="pnlGotoVisitors" runat="server">
                                <asp:Label runat="server" ID="lblNoRelevantRequest" Text="<%$ Resources:LocalizedText, lblNoRelevantRequest %>"
                                    CssClass="field_txt"></asp:Label>
                                <asp:LinkButton ID="btnAddNew" runat="server" Text="<%$ Resources:LocalizedText, lblClickHere %>"
                                    CssClass="LinkButton" Style="margin-left: 10px" Visible="True" OnClick="BtnAddNew_Click"></asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel ID="pnlGotoRequests" runat="server">
                                <asp:LinkButton ID="btnBackToRequests" Text="<%$ Resources:LocalizedText, btnBackToRequests %>"
                                    runat="server" CssClass="LinkButton" Width="330px" Visible="True" OnClick="BtnBackToRequests_Click"></asp:LinkButton>
                            </asp:Panel>
                        </td>
                        <td width="20px">
                            <asp:ImageButton ImageUrl="~/Images/excel_icon.GIF" ID="btnExport" runat="server"
                                Text="<%$ Resources:LocalizedText, btnExport %>" OnClick="BtnExport_Click" Width="16px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="7">
                <div style="height: 10px">
                    <tr>
                        <td colspan="7" align="left">
                            <asp:Label ID="lblVMS" runat="server" CssClass="errorLabel"></asp:Label>
                        </td>
                    </tr>
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
                <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
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
    <cc1:ModalPopupExtender ID="modalVisit" BackgroundCssClass="modalBackground" CancelControlID="imgClose2"
        DropShadow="true" X="-1" Y="-1" PopupControlID="pnlVisit" TargetControlID="btnAddNew"
        runat="server">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlVisit" runat="server" Width="300" Height="100" CssClass="ModalWindow"
        Style="display: none">
        <table>
            <tr>
                <td style="width: 12px"></td>
                <td></td>
                <td style="width: 12px" align="right">
                    <asp:ImageButton ID="imgClose2" runat="server" ImageUrl="../Images/Close.png" Height="12px" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left" style="padding-left: 10px">
                    <asp:Label ID="lblVisit" CssClass="field_txt" Style="font-size: 12px" runat="server"
                        Text="<%$ Resources:LocalizedText, lblVisit %>"></asp:Label>
                </td>
                <td></td>
            </tr>
            <tr style="text-align: center">
                <td></td>
                <td style="padding-top: 10px">
                    <asp:LinkButton ID="btnYes" runat="server" CssClass="cssButton" Style="text-decoration: none; padding-top: 4px;"
                        Width="60px" Text="<%$ Resources:LocalizedText, btnYes %>"
                        OnClick="BtnYes_Click" Height="18px" />
                    &nbsp
                <asp:LinkButton ID="btnNo" runat="server" CssClass="cssButton" Style="text-decoration: none; padding-top: 4px;"
                    Width="60px" Text="<%$ Resources:LocalizedText, btnNo %>"
                    PostBackUrl="~/VMSEnterInformationBySecurity.aspx" Height="18px" />
                </td>
                <td></td>
            </tr>
        </table>
    </asp:Panel>
    <center>
    <asp:Panel ID="pnlRequests" runat="server">
        <asp:Panel ID="pnlImage" runat="server" HorizontalAlign="Left">
            <table>
                <tr>
                    <td align="left" class="field_txt" style="font-weight: bold">
                        &nbsp;
                        <asp:Label ID="lblLegend" runat="server" Text="<%$ Resources:LocalizedText, lblLegend %>"
                            CssClass="field_txt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Image ID="image" runat="server" ImageUrl="~/Images/Black.jpg" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblBlack" runat="server" Text="<%$ Resources:LocalizedText, Yettoarrive %>"
                            CssClass="field_txt"></asp:Label>
                    </td>
                    <td>
                        <asp:Image ID="imgAmber" runat="server" ImageUrl="~/images/Amber1.jpg" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblAmber" Text="<%$ Resources:LocalizedText, lblAmber %>" runat="server"
                            CssClass="field_txt"></asp:Label>
                    </td>
                    <td>
                        <asp:Image ID="imgGreen" runat="server" ImageUrl="~/images/Green.jpg" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblGreen" runat="server" Text="<%$ Resources:LocalizedText, lblGreen %>"
                            CssClass="field_txt"></asp:Label>
                    </td>
                    <td>
                        <asp:Image ID="imgRed" runat="server" ImageUrl="~/images/Reddishblink.gif" Width="10" />
                    </td>
                    <td>
                        <asp:Label ID="lblRed" runat="server" Text="<%$ Resources:LocalizedText, lblExceedOutTime %>"
                            CssClass="field_txt"></asp:Label>
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
                                    CssClass="gridStyle" HeaderStyle-Wrap="True" PageSize="25" AllowPaging="False"
                                    OnRowDataBound="GrdResult_RowDataBound" GridLines="Vertical" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, SlNo %>" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ControlStyle Height="10px" Width="10px" />
                                            <ControlStyle Height="10px" Width="10px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Status %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <%-- Bugfix - Nightshift visitor start--%>
                                                <asp:Image ID="image" runat="server" ImageUrl="~/Images/Black.jpg" Width="10" />
                                                <%--'<%# GetImageUrl(DataBinder.Eval(Container.DataItem, "intime", "{0:d}") as string, DataBinder.Eval(Container.DataItem, "ExpectedOutTime", "{0:d}") as string,Eval("RequestStatus")as string) %>'--%>
                                                <%-- Bugfix - Nightshift visitor end--%>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Name %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVName" runat="server" Text='<%# Highlight(Eval("Name").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Organization %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVCompany" runat="server" Text='<%# Highlight(Eval("Company").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Mobile %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVMobile" runat="server" Text='<%# Highlight(Eval("MobileNo").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedText, Host %>" DataField="Host"
                                            ItemStyle-Width="150px" />
                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedText, VisitorType %>" DataField="Purpose" />
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Date %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <label id="startdate">
                                                    <%# Eval("Date", "{0:dd/MMM/yyyy}") %></label>                                                
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, ExpectedInTime %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <label id="starttime">                                                    
                                                    <%#Eval("intime")%></label>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>                              
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, lblExpectedOutTime %>"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <label id="expectedouttime">
                                                    <%#Eval("Expectedouttime")%></label>
                                                  
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actual In Time" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <label id="actualIntime">
                                                    <%#Eval("ActualInTime")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, ActualOutTime %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <label id="actualouttime">
                                                    <%#Eval("ActualOutTime")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedText, VCard %>" DataField="BadgeNo" />
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, BadgeStatus %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBadgeStatus" runat="server" Text='<%# Highlight(Eval("BadgeStatus").ToString()) %>' />
                                                <label id="hdnVisitorOffset" style="display: none;">
                                                    <%#Eval("Offset").ToString() %>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                      
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, CardLog %>" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="75px">
                                            <ItemTemplate>
                                               <asp:LinkButton ID="btnCardLog" runat="server" Text="<%$ Resources:LocalizedText, LogView %>"
                                                    CommandArgument='<%#Eval("VisitDetailsID")%>' CommandName="LogView" CssClass="GridLinkButton" OnClientClick='<%# Eval("VisitDetailsID","return GetCardLog({0})") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, BadgeStatusUpdate %>"
                                            ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Button ID="btnBgStatus" runat="server" Text="<%$ Resources:LocalizedText, Update %>"
                                                    CommandName="FindValue" CommandArgument='<%#Eval("VisitDetailsID") %>' CssClass="cssButton" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Actions %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="220px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnCheckIn" runat="server" Text="<%$ Resources:LocalizedText, CheckIn %>" 
                                                    CommandArgument='<%#Eval("VisitDetailsID")%>' CommandName="CheckIn" CssClass="GridLinkButton" 
                                                    OnClientClick='<%# Eval("VisitDetailsID","return Checkin({0},this)") %>'/>
                                              
                                                <asp:LinkButton ID="btnCheckOut" runat="server" Text="<%$ Resources:LocalizedText, CheckOut %>"
                                                    CommandArgument='<%#Eval("VisitDetailsID")%>' CommandName="CheckOut" CssClass="GridLinkButton" OnClientClick='<%# Eval("VisitDetailsID","return Checkout({0},this)") %>'/>                                         
                                                <asp:LinkButton ID="btnReIssue" runat="server" Text="<%$ Resources:LocalizedText, ReIssue %>"
                                                    CommandArgument='<%#Eval("VisitDetailsID")%>' CommandName="ReIssue" CssClass="GridLinkButton" OnClientClick='<%# Eval("VisitDetailsID","return ShowReissuePopup({0},this)") %>' />
                                                <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:LocalizedText, btnView %>"
                                                    CommandArgument='<%#Eval("VisitDetailsID")%>' CommandName="ViewDetailsLink" CssClass="GridLinkButton" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, ViewDetails %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="25px" Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnView" ImageUrl="~/Images/Search_1.png" Height="20px" runat="server"
                                                    CommandArgument='<%#Eval("VisitDetailsID")%>' CommandName="ViewDetailsLink" />
                                                <asp:HiddenField ID="hdnViewDetailsID" runat="server" />
                                                <asp:HiddenField ID="hdnBadgeStatus" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Comments %>" ItemStyle-HorizontalAlign="Center"
                                            Visible="false">
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                                    <asp:Label ID="lblReprintComment" Text="<%$ Resources:LocalizedText, SelectReason %>"
                                        runat="server"></asp:Label>
                                </td>
                                <td width="16px" valign="top">
                                    <div style="margin-top: 2px; float: right; margin-right: 2px;">
                                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="../Images/Close.png" Height="14px" /></div>
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
                                        <asp:ListItem Text="<%$ Resources:LocalizedText, Lost %>" Selected="true" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:LocalizedText, PrinterJammed %>" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                   
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
                <asp:Panel ID="pnlVisitor" runat="server">
                    <asp:UpdatePanel ID="UpnlVisitor" UpdateMode="Conditional" runat="server" Visible="false">
                        <ContentTemplate>
                            <center>
                           
                            <asp:GridView ID="grdVisitor" runat="server" CellPadding="4" ForeColor="#333333"
                                AutoGenerateColumns="False" Font-Names="Verdana" Font-Size="X-Small" DataKeyNames="VisitorID"
                                AllowPaging="true" OnPageIndexChanging="grdVisitor_PageIndexChanging"  GridLines="Vertical"  PageSize="20"
                                CssClass="gridStyle" HeaderStyle-Wrap="True" Style="text-align: center" Width="900px">
                                <HeaderStyle CssClass="visitor_tab_head" Height="30px" />
                                <AlternatingRowStyle BackColor="#f4f9fb" BorderColor="#C9DDE8" BorderStyle="Solid"
                                    BorderWidth="1px" Height="30px"/>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, SlNo %>" ItemStyle-Width="5px">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ControlStyle Height="10px" Width="10px" />
                                        <ControlStyle Height="10px" Width="10px" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Name %>" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Wrap="true" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnName" runat="server" Text='<%# Highlight(Eval("Name").ToString()) %>'
                                                PostBackUrl='<%#"~/VMSEnterInformationBySecurity.aspx?VisitorID="+Eval("VisitorID")%>'
                                                CssClass="GridLinkButton" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Organization %>" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Wrap="true" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompany" runat="server" Text='<%# Highlight(Eval("Company").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Designation %>" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Wrap="true" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%# Highlight(Eval("Designation").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Mobile %>" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobile" runat="server" Text='<%# Highlight(Eval("MobileNo").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, VisitorId %>" ItemStyle-HorizontalAlign="Center"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVisitorId" Text='<%#Eval("VisitorID") %>' runat="server"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtComments" Width="85px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                             
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#EFF3FB" BorderColor="Gray" />
                                <AlternatingRowStyle BackColor="#CDE3F1" ForeColor="black" BorderColor="Gray" />
                                 <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="10" />
                            <PagerStyle CssClass="cssPager" BackColor="#EDF6E3" Height="25px" VerticalAlign="Bottom"
                                HorizontalAlign="Right" />
                            </asp:GridView>
                
                        </center>
                            <div class="grid">
                                <div class="pager">
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfGvPanel" runat="server" />
    <asp:HiddenField ID="hdnRecordFound" runat="server" />
    <asp:HiddenField ID="hdnVisitDetailId" runat="server" />
    <asp:HiddenField ID="hfGvStatus" runat="server" />
    <asp:HiddenField ID="hdnCurrentLocalDate" runat="server" />
    <asp:HiddenField ID="hdnCheckin" runat="server" />
    <asp:HiddenField ID="hdnSecurityLocation" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnVisitDetailsId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnVcardNo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCheckoutDetails" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="hdnCurrentHoldingCard" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnRebind" runat="server" ClientIDMode="Static" />
    <div style="visibility: hidden">
        <asp:CheckBox ID="hfSearch" runat="server" Visible="true" />
    </div>

    <cc1:ModalPopupExtender ID="ModalPopupCheckin" BackgroundCssClass="modalBackground"
        DropShadow="true" X="-1" Y="-1" PopupControlID="pnlCheckin" TargetControlID="hdnCheckin" BehaviorID="popCheckin"
        runat="server">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlCheckin" runat="server" Width="650" Height="400" CssClass="ModalWindow"
        Style="display: none;">
        <div style="display: inline-block; width: 610px; height: 20px; padding-left: 10px; padding-top: 8px;" class="header_txt">
            Check-In Visitor
        </div>
        <div style="float: right; display: inline-block; width: 20px; position: absolute;">

            <img src="Images/Close.png" style="height: 20px; margin-top: 5px;" id="imgCheckinClose" onclick="ClosePopup('checkin')" />
        </div>
        <hr />

        <div style="margin-left: 7px;">
            <table>
                <tr contenteditable="true">
                    <td class="field_txt">Visitor Name :
                    </td>
                    <td style="width: 200px;" class="field_text">
                        <asp:Label ID="lblname" runat="server"></asp:Label>
                    </td>
                    <td class="field_txt">Visitor Organization :
                    </td>
                    <td>
                        <asp:Label ID="lblCname" runat="server" class="field_text"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div></div>
        <div class="field_txt" style="margin-top: 15px; margin-left: 10px;">
            Visiting Schedule:
        </div>
        <table id="tblVisitSummary" class="cellborder" align="center">
            <tr style="border: 1px solid black;">
                <td style="width: 100px;" class="GridHeader cell">Date</td>
                <td style="width: 100px;" class="GridHeader cell">From Time</td>
                <td style="width: 100px;" class="GridHeader cell">To Time</td>
            </tr>
            <tr class="cellborder" style="text-align: center;">
                <td class="cell">
                    <asp:Label ID="lblPopVisitDate" runat="server" CssClass="field_text"></asp:Label></td>
                <td class="cell">
                    <asp:Label ID="lblPopVisitFromTime" runat="server" CssClass="field_text"></asp:Label></td>
                <td class="cell">
                    <asp:Label ID="lblPopVisitEndTime" runat="server" CssClass="field_text"></asp:Label></td>
            </tr>
        </table>
        <br />
        
<%--Displaying Active Vcard--%>
        <div id="divVcardSummary">
             <div class="field_txt" style="margin-top: 15px; margin-left: 10px;">
            <p><font size="2" color="red" >Note: Visitor is yet to surrender the following visitor card(s) issued earlier.</font></p>
                 </div>
        <table id="tblVCardSummary" class="cellborder" align="Center">
            <tr style="border: 1px solid black;">
                <td style="width: 100px;" class="GridHeader cell">VCard Number</td>
                <td style="width: 100px;" class="GridHeader cell">Issued On</td>
                
            </tr> 
        </table>
     </div>
        <div style="margin-left: 10px;">
            <table style="margin-top: 70px;">
                <tr>
                    <td class="field_txt">Enter Vcard Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtVcardnumber" ClientIDMode="Static" MaxLength="7" Style="width: 80px; text-transform: uppercase" class="border" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <img src="Images/help.png" alt="" style="width: 20px; height: 16px;" title="Please enter card that is registered in visitor card inventory of Access Card application" />
                    </td>
                    <td>
                        <label id="lblValiadationText" class="errorLabel" style="display: none">hello</label></td>
                </tr>
            </table>
        </div>
        <div style="float: right; margin-right: 20px; margin-bottom: 10px;">
            <span id="spnProcessing" class="field_txt"></span>
            <table>

                <tr>
                    <td style="width: 70px;">
                        <input type="button" id="btn_Checkin" class="cssButton" value="Check-in" onclick="return CheckinVCard()" />
                    </td>
                    <td>
                        <input type="button" value="Cancel" id="btn_Cancel" class="cssButton" onclick="ClosePopup('checkin')" />

                    </td>
                </tr>
            </table>
            <asp:Button ID="btnTemp" runat="server" OnClick="btn_Checkin_Click" Style="display: none;" ClientIDMode="Static" />
        </div>


    </asp:Panel>

    <%--checkout popup--%>
    <cc1:ModalPopupExtender ID="ModalPopupCheckOut" BackgroundCssClass="modalBackground"
        DropShadow="true" X="-1" Y="-1" PopupControlID="pnlCheckout" TargetControlID="hdnCheckin" BehaviorID="popCheckout"
        runat="server">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlCheckout" runat="server" Width="550" Height="350" CssClass="ModalWindow"
        Style="display: none;">
        <div style="display: inline-block; width: 510px; height: 20px; padding-left: 10px; padding-top: 8px;" class="header_txt">
            Check-Out Visitor
        </div>
        <div style="float: right; display: inline-block; width: 20px; position: absolute;">

            <img src="Images/Close.png" style="height: 20px; margin-top: 5px;" id="imgCheckoutClose" onclick="ClosePopup('checkout')" />
        </div>
        <hr />


        <div></div>
        <div class="field_txt" style="margin-top: 20px; margin-left: 10px; padding-bottom: 20px;">
            List of active Vcard(s) of the visitor :
        </div>
        <table id="tblVisitorCardInfo" class="cellborder" align="center">
            <tr style="border: 1px solid black;">
                <td style="width: 50px;" class="GridHeader cell">S.No</td>
                <td style="width: 100px;" class="GridHeader cell">VCard#</td>
                <td style="width: 300px;" class="GridHeader cell">Status</td>
            </tr>

        </table>
        <div style="margin-left: 10px;">
        </div>
        <div style="float: right; margin-right: 30px; right: 10px; bottom: 10px; position: absolute;">
            <table>
                <tr>
                    <td style="width: 70px;">
                        <asp:Button ID="btn_Checkout" runat="server" CssClass="cssButton" Text="Check-out" OnClick="btn_Checkout_Click" ClientIDMode="Static" CommandArgument='<%#Eval("VisitDetailsID")%>'></asp:Button>
                    </td>
                    <td>
                        <input type="button" value="Cancel" id="btnCheckoutCancel" class="cssButton" onclick="ClosePopup('checkout')" />

                    </td>
                </tr>
            </table>
        </div>


    </asp:Panel>
    <%--cardLog popup--%>
    <cc1:ModalPopupExtender ID="ModalPopupCardLog" BackgroundCssClass="modalBackground"
        DropShadow="true" X="-1" Y="-1" PopupControlID="pnlCardlog" TargetControlID="hdnCheckin" BehaviorID="popCardlog"
        runat="server">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlCardlog" runat="server" Width="820" Height="350" CssClass="ModalWindow"
        Style="display: none;">
        <div style="display: inline-block; width: 780px; height: 20px; padding-left: 10px; padding-top: 8px;" class="header_txt">
            Card log
        </div>
        <div style="float: right; display: inline-block; width: 20px; position: absolute;">

            <img src="Images/Close.png" style="height: 20px; margin-top: 5px;" id="imgCardlogClose" onclick="ClosePopup('cardlog')" />
        </div>
        <hr />


        <div></div>

        <div style="margin-left: 10px;">
            <table>
                <tr contenteditable="true">
                    <td class="field_txt">Visitor Name :
                    </td>
                    <td style="width: 230px;" class="field_text">
                        <asp:Label ID="lblvisitorName" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                    <td class="field_txt">Visitor Organization : </td>
                    <td class="field_text" style="width: 300px;">
                        <asp:Label ID="lblOrganizantionName" runat="server" class="field_text" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>

                <tr contenteditable="true">
                    <td class="field_txt">&nbsp;</td>
                    <td style="width: 200px;" class="field_text">&nbsp;</td>
                </tr>
                <tr contenteditable="true">
                    <td class="field_txt"><b>Visiting Schedule: </b></td>
                    <td class="field_text" style="width: 200px;">
                        <asp:Label ID="lblMeetingSchedule" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div align="center">
            <table id="tblCardlogInfo" class="cellborder">
                <tr style="border: 1px solid black;">
                    <td style="width: 40px;" class="GridHeader cell">S.No</td>
                    <td style="width: 70px;" class="GridHeader cell">Date</td>
                    <td style="width: 70px;" class="GridHeader cell">VCard No.</td>
                    <td style="width: 120px;" class="GridHeader cell">Issued On</td>
                    <td style="width: 150px;" class="GridHeader cell">Status</td>
                    <td style="width: 150px;" class="GridHeader cell">Reason</td>
                    <td style="width: 130px;" class="GridHeader cell">Returned/Reported on</td>
                </tr>

            </table>
        </div>
        <div style="float: right; margin-right: 30px; right: 10px; bottom: 10px; position: absolute;">
            <span id="logProcess" class="field_txt"></span>
            <table>
                <tr>
                    <td style="width: 70px;">
                        <input type="button" value="Surrender" id="btnSurrender" class="cssButton" onclick="return SurrenderCard()" />
                    </td>
                    <td>
                        <input type="button" value="Cancel" id="btnCardLogCancel" class="cssButton" onclick="ClosePopup('cardlog')" />

                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <%--ReIssue--%>
    <cc1:ModalPopupExtender ID="ModalPopupReissue" BackgroundCssClass="modalBackground"
        DropShadow="true" X="-1" Y="-1" PopupControlID="pnlReissue" TargetControlID="hdnCheckin" BehaviorID="popReissue"
        runat="server">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlReissue" runat="server" Width="550" Height="600" CssClass="ModalWindow"
        Style="display: none;">
        <div style="display: inline-block; width: 510px; height: 20px; padding-left: 10px; padding-top: 8px;" class="header_txt">
            Re-Issue
        </div>
        <div style="float: right; display: inline-block; width: 20px; position: absolute;">

            <img src="Images/Close.png" style="height: 20px; margin-top: 5px;" id="imgReissueClose" onclick="ClosePopup('reissue')" />
        </div>
        <hr />

        <div style="margin-left: 7px;">
            <table>
                <tr contenteditable="true">
                    <td class="field_txt">Visitor Name :
                    </td>
                    <td style="width: 200px;" class="field_text">
                        <asp:Label ID="lblVname" ClientIDMode="Static" runat="server"></asp:Label>
                    </td>
                    <td class="field_txt">Visitor Organization :
                    </td>
                    <td>
                        <asp:Label ID="lblOrg" runat="server" class="field_text" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div></div>
        <div class="field_txt" style="margin-top: 20px; margin-left: 10px; padding-bottom: 20px;">
            Meeting Schedule:
        </div>
        <table id="tblReissueMeetingDetails" class="cellborder" align="center">
            <tr style="border: 1px solid black;">
                <td style="width: 50px;" class="GridHeader cell">Date</td>
                <td style="width: 200px;" class="GridHeader cell">FromTime</td>
                <td style="width: 200px;" class="GridHeader cell">ToTime</td>
            </tr>

        </table>
        <div style="margin-left: 10px;">
        </div>
        <div class="field_txt" style="margin-top: 20px; margin-left: 10px; padding-bottom: 20px;">
            List of active Vcard(s) of the visitor :
        </div>
        <table id="tblReissueCardDeatils" class="cellborder" align="center">
            <tr style="border: 1px solid black;">
                <td style="width: 50px;" class="GridHeader cell">S.No</td>
                <td style="width: 100px;" class="GridHeader cell">VCard#</td>
                <td style="width: 300px;" class="GridHeader cell">Status</td>
            </tr>

        </table>
        <div style="margin-left: 10px;">
            <table style="margin-top: 70px;">
                <tr>
                    <td class="field_txt">Enter Vcard Number
                    </td>
                    <td>
                        <asp:TextBox ID="txtVcard" ClientIDMode="Static" MaxLength="7" Style="width: 80px; text-transform: uppercase" class="border" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <img src="Images/help.png" alt="" style="width: 20px; height: 16px;" title="Please enter card that is registered in visitor card inventory of Access Card application" />
                    </td>
                    <td>
                        <label id="lblValidation" class="errorLabel" style="display: none"></label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-left: 10px;">
        </div>

        <div style="float: right; margin-right: 30px; right: 10px; bottom: 10px; position: absolute;">
            <span id="spnProcess" class="field_txt"></span>
            <table>
                <tr>
                    <td style="width: 70px;">
                        <input type="button" value="Reissue" class="cssButton" onclick="return ReissueCard()" />

                    </td>
                    <td>
                        <input type="button" value="Cancel" id="btnReissueCancel" class="cssButton" onclick="ClosePopup('reissue')" />

                    </td>
                </tr>
            </table>
            <asp:Button ID="btnReissueTemp" runat="server" OnClick="btn_ReIssue_Click" ClientIDMode="Static" Style="display: none" />
        </div>

    </asp:Panel>
</form>
