(function () {
    'use strict';
    angular
        .module('app')
        .controller('Room_FeeGroup', Room_FeeGroup)

    Room_FeeGroup.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Room_FeeGroup($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.page1 = "page1";
        $scope.search = " ";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.submitted = false;
        $scope.save = function () {


            if ($scope.myForm.$valid) {
                var data = {
                    "HLMRFG_Id": $scope.HLMRFG_Id,
                    "HRMRM_Id": $scope.HRMRM_Id,
                    "FMG_Id": $scope.FMG_Id
                };
                apiService.create("HlMasterRoom_FeeGroup/save", data).then(function (promise) {

                    if (promise.msg === 'Saved') {
                        swal("Data Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Data Not Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'updated') {
                        swal("Data Updated.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'failed') {
                        swal("Data Not Updated Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.duplicate === true) {
                        swal("Data already Exists.....!!!!!");
                    }
                    else {
                        swal("Something is Wrong...");
                    }
                });

            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.search = '';
        $scope.filtervalue1 = function (user) {

        };
        $scope.loaddata = function () {

            var data = {

            };
            apiService.create("HlMasterRoom_FeeGroup/loaddata", data).then(function (promise) {

                $scope.alldata1 = promise.alldata1;
                $scope.room1 = promise.roomid;
                $scope.group1 = promise.groupid;

            });
        };
        $scope.edittab1 = function (user) {

            var data = {
                "HLMRFG_Id": user.hlmrfG_Id
            };
            apiService.create("HlMasterRoom_FeeGroup/edittab1", data).then(function (promise) {

                $scope.HLMRFG_Id = promise.editlist[0].hlmrfG_Id;
                $scope.HRMRM_Id = promise.editlist[0].hrmrM_Id;
                $scope.FMG_Id = promise.editlist[0].fmG_Id;

            });
        };
        $scope.deactivYTab1 = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.hlmrfG_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.hlmrfG_ActiveFlag === false) {
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
                        apiService.create("HlMasterRoom_FeeGroup/deactive", usersem).
                            then(function (promise) {
                                if (promise.ret === true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        };


        $scope.Clearid = function () {
            $state.reload();
        };

    }

})();



