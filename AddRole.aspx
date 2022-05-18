<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    CodeBehind="AddRole.aspx.cs" Inherits="VMSDev.Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <br />
    <script type="text/javascript" language="JavaScript">
        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }

        function RevokeAccess() {
            var associateName = document.getElementById('<%=txtAssociateName.ClientID %>').value;
            var associateID = document.getElementById('<%=txtAssociateID.ClientID %>').value;

            if (associateName != '' && associateID != '')
                return confirm('Are you sure to revoke the access?');
            else if (associateName == '') {
                alert('Associate Name field cannot be empty');
                return false;
            }
            else if (associateID == '') {
                alert('Associate ID field cannot be empty');
                return false;
            }
        }
    </script>
    <br />
    <table id="tblAddRole" cellpadding="0" cellspacing="0" width="100%" style="height: 150px">
        <tr bgcolor="#D9E6DB">
            <td align="left" style="height: 3%" colspan="7">
                &nbsp;<asp:Label ID="lblRoleHeader" runat="server" CssClass="Table_header"> Add/Edit Role</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="height: 15px; width: 20%;" class="field_column_bg">
                <asp:Label ID="lblAssociateId" runat="server" CssClass="lblField" Style="text-align: left"
                    Text="Associate ID :"></asp:Label>&nbsp
            </td>
            <td align="left" style="height: 20px; width: 15%; white-space: nowrap">
                <asp:TextBox ID="txtAssociateID" align="left" maxlength="15" runat="server" Style="text-align: left"
                    CssClass="txtField" Width="180px" onkeypress="return onlyNumbers()" AutoCompleteType="Disabled"></asp:TextBox>
                <asp:ImageButton ID="imbFindAssociate" runat="server" Height="20px" ImageUrl="~/Images/Search_1.png"
                    Width="22px" AutoPostBack="True" OnClick="ImbFindAssociate_Click" ToolTip="Search Associate" ValidationGroup="AssociateID_Validate"/>
            </td>
            <td align="left" style="width: 15%">
                &nbsp;
            </td>
            <td align="right" style="height: 15px; width: 13%;" valign="middle" class="field_column_bg">
                <asp:Label ID="Label1" runat="server" CssClass="lblField" Style="text-align: left"
                    Text="Country :"></asp:Label>
                &nbsp
            </td>
            <td style="width: 15%" align="left">
                <asp:DropDownList ID="drpCountry" runat="server" align="left" AutoPostBack="True"
                    CssClass="field_text" OnSelectedIndexChanged="DrpCountry_SelectedIndexChanged"
                    TabIndex="2" Width="185px">
                </asp:DropDownList>
            </td>
            <td align="left" style="width: 15%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="field_column_bg">
                &nbsp;
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAssociateID" runat="server"
                    ControlToValidate="txtAssociateID" Display="Dynamic" ErrorMessage="Enter Associate ID"
                    Font-Names="Arial Narrow" Font-Size="Small" ValidationGroup="Submit_Validate"></asp:RequiredFieldValidator>

               <%-- <asp:RangeValidator ID="RangeValidator1" ControlToValidate="txtAssociateID" runat="server"
                    ErrorMessage="Enter a Valid Associate ID" Display="Dynamic" Font-Names="Arial Narrow"
                    Font-Size="Small" ValidationGroup="Submit_Validate"  MinimumValue="100000"
                    Type="Double"></asp:RangeValidator>--%>

            </td>
            <td>
                &nbsp;
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                &nbsp;
            </td>
            <td style="width: 20%; height: 19px;" align="left">
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" ControlToValidate="drpCountry"
                    ErrorMessage="Select Country" Font-Names="Arial Narrow" Font-Size="Small" ValidationGroup="Submit_Validate"></asp:RequiredFieldValidator>--%>
             <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="Select Country"
        OnServerValidate="CheckCountry" Display="Dynamic" ValidationGroup="Submit_Validate" Font-Names="Arial Narrow" Font-Size="Small"
        ControlToValidate="drpCountry"></asp:CustomValidator>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" style="height: 15px; width: 20%;" class="field_column_bg">
                <asp:Label ID="lblAssociateName" runat="server" Style="text-align: left" CssClass="lblField"
                    Text="Associate Name :"></asp:Label>&nbsp
            </td>
            <td align="left">
                <%--   <asp:Label ID="lblMessageValidAssoc" runat="server" Font-Names="Arial Narrow" Font-Size="Small"
                        ForeColor="Red"></asp:Label>--%>
                <asp:TextBox ID="txtAssociateName" runat="server" CssClass="txtField" ReadOnly="True"
                    Width="180px" TabIndex="1"></asp:TextBox>
            </td>
            <td>
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                <asp:Label ID="lblCity" runat="server" CssClass="lblField" Style="text-align: left"
                    Text="City :"></asp:Label>
            </td>
            <td style="width: 20%; height: 19px;" align="left">
                <asp:DropDownList ID="ddlCity" runat="server" align="left" AutoPostBack="True" CssClass="field_text"
                    OnSelectedIndexChanged="DdlCity_SelectedIndexChanged" TabIndex="2" Width="185px">
                </asp:DropDownList>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                &nbsp;
            </td>
            <td align="left" style="height: 15px">
                &nbsp;
            </td>
            <td align="left" style="width: 5%">
                &nbsp;
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                &nbsp;
            </td>
            <td style="width: 20%; height: 19px;" align="left">
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorCity" runat="server" ControlToValidate="ddlCity"
                    ErrorMessage="Select City" Font-Names="Arial Narrow" Font-Size="Small" ValidationGroup="Submit_Validate"></asp:RequiredFieldValidator>--%>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Select City"
        OnServerValidate="CheckCity" ValidationGroup="Submit_Validate" Display="Dynamic"  Font-Names="Arial Narrow" Font-Size="Small"
        ControlToValidate="ddlCity"></asp:CustomValidator>
            </td>
            <td style="width: 10%; height: 19px;" align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                <asp:Label ID="lblUserRole" runat="server" CssClass="lblField" Style="text-align: left"
                    Text="User Role:"></asp:Label>&nbsp
            </td>
            <td align="left" style="height: 15px">
                <asp:DropDownList ID="ddlUserRole" runat="server" Width="185px" CssClass="field_text"
                    AutoPostBack="True" TabIndex="4" OnSelectedIndexChanged="DdlUserRole_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td align="left" style="width: 5%">
                &nbsp;
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                <asp:Label ID="lblFacility" runat="server" Style="text-align: left" CssClass="lblField"
                    Text="  Facility :"></asp:Label>
            </td>
            <td style="width: 20%; height: 19px;" align="left">
                <asp:DropDownList ID="ddlFacility" runat="server" Width="185px" CssClass="field_text"
                    TabIndex="3">
                </asp:DropDownList>
            </td>
            <td style="width: 10%; height: 19px;" align="left">
            </td>
        </tr>
        <tr>
            <td class="field_column_bg">
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlUserRole"
                    ErrorMessage="Select User Role" Font-Names="Arial Narrow" Font-Size="Small" ValidationGroup="Submit_Validate"></asp:RequiredFieldValidator>
            </td>
            <td>
            </td>
            <td class="field_column_bg">
            </td>
            <td align="left">
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorFacility" runat="server" ControlToValidate="ddlFacility"
                    ErrorMessage="Select Facility" Font-Names="Arial Narrow" Font-Size="Small" ValidationGroup="Submit_Validate"></asp:RequiredFieldValidator>--%>
            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Select Facility"
        OnServerValidate="CheckFacility" ValidationGroup="Submit_Validate" Display="Dynamic"  Font-Names="Arial Narrow" Font-Size="Small"
        ControlToValidate="ddlFacility"></asp:CustomValidator>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="right" class="field_column_bg" style="height: 15px">
                <asp:Label ID="lblIDCardLocation" runat="server" CssClass="lblField" Style="text-align: left"
                    Text="ID Card Admin Location :" Visible="False"></asp:Label>
                &nbsp
            </td>
            <td align="left" style="height: 15px">
                <asp:DropDownList ID="ddlIDCardLocation" runat="server" CssClass="field_text" TabIndex="5"
                    Visible="False" Width="185px">
                </asp:DropDownList>
            </td>
            <td align="left" style="width: 5%">
                &nbsp;
            </td>
            <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                &nbsp
            </td>
            <td style="width: 20%" align="left">
                &nbsp;
            </td>
            <td align="left" style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="field_column_bg">
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIDCardLocation" runat="server"
                    ControlToValidate="ddlIDCardLocation" ErrorMessage="Select ID Card Location"
                    Font-Names="Arial Narrow" Font-Size="Small" ValidationGroup="Submit_Validate"></asp:RequiredFieldValidator>
            </td>
            <td>
            </td>
            <td class="field_column_bg">
            </td>
            <td align="left">
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="4" rowspan="2">
                <asp:Panel ID="pnlError" Width="600px" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1" Visible="false">
                    <table width="600px" style="margin: 0 0 0 0; padding: 0 0 0 0">
                        <tr>
                            <td width="70px" valign="middle">
                                <img height="25px" src="Images/error_icon.jpg" alt="" oncontextmenu="false" />
                            </td>
                            <td width="30px">
                                &nbsp;
                            </td>
                            <td width="500px" align="left">
                                <asp:Label ID="lblError" CssClass="errorLabel" Font-Size="12px" runat="server" Text="ErrorDescription"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td style="text-align: left" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" CssClass="cssButton" OnClick="BtnSubmit_Click"
                    TabIndex="5" Text="Submit" ValidationGroup="Submit_Validate" Width="65px" />
                &nbsp
                <asp:Button ID="btnViewAllocation" runat="server" Text="View Allocation" CssClass="cssButton"
                    OnClick="BtnViewAllocation_Click" TabIndex="6" />
                &nbsp
                <asp:Button ID="btnRevokeAccess" runat="server" Text="Revoke Access" CssClass="cssButton"
                   TabIndex="7"  Width="120px" 
                    onclick="BtnRevokeAccess_Click" 
                    onclientclick="javascript:return RevokeAccess();"/>
                &nbsp
                <asp:Button ID="btnClear" runat="server" CssClass="cssButton" OnClick="BtnClear_Click"
                    TabIndex="8" Text="Clear" Width="65px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td>
                &nbsp
            </td>
        </tr>
    </table>    
    
    <table style="float: none; width: 100%; background-color: #edf6fd; border-top-style: dotted;
        border-top-color: Black; border-top: 1px">
        <tr bgcolor="#D9E6DB">
            <td align="left" style="height: 3%">
                <asp:Label ID="lblAssociateDetails" runat="server" CssClass="Table_header" Text="User Allocations"
                    Font-Bold="True" Font-Size="12px"></asp:Label>
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right: 15px">
                <asp:ImageButton ID="imbExcel" runat="server" ImageUrl="~/Images/excel_icon.GIF"
                    AlternateText="Export to Excel" Height="20px" OnClick="ImbExcel_Click" TabIndex="8"
                    Visible="false" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="gvAllocations" CssClass="GridText" runat="server" Font-Names="Arial"
                    Font-Size="Small" ForeColor="#333333" GridLines="Both" Width="95%" EmptyDataText="No Records Found"
                    AllowPaging="True" Style="text-align: center" OnPageIndexChanging="GvAllocations_PageIndexChanging"
                    AutoGenerateColumns="False" >

                    <HeaderStyle BackColor="#006699" BorderColor="#003399" Font-Bold="True" ForeColor="White"
                        Font-Size="12px" />
                    <AlternatingRowStyle BackColor="#F4FCED" Font-Size="12px" Height="20px" />
                    <RowStyle BackColor="#EFF3FB" CssClass="grdField" Font-Size="12px" Height="20px" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo." ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Associate ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblAssociateID" runat="server" Text='<%#Eval("AssociateID")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Role" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblRole" runat="server" Text='<%#Eval("Role")%>' Style="padding-left: 5px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Country" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("Country")%>' Style="padding-left: 5px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City")%>' Style="padding-left: 5px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Facility" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblFacility" runat="server" Text='<%#Eval("Facility")%>' Style="padding-left: 5px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Provided By" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Provided On" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedOn" runat="server" Text='<%#Eval("UpdatedOn")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Panel ID="hdnGrid" runat="server" Visible="false">
                    <asp:GridView runat="server" ID="hdnGridView" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="SNo." ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Associate ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblAssociateID" runat="server" Text='<%#Eval("AssociateID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Role" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblRole" runat="server" Text='<%#Eval("Role")%>' Style="padding-left: 5px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%--added by Krishna(449138) for temp access card--%>
                            <asp:TemplateField HeaderText="Country" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("Country")%>' Style="padding-left: 5px" />
                                </ItemTemplate>
                            </asp:TemplateField> 

                            <asp:TemplateField HeaderText="City" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City")%>' Style="padding-left: 5px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Facility" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblFacility" runat="server" Text='<%#Eval("Facility")%>' Style="padding-left: 5px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Provided By" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Provided On" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdatedOn" runat="server" Text='<%#Eval("UpdatedOn")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
