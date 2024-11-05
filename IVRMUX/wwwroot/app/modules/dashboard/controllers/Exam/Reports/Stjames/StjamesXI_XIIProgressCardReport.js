(function () {
    'use strict';
    angular.module('app').controller('XIXIIprogresscardstjamesController', XIXIIprogresscardstjamesController)
    XIXIIprogresscardstjamesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window', 'Excel', '$timeout']
    function XIXIIprogresscardstjamesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window, Excel, $timeout) {

        $scope.asmaY_Year = "2019-2020";
        $scope.JSHSReport = false;
        $scope.Displayattendance = true;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.getexamlist = [];
        $scope.obj = {};
        $scope.generateddate = new Date();
        $scope.reporttype = 'indi';


        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.class_list = [];
            $scope.ASMCL_Id = "";
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
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
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];
            $scope.getgrouplist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "reporttype": $scope.reporttype
            };

            apiService.create("JSHSExamReports/get_Exam_group", data).then(function (promise) {
                if ($scope.reporttype === "groupwise") {
                    $scope.getgrouplist = promise.getgroupdetails;
                } else if ($scope.reporttype === "indi") {
                    $scope.getexamlist = promise.getexam;
                    angular.forEach($scope.getexamlist, function (term) {
                        term.EME_Id = true;                        
                    });
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };

        $scope.isOptionsRequired2 = function () {
            return !$scope.getgrouplist.some(function (options) {
                return options.EMPS_Id;
            });
        };

        $scope.onclickdates = function () {
            $scope.ASMS_Id = "";
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {

            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                var data = "";
                if ($scope.reporttype === "indi") {
                    angular.forEach($scope.getexamlist, function (term) {
                        if (term.EME_Id === true) {
                            $scope.termlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                        }
                    });

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "reporttype": $scope.reporttype,
                        "examlist": $scope.termlisttemp
                    };
                } else if ($scope.reporttype === "groupwise") {
                    angular.forEach($scope.getexamlist, function (term) {
                        if (term.EMPS_Id === true) {
                            $scope.termlisttemp.push({ EMPS_Id: term.empS_Id, EMPG_GroupName: term.empG_GroupName });
                        }
                    });

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "reporttype": $scope.reporttype,
                        "temp_groupDTO": $scope.termlisttemp
                    };
                } else if ($scope.reporttype === "promotion") {
                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "reporttype": $scope.reporttype
                    };
                }

                apiService.create("JSHSExamReports/stjamesexamreport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                            $scope.JSHSReport = true;
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlistnew;
                            console.log($scope.getstudentwisesubjectlist);
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getexamdetails = promise.getexamdetails;
                            $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                            $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;

                            $scope.tempexamdetails = [];

                            if ($scope.reporttype === "indi") {
                                angular.forEach($scope.getexamdetails, function (dd) {
                                    $scope.tempexamdetails.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id, columname: "Marks", emE_ExamCode: dd.emE_ExamCode});
                                    $scope.tempexamdetails.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id, columname: "Point", emE_ExamCode: dd.emE_ExamCode });
                                });

                                //$scope.getexamsubjectwisemarksdetails = promise.getexamsubjectwisemarksdetails;
                                

                                $scope.subjectdetails = [];

                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.subjectdetails = [];
                                    angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                        if (stu.AMST_Id === parseInt(sub.AMST_Id)) {
                                            $scope.subjectdetails.push({
                                                ISMS_SubjectName: sub.subjectname, ISMS_Id: sub.subid, subid: sub.subid,
                                                EYCES_AplResultFlg: sub.EYCES_AplResultFlg, subjectorder: sub.subjectorder
                                            });
                                        }
                                    });
                                    stu.subjects = $scope.subjectdetails;
                                });

                                $scope.marksdetails = [];
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.marksdetails = [];
                                    angular.forEach($scope.getstudentmarksdetails, function (subd) {
                                        if (stu.AMST_Id === parseInt(subd.AMST_Id)) {
                                            $scope.marksdetails.push(subd);
                                        }
                                    });
                                    stu.marks = $scope.marksdetails;
                                });


                                // Over All Total                           
                                $scope.totalgrade = [];
                                angular.forEach($scope.getstudentdetails, function (st) {
                                    $scope.totalgrade = [];
                                    angular.forEach($scope.getexamwisetotaldetails, function (su) {
                                        if (parseInt(st.AMST_Id) === parseInt(su.amsT_Id)) {
                                            $scope.totalgrade.push(su);
                                        }
                                    });
                                    st.markstotal = $scope.totalgrade;
                                });

                                $scope.attendancedetails = [];
                                angular.forEach($scope.getstudentdetails, function (dd) {
                                    $scope.attendancedetails = [];
                                    angular.forEach($scope.getstudentwiseattendancedetails, function (d) {
                                        if (dd.AMST_Id === d.AMST_Id) {
                                            $scope.attendancedetails.push({
                                                classheld: d.TOTALWORKINGDAYS, emE_Id: d.EME_Id,
                                                classatt: d.PRESENTDAYS,
                                                percentage: d.ATTENDANCEPERCENTAGE
                                            });
                                        }
                                    });
                                    dd.attendance = $scope.attendancedetails;
                                });
                            }

                            else if ($scope.reporttype === "promotion") {

                                $scope.tempexamdetails = [];
                                $scope.getexamdetails.push({ empG_GroupName : "Final Average", empsG_Order : 10001});
                                angular.forEach($scope.getexamdetails, function (dd) {
                                    $scope.tempexamdetails.push({ Groupname: dd.empG_GroupName, columnid: dd.empsG_Order, columname: "Marks" });
                                    $scope.tempexamdetails.push({ Groupname: dd.empG_GroupName, columnid: dd.empsG_Order, columname: "Point" });
                                });

                                console.log($scope.tempexamdetails);

                                $scope.getgroupexamdetails = promise.getgroupexamdetails;

                                $scope.getgroupexamdetails.push({ empG_GroupName: "Final Average", emE_Id: 50001, emE_ExamName: "Final Average" });

                                $scope.groupexam = [];

                                angular.forEach($scope.getexamdetails, function (dd) {
                                    $scope.groupexam = [];
                                    angular.forEach($scope.getgroupexamdetails, function (d) {
                                        if (d.empG_GroupName === dd.empG_GroupName) {
                                            $scope.groupexam.push(d);
                                        }                                       
                                    });

                                    dd.groupexamdetails = $scope.groupexam;
                                });



                                // Student Subject Wise
                                $scope.subjectdetails = [];
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.subjectdetails = [];
                                    angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                        if (stu.AMST_Id === parseInt(sub.AMST_Id)) {
                                            $scope.subjectdetails.push({
                                                ISMS_SubjectName: sub.subjectname, ISMS_Id: sub.subid,
                                                EYCES_AplResultFlg: sub.EYCES_AplResultFlg, subjectorder: sub.subjectorder,
                                                ISMS_SubjectCode: sub.ISMS_SubjectCode
                                            });
                                        }
                                    });
                                    stu.subjects = $scope.subjectdetails;
                                });

                                // Student Marks Wise
                                $scope.marksdetails = [];
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.marksdetails = [];
                                    angular.forEach($scope.getstudentmarksdetails, function (subd) {
                                        if (stu.AMST_Id === parseInt(subd.AMST_Id)) {
                                            $scope.marksdetails.push(subd);
                                        }
                                    });
                                    stu.marks = $scope.marksdetails;
                                });

                                // Over All Total                           
                                $scope.totalgrade = [];
                                angular.forEach($scope.getstudentdetails, function (st) {
                                    $scope.totalgrade = [];
                                    angular.forEach($scope.getexamwisetotaldetails, function (su) {
                                        if (parseInt(st.AMST_Id) === parseInt(su.AMST_Id)) {
                                            $scope.totalgrade.push(su);
                                        }
                                    });
                                    st.markstotal = $scope.totalgrade;
                                });

                                // Student Attendance Wise
                                $scope.attendancedetails = [];
                                angular.forEach($scope.getstudentdetails, function (dd) {
                                    $scope.attendancedetails = [];
                                    angular.forEach($scope.getstudentwiseattendancedetails, function (d) {
                                        if (dd.AMST_Id === d.AMST_Id) {
                                            $scope.attendancedetails.push(d);
                                        }
                                    });
                                    dd.attendance = $scope.attendancedetails;
                                });
                            }


                            console.log($scope.getstudentdetails);

                            angular.forEach($scope.year_list, function (dd) {
                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = dd.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.class_list, function (dd) {
                                if (dd.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                    $scope.cla = dd.asmcL_ClassName;
                                }
                            });

                            angular.forEach($scope.section_list, function (dd) {
                                if (dd.asmS_Id === parseInt($scope.ASMS_Id)) {
                                    $scope.sec = dd.asmC_SectionName;
                                }
                            });

                        } else {
                            swal("No Records Found");
                        }
                    }

                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.printToCart = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.reporttype === "indi") {
                innerContents = document.getElementById("Baldwin").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Stjames_XI_XII_PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="/css/print/StJames/Stjames_XI_XII_progresscardpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Stjames_XI_XII_printpdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else if ($scope.reporttype === "promotion") {
                innerContents = document.getElementById("Baldwin1").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Stjames_XI_XII_PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="/css/print/StJames/Stjames_XI_XII_progresscardpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Stjames_XI_XII_printpdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }            
        };

        $scope.Excel_HHS02 = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "CONSOLIDATED MARKS SHEET REPORT.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
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