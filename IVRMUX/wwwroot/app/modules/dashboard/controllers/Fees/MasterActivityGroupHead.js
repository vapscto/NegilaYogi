(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterActivityGroupHead', MasterActivityGroupHead)
    MasterActivityGroupHead.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function MasterActivityGroupHead($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.userPrivileges = [];
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.save = true;
                    $scope.savebtn = true;
                    $scope.savedisable = true;
                }
                else {
                    $scope.save = false;
                    $scope.savebtn = false;

                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;
                    $scope.editdisable = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;

                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                    $scope.deletedisable = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;

                }


            }
        }

        // ============================ng_click function for heads (ALL)========================//
        $scope.searchchkbx1 = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            var count = 0;
            angular.forEach($scope.head_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        //===ng_click function for heads (ALL voice versa)
        $scope.togchkbxC = function () {
            $scope.usercheck = $scope.head_list.every(function (options) {
                return options.selected;
            });
        }
        //=============================================== Grid check box selection
        $scope.deletebutton = true;
        $scope.check_allbox = function () { // all selection
            $scope.deletebutton = true;//delete button desable/enable
            var status = $scope.userselect;
            angular.forEach($scope.get_masterlist, function (itm) {
                itm.griddata = status;
                if (status == true) {
                    $scope.deletebutton = false;
                }
                else {
                    $scope.deletebutton = true;
                }
            });
        }
        $scope.userselect = "";
        $scope.togchkbxCgrid = function () { //single selection  
            $scope.selectedcheckbox = [];
            angular.forEach($scope.get_masterlist, function (set) {
                if (set.griddata == true) {
                    $scope.selectedcheckbox.push(set);
                }
            });
            if ($scope.selectedcheckbox.length > 0) {
                $scope.deletebutton = false;
            }
            else {
                $scope.deletebutton = true;
            }
        }        
        //=============================================clear function========================================//
        $scope.submitted = false;
        $scope.Clearid = function () {
            $scope.AMA_Id = '';
            $scope.AMA_ActivityName = '';
            $scope.AMA_ActivityDesc = '';
            angular.forEach($scope.head_list, function (itm) {
                if (itm.selected) {
                }
                $scope.usercheck = '';
                $scope.FMG_Id = '';
                $scope.selected = '';
                $scope.searchchkbx1 = '';
                $scope.group_list = [];
                $scope.head_list = [];
                $scope.submitted = false;
            });
            $state.reload();
        };
        $scope.isOptionsRequired = function () {
            return !$scope.head_list.some(function (options) {
                return options.selected;
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //============================================load data=====================================================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterActivityGroupHead/loaddata", pageid).then(function (promise) {
                $scope.group_list = promise.group_list;
                $scope.get_masterlist = promise.get_masterlist;
                $scope.get_master = $scope.get_masterlist.length;
            });
        }
        //=======================================get head()=========================================//
        $scope.gethead = function () {
            var data = {
                "FMG_Id": $scope.FMG_Id
            }
            apiService.create("MasterActivityGroupHead/gethead", data).then(function (promise) {
                if (promise.gethead.length > 0) {
                    $scope.head_list = promise.gethead;
                }
                else {
                    swal("No Record Found....!!");
                    $scope.head_list = [];
                }
            })
        }
        //====================================================save data=========================================================//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                $scope.heads = [];
                angular.forEach($scope.head_list, function (rr) {
                    if (rr.selected == true) {
                        $scope.heads.push(rr);
                    }
                });
                var data = {
                    "AMA_Id": $scope.amA_Id,
                    "AMA_ActivityName": $scope.AMA_Id,
                    "AMA_ActivityDesc": $scope.AMA_ActivityDesc,
                    "FMG_Id": $scope.FMG_Id,
                    "headid": $scope.heads,
                }
                apiService.create("MasterActivityGroupHead/savedata", data).then(function (promise) {
                    if (promise.duplicate == true) {
                        swal("Data is already Existing");
                    }
                    else if (promise.msg == 'Saved') {
                        swal("Data is saved");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data is not Saved");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Successfully Updated");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //==================================================active/deactive===========================================//
        $scope.masterDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.amA_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.amA_ActiveFlg == false) {
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
                        apiService.create("MasterActivityGroupHead/masterDecative", usersem).then(function (promise) {
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
        //==================================================deletedata=======================================//
        $scope.deletedata = function () {
            $scope.deletearraydata = [];
            angular.forEach($scope.get_masterlist, function (ss) {
                if (ss.griddata == true) {
                    $scope.deletearraydata.push(ss);
                }
            });
            var data = {
                "listdata23": $scope.deletearraydata,
            }
            apiService.create("MasterActivityGroupHead/deletedata", data).then(function (promise) {
                if (promise.returnval == true) {
                    swal("Record Deleted");
                    $state.reload();
                    return;
                }
                else {
                    swal("Record Not Deleted");
                    $state.reload();
                    return;
                };
            });
        };
    }
})();