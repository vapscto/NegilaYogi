(function () {
    'use strict';
    angular.module('app').controller('NDSStudentProgressCardController', NDSStudentProgressCardController)

    NDSStudentProgressCardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$compile']
    function NDSStudentProgressCardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $compile) {

        $scope.percounttotal = 0;
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.HHS_I_IV_grid = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.grandfinaltotal = 0;
        $scope.grandfinalmaxtotal = 0;
        $scope.grandtotalperc = 0;
        //TO  GEt The Values iN Grid
        $scope.grandavgtotal = 0;
        $scope.exm_sub_mrks_list = [];

        $scope.halfyearatt = [];
        $scope.fullyearatt = [];
        $scope.datesuf1 = "";
        var frommonth = "";

        $scope.BindData = function () {
            var pageid = 2;

            apiService.getURI("StudentProgressCardReport/stmarygetdetails", pageid).then(function (promise) {
                $scope.btn = false;
                $scope.getstudentdetails = promise.getstudentdetails;
                $scope.year_list = promise.getyear;
                $scope.classlist = promise.getclass;
                $scope.examorterm = promise.examorterm;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                    $scope.ASMCL_Id = $scope.getstudentdetails[0].asmcL_Id;
                    $scope.asmS_Id = $scope.getstudentdetails[0].asmS_Id;
                    $scope.asmaY_Id = $scope.getstudentdetails[0].asmaY_Id;

                    if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        $scope.examflag = false;
                        $scope.termflag = false;
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
            $scope.HHS_I_IV_grid = false;
            $scope.termflag = false;
            $scope.reportshowcount = 0;
            $scope.btn = false;
            if ($scope.ASMCL_Id !== undefined && $scope.ASMCL_Id !== null && $scope.ASMCL_Id !== "") {
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id
                };

                apiService.create("StudentProgressCardReport/stmaryonchangeclass", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getstudentdetails = promise.getstudentdetails;

                        if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                            $scope.examorterm = promise.examorterm;

                            if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                                $scope.getexamtermlist = promise.getexamtermlist;
                                $scope.examflag = false;
                                $scope.termflag = false;
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
            }
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

        $scope.NDS_Get_Progresscard_Report = function () {
            $scope.HHS_I_IV_grid = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.grandavgtotal = 0;

                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id
                };


                apiService.create("StudentProgressCardReport/NDS_Get_Progresscard_Report", data).then(function (promise) {

                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                    $scope.examflag_Temp = promise.examflag;
                    if ($scope.examflag_Temp === 1 || $scope.examflag_Temp === 2 || $scope.examflag_Temp === 3) {
                        if ($scope.getstudentmarksdetails_temp !== null && $scope.getstudentmarksdetails_temp.length > 0) {
                            $scope.HHS_I_IV_grid = true;
                            $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;

                            $scope.gettermdetails = promise.gettermdetails;
                            $scope.gettermexamdetails = promise.gettermexamdetails;
                            $scope.getgroupdetails = promise.getgroupdetails;
                            $scope.getgroupexamdetails = promise.getgroupexamdetails;
                            $scope.studentdetails = promise.getstudentdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                            $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;
                            $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                            $scope.getstudentwisesportsdetails = promise.getstudentwisesportsdetails;

                            $scope.groupwiseexamlist_temp = [];
                            $scope.groupwiseexamlist = [];


                            if ($scope.examflag_Temp === 3) {
                                $scope.getgroupdetails.push({
                                    empG_DistplayName: "Marks Obtained", empsG_GroupName: "Marks Obtained", empsG_MarksValue: 100
                                });

                                $scope.getgroupdetails.push({
                                    empG_DistplayName: "Grade", empsG_GroupName: "Grade", empsG_MarksValue: ""
                                });
                            }
                            // Group Wise Exam List
                            if ($scope.examflag_Temp === 2 || $scope.examflag_Temp === 1) {
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

                                    if ($scope.examflag_Temp === 2) {
                                        $scope.groupwiseexamlist_temp.push({
                                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                                        });
                                    }

                                    if ($scope.examflag_Temp === 2 || $scope.examflag_Temp === 1) {
                                        $scope.groupwiseexamlist_temp.push({
                                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                                            EME_ExamName: "Grade", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "Grade"
                                        });
                                    }

                                    if ($scope.examflag_Temp === 2) {
                                        $scope.groupwiseexamlist.push({
                                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800000,
                                            EME_ExamName: "Marks Obtained", EME_ExamOrder: 9800000, EMPG_DistplayName: dd.empG_DistplayName,
                                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: 'Total'
                                        });
                                    }
                                    if ($scope.examflag_Temp === 2 || $scope.examflag_Temp === 1) {
                                        $scope.groupwiseexamlist.push({
                                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                                            EME_ExamName: "Grade", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "Grade"
                                        });
                                    }
                                    dd.empsG_MarksValue = counttotal;
                                    dd.groupewiseexam = $scope.groupwiseexamlist_temp;
                                });
                            }

                            console.log($scope.getgroupdetails);
                            console.log($scope.groupwiseexamlist);

                            // Student Wise Subject List

                            $scope.stuname = $scope.studentdetails[0].studentname;
                            $scope.asmcL_ClassName = $scope.studentdetails[0].classname;
                            $scope.asmC_SectionName = $scope.studentdetails[0].sectionname;
                            $scope.amaY_RollNo = $scope.studentdetails[0].rollno;
                            $scope.dob = $scope.studentdetails[0].dob;
                            $scope.ASMAY_Year = $scope.studentdetails[0].ASMAY_Year;
                            $scope.amsT_PerStreet = $scope.studentdetails[0].AMST_PerStreet;
                            $scope.amsT_PerArea = $scope.studentdetails[0].AMST_PerArea;
                            $scope.amsT_PerCity = $scope.studentdetails[0].AMST_PerCity;
                            $scope.ivrmmS_Name = $scope.studentdetails[0].ivrmms_name;
                            $scope.ivrmmC_CountryName = $scope.studentdetails[0].IVRMMC_CountryName;
                            $scope.addressd1 = $scope.studentdetails[0].addressd1;
                            $scope.amsT_PerPincode = $scope.studentdetails[0].amsT_PerPincode;
                            $scope.mobileno = $scope.studentdetails[0].mobileno;
                            $scope.AMST_Photoname = $scope.studentdetails[0].AMST_Photoname;
                            $scope.amst_dob = new Date($scope.studentdetails[0].amst_dob);
                            $scope.housename = $scope.studentdetails[0].SPCCMH_HouseName;

                            $scope.getdate = $scope.amst_dob.getDate();
                            $scope.getyear = $scope.amst_dob.getFullYear();
                            var months = $scope.amst_dob.getMonth();

                            $scope.index = $scope.ordinal_suffix_of1($scope.getdate);
                            $scope.getmonth = $scope.getmontnames(months);

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

                            $scope.getsubjectwisetotaldetails = promise.getsubjectwisetotaldetails;

                            angular.forEach($scope.studentdetails, function (stu) {
                                angular.forEach(stu.studentsubjects, function (stusubj) {
                                    angular.forEach($scope.getsubjectwisetotaldetails, function (dd) {
                                        if (stusubj.ISMS_Id === dd.ismS_Id) {
                                            stusubj.marksobtained = dd.estmppS_ObtainedMarks;
                                            stusubj.gradeobtained = dd.estmppS_ObtainedGrade;
                                            stusubj.passfailflag = dd.estmppS_PassFailFlg;
                                            stusubj.obtainedgradepoints = dd.estmppS_GradePoints;
                                        }
                                    });
                                });
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

                            if (promise.examflag === 1) {
                                // Skills List
                                $scope.skill_list = [];
                                angular.forEach($scope.getstudentwiseskillslist, function (dd) {
                                    if ($scope.skill_list.length === 0) {
                                        $scope.skill_list.push({ ECS_SkillName: dd.ECS_SkillName });
                                    } else if ($scope.skill_list.length > 0) {
                                        var count = 0;
                                        angular.forEach($scope.skill_list, function (d) {
                                            if (dd.ECS_SkillName === d.ECS_SkillName) {
                                                count += 1;
                                            }
                                        });

                                        if (count === 0) {
                                            $scope.skill_list.push({ ECS_SkillName: dd.ECS_SkillName });
                                        }
                                    }
                                });

                                $scope.skill_list_skill_area = [];
                                angular.forEach($scope.skill_list, function (dd) {
                                    $scope.skill_list_skill_area = [];
                                    angular.forEach($scope.getstudentwiseskillslist, function (d) {
                                        if (dd.ECS_SkillName === d.ECS_SkillName) {
                                            if ($scope.skill_list_skill_area.length === 0) {
                                                $scope.skill_list_skill_area.push({ ECSA_SkillArea: d.ECSA_SkillArea, ECSA_SkillOrder: d.ECSA_SkillOrder });
                                            } else if ($scope.skill_list_skill_area.length > 0) {
                                                var count_skill_area = 0;
                                                angular.forEach($scope.skill_list_skill_area, function (ddd) {
                                                    if (ddd.ECSA_SkillArea == d.ECSA_SkillArea) {
                                                        count_skill_area += 1;
                                                    }
                                                });

                                                if (count_skill_area === 0) {
                                                    $scope.skill_list_skill_area.push({ ECSA_SkillArea: d.ECSA_SkillArea, ECSA_SkillOrder: d.ECSA_SkillOrder });
                                                }
                                            }
                                        }
                                    });

                                    dd.skillarea = $scope.skill_list_skill_area;
                                });
                                console.log($scope.skill_list);


                                // Activites List
                                $scope.activites_list = [];
                                angular.forEach($scope.getstudentwiseactiviteslist, function (d) {
                                    if ($scope.activites_list.length === 0) {
                                        $scope.activites_list.push({ ECACT_SkillName: d.ECACT_SkillName });
                                    } else if ($scope.activites_list.length > 0) {
                                        var count_activies_list = 0;
                                        angular.forEach($scope.activites_list, function (dd) {
                                            if (dd.ECACT_SkillName === d.ECACT_SkillName) {
                                                count_activies_list += 1;
                                            }
                                        });
                                        if (count_activies_list === 0) {
                                            $scope.activites_list.push({ ECACT_SkillName: d.ECACT_SkillName });
                                        }
                                    }
                                });

                                $scope.activites_list_area_list = [];
                                angular.forEach($scope.activites_list, function (dd) {
                                    $scope.activites_list_area_list = [];
                                    angular.forEach($scope.getstudentwiseactiviteslist, function (d) {
                                        if (dd.ECACT_SkillName === d.ECACT_SkillName) {
                                            if ($scope.activites_list_area_list.length === 0) {
                                                $scope.activites_list_area_list.push({ ECACTA_SkillArea: d.ECACTA_SkillArea, ECACTA_SkillOrder: d.ECACTA_SkillOrder });
                                            } else if ($scope.activites_list_area_list.length > 0) {
                                                var count_activites_list_area_list = 0;
                                                angular.forEach($scope.activites_list_area_list, function (ddd) {
                                                    if (ddd.ECACTA_SkillArea === d.ECACTA_SkillArea) {
                                                        count_activites_list_area_list += 1;
                                                    }
                                                });

                                                if (count_activites_list_area_list === 0) {
                                                    $scope.activites_list_area_list.push({ ECACTA_SkillArea: d.ECACTA_SkillArea, ECACTA_SkillOrder: d.ECACTA_SkillOrder });
                                                }
                                            }
                                        }
                                    });

                                    dd.activites_list_area_list = $scope.activites_list_area_list;
                                });
                            }

                            else if (promise.examflag === 2 || promise.examflag === 3) {
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
                            }


                            $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                            $scope.examwiseremarks = promise.examwiseremarks;
                            $scope.getparticipatedetails = promise.getparticipatedetails;

                            if ($scope.getpromotionremarksdetails !== null && $scope.getpromotionremarksdetails.length > 0) {
                                $scope.promotedclass = $scope.getpromotionremarksdetails[0].eprD_ClassPromoted;
                                $scope.remarks = $scope.getpromotionremarksdetails[0].eprD_Remarks;
                            }

                            angular.forEach($scope.gettermdetails, function (dd) {
                                angular.forEach($scope.examwiseremarks, function (d) {
                                    if (dd.ecT_Id === d.ecT_Id) {
                                        dd.generalremaks = d.ecterE_Remarks;
                                    }
                                });

                                angular.forEach($scope.getparticipatedetails, function (d) {
                                    if (dd.ecT_Id === d.ecT_Id) {
                                        dd.participate = d.esttA_Remarks;
                                    }
                                });

                                angular.forEach($scope.getstudentwiseattendancedetails, function (d) {
                                    if (d.ECT_Id === dd.ecT_Id) {
                                        dd.workingdays = d.TOTALWORKINGDAYS;
                                        dd.presentdays = d.PRESENTDAYS;
                                    }
                                });
                            });

                            var e1 = angular.element(document.getElementById("report"));
                            $compile(e1.html(promise.htmlstring))(($scope));
                            console.log($scope.studentdetails);
                        } else {
                            swal("Marks Not Found");
                        }
                    }

                    else if ($scope.examflag_Temp === 4) {
                        $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                        if ($scope.getstudentmarksdetails !== null && $scope.getstudentmarksdetails.length > 0) {
                            $scope.HHS_I_IV_grid = true;
                            $scope.studentdetails = promise.getstudentdetails;
                            $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                            $scope.getstudentwisesubjectsubsubjectlist = promise.getstudentwisesubjectsubsubjectlist;
                            $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                            $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                            $scope.getexam = promise.getexam;

                            $scope.selectedexamlist = [];

                            angular.forEach($scope.getexam, function (dd) {
                                $scope.selectedexamlist.push({ EME_Id: dd.emE_Id, EME_ExamName: dd.emE_ExamName });
                            });
                           
                            $scope.classteachername = $scope.studentdetails[0].hrmE_EmployeeFirstName;
                            $scope.stuname = $scope.studentdetails[0].studentname;
                            $scope.asmcL_ClassName = $scope.studentdetails[0].classname;
                            $scope.asmC_SectionName = $scope.studentdetails[0].sectionname;
                            $scope.amaY_RollNo = $scope.studentdetails[0].rollno;
                            $scope.dob = $scope.studentdetails[0].dob;
                            $scope.ASMAY_Year = $scope.studentdetails[0].ASMAY_Year;
                            $scope.amsT_PerStreet = $scope.studentdetails[0].AMST_PerStreet;
                            $scope.amsT_PerArea = $scope.studentdetails[0].AMST_PerArea;
                            $scope.amsT_PerCity = $scope.studentdetails[0].AMST_PerCity;
                            $scope.ivrmmS_Name = $scope.studentdetails[0].ivrmms_name;
                            $scope.ivrmmC_CountryName = $scope.studentdetails[0].IVRMMC_CountryName;
                            $scope.addressd1 = $scope.studentdetails[0].addressd1;
                            $scope.amsT_PerPincode = $scope.studentdetails[0].amsT_PerPincode;
                            $scope.mobileno = $scope.studentdetails[0].mobileno;
                            $scope.AMST_Photoname = $scope.studentdetails[0].AMST_Photoname;
                            $scope.amst_dob = new Date($scope.studentdetails[0].amst_dob);

                            $scope.getdate = $scope.amst_dob.getDate();
                            $scope.getyear = $scope.amst_dob.getFullYear();
                            var months = $scope.amst_dob.getMonth();

                            $scope.index = $scope.ordinal_suffix_of1($scope.getdate);
                            $scope.getmonth = $scope.getmontnames(months);

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
                            $scope.temp_subsubject = [];
                            angular.forEach($scope.studentdetails, function (stu) {
                                $scope.temp_subsubject = [];
                                angular.forEach(stu.studentsubjects, function (stu_subj) {
                                    $scope.temp_subsubject = [];
                                    angular.forEach($scope.getstudentwisesubjectsubsubjectlist, function (subsubj) {
                                        if (subsubj.ISMS_Id === stu_subj.ISMS_Id) {
                                            $scope.temp_subsubject.push(subsubj);
                                        }
                                    });
                                    stu_subj.subsubject = $scope.temp_subsubject;
                                });
                            });


                            //Attendance Details
                            angular.forEach($scope.studentdetails, function (stu) {
                                $scope.temp_attednancd = [];
                                angular.forEach($scope.getstudentwiseattendancedetails, function (stu_att) {
                                    if (stu.AMST_Id === stu_att.AMST_Id) {
                                        $scope.temp_attednancd.push(stu_att);
                                    }
                                });
                                stu.attendance = $scope.temp_attednancd;
                            });

                            //Marks Details
                            angular.forEach($scope.studentdetails, function (stu) {
                                $scope.temp_marks = [];
                                angular.forEach($scope.getstudentmarksdetails, function (stu_att) {
                                    if (stu.AMST_Id === stu_att.AMST_Id) {
                                        $scope.temp_marks.push(stu_att);
                                    }
                                });
                                stu.studentmarks = $scope.temp_marks;
                            });


                            $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                            if ($scope.getpromotionremarksdetails !== null && $scope.getpromotionremarksdetails.length > 0) {
                                $scope.promotedclass = $scope.getpromotionremarksdetails[0].eprD_ClassPromoted;
                            }
                            var e1 = angular.element(document.getElementById("report"));
                            $compile(e1.html(promise.htmlstring))(($scope));
                            console.log($scope.studentdetails);
                        } else {
                            swal("Details Not Found");
                        }                        
                    }                   
                });
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        };

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; });
        };

        //to print
        $scope.print_HHS02 = function () {
            var innerContents = "";
            var popupWinindow = "";
            innerContents = document.getElementById("HHS02").innerHTML;
            popupWinindow = window.open('');
            popupWinindow.document.open();
            if ($scope.examflag_Temp === 1 || $scope.examflag_Temp === 4) {
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/NDS/NDS_1_5_ReportCardPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            }
            else if ($scope.examflag_Temp === 2 || $scope.examflag_Temp === 3) {               
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/NDS/ND_6_8_ReportCardPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            }
            popupWinindow.document.close();
        };

        $scope.ordinal_suffix_of1 = function (datesufix1) {
            var j = datesufix1 % 10,
                k = datesufix1 % 100;
            if (j == 1 && k != 11) {
                $scope.datesuf1 = "st";
                return $scope.datesuf1;
            }
            if (j == 2 && k != 12) {
                $scope.datesuf1 = "nd";
                return $scope.datesuf1;
            }
            if (j == 3 && k != 13) {
                $scope.datesuf1 = "rd";
                return $scope.datesuf1;
            }
            $scope.datesuf1 = "th";
            return $scope.datesuf1;
        };

        //month names
        $scope.getmontnames = function (monthid) {
            frommonth = "";
            switch (monthid) {

                case 0:
                    frommonth = "JANUARY";
                    break;
                case 1:
                    frommonth = "FEBRUARY";
                    break;
                case 2:
                    frommonth = "MARCH";
                    break;
                case 3:
                    frommonth = "APRIL";
                    break;
                case 4:
                    frommonth = "MAY";
                    break;
                case 5:
                    frommonth = "JUNE";
                    break;
                case 6:
                    frommonth = "JULY";
                    break;
                case 7:
                    frommonth = "AUGUST";
                    break;
                case 8:
                    frommonth = "SEPTEMBER";
                    break;
                case 9:
                    frommonth = "OCTOBER";
                    break;
                case 10:
                    frommonth = "NOVEMBER";
                    break;
                case 11:
                    frommonth = "DECEMBER";
                    break;
                default:
                    frommonth = "";
                    break;
            }
            return frommonth;
        };




    }
})();
