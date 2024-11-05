
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VikasaFinalClasswisecumulativeController', VikasaFinalClasswisecumulativeController)

    VikasaFinalClasswisecumulativeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel','$timeout']
    function VikasaFinalClasswisecumulativeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("VikasaFinalClasswisecumulative/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.gradeList = promise.gradeList;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";
                });
        };

        $scope.get_class = function () {
            $scope.asmS_Id = "";
            $scope.amsT_Id = "";
            $scope.emE_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("VikasaSchoolTermWiseSubjectCumulativeReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;

                });
        };

        $scope.get_section = function () {
            $scope.amsT_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.studentDropdown = "";
            $scope.exsplt = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("VikasaAssessment2Report/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;
                });
        };

        $scope.get_category = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id
            };

            apiService.create("VikasaFinalClasswisecumulative/get_category", data).then(function (promise) {
                if (promise !== null) {
                    $scope.categoryList = promise.categoryList;
                    if ($scope.categoryList.length > 0) {
                        $scope.categoryList = promise.categoryList;
                    } else {
                        swal("No Records Found");
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.get_subject_group = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "EMCA_Id": $scope.emcA_Id
            };
            apiService.create("VikasaFinalClasswisecumulative/get_subject_group", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subject_group = promise.subject_group;

                    if ($scope.subject_group.length > 0) {
                        $scope.subject_group = promise.subject_group;

                    } else {
                        swal("No Records Found");
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // TO Show The Data
        $scope.submitted = false;

        $scope.showdetails = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "EMCA_Id": $scope.emcA_Id,
                    "EMG_Id": $scope.emG_Id,
                    "EMGR_Id": $scope.emgR_Id
                };

                apiService.create("VikasaFinalClasswisecumulative/showdetails", data).
                    then(function (promise) {

                        if (promise.getreport.length > 0) {
                            $scope.screport = true;
                            $scope.Cumureport = true;
                            $scope.getreport = promise.getreport;
                            $scope.getreporthead = promise.getreporthead;

                            console.log($scope.getreport);
                            console.log($scope.getreporthead);

                            angular.forEach($scope.yearlt, function (yr) {
                                if (yr.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                    $scope.asmaY_Year = yr.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.classDropdown, function (cl) {
                                if (cl.asmcL_Id === parseInt($scope.asmcL_Id)) {
                                    $scope.asmcL_ClassName = cl.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.sectionDropdown, function (sc) {
                                if (sc.asmS_Id === parseInt($scope.asmS_Id)) {
                                    $scope.asmC_SectionName = sc.asmC_SectionName;
                                }
                            });

                            //angular.forEach($scope.subjectDropdown, function (sub) {
                            //    if (sub.ismS_Id == $scope.ismS_Id) {
                            //        $scope.ismS_SubjectName = sub.subjectName;
                            //    }
                            //});


                            $scope.classteacher = promise.classteacher;
                            $scope.classteacher = $scope.classteacher[0].empname;
                            $scope.instname = promise.instname[0].mI_Name;
                            $scope.instnametwon = promise.instname[0].ivrmmcT_Name;
                            $scope.getsubjectavg = promise.getsubjectavg;

                            var total = 0;
                            var totalavg = 0;

                            angular.forEach($scope.getsubjectavg, function (dd) {
                                total = total + dd.estmppS_SectionAverage;
                            });

                            total = total.toFixed(2);

                            totalavg = total / $scope.getsubjectavg.length;
                            totalavg = totalavg.toFixed(2);
                            $scope.totalavg1 = totalavg;
                            $scope.getsubjectavg.push({ ismS_Id: 0, estmppS_SectionAverage: total });
                            $scope.getsubjectavg.push({ ismS_Id: 1000, estmppS_SectionAverage: totalavg });

                            console.log($scope.getsubjectavg);

                        } else {
                            swal('No record Found');
                            $scope.Cumureport = false;
                        }
                    });
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

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

        };
        // end for print

        $scope.exportToExcel = function (tableIds) {
            var excelname = "Final Class Wise Cumulative Report " + "Year : " + $scope.asmaY_Year + " Class-Section : " + $scope.asmcL_ClassName + "-" + $scope.asmC_SectionName + ".xls";
            var exportHref = Excel.tableToExcel(tableIds, 'Final Class Wise Cumulative Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
    }

})();