(function () {
    'use strict';
    angular.module('app').controller('NDS_9thStudentNewReportController', NDS_9thStudentNewReportController)

    NDS_9thStudentNewReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$compile']
    function NDS_9thStudentNewReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $compile) {

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
        var ASMCL_Id = 0; var ASMAY_Id = 0; var ASMS_Id = 0;
        $scope.Temp_AmstId = []; var AMST_Id = 0;
        var frommonth = "";
        $scope.imgname = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            $scope.imgname = admfigsettings[0].asC_Logo_Path;
        }
        else {
            $scope.imgname = "";
        }
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
                    $scope.amsT_Id = promise.amsT_Id;

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
                                $scope.HHS_I_IV_grid = true;
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

        //$scope.NDS_Get_Progresscard_Report = function () {
        //    $scope.HHS_I_IV_grid = false;
        //    $scope.submitted = true;
        //    if ($scope.myForm.$valid) {
        //        $scope.grandavgtotal = 0;
        //        var data = {
        //            "ASMCL_Id": $scope.ASMCL_Id
        //        };

        //        apiService.create("StudentProgressCardReport/NDS_Get_Progresscard_Report", data).then(function (promise) {
        //            $scope.examflag_Temp = promise.examflag;
        //            if ($scope.examflag_Temp === 2) {
        //                $scope.HHS_I_IV_grid = true;
        //                ASMCL_Id = $scope.ASMCL_Id;
        //                $scope.saveddata(ASMCL_Id, ASMS_Id, AMST_Id);
        //                var e1 = angular.element(document.getElementById("report"));
        //                $compile(e1.html(promise.htmlstring))(($scope));
        //            }


        //        });
        //    }
        //};
        $scope.saveddata = function (obj) {
            if ($scope.HHS_I_IV_grid = true) {

                $scope.JSHSReport = false;
                $scope.getstudentmarksdetails_temp = [];
                $scope.submitted = true;

                if ($scope.myForm.$valid) {
                    $scope.termlisttemp = [];

                    $scope.Temp_AmstId = [];
                    $scope.Temp_AmstId.push({ AMST_Id: $scope.amsT_Id });

                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "Temp_AmstIds": $scope.Temp_AmstId
                    };

                    apiService.create("JSHSExamReports/nds_9_report", data).then(function (promise) {

                        $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                        $scope.JSHSReport = true;
                        $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;

                        $scope.gettermdetails = promise.gettermdetails;
                        $scope.gettermexamdetails = promise.gettermexamdetails;
                        $scope.getgroupdetails = promise.getgroupdetails;

                        $scope.getgroupdetails.push({
                            empG_DistplayName: "Marks Obtained", empsG_GroupName: "Marks Obtained", empsG_MarksValue: 100
                        });

                        $scope.getgroupdetails.push({
                            empG_DistplayName: "Grade", empsG_GroupName: "Grade", empsG_MarksValue: ""
                        });

                        $scope.studentdetails = promise.getstudentdetails;
                        $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                        $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                        $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

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

                        // STUDENT WISE SKILL  LIST

                        $scope.skilllist_temp = [];
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.skilllist_temp = [];
                            var count = 0;
                            angular.forEach($scope.getstudentwiseskillslist, function (dd) {
                                if (stu.AMST_Id === dd.AMST_Id) {
                                    if ($scope.skilllist_temp.length === 0) {
                                        $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName, ECSA_SkillArea: dd.ECSA_SkillArea });
                                    } else if ($scope.skilllist_temp.length > 0) {
                                        count = 0;
                                        angular.forEach($scope.skilllist_temp, function (ddd) {
                                            if (ddd.ECS_SkillName === dd.ECS_SkillName) {
                                                count += 1;
                                            }
                                        });
                                        if (count === 0) {
                                            $scope.skilllist_temp.push({ ECS_SkillName: dd.ECS_SkillName, ECSA_SkillArea: dd.ECSA_SkillArea });
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
                                            $scope.skill_main_list_array.push({ scoregrade: dd.EMGD_Name, ecT_Id: dd.ECT_Id, ECSA_SkillArea: dd.ECSA_SkillArea });
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


                    });
                } else {
                    $scope.submitted = true;
                }
            }
            else {
                swal("Report Format Not Mapped Contact Administrator");
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
