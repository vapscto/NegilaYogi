(function () {
    'use strict';
    angular
        .module('app')
        .controller('VpfSummaryController', VpfSummaryController)

    VpfSummaryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function VpfSummaryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;

            $scope.all_check_empl = function () {
                var checkStatus = $scope.empl;
                var count = 0;
                angular.forEach($scope.employeelist, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
            }
            $scope.isOptionsRequired3 = function () {
                return !$scope.employeelist.some(function (options) {
                    return options.selected;
                });
            }

            $scope.addColumn4 = function () {

                $scope.empl = $scope.employeelist.every(function (options) {
                    return options.selected;
                });
            }


            apiService.getURI("PFTransaction/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }
                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }
                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeelist = promise.employeedropdown;
                }

            });
        };


        $scope.grandtotalreport = function () {
            $scope.summaryReportCal = [];
            $scope.summaryReport = [];
        }

        $scope.summaryReport = [];
        $scope.summaryReportCal = [];
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            $scope.albumNameArray1 = [];
            angular.forEach($scope.employeelist, function (role) {
                if (role.selected) $scope.albumNameArray1.push(role);
            })

            angular.forEach($scope.leaveyeardropdown, function (itm) {
                if ($scope.IMFY_Id == itm.imfY_Id) {
                    $scope.financialyear = itm.imfY_FinancialYear
                }

            });

            if ($scope.myForm.$valid) {
                var data = {
                    "IMFY_Id": $scope.IMFY_Id,
                    employee: $scope.albumNameArray1,
                    "Flag": $scope.rdopunch,
                    "Procedure": "HR_VPF_YearlyReport"
                };
                apiService.create("PFTransaction/getReport", data).then(function (promise) {                   
                    if (promise.employeeDetails != null && promise.employeeDetails.length > 0 && promise.employeeDetails != undefined) {
                        $scope.employeeDetails = promise.employeeDetails
                        $scope.HRME_EmployeeFirstName = $scope.employeeDetails[0].hrmE_EmployeeFirstName;
                        $scope.MI_Name = $scope.employeeDetails[0].mI_Name;
                        $scope.HRME_PFAccNo = $scope.employeeDetails[0].hrmE_PFAccNo;
                        $scope.HRME_EmployeeCode = $scope.employeeDetails[0].hrmE_EmployeeCode;
                    }


                    if (promise.employeePFreportDetails !== null && promise.employeePFreportDetails.length > 0) {
                        $scope.summaryReport = promise.employeePFreportDetails;
                        $scope.summaryReportCal = promise.employeePFreportDetails;
                        $scope.openingbalance = 0;
                        $scope.contribution = 0;
                        $scope.interest = 0;
                        $scope.totalamount = 0;
                        $scope.settlement = 0;
                        $scope.closingbalance = 0;
                        for (var i = 0; i < $scope.summaryReportCal.length; i++) {
                            $scope.openingbalance = $scope.openingbalance + $scope.summaryReportCal[i].HREVPFST_VOBAmount;
                            $scope.contribution = $scope.contribution + $scope.summaryReportCal[i].HREVPFST_Contribution;
                            $scope.interest = $scope.interest + $scope.summaryReportCal[i].HREVPFST_Intersest;
                            $scope.totalamount = $scope.totalamount + $scope.summaryReportCal[i].GRANDTOTAL;
                            $scope.settlement = $scope.settlement + $scope.summaryReportCal[i].HREVPFST_SettledAmount;
                            $scope.closingbalance = $scope.closingbalance + $scope.summaryReportCal[i].HREVPFST_ClosingBalance;
                        }
                    }
                    else {
                        swal("No Record Found");
                    }

                });
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cleardata = function () {
            $state.reload();
        }
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }


    }


})();