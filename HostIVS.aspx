<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HostIVS.aspx.cs" Inherits="VMSDev.HostIVS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title> </title>
<style type="text/css"> 
 
 a,a:visited{color: #0379b4;text-decoration: none;}
 
a:focus{outline:none;}
 
a:hover {text-decoration:none; color:#575757;}
 
 .outerDiv
{
 background-color:#fafcff;
 border-bottom: #bec2c7 2px solid;
 border-top: #e3e8ee 1px solid;
 border-right: #d9dde3 2px solid;
 border-left: #d9dde3 2px solid;
 margin:100px auto;
 color:#676767;
 width:600px;
 padding:20px;
}
</style>
</head>
<body>
<div class="outerDiv">
<div style="text-align: center;">
<img alt="One Cognizant" src="Images/1c_logo.png">
</div>
<div style="font-family: Trebuchet MS,Verdana,Arial,Helvetica,sans-serif; font-size: 14px; color: rgb(74, 74, 74); text-align: center; padding-top: 7px;">
<p style="font-weight:bold;font-size:medium;">
    Reports are under One Cognizant now !!!
</p>
<p style="font-weight:bold;font-size:medium;"> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please follow below links to View the Reports and Details.
</p>
<p style="font-weight:bold;font-size:medium; text-align:center"> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="<%= ConfigurationSettings.AppSettings["OneCAAPReport"] %>" target="_new" >HeadCount Report (Visitors List Report)</a>
</p>
<p style="font-weight:bold;font-size:medium;"> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="<%= ConfigurationSettings.AppSettings["OneCAAPReport"] %>" target="_new" >Data by Department Report</a>
</p>
<p style="font-weight:bold;font-size:medium;"> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="<%= ConfigurationSettings.AppSettings["OneCAAPReport"] %>" target="_new" >View Associates without Photo</a>
</p>
<p style="font-weight:bold;font-size:medium;"> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=<%= ConfigurationSettings.AppSettings["OneCAAPReport"] %>" target="_new" >VMS Usage Report</a>
</p>
<p style="font-weight:bold;font-size:medium;"> 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="<%= ConfigurationSettings.AppSettings["OneCAAPReport"] %>" target="_new" >Visit Status Report</a>
</p>
<p style="text-align:left;">
<a href="Default.aspx">Back to Home</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 We appreciate your co-operation!</p>
</div>
</div>
<ul>

</ul>
</body>

</html>
