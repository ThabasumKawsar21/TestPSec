<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VMSDev.Default"  MasterPageFile="~/VMSMain.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc5" %>--%>
<asp:Content ID="VMSDefaultContent" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
  <script src="Scripts/date.js" type="text/javascript"></script>
   
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
	function GetOffset() {				  
				    var CurrentDate = new Date();				 
				    var today = new Date().toString("dd/MM/yyyy HH:mm:ss"); 
				    Default.AssignTimeZoneOffset(CurrentDate.getTimezoneOffset());
				    Default.AssignCurrentDateTime(today);

				}
                </script>
    <script type="text/javascript"src="Scripts/common.js"></script>
    <script type="text/javascript"src="Scripts/Default.js"></script>
</asp:Content>
