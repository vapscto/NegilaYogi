(function () {
    'use strict';
    angular
.module('app')
.controller('SalaryDetailsController', SalaryDetailsController)

    SalaryDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    //dashboard.controller("EmployeeDashboardController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
    function SalaryDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
     
            $scope.onLoadGetData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var id = 1;
            $scope.departmentdropdown = [];
            $scope.Designation_types = [];
            $scope.deptcheck = false;
            $scope.desgcheck = false;
            $scope.employeelst = [];
            $scope.stfmsg = "";

            apiService.getURI("SalaryDetails/Getdepartment", id).
     then(function (promise) {
         $scope.groupTypedropdown = promise.groupTypedropdown;
         //$scope.departmentdropdown = promise.departmentdropdown;
         $scope.leaveyeardropdown = promise.leaveyeardropdown;
         $scope.monthdropdown = promise.monthdropdown;

     })
        }

        $scope.all_checkgrptype = function () {
            debugger;
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_department();
        }

        $scope.get_department = function () {
            $scope.Designation_types = [];
            $scope.desgcheck = '';
            $scope.deptcheck = '';

            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (options) {
                return options.selected;
            });

            $scope.get_departmentnew();
           
        }


        $scope.get_departmentnew = function () {
           // $scope.groupTypeselectedAll = "";
            var groupTypeselected = [];
            angular.forEach($scope.groupTypedropdown, function (itm) {
                if (itm.selected) {
                    groupTypeselected.push(itm.hrmgT_Id);//
                }

            });

            if (groupTypeselected != undefined) {
                var data = {
                    //"multipledep": groupidss,
                    hrmgT_IdList: groupTypeselected,
                }
                apiService.create("SalaryDetails/get_department", data).
                    then(function (promise) {

                        $scope.departmentdropdown = promise.departmentdropdown;
                    })
            }
            else {
                $scope.Designation_types = "";
                $scope.employeedropdown = "";
                $scope.departmentdropdown = "";
            }
        }



        $scope.all_checkdep = function () {
            $scope.Designation_types = [];
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        }

        $scope.get_designation = function () {
            $scope.deptcheck = $scope.departmentdropdown.every(function (options) {
                return options.selected;
            });
            
            $scope.get_designationnew();
        }
        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.departmentdropdown.length; i++) {
                if ($scope.departmentdropdown[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.departmentdropdown[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.departmentdropdown[i].hrmD_Id;
                }
            }

            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss,
                }
                apiService.create("SalaryDetails/get_designation", data).
                then(function (promise) {
                    
                    $scope.Designation_types = promise.designationdropdown;
                })
            }
            else {
                $scope.Designation_types = "";
                $scope.employeedropdown = "";
            }
        }

        $scope.all_checkdesg = function () {
            
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        }

        //fill desg end
        //fill employee start
        $scope.get_employee = function () {
            $scope.desgcheck = $scope.Designation_types.every(function (options) {

                return options.selected;
            });
            
            $scope.get_employeenew();
        }
        $scope.get_employeenew = function () {
            $scope.stf = false;
            var deptIds;
            for (var i = 0; i < $scope.departmentdropdown.length; i++) {
                if ($scope.departmentdropdown[i].selected == true) {

                    if (deptIds == undefined)
                        deptIds = $scope.departmentdropdown[i].hrmD_Id;
                    else
                        deptIds = deptIds + "," + $scope.departmentdropdown[i].hrmD_Id;
                }
            }
            var groupidss;
            for (var i = 0; i < $scope.Designation_types.length; i++) {
                if ($scope.Designation_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Designation_types[i].hrmdeS_Id;
                    else
                        groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledes": groupidss,
                    "multipledep": deptIds
                }
                apiService.create("SalaryDetails/get_employee", data).
                then(function (promise) {
                    
                    $scope.employeelst = promise.stafflist;                 
                })
            }
            else {
                $scope.employeelst = "";
            }
        }






        //GetEmployeeDetailsByLeaveYearAndMonth
        $scope.submitted = false;
     
        $scope.newgrd = false;
        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.empdetails = {};
        $scope.submitted = false;
        $scope.GenerateSalarySlip = function () {
            $scope.newgrd = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            $scope.esary = [];
            $scope.dsary = [];
            //  $scope.items = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.institutionDetails = {};
                $scope.empdetails = {};
                $scope.EmployeeDis = false;
                if ($scope.EmployeeDis) {
                    $scope.EmployeeDis = false;
                }
                $scope.earningDetails = [];
                $scope.deductionDetails = [];
                $scope.esary = [];
                $scope.dsary = [];
                $scope.items = [];
              
                $scope.employeeSalaryslipDetails = [];
                var data = {};
                if ($scope.type==2) {
                    $scope.Employee.hrmE_Id = $scope.Employee1.hrmE_Id
                    data = {
                        "HRME_Id": $scope.Employee.hrmE_Id,
                        "HRES_Year": $scope.Employee.hreS_Year,
                        "HRES_Month": $scope.Employee.hreS_Month,
                        "serchtype": $scope.type,

                    }
                }
                else {

                    var empidselected = [];
                    angular.forEach($scope.employeelst, function (dd) {
                        
                        empidselected.push(dd.hrmE_Id);//
                       

                    });

                    data = {
                        "HRES_Year": $scope.Employee.hreS_Year,
                        "HRES_Month": $scope.Employee.hreS_Month,
                        "serchtype": $scope.type,
                        empids: empidselected
                    }
                }
               
                
                apiService.create("SalaryDetails/GenerateEmployeeSalarySlip", data).
                            then(function (promise) {
                                
                                //Earning Deduction Details
                                if ($scope.type == 2) {
                                    if (promise.employeeSalaryslipDetails != null && promise.employeeSalaryslipDetails.length != 0) {

                                        $scope.EmployeeDis = true;

                                        var items = promise.employeeSalaryslipDetails;
                                        console.log(items);
                                        var getPortion = function (label) {
                                            var sum = 0;
                                            var out = items.filter(function (x) {
                                                var match = x.HRMED_EarnDedFlag == label;
                                                if (match)
                                                    sum += x.Amount;
                                                return match;
                                            });
                                            return { out, sum }
                                        };

                                        var es = getPortion('Earning');
                                        var ds = getPortion('Deduction');
                                        $scope.esary = es;
                                        $scope.dsary = ds;
                                        if (es.out.length >= ds.out.length) {
                                            for (var item of ds.out)
                                                es.out[ds.out.indexOf(item)].ds = item;
                                            $scope.items = es.out;
                                        } else {

                                            for (var item of es.out)
                                                ds.out[es.out.indexOf(item)].ds = item;
                                            $scope.items = ds.out;
                                        }

                                        var net = 0;
                                        var ern = 0;
                                        var ded = 0;
                                        ern = es.sum;
                                        ded = ds.sum;
                                        net = ern - ded;
                                        $scope.sall = [];
                                        $scope.totals = [ern, ded, net];
                                        //alert($scope.totals)
                                    }
                                    else {
                                        swal("no record found");
                                    }
                                }
                                else {
                                    if (promise.employeeSalaryslipDetails != null && promise.employeeSalaryslipDetails.length != 0) {
                                        $scope.newgrd = true;
                                        $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;
                                    }
                                    else {
                                        $scope.newgrd = false;
                                        swal("No Record Found");
                                    }
                                }
                              
                            })
            }
        }


        //Clear data
       
        $scope.cleardata = function () {            
            $scope.Employee = {};
            angular.forEach($scope.departmentdropdown, function (user) {
                user.selected = false;
            })
           
            $scope.employeelst = [];
            $scope.desgcheck = false;
            $scope.EmployeeDis = false;
            $scope.Designation_types = [];
            $state.reload();
            $scope.onLoadGetData();
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            
           // $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
                //|| field.$dirty;
        };

        //

    }

})();