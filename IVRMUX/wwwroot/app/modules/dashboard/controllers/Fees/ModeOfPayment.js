(function () {
    'use strict';
    angular
        .module('app')
        .controller('ModeOfPayment', ModeOfPayment)

    ModeOfPayment.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function ModeOfPayment($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {

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

        //===============clear============//
        $scope.submitted = false;
        $scope.Clearid = function () {

            $scope.IVRMMOD_Id = '';
            $scope.IVRMMOD_ModeOfPayment_Code = '';
            $scope.submitted = false;
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //=============================================== Grid check box selection
        $scope.deletebutton = true;
        $scope.check_allbox = function () { // all section
            $scope.deletebutton = true;
            var status = $scope.userselect;
            angular.forEach($scope.get_payment, function (itm) {
                itm.selected = status;
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
            //$scope.get_payment.every(function (options) {
            //    if (options.selected == true) {
            //        $scope.deletebutton = false;
            //    }
            //    else {
            //        $scope.deletebutton = true;
            //    }
            //});
            $scope.selectedcheckbox = [];
            angular.forEach($scope.get_payment, function (set) {
                if (set.selected == true) {
                    $scope.selectedcheckbox.push(set);
                }
            })
            if ($scope.selectedcheckbox.length > 0) {
                $scope.deletebutton = false;
            }
            else {
                $scope.deletebutton = true;
            }
        }
        ////===================voice versa
        //$scope.togchkbxCgrid = function () {
        //    $scope.userselect = $scope.get_payment.every(function (options) {
        //        if (options.selected == false) {
        //            $scope.deletebutton = true;
        //        }
        //        else {
        //            $scope.deletebutton = false;
        //        }
        //    });
        //}

        //===============================load============================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ModeOfPayment/loaddata", pageid).then(function (promise) {
                $scope.get_payment = promise.get_payment;
                $scope.get_pay = promise.get_payment.length;
            });
        }
        //============================save=====================//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMMOD_ModeOfPayment": $scope.IVRMMOD_Id,
                    "IVRMMOD_ModeOfPayment_Code": $scope.IVRMMOD_ModeOfPayment_Code,
                }
                apiService.create("ModeOfPayment/savedata", data).then(function (promise) {
                    if (promise.duplicate == true) {
                        swal("Data is already existing");
                    }
                    else if (promise.msg == 'Saved') {
                        swal("Data is saved successfully");
                        $state.reload();
                    }
                    else if (promise.msg == 'Updated') {
                        swal = ("Data is updated successfully");
                    }
                    else {
                        swal("Data is not saved");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //==============================================deletedata================================================//
        $scope.deletedata = function () {
            $scope.deletearray = [];
            angular.forEach($scope.get_payment, function (pp) {
                if (pp.selected == true) {
                    $scope.deletearray.push(pp);
                }
            });
            var data = {
                listdata07: $scope.deletearray
            }
            apiService.create("ModeOfPayment/deletedata", data).then(function (promise) {
                if (promise.returnval == true) {
                    swal("Record Deactivated Successfully");
                    $state.reload();
                    return;
                }
                else {
                    swal("Record is not Deactivated!!! Conatct Administrator");
                    $state.reload();
                    return;
                }
            });
        }
        //============================================paymentDecative==============================//
        $scope.paymentDecative = function (usersem, SweetAlert) {

            var dystring = "";

            if (usersem.ivrmmoD_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ivrmmoD_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("ModeOfPayment/paymentDecative", usersem).
                            then(function (promise) {

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