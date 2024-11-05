(function () {
    'use strict';
    angular.module('app').controller('PromotionReportI_VController', PromotionReportI_VController)

    PromotionReportI_VController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function PromotionReportI_VController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.promotionwiseremarks = false;
   
        $scope.Flag = "all";
        $scope.submitted = false;
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
        $scope.getstudentmarksdetails_temp = [];
        $scope.getreportdetails = true;
        $scope.details_report = true;
        $scope.subjectrank = true;
        $scope.Left_FlagAverage = false;
        var paginationformasters = '';
        var ivrmcofigsettings = [];
        var count = 0;
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


        $scope.SuperAverage = function () {
            if ($scope.Left_FlagAverage == true) {
                $scope.empG_GroupName = "";
            }
        }
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("PromotionReportDetails/getdata", pageid).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
            });
        };
        $scope.fonts = [
            { name: '10px', size: '10px ', class: 'font10' },
            { name: '11px', size: '11px ', class: 'font11' },
            { name: '12px', size: '12px ', class: 'font12' },
            { name: '13px', size: '13px ', class: 'font13' },
            { name: '14px', size: '14px ', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' },
            { name: '18px', size: '18px', class: 'font18' },
            { name: '25px', size: '25px', class: 'font25' }
        ];


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
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_GroupName = "";
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
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_GroupName = "";
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
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_GroupName = "";
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
                    $scope.subjectwisetotal = promise.subjectwisetotal;

                    $scope.all = true;
                    angular.forEach($scope.studentlistdetails, function (dd) {
                        dd.checkedsub = true;
                    });
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
            $scope.getstudentmarksdetails_temp = [];
            $scope.studentlistdetails = [];
            $scope.empG_GroupName = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("PromotionReportDetails/onchangesection", data).then(function (promise) {
                $scope.studentlistdetails = promise.studentlistdetails;
                 count = 0;
                $scope.all = true;
                angular.forEach($scope.studentlistdetails, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };

        $scope.saveddata = function (obj) {
            $scope.empG_DistplayNametemp = "";
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.getstudentwiseattendancedetails = [];
            $scope.submitted = true;
            $scope.studentwisemarks = [];
            $scope.studentdetails = [];
            $scope.getexamwisetotaldetails = [];
            $scope.ExamWise_PaperType = [];
            $scope.getparticipatedetails = [];
            $scope.Accdemic = "";
            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                $scope.Temp_AmstId = [];

                angular.forEach($scope.studentlistdetails, function (dd) {
                    if (dd.checkedsub) {
                        $scope.Temp_AmstId.push({ AMST_Id: dd.amsT_Id });
                    }
                });
                angular.forEach($scope.yearlist, function (dd) {
                    if (dd.asmaY_Id == $scope.asmaY_Id) {
                        $scope.Accdemic = dd.asmaY_Year;
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "Temp_AmstIds": $scope.Temp_AmstId,
                    "EMPG_GroupName": $scope.empG_GroupName,
                    "flag":"4"
                };

                apiService.create("JSHSExamReports/PromotionReportI_IV", data).then(function (promise) {
                    $scope.examwiseremarks = promise.examwiseremarks;
                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                    $scope.JSHSReport = true;
                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                    $scope.gettermdetails = promise.gettermdetails;
                    $scope.gettermexamdetails = promise.gettermexamdetails;
                    $scope.getgroupdetails = promise.getgroupdetails;
                    $scope.getgroupexamdetails = promise.getgroupexamdetails;
                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;
                    $scope.studentwisemarks = promise.getstudentwisetermwisedetails;
                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                    $scope.groupwiseexamlist_temp = [];
                    $scope.groupwiseexamlist = [];
                    $scope.getgradelist = [];
                    count = 0;
                    // /getgradelist
                    $scope.getgradelist = promise.getgradelist;
                    $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;
                    $scope.ExamWise_PaperType = promise.examWise_PaperType;
                    $scope.getparticipatedetails = promise.getparticipatedetails;
                    // Group Wise Exam List
                    angular.forEach($scope.getgroupdetails, function (dd) {
                        $scope.groupwiseexamlist_temp = [];
                        var counttotal = 0;
                        angular.forEach($scope.getgroupexamdetails, function (d) {
                            if (dd.empG_DistplayName === d.empG_DistplayName) {
                                counttotal += d.empsgE_ForMaxMarkrs;

                                $scope.groupwiseexamlist_temp.push({
                                    EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                    EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder, EMPG_DistplayName: d.empG_DistplayName,
                                    EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                    examnamedisplay: d.emE_ExamCode
                                });

                                $scope.groupwiseexamlist.push({
                                    EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                    EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder, EMPG_DistplayName: d.empG_DistplayName,
                                    EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                    examnamedisplay: d.emE_ExamCode
                                });
                            }
                        });

                        $scope.groupwiseexamlist_temp.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000,
                            EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal,
                            examnamedisplay: 'Total',
                            columname: ""


                        });


                        $scope.groupwiseexamlist_temp.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 98000012,
                            EME_ExamName: "Highest in Class", EME_ExamOrder: 98000012, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: null,
                            examnamedisplay: "Highest in Class",
                            columname: ""
                        });
                        $scope.groupwiseexamlist.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000,
                            EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total',
                            columname: "Marks",
                        });


                        $scope.groupwiseexamlist.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 98000012,
                            EME_ExamName: "Highest in Class", EME_ExamOrder: 98000012,
                            EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: null, examnamedisplay: "Highest in Class"
                        });

                        dd.empsG_MarksValue = counttotal;
                        dd.groupewiseexam = $scope.groupwiseexamlist_temp;
                    });

                    console.log($scope.getgroupdetails);
                    console.log($scope.groupwiseexamlist);

                    console.log($scope.studentdetails);

                    // Student Wise Subject List
                    $scope.studenwisesubjects = [];
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.studenwisesubjects = [];
                        angular.forEach($scope.getstudentwisesubjectlist, function (stusubj) {
                            if (stu.AMST_Id === stusubj.AMST_Id) {
                                $scope.studenwisesubjects.push(stusubj);
                            }
                        });
                        stu.studentsubjects = $scope.studenwisesubjects;
                    });

                    //Student Wise Marks List
                    $scope.studenwisemarks = [];
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.studenwisemarks = [];
                        angular.forEach($scope.getstudentmarksdetails, function (stusubj) {
                            if (stu.AMST_Id === stusubj.AMST_Id) {
                                $scope.studenwisemarks.push(stusubj);
                            }
                        });
                        stu.studentmarks = $scope.studenwisemarks;
                    });
                    //Total
                    $scope.getstudent_examwisemarks = [];
                    $scope.getstudent_examwisemarks = promise.getstudent_examwisemarks;
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.student_wisemarks = [];
                        angular.forEach($scope.getstudent_examwisemarks, function (stusubj) {
                            if (stu.AMST_Id === stusubj.AMST_Id) {
                                $scope.student_wisemarks.push(stusubj);
                            }
                        });
                        stu.student_marks = $scope.student_wisemarks;
                    });

                    

                    //$scope.skilllist_temp = [];
                    //angular.forEach($scope.studentdetails, function (stu) {
                    //    $scope.skilllist_temp = [];
                    //    var count = 0;
                    //    angular.forEach($scope.getstudentwiseskillslist, function (dd) {
                    //        if (stu.AMST_Id === dd.AMST_Id) {
                    //            if ($scope.skilllist_temp.length === 0) {
                    //                $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName });
                    //            } else if ($scope.skilllist_temp.length > 0) {
                    //                count = 0;
                    //                angular.forEach($scope.skilllist_temp, function (ddd) {
                    //                    if (ddd.ECS_SkillName === dd.ECS_SkillName) {
                    //                        count += 1;
                    //                    }
                    //                });
                    //                if (count === 0) {
                    //                    $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName });
                    //                }
                    //            }
                    //        }
                    //    });
                    //    stu.skill_main_list = $scope.skilllist_temp;
                    //});

                    //angular.forEach($scope.studentdetails, function (stu) {
                    //    $scope.skill_main_list_array = [];
                    //    angular.forEach(stu.skill_main_list, function (skill) {
                    //        $scope.skill_main_list_array = [];
                    //        angular.forEach($scope.gettermdetails, function (term) {
                    //            angular.forEach($scope.getstudentwiseskillslist, function (dd) {
                    //                if (dd.AMST_Id === stu.AMST_Id && skill.ECS_SkillName === dd.ECS_SkillName && term.ecT_Id === dd.ECT_Id) {

                    //                    $scope.skill_main_list_array.push({ scoregrade: dd.EMGD_Name, ecT_Id: dd.ECT_Id });
                    //                }
                    //            });
                    //        });
                    //        skill.skill_score_details = $scope.skill_main_list_array;
                    //    });
                    //});

                   
                    //$scope.activitylist_temp = [];

                    //angular.forEach($scope.studentdetails, function (stu) {
                    //    $scope.activitylist_temp = [];
                    //    var count1 = 0;
                    //    angular.forEach($scope.getstudentwiseactiviteslist, function (act) {
                    //        if (act.AMST_Id === stu.AMST_Id) {
                    //            if ($scope.activitylist_temp.length === 0) {
                    //                $scope.activitylist_temp.push({ ECACTA_SkillArea: act.ECACTA_SkillArea });
                    //            } else if ($scope.activitylist_temp.length > 0) {
                    //                count1 = 0;
                    //                angular.forEach($scope.activitylist_temp, function (dd) {
                    //                    if (dd.ECACTA_SkillArea === act.ECACTA_SkillArea) {
                    //                        count1 += 1;
                    //                    }
                    //                });

                    //                if (count1 === 0) {
                    //                    $scope.activitylist_temp.push({ ECACTA_SkillArea: act.ECACTA_SkillArea });
                    //                }
                    //            }
                    //        }
                    //    });
                    //    stu.activity_main_list = $scope.activitylist_temp;
                    //});

                    //$scope.activity_main_list_array = [];
                    //angular.forEach($scope.studentdetails, function (stu) {
                    //    $scope.activity_main_list_array = [];
                    //    angular.forEach(stu.activity_main_list, function (act) {
                    //        $scope.activity_main_list_array = [];
                    //        angular.forEach($scope.gettermdetails, function (term) {
                    //            angular.forEach($scope.getstudentwiseactiviteslist, function (dd) {
                    //                if (stu.AMST_Id === dd.AMST_Id && act.ECACTA_SkillArea === dd.ECACTA_SkillArea && term.ecT_Id === dd.ECT_Id) {
                    //                    $scope.activity_main_list_array.push({ EMGD_Name: dd.EMGD_Name, ecT_Id: dd.ECT_Id });
                    //                }
                    //            });
                    //        });
                    //        act.activity_score_details = $scope.activity_main_list_array;
                    //    });
                    //});

                    $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;

                    angular.forEach($scope.studentdetails, function (stu) {
                        angular.forEach($scope.getpromotionremarksdetails, function (dd) {
                            if (stu.AMST_Id === dd.amsT_Id) {
                                stu.remarks = dd.eprD_Remarks;
                                stu.promotedclass = dd.eprD_ClassPromoted;
                            }
                        });
                    });
                    //attendence
                    $scope.attendancedetails = [];
                    angular.forEach($scope.studentdetails, function (dd) {
                        $scope.attendancedetails = [];
                        angular.forEach($scope.getstudentwiseattendancedetails, function (d) {
                            if (dd.AMST_Id === d.AMST_Id) {
                                $scope.attendancedetails.push({
                                    classheld: d.TOTALWORKINGDAYS,
                                    emE_Id: d.EME_Id,
                                    classatt: d.PRESENTDAYS,
                                    percentage: d.ATTENDANCEPERCENTAGE
                                });
                            }
                        });
                        dd.attendance = $scope.attendancedetails;
                    });
                    $scope.examwiseremarksremarks = [];
                    $scope.examwiseremarksremarks = promise.getpromotionremarksdetails;
                    //examwiseremarks
                    // $scope.examwiseremarksremarks = [];
                    $scope.studentdetailsgraph = [];

                    angular.forEach($scope.studentdetails, function (dd) {
                        $scope.studentdetailsgraph.push(
                            {
                                AMST_Id: dd.AMST_Id,
                                chart: "chart" + dd.AMST_Id,
                                template: ' <div id = "chart' + dd.AMST_Id + '" style = "height: 360px; width: 100%;"></div>'

                            });
                    });
                    console.log($scope.studentdetails);
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.bindCanvas = function (grp) {
            $scope.classHighest = [];
            $scope.AverageHighest = [];
            $scope.subjectHighest = [];
            $scope.tempsubject = [];
            // $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;
            // $scope.ExamWise_PaperType = promise.examWise_PaperType;
            //  $scope.getparticipatedetails = promise.getparticipatedetails;

            var ctx = document.getElementById("pieChart" + grp.AMST_Id).getContext('2d');
            if ($scope.getstudentwisesubjectlist != null && $scope.getstudentwisesubjectlist.length > 0) {

                angular.forEach($scope.getstudentwisesubjectlist, function (stusubj) {
                    if (stusubj.AMST_Id === grp.AMST_Id && stusubj.EYCES_MarksDisplayFlg == true) {

                        $scope.tempsubject.push(stusubj.ISMS_SubjectName);
                    }
                });
                if ($scope.getexamwisetotaldetails != null && $scope.getexamwisetotaldetails.length > 0) {
                    angular.forEach($scope.getexamwisetotaldetails, function (stusubj) {
                        if (stusubj.AMST_Id === grp.AMST_Id && stusubj.EYCES_MarksDisplayFlg == true) {
                            $scope.classHighest.push(stusubj.ESTMPPS_ClassHighest);
                            $scope.AverageHighest.push(stusubj.ESTMPPS_SectionAverage);
                            $scope.subjectHighest.push(stusubj.ObtainedMarks);
                        }
                    });
                }
                //if ($scope.ExamWise_PaperType != null && $scope.ExamWise_PaperType.length > 0) {
                //    angular.forEach($scope.ExamWise_PaperType, function (stusubj) {
                //        if (stusubj.AMST_Id === grp.AMST_Id ) {
                //            $scope.AverageHighest.push(stusubj.ESTMPPS_ClassHighest);

                //        }
                //    });
                //}
                //if ($scope.getparticipatedetails != null && $scope.getparticipatedetails.length > 0) {
                //    angular.forEach($scope.getparticipatedetails, function (stusubj) {
                //        if (stusubj.AMST_Id === grp.AMST_Id && stusubj.EYCES_MarksDisplayFlg == true) {
                //            $scope.subjectHighest.push(stusubj.ObtainedMarks);

                //        }
                //    });
                //}
                var myChart = new Chart(ctx, {
                    type: 'bar',

                    data: {
                        labels: $scope.tempsubject,
                        datasets: [
                            {
                                label: 'Highest Marks',
                                data: $scope.classHighest,
                                backgroundColor: [
                                    'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)', 'rgb(63,81,181)'
                                ],
                                borderColor: [
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                ],
                                borderWidth: 1
                            },
                            {
                                label: 'Average Marks',
                                data: $scope.AverageHighest,
                                backgroundColor: [
                                    'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)', 'rgb(255,87,34)'
                                ],
                                borderColor: [
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                ],
                                borderWidth: 0
                            },
                            {
                                label: 'Obtained Marks',
                                data: $scope.subjectHighest,
                                backgroundColor: [
                                    'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,04)', 'rgb(0,255,0)', 'rgb(0,255,0)', 'rgb(0,255,04)'
                                ],
                                borderColor: [
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                    'black',
                                ],
                                borderWidth: 0
                            }
                        ],

                    },
                    options: {
                        responsive: true,
                        plugins: {
                            legend: {
                                display: false,
                                position: 'bottom',

                            },
                            tooltip: {
                                enabled: true
                            },
                        },
                        //added by adarsh
                        animation: {
                            duration: 1,
                            onComplete: function ({ chart }) {
                                const ctx = chart.ctx;

                                chart.config.data.datasets.forEach(function (adar, ms) {
                                    const meta = chart.getDatasetMeta(ms);

                                    meta.data.forEach(function (bar, index) {
                                        const data = adar.data[index];

                                        ctx.fillText(data, bar.x, bar.y - 5);
                                    });
                                });
                            }
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true,


                                }
                            }],
                            xAxis: {
                                ticks: {

                                    minRotation: 25,
                                    maxRotation: 25
                                }
                            }
                        }
                    }
                });

            }

        }

        $scope.printToCart = function () {
            if (count > 0) {
                var innerContents = document.getElementById("HHS02").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');                popupWinindow.document.close();
            }
            else {
                count = count + 1;
                angular.forEach($scope.studentdetailsgraph, function (dd) {
                    var canvas = document.getElementById("pieChart" + dd.AMST_Id);
                    dd.ImagePath = canvas.toDataURL();
                });
            }
        }
        $scope.exportToExcel = function (tableIds) {

            var excelname = "Tabulation Sheet ";
            var exportHref = Excel.tableToExcel(tableIds, 'Tabulation Sheet');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.termChange = function () {
            $scope.getstudentmarksdetails_temp = [];
            count = 0;
        }
        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlistdetails, function (dd) {
                dd.checkedsub = $scope.all;
            });
            $scope.getstudentmarksdetails_temp = [];
            count = 0;
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlistdetails.every(function (itm) { return itm.checkedsub; });
            $scope.getstudentmarksdetails_temp = [];
            count = 0;
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