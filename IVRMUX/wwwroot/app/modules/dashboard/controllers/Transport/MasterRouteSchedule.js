
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterRouteScheduleController', MasterRouteScheduleController)

    MasterRouteScheduleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function MasterRouteScheduleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "trrsC_Id";   //set the sortKey to the param passed
        $scope.sortReverse = true; //if true make it false and vice versa



        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.listshow = false;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterRouteSchedule/getdata", pageid).
        then(function (promise) {
            $scope.getzonearea = promise.getroute;
            if (promise.getdata.length > 0) {
                $scope.listshow = true;
                $scope.locationdetails = promise.getdata;
                $scope.presentCountgrid = $scope.locationdetails.length;
            }
            else {
                swal("No Records Found");
                $scope.listshow = false;
            }
        })
        }

        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //---Save--//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "TRRSC_Id": $scope.TRRSC_Id,
                    //"TRMR_Id": $scope.trmR_Id,
                    "TRRSC_ScheduleName": $scope.TRRSC_ScheduleName,
                    //"TRRSC_Date": new Date($scope.TRRSC_Date).toDateString(),
                }
                apiService.create("MasterRouteSchedule/savedata", data).then(function (promise) {
                    debugger;
                    if (promise.message == "Save") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfull");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfull");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        //--edit--//
        $scope.edit = function (user) {
            var data = {
                "TRRSC_Id": user.trrsC_Id
            }
            apiService.create("MasterRouteSchedule/edit", data).then(function (promise) {
                if (promise != null) {
                    if (promise.geteditdata.length > 0) {
                        $scope.TRRSC_ScheduleName = promise.geteditdata[0].trrsC_ScheduleName;
                      //  $scope.trmR_Id = promise.geteditdata[0].trmR_Id;
                        $scope.arealist = true;
                        $scope.TRRSC_Id = promise.geteditdata[0].trrsC_Id;
                       // $scope.TRRSC_Date = new Date(promise.geteditdata[0].trrsC_Date);
                    }
                }
            })
        }

        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trrsC_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("MasterRouteSchedule/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';
        

        $scope.cancel = function () {
            $state.reload();
        }
    };
})();


