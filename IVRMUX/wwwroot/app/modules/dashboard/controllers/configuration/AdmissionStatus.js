(function () {
    'use strict';
    angular
        .module('app')
        .controller('AdmissionStatusController', AdmissionStatusController)

    AdmissionStatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function AdmissionStatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            var pageid = 2;
            apiService.getURI("AdmissionStatus/getalldetails", pageid).
                then(function (promise) {
                    $scope.pages = promise.academicstatuslist;

                    //$scope.totalItems = $scope.pages.length;
                    //$scope.numPerPage = 5;
                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.searchsource = function () {
            var entereddata = $scope.search;

            var data = {
                "PAMS_Status": $scope.search,
                "PAMS_StatusFlag": $scope.type
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("AdmissionStatus/1", data).
                then(function (promise) {
                    $scope.pages = promise.pagesdata;
                    swal("searched Successfully");
                })
        }

        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.pamS_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("AdmissionStatus/deletepages", pageid).
                            then(function (promise) {

                                $scope.pages = promise.academicstatuslist;

                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                    $scope.PAMS_Status = "";
                                    $scope.PAMS_StatusFlag = "";
                                }
                                else {
                                    swal('Record Not Deleted Successfully!');
                                    $scope.PAMS_Status = "";
                                    $scope.PAMS_StatusFlag = "";
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }


        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pamS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("AdmissionStatus/getdetails", pageid).
                then(function (promise) {
                    $scope.PAMS_Id = promise.academicstatuslist[0].pamS_Id;
                    $scope.PAMS_Status = promise.academicstatuslist[0].pamS_Status;
                    $scope.PAMS_StatusFlag = promise.academicstatuslist[0].pamS_StatusFlag;

                })
        }

        $scope.clearfields = function () {
            $state.reload();
        }

        $scope.savepages = function () {

            var data = {
                "PAMS_Status": $scope.PAMS_Status,
                "PAMS_StatusFlag": $scope.PAMS_StatusFlag,
                "active": 1,
                "PAMS_Id": $scope.PAMS_Id
            }

            apiService.create("AdmissionStatus/", data).
                then(function (promise) {

                    if (promise.returnval === true && promise.returnduplicatestatus != "Duplicate") {
                        swal('Record Saved/Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval === false && promise.returnduplicatestatus != "Duplicate") {
                        swal('Record Not Saved/Updated Successfully');
                    }
                    else if (promise.returnduplicatestatus == "Duplicate") {
                        swal('Duplicate Record');
                        return;
                    }

                })
        };
    }

})();