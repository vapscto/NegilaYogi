(function () {
    'use strict';
    angular
        .module('app')
        .controller('PFYearlyReportStJamesController', PFYearlyReportStJamesController)

    PFYearlyReportStJamesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', 'Excel']
    function PFYearlyReportStJamesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, Excel) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.EmployeePFreportDetails = [];




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


        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            $scope.albumNameArray1 = [];
            angular.forEach($scope.employeelist, function (role) {
                if (role.emple) $scope.albumNameArray1.push(role);
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
                    "Flag": "EmployeePFreport",
                    "Procedure": "HR_PF_YearlyReport"

                };
                apiService.create("PFTransaction/getReport", data).then(function (promise) {
                    if (promise.employeeDetails != null && promise.employeeDetails.length > 0) {
                        $scope.employeeDetails = promise.employeeDetails;

                        $scope.HRME_EmployeeFirstName = $scope.employeeDetails[0].hrmE_EmployeeFirstName;
                        $scope.MI_Name = $scope.employeeDetails[0].mI_Name;
                        $scope.HRME_PFAccNo = $scope.employeeDetails[0].hrmE_PFAccNo;
                        $scope.HRME_EmployeeCode = $scope.employeeDetails[0].hrmE_EmployeeCode;

                        $scope.EmployeePFreportDetails = promise.employeePFreportDetails;



                    }
                    else {
                        swal("Record Not Found !")
                    }
                });
            }
        };



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

        $scope.isOptionsRequired3 = function () {

            return !$scope.employeelist.some(function (options) {
                return options.emple;
            });
        }

        $scope.addColumn4 = function () {
            $scope.empl = $scope.employeelist.every(function (itm) { return itm.emple; });
        };

        $scope.all_check_empl = function (empl) {

            var toggleStatus4 = empl;
            angular.forEach($scope.employeelist, function (itm) {
                itm.emple = toggleStatus4;
            });
        }




        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                // '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdfnew.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        //$scope.exportToExcel = function (tableId) {

        //    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);

        //}

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("Baldwin");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };


    }
})();