<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    CodeBehind="ApproveImage.aspx.cs" Inherits="VMSDev.ApproveImage" %>

<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.DynamicData" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .gridText
        {
            font-family: Arial;
            font-size: 12px;
            font-style: normal;
            font-weight: normal;
            font-variant: normal;
            color: #736F6E;
            text-decoration: none;
        }
        .cssPager td
        {
            padding-left: 4px;
            padding-right: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <table width="100%">
        <tr>
            <td width="100%" align="left" class="table_header_bg" style="height: 25px">
                <table width="100%">
                    <tr>
                        <td>
                            &nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header" Text="Approve Image Upload Requests"></asp:Label>
                            <asp:Label ID="lblUploadFailure" runat="server" CssClass="Error_Font" ></asp:Label>
                        </td>
                        <td width="100px" valign="bottom" align="right">
                            <span style="color: black; font-family: Arial,Verdana, Helvetica, sans-serif; font-weight: bold;
                                font-size: 9pt; vertical-align: bottom">Legend &nbsp;:</span>
                        </td>
                        <td width="100px" valign="bottom" align="center">
                            <asp:Image ID="imgApprove" ImageUrl="Images/approve.png" Height="15px" runat="server"
                                oncontextmenu="return false" />
                            &nbsp; <span style="color: black; font-family: Arial,Verdana, Helvetica, sans-serif;
                                font-size: 10pt; vertical-align: bottom">Approve</span>
                        </td>
                        <td width="100px" valign="bottom" align="left">
                            <asp:Image ID="Image1" ImageUrl="Images/Clear.png" Height="15px" runat="server" oncontextmenu="return false" />
                            &nbsp; <span style="color: black; font-family: Arial,Verdana, Helvetica, sans-serif;
                                font-size: 10pt; vertical-align: bottom">Reject</span>
                        </td>
                        <td width="30px">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ScriptManager ID="DefaultMasterScriptManager" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdImageChangeRequests" DataKeyNames="RequestID" runat="server"
                            AllowPaging="True" AutoGenerateColumns="False" AllowSorting="true" OnRowDataBound="GrdRowDataBound" 
                            CssClass="GridText" EmptyDataText="No Records Found" HorizontalAlign="Center" 
                            PagerStyle-HorizontalAlign="Center" PageSize="10" Width="100%" OnRowCommand="GrdRowCommand"
                            OnSorting="GrdSorting" OnPageIndexChanging="GrdImageChangeRequests_PageIndexChanging">
                            <RowStyle CssClass="gridText" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#C6D4BB" ForeColor="#736F6E" CssClass="gridText" Height="25px"
                                HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate >
                                        <asp:Label Text='<%#Eval("RequestID") %>' ID="lblReqID" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AssociateID" SortExpression="AssociateID">
                                    <ItemTemplate>
                                        <asp:Label CssClass="gridText" Text='<%#Eval("AssociateID") %>' ID="lblAssociateID"
                                            runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Associate Name" SortExpression="AssociateName">
                                    <ItemTemplate>
                                        <asp:Label CssClass="gridText" Text='<%#Eval("AssociateName") %>' ID="lblAssociateName"
                                            Style="padding: 0 0 0 20px" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Uploaded On" SortExpression="UploadedOn">
                                    <ItemTemplate>
                                        <asp:Label CssClass="gridText" Text='<%#Eval("UploadedOn") %>' ID="lblUploadedOn"
                                            runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Old Photo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:Image ID="imgOldSmall" runat="server" ImageUrl='' Height="50px" Width="50px" oncontextmenu="return false" />
                                           <ajaxToolkit:HoverMenuExtender ID="hme0" runat="Server" TargetControlID="imgOldSmall"
                                            PopupControlID="pnlOldBig" PopupPosition="Bottom" OffsetX="0" OffsetY="0" PopDelay="10" />
                                        <ajaxToolkit:DropShadowExtender ID="dse0" runat="server" TargetControlID="pnlOldBig"
                                            Opacity="0.75" TrackPosition="true" />
                                        <asp:Panel ID="pnlOldBig" runat="server">
                                            <asp:Image ID="imgOldBig" runat="server" ImageUrl='' Height="200px"  oncontextmenu="return false" />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New Photo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:Image ID="imgNewSmall" runat="server" ImageUrl='' Height="50px"  Width="50px" oncontextmenu="return false" />
                                          <ajaxToolkit:HoverMenuExtender ID="hme1" runat="Server" TargetControlID="imgNewSmall"
                                            PopupControlID="pnlNewBig" PopupPosition="Bottom" OffsetX="0" OffsetY="0" PopDelay="10" />
                                        <ajaxToolkit:DropShadowExtender ID="dse1" runat="server" TargetControlID="pnlNewBig"
                                            Opacity="0.75" TrackPosition="true" />
                                        <asp:Panel ID="pnlNewBig" runat="server">
                                            <asp:Image ID="imgNewBig" runat="server" ImageUrl='' Height="200px"  oncontextmenu="return false"/>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comments" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlComments" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DdlCommentsSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnApprove" />
                                                <asp:PostBackTrigger ControlID="btnReject" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="45%" align="right">
                                                            <asp:ImageButton ID="btnApprove" CommandName="APPROVE" ImageUrl="Images/approve.png"
                                                                oncontextmenu="return false" ToolTip="Approve" Height="35px" runat="server" />
                                                        </td>
                                                        <td width="10%">
                                                        </td>
                                                        <td width="45%" align="left">
                                                            <asp:ImageButton ID="btnReject" CommandName="REJECT" CausesValidation="false" ImageUrl="Images/Clear.png"
                                                                oncontextmenu="return false" ToolTip="Reject" Height="35px" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="10" />
                            <PagerStyle CssClass="cssPager" BackColor="#EDF6E3" Height="25px" VerticalAlign="Bottom"
                                HorizontalAlign="Right" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:SqlDataSource ID="SQLDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IVSConnectionString %>"
                    SelectCommand="IVS_GetImageChangeRequests" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
               <%-- <asp:SqlDataSource ID="SqlReasonsData" runat="server" ConnectionString="<%$ ConnectionStrings:IVSConnectionString %>"
                    SelectCommand="SELECT [LookUpID], [LookUpType], [DisplayOrder], [LookUpValue] FROM [tblLookUpData] WHERE ([LookUpType] = @LookUpType) ORDER BY [DisplayOrder]">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="ApproverReason" Name="LookUpType" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
                <%-- <asp:ObjectDataSource ID="odsImageRequests" runat="server" SelectMethod="GetAllImageChangeRequests"
                    EnablePaging="True" TypeName="VMSBusinessLayer.EmployeeBL" StartRowIndexParameterName="startIndex"
                    MaximumRowsParameterName="pageSize" SortParameterName="sortBy"></asp:ObjectDataSource>--%>
            </td>
        </tr>
    </table>
</asp:Content>
