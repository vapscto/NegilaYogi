(function () {
    'use strict';
    angular
        .module('app')
        .controller('GroupDeptDessgMappingcontroller', GroupDeptDessgMappingcontroller)
    GroupDeptDessgMappingcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function GroupDeptDessgMappingcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
       // $scope.edit_c = true;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        // ============================ng_click function for cast (ALL)========================//
        $scope.searchchkbx1 = "";
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.designation_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        };

        //===ng_click function for cast (ALL voice versa)
        $scope.togchkbxC = function () {
            $scope.usercheck = $scope.designation_list.every(function (options) {
                return options.selected;
            });
        };

        //=============================================clear function========================================//
        $scope.submitted = false;
        $scope.Clearid = function () {        
           
            angular.forEach($scope.designation_list, function (itm) {
                if (itm.selected) {
                }
                $scope.usercheck = '';            
                $scope.searchchkbx1 = '';
                $scope.IVRMMR_Id = '';
                $scope.selected = '';
                $scope.searchchkbx1 = '';
                $scope.religion_list = [];
                $scope.designation_list = [];
                $scope.submitted = false;
                $scope.edit_c = false;
            });
            $state.reload();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.designation_list.some(function (options) {
                return options.selected;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //============================================load data=====================================================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("GroupDeptDessg/loaddata", pageid).then(function (promise) {
                $scope.group_list = promise.groupTypedropdown;
                $scope.department_list = promise.departmentdropdown;
                $scope.designation_list = promise.designationdropdown;
                $scope.gridviewdata = promise.gridviewdata;
            });
        };

        //==============================save data=================================//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                $scope.designations = [];
                angular.forEach($scope.designation_list, function (rr) {
                    if (rr.selected == true) {
                        $scope.designations.push(rr.hrmdeS_Id);
                    }
                });
                var data = {
                    "HRMGT_Id": $scope.HRMGT_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                    "designationids": $scope.designations
                };
                apiService.create("GroupDeptDessg/savedata", data).then(function (promise) {
                    if (promise.msg == 'Saved') {
                        swal("Data is saved");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data is not Saved");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //=================Edit
        $scope.Editdata = function (user) {
            var data = {
                "HRGTDDS_Id": user.hrgtddS_Id
            };
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("GroupDeptDessg/Editdata", data).then(function (promise) {
                        if (promise.returnval == true) {
                            swal("Record Deleted Successfully!!!");
                            $state.reload();
                        }
                        else {
                            swal("Record Not Deleted Successfull!!!");
                            $state.reload();
                        }
                    });
                }
                else {
                    swal("Record Deletion Cancelled!!!");
                }
            });
        };

        //==================================================active/deactive===========================================//
        $scope.masterDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.hrgtddS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.hrgtddS_ActiveFlg == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To" + dystring + "Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + "it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("GroupDeptDessg/masterDecative", usersem).then(function (promise) {
                        if (promise.returnval == true) {
                            swal("Record " + dystring + "d" + " Successfully!!!");
                            $state.reload();
                        }
                        else {
                            swal("Record Not " + dystring + "d" + " Successfull!!!");
                            $state.reload();
                        }
                    });
                }
                else {
                    swal("Record Deactivation Cancelled!!!");
                }
            });
        };
    }
})();