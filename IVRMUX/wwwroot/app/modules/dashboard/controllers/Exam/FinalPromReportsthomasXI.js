(function () {
    'use strict';
    angular.module('app').controller('FinalPromReportsthomasXIController', FinalPromReporssthomasIIIController)
    FinalPromReporssthomasIIIController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function FinalPromReporssthomasIIIController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.getstudentlist = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.tempsubject = [];
        var chart = {};
        var count = 0;
        $scope.SuperAverage = true;
        $scope.logo = true;
        $scope.Math = window.Math;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            $scope.imgname = admfigsettings[0].asC_Logo_Path;
        } else {
            $scope.imgname = "";
        }
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();

            $scope.generateddate = new Date($scope.today);
        };

        $scope.getserverdate();

        $scope.BindData = function () {
            var pageid = 2;
            count = 0;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            count = 0;
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.termlistd = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.class_list = [];
            $scope.termlist = [];
            $scope.AMST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            count = 0;
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.termlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.AMST_Id = "";
            $scope.section_list = [];
            $scope.getstudentlist = [];
            $scope.ASMS_Id = "";
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        $scope.onsectionchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.AMST_Id = "";
            count = 0;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("JSHSExamReports/StudentDetailspramotion", data).then(function (promise) {
                $scope.getstudentlist = promise.getstudentlist;
                $scope.all = true;

                angular.forEach($scope.getstudentlist, function (dd) {
                    dd.AMST_Ids = true;
                });
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            count = 0;
            $scope.studentdetails = [];
            $scope.empG_DistplayNametemp = "";
            $scope.tempsubject = [];
            //  var chart = {};
            $scope.Temp_AmstIds = [];
            $scope.getallgrade = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
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
            $scope.gettermlist = [];
            if ($scope.myForm.$valid) {

                $scope.getgroupdetails = [];
                $scope.getgroupdetailspp = [];
                $scope.termlisttemp = [];
                $scope.Temp_AmstId = [];
                $scope.processtotgraph = [];
                angular.forEach($scope.class_list, function (dd) {
                    if (dd.asmcL_Id == $scope.ASMCL_Id) {
                        $scope.classs = dd.asmcL_ClassName;
                    }
                });
                angular.forEach($scope.section_list, function (dd) {
                    if (dd.asmS_Id == $scope.ASMS_Id) {
                        $scope.clasection = dd.asmC_SectionName;
                    }
                });
                angular.forEach($scope.year_list, function (dd) {
                    if (dd.asmaY_Id == $scope.ASMAY_Id) {
                        $scope.Accdemic = dd.asmaY_Year;
                    }
                });
                angular.forEach($scope.getstudentlist, function (dd) {
                    if (dd.AMST_Ids) {
                        $scope.Temp_AmstId.push({ AMST_Id: dd.amsT_Id });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "Temp_AmstIds": $scope.Temp_AmstId
                };

                apiService.create("JSHSExamReports/St_Thomos_Ix2023", data).then(function (promise) {
                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                    $scope.JSHSReport = true;
                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                    $scope.gettermdetails = promise.gettermdetails;
                    $scope.gettermexamdetails = promise.gettermexamdetails;
                    $scope.getexamdetails = promise.getgroupdetails;
                    $scope.getgroupdetailspp = promise.getgroupdetails;
                    $scope.getgroupexamdetails = promise.getgroupexamdetails;
                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.studentdetailsgraph = [];
                    $scope.getallgrade = promise.getallgrade;
                    $scope.gettermlist = promise.gettermlist;

                    $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;
                    if ($scope.getexamwisetotaldetails != null && $scope.getexamwisetotaldetails.length > 0) {
                        angular.forEach($scope.getexamwisetotaldetails, function (rt) {
                            rt.TotalPercentage = $scope.Math.round(rt.TotalPercentage);
                            rt.totaver = $scope.Math.round(rt.totaver);
                        });
                    }
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
                    angular.forEach($scope.studentdetails, function (dd) {
                        $scope.studentdetailsgraph.push(
                            {
                                AMST_Id: dd.AMST_Id,

                            });
                    });
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;
                    $scope.studentwisemarks = promise.studentwisemarks;
                    $scope.stud_work_attendence = promise.work_attendence;
                    $scope.stud_present_attendence = promise.present_attendence;
                    $scope.processtot = promise.savelisttot;

                    // $scope.processtot = promise.examwiseremarks;
                    // Group Wise Exam List
                    $scope.studentsubgraph = [];
                    angular.forEach($scope.getgroupdetails, function (dd) {
                        $scope.groupwiseexamlist_temp = [];
                        //= dd.empG_DistplayName + "(50) +" + 
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
                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                        });
                        $scope.groupwiseexamlist_temp.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                            EME_ExamName: "AVG", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "AVG"
                        });
                        $scope.groupwiseexamlist.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                        });
                        $scope.groupwiseexamlist.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                            EME_ExamName: "AVG", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "AVG"
                        });
                        dd.empsG_MarksValue = counttotal;
                        dd.groupewiseexam = $scope.groupwiseexamlist_temp;
                    });
                    //ESTMPPSG_GroupObtMarks = 45
                    //
                    console.log($scope.getgroupdetails);
                    console.log($scope.groupwiseexamlist);
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
                    //angular.forEach($scope.studentdetails[0].studentsubjects, function (stu) {

                    //    $scope.tempsubjectwo.push({
                    //        ISMS_SubjectName: stu.ISMS_SubjectName,
                    //        ISMS_SubjectCode: stu.ISMS_SubjectCode
                    //    });
                    //    if (stu.ISMS_Id < 1000) {
                    //        $scope.tempsubject.push(stu.ISMS_SubjectName)
                    //    }

                    //})

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
                    // Over All Total 



                    angular.forEach($scope.getexamdetails, function (dd) {
                        $scope.groupexam = [];
                        angular.forEach($scope.getgroupexamdetails, function (d) {
                            if (d.empG_GroupName === dd.empG_GroupName) {
                                $scope.groupexam.push(d);
                            }
                        });

                        dd.groupexamdetails = $scope.groupexam;
                    });

                    $scope.marksdetails = [];
                    angular.forEach(promise.getstudentdetails, function (stu) {
                        $scope.marksdetails = [];
                        angular.forEach($scope.getstudentmarksdetails, function (subd) {
                            if (stu.AMST_Id === parseInt(subd.AMST_Id)) {
                                $scope.marksdetails.push(subd);
                            }
                        });
                        stu.marks = $scope.marksdetails;
                    });



                    //added By sanjeev
                    $scope.getstudent_examwisemarks = [];
                    $scope.getstudent_examwisemarks = promise.examwiseremarks;
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.student_wisemarks = [];
                        angular.forEach($scope.getstudent_examwisemarks, function (stusubj) {
                            if (stu.AMST_Id === stusubj.AMST_Id) {
                                $scope.student_wisemarks.push(stusubj);
                            }
                        });
                        stu.student_marks = $scope.student_wisemarks;
                    });
                    //added Co curculam
                    $scope.getskill = [];                    $scope.getallgrade = promise.getallgrade;                    angular.forEach(promise.getallgrade, function (dev) {                        if ($scope.getskill.length === 0) {                            $scope.getskill.push(dev);                        }                        else if ($scope.getskill.length > 0) {                            var intcount = 0;                            angular.forEach($scope.getskill, function (emp) {                                if (emp.ECACT_Id === dev.ECACT_Id) {                                    intcount += 1;                                }                            });                            if (intcount === 0) {                                $scope.getskill.push(dev);                            }                        }                    });
                });
            } else {
                $scope.submitted = true;
            }
        };
        $scope.bindCanvas = function (grp) {
            $scope.processtotgraph = [];
            $scope.processAverage = [];
            $scope.tempsubject = [];
            var ctx = document.getElementById("pieChart" + grp.AMST_Id).getContext('2d');
            if ($scope.getstudentwisesubjectlist != null && $scope.getstudentwisesubjectlist.length > 0) {
                angular.forEach($scope.getstudentwisesubjectlist, function (stusubj) {
                    if (stusubj.AMST_Id === grp.AMST_Id && stusubj.ISMS_Id < 1000) {
                        $scope.processtotgraph.push(stusubj.ESTMPPSG_GroupObtMarks);
                        $scope.processAverage.push(stusubj.GroupAVGMarks);
                        $scope.tempsubject.push(stusubj.ISMS_SubjectName);
                    }

                });
                var myChart = new Chart(ctx, {
                    type: 'bar',

                    data: {
                        labels: $scope.tempsubject,
                        datasets: [
                            {
                                label: 'Marks Obtained',
                                data: $scope.processtotgraph,
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
                                data: $scope.processAverage,
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
                            }
                        ],

                    },
                    options: {
                        responsive: true,
                        plugins: {
                            legend: {
                                position: 'top',
                            },
                            tooltip: {
                                enabled: true
                            },
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true,


                                }
                            }],
                            xAxis: {
                                ticks: {

                                    minRotation: 70,
                                    maxRotation: 70
                                }
                            }
                        }
                    }
                });

            }

        }
        $scope.print_HHS02 = function () {
            if (count > 0) {
                var innerContents = "";                innerContents = document.getElementById("print").innerHTML;                var popupWinindow = window.open('');                popupWinindow.document.open();                popupWinindow.document.write('<html><head>' +                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');                popupWinindow.document.close();
            }
            else {
                count = count + 1;
                angular.forEach($scope.studentdetailsgraph, function (dd) {
                    var canvas = document.getElementById("pieChart" + dd.AMST_Id);
                    dd.ImagePath = canvas.toDataURL();
                });
            }



        }
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getstudentlist.some(function (options) {
                return options.AMST_Ids;
            });
        };

        $scope.OnClickAll = function () {
            count = 0;
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
            angular.forEach($scope.getstudentlist, function (dd) {
                dd.AMST_Ids = $scope.all;
            });
        };

        $scope.individual = function () {
            count = 0;
            $scope.all = $scope.getstudentlist.every(function (itm) { return itm.AMST_Ids; });
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();