
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
                    //$scope.year = promise.age_tilldate[0].age_Years ;
                    //$scope.month = promise.age_tilldate[0].age_Months ;
                    //$scope.day = promise.age_tilldate[0].age_Days ;

                    $scope.SPCCSH_Age = promise.age_tilldate[0].spccsH_Age;
                })
        };

        //================Form interacted
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //======================== Get Report data
        $scope.submitted = false;
        $scope.get_certificate = function () {

            $scope.asmayyear = "";

            $scope.academicyear = $scope.yearlt

            $scope.yeardata = $scope.academicyear;
            angular.forEach($scope.yeardata, function (tt) {

                if (tt.asmaY_Id == $scope.asmaY_Id) {
                    $scope.asmayyear = tt.asmaY_Year;
                }
            })

            $scope.Ayear = $scope.asmayyear.substring(0, 4);
            $scope.year2 = $scope.asmayyear.substring(5, 9);
            $scope.AcYeNo = parseInt($scope.Ayear);
            $scope.year21 = parseInt($scope.year2);


            $scope.prevYear = $scope.AcYeNo - 1;
            $scope.year212 = $scope.year21 - 1;

            $scope.previusyear = ($scope.prevYear + '-' + $scope.year212).toString();


            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "SPCCME_Id": $scope.SPCCME_Id,
                    "ASMAY_Year": $scope.previusyear,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HSEligibilityCerficate/get_certificate", data).
                    then(function (promise) {
                        $scope.date123 = new Date();
                        if (promise.datareport.length > 0) {
                            $scope.datareport = promise.datareport;
                            $scope.screport = true;
                            $scope.reportpart = true;
                            for (var p = 0; p < $scope.yearlt.length; p++) {
                                if ($scope.yearlt[p].asmaY_Id == $scope.asmaY_Id) {
                                    $scope.academicyear = $scope.yearlt[p].asmaY_Year;
                                }
                            }

                            $scope.clsslst = promise.clsslst;
                            $scope.prvclass = promise.clsslst[0].prevclsname;

                            var doob = promise.datareport[0].amsT_DOB;

                            var doobyr = doob.substring(0, 4);
                            var doobmnth = doob.substring(5, 7);
                            var doobdays = doob.substring(8, 10);

                            $scope.b1 = doobdays.substring(0, 1);
                            $scope.b2 = doobdays.substring(1, 2);
                            $scope.BB1 = $scope.b1 + $scope.b2;

                            $scope.b3 = doobmnth.substring(0, 1);
                            $scope.b4 = doobmnth.substring(1, 2);
                            $scope.BB2 = $scope.b3 + $scope.b4;

                            $scope.b5 = doobyr.substring(0, 1);
                            $scope.b6 = doobyr.substring(1, 2);
                            $scope.b7 = doobyr.substring(2, 3);
                            $scope.b8 = doobyr.substring(3, 4);
                            $scope.BB3 = $scope.b5 + $scope.b6 + $scope.b7 + $scope.b8;

                            var yeardata = $scope.academicyear;
                            $scope.Ayear = yeardata.substring(0, 4);
                            $scope.year2 = yeardata.substring(5, 9);
                            $scope.AcYeNo = parseInt($scope.Ayear);
                            $scope.year21 = parseInt($scope.year2);


                            $scope.prevYear = $scope.AcYeNo - 1;
                            $scope.year212 = $scope.year21 - 1;



                            $scope.monthY = 12;
                            $scope.dayY = 31;
                            $scope.curYeardt = $scope.prevYear + '-' + $scope.monthY + '-' + $scope.dayY;

                            $scope.lastyear = $scope.prevYear + '-' + $scope.year212;

                            $scope.dobYear = parseInt(doobyr);

                            $scope.dobMonth = parseInt(doobmnth);
                            $scope.dobDay = parseInt(doobdays);

                            $scope.age = $scope.prevYear - $scope.dobYear;
                            $scope.month = $scope.monthY - $scope.dobMonth;
                            $scope.Days = $scope.dayY - $scope.dobDay;


                            if ($scope.monthY < $scope.dobMonth || ($scope.monthY == $scope.dobMonth && $scope.dayY < $scope.dobDay)) {
                                age--;
                            }
                            //  alert(age + '-' + month + '-' + Days);
                            console.log($scop.age + '-' + $scope.month + '-' + $scope.Days);
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EligibilityCert/EligibilitycertPdf.css" />' +
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