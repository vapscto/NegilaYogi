(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeContributionReportController', EmployeeContributionReportController)
    EmployeeContributionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmployeeContributionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.Employee = {};

        //setToDate
        $scope.setToDate = function (FromDate) {

            $scope.Employee.ToDate = FromDate;
            $scope.minDateTo = new Date(
                $scope.Employee.ToDate.getFullYear(),
                $scope.Employee.ToDate.getMonth(),
                $scope.Employee.ToDate.getDate());


            $scope.Employee.ToDate = "";
        }

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeContributionReport/getalldetails", pageid).then(function (promise) {
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

                //earning dropdown
                if (promise.earningdropdown !== null && promise.earningdropdown.length > 0) {
                    $scope.earningdropdown = promise.earningdropdown;
                }
                //deduction dropdown
                if (promise.detectiondropdown !== null && promise.detectiondropdown.length > 0) {
                    $scope.detectiondropdown = promise.detectiondropdown;
                }


                //Set From date and To date
                $scope.Employee.FromDate = new Date();

                $scope.Employee.ToDate = $scope.Employee.FromDate;
                $scope.minDateTo = new Date(
                    $scope.Employee.ToDate.getFullYear(),
                    $scope.Employee.ToDate.getMonth(),
                    $scope.Employee.ToDate.getDate());

            })
        }

        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.institutionDetails = {};
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institutionDetails = {};
                $scope.totalselectedHeadAmount = 0;

                var groupTypeselected = [];
                var departmentselected = [];
                var designationselected = [];
                var employeeTypeselected = [];
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
                $scope.Employee.employeeTypeselected = employeeTypeselected;

                $scope.Employee.FromDate = $filter('date')($scope.Employee.FromDate, "yyyy-MM-dd");
                $scope.Employee.ToDate = $filter('date')($scope.Employee.ToDate, "yyyy-MM-dd");

                data = $scope.Employee;

                apiService.create("EmployeeContributionReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {


                        if (promise.institutionDetails != null) {
                            $scope.institutionDetails = promise.institutionDetails;

                            //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);

                            var instuteAddress = "";
                            if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {

                                instuteAddress = $scope.institutionDetails.mI_Address1;

                            }
                            if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {

                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;

                            }

                            if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {

                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;

                            }

                            $scope.CurrentInstuteAddress = instuteAddress;
                        }

                        $scope.headerlist = promise.headerlist;
                        if ($scope.headerlist == null) {
                            $scope.multiinfo = false;
                            $scope.oneinfo = true;
                        }
                        else {
                            $scope.multiinfo = true;
                            $scope.oneinfo = false;
                        }

                        if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                            $scope.EmployeeDis = true;
                            $scope.employeeDetails = promise.employeeDetails;
                            var totlhead = 0;
                            angular.forEach($scope.employeeDetails, function (itm) {
                                totlhead = totlhead + parseFloat(itm.selectedHeadAmount);
                            });

                            $scope.totalselectedHeadAmount = totlhead;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                    })
            }
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
            if ($scope.designationselectedAll == true && $scope.departmentselectedAll == true && $scope.groupTypeselectedAll == true) {
                $scope.GetEmployeeListByFilterSelection();
            }
            else {
                $scope.Employee.EarningDeduction = "";
                $scope.Employee.MonthBetweenDates = "";
                $scope.Employee.FormatType = "";
                $scope.employeedropdown = [];

            }
        }

        //single
        //By group Type
        //123
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
            if ($scope.designationselectedAll == true && $scope.departmentselectedAll == true && $scope.groupTypeselectedAll == true) {
                $scope.GetEmployeeListByFilterSelection();
            }
            else {
                $scope.Employee.EarningDeduction = "";
                $scope.Employee.MonthBetweenDates = "";
                $scope.Employee.FormatType = "";
                $scope.employeedropdown = [];

            }
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
            apiService.create("EmployeeContributionReport/get_depts", data).
                then(function (promise) {

                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                })
        };



        //By Department
        //123
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
            if ($scope.designationselectedAll == true && $scope.departmentselectedAll == true && $scope.groupTypeselectedAll == true) {
                $scope.GetEmployeeListByFilterSelection();
            }
            else {
                $scope.Employee.EarningDeduction = "";
                $scope.Employee.MonthBetweenDates = "";
                $scope.Employee.FormatType = "";
                $scope.employeedropdown = [];

            }
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
            apiService.create("EmployeeContributionReport/get_desig", data).
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
            if ($scope.designationselectedAll == true && $scope.departmentselectedAll == true && $scope.groupTypeselectedAll == true) {
                $scope.GetEmployeeListByFilterSelection();
            }
            else {
                $scope.Employee.EarningDeduction = "";
                $scope.Employee.MonthBetweenDates = "";
                $scope.Employee.FormatType = "";
                $scope.employeedropdown = [];

            }

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
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.submitted = false;
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

        $scope.setEarningHeadName = function (hrmeD_Id) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            if (hrmeD_Id != undefined && hrmeD_Id != null && hrmeD_Id != "") {

                //earningdropdown
                var hrmeD_Name = $filter('filter')($scope.earningdropdown, function (d) {
                    return d.hrmeD_Id === parseInt(hrmeD_Id);
                })[0].hrmeD_Name;

                $scope.selectedheadName = hrmeD_Name;
            }

        }

        $scope.setDeductionHeadName = function (hrmeD_Id) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            if (hrmeD_Id != undefined && hrmeD_Id != null && hrmeD_Id != "") {

                //earningdropdown
                var hrmeD_Name = $filter('filter')($scope.detectiondropdown, function (d) {
                    return d.hrmeD_Id === parseInt(hrmeD_Id);
                })[0].hrmeD_Name;

                $scope.selectedheadName = hrmeD_Name;
            }

        }

        $scope.GetEmployeeListByFilterSelection = function () {
            $scope.employeedropdown = [];
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            if ($scope.Employee.FormatType == 'Format1') {
                if ($scope.Employee.MonthBetweenDates == null) {
                    return;
                }
                $scope.totalselectedHeadAmount = 0;
                var groupTypeselected = [];
                var departmentselected = [];
                var designationselected = [];
                var employeeTypeselected = [];
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
                $scope.Employee.employeeTypeselected = employeeTypeselected;
                $scope.Employee.FromDate = $filter('date')($scope.Employee.FromDate, "yyyy-MM-dd");
                $scope.Employee.ToDate = $filter('date')($scope.Employee.ToDate, "yyyy-MM-dd");
                data = $scope.Employee;
                apiService.create("EmployeeContributionReport/FilterEmployeeData", data).
                    then(function (promise) {
                        if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                            $scope.employeedropdown = promise.employeedropdown;
                        } else {
                            swal('No Record Found to display..');
                            $scope.Employee.hrmE_Id = "";
                        }
                    })
            }
        }
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
        };

        $scope.exportToExcelone = function () {
            var divToPrint = document.getElementById("divone");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };

        $scope.exportToExceltwo = function () {
            var divToPrint = document.getElementById("divtwo");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };

    }
})();