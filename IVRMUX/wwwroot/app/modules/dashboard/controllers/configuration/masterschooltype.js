
(function () {
    'use strict';
    angular
.module('app')
        .controller('masterschooltypeController', masterschooltypeController)

    masterschooltypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', 'superCache']
    function masterschooltypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterMenuPageMapping/';
        };

        //$scope.Next = function () {
        //    $window.location.href = 'http://' + HostName + '/#/app/masterschooltype';
        //};

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.getAllDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            apiService.getURI("MasterBoardandSchoolType/getallSchoolTypedetails", pageid).
            then(function (promise) {
                
                $scope.sortKey = "ivrmmtyP_Id";
                $scope.reverse = true;
                $scope.students = promise.schoolTypeList;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };



        $scope.delete = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmmtyP_Id;
            var orgaid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterBoardandSchoolType/deleteSchoolTypedetails", orgaid).
                    then(function (promise) {
                        $scope.students = promise.schoolTypeList;
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully!', 'success');
                        }
                        else {
                            swal('Record Not Deleted', 'Failed');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.edit = function (employee) {
            $scope.editEmployee = employee.ivrmmtyP_Id;
            var templId = $scope.editEmployee;

            apiService.getURI("MasterBoardandSchoolType/getSchoolTypedetails", templId).
            then(function (promise) {
                $scope.IVRMMTYP_Type = promise.schoolTypeList[0].ivrmmtyP_Type;
                $scope.IVRMMTYP_Description = promise.schoolTypeList[0].ivrmmtyP_Description;
                $scope.IVRMMTYP_Id = promise.schoolTypeList[0].ivrmmtyP_Id;
            })
        }
        $scope.submitted = false;
        $scope.saveschoolTypedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMMTYP_Type": $scope.IVRMMTYP_Type,
                    "IVRMMTYP_Description": $scope.IVRMMTYP_Description,
                    "IVRMMTYP_Id": $scope.IVRMMTYP_Id
                }
                apiService.create("MasterBoardandSchoolType/saveSchoolTypeData", data)

                .then(function (promise) {
                    
                    if (promise.message != null && promise.message != "" && promise.message != "u" && promise.message != "a") {
                        swal("Master School Type Already Exist");
                        return;
                    }
                    if (promise.returnval == true) {


                        if (promise.message == "a") {
                            swal('Record Saved Successfully', 'Success');
                            $state.reload();
                        }
                        else {
                            swal('Record Updated Successfully', 'Success');
                            $state.reload();

                        }
                        $scope.students = promise.schoolTypeList;

                        //  swal('Record Saved/Updated Successfully', 'success');


                        $scope.clearid();
                        $state.reload();

                    }
                    else {

                        if (promise.message == "a") {
                            swal('Record Not Saved', 'Failed');
                            return;
                        }
                        else {
                            swal('Record Not Updated', 'Failed');
                            return;
                        }
                        //swal('Operation Failed', 'Failed');
                    }
                })
            }
        };
        //$scope.deactive = function (category) {

        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/json;'
        //        }
        //    }
        //    apiService.create("MasterCategory/deactivate", category).
        //    then(function (promise) {
        //        $scope.IVRMMB_Name = "";
        //        $scope.IVRMMB_Description = "";
        //        $scope.students = promise.boardList;
        //        swal('Record Activated/Deactivated Successfully', 'success');
        //    })
        //}
        $scope.clearid = function () {
            ////$scope.submitted = false;
            ////$scope.IVRMMTYP_Type = "";
            ////$scope.IVRMMTYP_Description = "";
            //$scope.IVRMMTYP_Type = "";
            //$scope.IVRMMTYP_Description = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }
})();