<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IVS.aspx.cs" Inherits="VMSDev.IVS" MasterPageFile="~/VMSMain.Master" %>

<%@ Register Src="~/UserControls/MsgBox.ascx" TagName="MessageBox" TagPrefix="MB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc5" %>
<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder"
    runat="server">

    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/common.js"></script>
    <script type="text/javascript" src="Scripts/IVS.js"></script>
    
    <script language="javascript" type="text/javascript">
        //window.onerror = blockError;
        //window.onload = setClipBoardData;

        function GetImageFromIDCard() {

            var IsCHireON = false;
            const CHireKey = '<%=System.Configuration.ConfigurationManager.AppSettings["CHireKey"]%>';
            const APIEnvUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["IDCardApiUrl"]%>';
            var id = $("#<%=txtEmpID.ClientID%>").val();
            var ChireFileContentId = $('#<%=hdnFileUploadID.ClientID%>').val();
            var apiresponse = 'API response : ';

            if (CHireKey === 'ON')
                IsCHireON = true;

            ////ID CARD CALL
            $.ajax({
                type: 'GET',
                url: APIEnvUrl,
                xhrFields: {
                    withCredentials: true
                },
                data: { "associateId": id },
                dataType: 'json',
                success: function (image) {
                    //debugger
                    if (image.indexOf("data:image") != -1) {
                        apiresponse += 'image found';
                        $('#<%=ImgAssociate.ClientID%>').attr("src", image);
                    }
                    else {
                        apiresponse += image;
                        image = "Images/char.jpeg";
                        IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#<%=ImgAssociate.ClientID%>').attr("src", image);
                    }

                    $('#<%=hdnImageURL.ClientID%>').val(image);
                    //TO DO: remove the log.
                    console.log(apiresponse);
                },
                error: function (e) {
                   // debugger
                    apiresponse += "Error: Response JSON : " + e.responseJSON + " Status : " + e.status + " Response Text : " + e.responseText;
                    $('#<%=hdnImageURL.ClientID%>').val("Images/char.jpeg");
                    IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#<%=ImgAssociate.ClientID%>').attr("src", "Images/char.jpeg");
                    
                    //TO DO: remove the log.
                    console.log(apiresponse);
                }
            });
        }

        // function to get image from Chire.
        function GetImageFromChire(filecontentID) {
            //file content id is already fetched from CHIRE VIEW
            
           
            if (filecontentID !== null && filecontentID !== '') {

                $.ajax({
                    type: 'POST',
                    url: "IVS.aspx/GetChireImageFromECM",
                    contentType: "application/json;charset=utf-8",
                    data: '{"contentId":"' + filecontentID + '"}',
                    dataType: 'json',
                    success: function (image) {
                        //debugger
                        console.log('Displaying Chire Image');
                        $('#<%=ImgAssociate.ClientID%>').attr("src", JSON.parse(image.d));
                        $('#<%=hdnImageURL.ClientID%>').val(JSON.parse(image.d));
                    }
                    ,
                    error: function (e) {
                       
                        console.log('Chire FLow error: ' + e.responseJSON);
                        $('#<%=ImgAssociate.ClientID%>').attr("src", "Images/char.jpeg");
                         $('#<%=hdnImageURL.ClientID%>').val("Images/char.jpeg");

                     }
                });
             }
            else {
                console.log('Chire file ID is null');
                 $('#<%=ImgAssociate.ClientID%>').attr("src", "Images/char.jpeg");
                $('#<%=hdnImageURL.ClientID%>').val("Images/char.jpeg");
            }
        }


        function OpenPopUpAccessCardReport() {
            var Url;
            var windowName;
            {
                Url = "Report_OneDayAccessCard/AccesscardReport.aspx";
            }

            if (windowName == undefined)
                windowName = "Popup";
            window.open(Url, windowName, "toolbar=0, menubar=0, location=0,resizable=0,status=1,top=20,left=20,height=650px,width=1170px,scrollbars=yes, maximize=1");
        }
        function setClipBoardData() {
            setInterval("window.clipboardData.setData('text','')", 20);
        }

        function blockError() {
            window.location.reload(true);
            return true;
        }

        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;
            if (charCode > 95 && charCode < 106)
                return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            if (e.shiftKey) return false;
            return true;
            return true;

        }

        function GetOffset() {
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            IVS.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            IVS.AssignCurrentDateTime(today);
        }
        function GetOffsetGrid() {
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            IVS.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            IVS.AssignCurrentDateTime(today);
            var r = document.getElementById('<%=btnIVSHidden.ClientID %>');
             r.click();
         }
         function SetDefaultTime() {
             var fromNow = new Date();
             var toNow = new Date();
             var formatFromDate = fromNow.format("dd/MM/yyyy");
             $('#<%=txtFromDate.ClientID %>').val(formatFromDate);
            var formatToDate = toNow.format("dd/MM/yyyy");
            $('#<%=txtToDate.ClientID %>').val(formatToDate);
            $('#<%=hdnFromDate.ClientID %>').val(fromNow.format("dd/MM/yyyy"))
            $('#<%=hdnToDate.ClientID %>').val(toNow.format("dd/MM/yyyy"))
        }
    </script>
    <script language="javascript" type="text/javascript">
        function SetIVSLocalTime() {
           // debugger;
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
            $('.gridStyle tr.even_row,.gridStyle tr.odd_row').each(function () {
                var meetingstarttime = (new Date($(this).children().find("label[id='startdate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='starttime']").html())).toString().split(' ');
                var meetingendtime = (new Date($(this).children().find("label[id='enddate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='starttime']").html())).toString().split(' ');
                var meetingreturnedtime = (new Date($(this).children().find("label[id='ReturnedDate']").html().split(' ')[0] + ' ' + $(this).children().find("label[id='ReturnedTime']").html())).toString().split(' ');
                var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
                var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
                try {
                    if (meetingreturnedtime.length > 1) {
                        var tempreturnedtime = gettimedtls(meetingreturnedtime[1], meetingreturnedtime[3], hours, mins).split('|');
                        meetingreturnedtime = (new Date(meetingreturnedtime[5], tempreturnedtime[0], meetingreturnedtime[2], tempreturnedtime[2], tempreturnedtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');

                        $(this).children().find("label[id='ReturnedDate']").html((meetingreturnedtime[2].length < 2 ? '0' + meetingreturnedtime[2] : meetingreturnedtime[2]) + ' ' + meetingreturnedtime[1] + ' ' + meetingreturnedtime[5]);
                        $(this).children().find("label[id='ReturnedTime']").html(meetingreturnedtime[3]);
                    }

                    meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                    meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                    $(this).children().find("label[id='startdate']").html((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);
                    $(this).children().find("label[id='starttime']").html(meetingstarttime[3]);
                    $(this).children().find("label[id='enddate']").html((meetingendtime[2].length < 2 ? '0' + meetingendtime[2] : meetingendtime[2]) + ' ' + meetingendtime[1] + ' ' + meetingendtime[5]);
                    //    $('input[name=hdnIssuedDate]').val($(this).children().find("label[id='startdate']").html());
                    $(this).children().find("input[id$='hdnIssuedDate']").val((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);

                    $(this).children().find("input[id$='hdnGridToDate']").val((meetingendtime[2].length < 2 ? '0' + meetingendtime[2] : meetingendtime[2]) + ' ' + meetingendtime[1] + ' ' + meetingendtime[5]);
                }
                catch (e) {

                }
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
        /*code to stop multiple confirm clicks*/
        var submit = 0;
        function CheckIsRepeat() {
            if (++submit > 1) {
                alert('An attempt was made to Confirm this form more than once; this extra attempt will be ignored.');
                return false;
            }
        }
    </script>


    <asp:ScriptManager ID="DefaultMasterScriptManager" runat="server">
    </asp:ScriptManager>
    <table id="Table1" border="0" cellpadding="0" runat="server" cellspacing="0" width="100%">
        <tr>
            <td colspan="4" align="center" valign="middle">
                <table width="100%" class="border" id="tblsearchoptions" visible="true" runat="server">
                    <tr>
                        <td align="left" class="table_header_bg" colspan="9">&nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblModuleValue %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="1250">
                                <tr>
                                    <td align="right" style="width: 11%; margin-left: 80px;" valign="top">
                                        <asp:Label ID="lblAssociateId" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, AssociateId %>"
                                            maxlength="15" Width="91px" Height="24px"></asp:Label>
                                        &nbsp;<div class="clear">
                                        </div>

                                        <asp:Label ID="LblAccess" runat="server" CssClass="field_text flt-left mtop-20" Text="Temporary Access Card No"
                                            Width="140px" />
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtEmpID" MaxLength="15" runat="server" AutoCompleteType="Disabled" CssClass="field_text flt-left"
                                            Width="80"></asp:TextBox>
                                        &nbsp;<asp:Button ID="btnSearch" runat="server" BackColor="#767561" CausesValidation="False"
                                            Font-Bold="False" Font-Size="10px" ForeColor="White" Height="21px" OnClick="BtnSearch_Click"
                                            CssClass="flt-left mtop-20 " Text="<%$ Resources:LocalizedText, Search %>" />
                                        &nbsp;<asp:Button ID="btnClear" runat="server" BackColor="#767561" CausesValidation="False"
                                            Font-Bold="False" Font-Size="10px" ForeColor="White" Height="21px" OnClick="BtnClear_Click"
                                            Text="<%$ Resources:LocalizedText, Clear %>" UseSubmitBehavior="False" CssClass="flt-left mtop-20" />
                                        <asp:Label ID="LblReason" runat="server" class="field_text flt-left mtop-20" Style="margin-left: 20px;" Text="<%$ Resources:LocalizedText, Reason %>"></asp:Label>
                                        <asp:RadioButtonList ID="RdlReason" runat="server" AutoPostBack="true" CssClass="field_text flt-left mtop-20"
                                            OnSelectedIndexChanged="RdlReason_SelectedIndexChanged" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:LocalizedText, ForgotIDCard %>"
                                                Value="20"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:LocalizedText, OnsiteReturn %>" Value="21"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:LocalizedText, Lost %>" Value="22"></asp:ListItem>
                                        </asp:RadioButtonList>

                                        &nbsp;<asp:Label ID="DDAccessLabel" runat="server" Text="Select CARD:" class="field_text"></asp:Label>

                                        <asp:DropDownList ID="ddAccessdetail" runat="server" AutoPostBack="true" CssClass="field_text"
                                            OnSelectedIndexChanged="DDAccessdetail_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="accesstextid" runat="server" AutoCompleteType="Disabled"
                                            MaxLength="10" Width="200px"></asp:TextBox>
                                        <asp:Label ID="lblCardIssuedCityName" runat="server" CssClass="field_text flt-right mtop-20">
                                        </asp:Label>
                                        <asp:Label ID="lblCardIssuedCity" runat="server" CssClass="field_text flt-right mtop-20"
                                            Text="<%$ Resources:LocalizedText, CityIVS %>"></asp:Label>
                                        <asp:Label ID="lblCardIssuedFacilityName" runat="server" CssClass="field_text flt-right mtop-20">
                                        </asp:Label>
                                        <asp:Label ID="lblCardIssuedFacility" runat="server" CssClass="field_text flt-right mtop-20"
                                            Text="<%$ Resources:LocalizedText, FacilityIVS %>" Height="16px"></asp:Label>





                                        <div class="clear">

                                            <asp:TextBox ID="txtAccess" runat="server" AutoCompleteType="Disabled"
                                                CssClass="field_text flt-left" MaxLength="09" Width="80px"></asp:TextBox>
                                        </div>

                                        <asp:Button ID="btnCheckIn" ValidationGroup="Access" runat="server" CssClass="btnStyle flt-left mtop-20" OnClick="BtnCheckIn_Click"
                                            Text="<%$ Resources:LocalizedText, CheckIn %>" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCheckOut" runat="server" CssClass="btnStyle flt-left mtop-20"
                                            OnClick="BtnCheckOut_Click" Text="<%$ Resources:LocalizedText, CheckOut %>" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnReprint" runat="server" CssClass="btnStyle flt-left mtop-20" OnClick="BtnReprint_Click"
                                            Text="<%$ Resources:LocalizedText, Reprint %>" />

                                        <asp:Label ID="LblFromDate" runat="server" CssClass="txtField m-left-20" margin-left="167px" Text="<%$ Resources:LocalizedText, FromDate %>"></asp:Label>
                                        <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="True" CssClass="txtField "
                                            Width="75px"></asp:TextBox>
                                        <cc5:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="imgFromDate" Format="dd/MM/yyyy" PopupPosition="BottomRight" EnableViewState="false">
                                        </cc5:CalendarExtender>

                                        <asp:Label ID="LblToDate" runat="server" CssClass="txtField" Text="<%$ Resources:LocalizedText, ToDate %>"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txtField " Width="75px"></asp:TextBox>
                                        <asp:ImageButton ID="imgToDate" runat="server" CausesValidation="false" ImageAlign="Middle"
                                            ImageUrl="~/Images/calender-icon.png" ToolTip="To View the calender control"
                                            Width="15px" CssClass="" Visible="true" />
                                        <cc5:CalendarExtender ID="ToDateCalendar" runat="server" EnableViewState="false"
                                            Format="dd/MMM/yyyy" PopupButtonID="imgToDate" PopupPosition="BottomRight" TargetControlID="txtToDate">
                                        </cc5:CalendarExtender>

                                        <%--newly added by abi end --%>

                                        <asp:Label ID="lblTempIdIssued" runat="server" CssClass="field_text" ForeColor="Red"></asp:Label>

                                        <asp:HiddenField ID="hdnPassDetailsID" runat="server" />
                                        <asp:HiddenField ID="hdnIssuedLocation" runat="server" />
                                        <asp:HiddenField ID="hdnIssuedCity" runat="server" />
                                        <asp:HiddenField ID="hdnFacility" runat="server" />
                                        <asp:HiddenField ID="hdnFromDate" runat="server" />
                                        <asp:HiddenField ID="hdnToDate" runat="server" />
                                        <asp:HiddenField ID="hdnFromTime" runat="server" />
                                        <asp:HiddenField ID="hdnToTime" runat="server" />

                                        <asp:Button ID="btnhidden" runat="server" Style="display: none" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="left">
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Access" ControlToValidate="txtAccess" Text="Please enter the Access Card ID!" runat="server" Font-Size="12px"/>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Access" ControlToValidate="txtAccess" Font-Size="12px"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="Access" Operator="DataTypeCheck" Type="Integer" ControlToValidate="txtAccess" Font-Size="12px" />
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="Access" Operator="GreaterThan" Type="Integer" ValueToCompare="0" ControlToValidate="txtAccess" ErrorMessage="Invalid Access Card ID!" Font-Size="12px"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="9">
                <asp:Panel ID="panelEmp" runat="server" Width="100%" CssClass="border">
                    <table id="tblEmp" cellpadding="0" cellspacing="0" width="100%" style="height: 304px">
                        <tr>
                            <td align="left" style="height: 15px">&nbsp;<asp:Label ID="lblEmployeeHeader" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblEmployeeHeader %>"
                                Font-Size="11px"></asp:Label>
                            </td>
                            <td align="left" style="height: 15px">
                                <asp:Label ID="lblTerminated" runat="server" CssClass="field_text" ForeColor="Red"></asp:Label>
                            </td>
                            <td align="left" style="height: 15px"></td>
                            <td align="left" style="height: 15px" valign="middle">
                                <asp:Label ID="lblManagerHeader" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblManagerHeader %>"
                                    Font-Size="11px"></asp:Label>
                            </td>
                            <td style="width: 20%"></td>
                        </tr>
                        <tr>
                            <td align="right" style="height: 15px; width: 10%;" class="field_column_bg">
                                <asp:Label ID="lblEmployeeID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, AssociateId %>"></asp:Label>:
                            </td>
                            <td align="left" style="height: 15px; width: 25%;">&nbsp;<asp:Label ID="lblEmpID" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                            <td align="left" rowspan="8" style="width: 20%">
                                <asp:Image ID="ImgAssociate" runat="server" ImageUrl="~/Images/char.jpeg" Height="150px"
                                    Style="max-width: 200px" oncontextmenu="return false" ImageAlign="Top" />
                            </td>
                            <td align="right" style="height: 15px; width: 10%;" valign="middle" class="field_column_bg">
                                <asp:Label ID="lblManagerID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, AssociateId %>"></asp:Label>:
                            </td>
                            <td style="width: 25%" align="left">&nbsp;<asp:Label ID="lblMgrID" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="field_column_bg" style="height: 15px">
                                <asp:Label ID="lblEmployeeName" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>:
                            </td>
                            <td align="left" style="height: 15px">&nbsp;<asp:Label ID="lblEmpName" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                                <asp:Label ID="lblManagerName" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>:
                            </td>
                            <td style="width: 20%" align="left">&nbsp;<asp:Label ID="lblMgrName" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="field_column_bg" style="height: 15px">
                                <asp:Label ID="lblEmailID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, EmailID %>"></asp:Label>:
                            </td>
                            <td align="left" style="height: 15px">&nbsp;<asp:Label ID="lblEmpEmailID" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                                <asp:Label ID="lblManagerEmailID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, EmailID %>"></asp:Label>:
                            </td>
                            <td style="width: 20%" align="left">&nbsp;<asp:Label ID="lblMgrEmailID" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="field_column_bg" style="height: 19px">
                                <asp:Label ID="lblEmpMobile" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Mobile %>"></asp:Label>:
                            </td>
                            <td align="left" style="height: 19px">&nbsp;<asp:Label ID="lblEmployeeMobile" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                            <td align="right" class="field_column_bg" style="height: 19px" valign="middle">
                                <asp:Label ID="lblMgrMobile" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Mobile %>"></asp:Label>:
                            </td>
                            <td style="width: 20%; height: 19px;" align="left">&nbsp;<asp:Label ID="lblManagerMobileNo" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="field_column_bg" style="height: 15px">
                                <asp:Label ID="lblEmpExtension" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Extension %>"></asp:Label>:
                            </td>
                            <td align="left" style="height: 15px">&nbsp;<asp:Label ID="lblEmployeeExtension" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                                <asp:Label ID="Label4" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Extension %>"></asp:Label>:
                            </td>
                            <td style="width: 20%" align="left">&nbsp;<asp:Label ID="lblManagerExtension" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="field_column_bg" style="height: 15px">
                                <asp:Label ID="lblLocation" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Location %>"></asp:Label>:
                            </td>
                            <td align="left" style="height: 15px">&nbsp;<asp:Label ID="lblEmpLocation" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                            <td align="left" style="height: 15px" valign="middle"></td>
                            <td style="width: 20%"></td>
                        </tr>
                        <tr>
                            <td align="right" class="field_column_bg" style="height: 15px">
                                <asp:Label ID="lblCity" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, City %>"></asp:Label>:
                            </td>
                            <td align="left" style="height: 15px">&nbsp;<asp:Label ID="lblEmpCity" runat="server" CssClass="field_text"></asp:Label>
                            </td>
                            <td align="left" style="height: 15px" valign="middle"></td>
                            <td style="width: 20%">
                                <asp:HiddenField ID="hdnFileUploadID" runat="server" />
                            </td>
                        </tr>

                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    &nbsp;
    <table border="0" id="Table2" cellpadding="0" cellspacing="0" runat="server" width="500px">
        <tr>
            <td align="center" style="border-right: black 1pt solid; border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid; background-color: #edf6fd; width: 100%; height: 19px;"
                id="errortbl" visible="false">
                <asp:Label ID="lblMessage" runat="server" CssClass="field_text" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblIDCardStatus" runat="server" Visible="false" CssClass="field_text"
                    Font-Bold="true" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="text-align: right">
        <asp:LinkButton ID="lbtnAccesscardreport" OnClientClick="return OpenPopUpAccessCardReport()"
            CssClass="cont-btn" runat="server">
                            <span>Temporary AccessCard Report</span>
        </asp:LinkButton>
    </div>
    &nbsp;
    <asp:UpdatePanel ID="Upnl" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnConfirm" />
            <asp:PostBackTrigger ControlID="btnReprint" />
            <asp:PostBackTrigger ControlID="btnCheckOut" />
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="btnCheckIn" />
            <asp:PostBackTrigger ControlID="btnClear" />
            <asp:PostBackTrigger ControlID="lstvwResults" />


        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" HorizontalAlign="Left">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td align="left" class="field_txt" style="font-weight: bold">
                                        <asp:Label ID="lblLegend" Text="<%$ Resources:LocalizedText, lblLegend %>" runat="server"
                                            CssClass="field_txt"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Image ID="imgAmber" runat="server" ImageUrl="~/images/Amber1.jpg" Width="10" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAmber" runat="server" Text="<%$ Resources:LocalizedText, lblAmber %>"
                                            CssClass="field_txt"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Image ID="imgGreen" runat="server" ImageUrl="~/images/Green.jpg" Width="10" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblGreen" runat="server" Text="<%$ Resources:LocalizedText, lblGreen %>"
                                            CssClass="field_txt"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <div style="height: 100%; width: 100%;">
                            <asp:Timer ID="TMRefresh" runat="server" Interval="600000" OnTick="RefreshOneDayCard_Tick">
                            </asp:Timer>
                        </div>

                        <td>
                            <asp:ListView ID="lstvwResults" OnItemCommand="LstvwResults_ItemCommand" OnItemDataBound="LstvwResults_ItemDataBound"
                                runat="server">
                                <LayoutTemplate>
                                    <table cellpadding="0" cellspacing="0" class="gridStyle" style="border: 1px solid #808080; width: 1338px">
                                        <tr style="background-color: #EFF3FB; border-color: Gray; text-align: center">
                                            <th align="center" style="width: 30px; border-right: 1px solid #808080; border-left: 1px solid #808080;">
                                                <asp:Label ID="lblSNo" runat="server" Text="<%$ Resources:LocalizedText, SlNo %>"></asp:Label>
                                            </th>
                                            <th style="width: 50px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:LocalizedText, Status %>"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 85px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblAssociateID" runat="server" Text="<%$ Resources:LocalizedText, AssociateId %>"></asp:Label>
                                                <%--  <asp:LinkButton ID="lnkAssociateID" runat="server" CommandName="Sort" CommandArgument="AssociateID"
                                                    Text="Associate ID"></asp:LinkButton>--%>
                                            </th>
                                            <th align="center" style="width: 150px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalizedText, AssociateName %>"></asp:Label>
                                                <%--<asp:LinkButton ID="lnkAssociateName" runat="server" CommandName="Sort" CommandArgument="AssociateName"
                                                    Text="Associate Name"></asp:LinkButton>--%>
                                            </th>
                                            <th align="center" style="width: 80px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblInDate" runat="server" Text="<%$ Resources:LocalizedText, InDate %>"></asp:Label>
                                            </th>
                                            <%--    gggg--%>
                                            <th align="center" style="width: 80px; border-right: 1px solid #808080;">
                                                <asp:Label ID="LblToDate" runat="server" Text="<%$ Resources:LocalizedText, ToDate %>"></asp:Label>
                                            </th>
                                            <%--    gggg--%>
                                            <th align="center" style="width: 60px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblInTime" runat="server" Text="<%$ Resources:LocalizedText,InTime %>"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 80px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblOutDate" runat="server" Text="<%$ Resources:LocalizedText, OutDate %>"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 64px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblOutTime" runat="server" Text="<%$ Resources:LocalizedText, OutTime %>"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 83px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblBadgeNumber" runat="server" Text="<%$ Resources:LocalizedText, BadgeNo %>"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 100px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblAccessCardNumber" runat="server" Text="<%$ Resources:LocalizedText, AccessCardNo %>"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 80px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblBadgeStatus" runat="server" Text="<%$ Resources:LocalizedText, BadgeStatus %>"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 100px; border-right: 1px solid #808080;">
                                                <asp:Label ID="lblIssuedFacility" runat="server" Text="IssuedFacility"></asp:Label>
                                            </th>
                                            <th style="border-right: 1px solid #808080;">
                                                <asp:Label ID="lblActions" runat="server" Text="<%$ Resources:LocalizedText, Actions %>"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class="even_row">
                                        <td align="right" style="padding-right: 5px; border-right: 1px solid #808080; border-left: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblId" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Image ID="imgStatus" runat="server" ImageUrl='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"~/Images/Amber1.jpg":"~/Images/Green.jpg"%>'
                                                Width="10" />
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblAssociateID" Text='<%#Eval("AssociateID")%>'></asp:Label>
                                        </td>
                                        <td align="left" style="border-right: 1px solid #808080; padding-left: 5px">
                                            <asp:Label runat="server" ID="lblAssociateName" Text='<%#Eval("AssociateName")%>'></asp:Label>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="startdate">
                                                <%#Eval("IssuedDate")%></label>
                                            <%--   <asp:Label runat="server" ID="lblIssuedDate" Text='<%#Eval("IssuedDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                            --%>
                                            <td align="center" style="border-right: 1px solid #808080;">
                                                <label id="enddate">
                                                    <%#Eval("ToDate")%></label>
                                                <%--   <asp:Label runat="server" ID="lblTo" Text='<%#Eval("ToDate","{0:dd-MMM-yyyy}")%>'></asp:Label>--%>
                                            </td>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="starttime">
                                                <%#Eval("IssuedDate", "{0:H:mm}")%></label>
                                            <%--   <asp:Label runat="server" ID="lblIssuedTime" Text='<%#Eval("IssuedDate","{0:hh:mm tt}")%>'></asp:Label>
                                            --%>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="ReturnedDate">
                                                <%#Eval("ReturnedDate")%></label>
                                            <%--  <asp:Label runat="server" ID="lblReturnedDate" Text='<%#Eval("ReturnedDate","{0:dd-MMM-yyyy}")%>'></asp:Label>--%>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="ReturnedTime">
                                                <%#Eval("ReturnedDate", "{0:H:mm}")%></label>
                                            <%--   <asp:Label runat="server" ID="lblReturnedTime" Text='<%#Eval("ReturnedDate","{0:hh:mm tt}")%>'></asp:Label>--%>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblPassNumber" Text='<%#Eval("PassNumber")%>'></asp:Label>
                                            <asp:Label runat="server" ID="hdnReminderCount" Visible="false" Text='<%#Eval("Remindercount")%>'></asp:Label>
                                            <%-- <asp:HiddenField ID="hdnReminderCount" runat="server" Value='<%#Eval("Remindercount")%>' />--%>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblAccessCardNo" Text='<%#Eval("AccessCardNo")%>'></asp:Label>
                                        </td>
                                        <td align="left" style="border-right: 1px solid #808080; padding-left: 10px">
                                            <asp:Label runat="server" ID="lblBadgeStatus" Text='<%#Eval("BadgeStatus")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnIssuedDate" Value='<%#Eval("IssuedDate")%>' runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hdnGridToDate" Value='<%#Eval("ToDate")%>' runat="server"></asp:HiddenField>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblIssuedFacility" Text='<%#Eval("IssuedLocation")%>'></asp:Label>
                                            <asp:Label runat="server" ID="hdnLocationId" Visible="false" Text='<%#Eval("IssuedLocationID")%>'></asp:Label>
                                            <asp:Label runat="server" ID="hdnIssuedCityName" Visible="false" Text='<%#Eval("IssuedCity")%>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:LinkButton ID="btnlstCheckOut" runat="server" Text="<%$ Resources:LocalizedText, Checkout %>"
                                                Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("PassDetailID") + ";" +Eval("AssociateID") +";"+Eval("IssuedLocationID") +";"+Eval("IssuedLocation") +";"+Eval("IssuedCity") %>' CommandName="CheckOut"
                                                CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>'
                                                OnClientClick="return confirm('Are you certain you want to checkout this request?');" />
                                            &nbsp;
                                           <asp:LinkButton ID="btnLost" runat="server" Text="<%$ Resources:LocalizedText, Lost %>"
                                               Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                               CommandArgument='<%#Eval("PassDetailID") + ";" +Eval("AssociateID") + ";" +Eval("AccessCardNo") +";"+Eval("IssuedLocationID") %>' CommandName="Lost"
                                               CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>' />
                                            &nbsp;
                                            <asp:LinkButton ID="btnenablecard" runat="server" Text="Enable permanent card"
                                                Enabled='<%#(Eval("EnableCard").Equals(DBNull.Value)) ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                Visible='<%#(Eval("AccessCardNo").Equals("")) || (Eval("IssuedDate").Equals(Eval("ToDate"))) ?Convert.ToBoolean(0):Convert.ToBoolean(1)%>'
                                                CommandArgument='<%#Eval("PassDetailID") + ";" +Eval("AssociateID") + ";" +Eval("AccessCardNo")%>' CommandName="enablecard"
                                                CssClass='<%#(Eval("EnableCard").Equals(DBNull.Value)) ?"GridLinkButton":"GridLinkButtonDisabled"%>' />

                                            <asp:LinkButton ID="btnPrint" runat="server" Text="<%$ Resources:LocalizedText, Reprint %>"
                                                Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("PassDetailID") + ";" + Eval("AssociateID")%>' CommandName="RePrint"
                                                CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>' />
                                            &nbsp;<asp:LinkButton ID="btnView" Text="<%$ Resources:LocalizedText, btnView %>"
                                                runat="server" CommandArgument='<%#Eval("AssociateID") + ";" +Eval("BadgeStatus") +";" + Eval("IssuedDate")+";" + Eval("ToDate") +";"+Eval("IssuedLocationID")%>'
                                                CommandName="ViewDetailsLink" CssClass="GridLinkButton" />
                                            &nbsp;
                                            <asp:LinkButton ID="btnSendReminder" runat="server" Text="<%$ Resources:LocalizedText, ReminderMail %>"
                                                Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("AssociateID")+ ";" +Eval("AssociateName")+ ";" + Eval("ToDate","{0:dd-MMM-yyyy}")+ ";" + Eval("ToDate","{0:hh:mm tt}")+ ";" + Eval("IssuedDate","{0:dd-MMM-yyyy}") + ";" + Eval("PassDetailID") +";"+Eval("IssuedLocation")  +";"+Eval("IssuedCity") %> '
                                                CommandName="SendReminder" CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>' />
                                            <MB:MessageBox ID="MsgBox1" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr style="background-color: #CDE3F1; color: black; border-color: Gray" class="odd_row">
                                        <td align="right" style="padding-right: 5px; border-right: 1px solid #808080; border-left: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblId" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Image ID="imgStatus" runat="server" ImageUrl='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"~/Images/Amber1.jpg":"~/Images/Green.jpg"%>'
                                                Width="10" />
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblAssociateID" Text='<%#Eval("AssociateID")%>'></asp:Label>
                                        </td>
                                        <td align="left" style="border-right: 1px solid #808080; padding-left: 5px">
                                            <asp:Label runat="server" ID="lblAssociateName" Text='<%#Eval("AssociateName")%>'></asp:Label>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="startdate">
                                                <%#Eval("IssuedDate")%></label>
                                            <%--     <asp:Label runat="server" ID="lblIssuedDate" Text='<%#Eval("IssuedDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                            --%>
                                            <td align="center" style="border-right: 1px solid #808080;">
                                                <label id="enddate">
                                                    <%#Eval("ToDate")%></label>
                                                <%--       <asp:Label runat="server" ID="lblTo" Text='<%#Eval("ToDate","{0:dd-MMM-yyyy}")%>'></asp:Label>--%>
                                            </td>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="starttime">
                                                <%#Eval("IssuedDate", "{0:H:mm}")%></label>
                                            <%--           <asp:Label runat="server" ID="lblIssuedTime" Text='<%#Eval("IssuedDate","{0:hh:mm tt}")%>'></asp:Label>--%>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="ReturnedDate">
                                                <%#Eval("ReturnedDate")%></label>
                                            <%--     <asp:Label runat="server" ID="lblReturnedDate" Text='<%#Eval("ReturnedDate","{0:dd-MMM-yyyy}")%>'></asp:Label>--%>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <label id="ReturnedTime">
                                                <%#Eval("ReturnedDate", "{0:H:mm}")%></label>
                                            <%--                              <asp:Label runat="server" ID="lblReturnedTime" Text='<%#Eval("ReturnedDate","{0:hh:mm tt}")%>'></asp:Label>--%>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblPassNumber" Text='<%#Eval("PassNumber")%>'></asp:Label>
                                            <asp:Label runat="server" ID="hdnReminderCount" Visible="false" Text='<%#Eval("Remindercount")%>'></asp:Label>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblAccessCardNo" Text='<%#Eval("AccessCardNo")%>'></asp:Label>
                                        </td>
                                        <td align="left" style="border-right: 1px solid #808080; padding-left: 10px">
                                            <asp:Label runat="server" ID="lblBadgeStatus" Text='<%#Eval("BadgeStatus")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnIssuedDate" Value='<%#Eval("IssuedDate")%>' runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hdnGridToDate" Value='<%#Eval("ToDate")%>' runat="server"></asp:HiddenField>
                                        </td>
                                        <td align="center" style="border-right: 1px solid #808080;">
                                            <asp:Label runat="server" ID="lblIssuedFacility" Text='<%#Eval("IssuedLocation")%>'></asp:Label>
                                            <asp:Label runat="server" ID="hdnLocationId" Visible="false" Text='<%#Eval("IssuedLocationID")%>'></asp:Label>
                                            <asp:Label runat="server" ID="hdnIssuedCityName" Visible="false" Text='<%#Eval("IssuedCity")%>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:LinkButton ID="btnlstCheckOut" runat="server" Text="<%$ Resources:LocalizedText, Checkout %>"
                                                Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("PassDetailID") + ";" +Eval("AssociateID") +";"+Eval("IssuedLocationID") +";"+Eval("IssuedLocation") +";"+Eval("IssuedCity") %>' CommandName="CheckOut"
                                                CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>'
                                                OnClientClick="return confirm('Are you certain you want to checkout this request?');" />
                                            &nbsp;
                                            <asp:LinkButton ID="btnLost" runat="server" Text="<%$ Resources:LocalizedText, Lost %>"
                                                Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("PassDetailID") + ";" +Eval("AssociateID") + ";" +Eval("AccessCardNo") +";"+Eval("IssuedLocationID") %>' CommandName="Lost"
                                                CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>' />
                                            &nbsp;
                                            <asp:LinkButton ID="btnenablecard" runat="server" Text="Enable permanent card"
                                                Enabled='<%#(Eval("EnableCard").Equals(DBNull.Value)) ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("PassDetailID") + ";" +Eval("AssociateID") + ";" +Eval("AccessCardNo")%>' CommandName="enablecard"
                                                Visible='<%#(Eval("AccessCardNo").Equals("")) || (Eval("IssuedDate").Equals(Eval("ToDate"))) ?Convert.ToBoolean(0):Convert.ToBoolean(1)%>'
                                                CssClass='<%#(Eval("EnableCard").Equals(DBNull.Value)) ?"GridLinkButton":"GridLinkButtonDisabled"%>' />


                                            <asp:LinkButton ID="btnPrint" runat="server" Text="<%$ Resources:LocalizedText, Reprint %>"
                                                Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("PassDetailID") + ";" + Eval("AssociateID")%>' CommandName="RePrint"
                                                CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>' />
                                            &nbsp;<asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:LocalizedText, btnView %>"
                                                CommandArgument='<%#Eval("AssociateID") + ";" +Eval("BadgeStatus") +";" + Eval("IssuedDate")+";" + Eval("ToDate") +";"+Eval("IssuedLocationID")%>' CommandName="ViewDetailsLink"
                                                CssClass="GridLinkButton" />
                                            &nbsp;
                                            <asp:LinkButton ID="btnSendReminder" runat="server" Text="<%$ Resources:LocalizedText, ReminderMail %>"
                                                Enabled='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?Convert.ToBoolean(1):Convert.ToBoolean(0)%>'
                                                CommandArgument='<%#Eval("AssociateID")+ ";" +Eval("AssociateName")+ ";" + Eval("ToDate","{0:dd-MMM-yyyy}")+ ";" + Eval("ToDate","{0:hh:mm tt}")+ ";" + Eval("IssuedDate","{0:dd-MMM-yyyy}") + ";" + Eval("PassDetailID") +";"+Eval("IssuedLocation")  +";"+Eval("IssuedCity") %> '
                                                CommandName="SendReminder" CssClass='<%#(Eval("BadgeStatus").ToString().ToUpper().Trim()=="ISSUED") ?"GridLinkButton":"GridLinkButtonDisabled"%>' />
                                            <MB:MessageBox ID="MsgBox1" runat="server" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                            <div class="divPagerStyle">
                                <asp:DataPager ID="pager" runat="server" PageSize="15" PagedControlID="lstvwResults"
                                    OnPreRender="Pager_PreRender">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Image" FirstPageImageUrl="~/images/arrow_bar.png"
                                            ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                        <asp:NextPreviousPagerField ButtonType="Image" PreviousPageImageUrl="~/images/left_arrow.png"
                                            ShowNextPageButton="False" />
                                        <asp:NumericPagerField ButtonCount="7" NumericButtonCssClass="command" CurrentPageLabelCssClass="current"
                                            NextPreviousButtonCssClass="command" />
                                        <asp:NextPreviousPagerField ButtonType="Image" NextPageImageUrl="~/images/arrow_right.png"
                                            ShowPreviousPageButton="False" />
                                        <asp:NextPreviousPagerField ButtonType="Image" LastPageImageUrl="~/images/arrow_bar_right.png"
                                            ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <%--   code by ram--%>
    <asp:Panel ID="idcardlostcheck" runat="server" CssClass="ModalWindow" Style="display: none;">
        <table width="100%">
            <caption>
                <br />
                <tr>
                    <td align="center" class="table_header_bg" colspan="9">
                        <asp:Label ID="IDCARDLOST" runat="server" CssClass="Table_header" Text="Are you sure you want to check out the respective card as lost?"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="Button5" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" OnClick="BtnidcardLostConfirm_Click" Text="<%$ Resources:LocalizedText, btnConfirm %>" />
                        &nbsp;<asp:Button ID="Button8" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" Text="<%$ Resources:LocalizedText, btnCancel %>" />
                        <br />
                    </td>
                </tr>
            </caption>

        </table>
    </asp:Panel>
    <%--   code by ram for temp access card --%>
    <asp:Panel ID="enablecardpopup" runat="server" CssClass="ModalWindow" Style="display: none;">
        <table width="100%">
            <caption>
                <br />
                <tr>
                    <td align="center" class="table_header_bg" colspan="9">
                        <asp:Label ID="enablecard" runat="server" CssClass="Table_header" Text="Are you sure you want to disable the temporary card and activate permanent card?"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="Button9" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" OnClick="Btnenablecardpopup" Text="<%$ Resources:LocalizedText, btnConfirm %>" />
                        &nbsp;<asp:Button ID="Button10" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" Text="<%$ Resources:LocalizedText, btnCancel %>" />
                        <br />
                    </td>
                </tr>
            </caption>

        </table>
    </asp:Panel>

    <asp:Panel ID="lostcheck" runat="server" CssClass="ModalWindow" Style="display: none;">
        <table width="100%">

            <tr>
                <td align="left" class="table_header_bg" colspan="9" style="padding-left: 200px;">
                    <asp:Label ID="Label3" runat="server" CssClass="Table_header" Text="Select lost Item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rbtLstRating" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Style="padding-left: 55px;">
                        <asp:ListItem Text="OnlyIDCard" Value="OnlyIDCard"></asp:ListItem>
                        <asp:ListItem Text="OnlyAccessCard" Value="OnlyAccessCard"></asp:ListItem>
                        <asp:ListItem Text="BothID & AccessCard" Value="BothIDCardAndAccessCard"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button7" runat="server" Text="<%$ Resources:LocalizedText, btnConfirm %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px"
                        OnClick="BtnLostConfirm_Click" />
                    &nbsp;<asp:Button ID="Button6" runat="server" Text="<%$ Resources:LocalizedText, btnCancel %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" />
                    <br />
                </td>
            </tr>

        </table>
    </asp:Panel>
    <asp:Panel ID="panEdit" runat="server" CssClass="ModalWindow" Style="display: none;">
        <table width="100%">
            <tr>
                <td align="left" class="table_header_bg" colspan="9">
                    <asp:Label ID="Label1" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblInformation %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="padding: 0px 0px 0px 10px">
                    <br />
                    <p class="MsoNormal">
                        <asp:Label ID="lblCommitting" runat="server" Text="<%$ Resources:LocalizedText, lblCommitting %>"
                            CssClass="MsoNormal"></asp:Label>
                    </p>
                    <p class="MsoNormal">
                        <asp:Label ID="lblConfirm" Text="<%$ Resources:LocalizedText, lblConfirm %>" runat="server"
                            CssClass="MsoNormal"></asp:Label>
                    </p>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnConfirm" runat="server" Text="<%$ Resources:LocalizedText, btnConfirm %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px"
                        OnClick="BtnConfirm_Click" OnClientClick="return CheckIsRepeat();" />
                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedText, btnCancel %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panEdit1" runat="server" CssClass="ModalWindow" Style="display: none;">
        <table width="100%">
            <tr>
                <td align="left" class="table_header_bg" colspan="9">
                    <asp:Label ID="Label2" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblInformation %>"></asp:Label>
                </td>
            </tr>
            <tr>

                <td align="left" style="padding: 0px 0px 0px 10px">
                    <br />
                    <p class="MsoNormal">
                        <asp:Label ID="lblCommitting1" runat="server" Text="<%$ Resources:LocalizedText, lblCommitting1 %>"
                            CssClass="MsoNormal"></asp:Label>
                    </p>
                    <p class="MsoNormal">
                        <asp:Label ID="Label5" Text="<%$ Resources:LocalizedText, lblConfirm %>" runat="server"
                            CssClass="MsoNormal"></asp:Label>
                    </p>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="Button1" runat="server" Text="<%$ Resources:LocalizedText, btnConfirm %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px"
                        OnClick="BtnConfirm_Click" OnClientClick="return CheckIsRepeat();" />
                    &nbsp;<asp:Button ID="Button2" runat="server" Text="<%$ Resources:LocalizedText, btnCancel %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panEdit2" runat="server" CssClass="ModalWindow" Style="display: none;">
        <table width="100%">
            <tr>
                <td align="left" class="table_header_bg" colspan="9">
                    <asp:Label ID="Label6" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblInformation %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="padding: 0px 0px 0px 10px">
                    <br />
                    <p class="MsoNormal">
                        <asp:Label ID="lblCommitting2" runat="server" Text="<%$ Resources:LocalizedText, lblCommitting2 %>"
                            CssClass="MsoNormal"></asp:Label>
                    </p>
                    <p class="MsoNormal">
                        <asp:Label ID="Label8" Text="<%$ Resources:LocalizedText, lblConfirm %>" runat="server"
                            CssClass="MsoNormal"></asp:Label>
                    </p>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="Button3" runat="server" Text="<%$ Resources:LocalizedText, btnConfirm %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px"
                        OnClick="BtnConfirm_Click" OnClientClick="return CheckIsRepeat();" />
                    &nbsp;<asp:Button ID="Button4" runat="server" Text="<%$ Resources:LocalizedText, btnCancel %>"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlConfirm" runat="server" CssClass="ModalWindow" Style="display: none;">
        <table width="100%">
            <tr>
                <td align="left" class="table_header_bg" colspan="9">
                    <asp:Label ID="lblInformation" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblInformation %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="padding: 0px 0px 0px 10px">
                    <br />
                    <p class="MsoNormal">
                        <asp:Label ID="lblReminderCommitting" runat="server" Text="<%$ Resources:LocalizedText, lblReminderCommitting %>"
                            CssClass="MsoNormal"></asp:Label>
                    </p>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="btnConfirmReminder" runat="server" Text="Checked, Send reminder"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px"
                        OnClick="BtnConfirmReminder_Click" />
                    &nbsp;<asp:Button ID="btnCancelReminder" runat="server" Text="Cancel"
                        BackColor="#767561" Font-Bold="False" Font-Size="11px" ForeColor="White" Height="21px" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>


    <MB:MessageBox ID="MsgBox" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="mpeenablecardpopup" runat="server" TargetControlID="btnhidden"
        DropShadow="true" X="-1" Y="-1" PopupControlID="enablecardpopup" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" PopupDragHandleControlID="enablecardpopup">
    </ajaxToolkit:ModalPopupExtender>
    <ajaxToolkit:ModalPopupExtender ID="mpeidcard" runat="server" TargetControlID="btnhidden"
        DropShadow="true" X="-1" Y="-1" PopupControlID="idcardlostcheck" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" PopupDragHandleControlID="idcardlostcheck">
    </ajaxToolkit:ModalPopupExtender>
    <ajaxToolkit:ModalPopupExtender ID="mpelostcheck" runat="server" TargetControlID="btnhidden"
        DropShadow="true" X="-1" Y="-1" PopupControlID="lostcheck" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" PopupDragHandleControlID="lostcheck">
    </ajaxToolkit:ModalPopupExtender>
    <ajaxToolkit:ModalPopupExtender ID="mpeConfirm" runat="server" TargetControlID="btnhidden"
        DropShadow="true" X="-1" Y="-1" PopupControlID="panEdit" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" PopupDragHandleControlID="panEdit">
    </ajaxToolkit:ModalPopupExtender>
    <ajaxToolkit:ModalPopupExtender ID="mpeConfirm1" runat="server" TargetControlID="btnhidden"
        DropShadow="true" X="-1" Y="-1" PopupControlID="panEdit1" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" PopupDragHandleControlID="panEdit1">
    </ajaxToolkit:ModalPopupExtender>
    <ajaxToolkit:ModalPopupExtender ID="mpeConfirm2" runat="server" TargetControlID="btnhidden"
        DropShadow="true" X="-1" Y="-1" PopupControlID="panEdit2" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" PopupDragHandleControlID="panEdit2">
    </ajaxToolkit:ModalPopupExtender>
    <ajaxToolkit:ModalPopupExtender ID="mpeConfirmRemider" runat="server" TargetControlID="btnhiddenReminder"
        DropShadow="true" X="-1" Y="-1" PopupControlID="pnlConfirm" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" PopupDragHandleControlID="panEdit">
    </ajaxToolkit:ModalPopupExtender>
    <ajaxToolkit:ModalPopupExtender ID="modalReprintComments" BackgroundCssClass="modalBackground"
        TargetControlID="hdnPassDetailsID" PopupControlID="pnlReprintComments" CancelControlID="imgClose"
        runat="server" X="-1" Y="-1" DropShadow="true">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlReprintComments" runat="server" Width="300px" Height="125px" CssClass="ModalWindow"
        Style="display: none">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr style="background: #EDF6E3; height: 25px">
                <td style="padding-left: 10px" class="header_txt" valign="top" align="left">
                    <asp:Label ID="lblReprintReason" runat="server" Text="<%$ Resources:LocalizedText, SelectReprintCommand %>"></asp:Label>
                </td>
                <td width="16px" valign="top">
                    <div style="margin-top: 2px; float: right; margin-right: 2px;">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/Close.png" Height="14px" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">&nbsp;
                    <asp:DropDownList ID="ddlReason" runat="server" Width="200px" ValidationGroup="validReason">
                        <asp:ListItem Text="<%$ Resources:LocalizedText, Lost %>" Selected="true" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:LocalizedText, PrinterJammed %>" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;<asp:Button ID="btnPrint" CssClass="cssButton" runat="server" Text="<%$ Resources:LocalizedText, Print %>"
                        OnClick="BtnPrint_Click" ValidationGroup="validReason" />

                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">&nbsp;
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="<%$ Resources:LocalizedText, SelectReason %>"
                        CssClass="errorLabel" ControlToValidate="ddlReason" MinimumValue="1" MaximumValue="20"
                        Type="Integer" ValidationGroup="validReason"></asp:RangeValidator>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnhiddenReminder" runat="server" Text="Button" Style="display: none"
            OnClick="BtnHiddenReminder_Click" />
    </asp:Panel>
    <asp:Button ID="btnHiddenClose" runat="server" Text="Button" Style="display: none"
        OnClick="BtnHidden_Click" />
    <asp:Button ID="btnIVSHidden" runat="server" Style="display: none" Text="Button"
        OnClick="BtnIVSHidden_Click" />
    <asp:Button ID="btnHidden1" runat="server" Text="Button" Style="display: none" OnClick="BtnHidden1_Click" />
    <asp:HiddenField ID="hdnReminder" Value="0" runat="server" />

    <asp:HiddenField ID="hdnManagerEmailID" runat="server" />
    <asp:HiddenField ID="hdnManagerName" runat="server" />
    <asp:HiddenField ID="hdnEmployeeName" runat="server" />
    <asp:HiddenField ID="hdnAssociateID" runat="server" />
    <asp:HiddenField ID="hdnImageURL" runat="server" />
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>







