(function () {
    'use strict';
    angular.module('app').controller('ExamStudentSubjectPaperTypeMappingController', ExamStudentSubjectPaperTypeMappingController);
    ExamStudentSubjectPaperTypeMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter'];

    function ExamStudentSubjectPaperTypeMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter) {

        $scope.userPrivileges = "";
        $scope.obj = {};
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }
        $scope.Left_Flag = false;
        $scope.Deactive_Flag = false;
        $scope.RemoveRecordsList = [];
        $scope.obj.all = false;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("StudentMapping/BindData_PT", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.year_list = promise.yearlist;
                    $scope.class_list = promise.classlist;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                }
            });
        };

        $scope.OnChangeYear_PT = function () {
            $scope.class_list = [];
            $scope.ASMCL_Id = "";
            $scope.section_list = [];
            $scope.ASMSL_Id = "";
            $scope.exam_list = [];
            $scope.EME_Id = "";
            $scope.subject_list = [];
            $scope.ISMS_Id = "";
            $scope.studlist = [];
            $scope.RemoveRecordsList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentMapping/OnChangeYear_GetClass_PT", data).then(function (promise) {
                if (promise !== null) {
                    $scope.class_list = promise.classlist;
                }
            });
        };

        $scope.OnChangeClass_PT = function () {
            $scope.section_list = [];
            $scope.ASMSL_Id = "";
            $scope.exam_list = [];
            $scope.EME_Id = "";
            $scope.subject_list = [];
            $scope.ISMS_Id = "";
            $scope.studlist = [];
            $scope.RemoveRecordsList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("StudentMapping/OnChangeClass_GetSection_PT", data).then(function (promise) {
                if (promise !== null) {
                    $scope.section_list = promise.seclist;
                }
            });
        };

        $scope.OnChangeSection_PT = function () {
            $scope.exam_list = [];
            $scope.EME_Id = "";
            $scope.subject_list = [];
            $scope.ISMS_Id = "";
            $scope.studlist = [];
            $scope.RemoveRecordsList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("StudentMapping/OnChangeSection_GetExam_PT", data).then(function (promise) {
                if (promise !== null) {
                    $scope.exam_list = promise.examlist;
                }
            });
        };

        $scope.OnChangeExam_PT = function () {
            $scope.subject_list = [];
            $scope.ISMS_Id = "";
            $scope.studlist = [];
            $scope.RemoveRecordsList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "EME_Id": $scope.EME_Id
            };

            apiService.create("StudentMapping/OnChangeExam_GetSubject_PT", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subject_list = promise.subjlist;
                }
            });
        };

        $scope.OnChangeSubject_PT = function () {
            $scope.studlist = [];
            $scope.RemoveRecordsList = [];
        };

        $scope.OnSearch_PT = function () {
            $scope.RemoveRecordsList = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "Left_Flag": $scope.Left_Flag,
                    "Deactive_Flag": $scope.Deactive_Flag
                };

                apiService.create("StudentMapping/OnSearch_PT", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.question_papertype_list === undefined && promise.question_papertype_list === null && promise.question_papertype_list.length === 0) {
                            swal("Master Exam Question Paper Type Not Entered");
                        }
                        else {
                            $scope.studlist = promise.studlist;
                            $scope.studmaplist = promise.studmaplist;
                            $scope.question_papertype_list = promise.question_papertype_list;
                            angular.forEach($scope.studlist, function (stu) {
                                stu.checked = false;
                                stu.delete_flag = false;
                                stu.disabled_flag = false;
                                stu.ESEWPT_Id = 0;
                                angular.forEach($scope.studmaplist, function (stu_map) {
                                    if (stu.amsT_Id === stu_map.amsT_Id) {
                                        stu.checked = true;
                                        stu.disabled_flag = true;
                                        stu.remove_flag = 1;
                                        stu.EMPATY_Id = stu_map.empatY_Id;
                                        stu.ESEWPT_Id = stu_map.esewpT_Id;
                                    }
                                });
                            });
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.OnSave_PT_Details = function (obj_myform) {
            if (obj_myform.$valid) {

                if ($scope.RemoveRecordsList.length > 0) {

                    var mgs = "";
                    var confirmmgs = "";
                    swal({
                        title: "Are you sure",
                        text: "Do You Want To Remove The Mapping For" + $scope.RemoveRecordsList.length + " Students",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Remove it!",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.OnSave_PT(obj_myform);
                            }
                            else {
                                swal("Record Remove Cancelled");
                            }
                        });
                } else {
                    $scope.OnSave_PT(obj_myform);
                }
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.OnSave_PT = function (obj_myform) {
            if (obj_myform.$valid) {
                $scope.selected_students = [];
                angular.forEach($scope.studlist, function (stu) {
                    if (stu.checked) {
                        $scope.selected_students.push({ AMST_Id: stu.amsT_Id, ESEWPT_Id: stu.ESEWPT_Id, EMPATY_Id: stu.EMPATY_Id });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "Selected_students": $scope.selected_students,
                    "Selected_RemoveRecordsList": $scope.RemoveRecordsList,
                };
                apiService.create("StudentMapping/OnSave_PT", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.OnClickRemove = function (stu_user) {
            var mgs = "";
            var confirmmgs = "";
            swal({
                title: "Are you sure",
                text: "Do You Want To Remove This Record",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Remove it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "ASMCL_Id": $scope.ASMCL_Id,
                            "ASMS_Id": $scope.ASMS_Id,
                            "EME_Id": $scope.EME_Id,
                            "ISMS_Id": $scope.ISMS_Id,
                            "AMST_Id": stu_user.amsT_Id,
                        };

                        apiService.create("StudentMapping/OnClickRemove_PT", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("You Can Not Delete This Record Because Marks Entered For The Selected Student");
                            }
                            else {
                                stu_user.disabled_flag = true;
                                stu_user.delete_flag = true;
                                stu_user.checked = false;
                                $scope.RemoveRecordsList.push({
                                    AMST_Id: stu_user.amsT_Id, StudentName: stu_user.amsT_FirstName, Admno: stu_user.amsT_AdmNo, RollNo: stu_user.amaY_RollNo,
                                    ESEWPT_Id: stu_user.ESEWPT_Id, EMPATY_Id: stu_user.EMPATY_Id
                                });
                                swal("Details Removed");
                            }
                        });
                    }
                    else {
                        swal("Record Remove Cancelled");
                    }
                });
        };

        $scope.RemoveRecords = function (stu_rmv_user, index) {
            $scope.RemoveRecordsList.splice(index, 1);

            angular.forEach($scope.studlist, function (dd) {
                if (dd.amsT_Id === stu_rmv_user.AMST_Id) {
                    dd.disabled_flag = false;
                    dd.delete_flag = false;
                    dd.checked = true;
                }
            });
        };

        // COMMON FUNCTIONS
        $scope.OnClear_PT = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.obj.all;
            angular.forEach($scope.studlist, function (itm) {
                if (itm.disabled_flag === false) {
                    itm.checked = toggleStatus;
                }                
            });
        };

        $scope.optionToggled = function () {
            $scope.obj.all = $scope.studlist.every(function (itm) { return itm.checked; })
        };

        $scope.isOptionsRequired = function () {
            return !$scope.studlist.some(function (options) {
                return options.checked;
            });
        };
    }
})();