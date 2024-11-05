(function () {
    'use strict';

    angular
        .module('app')
        .controller('VBSC_Events_CategoryController', VBSC_Events_CategoryController);

    VBSC_Events_CategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function VBSC_Events_CategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.obj = {};
        $scope.obj.vbscecT_GroupActivityFlg = true;
        $scope.obj.vbscecT_MaxNoOfGroup = true;
        $scope.obj.VBSCMCC_CCToWeight = true;

        $scope.loadgrid = function () {
            apiService.getURI("VBSC_Events_Category/loaddata/", 1).then(function (promise) {
                $scope.getcompitionCategory = promise.getcompitionCategory;
                $scope.getSportsCCName = promise.getSportsCCName;
                $scope.getMasterEvents = promise.getMasterEvents;
                $scope.geteventcategory = promise.geteventcategory;
                $scope.presentCountgrid = $scope.geteventcategory.length;
              
            });
        }
        $scope.getclick = function () {
            $scope.obj.vbscecT_MaxNoOfGroup = "";
            $scope.obj.vbscecT_MaxNoOfStudents = "";
        }
        $scope.submitted = false;
        $scope.saveRecord = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VBSCECT_MaxNoOfGroup = 0;
                var VBSCECT_MaxNoOfStudents = 0;
                if ($scope.vbscecT_GroupActivityFlg == true) {
                    VBSCECT_MaxNoOfGroup = $scope.obj.vbscecT_MaxNoOfGroup;
                    VBSCECT_MaxNoOfStudents = $scope.obj.vbscecT_MaxNoOfStudents;
                }
                var data = {
                    "VBSCECT_Id": $scope.vbscecT_Id,
                    "VBSCME_Id": $scope.vbscmE_Id,
                    "VBSCMCC_Id": $scope.vbscmcC_Id,
                    "VBSCMSCC_Id": $scope.vbscmscC_Id,
                    "VBSCECT_MaxNoOfGroup": VBSCECT_MaxNoOfGroup,
                    "VBSCECT_MaxNoOfStudents": VBSCECT_MaxNoOfStudents,
                    "VBSCECT_GroupActivityFlg": $scope.vbscecT_GroupActivityFlg,
                    "VBSCECT_Remarks": $scope.vbscecT_Remarks
                }
                
                apiService.create("VBSC_Events_Category/savedata", data).then(function (promise) {

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
                        apiService.create("VBSC_Events_Category/deactive", item).
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
