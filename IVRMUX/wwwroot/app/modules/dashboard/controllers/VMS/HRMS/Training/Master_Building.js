(function () {
    'use strict';
    angular
        .module('app')
        .controller('BuildingController', buildingController)

    buildingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function buildingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'HRMB_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.getAllDetail = function () {
                               $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_B", pageid)
                .then(function (promise) {
                    $scope.building = promise.building_list;
                    $scope.presentCountgrid = $scope.building.length;
                    
                })
        }

        


       // edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmB_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_B", pageid).
                then(function (promise) {
                    $scope.Id = promise.building_list[0].hrmB_Id;
                    $scope.Building = promise.building_list[0].hrmB_BuildingName;
                    $scope.description = promise.building_list[0].hrmB_Desc;

                })
        };

       
        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMB_BuildingName": $scope.Building,
                    "HRMB_Desc": $scope.description,
                    "HRMB_Id": $scope.Id
                }
                apiService.create("Training_Master/SaveEdit_B", data).
                    then(function (promise) {
                        if (promise.returnvales !== "") {

                            if (promise.returnvales === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnvales === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                            }
                            else if (promise.returnvales === "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                                $state.reload();
                                return;
                            }
                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };
        $scope.Clearid = function () {
            $state.reload();
        }
        //deactive
        $scope.deactive = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.hrmB_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Building Name?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deactivate_B", bL).
                            then(function (promise) {
                               
                                if (promise.hrmB_ActiveFlag === true) {
                                    if (promise.returnval === false) {
                                        swal('Master Building  Deactivated Successfully');
                                        }
                                    }
                                else if (promise.hrmB_ActiveFlag === false) {
                                    swal('Master Building Activated Successfully');
                                    }
                             //   }

                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };

        
       
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



