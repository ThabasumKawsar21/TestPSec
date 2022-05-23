<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchPage.aspx.cs" Inherits="VMSDev.SearchPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Search Visitor Page</title>
    <link href="App_Themes/UIStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function restoreScr() {
            // window.opener.blur();
            //         alert("hi");
            ////         window.open();
            //         opener.history.go(0);
            //
            self.focus();
            parent.blur();
            window.focus();

            //         window.opener.focus();

            //window.close();
        }


        function restoreScrclose() {
            // window.opener.blur();

            window.close();
            //         window.opener.focus();

        }
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 50%">
        <table style="padding: 10px; float: left; width: 100%">
            <tr>
                <td colspan="2">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                   <asp:SqlDataSource ID="dsSearchResult" runat="server" ConnectionString="<%$ ConnectionStrings:VMSConnectionString %>"
                        SelectCommand="SearchMasterDetails" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnSearchText" DefaultValue=" " Name="Search" PropertyName="Value"
                                Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtSearch" runat="server" Width="290px"></asp:TextBox>
                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" WatermarkText="first name, last name,mobile number,company"
                        TargetControlID="txtSearch" runat="server">
                    </asp:TextBoxWatermarkExtender>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" CssClass="cssButton" Text="Search" Width="80px"
                        OnClick="BtnSearch_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblVisitorInfo" runat="server" CssClass="lblField" Text="Visitor Information"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:GridView ID="grdSearchResult" runat="server" CellPadding="4" GridLines="None"
                        Width="95%" DataSourceID="dsSearchResult" AutoGenerateColumns="False" CssClass="lblField"
                        DataKeyNames="VisitorID" OnRowCommand="grdSearchResult_RowCommand">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField HeaderText="Name" DataField="Name" />
                            <asp:BoundField HeaderText="Company" DataField="Company" />
                            <asp:BoundField HeaderText="Designation" DataField="Designation" />
                            <asp:BoundField HeaderText="Mobile No." DataField="MobileNo" />
                            <asp:TemplateField HeaderText="View Details" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="Edit" CommandArgument='<%#Eval("VisitorID") %>' runat="server"
                                        Text="<img src='images/search.gif' border=0 title='View Details' width='13' height='13'>"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    <asp:HiddenField ID="hdnHostID" runat="server" />
                    <asp:HiddenField ID="hdnSearchText" runat="server" />
                    <table border="0" id="errortbl" cellpadding="0" cellspacing="0" runat="server" style="border-right: black 1pt solid;
                        border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid;
                        width: 60%;">
                        <tr>
                            <td colspan="6" align="Center" style="background-color: #edf6fd; width: 60%">
                                <asp:Label ID="lblMessage" runat="server" CssClass="lblField" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
