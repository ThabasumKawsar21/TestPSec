<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    CodeBehind="ApplicantImageUpload.aspx.cs" Inherits="VMSDev.ApplicantImageUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }      
    </script>
    <script language="javascript" type="text/javascript">

        function LoadImage() {
            var image = document.getElementById('VMSContentPlaceHolder_UImage');
            image.src = document.getElementById('VMSContentPlaceHolder_ImgUpload').value
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
        
         <%-- Delete/viewphoto--%>

        function show_confirm() {
            var r = confirm("Are you sure , want to delete the photo ?");
            if (r == true) {
//                 
                document.getElementById('VMSContentPlaceHolder_myHiddenField').value="yes";       
            }
            else {
                document.getElementById('VMSContentPlaceHolder_myHiddenField').value = "no";   
            }
        }



    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="center" valign="middle" style="height: 100%">
                <table width="100%" class="border" style="height: 98%">
                    <tr>
                        <td align="left" class="table_header_bg" style="height: 19px" colspan="2">
                            <asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header" Text="Capture\Upload Photograph"></asp:Label><asp:HiddenField
                                ID="HiddenImage" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="4" align="center" valign="middle" style="height: 100%">
                                        <asp:Panel ID="panelEmp" runat="server" Width="100%" CssClass="border">
                                            <table id="tblEmp" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="right" class="field_column_bg" style="width: 20%">
                                                        <asp:Label ID="lblEmployeeID" runat="server" CssClass="field_txt" Text="Applicant ID * "></asp:Label>:
                                                    </td>
                                                    <td align="left" class="field_column_bg">
                                                        &nbsp;
                                                        <asp:TextBox ID="txtApplicantIDCapture" runat="server" MaxLength="16" CssClass="txtField"
                                                            onkeypress="return isNumberKey(event)" Width="166px" ValidationGroup="ApplicantDetails"></asp:TextBox>
                                                        <%-- Delete/viewphoto--%>
                                                        &nbsp;<%-- Delete/viewphoto--%><%-- Delete/viewphoto--%><asp:ImageButton ID="imgbtnSearchImage"
                                                            runat="server" Height="20px" ImageUrl="~/Images/Search_1.png" OnClick="ImageButton1_Click1" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="lblApplicantIDError" runat="server" CssClass="field_txt" Font-Bold="True"
                                                            ForeColor="Red" Text="Please Enter a valid Applicant ID" Visible="false"></asp:Label>
                                                    </td>
                                                    <td align="left" rowspan="5" style="width: 40%" valign="middle">
                                                        <table id="tblImage" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td valign="middle" style="white-space: nowrap; padding-left:50px;">
                                                                    <asp:Image ID="UImage" runat="server" Height="140px" Visible="true" ImageUrl="~/Images/char.jpeg"
                                                                        oncontextmenu="return false" />
                                                                    <%-- Delete/viewphoto--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="field_column_bg">
                                                        <asp:Label ID="Label7" runat="server" CssClass="field_txt" Text="Location :  "></asp:Label>
                                                    </td>
                                                    <td align="left" class="field_column_bg">
                                                        &nbsp;&nbsp;
                                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="txtField" Enabled="False"
                                                            ValidationGroup="ApplicantDetails" Width="220px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblMandatory" runat="server" CssClass="field_txt" ForeColor="Black"
                                                            Text="* Indicates Mandatory Fields    "></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="field_column_bg" style="width: 20%">
                                                        <asp:Label ID="lblEmployeeImage" runat="server" CssClass="field_txt" Text="Applicant Image  "></asp:Label>:
                                                    </td>
                                                    <td align="left" class="field_column_bg">
                                                        &nbsp;
                                                        <asp:FileUpload ID="ImgUpload" runat="server" Width="220px" onkeypress="return false;"
                                                            onpaste="return false;" onkeydown="return false;" onmousedown="return false;"
                                                            onchange="LoadImage();" CssClass="txtField" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 20%">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblImage" runat="server" CssClass="field_txt" Font-Bold="False" ForeColor="Red"></asp:Label>
                                                        &nbsp;<asp:Label ID="lblMessage" runat="server" CssClass="field_txt" Font-Bold="True"
                                                            ForeColor="Red" Text="WASSUP" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="field_column_bg">
                                                    </td>
                                                    <td align="left" class="field_column_bg">
                                                        &nbsp;&nbsp;
                                                        <asp:Button ID="btnSave" runat="server" BackColor="#767561" Font-Bold="False" ForeColor="White"
                                                            OnClick="btnSave_Click" Text="Upload" Font-Size="10px" Height="21px" Width="71px" />
                                                        <asp:Button ID="Button3" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                                            ForeColor="White" OnClick="BtnClear_Click" Text="Clear" Height="21px" Width="71px" />
                                                        <input type="button" onclick=" return window.OpenwebcamWindow('webcam.aspx')" value="Web Cam"
                                                            style="color: White; font-size: 10px; background-color: #767561;" />
                                                    </td>
                                                </tr>
                                            </table>
                                                       <br />
                                        </asp:Panel>
                             
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <%--<asp:UpdatePanel ID="upanel" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                    <asp:AsyncPostBackTrigger ControlID="btnClear" />
                                </Triggers>
                                <ContentTemplate>
                              
                                    <table cellpadding="0" cellspacing="0" width="100%" style="background-color: Black">
                                        <tr>
                                            <td align="right" style="width: 15%; padding-right: 5px;" valign="bottom">
                                                <asp:Label ID="lblApplicantID" runat="server" CssClass="field_txt" ForeColor="White"
                                                    Text="Applicant ID : * "></asp:Label>
                                            </td>
                                            <td align="left" style="height: 30px" valign="bottom">
                                                
                                            </td>
                                            <td style="width: 10%; height: 30px; vertical-align: bottom; text-align: left; padding-left: 2px">
                                                &nbsp;</td>
                                            <td align="left" rowspan="7">
                                                <object id="SL" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
                                                    width="800px" style="height: 339px">
                                                 
                                                    <param name="source" value="ClientBin/SilverlightApplication1.xap" />
                                                    <param name="onError" value="onSilverlightError" />
                                                    <param name="background" value="white" />
                                                    <param name="minRuntimeVersion" value="4.0.50401.0" />
                                                    <param name="autoUpgrade" value="true" />
                                                    <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration: none">
                                                        <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                                                            style="border-style: none" />
                                                    </a>
                                                </object>
                                                <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
                                                    border: 0px"></iframe>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-right: 5px">
                                            </td>
                                            <td align="left" valign="middle" colspan="2" style="padding: 1px 1px 1px 0px">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top" style="padding-right: 5px" class="style1">
                                                &nbsp;</td>
                                            <td align="left" valign="top" class="style1">
                                                &nbsp;</td>
                                            <td class="style1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" valign="top" align="right" style="padding: 5px 20px 1px 10px">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="height: 50px">
                                            </td>
                                            <td align="center" valign="top">
                                                <asp:Button ID="btnSave" runat="server" BackColor="#767561" Font-Bold="False" ForeColor="White"
                                                    OnClick="btnSave_Click" Text="Upload" Font-Size="10px" Height="20px" Width="70px"
                                                    ValidationGroup="ApplicantDetails" />&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnClear" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                                                    ForeColor="White" OnClick="btnClear_Click" Text="Clear" Height="20px" Width="70px" />
                                                &nbsp;
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="height: 50px">
                                            </td>
                                            <td align="center" valign="top">
                                                <asp:FileUpload ID="ImgUpload" runat="server" onkeydown="return false;" onkeypress="return false;"
                                                    onmousedown="return false;" onpaste="return false;" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Button1" runat="server" BackColor="#767561" Font-Bold="False" ForeColor="White"
                                                    OnClick="btnUpload_Click" Text="Upload" Font-Size="10px" Height="20px" Width="70px"
                                                    ValidationGroup="ApplicantDetails" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                              
                                            </td>
                                        </tr>
                                    </table>
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
    </asp:Content>
