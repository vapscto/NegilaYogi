(function () {
    'use strict';
    angular.module('app').controller('JSHSCumulativeReportController', JSHSCumulativeReportController)
    JSHSCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'Excel', '$timeout']
    function JSHSCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, Excel, $timeout) {

        $scope.JSHSReport = false;
        $scope.getexamlist = [];


        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }

        $scope.coptyright = copty;

        var logopath = "";

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.getexamlist = [];
            $scope.EMGR_Id = "";
            $scope.ASMS_Id = "";
            $scope.ASMCL_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.JSHSReport = false;
            $scope.getexamlist = [];
            $scope.ASMS_Id = "";
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        //-----------section Selection
        $scope.onsectionchange = function () {
            $scope.JSHSReport = false;
            $scope.getexamlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_exam", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getexamlist = promise.getexam;
                    $scope.getsubjectlist = promise.getsubjects;
                    $scope.getgradelist = promise.getgradelist;

                    if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                        $scope.examlist = $scope.getexamlist;
                    } else {
                        swal("No Exam Found");
                        return;
                    }

                    if ($scope.getsubjectlist !== null && $scope.getsubjectlist.length > 0) {
                        $scope.subjectlist = $scope.getsubjectlist;
                    } else {
                        swal("No Subjects Found");
                        return;
                    }
                    if ($scope.getgradelist !== null && $scope.getgradelist.length > 0) {
                        $scope.gradedetails = $scope.getgradelist;
                    } else {
                        swal("No Grade Found");
                    }
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.examlist.some(function (options) {
                return options.EME_Id;
            });
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.JSHSReport = false;
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                angular.forEach($scope.examlist, function (term) {
                    if (term.EME_Id === true) {
                        $scope.termlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "examlist": $scope.termlisttemp
                };
                apiService.create("JSHSExamReports/get_cumulativereportdetails", data).then(function (promise) {
                    if (promise !== null) {

                        if (promise.getexamsubjectwisereport !== null && promise.getexamsubjectwisereport.length > 0) {
                            $scope.JSHSReport = true;

                            $scope.studentlist = [];
                            $scope.examwisesubsubject = [];


                            $scope.getexamsubjectwisereport = promise.getexamsubjectwisereport;

                            $scope.selectedexamlist = promise.getselectedexamlist;

                            $scope.getexamwisesubsubjectlist = promise.getexamwisesubsubjectlist;

                            angular.forEach($scope.getsubjectlist, function (subj) {
                                if (subj.ismS_Id === parseInt($scope.ISMS_Id)) {
                                    $scope.subjectname = subj.ismS_SubjectName;
                                }
                            });

                            angular.forEach($scope.year_list, function (yr) {
                                if (yr.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yearname = yr.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.class_list, function (cl) {
                                if (cl.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                    $scope.classname = cl.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.section_list, function (sec) {
                                if (sec.asmS_Id === parseInt($scope.ASMS_Id)) {
                                    $scope.sectionname = sec.asmC_SectionName;
                                }
                            });

                            $scope.getgradedetails = promise.getgradedetails;


                            // STUDENT LIST
                            angular.forEach($scope.getexamsubjectwisereport, function (student) {

                                if ($scope.studentlist.length === 0) {
                                    $scope.studentlist.push({
                                        AMST_Id: student.AMST_Id, studentname: student.STUDENTNAME, rollno: student.ROLLNO, admno: student.ADMNO
                                    });
                                } else if ($scope.studentlist.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach($scope.studentlist, function (dd) {
                                        if (dd.AMST_Id === student.AMST_Id) {
                                            already_cnt += 1;
                                        }
                                    });
                                    if (already_cnt === 0) {
                                        $scope.studentlist.push({
                                            AMST_Id: student.AMST_Id, studentname: student.STUDENTNAME, rollno: student.ROLLNO, admno: student.ADMNO
                                        });
                                    }
                                }
                            });

                            // EXAM WISE SUBSUBJECT LIST
                            angular.forEach($scope.selectedexamlist, function (exam) {
                                $scope.subsubjectlist = [];
                                angular.forEach($scope.getexamwisesubsubjectlist, function (sub) {
                                    if (sub.emE_Id === exam.emE_Id) {
                                        $scope.subsubjectlist.push({ EMSS_Id: sub.emsS_Id, EMSS_SubSubjectName: sub.emsS_SubSubjectName, EME_Id: sub.emE_Id });
                                    }
                                });

                                if ($scope.subsubjectlist.length === 0) {
                                    $scope.subsubjectlist.push({ EMSS_Id: sub.emsS_Id, EMSS_SubSubjectName: sub.emsS_SubSubjectName, EME_Id: exam.emE_Id });
                                }
                                exam.subsubjectexamlist = $scope.subsubjectlist;
                            });

                            console.log($scope.selectedexamlist);


                            // SUBSUBJECT LIST
                            $scope.subsubjectlistnew = [];
                            var examcount = 0;
                            var maxsubjectmarkssum = 0;

                            angular.forEach($scope.selectedexamlist, function (exam) {
                                examcount = 0;
                                maxsubjectmarkssum = 0;
                                angular.forEach($scope.getexamwisesubsubjectlist, function (sub) {
                                    if (sub.emE_Id === exam.emE_Id) {
                                        examcount += 1;
                                        maxsubjectmarkssum += sub.eycessS_MaxMarks;
                                        $scope.subsubjectlistnew.push({
                                            EMSS_Id: sub.emsS_Id, EMSS_SubSubjectName: sub.emsS_SubSubjectName,
                                            EME_Id: sub.emE_Id, newcolspan: 2
                                        });
                                    }
                                });
                                if (examcount > 1) {
                                    $scope.subsubjectlistnew.push({
                                        EMSS_Id: 5000, EMSS_SubSubjectName: "Total", EME_Id: exam.emE_Id, newcolspan: 1,
                                        maxsubjectmarks: maxsubjectmarkssum
                                    });
                                }
                            });

                            console.log($scope.subsubjectlistnew);

                            // Subsubjectwise grademarks list 

                            $scope.subsubjectgrademarkslist = [];
                            var maxtotalmarks = 0;
                            angular.forEach($scope.subsubjectlistnew, function (exam) {
                                angular.forEach($scope.getexamwisesubsubjectlist, function (sub) {
                                    if (sub.emE_Id === exam.EME_Id && exam.EMSS_Id === sub.emsS_Id) {
                                        maxtotalmarks += sub.eycessS_MaxMarks;
                                        $scope.subsubjectgrademarkslist.push({
                                            EMSS_Id: sub.emsS_Id, EMSS_SubSubjectName: sub.emsS_SubSubjectName,
                                            EME_Id: sub.emE_Id, marksorgrade: sub.eycessS_MaxMarks
                                        });
                                        $scope.subsubjectgrademarkslist.push({
                                            EMSS_Id: sub.emsS_Id, EMSS_SubSubjectName: sub.emsS_SubSubjectName,
                                            EME_Id: sub.emE_Id, marksorgrade: sub.eycessS_Grade
                                        });
                                    }

                                    if (exam.EMSS_SubSubjectName === "Total") {

                                        var counttotal = 0;
                                        angular.forEach($scope.subsubjectgrademarkslist, function (dd) {

                                            if (dd.EMSS_SubSubjectName === "Total" && exam.EME_Id === dd.EME_Id) {
                                                counttotal += 1;
                                            }
                                        });
                                        if (counttotal === 0) {
                                            $scope.subsubjectgrademarkslist.push({
                                                EMSS_Id: 5000, EMSS_SubSubjectName: "Total",
                                                EME_Id: exam.EME_Id, marksorgrade: exam.maxsubjectmarks
                                            });
                                        }
                                    }
                                });
                            });


                            // OVER ALL TOTAL

                            $scope.subsubjectgrademarkslist.push({
                                EMSS_Id: 0, EMSS_SubSubjectName: "MAXTOTAL",
                                EME_Id: 0, marksorgrade: maxtotalmarks
                            });
                            $scope.subsubjectgrademarkslist.push({
                                EMSS_Id: 0, EMSS_SubSubjectName: "MAXTOTAL",
                                EME_Id: 0, marksorgrade: "G"
                            });


                            $scope.totalmaxmarks = maxtotalmarks;

                            console.log($scope.subsubjectgrademarkslist);

                            var countcol = 0;
                            angular.forEach($scope.selectedexamlist, function (dd) {
                                countcol = 0;
                                angular.forEach($scope.subsubjectgrademarkslist, function (ddd) {
                                    if (ddd.EME_Id === dd.emE_Id) {
                                        countcol += 1;
                                    }
                                });
                                dd.colspan = countcol;
                            });

                            console.log($scope.selectedexamlist);


                            // STUDENT WISE MARKS LIST
                            $scope.studentmarkslist = [];
                            angular.forEach($scope.studentlist, function (student) {
                                $scope.studentmarkslist = [];
                                angular.forEach($scope.getexamsubjectwisereport, function (marks) {
                                    if (student.AMST_Id === marks.AMST_Id) {
                                        $scope.studentmarkslist.push(marks);
                                    }
                                });
                                student.studentmarks = $scope.studentmarkslist;
                            });

                            console.log($scope.studentlist);

                            var subjecttotal = 0;

                            angular.forEach($scope.studentlist, function (stu) {
                                subjecttotal = 0;
                                angular.forEach($scope.selectedexamlist, function (examlist) {
                                    subjecttotal = 0;
                                    angular.forEach(stu.studentmarks, function (submarks) {
                                        if (examlist.emE_Id === submarks.EME_Id) {
                                            angular.forEach($scope.subsubjectgrademarkslist, function (dd) {
                                                if (dd.EMSS_SubSubjectName === "Total" && dd.EMSS_Id === 5000 && dd.EME_Id === examlist.emE_Id) {
                                                    subjecttotal += submarks.SUBSUBJECTMARKS;
                                                }
                                            });
                                        }
                                    });
                                    stu.studentmarks.push({ EME_Id: examlist.emE_Id, EMSS_Id: 5000, SUBSUBJECTMARKS: subjecttotal });
                                });
                            });   

                            $scope.getgradereport = promise.getgradereport;


                            $scope.getinstitution = promise.getinstitution;

                            $scope.instname = $scope.getinstitution[0].mI_Name;
                            $scope.address1 = $scope.getinstitution[0].mI_Address1;
                            $scope.address2 = $scope.getinstitution[0].mI_Address2;
                            $scope.address3 = $scope.getinstitution[0].mI_Address3;
                            $scope.Pincode = $scope.getinstitution[0].mI_Pincode;

                        } else {
                            $scope.JSHSReport = false;
                            swal("No Records Found");
                        }
                    }
                });

            } else {
                $scope.submitted = true;
            }
        };

        //to print
        $scope.print_HHS02 = function () {
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

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };


        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
    }
})();