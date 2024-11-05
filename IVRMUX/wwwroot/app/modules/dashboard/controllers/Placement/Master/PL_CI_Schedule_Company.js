(function () {
    'use strict';
    angular
        .module('app')
        .controller('PL_CI_Schedule_CompanyController', PL_CI_Schedule_CompanyController)
    PL_CI_Schedule_CompanyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function PL_CI_Schedule_CompanyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.obj = {};



        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("PL_CI_Schedule_Company/loaddata", pageid).
                then(function (promise) {
                    $scope.get_Company = promise.get_Company;
                    $scope.get_details = promise.get_details;
                    $scope.get_Schdule = promise.get_Schdule;
                    $scope.presentCountgrid = $scope.get_Schdule.length;
                })
        };

        $scope.submitted = false;

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {                                
                    "PLMCOMP_Id": $scope.plmcomP_Id,
                    //"PLCISCHCOMJT_Id": $scope.plcischcomjT_Id,
                    "PLCISCHCOM_JobDetails": $scope.plcischcoM_JobDetails,
                    "PLCISCHCOM_DriveFromDate": new Date($scope.obj.FromDate).toDateString(),
                    "PLCISCHCOM_DriveToDate": new Date($scope.obj.ToDate).toDateString(),
                    "PLCISCHCOM_Id": $scope.plcischcoM_Id,
                    "PLCISCH_Id": $scope.plciscH_Id,

                         
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("PL_CI_Schedule_Company/savedata", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.plcischcoM_Id == 0 || promise.plcischcoM_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.plcischcoM_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.plcischcoM_Id == 0 || promise.plcischcoM_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.plcischcoM_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
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
        //
        //Deactive
        $scope.deactive = function (item, SweetAlert) {

            var data = {

                "PLCISCHCOM_Id": item.plcischcoM_Id
            }
            var dystring = "";
            if (item.plcischcoM_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plcischcoM_ActiveFlag == false) {
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
                        apiService.create("PL_CI_Schedule_Company/deactive", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                              
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
  
        $scope.edit = function (item) {            var data = {                "PLCISCHCOM_Id": item.plcischcoM_Id            }            apiService.create("PL_CI_Schedule_Company/editdetails", data).then(function (promise) {                if (promise.editdata != null && promise.editdata.length > 0) {                    $scope.editdata = promise.editdata;                                       $scope.plciscH_Id = $scope.editdata[0].plciscH_Id;                   // $scope.obj.FromDate = $scope.editdata[0].plcischcoM_DriveFromDate;                   // $scope.obj.ToDate = $scope.editdata[0].plcischcoM_DriveToDate;                    $scope.plcischcoM_JobDetails = $scope.editdata[0].plcischcoM_JobDetails;                    $scope.plmcomP_Id = $scope.editdata[0].plmcomP_Id;                    $scope.obj.FromDate = new Date($scope.editdata[0].plcischcoM_DriveFromDate);                    $scope.obj.ToDate = new Date($scope.editdata[0].plcischcoM_DriveToDate);                }            })        }



        //$scope.editdata = function (user) {
        //    $scope.plmcomP_CompanyName = user.plmcomP_CompanyName;
        //    $scope.plciscH_JobDetails = user.plciscH_JobDetails;
        //    $scope.plciscH_Id = user.plciscH_Id;
        //    $scope.plcischcoM_DriveFromDate = user.plcischcoM_DriveFromDate;
        //    $scope.plcischcoM_DriveToDate = user.plcischcoM_DriveToDate;
        //    $scope.plcischcoM_JobDetails = user.plcischcoM_JobDetails;
        //    $scope.plcischcoM_Id = user.plcischcoM_Id;         
        //  //  $scope.plcischcomjT_Id = user.plcischcomjT_Id;
        //    $scope.plmcomP_Id = user.plmcomP_Id;
        //    //$scope.PLCISCH_Id = user.PLCISCH_Id;
        //};
        $scope.clear = function () {
            $state.reload();
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }

})();