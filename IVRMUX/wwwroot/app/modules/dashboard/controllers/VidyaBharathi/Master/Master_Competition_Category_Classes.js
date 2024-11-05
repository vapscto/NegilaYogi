(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterCompetition_CategoryController', MasterCompetition_CategoryController)
    MasterCompetition_CategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function MasterCompetition_CategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
            apiService.getURI("VBSC_MasterCompetition_Category/loaddata", pageid).then(function (promise) {
                $scope.Master_trust = promise.master_trust;
                $scope.classArray = promise.classArray;
                $scope.ClasslistArray = promise.classlistArray;
                $scope.obj.MT_Id = promise.mT_Id;
                if ($scope.classArray != null && $scope.classArray.length > 0) {
                    angular.forEach($scope.classArray, function (stf) {
                        stf.selected = false;

                    });

                }
                $scope.Organsation(promise.mT_Id);
            })
        };
        $scope.isOptionsRequired = function () {
            return !$scope.classArray.some(function (options) {
                return options.selected;
            });
        };
        $scope.all_checkdesg = function (stateall) {
            var toggleStatus = stateall;
            angular.forEach($scope.classArray, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.togchkbxdesg();
        };
        $scope.Organsation = function (MT_Id) {
            $scope.getcategory = [];
            $scope.obj.VBSCMCC_Id = "";
            var data = {
                "MT_Id": MT_Id
            }
            apiService.create("VBSC_MasterCompetition_Category/Organsation", data).
                then(function (promise) {
                    if (promise.getReport != null && promise.getReport.length > 0) {
                        $scope.getcategory = promise.getReport;
                    }

                })
        }

        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VBSCMCCCL_Id = 0;
                var ASMCL_ID = 0;
                $scope.clalistsTemp = [];
                if ($scope.classArray != null && $scope.classArray.length > 0) {
                    angular.forEach($scope.classArray, function (stf) {
                        if (stf.selected == true) {
                            $scope.clalistsTemp.push({
                                ASMCL_ID: stf.asmcL_Id
                            });

                            ASMCL_ID = stf.asmcL_Id;

                        }

                    });

                }

                if ($scope.obj.VBSCMCCCL_Id > 0) {
                    VBSCMCCCL_Id = $scope.obj.VBSCMCCCL_Id;
                }

                var data = {
                    "VBSCMCCCL_Id": VBSCMCCCL_Id,
                    "clalists": $scope.clalistsTemp,
                    "VBSCMCC_Id": $scope.obj.VBSCMCC_Id,
                    "ASMCL_ID": ASMCL_ID,



                }
                apiService.create("VBSC_MasterCompetition_Category/savedataCl", data).
                    then(function (promise) {

                        if (promise.returnval == "save") {
                            swal("Record Saved Count : " + promise.savecount + " Duplicate Count : " + promise.duplicatecount +"");

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
            if (item.VBSCMCC_ActiveFlag == true) {
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
                        apiService.create("VBSC_MasterCompetition_Category/DeactivateCl", data).
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
            $scope.obj.MT_Id = user.MT_Id;
            $scope.obj.VBSCMCC_Id = user.VBSCMCC_Id;
            $scope.obj.VBSCMCCCL_Id = user.VBSCMCCCL_Id;
            if ($scope.classArray != null && $scope.classArray.length > 0) {
                angular.forEach($scope.classArray, function (stf) {
                    if (stf.asmcL_Id == user.ASMCL_ID) {
                       
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