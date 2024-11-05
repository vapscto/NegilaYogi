(function () {
    'use strict';

    angular.module('app').controller('ExamWiseTermReportController', ExamWiseTermReportController);
    ExamWiseTermReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter'];
    function ExamWiseTermReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {
        /* jshint validthis:true */

        $scope.printbutton = true;
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters = "";
        var copty = "";

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        //TO  GEt The Values iN Grid
        var pageid = 2;
        $scope.BindData = function () {
            apiService.getURI("MaldaProgressReportExam/Getdetails", pageid).
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.sectionlist;
                    $scope.exsplt = promise.examlist;
                });
        };



        $scope.OnAcdyear = function () {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("MaldaProgressReportExam/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.clslist = promise.classlist;
                    if ($scope.clslist.length === 0) {
                        swal("No Class Is Mapped For Selected Academic Year");
                    }
                } else {
                    swal("No Class Is Mapped For Selected Academic Year");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("MaldaProgressReportExam/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.seclist = promise.seclist;
                    if ($scope.seclist.length === 0) {
                        swal("No Section Is Mapped For Selected Class");
                    }
                } else {
                    swal("No Section Is Mapped For Selected Class");
                }

            });
        };

        $scope.onchangesection = function () {
            $scope.emE_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };

            apiService.create("MaldaProgressReportExam/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.exsplt = promise.exmstdlist;
                    if ($scope.exsplt.length === 0) {
                        swal("No Exam Is Mapped For Selected Details ");
                    }
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                }
            });
        };


        $scope.saveddata = function () {

            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                };
                apiService.create("MaldaProgressReportExam/getreportpromotion", data).then(function (promise) {

                    if (promise !== null) {

                        $scope.getstudentdetails = promise.getstudentdetails;
                        $scope.getsubjectlist = promise.getsubjectlist;
                        $scope.getexamlist = promise.getexamlist;
                        console.log($scope.getexamlist);
                        $scope.getsavedlist = promise.getsavedlist;
                        $scope.grade_detailslist = promise.grade_detailslist;
                        $scope.promotiondetails = promise.promotiondetails;

                        if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0 && $scope.getsavedlist !== null
                            && $scope.getsavedlist.length > 0) {

                            $scope.tempsubjectlist = [];

                            //angular.forEach($scope.getstudentdetails, function (stuid) {
                            //    $scope.tempsubjectlist = [];
                            //    angular.forEach($scope.getsavedlist, function (stuidsub) {
                            //        if (stuidsub.amstid === stuid.amstid) {
                            //            if ($scope.tempsubjectlist.length === 0) {
                            //                $scope.tempsubjectlist.push({ subjectid: stuidsub.ismsid, subjectname: stuidsub.subjectname });
                            //            } else if ($scope.tempsubjectlist.length > 0) {
                            //                var count = 0;
                            //                angular.forEach($scope.tempsubjectlist, function (dd) {
                            //                    if (dd.subjectid === stuidsub.ismsid) {
                            //                        count += 1;
                            //                    }
                            //                });
                            //                if (count === 0) {
                            //                    $scope.tempsubjectlist.push({ subjectid: stuidsub.ismsid, subjectname: stuidsub.subjectname });
                            //                }
                            //            }
                            //        }
                            //    });
                            //    stuid.subjectlist = $scope.tempsubjectlist;
                            //});

                            $scope.subjectlistnew = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.subjectlistnew = [];
                                angular.forEach($scope.getsubjectlist, function (stuidsub) {
                                    if (dd.amstid === stuidsub.AMST_Id) {
                                        $scope.subjectlistnew.push({ subjectid: stuidsub.ISMS_Id, subjectname: stuidsub.ISMS_SubjectName });
                                    }
                                });

                                dd.subjectlist = $scope.subjectlistnew;
                            });


                            console.log($scope.getstudentdetails);

                            $scope.saveddetails = [];
                            angular.forEach($scope.getstudentdetails, function (student) {
                                angular.forEach(student.subjectlist, function (subjects) {
                                    $scope.saveddetails = [];
                                    angular.forEach($scope.getsavedlist, function (save) {
                                        if (student.amstid === save.amstid && subjects.subjectid === save.ismsid) {
                                            $scope.saveddetails.push(save);
                                        }
                                    });

                                    subjects.marksdetails = $scope.saveddetails;
                                });
                            });

                            $scope.totalmarksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.totalmarksdetails = [];
                                angular.forEach($scope.getsavedlist, function (save) {
                                    if (dd.amstid === save.amstid && save.ismsid === 5000) {
                                        $scope.totalmarksdetails.push(save);
                                    }
                                });
                                dd.totalmarks = $scope.totalmarksdetails;
                            });


                            $scope.percentagemarksdetails = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.percentagemarksdetails = [];
                                angular.forEach($scope.getsavedlist, function (save) {
                                    if (dd.amstid === save.amstid && save.ismsid === 5001) {
                                        $scope.percentagemarksdetails.push(save);
                                    }
                                });
                                dd.percentagemarks = $scope.percentagemarksdetails;
                            });

                            $scope.workingdaysdetails = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.workingdaysdetails = [];
                                angular.forEach($scope.getsavedlist, function (save) {
                                    if (dd.amstid === save.amstid && save.ismsid === 5002) {
                                        $scope.workingdaysdetails.push(save);
                                    }
                                });
                                dd.workingdays = $scope.workingdaysdetails;
                            });

                            $scope.attedancedetails = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.attedancedetails = [];
                                angular.forEach($scope.getsavedlist, function (save) {
                                    if (dd.amstid === save.amstid && save.ismsid === 5003) {
                                        $scope.attedancedetails.push(save);
                                    }
                                });
                                dd.attendance = $scope.attedancedetails;
                            });

                            $scope.attendanceper = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.attendanceper = [];
                                angular.forEach($scope.getsavedlist, function (save) {
                                    if (dd.amstid === save.amstid && save.ismsid === 5004) {
                                        $scope.attendanceper.push(save);
                                    }
                                });
                                dd.attendanceperdetails = $scope.attendanceper;
                            });

                            $scope.rank = [];
                            angular.forEach($scope.getstudentdetails, function (dd) {
                                $scope.rank = [];
                                angular.forEach($scope.getsavedlist, function (save) {
                                    if (dd.amstid === save.amstid && save.ismsid === 5005) {
                                        $scope.rank.push(save);
                                    }
                                });
                                dd.rankdetails = $scope.rank;
                            });


                            console.log("MARKS");
                            console.log($scope.getstudentdetails);

                            angular.forEach($scope.getstudentdetails, function (d) {
                                angular.forEach($scope.promotiondetails, function (e) {
                                    if (d.amstid === e.amsT_Id) {
                                        d.resultpassfail = e.estmpP_TotalGrade + ' ' + e.estmpP_Result;
                                    }
                                });
                            });

                            angular.forEach($scope.yearlt, function (ye) {
                                if (parseInt($scope.asmaY_Id) === ye.asmaY_Id) {
                                    $scope.year = ye.asmaY_Year;
                                }
                            });
                            $scope.report = true;
                            $scope.printbutton = false;
                        } else {
                            swal("No Records Found");
                        }



                    } else {
                        swal("No Records Found");
                    }

                });

            } else {
                $scope.submitted = true;
            }


        };



        //to print
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/MaldaProgressReportPdf .css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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

        $scope.cancel = function () {
            $state.reload();
        };


    }
})();
