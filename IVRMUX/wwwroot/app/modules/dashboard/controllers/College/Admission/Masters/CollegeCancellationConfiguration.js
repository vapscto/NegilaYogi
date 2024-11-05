(function () {
    'use strict';

    angular
        .module('app')
        .controller('CollegeCancellationConfigurationController', CollegeCancellationConfigurationController);

    CollegeCancellationConfigurationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']

    function CollegeCancellationConfigurationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var id = 1;
        $scope.report = false;
        $scope.acacC_Id = 0;

        var paginationformasters;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
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


        $scope.clgload = function () {

            apiService.getDATA("CollegeCancellationConfiguration/").
                then(function (promise) {
                    $scope.getdetails = promise.getdetails;
                    $scope.getdetailsnew = promise.getdetails;

                    if ($scope.getdetails.length > 0) {
                        $scope.report = true;
                    }

                });
        };

        $scope.optionchange = function () {


            angular.forEach($scope.getdetailsnew, function (change) {

                if (change.acacC_DOAFlg !== parseInt($scope.ACACC_DOAFlg)) {
                    swal("You Can Not Change Cancellation Type");
                }
            });
        };

        $scope.onchangefromdays = function () {

            var fromdays = $scope.ACACC_FromDays;

            if ($scope.ACACC_FromDays >= $scope.ACACC_ToDays) {
                $scope.ACACC_ToDays = "";
            }

            angular.forEach($scope.getdetailsnew, function (from) {

                if (parseInt(from.acacC_ToDays) >= parseInt(fromdays)) {
                    swal("From Days Already Exists");
                    $scope.ACACC_FromDays = "";
                } else {
                    $scope.messageerror = "";
                }
            });
        };
        $scope.onchangetodays = function () {
            var todays = $scope.ACACC_ToDays;
            if (parseInt(todays) <= parseInt($scope.ACACC_FromDays)) {
                $scope.ACACC_ToDays = "";
                swal("To Days Should Be Greather Than From Days");
            }
        };

        $scope.onchangerefund = function () {
            $scope.ACACC_CancellationPer = "";
            var refundamt = $scope.ACACC_RefundAmountPer;

            if (parseInt(refundamt) <= 100) {
                $scope.ACACC_CancellationPer = 100 - parseFloat(refundamt);
            } else {
                $scope.ACACC_RefundAmountPer = "";
                swal("Refund Amount Percentage Should Be Less Than 100");
            }


        };

        $scope.submitted = false;

        $scope.savesem = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ACACC_DOAFlg": $scope.ACACC_DOAFlg,
                    "ACACC_FromDays": $scope.ACACC_FromDays,
                    "ACACC_ToDays": $scope.ACACC_ToDays,
                    "ACACC_RefundAmountPer": $scope.ACACC_RefundAmountPer,
                    "ACACC_CancellationPer": $scope.ACACC_CancellationPer,
                    "ACACC_Id": $scope.acacC_Id
                };
                apiService.create("CollegeCancellationConfiguration/saveconfig", data).then(function (promise) {

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



        $scope.edit = function (data) {

            apiService.create("CollegeCancellationConfiguration/editconfig", data).then(function (promise) {
                $scope.ACACC_DOAFlg = promise.editdetails[0].acacC_DOAFlg;
                $scope.acacC_Id = promise.editdetails[0].acacC_Id;
                $scope.ACACC_FromDays = promise.editdetails[0].acacC_FromDays;
                $scope.ACACC_ToDays = promise.editdetails[0].acacC_ToDays;
                $scope.ACACC_RefundAmountPer = promise.editdetails[0].acacC_RefundAmountPer;
                $scope.ACACC_CancellationPer = promise.editdetails[0].acacC_CancellationPer;
            });
        };

        $scope.activedeactiveconfig = function (data, SweetAlert) {

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
                        apiService.create("CollegeCancellationConfiguration/activedeactive", data).then(function (promise) {
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
