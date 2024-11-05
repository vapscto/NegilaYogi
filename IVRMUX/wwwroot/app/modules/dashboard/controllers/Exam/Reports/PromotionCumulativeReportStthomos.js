(function () {
    'use strict';
    angular.module('app').controller('PromotionStthomosController', PromotionCumulativeReportController)

    PromotionCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function PromotionCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.promotionwiseremarks = false;
        $scope.examwiseremarks = false;
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

                $scope.all = true;
                angular.forEach($scope.studentlistdetails, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };
        $scope.getpromotioncumulativereport = function () {
            $scope.submitted = true;
            $scope.getstudentmarksdetails_temp = [];
            if ($scope.myForm.$valid) {
                if ($scope.Left_FlagAverage == true) {
                    $scope.getpromotioncumulativeeFlagNew();
                    
                }
                else {
                    
                    $scope.getpromotioncumulativeFlag();
                }
            }

        }

        $scope.getpromotioncumulativeeFlagNew = function () {
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_DistplayNametemp = "";
            $scope.tempsubject = [];
            $scope.Temp_AmstIds = [];
            $scope.getallgrade = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.getgroupdetails = [];
            $scope.getgroupdetailspp = [];
            $scope.termlisttemp = [];
            $scope.Temp_AmstId = [];
            $scope.processtotgraph = [];
            $scope.studentwisemarks = [];
            $scope.groupwiseexamlist_temp = [];
            $scope.groupwiseexamlist = [];
            $scope.stud_present_attendence = [];
            $scope.stud_work_attendence = [];
            $scope.processtot = [];
            $scope.piotsubjets = [];
            $scope.Accdemic = "";
            $scope.classs = "";
            $scope.clasection = "";
            $scope.studenwisesubjectgraph = [];
            $scope.tempsubjectwo = [];
            $scope.processtotgraph = [];
            $scope.tempstudentdetails = [];
            $scope.studentdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.St_ThomosTotal = [];
            angular.forEach($scope.studentlistdetails, function (dd) {
                if (dd.checkedsub) {                   
                    $scope.tempstudentdetails.push({ AMST_Id: dd.amsT_Id });
                }
            });
           
            $scope.YearName = "";
            $scope.stream = "";
            $scope.section = "";
            angular.forEach($scope.yearlist, function (dd) {
                if (dd.asmaY_Id == $scope.asmaY_Id) {
                    $scope.YearName = dd.asmaY_Year;
                }
            });
            angular.forEach($scope.classlist, function (dd) {
                if (dd.asmcL_Id == $scope.asmcL_Id) {
                    $scope.stream = dd.asmcL_ClassName;
                }
            });
            angular.forEach($scope.sectionlist, function (dd) {
                if (dd.asmS_Id == $scope.asmS_Id) {
                    $scope.section = dd.asmC_SectionName;
                }
            });
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Temp_AmstIds": $scope.tempstudentdetails,
                "EMPG_GroupName": $scope.empG_GroupName,
                "reporttype": "superaverage",
            };
            apiService.create("JSHSExamReports/Pramotion_report", data).then(function (promise) {
                if (promise.getstudentmarksdetails != null && promise.getstudentmarksdetails.length > 0) {   
                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;                         
                    $scope.St_ThomosTotal = promise.st_ThomosTotal;                         
                    
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    //piotsubjets
                    $scope.colspan = $scope.getstudentwisesubjectlist.length;
                    $scope.processtot = promise.savelisttot;  
                }
                else {
                    swal("Record Not Found !")
                }
            });
        }
        $scope.getpromotioncumulativeFlag = function () {          
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_DistplayNametemp = "";
            $scope.tempsubject = [];
            $scope.Temp_AmstIds = [];
            $scope.getallgrade = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.getgroupdetails = [];
            $scope.getgroupdetailspp = [];
            $scope.termlisttemp = [];
            $scope.Temp_AmstId = [];
            $scope.processtotgraph = [];
            $scope.studentwisemarks = [];
            $scope.groupwiseexamlist_temp = [];
            $scope.groupwiseexamlist = [];
            $scope.stud_present_attendence = [];
            $scope.stud_work_attendence = [];
            $scope.processtot = [];
            $scope.Accdemic = "";
            $scope.classs = "";
            $scope.clasection = "";
            $scope.studenwisesubjectgraph = [];
            $scope.tempsubjectwo = [];
            $scope.processtotgraph = [];
            $scope.tempstudentdetails = [];
            $scope.studentdetails = [];
            angular.forEach($scope.studentlistdetails, function (dd) {
                if (dd.checkedsub) {
                    // $scope.tempstudentdetails.push(dd.amsT_Id);
                    $scope.tempstudentdetails.push({ AMST_Id: dd.amsT_Id });
                }
            });
            //yearlist
            $scope.YearName = "";
            $scope.stream = "";
            $scope.section = "";
            angular.forEach($scope.yearlist, function (dd) {
                if (dd.asmaY_Id == $scope.asmaY_Id) {
                    $scope.YearName = dd.asmaY_Year;
                }
            });
            angular.forEach($scope.classlist, function (dd) {
                if (dd.asmcL_Id == $scope.asmcL_Id) {
                    $scope.stream = dd.asmcL_ClassName;
                }
            });
            angular.forEach($scope.sectionlist, function (dd) {
                if (dd.asmS_Id == $scope.asmS_Id) {
                    $scope.section = dd.asmC_SectionName;
                }
            });
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Temp_AmstIds": $scope.tempstudentdetails,
                "EMPG_GroupName": $scope.empG_GroupName,
                "reporttype": "",
            };
            apiService.create("JSHSExamReports/Pramotion_report", data).then(function (promise) {
                if (promise.getstudentmarksdetails != null && promise.getstudentmarksdetails.length > 0) {
                    $scope.instname = promise.getinstitution;
                    $scope.inst_name = $scope.instname !== null && $scope.instname.length > 0 ? $scope.instname[0].mI_Name : "";
                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                    $scope.JSHSReport = true;
                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                    $scope.gettermdetails = promise.gettermdetails;
                    $scope.gettermexamdetails = promise.gettermexamdetails;
                    $scope.getgroupdetailspp = promise.getgroupdetails;
                    $scope.getgroupexamdetails = promise.getgroupexamdetails;
                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.studentdetailsgraph = [];
                    $scope.getallgrade = promise.getallgrade;
                    if ($scope.getgroupdetailspp != null && $scope.getgroupdetailspp.length > 0) {
                        angular.forEach($scope.getgroupdetailspp, function (t3) {
                            var al_cnt = 0;
                            angular.forEach($scope.getgroupdetails, function (rt) {
                                if (rt.empG_DistplayName === t3.empG_DistplayName) {
                                    al_cnt += 1;
                                }
                            });
                            if (al_cnt === 0) {
                                $scope.getgroupdetails.push({
                                    empG_GroupName: t3.empG_GroupName,
                                    empsG_Order: t3.empsG_Order,
                                    empG_DistplayName: t3.empG_DistplayName,
                                });
                            }
                        });

                    }
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

                    $scope.stud_work_attendence = promise.work_attendence;
                    $scope.stud_present_attendence = promise.present_attendence;

                    $scope.processtot = promise.savelisttot;

                    $scope.studentsubgraph = [];
                    angular.forEach($scope.getgroupdetails, function (dd) {
                        $scope.groupwiseexamlist_temp = [];

                        var counttotal = 0;
                        angular.forEach($scope.getgroupexamdetails, function (d) {
                            if (dd.empG_DistplayName === d.empG_DistplayName) {
                                counttotal += d.empsgE_ForMaxMarkrs;
                                $scope.groupwiseexamlist_temp.push({
                                    EMPG_GroupName: d.empG_GroupName,
                                    EME_Id: d.emE_Id,
                                    EME_ExamName: d.emE_ExamName,
                                    EME_ExamOrder: d.emE_ExamOrder,
                                    EMPG_DistplayName: d.empG_DistplayName,
                                    EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                    examnamedisplay: d.emE_ExamCode
                                });
                                $scope.groupwiseexamlist.push({
                                    EMPG_GroupName: d.empG_GroupName, EME_Id: d.emE_Id,
                                    EME_ExamName: d.emE_ExamName, EME_ExamOrder: d.emE_ExamOrder,
                                    EMPG_DistplayName: d.empG_DistplayName,
                                    EMPSGE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                    examnamedisplay: d.emE_ExamCode
                                });
                            }
                        });
                        $scope.groupwiseexamlist_temp.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                        });


                        $scope.groupwiseexamlist.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                        });

                        dd.empsG_MarksValue = counttotal;
                        dd.groupewiseexam = $scope.groupwiseexamlist_temp;
                    });

                    console.log($scope.getgroupdetails);
                    console.log($scope.groupwiseexamlist);

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
                    angular.forEach($scope.studentdetails[0].studentsubjects, function (stu) {
                        $scope.tempsubjectwo.push({
                            ISMS_SubjectName: stu.ISMS_SubjectName,
                            ISMS_SubjectCode: stu.ISMS_SubjectCode,
                            ISMS_Id: stu.ISMS_Id,
                        });
                        $scope.tempsubject.push(stu.ISMS_SubjectName)
                    })

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
                    // STUDENT WISE SKILL  LIST
                    $scope.skilllist_temp = [];
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.skilllist_temp = [];
                        var count = 0;
                        angular.forEach($scope.getstudentwiseskillslist, function (dd) {
                            if (stu.AMST_Id === dd.AMST_Id) {
                                if ($scope.skilllist_temp.length === 0) {
                                    $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName });
                                } else if ($scope.skilllist_temp.length > 0) {
                                    count = 0;
                                    angular.forEach($scope.skilllist_temp, function (ddd) {
                                        if (ddd.ECS_SkillName === dd.ECS_SkillName) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName });
                                    }
                                }
                            }
                        });
                        stu.skill_main_list = $scope.skilllist_temp;
                    });
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.skill_main_list_array = [];
                        angular.forEach(stu.skill_main_list, function (skill) {
                            $scope.skill_main_list_array = [];
                            angular.forEach($scope.gettermdetails, function (term) {
                                angular.forEach($scope.getstudentwiseskillslist, function (dd) {
                                    if (dd.AMST_Id === stu.AMST_Id && skill.ECS_SkillName === dd.ECS_SkillName && term.ecT_Id === dd.ECT_Id) {
                                        //$scope.skill_main_list_array.push({ scorename: dd.ECSA_SkillArea, ecT_Id: dd.ECT_Id });
                                        $scope.skill_main_list_array.push({ scoregrade: dd.EMGD_Name, ecT_Id: dd.ECT_Id });
                                    }
                                });
                            });
                            skill.skill_score_details = $scope.skill_main_list_array;
                        });
                    });
                    // Student Wise Discipline List
                    $scope.activitylist_temp = [];
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.activitylist_temp = [];
                        var count1 = 0;
                        angular.forEach($scope.getstudentwiseactiviteslist, function (act) {
                            if (act.AMST_Id === stu.AMST_Id) {
                                if ($scope.activitylist_temp.length === 0) {
                                    $scope.activitylist_temp.push({ ECACTA_SkillArea: act.ECACTA_SkillArea });
                                } else if ($scope.activitylist_temp.length > 0) {
                                    count1 = 0;
                                    angular.forEach($scope.activitylist_temp, function (dd) {
                                        if (dd.ECACTA_SkillArea === act.ECACTA_SkillArea) {
                                            count1 += 1;
                                        }
                                    });

                                    if (count1 === 0) {
                                        $scope.activitylist_temp.push({ ECACTA_SkillArea: act.ECACTA_SkillArea });
                                    }
                                }
                            }
                        });
                        stu.activity_main_list = $scope.activitylist_temp;
                    });
                    $scope.activity_main_list_array = [];
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.activity_main_list_array = [];
                        angular.forEach(stu.activity_main_list, function (act) {
                            $scope.activity_main_list_array = [];
                            angular.forEach($scope.gettermdetails, function (term) {
                                angular.forEach($scope.getstudentwiseactiviteslist, function (dd) {
                                    if (stu.AMST_Id === dd.AMST_Id && act.ECACTA_SkillArea === dd.ECACTA_SkillArea && term.ecT_Id === dd.ECT_Id) {
                                        $scope.activity_main_list_array.push({ EMGD_Name: dd.EMGD_Name, ecT_Id: dd.ECT_Id });
                                    }
                                });
                            });
                            act.activity_score_details = $scope.activity_main_list_array;
                        });
                    });
                    $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                    angular.forEach($scope.studentdetails, function (stu) {
                        angular.forEach($scope.getpromotionremarksdetails, function (dd) {
                            if (stu.AMST_Id === dd.amsT_Id) {
                                stu.remarks = dd.eprD_Remarks;
                                stu.promotedclass = dd.eprD_ClassPromoted;
                            }
                        });
                    });
                    console.log($scope.studentdetails);
                    $scope.getskill = [];                    $scope.getallgrade = promise.getallgrade;                    console.log($scope.getallgrade);                    angular.forEach(promise.getallgrade, function (dev) {                        if ($scope.getskill.length === 0) {                            $scope.getskill.push(dev);                        }                        else if ($scope.getskill.length > 0) {                            var intcount = 0;                            angular.forEach($scope.getskill, function (emp) {                                if (emp.ECACT_Id === dev.ECACT_Id) {                                    intcount += 1;                                }                            });                            if (intcount === 0) {                                $scope.getskill.push(dev);                            }                        }                    });
                    $scope.piotsubjets = [];
                   
                    angular.forEach($scope.processtot, function (stu) {
                        angular.forEach($scope.groupwiseexamlist, function (stusubj) {
                            if (stu.Flag == 0) {
                                $scope.piotsubjets.push({
                                    ISMS_SubjectName: stu.ISMS_SubjectName + '(' + stusubj.EMPSGE_ForMaxMarkrs + ')',
                                    ISMS_Id: stu.ISMS_Id,
                                    Flag: stu.Flag,
                                    SubjectName: stu.ISMS_SubjectName,

                                })

                            }
                            else {
                                if (stusubj.EME_Id == 9800000) {
                                    $scope.piotsubjets.push({
                                        ISMS_SubjectName: stu.ISMS_SubjectName + '(' + stusubj.EMPSGE_ForMaxMarkrs + ')',
                                        ISMS_Id: stu.ISMS_Id,
                                        Flag: stu.Flag,
                                        SubjectName: stu.ISMS_SubjectName,
                                    })
                                }

                            }

                        });

                    });
                    $scope.colspan2 = $scope.piotsubjets.length;
                    angular.forEach($scope.getstudentmarksdetails_temp, function (stu) {
                        angular.forEach(promise.getstudent_examwisemarks, function (stusubj) {
                            if (stu.AMST_Id == stusubj.AMST_Id && stusubj.EME_Id == 9800000) {
                                stu.TotalMarks = stusubj.ObtainedMarks;
                                stu.TotalPercentage = stusubj.TotalPercentage;
                            }

                        });

                    });

                    $scope.bindCanvas();
                }
                else {
                    swal("Record Not Found !")
                }
            });

        };

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
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
        }
        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlistdetails, function (dd) {
                dd.checkedsub = $scope.all;
            });
            $scope.getstudentmarksdetails_temp = [];
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlistdetails.every(function (itm) { return itm.checkedsub; });
            $scope.getstudentmarksdetails_temp = [];
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