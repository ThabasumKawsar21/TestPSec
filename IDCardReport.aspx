<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="IDCardReport.aspx.cs" Inherits="VMSDev.IDCardReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .GridPadding
        {
            padding-left: 5px;
        }
        .modalBackground
        {
            background-color: #878776;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }
        .ModalWindow
        {
            border: 1px solid #c0c0c0;
            background: #f0f0f0;
            padding: 0px 0px 0px 0px;
            width: 500px;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <table width="100%" class="border">
        <tr valign="bottom">
            <td align="left" class="table_header_bg" colspan="100%">
                &nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header" Text="ID Card Report"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="tdBold" width="15%">
                <asp:Label ID="lblFromDate" CssClass="field_text" runat="server" Text="From Date"
                    Font-Size="12px"></asp:Label>
            </td>
            <td align="left" width="15%" style="white-space: nowrap">
                &nbsp;
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtField" AutoPostBack="false"></asp:TextBox>
                <cc1:CalendarExtender ID="FromDateCalendar" runat="server" TargetControlID="txtFromDate"
                    PopupButtonID="imgFromDate" Format="dd/MM/yyyy" PopupPosition="BottomRight">
                </cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtenderFromDate" ClearMaskOnLostFocus="false"
                    InputDirection="RightToLeft" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999"
                    MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    MaskType="Date" ErrorTooltipEnabled="True" UserDateFormat="None" UserTimeFormat="None" />
                <asp:ImageButton ID="imgFromDate" Style="cursor: hand" ImageUrl="~/Images/calender-icon.png"
                    runat="server" Width="15px" ImageAlign="Middle" ToolTip="To View the calender control" />
            </td>
            <td align="right" width="15%">
                <asp:Label ID="lblToDate" CssClass="field_text" runat="server" Font-Size="12px" Text="To Date"></asp:Label>
            </td>
            <td width="15%" style="white-space: nowrap">
                &nbsp;
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
            <td width="10%">
            </td>
            <td align="right" class="tdBold" width="10%">
                <asp:Button ID="btnSearch" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                    ForeColor="White" Height="20px" Text="Get ID Card Report" CausesValidation="False"
                    OnClick="Btnsearch_Click" Width="107px" />
            </td>
            <td align="left" style="margin-left: 40px" width="10%">
                <asp:Button ID="btnClear" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                    ForeColor="White" Height="20px" OnClick="BtnClear_Click" Text="Clear" CausesValidation="False"
                    UseSubmitBehavior="False" />
            </td>
            <td width="10%">
            </td>
        </tr>
        <tr>
            <td colspan="100%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="100%">
                <table width="100%">
                    <tr>
                        <td width="5%">
                        </td>
                        <td width="80%" align="center">
                            <asp:Label ID="lblSuccessMessage" runat="server" CssClass="Table_header" Visible="false"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="imbExcel" runat="server" ImageUrl="~/Images/excel_icon.GIF"
                                AlternateText="Export to Excel" Width="16px" OnClick="Imbexcel_Click" Visible="False" />
                            &nbsp;
                        </td>
                        <td width="5%">
                        </td>
                    </tr>
               
        <tr>
        <td></td>
            <td colspan="2" style="text-align: center;">
                <center>
                    <asp:GridView ID="grdOuter" runat="server" AutoGenerateColumns="false" DataKeyNames="AssociateID"
                        ShowHeader="true" Width="100%" OnRowCommand="GrdOuter_RowCommand">
                        <RowStyle CssClass="field_txt" HorizontalAlign="Center" Height="25px" />
                        <HeaderStyle BackColor="#C6D4BB" CssClass="field_txt" HorizontalAlign="Center" Height="30px" />
                        <Columns>
                            <%--<asp:BoundField DataField="ApplicantID" HeaderStyle-HorizontalAlign="Center" HeaderText="Applicant ID">
                                <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="AssociateID" HeaderStyle-HorizontalAlign="Center" HeaderText="Associate ID">
                                <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AssociateName" HeaderStyle-HorizontalAlign="Center" HeaderText="Associate Name">
                                <ItemStyle HorizontalAlign="Left" Wrap="True" CssClass="GridPadding" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BloodGroup" HeaderStyle-HorizontalAlign="Center" HeaderText="Blood Group">
                                <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmergencyContact" HeaderStyle-HorizontalAlign="Center"
                                HeaderText="Emergency Contact" ItemStyle-CssClass="GridPadding">
                                <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        <%--    <asp:BoundField DataField="ImageAvailable" HeaderStyle-HorizontalAlign="Center" HeaderText="Image Captured">
                                <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CardPrinted" HeaderStyle-HorizontalAlign="Center" HeaderText="Card Printed">
                                <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                           <%-- <asp:BoundField DataField="ImageCapturedDate" HeaderStyle-HorizontalAlign="Center"
                                HeaderText="Image Captured Date" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="IDCard Last Generated Date">
                                <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkGeneratedDate" runat="server" Text='<%#Eval("IDCardLastGeneratedDate","{0:dd-MMM-yyyy HH:mm }")%>'
                                        CommandArgument='<%#Eval("AssociateID") %>' CommandName="ShowHistory"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ScriptManager runat="server">
                    </asp:ScriptManager>
                    <cc1:ModalPopupExtender ID="modalGeneratedDates" CancelControlID="Clear" BackgroundCssClass="modalBackground"
                        DropShadow="true" X="-1" Y="-1" PopupControlID="pnlGeneratedDates" TargetControlID="hfGvPanel"
                        runat="server">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="pnlGeneratedDates" runat="server" Width="300" Height="175" CssClass="ModalWindow"
                        Style="display: none">
                        <asp:ImageButton ID="Clear" Height="12" Width="12" runat="server" ImageUrl="~/Images/Clear.png"
                            Style="float: right; margin-top: 5px; margin-right: 5px" />
                        <asp:HiddenField ID="hdnApplicantId" runat="server" />
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdNoImageReport" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="GridText" HorizontalAlign="Center" OnPageIndexChanging="GrdNoImageReport_PageIndexChanging"
                                    PagerStyle-HorizontalAlign="Center" PageSize="5" Width="80%" Style="margin-top: 40px"
                                    PageIndex="0">
                                    <RowStyle CssClass="field_txt" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#C6D4BB" CssClass="field_txt" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="IDCardGeneratedDate" HeaderStyle-HorizontalAlign="Center"
                                            HeaderText="ID Card Generated Date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IssuedBy" HeaderStyle-HorizontalAlign="Center" HeaderText="Issued By">
                                            <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Prev" />
                                    <AlternatingRowStyle BackColor="#F4FCED" CssClass="field_txt" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:HiddenField ID="hfGvPanel" runat="server" />
                    <asp:HiddenField ID="hfGvStatus" runat="server" />
                </center>
                <br />
            </td>
            <td></td>
        </tr>
         </table>
            </td>
        </tr>
    </table>
</asp:Content>
