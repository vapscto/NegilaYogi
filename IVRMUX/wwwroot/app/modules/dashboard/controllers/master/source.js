(function () {
    'use strict';
    angular
.module('app')
.controller('MasterSourceController', MasterSourceController)

    MasterSourceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterSourceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'pamS_Id';
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

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("MasterSource/getalldetails", pageid).
        then(function (promise) {
            $scope.pages = promise.pagesdata;
            $scope.presentCountgrid = $scope.pages.length;
            //$scope.totalItems = $scope.pages.length;
            //$scope.numPerPage = 5;
        })

            //$scope.sort = function (keyname) {
            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchsource = function () {
           // var entereddata = $scope.search;

            var data = {
                "PAMS_SourceName": $scope.search,
                "PAMS_SourceDesc":$scope.type
            }
            apiService.create("MasterSource/1", data).
        then(function (promise) {
            $scope.pages = promise.pagesdata;
            //swal("searched Successfully");
        })
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.pamS_SourceName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.pamS_SourceDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0                 
        }


        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.pamS_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.getURI("MasterSource/deletepages", pageid).
                    then(function (promise) {
                        if (promise.returnMsg == "Delete") {
                            swal('You Can Not Delete This Record. It Is Already Mapped With Student');
                        }
                        else {
                            if (promise.returnval === true) {
                                swal('Record Deleted Successfully');

                            }
                            else {
                                swal('Record Not Deleted Successfully');
                            }
                        }
                        $scope.pages = promise.pagesdata;

                       
                        $state.reload();
                    })
                }
                else {
                    swal("Cancelled");
                    $state.reload();
                }
            });
        }

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pamS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterSource/getdetails", pageid).
            then(function (promise) {

                $scope.PAMS_Id = promise.pagesdata[0].pamS_Id;
                $scope.PAMS_SourceName = promise.pagesdata[0].pamS_SourceName;
                $scope.PAMS_SourceDesc = promise.pagesdata[0].pamS_SourceDesc;
            })
        }

        $scope.clearfields = function () {

            $state.reload();

        }

        $scope.submitted = false;
        $scope.savepages = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "PAMS_SourceName": $scope.PAMS_SourceName,
                    "PAMS_SourceDesc": $scope.PAMS_SourceDesc,
                    "PAMS_Id": $scope.PAMS_Id
                }
                apiService.create("MasterSource/", data).
                then(function (promise) {
                    if (promise.returnMsg != "") {
                        if (promise.returnMsg == "duplicate") {
                            swal('Master Source Record Already Exist');
                            $state.reload();
                            return;

                        } else if (promise.returnMsg == "add") {
                                swal('Record Saved Successfully');
                                $state.reload();

                        } else if (promise.returnMsg == "update") {
                                    swal('Record Updated Successfully');
                                    $state.reload();
                                } 
                    } else {
                        swal('Failed To Save/Update Record');
                        $state.reload();
                    }
                })
            }

          
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }

})();