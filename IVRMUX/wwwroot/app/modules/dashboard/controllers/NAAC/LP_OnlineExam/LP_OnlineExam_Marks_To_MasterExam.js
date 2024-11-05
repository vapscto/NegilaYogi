(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExam_Marks_To_MasterExamController', LP_OnlineExam_Marks_To_MasterExamController)

    LP_OnlineExam_Marks_To_MasterExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function LP_OnlineExam_Marks_To_MasterExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }


        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;

        $scope.maxdate = new Date();

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.reportbtn = true;

        $scope.loaddata = function () {
            var pageid = 4;
            apiService.getURI("LP_OnlineStudentExam/getloaddatareport", pageid).then(function (promise) {
                $scope.getyearlist = promise.getyearlist;

            });

        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.EME_Id = "";
            $scope.getmasterexamdetails = [];
            $scope.result = [];
            $scope.studentdetails = [];
            $scope.reportbtn = true;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangeyear", data).then(function (promise) {
                $scope.getclasslist = promise.getclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.EME_Id = "";
            $scope.getmasterexamdetails = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.studentdetails = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangeclass", data).then(function (promise) {
                $scope.getsectionlist = promise.getsectionlist;
            });

        };

        $scope.OnchangeSection = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.EME_Id = "";
            $scope.getmasterexamdetails = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;
            $scope.studentdetails = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id

            };
            apiService.create("LP_OnlineStudentExam/OnchangeSection", data).then(function (promise) {
                $scope.getsubjectlist = promise.getsubjectlist;
            });
        };

        $scope.onchangesubject = function () {
            $scope.studentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.EME_Id = "";
            $scope.getexamlist = [];
            $scope.getmasterexamdetails = [];
            $scope.result = [];
            $scope.reportbtn = true;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangesubject_studentmarks", data).then(function (promise) {
                $scope.getexamlist = promise.getexamlist;
            });
        };

        $scope.OnChangeExam = function () {
            $scope.getmasterexamdetails = [];
            $scope.EME_Id = "";
            $scope.studentdetails = [];
            angular.forEach($scope.getexamlist, function (dd) {
                if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                    $scope.obj.FMCB_fromDATE = new Date(dd.lpmoeeX_FromDateTime);
                    $scope.obj.FMCB_toDATE = new Date(dd.lpmoeeX_ToDateTime);
                }
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "LPMOEEX_Id": $scope.LPMOEEX_Id
            };

            apiService.create("LP_OnlineStudentExam/OnChangeExam", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getlpexamdetails = promise.getlpexamdetails;

                    if ($scope.getlpexamdetails !== null && $scope.getlpexamdetails.length > 0 && $scope.getlpexamdetails[0].emE_Id > 0) {
                        $scope.getmasterexamdetails = promise.getmasterexamdetails;

                        $scope.EME_Id = $scope.getlpexamdetails[0].emE_Id;
                        $scope.LPMOEEX_TotalMarks = $scope.getlpexamdetails[0].lpmoeeX_TotalMarks;

                    } else {
                        swal("For This Master Exam Is Not Mapped");
                    }
                }
            });
        };


        $scope.GetExam_OE_StudentList = function () {

            $scope.submitted1 = true;
            $scope.reportbtn = true;
            $scope.result = [];
            $scope.studentdetails = [];
            if ($scope.myForm.$valid) {
                $scope.fromdate = new Date($scope.obj.FMCB_fromDATE).toDateString();
                $scope.todate = new Date($scope.obj.FMCB_toDATE).toDateString();

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id

                };

                apiService.create("LP_OnlineStudentExam/GetExam_OE_StudentList", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.get_lpoe_studentmarks !== null && promise.get_lpoe_studentmarks.length > 0) {
                            $scope.studentdetails = promise.studentdetails;
                            $scope.get_lpoe_studentmarks = promise.get_lpoe_studentmarks;
                            $scope.get_exam_studentmarks = promise.get_exam_studentmarks;

                            $scope.get_yearly_exam_subject_details = promise.get_yearly_exam_subject_details;

                            var count = 0;
                            $scope.configurationsettings = promise.configuration;

                            if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "Name") {
                                $scope.sortKey = "amsT_FirstName";
                            }

                            else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "AdmNo") {
                                $scope.sortKey = "amsT_AdmNo";
                            }

                            else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "RollNo") {
                                $scope.sortKey = "amaY_RollNo";
                            }

                            else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype === "RegNo") {
                                $scope.sortKey = "amsT_RegistrationNo";
                            }

                            else {
                                $scope.sortKey = "amsT_FirstName";
                            }
                            $scope.display = true;
                            $scope.regno = false;
                            $scope.admno = false;
                            $scope.rollno = false;

                            if ($scope.configurationsettings !== null && $scope.configurationsettings.length > 0) {
                                if ($scope.configurationsettings[0].exmConfig_RegnoColumnDisplay === true) {
                                    $scope.regno = true;
                                    count = count + 1;
                                }
                                if ($scope.configurationsettings[0].exmConfig_AdmnoColumnDisplay === true) {
                                    $scope.admno = true;
                                    count = count + 1;
                                }
                                if ($scope.configurationsettings[0].exmConfig_RollnoColumnDisplay === true) {
                                    $scope.rollno = true;
                                    count = count + 1;
                                }
                                if (count === 0) {
                                    $scope.admno = true;
                                    $scope.rollno = true;
                                }
                            } else {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }

                            $scope.update = $scope.get_exam_studentmarks.length;
                            var marksorgrade_entryflag = $scope.get_yearly_exam_subject_details[0].eyceS_MarksGradeEntryFlg;
                            $scope.marksorgrade_entryflag = marksorgrade_entryflag;

                            if ($scope.get_yearly_exam_subject_details[0].eyceS_MarksGradeEntryFlg === 'M') {
                                $scope.allowpattern = "[0-9A-Z.]";
                                $scope.placeholder = "Enter Marks...";
                            }
                            else if ($scope.get_yearly_exam_subject_details[0].eyceS_MarksGradeEntryFlg === 'G') {
                                $scope.allowpattern = "[A-Z0-9+-]";
                                $scope.placeholder = "Enter Grade...";
                            }

                            $scope.maxmarks = $scope.get_yearly_exam_subject_details[0].eyceS_MaxMarks;
                            $scope.minmarks = $scope.get_yearly_exam_subject_details[0].eyceS_MinMarks;
                            $scope.marksentryfor = $scope.get_yearly_exam_subject_details[0].eyceS_MarksEntryMax;

                            angular.forEach($scope.getsubjectlist, function (dd) {
                                if (parseInt($scope.ISMS_Id) === dd.ismS_Id) {
                                    $scope.subjectnames = dd.ismS_SubjectName;
                                }
                            });

                            angular.forEach($scope.studentdetails, function (stu) {
                                stu.flag = "ME";
                                stu.selected_s = false;
                                angular.forEach($scope.get_lpoe_studentmarks, function (stu_marks) {
                                    if (stu.amsT_Id === stu_marks.amsT_Id) {
                                        stu.flag = stu_marks.lpstueX_TotalMarks != undefined && stu_marks.lpstueX_TotalMarks != null
                                            && stu_marks.lpstueX_TotalMarks != "" ? "OE" : "ME";
                                        stu.selected_s = stu_marks.lpstueX_TotalMarks != undefined && stu_marks.lpstueX_TotalMarks != null
                                            && stu_marks.lpstueX_TotalMarks != "" ? true : false;
                                        stu.ISMS_Id = stu_marks.ismS_Id;
                                        if ($scope.LPMOEEX_TotalMarks === $scope.marksentryfor) {
                                            stu.ESTM_Marks = stu_marks.lpstueX_TotalMarks;
                                        }
                                        else if ($scope.LPMOEEX_TotalMarks > $scope.marksentryfor) {
                                            var ratiodivided = $scope.LPMOEEX_TotalMarks / $scope.marksentryfor;
                                            stu.ESTM_Marks = stu_marks.lpstueX_TotalMarks / ratiodivided;
                                        }
                                        else if ($scope.LPMOEEX_TotalMarks < $scope.marksentryfor) {
                                            var ratiomultiple = $scope.marksentryfor / $scope.LPMOEEX_TotalMarks;
                                            stu.ESTM_Marks = stu_marks.lpstueX_TotalMarks * ratiomultiple;
                                        }
                                    }
                                });
                            });

                            if ($scope.get_exam_studentmarks !== null && $scope.get_exam_studentmarks.length > 0) {
                                angular.forEach($scope.studentdetails, function (stu) {
                                    angular.forEach($scope.get_exam_studentmarks, function (stu_marks) {
                                        if (stu.amsT_Id === stu_marks.amsT_Id && stu.flag === "ME") {
                                            stu.selected_s = stu_marks.estM_Marks != undefined && stu_marks.estM_Marks != null && stu_marks.estM_Marks != "" ?
                                                true : false;
                                            if (marksorgrade_entryflag === 'M') {
                                                if (stu_marks.estM_Flg === null || stu_marks.estM_Flg === "") {
                                                    var flags = 0;
                                                    var mraks = '' + stu_marks.estM_Marks + '';

                                                    if (mraks.match(/[.]/i)) {
                                                        var sliptmarks = mraks.split('.');
                                                        if (sliptmarks[0] >= 1 && sliptmarks[0] < 10) {
                                                            flags = 1;
                                                        }
                                                    } else {
                                                        if (stu_marks.estM_Marks >= 1 && stu_marks.estM_Marks < 10) {
                                                            flags = 1;
                                                        }
                                                    }
                                                    if (flags === 1) {
                                                        stu.ESTM_Marks = '0' + stu_marks.estM_Marks.toString();
                                                    } else {
                                                        stu.ESTM_Marks = stu_marks.estM_Marks.toString();
                                                    }
                                                }
                                                else {
                                                    stu.ESTM_Marks = stu_marks.estM_Flg.toString();
                                                }
                                            }
                                            else if (marksorgrade_entryflag === 'G') {
                                                if (stu_marks.estM_Flg === '' || stu_marks.estM_Flg === null) {
                                                    stu.ESTM_Marks = stu_marks.estM_Grade.toString();
                                                }
                                                else {
                                                    stu.ESTM_Marks = stu_marks.estM_Flg.toString();
                                                }
                                            }
                                        }
                                    });
                                });
                            }

                        } else {
                            swal("Online Exam Answer Paper Not Done Correction");
                        }
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.valid_marks_S = function (stu) {

            var obtainmarks = stu.ESTM_Marks.toString();
            if (obtainmarks.match(/[a-z]/i)) {
                if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M"
                    || obtainmarks.toUpperCase() === "OD") {
                    return true;
                } else {
                    stu.ESTM_Marks = "";
                    swal("Enter Only AB,L,M,OD");
                }
            } else {

                if (Number(obtainmarks) > Number($scope.marksentryfor)) {
                    stu.ESTM_Marks = "";
                    swal("Entered Marks Should Not Be More Than " + $scope.marksentryfor);
                }
            }
        };


        $scope.SaveOE_Marks_ME = function () {
            if ($scope.myForm.$valid) {
                $scope.main_save_list = [];
                angular.forEach($scope.studentdetails, function (stu) {
                    if (stu.selected_s) {
                        var ESTM_Marks = "";
                        var ESTM_Flg = "";
                        var ESTM_Grade = "";
                        var obtainmarks = stu.ESTM_Marks.toString();
                        if (obtainmarks.match(/[a-z]/i)) {
                            if (obtainmarks.toUpperCase() === "AB" || obtainmarks.toUpperCase() === "L" || obtainmarks.toUpperCase() === "M"
                                || obtainmarks.toUpperCase() === "OD") {
                                ESTM_Flg = obtainmarks;
                                ESTM_Marks = null;
                                ESTM_Grade = 'null';
                            } else {
                                ESTM_Flg = "";
                                ESTM_Marks = obtainmarks;
                                ESTM_Grade = 'null';
                            }
                        } else {
                            ESTM_Flg = "";
                            ESTM_Marks = obtainmarks;
                            ESTM_Grade = 'null';
                        }

                        $scope.main_save_list.push({
                            AMST_Id: stu.amsT_Id, ISMS_Id: Number($scope.ISMS_Id), ESTM_Marks: Number(ESTM_Marks), ESTM_Flg: ESTM_Flg,
                            ESTM_Grade: ESTM_Grade, ESTM_MarksGradeFlg: $scope.marksorgrade_entryflag
                        });
                    }
                });

                if ($scope.main_save_list.length > 0) {

                    swal({
                        title: "Are you sure",
                        text: "Do You Want To Transfer Marks To Exam ?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes , Transfer",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                var data = {
                                    "ASMAY_Id": $scope.ASMAY_Id,
                                    "ASMCL_Id": $scope.ASMCL_Id,
                                    "ISMS_Id": $scope.ISMS_Id,
                                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                                    "ASMS_Id": $scope.ASMS_Id,
                                    "EME_Id": $scope.EME_Id,
                                    "main_save_list": $scope.main_save_list
                                };

                                apiService.create("LP_OnlineStudentExam/SaveOE_Marks_ME", data).then(function (promise) {
                                    if (promise !== null) {
                                        if (promise.message === "Save") {
                                            swal("Marks Are Save/Updated Successfully");
                                            $state.reload();
                                        } else {
                                            swal("Failed To Save/Update Marks");
                                        }
                                    }
                                });
                            }
                            else {
                                swal("Marks Transfer Cancelled");
                            }
                        });
                } else {
                    swal("Select Students To Save The Marks");
                }
            }
            else {
                $scope.submitted1 = true;
            }

        };

        $scope.toggleAll_S = function (stas) {
            var toggleStatus = stas;
            angular.forEach($scope.studentdetails, function (itm) {
                if (toggleStatus === false && itm.flag === "OE") {
                    itm.selected_s = true;
                } else {
                    itm.selected_s = toggleStatus;
                }
            });
        };

        $scope.optionToggled_S = function () {
            $scope.all_s = $scope.studentdetails.every(function (itm) { return itm.selected_s; });
        };


        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };
    }
})();