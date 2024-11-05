(function () {
    'use strict';
    angular
        .module('app')
        .controller('PFSummaryController', PFSummaryController)

    PFSummaryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PFSummaryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.submitted = false;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
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


        $scope.pfreport = []; $scope.employeeDetails = [];
        $scope.SearchEmployee = function () {
            $scope.totownopeningbal = 0;
            $scope.totschoolopeningbal = 0;
            $scope.totowncontribution = 0;
            $scope.totschoolcontribution = 0;
            $scope.totowninterest = 0;
            $scope.totschoolinterest = 0;
            $scope.totowntotal = 0;
            $scope.totschooltotal = 0;
            $scope.totownsettlement = 0;
            $scope.totschoolsettlement = 0;
            $scope.totownclosingbal = 0;
            $scope.totschoolclosingbal = 0;
            $scope.submitted = true;
            $scope.albumNameArray1 = [];
            angular.forEach($scope.employeelist, function (role) {
                if (role.selected == true) { $scope.albumNameArray1.push(role); }
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
                    "Procedure": "HR_PF_YearlyReport"
                };
                apiService.create("PFTransaction/getReport", data).then(function (promise) {
                    if (promise.employeeDetails !== null && promise.employeeDetails.length > 0 && promise.employeeDetails != undefined) {
                        $scope.employeeDetails = promise.employeeDetails
                        $scope.HRME_EmployeeFirstName = $scope.employeeDetails[0].hrmE_EmployeeFirstName;
                        $scope.MI_Name = $scope.employeeDetails[0].mI_Name;
                        $scope.HRME_PFAccNo = $scope.employeeDetails[0].hrmE_PFAccNo;
                        $scope.HRME_EmployeeCode = $scope.employeeDetails[0].hrmE_EmployeeCode;
                    }

                    if (promise.employeePFreportDetails !== null && promise.employeePFreportDetails.length > 0) {
                        $scope.pfreport = promise.employeePFreportDetails;
                        for (var i = 0; i < promise.employeePFreportDetails.length; i++) {
                            $scope.totownopeningbal = $scope.totownopeningbal + $scope.pfreport[i].HREPFST_OBOwnAmount;
                            $scope.totschoolopeningbal = $scope.totschoolopeningbal + $scope.pfreport[i].HREPFST_OBInstituteAmount;
                            $scope.totowncontribution = $scope.totowncontribution + $scope.pfreport[i].HREVPFST_Contribution;
                            $scope.totschoolcontribution = $scope.totschoolcontribution + $scope.pfreport[i].HREPFST_IntstituteContribution;
                            $scope.totowninterest = $scope.totowninterest + $scope.pfreport[i].HREVPFST_Intersest;
                            $scope.totschoolinterest = $scope.totschoolinterest + $scope.pfreport[i].HREPFST_InstituteInterest;
                            $scope.totowntotal = $scope.totowntotal + $scope.pfreport[i].OWN_GRANDTOTAL;
                            $scope.totschooltotal = $scope.totschooltotal + $scope.pfreport[i].INST_GRANDTOTAL;
                            $scope.totownsettlement = $scope.totownsettlement + $scope.pfreport[i].HREPFST_OwnSettlementAmount;
                            $scope.totschoolsettlement = $scope.totschoolsettlement + $scope.pfreport[i].HREPFST_InstituteLSettlementAmount;
                            $scope.totownclosingbal = $scope.totownclosingbal + $scope.pfreport[i].HREPFST_OwnClosingBalance;
                            $scope.totschoolclosingbal = $scope.totschoolclosingbal + $scope.pfreport[i].HREPFST_InstituteClosingBalance;
                        }
                    }
                    else {
                        swal("No Record Found");
                    }




                });
            }
        };


        $scope.all_check_empl = function () {            var checkStatus = $scope.empl;            var count = 0;            angular.forEach($scope.employeelist, function (itm) {                itm.selected = checkStatus;                if (itm.selected == true) {                    count += 1;                }                else {                    count = 0;                }            });        }        $scope.isOptionsRequired3 = function () {            return !$scope.employeelist.some(function (options) {                return options.selected;            });        }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.cleararray = function () {
            $scope.pfreport = [];
        }

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("Baldwin");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };


    }


})();