(function () {
    'use strict';
    angular
        .module('app')
        .controller('PS8_ReportStjamesController', PS8_ReportStjamesController)

    PS8_ReportStjamesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PS8_ReportStjamesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object




        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PS7andPS8FormReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }



                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

            })
        }


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRES_Year": $scope.hreS_Year,
                }
                apiService.create("PS7andPS8FormReport/getdataps8", data).then(function (promise) {

                    if (promise.pfreport.length > 0 && promise.pfreport != null) {
                        //$scope.employeeDetails = promise.employeeDetails;

                        //$scope.hrmE_EmployeeFirstName = $scope.employeeDetails[0].hrmE_EmployeeFirstName;
                        //$scope.hrmE_FatherName = $scope.employeeDetails[0].hrmE_FatherName;
                        //$scope.mI_Name = $scope.employeeDetails[0].mI_Name;
                        //$scope.mI_Address1 = $scope.employeeDetails[0].mI_Address1;

                        $scope.nextyear = $scope.hreS_Year;
                        $scope.nextyear++;

                        $scope.pfreport = promise.pfreport;
                        $scope.Totalamountofwages = 0;
                        $scope.Totalpensionamount = 0;
                        angular.forEach($scope.pfreport, function (itm) {
                            $scope.Totalamountofwages = $scope.Totalamountofwages + itm.sumcondition;
                            $scope.Totalpensionamount = $scope.Totalpensionamount + itm.pensionamout;
                        });
                    }

                })
            }

        }



        $scope.printToCart = function (Stjamesform8) {
            var innerContents = document.getElementById("Stjamesform8").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
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