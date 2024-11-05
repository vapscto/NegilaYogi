
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HSEligibilityCerficateController', HSEligibilityCerficateController)

    HSEligibilityCerficateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function HSEligibilityCerficateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {


        $scope.reportpart = false;
        //==============================Load Data.
        $scope.BindData = function () {
            apiService.getDATA("HSEligibilityCerficate/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.eventList = promise.eventList;

                    $scope.classDropdown = promise.classList;

                })
            $scope.cancel();
        };


        //==========================Get Class List
        $scope.get_class = function () {

            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.AMST_Id = "";
            $scope.SPCCME_Id = "";
            $scope.stuDropdown = [];
            $scope.SPCCSH_Age = "";
            $scope.reportpart = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("HSEligibilityCerficate/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classList;

                })
        }


        //==========================Get Section List
        $scope.get_section = function () {

            $scope.ASMS_Id = "";
            $scope.AMST_Id = "";
            $scope.SPCCME_Id = "";
            $scope.stuDropdown = [];
            $scope.SPCCSH_Age = "";
            $scope.reportpart = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("HSEligibilityCerficate/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }


        //==========================Get Student data
        $scope.get_student = function () {

            $scope.AMST_Id = "";
            $scope.SPCCME_Id = "";
            $scope.SPCCSH_Age = "";
            $scope.reportpart = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("HSEligibilityCerficate/get_student", data)
                .then(function (promise) {
                    if (promise.studentList1.length > 0) {
                        $scope.stuDropdown = promise.studentList1;
                    }
                })
        }

        //=====================Age Calculation
        $scope.get_age = function () {

            $scope.SPCCME_Id = "";
            $scope.reportpart = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "AMST_Id": $scope.AMST_Id
            }
            apiService.create("HSEligibilityCerficate/get_age", data)
                .then(function (promise) {
                    //$scope.year = promise.age_tilldate[0].age_Years;
                    //$scope.month = promise.age_tilldate[0].age_Months;
                    //$scope.day = promise.age_tilldate[0].age_Days;

                    $scope.SPCCSH_Age = promise.age_tilldate[0].spccsH_Age;

                })
        };


        //================Form interacted
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //======================== Get Report data
        $scope.submitted = false;
        $scope.selectedStdList = [];
        $scope.get_certificate = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "SPCCME_Id": $scope.SPCCME_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HSEligibilityCerficate/get_PUcertificate", data).
                    then(function (promise) {

                        if (promise.pudatareport.length > 0) {
                            $scope.datareport = promise.pudatareport;
                            $scope.screport = true;
                            $scope.reportpart = true;
                            for (var p = 0; p < $scope.yearlt.length; p++) {
                                if ($scope.yearlt[p].asmaY_Id == $scope.asmaY_Id) {
                                    $scope.academicyear = $scope.yearlt[p].asmaY_Year;
                                }
                            }
                        }
                        else {
                            swal("No Records Found");
                        }
                    })
            }
        };

        //============================For Cancel
        $scope.cancel = function () {
            $scope.asmaY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.AMST_Id = "";
            $scope.SPCCPM_Id = "";
            $scope.Age_Till_Date = "";
            $scope.year = "";
            $scope.month = "";
            $scope.day = "";
            $scope.screport = false;
            $scope.reportpart = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.SPCCSH_Age = "";
            $scope.SPCCME_Id = "";
        }

        //================================================For Print Record
        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EligibilityCert/PuEligibilitypdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        //==========for print
        $scope.Print = function () {

            if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
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
        }

        // end for print


        //================================Export data In excel sheet
        $scope.exportToExcel = function (table) {

            //if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
            // }
            // else {
            //   swal("Please Select Records to be Exported");
            // }
        }
    }

})();