(function () {
    'use strict';
    angular
        .module('app')
        .controller('Master_TrainingTopicController', Master_TrainingTopicController);

    Master_TrainingTopicController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function Master_TrainingTopicController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.Id = 0;
        //get data
        $scope.getAllDetail = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            apiService.getURI("Training_Master/getdatatopic", pageid)
                .then(function (promise) {
                    $scope.getmasterdata = promise.getmasterdata; 
                });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

       // edit
        $scope.edibl = {};
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmtT_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_Topic", pageid).
                then(function (promise) {
                    $scope.Id = promise.edit_topic[0].hrmtT_Id;
                    $scope.hrmtT_Topic = promise.edit_topic[0].hrmtT_Topic;
                });
        };

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMTT_Topic": $scope.hrmtT_Topic,
                    "HRMTT_Id": $scope.Id
                };
                apiService.create("Training_Master/SaveEdit_Topic", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                $scope.getmasterdata = promise.getmasterdata; 
                                swal('Topic Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnvalue === "Add") {
                                $scope.getmasterdata = promise.getmasterdata; 
                                swal('Topic Saved Successfully');
                                $state.reload();
                            }
                            else if (promise.returnvalue === "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Topic Already Exist');
                            }
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        //deactive
        $scope.deactive = function (bL, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.hrmtT_ActiveFlg === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Topic ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deactivate_Topic", bL).
                            then(function (promise) {
                                if (promise.returnvalue === "false") {
                                    swal('Training Topic Deactivated Successfully');
                                }
                                else if (promise.returnvalue === "true") {
                                    swal('Training Topic Activated Successfully');
                                }                               
                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }

})();



