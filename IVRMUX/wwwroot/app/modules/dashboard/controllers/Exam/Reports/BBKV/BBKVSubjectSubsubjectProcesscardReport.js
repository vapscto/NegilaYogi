
(function () {
    'use strict';
    angular.module('app').controller('BBKVSubjectSubsubjectProcesscardReportController', BBKVSubjectSubsubjectProcesscardReportController)

    BBKVSubjectSubsubjectProcesscardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BBKVSubjectSubsubjectProcesscardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {



        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.amsT_Date = new Date();

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("VikasaAssessment2Report/Getdetails/", pageid).
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.gradedetails = promise.getgradenames;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.amsT_Id = "";
                    $scope.emE_Id = "";
                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";
                    $scope.exsplt = "";
                });
        };

        $scope.get_class = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.sectionDropdown = "";
            $scope.exsplt = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("VikasaAssessment2Report/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;
                });
        };

        $scope.get_section = function () {
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("VikasaAssessment2Report/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;
                });
        };

        $scope.get_Exam = function () {


            $scope.emE_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("VikasaAssessment2Report/get_exam", data)
                .then(function (promise) {
                    $scope.exsplt = promise.examList;
                });
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

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "EMGR_Id": $scope.EMGR_Id
                };

                var temp_list = [];
                apiService.create("VikasaProgressReportExam/cbsesavedetails", data).
                    then(function (promise) {

                        $scope.temp = promise.cbsesavelist;
                        $scope.cbsesavelist = promise.cbsesavelist;
                        $scope.cbsesubexamlist = promise.cbsesubexamlist;
                        $scope.cbsestudentlist = promise.cbsestudentlist;
                        $scope.cbsesubjectlist = promise.cbsesubjectlist;
                        $scope.grade_details = promise.grade_details;



                        console.log("-");
                        console.log($scope.cbsesavelist);
                        console.log("--");
                        console.log($scope.cbsesubexamlist);
                        console.log("---");
                        console.log($scope.cbsestudentlist);
                        console.log("----");
                        console.log($scope.cbsesubjectlist);
                        console.log("-----");

                        angular.forEach($scope.cbsestudentlist, function (dd) {
                            $scope.tempnew = [];
                            angular.forEach($scope.cbsesavelist, function (ddd) {
                                if (dd.amst_id === ddd.amst_id) {
                                    $scope.tempnew.push(ddd);
                                }
                            });
                            dd.tempnewlist = $scope.tempnew;
                        });

                        angular.forEach($scope.cbsestudentlist, function (d) {
                            $scope.sublist = [];
                            angular.forEach(d.tempnewlist, function (dd) {

                                if ($scope.sublist.length === 0) {
                                    $scope.sublist.push({ subjectname: dd.subjectname, subid: dd.subid, flag: dd.flag });
                                } else if ($scope.sublist.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.sublist, function (exm) {
                                        if (exm.subid === dd.subid) {
                                            al_exm_cnt += 1;
                                        }
                                    });
                                    if (al_exm_cnt === 0) {
                                        $scope.sublist.push({ subjectname: dd.subjectname, subid: dd.subid, flag: dd.flag });
                                    }
                                }

                            });
                            d.subjectlistnew = $scope.sublist;

                        });



                        var stu_subj_list_remaks = [];
                        $scope.remarks = promise.remarks;

                        $scope.stud_work_attendence = promise.work_attendence;
                        $scope.stud_present_attendence = promise.present_attendence;

                        angular.forEach($scope.yearlt, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.classDropdown, function (dc) {
                            if (dc.asmcL_Id === parseInt($scope.asmcL_Id)) {
                                $scope.asmcL_ClassName = dc.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.sectionDropdown, function (ds) {
                            if (ds.asmS_Id === parseInt($scope.asmS_Id)) {
                                $scope.asmC_SectionName = ds.asmC_SectionName;
                            }
                        });

                        angular.forEach($scope.exsplt, function (ds) {
                            if (ds.emE_Id === parseInt($scope.emE_Id)) {
                                $scope.examname = ds.emE_ExamName;
                            }
                        });

                        $scope.issuedate = new Date($scope.amsT_Date);

                    });
            }
        };

        $scope.cancel = function () {
            $scope.asmcL_Id = "";
            $scope.emcA_Id = "";
            $scope.asmaY_Id = "";
            $scope.emG_Id = "";
            $scope.asmS_Id = "";
            $scope.subjectlt = "";
            $scope.subjectlt1 = "";
            $scope.studentlist = false;
            $state.reload();
        };

        //for print

        // end for print

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {
            $scope.stu_grandmin_marks = 0;
            angular.forEach(exm_subjs, function (itm) {
                if (itm.eyceS_AplResultFlg) {
                    angular.forEach(stu_subjs, function (itm1) {
                        if (itm1.ismS_Id === itm.ismS_Id) {
                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
                        }
                    });
                }
            });
        };

        $scope.VIKASAProgressCardReport = function () {
            var innerContents = document.getElementById("VIKASAProgressCard").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/BBKV/Progresscard/BBKVNEWProgressCardReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

    }

})();