(function () {
    'use strict';
    angular
.module('app')
        .controller('ESIBMCReportController', ESIBMCReportController)

    ESIBMCReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function ESIBMCReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.Employee = {};
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


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.institutionDetails = {};
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institutionDetails = {};
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
                data = $scope.Employee;

                apiService.create("ESIReport/getEmployeedetailsBySelection", data).
                            then(function (promise) {

                                if (promise.institutionDetails != null) {
                                    $scope.institutionDetails = promise.institutionDetails;
                                    $scope.configurationDetails = promise.configurationDetails;
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

                                var code = "";
                                if ($scope.configurationDetails.hrC_ECodePrefix != null && $scope.configurationDetails.hrC_ECodePrefix != "") {

                                    code = $scope.configurationDetails.hrC_ECodePrefix;

                                }

                                $scope.HRES_Year = promise.hreS_Year;

                                //get DOI
                                $scope.HRES_Month = promise.hreS_Month;
                                if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                                    $scope.EmployeeDis = true;







                                    $scope.employeeDetails = promise.employeeDetails;

                                    var totalgrp = 0;
                                    var not_paid_list = [];
                                    var temp = [];
                                    angular.forEach($scope.employeeDetails, function (cls) {

                                        if (cls != null) {
                                            not_paid_list.push(cls);
                                            //  $scope.totalgrp += parseInt(cls.netSalary);
                                        }
                                        else {
                                            temp.push(cls);
                                        }

                                    })



                                    $scope.employeeDetails = not_paid_list;
                                 
                                    console.log($scope.employeeDetails);

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

        $scope.onChangeMonth = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.onChangeYear = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
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


        $scope.TotalbasicAmount = function () {
            var total = 0;
            if ($scope.employeeDetails != null) {
                for (var i = 0; i < $scope.employeeDetails.length; i++) {


                    var product = $scope.employeeDetails[i];
                    total += product.basicAmount;
                }
            }

            return total;
        }

        $scope.TotalemployeeContibution = function () {
            var total = 0;
            if ($scope.employeeDetails != null) {
                for (var i = 0; i < $scope.employeeDetails.length; i++) {


                    var product = $scope.employeeDetails[i];
                    total += product.employeeContibution;
                }
            }

            return total;
        }

        $scope.TotalemployerContibution = function () {
            var total = 0;
            if ($scope.employeeDetails != null) {
                for (var i = 0; i < $scope.employeeDetails.length; i++) {


                    var product = $scope.employeeDetails[i];
                    total += product.employerContibution;
                }
            }

            return total;
        }


        $scope.TotalAmount = function () {
            var total = 0;
            if ($scope.employeeDetails != null) {
                for (var i = 0; i < $scope.employeeDetails.length; i++) {


                    var product = $scope.employeeDetails[i];
                    total += product.totalAmount;
                }
            }

            return total;
        }



        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

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




    }

})();