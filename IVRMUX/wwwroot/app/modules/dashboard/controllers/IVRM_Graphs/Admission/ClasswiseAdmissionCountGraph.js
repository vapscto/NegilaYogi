(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClasswiseAdmissionCountGraphController', ClasswiseAdmissionCountGraphController)
    ClasswiseAdmissionCountGraphController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function ClasswiseAdmissionCountGraphController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.route = false;
        $scope.print = false;


        $scope.search = "";

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var copty;
        $scope.coptyright = copty;
      


        //  $scope.imgname = logopath;
        $scope.loaddata = function () {
       
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;

            apiService.create("studentbirthday/getalladmdetails", pageid).
                then(function (promise) {

                    $scope.adcyear = promise.accyear;
                    $scope.classlist = promise.classlist


                })
        }


        $scope.al_checkclass = function (all, ASMCL_Id) {



            $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.classlist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.classlist, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });


            

        }

        $scope.getclass = function (ASMCL_Id) {

          
            $scope.classlistarray = [];

            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }

            });

         
       
            //
        }
        $scope.isOptionsRequiredclass = function () {
            return !$scope.classlist.some(function (item) {
                return item.selected;
            });
        };

        $scope.onselectyear = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }


            apiService.create("CategoryWiseFeeCollection/getheadisbygrpid", data).
                then(function (promise) {
                    $scope.headlist = promise.fillfeehead;

                    //  $scope.arrlstinst1 = promise.fillinstallment;

                })
        }













        $scope.submitted = false;
        $scope.ShowReport = function () {

            if ($scope.myForm.$valid) {




                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "classlsttwo": $scope.classlistarray

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("studentbirthday/admcntdata", data).
                    then(function (promise) {
                        $scope.yearmodel = true;

                        $scope.studetails = promise.studentDetails;
                        
                        $scope.stutabledetails = promise.schooltabledetails;
                        
                        $scope.class_name = [];
                        $scope.class= [];
                        if ($scope.studetails != null) {
                            for (var i = 0; i < $scope.studetails .length; i++) {
                                $scope.class_name.push($scope.studetails [i].ASMCL_ClassName);
                            }
                            for (var i = 0; i < 1; i++) {
                                $scope.asmay_year=$scope.studetails[i].ASMAY_Year;
                            }

                            $scope.stdcount = [];
                            for (var i = 0; i < $scope.studetails .length; i++) {
                                $scope.stdcount.push($scope.studetails[i].Studentcount);
                            }

                        }

                        if ($scope.stutabledetails != null) {
                            for (var i = 0; i < $scope.stutabledetails.length; i++) {
                                $scope.class.push($scope.stutabledetails[i].ASMCL_ClassName);
                            }

                           

                        }

                      

                      //  function createChart1() {
                            $("#chart1").kendoChart({
                                title: {
                                    text: ""
                                },
                                legend: {
                                    position: "bottom"
                                },
                                chartArea: {
                                    background: ""
                                },
                                seriesDefaults: {
                                    type: "line",
                                    style: "smooth"
                                },
                                series: [
                                {
                                    name: "Count",
                                        data: $scope.stdcount,
                                }],
                                valueAxis: {
                                    labels: {
                                        format: "{0}%"
                                    },
                                    line: {
                                        visible: false
                                    },
                                    axisCrossingValue: -10
                                },
                                categoryAxis: {
                                    categories: $scope.class_name,
                                    majorGridLines: {
                                        visible: false
                                    },
                                    labels: {
                                        rotation: "auto"
                                    }
                                },
                                tooltip: {
                                    visible: true,
                                    format: "{0}%",
                                    template: "#= series.name #: #= value #"
                                }
                            });
                    //    }

                        //$(document).ready(createChart1);
                        //$(document).bind("kendo:skinChange", createChart1);


                        $("#chart2").kendoChart({
                            title: {
                                text: ""
                            },
                            legend: {
                                position: "top"
                            },
                            seriesDefaults: {
                                type: "column"
                            },
                            series: [{
                                name: "count",
                                data: $scope.stdcount,
                            }],
                            valueAxis: {
                                labels: {
                                    format: "{0}%"
                                },
                                line: {
                                    visible: false
                                },
                                axisCrossingValue: 0
                            },
                            categoryAxis: {
                                categories: $scope.class_name,
                                line: {
                                    visible: false
                                },
                                labels: {
                                    padding: { top: 35 }
                                }
                            },
                            tooltip: {
                                visible: true,
                                format: "{0}%",
                                template: "#= series.name #: #= value #"
                            }
                        });




                    })
            }
            else {
                $scope.submitted = true;

            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
            //|| field.$dirty;
        };



        $scope.printdatatable = [];
        $scope.exportToExcel = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var tableid = '';
                if ($scope.allorindiorcon == "all") {
                    if ($scope.result == 'FHW') {
                        tableid = '#table2';
                    }
                    else if ($scope.result == 'FSW') {
                        tableid = '#table1';
                    }
                    else if ($scope.result == 'FCW') {
                        tableid = '#table3';
                    }
                    else if ($scope.result == 'FRW') {
                        tableid = '#table4';
                    }

                }
                else {
                    if ($scope.report == 'WO') {
                        tableid = '#table5';
                    }
                    else if ($scope.report == 'AA') {
                        tableid = '#table6';
                    }
                    else {
                        tableid = '#table7';
                    }
                }

                var exportHref = Excel.tableToExcel(tableid, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
            //  $state.reload();

        }


        $scope.printData = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = "";
                if ($scope.allorindiorcon == "all") {
                    if ($scope.result == 'FHW') {
                        innerContents = document.getElementById("printSectionIdhad").innerHTML;
                    }
                    else if ($scope.result == 'FSW') {
                        innerContents = document.getElementById("printSectionIdstd1").innerHTML;
                    }

                    else if ($scope.result == 'FCW') {
                        innerContents = document.getElementById("printSectionIdcls").innerHTML;
                    }
                    else if ($scope.result == 'FRW') {
                        innerContents = document.getElementById("printSectionIdgrp").innerHTML;
                    }

                }
                else {
                    if ($scope.report == 'WO') {
                        innerContents = document.getElementById("printwaive").innerHTML;
                    }
                    else if ($scope.report == 'AA') {
                        innerContents = document.getElementById("printadjust").innerHTML;
                    }
                    else {
                        innerContents = document.getElementById("printOB").innerHTML;
                    }
                }
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
                // $state.reload();
            }
            else {
                swal("Please Select Records to be Printed");
            }
            //  $state.reload();
        }




        $scope.Clearid = function () {

            $scope.yearmodel = false;
        }


        {

            if ($scope.bulkdownload == true) {
                if ($scope.myForm.$valid) {

                    var FMT_Ids = [];
                    $scope.albumNameArraycolumn3 = [];
                    angular.forEach($scope.groupcount, function (groupcount) {
                        if (!!groupcount.selected) $scope.albumNameArraycolumn3.push({
                            columnID3: groupcount.fmT_Id

                        });
                    })

                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                        }
                    })


                    var data = {
                        //"active": $scope.active,
                        //"deactive": $scope.deactive,
                        //"left": $scope.left,
                        "ASMAY_Id": $scope.asmaY_Id,
                        //"ASMCL_Id": $scope.asmcL_Id,
                        //"FMH_Id": $scope.fmh_id,
                        //"AMSC_Id": $scope.asmC_Id,
                        //"TRMR_Id": $scope.trmR_Id,
                        //"radioval": $scope.result,
                        //"reporttype": $scope.allorindiorcon,
                        //"type": $scope.report,
                        //FMT_Ids: FMT_Ids,

                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("studentbirthday/radiobtndata", data).
                        then(function (promise) {


                            if (promise.reporttype == "all") {
                                if (promise.radioval == "FRW") {
                                    if (promise.groupalldata != null && promise.groupalldata != "") {
                                        $scope.grid_view = false;
                                        $scope.print = true;
                                        $scope.groups = promise.groupalldata;
                                        $scope.Grid_view = false;
                                        $scope.print_flag = false;
                                        $scope.std = false;
                                        $scope.cls = false;
                                        $scope.had = false;
                                        $scope.grp = true;
                                        $scope.presentCountgrid = promise.groupalldata.length;
                                        //     $scope.tot = $scope.getTotalgrp(promise.groupalldata);
                                        $scope.bulkdownload = true;


                                        $(document).ready(function () {
                                            initGridall();
                                        });

                                        function initGridall() {
                                            $('#gridall').empty();

                                            $("#gridall").kendoGrid({
                                                toolbar: ["excel"],
                                                excel: {
                                                    fileName: "FeeDetails.xlsx",
                                                    proxyURL: "",
                                                    filterable: true,
                                                    allPages: true
                                                },
                                                pdf: {
                                                    avoidLinks: true,
                                                    landscape: true,
                                                    repeatHeaders: true,
                                                    fileName: "FeeDetails.pdf",
                                                    allPages: true
                                                },
                                                dataSource: {
                                                    data: $scope.groups,
                                                    pageSize: 10,
                                                    //aggregate: $scope.tempaggary
                                                    aggregate: [{ field: 'FSS_OBArrearAmount', name: 'FSS_OBArrearAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_RefundAmount', name: 'FSS_RefundAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_AdjustedAmount', name: 'FSS_AdjustedAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_OBExcessAmount', name: 'FSS_OBExcessAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_WaivedAmount', name: 'FSS_WaivedAmount', format: "{0:n2}", aggregate: "sum" }]

                                                },

                                                sortable: true,
                                                pageable: true,
                                                groupable: false,
                                                filterable: true,
                                                columnMenu: true,
                                                reorderable: true,
                                                resizable: true,

                                                columns: [{
                                                    title: "SLNO",
                                                    template: "<span class='row-number'></span>", width: "80px"

                                                },
                                                {
                                                    name: 'TRMR_RouteName', field: 'TRMR_RouteName', title: 'Route Name', width: "130px", format: "{0:n2}", footerTemplate: "Total:",
                                                    groupFooterTemplate: "Total: "


                                                },
                                                {
                                                    name: 'FSS_OBArrearAmount', field: 'FSS_OBArrearAmount', title: 'Opening Balance', width: "130px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_RefundAmount', field: 'FSS_RefundAmount', title: 'Refunded Amount', width: "150px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_AdjustedAmount', field: 'FSS_AdjustedAmount', title: 'Adjusted Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_OBExcessAmount', field: 'FSS_OBExcessAmount', title: 'Excess Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_WaivedAmount', field: 'FSS_WaivedAmount', title: 'Waived Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                }
                                                ],

                                                dataBound: function () {
                                                    var pagenum = this.dataSource.page();
                                                    var pageitms = this.dataSource.pageSize();
                                                    var rows = this.items();
                                                    $(rows).each(function () {
                                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                        var rowLabel = $(this).find(".row-number");
                                                        $(rowLabel).html(index);
                                                    });
                                                }

                                            }).data("kendoGrid");
                                        }



                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                        $scope.bulkdownload = false;
                                    }

                                }
                                else if (promise.radioval == "FHW") {
                                    if (promise.headalldata != null && promise.headalldata != "") {
                                        $scope.heads = promise.headalldata;
                                        $scope.Grid_view = false;
                                        $scope.print_flag = false;
                                        $scope.std = false;
                                        $scope.cls = false;
                                        $scope.had = true;
                                        $scope.grp = false;
                                        $scope.presentCountgrid = $scope.heads.length;
                                        // $scope.tot = $scope.getTotalhd(promise.headalldata);
                                        $scope.bulkdownload = true;

                                        $(document).ready(function () {
                                            initGridall();
                                        });

                                        function initGridall() {
                                            $('#gridall').empty();

                                            $("#gridall").kendoGrid({
                                                toolbar: ["excel"],
                                                excel: {
                                                    fileName: "FeeDetails.xlsx",
                                                    proxyURL: "",
                                                    filterable: true,
                                                    allPages: true
                                                },
                                                pdf: {
                                                    avoidLinks: true,
                                                    landscape: true,
                                                    repeatHeaders: true,
                                                    fileName: "FeeDetails.pdf",
                                                    allPages: true
                                                },
                                                dataSource: {
                                                    data: $scope.heads,
                                                    pageSize: 10,
                                                    //aggregate: $scope.tempaggary
                                                    aggregate: [{ field: 'FSS_OBArrearAmount', name: 'FSS_OBArrearAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_RefundAmount', name: 'FSS_RefundAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_AdjustedAmount', name: 'FSS_AdjustedAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_OBExcessAmount', name: 'FSS_OBExcessAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_WaivedAmount', name: 'FSS_WaivedAmount', format: "{0:n2}", aggregate: "sum" }]

                                                },

                                                sortable: true,
                                                pageable: true,
                                                groupable: false,
                                                filterable: true,
                                                columnMenu: true,
                                                reorderable: true,
                                                resizable: true,

                                                //columns: $scope.colarrayall,
                                                columns: [{
                                                    title: "SLNO",
                                                    template: "<span class='row-number'></span>", width: "80px"

                                                },
                                                {
                                                    name: 'FMH_FeeName', field: 'FMH_FeeName', title: 'Fee Name', width: "130px", format: "{0:n2}", footerTemplate: "Total:",
                                                    groupFooterTemplate: "Total: "


                                                },
                                                {
                                                    name: 'FSS_OBArrearAmount', field: 'FSS_OBArrearAmount', title: 'Opening Balance', width: "130px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_RefundAmount', field: 'FSS_RefundAmount', title: 'Refunded Amount', width: "150px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_AdjustedAmount', field: 'FSS_AdjustedAmount', title: 'Adjusted Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_OBExcessAmount', field: 'FSS_OBExcessAmount', title: 'Excess Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_WaivedAmount', field: 'FSS_WaivedAmount', title: 'Waived Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                }


                                                ],
                                                dataBound: function () {
                                                    var pagenum = this.dataSource.page();
                                                    var pageitms = this.dataSource.pageSize();
                                                    var rows = this.items();
                                                    $(rows).each(function () {
                                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                        var rowLabel = $(this).find(".row-number");
                                                        $(rowLabel).html(index);
                                                    });
                                                }

                                            }).data("kendoGrid");
                                        }


                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                        $scope.bulkdownload = false;
                                    }

                                }
                                else if (promise.radioval == "FCW") {
                                    if (promise.classalldata != null && promise.classalldata != "") {
                                        $scope.classes = promise.classalldata;

                                        if ($scope.classes.length != 0) {
                                            //$scope.Grid_view = true;
                                            $scope.print_flag = false;
                                            $scope.std = false;
                                            $scope.cls = true;
                                            $scope.had = false;
                                            $scope.grp = false;
                                            $scope.presentCountgrid = $scope.classes.length;
                                            // $scope.tot = $scope.getTotalcls(promise.classalldata);
                                            $scope.bulkdownload = true;

                                            $(document).ready(function () {
                                                initGridall();
                                            });

                                            function initGridall() {
                                                $('#gridall').empty();

                                                $("#gridall").kendoGrid({
                                                    toolbar: ["excel"],
                                                    excel: {
                                                        fileName: "FeeDetails.xlsx",
                                                        proxyURL: "",
                                                        filterable: true,
                                                        allPages: true
                                                    },
                                                    pdf: {
                                                        avoidLinks: true,
                                                        landscape: true,
                                                        repeatHeaders: true,
                                                        fileName: "FeeDetails.pdf",
                                                        allPages: true
                                                    },
                                                    dataSource: {
                                                        data: $scope.classes,
                                                        pageSize: 10,
                                                        aggregate: [{ field: 'FSS_OBArrearAmount', name: 'FSS_OBArrearAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_RefundAmount', name: 'FSS_RefundAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_AdjustedAmount', name: 'FSS_AdjustedAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_OBExcessAmount', name: 'FSS_OBExcessAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_WaivedAmount', name: 'FSS_WaivedAmount', format: "{0:n2}", aggregate: "sum" }]

                                                    },

                                                    sortable: true,
                                                    pageable: true,
                                                    groupable: false,
                                                    filterable: true,
                                                    columnMenu: true,
                                                    reorderable: true,
                                                    resizable: true,

                                                    columns: [{
                                                        title: "SLNO",
                                                        template: "<span class='row-number'></span>", width: "80px"

                                                    },

                                                    {
                                                        name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class:Section', width: "130px", format: "{0:n2}", footerTemplate: "Total:",
                                                        groupFooterTemplate: "Total: "


                                                    },
                                                    {
                                                        name: 'FSS_OBArrearAmount', field: 'FSS_OBArrearAmount', title: 'Opening Balance', width: "130px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_RefundAmount', field: 'FSS_RefundAmount', title: 'Refunded Amount', width: "150px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_AdjustedAmount', field: 'FSS_AdjustedAmount', title: 'Adjusted Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_OBExcessAmount', field: 'FSS_OBExcessAmount', title: 'Excess Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_WaivedAmount', field: 'FSS_WaivedAmount', title: 'Waived Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    }
                                                    ],

                                                    dataBound: function () {
                                                        var pagenum = this.dataSource.page();
                                                        var pageitms = this.dataSource.pageSize();
                                                        var rows = this.items();
                                                        $(rows).each(function () {
                                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                            var rowLabel = $(this).find(".row-number");
                                                            $(rowLabel).html(index);
                                                        });
                                                    }

                                                }).data("kendoGrid");
                                            }

                                        }
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                        $scope.bulkdownload = false;
                                    }

                                }
                                else if (promise.radioval == "FSW") {
                                    if (promise.studentalldata != null && promise.studentalldata != "") {
                                        $scope.students = promise.studentalldata;
                                        if ($scope.students.length != 0) {
                                            //$scope.Grid_view = false;
                                            $scope.print_flag = false;
                                            $scope.std = true;
                                            $scope.cls = false;
                                            $scope.had = false;
                                            $scope.grp = false;
                                            $scope.presentCountgrid = $scope.students.length;
                                            //  $scope.tot = $scope.getTotalstd(promise.studentalldata);
                                            $scope.bulkdownload = true;

                                            $(document).ready(function () {
                                                initGridall();
                                            });

                                            function initGridall() {
                                                $('#gridall').empty();

                                                $("#gridall").kendoGrid({
                                                    toolbar: ["excel"],
                                                    excel: {
                                                        fileName: "FeeDetails.xlsx",
                                                        proxyURL: "",
                                                        filterable: true,
                                                        allPages: true
                                                    },
                                                    pdf: {
                                                        avoidLinks: true,
                                                        landscape: true,
                                                        repeatHeaders: true,
                                                        fileName: "FeeDetails.pdf",
                                                        allPages: true
                                                    },
                                                    dataSource: {
                                                        data: $scope.students,
                                                        pageSize: 10,
                                                        //aggregate: $scope.tempaggary
                                                        aggregate: [{ field: 'FSS_OBArrearAmount', name: 'FSS_OBArrearAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_RefundAmount', name: 'FSS_RefundAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_AdjustedAmount', name: 'FSS_AdjustedAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_OBExcessAmount', name: 'FSS_OBExcessAmount', format: "{0:n2}", aggregate: "sum" },
                                                        { field: 'FSS_WaivedAmount', name: 'FSS_WaivedAmount', format: "{0:n2}", aggregate: "sum" }]

                                                    },

                                                    sortable: true,
                                                    pageable: true,
                                                    groupable: false,
                                                    filterable: true,
                                                    columnMenu: true,
                                                    reorderable: true,
                                                    resizable: true,

                                                    // columns: $scope.colarrayall,
                                                    columns: [{
                                                        title: "SLNO",
                                                        template: "<span class='row-number'></span>", width: "80px"

                                                    },

                                                    {
                                                        name: 'StudentName', field: 'StudentName', title: 'Student Name', width: "130px"


                                                    },

                                                    {
                                                        name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class:Section', width: "130px", format: "{0:n2}", footerTemplate: "Total:",
                                                        groupFooterTemplate: "Total: "



                                                    },
                                                    {
                                                        name: 'FSS_OBArrearAmount', field: 'FSS_OBArrearAmount', title: 'Opening Balance', width: "130px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_RefundAmount', field: 'FSS_RefundAmount', title: 'Refunded Amount', width: "150px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_AdjustedAmount', field: 'FSS_AdjustedAmount', title: 'Adjusted Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_OBExcessAmount', field: 'FSS_OBExcessAmount', title: 'Excess Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    },
                                                    {
                                                        name: 'FSS_WaivedAmount', field: 'FSS_WaivedAmount', title: 'Waived Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                    }


                                                    ],
                                                    dataBound: function () {
                                                        var pagenum = this.dataSource.page();
                                                        var pageitms = this.dataSource.pageSize();
                                                        var rows = this.items();
                                                        $(rows).each(function () {
                                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                            var rowLabel = $(this).find(".row-number");
                                                            $(rowLabel).html(index);
                                                        });
                                                    }

                                                }).data("kendoGrid");
                                            }

                                        }
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                        $scope.bulkdownload = false;
                                    }

                                }
                            }
                            else {
                                if (promise.type == "WO") {
                                    if (promise.duration != null && promise.duration != "") {
                                        // $scope.grid_view = true;
                                        $scope.print = true;
                                        $scope.waived = promise.duration;
                                        $scope.Grid_view = true;
                                        $scope.print_flag = false;
                                        $scope.waiveoff = true;
                                        $scope.false = true;
                                        $scope.presentCountgrid = promise.duration.length;
                                        $scope.bulkdownload = true;


                                        $(document).ready(function () {
                                            initGridall();
                                        });

                                        function initGridall() {

                                            $('#gridall').empty();

                                            $("#gridall").kendoGrid({
                                                toolbar: ["excel"],
                                                excel: {
                                                    fileName: "FeeDetails.xlsx",
                                                    proxyURL: "",
                                                    filterable: true,
                                                    allPages: true
                                                },
                                                pdf: {
                                                    avoidLinks: true,
                                                    landscape: true,
                                                    repeatHeaders: true,
                                                    fileName: "FeeDetails.pdf",
                                                    allPages: true
                                                },
                                                dataSource: {
                                                    data: $scope.waived,
                                                    pageSize: 10,

                                                    aggregate: [{ field: 'FSS_WaivedAmount', name: 'FSS_WaivedAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_TotalToBePaid', name: 'FSS_TotalToBePaid', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_PaidAmount', name: 'FSS_PaidAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_ToBePaid', name: 'FSS_ToBePaid', format: "{0:n2}", aggregate: "sum" }],


                                                },

                                                sortable: true,
                                                pageable: true,
                                                groupable: false,
                                                filterable: true,
                                                columnMenu: true,
                                                reorderable: true,
                                                resizable: true,

                                                //columns: $scope.colarrayall,
                                                columns: [{
                                                    title: "SLNO",
                                                    template: "<span class='row-number'></span>", width: "80px"
                                                }, {
                                                    name: 'StudentName', field: 'StudentName', title: 'Student Name', width: "130px"
                                                }, {
                                                    name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class:Section', width: "130px"
                                                }, {
                                                    name: 'FMH_FeeName', field: 'FMH_FeeName', title: 'Fee Head', width: "130px", format: "{0:n2}", footerTemplate: "Total:",
                                                    groupFooterTemplate: "Total: "
                                                },
                                                {
                                                    name: 'FSS_WaivedAmount', field: 'FSS_WaivedAmount', title: 'Waivedoff Amount', width: "130px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_TotalToBePaid', field: 'FSS_TotalToBePaid', title: 'Total Amount', width: "150px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', title: 'Paid Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_ToBePaid', field: 'FSS_ToBePaid', title: 'Balance Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                }




                                                ],

                                                dataBound: function () {
                                                    var pagenum = this.dataSource.page();
                                                    var pageitms = this.dataSource.pageSize();
                                                    var rows = this.items();
                                                    $(rows).each(function () {
                                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                        var rowLabel = $(this).find(".row-number");
                                                        $(rowLabel).html(index);
                                                    });
                                                }

                                            }).data("kendoGrid");
                                        }

                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                        $scope.bulkdownload = false;
                                    }

                                }
                                else if (promise.type == "OB") {
                                    if (promise.categorydata != null && promise.categorydata != "") {
                                        $scope.grid_view = false;
                                        $scope.print = false;
                                        $scope.adjamt1 = false;
                                        $scope.opnbln = promise.categorydata;
                                        $scope.Grid_view = false;
                                        $scope.print_flag = false;
                                        $scope.opnblnd = true;
                                        $scope.waiveoff = false;
                                        $scope.presentCountgrid = promise.categorydata.length;
                                        // $scope.tot = $scope.getTotalwaived(promise.duration);
                                        $scope.bulkdownload = true;


                                        $(document).ready(function () {
                                            initGridall();
                                        });

                                        function initGridall() {
                                            $('#gridall').empty();

                                            $("#gridall").kendoGrid({
                                                toolbar: ["excel"],
                                                excel: {
                                                    fileName: "FeeDetails.xlsx",
                                                    proxyURL: "",
                                                    filterable: true,
                                                    allPages: true
                                                },
                                                pdf: {
                                                    avoidLinks: true,
                                                    landscape: true,
                                                    repeatHeaders: true,
                                                    fileName: "FeeDetails.pdf",
                                                    allPages: true
                                                },
                                                dataSource: {
                                                    data: $scope.students,
                                                    pageSize: 10,
                                                    //  aggregate: $scope.tempaggary
                                                    aggregate: [{ field: 'FMOB_Institution_Due', name: 'FMOB_Institution_Due', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_TotalToBePaid', name: 'FSS_TotalToBePaid', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_PaidAmount', name: 'FSS_PaidAmount', format: "{0:n2}", aggregate: "sum" },
                                                    { field: 'FSS_ToBePaid', name: 'FSS_ToBePaid', format: "{0:n2}", aggregate: "sum" }]

                                                },

                                                sortable: true,
                                                pageable: true,
                                                groupable: false,
                                                filterable: true,
                                                columnMenu: true,
                                                reorderable: true,
                                                resizable: true,

                                                //columns: $scope.colarrayall,
                                                columns: [{
                                                    title: "SLNO",
                                                    template: "<span class='row-number'></span>", width: "80px"

                                                },

                                                {
                                                    name: 'StudentName', field: 'StudentName', title: 'Student Name', width: "130px"


                                                },

                                                {
                                                    name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class:Section', width: "130px"


                                                },
                                                {
                                                    name: 'FMH_FeeName', field: 'FMH_FeeName', title: 'Fee Head', width: "130px"


                                                },
                                                {
                                                    name: 'FMOB_Institution_Due', field: 'FMOB_Institution_Due', title: 'Institution Due', width: "130px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_TotalToBePaid', field: 'FSS_TotalToBePaid', title: 'Total Charges', width: "150px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', title: 'Paid Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                },
                                                {
                                                    name: 'FSS_ToBePaid', field: 'FSS_ToBePaid', title: 'Balance Amount', width: "100px", format: "{0:n2}", aggregates: ["sum"], footerTemplate: "#=sum#"
                                                }


                                                ],
                                                dataBound: function () {
                                                    var pagenum = this.dataSource.page();
                                                    var pageitms = this.dataSource.pageSize();
                                                    var rows = this.items();
                                                    $(rows).each(function () {
                                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                        var rowLabel = $(this).find(".row-number");
                                                        $(rowLabel).html(index);
                                                    });
                                                }

                                            }).data("kendoGrid");
                                        }


                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                        $scope.bulkdownload = false;
                                    }
                                }

                                else {
                                    if (promise.fillstudent != null && promise.fillstudent != "") {


                                        $scope.adjamt1 = true;
                                        $scope.grid_view = true;
                                        $scope.print = true;
                                        $scope.Grid_view = true;
                                        $scope.print_flag = false;
                                        $scope.opnblnd = false;
                                        $scope.waiveoff = false;
                                        $scope.term_list1 = [];
                                        $scope.term_list = [];
                                        $scope.adjusted = promise.fillstudent;
                                        var temp_recp_list1 = promise.fillstudent;


                                        for (var m = 0; m < promise.fillstudent.length; m++) {

                                            var stu_id = promise.fillstudent[m].AMST_Id;
                                            var FSA_Id = promise.fillstudent[m].FSA_Id;
                                            var class_n = promise.fillstudent[m].ASMCL_ClassName;
                                            var adj_amt = promise.fillstudent[m].FSA_AdjustedAmount;
                                            var adj_dt = promise.fillstudent[m].FSA_Date;
                                            var stu_nm = promise.fillstudent[m].StudentName;
                                            var already_cnt = 0;

                                            angular.forEach($scope.term_list1, function (itm1) {
                                                if (itm1.stu_id == stu_id && itm1.stu_rpt.FSA_Id == FSA_Id) {
                                                    already_cnt += 1;
                                                }
                                            })

                                            if (already_cnt == 0) {
                                                $scope.temp_stu_rpt1 = [];
                                                var test1 = [];
                                                angular.forEach(temp_recp_list1, function (itm) {
                                                    if (itm.AMST_Id == stu_id && itm.FSA_Id == FSA_Id) {
                                                        $scope.temp_stu_rpt1.push(itm);
                                                    }
                                                })
                                                angular.forEach($scope.temp_stu_rpt1, function (itm1) {
                                                    var fsa_id = itm1.FSA_Id;
                                                    var fmh_name = itm1.FMH_FeeName;
                                                    if (itm1.FSA_Id == fsa_id) {
                                                        test1.push({ FSA_Id: itm1.FSA_Id, FMH_FeeName: itm1.FMH_FeeName });
                                                    }
                                                })

                                                $scope.term_list1.push({ stu_id: stu_id, FSA_Id: FSA_Id, class_name: class_n, adjamt: adj_amt, adjdate: adj_dt, stuname: stu_nm, stu_rpt: test1 });
                                            }
                                        }
                                        var final_list = [];
                                        $scope.admsudentslist = [];
                                        for (var l = 0; l < $scope.term_list1.length; l++) {
                                            var st_id = $scope.term_list1[l].stu_id;
                                            var fsa_id = $scope.term_list1[l].FSA_Id;
                                            var st_id_cnt = 0;
                                            angular.forEach($scope.admsudentslist, function (itm1) {
                                                if (itm1.stu_id == st_id && itm1.FSA_Id == fsa_id) {
                                                    st_id_cnt += 1;
                                                }
                                            })
                                            if (st_id_cnt == 0) {
                                                $scope.admsudentslist.push($scope.term_list1[l]);
                                            }
                                        }
                                        $scope.presentCountgrid = $scope.admsudentslist.length;
                                    }
                                    else {
                                        swal("No Record Found");
                                        $scope.Grid_view = false;
                                        $scope.print_flag = true;
                                        $scope.bulkdownload = false;
                                    }

                                }

                            }


                        })
                }
                else {
                    $scope.submitted = true;
                }
            }
            else {
                $scope.Grid_view = true;
                $scope.bulkdownload = false;
            }
        };

    }
})();
