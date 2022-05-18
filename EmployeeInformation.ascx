<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeInformation.ascx.cs"
    Inherits="VMSDev.UserControls.EmployeeInformation" %>
<style type="text/css">
    .border
    {
        border: 1px solid #808080;
        border-collapse: separate;
    }
    .border
    {
        border: 1px solid #808080;
        border-collapse: separate;
    }
    .Table_header
    {
        font-weight: bold;
        font-size: 12px;
        color: #5d8427;
        font-style: normal;
        font-family: Arial,Verdana, Helvetica, sans-serif;
        text-decoration: none;
        text-align: left;
    }
    .field_column_bg
    {
        background-color: #ECF7FD;
    }
    .field_text
    {
        font-family: Arial;
        font-size: 11px;
        font-style: normal;
        font-weight: normal;
        font-variant: normal;
        color: #00000;
        text-decoration: none;
    }
</style>
<asp:Panel ID="panelEmp" runat="server" Width="100%" CssClass="border">
    <table id="tblEmp" cellpadding="0" cellspacing="0" width="100%" style="height: 304px">
        <tr>
            <td align="left" style="height: 15px; padding-left: 10px" colspan="3">
                &nbsp;<asp:Label ID="lblEmployeeHeader" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblEmployeeHeader %>"></asp:Label>
            </td>
            <%-- <td align="left" style="height: 15px">
                            </td>
                            <td align="left" style="height: 15px">
                            </td>--%>
            <td align="left" style="height: 15px; padding-left: 10px" valign="middle" colspan="2">
                <asp:Label ID="lblManagerHeader" runat="server" CssClass="Table_header" Text="<%$ Resources:LocalizedText, lblManagerHeader %>"></asp:Label>
            </td>
            <%--    <td style="width: 20%">
                            </td>--%>
        </tr>
        <tr>
            <td align="right" style="height: 15px; width: 10%;" class="field_column_bg">
                <asp:Label ID="lblEmployeeID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, lblEmployeeID %>"></asp:Label>:
            </td>
            <td align="left" style="height: 15px; width: 25%;">
                &nbsp;<asp:Label ID="lblEmpID" runat="server" CssClass="field_text"></asp:Label>
            </td>
            <td align="left" rowspan="8" style="width: 20%; min-width:200px ">
                <asp:Image ID="ImgAssociate" runat="server" ImageUrl="~/Images/char.jpeg" Height="150px" style="max-width:200px"
                    oncontextmenu="return false" ImageAlign="Top" />
            </td>
            <td align="right" style="height: 15px; width: 10%;" valign="middle" class="field_column_bg">
                <asp:Label ID="lblManagerID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, lblEmployeeID %>"></asp:Label>:
            </td>
            <td style="width: 25%" align="left">
                &nbsp;<asp:Label ID="lblMgrID" runat="server" CssClass="field_text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                <asp:Label ID="lblEmployeeName" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>:
            </td>
            <td align="left" style="height: 15px">
                &nbsp;<asp:Label ID="lblEmpName" runat="server" CssClass="field_text"></asp:Label>
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                <asp:Label ID="lblManagerName" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Name %>"></asp:Label>:
            </td>
            <td style="width: 20%" align="left">
                &nbsp;<asp:Label ID="lblMgrName" runat="server" CssClass="field_text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                <asp:Label ID="lblEmailID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, EmailID %>"></asp:Label>:
            </td>
            <td align="left" style="height: 15px">
                &nbsp;<asp:Label ID="lblEmpEmailID" runat="server" CssClass="field_text"></asp:Label>
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                <asp:Label ID="lblManagerEmailID" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, EmailID %>"></asp:Label>:
            </td>
            <td style="width: 20%" align="left">
                &nbsp;<asp:Label ID="lblMgrEmailID" runat="server" CssClass="field_text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 19px">
                <asp:Label ID="lblEmpMobile" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Mobile %>"></asp:Label>:
            </td>
            <td align="left" style="height: 19px">
                &nbsp;<asp:Label ID="lblEmployeeMobile" runat="server" CssClass="field_text"></asp:Label>
            </td>
            <td align="right" class="field_column_bg" style="height: 19px" valign="middle">
                <asp:Label ID="lblMgrMobile" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Mobile %>"></asp:Label>:
            </td>
            <td style="width: 20%; height: 19px;" align="left">
                &nbsp;<asp:Label ID="lblManagerMobileNo" runat="server" CssClass="field_text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                <asp:Label ID="lblEmpExtension" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Extension %>"></asp:Label>:
            </td>
            <td align="left" style="height: 15px">
                &nbsp;<asp:Label ID="lblEmployeeExtension" runat="server" CssClass="field_text"></asp:Label>
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                <asp:Label ID="Label4" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Extension %>"></asp:Label>:
            </td>
            <td style="width: 20%" align="left">
                &nbsp;<asp:Label ID="lblManagerExtension" runat="server" CssClass="field_text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                <asp:Label ID="lblLocation" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, Location %>"></asp:Label>:
            </td>
            <td align="left" style="height: 15px">
                &nbsp;<asp:Label ID="lblEmpLocation" runat="server" CssClass="field_text"></asp:Label>
            </td>
            <td align="left" style="height: 15px" valign="middle">
            </td>
            <td style="width: 20%">
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                <asp:Label ID="lblCity" runat="server" CssClass="field_text" Text="<%$ Resources:LocalizedText, City %>"></asp:Label>:
            </td>
            <td align="left" style="height: 15px">
                &nbsp;<asp:Label ID="lblEmpCity" runat="server" CssClass="field_text"></asp:Label>
            </td>
            <td align="left" style="height: 15px" valign="middle">
            </td>
            <td style="width: 20%">
            </td>
        </tr>
        <%--<tr>
                                 <td align="right" class="field_column_bg" style="height: 15px">
                                     <asp:Label ID="lblDepartment" runat="server" CssClass="field_text" Text="Department "></asp:Label>:</td>
                                 <td align="left" style="height: 15px">
                                     &nbsp;<asp:Label ID="lblEmpDepartment" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="left" style="height: 15px" valign="middle">
                                 </td>
                                 <td style="width: 20%">
                                 </td>
                             </tr>--%>
    </table>
</asp:Panel>
