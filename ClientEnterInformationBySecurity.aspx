<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientEnterInformationBySecurity.aspx.cs"
    Inherits="VMSDev.ClientEnterInformationBySecurity" MasterPageFile="~/VMSMain.Master" %>

<%@ Register Src="UserControls/NewVisitorGeneralInformation.ascx" TagName="VisitorGeneralInformation"
    TagPrefix="uc1" %>
<%@ Register Src="UserControls/VisitorLocationInformation.ascx" TagName="VisitorLocationInformation"
    TagPrefix="uc2" %>
<%@ Register Src="UserControls/EquipmentPermitted.ascx" TagName="EquipmentPermitted"
    TagPrefix="uc3" %>
<%@ Register Src="UserControls/EmergencyContactInformation.ascx" TagName="EmergencyContactInformation"
    TagPrefix="uc4" %>
    <%@ Register Src="UserControls/EquipmentInCustody.ascx" TagName="EquipmentInCustody"
    TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:content id="VMSEnterInofrmationContent" contentplaceholderid="VMSContentPlaceHolder"
    runat="server">    
    <script type="text/javascript">
        var validationCancel = '<%=Resources.LocalizedText.CancelConfirmation%>'; 
    </script>
    <script language="javascript" type="text/javascript">    
     function pageload()
     {
    // GetOffsetTime();
     }
//     function Back()
//      {
//         history.go(-1);
//         return false;
//       }    
      function CloseParentWindow() {
            window.parent.location.href = window.parent.location.href;
            // return false;
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

    //var dialogresult = window.showModalDialog(flname, "UploadPhoto", "dialogHeight:450px;dialogWidth:800px;center:yes;status=0");
    var dialogresult = window.open(flname, "UploadPhoto", "dialogHeight:450px;dialogWidth:800px;center:yes;status=0");
       //__doPostBack('PostBackFromDialog', dialogresult);
  return true; 
}
 function OpenWindow(flname) {
         var obj=new Object(); 
         obj.Name = "parent";
        args.doPostBack = doPostBack;   
        var dialogresult =  window.showModalDialog(flname, "UploadPhoto", "dialogHeight:200px;dialogWidth:425px;center:yes;status=0");
        return true;
       if ( dialogresult != null )
           {
            __doPostBack('PostBackFromDialog', dialogresult);
           }
     }

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

    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript"src="Scripts/common.js"></script>
    <script type="text/javascript"src="Scripts/VMSEnterInformationBySecurity.js"></script>
    <script type="text/javascript"src="Scripts/VisitorLocationInformation.js"></script>
    <asp:scriptmanager id="ScriptManager1" runat="server">
    </asp:scriptmanager>
    <div style="text-align: center">
        <%--   <asp:LinkButton ID="lnkExpressCheckin" runat="server" OnClick="lnkExpressCheckin_Click" CausesValidation="false">Express Check in</asp:LinkButton>--%>
    </div>
    <uc1:VisitorGeneralInformation ID="VisitorGeneralInformationControl" runat="server" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:label runat="server" id="lblError1" forecolor="Red" align="left" cssclass="cssDisplay">
                </asp:label>
            </td>
        </tr>
    </table>
    <hr />
    <%--//Changes done for VMS CR VMS06072010CR09 by Priti--%>
    <uc2:VisitorLocationInformation ID="VisitorLocationInformationControl" runat="server"
        OnBubbleIndexChanged="ShowEquipmentPermittedControl" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:label runat="server" id="lblError2" forecolor="Red" align="left" cssclass="cssDisplay">
                </asp:label>
            </td>
        </tr>
    </table>
    <%--//Changes done for VMS CR VMS06072010CR09 by Priti--%>
    <%-- <table id="tblEquipmentPermittedControl" runat="server" visible="false" width="100%"><tr><td align="left" visible="false">
    --%>
    <asp:updatepanel id="EquipmentControl" runat="server">
        <contenttemplate>
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
            <uc3:EquipmentPermitted ID="EquipmentPermittedControl" runat="server" Visible="false" />
            <asp:Label runat="server" ID="lblError4" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
        </contenttemplate>
    </asp:updatepanel>
      <asp:updatepanel id="EquipmentInCustody" runat="server">
        <contenttemplate>
            <hr id="hr1" runat="server" visible="false" />
            <table align="center" width="98%">
                <tr align="center" style="height: 5px">
                    <td>
                        <asp:HiddenField ID="BatchNoEquipment" runat="server" />
                    </td>
                    <td width="60">
                    </td>
                    <td align="left">
                        <asp:Label runat="server" ID="lblErrorEquipment" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
                        <asp:CustomValidator runat="server" ID="custErrorEquipment" ></asp:CustomValidator>
                    </td>
                    <td align="center">
                        <asp:Label runat="server" ID="lblSubmitSuccessEquipment" ForeColor="Green" align="left" CssClass="cssDisplay"></asp:Label>
                        <asp:Label ID="lblMessageEquipment" runat="server" CssClass="cssDisplay" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
         
            <hr id="hrEquipmentcustody" runat="server" visible="false" />
            <uc5:EquipmentInCustody ID="EquipmentCustodyControl" runat="server" Visible="false" />
            <asp:Label runat="server" ID="lblError4Equipment" ForeColor="Red" align="left" CssClass="cssDisplay"></asp:Label>
            <hr />
        </contenttemplate>
    </asp:updatepanel>
    <%--  </td></tr></table>--%>
    <%--  //end Changes done for VMS CR VMS06072010CR09 by Priti--%>
  
    <uc4:EmergencyContactInformation ID="EmergencyContactInformationControl" runat="server" />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:label runat="server" id="lblError3" forecolor="Red" align="left" cssclass="cssDisplay">
                </asp:label>
            </td>
        </tr>
    </table>
    <br />
    <table align="right" runat="server" id="tblButtons">
        <tr align="center">
            <td align="center">
                <asp:updatepanel id="UPIDProof" runat="server" visible="false">
                    <contenttemplate>
                        <asp:Button runat="server" ID="btnIdProof" Name="Save" Text="UploadIDProof" OnClientClick="return OpenWindow('ImageUpload.aspx?strImage=Proof')"
                            ValidationGroup="Save" CssClass="cssButton" />
                    </contenttemplate>
                </asp:updatepanel>
            </td>
            <td align="center">
                <%-- <input type="button" id="button1" runat="server" value="<%$ Resources:LocalizedText, Back %>" class="cssButton" onclick="return Back(); " />--%>
                <asp:button runat="server" id="Back" text="<%$ Resources:LocalizedText, Back %>"
                    cssclass="cssButton" causesvalidation="false" onclick="Back_Click" />
            </td>
            <td align="center">
                <%--<input type="button" onclick="return OpenwebcamWindow('webcam.aspx')" value="<%$ Resources:LocalizedText, WebCam %>"
                    runat="server" id="btnWebcam" class="cssButton" />--%>
                <asp:hiddenfield id="hdnVisitDetailsID" runat="server" />
               
            </td>
            <td align="center">
                <%--<asp:button runat="server" id="GenerateBadge" text="Notify"
                    cssclass="cssButton" causesvalidation="false" onclick="BtnCheckIn_Click" />--%>
                <%--<asp:button runat="server" id="GenerateBadge" text="Notify"
                    cssclass="cssButton" causesvalidation="false" onclick="BtnNotify_Click" />--%>
            </td>
            <td align="center">
                <%-- <asp:UpdatePanel ID="UPPhoto" runat="server">--%>
               <%-- <asp:panel id="pnlImageUpload" runat="server">
                    <contenttemplate>
                        <asp:Button runat="server" ID="btnUpload" Name="Save" 
                            Text="<%$ Resources:LocalizedText, UploadPhoto %>"  CssClass="cssButton" />                            
                    </contenttemplate>
                </asp:panel>--%>
                <%--</asp:UpdatePanel>--%>
            </td>
            <%--<td align="center">
                <asp:Button runat="server" ID="Search" Text="Search" OnClientClick="Search()" OnClick="Search_Click"
                    CssClass="cssButton" CausesValidation="false" />
            </td>--%>
            <td align="center">
                <asp:button runat="server" id="Submit" name="Submit" text="<%$ Resources:LocalizedText, Submit %>"
                    onclick="Submit_Click" causesvalidation="true" cssclass="cssButton" />
            </td>
            <td align="center">
                <asp:button runat="server" id="Save" name="Save" text="<%$ Resources:LocalizedText, Save %>"
                    onclick="Save_Click" visible="false" causesvalidation="false" cssclass="cssButton" />
            </td>
            <td align="center">
                <asp:button runat="server" id="Reset" name="Reset" text="<%$ Resources:LocalizedText, Reset %>"
                    onclick="Reset_Click" validationgroup="Reset" cssclass="cssButton" />
            </td>
            <td align="center">
                <asp:button runat="server" id="Cancel" name="Cancel" text="<%$ Resources:LocalizedText, Cancel %>"
                    onclick="Cancel_Click" validationgroup="Cancel" cssclass="cssButton" onclientclick="javascript:return confirm(validationCancel);" />
            </td>
        </tr>
    </table>
    <asp:button id="btnHidden" runat="server" text="Button" style="display: none" onclick="BtnHidden_Click" />
    <asp:button id="btnHidden1" runat="server" text="Button" style="display: none" onclick="BtnHidden1_Click" />
    <asp:button id="btnTimeExtendHidden" runat="server" text="Button" style="display: none"
        onclick="BtnTimeExtendHidden_Click" />
    <asp:button id="btnCancelRequest" runat="server" text="Button" style="display: none"
        onclick="BtnCancelRequest_Click" />
    <br />
