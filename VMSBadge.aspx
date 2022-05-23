<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VMSBadge.aspx.cs" Inherits="VMSDev.VMSBadge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Badge</title>
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <link href="App_Themes/UIStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        @media print
        {
            .noPrint
            {
                display: none;
            }       
        
        }
        .Smallconstants
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            line-height: 12px;
            margin-left: 0px;
        }       
        
        .TokenNumber
        {                  
           position:fixed;
            height:35px;
            width:35px; 
            top:102px;
            left:516px
        }
        .Smallconstants td
        {
            padding-left: 5px;
        }
        
        .Notestyle
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 9px;
            line-height: 12px;
        }
        .constantlabelbold
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            width: 50px;
            line-height: 12px;
        }
        .constantNamelabelbold
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 12px;
            width: 50px;
            line-height: 12px;
        }
        .constantlabel
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            width: 50px;
            line-height: 12px;
        }
        .constantlabel2
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 7px;
            width: 50px;
            line-height: 12px;
        }
        .Rowstyle
        {
            width: 330px;
            vertical-align: top;
            line-height: 11px;
        }
        .Rowstyle2
        {
            width: 330px;
            vertical-align: top;
            line-height: 12px;
        }
        .constant_TD
        {
            vertical-align: top;
            width: 50px;
            line-height: 12px;
        }
        .Variables_TD
        {
            vertical-align: top;
            line-height: 12px;
        }
        .VariablesName_TD
        {
            width: 131px;
            vertical-align: top;
            line-height: 12px;
        }
        .style6
        {
            vertical-align: middle;
            line-height: 12px;
        }
        .style16
        {
            vertical-align: top;
            line-height: 12px;
            height: 15px;
        }
        .style19
        {
            width: 20px;
            height: 2px;
        }
        .style20
        {
            width: 85px;
            height: 1px;
            white-space: nowrap;
        }
        .style21
        {
            vertical-align: top;
            width: 60px;
            line-height: 12px;
            white-space: nowrap;
        }
        .style23
        {
            width: 11px;
            white-space: nowrap;
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Bold;
            font-size: 10px;
        }
        .style30
        {
            vertical-align: top;
            width: 60px;
            line-height: 12px;
            height: 18px;
        }
        .style31
        {
            width: 11px;
            white-space: nowrap;
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Bold;
            font-size: 10px;
            height: 18px;
        }
        .style32
        {
            vertical-align: top;
            line-height: 12px;
            height: 18px;
        }
        .style33
        {
            vertical-align: top;
            width: 60px;
            line-height: 12px;
            height: 20px;
        }
        .style34
        {
            width: 11px;
            white-space: nowrap;
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Bold;
            font-size: 10px;
            height: 20px;
        }
        .style35
        {
            vertical-align: top;
            line-height: 12px;
            height: 20px;
        }
        .style37
        {
            width: 76px;
            height: 24px;
        }
        .style38
        {
            vertical-align: top;
            width: 60px;
            line-height: 12px;
            height: 16px;
        }
        .style39
        {
            width: 11px;
            white-space: nowrap;
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Bold;
            font-size: 10px;
            height: 16px;
        }
        .style40
        {
            vertical-align: top;
            line-height: 12px;
            height: 16px;
            width:auto;
         
        }
        .style41
        {
            width: 334px;
        }
        .style42
        {
            height: 112px;
            width: 110px;
        }
        .style44
        {
            height: 18px;
            width: 110px;
        }
        .style45
        {
            height: 15px;
            width: 110px;
        }
        .style46
        {
            width: 200px;
        }
        .style47
        {
            height: 2px;
        }
        .style57
        {
            width: 110px;
        }
        .style58
        {
            height: 260px;
        }
        .style59
        {
            height: 24px;
        }
        .style60
        {
            vertical-align: top;
            width: 53px;
            line-height: 12px;
            height: 14px;
        }
        .style61
        {
            width: 11px;
            white-space: nowrap;
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Bold;
            font-size: 10px;
            height: 14px;
        }
        .style62
        {
            vertical-align: top;
            line-height: 12px;
            height: 14px;
        }
        .style63
        {
            width: 140px;
            height: 2px;
        }
        .style64
        {
            width: 140px;
            height: 24px;
        }
        .style65
        {
            width: 10px;
            height: 1px;
        }
        .style66
        {
            height: 1px;
        }
        .style67
        {
            height: 272px;
            width: 326px;
            table-layout: fixed;
        }
        .style68
        {
            width: 322px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        //            function SelectValue() {
        //                 
        //                var ss = document.getElementById('lblDateTimeIngen').innerText;
        //                document.getElementById('<%= lblDateTimeIngen.ClientID %>').focus();
        //                document.getElementById('lblDateTimeIngen').focus();
        //            }

        $(document).ready(function () {
            TimeConversion();
        })

        function refreshPage() {
            if (window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_ViewLogbySecurity1_btnHidden") != null) {
                window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_ViewLogbySecurity1_btnHidden").click();
            }

            if (window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_btnHidden1") != null) {
                window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_btnHidden1").click();
            }
        }
        function imgPrint_onclick() {
            window.print();
            window.close();
        }

        function TimeConversion() {

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
            var Startdate = document.getElementById('lblDateTimeIngen').innerText;
            var StartTime = document.getElementById('lblTimIngen').innerText;

            var Enddate = document.getElementById('lblToDateTimeIngen').innerText;
            var EndTime = document.getElementById('lblTimOutIngen').innerText;

            var meetingstarttime = (new Date(Startdate.split('-') + ' ' +
         StartTime.split(' ')[0]).toString().split(' '))


            var meetingendtime = (new Date(Enddate.split('-') + ' ' +
         EndTime.split(' ')[0]).toString().split(' '))

            var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
            meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');

            var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
            meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');

            try {
                $("#lblDateTimeIngen").text((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);
                $("#lblTimIngen").text(meetingstarttime[3]);


                $("#lblToDateTimeIngen").text((meetingendtime[2].length < 2 ? '0' + meetingendtime[2] : meetingendtime[2]) + ' ' + meetingendtime[1] + ' ' + meetingendtime[5]);
                $("#lblTimOutIngen").text(meetingendtime[3]);
            }
            catch (e)
            {}
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
</head>
<body onunload="refreshPage()" style="background-color: White; border-bottom-color: white">
    <form runat="server">
    <div id="printOption" class="noPrint" style="border-bottom-color: White;">
        &nbsp;&nbsp<a href="javascript:imgPrint_onclick()"><asp:Label ID="print" runat="server"
            Text="<%$ Resources:LocalizedText, Print %>"></asp:Label></a>
    </div>
    <table style="width: 745px; height: 290px">
        <tr>
            <td align="left" class="style41" valign="top">
                <table id="outtertable" runat="server" cellspacing="0" cellpadding="0" style="border: thin solid Black;"
                    class="style67">
                    <tr valign="top">
                        <td class="style68" valign="top">
                            <table id="innertable" cellspacing="0" cellpadding="0" style="height: 272px; width: 321px;
                                border-color: White; table-layout: fixed">
                                <tr align="center" style="height: 34px;">
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="321px" style="line-height: 10px; height: 10px;">
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="Table1" cellspacing="0" cellpadding="0" style="height: 43px; width: 322px;
                                            border-color: White; table-layout: fixed">
                                            <tr>
                                                <td style="width: 110px; vertical-align: top;" align="left" rowspan="2">
                                                    <%--<img id="cognizantlogo" src="~/Images/Logo_cognizant.jpg" runat="server" visible="true"
                                                        align="left" alt="<%$ Resources:LocalizedText, Logo %>" style="height: 34px;
                                                        width: 110px;" />--%>
                                                     <img id="cognizantlogo" src="~/Images/Cognizant_Blue_HD.png" runat="server" visible="true"
                                                        align="left" alt="<%$ Resources:LocalizedText, Logo %>" style="height: 34px;
                                                        width: 110px;" />

                                                </td>
                                                <td class="style65">
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style20">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedText, VisitorsPass %>"
                                                        class="Smallconstants">  </asp:Label>
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style66">
                                                    <asp:Label ID="lblbadge" runat="server" class="Smallconstants"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style59">
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style37">
                                                    <asp:Label ID="Label23" runat="server" Class="Smallconstants" Width="78px" Height="16px"
                                                        Text="<%$ Resources:LocalizedText, BadgeFacility %>"></asp:Label>
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style59">
                                                    <%-- <asp:Label ID="lblCity" runat="server" Class="Smallconstants" Height="16px"></asp:Label>--%>
                                                    <%--           </br>--%>
                                                    <asp:Label ID="lblFacility" runat="server" Class="Smallconstants" Height="16px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left" valign="top">
                                    <td>
                                        <table cellspacing="0" cellpadding="0">
                                            <tr class="Rowstyle">
                                                <td align="left" class="style33">
                                                    <asp:Label ID="lblNamehead" runat="server" Font-Bold="True" Class="constantNamelabelbold"
                                                        Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>
                                                </td>
                                                <td class="style34" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="style35" colspan="2">
                                                    <asp:Label ID="lblName" runat="server" Font-Bold="True" Class="constantNamelabelbold"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle">
                                                <td align="left" class="style30">
                                                    <asp:Label ID="Label1" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, Organization %>"></asp:Label>
                                                </td>
                                                <td class="style31" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="style32" colspan="2">
                                                    <asp:Label ID="lblOrganisation" runat="server" Class="constantlabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle">
                                                <td align="left" class="style30">
                                                    <asp:Label ID="Label6" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, VisitorType %>"></asp:Label>
                                                </td>
                                                <td class="style31" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="style32">
                                                    <asp:Label ID="lblPurpose" runat="server" Class="constantlabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle">
                                                <td align="left" class="style38">
                                                    <asp:Label ID="Label4" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, Host %>"></asp:Label>
                                                </td>
                                                <td class="style39" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="style40">
                                                    <asp:Label ID="lblHostName" runat="server" Class="constantlabel"></asp:Label>
                                                </td>
                                                <td rowspan="11" style="width: 114px; vertical-align: bottom" align="right">
                                                    <asp:Image ID="imgVisitor" runat="server" ImageUrl="~/Images/DummyPhoto.png" Width="125px"
                                                        align="right" Height="131px" ImageAlign="Right" />
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    <asp:Label ID="Label7" runat="server" Class="Smallconstants" Text="<%$ Resources:LocalizedText, From %>"></asp:Label>
                                                </td>
                                                <td class="style23" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="DateTimeIn">
                                                    <label id="lblDateTimeIngen" runat="server" class="Smallconstants" />
                                                    &nbsp;<label id="lblTimIngen" runat="server" class="Smallconstants" />
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    <asp:Label ID="Label8" runat="server" Class="Smallconstants" Text="<%$ Resources:LocalizedText, To %>"></asp:Label>
                                                </td>
                                                <td class="style23" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="Variables_TD">
                                                    <label id="lblToDateTimeIngen" runat="server" class="Smallconstants" />
                                                    &nbsp;<label id="lblTimOutIngen" runat="server" class="Smallconstants" />
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    &nbsp;
                                                </td>
                                                <td class="style23">
                                                    &nbsp;
                                                </td>
                                                <td align="left" class="Variables_TD">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style60">
                                                    &nbsp;
                                                </td>
                                                <td class="style61">
                                                    &nbsp;
                                                </td>
                                                <td align="left" class="style62">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    &nbsp;
                                                </td>
                                                <td class="style23">
                                                    &nbsp;
                                                </td>
                                                <td align="left" class="Variables_TD">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    &nbsp;
                                                </td>
                                                <td class="style23">
                                                    &nbsp;
                                                </td>
                                                <td align="left" class="Variables_TD">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="left" valign="top">
                                    <td valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td style="white-space: nowrap">
                                                    <p style="font-size: 9px; font-family: Interstate-Regular; width: 284px; margin-top: 0px;">
                                                        <asp:Label ID="lblBadgeNote" runat="server" Text="<%$ Resources:LocalizedText, lblBadgeNote %>"></asp:Label>
                                                    </p>
                                                </td>
                                                <td align="right">
                                                    <img id="Img3" src="~/Images/No_Smoke_Cam.jpg" runat="server" visible="true" style="height: 16px;
                                                        width: 28px; vertical-align: bottom" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="center" axis="180" class="style46" style="font-size: 4px">
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
                |<br />
            </td>
            <td>
                <div id="equipdiv" runat="server">
                    <table id="Table2" runat="server" cellspacing="0" cellpadding="0" style="height: 290px;
                        width: 334px; border-width: thin; border-style: solid; border-color: Black; table-layout: fixed">
                        <tr align="center">
                            <td class="style58">
                                <table id="Table3" cellspacing="0" cellpadding="0" style="height: 290px; width: 321px;
                                    border-color: White;">
                                    <tr align="center" style="height: 34px;">
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="321px" style="line-height: 10px; height: 10px;">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="Table5" cellspacing="0" cellpadding="0" style="height: 43px; width: 322px;
                                                border-color: White; table-layout: fixed">
                                                <tr>
                                                    <td style="width: 110px; vertical-align: top;" align="left" rowspan="2">
                                                        <%--<img id="Img2" src="~/Images/Logo_cognizant.jpg" runat="server" visible="true" align="left"
                                                            alt="<%$ Resources:LocalizedText, Logo %>" style="height: 34px; width: 110px;" />--%>
                                                        <img id="CTSLogo" src="~/Images/Cognizant_Blue_HD.png" runat="server" visible="true"
                                                        align="left" alt="<%$ Resources:LocalizedText, Logo %>" style="height: 34px;
                                                        width: 110px;" />
                                                    </td>
                                                    <td class="style19">
                                                    </td>
                                                    <td align="right" style="vertical-align: middle; font-family: Interstate-Bold; font-size: 8px;"
                                                        valign="middle" class="style63">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:LocalizedText, VisitorsPass %>"
                                                            Style="height: 34px; vertical-align: top; font-family: Interstate-Bold; font-size: 11px;">  </asp:Label>
                                                    </td>
                                                    <td align="left" style="vertical-align: middle; font-family: Interstate-Bold; font-size: 8px;"
                                                        valign="top" class="style47">
                                                        <asp:Label ID="lblbadge2" runat="server" Style="height: 34px; vertical-align: top;
                                                            font-family: Interstate-Bold; font-size: 11px;"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style59">
                                                    </td>
                                                    <td align="right" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                        valign="top" class="style64">
                                                        &nbsp;
                                                    </td>
                                                    <td align="center" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                        valign="top" class="style59">
                                                        <img id="Img5" src="~/Images/No_Smoke_Cam.jpg" runat="server" visible="true" style="height: 25px;
                                                            width: 45px; vertical-align: bottom" align="right" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" class="style42">
                                            <table cellspacing="0" cellpadding="0" style="width: 328px; height: 91px">
                                                <tr class="Rowstyle">
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td align="center" style="height:20px">
                                                                    <asp:Label ID="lblEquipmentCust" runat="server" Font-Bold="True" Style="vertical-align: middle"
                                                                        Class="constantNamelabelbold" Text="<%$ Resources:LocalizedText, lblEquipmentCust %>"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                               
                                                                    <div style="text-align:center; position:relative;">
                                                                        <asp:Label ID="lblToken" runat="server" Style="font-weight: bold;  position:absolute;  top:13px;  left:10px;  text-align:center;"></asp:Label>
                                                                        <img src="Images/TokenBorderblack.png" visible="false" runat="server" id="imgToken"    />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="Rowstyle">
                                                    <td align="left" class="style16">
                                                        <asp:Label ID="Label17" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, EquipmentPermitted %>"></asp:Label>
                                                        <asp:Label ID="lblNoEquipment" CssClass="Smallconstants" Visible="false" runat="server"
                                                            Text="<%$ Resources:LocalizedText, NoEquipmentPermitted %>" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="Rowstyle2">
                                                    <td align="left" class="style6" valign="top">
                                                        <asp:GridView Style="vertical-align: top" Width="100%" ID="GridView1" runat="server"
                                                            CellSpacing="0" CellPadding="0" RowStyle-BorderColor="White" RowStyle-BorderStyle="None"
                                                            RowStyle-Height="12px" BorderColor="White" HeaderStyle-HorizontalAlign="Left"
                                                            HorizontalAlign="Left" CssClass="Smallconstants" EmptyDataText="<%$ Resources:LocalizedText, NoEquipmentPermitted %>"
                                                            EmptyDataRowStyle-VerticalAlign="Top" EmptyDataRowStyle-Height="91px" EmptyDataRowStyle-BorderWidth="5px"
                                                            EmptyDataRowStyle-BorderColor="White" EmptyDataRowStyle-HorizontalAlign="Center"
                                                            EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, EquipmentType %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEqpTyp" Style="padding-left: 2px" Text='<%# Bind("EquipmentType") %>'
                                                                            runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Make %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMake" Style="padding-left: 2px" Text='<%# Bind("Make") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Model %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMake" Style="padding-left: 2px" Text='<%# Bind("Model") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, SerialNo %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNo" Style="padding-left: 2px" Text='<%# Bind("SerialNo") %>'
                                                                            runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:GridView ID="grdEquipments" Style="vertical-align: top" Width="100%" runat="server"
                                                            CellSpacing="0" CellPadding="1" RowStyle-BorderColor="White" RowStyle-BorderStyle="None"
                                                            RowStyle-Height="8px" BorderColor="White" HeaderStyle-HorizontalAlign="Center"
                                                            HorizontalAlign="Center" CssClass="Smallconstants" EmptyDataRowStyle-VerticalAlign="Top"
                                                            EmptyDataRowStyle-Height="60px" EmptyDataRowStyle-BorderWidth="5px" EmptyDataRowStyle-BorderColor="White"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-Font-Bold="true"
                                                            AutoGenerateColumns="false">
                                                            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="White"
                                                                BorderWidth="5px" Font-Bold="False" Height="60px"></EmptyDataRowStyle>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <RowStyle BorderStyle="None" />
                                                            <Columns>
                                                                <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false" />
                                                                <asp:TemplateField HeaderText="EQUIPMENT TYPE" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEquipmentType" runat="Server" Text='<%# Bind("EquipmentTypeId")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="ddlEquipmentType" runat="server" Width="120px" AutoPostBack="true"
                                                                            Text='<%# Bind("EquipmentType")%>' Height="10px" />
                                                                        <%--<asp:TextBox ID="txtOtherEquipment" runat="server" name="txtOtherEquipment" Text='<%#Bind("Others")%>'
                                                    MaxLength="20" Style="width: 70px;" />--%>
                                                                    </ItemTemplate>
                                                                    <%--  <HeaderStyle Font-Bold="false" HorizontalAlign="Center" />--%>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" Wrap="False"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="DESCRIPTION" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtDescription" runat="Server" MaxLength="20" OnPaste="false" Text='<%# Bind("Description")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Font-Bold="true" HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="100%" class="style44" align="left">
                                            <asp:Label ID="Label25" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, HostSignature %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" class="style45">
                                            <asp:Label ID="Label26" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, Time %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="bottom">
                                        <td colspan="100%" valign="bottom" class="style57">
                                            <div class="Disclaimer" id="Disclaimer" runat="server" style="font-size: 9px; padding-top: 3px;
                                                font-family: Interstate-Regular; white-space: nowrap">
                                                <asp:Label ID="DisclaimerFirst" Visible="false" runat="server" Text="<%$ Resources:LocalizedText, Disclaimer %>"></asp:Label>
                                                <br /><span style="padding-left:48px">
                                                <asp:Label ID="DisclaimerNext" Visible="false" runat="server" Text="<%$ Resources:LocalizedText, DisclaimerNext %>"></asp:Label></span>
                                            </div>
                                            <table style="width: 328px;">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <p style="font-size: 9px; font-family: Interstate-Regular;">
                                                            <asp:Label ID="lblNote" runat="server" Text="<%$ Resources:LocalizedText, Note %>"></asp:Label>
                                                        </p>
                                                    </td>
                                                    <td align="left" valign="top" style="white-space: nowrap">
                                                        <p style="font-size: 9px; font-family: Interstate-Regular;">
                                                            <asp:Label ID="LblEmergency" runat="server" Text="<%$ Resources:LocalizedText, lblEmergencyContactInstr %>"></asp:Label>
                                                            -&nbsp;
                                                            <asp:Label ID="lblsecurityno" runat="server" Class="Notestyle"></asp:Label></br>
                                                            <asp:Label ID="lblFollow" runat="server" Text="<%$ Resources:LocalizedText, KindlyFollow %>"></asp:Label></p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
