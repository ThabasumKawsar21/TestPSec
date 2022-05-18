<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkUploadDelete.aspx.cs"
    Inherits="VMSDev.BulkUploadDelete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <script type="text/javascript" language="javascript">
        function CloseParentWindow() {
            window.parent.location.href = window.parent.location.href;
            // return false;
        }
    </script>
   
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript" ></script>
    <script src="Scripts/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="Scripts/knockout.validation.js" type="text/javascript"></script>
    <script src="Scripts/knockout.mapping-latest.js" type="text/javascript"></script>
    <title></title>
    <link href="includes/base.css" rel="stylesheet" type="text/css" />
    <link href="includes/sharepointcore.css" rel="stylesheet" type="text/css" />
    <link href="includes/banner.css" rel="stylesheet" type="text/css" />
    <link href="includes/vms.css" rel="stylesheet" type="text/css" />
    <link href="includes/vms_homepage.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/stylesBulk.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="vms_bulkupload">
        <div class="pop_header">
         <div class="left flt_left"></div>
                       <div class="center flt_left" >
                <span class="flt_left align_center" style="margin-left:27%;">Contractor Details - Bulk Upload </span>
                <asp:ImageButton ID="ImgCancelUpload" AlternateText="Close" runat="server" ImageAlign="Right"
                    ImageUrl="~/Images/Close.png" OnClick="ImgCancelUpload_Click" Width="12" Height="12" CssClass="close"/>
            </div>
            <div class="right flt_left">
            </div>
           </div>
        <div class="pop_content">
            <div class="select_down_edit" >
              <%--  <p>
                    <span class="font_style1">Interview Candidates</span>-<span class="font_style2">Bulk
                        Upload</span></p>--%>
            </div>
            <div class="step error txt_center">
                You are about to delete the selected entry from the bulk upload list.<br />
                Delete the entry?
            </div>
            <div class="step">
                <table class="center" cellspacing="5">
                    <tr>
                        <td>
                            <%--  <a href="#" class="un_btns">
                            <span class="un_span">Continue</span></a>--%>
                            <asp:LinkButton ID="lbtnContinue" class="un_btns"  runat="server"
                                OnClick="LbtnContinue_Click"><span class="un_span">Continue</span></asp:LinkButton>
                               <%-- <a href="#" class="un_btns" id="lbtnContinue"><span class="un_span">Continue</span></a>--%>
                        </td>
                        <td>
                            <%--<a href="#" class="un_btns"><span class="un_span">Cancel</span></a>--%>
                            <asp:LinkButton ID="lbtnCancel" class="un_btns"  runat="server"
                                OnClick="LbtnCancel_Click"><span class="un_span">Cancel</span></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