<%--    <ajaxToolkit:ModalPopupExtender ID="ModalPopupUpload" runat="server" TargetControlID="btnUpload"
        CancelControlID="ImgCancelUpload" DropShadow="false" X="-1" Y="-1" PopupControlID="panUploadPhoto"
        BackgroundCssClass="modalBackground" PopupDragHandleControlID="panUploadPhoto"
        Drag="false">
    </ajaxToolkit:ModalPopupExtender>--%>
    <asp:panel id="panUploadPhoto" style="display: none" width="400px" height="390px"
        runat="server" cssclass="ModalWindow">
        <%--<div id="divImageUpload" style="padding-top: 5px">--%>
       <table style="width:100%;">
                 <tr>  <td align="right"> <%--<asp:imagebutton id="ImgCancelUpload" alternatetext="Close" runat="server" imagealign="Right"
                        imageurl="~/Images/Close.png" oncontextmenu="return false" style="position: relative;
                        height: 14px; width: 14px;" />--%></td>
                </tr>
                <tr><td align="center">
                <%--<iframe id="iframeupload" scrolling="no" style="height: 360px; width: 374px;" src="ImageUpload.aspx" marginheight="0" marginwidth="0" 
                        frameborder="0" runat="server"></iframe>--%>
                    <%--<iframe id="iframeupload" runat="server" scrolling="no" style="height: 360px; width: 374px;" src="ImageUpload.aspx" marginheight="0" marginwidth="0" 
                        frameborder="0"></iframe>--%>
                  
                    </td></tr>
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
</asp:content>
