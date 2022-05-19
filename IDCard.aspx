<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IDCard.aspx.cs" Inherits="VMSDev.IDCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <title>Cognizant ID Card Template</title>
    <link rel="Stylesheet" type="text/css" href="App_Themes/styleIDCard.css"/>	   
    
    
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function GetImageFromIDCard() {
            //debugger;
            var IsCHireON = false;
            const CHireKey = '<%=System.Configuration.ConfigurationManager.AppSettings["CHireKey"]%>';
          
            if (CHireKey === 'ON')
                IsCHireON = true;
            var id = document.getElementById('lblFrontID').textContent.trim();
            var apiresponse = 'API response : ';
            const APIEnvUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["IDCardApiUrl"]%>';
            
              var ChireFileContentId = $('#<%=hdnFileUploadID.ClientID%>').val();

            $.ajax({
                type: 'GET',
                url: APIEnvUrl,
                xhrFields: {
                    withCredentials: true
                },
                data: { "associateId": id },
                dataType: 'json',
                success: function (image) {
                    //debugger

                     if (image.indexOf("data:image") != -1) {
                        apiresponse += 'image found';
                        $('#<%=ImgAssociate.ClientID%>').attr("src", image);
                          $('#<%=hdnImageURL.ClientID%>').val(image);
                    }
                    else {
                        apiresponse += image;
                        image = "Images/char.jpeg";
                        $('#<%=hdnImageURL.ClientID%>').val(image);
                        IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#<%=ImgAssociate.ClientID%>').attr("src", image);
                    }
                   
                    
                     //TO DO: remove the log.
                     console.log(apiresponse);
                 },
                 error: function (e) {
                    // debugger
                     apiresponse += "Error: Response JSON : " + e.responseJSON + " Status : " + e.status + " Response Text : " + e.responseText;
                      $('#<%=hdnImageURL.ClientID%>').val("Images/char.jpeg");
                     IsCHireON ? GetImageFromChire(ChireFileContentId) : $('#<%=ImgAssociate.ClientID%>').attr("src", "Images/char.jpeg");
                   
                    //TO DO: remove the log.
                    console.log(apiresponse);
                }
            }); 
        }
      // function to get image from Chire.
        function GetImageFromChire(filecontentID) {
            //file content id is already fetched from CHIRE VIEW

          
            if (filecontentID !== null && filecontentID !== '') {

                $.ajax({
                    type: 'POST',
                    url: "IDCard.aspx/GetChireImageFromECM",
                    contentType: "application/json;charset=utf-8",
                    data: '{"contentId":"' + filecontentID + '"}',
                    dataType: 'json',

                    success: function (image) {
                        //debugger
                        console.log('Displaying ChireImage');
                        $('#<%=ImgAssociate.ClientID%>').attr("src", JSON.parse(image.d));
                        $('#<%=hdnImageURL.ClientID%>').val(JSON.parse(image.d));
                    }
                    ,
                    error: function (e) {
                        console.log('ERROR:CHIREFLOW');
                        $('#<%=ImgAssociate.ClientID%>').attr("src", "Images/char.jpeg");
                        $('#<%=hdnImageURL.ClientID%>').val("Images/char.jpeg");

                    }
                });
            }
            else {
                console.log('CHIRE_FILEID_NULL');
                $('#<%=ImgAssociate.ClientID%>').attr("src", "Images/char.jpeg");
                $('#<%=hdnImageURL.ClientID%>').val("Images/char.jpeg");
            }
        }


      

        function imgPrint_onclick() {

            window.print();

            var r = document.getElementById('<%=btnHidden.ClientID %>');
            r.click();
            window.close();

        }
        function lbtnPrint_onclick() {

            window.print();

            var r = document.getElementById('<%=btnHidden.ClientID %>');
            r.click();
            window.close();

        }

        function ReloadParent() {

            window.opener.location.href = window.opener.location.href;
        }

        function RemoveSessions() {
            PageMethods.RemoveSessions();
        }

        function setClipBoardData() {
            setInterval("window.clipboardData.setData('text','')", 20);
        }
        function blockError() {
            window.location.reload(true);
            return true;
        }

        function imgIDCardTop_onclick() {

        }

    </script>
    <link href="App_Themes/styleIDCard.css" rel="stylesheet" type="text/css"/>
    
</head>
<body onunload="RemoveSessions()" onload="setClipBoardData();">
    <form id="form1" runat="server">
    <asp:scriptmanager id="DefaultMasterScriptManager" enablepagemethods="true" enablepartialrendering="true"
        runat="server">
    </asp:scriptmanager>
<%--    <div id="printOption" class="noPrint" style="border-bottom-color: White;">
        
        <img id="imgPrint" title="Print" runat="server" alt="Print ID Card" src="Images/Printer-icon.png"
            height="25" onclick="imgPrint_onclick()" />
        <br />
        <a id="prints" runat="server" href="javascript:imgPrint_onclick()">
            <asp:label id="print" runat="server" text="<%$ Resources:LocalizedText, Print %>"></asp:label></a>
    </div> --%>
    <div class="Print">
        <!--Front ID Start -->
        <div class="IdContainerFront">  
            <div class="assPhoto">
                <asp:image id="ImgAssociate" cssclass="imageBorder" runat="server" imageurl="~/Images/DummyPhoto.png" />
            </div>	
            
            <label id="lblName" runat="server" class="assName" style="padding-top:12px;"></label>
            <label class="assNumber" id="lblFrontID" runat="server"></label>   
                    
            <div class="ctsLogo">
                <img src="Images/CognizantLogo.jpg" alt="CTS Logo" /></div>
            <div class="BotImg">
                <img alt="" src="~/Images/GreenLabel.png" class="TopImg" id="imgIdCardBottom" align="left"
                    runat="server" /></div>
        </div>
        <!--Front ID End -->
    </div>
    <div class="page-break">
    </div>
    <div class="Print">
        <!--Back ID Start -->
        <div class="IdContainerBack">            
            <div class="barCode">
                <asp:image id="imgBarCode" imageurl="~/Barcode.aspx?Code=123456" runat="server" imagealign="Bottom"
                    width="161px" height="35px" />
                <div style="text-align: center; font-size: 8px;">
                    <label id="lblBarcodeAssociateId" runat="server">
                    </label>
                </div>
            </div>
            <div class="assNumber2" style="text-align: center;">
                <label id="lblRearID" runat="server">
                </label>
                <asp:button id="btnHidden" runat="server" style="display: none" text="Button" onclick="Btnhidden_Click" />
            </div>


            <%--<div class="philtop">--%>
             
             <div class="innerDetDiv">				                
				<div class="emergencyDetails" >In Case of Emergency Call :<span> 1800 258 2345</span></div>
				<span class="name"><label id="lblCompanyName" runat="server"></label></span>
				<span class="address" ><label id="lblOfficeAddress" runat="server"></label>, <label id="lblArea" runat="server"></label>,
				<label id="lblCity" runat="server"></label>-<label id="lblPin" runat="server"></label>, <label id="lblCountry" runat="server"></label>.</span>
                <span class="address" style="padding-top:8px;">Tele : <label id="lblTele" runat="server"></label></span>
                 <span class="address" style="border-bottom:2px solid #000000;">
                    Fax : <label id="lblFax" runat="server"></label></span>
			</div>
                 
          <%--  </div>--%>
           
        </div>                
    </div>
        <asp:hiddenfield id="hdnAssociateId" runat="server" />
        <asp:hiddenfield id="hdnFileUploadID" runat="server" />
        <asp:hiddenfield id="hdnImageURL" runat="server" />
        
    
    </form>
</body>
</html>
