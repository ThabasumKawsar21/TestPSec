<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractorPreview.aspx.cs"
    Inherits="VMSDev.ContractorPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title></title>   
     <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />
  
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>


    <link href="App_Themes/printcardstyles-1.0.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jQueryRotate.js" type="text/javascript"></script>
</head>
<body>
    <form id="form2" runat="server">
    <a href="javascript:imgPrint_onclick()" class="noPrint">Print </a>

    <div class="id-card " style="width: 153px; height: 247px; position: relative;">

        <div style="height: 30px;">
        </div>
       
        <div class="front" style="height: 188px;">
            <div id="rotate" class="rotation" style="position: absolute; top: 87px; left: -19px;
                font-size: 12pt; font-family: 'Times New Roman', Times, serif; width: 70px; color: #fe0000;
                background: #fff;">
                <asp:label id="lblContratorId" runat="server"></asp:label>
            </div>
          
            <div class="famil-arial font-bold size-18pt txt-right" style="height: 31px; text-align: right;
                padding-top: 5px;">
                <div style="width: 90px; float: left; text-align: left;">
                    <asp:label runat="server" id="lblCWRID" Text="GVCGUVI0001" style="color: red; font-size: 9pt; font-family: Times New Roman;
                        color: #fe0000; margin-left:7px;"></asp:label>
                </div>
                <div style="float: left; width: 60px;">
                    <asp:label id="lblValid" runat="server"></asp:label>
                </div>
            </div>
            <div class="assc-photo" style="height: 120px; width: 95px; margin: 0 auto;">
                <%--  <div style="height:165px; width:145px;" class="noPrint">--%>
                <div style="height: 115px; width: 95px;">
                    <%--  <img src="Images/DummyPhoto.png" />--%>
                    <img src="images/sampleasscimg.png" />
                </div>
            </div>
            <%--  <div style="height:65px;">  --%>
            <div style="height: 50px;">
                <%-- <div class="famil-arial-nar font-bold size-16pt" style="height:25px; text-align:center;">--%>
                <div class="famil-arial-nar font-bold size-11pt" style="height: 19px; text-align: center;">
                    <asp:label id="lblContratcorName" runat="server"></asp:label>
                    <asp:hiddenfield id="hdnContratorId" runat="server" />
                </div>
                <%-- <div class="famil-arial-nar font-bold size-15pt" style="height:25px;text-align:center;">--%>
                <div class="famil-arial-nar font-bold size-10pt" style="height: 29px; text-align: center;">
                    <asp:label id="lblVendorName" runat="server"></asp:label>
                </div>
            </div>
        </div>
        <!--   Space for Bottom  Band-->
        <%-- <div style="height:55px; ">--%>
        <div style="height: 30px;">
        </div>
        <!--   Space for Bottom Band-->
    </div>
    <!--   Front side of id Card-End-->
    <!--   Back side of id Card-Start-->
    <%--    
    <div class="id-card  invert" style="width:250px; height:317px; padding:20px 12px; font-family:'Times New Roman', Times, serif;">
      
          <div class="famil-arial-nar size-17pt font-bold txtcenter cleartext" >--%>
    <%--         changed padding from 25px to 65--%>
    <div id="idcard" class="id-card  invert" style="width: 153px; height: 227px; padding: 65px 2px;
        font-family: 'Times New Roman', Times, serif;">
        <div class="famil-arial-nar size-17pt font-bold txtcenter cleartext">
            <asp:label id="lblVendorName1" runat="server"></asp:label>
        </div>
        <%--  <div class=" mar-5">
            <span class="size-12pt famil-arial-nar txtcenter cleartext">--%>
        <div class=" mar-5" style="padding-left: 5px">
            <span class="size-12pt famil-arial-nar txtcenter cleartext">Supervisor's Cell #
            </span><span class="size-13pt famil-arial-nar font-bold txtcenter cleartext">
                <asp:label id="lblSupervisiorCell" runat="server"></asp:label>
            </span>
        </div>
        <%-- <div  >--%>
        <div style="padding-left: 5px">
            <span class="size-12pt famil-arial-nar txtcenter cleartext ">Service Provider #
            </span><span class="size-13pt famil-arial-nar font-bold txtcenter cleartext">
                <asp:label id="lblServiceProviderCell" runat="server"></asp:label>
            </span>
        </div>
        <%--  <div  class=" mar-5 tele">--%>
        <div class=" mar-5 tele" style="padding-left: 2px">
            <%--<span class="famil-arial-nar font-bold size-10pt txtcenter cleartext">--%>
            <span class="famil-arial-nar  size-10pt txtcenter cleartext" style="font-size: 9px">
                Tele # to call if this card is found: 1800 258 2345 </span>
        </div>
        <div class="font-bold famil-arial-nar size-14pt mar-5 txtcenter cleartext">
            Valid Upto &nbsp;
            <asp:label id="lblValidUpTo" runat="server"></asp:label>
        </div>
        <div class=" famil-arial-nar size-12pt mar-5 txtcenter cleartext">
            Entry Permit for:
        </div>
        <div class=" famil-arial-nar size-12pt font-bold mar-5 txtcenter cleartext">
            <span>Cognizant Technology Solutions India Pvt. Ltd. </span>
        </div>
        <%-- <div class=" famil-arial-nar size-12pt txtcenter cleartext" >--%>
        <div class=" famil-arial-nar size-12pt txtcenter cleartext" style="height: 35px;">
            <asp:label id="lblFacilityAddress" runat="server"></asp:label>
        </div>
        <div class=" famil-arial-nar size-12pt txtcenter cleartext">
        </div>
        <div class=" famil-arial-nar size-12pt txtcenter cleartext">
        </div>
    </div>
    <!--   Back side of id Card-Start-->
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            //commented for rotate issue in ie9
            //            $('.rotation').jqrotate(-90);
            //            $('.id-card.invert').jqrotate(-180);


            $("#rotate").rotate(-90);
            $("#idcard").rotate(180);

      
            

        });
    
    
    </script>
    <script language="javascript" type="text/javascript">
        function imgPrint_onclick() {
            window.print();
            window.close();
        }  
    </script>
</body>
</html>
