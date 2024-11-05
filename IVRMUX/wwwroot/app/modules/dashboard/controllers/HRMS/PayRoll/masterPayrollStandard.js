(function () {
    'use strict';
    angular
.module('app')
.controller('masterPayrollStandardController', masterPayrollStandardController)

    masterPayrollStandardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function masterPayrollStandardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        $scope.payrolstan = {};

        $scope.cancelDis = false;
        $scope.onLoadGetData = function () {

            var pageid = 2;
            apiService.getURI("MasterPayrollStandard/getalldetails", pageid).then(function (promise) {


               if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


                }
                if (promise.hrStandardList !== null && promise.hrStandardList.length > 0) {
                    $scope.payrolstan = promise.hrStandardList[0];


                    if ($scope.payrolstan.hrC_ArrSalaryFlag === true) {
                        $scope.payrolstan.hrC_ArrSalaryFlag = 'Yes';
                    }
                    else {
                        $scope.payrolstan.hrC_ArrSalaryFlag = 'No';
                    }


                    if ($scope.payrolstan.hrC_CummArrFlag === true) {
                        $scope.payrolstan.hrC_CummArrFlag = 'Yes';
                    }
                    else {
                        $scope.payrolstan.hrC_CummArrFlag = 'No';
                    }



                  
                    $scope.cancelDis = true;
                }
            })
        }

        //compare both dates  payrolstan.hrC_SalaryFromDay,payrolstan.hrC_SalaryToDay
        //$scope.checkErr = function (FromDay, ToDay) {
        //    if (parseInt(FromDay) > parseInt(ToDay)) {
        //        if (parseInt(ToDay) != (parseInt(FromDay) - 1)) {

        //            swal("Salary To day should be one day Less than from day", 'Please Change Your day!');
        //            $scope.payrolstan.hrC_SalaryToDay = "";
        //            return false;
        //        }
               
        //    } else {
        //        swal("Salary From day should be greater than To day", 'Please Change Your day!');
        //        $scope.payrolstan.hrC_SalaryToDay = "";
        //        return false;
        //    }
        //};

        //$scope.checkErr1 = function (FromDay, ToDay) {
        //    if (parseInt(FromDay) > parseInt(ToDay)) {
        //        if (parseInt(ToDay) != (parseInt(FromDay) - 1)) {

        //            swal("Salary To day should be one day Less than from day", 'Please Change Your day!');
        //            $scope.payrolstan.hrC_SalaryToDay = "";
        //            return false;
        //        }
        //    } else {
        //        swal("Salary To day should be Less than from day", 'Please Change Your day!');
        //        $scope.payrolstan.hrC_SalaryToDay = "";
        //        return false;
        //    }
        //};


        //GET DAY IN DROPDOWN
        $scope.getDateRange = function (num) {
            return new Array(num);
        }



        // clear form data
        $scope.cancel = function () {
            $scope.payrolstan = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();

        }

        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.payrolstan.hrC_ArrSalaryFlag === 'Yes') {
                    $scope.payrolstan.hrC_ArrSalaryFlag = true;
                }
                else {
                    $scope.payrolstan.hrC_ArrSalaryFlag = false;
                }


                if ($scope.payrolstan.hrC_CummArrFlag === 'Yes') {
                    $scope.payrolstan.hrC_CummArrFlag = true;
                }
                else {
                    $scope.payrolstan.hrC_CummArrFlag = false;
                }


             
                var data = $scope.payrolstan;
                apiService.create("MasterPayrollStandard/", data).
                then(function (promise) {

                    if (promise.retrunMsg !== "") {

                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }

                        else if (promise.retrunMsg == "false") {
                            swal("Record Not saved / Updated..", 'Fail');

                        }
                        else if (promise.retrunMsg == "Add") {
                            swal("Record Saved Successfully..");
                        }
                        else if (promise.retrunMsg == "Update") {
                            swal("Record Updated Successfully..");
                        }
                        else if (promise.retrunMsg == "Order") {
                            swal("Already Exist the Course Name..");
                        }

                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }

                        $scope.onLoadGetData();
                        
                    }
                })
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.SetArrearValue = function (hrC_ArrSalaryFlag) {

           // alert(hrC_ArrSalaryFlag);

        }

        
    }
})();