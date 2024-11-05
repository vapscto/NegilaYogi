(function () {
    'use strict';
    angular
        .module('app')
        .controller('OtherCollegeStudentEntryController', OtherCollegeStudentEntryController)

    OtherCollegeStudentEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function OtherCollegeStudentEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searchValue = "";
        $scope.onload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("OtherCollegeStudentEntry/getdetails", pageid).
                then(function (promise) {
                    if (promise.count > 0) {

                        $scope.students = promise.otherstudentList;
                        $scope.presentCountgrid = $scope.students.length;
                        $scope.count = 1;
                    }
                    else {
                        swal("No Records Found");
                        $scope.count = 0;
                    }

                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };

        $scope.delete = function (det, SweetAlert) {
            $scope.delId = det.fmcosT_Id;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("OtherCollegeStudentEntry/delete/", $scope.delId).
                            then(function (promise) {

                                if (promise.returnval == "failed") {
                                    swal("Sorry.....You can not delete this record.Because it is already mapped");
                                    return;
                                }
                                else if (promise.returnval === "deleted") {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Not Deleted');
                                }
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }
        $scope.edit = function (det) {
            $scope.Id = det.fmcosT_Id;
            apiService.getURI("OtherCollegeStudentEntry/edit/", $scope.Id).
                then(function (promise) {
                    $scope.FMCOST_StudentName = promise.otherstudentList[0].fmcosT_StudentName;
                    $scope.FMCOST_StudentMobileNo = promise.otherstudentList[0].fmcosT_StudentMobileNo;
                    $scope.FMCOST_StudentEmailId = promise.otherstudentList[0].fmcosT_StudentEmailId;
                })
        }
        $scope.submitted = false;
        $scope.save = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "fmcosT_Id": $scope.Id,
                    "fmcosT_StudentName": $scope.FMCOST_StudentName,
                    "fmcosT_StudentMobileNo": $scope.FMCOST_StudentMobileNo,
                    "fmcosT_StudentEmailId": $scope.FMCOST_StudentEmailId
                }
                apiService.create("OtherCollegeStudentEntry/", data)

                    .then(function (promise) {

                        if (promise.returnval == "saved") {
                            swal('Record Saved Successfully');
                            $state.reload();
                        }
                        else if (promise.returnval == "savefailed") {
                            swal('Record Saving Failed');
                            $state.reload();
                        }
                        else if (promise.returnval == "updated") {
                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.returnval == "updatefailed") {
                            swal('Record Update Failed');
                            $state.reload();
                        }
                        else if (promise.returnval == "duplicate") {
                            swal('Record Already Exists');
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clearid = function () {
            $state.reload();

        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }
})();
