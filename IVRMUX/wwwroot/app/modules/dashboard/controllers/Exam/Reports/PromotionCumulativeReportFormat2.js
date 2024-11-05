﻿(function () {
    'use strict';
    angular.module('app').controller('PromotionCumulativeReportFormat2Controller', PromotionCumulativeReportFormat2Controller)

    PromotionCumulativeReportFormat2Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function PromotionCumulativeReportFormat2Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.promotionwiseremarks = false;
        $scope.examwiseremarks = false;
        $scope.btndisable = false;
        $scope.Left_Flag = true;
        $scope.Deactive_Flag = true;
        $scope.studentlist = [];
        $scope.configuration = [];
        $scope.getsubjectlist = [];
        $scope.reportdata = [];
        $scope.subjectwisetotal = [];
        $scope.studentwisetotal = [];
        $scope.getsubjectgrouplist = [];
        $scope.studentlistdetails = [];
        $scope.getreportdetails = false;
        $scope.details_report = false;
        $scope.subjectrank = false;
        $scope.Flag = "all";
        $scope.submitted = false;
        var paginationformasters = '';
        var ivrmcofigsettings = [];

        var copty;
        ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];

        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var logopath = "";
        admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("PromotionReportDetails/getdata", pageid).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
            });
        };

        $scope.print_flag = true;

        $scope.onchangeyear = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.studentlistdetails = [];
            $scope.btndisable = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("PromotionReportDetails/onchangeyear", data).then(function (Promise) {
                $scope.classlist = Promise.allclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.studentlistdetails = [];
            $scope.btndisable = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("PromotionReportDetails/onchangeclass", data).then(function (promise) {
                $scope.sectionlist = promise.allsectionlist;
            });
        };

        $scope.onchangesection = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.studentlistdetails = [];
            $scope.btndisable = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("PromotionReportDetails/onchangesection", data).then(function (promise) {

                if (promise !== null) {
                    $scope.studentlistdetails = promise.studentlistdetails;
                    $scope.all = true;
                    angular.forEach($scope.studentlistdetails, function (dd) {
                        dd.checkedsub = true;
                    });

                    if (promise.getpromotiondetails === null || promise.getpromotiondetails.length === 0) {
                        swal("Still Promotion Setting Not Done For The Selected Class & Section");
                    } else {
                        $scope.btndisable = true;
                    }
                }
            });
        };

        $scope.OnChangeLeftFlag = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.studentlistdetails = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("PromotionReportDetails/onchangesection", data).then(function (promise) {
                $scope.studentlistdetails = promise.studentlistdetails;

                $scope.all = true;
                angular.forEach($scope.studentlistdetails, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };  

        $scope.getpromotioncumulativereport = function () {
            if ($scope.myForm.$valid) {
                $scope.colarrayall = [];
                $scope.print_flag = true;
                $scope.getreportdetails = false;
                $scope.studentlist = [];
                $scope.configuration = [];
                $scope.getsubjectlist = [];
                $scope.reportdata = [];
                $scope.subjectwisetotal = [];
                $scope.studentwisetotal = [];
                $scope.getsubjectgrouplist = [];
                $scope.details_report = false;
                $scope.tempstudentdetails = [];
                angular.forEach($scope.studentlistdetails, function (dd) {
                    if (dd.checkedsub) {
                        $scope.tempstudentdetails.push(dd.amsT_Id);
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "AMST_Ids": $scope.tempstudentdetails
                };
                apiService.create("PromotionReportDetails/getpromotioncumulativereport_format2", data).then(function (promise) {
                    $scope.print_flag = false;
                    $scope.details_report = false;
                    $scope.getpromotiondetails = promise.getpromotiondetails;
                    if ($scope.getpromotiondetails !== null && $scope.getpromotiondetails.length > 0) {
                        $scope.studentlist = []

                        angular.forEach(promise.studentlist, function (stu) {
                            $scope.studentlist.push({
                                amsT_Id: stu.amsT_Id, amsT_FirstName: stu.amsT_FirstName,
                                amsT_RegistrationNo: stu.amsT_RegistrationNo, amsT_AdmNo: stu.amsT_AdmNo,
                                amaY_RollNo: stu.amaY_RollNo, amsT_DOB: stu.amsT_DOB
                            });
                        });

                        $scope.configuration = promise.configuration;
                        $scope.getsubjectlist = promise.getsubjectlist;
                        $scope.reportdata = promise.reportdata;
                        $scope.subjectwisetotal = promise.subjectwisetotal;
                        $scope.studentwisetotal = promise.studentwisetotal;
                        $scope.studenwise_remarks = promise.studenwise_remarks;
                        $scope.studentwise_individualexamremarks = promise.studentwise_individualexamremarks;
                        $scope.getexamlist = promise.getexamlist;
                        $scope.masterinstitution = promise.masterinstitution;

                        $scope.mI_name = $scope.masterinstitution !== null && $scope.masterinstitution.length > 0 ? $scope.masterinstitution[0].mI_Name : "";

                        $scope.getexamlist = [];

                        angular.forEach(promise.getexamlist, function (exm) {
                            $scope.getexamlist.push({ emE_Id: exm.emE_Id, emE_ExamName: exm.emE_ExamName, emE_ExamOrder: exm.emE_ExamOrder });
                        });

                        $scope.getexamlist.push({ emE_Id: 0, emE_ExamName: "FA", emE_ExamOrder: 500000 });

                        if ($scope.getpromotiondetails[0].emP_MarksPerFlg === "P" || $scope.getpromotiondetails[0].emP_MarksPerFlg === "M") {
                            if ($scope.reportdata !== null && $scope.reportdata.length > 0) {
                                $scope.getexamwiseavgmarkspromotion = promise.getexamwiseavgmarkspromotion;
                                $scope.details_report = true;
                                $scope.getreportdetails = true;
                                var count = 0;
                                angular.forEach($scope.studentlist, function (stu) {
                                    $scope.studentwise_marks = [];
                                    angular.forEach($scope.reportdata, function (stu_marks) {
                                        if (stu.amsT_Id === stu_marks.AMST_Id) {
                                            $scope.studentwise_marks.push({
                                                AMST_Id: stu_marks.AMST_Id, ISMS_Id: stu_marks.ISMS_Id,
                                                EME_Id: stu_marks.EME_Id, ObtainedMarks: stu_marks.ESTMPPSGE_ExamConvertedMarks,
                                                ObtainedGrade: stu_marks.ESTMPPSGE_ExamConvertedGrade, PassFailResult: stu_marks.ESTMPPSGE_ExamPassFailFlag
                                            });
                                        }
                                    });

                                    // Subject Wise Average
                                    angular.forEach($scope.subjectwisetotal, function (stu_subj_total) {
                                        if (stu_subj_total.amsT_Id === stu.amsT_Id) {
                                            $scope.studentwise_marks.push({
                                                AMST_Id: stu_subj_total.amsT_Id, ISMS_Id: stu_subj_total.ismS_Id,
                                                EME_Id: 0, ObtainedMarks: stu_subj_total.estmppS_ObtainedMarks,
                                                ObtainedGrade: stu_subj_total.estmppS_ObtainedGrade, PassFailResult: stu_subj_total.estmppS_PassFailFlg
                                            });
                                        }
                                    });

                                    // Exam Wise Total
                                    angular.forEach($scope.getexamwiseavgmarkspromotion, function (stu_total_exam_avg) {
                                        if (stu_total_exam_avg.AMST_Id === stu.amsT_Id) {
                                            $scope.studentwise_marks.push({
                                                AMST_Id: stu_total_exam_avg.AMST_Id, ISMS_Id: 0,
                                                EME_Id: stu_total_exam_avg.EME_Id, ObtainedMarks: stu_total_exam_avg.ESTMPPSGE_ExamConvertedMarks,
                                                //ObtainedGrade: stu_total_exam_avg.estmpP_ObtainedGrade, PassFailResult: stu_total_exam_avg.estmpP_PassFailFlg,
                                                ObtainedPercentage: stu_total_exam_avg.ExamPercentage
                                            });
                                        }
                                    });

                                    // Student Wise AVG Total
                                    angular.forEach($scope.studentwisetotal, function (stu_total_avg) {
                                        if (stu_total_avg.amsT_Id === stu.amsT_Id) {
                                            $scope.studentwise_marks.push({
                                                AMST_Id: stu_total_avg.AMST_Id, ISMS_Id: 0,
                                                EME_Id: 0, ObtainedMarks: stu_total_avg.estmpP_TotalObtMarks,
                                                ObtainedGrade: stu_total_avg.estmpP_ObtainedGrade, PassFailResult: stu_total_avg.estmpP_Result,
                                                ObtainedPercentage: stu_total_avg.estmpP_Percentage
                                            });
                                        }
                                    });

                                    stu.student_marks = $scope.studentwise_marks;
                                });
                            } else {
                                swal("No Records Found");
                            }

                            console.log($scope.studentlist);
                        }

                        //else if ($scope.getpromotiondetails[0].emP_MarksPerFlg === "T") {
                        //    if ($scope.reportdata !== null && $scope.reportdata.length > 0) {
                        //        $scope.getreportdetails = true;
                        //        $scope.details_report = true;
                        //        var counttotal = 0;
                        //        if ($scope.configuration !== null && $scope.configuration.length > 0) {
                        //            $scope.colarrayall = [{ title: "SLNO", template: "<span class='row-number'></span>", width: 100, locked: "true" },
                        //            { name: 'amsT_FirstName', field: 'amsT_FirstName', title: 'Student Name', width: 200, locked: "true" }];

                        //            if ($scope.configuration[0].exmConfig_RegnoColumnDisplay === true) {
                        //                $scope.colarrayall.push({ name: 'amsT_RegistrationNo', field: 'amsT_RegistrationNo', title: 'Reg.NO', width: 100 });
                        //                $scope.regno = true;
                        //                counttotal = counttotal + 1;
                        //            } else {
                        //                $scope.regno = false;
                        //            }

                        //            if ($scope.configuration[0].exmConfig_AdmnoColumnDisplay === true) {
                        //                $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                        //                $scope.admno = true;
                        //                counttotal = counttotal + 1;
                        //            } else {
                        //                $scope.admno = false;
                        //            }

                        //            if ($scope.configuration[0].exmConfig_RollnoColumnDisplay === true) {
                        //                $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });
                        //                $scope.rollno = true;
                        //                counttotal = counttotal + 1;
                        //            } else {
                        //                $scope.rollno = false;
                        //            }

                        //            if (counttotal === 0) {
                        //                $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                        //                $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });
                        //                $scope.admno = true;
                        //                $scope.rollno = true;
                        //            }
                        //        } else {
                        //            $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                        //            $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });
                        //            $scope.admno = true;
                        //            $scope.rollno = true;
                        //        }

                        //        /* GETTING DISTINCT SUBJECTS */
                        //        $scope.getsubjects = [];
                        //        var subjcounttotal = 0;
                        //        angular.forEach($scope.reportdata, function (dd) {
                        //            if ($scope.getsubjects.length === 0) {
                        //                $scope.getsubjects.push({
                        //                    ismS_SubjectName: dd.ISMS_SubjectName, ismS_Id: dd.ISMS_Id, AplResultFlg: dd.EYCES_AplResultFlg,
                        //                    EYCES_MarksDisplayFlg: dd.EYCES_MarksDisplayFlg, EYCES_GradeDisplayFlg: dd.EYCES_GradeDisplayFlg
                        //                });
                        //            } else if ($scope.getsubjects.length > 0) {
                        //                subjcounttotal = 0;
                        //                angular.forEach($scope.getsubjects, function (d) {
                        //                    if (dd.ISMS_Id === d.ismS_Id) {
                        //                        subjcounttotal += 1;
                        //                    }
                        //                });

                        //                if (subjcounttotal === 0) {
                        //                    $scope.getsubjects.push({
                        //                        ismS_SubjectName: dd.ISMS_SubjectName, ismS_Id: dd.ISMS_Id, AplResultFlg: dd.EYCES_AplResultFlg,
                        //                        EYCES_MarksDisplayFlg: dd.EYCES_MarksDisplayFlg, EYCES_GradeDisplayFlg: dd.EYCES_GradeDisplayFlg
                        //                    });
                        //                }
                        //            }
                        //        });


                        //        /* CREATING COLUMNS TO DISPLAY FOR SUBJECTS */
                        //        $scope.colarraytmp = [];
                        //        angular.forEach($scope.getsubjects, function (obj1) {
                        //            $scope.colarraytmp.push({
                        //                title: obj1.ismS_SubjectName, field: obj1.ismS_SubjectName, idd: obj1.ismS_Id,
                        //                AplResultFlg: obj1.AplResultFlg
                        //            });
                        //        });

                        //        /* CREATING COLUMNS TO DISPLAY FOR EXAM WITH SUBJECT WISE */
                        //        angular.forEach($scope.colarraytmp, function (qwe) {
                        //            $scope.nwtmpary = [];
                        //            angular.forEach($scope.getexamlist, function (obj2) {
                        //                var subexamname = obj2.emE_ExamName;
                        //                subexamname = subexamname.replace(".", "");
                        //                subexamname = subexamname.replace("&", "");
                        //                subexamname = subexamname.replace(" ", "");
                        //                subexamname = subexamname.replace("/", "");
                        //                subexamname = subexamname.replace("-", "");
                        //                subexamname = subexamname.replace(" ", "");

                        //                $scope.nwtmpary.push({
                        //                    title: obj2.emE_ExamName, field: "unique" + qwe.idd + subexamname, width: 100, displayname: obj2.emE_ExamName,
                        //                    grouporder: obj2.emE_ExamOrder
                        //                });
                        //            });
                        //            $scope.nwtmpary.push({
                        //                title: 'Total', field: "unique" + qwe.idd + 'Total', width: 100
                        //            });
                        //            qwe.columns = $scope.nwtmpary;
                        //        });

                        //        angular.forEach($scope.colarraytmp, function (d) {
                        //            $scope.colarrayall.push(d);
                        //        });

                        //        $scope.details = [];
                        //        angular.forEach($scope.studentlist, function (dd) {
                        //            $scope.details.push({
                        //                amsT_Id: dd.amsT_Id, amsT_FirstName: dd.amsT_FirstName, amsT_RegistrationNo: dd.amsT_RegistrationNo, amsT_AdmNo: dd.amsT_AdmNo,
                        //                amaY_RollNo: dd.amaY_RollNo
                        //            });
                        //        });

                        //        $scope.colarrayall.push({ name: 'total', field: 'total', title: 'Total', width: 100 });
                        //        $scope.colarrayall.push({ name: 'percentage', field: 'percentage', title: 'Percentage', width: 100 });
                        //        $scope.colarrayall.push({ name: 'rank', field: 'rank', title: 'Rank', width: 100 });
                        //        $scope.colarrayall.push({ name: 'result', field: 'result', title: 'Result', width: 100 });


                        //        // Individual Exam Wise Remarks
                        //        $scope.TempDistinct_Examnames = [];
                        //        if ($scope.examwiseremarks === true && $scope.studentwise_individualexamremarks !== null && $scope.studentwise_individualexamremarks.length > 0) {
                        //            $scope.TempDistinct_Examnames = $scope.studentwise_individualexamremarks.filter((item, i, arr) => arr.findIndex((t) => t.emE_Id === item.emE_Id) === i);

                        //            $scope.colarraytmp_remarks = [];
                        //            //$scope.colarraytmp_remarks.push({ title: "IE_Remarks", field: "IE_Remarks", idd: "IE_Remarks_0" });

                        //            $scope.nwtmpary_remarks = [];
                        //            angular.forEach($scope.TempDistinct_Examnames, function (dd_exam) {
                        //                var examname = dd_exam.emE_ExamName + "Remarks";
                        //                var examid = dd_exam.emE_Id + "Remarks";
                        //                $scope.nwtmpary_remarks.push({
                        //                    name: 'IE_Remarks' + examid, field: 'IE_Remarks' + examid, title: dd_exam.emE_ExamName,
                        //                    width: 200
                        //                });
                        //            });

                        //            $scope.colarrayall.push({
                        //                title: "IE_Remarks", field: "IE_Remarks", idd: "IE_Remarks_0",
                        //                columns: $scope.nwtmpary_remarks
                        //            });
                        //        }

                        //        // Promotion Wise Remarks
                        //        if ($scope.promotionwiseremarks === true && $scope.studenwise_remarks !== null && $scope.studenwise_remarks.length > 0) {
                        //            $scope.colarrayall.push({ name: 'stu_pe_remarks', field: 'stu_pe_remarks', title: 'Promotion Remarks', width: 200 });
                        //        }

                        //        console.log($scope.colarrayall);

                        //        /* SUBJECT EXAM WISE MARKS */
                        //        angular.forEach($scope.details, function (student) {
                        //            angular.forEach($scope.getexamlist, function (subjectsgrp) {
                        //                angular.forEach($scope.reportdata, function (list) {
                        //                    if (student.amsT_Id === list.AMST_Id && subjectsgrp.emE_Id === list.EME_Id) {
                        //                        var subexamname = list.EME_ExamName;
                        //                        subexamname = subexamname.replace(".", "");
                        //                        subexamname = subexamname.replace("&", "");
                        //                        subexamname = subexamname.replace(" ", "");
                        //                        subexamname = subexamname.replace("/", "");
                        //                        subexamname = subexamname.replace("-", "");
                        //                        subexamname = subexamname.replace(" ", "");

                        //                        if (list.EYCES_AplResultFlg === true) {

                        //                            if (list.ESTMPS_PassFailFlg === "AB") {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_PassFailFlg;
                        //                            } else if (list.ESTMPS_PassFailFlg === "M") {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_ObtainedMarks + "(" + list.ESTMPS_PassFailFlg + ")";
                        //                            } else if (list.ESTMPS_PassFailFlg === "Fail") {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_ObtainedMarks + "*";
                        //                            } else {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_ObtainedMarks;
                        //                            }
                        //                        } else {
                        //                            if (list.ESTMPS_PassFailFlg === "AB") {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_PassFailFlg;
                        //                            } else if (list.ESTMPS_PassFailFlg === "M") {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_ObtainedGrade + "(" + list.ESTMPS_PassFailFlg + ")";
                        //                            } else if (list.ESTMPS_PassFailFlg === "Fail") {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_ObtainedGrade + "*";
                        //                            } else {
                        //                                student["unique" + list.ISMS_Id + subexamname] = list.ESTMPS_ObtainedGrade;
                        //                            }
                        //                        }
                        //                    }
                        //                });
                        //            });
                        //        });

                        //        /* SUBJECT WISE MARKS */
                        //        angular.forEach($scope.details, function (student) {
                        //            angular.forEach($scope.getsubjects, function (subject) {
                        //                angular.forEach($scope.subjectwisetotal, function (list) {
                        //                    if (student.amsT_Id === list.amsT_Id && subject.ismS_Id === list.ismS_Id) {
                        //                        var subexamname = 'Total';
                        //                        subexamname = subexamname.replace(".", "");
                        //                        subexamname = subexamname.replace("&", "");
                        //                        subexamname = subexamname.replace(" ", "");
                        //                        subexamname = subexamname.replace("/", "");
                        //                        subexamname = subexamname.replace("-", "");
                        //                        subexamname = subexamname.replace(" ", "");

                        //                        if (subject.AplResultFlg === true) {
                        //                            if (list.estmppS_PassFailFlg === "AB") {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_PassFailFlg;
                        //                            } else if (list.estmppS_PassFailFlg === "M") {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_ObtainedMarks +
                        //                                    "(" + list.estmppS_PassFailFlg + ")";
                        //                            } else if (list.estmppS_PassFailFlg === "Fail") {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_ObtainedMarks; + "*";
                        //                            } else {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_ObtainedMarks;
                        //                            }
                        //                        } else {
                        //                            if (list.estmppS_PassFailFlg === "AB") {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_PassFailFlg;
                        //                            } else if (list.estmppS_PassFailFlg === "M") {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_ObtainedGrade +
                        //                                    "(" + list.estmppS_PassFailFlg + ")";
                        //                            } else if (list.estmppS_PassFailFlg === "Fail") {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_ObtainedGrade; + "*";
                        //                            } else {
                        //                                student["unique" + subject.ismS_Id + subexamname] = list.estmppS_ObtainedGrade;
                        //                            }
                        //                        }
                        //                    }
                        //                });
                        //            });
                        //        });

                        //        /* STUDENT WISE TOTAL , PERCENTAGE , RANK , RESULT */
                        //        angular.forEach($scope.details, function (student) {
                        //            angular.forEach($scope.studentwisetotal, function (dd) {
                        //                if (student.amsT_Id === dd.amsT_Id) {
                        //                    student.total = dd.estmpP_TotalObtMarks;
                        //                    student.percentage = dd.estmpP_Percentage;
                        //                    student.rank = dd.estmpP_SectionRank;
                        //                    student.result = dd.estmpP_Result;
                        //                }
                        //            });

                        //            // Individual Exam Wise Remarks
                        //            angular.forEach($scope.TempDistinct_Examnames, function (dd_exam) {
                        //                var examname = dd_exam.emE_ExamName + " Remarks";
                        //                var examid = dd_exam.emE_Id + "Remarks";
                        //                angular.forEach($scope.studentwise_individualexamremarks, function (dd_stu_remarks) {
                        //                    if (student.amsT_Id === dd_stu_remarks.amsT_Id && dd_exam.emE_Id === dd_stu_remarks.emE_Id) {
                        //                        student['IE_Remarks' + examid] = dd_stu_remarks.emeR_Remarks;
                        //                    }
                        //                });
                        //            });

                        //            //Promotion Wise Remarks
                        //            angular.forEach($scope.studenwise_remarks, function (dd_remarks) {
                        //                if (student.amsT_Id === dd_remarks.amsT_Id) {
                        //                    student.stu_pe_remarks = dd_remarks.eprD_PromotionName;
                        //                }
                        //            });
                        //        });

                        //        console.log($scope.details);
                        //    }
                        //    else {
                        //        swal("No Records Found");
                        //    }
                        //}

                        angular.forEach($scope.yearlist, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.classlist, function (dd) {
                            if (dd.asmcL_Id === parseInt($scope.asmcL_Id)) {
                                $scope.classname = dd.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.sectionlist, function (dd) {
                            if (dd.asmS_Id === parseInt($scope.asmS_Id)) {
                                $scope.sectionname = dd.asmC_SectionName;
                            }
                        });
                    } else {
                        swal("Promotion Settings Not Done For Selected Details");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId55").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/PromotionConsolidatedReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (table) {
            var data = "";
            if ($scope.dailybtedates === "overall") {
                data = "#A";
            } else {
                data = "#A1";
            }

            var exportHref = Excel.tableToExcel(data, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlistdetails, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlistdetails.every(function (itm) { return itm.checkedsub; });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlistdetails.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();