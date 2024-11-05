



(function () {
    'use strict';
    angular
.module('app')
.controller('MasterActivityController', MasterActivityController)

    MasterActivityController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function MasterActivityController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};


        $scope.sortKey = 'amA_Id';
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


        $scope.BindData = function () {
            apiService.getDATA("MasterActivity/Getdetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = paginationformasters;
           $scope.newuser = promise.masterActivityname;
           $scope.presentCountgrid = $scope.newuser.length;
       })
        };


        //$scope.order = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //to delete the data
        $scope.DeleteMasterActivitydata = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.amA_Id;
            var MdeleteId = $scope.deleteId;
            swal({
                title: "Are you sure?",
                text: "Do you want to delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.getURI("MasterActivity/MasterDeleteModulesDTO", MdeleteId).
                    then(function (promise) {
                        if (promise.message == "Delete") {
                            swal("You Can Not Delete This Record It Is Already Mapped With Student");
                        } else {
                            swal("Record Deleted Successfully");
                        }

                        $state.reload();
                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }

        // to Edit Data
        $scope.EditMasterActivitydata = function (EditRecord) {

            $scope.EditId = EditRecord.amA_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("MasterActivity/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.newuser.amA_Id = promise.masterActivityname[0].amA_Id;
                $scope.name = promise.masterActivityname[0].amA_Activity;
                $scope.description = promise.masterActivityname[0].amA_Activity_Desc;
            })
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveMasterActivitydata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "AMA_Activity": $scope.name,
                    "AMA_Activity_Desc": $scope.description,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterActivity/", data).then(function (promise) {

                    
                    if (promise.message != null && promise.message != "") {
                        swal(promise.message);
                        $state.reload();
                    }
                    else {
                        if (promise.returnval == true) {
                            if (promise.messageupdate == "Update") {
                                swal("Record Updated Successfully");
                            }
                            else {
                                swal("Record Saved Successfully");
                            }
                            $state.reload();
                        }
                        else {
                            if (promise.messageupdate == "Update") {
                                swal("Record Failed To Update");
                            }
                            else {
                                swal("Record Failed To Save");
                            }
                        }
                    }
                })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.amA_Activity)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amA_Activity_Desc)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }
})();