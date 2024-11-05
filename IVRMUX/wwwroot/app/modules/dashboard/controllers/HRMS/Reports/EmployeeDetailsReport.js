(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeDetailsReportController', EmployeeDetailsReportController)

    EmployeeDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache']
    function EmployeeDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {
        //object

        //Employeee
        // $scope.currentPage = 1;
        // $scope.itemsPerPage = 10;

        $scope.Employee = {};

        $scope.Employee.FormatType = "Format1";
        $scope.Employee.Working = true;
        $scope.Employee.Left = true;
        $scope.Employee.AllOrIndividual = "All";
        $scope.Employee.DOBJL = "DOB";




        //header list

        $scope.headerdropdown = [{ id: 'HRME_EmployeeCode', selected: true, value: 'Employee Code' },
        { id: 'HRME_EmployeeFirstName', selected: true, value: 'Employee Name' },
        { id: 'HRME_FatherName', selected: true, value: 'Father Name' },
        { id: 'HRME_MobileNo', selected: true, value: 'Mobile No.' },
        { id: 'HRME_EmailId', selected: true, value: 'Email Id' },
        { id: 'HRME_DOB', selected: true, value: 'Date Of Birth' },
            { id: 'EmergencyContect', selected: true, value: 'Emergency Contact Number ' },
            { id: 'DateofConfromation', selected: true, value: 'Date Of confirmation' },
            { id: 'HRMET_Id', selected: true, value: 'Employee Type' },
            { id: 'HRMGT_Id', selected: true, value: 'Group Type' },
            { id: 'HRMD_Id', selected: true, value: 'Department' },
            { id: 'HRMDES_Id', selected: true, value: 'Designation' },
            { id: 'HRMG_Id', selected: true, value: 'Grade' },
            { id: 'IVRMMG_Id', selected: true, value: 'Gender' },
            { id: 'IVRMMMS_Id', selected: true, value: 'Marital Status' },
            { id: 'ReligionId', selected: true, value: 'Religion' },
            { id: 'CasteId', selected: true, value: 'Caste' },
            { id: 'HRME_IdentificationMark', selected: true, value: 'Identification Mark' },
            { id: 'HRME_SubjectsTaught', selected: true, value: 'Subjects Taught' },
            { id: 'HRME_BiometricCode', selected: true, value: 'Biometric Code' },
            { id: 'HRME_PerStreet', selected: true, value: 'Permanent Address' },
            { id: 'HRME_LocStreet', selected: true, value: 'Communication Address' },
        //{ id: 'IVRMMMS_Id', selected: true, value: 'Marital Status' },
        //{ id: 'IVRMMG_Id', selected: true, value: 'Sex' },

        //{ id: 'ReligionId', selected: true, value: 'Religion' },
        //{ id: 'CasteId', selected: true, value: 'Caste' },
        //{ id: 'CasteCategoryId', selected: true, value: 'Caste Category' },
        { id: 'HRME_DOJ', selected: true, value: 'Date Of Join' },
        { id: 'HRME_DOL', selected: true, value: 'Date Of Left' },
        { id: 'HRME_ExpectedRetirementDate', selected: true, value: 'Expected Retirement Date' },
        { id: 'HRME_BloodGroup', selected: true, value: 'Blood Group' },
        { id: 'HRME_PANCardNo', selected: true, value: 'PAN Card No.' },
        { id: 'HRME_AadharCardNo', selected: true, value: 'Aadhar Card No.' },
        //{ id: 'HRME_QualificationName', selected: true, value: 'Qualification Name.' },

        //{ id: 'HRMEB_AccountNo', selected: true, value: 'Account No.' },
        { id: 'HREED_Amount', selected: true, value: 'Basic Amount' }
            //{ id: 'HRME_QualificationName', selected: true, value: 'Qualification Name.' }
            //{ id: 'HRMEQ_CollegeName', selected: true, value: 'College Name' },
            //{ id: 'HRMC_QulaificationName', selected: true, value: 'Course Name' }

        ]

        $scope.headerselectedAll = true;



        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeDetailsReport/getalldetails", pageid).then(function (promise) {


                //if (promise.headerdropdown !== null && promise.headerdropdown.length > 0) {

                //    var headerdropdown = Object.keys(promise.headerdropdown[0]);

                //    angular.forEach(headerdropdown, function (key, value) {
                //        $scope.headerdropdown.push({ id: key, selected: true, value: key });

                //    });

                //    $scope.headerselectedAll = true;
                //    $scope.GetEmployeeByheaderAll($scope.headerselectedAll);


                //}

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

                if (promise.employeeTypedropdown !== null && promise.employeeTypedropdown.length > 0) {
                    $scope.employeeTypedropdown = promise.employeeTypedropdown;

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


                //Set From date and To date
                $scope.Employee.FromDate = new Date();

                $scope.Employee.ToDate = $scope.Employee.FromDate;
                $scope.minDateTo = new Date(
                    $scope.Employee.ToDate.getFullYear(),
                    $scope.Employee.ToDate.getMonth(),
                    $scope.Employee.ToDate.getDate());


            })
        }


        //setToDate
        $scope.setToDate = function (FromDate) {

            $scope.Employee.ToDate = FromDate;
            $scope.minDateTo = new Date(
                $scope.Employee.ToDate.getFullYear(),
                $scope.Employee.ToDate.getMonth(),
                $scope.Employee.ToDate.getDate());


            $scope.Employee.ToDate = "";

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
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
                var headerselected = [];
                var groupTypeselected = [];
                var departmentselected = [];
                var designationselected = [];
                var employeeTypeselected = [];
                var data = {};

                if ($scope.Employee.FormatType === 'Format1') {



                }
                else if ($scope.Employee.FormatType === 'Format2') {

                    angular.forEach($scope.groupTypedropdown, function (itm) {
                        if (itm.selected) {
                            groupTypeselected.push(itm.hrmgT_Id);
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
                }


                angular.forEach($scope.headerdropdown, function (itm) {
                    if (itm.selected) {
                        headerselected.push({
                            columnID: itm.id,
                            columnName: itm.value
                        });
                        //
                    }

                });
                if (headerselected.length == 0) {
                    swal('Kindly select Headers To display');
                    return;
                }

                $scope.Employee.headerselected = headerselected;
                $scope.Employee.groupTypeselected = groupTypeselected;
                $scope.Employee.departmentselected = departmentselected;
                $scope.Employee.designationselected = designationselected;
                $scope.Employee.employeeTypeselected = employeeTypeselected;

                $scope.Employee.FromDate = $filter('date')($scope.Employee.FromDate, "yyyy-MM-dd");
                $scope.Employee.ToDate = $filter('date')($scope.Employee.ToDate, "yyyy-MM-dd");

                data = $scope.Employee;

                apiService.create("EmployeeDetailsReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {


                        if (promise.employeeDetailsfromDatabase !== null && promise.employeeDetailsfromDatabase.length > 0) {
                            $scope.EmployeeDis = true;


                            //$scope.rows = promise.employeeDetailsfromDatabase;

                            // $scope.cols = Object.keys($scope.rows[0]);

                            $scope.rows = promise.employeeDetailsfromDatabase;
                            $scope.columnsTest = promise.headerselected;
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }


                        //institution

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



                    })
            }

        }



        //By Header
        $scope.GetEmployeeByheaderAll = function (headerselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.headerselectedAll;
            angular.forEach($scope.headerdropdown, function (itm) {
                itm.selected = toggleStatus;

            });
        }


        //single
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
            apiService.create("EmployeeDetailsReport/get_depts", data).
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
            apiService.create("EmployeeDetailsReport/get_desig", data).
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


        //Clear data


        $scope.cleardata = function () {
            $scope.Employee = {};

            $scope.Employee.FormatType = "Format1";
            $scope.Employee.Working = true;
            $scope.Employee.Left = true;
            $scope.Employee.AllOrIndividual = "All";
            $scope.Employee.DOBJL = "DOB";
            $scope.Employee.FromDate = new Date();
            $scope.Employee.ToDate = $scope.Employee.FromDate;

            $scope.headerselectedAll = true;
            $scope.GetEmployeeByheaderAll($scope.headerselectedAll);


            $scope.employeeDetails = [];
            $scope.submitted = false;
            //$scope.headerselectedAll = false;
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

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.onClickFormatOne = function () {
            
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.onClickFormatTwo = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }


        $scope.GetEmployeeListByWorkingSelection = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var headerselected = [];
            var groupTypeselected = [];
            var departmentselected = [];
            var designationselected = [];
            var employeeTypeselected = [];
            var data = {};

            $scope.Employee.headerselected = headerselected;
            $scope.Employee.groupTypeselected = groupTypeselected;
            $scope.Employee.departmentselected = departmentselected;
            $scope.Employee.designationselected = designationselected;
            $scope.Employee.employeeTypeselected = employeeTypeselected;

            $scope.Employee.FromDate = $filter('date')($scope.Employee.FromDate, "yyyy-MM-dd");
            $scope.Employee.ToDate = $filter('date')($scope.Employee.ToDate, "yyyy-MM-dd");

            data = $scope.Employee;

            apiService.create("EmployeeDetailsReport/FilterEmployeeData", data).
                then(function (promise) {


                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;
                    }



                    if (promise.employeeTypedropdown !== null && promise.employeeTypedropdown.length > 0) {
                        $scope.employeeTypedropdown = promise.employeeTypedropdown;

                    }

                })

        }

        $scope.GetEmployeeListByLeftSelection = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var headerselected = [];
            var groupTypeselected = [];
            var departmentselected = [];
            var designationselected = [];
            var employeeTypeselected = [];
            var data = {};

            $scope.Employee.headerselected = headerselected;
            $scope.Employee.groupTypeselected = groupTypeselected;
            $scope.Employee.departmentselected = departmentselected;
            $scope.Employee.designationselected = designationselected;
            $scope.Employee.employeeTypeselected = employeeTypeselected;

            $scope.Employee.FromDate = $filter('date')($scope.Employee.FromDate, "yyyy-MM-dd");
            $scope.Employee.ToDate = $filter('date')($scope.Employee.ToDate, "yyyy-MM-dd");

            data = $scope.Employee;

            apiService.create("EmployeeDetailsReport/FilterEmployeeData", data).
                then(function (promise) {


                    if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                        $scope.employeedropdown = promise.employeedropdown;
                    }

                    if (promise.employeeTypedropdown !== null && promise.employeeTypedropdown.length > 0) {
                        $scope.employeeTypedropdown = promise.employeeTypedropdown;

                    }

                })
        }


        $scope.GetAllOrIndividualEmployee = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.getEmployeeByDOBJL = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.OnchageToDate = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.exportToExcel = function (tableId) {
            //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var exportHref = Excel.tableToExcel(tableId, 'EmployeeDetails');
            $timeout(function () { location.href = exportHref; }, 100);
        }



        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //    var newWin = window.open();
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //    // $state.reload();
        //}


        // printclass

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