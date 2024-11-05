(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlertReportController', AlertReportController);

    AlertReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache'];
    function AlertReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.SalaryFromDay = "";
        $scope.SalaryToDay = "";
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("AlertReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}


                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);

                }


                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                //if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                //    $scope.designationdropdown = promise.designationdropdown;

                //    $scope.designationselectedAll = true;
                //    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);


                //}


            });
        };


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.institutionDetails = {};
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.employeeDetails = {};
                var data = {};

                if ($scope.report === null || $scope.report === undefined) {
                    swal('Select Report Type');
                    return;
                }

                data = { "Type": $scope.report};

                apiService.create("AlertReport/getAlertReport", data).
                    then(function (promise) {
                        if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                            $scope.EmployeeDis = true;
                            $scope.employeedetailList = promise.employeedetailList;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                    });
            }

        };

        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

        $scope.onChange = function () {
            $scope.EmployeeDis = false;
        };

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("PFChallan").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("BankCash");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };

    }

})();