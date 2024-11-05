(function () {
    'use strict';
    angular.module('app').controller('BISStudentProgressReportController', BISStudentProgressReportController)

    BISStudentProgressReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$compile']
    function BISStudentProgressReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $compile) {

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

        $scope.saveddata = function () {
            $scope.HHS_I_IV_grid = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.grandavgtotal = 0;

                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id
                };


                apiService.create("StudentProgressCardReport/BISStudentProgressCardReport", data).then(function (promise) {

                    if (promise !== null && promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                        $scope.HHS_I_IV_grid = true;
                        $scope.getstudentmarksdetails_temp = promise.getstudentmarksdetails;
                        $scope.JSHSReport = true;
                        $scope.getstudentmarksdetails = $scope.getstudentmarksdetails_temp;
                        $scope.getgroupdetails = promise.getgroupdetails;
                        $scope.getgroupexamdetails = promise.getgroupexamdetails;
                        $scope.studentdetails = promise.getstudentdetails;
                        $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;

                        $scope.groupwiseexamlist_temp = [];
                        $scope.groupwiseexamlist = [];

                        angular.forEach($scope.getgroupdetails, function (grp) {
                            var TotalMarks = 0;
                            angular.forEach($scope.getgroupexamdetails, function (grpexam) {
                                if (grp.empG_GroupName === grpexam.empG_GroupName) {
                                    TotalMarks += grpexam.empsgE_ForMaxMarkrs;

                                    $scope.groupwiseexamlist.push({
                                        EME_Id: grpexam.emE_Id, EME_ExamName: grpexam.emE_ExamName,
                                        EME_ExamCode: grpexam.emE_ExamCode, EMPSGE_ForMaxMarkrs: grpexam.empsgE_ForMaxMarkrs,
                                        EMPG_GroupName: grpexam.empG_GroupName, EMPG_DistplayName: grpexam.empG_DistplayName
                                    });

                                    $scope.groupwiseexamlist_temp.push({
                                        EME_Id: grpexam.emE_Id, EME_ExamName: grpexam.emE_ExamName,
                                        EME_ExamCode: grpexam.emE_ExamCode, EMPSGE_ForMaxMarkrs: grpexam.empsgE_ForMaxMarkrs,
                                        EMPG_GroupName: grpexam.empG_GroupName, EMPG_DistplayName: grpexam.empG_DistplayName
                                    });
                                }
                            });

                            $scope.groupwiseexamlist.push({
                                EME_Id: 980000, EME_ExamName: 'Total',
                                EME_ExamCode: 'Total', EMPSGE_ForMaxMarkrs: TotalMarks,
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });

                            $scope.groupwiseexamlist.push({
                                EME_Id: 980002, EME_ExamName: 'Percentage',
                                EME_ExamCode: 'Percentage', EMPSGE_ForMaxMarkrs: '100%',
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });

                            $scope.groupwiseexamlist.push({
                                EME_Id: 980001, EME_ExamName: 'Grade',
                                EME_ExamCode: 'Grade', EMPSGE_ForMaxMarkrs: '',
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });
                        });


                        angular.forEach($scope.getgroupdetails, function (grp) {
                            $scope.groupwiseexamlist.push({
                                EME_Id: 9800000, EME_ExamName: grp.empG_DistplayName,
                                EME_ExamCode: grp.empG_DistplayName, EMPSGE_ForMaxMarkrs: grp.empsG_PercentValue,
                                EMPG_GroupName: grp.empG_GroupName, EMPG_DistplayName: grp.empG_DistplayName
                            });
                        });


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

                        $scope.classteachername = "";
                        if (promise.clstchname !== null && promise.clstchname.length > 0) {
                            $scope.classteachername = promise.clstchname[0].hrmE_EmployeeFirstName;
                        }

                        $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                        angular.forEach($scope.studentdetails, function (stu) {
                            angular.forEach($scope.getpromotionremarksdetails, function (dd) {
                                if (stu.AMST_Id === dd.amsT_Id) {
                                    stu.remarks = dd.eprD_Remarks == null ? "" : dd.eprD_Remarks;
                                    stu.promotedclass = dd.eprD_ClassPromoted == null ? "" : dd.eprD_ClassPromoted;
                                    stu.PromotionName = dd.eprD_PromotionName == null ? "" : dd.eprD_PromotionName;
                                }
                            });
                        });

                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;

                        $scope.nonapplicablesubject_examwisemarks = promise.nonapplicablesubject_examwisemarks;

                        console.log($scope.studentdetails);

                        var e1 = angular.element(document.getElementById("report"));
                        $compile(e1.html(promise.htmlstring))(($scope));
                    } else {
                        swal("No Records Found");
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
        $scope.printToCart = function () {
            var innerContents = document.getElementById("HHS02").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCEHSTermReportCardPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();
