(function () {
    'use strict';
    angular
        .module('app')
        .controller('Category_LevelsController', Category_LevelsController)
    Category_LevelsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function Category_LevelsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.obj.VBSCMCC_CCAgeFlag = true;
        $scope.obj.VBSCMCC_CCWeightFlag = true;
        $scope.obj.VBSCMCC_CCClassFlg = true;
        $scope.obj.stateall = false;
        $scope.searchValue = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("VBSC_MasterCompetition_Category/getdata", pageid).then(function (promise) {
                $scope.getcategory = promise.competetioncategory;
                $scope.Competelevelc = promise.competelevelc;                             
                $scope.Categorylevel = promise.getReport;                             
                if ($scope.Competelevelc != null && $scope.Competelevelc.length > 0) {
                    angular.forEach($scope.Competelevelc, function (stf) {
                        stf.selected = false;

                    });

                }
                
            })
        };
        $scope.isOptionsRequired = function () {
            return !$scope.Competelevelc.some(function (options) {
                return options.selected;
            });
        };
        $scope.all_checkdesg = function (stateall) {
            var toggleStatus = stateall;
            angular.forEach($scope.Competelevelc, function (itm) {
                itm.selected = toggleStatus;
            });
            
        };
       

        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VBSCMCCCLE_Id = 0;
                var VBSCMCL_Id = 0;
                $scope.Category_LevelTemp = [];
                if ($scope.Competelevelc != null && $scope.Competelevelc.length > 0) {
                    angular.forEach($scope.Competelevelc, function (stf) {
                        if (stf.selected == true) {
                            $scope.Category_LevelTemp.push({
                                VBSCMCL_Id: stf.vbscmcL_Id
                            });

                            VBSCMCL_Id = stf.vbscmcL_Id;

                        }

                    });

                }

                if ($scope.obj.VBSCMCCCLE_Id > 0) {
                    VBSCMCCCLE_Id = $scope.obj.VBSCMCCCLE_Id;
                }

                var data = {
                    "VBSCMCC_Id": $scope.obj.VBSCMCC_Id,
                    "VBSCMCCCLE_Id": VBSCMCCCLE_Id,
                    "Category_Level": $scope.Category_LevelTemp,                  
                    "VBSCMCL_Id": VBSCMCL_Id,
                }
                apiService.create("VBSC_MasterCompetition_Category/savedataVCl", data).
                    then(function (promise) {

                        if (promise.returnval == "save") {
                            swal("Record Saved Count : " + promise.savecount + " Duplicate Count : " + promise.duplicatecount + "");

                        }
                        else if (promise.returnval == "Notsave") {
                            swal("Record Not Saved !  Duplicate Count : " + promise.duplicatecount + "");

                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');

                        }
                        else if (promise.returnval == "dublicate") {
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

                "VBSCMCCCL_Id": item.VBSCMCCCL_Id
            }
            var dystring = "";
            if (item.VBSCMCCCLE_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.VBSCMCC_ActiveFlag == false) {
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
                        apiService.create("VBSC_MasterCompetition_Category/DeactivateVCl", data).
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
          
            $scope.obj.VBSCMCC_Id = user.VBSCMCC_Id;
            $scope.obj.VBSCMCCCLE_Id = user.VBSCMCCCLE_Id;
            if ($scope.Competelevelc != null && $scope.Competelevelc.length > 0) {
                angular.forEach($scope.Competelevelc, function (stf) {
                    if (stf.vbscmcL_Id == user.VBSCMCL_Id) {

                        stf.selected = true;
                    }

                });

            }

        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();