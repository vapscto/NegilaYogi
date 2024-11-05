
(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterdonationController', masterdonationController);
    masterdonationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function masterdonationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.searchValue = '';
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AlumniDonation/getdata_donation", pageid).
                then(function (promise) {
                    if (promise.alumnidonationlist != null || promise.alumnidonationlist > 0) {
                        $scope.donationlist = promise.alumnidonationlist;
                        $scope.presentCountgrid = $scope.donationlist.length;
                    }
                   
                })
        };
        //---------------------------------Save--------------------------------------------
        
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ALMDON_Id": $scope.ALMDON_Id,
                    "ALMDON_DonationName": $scope.ALMDON_DonationName,
                    "ALMDON_Amount": $scope.ALMDON_Amount,
                    "ALMDON_RegistrationFeeFlag": $scope.ALMDON_RegistrationFeeFlag
                   
                }
                apiService.create("AlumniDonation/save_donation", data).then(function (promise) {

                    if (promise.message == 'Add') {
                        swal('Record saved successfully');
                    }
                    else if (promise.message == 'Update') {
                      swal('Record updated successfully');
                        }
                   
                    else if (promise.message == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else if (promise.message == 'Error') {
                        swal('Operation Failed..!!');
                    }
                    else if (promise.message == 'exist') {
                        swal('Registration Fee Already Exist..!!');
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

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.deactive = function (item) {
           
            var dystring = "";
            if (item.almdoN_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.almdoN_ActiveFlag == false) {
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
                        apiService.create("AlumniDonation/deactive_donation", item).
                            then(function (promise) {
                                if (promise.message == 'true') {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.message == 'false'){
                                    swal("Record " + dystring + "d Successfully!!!");
                                }else if (promise.message == 'Error'){
                                    swal("Operation Failed..!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (item) {
            var data = {
                "ALMDON_Id": item.almdoN_Id
            }
            apiService.create("AlumniDonation/edit_donation", data).then(function (promise) {
                if (promise.edit_donation_list != null || promise.edit_donation_list > 0) {
                    $scope.ALMDON_Id = promise.edit_donation_list[0].almdoN_Id;
                    $scope.ALMDON_DonationName = promise.edit_donation_list[0].almdoN_DonationName;
                    $scope.ALMDON_Amount = promise.edit_donation_list[0].almdoN_Amount;
                }
                
            })
        }
       
       

       

    }
})();