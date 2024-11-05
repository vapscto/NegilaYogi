(function () {
    'use strict';
    angular
        .module('app')
        .controller('EPFcontributionbbsRegisterController', EPFcontributionbbsRegisterController)

    EPFcontributionbbsRegisterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EPFcontributionbbsRegisterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("ESIReport/getalldetails", pageid).then(function (promise) {

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

            })
        }


        $scope.pfreport = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institution = {};
                $scope.pfreport = [];
                $scope.CurrentInstuteAddress = "";
                $scope.month = "";
                $scope.year = "";

                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);//
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
                // var data = $scope.Employee;



                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,

                }

                apiService.create("EPFcontributionRegister/getEmployeedetailsBySelection", data).
                    then(function (promise) {
                        $scope.EmployeeDis = true;
                        if (promise.pfreport !== null && promise.pfreport.length > 0) {

                            $scope.pfreport = promise.pfreport;


                            $scope.pfreport = promise.pfreport;

                            var totalgrp = 0;
                            var not_paid_list = [];
                            var temp = [];
                            var not_paid_list1 = [];
                            angular.forEach($scope.pfreport, function (cls) {

                                if (cls != null) {
                                    var intrr = Math.round(cls.pfAmount);
                                    $scope.totalgrp = intrr;
                                    cls.pfAmount = $scope.totalgrp;
                                    cls.hreS_EPF = Math.round(cls.hreS_EPF);
                                    cls.hreS_FPF = Math.round(cls.hreS_FPF);
                                    cls.hreS_Ac5 = Math.round(cls.hreS_Ac5);
                                    //angular.forEach($scope.totalgrp, function (cls) {
                                    //    not_paid_list.push(cls);
                                    //});

                                    cls.netsalary = Math.round(cls.pfAmount) + Math.round(cls.hreS_EPF) + Math.round(cls.hreS_FPF) + Math.round(cls.hreS_Ac5);

                                    not_paid_list.push(cls);
                                }
                                else {
                                    temp.push(cls);
                                }

                            });



                            $scope.pfreport = not_paid_list;



                        }

                        if (promise.hreS_Month != "") {
                            $scope.month = promise.hreS_Month;
                        }
                        if (promise.hreS_Year != "") {
                            $scope.year = promise.hreS_Year;
                        }

                        //Institution Details
                        if (promise.institutionDetails !== null && promise.institutionDetails.length > 0) {

                            $scope.institution = promise.institutionDetails[0];

                            var instuteAddress = "";
                            if ($scope.institution.mI_Address1 != null && $scope.institution.mI_Address1 != "") {

                                instuteAddress = $scope.institution.mI_Address1;

                            }
                            if ($scope.institution.mI_Address2 != null && $scope.institution.mI_Address2 != "") {

                                instuteAddress = instuteAddress + ',' + $scope.institution.mI_Address2;

                            }

                            if ($scope.institution.mI_Address3 != null && $scope.institution.mI_Address3 != "") {

                                instuteAddress = instuteAddress + ',' + $scope.institution.mI_Address3;

                            }

                            $scope.CurrentInstuteAddress = instuteAddress;
                        }



                    })
            }

        }


        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.pfreport = [];
            $scope.institution = {};
            $scope.month = "";
            $scope.year = "";
            $scope.CurrentInstuteAddress = "";
            $scope.submitted = false;

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

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

        $scope.getAmountofWagesTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.amountofWages;
                }
            }

            return total;
        }


        $scope.getpfAmountTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.pfAmount;
                }
            }

            return total;
        }
        $scope.gethreS_EPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_EPF;
                }
            }

            return total;
        }
        $scope.gethreS_FPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_FPF;
                }
            }

            return total;
        }


        //

        $scope.gethreS_Ac5 = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_Ac5;
                }
            }

            return total;
        }


        $scope.getnetsalary = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.netsalary;
                }
            }

            return total;
        }



        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //    var newWin = window.open();
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //    // $state.reload();
        //}

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
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

        $scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.designationselectedAll;
            angular.forEach($scope.designationdropdown, function (itm) {
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
            apiService.create("ESIReport/get_depts", data).
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
            apiService.create("ESIReport/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                })
        };



        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

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
            apiService.create("ESIReport/get_desig", data).
                then(function (promise) {
                    if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                        $scope.designationdropdown = promise.designationdropdown;
                        $scope.designationselectedAll = true;
                        $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
                    }
                })
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("table");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };
    }

})();