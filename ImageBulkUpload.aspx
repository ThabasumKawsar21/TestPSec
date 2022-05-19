<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    CodeBehind="ImageBulkUpload.aspx.cs" Inherits="VMSDev.ImageBulkUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="App_Themes/BulkFileUploadStyle.css" rel="stylesheet" type="text/css" />
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function showAlert(x) {
        $('#<%=hdnBulkUploadID.ClientID %>').val(x);
            var r = document.getElementById('<%=btnGetImages.ClientID %>');
            r.click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <!-- outer wrapper starts-->
    <div class="wrapper">
        <asp:ScriptManager ID="sm1" runat="server">
        </asp:ScriptManager>
        <!-- header starts-->
        <div class="header">
            <!-- header ends-->
            <!-- main content starts-->
            <div class="mainContent">
                <!-- silverlight control goes here-->
                <div class="uploadField" id="silverlightControlHost">
                    <object id="slFileUpload" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
                        width="352px" height="35">
                        <param name="FileType" value='<% %>' />
                        <param name="source" value="ClientBin/BulkUpload.xap" />
                        <param name="onError" value="onSilverlightError" />
                        <param name="background" value="white" />
                        <param name="minRuntimeVersion" value="4.0.50826.0" />
                        <param name="autoUpgrade" value="true" />
                        <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0" style="text-decoration: none">
                            <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                                style="border-style: none" />
                        </a>
                    </object>
                    <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
                        border: 0px"></iframe>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgExcelIcon" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnTotalValue" Value="0" runat="server" />
                        <asp:Button ID="btnGetImages" runat="server" Text="Get Images" OnClick="BtnGetImages_Click"
                            Style="display: none" />
                        <asp:Panel CssClass="divMessage" ID="pnlMessage" runat="server">
                            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
                        </asp:Panel>
                        <asp:Panel CssClass="divView" ID="pnlViewBtns" runat="server">
                            <asp:ImageButton ID="imgExcelIcon" Height="20px" runat="server" ImageUrl="~/Images/excel_icon.GIF" ToolTip="Export to Excel"
                                OnClick="ImgExcelIcon_Click" Style="margin-top: 19px" />
                            <asp:ImageButton ID="imgGridView" Height="25px" runat="server" ImageUrl="~/Images/GridView.png"  ToolTip="Grid View"
                                OnClick="ImgGridView_Click" />
                            <asp:ImageButton ID="imgIconView" runat="server" Height="25px" ImageUrl="~/Images/IconView.png"  ToolTip="Tile View"
                                OnClick="ImgIconView_Click"/>
                        </asp:Panel>
                        <asp:Panel ID="pnlListView" CssClass="thumbContainer" runat="server">
                            <asp:HiddenField ID="hdnStartValue" runat="server" />
                            <asp:ImageButton ID="lnkPrevious" ImageUrl="~/Images/prevDataList.png" runat="server"
                                OnClick="LnkPrevious_Click" Style="height: 427px; width: 50px; display: inline;
                                float: left; position: absolute; left: 35px; top: 110px;" />
                            <asp:DataList ID="lstImages" runat="server" RepeatDirection="Horizontal" RepeatColumns="7"
                                OnItemDataBound="LstImages_ItemDataBound">
                                <ItemTemplate>
                                    <div class="divImage" style="display: block; vertical-align: middle">
                                        <asp:Panel ID="divHoverMessage" runat="server" CssClass="divHover" Style="display: none;
                                            background-color: #C5C5C5; min-height: 20px; position: absolute; left: 10px;
                                            width: 100px; top: 1px;">
                                            <asp:Label ID="lblMessage" CssClass="field_txt" runat="server" Text='<%#Eval("StatusMessage") %>'
                                                Font-Size="09" Style="margin-left: 20px"></asp:Label>
                                        </asp:Panel>
                                        <asp:Image ID="imgStatus" runat="server" ImageUrl="~/Images/Clear.png" Style="position: absolute;
                                            left: 5px; width: 20px; top: 0px;" />
                                        <ajaxToolkit:HoverMenuExtender runat="server" ID="hovExtender" TargetControlID="imgStatus"
                                            PopupControlID="divHoverMessage" PopDelay="1">
                                        </ajaxToolkit:HoverMenuExtender>
                                        <center>
                                            <asp:Image ImageAlign="Middle" ID="imgFile" runat="server" ImageUrl="~/Images/DummyPhoto.png"
                                                Style="max-height: 100px; max-width: 100px" /></center>
                                        <div style="position: absolute; display: block; left: 10px; top: 110px;">
                                            <asp:Label ID="lblFileName" runat="server" CssClass="field_txt" Text='<%#Eval("FileName") %>'
                                                Font-Size="11" Style="background-color: #ADC2EB"></asp:Label>
                                            <asp:Label ID="lblMessageID" runat="server" CssClass="field_txt" Text='<%#Eval("MessageID") %>'
                                                Font-Size="X-Small" Style="display: none"></asp:Label>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:ImageButton ID="lnkNext" ImageUrl="~/Images/nextDataList.png" runat="server"
                                OnClick="LnkNext_Click" Style="height: 427px; width: 50px; display: inline; float: right;
                                position: absolute; left: 940px; top: 110px;" />
                            <asp:HiddenField ID="hdnLastValue" runat="server" />
                        </asp:Panel>
                        <asp:Panel ID="pnlGridView" CssClass="thumbContainer" runat="server">
                            <asp:SqlDataSource ID="sdsUploadDetails" runat="server" ConnectionString="<%$ ConnectionStrings:IVSConnectionString %>"
                                SelectCommand="IVS_GetBulkUploadDetails" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnBulkUploadID" Name="BulkUploadID" PropertyName="Value"
                                        Type="Int64" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:GridView ID="grdUploadDetails" runat="server" CellPadding="4" ForeColor="#333333"
                                CssClass="GridText" EnableViewState="false" AutoGenerateColumns="false" PageSize="15"
                                OnSorting="GrdSorting" AllowPaging="true" EmptyDataText="No Records Found" HorizontalAlign="Center"
                                OnPageIndexChanging="GrdUploadDetails_PageIndexChanging" AllowSorting="true"
                                Width="100%" PagerStyle-HorizontalAlign="Center" GridLines="Vertical" OnRowDataBound="GrdUploadDetails_RowDataBound">
                                <AlternatingRowStyle BackColor="White" Height="25px" />
                                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle CssClass="header" BackColor="#990000" Font-Bold="True" ForeColor="White"
                                    Height="29px" />
                                <PagerStyle BackColor="#FFCC66" Height="25px" ForeColor="#333333" HorizontalAlign="Center" />
                                <RowStyle BackColor="#FFFBD6" Height="25px" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Successful Upload">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSuccess" runat="server" Checked='<%#Eval("IsSuccess") %>' Enabled="false" />
                                            <%--<asp:Label Text='<%#Eval("MessageID") %>' ID="lblReqID" runat="server" />--%>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="20%"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FileName" SortExpression="FileName">
                                        <ItemTemplate>
                                            <asp:Label CssClass="gridText" Text='<%#Eval("FileName") %>' ID="FileName" runat="server" Style="padding: 0 0 0 20px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                        <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Message" SortExpression="StatusMessage">
                                        <ItemTemplate>
                                            <asp:Label CssClass="gridText" Text='<%#Eval("StatusMessage") %>' ID="StatusMessage"
                                                Style="padding: 0 0 0 20px" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                        <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:HiddenField ID="hdnBulkUploadID" Value="0" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:HiddenField ID="hdnLoggedInUser" runat="server" />
        </div>
    </div>
</asp:Content>
