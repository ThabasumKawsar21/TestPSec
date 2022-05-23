<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VolumeofVisitors.aspx.cs"
    Inherits="VMSDev.VolumeofVisitors" MasterPageFile="~/VMSMain.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="VMSVolumeofVisitorsContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">

    <script language="javascript " type="text/javascript">
        //************************To allow only Numbers in text box fields******************************
        function allowDateCharacters(ie, moz) {

            if (moz != null) {
                //alert(moz);
                if ((moz >= 47) && (moz < 58) || moz == 8 || moz == 13) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {

                if ((ie >= 47) && (ie < 58)) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }



		
		
    </script>
    <script type="text/javascript" language="javascript">

        function allowTimeCharacters(ie, moz) {

            if (moz != null) {
                //alert(moz);
                if ((moz > 47) && (moz < 59) || moz == 8 || moz == 13) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {

                if ((ie > 47) && (ie < 59)) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }
    
    
    </script>
    <table width="100%">
        <tr>
        <td style="width:100%; ">
            <table id="tblVolumeofVisitors" runat="server" align="left" class="txtField">
                <tr>
            <td align="left">       
                    <asp:Label ID="lblVisitorInfo" runat="server" CssClass="lblHeada" Text="Volume of Visitors" width="100%"></asp:Label>
                </td>
            </tr>
                <tr>
                    <td align="left" style="width:100%;">
                    <asp:Label ID="lblTitle" runat="server" CssClass="innerTableHead" Width="400px"></asp:Label>
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" 
                            OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged"
                            RepeatDirection="Horizontal" CssClass="lblField">
                            <asp:ListItem>By Hour</asp:ListItem>
                            <asp:ListItem>By Day</asp:ListItem>
                            <asp:ListItem>By Week</asp:ListItem>
                            <asp:ListItem>By Month</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                
            </table>
           
    </td>
    </tr>
        <tr>
    <td>
    <hr width= 100% height=0.5 />
        <table align="left" class="txtField">
            <tr>
                <td>
                 <table title="tab" id="tblByHour" runat="server" align="left" class="txtField">
                            <tr>
                                <td class="tdBold" align= "right">
                                    <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="lblField"></asp:Label>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="txtField" 
                                       ></asp:TextBox>
                                    <asp:ImageButton ID="cldrIcon1" runat="server"  ImageUrl="~/Images/calender-icon.png"
                                        Width="15px"  />
                                    <ajax:CalendarExtender ID="CalenderExtender1" runat="server" PopupButtonID="cldrIcon1"
                                        TargetControlID="txtDate" Format="dd/MM/yyyy" >
                                    </ajax:CalendarExtender>
                                     <ajax:MaskedEditExtender ID="MaskedEditExtender2" ClearMaskOnLostFocus="false" runat="server" TargetControlID="txtDate" Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Date"  InputDirection="RightToLeft"  ErrorTooltipEnabled="True"/>
                                      <asp:RequiredFieldValidator id="RequiredDate" runat="server" ClearMaskOnLostFocus="false"  ControlToValidate="txtDate" InitialValue="__/__/____" ErrorMessage="Enter To Date" Display="None"></asp:RequiredFieldValidator>
          <ajax:ValidatorCalloutExtender id="ValidatorCalloutExtender5" runat="server" TargetControlID="RequiredDate"></ajax:ValidatorCalloutExtender>
                                </td>
                                <td class="tdBold" align="right">
                                    <asp:Label ID="lblFromTime" runat="server" Text="From Time"  CssClass="lblField"></asp:Label>
                                </td>
                                <td  align="left">
                                    <asp:TextBox ID="txtFromTime" runat="server" CssClass="txtField"  ></asp:TextBox>
                                   <ajax:MaskedEditExtender ID="MaskedEditFromTime" runat="server" TargetControlID="txtFromTime" Mask="99:99" ClearMaskOnLostFocus="false"  MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft" ErrorTooltipEnabled="True"/>
 
  
    
    
    <ajax:MaskedEditValidator ID="MaskedEditValidator44" runat="server" ControlToValidate="txtFromTime" ControlExtender="MaskedEditFromTime" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid Time!" EmptyValueMessage="Time is not entered" />
 
 
 
 
 
 
 
    <%--<asp:RequiredFieldValidator id="RequiredFieldFT" runat="server" ControlToValidate="txtFromTime" InitialValue="__:__" ErrorMessage="Enter From Time" Display="None"></asp:RequiredFieldValidator>
    <ajax:ValidatorCalloutExtender id="ValidatorCalloutExtender6" runat="server" TargetControlID="RequiredFieldFT"></ajax:ValidatorCalloutExtender> 
    <asp:RangeValidator id="RangeFT" runat="server" ControlToValidate="txtFromTime" MaximumValue="24:59" MinimumValue="00:00" ErrorMessage="Enter valid From Time" Display="None"></asp:RangeValidator>
    <ajax:ValidatorCalloutExtender id="ValidatorCalloutExtender3" runat="server" TargetControlID="RangeFT"></ajax:ValidatorCalloutExtender>
   --%>
                                </td>
                                <td class="tdBold" align="right">
                                    <asp:Label ID="lblToTime" runat="server" Text="To Time" CssClass="lblField"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtToTime" runat="server" CssClass="txtField" 
                                        ontextchanged="txtToTime_TextChanged"  ></asp:TextBox>
                                     <ajax:MaskedEditExtender ID="MaskedEditExtenderToTime" runat="server" TargetControlID="txtToTime" Mask="99:99" ClearMaskOnLostFocus="false"  MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" InputDirection="RightToLeft" ErrorTooltipEnabled="True"/>
  <ajax:MaskedEditValidator ID="MaskedEditValidator45" runat="server" ControlToValidate="txtToTime" ControlExtender="MaskedEditExtenderToTime" Display="Dynamic" IsValidEmpty="false" InvalidValueMessage="Invalid Time!" EmptyValueMessage="Time is not entered" />
   <%-- <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtToTime" InitialValue="__:__" ErrorMessage="Enter From Time" Display="None"></asp:RequiredFieldValidator>
    <ajax:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldFT"></ajax:ValidatorCalloutExtender> 
    <asp:RangeValidator id="RangeValidator1" runat="server" ControlToValidate="txtToTime" MaximumValue="23:59" MinimumValue="00:00" ErrorMessage="Enter valid To Time" Display="None"></asp:RangeValidator>
    <ajax:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RangeValidator1"></ajax:ValidatorCalloutExtender>--%>
                                </td>
                            </tr>
                            </table>
                </td>
            </tr>
            <tr>
            <td>
               <table id="tblByWeek" runat="server" align="left" class="txtField">
                    <tr>
                        <td class="tdBold" align="right">
                            <asp:Label ID="LblFromDate" runat="server" Text="From Date" CssClass="lblField"></asp:Label>
                           
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtWeekFromDate" runat="server" AutoPostBack="True" CssClass="txtField"
                                OnTextChanged="txtWeekFromDate_TextChanged" 
                                onKeyPress="return allowDateCharacters(event.keyCode, event.which);" 
                                Width="100px" ></asp:TextBox>
                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="Calendericon2"
                                TargetControlID="txtWeekFromDate" Format="yyyy/MM/dd">
                            </ajax:CalendarExtender>
                            
                           
                        </td>
                        <td>
                            <asp:ImageButton ID="Calendericon2" runat="server"  ImageUrl="~/Images/calender-icon.png"
                                Width="15px" />
                        </td>
                        <td class="tdBold" align="right">
                            <asp:Label ID="Label2" runat="server" Text="To Date" CssClass="lblField"></asp:Label>
                        </td>
                        <td align="left" >
                            <asp:TextBox ID="txtWeekToDate" runat="server"  CssClass="txtField"
                                onKeyPress="return allowDateCharacters(event.keyCode, event.which);" 
                                ReadOnly="True"></asp:TextBox>
                                
                              
                        </td>
                    </tr>
                </table>
            </td>
                <td>
                </td>
            </tr>
            <tr>
               <td>
               <table id="tblByDay" runat="server" align="left" class="txtField">
            <tr>
            <td class="tdBold" align="right">
                <asp:Label ID="LblFromDate1" runat="server" Text="From Date" CssClass="lblField"></asp:Label>
               
            </td>
            <td align="left">
                <asp:TextBox ID="txtdayFromDate" runat="server" CssClass="txtField" 
                    onKeyPress="return allowDateCharacters(event.keyCode, event.which);" ></asp:TextBox>
                <ajax:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="Calendericon3"
                    TargetControlID="txtdayFromDate" Format="dd/MM/yyyy">
                </ajax:CalendarExtender><asp:ImageButton ID="Calendericon3" runat="server"  ImageUrl="~/Images/calender-icon.png"
                    Width="15px" />
            </td>
            <td class="tdBold" align="right">
                <asp:Label ID="Lbltodate2" runat="server" Text="To Date" CssClass="lblField"></asp:Label>
                &nbsp;:
            </td>
            <td align="left">
                <asp:TextBox ID="txtdayTodate" runat="server" CssClass="txtField" 
                    onKeyPress="return allowDateCharacters(event.keyCode, event.which);" ></asp:TextBox>
                <ajax:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="Calendericon4"
                    TargetControlID="txtdayTodate" Format="dd/MM/yyyy">
                </ajax:CalendarExtender>
                <asp:ImageButton ID="Calendericon4" runat="server"  ImageUrl="~/Images/calender-icon.png"
                    Width="15px" />
            </td>
            </tr>
            </table>
               </td>
           </tr>
            <tr>
           <td>
                <table id="tblByMonth" runat="server" align="left" class="txtField">
            <tr>
            <td class="tdBold" align="right">
                <asp:Label ID="Lblfromdate5" runat="server" Text="From Date" CssClass="lblField"></asp:Label>
                
            </td>
            <td align="left">
                <asp:TextBox ID="txtMonthFromdate" runat="server" AutoPostBack="true" OnTextChanged="txtMonthFromdate_TextChanged"
                    Format="dd/MM/yyyy" onKeyPress="return allowDateCharacters(event.keyCode, event.which);"
                    CssClass="txtField" Width="100px" ></asp:TextBox>
                <ajax:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="Calendericon5"
                    TargetControlID="txtMonthFromdate" Format="yyyy/MM/dd">
                </ajax:CalendarExtender>
            </td>
            <td>
                <asp:ImageButton ID="Calendericon5" runat="server" ImageUrl="~/Images/calender-icon.png"
                    Width="15px" />
            </td>
            <td class="tdBold" align="right">
                <asp:Label ID="LblTodate5" runat="server" Text="To Date" CssClass="lblField"></asp:Label>
               
            </td>
            <td align="left">
                <asp:TextBox ID="txtMonthTodate" runat="server" CssClass="txtField" 
                    onKeyPress="return allowDateCharacters(event.keyCode, event.which);" 
                    ReadOnly=true></asp:TextBox>
            </td>
            </tr>
            </table>
           </td>
           </tr>
            <tr>
                <td align="left">
                    <table align="left" width="40%" class="txtField">
                <tr>
                <td rowspan="4" class="style1" align="left" width="150">
           <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click"
                                    CssClass="cssButton" Width="140px"    />
                       </td>
                       <td>         
                <asp:ImageButton ImageUrl="~/Images/excel_icon.GIF" ID="btnExport" runat="server"
                    Text="Export" OnClick="BtnExport_Click" Height="16px" Width="19px" border=1px/>
          </td>
                    <td align="center" >
                        <asp:Label ID="lblResult" runat="server" Text="Label" ForeColor="Red" 
                            CssClass="cssDisplay" Width="373px" Height="16px"></asp:Label>
                    </td>
                </tr>
            </table>
                </td>
            </tr>
        </table>
    </td>
        </tr>
        <tr>
    <td>
     <table align="left" class="txtField">
            <tr>
                    <td valign="top">
                    <div style="height:100%; width:97%;">
                <asp:Panel ID="PanelGrid" runat="server" ScrollBars="Auto" margin="0px" Height="300px" HorizontalAlign="Center "  >
                        <asp:GridView ID="grdResult" runat="server" CellPadding="3" ForeColor="#333333" GridLines="Vertical" 
                            AutoGenerateColumns="False" DataKeyNames="RequestID" CssClass="txtField" >
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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        </asp:Panel>
        </div>
                    </td>
                </tr>
            </table>
    </td>[p\
    </tr>
    </table>
</asp:Content>
 <asp:Content ID="Content1" runat="server" contentplaceholderid="head">

     <style type="text/css">
         .style1
         {
             width: 351px;
         }
     </style>

</asp:Content>

 
