(function () {
    'use strict';
    angular
.module('app')
.controller('salaryUpdationController', salaryUpdationController)
    salaryUpdationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function salaryUpdationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.reverse = true;



        $scope.Employee = {};

        $scope.Employee.EarningDeduction = 'All';
        $scope.Employee.AmountPercentage = 'AllAP';
       

        // sort table data
        $scope.sortTableData = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("salaryUpdation/getalldetails", pageid).then(function (promise) {
                
                
                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
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

                //head dropdown
                if (promise.headdropdown !== null && promise.headdropdown.length > 0) {

                    $scope.headdropdownOnLoad = promise.headdropdown;

                    $scope.headdropdown = $scope.headdropdownOnLoad;


                    $scope.headselectedAll = true;
                    $scope.GetEmployeeByheadAll($scope.headselectedAll);
                }


            })
        }

        $scope.SearchEmployee = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.employeeSelectedAll = false;
            $scope.employeeDetails = [];
            var groupTypeselected = [];
            var departmentselected = [];
            var designationselected = [];
            var headselected = [];
            var data = {};

            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }
            });

            angular.forEach($scope.departmentdropdown, function (itm) {
                if (itm.selected) {
                    departmentselected.push(itm.hrmD_Id);
                }
            });

            angular.forEach($scope.designationdropdown, function (itm) {
                if (itm.selected) {
                    designationselected.push(itm.hrmdeS_Id);
                }
            });

            angular.forEach($scope.headdropdown, function (itm) {
                if (itm.selected) {
                    headselected.push(itm.hrmeD_Id);
                }
            });



            if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0 && headselected.length == 0) {
                swal('Kindly select atleast one record');
                return;
            }

            $scope.Employee.groupTypeselected = groupTypeselected;
            $scope.Employee.departmentselected = departmentselected;
            $scope.Employee.designationselected = designationselected;
            $scope.Employee.headselected = headselected;

            data = $scope.Employee;

            apiService.create("salaryUpdation/FilterEmployeeData", data).
                           then(function (promise) {

                               if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                                   $scope.EmployeeDis = true;

                                   // $scope.employeeDetails = promise.employeeDetails;
                                  
                                   $scope.employeeDetails = [];
                                   angular.forEach(promise.employeeDetails, function (itm) {
                                       itm.disable = true;
                                        $scope.employeeDetails.push(itm);
                                   });

                                   $scope.earningheadlist = $scope.employeeDetails[0].earningresult;
                                   $scope.deductionheadlist = $scope.employeeDetails[0].deductionresult;


                               } else {

                                   swal('No Record Found to display..');
                                   if ($scope.EmployeeDis) {
                                       $scope.EmployeeDis = false;

                                       $scope.employeeSelectedAll = false;
                                       $scope.GetEmployeeAll($scope.employeeSelectedAll);
                                   }

                               }
                           })

        }


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.UpdateEmployeeDetails = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var employeeselected = [];

              
                angular.forEach($scope.employeeDetails, function (itm) {
                    if (itm.selected) {

                        employeeselected.push(itm);//
                    }
                });


                if (employeeselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

                var data = {
                    selectedEmpdetails : employeeselected
                };
                   
                   

              // var data = $scope.Employee;

                apiService.create("salaryUpdation/getEmployeedetailsBySelection", data).
                            then(function (promise) {

                                if (promise.retrunMsg !== "") {
                                     if (promise.retrunMsg == "false") {
                                         swal("Record Not Updated..");
                                         return;
                                    }
                                     else if (promise.retrunMsg == "updated") {
                                         swal("Record Updated Successfully..");
                                         $scope.cleardata();
                                    }
                                   
                                }
                                else {
                                    $scope.EmployeeDis = false;
                                    swal('No Record found to Update .. !');
                                    return;
                                }
                            })
            }
        }

        //By group Type
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

            $scope.departmentdropdown = [];
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            $scope.departmentselectedAll = false;

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
            apiService.create("salaryUpdation/get_depts", data).
                        then(function (promise) {

                            if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                                $scope.departmentdropdown = promise.departmentdropdown;
                                $scope.departmentselectedAll = true;
                                $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
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
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
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
            apiService.create("salaryUpdation/get_desig", data).
                        then(function (promise) {
                            if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                                $scope.designationdropdown = promise.designationdropdown;
                                $scope.designationselectedAll = true;
                                $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                            }
                        })
        };



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
            angular.forEach($scope.employeeDetails, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected) {
                    itm.disable = false;
                }
                else {

                    itm.disable = true;
                }

            });

        }


        //By Employeee Single
        $scope.GetEmployee = function (employee) {
            $scope.employeeSelectedAll = $scope.employeeDetails.every(function (itm) {
                return itm.selected;
            });
            if (employee.selected) {
                employee.disable = false;
            }
            else {

                employee.disable = true;
            }

        }

        //By head
        $scope.GetEmployeeByheadAll = function (headselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.headselectedAll;
            angular.forEach($scope.headdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
        }

        $scope.EmpSelectAll = function (selectallemp) {
            if ($scope.selectallemp == true) {
                angular.forEach($scope.employeeDetails, function (itm) {
                    itm.disable = false;
                    return itm.selected = true;
                    
                });
            }else {
                angular.forEach($scope.employeeDetails, function (itm) {  
                    itm.disable = true;
                    return itm.selected = false;
                   
                });
            }            
        }

        //By head Single
        $scope.GetEmployeeByhead = function (head) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.headselectedAll = $scope.headdropdown.every(function (itm) {
                return itm.selected;
            });
        }

        //Clear data
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            $scope.headselectedAll = false;
            $scope.search = "";
            $scope.Employee.EarningDeduction = 'All';
            $scope.Employee.AmountPercentage = 'AllAP';

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


        $scope.GetHeadListByFilterSelection = function (data) {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            //All
            if ($scope.Employee.EarningDeduction == 'All' && $scope.Employee.AmountPercentage == 'AllAP') {
                $scope.headdropdown = $scope.headdropdownOnLoad;


            } else if ($scope.Employee.EarningDeduction == 'All' && $scope.Employee.AmountPercentage == 'Amount') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {
                    return d.hrmeD_AmountPercentFlag === 'Amount';
                });

                $scope.headdropdown = list;


            } else if ($scope.Employee.EarningDeduction == 'All' && $scope.Employee.AmountPercentage == 'Percentage') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {
                    return d.hrmeD_AmountPercentFlag === 'Percentage';
                });

                $scope.headdropdown = list;
            }
            //Earning
            else if ($scope.Employee.EarningDeduction == 'Earning' && $scope.Employee.AmountPercentage == 'AllAP') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {
                    return d.hrmeD_EarnDedFlag === 'Earning';
                });

                $scope.headdropdown = list;
               
            } else if ($scope.Employee.EarningDeduction == 'Earning' && $scope.Employee.AmountPercentage == 'Amount') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {

                    return d.hrmeD_EarnDedFlag === 'Earning' && d.hrmeD_AmountPercentFlag === 'Amount';
                });

                $scope.headdropdown = list;


            } else if ($scope.Employee.EarningDeduction == 'Earning' && $scope.Employee.AmountPercentage == 'Percentage') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {

                    return d.hrmeD_EarnDedFlag === 'Earning' && d.hrmeD_AmountPercentFlag === 'Percentage';
                });

                $scope.headdropdown = list;


            }

            //Deduction
            else if ($scope.Employee.EarningDeduction == 'Deduction' && $scope.Employee.AmountPercentage == 'AllAP') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {
                    return d.hrmeD_EarnDedFlag === 'Deduction';
                });

                $scope.headdropdown = list;


            } else if ($scope.Employee.EarningDeduction == 'Deduction' && $scope.Employee.AmountPercentage == 'Amount') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {

                    return d.hrmeD_EarnDedFlag === 'Deduction' && d.hrmeD_AmountPercentFlag === 'Amount';
                });

                $scope.headdropdown = list;


            } else if ($scope.Employee.EarningDeduction == 'Deduction' && $scope.Employee.AmountPercentage == 'Percentage') {

                var list = $filter('filter')($scope.headdropdownOnLoad, function (d) {

                    return d.hrmeD_EarnDedFlag === 'Deduction' && d.hrmeD_AmountPercentFlag === 'Percentage';
                });

                $scope.headdropdown = list;

            }
          

        }



      




    }
})();