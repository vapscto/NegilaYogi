(function () {
    'use strict';
    angular
.module('app')
        .controller('masterboardController', masterboardController)

    masterboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function masterboardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'ivrmmB_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.getAllDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("MasterBoardandSchoolType/getalldetails", pageid).
            then(function (promise) {
                if (promise.boardList != null && promise.boardList != "") {
                    $scope.students = promise.boardList;
                    $scope.presentCountgrid = $scope.students.length;
                    $scope.count = 1;
                }
                else {
                    swal("No Records Found");
                    $scope.count = 0;
                }

            })
            //$scope.sort = function (keyname) {
            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.delete = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmmB_Id;
            var orgaid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterBoardandSchoolType/deletedetails", orgaid).
                    then(function (promise) {
                        $scope.students = promise.boardList;
                        $scope.presentCountgrid = $scope.students.length;
                        if (promise.returnval == "mapped") {
                            swal("Sorry.....You can not delete this record.Because it is mapped to student");
                            return;
                        }
                        else if (promise.returnval === "true") {
                            swal('Record Deleted Successfully');
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
        $scope.edit = function (employee) {
            $scope.editEmployee = employee.ivrmmB_Id;
            var templId = $scope.editEmployee;

            apiService.getURI("MasterBoardandSchoolType/getdetails", templId).
            then(function (promise) {
                $scope.IVRMMB_Id = promise.boardList[0].ivrmmB_Id;
                $scope.IVRMMB_Name = promise.boardList[0].ivrmmB_Name;
                $scope.IVRMMB_Description = promise.boardList[0].ivrmmB_Description;
            })
        }
        $scope.submitted = false;
        $scope.saveboarddata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMMB_Name": $scope.IVRMMB_Name,
                    "IVRMMB_Description": $scope.IVRMMB_Description,
                    "IVRMMB_Id": $scope.IVRMMB_Id
                }
                apiService.create("MasterBoardandSchoolType/", data)

                .then(function (promise) {

                    if (promise.returnval == "Add") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "false") {
                        swal('Record Not Saved/Updated');
                    }
                    else if (promise.returnval == "Duplicate") {
                        swal('Master Board Already Exists');
                        $state.reload();
                        return;
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
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.ivrmmB_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ivrmmB_Description)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }
})();