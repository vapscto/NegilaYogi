(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeActivityRequestController', fee1)

    fee1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function fee1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.saveddataa = [];
        $scope.loaddata = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeActivityRequest/loaddata", data).
                then(function (promise) {
                    $scope.studentsdata = promise.getstudata;
                    $scope.feeheadgrid = promise.fetheaddata;
                    $scope.savedlist = promise.getstusaveddata;
                })
        }

        $scope.toggleAll = function (allchkdata) {
            if (allchkdata === true) {
                $scope.showsavebtn = true;
            }
            else if (allchkdata === false) {
                $scope.showsavebtn = false;
            }

            var toggleStatus = allchkdata;
            angular.forEach($scope.feeheadgrid, function (itm) {
                itm.isSelected = toggleStatus;
            });
        }

        $scope.toggleAllsave = function (allchkdatasave) {
            if (allchkdatasave === true) {
                $scope.showdeletebtn = true;
            }
            else if (allchkdatasave === false){ 
                $scope.showdeletebtn = false;
            }
            var toggleStatusave = $scope.selectedAllsave;
            angular.forEach($scope.savedlist, function (itm) {
                itm.isSelectedsave = toggleStatusave;
            });
          
        }

        $scope.enabledeletebtn = function (user2) {
            if (user2.isSelectedsave === true) {
                $scope.showdeletebtn = true;
            }
            else if (user2.isSelectedsave === false) {
                $scope.showdeletebtn = false;
            }
        }

        $scope.togglesingle = function (headid) {

        };

        $scope.resultDatadelete = [];
        $scope.DeletRecord = function (savedlist) {

            angular.forEach(savedlist, function (itm) {
                if (itm.isSelectedsave=== true) {
                    $scope.resultDatadelete.push(itm);
                }
            });

            var data = {
                "headnames": $scope.resultDatadelete
            }
            apiService.create("FeeActivityRequest/deletedata", data).
                then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Deleted Sucessfully!");
                        $state.reload();
                    }
                    else {
                        swal("Kindly contact Administrator");
                    }
                })
        }

        $scope.resultData = [];
        $scope.savedata = function (feeheadgrid, amst_id) {


            var checkboxval = false;
            angular.forEach($scope.feeheadgrid, function (itm) {
                if (itm.isSelected === true) {
                    $scope.resultData.push(itm);
                    checkboxval = true;
                }
            })
               if (checkboxval === true) {
                var data = {
                    "headnames": $scope.resultData,
                    "AMST_Id": amst_id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeActivityRequest/savedata", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal("Request Sent Sucessfully!");
                            $state.reload();
                        }
                        else {
                            swal("Kindly contact Administrator");
                        }
                    })
            }
            else {
                swal("Kindly select atleast one head");
            }
        }
    }

}) ();