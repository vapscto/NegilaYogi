
(function () {
    'use strict';
    angular.module('app').controller('ExamTermWiseParticipationController', ExamTermWiseParticipationController)
    ExamTermWiseParticipationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ExamTermWiseParticipationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        $scope.select_cat = false;
        $scope.all_s = false;

        var paginationformasters = 0;
        var copty = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("ExamTermWiseRemarks/Getdetails_Participate", pageid).then(function (promise) {
                $scope.year_list = promise.getyear;
                $scope.getdetails = promise.getdetails;
            });
        };

        $scope.get_class = function () {
            $scope.select_cat = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ExamTermWiseRemarks/get_class", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classlist = promise.getclass;
                    if ($scope.classlist.length > 0) {
                        $scope.classlist = promise.getclass;
                    } else {
                        swal("Class Teacher Is Not Mapped For This Staff");
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.get_section = function () {
            $scope.select_cat = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("ExamTermWiseRemarks/get_section", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getsection = promise.getsection;
                    if ($scope.getsection.length > 0) {
                        $scope.sectionlist = promise.getsection;
                    } else {
                        swal("No Records Found");
                    }
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        $scope.get_exam = function () {
            $scope.select_cat = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("ExamTermWiseRemarks/get_term", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getexam = promise.getterm;
                    if ($scope.getexam.length > 0) {
                        $scope.exam_list = promise.getterm;
                    } else {
                        swal("No Exam Is Mapped For this Combination");
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.submitted = false;
        $scope.search_student = function () {
            $scope.select_cat = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ECT_Id": $scope.ECT_Id
                };

                apiService.create("ExamTermWiseRemarks/search_student_participate", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.student = promise.getstudentdetails;
                        $scope.getsavedetails = promise.getsavedetails;

                        if ($scope.student !== null && $scope.student.length > 0) {
                            $scope.select_cat = true;
                            angular.forEach($scope.getsavedetails, function (studsave) {
                                angular.forEach($scope.student, function (stud) {
                                    if (parseInt(stud.amsT_Id) === parseInt(studsave.amsT_Id)) {
                                        stud.checkedvalue = true;
                                        stud.ESTTA_Remarks = studsave.esttA_Remarks;
                                        stud.ESTTA_Id = studsave.esttA_Id;
                                    }
                                });
                            });
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.sortKey = 'studentname';
        $scope.sortReverse = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.submitted1 = false;
        $scope.save_details = function () {

            $scope.temp_student_details = [];
            angular.forEach($scope.student, function (dd) {
                if (dd.checkedvalue === true) {
                    $scope.temp_student_details.push({
                        AMST_Id: dd.amsT_Id, studentname: dd.studentname,
                        ESTTA_Remarks: dd.ESTTA_Remarks, ESTTA_Id: dd.ESTTA_Id
                    });
                }
            });

            if ($scope.temp_student_details.length === 0) {
                swal("Select Alteast One Student To Save The Details");
                return;
            }

            if ($scope.myForm1.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ECT_Id": $scope.ECT_Id,
                    "saved_participate_details": $scope.temp_student_details
                };

                apiService.create("ExamTermWiseRemarks/save_participate_details", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved /Update  Successfully");
                        } else {
                            swal("Failed To Save /Update Record");
                        }
                    } else {
                        swal("Failed To Save /Update Record");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.ViewStudentParticipateDetails = function (dd) {

            var data = {
                "ASMAY_Id": dd.asmaY_Id,
                "ASMCL_Id": dd.asmcL_Id,
                "ASMS_Id": dd.asmS_Id,
                "ECT_Id": dd.ecT_Id,
            };

            apiService.create("ExamTermWiseRemarks/ViewStudentParticipateDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewstudentdetails = promise.viewstudentdetails;
                    $('#popup').modal('show');
                }
            });
        };

        $scope.toggleAll_S = function (stas) {
            var toggleStatus = stas;
            angular.forEach($scope.student, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        };

        $scope.optionToggled = function () {
            $scope.all_s = $scope.student.every(function (itm) { return itm.checkedvalue; });
        };

        $scope.clear = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
    }
})();