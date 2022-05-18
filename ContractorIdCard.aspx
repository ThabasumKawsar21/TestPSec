<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractorIdCard.aspx.cs"
    Inherits="VMSDev.ContractorIdCard" MasterPageFile="~/VMSMain.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
   
    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jQuery.stringify.js" type="text/javascript"></script>
    <script type="text/javascript" src="ContractorScreen/Scripts/grid.locale-en.js"></script>
    <script type="text/javascript" src="ContractorScreen/Scripts/jquery.jqGrid.min.js"></script>
    <script type="text/javascript" src="ContractorScreen/Scripts/ContractorSearchGridnew.js"></script>
    <link href="includes/vms.css" rel="stylesheet" type="text/css" />
    <link href="includes/base.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Printstyles.css" rel="stylesheet" type="text/css" />
    <link href="includes/base.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/stylesBulk.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Styles.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/stylesContract-1.css" rel="stylesheet" type="text/css" />
    <script src="App_Themes/jquery.rotate-1.0.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.widget.js" type="text/javascript"></script>
    <link href="App_Themes/jquery.ui.autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Jquery-ui.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
//    var colNames = ['ContractorId', 'firstname', 'ctsid', 'Vendor', 'VendorPOC'];
//var colModel = [{ name: 'ContractorId', index: 'ContractorId', sortable: true, hidden: false, resizable: true },
//                { name: 'firstname', index: 'firstname', sortable: true, hidden: false, resizable: true },
//                { name: 'ctsid', index: 'ctsid', sortable: true, hidden: false, resizable: true },
//                { name: 'Vendor', index: 'Vendor', sortable: true, hidden: false, resizable: true },
//                { name: 'VendorPOC', index: 'VendorPOC', sortable: true, hidden: true, resizable: true },               
//];

        $(document).ready(function () {
            $('#<%=lbtnSave.ClientID %>').hide();
            var RoleId = "<%= Session["RoleID"]%>";
            if(RoleId=="Security")
            {
                $('.lnkEdit').hide();
            }

            //    $(document).ready(function () {
            //        $("#Table1").jqGrid({
            //    datatype: 'json',
            //    url: "ContractorScreen/ContractorDetailsPopUpForm.aspx/SearchContractor",
            //    mtype: 'POST',
            //    postData: {},
            //    height: '310px',
            //    //width:'100%',
            //    //autowidth: true,
            //    //altRows: true,
            //    //viewsortcols: true,
            //    colNames: colNames,
            //    colModel: colModel,
            //    multiselect: false,
            //    sortorder: "desc",
            //    rowNum: 10,
            //    rowList: [10, 20, 30, 50, 100],       
            //    viewrecords: true,
            //    pgtext: "Page {0} of {1}",
            //    loadonce: false,
            //    onCellSelect: function (rowId, iCol, cellContent, e) {
            //    }
            //});


            $(function () {

                $(".tb").autocomplete({

                    source: function (request, response) {
                        $.ajax({
                            url: "ContractorIdCard.aspx/GetVendorName",
                            data: "{ 'sname': '" + request.term + "' }",
                            dataType: "json",
                            type: "POST",
                            max: response && !response.scroll ? 10 : 150,
                            contentType: "application/json; charset=utf-8",
                            dataFilter: function (data) { return data; },
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        value: $.trim(item),
                                        result: $.trim(item)
                                    }
                                }))
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert(textStatus);
                            }
                        });
                    },
                    minLength: 2
                });
            });

            function DeleteContractor() {

                var selected = false;
                $('#<%=grdIDCardList.ClientID %>').find("input:checkbox").
                each(function () {

                    if (this.checked) {
                        selected = true;

                    }
                });

                if (selected)
                {
                    return confirm('Are you sure to delete the contractor entries');
                }
                else {
                    alert('Please select a contractor entry');
                    return false;
                }
            }
        })
    </script>
    <script runat="server">

        protected void Button2_Click(object sender, EventArgs e)
        {
            Label1.Text = "CloseUpload";
        }
    </script>
    <script type="text/javascript">
        var arrId = [];
        function SelectAllCheckboxes(chk) {
            $('#<%=grdIDCardList.ClientID %>').find("input:checkbox").
            each(function () {
                if (this != chk) {
                    this.checked = chk.checked;
                }
            });
        }

        //        function avoidSpecialCharacters(evt) {
        //         var e = event || evt; // for trans-browser compatibility
        //            var charCode = e.which || e.keyCode;
        //            //  alert("id of control is:" + obj.id);
        //            var ctrl = document.getElementById(evt.id);
        //            var temp = ctrl.value;
        //            
        //            if (charCode != 37 && charCode != 38 && charCode != 39 && charCode != 40 && charCode != 46 &&
        //            charCode != 8 && charCode != 9 && charCode != 13 && charCode != 16 && charCode != 17 && charCode != 18 && charCode !=20)
        //             {
        //                temp = temp.replace(/[^a-zA-Z 0-9]+/g, '');
        //                ctrl.value = temp;
        //            }
        //            //alert("id of control is:" + temp);
        //        };

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



        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }

        //        function allowAlpha(ie, moz) {

        //            if (moz != null) {
        //                //alert(moz);
        //                if ((moz == 32) || (moz >= 65) && (moz < 91) || moz == 8 || moz == 13 || (moz >= 97) && (moz < 123)) {
        //                    return true;
        //                }
        //                else {
        //                    return false;
        //                }
        //            }
        //            else {

        //                if ((ie == 32) || (ie == 37) || (ie == 38) || (ie >= 65) && (ie < 91) || (ie >= 97) && (ie < 123)) {
        //                    return true;
        //                }
        //                else {
        //                    return false;
        //                }
        //            }

        //        }

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

                if ((ie == 32) || (ie == 37) || (ie == 38) || (ie >= 65) && (ie < 91) || (ie >= 97) && (ie < 123) || (ie == 46) || (ie >= 48) && (ie < 57) || (ie == 45)) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }



        function allowAlphaSpecialChar(ie, moz) {

            if (moz != null) {
                //                alert(moz);
                if ((moz == 32) || (moz >= 65) && (moz < 91) || moz == 8 || moz == 13 || (moz >= 97) && (moz < 123) || (moz >= 33) && (moz < 65)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {

                if ((ie == 32) || (ie == 37) || (ie == 38) || (ie >= 65) && (ie < 91) || (ie >= 97) && (ie < 123) || (ie >= 33) && (ie < 65)) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }

        function imgPrint_onclick() {

            var Id = $('#' + '<%= hdnContratorId.ClientID %>').val();
            var idImg = Id;
            //  var idImg = Id;
            window.open('ContractorPreview.aspx?key=' + Id,
            'List', 'scrollbars=yes,resizable=no,top=150; width=320px,height=580px;left=300,location=center').focus();
            var serviceBase = 'ContractorIdCard.aspx/';
            $.ajax({
                type: "POST",
                url: serviceBase + "SavePrintDeatils",
                data: "{ 'id': '" + Id + "' }",
                contentType: "application/json; charset=utf-8",
                cache: false,
                async: false,
                dataType: "json",
                success: function (response) {

                }
            });

            return false;
        }

        function myclick() {
            $find("ModalPopupExtender1").show();
        }

        function SetReset() {
            $('#<%=lbtnReset.ClientID%>').show();
            $('#<%=ExportLink.ClientID%>').show();
            $('#<%=lbtnPrint.ClientID%>').show();

            $('#divexport').show();
        }

        function SetResetEdit() {
            $('#<%=lbtnReset.ClientID%>').hide();
            $('#<%=lbtnPrint.ClientID%>').hide();
            $('#<%=lbtnAddNew.ClientID%>').hide();
            $('#<%=lbtnSave.ClientID%>').hide();

        }

        function SetsetEdit() {
            $('#<%=lbtnReset.ClientID%>').show();

            $('#<%=lbtnPrint.ClientID%>').show();
            $('#<%=lbtnAddNew.ClientID%>').show();
            $('#<%=lbtnSave.ClientID%>').show();

        }

        function ShowReset() {
            $('#<%=lbtnReset.ClientID%>').show();
        }
        function HideReset() {
            $('#<%=lbtnReset.ClientID%>').hide();
            ExportReset();
        }
        function ExportReset() {
            $('#<%=ExportLink.ClientID%>').hide();
            $('#<%=lbtnPrint.ClientID%>').hide();
            $('#divexport').hide();
        }
        function CloseVisitorStatus(obj) {
            var idImg = obj.id;
            $('.newpop-track-wrap').css('display', 'none');
            return false;
        }

        function ShowLastPrintStatus(obj) {

            var idImg = obj.id;
            var serviceBase = 'ContractorIdCard.aspx/';
            var Id = $("#" + idImg).parent().find('.lblgrdContractorID').text();
            $("#" + idImg).parent().find('.newpop-track-wrap').css('width', '240');
            $("#" + idImg).parent().find('.newpop-track-wrap').css('height', '180');
            $("#" + idImg).parent().find('.newpop-track-wrap').css("left", event.clientX - 150);
            $("#" + idImg).parent().find('.newpop-track-wrap').css("top", event.clientY + 5);

            $.ajax({
                type: "POST",
                url: serviceBase + "GetLastPrintStatus",
                data: "{ 'id': " + Id + " }",
                contentType: "application/json; charset=utf-8",
                cache: false,
                async: false,
                dataType: "json",
                success: function (response) {
                    var status = response.d;
                    if (status != null) {
                        $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').show();
                        if (status.IDGeneratedDate != null)
                            $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblIDGeneratedDate').text(status.IDGeneratedDate);

                        if (status.PrintedDate != null)
                            $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblLastPrintedDate').text(status.PrintedDate);
                        if (status.PrintStatus != null)
                            $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblPrintStatus').text(status.PrintStatus);
                        if (status.UpdatedBy != null)
                            $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblPrintedBy').text(status.UpdatedBy);
                        if (status.LocationName != null)
                            $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblLocation').text(status.LocationName);

                        // $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').show();
                        $("#" + idImg).parent().find('.newpop-track-wrap').css('display', 'block');
                    }
                    else {
                        $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').show();
                        $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblLastPrintedDate').text('');
                        $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblIDGeneratedDate').text('');
                        $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblPrintStatus').text('Not Printed');
                        $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblPrintedBy').text('');
                        $("#" + idImg).parent().find('.newpop-track-wrap').find('.printstatus').find('.lblLocation').text('');
                        $("#" + idImg).parent().find('.newpop-track-wrap').css('display', 'block');
                    }


                }
            });
            return false;
        }

        function ShowValidation_Grid(obj) {
            var distinctMsgs = '';
            var Id = obj.id;
            var contractorId = $("#" + Id).parent().parent().find('.ContractorIdEdit').val();
            var Idlength = contractorId.length;
            var contractorName = $("#" + Id).parent().parent().find('.ContractorNameEdit').val();
            var vendorName = $("#" + Id).parent().parent().find('.VendorNameEdit').val();
            var supervisiorPhone = $("#" + Id).parent().parent().find('.SuperVisiorPhoneEdit').val();
            var supervisiorPhoneLength = supervisiorPhone.length;
            var vendorPhone = $("#" + Id).parent().parent().find('.VendorPhoneNumberEdit').val();
            var vendorPhoneLength = vendorPhone.length;
            var DocStatus = $("#" + Id).parent().parent().find('.DocStatusEdit').val();
            var status = $("#" + Id).parent().parent().find('.drpStatusEdit').val();

            if (contractorId != '') {
                if ((isNaN(contractorId)) || (Idlength < 6)) {
                    distinctMsgs = 'Please enter valid 6 digit contractor number<br />';
                }

            }

            if (contractorName == '') {
                distinctMsgs = distinctMsgs + 'Please enter Contractor Name<br />';
            }

            if (contractorName.length < 2) {
                distinctMsgs = distinctMsgs + 'Contractor Name length must have atleast 2 characters<br />';
            }
            else {
                contractorName = contractorName.replace(/[^a-zA-Z ]/g, "")
                if (contractorName.length < 2) {
                    distinctMsgs = distinctMsgs + 'Contractor Name must have atleast 2 alphabets<br />';
                }
            }

            if (vendorName == '') {
                distinctMsgs = distinctMsgs + 'Please enter Vendor Name<br />';
            }

            if (vendorName.length < 2) {

                distinctMsgs = distinctMsgs + 'Vendor Name length must have atleast 2 characters<br />';
            }
            else {
                vendorName = vendorName.replace(/[^a-zA-Z ]/g, "")
                if (vendorName.length < 2) {
                    distinctMsgs = distinctMsgs + 'Vendor Name must have atleast 2 alphabets<br />';
                }
            }

            if ((supervisiorPhone == '') || (supervisiorPhoneLength < 1)) {
                distinctMsgs = distinctMsgs + 'Please enter valid Supervisor Phone Number<br />';
            }

            if ((vendorPhone == '') || (vendorPhoneLength < 1)) {
                distinctMsgs = distinctMsgs + 'Please enter valid Vendor Phone Number<br />';
            }

            document.getElementById('<%=Span2.ClientID %>').innerHTML = distinctMsgs;
            if (distinctMsgs == '') {
                return true;
            }
            else {
                $('#divError').show();
                return false;
            }


        }

        function ShowValidationErrorDialog() {
            var distinctMsgs = '';
            var contractorId = $('#<%=txtContractorId.ClientID%>').val();
            //        var length = $("input:id*=txtContractorId]").val().length;
            var Idlength = contractorId.length;
            var vendorName = $.trim($('#<%=txtVendorName.ClientID%>').val());
            var contractorName = $.trim($('#<%=txtContractorName.ClientID%>').val());
            var supervisiorPhone = $.trim($('#<%=txtSupervisiorPhone.ClientID%>').val());
            var supervisiorPhoneLength = supervisiorPhone.length;
            var vendorPhone = $.trim($('#<%=txtVendorPhone.ClientID%>').val());
            var vendorPhoneLength = vendorPhone.length;
            var DocStatus = $('#<%=drpDocStatus.ClientID%>').val();
            var status = $('#<%=drpActive.ClientID%>').val();
            if (contractorId != '') {
                if ((isNaN(contractorId)) || (Idlength < 6)) {
                    distinctMsgs = 'Please enter valid 6 digit contractor number<br />';
                }

            }

            if (contractorName == '') {
                distinctMsgs = distinctMsgs + 'Please enter Contractor Name<br />';
            }

            if (contractorName.length < 2) {
                distinctMsgs = distinctMsgs + 'Contractor Name length must have atleast 2 characters<br />';
            }
            else {
                contractorName = contractorName.replace(/[^a-zA-Z ]/g, "")
                if (contractorName.length < 2) {
                    distinctMsgs = distinctMsgs + 'Contractor Name must have atleast 2 alphabets<br />';
                }
            }

            if (vendorName == '') {
                distinctMsgs = distinctMsgs + 'Please enter Vendor Name<br />';
            }

            if (vendorName.length < 2) {

                distinctMsgs = distinctMsgs + 'Vendor Name length must have atleast 2 characters<br />';
            }
            else {
                vendorName = vendorName.replace(/[^a-zA-Z ]/g, "")
                if (vendorName.length < 2) {
                    distinctMsgs = distinctMsgs + 'Vendor Name must have atleast 2 alphabets<br />';
                }
            }

            if ((supervisiorPhone == '') || (supervisiorPhoneLength < 1)) {
                distinctMsgs = distinctMsgs + 'Please enter valid Supervisor Phone Number<br />';
            }

            if ((vendorPhone == '') || (vendorPhoneLength < 1)) {
                distinctMsgs = distinctMsgs + 'Please enter valid Vendor Phone Number<br />';
            }

            document.getElementById('<%=Span2.ClientID %>').innerHTML = distinctMsgs;
            if (distinctMsgs == '')
                return true;
            else
                return false;
        }

        function PrevClick() {
            var currentPage = $('#<%=CurrentPage.ClientID%>').val();
            var totalcount = $('#<%=TotalSize.ClientID%>').val();
            var pageSize = $('#<%=PageSize.ClientID%>').val();
            if ((parseInt(currentPage, 10) - 1) >= 0) {
                $('#<%=CurrentPage.ClientID%>').val(parseInt(currentPage, 10) - 1);
            }
            BuilderPages();
            currentPage = $('#<%=CurrentPage.ClientID%>').val();
            MakeServercall(arrId[parseInt(currentPage, 10) - 1]);
            return false;
        }
        function NextClick() {
            var currentPage = $('#<%=CurrentPage.ClientID%>').val();
            var totalcount = $('#<%=TotalSize.ClientID%>').val();
            var pageSize = $('#<%=PageSize.ClientID%>').val();
            if ((parseInt(currentPage, 10) * parseInt(pageSize, 10)) < parseInt(totalcount, 10)) {
                $('#<%=CurrentPage.ClientID%>').val(parseInt(currentPage, 10) + 1);
            }
            BuilderPages();
            currentpage = $('#<%=CurrentPage.ClientID%>').val();
            var nextItem = parseInt(currentpage, 10) - 1;
            MakeServercall(arrId[nextItem]);
            return false;
        }

        function BuilderPages() {

            var totalcount = $('#<%=TotalSize.ClientID%>').val();
            var pageSize = $('#<%=PageSize.ClientID%>').val();
            var currentPage = $('#<%=CurrentPage.ClientID%>').val();
            $('#<%=lblCurrentPage.ClientID%>').text(currentPage);
            $('#<%=lblTotalPage.ClientID%>').text(totalcount);

            if (totalcount >= pageSize) {

                if ((parseInt(currentPage, 10) - 1) <= 0) {
                    $('#<%=imgPrev.ClientID%>').hide();
                }
                else {
                    $('#<%=imgPrev.ClientID%>').show();
                }
                if ((parseInt(currentPage, 10) * parseInt(pageSize, 10)) >= parseInt(totalcount, 10)) {
                    $('#<%=imgNext.ClientID%>').hide();
                }
                else {
                    $('#<%=imgNext.ClientID%>').show();
                }
            }
            else {
                $('#<%=imgPrev.ClientID%>').hide();
                $('#<%=imgNext.ClientID%>').hide();
            }
        }

        function AddNew() {
                       $('.selected input').val('');
                       $('.AddNew').show();
                       $('#<%=lbtnAddNew.ClientID%>').hide();
                       $('#<%=lbtnSave.ClientID%>').show();

                       $('#<%=grdIDCardList.ClientID%>').hide();
                       $('#<%=lblDetails.ClientID%>').hide();
                        HideReset();
                       ShowReset();
            ////window.open("ContractorScreen/ContractorDetailsPopUpForm.aspx", "Sample", "directories=no,titlebar=no,toolbar=no,location=no,toolbar=0, menubar=0, location=0,resizable=0,status=0,top=20,left=20,height=650px,width=1070px,scrollbars=yes, maximize=1");
            return false;
        }

        function showpop() {
            var Id = "";
            var totalcount = 0;
            var i = 0;
            var chkboxrowcount = $("#<%=grdIDCardList.ClientID%> input[id*='chkIDCard']:checkbox:checked").length;
            if (chkboxrowcount == 0) {
                alert("Please select at least one record");
                return false;
            }
            else {

                $('#<%=grdIDCardList.ClientID%>').children().
                find("tr[class='odd_row'],tr[class='even_row']").each(function () {
                    if ($(this).find('.chkselect')[0].checked) {
                        // arrId[i] = $(this).children().find("$('#<%=TotalSize.ClientID%>')").val();

                        arrId[i] = $(this).children().find("input[id='lblContractorId']").val();
                        i++;
                        totalcount++;
                    }
                });
                $('#<%=CurrentPage.ClientID%>').val("1");
                $('#<%=TotalSize.ClientID%>').val(totalcount);
                MakeServercall(arrId[0]);
            }
            return false;
        }
        function hidepop() {
            $('.overlay,.newpop-wrap').fadeOut(500);
        }
        function MakeServercall(Id) {
            var serviceBase = 'ContractorIdCard.aspx/';
            $.ajax({
                type: "POST",
                url: serviceBase + "GetContractorDetails",
                data: "{ 'id': '" + Id + "' }",
                contentType: "application/json; charset=utf-8",
                cache: false,
                async: false,
                dataType: "json",
                success: function (response) {
                    var status = response.d;
                    $('.overlay,.newpop-wrap').fadeIn(500);
                    $('#' + '<%= lblContratcorName.ClientID %>').text(status[0].ContractorName);
                    $('#' + '<%= lblVendorName.ClientID %>').text(status[0].VendorName);
                    $('#' + '<%= lblVendorName1.ClientID %>').text(status[0].VendorName);
                    $('#' + '<%= lblSupervisiorCell.ClientID %>').text(status[0].SupervisiorMobile);
                    $('#' + '<%= lblServiceProviderCell.ClientID %>').text(status[0].VendorPhoneNumber);
                    $('#' + '<%= lblValidUpTo.ClientID %>').text(status[0].CardValidlity);
                    $('#' + '<%= lblFacilityAddress.ClientID %>').text(status[0].FacilityAddress);
                    $('#' + '<%= lblValid.ClientID %>').text(status[0].CardValid);
                    $('#' + '<%= lblCWRID.ClientID %>').text(status[0].contractorIdentityNo);
                    $('#' + '<%= hdnContratorId.ClientID %>').val(status[0].ContractorId);
                    $('#' + '<%= lblContratorId.ClientID %>').text(status[0].ContractorNumber);

                    BuilderPages();
                    $('#<%=lbtnAddNew.ClientID%>').show();
                    $('#<%=lbtnSave.ClientID%>').hide();
                }
            });
        }

        function HidePopupAlert() {
            $('#divError').hide();
            return false;
        }

        function BindGrid(Id) {
            $('#<%=hdnContractorSearchId.ClientID %>').val(Id);
            // document.getElementById('<%=hdnContractorSearchId.ClientID %>').val(Id);
            var r = document.getElementById('<%=btnSearachbyId.ClientID %>');
            r.click();


        }

        function SaveDetails() {            

            if (ShowValidationErrorDialog()) {                
                var serviceBase = 'ContractorIdCard.aspx/';
                var contractorId = $('#<%=txtContractorId.ClientID%>').val();
                var vendorName = $.trim($('#<%=txtVendorName.ClientID%>').val());
                var contractorName = $.trim($('#<%=txtContractorName.ClientID%>').val());
                var supervisiorPhone = $('#<%=txtSupervisiorPhone.ClientID%>').val();
                var vendorPhone = $('#<%=txtVendorPhone.ClientID%>').val();

                var DocStatus = $('#<%=drpDocStatus.ClientID%>').val();
                var status = $('#<%=drpActive.ClientID%>').val();
                $.ajax({
                    type: "POST",
                    url: serviceBase + "SaveRequest",
                    data: "{ 'contractorId': '" + contractorId + "','contractorName': '" + contractorName + "', 'vendorName': '" + vendorName + "','docStatus': '" + DocStatus + "','status': '" + status + "', 'supervisiorPhone':'" + supervisiorPhone + "','vendorPhone':'" + vendorPhone + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (response) {
                        var Id = response.d;
                        if (Id != 0) {
                            alert("Successfully submitted");
                            $('.selected input').val('');
                            BindGrid(Id);
                        }
                        else {
                            alert("Contractor Id already exist");
                        }
                    }
                });
            }
            else {
                $('#divError').show();
            }
            return false;

        }

        $("#btnBulkUpload").click(function () {

            $find("ModalPopupExtender1").show();
        });



    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('div.rotation').jqrotate(-90);
        });
    

    </script>
    <style type="text/css">
        @media print
        {
            .content_contract
            {
                display: none;
            }
            .content .align-center, .A4
            {
                display: none;
            }
            .btns, .nav-left, .nav-right, .closebtn, .divid, .cont-btn, .strip img, .assc-photo img, .printcaption
            {
                display: none;
            }
            .id-card, .id-card
            {
                display: block !important;
            }
            .nav-cont table.tb1-main, .nav-cont table td.border-bot
            {
                vertical-align: middle !important;
                text-align: center;
                border: none !important;
            }
        }
    </style>
    <link href="App_Themes/cust-selectbox.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/stylesContract.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 646px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="VMSContentPlaceHolder" runat="server">
    <asp:ScriptManager ID="DefaultMasterScriptManager" EnablePageMethods="true" EnablePartialRendering="true"
        runat="server">
    </asp:ScriptManager>
    <div class="noPrint">
        <div class="content_contract">
            <div class="search_wrap">
                <div class="center flt-left">
                    <table>
                        <tr>
                            <td align="right" valign="top">
                                <label>
                                    Search by Keyword</label>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtSearch" CssClass="serachtxt" runat="server" onkeyup="avoidSpecialCharacters(this)">
                                </asp:TextBox>
                                <ajax:TextBoxWatermarkExtender ID="txtSearchValue" runat="server" TargetControlID="txtSearch"
                                    WatermarkText="Search by Contractor ID/Name/VendorName">
                                </ajax:TextBoxWatermarkExtender>
                                <%--<asp:Button ID="btnnewsearch" CssClass="searchbtn custom-btn" runat="server" />--%>
                                <asp:Button ID="btnSearch" CssClass="searchbtn custom-btn" runat="server" OnClick="BtnSearch_Click" />
                                <asp:Label ID="lblError" runat="server" CssClass="errorLabel"></asp:Label>
                            </td>
                            <td>
                                <a id="aBulkUpload" runat="server">
                                    <asp:ImageButton ID="imgBulkUpload" CssClass="img-text" runat="server" ImageUrl="Images/ContractorID/uploadt.png" />
                                    <%--<a href="#" class="custom-link-btn">Bulk upload<br />--%>
                                    <asp:LinkButton ID="btnBulkUpload" runat="server" class="custom-link-btn" Text="Bulk Upload" /></a>
                                <asp:HiddenField ID="hdnContractorSearchId" runat="server" />
                                <asp:Button ID="btnSearachbyId" runat="server" Text="Button" Style="display: none"
                                    OnClick="BtnSearachbyId_Click" />
                                <%-- <img src="Images/ContractorID/uploadt.png"  class="img-text" />--%>
                                <%--</a>--%>
                                <div>
                                    <ajax:ModalPopupExtender BackgroundCssClass="modalBg" ID="ModalPopupExtender1" runat="server"
                                        PopupControlID="PnlBulkUpload1" Drag="true" TargetControlID="btnBulkUpload">
                                    </ajax:ModalPopupExtender>
                                    <asp:Panel ID="PnlBulkUpload1" runat="server" CssClass="pnlBulkSearch" Style="display: none;
                                        width: 915px;" >                                   
                                        <iframe id="iframe1" allowtransparency="true" scrolling="no" class="iframe" style="height: 460px;
                                            width: 915px; background: #f8fdff;" src="BulkUpload.aspx" frameborder="0" marginheight="0"
                                            marginwidth="0" border="0" runat="server"></iframe>
                                        
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divexport" style="overflow: hidden; visibility: hidden margin-top:5px;">
                <table align="left">
                    <tr>
                        <td align="left" style="width: 500px;">
                            <span class="title flt-left" id="spanContract" runat="server">
                                <asp:Label ID="lblDetails" runat="server" CssClass="title flt-left"></asp:Label>
                            </span>
                        </td>
                        <td align="right" style="width: 500px;">
                            <a>
                                <asp:LinkButton ID="ExportLink" runat="server" class="custom-link-btn flt-right excel"
                                    Text="Export Excel" OnClick="ExportLink_Click" />
                            </a>
                        </td>
                        <td>
                            <img id="imgexportexcel" src="Images/export_exel.png" alt="Export" />
                        </td>
                    </tr>
                </table>
            </div>
            <table id="Table1">
            </table>
            <div id="pager">
            </div>
            <div class="tbl-wrap">
                <asp:GridView ID="grdIDCardList" Width="100%" runat="server" CellPadding="4" ForeColor="#333333"
                    AutoGenerateColumns="False" Font-Names="Verdana" Font-Size="X-Small" DataKeyNames="ContractorId"
                    CssClass="gridStyle" HeaderStyle-Wrap="True" PageSize="10" AllowPaging="True"
                    OnRowDataBound="GrdIDCardList_RowDataBound" OnPageIndexChanging="GrdIDCardList_PageIndexChanging"
                    GridLines="Vertical" AllowSorting="true" OnSelectedIndexChanged="GrdIDCardList_SelectedIndexChanged"
                    OnRowEditing="GrdIDCardList_RowEditing" OnRowCancelingEdit="GrdIDCardList_RowCancelingEdit"
                    OnRowUpdating="GrdIDCardList_RowUpdating">
                    <EmptyDataRowStyle CssClass="EmptyRecord" />
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);">
                                </asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" runat="server" class="chkselect" id="chkIDCard" />
                                <%--   <asp:CheckBox ID="chkIDCard" CssClass="chkselect" runat="server" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contractor Id" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75px">
                            <ItemTemplate>
                                <input type="hidden" name="lblContractorId" id="lblContractorId" value="<%#Eval("ContractorId")%>" />
                                <label id="lblContractorNumber">
                                    <%#Eval("ContractorNumber")%></label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContractorId" Maxlength="15" CssClass="ContractorIdEdit" onpaste="return false"
                                     runat="server" Text='<%# Eval("ContractorNumber")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contractor Name" ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lblAssociateDisplayName" runat="server" Text='<%# Eval("Name").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContractorName" class="ContractorNameEdit" MaxLength="20" runat="server"
                                    onpaste="return false" onKeyPress="return allowAlpha(event.keyCode, event.which);"
                                    Text='<%# Eval("Name")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VendorName" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblVendorName" runat="server" Text='<%# Eval("VendorName").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtVendorName" CssClass="VendorNameEdit  tb" MaxLength="25" onpaste="return false"
                                    runat="server" onKeyPress="return allowAlphaSpecialChar(event.keyCode, event.which);"
                                    Text='<%# Eval("VendorName")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DOCStatus" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90px">
                            <ItemTemplate>
                                <asp:Label ID="lblDOCStatus" runat="server" Text='<%# Eval("DOCStatus").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="drpDocStatus" CssClass="DocStatusEdit" runat="server">
                                    <asp:ListItem Text="Completed" Selected="True" runat="server"></asp:ListItem>
                                    <asp:ListItem Text="Incomplete" runat="server"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="drpStatus" CssClass="drpStatusEdit" runat="server">
                                    <asp:ListItem Text="Active" Selected="True" runat="server"></asp:ListItem>
                                    <asp:ListItem Text="InActive" runat="server"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supervisor Phone No." ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblSuperVisiorPhone" runat="server" Text='<%# Eval("SuperVisiorPhone").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSuperVisiorPhone" CssClass="SuperVisiorPhoneEdit" onpaste="return false"
                                    runat="server" MaxLength="15" onkeypress="return onlyNumbers()" Text='<%# Eval("SuperVisiorPhone")%>'
                                    Style="width: 100px;"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor Phone No." ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblVendorPhoneNumber" runat="server" Text='<%# Eval("VendorPhoneNumber").ToString() %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtVendorPhoneNumber" CssClass="VendorPhoneNumberEdit" onpaste="return false"
                                    onkeypress="return onlyNumbers()" MaxLength="15" runat="server" Style="width: 100px;"
                                    Text='<%# Eval("VendorPhoneNumber")%>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- added by bincey for status--%>
                        <asp:TemplateField HeaderText="Last Print Date " ItemStyle-HorizontalAlign="Left"
                            ItemStyle-CssClass="tab_itemStyle" ItemStyle-Width="12%">
                            <ItemTemplate>
                                <asp:Label ID="lblLastPrintDate" Text='<%# Eval("LastPrintedDate").ToString() %>'
                                    runat="server" />
                                <div class="newpop-track-wrap" style="position: absolute; opacity: .9; filter: alpha(opacity=80);
                                    z-index: 10; border: solid 1px black; display: none; background: #6aa7c9">
                                    <%-- <div class="top">
                                    </div>--%>
                                    <div class="content_track" style="width: 100%;">
                                        <div>
                                            <asp:ImageButton ID="imgVisitorClose" ToolTip="Close" AlternateText="Close" runat="server"
                                                ImageAlign="Right" ImageUrl="~/Images/ContractorID/closepng.png" OnClientClick="return CloseVisitorStatus(this);"
                                                CssClass="flt-right" />
                                        </div>
                                        <div class="align-center">
                                            <h3 class="vms-head" style="color: #fff;">
                                                Print Status</h3>
                                        </div>
                                        <div class="tbl-wrap">
                                            <div class="top">
                                            </div>
                                            <div class="content" style="width: 100%; padding-top: 10px">
                                                <div id="printstatus" class="printstatus">
                                                    <table style="width: 100%;" border="0">
                                                        <%--<tr>
                                                            <td style="text-align: left;" class="normalfont"   width="4px">
                                                                ID Generated Date :
                                                            </td>
                                                            <td width="2px">
                                                                :
                                                            </td>
                                                            <td width="4px">
                                                                <span class="lblIDGeneratedDate normalfont" id="lblIDGeneratedDate"></span>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td style="text-align: left;" class="normalfont" width="4px">
                                                                Last Printed Date :
                                                            </td>
                                                            <td width="2px">
                                                                :
                                                            </td>
                                                            <td width="4px">
                                                                <span class="lblLastPrintedDate normalfont" id="lblLastPrintedDate"></span>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td style="text-align: left;" class="normalfont"  width="4px">
                                                                Print Status
                                                            </td>
                                                            <td width="2px">
                                                                :
                                                            </td>
                                                            <td >
                                                                <span class="lblPrintStatus normalfont" id="lblPrintStatus"></span>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td style="text-align: left;" class="normalfont" width="4px">
                                                                Printed By
                                                            </td>
                                                            <td width="2px">
                                                                :
                                                            </td>
                                                            <td>
                                                                <span class="lblPrintedBy normalfont" id="lblPrintedBy"></span>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td style="text-align: left;" class="normalfont" width="4px">
                                                                Location
                                                            </td>
                                                            <td width="4px">
                                                                :
                                                            </td>
                                                            <td >
                                                                <span class="lblLocation normalfont" id="lblLocation"></span>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="bottom">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:ImageButton ID="ImgGetLastPrintedDate" ImageUrl="Images/ContractorId/expand.png"
                                    runat="server" OnClientClick="return ShowLastPrintStatus(this)" Style="width: 15px;
                                    height: 15px;" />
                                <asp:Label ID="lblgrdContractorID" CssClass="lblgrdContractorID" runat="server" Text='<%#Eval("ContractorId") %>'
                                    Style="display: none"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="lnkEdit" HeaderStyle-CssClass="lnkEdit" HeaderText="Action"
                            HeaderStyle-Width="130px">
                            <ItemTemplate>
                                <asp:ImageButton ID="lnkEdit" CssClass="lnkEdit" runat="server" ToolTip="Edit" CausesValidation="False"
                                    CommandName="Edit" src="images/edit_pen.png" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnupdate" runat="server" OnClientClick="return ShowValidation_Grid(this);"
                                    CommandName="Update" Text="Update"></asp:LinkButton>
                                <asp:LinkButton ID="btncancel" runat="server" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--ends edit option here--%>
                        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-CssClass="visit_head_col" Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnVisitDetailsID" runat="server" Value='<%#Eval("ContractorId") %>'>
                                </asp:HiddenField>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <%-- ends here--%>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle CssClass="even_row" BackColor="#EFF3FB" BorderColor="Gray" />
                    <AlternatingRowStyle CssClass="odd_row" BackColor="#CDE3F1" ForeColor="black" BorderColor="Gray" />
                    <HeaderStyle Wrap="False" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Prev" />
                </asp:GridView>
                <div class="AddNew" style="display: none">
                    <span class="title flt-left" id="span1" runat="server">
                        <asp:Label ID="Label1" runat="server" Text="New Contractor Details" CssClass="title flt-left">
                        </asp:Label>
                    </span>
                    <table width="100%" border="0" cellpadding="5">
                        <tr>
                            <th scope="col">
                                Contractor ID
                            </th>
                            <th scope="col">
                                Contractor Name
                            </th>
                            <th scope="col">
                                Vendor Name
                            </th>
                            <th scope="col">
                                DOC Status
                            </th>
                            <th scope="col">
                                Status
                            </th>
                            <th scope="col">
                                Supervisor Phone
                            </th>
                            <th scope="col">
                                Vendor Phone
                            </th>
                        </tr>
                        <tr class="selected">
                            <td>
                                <input type="text"  id="txtContractorId" runat="server" onpaste="return false"
                                    onkeyup="avoidSpecialCharacters(this)" maxlength="15" onkeypress="return onlyNumbers()" />
                            </td>
                            <td>
                                <input id="txtContractorName" maxlength="20" type="text" runat="server" onpaste="return false"
                                    onkeyup="avoidSpecialCharacters(this)" onkeypress="return allowAlpha(event.keyCode, event.which);" />
                            </td>
                            <td>
                                <input id="txtVendorName" class="tb" type="text" maxlength="25" runat="server" onpaste="return false"
                                    onkeypress="return allowAlphaSpecialChar(event.keyCode, event.which);" />
                                <%-- onkeyup="avoidSpecialCharacters(this)"--%>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDocStatus" runat="server">
                                    <asp:ListItem Text="Completed" Selected="True">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Incomplete">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpActive" runat="server">
                                    <asp:ListItem Text="Active" Selected="True">
                                    </asp:ListItem>
                                    <asp:ListItem Text="InActive">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupervisiorPhone" MaxLength="15" runat="server" onpaste="return false"
                                    onkeypress="return onlyNumbers()">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVendorPhone" MaxLength="15" runat="server" onpaste="return false"
                                    onkeypress="return onlyNumbers()">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divError" class="popwrap" style="display: none; height: 280px; top: 270px;
                    left: 360px;">
                    <div class="pop-header">
                        <span style="float: left">Error</span>
                        <div style="float: right">
                            <input type="image" src="Images/ContractorID/closepng.png" onclick="return HidePopupAlert();" /></div>
                    </div>
                    <div class="pop-content clearfix">
                        <div style="width: 350px; padding-left: 0px">
                            <span id="Span2" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px"
                                runat="server"></span>
                        </div>
                    </div>
                    <div class="pop-bot">
                    </div>
                </div>
                <input type="hidden" id="PageSize" name="PageSize" value="1" runat="server" />
                <input type="hidden" id="CurrentPage" name="CurrentPage" value="1" runat="server" />
                <input type="hidden" id="TotalSize" name="TotalSize" runat="server" />
            </div>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="lbtnAddNew" OnClientClick="return AddNew()" CssClass="cont-btn"
                            runat="server">
                            <span>Add New Contractor</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnSave" OnClientClick="return SaveDetails()" CssClass="cont-btn submitvisible"
                            runat="server">
                            <span>Submit</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnReset" CssClass="cont-btn" runat="server" OnClick="LbtnReset_Click">
                            <span>Reset</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" CssClass="cont-btn" runat="server" OnClick="LbtnDelete_Click"
                            OnClientClick="return DeleteContractor();">
                            <span>Delete</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnPrint" OnClientClick="return showpop()" CssClass="cont-btn printvisiblehide"
                            runat="server">
                            <span class="printcaption" id="spnPrint" runat="server">Print</span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="overlay">
    </div>
    <div class="newpop-wrap">
        <div class="top">
        </div>
        <div class="content" style="width: 100%;">
            <div>
                <a href="#" class="flt-right closebtn" onclick="hidepop();">
                    <img src="Images/ContractorID/closepng.png" /></a></div>
            <div class="align-center">
                <h3 class="vms-head">
                    Contractor ID Card - Admin</h3>
            </div>
            <div class="align-center">
                <h3 class="select-text">
                    Print preview screen</h3>
            </div>
            <div class="align-center">
            </div>
            <div class="tbl-wrap">
                <div class="content">
                    <div class="nav-left">
                        <a href="#">
                            <asp:ImageButton ID="imgPrev" ImageUrl="~/Images/ContractorID/leftnav.png" runat="server"
                                OnClientClick="return PrevClick()" ToolTip="Prev" /></a>
                    </div>
                    <div class="nav-cont">
                        <table width="100%" height="100%" class="tb1-main">
                            <tr>
                                <td class="border-bot" valign="middle">
                                    <div class="id-card">
                                        <div style="height: 34px;" class="strip">
                                            <img src="Images/ContractorID/idcardhead.png" /></div>
                                        <div class="front">
                                            <div class="uniq-id rotation">
                                                <asp:Label ID="lblContratorId" runat="server" Style="font-size: 12pt;"></asp:Label></div>
                                            <div class="famil-arial font-bold size-18pt">
                                                <div style="width: 110px; float: left; margin-left: 20px;">
                                                    <asp:Label runat="server" ID="lblCWRID" Style="color: red; padding-left: 2px; font-size: 9pt;
                                                        font-family: Times New Roman; color: #fe0000;"></asp:Label>
                                                </div>
                                                <div style="float: left; width: 100px;">
                                                    <asp:Label ID="lblValid" runat="server" Style="color: #000; margin-right: 30px; font-size: 14pt;"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="assc-photo" style="height: 155px;">
                                                <img src="Images/ContractorID/sampleasscimg.png" />
                                            </div>
                                            <div>
                                                <div class="famil-arial-nar font-bold size-10pt">
                                                    <asp:Label ID="lblContratcorName" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdnContratorId" runat="server" />
                                                    <div class="famil-arial-nar font-bold size-8pt">
                                                        <asp:Label ID="lblVendorName" runat="server"></asp:Label></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="height: 31px;" class="strip">
                                            <img src="Images/ContractorID/idcardhead.png" /></div>
                                    </div>
                                </td>
                                <td class="border-bot">
                                    <img src="Images/ContractorID/divider.png" class="flt-left mar-5 divid" />
                                    <div class="id-card" style="padding: 0 5px;">
                                        <div class="famil-arial-nar size-10pt mar-5">
                                        </div>
                                        <div class="famil-arial-nar size-13pt font-bold">
                                            <asp:Label ID="lblVendorName1" runat="server"></asp:Label></div>
                                        <div class=" mar-5">
                                            <span class="size-8.5pt famil-arial-nar">Supervisor's Cell # </span><span class="size-8.5pt famil-arial-nar font-bold">
                                                <asp:Label CssClass="bold-14" ID="lblSupervisiorCell" runat="server"></asp:Label></span></div>
                                        <div>
                                            <span class="size-8.5pt famil-arial-nar">Service Provider #</span> <span class="size-8.5pt famil-arial-nar font-bold">
                                                <asp:Label ID="lblServiceProviderCell" runat="server"></asp:Label></span></div>
                                        <div class=" mar-3 tele " style="white-space: nowrap">
                                            <span class="famil-arial-nar size-10pt mar-3">If found,Please inform :&nbsp; </span>
                                            <span class="famil-arial-nar font-bold size-9pt">1800 258 2345</span></div>
                                        <div class="font-bold famil-arial-nar size-10pt mar-3">
                                            <span class=" famil-arial-nar size-10pt mar-3">Valid upto </span>&nbsp;
                                            <asp:Label ID="lblValidUpTo" runat="server"></asp:Label></div>
                                        <div class=" famil-arial-nar size-10pt mar-3">
                                            Entry Permit for:</div>
                                        <div class=" famil-arial-nar size-10pt font-bold mar-3">
                                            Cognizant Technology Solutions<br />
                                            India Pvt. Ltd.</div>
                                        <div class=" famil-arial-nar size-8pt">
                                            <asp:Label ID="lblFacilityAddress" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right" class="border-bot bot ">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="nav-left" style="float: right">
                        <a href="#">
                            <asp:ImageButton ID="imgNext" ImageUrl="~/Images/ContractorID/rightnav.png" runat="server"
                                OnClientClick="return NextClick()" ToolTip="Prev" /></a>
                    </div>
                </div>
            </div>
            <%--  commenting ends here--%>
            <div class="align-center">
                <h3 class="req-id">
                    CARD PRINT SHEET : <span id="lblCurrentPage" runat="server"></span>/ <span id="lblTotalPage"
                        runat="server"></span>
                </h3>
            </div>
            <div class="align-center">
                <h3 class="select-text">
                </h3>
            </div>
            <table width="100%" border="0" cellspacing="0">
                <tr>
                    <td align="right" class="printcaption">
                        <asp:LinkButton ID="lbtnPrintCard" OnClientClick="return imgPrint_onclick()" CssClass="cont-btn"
                            runat="server">
                            <span class="printcaption">Print</span>
                        </asp:LinkButton>
                    </td>
                    <td align="left">
                        <%--  <a href="#" class="cont-btn"><span>Cancel</span></a>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="bottom">
        </div>
    </div>
    <div>
    </div>
    <div>
    </div>
    <div style="visibility: hidden">
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" UseSubmitBehavior="false" /></div>
    <div id="grid" class="bottomArea">
        <div id="pegGrid">
        </div>
    </div>
    <table id="example" class="display" width="100%">
    </table>
</asp:Content>
