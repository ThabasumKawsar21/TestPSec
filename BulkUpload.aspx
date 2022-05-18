<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkUpload.aspx.cs" Inherits="VMSDev.BulkUpload"
  %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
  <html xmlns="http://www.w3.org/1999/xhtml">
<%--<head>--%>
<head id="Head1" runat="server">
 <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <script type="text/javascript" language="javascript">
        function CloseParentWindow() {
              window.parent.location.href = window.parent.location.href;
//            window.close();
//            return false;
//            this.close();
        }         
    </script>   
    <link href="includes/base.css" rel="stylesheet" type="text/css" />
    <link href="includes/sharepointcore.css" rel="stylesheet" type="text/css" />
    <link href="includes/banner.css" rel="stylesheet" type="text/css" />
    <link href="includes/vms.css" rel="stylesheet" type="text/css" />
    <link href="includes/vms_homepage.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/stylesBulk.css" rel="Stylesheet" type="text/css" />
     <link href="~/App_Themes/stylesBulk.css" rel="Stylesheet" type="text/css" />
   
    
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript" ></script>
    <script src="Scripts/jquery.filestyle.js" type="text/javascript" ></script>
    <script type="text/javascript" language="javascript">
//        function CloseParentWindow() {
//            window.parent.location.href = window.parent.location.href;
//            // return false;

