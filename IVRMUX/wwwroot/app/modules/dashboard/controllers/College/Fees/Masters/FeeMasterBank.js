
(function () {
    'use strict';
    angular
.module('app')
        .controller('FeeheadClgController', FeeheadClgController)

    FeeheadClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FeeheadClgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            var data = {
                "pageid": 2
            }
            apiService.create("FeeHeadClg/getallbankdetails", data).
                then(function (promise) {
                    if (promise.getbankdetails.length>0) {
                        $scope.students = promise.getbankdetails;
                        $scope.presentCountgrid = $scope.students.length;

                    }
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.submitted = false;
        $scope.savedata = function () {

            if ($scope.myFormhead.$valid) {

                var data = {
                    "FMBANK_Id": $scope.FMBANK_Id,
                    "FMBANK_BankName": $scope.FMBANK_BankName,
                    "FMBANK_BankDescription": $scope.FMBANK_BankDescription,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeHeadClg/savedata", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal("Records Saved Successfully")
                        }
                        else if (promise.returnval === false) {
                            swal("Records not Saved Successfully")
                        }
                        $state.reload();
                    })
            }
        }

        //--Clear--//
        $scope.cance = function () {
            $state.reload();
        }

        //--Edit--//
        $scope.edit = function (user) {
            var data = {
                "FMBANK_Id": user.fmbanK_Id,
            }
            apiService.create("FeeHeadClg/edit", data).then(function (promise) {
                if (promise.geteditdata.length > 0) {
                    $scope.FMBANK_BankName = promise.geteditdata[0].fmbanK_BankName;
                    $scope.FMBANK_BankDescription = promise.geteditdata[0].fmbanK_BankDescription;
                    $scope.FMBANK_Id = promise.geteditdata[0].fmbanK_Id;
                
                }
                else {
                }
            })
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.fmbanK_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeHeadClg/activedeactive/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $state.reload();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();
                });
        }
    };
})();


