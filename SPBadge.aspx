<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SPBadge.aspx.cs" Inherits="VMSDev.SPBadge" %>

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
            white-space:nowrap;
            
        }
        .style21
        {
            vertical-align: top;
            width: 60px;
            line-height: 12px;
            white-space:nowrap;
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
            width: 150px;
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
            height: 29px;
            width: 110px;
        }
        .style45
        {
            height: 23px;
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
            width: 20px;
            height: 1px;
        }
        .style66
        {
            height: 1px;
        }
    </style>
        <script language="javascript" type="text/javascript">
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
                $("#lblDateTimeIngen").text((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);
                $("#lblTimIngen").text(meetingstarttime[3])


                $("#lblToDateTimeIngen").text((meetingendtime[2].length < 2 ? '0' + meetingendtime[2] : meetingendtime[2]) + ' ' + meetingendtime[1] + ' ' + meetingendtime[5]);
                $("#lblTimOutIngen").text(meetingendtime[3])
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
    <form id="form2" runat="server">
    <table style="height: 272px; width: 745px;">
        <tr>
            <td align="left" class="style41">
                <table id="outtertable" runat="server" cellspacing="0" cellpadding="0" style="height: 272px;
                    width: 334px; border-width: thin; border-style: solid; border-color: Black; table-layout: fixed">
                    <tr align="center">
                        <td>
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
                                                    <img id="cognizantlogo" src="~/Images/Logo_cognizant.jpg" runat="server" visible="true"
                                                        align="left" alt="<%$ Resources:LocalizedText, Logo %>" style="height: 34px; width: 110px;" />
                                                </td>
                                                <td class="style65">
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style20" >
                                                    <asp:Label ID="Label2" runat="server"  Text="<%$ Resources:LocalizedText, VisitorsPass %>"  class="Smallconstants">  </asp:Label>
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style66">
                                                    <asp:Label ID="lblbadge" runat="server" class="Smallconstants"></asp:Label>
                                                </td>
                                            </tr>
                                       <%--       <tr>
                                                <td class="style59">
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style37">
                                                    <asp:Label ID="LabelRequestId" runat="server" Class="Smallconstants" Width="78px" 
                                                        Height="16px" Text="Request Id:"></asp:Label>
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style59">
                                                     <asp:Label ID="lblRequestId" runat="server" Class="Smallconstants" Height="16px"></asp:Label>
                                                </td>
                                            </tr>--%>

                                            <tr>
                                                <td class="style59">
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style37">
                                                    <asp:Label ID="Label23" runat="server" Class="Smallconstants" Width="78px" 
                                                        Height="16px" Text="<%$ Resources:LocalizedText, BadgeFacility %>"></asp:Label>
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style59">
                                                    <asp:Label ID="lblCity" runat="server" Class="Smallconstants" Height="16px"></asp:Label> - <asp:Label
                                                        ID="lblFacility" runat="server" Class="Smallconstants" Height="16px"></asp:Label>
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
                                                    <asp:Label ID="lblNamehead" runat="server" Font-Bold="True" Class="constantNamelabelbold"  Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>
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
                                                    <asp:Label ID="Label6" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, Status %>"></asp:Label>
                                                </td>
                                                <td class="style31" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="style32">
                                                    <asp:Label ID="lblPurpose" runat="server" Class="constantlabel"></asp:Label>
                                                </td>
                                                <td rowspan="11" style="width: 114px; vertical-align: bottom" align="right">
                                                    <asp:Image ID="imgVisitor" runat="server" ImageUrl="~/Images/DummyPhoto.png" Width="125px"
                                                        align="right" Height="131px" ImageAlign="Right" />
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
                                                
                                            </tr>

                                            <%-- added by bincey for SP badge--%>

                                            <td></td>                                            
                                             
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    <asp:Label ID="lblBuddy" runat="server" Class="Smallconstants" Text="Buddy"></asp:Label>
                                                </td>
                                                <td class="style23" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="DateTimeIn" >                                                
                                                    <label id="lblBuddyIdValue" runat="server" class="Smallconstants" />
                                                    &nbsp;<label id="Label5" runat="server" class="Smallconstants" />
                                                </td>
                                                
                                            </tr>



                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    <asp:Label ID="Label7" runat="server" Class="Smallconstants" Text="<%$ Resources:LocalizedText, From %>"></asp:Label>
                                                </td>
                                                <td class="style23" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="DateTimeIn" >
                                                
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
                                                <td align="left" class="style30">
                                                <asp:Label ID="lblPermitType" runat="server" Class="Smallconstants" Text="PermitType"></asp:Label>
                                                    &nbsp;
                                                </td>
                                                <td class="style61">
                                                    :
                                                </td>
                                                <td align="left" class="style62">
                                                                                                 
                                                    <label id="lblPermitTypeValue" runat="server" class="Smallconstants" />
                                                    &nbsp;<label id="Label9" runat="server" class="Smallconstants" />
                                                
                                                </td>
                                            </tr>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    <asp:Label ID="lblRequestId" runat="server" Class="Smallconstants" Width="78px" 
                                                        Height="16px" Text="Request Id:"></asp:Label>
                                                </td>
                                                <td class="style23">
                                                    :&nbsp;
                                                </td>
                                                <td align="left" class="Variables_TD">
                                                    <asp:Label ID="lblRequestIdValue" runat="server" Class="Smallconstants" Height="16px"></asp:Label>
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
                                                <td style="white-space:nowrap">
                                                    <p style="font-size: 9px; font-family: Interstate-Regular; width: 284px; margin-top: 0px;">
                                                    <asp:Label Id="lblBadgeNote" runat="server" Text="<%$ Resources:LocalizedText, lblBadgeNote %>"></asp:Label>
                                                       </p>
                                                </td>
                                                <td align="right">
                                                    <img id="Img3" src="~/Images/No_Smoke_Cam.jpg" runat="server" visible="true" 
                                                        style="height: 16px; width: 28px; vertical-align: bottom" />
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
                    <table id="Table2" runat="server" cellspacing="0" cellpadding="0" style="height: 272px;
                        width: 334px; border-width: thin; border-style: solid; border-color: Black; table-layout: fixed">
                        <tr align="center">
                            <td class="style58">
                                <table id="Table3" cellspacing="0" cellpadding="0" style="height: 272px; width: 321px;
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
                                                        <img id="Img2" src="~/Images/Logo_cognizant.jpg" runat="server" visible="true" align="left"
                                                            alt="<%$ Resources:LocalizedText, Logo %>" style="height: 34px; width: 110px;" />
                                                    </td>
                                                    <td class="style19">
                                                    </td>
                                                    <td align="right" style="vertical-align: middle; font-family: Interstate-Bold; font-size: 8px;"
                                                        valign="middle" class="style63">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:LocalizedText, VisitorsPass %>"  Style="height: 34px;
                                                            vertical-align: top; font-family: Interstate-Bold; font-size: 11px;">  </asp:Label>
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
                                                        <img id="Img5" src="~/Images/No_Smoke_Cam.jpg" runat="server" visible="true" 
                                                            style="height: 25px; width: 45px; vertical-align: bottom" align="right" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" class="style42">
                                            <table cellspacing="0" cellpadding="0" style="width: 328px; height: 91px">
                                                <tr class="Rowstyle">
                                                    <td align="left" class="style16">
                                                        <asp:Label ID="Label17" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, EquipmentPermitted %>" ></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="Rowstyle2">
                                                    <td align="left" class="style6" valign="top" >
                                                     <center><asp:Label ID="lblNoEquipment" CssClass="Smallconstants" Visible="false" runat="server" Text="<%$ Resources:LocalizedText, NoEquipmentPermitted %>"  style="font-weight:bold"></asp:Label>
                                                        </center><asp:GridView Style="vertical-align: top" Width="100%" ID="GridView1" runat="server"
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
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" class="style44">
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
                                            <table style="width: 328px;">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <p style="font-size: 9px; font-family: Interstate-Regular;">
                                                        <asp:label Id="lblNote" runat="server" Text="<%$ Resources:LocalizedText, Note %>"></asp:label>
                                                          </p>
                                                    </td>
                                                    <td align="left" valign="top" style="white-space:nowrap">
                                                        <p style="font-size: 9px; font-family: Interstate-Regular;">
                                                        <asp:Label Id="LblEmergency" runat="server" Text="<%$ Resources:LocalizedText, lblEmergencyContactInstr %>"></asp:Label>
                                                             -&nbsp;
                                                            <asp:Label ID="lblsecurityno" runat="server" Class="Notestyle"></asp:Label></br>
                                                            <asp:Label Id="lblFollow" runat="server" Text="<%$ Resources:LocalizedText, KindlyFollow %>"></asp:Label></p>
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
    <div id="printOption" class="noPrint" style="border-bottom-color: White;">
    <a href="javascript:imgPrint_onclick()"><asp:label Id="print" runat="server" Text="<%$ Resources:LocalizedText, Print %>"></asp:label></a><br />
        <br />
    </div>
    </form>

</body>



</html>
