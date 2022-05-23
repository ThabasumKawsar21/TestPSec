<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true" CodeBehind="SPEnterInformationBySecurity.aspx.cs"
 Inherits="VMSDev.SPEnterInformationBySecurity" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
</asp:Content>--%>


<%@ Register Src="SafetyPermitUserControls/VisitorGeneralInformationSP.ascx" TagName="VisitorGeneralInformationSP"
    TagPrefix="uc1" %>
<%@ Register Src="SafetyPermitUserControls/VisitorLocationInformationSP.ascx" TagName="VisitorLocationInformationSP"
    TagPrefix="uc2" %>
<%@ Register Src="SafetyPermitUserControls/EquipmentPermittedSP.ascx" TagName="EquipmentPermittedSP"
    TagPrefix="uc3" %>
<%@ Register Src="SafetyPermitUserControls/EmergencyContactInformationSP.ascx" TagName="EmergencyContactInformationSP"
    TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder"
    runat="server">
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript"src="Scripts/common.js"></script>
    <script type="text/javascript"src="Scripts/VMSEnterInformationBySecurity.js"></script>
    <script type="text/javascript"src="Scripts/VisitorLocationInformationSP.js"></script>
    <script type="text/javascript">
        var validationCancel = '<%=Resources.LocalizedText.CancelConfirmation%>'; 
     </script>
    <script language="javascript" type="text/javascript">    
     function pageload()
     {
    // GetOffsetTime();
     }
     function Back()
      {
         history.go(-1);
         return false;
       }    
     
     function doPostBack()
      {
        <asp:Literal ID="litPostBack" runat="server" />
     }

function onlyNumbers(evt)
{
	var e = event || evt; // for trans-browser compatibility
	var charCode = e.which || e.keyCode;

	if (charCode > 31 && (charCode < 48 || charCode > 57))
		return false;
	return true;
}
function OpenwebcamWindow(flname) {   

      var dialogresult = window.showModalDialog(flname, "UploadPhoto", "dialogHeight:450px;dialogWidth:800px;center:yes;status=0");
       __doPostBack('PostBackFromDialog', dialogresult);
  return true; 
}
// function OpenWindow(flname) {
//         var obj=new Object(); 
//         obj.Name = "parent";
//        args.doPostBack = doPostBack;   
//        var dialogresult =  window.showModalDialog(flname, "UploadPhoto", "dialogHeight:200px;dialogWidth:425px;center:yes;status=0");
//        return true;
//       if ( dialogresult != null )
//           {
//            __doPostBack('PostBackFromDialog', dialogresult);
//           }
//     }

  function GetOffsetTime() {	
  		     	    var CurrentDate = new Date();                  
                    var today=  (new Date()).format('dd/MM/yyyy HH:mm:ss');
				    VMSEnterInformationBySecurity.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
                    VMSEnterInformationBySecurity.AssignCurrentDateTime(today);
				}

function TimeExtendMailtrigger()
{

   var r = document.getElementById('<%=btnTimeExtendHidden.ClientID %>');
   r.click();
}

