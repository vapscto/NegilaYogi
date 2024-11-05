(function () {
    'use strict';
    angular.module('app').controller('SARVODAYA_REPORTSeniorController', SARVODAYA_REPORTController)
    SARVODAYA_REPORTController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function SARVODAYA_REPORTController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        var count = 0;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.getstudentlist = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.imgname = "";
        $scope.ECTEX_MarksPercentValue = 15;
        $scope.grandeTotal = true;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            $scope.imgname = admfigsettings[0].asC_Logo_Path;
        }
        else {
            $scope.imgname = "";
        }



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
            $scope.grade_list = [];
            $scope.EMGR_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "flagtype": "Exam",
            };
            apiService.create("JSHSExamReports/GetStudentDetails", data).then(function (promise) {
                $scope.getstudentlist = promise.getstudentlist;
                $scope.all = true;

                angular.forEach($scope.getstudentlist, function (dd) {
                    dd.AMST_Ids = true;
                });
                $scope.grade_list = promise.getgradelist;
                angular.forEach($scope.year_list, function (dd) {
                    if (dd.asmaY_Id == $scope.ASMAY_Id) {
                        $scope.Accdemic = dd.asmaY_Year;
                    }
                });
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };




        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.SubExam_Names = [];
            $scope.getstudentmarksdetails = [];
            $scope.getskill = [];            $scope.getallgrade = [];
            $scope.studentdetails = [];
            $scope.groupwiseexamlist = [];
            $scope.submitted = true;
            $scope.getgroupdetails = [];
            $scope.getstudent_examwisemarks = [];
            $scope.stud_work_attendence = [];
            $scope.getstudentwiseattendancedetails = [];
            $scope.stud_present_attendence = [];
            $scope.gettermdetails = [];
            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                $scope.Temp_AmstId = [];

                angular.forEach($scope.getstudentlist, function (dd) {
                    if (dd.AMST_Ids) {
                        $scope.Temp_AmstId.push({ AMST_Id: dd.amsT_Id });
                    }
                });
                //TO GET THE NEXT PROMOTION CLASS AUTOMATICALLY --ADDED BY ADARSH
                $scope.promotedclass = [];
                var countclass=0
                angular.forEach($scope.class_list, function (nextclass, index) {
                    if ($scope.ASMCL_Id == nextclass.asmcL_Id && index + 1 < $scope.class_list.length) {
                        countclass=index+1
                        $scope.promotedclass = $scope.class_list[countclass].asmcL_ClassName
                    }
                });

                $scope.promotedyear ='';
                        $scope.promotedyear = $scope.year_list[0].asmaY_Year
                 
               

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "Temp_AmstIds": $scope.Temp_AmstId,
                    "EMGR_Id": $scope.EMGR_Id,
                    "ECTEX_MarksPercentValue": $scope.ECTEX_MarksPercentValue,
                    "EYCES_MaxMarks": 20
                };

                apiService.create("JSHSExamReports/Sarvodaya_ReportSenior", data).then(function (promise) {
                    if (promise.getstudentmarksdetails != null && promise.getstudentmarksdetails.length > 0) {
                        $scope.studentdetails = promise.getstudentdetails;
                        $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                        $scope.getgroupdetails = promise.getgroupdetails;
                        $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                        $scope.gettermdetails = promise.gettermdetails;

                        $scope.getsubjectwisetotaldetails = promise.getsubjectwisetotaldetails;
                        $scope.studenwisesubjects = [];
                        //subjects
                        angular.forEach($scope.studentdetails, function (stu) {
                            $scope.studenwisesubjects = [];
                            angular.forEach($scope.getstudentwisesubjectlist, function (stusubj) {
                                if (stu.AMST_Id === stusubj.AMST_Id) {
                                    $scope.studenwisesubjects.push(stusubj);
                                }
                            });
                            stu.studentsubjects = $scope.studenwisesubjects;
                        });
                        //exams
                        angular.forEach(promise.getgroupexamdetails, function (d) {
                            $scope.groupwiseexamlist.push({
                                empsgE_ForMaxMarkrs: d.empsgE_ForMaxMarkrs,
                                empG_GroupName: d.empG_GroupName,
                                EME_Id: d.emE_Id,
                                emE_ExamName: d.emE_ExamName,
                                emE_ExamOrder: d.emE_ExamOrder,
                                empG_DistplayName: d.empG_DistplayName,
                                emE_ExamCode: d.emE_ExamCode,
                                Colspan: null
                            });

                        });
                        //marks
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
                        //examwise totalmarks
                        $scope.skilllist_temp = [];
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
                        //skill Activity                                             if (promise.getstudentwiseskillslist != null && promise.getstudentwiseskillslist.length > 0) {
                            $scope.getstudentwiseskillslist = promise.getstudentwiseskillslist;
                            angular.forEach(promise.getstudentwiseskillslist, function (dev) {
                                if ($scope.skilllist_temp.length === 0) {
                                    $scope.skilllist_temp.push(dev);

                                }

                                else if ($scope.skilllist_temp.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.skilllist_temp, function (emp) {
                                        if (emp.ECS_Id === dev.ECS_Id) {
                                            intcount += 1;

                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.skilllist_temp.push(dev);

                                    }
                                }
                            });
                        }
                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;
                        //cancasa
                        $scope.studentdetailsgraph = [];
                        angular.forEach($scope.studentdetails, function (dd) {
                            $scope.studentdetailsgraph.push(
                                {
                                    AMST_Id: dd.AMST_Id,
                                    chart: "chart" + dd.AMST_Id,
                                    template: ' <div id = "chart' + dd.AMST_Id + '" style = "height: 100px; width: 98%;"></div>'

                                });
                        });
                        //Agaia adding column 
                        //SubExam_Names
                        $scope.SubExam_Names = [];
                        angular.forEach(promise.getstudentmarksdetails, function (dev) {                            if ($scope.SubExam_Names.length === 0) {                                $scope.SubExam_Names.push(dev);                            }                            else if ($scope.SubExam_Names.length > 0) {                                var intcount = 0;                                angular.forEach($scope.SubExam_Names, function (emp) {                                    if (emp.EMSE_SubExamName === dev.EMSE_SubExamName) {                                        intcount += 1;                                    }                                });                                if (intcount === 0) {                                    $scope.SubExam_Names.push(dev);                                                                    }                            }                        });
                        console.log("TEST", $scope.SubExam_Names)
                        // //getexam
                        $scope.SubExam_Names = promise.getexam;
                        if ($scope.groupwiseexamlist != null && $scope.groupwiseexamlist.length > 0) {
                            angular.forEach($scope.groupwiseexamlist, function (dev) {                                var Colspan = 0;                                angular.forEach($scope.SubExam_Names, function (emp) {                                    if (dev.EME_Id == emp.EME_Id && dev.empG_GroupName == emp.EMPSG_GroupName) {                                        Colspan = Colspan + 1;
                                    }                                });                                dev.Colspan = Colspan;                            });
                        }
                    }
                    else {
                        swal("Pramotion calculation Not Done !")
                    }


                });
            } else {
                $scope.submitted = true;
            }
        };

        //to print
        //$scope.print_HHS02 = function () {
        //    var innerContents = document.getElementById("HHS02").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();
        //};
        $scope.print_HHS02 = function () {
            if (count > 0) {
                var innerContents = "";                innerContents = document.getElementById("HHS02").innerHTML;                var popupWinindow = window.open('');                popupWinindow.document.open();                popupWinindow.document.write('<html><head>' +                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');                popupWinindow.document.close();
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
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
            angular.forEach($scope.getstudentlist, function (dd) {
                dd.AMST_Ids = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.getstudentlist.every(function (itm) { return itm.AMST_Ids; });
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
        //bind canvas
        $scope.bindCanvas = function (grp) {
            $scope.processtotgraph = [];
            $scope.processAverage = [];
            $scope.tempsubject = [];
            $scope.tempsubject1 = [];
            $scope.summativegraph = [];
            $scope.summativegraph1 = [];
            if ($scope.getstudentwisesubjectlist != null && $scope.getstudentwisesubjectlist.length > 0) {

                angular.forEach($scope.getstudentmarksdetails, function (stusubj1) {
                    if (stusubj1.EME_ExamName == "TOTAL" && stusubj1.EME_Id == 980000 && grp.AMST_Id == stusubj1.AMST_Id) {
                        $scope.summativegraph.push(stusubj1.ESTMPSSS_ObtainedMarks);
                        $scope.tempsubject.push(stusubj1.ISMS_SubjectName);
                    }
                    //
                    

                });
                
                //angular.forEach($scope.getstudentmarksdetails, function (stusubj1) {
                //    if (stusubj1.EME_ExamCode == "SA-02" && stusubj1.EME_Id != 9800000 && stusubj1.EME_Id != 9800001 && stusubj1.AMST_Id === grp.AMST_Id) {
                //        $scope.summativegraph1.push(stusubj1.ObtainedMarks);
                //        $scope.tempsubject2.push(stusubj1.ISMS_SubjectName);
                //    }
                    
                //});

            }
            var ctx = document.getElementById("pieChart" + grp.AMST_Id).getContext('2d');

            var myChart = new Chart(ctx, {
                type: 'bar',

                data: {
                    labels: $scope.tempsubject,
                    datasets: [
                        {
                            label: 'SA-1',
                            data: $scope.summativegraph,
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
                            label: 'SA-2',
                            data: $scope.summativegraph1,
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
})();