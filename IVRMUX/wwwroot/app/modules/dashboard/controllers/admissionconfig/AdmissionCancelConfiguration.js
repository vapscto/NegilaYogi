(function () {
    'use strict';

    angular.module('app').controller('AdmissionCancelConfigController', AdmissionCancelConfigController);

    AdmissionCancelConfigController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']

    function AdmissionCancelConfigController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var id = 1;
        $scope.report = false;
        $scope.acacC_Id = 0;

        var paginationformasters;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.submitted = false;

        $scope.CancelConfigLoad = function () {
            var pageid = 2;
            apiService.getURI("AdmissionStandard/CancelConfigLoad",pageid).then(function (promise) {
                $scope.getdetails = promise.getdetails;
                $scope.getdetailsnew = promise.getdetails;
                if ($scope.getdetails.length > 0) {
                    $scope.report = true;
                }
            });
        };

        $scope.optionchange = function () {
            angular.forEach($scope.getdetailsnew, function (change) {
                if (change.aacC_DOAFlg !== parseInt($scope.AACC_DOAFlg)) {
                    swal("You Can Not Change Cancellation Type");
                }
            });
        };

        $scope.onchangefromdays = function () {
            var fromdays = $scope.AACC_FromDays;
            if ($scope.AACC_FromDays >= $scope.AACC_ToDays) {
                $scope.AACC_ToDays = "";
            }
            angular.forEach($scope.getdetailsnew, function (from) {
                if (parseInt(from.aacC_ToDays) >= parseInt(fromdays)) {
                    swal("From Days Already Exists");
                    $scope.AACC_FromDays = "";
                } else {
                    $scope.messageerror = "";
                }
            });
        };

        $scope.onchangetodays = function () {
            var todays = $scope.AACC_ToDays;
            if (parseInt(todays) <= parseInt($scope.AACC_FromDays)) {
                $scope.AACC_ToDays = "";
                swal("To Days Should Be Greather Than From Days");
            }
        };

        $scope.SaveCancelConfigData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var flag = false;
                if ($scope.AACC_DOAFlg === "0") {
                    flag = false;
                } else {
                    flag = true;
                }

                var data = {
                    "AACC_DOAFlg": flag,
                    "AACC_FromDays": parseInt($scope.AACC_FromDays),
                    "AACC_ToDays": parseInt($scope.AACC_ToDays),                    
                    "AACC_CancellationPer": parseFloat($scope.AACC_CancellationPer),
                    "AACC_Id": $scope.AACC_Id === undefined || $scope.AACC_Id === null || $scope.AACC_Id === "" ? 0 : $scope.AACC_Id
                };
                apiService.create("AdmissionStandard/SaveCancelConfigData", data).then(function (promise) {

                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    } else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Failed To Update Record");
                        }
                    } else if (promise.message === "FromDays") {
                        swal("From Days Already Exists");
                    } else if (promise.message === "ToDays") {
                        swal("To Days Already Exists");
                    } else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditCancelConfig = function (data) {
            apiService.create("AdmissionStandard/EditCancelConfig", data).then(function (promise) {
                $scope.AACC_DOAFlg = promise.editdetails[0].aacC_DOAFlg;
                $scope.AACC_Id = promise.editdetails[0].aacC_Id;
                $scope.AACC_FromDays = promise.editdetails[0].aacC_FromDays;
                $scope.AACC_ToDays = promise.editdetails[0].aacC_ToDays;                 
                $scope.AACC_CancellationPer = promise.editdetails[0].aacC_CancellationPer;
            });
        };


        $scope.ActiveDeactiveCancelConfig = function (data, SweetAlert) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AdmissionStandard/ActiveDeactiveCancelConfig", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record Deleted Successfully");
                                $state.reload();
                            } else {
                                swal("Failed To Delete Record");
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cleardata = function () {
            $state.reload();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.search = "";

    }

})();
