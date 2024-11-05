(function () {
    'use strict';
    angular
.module('app')
.controller('PDAMasterHeadController', PDAMasterHeadController)
    PDAMasterHeadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function PDAMasterHeadController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("PDAMasterHead/getalldetails", pageid).
        then(function (promise) {

            $scope.totcountfirst = promise.pdadata.length;
            $scope.result = promise.pdadata;
            $scope.students = promise.headdata;

        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }


        $scope.saveGroupdata = function () {
            if ($scope.myFormhead.$valid) {
                var data = {
                    "PDAMH_Id":$scope.PDAMH_Id,
                    "fmh_id": $scope.fmH_Id,
                    "PDAMH_HeadName": $scope.PDAMH_HeadName,
                }
                apiService.create("PDAMasterHead/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        $scope.loaddata();
                        if (promise.returnval === true) {
                            if (promise.message != null) {
                                swal('Record Updated Successfully', 'success');
                                $state.reload();
                            }
                            else {
                                swal('Record Saved Successfully', 'success');
                                $state.reload();
                            }
                        }
                       
                    }
                    else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                        $scope.loaddata();
                    }
                    else {
                        if (promise.message != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }
                        $scope.loaddata();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };



        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pdamH_Id;
            var data = {
                "PDAMH_Id": $scope.editEmployee,
            }
            apiService.create("PDAMasterHead/getdetails", data).
            then(function (promise) {

                $scope.PDAMH_HeadName = promise.pdadata[0].pdamH_HeadName;
               // $scope.fmH_Id = promise.headdata[0].fmH_FeeName;
                $scope.fmH_Id = promise.pdadata[0].fmH_ID;
                $scope.PDAMH_Id = employee.pdamH_Id;
              
            })
        }


        $scope.deactive = function (groupData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupData.pdamH_ActiveFlag == true) {
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

                apiService.create("PDAMasterHead/deactivate", groupData).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " Successfully");
                    }
                    else {
                        swal("Record is already been used !!!");
                    }
                    $scope.loaddata();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }





    }
 })();
