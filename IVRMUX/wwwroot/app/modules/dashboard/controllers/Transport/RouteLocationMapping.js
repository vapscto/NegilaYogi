
(function () {
    'use strict';
    angular
.module('app')
.controller('RouteLocationMappingController', RouteLocationMappingController)

    RouteLocationMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function RouteLocationMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
       // paginationformasters = 10;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
        $scope.sortKey = 'trmrL_Id';
        $scope.sortReverse = true;
        $scope.masterlist = false;

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.getdetails1) {
                    $scope.getdetails1[index].trmrL_Order = Number(index) + 1;

                }


            }
        };

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("RouteLocationMapping/getdata/", pageid).then(function (promise) {
                if (promise != null) {
                    if (promise.getdetails.length > 0) {
                        $scope.routelocation = promise.getdetails;
                        $scope.presentCountgrid = $scope.routelocation.length;
                        $scope.masterlist = true;
                        $scope.getdetails1 = promise.getdetails1;
                    }
                    else {
                        swal("No Records Found");
                    }
                    // $scope.locationdetails = promise.locationdetails;
                    $scope.routedetailsarea = promise.routedetailsarea;
                }
                else {
                    swal("No Records Found");
                }
            })
        }
        $scope.searchValue = '';
        $scope.save = function () {
            if ($scope.myForm.$valid) {
                var ClassIDs = [];

                //if ($scope.checkboxchcked.length > 0) {
                //    ClassIDs = $scope.checkboxchcked;
                //}

                angular.forEach($scope.locationdetails,function(hi)
                {
                    if(hi.Selected)
                    {
                        ClassIDs.push({ TRML_Id: hi.trmL_Id, locationname: hi.trmL_LocationName });
                    }
                })
                //else if ($scope.edit == true && $scope.editClassList.length > 0) {
                //    ClassIDs = $scope.editClassList;
                //}
                var data = {
                    "selectedlocations": ClassIDs,
                    "TRMR_Id": $scope.TRMR_Id,
                    "TRMRL_Id": $scope.trmrL_Id
                }
                apiService.create("RouteLocationMapping/savedata/", data).then(function (Promise) {
                    if (Promise != null) {
                        if (Promise.returnval == true) {
                            if (Promise.message == "Save") {
                                if (Promise.msg == "Duplicate") {
                                    swal("Record Saved Successfully And Some Records Already Exists");
                                }
                                else {
                                    swal("Record Saved Successfully");
                                }
                            }
                            else if (Promise.message == "Update") {
                                if (Promise.msg == "Duplicate") {
                                    swal("Record updated Successfully And Some Records Already Exists");
                                }
                                else {
                                    swal("Record updated Successfully");
                                }
                            }
                        }
                        else if (Promise.returnval == false) {
                            if (Promise.message == "Save") {
                                swal("Failed To Save Record");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                    }
                    else {
                        swal("Record Not Saved / Updated Successfully ");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.checkboxchcked = [];
        $scope.editClassList = [];

        $scope.CheckedClassName = function (data) {
            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                $scope.checkboxchcked.splice($scope.checkboxchcked.indexOf(data), 1);
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.locationdetails.some(function (options) {
                return options.Selected;
            });
        }

        $scope.clear = function () {
            $state.reload();
        }


        $scope.getlocations = function () {
            var data = {
                "TRMR_Id": $scope.TRMR_Id
            }
            apiService.create("RouteLocationMapping/getlocations/", data).then(function (promise) {
                if (promise != null) {
                    if (promise.locationdetails.length > 0) {
                        $scope.locationdetails = promise.locationdetails;
                    }
                    else {
                        swal("All Locations Is Mapped For This Route");
                    }
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        $scope.getlocationsarea = function () {
            var data = {
                "TRMA_Id":$scope.TRMA_Id
            }
            apiService.create("RouteLocationMapping/getlocationsarea/", data).then(function (promise) {
                if (promise != null) {
                    if (promise.routedetails.length > 0) {
                        $scope.routedetails = promise.routedetails;
                    }
                    else {
                        swal("No Reocrds Found");
                    }

                }
                else {
                    swal("No Reocrds Found");
                }
            })
            
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';
        //$scope.filterValue = function (obj) {
            
        //    return (JSON.stringify(obj.routename)).indexOf($scope.searchValue) >= 0 ||
        //        (JSON.stringify(obj.locationname)).indexOf($scope.searchValue) >= 0
        //}


        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmrL_ActiveFlag === true) {
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
                apiService.create("RouteLocationMapping/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully.");
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


        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };

        $scope.getOrder = function (getdetails1) {
            var data = {
                "selectedorder": getdetails1
            }
            apiService.create("RouteLocationMapping/getOrder/", data).then(function (promise) {
                if (promise.returnval == true) {
                    swal("Record Updated Successfully");
                }
                else {
                    swal("Failed To Update Record");
                }
            })
        }

    };
})();


