(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterSpecialisation', masterSpecialisation)
    masterSpecialisation.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function masterSpecialisation($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.Clearid = function () {
            $scope.HRMSPL_SpecialisationName = '';
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };       
        //============load
        $scope.submitted = false;
        $scope.loaddata = function () {           
            var pageid = 2;
            apiService.getURI("masterSpecialisation/loaddata", pageid).then(function (promise) {               
                $scope.alldata = promise.alldata;
                $scope.get_master = $scope.alldata.length;
            });
        }
        //================save
        $scope.savedata = function () {            
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMSPL_Id": $scope.HRMSPL_Id,
                    "MI_Id": $scope.MI_Id,
                    "HRMSPL_SpecialisationName": $scope.HRMSPL_SpecialisationName,
                }
                apiService.create("masterSpecialisation/savedata", data).then(function (promise) {                   
                    if (promise.duplicate == true) {
                        swal("Record already existing");
                    }
                    else if (promise.msg == 'Saved') {
                        swal("Record saved succussfully");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Record not saved succussfully");
                    }
                    else if (promise.msg == 'Updated') {
                        swal("Record updated succussfully")
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //==============Edit
        $scope.EditData = function (user) {
            var data = {
                "HRMSPL_Id": user.hrmspL_Id	
            }
            apiService.create("masterSpecialisation/EditData", data).then(function (promise) {
                if (promise.editlist !== null && promise.editlist.length > 0) {
                    $scope.editlist = promise.editlist;
                    $scope.HRMSPL_Id = promise.editlist[0].hrmspL_Id;
                    $scope.MI_Id = promise.editlist[0].mI_Id;
                    $scope.HRMSPL_SpecialisationName = promise.editlist[0].hrmspL_SpecialisationName;                   
                }
            });
        }
        //=============masterDecative
        $scope.masterDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.hrmspL_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.hrmspL_ActiveFlg == false) {
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
                        apiService.create("masterSpecialisation/masterDecative", usersem).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfull!!!");
                                $state.reload();
                            }
                        })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
    }
})();