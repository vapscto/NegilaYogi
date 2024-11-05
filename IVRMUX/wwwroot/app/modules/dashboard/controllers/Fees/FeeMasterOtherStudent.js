(function () {
    'use strict';
    angular
.module('app')
        .controller('FeeMasterOtherStudentController', FeeMasterOtherStudentController)

    FeeMasterOtherStudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function FeeMasterOtherStudentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$stateParams) {

        $scope.searchValue = "";
        $scope.savedisable = true;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }

        $scope.onload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("FeeMasterOtherStudent/getdetails", pageid).
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
            $scope.delId = det.fmosT_Id;
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
                    apiService.DeleteURI("FeeMasterOtherStudent/delete/", $scope.delId).
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
            $scope.Id = det.fmosT_Id;
            apiService.getURI("FeeMasterOtherStudent/edit/", $scope.Id).
            then(function (promise) {
                $scope.FMOST_StudentName = promise.otherstudentList[0].fmosT_StudentName;
                $scope.FMOST_StudentMobileNo = promise.otherstudentList[0].fmosT_StudentMobileNo;
                $scope.FMOST_StudentEmailId = promise.otherstudentList[0].fmosT_StudentEmailId;
            })
        }
        $scope.submitted = false;
        $scope.save = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "FMOST_Id": $scope.Id,
                    "FMOST_StudentName": $scope.FMOST_StudentName,
                    "FMOST_StudentMobileNo": $scope.FMOST_StudentMobileNo,
                    "FMOST_StudentEmailId": $scope.FMOST_StudentEmailId
                }
                apiService.create("FeeMasterOtherStudent/", data)

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
