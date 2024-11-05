(function () {
    'use strict';

    angular
        .module('app')
        .controller('VBSC_Events_Category_StudentsController', VBSC_Events_Category_StudentsController);

    VBSC_Events_Category_StudentsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function VBSC_Events_Category_StudentsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.obj = {};

        $scope.loadgrid = function () {
            apiService.getURI("VBSC_Events_Category_Students/loaddata/", 1).then(function (promise) {
                $scope.geteventCategory = promise.geteventCategory;
            });
        }

        $scope.geteventCategory = [
            "Alfreds Futterkiste",
            "Berglunds snabbköp",
            "Centro comercial Moctezuma",
            "Ernst Handel",
        ]


        $scope.submitted = false;
        $scope.saveRecord = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "VBSCECT_Id": $scope.vbscecT_Id,
                    "VBSCME_Id": $scope.vbscmE_Id,
                    "VBSCMCC_Id": $scope.vbscmcC_Id
                   
                }
                
                apiService.create("VBSC_Events_Category_Students/savedata", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.vbscecT_Id == 0 || promise.vbscecT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.vbscecT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.vbscecT_Id == 0 || promise.vbscecT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.vbscecT_Id > 0) {
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
        
       
        $scope.deactive = function (item, SweetAlert) {
            $scope.VBSCECT_Id = item.vbscecT_Id;
            var dystring = "";
            if (item.vbscecT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.vbscecT_ActiveFlag == false) {
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
                        apiService.create("VBSC_Events_Category_Students/deactive", item).
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


        $scope.edit = function (sports) {
            $scope.vbscecT_Id = sports.vbscecT_Id;
            $scope.vbscmE_Id = sports.vbscmE_Id;
            $scope.vbscmcC_Id = sports.vbscmcC_Id;
            $scope.vbscmscC_Id = sports.vbscmscC_Id;
            $scope.vbscecT_GroupActivityFlg = sports.vbscecT_GroupActivityFlg;
            if ($scope.vbscecT_GroupActivityFlg == true) {
                $scope.obj.vbscecT_MaxNoOfGroup = sports.vbscecT_MaxNoOfGroup;
                $scope.obj.vbscecT_MaxNoOfStudents = sports.vbscecT_MaxNoOfStudents;
            }
            else {
                $scope.obj.vbscecT_MaxNoOfGroup = "";
                $scope.obj.vbscecT_MaxNoOfStudents = "";
            }
        };



        $scope.cancel = function () {
            $state.reload();
        }
        $scope.searchValue = '';
        
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
