(function () {
    'use strict';
    angular.module('app').controller('MarksEntryReportController', MarksEntryReportController)
    MarksEntryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel','$timeout']
    function MarksEntryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !==null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("MarksEntryReport/Getdetails", pageid).then(function (promise) {
                $scope.yearlt = promise.getyear;
            });
        };

        $scope.get_class = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.sectionDropdown = [];
            $scope.classDropdown = [];
            $scope.exsplt = [];
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("MarksEntryReport/get_class", data).then(function (promise) {
                $scope.classDropdown = promise.getclass;
            });
        };

        $scope.get_section = function () {
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = [];
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("MarksEntryReport/get_section", data).then(function (promise) {
                $scope.sectionDropdown = promise.getsection;
            });
        };

        $scope.get_Exam = function () {
            $scope.emE_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("MarksEntryReport/get_exam", data).then(function (promise) {
                $scope.exsplt = promise.getexam;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };




        // TO Save The Data
        $scope.submitted = false;
        $scope.get_report = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.report = false;
                var data = {
                    "EME_ID": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                };


                apiService.create("MarksEntryReport/get_report", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getreport = promise.getreport;

                        if ($scope.getreport !== null && $scope.getreport.length > 0) {
                            $scope.subjectlist = [];
                            $scope.report = true;
                            angular.forEach($scope.getreport, function (sub) {
                                if ($scope.subjectlist.length === 0) {
                                    $scope.subjectlist.push({ ISMS_Id: sub.ISMS_Id, ISMS_SubjectName: sub.ISMS_SubjectName });
                                } else if ($scope.subjectlist.length > 0) {
                                    var count = 0;
                                    angular.forEach($scope.subjectlist, function (dd) {
                                        if (dd.ISMS_Id === sub.ISMS_Id)
                                            count += 1;
                                    });
                                    if (count === 0) {
                                        $scope.subjectlist.push({ ISMS_Id: sub.ISMS_Id, ISMS_SubjectName: sub.ISMS_SubjectName });
                                    }
                                }
                            });


                            $scope.sectionlist = [];

                            angular.forEach($scope.getreport, function (sec) {
                                if ($scope.sectionlist.length === 0) {
                                    $scope.sectionlist.push({
                                        ASMS_Id: sec.ASMS_Id, ASMC_SectionName: sec.ASMC_SectionName, ASMCL_Id: sec.ASMCL_Id,
                                        ASMCL_ClassName: sec.ASMCL_ClassName
                                    });
                                } else if ($scope.sectionlist.length > 0) {
                                    var countsec = 0;
                                    angular.forEach($scope.sectionlist, function (dd) {
                                        if (dd.ASMS_Id === sec.ASMS_Id)
                                            countsec += 1;
                                    });
                                    if (countsec === 0) {
                                        $scope.sectionlist.push({
                                            ASMS_Id: sec.ASMS_Id, ASMC_SectionName: sec.ASMC_SectionName,
                                            ASMCL_ClassName: sec.ASMCL_ClassName
                                        });
                                    }
                                }
                            });
                            console.log($scope.subjectlist);
                            console.log($scope.sectionlist);

                            angular.forEach($scope.sectionlist, function (sec) {
                                $scope.tempsubject = [];
                                angular.forEach($scope.subjectlist, function (sub) {
                                    angular.forEach($scope.getreport, function (rep) {
                                        if (sec.ASMS_Id === rep.ASMS_Id && sub.ISMS_Id === rep.ISMS_Id) {
                                            $scope.tempsubject.push({
                                                ISMS_Id: rep.ISMS_Id, ISMS_SubjectName: rep.ISMS_SubjectName,
                                                SourceMarksColorFlag: rep.SourceMarksColorFlag, SourceMarksEntryFlag: rep.SourceMarksEntryFlag,
                                                SubjectOrder: rep.SubjectOrder, Employename: rep.Employename, SourceMarksEntry: rep.SourceMarksEntry
                                            });
                                        }
                                    });

                                });
                                sec.subjecttemp = $scope.tempsubject;
                            });
                            console.log("TEMP");
                            console.log($scope.sectionlist);

                            angular.forEach($scope.yearlt, function (y) {
                                if (y.asmaY_Id === parseInt($scope.asmaY_Id)){
                                    $scope.yearname = y.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.classDropdown, function (y) {
                                if (y.asmcL_Id === parseInt($scope.asmcL_Id)) {
                                    $scope.classname = y.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.sectionDropdown, function (y) {
                                if (y.asmS_Id === parseInt($scope.asmS_Id)) {
                                    $scope.sectioname = y.asmC_SectionName;
                                }
                            });

                            if (parseInt($scope.asmS_Id)===0) {
                                $scope.sectioname = "All";
                            }

                            angular.forEach($scope.exsplt, function (y) {
                                if (y.emE_Id === parseInt($scope.emE_Id)) {
                                    $scope.examname = y.emE_ExamName;
                                }
                            });

                        } else {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
            }

            console.log($scope.studentslt);
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "MARKS ENTERED REPORT.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }

})();