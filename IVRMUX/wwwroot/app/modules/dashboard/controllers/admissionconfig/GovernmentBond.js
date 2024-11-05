
(function () {
    'use strict';
    angular
.module('app')
.controller('GovernmentBondController', GovernmentBondController)

    GovernmentBondController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function GovernmentBondController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'imgB_Id';
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




        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getDATA("GovernmentBond/Getdetails").
       then(function (promise) {
           if (promise.count > 0) {
               $scope.newuser = promise.governmentBondname;
               $scope.presentCountgrid = $scope.newuser.length;
           }
           else {
               swal("No Records Found");
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

        $scope.DeleteGovernmentBonddata = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.imgB_Id;
            var MdeleteId = $scope.deleteId;
            swal({
                title: "Are you sure?",
                text: "Do you want to delete this record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("GovernmentBond/MasterDeleteModulesDTO/", MdeleteId).
                    then(function (promise) {
                        
                        if (promise.returnMsg != "" && promise.returnMsg != null) {
                            if (promise.returnMsg == "deleted") {
                                swal("Record Deleted Successfully");
                                $state.reload();
                            }
                            else if (promise.returnMsg == "deletefailed") {
                                swal("Failed To Delete Record");
                                $state.reload();
                            }
                            else {
                                swal(promise.returnMsg);
                                $state.reload();
                            }
                        }

                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }

        // to Edit Data
        $scope.EditGovernmentBonddata = function (EditRecord) {

            $scope.EditId = EditRecord.imgB_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("GovernmentBond/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.newuser.imgB_Id = promise.governmentBondname[0].imgB_Id;
                $scope.name = promise.governmentBondname[0].imgB_Name;
                $scope.description = promise.governmentBondname[0].imgB_Description;
            })
        };



        $scope.submitted = false;
        // TO Save The Data
        $scope.saveGovernmentBonddata = function () {
            if ($scope.myform.$valid) {
                var data = {
                    "IMGB_Id": $scope.EditId,
                    "IMGB_Name": $scope.name,
                    "IMGB_Description": $scope.description,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("GovernmentBond/", data).
                then(function (promise) {
                    if (promise.returnMsg != null && promise.returnMsg != "") {
                        if (promise.returnMsg == "add") {
                            swal("Record Saved Successfully");
                            $state.reload();

                        } else if (promise.returnMsg == "update") {
                            swal("Record Updated Successfully");
                            $state.reload();

                        } else
                            if (promise.returnMsg == "duplicate") {
                                swal("Record Already Exists");
                                $state.reload();
                                return;

                            }
                            else if (promise.returnMsg == "updatefailed") {
                                swal("Failed to Update Record");
                                $state.reload();
                                return;
                            }
                            else if (promise.returnMsg == "addfailed") {
                                swal("Failed to Save Record");
                                $state.reload();
                                return;
                            }
                    }
                    else {
                        swal("Failed to Save/Update Record");
                        $state.reload();
                        return;
                    }
                })


            } else {
                $scope.submitted = true;
            }


        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }


        $scope.searchValue = "";
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.imgB_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.imgB_Description)).indexOf(angular.lowercase($scope.searchValue)) >= 0

        }

    }

})();