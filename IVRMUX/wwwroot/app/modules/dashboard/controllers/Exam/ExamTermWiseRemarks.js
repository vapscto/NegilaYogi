
(function () {
    'use strict';
    angular.module('app').controller('TermwiseRemarksController', TermwiseRemarksController)
    TermwiseRemarksController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function TermwiseRemarksController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        $scope.select_cat = false;

        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("ExamTermWiseRemarks/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyear;
            });
        };

        $scope.get_class = function () {
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
            if ($scope.myForm.$valid) {
                var ECT_Id = 0;
                $scope.temp_student_details = [];
                if ($scope.examtype === 'PE') {
                    ECT_Id = 0;
                } else {
                    ECT_Id = $scope.ECT_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ECT_Id": ECT_Id,
                    "indiorfinal": $scope.examtype
                };

                apiService.create("ExamTermWiseRemarks/search_student", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.student = promise.getstudentdetails;
                        $scope.getsavedetails = promise.getsavedetails;
                        if ($scope.student.length > 0) {
                            if ($scope.getsavedetails.length > 0) {
                                angular.forEach($scope.getsavedetails, function (studsave) {
                                    angular.forEach($scope.student, function (stud) {
                                        if (parseInt(stud.amsT_Id) === parseInt(studsave.amsT_Id)) {
                                            stud.checkedvalue = true;
                                            stud.ecterE_Remarks = studsave.ecterE_Remarks;
                                            stud.ECTERE_Id = studsave.ecterE_Id;
                                        }
                                    });
                                });

                            } else {
                                $scope.student = promise.getstudentdetails;
                            }
                            $scope.select_cat = true;
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
            var ECT_Id = 0;
            $scope.temp_student_details = [];
            if ($scope.examtype === 'PE') {
                ECT_Id = 0;
            } else {
                ECT_Id = $scope.ECT_Id;
            }

            $scope.temp_student_details = [];
            angular.forEach($scope.student, function (dd) {
                if (dd.checkedvalue === true) {
                    $scope.temp_student_details.push({ AMST_Id: dd.amsT_Id, studentname: dd.studentname, ECTERE_Remarks: dd.ecterE_Remarks, ECTERE_Id: dd.ECTERE_Id });
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
                    "ECT_Id": ECT_Id,
                    "indiorfinal": $scope.examtype,
                    "saved_remarks_details": $scope.temp_student_details
                };

                apiService.create("ExamTermWiseRemarks/save_details", data).then(function (promise) {

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

        $scope.clear = function () {
            $state.reload();    
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.getlist = function () {
            $scope.select_cat = false;
            if ($scope.examtype === 0) {
                $scope.student = [];
                $scope.getsavedetails = [];
            } else {
                $scope.EME_Id = "";
                $scope.student = [];
                $scope.getsavedetails = [];
            }
        };
    }

})();