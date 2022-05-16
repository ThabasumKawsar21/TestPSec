<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EquipmentPermittedSP.ascx.cs" Inherits="VMSDev.SafetyPermitUserControls.EquipmentPermittedSP" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<body>
<script language="javascript" type="text/javascript">
    function ViewOtherTxtBoxControl(obj, others) 
    {
        var txtOtherControlID = obj.id.replace('ddlEquipmentType', 'txtOthers');
        var txtOtherControl = document.getElementById(txtOtherControlID)

        var selected_text = obj.options[obj.selectedIndex].text;
        if (selected_text == others)
                txtOtherControl.style.visibility = "visible";
		    else
		        txtOtherControl.style.visibility = "hidden";
		}
    function EquipmentValidation() {    
		   alert("Please Select any one of the Item"); 
		}
</script>

<table class="tblHeadStyle" width="100%">
    <tr>
        <td colspan="8" align="left">        
            <asp:Label ID="lblEquipmentsPermitted" runat="server" CssClass="lblHeada"  Text="<%$ Resources:LocalizedText, EquipmentPermitted %>"></asp:Label>            
        </td>              
    </tr>
</table>  
 
<div>
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <table>
            <tr align="left">
            <td style="width:20px"></td>
                <td>
                 <asp:Panel ID="PanelEqipment" runat="server" ScrollBars="Auto" margin="0px" Height="100px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
                        Width="819px"  BorderStyle="Double" 
                        
                        OnRowDataBound="GridView1_RowDataBound" onRowDeleting="GridView1_RowDeleting"> 
                        <Columns>
                            <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false"/> 
                            <asp:TemplateField Headertext="<%$ Resources:LocalizedText, EquipmentType %>" ItemStyle-Width="250px">
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEquipmentType" runat="Server" Text='<%# Bind("EquipmentType")%>' Visible="false"></asp:Label>
                                                <asp:DropDownList id="ddlEquipmentType" runat="server" style="width:100px" AutoPostBack="true" class="ddlField" OnSelectedIndexChanged="DdlEquipmentType_SelectedIndexChanged" /></td>
                                       <%--    <asp:RequiredFieldValidator id="Requiredcountry" runat="server" ControlToValidate="ddlEquipmentType" InitialValue="0"  ErrorMessage="Select any one of the Item" Display="None"></asp:RequiredFieldValidator>
                                         <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="Requiredcountry"></cc2:ValidatorCalloutExtender>        
                                       --%>     <td>
                                                <input type="text" id="txtOthers" maxlength="15" runat="server" class="txtField" value='<%# Bind("Others")%>' style="width:100px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <HeaderStyle CssClass="lblField" />
                            </asp:TemplateField>
                            <asp:TemplateField Headertext="<%$ Resources:LocalizedText, Make %>">
                                <ItemTemplate>
                                  <asp:TextBox ID="txtMake" runat="Server" MaxLength="8" OnPaste="false" CssClass="txtField" Text='<%# Bind("Make")%>'></asp:TextBox>                        
                                </ItemTemplate> 
                                <HeaderStyle CssClass="lblField" />
                            </asp:TemplateField>
                            <asp:TemplateField Headertext="<%$ Resources:LocalizedText, Model %>">
                                <ItemTemplate>
                                  <asp:TextBox ID="txtModel" runat="Server" MaxLength="10" OnPaste="false" CssClass="txtField" Text='<%# Bind("Model")%>'></asp:TextBox>                        
                                </ItemTemplate> 
                                <HeaderStyle CssClass="lblField" />
                            </asp:TemplateField>
                            <asp:TemplateField Headertext="<%$ Resources:LocalizedText, SerialNo %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSerialNo" runat="server" MaxLength="20" OnPaste="false" CssClass="txtField" Text='<%# Bind("SerialNo")%>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle CssClass="lblField" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="ButtonAdd" runat="server" Text="+" OnClick="ButtonAdd_Click" BackColor="#edf6fd" CausesValidation="false" Width="27px" Height="25px"/> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="ButtonDelete" runat="server" Text="-" CausesValidation="false" CommandName="Delete" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' BackColor="#edf6fd" Width="27px" Height="25px"/> 
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    
                    </asp:GridView>
                   </asp:Panel>
                </td>
            </tr>
            <tr align="center">
                 <td colspan="4">
                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red" CssClass="cssDisplay"></asp:Label>
                </td>
            </tr>
         </table>
    </ContentTemplate>
</asp:UpdatePanel>
 

</div>
</body>
</html>
