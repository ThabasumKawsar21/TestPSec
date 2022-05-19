<%@ Page Title="" Language="C#" MasterPageFile="~/VMSMain.Master" AutoEventWireup="true"
    CodeBehind="GenerateIDCard.aspx.cs" Inherits="VMSDev.GenerateIDCard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <style type="text/css">
        .style2
        {
            width: 123px;
        }
        .styleImage
        {
            max-height: 150px;
            width: 200px;
        }
        .bttnCancel
        {
            background-color: #DF0101;
            border: #B40404 2px solid;
            color: White;
            font-family: Verdana;
            font-size: 8pt;
            font-weight: bold;
            height: 21px;
            left: 305px;
            bottom: 12px;
            position: absolute;
        }
        .bttnOK
        {
            background-color: #669977;
            border: #336633 2px solid;
            visibility: visible;
            color: White;
            font-family: Verdana;
            font-size: 8pt;
            font-weight: bold;
            height: 21px;
            left: 247px;
            bottom: 12px;
            position: absolute;
        }
        
        #labelClass
        {
            margin-top: 1.3pt;
            margin-right: 5.75pt;
            margin-bottom: 12.0pt;
            margin-left: 0in;
            line-height: 12.0pt;
            font-size: 10.0pt;
        }
        .style3
        {
            width: 169px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    
<script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.Jcrop.js" type="text/javascript"></script>
    <link href="App_Themes/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        ////IVS Image Source integrationwith ID card and CHire
              function GetImageFromIDCard() {

            const CHireKey = '<%=System.Configuration.ConfigurationManager.AppSettings["CHireKey"]%>';
                  var IsCHireON = false;
            if (CHireKey === 'ON')
                IsCHireON = true;
            var id = $("#<%=txtID.ClientID%>").val();
            var apiresponse = 'API response : ';
            const APIEnvUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["IDCardApiUrl"]%>';
                    var ChireFileContentId = $('#<%=hdnFileUploadId.ClientID%>').val();

            $.ajax({
                type: 'GET',
                url: APIEnvUrl,
                xhrFields: {
                    withCredentials: true
                },
                data: { "associateId": id },
                dataType: 'json',
                success: function (image) {
                   // debugger

                    // debugger
                    if (image.indexOf("data:image") != -1) {
                        apiresponse += 'image found';
                        $('#<%=imgAssociateImage.ClientID%>').attr("src", image);
                         $('#<%=hdnAssociateImage.ClientID%>').val(image);
                    }
                    else {
                        apiresponse += image;
                        image = "Images/char.jpeg";
                         $('#<%=hdnAssociateImage.ClientID%>').val(image);
                        IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#<%=imgAssociateImage.ClientID%>').attr("src", image);
                       }
                    
                     //TO DO: remove the log.
                     console.log(apiresponse);
                 },
                 error: function (e) {
                   //  debugger
                     apiresponse += "Error: Response JSON : " + e.responseJSON + " Status : " + e.status + " Response Text : " + e.responseText;
                     $('#<%=hdnAssociateImage.ClientID%>').val("Images/char.jpeg");
                     IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#<%=imgAssociateImage.ClientID%>').attr("src", "Images/char.jpeg");
                    //TO DO: remove the log.
                    console.log(apiresponse);
                }
            });
        }

        // function to get image from Chire.
        function GetImageFromChire(filecontentID) {
            //file content id is already fetched from CHIRE VIEW
debugger
           
            if (filecontentID !== null && filecontentID !== '') {

                $.ajax({
                    type: 'POST',
                    url: "GenerateIDCard.aspx/GetChireImageFromECMForIDcard",
                    contentType: "application/json;charset=utf-8",
                    data: '{"contentId":"' + filecontentID + '"}',
                    dataType: 'json',
                    success: function (image) {
                       // debugger
                        console.log('Displaying Chire Image');
                        $('#<%=imgAssociateImage.ClientID%>').attr("src",JSON.parse(image.d));
                         $('#<%=hdnAssociateImage.ClientID%>').val(JSON.parse(image.d));
                    }
                    ,
                    error: function (e) {
                        //debugger
                        console.log('ERROR: Chire Flow');
                        $('#<%=imgAssociateImage.ClientID%>').attr("src", "Images/char.jpeg");
                         $('#<%=hdnAssociateImage.ClientID%>').val("Images/char.jpeg");
                    }
                });
            }
            else {
                console.log(' Chire Image id null');
                $('#<%=imgAssociateImage.ClientID%>').attr("src", "Images/char.jpeg");
                 $('#<%=hdnAssociateImage.ClientID%>').val("Images/char.jpeg");
            }
        }


        function PreviewImage() {
            
            $('#<%= testImage.ClientID %>').Jcrop({
               onChange: showCoords,
                onSelect: showCoords,
                onRelease: clearCoords
            });
        }
        // Simple event handler, called from onChange and onSelect 
        // event handlers, as per the Jcrop invocation above
        function showCoords(c) {                        
            $('#<%= x1.ClientID %>').val(c.x);
            $('#<%= y1.ClientID %>').val(c.y);
            $('#<%= x2.ClientID %>').val(c.x2);
            $('#<%= y2.ClientID %>').val(c.y2);
            $('#<%= width.ClientID %>').val(c.w);
            $('#<%= height.ClientID %>').val(c.h);

        };


        function clearCoords() {

            // $('#coords input').val('');
            $('#<%= x1.ClientID %>').val('');
            $('#<%= y1.ClientID %>').val('');
            $('#<%= x2.ClientID %>').val('');
            $('#<%= y2.ClientID %>').val('');
            $('#<%= width.ClientID %>').val('');
            $('#<%= height.ClientID %>').val('');

        };
    </script>
    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            //  
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            else
                return true;
        }
        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        };

                function NameSelect(sender) {

                    document.getElementById('<%=hdnNameSelect.ClientID %>').value = sender.value;
                }

                function validateOk() {
                    var valueName = document.getElementById('<%=hdnNameSelect.ClientID %>').value;
                        if (valueName == '') {
                        alert("please choose a name!!");
                        return false;
                    }
                    else {
                        return true;
                        }
                }
    </script>
    <asp:ScriptManager ID="ScriptMgrImageUpload" EnablePartialRendering="true" runat="server">
    </asp:ScriptManager>
    <table id="Table33" cellpadding="1" cellspacing="2" width="100%" class="noPrint">
        <tr>
            <td colspan="2" align="left" class="table_header_bg" style="height: 19px">
                &nbsp;&nbsp;<asp:Label ID="Label34" runat="server" CssClass="Table_header" Text="Generate ID Card"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50%;" align="left">
                <table style="margin-left: 0px">
                    <tr>
                        <td align="left" valign="middle" style="padding-left: 3px">
                            <asp:Label runat="server" CssClass="field_txt" ID="lblAssociateID" Text="Associate ID"></asp:Label>
                            <asp:TextBox ID="txtID" runat="server" CssClass="txtField" MaxLength="15" onkeypress="return onlyNumbers()"></asp:TextBox>
                            &nbsp;<asp:ImageButton ID="imgbtnSearchImage" runat="server" ImageUrl="~/Images/Search_1.png"
                                OnClick="ImgbtnSearchImage_Click" Height="15px" />&nbsp;<asp:ImageButton ID="imgbtnClearID"
                                    runat="server" Width="15px" ImageUrl="~/Images/Clear.png" OnClick="ImgbtnClearID_Click" />
                           <%-- <asp:RegularExpressionValidator ID="regAssociateId" runat="server" ControlToValidate="txtID"
                                ValidationExpression="[0-9]{,}$" ErrorMessage="Please Enter Valid Associate Id"
                                Display="Static"></asp:RegularExpressionValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div align="left">
                                <asp:Panel ID="pnlError" runat="server" Width="400px" Style="padding-left: 20px"
                                    ForeColor="Red" Font-Size="Smaller" HorizontalAlign="Center" Visible="false">
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td rowspan="3">
                <asp:Panel ID="pnlPreviewID" runat="server" Width="500px" BorderColor="#C6D4BB" BorderWidth="3px"
                    Visible="False">
                    <table width="500px">
                        <tr>
                            <td>
                                <div id="printOption" class="noPrint" style="border-bottom-color: White">
                                    <%-- <asp:ImageButton alt="" ID="ImgPrinter" runat="server" ImageUrl="~/Images/Printer-icon.png" OnClick="ImgPrinter_click"/>--%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="div_print" class="Print" style="display: inline">
                                    <!--Front ID Start -->
                                    <div class="IdContainerFront">
                                        <div class="TopImg">
                                            <img alt="" id="imgIDCardTop" runat="server" src="Images/Kolkata/CardTop.jpg" class="TopImg" /></div>
                                        <label id="lblyear" runat="server" class="issueYear">
                                            11</label>
                                        <div class="assPhoto">
                                            <asp:Image ID="ImgAssociate" runat="server" ImageUrl="~/Images/DummyPhoto.png" /></div>
                                        <label id="lblName" runat="server" class="assName">
                                            Associate Name</label>
                                        <label class="assNumber" id="lblFrontID" runat="server">
                                            123456</label>
                                        <div class="marTop10">
                                            <img alt="" id="imgFrontLine" runat="server" src="Images/Kolkata/line.jpg" width="180"
                                                height="2" /></div>
                                        <div class="ctsLogo">
                                            <img src="Images/IDCard_CognizantLogo.png" alt="CTS Logo" height="38" /></div>
                                        <div class="BotImg">
                                            <img alt="" src="Images/Kolkata/CardBottom.jpg" class="TopImg" id="imgIdCardBottom"
                                                runat="server" /></div>
                                    </div>
                                    <!--Front ID End -->
                                </div>
                                <div class="page-break" style="display: inline">
                                </div>
                                <div class="Print" style="display: inline">
                                    <!--Back ID Start -->
                                    <div class="IdContainerBack">
                                        <div class="TopImg">
                                            <img alt="" id="imgIDRearTop" runat="server" src="Images/Kolkata/CardTop.jpg" class="TopImg" /></div>
                                        <div class="barCode">
                                            <asp:Image ID="imgBarCode" ImageUrl="~/Barcode.aspx?Code=123456" runat="server" ImageAlign="Bottom"
                                                Width="185px" Height="50px" />
                                        </div>
                                        <div class="assNumber2">
                                            <label id="lblRearID" runat="server">
                                                221107</label>
                                        </div>
                                        <div class="marTop10">
                                            <label>
                                                Emergency contact #
                                            </label>
                                            <label id="lblEmergencyContactID" runat="server" class="boldText">
                                                9840817738</label>
                                        </div>
                                        <div class="marTop10">
                                            <label>
                                                Blood Group
                                            </label>
                                            <label id="lblBloodGroupID" runat="server" class="boldText">
                                                o+</label>
                                        </div>
                                        <div class="marTop15">
                                            <img id="imgRearLine1" runat="server" alt="" src="Images/Kolkata/line.jpg" width="185"
                                                height="2" /></div>
                                        <div class="inform">
                                            <label>
                                                If found, please inform: <span>1800 258 2345</span></label>
                                        </div>
                                        <div class="marTop16">
                                            <img id="imgRearLine2" runat="server" alt="" src="Images/Kolkata/line.jpg" width="185"
                                                height="2" /></div>
                                        <div class="contactCts" align="left">
                                            <asp:Label ID="lblCognizant" CssClass="boldText" Font-Size="6.5pt" Width="125px"
                                                Font-Bold="true" runat="server" Text="Cognizant Technology Solutions India Pvt. Ltd"
                                                Style="text-align: Left;"></asp:Label><br />
                                            <asp:Label ID="lblFacilityAddress" CssClass="marTop5" Font-Size="6.5pt" Width="125px"
                                                runat="server" Text="Address" Style="text-align: Left;"></asp:Label><br />
                                        </div>
                                        <div class="BotImg">
                                            <img id="imgIDRearBottom" runat="server" alt="" src="Images/Kolkata/CardBottom.jpg"
                                                class="TopImg" /></div>
                                    </div>
                                    <!--Back ID End -->
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblMessage" runat="server" CssClass="field_txt" ForeColor="Red" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlDetails" runat="server">
                    <asp:Table ID="tblDetails" runat="server" Width="400px" BorderColor="#C6D4BB" BorderWidth="3px"
                        HorizontalAlign="Left" CellPadding="5" CellSpacing="5" Visible="False">
                        <asp:TableHeaderRow BackColor="#C6D4BB">
                            <asp:TableHeaderCell Text="Associate Information" CssClass="Table_header" ForeColor="Black"
                                Width="100%" ColumnSpan="2">            
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell align="right">
                                <asp:Image runat="server" ID="imgAssociateImage" Style="max-height: 150px; max-width: 200px" />
                                <%--        <img runat="server" src="Images/DummyPhoto.png" id="AssociateImage"
                                style="max-height: 150" width="200" />--%>
                            </asp:TableCell>
                            <%--<asp:TableCell Style="white-space: nowrap;">
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/Edit_image.png" Width="24px"
                                    Height="24px" runat="server" OnClick="ImgbtnEdit_Click" ToolTip="Crop Image" />
                                <asp:ImageButton ID="imgDownload" ImageUrl="~/Images/download image.bmp" Width="32px"
                                    Height="32px" runat="server" OnClick="ImgDownload_Click" ToolTip="Download Image" />
                                <asp:ImageButton ID="imgPrintStatus" Visible="false" ImageUrl="~/Images/update.jpg"
                                    OnClick="ImgPrintStatus_Click" runat="server" ToolTip="Update Print Status" />
                            </asp:TableCell>--%>
                        </asp:TableRow>
                        <asp:TableRow align="left">
                            <asp:TableCell>
                                <asp:Label ID="lblID" Text="Associate Id" CssClass="field_txt" runat="server"> 
                                </asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblIDText" runat="server" CssClass="field_txt">                    
                                </asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow align="left">
                            <asp:TableCell>
                                <asp:Label ID="lblFirstName" Text="First Name" CssClass="field_txt" runat="server"> 
                                </asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblFirstNameText" runat="server" CssClass="field_txt">                    
                                </asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow align="left">
                            <asp:TableCell>
                                <asp:Label ID="lblLastName" Text="Last Name" CssClass="field_txt" runat="server"> 
                                </asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblLastNameText" runat="server" CssClass="field_txt">                    
                                </asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow align="left">
                            <asp:TableCell>
                                <asp:Label ID="lblCity" Text="City" CssClass="field_txt" runat="server"> 
                                </asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblCityText" runat="server" CssClass="field_txt">                    
                                </asp:Label>
                                <asp:HiddenField ID="hdnFileUploadId" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow align="left">
                            <%--#DPNAME--%>
                            <asp:TableCell>
                                <asp:Label ID="lblDisplayName" Text="Display Name" CssClass="field_txt" runat="server"> 
                                </asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="lblDisplayNameText" runat="server" CssClass="field_txt">                    
                                </asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow align="left">
                            <asp:TableCell align="right">
                                <asp:Button ID="btnShowPreview" CssClass="cssIDButton" runat="server" Text="Show Preview"
                                    OnClick="BtnGenerateIDCard_Click" ValidationGroup="OnPreview" />
                            </asp:TableCell>
                            <asp:TableCell>
                            <%--<asp:RequiredFieldValidator ID="ReqDisplay" runat="server" Style="padding-left: 8px"
                                TabIndex="5" ErrorMessage="Enter Display Name" Display="Dynamic" Font-Names="Arial Narrow"
                                Font-Bold="true" Font-Size="7" ForeColor="Red" ControlToValidate="txtDisplayName"
                                ValidationGroup="OnPreview"></asp:RequiredFieldValidator>--%>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <div>
        <p>
        </p>
        <div>
        </div>
        <asp:Button ID="btnHidden" runat="server" Style="display: none" Text="Button" />
        <ajaxToolkit:ModalPopupExtender ID="mdlCropImage" runat="server" PopupControlID="pnlCropImage"
            BackgroundCssClass="modalBackground" CancelControlID="imgCancel" TargetControlID="btnHidden">
        </ajaxToolkit:ModalPopupExtender>
         <asp:Panel ID="pnlCropImage" runat="server" CssClass="ModalWindow" Style="display: none">
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblCrop" runat="server" class="inform" Text="Click and drag to crop the selected area">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <img runat="server" src="~/Images/DummyPhoto.png" alt="" id="testImage" class="img-crop" />
                        </div>
                    </td>
                    <td>
                       <%-- <asp:ImageButton ID="imgSave" AlternateText="Save" runat="server" ImageUrl="~/Images/approve.png"
                            Width="25px" OnClick="ImgSave_Click" />--%>
                        <asp:ImageButton ID="imgCancel" AlternateText="Cancel" runat="server" ImageUrl="~/Images/Close.png"
                            Width="25px" />
                    </td>
                </tr>
                <caption>
                    <input type="text" name="x1" id="x1" runat="server" style="display: none" />
                    <input type="text" name="y1" id="y1" runat="server" style="display: none" />
                    <input type="text" name="x2" id="x2" runat="server" style="display: none" />
                    <input type="text" name="y2" id="y2" runat="server" style="display: none" />
                    <input type="text" name="width" id="width" runat="server" style="display: none" />
                    <input type="text" name="height" id="height" runat="server" style="display: none" />
                    <input type="hidden" name="hdnUploadedImage" id="hdnUploadedImage" runat="server" />
                    <input type="hidden" name="hdnUploadedImageFileName" id="hdnUploadedImageFileName"
                        runat="server" />

                     <input type="hidden" name="hdnAssociateImage" id="hdnAssociateImage" runat="server" />
                </caption>
                </tr>
            </table>
        </asp:Panel>
       
               <ajaxToolkit:ModalPopupExtender ID="mdlNineNameSuggestions" runat="server" PopupControlID="pnlNineNameSuggestion"
            BackgroundCssClass="modalBackground" CancelControlID="btnCancel" TargetControlID="btnHidden" >
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlNineNameSuggestion" runat="server" CssClass="popupWindow" Style="display: none">
            <table id="tblNames" runat="server">
                <tr>
                    <td align="justify" style="background-color: #CEF6E3;" colspan="3">
                        <p>
                            <asp:Label ID="lblInformation" runat="server" Style="color: green;" Text=""></asp:Label>
                        </p>
                        <p>
                            <asp:Label ID="lblInformatn" runat="server" Style="color: green;" Text=""></asp:Label>
                        </p>
                    </td>
                </tr>
                <tr id="lblRow" style="background-color: White;">
                    <td id="first" style="text-align:left;" runat="server">
                        <asp:Label ID="firstName" runat="server" Style="font-size:14px; font-weight:bold;" Text="First Name Based Options">
                        </asp:Label>
                    </td>
                    <td id="mid" runat="server" style="text-align:left; visibility:visible;" >
                        <asp:Label ID="midName" runat="server" Style="font-size:14px; font-weight:bold; visibility:visible;" Text="Middle Name Based Options"> 
                        </asp:Label>
                    </td>
                    <td id="last" runat="server" style="text-align:left; visibility:visible;">
                        <asp:Label ID="lastName" runat="server" Style="font-size:14px; font-weight:bold; visibility:visible;" Text="Last Name Based Options">
                        </asp:Label>
                    </td>
                </tr>
                <tr id="dataRow" style="background-color: White;">
                    <td id="ii" runat="server" colspan="3">
                        <div id="container" runat="server">
                        </div>
                     </td>
                </tr>
                <tr>
                    <td style="background-color: #E0F2F7;" colspan="3">
                        <p style="width: 480px">
                            <asp:Label ID="note" runat="server" Class="labelClass" Style="color: green; font-weight: bold; padding-left:150px;">Note</asp:Label>
                        </p>
                        <p style="width: 617px; margin-left: 7px; text-align:left;">
                          • The above given name suggestion is a combination of associate’s First Name, Middle Name &amp; Last Name as present in HCM.
                        </p>
                        <p style="width: 607PX; margin-left: 7px; text-align:left;">
                          • You can confirm with the associate on the name that needs to be selected to avoid re-printing of the card.
                        </p>
                    </td>
                </tr>
                 <tr id="btnOkRow" style="height: 45px;">
                    <td id="rw" colspan="3" style="background-color: White;">
                        <asp:Button ID="btnOK" runat="server" Text="OK" Class="bttnOK" OnClick="BtnOK_Click" OnClientClick="return validateOk()" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Class="bttnCancel" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="hdnAssocId" runat="server" />
    <asp:HiddenField ID="hdnNameSelect" runat="server" />
    <asp:HiddenField ID="hdnLocation" runat="server" />  <asp:HiddenField ID="hdnFileDwnLoad" runat="server" />
</asp:Content>
