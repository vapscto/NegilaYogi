(function () {
    'use strict';
    angular
.module('app')
.controller('MonthEndReportController', MonthEndReportController)

    MonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function MonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("MonthEndReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

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
            });
        };

        $scope.onchageDropdownValue = function () {
            $scope.EmployeeDis = false;
            //$scope.GetEmployeeList();
        };

        var chart;
        $scope.employeeSalaryslipDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;

        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);
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
                $scope.today = new Date();
                //var data = $scope.Employee;
                var data =
                {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected
                };

                apiService.create("MonthEndReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {

                        $scope.selectedMonth = promise.hreS_Month;
                        $scope.showGrafh = true;
                        $scope.selectedYear = promise.hreS_Year;

                        //
                        $scope.workingEmployee = promise.workingEmployee;
                        $scope.leftEmployee = promise.leftEmployee;
                        $scope.newEmployee = promise.newEmployee;
                        $scope.salaryGenerated = promise.salaryGenerated;
                        $scope.salaryslipGenerated = promise.salaryslipGenerated;

                        //missing details of working employees
                        $scope.missingPhoto = promise.missingPhoto;
                        $scope.missingEmailId = promise.missingEmailId;
                        $scope.missingContactNumber = promise.missingContactNumber;

                        //missing details of left employees
                        $scope.missingPhotoleft = promise.missingPhotoleft;
                        $scope.missingEmailIdleft = promise.missingEmailIdleft;
                        $scope.missingContactNumberleft = promise.missingContactNumberleft;

                        //$scope.salaryslipsent = promise.salaryslipsent;
                        $scope.salaryslipsent = promise.emailcount[0].count;
                        //$scope.salaryslipsmssent = promise.salaryslipsmssent;
                        $scope.salaryslipsmssent = promise.smscount[0].count;

                        //missing details of new employees
                        $scope.missingPhotonew = promise.missingPhotonew;
                        $scope.missingEmailIdnew = promise.missingEmailIdnew;
                        $scope.missingContactNumbernew = promise.missingContactNumbernew;
                        //


                        //monthendDate
                        $scope.monthendDate = promise.monthendDate;

                        //financialYear
                        $scope.FinancialYear = "2017-2018";



                        chart = new CanvasJS.Chart("rangeBarChat",
                            {
                                width: 900,
                                height: 300,
                                axisX: {
                                    interval: 1
                                },
                                axisY2: {
                                    interval: 1

                                },
                                dataPointMaxWidth: 50,
                                data: [
                                    {
                                        type: "column",
                                        showInLegend: true,
                                        legendText: "Count",
                                        legendMarkerColor: "gold",
                                        dataPoints: [
                                            { x: 1, y: $scope.workingEmployee, label: "workingEmployee" },
                                            { x: 2, y: $scope.leftEmployee, label: "leftEmployee" },
                                            { x: 3, y: $scope.newEmployee, label: "newEmployee" },
                                            { x: 4, y: $scope.salaryGenerated, label: "salaryGenerated" },
                                            { x: 5, y: $scope.salaryslipGenerated, label: "salaryslipGenerated" }
                                        ]
                                    }
                                    //{
                                    //    type: "column",
                                    //    showInLegend: true,
                                    //    legendText: "PHOTO",
                                    //    color: "silver",
                                    //    dataPoints: [
                                    //    { x: 1, y: 0, label: "PHOTO" },
                                    //    { x: 2, y: 0, label: "PHOTO" },
                                    //    { x: 3, y: 0, label: "PHOTO" },
                                    //    { x: 4, y: 0, label: "PHOTO" },
                                    //    { x: 5, y: 0, label: "PHOTO" },
                                    //  //  { x: 6, y: 0, label: "SMS" }
                                    //    ]
                                    //},
                                    //{
                                    //    type: "column",
                                    //    showInLegend: true,
                                    //    legendText: "Email",
                                    //    color: "#DCA978",
                                    //    dataPoints: [
                                    //    { x: 1, y: 0, label: "workingEmployee" },
                                    //    { x: 2, y: 0, label: "leftEmployee" },
                                    //    { x: 3, y: 0, label: "newEmployee" },
                                    //    { x: 4, y: 0, label: "salaryGenerated" },
                                    //    { x: 5, y: 0, label: "salaryslipGenerated" },
                                    //   // { x: 6, y: 0, label: "ECS" }
                                    //    ]
                                    //}
                                ]

                            });

                        chart.render();

                        if (promise.institutionDetails != null) {
                            $scope.EmployeeDis = true;
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
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }

                    });
            }

        };
       

        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeSalaryslipDetails = [];
            $scope.submitted = false;
            $scope.EmployeeDis = false;
            $scope.search = "";

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.printToCart = function (Baldwin) {
            var base64Image = chart.canvas.toDataURL();
            // document.getElementById('rangeBarChat').style.display = 'none';
            document.getElementById('chartImage').src = base64Image;
            var innerContents = document.getElementById("tablegrp").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();

            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


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
        };

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
            });
            var data = {
                hrmgT_IdList: ids
            };
            apiService.create("EmployeeSalarySlipGeneration/get_depts", data).
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
            $scope.get_desig();
        };

        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
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
            apiService.create("EmployeeSalarySlipGeneration/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                });
        };

        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

                return itm.selected;
            });
        };

    }


})();