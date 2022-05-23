<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemporaryIDCard.aspx.cs"
    Inherits="VMSDev.TemporaryIDCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @media print {
            .noPrint {
                display: none;
            }
        }

        .Smallconstants {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            line-height: 12px;
            margin-left: 0px;
        }


        .Notestyle {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 9px;
            line-height: 12px;
        }

        .constantlabelbold {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            width: 50px;
            line-height: 12px;
        }

        .constantNamelabelbold {
            text-align: left;
            vertical-align: top;
            font-family: Arial;
            font-size: 12px;
            width: 50px;
            line-height: 12px;
        }

        .constantlabel {
            text-align: left;
            vertical-align: middle;
            font-family: Arial;
            font-size: 12px;
            width: 300px;
            line-height: 14px;
        }

        .constantlabel2 {
            text-align: left;
            font-family: Arial;
            font-size: 10px;
            width: 100px;
            line-height: 22px;
        }

        .Rowstyle {
            height: 22px;
            width: 330px;
            vertical-align: middle;
            line-height: 11px;
        }

        .Rowstyle2 {
            width: 330px;
            vertical-align: top;
            line-height: 12px;
        }

        .constant_TD {
            vertical-align: top;
            width: 50px;
            line-height: 12px;
        }

        .Variables_TD {
            line-height: 12px;
        }

        .VariablesName_TD {
            width: 131px;
            vertical-align: top;
            line-height: 12px;
        }

        .IDFontBold {
            font-family: Arial Narrow;
            font-weight: bold;
        }

        .IDFontNormal {
            font-family: Arial Narrow;
            font-weight: normal;
        }

        .style1 {
            width: 27px;
        }
    </style>

    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        function GetImageFromIDCard() {
           
            var IsCHireON = false;
            const CHireKey = '<%=System.Configuration.ConfigurationManager.AppSettings["CHireKey"]%>';
            var apiresponse = 'API response : ';
            const APIEnvUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["IDCardApiUrl"]%>';
            var ChireFileContentId = $('#<%=hdnFileContent.ClientID%>').val();
            var id = $('#lblAssociateID').text().trim();

            if (CHireKey === 'ON')
                IsCHireON = true;

            $.ajax({
                type: 'GET',
                url: APIEnvUrl,
                xhrFields: {
                    withCredentials: true
                },
                data: { "associateId": id },
                dataType: 'json',
                success: function (image) {
                    debugger
                    if (image.indexOf("data:image") != -1) {
                        apiresponse += 'image found';
                        $('#imgAssociate').attr("src", image);
                    }
                    else {
                        debugger
                        apiresponse += image;
                        image = "Images/char.jpeg";
                        IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#imgAssociate').attr("src", image);
                    }

                    //TO DO: remove the log.
                    console.log(apiresponse);
                },
                error: function (e) {
                    debugger
                    apiresponse += "Error: Response JSON : " + e.responseJSON + " Status : " + e.status + " Response Text : " + e.responseText;
                    IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#imgAssociate').attr("src", "Images/char.jpeg");
                    //TO DO: remove the log.
                    console.log(apiresponse);
                }
            });
        }

        // function to get image from Chire.
        function GetImageFromChire(filecontentID) {
            //file content id is already fetched from CHIRE VIEW

           debugger
            if (filecontentID !== null && filecontentID !== '') {


                $.ajax({
                    type: 'POST',
                    url: "TemporaryIDCard.aspx/GetChireImageFromECM",
                    contentType: "application/json;charset=utf-8",
                    data: '{"contentId":"' + filecontentID + '"}',
                    dataType: 'json',
                    success: function (image) {
                        debugger
                        console.log('Displaying Chire Image');
                        $('#imgAssociate').attr("src", JSON.parse(image.d));

                    }
                  ,
                    error: function (e) {
                        debugger
                        console.log('Chire FLow error: ' + e.responseJSON);
                        $('#imgAssociate').attr("src", "Images/char.jpeg");


                    }
                });
            }
            else {
                debugger
                console.log('Chire file contnet id is null');
                $('#imgAssociate').attr("src", "Images/char.jpeg");

            }

        }



        function refreshPage() {
            if (window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_btnHiddenClose") != null) {
                window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_btnHiddenClose").click();
            }

            //            if (window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_btnHidden1") != null) {
            //                window.opener.document.getElementById("ctl00_VMSContentPlaceHolder_btnHidden1").click();
            //            }
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
            var Startdate = document.getElementById('lblFrom').innerText;
            var StartTime = document.getElementById('hdnFromTime').value;

            var Enddate = document.getElementById('lblTo').innerText;
            var EndTime = document.getElementById('hdnToTime').value;

            var meetingstarttime = (new Date(Startdate.split('-') + ' ' +
                StartTime.split(' ')[0]).toString().split(' '))


            var meetingendtime = (new Date(Enddate.split('-') + ' ' +
                EndTime.split(' ')[0]).toString().split(' '))

            var tempstarttime = gettimedtls(meetingstarttime[1], meetingstarttime[3], hours, mins).split('|');
            meetingstarttime = (new Date(meetingstarttime[5], tempstarttime[0], meetingstarttime[2], tempstarttime[2], tempstarttime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
            try {
                var tempendtime = gettimedtls(meetingendtime[1], meetingendtime[3], hours, mins).split('|');
                meetingendtime = (new Date(meetingendtime[5], tempendtime[0], meetingendtime[2], tempendtime[2], tempendtime[3])).toString().replace(/[0-9]{1,2}(:[0-9]{2}){2}/, function (time) { var hms = time.split(':'), h = +hms[0], suffix = (h < 12) ? 'AM' : 'PM'; hms[0] = h % 12 || 12; hms.splice(2, 1); return hms.join(':') + suffix }).split(' ');
                //$("#lblValidfrom").text((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);           
                $('#<%=lblFrom.ClientID %>').text((meetingstarttime[2].length < 2 ? '0' + meetingstarttime[2] : meetingstarttime[2]) + ' ' + meetingstarttime[1] + ' ' + meetingstarttime[5]);
                 //  $("#hdnFromTime").text(meetingstarttime[3])
                 $("#lblTo").text((meetingendtime[2].length < 2 ? '0' + meetingendtime[2] : meetingendtime[2]) + ' ' + meetingendtime[1] + ' ' + meetingendtime[5]);
                 //  $("#hdnToTime").text(meetingendtime[3])
             }
             catch (ex) { }
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
<body onunload="refreshPage()">
    <form id="form1" runat="server">
        <table style="height: 272px; width: 770px;">
            <tr>
                <td align="center" style="width: 365px;">
                    <table id="outtertable" runat="server" cellspacing="0" cellpadding="0" style="height: 272px; width: 334px; border-width: thin; border-style: solid; border-color: Black; table-layout: fixed">
                        <tr align="center">
                            <td>
                                <table id="innertable" cellspacing="0" cellpadding="0" style="height: 272px; width: 330px; border-color: White;">

                                    <tr align="center" style="height: 34px;">

                                        <td colspan="100%" style="vertical-align: top; height: 50px; padding-left: 5px; vertical-align: middle" align="left">
                                            <img id="cognizantlogo" src="~/Images/Cognizant_Blue_HD.png" runat="server" visible="true"
                                                align="left" alt="Logo" oncontextmenu="return false" style="height: 34px; width: 110px;" />

                                        </td>

                                    </tr>
                                    <tr align="left" valign="top">

                                        <td>
                                            <table cellspacing="0" cellpadding="0" style="width: 330px">
                                                <tr>
                                                    <td colspan="100%" style="text-align: center; height: 30px; vertical-align: middle; font-weight: bolder; font-size: larger; color: #004B7D; font-family: Arial">
                                                        <asp:Label ID="lblTempId" runat="server" Text="<%$ Resources:LocalizedText, lblTempId %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="text-align: left; vertical-align: middle; height: 24px; padding-left: 10px;">
                                                        <asp:Label ID="lblValidfor" runat="server" Style="font-size: 14px; font-weight: bolder"
                                                            Class="constantNamelabelbold" Text="<%$ Resources:LocalizedText, lblValidfrom %>"></asp:Label>&nbsp;
                                                    <label id="lblFrom" runat="server" style="font-size: 14px; font-weight: bolder" class="constantNamelabelbold"></label>

                                                        <asp:HiddenField ID="hdnFromTime" runat="server" />

                                                        <asp:Label ID="lblToText" runat="server" Style="font-size: 14px; font-weight: bolder"
                                                            Class="constantNamelabelbold" Text="<%$ Resources:LocalizedText, lblValidTo %>"></asp:Label>&nbsp;
                                                 
                                                <label id="lblTo" runat="server" style="font-size: 14px; font-weight: bolder" class="constantNamelabelbold"></label>
                                                        <asp:HiddenField ID="hdnToTime" runat="server" />


                                                    </td>
                                                </tr>

                                                <tr class="Rowstyle">
                                                    <td align="left" style="width: 80px; padding-left: 10px;">
                                                        <asp:Label ID="lblNamehead" runat="server" Font-Bold="True" Class="constantNamelabelbold"
                                                            Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>
                                                    </td>
                                                    <td align="center" style="font-family: Arial; width: 20px; font-size: 12px;">:
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 230px;">
                                                        <asp:Label ID="lblName" runat="server" Font-Bold="True" CssClass="constantNamelabelbold"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="Rowstyle">
                                                    <td align="left" style="padding-left: 10px;">
                                                        <asp:Label ID="Label1" runat="server" Class="constantlabel"
                                                            Text="<%$ Resources:LocalizedText, AssociateId %>"></asp:Label>
                                                    </td>
                                                    <td align="center" style="font-family: Arial; font-size: 12px;" class="style1">:
                                                    </td>
                                                    <td align="left" style="width: 90px;">
                                                        <asp:Label ID="lblAssociateID" runat="server" Class="constantlabel"></asp:Label>
                                                    </td>
                                                    <td rowspan="5" style="width: 130px; vertical-align: bottom; padding-right: 10px" align="right">
                                                        <asp:Image ID="imgAssociate" runat="server" ImageUrl="~/Images/DummyPhoto.png" Width="120px" Style="max-height: 126px"
                                                            align="right" oncontextmenu="return false" ImageAlign="Right" />
                                                    </td>
                                                </tr>
                                                <tr class="Rowstyle">
                                                    <td align="left" style="padding-left: 10px;">
                                                        <asp:Label ID="lblFacility" runat="server" Class="constantlabel"
                                                            Text="<%$ Resources:LocalizedText, Facility %>"></asp:Label>
                                                    </td>
                                                    <td align="center" style="font-family: Arial; font-size: 12px;" class="style1">:
                                                    </td>
                                                    <td align="left" class="Variables_TD">
                                                        <asp:Label ID="lblFacilityName" runat="server" Class="constantlabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="Rowstyle">
                                                    <td align="left"></td>
                                                </tr>
                                                <tr class="Rowstyle">
                                                    <td align="left"></td>
                                                </tr>
                                            </table>
                                        </td>

                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" valign="top" align="center" style="height: 20px">
                                            <img id="imgAssociateFlag" src="~/Images/IsAssociate.png" runat="server" visible="true"
                                                align="center" oncontextmenu="return false" alt="" style="width: 220px; vertical-align: bottom" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="center" axis="180" style="font-size: 4px; width: 25px;">|<br />
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
                <td align="center" style="width: 365px;">
                    <table id="Table2" runat="server" cellspacing="0" cellpadding="0" style="height: 272px; width: 334px; border-width: thin; border-style: solid; border-color: Black; table-layout: fixed">
                        <tr align="center">
                            <td>
                                <table id="Table3" cellspacing="0" cellpadding="0" style="height: 272px; width: 321px; border-color: White;">
                                    <tr align="left" valign="top">
                                        <td colspan="100%" style="height: 44px">&nbsp;
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td style="width: 80px; height: 22px; padding-left: 10px; vertical-align: middle">

                                            <asp:Label ID="lblEmpPass" Style="width: 100px" runat="server"
                                                Class="constantlabel2" Text="<%$ Resources:LocalizedText, lblEmpPass %>"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 20px; padding-top: 3px; font-family: Arial; font-size: 10px; vertical-align: middle">:
                                        </td>
                                        <td align="left" style="width: 220px; vertical-align: middle">
                                            <asp:Label ID="lblPassNumber" runat="server" Style="font-weight: bold" Class="constantlabel2"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" style="height: 50px; padding-left: 10px">

                                            <asp:Image ID="imgBarCode" ImageUrl="~/Barcode.aspx?Code=123456" runat="server" ImageAlign="Bottom"
                                                Width="185px" Height="50px" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="100%" style="height: 20px"></td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" style="height: 22px; padding-left: 10px">
                                            <asp:Label ID="lblEmercengyContact" Style="width: 100px" runat="server" Class="constantlabel2"
                                                Text="<%$ Resources:LocalizedText, EmercengyContact %>"></asp:Label>
                                            &nbsp;<asp:Label ID="lblEmercengyContactNumber" runat="server" Style="font-weight: bold"
                                                Class="constantlabel2"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" style="height: 22px; padding-left: 10px">
                                            <asp:Label ID="lblBloodGroup" Style="width: 100px" runat="server"
                                                Text="<%$ Resources:LocalizedText, BloodGroup %>" Class="constantlabel2"></asp:Label>
                                            &nbsp;<asp:Label ID="lblBloodGroup1" runat="server" Style="font-weight: bold" Class="constantlabel2"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" style="height: 20px">&nbsp;
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" align="left" style="padding-left: 10px">
                                            <asp:Label ID="lblCognizant" CssClass="IDFontBold" Font-Bold="true" Font-Size="7pt"
                                                Width="225px" runat="server" Text="<%$ Resources:LocalizedText, CompanyName %>"
                                                Style="text-align: left; vertical-align: top"></asp:Label><br />
                                            <asp:Label ID="lblFacilityAddress" CssClass="IDFontNormal" Font-Size="6" runat="server"
                                                Text="<%$ Resources:LocalizedText, Address %>"></asp:Label><br />
                                            <asp:Label ID="lblArea" CssClass="IDFontNormal" Font-Size="6" runat="server" Text="<%$ Resources:LocalizedText, lblArea %>"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td colspan="100%" valign="top" align="center" style="height: 20px">
                                            <img id="imgAssociateFlag2" src="~/Images/IsAssociate.png" runat="server" visible="true"
                                                align="middle" alt="" oncontextmenu="return false" style="width: 220px; vertical-align: bottom" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="printOption" class="noPrint" style="border-bottom-color: White;">
            <a href="javascript:imgPrint_onclick()">
                <asp:Label ID="print" runat="server" Text="<%$ Resources:LocalizedText, Print %>"></asp:Label></a><br />
            <br />
        </div>
        <asp:HiddenField id="hdnFileContent" runat="server" />
    </form>
    <script language="javascript" type="text/javascript">

        function imgPrint_onclick() {
            window.print();
            window.close();
        }

    </script>
</body>
</html>
