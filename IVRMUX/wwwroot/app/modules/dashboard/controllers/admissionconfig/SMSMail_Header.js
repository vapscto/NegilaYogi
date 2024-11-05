(function () {
    'use strict';
    angular
        .module('app')
        .controller('SMSMail_HeaderController', SMSMail_HeaderController);

    SMSMail_HeaderController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function SMSMail_HeaderController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;
        $scope.page1 = "page1";
        $scope.search = " ";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.submitted = false;
        $scope.getdata1 = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMH_Id": $scope.ISMH_Id,
                   
                    "ISMH_HeaderName": $scope.ismH_HeaderName,
                };
                apiService.create("SMSMail_Header/getdata", data).then(function (promise) {
                   
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
        $scope.getalldetails = function () {
            var data = 1;
            apiService.getURI("SMSMail_Header/getalldetails", data).then(function (promise) {
                $scope.alldata = promise.alldata;
            });
        };

        $scope.edittab1 = function (user) {

            var data = {
                "ISMH_Id": user.ismH_Id
            };
            apiService.create("SMSMail_Header/edittab1", data).then(function (promise) { 
                //$scope.ismH_Id = promise.editlist[0].ismH_Id;
                $scope.ISMH_Id = promise.editlist[0].ismH_Id;
                $scope.ismH_HeaderName = promise.editlist[0].ismH_HeaderName;
            });
        };


        $scope.deletetrip = function (user) {
            var mgs = "Delete";
            var confirmmgs = "Deleted";

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

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("SMSMail_Header/delete", user).
                            then(function (promise) {

                                if (promise.msg == 'success') {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {

                                    swal("Record " + mgs + " Failed");
                                }



                                $state.reload();

                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.Clearid = function () {
            $state.reload();
        };     
    }
})();