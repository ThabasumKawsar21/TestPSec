<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewHistorybyHost.ascx.cs" Inherits="VMSDev.UserControls.ViewHistorybyHost" %>
<script language="javascript" type="text/javascript">
	 //************************To allow only Alpha characters in text box fields******************************
     function allowAlpha(ie, moz)
		{
		
		if(moz!= null) 
		{
		//alert(moz);
	     if ((moz == 32) || (moz >= 65) && (moz < 91) || moz == 8 || moz == 13 || (moz >= 97) && (moz < 123))
		{
		return true;
		}
		else
		{
		return false;
		}
		}
		else
		{

		if ((ie == 32) || (ie >= 65) && (ie < 91) || (ie >= 97) && (ie < 123))	
		{
		return true;
		}
		else
		{
		return false;
		}
		}
		
		}
    </script>
    <table class="tblHeadStyle" width="100%"  >
        <tr>
            <td class ="tdBold" align="right" style="width:15%">
                <asp:Label ID="lblVisitorFirstName" runat="server" CssClass="lblField" Text="Visitor First Name" Width="129px"></asp:Label>
             </td>
             <td align="left">
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="txtField" onKeyPress="return allowAlpha(event.keyCode, event.which);"> </asp:TextBox>
             </td>
             <td class ="tdBold" align="right" style="width:15%">
                <asp:Label ID="lblVisitorLastName" runat="server" CssClass="lblField" Text="Visitor Last Name"></asp:Label>
             </td>
             <td align="left">
             <asp:TextBox ID="txtLastName" runat="server" CssClass="txtField" 
                     onKeyPress="return allowAlpha(event.keyCode, event.which);" TabIndex="1"></asp:TextBox>         
             </td>
             <td class ="tdBold" align="right"  style="width:15%">
                <asp:Label ID="lblCompany" runat="server" CssClass="lblField" Text="Company"></asp:Label>
             </td>
             <td align="Left">
             <asp:TextBox ID="txtCompany" runat="server" CssClass="txtField" TabIndex="2"></asp:TextBox>      
             </td>
            
        </tr>
        <tr>
           <td class ="tdBold" align="right"><asp:Label ID="lblDesignation" runat="server" CssClass="lblField" Text="Designation" ></asp:Label></td>     
            <td align="left">
                <asp:TextBox ID="txtDesignation" runat="server" CssClass="txtField" 
                    onKeyPress="return allowAlpha(event.keyCode, event.which);" TabIndex="3"></asp:TextBox>
            </td>
            <td class ="tdBold" align="right">
                <asp:Label ID="lblNativeCountry" runat="server" CssClass="lblField" Text="Native Country"></asp:Label>
            </td>
            <td  align="left">
                <asp:DropDownList ID="ddlNativeCountry" runat="server" CssClass="ddlField" 
                    Width="120px" TabIndex="4" ></asp:DropDownList>
            </td>
            <td class ="tdBold" align="right">
                <asp:Label ID="lblPurpose" runat="server" CssClass="lblField" Text="Visitor Type"></asp:Label>
            </td>
            <td  align="left">
                <asp:DropDownList ID="ddlPurpose" runat="server" Height="30px" 
                    onselectedindexchanged="ddlPurpose_SelectedIndexChanged" 
                    AutoPostBack="True" CssClass="ddlField" TabIndex="5"></asp:DropDownList>
                <asp:TextBox ID="txtOthers" runat="server" ></asp:TextBox>
           </td>
       </tr>
       <tr>
        <td class ="tdBold" align="right"><asp:Label ID="lblVisitingCity" runat="server" CssClass="lblField" Text="Visiting City" ></asp:Label></td>
        <td align="left">
            <asp:DropDownList ID="ddlVisitingCity" runat="server" TabIndex="6" 
               OnSelectedIndexChanged="ddlVisitingCity_SelectedIndexChanged" AutoPostBack=true CssClass="ddlField"></asp:DropDownList>
        </td>
        <td class ="tdBold" align="right">
            <asp:Label ID="lblFacility" runat="server" CssClass="lblField" Text="Facility"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="ddlFacility" runat="server" TabIndex="7" CssClass="ddlField">
                <asp:ListItem>Select</asp:ListItem>
         </asp:DropDownList>
        </td>
        <td colspan="2" align="right">
            <asp:Button ID="btnSearch" runat="server" Text="Search" 
                onclick="BtnSearch_Click" CssClass="cssButton" TabIndex="8" />&nbsp;&nbsp;
            <asp:Button ID="btnReset" runat="server" onclick="btnReset_Click1" 
                Text="Reset"  CssClass="cssButton" TabIndex="9"/>
      </td>
    </tr>
  </table>
  <hr width=100% height=1 />
  <br />
  <table style="width:100%;">
      <tr>
        <td align="center">
        <asp:Label ID="lblResult" runat="server"  ForeColor="Red" Text="" CssClass="cssDisplay"></asp:Label>
        </td>
       </tr> 
      <tr><td>&nbsp;</td></tr>
      <tr>
          <td>
            <div style="height:100%; width:100%;">
                <asp:Panel ID="PanelGrid" runat="server" ScrollBars="Auto" margin="0px" Height="400px" HorizontalAlign="Center "  >
                <asp:GridView ID="grdResult"  runat="server" CellPadding="4" ForeColor="#333333" Width="100%" 
                     AutoGenerateColumns="False" DataKeyNames="RequestID" GridLines="Vertical"  
                     onrowcommand="grdResult_RowCommand" PageSize="15" AllowPaging="True"  onpageindexchanging="grdResult_PageIndexChanging" >
                    <RowStyle BackColor="#EFF3FB" CssClass="grdField"/>
                    <Columns>
                     <asp:TemplateField HeaderText="Sl.no">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                        <asp:BoundField HeaderText = "Name" DataField="Name" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText = "Company" DataField="Company" ItemStyle-HorizontalAlign="Left"  />
                        <asp:BoundField HeaderText = "Email ID" DataField="EmailID" ItemStyle-HorizontalAlign="Left"  />
                        <asp:BoundField HeaderText="From Date" DataField="FromDate" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />
                        <asp:BoundField HeaderText="To Date" DataField="ToDate" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />
                        <asp:BoundField HeaderText="Designation" DataField="Designation" ItemStyle-HorizontalAlign="Left"  />
                        <asp:BoundField HeaderText="Native Country" DataField="NativeCountry" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="Visitor Type" DataField="Purpose" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />
                        <asp:BoundField HeaderText="Visiting City" DataField="VisitingCity" ItemStyle-HorizontalAlign="Left"  />
                        <asp:BoundField HeaderText="Host" DataField="Host" ItemStyle-HorizontalAlign="Center"  />
                        <asp:TemplateField HeaderText="View Details" ItemStyle-HorizontalAlign="Center" >
                             <ItemTemplate  >
                            <asp:LinkButton ID="Edit"  CommandName="ViewDetailsLink"  CommandArgument='<%#Eval("RequestID")%>'  runat="server"   text="<img src='images/search.gif' border=0 title='View Details' width='13' height='13'>"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                     </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" CssClass="grdField" ForeColor= "White" Font-Bold="True"  />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White"/>
                </asp:GridView>
                </asp:Panel>
            </div>
          </td>
      </tr>
  </table>
