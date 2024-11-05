(function () {
    'use strict';
    angular.module('app').controller('ExamPromotionCalculation_SSSEController', ExamPromotionCalculation_SSSEController)
    ExamPromotionCalculation_SSSEController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamPromotionCalculation_SSSEController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("ExamCalculation_SSSE/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
            });
        };

        $scope.submitted = false;
        $scope.promotionsaveddata = function (asmaY_Id, asmcL_Id, asmS_Id) {
            if ($scope.myForm.$valid) {
                var ESTMPP_PublishToStudentFlg = 0;
                //if ($scope.multibranch === true) {
                //    ESTMPP_PublishToStudentFlg = 1;
                //} else {
                //    ESTMPP_PublishToStudentFlg = 0;
                //}
                var data = {
                    "ASMAY_Id": asmaY_Id,
                    "ASMCL_Id": asmcL_Id,
                    "ASMS_Id": asmS_Id,
                    "ESTMPP_PublishToStudentFlg": ESTMPP_PublishToStudentFlg
                };
                apiService.create("ExamCalculation_SSSE/promotionsaveddata", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal('Marks Calculated Successfully');
                    }
                    else {
                        swal('Marks Not Calculated !!!');
                    }
                    $scope.clear();
                });
            }
            else {
                $scope.submitted = true;
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

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.get_classes = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("ExamCalculation_SSSE/get_classes", data).
                then(function (promise) {
                    $scope.clslist = promise.classlist;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
                    $scope.seclist = [];
                    $scope.exsplt = [];
                    if (promise.classlist === null || promise.classlist === "") {
                        swal("Classes are Not Mapped To Selected Academic Year!!!");
                    }
                });
        };

        $scope.get_cls_sections = function (cls_id) {

            if ($scope.asmaY_Id !== "" && $scope.asmaY_Id !== undefined && $scope.asmaY_Id !== null) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
                    $scope.exsplt = [];
                    if (promise.seclist === null || promise.seclist === "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                });
            }
            else {

                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";

            }
        };

        $scope.onchangesection = function (cls_id) {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };

            apiService.create("PromotionCalculation/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.countcalculated = promise.countcalculated;
                }
            });
        };


        $scope.get_exams = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            if (ASMAY_Id !== null && ASMAY_Id !== undefined && ASMCL_Id !== null && ASMCL_Id !== undefined) {
                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id
                };

                apiService.create("ExamCalculation_SSSE/get_exams", data).then(function (promise) {

                    $scope.emE_Id = "";
                    $scope.exsplt = promise.exmstdlist;
                    if (promise.exmstdlist === null || promise.exmstdlist === "") {
                        swal("Exams are Not Mapped To Selected Class And Section!!!");
                    }
                });
            }
            else {
                swal("Please Select Academic Year  And Class First !!!");
                $scope.asmS_Id = "";
            }
        };

        $scope.publishtostudentportal = function (asmaY_Id, asmcL_Id, asmS_Id) {
            if ($scope.myForm.$valid) {
                swal({
                    title: "Do you want to Publish The Marks To Students Portals",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Publish!",
                    cancelButtonText: "No, Cancel Publish!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var ESTMP_PublishToStudentFlg = 0;
                            var data = {
                                "ASMAY_Id": asmaY_Id,
                                "ASMCL_Id": asmcL_Id,
                                "ASMS_Id": asmS_Id
                            };

                            apiService.create("ExamCalculation_SSSE/publishtostudentportal", data).then(function (promise) {
                                if (promise.returnval === true) {
                                    swal('Marks Published To Student');
                                } else {
                                    swal('Failed To Publish');
                                }
                                $state.reload();
                            });
                        }
                        else {
                            swal("Marks Publish Process Is Cancelled");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clear = function () {
             $state.reload();
            //  $scope.BindData();
            //$scope.asmaY_Id = "";
            //$scope.asmcL_Id = "";
            //$scope.asmS_Id = "";
            //$scope.emE_Id = "";
            //$scope.clslist = [];
            //$scope.seclist = [];
            //$scope.exsplt = [];
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
        };
    }
})();