(function () {
    'use strict';

    angular
        .module('app')
        .controller('EmployeeSalaryIncreementProcessController', EmployeeSalaryIncreementProcessController);

    EmployeeSalaryIncreementProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function EmployeeSalaryIncreementProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $route, superCache) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.obj = {};

        $scope.HREIC_IncrementDate = new Date();
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.employeedropdown = [];
        //=====================================Load Data
        $scope.loadgrid = function () {

            $scope.all_check_empl = function () {
                var checkStatus = $scope.empl;
                var count = 0;
                angular.forEach($scope.employeedropdown, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
            }
            $scope.isOptionsRequired3 = function () {
                return !$scope.employeedropdown.some(function (options) {
                    return options.selected;
                });
            }

            $scope.addColumn4 = function () {

                $scope.empl = $scope.employeedropdown.every(function (options) {
                    return options.selected;
                });
            }


            apiService.getURI("EmployeeSalaryIncreementProcess/getalldetails/", 1).then(function (promise) {
                $scope.employeedropdown = promise.employeelist;
                $scope.headdropdown = promise.earningdeductiontype;
                $scope.monthlist = promise.monthdropdown;
                $scope.get_grid = promise.griddata;
                $scope.presentCountgrid = $scope.get_grid.length;


            });
        }
        //========================================================================================
        $scope.GetDetailsByEmployee = function (hrmE_Id) {

            var data = {
                "HRME_Id": hrmE_Id.hrmE_Id.hrmE_Id,
                //"HRME_Id": $scope.hrmE_Id,     
            }
            apiService.create("EmployeeSalaryIncreementProcess/Empdetails", data).then(function (promise) {

                $scope.getempsdetails = promise.getempsdetails;

                $scope.HRMD_DepartmentName = promise.getempsdetails[0].HRMD_DepartmentName;
                $scope.HRMDES_DesignationName = promise.getempsdetails[0].HRMDES_DesignationName;
                $scope.HRME_DOJ = new Date(promise.getempsdetails[0].HRME_DOJ).toDateString();

            })
        }

        // save -------------------

        $scope.savedata = function () {
            $scope.submitted = true;

            $scope.albumNameArray1 = [];
            angular.forEach($scope.employeedropdown, function (role) {
                if (role.selected) $scope.albumNameArray1.push(role);
            })

            if ($scope.myForm.$valid) {
                var percentage = 0;
                var amount = 0;
                if ($scope.radioval == 'Percentage') {
                    percentage = $scope.hresA_AppliedAmount;
                }
                else {
                    amount = $scope.hresA_AppliedAmount;
                }

                var data = {
                    //"HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
                     employee: $scope.albumNameArray1,
                    "HRMED_Id": $scope.obj.hrmeD_Id.hrmeD_Id,
                    "HREIC_IncrementDate": new Date($scope.HREIC_IncrementDate).toDateString(),
                    "HREICED_Amount": amount,
                    "HREICED_Percentage": percentage,


                }

                apiService.create("EmployeeSalaryIncreementProcess/", data).then(function (promise) {

                    if (promise.retrunMsg == 'Add') {

                        swal('Record saved successfully');


                    }

                    else {
                        swal('Failed to save, please contact administrator');

                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.isOptionsRequired3 = function () {            return !$scope.employeedropdown.some(function (options) {                return options.selected;            });        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

        // edit

        $scope.edit = function (item, get_org) {            var data = {                "HRME_Id": item.hrmE_Id,            };            apiService.create("EmployeeSalaryIncreementProcess/Edit", data).then(function (promise) {                if (promise !== null) {                    $scope.getloaddetails = promise.getloaddetails;                    $scope.editflag = true;                    $scope.HRMRAM_Id = promise.getloaddetails[0].hrmraM_Id;                    $scope.HRMR_Id = promise.getloaddetails[0].hrmR_Id;                    $scope.perday = promise.getloaddetails[0].hrmraM_RentPerDay;                    $scope.hrs = promise.getloaddetails[0].hrmraM_NoOfHrs;                    $scope.perhrs = promise.getloaddetails[0].hrmraM_RentPerHour;                    $scope.HRMRAM_filepath = promise.getloaddetails[0].hrmraM_filepath;                }            });        }

        // deactive
        $scope.deactive = function (item) {
            var data = {
                "HREIC_Id": item.HREIC_Id,
            };

            var dystring = "Delete";

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
                        apiService.getURI("EmployeeSalaryIncreementProcess/ActiveDeactiveRecord", item.HREIC_Id).
                            then(function (promise) {
                                if (promise.retrunMsg == "updated") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }






        //  ===================================

    }
})();
