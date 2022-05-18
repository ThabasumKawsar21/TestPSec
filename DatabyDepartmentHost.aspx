<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatabyDepartmentHost.aspx.cs"
    Inherits="VMSDev.DatabyDepartmentHost" MasterPageFile="~/VMSMain.Master" %>

<asp:Content ID="DatabyDepartmentHost" ContentPlaceHolderID="VMSContentPlaceHolder"
    runat="server">
    <table id="Table1" runat="server">
     <tr >
    <td colspan="8" align="left" >       
            <asp:Label ID="lblVisitorInfo" runat="server" CssClass="lblHeada" Text="DatabyDepartment"></asp:Label>
            <br />
            <br />
        </td>
    </tr>
      <%--  <tr>
            <td colspan="3">
                <asp:Label ID="lblTitle" runat="server" BackColor="#0080C0" Font-Bold="True" ForeColor="White"
                    Width="400px" Height="25px" Text="Data by Department and Host"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td class="tdBold" align="right">
                <asp:Label ID="lblDepartment" runat="server" Text="Department" CssClass="lblField"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlDepartment" runat="server" 
                    CssClass="ddlField" Width="170px">
                </asp:DropDownList>
            </td>
            <td class="tdBold" align="right">
                <asp:Label ID="lblHost" runat="server" Text="Host" CssClass="lblField"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtHost" runat="server" CssClass="txtField" ></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click"
                    CssClass="cssButton"  Width="120px" />
            </td>
            <td>
            
            
                <asp:ImageButton ImageUrl="~/Images/excel_icon.GIF" ID="btnExport" runat="server"
                    Text="Export" OnClick="BtnExport_Click" Height="16px" Width="19px" />
            
            
            </td>
            
        </tr>
    </table>
    <hr width= 100% height=0.5 />
    <table>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td> 
             <asp:Label ID="lblResult" runat="server" Text="Label" ForeColor="Red"  CssClass="cssDisplay" width="350"></asp:Label>
            <div>
                <asp:Panel ID="PanelGrid" runat="server" ScrollBars="Auto" margin="0px" Height="200px" HorizontalAlign="Center "  >
                <asp:GridView ID="grdResult" runat="server" CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                    AutoGenerateColumns="False" CssClass="txtField" DataKeyNames="RequestID" 
                       >
                    <RowStyle BackColor="#EFF3FB" CssClass="grdField" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sl.no">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:BoundField HeaderText="Company" DataField="Company" />
                        <asp:BoundField HeaderText="Designation" DataField="Designation" />
                        <asp:BoundField HeaderText="From Date" DataField="FromDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="To Date" DataField="ToDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="In Time" DataField="Intime" />
                        <asp:BoundField HeaderText="Out Time" DataField="ExpectedOutTime" />
                        <asp:BoundField HeaderText="Actual Out Time" DataField="ActualOutTime" />
                        <asp:BoundField HeaderText="Native Country" DataField="NativeCountry" />
                        <asp:BoundField HeaderText="Purpose" DataField="Purpose" />
                        <asp:BoundField HeaderText="Visiting City" DataField="VisitingCity" />
                        <asp:BoundField HeaderText="Facility" DataField="Facility" />
                        <asp:BoundField HeaderText="Host" DataField="Host" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
               
                </asp:Panel>
                </div>
                
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
