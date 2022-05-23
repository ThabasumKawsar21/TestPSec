<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldVMSEnterInformationBySecurity.aspx.cs"
    Inherits="VMSDev.OldVMSEnterInformationBySecurity" MasterPageFile="~/VMSMain.Master" %>

<%@ Register Src="~/OldUserControls/OldNewVisitorGeneralInformation.ascx" TagName="VisitorGeneralInformation"
    TagPrefix="uc1" %>
<%@ Register Src="~/OldUserControls/OldVisitorLocationInformation.ascx" TagName="VisitorLocationInformation"
    TagPrefix="uc2" %>
<%@ Register Src="~/OldUserControls/OldEquipmentPermitted.ascx" TagName="EquipmentPermitted"
    TagPrefix="uc3" %>
<%@ Register Src="~/OldUserControls/OldEmergencyContactInformation.ascx" TagName="EmergencyContactInformation"
    TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="App_Themes/Jquery-ui.css" rel="stylesheet" />

    <style type="text/css">
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
            margin-right: 5.75pt;
            margin-bottom: 12.0pt;
            margin-left: 0in;
            line-height: 12.0pt;
            font-size: 10.0pt;
            font-family: "Arial", "sans-serif";
        }
    </style>
</asp:Content>

<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder"
    runat="server">
   
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/moment.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/common.js"></script>
    <script type="text/javascript" src="Scripts/VisitorLocationInformation.js"></script>
    <script type="text/javascript" src="Scripts/VMSEnterInformationBySecurity.js"></script>

    <script type="text/javascript">
        var validationCancel = '<%=Resources.LocalizedText.CancelConfirmation%>';
    </script>
    <script language="javascript" type="text/javascript">    

        //jQuery(function() {
        //    debugger;
        //    var dlg = jQuery("#Popup").dialog({
        //        draggable: true,
        //        resizable: true,
        //        show: 'Transfer',
        //        hide: 'Transfer',
        //        width: 320,
        //        autoOpen: false,
        //        minHeight: 10,
        //        minwidth: 10
        //    });
        //    dlg.parent().appendTo(jQuery("form:first"));
        //});
        function CloseParentWindow() {
            window.parent.location.href = window.parent.location.href;
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
            <asp:Label runat="server" ID="lblError4" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>


    <%--<uc4:EmergencyContactInformation ID="EmergencyContactInformationControl" runat="server" />--%>
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError3" ForeColor="Red" align="left" CssClass="cssDisplay">
                </asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table align="right" runat="server" id="tblButtons">
        <tr align="center">
            <td align="center">
                <asp:UpdatePanel ID="UPIDProof" runat="server" Visible="false">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnIdProof" Name="Save" Text="UploadIDProof" OnClientClick="return OpenWindow('ImageUpload.aspx?strImage=Proof')"
                            ValidationGroup="Save" CssClass="cssButton" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Back" Text="<%$ Resources:LocalizedText, Back %>"
                    CssClass="cssButton" CausesValidation="false" OnClick="Back_Click" />
            </td>
            <td align="center">
                <input type="button" onclick="return OpenwebcamWindow('webcam.aspx')" value="<%$ Resources:LocalizedText, WebCam %>"
                    runat="server" id="btnWebcam" class="cssButton" />
                <asp:HiddenField ID="hdnVisitDetailsID" runat="server" />

            </td>
            <td align="center">
                <asp:Button runat="server" ID="GenerateBadge" Text="<%$ Resources:LocalizedText, GenerateBadgeRequest %>"
                    CssClass="cssButton" CausesValidation="false" OnClick="BtnCheckIn_Click" />
            </td>
            <td align="center">
                <asp:Panel ID="pnlImageUpload" runat="server">

                    <asp:Button runat="server" ID="btnUpload" Name="Save"
                        Text="<%$ Resources:LocalizedText, UploadPhoto %>" CssClass="cssButton" />

                </asp:Panel>

            </td>

            <td align="center">
                <asp:Button runat="server" ID="Submit" name="Submit" Text="<%$ Resources:LocalizedText, Submit %>"
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

    <%-- ECM related hidden fields --%>
    <%--<asp:HiddenField ID="_ecmMClientHdnField" runat="server" />
    <asp:HiddenField ID="FileName" runat="server" />
    <asp:HiddenField ID="ecmMHiddenField" runat="server" />
    <asp:HiddenField ID="_ecmContentIdList" runat="server" />
    <asp:HiddenField ID="originalFileName" runat="server" />
    <asp:HiddenField ID="FileContentId" runat="server" />
    <asp:HiddenField ID="FileAppDocId" runat="server" />
    <asp:HiddenField ID="EcmDid" runat="server" />--%>

    <asp:Button ID="btnHidden" runat="server" Text="Button" Style="display: none" OnClick="BtnHidden_Click" />
    <asp:Button ID="btnHidden1" runat="server" Text="Button" Style="display: none" OnClick="BtnHidden1_Click" />
    <asp:Button ID="btnTimeExtendHidden" runat="server" Text="Button" Style="display: none"
        OnClick="BtnTimeExtendHidden_Click" />
    <asp:Button ID="btnCancelRequest" runat="server" Text="Button" Style="display: none"
        OnClick="BtnCancelRequest_Click" />
    <br />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupUpload" runat="server" TargetControlID="btnUpload"
        CancelControlID="ImgCancelUpload" DropShadow="false" X="-1" Y="-1" PopupControlID="panUploadPhoto"
        BackgroundCssClass="modalBackground" PopupDragHandleControlID="panUploadPhoto"
        Drag="false">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="panUploadPhoto" Style="display: none" Width="700px" Height="390px"
        runat="server" CssClass="ModalWindow">
        <table style="width: 100%;">
            <tr>
                <td align="right">
                    <asp:ImageButton ID="ImgCancelUpload" AlternateText="Close" runat="server" ImageAlign="Right"
                        ImageUrl="~/Images/Close.png" oncontextmenu="return false" Style="position: relative; height: 14px; width: 14px;" /></td>
            </tr>
            <tr>
                <td align="center">
                 <%--   <div id="Popup" class="rf-item input" style="height: auto; width: 400px; display: none; background: transparent;" title="Demo Text">--%>
                      <iframe id="iframeupload" scrolling="no" style="height: 306px; width:600px;"  marginheight="0" marginwidth="0" frameborder="0"  runat="server" src="ECM.aspx"></iframe>

                    <%--</div>--%>
                   

                </td>
            </tr>
        </table>
         
    </asp:Panel>
   <%-- <asp:Button runat="server" ID="btntest" Name="Save" Text="ECM Test" 
                            ValidationGroup="Save" CssClass="cssButton" OnClick="btntest_Click"  OnClientClick ="LoadPopup"/>
       <div id="Popup" class="rf-item input" style="height: auto; width: 400px; display: none; background: transparent;" title="Demo Text">
      <iframe id="iframe1" scrolling="no" style="height: 306px; width:374px;"  marginheight="0" marginwidth="0" frameborder="0"  runat="server" ></iframe>
           </div>
    <asp:Button runat="server" ID="btnDownload" Name="Save" Text="ECM Download Test" 
                            ValidationGroup="Save" CssClass="cssButton" OnClick="btnDownload_Click"  /> --%>
   
</asp:Content>
