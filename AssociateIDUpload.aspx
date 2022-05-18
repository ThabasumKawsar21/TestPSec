<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true" CodeBehind="AssociateIDUpload.aspx.cs" Inherits="VMSDev.AssociateIDUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <style>
        .GridStyle
        {
            border: 1px solid #808080;
            border-collapse: collapse;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-style: normal;
            font-weight: normal;
            color: #575757;
            text-decoration: none;
        }
        .cellText
        {
            padding-left: 10px;
        }
    </style>
<div>
<table width="100%" cellspacing="1" cellpadding="1"  style="border:1px solid lightgrey;">
                                <tr>
   <td align="left" class="table_header_bg" style="height: 19px">
                            &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" CssClass="Table_header" Text="Associate ID Upload"></asp:Label>
                        </td>
                    
</tr>
                                <tr>
                             <td valign="top">
                              <table id="Table1" cellpadding="1" cellspacing="2" width="100%">
                               <tr>
                                    <td colspan="2" align="right" style="font-weight:bold;" class="field_txt">
                                        <asp:HyperLink ID="hplAssociateIDUploadTemplate" Font-Underline="true"
                                            NavigateUrl="~/MailTemplates/AssociateID_Upload_Template.xlsx" runat="server">Associate ID Upload Template</asp:HyperLink>
                                    </td>
                                </tr>
                              <tr>
                               <td align="right" style="width: 25%">
                                        <asp:Label ID="lblExcelFile" runat="server" CssClass="field_txt" Text="Select the Excel File :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="fupExcel" runat="server" Width="256px" 
                                            CssClass="txtField" />
                                    </td>
                              </tr>
                              <tr>
                                    <td align="right" valign="middle" style="height:25px;">
                                        <asp:Button ID="btnUploadExcel" runat="server" BackColor="#767561" Font-Bold="False"
                                            ForeColor="White" Text="Upload" Font-Size="10px" Height="20px" Width="70px" OnClick="btnUploadExcel_Click" /></td>
                                    <td align="left" valign="middle" style="height: 25px">
                                        <asp:Button ID="btnClearExcel" runat="server" BackColor="#767561" Font-Bold="False"
                                            Font-Size="10px" ForeColor="White" Text="Clear" Height="20px" Width="70px" OnClick="btnClearExcel_Click" />
                                    </td>
                               
                                </tr>
                                
                                  <tr>
                                  <td style="height: 25px"></td>
                                    <td  align="left">
                                    <asp:Label ID="lblMessage" runat="server" CssClass="field_txt" style="font-weight:bold;" Visible="false" ></asp:Label>
                                    </td>
                                    </tr>
                                       <tr>
                                    <td colspan="2" align="center">
                                        <asp:GridView ID="DataGrid2" runat="server" CssClass="GridStyle" 
                                            AutoGenerateColumns="false" HorizontalAlign="Center" PagerStyle-HorizontalAlign="Center"
                                            Visible="false">
                                            <RowStyle CssClass="field_txt" HorizontalAlign="Left" Height="22px" />
                                            <HeaderStyle BackColor="#C6D4BB" CssClass="field_txt" HorizontalAlign="Center" Height="25px" />
                                            <Columns>
                                                <asp:BoundField ItemStyle-CssClass="cellText" HeaderText="Applicant ID" DataField="ApplicantID"
                                                    HeaderStyle-Width="120px" />
                                                <asp:BoundField ItemStyle-CssClass="cellText" HeaderText="Associate ID" DataField="AssociateID"
                                                    HeaderStyle-Width="90px" />
                                                <asp:BoundField ItemStyle-CssClass="cellText" HeaderText="Associate Name" DataField="AssociateName" 
                                                 HeaderStyle-Width="175px" />
                                                <asp:BoundField ItemStyle-CssClass="cellText" HeaderText="Error Description" DataField="Error"
                                                    HeaderStyle-Width="170px" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                              </table>
                             </td>
                                   
                             
                                </tr>
                               
                               
                              
</table>
</div>
</asp:Content>
