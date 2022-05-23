<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    CodeBehind="UsageReport.aspx.cs" Inherits="VMSDev.UsageReport" %>

<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <style type="text/css">
        .style3
        {
            width: 212px;
        }
        .style4
        {
            width: 178px;
        }
    </style>
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function pageLoad() {

            GetOffsetTime();

        }


        function GetOffsetTime() {

            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            UsageReport.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            UsageReport.AssignCurrentDateTime(today);
        }

        function fnCheck() {
            var value = $("#<%=drpCountry.ClientID %> option:selected").val();
            if (value == "") {
                alert("Please select country");
                return false;
            }
        }
        function GetOffset() {
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            UsageReport.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            UsageReport.AssignCurrentDateTime(today);

            var r = document.getElementById('<%=btnUsageReportHidden.ClientID %>');
            r.click();

        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="VMSContentPlaceHolder" runat="server">
    <asp:scriptmanager id="ScriptManager1" runat="server">
    </asp:scriptmanager>
    <asp:updatepanel id="filterpanel" runat="server">
        <triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            
        </triggers>
        <contenttemplate>
            <table class="tblHeadStyle" width="100%">
                 <tr>
            <td colspan="5" align="left">
                <asp:label id="lblVisitorInfo" runat="server" cssclass="lblHeada" text="Usage Report">
                </asp:label>
            </td>           
        </tr>
                <tr>
                    <td align="right" class="tdBold" >
                        <asp:Label ID="lblCountry" runat="server" CssClass="lblField" Text="Country"></asp:Label>
                    </td>
                    <td align="left" class="style3">
                        <asp:DropDownList ID="drpCountry" runat="server" AutoPostBack="true" 
                            OnSelectedIndexChanged="DrpCountry_SelectedIndexChanged" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="tdBold" >
                        <asp:Label ID="lblCity" runat="server" CssClass="lblField" Text="City"></asp:Label>
                    </td>
                    <td align="left" class="style3">
                        <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" 
                            OnSelectedIndexChanged="DdlCity_SelectedIndexChanged" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <td align="right" class="tdBold">
                        <asp:Label ID="lblFacility0" runat="server" CssClass="lblField" Text="Facility"></asp:Label>
                    </td>
                    <td align="left" class="style4">
                        <asp:DropDownList ID="ddlFacility" runat="server" Width="235px">
                        </asp:DropDownList>
                    </td>
                      <td align="left" class="tdBold">
                        <asp:Label ID="lblDepartment" runat="server" CssClass="lblField" Text="Department"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="235px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdBold">
                        <asp:Label ID="lblFromDate" runat="server" CssClass="lblField" Text="From Date"></asp:Label>
                    </td>
                    <td align="left" class="style3">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtField" AutoPostBack="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                            PopupButtonID="imgFromDate" Format="dd/MM/yyyy" PopupPosition="BottomRight">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtenderFromDate" ClearMaskOnLostFocus="false"
                            InputDirection="RightToLeft" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999"
                            MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                            MaskType="Date" ErrorTooltipEnabled="True" UserDateFormat="None" UserTimeFormat="None" />
                        <asp:ImageButton ID="imgFromDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                            runat="server" Width="15px" ImageAlign="Middle" ToolTip="To View the calender control"/>
                        <%-- <cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtFromDate" ControlExtender="MaskedEditExtenderFromDate" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid date!" EmptyValueMessage="The date is not entered">

 

        </cc1:MaskedEditValidator>   --%>
                    </td>
                    <td align="right" class="tdBold">
                        <asp:Label ID="lblToDate" runat="server" CssClass="lblField" Text="To Date"></asp:Label>
                    </td>
                    <td colspan="2" align="left" style="margin-left: 40px" class="style4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txtField" AutoPostBack="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="ToDateCalendar" runat="server" TargetControlID="txtToDate"
                            PopupButtonID="imgToDate" Format="dd/MM/yyyy" PopupPosition="BottomRight">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtenderToDate" InputDirection="RightToLeft"
                            ClearMaskOnLostFocus="false" runat="server" TargetControlID="txtToDate" Mask="99/99/9999"
                            MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                            MaskType="Date" ErrorTooltipEnabled="True" />
                        <asp:ImageButton ID="imgToDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                            runat="server" Width="15px" ImageAlign="Middle" ToolTip="To View the calender control" />
                    </td>

                    <%--<cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtFromDate" ControlExtender="MaskedEditExtenderToDate" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid date!" EmptyValueMessage="The date is not entered"/>--%>
                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" OnClientClick="return fnCheck()" OnClick="BtnSearch_Click" Text="Search" CssClass="cssButton" />
                        <%--<asp:ImageButton ImageUrl="~/Images/excel_icon.GIF" ID="btnExport" runat="server"
                            Text="Export" OnClick="BtnExport_Click" Width="16px" />--%>
                    </td>
                </tr>
            </table>
        </contenttemplate>
    </asp:updatepanel>
    <hr width="100%" height="1" />
    <br />
    <asp:panel id="PanelEqipment" runat="server" scrollbars="Auto" margin="0px" height="150px">
        <table align="center">
            <tr>
                <td>
                    <table border="0" id="errortbl" cellpadding="0" cellspacing="0" runat="server" style="border-right: black 1pt solid;
                        border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid;
                        width: 100%;">
                        <tr>
                            <td colspan="6" align="center" style="background-color: #edf6fd; width: 100%">
                                &nbsp;
                                <asp:label id="lblMessage" runat="server" cssclass="field_txt" forecolor="Red"></asp:label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <asp:gridview id="grdResult" runat="server" cellpadding="4" forecolor="#333333" autogeneratecolumns="False"
                        font-names="Arial" font-size="X-Small" headerstyle-wrap="True" pagesize="25"
                        allowpaging="False" cssclass="grdField" emptydatatext="No records found" emptydatarowstyle-backcolor="white"
                        emptydatarowstyle-forecolor="Red" emptydatarowstyle-font-size="13px" emptydatarowstyle-borderstyle="None"
                        emptydatarowstyle-font-bold="true" gridlines="Vertical" width="700px">
                        <rowstyle backcolor="#EFF3FB" cssclass="grdField" />
                        <columns>
                            <asp:TemplateField HeaderText="Sl.no">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <ControlStyle Height="10px" Width="10px" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="City" DataField="City" />
                            <asp:BoundField HeaderText="Facility" DataField="Facility" />
                            <asp:BoundField HeaderText="Count" DataField="Count" />
                            <%--  // changed by Priti 30 April for VMS Observations--%>
                            <%-- <asp:BoundField HeaderText="Visitor Reference No" DataField="VisitorReferenceNo" /> --%>
                            <%-- <asp:BoundField HeaderText="Badge Status" DataField="BadgeStatus"  />--%>
                            <%--  // changed by Priti 28th June for CR3 Issues--%>
                        </columns>
                        <footerstyle backcolor="#507CD1" font-bold="True" forecolor="White" />
                        <pagerstyle backcolor="#2461BF" forecolor="White" horizontalalign="Center" />
                        <selectedrowstyle backcolor="#99CCFF" font-bold="True" forecolor="#333333" />
                        <headerstyle backcolor="#507CD1" font-bold="True" forecolor="White" />
                        <editrowstyle backcolor="#2461BF" />
                        <alternatingrowstyle backcolor="White" />
                    </asp:gridview>
                </td>
                <td valign="top" align="right">
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <br />
    </asp:panel>
    <hr />
    <table>
        <tr>
            <td align="left">
                <asp:chart id="Chart2" runat="server" height="296px" width="412px" backcolor="#d6ecfe"
                    backgradientstyle="TopBottom" borderwidth="2px" bordercolor="#1A3B69">
                    <series>
                        <asp:Series Name="statuscount" XValueType="String" MarkerSize="9" ChartType="Pie"
                            IsValueShownAsLabel="true" >
                        </asp:Series>
                    </series>
                    <titles>
                        <asp:Title Text="Visit Status" ForeColor="Snow" Docking="Top" BackColor="#507CD1"
                            Font="Arial, 10pt, style=Bold" BorderColor="Khaki">
                        </asp:Title>
                    </titles>
                    <chartareas>
                        <asp:ChartArea Name="ChartArea1" ShadowColor="Gray" Area3DStyle-Enable3D="true" Area3DStyle-Rotation="10" BackColor="#d6ecfe" BackGradientStyle="TopBottom">
                            <AxisY LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8" LineWidth="0">
                                <LabelStyle ForeColor="Black" />
                                <MajorGrid LineColor="Silver" />
                            </AxisY>
                            <AxisX LineColor="Silver" LabelAutoFitMaxFontSize="8" Interval="1">
                                <LabelStyle />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisX>
                        </asp:ChartArea>
                    </chartareas>
                    <legends>
                        <asp:Legend Name="DefaultLegend" Enabled="True" Docking="Bottom" />
                    </legends>
                </asp:chart>
            </td>
            <td align="left">
                <asp:chart id="Chart1" runat="server" height="296px" width="412px" backcolor="211, 223, 240"
                    borderlinedashstyle="Solid" backgradientstyle="TopBottom" borderwidth="2px" bordercolor="#1A3B69"
                    title="Requests Count">
                    <series>
                      
                        <asp:Series Name="Security" IsValueShownAsLabel="true" >
                        </asp:Series>
                          <asp:Series Name="Host" XValueType="String" MarkerSize="9" IsValueShownAsLabel="true" >
                        </asp:Series>
                    </series>
                    <titles>
                        <asp:Title Text="Host/Security Request count" ForeColor="Snow" Docking="Top" BackColor="#507CD1"
                            Font="Arial, 10pt, style=Bold" BorderColor="Khaki">
                        </asp:Title>
                    </titles>
                    <chartareas>
                        <asp:ChartArea Name="ChartArea1" ShadowColor="Gray" Area3DStyle-Enable3D="true" Area3DStyle-Rotation="10">
                            <AxisY LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8" LineWidth="0">
                                <LabelStyle ForeColor="Black" />
                                <MajorGrid LineColor="Silver" />
                            </AxisY>
                            <AxisX LineColor="Silver" LabelAutoFitMaxFontSize="8" Interval="1">
                                <LabelStyle />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisX>
                        </asp:ChartArea>
                    </chartareas>
                    <legends>
                        <asp:Legend Name="DefaultLegend" Enabled="True" Docking="Bottom" />
                    </legends>
                </asp:chart>
            </td>
        </tr>
    </table>
    <asp:button id="btnUsageReportHidden" runat="server" style="display: none" text="Button"
        onclick="BtnUsageReportHidden_Click" />
</asp:content>
