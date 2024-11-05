(function () {
    'use strict';
    angular.module('app').controller('PromotionCalculationController', PromotionCalculationController)

    PromotionCalculationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function PromotionCalculationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("PromotionCalculation/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
            });
        };

        $scope.get_classes = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("PromotionCalculation/get_classes", data).then(function (promise) {
                $scope.clslist = promise.classlist;
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";
                $scope.seclist = [];
                if (promise.classlist == null || promise.classlist == "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }
            });
        };

        $scope.get_cls_sections = function (cls_id) {
            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.asmaY_Id != null) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };

                apiService.create("PromotionCalculation/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                    if (promise.seclist == null || promise.seclist == "") {
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

        $scope.submitted = false;
        $scope.saveddata = function (asmaY_Id, asmcL_Id, asmS_Id) {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": asmaY_Id,
                    "ASMCL_Id": asmcL_Id,
                    "ASMS_Id": asmS_Id
                };
                apiService.create("PromotionCalculation/Calculation", data).then(function (promise) {
                    swal('Promotion Calculated Successfully');
                    $scope.clear();
                });

            }
            else {
                $scope.submitted = true;
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

                            apiService.create("PromotionCalculation/publishtostudentportal", data).then(function (promise) {
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.clslist = [];
            $scope.seclist = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
    }

})();