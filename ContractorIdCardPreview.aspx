<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractorIdCardPreview.aspx.cs"
    Inherits="VMSDev.ContractorIdCardPreview" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="App_Themes/jquery.rotate-1.0.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('div.rotation').jqrotate(-90);
            $('div.invert').jqrotate(-180);
            //  $('div.CardPrint').jqrotate(-180);
            // $('div.port').jqrotate(-90);

        });

        function printDiv(divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
            window.close();
        }

        function imgPrint_onclick() {
            window.print();
            window.close();
        }     

    </script>
    <link href="App_Themes/stylesContract.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        
    </style>
    <style type="text/css">
        @media all
        {
            .page-break
            {
                display: none;
            }
        }                  
        
        @page { size : portrait !important;
               
        }      
              
        @media print
        {
        

        .page
        {
     -webkit-transform: rotate(-90deg); -moz-transform:rotate(-90deg);
     filter:progid:DXImageTransform.Microsoft.BasicImage(rotation=3);
       }
                 .page-break
            {
                display: block;
                page-break-before: always;
            }
        
            .noPrint
            {
                display: none;
            }
            .Print
            {
                display: table;
                  
            }
            
            #myFooter, #myHeader
            {    display: none;
                 
            }
            
            
            
            
             .content_contract{ display:none;}
 .content .align-center,.A4{ display:none;}
 .btns,.nav-left,.nav-right,.closebtn,.divid,.cont-btn,.strip img,.assc-photo img,.printcaption{ display:none;}
.id-card,.id-card{ display:block !important;}
.nav-cont table.tb1-main,.nav-cont table td.border-bot{ vertical-align:middle !important;text-align:center; border:none !important;}

        }
     
    </style>
    <style type="text/css">
        /*General Class Start*/
        *
        {
            margin: 0;
            padding: 0;
        }
        
        
        .IdContainerFront
        {
            height: 150px;
            width: 194px;
            border: 0px;
            margin: auto;
            position: relative;
            margin-bottom: 2px;
            padding: 0 5px 0 5px;
            font: normal 10px Arial Narrow;
        }
        .IdContainerBack
        {
            height: 130px;
            width: 194px;
            border: 0px;
            margin: auto;
            position: relative;
            margin-bottom: 2px;
            padding: 0 5px 0 5px;
            font: normal 10px Arial Narrow;
        }
    </style>
</head>
<body style="background-color: White; border-bottom-color: white">
    <form id="form2" runat="server">
    <div id="printOption" class="noPrint" style="border-bottom-color: White;">
        <a href="javascript:printDiv('CardPrint')">
            <asp:Label ID="print" runat="server" Text="<%$ Resources:LocalizedText, Print %>"></asp:Label></a><br />
    </div>
    <div id="CardPrint">
        <div class="Print">
            <!--Front ID Start -->
            <div class="IdContainerFront port">
                <div class="id-card" style="padding-left: 5px;">
                    <div style="height: 38px;" class="strip">
                    </div>
                    <div class="front" style="text-align: center">
                        <div class="uniq-id rotation font-fix">
                            <asp:Label ID="lblContratorId" runat="server"></asp:Label></div>
                        <div class="assc-photo" style="height: 150px;">
                            <div class="famil-arial font-bold size-18pt txt-right">
                                <asp:Label ID="lblValid" runat="server"></asp:Label></div>
                            <img src="Images/DummyPhoto.png" />
                        </div>
                        <div>
                            <div class="famil-arial-nar font-bold size-14pt">
                                <asp:Label ID="lblContratcorName" runat="server"> </asp:Label>
                                <asp:HiddenField ID="hdnContratorId" runat="server" />
                                <div class="famil-arial-nar font-bold size-12pt">
                                    <asp:Label ID="lblVendorName" runat="server"></asp:Label></div>
                            </div>
                        </div>
                    </div>
                    <div style="height: 38px;" class="strip">
                    </div>
                </div>
            </div>
        </div>
        <div class="Print">
            <div class="IdContainerBack invert">
                <div class="id-card" style="padding: 0 0px;">
                    <div class="famil-arial-nar size-13pt font-bold font-fix" style="text-align: center">
                        <asp:Label ID="lblVendorName1" runat="server"></asp:Label></div>
                    <div class=" mar-5 font-fix">
                        <span class="size-8.5pt famil-arial-nar font-fix">Supervisor's Cell # </span><span class="size-8.5pt famil-arial-nar font-bold font-fix">
                            <asp:Label CssClass="bold-14 font-fix" ID="lblSupervisiorCell" runat="server"> </asp:Label></span></div>
                    <div>
                        <span class="size-8.5pt famil-arial-nar font-fix">Service Provider #</span> <span class="size-8.5pt famil-arial-nar font-bold font-fix">
                            <asp:Label ID="lblServiceProviderCell" runat="server"></asp:Label></span></div>
                    <div class=" mar-3 tele font-fix" style="white-space: nowrap">
                        <span class="famil-arial-nar size-10pt mar-3 font-fix">If found,Please inform :&nbsp; </span>
                        <span class="famil-arial-nar font-bold size-9pt font-fix">1800 258 2345</span></div>
                    <div class="font-bold famil-arial-nar size-10pt mar-3 font-fix">
                        <span class=" famil-arial-nar size-10pt mar-3 font-fix">Valid upto </span>&nbsp;
                        <asp:Label ID="lblValidUpTo" runat="server"></asp:Label></div>
                    <div class=" famil-arial-nar size-10pt mar-3 font-fix">
                        Entry Permit for:</div>
                    <div class=" famil-arial-nar size-10pt font-bold mar-3 font-fix">
                        <span style="white-space: nowrap">Cognizant Technology Solutions </span>
                        <br />
                        India Pvt. Ltd.</div>
                    <div class=" famil-arial-nar size-8pt font-fix">
                        <asp:Label ID="lblFacilityAddress" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
