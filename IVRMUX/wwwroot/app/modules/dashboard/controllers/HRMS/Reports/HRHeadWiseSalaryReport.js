(function () {
    'use strict';
    angular
        .module('app')
        .controller('HRHeadWiseSalaryReportController', HRHeadWiseSalaryReportController)

    HRHeadWiseSalaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', 'Excel', '$timeout']
    function HRHeadWiseSalaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, Excel, $timeout) {
        $scope.Logo_Path = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            $scope.Logo_Path = ivrmcofigsettings[0].asC_Logo_Path ;          
        }

        $scope.hrmE_Id = 0;
        $scope.allind = "All";
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("CumulativeSalaryReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }                
                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);
                }

                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.dept = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;
                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    $scope.GetEmployeeByDesignation()
                }

               
                $scope.dearningdeductiondetails = [];
                if (promise.earningdeductiondetails !== null && promise.earningdeductiondetails.length > 0) {
                    $scope.earningdeductiondetails = promise.earningdeductiondetails;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                    $scope.monthselectedAll = true;
                    angular.forEach($scope.monthdropdown, function (itm) {
                        itm.selected = true
                    });

                }
                if (promise.configurationDetails != null) {
                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;
                }
            });
        };

      
      
        




        $scope.earningdeductionoption = function (items) {
            $scope.dearningdeductiondetails = [];
            if ($scope.EDOption == 'Earning') {
                angular.forEach($scope.earningdeductiondetails, function (itm) {
                    if (itm.hrmeD_EarnDedFlag == "Earning") {
                        $scope.dearningdeductiondetails.push(itm);
                        $scope.earningselectedAll = true;
                        itm.selected = true;
                       
                    }
               });
            }
            else if ($scope.EDOption == 'Deduction') {
                angular.forEach($scope.earningdeductiondetails, function (itm) {
                    if (itm.hrmeD_EarnDedFlag == "Deduction") {
                        $scope.dearningdeductiondetails.push(itm);
                        $scope.deductionselectedAll = true;
                        itm.selected = true                       
                    }
                });

            }
            else {
                $scope.dearningdeductiondetails = $scope.earningdeductiondetails
            }

        }

        $scope.getVolumeSumgirls = function (items) {
            return items
                .map(function (x) { return x.empGrossSal; })
                .reduce(function (a, b) { return a + b; });
        };

        $scope.addColumn = function (role, indexx, headertest) {
            angular.forEach(headertest, function (subscription, index) {
                if (indexx != index)
                    subscription.selected = false;
            });
            $scope.get_desig();
        };

        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                var product = $scope.employeeSalaryslipDetails[i];
                total += product.totalEmployees;
            }
            return Math.round(total);
        };

        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.EmployeeDis = false;
            $scope.submitted = true;
            $scope.tempdetails = [];
            if ($scope.myForm.$valid) {
                $scope.institutionDetails = {};
                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);
                    }
                });

                var departmentselected = [];
                var departmentselectedname = [];
                angular.forEach($scope.departmentdropdown, function (itm) {
                    if (itm.selected) {
                        departmentselected.push(itm.hrmD_Id);
                        $scope.deptnamedisplat = itm.hrmD_DepartmentName;
                        // $scope.deptname = $scope.departmentdropdown[0].hrmD_DepartmentName;
                    }
                });

                $scope.deptname = $scope.deptnamedisplat;

                var designationselected = [];
                angular.forEach($scope.designationdropdown, function (itm) {
                    if (itm.selected) {
                        designationselected.push(itm.hrmdeS_Id);
                    }

                });


                var monthselected = [];
                angular.forEach($scope.monthdropdown, function (itm) {
                    if (itm.selected) {
                        monthselected.push({ IVRM_Month_Id: itm.ivrM_Month_Id, IVRM_Month_Name: itm.ivrM_Month_Name });

                    }

                });

                var monthselected1 = [];
                angular.forEach($scope.monthdropdown1, function (itm) {
                    if (itm.selected) {
                        monthselected1.push({ IVRM_Month_Id: itm.ivrM_Month_Id, IVRM_Month_Name: itm.ivrM_Month_Name });

                    }

                });

               



                if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0 && monthselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

                var data = {
                    "HRME_Id": $scope.hrmE_Id,
                    "HRES_Year": $scope.Employee.hreS_Year,
                    monthselected: monthselected,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,
                    "comm": "0"
                };

                apiService.create("CumulativeSalaryReport/headwisereport", data).then(function (promise) {
                        

                        if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                           // $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails
                          
                            $scope.employeeSalaryslipDetails = [];

                            angular.forEach($scope.dearningdeductiondetails, function (itm) {
                                if (itm.selected == true) {
                                    angular.forEach(promise.employeeSalaryslipDetails, function (itm1) {
                                        if (itm.hrmeD_Name == itm1.HRMED_Name) {
                                            $scope.employeeSalaryslipDetails.push(itm1)
                                        }
                                    });
                                }
                               
                            });
                            $scope.Headwise_Amount = 0;
                            angular.forEach(promise.employeeSalaryslipDetails, function (itm1) {
                                $scope.Headwise_Amount = $scope.Headwise_Amount + itm1.Headwise_Amount;
                            });

                            
                            
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                    });
            }
        };

        //By group Type
        $scope.GetmonthAll = function (monthselectedAll) {
            var toggleStatus = monthselectedAll;
            angular.forEach($scope.monthdropdown, function (itm) {
                itm.selected = toggleStatus;
            });


        };


        $scope.GetearningAll = function (earningselectedAll) {
            var toggleStatus = earningselectedAll;
            angular.forEach($scope.dearningdeductiondetails, function (itm) {
                itm.selected = toggleStatus;
            });


        };

        $scope.Getearninglist = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.earningselectedAll = $scope.dearningdeductiondetails.every(function (itm) {
                return itm.selected;
            });

        };




        //$scope.GetearningAll = function (earningselectedAll) {
        //    var toggleStatus = earningselectedAll;
        //    angular.forEach($scope.deductiondetails, function (itm) {
        //        itm.selected = toggleStatus;
        //    });


        //};









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

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.monthselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });
            $scope.get_depts();
        };

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            $scope.departmentdropdown = [];
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            $scope.monthselectedAll = false;
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
            });
            var data = {
                hrmgT_IdList: ids
            };
            apiService.create("CumulativeSalaryReport/get_depts", data).
                then(function (promise) {
                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
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
            });

            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.monthselectedAll = toggleStatus;
            });
            $scope.get_desig();
        };

        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            $scope.monthselectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            //$scope.get_desig();
        };

        $scope.get_desig = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });
            var ids1 = [];
            angular.forEach($scope.departmentdropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids1.push(grp_t.hrmD_Id);
                }
            });
            var data = {
                hrmgT_IdList: ids,
                hrmD_IdList: ids1
            };
            apiService.create("CumulativeSalaryReport/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);


                    }
                });
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
        };
        $scope.GetmonthsAll = function (monthselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.monthselectedAll;
            angular.forEach($scope.monthdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.GetmonthsAll = function (monthselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.monthselectedAll;
            angular.forEach($scope.monthdropdown1, function (itm) {
                itm.selected = toggleStatus;
            });
        };






        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });

        };

        //By Month 
        $scope.Getmonthlist = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.monthselectedAll = $scope.monthdropdown.every(function (itm) {
                return itm.selected;
            });

        };






        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeSalaryslipDetails = [];
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
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        };

        //Total for per column
        $scope.TotalgrossEarning = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossEarning;
                }
            }
            return Math.round(total);
        };

        $scope.TotalgrossDeduction = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossDeduction;
                }
            }
            return Math.round(total);
        };

        $scope.TotalnetSalary = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails !== null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.netSalary;
                }
            }
            return Math.round(total);
        };

        $scope.All_Individual = function () {

            if ($scope.allind == 'Indi')
                $scope.disabledata = false;
            else
                $scope.disabledata = true;
            $scope.hrmE_Id = 0;

        }



        $scope.GetEmployeeByDesignation = function () {
            $scope.desgcheck = $scope.designationdropdown.every(function (options) {

                return options.selected;
            });

            $scope.get_employeenew();
        }
        $scope.get_employeenew = function () {

            var typeIds;

            for (var i = 0; i < $scope.groupTypedropdown.length; i++) {
                if ($scope.groupTypedropdown[i].selected == true) {

                    if (typeIds == undefined)
                        typeIds = $scope.groupTypedropdown[i].hrmgT_Id;
                    else
                        typeIds = typeIds + "," + $scope.groupTypedropdown[i].hrmgT_Id;
                }
            }
           








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
            for (var i = 0; i < $scope.designationdropdown.length; i++) {
                if ($scope.designationdropdown[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.designationdropdown[i].hrmdeS_Id;
                    else
                        groupidss = groupidss + "," + $scope.designationdropdown[i].hrmdeS_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledes": groupidss,
                    "multipletype": typeIds,
                    "multipledep": deptIds
                }
                apiService.create("EmployeeLogReport/get_employee", data).
                    then(function (promise) {

                        $scope.Employeelst = promise.fillemployee;
                        //if ($scope.Employeelst.length > 0) {
                        //   // $scope.hideempse = false;
                        //   // $scope.Employeelst[0].Selected = true;
                        //    $scope.hrmE_Id = $scope.Employeelst[0].hrmE_Id;
                        //}
                    })
            }
            else {
                $scope.Employeelst = "";
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

        $scope.exportToExcel = function (tableId) {
            //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var exportHref = Excel.tableToExcel(tableId, 'CumulativeSalary');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };
    }

})();