function CancelRequestMail()
{

 var r = document.getElementById('<%=btnCancelRequest.ClientID %>');
   r.click();
}


    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: center">
        <%--   <asp:LinkButton ID="lnkExpressCheckin" runat="server" OnClick="lnkExpressCheckin_Click" CausesValidation="false">Express Check in</asp:LinkButton>--%>
    </div>
    <uc1:VisitorGeneralInformationSP ID="VisitorGeneralInformationControlSP" 
        runat="server" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError1" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
            </td>
        </tr>
    </table>
    <hr />
    <%--//Changes done for VMS CR VMS06072010CR09 by Priti--%>
    <uc2:VisitorLocationInformationSP ID="VisitorLocationInformationControlSP" runat="server"
        OnBubbleIndexChanged="ShowEquipmentPermittedControl" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError2" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
            </td>
        </tr>
    </table>
    <%--//Changes done for VMS CR VMS06072010CR09 by Priti--%>
    <%-- <table id="tblEquipmentPermittedControl" runat="server" visible="false" width="100%"><tr><td align="left" visible="false">
    --%>
    <asp:UpdatePanel ID="EquipmentControl" runat="server">
        <ContentTemplate>
            <hr id="hrEquipmentControl" runat="server" visible="false" />
            <table align="center" width="98%">
                <tr align="center" style="height: 5px">
                    <td>
                        <asp:HiddenField ID="BatchNo" runat="server" />
                    </td>
                    <td width="60">
                    </td>
                    <td align="left">
                        <asp:Label runat="server" ID="lblError" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
                        <asp:CustomValidator runat="server" ID="custError" ></asp:CustomValidator>
                    </td>
                    <td align="center">
                        <asp:Label runat="server" ID="lblSubmitSuccess" ForeColor="Green" align="left" CssClass="cssDisplay"></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" CssClass="cssDisplay" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <uc3:EquipmentPermittedSP ID="EquipmentPermittedControlSP" runat="server" Visible="false" />
            <asp:Label runat="server" ID="lblError4" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--  </td></tr></table>--%>
    <%--  //end Changes done for VMS CR VMS06072010CR09 by Priti--%>
    <hr />
    <uc4:EmergencyContactInformationSP ID="EmergencyContactInformationControlSP" 
        runat="server" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label runat="server" ID="lblError3" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
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
                <%--<input type="button" id="button1" runat="server" value="<%$ Resources:LocalizedText, Back %>" class="cssButton" onclick="return Back(); " />--%>
                <asp:button runat="server" id="Back" text="<%$ Resources:LocalizedText, Back %>"
                    cssclass="cssButton" causesvalidation="false" onclick="Back_Click" />
            </td>
            <td align="center">
                <input type="button" onclick="return OpenwebcamWindow('webcam.aspx')" value="<%$ Resources:LocalizedText, WebCam %>"
                    runat="server" id="btnWebcam" class="cssButton" />
                <asp:HiddenField ID="hdnVisitDetailsID" runat="server" />
              
            </td>
            <td align="center">
                <asp:Button runat="server" ID="GenerateBadge" Text="<%$ Resources:LocalizedText, GenerateBadgeRequest %>" CssClass="cssButton"
                    CausesValidation="false" OnClick="BtnCheckIn_Click" />
            </td>
            <td align="center">
                <%--<asp:UpdatePanel ID="UPPhoto" runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnUpload" Name="Save" Text="<%$ Resources:LocalizedText, UploadPhoto %>" OnClientClick="OpenWindow('ImageUpload.aspx?strImage=Photo')"
                            ValidationGroup="Save" CssClass="cssButton" />
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <asp:panel id="pnlImageUpload" runat="server">
                    <contenttemplate>
                        <asp:Button runat="server" ID="btnUpload" Name="Save" Text="<%$ Resources:LocalizedText, UploadPhoto %>"  CssClass="cssButton" />                            
                    </contenttemplate>
                </asp:panel>
            </td>
            <%--<td align="center">
                <asp:Button runat="server" ID="Search" Text="Search" OnClientClick="Search()" OnClick="Search_Click"
                    CssClass="cssButton" CausesValidation="false" />
            </td>--%>
            <td align="center">
                <asp:Button runat="server" ID="Submit" Name="Submit" Text="<%$ Resources:LocalizedText, Submit %>" OnClick="Submit_Click"
                    CausesValidation="true" CssClass="cssButton" />
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Save" Name="Save" Text="<%$ Resources:LocalizedText, Save %>" OnClick="Save_Click"
                    Visible="false" CausesValidation="false" CssClass="cssButton" />
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Reset" Name="Reset" Text="<%$ Resources:LocalizedText, Reset %>"  OnClick="Reset_Click"
                    ValidationGroup="Reset" CssClass="cssButton" />
            </td>
            <td align="center">
                <asp:Button runat="server" ID="Cancel" Name="Cancel" Text="<%$ Resources:LocalizedText, Cancel %>"  OnClick="Cancel_Click"
                    ValidationGroup="Cancel" CssClass="cssButton" OnClientClick="javascript:return confirm(validationCancel);"  />
            </td>
        </tr>        
    </table>
    <asp:Button ID="btnHidden" runat="server" Text="Button" Style="display: none" OnClick="BtnHidden_Click" />
    <asp:Button ID="btnHidden1" runat="server" Text="Button" Style="display: none" OnClick="BtnHidden1_Click" />
     <asp:Button ID="btnTimeExtendHidden" runat="server" Text="Button" Style="display: none" OnClick="BtnTimeExtendHidden_Click" />
     <asp:Button ID="btnCancelRequest" runat="server" Text="Button" Style="display: none" OnClick="BtnCancelRequest_Click" />
    <br />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupUpload" runat="server" TargetControlID="btnUpload"
        CancelControlID="ImgCancelUpload" DropShadow="false" X="-1" Y="-1" PopupControlID="panUploadPhoto"
        BackgroundCssClass="modalBackground" PopupDragHandleControlID="panUploadPhoto"
        Drag="false">
    </ajaxToolkit:ModalPopupExtender>
    <asp:panel id="panUploadPhoto" style="display: none" width="400px" height="390px"
        runat="server" cssclass="ModalWindow">
        <%--<div id="divImageUpload" style="padding-top: 5px">--%>
       <table style="width:100%;">
                 <tr>  <td align="right"> <asp:imagebutton id="ImgCancelUpload" alternatetext="Close" runat="server" imagealign="Right"
                        imageurl="~/Images/Close.png" oncontextmenu="return false" style="position: relative;
                        height: 14px; width: 14px;" /></td>
                </tr>
                <tr><td align="center">
                <iframe id="iframeupload" scrolling="no" style="height: 360px; width: 374px;" src="ImageUpload.aspx"
                    marginheight="0" marginwidth="0" border="0" runat="server" ></iframe></td></tr>
            </table>
        <%-- </div>--%>
    </asp:panel>
    <style type="text/css">
        .ModalWindow
        {
            border: 1px solid #c0c0c0;
            background: #f0f0f0;
            padding: 0px 0px 0px 0px;
            width: 500px;
            height: auto;
        }
        .modalBackground
        {
            background-color: #878776;
            filter: alpha(opacity=40);
            opacity: 0.5;
        }
        p.MsoNormal
        {
            margin-top: 1.3pt;
            margin-right: 5.75pt;
            margin-bottom: 12.0pt;
            margin-left: 0in;
            line-height: 12.0pt;
            font-size: 10.0pt;
            font-family: "Arial" , "sans-serif";
        }
    </style>
</asp:Content>
