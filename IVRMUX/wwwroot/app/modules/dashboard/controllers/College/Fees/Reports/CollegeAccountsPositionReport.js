(function () {
    'use strict';

    angular
        .module('app')
        .controller('CollegeAccountsPositionReportController', CollegeAccountsPositionReportController);

    CollegeAccountsPositionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];
    function CollegeAccountsPositionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.obj = {};

        $scope.asondate = false;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 1;
            $scope.CollegeAccountsPositionReport = false;
            apiService.getURI("CollegeAccountsPositionReport/loaddata/", pageid).
                then(function (promise) {
                    $scope.academicyear = promise.yearlst;
                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.customgrpList = promise.customgrpList;
                    for (var i = 0; i < $scope.customgrpList.length; i++) {
                        $scope.customgrpList[i].Selected1 = true;
                    }

                    $scope.groupList = promise.grouplist;
                    for (var i = 0; i < $scope.groupList.length; i++) {
                        $scope.groupList[i].Selected2 = true;
                    }
                    $scope.feeConfiguration = promise.feeconfiguration;
                    $scope.FMC_GroupOrTermFlg = promise.feeconfiguration[0].fmC_GroupOrTermFlg; 
                    if (promise.feeconfiguration[0].fmC_GroupOrTermFlg == "T") {
                        $scope.termList = promise.termsList;
                        for (var i = 0; i < $scope.termList.length; i++) {
                            $scope.termList[i].Selected3 = true;
                        }
                    }

                });
            $scope.sort = function (keyname) {
                $scope.columnSort = true;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }
        $scope.getSelectedYear = function () {
            var year = $scope.obj.ASMAY_Id;
        }

        $scope.changeradio = function () {
            $scope.feeaccountsPosition = false;
            $scope.feeaccountsPositionReport = [];

        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Cancel = function () {
            $state.reload();
        }
        $scope.isOptionsRequired1 = function () {

            return !$scope.customgrpList.some(function (options) {
                return options.Selected1;
            });
        }
        $scope.isOptionsRequired2 = function () {

            return !$scope.groupList.some(function (options) {
                return options.Selected2;
            });
        }
        $scope.isOptionsRequired3 = function () {

            return !$scope.termList.some(function (options) {
                return options.Selected3;
            });
        }
        $scope.selectedCGList = [];
        $scope.groupByCG = function () {
            $scope.selectedCGList.length = 0;
            for (var j = 0; j < $scope.customgrpList.length; j++) {
                if ($scope.customgrpList[j].Selected1 == true) {
                    $scope.selectedCGList.push($scope.customgrpList[j]);
                }
            }
            var selectedCG = {
                "selectedCGList": $scope.selectedCGList
            }
            apiService.create("CollegeAccountsPositionReport/getgroupByCG/", selectedCG).
                then(function (promise) {
                    $scope.groupList = promise.grouplist;
                    for (var i = 0; i < $scope.groupList.length; i++) {
                        $scope.groupList[i].Selected2 = true;
                    }
                })
        }

        $scope.submitted = false;
        $scope.showExportbutton = true;
        $scope.selectedGroupList = [];
        $scope.selectedTermList = [];
        $scope.selectedCustGrpList = [];
        $scope.getReport = function () {

            if ($scope.myForm.$valid) {

                $scope.selectedGroupList.length = 0;
                $scope.selectedTermList.length = 0;
                $scope.selectedCustGrpList.length = 0;
                if ($scope.customgrpList != "" && $scope.customgrpList != null) {
                    if ($scope.customgrpList.length > 0) {
                        for (var i = 0; i < $scope.customgrpList.length; i++) {
                            if ($scope.customgrpList[i].Selected1 == true) {
                                $scope.selectedCustGrpList.push($scope.customgrpList[i]);
                            }
                        }
                    }
                }

                if ($scope.groupList != "" && $scope.groupList != null) {
                    if ($scope.groupList.length > 0) {
                        for (var i = 0; i < $scope.groupList.length; i++) {
                            if ($scope.groupList[i].Selected2 == true) {
                                $scope.selectedGroupList.push($scope.groupList[i]);
                            }
                        }
                    }
                }



                var FMT_Ids = [];

                angular.forEach($scope.termList, function (ty) {
                    if (ty.Selected3) {
                        FMT_Ids.push(ty.fmT_Id);
                    }
                })
                $scope.obj.dt = new Date().toDateString();
                $scope.obj.frmdate = new Date().toDateString();
                $scope.obj.todate = new Date().toDateString();

                if ($scope.obj.status == undefined) {
                    $scope.obj.status = "false"
                }

                if ($scope.asondate == true) {
                    var data = {
                        "type": $scope.obj.rdo,
                        "Status": $scope.obj.status,
                        "ASMAY_Id": $scope.obj.ASMAY_Id,
                        //"ASMCL_Id": $scope.obj.ASMCL_Id,
                        //"ASMS_Id": $scope.obj.ASMS_Id,
                        "Date": $scope.obj.dt,
                        "FromDate": $scope.obj.frmdate,
                        "ToDate": $scope.obj.todate,
                        "selectedCGList": $scope.selectedCustGrpList,
                        "selectedGroup": $scope.selectedGroupList,
                        FMT_Ids: FMT_Ids,
                        "asondate": $scope.asonduedate.toDateString(),
                        "AMCO_Id": $scope.obj.AMCO_Id,
                        "AMB_Id": $scope.obj.AMB_Id,
                        "AMSE_Id": $scope.obj.AMSE_Id,
                    }
                }
                else {
                    var data = {
                        "type": $scope.obj.rdo,
                        "Status": $scope.obj.status,
                        "ASMAY_Id": $scope.obj.ASMAY_Id,
                        "ASMCL_Id": $scope.obj.ASMCL_Id,
                        "ASMS_Id": $scope.obj.ASMS_Id,
                        "Date": $scope.obj.dt,
                        "FromDate": $scope.obj.frmdate,
                        "ToDate": $scope.obj.todate,
                        "selectedCGList": $scope.selectedCustGrpList,
                        "selectedGroup": $scope.selectedGroupList,
                        FMT_Ids: FMT_Ids,
                    }
                }

                apiService.create("CollegeAccountsPositionReport/getReport/", data).
                    then(function (promise) {
                        if (promise.count > 0) {
                            $scope.totalCharges = 0;
                            $scope.totalConcession = 0;
                            $scope.totalRebate = 0;
                            $scope.totalWaiveOff = 0;
                            $scope.totalFine = 0;
                            $scope.totalCollection = 0;
                            $scope.totalDebitBal = 0;
                            $scope.totalLastYrDue = 0;

                            $scope.totalPFY_EndDate_DebitBalance = 0;
                            $scope.totalCFY_PaidAmount = 0;
                            $scope.totalCFY_BalanceAmount = 0;
                            $scope.totalExcessAmount = 0;
                            $scope.totaloverallpaidAmount = 0;

                            $scope.feeaccountsPosition = true;
                            $scope.feeaccountsPositionReport = promise.studentdata;

                            if ($scope.asondate == true) {
                                for (var i = 0; i < $scope.feeaccountsPositionReport.length; i++) {

                                    $scope.totalCharges = Number($scope.totalCharges) + Number($scope.feeaccountsPositionReport[i].charges);
                                    $scope.totalConcession = Number($scope.totalConcession) + Number($scope.feeaccountsPositionReport[i].concession);
                                    $scope.totalRebate = Number($scope.totalRebate) + Number($scope.feeaccountsPositionReport[i].rebate);
                                    $scope.totalWaiveOff = Number($scope.totalWaiveOff) + Number($scope.feeaccountsPositionReport[i].waiveOff);
                                    $scope.totalFine = Number($scope.totalFine) + Number($scope.feeaccountsPositionReport[i].fine);
                                    $scope.totalCollection = Number($scope.totalCollection) + Number($scope.feeaccountsPositionReport[i].collection);
                                    $scope.totalLastYrDue = Number($scope.totalLastYrDue) + Number($scope.feeaccountsPositionReport[i].lastYearDue);

                                    $scope.totalPFY_EndDate_DebitBalance = Number($scope.totalPFY_EndDate_DebitBalance) + Number($scope.feeaccountsPositionReport[i].pfY_EndDate_DebitBalance);
                                    $scope.totalCFY_BalanceAmount = Number($scope.totalCFY_BalanceAmount) + Number($scope.feeaccountsPositionReport[i].cfY_BalanceAmount);
                                    $scope.totalCFY_PaidAmount = Number($scope.totalCFY_PaidAmount) + Number($scope.feeaccountsPositionReport[i].cfY_PaidAmount);
                                    $scope.totalExcessAmount = Number($scope.totalExcessAmount) + Number($scope.feeaccountsPositionReport[i].excessAmount);
                                    $scope.totaloverallpaidAmount = Number($scope.totalCFY_PaidAmount) + Number($scope.totalCollection)
                                }
                            }
                            else {
                                for (var i = 0; i < $scope.feeaccountsPositionReport.length; i++) {
                                    $scope.totalCharges = Number($scope.totalCharges) + Number($scope.feeaccountsPositionReport[i].charges);
                                    $scope.totalConcession = Number($scope.totalConcession) + Number($scope.feeaccountsPositionReport[i].concession);
                                    $scope.totalRebate = Number($scope.totalRebate) + Number($scope.feeaccountsPositionReport[i].rebate);
                                    $scope.totalWaiveOff = Number($scope.totalWaiveOff) + Number($scope.feeaccountsPositionReport[i].waiveOff);
                                    $scope.totalFine = Number($scope.totalFine) + Number($scope.feeaccountsPositionReport[i].fine);
                                    $scope.totalCollection = Number($scope.totalCollection) + Number($scope.feeaccountsPositionReport[i].collection);
                                    $scope.totalDebitBal = Number($scope.totalDebitBal) + Number($scope.feeaccountsPositionReport[i].debitBalance);
                                    $scope.totalLastYrDue = Number($scope.totalLastYrDue) + Number($scope.feeaccountsPositionReport[i].lastYearDue);
                                }
                            }

                            $scope.showExportbutton = false;
                        }
                        else {
                            swal("No Records Found");
                            $scope.CollegeAccountsPositionReport = false;
                            $scope.showExportbutton = true;
                        }
                    });

            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.printdatatable = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.feeaccountsPositionReport, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length > 0) {
                $scope.totalCharges = 0;
                $scope.totalConcession = 0;
                $scope.totalRebate = 0;
                $scope.totalWaiveOff = 0;
                $scope.totalFine = 0;
                $scope.totalCollection = 0;
                $scope.totalDebitBal = 0;
                $scope.totalLastYrDue = 0;
                for (var i = 0; i < $scope.printdatatable.length; i++) {
                    $scope.totalCharges = Number($scope.totalCharges) + Number($scope.printdatatable[i].charges);
                    $scope.totalConcession = Number($scope.totalConcession) + Number($scope.printdatatable[i].concession);
                    $scope.totalRebate = Number($scope.totalRebate) + Number($scope.printdatatable[i].rebate);
                    $scope.totalWaiveOff = Number($scope.totalWaiveOff) + Number($scope.printdatatable[i].waiveOff);
                    $scope.totalFine = Number($scope.totalFine) + Number($scope.printdatatable[i].fine);
                    $scope.totalCollection = Number($scope.totalCollection) + Number($scope.printdatatable[i].collection);
                    $scope.totalDebitBal = Number($scope.totalDebitBal) + Number($scope.printdatatable[i].debitBalance);
                    $scope.totalLastYrDue = Number($scope.totalLastYrDue) + Number($scope.printdatatable[i].lastYearDue);
                }
            }
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.feeaccountsPositionReport.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.totalCharges = 0;
                $scope.totalConcession = 0;
                $scope.totalRebate = 0;
                $scope.totalWaiveOff = 0;
                $scope.totalFine = 0;
                $scope.totalCollection = 0;
                $scope.totalDebitBal = 0;
                $scope.totalLastYrDue = 0;
                for (var i = 0; i < $scope.printdatatable.length; i++) {
                    $scope.totalCharges = Number($scope.totalCharges) + Number($scope.printdatatable[i].charges);
                    $scope.totalConcession = Number($scope.totalConcession) + Number($scope.printdatatable[i].concession);
                    $scope.totalRebate = Number($scope.totalRebate) + Number($scope.printdatatable[i].rebate);
                    $scope.totalWaiveOff = Number($scope.totalWaiveOff) + Number($scope.printdatatable[i].waiveOff);
                    $scope.totalFine = Number($scope.totalFine) + Number($scope.printdatatable[i].fine);
                    $scope.totalCollection = Number($scope.totalCollection) + Number($scope.printdatatable[i].collection);
                    $scope.totalDebitBal = Number($scope.totalDebitBal) + Number($scope.printdatatable[i].debitBalance);
                    $scope.totalLastYrDue = Number($scope.totalLastYrDue) + Number($scope.printdatatable[i].lastYearDue);
                }
            }
        }
        $scope.exportdiv = false;
        $scope.ExportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                $scope.exportdiv = false;
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
                $scope.exportdiv = false;
            }
        }
        $scope.printData = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
    }

})();
