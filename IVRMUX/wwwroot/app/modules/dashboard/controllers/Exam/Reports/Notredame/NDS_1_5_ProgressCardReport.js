(function () {
    'use strict';
    angular.module('app').controller('NDS15ProgressCardReportController', NDS15ProgressCardReportController)
    NDS15ProgressCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function NDS15ProgressCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.datesuf1 = "";
        var frommonth = "";

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
            // $scope.maxDatedof = new Date($scope.today);

        };

        $scope.getserverdate();

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.termlistd = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.class_list = [];
            $scope.termlist = [];
            $scope.getstudentlist = [];
            $scope.AMST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.termlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.AMST_Id = "";

            $scope.section_list = [];
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

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("JSHSExamReports/GetStudentDetails", data).then(function (promise) {

                $scope.getstudentlist = promise.getstudentlist;
            });
        };

        $scope.Onchangestudent = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {

            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "AMST_Id": $scope.AMST_Id
                };

                apiService.create("JSHSExamReports/nds_1_5_report", data).then(function (promise) {

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
                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                    $scope.getstudentwisesportsdetails = promise.getstudentwisesportsdetails;

                    $scope.groupwiseexamlist_temp = [];
                    $scope.groupwiseexamlist = [];

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
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                            EME_ExamName: "Grade", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "Grade"
                        });


                        $scope.groupwiseexamlist.push({
                            EMPG_GroupName: dd.empG_GroupName, EME_Id: 9800001,
                            EME_ExamName: "Grade", EME_ExamOrder: 9800001, EMPG_DistplayName: dd.empG_DistplayName,
                            EMPSGE_ForMaxMarkrs: counttotal, examnamedisplay: "Grade"
                        });

                        dd.empsG_MarksValue = counttotal;
                        dd.groupewiseexam = $scope.groupwiseexamlist_temp;
                    });

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

                    $scope.getdate = $scope.amst_dob.getDate();
                    $scope.getyear = $scope.amst_dob.getFullYear();
                    var months = $scope.amst_dob.getMonth();

                    $scope.index = $scope.ordinal_suffix_of1($scope.getdate);
                    $scope.getmonth = $scope.getmontnames(months);

                    $scope.getsudentwisehousedetails = promise.getsudentwisehousedetails;
                    if ($scope.getsudentwisehousedetails !== null && $scope.getsudentwisehousedetails.length > 0) {
                        $scope.housename = $scope.getsudentwisehousedetails[0].spccmH_HouseName;
                    }


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


                    $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                    $scope.examwiseremarks = promise.examwiseremarks;
                    $scope.getparticipatedetails = promise.getparticipatedetails;

                    if ($scope.getpromotionremarksdetails !== null && $scope.getpromotionremarksdetails.length > 0) {
                        $scope.promotedclass = $scope.getpromotionremarksdetails[0].eprD_ClassPromoted;
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



                    console.log($scope.studentdetails);

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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/NDS/NDS_1_5_ReportCardPdf.css" />' +
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