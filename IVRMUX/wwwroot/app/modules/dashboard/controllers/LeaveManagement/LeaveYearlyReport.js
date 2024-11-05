

(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveYearlyReportController', LeaveYearlyReportController)
    LeaveYearlyReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache']
    function LeaveYearlyReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {
        $scope.editEmployee = {};

        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.albumNameArraycolumn = [];
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.showdepartment = true;
        $scope.showdesignation = true;

        $scope.loadData = function () {

            var id = 2;
            // $scope.all_check();
            apiService.getURI("LeaveYearlyReport/getleavereport", id).
                then(function (promise) {


                    $scope.actvmonth = true;
                    $scope.actvyear = true;
                    $scope.actvfromdate = true;
                    $scope.actvtodate = true;

                    $scope.staff_types = promise.stf_types;
                    $scope.Department_types = promise.department_types;
                    $scope.Designation_types = promise.designation_types;
                    $scope.academic_year_name = promise.academic_year_name;
                    $scope.credit_month = promise.credit_month;
                    $scope.get_year = promise.get_year;
                    $scope.get_emp = promise.get_emp;


                    $scope.temp_department_arr = promise.department_types;
                    $scope.temp_designation_arr = promise.designation_types;
                    $scope.temp_employee_arr = promise.get_emp;

                    $scope.count = $scope.Department_types.length;
                    $scope.count1 = $scope.Designation_types.length;
                    $scope.countl = $scope.get_emp.length;



                    //if ($scope.count == 0) {
                    //    swal("Data not Found !!");
                    //    $scope.ckdept = false;
                    //}

                })
        };

        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.class = toggleStatus;
            });
            $scope.get_departments();
        }
        $scope.all_check_dept = function () {

            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.class1 = toggleStatus;
            });
            $scope.get_designation();
        }
        $scope.all_check_desg = function () {

            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.class2 = toggleStatus;
            });
            $scope.get_Employees();
        }
        $scope.all_check_emp = function () {

            var toggleStatus = $scope.empcheck;
            angular.forEach($scope.get_emp, function (itm) {
                itm.emplname = toggleStatus;
            });

        }

        $scope.togchkbx = function () {

            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.class;
            });
            $scope.get_departments();
        }

        $scope.deptchkbx = function () {

            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.class1;
            });
            $scope.get_designation();
        }
        $scope.desgchkbx = function () {

            $scope.desgcheck = $scope.Designation_types.every(function (options) {
                return options.class2;
            });
            $scope.get_Employees();

        }

        $scope.emp_namechkbx = function () {

            $scope.empcheck = $scope.get_emp.every(function (options) {
                return options.emplname;
            });
        }
        //------------------------------Enable and Disable
        $scope.checkleave = function () {

            if ($scope.ckvalleave == 1) {
                $scope.actleave = false;
            }
            else {
                $scope.actleave = true;
            }
        }

        $scope.showmonth = function () {

            $scope.myDate_from = "";
            $scope.myDate_to = "";
            if ($scope.monthcheck == 1) {
                $scope.actvmonth = false;
            }
            else {
                $scope.actvmonth = true;
            }
        }
        $scope.showyear = function () {

            if ($scope.checkboxval == 1) {
                $scope.actvyear = false;
            }
            else {
                $scope.actvyear = true;
            }
        }
        $scope.showfromdate = function () {

            $scope.classmonth = "";
            $scope.yearname = "";
            if ($scope.checkboxval_from == 1) {
                $scope.actvfromdate = false;
            }
            else {
                $scope.actvfromdate = true;
            }
        }
        $scope.showtodate = function () {

            if ($scope.checkboxval_to == 1) {
                $scope.actvtodate = false;
            }
            else {
                $scope.actvtodate = true;
            }
        }

        $scope.compdate = function () {


            var a = new Date($scope.myDate_from).toDateString();
            var b = new Date($scope.myDate_to).toDateString();

            if (a <= b) {
                return true;
            }
            else {
                swal("To Date must be greater then From selected Date !!!!");
            }
        }
        //$scope.count = 0;

        $scope.get_departments = function () {

            $scope.selectedemptypes = [];
            angular.forEach($scope.staff_types, function (role) {
                if (role.class) $scope.selectedemptypes.push(role);
            })
            if ($scope.selectedemptypes.length != 0) {
                var data = {
                    emptypes: $scope.selectedemptypes,
                }
                apiService.create("LeaveYearlyReport/get_departments", data).
                    then(function (promise) {


                        $scope.Department_types = promise.department_types;
                        $scope.count = $scope.Department_types.length;

                        if ($scope.count == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Department Are Mapped with Selected Group Type !!!!");
                        }
                    })
            }
            else if ($scope.selectedemptypes.length == 0) {
                $scope.Department_types = $scope.temp_department_arr;
                $scope.count = $scope.Department_types.length;
            }
        }

        $scope.get_designation = function () {

            $scope.selectedemptypes = [];
            angular.forEach($scope.Department_types, function (role) {
                if (role.class1) $scope.selectedemptypes.push(role);
            })
            if ($scope.selectedemptypes.length != 0) {
                var data = {
                    emptypes: $scope.selectedemptypes,
                }
                apiService.create("LeaveYearlyReport/get_designation", data).
                    then(function (promise) {
                        $scope.Designation_types = promise.designation_types;
                        $scope.count1 = $scope.Designation_types.length;
                        if ($scope.count1 == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Designation Are Mapped with Selected Department !!!!");
                            //return false;                            
                        }
                    })
            }
            else if ($scope.selectedemptypes.length == 0) {
                $scope.Designation_types = $scope.temp_designation_arr;
                $scope.count1 = $scope.Designation_types.length;
            }
        }

        //  $scope.countl = 0;

        $scope.get_Employees = function () {

            $scope.selectedemptypes = [];
            angular.forEach($scope.Designation_types, function (role) {
                if (role.class2) $scope.selectedemptypes.push(role);
            })
            if ($scope.selectedemptypes.length != 0) {
                var data = {
                    emptypes: $scope.selectedemptypes,
                }
                apiService.create("LeaveYearlyReport/get_Employees", data).
                    then(function (promise) {


                        $scope.get_emp = promise.get_emp;
                        $scope.countl = $scope.get_emp.length;

                        if ($scope.countl == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Employee Mapped with Selected Designation !!!!");
                        }
                    })
            }
            else if ($scope.selectedemptypes.length == 0) {
                $scope.get_emp = $scope.temp_employee_arr;
                $scope.countl = $scope.get_emp.length;
            }
        }



        $scope.gridReport = {
            enableFiltering: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'hrmE_Id', displayName: 'Employee ID' },
                { name: 'hrmgT_EmployeeGroupType', displayName: 'Group Type' },
                { name: 'hrmD_DepartmentName', displayName: 'Department' },
                { name: 'hrmdeS_DesignationName', displayName: 'Designation' },
                { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name' },
                { name: 'hrmL_LeaveName', displayName: 'Leave Name' },


                { name: 'hrmlY_FromDate', displayName: 'From Date' },
                { name: 'hrmlY_ToDate', displayName: 'To Date' },
            ]
        };

        $scope.get_report = function () {

            var splitcolumn = [];
            if ($scope.myForm.$valid) {
                $scope.selectedemptypes = [];
                angular.forEach($scope.get_emp, function (itm1) {
                    if (itm1.emplname) $scope.selectedemptypes.push({ HRME_Id: itm1.hrmE_Id });
                })

                var fromdate = $scope.hrmlY_Id + "-01-01";
                var todate = $scope.hrmlY_Id +  "-12-31";
                //var data;
                var data = {
                    "hrmlY_Id": $scope.hrmlY_Id,
                    "fromdate": fromdate,
                    "todate": todate,
                    "multihrme_id": $scope.selectedemptypes
                }

                apiService.create("LeaveYearlyReport/get_report", data).
                    then(function (promise) {
                        $scope.EmployeeDis = true;
                        $scope.activityIds = promise.activityIds;
                        $scope.splitcolumn = promise.columnnames.split(',');
                        $scope.columnnames = $scope.splitcolumn;
                        $scope.tableone = promise.filldata;
                        angular.forEach($scope.activityIds, function (value, key) {
                            var fdate = value.hreltD_FromDate.split('T');
                            value.hreltD_FromDate = fdate[0];
                            var tdate = value.hreltD_ToDate.split('T');
                            value.hreltD_ToDate = tdate[0];
                        });
                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'LeaveReport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        }


    }
})();