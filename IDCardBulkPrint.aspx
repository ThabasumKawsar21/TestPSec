<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IDCardBulkPrint.aspx.cs"
    Inherits="VMSDev.IDCardBulkPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cognizant ID Card Template</title>

    <style type="text/css"> 

@page :left {
    margin-left: 0in;
    margin-right: 0in;
}
@page :right {
    margin-left: 0in;
    margin-right: 0in;
}
</style>
    <style type="text/css">
        /*General Class Start*/
        *
        {
            padding: 0;
            margin-right: 0;
            margin-top: 0;
            margin-bottom: 0;
        }
        
        
        .IdContainerFront
        {
            height: 330px;
            width: 194px;
            border: 0px solid red;
            margin: auto;
            position: relative;
            margin-bottom: 0px;
            padding: 0 5px 0 5px;
            font: normal 10px Arial Narrow;
        }
        .IdContainerBack
        {
            height: 325px;
            width: 194px;
            border: 0px solid red;
            margin: auto;
            position: relative;
            margin-bottom: 0px;
            padding: 0 5px 0 5px;
            font: normal 10px Arial Narrow;
        }
        .TopImg
        {
            width: 145px;
            margin: auto;
        }
        .labelImg
        {
            width: 189px;
            margin: auto;
        }
        .BotImg
        {
            width: 145px;
            margin: auto;
            position: absolute;
            bottom: 0;
            left: 30px;
            right: 30px;
        }
        .marTop5
        {
            margin-top: 5px;
        }
        .marTop10
        {
            margin-top: 2px;
            margin-left: 8px;
        }
        .marTop15
        {
            margin-top: 15px;
            margin-left: 8px;
        }
        .marTop16
        {
            margin-left: 8px;
        }
        
        .boldText
        {
            font-weight: bold;
        }
        /*General Class End*/
        
        /*Front ID Card Class Start*/
        .issueYear
        {
            color: #9B9B9B;
            font-size: 32px;
            font-weight: bold;
            margin: 10px 0 0 10px;
        }
        .assPhoto
        {
            width: 107px;
            height: 107px;
            margin: 6px auto 0 auto;
            vertical-align: middle;
            text-align: center;
        }
        .imageBorder
        {
            border: 1px solid black !important;
        }
        .assName, .assNumber
        {
            text-align: center;
            font: bold 17px arial narrow;
            margin-top: 2px;
        }
        .ctsLogo
        {
            height: 38px;
            width: 170px;
            padding-top: 1px;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 0;
        }
        .ctsLogo img
        {
            width: 135px;
            margin: auto;
            display: block;
        }
        .IdContainerFront label
        {
            display: block;
        }
        .IdContainerFront label span
        {
            font-weight: bold;
        }
        /*Front ID Card Class End*/
        
        /*Back ID Card Class Start*/
        .barCode
        {
            height: 50px;
            border: 0px solid red;
            margin: 30px 0px 0px 0px;
            text-align: left;
        }
        .assNumber2
        {
            font: bold 12pt arial narrow;
            margin-top: 18px;
            margin-left: 8px;
        }
        .bgspace
        {
            height: px;
        }
        .inform
        {
            font-weight: bold;
            padding-top: 5px;
            text-align: justify;
            height: 20px;
            margin-left: 8px;
        }
        .inform label
        {
            font-size: 8pt;
        }
        .inform label span
        {
            font-size: 10pt;
        }
        .contactCts
        {
            font-size: 5.5pt;
            margin-top: 15px;
            margin-left: 8px;
        }
        .contactCts label
        {
            display: block;
        }
        .contactCts span
        {
            font-weight: bold;
        }
        /*Back ID Card Class End*/
    </style>
    <style type="text/css">
        @media all
        {
            .page-break
            {
                display: none;
            }
        }
        
        @media print
        {
            .page-break
            {
                display: block;
                page-break-before: always;
                page-break-after:avoid;
            }
        
            .noPrint
            {
                display: none;
            }
            .Print
            {
                display: table;
            }
            
            
            @page :left {
                margin-left: 0 !important;
                margin-right:0 !important;
            }

            @page :right {
                margin-left: 0 !important;
                margin-right: 0 !important;
            }

            
            #myFooter, #myHeader
            {    display: none;
                 
            }
            
            #wrapper, #content {	     
	        margin: 0 0 0 0 ;
	        padding: 0; 
	        border: 0;
	        float: none !important;
	
	}

            
        }
        .Smallconstants
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            line-height: 12px;
            margin-left: 0px;
        }
        
        .Rowstyle
        {
            width: 330px;
            vertical-align: top;
            line-height: 11px;
        }
        .constantNamelabelbold
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 12px;
            width: 50px;
            line-height: 12px;
        }
        .constantlabel
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            width: 50px;
            line-height: 12px;
        }
        .constantlabelbold
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 11px;
            width: 50px;
            line-height: 12px;
        }
        .Variables_TD
        {
            vertical-align: top;
            line-height: 12px;
        }
        .Rowstyle2
        {
            width: 330px;
            vertical-align: top;
            line-height: 12px;
        }
        
        
        .Notestyle
        {
            text-align: left;
            vertical-align: top;
            font-family: Interstate-Regular;
            font-size: 9px;
            line-height: 12px;
        }
        
        #form2
        {
            height: 330px;
        }
        .IDFontBold
        {
            font-family: Arial Narrow;
            font-weight: bold;
        }
        .IDFontNormal
        {
            font-family: Arial Narrow;
            font-weight: normal;
        }
        .style1
        {
            width: 269px;
        }
        .style2
        {
            width: 72px;
        }
           .styleImage
        {
            max-height: 150px;
            width: 200px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function imgPrint_onclick() {
             
            window.print();
        }
        function ReloadParent() {
            window.opener.location.href = window.opener.location.href;
        }

     
    </script>

    <link href="App_Themes/print.css" media="print" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager id="DefaultMasterScriptManager" enablepagemethods="true" enablepartialrendering="true"
        runat="server">
    </asp:scriptmanager>
    <div id="printOption" class="noPrint" style="border-bottom-color: White;">
        <%-- <a href="javascript:window.print()">--%>
        <img id="imgPrint" alt="Print ID Card" src="Images/Printer-icon.png" height="25px"
            onclick="imgPrint_onclick()" />
        <asp:imagebutton id="imgPrev" imageurl="~/Images/arrow_bar.png" width="15px" height="15px"
            runat="server" onclick="ImgPrev_Click" tooltip="Prev Associate Details" />&nbsp;
        <asp:imagebutton id="imgNext" imageurl="~/Images/arrow_bar_right.png" width="15px"
            height="15px" runat="server" onclick="ImgNext_Click" tooltip="Next Associate Details" />
        <input type="hidden" id="PageSize" name="PageSize" value="1" runat="server" />
        <input type="hidden" id="CurrentPage" name="CurrentPage" value="1" runat="server" />
        <input type="hidden" id="TotalSize" name="TotalSize" runat="server" />
        <%-- </a><asp:Button ID="Button1" 
            runat="server" onclick="Button1_Click" Text="Back Side" BackColor="#666633" ForeColor="White"/>--%>
    </div>
    <div class="Print">
        <!--Front ID Start -->
        <div class="IdContainerFront">
            <div class="TopImg">
                <img alt="" id="imgIDCardTop" runat="server" src="Images/Chennai/CardTop.jpg" class="TopImg" />
            </div>
            <div class="labelImg">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="style1">
                            <label id="lblyear" runat="server" class="issueYear">
                            </label>
                        </td>
                        <td align="right" style="padding-top: 0px" class="style2">
                            <img alt="" id="imgholo" runat="server" src="Images/hologram_circle2.png" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="assPhoto">
                <asp:image id="ImgAssociate" cssclass="imageBorder" height="100%" width="100%" runat="server"
                    imageurl="~/Images/DummyPhoto.png" />
            </div>
            <label id="lblName" runat="server" class="assName">
            </label>
            <label class="assNumber" id="lblFrontID" runat="server">
            </label>
            <div class="marTop10">
                <img alt="" id="imgFrontLine" runat="server" src="Images/Chennai/line.jpg" width="180"
                    height="2"/></div>
            <div class="ctsLogo">
                <img src="Images/IDCard_CognizantLogo.png" alt="CTS Logo" height="38" /></div>
            <div class="BotImg">
                <img alt="" src="Images/Chennai/IsAssociate.jpg" class="TopImg" id="imgIdCardBottom"
                    runat="server" /></div>
        </div>
        <!--Front ID End -->
    </div>
    <div class="page-break">
    </div>
    <div class="Print">
        <!--Back ID Start -->
        <div class="IdContainerBack">
            <div class="TopImg">
                <img alt="" id="imgIDRearTop" runat="server" src="Images/Chennai/CardTop-1.png" class="TopImg" /></div>
            <div class="barCode">
                <%--    <label id="barcode" runat="server" style="font-size:30pt; line-height:50px;  font-family: 3 of 9 Barcode;">
                    *2 2 1 1 0 7*</label>--%>
                <asp:image id="imgBarCode" imageurl="~/Barcode.aspx?Code=123456" runat="server" imagealign="Bottom"
                     width="161px" height="35px"/>
        <div style="text-align: center; font-size: 8px;">
                    <label id="lblBarcodeAssociateId" runat="server">
                    </label>
                </div>          
            </div>
            <%--<div class="marTop10">
                <label>
                    Emergency contact #
                </label>
                <label id="lblEmergencyContact" runat="server" class="boldText">
                    <%# Eval("EmergencyContact")%></label>
            </div>
            <div class="marTop10">
                <label>
                    Blood Group
                </label>
                <label id="lblBloodGroup" runat="server" class="boldText">
                    <%# Eval("BloodGroup")%>
                </label>--%>
                
        <div class="assNumber2" style="text-align: center; width: 178px;">
                <label id="lblRearID" runat="server" >
            <%--<%# Eval("AssociateID")%>--%></label>
            </div>
                
            </div>
            <%--<div class="marTop15">
                <img id="imgRearLine1" runat="server" alt="" src="Images/Kolkata/line.jpg" width="185"
                    height="2" /></div>
            <div class="inform">
                <label>
                    If found, please inform: <span>1800 200 2345</span></label>
            </div>
            <div class="marTop16">
                <img id="imgRearLine2" runat="server" alt="" src="Images/Kolkata/line.jpg" width="185"
                    height="2" /></div>
            <div class="contactCts">
                <asp:label id="lblCognizant" cssclass="boldText" font-size="6.5pt" width="125px"
                    font-bold="true" runat="server" text="Cognizant Technology Solutions India Pvt. Ltd"
                    style="text-align: Left; vertical-align: top;"></asp:label><br />
                <asp:label id="lblFacilityAddress" cssclass="marTop5" font-size="6.5" width="180px"
                    runat="server" text="Address"></asp:label>          
            </div>
            <div class="BotImg">
                <img id="imgIDRearBottom" runat="server" alt="" src="Images/Kolkata/CardBottom.jpg"
                    class="TopImg" /></div>--%>
                    <asp:hiddenfield id="hdnAssociateId" runat="server" />
        </div>
        <!--Back ID End -->
    <%--</div>--%>
    </form>
</body>
</html>
