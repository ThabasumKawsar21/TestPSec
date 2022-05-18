<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientPage.aspx.cs" Inherits="VMSDev.ClientPage"
    MasterPageFile="~/VMSMain.Master" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="css/MasterCss/jquery-ui-1.10.1.custom.css" rel="stylesheet" />
    <link href="css/MasterCss/styles.css?version=2.0" rel="stylesheet" />
    <link href="css/MasterCss/kendo.common-material.min.css" rel="stylesheet" />
    <link href="css/MasterCss/kendo.material.min.css" rel="stylesheet" />
    <link href="css/MasterCss/kendo.material.mobile.min.css" rel="stylesheet" />

    <style>
        #overlay {
            position: fixed;
            display: none;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5);
            z-index: 2;
            cursor: pointer;
        }

        #textOverlay {
            position: absolute;
            top: 50%;
            left: 50%;
            font-size: 20px;
            color: white;
            transform: translate(-50%,-50%);
            -ms-transform: translate(-50%,-50%);
        }

        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: #ababab;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=80);
        }

        #modal {
            display: none;
            position: absolute;
            top: 45%;
            left: 45%;
            width: 200px;
            height: 100px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 1002;
            text-align: center;
            overflow: auto;
        }

        .tooltip {
            position: absolute;
            display: inline-block;
        }

            .tooltip .tooltiptext {
                visibility: hidden;
                width: 500px;
                background-color: #3188b5;
                color: white;
                text-align: left;
                border-radius: 6px;
                padding: 5px 0;
                position: absolute;
                z-index: 1;
                bottom: 125%;
                left: 50%;
                margin-left: -245px;
                opacity: 0;
                transition: opacity 0.3s;
                border: 1px solid #cccccc;
                padding: 10px;
                box-sizing: border-box;
            }

                .tooltip .tooltiptext::after {
                    content: "";
                    position: absolute;
                    top: 100%;
                    left: 50%;
                    margin-left: -5px;
                    border-width: 5px;
                    border-style: solid;
                    border-color: #555 transparent transparent transparent;
                }

            .tooltip:hover .tooltiptext {
                visibility: visible;
                opacity: 1;
            }

        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <div id="wrapper">
        <div id="fade"></div>
        <div id="modal" style="display: none">
            <img id="loader" src="images/New%20Images/Rolling-1s-200px.gif" style="height: 93%; margin-top: -10%;" /><br />
            <span style="font: 14px bold; font-family: Verdana; color: #0033a0;">Loading Please wait....</span>
        </div>
        <div class="MainContainer">

            <div class="SubContainerLeft" style="visibility: hidden">
                <div class="divLocationdetails" style="height: 40px; margin-top: 19px;">
                    <br />
                    
                    <input id="txtpurpose" type="text" runat="server" class="textBoxhome" placeholder="Clients" disabled="disabled" style="margin-top: -20px; margin-left: 15px;" />
                   </div>
                <input id="txtothers" type="text" runat="server" style="display: none; width: 246px; background-color: white; border: 1px solid #cccccc; margin-top: 10px; padding-left: 22px;" class="textBoxhome" placeholder="Please Enter Others" />
                <div id="Locationdetails">
                    <div class="divLocationdetails">
                        <input id="txtvisitdate" type="text" runat="server" class="textBoxhome" placeholder="Meeting Date" disabled="disabled" style="width: 220px; margin-left: 17px" />
                        <%-- <img id="imgdate" class="imgdate" src="images/New%20Images/img_trans.gif" />--%>
                        <input type="text" id="txtstarttime" value="Start Time" runat="server" class="textBoxhome Starttime" onkeypress="return AllowAlphanumeric(event)" style="" placeholder="Start Time" disabled="disabled" />
                        <%--<img id="imgstarttime" src="images/New%20Images/img_trans.gif" class="ImgStartTime" />--%>
                        <input type="text" id="txtendtime" runat="server" class="textBoxhome Endtime" onkeypress="return AllowAlphanumeric(event)" placeholder="End Time" disabled="disabled" />
                        <%--  <img src="images/New%20Images/img_trans.gif" class="ImgEndTime" />--%>
                        <div class="select-wrapper">
                            <%-- <asp:DropDownList ID="ddllocationlist" runat="server" CssClass="custom_select" Enabled="false">
                            <asp:ListItem>Meeting Location</asp:ListItem>
                        </asp:DropDownList>--%>
                            <input type="text" id="txtmeetinglocation" runat="server" style="margin-left: 16px; width: 225px; height: 42px;" class="textBoxhome" placeholder="Meeting Location" disabled="disabled" />
                            <img src="images/New%20Images/img_trans.gif" style="background: url('/images/New%20Images/VMS-Icons.png') no-repeat -371px -33px; width: 10px; height: 6px; margin-top: -8%; margin-left: 87%; position: absolute;" />
                        </div>
                        <input type="text" id="txthostID" runat="server" style="margin-left: 16px; width: 225px; height: 42px;" class="textBoxhome" placeholder="Host Name or ID" disabled="disabled" />
                        <div class="tooltip" style="margin-top: 0%; margin-left: 0%; float: right;">
                            <img src="images/New%20Images/img_trans.gif" class="ImgHostID" /><span class="tooltiptext">Tooltip text</span>
                        </div>
                        
                    </div>
                </div>
                <%--timedetails--%>
                <div id="Timedetails">
                    <div class="divLocationdetails">
                        <label id="lblnotifytime" class="textBoxhome" style="margin-top: 10px; margin-left: -49px;">Host Notified On :</label><br />
                        <input type="text" id="notifytime" runat="server" style="margin-left: 16px; width: 225px; height: 42px; margin-top: -25px;" class="textBoxhome" disabled="disabled" />
                        <label id="lbldispatchtime" class="textBoxhome" style="margin-top: 10px; margin-left: -38px;">Pass Dispatched On :</label><br />
                        <input type="text" id="dispatchtime" runat="server" style="margin-left: 16px; width: 225px; height: 42px; margin-top: -25px;" class="textBoxhome" disabled="disabled" />
                        <label id="lblpasscollectedby" class="textBoxhome" style="margin-top: 10px; margin-left: -45px;">Pass Collected By  :</label><br />
                        <input type="text" id="textpasscollectedby" runat="server" style="margin-left: 16px; width: 225px; height: 42px; margin-top: -25px;" class="textBoxhome" disabled="disabled" />
                    </div>
                </div>
            </div>
            <div class="SubContainerRight">
               
                <div class="divRecentvisitor">
                    <table class="Recentvisitortable" id="tblRecentvisitor">
                        <thead>
                            <tr class="RecentvisitortableTheadtr">
                                <td style="width: 40px; text-align: center; display: none;" id="trhead">
                                    <input type="checkbox" class="dataaccesscheckbox1" style="width: 15px; height: 15px;" runat="server" checked="checked" onchange="selectall()" />
                                </td>
                                <td style="width: 250px; text-align: center;">Visitor Name
                                </td>
                                <td style="width: 150px; text-align: center;">Organization
                                </td>
                                <td style="width: 150px; text-align: center;">Mobile Number
                                </td>
                                <td style="width: 150px; text-align: center;">Vcard#
                                    <div class="tooltip" style="margin-top: 0%; margin-left: 0%; float: right;">
                                        <img src="Images/info.png" style="width: 15px;" />
                                        <span class="tooltiptext">Please enter the card that is registered in
                                            <br />
                                            visitor card inventory of Access card application<br />
                                        </span>
                                    </div>

                                </td>
                                <td style="width: 150px; text-align: center;">Access Card
                                    <div class="tooltip" style="margin-top: 0%; margin-left: 0%; float: right;">
                                        <img src="Images/info.png" style="width: 15px;" />
                                        <span class="tooltiptext">Access card number will be prepopulated automatically
                                            <br />
                                            based on entered Vcard number.
                                            <br />
                                            In case of incorrect access card number,
                                            <br />
                                            please have that rectified in Access card – Visitor card inventory
                                            <br />
                                        </span>
                                    </div>
                                </td>
                                <td style="width: 120px; text-align: center;">Equipment
                                </td>
                                <td style="width: 90px; display: none; text-align: center;" id="badgestatus">Vcard Status
                                </td>
                                <td style="width: 120px; text-align: center;">Visit Details
                                </td>
                                <td style="width: 120px; text-align: center;">Log Details
                                </td>
                                <td style="width: 48px; display: none;"></td>

                            </tr>
                        </thead>
                        <tbody id="RecentVisitortablebody">
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="footerContainer" style="top: 550px; color: green; text-align: left;" id="divnotes" runat="server">
                <p>Note:</p>
                <p>1. You will be able to notify the host to collect the visitor Vcards once card details are updated for all visitors.</p>
                <p>2. Post notifying the host, when Vcards are not collected, three reminders mails will be sent to host in the interval of  every 12 hours.</p>
            </div>
            <div class="footerContainer">
                <span style="color: red; margin-left: 17px;" id="spnerror"></span>
               
                <asp:Button CssClass="btnPreviewSubmit" runat="server" Text="Back" OnClick="Back_Click" ID="btnback"></asp:Button>
                <%--<button class="btnAddCart" onclick="return notifyClick()" runat="server" id="btnnotify">Notify</button>--%>
                <input type="button" class="btnAddCart" onclick="return notifyClick()" runat="server" id="btnnotify" value="Notify" />
                <%--<button class="btnPreviewSubmit" onclick="return updatedetails()" runat="server" id="btnupdate">Update</button>--%>
                <input type="button" class="btnPreviewSubmit" onclick="return updatedetails()" runat="server" id="btnupdate" value="Update" />
                
                <input type="button" class="btnPreviewSubmit" onclick="returncheckoutconfirm()" runat="server" id="btnretcheckout" visible="false" value="Return & Check-Out" />
                <input type="button" class="btnPreviewSubmit" onclick="lostcheckoutconfirm()" runat="server" id="btnlcheckout" visible="false" value="Lost & Check-Out" />
                <input type="button" class="btnPreviewSubmit" onclick="reissuecardsconfirm()" runat="server" id="btnreissue" visible="false" value="Re - Issue" />
                <input type="button" class="btnPreviewSubmit" onclick="resetvalue()" runat="server" id="btnreset" visible="false" value="Reset" />        
            </div>
        </div>
    </div>
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="closeequip">Close</span>
                <h1 style="font-size: 14px; font-weight: bold; font-family: Verdana; margin-top: 18px; margin-left: 2px;">Equipment Information</h1>
            </div>
            <div class="modal-body">
                <table style="width: 100%; border-collapse: collapse;">
                    <thead>
                        <tr class="RecentvisitortableTheadtr">
                            <td>Type
                            </td>
                            <td>Make
                            </td>
                            <td>Model
                            </td>
                            <td>Serial
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="Recentvisitortablebodytrodd">
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtlaptopType" runat="server" value="Laptop" placeholder="Laptop" readonly="readonly" maxlength="40" class=" textBoxhome textboxBg Equipmenttabletexbox" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtlaptopMake" runat="server" placeholder="Make" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtlaptopModel" runat="server" placeholder="Model #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtlaptopserial" runat="server" placeholder="Serial #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                        </tr>
                        <tr class="Recentvisitortablebodytrodd">
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtdatastoragedeviceType" runat="server" value="Data Storage Device" placeholder="Data Storage Device" readonly="readonly" class="textBox textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtdatastoragedeviceMake" runat="server" placeholder="Make" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtdatastoragedeviceModel" runat="server" placeholder="Model #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtdatastoragedeviceSerial" runat="server" placeholder="Serial #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                        </tr>
                        <tr class="Recentvisitortablebodytrodd">
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtUSBharddiskType" runat="server" value="USB Hard Disk" placeholder="USB Hard Disk" readonly="readonly" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtUSBharddiskMake" runat="server" placeholder="Make" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtUSBharddiskModel" runat="server" placeholder="Model #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtUSBharddiskSerial" runat="server" placeholder="Serial #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                        </tr>
                        <tr class="Recentvisitortablebodytrodd">
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtcameraType" runat="server" value="Camera" placeholder="Camera" readonly="readonly" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtcameraMake" runat="server" placeholder="Make" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtcameraModel" runat="server" placeholder="Model #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtcameraSerial" runat="server" placeholder="Serial #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                        </tr>
                        <tr class="Recentvisitortablebodytrodd">
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtIPodType" runat="server" value="IPod" placeholder="IPod" readonly="readonly" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtIPodMake" runat="server" placeholder="Make" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtIPodModel" runat="server" placeholder="Model #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtIPodSerial" runat="server" placeholder="Serial #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                        </tr>
                        <tr class="Recentvisitortablebodytrodd">
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtotherType" runat="server" value="Other" placeholder="Enter Other" readonly="readonly" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtotherMake" runat="server" placeholder="Make" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtotherModel" runat="server" placeholder="Model #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                            <td class="Equipmenttabletd">
                                <input type="text" id="txtotherSerial" runat="server" placeholder="Serial #" class="textBoxhome textboxBg Equipmenttabletexbox" maxlength="40" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            
        </div>

    </div>

    <div id="visitdetails" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="closeequip">Close</span>
                <h1 style="font: 16px bold; font-family: Verdana; margin-top: 18px; margin-left: 2px;">Visit Details</h1>
            </div>
            <div class="modal-body">
                <table style="width: 100%; border-collapse: collapse;">
                    <thead>
                        <tr class="RecentvisitortableTheadtr">
                            <td style="text-align: center">Location
                            </td>
                            <td style="text-align: center">Visit Date(s)
                            </td>
                            <td style="text-align: center">From Time
                            </td>
                            <%--                            <td>TO DATE
                            </td>--%>
                            <td style="text-align: center">To Time
                            </td>
                            <td style="text-align: center">Host
                            </td>
                        </tr>
                    </thead>
                    <tbody id="visitdetailstbody">
                    </tbody>
                </table>
            </div>

        </div>

    </div>

    <div id="logdetails" class="modal">

        <!-- Modal content -->
        <div class="modal-content" style="width: 1106px;">
            <div class="modal-header">
                <span class="closeequip">Close</span>
                <h1 style="font-size: 16px; font-weight: bold; font-family: Verdana; margin-top: 18px; margin-left: 2px;">Log Details</h1>
            </div>
            <div class="modal-body" style="padding: 2px 4px;">
                <table style="width: 100%; border-collapse: collapse;">
                    <thead>
                        <tr class="RecentvisitortableTheadtr">
                            <td style="text-align: center">VCard#
                            </td>
                            <td style="text-align: center">Access Card#
                            </td>
                            <td style="text-align: center">Vcard Issued On
                            </td>
                            <td style="text-align: center">Vcard Issued At
                            </td>
                            <td style="text-align: center">Vcard Issued To
                            </td>
                            <td style="text-align: center">Vcard Status
                            </td>
                            <td style="text-align: center">Returned/Reported By
                            </td>
                            <td style="text-align: center">Returned/Reported At
                            </td>
                            <td style="text-align: center">Returned/Reported On
                            </td>

                        </tr>
                    </thead>
                    <tbody id="logdetailstbody">
                    </tbody>
                </table>
            </div>
            </div>

    </div>

    <div id="CardModel" class="modal" runat="server">
        <!-- Modal content -->
        <div class="modal-content" style="width: 400px; /*margin-left: 435px; */ height: 200px">
            <div class="modal-header">
                <span class="close">Close</span>
                <h1 style="font-size: 16px; font-weight: bold; font-family: Verdana; margin-top: 18px;">Pass Dispatch</h1>
            </div>
            <div class="modal-body" style="height: 80px;">
                <table style="width: 100%;">

                    <tbody id="tbodycart" style="align-content: center; font-size: 14px; font-family: Verdana">
                        <%--<asp:Label >Collectd By<span style="color:red">*</span></asp:Label>--%>

                        <tr style="height: 75px">
                            <td><span>Notification mail has been triggered to the Host.</span></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="modal-footer">
                <asp:Button Text="Confirm" OnClick="BtnNotifyconfirm_Click" runat="server" CssClass="btnAddCart"></asp:Button>
            </div>
        </div>
    </div>

    <div id="SucessModel" class="modal">

        <!-- Modal content -->
        <div class="modal-content" style="width: 400px; /*margin-left: 435px; */ height: 200px; margin-bottom: 260px">
            <div class="modal-header">
                <span class="close">Close</span>
                <h1 style="font-size: 16px; font-weight: bold; font-family: Verdana; margin-top: 18px;">Confirmation</h1>
            </div>
            <div class="modal-body" style="height: 80px;">
                <table style="width: 100%; margin-top: 25px">

                    <tbody id="tbodycart1" style="align-content: center; font-size: 14px; font-family: Verdana;">
                    </tbody>
                </table>
            </div>
            <div class="modal-footer">
                <%--    <button class="btnPreviewSubmit" id="updateok">Ok</button>--%>
                <input type="button" class="btnPreviewSubmit" id="updateok" value="OK" runat="server" />


            </div>
        </div>
    </div>

    <div id="CheckoutModel" class="modal">

        <!-- Modal content -->
        <div class="modal-content" style="width: 400px; /*margin-left: 435px; */ height: 200px; margin-bottom: 260px">
            <div class="modal-header">
                <span class="close">Close</span>
                <h1 style="font-size: 16px; font-weight: bold; font-family: Verdana; margin-top: 18px;">Notification</h1>
            </div>
            <div class="modal-body" style="height: 80px;">
                <table style="width: 100%; margin-top: 7px">

                    <tbody id="tbodycart2" style="align-content: center; font-size: 14px; font-family: Verdana;">
                        <tr style="height: 75px">
                            <td><span>Selected requests will be treated as "Returned" and other requests will be treated a "Lost".Are you sure, you want to continue?</span></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="modal-footer">
                <input type="button" class="btnPreviewSubmit" id="btncheckoutconfirm" runat="server" value="Yes" />
                <input type="button" class="btnPreviewSubmit" id="btncheckoutclose" runat="server" value="No" />

            </div>
        </div>
    </div>

    <div id="returnmodal" class="modal" runat="server">

        <!-- Modal content -->
        <div class="modal-content" style="width: 400px; /*margin-left: 435px; */ height: 200px">
            <div class="modal-header">
                <span class="close" onclick="close()">Close</span>
                <%--<asp:Button Text="Close"   BorderWidth="0px" OnClick="Close_Click" runat="server" CssClass="btnAddCart"></asp:Button>--%>
                <h1 style="font-size: 16px; font-weight: bold; font-family: Verdana; margin-top: 23px;">Return and Check-Out</h1>
            </div>
            <div class="modal-body" style="height: 167px; margin-top: -25px;">
                <table style="width: 100%; margin-top: 20px">
                    <tbody id="tbodyreturncheckout" style="align-content: center">


                        <tr style="height: 75px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Vcards returned By<span style="color: red">*</span></label></td>
                            <td>
                                <input type="text" runat="server" id="txtreturnedby" maxlength="15" onkeypress="return isNumber(event)" class="text_field" onblur="associatecheck()" /></td>
                        </tr>
                        <tr style="height: 0px">
                            <td></td>
                            <td>
                                <input type="text" runat="server" id="txtcheckassociatereturn" style="display: none;" disabled="disabled" class="text_field" /></td>
                        </tr>
                        <tr style="height: 75px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Vcards Returned at <span style="color: red">*</span></label></td>
                            <td>
                                <%--<input type="text" runat="server" id="Text2" maxlength="6"  onkeypress="return isNumber(event)" class="text_field" /></td>--%>
                                <asp:DropDownList ID="returnddlfacility" runat="server" CssClass="custom_select">
                                </asp:DropDownList>
                        </tr>
                        <tr style="height: 75px">
                            <td>
                                <span id="returncheckspan" style="color: red; display: none;">Kindly enter a valid Associate ID.</span></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="modal-footer">

                <input type="button" runat="server" class="btnPreviewSubmit" id="btnreturncheckout" onclick="returnandcheckout()" value="Confirm Return" />
                <%--<asp:Button Text="Confirm" OnClick="BtnCollectconfirm_Click" runat="server" CssClass="btnAddCart"></asp:Button>--%>
            </div>
        </div>
    </div>

    <div id="lostmodal" class="modal" runat="server">

        <!-- Modal content -->
        <div class="modal-content" style="width: 400px; /*margin-left: 435px; */ height: 277px">
            <div class="modal-header">
                <span class="close" onclick="close()">Close</span>
                <%--<asp:Button Text="Close"   BorderWidth="0px" OnClick="Close_Click" runat="server" CssClass="btnAddCart"></asp:Button>--%>
                <h1 style="font-size: 16px; font-weight: bold; font-family: Verdana; margin-top: 23px;">Lost and Check-Out</h1>
            </div>
            <div class="modal-body" style="height: 254px; margin-top: -35px;">
                <table style="width: 100%; margin-top: 25px">
                    <tbody id="tbodylostcheckout" style="align-content: center">


                        <tr style="height: 75px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Reported by<span style="color: red">*</span></label></td>
                            <td>
                                <input type="text" runat="server" id="txtlostreturnedby"  onkeypress="return isNumber(event)" class="text_field" onblur="associatecheck()" /></td>
                        </tr>
                        <tr style="height: 0px">
                            <td></td>
                            <td>
                                <input type="text" runat="server" id="txtcheckedassociatelost" style="display: none;" disabled="disabled" class="text_field" /></td>
                        </tr>
                        <tr style="height: 75px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Reported at <span style="color: red">*</span></label></td>
                            <td>
                                <%--<input type="text" runat="server" id="Text2" maxlength="6"  onkeypress="return isNumber(event)" class="text_field" /></td>--%>
                                <asp:DropDownList ID="lostddlfacility" runat="server" CssClass="custom_select">
                                </asp:DropDownList>
                        </tr>
                        <tr style="height: 64px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Card lost on<span style="color: red">*</span></label></td>
                            <td>
                                <asp:TextBox ID="txtreporteddate" runat="server" placeholder="MM/DD /YYYY" onkeydown="return false" onpaste="return false"></asp:TextBox>
                        </tr>

                        <tr style="height: 60px">
                            <td>
                                <span id="lostcheckspan" style="color: red; display: none;">Kindly enter a valid Associate ID.</span>
                                <span id="lostdatespan" style="color: red; display: none;">Kindly enter a valid date.</span></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="modal-footer">

                <input type="button" runat="server" class="btnPreviewSubmit" id="btnlostcheckout" onclick="lostandcheckout()" value="Confirm Lost" />
                <%--<asp:Button Text="Confirm" OnClick="BtnCollectconfirm_Click" runat="server" CssClass="btnAddCart"></asp:Button>--%>
            </div>
        </div>
    </div>

    <div id="reissuemodal" class="modal" runat="server">

        <!-- Modal content -->
        <div class="modal-content" style="width: 400px; /*margin-left: 435px; */ height: 268px">
            <div class="modal-header">
                <span class="close" onclick="close()">Close</span>
                <%--<asp:Button Text="Close"   BorderWidth="0px" OnClick="Close_Click" runat="server" CssClass="btnAddCart"></asp:Button>--%>
                <h1 style="font-size: 16px; font-weight: bold; font-family: Verdana; margin-top: 23px;">Re-Issue Confirmation</h1>
            </div>
            <div class="modal-body" style="height: 323px; margin-top: -113px;">
                <table style="width: 100%; margin-top: 25px">
                    <tbody id="tbodyreissue" style="align-content: center">


                        <tr style="height: 75px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Reason for Re-Issue<span style="color: red">*</span></label></td>
                            <td>
                                <%--<input type="text" runat="server" id="Text2" maxlength="6"  onkeypress="return isNumber(event)" class="text_field" /></td>--%>
                                <asp:DropDownList ID="ddlreason" runat="server" CssClass="custom_select">

                                    <%--<asp:ListItem>Lost</asp:ListItem>
                                    <asp:ListItem>Access card not working</asp:ListItem>--%>
                                </asp:DropDownList>
                        </tr>

                        <tr style="height: 75px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Requested By<span style="color: red">*</span></label></td>
                            <td>
                                <input type="text" runat="server" id="txtreissuerequestedby"  onkeypress="return isNumber(event)" class="text_field" onblur="associatecheck()" /></td>
                        </tr>
                        <tr style="height: 0px">
                            <td></td>
                            <td>
                                <input type="text" runat="server" id="txtcheckedassociatereissue" style="display: none;" disabled="disabled" class="text_field" /></td>
                        </tr>
                        
                        <tr style="height: 75px">
                            <td>
                                <label style="font-size: 14px; font-family: Verdana;">Re-Issued Facility<span style="color: red">*</span></label></td>
                            <td>
                                <%--<input type="text" runat="server" id="Text2" maxlength="6"  onkeypress="return isNumber(event)" class="text_field" /></td>--%>
                                <asp:DropDownList ID="reissueddlfacility" runat="server" CssClass="custom_select">
                                </asp:DropDownList>
                        </tr>
                        <tr style="height: 75px">
                            <td>
                                <span id="reissuecheckspan" style="color: red; display: none;">Kindly enter a valid Associate ID.</span></td>
                        </tr>

                    </tbody>
                </table>
            </div>
            <div class="modal-footer">

                <input type="button" runat="server" class="btnPreviewSubmit" id="btnreissuecomplete" onclick="reissuecompleteclick()" value="Confirm Re-Issue" />
                <%--<asp:Button Text="Confirm" OnClick="BtnCollectconfirm_Click" runat="server" CssClass="btnAddCart"></asp:Button>--%>
            </div>
        </div>
    </div>

    <div id="overlay">
        <div id="textOverlay">Fetching Access Card details...</div>
    </div>

    <asp:HiddenField ID="hdnIsEditable" runat="server" />
    <asp:HiddenField ID="hdnRequestID" runat="server" />
    <asp:HiddenField ID="hdnDCid" runat="server" />
    <asp:HiddenField ID="hdnvisitorID" runat="server" />
    <asp:HiddenField ID="hdnisbulkupload" runat="server" />
    <asp:HiddenField ID="hdnParentID" runat="server" />
    <asp:HiddenField ID="hdnSecurityID" runat="server" />
    <asp:HiddenField ID="hdnbadgestatus" runat="server" />
    <asp:HiddenField ID="hdnaccount" runat="server" />
    <asp:HiddenField ID="hdnnotifyclick" runat="server" />
    <asp:HiddenField ID="hdnloginfacility" runat="server" />

    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.js"></script>
    <script  type="text/javascript"src="Scripts/kendo.all.min.js"></script>
    <script  type="text/javascript" src="Scripts/select2.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#txtreporteddate").datepicker({ maxDate: 0 });
        });

        $(function () {
            $(document).on('keydown','table',(function (e) {
                if (e.keyCode == 9) {
                    e.preventDefault();
                    
                }
            }))
        });


        $(function () {

            $('table').on('keyup', '.vcard', function (e) { 
            
               
                var currentVcardID = e.currentTarget.id;
                var currentRequestID = currentVcardID.match(/[\d]+|\d+/);
                if (checkempty(currentRequestID)) {
                    avoidSpecialCharacters(e.currentTarget);
                    var currentvcard = $("#" + currentVcardID).val();
                    if (currentvcard.length == 7) {
                        CheckVcardGetAccessCard(currentRequestID);
                    }

                    else {
                        $("#tdtxtaccesscard" + currentRequestID).val("");
                        $("#tdtxtaccesscard" + currentVcardID).css('border-color', '1px black');

                        if (currentvcard.length < 7) {
                          
                           // $("#spnerror").text("*Error : Kindly enter valid Vcard number(s)");
                           // $("#tdtxtvcard" + currentVcardID).css('border-color', 'red');

                            $("#btnupdate").css("background-color", "grey");
                            $("#btnupdate").attr("disabled", "disabled");
                            $("#btnnotify").css("background-color", "grey");
                            $("#btnnotify").attr("disabled", "disabled");
                            $("#btnreissue").css("background-color", "grey");
                            $("#btnreissue").attr("disabled", "disabled");

                        }
                       
                    }
                }
                else {
                    
                    $("#spnerror").text("");
                    $("#btnupdate").css("background-color", "#3188B5");
                    $("#btnupdate").removeAttr("disabled");
                    $("#btnreissue").css("background-color", "#3188B5");
                    $("#btnupdate").removeAttr("disabled");
                }
            });
        });

        $(document).ready(function () {

            $("#modal").show();
            $("#fade").show();
            var parentid = $('#hdnParentID').val();
            var securityid = $('#hdnSecurityID').val();
            var notifyclickvalue = 0;
            $('#hdnnotifyclick').val(notifyclickvalue);

            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/GetVisitorList",
                contentType: "application/json;charset=utf-8",
                data: "{'parentID':" + parentid + "}",
                dataType: "json",
                success: function (data) {

                    var Recentdata = JSON.parse(data.d);
                    var RecentData_append = "";
                    if (Recentdata[0].badgestatus == "issued" || Recentdata[0].badgestatus == "Returned" || Recentdata[0].badgestatus == "Lost") {
                        document.getElementById('trhead').style.display = 'table-cell';
                        document.getElementById('badgestatus').style.display = 'table-cell';
                        for (var i = 0; i < Recentdata.length; i++) {
                            if (i % 2 == 0) {
                                RecentData_append += "<tr class='Recentvisitortablebodytrodd' id='tr" + Recentdata[i].RequestID + "'>";
                            }
                            else {
                                RecentData_append += "<tr class='Recentvisitortablebodytreven' id='tr" + Recentdata[i].RequestID + "'>";
                            }

                            if (Recentdata[i].badgestatus == "Returned" || Recentdata[i].badgestatus == "Lost") {
                                RecentData_append += "<td style='width:44px;text-align:center;display:table-cell;'>";
                                RecentData_append += "<input type='checkbox'  value='" + Recentdata[i].RequestID + "' class='dataaccesscheckbox' id='tdchkselect" + Recentdata[i].RequestID + "' disabled>";
                                RecentData_append += "</td>";
                            }
                            else {
                                RecentData_append += "<td style='width:44px;text-align:center;display:table-cell;'>";
                                RecentData_append += "<input type='checkbox'  checked='checked' value='" + Recentdata[i].RequestID + "' class='dataaccesscheckbox' id='tdchkselect" + Recentdata[i].RequestID + "' onchange='UserSelectChange(" + Recentdata[i].RequestID + ")'>";
                                RecentData_append += "</td>";
                            }
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:168px;'>";
                            RecentData_append += "<input type='text' id='tdtxtvisitorName" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].FirstName + "' disabled />";
                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:150px;'>";
                            RecentData_append += "<input type='text' id='tdtxtcompany" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].Company + "' disabled />";
                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' >";
                            var Mobile = Recentdata[i].MobileNo.split('-');
                            RecentData_append += "<select type='text' id='tdtxtCountryCode" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' style='width:45px;' disabled ><option>" + Mobile[0] + "</option></select>-<input id='tdtxtmobileno" + Recentdata[i].RequestID + "' style='margin-left:2%;' class='Recentvisitortabletextbox' type='text' value='" + Mobile[1] + "' disabled />";
                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:158px;text-align:  center;'>";

                            var RID = Recentdata[i].RequestID;
                            var VCNo = $.trim(Recentdata[i].VcardNumber);
                            var vdBadge = Recentdata[i].BadgeNo;

                            // Binding VCARD NO from Visitdetails table if the entry is checked in from request creation page
                            if ((VCNo == '' || VCNo == null) && $.trim(vdBadge) != '')
                            {
                                VCNo = $.trim(vdBadge);

                                //Getting access card no for the VCARD no using service call
                                var accessNo = GetAccessCardNo(VCNo);
                                Recentdata[i].AccessCardNumber = accessNo;
                            }

                            var ACNo = "";
                            //// Vcard addition
                            if (VCNo == null || VCNo == undefined || VCNo == "") {
                                VCNo = "";
                                RecentData_append += "<input type='text' id='tdtxtvcard" + Recentdata[i].RequestID + "' maxlength='7' style='text-transform: uppercase;border: 1px solid #596679;'  class='Recentvisitortableaftertextbox vcard' value='" + VCNo + "'/>";
                                RecentData_append += "<input type='hidden' id='hdntdtxtvcard" + Recentdata[i].RequestID + "'value='" + VCNo + "'/>";
                            }
                            else {

                                ACNo = Recentdata[i].AccessCardNumber;
                                RecentData_append += "<input type='text' id='tdtxtvcard" + Recentdata[i].RequestID + "' maxlength='7' style='text-transform: uppercase; border: 1px solid #596679;' class='Recentvisitortableaftertextbox vcard' value='" + VCNo + "' disabled />";
                                RecentData_append += "<input type='hidden' id='hdntdtxtvcard" + Recentdata[i].RequestID + "' value='" + VCNo + "'/>";
                            }


                            //// coloumn addded for access card
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:150px;'>";
                            RecentData_append += "<input type='text' id='tdtxtaccesscard" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + ACNo + "' disabled />";
                            RecentData_append += "</td>";

                            RecentData_append += "<td class='Recentvisitortabletd' style='width:120px;'>";
                            var equip = Recentdata[i].PermitITEquipments;
                            if (equip == true) {
                                RecentData_append += "<a id='tdaequipment" + Recentdata[i].RequestID + "' href='#' style='color:#006583;cursor:pointer;margin-left:10px;' onclick='OpenEquipment(" + Recentdata[i].VisitorID + ")'>Yes</a>";
                            }
                            else {
                                RecentData_append += "<a id='tdaequipment" + Recentdata[i].RequestID + "' href='#' style='color:#596679;cursor:pointer;margin-left:10px;' onclick='OpenEquipment(" + Recentdata[i].VisitorID + ")'>No</a>";
                            }
                            RecentData_append += "</td>";
                            RecentData_append += "<td style='width:1px;'>";
                            RecentData_append += "<input type='text' id='tdbadge" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].badgestatus + "' />";
                            RecentData_append += "</td>";
                            RecentData_append += "<td style='width:1px;text-align:  center;'>";
                            RecentData_append += "<a id='tdvisitdetails" + Recentdata[i].RequestID + "' href='#' style='color:#006583;cursor:pointer;margin-left:10px;' onclick='Openvisitdetails(" + Recentdata[i].VisitorID + ")'>View</a>";
                            RecentData_append += "</td>";
                            RecentData_append += "<td style='width:1px;text-align:  center;'>";
                            RecentData_append += "<a id='tdlogdetails" + Recentdata[i].RequestID + "' href='#' style='color:#006583;cursor:pointer;margin-left:10px;' onclick='Openlogdetails(" + Recentdata[i].VisitorID + ")'>View</a>";
                            RecentData_append += "</td>";
                            RecentData_append += "<td style='width:1px;display:none;'>";
                            RecentData_append += "<input type='text' id='tdvisitorid" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].VisitorID + "' style='visibility:hidden;' />";
                            RecentData_append += "</td>";
                            RecentData_append += "</tr >";
                        }
                    }
                    else {

                        for (var i = 0; i < Recentdata.length; i++) {
                            if (i % 2 == 0) {
                                RecentData_append += "<tr class='Recentvisitortablebodytrodd' id='tr" + Recentdata[i].RequestID + "'>";
                            }
                            else {
                                RecentData_append += "<tr class='Recentvisitortablebodytreven' id='tr" + Recentdata[i].RequestID + "'>";
                            }
                            RecentData_append += "<td style='width:44px;text-align:center;display:none;'>";
                            RecentData_append += "<input type='checkbox' style='visibility:hidden;' checked='checked'  value='" + Recentdata[i].RequestID + "' class='dataaccesscheckbox' id='tdchkselect" + Recentdata[i].RequestID + "' onchange='UserSelectChange(" + Recentdata[i].RequestID + ")'>";
                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:168px;'>";
                            RecentData_append += "<input type='text' id='tdtxtvisitorName" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].FirstName + "' disabled />";
                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:150px;'>";
                            RecentData_append += "<input type='text' id='tdtxtcompany" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].Company + "' disabled />";
                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' >";
                            var Mobile = Recentdata[i].MobileNo.split('-');
                            RecentData_append += "<select type='text' id='tdtxtCountryCode" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' style='width:45px;' disabled ><option>" + Mobile[0] + "</option></select>-<input id='tdtxtmobileno" + Recentdata[i].RequestID + "' style='margin-left:2%;' class='Recentvisitortabletextbox' type='text' value='" + Mobile[1] + "' disabled />";
                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:158px;text-align:  center;'>";

                            var RID = Recentdata[i].RequestID;


                            var VCNo = Recentdata[i].VcardNumber;
                            var ACNo = "";
                            //// Vcard addition
                            if (VCNo == null || VCNo == undefined || VCNo == "") {
                                VCNo = "";
                                RecentData_append += "<input type='text' id='tdtxtvcard" + Recentdata[i].RequestID + "' maxlength='7' style='text-transform: uppercase; border: 1px solid #596679;' class='Recentvisitortableaftertextbox vcard' value='" + VCNo + "'/>";
                                RecentData_append += "<input type='hidden' id='hdtdtxtvcard" + Recentdata[i].RequestID + "' value='" + VCNo + "'/>";
                            }
                            else {
                                ACNo = Recentdata[i].AccessCardNumber;
                                VCNo = Recentdata[i].VcardNumber;
                                RecentData_append += "<input type='text' id='tdtxtvcard" + Recentdata[i].RequestID + "' maxlength='7' style='text-transform: uppercase; border: 1px solid #596679;' class='Recentvisitortableaftertextbox vcard ' value='" + VCNo + "' />";
                                RecentData_append += "<input type='hidden' id='hdtdtxtvcard" + Recentdata[i].RequestID + "' value='" + VCNo + "'/>";
                            }
                             //// coloumn addded for access card
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:150px;'>";
                            RecentData_append += "<input type='text' id='tdtxtaccesscard" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + ACNo + "' disabled />";
                            RecentData_append += "</td>";

                            RecentData_append += "</td>";
                            RecentData_append += "<td class='Recentvisitortabletd' style='width:120px;'>";
                            var equip = Recentdata[i].PermitITEquipments;
                            if (equip == true) {
                                RecentData_append += "<a id='tdaequipment" + Recentdata[i].RequestID + "' href='#' style='color:#006583;cursor:pointer;margin-left:10px;' onclick='OpenEquipment(" + Recentdata[i].VisitorID + ")'>Yes</a>";
                            }
                            else {
                                RecentData_append += "<a id='tdaequipment" + Recentdata[i].RequestID + "' href='#' style='color:#596679;cursor:pointer;margin-left:10px;' onclick='OpenEquipment(" + Recentdata[i].VisitorID + ")'>No</a>";
                            }
                            RecentData_append += "</td>";
                            RecentData_append += "<td style='width:1px;text-align:  center;'>";
                            RecentData_append += "<a id='tdvisitdetails" + Recentdata[i].RequestID + "' href='#' style='color:#006583;cursor:pointer;margin-left:10px;' onclick='Openvisitdetails(" + Recentdata[i].VisitorID + ")'>View</a>";
                            RecentData_append += "</td>";
                            RecentData_append += "<td style='width:1px;text-align:  center;'>";
                            RecentData_append += "<a id='tdlogdetails" + Recentdata[i].RequestID + "' href='#' style='color:#006583;cursor:pointer;margin-left:10px;' onclick='Openlogdetails(" + Recentdata[i].VisitorID + ")'>View</a>";
                            RecentData_append += "</td>";
                            RecentData_append += "<td style='width:1px;display:none;'>";
                            RecentData_append += "<input type='text' id='tdvisitorid" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].VisitorID + "' style='visibility:hidden;' />";
                            RecentData_append += "</td>";
                            RecentData_append += "</tr >";
                        }
                    }
                    $("#RecentVisitortablebody").html(RecentData_append);

                    $('#txtvisitdate').val(Recentdata[0].FromDate);
                    $('#txtstarttime').val(Recentdata[0].FromTime);
                    $('#txtendtime').val(Recentdata[0].ToTime);
                    $('#txtmeetinglocation').val(Recentdata[0].Facility);
                    
                    $('#txthostID').val(Recentdata[0].HostID);

                    var notifiedtimecheck = Recentdata[0].NotifiedTime;

                    if (notifiedtimecheck != null) {
                        var notifiedtime = new Date(notifiedtimecheck);
                        $('#notifytime').val(notifiedtime.toLocaleString());
                    }
                    else {
                        $('#notifytime').val(notifiedtimecheck);
                    }
                    var dispatchedtimecheck = Recentdata[0].Dispatchedtime;
                    if (dispatchedtimecheck != null) {
                        var dispatchedtime = new Date(dispatchedtimecheck)

                        $('#dispatchtime').val(dispatchedtime.toLocaleString());
                    }
                    else {
                        $('#dispatchtime').val(dispatchedtimecheck);
                    }

                    $('#textpasscollectedby').val(Recentdata[0].CollectedByName);

                    var checkcount = 0;
                    for (var check = 0; check < Recentdata.length; check++) {
                        if (Recentdata[check].badgestatus == "issued") {
                            checkcount = checkcount + 1;
                        }

                    }
                    if (checkcount == 0) {
                        $("#btnretcheckout").css("background-color", "grey");
                        $("#btnretcheckout").attr("disabled", "disabled");
                        $("#btnlcheckout").css("background-color", "grey");
                        $("#btnlcheckout").attr("disabled", "disabled");
                        $("#btnreissue").css("background-color", "grey");
                        $("#btnreissue").attr("disabled", "disabled");

                    }
                    var account = 0;
                    for (var i = 0; i < Recentdata.length; i++) {
                        
                        var Accessnumber = Recentdata[i].AccessCardNumber;
                        if (Accessnumber == "" || Accessnumber == null || Accessnumber == undefined) {
                        account = account + 1;
                        }
                    }
                    $('#hdnaccount').val(account);
                    if (account > 0) {

                    }
                    else {
                        $("#btnnotify").css("background-color", "#3188B5");
                        $("#btnnotify").removeAttr("disabled");
                    }
                   
                    if (Recentdata[0].badgestatus == "Host Notified") {
                        $("#btnupdate").css("background-color", "grey");
                        $("#btnupdate").attr("disabled", "disabled");
                        $("#btnnotify").css("background-color", "grey");
                        $("#btnnotify").attr("disabled", "disabled");
                    }
                    $("#modal").hide();
                    $("#fade").hide();
                },
                error: function (result) {
                   
                    $("#modal").hide();
                    $("#fade").hide();
                    alert("error");
                }
            });


        });

        function notifyClick() {

            var parentid = $('#hdnParentID').val();
            var securityid = $('#hdnSecurityID').val();
           
            var notifyclickvalue = 0;

            var chkArray = [];
            $(".dataaccesscheckbox:checked").each(function () {
                chkArray.push($(this).val());
            });
            var selected;
            selected = chkArray.join(',');
            var selectedspilt = selected.split(',');
            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/NotifyClick",
                contentType: "application/json;charset=utf-8",
                data: '{"parentid":"' + parentid + '","securityid":"' + securityid + '"}',
                dataType: "json",
                success: function (data) {
                    
                    notifyclickvalue = notifyclickvalue + 1;
                    SucessModel.style.display = "block";
                    tbodycart1.textContent = "Notification mail has been sent to the Host to collect the passes.";
                    $('#hdnnotifyclick').val(notifyclickvalue);
                    $('#hdnaccount').val(0);
                    $("#btnnotify").css("background-color", "grey");
                    $("#btnnotify").attr("disabled", "disabled");
                    $("#btnupdate").css("background-color", "grey");
                    $("#btnupdate").attr("disabled", "disabled");
                    

                },
                error: function (result) {
                    alert("error");
                }
            });


            return false;
        }

        function checkempty(ID) {
            var vc = $('#tdtxtvcard' + ID).val();
            if (vc == "" || vc == null) {
                $("#spnerror").text("*Error : Enter Vcard number");
                $("#tdtxtvcard" + ID).css('border-color', 'red');
                return false;
            }
            else {
                $("#spnerror").text("");
                $("#tdtxtvcard" + ID).css('border-color', 'black');
                return true;
            }
        }

        function checkoutclientconfirm() {
            CheckoutModel.style.display = "block";
        }

        $("#btncheckoutconfirm").click(function () {
            checkoutclient();
            $("#btncheckout").css("background-color", "grey");
            $("#btncheckout").attr("disabled", "disabled");
            CheckoutModel.style.display = "none";
        })
        $("#btncheckoutclose").click(function () {
            CheckoutModel.style.display = "none";
            $("#btncheckout").css("background-color", "#8ac542");
            $("#btncheckout").removeAttr("disabled");
        })

        function checkoutclient() {
            var parentid = $('#hdnParentID').val();
            var securityid = $('#hdnSecurityID').val();

            var mainarray = [];
            var selectArray = [];
            var unselectArray = [];
            var visitorselected = [];
            var visitorunselected = [];



            $(".dataaccesscheckbox").each(function () {
                mainarray.push($(this).val());
            });


            $(".dataaccesscheckbox:checked").each(function () {
                selectArray.push($(this).val());
            });

            var main;
            main = mainarray.join(',');
            var mainsplit = main.split(',');

            var selected;
            selected = selectArray.join(',');
            var selectedspilt = selected.split(',');


            for (var x = 0; x < mainsplit.length; x++) {
                var checkvalue = mainsplit[x];
                if (selected.indexOf(checkvalue) == -1) {
                    unselectArray.push(mainsplit[x]);
                }

            }

            var unselected;
            unselected = unselectArray.join(',');
            var unselectedsplit = unselected.split(',');


            if (selectedspilt.length > 0) {
                for (var i = 0; i < selectedspilt.length; i++) {
                    var request = selectedspilt[i];
                    var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                    visitorselected.push(visitorid);
                }
            }

            if (unselectedsplit.length > 0) {
                for (var i = 0; i < unselectedsplit.length; i++) {
                    var request = unselectedsplit[i];
                    var visitorid = $("#tdvisitorid" + unselectedsplit[i]).val();
                    visitorunselected.push(visitorid);
                }
            }

            var selectedvisitorid;
            selectedvisitorid = visitorselected.join(',');
            var selectedvisitors = selectedvisitorid.split(',');

            var unselectedvisitorid;
            unselectedvisitorid = visitorunselected.join(',');
            var unselectedvisitors = unselectedvisitorid.split(',');

            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/Checkoutclients",
                contentType: "application/json;charset=utf-8",
                data: '{"parentid":"' + parentid + '","requestid":"' + selectedspilt + '","securityid":"' + securityid + '","selectedvisitorid":"' + selectedvisitors + '","unselectedvisitorid":"' + unselectedvisitors + '"}',
                dataType: "json",
                success: function (data) {
                    
                    $("#btncheckout").css("background-color", "#596679");
                    $("#btncheckout").attr("disabled", "disabled");
                    SucessModel.style.display = "block";
                    tbodycart1.textContent = "Request are checkout successfully.";
                    },
                error: function (result) {
                    SucessModel.style.display = "block";
                    tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                }
            });

            return false;

        }

        function updatedetails(e) {
          
            $("#btnnotify").css("background-color", "grey");
            $("#btnnotify").attr("disabled", "disabled");
            var isReissue = "0";
            $("#fade").show();
            $("#modal").show();

            var chkArray = [];
            var accesscardarray = [];
            $(".dataaccesscheckbox:checked").each(function () {
                chkArray.push($(this).val());
            });
            var account = 0;
            var checknull = 0;
            var checkzero = 0;
            var selected;
            selected = chkArray.join(',');
            var selectedspilt = selected.split(',');

            var parentid = $('#hdnParentID').val();
            var securityid = $('#hdnSecurityID').val();
            var location = $('#hdnloginfacility').val().split('-')[1].toUpperCase();
            if (selectedspilt.length > 0) {

                //// duplicate check
                for (var i = 0; i < selectedspilt.length; i++) {
                    var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                    var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                    var request = selectedspilt[i];
                    var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                    accesscardarray.push($("#tdtxtaccesscard" + selectedspilt[i]).val());
                    $("#tdtxtaccesscard" + selectedspilt[i]).css('border-color', 'transparent');
                    $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'transparent');
  }
                var accesscardlist = accesscardarray.join(',');
                var accesscardsplit = accesscardlist.split(',');

                //// duplicate check.
                var sorted_arr = accesscardarray.slice().sort();
                var duplicatecard = [];
                for (var i = 0; i < sorted_arr.length - 1; i++) {
                    if (sorted_arr[i + 1] == sorted_arr[i]) {
                        if (sorted_arr[i] != "") {
                            duplicatecard.push(sorted_arr[i]);
                        }
                    }
                }
                if (duplicatecard.length > 0) {
                    if (selectedspilt.length > 0) {
                        for (var i = 0; i < selectedspilt.length; i++) {
                            var duplicatecount = 0;
                            for (var j = 0; j < duplicatecard.length; j++) {
                                if (duplicatecard[j] == $("#tdtxtaccesscard" + selectedspilt[i]).val()) {
                                    //$("#tdtxtaccesscard" + selectedspilt[i]).css('border-color', 'red');
                                    $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');
                                    
                                }
                            }
                         }
                        $("#spnerror").text("*Error : Duplicate  Vcard number.");
                        $("#modal").hide();
                        $("#fade").hide();
                        return false;
                    }
                }

    
                for (var i = 0; i < selectedspilt.length; i++) {
                    //// ////debugger;
                    var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                    var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                    var VcardNumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                    var request = selectedspilt[i];
                    var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                    if (VcardNumber == "" || VcardNumber == null || VcardNumber == undefined) {
                        checknull = checknull + 1;
                        $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');

                    }
                    if (checknull == selectedspilt.length) {
                        $("#spnerror").text("*Error : Kindly enter atleast one valid Vcard number");
                        $("#modal").hide();
                        $("#fade").hide();
                        return false;
                    }

                }

                var checkvalidvcard = 0;
                for (var i = 0; i < selectedspilt.length; i++) {
                    var VcardNumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                    if (VcardNumber.length < 7 && VcardNumber.length> 0) {
                        checkvalidvcard = checkvalidvcard + 1;
                        $("#tdtxtaccesscard" + selectedspilt[i]).val("");
                        $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');
                        $("#tdtxtaccesscard" + selectedspilt[i]).css('border-color', '1px black');
                    }
                }
                if (checkvalidvcard > 0) {
                    $("#spnerror").text("*Error : Kindly enter valid Vcard number(s)");
                    $("#btnupdate").css("background-color", "grey");
                    $("#btnupdate").attr("disabled", "disabled");
                    $("#btnnotify").css("background-color", "grey");
                    $("#btnnotify").attr("disabled", "disabled");
                    $("#btnreissue").css("background-color", "grey");
                    $("#btnreissue").attr("disabled", "disabled");
                    $("#modal").hide();
                    $("#fade").hide();
                    return false;
                }

                var checklocation = 0;
                var AccessCardnotPopulated = 0;
                for (var i = 0; i < selectedspilt.length; i++) {
                    var VcardNumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                    var AccessCardNumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                    if (VcardNumber.length == 7) {
                        var vcardlocation = VcardNumber.substring(0, 3).toUpperCase();
                        if ((vcardlocation == "" || vcardlocation == null || vcardlocation == undefined)) {
                        }
                        else {
                            if (location != vcardlocation) {
                                checklocation = checklocation + 1;
                                $("#tdtxtaccesscard" + selectedspilt[i]).val("");
                                $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');

                            }
                            else {
                                if (AccessCardNumber == "" || AccessCardNumber == null || AccessCardNumber == undefined)
                                {
                                    AccessCardnotPopulated = AccessCardnotPopulated + 1;
                                    $("#tdtxtaccesscard" + selectedspilt[i]).val("");
                                    $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');

                                }
                            }

                        }
                    }
                }
                if (checklocation > 0) {
                    $("#btnupdate").css("background-color", "grey");
                    $("#btnupdate").attr("disabled", "disabled");
                    $("#spnerror").text("*Error : Please assign a card which is tagged to your city.");
                    $("#modal").hide();
                    $("#fade").hide();
                    return false;
                }
                if (AccessCardnotPopulated > 0)
                {
                    $("#btnupdate").css("background-color", "grey");
                    $("#btnupdate").attr("disabled", "disabled");
                    $("#spnerror").text("*Error : Kindly enter valid Client Vcard Number(s)");
                    $("#modal").hide();
                    $("#fade").hide();
                    return false;
                }

                $.ajax({
                    type: "POST",
                    url: "ClientPage.aspx/Checkaccesscard",
                    contentType: "application/json;charset=utf-8",
                    data: '{"accesscards":"' + accesscardsplit + '","parentid":"' + parentid + '","isReissue":" '+ isReissue +' "}',
                    dataType: "json",
                    success: function (data) {
                       
                        var result = JSON.parse(data.d);
                        if (result.length > 0) {
                            if (selectedspilt.length > 0) {
                                for (var i = 0; i < selectedspilt.length; i++) {
                                   
                                    for (var j = 0; j < result.length; j++) {
                                        if (result[j] == $("#tdtxtaccesscard" + selectedspilt[i]).val()) {
                                            $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');
                                        }
                                    }

                                }
                                $("#spnerror").text("*Error : Entered v-card(s) is/are already in use.");

                                $("#modal").hide();
                                $("#fade").hide();
                                return false;
                            }
                        }
                        else {
                            // 

                            for (var i = 0; i < selectedspilt.length; i++) {
                                var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                                var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                                var Vcardnumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                                var request = selectedspilt[i];
                                var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                                $.ajax({
                                    type: "POST",
                                    url: "ClientPage.aspx/UpdateCardDetails",
                                    contentType: "application/json;charset=utf-8",
                                    data: '{"visitorname":"' + Name + '","accessnumber":"' + Accessnumber + '","requestid":"' + request + '","visitorid":"' + visitorid + '","vcardnumber":"' + Vcardnumber + '"}',
                                    dataType: "json",
                                    success: function (data) { //debugger
                                        //debugger;
                                        for (var i = 0; i < selectedspilt.length; i++) {
                                            var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                                            var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                                            var Vcardnumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                                            var request = selectedspilt[i];
                                            var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                                            if (Vcardnumber == "" || Vcardnumber == null || Vcardnumber == undefined) {

                                                account = account + 1;


                                            }
                                            else {
                                                $("#tdtxtvcard" + selectedspilt[i]).removeClass("Recentvisitortableaftertextbox");
                                                $("#tdtxtvcard" + selectedspilt[i]).addClass("Recentvisitortabletextbox");
                                                $("#tdtxtvcard" + selectedspilt[i]).attr("disabled", "disabled");
                                            }
                                        }
                                        $('#hdnaccount').val(account);
                                        if (account > 0) {

                                        }
                                        else {
                                           

                                            $("#btnnotify").css("background-color", "#3188B5");
                                            $("#btnnotify").removeAttr("disabled");
                                        }

                                        for (var i = 0; i < selectedspilt.length; i++) {
                                            var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                                            var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                                            var request = selectedspilt[i];
                                            var Vcardnumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                                            var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                                        }
                                        $("#modal").hide();
                                        $("#fade").hide();
                                        var span = document.getElementsByClassName("close")[0];
                                        var modal = document.getElementById('SucessModel');
                                        modal.style.display = "block";
                                        tbodycart1.textContent = "Details are updated successfully.";

                                    },
                                    error: function (result) {
                                        $("#modal").hide();
                                        $("#fade").hide();
                                        alert("Error Occured");

                                    }
                                });

                            }
                        }


                    },
                    error: function (result) {
                        $("#modal").hide();
                        $("#fade").hide();
                        SucessModel.style.display = "block";

                        tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                    }
                });





                //}
            }
            return false;
        }


        function Openvisitdetails(visitid) {
           
            var parentid = $('#hdnParentID').val();
            var securityid = $('#hdnSecurityID').val();
            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/GetVisitDetails",
                contentType: "application/json;charset=utf-8",
                data: "{'parentID':" + parentid + ",'visitorID':" + visitid + "}",
                dataType: "json",
                success: function (data) {
                    //// ////debugger;
                    //alert("success");
                    var Recentdata = JSON.parse(data.d);
                    var RecentData_append = "";
                    for (var i = 0; i < Recentdata.length; i++) {
                        if (i % 2 == 0) {
                            RecentData_append += "<tr class='Recentvisitortablebodytrodd' id='tr" + Recentdata[i].requestid + "'>";
                        }
                        else {
                            RecentData_append += "<tr class='Recentvisitortablebodytreven' id='tr" + Recentdata[i].requestid + "'>";
                        }
                        RecentData_append += "<td class='Recentvisitortabletd' style='width:168px;'>";
                        RecentData_append += "<input type='text' id='tdtxtlocation" + Recentdata[i].requestid + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].facility + "' disabled />";
                        RecentData_append += "</td>";
                        RecentData_append += "<td class='Recentvisitortabletd' style='width:150px;'>";

                        RecentData_append += "<div class='tooltip' style='margin-top: -2%; margin-left: 0%; float: right;'><span class='tooltiptext' style='width: 300px;margin-left: -162px;word-break: break-word;'>" + Recentdata[i].finalVisitDate + "</span>";
                        RecentData_append += "<input type='text' id='tdtxtfromdate" + Recentdata[i].requestid + "' style='width: 125px;text-align: left;' class='Recentvisitortabletextbox' value='" + Recentdata[i].finalVisitDate + "' disabled/>";
                        RecentData_append += "</div>";

                        ////RecentData_append += "<input type='text' id='tdtxtfromdate" + Recentdata[i].requestid + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].finalVisitDate + "' disabled />";
                        RecentData_append += "</td>";
                        //RecentData_append += "<td class='Recentvisitortabletd' style='width:150px;'>";
                        //RecentData_append += "<input type='text' id='tdtxtfromdate" + Recentdata[i].requestid + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].FromDate + "' disabled />";
                        //RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        RecentData_append += "<input type='text' id='tdfromtime" + Recentdata[i].requestid + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].FromTime + "' />";
                        RecentData_append += "</td>";
                        //RecentData_append += "<td style='width:1px;'>";
                        //RecentData_append += "<input type='text' id='tdtodate" + Recentdata[i].requestid + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].ToDate + "' />";
                        //RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        RecentData_append += "<input type='text' id='tdtotime" + Recentdata[i].requestid + "' class='Recentvisitortabletextbox' value='" + Recentdata[i].ToTime + "' />";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        RecentData_append += "<input type='text' id='tdhostid" + Recentdata[i].requestid + "' style='width: 190px;' class='Recentvisitortabletextbox' value='" + Recentdata[i].hostid + "' />";
                        RecentData_append += "</td>";
                        RecentData_append += "</tr >";
                    }


                    $("#visitdetailstbody").html(RecentData_append);
                    visitdetails.style.display = "block";

                },
                error: function (result) {
                    //alert("Error login");

                    alert(error);
                }
            });


        }

        function selectall() {
           
            var mainarray = [];
            $(".dataaccesscheckbox").each(function () {
                mainarray.push($(this).val());
            });
            var main;
            main = mainarray.join(',');
            var mainsplit = main.split(',');


            var s = $('.dataaccesscheckbox1').prop('checked');
            var able = $('.dataaccesscheckbox').prop('disabled');

            for (var able = 0; able < mainsplit.length; able++) {
                var ablecheck = $("#tdchkselect" + mainsplit[able]).prop('disabled');
                if ($("#btnreissue").val() == "Submit") {
                    if (s == false) {
                        if (ablecheck == false) {
                            $("#tdchkselect" + mainsplit[able]).prop('checked', false);
                            $("#tdtxtaccesscard" + mainsplit[able]).attr("disabled", "disabled");
                            $("#tdtxtaccesscard" + mainsplit[able]).css('border-color', 'transparent');
                           
                        }
                    }
                    else {
                        if (ablecheck == false) {
                            $("#tdchkselect" + mainsplit[able]).prop('checked', true);
                           
                            $("#tdtxtaccesscard" + mainsplit[able]).removeAttr("disabled");
                            $("#tdtxtaccesscard" + mainsplit[able]).css('border-color', 'white');
                           
                        }

                    }
                }
                else {
                    if (s == false) {
                        if (ablecheck == false) {
                            $("#tdchkselect" + mainsplit[able]).prop('checked', false);
                        }
                    }
                    else {
                        if (ablecheck == false) {
                            $("#tdchkselect" + mainsplit[able]).prop('checked', true);
                        }

                    }
                }


            }

        }

        function Openlogdetails(VID) { 
           
            var parentid = $('#hdnParentID').val();
            var securityid = $('#hdnSecurityID').val();
            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/GetLogDetails",
                contentType: "application/json;charset=utf-8",
                data: "{'parentID':" + parentid + ",'visitorID':" + VID + "}",
                dataType: "json",
                success: function (data) {
                    var Recentdata = JSON.parse(data.d);
                    console.log(JSON.stringify(data.d));
                    var RecentData_append = "";
                    for (var i = 0; i < Recentdata.length; i++) {
                        if (i % 2 == 0) {
                            RecentData_append += "<tr class='Recentvisitortablebodytrodd' id='tr" + Recentdata[i].RequestID + "'>";
                        }
                        else {
                            RecentData_append += "<tr class='Recentvisitortablebodytreven' id='tr" + Recentdata[i].RequestID + "'>";
                        }
                        RecentData_append += "<td style='width:1px;'>";
                        var logACno;
                        var logVCno;
                        if (Recentdata[i].AccessCardNumber != null) {
                            logACno = Recentdata[i].AccessCardNumber;
                            logVCno = Recentdata[i].VcardNumber;
                        }
                        else {
                            logACno = "";
                            logVCno = "";
                            if ($.trim(Recentdata[i].BadgeNo) != '')
                            {
                                logVCno = Recentdata[i].BadgeNo;

                                // Getting access card no with service call
                                logACno = GetAccessCardNo(logVCno);
                            }
                        }
                        RecentData_append += "<input type='text' style='width: 51px;' id='tdtxtvcardnumber" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + logVCno + "' disabled />";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        RecentData_append += "<input type='text' style='width: 51px;' id='tdtxtaccesscardnumber" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + logACno + "' disabled />";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        var finaldisaptchtime;
                        if (Recentdata[i].Dispatchedtime != null) { 
                            var logdispatchtime = new Date(Recentdata[i].Dispatchedtime);
                            finaldisaptchtime = logdispatchtime.toLocaleString();
                        }
                        else {
                            finaldisaptchtime = "";
                        }
                        RecentData_append += "<input type='text' id='tddispatchedtime" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' style='width: 145px;' value='" + finaldisaptchtime + "' disabled/>";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        var issuedfacility;
                        if (Recentdata[i].Facility != null && (Recentdata[i].AccessCardNumber != null || $.trim(Recentdata[i].BadgeNo) != '' )) {
                            issuedfacility = Recentdata[i].Facility;

                        }
                        else {
                            issuedfacility = "";
                        }
                        RecentData_append += "<input type='text' id='tdissuedfacility" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' style='width: 110px;' value='" + issuedfacility + "' disabled/>";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        var logcollectedby;
                        if (Recentdata[i].CollectedBy != null) {
                            logcollectedby = Recentdata[i].CollectedBy;
                        }
                        else {
                            logcollectedby = "";
                        }
                        RecentData_append += "<input type='text' id='tdcollectedby" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + logcollectedby + "' disabled/>";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:160px;'>";
                        var logBstatus;
                        if (Recentdata[i].ReissueStatus != null) {
                            logBstatus = Recentdata[i].ReissueStatus;
                        }
                        else if (Recentdata[i].badgestatus == null || (Recentdata[i].AccessCardNumber == null && $.trim(Recentdata[i]) == '')) {
                            logBstatus = "";
                        }
                        else {
                            logBstatus = Recentdata[i].badgestatus;
                        }

                        RecentData_append += "<div class='tooltip' style='margin-top: -1.5%; margin-left: 0%; float: right;'><span class='tooltiptext' style='width: 300px;margin-left: -162px;'>" + logBstatus + "</span>";
                        RecentData_append += "<input type='text' id='tdtxtbadgestatus" + Recentdata[i].RequestID + "' style='width: 125px;' class='Recentvisitortabletextbox' value='" + logBstatus + "' disabled/>";
                        RecentData_append += "</div>";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        var returnedby;
                        if (Recentdata[i].ReportedBy != null) {
                            returnedby = Recentdata[i].ReportedBy;
                        }
                        else {
                            returnedby = "";
                        }
                        RecentData_append += "<input type='text' id='tdreturnedby" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + returnedby + "' disabled/>";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        var returnedat;
                        if (Recentdata[i].ReportedFacility != null) {
                            returnedat = Recentdata[i].ReportedFacility;
                        }
                        else if (Recentdata[i].ReissuedFacility != null) {
                            returnedat = Recentdata[i].ReissuedFacility;
                        }
                        else {
                            returnedat = "";
                        }
                        RecentData_append += "<input type='text' id='tdreturnedat" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + returnedat + "' disabled/>";
                        RecentData_append += "</td>";
                        RecentData_append += "<td style='width:1px;'>";
                        var returnedon; 
                        if (Recentdata[i].ReportedOn != null) {
                          
                            var returntime = new Date(Recentdata[i].ReportedOn);
                            returnedon = returntime.toLocaleString();
                            ////dev comments : bug fix - june-20-2019.
                            ////The time of the 'lost and checkout' time of lost card is not saved,so was displayed 12 am default,trimming time as a result.
                            if ((Recentdata[i].badgestatus == 'Lost') && Recentdata[i].ReissueStatus == null) {  
                                
                                returnedon = returnedon.split(/[\s,]+/)[0];
                            }

                        }
                        else {
                            returnedon = "";
                        }
                        RecentData_append += "<input type='text' style='width: 154px;' id='tdreturnedon" + Recentdata[i].RequestID + "' class='Recentvisitortabletextbox' value='" + returnedon + "' disabled/>";
                        RecentData_append += "</td>";

                        RecentData_append += "</tr >";
                    }


                    $("#logdetailstbody").html(RecentData_append);
                    logdetails.style.display = "block";

                },
                error: function (result) {alert("error");
                }
            });
        }

        function returncheckoutconfirm() { 

            var selectArray = [];
            $(".dataaccesscheckbox:checked").each(function () {
                selectArray.push($(this).val());
            });
            if (selectArray.length <= 0) {
                $("#spnerror").text("*Error : Kindly select atleast one request.");
                $("#modal").hide();
                $("#fade").hide();
                return false;

            }
            else {
                $("#spnerror").text("");
                var securityid = $('#hdnSecurityID').val();
                var securityfacility = $('#hdnloginfacility').val();
                $.ajax({
                    type: "POST",
                    url: "ClientPage.aspx/LoadFacilitylist",
                    contentType: "application/json;charset=utf-8",
                    data: '{"securityid":"' + securityid + '"}',
                    dataType: "json",
                    success: function (data) {
                       
                        var result = JSON.parse(data.d);
                        for (var list = 0; list < result.length; list++) {
                            
                            $("#returnddlfacility").append($("<option></option>").val(result[list]).html(result[list]));

                        }
                        
                        $("#returnddlfacility").val(securityfacility);
                    },
                    error: function (result) {
                        
                        SucessModel.style.display = "block";
                        tbodycart1.textContent = "Error occured.";
                    }
                });
                returnmodal.style.display = "block";
            }
        }

        function lostcheckoutconfirm() {
            var selectArray = [];
            $(".dataaccesscheckbox:checked").each(function () {
                selectArray.push($(this).val());
            });
            if (selectArray.length <= 0) {
                $("#spnerror").text("*Error : Kindly select atleast one request.");
                $("#modal").hide();
                $("#fade").hide();
                return false;

            }
            else {
                $("#spnerror").text("");
                var securityid = $('#hdnSecurityID').val();
                var securityfacility = $('#hdnloginfacility').val();
                $.ajax({
                    type: "POST",
                    url: "ClientPage.aspx/LoadFacilitylist",
                    contentType: "application/json;charset=utf-8",
                    data: '{"securityid":"' + securityid + '"}',
                    dataType: "json",
                    success: function (data) {
                        
                        var result = JSON.parse(data.d);
                        for (var list = 0; list < result.length; list++) {
                          
                            $("#lostddlfacility").append($("<option selected=" + securityfacility + " value=" + securityfacility + "></option>").val(result[list]).html(result[list]));
                        }
                        $("#lostddlfacility").val(securityfacility);
                    },
                    error: function (result) {
                        
                        SucessModel.style.display = "block";
                        tbodycart1.textContent = "Error occured.";
                    }
                });
                lostmodal.style.display = "block";
            }
        }

        function reissuecardsconfirm() { 
            $("#modal").show();
            $("#fade").show();
            var parentid = $('#hdnParentID').val();
            var securityid = $('#hdnSecurityID').val();
            var location = $('#hdnloginfacility').val().split('-')[1].toUpperCase();

            var mainarray = [];
            var selectArray = [];
            var unselectArray = [];
            var visitorselected = [];
            var visitorunselected = [];
            var accesscardarray = [];
            var checkzero = 0;
            var checknull = 0;

            var isReissue = "1";

            $(".dataaccesscheckbox").each(function () {
                mainarray.push($(this).val());
            });


            $(".dataaccesscheckbox:checked").each(function () {
                selectArray.push($(this).val());
            });

            var main;
            main = mainarray.join(',');
            var mainsplit = main.split(',');

            var selected;
            selected = selectArray.join(',');
            var selectedspilt = selected.split(',');
            for (var x = 0; x < mainsplit.length; x++) {
                var checkvalue = mainsplit[x];
                if (selected.indexOf(checkvalue) == -1) {
                    unselectArray.push(mainsplit[x]);
                }

            }

            var unselected;
            unselected = unselectArray.join(',');
            var unselectedsplit = unselected.split(',');
            var vcardarray = [];

            if ($("#btnreissue").val() == "Submit") {
                  if (selectedspilt.length > 0) {
                      for (var i = 0; i < selectedspilt.length; i++) {
                          var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                          var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                          var request = selectedspilt[i];
                          var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                          accesscardarray.push($("#tdtxtaccesscard" + selectedspilt[i]).val());
                          $("#tdtxtaccesscard" + selectedspilt[i]).css('border-color', 'transparent');
                          $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'transparent');
                      }
                      var accesscardlist = accesscardarray.join(',');
                      var accesscardsplit = accesscardlist.split(',');

                      //// duplicate check.
                      var sorted_arr = accesscardarray.slice().sort();
                      var duplicatecard = [];
                      for (var i = 0; i < sorted_arr.length - 1; i++) {
                          if (sorted_arr[i + 1] == sorted_arr[i]) {
                              if (sorted_arr[i] != "") {
                                  duplicatecard.push(sorted_arr[i]);
                              }
                          }
                      }
                      if (duplicatecard.length > 0) { 
                          if (selectedspilt.length > 0) {
                              for (var i = 0; i < selectedspilt.length; i++) {
                                  var duplicatecount = 0;
                                  for (var j = 0; j < duplicatecard.length; j++) {
                                      if (duplicatecard[j] == $("#tdtxtaccesscard" + selectedspilt[i]).val()) {
                                          // $("#tdtxtaccesscard" + selectedspilt[i]).css('border-color', 'red');
                                          $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');

                                      }
                                  }
                              }
                              $("#spnerror").text("*Error : Duplicate Vcard number.");
                              $("#modal").hide();
                              $("#fade").hide();
                              return false;
                          }
                      }
                     

                  
                      for (var i = 0; i < selectedspilt.length; i++) {
                      
                        var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                        var Vcardnumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                        if (Vcardnumber == "" || Vcardnumber == null || Vcardnumber == undefined) {
                            //checknull = checknull + 1;
                            $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');
                            $("#spnerror").text("*Error : Kindly enter Vcard number(s)");
                            $("#modal").hide();
                            $("#fade").hide();
                            return false;
                        }
                    }

                    var checkvalidvcard = 0;
                    for (var i = 0; i < selectedspilt.length; i++) {
                        var VcardNumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                        if (VcardNumber.length < 7 && VcardNumber.length > 0) {
                            checkvalidvcard = checkvalidvcard + 1;
                            $("#tdtxtaccesscard" + selectedspilt[i]).val("");
                            $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');
                            $("#tdtxtaccesscard" + selectedspilt[i]).css('border-color', '1px black');
                        }
                    }
                    if (checkvalidvcard > 0) {
                        $("#spnerror").text("*Error : Kindly enter valid Vcard number(s)");
                        $("#btnreissue").css("background-color", "grey");
                        $("#btnreissue").attr("disabled", "disabled");
                        $("#modal").hide();
                        $("#fade").hide();
                        return false;
                    }

                    var checklocation = 0;
                    var AccessCardnotPopulated = 0;
                    for (var i = 0; i < selectedspilt.length; i++) {

                        var AccessCardNumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                        var VcardNumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                        if (VcardNumber.length == 7) {
                            var vcardlocation = VcardNumber.substring(0, 3).toUpperCase();
                            if ((vcardlocation == "" || vcardlocation == null || vcardlocation == undefined)) { }
                            else {
                                if (location != vcardlocation) { //// eg : if loc is KOC and entry is : MUMGHGT
                                    checklocation = checklocation + 1;
                                    $("#tdtxtaccesscard" + selectedspilt[i]).val("");
                                    $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');

                                }

                                else {///// example : if loc is KOC and entry is : KOCTYUY
                                    if (AccessCardNumber == "" || AccessCardNumber == null || AccessCardNumber == undefined) {
                                        AccessCardnotPopulated = AccessCardnotPopulated + 1;
                                        $("#tdtxtaccesscard" + selectedspilt[i]).val("");
                                        $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');
                                    }
                                }

                            }
                        }
                    }
                    if (checklocation > 0) {
                       
                        $("#btnreissue").css("background-color", "grey");
                        $("#btnreissue").attr("disabled", "disabled");
                        $("#spnerror").text("*Error : Please assign a card which is tagged to your city.");
                        $("#modal").hide();
                        $("#fade").hide();
                        return false;
                    }

                    if (AccessCardnotPopulated > 0)
                    {
                        $("#btnreissue").css("background-color", "grey");
                        $("#btnreissue").attr("disabled", "disabled");
                        $("#spnerror").text("*Error : Kindly enter valid Client Vcard number(s).");
                        $("#modal").hide();
                        $("#fade").hide();
                        return false;
                    }



                    $.ajax({
                        type: "POST",
                        url: "ClientPage.aspx/Checkaccesscard",
                        contentType: "application/json;charset=utf-8",
                        data: '{"accesscards":"' + accesscardsplit + '","parentid":"' + parentid + '","isReissue":"' + isReissue +'"}',
                        dataType: "json",
                        success: function (data) {
                          
                            var result = JSON.parse(data.d);
                            var samecardcheck = 0;
                            if (result.length > 0) {
                                if (selectedspilt.length > 0) {
                                    for (var i = 0; i < selectedspilt.length; i++) {
                                       
                                        for (var j = 0; j < result.length; j++) {
                                            if (result[j] == $("#tdtxtaccesscard" + selectedspilt[i]).val()) {
                                                $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');

                                            }
                                        }
                                    }
                                    $("#spnerror").text("*Error : Entered v-card(s) is/are already in use.");
                                    $("#modal").hide();
                                    $("#fade").hide();
                                    return false;



                                }

                            }
                            else {
                                $("#modal").hide();
                                $("#fade").hide();
                                reissuemodal.style.display = "block";
                            }
                        },
                        error: function (result) {
                            $("#modal").hide();
                            $("#fade").hide();
                            SucessModel.style.display = "block";
                            tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                        }
                    });

                }
                else {
                    $("#spnerror").text("*Error : Kindly select atleast one request.");
                    $("#modal").hide();
                    $("#fade").hide();
                    return false;
                }

            }
            else {
                if (selectArray.length <= 0) {
                    $("#spnerror").text("*Error : Kindly select atleast one request.");
                    $("#modal").hide();
                    $("#fade").hide();
                    return false;

                }
                else {
                    $("#spnerror").text("");
                    var securityid = $('#hdnSecurityID').val();

                    var securityfacility = $('#hdnloginfacility').val();

                    $.ajax({
                        type: "POST",
                        url: "ClientPage.aspx/LoadFacilitylist",
                        contentType: "application/json;charset=utf-8",
                        data: '{"securityid":"' + securityid + '"}',
                        dataType: "json",
                        success: function (data) {
                           
                            var result = JSON.parse(data.d);
                            for (var list = 0; list < result.length; list++) {
                                $("#reissueddlfacility").append($("<option selected=" + securityfacility + " value=" + securityfacility + "></option>").val(result[list]).html(result[list]));

                            }
                            $("#reissueddlfacility").val(securityfacility);
                        },
                        error: function (result) {
                            
                            SucessModel.style.display = "block";
                            tbodycart1.textContent = "Error occured.";
                        }
                    });

                    $.ajax({
                        type: "POST",
                        url: "ClientPage.aspx/LoadReissuelist",
                        contentType: "application/json;charset=utf-8",
                        data: '{}',
                        dataType: "json",
                        success: function (data) {
                            
                            var result = JSON.parse(data.d);
                            for (var list = 0; list < result.length; list++) {
                                $("#ddlreason").append($("<option></option>").val(result[list]).html(result[list]));
                            }

                        },
                        error: function (result) {
                          
                            SucessModel.style.display = "block";
                            tbodycart1.textContent = "Error occured.";
                        }
                    });


                    if (selectedspilt.length > 0) {
                        for (var i = 0; i < selectedspilt.length; i++) {

                            //$("#tdtxtaccesscard" + selectedspilt[i]).removeAttr("disabled");
                            //$("#tdtxtaccesscard" + selectedspilt[i]).css('border-color', 'white');

                            $("#tdtxtvcard" + selectedspilt[i]).removeAttr("disabled");
                            $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'black');

                        }
                    }
                    var selectedvisitorid;
                    selectedvisitorid = visitorselected.join(',');
                    var selectedvisitors = selectedvisitorid.split(',');

                    var unselectedvisitorid;
                    unselectedvisitorid = visitorunselected.join(',');
                    var unselectedvisitors = unselectedvisitorid.split(',');

                    $("#modal").hide();
                    $("#fade").hide();
                    SucessModel.style.display = "block";
                    tbodycart1.textContent = "Kindly update the  V-card number for the selected request and click on submit button.";
                    //// $("#spnerror").text("*Error : Kindly update the Access card number for the selected request and click on submit button.");
                    $("#btnreissue").val("Submit");
                    $("#btnretcheckout").css("background-color", "grey");
                    $("#btnretcheckout").attr("disabled", "disabled");
                    $("#btnlcheckout").css("background-color", "grey");
                    $("#btnlcheckout").attr("disabled", "disabled");
                    $("#btnback").css("background-color", "grey");
                    $("#btnback").attr("disabled", "disabled");
                }
            }
        }


        function reissuecompleteclick() {

            $("#modal").show();
            $("#fade").show();
            var reissuerequesttext = $("#txtreissuerequestedby").val();
            var reissuecollectedtext = $("#txtreissuerequestedby").val();
            var ddlreason = document.getElementById('ddlreason');
            var reissuereason = ddlreason.options[ddlreason.selectedIndex].value;
            reissuereason = reissuereason + " and Reissued";
            var ddlfaclity = document.getElementById('reissueddlfacility');
            var selectedfacility = ddlfaclity.options[ddlfaclity.selectedIndex].text;
         
            if (reissuerequesttext == "" || reissuecollectedtext == "" || reissuerequesttext == "000000" || reissuecollectedtext == "000000" || reissuerequesttext == null || reissuecollectedtext == null) {

                reissuecheckspan.style.display = "block";
                $("#modal").hide();
                $("#fade").hide();
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "ClientPage.aspx/Checkvalidassociate",
                    contentType: "application/json;charset=utf-8",
                    data: '{"reportedby":"' + reissuerequesttext + '"}',
                    dataType: "json",
                    success: function (data) {
                        //// ////debugger;
                        var Returndata = JSON.parse(data.d);
                        if (Returndata.length > 0) {
                            var parentid = $('#hdnParentID').val();
                            var securityid = $('#hdnSecurityID').val();

                            var mainarray = [];
                            var selectArray = [];
                            var unselectArray = [];
                            var visitorselected = [];
                            var visitorunselected = [];



                            $(".dataaccesscheckbox").each(function () {
                                mainarray.push($(this).val());
                            });


                            $(".dataaccesscheckbox:checked").each(function () {
                                selectArray.push($(this).val());
                            });

                            var main;
                            main = mainarray.join(',');
                            var mainsplit = main.split(',');

                            var selected;
                            selected = selectArray.join(',');
                            var selectedspilt = selected.split(',');


                            for (var x = 0; x < mainsplit.length; x++) {
                                var checkvalue = mainsplit[x];
                                if (selected.indexOf(checkvalue) == -1) {
                                    unselectArray.push(mainsplit[x]);
                                }

                            }

                            var unselected;
                            unselected = unselectArray.join(',');
                            var unselectedsplit = unselected.split(',');


                            if (selectedspilt.length > 0) {
                                for (var i = 0; i < selectedspilt.length; i++) {
                                    var request = selectedspilt[i];
                                    var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                                    visitorselected.push(visitorid);
                                }
                            }

                            if (unselectedsplit.length > 0) {
                                for (var i = 0; i < unselectedsplit.length; i++) {
                                    var request = unselectedsplit[i];
                                    var visitorid = $("#tdvisitorid" + unselectedsplit[i]).val();
                                    visitorunselected.push(visitorid);
                                }
                            }

                            var selectedvisitorid;
                            selectedvisitorid = visitorselected.join(',');
                            var selectedvisitors = selectedvisitorid.split(',');

                            var unselectedvisitorid;
                            unselectedvisitorid = visitorunselected.join(',');
                            var unselectedvisitors = unselectedvisitorid.split(',');

                            for (var i = 0; i < selectedspilt.length; i++) {
                                var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                                var Accessnumber = $("#tdtxtaccesscard" + selectedspilt[i]).val();
                                var Vcardnumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                                var request = selectedspilt[i];
                                var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                                var OrginalVcardnumber = $("#hdntdtxtvcard" + selectedspilt[i]).val(); 
                                $.ajax({
                                    type: "POST",
                                    url: "ClientPage.aspx/Reissuecards",
                                    contentType: "application/json;charset=utf-8",
                                    data: '{"visitorname":"' + Name +
                                    '","accessnumber":"' + Accessnumber +
                                    '","vcardnumber":"' + OrginalVcardnumber +
                                    '","Newvcardnumber":"' + Vcardnumber +
                                    '","requestid":"' + request + '","visitorid":"' + visitorid + '","parentid":"' + parentid + '","reissuedby":"' + reissuerequesttext + '","recollectedby":"' + reissuecollectedtext + '","reissuereason":"' + reissuereason + '","selectedfacility":"' + selectedfacility + '"}',
                                    dataType: "json",
                                    success: function (data) {
                                        ////debugger;
                                        reissuemodal.style.display = "none";
                                        $("#txtreissuerequestedby").val("");
                                        $("#txtreissuecollectedby").val("");
                                        $("#txtreissuerequestedby").val("");
                                        $("#txtlostretunredby").val("");
                                        $("#txtreturnedby").val("");
                                        $("#txtcheckedassociatelost").attr("style", "display:none");
                                        $("#txtcheckassociatereturn").attr("style", "display:none");
                                        $("#txtcheckedassociatereissue").attr("style", "display:none");
                                        $("#btnreissue").val("Re - Issue");
                                        $("#btnretcheckout").css("background-color", "#3188B5");
                                        $("#btnretcheckout").removeAttr("disabled");
                                        $("#btnlcheckout").css("background-color", "#3188B5");
                                        $("#btnlcheckout").removeAttr("disabled");

                                        for (var main = 0; main < mainsplit.length; main++) {
                                            $("#tdchkselect" + mainsplit[main]).removeAttr("disabled");
                                            $("#tdchkselect" + mainsplit[main]).attr("checked", "checked");
                                        }
                                        $("#modal").hide();
                                        $("#fade").hide();
                                        var modal = document.getElementById('SucessModel');
                                        modal.style.display = "block";
                                        tbodycart1.textContent = "Vcard(s) has been re-issued successfully.";

                                    },
                                    error: function (result) {
                                        $("#modal").hide();
                                        $("#fade").hide();
                                        alert("Error Occured");

                                    }
                                });
                            }
                        }
                        else {
                            reissuecheckspan.style.display = "block";
                            $("#modal").hide();
                            $("#fade").hide();
                        }

                    },
                    error: function (result) {
                        SucessModel.style.display = "block";
                        tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                    }
                });

            }
        }

        function returnandcheckout() {
           
            var returntext = $("#txtreturnedby").val();
            var ddlfaclity = document.getElementById('returnddlfacility');
            var selectedfacility = ddlfaclity.options[ddlfaclity.selectedIndex].value;
            if (returntext == null || returntext == "" || returntext == undefined || returntext == "000000") {
                returncheckspan.style.display = "block";
            }

            else {

                $.ajax({
                    type: "POST",
                    url: "ClientPage.aspx/Checkvalidassociate",
                    contentType: "application/json;charset=utf-8",
                    data: '{"reportedby":"' + returntext + '"}',
                    dataType: "json",
                    success: function (data) {
                        //// ////debugger;
                        var Returndata = JSON.parse(data.d);
                        if (Returndata.length > 0) {
                            returncheckspan.style.display = "none";
                            var parentid = $('#hdnParentID').val();
                            var securityid = $('#hdnSecurityID').val();

                            var mainarray = [];
                            var selectArray = [];
                            var unselectArray = [];
                            var visitorselected = [];
                            var visitorunselected = [];



                            $(".dataaccesscheckbox").each(function () {
                                mainarray.push($(this).val());
                            });


                            $(".dataaccesscheckbox:checked").each(function () {
                                selectArray.push($(this).val());
                            });

                            var main;
                            main = mainarray.join(',');
                            var mainsplit = main.split(',');

                            var selected;
                            selected = selectArray.join(',');
                            var selectedspilt = selected.split(',');


                            for (var x = 0; x < mainsplit.length; x++) {
                                var checkvalue = mainsplit[x];
                                if (selected.indexOf(checkvalue) == -1) {
                                    unselectArray.push(mainsplit[x]);
                                }

                            }

                            var unselected;
                            unselected = unselectArray.join(',');
                            var unselectedsplit = unselected.split(',');


                            if (selectedspilt.length > 0) {
                                for (var i = 0; i < selectedspilt.length; i++) {
                                    var request = selectedspilt[i];
                                    var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                                    visitorselected.push(visitorid);
                                }
                            }

                            if (unselectedsplit.length > 0) {
                                for (var i = 0; i < unselectedsplit.length; i++) {
                                    var request = unselectedsplit[i];
                                    var visitorid = $("#tdvisitorid" + unselectedsplit[i]).val();
                                    visitorunselected.push(visitorid);
                                }
                            }

                            var selectedvisitorid;
                            selectedvisitorid = visitorselected.join(',');
                            var selectedvisitors = selectedvisitorid.split(',');

                            var unselectedvisitorid;
                            unselectedvisitorid = visitorunselected.join(',');
                            var unselectedvisitors = unselectedvisitorid.split(',');
                            returncheckspan.style.display = "none";
                            $.ajax({
                                type: "POST",
                                url: "ClientPage.aspx/ReturnandCheckout",
                                contentType: "application/json;charset=utf-8",
                                data: '{"parentid":"' + parentid + '","requestid":"' + selectedspilt + '","securityid":"' + securityid + '","selectedvisitorid":"' + selectedvisitors + '","selectedfacility":"' + selectedfacility + '","reportedby":"' + returntext + '"}',
                                dataType: "json",
                                success: function (data) {
                                    //// ////debugger;
                                    var Returndata = JSON.parse(data.d);
                                    var checkcount = 0;
                                    for (var check = 0; check < Returndata.length; check++) {
                                        if (Returndata[check].BadgeStatus == "issued") {
                                            checkcount = checkcount + 1;
                                        }
                                        else {
                                            $("#tdchkselect" + Returndata[check].RequestID).attr("disabled", "disabled");
                                            $("#tdchkselect" + Returndata[check].RequestID).removeAttr("checked");
                                        }
                                    }

                                    if (checkcount == 0) {
                                        $("#btnretcheckout").css("background-color", "grey");
                                        $("#btnretcheckout").attr("disabled", "disabled");
                                        $("#btnlcheckout").css("background-color", "grey");
                                        $("#btnlcheckout").attr("disabled", "disabled");
                                        $("#btnreissue").css("background-color", "grey");
                                        $("#btnreissue").attr("disabled", "disabled");
                                    }

                                    $("#txtreturnedby").val("");
                                    $("#txtreissuerequestedby").val("");
                                    $("#txtlostretunredby").val("");
                                    $("#txtreturnedby").val("");
                                    $("#txtcheckedassociatelost").attr("style", "display:none");
                                    $("#txtcheckassociatereturn").attr("style", "display:none");
                                    $("#txtcheckedassociatereissue").attr("style", "display:none");
                                    returnmodal.style.display = "none";
                                    SucessModel.style.display = "block";
                                    tbodycart1.textContent = "Pass returned successfully.";
                                    //}


                                },
                                error: function (result) {
                                    SucessModel.style.display = "block";
                                    tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                                }
                            });
                        }
                        else {
                            returncheckspan.style.display = "block";
                            $("#modal").hide();
                            $("#fade").hide();
                        }

                    },
                    error: function (result) {
                        SucessModel.style.display = "block";
                        tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                    }
                });



            }
            return false;
        }

        function lostandcheckout() {
            //// ////debugger;
            ////alert("lostandcheckout")
            var losttext = $("#txtlostreturnedby").val();
            var ddlfaclity = document.getElementById('lostddlfacility');
            var selectedfacility = ddlfaclity.options[ddlfaclity.selectedIndex].value;
            var reporteddate = $("#txtreporteddate").val();

            if (losttext == null || losttext == "" || losttext == undefined || losttext == "000000") {
                lostcheckspan.style.display = "block";
            }
            else if (reporteddate == null || reporteddate == undefined || reporteddate == "") {
                lostcheckspan.style.display = "none";
                lostdatespan.style.display = "block";
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "ClientPage.aspx/Checkvalidassociate",
                    contentType: "application/json;charset=utf-8",
                    data: '{"reportedby":"' + losttext + '"}',
                    dataType: "json",
                    success: function (data) {
                        //// ////debugger;
                        var Returndata = JSON.parse(data.d);
                        if (Returndata.length > 0) {
                            lostcheckspan.style.display = "none";
                            lostdatespan.style.display = "none";
                            var parentid = $('#hdnParentID').val();
                            var securityid = $('#hdnSecurityID').val();

                            var mainarray = [];
                            var selectArray = [];
                            var unselectArray = [];
                            var visitorselected = [];
                            var visitorunselected = [];



                            $(".dataaccesscheckbox").each(function () {
                                mainarray.push($(this).val());
                            });


                            $(".dataaccesscheckbox:checked").each(function () {
                                selectArray.push($(this).val());
                            });

                            var main;
                            main = mainarray.join(',');
                            var mainsplit = main.split(',');

                            var selected;
                            selected = selectArray.join(',');
                            var selectedspilt = selected.split(',');


                            for (var x = 0; x < mainsplit.length; x++) {
                                var checkvalue = mainsplit[x];
                                if (selected.indexOf(checkvalue) == -1) {
                                    unselectArray.push(mainsplit[x]);
                                }

                            }

                            var unselected;
                            unselected = unselectArray.join(',');
                            var unselectedsplit = unselected.split(',');
                            var lostvcardarray = [];


                            if (selectedspilt.length > 0) {
                                for (var i = 0; i < selectedspilt.length; i++) {
                                    var request = selectedspilt[i];
                                    var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                                    visitorselected.push(visitorid);
                                    var lostcard = $("#tdtxtvcard" + selectedspilt[i]).val();
                                    lostvcardarray.push(lostcard);

                                }
                            }

                            if (unselectedsplit.length > 0) {
                                for (var i = 0; i < unselectedsplit.length; i++) {
                                    var request = unselectedsplit[i];
                                    var visitorid = $("#tdvisitorid" + unselectedsplit[i]).val();
                                    visitorunselected.push(visitorid);
                                }
                            }

                            var selectedvisitorid;
                            selectedvisitorid = visitorselected.join(',');
                            var selectedvisitors = selectedvisitorid.split(',');
                            var vcardlist = lostvcardarray.join(',').split(',');
                            ////debugger
                            var unselectedvisitorid;
                            unselectedvisitorid = visitorunselected.join(',');
                            var unselectedvisitors = unselectedvisitorid.split(',');
                           
                            $.ajax({
                                type: "POST",
                                url: "ClientPage.aspx/LostandCheckout",
                                contentType: "application/json;charset=utf-8",
                                data: '{"parentid":"' + parentid +
                                '","requestid":"' + selectedspilt +
                                '","securityid":"' + securityid +
                                '","selectedvisitorid":"' + selectedvisitors +
                                '","lostvcards":"' + vcardlist +
                                '","selectedfacility":"' + selectedfacility +
                                '","reportedby":"' + losttext + '","reportedon":"' + reporteddate + '"}',
                                dataType: "json",
                                success: function (data) {
                                    ////debugger;
                                    var Returndata = JSON.parse(data.d);
                                    if (Returndata != "Error occured") {
                                        var checkcount = 0;
                                        for (var check = 0; check < Returndata.length; check++) {
                                            if (Returndata[check].BadgeStatus == "issued") {
                                                checkcount = checkcount + 1;
                                            }
                                            else {
                                                $("#tdchkselect" + Returndata[check].RequestID).attr("disabled", "disabled");
                                                $("#tdchkselect" + Returndata[check].RequestID).removeAttr("checked");
                                            }
                                        }
                                        if (checkcount == 0) {
                                            $("#btnretcheckout").css("background-color", "grey");
                                            $("#btnretcheckout").attr("disabled", "disabled");
                                            $("#btnlcheckout").css("background-color", "grey");
                                            $("#btnlcheckout").attr("disabled", "disabled");
                                            $("#btnreissue").css("background-color", "grey");
                                            $("#btnreissue").attr("disabled", "disabled");

                                        }
                                        $("#txtlostreturnedby").val("");
                                        $("#txtreissuerequestedby").val("");
                                        $("#txtlostretunredby").val("");
                                        $("#txtreturnedby").val("");
                                        $("#txtcheckedassociatelost").attr("style", "display:none");
                                        $("#txtcheckassociatereturn").attr("style", "display:none");
                                        $("#txtcheckedassociatereissue").attr("style", "display:none");
                                        lostmodal.style.display = "none";
                                        SucessModel.style.display = "block";
                                        tbodycart1.textContent = "Lost and checkout successfully done.";
                                        //}
                                    }
                                    else {
                                        ////debugger
                                        lostcheckspan.style.display = "block";
                                        $("#modal").hide();
                                        $("#fade").hide();
                                    }

                                },
                                error: function (result) {
                                    ////debugger
                                    SucessModel.style.display = "block";
                                    tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                                }
                            });
                        }
                        else {
                            ////debugger
                            lostcheckspan.style.display = "block";
                            $("#modal").hide();
                            $("#fade").hide();
                        }

                    },
                    error: function (result) {
                        SucessModel.style.display = "block";
                        tbodycart1.textContent = "Error occured. Kindly check and submit again.";
                    }
                });





            }
            return false;
        }

        function resetvalue() {
            //// ////debugger;
            window.location.reload();
        }

        function associatecheck() {

            //// ////debugger;
            var checkassociate;

            var reissueassociatecheck = $("#txtreissuerequestedby").val();
            var lostassocaitecheck = $("#txtlostreturnedby").val();
            var returnassociatecheck = $("#txtreturnedby").val();

            if (reissueassociatecheck != "") {
                checkassociate = reissueassociatecheck;
            }
            else if (lostassocaitecheck != "") {
                checkassociate = lostassocaitecheck;
            }
            else {
                checkassociate = returnassociatecheck;
            }
            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/Checkvalidassociate",
                contentType: "application/json;charset=utf-8",
                data: '{"reportedby":"' + checkassociate + '"}',
                dataType: "json",
                success: function (data) {
                    //// ////debugger;
                    var Returndata = JSON.parse(data.d);
                    if (Returndata.length > 0) {
                        $("#txtcheckedassociatelost").attr("style", "display:block");
                        $("#txtcheckedassociatelost").val(Returndata[0].LastName + "," + Returndata[0].FirstName + "(" + checkassociate + ")");
                        $("#txtcheckassociatereturn").attr("style", "display:block");
                        $("#txtcheckassociatereturn").val(Returndata[0].LastName + "," + Returndata[0].FirstName + "(" + checkassociate + ")");
                        $("#txtcheckedassociatereissue").attr("style", "display:block");
                        $("#txtcheckedassociatereissue").val(Returndata[0].LastName + "," + Returndata[0].FirstName + "(" + checkassociate + ")");

                        $("#lostcheckspan").attr("style", "display:none");
                        $("#reissuecheckspan").attr("style", "display:none");
                        $("#returncheckspan").attr("style", "display:none");
                    }
                    else {
                        $("#txtcheckedassociatelost").attr("style", "display:none");
                        $("#txtcheckassociatereturn").attr("style", "display:none");
                        $("#txtcheckedassociatereissue").attr("style", "display:none");
                        $("#lostcheckspan").attr("style", "display:block");
                        $("#reissuecheckspan").attr("style", "display:block");
                        $("#returncheckspan").attr("style", "display:block");
                    }
                },
                error: function (data) {

                }
            });
        }

        function UserSelectChange(requestid) {
            ////debugger;
            if ($("#btnreissue").val() == "Submit") { ////debugger
                var ablecheck = $("#tdchkselect" + requestid).prop('checked');
                if (ablecheck == false) {
                    /////// $("#tdchkselect" + requestid).prop('checked', true);
                    $("#tdtxtaccesscard" + requestid).attr("disabled", "disabled");
                    $("#tdtxtaccesscard" + requestid).css('border-color', 'transparent');
                    ////$("#tdtxtvcard" + requestid).attr("disabled", "disabled");
                    ////$("#tdtxtvcard" + requestid).css('border-color', 'transparent');

                }

                else {
                    //// $("#tdchkselect" + requestid).prop('checked', false);
                   // $("#tdtxtaccesscard" + requestid).removeAttr("disabled");
                   // $("#tdtxtaccesscard" + requestid).css('border-color', 'white');
                    ////$("#tdtxtaccesscard" + requestid).val("");                     
                }
            }
        }
        function OpenEquipment(ID) {
           
            $("#txtlaptopMake").val("");
            $("#txtlaptopModel").val("");
            $("#txtlaptopserial").val("");
            $("#txtdatastoragedeviceMake").val("");
            $("#txtdatastoragedeviceModel").val("");
            $("#txtdatastoragedeviceSerial").val("");
            $("#txtUSBharddiskMake").val("");
            $("#txtUSBharddiskModel").val("");
            $("#txtUSBharddiskSerial").val("");
            $("#txtcameraMake").val("");
            $("#txtcameraModel").val("");
            $("#txtcameraSerial").val("");
            $("#txtIPodMake").val("");
            $("#txtIPodModel").val("");
            $("#txtIPodSerial").val("");
            $("#txtotherMake").val("");
            $("#txtotherModel").val("");
            $("#txtotherSerial").val("");
            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/GetEquipmentDetails",
                contentType: "application/json;charset=utf-8",
                data: '{"visitorID":"' + ID + '"}',
                dataType: "json",
                success: function (data) {
                    var jsondata = JSON.parse(data.d);
                    for (var i = 0; i < jsondata.length; i++) {
                        if (jsondata[i].EquipmentType == "Laptop") {
                            $("#txtlaptopMake").val(jsondata[i].EquipmentMake);
                            $("#txtlaptopModel").val(jsondata[i].EquipmentModel);
                            $("#txtlaptopserial").val(jsondata[i].EquipmentSerial);
                        }
                        if (jsondata[i].EquipmentType == "Data Storage Device") {
                            $("#txtdatastoragedeviceMake").val(jsondata[i].EquipmentMake);
                            $("#txtdatastoragedeviceModel").val(jsondata[i].EquipmentModel);
                            $("#txtdatastoragedeviceSerial").val(jsondata[i].EquipmentSerial);
                        }
                        if (jsondata[i].EquipmentType == "USB Hard Disk") {
                            $("#txtUSBharddiskMake").val(jsondata[i].EquipmentMake);
                            $("#txtUSBharddiskModel").val(jsondata[i].EquipmentModel);
                            $("#txtUSBharddiskSerial").val(jsondata[i].EquipmentSerial);
                        }
                        if (jsondata[i].EquipmentType == "Camera") {
                            $("#txtcameraMake").val(jsondata[i].EquipmentMake);
                            $("#txtcameraModel").val(jsondata[i].EquipmentModel);
                            $("#txtcameraSerial").val(jsondata[i].EquipmentSerial);
                        }
                        if (jsondata[i].EquipmentType == "I Pod") {
                            $("#txtIPodMake").val(jsondata[i].EquipmentMake);
                            $("#txtIPodModel").val(jsondata[i].EquipmentModel);
                            $("#txtIPodSerial").val(jsondata[i].EquipmentSerial);
                        }
                        if (jsondata[i].EquipmentType == "Others") {
                            $("#txtotherMake").val(jsondata[i].EquipmentMake);
                            $("#txtotherModel").val(jsondata[i].EquipmentModel);
                            $("#txtotherSerial").val(jsondata[i].EquipmentSerial);
                        }
                    }
                    var span = document.getElementsByClassName("close")[0];
                    var modal = document.getElementById('myModal');
                    modal.style.display = "block";
                    $("#btnequipsubmit").attr("onclick", "SubmitEquipmentDetails(" + ID + ")");
                },
            });
            
        }


        $(".close").click(function () {
            
            SucessModel.style.display = "none";
            // BulkModel.style.display = "none";
            myModal.style.display = "none";
            CheckoutModel.style.display = "none";
            returnmodal.style.display = "none";
            lostmodal.style.display = "none";
            reissuemodal.style.display = "none";
            $(".btnAddCart").css("background-color", "#3188B5");
            $(".btnAddCart").removeAttr("disabled");
            $("#btnupdate").css("background-color", "grey");
            $("#btnupdate").attr("disabled", "disabled");
            $("#txtreturnedby").val("");
            $("#txtcheckassociatereturn").val("");
            $("#txtlostreturnedby").val("");
            $("#txtcheckedassociatelost").val("");
            $("#txtreissuerequestedby").val("");
            $("#txtcheckedassociatereissue").val("");
            $("#txtcheckedassociatelost").attr("style", "display:none");
            $("#txtcheckassociatereturn").attr("style", "display:none");
            $("#txtcheckedassociatereissue").attr("style", "display:none");
        });

        $(".closeequip").click(function () {
            SucessModel.style.display = "none";
            // BulkModel.style.display = "none";
            myModal.style.display = "none";
            visitdetails.style.display = "none";
            logdetails.style.display = "none";
            reissuemodal.style.display = "none";
        }
        )

        $("#updateok").click(function () {
            SucessModel.style.display = "none";
            
            if (tbodycart1.textContent == "Notification mail has been sent to the Host to collect the passes.") {
                window.location = "Clientvisit.aspx?requeststatus=To Be Processed";
            }
            else if ($("#btnreissue").val() != "Submit") {
                window.location.reload();
            }
        })

        function CheckVcardGetAccessCard(id) { 
        
            $("#fade").show();
            $("#modal").show();

            var chkArray = [];
            var vcardarray = [];
            $(".dataaccesscheckbox:checked").each(function () {
                chkArray.push($(this).val());
            });

            var account = 0;
            var checknull = 0;
            var checkzero = 0;
            var selected;
            selected = chkArray.join(',');
            var selectedspilt = selected.split(',');

            var parentid = $('#hdnParentID').val();
            var location = $('#hdnloginfacility').val().split('-')[1].toUpperCase();

            
            var vcardlocation = $("#tdtxtvcard" + id).val().substring(0, 3).toUpperCase();;
            if ((vcardlocation == "" || vcardlocation == null || vcardlocation == undefined)) { }
            else {
                if (location != vcardlocation) {
                    $("#tdtxtaccesscard" + id).val("");
                    $("#tdtxtvcard" + id).css('border-color', 'red');
                    $("#btnupdate").css("background-color", "grey");
                    $("#btnupdate").attr("disabled", "disabled");


                    $("#spnerror").text("*Error : Please assign a card which is tagged to your city.");
                    $("#modal").hide();
                    $("#fade").hide();
                    return false;
                }
                else {
                    $("#btnupdate").css("background-color", "#3188B5");
                    $("#btnupdate").removeAttr("disabled")
                }
            }
            
            var securityid = $('#hdnSecurityID').val();
            if (selectedspilt.length > 0) {

               
           
                for (var i = 0; i < selectedspilt.length; i++) {

                    var Name = $("#tdtxtvisitorName" + selectedspilt[i]).val();
                    var VcardNumber = $("#tdtxtvcard" + selectedspilt[i]).val();
                    var request = selectedspilt[i];
                    var visitorid = $("#tdvisitorid" + selectedspilt[i]).val();
                    vcardarray.push($("#tdtxtvcard" + selectedspilt[i]).val());

                    //$("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'black');
                }

                var vcardlist = vcardarray.join(',');
                var vcardsplit = vcardlist.split(',');

                ///var arr = [9, 9, 111, 2, 3, 4, 4, 5, 7];
                var sorted_arr = vcardarray.slice().sort(); // You can define the comparing function here. 
                // JS by default uses a crappy string compare.
                // (we use slice to clone the array so the
                // original array won't be modified)

                var duplicatecard = [];
                for (var i = 0; i < sorted_arr.length - 1; i++) {
                    if (sorted_arr[i + 1] == sorted_arr[i]) {
                        if (sorted_arr[i] != "") {
                            duplicatecard.push(sorted_arr[i]);
                        }
                    }
                }

                if (duplicatecard.length > 0) {
                    if (selectedspilt.length > 0) {
                        for (var i = 0; i < selectedspilt.length; i++) {
                            var duplicatecount = 0;
                            for (var j = 0; j < duplicatecard.length; j++) {
                                if (duplicatecard[j] == $("#tdtxtvcard" + selectedspilt[i]).val()) {
                                    $("#tdtxtvcard" + selectedspilt[i]).css('border-color', 'red');
                                    ////duplicatecount = duplicatecount + 1;
                                }
                            }
                        }
                        $("#spnerror").text("*Error : Duplicate Vcard number.");
                        $("#modal").hide();
                        $("#fade").hide();
                        return false;
                    }
                }
           
             var currentvcard = $("#tdtxtvcard" + id).val();

                //// ajax call to call the service to validate the v card and autopopulate the access card
             //// expected result : access card number against a vcard. if error : "Invalid Card will be returned
             GetCardDetails(currentvcard, id);
               
            }

        }

        function GetCardDetails(currentvcard, id)
        {
            var location = $('#hdnloginfacility').val().split('-')[1].toUpperCase();

            $.ajax({
                type: "POST",
                url: "ClientPage.aspx/GetAccessCardDetailsByVcard",
              //  async: false,
                contentType: "application/json;charset=utf-8",
                data: '{"vcard":"' + currentvcard + '","location":"' + location + '"}',
                dataType: "json",
                beforeSend: function () {
                    $("#overlay").show();
                },
                success: function (data) {

                    var result = JSON.parse(data.d);  //// expected result : access card number

                    if (result.length > 0) {

                        if (result == "Invalid Vcard" || result == "No Access Card Linked") {
                            $("#tdtxtaccesscard" + id).val("");
                            $("#tdtxtvcard" + id).css('border-color', 'red');
                            $("#spnerror").text("*Error : Kindly enter valid Client Vcard number(s)");
                            $("#btnupdate").css("background-color", "grey");
                            $("#btnupdate").attr("disabled", "disabled");
                            $("#btnreissue").css("background-color", "grey");
                            $("#btnreissue").attr("disabled", "disabled");

                            $("#modal").hide();
                            $("#fade").hide();
                            return false;
                        }
                        else if (result == "Location mismatch") {

                            $("#tdtxtaccesscard" + id).val("");
                            $("#tdtxtvcard" + id).css('border-color', 'red');
                            $("#spnerror").text("*Error : Please assign a card which is tagged to your city.");

                            $("#modal").hide();
                            $("#fade").hide();

                        }

                        else {


                            $("#btnupdate").css("background-color", "#3188B5");
                            $("#btnupdate").removeAttr("disabled");

                            $("#btnreissue").css("background-color", "#3188B5");
                            $("#btnreissue").removeAttr("disabled");


                            var Name = $("#tdtxtvisitorName" + id).val();
                            var VcardNumber = $("#tdtxtvcard" + id).val();
                            var Accessnumber = result;
                            $("#tdtxtaccesscard" + id).val("");
                            $("#tdtxtaccesscard" + id).val(Accessnumber);
                            var visitorid = $("#tdvisitorid" + id).val();

                            if (VcardNumber == "" || VcardNumber == null || VcardNumber == undefined) {
                                //// do nothing
                            }
                            else {
                                $("#tdtxtvcard" + id).removeClass("Recentvisitortableaftertextbox");
                                $("#tdtxtvcard" + id).addClass("Recentvisitortabletextbox");

                            }

                            $("#modal").hide();
                            $("#fade").hide();
                        }
                    }

                    else {
                    }
                },
                complete: function () {
                    $("#overlay").hide();
                },
                error: function (result) {  ////// need to test this scenario
                    ////debugger
                    $("#modal").hide();
                    $("#fade").hide();
                    alert("Error Occured");

                }
            });
        }

        function GetAccessCardNo(vcard)
        {
            var accessCard = '';
           $.ajax({
                type: "POST",
                url: "ClientPage.aspx/GetAccessCardNo",
                contentType: "application/json;charset=utf-8",
                data: '{"vcard":"' + vcard + '"}',
                dataType: "json", 
                async: false,
                success: function (data) {
                    if (data.d.length > 0) {
                        accessCard = data.d;                        
                    }
                }               
            });

           return accessCard;
        }

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 9 || charCode == 13) {
                return false;
            }
            return true;
        }

        function avoidSpecialCharacters(obj) {

            var ctrl = document.getElementById(obj.id);
            var temp = ctrl.value;
            temp = temp.replace(/[^a-zA-Z 0-9]+/g, '');
            ctrl.value = temp;

        };
      
        function initPicker(picker, selectedDates) {
            var isInit = false;
            var kendoPicker = picker.data('kendoDatePicker');

            kendoPicker.bind('open', function () {
                if (!isInit) {
                    //assuming that corresponding calendar widget has id 'picker_dateview'
                    var calendar = $('#' + picker.attr('id') + '_dateview > .k-calendar');

                    initCalendar(calendar, selectedDates, function () {
                        updatePicker(picker, selectedDates);
                    });

                    isInit = true;
                }
            });

            picker.on('input change blur', function () {
                updatePicker(picker, selectedDates);
            });

            updatePicker(picker, selectedDates);
        }

        function initCalendar(calendar, selectedDates, onUpdate) {
            var kendoCalendar = calendar.data('kendoCalendar');

            kendoCalendar.bind('navigate', function () {
                setTimeout(function () {
                    updateCalendar(calendar, selectedDates);
                }, 0);
            });

            updateCalendar(calendar, selectedDates);

            calendar.on('click', function (event) {
                var cell = $(event.target).closest('td');
                var isClickedOnDayCell = cell.length !== 0 && isDayCell(cell);

                if (isClickedOnDayCell) {
                    var date = dateFromCell(cell).getTime();
                    var isDateAlreadySelected = selectedDates.some(function (selected) {
                        return selected === date;
                    });

                    if (isDateAlreadySelected) {
                        selectedDates.splice(selectedDates.indexOf(date), 1);

                    } else {
                        selectedDates.push(date);
                    }

                    updateCell(cell, selectedDates);

                    if (onUpdate !== undefined) {
                        onUpdate();
                    }
                }
            });
        }

        function updatePicker(picker, selectedDates) {
            var datesString = selectedDates.sort().reduce(function (acc, selected, index) {
                var date = new Date(selected);
                var formattedDate = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();

                return acc + formattedDate + (index === (selectedDates.length - 1) ? '' : ', ');
            }, '');

            picker.val(datesString);
        }

        function updateCalendar(calendar, selectedDates) {
            calendar.find('td > a').parent().each(function (i, item) {
                var cell = $(item);

                if (isDayCell(cell)) {
                    updateCell(cell, selectedDates);
                }
            });
        }

        function updateCell(cell, selectedDates) {
            var isCellSelected = selectedDates.some(function (selected) {
                return selected === dateFromCell(cell).getTime();
            });

            if (isCellSelected) {
                cell.addClass('selected1');

            } else {
                cell.removeClass('selected1');
            }
        }

        function isDayCell(cell) {
            return /^\d{1,2}$/.test(cell.find('a').text());
        }

        function dateFromCell(cell) {
            return new Date(convertDataValue(cell.find('a').attr('data-value')));
        }

        //convert from 'YYYY/MM/DD', where MM = 0..11
        function convertDataValue(date) {       
            var regex = /\/(\d+)\//;
            var month = +date.match(regex)[1] + 1;
            return date.replace(regex, '/' + month + '/');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