//        }
        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }

        function avoidSpecialCharacters(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;
            //  alert("id of control is:" + obj.id);
            var ctrl = document.getElementById(evt.id);
            var temp = ctrl.value;

            if (charCode != 37 && charCode != 38 && charCode != 39 && charCode != 40 &&
            charCode != 8 && charCode != 9 && charCode != 13 && charCode != 16 && charCode != 17 && charCode != 18 && charCode != 20) {
                temp = temp.replace(/[^a-zA-Z 0-9]+\\./g, '');
                ctrl.value = temp;
            }
            //alert("id of control is:" + temp);
        };

        function allowAlpha(ie, moz) {

            if (moz != null) {
                //alert(moz);
                if ((moz == 32) || (moz >= 65) && (moz < 91) || moz == 8 || moz == 13 || (moz >= 97) && (moz < 123) || (moz == 46) || (moz >= 48) && (moz < 57) || (moz == 45)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {

                if ((ie == 32) || (ie >= 65) && (ie < 91) || (ie >= 97) && (ie < 123) || (ie == 46) || (ie >= 48) && (ie < 57) || (ie == 45)) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }
//        $(document).ready(function () {

//            function LoaderGIF() {
//                $('#img1').css('visibility', 'visible');
//                return true;
//            }
//        });
    </script>   
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 500px;
        }
    </style>
</head>
<body>
    <form id="form" runat="server" >
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

  <div class="vms_bulkupload wid_900" id="Bulk" runat="server">
        <div class="pop_header">
           <%-- <div class="left flt_left">
            </div>--%>
            <div class="center flt_left" runat="server" id="Close" style="width:909px;">
                <span class="flt_left align_center">Contractor ID Card</span> 
                <asp:ImageButton ID="ImgCancelUpload" AlternateText="Close" runat="server" ImageAlign="Right"
                    ImageUrl="~/Images/Close.png" OnClientClick="CloseParentWindow();"
                    Width="12" Height="12" CssClass="close" />
        
            </div>
             <%--<div class="right flt_left">
            </div>--%>
        </div>
        <div class="pop_content" style=" padding-left:15px;">
            <div class="select_down_edit">
                <p>
                    <span class="font_style1">Preview-Contract Details</span> -<span class="font_style2">Bulk
                        Upload</span></p>
                <p style="height: 44px;">
                    <img src="images/steps.png" /></p>
            </div>

            <div id="divUpload" runat="server">
                <div class="step">
                    <h3>
                       Step 1 :</h3>
                      <span class="pad"><a href="UploadTemplate/ContractorList.xlsx" id="template"
                        class="font_style3 pad block">
                        <asp:Image ID="imgDownload" runat="server" ImageUrl="~/images/download.png" class="v_align" />
                        <%--   </a>--%>
                        Download Template</a></span>
                </div>
                <div class="step">
                    <h3>
                        Step 2 :</h3>
                    <a class="font_style3 pad block">
                        <img src="images/edit.png" class="v_align" /><span class="pad"> Upload Template</span></a>&nbsp;
                    <asp:FileUpload ID="btnUpload" runat="server" CssClass="fileUpload"></asp:FileUpload>
                    <div class="clear" id="Clear" runat="server">
                        <img src="Images/spinner.gif" alt="Validating" id="img1" style="visibility: hidden;" />
                    </div>
                    <asp:Label ID="lblmessage" runat="Server" ForeColor="Red" Font-Italic="true"></asp:Label>
                    <div id="Error" class="disclaim" runat="server">
                        <asp:Label ID="Label6" runat="server" ForeColor="Red" Font-Italic="true" Text="* Incorrect Entries in the Template. Preview and Edit to Correct the Errors"></asp:Label>
                    </div>
                    <div id="Success" class="disclaim" runat="server">
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Green" Font-Italic="true"></asp:Label>
                    </div>
                </div>
                <div class="step" >
                    <table class="center" align="left"> 
                        <tr>
                            <td>
                                <asp:LinkButton ID="Validate" OnClick="LbtnValidate_Click" runat="server" class="un_btns">
                                <span class="un_span">Validate</span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnUpload" OnClick="LbtnUpload_Click" runat="server" class="un_btns"><span class="un_span">Upload</span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnPreviewEdit" OnClick="LbtnPreviewEdit_Click" runat="server"
                                    class="un_btns"><span class="un_span">Preview & Edit</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="step pad_left" >
               <center>
                    <div id="EditGrid" runat="server" style="height:315px; width:850px; overflow:auto; overflow-x: auto; overflow-y: auto;">

                    <asp:GridView ID="dgAssociate" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" BorderStyle="Solid" BorderWidth="1px" 
                            EmptyDataRowStyle-Height="25px"
                            GridLines="None" OnRowEditing="Dgassociate_RowEditing" 
                            OnRowCancelingEdit="Dgassociate_RowCancelingEdit"
                            OnRowUpdating="Dgassociate_RowUpdating" OnRowDeleting="Dgassociate_RowDeleting"
                            OnPageIndexChanging="Dgassociate_PageIndexChanging" 
                            OnRowDataBound="Dgassociate_RowDataBound"
                            PageSize="10" CssClass="info_tbl" Font-Size="X-Small" width="965">
                            <HeaderStyle CssClass="header ht32" />
                            <RowStyle CssClass="odd" />
                            <Columns>
                                <asp:TemplateField HeaderText="Contractor ID" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContratorId" runat="server" Text='<%# Eval("ContractorId") %>'>
                                        </asp:Label>    
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtContractorId" onpaste="return false" onkeyup="avoidSpecialCharacters(this)" MaxLength="15" onkeypress="return onlyNumbers()" runat="server" Text='<%# Eval("ContractorId")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ContractorName" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContractorName" runat="server"  Text='<%# Eval("ContractorName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtContractorName" maxlength="20" runat="server" onpaste="return false"
                                        onKeyPress="return allowAlpha(event.keyCode, event.which);" Text='<%# Eval("ContractorName")%>'></asp:TextBox>

                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Name" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendorName" runat="server" Text='<%# Eval("VendorName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtVendorName" maxlength="25" onpaste="return false" runat="server" Text='<%# Eval("VendorName")%>' 
                                       ></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SupervisorPhone"  HeaderStyle-Width="105px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSuperVisiorPhone" runat="server" Text='<%# Eval("SuperVisiorPhone") %>' >
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSuperVisiorPhone" runat="server" onpaste="return false" onkeypress="return onlyNumbers()"  Text='<%# Eval("SuperVisiorPhone")%>' 
                                        style="width:100px;"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Phone" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendorPhoneNumber" runat="server" Text='<%# Eval("VendorPhoneNumber") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtVendorPhoneNumber" onkeypress="return onlyNumbers()" onpaste="return false"
                                        MaxLength="11" runat="server"  style="width:100px;"
                                         Text='<%# Eval("VendorPhoneNumber")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DOC Status" HeaderStyle-Width="75px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOCStatus" runat="server" Text='<%# Eval("DOCStatus") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:DropdownList Id="drpDocStatus" runat="server">
                                    <asp:ListItem Text="Completed" Selected="True" runat="server"></asp:ListItem>
                                    <asp:ListItem Text="Incomplete"  runat="server"></asp:ListItem>
                                    </asp:DropdownList> 
                                 
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                     <asp:DropdownList Id="drpStatus" runat="server">
                                    <asp:ListItem Text="Active" Selected="True"  runat="server"></asp:ListItem>
                                    <asp:ListItem Text="InActive"  runat="server"></asp:ListItem>
                                    </asp:DropdownList> 
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <%--        <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" 
                                            Text="Edit">--%>
                                      <asp:ImageButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                            src="images/edit_pen.png" />
                                        <asp:ImageButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            src="images/delete.png" OnClick="LnkDelete_Click" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="btnupdate" runat="server" CommandName="Update" Text="Update"></asp:LinkButton>
                                        <asp:LinkButton ID="btncancel" runat="server" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerSettings Mode="Numeric" Visible="true" NextPageText="Next" PreviousPageText="Prev" />
                            <PagerStyle HorizontalAlign="Right" />
                            <AlternatingRowStyle CssClass="odd" />
                        </asp:GridView>
                    </div>
                </center>
                
            </div>
            <div class="step">
                <div id="Div1" class="disclaim" runat="server">
                    <asp:Label ID="lblmessage2" runat="server" ForeColor="Green" Font-Italic="true"></asp:Label>
                </div>
                <table class="center" align="left">
                 
                         <tr>
                        <td >
                            <asp:LinkButton ID="lbtnValidate1" class="un_btns" Text="Validate" runat="server"
                                OnClick="LbtnValidate1_Click"><span class="un_span">Validate</span></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbtnBack" class="un_btns" Text="Back" runat="server" OnClick="LbtnBack_Click"><span class="un_span">Back</span> </asp:LinkButton>
                        </td>
                         <td>
                                <asp:LinkButton ID="lbtnUpload1" OnClick="LbtnUpload1_Click" runat="server" class="un_btns"><span class="un_span">Upload</span></asp:LinkButton>
                            </td>
                            <td>
                            <asp:LinkButton ID="lbtnBack1" class="un_btns" Text="Back" runat="server" OnClick="LbtnBack1_Click"><span class="un_span">Back</span> </asp:LinkButton>
                        </td>                        
                         <td style="padding-left:12px;">
                            <asp:Panel ID="pnlError" runat="server" Style=" color: Red; font-size:12px;">
                            </asp:Panel>
                      </td>
                    </tr>
                     
                </table>
            </div>
            <asp:Button ID="btnDelete" runat="server" Style="display: none;" />
        </div>
    </div>
     <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBg" ID="ModelPopupBulkUploadDelete"
        runat="server" PopupControlID="PnlBulkUploadDelete" Drag="true" TargetControlID="btnDelete">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PnlBulkUploadDelete" runat="server"  Style="display: none;">
        <iframe id="iframeBulkUploadDelete" allowtransparency="true" scrolling="no" class="del-iframe"
            style="height: 170px; background: #f8fdff; width: 498px"  src="BulkUploadDelete.aspx"
            frameborder="0" marginheight="0" marginwidth="0" border="0" runat="server" sandbox="allow-same-origin allow-scripts allow-popups allow-forms"></iframe>
    </asp:Panel>
  </form>
  </body>
  </html>
