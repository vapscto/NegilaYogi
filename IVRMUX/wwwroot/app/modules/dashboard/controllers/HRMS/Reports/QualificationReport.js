(function () {
    'use strict';
    angular
.module('app')
        .controller('QualificationReport', QualificationReport)
    QualificationReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function QualificationReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {       
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SalaryFromDay = "";
        $scope.SalaryToDay = "";
        // Get form Details at onload 
        $scope.onLoadGetData = function () {           
            var pageid = 2;
            apiService.getURI("QualificationReport/getalldetails", pageid).then(function (promise) {               
                if (promise.qualificationlist !== null && promise.qualificationlist.length > 0) {
                    $scope.qnamedropdown = promise.qualificationlist;
                }
                if (promise.castlist !== null && promise.castlist.length > 0) {
                    $scope.castedropdown = promise.castlist;
                }               
            })
        }
        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;      
        $scope.submitted = false;
        $scope.SearchEmployee = function () {           
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.employeeDetails = {};                
                if ($scope.Employee.report === null || $scope.Employee.report === undefined) {
                    swal('Select Report Type');
                    return;
                }               
                var data = {
                    "HRME_QualificationName": $scope.Employee.hrmE_QualificationName,
                    "IMC_CasteName": $scope.Employee.imC_CasteName,
                    "Type": $scope.Employee.report
                };
                apiService.create("QualificationReport/getQualificationReport", data).
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
        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.submitted = false;
            $scope.Employee.report = '';
            $scope.Employee.hrmE_QualificationName = '';
            $scope.Employee.imC_CasteName = '';            
            $scope.employeedetailList=[];           
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }
        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
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