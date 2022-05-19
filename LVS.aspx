<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LVS.aspx.cs" Inherits="VMSDev.LVS" MasterPageFile="~/VMSMain.Master"%>



  

<asp:Content ID="VMSEnterInofrmationContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server" >

    <script language="javascript" type ="text/javascript"  >


    //begin LVS19032010CR01

    window.onload = function() {

        var txtasset = document.getElementById('VMSContentPlaceHolder_txtAssetNumber');
        txtasset.focus();
    }


    //end

    function setClipBoardData() {
        setInterval("window.clipboardData.setData('text','')", 20);
    }
    function blockError() {
        window.location.reload(true);
        return true;
    }




    function SpecialCharacterValidation(e) {
        var keycode;
        if (window.event)
            keycode = window.event.keyCode;
        else if (event) keycode = event.keyCode;
        else if (e) keycode = e.which;
        else return true;
        if (keycode > 47 && keycode <= 57) {
            return true;
        }
        else {
            return false;
        }
    }
    function NumberCharacterValidation(e) {
        //  
        var keycode;
        if (window.event)
            keycode = window.event.keyCode;
        else if (event) keycode = event.keyCode;
        else if (e) keycode = e.which;
        else return true;
        if ((keycode > 47 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122)) {
            return true;
        }
        else {
            return false;
        }
    }


</script>
 
