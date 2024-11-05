(function () {
    'use strict';
    angular.module('app').controller('JSHStudentProgressCardReportController', JSHStudentProgressCardReportController)
    JSHStudentProgressCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'Excel', '$timeout', '$compile']
    function JSHStudentProgressCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, Excel, $timeout, $compile) {

        $scope.reportshowcount = 0;
        $scope.reportdata = true;
        $scope.ASA_FromDate = new Date();

        $scope.examflag = false;
        $scope.termflag = false;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("StudentProgressCardReport/getdetails", pageid).then(function (promise) {

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
                            swal("Marks Are Not Published");
                        }
                    }

                    if ($scope.examorterm === 'Term' || $scope.examorterm === "TermPromotion") {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        $scope.examflag = false;
                        $scope.termflag = true;

                        if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                            swal("Marks Are Not Published");
                        }
                    }

                } else {
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
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("StudentProgressCardReport/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getstudentdetails = promise.getstudentdetails;

                    if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                        $scope.examorterm = promise.examorterm;

                        if ($scope.examorterm === 'Exam') {
                            $scope.getexamtermlist = promise.getexamtermlist;
                            $scope.examflag = true;
                            $scope.termflag = false;

                            if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                                swal("Marks Are Not Published");
                            }
                        }

                        if ($scope.examorterm === 'Term' || $scope.examorterm === "TermPromotion") {
                            $scope.getexamtermlist = promise.getexamtermlist;
                            $scope.examflag = false;
                            $scope.termflag = true;

                            if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                                swal("Marks Are Not Published");
                            }
                        }

                    } else {
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
                var termid = 0;
                if ($scope.examorterm === "Exam") {
                    termid = 0;
                    emeid = obj.EME_Id;
                } else {
                    termid = obj.ECT_Id;
                    emeid = 0;
                }
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "EME_Id": emeid,
                    "ECT_Id": termid
                };
                apiService.create("StudentProgressCardReport/getreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.reportshowcount = 0;
                        $scope.generateddate = new Date();

                        if (promise.message !== "NotPublished") {

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

                            if (promise.examorterm === "Term" && promise.spflag === "1") {
                                $scope.jshsexamterm = "css/print/JSHS/JSHSTermReportCard.css";

                                if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {

                                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;

                                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;

                                    $scope.getgradedetails = promise.getgradedetails;

                                    $scope.gettermdetails = promise.gettermdetails;

                                    $scope.gettermexamdetails = promise.gettermexamdetails;

                                    $scope.studentdetails = promise.getstudentdetailsreport;

                                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;

                                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

                                    $scope.getstudentwisesportsdetails = promise.getstudentwisesportsdetails;

                                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;

                                    $scope.getstudentwisetermwisedetails = promise.getstudentwisetermwisedetails;


                                    // STUDENT WISE SUBJECT LIST
                                    $scope.studentwisesubject = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwisesubject = [];
                                        angular.forEach($scope.getstudentwisesubjectlist, function (subject) {
                                            if (student.AMST_Id === subject.AMST_Id) {
                                                $scope.studentwisesubject.push(subject);
                                            }
                                        });
                                        student.subjectlist = $scope.studentwisesubject;
                                    });

                                    //STUDENT WISE ATTENDANCE DETAILS
                                    angular.forEach($scope.studentdetails, function (student) {
                                        angular.forEach($scope.getstudentwiseattendancedetails, function (att) {
                                            if (student.AMST_Id === att.AMST_Id) {
                                                student.presentdays = att.PRESENTDAYS;
                                                student.totalworkingdays = att.TOTALWORKINGDAYS;
                                            }
                                        });
                                    });

                                    // STUDENT WISE TERM WISE REMARKS DETAILS

                                    angular.forEach($scope.studentdetails, function (stu) {
                                        angular.forEach($scope.getstudentwisetermwisedetails, function (d) {
                                            if (stu.AMST_Id === d.AMST_Id) {
                                                stu.remarks = d.ECTERE_Remarks;
                                            }
                                        });
                                    });

                                    // STUDENT WISE SKILLS LIST 
                                    $scope.studentwiseskillslist = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwiseskillslist = [];
                                        angular.forEach($scope.getstudentwiseskillslist, function (skills) {
                                            if (student.AMST_Id === skills.AMST_Id) {
                                                $scope.studentwiseskillslist.push(skills);
                                            }
                                        });
                                        student.skillslist = $scope.studentwiseskillslist;
                                    });

                                    // STUDENT WISE Activites LIST 
                                    $scope.studentwiseactiviteslist = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwiseactiviteslist = [];
                                        angular.forEach($scope.getstudentwiseactiviteslist, function (activites) {
                                            if (student.AMST_Id === activites.AMST_Id) {
                                                $scope.studentwiseactiviteslist.push(activites);
                                            }
                                        });
                                        student.activiteslist = $scope.studentwiseactiviteslist;
                                    });

                                    // STUDENT WISE Sports LIST 
                                    $scope.studentwisesportslist = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        angular.forEach($scope.getstudentwisesportsdetails, function (sport) {
                                            if (student.AMST_Id === sport.AMST_Id) {
                                                student.height = sport.SPCCSHW_Height;
                                                student.weight = sport.SPCCSHW_Weight;
                                            }
                                        });
                                    });


                                    // STUDENT WISE MARKS LIST 
                                    $scope.studentwisemarksdetails = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwisemarksdetails = [];
                                        angular.forEach($scope.getstudentmarksdetails, function (marks) {
                                            if (student.AMST_Id === marks.AMST_Id) {
                                                $scope.studentwisemarksdetails.push(marks);
                                            }
                                        });
                                        student.markslist = $scope.studentwisemarksdetails;
                                    });

                                    console.log($scope.studentdetails);


                                    angular.forEach($scope.termlist, function (dd) {
                                        if (dd.ecT_Id === parseInt($scope.ECT_Id)) {
                                            $scope.termname = dd.ecT_TermName.toUpperCase();
                                        }
                                    });

                                    angular.forEach($scope.year_list, function (dd) {
                                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                            $scope.yearname = dd.asmaY_Year;
                                        }
                                    });

                                    $scope.reportshowcount = 1;
                                } else {
                                    swal("No Records Found");
                                }
                            }

                            if (promise.examorterm === "Term" && promise.spflag === "2") {
                                $scope.jshsexamterm = "css/print/JSHS/JSHSTermReportCard.css";
                                if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {

                                    $scope.reportshowcount = 1;

                                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                                    $scope.JSHSReport = true;
                                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                                    $scope.getgradedetails = promise.getgradedetails;
                                    $scope.gettermdetails = promise.gettermdetails;
                                    $scope.gettermexamdetails = promise.gettermexamdetails;

                                    $scope.getgroupdetails = promise.getgroupdetails;

                                    $scope.subcolumnsexamdetails = [];

                                    angular.forEach($scope.getgroupdetails, function (dd) {
                                        $scope.subcolumnsexamdetails.push({
                                            columdisplay: dd.empG_DistplayName, columgroup: dd.empG_GroupName, colummarks: '(' + dd.empsG_PercentValue + ')'
                                        });
                                    });

                                    $scope.subcolumnsexamdetails.push({ columnid: 500000, columdisplay: 'MARKS OBTAINED', columgroup: 'Subject Total', colummarks: '(100)' });
                                    $scope.subcolumnsexamdetails.push({ columnid: 500001, columdisplay: 'Grade', columgroup: 'Subject Grade', colummarks: '' });

                                    $scope.studentdetails = promise.getstudentdetailsreport;

                                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;

                                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

                                    $scope.getstudentwisesportsdetails = promise.getstudentwisesportsdetails;

                                    $scope.getpromotionmarksdetails = promise.getpromotionmarksdetails;

                                    // STUDENT WISE SUBJECT LIST
                                    $scope.studentwisesubject = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwisesubject = [];
                                        angular.forEach($scope.getstudentwisesubjectlist, function (subject) {
                                            if (student.AMST_Id === subject.AMST_Id) {
                                                $scope.studentwisesubject.push(subject);
                                            }
                                        });
                                        student.subjectlist = $scope.studentwisesubject;
                                    });


                                    // STUDENT WISE SKILL  LIST 
                                    $scope.studentwiseskillslist = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwiseskillslist = [];
                                        angular.forEach($scope.getstudentwiseskillslist, function (skills) {
                                            if (student.AMST_Id === skills.AMST_Id) {
                                                $scope.studentwiseskillslist.push(skills);
                                            }
                                        });
                                        student.skillslist = $scope.studentwiseskillslist;
                                    });


                                    // STUDENT WISE Activites LIST 
                                    $scope.studentwiseactiviteslist = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwiseactiviteslist = [];
                                        angular.forEach($scope.getstudentwiseactiviteslist, function (activites) {
                                            if (student.AMST_Id === activites.AMST_Id) {
                                                $scope.studentwiseactiviteslist.push(activites);
                                            }
                                        });
                                        student.activiteslist = $scope.studentwiseactiviteslist;
                                    });


                                    // STUDENT WISE Sports LIST 
                                    $scope.studentwisesportslist = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        angular.forEach($scope.getstudentwisesportsdetails, function (sport) {
                                            if (student.AMST_Id === sport.AMST_Id) {
                                                student.height = sport.SPCCSHW_Height;
                                                student.weight = sport.SPCCSHW_Weight;
                                            }
                                        });
                                    });

                                    //$scope.colspan_sport = $scope.termlisttemp.length * 2;

                                    //$scope.colspan_sport_head = $scope.colspan_sport * 2;

                                    // STUDENT WISE MARKS LIST 

                                    $scope.studentwisemarksdetails = [];
                                    angular.forEach($scope.studentdetails, function (student) {
                                        $scope.studentwisemarksdetails = [];
                                        angular.forEach($scope.getstudentmarksdetails, function (marks) {
                                            if (student.AMST_Id === marks.AMST_Id) {
                                                $scope.studentwisemarksdetails.push(marks);
                                            }
                                        });
                                        student.markslist = $scope.studentwisemarksdetails;
                                    });

                                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;

                                    angular.forEach($scope.studentdetails, function (dd) {
                                        angular.forEach($scope.getstudentwiseattendancedetails, function (att) {
                                            if (att.AMST_Id === dd.AMST_Id) {
                                                dd.presentdays = att.PRESENTDAYS;
                                                dd.totalworkingdays = att.TOTALWORKINGDAYS;
                                                dd.totalpercentage = att.ATTENDANCEPERCENTAGE;
                                            }
                                        });
                                    });

                                    $scope.getstudentwisetermwisedetails = promise.getstudentwisetermwisedetails;

                                    angular.forEach($scope.studentdetails, function (dd) {
                                        angular.forEach($scope.getstudentwisetermwisedetails, function (d) {
                                            if (dd.AMST_Id === d.AMST_Id) {
                                                dd.remarks = d.ECTERE_Remarks;
                                            }
                                        });
                                    });

                                    angular.forEach($scope.studentdetails, function (dd) {
                                        angular.forEach($scope.getpromotionmarksdetails, function (ddd) {

                                            if (dd.AMST_Id === ddd.amsT_Id) {
                                                dd.overallmaxmarks = ddd.estmpP_TotalMaxMarks;
                                                dd.overallobtainedmarks = Math.round(ddd.estmpP_TotalObtMarks);
                                                dd.overallgrade = ddd.estmpP_TotalGrade;
                                            }
                                        });
                                    });

                                    console.log($scope.studentdetails);

                                    angular.forEach($scope.year_list, function (dd) {
                                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                            $scope.yearname = dd.asmaY_Year;
                                        }
                                    });

                                    angular.forEach($scope.termlist, function (dd) {
                                        if (dd.ecT_Id === parseInt($scope.ECT_Id)) {
                                            $scope.termname = dd.ecT_TermName;
                                        }
                                    });
                                } else {
                                    swal("No Records Found");
                                }
                            }

                            if ($scope.reportshowcount === 1) {
                                var e1 = angular.element(document.getElementById("report"));
                                $compile(e1.html(promise.htmlstring))(($scope));
                            }
                        } else {
                            swal("Still Marks Are Not Publish For Selected Details");
                            $scope.reportshowcount = 0;
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
            if ($scope.examorterm === "Exam") {
                innerContents = document.getElementById("printareaId").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/ProgressCardReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            if ($scope.examorterm === "Term") {
                innerContents = document.getElementById("HHS02").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSTermReportCardPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
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
    }
})();