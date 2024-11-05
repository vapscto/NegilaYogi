(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_Transaction_DetailsController', CMS_Transaction_DetailsController)
    CMS_Transaction_DetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_Transaction_DetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_Transaction/loaddatatwo", pageid).then(function (promise) {
                $scope.getreport = promise.pages;
                if ($scope.getreport != null && $scope.getreport.length > 0) {
                    $scope.presentCountgrid = $scope.getreport.length;
                }
                else if (promise.returnval == "admin") {
                    swal('Please Contact  Administrator  !');
                }
            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSTRANSDET_Id = 0;
                if ($scope.CMSTRANSDET_Id > 0) {
                    CMSTRANSDET_Id = $scope.CMSTRANSDET_Id;
                }
                var data = {
                    "CMSTRANSDET_Id": CMSTRANSDET_Id,
                    "CMSTRANS_Id": $scope.CMSTRANS_Id,
                    "CMSTRANSMEMTYINT_Id": $scope.CMSTRANSMEMTYINT_Id,
                    "CMSTRANSDET_Qty": $scope.CMSTRANSDET_Qty,
                    "CMSTRANSDET_Amount": $scope.CMSTRANSDET_Amount,
                    "CMSTRANSDET_Tax": $scope.CMSTRANSDET_Tax,
                    "CMSTRANSDET_NetAmount": $scope.CMSTRANSDET_NetAmount,
                }
                apiService.create("CMS_Transaction/savedatatwo", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                        $state.reload();
                    })
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "CMSTRANSDET_Id": item.cmstransdeT_Id
            }
            var dystring = "";
            if (item.cmstransdeT_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.cmstransdeT_ActiveFlg == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("CMS_Transaction/deactivetwo", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.edit = function (user) {
            var data = {
                "CMSTRANSDET_Id": user.cmstransdeT_Id
            }
            apiService.create("CMS_Transaction/edittwo", data).
                then(function (promise) {
                    if (promise.editarray != null && promise.editarray.length > 0) {
                        $scope.CMSTRANSDET_Id = promise.editarray[0].cmstransdeT_Id;
                        $scope.CMSTRANS_Id = promise.editarray[0].cmstranS_Id;
                        $scope.CMSTRANSMEMTYINT_Id = promise.editarray[0].cmstransmemtyinT_Id;
                        $scope.CMSTRANSDET_Qty = promise.editarray[0].cmstransdeT_Qty;
                        $scope.CMSTRANSDET_Amount = promise.editarray[0].cmstransdeT_Amount;
                        $scope.CMSTRANSDET_Tax = promise.editarray[0].cmstransdeT_Tax;
                        $scope.CMSTRANSDET_NetAmount = promise.editarray[0].cmstransdeT_NetAmount;
                    }
                })
            
        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();