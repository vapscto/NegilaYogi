(function () {
    'use strict';
    angular
.module('app')
.controller('PFForm12BBInvestmentDeclarationFormatController', PFForm12BBInvestmentDeclarationFormatController)

    PFForm12BBInvestmentDeclarationFormatController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PFForm12BBInvestmentDeclarationFormatController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PFForm12BBInvestmentDeclarationFormat/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                //if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                //    $scope.monthdropdown = promise.monthdropdown;
                //}

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

                //if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                //    $scope.groupTypedropdown = promise.groupTypedropdown;
                //    $scope.groupTypeselectedAll = true;
                //    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
                //}

                //if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                //    $scope.departmentdropdown = promise.departmentdropdown;
                //    $scope.departmentselectedAll = true;
                //    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                //}

                //if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                //    $scope.designationdropdown = promise.designationdropdown;
                //    $scope.designationselectedAll = true;
                //    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                //}
            });
        };


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var groupTypeselected = [];
                var departmentselected = [];
                // var designationselected = [];
                var data = {};

                //angular.forEach($scope.groupTypedropdown, function (itm) {
                //    if (itm.selected) {
                //        groupTypeselected.push(itm.hrmgT_Id);
                //    }
                //});

                //angular.forEach($scope.departmentdropdown, function (itm) {
                //    if (itm.selected) {
                //        departmentselected.push(itm.hrmD_Id);
                //    }
                //});

                //angular.forEach($scope.designationdropdown, function (itm) {
                //    if (itm.selected) {
                //        designationselected.push(itm.hrmdeS_Id);
                //    }
                //});

                //if (groupTypeselected.length == 0 && departmentselected.length == 0) {
                //    swal('Kindly select atleast one record');
                //    return;
                //}

                //$scope.Employee.groupTypeselected = groupTypeselected;
                //$scope.Employee.departmentselected = departmentselected;
                // $scope.Employee.designationselected = designationselected;
                data = $scope.Employee;

                apiService.create("PFForm12BBInvestmentDeclarationFormat/getEmployeedetailsBySelection", data).
                    then(function (promise) {
                        if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                            $scope.EmployeeDis = true;
                            $scope.employeeDetails = promise.employeeDetails[0];
                            $scope.contactnum = promise.contactnum[0].hrmemnO_MobileNo;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                    });
            }
        };



        //By Employee Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;

            });
        }


        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

        }


        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;

            });


        }


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
        }



        //By Designation
        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        }


        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        }


    }


})();