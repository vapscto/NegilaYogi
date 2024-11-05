
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SRKVSHSEligibilityCerficate', SRKVSHSEligibilityCerficate)

    SRKVSHSEligibilityCerficate.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SRKVSHSEligibilityCerficate($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {




        $scope.reportpart = false;
        var ageFormat = "";
        //$scope.AMST_Date = new Date(2022-12-31);
        $scope.AMST_Date = new Date("12-31-2022");
        //==============================Load Data.
        $scope.BindData = function () {
            apiService.getDATA("HSEligibilityCerficate/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.eventList = promise.datareport;
                    $scope.classDropdown = promise.classList;
                    $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                    $scope.stuDropdown = promise.pudatareport;
                })
            //$scope.cancel();
        };
        $scope.onitemchange = function () {
            $scope.datareport = [];
            ageFormat = "";
            $scope.ASMC_SectionName = ""; $scope.ASMCL_ClassName = ""; $scope.SPCCME_Id = "";
            if ($scope.amsT_Id.amsT_Id != null && $scope.amsT_Id.amsT_Id > 0) {
                angular.forEach($scope.stuDropdown, function (stusu) {
                    if (stusu.amsT_Id === $scope.amsT_Id.amsT_Id) {
                        $scope.ASMCL_ClassName = stusu.ASMCL_ClassName;
                        $scope.ASMC_SectionName = stusu.ASMC_SectionName;
                        $scope.reportpart = false;
                        var data = {
                            "ASMAY_Id": $scope.asmaY_Id,
                            "ASMS_Id": stusu.ASMS_Id,
                            "ASMCL_Id": stusu.ASMCL_Id,
                            "AMST_Id": $scope.amsT_Id.amsT_Id,
                            "SPCCME_EventName": "New",
                            "AMST_Date": new Date($scope.AMST_Date).toDateString()

                        }
                        apiService.create("HSEligibilityCerficate/get_age", data)
                            .then(function (promise) {
                                $scope.SPCCSH_Age = promise.age_tilldate[0].spccsH_Age;
                                ageFormat = promise.age_tilldate[0].spccsH_Age_Format;
                            })
                        return;
                    }
                });
            }
        }

        //==========================Get Class List
        $scope.get_class = function () {
            $scope.pudatareport = [];
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
                    $scope.pudatareport = promise.pudatareport;
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
        //================Form interacted
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //======================== Get Report data
        $scope.submitted = false;
        $scope.get_certificate = function () {
            $scope.datareport = [];
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
            var ASMCL_Id = 0;
            var ASMS_Id = 0;
            if ($scope.amsT_Id.amsT_Id != null && $scope.amsT_Id.amsT_Id > 0) {
                angular.forEach($scope.stuDropdown, function (stusu) {
                    if (stusu.amsT_Id === $scope.amsT_Id.amsT_Id) {
                        ASMCL_Id = stusu.ASMCL_Id;
                        ASMS_Id = stusu.ASMS_Id;

                    }
                });
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_Id": $scope.amsT_Id.amsT_Id,
                    "ASMAY_Year": $scope.previusyear,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMS_Id": ASMS_Id,
                    "SPCCME_EventName": "New",
                    "SPCCME_Id": $scope.SPCCME_Id
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
                            for (var q = 0; q < $scope.datareport.length; q++) {
                                var myarr = $scope.datareport[q].JoinYear.split("-");
                                $scope.datareport[q].JoinYear = myarr[0];
                            }
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
                            var myArray = ageFormat.split("-");
                            $scope.age = myArray[0];
                            $scope.month = myArray[1];
                            $scope.Days = myArray[2];
                            //$scope.age = $scope.prevYear - $scope.dobYear;

                            //$scope.month = $scope.monthY - $scope.dobMonth;
                            //$scope.Days = $scope.dayY - $scope.dobDay;
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
            $state.reload();

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