<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateBadge.aspx.cs"
    Inherits="VMSDev.GenerateBadge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--<script language="javascript" type="text/javascript">
    function onload()
        
    {
    
    var lblFirstName = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorGeneralInformationControl_txtFirstName");
    var lblLastName = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorGeneralInformationControl_txtLastName");
     

    var name = document.getElementsByTagName("namefont");

    if (((lblFirstName).value + ", " + (lblLastName).value).length > 20) {
        document.getElementsByTagName("namefont").style = " font-size:9px ";
    }

    else {
        document.getElementsByTagName("namefont").style = " font-size:20px ";
    }
    }
</script>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Badge</title>
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
            width: 112px;
            height: 2px;
        }
        .style21
        {
            vertical-align: top;
            width: 53px;
            line-height: 12px;
        }
        .style22
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 10px;
            width: 53px;
            line-height: 12px;
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
            width: 53px;
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
            width: 53px;
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
            width: 112px;
            height: 24px;
        }
        .style38
        {
            vertical-align: top;
            width: 53px;
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
    </style>
</head>
<%--updated for CR IRVMS22062010CR07 done by Priti
--%>
<body onload="Page_load()" onunload="refreshPage()" style="background-color: White;
    border-bottom-color: white">
    <form id="form1" runat="server">
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
                                                        align="left" alt="Logo" style="height: 34px; width: 110px;" />
                                                </td>
                                                <td class="style19">
                                                </td>
                                                <td align="right" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style20">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedText, VisitorsPass %>" class="Smallconstants">  </asp:Label>
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style47">
                                                    <asp:Label ID="lblbadge" runat="server" class="Smallconstants"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style59">
                                                </td>
                                                <td align="right" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style37">
                                                    <asp:Label ID="Label23" runat="server" Class="Smallconstants" Width="70px" Height="16px" 
                                                    Text="<%$ Resources:LocalizedText, BadgeFacility %>"></asp:Label>
                                                </td>
                                                <td align="left" style="vertical-align: top; font-family: Interstate-Bold; font-size: 8px;"
                                                    valign="top" class="style59">
                                                    <asp:Label ID="lblCity" runat="server" Class="Smallconstants" Height="16px"></asp:Label><asp:Label
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
                                                    <asp:Label ID="lblNamehead" runat="server" Font-Bold="True" Class="constantNamelabelbold" Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>
                                                </td>
                                                <td class="style34" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="style35" colspan="2">
                                                    <asp:Label ID="lblName" runat="server" Font-Bold="True" Class="constantNamelabelbold"> </asp:Label>
                                                </td>
                                                <%-- <td rowspan="8" style="width: 114px; vertical-align: top">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="Images/DummyPhoto.png" Width="103px"
                                                        align="right" Height="98px" />
                                                </td>--%>
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
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/DummyPhoto.png" Width="125px"
                                                        align="right" Height="131px" ImageAlign="Right" />
                                                </td>
                                            </tr>
                                            <%-- <tr class="Rowstyle">
                                                <td align="left" class="style22">
                                                    <asp:Label ID="Label24" runat="server" Class="constantlabelbold">Escort</asp:Label>
                                                </td>
                                                <td class="style23" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="Variables_TD">
                                                    <asp:Label ID="lblEscort1" runat="server" Class="constantlabelbold"> </asp:Label>
                                                </td>
                                            </tr>--%>
                                            <tr class="Rowstyle2">
                                                <td align="left" class="style21">
                                                    <asp:Label ID="Label7" runat="server" Class="Smallconstants" Text="<%$ Resources:LocalizedText, From %>"></asp:Label>
                                                </td>
                                                <td class="style23" align="center">
                                                    :
                                                </td>
                                                <td align="left" class="Variables_TD">
                                                    <asp:Label ID="lblDateTimeIn" runat="server" Class="Smallconstants"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblTimIn" runat="server" Class="Smallconstants"></asp:Label>
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
                                                    <asp:Label ID="lblToDate" runat="server" Font-Bold="True" Class="Smallconstants"></asp:Label>
                                                    &nbsp;<asp:Label ID="lblTimOut" runat="server" Font-Bold="True" Class="Smallconstants"></asp:Label>
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
                                                <td>
                                                    <p style="font-size: 9px; font-family: Interstate-Regular; width: 284px; margin-top: 0px;"
                                                    runat="server" Text="<%$ Resources:LocalizedText, lblBadgeNote %>" >
                                                        <%--Note&nbsp;:&nbsp;&nbsp;Please return this badge at the security before leaving this
                                                        premises--%></p>
                                                </td>
                                                <td align="right">
                                                    <img id="Img3" src="~/Images/No_Smoke_Cam.jpg" runat="server" visible="true" alt="Logo"
                                                        style="height: 16px; width: 28px; vertical-align: bottom" />
                                                </td>
                                            </tr>
                                        </table>
                                        <%--  <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="321px" style="line-height: 6px; height: 6px">
                                                </td>
                                            </tr>
                                        </table>--%>
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
                                                            alt="Logo" style="height: 34px; width: 110px;" />
                                                    </td>
                                                    <td class="style19">
                                                    </td>
                                                    <td align="right" style="vertical-align: middle; font-family: Interstate-Bold; font-size: 8px;"
                                                        valign="middle" class="style63">
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:LocalizedText, VisitorsPass %>" Style="height: 34px;
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
                                                        <img id="Img5" src="~/Images/No_Smoke_Cam.jpg" runat="server" visible="true" alt="Logo"
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
                                                        <asp:Label ID="Label17" runat="server" Class="constantlabel" Text="<%$ Resources:LocalizedText, EquipmentPermitted %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%-- </table>--%>
                                                <%-- <hr size="1" style="z-index: 1; left: 13px; top: 229px; position: absolute; height: 1px;
                                                width: 668px" />--%>
                                                <%-- <table  cellspacing="0" cellpadding="0" >--%>
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
                                                            Note&nbsp;:</p>
                                                    </td>
                                                    <td align="left" valign="top" style="white-space:nowrap">
                                                        <p style="font-size: 9px; font-family: Interstate-Regular;">
                                                            In case of any emergency please contact the security at -&nbsp;
                                                            <asp:Label ID="lblsecurityno" runat="server" Class="Notestyle"></asp:Label></br>
                                                            Kindly follow the instructions provided on the reverse</p>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="321px" style="line-height: 6px; height: 6px">
                                                    </td>
                                                </tr>
                                            </table>--%>
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
        <a href="javascript:imgPrint_onclick()">Print</a><br />
        <br />
    </div>
    <!--<input type= value="Print" onclick="window.print();"/>-->
    <script language="javascript" type="text/javascript">

        function imgPrint_onclick() {          
            window.print();
            window.close();
        }
        ///updated for CR IRVMS22062010CR07  starts here done by Priti
        /// getting values from VMSEnterInformationBySecurity.aspx
        function refreshPage() {
            window.opener.document.getElementById("VMSContentPlaceHolder_btnHidden").click();
            window.close();
        }
        function Page_load() {
            alert("Visitor's Badge is successfully generated.");
            var lblFirstName = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorGeneralInformationControl_txtFirstName");
            var lblLastName = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorGeneralInformationControl_txtLastName");
            var lblOrganisation = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorGeneralInformationControl_txtCompany");
            var lblPurpose = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_ddlPurpose");
            var lblPurpose1 = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtPurpose");
            //            var lblHostName = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtHost");
            //Begin Changes for VMS CR 0013
            //            if (lblHostName.length > 15) {
            //                alert("yes");
            //            }
            //            else {
            //                alert("no" + lblHostName.length.toString());
            //            }
            //End of Changes for VMS CR 0013
            // var lblHostContactNo = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtHostContactNo");
            var lblDateIn = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtFromDate");
            var lblTimeIn = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtFromTime");
            var lblDateOut = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtToDate");
            var lblTimeOut = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtToTime");
            var lblEscort = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_txtEscort");
            //            var lblEscortNo = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_myHiddenVar");

            var lblFacility = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_ddlFacility");
            var lblBatchNo = window.opener.document.getElementById("VMSContentPlaceHolder_BatchNo");



            var lblCityname = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_ddlCity");
            var lblFacilityname = window.opener.document.getElementById("VMSContentPlaceHolder_VisitorLocationInformationControl_ddlFacility");
            //var btnHidden=
            var LaptopCount = 0;
            var CameraCount = 0;
            var DataStoragedeviceCount = 0;
            var USBHarddiskcount = 0;
            var Ipodcount = 0;
            var OthersCount = 0;




            if (((lblFirstName).value + ", " + (lblLastName).value).length > 20) {
                document.getElementsByTagName("namefont").style = " font-size:9px ";
            }

            else {
                document.getElementsByTagName("namefont").style = " font-size:20px ";
            }

            document.getElementById("lblName").innerText = ((lblFirstName).value + ", " + (lblLastName).value).toUpperCase();
            document.getElementById("lblOrganisation").innerText = (lblOrganisation).value;
            if (lblPurpose.options[lblPurpose.selectedIndex].value != "Others") {
                document.getElementById("lblPurpose").innerText = lblPurpose.options[lblPurpose.selectedIndex].value;
                //document.getElementById("lblPurpose1").innerText= lblPurpose.options[lblPurpose.selectedIndex].value;
            }
            else {
                document.getElementById("lblPurpose").innerText = (lblPurpose1).value;
                // document.getElementById("lblPurpose1").innerText= (lblPurpose1).value;
            }

            //            document.getElementById("lblHostName").innerText = (lblHostName).value;

            //            var re = /\$|,|@|#|~|`|\%|\*|\^|\&|\(|\)|\+|\=|\[|\-|\_|\]|\[|\}|\{|\;|\:|\'|\"|\<|\>|\?|\||\\|\!|\$|\./g;
            var re = /^([0-9]+)$/;

            // remove special characters like "$" and "," etc...


            //            var contno = (lblHostContactNo).value.replace(/[^0-9]/g, '');

            //            document.getElementById("lblHostContactNo").innerText = contno.toString();

            //            document.getElementById("lblHostContactNo").innerText.replace(re, '');



            var dateBits = (lblDateIn).value.split('/');
            var Indate;
            var Outdate;
            Indate = dateBits[1].toString() + '-' + dateBits[0].toString() + '-' + dateBits[2];

            var months = new Array(13);
            months[1] = "Jan";
            months[2] = "Feb";
            months[3] = "Mar";
            months[4] = "Apr";
            months[5] = "May";
            months[6] = "Jun";
            months[7] = "Jul";
            months[8] = "Aug";
            months[9] = "Sep";
            months[10] = "Oct";
            months[11] = "Nov";
            months[12] = "Dec";


            Indate = dateBits[0].toString() + '-' + (months[parseInt(dateBits[1].toString(), 10)]).toString() + '-' + dateBits[2];
            //            document.getElementById("lblDateTimeIn").innerText = (lblDateIn).value + " " + (lblTimeIn).value;
            document.getElementById("lblDateTimeIn").innerText = Indate.toString();

            dateBits = (lblDateOut).value.split('/');
            Outdate = dateBits[1].toString() + '-' + dateBits[0].toString() + '-' + dateBits[2];
            Outdate = dateBits[0].toString() + '-' + (months[parseInt(dateBits[1].toString(), 10)]).toString() + '-' + dateBits[2];
            //            document.getElementById("lblDateTimeOut").innerText = (lblDateOut).value + " " + (lblTimeOut).value;
            document.getElementById("lblToDate").innerText = Outdate.toString();

            document.getElementById("lblTimIn").innerText = (lblTimeIn).value + " Hrs";
            document.getElementById("lblTimOut").innerText = (lblTimeOut).value + " Hrs";
            //            document.getElementById("lblEscort").innerText = (lblEscort).value;
            //            document.getElementById("lblFacility").innerText = lblFacility.options[lblFacility.selectedIndex].value;
            //            document.getElementById("lblFacility1").innerText = lblFacility.options[lblFacility.selectedIndex].value;
            //alert((lblBatchNo).value);
            document.getElementById("lblbadge").innerText = (lblBatchNo).value;

            var euipdiv1 = document.getElementById("equipdiv");

            // 
            if (euipdiv1 != null) {
              
                document.getElementById("lblbadge2").innerText = (lblBatchNo).value;
                //document.getElementById("lblEscort1").innerText = (lblEscort).value;
                //                var lblEscortNo = document.getElementById("lblEscortcontact");
                //                var Escortcontno = (lblEscortNo).value.replace(/[^0-9]/g, '');
                //                document.getElementById("lblEscortcontact").innerText = Escortcontno.toString();

                document.getElementById("lblCity").innerText = lblCityname.options[lblCityname.selectedIndex].value.toString() + '-';
                document.getElementById("lblFacility").innerText = lblFacilityname.options[lblFacilityname.selectedIndex].value.toString();

            }
            //            document.getElementById("lblBatchNo1").innerText = (lblBatchNo).value;
            //            for (var i = 2; i <= 6; i++) {

            //                var lblEquipment = window.opener.document.getElementById("VMSContentPlaceHolder_EquipmentPermittedControl_GridView1_ctl0" + i + "_ddlEquipmentType");
            //                //alert(((lblEquipment).value));
            //                // changed by Priti 28th June for CR3 Issues
            //                if (lblEquipment != null) {
            //                    if (((lblEquipment).value) == 23) {
            //                        CameraCount++;
            //                        if (document.getElementById("lblEquipment").innerText == "") {
            //                            document.getElementById("lblEquipment").innerText = "Camera" + CameraCount;
            //                        }
            //                        else {
            //                            document.getElementById("lblEquipment").innerText = document.getElementById("lblEquipment").innerText + ", " + "Camera" + CameraCount;
            //                        }
            //                    }
            //                    if (((lblEquipment).value) == 12) {
            //                        LaptopCount++;
            //                        if (document.getElementById("lblEquipment").innerText == "") {
            //                            document.getElementById("lblEquipment").innerText = "Laptop" + LaptopCount;
            //                        }
            //                        else {
            //                            document.getElementById("lblEquipment").innerText = document.getElementById("lblEquipment").innerText + ", " + "Laptop" + LaptopCount;
            //                        }
            //                    }
            //                    if (((lblEquipment).value) == 13) {
            //                        DataStoragedeviceCount++;
            //                        if (document.getElementById("lblEquipment").innerText == "") {
            //                            document.getElementById("lblEquipment").innerText = "Data Storage Device" + DataStoragedeviceCount;
            //                        }
            //                        else {
            //                            document.getElementById("lblEquipment").innerText = document.getElementById("lblEquipment").innerText + ", " + "Data Storage Device" + DataStoragedeviceCount;

            //                        }
            //                    }
            //                    if (((lblEquipment).value) == 14) {
            //                        USBHarddiskcount++;
            //                        if (document.getElementById("lblEquipment").innerText == "") {
            //                            document.getElementById("lblEquipment").innerText = "USB Hard Disk" + USBHarddiskcount;
            //                        }
            //                        else {
            //                            document.getElementById("lblEquipment").innerText = document.getElementById("lblEquipment").innerText + ", " + "USB Hard Disk" + USBHarddiskcount;

            //                        }
            //                    }
            //                    if (((lblEquipment).value) == 24) {
            //                        Ipodcount++;
            //                        if (document.getElementById("lblEquipment").innerText == "") {
            //                            document.getElementById("lblEquipment").innerText = "I Pod" + Ipodcount;
            //                        }
            //                        else {
            //                            document.getElementById("lblEquipment").innerText = document.getElementById("lblEquipment").innerText + ", " + "I Pod" + Ipodcount;

            //                        }
            //                    }
            //                    if (((lblEquipment).value) == 25) {
            //                        OthersCount++;
            //                        if (document.getElementById("lblEquipment").innerText == "") {
            //                            document.getElementById("lblEquipment").innerText = "Others" + OthersCount;
            //                        }
            //                        else {
            //                            document.getElementById("lblEquipment").innerText = document.getElementById("lblEquipment").innerText + ", " + "Others" + OthersCount;

            //                        }
            //                    }


            //                }
            //            }
        }



        function Img1_onclick() {

        }

    </script>
    </form>
</body>
</html>
