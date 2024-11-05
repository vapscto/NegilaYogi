(function () {
    'use strict';
    angular.module('app').controller('JSHSStudentIndividualTermReportController', jSHSStudentIndividualTermReportController)
    jSHSStudentIndividualTermReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function jSHSStudentIndividualTermReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.isOptionsRequired1 = function () {
            return !$scope.termlist.some(function (options) {
                return options.ECT_Id;
            });
        };

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSPortal_StudentReports/Getdetails_IT", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
                      var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSPortal_StudentReports/get_Terms_IT", data).then(function (promise) {
                $scope.grade_list = promise.getgradelist;
                $scope.termlist = promise.gettermlist;
            });
        };

      
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {

            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                angular.forEach($scope.termlist, function (term) {
                    if (term.ECT_Id === true) {
                        $scope.termlisttemp.push({ ECT_Id: term.ecT_Id, ECT_TermName: term.ecT_TermName });
                    }
                });

                var data = {
                   
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EMGR_Id": $scope.EMGR_Id,
                    "termlist": $scope.termlisttemp
                };

                apiService.create("JSHSPortal_StudentReports/get_reportdetails_IT", data).then(function (promise) {

                    $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                    $scope.JSHSReport = true;
                    $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                    $scope.getgradedetails = promise.getgradedetails;
                    $scope.gettermdetails = promise.gettermdetails;
                    $scope.gettermexamdetails = promise.gettermexamdetails;

                    //angular.forEach($scope.gettermdetails, function (dd) {
                    //    $scope.gettermexamdetails.push({ ecT_Id: dd.ecT_Id, emE_ExamName: 'Marks Obtained', emE_Id: 50000 });
                    //    $scope.gettermexamdetails.push({ ecT_Id: dd.ecT_Id, emE_ExamName: 'Grade', emE_Id: 50000 });
                    //});

                    $scope.termwiseexamlist = [];

                    angular.forEach($scope.gettermdetails, function (dd) {
                        angular.forEach($scope.gettermexamdetails, function (ddd) {
                            if (dd.ecT_Id === ddd.ecT_Id) {
                                $scope.termwiseexamlist.push({
                                    ecT_Id: dd.ecT_Id, emE_ExamName: ddd.emE_ExamName + '(' + ddd.ecteX_MarksPercentValue + ')',
                                    emE_Id: ddd.emE_Id, ecteX_MarksPercentValue: ddd.ecteX_MarksPercentValue
                                });
                            }
                        });

                        $scope.termwiseexamlist.push({ ecT_Id: dd.ecT_Id, emE_ExamName: 'Marks Obtained', emE_Id: 50000 });
                        $scope.termwiseexamlist.push({ ecT_Id: dd.ecT_Id, emE_ExamName: 'Grade', emE_Id: 50000 });

                        dd.termexamdetails = $scope.termwiseexamlist;
                    });

                    console.log($scope.termwiseexamlist);
                    console.log($scope.gettermdetails);

                    $scope.studentdetails = promise.getstudentdetails;

                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                    $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;

                    $scope.getstudentwiseactiviteslist = promise.getstudentwiseactiviteslist;

                    $scope.getstudentwisesportsdetails = promise.getstudentwisesportsdetails;


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

                    // PART 2
                    $scope.temp_part_two_array = [];
                    angular.forEach($scope.gettermdetails, function (dd) {
                        $scope.temp_part_two_array.push({ ecT_Id: dd.ecT_Id, ecT_TermName: dd.ecT_TermName, parttwoheadername: 'NAME OF THE ACTIVITY' });
                        $scope.temp_part_two_array.push({ ecT_Id: dd.ecT_Id, ecT_TermName: dd.ecT_TermName, parttwoheadername: 'GRADE' });
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
                                        $scope.skill_main_list_array.push({ scorename: dd.ECSA_SkillArea, ecT_Id: dd.ECT_Id });
                                        $scope.skill_main_list_array.push({ scoregrade: dd.EMGD_Name, ecT_Id: dd.ECT_Id });
                                    }
                                });
                            });
                            skill.skill_score_details = $scope.skill_main_list_array;
                        });
                    });

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


                    // STUDENT WISE Sports LIST 
                    //$scope.studentwisesportslist = [];
                    //angular.forEach($scope.studentdetails, function (student) {
                    //    $scope.studentwisesubject = [];
                    //    angular.forEach($scope.getstudentwisesportsdetails, function (sport) {
                    //        if (student.AMST_Id === sport.AMST_Id) {
                    //            student.height = sport.SPCCSHW_Height;
                    //            student.weight = sport.SPCCSHW_Weight;
                    //        }
                    //    });
                    //});
                    $scope.colspan_sport = $scope.termlisttemp.length * 2;

                    $scope.colspan_sport_head = $scope.colspan_sport * 2;


                    $scope.sportsdetails = [];
                    angular.forEach($scope.gettermdetails, function (dd) {
                        angular.forEach($scope.getstudentwisesportsdetails, function (ss) {
                            if (dd.ecT_Id === ss.ECT_Id) {
                                $scope.sportsdetails.push({ colname: 'Height (cm)', colvalue: ss.SPCCSHW_Height, colectid: ss.ECT_Id });
                                $scope.sportsdetails.push({ colname: ss.SPCCSHW_Height, colvalue: ss.SPCCSHW_Height, colectid: ss.ECT_Id });
                                $scope.sportsdetails.push({ colname: 'Weight (kg)', colvalue: ss.SPCCSHW_Height, colectid: ss.ECT_Id });
                                $scope.sportsdetails.push({ colname: ss.SPCCSHW_Weight, colvalue: ss.SPCCSHW_Weight, colectid: ss.ECT_Id });
                            }
                        });
                        dd.colspan_sport_subhead = 4;
                    });

                    console.log($scope.sportsdetails);

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


                    console.log($scope.studentdetails);
                    var i = 0;
                    $scope.reportname = "";
                    if ($scope.termlisttemp.length === 1) {
                        angular.forEach($scope.termlisttemp, function (dd, index) {
                            if (index === 0) {
                                $scope.termname = dd.ECT_TermName;
                            } else {
                                i += 1;
                                $scope.termname = $scope.termname + ' , ' + dd.ECT_TermName;
                            }
                        });

                        $scope.reportname = "PROGRESS REPORT FOR " + $scope.termname.toUpperCase();
                    } else {
                        $scope.reportname = "FINAL REPORT CARD";
                    }

                    angular.forEach($scope.year_list, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.yearname = dd.asmaY_Year;
                        }
                    });

                });
            } else {
                $scope.submitted = true;
            }
        };

        //to print
        $scope.print_HHS02 = function () {
            var innerContents = document.getElementById("HHS02").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSOverallTermReportCardPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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