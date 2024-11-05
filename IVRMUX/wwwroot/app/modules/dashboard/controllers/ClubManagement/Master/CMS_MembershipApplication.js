(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_MembershipApplicationController', CMS_MembershipApplicationController)
    CMS_MembershipApplicationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_MembershipApplicationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
            apiService.getURI("CMS_MembershipApplication/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getarray;
            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSMAPPL_Id = 0;
                if ($scope.CMSMAPPL_Id > 0) {
                    CMSMAPPL_Id = $scope.CMSMAPPL_Id;
                }
                var data = {
                    "CMSMAPPL_ApplicantsName": $scope.CMSMAPPL_ApplicantsName,
                    "CMSMAPPL_Address": $scope.CMSMAPPL_Address,
                    "CMSMAPPL_PhoneNo": $scope.CMSMAPPL_PhoneNo,
                    "CMSMAPPL_EMailId": $scope.CMSMAPPL_EMailId,
                    "CMSMAPPL_ApplicationDate": new Date($scope.CMSMAPPL_ApplicationDate).toDateString(),
                    "CMSMAPPL_ApplicationNo": $scope.CMSMAPPL_ApplicationNo,
                    "CMSMAPPL_Id": CMSMAPPL_Id
                }
                apiService.create("CMS_MembershipApplication/savedata", data).
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
        //
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {

            var data = {

                "CMSMAPPL_Id": item.cmsmappL_Id
            }
            var dystring = "";
            if (item.cmsmappL_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsmappL_ActiveFlag == false) {
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
                        apiService.create("CMS_MembershipApplication/deactive", data).
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
            debugger;
            $scope.CMSMAPPL_Id = user.cmsmappL_Id;
            $scope.CMSMAPPL_ApplicantsName = user.cmsmappL_ApplicantsName;
            $scope.CMSMAPPL_Address = user.cmsmappL_Address;
            $scope.CMSMAPPL_PhoneNo = user.cmsmappL_PhoneNo;
            $scope.CMSMAPPL_EMailId = user.cmsmappL_EMailId;
            $scope.CMSMAPPL_ApplicationDate = new Date(user.cmsmappL_ApplicationDate);
            $scope.CMSMAPPL_ApplicationNo = user.cmsmappL_ApplicationNo;
        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();