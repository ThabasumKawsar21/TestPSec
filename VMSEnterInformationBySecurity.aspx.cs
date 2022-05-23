<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VMSEnterInformationBySecurity.aspx.cs"
    Inherits="VMSDev.VMSEnterInformationBySecurity" MasterPageFile="~/VMSMain.Master" %>

<%@ Register Src="UserControls/NewVisitorGeneralInformation.ascx" TagName="VisitorGeneralInformation"
    TagPrefix="uc1" %>
<%@ Register Src="UserControls/VisitorLocationInformation.ascx" TagName="VisitorLocationInformation"
    TagPrefix="uc2" %>
<%@ Register Src="UserControls/EquipmentPermitted.ascx" TagName="EquipmentPermitted"
    TagPrefix="uc3" %>
<%@ Register Src="UserControls/EmergencyContactInformation.ascx" TagName="EmergencyContactInformation"
    TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="App_Themes/Jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        #overlay {
            position: fixed;
            display: none;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5);
            z-index: 2;
            cursor: pointer;
        }

        #textOverlay {
            position: absolute;
            top: 50%;
            left: 50%;
            font-size: 20px;
            color: white;
            transform: translate(-50%,-50%);
            -ms-transform: translate(-50%,-50%);
        }

        .ModalWindow {
            border: 1px solid #c0c0c0;
            background: #f0f0f0;
            padding: 0px 0px 0px 0px;
            width: 500px;
            height: auto;
        }

        .modalBackground {
            background-color: #878776;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }

        p.MsoNormal {
            margin-top: 1.3pt;
            font-family: "Arial", "sans-serif";
        }
    </style>

 
    <script src="Scripts/jquery-3.4.1.min.js"type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/common.js"></script>
    <script src="Scripts/moment.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/VisitorLocationInformation.js"></script>
   
</asp:Content>
<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder"
    runat="server">
    <script type="text/javascript">
        var validationCancel = '<%=Resources.LocalizedText.CancelConfirmation%>';
    </script>
    <script language="javascript" type="text/javascript">    

        function CloseParentWindow() {
            window.parent.location.href = window.parent.location.href;
            // return false;
        }

        function doPostBack() {
            <asp:Literal ID="litPostBack" runat="server" />

        }

        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function OpenwebcamWindow(flname) {

            //var dialogresult = window.showModalDialog(flname, "UploadPhoto", "dialogHeight:450px;dialogWidth:800px;center:yes;status=0");
            var dialogresult = window.open(flname, "UploadPhoto", "dialogHeight:450px;dialogWidth:800px;center:yes;status=0");
            //__doPostBack('PostBackFromDialog', dialogresult);
            return true;
        }
        function OpenWindow(flname) {
            var obj = new Object();
            obj.Name = "parent";
            args.doPostBack = doPostBack;
            var dialogresult = window.showModalDialog(flname, "UploadPhoto", "dialogHeight:200px;dialogWidth:425px;center:yes;status=0");
            return true;
            if (dialogresult != null) {
                __doPostBack('PostBackFromDialog', dialogresult);
            }
        }

        function GetOffsetTime() {
            var CurrentDate = new Date();
            var today = (new Date()).format('dd/MM/yyyy HH:mm:ss');
            VMSEnterInformationBySecurity.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
            VMSEnterInformationBySecurity.AssignCurrentDateTime(today);
        }

        function TimeExtendMailtrigger() {
            var r = document.getElementById('<%=btnTimeExtendHidden.ClientID %>');
            r.click();
        }

        function CancelRequestMail() {
            var r = document.getElementById('<%=btnCancelRequest.ClientID %>');
            r.click();
        }


    </script>


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc1:VisitorGeneralInformation ID="VisitorGeneralInformationControl" runat="server" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError1" ForeColor="Red" align="left" CssClass="cssDisplay">
                </asp:Label>
            </td>
        </tr>
    </table>
    <hr />
    <uc2:VisitorLocationInformation ID="VisitorLocationInformationControl" runat="server"
        OnBubbleIndexChanged="ShowEquipmentPermittedControl" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError2" ForeColor="Red" align="left" CssClass="cssDisplay">
                </asp:Label>
            </td>
        </tr>
    </table>

    <asp:UpdatePanel ID="EquipmentControl" runat="server">
        <ContentTemplate>
            <hr id="hrEquipmentControl" runat="server" visible="false" />
            <table align="center" width="98%">
                <tr align="center" style="height: 5px">
                    <td>
                        <asp:HiddenField ID="BatchNo" runat="server" />
                    </td>
                    <td width="60"></td>
                    <td align="left">
                        <asp:Label runat="server" ID="lblError" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
                        <asp:CustomValidator runat="server" ID="custError"></asp:CustomValidator>
                    </td>
                    <td align="center">
                        <asp:Label runat="server" ID="lblSubmitSuccess" ForeColor="Green" align="left" CssClass="cssDisplay"></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" CssClass="cssDisplay" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <uc3:EquipmentPermitted ID="EquipmentPermittedControl" runat="server" Visible="false" />

            <asp:Panel ID="pnlCardDetails" runat="server">
                <div style="text-align: left;">
                    <asp:Label ID="lblCardInfo" runat="server" CssClass="lblHeada" Text="<%$ Resources:LocalizedText, CardInformation %>">
                    </asp:Label>
                </div>

                <table>
                    <tr id="rowChekinRadio" runat="server" style="text-align: left">
                        <td colspan="4" class="lblField">Do you want to check in this visitor now ?<asp:RadioButton ID="rbYes" runat="server" GroupName="checkin" Text="Yes" />
                            <asp:RadioButton ID="rbNo" runat="server" GroupName="checkin" Text="No" />
                        </td>

                        <td style="width: 150px;">&nbsp;</td>

                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lblVCardEntry" runat="server" CssClass="lblField" Text="Enter VCard number" ToolTip="Please enter the card that is registered in visitor card inventory of Access card application."></asp:Label>

                            <asp:Label ID="reqVCardSpn" runat="server" Text="*" Style="color: red"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVCard" runat="server" BorderStyle="Solid" BorderWidth="1px" MaxLength="7" Style="text-transform: uppercase" Width="80"></asp:TextBox>
                        </td>
                        <td style="width: 50px; text-align: left;">
                            <img src="Images/help.png" alt="" style="width: 20px; height: 16px;" title="Access card number will be prepopulated automatically based on entered Vcard number. In case of incorrect access card number, please have that rectified in Access card – Visitor card inventory" />
                        </td>
                        <td style="width: 125px;">
                            <asp:Label ID="lbl_AccessCard" runat="server" CssClass="lblField"></asp:Label>
                        </td>
                        <td style="width: 150px;">
                            <asp:Label ID="lblAccessCard" runat="server" ForeColor="#cc3300" Style="margin-left: 10px;"></asp:Label>
                        </td>
                    </tr>

                </table>
                <div>
                    <span id="vcardError" class="errorLabel"></span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError3" ForeColor="Red" align="left" CssClass="cssDisplay">
                </asp:Label>

            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError4" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label></td>
        </tr>
    </table>
    <br />
    <table align="right" runat="server" id="tblButtons">
        <tr align="center">

            <td align="center">
                <asp:Button runat="server" ID="Back" Text="<%$ Resources:LocalizedText, Back %>"
                    CssClass="cssButton" CausesValidation="false" OnClick="Back_Click" />
            </td>
            <td align="center">
                <asp:Panel ID="pnlImageUpload" runat="server">
                    <asp:Button runat="server" ID="btnCheckin" Text="Check-in" CausesValidation="true" OnClientClick="return ValidateVcard('checkin')" CssClass="cssButton" OnClick="btnCheckin_Click1" />
                </asp:Panel>
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Submit" name="Submit" OnClientClick="return ValidateVcard('submit')" Text="<%$ Resources:LocalizedText, Submit %>"
                    OnClick="Submit_Click" CausesValidation="true" CssClass="cssButton" />
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Save" name="Save" Text="<%$ Resources:LocalizedText, Save %>"
                    OnClick="Save_Click" Visible="false" CausesValidation="false" CssClass="cssButton" />
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Reset" name="Reset" Text="<%$ Resources:LocalizedText, Reset %>"
                    OnClick="Reset_Click" ValidationGroup="Reset" CssClass="cssButton" />
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Cancel" name="Cancel" Text="<%$ Resources:LocalizedText, Cancel %>"
                    OnClick="Cancel_Click" ValidationGroup="Cancel" CssClass="cssButton" OnClientClick="javascript:return confirm(validationCancel);" />
            </td>

        </tr>
    </table>
    <asp:HiddenField ID="hdnVisitDetailsID" runat="server" />
    <asp:HiddenField ID="hdnAccessCard" runat="server" />
    <asp:Button ID="btnTimeExtendHidden" runat="server" Text="Button" Style="display: none"
        OnClick="BtnTimeExtendHidden_Click" />
    <asp:Button ID="btnCancelRequest" runat="server" Text="Button" Style="display: none"
        OnClick="BtnCancelRequest_Click" />
    <br />
    <div id="overlay">
        <div id="textOverlay">Fetching Access Card details...</div>
    </div>
    <asp:HiddenField ID="hdnSecurityFacility" runat="server" />
     <script type="text/javascript" src="Scripts/VMSEnterInformationBySecurity.js"></script>
</asp:Content>
