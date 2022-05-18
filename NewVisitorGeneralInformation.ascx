<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewVisitorGeneralInformation.ascx.cs"
    Inherits="VMSDev.UserControls.NewVisitorGeneralInformation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<script language="javascript" type="text/javascript">
    //************************To allow only Alpha characters in text box fields******************************
    function allowAlpha(ie, moz) {

        if (moz != null) {
            //alert(moz);
            if ((moz == 32) || (moz >= 65) && (moz < 91) || moz == 8 || moz == 13 || (moz >= 97) && (moz < 123)) {
                return true;
            }
            else {
                return false;
            }
        }
        else {

            if ((ie == 32) || (ie >= 65) && (ie < 91) || (ie >= 97) && (ie < 123)) {
                return true;
            }
            else {
                return false;
            }
        }

    }

    function allowNoForMobile(ie, moz) {
        if (moz != null) {
            if (((moz >= 48) && (moz < 58)) || moz == 45) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            if (((ie >= 48) && (ie < 58)) || ie == 45) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    function OpenWindow(flname) {
        var args = new Object();
        args.window = window;
        var dialogresult = window.showModalDialog(flname, "Preview", 'height=300,width=600,top=20,left=0,scrollbars=no,resizable=no,location=no');
        if (dialogresult != null) {
            __doPostBack('PostBackFromDialog', dialogresult);
        }

    }

</script>
<table class="tblHeadStyle">
    <tr>
        <%-- // added for CR IRVMS22062010CR07  starts here done by Priti   --%>
        <td colspan="1" align="left">
            <asp:label id="lblVisitorInfo" runat="server" cssclass="lblHeada" text="<%$ Resources:LocalizedText, VisitorInformation %>">
            </asp:label>
        </td>
        <td colspan="1" align="right">
            &nbsp;</td>
        <td colspan="1" align="left">
            &nbsp;</td>
        <%-- //for testing defects VMS_CR06_08 of IRVMS22062010CR07--%>
        <td colspan="3">
            &nbsp;</td>
        <%-- // end addition for CR IRVMS22062010CR07  starts here done by Priti   --%>
        <td colspan="2" align="right">
            <asp:label id="lblRequired" runat="server" cssclass="lblRequired" text="*"></asp:label>&nbsp;
            <asp:label id="lblIndication" runat="server" cssclass="lblIndication" text="<%$ Resources:LocalizedText, MandatoryField %>">
            </asp:label>
        </td>
    </tr>
    <tr>
        <td rowspan="3">
            &nbsp;</td>
        <td align="right" class="tdBold">
            <asp:label id="lblFirstName" runat="server" cssclass="lblField" text="Name">
            </asp:label>
            <asp:label id="lblRequiredTitle" runat="server" cssclass="lblRequired" text="*"></asp:label>
        </td>
        <td align="left">
            <asp:textbox id="txtFirstName" runat="server" cssclass="txtField" maxlength="50"
                width="130px">
            </asp:textbox>
            <asp:requiredfieldvalidator id="RequiredFN" runat="server" controltovalidate="txtFirstName"
                errormessage="Enter Name" display="None">
            </asp:requiredfieldvalidator>
            <cc2:validatorcalloutextender id="ValidatorCalloutExtender2" runat="server" targetcontrolid="RequiredFN">
            </cc2:validatorcalloutextender>
        </td>
         <td align="right" class="tdBold">
            <asp:label id="lblCompany" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, Company %>"></asp:label>
            <asp:label id="lblRequiredFieldCompany" runat="server" cssclass="lblRequired"  Text="*">
            </asp:label>
        </td>
        <td align="left">
            <asp:textbox id="txtCompany" runat="server" cssclass="txtField" maxlength="50" width="130px">
            </asp:textbox>
         </td>
         <td align="left" class="tdBold" style="width: 150px;">
            <asp:label id="lblMobile" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, Mobile %>"></asp:label>
             <asp:Label ID="lblRequiredMobile" runat="server" CssClass="lblRequired" Text="*"></asp:Label>
        </td>
         <td align="left">
            <asp:textbox id="txtCountryCode" runat="server" cssclass="txtField" maxlength="4"
                type="CountryCode" onkeypress="return allowNoForMobile(event.keyCode, event.which);"
                tooltip="0001" width="26px" style="position:relative; left:-102px;" enabled="True">
            </asp:textbox>
            </td>
        <td align="right">
        <asp:textbox id="txtMobileNo" runat="server" cssclass="txtField" style="position:relative; left:-235px;" maxlength="10" type="MobileNumber"
                onkeypress="return allowNoForMobile(event.keyCode, event.which);" tooltip="9888888888"
                width="90px">
            </asp:textbox> 
            <asp:requiredfieldvalidator id="RequiredMobile" runat="server" controltovalidate="txtMobileNo"
                errormessage="<%$ Resources:LocalizedText, EnterMobile %>" display="None">
            </asp:requiredfieldvalidator>
            <cc2:validatorcalloutextender id="ValidatorCalloutExtender3" runat="server" targetcontrolid="RequiredMobile">
            </cc2:validatorcalloutextender>  
             </td>
        <%--<td align="right" class="tdBold">
            <asp:label id="lblAccessCardNo" runat="server" cssclass="lblField" text="Access Card Number"></asp:label>
            <asp:label id="lblRequiredAcNo" runat="server" cssclass="lblRequired" text="*">
            </asp:label>
        </td>--%>
       <%-- <td align="left">
            <asp:textbox id="txtAccessnumber" runat="server" cssclass="txtField" maxlength="6" width="130px">
            </asp:textbox>
             <asp:requiredfieldvalidator id="RequiredAccesscardnumber" runat="server" controltovalidate="txtAccessnumber"
                errormessage="Please enter access card number" display="None">
            </asp:requiredfieldvalidator>
            <cc2:validatorcalloutextender id="ValidatorCalloutExtender4" runat="server" targetcontrolid="RequiredAccesscardnumber">
            </cc2:validatorcalloutextender> 
         </td>--%>
        <%--<td align="right" class="tdBold">
            <asp:label id="lblLastName" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, LastName %>">
            </asp:label>
            <asp:label id="lblRequiredLastName" runat="server" cssclass="lblRequired" text="*"></asp:label>
        </td>
        <td align="left">
            <asp:textbox id="txtLastName" runat="server" cssclass="txtField" maxlength="50" width="130px">
            </asp:textbox>
            <asp:requiredfieldvalidator id="RequiredLN" runat="server" controltovalidate="txtLastName"
                errormessage="<%$ Resources:LocalizedText, EnterLastName %>" display="None">
            </asp:requiredfieldvalidator>
            <cc2:validatorcalloutextender id="ValidatorCalloutExtender1" runat="server" targetcontrolid="RequiredLN">
            </cc2:validatorcalloutextender>
        </td>--%>
        <%--<td align="right" class="tdBold">
            <asp:label id="lblGender" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, Gender %>">
            </asp:label>
            <%--   <asp:Label ID="lblRequiredFirstName" runat="server" CssClass="lblRequired" Text="*"></asp:Label>--%>
        <%--</td>--%>
        <td align="left">
           <%-- <asp:dropdownlist id="ddlGender" runat="server" width="130px" height="22px" cssclass="ddlField"
                style="margin-left: 0px">
                <asp:listitem value="0" text="<%$ Resources:LocalizedText, Select %>">
                </asp:listitem>
                <asp:listitem value="M" text="<%$ Resources:LocalizedText, Male %>">
                </asp:listitem>
                <asp:listitem value="F" text="<%$ Resources:LocalizedText, Female %>">
                </asp:listitem>
            </asp:dropdownlist>--%>
        </td>
    </tr>
    <tr>
       <%-- <td align="right" class="tdBold">
            <asp:label id="lblCompany" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, Company %>"></asp:label>
            <asp:label id="lblRequiredFieldCompany" runat="server" cssclass="lblRequired" text="*">
            </asp:label>
        </td>
        <td align="left">
            <asp:textbox id="txtCompany" runat="server" cssclass="txtField" maxlength="50" width="130px">
            </asp:textbox>
         </td>--%>
        <%--<td align="right" class="tdBold">
            <asp:label id="lblDesignation" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, Designation %>">
            </asp:label>
        </td>
        <td align="left">
            <asp:textbox id="txtDesignation" runat="server" maxlength="50" cssclass="txtField"
                width="130px">
            </asp:textbox>
        </td>--%>
       <%-- <td align="right" class="tdBold">
            <asp:label id="lblEmail" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, EmailID %>"></asp:label>--%>
            <%-- <asp:Label ID="lblRequiredEmail" runat="server" CssClass="lblRequired" Text="*"></asp:Label>--%>
        <%--</td>
        <td align="left">
            <asp:textbox id="txtEmail" runat="server" cssclass="txtField" maxlength="50" width="130px">
            </asp:textbox>--%>
            <%-- <asp:RequiredFieldValidator id="RequiredEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Enter Email Id" Display="None"></asp:RequiredFieldValidator>
         <cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredEmail"></cc2:ValidatorCalloutExtender>--%>
            <%--<asp:regularexpressionvalidator id="Emailformat" runat="server" controltovalidate="txtEmail"
                errormessage="<%$ Resources:LocalizedText, EmailCheck %>" display="None" validationexpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
            </asp:regularexpressionvalidator>
            <cc2:validatorcalloutextender id="ValidatorCalloutExtender5" runat="server" targetcontrolid="Emailformat">
            </cc2:validatorcalloutextender>
        </td>--%>
    </tr>
    <tr>
       <%-- <td align="right" class="tdBold">
            <asp:label id="lblNativeCountry" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, NativeCountry %>">
            </asp:label>
            
        </td>
        <td align="left">
            <asp:dropdownlist id="ddlNativeCountry" runat="server" cssclass="ddlField" width="130px"
                autopostback="True" onselectedindexchanged="DdlNativeCountry_SelectedIndexChanged">
            </asp:dropdownlist>
        </td>--%>
        <%--<td align="right" class="tdBold">
            <asp:label id="lblMobile" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, Mobile %>"></asp:label>
            <%-- <asp:Label ID="lblRequiredMobile" runat="server" CssClass="lblRequired" Text="*"></asp:Label>--%>
      <%--  </td>
        <td align="left">
            <asp:textbox id="txtCountryCode" runat="server" cssclass="txtField" maxlength="4"
                type="CountryCode" onkeypress="return allowNoForMobile(event.keyCode, event.which);"
                tooltip="0001" width="35px" enabled="True">
            </asp:textbox>
            <asp:textbox id="txtMobileNo" runat="server" cssclass="txtField" maxlength="12" type="MobileNumber"
                onkeypress="return allowNoForMobile(event.keyCode, event.which);" tooltip="9888888888"
                width="90px">
            </asp:textbox>          
        </td>--%>
     
     <%-- <td align="right" class="tdBold">
            <asp:label id="lblMobile" runat="server" cssclass="lblField" text="<%$ Resources:LocalizedText, Mobile %>"></asp:label>
             <asp:Label ID="lblRequiredMobile" runat="server" CssClass="lblRequired" Text="*"></asp:Label>
        </td>
         <td align="left">
            <asp:textbox id="txtCountryCode" runat="server" cssclass="txtField" maxlength="4"
                type="CountryCode" onkeypress="return allowNoForMobile(event.keyCode, event.which);"
                tooltip="0001" width="35px" enabled="True">
            </asp:textbox>
            </td>
        <td align="left">
        <asp:textbox id="txtMobileNo" runat="server" cssclass="txtField" style="position:relative; left:-190px;" maxlength="10" type="MobileNumber"
                onkeypress="return allowNoForMobile(event.keyCode, event.which);" tooltip="9888888888"
                width="90px">
            </asp:textbox> 
            <asp:requiredfieldvalidator id="RequiredMobile" runat="server" controltovalidate="txtMobileNo"
                errormessage="<%$ Resources:LocalizedText, EnterMobile %>" display="None">
            </asp:requiredfieldvalidator>
            <cc2:validatorcalloutextender id="ValidatorCalloutExtender3" runat="server" targetcontrolid="RequiredMobile">
            </cc2:validatorcalloutextender>  
             </td>--%>
        <td align="right">
            <%--<asp:Label ID="lblIDProof" runat="server" CssClass="lblField" Text="ID Proof"></asp:Label>--%>
        </td>
        <td align="left">
            <asp:checkbox id="chkIsCofidential" runat="server" visible="false" />
            <div id="divIDProof" runat="server">
           </div>
            
        </td>
    </tr>
</table>

