
(function () {
    'use strict';
    angular.module('app').controller('MasterBatchController', MasterBatchController);

    MasterBatchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']

    function MasterBatchController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var id = 1;
        $scope.currentPage = 1;
        $scope.obj = {};
        $scope.obj.search = "";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;

        $scope.clgload = function () {
            apiService.getDATA("Masterbatch/").then(function (promise) {
                if (promise.returnval == true) {
                    $scope.getdetails = promise.batchlist
                }
                else if (promise.returnval == false) {
                    swal('No record found');
                }
            });
        };

        $scope.submitted = false;

        $scope.savebatch = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "ACMB_Id": $scope.ACMB_Id,
                    "ACMSN_SessionName": $scope.Session_Name,
                    "ACMNS_Order": $scope.Batch_Order
                }
                apiService.create("Masterbatch/savebatch", data).then(function (promise) {

                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exist");
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.edit = function (data) {
            apiService.create("Masterbatch/editbatch", data).then(function (promise) {
                if (promise.returnval === true) {
                    $scope.ACMB_Id = promise.batchlist[0].acmB_Id;
                    $scope.Session_Name = promise.batchlist[0].acmsN_SessionName;
                    $scope.Batch_Order = promise.batchlist[0].acmnS_Order;
                }
            });
        };

        $scope.activedeactivebranch = function (data, SweetAlert) {
            var mgs = "";
            if (data.acmsN_ActiveFlag == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Batch?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("Masterbatch/activedeactivebatch", data).then(function (promise) {

                            if (promise.message != "" && promise.message != null) {
                                swal(promise.message);
                            }
                            else {
                                swal("Failed to Activate/Deactivate Semester");
                            }


                            $state.reload();
                        })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.cleardata = function () {
            $state.reload();
        }
        
        $scope.filtervalue = function (obj) {
            return (angular.lowercase(obj.acmsN_SessionName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (JSON.stringify(obj.acmnS_Order)).indexOf($scope.obj.search) >= 0;
        };
    }
})();