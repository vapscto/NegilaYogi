
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VBSC_EventsController', VBSC_EventsController);
    VBSC_EventsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$filter']
    function VBSC_EventsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {



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
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; todate
        };


        

        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("VBSC_Events/getloaddata", pageid).
                then(function (promise) {


                    $scope.get_Competitionlevel = promise.get_Competitionlevel;
                    $scope.academicYear = promise.academicYear;
                    $scope.get_eventlist = promise.get_eventlist;
                    $scope.get_VBSCeventlist = promise.get_VBSCeventlist;
                   


                    //$scope.presentCountgrid = $scope.get_customer.length;
                })
        };
        //---------------------------------Save--------------------------------------------
       
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var vbscE_StartTime = $scope.vbscE_StartTime;
                var vbscE_EndTime = $scope.vbscE_EndTime;
                $scope.sresult = $filter('date')(vbscE_StartTime, 'HH:mm');
                $scope.eresult = $filter('date')(vbscE_EndTime, 'HH:mm');

                var data = {
                    "VBSCMCL_Id": $scope.vbscmcL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "VBSCME_Id": $scope.vbscmE_Id,
                    "VBSCE_VenueName": $scope.vbscE_VenueName,
                    "VBSCE_StartDate": new Date($scope.vbscE_StartDate).toDateString(),
                    "VBSCE_EndDate": new Date($scope.vbscE_EndDate).toDateString(),
                    "VBSCE_StartTime": $scope.sresult ,
                    "VBSCE_EndTime": $scope.eresult,
                    "VBSCE_Remarks": $scope.vbscE_Remarks,
                    "VBSCE_Id": $scope.vbscE_Id,

                }

                apiService.create("VBSC_Events/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.vbscE_Id == 0 || promise.vbscE_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.vbscE_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.vbscE_Id == 0 || promise.vbscE_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.vbscE_Id > 0) {
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

        $scope.deactive = function (item, SweetAlert) {
            $scope.vbscE_Id = item.vbscE_Id;
            var dystring = "";
            if (item.vbscE_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.vbscE_ActiveFlag == false) {
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
                        apiService.create("VBSC_Events/deactive", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.EditDetails = function (user) {
            $scope.asmaY_Id = user.asmaY_Id;
            $scope.vbscmcL_Id = user.vbscmcL_Id;
            $scope.vbscmE_Id = user.vbscmE_Id;
            $scope.vbscE_VenueName = user.vbscE_VenueName;
            $scope.vbscE_Remarks = user.vbscE_Remarks;
            $scope.vbscE_StartDate = new Date(user.vbscE_StartDate);
            $scope.vbscE_EndDate = new Date(user.vbscE_EndDate);
            $scope.vbscE_StartTime = moment(user.vbscE_StartTime, 'HH:mm a').format();
            $scope.vbscE_EndTime = moment(user.vbscE_EndTime, 'HH:mm a').format();
            //$scope.vbscE_StartTime = user.vbscE_StartTime;
            //$scope.vbscE_EndTime = user.vbscE_EndTime;
            $scope.vbscE_Id = user.vbscE_Id;
        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';


    }
})();