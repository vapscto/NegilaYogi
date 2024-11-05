(function () {
    'use strict';
    angular
.module('app')
.controller('massUpdationController', massUpdationController)
    massUpdationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function massUpdationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.reverse = true;



        $scope.Employee = {};

      //  $scope.Employee.EarningDeduction = 'All';
     //   $scope.Employee.AmountPercentage = 'AllAP';


        // sort table data
        $scope.sortTableData = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("massUpdation/getalldetails", pageid).then(function (promise) {


                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //    $scope.employeeSelectedAll = true;
                //    $scope.GetEmployeeAll($scope.employeeSelectedAll);
                //}

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

                //type dropdown
                if (promise.eardettypelist !== null && promise.eardettypelist.length > 0) {
                    $scope.eardettypeDropdown = promise.eardettypelist;
                }

                //head dropdown
                if (promise.headdropdown !== null && promise.headdropdown.length > 0) {

                    $scope.headdropdownOnLoad = promise.headdropdown;

                    $scope.headdropdown = $scope.headdropdownOnLoad;
                }

               
            })
        }

        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SaveEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                var data = {};
                var employeeselected = [];

                angular.forEach($scope.employeedropdown, function (itm) {
                    if (itm.selected) {
                        employeeselected.push(itm.hrmE_Id);
                    }
                });

                if (employeeselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

               
                $scope.Employee.employeeselected = employeeselected;

                data = $scope.Employee;

                apiService.create("massUpdation/getEmployeedetailsBySelection", data).
                            then(function (promise) {

                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg == "false") {
                                        swal("Record Not Updated..", 'Fail');
                                    }
                                    else if (promise.retrunMsg == "Add") {
                                        swal("Record Added/Updated Successfully..");
                                    }
                                    else if (promise.retrunMsg == "Remove") {
                                        swal("Record Removed Successfully..");
                                    }

                                    $scope.cleardata();
                                }
                            })
            }
        }



        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
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
            apiService.create("massUpdation/get_depts", data).
                        then(function (promise) {

                            if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                                $scope.departmentdropdown = promise.departmentdropdown;
                                $scope.departmentselectedAll = true;
                                $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                            }
                        })
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
            apiService.create("massUpdation/get_desig", data).
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



        //Clear data
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.submitted = false;
            $scope.employeedropdown = [];
            //$scope.Employee.EarningDeduction = 'All';
           // $scope.Employee.AmountPercentage = 'AllAP';
            $scope.AmountPercentDis = true;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
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

      
        $scope.GetEmployeeListByFilterSelection = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.employeedropdown = [];
                var groupTypeselected = [];
                var departmentselected = [];
                var designationselected = [];
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

                if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

                $scope.Employee.groupTypeselected = groupTypeselected;
                $scope.Employee.departmentselected = departmentselected;
                $scope.Employee.designationselected = designationselected;
                $scope.Employee.Type = "";

                data = $scope.Employee;

                apiService.create("massUpdation/FilterEmployeeData", data).
                               then(function (promise) {

                                   if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {

                                       $scope.currentPage = 1;
                                       $scope.itemsPerPage = 5;
                                       $scope.reverse = true;

                                       $scope.employeedropdown = promise.employeedropdown;

                                      // $scope.employeeSelectedAll = true;
                                       //$scope.GetEmployeeAll($scope.employeeSelectedAll);

                                       $scope.EmployeeDis = true;
                                   } else {
                                       swal('No Record Found to display..');
                                       $scope.employeedropdown =[];
                                   }
                               })

        }


        $scope.GetHeadListByFilterSelection = function (data) {

            //if ($scope.EmployeeDis) {
            //    $scope.EmployeeDis = false;
            //}

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

        $scope.GetEmpListBySelectedHead = function () {

            if ($scope.Employee.hrmeD_Id != "" && $scope.Employee.hrmeD_Id != undefined) {

                angular.forEach($scope.employeedropdown, function (itm) {
                    itm.selected = false;

                });

                $scope.Employee.Type = "Head";
                var data = $scope.Employee;

                apiService.create("massUpdation/FilterEmployeeData", data).
                               then(function (promise) {
                                   $scope.Employee.Type = "";
                                   $scope.currentPage = 1;
                                   $scope.itemsPerPage = 5;
                                   $scope.reverse = true;

                                   if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {

                                       var selectedemp = promise.employeedropdown;

                                       angular.forEach($scope.employeedropdown, function (itm) {

                                           angular.forEach(selectedemp, function (itmIn) {

                                               if (itmIn.hrmE_Id == itm.hrmE_Id) {
                                                   itm.selected = true;
                                               } 

                                           })
                                       });

                                   }
                                  
                               })

            }

           
        }

    }
})();