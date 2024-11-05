(function () {
    'use strict';
    angular.module('app').controller('Master_External_TrainingTypeController', Master_External_TrainingTypeController)

    Master_External_TrainingTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Master_External_TrainingTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        
        $scope.submitted = false;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("Master_External_TrainingType/onloaddata", pageid).then(function (promise) {
                $scope.getloaddetails = promise.getloaddetails;
                $scope.presentCountgrid = $scope.getloaddetails.length;
            });
        };
        $scope.hrmetrtY_Id = '0';
        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMETRTY_Id": $scope.hrmetrtY_Id,
                    "HRMETRTY_ExternalTrainingType": $scope.hrmetrtY_ExternalTrainingType,
                    "HRMETRTY_MinimumTrainingHrs": $scope.hrmetrtY_MinimumTrainingHrs
                };

                apiService.create("Master_External_TrainingType/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (user) {
            $scope.hrmetrtY_Id = user.hrmetrtY_Id;
            $scope.hrmetrtY_ExternalTrainingType = user.hrmetrtY_ExternalTrainingType;
            $scope.hrmetrtY_MinimumTrainingHrs = user.hrmetrtY_MinimumTrainingHrs;
        };


        $scope.deactiveY = function (user, SweetAlert) {
            var data = {
                "HRMETRTY_Id": user.hrmetrtY_Id
            };

            var dystring = "";
            if (user.hrmetrtY_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hrmetrtY_ActiveFlag === false) {
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
                        apiService.create("Master_External_TrainingType/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();

