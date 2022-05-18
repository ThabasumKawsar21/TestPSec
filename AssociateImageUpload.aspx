<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssociateImageUpload.aspx.cs"
    Inherits="VMSDev.AssociateImageUpload" MasterPageFile="~/VMSMain.Master" %>

<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder"
    runat="server">
    <script language="javascript" type="text/javascript">

        //        function LoadImage() {
        //            var image = document.getElementById('<%=UImage.ClientID %>');
        //            image.src = document.getElementById('<%=ImgUpload.ClientID %>').value
        //        }

        function showPreview() {
            // 
            var cam = document.getElementById('<%=btnCam.ClientID %>');
            cam.setAttribute("disabled", "true");
            var save = document.getElementById('<%=btnSave.ClientID %>');
            save.attributes.removeNamedItem("disabled");
        }

        function OpenwebcamWindow(flname) {
            var dialogresult = window.showModalDialog(flname, "UploadPhoto", "dialogHeight:450px;dialogWidth:800px;center:yes;status=0");
            //       if ( dialogresult != null )
            //          {
            __doPostBack('PostBackFromDialog', dialogresult);
            //         }
            return true;
        }

        function RefreshPage() {
            window.document.forms(0).submit();
        }
        function setClipBoardData() {
            setInterval("window.clipboardData.setData('text','')", 20);
        }
        function blockError() {
            window.location.reload(true);
            return true;
        }

        function SpecialCharacterValidation(e) {
            var keycode;
            if (window.event)
                keycode = window.event.keyCode;
            else if (event) keycode = event.keyCode;
            else if (e) keycode = e.which;
            else return true;
            if (keycode > 47 && keycode <= 57) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="4" align="center" valign="middle" style="height: 100%">
                <table width="100%" class="border" style="height: 98%; table-layout: fixed">
                    <tr>
                        <td colspan="2" align="left" class="table_header_bg" style="height: 19px">
                            &nbsp;&nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header"
                                Text="Upload Photograph"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:ScriptManager ID="scriptManager1" runat="server">
                            </asp:ScriptManager>
                            <asp:UpdatePanel ID="upnlAssociateImageUpload" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Panel ID="panelEmp" runat="server" Width="100%" CssClass="border">
                                        <table id="tblEmp" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="right" class="field_column_bg" style="width: 20%">
                                                    <asp:Label ID="lblEmployeeID" runat="server" CssClass="field_txt" Text="Associate ID "></asp:Label>:
                                                </td>
                                                <td align="left" class="field_column_bg" style="padding: 0px 0px 0px 10px">
                                                    <asp:TextBox ID="txtAssociateID" runat="server" CssClass="txtField"
                                                        onkeypress="javascript:return SpecialCharacterValidation(this);" ValidationGroup="1" MaxLength="15"
                                                        Width="213px"></asp:TextBox>
                                                    <%-- Delete/viewphoto--%>
                                                    <asp:Button ID="btnView" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                                        ValidationGroup="1" ForeColor="White" Height="21px" OnClick="BtnView_Click" Text="View photo"
                                                        Width="71px" />
                                                    &nbsp;<asp:RequiredFieldValidator ID="rfvAssociateID" runat="server" CssClass="field_txt"
                                                        ErrorMessage="* Please enter an associate ID" ValidationGroup="1" ControlToValidate="txtAssociateID"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" rowspan="4" style="width: 40%" valign="middle">
                                                    <table id="tblImage" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td valign="middle" style="white-space: nowrap; vertical-align: middle">
                                                                <div style="width: 205px">
                                                                    <asp:Image ID="UImage" runat="server" Visible="true" ImageUrl="~/Images/char.jpeg"
                                                                        Height="150px" Style="max-width: 200px" oncontextmenu="return false" />
                                                                </div>
                                                                <%-- Delete/viewphoto--%>
                                                                <%--<div style="width:205px">
                                                          <center><asp:Button ID="btnDelete" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                                            ForeColor="White" Height="21px" Text="Delete photo if any mismatch" OnClick="btnDelete_Click"
                                                            Visible="false" OnClientClick="show_confirm()" /></center> 
                                                            </div>  --%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="field_column_bg" style="width: 20%">
                                                    <asp:Label ID="lblEmployeeImage" runat="server" CssClass="field_txt" Text="Associate Image "></asp:Label>:
                                                </td>
                                                <td align="left" class="field_column_bg" style="padding: 0px 0px 0px 10px">
                                                    <asp:FileUpload ID="ImgUpload" runat="server" Width="291px" onkeypress="return false;"
                                                        EnableViewState="true" onpaste="return false;" onkeydown="return false;" onmousedown="return false;"
                                                        onchange="javascript:showPreview();" CssClass="txtField" />
                                                    <%--  <asp:Button ID="btnHidden" runat="server" Style="display: none" Text="Button" OnClick="btnHidden_Click" />
                                                    --%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="field_column_bg" style="width: 20%">
                                                </td>
                                                <td align="left" class="field_column_bg" style="padding: 0px 0px 0px 10px">
                                                    <asp:Label ID="lblImage" runat="server" CssClass="field_txt" Font-Bold="False" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style1">
                                                </td>
                                                <td align="left" class="style2" style="padding: 0px 0px 0px 10px">
                                                    <asp:Button ID="btnSave" runat="server" BackColor="#767561" Font-Bold="False" ForeColor="White"
                                                        ToolTip="Upload Image" OnClick="BtnSave_Click" Text="Upload" Font-Size="10px"
                                                        Enabled="false" Height="21px" Width="60px" />
                                                    &nbsp;<asp:Button ID="btnClear" runat="server" BackColor="#767561" Font-Bold="False"
                                                        Font-Size="10px" ToolTip="Clear Fields" ForeColor="White" OnClick="BtnClear_Click"
                                                        Text="Clear" Height="21px" Width="60px" />
                                                    &nbsp;<input type="button" onclick=" return window.OpenwebcamWindow('webcam.aspx')"
                                                        runat="server" id="btnCam" value="Web Cam" style="color: White; font-size: 10px;
                                                        background-color: #767561; width: 70px" />&nbsp;<asp:Button ID="btnDelete" runat="server"
                                                            BackColor="#767561" Font-Bold="False" Font-Size="10px" ForeColor="White" Height="21px"
                                                            Text="Delete photo" OnClick="BtnDelete_Click" ToolTip="Delete photo if any mismatch"
                                                            Visible="false" OnClientClick="javascript:return confirm('Are you sure, you want to delete the photo?');"
                                                            Width="80px" />
                                                    <asp:HiddenField ID="hdnFileID" runat="server" />
                                                   
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <table border="0" cellpadding="0" cellspacing="0" width="98%">
                                        <tr>
                                            <td colspan="6" align="center" valign="middle">
                                                <br />
                                                <table border="0" id="errortbl" cellpadding="0" cellspacing="0" runat="server" style="border-right: black 1pt solid;
                                                    border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid;
                                                    width: 60%;">
                                                    <tr>
                                                        <td colspan="6" align="left" style="background-color: #edf6fd; width: 60%">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblMessage" runat="server" CssClass="field_txt" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            width: 20%;
            height: 41px;
            background-color: #ECF7FD;
        }
        .style2
        {
            height: 41px;
            background-color: #ECF7FD;
        }
    </style>
</asp:Content>
