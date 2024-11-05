(function () {
    'use strict';
    angular
        .module('app')
        .controller('SCHTransactionDetailsReportController', SCHTransactionDetailsReportController)

    SCHTransactionDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout','Excel']
    function SCHTransactionDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.statuscount = true;
        $scope.statuscount1 = false;

        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //-------------Load-data...
        $scope.loaddata = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("SCHTransactionDetailsReport/getdetails", pageid).then(function (promise) {

                $scope.deptlist = promise.deptlist;
                $scope.msterliblist1 = promise.msterliblist1;
            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //------------End-Load-data...

        //---------------Get-Report
        $scope.get_report = function () {
            $scope.colarrayall = [];
            if ($scope.myForm.$valid) {
                if ($scope.statuscount != false || $scope.statuscount1 != false) {



                    var data = {};
                    debugger;
                    if ($scope.statuscount == true) {
                        var fromdate1 = $scope.IssueFromDate == null ? "" : $filter('date')($scope.IssueFromDate, "yyyy-MM-dd");
                        var todate1 = $scope.IssueToDate == null ? "" : $filter('date')($scope.IssueToDate, "yyyy-MM-dd");
                    }
                    else {
                        var fromdate1 = undefined;
                        var todate1 = undefined;
                    }


                    if ($scope.statuscount1 == true) {
                        var fromdate2 = $scope.returnFromDate == null ? "" : $filter('date')($scope.returnFromDate, "yyyy-MM-dd");
                        var todate2 = $scope.returnToDate == null ? "" : $filter('date')($scope.returnToDate, "yyyy-MM-dd");
                    }
                    else {
                        var fromdate2 = undefined;
                        var todate2 = undefined;
                    }

                    var data = {
                        "Type": $scope.issuertype1,
                        "Type2": $scope.Type2,
                        "Fromdate": fromdate1,
                        "ToDate": todate1,
                        "retFromdate": fromdate2,
                        "retToDate": todate2,
                        "LMD_Id": $scope.lmD_Id,
                        "statuscount": $scope.statuscount,
                        "statuscount1": $scope.statuscount1,
                        "LMAL_Id": $scope.LMAL_Id,
                    }




                    apiService.create("SCHTransactionDetailsReport/get_report", data).then(function (promise) {
                        if (promise.griddata.length > 0) {

                            $scope.getdata = promise.griddata;
                            angular.forEach($scope.getdata, function (ff) {
                                ff.LBTR_IssuedDate = ff.LBTR_IssuedDate == null ? "" : $filter('date')(ff.LBTR_IssuedDate, "dd/MM/yyyy");

                                if (ff.LBTR_Status == 'Return') {
                                    ff.LBTR_ReturnedDate = ff.LBTR_ReturnedDate == null ? "" : $filter('date')(ff.LBTR_ReturnedDate, "dd/MM/yyyy");
                                }
                                else {
                                    ff.LBTR_ReturnedDate = '';
                                }


                            })

                            console.log($scope.getdata)

                            $scope.tablediv = true;;
                            $scope.printd = true;


                            if ($scope.issuertype1 == 'std') {
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"

                                },

                                {
                                    name: 'LMB_BookTitle', field: 'LMB_BookTitle', title: 'Book Title', width: "130px"
                                },
                                {
                                    name: 'LMBANO_AccessionNo', field: 'LMBANO_AccessionNo', title: 'Accession No', width: "150px"
                                },
                                {
                                    name: 'AMST_Name', field: 'AMST_Name', title: 'Name', width: "150px"
                                },
                                {
                                    name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Adm. No.'
                                },


                                {
                                    name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class', width: "130px"
                                },
                                {
                                    name: 'ASMC_SectionName', field: 'ASMC_SectionName', title: 'Section', width: "130px"
                                },
                                {
                                    name: 'LBTR_Status', field: 'LBTR_Status', title: 'Book Status', width: "130px"
                                },
                                {
                                    name: 'LBTR_IssuedDate', field: 'LBTR_IssuedDate', title: 'Issue Date', width: "130px", template: "#= kendo.toString(LBTR_IssuedDate, 'dd/MM/yyyy') #"
                                },
                                {
                                    name: 'LBTR_ReturnedDate', field: 'LBTR_ReturnedDate', title: 'Return Date', width: "130px", template: "#= kendo.toString(LBTR_ReturnedDate, 'dd/MM/yyyy') #"
                                }
                                ];
                            }
                            if ($scope.issuertype1 == 'stf') {
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"

                                },

                                {
                                    name: 'LMB_BookTitle', field: 'LMB_BookTitle', title: 'Book Title', width: "130px"
                                },
                                {
                                    name: 'LMBANO_AccessionNo', field: 'LMBANO_AccessionNo', title: 'Accession No', width: "150px"
                                },
                                {
                                    name: 'EmpName', field: 'EmpName', title: 'Name', width: "150px"
                                },
                                {
                                    name: 'HRME_EmployeeCode', field: 'HRME_EmployeeCode', title: 'Emp. Code'
                                },

                                {
                                    name: 'LBTR_Status', field: 'LBTR_Status', title: 'Book Status', width: "130px"
                                },
                                {
                                    name: 'LBTR_IssuedDate', field: 'LBTR_IssuedDate', title: 'Issue Date', width: "130px", template: "#= kendo.toString(LBTR_IssuedDate, 'dd/MM/yyyy') #"
                                },
                                {
                                    name: 'LBTR_ReturnedDate', field: 'LBTR_ReturnedDate', title: 'Return Date', width: "130px", template: "#= kendo.toString(LBTR_ReturnedDate, 'dd/MM/yyyy') #"
                                }
                                ];
                            }


                            if ($scope.issuertype1 == 'dep') {
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"

                                },

                                {
                                    name: 'LMB_BookTitle', field: 'LMB_BookTitle', title: 'Book Title', width: "130px"
                                },
                                {
                                    name: 'LMBANO_AccessionNo', field: 'LMBANO_AccessionNo', title: 'Accession No', width: "150px"
                                },
                                {
                                    name: 'HRMD_DepartmentName', field: 'HRMD_DepartmentName', title: 'Department Name', width: "150px"
                                },

                                {
                                    name: 'LBTR_Status', field: 'LBTR_Status', title: 'Book Status', width: "130px"
                                },
                                {
                                    name: 'LBTR_IssuedDate', field: 'LBTR_IssuedDate', title: 'Issue Date', width: "130px", template: "#= kendo.toString(LBTR_IssuedDate, 'dd/MM/yyyy') #"
                                },
                                {
                                    name: 'LBTR_ReturnedDate', field: 'LBTR_ReturnedDate', title: 'Return Date', width: "130px", template: "#= kendo.toString(LBTR_ReturnedDate, 'dd/MM/yyyy') #"
                                }
                                ];
                            }

                            if ($scope.issuertype1 == 'gst') {
                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"

                                },

                                {
                                    name: 'LMB_BookTitle', field: 'LMB_BookTitle', title: 'Book Title', width: "130px"
                                },
                                {
                                    name: 'LMBANO_AccessionNo', field: 'LMBANO_AccessionNo', title: 'Accession No', width: "150px"
                                },
                                {
                                    name: 'LBTR_GuestName', field: 'LBTR_GuestName', title: 'Guest Name', width: "150px"
                                },
                                {
                                    name: 'LBTR_GuestContactNo', field: 'LBTR_GuestContactNo', title: 'Contact No', width: "150px"
                                },
                                {
                                    name: 'LBTR_GuestEmailId', field: 'LBTR_GuestEmailId', title: 'Email', width: "150px"
                                },
                                {
                                    name: 'LBTR_Status', field: 'LBTR_Status', title: 'Book Status', width: "130px"
                                },
                                {
                                    name: 'LBTR_IssuedDate', field: 'LBTR_IssuedDate', title: 'Issue Date', width: "130px", template: "#= kendo.toString(LBTR_IssuedDate, 'dd/MM/yyyy') #"
                                },
                                {
                                    name: 'LBTR_ReturnedDate', field: 'LBTR_ReturnedDate', title: 'Return Date', width: "130px", template: "#= kendo.toString(LBTR_ReturnedDate, 'dd/MM/yyyy') #"
                                }
                                ];
                            }

                            $(document).ready(function () {
                                initGridall();
                            });
                            function initGridall() {
                                $('#gridall').empty();
                                // gridall = $("#gridall").kendoGrid({
                                $("#gridall").kendoGrid({
                                    toolbar: ["excel"],
                                    excel: {
                                        fileName: "BookTransactionDetailsReport.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        //allPages: true,
                                        avoidLinks: true,
                                        //paperSize: "A3",
                                        // margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                                        landscape: true,
                                        repeatHeaders: true,
                                        // template: $("#page-template").html(),
                                        //  scale: 0.8,
                                        fileName: "BookTransactionDetailsReport.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        //type: "odata",
                                        //transport: {
                                        //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                        //},
                                        data: $scope.getdata,
                                        pageSize: 10,
                                        //aggregate: [
                                        //    { name: 'totalpayable', field: 'totalpayable', aggregate: "sum" },
                                        //    { name: 'FSS_PaidAmount', field: 'FSS_PaidAmount', aggregate: "sum" },
                                        //    { name: 'balance', field: 'balance', aggregate: "sum" },
                                        //    { name: 'concession', field: 'concession', aggregate: "sum" },
                                        //    { name: 'fine', field: 'fine', aggregate: "sum" },
                                        //    { name: 'rebate', field: 'rebate', aggregate: "sum" },
                                        //    { name: 'waived', field: 'waived', aggregate: "sum" }

                                        //]
                                    },
                                    sortable: true,
                                    //pageable: false,
                                    pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,
                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize()
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
                            swal('Record Not Available!!!');
                            $state.reload();
                        }
                    })
                }
                else {
                    swal('Select Issue OR Return Date!!!');
                }
            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report

        $scope.transtypechange = function () {
            $scope.colarrayall = [];
            $scope.getdata = [];
            $scope.tablediv = false;



    }

        $scope.getdata = [];

        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookTypeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 10000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }

        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

