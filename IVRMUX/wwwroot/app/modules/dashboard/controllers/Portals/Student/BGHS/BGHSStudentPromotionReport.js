(function () {
    'use strict';
    angular.module('app').controller('BGHSStudentPromotionReportController', BGHSStudentPromotionReportController)
    BGHSStudentPromotionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'Excel', '$timeout', '$compile']
    function BGHSStudentPromotionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, Excel, $timeout, $compile) {

        $scope.reportshowcount = 0;
        $scope.reportdata = true;
        $scope.ASA_FromDate = new Date();

        $scope.examflag = false;
        $scope.termflag = false;

        $scope.btn = true;

        $scope.BindData = function () {
            var pageid = "Promotion";

            apiService.getURI("StudentProgressCardReport/Bghsgetdetails", pageid).then(function (promise) {
                $scope.btn = false;
                $scope.getstudentdetails = promise.getstudentdetails;
                $scope.year_list = promise.getyear;
                $scope.classlist = promise.getclass;
                $scope.examorterm = promise.examorterm;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                    $scope.ASMCL_Id = $scope.getstudentdetails[0].asmcL_Id;
                    $scope.asmS_Id = $scope.getstudentdetails[0].asmS_Id;
                    $scope.asmaY_Id = $scope.getstudentdetails[0].asmaY_Id;

                    if ($scope.examorterm === 'Exam') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        $scope.examflag = true;
                        $scope.termflag = false;

                        if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                            $scope.btn = true;
                            swal("Marks Are Not Published");
                        }
                    }

                    else if ($scope.examorterm === 'Promotion') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        $scope.examflag = false;
                        $scope.termflag = true;

                        if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                            $scope.btn = true;
                            swal("Marks Are Not Published");
                        }
                    }
                    else {
                        $scope.btn = true;
                        swal("Report Format Not Mapped Contact Administrator");
                    }
                } else {
                    $scope.btn = true;
                    swal("No Studnet Data Found");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.getexamtermlist = [];
            $scope.obj.ECT_Id = "";
            $scope.obj.EME_Id = "";
            $scope.examflag = false;
            $scope.termflag = false;
            $scope.reportshowcount = 0;
            $scope.btn = false;
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "EPCFT_ExamFlag": "Promotion"
            };

            apiService.create("StudentProgressCardReport/Bghsonchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getstudentdetails = promise.getstudentdetails;

                    if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                        $scope.examorterm = promise.examorterm;

                        if ($scope.examorterm === 'Exam') {
                            $scope.getexamtermlist = promise.getexamtermlist;
                            $scope.examflag = true;
                            $scope.termflag = false;

                            if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                                $scope.btn = true;
                                swal("Marks Are Not Published");
                            }
                        }

                        else if ($scope.examorterm === 'Promotion') {
                            $scope.getexamtermlist = promise.getexamtermlist;
                            $scope.examflag = false;
                            $scope.termflag = true;

                            if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                                $scope.btn = true;
                                swal("Marks Are Not Published");
                            }
                        }
                        else {
                            $scope.btn = true;
                            swal("Report Format Not Mapped Contact Administrator");
                        }
                    } else {
                        $scope.btn = true;
                        swal("No Studnet Data Found");
                    }
                }
            });
        };

        $scope.onchangeexam = function () {
            $scope.reportshowcount = 0;
        };

        $scope.onchangeterm = function () {
            $scope.reportshowcount = 0;
        };

        $scope.obj = {};

        $scope.saveddata = function (obj) {
            $scope.submitted = true;
            $scope.reportdata = true;
            $scope.reportshowcount = 0;
            if ($scope.myForm.$valid) {
                var emeid = 0;
                if ($scope.examorterm === "Exam") {
                    emeid = obj.EME_Id;
                } else {
                    emeid = 0;
                }
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "EME_Id": emeid,
                    "EPCFT_ExamFlag": "Promotion"
                };
                apiService.create("StudentProgressCardReport/Bghsgetreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.reportshowcount = 0;
                        $scope.generateddate = new Date();
                        $scope.spflag = promise.spflag;

                        if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                            $scope.JSHSReport = true;
                            $scope.reportshowcount = 1;
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getstudentdetails = promise.getstudentdetails;
                            $scope.getexamdetails = promise.getexamdetails;

                            $scope.getgroupexamdetails = promise.getgroupexamdetails;
                            $scope.getexammaxmarks = promise.getexammaxmarks;

                            $scope.groupexamdetails = [];
                            angular.forEach($scope.getexamdetails, function (dd) {
                                $scope.groupexamdetails = [];
                                angular.forEach($scope.getgroupexamdetails, function (d) {
                                    if (d.empG_GroupName === dd.empG_GroupName) {
                                        $scope.groupexamdetails.push(d);
                                    }
                                });

                                $scope.groupexamdetails.push({
                                    empG_GroupName: dd.empG_GroupName, emE_Id: 10000, emE_ExamName: 'Total', emE_ExamOrder: 10000,
                                    empG_DistplayName: dd.empG_DistplayName
                                });
                                $scope.groupexamdetails.push({
                                    empG_GroupName: dd.empG_GroupName, emE_Id: 100001, emE_ExamName: 'Grade', emE_ExamOrder: 100001,
                                    empG_DistplayName: dd.empG_DistplayName
                                });

                                dd.examgrpdetails = $scope.groupexamdetails;
                            });


                            $scope.groupexamdetailsnew = [];
                            angular.forEach($scope.getexamdetails, function (dd) {

                                angular.forEach($scope.getgroupexamdetails, function (d) {
                                    if (d.empG_GroupName === dd.empG_GroupName) {
                                        $scope.groupexamdetailsnew.push(d);
                                    }
                                });

                                $scope.groupexamdetailsnew.push({
                                    empG_GroupName: dd.empG_GroupName, emE_Id: 10000, emE_ExamName: 'Total', emE_ExamOrder: 10000,
                                    empG_DistplayName: dd.empG_DistplayName
                                });
                                $scope.groupexamdetailsnew.push({
                                    empG_GroupName: dd.empG_GroupName, emE_Id: 100001, emE_ExamName: 'Grade', emE_ExamOrder: 100001,
                                    empG_DistplayName: dd.empG_DistplayName
                                });

                            });

                            angular.forEach($scope.getexamdetails, function (dd) {
                                var counttotal = 0;
                                angular.forEach($scope.groupexamdetailsnew, function (d) {
                                    angular.forEach($scope.getexammaxmarks, function (ddd) {
                                        if (dd.empG_GroupName === d.empG_GroupName && d.emE_Id === ddd.emE_Id && d.emE_Id !== 10000 && d.emE_Id !== 100001) {
                                            d.maxmarks = ddd.eyceS_MaxMarks;
                                            counttotal += ddd.eyceS_MaxMarks;
                                        } else if (dd.empG_GroupName === d.empG_GroupName && d.emE_Id === 10000) {
                                            d.maxmarks = counttotal;
                                        }
                                    });
                                });
                            });

                            // STUDENT WISE SUBJECT DETAILS
                            $scope.subjectdetails = [];
                            var nonapplysubject = 0;
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.subjectdetails = [];
                                angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                    if (stu.AMST_Id === sub.AMST_Id) {
                                        $scope.subjectdetails.push(sub);
                                    }
                                    if (sub.EYCES_AplResultFlg === false) {
                                        nonapplysubject += 1;
                                    }
                                });
                                stu.subjects = $scope.subjectdetails;
                                if (nonapplysubject === 0) {
                                    stu.subjectdisplay = 0;
                                } else {
                                    stu.subjectdisplay = 1;
                                }
                            });

                            // STUDENT WISE MARKS DETAILS
                            $scope.marksdetails = [];
                            $scope.totalmarksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (stu) {
                                $scope.marksdetails = [];
                                $scope.totalmarksdetails = [];
                                angular.forEach($scope.getstudentmarksdetails, function (sub) {
                                    if (stu.AMST_Id === sub.AMST_Id) {
                                        if (sub.ISMS_Id !== 50001) {
                                            $scope.marksdetails.push(sub);
                                        } else {
                                            $scope.totalmarksdetails.push(sub);
                                        }
                                    }
                                });
                                stu.marks = $scope.marksdetails;
                                stu.totalmarks = $scope.totalmarksdetails;
                            });


                            $scope.getclassteacher = promise.getclassteacher;
                            if ($scope.getclassteacher !== null && $scope.getclassteacher.length > 0) {
                                $scope.clastechname = $scope.getclassteacher[0].classteachername;
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

                        if ($scope.reportshowcount === 1) {
                            var e1 = angular.element(document.getElementById("report"));
                            $compile(e1.html(promise.htmlstring))(($scope));
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
            }
        };



        $scope.saveddata_backup = function (obj) {
            $scope.submitted = true;
            $scope.reportdata = true;
            $scope.reportshowcount = 0;
            if ($scope.myForm.$valid) {
                var emeid = 0;
                if ($scope.examorterm === "Exam") {
                    emeid = obj.EME_Id;
                } else {
                    emeid = 0;
                }
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "EME_Id": emeid,
                    "EPCFT_ExamFlag": "Promotion"
                };
                apiService.create("StudentProgressCardReport/Bghsgetreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.reportshowcount = 0;
                        $scope.generateddate = new Date();
                        $scope.spflag = promise.spflag;
                        if (promise.examorterm === "Exam") {
                            $scope.jshsexamterm = "css/print/JSHS/ProgressCardReport.css";
                            if (promise.savelist !== null && promise.savelist.length > 0) {
                                $scope.bb = promise.savelist;
                                $scope.CurrentDate = $scope.ASA_FromDate;
                                $scope.studenttemparray = [];
                                $scope.reportdata = false;
                                angular.forEach($scope.bb, function (rr) {
                                    if ($scope.studenttemparray.length === 0) {
                                        $scope.studenttemparray.push({
                                            amst_id: rr.amsT_Id, student_name: rr.amsT_FirstName, class_name: rr.asmcL_ClassName, section_name: rr.asmC_SectionName,
                                            rollno: rr.amaY_RollNo, fathersname: rr.amsT_FatherName, mothername: rr.amsT_MotherName, dob: rr.amsT_DOB,
                                            asmaY_Year: rr.asmaY_Year, estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalGrade: rr.estmP_TotalGrade,
                                            estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, admno: rr.amsT_AdmNo
                                        });
                                    }
                                    else if ($scope.studenttemparray.length > 0) {
                                        var count = 0;
                                        angular.forEach($scope.studenttemparray, function (yy) {
                                            if (yy.amst_id === rr.amsT_Id) {
                                                count += 1;
                                            }
                                        });
                                        if (count === 0) {
                                            $scope.studenttemparray.push({
                                                amst_id: rr.amsT_Id, student_name: rr.amsT_FirstName, class_name: rr.asmcL_ClassName,
                                                section_name: rr.asmC_SectionName, rollno: rr.amaY_RollNo, fathersname: rr.amsT_FatherName,
                                                mothername: rr.amsT_MotherName, dob: rr.amsT_DOB, asmaY_Year: rr.asmaY_Year,
                                                estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalGrade: rr.estmP_TotalGrade,
                                                estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, admno: rr.amsT_AdmNo
                                            });
                                        }
                                    }
                                });

                                $scope.subjecttemparray = [];

                                angular.forEach($scope.studenttemparray, function (ww) {
                                    $scope.subjecttemparray = [];
                                    angular.forEach($scope.bb, function (tt) {
                                        if (ww.amst_id === tt.amsT_Id) {
                                            $scope.subjecttemparray.push({
                                                subj_id: tt.ismS_Id, sub_name: tt.ismS_SubjectName, estmpS_MaxMarks: tt.estmpS_MaxMarks, estmpS_ObtainedMarks: tt.estmpS_ObtainedMarks, estmpS_ObtainedGrade: tt.estmpS_ObtainedGrade, eyceS_AplResultFlg: tt.eyceS_AplResultFlg
                                            });
                                        }
                                    });
                                    ww.subjectlist = $scope.subjecttemparray;
                                });

                                $scope.Presentattendence = promise.present_attendence;
                                angular.forEach($scope.studenttemparray, function (dd) {
                                    angular.forEach($scope.Presentattendence, function (att) {
                                        if (dd.amst_id === att.AMST_Id) {
                                            dd.presentdays = att.classattended;
                                            dd.workingdays = att.classheld;
                                        }
                                    });
                                });

                                $scope.examwiseremarks = promise.examwiseremarks;

                                angular.forEach($scope.studenttemparray, function (d) {
                                    angular.forEach($scope.examwiseremarks, function (at) {
                                        if (d.amst_id === at.amsT_Id) {
                                            d.remarksd = at.emeR_Remarks;
                                        }
                                    });
                                });

                                $scope.getstudentdetails = promise.getstudentdetails;

                                angular.forEach($scope.studenttemparray, function (dd) {
                                    angular.forEach($scope.getstudentdetails, function (d) {
                                        if (dd.amst_id === d.amsT_Id) {
                                            dd.photoname = d.photoname;
                                        }
                                    });
                                });

                                angular.forEach($scope.examlist, function (e) {
                                    if (e.emE_Id === parseInt(obj.EME_Id)) {
                                        $scope.examname = e.emE_ExamName.toUpperCase();
                                    }
                                });

                                $scope.reportshowcount = 1;
                            } else {
                                swal("No Records Found");
                            }
                        }                       

                        else if (promise.examorterm === "Promotion" && promise.spflag === "2") {
                            if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                                $scope.JSHSReport = true;
                                $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                                $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                                $scope.getstudentdetails = promise.getstudentdetails;
                                $scope.getexamdetails = promise.getexamdetails;

                                $scope.getgroupexamdetails = promise.getgroupexamdetails;
                                $scope.getexammaxmarks = promise.getexammaxmarks;

                                $scope.groupexamdetails = [];
                                angular.forEach($scope.getexamdetails, function (dd) {
                                    $scope.groupexamdetails = [];
                                    angular.forEach($scope.getgroupexamdetails, function (d) {
                                        if (d.empG_GroupName === dd.empG_GroupName) {
                                            $scope.groupexamdetails.push(d);
                                        }
                                    });

                                    $scope.groupexamdetails.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 10000, emE_ExamName: 'Total', emE_ExamOrder: 10000,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });
                                    $scope.groupexamdetails.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 100001, emE_ExamName: 'Grade', emE_ExamOrder: 100001,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });

                                    dd.examgrpdetails = $scope.groupexamdetails;
                                });


                                $scope.groupexamdetailsnew = [];
                                angular.forEach($scope.getexamdetails, function (dd) {

                                    angular.forEach($scope.getgroupexamdetails, function (d) {
                                        if (d.empG_GroupName === dd.empG_GroupName) {
                                            $scope.groupexamdetailsnew.push(d);
                                        }
                                    });

                                    $scope.groupexamdetailsnew.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 10000, emE_ExamName: 'Total', emE_ExamOrder: 10000,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });
                                    $scope.groupexamdetailsnew.push({
                                        empG_GroupName: dd.empG_GroupName, emE_Id: 100001, emE_ExamName: 'Grade', emE_ExamOrder: 100001,
                                        empG_DistplayName: dd.empG_DistplayName
                                    });

                                });

                                angular.forEach($scope.getexamdetails, function (dd) {
                                    var counttotal = 0;
                                    angular.forEach($scope.groupexamdetailsnew, function (d) {
                                        angular.forEach($scope.getexammaxmarks, function (ddd) {
                                            if (dd.empG_GroupName === d.empG_GroupName && d.emE_Id === ddd.emE_Id && d.emE_Id !== 10000 && d.emE_Id !== 100001) {
                                                d.maxmarks = ddd.eyceS_MaxMarks;
                                                counttotal += ddd.eyceS_MaxMarks;
                                            } else if (dd.empG_GroupName === d.empG_GroupName && d.emE_Id === 10000) {
                                                d.maxmarks = counttotal;
                                            }
                                        });
                                    });
                                });

                                // STUDENT WISE SUBJECT DETAILS
                                $scope.subjectdetails = [];
                                var nonapplysubject = 0;
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.subjectdetails = [];
                                    angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                        if (stu.AMST_Id === sub.AMST_Id) {
                                            $scope.subjectdetails.push(sub);
                                        }
                                        if (sub.EYCES_AplResultFlg === false) {
                                            nonapplysubject += 1;
                                        }
                                    });
                                    stu.subjects = $scope.subjectdetails;
                                    if (nonapplysubject === 0) {
                                        stu.subjectdisplay = 0;
                                    } else {
                                        stu.subjectdisplay = 1;
                                    }
                                });

                                // STUDENT WISE MARKS DETAILS
                                $scope.marksdetails = [];
                                $scope.totalmarksdetails = [];
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.marksdetails = [];
                                    $scope.totalmarksdetails = [];
                                    angular.forEach($scope.getstudentmarksdetails, function (sub) {
                                        if (stu.AMST_Id === sub.AMST_Id) {
                                            if (sub.ISMS_Id !== 50001) {
                                                $scope.marksdetails.push(sub);
                                            } else {
                                                $scope.totalmarksdetails.push(sub);
                                            }
                                        }
                                    });
                                    stu.marks = $scope.marksdetails;
                                    stu.totalmarks = $scope.totalmarksdetails;
                                });                            


                                $scope.getclassteacher = promise.getclassteacher;
                                if ($scope.getclassteacher !== null && $scope.getclassteacher.length > 0) {
                                    $scope.clastechname = $scope.getclassteacher[0].classteachername;
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

                        else {
                            swal("Report Format Not Mapped Contact Administrator");
                        }
                        if ($scope.reportshowcount === 1) {
                            var e1 = angular.element(document.getElementById("report"));
                            $compile(e1.html(promise.htmlstring))(($scope));
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
            }
        };

        $scope.print_HHS02 = function () {
            var innerContents = "";
            var popupWinindow = "";
            
                innerContents = document.getElementById("pdf").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.reportdata = true;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("StudentProgressCardReport/get_Exam_grade_pc", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
                $scope.getexamlist = promise.getexam;
                if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                    $scope.examlist = $scope.getexamlist;
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

        $scope.romanfunction = function (num) {
            var lookup = { M: 1000, CM: 900, D: 500, CD: 400, C: 100, XC: 90, l: 50, xl: 40, x: 10, ix: 9, v: 5, iv: 4, i: 1 },
                roman = '',
                i;
            for (i in lookup) {
                while (num >= lookup[i]) {
                    roman += i;
                    num -= lookup[i];
                }
            }
            $scope.romnanumber = roman;
        };
    }
})();