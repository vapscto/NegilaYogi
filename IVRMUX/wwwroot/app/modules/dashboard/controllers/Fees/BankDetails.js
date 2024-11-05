(function () {
    'use strict';
    angular
        .module('app')
        .controller('BankDetailsController', BankDetailsController)

    BankDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function BankDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

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
        $scope.getdata = function () {
          

            if ($scope.myForm.$valid) {            
                var data = {
                    "FBD_ID": $scope.FBD_ID,
                    "Class_Category": $scope.Class_Category,
                    "Class": $scope.Class,
                    "Bank_Name": $scope.Bank_Name,
                    "Bank_Address": $scope.Bank_Address,
                    "Acc_No": $scope.Acc_No,
                    "IFSC": $scope.IFSC,
                    "ACC_name": $scope.ACC_name
                   
                };
               apiService.create("BankDetails/getdata", data).then(function (promise) {

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
            //return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.number)).indexOf(angular.lowercase($scope.search)) >= 0 ||
            //    (angular.lowercase(user.email)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        $scope.getalldetails = function () {
                 
            var data = {
               // pageid = 2
            }          
            apiService.create("BankDetails/getalldetails", data).then(function (promise) {
                $scope.alldata = promise.alldata;          
                $scope.classlist = promise.classlist;//class
            });
        };
         $scope.edittab1 = function (user) {

            var data = {
                "FBD_ID": user.fbD_ID
                
            }
            apiService.create("BankDetails/edittab1", data).then(function (promise) {

                $scope.FBD_ID = promise.editlist[0].fbD_ID;
                $scope.Class_Category = promise.editlist[0].class_Category;
                $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                $scope.Bank_Name = promise.editlist[0].bank_Name;
                $scope.Bank_Address = promise.editlist[0].bank_Address;
                $scope.Acc_No = promise.editlist[0].acc_No;
                $scope.IFSC = promise.editlist[0].ifsc;
                $scope.ACC_name = promise.editlist[0].acC_name;
                $scope.asmcL_ClassName = promise.editlist[0].classname;
              
            });
        };
        $scope.deactivYTab1 = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.active_Flag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.active_Flag === false) {
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
                        apiService.create("BankDetails/deactive", usersem).
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