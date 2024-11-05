(function () {
    'use strict';
    angular
.module('app')
.controller('SalaryCalculationController', SalaryCalculationController)

    SalaryCalculationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function SalaryCalculationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
       


       // $scope.SalaryDate = true;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.reverse = true;
        $scope.EmployeeByMonthYearDis = false;
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("employeeSalaryCalculation/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }


                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}

                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
                }


                if (promise.employeeTypedropdown !== null && promise.employeeTypedropdown.length > 0) {
                    $scope.employeeTypedropdown = promise.employeeTypedropdown;

                    $scope.employeeTypeselectedAll = true;
                    $scope.GetEmployeeByTypeAll($scope.employeeTypeselectedAll);

                }

               
                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;

                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);

                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;

                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                }




                if (promise.configurationDetails != null) {

                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;

                }

            })
        }


        $scope.SetMonthByYear = function (year) {
            // swal(year);
            if (($scope.Employee.hreS_Month != null && $scope.Employee.hreS_Month != "" && $scope.Employee.hreS_Month != undefined) && ($scope.Employee.hreS_Year != null && $scope.Employee.hreS_Year != "" && $scope.Employee.hreS_Year != undefined)) {

                $scope.DisplayDay = true;
                var out = $scope.monthdropdown.filter(function (x) {
                    var match = x.ivrM_Month_Name == $scope.Employee.hreS_Month;

                    return match;
                });


                var yearId = parseInt($scope.Employee.hreS_Year);
                var monthId = parseInt(out[0].ivrM_Month_Id);
                var startDay = parseInt($scope.SalaryFromDay);
                var endDay = parseInt($scope.SalaryToDay);

                //  var prevyearId = 0;
                //  var prevMonthId = 0;

                if (startDay > 1 && monthId < 12) {

                    $scope.hreS_FromDate = startDay + '/' + monthId + '/' + yearId;
                    $scope.hreS_ToDate = endDay + '/' + (monthId + 1) + '/' + yearId;
                } else if (startDay > 1 && monthId == 12) {

                    $scope.hreS_FromDate = startDay + '/' + monthId + '/' + yearId;
                    $scope.hreS_ToDate = endDay + '- 01 -' + (yearId + 1);
                } else {

                    $scope.hreS_FromDate = startDay + '/' + monthId + '/' + yearId;
                    var days = getNumberOfDays(yearId, monthId);

                    $scope.hreS_ToDate = days + '/' + monthId + '/' + yearId;
                }



                $scope.SearchEmployee();

            }
            else {
                $scope.DisplayDay = false;
            }

        }
       
        $scope.DisplayDay = false;
        $scope.SetFromDateAndToDateByMonth = function (hreS_Month, hreS_Year) {

            if ((hreS_Month != null && hreS_Month != "" && hreS_Month != undefined) && (hreS_Year != null && hreS_Year != "" && hreS_Year != undefined)) {
                $scope.DisplayDay = true;

            var out = $scope.monthdropdown.filter(function (x) {
                var match = x.ivrM_Month_Name == hreS_Month;
              
                return match;
            });


            
            var yearId = parseInt(hreS_Year);
            var monthId =parseInt(out[0].ivrM_Month_Id);
            var startDay = parseInt($scope.SalaryFromDay);
            var endDay = parseInt($scope.SalaryToDay);

          //  var prevyearId = 0;
          //  var prevMonthId = 0;

            if (startDay > 1 && monthId < 12) {

                $scope.hreS_FromDate = startDay + '/' + monthId + '/' + yearId;
                $scope.hreS_ToDate = endDay + '/' + (monthId + 1) + '/' + yearId;
            } else if (startDay > 1 && monthId == 12) {

                $scope.hreS_FromDate = startDay + '/' + monthId + '/' + yearId;
                $scope.hreS_ToDate = endDay + '- 01 -' + (yearId + 1);
            } else {

                $scope.hreS_FromDate = startDay + '/' + monthId + '/' + yearId;
                var days = getNumberOfDays(yearId, monthId);

                $scope.hreS_ToDate = days + '/' + monthId + '/' + yearId;
            }


            //if (monthId == 1) {
            //    prevMonthId = 12;
            //    prevyearId = yearId - 1;
            //} else {
            //     prevyearId = yearId;
            //     prevMonthId = monthId; //prevMonthId = monthId - 1;

            //}

            //$scope.Employee.hreS_FromDate = new Date('' + prevyearId + '-' + prevMonthId + '-' + startDay + '');
            //$scope.Employee.hreS_ToDate = new Date('' + yearId + '-' + monthId + '-' + endDay + '');

            $scope.SearchEmployee();

            }
            else {
                $scope.DisplayDay = false;
            }
        }


        function getNumberOfDays(year, month) {
            var isLeap = ((year % 4) == 0 && ((year % 100) != 0 || (year % 400) == 0));
            return [0,31, (isLeap ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
        }


        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        }


        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {

                return itm.selected;
            });

            $scope.get_depts();
        };
        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids
            }
            apiService.create("employeeSalaryCalculation/get_depts", data).
                        then(function (promise) {

                            if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                                $scope.departmentdropdown = promise.departmentdropdown;
                                $scope.departmentselectedAll = true;
                                $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                            }
                        })
        };



        //By Employee Type
        $scope.GetEmployeeByTypeAll = function (employeeTypeselectedAll) {

            var toggleStatus = $scope.employeeTypeselectedAll;
            angular.forEach($scope.employeeTypedropdown, function (itm) {
                itm.selected = toggleStatus;

            });
        }


        //single
        $scope.GetEmployeeByType = function (employeeType) {

            $scope.employeeTypeselectedAll = $scope.employeeTypedropdown.every(function (itm) {

                return itm.selected;
            });

        }
        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            })
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            })
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            }
            apiService.create("employeeSalaryCalculation/get_desig", data).
                        then(function (promise) {
                            if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                                $scope.designationdropdown = promise.designationdropdown;
                                $scope.designationselectedAll = true;
                                $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                            }
                        })
        };


        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            })

            $scope.get_desig();

        }


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
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



        //Employeee
     

        //By Employeee
        $scope.GetEmployeeAll = function (employeeSelectedAll) {

            var toggleStatus = $scope.employeeSelectedAll;
            angular.forEach($scope.employeedropdown, function (itm) {
                itm.selected = toggleStatus;

            });

        }


        //By Employeee Single
        $scope.GetEmployee = function (designation) {

            $scope.employeeSelectedAll = $scope.employeedropdown.every(function (itm) {

                return itm.selected;
            });
        }

        $scope.EmployeeDis = false;
        //Search employee
       // $scope.submitted = false;
        $scope.SearchEmployee = function () {

          //  $scope.submitted = true;
            //  if ($scope.myForm.$valid) {
            $scope.EmployeeByMonthYearDis = false;
            var groupTypeselected = [];

            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }
            });

            var employeeTypeselected = [];
            angular.forEach($scope.employeeTypedropdown, function (itm) {
                if (itm.selected) {
                    employeeTypeselected.push(itm.hrmeT_Id);
                }

            });

            var departmentselected = [];
            angular.forEach($scope.departmentdropdown, function (itm) {
                if (itm.selected) {
                    departmentselected.push(itm.hrmD_Id);
                }

            });


            var designationselected = [];
            angular.forEach($scope.designationdropdown, function (itm) {
                if (itm.selected) {
                    designationselected.push(itm.hrmdeS_Id);
                }

            });

            if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0) {
                swal('Kindly select atleast one record');
                return;
            }
            $scope.employeedropdown = [];
            var data = {
                hrmgT_IdList: groupTypeselected,
                hrmD_IdList: departmentselected,
                hrmdeS_IdList: designationselected,
                "HRES_Year": $scope.Employee.hreS_Year,
                "HRES_Month": $scope.Employee.hreS_Month
            }

            apiService.create("employeeSalaryCalculation/getEmployeedetailsBySelection", data).
                        then(function (promise) {

                            if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                                $scope.employeedropdown = promise.employeedropdown;
                                $scope.EmployeeDis = true;

                                angular.forEach($scope.employeedropdown, function (value, key) {
                                    $scope.employeeSelectedAll = true;
                                    $scope.GetEmployeeAll($scope.employeeSelectedAll);

                                });

                                if (($scope.Employee.hreS_Month != null && $scope.Employee.hreS_Month != "" && $scope.Employee.hreS_Month != undefined) && ($scope.Employee.hreS_Year != null && $scope.Employee.hreS_Year != "" && $scope.Employee.hreS_Year != undefined)) {
                                    $scope.EmployeeByMonthYearDis = true;
                                } else {
                                    $scope.EmployeeByMonthYearDis = false;
                                }

                            }
                            else {
                                $scope.EmployeeDis = false;
                                $scope.employeeSelectedAll = false;
                                $scope.GetEmployeeAll($scope.employeeSelectedAll);
                                swal('No Record found to display .. !');
                                return;
                            }

                        })
    }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.submitted = false;
            $scope.EmployeeByMonthYearDis = false;
            $scope.groupTypeselectedAll = false;
            $scope.employeeTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            $scope.EmployeeDis = false;
            $scope.search = "";
            $scope.DisplayDay = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

     

        //Generate salary

        //Search employee
        $scope.submitted = false;
        $scope.GenerateEmployeeSalary = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var employeeselected = [];
                angular.forEach($scope.employeedropdown, function (itm) {
                    if (itm.selected) {
                        employeeselected.push(itm);
                    }

                });

                if (employeeselected.length == 0) {
                    swal('Kindly select atleast one Employee to generate salary..!');
                    return;
                }

                $scope.Employee.masterEmployeeList = employeeselected;
                var data = $scope.Employee;
                
                apiService.create("employeeSalaryCalculation/calculateSelectedEmployeeSalary", data).
                            then(function (promise) {

                                if (promise.retrunMsg !== "") {

                                    if (promise.retrunMsg == "ConfigurationMissing") {

                                        swal("Configuration Not done for HRMS");
                                        return;

                                    }
                                    else if (promise.retrunMsg == "MethodNotFound") {
                                        swal('Salary Calculation Method type not set in configuration');
                                        return;
                                    }
                                    else if (promise.retrunMsg == "ConfigurationMissing") {
                                        swal(promise.retrunMsg);
                                    }
                                    else if (promise.retrunMsg == "Generated") {
                                        swal("Salary Generated");
                                        $scope.cleardata();
                                        $scope.onLoadGetData();
                                    }
                                    else if (promise.retrunMsg == "NotGenerated") {
                                        swal('Salary Not Generated', 'Something went wrong .. Try again later');
                                        $scope.cleardata();
                                        $scope.onLoadGetData();
                                        return;
                                    }
                                   
                                    

                                }
                            })
            }

            

        }


        // sort table data
        $scope.sortTableData = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }
})();