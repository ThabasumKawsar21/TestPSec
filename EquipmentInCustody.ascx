<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EquipmentInCustody.ascx.cs" Inherits="VMSDev.UserControls.EquipmentInCustody" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<body>
<table class="tblHeadStyle" width="100%">
    <tr>
        <td colspan="8" align="left">        
            <asp:Label ID="lblEquipmentsCustody" runat="server" CssClass="lblHeada"  
                Text="<%$ Resources:LocalizedText, lblEquipmentsCustody %>" Font-Bold="True" 
                Font-Names="Arial Unicode MS"></asp:Label>   
                </td>              
    </tr>
   </table>
   <table style="border-color: #000000; border-width: 5px;">
   <tr >
   <td >
      <asp:updatepanel updatemode="Conditional" id="upnlEquipments" runat="server">
   
                           
      <contenttemplate>

                                <asp:GridView ID="grdEquipments" runat="server" AutoGenerateColumns="false" GridLines="None"
                                    CellPadding="1" CellSpacing="1" Width="500px" BorderStyle="Solid" 
                                    onrowdeleting="GrdEquipments_RowDeleting" 
                                    onrowdatabound="GrdEquipments_RowDataBound" Font-Names="Calibri" 
                                    Font-Size="Small">
                                    <RowStyle BorderStyle="None" />
                                    <Columns>
                                        <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false" />                                      
                                        <asp:TemplateField  HeaderText="<%$ Resources:LocalizedText, EquipmentType %>"  HeaderStyle-HorizontalAlign="Left" 
                                            ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEquipmentType" runat="Server" Text='<%# Bind("EquipmentTypeId")%>' visible="false"
                                                    ></asp:Label>
                                                <asp:DropDownList ID="ddlEquipmentType" runat="server" Width="120px" AutoPostBack="true"
                                                    Height="25px" OnSelectedIndexChanged="DdlEquipmentType_SelectedIndexChanged" />
                                                <%--<asp:TextBox ID="txtOtherEquipment" runat="server" name="txtOtherEquipment" Text='<%#Bind("Others")%>'
                                                    MaxLength="20" Style="width: 70px;" />--%>
                                            </ItemTemplate>
                                            <%--  <HeaderStyle Font-Bold="false" HorizontalAlign="Center" />--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedText, Description %>" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescription" runat="Server" MaxLength="20" OnPaste="false" Width="220px"
                                                    Text='<%# Bind("Description")%>' ></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="false" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        
                                       
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ButtonAdd" runat="server" ImageUrl="~/Images/IT_equip.jpg" CausesValidation="false"
                                                    Width="20px" Height="20px" OnClick="ButtonAdd_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="false" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle Font-Bold="false" />
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="ButtonDelete1" runat="server" ImageUrl="~/Images/IT_equip2.jpg"
                                                    CausesValidation="false" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                                    BackColor="#edf6fd" Width="20px" Height="20px" />--%>
                                                    <asp:ImageButton ID="ButtonDelete" runat="server"  ImageUrl="~/Images/IT_equip2.jpg" CommandName="Delete" CausesValidation="false" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                                    Width="20px" BackColor="#edf6fd" Height="20px" />
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                    </Columns>
                                </asp:GridView>
                               <%-- <asp:RequiredFieldValidator ID="RequiredDescription" runat="server" ControlToValidate="grdEquipments
                             ErrorMessage="Please enter Equipment Description" Display="None"></asp:RequiredFieldValidator>
                        <cc5:ValidatorCalloutExtender ID="ValidatorCalloutExtenderDesc1" runat="server" TargetControlID="RequiredDescription">
                        </cc5:ValidatorCalloutExtender>--%>
                              

                                </contenttemplate>
                                </asp:updatepanel>
                           
                                </td>
                                <td style="width:100px;">
                                <table style="width:100px;">
                                <tr><td>
                                <asp:Label ID="lbltokenNumber" runat="Server" Width="80px"
                                        Text="<%$ Resources:LocalizedText, lbltokenNumber %>" visible="False" 
                                        Font-Names="Calibri" Font-Size="Small"
                                                    ></asp:Label>
                                                    </td>
                                                    <td>
                                                <asp:Label ID="txttoken" runat="Server" MaxLength="20" ReadOnly="true" 
                                                    OnPaste="false" Width="50px" Visible="False"></asp:Label>
                                                    </td>
                                                    </tr>
                                                    </table>
                                                    </td>
                                                    </table> 
                                                     
                                           <asp:CustomValidator ID="custDescription" runat="server"
                                  ErrorMessage="Please enter Equipment Description"
                                  OnServerValidate="Check_Description" Display="None">
                                  </asp:CustomValidator> 
                                             
                                             
                       
</body>
