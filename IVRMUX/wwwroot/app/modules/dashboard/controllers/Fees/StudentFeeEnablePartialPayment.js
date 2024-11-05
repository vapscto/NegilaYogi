(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentFeeEnablePartialPaymentController', StudentFeeEnablePartialPaymentController)
    StudentFeeEnablePartialPaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function StudentFeeEnablePartialPaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.FSEPP_RemarksDate = new Date();
        $scope.obj = {};

        $scope.onLoadGetData = function () {
            var pageid = 2;

            apiService.getURI("StudentFeeEnablePartialPayment/GetYearList", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                //$scope.sectionlist = promise.sectionlist;
                $scope.classcount = promise.fillclass;
                $scope.alldata = promise.alldata;
            })
        };
        $scope.submitted = false;
  
        $scope.interacted = function (field) {
            return $scope.submitted;
        }; 
        //==================student============//
        $scope.get_student = function () {
            debugger;
            $scope.studentlist = [];
            $scope.submitted = false;
            if ($scope.myForm.$valid) {

                if ($scope.amsC_Id == 'ALL') {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "AMSC_Id": $scope.amsC_Id,
                        "ASMCL_Id": $scope.asmcL_Id,                       

                    }
                }
                else {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "AMSC_Id": $scope.amsC_Id,

                    }
                }

                apiService.create("StudentFeeEnablePartialPayment/get_student", data).
                    then(function (promise) {
                        $scope.studentlist = promise.studentlist;
                        if ($scope.studentlist.length == 0 || $scope.studentlist == null) {
                            swal('No Student!!!');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }
        //=================end===================/////////////////
        // clear form data
        $scope.cancel = function () {
            $scope.search = "";
            $state.reload();
        };
        $scope.onselectclass = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("StudentFeeEnablePartialPayment/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.fillsection;
                })
        }
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "FSEPP_Id": $scope.FSEPP_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMST_Id": $scope.obj.amsT_Id.amsT_Id,
                    "FSEPP_RemarksDate": new Date($scope.FSEPP_RemarksDate).toDateString(),
                    "FSEPP_Remarks": $scope.FSEPP_Remarks
                }
                apiService.create("StudentFeeEnablePartialPayment/savedata", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Add") {
                            swal("Record Saved Successfully..");
                            $state.reload();
                        }
                        else if (promise.retrunMsg == "false") {
                            swal("Record Not saved / Updated..", 'Fail');

                        }
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $scope.clear();
                })

            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fsepP_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StudentFeeEnablePartialPayment/deactivate", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Activated") {
                                    swal("Record Activated successfully");
                                    $state.reload();
                                }
                                else if (promise.retrunMsg === "Deactivated") {
                                    swal("Record Deactivated successfully");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }
                            }

                        })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }

    }
})();