(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentFeedbackFormController', StudentFeedbackFormController)

    StudentFeedbackFormController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function StudentFeedbackFormController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.asgfE_Id = 0;
        $scope.flag = "";

        $scope.sortKey = 'asgfE_Id';
        $scope.sortReverse = true;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            apiService.getDATA("StudentFeedbackForm/getloaddata").
                then(function (promise) {
                    $scope.instname = promise.instname;
                    $scope.institutename = $scope.instname[0].mI_Name;
                    $scope.get_feedback = promise.get_feedback;
                    $scope.presentCountgrid = $scope.get_feedback.length;
                })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //====================== Save Feedback

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASGFE_FeedBack": $scope.asgfE_FeedBack,
                    "ASGFE_Id": $scope.asgfE_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("StudentFeedbackForm/savefeedback", data).then(function (promise) {
                    if (promise.returnval == true) {
                        if (promise.asgfE_Id == 0 || promise.asgfE_Id < 0) {
                            swal('Thanks for your Feedback....!!');
                        }
                        else if (promise.asgfE_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else {
                        if (promise.asgfE_Id == 0 || promise.asgfE_Id < 0) {
                            swal('Failed to submit, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $scope.asgfE_FeedBack = '';
            $scope.submitted = false;
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.ASGFE_Id = item.asgfE_Id;
            var dystring = "";
            if (item.asgfE_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.asgfE_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StudentFeedbackForm/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Feedback " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Feedback Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Feedback " + dystring + " Cancelled!!!");
                    }
                });
        }



    };
})();