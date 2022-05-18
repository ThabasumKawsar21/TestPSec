<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractorPreviewLandscape.aspx.cs" Inherits="VMSDev.ContractorProviewLandscape" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="App_Themes/jquery.rotate-1.0.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            height: 22px;
            width: 156px;
        }
    </style>
        <link href="App_Themes/stylesContract.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         .noPrint
            {
       
                text-align:right;}
        @media all
        {
            .page-break
            {
                display: none;
            }
        }                  
        
        @page { 
               
        }      
              
        @media print
        {
        

     
                 .page-break
            {
                display: block;
                page-break-before: always;
            }
        
            .noPrint
            {
                display: none;
                text-align:right;
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
.id-card{ display:block !important;width:204px ; height:269px; margin:0 auto; font-family:Arial; color:#000;   word-wrap: break-word; }

.nav-cont table.tb1-main,.nav-cont table td.border-bot{ vertical-align:middle !important;text-align:center; border:none !important;}

        }
     
    </style>
     <script type="text/javascript">
        $(document).ready(function () {
          //  $('div.rotation').jqrotate(90);
           $('div.invert').jqrotate(-270);
           $('div.rotate90').jqrotate(-90);
            
         //   $('div.IdContainerFront').jqrotate(-90);
           // $('div.IdContainerBack').jqrotate(270);
            
            //  $('div.CardPrint').jqrotate(-180);
            // $('div.port').jqrotate(-90);

       });

       function imgPrint_onclick() {
           window.print();
           window.close();
       }  
        </script>
</head>
<body style="background-color: White; border-bottom-color: white">
    <form id="form1" runat="server">
    
    <div class="PrintDiv" >     
     <div class="id-card rotate90" style="text-align:center;float:left" >
                    <div style="height: 34px;" class="strip">
                    </div>
                    <div class="front" style="text-align: center; position:static;">
                       <div class="uniq-id rotation">
    <asp:Label ID="lblContratorId" runat="server"></asp:Label></div>
                        <div class="assc-photo" style="height:140px;">
                            <div class="famil-arial font-bold size-18pt txt-right">
                                <asp:Label ID="lblValid" runat="server" CssClass="famil-arial font-bold size-18pt txt-right"></asp:Label></div>
                           <%-- <img src="Images/DummyPhoto.png" />--%>
                        </div>
                        <div>
                            <div class="famil-arial-nar font-bold size-14pt">
                                <asp:Label ID="lblContratcorName" runat="server" CssClass="famil-arial-nar font-bold size-14pt"> </asp:Label>
                                 </div>
                                <div class="famil-arial-nar font-bold size-12pt">
                                    <asp:Label ID="lblVendorName" runat="server" CssClass="famil-arial-nar font-bold size-12pt"></asp:Label></div>                           
                        </div>
                    </div>
                    <div style="height: 31px;" class="strip">
                    </div>
                </div>                                    
    <div class="id-card invert" style="text-align:center;float:left;margin-left:80px">
                    <div class="famil-arial-nar size-13pt font-bold" style="text-align: center">
                        <asp:Label ID="lblVendorName1" runat="server" CssClass="famil-arial-nar size-13pt font-bold"></asp:Label></div>
                    <div class=" mar-5" >
                        <span class="size-8-5pt famil-arial-nar">Supervisor's Cell # </span><span class="size-8-5pt famil-arial-nar font-bold">
                        <asp:Label CssClass="size-8-5pt famil-arial-nar font-bold" ID="lblSupervisiorCell" runat="server"> </asp:Label></span></div>
                    <div >
                        <span class="size-8-5pt famil-arial-nar">Service Provider #</span> <span class="size-8-5pt famil-arial-nar font-bold">
                            <asp:Label ID="lblServiceProviderCell" runat="server" CssClass="size-8-5pt famil-arial-nar font-bold"></asp:Label></span></div>
                    <div class=" mar-3 tele " style="white-space: nowrap">
                        <span class="famil-arial-nar size-10pt mar-3">If found,Please inform :&nbsp; </span>
                        <span class="famil-arial-nar font-bold size-9pt">1800 258 2345</span></div>
                    <div class="font-bold famil-arial-nar size-10pt mar-3">
                        <span class=" famil-arial-nar size-10pt mar-3">Valid upto </span>&nbsp;
                        <asp:Label ID="lblValidUpTo" runat="server" CssClass="famil-arial-nar size-10pt mar-3"></asp:Label></div>
                    <div class="famil-arial-nar size-10pt mar-3">
                        Entry Permit for:</div>
                    <div class=" famil-arial-nar size-10pt font-bold mar-3">
                        <span>Cognizant Technology Solutions India Pvt. Ltd.</span>   </div>
                    <div class=" famil-arial-nar size-8-5pt">
                        <asp:Label ID="lblFacilityAddress" runat="server" CssClass="famil-arial-nar size-8-5pt"></asp:Label>
                    </div>
                </div>
       
   
           
      
    </div>
    <div id="printOption" class="noPrint" style="border-bottom-color: White; ">
        <a href="javascript:imgPrint_onclick()" ><asp:label Id="print" runat="server" Text="<%$ Resources:LocalizedText, Print %>"></asp:label></a><br />
       <asp:HiddenField ID="hdnContratorId" runat="server" />
    </div>
    </form>
</body>
</html>
