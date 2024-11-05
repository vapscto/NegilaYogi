
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VikasaSubjectwiseCumulativeReportController', VikasaSubjectwiseCumulativeReportController)

    VikasaSubjectwiseCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function VikasaSubjectwiseCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {



        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("VikasaSubjectwiseCumulativeReport/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";
                    $scope.gradelist = promise.gradeList;

                })
        };

        $scope.get_class = function () {
            $scope.asmS_Id = "";
            $scope.amsT_Id = "";
            $scope.emE_Id = "";
            $scope.getreport = [];
            $scope.getreporthead = [];
            $scope.Cumureport = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("VikasaSubjectwiseCumulativeReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;

                })
        }
        $scope.get_section = function () {

            $scope.amsT_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.studentDropdown = "";
            $scope.exsplt = "";
            $scope.getreport = [];
            $scope.getreporthead = [];
            $scope.Cumureport = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("VikasaSubjectwiseCumulativeReport/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        $scope.get_category = function () {

            $scope.amsT_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            $scope.getreport = [];
            $scope.getreporthead = [];
            $scope.Cumureport = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id

            }
            apiService.create("VikasaSubjectwiseCumulativeReport/get_category", data)
                .then(function (promise) {
                    $scope.categoryDropdown = promise.categoryList;


                })

        }

        $scope.get_subject = function (asmS_Id) {

            $scope.amsT_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            $scope.getreport = [];
            $scope.getreporthead = [];
            $scope.Cumureport = false;
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "EMCA_Id": $scope.emcA_Id,
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("VikasaSubjectwiseCumulativeReport/get_subject", data)
                .then(function (promise) {
                    $scope.subjectDropdown = promise.subjectList;
                })

        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        // TO Show The Data
        $scope.submitted = false;

        $scope.showdetails = function () {
            $scope.submitted = true;
            $scope.getreport = [];
            $scope.getreporthead = [];
            $scope.Cumureport = false;

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "EMCA_Id": $scope.emcA_Id,
                    "EMGR_Id": $scope.emgR_Id,
                    "Type": $scope.entry
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("VikasaSubjectwiseCumulativeReport/showdetails", data).
                    then(function (promise) {

                        if (promise.getreport.length > 0) {
                            $scope.screport = true;
                            $scope.Cumureport = true;
                            $scope.getreport = promise.getreport;
                            $scope.getreporthead = promise.getreporthead;

                            angular.forEach($scope.yearlt, function (yr) {
                                if (yr.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.asmaY_Year = yr.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.classDropdown, function (cl) {
                                if (cl.asmcL_Id == $scope.asmcL_Id) {
                                    $scope.asmcL_ClassName = cl.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.sectionDropdown, function (sc) {
                                if (sc.asmS_Id == $scope.asmS_Id) {
                                    $scope.asmC_SectionName = sc.asmC_SectionName;
                                }
                            });

                            angular.forEach($scope.subjectDropdown, function (sub) {
                                if (sub.ismS_Id == $scope.ismS_Id) {
                                    $scope.ismS_SubjectName = sub.subjectName;
                                }
                            });


                            $scope.classteacher = promise.classteacher;
                            $scope.classteacher = $scope.classteacher[0].empname;
                            $scope.instname = promise.instname[0].mI_Name;
                            $scope.instnametwon = promise.instname[0].ivrmmcT_Name;

                        } else {
                            swal('No record Found');
                            $scope.Cumureport = false;
                        }
                    });
            }
        };

        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }

        //for print
        $scope.printData = function () {


            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }
        // end for print


        $scope.exportToExcel = function (tableIds) {
            var excelname = "Cumulative Report Subject " + $scope.ismS_SubjectName + "Year : " + $scope.asmaY_Year + " Class-Section : " + $scope.asmcL_ClassName + "-" + $scope.asmC_SectionName + ".xls";
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);


            //var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            //$timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.radiobtn = function () {
            $scope.getreport = [];
            $scope.getreporthead = [];
            $scope.Cumureport = false;
        }
    }

})();