<table border="0" cellpadding="0" cellspacing="0" width="100%"  >     
            <tr>
             <td colspan="4" align="center" valign="middle" style="height:100%" >
             <table width="100%" class="border">                     
                 <tr>
                     <td align="left" class="table_header_bg" colspan="9">
                         &nbsp;<asp:Label ID="lblModuleValue" runat="server" CssClass="Table_header" Text="Verify Laptop"></asp:Label></td>
                 </tr>
                 <tr>                 
                     <td style="width: 8%" valign="baseline" align="right">                         
                         <asp:Label ID="lblAssociateID" runat="server" CssClass="field_text" Text="AssociateID :"></asp:Label></td>                         
                     <td align="left" colspan="8" valign="baseline" class="field_txt">
                         &nbsp;<asp:TextBox
                             ID="txtAssociateID" runat="server" CssClass="field_text" maxlength="15" Width="10%"></asp:TextBox>&nbsp;or
                         <%-- <asp:RangeValidator id="AssociateIDrange" runat="server" ControlToValidate="txtAssociateID"  ErrorMessage="Select Native country" ></asp:RangeValidator >
            <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender10" runat="server" TargetControlID="AssociateIDrange"></cc2:ValidatorCalloutExtender> --%>
                         <asp:Label ID="Label6" runat="server" CssClass="field_text" Text="Serial No :"></asp:Label>
                         <asp:TextBox ID="txtSerialNo" runat="server" Width="10%" CssClass="field_text" MaxLength="30"></asp:TextBox>
                         &nbsp;or
                         <asp:Label ID="lblAssetNumber" runat="server" CssClass="field_text" Text="AssetNumber :" ></asp:Label><asp:TextBox
                             ID="txtAssetNumber" runat="server" CssClass="field_text" MaxLength="8" Width="10%"  ></asp:TextBox>
                         <asp:Button ID="btnSearch" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                             ForeColor="White" Height="21px" OnClick="BtnSearch_Click" Text="Search" CausesValidation="False" />&nbsp;
                         <asp:Button ID="btnClear" runat="server" BackColor="#767561" Font-Bold="False" Font-Size="10px"
                             ForeColor="White" Height="21px"  Text="Clear" CausesValidation="False" UseSubmitBehavior="False" OnClick="BtnClear_Click" />&nbsp;&nbsp;
                         &nbsp; &nbsp;&nbsp;
      
                         </td>
                         
                 </tr>
                 <tr>
                     <td align="right" style="width: 8%" valign="baseline">
                     </td>
                     <td id="tdTop"  runat="server" align="left" class="field_txt" colspan="8" valign="baseline">
                        <asp:Label ID="lblCardIssuedLocation" runat="server" CssClass="field_text" 
                             Text="Location :"></asp:Label>&nbsp;<asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DdlLocation_selectedIndexChanged" DataTextField="LocationCity" DataValueField="LocationCity" CssClass="field_text"></asp:DropDownList>&nbsp;
                         <asp:Label ID="lblCardIssuedFacility" runat="server" CssClass="field_text" 
                             Text="Facility:"></asp:Label>&nbsp;<asp:DropDownList ID="ddlFacility" 
                             DataValueField="LocationId" DataTextField="LocationName" 
                             OnSelectedIndexChanged="DdlFacility_selectedIndexChanged" runat="server" 
                             CssClass="field_text" AutoPostBack="true"></asp:DropDownList>
                         <asp:RadioButton ID="rdoCheckIn" runat="server" CssClass="field_txt" OnCheckedChanged="RdoCheckIn_CheckedChanged"
                             Text="Check-in" AutoPostBack="True" />
                         <asp:RadioButton ID="rdoCheckOut" runat="server" CssClass="field_txt"
                             OnCheckedChanged="RdoCheckOut_CheckedChanged" Text="Check-out" AutoPostBack="True" />
                         <asp:Button ID="btnIdentityCard" runat="server" Text="Verify Laptop" BackColor="#767561" Font-Size="10px" Height="21px" Font-Bold="False" ForeColor="White" OnClick = "BtnVerify_Click" UseSubmitBehavior="False" /></td>
                 </tr>
                 </table>
                 <tr>
                     <td colspan="9">
                         <asp:Panel ID="panelEmp" runat="server" Width="100%" CssClass="border">
                         <table id ="tblEmp" cellpadding="0" cellspacing="0" width ="100%" style="height: 304px">
                             <tr>
                                 <td align="left" valign="middle">
                                     &nbsp;<asp:Label ID="lblEmployeeHeader" runat="server" CssClass="Table_header" Text="Employee Details" Font-Size="11px"></asp:Label></td>
                                 <td align="left" style="height: 15px">
                                 </td>
                                 <td align="left" style="height: 15px">
                                 </td>
                                 <td align="left" valign="middle">
                                     &nbsp;<asp:Label ID="lblLaptopHeader" runat="server" CssClass="Table_header" Text="Laptop Details" Font-Size="11px"></asp:Label></td>
                                 <td style="width: 20%">
                                 </td>
                             </tr>
                             <tr>
                                 <td align="right"  style="height: 15px; width: 10%;" class="field_column_bg">
                                     <asp:Label ID="lblEmployeeID" runat="server" CssClass="field_text" Text="ID "></asp:Label>:</td>
                                     <td align="left"  style="height: 15px; width: 15%;">
                                         &nbsp;<asp:Label ID="lblEmpID" runat="server" CssClass="field_text"></asp:Label></td>
                                     <td align="left" rowspan="8" style="width: 15%">
                                         <asp:Image ID="ImgAssociate" runat="server" Height="137px" 
                                             ImageUrl="~/Images/char.jpeg" oncontextmenu="return false" Width="119px" /></td>
                                 <td align="right"  style="height: 15px; width: 10%;" valign="middle" class="field_column_bg">
                                 <asp:Label ID="lblLaptopMake" runat="server" CssClass="field_text" Text="Make"></asp:Label>:</td>
                                 <td style="width: 20%" align="left">
                                     &nbsp;<asp:Label ID="lblMake" runat="server" CssClass="field_text"></asp:Label></td>
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg" style="height: 15px">
                                     <asp:Label ID="lblEmployeeName" runat="server" CssClass="field_text" Text="Name "></asp:Label>:</td>
                                 <td align="left" style="height: 15px">
                                     &nbsp;<asp:Label ID="lblEmpName" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                                     <asp:Label ID="lblLaptopModel" runat="server" CssClass="field_text" Text="Model"></asp:Label>:</td>
                                 <td style="width: 20%" align="left">
                                     &nbsp;<asp:Label ID="lblModel" runat="server" CssClass="field_text"></asp:Label></td>
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg" style="height: 19px">
                                     <asp:Label ID="lblEmailID" runat="server" CssClass="field_text" Text="EmailID "></asp:Label>:</td>
                                 <td align="left" style="height: 19px">
                                     &nbsp;<asp:Label ID="lblEmpEmailID" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="right" class="field_column_bg" style="height: 19px" valign="middle">
                                     <asp:Label ID="lblSerialNo" runat="server" CssClass="field_text" Text="Serial Number"></asp:Label>:</td>
                                 <td style="width: 20%; height: 19px;" align="left">
                                     &nbsp;<asp:Label ID="lblSerial" runat="server" CssClass="field_text"></asp:Label></td>
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg" style="height: 15px">
                                     <asp:Label ID="lblEmpMobile" runat="server" CssClass="field_text" Text="Mobile"></asp:Label>:</td>
                                 <td align="left" style="height: 15px">
                                     &nbsp;<asp:Label ID="lblEmployeeMobile" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                                     <asp:Label ID="lblAsset" runat="server" CssClass="field_text" Text="Asset Number"></asp:Label>:</td>
                                 <td style="width: 20%" align="left">
                                     &nbsp;<asp:Label ID="lblAssetNo" runat="server" CssClass="field_text"></asp:Label></td>
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg" style="height: 15px">
                                     <asp:Label ID="lblEmpExtension" runat="server" CssClass="field_text" Text="Extension"></asp:Label>:</td>
                                 <td align="left" style="height: 15px">
                                     <asp:Label ID="lblEmployeeExtension" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="right" class="field_column_bg" style="height: 15px" valign="middle">
                                     <asp:Label ID="lblIssueDate" runat="server" CssClass="field_text" Text="Date Issued(mm/dd/yyyy)"></asp:Label>:</td>
                                 <td align="left" style="width: 20%">
                                 <asp:Label ID="lblDateIssued" runat="server" CssClass="field_text"></asp:Label></td>
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg" style="height: 15px">
                                     <asp:Label ID="lblLocation" runat="server" CssClass="field_text" Text="Location "></asp:Label>:</td>
                                 <td align="left" style="height: 15px">
                                     &nbsp;<asp:Label ID="lblEmpLocation" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="left" style="height: 15px" valign="middle">
                                 </td>
                                 <td style="width: 20%">
                                 </td>
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg" style="height: 15px">
                                     <asp:Label ID="lblCountry" runat="server" CssClass="field_text" Text="Country "></asp:Label>:</td>
                                 <td align="left" style="height: 15px">
                                     &nbsp;<asp:Label ID="lblEmpCountry" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="left" style="height: 15px" valign="middle">
                                 </td>
                                 <td style="width: 20%">
                                 </td>
                             </tr>
                             <tr>
                                 <td align="right" class="field_column_bg" style="height: 15px">
                                     <asp:Label ID="lblDepartment" runat="server" CssClass="field_text" Text="Department "></asp:Label>:</td>
                                 <td align="left" style="height: 15px">
                                     &nbsp;<asp:Label ID="lblEmpDepartment" runat="server" CssClass="field_text"></asp:Label></td>
                                 <td align="left" style="height: 15px" valign="middle">
                                 </td>
                                 <td style="width: 20%">
                                 </td>
                             </tr>
             </table>
                             &nbsp;&nbsp;
           </asp:Panel>
                         </td>
                 </tr>
       </table>
                 
      <hr/>

            
           
                 <table width=100%  >
                 <tr>
                  
                 <td align="right" class="style1" valign="top" width="50">
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                     <br />
                  <asp:Label ID="txtMemeberID" runat="server" CssClass="Table_header"></asp:Label>
                  &nbsp;&nbsp;&nbsp;
                                     
                   </td>
                     <td align="left" valign="top" width="200">
                         <br />
                         <br />
                  <asp:Label ID="txtMembername" runat="server" CssClass="Table_header"></asp:Label>
                   
                     </td>
                   <td class="style2">
                 
                   <asp:Image ID="ImgMember" runat="server" Height="109px" 
                           ImageUrl="~/Images/char.jpeg" oncontextmenu="return false" Width="107px" /></td>
             
                   </td>
                   
                   
                  
                   
                  
                   <td id="tdmember" align="left" class="field_txt" valign="middle"
                         runat="server" visible=false>
                        <asp:Label ID="Label1" runat="server" CssClass="field_text" 
                             Text="Location :"></asp:Label>&nbsp;<asp:DropDownList 
                            ID="ddlLocationbottom" runat="server" AutoPostBack="true" 
                            OnSelectedIndexChanged="DdlLocation_selectedIndexChanged" 
                            DataTextField="LocationCity" DataValueField="LocationCity" 
                            CssClass="field_text"></asp:DropDownList>&nbsp;
                            
                            
                             
                            
                            
                            
                            
                            
                         <asp:Label ID="Label2" runat="server" CssClass="field_text" 
                             Text="Facility:"></asp:Label>&nbsp;<asp:DropDownList ID="ddlFacilitybottom" 
                             DataValueField="LocationId" DataTextField="LocationName" 
                             OnSelectedIndexChanged="DdlFacility_selectedIndexChanged" runat="server" 
                             CssClass="field_text" AutoPostBack="true"></asp:DropDownList>
                         <asp:RadioButton ID="rdobottomIN" runat="server" CssClass="field_txt" 
                             Text="Check-in" AutoPostBack="True" 
                            oncheckedchanged="RdobottomIN_CheckedChanged" />
                         <asp:RadioButton ID="rdobottomOUT" runat="server" CssClass="field_txt"
                              Text="Check-out" 
                            AutoPostBack="True" oncheckedchanged="RdobottomOUT_CheckedChanged" />
                         <asp:Button ID="btnVerify" runat="server" Text="Verify Laptop" 
                            BackColor="#767561" Font-Size="10px" Height="21px" Font-Bold="False" 
                            ForeColor="White" OnClick = "BtnVerify_Click" UseSubmitBehavior="False" /></td>
                  
                   
                   </tr>
                  
                    </table>  
                    
                    <br />      
                <table border="0" id="Table1" cellpadding="0" cellspacing ="0" runat="server" 
              >
                 <tr>
             <td colspan="6"  align="left" style="border-right: black 1pt solid; border-top: black 1pt solid; border-left: black 1pt solid; border-bottom: black 1pt solid;background-color: #edf6fd; width: 60%; height: 19px;" id="errortbl" visible="false">
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <asp:Label ID="lblMessage" runat="server" CssClass="field_text" ForeColor="Red"></asp:Label>&nbsp;
                           
             </td>
                 </tr>
           </table>
            <asp:Label ID="lblSuccessMessage" runat="server" CssClass="Table_header"></asp:Label>
           
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style1
        {
            width: 173px;
        }
        .style2
        {
            width: 139px;
        }
    </style>
</asp:Content>
