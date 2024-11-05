
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_ItemConsumptionReportController', INV_ItemConsumptionReportController);
    INV_ItemConsumptionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_ItemConsumptionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.obj = {};
        $scope.tablediv = false;
        $scope.printIC = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        //==========================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var data = {
                "optionflag": $scope.optionflag
            };
            apiService.create("INV_ItemConsumptionReport/getloaddata", data).
                then(function (promise) {
                    $scope.get_ICreportdetails = promise.get_ICreportdetails;
                });
        };

        $scope.typerdochange = function (typeflag) {
            if ($scope.optionflag == null || $scope.optionflag == undefined || $scope.optionflag == '' ) {
                $scope.optionflag = 'Item';
            }
            $scope.tablediv = false;
            $scope.get_ICreportdetails = "";
            $scope.get_ICReport = "";
            $scope.startdate = null;
            $scope.enddate = null;
            $scope.tflag = typeflag;
            $scope.loaddata($scope.tflag);
        };
        $scope.onrdochange = function (optionflag) {
            $scope.tablediv = false;
            $scope.get_ICreportdetails = "";
            $scope.get_ICReport = "";
            $scope.startdate = null;
            $scope.enddate = null;
            $scope.flag = optionflag;
            $scope.loaddata($scope.flag);
        };

        //========================STAFF
        $scope.togchkbxST = function () {
            $scope.stall = $scope.get_ICreportdetails.every(function (str) {
                return str.stck;
            });
        };
        $scope.isOptionsRequired = function () {
            if ($scope.optionflag === 'Staff') {
                return !$scope.get_ICreportdetails.some(function (options) {
                    return options.stck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkST = function (stri) {
            $scope.stall = stri;
            var toggleStatus = $scope.stall;
            angular.forEach($scope.get_ICreportdetails, function (st) {
                st.stck = toggleStatus;
            });
        };

        //==================================IC Report
     var   totalqty = 0;
        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.get_ICReport = [];
            $scope.tablediv = false;
            $scope.printsale = false;
            totalqty = 0;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
                $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
                if ($scope.bw_dates === true) {
                    $scope.sDate = $scope.start_Date;
                    $scope.eDate = $scope.end_Date;
                }
                else {
                    $scope.sDate = "";
                    $scope.eDate = "";
                }

                if ($scope.sDate == null || $scope.sDate == undefined) {
                    $scope.sDate = "";
                }
                if ($scope.eDate == null || $scope.eDate == undefined) {
                    $scope.eDate = "";
                }

                if ($scope.typeflag === "All" ) {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": "",
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Item") {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": $scope.obj.INVMI_Id.INVMI_Id,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Staff") {
                    $scope.staffarray = [];
                    angular.forEach($scope.get_ICreportdetails, function (stf) {
                        if (stf.stck === true) {
                            $scope.staffarray.push(stf);
                        }
                    });
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "staffarray": $scope.staffarray,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates
                    };
                }

                else if ($scope.optionflag === "Department") {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": $scope.obj.HRMD_Id.HRMD_Id,
                        "AMST_Id": 0,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                else if ($scope.optionflag === "Student") {
                    data = {
                        "typeflag": $scope.typeflag,
                        "optionflag": $scope.optionflag,
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "INVMI_Id": 0,
                        "HRME_Id": 0,
                        "HRMD_Id": 0,
                        "AMST_Id": $scope.obj.AMST_Id.AMST_Id,
                        "bwdateflag": $scope.bw_dates
                    };
                }
                apiService.create("INV_ItemConsumptionReport/onreport", data).
                    then(function (promise) {
                        if (promise.get_ICReport.length > 0) {
                            $scope.get_ICReport = promise.get_ICReport;
                            $scope.presentCountgrid = $scope.get_ICReport.length;
                            if (promise.get_ICReport.length > 0) {
                                $scope.tablediv = true;
                                $scope.printsale = true;

                                if ($scope.typeflag === "All") {
                                    angular.forEach($scope.get_ICReport, function (sl) {
                                        totalqty += parseFloat(sl.INVTIC_ICQty);
                                        $scope.totalqty = totalqty;
                                        $scope.totalqty = parseFloat($scope.totalqty);
                                        $scope.totalqty = $scope.totalqty.toFixed(2);
                                    });
                                    $scope.colarrayall = [{
                                        title: "SLNO",
                                        template: "<span class='row-number'></span>", width: "80px"
                                    },
                                    {
                                        name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                    },
                                    {
                                        name: 'INVMUOM_UOMName', field: 'INVMUOM_UOMName', title: 'UOM', width: "100px"
                                    },
                                    {
                                        name: 'INVMIC_ICDate', field: 'INVMIC_ICDate', title: 'Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMIC_ICDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                    }
                                        ,
                                    {
                                        name: 'INVTIC_ICQty', field: 'INVTIC_ICQty', title: 'Quantity', width: "100px"
                                    },
                                    {
                                        name: 'INVTIC_Naration', field: 'INVTIC_Naration', title: 'Narration', width: "100px"
                                    }
                                    ];
                                }
                                else if ($scope.typeflag === "Overall") {
                                    angular.forEach($scope.get_ICReport, function (sl) {
                                        totalqty += parseFloat(sl.icQty);
                                        $scope.totalqty = totalqty;
                                        $scope.totalqty = parseFloat($scope.totalqty);
                                        $scope.totalqty = $scope.totalqty.toFixed(2);
                                    });
                                    if ($scope.optionflag === "Item") {

                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                        },
                                        {
                                            name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: "100px"
                                        },
                                        {
                                            name: 'icQty', field: 'icQty', title: 'Quantity', width: "100px"
                                        }];
                                    }

                                    else if ($scope.optionflag === "Staff") {
                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'membername', field: 'membername', title: 'Employee Name', width: "150px"
                                        },
                                        {
                                            name: 'HRME_EmployeeCode', field: 'HRME_EmployeeCode', title: 'Employee Code', width: "100px"
                                        },
                                        {
                                            name: 'icQty', field: 'icQty', title: 'Quantity', width: "100px"
                                        }];

                                    }

                                    else if ($scope.optionflag === "Department") {
                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'membername', field: 'membername', title: 'Department', width: "150px"
                                        },
                                        {
                                            name: 'icQty', field: 'icQty', title: 'Quantity', width: "100px"
                                        }];

                                    }
                                    else if ($scope.optionflag === "Student") {
                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'membername', field: 'membername', title: 'Student Name', width: "150px"
                                        },
                                        {
                                            name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Adm. No.', width: "100px"
                                        },
                                        {
                                            name: 'icQty', field: 'icQty', title: 'Quantity', width: "100px"
                                        }];
                                    }
                                }

                                else if ($scope.typeflag === "Detailed") {
                                    angular.forEach($scope.get_ICReport, function (sl) {
                                        totalqty += parseFloat(sl.INVTIC_ICQty);
                                        $scope.totalqty = totalqty;
                                        $scope.totalqty = parseFloat($scope.totalqty);
                                        $scope.totalqty = $scope.totalqty.toFixed(2);
                                    });
                                    if ($scope.optionflag === "Item") {

                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                        },
                                        {
                                            name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: "100px"
                                        },
                                        {
                                            name: 'INVMIC_ICDate', field: 'INVMIC_ICDate', title: 'Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMIC_ICDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                        },
                                        {
                                            name: 'INVTIC_ICQty', field: 'INVTIC_ICQty', title: 'Quantity', width: "100px"
                                        }];
                                    }

                                    else if ($scope.optionflag === "Staff") {
                                        angular.forEach($scope.get_ICReport, function (sl) {
                                            totalqty += parseFloat(sl.icQty);
                                            $scope.totalqty = totalqty;
                                            $scope.totalqty = parseFloat($scope.totalqty);
                                            $scope.totalqty = $scope.totalqty.toFixed(2);
                                        });
                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                        },
                                        {
                                            name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: "100px"
                                        },
                                        {
                                            name: 'INVMIC_ICDate', field: 'INVMIC_ICDate', title: 'Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMIC_ICDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                        },
                                        {
                                            name: 'membername', field: 'membername', title: 'Employee Name', width: "150px"
                                        },
                                        {
                                            name: 'HRME_EmployeeCode', field: 'HRME_EmployeeCode', title: 'Employee Code', width: "100px"
                                        },
                                        {
                                            name: 'INVTIC_ICQty', field: 'INVTIC_ICQty', title: 'Quantity', width: "100px"
                                        }];

                                    }

                                    else if ($scope.optionflag === "Department") {
                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                        },
                                        {
                                            name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: "100px"
                                        },
                                        {
                                            name: 'INVMIC_ICDate', field: 'INVMIC_ICDate', title: 'Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMIC_ICDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                        },
                                        {
                                            name: 'membername', field: 'membername', title: 'Employee Name', width: "150px"
                                        },
                                        {
                                            name: 'INVTIC_ICQty', field: 'INVTIC_ICQty', title: 'Quantity', width: "100px"
                                        }];

                                    }
                                    else if ($scope.optionflag === "Student") {
                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"
                                        },
                                        {
                                            name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                        },
                                        {
                                            name: 'INVMI_ItemCode', field: 'INVMI_ItemCode', title: 'Item Code', width: "100px"
                                        },
                                        {
                                            name: 'INVMIC_ICDate', field: 'INVMIC_ICDate', title: 'Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMIC_ICDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                        },
                                        {
                                            name: 'membername', field: 'membername', title: 'Employee Name', width: "150px"
                                        },
                                        {
                                            name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Adm. No.', width: "100px"
                                        },
                                        {
                                            name: 'INVTIC_ICQty', field: 'INVTIC_ICQty', title: 'Quantity', width: "100px"
                                        }];
                                    }
                                }

                                $(document).ready(function () {
                                    initGridall();
                                });
                                function initGridall() {
                                    $('#gridall').empty();
                                    //gridall =
                                    $("#gridall").kendoGrid({
                                        toolbar: ["excel"],
                                        excel: {
                                            fileName: "ItemConsumptionReport.xlsx",
                                            proxyURL: "",
                                            filterable: true,
                                            allPages: true
                                        },
                                        pdf: {
                                            avoidLinks: true,
                                            landscape: true,
                                            repeatHeaders: true,
                                            fileName: "ItemConsumptionReport.pdf",
                                            allPages: true
                                        },
                                        dataSource: {
                                            data: $scope.get_ICReport,
                                            pageSize: 10,
                                        },
                                        sortable: true,
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
                                swal("No Record Found...!!");
                                $scope.get_SalesReport = [];
                            }
                        }
                        else {
                            swal("No Record Found...!!");
                            $scope.get_ICReport = [];
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel        
        $scope.exportToExcel = function (export_id) {

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
            $timeout(function () {
                location.href = exportHref;
            }, 100);

        };
        $scope.printData = function () {

            var innerContents = document.getElementById("printIC").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();