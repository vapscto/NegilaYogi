

(function () {
    'use strict';
    angular
.module('app')
.controller('MasterFeeFineSlabClgController', MasterFeeFineSlabClgController)

    MasterFeeFineSlabClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterFeeFineSlabClgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "fmfS_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.disabletypedrp = false;

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ((angular.lowercase(obj.fmfS_FineType)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (JSON.stringify(obj.fmfS_FromDay)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (JSON.stringify(obj.fmfS_ToDay)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.fmfS_ECSFlag)).indexOf(angular.lowercase($scope.searchValue)) >= 0);
        }


        $scope.onchangefinetype = function (value) {

            if (value == "GreaterThan") {
                $scope.todateshow = false;
                $scope.from = true;
            }
            else if (value == "LessThan") {
                $scope.todateshow = false;
                $scope.from = true;
            }
            else if (value == "Between") {
                $scope.todateshow = true;
                $scope.from = true;
            }
            else if (value == "") {
                $scope.todateshow = false;
                $scope.from = false;
            }
        }

        $scope.loaddata = function () {
            if ($scope.FMFS_FineType == "") {
                $scope.todateshow = false;
                $scope.from = false;
            }
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;

            apiService.getURI("MasterFeeFineSlabClg/getalldetails", pageid).
        then(function (promise) {
            $scope.students = promise.groupFineSlab;

            $scope.totcountfirst = $scope.students.length;
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }
        $scope.submitted = false;
        $scope.saveGroupdata = function () {
            
            if ($scope.myForm1.$valid) {

                if ($scope.FMFS_FromDay > 365 || $scope.FMFS_ToDay > 365) {
                    // swal("From/To days should be less than 365")
                    swal("From/To days should Not be Greater than 365")
                    $scope.Clearid();
                }
                else {
                    if ($scope.FMFS_FromDay < $scope.FMFS_ToDay || $scope.FMFS_FineType == "GreaterThan" || $scope.FMFS_FineType == "LessThan") {
                        if ($scope.FMFS_FineType === "select") {
                            swal('Please Select Finetype');
                        }
                        else {
                            var Refundflag = $scope.FMFS_ECSFlag
                            if (Refundflag == true) {
                                Refundflag = "E";
                            }
                            else {
                                Refundflag = "R";
                            }

                            if ($scope.FMFS_FineType == "Between") {
                                var data = {
                                    "FMFS_Id": $scope.fmfS_Id,
                                    "FMFS_FineType": $scope.FMFS_FineType,
                                    "FMFS_FromDay": $scope.FMFS_FromDay,
                                    "FMFS_ToDay": $scope.FMFS_ToDay,
                                    "FMFS_ECSFlag": Refundflag,
                                    "FMFS_ActiveFlag": true,
                                }
                            }
                            else if ($scope.FMFS_FineType == "GreaterThan") {
                                var data = {
                                    "FMFS_Id": $scope.fmfS_Id,
                                    // "FMFS_FineType": $scope.FMFS_FineType,
                                    "FMFS_FineType": 'Greater Than',
                                    "FMFS_FromDay": $scope.FMFS_FromDay,
                                    "FMFS_ToDay": 0,
                                    "FMFS_ECSFlag": Refundflag,
                                    "FMFS_ActiveFlag": true,
                                }
                            }
                            else if ($scope.FMFS_FineType == "LessThan") {
                                var data = {
                                    "FMFS_Id": $scope.fmfS_Id,
                                    "FMFS_FineType": 'Less Than',
                                    "FMFS_FromDay": $scope.FMFS_FromDay,
                                    "FMFS_ToDay": 0,
                                    "FMFS_ECSFlag": Refundflag,
                                    "FMFS_ActiveFlag": true,
                                }
                            }


                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            }
                            apiService.create("MasterFeeFineSlabClg/savedetail", data).
                            then(function (promise) {
                                $scope.students = promise.groupFineSlab;
                                if (promise.returnduplicatestatus === 'Duplicate' && promise.returnval === false) {
                                    swal("Record Already Exist !");
                                    $state.reload();
                                }
                                else if (promise.returnval === true) {
                                    if (promise.message != null) {
                                        swal('Record Updated Successfully', 'success');
                                    }
                                    else {
                                        swal('Record Saved Successfully', 'success');
                                    }

                                    $scope.loaddata();
                                    $scope.Clearid();
                                }
                                else {

                                    if (promise.message != null) {
                                        swal('Record Not Updated', 'success');
                                    }
                                    else {
                                        swal('Record Not Saved', 'success');
                                    }
                                }
                            })

                        }
                    }
                    else {
                        swal('To FromDay should be greater than from Today');
                    }
                }
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMFS_FineType": $scope.search,
                "FMFS_FineType": $scope.search,
                "FMFS_FromDay": $scope.search,
                "FMFS_ToDay": $scope.search,
                "FMFS_ECSFlag": $scope.search,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("MasterFeeFineSlabClg/1", data).
        then(function (promise) {
            $scope.students = promise.groupFineSlab;
            swal("searched Successfully");
        })
        }
        $scope.checkErr = function (FromDate, ToDate) {

            if (FromDate > new ToDate) {
                swal('To FromDay should be greater than from Today');
                return false;
            }
        };
        $scope.Clearid = function () {
            $scope.fmfS_Id = 0;
            $scope.MI_Id = "2";
            $scope.FMFS_FineType = "";
            $scope.FMFS_FromDay = "";
            $scope.FMFS_ToDay = "";
            $scope.searchValue = "";
            $scope.FMFS_ECSFlag = false;
            $scope.submitted = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();

        }
        //$scope.searchsource = function () {
        //    var entereddata = $scope.search;
        //    var data = {
        //        "FMFS_FineType": $scope.search,
        //        "FMFS_ToDay": $scope.type
        //    }
        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/json;'
        //        }
        //    }
        //    apiService.create("MasterFeeFineSlabClg/1", data).
        //then(function (promise) {
        //    $scope.students = promise.groupFineSlab;
        //    swal("searched Successfully");
        //})
        //}
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmfS_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterFeeFineSlabClg/deletepages", pageid).
                    then(function (promise) {
                        $scope.students = promise.groupFineSlab;
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fmfS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterFeeFineSlabClg/getdetails", pageid).
            then(function (promise) {

                $scope.disabletypedrp = true;

                if (promise.groupFineSlab[0].fmfS_FineType == "Between") {
                    $scope.FMFS_FineType = "Between";
                    $scope.from = true;
                    $scope.todateshow = true;
                }
                else if (promise.groupFineSlab[0].fmfS_FineType == "GreaterThan") {
                    $scope.FMFS_FineType = "GreaterThan";
                    $scope.from = true;
                    $scope.todateshow = false;
                }
                else {
                    $scope.FMFS_FineType = "LessThan";
                    $scope.from = true;
                    $scope.todateshow = false;
                }

                $scope.FMFS_FromDay = promise.groupFineSlab[0].fmfS_FromDay;
                $scope.FMFS_ToDay = promise.groupFineSlab[0].fmfS_ToDay;
                if (promise.groupFineSlab[0].fmfS_ECSFlag == "E") {
                    $scope.FMFS_ECSFlag = true;
                }
                else {
                    $scope.FMFS_ECSFlag = false;
                }

                $scope.fmfS_Id = promise.groupFineSlab[0].fmfS_Id;
            })
        }
        //for deactive avtive 
        $scope.deactive = function (groupFineSlab) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupFineSlab.fmfS_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("MasterFeeFineSlabClg/deactivate", groupFineSlab).
                then(function (promise) {
                     $scope.MI_Id = "4";
                    $scope.FMFS_FineType = "";
                    $scope.FMFS_FromDay = "";
                    $scope.FMFS_ToDay = "";
                    $scope.FMFS_ECSFlag = "";
                    $scope.students = promise.groupFineSlab;
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " " + "Successfully");
                    }
                    else if (promise.message == "used")
                    { swal("Record cannot Deactivate.It is already used"); }
                    else {
                        swal("Record Not Saved/Updated successfully");
                    }
                    $state.reload();
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        })
        }
    };
})();
